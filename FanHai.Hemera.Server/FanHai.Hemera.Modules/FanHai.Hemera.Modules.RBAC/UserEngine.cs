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
    /// 对用户或操作员信息进行操作的具体引擎类。
    /// </summary>
    /// <see cref="IUserEngine"/>
    /// <see cref="AbstractEngine"/>
    public class UserEngine :AbstractEngine, IUserEngine
    {
        private Database db=null;  //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UserEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 获取用户信息。根据用户工号获取用户信息。
        /// </summary>
        /// <param name="ds">
        /// 数据集对象。数据集对象中必须包含表名为<see cref="RBAC_USER_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// 数据表中包含名称为<see cref="RBAC_USER_FIELDS.FIELD_USERNAME"/>的列，用于存放员工号。为查询提供查询条件。
        /// </param>
        /// <returns>
        /// 包含查询得到的用户信息的数据集对象。
        /// </returns>
        public DataSet CheckUser(DataSet ds)
        {
            DataSet dsReturn = new DataSet();
            if (ds!=null && ds.Tables.Contains(RBAC_USER_FIELDS.DATABASE_TABLE_NAME))
            {
                try
                {
                    DataTable dt = ds.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME];
                    string szUserName = dt.Rows[0][RBAC_USER_FIELDS.FIELD_USERNAME].ToString();
                    string sql = string.Format("SELECT * FROM RBAC_USER WHERE LOWER(BADGE)= LOWER('{0}') AND IS_ACTIVE=1",
                                                szUserName.PreventSQLInjection());
                    dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
                    dsReturn.Tables[0].TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("CheckUser Error: " + ex.Message);
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取用户信息。根据用户主键查询对应的用户信息。
        /// </summary>
        /// <param name="dataset">
        /// 数据集对象。数据集对象中必须包含名称为"<see cref="RBAC_USER_FIELDS.DATABASE_TABLE_NAME"/>"的数据表对象。
        /// 数据表中必须包含两个列"name"和"value"。列name存放的查询条件，列value存放查询条件对应的值。
        /// 目前可以使用的查询条件名称（即，列name存放的值）：
        /// <see cref="RBAC_USER_FIELDS.FIELD_USER_KEY"/>
        /// </param>
        /// <returns>包含用户信息和执行结果的数据集对象。</returns>
        public DataSet GetUserInfo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //define sql 
            string sql = "";
            string userKey = ""; 
            try
            {
                if (dsParams.Tables.Contains(RBAC_USER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dsParams.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);                                      
                    if (hashData.Contains(RBAC_USER_FIELDS.FIELD_USER_KEY))
                    {
                        userKey = hashData[RBAC_USER_FIELDS.FIELD_USER_KEY].ToString();
                    }
                    if (userKey != "")
                    {
                        sql = "SELECT * FROM RBAC_USER WHERE USER_KEY='" + userKey.PreventSQLInjection() + "'";
                    }
                    else
                    {
                        sql = "SELECT * FROM RBAC_USER";
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }                
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetUserInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dsParams">包含用户信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddUser(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                RBAC_USER_FIELDS userFields = new RBAC_USER_FIELDS();
                if (dsParams.Tables.Contains(RBAC_USER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dsParams.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string userName = hashData[RBAC_USER_FIELDS.FIELD_BADGE].ToString();
                    sql = "SELECT * FROM RBAC_USER WHERE BADGE='" + userName.PreventSQLInjection() + "'";
                    IDataReader read = db.ExecuteReader(CommandType.Text, sql);
                    try
                    {
                        if (read.Read())
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.UserEngine.UserAlreadyExist}");
                            return dsReturn;
                        }
                    }
                    finally
                    {
                        read.Close();
                        read.Dispose();
                        read = null;
                    }

                    hashData.Add(RBAC_USER_FIELDS.FIELD_CREATE_TIME, null);
                    hashData.Add(RBAC_USER_FIELDS.FIELD_IS_ACTIVE,"1");
                    hashData.Add(RBAC_USER_FIELDS.FIELD_IS_APPROVED,"0");
                    hashData.Add(RBAC_USER_FIELDS.FIELD_IS_LOCKED_OUT,"0");
                    hashData.Add(RBAC_USER_FIELDS.FIELD_EDITOR, hashData[RBAC_USER_FIELDS.FIELD_CREATOR].ToString());
                    hashData.Add(RBAC_USER_FIELDS.FIELD_EDIT_TIME, null);
                    sql = DatabaseTable.BuildInsertSqlStatement(userFields,hashData,null);
                    db.ExecuteNonQuery(dbtran,CommandType.Text, sql);
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }               
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddUser Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                dbconn.Close();
                dbtran = null;
                dbconn = null;
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新用户。
        /// </summary>
        /// <param name="dsParams">包含用户信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateUser(DataSet dsParams)
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
                if (dsParams.Tables.Contains(RBAC_USER_FIELDS.DATABASE_TABLE_NAME))
                {
                    RBAC_USER_FIELDS userFields = new RBAC_USER_FIELDS();
                    DataTable dataTable = dsParams.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    hashData.Add(RBAC_USER_FIELDS.FIELD_EDIT_TIME, null);
                    WhereConditions wc = new WhereConditions(RBAC_USER_FIELDS.FIELD_USER_KEY, hashData[RBAC_USER_FIELDS.FIELD_USER_KEY].ToString());
                    hashData.Remove(RBAC_USER_FIELDS.FIELD_USER_KEY);
                    sql = DatabaseTable.BuildUpdateSqlStatement(userFields, hashData, wc);
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
                LogService.LogError("UpdateUser Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                dbconn.Close();
                dbtran = null;
                dbconn = null;
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteUser(string userKey)
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
                if (!string.IsNullOrEmpty(userKey))
                {
                    string uKey = userKey.PreventSQLInjection();
                    sql = "DELETE FROM RBAC_USER WHERE USER_KEY='" + uKey + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    sql = "DELETE FROM RBAC_USER_IN_ROLE WHERE USER_KEY='" + uKey + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete privilege
                    sql = @"DELETE FROM RBAC_PRIVILEGE WHERE PRIVILEGE_KEY IN (SELECT PRIVILEGE_KEY FROM RBAC_USER_OWN_PRIVILEGES WHERE USER_KEY='" + uKey + "')";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    //delete user own privilege
                    sql = "DELETE FROM RBAC_USER_OWN_PRIVILEGES WHERE USER_KEY='" + uKey + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }  
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteUser Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                dbconn.Close();
                dbtran = null;
                dbconn = null;
            }
            return dsReturn;
        }      
        /// <summary>
        /// 根据用户名或者工号查询用户数据。
        /// </summary>
        /// <param name="userName">用户名或者工号。</param>
        /// <returns>包含用户信息和执行结果的数据集对象。</returns>
        public DataSet SearchUser(string userName)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    string uName = userName.PreventSQLInjection();
                    sql = "SELECT * FROM RBAC_USER WHERE USERNAME LIKE '%" + uName + "%' or BADGE like '%" + uName + "%'";
                }
                else
                {
                    sql = "SELECT * FROM RBAC_USER";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据用户主键查询用户权限。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户权限和执行结果的数据集对象。</returns>
        public DataSet GetPrivilegeOfUser(string userKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            
            try
            {
                if (!string.IsNullOrEmpty(userKey))
                {
                    string uKey = userKey.PreventSQLInjection();
                    sql = @"SELECT DISTINCT E.RESOURCE_GROUP_CODE+C.RESOURCE_CODE+D.OPERATION_CODE PRIVILEGE_CODE
                          FROM RBAC_PRIVILEGE      A,
                               RBAC_RESOURCE       C,
                               RBAC_OPERATION      D,
                               RBAC_RESOURCE_GROUP E
                         WHERE A.RESOURCE_KEY = C.RESOURCE_KEY
                           AND A.OPERATION_KEY = D.OPERATION_KEY
                           AND C.RESOURCE_GROUP_KEY = E.RESOURCE_GROUP_KEY
                           AND A.PRIVILEGE_KEY IN
                                              (SELECT B.PRIVILEGE_KEY FROM RBAC_ROLE_OWN_PRIVILEGE B
                                                WHERE B.ROLE_KEY IN
                                                               (SELECT ROLE_KEY FROM RBAC_USER_IN_ROLE WHERE USER_KEY = '" + uKey + "') " +
                                                "UNION ALL SELECT A.PRIVILEGE_KEY FROM RBAC_USER_OWN_PRIVILEGES A WHERE A.USER_KEY = '" + uKey + "')";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrivilegeOfUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取用户拥有权限的工序名称。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 使用,分割的工序名称字符串。
        /// </returns>
        public DataSet GetOperationOfUser(string userKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (!string.IsNullOrEmpty(userKey))
                {
                    sql = string.Format(@"SELECT DISTINCT a.OPERATION_NAME,b.SORT_SEQ
                                        FROM RBAC_ROLE_OWN_OPERATION a
                                        LEFT JOIN POR_ROUTE_OPERATION_VER b ON a.OPERATION_NAME=b.ROUTE_OPERATION_NAME
                                        WHERE b.OPERATION_STATUS=1 AND ROLE_KEY IN (SELECT ROLE_KEY 
                                                                                    FROM RBAC_USER_IN_ROLE WHERE USER_KEY = '{0}')
                                        ORDER BY b.SORT_SEQ", userKey.PreventSQLInjection());
                    DataTable dtTable = new DataTable();
                    dtTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    dtTable.TableName = RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }                
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
                LogService.LogError("GetOperationOfUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取用户拥有权限的线上仓信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>
        /// 包含用户拥有权限的线上仓信息。
        /// </returns>
        public DataSet GetStoreOfUser(string userKey, string roomName)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (!string.IsNullOrEmpty(userKey))
                {
                    sql = @"SELECT DISTINCT a.STORE_NAME,c.ROOM_NAME,c.ROOM_KEY
                            FROM RBAC_ROLE_OWN_STORE a
                            INNER JOIN WST_STORE b ON a.STORE_NAME=b.STORE_NAME
                            INNER JOIN V_LOCATION c ON b.LOCATION_KEY=c.ROOM_KEY
                            WHERE a.ROLE_KEY IN (SELECT ROLE_KEY FROM RBAC_USER_IN_ROLE WHERE USER_KEY = '" + userKey.PreventSQLInjection() + "')";
                    if (!string.IsNullOrEmpty(roomName))
                    {
                        sql += " AND c.ROOM_NAME='" + roomName.PreventSQLInjection() + "'";
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = RBAC_ROLE_OWN_STORE_FIELDS.DATABASE_TABLE_NAME;
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStoreOfUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取用户拥有权限的线上仓信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 包含用户拥有权限的线上仓信息。
        /// </returns>
        public DataSet GetStoreOfUser(string userKey)
        {
            return GetStoreOfUser(userKey, null);
        }
        /// <summary>
        /// 获取用户拥有权限的线别信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>
        /// 包含用户拥有权限的线别信息。
        /// </returns>
        public DataSet GetLineOfUser(string userKey,string roomName)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (!string.IsNullOrEmpty(userKey))
                {
                    sql = @"SELECT DISTINCT a.LINE_NAME,d.ROOM_KEY,d.ROOM_NAME
                            FROM RBAC_ROLE_OWN_LINES a
                            INNER JOIN FMM_PRODUCTION_LINE b ON a.LINE_NAME=b.LINE_NAME
                            INNER JOIN FMM_LOCATION_LINE c ON b.PRODUCTION_LINE_KEY=c.LINE_KEY
                            INNER JOIN V_LOCATION d ON c.LOCATION_KEY=d.AREA_KEY
                            WHERE a.ROLE_KEY IN (SELECT ROLE_KEY FROM RBAC_USER_IN_ROLE WHERE USER_KEY = '" + userKey.PreventSQLInjection() + "')";
                    if (!string.IsNullOrEmpty(roomName))
                    {
                        sql += " AND d.ROOM_NAME='" + roomName.PreventSQLInjection() + "'";
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = RBAC_ROLE_OWN_LINES_FIELDS.DATABASE_TABLE_NAME;

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLineOfUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取用户拥有权限的线别信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 包含用户拥有权限的线别信息。
        /// </returns>
        public DataSet GetLineOfUser(string userKey)
        {
            return GetLineOfUser(userKey, null);
        }
        /// <summary>
        /// 修改用户密码。
        /// </summary>
        /// <param name="userTable">包含用户信息的数据表。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet ChangePassword(DataTable userTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                Hashtable hashTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(userTable);
                string badge=Convert.ToString(hashTable[RBAC_USER_FIELDS.FIELD_BADGE]).PreventSQLInjection();
                string password=Convert.ToString(hashTable[RBAC_USER_FIELDS.FIELD_PASSWORD]).PreventSQLInjection();
                string newPassword = Convert.ToString(hashTable[RBAC_USER_FIELDS.FIELD_PASSWORD_NEW]).PreventSQLInjection();
                string sql = string.Format(@"SELECT USERNAME FROM RBAC_USER where BADGE='{0}' and PASSWORD='{1}'", badge, password);
                object userName = db.ExecuteScalar(CommandType.Text, sql);
                if (userName != null && userName!=DBNull.Value)
                {
                    sql = @"UPDATE RBAC_USER SET PASSWORD='" + newPassword + "' WHERE BADGE='" + badge + "'";
                    db.ExecuteNonQuery(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "原始密码不正确");
                }
            }
            catch(Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 记录用户登入信息。
        /// </summary>
        /// <param name="dsParams">包含用户登录信息的数据集对象。</param>
        /// <returns>true：记录成功。false：记录失败。</returns>
        public bool LogUserLoginInfo(DataSet dsParams)
        {
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(RBAC_LOGIN_LOG_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable loginLogDataTable = dsParams.Tables[RBAC_LOGIN_LOG_FIELDS.DATABASE_TABLE_NAME];
                    loginLogDataTable.Rows[0][RBAC_LOGIN_LOG_FIELDS.FIELD_SERVER_IP] = System.Environment.MachineName;
                    RBAC_LOGIN_LOG_FIELDS loginLogFields = new RBAC_LOGIN_LOG_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(loginLogFields, loginLogDataTable);

                    if (sqlStringList.Count > 0)
                    {
                        if (db.ExecuteNonQuery(CommandType.Text, sqlStringList[0]) > 0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                LogService.LogError("LogUserLoginInfo Error: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 记录用户登出信息。
        /// </summary>
        /// <param name="loginLogKey">用户登录日志主键。</param>
        /// <returns>true：记录成功。false:记录失败。</returns>
        public bool LogUserLogoutInfo(string loginLogKey)
        {
            try
            {
                RBAC_LOGIN_LOG_FIELDS loginLogFields = new RBAC_LOGIN_LOG_FIELDS();

                Hashtable updatedFields = new Hashtable();

                updatedFields.Add(RBAC_LOGIN_LOG_FIELDS.FIELD_LOGOUT_TIME, string.Empty);

                Conditions conditions = new Conditions();

                Condition condition = new Condition(DatabaseLogicOperator.And, RBAC_LOGIN_LOG_FIELDS.FIELD_LOGIN_LOG_KEY, DatabaseCompareOperator.Equal,
                    loginLogKey);

                conditions.Add(condition);

                string sqlString = DatabaseTable.BuildUpdateSqlStatement(loginLogFields, updatedFields, conditions);

                if (!string.IsNullOrEmpty(sqlString))
                {
                    if (db.ExecuteNonQuery(CommandType.Text, sqlString) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LogService.LogError("LogUserLogoutInfo Error: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 添加操作员。
        /// </summary>
        /// <param name="dtParams">包含操作员信息的数据表对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet InsertOperator(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            if (dtParams != null && dtParams.Rows.Count > 0)
            {
                try
                {
                    string badge = dtParams.Rows[0][RBAC_OPERATOR_FIELDS.BADGE].ToString();

                    string sql = @"select BADGE from RBAC_OPERATOR where BADGE='" + badge.PreventSQLInjection() + "' and STATUS=0";
                    object o = db.ExecuteScalar(CommandType.Text, sql);
                    if (o != null && o != DBNull.Value)
                    {
                        dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "员工号已经存在");
                    }
                    else
                    {
                        sql = DatabaseTable.BuildInsertSqlStatement(new RBAC_OPERATOR_FIELDS(),
                                                                        dtParams,
                                                                        0,
                                                                        new Dictionary<string, string>() 
                                                                        {                                                             
                                                                            {RBAC_OPERATOR_FIELDS.CREATE_TIME, null},                                                            
                                                                        },
                                                                        new List<string>());
                        db.ExecuteNonQuery(CommandType.Text, sql);
                        dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    }

                }
                catch (Exception ex)
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("InsertOperator Error:" + ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "输入数据为空。");
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除操作员。
        /// </summary>
        /// <param name="dtParams">包含操作员信息的数据表对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteOperator(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            if (dtParams != null && dtParams.Rows.Count > 0)
            {
                DbConnection dbconn = null;
                DbTransaction dbtran = null;
                try
                {
                    dbconn = db.CreateConnection();
                    dbconn.Open();
                    //Create Transaction  
                    dbtran = dbconn.BeginTransaction();

                    List<string> sqlList = new List<string>();
                    foreach (DataRow dataRow in dtParams.Rows)
                    {
                        //string sql = "delete from RBAC_OPERATOR where COL_KEY='" + dataRow[RBAC_OPERATOR_FIELDS.COL_KEY].ToString() + "'";
                        string colKey = Convert.ToString(dataRow[RBAC_OPERATOR_FIELDS.COL_KEY]);
                        string sql = @"UPDATE RBAC_OPERATOR SET STATUS=1 WHERE COL_KEY='" + colKey.PreventSQLInjection() + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    }
                    dbtran.Commit();
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("DeleteOperator Error:" + ex.Message);
                }
                finally
                {
                    dbconn.Close();
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "输入数据为空。");
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询操作人员。
        /// </summary>
        /// <param name="badge">工号或用户名。模糊匹配查询。</param>
        /// <returns>包含操作人员信息的数据集对象。
        /// 该数据集对象附加扩展属性,扩展属性键值为<see cref="PARAMETERS.OUTPUT_MESSAGE"/>,可以通过扩展属性获取额外的输出信息。
        /// <code>
        /// dsReturn.ExtendedProperties[PARAMETERS.OUTPUT_MESSAGE]
        /// </code>
        /// </returns>
        public DataSet SearchOperator(string badge)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT * FROM RBAC_OPERATOR WHERE STATUS=0";
                if (!string.IsNullOrEmpty(badge))
                {
                    sql += " AND BADGE LIKE '%" + badge + "%' OR USER_NAME LIKE '%" + badge.PreventSQLInjection() + "%'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
            }
            catch (Exception ex)
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                LogService.LogError("Search Operator Error:" + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 验证操作员是否存在
        /// </summary>
        /// <param name="badge">操作员工号。</param>
        /// <returns>包含操作员信息的数据集对象。
        /// 该数据集对象附加扩展属性,扩展属性键值为<see cref="PARAMETERS.OUTPUT_MESSAGE"/>,可以通过扩展属性获取额外的输出信息。
        /// <code>
        /// dsReturn.ExtendedProperties[PARAMETERS.OUTPUT_MESSAGE]
        /// </code>
        /// </returns>
        public DataSet CheckOperator(string badge)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (badge != string.Empty)
                {
                    string sql = @"select BADGE from RBAC_OPERATOR where BADGE='" + badge.PreventSQLInjection() + "'";
                    object badgeObj = db.ExecuteScalar(CommandType.Text, sql);
                    if (badgeObj != null && badgeObj != DBNull.Value)
                    {
                        dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    }
                    else
                    {
                        dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "员工号不存在！");
                    }
                    
                }
                else
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "员工号为空。");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("Check Operator Error:" + ex.Message);
            }
            return dsReturn;
        }

        public DataSet CheckLastPackageOperator(string badge)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                string sql = string.Format(@"select t.USER_KEY 
                                            from RBAC_USER t 
                                            left join RBAC_USER_IN_ROLE t1 on t.USER_KEY=t1.USER_KEY
                                            left join RBAC_ROLE t2 on t1.ROLE_KEY=t2.ROLE_KEY
                                            where (t.BADGE='{0}' or t.USER_KEY='{0}') and t2.ROLE_NAME like '%尾单%'", 
                                            badge.PreventSQLInjection());

                DataTable dtUser = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtUser.TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtUser, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

            }
            catch (Exception ex)
            {
                LogService.LogError("Check Operator Error:" + ex.Message);
            }
            return dsReturn;
        }
    }
}
