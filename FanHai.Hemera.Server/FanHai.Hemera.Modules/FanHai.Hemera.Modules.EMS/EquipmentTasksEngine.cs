//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-09-19            重构 迁移到SQL Server数据库
// =================================================================================
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
    public class EquipmentTasksEngine : AbstractEngine
    {
        private Database db;

        public EquipmentTasksEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentTasksEngine()
        {

        }

        public override void Initialize()
        {
            
        }

        public DataSet GetTask(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            int pages = 0;
            int records = 0;

            if (dsParams != null &&
                dsParams.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                dsParams.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string equipmentKey = dsParams.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY) ? dsParams.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY].ToString() : string.Empty;
                string taskKey = dsParams.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY) ? dsParams.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].ToString() : string.Empty;
                string taskName = dsParams.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME) ? dsParams.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME].ToString() : string.Empty;
                string taskState = dsParams.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE) ? dsParams.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE].ToString() : string.Empty;

                int pageNo = Convert.ToInt32(dsParams.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(dsParams.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    string sqlString = @"SELECT ET.*,ECS.EQUIPMENT_CHANGE_STATE_NAME,ECR.EQUIPMENT_CHANGE_REASON_NAME,E.EQUIPMENT_NAME
                                        FROM EMS_EQUIPMENT_TASKS ET
                                        INNER JOIN EMS_EQUIPMENTS  E ON ET.EQUIPMENT_KEY = E.EQUIPMENT_KEY
                                        LEFT JOIN EMS_EQUIPMENT_CHANGE_REASONS ECR ON ET.EQUIPMENT_CHANGE_REASON_KEY = ECR.EQUIPMENT_CHANGE_REASON_KEY
                                        LEFT JOIN EMS_EQUIPMENT_CHANGE_STATES  ECS ON ET.EQUIPMENT_CHANGE_STATE_KEY = ECS.EQUIPMENT_CHANGE_STATE_KEY";

                    if (!string.IsNullOrEmpty(taskState))
                    {
                        sqlString += string.Format(" AND ET.EQUIPMENT_TASK_STATE = '{0}'", taskState.PreventSQLInjection());
                    }

                    if (!string.IsNullOrEmpty(taskName))
                    {
                        sqlString += string.Format(" AND ET.EQUIPMENT_TASK_NAME LIKE '%{0}%'", taskName.PreventSQLInjection());
                    }

                    if (!string.IsNullOrEmpty(taskKey))
                    {
                        sqlString += string.Format(" AND ET.EQUIPMENT_TASK_KEY = '{0}'", taskKey.PreventSQLInjection());
                    }

                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sqlString += string.Format(" AND E.EQUIPMENT_KEY = '{0}'", equipmentKey.PreventSQLInjection());
                    }

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, dsReturn, EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME });
                    }

                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetTask Error: " + ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return dsReturn;
        }

        public DataSet GetTaskCourse(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            if (dsParams != null && dsParams.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY))
            {
                string taskKey = dsParams.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();

                try
                {
                    string sqlString = @"SELECT ETC.*,ECS.EQUIPMENT_CHANGE_STATE_NAME,ECR.EQUIPMENT_CHANGE_REASON_NAME
                                        FROM EMS_EQUIPMENT_TASK_COURSES   ETC
                                        LEFT JOIN EMS_EQUIPMENT_CHANGE_REASONS ECR ON ETC.EQUIPMENT_CHANGE_REASON_KEY = ECR.EQUIPMENT_CHANGE_REASON_KEY
                                        LEFT JOIN EMS_EQUIPMENT_CHANGE_STATES  ECS ON ETC.EQUIPMENT_CHANGE_STATE_KEY = ECS.EQUIPMENT_CHANGE_STATE_KEY";

                    if (!string.IsNullOrEmpty(taskKey))
                    {
                        sqlString += string.Format(" AND ETC.EQUIPMENT_TASK_KEY = '{0}'", taskKey.PreventSQLInjection());
                    }

                    sqlString += " ORDER BY ETC.SEND_TIMESTAMP ASC";

                    db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENT_TASK_COURSES_FIELDS.DATABASE_TABLE_NAME });

                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetTaskCourse Error: " + ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return dsReturn;
        }

        public DataSet GetTaskCoursePart(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY))
            {
                string taskCourseKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();

                try
                {
                    string sqlString = @"SELECT ETCP.*,
                                               EP.EQUIPMENT_PART_NAME,
                                               EP.DESCRIPTION,
                                               EP.EQUIPMENT_PART_TYPE,
                                               EP.EQUIPMENT_PART_MODE,
                                               EP.EQUIPMENT_PART_UNIT
                                         FROM EMS_EQP_TASK_COURSE_PARTS ETCP, EMS_EQUIPMENT_PARTS EP
                                         WHERE ETCP.EQP_PART_KEY = EP.EQUIPMENT_PART_KEY";

                    if (!string.IsNullOrEmpty(taskCourseKey))
                    {
                        sqlString += string.Format(" AND ETCP.EQP_TASK_COURSE_KEY = '{0}'", taskCourseKey.PreventSQLInjection());
                    }

                    db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQP_TASK_COURSE_PARTS_FIELDS.DATABASE_TABLE_NAME });

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetTaskCoursePart Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertTask(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS))
            {
                DataTable tasksDataTable = reqDS.Tables[EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME];

                string taskCourseKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY].ToString();
                string receiveDeptKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY].ToString();
                string receiveDeptName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME].ToString();
                string comments = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS].ToString();

                try
                {
                    if (tasksDataTable.Rows.Count > 0)
                    {
                        string taskKey = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].ToString();
                        string taskState = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE].ToString();
                        string createUserKey = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY].ToString();
                        string equipmentKey = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                        string equipmentChangeStateKey = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                        string equipmentChangeReasonKey = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].ToString();
                        string creator = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR].ToString();
                        string createTimeZone = tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].ToString();
                        string taskName = string.Empty;
                        string createTimeStamp = string.Empty;
                        string createTime = string.Empty;
                        string sqlString = string.Empty;
                        object scalar = null;

                        using (DbConnection connection = db.CreateConnection())
                        {
                            connection.Open();

                            using (DbTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(equipmentChangeStateKey))
                                    {
                                        #region Validate Equipment State

                                        sqlString = string.Format(@"SELECT E.EQUIPMENT_KEY
                                                                    FROM EMS_EQUIPMENTS E
                                                                    WHERE EXISTS (SELECT ECS.EQUIPMENT_FROM_STATE_KEY
			                                                                      FROM EMS_EQUIPMENT_CHANGE_STATES ECS
			                                                                      WHERE ECS.EQUIPMENT_FROM_STATE_KEY = E.EQUIPMENT_STATE_KEY
			                                                                      AND ECS.EQUIPMENT_CHANGE_STATE_KEY ='{1}')
                                                                    AND E.EQUIPMENT_KEY = '{0}'", 
                                                                    equipmentKey.PreventSQLInjection(), 
                                                                    equipmentChangeStateKey.PreventSQLInjection());

                                        scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlString);

                                        if (scalar == null || scalar == DBNull.Value)
                                        {
                                            throw new Exception("设备当前状态与设备转变状态的起始状态不符合,不能创建该设备作业!");
                                        }

                                        #endregion

                                        #region Get Equipment Change Target State

                                        sqlString = string.Format(@"SELECT ECS.EQUIPMENT_TO_STATE_KEY
                                                                    FROM EMS_EQUIPMENT_CHANGE_STATES ECS
                                                                    WHERE ECS.EQUIPMENT_CHANGE_STATE_KEY = '{0}'",
                                                                    equipmentChangeStateKey.PreventSQLInjection());

                                        scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlString);

                                        string equipmentToStateKey = string.Empty;

                                        if (scalar != null && scalar != DBNull.Value)
                                        {
                                            equipmentToStateKey = scalar.ToString();
                                        }
                                        else
                                        {
                                            throw new Exception("获取设备转变状态的目标状态失败!");
                                        }

                                        #endregion

                                        #region Change Equipment State

                                        sqlString = string.Format(@"UPDATE EMS_EQUIPMENTS
                                                                    SET EQUIPMENT_STATE_KEY = '{1}',
                                                                       EQUIPMENT_CHANGE_STATE_KEY = '{2}',
                                                                       EDITOR = '{3}',
                                                                       EDIT_TIMEZONE = '{4}',
                                                                       EDIT_TIME = GETDATE()
                                                                    WHERE EQUIPMENT_KEY = '{0}'",
                                                                    equipmentKey.PreventSQLInjection(),
                                                                    equipmentToStateKey.PreventSQLInjection(),
                                                                    equipmentChangeStateKey.PreventSQLInjection(),
                                                                    creator.PreventSQLInjection(),
                                                                    createTimeZone.PreventSQLInjection());

                                        if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                        {
                                            throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                        }

                                        #endregion

                                        #region Update Equipment State Log

                                        sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_LOG SET END_TIMESTAMP = GETDATE() 
                                                                    WHERE EQUIPMENT_KEY = '{0}' AND END_TIMESTAMP IS NULL", 
                                                                    equipmentKey.PreventSQLInjection());

                                        db.ExecuteNonQuery(transaction, CommandType.Text, sqlString);

                                        #endregion

                                        #region Insert Equipment State Log

                                        sqlString = string.Format(@"INSERT INTO EMS_EQUIPMENT_LOG
                                                                    (EQUIPMENT_LOG_KEY,EQUIPMENT_KEY,EQUIPMENT_STATE_KEY,EQUIPMENT_CHANGE_STATE_KEY,EQUIPMENT_CHANGE_REASON_KEY,START_TIMESTAMP)
                                                                    VALUES('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE())",
                                                                    UtilHelper.GenerateNewKey(0),
                                                                    equipmentKey.PreventSQLInjection(),
                                                                    equipmentToStateKey.PreventSQLInjection(),
                                                                    equipmentChangeStateKey.PreventSQLInjection(), 
                                                                    equipmentChangeReasonKey.PreventSQLInjection());

                                        if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                        {
                                            throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                        }

                                        #endregion
                                    }

                                    #region Generate Task Name

                                    string taskNamePrefix = DateTime.Now.ToString(COMMON_FORMAT.FAST_DATE_FORMAT);

                                    scalar = db.ExecuteScalar(transaction, CommandType.Text, string.Format(@"SELECT MAX(ET.EQUIPMENT_TASK_NAME)
                                                                                                            FROM EMS_EQUIPMENT_TASKS ET 
                                                                                                            WHERE ET.EQUIPMENT_TASK_NAME LIKE '{0}%'", 
                                                                                                            taskNamePrefix.PreventSQLInjection()));

                                    uint serialNumber = 1;

                                    if (scalar != null && scalar != DBNull.Value)
                                    {
                                        string maxTaskName = scalar.ToString();

                                        serialNumber = Convert.ToUInt32(maxTaskName.Substring(maxTaskName.Length - 3, 3)) + 1;
                                    }

                                    taskName = taskNamePrefix + serialNumber.ToString("000");

                                    #endregion

                                    #region Generate Task Insert SQL Statement

                                    tasksDataTable.Rows[0][EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME] = taskName;

                                    EMS_EQUIPMENT_TASKS_FIELDS tasksFields = new EMS_EQUIPMENT_TASKS_FIELDS();

                                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(tasksFields, tasksDataTable);

                                    #endregion

                                    #region Insert Task Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlStringList[0]) > 0)
                                    {
                                        #region Generate Task Course Insert SQL Statement

                                        sqlString = string.Format(@"INSERT INTO EMS_EQUIPMENT_TASK_COURSES
                                                              (EQUIPMENT_TASK_COURSE_KEY,
                                                               EQUIPMENT_TASK_KEY,
                                                               EQUIPMENT_TASK_STATE,
                                                               SEND_USER_KEY,
                                                               RECEIVE_DEPT_KEY,
                                                               REMARK,
                                                               COMMENTS,
                                                               EQUIPMENT_CHANGE_STATE_KEY,
                                                               EQUIPMENT_CHANGE_REASON_KEY)
                                                            VALUES
                                                              ('{0}','{1}','{2}','{3}','{4}','提交{5}处理','{6}','{7}','{8}')",
                                                            taskCourseKey.PreventSQLInjection(),
                                                            taskKey.PreventSQLInjection(),
                                                            taskState.PreventSQLInjection(),
                                                            createUserKey.PreventSQLInjection(),
                                                            receiveDeptKey.PreventSQLInjection(),
                                                            receiveDeptName.PreventSQLInjection(),
                                                            comments.PreventSQLInjection(),
                                                            equipmentChangeStateKey.PreventSQLInjection(),
                                                            equipmentChangeReasonKey.PreventSQLInjection());

                                        #endregion

                                        #region Insert Task Course Data

                                        if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                        {
                                            #region Get Create TimeStamp And Create Time

                                            List<string> interestFields = new List<string>();

                                            interestFields.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP);
                                            interestFields.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME);

                                            Conditions conditions = new Conditions();

                                            conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY, GlobalEnums.DatabaseCompareOperator.Equal, taskKey);

                                            sqlString = DatabaseTable.BuildQuerySqlStatement(tasksFields, interestFields, conditions);

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
                                    }
                                    else
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }

                                    #endregion

                                    #region Send Notify Email To Specified Department

                                    List<string> sendToList = new List<string>();

                                    sendToList.Add(String.Empty); //Department E-Mail Address

                                    List<string> copyToList = new List<string>();

                                    string subject = string.Format("设备作业[{0}]待处理", taskName);
                                    string content = string.Format("用户[{0}]创建设备作业:{1}，提交{2}处理!", creator, taskName, receiveDeptName);

                                    try
                                    {
                                        AllCommonFunctions.LotusSendEMail(sendToList.ToArray(), copyToList.ToArray(), subject, content);
                                    }
                                    catch (Exception e)
                                    {
                                        LogService.LogError(string.Format("Send Equipment Task[{0}] Email is failed! Fail Reason:{1}.", taskName, e.Message));
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
                        resDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME, taskName);
                        resDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP, createTimeStamp);
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

                    LogService.LogError("InsertTask Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateTaskStartData(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY) &&
                reqDS.ExtendedProperties.Contains(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.Contains(PARAMETERS.INPUT_EDIT_TIMEZONE))
            {
                string taskKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].ToString();
                string startUserKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTimezone = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIMEZONE].ToString();

                try
                {
                    string startTimeStamp = string.Empty;
                    string editTime = string.Empty;

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Update Task Start Data

                                string sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_TASKS
                                                                 SET START_USER_KEY  = '{1}',
                                                                       START_TIMESTAMP = GETDATE(),
                                                                       EDITOR          = '{2}',
                                                                       EDIT_TIME       = GETDATE(),
                                                                       EDIT_TIMEZONE   = '{3}'
                                                                 WHERE START_USER_KEY IS NULL
                                                                 AND START_TIMESTAMP IS NULL
                                                                 AND EQUIPMENT_TASK_KEY = '{0}'", 
                                                                 taskKey.PreventSQLInjection(),
                                                                 startUserKey.PreventSQLInjection(),
                                                                 editor.PreventSQLInjection(),
                                                                 editTimezone.PreventSQLInjection());

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    #region Get Start TimeStamp And Update Time

                                    List<string> interestFields = new List<string>();

                                    interestFields.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP);
                                    interestFields.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME);

                                    Conditions conditions = new Conditions();

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY, GlobalEnums.DatabaseCompareOperator.Equal, taskKey);

                                    EMS_EQUIPMENT_TASKS_FIELDS tasksFields = new EMS_EQUIPMENT_TASKS_FIELDS();

                                    sqlString = DatabaseTable.BuildQuerySqlStatement(tasksFields, interestFields, conditions);

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
                    resDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP, startTimeStamp);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateTaskStartData Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateTaskCourseReceiveData(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && 
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY))
            {
                string taskCourseKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY].ToString();
                string receiveUserKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY].ToString();

                try
                {
                    string receiveTimeStamp = string.Empty;

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Update Task Course Receive Date

                                string sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_TASK_COURSES
                                                                   SET RECEIVE_USER_KEY = '{1}', RECEIVE_TIMESTAMP = GETDATE()
                                                                   WHERE RECEIVE_USER_KEY IS NULL
                                                                   AND RECEIVE_TIMESTAMP IS NULL
                                                                   AND EQUIPMENT_TASK_COURSE_KEY = '{0}'", 
                                                                   taskCourseKey, receiveUserKey);

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    #region Get Receive TimeStamp

                                    EMS_EQUIPMENT_TASK_COURSES_FIELDS taskCourseFields = new EMS_EQUIPMENT_TASK_COURSES_FIELDS();

                                    receiveTimeStamp = AllCommonFunctions.GetSpecifyTableColumnData(taskCourseFields, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY, taskCourseKey, transaction);

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
                    resDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP, receiveTimeStamp);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateTaskCourseReceiveData Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet ForwardTask(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME) &&
                reqDS.ExtendedProperties.Contains(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_NAME))
            {
                string taskName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME].ToString();
                string newTaskCourseKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string taskCourseKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY].ToString();
                string receiveDeptKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY].ToString();
                string receiveDeptName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME].ToString();
                string comments = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS].ToString();
                string sendUserKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY].ToString();
                string sendUserName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_NAME].ToString();

                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Update Task Course Handle Date

                                string sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_TASK_COURSES
                                                                   SET HANDLE_USER_KEY = '{1}', HANDLE_TIMESTAMP = GETDATE()
                                                                   WHERE HANDLE_USER_KEY IS NULL
                                                                   AND HANDLE_TIMESTAMP IS NULL
                                                                   AND EQUIPMENT_TASK_COURSE_KEY = '{0}'", 
                                                                   taskCourseKey.PreventSQLInjection(),
                                                                   sendUserKey.PreventSQLInjection());

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Insert Task Course Forward Data

                                sqlString = string.Format(@"INSERT INTO EMS_EQUIPMENT_TASK_COURSES
                                                              (EQUIPMENT_TASK_COURSE_KEY,
                                                               EQUIPMENT_TASK_KEY,
                                                               EQUIPMENT_TASK_STATE,
                                                               SEND_USER_KEY,
                                                               RECEIVE_DEPT_KEY,
                                                               REMARK,
                                                               COMMENTS,
                                                               EQUIPMENT_CHANGE_STATE_KEY,
                                                               EQUIPMENT_CHANGE_REASON_KEY)
                                                              SELECT '{1}' AS EQUIPMENT_TASK_COURSE_KEY,
                                                                     EQUIPMENT_TASK_KEY,
                                                                     EQUIPMENT_TASK_STATE,
                                                                     '{2}' AS SEND_USER_KEY,
                                                                     '{3}' AS RECEIVE_DEPT_KEY,
                                                                     '转交{4}处理' AS REMARK,
                                                                     '{5}' AS COMMENTS,
                                                                     EQUIPMENT_CHANGE_STATE_KEY,
                                                                     EQUIPMENT_CHANGE_REASON_KEY
                                                               FROM EMS_EQUIPMENT_TASK_COURSES
                                                               WHERE EQUIPMENT_TASK_COURSE_KEY = '{0}'",
                                                            taskCourseKey.PreventSQLInjection(),
                                                            newTaskCourseKey.PreventSQLInjection(),
                                                            sendUserKey.PreventSQLInjection(),
                                                            receiveDeptKey.PreventSQLInjection(),
                                                            receiveDeptName.PreventSQLInjection(),
                                                            comments.PreventSQLInjection());

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Send Notify Email To Specified Department

                                List<string> sendToList = new List<string>();

                                sendToList.Add(String.Empty); //Department E-Mail Address

                                List<string> copyToList = new List<string>();

                                string subject = string.Format("设备作业[{0}]待处理", taskName);
                                string content = string.Format("用户[{0}]转交设备作业:{1}，转交{2}处理!", sendUserName, taskName, receiveDeptName);

                                try
                                {
                                    AllCommonFunctions.LotusSendEMail(sendToList.ToArray(), copyToList.ToArray(), subject, content);
                                }
                                catch (Exception e)
                                {
                                    LogService.LogError(string.Format("Send Equipment Task[{0}] Email is failed! Fail Reason:{1}.", taskName, e.Message));
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
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("ForwardTask Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet ProcessingTask(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_EQP_TASK_COURSE_PARTS_FIELDS.DATABASE_TABLE_NAME) && 
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY) &&
                reqDS.ExtendedProperties.Contains(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.Contains(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.Contains(PARAMETERS.INPUT_EDIT_TIMEZONE) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_NOTES) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY) &&
                reqDS.ExtendedProperties.Contains(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_NAME))
            {
                DataTable taskPartsDataTable = reqDS.Tables[EMS_EQP_TASK_COURSE_PARTS_FIELDS.DATABASE_TABLE_NAME];
                string taskKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].ToString();
                string taskName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME].ToString();
                string equipmentKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                string newTaskCourseKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTimezone = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIMEZONE].ToString();
                string taskCourseKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY].ToString();
                string equipmentChangeStateKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                string equipmentChangeReasonKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].ToString();
                string notes = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_NOTES].ToString();
                string receiveDeptKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY].ToString();
                string receiveDeptName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME].ToString();
                string comments = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS].ToString();
                string sendUserKey = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY].ToString();
                string sendUserName = reqDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_NAME].ToString();
                string sqlString = string.Empty;
                string taskState = string.Empty;
                string remark = string.Empty;

                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(equipmentChangeStateKey))
                                {
                                    #region Validate Equipment State

                                    sqlString = string.Format(@"SELECT E.EQUIPMENT_KEY
                                                                FROM EMS_EQUIPMENTS E
                                                                WHERE EXISTS (SELECT ECS.EQUIPMENT_FROM_STATE_KEY
                                                                              FROM EMS_EQUIPMENT_CHANGE_STATES ECS
                                                                              WHERE ECS.EQUIPMENT_FROM_STATE_KEY = E.EQUIPMENT_STATE_KEY
                                                                              AND ECS.EQUIPMENT_CHANGE_STATE_KEY ='{1}')
                                                                AND E.EQUIPMENT_KEY = '{0}'", 
                                                                equipmentKey.PreventSQLInjection(),
                                                                equipmentChangeStateKey.PreventSQLInjection());

                                    object scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlString);

                                    if (scalar == null || scalar == DBNull.Value)
                                    {
                                        throw new Exception("设备当前状态与设备转变状态的起始状态不符合,不能处理该设备作业!");
                                    }

                                    #endregion

                                    #region Get Equipment Change Target State

                                    sqlString = string.Format(@"SELECT ECS.EQUIPMENT_TO_STATE_KEY
                                                                FROM EMS_EQUIPMENT_CHANGE_STATES ECS
                                                                WHERE ECS.EQUIPMENT_CHANGE_STATE_KEY = '{0}'",
                                                                equipmentChangeStateKey.PreventSQLInjection());

                                    scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlString);

                                    string equipmentToStateKey = string.Empty;

                                    if (scalar != null && scalar != DBNull.Value)
                                    {
                                        equipmentToStateKey = scalar.ToString();
                                    }
                                    else
                                    {
                                        throw new Exception("获取设备转变状态的目标状态失败!");
                                    }

                                    #endregion

                                    #region Change Equipment State

                                    sqlString = string.Format(@"UPDATE EMS_EQUIPMENTS
                                                                SET EQUIPMENT_STATE_KEY = '{1}',
                                                                    EQUIPMENT_CHANGE_STATE_KEY = '{2}',
                                                                    EDITOR = '{3}',
                                                                    EDIT_TIMEZONE = '{4}',
                                                                    EDIT_TIME = GETDATE()
                                                                WHERE EQUIPMENT_KEY = '{0}'",
                                                                equipmentKey.PreventSQLInjection(),
                                                                equipmentToStateKey.PreventSQLInjection(),
                                                                equipmentChangeStateKey.PreventSQLInjection(),
                                                                editor.PreventSQLInjection(),
                                                                editTimezone.PreventSQLInjection());

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }

                                    #endregion

                                    #region Update Equipment State Log

                                    sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_LOG SET END_TIMESTAMP = GETDATE()
                                                                WHERE EQUIPMENT_KEY = '{0}' AND END_TIMESTAMP IS NULL",
                                                                equipmentKey.PreventSQLInjection());

                                    db.ExecuteNonQuery(transaction, CommandType.Text, sqlString);

                                    #endregion

                                    #region Insert Equipment State Log

                                    sqlString = string.Format(@"INSERT INTO EMS_EQUIPMENT_LOG
                                                                  (EQUIPMENT_LOG_KEY,
                                                                   EQUIPMENT_KEY,
                                                                   EQUIPMENT_STATE_KEY,
                                                                   EQUIPMENT_CHANGE_STATE_KEY,
                                                                   EQUIPMENT_CHANGE_REASON_KEY,
                                                                   START_TIMESTAMP)
                                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE())",
                                                                UtilHelper.GenerateNewKey(0),
                                                                equipmentKey.PreventSQLInjection(),
                                                                equipmentToStateKey.PreventSQLInjection(),
                                                                equipmentChangeStateKey.PreventSQLInjection(),
                                                                equipmentChangeReasonKey.PreventSQLInjection());

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }

                                    #endregion

                                    //Equipment State 420EFAD0-CA7E-452b-8400-3F3863736152-000 is IDLE
                                    if (equipmentToStateKey == "420EFAD0-CA7E-452b-8400-3F3863736152-000") //Task is Finished
                                    {
                                        taskState = "已处理";
                                        remark = "设备作业完成";
                                    }
                                    else
                                    {
                                        taskState = "待处理";
                                        remark = string.Format("提交{0}处理", receiveDeptName);
                                    }
                                }
                                else
                                {
                                    taskState = "已处理";
                                    remark = "设备作业完成";
                                }

                                #region Update Task Course Handle Date

                                sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_TASK_COURSES
                                                            SET HANDLE_USER_KEY  = '{1}',
                                                                HANDLE_TIMESTAMP = GETDATE(),
                                                                HANDLE_NOTES     = '{2}'
                                                            WHERE HANDLE_USER_KEY IS NULL
                                                            AND HANDLE_TIMESTAMP IS NULL
                                                            AND EQUIPMENT_TASK_COURSE_KEY = '{0}'",
                                                            taskCourseKey.PreventSQLInjection(),
                                                            sendUserKey.PreventSQLInjection(),
                                                            notes.PreventSQLInjection());

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Insert Task Course Process Data

                                if (taskState == "已处理")
                                {
                                    sqlString = string.Format(@"INSERT INTO EMS_EQUIPMENT_TASK_COURSES
                                                              (EQUIPMENT_TASK_COURSE_KEY,
                                                               EQUIPMENT_TASK_KEY,
                                                               EQUIPMENT_TASK_STATE,
                                                               SEND_USER_KEY,
                                                               RECEIVE_DEPT_KEY,
                                                               HANDLE_USER_KEY,
                                                               HANDLE_TIMESTAMP,
                                                               REMARK,
                                                               COMMENTS,
                                                               EQUIPMENT_CHANGE_STATE_KEY,
                                                               EQUIPMENT_CHANGE_REASON_KEY)
                                                            VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', GETDATE(), '{6}', '{7}', '{8}', '{9}')",
                                                            newTaskCourseKey.PreventSQLInjection(),
                                                            taskKey.PreventSQLInjection(),
                                                            taskState.PreventSQLInjection(),
                                                            sendUserKey.PreventSQLInjection(),
                                                            receiveDeptKey.PreventSQLInjection(),
                                                            sendUserKey.PreventSQLInjection(),
                                                            remark.PreventSQLInjection(),
                                                            comments.PreventSQLInjection(),
                                                            equipmentChangeStateKey.PreventSQLInjection(),
                                                            equipmentChangeReasonKey.PreventSQLInjection());
                                }
                                else
                                {
                                    sqlString = string.Format(@"INSERT INTO EMS_EQUIPMENT_TASK_COURSES
                                                              (EQUIPMENT_TASK_COURSE_KEY,
                                                               EQUIPMENT_TASK_KEY,
                                                               EQUIPMENT_TASK_STATE,
                                                               SEND_USER_KEY,
                                                               RECEIVE_DEPT_KEY,
                                                               REMARK,
                                                               COMMENTS,
                                                               EQUIPMENT_CHANGE_STATE_KEY,
                                                               EQUIPMENT_CHANGE_REASON_KEY)
                                                            VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                                            newTaskCourseKey.PreventSQLInjection(),
                                                            taskKey.PreventSQLInjection(),
                                                            taskState.PreventSQLInjection(),
                                                            sendUserKey.PreventSQLInjection(),
                                                            receiveDeptKey.PreventSQLInjection(),
                                                            remark.PreventSQLInjection(),
                                                            comments.PreventSQLInjection(),
                                                            equipmentChangeStateKey.PreventSQLInjection(),
                                                            equipmentChangeReasonKey.PreventSQLInjection());
                                }

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Update Task Course Parts Data

                                if (taskPartsDataTable.Rows.Count > 0)
                                {
                                    DbCommand insertCommand = connection.CreateCommand();

                                    insertCommand.CommandType = CommandType.Text;
                                    insertCommand.CommandText = @"INSERT INTO EMS_EQP_TASK_COURSE_PARTS(EQP_TASK_COURSE_PART_KEY, EQP_TASK_COURSE_KEY, EQP_PART_KEY, QUANTITY)
                                                                  VALUES(@P1, @P2, @P3, @P4)";

                                    db.AddInParameter(insertCommand, "P1", DbType.String, EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_PART_KEY, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P2", DbType.String, EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_KEY, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P3", DbType.String, EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_PART_KEY, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P4", DbType.Decimal, EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_QUANTITY, DataRowVersion.Current);

                                    DbCommand deleteCommand = connection.CreateCommand();

                                    deleteCommand.CommandType = CommandType.Text;
                                    deleteCommand.CommandText = @"DELETE EMS_EQP_TASK_COURSE_PARTS WHERE EQP_TASK_COURSE_PART_KEY = :P1";

                                    db.AddInParameter(deleteCommand, "P1", DbType.String, EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_PART_KEY, DataRowVersion.Current);

                                    if (db.UpdateDataSet(reqDS, EMS_EQP_TASK_COURSE_PARTS_FIELDS.DATABASE_TABLE_NAME, insertCommand, null, deleteCommand, transaction) <= 0)
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }
                                }

                                #endregion

                                #region Update Task Handle Data

                                if (taskState == "已处理")
                                {
                                    sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_TASKS
                                                                   SET EQUIPMENT_TASK_STATE        = '{1}',
                                                                       COMPLETE_USER_KEY           = '{2}',
                                                                       COMPLETE_TIMESTAMP          = GETDATE(),
                                                                       EQUIPMENT_CHANGE_STATE_KEY  = '{3}',
                                                                       EQUIPMENT_CHANGE_REASON_KEY = '{4}',
                                                                       EDITOR                      = '{5}',
                                                                       EDIT_TIME                   = GETDATE(),
                                                                       EDIT_TIMEZONE               = '{6}'
                                                                 WHERE EQUIPMENT_TASK_KEY = '{0}'",
                                                                 taskKey.PreventSQLInjection(),
                                                                 taskState.PreventSQLInjection(),
                                                                 sendUserKey.PreventSQLInjection(),
                                                                 equipmentChangeStateKey.PreventSQLInjection(),
                                                                 equipmentChangeReasonKey.PreventSQLInjection(),
                                                                 editor.PreventSQLInjection(),
                                                                 editTimezone.PreventSQLInjection());
                                }
                                else
                                {
                                    sqlString = string.Format(@"UPDATE EMS_EQUIPMENT_TASKS
                                                                SET EQUIPMENT_TASK_STATE        = '{1}',
                                                                       EQUIPMENT_CHANGE_STATE_KEY  = '{2}',
                                                                       EQUIPMENT_CHANGE_REASON_KEY = '{3}',
                                                                       EDITOR                      = '{4}',
                                                                       EDIT_TIME                   = GETDATE(),
                                                                       EDIT_TIMEZONE               = '{5}'
                                                                WHERE EQUIPMENT_TASK_KEY = '{0}'",
                                                                taskKey.PreventSQLInjection(),
                                                                taskState.PreventSQLInjection(),
                                                                equipmentChangeStateKey.PreventSQLInjection(),
                                                                equipmentChangeReasonKey.PreventSQLInjection(),
                                                                editor.PreventSQLInjection(),
                                                                editTimezone.PreventSQLInjection());
                                }

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Send Notify Email To Specified Department

                                List<string> sendToList = new List<string>();

                                sendToList.Add(String.Empty); //Department E-Mail Address

                                List<string> copyToList = new List<string>();

                                string subject = string.Empty;
                                string content = string.Empty;

                                if (taskState == "已处理")
                                {
                                    subject = string.Format("设备作业[{0}]已处理", taskName);
                                    content = string.Format("用户[{0}]完成设备作业:{1}!", sendUserName, taskName);
                                }
                                else
                                {
                                    subject = string.Format("设备作业[{0}]待处理", taskName);
                                    content = string.Format("用户[{0}]提交设备作业:{1}，提交{2}处理!", sendUserName, taskName, receiveDeptName);
                                }

                                try
                                {
                                    AllCommonFunctions.LotusSendEMail(sendToList.ToArray(), copyToList.ToArray(), subject, content);
                                }
                                catch (Exception e)
                                {
                                    LogService.LogError(string.Format("Send Equipment Task[{0}] Email is failed! Fail Reason:{1}.", taskName, e.Message));
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
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("ProcessingTask Error: " + ex.Message);
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
