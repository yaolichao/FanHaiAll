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
    public class EquipmentCheckItemEntity : SEntity
    {
        #region Constructor

        public EquipmentCheckItemEntity()
        {

        }

        #endregion

        #region Public Properties

        private string checkItemKey = string.Empty;

        public string CheckItemKey
        {
            set
            {
                checkItemKey = value;
            }
            get
            {
                return checkItemKey;
            }
        }

        private string checkItemName = string.Empty;

        public string CheckItemName
        {
            set
            {
                checkItemName = value;

                ValidateDirtyList(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, value);
            }
            get
            {
                return checkItemName;
            }
        }

        private string checkItemType = string.Empty;

        public string CheckItemType
        {
            set
            {
                checkItemType = value;

                ValidateDirtyList(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE, value);
            }
            get
            {
                return checkItemType;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        #endregion

        #region Public Override Methods

        public override bool Insert()
        {
            DataSet reqDS = new DataSet();

            DataTable checkItemsDataTable = EMS_CHECKITEMS_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY, checkItemKey },
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, checkItemName },
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE, checkItemType },
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION, description },
                                                            
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref checkItemsDataTable, dataRow);

            checkItemsDataTable.AcceptChanges();

            reqDS.Tables.Add(checkItemsDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckItemsEngine, FanHai.Hemera.Modules.EMS", "InsertCheckItem", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkItemKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            this.DirtyList.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME, string.Empty, string.Empty));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable checkItemsDataTable = EMS_CHECKITEMS_FIELDS.CreateDataTable(false);

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref checkItemsDataTable, this.DirtyList);

            checkItemsDataTable.AcceptChanges();

            reqDS.Tables.Add(checkItemsDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckItemsEngine, FanHai.Hemera.Modules.EMS", "UpdateCheckItem", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkItemKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckItemsEngine, FanHai.Hemera.Modules.EMS", "DeleteCheckItem", reqDS, out msg);

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
        /// Load Check Items Data
        /// </summary>
        /// <param name="checkItemName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-12 10:05:02
        public DataTable LoadCheckItemsData(string checkItemName, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, checkItemName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentCheckItemsEngine, FanHai.Hemera.Modules.EMS", "GetCheckItems", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
