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
    /// 排班计划数据的管理类。
    /// </summary>
    public class ScheduleEngine : AbstractEngine, ISchedule
    {
        private Database db;   //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ScheduleEngine()
        {
            //ceate db
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 添加排班计划记录。
        /// </summary>
        /// <param name="dataSet">包含排班计划数据表的数据集对象。
        /// 排班计划数据表的表名必须为<see cref="CAL_SCHEDULE.DATABASE_TABLE_NAME"/>。
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddSchedule(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sqlCommand = "";
            string scheduleName = "";
            if (null != dsParams && dsParams.Tables.Contains(CAL_SCHEDULE.DATABASE_TABLE_NAME))
            {
                try
                {
                    dbConn = db.CreateConnection();                             //数据库连接对象。
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();                         //Create Transaction  
                    DataTable dataTable = dsParams.Tables[CAL_SCHEDULE.DATABASE_TABLE_NAME];
                    CAL_SCHEDULE scheduleFields = new CAL_SCHEDULE();
                    //根据排班计划名称查询排班计划是否存在。
                    scheduleName = dataTable.Rows[0][CAL_SCHEDULE.FIELD_SCHEDULE_NAME].ToString();
                    sqlCommand =string.Format(@"SELECT SCHEDULE_NAME FROM CAL_SCHEDULE WHERE SCHEDULE_NAME='{0}'",
                                                scheduleName.PreventSQLInjection());
                    using(IDataReader dataReader = db.ExecuteReader(CommandType.Text, sqlCommand))
                    {
                        if (dataReader.Read())//如果排班计划存在。
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Addins.Msg.NameIsExist}");
                            return dsReturn;
                        }
                    }
                    //插入排班计划记录的SQL命令对象。
                    sqlCommand = DatabaseTable.BuildInsertSqlStatement(scheduleFields, dataTable, 0,
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {CAL_SCHEDULE.FIELD_CREATE_TIME, null},                                                            
                                                    },
                                                    new List<string>()
                                                    );
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("AddSchedule Error: " + ex.Message);
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
        /// 查询排班计划信息。
        /// </summary>
        /// <param name="scheduleName">排班计划名称。</param>
        /// <returns>包含排班计划信息的数据集对象。</returns>
        public DataSet SearchSchedule(string scheduleName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT SCHEDULE_KEY,SCHEDULE_NAME,DESCRIPTIONS,MAXOVERLAPTIME,CYCLEREFERENCEDATE FROM CAL_SCHEDULE";
                //判断传入参入参数值(即班次名称)不为空
                if (scheduleName != string.Empty)
                {
                    //模糊查询
                    sql = sql + string.Format(" WHERE SCHEDULE_NAME LIKE '%{0}%'",scheduleName.PreventSQLInjection());
                }
                //返回结果集
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchSchedule Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据排班计划主键获取班次数据。
        /// </summary>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <returns>包含班次数据的数据集对象。</returns>
        public DataSet GetShift(string scheduleKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql =string.Format(@"SELECT A.SHIFT_KEY,A.SHIFT_NAME,A.START_TIME,A.END_TIME,A.DESCRIPTIONS,A.OVER_DAY                               
                                            FROM CAL_SHIFT A
                                            WHERE A.SHIFT_KEY IN (SELECT SHIFT_KEY
                                                                 FROM CAL_SCHEDULE_SHIFT
                                                                 WHERE SCHEDULE_KEY = '{0}')",
                                            scheduleKey.PreventSQLInjection());
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
        /// 更新排班计划数据。
        /// </summary>
        /// <param name="dsParams">包含排班计划数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateSchedule(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string scheduleName = "";
            if (dsParams.Tables.Contains(CAL_SCHEDULE.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[CAL_SCHEDULE.DATABASE_TABLE_NAME];
                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    if (dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == CAL_SCHEDULE.FIELD_SCHEDULE_NAME)
                    {
                        scheduleName = dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE].ToString();
                    }
                }
                if (scheduleName != "")
                {
                    string strSql =string.Format(@"SELECT SCHEDULE_NAME FROM CAL_SCHEDULE WHERE SCHEDULE_NAME='{0}'",scheduleName.PreventSQLInjection());
                    using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql))
                    {
                        if (dataReader.Read())
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Addins.Msg.NameIsExist}");
                            return dsReturn;
                        }
                    }
                }
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new CAL_SCHEDULE(),
                                                        dsParams.Tables[CAL_SCHEDULE.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                        {CAL_SCHEDULE.FIELD_EDIT_TIME,null},                                                            
                                                        },
                                                        new List<string>() 
                                                        {
                                                         CAL_SCHEDULE.FIELD_SCHEDULE_KEY                                                         
                                                        },
                                                        CAL_SCHEDULE.FIELD_SCHEDULE_KEY);
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
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("UpdateSchedule Error: " + ex.Message);
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
        /// 删除排班计划数据。
        /// </summary>
        /// <param name="dsParams">包含排班主键的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteSchedule(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sql = "";
            string shiftKey = "";
            string scheduleKey = "";
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                //Create Transaction  
                dbTran = dbConn.BeginTransaction();
                if (dsParams.Tables.Contains(CAL_SCHEDULE.DATABASE_TABLE_NAME))
                {
                    scheduleKey = dsParams.Tables[CAL_SCHEDULE.DATABASE_TABLE_NAME].Rows[0][CAL_SCHEDULE.FIELD_SCHEDULE_KEY].ToString();
                    sql =string.Format(@"DELETE FROM CAL_SCHEDULE WHERE SCHEDULE_KEY='{0}'",scheduleKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    if (dsParams.Tables.Contains(CAL_SHIFT.DATABASE_TABLE_NAME))
                    {
                        DataTable shiftTable = dsParams.Tables[CAL_SHIFT.DATABASE_TABLE_NAME];
                        for (int i = 0; i < shiftTable.Rows.Count; i++)
                        {
                            shiftKey = shiftTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_KEY].ToString();
                            ShiftEngine.DeleteShift(db, dbTran, scheduleKey, shiftKey);
                        }
                    }
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.CalShiftEngine.TableIsNotExist}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                dbTran.Rollback();
                LogService.LogError("DeleteSchedule Error: " + ex.Message);
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
        /// 保存具体月份和天的排班数据。
        /// </summary>
        /// <param name="dsParams">包含具体月份和天的排班数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveShiftOfSchedule(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sql = "";
            string mkey = "";
            if (dsParams.Tables.Contains(CAL_SCHEDULE_MONTH.DATABASE_TABLE_NAME))
            {
                try
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    //Create Transaction  
                    dbTran = dbConn.BeginTransaction();
                    DataTable shiftMonthTable = dsParams.Tables[CAL_SCHEDULE_MONTH.DATABASE_TABLE_NAME];
                    mkey = shiftMonthTable.Rows[0][CAL_SCHEDULE_MONTH.FIELD_MKEY].ToString();
                    CAL_SCHEDULE_MONTH schduleMonthFields = new CAL_SCHEDULE_MONTH();
                    sql = DatabaseTable.BuildInsertSqlStatement(schduleMonthFields, shiftMonthTable, 0, 
                                                        new Dictionary<string, string>(){},
                                                        new List<string>());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    if (dsParams.Tables.Contains(CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME))
                    {
                        DataTable shiftDayTable = dsParams.Tables[CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME];
                        CAL_SCHEDULE_DAY shiftDayFields = new CAL_SCHEDULE_DAY();
                        for (int i = 0; i < shiftDayTable.Rows.Count; i++)
                        {
                            sql = DatabaseTable.BuildInsertSqlStatement(shiftDayFields, shiftDayTable, i, 
                                                        new Dictionary<string, string>()
                                                        {
                                                            {CAL_SCHEDULE_DAY.FIELD_MKEY,mkey},
                                                        },
                                                        new List<string>());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                    }
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("SaveShiftOfSchedule Error: " + ex.Message);
                }
                finally
                {
                    dbTran.Dispose();
                    dbConn.Close();
                    dbConn.Dispose();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取排班计划主键。
        /// </summary>
        /// <param name="year">年份。</param>
        /// <param name="month">月份。</param>
        /// <returns>排班计划主键。</returns>
        public string GetScheduleKey(string year, string month)
        {
            string scheduleKey = "";
            try
            {
                string sql =string.Format(@"SELECT SCHEDULE_KEY FROM CAL_SCHEDULE_MONTH WHERE CUR_YEAR='{0}' AND CUR_MONTH='{1}'",
                                            year.PreventSQLInjection(),month.PreventSQLInjection());
                using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, sql))
                {
                    if (dataReader.Read())
                    {
                        scheduleKey = dataReader[CAL_SCHEDULE_MONTH.FIELD_SCHEDULE_KEY].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetScheduleKey Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
            return scheduleKey;
        }
        /// <summary>
        /// 根据年份月份和排班计划主键获取排班数据。
        /// </summary>
        /// <param name="year">年份。</param>
        /// <param name="month">月份。</param>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <returns>包含排班数据的数据集对象。</returns>
        public DataSet GetShiftOfSchedule(string year, string month, string scheduleKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql =string.Format(@"SELECT MKEY,DKEY,DAY,SHIFT_VALUE,SHIFT_KEY 
                                            FROM CAL_SCHEDULE_DAY WHERE MKEY IN (SELECT MKEY 
                                                                                FROM CAL_SCHEDULE_MONTH 
                                                                                WHERE CUR_YEAR='{0}' AND CUR_MONTH='{1}' AND SCHEDULE_KEY='{2}')",
                                            year.PreventSQLInjection(), month.PreventSQLInjection(), scheduleKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                DataTable dataTable = dsReturn.Tables[0];
                dataTable.TableName = CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShiftOfSchedule Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新具体月份和天的排班数据。
        /// </summary>
        /// <param name="dsParams">包含具体月份和天的排班数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateShiftOfSchedule(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sql = "";
            if (dsParams.Tables.Contains(CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME))
            {
                DataTable dataTable = dsParams.Tables[CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME];
                CAL_SCHEDULE_DAY schedukeDayFields = new CAL_SCHEDULE_DAY();
                try
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    //Create Transaction  
                    dbTran = dbConn.BeginTransaction();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        OperationAction action = (OperationAction)Convert.ToInt32(dataTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                        switch (action)
                        {
                            case OperationAction.New:
                                sql = DatabaseTable.BuildInsertSqlStatement(schedukeDayFields, dataTable, i, 
                                                        new Dictionary<string, string>()
                                                        {

                                                        },
                                                       new List<string>(){
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                        
                                                       });

                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                                break;
                            case OperationAction.Delete:
                                sql = @"DELETE FROM CAL_SCHEDULE_DAY WHERE DKEY='" + dataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_DKEY].ToString().PreventSQLInjection() + "'";
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                                break;
                            case OperationAction.Modified:
                                sql = @"UPDATE CAL_SCHEDULE_DAY SET SHIFT_VALUE='" + dataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_SHIFT_VALUE].ToString().PreventSQLInjection() + "' " +
                                    "WHERE DKEY='" + dataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_DKEY].ToString().PreventSQLInjection() + "'";
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                                break;
                        }
                    }
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("UpdateShiftOfSchedule Error: " + ex.Message);
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
        /// 删除指定月份的排班数据。
        /// </summary>
        /// <param name="monthKey">排班月份主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteShiftOfSchedule(string monthKey)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sql = "";
            if (string.IsNullOrEmpty(monthKey))
            {
                try
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    //Create Transaction  
                    dbTran = dbConn.BeginTransaction();
                    sql = @"DELETE FROM CAL_SCHEDULE_DAY WHERE MKEY='" + monthKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    sql = @"DELETE FROM CAL_SCHEDULE_MONTH WHERE MKEY='" + monthKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("DeleteShiftOfSchedule Error: " + ex.Message);
                }
                finally
                {
                    dbTran.Dispose();
                    dbConn.Close();
                    dbConn.Dispose();
                }
            }
            return dsReturn;
        }
    }
   
}
