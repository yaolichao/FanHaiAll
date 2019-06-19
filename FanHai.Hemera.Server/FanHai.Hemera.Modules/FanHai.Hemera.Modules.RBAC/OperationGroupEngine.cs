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
    /// 系统操作组数据管理类。
    /// </summary>
    public class OperationGroupEngine : AbstractEngine, IOperationGroupEngine
    {
        private Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationGroupEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 添加系统操作组。
        /// </summary>
        /// <param name="dsParams">包含系统操作组的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddOperationGroup(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();  
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            if (null != dsParams && dsParams.Tables.Contains(RBAC_OPERATION_GROUP_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[RBAC_OPERATION_GROUP_FIELDS.DATABASE_TABLE_NAME];
                string groupName = Convert.ToString(dtParams.Rows[0][RBAC_OPERATION_GROUP_FIELDS.FIELD_GROUP_NAME]);
                string sqlCommand = "SELECT COUNT(*) FROM RBAC_OPERATION_GROUP WHERE GROUP_NAME ='" + groupName.PreventSQLInjection() + "'";
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (count>0)//系统资源组是否存在。
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.RBAC.OperationGroupEngine.OperationNameAlreadyExist}");
                }
                else
                {
                    //生成INSERT SQL
                    List<string> sqlCommandList = new List<string>();
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                            new RBAC_OPERATION_GROUP_FIELDS(),
                                                            dsParams.Tables[RBAC_OPERATION_GROUP_FIELDS.DATABASE_TABLE_NAME],
                                                            new Dictionary<string, string>() 
                                                            {                                                             
                                                            {RBAC_OPERATION_GROUP_FIELDS.FIELD_CREATE_TIME, null},                                                            
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
                            LogService.LogError("AddOperationGroup Error: " + ex.Message);
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
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;    
        }

        /// <summary>
        /// 删除系统操作组。
        /// </summary>
        /// <param name="resourceGroupKey">系统操作组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteOperationGroup(string operationGroupKey)
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
                if (operationGroupKey != "")
                {
                    sql = "DELETE FROM RBAC_OPERATION_GROUP WHERE OPERATION_GROUP_KEY='" + operationGroupKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    sql = "DELETE FROM RBAC_OPERATION WHERE OPERATION_GROUP_KEY='" + operationGroupKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteOperationGroup Error: " + ex.Message);
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
        /// 获取系统操作组。
        /// </summary>
        /// <returns>包含系统操作组的数据集对象。</returns>
        public DataSet GetOperationGroup()
        {
            DataSet dsReturn = new DataSet();
            //define sql 
            string sql = "";
            try
            {
                sql = "SELECT OPERATION_GROUP_KEY,GROUP_NAME FROM RBAC_OPERATION_GROUP ORDER BY GROUP_NAME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOperationGroup Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
