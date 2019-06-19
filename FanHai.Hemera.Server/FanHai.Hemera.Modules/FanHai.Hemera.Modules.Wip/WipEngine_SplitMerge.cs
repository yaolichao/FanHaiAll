using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils;

using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Modules.FMM;


namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 执行合并批次操作。
        /// </summary>
        /// <param name="dsParams">包含合并批次数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet MergeLot(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;              
            try
            {
                 dbConn = db.CreateConnection();
                 dbConn.Open();
                 //Create Transaction  
                 dbTran = dbConn.BeginTransaction();
                 //check edittime
                 if (dsParams.Tables.Count > 0)
                 { 
                     //检查记录是否过期。
                     List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                     KeyValuePair<string, string> kvp = new KeyValuePair<string,string> ();
                     if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA)) 
                     {
                         DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                         Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                         kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY,htParams[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                         listCondition.Add(kvp);
                         if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, htParams[POR_LOT_FIELDS.FIELD_EDIT_TIME].ToString()))
                         {
                             SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                             return dsReturn;
                         }
                     }
                     //检查记录是否过期。
                     if (dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                     {
                         listCondition = new List<KeyValuePair<string, string>>();
                         DataTable parameterdatatable = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                         
                         for (int i = 0; i < parameterdatatable.Rows.Count; i++)                         
                         {
                             kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, parameterdatatable.Rows[i]["LOT_KEY"].ToString());
                             listCondition.Add(kvp);
                             if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, parameterdatatable.Rows[i]["EDIT_TIME"].ToString()))
                             {
                                 SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                                 return dsReturn;
                             }
                         }
                     }

                 }
                 MergeLot(db, dbTran, dsParams);
                 SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                 dbTran.Commit();
             }
             catch (Exception ex)
             {
                 SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("MergeLot Error: " + ex.Message);
                 dbTran.Rollback();
             }
             finally
             {
                 dbConn.Close();
                 dbTran.Dispose();
                 dbConn.Dispose();
             }
             DateTime endTime = DateTime.Now;
             LogService.LogInfo("MergeLot Time: " + (endTime - startTime).TotalMilliseconds.ToString());
             return dsReturn;
        }
        /// <summary>
        /// 执行合并批次的操作。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库操作事务。</param>
        /// <param name="dsParams">包含合并数据的数据集。</param>
        private void  MergeLot(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            DataSet ds;
            string strParentLotKey = "";
            string strParentLotNumber = "";
            string strParentTransKey = "";
            string strParentStepKey = "";
            string strParentRouteKey = "";
            string strParentEnterpriseKey = "";
            string strParentQTY = "";
            string strTotalQTY = "";
            string strComment = "";
            string strEditor = "", strEditTimeZone = "", strEditTime = "";
            string strLineKey = "", strShiftName = "", strWorkOrderKey = "",strStateFlag="";
            string oprLine = "";
 
            if (dsParams.Tables.Count > 0)
            {
                //获取输入参数。
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                strParentLotKey = htParams[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                strParentLotNumber = htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
                strTotalQTY = htParams[POR_LOT_FIELDS.FIELD_QUANTITY].ToString();
                strComment = htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT].ToString();
                strEditor = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                strEditTimeZone = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                strEditTime = UtilHelper.GetSysdate(db).ToString("yyyy-MM-dd HH:mm:ss");
                string shiftKey = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
                string operComputerName = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
                if (htParams.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME))
                {
                    strShiftName = htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME].ToString();
                }
                else
                {
                    strShiftName = UtilHelper.GetShiftValue(db, strEditTime);
                }
                if (string.IsNullOrEmpty(shiftKey))
                {
                    shiftKey = UtilHelper.GetShiftKey(db, strEditTime);
                }
                //获取合并到批次的详细信息。
                ds = LotManagement.GetLotDetailsEx(db,dbtran, strParentLotKey);
                strParentStepKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                strParentRouteKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
                strParentEnterpriseKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                strParentQTY = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString();
                strLineKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString();
                oprLine = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();
                strWorkOrderKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
                strStateFlag = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                string reworkFlag = Convert.ToString(ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                //更新合并到批次的数据
                sql =string.Format(@"UPDATE POR_LOT 
                                    SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}' 
                                    WHERE LOT_KEY='{3}'",
                                    strTotalQTY.PreventSQLInjection(),
                                    strEditor.PreventSQLInjection(),
                                    strEditTimeZone.PreventSQLInjection(),
                                    strParentLotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                //记录合批操作。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                Hashtable parenttransactionTable = new Hashtable();
                DataTable parenttransaction = new DataTable();
                strParentTransKey = UtilHelper.GenerateNewKey(0);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, strParentLotKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, strParentQTY);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, strTotalQTY);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGE);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, strComment);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, strEditor);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, strEditTimeZone);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, strEditTime);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, strStateFlag);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, strEditor);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, strLineKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, parenttransactionTable, null);
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                string strChildLotKey = "";
                string strChildLotNumber = "";
                string strChildQty = "";

                if (dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtBaseParameters = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                    //循环被合并的批次。
                    for (int i = 0; i < dtBaseParameters.Rows.Count; i++)
                    {                                
                        strChildLotKey = dtBaseParameters.Rows[i]["LOT_KEY"].ToString();
                        strChildLotNumber = dtBaseParameters.Rows[i]["LOT_NUMBER"].ToString();
                        strChildQty = dtBaseParameters.Rows[i]["QUANTITY"].ToString();
                        //更新被合并批次的数据
                        sql =string.Format(@"UPDATE POR_LOT
                                            SET QUANTITY='0',DELETED_TERM_FLAG='1',EDITOR='{0}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{1}' 
                                            WHERE LOT_KEY='{2}'",
                                            strEditor.PreventSQLInjection(),
                                            strEditTimeZone.PreventSQLInjection(),
                                            strChildLotKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        //插入被合并批次的交易记录
                        Hashtable childtransactionTable = new Hashtable();
                        DataTable childtransaction = new DataTable();
                        string strChildTransKey = UtilHelper.GenerateNewKey(0);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, strChildLotKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, strChildQty);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, "0");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGED);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, strEditor);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, strEditTimeZone);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, strEditTime);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, strStateFlag);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, strEditor);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, strLineKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipFields, childtransactionTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        //插入WIP_MERGE表
                        Hashtable MergeHashTable = new Hashtable();
                        WIP_MERGE_FIELDS wipMerge = new WIP_MERGE_FIELDS();
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_CHILD_LOT_KEY, strChildLotKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_MAIN_LOT_KEY, strParentLotKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY, strChildQty);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDITOR, strEditor);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDIT_TIME, strEditTime);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDIT_TIMEZONE, strEditTimeZone);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_CHILD_TRANSACTION_KEY, strChildTransKey);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipMerge, MergeHashTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    }
                }
            }
            
        }
        /// <summary>
        /// 根据合并批次主键获取可被合并的批次数据。
        /// </summary>
        /// <param name="strParentLotKey">合并批次主键。</param>
        /// <returns>包含可被合并的批次数据的数据集对象。</returns>
        public DataSet GetLotsForMerge(string strParentLotKey)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            string strParentLot = strParentLotKey;
            string strWorkOrderKey = "", strStepKey = "", strLineKey = "";
            string strStateFlag = "", strReworkFlag = "";
            try
            {
                ds = LotManagement.GetLotBasicInfo(db, strParentLot);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strWorkOrderKey = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
                    strStepKey = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                    strLineKey = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString();
                    strStateFlag = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                    strReworkFlag = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED].ToString();
                }
                string sql =string.Format(@"SELECT '' AS CHECK_BOX,A.LOT_KEY,A.LOT_NUMBER,A.LINE_NAME,A.WORK_ORDER_SEQ,
                                                    A.QUANTITY,A.HOLD_FLAG,A.STATUS,A.STATE_FLAG,
		                                            A.CUR_PRODUCTION_LINE_KEY,A.CUR_ROUTE_VER_KEY,
		                                            A.CUR_STEP_VER_KEY,A.WORK_ORDER_KEY,
		                                            A.ROUTE_ENTERPRISE_VER_KEY,C.ORDER_NUMBER,C.PART_NUMBER,
		                                            D.ROUTE_NAME,B.ROUTE_OPERATION_VER_KEY,
		                                            B.SCRAP_REASON_CODE_CATEGORY_KEY,B.ROUTE_STEP_NAME,A.EDIT_TIME
                                            FROM POR_WORK_ORDER C,POR_ROUTE_ROUTE_VER D,POR_ROUTE_STEP B,POR_LOT A
                                            WHERE C.WORK_ORDER_KEY=A.WORK_ORDER_KEY
                                            AND ( A.CUR_STEP_VER_KEY = B.ROUTE_STEP_KEY AND A.CUR_ROUTE_VER_KEY = D.ROUTE_ROUTE_VER_KEY) 
                                            AND A.STATUS = 1 
                                            AND A.QUANTITY != 0 
                                            AND A.DELETED_TERM_FLAG = 0 
                                            AND A.HOLD_FLAG=0 
                                            AND A.LOT_KEY !='{0}' 
                                            AND A.WORK_ORDER_KEY = '{1}'
                                            AND A.CUR_STEP_VER_KEY = '{2}' 
                                            AND A.STATE_FLAG = {3}",
                                            strParentLotKey.PreventSQLInjection(),
                                            strWorkOrderKey.PreventSQLInjection(),
                                            strStepKey.PreventSQLInjection(),
                                            strStateFlag.PreventSQLInjection());
                if (strReworkFlag == "0")
                {
                    sql = sql +string.Format(" AND A.REWORK_FLAG={0}",strReworkFlag.PreventSQLInjection());
                }
                else
                {
                    sql = sql + " AND A.REWORK_FLAG>0";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotsForMerge Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据合并批次主键获取可被合并的批次数据。
        /// </summary>
        /// <param name="strParentLotKey">合并批次主键。</param>
        /// <returns>包含可被合并的批次数据的数据集对象。</returns>
        public DataSet GetConformLotsForMerge(string strParentLotKey)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            string strParentLot = strParentLotKey;
            string strWorkOrderKey = "", strStepKey = "", strLineKey = "";
            string strStateFlag = "";
            try
            {
                ds = LotManagement.GetLotBasicInfo(db, strParentLot);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strWorkOrderKey = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
                    strStepKey = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                    strLineKey = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString();
                    strStateFlag = ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                }
                string sql =string.Format(@"SELECT A.LOT_KEY,A.LOT_NUMBER,A.LINE_NAME,A.WORK_ORDER_SEQ,
                                              A.QUANTITY,A.HOLD_FLAG,A.STATUS,A.STATE_FLAG,
                                              A.CUR_PRODUCTION_LINE_KEY,A.CUR_ROUTE_VER_KEY,
                                              A.CUR_STEP_VER_KEY,A.WORK_ORDER_KEY,
                                              A.ROUTE_ENTERPRISE_VER_KEY,C.ORDER_NUMBER,C.PART_NUMBER,
                                              D.ROUTE_NAME,B.ROUTE_OPERATION_VER_KEY,
                                              B.SCRAP_REASON_CODE_CATEGORY_KEY,B.ROUTE_STEP_NAME
                                          FROM POR_WORK_ORDER C,POR_ROUTE_ROUTE_VER D,POR_ROUTE_STEP B,POR_LOT A
                                          WHERE C.WORK_ORDER_KEY=A.WORK_ORDER_KEY
                                          AND ( A.CUR_STEP_VER_KEY = B.ROUTE_STEP_KEY
                                          AND A.CUR_ROUTE_VER_KEY = D.ROUTE_ROUTE_VER_KEY) AND A.STATUS = 1 
                                          AND A.QUANTITY != 0 
                                          AND A.DELETED_TERM_FLAG = 0 
                                          AND A.HOLD_FLAG=0 
                                          AND A.WORK_ORDER_KEY ='{0}' 
                                          AND A.CUR_STEP_VER_KEY='{1}' 
                                          AND A.STATE_FLAG ={2}",
                                          strWorkOrderKey.PreventSQLInjection(),
                                          strStepKey.PreventSQLInjection(),
                                          strStateFlag.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetConformLotsForMerge Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 将线边仓中的批次进行合并。
        /// </summary>
        /// <param name="dsParams">
        /// 包含合批数据的数据集对象.
        /// (1)数据集对象中包含一个名称为<see cref="BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// 数据表中存放待合并的批次返工或退库数据。
        /// (2) 数据集对象中包含一个名称为<see cref="TRANS_TABLES.TABLE_MAIN_DATA"/>的数据表对象。
        /// 数据表中必须包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。
        /// 键名：
        /// 合并到批次的返工或退库记录的主键<see cref="WST_STORE_MAT_FIELDS.FIELD_ROW_KEY"/>,
        /// 合并到批次的批次号<see cref="POR_LOT_FIELDS.FIELD_LOT_NUMBER"/>,
        /// 合并后批次的数量<see cref="POR_LOT_FIELDS.FIELD_QUANTITY"/>,
        /// 编辑人<see cref="WIP_TRANSACTION_FIELDS.FIELD_EDITOR"/>,
        /// 编辑时区<see cref="WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY"/>,
        /// 编辑时间<see cref="WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME"/>,
        /// 批次主键<see cref="POR_LOT_FIELDS.FIELD_LOT_KEY"/>。
        /// </param>
        /// <returns>
        /// 包含批次合并执行结果的数据集对象。
        /// </returns>
        public DataSet MergeLotInStore(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;

            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                //组织合批数据
                if (dsParams.Tables.Count > 0)
                {
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
                    //存放待合并的批次返工或退库数据的数据表存在。
                    if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                    {
                        DataTable dataTable = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                        Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                        kvp = new KeyValuePair<string, string>(WST_STORE_MAT_FIELDS.FIELD_ROW_KEY, hashData[WST_STORE_MAT_FIELDS.FIELD_ROW_KEY].ToString());
                        listCondition.Add(kvp);
                        //检查记录是否过期。
                        if (UtilHelper.CheckRecordExpired(db, WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME, listCondition, hashData[WST_STORE_MAT_FIELDS.FIELD_EDIT_TIME].ToString()))
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                            return dsReturn;
                        }
                    }
                    
                    if (dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                    {
                        listCondition = new List<KeyValuePair<string, string>>();
                        DataTable parameterdatatable = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];

                        for (int i = 0; i < parameterdatatable.Rows.Count; i++)
                        {
                            kvp = new KeyValuePair<string, string>(WST_STORE_MAT_FIELDS.FIELD_ROW_KEY, parameterdatatable.Rows[i][WST_STORE_MAT_FIELDS.FIELD_ROW_KEY].ToString());
                            listCondition.Add(kvp);
                            //检查记录是否过期。
                            if (UtilHelper.CheckRecordExpired(db, WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME, listCondition, parameterdatatable.Rows[i][WST_STORE_MAT_FIELDS.FIELD_EDIT_TIME].ToString()))
                            {
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                                return dsReturn;
                            }
                        }
                    }
                }
                MergeLotInStore(db, dbtran, dsParams);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                dbtran.Commit();
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("MergeLotInStore Error: " + ex.Message);
                dbtran.Rollback();
            }
            finally
            {
                dbconn.Close();
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("MergeLotInStore Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
        /// <summary>
        /// 将线边仓中的批次进行合并。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库操作事务对象。</param>
        /// <param name="dsParams">
        /// 包含合批数据的数据集对象.
        /// (1)数据集对象中包含一个名称为<see cref="BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// 数据表中存放待合并的批次返工或退库数据。
        /// (2) 数据集对象中包含一个名称为<see cref="TRANS_TABLES.TABLE_MAIN_DATA"/>的数据表对象。
        /// 数据表中必须包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。
        /// 键名：
        /// 合并到批次的返工或退库记录的主键<see cref="WST_STORE_MAT_FIELDS.FIELD_ROW_KEY"/>,
        /// 合并到批次的批次号<see cref="POR_LOT_FIELDS.FIELD_LOT_NUMBER"/>,
        /// 合并后批次的数量<see cref="POR_LOT_FIELDS.FIELD_QUANTITY"/>,
        /// 编辑人<see cref="WIP_TRANSACTION_FIELDS.FIELD_EDITOR"/>,
        /// 编辑时区<see cref="WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY"/>,
        /// 编辑时间<see cref="WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME"/>,
        /// 批次主键<see cref="POR_LOT_FIELDS.FIELD_LOT_KEY"/>。
        /// </param>
        private void MergeLotInStore(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            String sql = string.Empty;
            DataSet dsReturn = new DataSet();
            DataSet ds;
            string strParentRowKey = "";
            string strParentLotNumber = "";
            string strParentTransKey = "";
            string strParentStepKey = "";
            string strParentRouteKey = "";
            string strParentEnterpriseKey = "";
            string strParentQTY = "";
            string strTotalQTY = "";
            string strSumQTY = "";
            string strComment = "";
            string strEditor = "", strEditTimeZone = "", strEditTime = "";
            string strLineKey = "", strShiftName = "", strWorkOrderKey = "", strStateFlag = "";

            if (dsParams.Tables.Count > 0)//数据集中包含数据表。
            {
                DataTable dataTable = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                strParentRowKey = hashData[WST_STORE_MAT_FIELDS.FIELD_ROW_KEY].ToString();
                strParentLotNumber = hashData["LOT_NUMBER"].ToString();
                strSumQTY = hashData[POR_LOT_FIELDS.FIELD_QUANTITY].ToString();
                //strComment = hashData[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT].ToString();
                strEditor = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                strEditTimeZone = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                strEditTime = UtilHelper.GetSysdate(db).ToString("yyyy-MM-dd HH:mm:ss");
                // Get Lot Details
                ds = StoreEngine.GetStoreLotDetailsInfor(db, strParentRowKey);
                strParentStepKey = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME].Rows[0][WST_STORE_MAT_FIELDS.FIELD_STEP_KEY].ToString();
                strParentRouteKey = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME].Rows[0][WST_STORE_MAT_FIELDS.FIELD_ROUTE_KEY].ToString();
                strParentEnterpriseKey = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME].Rows[0][WST_STORE_MAT_FIELDS.FIELD_ENTERPRISE_KEY].ToString();
                strParentQTY = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME].Rows[0][WST_STORE_MAT_FIELDS.FIELD_ITEM_QTY].ToString();

                strLineKey = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME].Rows[0][WST_STORE_MAT_FIELDS.FIELD_LINE_KEY].ToString();
                strShiftName = UtilHelper.GetShiftKey(db, strEditTime);
                strWorkOrderKey = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
    
                //更新WST_STORE_MAT记录
                sql =string.Format(@"UPDATE WST_STORE_MAT 
                                    SET ITEM_QTY= '{0}', EDIT_TIME=GETDATE() 
                                    WHERE ROW_KEY='{1}'",
                                    strSumQTY.PreventSQLInjection(),
                                    strParentRowKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                strTotalQTY = (Convert.ToInt32(strParentQTY) + Convert.ToInt32(strSumQTY)).ToString();

                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();

                #region 合并批次操作记录
                Hashtable parenttransactionTable = new Hashtable();
                DataTable parenttransaction = new DataTable();
                strParentTransKey = UtilHelper.GenerateNewKey(0);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, hashData[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_CHILD_LOT_KEY,strParentRowKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, strParentQTY);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, strTotalQTY);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_STOREMERGE);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, strComment);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, strEditor);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, strEditTimeZone);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, strEditTime);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, strLineKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, strStateFlag);
                //向WIP_TRANSACTION插入合并到批次的操作记录
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, parenttransactionTable, null);
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                #endregion

                #region 生成子批次
                string strChildRowKey = "";
                string strChildLotNumber = "";
                string strChildQty = "";

                if (dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable parameterdatatable = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                    for (int i = 0; i < parameterdatatable.Rows.Count; i++)
                    {
                        //更新WST_STORE_MAT数据            
                        strChildRowKey = parameterdatatable.Rows[i][WST_STORE_MAT_FIELDS.FIELD_ROW_KEY].ToString();
                        strChildLotNumber = parameterdatatable.Rows[i]["LOT_NUMBER"].ToString();
                        strChildQty = parameterdatatable.Rows[i][WST_STORE_MAT_FIELDS.FIELD_ITEM_QTY].ToString();                        

                        sql = @"UPDATE WST_STORE_MAT 
                                SET ITEM_QTY='0',OBJECT_STATUS='3', EDIT_TIME=GETDATE() 
                                WHERE ROW_KEY='" + strChildRowKey.PreventSQLInjection() + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        //向WIP_TRANSACTION插入被合并批次的操作记录
                        Hashtable childtransactionTable = new Hashtable();
                        DataTable childtransaction = new DataTable();
                        //sql = "";
                        string strChildTransKey = UtilHelper.GenerateNewKey(0);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY,parameterdatatable.Rows[i][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_CHILD_LOT_KEY,strChildRowKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, strChildQty);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, "0");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_STORE_MERGED);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, strEditor);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, strEditTimeZone);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, strEditTime);

                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, strLineKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, strStateFlag);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipFields, childtransactionTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //向WIP_MERGE插入合并批次记录
                        Hashtable MergeHashTable = new Hashtable();
                        WIP_MERGE_FIELDS wipMerge = new WIP_MERGE_FIELDS();

                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_CHILD_LOT_KEY, strChildRowKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_MAIN_LOT_KEY, strParentRowKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY, strChildQty);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDITOR, strEditor);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDIT_TIME, strEditTime);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDIT_TIMEZONE, strEditTimeZone);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_CHILD_TRANSACTION_KEY, strChildTransKey);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipMerge, MergeHashTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    }                    
                }
                #endregion

            }
        }

    }

 
}
