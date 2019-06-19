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
using Microsoft.Practices.EnterpriseLibrary.Common;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 工艺流程数据管理类。
    /// </summary>
    public class RouteEngine:AbstractEngine,IRouteEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RouteEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化函数。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 新增工艺流程。
        /// </summary>
        /// <param name="dsParams">包含工艺流程数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet RouteInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();

                    string strVersion = "1";
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME];
                    //获取版本号。
                    if (dtParams.Rows.Count == 1)
                    {
                        string routeName=Convert.ToString(dtParams.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                        sqlCommand =string.Format(@"SELECT MAX(ROUTE_VERSION)+1 AS ROUTE_VERSION 
                                                   FROM POR_ROUTE_ROUTE_VER 
                                                   WHERE ROUTE_NAME ='{0}'",routeName.PreventSQLInjection());
                        object objVersion = db.ExecuteScalar(CommandType.Text, sqlCommand);
                        if (objVersion != null && objVersion != DBNull.Value)
                        {
                            strVersion = Convert.ToString(objVersion);
                        }
                    }
                    //生成工艺流程INSERT SQL
                    List<string> sqlCommandRoute = new List<string>();
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandRoute,
                                                            new POR_ROUTE_ROUTE_VER_FIELDS(),
                                                            dsParams.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME],
                                                            new Dictionary<string, string>() 
                                                                { 
                                                                    {POR_ROUTE_ROUTE_VER_FIELDS.FIELD_CREATE_TIME, null},
                                                                    {POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIME, null},
                                                                    {POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_VERSION, strVersion}
                                                                },
                                                            new List<string>());
                   
                    if (1 == sqlCommandRoute.Count && sqlCommandRoute[0].Length > 20)
                    {
                        sqlCommandList.Add(sqlCommandRoute[0]);
                        //生成工步INSERT SQL
                        if (dsParams.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_ROUTE_STEP_FIELDS(),
                                                                   dsParams.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   { 
                                                                      {POR_ROUTE_STEP_FIELDS.FIELD_CREATE_TIME, null},
                                                                      {POR_ROUTE_STEP_FIELDS.FIELD_EDIT_TIME, null},
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        //生成工步属性INSERT SQL
                        if (dsParams.Tables.Contains(POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_ROUTE_STEP_ATTR_FIELDS(),
                                                                   dsParams.Tables[POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   {  
                                                                      {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME,null},
                                                                      {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        //生成工步参数INSERT SQL
                        if (dsParams.Tables.Contains(POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_ROUTE_STEP_PARAM_FIELDS(),
                                                                   dsParams.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>(),
                                                                   new List<string>());
                        }
                        //执行数据新增。
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
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, strVersion);
                            }
                            catch (Exception e)
                            {
                                dbTrans.Rollback();
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, -1, e.Message);
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
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0003}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("RouteInsert Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新工艺流程。
        /// </summary>
        /// <param name="dsParams">包含工艺流程的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet RouteUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams)
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    //生成更新工艺流程的SQL
                    if (dsParams.Tables.Contains(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                            new POR_ROUTE_ROUTE_VER_FIELDS(),
                                                            dsParams.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME],
                                                            new Dictionary<string, string>() 
                                                            { 
                                                                {POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIME,null}
                                                            },
                                                            new List<string>() { POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY },
                                                            POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY);
                    }
                    //生成操作工步的SQL语句
                    if (dsParams.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildSqlStatementsForDML(ref sqlCommandList, 
                                                             new POR_ROUTE_STEP_FIELDS(),
                                                             new POR_ROUTE_STEP_ATTR_FIELDS(),
                                                             dsParams.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME],
                                                             POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY);
                    }
                    //生成更新工步的SQL
                    if (dsParams.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                            new POR_ROUTE_STEP_FIELDS(),
                                                            dsParams.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"],
                                                            new Dictionary<string, string>() 
                                                            { 
                                                               {POR_ROUTE_STEP_FIELDS.FIELD_EDIT_TIME,null},
                                                            },
                                                            new List<string>() { POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY },
                                                            POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY);
                    }
                    //生成操作工步自定义属性的SQL
                    if (dsParams.Tables.Contains(POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList,
                                                                new POR_ROUTE_STEP_ATTR_FIELDS(),
                                                                dsParams.Tables[POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY);
                    }
                    //生成工步参数的SQL
                    if (dsParams.Tables.Contains(POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME))
                    {
                        POR_ROUTE_STEP_PARAM_FIELDS paramFileds = new POR_ROUTE_STEP_PARAM_FIELDS();
                        DataTable dtStepParams = dsParams.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME];
                        if (dtStepParams != null)
                        {
                            //新增
                            DataTable dtInsertStepParams = dtStepParams.GetChanges(DataRowState.Added);
                            if (dtInsertStepParams != null)
                            {
                                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                       paramFileds,
                                                                       dtInsertStepParams,
                                                                       new Dictionary<string, string>(),
                                                                       new List<string>());
                            }
                            //更新修改。
                            DataTable dtUpdateStepParams = dtStepParams.GetChanges(DataRowState.Modified);
                            if (dtUpdateStepParams != null)
                            {
                                foreach (DataRow dr in dtUpdateStepParams.Rows)
                                {
                                    Hashtable htStepParams = CommonUtils.ConvertRowToHashtable(dr);
                                    string stepParamKey = Convert.ToString(htStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_PARAM_KEY]);
                                    htStepParams.Remove(POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_PARAM_KEY);
                                    Conditions cons = new Conditions();
                                    cons.Add(DatabaseLogicOperator.And, POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_PARAM_KEY, DatabaseCompareOperator.Equal, stepParamKey);
                                    string updateSql = DatabaseTable.BuildUpdateSqlStatement(paramFileds, htStepParams, cons);
                                    sqlCommandList.Add(updateSql);
                                }
                            }
                        }
                    }
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
                LogService.LogError("RouteUpdate Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除工艺流程。
        /// </summary>
        /// <param name="routeKey">工艺流程主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet RouteDelete(string routeKey)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(routeKey))
                {
                    //删除工步属性。
                    sqlCommand = string.Format(@"DELETE FROM {0}  WHERE {1} IN (SELECT DISTINCT {2} FROM {3} WHERE {4} = '{5}')",
                                            POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY,
                                            POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY,
                                            POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY,
                                            routeKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    //删除工步参数。
                    sqlCommand = string.Format(@"DELETE FROM {0}  WHERE {1} IN (SELECT DISTINCT {2} FROM {3} WHERE {4} = '{5}')",
                                            POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_KEY,
                                            POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY,
                                            POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY,
                                            routeKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    //删除工步
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}= '{2}'",
                                            POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY,
                                            routeKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    //删除工艺流程
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}='{2}'",
                                            POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY,
                                            routeKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);

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
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0005}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("RouteDelete Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询工艺流程。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含工艺流程数据的数据集对象。</returns>
        public DataSet RouteSearch(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                if (dsParams!=null && dsParams.Tables.Contains(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string routeName = Convert.ToString(htParams[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    sqlCommand = string.Format(@"SELECT * 
                                                FROM POR_ROUTE_ROUTE_VER 
                                                WHERE ROUTE_NAME LIKE '%{0}%'",
                                                routeName.PreventSQLInjection());

                    if (htParams[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_VERSION] != null)
                    {
                        string version = Convert.ToString(htParams[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_VERSION]);
                        sqlCommand += "AND ROUTE_VERSION = '" + version.PreventSQLInjection() + "'";
                    }

                    sqlCommand += " AND ROUTE_STATUS <> 2 ORDER BY ROUTE_NAME";
                }
                else
                {
                    sqlCommand = "SELECT * FROM POR_ROUTE_ROUTE_VER WHERE ROUTE_STATUS <> '2' ORDER BY ROUTE_NAME";
                }

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("RouteSearch Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取不重复的工艺流程名。
        /// </summary>
        /// <returns>包含工艺流程名的数据集对象。</returns>
        public DataSet GetRouteName()
        {
            string sql = "";
            DataSet dsReturn = new DataSet();
            try
            {
                sql = @"SELECT DISTINCT ROUTE_NAME 
                        FROM POR_ROUTE_ROUTE_VER
                        WHERE ROUTE_NAME IS NOT NULL AND ROUTE_STATUS=1";
                dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRouteName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工艺流程主键获取工艺流程。
        /// </summary>
        /// <param name="routeKey">工艺流程主键。</param>
        /// <returns>包含工艺流程的数据集对象。</returns>
        public DataSet GetRouteByKey(string routeKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(routeKey))
                {

                    //获取工步属性。
                    string sqlCommand =string.Format(@"SELECT A.*, B.DATA_TYPE
                                                  FROM POR_ROUTE_STEP_ATTR A
                                                  LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                                  WHERE ROUTE_STEP_KEY IN (SELECT DISTINCT ROUTE_STEP_KEY
                                                                           FROM POR_ROUTE_STEP 
                                                                           WHERE ROUTE_ROUTE_VER_KEY = '{0}') 
                                                  ORDER BY 1",routeKey.PreventSQLInjection());

                    DataTable dtTable = new DataTable();
                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    //获取工步参数。
                    sqlCommand = string.Format(@"SELECT A.*
                                                FROM POR_ROUTE_STEP_PARAM A
                                                WHERE ROUTE_STEP_KEY IN (SELECT DISTINCT ROUTE_STEP_KEY
                                                                       FROM POR_ROUTE_STEP 
                                                                       WHERE ROUTE_ROUTE_VER_KEY = '{0}') 
                                                AND A.IS_DELETED=0
                                                ORDER BY ROUTE_STEP_KEY,PARAM_INDEX", 
                                                routeKey.PreventSQLInjection());
                    dtTable = new DataTable();
                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    //获取工步数据。
                    sqlCommand =string.Format(@"SELECT A.*, B.EDC_NAME, B.EDC_VERSION, C.SP_NAME, C.SP_VERSION,
                                                       D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                       E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME,
                                                       F.ENTERPRISE_NAME, F.ROUTE_NAME,F.ROUTE_STEP_NAME RE_ROUTE_STEP_NAME
                                                FROM POR_ROUTE_STEP A
                                                LEFT JOIN EDC_MAIN B ON B.EDC_KEY = A.EDC_LIST_KEY
                                                LEFT JOIN EDC_SP C ON C.SP_KEY = A.SAMPLING_KEY
                                                LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                                LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                                LEFT JOIN V_PROCESS_PLAN F ON A.RE_ROUTE_ENTERPRISE_VER_KEY = F.ROUTE_ENTERPRISE_VER_KEY
                                                                              AND A.RE_START_ROUTE_VER_KEY = F.ROUTE_ROUTE_VER_KEY
                                                                              AND A.RE_START_STEP_KEY = F.ROUTE_STEP_KEY
                                               WHERE A.ROUTE_ROUTE_VER_KEY = '{0}' 
                                               ORDER BY ROUTE_STEP_SEQ", routeKey.PreventSQLInjection());

                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    //获取工艺流程数据。
                    sqlCommand =string.Format(@"SELECT * FROM POR_ROUTE_ROUTE_VER
                                            WHERE ROUTE_ROUTE_VER_KEY = '{0}'
                                            ORDER BY 1",routeKey.PreventSQLInjection());

                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0006}");
                }

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRouteByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工艺流程。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -----------------------
        /// {route name}
        /// -----------------------
        /// </param>
        /// <returns>包含工艺流程的数据集对象。</returns>
        public DataSet GetMaxVerRoute(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //define sqlCommand 
            string sqlCommand = string.Empty;
            string routeName = string.Empty;
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    routeName = Convert.ToString(htParams[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                }

                sqlCommand =string.Format(@"SELECT A.ROUTE_ROUTE_VER_KEY,
                                                  A.DESCRIPTION,
                                                  A.ROUTE_VERSION,
                                                  A.ROUTE_NAME
                                            FROM POR_ROUTE_ROUTE_VER A
                                            INNER JOIN (SELECT B.ROUTE_NAME, MAX(B.ROUTE_VERSION) AS ROUTE_VERSION
                                                        FROM POR_ROUTE_ROUTE_VER B
                                                        WHERE B.ROUTE_STATUS = 1 AND ROUTE_NAME LIKE '%{0}%'
                                                        GROUP BY B.ROUTE_NAME) T ON A.ROUTE_NAME = T.ROUTE_NAME AND A.ROUTE_VERSION = T.ROUTE_VERSION
                                            ORDER BY 1",routeName.PreventSQLInjection());
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaxVerRoute Error: " + ex.Message);
            }
            return dsReturn;
        }
 
        /// <summary>
        /// 添加工艺流程和生产线的关联关系。
        /// </summary>
        /// <param name="dsParams">包含工艺流程和生产线关联的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddRouteLine(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains("param"))
                {
                    DataTable dtParams = dsParams.Tables["param"];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    using (DbConnection dbconn = db.CreateConnection())
                    {
                        //Open Connection
                        dbconn.Open();
                        //Create Transaction
                        DbTransaction dbtran = dbconn.BeginTransaction();
                        try
                        {
                            string routeKey = Convert.ToString(htParams["ROUTE_ROUTE_VER_KEY"]);
                            string lineKey = Convert.ToString(htParams["PRODUCTION_LINE_KEY"]);
                            string lineName = Convert.ToString(htParams["LINE_NAME"]);
                            string sql =string.Format(@"INSERT INTO POR_ROUTE_VER_LINE(ROUTE_ROUTE_VER_KEY, PRODUCTION_LINE_KEY, LINE_NAME)
                                                        VALUES('{0}','{1}','{2}')",
                                                        routeKey.PreventSQLInjection(),
                                                        lineKey.PreventSQLInjection(),
                                                        lineName.PreventSQLInjection());
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            //Commit Transaction
                            dbtran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            LogService.LogError("AddRouteLine Error: " + ex.Message);
                            //Rollback Transaction
                            dbtran.Rollback();
                        }
                        finally
                        {
                            dbtran = null;
                            //Close Connection
                            dbconn.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddRouteLine Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除工艺流程和生产线的关联关系。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ------------------------------
        /// {ROUTE_ROUTE_VER_KEY}
        /// {PRODUCTION_LINE_KEY}
        /// ------------------------------
        /// </param>
        /// <returns>DataSet</returns>
        public DataSet DeleteRouteLine(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains("param"))
                {
                    DataTable dtParams = dsParams.Tables["param"];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    using (DbConnection dbconn = db.CreateConnection())
                    {
                        //Open Connection
                        dbconn.Open();
                        //Create Transaction
                        DbTransaction dbtran = dbconn.BeginTransaction();
                        try
                        {
                            string routeKey = Convert.ToString(htParams["ROUTE_ROUTE_VER_KEY"]);
                            string lineKey = Convert.ToString(htParams["PRODUCTION_LINE_KEY"]);
                            string sql=string.Format(@"DELETE FROM POR_ROUTE_VER_LINE WHERE ROUTE_ROUTE_VER_KEY = '{0}' AND PRODUCTION_LINE_KEY = '{1}'",
                                                    routeKey.PreventSQLInjection(),
                                                    lineKey.PreventSQLInjection());

                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            //Commit Transaction
                            dbtran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            LogService.LogError("DeleteRouteLine Error: " + ex.Message);
                            //Rollback Transaction
                            dbtran.Rollback();
                        }
                        finally
                        {
                            dbtran = null;
                            //Close Connection
                            dbconn.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteRouteLine Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        ///  查询工艺流程和生产线的关联关系。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象，可以为null。
        /// ------------------------------
        /// {ROUTE_ROUTE_VER_KEY}
        /// {PRODUCTION_LINE_KEY}
        /// ------------------------------
        /// </param>
        /// <returns>包含工艺流程和生产线的关联关系的数据集对象。</returns>
        public DataSet SearchRouteLine(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                if (dsParams != null && dsParams.Tables.Contains("param"))
                {
                    DataTable dataTable = dsParams.Tables["param"];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string routeKey = Convert.ToString(hashData["ROUTE_ROUTE_VER_KEY"]);
                    sql = string.Format(@"SELECT A.ROUTE_ROUTE_VER_KEY,A.PRODUCTION_LINE_KEY,B.ROUTE_NAME,C.LINE_CODE,A.LINE_NAME,C.DESCRIPTIONS
                                        FROM POR_ROUTE_VER_LINE A
                                        LEFT JOIN POR_ROUTE_ROUTE_VER B ON A.ROUTE_ROUTE_VER_KEY = B.ROUTE_ROUTE_VER_KEY
                                        LEFT JOIN FMM_PRODUCTION_LINE C ON A.PRODUCTION_LINE_KEY = C.PRODUCTION_LINE_KEY
                                        AND A.ROUTE_ROUTE_VER_KEY = '{0}'",
                                        routeKey.PreventSQLInjection());
                    if (hashData["PRODUCTION_LINE_KEY"] != null)
                    {
                        string lineKey = Convert.ToString(hashData["PRODUCTION_LINE_KEY"]);
                        sql += "AND A.PRODUCTION_LINE_KEY ='" + lineKey.PreventSQLInjection() + "'";
                    }
                    sql += "ORDER BY A.LINE_NAME,A.PRODUCTION_LINE_KEY ";
                }
                else
                {
                    sql = @"SELECT A.ROUTE_ROUTE_VER_KEY,A.PRODUCTION_LINE_KEY,B.ROUTE_NAME,C.LINE_CODE,A.LINE_NAME,C.DESCRIPTIONS
                            FROM POR_ROUTE_VER_LINE A
                            LEFT JOIN POR_ROUTE_ROUTE_VER B ON A.ROUTE_ROUTE_VER_KEY = B.ROUTE_ROUTE_VER_KEY
                            LEFT JOIN FMM_PRODUCTION_LINE C ON A.PRODUCTION_LINE_KEY = C.PRODUCTION_LINE_KEY
                            ORDER BY A.LINE_NAME,A.PRODUCTION_LINE_KEY";
                }
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtTable.TableName = "POR_ROUTE_LINE";
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchRouteLine Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取已激活的工艺流程数据。
        /// </summary>
        /// <returns>
        /// 包含已激活的工艺流程数据的数据集对象。
        /// [ROUTE_ROUTE_VER_KEY,ROUTE_NAME,DESCRIPTION]
        /// </returns>
        public DataSet GetActivedRouteData()
        {
            LogService.LogInfo("GetActiveRouteData");

            string sql = @"SELECT ROUTE_ROUTE_VER_KEY,ROUTE_NAME,DESCRIPTION
                           FROM POR_ROUTE_ROUTE_VER
                           WHERE ROUTE_STATUS=1";
            DataSet dsReturn = new DataSet();
            try
            {
                dsReturn=db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetActivedRouteData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工艺流程主键获取工步数据。
        /// </summary>
        /// <param name="routeKey">工艺流程主键</param>
        /// <returns>
        /// 包含工步数据的数据集对象。
        /// [ROUTE_STEP_KEY,ROUTE_STEP_NAME,ROUTE_STEP_SEQ,ROUTE_OPERATION_VER_KEY]
        /// </returns>
        public DataSet GetRouteStepDataByRouteKey(string routeKey)
        {
            string sql = string.Format(@"SELECT ROUTE_STEP_KEY,ROUTE_STEP_NAME,ROUTE_STEP_SEQ,ROUTE_OPERATION_VER_KEY
                                        FROM POR_ROUTE_STEP
                                        WHERE ROUTE_ROUTE_VER_KEY='{0}'
                                        ORDER BY ROUTE_STEP_SEQ",routeKey);
            DataSet dsReturn = new DataSet();
            try
            {
                dsReturn=db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRouteStepDataByRouteKey Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据工步主键获取工步数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含工步数据的数据集对象。</returns>
        public DataSet GetStepDataByKey(string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取工步属性。
                string sqlCommand = string.Format(@"SELECT A.*, B.DATA_TYPE
                                              FROM POR_ROUTE_STEP_ATTR A
                                              LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                              WHERE A.ROUTE_STEP_KEY  = '{0}'
                                              ORDER BY 1", 
                                              stepKey.PreventSQLInjection());

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //获取工步参数。
                sqlCommand = string.Format(@"SELECT A.*
                                            FROM POR_ROUTE_STEP_PARAM A
                                            WHERE A.ROUTE_STEP_KEY = '{0}'
                                            AND A.IS_DELETED=0
                                            ORDER BY A.ROUTE_STEP_KEY,A.PARAM_INDEX",
                                            stepKey.PreventSQLInjection());
                dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //获取工步数据。
                sqlCommand = string.Format(@"SELECT A.*,
                                                   D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                   E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME,
                                                   F.ENTERPRISE_NAME, F.ROUTE_NAME,F.ROUTE_STEP_NAME RE_ROUTE_STEP_NAME
                                            FROM POR_ROUTE_STEP A
                                            LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                            LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                            LEFT JOIN V_PROCESS_PLAN F ON A.RE_ROUTE_ENTERPRISE_VER_KEY = F.ROUTE_ENTERPRISE_VER_KEY
                                                                          AND A.RE_START_ROUTE_VER_KEY = F.ROUTE_ROUTE_VER_KEY
                                                                          AND A.RE_START_STEP_KEY = F.ROUTE_STEP_KEY
                                           WHERE A.ROUTE_STEP_KEY = '{0}' 
                                           ORDER BY ROUTE_STEP_SEQ", stepKey.PreventSQLInjection());

                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStepDataByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工步主键获取工步及其工步参数数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="dctype">数据采集时刻。0:进站时采集 1：出站时采集</param>
        /// <returns>包含工步数据的数据集对象。</returns>
        public DataSet GetStepBaseDataAndParamDataByKey(string stepKey, int dctype)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取工步参数。
                string sqlCommand = string.Format(@"SELECT A.*
                                            FROM POR_ROUTE_STEP_PARAM A
                                            WHERE A.ROUTE_STEP_KEY = '{0}'
                                            AND A.IS_DELETED=0
                                            AND A.DC_TYPE={1}
                                            ORDER BY A.ROUTE_STEP_KEY,A.PARAM_INDEX",
                                            stepKey.PreventSQLInjection(),
                                            dctype);
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //获取工步数据。
                sqlCommand = string.Format(@"SELECT A.*,
                                                   D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                   E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME,
                                                   F.ENTERPRISE_NAME, F.ROUTE_NAME,F.ROUTE_STEP_NAME RE_ROUTE_STEP_NAME
                                            FROM POR_ROUTE_STEP A
                                            LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                            LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                            LEFT JOIN V_PROCESS_PLAN F ON A.RE_ROUTE_ENTERPRISE_VER_KEY = F.ROUTE_ENTERPRISE_VER_KEY
                                                                          AND A.RE_START_ROUTE_VER_KEY = F.ROUTE_ROUTE_VER_KEY
                                                                          AND A.RE_START_STEP_KEY = F.ROUTE_STEP_KEY
                                           WHERE A.ROUTE_STEP_KEY = '{0}' 
                                           ORDER BY ROUTE_STEP_SEQ", stepKey.PreventSQLInjection());

                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStepDataByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工步主键获取工步自定义数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含工步自定义数据的数据集对象。</returns>
        public DataSet GetStepUda(string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT A.*
                                            FROM   POR_ROUTE_STEP_ATTR A
                                            WHERE  A.ROUTE_STEP_KEY ='{0}'",
                                            stepKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStepUda Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取工步指定的自定义属性值。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="attributeName">自定义属性名。</param>
        /// <returns>自定义属性名对应的属性值。</returns>
        public string GetStepUdaValue(string stepKey, string attributeName)
        {
            string strReturn = string.Empty;
            string sql = string.Format(@"SELECT A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE
                                        FROM   POR_ROUTE_STEP_ATTR A
                                        WHERE  A.ROUTE_STEP_KEY ='{0}' AND A.ATTRIBUTE_NAME='{1}'",
                                        stepKey.PreventSQLInjection(),
                                        attributeName.PreventSQLInjection());
            DataSet attributeData = db.ExecuteDataSet(CommandType.Text, sql);
            if (attributeData.Tables[0] != null && attributeData.Tables[0].Rows.Count > 0)
            {
                strReturn = attributeData.Tables[0].Rows[0]["ATTRIBUTE_VALUE"].ToString();
            }
            return strReturn;
        }
    }
}
