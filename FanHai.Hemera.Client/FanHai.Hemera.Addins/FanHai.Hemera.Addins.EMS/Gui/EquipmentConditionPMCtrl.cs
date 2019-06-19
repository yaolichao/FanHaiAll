using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Gui.Core;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentConditionPMCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListJobEntity Object

        EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

        #endregion

        #region EquipmentConditionPMEntity Object

        EquipmentConditionPMEntity equipmentConditionPMEntity = new EquipmentConditionPMEntity();

        #endregion

        #region Constructor

        public EquipmentConditionPMCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentConditionPMCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentConditionPMCtrl_afterStateChanged);

            LoadEquipmentLocationData();
            LoadEquipmentGroupData();
            LoadEquipmentData();
            LoadNotifyUsersData();
            LoadEquipmentChangeStateData();
            LoadEquipmentChangeReasonsData();

            this.State = ControlState.Read;
        }

        private void EquipmentConditionPMCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtConditionPMName.Enabled = false;
                    this.txtCheckListName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.cmbCounterType.Enabled = false;
                    this.txtCounterWarnning.Enabled = false;
                    this.txtCounterTarget.Enabled = false;
                    this.txtCounterTotal.Enabled = false;
                    this.cmbNotifyUser.Enabled = false;
                    this.cmbNotifyCCUser.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = false;
                    this.cmbEquipmentChangeReason.Enabled = false;

                    grvConditionPMList_FocusedRowChanged(this.grvConditionPMList, new FocusedRowChangedEventArgs(-1, this.grvConditionPMList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtConditionPMName.Enabled = false;
                    this.txtCheckListName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.cmbCounterType.Enabled = false;
                    this.txtCounterWarnning.Enabled = false;
                    this.txtCounterTarget.Enabled = false;
                    this.txtCounterTotal.Enabled = false;
                    this.cmbNotifyUser.Enabled = false;
                    this.cmbNotifyCCUser.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = false;
                    this.cmbEquipmentChangeReason.Enabled = false;

                    grvConditionPMList_FocusedRowChanged(this.grvConditionPMList, new FocusedRowChangedEventArgs(-1, this.grvConditionPMList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtConditionPMName.Enabled = true;
                    this.txtCheckListName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.cmbCounterType.Enabled = true;
                    this.txtCounterWarnning.Enabled = true;
                    this.txtCounterTarget.Enabled = true;
                    this.txtCounterTotal.Enabled = false;
                    this.cmbNotifyUser.Enabled = true;
                    this.cmbNotifyCCUser.Enabled = true;
                    this.cmbEquipmentChangeState.Enabled = true;
                    this.cmbEquipmentChangeReason.Enabled = true;

                    this.txtConditionPMName.Text = string.Empty;
                    this.txtCheckListName.Text = string.Empty;
                    this.txtCheckListName.Tag = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    this.cmbCounterType.EditValue = string.Empty;
                    this.txtCounterWarnning.EditValue = 0;
                    this.txtCounterTarget.EditValue = 0;
                    this.txtCounterTotal.EditValue = 0;
                    this.cmbNotifyUser.EditValue = string.Empty;
                    this.cmbNotifyCCUser.EditValue = string.Empty;
                    this.cmbEquipmentChangeState.EditValue = string.Empty;
                    this.cmbEquipmentChangeReason.EditValue = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtConditionPMName.Enabled = true;
                    this.txtCheckListName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.cmbCounterType.Enabled = true;
                    this.txtCounterWarnning.Enabled = true;
                    this.txtCounterTarget.Enabled = true;
                    this.txtCounterTotal.Enabled = false;
                    this.cmbNotifyUser.Enabled = true;
                    this.cmbNotifyCCUser.Enabled = true;
                    this.cmbEquipmentChangeState.Enabled = true;
                    this.cmbEquipmentChangeReason.Enabled = true;

                    grvConditionPMList_FocusedRowChanged(this.grvConditionPMList, new FocusedRowChangedEventArgs(-1, this.grvConditionPMList.FocusedRowHandle));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            #region Initial Equipment Location ComboBox

            this.cmbEquipmentLocation.Properties.Columns.Clear();
            this.cmbEquipmentLocation.Properties.Columns.Add(new LookUpColumnInfo(FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME));
            this.cmbEquipmentLocation.Properties.ShowHeader = false;
            this.cmbEquipmentLocation.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            this.cmbEquipmentLocation.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;

            #endregion

            #region Initial Equipment Group ComboBox

            this.cmbEquipmentGroup.Properties.Columns.Clear();
            this.cmbEquipmentGroup.Properties.Columns.Add(new LookUpColumnInfo(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME));
            this.cmbEquipmentGroup.Properties.ShowHeader = false;
            this.cmbEquipmentGroup.Properties.DisplayMember = EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME;
            this.cmbEquipmentGroup.Properties.ValueMember = EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY;

            #endregion

            #region Initial Equipment ComboBox

            this.cmbEquipment.Properties.Columns.Clear();
            this.cmbEquipment.Properties.Columns.Add(new LookUpColumnInfo(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME));
            this.cmbEquipment.Properties.ShowHeader = false;
            this.cmbEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME;
            this.cmbEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;

            #endregion

            #region Initial Counter Type ComboBox

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("COUNTER_TYPE");

            dataTable.Rows.Add("ByBatch");
            dataTable.Rows.Add("ByLot");
            dataTable.Rows.Add("ByCell");

            dataTable.AcceptChanges();

            this.cmbCounterType.Properties.Columns.Clear();
            this.cmbCounterType.Properties.Columns.Add(new LookUpColumnInfo("COUNTER_TYPE"));
            this.cmbCounterType.Properties.ShowHeader = false;
            this.cmbCounterType.Properties.DisplayMember = "COUNTER_TYPE";
            this.cmbCounterType.Properties.ValueMember = "COUNTER_TYPE";

            this.cmbCounterType.Properties.DataSource = dataTable;

            #endregion

            #region Initial Notify User ComboBox

            this.cmbNotifyUser.Properties.Columns.Clear();
            this.cmbNotifyUser.Properties.Columns.Add(new LookUpColumnInfo(RBAC_USER_FIELDS.FIELD_USERNAME));
            this.cmbNotifyUser.Properties.ShowHeader = false;
            this.cmbNotifyUser.Properties.DisplayMember = RBAC_USER_FIELDS.FIELD_USERNAME;
            this.cmbNotifyUser.Properties.ValueMember = RBAC_USER_FIELDS.FIELD_USER_KEY;

            #endregion

            #region Initial Notify CC User ComboBox

            this.cmbNotifyCCUser.Properties.Columns.Clear();
            this.cmbNotifyCCUser.Properties.Columns.Add(new LookUpColumnInfo(RBAC_USER_FIELDS.FIELD_USERNAME));
            this.cmbNotifyCCUser.Properties.ShowHeader = false;
            this.cmbNotifyCCUser.Properties.DisplayMember = RBAC_USER_FIELDS.FIELD_USERNAME;
            this.cmbNotifyCCUser.Properties.ValueMember = RBAC_USER_FIELDS.FIELD_USER_KEY;

            #endregion

            #region Initial Equipment Change State ComboBox

            this.cmbEquipmentChangeState.Properties.Columns.Clear();
            this.cmbEquipmentChangeState.Properties.Columns.Add(new LookUpColumnInfo(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME));
            this.cmbEquipmentChangeState.Properties.ShowHeader = false;
            this.cmbEquipmentChangeState.Properties.DisplayMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME;
            this.cmbEquipmentChangeState.Properties.ValueMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY;

            #endregion

            #region Initial Equipment Change Reasons

            this.cmbEquipmentChangeReason.Properties.Columns.Clear();
            this.cmbEquipmentChangeReason.Properties.Columns.Add(new LookUpColumnInfo(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME));
            this.cmbEquipmentChangeReason.Properties.ShowHeader = false;
            this.cmbEquipmentChangeReason.Properties.DisplayMember = EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME;
            this.cmbEquipmentChangeReason.Properties.ValueMember = EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY;

            #endregion

            #region Initial Equipment Condition PM Grid

            dataTable = GetEmptyConditionPMDataTable();

            ControlUtils.InitialGridView(this.grvConditionPMList, dataTable);

            #endregion
        }

        protected override void InitUIResourcesByCulture()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");

            this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            this.tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            this.tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");

            this.grvConditionPMList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvConditionPMList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME].Caption = "计划PM名称";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TYPE].Caption = "计数类型";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_WARNNING].Caption = "计数警告值";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TARGET].Caption = "计数目标值";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TOTAL].Caption = "计数总数值";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_NAME].Caption = "通知用户";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_NAME].Caption = "通知抄送用户";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].Caption = "设备转变状态名称";
            this.grvConditionPMList.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME].Caption = "设备转变原因名称";
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = "设备名称";
            this.grvConditionPMList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].Caption = "检查表单名称";

            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvConditionPMList.Columns[EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvConditionPMList.BestFitColumns();
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentConditionPMEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentConditionPMEntity.ConditionKey =  CommonUtils.GenerateNewKey(0);
                    this.equipmentConditionPMEntity.EquipmentKey = this.cmbEquipment.EditValue.ToString();

                    this.equipmentConditionPMEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentConditionPMEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentConditionPMEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentConditionPMEntity.ConditionKey = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY).ToString();
                    this.equipmentConditionPMEntity.ConditionName = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME).ToString();
                    this.equipmentConditionPMEntity.CheckListKey = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                    this.equipmentConditionPMEntity.Description = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_DESCRIPTION).ToString();
                    this.equipmentConditionPMEntity.CounterType = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TYPE).ToString();
                    this.equipmentConditionPMEntity.CounterWarnning = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_WARNNING).ToString();
                    this.equipmentConditionPMEntity.CounterTarget = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TARGET).ToString();
                    this.equipmentConditionPMEntity.CounterTotal = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TOTAL).ToString();
                    this.equipmentConditionPMEntity.NotifyUserKey = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_KEY).ToString();
                    this.equipmentConditionPMEntity.NotifyCCUserKey = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_KEY).ToString();
                    this.equipmentConditionPMEntity.EquipmentChangeStateKey = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();
                    this.equipmentConditionPMEntity.EquipmentChangeReasonKey = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY).ToString();

                    this.equipmentConditionPMEntity.Editor = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentConditionPMEntity.EditTimeZone = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentConditionPMEntity.EditTime = this.grvConditionPMList.GetFocusedRowCellValue(EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentConditionPMEntity.IsInitializeFinished = true;

            this.equipmentConditionPMEntity.ConditionName = this.txtConditionPMName.Text.Trim();
            this.equipmentConditionPMEntity.CheckListKey = this.txtCheckListName.Tag.ToString();
            this.equipmentConditionPMEntity.Description = this.txtDescription.Text.Trim();
            this.equipmentConditionPMEntity.CounterType = this.cmbCounterType.Text;
            this.equipmentConditionPMEntity.CounterWarnning = this.txtCounterWarnning.Text.Trim();
            this.equipmentConditionPMEntity.CounterTarget = this.txtCounterTarget.Text.Trim();
            this.equipmentConditionPMEntity.CounterTotal = this.txtCounterTotal.Text.Trim();
            this.equipmentConditionPMEntity.NotifyUserKey = this.cmbNotifyUser.EditValue.ToString();
            this.equipmentConditionPMEntity.NotifyCCUserKey = this.cmbNotifyCCUser.EditValue.ToString();
            this.equipmentConditionPMEntity.EquipmentChangeStateKey = this.cmbEquipmentChangeState.Text.Length == 0 ? string.Empty : this.cmbEquipmentChangeState.EditValue.ToString();
            this.equipmentConditionPMEntity.EquipmentChangeReasonKey = this.cmbEquipmentChangeReason.Text.Length == 0 ? string.Empty : this.cmbEquipmentChangeReason.EditValue.ToString();
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    //TODO: Refresh Condition PM Data

                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentConditionPMEntity.DirtyList)
                    {
                        this.grvConditionPMList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY)
                        {
                            this.grvConditionPMList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, this.txtCheckListName.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY)
                        {
                            this.grvConditionPMList.SetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_NAME, this.cmbNotifyUser.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY)
                        {
                            this.grvConditionPMList.SetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_NAME, this.cmbNotifyCCUser.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY)
                        {
                            this.grvConditionPMList.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, this.cmbEquipmentChangeState.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY)
                        {
                            this.grvConditionPMList.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME, this.cmbEquipmentChangeReason.Text);
                        }
                    }

                    this.grvConditionPMList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvConditionPMList.DeleteRow(this.grvConditionPMList.FocusedRowHandle);

                    this.grvConditionPMList.UpdateCurrentRow();
                    break;
            }

            this.grvConditionPMList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Condition PM Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-08 15:32:16
        private DataTable GetEmptyConditionPMDataTable()
        {
            DataTable dataTable = EMS_PM_CONDITION_FIELDS.CreateDataTable(true);

            dataTable.Columns.Add(EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_NAME);
            dataTable.Columns.Add(EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
            dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

            return dataTable;
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 15:11:03
        private void LoadEquipmentLocationData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentLocationData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                DataRow newRow = dataTable.NewRow();

                newRow[0] = -1;
                newRow[1] = string.Empty;

                dataTable.Rows.InsertAt(newRow, 0);

                this.cmbEquipmentLocation.Properties.DataSource = dataTable;
            }
            else
            {
                this.cmbEquipmentLocation.Properties.DataSource = null;

                MessageService.ShowError(msg);
            }
        }

        /// <summary>
        /// Load Equipment Group Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 15:11:09
        private void LoadEquipmentGroupData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentGroupData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                DataRow newRow = dataTable.NewRow();

                newRow[0] = -1;
                newRow[1] = string.Empty;

                dataTable.Rows.InsertAt(newRow, 0);

                this.cmbEquipmentGroup.Properties.DataSource = dataTable;
            }
            else
            {
                this.cmbEquipmentGroup.Properties.DataSource = null;

                MessageService.ShowError(msg);
            }
        }

        /// <summary>
        /// Load Equipment Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 15:11:14
        private void LoadEquipmentData()
        {
            string msg;

            string equipmentLocationKey = string.Empty;
            string equipmentGroupKey = string.Empty;

            if (this.cmbEquipmentLocation.Text.Length > 0)
            {
                equipmentLocationKey = this.cmbEquipmentLocation.EditValue.ToString();
            }

            if (this.cmbEquipmentGroup.Text.Length > 0)
            {
                equipmentGroupKey = this.cmbEquipmentGroup.EditValue.ToString();
            }

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentData(equipmentLocationKey, equipmentGroupKey, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                DataRow newRow = dataTable.NewRow();

                newRow[0] = -1;
                newRow[1] = string.Empty;

                dataTable.Rows.InsertAt(newRow, 0);

                this.cmbEquipment.Properties.DataSource = dataTable;
            }
            else
            {
                this.cmbEquipment.Properties.DataSource = null;

                MessageService.ShowError(msg);
            }
        }

        /// <summary>
        /// Load Notify Users Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 15:11:24
        private void LoadNotifyUsersData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadUsersData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.cmbNotifyUser.Properties.DataSource = dataTable;
                this.cmbNotifyCCUser.Properties.DataSource = dataTable;
            }
            else
            {
                this.cmbNotifyUser.Properties.DataSource = null;
                this.cmbNotifyCCUser.Properties.DataSource = null;

                MessageService.ShowError(msg);
            }
        }

        /// <summary>
        /// Load Equipment Change State Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 15:11:32
        private void LoadEquipmentChangeStateData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentChangeStateData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                DataRow newRow = dataTable.NewRow();

                newRow[0] = -1;
                newRow[1] = string.Empty;

                dataTable.Rows.InsertAt(newRow, 0);

                this.cmbEquipmentChangeState.Properties.DataSource = dataTable;
            }
            else
            {
                this.cmbEquipmentChangeState.Properties.DataSource = null;

                MessageService.ShowError(msg);
            }
        }

        /// <summary>
        /// Load Equipment Change Reasons Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 15:11:37
        private void LoadEquipmentChangeReasonsData()
        {
            if (this.cmbEquipmentChangeState.Text.Length > 0)
            {
                string msg;

                DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentChangeReasonsData(this.cmbEquipmentChangeState.EditValue.ToString(), out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    DataRow newRow = dataTable.NewRow();

                    newRow[0] = -1;
                    newRow[1] = string.Empty;

                    dataTable.Rows.InsertAt(newRow, 0);

                    this.cmbEquipmentChangeReason.Properties.DataSource = dataTable;
                }
                else
                {
                    this.cmbEquipmentChangeReason.Properties.DataSource = null;

                    MessageService.ShowError(msg);
                }
            }
            else
            {
                this.cmbEquipmentChangeReason.Properties.DataSource = null;
            }
        }

        /// <summary>
        /// Load Condition PM Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-08 16:58:12
        private void LoadConditionPMData()
        {
            string msg;

            int pageNo, pageSize, pages, records;

            this.paginationConditionPM.GetPaginationProperties(out pageNo, out pageSize);

            string equipmentKey = string.Empty;

            if (this.cmbEquipment.Text.Length > 0)
            {
                equipmentKey = this.cmbEquipment.EditValue.ToString();
            }

            DataTable dataTable = this.equipmentConditionPMEntity.LoadConditionPMData(equipmentKey, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationConditionPM.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdConditionPMList.DataSource = dataTable;
            }
            else
            {
                this.grdConditionPMList.DataSource = GetEmptyConditionPMDataTable();

                MessageService.ShowError(msg);
            }

            this.grvConditionPMList.BestFitColumns();

            State = ControlState.Read;
        }

        #endregion

        #region Component Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadConditionPMData();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            State = ControlState.New;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (State == ControlState.New)
            {
                if (this.cmbEquipment.Text.Length == 0)
                {
                    MessageService.ShowMessage("请选择设备!");

                    this.cmbEquipment.Focus();

                    return;
                }
            }

            if (this.txtConditionPMName.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入条件PM名称!");

                this.txtConditionPMName.Focus();

                return;
            }

            if (this.txtCheckListName.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择检查表单名称!");

                this.txtCheckListName.Focus();

                return;
            }

            if (this.cmbCounterType.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择计数类型!");

                this.cmbCounterType.Focus();

                return;
            }

            if (this.txtCounterWarnning.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入计数警告值!");

                this.txtCounterWarnning.Focus();

                return;
            }

            if (Convert.ToInt32(this.txtCounterWarnning.EditValue) <= 0)
            {
                MessageService.ShowMessage("输入计数警告值必须大于零!");

                this.txtCounterWarnning.Focus();

                return;
            }

            if (this.txtCounterTarget.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入计数目标值!");

                this.txtCounterTarget.Focus();

                return;
            }

            if (Convert.ToInt32(this.txtCounterTarget.EditValue) <= 0)
            {
                MessageService.ShowMessage("输入计数目标值必须大于零!");

                this.txtCounterTarget.Focus();

                return;
            }

            if (Convert.ToInt32(this.txtCounterWarnning.EditValue) >= Convert.ToInt32(this.txtCounterTarget.EditValue))
            {
                MessageService.ShowMessage("输入计数警告值必须小于输入计数目标值!");

                this.txtCounterWarnning.Focus();

                return;
            }

            if (this.cmbNotifyUser.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择通知用户!");

                this.cmbNotifyUser.Focus();

                return;
            }

            MapControlsToEntity();

            if (State == ControlState.New)
            {
                if (this.equipmentConditionPMEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadConditionPMData();
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentConditionPMEntity.IsDirty)
                {
                    if (this.equipmentConditionPMEntity.Update())
                    {
                        MapEntityToControls();

                        MessageService.ShowMessage("更新数据成功!");

                        State = ControlState.Read;
                    }
                }
                else
                {
                    MessageService.ShowMessage("数据未修改!");
                }
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            State = ControlState.Read;
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (this.grvConditionPMList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除条件PM!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该条件PM吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentConditionPMEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadConditionPMData();
                }
            }
        }

        private void cmbEquipmentLocation_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private void cmbEquipmentGroup_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadConditionPMData();
        }

        private void txtCheckListName_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentCheckList", false))
            {
                if (queryDialog.ShowDialog() == DialogResult.OK)
                {
                    if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                    {
                        DataRow selectedRow = queryDialog.SelectedData[0];

                        this.txtCheckListName.Text = selectedRow[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].ToString();
                        this.txtCheckListName.Tag = selectedRow[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY].ToString();
                    }
                }
            }
        }

        private void cmbEquipmentChangeState_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentChangeReasonsData();
        }

        private void grdConditionPMList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvConditionPMList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                State = ControlState.Edit;
            }
        }

        private void grvConditionPMList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtConditionPMName.Text = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME).ToString();
                this.txtCheckListName.Text = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME).ToString();
                this.txtCheckListName.Tag = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                this.txtDescription.Text = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_DESCRIPTION).ToString();
                this.cmbCounterType.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TYPE);
                this.txtCounterWarnning.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_WARNNING);
                this.txtCounterTarget.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TARGET);
                this.txtCounterTotal.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_COUNTER_TOTAL);
                this.cmbNotifyUser.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_USER_KEY);
                this.cmbNotifyCCUser.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_NOTIFY_CC_USER_KEY);
                this.cmbEquipmentChangeState.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);
                this.cmbEquipmentChangeReason.EditValue = this.grvConditionPMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);
            }
            else
            {
                this.txtConditionPMName.Text = string.Empty;
                this.txtCheckListName.Text = string.Empty;
                this.txtCheckListName.Tag = string.Empty;
                this.txtDescription.Text = string.Empty;
                this.cmbCounterType.EditValue = string.Empty;
                this.txtCounterWarnning.EditValue = 0;
                this.txtCounterTarget.EditValue = 0;
                this.txtCounterTotal.EditValue = 0;
                this.cmbNotifyUser.EditValue = string.Empty;
                this.cmbNotifyCCUser.EditValue = string.Empty;
                this.cmbEquipmentChangeState.EditValue = string.Empty;
                this.cmbEquipmentChangeReason.EditValue = string.Empty;
            }
        }

        private void paginationConditionPM_DataPaging()
        {
            LoadConditionPMData();
        }

        #endregion
    }
}
