using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;

namespace FanHai.Hemera.Modules.FMM
{  
    /// <summary>
    /// 产品型号及产品设置操作类
    /// </summary>
    public class PlanAimEngine : AbstractEngine, IRptCommonEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PlanAimEngine()
        {
            db = DatabaseFactory.CreateDatabase("SQLServerHis");
        }
        /// <summary>
        /// 获取计划输入数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        /// genchille.yang 2012-10-30 12:15
        public DataSet GetRptPlanAimData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"select * from RPT_PLAN_AIM t
                                where t.ISFLAG=1 ";

                if (hstable.ContainsKey(RPT_PLAN_AIM.FIELDS_LOCATION_NAME))
                    sqlCommand += string.Format(" and t.LOCATION_NAME in ({0})", Convert.ToString(hstable[RPT_PLAN_AIM.FIELDS_LOCATION_NAME]));
                if (hstable.ContainsKey(RPT_PLAN_AIM.FIELDS_PART_TYPE))
                    sqlCommand += string.Format(" and t.PART_TYPE in ({0})", Convert.ToString(hstable[RPT_PLAN_AIM.FIELDS_PART_TYPE]));
                if (hstable.ContainsKey(RPT_PLAN_AIM.FIELDS_WORK_ORDER_NO))
                    sqlCommand += string.Format(" and t.WORK_ORDER_NO in ({0})", Convert.ToString(hstable[RPT_PLAN_AIM.FIELDS_WORK_ORDER_NO]));
                if (hstable.ContainsKey(RPT_PLAN_AIM.FIELDS_PRO_ID))
                    sqlCommand += string.Format("  and t.PRO_ID in ({0})", Convert.ToString(hstable[RPT_PLAN_AIM.FIELDS_PRO_ID]));
                if (hstable.ContainsKey(RPT_PLAN_AIM.FIELDS_PLAN_DATE_START))
                    sqlCommand += string.Format("  and t.PLAN_DATE_START >= CONVERT(datetime,'{0}')", Convert.ToString(hstable[RPT_PLAN_AIM.FIELDS_PLAN_DATE_START]));
                if (hstable.ContainsKey(RPT_PLAN_AIM.FIELDS_PLAN_DATE_END))
                    sqlCommand += string.Format("  and t.PLAN_DATE_START <= CONVERT(datetime,'{0}')", Convert.ToString(hstable[RPT_PLAN_AIM.FIELDS_PLAN_DATE_END]));

                sqlCommand += @" order by " + RPT_PLAN_AIM.FIELDS_PLAN_DATE_START + " asc," + RPT_PLAN_AIM.FIELDS_PART_TYPE + " asc,"
                    + RPT_PLAN_AIM.FIELDS_PRO_ID + " asc," + RPT_PLAN_AIM.FIELDS_WORK_ORDER_NO + " asc," + RPT_PLAN_AIM.FIELDS_LOCATION_NAME + " asc";

                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSet.Columns.Add(RPT_PLAN_AIM.FIELDS_ISNEW);
                dtPowerSet.TableName = RPT_PLAN_AIM.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRptPlanAimData Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 保存计划数据
        /// </summary>
        /// <param name="dsPowerSet"></param>
        /// <returns></returns>
        /// Owner genchille.yang 2013-1-20 17:51
        public DataSet SaveRptPlanAimData(DataSet dsPowerSet)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtplan_Update = null, dtplan_Insert = null;
                       
            List<string> sqlCommandList = new List<string>();
            if (dsPowerSet.Tables.Contains(RPT_PLAN_AIM.DATABASE_TABLE_FORINSERT))
            {
                dtplan_Insert = dsPowerSet.Tables[RPT_PLAN_AIM.DATABASE_TABLE_FORINSERT];
                if (dtplan_Insert.Columns.Contains(RPT_PLAN_AIM.FIELDS_ISNEW))
                    dtplan_Insert.Columns.Remove(RPT_PLAN_AIM.FIELDS_ISNEW);
            }

            if (dsPowerSet.Tables.Contains(RPT_PLAN_AIM.DATABASE_TABLE_FORUPDATE))
            {
                dtplan_Update = dsPowerSet.Tables[RPT_PLAN_AIM.DATABASE_TABLE_FORUPDATE];
                if (dtplan_Update.Columns.Contains(RPT_PLAN_AIM.FIELDS_ISNEW))
                    dtplan_Update.Columns.Remove(RPT_PLAN_AIM.FIELDS_ISNEW);
            }
           
                      
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                RPT_PLAN_AIM planaimFields = new RPT_PLAN_AIM();
              
