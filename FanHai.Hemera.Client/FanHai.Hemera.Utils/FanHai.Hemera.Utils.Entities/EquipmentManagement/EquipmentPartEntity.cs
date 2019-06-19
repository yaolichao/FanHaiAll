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
    public class EquipmentPartEntity : SEntity
    {
        #region Constructor

        public EquipmentPartEntity()
        {

        }

        #endregion

        #region Public Properties

        private string partKey = string.Empty;

        public string PartKey
        {
            set
            {
                partKey = value;
            }
            get
            {
                return partKey;
            }
        }

        private string partName = string.Empty;

        public string PartName
        {
            set
            {
                partName = value;

                ValidateDirtyList(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, value);
            }
            get
            {
                return partName;
            }
        }

        private string description = string.Empty;

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        private string partType = string.Empty;

        public string PartType
        {
            set
            {
                partType = value;

                ValidateDirtyList(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE, value);
            }
            get
            {
                return partType;
            }
        }

        private string partMode = string.Empty;

        public string PartMode
        {
            set
            {
                partMode = value;

                ValidateDirtyList(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE, value);
            }
            get
            {
                return partMode;
            }
        }

        private string partUnit = string.Empty;

        public string PartUnit
        {
            set
            {
                partUnit = value;

                ValidateDirtyList(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT, value);
            }
            get
            {
                return partUnit;
            }
        }

        #endregion

        #region Public Override Methods

        public override bool Insert()
        {
            DataSet reqDS = new DataSet();

            DataTable partsDataTable = EMS_EQUIPMENT_PARTS_FIELDS.CreateDataTable(false);

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY, partKey },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, partName },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION, description },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE, partType },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE, partMode },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT, partUnit },
                                                            
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref partsDataTable, dataRow);

            partsDataTable.AcceptChanges();

            reqDS.Tables.Add(partsDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentPartsEngine, FanHai.Hemera.Modules.EMS", "InsertPart", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, partKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            this.DirtyList.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME, string.Empty, string.Empty));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable partsDataTable = EMS_EQUIPMENT_PARTS_FIELDS.CreateDataTable(false);

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref partsDataTable, this.DirtyList);

            partsDataTable.AcceptChanges();

            reqDS.Tables.Add(partsDataTable);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentPartsEngine, FanHai.Hemera.Modules.EMS", "UpdatePart", reqDS, out msg);

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

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, partKey);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDITOR, Editor);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_EDIT_TIME, EditTime);

            string msg;

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentPartsEngine, FanHai.Hemera.Modules.EMS", "DeletePart", reqDS, out msg);

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
        /// Load Equipment Parts Data
        /// </summary>
        /// <param name="partName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-12 10:12:19
        public DataTable LoadPartsData(string partName, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, partName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentPartsEngine, FanHai.Hemera.Modules.EMS", "GetParts", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
