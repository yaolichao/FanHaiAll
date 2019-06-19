using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;

namespace FanHai.Hemera.Modules.FMM
{  
    /// <summary>
    /// 客户端及其配置的数据管理类。
    /// </summary>
    public class ComputerEngine : AbstractEngine, IComputerEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ComputerEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 新增客户端名称和相应的配置属性。
        /// </summary>
        /// <param name="dsParams">包含客户端名称和相应配置属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddComputer(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null != dsParams && dsParams.Tables.Contains(COMPUTER_FIELDS.DATABASE_TABLE_NAME))
            {
                List<string> sqlCommandList = new List<string>();
                //判断机器名是否存在。
                DataTable dtParams=dsParams.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME];
                string computerName = Convert.ToString(dtParams.Rows[0][COMPUTER_FIELDS.FIELDS_COMPUTER_NAME]);
                string sqlCommand = @"SELECT COUNT(*) FROM COMPUTER_CONFIG WHERE COMPUTER_NAME='"+computerName.ToUpper().PreventSQLInjection()+"'";
                int count =Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (count>0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"该机器名已经存在。");
                    return dsReturn;
                }
                //生成INSERT SQL
                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                    new COMPUTER_FIELDS(),
                                                    dsParams.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME],
                                                    new Dictionary<string, string>() 
                                                    { 
                                                        {COMPUTER_FIELDS.FIELDS_EDIT_TIME,null},
                                                        {COMPUTER_FIELDS.FIELDS_EDIT_TIMEZONE, "CN-ZH"},
                                                    },
                                                    new List<string>());
                if (1 == sqlCommandList.Count && sqlCommandList[0].Length > 20)
                {
                    //存在客户端配置属性数据。
                    if (dsParams.Tables.Contains(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                    {
                        //生成INSERT SQL
                        DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                               new COMPUTER_ATTR_FIELDS(),
                                                               dsParams.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME],
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
                            LogService.LogError("AddComputer Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTrans = null;
                            dbCon.Close();
                            dbCon = null;
                        }
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "More than one Computer in input parameter");
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "No Computer Tables in input paremter.");
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据客户端名称获取客户端及其配置数据。
        /// </summary>
        /// <param name="computerName">客户端名称。</param>
        /// <returns>包含客户端及其配置数据的数据集对象。</returns>
        public DataSet GetComputerByName(string computerName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (computerName != string.Empty)
                {
                    string cName = computerName.ToUpper().PreventSQLInjection();
                    //客户端数据。
                    string sql = string.Format(@"SELECT A.* FROM COMPUTER_CONFIG A WHERE COMPUTER_NAME='{0}'",cName);
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = COMPUTER_FIELDS.DATABASE_TABLE_NAME;
                    //获取客户端配置数据。
                    sql = string.Format(@"SELECT A.*, C.DATA_TYPE AS DATA_TYPE
                                        FROM COMPUTER_CONFIG_ATTR A
                                        INNER JOIN COMPUTER_CONFIG B ON A.COMPUTER_KEY = B.ROW_KEY
                                        LEFT JOIN BASE_ATTRIBUTE C ON A.ATTRIBUTE_KEY=C.ATTRIBUTE_KEY
                                        WHERE B.COMPUTER_NAME = '{0}'", cName);
                    DataTable udaDataTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    udaDataTable.TableName = COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    //ADD UDA TABLE TO DATASET
                    dsReturn.Merge(udaDataTable, true, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"Object key is null");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetComputerByName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询客户端数据。
        /// </summary>
        /// <param name="dsParams">包含查询参数的数据集对象。</param>
        /// <returns>包含客户端数据的数据集对象。</returns>
        public DataSet SearchComputers(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = "";

            try
            {
                sqlCommand = @"SELECT A.* FROM COMPUTER_CONFIG A WHERE 1=1";

                //判断是否存在指定表与表集是否为空
                if (dsParams != null && dsParams.Tables.Contains(COMPUTER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    //存在COMPUTER_NAME计算机名的值则进行模糊查询
                    if (htParams.Contains(COMPUTER_FIELDS.FIELDS_COMPUTER_NAME))
                    {
                        string computerName=Convert.ToString(htParams[COMPUTER_FIELDS.FIELDS_COMPUTER_NAME]);
                        sqlCommand += " AND COMPUTER_NAME LIKE '%" + computerName.PreventSQLInjection() + "%'";
                    }
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = COMPUTER_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchComputers Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新客户端及其配置数据。
        /// </summary>
        /// <param name="dsParams">包含客户端及其配置数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateComputer(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                List<string> sqlCommandList = new List<string>();
                //创建UPDATE SQL
                if (dsParams.Tables.Contains(COMPUTER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new COMPUTER_FIELDS(),
                           dsParams.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME],
                           new Dictionary<string, string>() { 
                                                                {COMPUTER_FIELDS.FIELDS_EDIT_TIME, null},
                                                                {COMPUTER_FIELDS.FIELDS_EDIT_TIMEZONE, "CN-ZH"}
                                                             },
                           new List<string>() { },
                           COMPUTER_FIELDS.FIELDS_CODE_KEY);
                }
                if (dsParams.Tables.Contains(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList,
                                                                   new COMPUTER_ATTR_FIELDS(),
                                                                   dsParams.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   COMPUTER_ATTR_FIELDS.FIELDS_COMPUTER_KEY);
                }
                //插入数据。
                if (sqlCommandList.Count > 0)
                {
                    //Database database = DatabaseFactory.CreateDatabase();
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
                        LogService.LogError("UpdateComputer Error: " + ex.Message);
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
                LogService.LogError("UpdateComputer Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除客户端及其配置数据。
        /// </summary>
        /// <param name="computerKey">客户端主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteComputer(string computerKey)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = "";
                  
            //删除客户端配置数据
            sqlCommand = "DELETE FROM " + COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + COMPUTER_ATTR_FIELDS.FIELDS_COMPUTER_KEY + " = '" + computerKey.PreventSQLInjection() + "'";
            sqlCommandList.Add(sqlCommand);

            //删除客户端数据
            sqlCommand = "DELETE FROM " + COMPUTER_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + COMPUTER_FIELDS.FIELDS_CODE_KEY + " = '" + computerKey.PreventSQLInjection() + "'";
            sqlCommandList.Add(sqlCommand);

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
                LogService.LogError("DeleteComputer Error: " + ex.Message);
            }
            finally
            {
                dbTrans = null;
                dbCon.Close();
                dbCon = null;
            }
            return dsReturn;
        }

    }
}

