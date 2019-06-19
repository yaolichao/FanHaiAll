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
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ProducModel : BaseUserCtrl
    {
        ProductModelEntity _proModelEntity = new ProductModelEntity();
        DataTable dtPowerAttr = new DataTable(), dtPowerLevel = new DataTable(), dtPowerCtm = new DataTable();
        string _promodel_key = string.Empty, _promodel_cp_key = string.Empty, _promodel_Level_key = string.Empty;
        bool isMain = false, isDtl_Att = false, isDtl_Level = false;
        ControlState _ctrlState = new ControlState();
        public delegate void AfterStateChanged(ControlState controlState);
        public AfterStateChanged afterStateChanged = null;
        public ProducModel()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.lblMenu.Text = "基础数据 > 工艺参数设置 > 组件型号";
            GridViewHelper.SetGridView(gvProModel);
            GridViewHelper.SetGridView(gvPowerAttr);
            GridViewHelper.SetGridView(gvPowerLevel);
            GridViewHelper.SetGridView(gvEffCtm);
            
            this.btnAdd.Text =  StringParser.Parse("${res:Global.New}");//新增
            this.btnModify.Text = StringParser.Parse("${res:Global.Update}");//修改
            this.btnDel.Text = StringParser.Parse("${res:Global.Delete}");//删除
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");//取消
            this.btSave.Text = StringParser.Parse("${res:Global.Save}");//保存

            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GroupControl.0001}");//型号设置
            PROMODEL_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0001}");//产品型号
            MEMO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0002}");//备注
            CELL_AREA.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0003}");//电池片面积
            CELL_NUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0004}");//电池片数量
            SN_LONGTH.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0005}");//序列号长度

            tab_controlparam.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.tabControl.0001}"); //控制参数设置
            btnNewAttr.Text = StringParser.Parse("${res:Global.New}");//新增
            btnDelAttr.Text = StringParser.Parse("${res:Global.Delete}");//删除
            CONTROL_OBJ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0006}");//控制对象
            CONTROL_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0007}");//控制类型
            CONTROL_VALUE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0008}"); //值

            tab_powerlevel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.tabControl.0002}"); //档位设定
            btnNewLevel.Text = StringParser.Parse("${res:Global.New}");//新增
            btnDelLevel.Text = StringParser.Parse("${res:Global.Delete}");//删除
            CREATE_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0009}");  //创建时间

            xtbgCtm.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.tabControl.0003}");//CTM范围设置
            layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.txt.0001}");//初始值率值
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.txt.0002}");//自增长幅度
            layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.txt.0003}");//行数
            sptGenerate.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.btn.0001}");//自动生成行记录
            simpleButton1.Text = StringParser.Parse("${res:Global.Delete}");//删除
            smpCtmDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.btn.0002}");//清空
            gcChose.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0009}"); //选择
            gcEffUp.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0010}"); //效率档设定上限
            gcEffLow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0011}"); //效率档设定下限
            gcCtmUp.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0012}"); //CTM上限
            gcCtmLow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.GridControl.0013}"); //CTM下限
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
                    if (isMain)
                    {
                        this.btnAdd.Enabled = false;
                        this.btnModify.Enabled = false;
                        this.btnCancel.Enabled = true;
                        this.gvProModel.OptionsBehavior.Editable = true;
                    }
                    this.gvPowerAttr.OptionsBehavior.Editable = true;
                    this.gvPowerLevel.OptionsBehavior.Editable = true;
                    this.simpleButton1.Enabled = true;
                    this.sptGenerate.Enabled = true;
                    this.smpCtmDelete.Enabled = true;
                    this.btSave.Enabled = true;
                    break;
                case ControlState.New:
                    if (isMain)
                    {
                        this.btnAdd.Enabled = false;
                        this.btnModify.Enabled = false;
                        this.gvProModel.OptionsBehavior.Editable = true;
                    }
                    this.simpleButton1.Enabled = true;
                    this.sptGenerate.Enabled = true;
                    this.smpCtmDelete.Enabled = true;
                    AddNewRow();
                    this.btnCancel.Enabled = true;
                    this.btSave.Enabled = true;
                    break;
                case ControlState.ReadOnly:
                    this.gvProModel.OptionsBehavior.Editable = false;
                    this.gvPowerAttr.OptionsBehavior.Editable = false;
                    this.gvPowerLevel.OptionsBehavior.Editable = false;
                    this.btnCancel.Enabled = false;
                    this.btnModify.Enabled = true;
                    this.btnAdd.Enabled = true;
                    this.btSave.Enabled = false;
                    this.simpleButton1.Enabled = false;
                    this.sptGenerate.Enabled = false;
                    this.smpCtmDelete.Enabled = false;
                    InitDataBind();
                    break;
                default:
                    break;
            }
            isMain = false;
            isDtl_Att = false;
            isDtl_Level = false;
        }

        private void AddNewRow()
        {
            if (isMain)
            {
                DataTable dtAddRow_ProModel = ((DataView)gvProModel.DataSource).Table;
                DataRow dr = dtAddRow_ProModel.NewRow();
                dr[BASE_PRODUCTMODEL.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                dr[BASE_PRODUCTMODEL.FIELDS_ISNEW] = BASE_DECAYCOEFFI.FIELDS_ISNEW;
                dtAddRow_ProModel.Rows.Add(dr);
                this.gcProModel.DataSource = dtAddRow_ProModel;
            }
            if (isDtl_Att && !_promodel_key.Equals(string.Empty))
            {
                DataTable dtAddRow_ProModel_cp=new DataTable();
                if (null != gvPowerAttr.DataSource)
                    dtAddRow_ProModel_cp = ((DataView)gvPowerAttr.DataSource).Table;
                else
                    dtAddRow_ProModel_cp = dtPowerAttr;
                DataRow drArr = dtAddRow_ProModel_cp.NewRow();
                drArr[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                drArr[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_KEY] = _promodel_key;
                drArr[BASE_PRODUCTMODEL.FIELDS_ISNEW] = BASE_DECAYCOEFFI.FIELDS_ISNEW;
                dtAddRow_ProModel_cp.Rows.Add(drArr);
                this.gcPowerAttr.DataSource = dtAddRow_ProModel_cp;
                this.gvPowerAttr.OptionsBehavior.Editable = true;
            }
            if (isDtl_Level && !_promodel_key.Equals(string.Empty))
            {
                DataTable dtAddRow_ProModel_Level = new DataTable();
                if (null != gvPowerLevel.DataSource)
                    dtAddRow_ProModel_Level = ((DataView)gvPowerLevel.DataSource).Table;
                else
                    dtAddRow_ProModel_Level = dtPowerLevel;
                DataRow drLevel = dtAddRow_ProModel_Level.NewRow();
                drLevel[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                drLevel[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_KEY] = _promodel_key;
                //drLevel[BASE_PRODUCTMODEL_CP.FIELDS_CREATE_TIME] = "";
                drLevel[BASE_PRODUCTMODEL_POWER.FIELDS_ISNEW] = BASE_DECAYCOEFFI.FIELDS_ISNEW;
                dtAddRow_ProModel_Level.Rows.Add(drLevel);
                this.gcPowerLevel.DataSource = dtAddRow_ProModel_Level;
                this.gvPowerLevel.OptionsBehavior.Editable = true;
            }
        }

        private void ProducModel_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(onChangeControlState);
            InitBindLue();
            this.CtrlState = ControlState.ReadOnly;
        }

        private void InitBindLue()
        {
            DataTable dtProperties = new BasePowerSetEntity().GetBasicPowerSetEngine_CommonData(string.Empty).Tables[BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtControlPara = dtProperties.Clone();
            DataRow[] drs02 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_CONTROLPARA));
            foreach (DataRow dr in drs02)
                dtControlPara.ImportRow(dr);
            repositoryItemControl_obj.DisplayMember = "Column_Name";
            repositoryItemControl_obj.ValueMember = "Column_code";
            repositoryItemControl_obj.DataSource = dtControlPara;

            DataTable dtTestRule_Sign = dtProperties.Clone();
            DataRow[] drs03 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_SIGN));
            foreach (DataRow dr in drs03)
                dtTestRule_Sign.ImportRow(dr);
            repositoryItemControl_type.DisplayMember = "Column_Name";
            repositoryItemControl_type.ValueMember = "Column_code";
            repositoryItemControl_type.DataSource = dtTestRule_Sign;
        
        }

        private void InitDataBind()
        {
            DataSet dsDataBind = _proModelEntity.GetProductModelAndCP();
            if (_proModelEntity.ErrorMsg.Equals(string.Empty))
            {
                dtPowerAttr = dsDataBind.Tables[BASE_PRODUCTMODEL_CP.DATABASE_TABLE_NAME];
                dtPowerAttr.AcceptChanges();             

                dtPowerLevel = dsDataBind.Tables[BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_NAME];
                dtPowerLevel.AcceptChanges();

                dtPowerCtm = dsDataBind.Tables[BASE_PRODUCTMODEL_CTM.DATABASE_TABLE_NAME];
                dtPowerCtm.AcceptChanges();

                DataTable dtGvProModel = dsDataBind.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_NAME];
                this.gcProModel.MainView = gvProModel;
                this.gcProModel.DataSource = null;
                this.gcProModel.DataSource = dtGvProModel;
                this.gvProModel.BestFitColumns();

                InitGvBindData();
            }
            else
                MessageService.ShowMessage(_proModelEntity.ErrorMsg);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isMain = true;
            this.CtrlState = ControlState.New;
        }
   

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            isMain = true;
            this.CtrlState = ControlState.Edit;
        }
        /// <summary>
        /// 取消作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.ReadOnly;
        }
        private bool IsValidData(DataTable dtValid)
        {
            bool bl_bak = true;
            string tableName = dtValid.TableName;
            if (BASE_PRODUCTMODEL.DATABASE_TABLE_NAME == tableName)
            {
                foreach (DataRow dr in dtValid.Rows)
                {
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME].ToString()))
                    {
                        //MessageService.ShowMessage("产品型号不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0001}")); //产品型号不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL.FIELDS_CELL_AREA].ToString()))
                    {
                        //MessageService.ShowMessage("电池片面积不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0002}")); //电池片面积不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL.FIELDS_CELL_NUM].ToString()))
                    {
                        //MessageService.ShowMessage("电池片数量不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0003}")); //电池片数量不能为空!
                        bl_bak = false;
                        break;
                    }
                }
            }
            if (BASE_PRODUCTMODEL_CP.DATABASE_TABLE_NAME == tableName)
            {
                foreach (DataRow dr in dtValid.Rows)
                {
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_CP.FIELDS_CONTROL_OBJ].ToString()))
                    {
                        //MessageService.ShowMessage("控制对象不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0004}")); //控制对象不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_CP.FIELDS_CONTROL_TYPE].ToString()))
                    {
                        //MessageService.ShowMessage("控制类型不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0005}")); //控制类型不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_CP.FIELDS_CONTROL_VALUE].ToString()))
                    {
                        //MessageService.ShowMessage("控制值不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0006}")); //控制值不能为空!
                        bl_bak = false;
                        break;
                    }
                }
            }
            if (BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_NAME == tableName)
            {
                foreach (DataRow dr in dtValid.Rows)
                {
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_POWER.FIELDS_PM].ToString()))
                    {
                        //MessageService.ShowMessage("Pm值不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0007}")); //Pm值不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_POWER.FIELDS_ISC].ToString()))
                    {
                        //MessageService.ShowMessage("Isc值不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0008}")); //Isc值不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_POWER.FIELDS_VOC].ToString()))
                    {
                        //MessageService.ShowMessage("Voc值不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0009}")); //Voc值不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_POWER.FIELDS_IMP].ToString()))
                    {
                        //MessageService.ShowMessage("Imp值不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0010}")); //Imp值不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_PRODUCTMODEL_POWER.FIELDS_VMP].ToString()))
                    {
                        //MessageService.ShowMessage("Vmp值不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0011}")); //Vmp值不能为空!
                        bl_bak = false;
                        break;
                    }
                }
            }
            return bl_bak;
            
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            DataTable dtGvModel = ((DataView)gvProModel.DataSource).Table;
            DataTable dtGvPowerAttr = ((DataView)gvPowerAttr.DataSource).Table;
            DataTable dtGvPowerLevel = ((DataView)gvPowerLevel.DataSource).Table;
            if (this.gvEffCtm.State == GridState.Editing && this.gvEffCtm.IsEditorFocused && this.gvEffCtm.EditingValueModified)
            {
                this.gvEffCtm.SetFocusedRowCellValue(this.gvEffCtm.FocusedColumn, this.gvEffCtm.EditingValue);
            }
            try
            {
                ((DataView)gvEffCtm.DataSource).Table.AcceptChanges();
            }
            catch (Exception ex)
            { }
            DataTable dtGvEffCtm = ((DataView)gvEffCtm.DataSource).Table;

            DataTable dtGvModel_Update = dtGvModel.GetChanges(DataRowState.Modified);
            DataTable dtGvPowerAttr_Update = dtGvPowerAttr.GetChanges(DataRowState.Modified);
            DataTable dtGvPowerLevel_Update = dtGvPowerLevel.GetChanges(DataRowState.Modified);
            DataTable dtGvModel_Insert = dtGvModel.GetChanges(DataRowState.Added);
            DataTable dtGvPowerAttr_Insert = dtGvPowerAttr.GetChanges(DataRowState.Added);
            DataTable dtGvPowerLevel_Insert = dtGvPowerLevel.GetChanges(DataRowState.Added);
            DataSet dsProModel = new DataSet();

            if (dtGvModel_Update != null && dtGvModel_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtGvModel_Update.Clone();
                foreach (DataRow dr in dtGvModel_Update.Rows)
                {
                    DataRow[] drUpdates = dtGvModel.Select(string.Format(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY + "='{0}'", dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtUpdate.Rows.Add(drNew);
                }
                if (!IsValidData(dtUpdate)) return;
                SaveToDataBase(dsProModel, dtUpdate, BASE_PRODUCTMODEL.DATABASE_TABLE_FORUPDATE);        
            }
            if (dtGvModel_Insert != null && dtGvModel_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtGvModel_Insert.Clone();
                foreach (DataRow dr in dtGvModel_Insert.Rows)
                {
                    DataRow[] drInserts = dtGvModel.Select(string.Format(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY + "='{0}'", dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    dtInsert.Rows.Add(drNew);
                }
                if (!IsValidData(dtInsert)) return;
                bool isExit = _proModelEntity.IsExistProductModel(dtInsert);
                if (!isExit && !string.IsNullOrEmpty(_proModelEntity.ErrorMsg))
                {
                    MessageService.ShowMessage(_proModelEntity.ErrorMsg);
                    return;
                }

                SaveToDataBase(dsProModel, dtInsert, BASE_PRODUCTMODEL.DATABASE_TABLE_FORINSERT);
            }
            //------------------------------------------------------------------------------------------------------------
            if (dtGvPowerAttr_Update != null && dtGvPowerAttr_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtGvPowerAttr_Update.Clone();
                foreach (DataRow dr in dtGvPowerAttr_Update.Rows)
                {
                    DataRow[] drUpdates = dtGvPowerAttr.Select(string.Format(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY + "='{0}'", dr[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtUpdate.Rows.Add(drNew);
                }
                if (!IsValidData(dtUpdate)) return;
                SaveToDataBase(dsProModel, dtUpdate, BASE_PRODUCTMODEL_CP.DATABASE_TABLE_FORUPDATE);
            }
            if (dtGvPowerAttr_Insert != null && dtGvPowerAttr_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtGvPowerAttr_Insert.Clone();
                foreach (DataRow dr in dtGvPowerAttr_Insert.Rows)
                {
                    DataRow[] drInserts = dtGvPowerAttr.Select(string.Format(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY + "='{0}'", dr[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    dtInsert.Rows.Add(drNew);
                }
                if (!IsValidData(dtInsert)) return;
                SaveToDataBase(dsProModel, dtInsert, BASE_PRODUCTMODEL_CP.DATABASE_TABLE_FORINSERT);        
            }
            //------------------------------------------------------------------------------------------------------------------
            if (dtGvPowerLevel_Update != null && dtGvPowerLevel_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtGvPowerLevel_Update.Clone();
                foreach (DataRow dr in dtGvPowerLevel_Update.Rows)
                {
                    DataRow[] drUpdates = dtGvPowerLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY + "='{0}'", dr[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtUpdate.Rows.Add(drNew);
                }
                if (!IsValidData(dtUpdate)) return;
                SaveToDataBase(dsProModel, dtUpdate, BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_FORUPDATE);
            }
            if (dtGvPowerLevel_Insert != null && dtGvPowerLevel_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtGvPowerLevel_Insert.Clone();
                foreach (DataRow dr in dtGvPowerLevel_Insert.Rows)
                {
                    DataRow[] drInserts = dtGvPowerLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY + "='{0}'", dr[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    dtInsert.Rows.Add(drNew);
                }
                if (!IsValidData(dtInsert)) return;
                SaveToDataBase(dsProModel, dtInsert, BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_FORINSERT);
            }
            //------------------------------------------------------------------------------------------------------------------
            SaveToDataBase(dsProModel, dtGvEffCtm, "BASE_PRODUCTMODEL_CTM_INSERT");
            bool bl_Bak = _proModelEntity.SaveProductModel(dsProModel);
            if (!bl_Bak)
            {
                //MessageService.ShowMessage("保存失败!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0012}")); //保存失败!
                return;
            }
            else
            {
                //MessageService.ShowMessage("保存成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0013}")); //保存成功!
                isMain = true;
                isDtl_Att = true;
                isDtl_Level = true;
                this.CtrlState = ControlState.ReadOnly;
                return;
                
            }
        }

        private void SaveToDataBase(DataSet database,DataTable datatable,string tableName)
        {
            datatable.TableName = tableName;
            if (datatable.Columns.Contains(BASE_PRODUCTMODEL.FIELDS_ISNEW))
                datatable.Columns.Remove(BASE_PRODUCTMODEL.FIELDS_ISNEW);
            database.Merge(datatable, true, MissingSchemaAction.Add);
        }

        private void gvPowerLevel_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gvPowerLevel.FocusedRowHandle > -1)
                _promodel_Level_key = ((DataRowView)(((GridView)sender).GetRow(e.FocusedRowHandle))).Row[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY].ToString();
        }
        private void gvPowerAttr_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gvPowerAttr.FocusedRowHandle > -1)
                _promodel_cp_key = ((DataRowView)(((GridView)sender).GetRow(e.FocusedRowHandle))).Row[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY].ToString();
        }

        private void gvPowerLevel_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_promodel_Level_key.Trim()))
            {
                try
                {
                    DataTable dtLevel = ((DataView)gvPowerLevel.DataSource).Table;
                    DataRow[] drs = dtLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY + "='{0}'", _promodel_Level_key.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch// (Exception ex)
                { }
            }
        }   
        private void gvProModel_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {

        }
        private void gvPowerAttr_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_promodel_cp_key))
            {
                try
                {
                    DataTable dtAttr = ((DataView)gvPowerAttr.DataSource).Table;
                    DataRow[] drs = dtAttr.Select(string.Format(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY + "='{0}'", _promodel_cp_key.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch //(Exception ex) 
                { }
            }
        }

        private void gvProModel_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            InitGvBindData();     
        }

        private void InitGvBindData()
        {
            if (gvProModel.FocusedRowHandle >= 0)
            {
                try
                {
                    _promodel_key = this.gvProModel.GetRowCellValue(gvProModel.FocusedRowHandle, BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY).ToString().Trim();
                    DataTable dtCommonAttr = dtPowerAttr.Clone();
                    DataTable dtCommonLevel = dtPowerLevel.Clone();
                    DataTable dtCommonCtm = dtPowerCtm.Clone();
                    DataRow[] drPowerAttrs = dtPowerAttr.Select(string.Format(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY + "='{0}'", _promodel_key));
                    DataRow[] drPowerLevels = dtPowerLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_KEY + "='{0}'", _promodel_key));
                    DataRow[] drPowerCtm = dtPowerCtm.Select(string.Format(BASE_PRODUCTMODEL_CTM.FIELDS_PROMODEL_KEY + "='{0}'", _promodel_key));
                    foreach (DataRow dr in drPowerAttrs)
                        dtCommonAttr.ImportRow(dr);
                    dtCommonAttr.AcceptChanges();
                    this.gcPowerAttr.MainView = gvPowerAttr;
                    this.gcPowerAttr.DataSource = null;
                    this.gvPowerAttr.FocusedRowHandle = -1;
                    this.gcPowerAttr.DataSource = dtCommonAttr;
                    //

                    foreach (DataRow dr in drPowerLevels)
                        dtCommonLevel.ImportRow(dr);
                    dtCommonLevel.AcceptChanges();
                    this.gcPowerLevel.MainView = gvPowerLevel;
                    this.gcPowerLevel.DataSource = null;
                    this.gvPowerLevel.FocusedRowHandle = -1;
                    this.gcPowerLevel.DataSource = dtCommonLevel;

                    foreach (DataRow dr in drPowerCtm)
                        dtCommonCtm.ImportRow(dr);
                    dtCommonCtm.AcceptChanges();
                    this.gcEffCtm.MainView = gvEffCtm;
                    this.gcEffCtm.DataSource = null;
                    this.gvEffCtm.FocusedRowHandle = -1;
                    this.gcEffCtm.DataSource = dtCommonCtm;
                }
                catch //(Exception ex)
                { }
            }
            else
            {
                this.gcPowerAttr.DataSource = null;
                this.gcPowerLevel.DataSource = null;
                this.gcEffCtm.DataSource = null;
            }
        }
 
        private void btnNewAttr_Click(object sender, EventArgs e)
        {
            isDtl_Att = true;            
            this.CtrlState = ControlState.New;
        }       
        private void btnNewLevel_Click(object sender, EventArgs e)
        {
            isDtl_Level = true;
            this.CtrlState = ControlState.New;
        }

        private void gvProModel_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvPowerAttr_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvPowerLevel_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvEffCtm_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnDelAttr_Click(object sender, EventArgs e)
        {
            //删除作业  
            if (!string.IsNullOrEmpty(_promodel_cp_key.Trim()))
            {
                try
                {
                    DataSet dsProModel = new DataSet();
                    DataTable dtAttr = ((DataView)gvPowerAttr.DataSource).Table;
                    DataTable dtAttr_del = dtAttr.Clone();
                    DataRow[] drs = dtAttr.Select(string.Format(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY + "='{0}'", _promodel_cp_key.Trim()));
                    DataRow dr = drs[0];

                    dr[BASE_PRODUCTMODEL_CP.FIELDS_ISFLAG] = 0;
                    dtAttr_del.ImportRow(dr);
                    if (dtAttr_del.Columns.Contains(BASE_PRODUCTMODEL_CP.FIELDS_ISNEW))
                        dtAttr_del.Columns.Remove(BASE_PRODUCTMODEL_CP.FIELDS_ISNEW);
                    dsProModel.Merge(dtAttr_del, true, MissingSchemaAction.Add);

                    bool bl_Bak = _proModelEntity.DelProductModel(dsProModel);
                    if (!bl_Bak)
                    {
                        //MessageService.ShowMessage("删除失败!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0014}")); //删除失败!
                    }
                    else
                    {
                        //MessageService.ShowMessage("删除成功!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0015}")); //删除成功!
                        DataRow drDel = dtPowerAttr.Select(string.Format(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY + "='{0}'", dr[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY].ToString()))[0];
                        dtPowerAttr.Rows.Remove(drDel);
                        dtPowerAttr.AcceptChanges();                        
                        InitGvBindData();                       
                    }
                }
                catch //(Exception ex) 
                { }
            }
        }

        private void btnDelLevel_Click(object sender, EventArgs e)
        {          
            //删除作业  
            if (!string.IsNullOrEmpty(_promodel_Level_key.Trim()))
            {
                try
                {
                    DataSet dsProModel = new DataSet();
                    DataTable dtLevel = ((DataView)gvPowerLevel.DataSource).Table;
                    DataTable dtLevel_del = dtLevel.Clone();
                    DataRow[] drs = dtLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY + "='{0}'", _promodel_Level_key.Trim()));
                    DataRow dr = drs[0];
                   
                    dr[BASE_PRODUCTMODEL_POWER.FIELDS_ISFLAG] = 0;
                    dtLevel_del.Rows.Add(dr.ItemArray);
                    if (dtLevel_del.Columns.Contains(BASE_PRODUCTMODEL_POWER.FIELDS_ISNEW))
                        dtLevel_del.Columns.Remove(BASE_PRODUCTMODEL_POWER.FIELDS_ISNEW);
                    dsProModel.Merge(dtLevel_del, true, MissingSchemaAction.Add);
                  
                    bool bl_Bak = _proModelEntity.DelProductModel(dsProModel);
                    if (!bl_Bak && !string.IsNullOrEmpty(_proModelEntity.ErrorMsg))
                    {
                        //MessageService.ShowMessage("删除失败!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0014}")); //删除失败!
                    }
                    else
                    {
                        //MessageService.ShowMessage("删除成功!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0015}")); //删除成功!
                        DataRow drDel = dtPowerLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY + "='{0}'", dr[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY].ToString()))[0];
                        dtPowerLevel.Rows.Remove(drDel);
                        dtPowerLevel.AcceptChanges();
                        InitGvBindData();
                    }
                }
                catch //(Exception ex) 
                { }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gvProModel.FocusedRowHandle > -1)
            {
                //if (MessageService.AskQuestion(string.Format("确定要删除{0}型号么?",gvProModel.GetFocusedRowCellValue(BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME).ToString()), "提示"))
                //{
                if (MessageService.AskQuestion(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0016}"),
                    gvProModel.GetFocusedRowCellValue(BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME).ToString()), StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0017}")))
                {
                    DataTable dtModel = ((DataView)gvProModel.DataSource).Table;
                    DataRow dr = gvProModel.GetDataRow(gvProModel.FocusedRowHandle); ;
                    dr[BASE_PRODUCTMODEL.FIELDS_ISFLAG] = 0;

                    DataSet ds_del = new DataSet();
                    DataTable dtModel_del = dtModel.Clone();
                    dtModel_del.ImportRow(dr);
                    dtModel_del.TableName = BASE_PRODUCTMODEL.DATABASE_TABLE_NAME;
                    if (dtModel_del.Columns.Contains(BASE_PRODUCTMODEL.FIELDS_ISNEW))
                        dtModel_del.Columns.Remove(BASE_PRODUCTMODEL.FIELDS_ISNEW);
                    ds_del.Tables.Add(dtModel_del);

                    bool bl_Bak = _proModelEntity.DelProductModel(ds_del);
                    if (!bl_Bak && !string.IsNullOrEmpty(_proModelEntity.ErrorMsg))
                    {
                        //MessageService.ShowMessage("删除失败!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0014}")); //删除失败!
                    }
                    else
                    {
                        //MessageService.ShowMessage("删除成功!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0015}")); //删除成功!
                        dtModel.Rows.Remove(dr);
                        dtModel.AcceptChanges();
                        gcProModel.DataSource = dtModel;
                    }
                }
            }
        }

        private void sptGenerate_Click(object sender, EventArgs e)
        {
            DataTable dtEffCtm = new DataTable();
            dtEffCtm.Columns.Add("EFF_UP", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("EFF_LOW", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("CTM_UP", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("CTM_LOW", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("PROMODEL_KEY", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("ischecked", typeof(bool));
            if (string.IsNullOrEmpty(txtInitialize.Text.ToString()))
            {
                //MessageBox.Show("未设定效率档位的初始值信息，请设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0018}"));//未设定效率档位的初始值信息，请设定！
                return;
            }
            if (string.IsNullOrEmpty(txtEffValue.Text.ToString()))
            {
                //MessageBox.Show("未设定自增长幅度信息，请设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0019}"));//未设定自增长幅度信息，请设定！
                return;
            }
            if (string.IsNullOrEmpty(txtRows.Text.ToString()))
            {
                //MessageBox.Show("未设定自动生成行数信息，请设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0020}"));//未设定自动生成行数信息，请设定！
                return;
            }
            if (this.gcEffCtm.DataSource != null)
                dtEffCtm = ((DataView)this.gvEffCtm.DataSource).Table;

            decimal initialize = Convert.ToDecimal(txtInitialize.Text.ToString());
            decimal effValue = Convert.ToDecimal(txtEffValue.Text.ToString());
            for (int i = 1; i <= Convert.ToInt32(txtRows.Text.ToString()); i++)
            {
                DataRow drEffctm = dtEffCtm.NewRow();
                drEffctm["EFF_LOW"] = initialize.ToString("0.00");
                initialize = initialize + effValue;
                drEffctm["EFF_UP"] = initialize.ToString("0.00");
                drEffctm["PROMODEL_KEY"] = _promodel_key = this.gvProModel.GetRowCellValue(gvProModel.FocusedRowHandle, BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY).ToString().Trim();
                DataRow[] drrow = dtEffCtm.Select("EFF_UP = '"+drEffctm["EFF_UP"]+"' AND EFF_LOW = '"+drEffctm["EFF_LOW"]+"'");
                if (drrow.Length > 0)
                {
                    //MessageBox.Show("已经存在行信息:效率上限-【" + drEffctm["EFF_UP"] + "】|效率下限-【" + drEffctm["EFF_LOW"] + "】", "系统提示");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0023}") + 
                        drEffctm["EFF_UP"] + 
                        StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0024}") +
                        drEffctm["EFF_LOW"] + StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0025}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    continue;
                }
                dtEffCtm.Rows.Add(drEffctm);
            }
            gcEffCtm.DataSource = dtEffCtm;
        }

        private void smpCtmDelete_Click(object sender, EventArgs e)
        {
            this.gcEffCtm.DataSource = null;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dtEffCtm = new DataTable();
            if (this.gcEffCtm.DataSource != null)
                dtEffCtm = ((DataView)this.gvEffCtm.DataSource).Table;
            else
            {
                //MessageBox.Show("列表信息为空没有需要删除的数据！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0021}"), StringParser.Parse("${res:Global.SystemInfo}"));//列表信息为空没有需要删除的数据！
                return;
            }
            DataRow[] dr = dtEffCtm.Select("ischecked = true");
            if (dr.Length > 0)
            {
                foreach (DataRow dr01 in dr)
                    dtEffCtm.Rows.Remove(dr01);
                this.gcEffCtm.DataSource = dtEffCtm;
            }
            else
            {
                //MessageBox.Show("请选择需要删除的数据信息！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ProducModel.msg.0022}"), StringParser.Parse("${res:Global.SystemInfo}"));//请选择需要删除的数据信息！
                return;
            }
            
        }
    }
}
