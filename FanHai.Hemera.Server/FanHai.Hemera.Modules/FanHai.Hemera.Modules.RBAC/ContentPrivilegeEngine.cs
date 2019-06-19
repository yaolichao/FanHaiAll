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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.RBAC
{
    /// <summary>
    /// 系统权限数据管理类。
    /// </summary>
    partial class PrivilegeEngine
    {
        /// <summary>
        /// 根据角色获取拥有的线别名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含线别名称的数据集对象。</returns>
        public DataSet GetLinesOwnedByRole(string roleKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT LINE_NAME FROM RBAC_ROLE_OWN_LINES WHERE ROLE_KEY='"+roleKey.PreventSQLInjection()+"'";
                dsReturn = db.ExecuteDataSet(CommandType.Text,sql);                
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                LogService.LogError("GetLinesOwnedByRole Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不属于角色的线别名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含线别名称的数据集对象。</returns>
        public DataSet GetLinesRoleNotOwn(string roleKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT LINE_NAME FROM FMM_PRODUCTION_LINE
                        WHERE LINE_NAME NOT IN
                        (SELECT LINE_NAME FROM RBAC_ROLE_OWN_LINES WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "')";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                LogService.LogError("GetLinesRoleNotOwn Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }    
        /// <summary>
        /// 保存角色拥有的线别。
        /// </summary>
        /// <param name="dsParams">包含角色拥有线别数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveLinesOfRole(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(RBAC_ROLE_OWN_LINES_FIELDS.DATABASE_TABLE_NAME))
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();

                DataTable dtParams = dsParams.Tables[RBAC_ROLE_OWN_LINES_FIELDS.DATABASE_TABLE_NAME];
                RBAC_ROLE_OWN_LINES_FIELDS roleOwnLineFields = new RBAC_ROLE_OWN_LINES_FIELDS();
                try
                {
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string roleKey=Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_LINES_FIELDS.FIELD_ROLE_KEY]);
                        WhereConditions delCondition = new WhereConditions(RBAC_ROLE_OWN_LINES_FIELDS.FIELD_ROLE_KEY,roleKey);
                        OperationAction action = (OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                        switch (action)
                        {
                            case OperationAction.New:
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(roleOwnLineFields, dtParams,
                                                    i, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                    },
                                                    new List<string>(){
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                    });
                                
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            case OperationAction.Delete: 
                                string lineName=Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_LINES_FIELDS.FIELD_LINE_NAME]);
                                sqlCommand = DatabaseTable.BuildDeleteSqlStatement(roleOwnLineFields, delCondition);
                                sqlCommand += " AND LINE_NAME='" + lineName.PreventSQLInjection()+ "'";
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            default:
                                break;
                        }
                    }
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("SaveLinesOfRole Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbtran.Rollback();
                }
                finally
                {
                    dbtran = null;
                    //Close Connection
                    dbconn.Close();
                    dbconn = null;
                }               
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据角色获取拥有的工序名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含工序名称数据的数据集对象。</returns>
        public DataSet GetOperationOwnedByRole(string roleKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT OPERATION_NAME FROM RBAC_ROLE_OWN_OPERATION WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                LogService.LogError("GetOperationOwnedByRole Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不属于角色的工序名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含工序名称数据的数据集对象。</returns>
        public DataSet GetOperationRoleNotOwn(string roleKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT DISTINCT ROUTE_OPERATION_NAME FROM POR_ROUTE_OPERATION_VER 
                       WHERE OPERATION_STATUS='1'
                       AND ROUTE_OPERATION_NAME NOT IN(SELECT OPERATION_NAME FROM RBAC_ROLE_OWN_OPERATION WHERE ROLE_KEY='" + roleKey.PreventSQLInjection() + "')";              
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                LogService.LogError("GetOperationRoleNotOwn Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存角色拥有的工序数据。
        /// </summary>
        /// <param name="dsParams">包含角色拥有工序数据的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveOperationsOfRole(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME))
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();

                DataTable dtParams = dsParams.Tables[RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME];
                RBAC_ROLE_OWN_OPERATION_FIELDS roleOwnOperationFields = new RBAC_ROLE_OWN_OPERATION_FIELDS();
                try
                {
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string roleKey = Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_ROLE_KEY]);
                        WhereConditions delCondition = new WhereConditions(RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_ROLE_KEY,roleKey);
                        OperationAction action = (OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                        switch (action)
                        {
                            case OperationAction.New:
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(roleOwnOperationFields, 
                                                    dtParams, i, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDIT_TIME,null},                                                            
                                                    },
                                                    new List<string>(){
                                                         COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                    });

                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            case OperationAction.Delete:
                                string operationName=Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME]);
                                sqlCommand = DatabaseTable.BuildDeleteSqlStatement(roleOwnOperationFields, delCondition);
                                sqlCommand += " AND OPERATION_NAME='" + operationName.PreventSQLInjection() + "'";
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            default:
                                break;
                        }
                    }
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("SaveOperationsOfRole Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbtran.Rollback();
                }
                finally
                {
                    dbtran=null;
                    //Close Connection
                    dbconn.Close();
                    dbconn=null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存角色拥有的线上仓数据。
        /// </summary>
        /// <param name="dsParams">包含角色拥有线上仓数据的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveStoreOfRole(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(RBAC_ROLE_OWN_STORE_FIELDS.DATABASE_TABLE_NAME))
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();

                DataTable dtParams = dsParams.Tables[RBAC_ROLE_OWN_STORE_FIELDS.DATABASE_TABLE_NAME];
                RBAC_ROLE_OWN_STORE_FIELDS roleOwnStoreFields = new RBAC_ROLE_OWN_STORE_FIELDS();
                try
                {
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string roleKey=Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_ROLE_KEY]);
                        WhereConditions delCondition = new WhereConditions(RBAC_ROLE_OWN_STORE_FIELDS.FIELD_ROLE_KEY, roleKey);
                        OperationAction action = (OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                        switch (action)
                        {
                            case OperationAction.New:
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(roleOwnStoreFields, 
                                                    dtParams, i, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                    },
                                                    new List<string>(){
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                    });

                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            case OperationAction.Delete:
                                string storeName = Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME]);
                                sqlCommand = DatabaseTable.BuildDeleteSqlStatement(roleOwnStoreFields, delCondition);
                                sqlCommand += " AND STORE_NAME='" + storeName.PreventSQLInjection() + "'";
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            default:
                                break;
                        }
                    }
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    LogService.LogError("SaveStoreOfRole Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbtran.Rollback();
                }
                finally
                {
                    dbtran = null;
                    //Close Connection
                    dbconn.Close();
                    dbconn = null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取角色的线上仓名称数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <param name="type">类型，是否拥有（SELECT：角色拥有，UNSELECT：角色不拥有。）</param>
        /// <returns>包含线上仓名称数据的数据集对象。</returns>
        public DataSet GetStoreOfRole(string roleKey,string type)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (string.IsNullOrEmpty(roleKey) || string.IsNullOrEmpty(type))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "输入参数为空");
                    return dsReturn;
                }
                else
                {
                    if (type.ToUpper().Trim() == "SELECT")
                    {
                        sql = string.Format(@"SELECT STORE_NAME 
                                            FROM RBAC_ROLE_OWN_STORE WHERE ROLE_KEY='{0}'",
                                            roleKey.PreventSQLInjection());
                    }
                    else if (type.ToUpper().Trim() == "UNSELECT")
                    {
                        sql =string.Format(@"SELECT DISTINCT STORE_NAME FROM WST_STORE
                                          WHERE OBJECT_STATUS='1'
                                          AND STORE_NAME NOT IN (SELECT STORE_NAME FROM RBAC_ROLE_OWN_STORE WHERE ROLE_KEY='{0}')",
                                          roleKey.PreventSQLInjection());
                    }
                    else
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("输入参数错误:{0}。", type));
                        return dsReturn;
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetStoreOfRole Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取角色的用户数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <param name="type">类型，是否拥有（SELECT：角色拥有，UNSELECT：角色不拥有。）</param>
        /// <returns>包含用户数据的数据集对象。</returns>
        public DataSet GetUserOfRole(string roleKey, string type)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (string.IsNullOrEmpty(roleKey) || string.IsNullOrEmpty(type))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "输入参数为空");
                    return dsReturn;
                }
                else
                {
                    if (type.ToUpper().Trim() == "SELECT")
                    {
                        sql = string.Format(@"SELECT 'False' AS COLUMN_CHECK,u.USERNAME,u.BADGE,u.USER_KEY
                                            FROM RBAC_USER_IN_ROLE t, RBAC_USER u
                                            WHERE t.USER_KEY = u.USER_KEY
                                            AND t.ROLE_KEY = '{0}'",
                                            roleKey.PreventSQLInjection());
                    }
                    else if (type.ToUpper().Trim() == "UNSELECT")
                    {
                        sql = string.Format(@"SELECT 'False' AS COLUMN_CHECK,a.USERNAME,a.BADGE, a.USER_KEY
                                            FROM RBAC_USER a
                                            WHERE a.USER_KEY NOT IN (SELECT t.USER_KEY FROM RBAC_USER_IN_ROLE t WHERE t.ROLE_KEY = '{0}')",
                                            roleKey.PreventSQLInjection());
                    }
                    else
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("输入参数错误:{0}。", type));
                        return dsReturn;
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                
            }
            catch (Exception ex)
            {
                LogService.LogError("GetUserOfRole Error:"+ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取角色拥有的机台状态数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <param name="type">类型，是否拥有（SELECT：角色拥有，UNSELECT：角色不拥有。）</param>
        /// <returns>包含机台状态数据的数据集对象。</returns>
        public DataSet GetStatusOfRole(string roleKey, string type)//modify by qym 20120606
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (string.IsNullOrEmpty(roleKey) || string.IsNullOrEmpty(type))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "输入参数为空");
                    return dsReturn;
                }
                else
                {
                    if (type.ToUpper().Trim() == "SELECT")
                    {
                        sql = string.Format(@"SELECT EQUIPMENT_STATE_NAME
                                            FROM RBAC_ROLE_OWN_STATUS 
                                            WHERE ROLE_KEY='{0}'",
                                            roleKey.PreventSQLInjection());
                    }
                    else if (type.ToUpper().Trim() == "UNSELECT")
                    {
                        sql = string.Format(@"SELECT DISTINCT EQUIPMENT_STATE_NAME
                                            FROM EMS_EQUIPMENT_STATES
                                            WHERE  EQUIPMENT_STATE_NAME NOT IN(SELECT EQUIPMENT_STATE_NAME 
                                                                               FROM RBAC_ROLE_OWN_STATUS 
                                                                               WHERE ROLE_KEY='{0}')",
                                            roleKey.PreventSQLInjection());
                    }
                    else
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("输入参数错误:{0}。",type));
                        return dsReturn;
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetStoreOfRole Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存角色的机台状态权限
        /// </summary>
        /// <param name="dsParams">包含角色拥有机台状态数据的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveStatusOfRole(DataSet dsParams)//add by qym 20120606 13:40
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(RBAC_ROLE_OWN_STATUS_FIELDS.DATABASE_TABLE_NAME))
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
  
                DataTable dtParams = dsParams.Tables[RBAC_ROLE_OWN_STATUS_FIELDS.DATABASE_TABLE_NAME];
                RBAC_ROLE_OWN_STATUS_FIELDS roleOwnStatusFields = new RBAC_ROLE_OWN_STATUS_FIELDS();
                try
                {
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string roleKey=Convert.ToString( dtParams.Rows[i][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_ROLE_KEY]);
                        WhereConditions delCondition = new WhereConditions(RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_ROLE_KEY,roleKey);
                        OperationAction action =(OperationAction) Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                        switch (action)
                        {
                            case OperationAction.New:
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(roleOwnStatusFields, 
                                                    dtParams, i, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                    },
                                                    new List<string>(){
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                    });

                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            case OperationAction.Delete:
                                string stateName=Convert.ToString(dtParams.Rows[i][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME]);
                                sqlCommand = DatabaseTable.BuildDeleteSqlStatement(roleOwnStatusFields, delCondition);
                                sqlCommand += " AND EQUIPMENT_STATE_NAME='" + stateName.PreventSQLInjection() + "'";
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            default:
                                break;
                        }
                    }
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("SaveStatusOfRole Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbtran.Rollback();
                }
                finally
                {
                    dbtran=null;
                    //Close Connection
                    dbconn.Close();
                    dbconn=null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;
        }
    }
}
