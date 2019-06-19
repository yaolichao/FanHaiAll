using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;

namespace FanHai.Hemera.Addins.SPC.Gui
{
    public partial class CommControlPlan : Form
    {
        private DataTable _dtCommon;
        private string formType = string.Empty;
        private List<string> lst = new List<string>();
        public object[] SelectedControlPlanData = null;
        public DataRow drBak = null;
        public string bcEquipmentIDs = string.Empty;
        //DataTable dtGrid,equipmentTable;
        DataTable dtWeaks, dtSteps, dtParams, dtProductCode;//成品类型 //dtAreas, dtLines,
        SpcEntity _spcEntity = new SpcEntity();
        private bool isMultiSelect = false;
        public CommControlPlan(string _formType)
        {
            formType = _formType;
            InitializeComponent();
        }

        public CommControlPlan()
        {
            InitializeComponent();
        }
        public CommControlPlan(string _formType, string strequipments)
        {
            formType = _formType;
            if (!string.IsNullOrEmpty(strequipments))
            {
                lst = strequipments.Split(',').ToList<string>();
            }

            InitializeComponent();
        }

        private void EquipmentControlPlan_Load(object sender, EventArgs e)
        {
            if (formType.Trim().Equals(string.Empty)) return;
            BindLueData();
            InitialFormControls();
         
            gvBind();
            
            //_dtCommon.Columns.Add("isSelected", typeof(bool));
            //gcGridControl.MainView = gvGridView;
            //gcGridControl.DataSource = _dtCommon;

            //for (int i = 0; i < gvGridView.DataRowCount; i++)
            //{
            //    string _eid = gvGridView.GetRowCellValue(i, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY).ToString();
            //    if (lst.Contains(_eid))
            //        gvGridView.SetRowCellValue(i, "isSelected", true);
            //}
        }

