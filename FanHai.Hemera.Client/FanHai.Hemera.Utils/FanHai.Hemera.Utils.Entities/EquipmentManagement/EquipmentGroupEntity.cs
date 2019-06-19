using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    /// <summary>
    /// Equipment Group Entity Object
    /// </summary>
    /// Owner:Andy Gao 2010-08-02 13:41:37
    public class EquipmentGroupEntity : SEntity
    {
        #region Constructor

        public EquipmentGroupEntity()
        {

        }

        public EquipmentGroupEntity(string equipmentGroupKey)
        {
            this.equipmentGroupKey = equipmentGroupKey;
        }

        #endregion

        #region Private Properties

        private string equipmentGroupKey = string.Empty;
        private string equipmentGroupName = string.Empty;
        private string spec = string.Empty;
        private string description = string.Empty;

        #endregion

        #region Public Properties

        public string EquipmentGroupKey
        {
            set
            {
                equipmentGroupKey = value;
            }
            get
            {
                return equipmentGroupKey;
            }
        }

        public string EquipmentGroupName
        {
            set
            {
                equipmentGroupName = value;

                ValidateDirtyList(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, value);
            }
            get
            {
                return equipmentGroupName;
            }
        }

        public string Spec
        {
            set
            {
                spec = value;

                ValidateDirtyList(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC, value);
            }
            get
            {
                return spec;
            }
        }

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (DirtyList.ContainsKey(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region Public Methods

        public override bool Load()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentGroupKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentGroupKey))
                {
                    inputKey = equipmentGroupKey;
                }

                if (!string.IsNullOrEmpty(Editor))
                {
                    inputEditor = Editor;
                }

                if (!string.IsNullOrEmpty(EditTime))
                {
                    inputEditTime = EditTime;
                }

                inputParamDataTable.Rows.Add(new object[] { inputKey, inputEditor, inputEditTime });

                inputParamDataTable.AcceptChanges();

                reqDS.Tables.Add(inputParamDataTable);
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentGroups().GetEquipmentGroups(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                SetEquipmentGroupEntityProperties(resDS.Tables[EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME]);

                return true;
            }
            else
            {
                MessageService.ShowError(returnMsg);

                return false;
            }

            #endregion
        }

        public override bool Insert()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Groups Data

            DataTable equipmentGroupsDataTable = EMS_EQUIPMENT_GROUPS_FIELDS.CreateDataTable();

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, equipmentGroupKey },
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, equipmentGroupName},
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC, spec},
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION, description},
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentGroupsDataTable, dataRow);

            equipmentGroupsDataTable.AcceptChanges();

            reqDS.Tables.Add(equipmentGroupsDataTable);

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentGroupKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentGroupKey))
                {
                    inputKey = equipmentGroupKey;
                }

                if (!string.IsNullOrEmpty(Creator))
                {
                    inputEditor = Creator;
                }

                if (!string.IsNullOrEmpty(CreateTime))
                {
                    inputEditTime = CreateTime;
                }

                inputParamDataTable.Rows.Add(new object[] { inputKey, inputEditor, inputEditTime });

                inputParamDataTable.AcceptChanges();

                reqDS.Tables.Add(inputParamDataTable);
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentGroups().InsertEquipmentGroups(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string outputcreateTime = string.Empty;

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS, ref outputcreateTime);

            if (string.IsNullOrEmpty(returnMsg))
            {
                CreateTime = outputcreateTime;

                return true;
            }
            else
            {
                MessageService.ShowError(returnMsg);

                return false;
            }

            #endregion
        }

        public override bool Update()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentGroupKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentGroupKey))
                {
                    inputKey = equipmentGroupKey;
                }

                if (!string.IsNullOrEmpty(Editor))
                {
                    inputEditor = Editor;
                }

                if (!string.IsNullOrEmpty(EditTime))
                {
                    inputEditTime = EditTime;
                }

                inputParamDataTable.Rows.Add(new object[] { inputKey, inputEditor, inputEditTime });

                inputParamDataTable.AcceptChanges();

                reqDS.Tables.Add(inputParamDataTable);
            }

            #endregion

            #region Build Equipment Groups Data

            this.DirtyList.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME, "", ""));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable equipmentGroupsDataTable = EMS_EQUIPMENT_GROUPS_FIELDS.CreateDataTable();

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentGroupsDataTable, DirtyList);

            equipmentGroupsDataTable.AcceptChanges();

            reqDS.Tables.Add(equipmentGroupsDataTable);

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentGroups().UpdateEquipmentGroups(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string outputEditTime = string.Empty;

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS, ref outputEditTime);

            if (string.IsNullOrEmpty(returnMsg))
            {
                EditTime = outputEditTime;

                return true;
            }
            else
            {
                MessageService.ShowError(returnMsg);

                return false;
            }

            #endregion
        }

        public override bool Delete()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentGroupKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentGroupKey))
                {
                    inputKey = equipmentGroupKey;
                }

                if (!string.IsNullOrEmpty(Editor))
                {
                    inputEditor = Editor;
                }

                if (!string.IsNullOrEmpty(EditTime))
                {
                    inputEditTime = EditTime;
                }

                inputParamDataTable.Rows.Add(new object[] { inputKey, inputEditor, inputEditTime });
                inputParamDataTable.AcceptChanges();

                reqDS.Tables.Add(inputParamDataTable);
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentGroups().DeleteEquipmentGroups(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                return true;
            }
            else
            {
                MessageService.ShowError(returnMsg);

                return false;
            }

            #endregion
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set Equipment Group Entity Properties Data
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2010-07-12 09:37:53
        private void SetEquipmentGroupEntityProperties(DataTable dataTable)
        {
            if (dataTable != null && dataTable.TableName == EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                EquipmentGroupKey = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY].ToString();
                EquipmentGroupName = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].ToString();
                Spec = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC].ToString();
                Description = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION].ToString();

                Creator = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATOR].ToString();
                CreateTimeZone = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].ToString();
                CreateTime = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIME].ToString();

                Editor = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR].ToString();
                EditTimeZone = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                EditTime = row[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME].ToString();
            }
        }

        #endregion
    }
}
