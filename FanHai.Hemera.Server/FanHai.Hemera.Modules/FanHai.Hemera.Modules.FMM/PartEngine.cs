using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;

using System.Data.Common;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 产品料号数据管理类。
    /// </summary>
    public class PartEngine : AbstractEngine, IPartEngine
    {
        private Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PartEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 新增产品料号数据。
        /// </summary>
        /// <param name="dsParams">包含产品料号数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet PartInsert(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null != dsParams && dsParams.Tables.Contains(POR_PART_FIELDS.DATABASE_TABLE_NAME))
            {
                
                List<string> sqlCommandList = new List<string>();
                DataTable dtParams=dsParams.Tables[POR_PART_FIELDS.DATABASE_TABLE_NAME];
                string partName=Convert.ToString(dtParams.Rows[0][POR_PART_FIELDS.FIELD_PART_NAME]);
                string strSql = string.Format("SELECT COUNT(*) FROM POR_PART WHERE PART_NAME ='{0}'",partName.PreventSQLInjection());
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.FMM.PartNameIsExist}");
                    return dsReturn;
                }
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                    new POR_PART_FIELDS(),
                                                    dsParams.Tables[POR_PART_FIELDS.DATABASE_TABLE_NAME],
                                                    new Dictionary<string, string>() 
                                                    { 
                                                        {POR_PART_FIELDS.FIELD_CREATE_TIME, null},
                                                        {POR_PART_FIELDS.FIELD_CREATE_TIMEZONE, "CN-ZH"},
                                                        {POR_PART_FIELDS.FIELD_EDIT_TIME, null},
                                                        {POR_PART_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"},
                                                    },
                                                    new List<string>());
                if (1 == sqlCommandList.Count && sqlCommandList[0].Length > 20)
                {
                    if (dsParams.Tables.Contains(POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                               new POR_PART_ATTR_FIELDS(),
                                                               dsParams.Tables[POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                               new Dictionary<string, string>() 
                                                               { 
                                                                    {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                                                    {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                               },
                                                               new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                    }
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
                            LogService.LogError("PartInsert Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTrans = null;
                            dbCon.Close();
                            dbCon = null;
                        }
                    }
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "No Table in input paremter.");
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新产品料号数据。
        /// </summary>
        /// <param name="dsParams">包含产品料号数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet PartUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                List<string> sqlCommandList = new List<string>();
                //创建UPDATE SQL
                if (dsParams.Tables.Contains(POR_PART_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[POR_PART_FIELDS.DATABASE_TABLE_NAME];
                    string partName = Convert.ToString(dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE]);
                    string strSql = string.Format("SELECT COUNT(*) FROM POR_PART WHERE PART_NAME ='{0}'", partName.PreventSQLInjection());
                    int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                    if (count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.FMM.PartNameIsExist}");
                        return dsReturn;
                    }
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new POR_PART_FIELDS(),
                           dsParams.Tables[POR_PART_FIELDS.DATABASE_TABLE_NAME],
                           new Dictionary<string, string>() { 
                                                                {POR_PART_FIELDS.FIELD_EDIT_TIME, null},
                                                                {POR_PART_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                             },
                           new List<string>() {},
                           POR_PART_FIELDS.FIELD_PART_KEY);
                }
                //创建更新产品料号自定义属性的UPDATE SQL
                if (dsParams.Tables.Contains(POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList,
                                                           new POR_PART_ATTR_FIELDS(),
                                                           dsParams.Tables[POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                           POR_PART_ATTR_FIELDS.FIELDS_PART_KEY);
                }
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
                        LogService.LogError("PartUpdate Error: " + ex.Message);
                    }
                    finally
                    {
                        dbTrans = null;
                        dbCon.Close();
                        dbCon = null;
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PartUpdate Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据产品料号数据主键删除产品料号数据。
        /// </summary>
        /// <param name="partKey">产品料号主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet PartDelete(string partKey)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            //删除产品料号自定义属性。
            string sqlCommand = "DELETE FROM " + POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME + 
                         " WHERE " + POR_PART_ATTR_FIELDS.FIELDS_PART_KEY + " = '" + partKey.PreventSQLInjection() + "'";
            sqlCommandList.Add(sqlCommand);

            //删除产品料号
            sqlCommand = "DELETE FROM " + POR_PART_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + POR_PART_FIELDS.FIELD_PART_KEY + " = '" + partKey.PreventSQLInjection() + "'";
            sqlCommandList.Add(sqlCommand);

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
                    LogService.LogError("PartDelete Error: " + ex.Message);
                }
                finally
                {
                    dbTrans = null;
                    dbCon.Close();
                    dbCon = null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "Can NOT build sql statements for deleting Part.");
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取产品料号数据的自定义属性列数据。
        /// </summary>
        /// <returns>包含产品料号自定义属性列的数据集对象。</returns>
        public DataSet GetAttributsColumnsForPart()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = @" SELECT BASE_ATTRIBUTE.ATTRIBUTE_KEY, BASE_ATTRIBUTE.ATTRIBUTE_NAME,BASE_ATTRIBUTE.DESCRIPTION,BASE_ATTRIBUTE.DATA_TYPE,'' AS DATA_TYPESTRING
                               FROM BASE_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY
                               WHERE BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY =BASE_ATTRIBUTE.CATEGORY_KEY AND BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME='Uda_Part'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAttributsColumnsForPart Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据产品料号查询产品料号数据。
        /// </summary>
        /// <param name="partName">产品料号名。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public DataSet SearchPart(string partName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = "SELECT * FROM POR_PART";
                if (!string.IsNullOrEmpty(partName))
                {
                    sql = sql + " WHERE PART_NAME LIKE '%" + partName.PreventSQLInjection() + "%'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchPart Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据产品料号主键获取产品料号数据。
        /// </summary>
        /// <param name="partKey">产品料号主键。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public DataSet GetPartByKey(string partKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(partKey))
                {
                    string sql = @" SELECT A.*,B.ENTERPRISE_NAME,B.ENTERPRISE_VERSION,C.ROUTE_NAME ,D.ROUTE_STEP_NAME 
                                    FROM POR_PART A
                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.CUR_ENTERPRISE_VER_KEY
                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                    WHERE A.PART_KEY='"+partKey.PreventSQLInjection()+"'";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = POR_PART_FIELDS.DATABASE_TABLE_NAME;
                    sql = @"SELECT A.PART_KEY ,A.ATTRIBUTE_KEY,A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE,A.EDIT_TIME ,B.DATA_TYPE AS DATA_TYPE,A.EDITOR   
                            FROM POR_PART_ATTR A 
                            LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY=B.ATTRIBUTE_KEY
                            WHERE A.PART_KEY = '" +partKey.PreventSQLInjection()+ "'";
                    DataTable udaDataTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    udaDataTable.TableName =POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME;

                    dsReturn.Merge(udaDataTable, true, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPartByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据产品料号料号获取产品料号数据。
        /// </summary>
        /// <param name="partNumber">产品料号料号。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public  DataSet GetPartByPartNumber(string partNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(partNumber))
                {
                    string sql = @" SELECT A.*,B.ENTERPRISE_NAME,B.ENTERPRISE_VERSION,C.ROUTE_NAME ,D.ROUTE_STEP_NAME 
                                    FROM POR_PART A
                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.CUR_ENTERPRISE_VER_KEY
                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY 
                                    WHERE A.PART_NAME='" + partNumber.PreventSQLInjection() + "'";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = POR_PART_FIELDS.DATABASE_TABLE_NAME;
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPartByPartNumber Error: " + ex.Message);
            }
            return dsReturn;
        }
       /// <summary>
       /// 根据产品料号料号和工步名称获取产品料号数据。
       /// </summary>
       /// <remarks>
       /// 从POR_PART表和V_PROCESS_PLAN视图获取产品料号信息、途程、途程组、途程中的工步等表数据
       /// </remarks>
       /// <param name="partNumber">产品料号物料号</param>
       /// <param name="routStepName">工步名称,可以为空</param>
       /// <returns>
       /// 包含产品料号数据的数据集对象。
       /// [PART_KEY,PART_VERSION,PART_NAME,PART_STATUS,PART_TYPE,PART_MODULE,CUR_ENTERPRISE_VER_KEY,CUR_ROUTE_VER_KEY,
       ///  ROUTE_STEP_KEY,ENTERPRISE_NAME,ENTERPRISE_VERSION,ROUTE_STEP_NAME,ROUTE_NAME,Descriptions]
       /// </returns>
       public DataSet GetPartByPartNumberAndStepName(string partNumber, string routStepName)
       {
           DataSet dsReturn = new DataSet();
           try
           {
               if (!string.IsNullOrEmpty(partNumber))
               {

                   string sql = @" SELECT A.PART_KEY,A.PART_VERSION,A.PART_NAME,A.PART_STATUS,A.PART_TYPE,A.PART_MODULE,A.CUR_ENTERPRISE_VER_KEY,A.CUR_ROUTE_VER_KEY,
                                          B.ROUTE_STEP_KEY,B.ENTERPRISE_NAME,B.ENTERPRISE_VERSION,B.ROUTE_STEP_NAME,B.ROUTE_NAME,B.DESCRIPTIONS 
                                   FROM POR_PART A,V_PROCESS_PLAN B
                                   WHERE A.CUR_ENTERPRISE_VER_KEY = B.ROUTE_ENTERPRISE_VER_KEY
                                   AND A.CUR_ROUTE_VER_KEY = B.ROUTE_ROUTE_VER_KEY
                                   AND A.PART_NAME='" + partNumber.PreventSQLInjection() + "'";
                   if (!string.IsNullOrEmpty(routStepName))
                   {
                       sql += " AND B.ROUTE_STEP_NAME ='" + routStepName.PreventSQLInjection() + "'";
                   }
                   sql += " ORDER BY B.ROUTE_STEP_SEQ";
                   dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                   dsReturn.Tables[0].TableName = POR_PART_FIELDS.DATABASE_TABLE_NAME;
                   FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
               }
           }
           catch (Exception ex)
           {
               FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               LogService.LogError("GetPartByPartNumberAndStepName Error: " + ex.Message);
           }
           return dsReturn;
       }

       #region IPartEngine 成员


       public DataSet GetPartType()
       {
           DataSet dsReturn = new DataSet();
           string sqlCommon = string.Empty;
           try
           {
               sqlCommon = @"SELECT PROMODEL_NAME AS PART_TYPE FROM BASE_PRODUCTMODEL WHERE ISFLAG = 1 ";
               dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);

               FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
           }
           catch (Exception ex)
           {
               FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               LogService.LogError("CheckPart Error: " + ex.Message);
           }
           return dsReturn;
       }

       #endregion
    }
}
