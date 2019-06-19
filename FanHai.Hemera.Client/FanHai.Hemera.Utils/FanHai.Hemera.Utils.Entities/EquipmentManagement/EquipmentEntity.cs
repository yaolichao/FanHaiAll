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
    /// Equipment Entity Object
    /// </summary>
    /// Owner:Andy Gao 2010-08-05 16:14:25
    public class EquipmentEntity : SEntity
    {
        #region Constructor

        public EquipmentEntity()
        {

        }

        public EquipmentEntity(string equipmentKey)
        {
            this.equipmentKey = equipmentKey;
        }

        #endregion

        #region Private Properties

        private string equipmentKey = string.Empty;
        private string equipmentName = string.Empty;
        private string description = string.Empty;
        private string equipmentCode = string.Empty;
        private string equipmentMode = string.Empty;
        private string maxQuantity = "0";
        private string minQuantity = "0";
        private string equipmentType = string.Empty;
        private string equipmentGroupKey = string.Empty;
        private string equipmentLocationKey = string.Empty;
        private string isBatch = "0";
        private string isChamber = "0";
        private string isMultiChamber = "0";
        private string chamberTotal = "0";
        private string chamberIndex = "0";

        private string parentEquipmentKey = string.Empty;
        private string equipmentStateKey = string.Empty;
        private string equipmentChangeStateKey = string.Empty;

        private string equipment_wph=string.Empty;
        private string equipment_av_time=string.Empty;
        private string equipment_tract_time=string.Empty;
        private string equipment_assetsno=string.Empty;
        private string equipmentRealKey= string.Empty;
        #endregion

        #region Public Properties

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

        public string EquipmentName
        {
            set
            {
                equipmentName = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, value);
            }
            get
            {
                return equipmentName;
            }
        }

        public string Description
        {
            set
            {
                description = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION, value);
            }
            get
            {
                return description;
            }
        }

        public string EquipmentCode
        {
            set
            {
                equipmentCode = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE, value);
            }
            get
            {
                return equipmentCode;
            }
        }

        public string EquipmentMode
        {
            set
            {
                equipmentMode = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE, value);
            }
            get
            {
                return equipmentMode;
            }
        }

        public string MaxQuantity
        {
            set
            {
                maxQuantity = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY, value);
            }
            get
            {
                return maxQuantity;
            }
        }

        public string MinQuantity
        {
            set
            {
                minQuantity = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY, value);
            }
            get
            {
                return minQuantity;
            }
        }

        public string EquipmentType
        {
            set
            {
                equipmentType = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE, value);
            }
            get
            {
                return equipmentType;
            }
        }

        public string EquipmentGroupKey
        {
            set
            {
                equipmentGroupKey = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, value);
            }
            get
            {
                return equipmentGroupKey;
            }
        }

        public string EquipmentLocationKey
        {
            set
            {
                equipmentLocationKey = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, value);
            }
            get
            {
                return equipmentLocationKey;
            }
        }

        public string IsBatch
        {
            set
            {
                isBatch = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, value);
            }
            get
            {
                return isBatch;
            }
        }

        public string IsChamber
        {
            set
            {
                isChamber = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, value);
            }
            get
            {
                return isChamber;
            }
        }

        public string IsMultiChamber
        {
            set
            {
                isMultiChamber = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER, value);
            }
            get
            {
                return isMultiChamber;
            }
        }

        public string ChamberTotal
        {
            set
            {
                chamberTotal = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL, value);
            }
            get
            {
                return chamberTotal;
            }
        }

        public string ChamberIndex
        {
            set
            {
                chamberIndex = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX, value);
            }
            get
            {
                return chamberIndex;
            }
        }

        public string ParentEquipmentKey
        {
            set
            {
                parentEquipmentKey = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY, value);
            }
            get
            {
                return parentEquipmentKey;
            }
        }

        public string EquipmentStateKey
        {
            set
            {
                equipmentStateKey = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY, value);
            }
            get
            {
                return equipmentStateKey;
            }
        }

        public string EquipmentChangeStateKey
        {
            set
            {
                equipmentChangeStateKey = value;

                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, value);
            }
            get
            {
                return equipmentChangeStateKey;
            }
        }

        public string Equipment_WPH
        {
            set
            {
                equipment_wph = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH, value);
            }
            get
            {
                return equipment_wph;
            }
        }

        public string Equipment_Tract_Time
        {
            set
            {
                equipment_tract_time = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME, value);
            }
            get
            {
                return equipment_tract_time;
            }
        }

        public string Equipment_Av_Time
        {
            set
            {
                equipment_av_time = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME, value);
            }
            get
            {
                return equipment_av_time;
            }
        }

        public string Equipment_AssetsNo
        {
            set
            {
                equipment_assetsno = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO, value);
            }
            get
            {
                return equipment_assetsno;
            }
        }
        /// <summary>
        /// 虚拟设备对应的物理设备主键。
        /// </summary>
        public string EquipmentRealKey
        {
            set
            {
                equipmentRealKey = value;
                ValidateDirtyList(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY, value);
            }
            get
            {
                return equipmentRealKey;
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX) ||                    
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY)||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO)||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME)||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME)||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY) ||
                    DirtyList.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get;
            private set;
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

            if (!string.IsNullOrEmpty(equipmentKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    inputKey = equipmentKey;
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
                    int pages;
                    int records;

                    resDS = serverFactory.CreateIEquipments().GetEquipments(reqDS, string.Empty, -1, -1, out pages, out records);
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
                SetEquipmentEntityProperties(resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME]);

                return true;
            }
            else
            {
                MessageService.ShowError(returnMsg);

                return false;
            }

            #endregion
        }

        public DataSet GetInitEquipmentChangeState()
        {
            DataSet resDS = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipments().GetInitEquipmentChangeState();
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return resDS;
        }

        public override bool Insert()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Data

            DataTable equipmentsDataTable = EMS_EQUIPMENTS_FIELDS.CreateDataTable();

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey },
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, equipmentName},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION, description},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE, equipmentCode},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE, equipmentMode},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY, maxQuantity},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY, minQuantity},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE, equipmentType},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, equipmentGroupKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, equipmentLocationKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, isBatch},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, IsChamber},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX,chamberIndex},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL,chamberTotal},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER,isMultiChamber},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY, parentEquipmentKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY, equipmentStateKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY, equipmentRealKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME, string.Empty }
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentsDataTable, dataRow);

            equipmentsDataTable.AcceptChanges();

            reqDS.Tables.Add(equipmentsDataTable);

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    inputKey = equipmentKey;
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
                    resDS = serverFactory.CreateIEquipments().InsertEquipments(reqDS);
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

        public override bool Insert(DataSet ds)
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Data
            DataTable equipmentsDataTable = EMS_EQUIPMENTS_FIELDS.CreateDataTable();

            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                        {
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey },
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, equipmentName},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION, description},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE, equipmentCode},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE, equipmentMode},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY, maxQuantity},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY, minQuantity},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE, equipmentType},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, equipmentGroupKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, equipmentLocationKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, isBatch},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, IsChamber},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX,chamberIndex},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL,chamberTotal},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER,isMultiChamber},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY, parentEquipmentKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY, equipmentStateKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY,equipmentChangeStateKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY,equipmentRealKey},
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR, Creator },
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, CreateTimeZone },
                                                            { EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME, string.Empty },
                                                            {EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO,equipment_assetsno},
                                                            {EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME,equipment_av_time},
                                                            {EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH,equipment_wph},
                                                            {EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME,equipment_tract_time}
                                                        };

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentsDataTable, dataRow);

            equipmentsDataTable.AcceptChanges();

            //reqDS.Tables.Add(equipmentsDataTable);
            ds.Tables.Add(equipmentsDataTable);

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    inputKey = equipmentKey;
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

                //reqDS.Tables.Add(inputParamDataTable);
                ds.Tables.Add(inputParamDataTable);
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipments().InsertEquipments(ds);
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

            if (!string.IsNullOrEmpty(equipmentKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    inputKey = equipmentKey;
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

            #region Build Equipment Data

            this.DirtyList.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME, new DirtyItem(EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME, "", ""));

            Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataTable equipmentsDataTable = EMS_EQUIPMENTS_FIELDS.CreateDataTable();

            FanHai.Hemera.Utils.Common.Utils.AddKeyValuesToDataTable(ref equipmentsDataTable, DirtyList);

            equipmentsDataTable.AcceptChanges();

            reqDS.Tables.Add(equipmentsDataTable);

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipments().UpdateEquipments(reqDS);
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

            if (!string.IsNullOrEmpty(equipmentKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    inputKey = equipmentKey;
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
                    resDS = serverFactory.CreateIEquipments().DeleteEquipments(reqDS);
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
        /// Set Equipment Entity Properties Data
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2010-08-06 10:21:08
        private void SetEquipmentEntityProperties(DataTable dataTable)
        {
            if (dataTable != null && dataTable.TableName == EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                EquipmentKey = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                EquipmentName = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].ToString();
                Description = row[EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION].ToString();
                EquipmentCode = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].ToString();
                EquipmentMode = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE].ToString();
                MaxQuantity = row[EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY].ToString();
                MinQuantity = row[EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY].ToString();
                EquipmentType = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE].ToString();
                EquipmentGroupKey = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY].ToString();
                EquipmentLocationKey = row[EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY].ToString();
                IsBatch = row[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].ToString();
                IsChamber = row[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].ToString();
                IsMultiChamber = row[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].ToString();
                ChamberTotal = row[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].ToString();
                ChamberIndex = row[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].ToString();
                ParentEquipmentKey = row[EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].ToString();
                EquipmentStateKey = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY].ToString();
                EquipmentChangeStateKey = row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                this.EquipmentRealKey = Convert.ToString(row[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY]);
                Creator = row[EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR].ToString();
                CreateTimeZone = row[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].ToString();
                CreateTime = row[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME].ToString();

                Editor = row[EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR].ToString();
                EditTimeZone = row[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                EditTime = row[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME].ToString();
            }
        }
        #endregion

        /// <summary>
        /// 获得状态切换可用的设备信息
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner :genchille.yang 2012-05-03 10:07:22
        /// Modify by genchille.yang 2012-05-09 14:04:28
        public DataTable GetStateEventEquipments(string equipmentName, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, equipmentName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);
            reqDS.ExtendedProperties.Add(PROPERTY_FIELDS.USER_NAME, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            reqDS.ExtendedProperties.Add("FLAG", "Q");

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.Equipments, FanHai.Hemera.Modules.EMS", "GetParentEquipments", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Get Parent Equipment data
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner: Rayna liu 2011-09-21 10:03 获取设备看板中未使用过的父设备
        public DataTable GetParentEquipments(string equipmentName, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, equipmentName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);
            reqDS.ExtendedProperties.Add(PROPERTY_FIELDS.USER_NAME, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            reqDS.ExtendedProperties.Add("FLAG", "E");
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.Equipments, FanHai.Hemera.Modules.EMS", "GetParentEquipments", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetChildEquipments(string equipmentName, int pageNo, int pageSize, out int pages, out int records, out string msg)
        {
            pages = 0;
            records = 0;

            DataSet reqDS = new DataSet();

            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, equipmentName);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGENO, pageNo);
            reqDS.ExtendedProperties.Add(PARAMETERS.INPUT_PAGESIZE, pageSize);

            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.Equipments, FanHai.Hemera.Modules.EMS", "GetChildEquipments", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                pages = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_PAGES) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_PAGES]) : 0;
                records = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_RECORDS) ? Convert.ToInt32(resDS.ExtendedProperties[PARAMETERS.OUTPUT_RECORDS]) : 0;

                return resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取设备信息。
        /// </summary>
        /// <param name="equipmentCode">设备编码，左匹配模糊查询。</param>
        /// <param name="equipmentName">设备名称，左匹配模糊查询。</param>
        /// <param name="equipmentType">设备类型。如果为空，则查询所有类型的设备。</param>
        /// <param name="pconfig">数据分页查询的配置对象。</param>
        /// <returns>包含设备数据信息的数据集对象。</returns>
        public DataSet GetEquipments(string equipmentCode, string equipmentName, string equipmentType, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipments(equipmentCode, equipmentName, equipmentType, ref pconfig);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

    }
}
