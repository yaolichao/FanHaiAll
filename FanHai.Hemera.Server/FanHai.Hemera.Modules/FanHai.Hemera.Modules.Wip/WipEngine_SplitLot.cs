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
        /// 拆分批次。
        /// </summary>
        /// <param name="dsParams">包含拆分批次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SplitLot(DataSet dsParams)
        {
            return SplitLotTransact(dsParams);
        }
        /// <summary>
        /// 执行拆分批次。
        /// </summary>
        /// <param name="dsParams">包含拆分批次数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet SplitLotTransact(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbTransaction dbtran = null;
            DbConnection dbconn = null;
            string lotKey = "", strEditTime = "", strEditor = "";
            try
            {
                //Open Connection
                dbconn = db.CreateConnection();
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();

                #region CheckExpired
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dataTable = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    lotKey = hashData[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                    strEditTime = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME].ToString();
                    strEditor = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, strEditTime))
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                        return dsReturn;
                    }
                }
                #endregion

                SplitLotTransact(db, dbtran, dsParams,ref dsReturn );
                dbtran.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SplitLotTransact Error: " + ex.Message);
                //Rollback Transaction
                dbtran.Rollback();
            }
            finally
            {
                //Close Connection
                dbconn.Close();
            }
            DateTime endTime = DateTime.Now;
            LogService.LogInfo("SplitLotTransact Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 执行拆分批次。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务操作对象。</param>
        /// <param name="dataset">包含拆分批次数据的数据集。</param>
        /// <param name="dsReturn">包含执行结果的数据集。</param>
        /// <returns></returns>
        internal static int SplitLotTransact(Database db, DbTransaction dbtran, DataSet dsParams, ref DataSet dsReturn)
        {
            String sql = string.Empty;
            DataSet ds;
            string strParentLotKey = "";
            string strParentLotNumber = "";
            string strParentSEQ = "";
            string strParentTransKey = "";
            string strParentStepKey = "";
            string strParentRouteKey = "";
            string strParentEnterpriseKey = "";
            string strParentQTY = "";
            string strRemnantQTY = "";
            string strComment = "";
            string strEditor = "", strEditTimeZone = "", strEditTime = "";
            string strLineKey = "", strShiftName = "", strWorkOrderKey = "", strParentStateFlag = "";
            string oprLine = string.Empty;
            string reworkFlag = string.Empty;
            int splitQuantitySum = 0;
            int intSEQ = 0;
            int workOrderStep = 0;
            if (dsParams.Tables.Count > 0)
            {
                //获取参数输入。
                strEditTime = UtilHelper.GetSysdate(db).ToString("yyyy-MM-dd HH:mm:ss");
                DataTable dataTable = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                strParentLotKey = hashData[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();              
                strRemnantQTY = hashData[POR_LOT_FIELDS.FIELD_QUANTITY].ToString();
                strComment = hashData[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT].ToString();
                strEditor = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                strEditTimeZone = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                strParentQTY = hashData[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN].ToString();
                string shiftKey = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
                string operComputerName = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
                if (hashData.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME))
                {
                    strShiftName = hashData[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME].ToString();
                }
                else 
                {
                    strShiftName = UtilHelper.GetShiftValue(db, strEditTime);
                }
                if (string.IsNullOrEmpty(shiftKey))
                {
                    shiftKey = UtilHelper.GetShiftKey(db, strEditTime);
                }

                if (hashData.Contains(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY))
                {
                    strParentStepKey = hashData[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                }
                if (hashData.Contains(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY))
                {
                    strParentRouteKey = hashData[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
                }
                if (hashData.Contains(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY))
                {
                    strParentEnterpriseKey = hashData[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                }
                if (hashData.Contains(POR_LOT_FIELDS.FIELD_STATE_FLAG))
                {
                    strParentStateFlag = hashData[POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                }
                if (hashData.ContainsKey(COMMON_FIELDS.FIELD_WORK_ORDER_STEP))
                {
                    workOrderStep = Convert.ToInt32(hashData[COMMON_FIELDS.FIELD_WORK_ORDER_STEP]);
                }
                //获取被拆分批次信息。
                ds = LotManagement.GetLotDetailsEx(db, dbtran, strParentLotKey);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strParentSEQ = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ].ToString();
                    strParentLotNumber = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();

                    strWorkOrderKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
                    strLineKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString();
                    oprLine = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();
                    reworkFlag = Convert.ToString(ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                    if (strParentStepKey == "" || strParentRouteKey == "" || strParentEnterpriseKey == "" || strParentStateFlag == "")
                    {
                        strParentStepKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                        strParentRouteKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
                        strParentEnterpriseKey = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                        strParentStateFlag = ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                    }
                }
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                //更新批次信息
                sql = string.Format(@"UPDATE POR_LOT 
                                    SET QUANTITY='{0}',EDITOR='{1}',EDIT_TIME= GETDATE(),EDIT_TIMEZONE='{2}' ",
                                    strRemnantQTY.PreventSQLInjection(), 
                                    strEditor.PreventSQLInjection(), 
                                    strEditTimeZone.PreventSQLInjection());
                //如果数量为0终止批次。
                if (strRemnantQTY == "0")
                {
                    sql += ",DELETED_TERM_FLAG=1 ";
                }
                sql+= string.Format("WHERE LOT_KEY='{0}'",strParentLotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                #region 记录批次拆分操作数据
                Hashtable parenttransactionTable = new Hashtable();
                DataTable parenttransaction = new DataTable();
                strParentTransKey = UtilHelper.GenerateNewKey(0);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, strParentLotKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, strParentQTY);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, strRemnantQTY);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SPLIT);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, strComment);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, strEditor);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, strEditTimeZone);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, strParentStateFlag);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, strEditor);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, strLineKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);

                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, parenttransactionTable, null);
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                #endregion

                #region 创建子批次。

                string strChildLotKey = "";
                string strChildLotNumber = "";
                string strChildQty = "";
                string strChildLotSEQ = "";
                //int intSEQ = 0;
                int intCount = 0;
                if (dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtReturn = new DataTable();
                    dtReturn.Columns.Add("ITEM_SEQ", Type.GetType("System.String"));
                    dtReturn.Columns.Add("LOT_NUMBER", Type.GetType("System.String"));
                    dtReturn.Columns.Add("QUANTITY", Type.GetType("System.String"));
                    dtReturn.Columns.Add("LOT_KEY", Type.GetType("System.String"));

                    intCount = strParentSEQ.Length;
                    //get exist max seq
                    sql = string.Format(@"SELECT MAX(A.WORK_ORDER_SEQ) AS WORK_ORDER_SEQ, 
	                                           SUBSTRING(MAX(A.WORK_ORDER_SEQ),{0},2) AS WORK_ORDER_STEP,
	                                           LEN(MAX(A.WORK_ORDER_SEQ)),A.WORK_ORDER_KEY 
                                        FROM POR_LOT A  
                                        WHERE A.WORK_ORDER_KEY = '{1}'
                                        AND A.WORK_ORDER_SEQ LIKE '{2}.%'
                                        GROUP BY A.WORK_ORDER_KEY",
                                        (intCount + 2), 
                                        strWorkOrderKey.PreventSQLInjection(), 
                                        strParentSEQ.PreventSQLInjection());
                    using (IDataReader readerSEQ = db.ExecuteReader(dbtran,CommandType.Text, sql))
                    {
                        if (readerSEQ.Read())
                        {
                            if (readerSEQ["WORK_ORDER_STEP"].ToString() != "")
                            {
                                intSEQ = Int32.Parse(readerSEQ["WORK_ORDER_STEP"].ToString());
                            }
                        }
                        readerSEQ.Close();
                    }
                    if (workOrderStep > intSEQ)
                    {
                        intSEQ = workOrderStep;
                    }
                    DataTable parameterdatatable = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                    for (int i = 0; i < parameterdatatable.Rows.Count; i++)
                    {
                        //get form collect data                                    
                        strChildLotKey = UtilHelper.GenerateNewKey(0);
                        strChildLotNumber = GetChildLotNumber(db,dbtran,strParentLotNumber,i+1);
                        strChildQty = parameterdatatable.Rows[i]["QUANTITY"].ToString();
                        strChildLotSEQ = parameterdatatable.Rows[i]["SPLIT_SEQ"].ToString();
                        dtReturn.Rows.Add(strChildLotSEQ, strChildLotNumber, strChildQty, strChildLotKey);

                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY] = strChildLotKey;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER] = strChildLotNumber;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY] = strChildQty;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME] = strEditTime;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CREATE_TIME] = strEditTime;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_START_WAIT_TIME] = strEditTime;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG] = strParentStateFlag;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY] = strParentEnterpriseKey;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY] = strParentRouteKey;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY] = strParentStepKey;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_IS_MAIN_LOT] = 0;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_OPR_COMPUTER] = operComputerName;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CREATOR] = strEditor;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_EDITOR] = strEditor;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE] = strEditTimeZone;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_CREATE_TIMEZONE_KEY] = strEditTimeZone;
                        ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_IS_PRINT] = "0";
                        if (intSEQ == 0)
                        {
                            //new SEQ
                            ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ] = strParentSEQ + "." + strChildLotSEQ;
                        }
                        else
                        {
                            //add SEQ
                            intSEQ = intSEQ + 1;
                            ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ] = strParentSEQ + "." + intSEQ.ToString("00");
                        }

                        //Create chilid Lot
                        if (ds.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                            ds.Tables.Remove(TRANS_TABLES.TABLE_MAIN_DATA);

                        #region Create Lot & Create Udas for the lot
                        Hashtable hashDataOfLot = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(ds.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0]);
                        //initialize tablefields
                        POR_LOT_FIELDS porLotFields = new POR_LOT_FIELDS();
                        POR_LOT_ATTR_FIELDS lotUda = new POR_LOT_ATTR_FIELDS();

                        //get sql
                        sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, hashDataOfLot, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //create udas for the child lot
                        if (ds.Tables.Contains(POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME))
                        {
                            List<string> sqlCommandList = new List<string>();
                            foreach (DataRow row in ds.Tables[POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME].Rows)
                            {
                                row[POR_LOT_ATTR_FIELDS.FIELD_LOT_KEY] = strChildLotKey;
                            }
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_LOT_ATTR_FIELDS(),
                                                                   ds.Tables[POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   {  
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME,null},
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() { BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE });

                            foreach (string sqlU in sqlCommandList)
                            {
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlU);
                            }
                        }
                        #endregion

                        #region Insert WipTransaction
                        //insert Wip transaction 
                        Hashtable childtransactionTable = new Hashtable();
                        DataTable childtransaction = new DataTable();
                        string strChildTransKey =UtilHelper.GenerateNewKey(0);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, strChildLotKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, strChildQty);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, strChildQty);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, strEditor);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, strEditTimeZone);

                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, strLineKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, strParentStateFlag);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, strEditor);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);

                        sql = DatabaseTable.BuildInsertSqlStatement(wipFields, childtransactionTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //insert wip split 
                        Hashtable splitHashTable = new Hashtable();
                        DataTable splitTable = new DataTable();
                        WIP_SPLIT_FIELDS wipSplit = new WIP_SPLIT_FIELDS();

                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_CHILD_LOT_KEY, strChildLotKey);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_CHILD_LOT_TRANSACTION_KEY, strChildTransKey);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_SPLIT_QUANTITY, strChildQty);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_STEP_KEY, strParentStepKey);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_ROUTE_KEY, strParentRouteKey);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_ENTERPRISE_KEY, strParentEnterpriseKey);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_EDITOR, strEditor);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_EDIT_TIME, null);
                        splitHashTable.Add(WIP_SPLIT_FIELDS.FIELD_EDIT_TIMEZONE, strEditTimeZone);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipSplit, splitHashTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        #endregion
                    }
                    dtReturn.TableName = "CHILDLOT_DATA";
                    dsReturn.Merge(dtReturn, false, MissingSchemaAction.Add);
                }
                #endregion

                #region 获取批次当前加工的设备数据
                sql =string.Format(@"SELECT EQUIPMENT_KEY,LOT_EQUIPMENT_KEY 
                                    FROM EMS_LOT_EQUIPMENT 
                                    WHERE LOT_KEY='{0}' AND STEP_KEY='{1}' AND END_TIMESTAMP IS NULL",
                                    strParentLotKey.PreventSQLInjection(),
                                    strParentStepKey.PreventSQLInjection());
                DataSet dsLotEquipment = db.ExecuteDataSet(dbtran,CommandType.Text, sql);
                if (dsLotEquipment.Tables[0].Rows.Count > 0)
                {
                    string equipmentKey = dsLotEquipment.Tables[0].Rows[0]["EQUIPMENT_KEY"].ToString();
                    string lotEquipmentKey = dsLotEquipment.Tables[0].Rows[0]["LOT_EQUIPMENT_KEY"].ToString();
                    if (dsReturn.Tables.Contains("CHILDLOT_DATA"))
                    {
                        for (int i = 0; i < dsReturn.Tables["CHILDLOT_DATA"].Rows.Count; i++)
                        {
                            string splitLotKey = dsReturn.Tables["CHILDLOT_DATA"].Rows[i]["LOT_KEY"].ToString();
                            int splitQuantity = Convert.ToInt32(dsReturn.Tables["CHILDLOT_DATA"].Rows[i]["QUANTITY"]);
                            splitQuantitySum += splitQuantity;
                            sql = string.Format(@"INSERT INTO EMS_LOT_EQUIPMENT
                                                (LOT_EQUIPMENT_KEY, LOT_KEY,EQUIPMENT_KEY, START_TIMESTAMP, USER_KEY, QUANTITY,STEP_KEY)
                                                VALUES('{0}','{1}','{2}',  GETDATE(), '{3}', {4},'{5}')", 
                                                UtilHelper.GenerateNewKey(0),
                                                splitLotKey.PreventSQLInjection(),
                                                equipmentKey.PreventSQLInjection(),
                                                strEditor.PreventSQLInjection(), 
                                                splitQuantity, 
                                                strParentStepKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        }
                    }
                    sql =string.Format(@"UPDATE EMS_LOT_EQUIPMENT 
                                        SET QUANTITY=QUANTITY-{0} 
                                        WHERE LOT_EQUIPMENT_KEY='{1}'",
                                        splitQuantitySum,
                                        lotEquipmentKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }
                #endregion
            }
            return intSEQ;
        }
        /// <summary>
        /// 根据父批号获取拆分后批号。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="strParentLotNumber">父批号。</param>
        /// <param name="index"></param>
        /// <returns>拆分后的子批号。</returns>
        private static string GetChildLotNumber(Database db, DbTransaction dbTrans, string strParentLotNumber, int index)
        {
            string strChildLotNumber = string.Empty;
            string strMaxNum = string.Empty;
            string sql = string.Empty;
            string strPrepositive = strParentLotNumber;
            sql = string.Format(@"SELECT ISNULL(MAX(SUBSTRING(A.LOT_NUMBER, LEN('{0}')+2,2)),0) AS MAX_NUM 
                                FROM POR_LOT A 
                                WHERE A.LOT_NUMBER LIKE '{0}%'", 
                                strPrepositive.PreventSQLInjection());
            object objMaxNum = db.ExecuteScalar(dbTrans, CommandType.Text, sql);
            int maxNum = 0;
            if (objMaxNum == null || objMaxNum == DBNull.Value)
            {
                maxNum = Convert.ToInt32(maxNum);
            }
            maxNum=maxNum+1;
            strMaxNum = maxNum.ToString().PadLeft(2, '0');
            strChildLotNumber = strPrepositive +"-" +strMaxNum;
            return strChildLotNumber;
        }
        
    }
}
