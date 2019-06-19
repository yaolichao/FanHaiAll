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
    /// 用户角色数据管理类。
    /// </summary>
    /// <see cref="IUserRoleEngine"/>
    /// <see cref="AbstractEngine"/>
    public class UserRoleEngine : AbstractEngine, IUserRoleEngine
    {
        private Database db=null;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UserRoleEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据用户主键获取用户所属角色信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户所属角色信息的数据集对象。</returns>
        public DataSet GetRolesOfUser(string userKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT A.ROLE_NAME,A.ROLE_KEY 
                       FROM RBAC_ROLE A,RBAC_USER_IN_ROLE B 
                       WHERE A.ROLE_KEY=B.ROLE_KEY AND B.USER_KEY='"+userKey.PreventSQLInjection()+"'";
                dsReturn=db.ExecuteDataSet(CommandType.Text,sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRolesOfUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据角色主键获取角色中的用户信息。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含用户信息的数据集对象。</returns>
        public DataSet GetUsersOfRole(string roleKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT A.USER_KEY,A.USERNAME 
                        FROM RBAC_USER A,RBAC_USER_IN_ROLE B 
                        WHERE A.USER_KEY=B.USER_KEY AND B.ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetUsersOfRole Error: "+ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据用户主键获取用户不属于的角色信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户不属于的角色信息的数据集对象。</returns>
        public DataSet GetRolesNotBelongToUser(string userKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql =string.Format(@"SELECT a.ROLE_KEY, a.ROLE_NAME
                                    FROM RBAC_ROLE a
                                    WHERE ROLE_KEY NOT IN (SELECT ROLE_KEY FROM RBAC_USER_IN_ROLE WHERE USER_KEY='{0}') 
                                    ORDER BY a.ROLE_NAME", userKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRolesNotBelongToUser Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存用户角色信息。
        /// </summary>
        /// <param name="dsParams">包含用户角色数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveUserRole(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(RBAC_USER_IN_ROLE_FIELDS.DATABASE_TABLE_NAME))
            {
                try
                {
                    dbconn = db.CreateConnection();
                    dbconn.Open();
                    //Create Transaction  
                    dbtran = dbconn.BeginTransaction();

                    DataTable dtParams = dsParams.Tables[RBAC_USER_IN_ROLE_FIELDS.DATABASE_TABLE_NAME];
                    RBAC_USER_IN_ROLE_FIELDS userInRoleFields = new RBAC_USER_IN_ROLE_FIELDS();
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string userKey=Convert.ToString(dtParams.Rows[i][RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY]);
                        OperationAction action=(OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                        WhereConditions delCondition = new WhereConditions(RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY, userKey);
                        switch (action)
                        {
                            case OperationAction.New:
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(
                                                        userInRoleFields,
                                                        dtParams, 
                                                        i, 
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIME, null}
                                                        },
                                                        new List<string>()
                                                        {
                                                            COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION
                                                        });

                                db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                                break;
                            case OperationAction.Delete:
                                string roleKey = Convert.ToString(dtParams.Rows[i][RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY]);
                                sqlCommand = DatabaseTable.BuildDeleteSqlStatement(userInRoleFields, delCondition);
                                sqlCommand += " AND ROLE_KEY='" + roleKey.PreventSQLInjection() + "'";
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
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbtran.Rollback();
                    LogService.LogError("SaveUserRole Error: " + ex.Message);
                }
                finally
                {
                    if (dbtran != null)
                    {
                        dbtran = null;
                    }
                    if (dbconn != null)
                    {
                        //Close Connection
                        dbconn.Close();
                        dbconn = null;
                    }
                }
                
            }           
            return dsReturn;            
        }
    }
}
