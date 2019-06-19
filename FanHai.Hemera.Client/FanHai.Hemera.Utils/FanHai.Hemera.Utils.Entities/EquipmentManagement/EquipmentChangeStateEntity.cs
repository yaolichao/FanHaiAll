using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Constants;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    /// <summary>
    /// Equipment Change State Entity
    /// </summary>
    /// Owner:Andy Gao 2010-07-16 10:59:50
    public class EquipmentChangeStateEntity : SEntity
    {
        #region Constructor

        public EquipmentChangeStateEntity()
        {

        }

        public EquipmentChangeStateEntity(string equipmentChangeStateKey)
        {
            this.equipmentChangeStateKey = equipmentChangeStateKey;
        }

        #endregion

        #region Private Properties

        private string equipmentChangeStateKey = string.Empty;
        private string equipmentChangeStateName = string.Empty;
        private string description = string.Empty;
        private string equipmentFromStateKey = string.Empty;
        private string equipmentToStateKey = string.Empty;
        private EntityState entityState = EntityState.None;

        #endregion

        #region Public Properties

        public string EquipmentChangeStateKey
        {
            set
            {
                equipmentChangeStateKey = value;
            }
            get
            {
                return equipmentChangeStateKey;
            }
        }

        public string EquipmentChangeStateName
        {
            set
            {
                equipmentChangeStateName = value;

                ValidateDirtyList(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, value);
            }
            get
            {
                return equipmentChangeStateName;
            }
        }

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        public string EquipmentFromStateKey
        {
            set
            {
                equipmentFromStateKey = value;

                ValidateDirtyList(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY, value);
            }
            get
            {
                return equipmentFromStateKey;
            }
        }

        public string EquipmentToStateKey
        {
            set
            {
                equipmentToStateKey = value;

                ValidateDirtyList(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY, value);
            }
            get
            {
                return equipmentToStateKey;
            }
        }

        public EntityState EntityState
        {
            get
            {
                return entityState;
            }
            set
            {
                entityState = value;
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

            if (!string.IsNullOrEmpty(equipmentChangeStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentChangeStateKey))
                {
                    inputKey = equipmentChangeStateKey;
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
                    resDS = serverFactory.CreateIEquipmentChangeStates().GetEquipmentChangeStates(reqDS);
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
                SetEquipmentChangeStateEntityProperties(resDS.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME]);

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
        /// Set Equipment Change State Entity Properties Data
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2010-07-16 11:16:18
        private void SetEquipmentChangeStateEntityProperties(DataTable dataTable)
        {
            if (dataTable != null && dataTable.TableName == EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                EquipmentChangeStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                EquipmentChangeStateName = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].ToString();
                Description = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION].ToString();
                EquipmentFromStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].ToString();
                EquipmentToStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();
            }
        }

        #endregion
    }
}
