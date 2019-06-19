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

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 排班计划班次数据管理类。
    /// </summary>
    public class ShiftEngine: AbstractEngine,IShift
    {
        private Database db;//数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ShiftEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据自定义属性类别名获取自定义属性值数据。
        /// </summary>
        /// <param name="tableName">自定义属性类别名。</param>
        /// <returns>包含自定义属性值数据的数据集对象。</returns>
        public DataSet GetLookUpEditData(string tableName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT CRM_ATTRIBUTE.ATTRIBUTE_VALUE AS CODE
                                            FROM CRM_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY,BASE_ATTRIBUTE
                                            WHERE (BASE_ATTRIBUTE.ATTRIBUTE_KEY=CRM_ATTRIBUTE.ATTRIBUTE_KEY 
                                            AND BASE_ATTRIBUTE.CATEGORY_KEY=BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY)
                                            AND BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME = '{0}'
                                            ORDER BY CRM_ATTRIBUTE.ATTRIBUTE_VALUE",
                                            tableName.PreventSQLInjection());
                //excute sql
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLookUpEditData Error: "+ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增排班计划班次数据。
        /// </summary>
        /// <param name="dsParams">包含排班计划班次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddShift(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(CAL_SHIFT.DATABASE_TABLE_NAME))
            {
                try
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    //Create Transaction  
                    dbTran = dbConn.BeginTransaction();
                    DataTable dtParams = dsParams.Tables[CAL_SHIFT.DATABASE_TABLE_NAME];
                    //创建cal_shift对应的对象
                    CAL_SHIFT shiftFields = new CAL_SHIFT();
                    string shiftKey = dtParams.Rows[0][CAL_SHIFT.FIELD_SHIFT_KEY].ToString();
                    string scheduleKey = dtParams.Rows[0][CAL_SCHEDULE.FIELD_SCHEDULE_KEY].ToString();                       
                    sqlCommand = DatabaseTable.BuildInsertSqlStatement(shiftFields, dtParams, 0, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {CAL_SHIFT.FIELD_CREATE_TIME, null}, 
                                                        {CAL_SHIFT.FIELD_EDIT_TIME,null}
                                                    },
                                                    new List<string>() { CAL_SCHEDULE.FIELD_SCHEDULE_KEY }
                                                    );
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                   
                    sqlCommand =string.Format(@"INSERT INTO CAL_SCHEDULE_SHIFT(SCHEDULE_KEY,SHIFT_KEY) 
                                                VALUES('{0}','{1}')",
                                                scheduleKey.PreventSQLInjection(),
                                                shiftKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    dbTran.Commit();  
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("AddShift Error: " + ex.Message);
                }
                finally
                {
                    dbTran.Dispose();
                    dbConn.Close();
                    dbConn.Dispose();
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.CalShiftEngine.TableIsNotExist}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新排班计划班次数据。
        /// </summary>
        /// <param name="dsParams">包含排班计划班次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateShift(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;   
            if (dsParams.Tables.Contains(CAL_SHIFT.DATABASE_TABLE_NAME))
            {              
                #region 检查记录是否过期。
                string oldEditTime = "",shiftKey="";
                for (int i = 0; i < dsParams.Tables[0].Rows.Count; i++)
                {
                    if (dsParams.Tables[0].Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == COMMON_FIELDS.FIELD_COMMON_EDIT_TIME)
                    {
                        oldEditTime = dsParams.Tables[0].Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE].ToString();
                        shiftKey = dsParams.Tables[0].Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY].ToString();
                        break;
                    }
                }
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(CAL_SHIFT.FIELD_SHIFT_KEY,shiftKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                if (UtilHelper.CheckRecordExpired(db,CAL_SHIFT.DATABASE_TABLE_NAME, listCondition, oldEditTime))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                    return dsReturn;
                }
                #endregion

                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new CAL_SHIFT(),
                                                        dsParams.Tables[CAL_SHIFT.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                        {CAL_SHIFT.FIELD_EDIT_TIME,null},                                                            
                                                        },
                                                        new List<string>() 
                                                        {
                                                         CAL_SHIFT.FIELD_SHIFT_KEY                                                          
                                                        },
                                                        CAL_SHIFT.FIELD_SHIFT_KEY);
                if (sqlCommandList.Count > 0)
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    //Create Transaction  
                    dbTran = dbConn.BeginTransaction();
                    try
                    {                        
                        foreach (string sql in sqlCommandList)
                        {
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        dbTran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
                        LogService.LogError("UpdateShift Error: " + ex.Message);
                    }
                    finally
                    {
                        dbTran.Dispose();
                        dbConn.Close();
                        dbConn.Dispose();
                    }
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.CalShiftEngine.TableIsNotExist}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除排班计划班次数据。
        /// </summary>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <param name="shiftKey">班次主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteShift(string scheduleKey, string shiftKey)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                //Create Transaction  
                dbTran = dbConn.BeginTransaction();
                DeleteShift(db,dbTran,scheduleKey,shiftKey);
                dbTran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                dbTran.Rollback();
                LogService.LogError("DeleteShift Error: " + ex.Message);
            }
            finally
            {
                dbTran.Dispose();
                dbConn.Close();
                dbConn.Dispose();
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除班次数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库操作事务对象。</param>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <param name="shiftKey">班次主键。</param>
        internal static void DeleteShift(Database db,DbTransaction dbtran,string scheduleKey,string shiftKey)
        {
            string sql = string.Format(@"DELETE FROM CAL_SHIFT WHERE SHIFT_KEY='{0}'",
                                        shiftKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbtran,CommandType.Text,sql);
            sql =string.Format(@"DELETE FROM CAL_SCHEDULE_SHIFT WHERE SCHEDULE_KEY='{0}' AND SHIFT_KEY='{1}'",
                                scheduleKey.PreventSQLInjection(),
                                shiftKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbtran,CommandType.Text,sql);
        }
        /// <summary>
        /// 根据班次主键获取班次数据。
        /// </summary>
        /// <param name="shiftKey">班次主键。</param>
        /// <returns>包含班次数据的数据集对象。</returns>
        public DataSet GetShift(string shiftKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql =string.Format(@"SELECT A.* FROM CAL_SHIFT A WHERE A.SHIFT_KEY='{0}'",shiftKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                DataTable dataTable = dsReturn.Tables[0];
                dataTable.TableName = CAL_SHIFT.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShift Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 判别班别是否存在。返回班别对应的主键标识符。
        /// </summary>
        /// <param name="shiftValue">班别值。</param>
        /// <returns>返回班别主键的标识字符串。空字符串代表班别不存在</returns>
        public string IsExistsShift(string shiftValue)
        {
            string shiftKey = string.Empty;
            try
            {
                string sql = string.Format(@"SELECT T.DKEY
                                            FROM CAL_SCHEDULE_DAY T, V_SHIFT_OVERTIME T1
                                            WHERE T.SHIFT_KEY = T1.SHIFT_KEY
                                            AND T.SHIFT_VALUE = '{0}'
                                            AND SYSDATETIME() BETWEEN T.STARTTIME AND DATEADD(mi, T1.OVERTIME,T.ENDTIME)",
                                            shiftValue.PreventSQLInjection());
                object o = db.ExecuteScalar(CommandType.Text, sql);
                if (o != null && o != DBNull.Value)
                {
                    shiftKey = o.ToString();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("IsExistsShift Error: " + ex.Message);
            }
            return shiftKey;
        }

        /// <summary>
        /// 根据当前日期获取班别名称。
        /// </summary>
        /// <returns>班别名称。</returns>
        public string GetShiftNameBySysdate()
        {
            string shiftName = string.Empty;
            try
            {
                string sql = @"SELECT TT.SHIFT_VALUE
                            FROM 
                            (
                                SELECT T.SHIFT_VALUE, ROW_NUMBER() OVER(ORDER BY T.STARTTIME DESC) SEQNUM
                                FROM CAL_SCHEDULE_DAY T, V_SHIFT_OVERTIME T1
                                WHERE  SYSDATETIME() BETWEEN T.STARTTIME AND DATEADD(mi, T1.OVERTIME,T.ENDTIME) 
                                AND T.SHIFT_KEY = T1.SHIFT_KEY
                            ) TT
                            WHERE TT.SEQNUM = 1";
                object o = db.ExecuteScalar(CommandType.Text, sql);
                if (o != null && o != DBNull.Value)
                {
                    shiftName = o.ToString();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetShiftNameBySysdate Error: " + ex.Message);
            }
            return shiftName;
        }
    }
    
}
