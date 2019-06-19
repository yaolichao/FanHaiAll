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
    /// 角色数据管理类。
    /// </summary>
    public class RoleEngine:AbstractEngine,IRoleEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RoleEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据角色主键获取角色数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含角色数据的数据集对象。</returns>
        public DataSet GetRoleInfo(string roleKey)
        {
            DataSet dsReturn = new DataSet();
            //define sql 
            string sql = "";            
            try
            {                
                if (!string.IsNullOrEmpty(roleKey))
                {
                    sql = "SELECT * FROM RBAC_ROLE WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                }
                else
                {
                    sql = "SELECT * FROM RBAC_ROLE";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRoleInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增角色。
        /// </summary>
        /// <param name="dataset">包含角色数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>        
        public DataSet AddRole(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                RBAC_ROLE_FIELDS roleFields = new RBAC_ROLE_FIELDS();
                if (null != dsParams && dsParams.Tables.Contains(RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dsParams.Tables[RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string roleName = Convert.ToString(hashData[RBAC_ROLE_FIELDS.FIELD_ROLE_NAME]);
                    sql = "SELECT COUNT(*) FROM RBAC_ROLE WHERE ROLE_NAME ='" + roleName.PreventSQLInjection()+ "'";
                    int count=Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                    if (count>0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.RoleEngine.RollAlreadyExist}");
                    }
                    else
                    {
                        hashData.Add(RBAC_ROLE_FIELDS.FIELD_CREATE_TIME,null);
                        sql = DatabaseTable.BuildInsertSqlStatement(roleFields, hashData, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        dbtran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddRole Error: " + ex.Message);
            }
            finally
            {
                if (dbtran != null)
                {
                    dbtran = null;
                }
                //Close Connection
                if (dbconn != null)
                {
                    dbconn.Close();
                    dbconn = null;
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新角色。
        /// </summary>
        /// <param name="dsParams">包含角色数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateRole(DataSet dsParams)
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
                RBAC_ROLE_FIELDS roleFields = new RBAC_ROLE_FIELDS();
                if (null != dsParams && dsParams.Tables.Contains(RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dsParams.Tables[RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    hashData.Add(RBAC_ROLE_FIELDS.FIELD_EDIT_TIME, null);
                    string roleKey=Convert.ToString(hashData[RBAC_ROLE_FIELDS.FIELD_ROLE_KEY]);
                    WhereConditions wc = new WhereConditions(RBAC_ROLE_FIELDS.FIELD_ROLE_KEY, roleKey);
                    hashData.Remove(RBAC_ROLE_FIELDS.FIELD_ROLE_KEY);
                    sql = DatabaseTable.BuildUpdateSqlStatement(roleFields, hashData, wc);
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    dbtran.Commit();
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateRole Error: " + ex.Message);
            }
            finally
            {
                if (dbtran != null)
                {
                    dbtran = null;
                }
                //Close Connection
                if (dbconn != null)
                {
                    dbconn.Close();
                    dbconn = null;
                }
            }

            return dsReturn;
        }
        /// <summary>
        /// 删除角色。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteRole(string roleKey)
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
                if (roleKey != "")
                {
                    //delete role
                    sql = "DELETE FROM RBAC_ROLE WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete users in the role
                    sql = @"DELETE FROM RBAC_USER_IN_ROLE WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete privilege
                    sql = @"DELETE FROM RBAC_PRIVILEGE WHERE PRIVILEGE_KEY IN 
                      (SELECT PRIVILEGE_KEY FROM RBAC_ROLE_OWN_PRIVILEGE WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "')";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete role own privilege
                    sql = @"DELETE FROM RBAC_ROLE_OWN_PRIVILEGE WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete role own lines
                    sql = "DELETE FROM RBAC_ROLE_OWN_LINES WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete role own operations
                    sql = "DELETE FROM RBAC_ROLE_OWN_OPERATION WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }                
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteRole Error: " + ex.Message);
            }
            finally
            {
                if (dbtran != null)
                {
                    dbtran = null;
                }
                //Close Connection
                if (dbconn != null)
                {
                    dbconn.Close();
                    dbconn = null;
                }
            }
            return dsReturn;
        }        
    }
}
