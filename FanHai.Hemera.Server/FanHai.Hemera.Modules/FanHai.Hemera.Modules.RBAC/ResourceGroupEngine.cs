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
    /// 系统资源组数据管理类。
    /// </summary>
    public class ResourceGroupEngine : AbstractEngine,IResourceGroupEngine
    {
        private Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ResourceGroupEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 添加资源组。
        /// </summary>
        /// <param name="dsParams">包含资源组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddResourceGroup(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();           
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            if (null != dsParams &&　dsParams.Tables.Contains(RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME];
                //判断资源组名称是否存在。
                string groupName = Convert.ToString(dtParams.Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME]);
                string sqlCommand = "SELECT COUNT(*) FROM RBAC_RESOURCE_GROUP WHERE GROUP_NAME ='" + groupName.PreventSQLInjection()+ "'";
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.ResourceGroupEngine.GroupNameAlreadyExist}");
                    return dsReturn;
                }
                //判断资源组代码是否存在。
                string groupCode=Convert.ToString(dtParams.Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE]);
                string strSql = @"SELECT COUNT(*) FROM RBAC_RESOURCE_GROUP WHERE RESOURCE_GROUP_CODE='" + groupCode.PreventSQLInjection() + "'";
                count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.ResourceGroupEngine.GroupCodeAlreadyExist}");
                    return dsReturn;
                }
                //生成INSERT SQL
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                        new RBAC_RESOURCE_GROUP_FIELDS(),
                                                        dsParams.Tables[RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {RBAC_RESOURCE_GROUP_FIELDS.FIELD_CREATE_TIME, null}, 
                                                            {RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE,"01"},
                                                        },
                                                        new List<string>());
                //插入到数据库。
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
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        dbtran.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("AddResourceGroup Error: " + ex.Message);
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
            return dsReturn;            
        }
        /// <summary>
        /// 更新资源组。
        /// </summary>
        /// <param name="dsParams">包含资源组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateResourceGroup(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string resourceGroupCode = "";
            if (null != dsParams && dsParams.Tables.Contains(RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME];  
                //判断修改的资源组代码是否存在。
                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    if (dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE)
                    {
                        resourceGroupCode = dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE].ToString();
                    }
                }
                if (!string.IsNullOrEmpty(resourceGroupCode))
                {
                    string strSql = @"SELECT COUNT(*) FROM RBAC_RESOURCE_GROUP WHERE RESOURCE_GROUP_CODE='" +resourceGroupCode.PreventSQLInjection()+ "'";
                    int count= Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                    if (count>0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.ResourceGroupEngine.GroupCodeAlreadyExist}");
                        return dsReturn;
                    }
                }
                //生成INSERT SQL
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new RBAC_RESOURCE_GROUP_FIELDS(),
                                                        dsParams.Tables[RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {RBAC_RESOURCE_GROUP_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                        },
                                                        new List<string>() 
                                                        {
                                                            RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY                                                            
                                                        },
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY);
                //更新数据
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
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        dbtran.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("UpdateResourceGroup Error: " + ex.Message);
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
            return dsReturn;            
        }
        /// <summary>
        /// 删除资源组数据。
        /// </summary>
        /// <param name="resourceGroupKey">资源组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteResourceGroup(string resourceGroupKey)
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
                if (resourceGroupKey != "")
                {
                    sql = "DELETE FROM RBAC_RESOURCE_GROUP WHERE RESOURCE_GROUP_KEY='" + resourceGroupKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    sql = "DELETE FROM RBAC_RESOURCE WHERE RESOURCE_GROUP_KEY='" + resourceGroupKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }     
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message.ToString());
                LogService.LogError("DeleteResourceGroup Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                dbconn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取资源组数据。
        /// </summary>
        /// <param name="resourceGroupKey">资源组主键。</param>
        /// <returns>包含资源组数据的数据集对象。</returns>
        public DataSet GetResourceGroup(string resourceGroupKey)
        {
            DataSet dsReturn = new DataSet();
            //define sql 
            string sql = "";
            try
            {
                if (resourceGroupKey == "")
                {
                    sql = "SELECT RESOURCE_GROUP_KEY,GROUP_NAME FROM RBAC_RESOURCE_GROUP order by RESOURCE_GROUP_CODE,GROUP_NAME ";
                }
                else
                {
                    sql = "SELECT * FROM RBAC_RESOURCE_GROUP WHERE RESOURCE_GROUP_KEY='" + resourceGroupKey.PreventSQLInjection() + "' order by RESOURCE_GROUP_CODE,GROUP_NAME";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetResourceGroup Error: " + ex.Message);
            }
            return dsReturn;
        }       
    }
}
