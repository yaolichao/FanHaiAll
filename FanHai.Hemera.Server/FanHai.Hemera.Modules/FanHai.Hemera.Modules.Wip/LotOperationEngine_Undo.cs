using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Modules.FMM;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Interface.EquipmentManagement;



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// 获取批次最后一笔操作记录。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次最后一笔操作记录的数据集对象。</returns>
        public DataSet GetLotLastestActivity(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT TOP 1 b.LOT_NUMBER,b.CREATE_OPERTION_NAME,b.FACTORYROOM_KEY,b.PALLET_NO,b.EDIT_TIME LOT_EDIT_TIME,
                                                    a.*
                                            FROM WIP_TRANSACTION a
                                            INNER JOIN POR_LOT b ON a.PIECE_KEY=b.LOT_KEY
                                            WHERE b.LOT_NUMBER='{0}'
                                            AND a.UNDO_FLAG=0
                                            AND a.ACTIVITY IN ('CREATELOT','TRACKIN','TRACKOUT',
                                                               'DEFECT','SCRAP','ADJUST',
                                                               'HOLD','RELEASE','REWORK','CHANGE_WO','CHANGE_PROID',
                                                               'PATCH','PATCHED','CELLSCRAP',
                                                               'CELLDEFECT','TERMINALLOT','RETURN_MATERIAL','TO_WAREHOUSE')
                                            ORDER BY TIME_STAMP DESC",
                                            lotNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 批次撤销操作。
        /// </summary>
        /// <param name="dsParams">包含撤销操作信息的数据集对象。</param>
        /// <returns>
        /// 包含结果数据的数据集对象。
        /// </returns>
        public DataSet LotUndo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)            //存放操作数据
                    )
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
                //检查是否存在重复的批次主键。
                var lnq = from item in dtTransaction.AsEnumerable()
                          group item by item[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复记录，请检查。");
                    return dsReturn;
                }
                //检查记录是否过期。防止重复修改。
                foreach (DataRow drTransaction in dtTransaction.Rows)
                {
                    string opEditTime = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME]);   //操作前批次编辑时间
                    string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);       //批次主键
                    string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);          //编辑人
                    string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
                    //检查记录是否过期。防止重复修改。
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                    if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "信息已过期，请关闭该界面后重试。");
                        return dsReturn;
                    }
                }
                StringBuilder sbMsg = new StringBuilder();
                //撤销批次操作。
                for (int i = 0; i < dtTransaction.Rows.Count;i++ )
                {
                    DataRow drTransaction = dtTransaction.Rows[i];
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        try
                        {
                            dbConn.Open();
                            DbTransaction dbTran = dbConn.BeginTransaction();
                            string activity = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                            if (activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT)
                            {
                                UndoCreateLot(dbTran, drTransaction);
                            }
                            else
                            {
                                UndoCommon(dbTran, drTransaction);
                            }
                            dbTran.Commit();
                        }
                        catch (Exception ex)
                        {
                            sbMsg.AppendFormat("失败：第{0}行处理失败 {1}；",i+1, ex.Message);
                        }
                    }
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotUndo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 撤销批次操作记录。
        /// </summary>
        /// <param name="dbTran">数据库操作事务对象。</param>
        /// <param name="drTransaction">批次的操作记录。</param>
        private void UndoCommon(DbTransaction dbTran, DataRow drTransaction)
        {
            if (drTransaction == null)
            {
                return;
            }
            string activity = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
            string transactionKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
            string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);
            string stepKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);
            string stepName = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME]);
            string equipmentKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);          //编辑人
            string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
            string sqlCommand = string.Empty;
            int stateFlag = Convert.ToInt32(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG]);
            //如果调整批次时，批次状态为已进站
            //则更新批次当前工步对应的EMS_LOT_EQUIPMENT的最后一笔记录的END_TIMESTAMP=NULL
            if ((stateFlag>0 && !string.IsNullOrEmpty(equipmentKey))
                &&
                (activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_ADJUST
                 || activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_REWORK))
            {
                sqlCommand = string.Format(@"UPDATE EMS_LOT_EQUIPMENT
                                             SET END_TIMESTAMP = NULL
                                             WHERE LOT_KEY = '{0}'
                                             AND STEP_KEY = '{1}'
                                             AND EQUIPMENT_KEY = '{2}'",
                                             lotKey.PreventSQLInjection(),
                                             stepKey.PreventSQLInjection(),
                                             equipmentKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            else if (activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE)
            {
                sqlCommand = string.Format(@"UPDATE WIP_HOLD_RELEASE
                                           SET IS_RELEASE=0,
                                               RELEASE_OPERATOR=NULL,
                                               RELEASE_TIME=NULL,
                                               RELEASE_TIMEZONE=NULL,
                                               RELEASE_DESCRIPTION=NULL,
                                               EDITOR='{1}',
                                               EDIT_TIME=GETDATE(),
                                               EDIT_TIMEZONE='{2}'
                                           WHERE RELEASE_TRANSACTION_KEY='{0}'",
                                           transactionKey.PreventSQLInjection(),
                                           editor.PreventSQLInjection(),
                                           timeZone.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            //执行撤销的存储过程。
            DbCommand cmd = db.GetStoredProcCommand("SP_UNDO_ACTIVITY");
            db.AddInParameter(cmd, "@p_lotKey",DbType.String,lotKey);
            db.AddInParameter(cmd, "@p_activity",DbType.String,activity);
            db.AddInParameter(cmd, "@p_stepName", DbType.String, stepName);
            db.AddInParameter(cmd, "@p_transactionKey",DbType.String,transactionKey);
            db.AddInParameter(cmd, "@p_editor",DbType.String,editor);
            db.AddInParameter(cmd, "@p_comment", DbType.String, drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
            db.ExecuteNonQuery(cmd, dbTran);
        }
        /// <summary>
        /// 撤销创建批次的操作。
        /// </summary>
        /// <param name="dbTran">数据库操作事务对象。</param>
        /// <param name="drTransaction">批次的操作记录。</param>
        private void UndoCreateLot(DbTransaction dbTran, DataRow drTransaction)
        {
            if (drTransaction == null)
            {
                return;
            }
            string activity = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
            if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT)
            {
                return;
            }
            string transactionKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
            string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);
            string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            //查询工单主键，批次数量，物料批号，车间主键,工序名称。
            string sqlCommand = string.Format(@"SELECT a.WORK_ORDER_KEY,a.QUANTITY,a.MATERIAL_CODE,a.MATERIAL_LOT,a.FACTORYROOM_KEY,
                                                    a.CUR_STEP_VER_KEY,a.CREATE_OPERTION_NAME,a.LOT_TYPE
                                               FROM POR_LOT a
                                               WHERE a.LOT_KEY='{0}'",
                                               lotKey.PreventSQLInjection());
            DataSet ds = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count < 0)
            {
                return;
            }
            double quantity = Convert.ToDouble(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);
            string workOrderKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string materialLot = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_LOT]);
            string materialCode = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_CODE]);
            string factoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            string createOperationName = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]);
            string lotType = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_TYPE]);
            //非组件补片批次，更新工单剩余数量
            if (lotType != "L")
            {
                //更新工单剩余数量+1。
                sqlCommand = string.Format(@"UPDATE POR_WORK_ORDER
                                            SET QUANTITY_LEFT = QUANTITY_LEFT + 1
                                            WHERE WORK_ORDER_KEY = '{0}'",
                                            workOrderKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            string storeKey = string.Empty;
            //根据车间主键+工序+仓库类型获取线上仓主键。
            sqlCommand = string.Format(@"SELECT STORE_KEY,STORE_NAME
                                        FROM WST_STORE 
                                        WHERE STORE_TYPE='9' AND LOCATION_KEY='{0}' AND OPERATION_NAME='{1}'",
                                        factoryRoomKey.PreventSQLInjection(),
                                        createOperationName.PreventSQLInjection());
            ds = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
            }
            string storeMaterialDetailKey = string.Empty;
            //根据线上仓主键 + 物料批号键获取线上仓物料明细主键。
            sqlCommand = string.Format(@"SELECT b.STORE_MATERIAL_DETAIL_KEY
                                        FROM WST_STORE_MATERIAL a
                                        LEFT JOIN POR_MATERIAL c ON a.MATERIAL_KEY=c.MATERIAL_KEY
                                        LEFT JOIN WST_STORE_MATERIAL_DETAIL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
                                        WHERE a.STORE_KEY='{0}' AND b.MATERIAL_LOT='{1}' AND c.MATERIAL_CODE='{2}'",
                                        storeKey.PreventSQLInjection(),
                                        materialLot.PreventSQLInjection(),
                                        materialCode.PreventSQLInjection());
            ds = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                storeMaterialDetailKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"]);
            }
            //更新线上仓+物料批次数量。
            sqlCommand = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                         SET CURRENT_QTY=CURRENT_QTY+{0}
                                         WHERE STORE_MATERIAL_DETAIL_KEY='{1}'",
                                         quantity, 
                                         storeMaterialDetailKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            //更新创建批次的操作记录。
            string undoTransactionKey = UtilHelper.GenerateNewKey(0);
            //记录批次信息
            AddWIPLot(dbTran, undoTransactionKey, lotKey);
            //删除批次。
            sqlCommand = string.Format(@"UPDATE POR_LOT SET STATUS=2,DELETED_TERM_FLAG=2,LOT_NUMBER='^'+LOT_NUMBER+'^' WHERE LOT_KEY='{0}'",
                                       lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            //更新操作交易记录的标记位。
            string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
            string sql = string.Format(@"UPDATE WIP_TRANSACTION 
                                         SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}'
                                         WHERE TRANSACTION_KEY = '{1}'",
                                         undoTransactionKey.PreventSQLInjection(),
                                         transactionKey.PreventSQLInjection(),
                                         editor.PreventSQLInjection(),
                                         timeZone.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //插入撤销创建批次的操作记录。
            AddUndoTransaction(dbTran, undoTransactionKey, drTransaction);
        }
        /// <summary>
        /// 添加撤销某笔历史操作记录的操作记录。
        /// </summary>
        /// <param name="dbtran">数据库事务操作对象。</param>
        /// <param name="transactionKey">撤销操作的主键。</param>
        private void AddUndoTransaction(DbTransaction dbtran,string transactionKey, DataRow drTransaction)
        {
            object activity = string.Format("{0}${1}", ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_UNDO,
                                                       drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
            Hashtable htTransaction = new Hashtable();
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, activity);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
        }

        /// <summary>
        /// 新增批次操作前记录。
        /// </summary>
        /// <param name="dbtran">数据库操作事务。</param>
        /// <param name="transactionKey">操作交易记录。</param>
        /// <param name="lotKey">批次主键。</param>
        private void AddWIPLot(DbTransaction dbtran, string transactionKey, string lotKey)
        {
            string sqlCommand = string.Format(@"INSERT INTO WIP_LOT(TRANSACTION_KEY, LOT_KEY, LOT_NUMBER, WORK_ORDER_KEY, WORK_ORDER_NO, WORK_ORDER_SEQ, PART_VER_KEY, PART_NUMBER, 
                                                      PRO_ID, PRIORITY, QUANTITY_INITIAL, QUANTITY, ROUTE_ENTERPRISE_VER_KEY, CUR_ROUTE_VER_KEY, CUR_STEP_VER_KEY, CUR_PRODUCTION_LINE_KEY, 
                                                      LINE_NAME, START_WAIT_TIME, START_PROCESS_TIME, EDC_INS_KEY, STATE_FLAG, IS_MAIN_LOT, SPLIT_FLAG, LOT_SEQ, REWORK_FLAG, HOLD_FLAG, 
                                                      SHIPPED_FLAG, DELETED_TERM_FLAG, IS_PRINT, LOT_TYPE, CREATE_TYPE,COLOR, STATUS, OPERATOR, OPR_LINE, OPR_COMPUTER, OPR_LINE_PRE, CHILD_LINE, 
                                                      MATERIAL_CODE, MATERIAL_LOT, SUPPLIER_NAME, SI_LOT, EFFICIENCY, FACTORYROOM_KEY, FACTORYROOM_NAME, CREATE_OPERTION_NAME, SHIFT_NAME,
                                                      DESCRIPTIONS, CREATOR, CREATE_TIME, CREATE_TIMEZONE_KEY, EDITOR, EDIT_TIME, EDIT_TIMEZONE, PRO_LEVEL,PALLET_NO,PALLET_TIME,LOT_SIDECODE,
                                                      LOT_CUSTOMERCODE,LOT_LINE_KEY,LOT_LINE_CODE,LAMINATING_MACHINE,WELDING_TRACKOUT_TIME)
                                                SELECT  '{1}',LOT_KEY, LOT_NUMBER, WORK_ORDER_KEY, WORK_ORDER_NO, WORK_ORDER_SEQ, PART_VER_KEY, PART_NUMBER, 
                                                      PRO_ID, PRIORITY, QUANTITY_INITIAL, QUANTITY, ROUTE_ENTERPRISE_VER_KEY, CUR_ROUTE_VER_KEY, CUR_STEP_VER_KEY, CUR_PRODUCTION_LINE_KEY, 
                                                      LINE_NAME, START_WAIT_TIME, START_PROCESS_TIME, EDC_INS_KEY, STATE_FLAG, IS_MAIN_LOT, SPLIT_FLAG, LOT_SEQ, REWORK_FLAG, HOLD_FLAG, 
                                                      SHIPPED_FLAG, DELETED_TERM_FLAG, IS_PRINT, LOT_TYPE, CREATE_TYPE,COLOR,STATUS, OPERATOR, OPR_LINE, OPR_COMPUTER, OPR_LINE_PRE, CHILD_LINE, 
                                                      MATERIAL_CODE, MATERIAL_LOT, SUPPLIER_NAME, SI_LOT, EFFICIENCY, FACTORYROOM_KEY, FACTORYROOM_NAME, CREATE_OPERTION_NAME, SHIFT_NAME,
                                                      DESCRIPTIONS, CREATOR, CREATE_TIME, CREATE_TIMEZONE_KEY, EDITOR, EDIT_TIME, EDIT_TIMEZONE, PRO_LEVEL,PALLET_NO,PALLET_TIME,LOT_SIDECODE,
                                                      LOT_CUSTOMERCODE,LOT_LINE_KEY,LOT_LINE_CODE,LAMINATING_MACHINE,WELDING_TRACKOUT_TIME
                                                FROM  POR_LOT
                                                WHERE LOT_KEY='{0}'",
                                       lotKey.PreventSQLInjection(),
                                       transactionKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
        }
    }
}
