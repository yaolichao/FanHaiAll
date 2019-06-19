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
    public class EquipmentCheckListEntity : SEntity
    {
        #region Constructor

        public EquipmentCheckListEntity()
        {

        }

        #endregion

        #region Public Properties

        private string checkListKey = string.Empty;

        public string CheckListKey
        {
            set
            {
                checkListKey = value;
            }
            get
            {
                return checkListKey;
            }
        }

        private string checkListName = string.Empty;

        public string CheckListName
        {
            set
            {
                checkListName = value;

                ValidateDirtyList(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, value);
            }
            get
            {
                return checkListName;
            }
        }

        private string checkListType = string.Empty;

        public string CheckListType
        {
            set
            {
                checkListType = value;

                ValidateDirtyList(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE, value);
            }
            get
            {
                return checkListType;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        public DataTable CheckListItemsData = null;

        public bool IsCheckListItemsModified = false;

        #endregion

        #region Public Override Methods

        public override bool Insert()
        {
            DataSet reqDS = new DataSet();

            DataTable checkListDataTable = EMS_CHECKLIST_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY, checkListKey },
                                                            { EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, checkListName },
                                                            { EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE, checkListType },
                                                            { EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION, description },
                                                            
                                                            { EMS_CHECKLIST_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref checkListDataTable, dataRow);

            checkListDataTable.AcceptChanges();

            reqDS.Tables.Add(checkListDataTable);

            if(this.CheckListItemsData != null && this.CheckListItemsData.Rows.Count > 0)
            {
                reqDS.Tables.Add(this.CheckListItemsData.Copy());
            }

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListEngine, FanHai.Hemera.Modules.EMS", "InsertCheckList", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            this.DirtyList.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME, string.Empty, string.Empty));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable checkListDataTable = EMS_CHECKLIST_FIELDS.CreateDataTable(false);

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref checkListDataTable, this.DirtyList);

            checkListDataTable.AcceptChanges();

            reqDS.Tables.Add(checkListDataTable);

            if (this.IsCheckListItemsModified && this.CheckListItemsData != null && this.CheckListItemsData.Rows.Count > 0)
            {
                reqDS.Tables.Add(this.CheckListItemsData.Copy());
            }

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListEngine, FanHai.Hemera.Modules.EMS", "UpdateCheckList", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListEngine, FanHai.Hemera.Modules.EMS", "DeleteCheckList", reqDS, out msg);

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
        /// Load Check List Data
        /// </summary>
        /// <param name="checkListName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-14 13:19:43
        public DataTable LoadCheckListData(string checkListName, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListEngine, FanHai.Hemera.Modules.EMS", "GetCheckList", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Load Check List Items Data
        /// </summary>
        /// <param name="checkListKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-15 15:30:46
        public DataTable LoadCheckListItemsData(string checkListKey, out string msg)
        {
            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkListKey);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckListEngine, FanHai.Hemera.Modules.EMS", "GetCheckListItems", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
