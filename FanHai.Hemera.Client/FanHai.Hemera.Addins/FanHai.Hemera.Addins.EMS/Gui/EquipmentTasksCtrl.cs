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
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentTasksCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListJobEntity Object

        EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

        #endregion

        #region EquipmentTaskEntity Object

        EquipmentTaskEntity equipmentTaskEntity = new EquipmentTaskEntity();

        #endregion

        #region Constructor

        public EquipmentTasksCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentTasksCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentTasksCtrl_afterStateChanged);

            LoadEquipmentLocationData();
            LoadEquipmentGroupData();
            LoadEquipmentData();
            LoadReceiveDeptsData();
            LoadEquipmentChangeStateData();
            LoadEquipmentChangeReasonsData();

            this.State = ControlState.Read;
        }

        private void EquipmentTasksCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtTaskName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.cmbReceiveDept.Enabled = false;
                    this.txtComments.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = false;
                    this.cmbEquipmentChangeReason.Enabled = false;

                    grvTaskList_FocusedRowChanged(this.grvTaskList, new FocusedRowChangedEventArgs(-1, this.grvTaskList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtTaskName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.cmbReceiveDept.Enabled = false;
                    this.txtComments.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = false;
                    this.cmbEquipmentChangeReason.Enabled = false;

                    grvTaskList_FocusedRowChanged(this.grvTaskList, new FocusedRowChangedEventArgs(-1, this.grvTaskList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtTaskName.Enabled = false;
                    this.txtDescription.Enabled = true;
                    this.cmbReceiveDept.Enabled = true;
                    this.txtComments.Enabled = true;
                    this.cmbEquipmentChangeState.Enabled = true;
                    this.cmbEquipmentChangeReason.Enabled = true;

                    this.txtTaskName.Text = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    this.cmbReceiveDept.EditValue = string.Empty;
                    this.txtComments.Text = string.Empty;
                    this.cmbEquipmentChangeState.EditValue = string.Empty;
                    this.cmbEquipmentChangeReason.EditValue = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtTaskName.Enabled = false;
                    this.txtDescription.Enabled = true;
                    this.cmbReceiveDept.Enabled = false;
                    this.txtComments.Enabled = false;
                    this.cmbEquipmentChangeState.Enabled = true;
                    this.cmbEquipmentChangeReason.Enabled = true;

                    grvTaskList_FocusedRowChanged(this.grvTaskList, new FocusedRowChangedEventArgs(-1, this.grvTaskList.FocusedRowHandle));
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

            #region Initial Receive Department ComboBox

            this.cmbReceiveDept.Properties.Columns.Clear();
            this.cmbReceiveDept.Properties.Columns.Add(new LookUpColumnInfo("DEPT_NAME"));
            this.cmbReceiveDept.Properties.ShowHeader = false;
            this.cmbReceiveDept.Properties.DisplayMember = "DEPT_NAME";
            this.cmbReceiveDept.Properties.ValueMember = "DEPT_KEY";

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

            #region Initial Equipment Task Grid

            DataTable dataTable = GetEmptyTaskDataTable();
            
            ControlUtils.InitialGridView(this.grvTaskList, dataTable);

            string msg;

            dataTable = this.equipmentCheckListJobEntity.LoadUsersData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                RepositoryItemGridLookUpEdit usersLookUpEdit = new RepositoryItemGridLookUpEdit();

                usersLookUpEdit.DisplayMember = RBAC_USER_FIELDS.FIELD_USERNAME;
                usersLookUpEdit.ValueMember = RBAC_USER_FIELDS.FIELD_USER_KEY;
                usersLookUpEdit.NullText = string.Empty;
                usersLookUpEdit.DataSource = dataTable;

                GridColumn gridColumn = this.grvTaskList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY);

                if (gridColumn != null)
                {
                    gridColumn.ColumnEdit = usersLookUpEdit;
                }

                gridColumn = this.grvTaskList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY);

                if (gridColumn != null)
                {
                    gridColumn.ColumnEdit = usersLookUpEdit;
                }

                gridColumn = this.grvTaskList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_USER_KEY);

                if (gridColumn != null)
                {
                    gridColumn.ColumnEdit = usersLookUpEdit;
                }
            }
            else
            {
                MessageService.ShowError(msg);
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
            //注释 by peter zhang 工具栏图标从当前项目中获取
            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //this.tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            //this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //this.tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");
            this.lblTitle.Text = "设备作业";

            this.grvTaskList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvTaskList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME].Caption = "作业名称";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE].Caption = "作业状态";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY].Caption = "创建用户";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].Caption = "创建时间";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY].Caption = "开始用户";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].Caption = "开始时间";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_USER_KEY].Caption = "完成用户";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].Caption = "完成时间";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].Caption = "设备转变状态名称";
            this.grvTaskList.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME].Caption = "设备转变原因名称";
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = "设备名称";

            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvTaskList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvTaskList.BestFitColumns();
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentTaskEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentTaskEntity.TaskKey =  CommonUtils.GenerateNewKey(0);
                    this.equipmentTaskEntity.EquipmentKey = this.cmbEquipment.EditValue.ToString();
                    this.equipmentTaskEntity.TaskState = "待处理";
                    this.equipmentTaskEntity.ReceiveDeptKey = this.cmbReceiveDept.EditValue.ToString();
                    this.equipmentTaskEntity.ReceiveDeptName = this.cmbReceiveDept.Text;
                    this.equipmentTaskEntity.Comments = this.txtComments.Text.Trim();
                    this.equipmentTaskEntity.CreateUserKey = PropertyService.Get(PROPERTY_FIELDS.USER_KEY);

                    this.equipmentTaskEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentTaskEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentTaskEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentTaskEntity.TaskKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY).ToString();
                    this.equipmentTaskEntity.Description = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION).ToString();
                    this.equipmentTaskEntity.EquipmentChangeStateKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();
                    this.equipmentTaskEntity.EquipmentChangeReasonKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY).ToString();

                    this.equipmentTaskEntity.Editor = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentTaskEntity.EditTimeZone = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentTaskEntity.EditTime = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentTaskEntity.IsInitializeFinished = true;

            this.equipmentTaskEntity.Description = this.txtDescription.Text.Trim();
            this.equipmentTaskEntity.EquipmentChangeStateKey = this.cmbEquipmentChangeState.Text.Length == 0 ? string.Empty : this.cmbEquipmentChangeState.EditValue.ToString();
            this.equipmentTaskEntity.EquipmentChangeReasonKey = this.cmbEquipmentChangeReason.Text.Length == 0 ? string.Empty : this.cmbEquipmentChangeReason.EditValue.ToString();
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    //TODO: Refresh Task Data

                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentTaskEntity.DirtyList)
                    {
                        this.grvTaskList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);

                        if (keyValue.Key == EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY)
                        {
                            this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, this.cmbEquipmentChangeState.Text);
                        }

                        if (keyValue.Key == EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY)
                        {
                            this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME, this.cmbEquipmentChangeReason.Text);
                        }
                    }

                    if (!string.IsNullOrEmpty(this.equipmentTaskEntity.CreateTimeStamp))
                    {
                        this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP, this.equipmentTaskEntity.CreateTimeStamp);
                    }

                    this.grvTaskList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvTaskList.DeleteRow(this.grvTaskList.FocusedRowHandle);

                    this.grvTaskList.UpdateCurrentRow();
                    break;
            }

            this.grvTaskList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Task Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-18 08:22:50
        private DataTable GetEmptyTaskDataTable()
        {
            DataTable dataTable = EMS_EQUIPMENT_TASKS_FIELDS.CreateDataTable(true);

            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

            return dataTable;
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-17 14:28:21
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
        /// Owner:Andy Gao 2011-08-17 14:28:27
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
        /// Owner:Andy Gao 2011-08-17 14:28:35
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
        /// Load Receive Departments Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-17 14:59:18
        private void LoadReceiveDeptsData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadDeptsData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.cmbReceiveDept.Properties.DataSource = dataTable;
            }
            else
            {
                this.cmbReceiveDept.Properties.DataSource = null;

                MessageService.ShowError(msg);
            }
        }

        /// <summary>
        /// Load Equipment Change State Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-17 14:29:38
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
        /// Owner:Andy Gao 2011-08-17 14:29:41
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
        /// Load Task Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-18 14:51:46
        private void LoadTaskData()
        {
            string msg;

            int pageNo, pageSize, pages, records;

            this.paginationTask.GetPaginationProperties(out pageNo, out pageSize);

            string equipmentKey = string.Empty;

            if (this.cmbEquipment.Text.Length > 0)
            {
                equipmentKey = this.cmbEquipment.EditValue.ToString();
            }

            DataTable dataTable = this.equipmentTaskEntity.LoadTaskData(equipmentKey, string.Empty, string.Empty, string.Empty, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationTask.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdTaskList.DataSource = dataTable;
            }
            else
            {
                this.grdTaskList.DataSource = GetEmptyTaskDataTable();

                MessageService.ShowError(msg);
            }

            this.grvTaskList.BestFitColumns();

            State = ControlState.Read;
        }

        #endregion

        #region Component Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadTaskData();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.State = ControlState.New;
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

            if (this.cmbReceiveDept.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择接收部门!");

                this.cmbReceiveDept.Focus();

                return;
            }

            MapControlsToEntity();

            if (State == ControlState.New)
            {
                if (this.equipmentTaskEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadTaskData();
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentTaskEntity.IsDirty)
                {
                    if (this.equipmentTaskEntity.Update())
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
            if (this.grvTaskList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除任务!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该任务吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentTaskEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadTaskData();
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
            LoadTaskData();
        }

        private void cmbEquipmentChangeState_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentChangeReasonsData();
        }

        private void grdTaskList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvTaskList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                //State = ControlState.Edit;
            }
        }

        private void grvTaskList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtTaskName.Text = this.grvTaskList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME).ToString();
                this.txtDescription.Text = this.grvTaskList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION).ToString();
                this.cmbReceiveDept.EditValue = string.Empty;
                this.txtComments.Text = string.Empty;
                this.cmbEquipmentChangeState.EditValue = this.grvTaskList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);
                this.cmbEquipmentChangeReason.EditValue = this.grvTaskList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);
            }
            else
            {
                this.txtTaskName.Text = string.Empty;
                this.txtDescription.Text = string.Empty;
                this.cmbReceiveDept.EditValue = string.Empty;
                this.txtComments.Text = string.Empty;
                this.cmbEquipmentChangeState.EditValue = string.Empty;
                this.cmbEquipmentChangeReason.EditValue = string.Empty;
            }
        }

        private void paginationTask_DataPaging()
        {
            LoadTaskData();
        }

        #endregion
    }
}
