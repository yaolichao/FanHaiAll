using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Constants;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using System.Collections;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    public class EquipmentCheckListJobEntity : SEntity
    {
        #region Constructor

        public EquipmentCheckListJobEntity()
        {

        }

        #endregion

        #region Public Properties

        private string checkListJobKey = string.Empty;

        public string CheckListJobKey
        {
            set
            {
                checkListJobKey = value;
            }
            get
            {
                return checkListJobKey;
            }
        }

        private string checkListJobName = string.Empty;

        public string CheckListJobName
        {
            set
            {
                checkListJobName = value;
            }
            get
            {
                return checkListJobName;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        private string checkListJobState = string.Empty;

        public string CheckListJobState
        {
            set
            {
                checkListJobState = value;
            }
            get
            {
                return checkListJobState;
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

        private string checkListKey = string.Empty;

        public string CheckListKey
        {
            set
            {
                checkListKey = value;

                ValidateDirtyList(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY, value);
            }
            get
            {
                return checkListKey;
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

        #endregion

        #region Public Override Methods

        public override bool Insert()
        {
            DataSet reqDS = new DataSet();

            DataTable checkListJobsDataTable = EMS_CHECKLIST_JOBS_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, checkListJobKey },
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME, checkListJobName},
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION, description },
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE, checkListJobState },
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey },
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY, checkListKey },

                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref checkListJobsDataTable, dataRow);

            checkListJobsDataTable.AcceptChanges();

            reqDS.Tables.Add(checkListJobsDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "InsertCheckListJob", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                CheckListJobName = resDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME) ? resDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME].ToString() : string.Empty;
                CreateTimeStamp = resDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP) ? resDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].ToString() : string.Empty;
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
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListJobKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            this.DirtyList.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME, string.Empty, string.Empty));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable checkListJobsDataTable = EMS_CHECKLIST_JOBS_FIELDS.CreateDataTable(false);

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref checkListJobsDataTable, this.DirtyList);

            checkListJobsDataTable.AcceptChanges();

            reqDS.Tables.Add(checkListJobsDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "UpdateCheckListJob", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                EditTime = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_EDIT_TIME) ? resDS.ExtendedProperties[PARAMETERS.OUTPUT_EDIT_TIME].ToString() : string.Empty;

                return true;
            }
            else
            {
                MessageService.ShowError(msg);

                return false;
            }
        }

        public override bool Delete()
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListJobKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "DeleteCheckListJob", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return true;
            }
            else
            {
                MessageService.ShowError(msg);

                return false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Equipment Change State Data
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-03 14:48:24
        public DataTable LoadEquipmentChangeStateData(out string msg)
        {
            #region Call Remoting Interface

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                resDS = serverFactory.CreateIEquipmentChangeStates().GetEquipmentChangeStates(reqDS);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            DataTable dataTable = resDS.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                //Equipment State 420EFAD0-CA7E-452b-8400-3F3863736152-000 is IDLE
                //Equipment State 3B78582E-136D-46d3-A7FE-D2EA3B5DC645-000 is Running
                if ((dataRow[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].ToString() != "420EFAD0-CA7E-452b-8400-3F3863736152-000") ||
                    (dataRow[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString() == "3B78582E-136D-46d3-A7FE-D2EA3B5DC645-000"))
                {
                    dataRow.Delete();
                }
            }

            dataTable.AcceptChanges();

            return dataTable;

            #endregion
        }

        /// <summary>
        /// Load Equipment Change State Data
        /// </summary>
        /// <param name="equipmentChangeStateKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-25 17:56:52
        public DataTable LoadEquipmentChangeStateData(string equipmentChangeStateKey, out string msg)
        {
            #region Call Remoting Interface

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                resDS = serverFactory.CreateIEquipmentChangeStates().GetEquipmentChangeStates(equipmentChangeStateKey);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            return resDS.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME];;

            #endregion
        }

        /// <summary>
        /// Load Equipment Change Reasons Data
        /// </summary>
        /// <param name="equipmentChangeStateKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-03 14:58:11
        public DataTable LoadEquipmentChangeReasonsData(string equipmentChangeStateKey, out string msg)
        {
            #region Call Remoting Interface

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            if (!string.IsNullOrEmpty(equipmentChangeStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                inputKey = equipmentChangeStateKey;

                inputParamDataTable.Rows.Add(new object[] { inputKey, inputEditor, inputEditTime });

                inputParamDataTable.AcceptChanges();
                
                reqDS.Tables.Add(inputParamDataTable);
            }

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                
                resDS = serverFactory.CreateIEquipmentChangeReasons().GetEquipmentChangeReasons(reqDS);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            return resDS.Tables[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME];

            #endregion
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-20 13:07:29
        public DataTable LoadEquipmentLocationData(out string msg)
        {
            #region Call Remoting Interface

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                resDS = serverFactory.CreateILocationEngine().GetAllLoactions(reqDS);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            return resDS.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];

            #endregion
        }

        /// <summary>
        /// Load Equipment Group Data
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-20 13:11:03
        public DataTable LoadEquipmentGroupData(out string msg)
        {
            #region Call Remoting Interface

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                resDS = serverFactory.CreateIEquipmentGroups().GetEquipmentGroups(reqDS);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            return resDS.Tables[EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME];

            #endregion
        }

        /// <summary>
        /// Load Equipment Data
        /// </summary>
        /// <param name="equipmentLocationKey"></param>
        /// <param name="equipmentGroupKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-20 13:14:58
        public DataTable LoadEquipmentData(string equipmentLocationKey, string equipmentGroupKey, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, equipmentLocationKey);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, equipmentGroupKey);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.Equipments, FanHai.Hemera.Modules.EMS", "GetEquipments", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Load Users Data
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-05 10:56:49
        public DataTable LoadUsersData(out string msg)
        {
            #region Call Remoting Interface

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(new Hashtable());
            dataTable.TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;

            reqDS.Tables.Add(dataTable);

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                resDS = serverFactory.CreateIUserEngine().GetUserInfo(reqDS);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            return resDS.Tables[0];

            #endregion
        }

        /// <summary>
        /// Load Depts Data
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-17 14:52:36
        public DataTable LoadDeptsData(out string msg)
        {
            //TODO: Don't implement use simulate data
            msg = string.Empty;

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("DEPT_KEY");
            dataTable.Columns.Add("DEPT_NAME");

            dataTable.Rows.Add("54d1e6f7-4570-4af7-9766-af75dbcb3040-000", "设备部门");
            dataTable.Rows.Add("6B9B393C-F51C-4e10-B1AC-BDBDB8100E5B-000", "工艺部门");
            dataTable.Rows.Add("F7E49AF9-2AEC-4bdf-B617-34CA43FC2C0E-000", "制造部门");
            dataTable.Rows.Add("47E3704B-EC55-47a4-826A-1C42E744B32C-000", "生产部门");

            return dataTable;
        }

        /// <summary>
        /// Load Check List Jobs Data
        /// </summary>
        /// <param name="equipmentKey"></param>
        /// <param name="checkListJobKey"></param>
        /// <param name="checkListJobName"></param>
        /// <param name="checkListJobState"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-21 08:30:09
        public DataTable LoadCheckListJobsData(string equipmentKey, string checkListJobKey, string checkListJobName, string checkListJobState, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, checkListJobKey);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME, checkListJobName);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE, checkListJobState);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "GetCheckListJobs", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Load Check List Job Data
        /// </summary>
        /// <param name="checkListJobKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-26 14:44:49
        public DataTable LoadCheckListJobData(string checkListJobKey, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListJobKey);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "GetCheckListJobData", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_CHECKLIST_JOB_DATA_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Start Check List Job
        /// </summary>
        /// <param name="checkListJobKey"></param>
        /// <param name="checkListKey"></param>
        /// <param name="editor"></param>
        /// <param name="editTimeZone"></param>
        /// <param name="editTime"></param>
        /// <param name="startTimeStamp"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-26 17:45:48
        public void StartCheckListJob(string checkListJobKey, string checkListKey, string editor, string editTimeZone, ref string editTime, out string startTimeStamp, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, checkListJobKey);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY, checkListKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIMEZONE, editTimeZone);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, editTime);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "StartCheckListJob", reqDS, out msg);

            startTimeStamp = string.Empty;
            editTime = string.Empty;

            if (string.IsNullOrEmpty(msg))
            {
                if (resDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP))
                {
                    startTimeStamp = resDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].ToString();
                }

                if (resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_EDIT_TIME))
                {
                    editTime = resDS.ExtendedProperties[PARAMETERS.OUTPUT_EDIT_TIME].ToString();
                }
            }
        }

        /// <summary>
        /// Save Check List Job Data
        /// </summary>
        /// <param name="checkListJobKey"></param>
        /// <param name="checkItemKey"></param>
        /// <param name="checkItemValue"></param>
        /// <param name="remark"></param>
        /// <param name="completeTimeStamp"></param>
        /// <param name="msg"></param>
        /// Owner:Andy Gao 2011-07-26 19:20:19
        public void SaveCheckListJobData(string checkListJobKey, string checkItemKey, string checkItemValue, string remark, out string completeTimeStamp, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKLIST_JOB_KEY, checkListJobKey);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_KEY, checkItemKey);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE, checkItemValue);
            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK, remark);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "SaveCheckListJobData", reqDS, out msg);

            completeTimeStamp = string.Empty;

            if (string.IsNullOrEmpty(msg))
            {
                if (resDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP))
                {
                    completeTimeStamp = resDS.ExtendedProperties[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP].ToString();
                }
            }
        }

        /// <summary>
        /// Complete Check List Job
        /// </summary>
        /// <param name="checkListJobKey"></param>
        /// <param name="editor"></param>
        /// <param name="editTimeZone"></param>
        /// <param name="editTime"></param>
        /// <param name="completeTimeStamp"></param>
        /// <param name="msg"></param>
        /// Owner:Andy Gao 2011-07-26 20:31:00
        public void CompleteCheckListJob(string checkListJobKey, string editor, string editTimeZone, ref string editTime, out string completeTimeStamp, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, checkListJobKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIMEZONE, editTimeZone);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, editTime);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListJobsEngine, FanHai.Hemera.Modules.EMS", "CompleteCheckListJob", reqDS, out msg);

            completeTimeStamp = string.Empty;
            editTime = string.Empty;

            if (string.IsNullOrEmpty(msg))
            {
                if (resDS.ExtendedProperties.ContainsKey(EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP))
                {
                    completeTimeStamp = resDS.ExtendedProperties[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].ToString();
                }

                if (resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_EDIT_TIME))
                {
                    editTime = resDS.ExtendedProperties[PARAMETERS.OUTPUT_EDIT_TIME].ToString();
                }
            }
        }

        #endregion
    }
}
