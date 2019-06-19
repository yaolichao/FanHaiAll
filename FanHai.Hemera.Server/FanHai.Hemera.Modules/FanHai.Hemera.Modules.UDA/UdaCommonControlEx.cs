using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using System.Collections;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FanHai.Hemera.Modules.UDA
{
    /// <summary>
    /// 自定义属性、生产线及其自定义属性数据的管理类。
    /// </summary>
    public class UdaCommonControlEx : AbstractEngine, IUdaCommonControlEx
    {
        private Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UdaCommonControlEx()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 获取自定义属性对象类型。
        /// </summary>
        /// <returns>包含自定义属性对象类型的数据集对象。</returns>
        public DataSet GetUdaObjectType()
        {
            DataSet dsObjectType = new DataSet();
            string sql = @" SELECT DISTINCT T.OBJECT_TYPE
                            FROM BASE_ATTRIBUTE_VALUE T
                            ORDER BY T.OBJECT_TYPE";
            try
            {
                dsObjectType = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetUdaObjectType Error: " + ex.Message);
            }
            return dsObjectType;

        }
        /// <summary>
        /// 根据自定义属性数据主键获取对象类型。
        /// </summary>
        /// <param name="objKey">自定义属性数据主键。</param>
        /// <returns>自定义属性数据对应的对象类型。</returns>
        public DataSet GetUdaObjectNameList(string objKey)
        {
            DataSet dsObjectName = new DataSet();
            string sql = @" SELECT DISTINCT t.OBJECT_TYPE
                            FROM BASE_ATTRIBUTE_VALUE t 
                            WHERE 1=1";

            if (!string.IsNullOrEmpty(objKey))
            {
                sql = sql + " AND t.OBJECT_KEY = '" + objKey.PreventSQLInjection() + "'";
            }
            sql = sql + "  ORDER BY t.OBJECT_TYPE";
            try
            {
                dsObjectName = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {

                LogService.LogError("GetUdaObjectNameList Error: " + ex.Message);
            }
            return dsObjectName;

        }
        /// <summary>
        /// 为生产线添加自定义属性值。
        /// </summary>
        /// <param name="dataSet">包含生产线自定义属性的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddLineAttributeValue(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null != dsParams && dsParams.Tables.Contains(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME))
            {
                List<string> sqlCommandList = new List<string>();
                DataTable dtParams= dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];
                string objectName = Convert.ToString(dtParams.Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME]).ToUpper();
                //判断线别是否存在。
                string sqlCommand = @"SELECT LINE_NAME FROM FMM_PRODUCTION_LINE  WHERE LINE_NAME='" + objectName.PreventSQLInjection() + "'";
                object name = db.ExecuteScalar(CommandType.Text, sqlCommand);
                if (name != null)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "该线别已经存在");
                    return dsReturn;
                }
                //生成INSERT SQL
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                    new FMM_PRODUCTION_LINE_FIELDS(),
                                                    dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME],
                                                    new Dictionary<string, string>() 
                                                    { 
                                                        {FMM_PRODUCTION_LINE_FIELDS.FIELD_EDIT_TIME,null},
                                                        {FMM_PRODUCTION_LINE_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"},
                                                    },
                                                    new List<string>());
                if (1 == sqlCommandList.Count && sqlCommandList[0].Length > 20)
                {
                    //生产线自定义属性
                    if (dsParams.Tables.Contains(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                               new BASE_ATTRIBUTE_VALUE_FIELDS(),
                                                               dsParams.Tables[BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME],
                                                               new Dictionary<string, string>() 
                                                               { 
                                                                    {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                                                    {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                               },
                                                               new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                    }
                    //执行数据插入
                    if (sqlCommandList.Count > 0)
                    {

                        DbConnection dbCon = db.CreateConnection();
                        dbCon.Open();
                        DbTransaction dbTrans = dbCon.BeginTransaction();
                        try
                        {
                            foreach (string sql in sqlCommandList)
                            {
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                            }
                            dbTrans.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            dbTrans.Rollback();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            LogService.LogError("AddLineAttributeValue Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTrans = null;
                            dbCon.Close();
                            dbCon = null;
                        }
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "More than one Attribute in input parameter");
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "No Attribute Tables in input paremter.");
            }
            return dsReturn;
        }
       
        /// <summary>
        /// 根据生产线名获取生产线对应的自定义属性数据。
        /// </summary>
        /// <param name="lineName">生产线名。</param>
        /// <returns>包含自定义属性数据的数据集对象。</returns>
        public DataSet GetLineTypeByName(string lineName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(lineName))
                {
                    string lName = lineName.ToUpper();
                    string sql = @" SELECT A.* FROM FMM_PRODUCTION_LINE A WHERE LINE_NAME='" + lName.PreventSQLInjection() + "'";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;

                    sql = @"SELECT A.*, C.DATA_TYPE AS DATA_TYPE
                            FROM BASE_ATTRIBUTE_VALUE A
                            INNER JOIN FMM_PRODUCTION_LINE B ON  A.OBJECT_KEY = B.PRODUCTION_LINE_KEY
                            LEFT JOIN BASE_ATTRIBUTE C ON C.ATTRIBUTE_KEY=A.ATTRIBUTE_KEY
                            WHERE B.LINE_NAME= '" + lName.PreventSQLInjection() + "'";
                    DataTable udaDataTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    udaDataTable.TableName = BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME;
                    //ADD UDA TABLE TO DATASET
                    dsReturn.Merge(udaDataTable, true, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "Object key is null");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLineTypeByName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询生产线数据
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含生产线数的数据集对象。</returns>
        public DataSet SearchLineAttribute(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = "";
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    sqlCommand = @"SELECT * FROM FMM_PRODUCTION_LINE A WHERE 1=1";
                    if (htParams.Contains(FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME))
                    {
                        string lineName=Convert.ToString(htParams[FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME]);
                        sqlCommand += " AND LINE_NAME LIKE '%" + lineName.PreventSQLInjection() + "%'";
                    }
                }
                else
                {
                    sqlCommand = "SELECT * FROM FMM_PRODUCTION_LINE";
                }
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchLineAttribute Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新生产线及其对应的自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含生产线及其对应的自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateLineTypeAttribute(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                List<string> sqlCommandList = new List<string>();
                //生成更新生产线数据的SQL
                if (dsParams.Tables.Contains(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new FMM_PRODUCTION_LINE_FIELDS(),
                                                       dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME],
                                                       new Dictionary<string, string>() 
                                                       { 
                                                            {FMM_PRODUCTION_LINE_FIELDS.FIELD_EDIT_TIME,null},
                                                            {FMM_PRODUCTION_LINE_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                       },
                                                       new List<string>() { },
                                                       FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY);
                }
                //生成UPDATE 自定义属性数据的SQL
                if (dsParams.Tables.Contains(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList,
                                                           new BASE_ATTRIBUTE_VALUE_FIELDS(),
                                                           dsParams.Tables[BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME],
                                                           BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_KEY);
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
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("update Attribute Error: " + ex.Message);
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
                LogService.LogError("UpdateLineTypeAttribute Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除生产线及其对应的自定义属性数据。
        /// </summary>
        /// <param name="objectKey">生产线主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteLineTypeAttribute(string objectKey)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = "";


            sqlCommand = string.Format(@"DELETE FROM {0} WHERE {1} = '{2}'",
                                        BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_KEY,
                                        objectKey.PreventSQLInjection());
            sqlCommandList.Add(sqlCommand);

            sqlCommand = string.Format(@"DELETE FROM {0} WHERE  {1} = '{2}'",
                                        FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                        FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,
                                        objectKey.PreventSQLInjection());
            sqlCommandList.Add(sqlCommand);

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
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ComputerConfigueDelete Error: " + ex.Message);
            }
            finally
            {
                dbTrans = null;
                dbConn.Close();
                dbConn = null;
            }

            return dsReturn;
        }
        /// <summary>
        /// 根据自定义属性类别获取自定义属性信息。
        /// </summary>
        /// <param name="objCategory">自定义属性类别。</param>
        /// <returns>包含自定义属性信息的数据集对象。</returns>
        public DataSet GetObjectAttributsForCategory(string objCategory)
        {
            string sql = "";    //define sql string
            DataSet dsReturn = new DataSet();   //DATASET TO REVEIVE DATA
            try
            {
               sql = @" SELECT T.CATEGORY_KEY, T.CATEGORY_NAME
                        FROM BASE_ATTRIBUTE_CATEGORY T
                        WHERE T.CATEGORY_NAME =";

                switch (objCategory)
                    {
                        case "Line":
                            sql += "'Uda_Line'";
                            break;
                        default:
                            sql += "'" + objCategory.PreventSQLInjection() + "'";
                            break;
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAttributsColumnsForSomeCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新生产线信息。
        /// </summary>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="lineName">生产线名称。</param>
        /// <param name="lineCode">生产线代码。</param>
        /// <param name="description">描述。</param>
        /// <param name="editor">编辑人。</param>
        /// <param name="edit_timezone">编辑时区。</param>
        /// <returns>更新生产线信息的记录数。</returns>
        public int UpdateLineInfo(string lineKey, string lineName, string lineCode, 
                                  string description, string editor, string edit_timezone)
        {
            int exeResult = 0;
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            try
            {
                string sql = string.Format(@"UPDATE FMM_PRODUCTION_LINE 
                                    SET LINE_NAME = '{0}',
                                    LINE_CODE = '{1}',
                                    DESCRIPTIONS ='{2}',
                                    EDIT_TIME = GETDATE(),
                                    EDITOR = '{3}',
                                    EDIT_TIMEZONE = '{4}'
                                    WHERE PRODUCTION_LINE_KEY = '{5}'",
                                    lineName.PreventSQLInjection(),
                                    lineCode.PreventSQLInjection(),
                                    description.PreventSQLInjection(),
                                    editor.PreventSQLInjection(),
                                    edit_timezone.PreventSQLInjection(),
                                    lineKey.PreventSQLInjection());
                exeResult = db.ExecuteNonQuery(dbTrans,CommandType.Text, sql);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UpdateLineInfo Error: " + ex.Message);
            }
            finally
            {
                dbTrans = null;
                dbConn.Close();
                dbConn = null;
            }
            return exeResult;
        }
    }
}
