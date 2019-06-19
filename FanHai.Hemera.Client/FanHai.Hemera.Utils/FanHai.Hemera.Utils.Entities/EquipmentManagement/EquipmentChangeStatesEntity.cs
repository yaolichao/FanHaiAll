using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using FanHai.Hemera.Utils.Common;
using System.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    /// <summary>
    /// Equipment Change States Entity
    /// </summary>
    /// Owner:Andy Gao 2010-07-16 16:31:40
    public class EquipmentChangeStatesEntity
    {
        #region Private Variables

        private List<EquipmentChangeStateEntity> equipmentChangeStateList = new List<EquipmentChangeStateEntity>();

        private EquipmentChangeStateEntityComparer equipmentChangeStateEntityComparer = new EquipmentChangeStateEntityComparer();

        #endregion

        #region Public Properties

        public List<EquipmentChangeStateEntity> EquipmentChangeStateList
        {
            get
            {
                return equipmentChangeStateList;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Remove Entity State Is Added Match
        /// </summary>
        /// <param name="changeStateEntity"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-21 13:20:50
        private bool RemoveAddedMatch(EquipmentChangeStateEntity changeStateEntity)
        {
            if (changeStateEntity.EntityState == EntityState.Added)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove Entity State Is Deleted Match
        /// </summary>
        /// <param name="changeStateEntity"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-21 13:36:05
        private bool RemoveDeletedMatch(EquipmentChangeStateEntity changeStateEntity)
        {
            if (changeStateEntity.EntityState == EntityState.Deleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initial Equipment Change State List
        /// </summary>
        /// <param name="equipmentChangeStatesDataTable"></param>
        /// Owner:Andy Gao 2010-07-16 14:02:02
        public void InitEquipmentChangeStateList(DataTable equipmentChangeStatesDataTable)
        {
            equipmentChangeStateList.Clear();

            if (equipmentChangeStatesDataTable != null && equipmentChangeStatesDataTable.Rows.Count > 0)
            {
                foreach (DataRow row in equipmentChangeStatesDataTable.Rows)
                {
                    EquipmentChangeStateEntity equipmentChangeStateEntity = new EquipmentChangeStateEntity();

                    equipmentChangeStateEntity.EquipmentChangeStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                    equipmentChangeStateEntity.EquipmentChangeStateName = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].ToString();
                    equipmentChangeStateEntity.Description = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION].ToString();
                    equipmentChangeStateEntity.EquipmentFromStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].ToString();
                    equipmentChangeStateEntity.EquipmentToStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();

                    equipmentChangeStateEntity.EntityState = EntityState.Deleted;

                    equipmentChangeStateList.Add(equipmentChangeStateEntity);
                }
            }
        }

        /// <summary>
        /// Resume Current Equipment Change States Entity
        /// </summary>
        /// Owner:Andy Gao 2010-07-19 10:21:11
        public void ResumeEquipmentChangeStatesEntity()
        {
            //Remove All Equipment Change States By The EntityState is Added
            equipmentChangeStateList.RemoveAll(RemoveAddedMatch);

            foreach (EquipmentChangeStateEntity equipmentChangeStateEntity in equipmentChangeStateList)
            {
                equipmentChangeStateEntity.EntityState = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Update Current Equipment Change States Entity
        /// </summary>
        /// Owner:Andy Gao 2010-07-21 13:39:49
        public void UpdateEquipmentChangeStatesEntity()
        {
            //Remove All Equipment Change States By The EntityState is Deleted
            equipmentChangeStateList.RemoveAll(RemoveDeletedMatch);

            foreach (EquipmentChangeStateEntity equipmentChangeStateEntity in equipmentChangeStateList)
            {
                equipmentChangeStateEntity.EntityState = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Is Equipment Change States Updated
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-21 13:41:59
        public bool IsEquipmentChangeStatesUpdated()
        {
            foreach (EquipmentChangeStateEntity equipmentChangeStateEntity in equipmentChangeStateList)
            {
                if (equipmentChangeStateEntity.EntityState != EntityState.None)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get Equipment Change States Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-19 09:10:21
        public DataTable GetEquipmentChangeStatesDataTable()
        {
            DataTable equipmentChangeStatesDataTable = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.CreateDataTable();

            foreach (EquipmentChangeStateEntity equipmentChangeStateEntity in equipmentChangeStateList)
            {
                if (equipmentChangeStateEntity.EntityState != EntityState.None)
                {
                    Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateEntity.EquipmentChangeStateKey },
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, equipmentChangeStateEntity.EquipmentChangeStateName},
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION, equipmentChangeStateEntity.Description},
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY, equipmentChangeStateEntity.EquipmentFromStateKey},
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY, equipmentChangeStateEntity.EquipmentToStateKey }
                                                        };

                    FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentChangeStatesDataTable, dataRow);

                    equipmentChangeStatesDataTable.AcceptChanges();
                }
            }

            return equipmentChangeStatesDataTable;
        }

        /// <summary>
        /// Add Equipment Change State For Update
        /// </summary>
        /// <param name="equipmentChangeStateEntity"></param>
        /// Owner:Andy Gao 2010-07-16 14:08:32
        public void Add(EquipmentChangeStateEntity equipmentChangeStateEntity)
        {
            if (equipmentChangeStateEntity != null)
            {
                if (equipmentChangeStateList.Contains(equipmentChangeStateEntity, equipmentChangeStateEntityComparer))
                {
                    EquipmentChangeStateEntity actualEquipmentChangeStateEntity = equipmentChangeStateList.Find(
                        delegate(EquipmentChangeStateEntity obj)
                        {
                            return obj.EquipmentFromStateKey == equipmentChangeStateEntity.EquipmentFromStateKey 
                                && obj.EquipmentToStateKey == equipmentChangeStateEntity.EquipmentToStateKey;
                        }
                    );

                    if (actualEquipmentChangeStateEntity != null)
                    {
                        actualEquipmentChangeStateEntity.EntityState = EntityState.None;
                    }
                }
                else
                {
                    equipmentChangeStateEntity.EntityState = EntityState.Added;

                    equipmentChangeStateList.Add(equipmentChangeStateEntity);
                }
            }
        }

        public bool Save()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Change States Data

            DataTable equipmentChangeStatesDataTable = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.CreateDataTable();

            foreach (EquipmentChangeStateEntity equipmentChangeStateEntity in equipmentChangeStateList)
            {
                if (equipmentChangeStateEntity.EntityState != EntityState.None)
                {
                    Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateEntity.EquipmentChangeStateKey },
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, equipmentChangeStateEntity.EquipmentChangeStateName},
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION, equipmentChangeStateEntity.Description},
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY, equipmentChangeStateEntity.EquipmentFromStateKey},
                                                            { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY, equipmentChangeStateEntity.EquipmentToStateKey }
                                                        };

                    FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentChangeStatesDataTable, dataRow);

                    if (equipmentChangeStateEntity.EntityState == EntityState.Added)
                    {
                        equipmentChangeStatesDataTable.Rows[equipmentChangeStatesDataTable.Rows.Count - 1].AcceptChanges();
                        equipmentChangeStatesDataTable.Rows[equipmentChangeStatesDataTable.Rows.Count - 1].SetAdded();
                    }
                    else if (equipmentChangeStateEntity.EntityState == EntityState.Deleted)
                    {
                        equipmentChangeStatesDataTable.Rows[equipmentChangeStatesDataTable.Rows.Count - 1].AcceptChanges();
                        equipmentChangeStatesDataTable.Rows[equipmentChangeStatesDataTable.Rows.Count - 1].Delete();
                    }
                }
            }

            reqDS.Tables.Add(equipmentChangeStatesDataTable);

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentChangeStates().UpdateEquipmentChangeStates(reqDS);
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
    }

    /// <summary>
    /// Equipment Change State Entity Comparer
    /// </summary>
    /// Owner:Andy Gao 2010-07-16 12:45:22
    public class EquipmentChangeStateEntityComparer : IEqualityComparer<EquipmentChangeStateEntity>
    {
        #region IEqualityComparer<EquipmentChangeStateEntity> Members

        public bool Equals(EquipmentChangeStateEntity x, EquipmentChangeStateEntity y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

            return (x.EquipmentFromStateKey == y.EquipmentFromStateKey) && (x.EquipmentToStateKey == y.EquipmentToStateKey);
        }

        public int GetHashCode(EquipmentChangeStateEntity obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashChangeStateKey = obj.EquipmentChangeStateKey == null ? 0 : obj.EquipmentChangeStateKey.GetHashCode();

            int hashChangeStateName = obj.EquipmentChangeStateName == null ? 0 : obj.EquipmentChangeStateName.GetHashCode();

            return hashChangeStateKey ^ hashChangeStateName;
        }

        #endregion
    }
}
