using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using System.Data.Common;

namespace SolarViewer.Hemera.Modules.EMS
{
    public class EquipmentSchedulePMEngine : AbstractEngine
    {
        private Database db;

        public EquipmentSchedulePMEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentSchedulePMEngine()
        {

        }

        public override void Initialize()
        {
            
        }

        public DataSet GetSchedulePM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string equipmentKey = reqDS.ExtendedProperties.ContainsKey(EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_KEY) ? reqDS.ExtendedProperties[EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_KEY].ToString() : string.Empty;
                
                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    string sqlString = @"SELECT PS.*,
                                               U.USERNAME AS NOTIFY_USER_NAME,
                                               UC.USERNAME AS NOTIFY_CC_USER_NAME,
                                               ECS.EQUIPMENT_CHANGE_STATE_NAME,
                                               ECR.EQUIPMENT_CHANGE_REASON_NAME,
                                               E.EQUIPMENT_NAME,
                                               C.CHECKLIST_NAME
                                          FROM EMS_PM_SCHEDULE              PS,
                                               EMS_CHECKLIST                C,
                                               EMS_EQUIPMENTS               E,
                                               RBAC_USER                    U,
                                               RBAC_USER                    UC,
                                               EMS_EQUIPMENT_CHANGE_REASONS ECR,
                                               EMS_EQUIPMENT_CHANGE_STATES  ECS
                                         WHERE PS.CHECKLIST_KEY = C.CHECKLIST_KEY
                                           AND PS.EQUIPMENT_KEY = E.EQUIPMENT_KEY
                                           AND PS.NOTIFY_USER_KEY = U.USER_KEY
                                           AND PS.NOTIFY_CC_USER_KEY = UC.USER_KEY(+)
                                           AND PS.EQUIPMENT_CHANGE_REASON_KEY = ECR.EQUIPMENT_CHANGE_REASON_KEY(+)
                                           AND PS.EQUIPMENT_CHANGE_STATE_KEY = ECS.EQUIPMENT_CHANGE_STATE_KEY(+)";

                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sqlString += string.Format(" AND E.EQUIPMENT_KEY = '{0}'", equipmentKey);
                    }

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetSchedulePM Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertSchedulePM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable schedulePMDataTable = reqDS.Tables[EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME];

