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



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// （电池片/组件）报废操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP"/>和<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP"/>。
        /// </remarks>
        /// <param name="dsParams">包含报废信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotScrap(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)            //存放操作数据
                    || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //批次操作记录不能为空记录。
                    )          
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
                Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
                Hashtable htParams =CommonUtils.ConvertToHashtable(dtParams);
                string opEditTime = Convert.ToString(htParams[COMMON_FIELDS.FIELD_COMMON_EDIT_TIME]);   //操作时编辑时间
                string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);
                string editTimeZone= Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
                string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
                double leftQty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);
                string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                //检查记录是否过期。防止重复修改。
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                    return dsReturn;
                }

                LotScrap(dsParams, dbTran);
                int deletedTermFlag = 0;
                //是否是组件报废操作 或者物料剩余数量为0，则结束批次。
                if (activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP
                    || leftQty == 0)
                {
                    deletedTermFlag = 1;//需要结束批次。
                }
                //更新批次数量。
                string sql = string.Format(@"UPDATE POR_LOT 
                                            SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',DELETED_TERM_FLAG={4}
                                            WHERE LOT_KEY='{3}'",
                                            leftQty,
                                            editor.PreventSQLInjection(),
                                            editTimeZone.PreventSQLInjection(),
                                            lotKey.PreventSQLInjection(),
                                            deletedTermFlag);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotScrap Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        ///（电池片/组件）报废操作
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP"/>和<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP"/>。
        /// </remarks>
        /// <param name="dsParams">包含报废信息的数据集对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        private void LotScrap(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                || !dsParams.Tables.Contains(WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME)
                || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //批次操作记录不能为空记录。
               )
            {
                throw new Exception("传入参数不正确，请检查。");
            }
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
            //操作动作必须是 SCRAP 或 CELLSCRAP
            if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP
                && activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP)
            {
                throw new Exception("传入参数的报废操作动作不正确，请检查。");
            }
            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);

            string sql = string.Empty;
            //是否是组件报废操作？
            if (activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP)
            {
                //更新批次状态。
                sql = string.Format(@"UPDATE POR_LOT 
                                    SET DELETED_TERM_FLAG=1
                                    WHERE LOT_KEY='{0}'",
                                    lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            }
            //向WIP_TRANSACTION表插入批次报废的操作记录。
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            }
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //如果数据集中包含名称WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME的数据表对象。
            if (dsParams.Tables.Contains(WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME))
            {
                WIP_SCRAP_FIELDS scrapFields = new WIP_SCRAP_FIELDS();
                DataTable dtScrap = dsParams.Tables[WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME];              //存放报废明细数据
                //遍历批次的报废数据。
                for (int i = 0; i < dtScrap.Rows.Count; i++)
                {
                    DataRow drScrap = dtScrap.Rows[i];
                    Hashtable htScrap = CommonUtils.ConvertRowToHashtable(drScrap);
                    if (!htScrap.ContainsKey(WIP_SCRAP_FIELDS.FIELD_TRANSACTION_KEY))
                    {
                        htScrap.Add(WIP_SCRAP_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                    }
                    htScrap[WIP_SCRAP_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                    //插入一笔批次报废数据。
                    sql = DatabaseTable.BuildInsertSqlStatement(scrapFields, htScrap, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
            }
        }

        #region ILotOperationEngine 成员

        /// <summary>
        /// 根据工步主键，工厂主键，设备主键，工序名称，工单号，采集类型获取参数信息以及对应设备虚拟仓数量
        /// </summary>
        /// <param name="stepKey">工步主键</param>
        /// <param name="roomKey">工厂主键</param>
        /// <param name="equipmentKey">设备主键</param>
        /// <param name="operationName">工序名称</param>
        /// <param name="workorder">工单号</param>
        /// <param name="dcType">采集类型</param>
        /// <returns>结果集</returns>
        public DataSet GetStepParams(string stepKey, string roomKey, string equipmentKey, string operationName, string workorder, int dcType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取工步参数。
                string sqlCommand = string.Format(@"SELECT D.*,C.FACTORY_KEY,C.FACTORY_NAME,C.OPERATION_NAME,C.EQUIPMENT_KEY,C.EQUIPMENT_NAME,
                                                            C.PARAMETER,C.PARAMETER_KEY,C.MATERIAL_CODE,
                                                            CASE WHEN C.SUPPLIER_CODE IS NULL THEN '' ELSE C.SUPPLIER_CODE END AS SUPPLIER_CODE,
                                                            C.SUM_QTY,C.SENDING_UNIT,
                                                            C.ORDER_NUMBER,C.SUPPLIER,C.CTIME FROM 
                                                            (SELECT * FROM POR_ROUTE_STEP_PARAM   
                                                            WHERE ROUTE_STEP_KEY = '{0}'
                                                            AND IS_DELETED = 0
                                                            AND IS_FEEDING = 1
                                                            AND DC_TYPE = {1}) D
                                                            LEFT JOIN (
                                                            (SELECT * FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE A INNER JOIN (
                                                                SELECT MIN(CREATE_TIME) AS CTIME FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE
                                                                WHERE FACTORY_KEY = '{2}'
                                                                AND OPERATION_NAME = '{3}'
                                                                AND EQUIPMENT_KEY = '{4}'
                                                                AND ORDER_NUMBER = '{5}'
                                                                AND STATUS = 1
                                                                GROUP BY PARAMETER) B
                                                            ON A.CREATE_TIME = B.CTIME
                                                            WHERE A.FACTORY_KEY = '{2}'
                                                                AND A.OPERATION_NAME = '{3}'
                                                                AND A.EQUIPMENT_KEY = '{4}'
                                                                AND A.ORDER_NUMBER = '{5}'
                                                                AND STATUS = 1)) C
                                                            ON D.PARAM_NAME = C.PARAMETER",
                                                            stepKey,
                                                            dcType,
                                                            roomKey,
                                                            operationName,
                                                            equipmentKey,
                                                            workorder
                                                            );
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStepParams Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}