                try
                {
                    if (dtplan_Insert != null && dtplan_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtplan_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(planaimFields, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtplan_Update != null && dtplan_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtplan_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(RPT_PLAN_AIM.FIELDS_PLANID, hashTable[RPT_PLAN_AIM.FIELDS_PLANID].ToString());
                            hashTable.Remove(RPT_PLAN_AIM.FIELDS_PLANID);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(planaimFields, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                  
                    
                   
                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveRptPlanAimData Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }
        /// <summary>
        /// 获得产品ID号，工单号，产品型号
        /// </summary>
        /// <returns></returns>
        public DataSet GetProWoModuleType()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                POR_PRODUCT _product = new POR_PRODUCT();
                POR_WORK_ORDER_FIELDS _workorder = new POR_WORK_ORDER_FIELDS();
                BASE_PRODUCTMODEL _promodel = new BASE_PRODUCTMODEL();

                Conditions _conditions = new Conditions();
                List<string> lst = new List<string>();
                lst.Add(POR_PRODUCT.FIELDS_PRODUCT_CODE);
                lst.Add(POR_PRODUCT.FIELDS_PRODUCT_KEY);
                _conditions.Add(DatabaseLogicOperator.And,POR_PRODUCT.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_product, lst, _conditions);
                sqlCommand += @" order by " + POR_PRODUCT.FIELDS_PRODUCT_CODE + " asc";
                DataTable dtProduct = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtProduct.TableName = POR_PRODUCT.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtProduct, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                lst = new List<string>();
                lst.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER);
                lst.Add(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY);
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_workorder, lst, _conditions);
                sqlCommand += @" order by " + POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + " asc";
                DataTable dtWorkOrder = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtWorkOrder.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtWorkOrder, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                lst = new List<string>();
                lst.Add(BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME);
                lst.Add(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY);
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_promodel, lst, _conditions);
                sqlCommand += @" order by " + BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME + " asc";
                DataTable dtModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtModel.TableName = BASE_PRODUCTMODEL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtModel, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetProWoModuleType Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取班别数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetShiftName()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = "select * from CAL_SHIFT";
            DataTable dtShift = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            dtShift.TableName = CAL_SHIFT.DATABASE_TABLE_NAME;
            dsReturn.Merge(dtShift, true, MissingSchemaAction.Add);

