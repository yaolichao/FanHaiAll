using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using FanHai.Hemera.Utils.DatabaseHelper;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 生产线别数据管理类。
    /// </summary>
    public class LineManageEngine:AbstractEngine,ILineManageEngine
    {
        private Database db = null;
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LineManageEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 新增线别。
        /// </summary>
        /// <param name="dsParams">包含线别数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddLine(DataSet dsParams)
        {      
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME))
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();

                DataTable dtParams = dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];
                FMM_PRODUCTION_LINE_FIELDS lineFields = new FMM_PRODUCTION_LINE_FIELDS();
                //查找编码是否已经存在
                string lineCode=Convert.ToString(dtParams.Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE]);
                sqlCommand = "SELECT COUNT(*) FROM FMM_PRODUCTION_LINE WHERE LINE_CODE ='" + lineCode.PreventSQLInjection() + "'";
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                //存在就返回
                if (count>0)
                {
                    //编码已经存在，请重新命名
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.FMM.LineManageEngine.CodeAlreadyExist}");
                    return dsReturn;
                }
                try
                {
                    //循环次数为数据的行数
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        sqlCommand = DatabaseTable.BuildInsertSqlStatement(lineFields, dtParams, i, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {FMM_PRODUCTION_LINE_FIELDS.FIELD_CREATE_TIME,null},                                                            
                                                    },
                                                    new List<string>());

                        db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                    }
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("AddLine Error: " + ex.Message);
                }
                finally
                {
                    dbtran = null;
                    dbconn.Close();
                    dbconn = null;
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线别主键获取线别数据。
        /// </summary>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>包含线别数据的数据集对象。</returns>
        public DataSet GetLine(string lineKey)
        {            
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {               
                if (string.IsNullOrEmpty(lineKey))
                {  
                    sql = "SELECT * FROM FMM_PRODUCTION_LINE";                                
                }
                else
                {
                    sql = "SELECT * FROM FMM_PRODUCTION_LINE WHERE PRODUCTION_LINE_KEY='" + lineKey.PreventSQLInjection() + "'";                    
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLine Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除线别。
        /// </summary>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteLine(string lineKey)
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
                if (!string.IsNullOrEmpty(lineKey))
                {
                    string sql = @"DELETE FROM FMM_PRODUCTION_LINE WHERE PRODUCTION_LINE_KEY='" + lineKey.PreventSQLInjection() + "'";    
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);                   
                }
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message.ToString());
                LogService.LogError("DeleteLine Error: " + ex.Message);
            }
            finally
            {
                dbtran = null;
                //Close Connection
                dbconn.Close();
                dbconn = null;
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新线别。
        /// </summary>
        /// <param name="dsParams">包含线别数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateLine(DataSet dsParams)
        {               
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string lineCode = "";
            if (null != dsParams && dsParams.Tables.Contains(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];                  
                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    if (dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE)
                    {
                        lineCode = Convert.ToString(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE]);
                    }
                }
                //如果有修改线别代码，判断新的线别代码是否存在。
                if (!string.IsNullOrEmpty(lineCode))
                {
                    string strSql = @"SELECT COUNT(*) FROM FMM_PRODUCTION_LINE WHERE LINE_CODE='" +lineCode.PreventSQLInjection()+ "'";
                    int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                    if (count>0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.FMM.LineManageEngine.CodeAlreadyExist}");
                        return dsReturn;
                    }
                }
                //生成更新SQL
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new FMM_PRODUCTION_LINE_FIELDS(),
                                                        dsParams.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {FMM_PRODUCTION_LINE_FIELDS.FIELD_EDIT_TIME,null},                                                            
                                                        },
                                                        new List<string>() 
                                                        {
                                                            FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY                                                           
                                                        },
                                                        FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY);
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
                        LogService.LogError("UpdateLine Error: " + ex.Message);
                    }
                    finally
                    {
                        dbtran = null;
                        //Close Connection
                        dbconn.Close();
                        dbconn = null;
                    }
                }
            }
            return dsReturn;            
        }
        /// <summary>
        /// 根据线别名称获取线别数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// 数据表结构。
        /// ----------------------
        ///| LINE_NAME  |   ROWNUM|
        /// ----------------------
        /// </param>
        /// <returns>包含线别数据的数据集对象。</returns>
        public DataSet GetHelpInfoForLineHelpForm(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string lineName=Convert.ToString(dsParams.Tables[0].Rows[0][1]);
                string rowNum=Convert.ToString(dsParams.Tables[0].Rows[1][1]);
                string sql = string.Format(@"SELECT TOP {1} PRODUCTION_LINE_KEY,LINE_CODE,LINE_NAME 
                                           FROM FMM_PRODUCTION_LINE 
                                           WHERE LINE_NAME LIKE '%{0}%' 
                                           ORDER BY PRODUCTION_LINE_KEY",
                                            lineName.PreventSQLInjection(),
                                            rowNum.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetHelpInfoForLineHelpForm Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线别名称获取线别信息。
        /// </summary>
        /// <param name="lines">使用“逗号(,)”分开的线别名称字符串。“X01,X02,C01...”</param>
        /// <returns>包含线别信息的数据集对象。</returns>
        public DataSet GetLinesInfo(string lines)
        {
            DataSet dsReturn = new DataSet();
            string sqlCondition = "";
            try
            {
                string sql = "";
                //线别名称长度>0,即方法参数传入了线别名称。
                if (lines.Length > 0)
                {
                    sql = @"SELECT PRODUCTION_LINE_KEY,LINE_NAME,LINE_CODE FROM FMM_PRODUCTION_LINE WHERE 1=1";
                    sqlCondition = UtilHelper.BuilderWhereConditionString("LINE_NAME", lines.Split(','));
                    sql += sqlCondition;
                }
                else//如果没有传入线别名称。
                {
                    sql = @"SELECT PRODUCTION_LINE_KEY,LINE_NAME,LINE_CODE FROM FMM_PRODUCTION_LINE WHERE LINE_NAME = ''";                    

                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLines Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线别名称和工厂车间主键获取线别信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间主键。</param>
        /// <param name="lines">可选，使用“逗号(,)”分开的线别名称字符串。“X01,X02,C01...”</param>
        /// <returns>
        /// 包含线别信息的数据集对象。
        /// [LINE_NAME,PRODUCTION_LINE_KEY,LINE_CODE,ROOM_NAME,ROOM_KEY]
        /// </returns>
        public DataSet GetLinesInfo(string factoryRoomKey, string lines)
        {
            DataSet dsReturn = new DataSet();
            string sqlCondition = "";
            const string CONST_SQL = @"SELECT distinct a.LINE_NAME,a.PRODUCTION_LINE_KEY,a.LINE_CODE,c.ROOM_NAME,c.ROOM_KEY
                                        FROM FMM_PRODUCTION_LINE a
                                        LEFT JOIN FMM_LOCATION_LINE b ON a.PRODUCTION_LINE_KEY=b.LINE_KEY
                                        LEFT JOIN V_LOCATION c ON b.LOCATION_KEY=c.AREA_KEY
                                        WHERE c.ROOM_KEY='{0}'";
            try
            {
                string sql = string.Format(CONST_SQL, factoryRoomKey.PreventSQLInjection());
                //线别名称长度>0,即方法参数传入了线别名称。
                if (!string.IsNullOrEmpty(lines))
                {
                    sqlCondition = UtilHelper.BuilderWhereConditionString("LINE_NAME", lines.Split(','));
                    sql += sqlCondition;
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLinesInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        ///根据工厂车间获取线别名称
        /// </summary>
        /// <param name="factoryroom">工厂车间名称。</param>
        /// <returns>包含线别数据的数据集对象。</returns>
        /// comment by jing.xie 2012-3-30
        public DataSet GetLinesByFactoryRoom(string factoryroom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT C.PRODUCTION_LINE_KEY,C.LINE_NAME 
                            FROM V_LOCATION A ,FMM_LOCATION_LINE  B , FMM_PRODUCTION_LINE C 
                            WHERE A.AREA_KEY = B.LOCATION_KEY AND  B.LINE_KEY = C.PRODUCTION_LINE_KEY
                            AND A.ROOM_NAME ='" + factoryroom.PreventSQLInjection() + "'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLines Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 根据用户主键获取用户拥有的线别数据（始终包含一条线别名称为空的记录。）。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户拥有的线别数据的数据集对象。</returns>
        public DataSet GetLinesInfoContainEmpty(string userKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT '' AS LINE_NAME, '0' AS PRODUCTION_LINE_KEY
                                             UNION 
                                             SELECT A.LINE_NAME, B.PRODUCTION_LINE_KEY
                                             FROM RBAC_ROLE_OWN_LINES A
                                             LEFT JOIN FMM_PRODUCTION_LINE B ON A.LINE_NAME = B.LINE_NAME
                                             WHERE ROLE_KEY IN (SELECT ROLE_KEY
                                                                FROM RBAC_USER_IN_ROLE
                                                                WHERE USER_KEY = '{0}')
                                             ORDER BY PRODUCTION_LINE_KEY ", userKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLinesInfoContainEmpty Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
