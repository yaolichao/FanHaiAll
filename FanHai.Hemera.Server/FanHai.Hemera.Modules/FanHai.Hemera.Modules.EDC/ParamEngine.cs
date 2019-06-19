#region using
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;

using Microsoft.Practices.EnterpriseLibrary.Data;
#endregion


namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 采集参数的数据管理类。
    /// </summary>
    public class ParamEngine : AbstractEngine, IParamEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ParamEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 新增采集参数。
        /// </summary>
        /// <param name="dataSet">包含采集参数的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet ParamInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    //生成INSERT 采集参数 SQL
                    List<string> sqlCommandParam = new List<string>();
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandParam,
                                                        new BASE_PARAMETER_FIELDS(),
                                                        dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        { 
                                                            {COMMON_FIELDS.FIELD_COMMON_CREATE_TIME,null},
                                                            {COMMON_FIELDS.FIELD_COMMON_CREATE_TIMEZONE, "CN-ZH"},
                                                            {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                                            {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"},
                                                        },
                                                        new List<string>());

                    if (1 == sqlCommandParam.Count && sqlCommandParam[0].Length > 20)
                    {
                        sqlCommandList.Add(sqlCommandParam[0]);
                        //生成采集参数计算公式的 INSERT SQL
                        if (dsParams.Tables.Contains(BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new BASE_PARAMETER_DERIVTION_FIELDS(),
                                                                   dsParams.Tables[BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                    { 
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"},
                                                                    },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        //执行数据插入。
                        if (sqlCommandList.Count > 0)
                        {
                            DbConnection dbConn = db.CreateConnection();
                            dbConn.Open();
                            DbTransaction dbTrans = dbConn.BeginTransaction();
                            try
                            {
                                foreach (string sql in sqlCommandList)
                                {
                                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                                }
                                dbTrans.Commit();
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, "");
                            }
                            catch (Exception e)
                            {
                                dbTrans.Rollback();
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, -1, e.Message);
                                LogService.LogError("ParamInsert Error: " + e.Message);
                            }
                            finally
                            {
                                dbTrans = null;
                                dbConn.Close();
                                dbConn = null;
                            }
                        }
                    }
                    else
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0002}");
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ParamInsert Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除采集参数。
        /// </summary>
        /// <param name="paramKey">采集参数主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteParam(string paramKey)
        {
            DataSet dsReturn =  new DataSet();
            //define sql 
            List<string> sqlCommand = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(paramKey))
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            sqlCommand.Add(string.Format(@"DELETE BASE_PARAMETER_DERIVTION WHERE DERIVATION_KEY  = '{0}'", paramKey.PreventSQLInjection()));
                            sqlCommand.Add(string.Format(@"DELETE BASE_PARAMETER WHERE PARAM_KEY = '{0}'", paramKey.PreventSQLInjection()));

                            foreach (string sql in sqlCommand)
                            {
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            }
                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            dbTran.Rollback();
                            LogService.LogError("DeleteParam Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran=null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteParam Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询采集参数。
        /// </summary>
        /// <param name="dataset">
        /// 包含查询条件的数据集对象。
        /// ----------------------------------
        /// {PARAM_NAME}
        /// ----------------------------------
        /// </param>
        /// <returns>包含采集参数的数据集对象。</returns>
        public DataSet SearchParam(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    string paramName=Convert.ToString(htParams[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME]);
                    
                    sbSql.AppendFormat(@"SELECT * FROM BASE_PARAMETER
                                         WHERE PARAM_NAME LIKE '%{0}%' AND STATUS<> 2",
                                         paramName.PreventSQLInjection());
                    if (htParams.ContainsKey(BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY))
                    {
                        string category=Convert.ToString(htParams[BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY]);
                        sbSql.AppendFormat(" AND PARAM_CATEGORY='{0}'", category.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(BASE_PARAMETER_FIELDS.FIELD_STATUS))
                    {
                        string status = Convert.ToString(htParams[BASE_PARAMETER_FIELDS.FIELD_STATUS]);
                        sbSql.AppendFormat(" AND STATUS='{0}'", status.PreventSQLInjection());
                    }
                    sbSql.Append(" ORDER BY PARAM_NAME");
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                dsReturn.Tables[0].TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchParam Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据采集参数主键获取用于计算该参数的采集参数。
        /// </summary>
        /// <param name="paramKey">被计算的采集参数主键。 </param>
        /// <returns>
        /// 包含参与计算的采集参数的数据集对象。。
        /// 【ROW_KEY,DERIVATION_KEY,PARAM_KEY,PARAM_NAME】
        /// </returns>
        public DataSet GetParamDerivationByKey(string paramKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(paramKey))
                {
                    sqlCommand = string.Format(@"SELECT ROW_KEY,DERIVATION_KEY,PARAM_KEY,PARAM_NAME
                                                FROM BASE_PARAMETER_DERIVTION
                                                WHERE DERIVATION_KEY = '{0}'",
                                                paramKey.PreventSQLInjection());

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetParamDerivationByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据采集参数主键获取采集参数及其对应的计算数据
        /// </summary>
        /// <param name="paramKey">采集参数主键。</param>
        /// <returns>包含采集参数及其对应的计算数据的数据集对象。</returns>
        public DataSet GetParamByKey(string paramKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(paramKey))
                {

                    sqlCommand = string.Format(@"SELECT * FROM BASE_PARAMETER WHERE PARAM_KEY = '{0}'", paramKey.PreventSQLInjection());
                    DataTable dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);


                    sqlCommand = string.Format(@"SELECT * FROM BASE_PARAMETER_DERIVTION WHERE DERIVATION_KEY = '{0}'", paramKey.PreventSQLInjection());
                    DataTable dtTable1 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable1.TableName = BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable1, false, MissingSchemaAction.Add);
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetParamByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不重复的采集参数名称。
        /// </summary>
        /// <returns>包含采集参数名称的数据集对象。</returns>
        public DataSet GetDistinctParamName()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = @"SELECT DISTINCT PARAM_KEY,PARAM_NAME
                                      FROM BASE_PARAMETER
                                      WHERE STATUS <> 2 
                                      ORDER BY PARAM_NAME";

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctParamName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新采集参数及其对应的计算数据。
        /// </summary>
        /// <param name="dsParams">包含采集参数及其对应的计算数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet ParamUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams)
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    //更新采集参数。
                    if (dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new BASE_PARAMETER_FIELDS(),
                            dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME],
                              new Dictionary<string, string>() 
                                  { 
                                      {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                      {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"},
                                  },
                            new List<string>() { BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY },
                            BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY);
                    }
                    //更新参数的计算数据。
                    if (dsParams.Tables.Contains(BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildDMLSqlStatements(ref sqlCommandList,
                            new BASE_PARAMETER_DERIVTION_FIELDS(),
                            dsParams.Tables[BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME],
                             new Dictionary<string, string>() 
                                  { 
                                      {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                      {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"},
                                  },
                            new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION },
                            BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_KEY);
                    }
                    //执行数据更新。
                    if (sqlCommandList.Count > 0)
                    {
                        DbConnection dbConn = db.CreateConnection();
                        dbConn.Open();
                        DbTransaction dbTrans = dbConn.BeginTransaction();
                        try
                        {
                            foreach (string sql in sqlCommandList)
                            {
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                            }
                            dbTrans.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception e)
                        {
                            dbTrans.Rollback();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, e.Message);
                            LogService.LogError("ParamUpdate Error: " + e.Message);
                        }
                        finally
                        {
                            dbTrans = null;
                            dbConn.Close();
                            dbConn = null;
                        }
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0007}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ParamUpdate Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetBaseParamsByCategory()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = @" select * from BASE_PARAMETER T
                                     WHERE T.PARAM_CATEGORY='5'
                                     AND T.STATUS<>2";

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBaseParamsByCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
