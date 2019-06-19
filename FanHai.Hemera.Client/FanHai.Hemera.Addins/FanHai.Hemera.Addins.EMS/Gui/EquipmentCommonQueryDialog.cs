using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment Common Query Data Dialog
    /// </summary>
    /// Owner:Andy Gao 2011-07-15 13:57:41
    public partial class EquipmentCommonQueryDialog : BaseDialog
    {
        #region Public Fields

        public DataRow[] SelectedData = new DataRow[] { };

        #endregion

        #region Private Variables

        private string formType = string.Empty;
        private bool isMultiSelect = false;

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formType"></param>
        /// <param name="isMultiSelect"></param>
        public EquipmentCommonQueryDialog(string formType, bool isMultiSelect)
        {
            this.formType = formType;
            this.isMultiSelect = isMultiSelect;

            InitializeComponent();

            InitUi();
        }

        private void InitUi()
        {
            lcgDataQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.lbl.0001}");
            lcgDataList.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.lbl.0002}");
            btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
            btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
        }

        #endregion

        #region Form Events

        /// <summary>
        /// 画面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentCommonQueryDialog_Load(object sender, EventArgs e)
        {
            InitialFormControls();
            LoadData("");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initial Form Controls By Form Type
        /// </summary>
        private void InitialFormControls()
        {
            GridViewHelper.SetGridView(grvDataList);
            DataTable dataTable;
            DataColumn dc;
            GridColumn gridColumn;

            switch (this.formType)
            {
                case "EquipmentCheckItems": //设备检查项

                    #region Initial Equipment Check Items Controls

                    this.Text = "设备检查项查询";
                    this.lciQueryLabel.Text = "检查项名称";

                    dataTable = new DataTable(EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY);

                    dc.Caption = "检查项ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME);

                    dc.Caption = "检查项名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE);

                    dc.Caption = "检查项类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentCheckList": //设备检查表单

                    #region Initial Equipment Check List Controls

                    this.Text = "设备检查表单查询";
                    this.lciQueryLabel.Text = "检查表单名称";

                    dataTable = new DataTable(EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY);

                    dc.Caption = "检查表单ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

                    dc.Caption = "检查表单名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE);

                    dc.Caption = "检查表单类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentCheckListJob": //设备检查表单任务

                    #region Initial Equipment Check List Job Controls

                    this.Text = "设备检查表单任务查询";
                    this.lciQueryLabel.Text = "检查表单任务名称";

                    dataTable = new DataTable(EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY);

                    dc.Caption = "检查表单任务ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME);

                    dc.Caption = "检查表单任务名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE);

                    dc.Caption = "检查表单任务状态";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP);

                    dc.Caption = "创建时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP);

                    dc.Caption = "开始时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP);

                    dc.Caption = "完成时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_TYPE);

                    dc.Caption = "PM类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_KEY);

                    dc.Caption = "PMID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_NAME);

                    dc.Caption = "PM名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = "设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY);

                    dc.Caption = "检查表单ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

                    dc.Caption = "检查表单名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentPart": //设备备件

                    #region Initial Equipment Part Controls

                    this.Text = "设备备件查询";
                    this.lciQueryLabel.Text = "备件名称";

                    dataTable = new DataTable(EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY);

                    dc.Caption = "备件ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME);

                    dc.Caption = "备件名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE);

                    dc.Caption = "备件类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE);

                    dc.Caption = "备件型号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT);

                    dc.Caption = "备件单位";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentTask": //设备作业

                    #region Initial Equipment Task Controls

                    this.Text = "设备作业查询";
                    this.lciQueryLabel.Text = "作业名称";

                    dataTable = new DataTable(EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY);

                    dc.Caption = "作业ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME);

                    dc.Caption = "作业名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE);

                    dc.Caption = "作业状态";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY);

                    dc.Caption = "创建用户";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP);

                    dc.Caption = "创建时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY);

                    dc.Caption = "开始用户";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP);

                    dc.Caption = "开始时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_USER_KEY);

                    dc.Caption = "完成用户";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP);

                    dc.Caption = "完成时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);

                    dc.Caption = "设备转变状态ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);

                    dc.Caption = "设备转变状态名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);

                    dc.Caption = "设备转变原因ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);

                    dc.Caption = "设备转变原因名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = "设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;

                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    string msg;

                    dataTable = new EquipmentCheckListJobEntity().LoadUsersData(out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        RepositoryItemGridLookUpEdit usersLookUpEdit = new RepositoryItemGridLookUpEdit();

                        usersLookUpEdit.DisplayMember = RBAC_USER_FIELDS.FIELD_USERNAME;
                        usersLookUpEdit.ValueMember = RBAC_USER_FIELDS.FIELD_USER_KEY;
                        usersLookUpEdit.NullText = string.Empty;
                        usersLookUpEdit.DataSource = dataTable;

                        gridColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY);

                        if (gridColumn != null)
                        {
                            gridColumn.ColumnEdit = usersLookUpEdit;
                        }

                        gridColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY);

                        if (gridColumn != null)
                        {
                            gridColumn.ColumnEdit = usersLookUpEdit;
                        }

                        gridColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_USER_KEY);

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

                    break;
                case "EquipmentLayout"://设备布局图
                    #region Initial Equipment Layout Controls

                    this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.title}"); //"设备布局图查询";
                    this.lciQueryLabel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.lbl.0001}"); //"设备布局图名称";

                    dataTable = new DataTable(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0001}"); //"选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0002}");// "布局图ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0003}");// "布局图名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_DESC);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0004}");// "描述";
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);
                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
                    this.grvDataList.Columns[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].Visible = false;

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }
                    #endregion
                    break;

                case "Equipment_Q"://设备状态切换
                case "Equipment_E": //设备
                    #region Equipment Grid

                    this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.title}");//设备查询
                    this.lciQueryLabel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.lbl.0001}");//设备编码

                    dataTable = new DataTable(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0001}"); //"选择";
                    dc.ReadOnly = false;


                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0002}"); //"设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0003}"); //"设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0004}"); //"设备描述";
                    dc.ReadOnly = true;

                    //Add by qym 20120530
                    dc = dataTable.Columns.Add("EQUIPMENT_STATE_NAME");
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0005}"); //"设备状态";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0006}"); //"设备编码";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0007}"); //"设备类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0008}"); //"设备型号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0009}"); //"最小加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0010}"); //"最大加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, typeof(bool));
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0011}"); //"是否支持批处理";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER, typeof(bool));
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0012}"); //"是否多腔体设备";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0013}"); //"腔体个数";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0014}"); //"腔体编号";




                    ControlUtils.InitialGridView(this.grvDataList, dataTable);
                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
                    GridColumn gridColumn2 = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH);
                    GridColumn gridColumn3 = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER);
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].Visible = false;

                    //add by qym 20120530
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Visible = false;


                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;
                        gridColumn2.ColumnEdit = checkEdit;
                        gridColumn3.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }
                    #endregion
                    break;
                case "Equipment_C"://腔体
                    #region Equipment Grid

                    this.Text = "设备查询";
                    this.lciQueryLabel.Text = "设备编码";

                    dataTable = new DataTable(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = "设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION);
                    dc.Caption = "设备描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE);
                    dc.Caption = "设备编码";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);
                    dc.Caption = "设备类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE);
                    dc.Caption = "设备型号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY);
                    dc.Caption = "父设备主键";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add("PARENT_EQUIPMENT_NAME");
                    dc.Caption = "父设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY);
                    dc.Caption = "最小加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY);
                    dc.Caption = "最大加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, typeof(bool));
                    dc.Caption = "是否支持批处理";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, typeof(bool));
                    dc.Caption = "是否腔体";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX);
                    dc.Caption = "腔体编号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER);
                    dc.Caption = "是否多腔体设备";

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL);
                    dc.Caption = "腔体数量";


                    ControlUtils.InitialGridView(this.grvDataList, dataTable);
                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
                    GridColumn batchColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH);
                    GridColumn chamberColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER);
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Visible = false;


                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;
                        batchColumn.ColumnEdit = checkEdit;
                        chamberColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }
                    #endregion

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Load Data By Query Value
        /// </summary>
        /// <param name="queryValue"></param>
        /// Owner:Andy Gao 2011-07-15 10:37:57
        private void LoadData(string queryValue)
        {
            string msg;
            int pageNo, pageSize, pages, records;
            DataTable dataTable;
            DataColumn dc;
            if (!queryValue.Trim().Equals(""))
                queryValue = queryValue.Trim().ToUpper();
            switch (this.formType)
            {
                case "EquipmentCheckItems": //设备检查项

                    #region Load Equipment Check Items Data

                    EquipmentCheckItemEntity equipmentCheckItemEntity = new EquipmentCheckItemEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentCheckItemEntity.LoadCheckItemsData(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentCheckList": //设备检查表单

                    #region Load Equipment Check List Data

                    EquipmentCheckListEntity equipmentCheckListEntity = new EquipmentCheckListEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentCheckListEntity.LoadCheckListData(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentCheckListJob": //设备检查表单任务

                    #region Load Equipment Check List Job Data

                    EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentCheckListJobEntity.LoadCheckListJobsData(string.Empty, string.Empty, queryValue, string.Empty, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentPart": //设备备件

                    #region Load Equipment Part Data

                    EquipmentPartEntity equipmentPartEntity = new EquipmentPartEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentPartEntity.LoadPartsData(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentTask": //设备作业

                    #region Load Equipment Task Data

                    EquipmentTaskEntity equipmentTaskEntity = new EquipmentTaskEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentTaskEntity.LoadTaskData(string.Empty, string.Empty, queryValue, string.Empty, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentLayout"://设备布局图
                    #region Load Equipment Layout Data

                    EquipmentLayoutEntity equipmentLayoutEntity = new EquipmentLayoutEntity();

                    this.paginationDataList.Visible = false;

                    dataTable = equipmentLayoutEntity.SearchEquipmentLayout(txtQueryValue.Text, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;
                        this.grdDataList.DataSource = dataTable;
                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;
                        MessageService.ShowError(msg);
                    }
                    #endregion                   
                    break;

                case "Equipment_E": //设备                    
                case "Equipment_C"://腔体
                    #region Get equipment data

                    EquipmentEntity equipmentEntity = new EquipmentEntity();
                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);
                    string equipmentType = formType.Substring(formType.Length - 1, 1);
                    if (equipmentType == "E")
                    {
                        dataTable = equipmentEntity.GetParentEquipments(queryValue, pageNo, pageSize, out pages, out records, out msg);
                    }
                    else
                    {
                        dataTable = equipmentEntity.GetChildEquipments(queryValue, pageNo, pageSize, out pages, out records, out msg);
                    }

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }
                    #endregion
                    break;
                case "Equipment_Q"://设备状态切换
                    #region Get equipment data
                    //modi by chao.pang 2012/5/8
                    //grvDataList.OptionsBehavior.Editable = false;
                    grvDataList.Columns[1].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[2].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[3].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[4].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[5].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[6].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[7].OptionsColumn.AllowEdit = false;
                    ///////

                    EquipmentEntity equipmentEntityEvent = new EquipmentEntity();
                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);
                    string equipmentTypeEvent = formType.Substring(formType.Length - 1, 1);
                    dataTable = equipmentEntityEvent.GetStateEventEquipments(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Components Events

        private void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.isMultiSelect)
            {
                DataTable dataTable = this.grdDataList.DataSource as DataTable;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                    foreach (DataRow selectedRow in selectedRows)
                    {
                        selectedRow[COMMON_FIELDS.FIELD_COMMON_CHECKED] = false;
                    }
                }
            }

            if (this.grvDataList.EditingValueModified)
            {
                this.grvDataList.SetFocusedValue(this.grvDataList.EditingValue);
                this.grvDataList.UpdateCurrentRow();
            }

        }

        private void paginationDataList_DataPaging()
        {
            LoadData(this.txtQueryValue.Text.Trim());
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData(this.txtQueryValue.Text.Trim());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dataTable = this.grdDataList.DataSource as DataTable;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                if (selectedRows.Length > 0)
                {
                    this.SelectedData = selectedRows;

                    this.DialogResult = DialogResult.OK;

                    this.Close();
                }
                else
                {
                    MessageService.ShowMessage("请选择数据!");
                }
            }
            else
            {
                MessageService.ShowMessage("请查询数据!");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        //modi by chao.pang 2012/5/8
        private void grvDataList_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = grvDataList.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                DataTable dataTable = this.grdDataList.DataSource as DataTable;
                string s1 = grvDataList.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE).ToString();

                DataRow[] dr = dataTable.Select(string.Format("EQUIPMENT_CODE='{0}'", s1));
                dr[0][COMMON_FIELDS.FIELD_COMMON_CHECKED] = true;
            }

            btnOK_Click(sender, e);
        }
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
