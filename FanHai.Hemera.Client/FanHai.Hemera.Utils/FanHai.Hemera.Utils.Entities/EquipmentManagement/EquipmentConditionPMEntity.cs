using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Constants;
using System.Data;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    public class EquipmentConditionPMEntity : SEntity
    {
        #region Constructor

        public EquipmentConditionPMEntity()
        {

        }

        #endregion

        #region Public Properties

        private string conditionKey = string.Empty;

        public string ConditionKey
        {
            set
            {
                conditionKey = value;
            }
            get
            {
                return conditionKey;
            }
        }

        private string conditionName = string.Empty;

        public string ConditionName
        {
            set
            {
                conditionName = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME, value);
            }
            get
            {
                return conditionName;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_DESCRIPTION, value);
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

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_CHECKLIST_KEY, value);
            }
            get
            {
                return checkListKey;
            }
        }

        private string counterType = string.Empty;

        public string CounterType
        {
            set
            {
                counterType = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TYPE, value);
            }
            get
            {
                return counterType;
            }
        }

        private string counterWarnning = "0";

        public string CounterWarnning
        {
            set
            {
                counterWarnning = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_WARNNING, value);
            }
            get
            {
                return counterWarnning;
            }
        }

        private string counterTarget = "0";

        public string CounterTarget
        {
            set
            {
                counterTarget = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TARGET, value);
            }
            get
            {
                return counterTarget;
            }
        }

        private string counterTotal = "0";

        public string CounterTotal
        {
            set
            {
                counterTotal = value;
            }
            get
            {
                return counterTotal;
            }
        }

        private string notifyUserKey = string.Empty;

        public string NotifyUserKey
        {
            set
            {
                notifyUserKey = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_KEY, value);
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

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_KEY, value);
            }
            get
            {
                return notifyCCUserKey;
            }
        }

        private string equipmentChangeStateKey = string.Empty;

        public string EquipmentChangeStateKey
        {
            set
            {
                equipmentChangeStateKey = value;

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, value);
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

                ValidateDirtyList(EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, value);
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

            DataTable conditionPMDataTable = EMS_PM_CONDITION_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY, conditionKey },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME, conditionName},
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_CHECKLIST_KEY, checkListKey },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_DESCRIPTION, description },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TYPE, counterType },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_WARNNING, counterWarnning },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TARGET, counterTarget },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TOTAL, counterTotal },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_KEY, notifyUserKey },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_KEY, notifyCCUserKey },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateKey },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, equipmentChangeReasonKey },

                                                            { EMS_PM_CONDITION_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_PM_CONDITION_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref conditionPMDataTable, dataRow);

            conditionPMDataTable.AcceptChanges();

            reqDS.Tables.Add(conditionPMDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentConditionPMEngine, FanHai.Hemera.Modules.EMS", "InsertConditionPM", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, conditionKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            this.DirtyList.Add(EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME, string.Empty, string.Empty));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable conditionPMDataTable = EMS_PM_CONDITION_FIELDS.CreateDataTable(false);

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref conditionPMDataTable, this.DirtyList);

            conditionPMDataTable.AcceptChanges();

            reqDS.Tables.Add(conditionPMDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentConditionPMEngine, FanHai.Hemera.Modules.EMS", "UpdateConditionPM", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, conditionKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentConditionPMEngine, FanHai.Hemera.Modules.EMS", "DeleteConditionPM", reqDS, out msg);

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
        /// Load Condition PM Data
        /// </summary>
        /// <param name="equipmentKey"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-08 16:17:30
        public DataTable LoadConditionPMData(string equipmentKey, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentConditionPMEngine, FanHai.Hemera.Modules.EMS", "GetConditionPM", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
