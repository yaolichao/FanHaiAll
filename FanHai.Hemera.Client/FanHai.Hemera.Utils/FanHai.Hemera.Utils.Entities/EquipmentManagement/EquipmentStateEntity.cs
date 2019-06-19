using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    /// <summary>
    /// Equipment State Entity Object
    /// </summary>
    /// Owner:Andy Gao 2010-07-06 12:55:45
    public class EquipmentStateEntity : SEntity
    {
        #region Constructor

        public EquipmentStateEntity()
        {

        }

        public EquipmentStateEntity(string equipmentStateKey)
        {
            this.equipmentStateKey = equipmentStateKey;
        }

        #endregion

        #region Private Properties

        private string equipmentStateKey = string.Empty;
        private string equipmentStateName = string.Empty;
        private string description = string.Empty;
        private string equipmentStateType = string.Empty;
        private string equipmentStateCategory = string.Empty;

        #endregion

        #region Public Properties

        public string EquipmentStateKey
        {
            set
            {
                equipmentStateKey = value;
            }
            get
            {
                return equipmentStateKey;
            }
        }

        public string EquipmentStateName
        {
            set
            {
                equipmentStateName = value;

                ValidateDirtyList(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME, value);
            }
            get
            {
                return equipmentStateName;
            }
        }

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        public string EquipmentStateType
        {
            set
            {
                equipmentStateType = value;

                ValidateDirtyList(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE, value);
            }
            get
            {
                return equipmentStateType;
            }
        }

        public string EquipmentStateCategory
        {
            set
            {
                equipmentStateCategory = value;

                ValidateDirtyList(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY, value);
            }
            get
            {
                return equipmentStateCategory;
            }
        }

        public override bool IsDirty
        {
            get
            {
                if(DirtyList.ContainsKey(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY))
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

            if (!string.IsNullOrEmpty(equipmentStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentStateKey))
                {
                    inputKey = equipmentStateKey;
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
                    resDS = serverFactory.CreateIEquipmentStates().GetEquipmentStates(reqDS);
                }
            }
            catch(Exception ex)
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
                SetEquipmentStateEntityProperties(resDS.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME]);

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

            #region Build Equipment State Data

            DataTable equipmentStateDataTable = EMS_EQUIPMENT_STATES_FIELDS.CreateDataTable();

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY, equipmentStateKey },
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME, equipmentStateName},
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION, description},
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE, equipmentStateType},
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY, equipmentStateCategory },
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentStateDataTable, dataRow);

            equipmentStateDataTable.AcceptChanges();

            reqDS.Tables.Add(equipmentStateDataTable);

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentStateKey))
                {
                    inputKey = equipmentStateKey;
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
                    resDS = serverFactory.CreateIEquipmentStates().InsertEquipmentStates(reqDS);
                }
            }
            catch(Exception ex)
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

            if (!string.IsNullOrEmpty(equipmentStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentStateKey))
                {
                    inputKey = equipmentStateKey;
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

            #region Build Equipment State Data

            this.DirtyList.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME, "", ""));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable equipmentStateDataTable = EMS_EQUIPMENT_STATES_FIELDS.CreateDataTable();

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentStateDataTable, DirtyList);

            equipmentStateDataTable.AcceptChanges();

            reqDS.Tables.Add(equipmentStateDataTable);

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentStates().UpdateEquipmentStates(reqDS);
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

            if (!string.IsNullOrEmpty(equipmentStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentStateKey))
                {
                    inputKey = equipmentStateKey;
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
                    //删除设备状态中的信息
                    resDS = serverFactory.CreateIEquipmentStates().DeleteEquipmentStates(reqDS);
                }
            }
            catch(Exception ex)
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
        /// Set Equipment State Entity Properties Data
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2010-07-12 09:37:53
        private void SetEquipmentStateEntityProperties(DataTable dataTable)
        {
            if (dataTable != null && dataTable.TableName == EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                EquipmentStateKey = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY].ToString();
                EquipmentStateName = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                Description = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION].ToString();
                EquipmentStateType = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].ToString();
                EquipmentStateCategory = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY].ToString();

                Creator = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATOR].ToString();
                CreateTimeZone = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIMEZONE_KEY].ToString();
                CreateTime = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIME].ToString();

                Editor = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR].ToString();
                EditTimeZone = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                EditTime = row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME].ToString();
            }
        }

        #endregion
    }
}