            return dsReturn;
        }
        public DataSet GetOperation()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = "select * from POR_ROUTE_OPERATION_VER t order by t.SORT_SEQ asc";
            DataTable dtShift = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            dtShift.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
            dsReturn.Merge(dtShift, true, MissingSchemaAction.Add);

            return dsReturn;
        }

        /// <summary>
        /// 获取线别
        /// </summary>
        /// <returns></returns>
        public DataSet GetLines()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = "SELECT DISTINCT A.LINE_CODE,A.LINE_NAME"
                               +" FROM FMM_PRODUCTION_LINE A"
                               +" INNER JOIN FMM_LOCATION_LINE B ON A.PRODUCTION_LINE_KEY = B.LINE_KEY"
                               +" INNER  JOIN FMM_LOCATION C ON C.LOCATION_KEY = B.LOCATION_KEY"
                               +" INNER JOIN EMS_EQUIPMENTS D ON D.LOCATION_KEY = C.LOCATION_KEY";
            DataTable dtShift = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            dtShift.TableName = RPT_PLAN_AIM.DATABASE_TABLE_NAME;
            dsReturn.Merge(dtShift, true, MissingSchemaAction.Add);
            return dsReturn;
        }

        /// <summary>
        /// 获取工序排班数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetOptSettingData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT [OPTSETTING_KEY],[LOCATION_KEY],[LOCATION_NAME],[OPERATION_KEY]
                                ,[OPERATION_NAME],[SHIFT_NAME],[START_TIME],[END_TIME],[OVER_DAY],[REMARK]
                                ,[CREATE_TIME],[CREATOR],[EDIT_TIME],[EDITOR],[ISFLAG],[SORT_SEQ],[SHIFT_KEY]
                                FROM [BASE_OPT_SETTING] where ISFLAG=1 ";

                if (hstable.ContainsKey(BASE_OPT_SETTING.FIELDS_LOCATION_NAME))
                    sqlCommand += string.Format(" and LOCATION_NAME in ({0})", Convert.ToString(hstable[BASE_OPT_SETTING.FIELDS_LOCATION_NAME]));

                if (hstable.ContainsKey(BASE_OPT_SETTING.FIELDS_OPERATION_NAME))
                    sqlCommand += string.Format(" and OPERATION_NAME in ({0})", Convert.ToString(hstable[BASE_OPT_SETTING.FIELDS_OPERATION_NAME]));

                sqlCommand += @" order by " + BASE_OPT_SETTING.FIELDS_SORT_SEQ + " asc, " + BASE_OPT_SETTING.FIELDS_EDIT_TIME + " desc";

                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                dtPowerSet.TableName = BASE_OPT_SETTING.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOptSettingData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet SaveOptSettingData(DataSet dsOptSetting)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtOptSetting_Update = null, dtOptSettings_Insert = null;

            List<string> sqlCommandList = new List<string>();
            if (dsOptSetting.Tables.Contains(BASE_OPT_SETTING.DATABASE_TABLE_FORINSERT))
            {
                dtOptSettings_Insert = dsOptSetting.Tables[BASE_OPT_SETTING.DATABASE_TABLE_FORINSERT];              
            }

            if (dsOptSetting.Tables.Contains(BASE_OPT_SETTING.DATABASE_TABLE_FORUPDATE))
            {
                dtOptSetting_Update = dsOptSetting.Tables[BASE_OPT_SETTING.DATABASE_TABLE_FORUPDATE];               
            }


            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                BASE_OPT_SETTING optSettingsFields = new BASE_OPT_SETTING();

                try
                {
                    if (dtOptSettings_Insert != null && dtOptSettings_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtOptSettings_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(optSettingsFields, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtOptSetting_Update != null && dtOptSetting_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtOptSetting_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_OPT_SETTING.FIELDS_OPTSETTING_KEY, hashTable[BASE_OPT_SETTING.FIELDS_OPTSETTING_KEY].ToString());
                            hashTable.Remove(BASE_OPT_SETTING.FIELDS_OPTSETTING_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(optSettingsFields, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }



                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveOptSettingData Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取厂别数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DataSet GetFactoryDate()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT LOCATION_KEY,LOCATION_NAME";
                sql += " FROM FMM_LOCATION";
                sql += " WHERE LOCATION_LEVEL=5";
                sql += " ORDER BY LOCATION_NAME ASC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFactoryDate Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取生产排班数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DataSet GetFactoryShiftData(string sFactoryShiftSetKey,string sFactoryKey,string sDate,string sShiftValue,string sFactoryShiftName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT FACTORYSHIFTSET_KEY,FACTORYROOM_KEY,FACTORYROOM_NAME,DATA_DATE,SHIFT_VALUE,SHIFT_NAME,FACTORYSHIFT_NAME";
                sql += " FROM BASE_FACTORYSHIFT_SET";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sFactoryShiftSetKey))
                {
                    sql += " AND FACTORYSHIFTSET_KEY='" + sFactoryShiftSetKey + "'";
                }
                if (!string.IsNullOrEmpty(sFactoryKey))
                {
                    sql += " AND FACTORYROOM_KEY='" + sFactoryKey + "'";
                }
                if (!string.IsNullOrEmpty(sDate))
                {
                    sql += " AND DATA_DATE='" + sDate + "'";
                }
                if (!string.IsNullOrEmpty(sShiftValue))
                {
                    sql += " AND SHIFT_VALUE='" + sShiftValue + "'";
                }
                if (!string.IsNullOrEmpty(sFactoryShiftName))
                {
                    sql += " AND FACTORYSHIFT_NAME='" + sFactoryShiftName + "'";
                }
                sql += " ORDER BY FACTORYROOM_KEY,DATA_DATE,SHIFT_VALUE,FACTORYSHIFT_NAME ASC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFactoryShiftData Error: " + ex.Message);
            }
            return dsReturn;
        }


        public DataSet UpdateData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    int irows = 0;
                    irows = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    dbTran.Commit();
                    dsReturn.ExtendedProperties.Add("rows", irows.ToString());
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError(sUpFuntionName + " Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
    }
}

