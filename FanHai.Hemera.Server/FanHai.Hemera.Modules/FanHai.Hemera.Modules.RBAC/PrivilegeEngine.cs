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
    /// 系统权限数据管理类。
    /// </summary>
    public partial class PrivilegeEngine : AbstractEngine, IPrivilegeEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PrivilegeEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 保存系统权限。
        /// </summary>
        /// <param name="dsParams">包含系统权限数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SavePrivilege(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                if (dsParams.Tables.Contains(RBAC_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME))
                {
                    AddPrivilege(db,dbtran,dsParams);
                }
                if (dsParams.Tables.Contains(RBAC_ROLE_OWN_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME))
                {                   
                    SaveRolePrivilege(db,dbtran,dsParams);
                }
                if (dsParams.Tables.Contains(RBAC_USER_OWN_PRIVILEGES_FIELDS.DATABASE_TABLE_NAME))
                {                   
                    SaveUserPrivilege(db,dbtran,dsParams);
                }                
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                dbtran.Rollback();
                LogService.LogError("SavePrivilege Error: " + ex.Message);
            }
            finally
            {
                dbconn.Close();
            }
            return dsReturn;            
        }
        /// <summary>
        /// 获取系统权限数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含系统权限数据的数据集对象。</returns>
        public DataSet GetPrivilege(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    if (dtParams.Rows.Count > 0)
                    {
                        string rowKey = Convert.ToString(dtParams.Rows[0]["ROW_KEY"]);
                        string operationGroupKey = Convert.ToString(dtParams.Rows[0][RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY]);
                        string resourceGroupKey = Convert.ToString(dtParams.Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY]);
                        string tableName = dtParams.Rows[0]["TABLE_NAME"].ToString();
                        if (tableName == RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME)
                        {
                            sql = @"SELECT
                            A.PRIVILEGE_KEY,
                            A.RESOURCE_KEY,
                            A.OPERATION_KEY 
                            FROM RBAC_PRIVILEGE A,
                            RBAC_ROLE_OWN_PRIVILEGE B
                            WHERE B.ROLE_KEY='" + rowKey.PreventSQLInjection() + "' AND A.PRIVILEGE_KEY=B.PRIVILEGE_KEY";
                        }
                        else if (tableName == RBAC_USER_FIELDS.DATABASE_TABLE_NAME)
                        {
                            sql = @"SELECT
                            A.PRIVILEGE_KEY,
                            A.RESOURCE_KEY,
                            A.OPERATION_KEY 
                            FROM RBAC_PRIVILEGE A,
                            RBAC_USER_OWN_PRIVILEGES B
                            WHERE B.USER_KEY='" + rowKey.PreventSQLInjection() + "' AND A.PRIVILEGE_KEY=B.PRIVILEGE_KEY";
                        }

                        if (resourceGroupKey != "")
                        {
                            sql += " AND A.RESOURCE_KEY IN(SELECT RESOURCE_KEY FROM RBAC_RESOURCE WHERE RESOURCE_GROUP_KEY='" + resourceGroupKey.PreventSQLInjection() + "')";
                        }
                        if (operationGroupKey != "")
                        {
                            sql += " AND A.OPERATION_KEY IN(SELECT OPERATION_KEY FROM RBAC_OPERATION WHERE OPERATION_GROUP_KEY='" + operationGroupKey.PreventSQLInjection() + "')";
                        }
                        dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrivilege Error: " + ex.Message);
            }
            return dsReturn;
        }       
        /// <summary>
        /// 添加权限。
        /// </summary>
        private void AddPrivilege(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            DataTable dataTable = dsParams.Tables[RBAC_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME];
            List<string> sqlCommandList = new List<string>();
            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                    new RBAC_PRIVILEGE_FIELDS(),
                                                    dsParams.Tables[RBAC_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME],
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {RBAC_PRIVILEGE_FIELDS.FIELD_CREATE_TIME, null},                                                            
                                                    },
                                                    new List<string>());
            if (sqlCommandList.Count > 0)
            {
                
                foreach (string sql in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }
            }                    
        }
        /// <summary>
        /// 保存角色权限。
        /// </summary>
        private void SaveRolePrivilege(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            string sqlCommand = "";
            if (dsParams.Tables.Contains(RBAC_ROLE_OWN_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_ROLE_OWN_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME];
                RBAC_ROLE_OWN_PRIVILEGE_FIELDS roleOwnPrivilegeFields = new RBAC_ROLE_OWN_PRIVILEGE_FIELDS();
                RBAC_PRIVILEGE_FIELDS privilegeFields = new RBAC_PRIVILEGE_FIELDS();

                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    WhereConditions delCondition = new WhereConditions(RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY, dtParams.Rows[i][RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY].ToString());
                    OperationAction action = (OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                    switch (action)
                    {
                        case OperationAction.New:
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(roleOwnPrivilegeFields, dtParams, i, 
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                        },
                                                        new List<string>(){
                                                             COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                        });

                            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                            break;
                        case OperationAction.Delete:
                            string roleKey = Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_ROLE_KEY]);
                            sqlCommand = DatabaseTable.BuildDeleteSqlStatement(roleOwnPrivilegeFields, delCondition);
                            sqlCommand += " AND ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);

                            sqlCommand = DatabaseTable.BuildDeleteSqlStatement(privilegeFields,delCondition);
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                            break;
                        default:
                            break;
                    }
                }
            }  
        }
        /// <summary>
        /// 保存用户权限。
        /// </summary>
        private void SaveUserPrivilege(Database db, DbTransaction dbtran, DataSet dsParams)
        {  
            string sqlCommand = "";
            if (dsParams.Tables.Contains(RBAC_USER_OWN_PRIVILEGES_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_USER_OWN_PRIVILEGES_FIELDS.DATABASE_TABLE_NAME];
                RBAC_USER_OWN_PRIVILEGES_FIELDS userOwnPrivilegeFields = new RBAC_USER_OWN_PRIVILEGES_FIELDS();
                RBAC_PRIVILEGE_FIELDS privilegeFields = new RBAC_PRIVILEGE_FIELDS();

                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    string privilegeKey=Convert.ToString(dtParams.Rows[i][RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_PRIVILEGE_KEY]);
                    WhereConditions delCondition = new WhereConditions(RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_PRIVILEGE_KEY, privilegeKey);
                    OperationAction action = (OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                    switch (action)
                    {
                        case OperationAction.New:
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(userOwnPrivilegeFields, dtParams, i, 
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDIT_TIME,null},                                                            
                                                        },
                                                        new List<string>(){
                                                            COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                        });

                            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                            break;
                        case OperationAction.Delete:
                            string userKey = Convert.ToString(dtParams.Rows[i][RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_USER_KEY]);
                            sqlCommand = DatabaseTable.BuildDeleteSqlStatement(userOwnPrivilegeFields, delCondition);
                            sqlCommand += " AND USER_KEY='" + userKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);

                            sqlCommand = DatabaseTable.BuildDeleteSqlStatement(privilegeFields, delCondition);
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                            break;
                        default:
                            break;
                    }
                }
            }  
        }
    }
}
