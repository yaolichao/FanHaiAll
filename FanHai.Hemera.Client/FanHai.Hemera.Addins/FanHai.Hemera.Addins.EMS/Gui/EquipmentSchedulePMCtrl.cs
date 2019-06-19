using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentSchedulePMCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListJobEntity Object

        EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

        #endregion

        #region EquipmentSchedulePMEntity Object

        EquipmentSchedulePMEntity equipmentSchedulePMEntity = new EquipmentSchedulePMEntity();

        #endregion

        #region Constructor

        public EquipmentSchedulePMCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentSchedulePMCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentSchedulePMCtrl_afterStateChanged);

            LoadEquipmentLocationData();
            LoadEquipmentGroupData();
            LoadEquipmentData();
            LoadNotifyUsersData();
            LoadEquipmentChangeStateData();
            LoadEquipmentChangeReasonsData();

            this.State = ControlState.Read;
        }

        private void EquipmentSchedulePMCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtSchedulePMName.Enabled = false;
                    this.txtCheckListName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtFrequence.Enabled = false;
                    this.cmbFrequenceUnit.Enabled = false;
                    this.cmbNotifyUser.Enabled = false;
                    this.cmbNotifyCCUser.Enabled = false;
                    this.txtNotifyAdvancedTime.Enabled = false;
                    this.dteNextEventTime.Enabled = false;
                    this.chkIsBaseActualFinishTime.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = false;
                    this.cmbEquipmentChangeReason.Enabled = false;

                    grvSchedulePMList_FocusedRowChanged(this.grvSchedulePMList, new FocusedRowChangedEventArgs(-1, this.grvSchedulePMList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtSchedulePMName.Enabled = false;
                    this.txtCheckListName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtFrequence.Enabled = false;
                    this.cmbFrequenceUnit.Enabled = false;
                    this.cmbNotifyUser.Enabled = false;
                    this.cmbNotifyCCUser.Enabled = false;
                    this.txtNotifyAdvancedTime.Enabled = false;
                    this.dteNextEventTime.Enabled = false;
                    this.chkIsBaseActualFinishTime.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = false;
                    this.cmbEquipmentChangeReason.Enabled = false;

                    grvSchedulePMList_FocusedRowChanged(this.grvSchedulePMList, new FocusedRowChangedEventArgs(-1, this.grvSchedulePMList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtSchedulePMName.Enabled = true;
                    this.txtCheckListName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.txtFrequence.Enabled = true;
                    this.cmbFrequenceUnit.Enabled = true;
                    this.cmbNotifyUser.Enabled = true;
                    this.cmbNotifyCCUser.Enabled = true;
                    this.txtNotifyAdvancedTime.Enabled = true;
                    this.dteNextEventTime.Enabled = false;
                    this.chkIsBaseActualFinishTime.Enabled = true;
                    this.cmbEquipmentChangeState.Enabled = true;
                    this.cmbEquipmentChangeReason.Enabled = true;

                    this.txtSchedulePMName.Text = string.Empty;
                    this.txtCheckListName.Text = string.Empty;
                    this.txtCheckListName.Tag = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    this.txtFrequence.EditValue = 0;
                    this.cmbFrequenceUnit.EditValue = string.Empty;
                    this.cmbNotifyUser.EditValue = string.Empty;
                    this.cmbNotifyCCUser.EditValue = string.Empty;
                    this.txtNotifyAdvancedTime.EditValue = 0.00f;
                    this.dteNextEventTime.EditValue = string.Empty;
                    this.chkIsBaseActualFinishTime.Checked = false;
                    this.cmbEquipmentChangeState.EditValue = string.Empty;
                    this.cmbEquipmentChangeReason.EditValue = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtSchedulePMName.Enabled = true;
                    this.txtCheckListName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.txtFrequence.Enabled = true;
                    this.cmbFrequenceUnit.Enabled = true;
                    this.cmbNotifyUser.Enabled = true;
                    this.cmbNotifyCCUser.Enabled = true;
                    this.txtNotifyAdvancedTime.Enabled = true;
                    this.dteNextEventTime.Enabled = false;
                    this.chkIsBaseActualFinishTime.Enabled = true;
                    this.cmbEquipmentChangeState.Enabled = true;
                    this.cmbEquipmentChangeReason.Enabled = true;

                    grvSchedulePMList_FocusedRowChanged(this.grvSchedulePMList, new FocusedRowChangedEventArgs(-1, this.grvSchedulePMList.FocusedRowHandle));
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

            #region Initial Frequence Unit ComboBox

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("FREQUENCE_UNIT");

            dataTable.Rows.Add("小时");
            dataTable.Rows.Add("天");
            dataTable.Rows.Add("周");
            dataTable.Rows.Add("月");
            dataTable.Rows.Add("年");

            dataTable.AcceptChanges();

            this.cmbFrequenceUnit.Properties.Columns.Clear();

            this.cmbFrequenceUnit.Properties.Columns.Add(new LookUpColumnInfo("FREQUENCE_UNIT"));

            this.cmbFrequenceUnit.Properties.ShowHeader = false;

            this.cmbFrequenceUnit.Properties.DisplayMember = "FREQUENCE_UNIT";
            this.cmbFrequenceUnit.Properties.ValueMember = "FREQUENCE_UNIT";

            this.cmbFrequenceUnit.Properties.DataSource = dataTable;

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

            #region Initial Equipment Schedule PM Grid

            dataTable = GetEmptySchedulePMDataTable();

            ControlUtils.InitialGridView(this.grvSchedulePMList, dataTable);

            GridColumn gridColumn = this.grvSchedulePMList.Columns.ColumnByFieldName(EMS_PM_SCHEDULE_FIELDS.FIELD_BASE_ACTUAL_FINISH_TIME);

            if (gridColumn != null)
            {
                RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);

                gridColumn.ColumnEdit = checkEdit;
            }

            #endregion
        }

        protected override void InitUIResourcesByCulture()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");

            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //this.tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            //this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //this.tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");
            this.lblTitle.Text = "设备计划PM";

            this.grvSchedulePMList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvSchedulePMList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME].Caption = "计划PM名称";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE].Caption = "频率";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE_UNIT].Caption = "频率单位";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_NAME].Caption = "通知用户";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_NAME].Caption = "通知抄送用户";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_ADVANCED_TIME].Caption = "通知提前时间(小时)";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME].Caption = "下一次PM时间";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_BASE_ACTUAL_FINISH_TIME].Caption = "是否基于实际PM完成时间计算";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].Caption = "设备转变状态名称";
            this.grvSchedulePMList.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME].Caption = "设备转变原因名称";
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = "设备名称";
            this.grvSchedulePMList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].Caption = "检查表单名称";

            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvSchedulePMList.Columns[EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvSchedulePMList.BestFitColumns();
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentSchedulePMEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentSchedulePMEntity.ScheduleKey =  CommonUtils.GenerateNewKey(0);
                    this.equipmentSchedulePMEntity.EquipmentKey = this.cmbEquipment.EditValue.ToString();

                    this.equipmentSchedulePMEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentSchedulePMEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentSchedulePMEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentSchedulePMEntity.ScheduleKey = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY).ToString();
                    this.equipmentSchedulePMEntity.ScheduleName = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME).ToString();
                    this.equipmentSchedulePMEntity.CheckListKey = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                    this.equipmentSchedulePMEntity.Description = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_DESCRIPTION).ToString();
                    this.equipmentSchedulePMEntity.Frequence = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE).ToString();
                    this.equipmentSchedulePMEntity.FrequenceUnit = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE_UNIT).ToString();
                    this.equipmentSchedulePMEntity.NotifyUserKey = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY).ToString();
                    this.equipmentSchedulePMEntity.NotifyCCUserKey = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY).ToString();
                    this.equipmentSchedulePMEntity.NotifyAdvancedTime = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_ADVANCED_TIME).ToString().TrimEnd('.');
                    this.equipmentSchedulePMEntity.IsBaseActualFinishTime = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_BASE_ACTUAL_FINISH_TIME).ToString();
                    this.equipmentSchedulePMEntity.EquipmentChangeStateKey = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();
                    this.equipmentSchedulePMEntity.EquipmentChangeReasonKey = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY).ToString();

                    this.equipmentSchedulePMEntity.Editor = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentSchedulePMEntity.EditTimeZone = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentSchedulePMEntity.EditTime = this.grvSchedulePMList.GetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentSchedulePMEntity.IsInitializeFinished = true;

            this.equipmentSchedulePMEntity.ScheduleName = this.txtSchedulePMName.Text.Trim();
            this.equipmentSchedulePMEntity.CheckListKey = this.txtCheckListName.Tag.ToString();
            this.equipmentSchedulePMEntity.Description = this.txtDescription.Text.Trim();
            this.equipmentSchedulePMEntity.Frequence = this.txtFrequence.Text.Trim();
            this.equipmentSchedulePMEntity.FrequenceUnit = this.cmbFrequenceUnit.Text;
            this.equipmentSchedulePMEntity.NotifyUserKey = this.cmbNotifyUser.EditValue.ToString();
            this.equipmentSchedulePMEntity.NotifyCCUserKey = this.cmbNotifyCCUser.EditValue.ToString();
            this.equipmentSchedulePMEntity.NotifyAdvancedTime = this.txtNotifyAdvancedTime.Text.TrimEnd('.');
            this.equipmentSchedulePMEntity.IsBaseActualFinishTime = this.chkIsBaseActualFinishTime.Checked ? "1" : "0";
            this.equipmentSchedulePMEntity.EquipmentChangeStateKey = this.cmbEquipmentChangeState.Text.Length == 0 ? string.Empty : this.cmbEquipmentChangeState.EditValue.ToString();
            this.equipmentSchedulePMEntity.EquipmentChangeReasonKey = this.cmbEquipmentChangeReason.Text.Length == 0 ? string.Empty : this.cmbEquipmentChangeReason.EditValue.ToString();
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    //TODO: Refresh Schedule PM Data
                    
                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentSchedulePMEntity.DirtyList)
                    {
                        this.grvSchedulePMList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY)
                        {
                            this.grvSchedulePMList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, this.txtCheckListName.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY)
                        {
                            this.grvSchedulePMList.SetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_NAME, this.cmbNotifyUser.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY)
                        {
                            this.grvSchedulePMList.SetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_NAME, this.cmbNotifyCCUser.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY)
                        {
                            this.grvSchedulePMList.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, this.cmbEquipmentChangeState.Text);
                        }

                        if (keyValue.Key == EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY)
                        {
                            this.grvSchedulePMList.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME, this.cmbEquipmentChangeReason.Text);
                        }
                    }

                    if (!string.IsNullOrEmpty(this.equipmentSchedulePMEntity.NextEventTime))
                    {
                        this.grvSchedulePMList.SetFocusedRowCellValue(EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME, this.equipmentSchedulePMEntity.NextEventTime);
                    }

                    this.grvSchedulePMList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvSchedulePMList.DeleteRow(this.grvSchedulePMList.FocusedRowHandle);

                    this.grvSchedulePMList.UpdateCurrentRow();
                    break;
            }

            this.grvSchedulePMList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Schedule PM Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-04 08:44:29
        private DataTable GetEmptySchedulePMDataTable()
        {
            DataTable dataTable = EMS_PM_SCHEDULE_FIELDS.CreateDataTable(true);

            dataTable.Columns.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_NAME);
            dataTable.Columns.Add(EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
            dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

            return dataTable;
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-03 15:15:11
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
        /// Owner:Andy Gao 2011-08-03 15:15:18
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
        /// Owner:Andy Gao 2011-08-03 15:15:23
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
        /// Owner:Andy Gao 2011-08-05 11:08:33
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
        /// Owner:Andy Gao 2011-08-03 15:29:08
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
        /// Owner:Andy Gao 2011-08-03 15:45:14
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
        /// Load Schedule PM Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-04 10:56:40
        private void LoadSchedulePMData()
        {
            string msg;

            int pageNo, pageSize, pages, records;

            this.paginationSchedulePM.GetPaginationProperties(out pageNo, out pageSize);

            string equipmentKey = string.Empty;

            if (this.cmbEquipment.Text.Length > 0)
            {
                equipmentKey = this.cmbEquipment.EditValue.ToString();
            }

            DataTable dataTable = this.equipmentSchedulePMEntity.LoadSchedulePMData(equipmentKey, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationSchedulePM.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdSchedulePMList.DataSource = dataTable;
            }
            else
            {
                this.grdSchedulePMList.DataSource = GetEmptySchedulePMDataTable();

                MessageService.ShowError(msg);
            }

            this.grvSchedulePMList.BestFitColumns();

            State = ControlState.Read;
        }

        #endregion

        #region Component Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadSchedulePMData();
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

            if (this.txtSchedulePMName.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入计划PM名称!");

                this.txtSchedulePMName.Focus();

                return;
            }

            if (this.txtCheckListName.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择检查表单名称!");

                this.txtCheckListName.Focus();

                return;
            }

            if (this.txtFrequence.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入频率!");

                this.txtFrequence.Focus();

                return;
            }

            if (Convert.ToInt32(this.txtFrequence.EditValue) <= 0)
            {
                MessageService.ShowMessage("输入频率必须大于零!");

                this.txtFrequence.Focus();

                return;
            }

            if (this.cmbFrequenceUnit.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择频率单位!");

                this.cmbFrequenceUnit.Focus();

                return;
            }

            if (this.cmbNotifyUser.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择通知用户!");

                this.cmbNotifyUser.Focus();

                return;
            }

            if (this.txtNotifyAdvancedTime.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入通知提前时间!");

                this.txtNotifyAdvancedTime.Focus();

                return;
            }

            if (Convert.ToSingle(this.txtNotifyAdvancedTime.EditValue) <= 0)
            {
                MessageService.ShowMessage("输入通知提前时间必须大于零!");

                this.txtFrequence.Focus();

                return;
            }

            MapControlsToEntity();

            if (State == ControlState.New)
            {
                if (this.equipmentSchedulePMEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadSchedulePMData();
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentSchedulePMEntity.IsDirty)
                {
                    if (this.equipmentSchedulePMEntity.Update())
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
            if (this.grvSchedulePMList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除计划PM!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该计划PM吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentSchedulePMEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadSchedulePMData();
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
            LoadSchedulePMData();
        }

        private void txtCheckListName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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

        private void grdSchedulePMList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvSchedulePMList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                State = ControlState.Edit;
            }
        }

        private void grvSchedulePMList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtSchedulePMName.Text = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_NAME).ToString();
                this.txtCheckListName.Text = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME).ToString();
                this.txtCheckListName.Tag = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                this.txtDescription.Text = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_DESCRIPTION).ToString();
                this.txtFrequence.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE);
                this.cmbFrequenceUnit.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_FREQUENCE_UNIT);
                this.cmbNotifyUser.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_USER_KEY);
                this.cmbNotifyCCUser.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_CC_USER_KEY);
                this.txtNotifyAdvancedTime.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_NOTIFY_ADVANCED_TIME);
                this.dteNextEventTime.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_NEXT_EVENT_TIME);
                this.chkIsBaseActualFinishTime.Checked = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_BASE_ACTUAL_FINISH_TIME).ToString() == "1";
                this.cmbEquipmentChangeState.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);
                this.cmbEquipmentChangeReason.EditValue = this.grvSchedulePMList.GetRowCellValue(e.FocusedRowHandle, EMS_PM_SCHEDULE_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);
            }
            else
            {
                this.txtSchedulePMName.Text = string.Empty;
                this.txtCheckListName.Text = string.Empty;
                this.txtCheckListName.Tag = string.Empty;
                this.txtDescription.Text = string.Empty;
                this.txtFrequence.EditValue = 0;
                this.cmbFrequenceUnit.EditValue = string.Empty;
                this.cmbNotifyUser.EditValue = string.Empty;
                this.cmbNotifyCCUser.EditValue = string.Empty;
                this.txtNotifyAdvancedTime.EditValue = 0.00f;
                this.dteNextEventTime.EditValue = string.Empty;
                this.chkIsBaseActualFinishTime.Checked = false;
                this.cmbEquipmentChangeState.EditValue = string.Empty;
                this.cmbEquipmentChangeReason.EditValue = string.Empty;
            }
        }

        private void paginationSchedulePM_DataPaging()
        {
            LoadSchedulePMData();
        }

        #endregion
    }
}
