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
    /// 系统操作数据管理类。
    /// </summary>
    public class RBACOperationEngine : AbstractEngine, IRBACOperationEngine
    {
        private Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RBACOperationEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 添加系统操作。
        /// </summary>
        /// <param name="dsParams">包含系统操作数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddOperation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            if (null != dsParams &&　dsParams.Tables.Contains(RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME))
            {
  
                DataTable dtParams = dsParams.Tables[RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME];
                //判断系统操作名称是否存在。
                string operationName = Convert.ToString(dtParams.Rows[0][RBAC_OPERATION_FIELDS.FIELD_OPERATION_NAME]);
                string groupKey = Convert.ToString(dtParams.Rows[0][RBAC_OPERATION_FIELDS.FIELD_OPERATION_GROUP_KEY]);
                string sqlCommand = @"SELECT COUNT(*) FROM RBAC_OPERATION WHERE 
                                      OPERATION_NAME ='" + operationName.PreventSQLInjection()+ "' AND " +
                                     "OPERATION_GROUP_KEY='" + groupKey.PreventSQLInjection() + "'";
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.OperationEngine.OperationNameAlreadyExist}");
                    return dsReturn;
                }
                //判断系统操作代码是否存在。
                string operationCode = Convert.ToString(dtParams.Rows[0][RBAC_OPERATION_FIELDS.FIELD_OPERATION_CODE]);
                string strSql = @"SELECT COUNT(*) FROM RBAC_OPERATION WHERE OPERATION_CODE='" + operationCode.PreventSQLInjection() + "'";
                count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (count > 0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.OperationEngine.OperationCodeAlreadyExist}");
                    return dsReturn;
                }
                //生成INSERT SQL
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                        new RBAC_OPERATION_FIELDS(),
                                                        dsParams.Tables[RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {RBAC_OPERATION_FIELDS.FIELD_CREATE_TIME,null},                                                            
                                                        },
                                                        new List<string>());
                //插入数据
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
                        LogService.LogError("AddOperation Error: " + ex.Message);
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
        /// 更新系统操作。
        /// </summary>
        /// <param name="dsParams">包含系统操作数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateOperation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string operationCode="";
            if (null != dsParams &&　dsParams.Tables.Contains(RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME];               
                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    if (dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == RBAC_OPERATION_FIELDS.FIELD_OPERATION_CODE)
                    {
                        operationCode = dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE].ToString();
                    }
                }
                //如果系统代码有更新，则判断系统代码是否存在。
                if (operationCode != "")
                {
                    string strSql = @"SELECT COUNT(*) FROM RBAC_OPERATION WHERE OPERATION_CODE='" + operationCode.PreventSQLInjection() + "'";
                    int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                    if (count>0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.OperationEngine.OperationCodeAlreadyExist}");
                        return dsReturn;
                    }
                }
                //生成更新SQL 
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new RBAC_OPERATION_FIELDS(),
                                                        dsParams.Tables[RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                        {RBAC_OPERATION_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                        },
                                                        new List<string>() 
                                                       {
                                                         RBAC_OPERATION_FIELDS.FIELD_OPERATION_KEY                                                           
                                                       },
                                                        RBAC_OPERATION_FIELDS.FIELD_OPERATION_KEY);
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
                        LogService.LogError("UpdateOperation Error: " + ex.Message);
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
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0007}");
            }
            return dsReturn;            
        }

        /// <summary>
        /// 删除系统操作。
        /// </summary>
        /// <param name="operationKey">系统操作主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteOperation(string operationKey)
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
                sql = "DELETE FROM RBAC_OPERATION WHERE OPERATION_KEY='" + operationKey.PreventSQLInjection() + "'";
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message.ToString());
                LogService.LogError("DeleteOperation Error: " + ex.Message);
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
        /// 获取系统操作数据。
        /// </summary>
        /// <param name="operationKey">系统操作主键。</param>
        /// <returns>包含系统操作数据的数据集对象。</returns>
        public DataSet GetOperation(string operationGroupKey)
        {
            DataSet dsReturn = new DataSet();
            //define sql 
            string sql = "";
            try
            {
                if (operationGroupKey != "")
                {
                    sql = "SELECT * FROM RBAC_OPERATION WHERE OPERATION_GROUP_KEY='" + operationGroupKey.PreventSQLInjection() + "'";
                }
                else
                {
                    sql = "SELECT * FROM RBAC_OPERATION";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOperation Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
