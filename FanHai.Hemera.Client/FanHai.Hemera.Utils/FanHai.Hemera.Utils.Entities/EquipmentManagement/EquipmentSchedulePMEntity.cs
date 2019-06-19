using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    public class EquipmentSchedulePMEntity : SEntity
    {
        #region Constructor

        public EquipmentSchedulePMEntity()
        {

        }

        #endregion

        #region Public Properties

        private string scheduleKey = string.Empty;

        public string ScheduleKey
        {
            set
            {
                scheduleKey = value;
            }
            get
            {
                return scheduleKey;
            }
        }

        private string scheduleName = string.Empty;

        public string ScheduleName
        {
            set
            {
                scheduleName = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME, value);
            }
            get
            {
                return scheduleName;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_DESCRIPTION, value);
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

        private string checkListKey = string.Empty;

        public string CheckListKey
        {
            set
            {
                checkListKey = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY, value);
            }
            get
            {
                return checkListKey;
            }
        }

        private string frequence = "0";

        public string Frequence
        {
            set
            {
                frequence = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE, value);
            }
            get
            {
                return frequence;
            }
        }

        private string frequenceUnit = string.Empty;

        public string FrequenceUnit
        {
            set
            {
                frequenceUnit = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE_UNIT, value);
            }
            get
            {
                return frequenceUnit;
            }
        }

        private string notifyUserKey = string.Empty;

        public string NotifyUserKey
        {
            set
            {
                notifyUserKey = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY, value);
            }
            get
            {
                return notifyUserKey;
            }
        }

        private string notifyCCUserKey = string.Empty;

        public string NotifyCCUserKey
        {
            set
            {
                notifyCCUserKey = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY, value);
            }
            get
            {
                return notifyCCUserKey;
            }
        }

        private string notifyAdvancedTime = "0";

        public string NotifyAdvancedTime
        {
            set
            {
                notifyAdvancedTime = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_ADVANCED_TIME, value);
            }
            get
            {
                return notifyAdvancedTime;
            }
        }

        private string nextEventTime = string.Empty;

        public string NextEventTime
        {
            set
            {
                nextEventTime = value;
            }
            get
            {
                return nextEventTime;
            }
        }

        private string isBaseActualFinishTime = "0";

        public string IsBaseActualFinishTime
        {
            set
            {
                isBaseActualFinishTime = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_BASE_ACTUAL_FINISH_TIME, value);
            }
            get
            {
                return isBaseActualFinishTime;
            }
        }

        private string equipmentChangeStateKey = string.Empty;

        public string EquipmentChangeStateKey
        {
            set
            {
                equipmentChangeStateKey = value;

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, value);
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

                ValidateDirtyList(EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, value);
            }
            get
            {
                return equipmentChangeReasonKey;
            }
        }

        #endregion

        #region Public Override Methods

        public override bool Insert()
        {
            DataSet reqDS = new DataSet();

            DataTable schedulePMDataTable = EMS_PM_SCHEDULE_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY, scheduleKey },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME, scheduleName},
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY, checkListKey },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_DESCRIPTION, description },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE, frequence },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE_UNIT, frequenceUnit },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY, notifyUserKey },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY, notifyCCUserKey },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_ADVANCED_TIME,notifyAdvancedTime },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME, string.Empty },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_BASE_ACTUAL_FINISH_TIME, isBaseActualFinishTime },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateKey },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, equipmentChangeReasonKey },

                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_PM_SCHEDULE_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref schedulePMDataTable, dataRow);

            schedulePMDataTable.AcceptChanges();

            reqDS.Tables.Add(schedulePMDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentSchedulePMEngine, FanHai.Hemera.Modules.EMS", "InsertSchedulePM", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                NextEventTime = resDS.ExtendedProperties.ContainsKey(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME) ? resDS.ExtendedProperties[EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME].ToString() : string.Empty;
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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, scheduleKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            this.DirtyList.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME, string.Empty, string.Empty));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable schedulePMDataTable = EMS_PM_SCHEDULE_FIELDS.CreateDataTable(false);

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref schedulePMDataTable, this.DirtyList);

            schedulePMDataTable.AcceptChanges();

            reqDS.Tables.Add(schedulePMDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentSchedulePMEngine, FanHai.Hemera.Modules.EMS", "UpdateSchedulePM", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                NextEventTime = resDS.ExtendedProperties.ContainsKey(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME) ? resDS.ExtendedProperties[EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME].ToString() : string.Empty;
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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, scheduleKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentSchedulePMEngine, FanHai.Hemera.Modules.EMS", "DeleteSchedulePM", reqDS, out msg);

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
        /// Load Schedule PM Data
        /// </summary>
        /// <param name="equipmentKey"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-04 10:09:20
        public DataTable LoadSchedulePMData(string equipmentKey, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentSchedulePMEngine, FanHai.Hemera.Modules.EMS", "GetSchedulePM", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_PM_SCHEDULE_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
