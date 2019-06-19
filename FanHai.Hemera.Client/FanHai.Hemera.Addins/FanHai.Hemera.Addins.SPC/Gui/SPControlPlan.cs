using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.SPC.Gui
{
    public partial class SPControlPlan : BaseUserCtrl
    {
        bool blweaks = false;
        //DataTable dtGrid;
        /// <summary>
        /// dtGrid gridview数据表，equipmentTable 设备数据表，dtWeaks 工厂数据表，dtSteps工步数据表，dtParams参数数据表
        /// </summary>
        DataTable equipmentTable, dtWeaks, dtSteps, dtParams, dtProductCode;//成品类型 //dtAreas, dtLines,
        SpcEntity _spcEntity = new SpcEntity();
        string _controlPlanId = string.Empty, state = string.Empty;
        ControlState _ctrlState = new ControlState();
        public delegate void AfterStateChanged(ControlState controlState);
        public AfterStateChanged afterStateChanged = null;
        public SPControlPlan()
        {
            InitializeComponent();
        }
        //Control state property
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        public void onChangeControlState(ControlState cState)
        {
            switch (cState)
            {
                case ControlState.Edit:
                    //this.gvMainControlPlan.OptionsBehavior.Editable = true;
                    this.btnAdd.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnActive.Enabled = false;
                    this.btnCancel.Enabled = true;

                    this.txtControlDesc.Enabled = true;
                    this.lueWeaks.Enabled = true;
                    this.lueSteps.Enabled = true;
                    this.lueParames.Enabled = true;
                    this.btnEditABNORMALIDS.Enabled = true;
                    this.luePRODUCTCODE.Enabled = true;
                    this.lueCONTROLTYPE.Enabled = true;
                    break;
                case ControlState.New:
                    //this.gvMainControlPlan.OptionsBehavior.Editable = true;                    
                    this.btnDelete.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnAdd.Enabled = false;
                    this.btnActive.Enabled = false;
                    this.btnCancel.Enabled = true;

                    this.txtControlDesc.Enabled = true;
                    this.lueWeaks.Enabled = true;
                    this.lueSteps.Enabled = true;
                    this.lueParames.Enabled = true;
                    this.btnEditABNORMALIDS.Enabled = true;
                    this.luePRODUCTCODE.Enabled = true;
                    this.lueCONTROLTYPE.Enabled = true;

                    this.txtControlCode.Text = string.Empty;
                    this.txtControlDesc.Text = string.Empty;
                    this.lueWeaks.EditValue = null;
                    this.lueSteps.EditValue = null;
                    this.lueParames.EditValue = null;
                    this.luePRODUCTCODE.EditValue = null;
                    this.lueCONTROLTYPE.EditValue = null;
                    this.txtUpBound.Text = string.Empty;
                    this.txtTa.Text = string.Empty;
                    this.txtLowBound.Text = string.Empty;
                    this.btnEditABNORMALIDS.Text = string.Empty;
                    if (dtParams != null)
                    {
                        this.lueParames.Properties.DataSource = dtParams;
                        this.lueParames.Properties.DisplayMember = BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME;
                        this.lueParames.Properties.ValueMember = BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY;
                        lueParames.Properties.BestFit();
                    }
                    break;
                case ControlState.ReadOnly:
                    //this.gvMainControlPlan.OptionsBehavior.Editable = false;
                    this.btnAdd.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.btSave.Enabled = false;
                    this.btnActive.Enabled = true;
                    this.btnCancel.Enabled = false;
                    this.txtControlDesc.Enabled = false;
                    this.lueWeaks.Enabled = false;
                    this.lueSteps.Enabled = false;
                    this.lueParames.Enabled = false;
                    this.btnEditABNORMALIDS.Enabled = false;
                    this.luePRODUCTCODE.Enabled = false;
                    this.lueCONTROLTYPE.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void SPControlPlan_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(onChangeControlState);
            this.CtrlState = ControlState.ReadOnly;
            this.gvMainControlPlan.OptionsBehavior.Editable = false;
            BindLueData();
            InitialData(string.Empty);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _controlPlanId = string.Empty;
            this.CtrlState = ControlState.New;
            //this.gvMainControlPlan.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
        }

        public void BindLueData()
        {
            DataSet dsBind = _spcEntity.GetSpcControlPlan();
            //车间
            dtWeaks = InsertBlankRow(dsBind.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
            this.lueWeaks.Properties.DataSource = dtWeaks;
            this.lueWeaks.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            this.lueWeaks.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
            LookUpColumnInfoCollection col01 = lueWeaks.Properties.Columns;
            lueWeaks.Properties.BestFit();
            //表格车间
            repositoryItemluewerks.DataSource = dtWeaks;
            repositoryItemluewerks.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            repositoryItemluewerks.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
            //if (dsBind.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            //{
            //    lueWeaks.ItemIndex = -1;
            //}                        
            //工序
            dtSteps = InsertBlankRow(dsBind.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME]);
            this.lueSteps.Properties.DataSource = dtSteps;
            this.lueSteps.Properties.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            this.lueSteps.Properties.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
            lueSteps.Properties.BestFit();
            //表格工序
            repositoryItemluesteps.DataSource = dtSteps;
            repositoryItemluesteps.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            repositoryItemluesteps.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
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
            //表格参数
            repositoryItemlueparam.DataSource = dtParams;
            repositoryItemlueparam.DisplayMember = BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME;
            repositoryItemlueparam.ValueMember = BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY;
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
            //表格参数
            repositoryItemluePRODUCTCODE.DataSource = dtProductCode;
            repositoryItemluePRODUCTCODE.DisplayMember = "TYPENAME";
            repositoryItemluePRODUCTCODE.ValueMember = "TYPECODE";
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

            #region 修改后，暂时未用的数据源
            //设备
            //equipmentTable = dsBind.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            //lueEquipment.Properties.DataSource = equipmentTable;
            //lueEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME;
            //lueEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;
            //lueEquipment.ItemIndex = -1;
            //lueEquipment.Enabled = false;
            //表格设备
            //repositoryItemluequipment.DataSource = equipmentTable;
            //repositoryItemluequipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME;
            //repositoryItemluequipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;
            ////区域
            // dtAreas = dsBind.Tables["FMM_LOCATION_AREAS"];
            //this.lueAreas.Properties.DataSource = dtAreas;
            //this.lueAreas.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            //this.lueAreas.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
            //lueAreas.Properties.BestFit();
            ////表格区域
            //repositoryItemlueareas.DataSource = dtAreas;
            //repositoryItemlueareas.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            //repositoryItemlueareas.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
            //if (dsBind.Tables["FMM_LOCATION_AREAS"].Rows.Count > 0)
            //{
            //    lueAreas.ItemIndex = -1;
            //}
            ////产线
            // dtLines = dsBind.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];
            //this.lueLines.Properties.DataSource = dtLines;
            //this.lueLines.Properties.DisplayMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME;
            //this.lueLines.Properties.ValueMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE;
            //lueLines.Properties.BestFit();
            ////表格产线
            //repositoryItemlueline.DataSource = dtLines;
            //repositoryItemlueline.Properties.DisplayMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME;
            //repositoryItemlueline.Properties.ValueMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY;
            //if (dsBind.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            //{
            //    lueLines.ItemIndex = -1;
            //}
            #endregion
        }
        /// <summary>
        /// 绑定列表数据
        /// </summary>
        private void InitialData(string s)
        {
            DataTable dtGrid = new DataTable();
            DataView dv = null;
            dtGrid = _spcEntity.GetSPControlGridData().Tables[0];
            this.gcControlPlan.MainView = gvMainControlPlan;
            if (!string.IsNullOrEmpty(s))
            {
                dv = dtGrid.DefaultView;
                dv.RowFilter = s;
                dtGrid = dv.ToTable();
            }

            this.gcControlPlan.DataSource = dtGrid;
            this.gvMainControlPlan.BestFitColumns();

            //if (dsBind.Tables[0].Rows.Count > 0)
            //    this.gvMainControlPlan.OptionsBehavior.Editable = false;
        }

        private void BindGridView(DataTable dt)
        {
            this.gcControlPlan.MainView = gvMainControlPlan;
            this.gcControlPlan.DataSource = dt;
            this.gvMainControlPlan.BestFitColumns();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            DataTable dtGvBind = ((DataView)gvMainControlPlan.DataSource).Table;
            DataSet dsSave = new DataSet();

            string _controlCode = "", _codeDesc = "", _productCode = "", _werks = "";// _areasKey = "", _lineKey = "";
            //string  _states = "";
            string _stepKey = "", _paramentId = "", _speciment = "", _controlType = "", _ctlDate = "", _abnormalids = "";

            DataTable saveTable = Utils.Common.Utils.CreateDataTableWithColumns(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME, new SPC_CONTROL_PLAN_FIELD().FIELDS.Keys.ToList<string>());
            DataTable saveTableForUpdate = saveTable.Clone();
            saveTableForUpdate.TableName = SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE;

            if (lueWeaks.EditValue == null)
            {
                MessageService.ShowMessage("请选择车间!");
                lueWeaks.Focus();
                return;
            }
            if (lueSteps.EditValue == null)
            {
                MessageService.ShowMessage("请选择工序!");
                lueSteps.Focus();
                return;
            }
            if (lueParames.EditValue == null)
            {
                MessageService.ShowMessage("请选择管控参数!");
                lueParames.Focus();
                return;
            }
            if (luePRODUCTCODE.EditValue == null)
            {
                MessageService.ShowMessage("请选择成品类型!");
                luePRODUCTCODE.Focus();
                return;
            }
            if (lueCONTROLTYPE.EditValue == null)
            {
                MessageService.ShowMessage("请选择控制图类型!");
                lueCONTROLTYPE.Focus();
                return;
            }

            _controlCode = txtControlCode.Text.Trim();
            _codeDesc = txtControlDesc.Text.Trim();
            _werks = lueWeaks.EditValue.ToString().Trim();
            _stepKey = lueSteps.EditValue.ToString().Trim();
            _paramentId = lueParames.EditValue.ToString().Trim();
            _controlType = lueCONTROLTYPE.EditValue.ToString().Trim();
            _productCode = luePRODUCTCODE.EditValue.ToString().Trim();
            _abnormalids = btnEditABNORMALIDS.Text.Trim();
            DataTable dtSave = new DataTable();
            if (_ctrlState == ControlState.Edit)
            {
                if (txtControlCode.Text.Trim().Equals(string.Empty))
                {
                    MessageService.ShowMessage("管控代码不能为空，请与系统管理员联系!");
                    txtControlCode.Focus();
                    return;
                }
                if (_controlPlanId.Trim().Equals(string.Empty))
                {
                    MessageService.ShowMessage("后台程序，未找到管控计划ID，请与系统管理员联系!");
                    return;
                }
                state = "1";
                dtSave = saveTableForUpdate;
            }
            if (_ctrlState == ControlState.New)
            {
                _controlPlanId = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                dtSave = saveTable;
                state = "1";
            }

            Dictionary<string, string> rowDataMain = new Dictionary<string, string>()
                                                                            {
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID,_controlPlanId},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE,_controlCode},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC,_codeDesc},
                                                                               // {SPC_CONTROL_PLAN_FIELD.FIELD_AREAS_KEY,_areasKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE,_controlType},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CTLDATE,_ctlDate},
                                                                               // {SPC_CONTROL_PLAN_FIELD.FIELD_LINE_KEY,_lineKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID,_paramentId},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE,_productCode},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_SPECIMEN,_speciment},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_STATES,state},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY,_stepKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_WERKS,_werks},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS,_abnormalids}
                                                                                //{SPC_CONTROL_PLAN_FIELD.FIELD_STATES,state }
                                                                                //{SPC_CONTROL_PLAN_FIELD.FIELD_CREATOR ,  PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                                                //{SPC_CONTROL_PLAN_FIELD.FIELD_CREATE_TIME,  string.Empty}                                               
                                                                            };
            if (_ctrlState == ControlState.New)
            {
                rowDataMain.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CREATOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                rowDataMain.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CREATE_TIME, string.Empty);
            }
            if (_ctrlState == ControlState.Edit)
            {
                rowDataMain.Add(SPC_CONTROL_PLAN_FIELD.FIELD_EDIT_TIME, string.Empty);
                rowDataMain.Add(SPC_CONTROL_PLAN_FIELD.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            }


            FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(dtSave, rowDataMain);

            if (dtSave.Rows.Count > 0)
                dsSave.Merge(dtSave, true, MissingSchemaAction.Add);

            #region 新增和编辑保存数据
            string controlCode = string.Empty;
            if (_spcEntity.SaveSpcControlPlan(dsSave, out controlCode))
            {
                MessageService.ShowMessage("保存成功!");
                if (this.CtrlState == ControlState.New)
                    txtControlCode.Text = controlCode;

                this.CtrlState = ControlState.ReadOnly;
                InitialData(string.Empty);
            }
            #endregion
        }

        /// <summary>
        /// 新增/编辑 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click_Old(object sender, EventArgs e)
        {
            DataTable dtGvBind = ((DataView)gvMainControlPlan.DataSource).Table;
            DataSet dsSave = new DataSet();

            string _controlPlanId = "", _controlCode = "", _codeDesc = "", _productCode = "", _werks = "";// _areasKey = "", _lineKey = "";
            string _stepKey = "", _paramentId = "", _speciment = "", _controlType = "", _ctlDate = "", _states = "", _abnormalids = "";

            DataTable saveTable = Utils.Common.Utils.CreateDataTableWithColumns(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME, new SPC_CONTROL_PLAN_FIELD().FIELDS.Keys.ToList<string>());
            DataTable saveTableForUpdate = saveTable.Clone();
            saveTableForUpdate.TableName = SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE;



            #region Modify by genchille.yang 2012-05-06 14:42:32
            if (_ctrlState == ControlState.Edit)
            {
                DataTable dtOld = dtGvBind.GetChanges(DataRowState.Modified);
                if (dtOld == null)
                {
                    MessageService.ShowMessage("未作任何修改,无需保存!");
                    this.CtrlState = ControlState.ReadOnly;
                    return;
                }
            }
            else if (_ctrlState == ControlState.New)
            {
                DataTable dtOld = dtGvBind.GetChanges(DataRowState.Added);
                if (dtOld == null)
                {
                    MessageService.ShowMessage("没有新增数据,无需保存!");
                    this.CtrlState = ControlState.ReadOnly;
                    return;
                }
            }
            else
            {
                MessageService.ShowMessage("数据为只读状态，无法保存!");
                return;
            }

            #region  //新增数据
            DataTable dtAdd = dtGvBind.GetChanges(DataRowState.Added);
            if (dtAdd != null)
            {
                foreach (DataRow dr in dtAdd.Rows)
                {
                    if (string.IsNullOrEmpty(dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString()))
                        _controlPlanId = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                    else
                        _controlPlanId = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString();

                    #region 给变量赋值
                    _controlCode = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE].ToString();
                    _codeDesc = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC].ToString();
                    _productCode = dr[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString();
                    _werks = dr[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString();
                    //_areasKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_AREAS_KEY].ToString();
                    // _lineKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_LINE_KEY].ToString();
                    _stepKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].ToString();
                    _paramentId = dr[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString();
                    _controlType = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
                    _abnormalids = dr[SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS].ToString();
                    _ctlDate = DateTime.Now.ToShortDateString();
                    _states = "0";
                    if (string.IsNullOrEmpty(_controlCode.Trim()))
                    {
                        MessageService.ShowError("管控代码不能为空!");
                        return;
                    }
                    if (string.IsNullOrEmpty(_werks.Trim()))
                    {
                        MessageService.ShowError("车间代码不能为空!");
                        return;
                    }
                    if (string.IsNullOrEmpty(_stepKey.Trim()))
                    {
                        MessageService.ShowError("工序不能为空!");
                        return;
                    }
                    if (string.IsNullOrEmpty(_paramentId.Trim()))
                    {
                        MessageService.ShowError("参数不能为空!");
                        return;
                    }
                    if (string.IsNullOrEmpty(_controlType.Trim()))
                    {
                        MessageService.ShowError("控制图类型不能为空!");
                        return;
                    }
                    if (string.IsNullOrEmpty(_abnormalids.Trim()))
                    {
                        MessageService.ShowError("异常规则不能为空!");
                        return;
                    }
                    #endregion

                    Dictionary<string, string> rowDataMain = new Dictionary<string, string>()
                                                                            {
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID,_controlPlanId},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE,_controlCode},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC,_codeDesc},
                                                                               // {SPC_CONTROL_PLAN_FIELD.FIELD_AREAS_KEY,_areasKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE,_controlType},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CTLDATE,_ctlDate},
                                                                               // {SPC_CONTROL_PLAN_FIELD.FIELD_LINE_KEY,_lineKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID,_paramentId},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE,_productCode},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_SPECIMEN,_speciment},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_STATES,_states},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY,_stepKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_WERKS,_werks},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS,_abnormalids},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_EDITOR, "G0115"},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToShortDateString()}
                                                                            };

                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(saveTable, rowDataMain);
                }
            }
            if (saveTable.Rows.Count > 0)
                dsSave.Merge(saveTable, true, MissingSchemaAction.Add);
            #endregion

            #region //编辑数据
            DataTable dtUpdate = dtGvBind.GetChanges(DataRowState.Modified);
            if (dtUpdate != null)
            {
                foreach (DataRow dr in dtUpdate.Rows)
                {
                    if (string.IsNullOrEmpty(dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString()))
                        _controlPlanId = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                    else
                        _controlPlanId = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString();

                    if (string.IsNullOrEmpty(_controlPlanId))
                    {
                        MessageService.ShowMessage("编辑的资料有误,请核实!");
                        return;
                    }

                    #region 给变量赋值
                    _controlCode = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE].ToString();
                    if (string.IsNullOrEmpty(_controlCode.Trim()))
                    {
                        MessageService.ShowError("管控代码不能为空!");
                        return;
                    }
                    _codeDesc = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC].ToString();
                    _productCode = dr[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString();
                    _werks = dr[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString();
                    //_areasKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_AREAS_KEY].ToString();
                    //_lineKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_LINE_KEY].ToString();
                    _stepKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].ToString();
                    _paramentId = dr[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString();
                    _controlType = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
                    _abnormalids = dr[SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS].ToString();
                    _ctlDate = DateTime.Now.ToShortDateString();
                    _states = "0";
                    #endregion


                    Dictionary<string, string> rowDataMain = new Dictionary<string, string>()
                                                                            {
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID,_controlPlanId},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE,_controlCode},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC,_codeDesc},
                                                                              //  {SPC_CONTROL_PLAN_FIELD.FIELD_AREAS_KEY,_areasKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE,_controlType},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_CTLDATE,_ctlDate},
                                                                               // {SPC_CONTROL_PLAN_FIELD.FIELD_LINE_KEY,_lineKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID,_paramentId},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE,_productCode},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_SPECIMEN,_speciment},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_STATES,_states},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY,_stepKey},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_WERKS,_werks},
                                                                                {SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS,_abnormalids},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_EDITOR, "G0115"},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToShortDateString()}
                                                                            };

                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(saveTableForUpdate, rowDataMain);
                }
            }
            if (saveTableForUpdate.Rows.Count > 0)
                dsSave.Merge(saveTableForUpdate, true, MissingSchemaAction.Add);
            #endregion

            #endregion

            #region 新增和编辑保存数据
            //if (_spcEntity.SaveSpcControlPlan(dsSave))
            //{
            //    MessageService.ShowMessage("保存成功!");
            //    this.CtrlState = ControlState.ReadOnly;
            //    this.gvMainControlPlan.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            //    InitialData();
            //}
            #endregion
        }

        /// <summary>
        /// 查看管控该规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            CommControlPlan commControlPlan = new CommControlPlan("SpcControlPlan");

            if (commControlPlan.ShowDialog() == DialogResult.OK)
            {
                if (commControlPlan.SelectedControlPlanData == null)
                {
                    MessageService.ShowMessage("未选择数据,请确认!");
                    return;
                }

                DataRow dr = commControlPlan.drBak;
                //Hashtable ht = new Hashtable();
                //if (commControlPlan.SelectedControlPlanData != null)
                //{
                //    for (int i = 0; i < commControlPlan.SelectedControlPlanData.Length; i++)
                //    { 
                //    ht.Add(commControlPlan.SelectedControlPlanData.GetValue(i))
                //    }
                //}
                //        (DataRow)commControlPlan.SelectedControlPlanData;
                //gvMainControlPlan.SetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS, acp.abnormals);


                string s = "1=1 ";

                if (!dr[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString().Equals(string.Empty))
                    s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_WERKS + "='{0}' ", dr[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString());
                if (!dr[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].ToString().Equals(string.Empty))
                    s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY + "='{0}' ", dr[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].ToString());
                if (!dr[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString().Equals(string.Empty))
                    s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID + "='{0}' ", dr[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString());
                if (!dr[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString().Equals(string.Empty))
                    s += string.Format("AND " + SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE + "='{0}' ", dr[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString());
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

                InitialData(s);

                //DataTable dtGv = dtGrid;
                //DataView dv = dtGv.DefaultView;
                //dv.RowFilter = s;
                //dtGv = dv.ToTable();

                //if (dtGv.Rows.Count > 0)
                //{
                //    this.gcControlPlan.MainView = gvMainControlPlan;
                //    this.gcControlPlan.DataSource = dtGv;
                //}
                //else
                //{
                //    MessageService.ShowMessage("未查询到数据!");
                //    return;
                //}
            }
        }
        /// <summary>
        /// 删除多比资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> strArr = new List<string>();
            DataRow dr01 = gvMainControlPlan.GetFocusedDataRow();
            if (dr01 == null) return;
            if (!dr01[SPC_CONTROL_PLAN_FIELD.FIELD_STATES].ToString().Equals("0"))
            {
                MessageService.ShowMessage("只能删除未激活的数据!");
                return;
            }

            strArr.Add(dr01[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString());
            strArr.Add("3");//3:表示删除
            strArr.Add("GO115");

            if (_spcEntity.UpdateControlPlan(strArr))
            {
                MessageService.ShowMessage("选中的资料已删除!");
                InitialData(string.Empty);
            }
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            DataRow dr01 = gvMainControlPlan.GetFocusedDataRow();
            if (dr01 == null) return;
            if (!dr01[SPC_CONTROL_PLAN_FIELD.FIELD_STATES].ToString().Equals("0"))
            {
                MessageService.ShowMessage("请选择未激活的数据!");
                return;
            }

            EntityStatus _entityStatus = new EntityStatus();
            //show dialog
            StatusDialog status = new StatusDialog();
            status.ShowDialog();
            if (status._dialogResult == DialogResult.OK)
            {
                _entityStatus = status._entityStatus;
            }
            else
                return;

            List<string> strArr = new List<string>();
            strArr.Add(dr01[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString());
            strArr.Add(Convert.ToInt32(_entityStatus).ToString());
            strArr.Add("G0115");

            if (_spcEntity.UpdateControlPlan(strArr))
            {
                MessageService.ShowMessage("操作完成!");
                InitialData(string.Empty);
            }
        }

        private void gvMainControlPlan_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID))
            //{
            //    DataRow[] drRows = ((DataTable)repositoryItemlueparam.DataSource).Select(string.Format(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY + "='{0}'", e.Value));

            //    gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY, drRows[0][BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY]);
            //    gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_TARGET, drRows[0][BASE_PARAMETER_FIELDS.FIELD_TARGET]);
            //    gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY, drRows[0][BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY]);
            //}
            //string _areas = string.Empty, _steps = string.Empty;
            //if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_WERKS))
            //{
            //    if (equipmentTable == null) return;                  
            //    DataRow[] dr01 = dtWeaks.Select(string.Format("LOCATION_KEY='{0}'", e.Value.ToString()));
            //    if (dr01 != null)
            //        _areas = dr01[0]["LOCATION_KEY"].ToString();
            //    else
            //        _areas = string.Empty;

            //     _steps = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).ToString();
            //    BindEquipment(_areas, _steps);               
            //}
            //if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY))
            //{
            //    if (equipmentTable == null) return;
            //     _steps = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).ToString();
            //     string steps_name = gvMainControlPlan.GetRowCellDisplayText(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
            //    string _weaks = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS).ToString();
            //    if (!string.IsNullOrEmpty(steps_name))
            //        EnabledParams(steps_name);

            //    DataRow[] dr01 = dtWeaks.Select(string.Format("LOCATION_KEY='{0}'", _weaks));
            //    if (dr01 != null)
            //        _areas = dr01[0]["LOCATION_KEY"].ToString();
            //    else
            //        _areas = string.Empty;

            //    BindEquipment(_areas, _steps);
            //}
        }

        private void EnabledParams(string _steps)
        {
            DataTable dt2BindParams = dtParams.Clone();
            string stepname = _steps;
            if (string.IsNullOrEmpty(stepname))
                lueParames.ItemIndex = -1;
            else
            {
                DataRow[] drArr = dtParams.Select(string.Format("OPERATION_NAME LIKE '%{0}'", lueSteps.Text.Trim()));
                foreach (DataRow dr in drArr)
                    dt2BindParams.ImportRow(dr);
                dt2BindParams.AcceptChanges();

                repositoryItemlueparam.DataSource = dt2BindParams;
                //lueParames.Properties.DataSource = dt2BindParams;
            }
        }
        private void BindEquipment(string _areas, string _step)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(_areas) && !string.IsNullOrEmpty(_step))
                s = string.Format("LOCATION_KEY='{0}' AND OPERATION_KEY='{1}'", _areas, _step);
            else if (!string.IsNullOrEmpty(_areas))
                s = string.Format("LOCATION_KEY='{0}'", _areas);
            else if (!string.IsNullOrEmpty(_step))
                s = string.Format("OPERATION_KEY='{1}'", _areas);
            if (string.IsNullOrEmpty(s)) return;

            DataView dv = equipmentTable.DefaultView;
            dv.RowFilter = s;

            DataTable dtequipment = dv.ToTable();
            repositoryItemluequipment.DataSource = dtequipment;
        }


        private void lueWeaks_EditValueChanged(object sender, EventArgs e)
        {
            if (lueWeaks.ItemIndex > -1)
                blweaks = true;
            else
                blweaks = false;
            EnabledEquipment();
        }

        private void lueSteps_EditValueChanged(object sender, EventArgs e)
        {
            if (lueSteps.ItemIndex > -1)
            {
                EnabledParams();
            }
        }
        private void luePRODUCTCODE_EditValueChanged(object sender, EventArgs e)
        {
            if (luePRODUCTCODE.ItemIndex > -1)
            {
                EnabledParams();
            }
        }

        private void EnabledEquipment()
        {
            //string s = string.Empty;
            //DataView dv;
            //if (equipmentTable == null)
            //    return;
            //else
            //    dv = equipmentTable.DefaultView;

            //if (blweaks && blSteps)
            //{
            //    //DataRow[] dr01 = ((DataTable)lueWeaks.Properties.DataSource).Select(string.Format("LOCATION_KEY='{0}'", lueWeaks.EditValue.ToString()));
            //    DataRow dr01 = ((DataRowView)(lueWeaks.GetSelectedDataRow())).Row;
            //    s = string.Format("LOCATION_KEY='{0}' AND OPERATION_KEY='{1}'", dr01["LOCATION_KEY"].ToString(), lueSteps.EditValue.ToString());
            //}
            //else if (blweaks)
            //{
            //    //DataRow[] dr01 = ((DataTable)lueWeaks.Properties.DataSource).Select(string.Format("LOCATION_KEY='{0}'", lueWeaks.EditValue.ToString()));
            //    //DataRow dr = ((DataRowView)(lueControlCode.GetSelectedDataRow())).Row;
            //    DataRow dr01 = ((DataRowView)(lueWeaks.GetSelectedDataRow())).Row;
            //    s = string.Format("LOCATION_KEY='{0}'", dr01["LOCATION_KEY"].ToString());
            //}
            //else if (blSteps)
            //    s = string.Format("OPERATION_KEY='{0}'", lueSteps.EditValue.ToString());
            //else
            //{
            //    lueEquipment.Enabled = false;
            //    return;
            //}
            //if (string.IsNullOrEmpty(s))
            //    return;  
            //dv.RowFilter = s;
            //DataTable dtequipment = dv.ToTable();
            //lueEquipment.Properties.DataSource = dtequipment;
            //lueEquipment.Enabled = true;
        }
        /// <summary>
        /// 根据工序选择，获得工序下相应的采集参数
        /// </summary>
        private void EnabledParams()
        {
            string partType = string.Empty;
            DataTable dt2BindParams = dtParams.Clone();
            string stepname = lueSteps.Text.Trim();
            if (luePRODUCTCODE.EditValue != null)
                partType = luePRODUCTCODE.EditValue.ToString();

            if (!string.IsNullOrEmpty(stepname) && !string.IsNullOrEmpty(partType))
            {
                DataRow[] drArr = dtParams.Select(string.Format("OPERATION_NAME LIKE '{0}%' AND PART_TYPE='{1}'", lueSteps.Text.Trim(), partType));
                foreach (DataRow dr in drArr)
                    dt2BindParams.ImportRow(dr);
                dt2BindParams.AcceptChanges();

                lueParames.Properties.DataSource = dt2BindParams;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.ReadOnly;
            //this.gvMainControlPlan.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            //InitialData();

        }
        //选择异常规则 OR 设备号
        string tmpValue = string.Empty, rowkey = string.Empty;

        private void gvMainControlPlan_RowClick(object sender, RowClickEventArgs e)
        {
            rowkey = ((DataRowView)(((GridView)sender).GetRow(e.RowHandle))).Row[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString();
        }
        private void gvMainControlPlan_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            tmpValue = e.CellValue.ToString();

            #region 异常规则
            if (e.Column.Name.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS))//异常规则
            {
                if (this.CtrlState == ControlState.ReadOnly) return;

                string strbanormals = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS) == null ? "" : gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS).ToString();

                AbnormalControlPlan acp = new AbnormalControlPlan(strbanormals);
                if (acp.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(acp.abnormals))
                    {
                        MessageService.ShowMessage("未选择数据,请确认!");
                        return;
                    }
                    else
                        gvMainControlPlan.SetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS, acp.abnormals);
                }
            }
            #endregion

            #region 设备 未用 Modify by genchille.yang 2012-05-04 21:09:21
            //if (e.Column.Name.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_EQUIPMENT_KEY))//设备
            //{
            //    if (this.CtrlState == ControlState.ReadOnly) return;

            //    string strequipments = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_EQUIPMENT_KEY) == null ? "" : gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_EQUIPMENT_KEY).ToString();
            //    //获得所选择的车间
            //    string g_werks = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS) == null ? "" : gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS).ToString();
            //    string g_werks_name = gvMainControlPlan.GetRowCellDisplayText(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS);
            //    //获得所选择的工序
            //    string g_steps = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY) == null ? "" : gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).ToString();
            //    string g_steps_name = gvMainControlPlan.GetRowCellDisplayText(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
            //    if (!string.IsNullOrEmpty(g_werks) && !string.IsNullOrEmpty(g_steps))
            //    {
            //        DataRow[] dr01 = dtWeaks.Select(string.Format("LOCATION_KEY='{0}'", g_werks));
            //        if (dr01.Length < 1) return;
            //        DataRow[] dr02 = dtSteps.Select(string.Format("ROUTE_OPERATION_VER_KEY='{0}'", g_steps));
            //        if (dr02.Length < 1) return;

            //        string s = string.Format("LOCATION_KEY='{0}' AND  OPERATION_KEY='{1}'", dr01[0]["LOCATION_KEY"].ToString(), dr02[0]["ROUTE_OPERATION_VER_KEY"].ToString());
            //        //string  s = string.Format("LOCATION_KEY='{0}' AND OPERATION_KEY='{1}'", dr01[0]["LOCATION_KEY_AREAS"].ToString(), g_steps);
            //        DataView dv = equipmentTable.DefaultView;
            //        dv.RowFilter = s;
            //        DataTable dtequipment = dv.ToTable();
            //        if (dtequipment == null || dtequipment.Rows.Count < 1)
            //        {
            //            MessageService.ShowMessage(string.Format("在车间：{0}+工序：{1}中,没有设备可选!", g_werks_name, g_steps_name));
            //            return;
            //        }

            //        EquipmentControlPlan ecp = new EquipmentControlPlan(dtequipment, strequipments);
            //        if (ecp.ShowDialog() == DialogResult.OK)
            //        {
            //            if (string.IsNullOrEmpty(ecp.bcEquipmentIDs))
            //            {
            //                MessageService.ShowMessage("未选择数据,请确认!");
            //                return;
            //            }
            //            else
            //                gvMainControlPlan.SetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_EQUIPMENT_KEY, ecp.bcEquipmentIDs);
            //        }
            //    }
            //}
            #endregion
        }

        private void btnAdd1_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {

        }

        private void btSave1_Click(object sender, EventArgs e)
        {

        }

        private void lueParames_EditValueChanged(object sender, EventArgs e)
        {
            if (lueParames.EditValue != null && !lueParames.EditValue.ToString().Equals(string.Empty))
            {
                string stext = lueSteps.Text.Trim();
                if (!stext.Equals(string.Empty))
                {
                    DataTable dt = lueParames.Properties.DataSource as DataTable;
                    if (dt == null) return;
                    DataRow[] drs = dt.Select(string.Format("OPERATION_NAME = '{0}' AND PARAM_KEY = '{1}'", stext, lueParames.EditValue.ToString()));
                    if (drs.Length < 1) return;
                    txtUpBound.Text = drs[0][BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY].ToString();
                    txtTa.Text = drs[0][BASE_PARAMETER_FIELDS.FIELD_TARGET].ToString();
                    txtLowBound.Text = drs[0][BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY].ToString();
                }
            }
        }

        private void gvMainControlPlan_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            //异常规则
            if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID))
            {

                //DataRow[] drRows = ((DataTable)repositoryItemlueparam.DataSource).Select(string.Format(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY + "='{0}'", e.Value));
                DataRow[] drRows = dtParams.Select(string.Format(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY + "='{0}'", e.Value));

                gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY, drRows[0][BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY]);
                gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_TARGET, drRows[0][BASE_PARAMETER_FIELDS.FIELD_TARGET]);
                gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY, drRows[0][BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY]);
            }
            //string _areas = string.Empty, _steps = string.Empty;

            ////根据工序，获得工序对应的参数
            //if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY))
            //{              
            //    _steps = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).ToString();
            //    string stext = gvMainControlPlan.GetRowCellDisplayText(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).Trim();
            //    if (dtParams != null)
            //    {
            //        DataTable dtParams2 = dtParams.Clone();
            //        DataRow[] drs = dtParams.Select(string.Format("OPERATION_NAME='{0}'", stext));
            //        if (drs.Length < 1) return;
            //        foreach (DataRow dr in drs)
            //            dtParams2.ImportRow(dr);

            //        repositoryItemlueparam.Properties.DataSource = dtParams2;

            //        gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY, 0);
            //        gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_TARGET, 0);
            //        gvMainControlPlan.SetRowCellValue(e.RowHandle, BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY, 0);
            //    }               
            //    //string steps_name = gvMainControlPlan.GetRowCellDisplayText(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
            //    //string _weaks = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS).ToString();
            //    //if (!string.IsNullOrEmpty(steps_name))
            //    //    EnabledParams(steps_name);

            //    //DataRow[] dr01 = dtWeaks.Select(string.Format("LOCATION_KEY='{0}'", _weaks));
            //    //if (dr01 != null)
            //    //    _areas = dr01[0]["LOCATION_KEY"].ToString();
            //    //else
            //    //    _areas = string.Empty;

            //    //BindEquipment(_areas, _steps);
            //}
            //更新数据源
            if (!e.Value.ToString().Trim().Equals(tmpValue))
            {
                if (!string.IsNullOrEmpty(rowkey.Trim()))
                {
                    DataTable dtMain = ((DataView)gvMainControlPlan.DataSource).Table;
                    DataRow[] drs = dtMain.Select(string.Format("CONTROLPLANID='{0}'", rowkey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
            }

            //根据工厂+工序，主要是为了获取设备——暂不用
            #region
            //if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_WERKS))
            //{
            //    if (equipmentTable == null) return;
            //    DataRow[] dr01 = dtWeaks.Select(string.Format("LOCATION_KEY='{0}'", e.Value.ToString()));
            //    if (dr01 != null)
            //        _areas = dr01[0]["LOCATION_KEY"].ToString();
            //    else
            //        _areas = string.Empty;

            //    _steps = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).ToString();
            //    BindEquipment(_areas, _steps);
            //}

            //获得工步，根据工步获得设备——暂不用
            //if (e.Column.FieldName.ToUpper().Equals(SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY))
            //{
            //    if (equipmentTable == null) return;
            //    _steps = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY).ToString();
            //    string steps_name = gvMainControlPlan.GetRowCellDisplayText(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
            //    string _weaks = gvMainControlPlan.GetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS).ToString();
            //    if (!string.IsNullOrEmpty(steps_name))
            //        EnabledParams(steps_name);

            //    DataRow[] dr01 = dtWeaks.Select(string.Format("LOCATION_KEY='{0}'", _weaks));
            //    if (dr01 != null)
            //        _areas = dr01[0]["LOCATION_KEY"].ToString();
            //    else
            //        _areas = string.Empty;

            //    BindEquipment(_areas, _steps);
            //}
            #endregion
        }

        //新增一行数据到数据表
        public DataTable InsertBlankRow(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
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
        //双击表格行，修改表格数据
        private void gcControlPlan_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.gvMainControlPlan.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                //修改状态为edit
                this.CtrlState = ControlState.Edit;
                //行焦点改变事件
                gvMainControlPlan_FocusedRowChanged(this.gvMainControlPlan, new FocusedRowChangedEventArgs(-1, this.gvMainControlPlan.FocusedRowHandle));
            }
        }
        //表格焦点变动，触发该事件
        private void gvMainControlPlan_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                //选中行数据
                _controlPlanId = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID).ToString().Trim();
                state = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STATES).ToString().Trim();
                this.txtControlCode.Text = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE).ToString();
                this.txtControlDesc.Text = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_CODEDESC).ToString();
                this.lueWeaks.EditValue = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_WERKS);
                this.lueSteps.EditValue = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY);
                this.lueParames.EditValue = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID);

                this.luePRODUCTCODE.EditValue = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE);
                this.lueCONTROLTYPE.EditValue = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE);
                this.btnEditABNORMALIDS.Text = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS).ToString();

                this.txtUpBound.Text = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY).ToString();
                this.txtTa.Text = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, BASE_PARAMETER_FIELDS.FIELD_TARGET).ToString();
                this.txtLowBound.Text = this.gvMainControlPlan.GetRowCellValue(e.FocusedRowHandle, BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY).ToString();

                DataRow[] drRows = dtParams.Select(string.Format(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY + "='{0}'", this.lueParames.EditValue.ToString()));
                if (drRows.Length > 0)
                {
                    this.txtUpBound.Text = drRows[0][BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY].ToString();
                    this.txtTa.Text = drRows[0][BASE_PARAMETER_FIELDS.FIELD_TARGET].ToString();
                    this.txtLowBound.Text = drRows[0][BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY].ToString();
                }

            }
            else
            {
                if (this.CtrlState == ControlState.New)
                {
                    this.txtControlCode.Text = string.Empty;
                    this.txtControlDesc.Text = string.Empty;
                    lueWeaks.EditValue = string.Empty;
                    lueSteps.EditValue = string.Empty;
                    lueParames.EditValue = string.Empty;
                    luePRODUCTCODE.EditValue = string.Empty;
                    lueCONTROLTYPE.EditValue = string.Empty;
                    txtUpBound.Text = string.Empty;
                    txtTa.Text = string.Empty;
                    txtLowBound.Text = string.Empty;
                    this.btnEditABNORMALIDS.Text = string.Empty;
                }
            }

        }
        //选择异常规则
        private void btnEditABNORMALIDS_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            #region 异常规则

            if (this.CtrlState == ControlState.Edit || this.CtrlState == ControlState.New)
            {
                string strbanormals = this.btnEditABNORMALIDS.Text.Trim();

                AbnormalControlPlan acp = new AbnormalControlPlan(strbanormals);
                if (acp.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(acp.abnormals))
                    {
                        MessageService.ShowMessage("未选择数据,请确认!");
                        return;
                    }
                    else
                        btnEditABNORMALIDS.Text = acp.abnormals;
                    //gvMainControlPlan.SetRowCellValue(e.RowHandle, SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS, acp.abnormals);
                }
            }
            #endregion
        }
    }
}
