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
using FanHai.Hemera.Share.Common;
#endregion


namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 原因代码数据管理类。
    /// </summary>
    public class ReasonCodeEngine : AbstractEngine, IReasonCodeEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReasonCodeEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 插入原因代码数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含原因代码数据的数据表集。<see cref="FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>
        /// 包含执行结果的数据集对象。
        /// </returns>
        public DataSet ReasonCodeInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null != dsParams && dsParams.Tables.Contains(FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME))
            {
                List<string> sqlCommandList = new List<string>();
                //插入新增
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                       new FMM_REASON_CODE_FIELDS(),
                                                       dsParams.Tables[FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME],
                                                       new Dictionary<string, string>(),
                                                       new List<string>());
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
                    LogService.LogError("ReasonCodeInsert Error: " + e.Message);
                }
                finally
                {
                    dbTrans = null;
                    dbConn.Close();
                    dbConn = null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新原因代码数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含原因代码数据的数据表集。<see cref="FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME"/></param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        public DataSet ReasonCodeUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null != dsParams)
            {
                List<string> sqlCommandList = new List<string>();
                if (dsParams.Tables.Contains(FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new FMM_REASON_CODE_FIELDS(),
                                                        dsParams.Tables[FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        { 
                                                            {FMM_REASON_CODE_FIELDS.FIELD_EDIT_TIME, null}
                                                         },
                                                        new List<string>() { FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY },
                                                        FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY);
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
                        LogService.LogError("ReasonCodeUpdate Error: " + e.Message);
                    }
                    finally
                    {
                        dbConn.Close();
                    }
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0007}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据原因代码主键删除原因代码数据。
        /// </summary>
        /// <param name="codeKey">
        /// 原因代码主键。
        /// </param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        public DataSet DeleteReasonCode(string codeKey)
        {
            //get dynamic dataset constructor
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(codeKey))
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            string sql = string.Format("DELETE FROM FMM_REASON_CODE WHERE REASON_CODE_KEY = '{0}'", 
                                                        codeKey.PreventSQLInjection());

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("DeleteReasonCode Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteReasonCode Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 获取原因代码数据。
        /// </summary>
        /// <param name="dtParams">
        /// 包含查询条件名称和值的数据表对象。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// </returns>
        public DataSet GetReasonCode(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(@"SELECT * FROM FMM_REASON_CODE WHERE 1=1");
                if (dtParams != null)
                {
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME))
                    {
                        string codeName = Convert.ToString(htParams[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME]);
                        sbSql.AppendFormat(@" AND REASON_CODE_NAME LIKE '%{0}%'", codeName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE))
                    {
                        string codeType = Convert.ToString(htParams[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]);
                        sbSql.AppendFormat(@" AND REASON_CODE_TYPE = '{0}'", codeType.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS))
                    {
                        string codeClass = Convert.ToString(htParams[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS]);
                        sbSql.AppendFormat(@" AND REASON_CODE_CLASS = '{0}'", codeClass.PreventSQLInjection());
                    }
                }
                sbSql.Append(" ORDER BY REASON_CODE_NAME");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                dsReturn.Tables[0].TableName = FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME;
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCode Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取原因代码数据。
        /// </summary>
        /// <param name="type">
        /// 原因代码类型。
        /// </param>
        /// <param name="name">
        /// 原因代码名称。
        /// </param>
        /// <param name="rcClass">
        /// 原因代码分类。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// </returns>
        public DataSet GetReasonCode(string type,string name,string rcClass)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT * FROM FMM_REASON_CODE 
                                   WHERE REASON_CODE_TYPE = '{0}' 
                                   AND REASON_CODE_NAME = '{1}'
                                   AND REASON_CODE_CLASS='{2}'",
                                   type.PreventSQLInjection(),
                                   name.PreventSQLInjection(),
                                   rcClass.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                dsReturn.Tables[0].TableName = FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCode Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不重复的原因代码数据。
        /// </summary>
        /// <returns>
        /// 包含不重复原因代码数据的数据集对象。
        /// [REASON_CODE_KEY,REASON_CODE_NAME]
        /// </returns>
        public DataSet GetDistinctReasonCodeName()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT DISTINCT REASON_CODE_KEY, REASON_CODE_NAME
                               FROM FMM_REASON_CODE
                               ORDER BY REASON_CODE_NAME";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctReasonCodeName Error: " + ex.Message);
            }
            return dsReturn;
        }
        
        /// <summary>
        /// 获取不存在于某个类别主键中的原因代码数据。
        /// </summary>
        /// <param name="categoryKey">原因代码类别主键。</param>
        /// <param name="categoryType">原因代码类别类型。</param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// </returns>
        public DataSet GetReasonCodeNotExistCategory(string categoryKey, string categoryType)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommond = string.Empty;
            try
            {
                if (categoryKey != string.Empty)
                {
                    sqlCommond = string.Format(@"SELECT REASON_CODE_KEY, REASON_CODE_NAME,REASON_CODE_CLASS
                                                FROM FMM_REASON_CODE
                                                WHERE REASON_CODE_KEY NOT IN (SELECT DISTINCT REASON_CODE_KEY 
                                                                              FROM FMM_REASON_R_CATEGORY
                                                                              WHERE CATEGORY_KEY = '{0}') 
                                                AND REASON_CODE_TYPE = '{1}'
                                                ORDER BY REASON_CODE_NAME", 
                                                categoryKey.PreventSQLInjection(), 
                                                categoryType.PreventSQLInjection());
                }

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommond).Tables[0];
                dtTable.TableName = FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCodeNotExistCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 插入原因代码类别数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含原因代码类别数据的数据表集。<see cref="FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        public DataSet ReasonCodeCategoryInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME))
                {
                    List<string> sqlCommandList = new List<string>();
                    //新增数据
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                           new FMM_REASON_CODE_CATEGORY_FIELDS(),
                                                           dsParams.Tables[FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME],
                                                           new Dictionary<string, string>() 
                                                           { 
                                                               {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDIT_TIME, null},
                                                               {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                           },
                                                           new List<string>());

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
                            LogService.LogError("ReasonCodeCategoryInsert Error: " + e.Message);
                        }
                        finally
                        {
                            dbConn.Close();
                        }
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
                LogService.LogError("ReasonCodeCategoryInsert Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除原因代码类型。
        /// </summary>
        /// <param name="codeCategoryKey">原因代码类型主键。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet DeleteReasonCodeCategory(string codeCategoryKey)
        {
            //get dynamic dataset constructor
            DataSet dataDs = new DataSet();
            try
            {
                if (codeCategoryKey != string.Empty)
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {

                            string sqlCommand= string.Format("DELETE FROM FMM_REASON_CODE_CATEGORY WHERE REASON_CODE_CATEGORY_KEY = '{0}'",
                                                             codeCategoryKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //Commit Transaction
                            dbTran.Commit();

                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("DeleteReasonCodeCategory Error: " + ex.Message);
                        }
                        finally
                        {
                            //Close Connection
                            dbConn.Close();
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("DeleteReasonCodeCategory Error: " + ex.Message);
            }

            return dataDs;
        }
        /// <summary>
        /// 获取原因代码类型数据。
        /// </summary>
        /// <param name="dtParams">包含查询条件的数据表对象。 </param>
        /// <returns>包含原因代码类型数据的数据集对象。</returns>
        public DataSet GetReasonCodeCategory(DataTable dtParams)
        {
            //get dynamic dataset constructor
            DataSet dtReturn = new DataSet();
            //define sqlCommond 
            string sqlCommand = string.Empty;
            try
            {
                Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                sqlCommand = @"SELECT * FROM FMM_REASON_CODE_CATEGORY ";

                if (dtParams != null)
                {
                    string name = Convert.ToString(htParams["REASON_CODE_CATEGORY_NAME"]);
                    string type = Convert.ToString(htParams["REASON_CODE_CATEGORY_TYPE"]);
                    sqlCommand += " WHERE REASON_CODE_CATEGORY_NAME LIKE '%" + name.PreventSQLInjection() + "%' " +
                                    " AND REASON_CODE_CATEGORY_TYPE LIKE '%" + type.PreventSQLInjection() + "%' ";
                }

                sqlCommand += " ORDER BY REASON_CODE_CATEGORY_NAME";

                dtReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dtReturn.Tables[0].TableName = FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dtReturn, "");

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dtReturn, ex.Message);
                LogService.LogError("GetReasonCodeCategory Error: " + ex.Message);
            }
            return dtReturn;
        }
        /// <summary>
        /// 获取不重复的原因代码类型数据。
        /// </summary>
        /// <returns>包含原因代码类型数据的数据集对象。</returns>
        public DataSet GetDistinctReasonCodeCategoryName()
        {
            //get dynamic dataset constructor
            DataSet dsReturn = new DataSet();
            //define sqlCommand 
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT DISTINCT REASON_CODE_CATEGORY_KEY, REASON_CODE_CATEGORY_NAME
                                 FROM FMM_REASON_CODE_CATEGORY
                                ORDER BY REASON_CODE_CATEGORY_NAME";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;

                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctReasonCodeCategoryName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据原因代码名称和类型获取原因代码类型数据。
        /// </summary>
        /// <param name="dsParams">包含查询参数的数据集对象。</param>
        /// <returns>包含原因代码类型数据的数据集对象。</returns>
        public DataSet GetReasonCodeByNameAndType(DataSet dsParams)
        {
            //get dynamic dataset constructor
            DataSet dsReturn = new DataSet();
            //define sqlCommond 
            string sqlCommond = string.Empty;
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    string type = Convert.ToString(htParams["REASON_CODE_CATEGORY_TYPE"]);
                    string name = Convert.ToString(htParams["REASON_CODE_CATEGORY_NAME"]);
                    sqlCommond = @"SELECT REASON_CODE_CATEGORY_KEY,
                                          REASON_CODE_CATEGORY_NAME,
                                          DESCRIPTIONS,
                                          EDITOR,
                                          EDIT_TIME,
                                          EDIT_TIMEZONE,
                                          REASON_CODE_CATEGORY_TYPE
                                    FROM FMM_REASON_CODE_CATEGORY
                                    WHERE REASON_CODE_CATEGORY_TYPE='" + type.PreventSQLInjection() + "' ";

                    if (string.IsNullOrEmpty(name))
                    {
                        sqlCommond += " AND REASON_CODE_CATEGORY_NAME = '" + name.PreventSQLInjection() + "' ";
                    }
                    sqlCommond += " ORDER BY REASON_CODE_CATEGORY_NAME";
                }

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommond).Tables[0];
                dtTable.TableName = FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCodeByNameAndType Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新原因代码类型数据。
        /// </summary>
        /// <param name="dsParams">包含原因代码类型数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet ReasonCodeCategoryUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams)
                {
                    List<string> sqlCommandList = new List<string>();
                    if (dsParams.Tables.Contains(FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                            new FMM_REASON_CODE_CATEGORY_FIELDS(),
                            dsParams.Tables[FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME],
                            new Dictionary<string, string>() { 
                                                                {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDIT_TIME, null},
                                                                {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                             },
                            new List<string>() { FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY },
                            FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY);
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
                            LogService.LogError("ReasonCodeCategoryUpdate Error: " + e.Message);
                        }
                        finally
                        {
                            dbConn.Close();
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
                LogService.LogError("ReasonCodeCategoryUpdate Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据原因代码分类主键获取原因代码数据。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// </returns>
        public DataSet GetReasonCategory(string categoryKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommond = string.Empty;
            try
            {
                if (categoryKey != string.Empty)
                {
                    sqlCommond = string.Format(@"SELECT a.REASON_CODE_KEY,a.REASON_CODE_NAME,a.REASON_CODE_CLASS
                                               FROM FMM_REASON_CODE a
                                               WHERE EXISTS (SELECT 1
                                                             FROM FMM_REASON_R_CATEGORY
                                                             WHERE CATEGORY_KEY = '{0}'
                                                             AND REASON_CODE_KEY=a.REASON_CODE_KEY) 
                                               ORDER BY a.REASON_CODE_NAME", categoryKey.PreventSQLInjection());
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommond);
                dsReturn.Tables[0].TableName = FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 添加原因代码和类型的关联。
        /// </summary>
        /// <param name="dsParams">包含原因代码和类型关联的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddReasonCategory(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(FMM_REASON_R_CATEGORY_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dsParams.Tables[FMM_REASON_R_CATEGORY_FIELDS.DATABASE_TABLE_NAME];
                    FMM_REASON_R_CATEGORY_FIELDS categoryTable = new FMM_REASON_R_CATEGORY_FIELDS();

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {

                                Hashtable fields = new Hashtable()
                                                                 {
                                                                   {FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY,
                                                                       row[FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY]},
                                                                   {FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY,
                                                                       row[FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY]}
                                                                 };
                                string sqlCommand = DatabaseTable.BuildInsertSqlStatement(categoryTable, fields, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
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
                            LogService.LogError("AddReasonCategory Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddReasonCategory Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 删除原因代码和类型的关联。
        /// </summary>
        /// <param name="dsParams">包含原因代码和类型关联的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteReasonCategory(DataSet dsParams)
        {
            //get dynamic dataset constructor
            DataSet dataDs = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(FMM_REASON_R_CATEGORY_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dsParams.Tables[FMM_REASON_R_CATEGORY_FIELDS.DATABASE_TABLE_NAME];
                    FMM_REASON_R_CATEGORY_FIELDS categoryTable = new FMM_REASON_R_CATEGORY_FIELDS();

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string categoryKey = Convert.ToString(row[FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY]);
                                string codeKey = Convert.ToString(row[FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY]);
                                string sqlCommand =string.Format(@"DELETE FROM FMM_REASON_R_CATEGORY
                                                                 WHERE CATEGORY_KEY = '{0}'
                                                                 AND REASON_CODE_KEY = '{1}'",
                                                                 categoryKey.PreventSQLInjection(),
                                                                 codeKey.PreventSQLInjection());

                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }

                            //Commit Transaction
                            dbTran.Commit();

                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("DeleteReasonCategory Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("DeleteReasonCategory Error: " + ex.Message);
            }

            return dataDs;
        }
        /// <summary>
        /// 根据原因代码分组名称获取超时原因代码数据。
        /// </summary>
        /// <param name="categoryName">
        /// 原因代码分组名称,
        /// 如果为空则获取原因代码分组名称为DLR_DEFAULT的超时原因代码。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集。
        /// </returns>
        public DataSet GetReasonCodeForDelay(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                categoryName = "DLR_DEFAULT";
            }
            return GetReasonCodeByCategoryName(categoryName, "DLR");
        }
        /// <summary>
        /// 根据原因代码分组名称获取回收原因代码数据。
        /// </summary>
        /// <param name="categoryName">
        /// 原因代码分组名称,
        /// 如果为空则获取原因代码分组名称为RC_DEFAULT的回收原因代码。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集。
        /// </returns>
        public DataSet GetReasonCodeForRecover(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                categoryName = "RC_DEFAULT";
            }
            return GetReasonCodeByCategoryName(categoryName, "RC");
        }
        /// <summary>
        /// 根据原因代码分组名称获取退库原因代码数据。
        /// </summary>
        /// <param name="categoryName">
        /// 原因代码分组名称,
        /// 如果为空则获取原因代码分组名称为TK_DEFAULT的原因代码。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集。
        /// </returns>
        public DataSet GetReasonCodeForReturnMaterial(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                categoryName = "TK_DEFAULT";
            }
            return GetReasonCodeByCategoryName(categoryName, "TK");
        }
        /// <summary>
        /// 根据分组名称和类型获取原因代码。
        /// </summary>
        /// <param name="categoryName">分组名称。</param>
        /// <param name="type">类型。</param>
        /// <returns>包含原因代码数据的数据集。</returns>
        private DataSet GetReasonCodeByCategoryName(string categoryName,string type)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT  A.REASON_CODE_NAME,A.REASON_CODE_KEY
                                            FROM FMM_REASON_CODE     A,
                                                 FMM_REASON_CODE_CATEGORY B,
                                                 FMM_REASON_R_CATEGORY    C
                                            WHERE A.REASON_CODE_KEY = C.REASON_CODE_KEY
                                            AND B.REASON_CODE_CATEGORY_KEY = C.CATEGORY_KEY
                                            AND A.REASON_CODE_TYPE='{1}'
                                            AND B.REASON_CODE_CATEGORY_NAME = '{0}'",
                                            categoryName.PreventSQLInjection(),
                                            type.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCodeByCategoryName Error: " + ex.Message);
            }
            return dsReturn;
        }


    }
}

