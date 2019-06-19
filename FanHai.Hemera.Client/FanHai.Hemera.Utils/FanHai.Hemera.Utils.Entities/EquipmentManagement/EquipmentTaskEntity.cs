using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Constants;
using System.Data;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    public class EquipmentTaskEntity : SEntity
    {
        #region Constructor

        public EquipmentTaskEntity()
        {

        }

        #endregion

        #region Public Properties

        private string taskKey = string.Empty;

        public string TaskKey
        {
            set
            {
                taskKey = value;
            }
            get
            {
                return taskKey;
            }
        }

        private string taskName = string.Empty;

        public string TaskName
        {
            set
            {
                taskName = value;
            }
            get
            {
                return taskName;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        private string equipmentKey = string.Empty;

        public string EquipmentKey
        {
            set
            {
                equipmentKey = value;
            }
            get
            {
                return equipmentKey;
            }
        }

        private string taskState = string.Empty;

        public string TaskState
        {
            set
            {
                taskState = value;
            }
            get
            {
                return taskState;
            }
        }

        private string createUserKey = string.Empty;

        public string CreateUserKey
        {
            set
            {
                createUserKey = value;
            }
            get
            {
                return createUserKey;
            }
        }

        private string createTimeStamp = string.Empty;

        public string CreateTimeStamp
        {
            set
            {
                createTimeStamp = value;
            }
            get
            {
                return createTimeStamp;
            }
        }

        private string equipmentChangeStateKey = string.Empty;

        public string EquipmentChangeStateKey
        {
            set
            {
                equipmentChangeStateKey = value;

                ValidateDirtyList(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, value);
            }
            get
            {
                return equipmentChangeStateKey;
            }
        }

        private string equipmentChangeReasonKey = string.Empty;

        public string EquipmentChangeReasonKey
        {
            set
            {
                equipmentChangeReasonKey = value;

                ValidateDirtyList(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, value);
            }
            get
            {
                return equipmentChangeReasonKey;
            }
        }

        private string receiveDeptKey = string.Empty;

        public string ReceiveDeptKey
        {
            set
            {
                receiveDeptKey = value;
            }
            get
            {
                return receiveDeptKey;
            }
        }

        private string receiveDeptName = string.Empty;

        public string ReceiveDeptName
        {
            set
            {
                receiveDeptName = value;
            }
            get
            {
                return receiveDeptName;
            }
        }

        private string comments = string.Empty;

        public string Comments
        {
            set
            {
                comments = value;
            }
            get
            {
                return comments;
            }
        }

        #endregion

        #region Public Override Methods

        public override bool Insert()
        {
            DataSet reqDS = new DataSet();

            DataTable tasksDataTable = EMS_EQUIPMENT_TASKS_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY, taskKey },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME, taskName},
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION, description },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE, taskState },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY, createUserKey },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateKey },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, equipmentChangeReasonKey },

                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref tasksDataTable, dataRow);

            tasksDataTable.AcceptChanges();

            reqDS.Tables.Add(tasksDataTable);

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY, CommonUtils.GenerateNewKey(0));
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY, receiveDeptKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME, receiveDeptName);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS, comments);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "InsertTask", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                TaskName = resDS.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME) ? resDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME].ToString() : string.Empty;
                CreateTimeStamp = resDS.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP) ? resDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].ToString() : string.Empty;
                CreateTime = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_EDIT_TIME) ? resDS.ExtendedProperties[PARAMETERS.OUTPUT_EDIT_TIME].ToString() : string.Empty;

                return true;
            }
            else
            {
                MessageService.ShowError(msg);

                return false;
            }
        }

        public override bool Update()
        {
            return false;
        }

        public override bool Delete()
        {
            return false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Task Data
        /// </summary>
        /// <param name="equipmentKey"></param>
        /// <param name="taskKey"></param>
        /// <param name="taskName"></param>
        /// <param name="taskState"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-18 14:39:51
        public DataTable LoadTaskData(string equipmentKey, string taskKey, string taskName, string taskState, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY, taskKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME, taskName);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE, taskState);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "GetTask", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Load Task Course Data
        /// </summary>
        /// <param name="taskKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-25 14:17:22
        public DataTable LoadTaskCourseData(string taskKey, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, taskKey);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "GetTaskCourse", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_EQUIPMENT_TASK_COURSES_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Load Task Course Part Data
        /// </summary>
        /// <param name="taskCourseKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-25 14:41:41
        public DataTable LoadTaskCoursePartData(string taskCourseKey, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, taskCourseKey);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "GetTaskCoursePart", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_EQP_TASK_COURSE_PARTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update Task Start Data
        /// </summary>
        /// <param name="taskKey"></param>
        /// <param name="startUserKey"></param>
        /// <param name="editor"></param>
        /// <param name="editTimezone"></param>
        /// <param name="startTimeStamp"></param>
        /// <param name="editTime"></param>
        /// <param name="msg"></param>
        /// Owner:Andy Gao 2011-08-29 10:22:30
        public void UpdateTaskStartData(string taskKey, string startUserKey, string editor, string editTimezone, out string startTimeStamp, out string editTime, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY, taskKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY, startUserKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIMEZONE, editTimezone);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "UpdateTaskStartData", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                startTimeStamp = resDS.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP) ? resDS.ExtendedProperties[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].ToString() : string.Empty;
                editTime = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_EDIT_TIME) ? resDS.ExtendedProperties[PARAMETERS.OUTPUT_EDIT_TIME].ToString() : string.Empty;
            }
            else
            {
                startTimeStamp = string.Empty;
                editTime = string.Empty;
            }
        }

        /// <summary>
        /// Update Task Course Receive Data
        /// </summary>
        /// <param name="taskCourseKey"></param>
        /// <param name="receiveUserKey"></param>
        /// <param name="msg"></param>
        /// Owner:Andy Gao 2011-08-29 10:35:07
        public void UpdateTaskCourseReceiveData(string taskCourseKey, string receiveUserKey, out string receiveTimeStamp, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY, taskCourseKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY, receiveUserKey);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "UpdateTaskCourseReceiveData", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                receiveTimeStamp = resDS.ExtendedProperties.ContainsKey(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP) ? resDS.ExtendedProperties[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP].ToString() : string.Empty;
            }
            else
            {
                receiveTimeStamp = string.Empty;
            }
        }

        /// <summary>
        /// Forward Task Data
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="taskCourseKey"></param>
        /// <param name="receiveDeptKey"></param>
        /// <param name="receiveDeptName"></param>
        /// <param name="comments"></param>
        /// <param name="userKey"></param>
        /// <param name="userName"></param>
        /// <param name="msg"></param>
        /// Owner:Andy Gao 2011-08-29 15:39:08
        public void ForwardTaskData(string taskName, string taskCourseKey, string receiveDeptKey, string receiveDeptName, string comments, string userKey, string userName, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME, taskName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, CommonUtils.GenerateNewKey(0));
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY, taskCourseKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY, receiveDeptKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME, receiveDeptName);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS, comments);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY, userKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_NAME, userName);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "ForwardTask", reqDS, out msg);
        }

        /// <summary>
        /// Processing Task Data
        /// </summary>
        /// <param name="taskKey"></param>
        /// <param name="taskName"></param>
        /// <param name="equipmentKey"></param>
        /// <param name="taskCourseKey"></param>
        /// <param name="equipmentChangeStateKey"></param>
        /// <param name="equipmentChangeReasonKey"></param>
        /// <param name="notes"></param>
        /// <param name="receiveDeptKey"></param>
        /// <param name="receiveDeptName"></param>
        /// <param name="comments"></param>
        /// <param name="userKey"></param>
        /// <param name="userName"></param>
        /// <param name="userTimezone"></param>
        /// <param name="taskParts"></param>
        /// <param name="msg"></param>
        /// Owner:Andy Gao 2011-08-30 10:22:00
        public void ProcessingTaskData(string taskKey, string taskName, string equipmentKey, string taskCourseKey, string equipmentChangeStateKey, string equipmentChangeReasonKey,
            string notes, string receiveDeptKey, string receiveDeptName, string comments, string userKey, string userName, string userTimezone, DataTable partsDataTable, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY, taskKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME, taskName);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, CommonUtils.GenerateNewKey(0));
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, userName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIMEZONE, userTimezone);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY, taskCourseKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, equipmentChangeReasonKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_NOTES, notes);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY, receiveDeptKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_NAME, receiveDeptName);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS, comments);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY, userKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_NAME, userName);

            reqDS.Tables.Add(partsDataTable);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentTasksEngine, FanHai.Hemera.Modules.EMS", "ProcessingTask", reqDS, out msg);
        }

        #endregion
    }
}
