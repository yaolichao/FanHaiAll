using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FanHai.Hemera.Modules.RBAC
{
    /// <summary>
    /// 系统资源数据管理类。
    /// </summary>
    public class ResourceEngine:AbstractEngine,IResourceEngine
    {
        private  Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ResourceEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 添加系统资源。
        /// </summary>
        /// <param name="dsParams">包含系统资源的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddResource(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();  
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            if (null != dsParams && dsParams.Tables.Contains(RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME];
                string resourceName = Convert.ToString(dtParams.Rows[0][RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_NAME]);
                string groupKey=Convert.ToString(dtParams.Rows[0][RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_GROUP_KEY]);
                //判断资源名称是否存在。
                string sqlCommand = @"SELECT COUNT(*) FROM RBAC_RESOURCE WHERE 
                                      RESOURCE_NAME ='" + resourceName.PreventSQLInjection() + "' AND " +
                                     "RESOURCE_GROUP_KEY='" + groupKey.PreventSQLInjection()+ "'";
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.ResourceEngine.ResourceNameAlreadyExist}");
                    return dsReturn;
                }
                //判断资源代码是否存在。
                string resourceCode=Convert.ToString(dtParams.Rows[0][RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_CODE]);
                string strSql = @"SELECT  COUNT(*) FROM RBAC_RESOURCE WHERE 
                                 RESOURCE_CODE='" +resourceCode.PreventSQLInjection()+"' AND "+
                                 "RESOURCE_GROUP_KEY='" + groupKey.PreventSQLInjection() + "'";
                count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.ResourceEngine.ResourceCodeAlreadyExist}");
                    return dsReturn;
                }
                //生成INSERT SQL
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                        new RBAC_RESOURCE_FIELDS(),
                                                        dsParams.Tables[RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                        {RBAC_RESOURCE_FIELDS.FIELD_CREATE_TIME, null},                                                            
                                                        },
                                                        new List<string>());
                //插入数据。
                if (sqlCommandList.Count > 0)
                {
                    dbconn = db.CreateConnection();
                    dbconn.Open();
                    //Create Transaction  
                    dbtran = dbconn.BeginTransaction();
                    try
                    {
                        foreach (string sql in sqlCommandList)
                        {
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        }
                        dbtran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                    catch (Exception ex)
                    {
                        dbtran.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("AddResource Error: " + ex.Message);
                    }
                    finally
                    {
                        //Close Connection
                        dbconn.Close();
                        dbtran = null;
                        dbconn = null;
                    }
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;            
        }

        /// <summary>
        /// 更新资源数据。
        /// </summary>
        /// <param name="resourceKey">包含资源数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateResource(DataSet dsParams)
        {
           DataSet dsReturn = new DataSet();
           DbConnection dbconn = null;
           DbTransaction dbtran = null;
           string resourceCode = "", groupKey = "";
           if (null != dsParams && dsParams.Tables.Contains(RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME))
           {
               DataTable dataTable = dsParams.Tables[RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME];
               for (int i = 0; i < dataTable.Rows.Count; i++)
               {
                   //资源代码
                   if (dataTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_CODE)
                   {
                       resourceCode = dataTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE].ToString();
                   }
                   //资源组主键。
                   if (dataTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_GROUP_KEY)
                   {
                       groupKey = dataTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE].ToString();
                   }
               }
               //如果资源代码有修改，判断新增的资源代码是否存在。
               if (resourceCode != "" && groupKey!="")
               {
                   string strSql = @"SELECT COUNT(*) FROM RBAC_RESOURCE WHERE RESOURCE_CODE='" +resourceCode.PreventSQLInjection()+"' AND " +
                                     "RESOURCE_GROUP_KEY='" +groupKey+ "'";
                   int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                   if (count>0)
                   {
                       FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.ResourceEngine.ResourceCodeAlreadyExist}");
                       return dsReturn;
                   }
               }
               //生成UPDATE SQL
               List<string> sqlCommandList = new List<string>();
               DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                       new RBAC_RESOURCE_FIELDS(),
                                                       dsParams.Tables[RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME],
                                                       new Dictionary<string, string>() 
                                                       {                                                             
                                                        {RBAC_RESOURCE_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                       },
                                                       new List<string>() 
                                                       {
                                                        RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_KEY                                                            
                                                       },
                                                       RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_KEY);
               //更新数据。
               if (sqlCommandList.Count > 0)
               {
                   dbconn = db.CreateConnection();
                   dbconn.Open();
                   //Create Transaction  
                   dbtran = dbconn.BeginTransaction();
                   try
                   {
                       foreach (string sql in sqlCommandList)
                       {
                           db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                       }
                       dbtran.Commit();
                       FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                   }
                   catch (Exception ex)
                   {
                       dbtran.Rollback();
                       FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                       LogService.LogError("UpdateResource Error: " + ex.Message);
                   }
                   finally
                   {
                       dbtran = null;
                       //Close Connection
                       dbconn.Close();
                       dbconn = null;
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
        /// 删除资源数据。
        /// </summary>
        /// <param name="resourceKey">资源主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteResource(string resourceKey)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sql = "";
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                sql = "DELETE FROM RBAC_RESOURCE WHERE RESOURCE_KEY='"+resourceKey.PreventSQLInjection()+"'";
                db.ExecuteNonQuery(dbtran,CommandType.Text,sql);
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message.ToString());
                LogService.LogError("DeleteResource Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                if (dbconn != null)
                {
                    dbconn.Close();
                }
                dbtran = null;
                dbconn = null;
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取资源数据。
        /// </summary>
        /// <param name="resourceKey">资源主键。</param>
        /// <returns>包含资源主键的数据集对象。</returns>
        public DataSet GetResource(string resourceGroupKey)
        {
          DataSet dsReturn = new DataSet();
          //define sql 
          string sql = "";
          try
          {
              if (resourceGroupKey != "")
              {
                  sql = "SELECT * FROM RBAC_RESOURCE WHERE RESOURCE_GROUP_KEY='" + resourceGroupKey.PreventSQLInjection() + "'";
              }
              else
              {
                  sql = "SELECT * FROM RBAC_RESOURCE";
              }
              dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
              FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);

          }
          catch (Exception ex)
          {
              FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
              LogService.LogError("GetResource Error: " + ex.Message);
          }
          return dsReturn;
        }
    }
}
