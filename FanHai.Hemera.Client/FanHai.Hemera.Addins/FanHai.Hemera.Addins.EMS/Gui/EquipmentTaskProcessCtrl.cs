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
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentTaskProcessCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListJobEntity Object

        EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

        #endregion

        #region EquipmentTaskEntity Object

        EquipmentTaskEntity equipmentTaskEntity = new EquipmentTaskEntity();

        #endregion

        #region Constructor

        public EquipmentTaskProcessCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentTaskProcessCtrl_Load(object sender, EventArgs e)
        {
            LoadEquipmentLocationData();
            LoadEquipmentGroupData();
            LoadEquipmentData();
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

            #region Initial Task State ComboBox

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("TASK_STATE");

            dataTable.Rows.Add(string.Empty);
            dataTable.Rows.Add("待处理");
            dataTable.Rows.Add("已处理");

            dataTable.AcceptChanges();

            this.cmbTaskState.Properties.Columns.Clear();
            this.cmbTaskState.Properties.Columns.Add(new LookUpColumnInfo("TASK_STATE"));
            this.cmbTaskState.Properties.ShowHeader = false;
            this.cmbTaskState.Properties.DisplayMember = "TASK_STATE";
            this.cmbTaskState.Properties.ValueMember = "TASK_STATE";

            this.cmbTaskState.Properties.DataSource = dataTable;

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

            #region Initial Receive Department ComboBox

            this.cmbReceiveDept.Properties.Columns.Clear();
            this.cmbReceiveDept.Properties.Columns.Add(new LookUpColumnInfo("DEPT_NAME"));
            this.cmbReceiveDept.Properties.ShowHeader = false;
            this.cmbReceiveDept.Properties.DisplayMember = "DEPT_NAME";
            this.cmbReceiveDept.Properties.ValueMember = "DEPT_KEY";

            DataTable deptsDataTable = GetDeptsData();

            this.cmbReceiveDept.Properties.DataSource = deptsDataTable;

            #endregion

            #region Initial Equipment Task Grid

            dataTable = GetEmptyTaskDataTable();

            ControlUtils.InitialGridView(this.grvTaskList, dataTable);

            RepositoryItemGridLookUpEdit usersLookUpEdit = new RepositoryItemGridLookUpEdit();

            usersLookUpEdit.DisplayMember = RBAC_USER_FIELDS.FIELD_USERNAME;
            usersLookUpEdit.ValueMember = RBAC_USER_FIELDS.FIELD_USER_KEY;
            usersLookUpEdit.NullText = string.Empty;
            usersLookUpEdit.DataSource = GetUsersData();

            GridColumn gridColumn = null;

            gridColumn = this.grvTaskList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY);

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

            #endregion

            #region Initial Equipment Part Grid

            dataTable = GetEmptyPartDataTable();

            ControlUtils.InitialGridView(this.grvPartList, dataTable);

            #endregion

            #region Initial Equipment Task Log Grid

            dataTable = GetEmptyTaskCourseDataTable();

            ControlUtils.InitialGridView(this.grvTaskLogList, dataTable);

            gridColumn = this.grvTaskLogList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY);

            if (gridColumn != null)
            {
                gridColumn.ColumnEdit = usersLookUpEdit;
            }

            gridColumn = this.grvTaskLogList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY);

            if (gridColumn != null)
            {
                gridColumn.ColumnEdit = usersLookUpEdit;
            }

            gridColumn = this.grvTaskLogList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_USER_KEY);

            if (gridColumn != null)
            {
                gridColumn.ColumnEdit = usersLookUpEdit;
            }

            gridColumn = this.grvTaskLogList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY);

            if (gridColumn != null)
            {
                RepositoryItemGridLookUpEdit deptsLookUpEdit = new RepositoryItemGridLookUpEdit();

                deptsLookUpEdit.DisplayMember = "DEPT_NAME";
                deptsLookUpEdit.ValueMember = "DEPT_KEY";
                deptsLookUpEdit.NullText = string.Empty;
                deptsLookUpEdit.DataSource = deptsDataTable;

                gridColumn.ColumnEdit = deptsLookUpEdit;
            }

            gridColumn = this.grvTaskLogList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_NOTES);

            if (gridColumn != null)
            {
                RepositoryItemMemoEdit notesMemoEdit = new RepositoryItemMemoEdit();

                notesMemoEdit.MaxLength = 4000;

                gridColumn.ColumnEdit = notesMemoEdit;
            }

            #endregion

            #region Initial Equipment Task Processing

            this.btnForward.Enabled = false;
            this.btnProcessing.Enabled = false;

            this.cmbEquipmentChangeState.Enabled = false;
            this.cmbEquipmentChangeReason.Enabled = false;
            this.txtNotes.Enabled = false;
            this.cmbReceiveDept.Enabled = false;
            this.txtComments.Enabled = false;

            this.txtPartName.Enabled = false;
            this.txtPartQty.Enabled = false;
            this.btnAdd.Enabled = false;
            this.btnRemove.Enabled = false;

            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;

            #endregion
        }

        protected override void InitUIResourcesByCulture()
        {

            this.lblTitle.Text = "设备作业处理";

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

            this.grvPartList.Columns[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_PART_KEY].Visible = false;
            this.grvPartList.Columns[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_KEY].Visible = false;
            this.grvPartList.Columns[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_PART_KEY].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME].Caption = "备件名称";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE].Caption = "备件类型";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE].Caption = "备件型号";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT].Caption = "备件单位";
            this.grvPartList.Columns[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_QUANTITY].Caption = "备件数量";

            this.grvPartList.BestFitColumns();

            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY].Visible = false;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_KEY].Visible = false;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_STATE].Caption = "作业状态";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_USER_KEY].Caption = "发送用户";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_TIMESTAMP].Caption = "发送时间";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_SEND_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY].Caption = "接收部门";

            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY].Caption = "接收用户";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP].Caption = "接收时间";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;

            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_USER_KEY].Caption = "处理用户";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_TIMESTAMP].Caption = "处理时间";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;

            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_NOTES].Caption = "处理记录";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_REMARK].Caption = "备注";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS].Caption = "注释";

            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].Caption = "设备转变状态名称";
            this.grvTaskLogList.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME].Caption = "设备转变原因名称";

            this.grvTaskLogList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Task Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-24 13:45:27
        private DataTable GetEmptyTaskDataTable()
        {
            DataTable dataTable = EMS_EQUIPMENT_TASKS_FIELDS.CreateDataTable(false);

            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

            return dataTable;
        }

        /// <summary>
        /// Get Empty Part Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-25 08:48:52
        private DataTable GetEmptyPartDataTable()
        {
            DataTable dataTable = new DataTable(EMS_EQP_TASK_COURSE_PARTS_FIELDS.DATABASE_TABLE_NAME);

            dataTable.Columns.Add(EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_PART_KEY);
            dataTable.Columns.Add(EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_KEY);
            dataTable.Columns.Add(EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_PART_KEY);
            dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION);
            dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE);
            dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE);
            dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT);
            dataTable.Columns.Add(EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_QUANTITY, typeof(decimal));

            return dataTable;
        }

        /// <summary>
        /// Get Empty Task Course Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-25 12:51:51
        private DataTable GetEmptyTaskCourseDataTable()
        {
            DataTable dataTable = EMS_EQUIPMENT_TASK_COURSES_FIELDS.CreateDataTable();

            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);

            return dataTable;
        }

        /// <summary>
        /// Get Users Data
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-25 13:10:22
        private DataTable GetUsersData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadUsersData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return dataTable;
            }
            else
            {
                MessageService.ShowError(msg);

                return null;
            }
        }

        /// <summary>
        /// Get Departments Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-25 13:11:39
        private DataTable GetDeptsData()
        {
            string msg;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadDeptsData(out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return dataTable;
            }
            else
            {
                MessageService.ShowError(msg);

                return null;
            }
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-24 13:25:00
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
        /// Owner:Andy Gao 2011-08-24 13:25:07
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
        /// Owner:Andy Gao 2011-08-24 13:25:14
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
        /// Load Equipment Change State Data
        /// </summary>
        /// <param name="equipmentChangeStateKey"></param>
        /// Owner:Andy Gao 2011-08-25 18:15:54
        private void LoadEquipmentChangeStateData(string equipmentChangeStateKey)
        {
            if (string.IsNullOrEmpty(equipmentChangeStateKey))
            {
                this.cmbEquipmentChangeState.Properties.DataSource = null;
            }
            else
            {
                string msg;

                DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentChangeStateData(equipmentChangeStateKey, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.cmbEquipmentChangeState.Properties.DataSource = dataTable;
                }
                else
                {
                    this.cmbEquipmentChangeState.Properties.DataSource = null;

                    MessageService.ShowError(msg);
                }
            }
        }

        /// <summary>
        /// Load Equipment Change Reasons Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-25 18:15:46
        private void LoadEquipmentChangeReasonsData()
        {
            if (this.cmbEquipmentChangeState.Text.Length > 0)
            {
                string msg;

                DataTable dataTable = this.equipmentCheckListJobEntity.LoadEquipmentChangeReasonsData(this.cmbEquipmentChangeState.EditValue.ToString(), out msg);

                if (string.IsNullOrEmpty(msg))
                {
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
        /// Owner:Andy Gao 2011-08-24 13:48:22
        private void LoadTaskData()
        {
            string msg;

            int pageNo, pageSize, pages, records;

            string equipmentKey = string.Empty;
            string taskKey = string.Empty;

            if (this.cmbEquipment.Text.Length > 0)
            {
                equipmentKey = this.cmbEquipment.EditValue.ToString();
            }

            if (this.txtTaskName.Text.Length > 0)
            {
                taskKey = this.txtTaskName.Tag.ToString();
            }

            string taskState = this.cmbTaskState.Text;

            pageNo = -1;
            pageSize = -1;

            DataTable dataTable = this.equipmentTaskEntity.LoadTaskData(equipmentKey, taskKey, string.Empty, taskState, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.grdTaskList.DataSource = dataTable;
            }
            else
            {
                this.grdTaskList.DataSource = GetEmptyTaskDataTable();

                MessageService.ShowError(msg);
            }

            this.grvTaskList.BestFitColumns();

            grvTaskList_FocusedRowChanged(this.grvTaskList, new FocusedRowChangedEventArgs(-1, this.grvTaskList.FocusedRowHandle));
        }

        /// <summary>
        /// Load Task Course Data
        /// </summary>
        /// <param name="taskKey"></param>
        /// Owner:Andy Gao 2011-08-25 14:19:45
        private void LoadTaskCourseData(string taskKey)
        {
            string msg;

            if (string.IsNullOrEmpty(taskKey))
            {
                this.grdTaskLogList.DataSource = GetEmptyTaskCourseDataTable();
            }
            else
            {
                DataTable dataTable = this.equipmentTaskEntity.LoadTaskCourseData(taskKey, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grdTaskLogList.DataSource = dataTable;
                }
                else
                {
                    this.grdTaskLogList.DataSource = GetEmptyTaskCourseDataTable();

                    MessageService.ShowError(msg);
                }
            }

            this.grvTaskLogList.BestFitColumns();

            grvTaskLogList_FocusedRowChanged(this.grvTaskLogList, new FocusedRowChangedEventArgs(-1, this.grvTaskLogList.FocusedRowHandle));
        }

        /// <summary>
        /// Load Task Course Part Data
        /// </summary>
        /// <param name="taskCourseKey"></param>
        /// Owner:Andy Gao 2011-08-25 14:44:35
        private void LoadTaskCoursePartData(string taskCourseKey)
        {
            string msg;

            if (string.IsNullOrEmpty(taskCourseKey))
            {
                this.grdPartList.DataSource = GetEmptyPartDataTable();
            }
            else
            {
                DataTable dataTable = this.equipmentTaskEntity.LoadTaskCoursePartData(taskCourseKey, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grdPartList.DataSource = dataTable;
                }
                else
                {
                    this.grdPartList.DataSource = GetEmptyPartDataTable();

                    MessageService.ShowError(msg);
                }
            }

            this.grvPartList.BestFitColumns();

            this.txtPartName.Text = string.Empty;
            this.txtPartName.Tag = null;

            this.txtPartQty.EditValue = decimal.Zero;
        }

        /// <summary>
        /// Update Task Start And Receive Data
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-08-29 10:57:01
        private bool UpdateTaskStartAndReceiveData()
        {
            string msg;

            #region Update Task Start Data

            if (this.grvTaskList.FocusedRowHandle >= 0)
            {
                if (this.grvTaskList.GetFocusedRowCellDisplayText(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY) == string.Empty &&
                    this.grvTaskList.GetFocusedRowCellDisplayText(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP) == string.Empty)
                {
                    string taskKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY).ToString();
                    string startUserKey = PropertyService.Get(PROPERTY_FIELDS.USER_KEY);
                    string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    string editTimezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    string startTimeStamp, editTime;

                    this.equipmentTaskEntity.UpdateTaskStartData(taskKey, startUserKey, editor, editTimezone, out startTimeStamp, out editTime, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY, startUserKey);
                        this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP, startTimeStamp);
                        this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR, editor);
                        this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimezone);
                        this.grvTaskList.SetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME, editTime);

                        this.grvTaskList.UpdateCurrentRow();
                        this.grvTaskList.BestFitColumns();
                    }
                    else
                    {
                        MessageService.ShowError(msg);

                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            #endregion

            #region Update Task Course Receive Data

            int rowCount = this.grvTaskLogList.RowCount;

            if (rowCount > 0)
            {
                int lastRowIndex = rowCount - 1;

                this.grvTaskLogList.FocusedRowHandle = lastRowIndex;

                if (this.grvTaskLogList.GetRowCellDisplayText(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY) == string.Empty &&
                    this.grvTaskLogList.GetRowCellDisplayText(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP) == string.Empty)
                {
                    string taskCourseKey = this.grvTaskLogList.GetRowCellValue(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY).ToString();
                    string receiveUserKey = PropertyService.Get(PROPERTY_FIELDS.USER_KEY);
                    string receiveTimeStamp;

                    this.equipmentTaskEntity.UpdateTaskCourseReceiveData(taskCourseKey, receiveUserKey, out receiveTimeStamp, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.grvTaskLogList.SetRowCellValue(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_USER_KEY, receiveUserKey);
                        this.grvTaskLogList.SetRowCellValue(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_TIMESTAMP, receiveTimeStamp);

                        this.grvTaskLogList.UpdateCurrentRow();
                        this.grvTaskLogList.BestFitColumns();
                    }
                    else
                    {
                        MessageService.ShowError(msg);

                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            #endregion

            return true;
        }

        #endregion

        #region Component Events

        private void cmbEquipmentLocation_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private void cmbEquipmentGroup_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private void txtTaskName_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentTask", false))
                {
                    if (queryDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                        {
                            DataRow selectedRow = queryDialog.SelectedData[0];

                            this.txtTaskName.Text = selectedRow[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME].ToString();
                            this.txtTaskName.Tag = selectedRow[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].ToString();
                        }
                    }
                }
            }
            else if (e.Button.Kind == ButtonPredefines.Delete)
            {
                this.txtTaskName.Text = string.Empty;
                this.txtTaskName.Tag = string.Empty;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTaskData();
        }

        private void grvTaskList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string taskKey = string.Empty;

            if (e.FocusedRowHandle >= 0)
            {
                taskKey = this.grvTaskList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY).ToString();
            }

            LoadTaskCourseData(taskKey);
        }

        private void cmbEquipmentChangeState_EditValueChanged(object sender, EventArgs e)
        {
            LoadEquipmentChangeReasonsData();
        }

        private void txtPartName_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentPart", false))
                {
                    if (queryDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                        {
                            DataRow selectedRow = queryDialog.SelectedData[0];

                            this.txtPartName.Text = selectedRow[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME].ToString();
                            this.txtPartName.Tag = selectedRow.ItemArray;
                        }
                    }
                }
            }
            else if (e.Button.Kind == ButtonPredefines.Delete)
            {
                this.txtPartName.Text = string.Empty;
                this.txtPartName.Tag = null;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.txtPartName.Text.Length > 0)
            {
                decimal partQty = Convert.ToDecimal(this.txtPartQty.EditValue);

                if (partQty == decimal.Zero)
                {
                    MessageService.ShowMessage("备件数量不能为零!");

                    this.txtPartQty.Focus();
                }
                else
                {
                    object[] partItems = this.txtPartName.Tag as object[];

                    if (partItems != null && partItems.Length > 0)
                    {
                        DataTable dataTable = this.grdPartList.DataSource as DataTable;

                        if (dataTable != null)
                        {
                            int rowCount = this.grvTaskLogList.RowCount;

                            if (rowCount > 0)
                            {
                                int lastRowIndex = rowCount - 1;

                                string taskCourseKey = this.grvTaskLogList.GetRowCellValue(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY).ToString();

                                DataRow dataRow = dataTable.NewRow();

                                dataRow[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_PART_KEY] =  CommonUtils.GenerateNewKey(0);
                                dataRow[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_TASK_COURSE_KEY] = taskCourseKey;
                                dataRow[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_EQP_PART_KEY] = partItems[1];
                                dataRow[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME] = partItems[2];
                                dataRow[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION] = partItems[3];
                                dataRow[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE] = partItems[4];
                                dataRow[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE] = partItems[5];
                                dataRow[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT] = partItems[6];
                                dataRow[EMS_EQP_TASK_COURSE_PARTS_FIELDS.FIELD_QUANTITY] = this.txtPartQty.EditValue;

                                dataTable.Rows.Add(dataRow);

                                this.grvPartList.BestFitColumns();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageService.ShowMessage("请选择备件名称!");

                this.txtPartName.Focus();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.grvPartList.FocusedRowHandle >= 0)
            {
                if (MessageService.AskQuestion("确认要移除该备件吗?"))
                {
                    this.grvPartList.DeleteRow(this.grvPartList.FocusedRowHandle);
                }
            }
        }

        private void grvTaskLogList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            string taskState = string.Empty;
            string taskCourseKey = string.Empty;

            if (this.grvTaskList.FocusedRowHandle >= 0)
            {
                taskState = this.grvTaskList.GetRowCellValue(this.grvTaskList.FocusedRowHandle, EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE).ToString();
            }

            if (e.FocusedRowHandle >= 0)
            {
                taskCourseKey = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY).ToString();

                string equipmentChangeStateKey = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();
                string equipmentChangeStateName = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME).ToString();

                DataTable dataTable = null;

                if (string.IsNullOrEmpty(equipmentChangeStateKey))
                {
                    this.cmbEquipmentChangeState.Properties.DataSource = null;
                }
                else
                {
                    dataTable = new DataTable(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME);

                    dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);
                    dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);

                    dataTable.Rows.Add(equipmentChangeStateKey, equipmentChangeStateName);

                    this.cmbEquipmentChangeState.Properties.DataSource = dataTable;
                    this.cmbEquipmentChangeState.EditValue = equipmentChangeStateKey;
                    this.cmbEquipmentChangeState.Tag = equipmentChangeStateKey;
                }

                string equipmentChangeReasonKey = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY).ToString();
                string equipmentChangeReasonName = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME).ToString();

                if (string.IsNullOrEmpty(equipmentChangeReasonKey))
                {
                    this.cmbEquipmentChangeReason.Properties.DataSource = null;
                }
                else
                {
                    dataTable = new DataTable(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME);

                    dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);
                    dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);

                    dataTable.Rows.Add(equipmentChangeReasonKey, equipmentChangeReasonName);

                    this.cmbEquipmentChangeReason.Properties.DataSource = dataTable;
                    this.cmbEquipmentChangeReason.EditValue = equipmentChangeReasonKey;
                    this.cmbEquipmentChangeReason.Tag = equipmentChangeReasonKey;
                }

                this.txtNotes.Text = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_HANDLE_NOTES).ToString();
                this.cmbReceiveDept.EditValue = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_RECEIVE_DEPT_KEY).ToString();
                this.txtComments.Text = this.grvTaskLogList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_COMMENTS).ToString();

                if (taskState == "待处理")
                {
                    this.btnForward.Enabled = true;
                    this.btnProcessing.Enabled = true;
                }
                else
                {
                    this.btnForward.Enabled = false;
                    this.btnProcessing.Enabled = false;
                }
            }
            else
            {
                this.cmbEquipmentChangeState.Properties.DataSource = null;
                this.cmbEquipmentChangeReason.Properties.DataSource = null;
                this.txtNotes.Text = string.Empty;
                this.cmbReceiveDept.EditValue = string.Empty;
                this.txtComments.Text = string.Empty;

                this.btnForward.Enabled = false;
                this.btnProcessing.Enabled = false;
            }

            this.cmbEquipmentChangeState.Enabled = false;
            this.cmbEquipmentChangeReason.Enabled = false;
            this.txtNotes.Enabled = false;
            this.cmbReceiveDept.Enabled = false;
            this.txtComments.Enabled = false;

            this.txtPartName.Enabled = false;
            this.txtPartQty.Enabled = false;
            this.btnAdd.Enabled = false;
            this.btnRemove.Enabled = false;

            LoadTaskCoursePartData(taskCourseKey);

            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (UpdateTaskStartAndReceiveData())
            {
                this.btnForward.Enabled = false;
                this.btnProcessing.Enabled = false;

                this.cmbEquipmentChangeState.Enabled = false;
                this.cmbEquipmentChangeReason.Enabled = false;
                this.txtNotes.Enabled = false;
                this.cmbReceiveDept.Enabled = true;
                this.txtComments.Enabled = true;

                this.txtPartName.Enabled = false;
                this.txtPartQty.Enabled = false;
                this.btnAdd.Enabled = false;
                this.btnRemove.Enabled = false;

                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;

                this.btnSave.Tag = "FORWARD";
            }
        }

        private void btnProcessing_Click(object sender, EventArgs e)
        {
            if (UpdateTaskStartAndReceiveData())
            {
                string equipmentChangeStateKey = string.Empty;

                if (this.grvTaskList.FocusedRowHandle >= 0)
                {
                    equipmentChangeStateKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();
                }

                LoadEquipmentChangeStateData(equipmentChangeStateKey);
                LoadEquipmentChangeReasonsData();

                this.btnForward.Enabled = false;
                this.btnProcessing.Enabled = false;

                this.cmbEquipmentChangeState.Enabled = true;
                this.cmbEquipmentChangeReason.Enabled = true;
                this.txtNotes.Enabled = true;
                this.cmbReceiveDept.Enabled = true;
                this.txtComments.Enabled = true;

                this.txtPartName.Enabled = true;
                this.txtPartQty.Enabled = true;
                this.btnAdd.Enabled = true;
                this.btnRemove.Enabled = true;

                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;

                this.btnSave.Tag = "PROCESSING";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string taskKey = string.Empty;
            string taskName = string.Empty;
            string equipmentKey = string.Empty;
            string taskCourseKey = string.Empty;
            string equipmentChangeStateKey = string.Empty;
            string equipmentChangeReasonKey = string.Empty;
            string notes = string.Empty;
            string receiveDeptKey = string.Empty;
            string receiveDeptName = string.Empty;
            string comments = string.Empty;
            string userKey = string.Empty;
            string userName = string.Empty;
            string userTimezone = string.Empty;
            string msg;

            if (this.grvTaskList.FocusedRowHandle >= 0)
            {
                taskKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY).ToString();
                taskName = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME).ToString();
                equipmentKey = this.grvTaskList.GetFocusedRowCellValue(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY).ToString();
            }
            else
            {
                return;
            }

            DataTable equipmentChangeStateDataTable = this.cmbEquipmentChangeState.Properties.DataSource as DataTable;

            if (equipmentChangeStateDataTable != null && equipmentChangeStateDataTable.Rows.Count > 0)
            {
                if (this.cmbEquipmentChangeState.Text.Length == 0)
                {
                    MessageService.ShowMessage("请选择设备转变状态!");

                    this.cmbEquipmentChangeState.Focus();

                    return;
                }
            }

            if (this.cmbReceiveDept.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择接收部门!");

                this.cmbReceiveDept.Focus();

                return;
            }

            int rowCount = this.grvTaskLogList.RowCount;

            if (rowCount > 0)
            {
                int lastRowIndex = rowCount - 1;

                taskCourseKey = this.grvTaskLogList.GetRowCellValue(lastRowIndex, EMS_EQUIPMENT_TASK_COURSES_FIELDS.FIELD_EQUIPMENT_TASK_COURSE_KEY).ToString();
                equipmentChangeStateKey = this.cmbEquipmentChangeState.Text.Length > 0 ? this.cmbEquipmentChangeState.EditValue.ToString() : string.Empty;
                equipmentChangeReasonKey = this.cmbEquipmentChangeReason.Text.Length > 0 ? this.cmbEquipmentChangeReason.EditValue.ToString() : string.Empty;
                notes = this.txtNotes.Text;
                receiveDeptKey = this.cmbReceiveDept.EditValue.ToString();
                receiveDeptName = this.cmbReceiveDept.Text;
                comments = this.txtComments.Text.Trim();
                userKey = PropertyService.Get(PROPERTY_FIELDS.USER_KEY);
                userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                userTimezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            }
            else
            {
                return;
            }

            string taskOperation = this.btnSave.Tag.ToString();

            if (taskOperation == "FORWARD")
            {
                this.equipmentTaskEntity.ForwardTaskData(taskName, taskCourseKey, receiveDeptKey, receiveDeptName, comments, userKey, userName, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    MessageService.ShowMessage("转交成功!");

                    grvTaskList_FocusedRowChanged(this.grvTaskList, new FocusedRowChangedEventArgs(-1, this.grvTaskList.FocusedRowHandle));
                }
                else
                {
                    MessageService.ShowError(msg);
                }
            }
            else if (taskOperation == "PROCESSING")
            {
                DataTable partsDataTable = this.grdPartList.DataSource as DataTable;

                this.equipmentTaskEntity.ProcessingTaskData(taskKey, taskName, equipmentKey, taskCourseKey, equipmentChangeStateKey, equipmentChangeReasonKey, notes,
                    receiveDeptKey, receiveDeptName, comments, userKey, userName, userTimezone, partsDataTable.Copy(), out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    MessageService.ShowMessage("处理成功!");

                    LoadTaskData();
                }
                else
                {
                    MessageService.ShowError(msg);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grvTaskLogList_FocusedRowChanged(this.grvTaskLogList, new FocusedRowChangedEventArgs(-1, this.grvTaskLogList.FocusedRowHandle));
        }

        #endregion
    }
}
