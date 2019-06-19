using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentCheckListJobsCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListJobEntity Object

        EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

        #endregion

        #region Constructor

        public EquipmentCheckListJobsCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentCheckListJobsCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentCheckListJobsCtrl_afterStateChanged);

            LoadEquipmentLocationData();
            LoadEquipmentGroupData();
            LoadEquipmentData();

            this.State = ControlState.Read;
        }

        private void EquipmentCheckListJobsCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtCheckListJobName.Enabled = false;
                    this.txtCheckListName.Enabled = false;
                    this.txtDescription.Enabled = false;

                    grvCheckListJobList_FocusedRowChanged(this.grvCheckListJobList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckListJobName.Enabled = false;
                    this.txtCheckListName.Enabled = false;
                    this.txtDescription.Enabled = false;

                    grvCheckListJobList_FocusedRowChanged(this.grvCheckListJobList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckListJobName.Enabled = false;
                    this.txtCheckListName.Enabled = true;
                    this.txtDescription.Enabled = true;

                    this.txtCheckListJobName.Text = string.Empty;
                    this.txtCheckListName.Text = string.Empty;
                    this.txtCheckListName.Tag = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckListJobName.Enabled = false;
                    this.txtCheckListName.Enabled = true;
                    this.txtDescription.Enabled = true;

                    grvCheckListJobList_FocusedRowChanged(this.grvCheckListJobList, new FocusedRowChangedEventArgs(-1, this.grvCheckListJobList.FocusedRowHandle));
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

            #region Initial Check List Job Grid

            DataTable dataTable = GetEmptyCheckListJobDataTable();

            ControlUtils.InitialGridView(this.grvCheckListJobList, dataTable);

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
            this.lblTitle.Text = "设备检查表单任务";

            this.grvCheckListJobList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvCheckListJobList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

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
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentCheckListJobEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentCheckListJobEntity.CheckListJobKey =  CommonUtils.GenerateNewKey(0);
                    this.equipmentCheckListJobEntity.EquipmentKey = this.cmbEquipment.EditValue.ToString();
                    this.equipmentCheckListJobEntity.CheckListJobState = "ACTIVED";

                    this.equipmentCheckListJobEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentCheckListJobEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentCheckListJobEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentCheckListJobEntity.CheckListJobKey = this.grvCheckListJobList.GetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY).ToString();
                    this.equipmentCheckListJobEntity.CheckListKey = this.grvCheckListJobList.GetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                    this.equipmentCheckListJobEntity.Description = this.grvCheckListJobList.GetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION).ToString();

                    this.equipmentCheckListJobEntity.Editor = this.grvCheckListJobList.GetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentCheckListJobEntity.EditTimeZone = this.grvCheckListJobList.GetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentCheckListJobEntity.EditTime = this.grvCheckListJobList.GetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentCheckListJobEntity.IsInitializeFinished = true;

            this.equipmentCheckListJobEntity.CheckListKey = this.txtCheckListName.Tag.ToString();
            this.equipmentCheckListJobEntity.Description = this.txtDescription.Text.Trim();
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    this.grvCheckListJobList.AddNewRow();

                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY, this.equipmentCheckListJobEntity.CheckListJobKey);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME, this.equipmentCheckListJobEntity.CheckListJobName);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY, this.equipmentCheckListJobEntity.EquipmentKey);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY, this.equipmentCheckListJobEntity.CheckListKey);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION, this.equipmentCheckListJobEntity.Description);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE, this.equipmentCheckListJobEntity.CheckListJobState); 
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP, this.equipmentCheckListJobEntity.CreateTimeStamp); 

                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR, this.equipmentCheckListJobEntity.Creator);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, this.equipmentCheckListJobEntity.CreateTimeZone);
                    this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME, this.equipmentCheckListJobEntity.CreateTime);

                    this.grvCheckListJobList.UpdateCurrentRow();
                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentCheckListJobEntity.DirtyList)
                    {
                        this.grvCheckListJobList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);

                        if (keyValue.Key == EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY)
                        {
                            this.grvCheckListJobList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, this.txtCheckListName.Text);
                        }
                    }

                    this.grvCheckListJobList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvCheckListJobList.DeleteRow(this.grvCheckListJobList.FocusedRowHandle);

                    this.grvCheckListJobList.UpdateCurrentRow();
                    break;
            }

            this.grvCheckListJobList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Check List Job Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-21 08:45:55
        private DataTable GetEmptyCheckListJobDataTable()
        {
            DataTable dataTable = EMS_CHECKLIST_JOBS_FIELDS.CreateDataTable(true);

            dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_NAME);
            dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
            dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

            return dataTable;
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        /// Owner:Andy Gao 2011-07-20 15:21:34
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
        /// Owner:Andy Gao 2011-07-20 15:23:37
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
        /// Owner:Andy Gao 2011-07-20 15:25:45
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

            this.paginationCheckListJob.GetPaginationProperties(out pageNo, out pageSize);

            string equipmentKey = string.Empty;

            if (this.cmbEquipment.Text.Length > 0)
            {
                equipmentKey = this.cmbEquipment.EditValue.ToString();
            }

            DataTable dataTable = this.equipmentCheckListJobEntity.LoadCheckListJobsData(equipmentKey, string.Empty, string.Empty, string.Empty, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationCheckListJob.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdCheckListJobList.DataSource = dataTable;
            }
            else
            {
                this.grdCheckListJobList.DataSource = GetEmptyCheckListJobDataTable();

                MessageService.ShowError(msg);
            }

            this.grvCheckListJobList.BestFitColumns();

            State = ControlState.Read;
        }

        #endregion

        #region Component Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadCheckListJobsData();
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

            if (this.txtCheckListName.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择检查表单名称!");

                this.txtCheckListName.Focus();

                return;
            }

            MapControlsToEntity();

            if (State == ControlState.New)
            {
                if (this.equipmentCheckListJobEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadCheckListJobsData();
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentCheckListJobEntity.IsDirty)
                {
                    if (this.equipmentCheckListJobEntity.Update())
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
            if (this.grvCheckListJobList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除检查表单任务!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该检查表单任务吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentCheckListJobEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadCheckListJobsData();
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
            LoadCheckListJobsData();
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

        private void grvCheckListJobList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtCheckListJobName.Text = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME).ToString();
                this.txtCheckListName.Text = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME).ToString();
                this.txtCheckListName.Tag = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                this.txtDescription.Text = this.grvCheckListJobList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION).ToString();
            }
            else
            {
                this.txtCheckListJobName.Text = string.Empty;
                this.txtCheckListName.Text = string.Empty;
                this.txtCheckListName.Tag = string.Empty;
                this.txtDescription.Text = string.Empty;
            }
        }

        private void grdCheckListJobList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvCheckListJobList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                string checkListJobState = this.grvCheckListJobList.GetRowCellDisplayText(gridHitInfo.RowHandle, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE);

                if (checkListJobState == "ACTIVED")
                {
                    State = ControlState.Edit;
                }
                else
                {
                    MessageService.ShowMessage(string.Format("当前检查表单任务状态:{0},不能修改!", checkListJobState));
                }
            }
        }

        private void paginationCheckListJob_DataPaging()
        {
            LoadCheckListJobsData();
        }

        #endregion
    }
}
