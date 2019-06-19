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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 产品型号及产品设置操作类
    /// </summary>
    public class LineSettingEngine : AbstractEngine, ILineSettingEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LineSettingEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 通过用户获取对应有权限的线别信息
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="LineName">线别名称</param>
        /// <returns>用户拥有权限的线别表集</returns>
        public DataSet GetLineByUserNameAndLineName(string userName, string lineName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT DISTINCT D.PRODUCTION_LINE_KEY,D.LINE_NAME,D.LINE_CODE,D.DESCRIPTIONS
                                                    FROM RBAC_USER A
                                                    INNER JOIN RBAC_USER_IN_ROLE B ON B.USER_KEY = A.USER_KEY
                                                    INNER JOIN RBAC_ROLE_OWN_LINES C ON C.ROLE_KEY = B.ROLE_KEY
                                                    INNER JOIN FMM_PRODUCTION_LINE D ON D.LINE_NAME = C.LINE_NAME
                                                    WHERE USERNAME = '{0}'
                                                    ", userName);

                if (!string.IsNullOrEmpty(lineName))
                {
                    sqlCommand = sqlCommand + string.Format("AND D.LINE_NAME LIKE '%{0}%'", lineName);
                }

                 dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLineByUser Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 通过线别主键获取对应的子线信息
        /// </summary>
        /// <param name="lineKey">线别主键</param>
        /// <returns>线别主键对应的子线的数据集合</returns>
        public DataSet GetSubLineByLineKey(string mainLineKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT * 
                                                    FROM FMM_PRODUCTION_LINE_SUB
                                                    WHERE PRODUCTION_LINE_KEY = '{0}'
                                                    AND IS_USED = 'Y'", mainLineKey);


                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSubLineByLineKey Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 通过用户获取对应有权限的工序信息
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="LineName">工序名称</param>
        /// <returns>用户拥有权限的工序表集</returns>
        public DataSet GetOperationByUserNameAndOperationName(string userName, string operationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT C.OPERATION_NAME
                                                    FROM RBAC_USER A
                                                    INNER JOIN RBAC_USER_IN_ROLE B ON B.USER_KEY = A.USER_KEY
                                                    INNER JOIN RBAC_ROLE_OWN_OPERATION C ON C.ROLE_KEY = B.ROLE_KEY
                                                    WHERE USERNAME = '{0}'
                                                    ", userName);

                if (!string.IsNullOrEmpty(operationName))
                {
                    sqlCommand = sqlCommand + string.Format("AND C.OPERATION_NAME LIKE '%{0}%'", operationName);
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLineByUser Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 对线别对应的修改进行进行保存
        /// </summary>
        /// <param name="dsLine">线别操作相关的表集</param>
        /// <returns>操作返回的表集信息</returns>
        public DataSet SaveLineInfo(DataSet dsLine)
        {
            DataSet dsReturn = new DataSet();
            FMM_PRODUCTION_LINE_SUB_FIELDS line_Sub = new FMM_PRODUCTION_LINE_SUB_FIELDS();

            DataTable dtLine_Insert = null, dtLine_Update = null, dtLine_Delete = null;

            if (dsLine.Tables.Contains(FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORINSERT))
                dtLine_Insert = dsLine.Tables[FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORINSERT];
            if (dsLine.Tables.Contains(FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORUPDATE))
                dtLine_Update = dsLine.Tables[FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORUPDATE];
            if (dsLine.Tables.Contains(FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORDELETE))
                dtLine_Delete = dsLine.Tables[FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORDELETE];

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;

                try
                {
                    #region //功率分档信息保存
                    if (dtLine_Insert != null && dtLine_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtLine_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE SUB_LINE_KEY = '{1}'", line_Sub.TABLE_NAME,
                                                                                     dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_SUB_LINE_KEY]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_VERSION_NO] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }
                            else
                            {
                                dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_VERSION_NO] = 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(line_Sub, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtLine_Update != null && dtLine_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtLine_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE SUB_LINE_KEY = '{2}'
                                                         AND VERSION_NO = '{3}'", line_Sub.TABLE_NAME,
                                                                                  dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_EDITER],
                                                                                  dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_SUB_LINE_KEY],
                                                                                  dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_VERSION_NO]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE SUB_LINE_KEY = '{1}'", line_Sub.TABLE_NAME,
                                                                                     dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_SUB_LINE_KEY]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_VERSION_NO] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }
                            dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_CREATER] = dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_EDITER];
                            dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_EDIT_TIME] = DBNull.Value;
                            dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_CREATE_TIME] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(line_Sub, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtLine_Delete != null && dtLine_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtLine_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE SUB_LINE_KEY = '{2}'
                                                         AND VERSION_NO = '{3}'", line_Sub.TABLE_NAME,
                                                                                  dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_EDITER],
                                                                                  dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_SUB_LINE_KEY],
                                                                                  dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_VERSION_NO]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion



                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveLineInfo Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        /// <summary>
        /// 检查选定的线别是否为组件允许过站的线别
        /// </summary>
        /// <param name="lotLineKey">绑定的线别主键</param>
        /// <param name="curLineKey">当前的线别主键</param>
        /// <param name="curOperationName">当前工序名称</param>
        /// <returns>操作结果</returns>
        public DataSet CheckLotLine(string lotLineKey, string curLineKey, string curOperationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"
                                                    SELECT * FROM FMM_PRODUCTION_LINE_SUB
                                                    WHERE PRODUCTION_LINE_KEY = '{0}'
                                                    AND PRODUCTION_LINE_SUB_KEY = '{1}'
                                                    AND OPERATION_NAME = '{2}'", 
                                                                                 lotLineKey,
                                                                                 curLineKey,
                                                                                 curOperationName);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("CheckLotLine Error: " + ex.Message);
            }
            return dsReturn;
        }

    }
}

