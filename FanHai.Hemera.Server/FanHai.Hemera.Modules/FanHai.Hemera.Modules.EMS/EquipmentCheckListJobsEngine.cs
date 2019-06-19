using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Utils.StaticFuncs;
using System.Data.Common;

namespace SolarViewer.Hemera.Modules.EMS
{
    public class EquipmentCheckListJobsEngine : AbstractEngine
    {
        private Database db;

        public EquipmentCheckListJobsEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentCheckListJobsEngine()
        {

        }

        public override void Initialize()
        {

        }

        public DataSet GetCheckListJobs(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string equipmentKey = reqDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY) ? reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY].ToString() : string.Empty;
                string checkListJobKey = reqDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY) ? reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].ToString() : string.Empty;
                string checkListJobName = reqDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME) ? reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME].ToString() : string.Empty;
                string checkListJobState = reqDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE) ? reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE].ToString() : string.Empty;
                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    string sqlString = @"SELECT CJ.*,
                                               E.EQUIPMENT_NAME,
                                               C.CHECKLIST_NAME,
                                               CASE CJ.PM_TYPE
                                                 WHEN 'SCHEDULE' THEN
                                                  PS.SCHEDULE_NAME
                                                 WHEN 'CONDITION' THEN
                                                  PC.CONDITION_NAME
                                                 ELSE
                                                  ''
                                               END AS PM_NAME
                                          FROM EMS_CHECKLIST_JOBS CJ,
                                               EMS_CHECKLIST      C,
                                               EMS_PM_SCHEDULE    PS,
                                               EMS_PM_CONDITION   PC,
                                               EMS_EQUIPMENTS     E
                                         WHERE CJ.CHECKLIST_KEY = C.CHECKLIST_KEY
                                           AND CJ.PM_KEY = PS.SCHEDULE_KEY(+)
                                           AND CJ.PM_KEY = PC.CONDITION_KEY(+)
                                           AND CJ.EQUIPMENT_KEY = E.EQUIPMENT_KEY";

                    if (!string.IsNullOrEmpty(checkListJobState))
                    {
                        sqlString += string.Format(" AND CJ.CHECKLIST_JOB_STATE = '{0}'", checkListJobState);
                    }

                    if (!string.IsNullOrEmpty(checkListJobName))
                    {
                        sqlString += string.Format(" AND CJ.CHECKLIST_JOB_NAME LIKE '%{0}%'", checkListJobName);
                    }

                    if (!string.IsNullOrEmpty(checkListJobKey))
                    {
                        sqlString += string.Format(" AND CJ.CHECKLIST_JOB_KEY = '{0}'", checkListJobKey);
                    }

                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sqlString += string.Format(" AND E.EQUIPMENT_KEY = '{0}'", equipmentKey);
                    }

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetCheckListJobs Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet GetCheckListJobData(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY))
            {
                string checkListJobKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();

                try
                {
                    string sqlString = string.Format(@"SELECT CJD.CHECKLIST_JOB_KEY,
                                                           CJD.CHECKITEM_KEY,
                                                           CI.SEQUENCE,
                                                           CIS.CHECKITEM_NAME,
                                                           CIS.CHECKITEM_TYPE,
                                                           CIS.DESCRIPTION,
                                                           CI.STANDARD,
                                                           CI.OPTIONAL,
                                                           CJD.CHECKITEM_VALUE,
                                                           CJD.REMARK,
                                                           CJD.COMPLETE_TIMESTAMP
                                                      FROM EMS_CHECKLIST_JOB_DATA CJD,
                                                           EMS_CHECKLIST_JOBS     CJ,
                                                           EMS_CHECKLIST_ITEM     CI,
                                                           EMS_CHECKITEMS         CIS
                                                     WHERE CI.CHECKITEM_KEY = CIS.CHECKITEM_KEY
                                                       AND CJ.CHECKLIST_KEY = CI.CHECKLIST_KEY
                                                       AND CJD.CHECKITEM_KEY = CIS.CHECKITEM_KEY
                                                       AND CJD.CHECKLIST_JOB_KEY = CJ.CHECKLIST_JOB_KEY
                                                       AND CJD.CHECKLIST_JOB_KEY = '{0}'
                                                     ORDER BY CI.SEQUENCE ASC", checkListJobKey);

                    db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_CHECKLIST_JOB_DATA_FIELDS.DATABASE_TABLE_NAME });

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetCheckListJobData Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertCheckListJob(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable checkListJobsDataTable = reqDS.Tables[EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME];

                try
                {
                    if (checkListJobsDataTable.Rows.Count > 0)
                    {
                        string checkListJobKey = checkListJobsDataTable.Rows[0][EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].ToString();
                        string equipmentKey = checkListJobsDataTable.Rows[0][EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                        string checkListJobName = string.Empty;
                        string createTimeStamp = string.Empty;
                        string createTime = string.Empty;

                        using (DbConnection connection = db.CreateConnection())
                        {
                            connection.Open();

                            using (DbTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    #region Generate Check List Job Name

                                    string equipmentName = string.Empty;

                                    object scalar = db.ExecuteScalar(transaction, CommandType.Text, string.Format("SELECT EQUIPMENT_NAME FROM EMS_EQUIPMENTS E WHERE E.EQUIPMENT_KEY = '{0}'", equipmentKey));

                                    if (scalar != null && scalar != DBNull.Value)
                                    {
                                        equipmentName = scalar.ToString();
                                    }

                                    if (string.IsNullOrEmpty(equipmentName))
                                    {
                                        throw new Exception("所选设备的名称为空!");
                                    }

                                    string checkListJobNamePrefix = DateTime.Now.ToString(COMMON_FORMAT.FAST_DATE_FORMAT) + equipmentName;

                                    scalar = db.ExecuteScalar(transaction, CommandType.Text, string.Format("SELECT MAX(CLJ.CHECKLIST_JOB_NAME) FROM EMS_CHECKLIST_JOBS CLJ WHERE CLJ.CHECKLIST_JOB_NAME LIKE '{0}%'", checkListJobNamePrefix));

                                    uint serialNumber = 1;

                                    if (scalar != null && scalar != DBNull.Value)
                                    {
                                        string maxCheckListJobName = scalar.ToString();

                                        serialNumber = Convert.ToUInt32(maxCheckListJobName.Substring(maxCheckListJobName.Length - 3, 3)) + 1;
                                    }

                                    checkListJobName = checkListJobNamePrefix + serialNumber.ToString("000");

                                    #endregion

                                    #region Generate Check List Job Insert SQL Statement

                                    checkListJobsDataTable.Rows[0][EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME] = checkListJobName;

                                    EMS_CHECKLIST_JOBS_FIELDS checkListJobsFields = new EMS_CHECKLIST_JOBS_FIELDS();

                                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(checkListJobsFields, checkListJobsDataTable);

                                    #endregion

                                    #region Insert Check List Job Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlStringList[0]) > 0)
                                    {
                                        #region Get Create TimeStamp And Create Time

                                        List<string> interestFields = new List<string>();

                                        interestFields.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP);
                                        interestFields.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME);

                                        Conditions conditions = new Conditions();

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkListJobKey);

                                        string sqlString = DatabaseTable.BuildQuerySqlStatement(checkListJobsFields, interestFields, conditions);

                                        using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, sqlString))
                                        {
                                            if (dataReader.Read())
                                            {
                                                createTimeStamp = dataReader.GetDateTime(0).ToString();
                                                createTime = dataReader.GetDateTime(1).ToString();
                                            }

                                            dataReader.Close();
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
                        resDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME, checkListJobName);
                        resDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP, createTimeStamp);
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, createTime);
                    }
                    else
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                    }
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("InsertCheckListJob Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateCheckListJob(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                DataTable checkListJobsDataTable = reqDS.Tables[EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME];

                string checkListJobKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_CHECKLIST_JOBS_FIELDS checkListJobsFields = new EMS_CHECKLIST_JOBS_FIELDS();

                    #region Build Update SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(checkListJobKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, checkListJobKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(checkListJobsFields, checkListJobsDataTable, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0 && checkListJobsDataTable.Rows.Count > 0)
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
                                #region Update Check List Job Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    editTime = AllCommonFunctions.GetSpecifyTableColumnData(checkListJobsFields, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY,
                                        checkListJobKey, transaction);
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
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateCheckListJob Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet DeleteCheckListJob(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string checkListJobKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_CHECKLIST_JOBS_FIELDS checkListJobsFields = new EMS_CHECKLIST_JOBS_FIELDS();

                    #region Build Delete SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(checkListJobKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, checkListJobKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(checkListJobsFields, conditionsList);

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
                                #region Delete Check List Job Item Data

                                db.ExecuteNonQuery(transaction, CommandType.Text, string.Format("DELETE EMS_CHECKLIST_JOB_DATA WHERE CHECKLIST_JOB_KEY = '{0}'", checkListJobKey));

                                #endregion

                                #region Delete Check List Job Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
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
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("DeleteCheckListJob Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet StartCheckListJob(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && 
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIMEZONE) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string checkListJobKey = reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].ToString();
                string checkListKey = reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY].ToString();
                string startTimeStamp = string.Empty;
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTimeZone = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIMEZONE].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();
                string sqlString = string.Empty;

                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Insert Check List Job Item Data

                                sqlString = string.Format(@"INSERT INTO EMS_CHECKLIST_JOB_DATA
                                                              (CHECKLIST_JOB_KEY, CHECKITEM_KEY)
                                                              SELECT '{0}' AS CHECKLIST_JOB_KEY, CI.CHECKITEM_KEY
                                                                FROM EMS_CHECKLIST_ITEM CI
                                                               WHERE CI.CHECKLIST_KEY = '{1}'
                                                               ORDER BY CI.SEQUENCE ASC", checkListJobKey, checkListKey);

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Update Check List Job Data

                                sqlString = string.Format(@"UPDATE EMS_CHECKLIST_JOBS
                                                               SET CHECKLIST_JOB_STATE = 'STARTED',
                                                                   START_TIMESTAMP     = SYSDATE,
                                                                   EDITOR              = '{0}',
                                                                   EDIT_TIME           = SYSDATE,
                                                                   EDIT_TIMEZONE       = '{1}'
                                                                WHERE", editor, editTimeZone);

                                if(string.IsNullOrEmpty(editTime))
                                {
                                    sqlString += " EDIT_TIME IS NULL";
                                }
                                else
                                {
                                    sqlString += string.Format(" EDIT_TIME = TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS')", editTime);
                                }

                                sqlString += string.Format(" AND CHECKLIST_JOB_KEY = '{0}'", checkListJobKey);

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    #region Get Start TimeStamp And Edit Time

                                    EMS_CHECKLIST_JOBS_FIELDS checkListJobsFields = new EMS_CHECKLIST_JOBS_FIELDS();

                                    List<string> interestFields = new List<string>();

                                    interestFields.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP);
                                    interestFields.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME);

                                    Conditions conditions = new Conditions();

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkListJobKey);

                                    sqlString = DatabaseTable.BuildQuerySqlStatement(checkListJobsFields, interestFields, conditions);

                                    using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, sqlString))
                                    {
                                        if (dataReader.Read())
                                        {
                                            startTimeStamp = dataReader.GetDateTime(0).ToString();
                                            editTime = dataReader.GetDateTime(1).ToString();
                                        }

                                        dataReader.Close();
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
                    resDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP, startTimeStamp);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("StartCheckListJob Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet SaveCheckListJobData(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKLIST_JOB_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE) &&
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK))
            {
                string checkListJobKey = reqDS.ExtendedProperties[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKLIST_JOB_KEY].ToString();
                string checkItemKey = reqDS.ExtendedProperties[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_KEY].ToString();
                string checkItemValue = reqDS.ExtendedProperties[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE].ToString();
                string remark = reqDS.ExtendedProperties[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK].ToString();
                string completeTimeStamp = string.Empty;

                try
                {
                    #region Save Check List Job Item Data

                    string sqlString = string.Format(@"UPDATE EMS_CHECKLIST_JOB_DATA
                                                       SET CHECKITEM_VALUE    = '{2}',
                                                           COMPLETE_TIMESTAMP = SYSDATE,
                                                           REMARK             = '{3}'
                                                     WHERE CHECKLIST_JOB_KEY = '{0}'
                                                       AND CHECKITEM_KEY = '{1}'", checkListJobKey, checkItemKey, checkItemValue, remark);

                    if (db.ExecuteNonQuery(CommandType.Text, sqlString) > 0)
                    {
                        sqlString = string.Format(@"SELECT COMPLETE_TIMESTAMP
                                                   FROM EMS_CHECKLIST_JOB_DATA
                                                  WHERE CHECKLIST_JOB_KEY = '{0}'
                                                    AND CHECKITEM_KEY = '{1}'", checkListJobKey, checkItemKey);

                        object scalar = db.ExecuteScalar(CommandType.Text, sqlString);

                        if (scalar != null && scalar != DBNull.Value)
                        {
                            completeTimeStamp = scalar.ToString();
                        }
                    }
                    else
                    {
                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                    }

                    #endregion

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP, completeTimeStamp);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("SaveCheckListJobData Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet CompleteCheckListJob(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.Contains(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIMEZONE) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string checkListJobKey = reqDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].ToString();
                string completeTimeStamp = string.Empty;
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTimeZone = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIMEZONE].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();
                string sqlString = string.Empty;

                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Update Check List Job Data

                                sqlString = string.Format(@"UPDATE EMS_CHECKLIST_JOBS
                                                               SET CHECKLIST_JOB_STATE = 'COMPLETED',
                                                                   COMPLETE_TIMESTAMP     = SYSDATE,
                                                                   EDITOR              = '{0}',
                                                                   EDIT_TIME           = SYSDATE,
                                                                   EDIT_TIMEZONE       = '{1}'
                                                                WHERE", editor, editTimeZone);

                                if (string.IsNullOrEmpty(editTime))
                                {
                                    sqlString += " EDIT_TIME IS NULL";
                                }
                                else
                                {
                                    sqlString += string.Format(" EDIT_TIME = TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS')", editTime);
                                }

                                sqlString += string.Format(" AND CHECKLIST_JOB_KEY = '{0}'", checkListJobKey);

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    #region Get Complete TimeStamp And Edit Time

                                    EMS_CHECKLIST_JOBS_FIELDS checkListJobsFields = new EMS_CHECKLIST_JOBS_FIELDS();

                                    List<string> interestFields = new List<string>();

                                    interestFields.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP);
                                    interestFields.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME);

                                    Conditions conditions = new Conditions();

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkListJobKey);

                                    sqlString = DatabaseTable.BuildQuerySqlStatement(checkListJobsFields, interestFields, conditions);

                                    using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, sqlString))
                                    {
                                        if (dataReader.Read())
                                        {
                                            completeTimeStamp = dataReader.GetDateTime(0).ToString();
                                            editTime = dataReader.GetDateTime(1).ToString();
                                        }

                                        dataReader.Close();
                                    }

                                    #endregion
                                }
                                else
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Process Check List Job PM Data

                                #region Get Check List Job PM Data

                                string pmType = string.Empty;
                                string pmKey = string.Empty;

                                sqlString = string.Format(@"SELECT CLJ.PM_TYPE, CLJ.PM_KEY
                                                              FROM EMS_CHECKLIST_JOBS CLJ
                                                             WHERE CLJ.CHECKLIST_JOB_KEY = '{0}'", checkListJobKey);

                                using (IDataReader dataReader = db.ExecuteReader(transaction, CommandType.Text, sqlString))
                                {
                                    if (dataReader.Read())
                                    {
                                        pmType = dataReader.GetValue(0).ToString();
                                        pmKey = dataReader.GetValue(1).ToString();
                                    }

                                    dataReader.Close();
                                }

                                #endregion

                                #region Schedule PM

                                if (pmType == "SCHEDULE" && pmKey.Length > 0)
                                {
                                    sqlString = string.Format(@"SELECT PMS.BASE_ACTUAL_FINISH_TIME FROM EMS_PM_SCHEDULE PMS WHERE PMS.SCHEDULE_KEY = '{0}'", pmKey);

                                    object scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlString);

                                    if (scalar != null && scalar != DBNull.Value)
                                    {
                                        if (scalar.ToString() == "1")
                                        {
                                            #region Calculate Schedule PM Next Event Time

                                            sqlString = string.Format(@"UPDATE EMS_PM_SCHEDULE
                                                                           SET NEXT_EVENT_TIME = CASE FREQUENCE_UNIT WHEN '小时' THEN SYSDATE + FREQUENCE * INTERVAL '1' HOUR WHEN '天' THEN SYSDATE + FREQUENCE * INTERVAL '1' DAY WHEN '周' THEN SYSDATE + FREQUENCE * INTERVAL '7' DAY WHEN '月' THEN SYSDATE + FREQUENCE * INTERVAL '1' MONTH WHEN '年' THEN SYSDATE + FREQUENCE * INTERVAL '1' YEAR ELSE SYSDATE + FREQUENCE * INTERVAL '1' HOUR END
                                                                         WHERE SCHEDULE_KEY = '{0}'", pmKey);

                                            if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                            {
                                                throw new Exception("计算计划PM下次触发时间出错!");
                                            }

                                            #endregion
                                        }
                                    }
                                }

                                #endregion

                                #region Condition PM

                                if (pmType == "CONDITION" && pmKey.Length > 0)
                                {
                                    //TODO: Not Implement
                                }

                                #endregion

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
                    resDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP, completeTimeStamp);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("CompleteCheckListJob Error: " + ex.Message);
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
