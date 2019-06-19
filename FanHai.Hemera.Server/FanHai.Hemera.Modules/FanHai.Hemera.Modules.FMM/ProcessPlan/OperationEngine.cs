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
    /// 工序数据管理类。
    /// </summary>
    public class OperationEngine:AbstractEngine,IOperationEngine
    {
        private Database db;  //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 新增工序。
        /// </summary>
        /// <param name="dsParams">包含工序数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet OperationInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    List<string> sqlCommandList = new List<string>();
                    string strVersion = "1";
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
                    string operationName = Convert.ToString(dtParams.Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME]);
                    string timezone = Convert.ToString(dtParams.Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIMEZONE]);
                    string editor = Convert.ToString(dtParams.Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDITOR]);
                    //获取版本号。
                    if (dtParams.Rows.Count == 1)
                    {
                        string sqlCommand =string.Format(@"SELECT MAX(OPERATION_VERSION)+1 AS OPERATION_VERSION 
                                                        FROM POR_ROUTE_OPERATION_VER 
                                                        WHERE ROUTE_OPERATION_NAME ='{0}'",
                                                        operationName.PreventSQLInjection());
                        object version=db.ExecuteScalar(CommandType.Text, sqlCommand);
                        if(version!=null && version!=DBNull.Value){
                            strVersion=version.ToString();
                        }
                    }
                    //生成INSERT SQL
                    List<string> sqlCommandOperation = new List<string>();
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandOperation,
                                                        new POR_ROUTE_OPERATION_VER_FIELDS(),
                                                        dsParams.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        { 
                                                            {POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION, strVersion}
                                                        },
                                                        new List<string>());
                    if (1 == sqlCommandOperation.Count && sqlCommandOperation[0].Length > 20)
                    {
                        sqlCommandList.Add(sqlCommandOperation[0]);
                        //生成工序属性的INSERT SQL
                        if (dsParams.Tables.Contains(POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_ROUTE_OPERATION_ATTR_FIELDS(),
                                                                   dsParams.Tables[POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   { 
                                                                        {POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_EDIT_TIME,null},
                                                                        {POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_EDIT_TIMEZONE, timezone}
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        //生成工序参数的INSERT SQL
                        if (dsParams.Tables.Contains(POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_ROUTE_OPERATION_PARAM_FIELDS(),
                                                                   dsParams.Tables[POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>(),
                                                                   new List<string>());
                        }
                        //执行数据操作。
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
                LogService.LogError("OperationInsert Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除工序数据。
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet OperationDelete(string operationKey)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = string.Empty;
            try
            {
                if (operationKey.Trim().Length > 0)
                {
                    // Please Don't change the delete sequence, otherwise will get error result

                    // 删除工序属性数据
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}='{2}'",
                                                POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME,
                                                POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY,
                                                operationKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    // 删除工序
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}='{2}'",
                                                POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME,
                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
                                                operationKey.PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                    // 删除工序参数
                    sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1}='{2}'",
                                                POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME,
                                                POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_OPERATION_VER_KEY,
                                                operationKey.PreventSQLInjection());
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
                            dbConn.Close();
                            dbTrans=null;
                            dbConn=null;
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
                LogService.LogError("OperationDelete Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新工序数据。
        /// </summary>
        /// <param name="dsParams">包含工序数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet OperationUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dtCommon = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                Hashtable htCommon = CommonUtils.ConvertToHashtable(dtCommon);
                string edittime = Convert.ToString(htCommon[COMMON_FIELDS.FIELD_COMMON_EDIT_TIME]);
                string timezone = Convert.ToString(htCommon[COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE]);
                string editor = Convert.ToString(htCommon[COMMON_FIELDS.FIELD_COMMON_EDITOR]);
                List<string> sqlCommandList = new List<string>();
                //生成更新工序数据的SQL
                if (dsParams.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    Hashtable htOpeartion = CommonUtils.ConvertToHashtable(dsParams.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME]);
                    string operationKey = Convert.ToString(htOpeartion[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
                    htOpeartion.Remove(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY);
                    Conditions cons = new Conditions();
                    cons.Add(DatabaseLogicOperator.And, POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_OPERATION_VER_KEY, DatabaseCompareOperator.Equal, operationKey);
                    string updateSql = DatabaseTable.BuildUpdateSqlStatement(new POR_ROUTE_OPERATION_VER_FIELDS(), htOpeartion, cons);
                    sqlCommandList.Add(updateSql);
                }
                //生成更新工序属性数据的SQL
                if (dsParams.Tables.Contains(POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList,
                                                            new POR_ROUTE_OPERATION_ATTR_FIELDS(),
                                                            dsParams.Tables[POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                            POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY);

                }
                //生成工序参数的SQL
                if (dsParams.Tables.Contains(POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME))
                {
                    POR_ROUTE_OPERATION_PARAM_FIELDS paramFileds = new POR_ROUTE_OPERATION_PARAM_FIELDS();
                    DataTable dtOperationParams = dsParams.Tables[POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME];
                    if (dtOperationParams != null)
                    {
                        //新增
                        DataTable dtInsertOperationParams = dtOperationParams.GetChanges(DataRowState.Added);
                        if (dtInsertOperationParams != null)
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   paramFileds,
                                                                   dtInsertOperationParams,
                                                                   new Dictionary<string, string>(),
                                                                   new List<string>());
                        }
                        //更新修改。
                        DataTable dtUpdateOperationParams = dtOperationParams.GetChanges(DataRowState.Modified);
                        if (dtUpdateOperationParams != null)
                        {
                            foreach (DataRow dr in dtUpdateOperationParams.Rows)
                            {
                                Hashtable htOpeartionParams = CommonUtils.ConvertRowToHashtable(dr);
                                string operationParamKey = Convert.ToString(htOpeartionParams[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_ROUTE_OPERATION_PARAM_KEY]);
                                htOpeartionParams.Remove(POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_OPERATION_VER_KEY);
                                Conditions cons = new Conditions();
                                cons.Add(DatabaseLogicOperator.And, POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_ROUTE_OPERATION_PARAM_KEY, DatabaseCompareOperator.Equal, operationParamKey);
                                string updateSql = DatabaseTable.BuildUpdateSqlStatement(paramFileds, htOpeartionParams, cons);
                                sqlCommandList.Add(updateSql);
                            }
                        }
                    }
                }
                //执行更新数据操作。
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
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("OperationUpdate Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询工序数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        public DataSet OperationSearch(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string operationName = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME]);
                    sqlCommand =string.Format(@"SELECT * 
                                                FROM POR_ROUTE_OPERATION_VER 
                                                WHERE ROUTE_OPERATION_NAME  LIKE '%{0}%'",
                                                operationName.PreventSQLInjection());

                    if (htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION] != null)
                    {
                        string version = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION]);
                        sqlCommand += "AND OPERATION_VERSION = '" + version.PreventSQLInjection() + "'";
                    }

                    sqlCommand += " AND OPERATION_STATUS <> 2 ORDER BY ROUTE_OPERATION_NAME";
                }
                else
                {
                    sqlCommand = "SELECT * FROM POR_ROUTE_OPERATION_VER WHERE OPERATION_STATUS <> '2' ORDER BY ROUTE_OPERATION_NAME";
                }

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("OperationSearch Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询工序数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="records">总记录数。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        public DataSet GetOperations(DataSet dsParams, string operationName, int pageNo, int pageSize, out int pages, out int records)
        {
            DataSet dsReturn = new DataSet();

            pages = 0;
            records = 0;

            try
            {
                POR_ROUTE_OPERATION_VER_FIELDS operationsFields = new POR_ROUTE_OPERATION_VER_FIELDS();

                #region 返回的数据列

                List<string> interestColumns = new List<string>();

                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IMAGE_KEY);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS);
                interestColumns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IS_REWORKABLE);

                #endregion

                #region 组织查询条件

                Conditions conditions = new Conditions();

                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    if (inputParamDataTable != null && inputParamDataTable.Columns.Contains(PARAMETERS_INPUT.FIELD_KEY))
                    {
                        foreach (DataRow row in inputParamDataTable.Rows)
                        {
                            object key = row[PARAMETERS_INPUT.FIELD_KEY];

                            if (key == null || key == DBNull.Value)
                            {
                                conditions.Add(DatabaseLogicOperator.And, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, DatabaseCompareOperator.Null, string.Empty);
                            }
                            else
                            {
                                conditions.Add(DatabaseLogicOperator.And, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, DatabaseCompareOperator.Equal, key.ToString());
                            }
                        }
                    }
                }

                #endregion
                //已激活的工序。
                conditions.Add(DatabaseLogicOperator.And, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS, DatabaseCompareOperator.Equal, "1");
                //工序名称
                if (!string.IsNullOrEmpty(operationName))
                {
                    conditions.Add(DatabaseLogicOperator.And, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, DatabaseCompareOperator.Like, string.Format("%{0}%", operationName));
                }
                //创建查询SQL
                string sqlString = DatabaseTable.BuildQuerySqlStatement(operationsFields, interestColumns, conditions);
                
                if (pageNo > 0 && pageSize > 0)//分页查询
                {
                    AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, dsReturn, POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME);
                }
                else
                {
                    db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME });
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOperations Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 根据工序主键获取工序数据。
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        public DataSet GetOperationByKey(string operationKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(operationKey))
                {
                    //获取工序自定义属性
                    string sqlCommand =string.Format(@"SELECT A.*, B.DATA_TYPE
                                                      FROM POR_ROUTE_OPERATION_ATTR A
                                                      LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                                      WHERE ROUTE_OPERATION_VER_KEY = '{0}'
                                                      ORDER BY 1",
                                                      operationKey.PreventSQLInjection());
                    DataTable dtUDAs = new DataTable();
                    dtUDAs = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtUDAs.TableName = POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtUDAs, false, MissingSchemaAction.Add);
                    //获取工序参数。
                    sqlCommand = string.Format(@"SELECT A.*
                                              FROM POR_ROUTE_OPERATION_PARAM A
                                              WHERE ROUTE_OPERATION_VER_KEY = '{0}'
                                              AND IS_DELETED=0
                                              ORDER BY PARAM_INDEX",
                                              operationKey.PreventSQLInjection());
                    DataTable dtParams = new DataTable();
                    dtParams = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtParams.TableName = POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtParams, false, MissingSchemaAction.Add);
                    //获取工序数据
                    sqlCommand =string.Format(@"SELECT A.*,
                                                        D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                        E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME
                                                FROM POR_ROUTE_OPERATION_VER A
                                                LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                                LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                                WHERE A.ROUTE_OPERATION_VER_KEY = '{0}'
                                                ORDER BY 1", operationKey.PreventSQLInjection());
                    DataTable dtMain = new DataTable();
                    dtMain = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtMain.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtMain, false, MissingSchemaAction.Add);
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0006}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOperationByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        public DataSet GetMaxVerOperation(DataSet dsQueryParams)
        {
            DataSet dsReturn = new DataSet();
            string operationName = string.Empty;
            try
            {
                if (dsQueryParams != null && dsQueryParams.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtQueryParams = dsQueryParams.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htQueryParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtQueryParams);
                    operationName = Convert.ToString(htQueryParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME]);
                }
                //获取工序自定义属性数据。
               string sqlCommand =string.Format(@"SELECT A.*, B.DATA_TYPE
                                                  FROM POR_ROUTE_OPERATION_ATTR A
                                                  LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                                  WHERE ROUTE_OPERATION_VER_KEY IN (SELECT ROUTE_OPERATION_VER_KEY
                                                                                    FROM POR_ROUTE_OPERATION_VER A
                                                                                    WHERE  NOT EXISTS(SELECT 1 
		                                                                                              FROM POR_ROUTE_OPERATION_VER
		                                                                                              WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
		                                                                                              AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                                                    AND A.ROUTE_OPERATION_NAME LIKE '%{0}%')",
                                               operationName.PreventSQLInjection());
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //获取工序参数数据。
                sqlCommand = string.Format(@"SELECT A.*
                                          FROM POR_ROUTE_OPERATION_PARAM A
                                          WHERE ROUTE_OPERATION_VER_KEY IN (SELECT ROUTE_OPERATION_VER_KEY
                                                                            FROM POR_ROUTE_OPERATION_VER A
                                                                            WHERE  NOT EXISTS(SELECT 1 
	                                                                                          FROM POR_ROUTE_OPERATION_VER
	                                                                                          WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
	                                                                                          AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                                            AND A.ROUTE_OPERATION_NAME LIKE '%{0}%')
                                          AND IS_DELETED=0
                                          ORDER BY ROUTE_OPERATION_VER_KEY,PARAM_INDEX",
                                          operationName.PreventSQLInjection());
                DataTable dtParams = new DataTable();
                dtParams = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtParams.TableName = POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtParams, false, MissingSchemaAction.Add);
                //获取工序数据。
                sqlCommand = string.Format(@"SELECT A.*, 
                                                D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME
                                          FROM POR_ROUTE_OPERATION_VER A
                                          LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                          LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                          WHERE NOT EXISTS(SELECT 1 
			                                               FROM POR_ROUTE_OPERATION_VER
			                                               WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
			                                               AND OPERATION_VERSION>A.OPERATION_VERSION)
                                          AND A.ROUTE_OPERATION_NAME LIKE '%{0}%' 
                                          ORDER BY 1",
                                          operationName.PreventSQLInjection());

                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaxVerOperation Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        public DataSet GetMaxVersionOperation(string operationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取工序自定义属性数据。
                string sqlCommand = string.Format(@"SELECT A.*, B.DATA_TYPE
                                                  FROM POR_ROUTE_OPERATION_ATTR A
                                                  LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                                  WHERE ROUTE_OPERATION_VER_KEY IN (SELECT ROUTE_OPERATION_VER_KEY
                                                                                    FROM POR_ROUTE_OPERATION_VER A
                                                                                    WHERE  NOT EXISTS(SELECT 1 
		                                                                                              FROM POR_ROUTE_OPERATION_VER
		                                                                                              WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
		                                                                                              AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                                                    AND A.ROUTE_OPERATION_NAME = '{0}')",
                                                operationName.PreventSQLInjection());
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //获取工序参数数据。
                sqlCommand = string.Format(@"SELECT A.*
                                          FROM POR_ROUTE_OPERATION_PARAM A
                                          WHERE ROUTE_OPERATION_VER_KEY IN (SELECT ROUTE_OPERATION_VER_KEY
                                                                            FROM POR_ROUTE_OPERATION_VER A
                                                                            WHERE  NOT EXISTS(SELECT 1 
	                                                                                          FROM POR_ROUTE_OPERATION_VER
	                                                                                          WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
	                                                                                          AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                                            AND A.ROUTE_OPERATION_NAME = '{0}')
                                          AND IS_DELETED=0
                                          ORDER BY ROUTE_OPERATION_VER_KEY,PARAM_INDEX",
                                          operationName.PreventSQLInjection());
                DataTable dtParams = new DataTable();
                dtParams = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtParams.TableName = POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtParams, false, MissingSchemaAction.Add);
                //获取工序数据。
                sqlCommand = string.Format(@"SELECT A.*,  
                                                    D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                    E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME
                                          FROM POR_ROUTE_OPERATION_VER A
                                          LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                          LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                          WHERE NOT EXISTS(SELECT 1 
			                                               FROM POR_ROUTE_OPERATION_VER
			                                               WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
			                                               AND OPERATION_VERSION>A.OPERATION_VERSION)
                                          AND A.ROUTE_OPERATION_NAME = '{0}' 
                                          ORDER BY 1",
                                          operationName.PreventSQLInjection());

                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaxVersionOperation Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序基本数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>包含工序基本数据的数据集对象。</returns>
        public DataSet GetMaxVersionOperationBaseInfo(string operationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取工序数据。
                string sqlCommand = string.Format(@"SELECT A.*, 
                                                        D.REASON_CODE_CATEGORY_NAME AS SCRAP_REASON_CODE_CATEGORY_NAME, 
                                                        E.REASON_CODE_CATEGORY_NAME AS DEFECT_REASON_CODE_CATEGORY_NAME
                                                  FROM POR_ROUTE_OPERATION_VER A
                                                  LEFT JOIN FMM_REASON_CODE_CATEGORY D ON D.REASON_CODE_CATEGORY_KEY = A.SCRAP_REASON_CODE_CATEGORY_KEY
                                                  LEFT JOIN FMM_REASON_CODE_CATEGORY E ON E.REASON_CODE_CATEGORY_KEY = A.DEFECT_REASON_CODE_CATEGORY_KEY
                                                  WHERE NOT EXISTS(SELECT 1 
			                                                       FROM POR_ROUTE_OPERATION_VER
			                                                       WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
			                                                       AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                  AND A.ROUTE_OPERATION_NAME = '{0}' 
                                                  ORDER BY A.OPERATION_VERSION DESC",
                                                  operationName.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaxVersionOperationBaseInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序自定义属性数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <param name="attrName">自定义属性名称,如果为空则查询所有自定义属性数据。</param>
        /// <returns>包含工序自定义数据的数据集对象。</returns>
        public DataSet GetMaxVersionOperationAttrInfo(string operationName,string attrName)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                //获取工序自定义属性数据。
                sbSql.AppendFormat(@"SELECT A.*, B.DATA_TYPE
                                  FROM POR_ROUTE_OPERATION_ATTR A
                                  LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                  WHERE ROUTE_OPERATION_VER_KEY IN (SELECT ROUTE_OPERATION_VER_KEY
                                                                    FROM POR_ROUTE_OPERATION_VER A
                                                                    WHERE  NOT EXISTS(SELECT 1 
                                                                                      FROM POR_ROUTE_OPERATION_VER
                                                                                      WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
                                                                                      AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                                    AND A.ROUTE_OPERATION_NAME = '{0}')",
                                  operationName.PreventSQLInjection());
                if (!string.IsNullOrEmpty(attrName))
                {
                    sbSql.AppendFormat(" AND A.ATTRIBUTE_NAME='{0}'", attrName.PreventSQLInjection());
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                dsReturn.Tables[0].TableName = POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaxVersionOperationAttrInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序基础数据和参数数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>包含工序参数数据的数据集对象。</returns>
        public DataSet GetOperationBaseAndParamInfo(string operationName)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                //获取工序参数数据。
                sbSql.AppendFormat(@"SELECT A.*
                                    FROM POR_ROUTE_OPERATION_PARAM A
                                    WHERE ROUTE_OPERATION_VER_KEY IN (SELECT ROUTE_OPERATION_VER_KEY
                                                                FROM POR_ROUTE_OPERATION_VER A
                                                                WHERE  NOT EXISTS(SELECT 1 
                                                                                  FROM POR_ROUTE_OPERATION_VER
                                                                                  WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
                                                                                  AND OPERATION_VERSION>A.OPERATION_VERSION)
                                                                AND A.ROUTE_OPERATION_NAME = '{0}')
                                   AND A.IS_DELETED=0",
                                  operationName.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                dsReturn.Tables[0].TableName = POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME;

                //获取工序数据。
                string sql = string.Format(@"SELECT A.*
                                          FROM POR_ROUTE_OPERATION_VER A
                                          WHERE NOT EXISTS(SELECT 1 
	                                                       FROM POR_ROUTE_OPERATION_VER
	                                                       WHERE ROUTE_OPERATION_NAME=A.ROUTE_OPERATION_NAME 
	                                                       AND OPERATION_VERSION>A.OPERATION_VERSION)
                                          AND A.ROUTE_OPERATION_NAME = '{0}' 
                                          ORDER BY A.OPERATION_VERSION DESC", 
                                          operationName.PreventSQLInjection());
                DataTable dtOperation = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtOperation.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtOperation, false, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaxVersionOperationParamInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取已激活且不重复的工序名称。
        /// </summary>
        /// <returns>
        /// 包含工序名称的数据集对象。
        /// [ROUTE_OPERATION_NAME]
        /// </returns>
        public DataSet GetDistinctOperationNameList()
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            try
            {
                sql = @"SELECT DISTINCT ROUTE_OPERATION_NAME
                        FROM POR_ROUTE_OPERATION_VER 
                        WHERE ROUTE_OPERATION_NAME IS NOT NULL 
                        AND OPERATION_STATUS>=1";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctOperationNameList Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
