using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment States GUI
    /// </summary>    
    public partial class EquipmentStatesChange : BaseUserCtrl
    {
        #region Constructor
        /// <summary>
        /// 获取设备主键并绑定
        /// </summary>
        string equipmentKey = string.Empty, equipment_state_change_key = string.Empty, equipment_state_key = string.Empty, equipment_event_key = string.Empty;
        EquipmentStateEventsEntity equipmentStateEvent = new EquipmentStateEventsEntity();

        private bool blMyInited = false; //用于Paint事件

        public DataRow[] SelectedData = new DataRow[] { };
        private string formType = string.Empty;
        private bool isMultiSelect = false;
        
        public EquipmentStatesChange()
        {
             InitializeComponent();
             //WorkbenchSingleton.Workbench.ViewOpened -= Workbench_ViewOpened;
             //自带的方法视图打开的时候触发在打开视图之后
             //WorkbenchSingleton.Workbench.ViewOpened += new ViewContentEventHandler(Workbench_ViewOpened);

            InitUi();
        }

        private void InitUi()
        {
            GridViewHelper.SetGridView(grdViewStateEvent);
            tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            //tsbCancel.Text = StringParser.Parse("${res:Global.CloseButtonText}");
            //lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0001}");
            grpState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0002}");
            lblStateName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0003}");
            lblStateType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0004}");
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0005}");
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0006}");
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0007}");
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0008}");
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0009}");
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.lbl.0010}");
            EQUIPMENT_CODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Column.0001}");
            EQUIPMENT_GROUP_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Column.0002}");
            EQUIPMENT_STATE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Column.0003}");
            EQUIPMENT_CHANGE_STATE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Column.0004}");
            DESCRIPTION.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Column.0005}");
            CREATE_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Column.0006}");
            btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
        }

        #endregion

        #region UI Events

        private void EquipmentStates_Load(object sender, EventArgs e)
        {
            tsbSave.Enabled = false;
        }
        #endregion

        #region Override Methods

        /// <summary>
        /// 初始化页面操作权限
        /// </summary>
        protected override void InitUIAuthoritiesByUser()
        {

        }
        #endregion

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.formType = "Equipment_Q";
            this.isMultiSelect = false;
            InitialFormControls();
            LoadData(this.txtQueryValue.Text.Trim());
        }
        /// <summary>
        /// Load Data By Query Value
        /// </summary>
        /// <param name="queryValue"></param>
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dataTable = this.grdDataList.DataSource as DataTable;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                if (selectedRows.Length > 0)
                {
                    this.SelectedData = selectedRows;
                }
                else
                {
                    MessageService.ShowMessage("请选择数据!");
                }
                if (SelectedData != null && SelectedData.Length > 0)
                {
                    DataRow selectedRow = SelectedData[0];

                    //判断是否有设备，若没有选择设备，则UI保存按钮不可用
                    //if (string.IsNullOrEmpty(equipmentKey.Trim()) || !equipmentKey.Trim().Equals(selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString()))
                    if (!string.IsNullOrEmpty(selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString().Trim()))
                        tsbSave.Enabled = true;

                    //获取设备主键并绑定
                    equipmentKey = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                    //当前设备所在事件
                    equipment_state_change_key = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                    //获得设备当前状态
                    equipment_state_key = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY].ToString();

                    
                    DataSet dsReturn = equipmentStateEvent.GetCurrentEquipment2(equipmentKey, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                    //DataSet dsReturn = equipmentStateEvent.GetCurrentEquipment2(equipmentKey);

                    //绑定当前状态
                    DataTable dtEquipment = dsReturn.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                    if (dtEquipment != null && dtEquipment.Rows.Count > 0)
                    {
                        DataRow drEquipments = dtEquipment.Rows[0];
                        //设备编号
                        txtEquipmentNo.Text = drEquipments[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].ToString();
                        //当前主状态
                        txtCMStates.Text = drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                        //当前状态
                        txtCState.Text = drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].ToString() + "(" + drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION].ToString() + ")";
                        txtCState.Properties.AppearanceReadOnly.BackColor = new ColorType().GetStateColor(drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].ToString());

                        //设备组
                        txtEquipmentGroupNo.Text = drEquipments[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].ToString();

                        //清除设备事件
                        lueEquipmentEventNo.ItemIndex = -1;
                        lueEquipmentEventNo.Properties.DataSource = null;
                        txtDMStates.Text = string.Empty;
                        txtDState.Text = string.Empty;
                        txtDState.Tag = null;

                        //当前设备事件
                        //equipment_event_key = drEquipments[EMS_STATE_EVENT_FIELDS.EVENT_KEY].ToString();
                    }
                    //显示历史数据状态
                    DataTable dtEquipmentEventHis = dsReturn.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME];
                    grdStateEvent.MainView = grdViewStateEvent;
                    grdStateEvent.DataSource = dtEquipmentEventHis;
                    //绑定设备切换事件
                    DataTable dtStateEvent = dsReturn.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME];
                    lueEquipmentEventNo.Properties.DisplayMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME;
                    lueEquipmentEventNo.Properties.ValueMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY;
                    lueEquipmentEventNo.Properties.DataSource = dtStateEvent;
                }
                txtDState.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(251, 248, 240);
            }
            else
            {
                MessageService.ShowMessage("请查询数据!");
            }
        }
        /// <summary>
        /// 单击设备号栏位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEquipmentNo_Click(object sender, EventArgs e)
        {
            using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("Equipment_Q", false))
            {
                if (queryDialog.ShowDialog() == DialogResult.OK)
                {
                    if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                    {
                        DataRow selectedRow = queryDialog.SelectedData[0];

                        //判断是否有设备，若没有选择设备，则UI保存按钮不可用
                        //if (string.IsNullOrEmpty(equipmentKey.Trim()) || !equipmentKey.Trim().Equals(selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString()))
                        if (!string.IsNullOrEmpty(selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString().Trim()))
                            tsbSave.Enabled = true;

                        //获取设备主键并绑定
                        equipmentKey = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                        //当前设备所在事件
                        equipment_state_change_key = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                        //获得设备当前状态
                        equipment_state_key = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY].ToString();

                        
                        DataSet dsReturn = equipmentStateEvent.GetCurrentEquipment2(equipmentKey, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                        //DataSet dsReturn = equipmentStateEvent.GetCurrentEquipment2(equipmentKey);

                        //绑定当前状态
                        DataTable dtEquipment = dsReturn.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                        if (dtEquipment != null && dtEquipment.Rows.Count > 0)
                        {
                            DataRow drEquipments = dtEquipment.Rows[0];
                            //设备编号
                            txtEquipmentNo.Text = drEquipments[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].ToString();
                            //当前主状态
                            txtCMStates.Text = drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                            //当前状态
                            txtCState.Text = drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].ToString() + "(" + drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION].ToString() + ")";
                            txtCState.Properties.AppearanceReadOnly.BackColor = new ColorType().GetStateColor(drEquipments[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].ToString());
                           
                            //设备组
                            txtEquipmentGroupNo.Text = drEquipments[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].ToString();

                            //清除设备事件
                            lueEquipmentEventNo.ItemIndex = -1;
                            lueEquipmentEventNo.Properties.DataSource = null;
                            txtDMStates.Text = string.Empty;
                            txtDState.Text = string.Empty;
                            txtDState.Tag = null;
                           
                            //当前设备事件
                            //equipment_event_key = drEquipments[EMS_STATE_EVENT_FIELDS.EVENT_KEY].ToString();
                        }
                        //显示历史数据状态
                        DataTable dtEquipmentEventHis = dsReturn.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME];
                        grdStateEvent.MainView = grdViewStateEvent;
                        grdStateEvent.DataSource = dtEquipmentEventHis;
                        //绑定设备切换事件
                        DataTable dtStateEvent = dsReturn.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME];
                        lueEquipmentEventNo.Properties.DisplayMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME;
                        lueEquipmentEventNo.Properties.ValueMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY;
                        lueEquipmentEventNo.Properties.DataSource = dtStateEvent;
                    }
                    txtDState.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.FromArgb(227, 241, 254);
                }
            }

           
        }
        /// <summary>
        /// 键盘回车键，调用单击设备事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEquipmentNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtEquipmentNo_Click(null, null);
            }
        }
        /// <summary>
        /// 保存设备事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (lueEquipmentEventNo.ItemIndex == -1)
            {
                MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.msg.0001}"));//请选择登录事件号!
                return;
            }

            if (string.IsNullOrEmpty(meDescription.Text.Trim()))
            {
                MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.msg.0002}"));//备注不能为空!
                meDescription.Focus();
                return;
            }
            DataSet dsSave=new DataSet();
            DataTable saveTableStateEvent = Utils.Common.Utils.CreateDataTableWithColumns(EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME, new EMS_STATE_EVENT_FIELDS().FIELDS.Keys.ToList<string>());
            DataTable saveTableStateEventForUpdate = saveTableStateEvent.Clone();
            saveTableStateEventForUpdate.TableName = EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME_UPDATE;

            DataRow drUpdate = saveTableStateEventForUpdate.NewRow();
            //drUpdate[EMS_STATE_EVENT_FIELDS.EVENT_KEY] = equipment_event_key;//当前事件ID
            drUpdate[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY] = equipmentKey; //
            drUpdate[EMS_STATE_EVENT_FIELDS.EQUIPMENT_CHANGE_STATE_KEY] = lueEquipmentEventNo.EditValue.ToString(); //什么状态到另外一个状态
            drUpdate[EMS_STATE_EVENT_FIELDS.EQUIPMENT_TO_STATE_KEY] = txtDState.Tag.ToString();
            drUpdate[EMS_STATE_EVENT_FIELDS.EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drUpdate[EMS_STATE_EVENT_FIELDS.DESCRIPTION] = meDescription.Text.Trim();
            drUpdate[EMS_STATE_EVENT_FIELDS.STATE] = 1;
            saveTableStateEventForUpdate.Rows.Add(drUpdate);

            dsSave.Tables.Add(saveTableStateEventForUpdate);
            DataRow drNew = saveTableStateEvent.NewRow();
            drNew[EMS_STATE_EVENT_FIELDS.EVENT_KEY] =  CommonUtils.GenerateNewKey(0);
            drNew[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY] = equipmentKey;
            drNew[EMS_STATE_EVENT_FIELDS.EQUIPMENT_FROM_STATE_KEY] = txtDState.Tag.ToString();
            drNew[EMS_STATE_EVENT_FIELDS.CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drNew[EMS_STATE_EVENT_FIELDS.STATE] = 1;
            saveTableStateEvent.Rows.Add(drNew);
            dsSave.Tables.Add(saveTableStateEvent);

            if (equipmentStateEvent.UpdateEquipmentStateEvents(dsSave))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.msg.0003}"));//保存成功!
                tsbSave.Enabled = false;
                tsbCancel_Click(sender, e);
            }
            else
            {
                MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.msg.0004}"));//保存失败!
            }

        }
        /// <summary>
        /// 关闭当前页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //遍历工作台中已经打开的视图对象
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开创建批次的视图，则选中该视图显示，返回以结束该方法的运行
                if (viewContent is EquipmentStatesChangeViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
        }

        /// <summary>
        /// 改变切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipmentEventNo_EditValueChanged(object sender, EventArgs e)
        {
            string eventKey = lueEquipmentEventNo.EditValue.ToString();
            if (!string.IsNullOrEmpty(eventKey.Trim()))
            {
                DataTable dtEvent = lueEquipmentEventNo.Properties.DataSource as DataTable;
                DataRow[] drArr = dtEvent.Select(string.Format("EQUIPMENT_CHANGE_STATE_KEY='{0}'", eventKey));
                if (drArr.Length > 0)
                {
                    txtDMStates.Text = drArr[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].ToString();
                    txtDState.Text = drArr[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString() + "(" + drArr[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION].ToString() + ")";
                    txtDState.Tag = drArr[0][EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();
                    txtDState.Properties.AppearanceReadOnly.BackColor = new ColorType().GetStateColor(drArr[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString());
                }
            }
        }
        //界面绘制时触发的事件
        private void EquipmentStatesChange_Paint(object sender, PaintEventArgs e)
        {
             if (!blMyInited)
             {
                 //txtEquipmentNo_Click(null, null);
                 blMyInited = true;
             } 
        }

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
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
    }
}