        private void gvBind()
        {
            _dtCommon = null;
            _dtCommon = _spcEntity.GetSPControlGridData().Tables[0];
        }
        private void InitialFormControls()
        {
            DataTable dataTable;
            DataColumn dc;
            GridColumn gridColumn;

            switch (this.formType)
            {
                case "SpcControlPlan": //管控计划
                case "SpcChart": //SPC图形

                    #region 管控计划，生成管控图，查询管控计划

                    this.Text = "管控计划查询";                

                    dataTable = new DataTable(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME);

                    //选择
                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));
                    dc.Caption = "选择";                    
                    dc.ReadOnly = false;                   
                    //管控计划代码
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE);
                    dc.Caption = "管控代码";
                    dc.ReadOnly = true;
                    //管控代码描述
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC);
                    dc.Caption = "代码描述";
                    dc.ReadOnly = true;                   
                    //工厂
                    dc = dataTable.Columns.Add("LOCATION_NAME");
                    dc.Caption = "工厂";
                    dc.ReadOnly = true;
                    //工序
                    dc = dataTable.Columns.Add("STEP_KEY");
                    dc.Caption = "工序";
                    dc.ReadOnly = true;              
                    //成品类型
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE);
                    dc.Caption = "成品类型";
                    dc.ReadOnly = true;
                    //控制图类型
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE);
                    dc.Caption = "控制图类型";
                    dc.ReadOnly = true;
                    //参数
                    dc = dataTable.Columns.Add("PARAM_NAME");
                    dc.Caption = "参数";
                    dc.ReadOnly = true;
                    //上限
                    dc = dataTable.Columns.Add("UPPER_BOUNDARY");
                    dc.Caption = "上限";
                    dc.ReadOnly = true;
                    //目标值
                    dc = dataTable.Columns.Add("TARGET");
                    dc.Caption = "目标值";
                    dc.ReadOnly = true;
                    //上限
                    dc = dataTable.Columns.Add("LOWER_BOUNDARY");
                    dc.Caption = "下限";
                    dc.ReadOnly = true;
                    //激活状态
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_STATES);
                    dc.Caption = "状态";
                    dc.ReadOnly = true;
                    //id
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID);
                    dc.ReadOnly = true;
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_WERKS);
                    dc.ReadOnly = true;
                    //dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
                    //dc.ReadOnly = true;
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID);
                    dc.ReadOnly = true;
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CREATOR);
                    dc.ReadOnly = true;
                    dc = dataTable.Columns.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.gvGridView, dataTable);

                    this.gvGridView.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;                    
                    this.gvGridView.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                    this.gvGridView.Columns[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].Visible = false;
                    this.gvGridView.Columns[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].Visible = false;
                    //this.gvGridView.Columns[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].Visible = false;
                    this.gvGridView.Columns[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].Visible = false;
                    this.gvGridView.Columns[SPC_CONTROL_PLAN_FIELD.FIELD_CREATOR].Visible = false;
                    this.gvGridView.Columns[SPC_CONTROL_PLAN_FIELD.FIELD_CREATE_TIME].Visible = false;


                    gridColumn = this.gvGridView.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
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

                        this.gvGridView.FormatConditions.Add(checkCondition);
                    }
                    gridColumn = null;
                    gridColumn = this.gvGridView.Columns.ColumnByFieldName(SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
                    if (gridColumn != null)
                    {
                        //dtSteps
                        RepositoryItemLookUpEdit stepEdit = new RepositoryItemLookUpEdit();
                        LookUpColumnInfo lookUpColumninfo = new LookUpColumnInfo(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, "");
                        stepEdit.Columns.Add(lookUpColumninfo);
                        stepEdit.NullText = string.Empty;
                        gridColumn.ColumnEdit = stepEdit;

                        stepEdit.DataSource = dtSteps;
                        stepEdit.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
                        stepEdit.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
                    }

                    #endregion

                    break;

                case "SpcEquipments": //SPC图形中，选择设备作业

                    #region Initial Equipment Task Controls

                    this.Text = "选择设备";      

                    dataTable = new DataTable(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;
                    //设备编号
                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE);
                    dc.Caption = "设备编号";
                    dc.ReadOnly = true;
                    //设备名称
                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;
                    //设备描述                   
                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION);
                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.gvGridView, dataTable);

                    this.gvGridView.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.gvGridView.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.gvGridView.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.gvGridView.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;                  
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
                DataTable dataTable = this.gvGridView.DataSource as DataTable;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                    foreach (DataRow selectedRow in selectedRows)
                    {
                        selectedRow[COMMON_FIELDS.FIELD_COMMON_CHECKED] = false;
                    }
                }
            }

            if (this.gvGridView.EditingValueModified)
            {
                this.gvGridView.SetFocusedValue(this.gvGridView.EditingValue);
                this.gvGridView.UpdateCurrentRow();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (_dtCommon.Rows.Count < 1) return;
            this.gcGridControl.DataSource = null;

            string s = "1=1 ";
            if (!txtCONTROLCODE.Text.Trim().Equals(string.Empty)) //管控代码
                s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE + "='{0}'", txtCONTROLCODE.Text.Trim());
            if (lueWeaks.ItemIndex > -1) //车间
                s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_WERKS + "='{0}' ", lueWeaks.EditValue.ToString());
            if (lueSteps.ItemIndex > -1) //工序
                s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY + "='{0}' ", lueSteps.EditValue.ToString());
            if (lueParames.ItemIndex > -1)  //参数
                s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID + "='{0}' ", lueParames.EditValue.ToString());
            if (luePRODUCTCODE.EditValue != null)  //产品类型
                s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE + "='{0}' ", luePRODUCTCODE.EditValue.ToString());
            if (lueCONTROLTYPE.EditValue != null)  //图类型
                s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE + "='{0}' ", lueCONTROLTYPE.EditValue.ToString());
          
            //if (cmbStates.SelectedIndex > -1)
            //{
            //    string v=string.Empty;
            //    //EntityStatus( cmbStates.SelectedText)
            //    if (EntityStatus.Active.ToString().Equals(cmbStates.SelectedItem))
            //        v = Convert.ToInt32(EntityStatus.Active).ToString();
            //    else if (EntityStatus.Archive.ToString().Equals(cmbStates.SelectedItem))
            //        v = Convert.ToInt32(EntityStatus.Archive).ToString();
            //    else if (EntityStatus.InActive.ToString().Equals(cmbStates.SelectedItem))
            //        v = Convert.ToInt32(EntityStatus.InActive).ToString();

            //    s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_STATES + "='{0}' ", v);
            //}
            if (string.IsNullOrEmpty(s)) return;

            DataTable dtGv = _dtCommon.Copy();
            DataView dv = dtGv.DefaultView;
            dv.RowFilter = s;
            dtGv = dv.ToTable();

            if (dtGv.Rows.Count > 0)
            {
                this.gcGridControl.DataSource = dtGv;
                gvGridView.BestFitColumns();
            }
            else
            {
                MessageService.ShowMessage("未查询到数据!");
                return;
            }  
        }
        public void BindLueData()
        {
            DataSet dsBind = _spcEntity.GetSpcControlPlan();
            //车间
            dtWeaks = InsertBlankRow(dsBind.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
            this.lueWeaks.Properties.DataSource = dtWeaks;
            this.lueWeaks.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            this.lueWeaks.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
   
            lueWeaks.Properties.BestFit();                              
            //工序
            dtSteps = InsertBlankRow(dsBind.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME]);
            this.lueSteps.Properties.DataSource = dtSteps;
            this.lueSteps.Properties.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            this.lueSteps.Properties.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
            lueSteps.Properties.BestFit();           
            if (dsBind.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                lueSteps.ItemIndex = -1;
            }
            //参数
            dtParams = InsertBlankRow(dsBind.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME]);
            this.lueParames.Properties.DataSource = dtParams;
            this.lueParames.Properties.DisplayMember = BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME;
            this.lueParames.Properties.ValueMember = BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY;
            lueParames.Properties.BestFit();         
            if (dsBind.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                lueParames.ItemIndex = -1;
            }
            //成品类型
            dtProductCode = InsertBlankRow(dsBind.Tables["PARTYPE"]);
            this.luePRODUCTCODE.Properties.DataSource = dtProductCode;
            this.luePRODUCTCODE.Properties.DisplayMember = "TYPENAME";
            this.luePRODUCTCODE.Properties.ValueMember = "TYPECODE";
            luePRODUCTCODE.Properties.BestFit();          
            if (dsBind.Tables["PARTYPE"].Rows.Count > 0)
            {
                luePRODUCTCODE.ItemIndex = -1;
            }

            //控制图类型
            DataTable dtChart = ChartType.GetChartType();
            this.lueCONTROLTYPE.Properties.DataSource = dtChart;
            this.lueCONTROLTYPE.Properties.DisplayMember = "TYPECODE";
            this.lueCONTROLTYPE.Properties.ValueMember = "TYPECODE";
            lueCONTROLTYPE.ItemIndex = -1;
           
        }
        //新增一行数据到数据表
        public DataTable InsertBlankRow(DataTable dt)
        {
            DataTable dtNew = dt.Copy();
            DataRow dr = dtNew.NewRow();
            foreach (DataColumn dc in dtNew.Columns)
            {
                if (dc.DataType.Equals(typeof(string)))
                    dr[dc.ColumnName] = string.Empty;
                if (dc.DataType.Equals(typeof(decimal)) || dc.DataType.Equals(typeof(int)) || dc.DataType.Equals(typeof(double)))
                    dr[dc.ColumnName] = 0;
                else
                    dr[dc.ColumnName] = string.Empty;

            }
            dtNew.Rows.InsertAt(dr, 0);
            return dtNew;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        private void gcGridControl_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gvGridView.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                DataRow selectedSpcControlDataRow = this.gvGridView.GetDataRow(rowHandle);
                drBak = this.gvGridView.GetDataRow(rowHandle);

                this.SelectedControlPlanData = new object[selectedSpcControlDataRow.ItemArray.Length - 1];

                for (int i = 0; i < this.SelectedControlPlanData.Length; i++)
                {
                    this.SelectedControlPlanData.SetValue(selectedSpcControlDataRow.ItemArray[i + 1], i);
                }

                return true;
            }
            return false;
        }

        private void luePRODUCTCODE_EditValueChanged(object sender, EventArgs e)
        {

        }

    }
}
