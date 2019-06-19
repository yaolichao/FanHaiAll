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
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentCheckListJobCompleteCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListJobEntity Object

        EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

        #endregion

        #region Constructor

        public EquipmentCheckListJobCompleteCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentCheckListJobCompleteCtrl_Load(object sender, EventArgs e)
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

            #region Initial Check List Job State ComboBox

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("CHECKLIST_JOB_STATE");

            dataTable.Rows.Add(string.Empty);
            dataTable.Rows.Add("ACTIVED");
            dataTable.Rows.Add("STARTED");
            dataTable.Rows.Add("COMPLETED");

            dataTable.AcceptChanges();

            this.cmbCheckListJobState.Properties.Columns.Clear();
            this.cmbCheckListJobState.Properties.Columns.Add(new LookUpColumnInfo("CHECKLIST_JOB_STATE"));
            this.cmbCheckListJobState.Properties.ShowHeader = false;
            this.cmbCheckListJobState.Properties.DisplayMember = "CHECKLIST_JOB_STATE";
            this.cmbCheckListJobState.Properties.ValueMember = "CHECKLIST_JOB_STATE";

            this.cmbCheckListJobState.Properties.DataSource = dataTable;

            #endregion

            #region Initial Check List Job Grid

            dataTable = GetEmptyCheckListJobDataTable();

            ControlUtils.InitialGridView(this.grvCheckListJobList, dataTable);

            #endregion

            #region Initial Check List Job Data Grid

            dataTable = GetEmptyCheckListJobDataDataTable();

            ControlUtils.InitialGridView(this.grvCheckListJobItemList, dataTable);

            GridColumn gridColumn = this.grvCheckListJobItemList.Columns.ColumnByFieldName(EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL);

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
            this.lblTitle.Text = "设备检查表单任务完成";

            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME].Caption = "检查表单任务名称";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE].Caption = "检查表单任务状态";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].Caption = "创建时间";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].Caption = "开始时间";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].Caption = "完成时间";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_TYPE].Caption = "PM类型";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_KEY].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_NAME].Caption = "PM名称";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
            this.grvCheckListJobList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = "设备名称";
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].Caption = "检查表单名称";

            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvCheckListJobList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvCheckListJobList.BestFitColumns();

            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKLIST_JOB_KEY].Visible = false;
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_KEY].Visible = false;
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE].Caption = "序号";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE].Width = 35;
            this.grvCheckListJobItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].Caption = "检查项名称";
            this.grvCheckListJobItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE].Caption = "检查项类型";
            this.grvCheckListJobItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD].Caption = "参考标准";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL].Caption = "是否可选";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE].Caption = "检查项值";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK].Caption = "备注";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP].Caption = "完成时间";
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.grvCheckListJobItemList.Columns[EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;

            this.grvCheckListJobItemList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Check List Job DataTable
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-26 10:49:06
        private DataTable GetEmptyCheckListJobDataTable()
        {
            DataTable dataTable = EMS_CHECKLIST_JOBS_FIELDS.CreateDataTable(false);

            dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
            dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

            return dataTable;
        }

        /// <summary>
        /// Get Empty Check List Job Data DataTable
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-26 15:21:49
        private DataTable GetEmptyCheckListJobDataDataTable()
        {
            DataTable dataTable = new DataTable(EMS_CHECKLIST_JOB_DATA_FIELDS.DATABASE_TABLE_NAME);

            dataTable.Columns.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKLIST_JOB_KEY);
            dataTable.Columns.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_KEY);
            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE);

            dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME);
            dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE);
            dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION);

            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD);
            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL);

            dataTable.Columns.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE);
            dataTable.Columns.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK);
            dataTable.Columns.Add(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP);

            return dataTable;
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// Owner:Andy Gao 2011-07-26 08:41:53
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
        /// Owner:Andy Gao 2011-07-26 08:41:50
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
        /// Owner:Andy Gao 2011-07-26 08:41:40
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
        /// Load Check List Jobs Data
        /// </summary>
        /// Owner:Andy Gao 2011-07-21 08:31:36
        private void LoadCheckListJobsData()
        {
            string msg;

            int pageNo, pageSize, pages, records;

            string equipmentKey = string.Empty;
            string checkListJobKey = string.Empty;

            if (this.cmbEquipment.Text.Length > 0)
            {
                equipmentKey = this.cmbEquipment.EditValue.ToString();
            }

            if (this.txtCheckListJobName.Text.Length > 0)
            {
                checkListJobKey = this.txtCheckListJobName.Tag.ToString();
            }

            string checkListJobState = this.cmbCheckListJobState.Text;

            pageNo = -1;
            pageSize = -1;

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadCheckListJobsData(equipmentKey, checkListJobKey, string.Empty, checkListJobState, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.grdCheckListJobList.DataSource = dataTable;
            }
            else
            {
                this.grdCheckListJobList.DataSource = GetEmptyCheckListJobDataTable();

                MessageService.ShowError(msg);
            }

            this.grvCheckListJobList.BestFitColumns();

            grvCheckListJobList_FocusedRowChanged(this.grvCheckListJobList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobList.FocusedRowHandle));
        }

        /// <summary>
        /// Load Check List Job Data
        /// </summary>
        /// <param name="checkListJobKey"></param>
        /// Owner:Andy Gao 2011-07-26 15:33:45
        private void LoadCheckListJobData(string checkListJobKey)
        {
            string msg;

            if (string.IsNullOrEmpty(checkListJobKey))
            {
                this.grdCheckListJobItemList.DataSource = GetEmptyCheckListJobDataDataTable();
            }
            else
            {
                DataTable dataTable = this.equipmentCheckListJobEntity.LoadCheckListJobData(checkListJobKey, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grdCheckListJobItemList.DataSource = dataTable;
                }
                else
                {
                    this.grdCheckListJobItemList.DataSource = GetEmptyCheckListJobDataDataTable();

                    MessageService.ShowError(msg);
                }
            }

            this.grvCheckListJobItemList.BestFitColumns();

            grvCheckListJobItemList_FocusedRowChanged(this.grvCheckListJobItemList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobItemList.FocusedRowHandle));
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

        private void txtCheckListJobName_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentCheckListJob", false))
                {
                    if (queryDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                        {
                            DataRow selectedRow = queryDialog.SelectedData[0];

                            this.txtCheckListJobName.Text = selectedRow[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME].ToString();
                            this.txtCheckListJobName.Tag = selectedRow[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].ToString();
                        }
                    }
                }
            }
            else if (e.Button.Kind == ButtonPredefines.Delete)
            {
                this.txtCheckListJobName.Text = string.Empty;
                this.txtCheckListJobName.Tag = string.Empty;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadCheckListJobsData();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.grvCheckListJobList.FocusedRowHandle >= 0)
            {
                string checkListJobKey = this.grvCheckListJobList.GetRowCellValue(this.grvCheckListJobList.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY).ToString();
                string checkListKey = this.grvCheckListJobList.GetRowCellValue(this.grvCheckListJobList.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                string editTime = this.grvCheckListJobList.GetRowCellValue(this.grvCheckListJobList.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME).ToString();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                string startTimeStamp, msg;

                this.equipmentCheckListJobEntity.StartCheckListJob(checkListJobKey, checkListKey, editor, editTimeZone, ref editTime, out startTimeStamp, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE, "STARTED");
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP, startTimeStamp);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR, editor);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME, editTime);
                    this.grvCheckListJobList.UpdateCurrentRow();
                    this.grvCheckListJobList.BestFitColumns();

                    grvCheckListJobList_FocusedRowChanged(this.grvCheckListJobList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobList.FocusedRowHandle));
                }
                else
                {
                    MessageService.ShowError(msg);
                }
            }
            else
            {
                MessageService.ShowMessage("请选择需要开始的检查表单任务!");
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            int rowCount = this.grvCheckListJobItemList.RowCount;

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                if (this.grvCheckListJobItemList.GetRowCellDisplayText(rowIndex, EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP).Length == 0)
                {
                    MessageService.ShowMessage("该检查表单任务存在未完成项,请完成后再提交!");

                    this.grvCheckListJobItemList.FocusedRowHandle = rowIndex;

                    grvCheckListJobItemList_FocusedRowChanged(this.grvCheckListJobItemList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobItemList.FocusedRowHandle));

                    return;
                }
            }

            if (this.grvCheckListJobList.FocusedRowHandle >= 0)
            {
                string checkListJobKey = this.grvCheckListJobList.GetRowCellValue(this.grvCheckListJobList.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY).ToString();
                string editTime = this.grvCheckListJobList.GetRowCellValue(this.grvCheckListJobList.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME).ToString();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                string completeTimeStamp, msg;

                this.equipmentCheckListJobEntity.CompleteCheckListJob(checkListJobKey, editor, editTimeZone, ref editTime, out completeTimeStamp, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE, "COMPLETED");
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP, completeTimeStamp);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR, editor);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME, editTime);
                    this.grvCheckListJobList.UpdateCurrentRow();
                    this.grvCheckListJobList.BestFitColumns();

                    grvCheckListJobList_FocusedRowChanged(this.grvCheckListJobList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobList.FocusedRowHandle));
                }
                else
                {
                    MessageService.ShowError(msg);
                }
            }
            else
            {
                MessageService.ShowMessage("请选择需要完成的检查表单任务!");
            }
        }

        private void grvCheckListJobList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string checkListJobKey = string.Empty;
            string checkListJobState = string.Empty;

            if (e.FocusedRowHandle >= 0)
            {
                checkListJobKey = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY).ToString();
                this.txtEquipment.Text = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME).ToString();
                this.txtCheckListJob.Text = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME).ToString();
                checkListJobState = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE).ToString();
                this.txtCheckListJobState.Text = checkListJobState;
                this.txtCheckList.Text = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME).ToString();
            }
            else
            {
                this.txtEquipment.Text = string.Empty;
                this.txtCheckListJob.Text = string.Empty;
                this.txtCheckListJobState.Text = string.Empty;
                this.txtCheckList.Text = string.Empty;
            }

            switch (checkListJobState)
            {
                case "ACTIVED":
                    this.btnStart.Enabled = true;
                    this.btnComplete.Enabled = false;

                    this.txtCheckListItemValue.Enabled = false;
                    this.txtRemark.Enabled = false;

                    this.btnOK.Enabled = false;
                    this.btnCancel.Enabled = false;
                    break;
                case "STARTED":
                    this.btnStart.Enabled = false;
                    this.btnComplete.Enabled = true;

                    this.txtCheckListItemValue.Enabled = true;
                    this.txtRemark.Enabled = true;

                    this.btnOK.Enabled = true;
                    this.btnCancel.Enabled = true;
                    break;
                case "COMPLETED":
                    this.btnStart.Enabled = false;
                    this.btnComplete.Enabled = false;

                    this.txtCheckListItemValue.Enabled = false;
                    this.txtRemark.Enabled = false;

                    this.btnOK.Enabled = false;
                    this.btnCancel.Enabled = false;
                    break;
                default:
                    this.btnStart.Enabled = false;
                    this.btnComplete.Enabled = false;

                    this.txtCheckListItemValue.Enabled = false;
                    this.txtRemark.Enabled = false;

                    this.btnOK.Enabled = false;
                    this.btnCancel.Enabled = false;
                    break;
            }

            LoadCheckListJobData(checkListJobKey);
        }

        private void grvCheckListJobItemList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.txtCheckListItemValue.Properties.Mask.BeepOnError = false;
            this.txtCheckListItemValue.Properties.Mask.EditMask = string.Empty;
            this.txtCheckListItemValue.Properties.Mask.IgnoreMaskBlank = true;
            this.txtCheckListItemValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.txtCheckListItemValue.Properties.Mask.ShowPlaceHolders = true;
            this.txtCheckListItemValue.Properties.Mask.UseMaskAsDisplayFormat = false;
            this.txtCheckListItemValue.Properties.NullText = string.Empty;

            if (e.FocusedRowHandle >= 0)
            {
                this.txtCheckListItem.Text = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME).ToString();
                this.txtCheckListItemDesc.Text = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION).ToString();
                this.txtStandard.Text = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD).ToString();
                this.txtOptional.Checked = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL).ToString() == "1";

                string checkItemType = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE).ToString();

                switch (checkItemType)
                {
                    case "STRING":
                        //TODO: Use Default Setting
                        break;
                    case "DATA":
                        this.txtCheckListItemValue.Properties.Mask.BeepOnError = false;
                        this.txtCheckListItemValue.Properties.Mask.EditMask = "f2";
                        this.txtCheckListItemValue.Properties.Mask.IgnoreMaskBlank = false;
                        this.txtCheckListItemValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                        this.txtCheckListItemValue.Properties.Mask.ShowPlaceHolders = true;
                        this.txtCheckListItemValue.Properties.Mask.UseMaskAsDisplayFormat = true;
                        this.txtCheckListItemValue.Properties.NullText = "0.00";
                        break;
                    case "Y/N":
                        this.txtCheckListItemValue.Properties.Mask.BeepOnError = false;
                        this.txtCheckListItemValue.Properties.Mask.EditMask = "Yes|No";
                        this.txtCheckListItemValue.Properties.Mask.IgnoreMaskBlank = true;
                        this.txtCheckListItemValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                        this.txtCheckListItemValue.Properties.Mask.ShowPlaceHolders = true;
                        this.txtCheckListItemValue.Properties.Mask.UseMaskAsDisplayFormat = true;
                        this.txtCheckListItemValue.Properties.NullText = "No";
                        break;
                }

                this.txtCheckListItemValue.Text = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE).ToString();
                this.txtRemark.Text = this.grvCheckListJobItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK).ToString();

                this.txtCheckListItemValue.Focus();
                this.txtCheckListItemValue.SelectAll();
            }
            else
            {
                this.txtCheckListItem.Text = string.Empty;
                this.txtCheckListItemDesc.Text = string.Empty;
                this.txtStandard.Text = string.Empty;
                this.txtOptional.Checked = false;

                this.txtCheckListItemValue.Text = string.Empty;
                this.txtRemark.Text = string.Empty;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.grvCheckListJobList.FocusedRowHandle >= 0)
            {
                string checkListJobState = this.grvCheckListJobList.GetRowCellValue(this.grvCheckListJobList.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE).ToString();

                if (checkListJobState != "STARTED")
                {
                    MessageService.ShowMessage(string.Format("当前检查表单任务状态:{0},不能保存检查表单任务项数据!", checkListJobState));

                    return;
                }
            }
            else
            {
                MessageService.ShowMessage("请选择检查表单任务!");

                return;
            }

            if (this.grvCheckListJobItemList.FocusedRowHandle >= 0)
            {
                string checkListJobKey = this.grvCheckListJobItemList.GetRowCellValue(this.grvCheckListJobItemList.FocusedRowHandle, EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKLIST_JOB_KEY).ToString();
                string checkItemKey = this.grvCheckListJobItemList.GetRowCellValue(this.grvCheckListJobItemList.FocusedRowHandle, EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_KEY).ToString();
                bool isOptional = this.grvCheckListJobItemList.GetRowCellValue(this.grvCheckListJobItemList.FocusedRowHandle, EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL).ToString() == "1";
                string checkItemValue = string.Empty;
                string remark = this.txtRemark.Text.Trim();

                if (this.txtCheckListItemValue.Text.Trim().Length > 0)
                {
                    checkItemValue = this.txtCheckListItemValue.Text.Trim();
                }
                else
                {
                    if (!isOptional)
                    {
                        MessageService.ShowMessage("请输入检查表单任务项值!");

                        this.txtCheckListItemValue.Focus();
                        this.txtCheckListItemValue.SelectAll();

                        return;
                    }
                }

                string completeTimeStamp, msg;

                this.equipmentCheckListJobEntity.SaveCheckListJobData(checkListJobKey, checkItemKey, checkItemValue, remark, out completeTimeStamp, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grvCheckListJobItemList.SetFocusedRowCellValue(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_CHECKITEM_VALUE, checkItemValue);
                    this.grvCheckListJobItemList.SetFocusedRowCellValue(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_REMARK, remark);
                    this.grvCheckListJobItemList.SetFocusedRowCellValue(EMS_CHECKLIST_JOB_DATA_FIELDS.FIELD_COMPLETE_TIMESTAMP, completeTimeStamp);
                    this.grvCheckListJobItemList.UpdateCurrentRow();
                    this.grvCheckListJobItemList.BestFitColumns();

                    int nextIndex = this.grvCheckListJobItemList.FocusedRowHandle + 1;

                    if (nextIndex < this.grvCheckListJobItemList.RowCount)
                    {
                        this.grvCheckListJobItemList.FocusedRowHandle = nextIndex;
                    }
                    else
                    {
                        if (MessageService.AskQuestion("该检查表单任务项已经完成最后一项,是否完成该检查表单任务?"))
                        {
                            btnComplete_Click(this.btnComplete, EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    MessageService.ShowError(msg);
                }
            }
            else
            {
                MessageService.ShowMessage("请选择需要完成的检查表单任务项!");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.grvCheckListJobItemList.FocusedRowHandle >= 0)
            {
                grvCheckListJobItemList_FocusedRowChanged(this.grvCheckListJobItemList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobItemList.FocusedRowHandle));
            }
        }

        #endregion
    }
}
