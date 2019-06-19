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
using Microsoft.Practices.EnterpriseLibrary.Common;
using FanHai.Hemera.Share.Common;
#endregion

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 工艺流程组数据管理类。
    /// </summary>
    public class EnterpriseEngine : AbstractEngine, IEnterpriseEngine
    {
        private Database db;   //数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EnterpriseEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 新增工艺流程组。
        /// </summary>
        /// <param name="dsParams">包含工艺流程组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。/returns>
        public DataSet EnterpriseInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    string strVersion = "1";
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME];
                    if (dtParams.Rows.Count == 1)
                    {
                        //获取版本号。
                        string enterpriseName = Convert.ToString(dtParams.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                        sqlCommand = string.Format(@"SELECT MAX(ENTERPRISE_VERSION)+1 AS ENTERPRISE_VERSION 
                                                   FROM POR_ROUTE_ENTERPRISE_VER 
                                                   WHERE ENTERPRISE_NAME ='{0}'",
                                                   enterpriseName.PreventSQLInjection());

                        IDataReader readerVersion = db.ExecuteReader(CommandType.Text, sqlCommand);
                        try
                        {
                            if (readerVersion.Read())
                            {
                                if (readerVersion[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION].ToString() != "")
                                {
                                    strVersion = readerVersion[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION].ToString();
                                }
                            }
                            else
                            {
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0001}");
                                return dsReturn;
                            }
                        }
                        finally
                        {
                            readerVersion.Close();
                            readerVersion.Dispose();
                            readerVersion = null;
                        }
                    }
                    //生成工艺流程组INSERT SQL
                    List<string> sqlCommandEnterprise = new List<string>();
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandEnterprise,
                                                           new POR_ROUTE_ENTERPRISE_VER_FIELDS(),
                                                           dsParams.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME],
                                                           new Dictionary<string, string>() 
                                                           { 
                                                               {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_CREATE_TIME, null},
                                                               {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_CREATE_TIMEZONE, "CN-ZH"},
                                                               {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIME, null},
                                                               {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"},
                                                               {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION, strVersion}
                                                           },
                                                           new List<string>());
                    //生成工艺流程组和工艺流程关联关系的INSERT SQL
                    if (1 == sqlCommandEnterprise.Count && sqlCommandEnterprise[0].Length > 20)
                    {
                        sqlCommandList.Add(sqlCommandEnterprise[0]);

                        if (dsParams.Tables.Contains(POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_ROUTE_EP_VER_R_VER_FIELDS(),
                                                                   dsParams.Tables[POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>(),
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
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
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("EnterpriseInsert Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除工艺流程组。
        /// </summary>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet EnterpriseDelete(string enterpriseKey)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(enterpriseKey))
                {
                    //删除工艺流程组合工艺流程的关联关系
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}='{2}'",
                                            POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME,
                                            POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY,
                                            enterpriseKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    //删除工艺流程组
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}='{2}'",
                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME,
                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY,
                                           enterpriseKey.PreventSQLInjection());
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
                LogService.LogError("EnterpriseDelete Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询工艺流程组。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet EnterpriseSearch(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string enterpriseName = Convert.ToString(htParams[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);

                    sqlCommand = @"SELECT * FROM POR_ROUTE_ENTERPRISE_VER WHERE ENTERPRISE_NAME 
                             LIKE '%" + enterpriseName.PreventSQLInjection() + "%'";

                    if (htParams[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION] != null)
                    {
                        string version = Convert.ToString(htParams[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION]);
                        sqlCommand += "AND ENTERPRISE_VERSION = '" + version.PreventSQLInjection() + "'";
                    }

                    sqlCommand += " AND ENTERPRISE_STATUS <> 2 ORDER BY ENTERPRISE_NAME";
                }
                else
                {
                    sqlCommand = "SELECT * FROM POR_ROUTE_ENTERPRISE_VER WHERE ENTERPRISE_STATUS <> '2' ORDER BY ENTERPRISE_NAME";
                }

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("EnterpriseSearch Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工艺流程组主键获取工艺流程组数据。
        /// </summary>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <returns>包含工艺流程组数据的数据集对象。</returns>
        public DataSet GetEnterpriseByKey(string enterpriseKey)
        {
            DataSet dsReturn = new DataSet();
            //define sqlCommand 
            String[] sqlCommand = new String[2];
            try
            {
                if (!string.IsNullOrEmpty(enterpriseKey))
                {

                    sqlCommand[0] = string.Format(@"SELECT A.*, B.ROUTE_NAME, B.DESCRIPTION
                                                FROM POR_ROUTE_EP_VER_R_VER A, POR_ROUTE_ROUTE_VER B
                                                WHERE A.ROUTE_ROUTE_VER_KEY = B.ROUTE_ROUTE_VER_KEY
                                                AND A.ROUTE_ENTERPRISE_VER_KEY = '{0}'
                                                ORDER BY A.ROUTE_SEQ",
                                                enterpriseKey.PreventSQLInjection());

                    sqlCommand[1] = string.Format(@"SELECT * 
                                                FROM POR_ROUTE_ENTERPRISE_VER
                                                WHERE ROUTE_ENTERPRISE_VER_KEY = '{0}' 
                                                ORDER BY ENTERPRISE_NAME",
                                                enterpriseKey.PreventSQLInjection());

                    DataTable dtTable = new DataTable();
                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand[0]).Tables[0];
                    dtTable.TableName = POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);

                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand[1]).Tables[0];
                    dtTable.TableName = POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    //add paramter table
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
                LogService.LogError("GetEnterpriseByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新工艺流程组数据。
        /// </summary>
        /// <param name="dsParams">包含工艺流程组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet EnterpriseUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams)
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    //生成更新SQL
                    if (dsParams.Tables.Contains(POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                            new POR_ROUTE_ENTERPRISE_VER_FIELDS(),
                                                            dsParams.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME],
                                                            new Dictionary<string, string>() 
                                                            { 
                                                                {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIME, null},
                                                                {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                            },
                                                            new List<string>() { POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY },
                                                            POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY);
                    }

                    if (dsParams.Tables.Contains(POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildSqlStatementsForDML(ref sqlCommandList,
                                                            new POR_ROUTE_EP_VER_R_VER_FIELDS(), null,
                                                            dsParams.Tables[POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME],
                                                            POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQUENCE_KEY);
                    }
                    if (dsParams.Tables.Contains(POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                               new POR_ROUTE_EP_VER_R_VER_FIELDS(),
                                                                dsParams.Tables[POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"],
                                                                new Dictionary<string, string>(),
                                                                new List<string>() { POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQUENCE_KEY },
                                                                POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQUENCE_KEY);
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
                LogService.LogError("EnterpriseUpdate Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不重复的工艺流程组名称。
        /// </summary>
        /// <returns>包含工艺流程组名称的数据集对象。</returns>
        public DataSet GetDistinctEnterpriseName()
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT DISTINCT ENTERPRISE_NAME 
                        FROM POR_ROUTE_ENTERPRISE_VER
                        WHERE ENTERPRISE_NAME IS NOT NULL AND ENTERPRISE_STATUS=1";
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtTable.TableName = "POR_ROUTE_ENTERPRISE_VER";
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctEnterpriseName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取工艺流程组信息。
        /// </summary>
        /// <returns>包含最高版本工艺流程组数据的数据集对象。</returns>
        public DataSet GetHelpInfoForEnterprise()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT ROUTE_ENTERPRISE_VER_KEY,B.ENTERPRISE_NAME AS ENTERPRISE_NAME ,B.ENTERPRISE_VERSION AS ENTERPRISE_VERSION 
                               FROM POR_ROUTE_ENTERPRISE_VER A 
                               RIGHT JOIN 
                               (SELECT ENTERPRISE_NAME, MAX(ENTERPRISE_VERSION) AS ENTERPRISE_VERSION FROM POR_ROUTE_ENTERPRISE_VER GROUP BY ENTERPRISE_NAME ) B   
                               ON A.ENTERPRISE_VERSION = B.ENTERPRISE_VERSION AND A.ENTERPRISE_NAME =B.ENTERPRISE_NAME
                               ORDER BY B.ENTERPRISE_NAME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetHelpInfoForEnterprise Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据车间名称获取工艺流程组信息。
        /// </summary>
        /// <param name="roomName">车间名称。</param>
        /// <returns>包含最高版本工艺流程数据的数据集对象。</returns>
        public DataSet GetHelpInfoForEnterprise(string roomName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT ROUTE_ENTERPRISE_VER_KEY,B.ENTERPRISE_NAME AS ENTERPRISE_NAME ,B.ENTERPRISE_VERSION AS ENTERPRISE_VERSION 
                                           FROM POR_ROUTE_ENTERPRISE_VER A 
                                           RIGHT JOIN 
                                                (SELECT ENTERPRISE_NAME, MAX(ENTERPRISE_VERSION) AS ENTERPRISE_VERSION FROM POR_ROUTE_ENTERPRISE_VER GROUP BY ENTERPRISE_NAME ) B   
                                                ON A.ENTERPRISE_VERSION = B.ENTERPRISE_VERSION AND A.ENTERPRISE_NAME =B.ENTERPRISE_NAME
                                           WHERE EXISTS (SELECT aa.ENTERPRISE_NAME FROM V_PROCESS_PLAN aa
                                                        LEFT JOIN POR_ROUTE_PART_RET bb ON aa.ROUTE_NAME=bb.ROUTE_NAME
                                                        WHERE aa.ENTERPRISE_NAME=A.ENTERPRISE_NAME AND bb.ROOM_NAME='{0}')
                                           ORDER BY B.ENTERPRISE_NAME",
                                           roomName.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetHelpInfoForEnterprise Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取指定工艺流程组内的工艺流程信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含工艺流程组主键的数据集对象。
        /// ------------------------------
        /// {EnterpriseKey}
        /// ------------------------------
        /// </param>
        /// <returns>包含最高版本工艺流程数据的数据集对象。</returns>
        public DataSet GetHelpInfoForEnterpriserRout(DataSet dsParams)
        {
            string enterpriseKey = Convert.ToString(dsParams.Tables[0].Rows[0][1]);
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT D.ROUTE_ROUTE_VER_KEY,D.ROUTE_NAME AS ROUTE_NAME ,D.ROUTE_VERSION AS ROUTE_VERSION ,E.ROUTE_SEQ 
                                        FROM POR_ROUTE_ROUTE_VER D ,POR_ROUTE_EP_VER_R_VER E 
                                        WHERE D.ROUTE_ROUTE_VER_KEY=E.ROUTE_ROUTE_VER_KEY AND E.ROUTE_ENTERPRISE_VER_KEY='{0}'
                                        ORDER BY E.ROUTE_SEQ ", enterpriseKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetHelpInfoForEnterpriserRout Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取指定工艺流程内的工步信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含工艺流程主键的数据集对象。
        /// ------------------------------
        /// {RouteKey}
        /// ------------------------------
        /// </param>
        /// <returns>包含最高版本工艺流程内工步数据的数据集对象。</returns>
        public DataSet GetHelpInfoForEnterpriserRoutStep(DataSet dsParams)
        {
            string routeKey = Convert.ToString(dsParams.Tables[0].Rows[0][1]);
            //define return dataset
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT ROUTE_STEP_KEY,ROUTE_STEP_NAME 
                                            FROM POR_ROUTE_STEP 
                                            WHERE ROUTE_ROUTE_VER_KEY = '{0}'
                                            ORDER BY ROUTE_STEP_SEQ",
                                            routeKey.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetHelpInfoForEnterpriserRoutStep Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取当前工艺流程工步的前一工步。
        /// </summary>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <param name="routeKey">ROUTE_ROUTE_VER_KEY（工艺流程主键）</param>
        /// <param name="stepKey">ROUTE_STEP_KEY（工步主键）。</param>
        /// <returns>包含前一工步数据的数据集对象。</returns>
        public DataSet GetEnterprisePreviousRouteAndStep(string enterpriseKey, string routeKey, string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (string.IsNullOrEmpty(enterpriseKey) || string.IsNullOrEmpty(routeKey) || string.IsNullOrEmpty(stepKey))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数为空，请检查。");
                    LogService.LogError(
                                        string.Format(
                                        "GetEnterprisePreviousRouteAndStep Error: 传入参数为空，请检查。EnterpriseKey:{0};RouteKey:{1};StepKey:{2}。",
                                        enterpriseKey, routeKey, stepKey)
                                       );
                    return dsReturn;
                }

                string sql = string.Format(@"WITH ROUTE AS
                                            (
                                                SELECT ROW_NUMBER() OVER (ORDER BY ROUTE_SEQ,ROUTE_STEP_SEQ) AS ROWNUMBER,
                                                        ROUTE_ENTERPRISE_VER_KEY,
                                                        ROUTE_ROUTE_VER_KEY,
                                                        ROUTE_STEP_KEY,
                                                        ENTERPRISE_NAME,
                                                        ROUTE_NAME,
                                                        ROUTE_STEP_NAME
                                                FROM V_PROCESS_PLAN 
                                                WHERE ROUTE_ENTERPRISE_VER_KEY='{0}'
                                            )
                                            SELECT ROUTE_ENTERPRISE_VER_KEY,ENTERPRISE_NAME,
	                                               ROUTE_ROUTE_VER_KEY,ROUTE_NAME,
	                                               ROUTE_STEP_KEY,ROUTE_STEP_NAME  
                                            FROM ROUTE a WHERE a.ROWNUMBER=  (SELECT b.ROWNUMBER 
                                                                              FROM ROUTE b 
                                                                              WHERE b.ROUTE_ROUTE_VER_KEY='{1}'
                                                                              AND b.ROUTE_STEP_KEY='{2}')-1",
                                            enterpriseKey.PreventSQLInjection(),
                                            routeKey.PreventSQLInjection(),
                                            stepKey.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEnterprisePreviousRouteAndStep Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取当前工艺流程工步的下一工步。
        /// </summary>
        /// <remarks>
        /// 如果当前工步在当前工艺流程是最后一个工步但所属工艺流程组有下一工艺流程，则下一工步为下一工艺流程的第一个工步。
        /// </remarks>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <param name="routeKey">ROUTE_ROUTE_VER_KEY（工艺流程主键）</param>
        /// <param name="stepKey">ROUTE_STEP_KEY（工步主键）。</param>
        /// <returns>包含下一工步数据的数据集对象。</returns>
        public DataSet GetEnterpriseNextRouteAndStep(string enterpriseKey, string routeKey, string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (string.IsNullOrEmpty(enterpriseKey) || string.IsNullOrEmpty(routeKey) || string.IsNullOrEmpty(stepKey))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数为空，请检查。");
                    LogService.LogError(
                                        string.Format(
                                        "GetEnterpriseNextRouteAndStep Error: 传入参数为空，请检查。EnterpriseKey:{0};RouteKey:{1};StepKey:{2}。",
                                        enterpriseKey, routeKey, stepKey)
                                       );
                    return dsReturn;
                }

                string sql = string.Format(@"WITH ROUTE AS
                                            (
                                                SELECT ROW_NUMBER() OVER (ORDER BY ROUTE_SEQ,ROUTE_STEP_SEQ) AS ROWNUMBER,
                                                        ROUTE_ENTERPRISE_VER_KEY,
                                                        ROUTE_ROUTE_VER_KEY,
                                                        ROUTE_STEP_KEY,
                                                        ENTERPRISE_NAME,
                                                        ROUTE_NAME,
                                                        ROUTE_STEP_NAME
                                                FROM V_PROCESS_PLAN 
                                                WHERE ROUTE_ENTERPRISE_VER_KEY='{0}'
                                            )
                                            SELECT ROUTE_ENTERPRISE_VER_KEY,ENTERPRISE_NAME,
	                                               ROUTE_ROUTE_VER_KEY,ROUTE_NAME,
	                                               ROUTE_STEP_KEY,ROUTE_STEP_NAME  
                                            FROM ROUTE a WHERE a.ROWNUMBER=1+(SELECT b.ROWNUMBER 
                                                                              FROM ROUTE b 
                                                                              WHERE b.ROUTE_ROUTE_VER_KEY='{1}'
                                                                              AND b.ROUTE_STEP_KEY='{2}')",
                                            enterpriseKey.PreventSQLInjection(),
                                            routeKey.PreventSQLInjection(),
                                            stepKey.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEnterpriseNextRouteAndStep Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取工艺流程信息。
        /// </summary>
        /// <returns>包含工艺流程信息的数据集。</returns>
        public DataSet GetProcessPlan()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT A.* 
                               FROM V_PROCESS_PLAN A
                               ORDER BY A.ENTERPRISE_NAME,A.ROUTE_SEQ,A.ROUTE_STEP_SEQ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工厂车间名称和产品类型获取工艺流程信息。
        /// </summary>
        /// <param name="factoryName">车间名称。</param>
        /// <param name="productType">成品类型。</param>
        /// <param name="isRework">重工标记.true:重工。false:正常。</param>
        /// <returns>包含工艺流程信息的数据集。</returns>
        public DataSet GetProcessPlan(string factoryName, string productType, bool isRework)
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            string strCondition = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(factoryName))
                {
                    sql = string.Format(@"SELECT DISTINCT ROUTE_NAME 
                                    FROM POR_ROUTE_PART_RET
                                    WHERE PART_TYPE='{0}' AND ISREWORK={1} AND ISSHOW = 1",
                                        productType.PreventSQLInjection(),
                                        isRework ? "1" : "0");
                }
                else
                {
                    sql = string.Format(@"SELECT DISTINCT ROUTE_NAME 
                                    FROM POR_ROUTE_PART_RET
                                    WHERE ROOM_NAME='{0}' AND PART_TYPE='{1}' AND ISREWORK={2} AND ISSHOW = 1",
                                                           factoryName.PreventSQLInjection(),
                                                           productType.PreventSQLInjection(),
                                                           isRework ? "1" : "0");
                }
                DataSet dsRoutePart = db.ExecuteDataSet(CommandType.Text, sql);

                if (dsRoutePart.Tables[0].Rows.Count < 1)
                {
                    strCondition = " 1!=1";
                }
                else if (dsRoutePart.Tables[0].Rows.Count == 1)
                {
                    string routeName = Convert.ToString(dsRoutePart.Tables[0].Rows[0]["ROUTE_NAME"]);
                    strCondition = " ROUTE_NAME='" + routeName.PreventSQLInjection() + "'";
                }
                if (dsRoutePart.Tables[0].Rows.Count > 1)
                {
                    strCondition = " ROUTE_NAME in ( ";
                    for (int i = 0; i < dsRoutePart.Tables[0].Rows.Count; i++)
                    {
                        string routeName = Convert.ToString(dsRoutePart.Tables[0].Rows[i]["ROUTE_NAME"]);
                        strCondition = strCondition + "'" + routeName.PreventSQLInjection() + "',";
                    }
                    //去了最后一个逗号，加上括号
                    strCondition = strCondition.TrimEnd(',');
                    strCondition = strCondition + " )";
                }
                sql = @"SELECT A.* 
                        FROM V_PROCESS_PLAN A 
                        WHERE {0} 
                        AND A.ENTERPRISE_STATUS=1 AND A.ROUTE_STATUS=1
                        ORDER BY A.ENTERPRISE_NAME,A.ROUTE_SEQ,A.ROUTE_STEP_SEQ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, String.Format(sql, strCondition));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工厂车间名称和产品类型获取首工序的工艺流程信息。
        /// </summary>
        /// <param name="factoryName">车间名称。</param>
        /// <param name="productType">成品类型。</param>
        /// <param name="isRework">重工标记.true:重工。false:正常。</param>
        /// <returns>包含工艺流程首工序信息的数据集。</returns>
        public DataSet GetProcessPlanFirstOperation(string factoryName, string productType, bool isRework)
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            string strCondition = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(factoryName))
                {
                    sql = string.Format(@"SELECT DISTINCT ROUTE_NAME 
                                    FROM POR_ROUTE_PART_RET
                                    WHERE  PART_TYPE='{0}' AND ISREWORK={1}",
                                        productType.PreventSQLInjection(),
                                        isRework ? "1" : "0");
                }
                else
                {
                    sql = string.Format(@"SELECT DISTINCT ROUTE_NAME 
                                    FROM POR_ROUTE_PART_RET
                                    WHERE ROOM_NAME='{0}' AND PART_TYPE='{1}' AND ISREWORK={2}",
                                        factoryName.PreventSQLInjection(),
                                        productType.PreventSQLInjection(),
                                        isRework ? "1" : "0");
                }
                DataSet dsRoutePart = db.ExecuteDataSet(CommandType.Text, sql);

                if (dsRoutePart.Tables[0].Rows.Count < 1)
                {
                    strCondition = " 1!=1";
                }
                else if (dsRoutePart.Tables[0].Rows.Count == 1)
                {
                    string routeName = Convert.ToString(dsRoutePart.Tables[0].Rows[0]["ROUTE_NAME"]);
                    strCondition = " ROUTE_NAME='" + routeName.PreventSQLInjection() + "'";
                }
                if (dsRoutePart.Tables[0].Rows.Count > 1)
                {
                    strCondition = string.Empty;
                    for (int i = 0; i < dsRoutePart.Tables[0].Rows.Count; i++)
                    {
                        string routeName = Convert.ToString(dsRoutePart.Tables[0].Rows[i]["ROUTE_NAME"]);
                        strCondition = string.Format("{0}'{1}',", strCondition, routeName.PreventSQLInjection());
                    }
                    strCondition = string.Format(" ROUTE_NAME IN ({0})", strCondition.TrimEnd(','));
                }
                ////ORDER BY A.ENTERPRISE_NAME,A.ROUTE_SEQ,A.ROUTE_STEP_SEQ";
                sql = @"SELECT  A.ENTERPRISE_NAME,A.ENTERPRISE_VERSION,A.ROUTE_NAME,A.ROUTE_SEQ,A.ROUTE_STEP_NAME,A.ROUTE_STEP_SEQ,
                        A.DESCRIPTIONS,A.ROUTE_ENTERPRISE_VER_KEY,A.ROUTE_ROUTE_VER_KEY,A.ROUTE_STEP_KEY,A.ROUTE_NEXT_STEP_KEY
                        FROM V_PROCESS_PLAN A 
                        WHERE ROUTE_STEP_SEQ=(SELECT MIN(ROUTE_STEP_SEQ) FROM  V_PROCESS_PLAN B WHERE B.ENTERPRISE_NAME=A.ENTERPRISE_NAME AND B.ROUTE_NAME=A.ROUTE_NAME)
                        AND {0}
                        AND A.ENTERPRISE_STATUS=1 AND A.ROUTE_STATUS=1                      
                        ORDER BY case A.ENTERPRISE_NAME when '常规工艺组' then 1
                        when 'SH-ZTSM-MAIN(增加灌胶)' then 2
						else 3 end";
                dsReturn = db.ExecuteDataSet(CommandType.Text, String.Format(sql, strCondition));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
    }
}
