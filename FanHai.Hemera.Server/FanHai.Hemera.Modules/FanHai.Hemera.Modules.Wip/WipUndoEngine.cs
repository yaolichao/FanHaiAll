//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-09-13            重构 迁移到SQL Server数据库
// =================================================================================
#region using
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Share.Constants;

using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Utils.DatabaseHelper;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SolarViewer.Hemera.Share.Common;
#endregion

namespace SolarViewer.Hemera.Modules.Wip
{
    /// <summary>
    /// 在制品操作撤销类。
    /// </summary>
    public class WipUndoEngine : AbstractEngine, IWipUndoEngine
    {
        /// <summary>
        /// 数据库对象。
        /// </summary>
        private Database db;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WipUndoEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化函数。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 获取批次的历史操作记录。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次及其子批次的历史操作记录的数据集对象。</returns>
        public DataSet GetLotHistoryAction(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT b.LOT_NUMBER,b.CREATE_OPERTION_NAME,b.FACTORYROOM_KEY,b.MATERIAL_CODE,b.MATERIAL_LOT,
                                                    a.*
                                            FROM WIP_TRANSACTION a
                                            INNER JOIN POR_LOT b ON a.PIECE_KEY=b.LOT_KEY
                                            WHERE a.TIME_STAMP=(SELECT MAX(TIME_STAMP) 
                                                                FROM WIP_TRANSACTION
                                                                WHERE PIECE_KEY=a.PIECE_KEY)
                                            AND b.LOT_NUMBER='{0}'
                                            AND a.UNDO_FLAG=0",
                                            lotNumber.PreventSQLInjection());
                dsReturn=db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 撤销创建批次的操作。。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true：撤销进站成功。false：撤销进站失败。</returns>
        public bool UndoCreateMainLot(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                int quantity = 0;
                string workOrderKey = string.Empty;
                int reworkFlag = 0;
                string materialLot = string.Empty;
                string factoryRoomKey = string.Empty;
                string operationName = string.Empty;
                string createOperationName = string.Empty;
                //查询工单主键，批次数量，重工标记，物料批号，车间主键，工步主键,工序名称。
                sqlCommand =string.Format(@"SELECT a.WORK_ORDER_KEY,a.QUANTITY,a.REWORK_FLAG,a.MATERIAL_LOT,a.FACTORYROOM_KEY,
                                                  a.CUR_STEP_VER_KEY,b.ROUTE_OPERATION_NAME,a.CREATE_OPERTION_NAME
                                           FROM POR_LOT a
                                           LEFT JOIN POR_ROUTE_STEP b ON a.CUR_STEP_VER_KEY=b.ROUTE_STEP_KEY
                                           WHERE a.LOT_KEY='{0}'",lotKey.PreventSQLInjection());
                DataSet ds = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["QUANTITY"]);
                    workOrderKey = Convert.ToString(ds.Tables[0].Rows[0]["WORK_ORDER_KEY"]);
                    reworkFlag = Convert.ToInt32(ds.Tables[0].Rows[0]["REWORK_FLAG"]);
                    materialLot = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_LOT"]);
                    factoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0]["FACTORYROOM_KEY"]);
                    operationName = Convert.ToString(ds.Tables[0].Rows[0]["ROUTE_OPERATION_NAME"]);
                    createOperationName = Convert.ToString(ds.Tables[0].Rows[0]["CREATE_OPERTION_NAME"]);
                }
                else
                {
                    LogService.LogError("UndoCreateMainLot ERROR: 批次不存在。");
                    return false;
                }
                //更新批次为已删除。
                sqlCommand = string.Format(@"UPDATE POR_LOT
                                             SET DELETED_TERM_FLAG=2,EDITOR='{1}',EDIT_TIME=GETDATE()
                                             WHERE LOT_KEY='{0}'", 
                                             lotKey.PreventSQLInjection(), 
                                             editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                //如果是正常批次。
                if (reworkFlag == 0)
                {
                    //更新工单剩余数量。
                    sqlCommand = string.Format(@"UPDATE POR_WORK_ORDER
                                             SET QUANTITY_LEFT = QUANTITY_LEFT + {0}
                                             WHERE WORK_ORDER_KEY = '{1}'", 
                                             quantity, workOrderKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    string storeKey = string.Empty;
                    //根据车间主键+工序+仓库类型获取线上仓主键。
                    sqlCommand = string.Format(@"SELECT STORE_KEY,STORE_NAME
                                                FROM WST_STORE 
                                                WHERE STORE_TYPE='9' AND LOCATION_KEY='{0}' AND OPERATION_NAME='{1}'",
                                                factoryRoomKey, createOperationName);
                    ds = db.ExecuteDataSet(dbTrans, CommandType.Text, sqlCommand);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
                    }
                    string storeMaterialDetailKey = string.Empty;
                    //根据线上仓主键 + 物料批号键获取线上仓物料明细主键。
                    sqlCommand = string.Format(@"SELECT b.STORE_MATERIAL_DETAIL_KEY
                                                FROM WST_STORE_MATERIAL a
                                                LEFT JOIN WST_STORE_MATERIAL_DETAIL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
                                                WHERE a.STORE_KEY='{0}' AND b.MATERIAL_LOT='{1}'", storeKey, materialLot);
                    ds = db.ExecuteDataSet(dbTrans, CommandType.Text, sqlCommand);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        storeMaterialDetailKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"]);
                    }
                    //更新线上仓+物料批次数量。
                    sqlCommand = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                                 SET CURRENT_QTY=CURRENT_QTY+{0}
                                                 WHERE STORE_MATERIAL_DETAIL_KEY='{1}'", 
                                                 quantity, storeMaterialDetailKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                }
                else if (reworkFlag == 1)
                {
                    string storeKey = string.Empty;
                    //获取重工线上仓主键。
                    sqlCommand = string.Format(@"SELECT K.STORE_KEY 
                                                FROM WST_STORE K 
                                                WHERE K.STORE_TYPE = 0 
                                                AND K.LOCATION_KEY =  '{0}' 
                                                AND K.OPERATION_NAME =  '{1}'",
                                                factoryRoomKey.PreventSQLInjection(), 
                                                createOperationName.PreventSQLInjection());
                    ds = db.ExecuteDataSet(dbTrans, CommandType.Text, sqlCommand);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
                    }
                    //更新工单返工数量。
                    sqlCommand = string.Format(@"UPDATE POR_WORK_ORDER
                                             SET QUANTITY_REWORK =ISNULL(W.QUANTITY_REWORK,0) + {0}
                                             WHERE WORK_ORDER_KEY = '{1}'", 
                                             quantity, workOrderKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    //更新重工线上仓数据。
                    sqlCommand = string.Format(@"UPDATE t 
                                                 SET t.SUM_QUANTITY=t.SUM_QUANTITY+{0}
                                                 FROM WST_STORE_SUM t 
                                                 WHERE t.WORKORDER_NUMBER=(SELECT P.ORDER_NUMBER FROM POR_WORK_ORDER P WHERE P.WORK_ORDER_KEY='{1}')
                                                 AND t.STORE_KEY = '{2}'",
                                                 quantity, workOrderKey.PreventSQLInjection(), storeKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                }
                //更新创建批次的操作记录。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                             SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                             WHERE TRANSACTION_KEY = '{1}'", 
                                             undoTransactionKey.PreventSQLInjection(), 
                                             transactionKey.PreventSQLInjection(),
                                             editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //插入撤销创建批次的操作记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoCreateMainLot ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbConn.Close();
            }
            return true;
        }
        /// <summary>
        /// 撤销批次进站。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="transactionKey">交易主键。</param>
        /// <param name="pretransactionKey">上一个交易主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true：撤销进站成功。false：撤销进站失败。</returns>
        public bool UndoTrackInLot(string lotKey, string stepKey, string transactionKey, string pretransactionKey,string editor)
        {
            string sqlCommand = string.Empty;
            List<string> sqlCommandList = new List<string>();
            int stateFlag = 0; //(WaitingForTrackInEDC）
            //根据上一交易主键查询交易记录。
            sqlCommand =string.Format(@"SELECT ACTIVITY,STATE_FLAG,EDITOR,EDIT_TIME,EDIT_TIMEZONE_KEY 
                                       FROM WIP_TRANSACTION 
                                       WHERE TRANSACTION_KEY='{0}'",
                                       pretransactionKey.PreventSQLInjection());
            DataTable dataTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            //上一交易为INEDC
            if (dataTable.Rows[0][WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY].ToString() == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC)
            {
                stateFlag = 1;//(WaitingForTrackIn）
            }
            //更新批次信息。
            sqlCommand = string.Format(@"UPDATE POR_LOT
                                         SET STATE_FLAG='{0}',
                                             EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                             EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                             EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                             OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}')
                                         WHERE LOT_KEY = '{2}'", 
                                         stateFlag,
                                         pretransactionKey.PreventSQLInjection(), 
                                         lotKey.PreventSQLInjection());
            sqlCommandList.Add(sqlCommand);
            //更新批次设备信息。
            sqlCommand = string.Format(@"UPDATE EMS_LOT_EQUIPMENT
                                         SET END_TIMESTAMP = GETDATE(),QUANTITY=0
                                         WHERE LOT_EQUIPMENT_KEY IN (SELECT TOP 1 LOT_EQUIPMENT_KEY
                                                                     FROM EMS_LOT_EQUIPMENT
                                                                     WHERE LOT_KEY = '{0}'
                                                                     AND STEP_KEY = '{1}'
                                                                     ORDER BY START_TIMESTAMP DESC)", 
                                         lotKey.PreventSQLInjection(), 
                                         stepKey.PreventSQLInjection());
            sqlCommandList.Add(sqlCommand);
            string undoTransactionKey = UtilHelper.GenerateNewKey(0);
            //更新交易记录为撤销。
            sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                         SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                         WHERE TRANSACTION_KEY = '{1}'", 
                                         undoTransactionKey.PreventSQLInjection(), 
                                         transactionKey.PreventSQLInjection(), 
                                         editor.PreventSQLInjection());
            sqlCommandList.Add(sqlCommand);

            //更新WIP_JOB信息。自动出站JOB
            sqlCommand = string.Format(@"UPDATE WIP_JOB 
                                        SET JOB_STATUS=1,JOB_CLOSETYPE='UndoTrackIn'
                                        WHERE LOT_KEY='{0}' AND STEP_KEY='{1}'", 
                                        lotKey.PreventSQLInjection(), 
                                        stepKey.PreventSQLInjection());
            sqlCommandList.Add(sqlCommand);

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //执行SQL。
                foreach (string sql in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoTrackInLot ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销出站前数据采集。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="transactionKey">操作记录主键。</param>
        /// <param name="pretransactionKey">上一个操作记录主键。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoOutEDC(string lotKey, string transactionKey, string pretransactionKey, string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //更新批次信息。设置批次状态为上一状态。
                string sql = string.Format(@"UPDATE POR_LOT
                                             SET STATE_FLAG=4, EDC_INS_KEY='',
                                                 EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                 EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                 EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                 OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                             WHERE LOT_KEY = '{1}'", 
                                             pretransactionKey.PreventSQLInjection(), 
                                             lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                //更新批次采集的明细数据为无效数据。
                sql = string.Format(@"UPDATE T  
                                      SET T.DELETED_FLAG = 1,T.EDITOR='{1}',T.EDIT_TIME=GETDATE()
                                      FROM EDC_COLLECTION_DATA T 
                                      WHERE T.EDC_INS_KEY = (SELECT W.EDC_INS_KEY
                                                             FROM WIP_TRANSACTION W
                                                             WHERE W.TRANSACTION_KEY = '{0}')", 
                                      transactionKey.PreventSQLInjection(), editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //更新批次采集的数据为无效数据。
                sql = string.Format(@"UPDATE M
                                      SET M.DELETED_FLAG = 1  
                                      FROM EDC_MAIN_INS M
                                      WHERE M.EDC_INS_KEY = (SELECT W.EDC_INS_KEY
                                                              FROM WIP_TRANSACTION W
                                                              WHERE W.TRANSACTION_KEY = '{0}') ", 
                                      transactionKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //更新SPC数据为无效数据
                sql = string.Format(@"UPDATE T  
                                      SET T.DELETED_FLAG = 1,EDITOR='{1}',EDIT_TIME=GETDATE()
                                      FROM SPC_PARAM_DATA T
                                      WHERE T.EDC_INS_KEY = (SELECT W.EDC_INS_KEY
                                                             FROM WIP_TRANSACTION W
                                                             WHERE W.TRANSACTION_KEY = '{0}')", 
                                      transactionKey.PreventSQLInjection(), 
                                      editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                //更新操作交易记录为撤销。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sql = string.Format(@"UPDATE WIP_TRANSACTION 
                                      SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                      WHERE TRANSACTION_KEY = '{1}'", 
                                      undoTransactionKey.PreventSQLInjection(), 
                                      transactionKey.PreventSQLInjection(), 
                                      editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //新增撤销操作的操作记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoOutEDC ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销批次出站作业。
        /// </summary>
        /// <param name="transactionKey">批次出站操作的主键。</param>
        /// <param name="pretransactionKey">批次出站上一个操作的主键</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="editor">撤销人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoTrackOut(string transactionKey, string pretransactionKey, string lotKey, string editor, string stepKey)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                //更新批次的工艺流程信息。
                sql = string.Format(@"UPDATE POR_LOT 
                                    SET	ROUTE_ENTERPRISE_VER_KEY=(SELECT ENTERPRISE_KEY FROM WIP_STEP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                        CUR_ROUTE_VER_KEY=(SELECT ROUTE_KEY FROM WIP_STEP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                        CUR_STEP_VER_KEY=(SELECT STEP_KEY FROM WIP_STEP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                    WHERE LOT_KEY='{1}'", 
                                    transactionKey.PreventSQLInjection(), 
                                    lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //更新批次信息。
                sql = string.Format(@"UPDATE POR_LOT
                                    SET STATE_FLAG=9,
                                        EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                        EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                        EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                        OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                    WHERE LOT_KEY='{1}'", 
                                    pretransactionKey.PreventSQLInjection(), 
                                    lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //更新出站操作交易记录的标记位。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sql = string.Format(@"UPDATE WIP_TRANSACTION 
                                     SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                     WHERE TRANSACTION_KEY = '{1}'", 
                                     undoTransactionKey.PreventSQLInjection(), 
                                     transactionKey.PreventSQLInjection(), 
                                     editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //插入撤销操作记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT, dbTrans);

                //查询设备主键。
                sql =string.Format("SELECT EQUIPMENT_KEY FROM WIP_TRANSACTION WHERE TRANSACTION_KEY='{0}'",
                                    transactionKey.PreventSQLInjection());
                string equipmentKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    //更新EMS_LOT_EQUIPMENT 表
                    sql = string.Format(@"UPDATE A
                                         SET A.END_TIMESTAMP = NULL
                                         FROM  EMS_LOT_EQUIPMENT A
                                         WHERE A.LOT_EQUIPMENT_KEY = (SELECT TOP 1 LOT_EQUIPMENT_KEY
                                                                    FROM EMS_LOT_EQUIPMENT
                                                                    WHERE LOT_KEY = '{0}'
                                                                    AND STEP_KEY = '{1}'
                                                                    AND EQUIPMENT_KEY = '{2}'
                                                                    ORDER BY START_TIMESTAMP DESC)",
                                         lotKey.PreventSQLInjection(), 
                                         stepKey.PreventSQLInjection(), 
                                         equipmentKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                //更新WIP_JOB信息。自动出站JOB
                sql = string.Format(@"UPDATE WIP_JOB SET JOB_STATUS=0 
                                      WHERE LOT_KEY='{0}' AND STEP_KEY='{1}'",
                                      lotKey.PreventSQLInjection(), stepKey.PreventSQLInjection());
                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    sql += string.Format("  AND EQUIPMENT_KEY='{0}'", equipmentKey.PreventSQLInjection());
                }
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoTrackOut ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbConn.Close();
            }
            return true;
        }
        /// <summary>
        /// 撤销拆分批次操作。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="pretransactionKey">上一个操作主键。</param> 
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoSplitLot(string transactionKey, string pretransactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;
            List<string> sqlCommandList = new List<string>();

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //获取当前撤销拆分批次的批次是否存在子批次。
                sqlCommand = string.Format(@"SELECT S.TRANSACTION_KEY
                                             FROM WIP_TRANSACTION S
                                             WHERE S.TRANSACTION_KEY IN
                                                   (SELECT B.CHILD_LOT_TRANSACTION_KEY
                                                    FROM WIP_SPLIT B, WIP_TRANSACTION A
                                                    WHERE A.TRANSACTION_KEY = B.TRANSACTION_KEY
                                                    AND A.TRANSACTION_KEY ='{0}') 
                                              AND S.UNDO_FLAG = 0", transactionKey.PreventSQLInjection());

                DataSet ds = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                //被撤销的批次存在子批次，不允许撤销。
                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("${res:SolarViewer.Hemera.Modules.FMM.Lot.ExistChildLot}");
                }
                else
                {
                    string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                    //更新撤销批次记录。
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                 WHERE TRANSACTION_KEY = '{1}'", 
                                                 undoTransactionKey.PreventSQLInjection(), 
                                                 transactionKey.PreventSQLInjection(), 
                                                 editor.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SPLIT, dbTrans);
                    //更新批次信息。
                    sqlCommand = string.Format(@"UPDATE POR_LOT
                                                 SET EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                     EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                     EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                     OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                                 WHERE LOT_KEY = '{1}'", 
                                                 pretransactionKey.PreventSQLInjection(), 
                                                 lotKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);

                    foreach (string sql in sqlCommandList)
                    {
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                    }
                    dbTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoSplitLot ERROR:" + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销创建子批的操作记录。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoCreateLot(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;
            List<string> sqlCommandList = new List<string>();

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();

            try
            {
                int childLotQuantity = 0;
                //获取子批次相关的设备信息。
                sqlCommand = string.Format(@"SELECT EL.LOT_EQUIPMENT_KEY, WS.SPLIT_QUANTITY
                                             FROM EMS_LOT_EQUIPMENT EL, WIP_SPLIT WS
                                             WHERE EL.LOT_KEY = WS.CHILD_LOT_KEY
                                             AND EL.STEP_KEY = WS.STEP_KEY
                                             AND WS.CHILD_LOT_TRANSACTION_KEY = '{0}'
                                             AND EL.END_TIMESTAMP IS NULL", transactionKey.PreventSQLInjection());
                DataTable dataTable = db.ExecuteDataSet(dbTrans,CommandType.Text, sqlCommand).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    childLotQuantity = Convert.ToInt32(dataTable.Rows[0]["SPLIT_QUANTITY"]);
                    string childLotEquipmentKey = dataTable.Rows[0]["LOT_EQUIPMENT_KEY"].ToString();
                    //更新子批次的设备信息。
                    sqlCommand = string.Format(@"UPDATE EMS_LOT_EQUIPMENT
                                                 SET END_TIMESTAMP =  GETDATE(),QUANTITY=0
                                                 WHERE LOT_EQUIPMENT_KEY = '{0}'", childLotEquipmentKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    //获取和主批次相关的设备信息主键。
                    sqlCommand = string.Format(@"SELECT EL.LOT_EQUIPMENT_KEY
                                                 FROM EMS_LOT_EQUIPMENT EL,
                                                        (SELECT WT.PIECE_KEY, WT.STEP_KEY
                                                         FROM WIP_TRANSACTION WT, WIP_SPLIT WS
                                                         WHERE WT.TRANSACTION_KEY = WS.TRANSACTION_KEY
                                                         AND WS.CHILD_LOT_TRANSACTION_KEY = '{0}') WT
                                                  WHERE EL.LOT_KEY = WT.PIECE_KEY
                                                  AND EL.STEP_KEY = WT.STEP_KEY
                                                  AND EL.END_TIMESTAMP IS NULL", transactionKey.PreventSQLInjection());
                    string parentLotEquipmentKey =Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlCommand));

                    if (!string.IsNullOrEmpty(parentLotEquipmentKey))
                    {
                        //更新设备主批次的数量。
                        sqlCommand = string.Format(@"UPDATE EMS_LOT_EQUIPMENT
                                                     SET QUANTITY = QUANTITY + {0}
                                                     WHERE LOT_EQUIPMENT_KEY = '{1}'",
                                                     childLotQuantity, parentLotEquipmentKey.PreventSQLInjection());
                        sqlCommandList.Add(sqlCommand);
                    }
                }
                //获取子批次的数量。
                sqlCommand =string.Format(@"SELECT QUANTITY FROM POR_LOT WHERE LOT_KEY='{0}'",lotKey.PreventSQLInjection());
                int childQuantity = Convert.ToInt32(db.ExecuteScalar(dbTrans, CommandType.Text, sqlCommand));
                //撤销子批次创建。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                            SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                            WHERE TRANSACTION_KEY = '{1}'", 
                                            undoTransactionKey.PreventSQLInjection(), 
                                            transactionKey.PreventSQLInjection(), 
                                            editor.PreventSQLInjection());
                sqlCommandList.Add(sqlCommand);

                //插入撤销操作。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT, dbTrans);

                //更新批次信息。
                sqlCommand = string.Format(@"UPDATE POR_LOT SET QUANTITY=0,DELETED_TERM_FLAG=1 WHERE LOT_KEY='{0}'", 
                                            lotKey.PreventSQLInjection());
                sqlCommandList.Add(sqlCommand);

                //更新主批次信息。
                sqlCommand = string.Format(@"UPDATE POR_LOT
                                             SET QUANTITY = QUANTITY + {0},DELETED_TERM_FLAG=0
                                             WHERE LOT_KEY IN (SELECT WT.PIECE_KEY
                                                               FROM WIP_TRANSACTION WT, WIP_SPLIT WS
                                                               WHERE WT.TRANSACTION_KEY = WS.TRANSACTION_KEY
                                                               AND WS.CHILD_LOT_TRANSACTION_KEY = '{1}')", 
                                             childQuantity, transactionKey.PreventSQLInjection());
                sqlCommandList.Add(sqlCommand);

                foreach (string sql in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoCreateLot ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销合并批次操作。
        /// </summary>
        /// <param name="transactionKey">合并批次的操作主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet UndoMerge(string transactionKey, string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = string.Empty;
            bool undoMergeFinished = true;//if all child lot execute undo succeed
            DataSet dsReturn = new DataSet();
            try
            {
                //获取被合并的批次信息。
                sqlCommand = string.Format(@"SELECT A.TRANSACTION_KEY, A.CHILD_LOT_KEY, A.CHILD_TRANSACTION_KEY, A.MERGE_QUANTITY, A.MAIN_LOT_KEY
                                             FROM WIP_MERGE A
                                             WHERE A.TRANSACTION_KEY ='{0}'
                                             AND A.CHILD_TRANSACTION_KEY IN (SELECT C.TRANSACTION_KEY
                                                                             FROM WIP_TRANSACTION C
                                                                             WHERE C.TRANSACTION_KEY IN (SELECT B.CHILD_TRANSACTION_KEY
                                                                                                          FROM WIP_MERGE B
                                                                                                          WHERE B.TRANSACTION_KEY ='{0}')
                                                                              AND C.UNDO_FLAG = 0)",
                                              transactionKey.PreventSQLInjection());
                DataSet dsMerge = db.ExecuteDataSet(dbTrans,CommandType.Text, sqlCommand);
                //存在被合并的操作记录。
                if (null != dsMerge && dsMerge.Tables.Count > 0 && dsMerge.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dsMerge.Tables[0].Rows)
                    {
                        //select reworked quantity of child lot
                        sqlCommand =string.Format("SELECT QUANTITY_OUT FROM WIP_TRANSACTION WHERE TRANSACTION_KEY='{0}'",
                                                   Convert.ToString(dataRow[WIP_MERGE_FIELDS.FIELD_CHILD_TRANSACTION_KEY]).PreventSQLInjection());
                        int quantityOut = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                        if (quantityOut > 0) //不能撤销被合批的记录。
                        {
                            undoMergeFinished = false;
                            continue;
                        }
                        else
                        {
                            //更新被合批数量。
                            sqlCommand = string.Format(@"UPDATE POR_LOT SET QUANTITY={0},
                                                         DELETED_TERM_FLAG=0 WHERE LOT_KEY='{1}'",
                                                         Convert.ToString(dataRow[WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY]).PreventSQLInjection(),
                                                         Convert.ToString(dataRow[WIP_MERGE_FIELDS.FIELD_CHILD_LOT_KEY]).PreventSQLInjection());
                            sqlCommandList.Add(sqlCommand);
                            //更新合批数量。
                            sqlCommand = string.Format(@"UPDATE POR_LOT SET QUANTITY=QUANTITY-{0} WHERE LOT_KEY='{1}'",
                                                         Convert.ToString(dataRow[WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY]).PreventSQLInjection(),
                                                         Convert.ToString(dataRow[WIP_MERGE_FIELDS.FIELD_MAIN_LOT_KEY]).PreventSQLInjection());
                            sqlCommandList.Add(sqlCommand);
                            //更新被合批操作记录。
                            string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                            sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                                        SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                        WHERE TRANSACTION_KEY = '{1}'", 
                                                        undoTransactionKey.PreventSQLInjection(), 
                                                        Convert.ToString(dataRow[WIP_MERGE_FIELDS.FIELD_CHILD_TRANSACTION_KEY]).PreventSQLInjection(), 
                                                        editor.PreventSQLInjection());
                            sqlCommandList.Add(sqlCommand);
                            //添加撤销被合批操作记录。
                            AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGE, dbTrans);
                        }
                    }
                }
                if (undoMergeFinished)
                {
                    //更新主批的合批交易记录。
                    string undoKey = UtilHelper.GenerateNewKey(0);
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                                SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                WHERE TRANSACTION_KEY = '{1}'",
                                                undoKey.PreventSQLInjection(),
                                                transactionKey.PreventSQLInjection(),
                                                editor.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    //添加撤销操作。
                    AddUndoTransaction(undoKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGE, dbTrans);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                else
                {
                    //撤销部分被合批记录成功。
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.FMM.Lot.PartUndoMergeSucceed}");
                }
                foreach (string sql in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoMerge ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return dsReturn;
        }
        /// <summary>
        /// 撤销被合并批次操作。
        /// </summary>
        /// <param name="transactionKey">被合批的操作主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoMerged(string transactionKey, string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            string sqlCommand = string.Empty;
            try
            {
                //查询被合批记录。
                sqlCommand =string.Format(@"SELECT QUANTITY_OUT FROM WIP_TRANSACTION WHERE TRANSACTION_KEY='{0}'",
                                            transactionKey.PreventSQLInjection());
                int quantityOut = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                //if the quantity of out big then zero,it's means the child lot is not over,               
                //so can't execute the action of undo
                if (quantityOut > 0)//不能撤销被合批操作。
                {
                    throw new Exception("${res:SolarViewer.Hemera.Modules.FMM.Lot.CanNotUndo}");
                }
                //获取主批次信息。
                sqlCommand = string.Format(@"SELECT TRANSACTION_KEY,MAIN_LOT_KEY,CHILD_LOT_KEY,MERGE_QUANTITY 
                                             FROM WIP_MERGE WHERE CHILD_TRANSACTION_KEY='{0}'", 
                                            transactionKey.PreventSQLInjection());
                DataSet ds = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //更新主批数量。
                    sqlCommand = string.Format(@"UPDATE POR_LOT SET QUANTITY=QUANTITY-{0} WHERE LOT_KEY='{1}'",
                                                 Convert.ToString(ds.Tables[0].Rows[0][WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY]).PreventSQLInjection(),
                                                 Convert.ToString(ds.Tables[0].Rows[0][WIP_MERGE_FIELDS.FIELD_MAIN_LOT_KEY]).PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    //更新被合批的数量。
                    sqlCommand = string.Format(@"UPDATE POR_LOT SET QUANTITY={0},DELETED_TERM_FLAG=0 WHERE LOT_KEY='{1}'",
                                                 ds.Tables[0].Rows[0][WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY],
                                                 ds.Tables[0].Rows[0][WIP_MERGE_FIELDS.FIELD_CHILD_LOT_KEY]);
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    //更新操作为撤销，并添加撤销记录。
                    string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                                SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                WHERE TRANSACTION_KEY = '{1}'", 
                                                undoTransactionKey.PreventSQLInjection(), 
                                                transactionKey.PreventSQLInjection(), 
                                                editor.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGED, dbTrans);
                    dbTrans.Commit();
                }
                else
                {
                    throw new Exception("${res:SolarViewer.Hemera.Modules.FMM.Lot.CanNotFindMainLot}");
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoMerged ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销暂停批次的操作。
        /// </summary>
        /// <param name="transactionKey">暂停批次操作的主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoHoldLot(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //更新批次主键。
                sqlCommand = string.Format(@"UPDATE POR_LOT
                                             SET HOLD_FLAG = 0, 
                                                EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                             WHERE LOT_KEY='{1}'", 
                                             transactionKey.PreventSQLInjection(), 
                                             lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //更新操作记录。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                            SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                            WHERE TRANSACTION_KEY = '{1}'", 
                                            undoTransactionKey.PreventSQLInjection(), 
                                            transactionKey.PreventSQLInjection(),
                                            editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //添加撤销操作。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoHoldLot ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销释放批次的操作。
        /// </summary>
        /// <param name="transactionKey">释放批次操作的主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoReaseLot(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //更新批次
                sqlCommand = string.Format(@"UPDATE POR_LOT 
                                             SET HOLD_FLAG = 1, 
                                                EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                             WHERE LOT_KEY='{1}'", 
                                             transactionKey.PreventSQLInjection(), 
                                             lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //更新释放批次的操作。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                            SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                            WHERE TRANSACTION_KEY = '{1}'", 
                                            undoTransactionKey.PreventSQLInjection(), 
                                            transactionKey.PreventSQLInjection(), 
                                            editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //添加撤销操作的记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoReaseLot ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销报废操作。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="transactionKey">报废操作的主键。</param>
        /// <param name="pretransactionKey">报废操作的上一个主键。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoSetLoss(string lotKey, string transactionKey, string pretransactionKey, string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //获取报废数量。
                string sql =string.Format(@"SELECT ISNULL(QUANTITY_IN,0)  FROM WIP_TRANSACTION  WHERE TRANSACTION_KEY='{0}'",
                                            transactionKey.PreventSQLInjection());
                int quantity = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                //更新批次信息。
                sql = string.Format(@"UPDATE POR_LOT
                                    SET QUANTITY=QUANTITY+{0},
	                                    EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
	                                    EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
	                                    EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
	                                    OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}')
                                    WHERE LOT_KEY = '{2}'", 
                                    quantity,
                                    pretransactionKey.PreventSQLInjection(), 
                                    lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                //更新撤销操作。
                sql = string.Format(@"UPDATE WIP_TRANSACTION 
                                      SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                      WHERE TRANSACTION_KEY = '{1}'", 
                                      undoTransactionKey.PreventSQLInjection(), 
                                      transactionKey.PreventSQLInjection(), 
                                      editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //添加撤销记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoSetLoss ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销不良重工出线上仓操作。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="preTransactionKey">上一操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoRWOutStore(string transactionKey, string preTransactionKey, string lotKey, string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //从WIP_TRANSACTION 获取CHILD_LOT_KEY,数量
                string sql =string.Format(@"SELECT ISNULL(QUANTITY_IN,0) QUANTITY,CHILD_LOT_KEY
                                            FROM WIP_TRANSACTION  
                                            WHERE TRANSACTION_KEY='{0}'",
                                            transactionKey.PreventSQLInjection());
                DataTable dt = db.ExecuteDataSet(dbTrans, CommandType.Text, sql).Tables[0];
                int quantity = Convert.ToInt32(dt.Rows[0]["QUANTITY"]);
                string childLotKey = Convert.ToString(dt.Rows[0]["CHILD_LOT_KEY"]);
                //更新子批次状态为1：申请过账。
                sql =string.Format(@"UPDATE WST_STORE_MAT 
                                    SET OBJECT_STATUS='1'
                                    WHERE ROW_KEY='{0}'",
                                    childLotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //根据ROW_KEY从WST_STORE_MAT获取STORE_KEY，ORDER_NUMBER
                sql = string.Format(@"SELECT STORE_KEY,WORKORDER_NUMBER
                                    FROM WST_STORE_MAT 
                                    WHERE ROW_KEY='{0}'", 
                                    childLotKey.PreventSQLInjection());
                dt = db.ExecuteDataSet(dbTrans, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string storeKey = Convert.ToString(dt.Rows[0]["STORE_KEY"]);
                    string orderNumber = Convert.ToString(dt.Rows[0]["WORKORDER_NUMBER"]);
                    //根据STORE_KEY，ORDER_NUMBER从WST_STORE_SUM获取第一笔资料的ROW_KEY
                    sql = string.Format(@"SELECT ROW_KEY 
                                          FROM WST_STORE_SUM 
                                          WHERE WORKORDER_NUMBER='{0}' AND STORE_KEY='{1}'",
                                          orderNumber.PreventSQLInjection(), 
                                          storeKey.PreventSQLInjection());
                    dt = db.ExecuteDataSet(dbTrans, CommandType.Text, sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //更新WST_STORE_SUM数量
                        string rowKey = Convert.ToString(dt.Rows[0]["ROW_KEY"]);
                        sql = string.Format(@"UPDATE WST_STORE_SUM 
                                            SET SUM_QUANTITY=ISNULL(SUM_QUANTITY,0)-{0},
                                                SUM_QUANTITY_ALL=ISNULL(SUM_QUANTITY_ALL,0)-{0} 
                                            WHERE ROW_KEY='{1}'", 
                                            quantity, 
                                            rowKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                    }
                }
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                //更新撤销操作。
                sql = string.Format(@"UPDATE WIP_TRANSACTION 
                                    SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                    WHERE TRANSACTION_KEY = '{1}'", 
                                    undoTransactionKey.PreventSQLInjection(), 
                                    transactionKey.PreventSQLInjection(), 
                                    editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //添加撤销记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoRWOutStore ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销不良重工原因分类操作。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="preTransactionKey">上一操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoRWStoreTransaction(string transactionKey, string preTransactionKey, string lotKey, string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //从WIP_TRANSACTION 获取CHILD_LOT_KEY,数量
                string sql =string.Format(@"SELECT NVL(QUANTITY_IN,0) QUANTITY,CHILD_LOT_KEY
                                            FROM WIP_TRANSACTION  WHERE TRANSACTION_KEY='{0}'",
                                            transactionKey.PreventSQLInjection());
                DataTable dt = db.ExecuteDataSet(dbTrans, CommandType.Text, sql).Tables[0];
                int quantity = Convert.ToInt32(dt.Rows[0]["QUANTITY"]);
                string childLotKey = Convert.ToString(dt.Rows[0]["CHILD_LOT_KEY"]);
                //更新子批次状态为0：初始状态。
                sql =string.Format(@"UPDATE WST_STORE_MAT SET OBJECT_STATUS='0' WHERE ROW_KEY='{0}'",
                                    childLotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                //更新撤销操作。
                sql = string.Format(@"UPDATE WIP_TRANSACTION SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                      WHERE TRANSACTION_KEY = '{1}'", 
                                      undoTransactionKey.PreventSQLInjection(), 
                                      transactionKey.PreventSQLInjection(), 
                                      editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                //添加撤销记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoRWStoreTransaction ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销不良重工拆批操作。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="preTransactionKey">上一操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoSplitToStore(string transactionKey, string preTransactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            int iSplitQty = 0;
            string childKey = string.Empty;
            try
            {
                //获取拆批数量及其拆分的批次主键。
                sqlCommand =string.Format(@"SELECT SPLIT_QUANTITY,CHILD_LOT_KEY 
                                            FROM WIP_SPLIT 
                                            WHERE TRANSACTION_KEY='{0}'", 
                                            transactionKey.PreventSQLInjection());
                DataTable dataTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    //循环所有子批次。
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        iSplitQty += Convert.ToInt32(dataRow["SPLIT_QUANTITY"].ToString());
                        childKey = dataRow["CHILD_LOT_KEY"].ToString();
                        //更新拆分出的批次为作废状态。
                        sqlCommand =string.Format(@"UPDATE WST_STORE_MAT SET OBJECT_STATUS='3' WHERE ROW_KEY='{0}'",
                                                    childKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    }
                    //更新主批信息。
                    sqlCommand = string.Format(@"UPDATE POR_LOT
                                                SET QUANTITY=QUANTITY+{0},
                                                    EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                                    EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                                    EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                                    OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}')
                                                WHERE LOT_KEY = '{2}'", 
                                                iSplitQty, 
                                                preTransactionKey.PreventSQLInjection(), 
                                                lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    //更新撤销操作。
                    string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                WHERE TRANSACTION_KEY = '{1}'", 
                                                undoTransactionKey.PreventSQLInjection(), 
                                                transactionKey.PreventSQLInjection(), 
                                                editor.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    //添加撤销记录。
                    AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SPLIT_TOSTORE, dbTrans);
                    dbTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoSplitToStore ERROR: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销不良重工入线上仓操作。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoMoveToStore(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();

            try
            {
                //获取拆批的批次主键。
                sqlCommand = @"SELECT CHILD_LOT_KEY FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '" + transactionKey.PreventSQLInjection() + "'";
                DataSet ds = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string rowKey = ds.Tables[0].Rows[0]["CHILD_LOT_KEY"].ToString();
                    //更新WST_STORE_MAT状态。
                    sqlCommand = @"UPDATE WST_STORE_MAT SET OBJECT_STATUS='3' WHERE ROW_KEY = '" + rowKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    //更新撤销操作。
                    string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                                SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                WHERE TRANSACTION_KEY = '{1}'", 
                                                undoTransactionKey.PreventSQLInjection(),
                                                transactionKey.PreventSQLInjection(),
                                                editor.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    //添加撤销记录。
                    AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RWTOSTORE, dbTrans);
                    dbTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoMoveToStore ERROR: " + ex.Message);
                throw ex;

            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销批次退料操作。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="pretransactionKey">上一操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoLossBattery(string transactionKey, string pretransactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();

            int iSetLossQty = 0;

            try
            {
                sqlCommand =string.Format(@"SELECT ISNULL(QUANTITY_IN,0) AS QUANTITY FROM WIP_TRANSACTION T WHERE T.TRANSACTION_KEY='{0}'",
                                            transactionKey.PreventSQLInjection());
                iSetLossQty = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                //更新批次数据。
                sqlCommand = string.Format(@"UPDATE POR_LOT
                                            SET QUANTITY=QUANTITY+{0},
                                                EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                                EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                                EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}'),
                                                OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{1}')
                                            WHERE LOT_KEY = '{2}'", 
                                            iSetLossQty,
                                            pretransactionKey.PreventSQLInjection(), 
                                            lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //获取批次信息（是否重工批次，工单主键）
                sqlCommand = string.Format(@"SELECT REWORK_FLAG,WORK_ORDER_KEY,FACTORYROOM_KEY,CREATE_OPERTION_NAME,MATERIAL_CODE,MATERIAL_LOT
                                             FROM POR_LOT 
                                             WHERE LOT_KEY='{0}'", 
                                             lotKey.PreventSQLInjection());
                DataSet ds= db.ExecuteDataSet(dbTrans, CommandType.Text, sqlCommand);
                string reworkFlag = Convert.ToString(ds.Tables[0].Rows[0]["REWORK_FLAG"]);
                string materialLot = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_LOT"]);
                string materialCode = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_CODE"]);
                string factoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0]["FACTORYROOM_KEY"]);
                string createOperationName = Convert.ToString(ds.Tables[0].Rows[0]["CREATE_OPERTION_NAME"]);
                string workOrderKey = Convert.ToString(ds.Tables[0].Rows[0]["WORK_ORDER_KEY"]);
                //如果是正常批次。
                if (reworkFlag == "0")
                {
                    //更新工单剩余数量。
                    string sql = string.Format(@"UPDATE POR_WORK_ORDER 
                                                SET QUANTITY_LEFT = QUANTITY_LEFT - {0}
                                                WHERE WORK_ORDER_KEY = '{1}'", 
                                                iSetLossQty, 
                                                workOrderKey.PreventSQLInjection());
                    int ret = db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                    //正常批次批次退料撤销后不进行数量回减 modify by peter 20120621
//                    string storeKey = string.Empty;
//                    //根据车间主键+工序+仓库类型获取线上仓主键。
//                    sql = string.Format(@"SELECT STORE_KEY,STORE_NAME
//                                                FROM WST_STORE 
//                                                WHERE STORE_TYPE='9' AND LOCATION_KEY='{0}' AND OPERATION_NAME='{1}'",
//                                                factoryRoomKey, createOperationName);
//                    DataSet ds = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
//                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//                    {
//                        storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
//                    }
//                    string storeMaterialDetailKey = string.Empty;
//                    //根据线上仓主键 + 物料批号键获取线上仓物料明细主键。
//                    sql = string.Format(@"SELECT b.STORE_MATERIAL_DETAIL_KEY
//                                            FROM WST_STORE_MATERIAL a
//                                              LEFT JOIN POR_MATERIAL c ON a.MATERIAL_KEY=c.MATERIAL_KEY
//                                            LEFT JOIN WST_STORE_MATERIAL_DETAIL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
//                                            WHERE a.STORE_KEY='{0}' AND b.MATERIAL_LOT='{1}' AND c.MATERIAL_CODE='{2}'", storeKey, materialLot, materialCode);
//                    ds = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
//                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//                    {
//                        storeMaterialDetailKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"]);
//                    }
//                    //更新线上仓+物料批次数量。
//                    sql = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL SET CURRENT_QTY=CURRENT_QTY-{0}
//                                                 WHERE STORE_MATERIAL_DETAIL_KEY='{1}'", setLossQty, storeMaterialDetailKey);
//                    ret = db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }
                //重工批次。
                else if (reworkFlag == "1")
                {
                    string storeKey = string.Empty;
                    //获取重工线上仓主键。
                    string sql = string.Format(@"SELECT K.STORE_KEY 
                                                FROM WST_STORE K
                                                WHERE K.STORE_TYPE = 0 
                                                AND K.LOCATION_KEY =  '{0}' 
                                                AND K.OPERATION_NAME =  '{1}'",
                                                factoryRoomKey.PreventSQLInjection(), 
                                                createOperationName.PreventSQLInjection());
                    ds= db.ExecuteDataSet(dbTrans, CommandType.Text, sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
                    }
                    //更新工单返工数量。
                    sql = string.Format(@"UPDATE POR_WORK_ORDER
                                         SET QUANTITY_REWORK =ISNULL(QUANTITY_REWORK,0) - {0}
                                         WHERE WORK_ORDER_KEY = '{1}'", 
                                        iSetLossQty, workOrderKey.PreventSQLInjection());
                    int ret = db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                    //更新重工线上仓数据。
                    sql = string.Format(@"UPDATE t 
                                          SET t.SUM_QUANTITY=t.SUM_QUANTITY-{0}
                                          FROM WST_STORE_SUM t 
                                          WHERE t.WORKORDER_NUMBER=(SELECT P.ORDER_NUMBER FROM POR_WORK_ORDER P WHERE P.WORK_ORDER_KEY='{1}')
                                          AND t.STORE_KEY = '{2}'",
                                          iSetLossQty, workOrderKey.PreventSQLInjection(), storeKey.PreventSQLInjection());
                    ret = db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }

                //更新撤销操作。
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                            SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                            WHERE TRANSACTION_KEY = '{1}'", 
                                            undoTransactionKey.PreventSQLInjection(), 
                                            transactionKey.PreventSQLInjection(), 
                                            editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                //添加撤销记录。
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoLossBattery ERROR: " + ex.Message);
                throw ex;

            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }

            return true;

        }
        /// <summary>
        /// 撤销进站前数据采集。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="transactionKey">操作主键。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoInEDC(string lotKey, string transactionKey, string pretransactionKey,string editor)
        {
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                //update lot info ,set state is WaitintForTrackInEDC
                string sql = string.Format(@"UPDATE POR_LOT
                                            SET STATE_FLAG=0, 
                                                EDITOR=(SELECT EDITOR FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                EDIT_TIME=(SELECT EDIT_TIME FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                EDIT_TIMEZONE=(SELECT EDIT_TIMEZONE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}'),
                                                OPR_LINE=(SELECT OPR_LINE FROM WIP_TRANSACTION WHERE TRANSACTION_KEY = '{0}')
                                            WHERE LOT_KEY = '{1}'", 
                                            pretransactionKey.PreventSQLInjection(), 
                                            lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                //update tansaction info 
                string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                sql = string.Format(@"UPDATE WIP_TRANSACTION 
                                      SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                      WHERE TRANSACTION_KEY = '{1}'", 
                                      undoTransactionKey.PreventSQLInjection(), 
                                      transactionKey.PreventSQLInjection(), 
                                      editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                //insert tansaction info of undo
                AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC, dbTrans);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoInEDC: " + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销线上仓批次合并记录。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoStoreMerge(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();

            int iMergeQty = 0;
            string mainLotKey = string.Empty;
            string childLotKey = string.Empty;
            
            try
            {
                sqlCommand = @"SELECT MERGE_QUANTITY,MAIN_LOT_KEY,CHILD_LOT_KEY 
                               FROM WIP_MERGE 
                               WHERE TRANSACTION_KEY='" + transactionKey.PreventSQLInjection()+ "'";
                DataTable dataTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    mainLotKey = dataTable.Rows[0]["MAIN_LOT_KEY"].ToString();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        iMergeQty += Convert.ToInt32(dataRow["MERGE_QUANTITY"]);
                        childLotKey = dataRow["CHILD_LOT_KEY"].ToString();

                        //update wst_store_mat child lot info
                        sqlCommand = string.Format(@"UPDATE WST_STORE_MAT 
                                                    SET OBJECT_STATUS='0',ITEM_QTY={0}
                                                    WHERE ROW_KEY='{1}'",
                                                    Convert.ToInt32(dataRow["MERGE_QUANTITY"]),
                                                    childLotKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                    }

                    //update wst_store_mat main lot info
                    sqlCommand = string.Format(@"UPDATE WST_STORE_MAT 
                                                SET OBJECT_STATUS='0',ITEM_QTY=ITEM_QTY-{0} 
                                                WHERE ROW_KEY='{1}'", 
                                                iMergeQty, mainLotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    //update wip_transaction info
                    string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                                SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                WHERE TRANSACTION_KEY = '{1}'", 
                                                undoTransactionKey.PreventSQLInjection(),
                                                transactionKey.PreventSQLInjection(),
                                                editor.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_STOREMERGE, dbTrans);
                    dbTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoStoreMerge ERROR:" + ex.Message);
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 撤销线上仓批次被合并记录。
        /// </summary>
        /// <param name="transactionKey">操作主键。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <returns>true:撤销成功，false:撤销失败。</returns>
        public bool UndoStoreMerged(string transactionKey, string lotKey, string editor)
        {
            string sqlCommand = string.Empty;

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();

            int iMergeQty = 0;
            string childLotKey = string.Empty;
            string mainLotKey = string.Empty;

            try
            {
                sqlCommand = @"SELECT MERGE_QUANTITY,CHILD_LOT_KEY,MAIN_LOT_KEY 
                               FROM WIP_MERGE 
                               WHERE CHILD_TRANSACTION_KEY='" + transactionKey.PreventSQLInjection() + "'";
                DataTable dataTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    iMergeQty = Convert.ToInt32(dataTable.Rows[0]["MERGE_QUANTITY"]);
                    childLotKey = dataTable.Rows[0]["CHILD_LOT_KEY"].ToString();
                    mainLotKey = dataTable.Rows[0]["MAIN_LOT_KEY"].ToString();

                    //update wst_store_mat
                    sqlCommand = string.Format(@"UPDATE WST_STORE_MAT 
                                                SET OBJECT_STATUS='0',ITEM_QTY= {0} WHERE ROW_KEY='{1}'", 
                                                iMergeQty, childLotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    sqlCommand = string.Format(@"UPDATE WST_STORE_MAT SET OBJECT_STATUS='0',ITEM_QTY= ITEM_QTY-{0} 
                                                WHERE ROW_KEY='{1}'", iMergeQty, mainLotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    //update wip_transaction info    
                    string undoTransactionKey = UtilHelper.GenerateNewKey(0);
                    sqlCommand = string.Format(@"UPDATE WIP_TRANSACTION 
                                                SET UNDO_FLAG = 1,UNDO_TRANSACTION_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{2}',ETL_FLAG='U'
                                                WHERE TRANSACTION_KEY = '{1}'", 
                                                undoTransactionKey.PreventSQLInjection(),
                                                transactionKey.PreventSQLInjection(),
                                                editor.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);

                    AddUndoTransaction(undoTransactionKey, editor, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_STORE_MERGED, dbTrans);
                    dbTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UndoStoreMerged ERROR: " + ex.Message);
                throw ex;
                
            }
            finally
            {
                dbTrans.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 添加撤销某笔历史操作记录的操作记录。
        /// </summary>
        /// <param name="transactionKey">撤销操作的主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <param name="type">撤销操作的类型。</param>
        /// <param name="dbtran">数据库事务操作对象。</param>
        private void AddUndoTransaction(string transactionKey, string editor,string type,DbTransaction dbtran)
        {
            Hashtable hashTable= new Hashtable();           
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_UNDO + type);            
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);           
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, "CN-ZH");
            string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, hashTable,null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
        }

    }
}