                if (schedulePMDataTable.Rows.Count > 0)
                {
                    try
                    {
                        EMS_PM_SCHEDULE_FIELDS schedulePMFields = new EMS_PM_SCHEDULE_FIELDS();

                        List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(schedulePMFields, schedulePMDataTable);

                        string scheduleKey = schedulePMDataTable.Rows[0][EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY].ToString();
                        string scheduleName = schedulePMDataTable.Rows[0][EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME].ToString();
                        string nextEventTime = string.Empty;
                        string createTime = string.Empty;
                        string sqlString = string.Empty;

                        if (sqlStringList.Count > 0)
                        {
                            sqlString = sqlStringList[0];
                        }

                        using (DbConnection connection = db.CreateConnection())
                        {
                            connection.Open();

                            using (DbTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    #region Validate Schedule PM Name

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(schedulePMFields, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME, scheduleName, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("计划PM名称已存在!");
                                    }

                                    #endregion

                                    #region Insert Schedule PM Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                    {
                                        #region Calculate Schedule PM Next Event Time

                                        sqlString = string.Format(@"UPDATE EMS_PM_SCHEDULE
                                                                       SET NEXT_EVENT_TIME = CASE FREQUENCE_UNIT WHEN '小时' THEN SYSDATE + FREQUENCE * INTERVAL '1' HOUR WHEN '天' THEN SYSDATE + FREQUENCE * INTERVAL '1' DAY WHEN '周' THEN SYSDATE + FREQUENCE * INTERVAL '7' DAY WHEN '月' THEN SYSDATE + FREQUENCE * INTERVAL '1' MONTH WHEN '年' THEN SYSDATE + FREQUENCE * INTERVAL '1' YEAR ELSE SYSDATE + FREQUENCE * INTERVAL '1' HOUR END
                                                                     WHERE SCHEDULE_KEY = '{0}'", scheduleKey);

                                        if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                        {
                                            #region Get Next Event Time And Create Time

                                            List<string> interestFields = new List<string>();

                                            interestFields.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME);
                                            interestFields.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_CREATE_TIME);

                                            Conditions conditions = new Conditions();

                                            conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY, GlobalEnums.DatabaseCompareOperator.Equal, scheduleKey);

                                            sqlString = DatabaseTable.BuildQuerySqlStatement(schedulePMFields, interestFields, conditions);

                                            using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, sqlString))
                                            {
                                                if (dataReader.Read())
                                                {
                                                    nextEventTime = dataReader.GetDateTime(0).ToString();
                                                    createTime = dataReader.GetDateTime(1).ToString();
                                                }

                                                dataReader.Close();
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            throw new Exception("计算下次PM时间出错!");
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }

                                    #endregion

                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();

                                    throw;
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }

                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                        resDS.ExtendedProperties.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME, nextEventTime);
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, createTime);
                    }
                    catch (Exception ex)
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                        LogService.LogError("InsertSchedulePM Error: " + ex.Message);
                    }
                }
                else
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateSchedulePM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                DataTable schedulePMDataTable = reqDS.Tables[EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME];

                string scheduleKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_PM_SCHEDULE_FIELDS schedulePMFields = new EMS_PM_SCHEDULE_FIELDS();

                    #region Build Update SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(scheduleKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, scheduleKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(schedulePMFields, schedulePMDataTable, conditionsList);

                    if (sqlStringList.Count > 0 && schedulePMDataTable.Rows.Count > 0)
                    {
                        string scheduleName = schedulePMDataTable.Rows[0][EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME].ToString();
                        string frequence = schedulePMDataTable.Rows[0][EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE].ToString();
                        string frequenceUnit = schedulePMDataTable.Rows[0][EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE_UNIT].ToString();
                        string nextEventTime = string.Empty;
                        string sqlString = sqlStringList[0];

                        using (DbConnection connection = db.CreateConnection())
                        {
                            connection.Open();

                            using (DbTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    #region Validate Schedule PM Name

                                    if (!string.IsNullOrEmpty(scheduleName))
                                    {
                                        conditions = new Conditions();

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME, GlobalEnums.DatabaseCompareOperator.Equal, scheduleName);

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY, GlobalEnums.DatabaseCompareOperator.NotEqual, scheduleKey);

                                        string returnData = AllCommonFunctions.GetSpecifyTableColumnData(schedulePMFields, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME, conditions, transaction);

                                        if (!string.IsNullOrEmpty(returnData))
                                        {
                                            throw new Exception("计划PM名称已存在!");
                                        }
                                    }

                                    #endregion

                                    #region Update Schedule PM Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                    {
                                        if (string.IsNullOrEmpty(frequence) && string.IsNullOrEmpty(frequenceUnit))
                                        {
                                            editTime = AllCommonFunctions.GetSpecifyTableColumnData(schedulePMFields, EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY, scheduleKey, transaction);
                                        }
                                        else
                                        {
                                            #region Calculate Schedule PM Next Event Time

                                            sqlString = string.Format(@"UPDATE EMS_PM_SCHEDULE
                                                                       SET NEXT_EVENT_TIME = CASE FREQUENCE_UNIT WHEN '小时' THEN SYSDATE + FREQUENCE * INTERVAL '1' HOUR WHEN '天' THEN SYSDATE + FREQUENCE * INTERVAL '1' DAY WHEN '周' THEN SYSDATE + FREQUENCE * INTERVAL '7' DAY WHEN '月' THEN SYSDATE + FREQUENCE * INTERVAL '1' MONTH WHEN '年' THEN SYSDATE + FREQUENCE * INTERVAL '1' YEAR ELSE SYSDATE + FREQUENCE * INTERVAL '1' HOUR END
                                                                     WHERE SCHEDULE_KEY = '{0}'", scheduleKey);

                                            if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                            {
                                                #region Get Next Event Time And Edit Time

                                                List<string> interestFields = new List<string>();

                                                interestFields.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME);
                                                interestFields.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME);

                                                conditions = new Conditions();

                                                conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY, GlobalEnums.DatabaseCompareOperator.Equal, scheduleKey);

                                                sqlString = DatabaseTable.BuildQuerySqlStatement(schedulePMFields, interestFields, conditions);

                                                using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, sqlString))
                                                {
                                                    if (dataReader.Read())
                                                    {
                                                        nextEventTime = dataReader.GetDateTime(0).ToString();
                                                        editTime = dataReader.GetDateTime(1).ToString();
                                                    }

                                                    dataReader.Close();
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                throw new Exception("计算下次PM时间出错!");
                                            }

                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }

                                    #endregion

                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();

                                    throw;
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }

                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                        resDS.ExtendedProperties.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME, nextEventTime);
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                    }
                    else
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                    }
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateSchedulePM Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet DeleteSchedulePM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string scheduleKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_PM_SCHEDULE_FIELDS schedulePMFields = new EMS_PM_SCHEDULE_FIELDS();

                    #region Build Delete SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(scheduleKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, scheduleKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(schedulePMFields, conditionsList);

                    if (sqlStringList.Count > 0)
                    {
                        #region Delete Schedule PM Data

                        if (db.ExecuteNonQuery(CommandType.Text, sqlStringList[0]) > 0)
                        {
                            resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                        }
                        else
                        {
                            throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                        }

                        #endregion
                    }
                    else
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                    }
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("DeleteSchedulePM Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }
    }
}
