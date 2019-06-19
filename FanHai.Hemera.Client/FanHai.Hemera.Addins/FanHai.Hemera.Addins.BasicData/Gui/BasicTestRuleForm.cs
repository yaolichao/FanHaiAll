using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicTestRuleForm : BaseDialog
    {
        public string testRule_key = string.Empty;
        public bool isEdit = false;
        BaseTestRuleEntity _baseTestRuleEntity = new BaseTestRuleEntity();
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();
        DataTable dtPrintSet = new DataTable();
        DataTable _dtTestRule = null, dtProperties = null;
        DataSet dsProModel = null;
        
        string controlParakey = string.Empty, avgPowerkey = string.Empty, powerControlkey = string.Empty;
        string productLevelkey = string.Empty, decaykey = string.Empty, printsetkey = string.Empty;

        public BasicTestRuleForm()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.Name =  StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0015}");//测试规则设置
            xtraTabPage1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0001}");//基本信息
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0002}");//规则代码
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0003}");//规则名称
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0004}");//备注
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0005}");//功率分档
            layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0006}");//终检类型
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0007}");//功率精度
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0008}");//满托数

            xtraTabPage2.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0009}");//控制参数
            CONTROL_OBJ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0001}");//控制对象
            CONTROL_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0002}");//控制类型
            CONTROL_VALUE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0003}");//值
            btnAdd_Ctlpara.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0001}");//新增
            btnDel_Ctlpara.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0002}");//删除

            xtraTabPage3.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0010}");//平均功率
            POWERSET_KEY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0004}");//功率类型
            PS_SEQ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0005}");//功率序号
            AVGPOWER_MIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0006}");//最小功率
            AVGPOWER_MAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0007}");//最大功率
            btnAdd_AvgPower.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0001}");//新增
            btnDel_AvgPower.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0002}");//删除

            xtraTabPage4.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0011}");//功率控制
            SEQ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0008}");//序号
            gridColumn2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0009}");//功率类型
            gridColumn3.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0010}");//功率序号
            POWERCTL_OBJ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0011}");//控制对象
            POWERCTL_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0012}");//运算符
            POWERCTL_VALUE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0013}");//值
            btnAdd_PowerControl.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0001}");//新增
            btnDel_PowerControl.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0002}");//删除

            xtraTabPage5.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0012}");//产品等级
            PROLEVEL_SEQ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0014}");//序号
            GRADE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0015}");//等级
            MIN_LEVEL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0016}");//混档(主分档）
            MIN_LEVEL_DETAIL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0017}");//混档(子分档)
            MIN_COLOR.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0018}");//混花
            PALLET_GROUP.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0019}");//包装组
            btnAdd_ProductLevel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0001}");//新增
            btnDel_ProductLevel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0002}");//删除

            xtraTabPage6.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0013}");//衰减打印设置
            groupControl1.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0016}");//衰减设置
            DECAY_SQL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0020}");//序号
            DECOEFFI_KEY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0021}");//衰减系数
            DECAY_POWER_MIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0022}");//功率下线
            DECAY_POWER_MAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0023}");//功率上线
            btnAdd_Decay.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0001}");//新增
            btnDel_Decay.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0002}");//删除
            groupControl2.Text= StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.lbl.0014}");//打印标签设置
            VIEW_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0024}");//打印标签
            PRINT_QTY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0025}");//张数
            ISLABEL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0026}");//铭牌
            ISMAIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0027}");//主标签
            ISPACKAGEPRINT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.GridControl.0028}");//包装打印
            btnAdd_PrintSet.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0001}");//新增
            btnDel_PrintSet.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0002}");//删除

            btnSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0003}");//保存
            btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.btn.0004}");//取消
        }

        private void BasicTestRuleForm_Load(object sender, EventArgs e)
        {
            if (!isEdit)
                testRule_key = CommonUtils.GenerateNewKey(0);
            else
                this.txtTestRule_Code.Properties.ReadOnly = true;

            InitProductModelData();
            InitData();
        }
        /// <summary>
        /// 获得产品类型的设定信息
        /// </summary>
        private void InitProductModelData()
        {
            dsProModel = new ProductModelEntity().GetProductModelAndCP();
        }

        private void InitData()
        {
            Hashtable hsTable = new Hashtable();
            hsTable[BASE_TESTRULE.FIELDS_TESTRULE_KEY] = testRule_key;
            DataSet dsTestRuleAllData = _baseTestRuleEntity.GetTestRuleDeatilData(hsTable);
            if (!string.IsNullOrEmpty(_baseTestRuleEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_baseTestRuleEntity.ErrorMsg);
                return;
            }
            DataTable dtPowerSet = _basePowerSetEntity.GetPowerSetData(hsTable).Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
            DataView dv = dtPowerSet.DefaultView;
            DataTable dtBind = dv.ToTable(true, new string[] { BASE_POWERSET.FIELDS_PS_CODE, BASE_POWERSET.FIELDS_PS_RULE });
            luePs_Code.Properties.DisplayMember = BASE_POWERSET.FIELDS_PS_CODE;
            luePs_Code.Properties.ValueMember = BASE_POWERSET.FIELDS_PS_CODE;
            luePs_Code.Properties.DataSource = dtBind;

            BindAttributeData();
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE.DATABASE_TABLE_NAME))
                BindTestRuleData(dsTestRuleAllData.Tables[BASE_TESTRULE.DATABASE_TABLE_NAME]);
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_CTLPARA.DATABASE_TABLE_NAME))
                BindControlPara(dsTestRuleAllData.Tables[BASE_TESTRULE_CTLPARA.DATABASE_TABLE_NAME]);
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_NAME))
                BindAvgPower(dsTestRuleAllData.Tables[BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_NAME]);
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_POWERCTL.DATABASE_TABLE_NAME))
                BindPowerControl(dsTestRuleAllData.Tables[BASE_TESTRULE_POWERCTL.DATABASE_TABLE_NAME]);
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME))
                BindProductLevel(dsTestRuleAllData.Tables[BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME]);

            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_PRINTSET.DATABASE_TABLE_NAME))
                dtPrintSet = dsTestRuleAllData.Tables[BASE_TESTRULE_PRINTSET.DATABASE_TABLE_NAME];
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME))
                BindDecay(dsTestRuleAllData.Tables[BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME]);
        }

        private void BindAttributeData()
        {
            dtProperties = _basePowerSetEntity.GetBasicPowerSetEngine_CommonData(string.Empty).Tables[BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtLastTestType = dtProperties.Clone();
            DataRow[] drs01 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_LASTTESTTYPE));
            foreach (DataRow dr in drs01)
                dtLastTestType.ImportRow(dr);
            lueLast_Test_Type.Properties.DisplayMember = "Column_Name";
            lueLast_Test_Type.Properties.ValueMember = "Column_Name";
            lueLast_Test_Type.Properties.DataSource = dtLastTestType;

            DataTable dtControlPara = dtProperties.Clone();
            DataRow[] drs02 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_CONTROLPARA));
            foreach (DataRow dr in drs02)
                dtControlPara.ImportRow(dr);
            repositoryItemCtl_obj.DisplayMember = "Column_Name";
            repositoryItemCtl_obj.ValueMember = "Column_code";
            repositoryItemCtl_obj.DataSource = dtControlPara;

            DataTable dtTestRule_Sign = dtProperties.Clone();
            DataRow[] drs03 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_SIGN));
            foreach (DataRow dr in drs03)
                dtTestRule_Sign.ImportRow(dr);
            repositoryItemCtl_type.DisplayMember = "Column_Name";
            repositoryItemCtl_type.ValueMember = "Column_code";
            repositoryItemCtl_type.DataSource = dtTestRule_Sign;

            repositoryItemPowerctl_type.DisplayMember = "Column_Name";
            repositoryItemPowerctl_type.ValueMember = "Column_code";
            repositoryItemPowerctl_type.DataSource = dtTestRule_Sign;

            DataTable dtTestRule_PowerCtrl = dtProperties.Clone();
            DataRow[] drs04 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_POWERCONTROL));
            foreach (DataRow dr in drs04)
                dtTestRule_PowerCtrl.ImportRow(dr);
            repositoryItemPowerctl_obj.DisplayMember = "Column_Name";
            repositoryItemPowerctl_obj.ValueMember = "Column_code";
            repositoryItemPowerctl_obj.DataSource = dtTestRule_PowerCtrl;

            DataTable dtTestRule_Grade = dtProperties.Clone();
            DataRow[] drs05 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCT_GRADE));
            foreach (DataRow dr in drs05)
                dtTestRule_Grade.ImportRow(dr);
            repositoryItemGrade.DisplayMember = "Column_Name";
            repositoryItemGrade.ValueMember = "Column_code";
            repositoryItemGrade.DataSource = dtTestRule_Grade;
            //repositoryItemGrade

            DataTable dtlueGrid = _basePowerSetEntity.GetPowerSetData(new Hashtable()).Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
            DataView dvGrid = dtlueGrid.DefaultView;
            DataTable dtLueGridBind = dvGrid.ToTable(true, new string[] { BASE_POWERSET.FIELDS_PS_CODE, BASE_POWERSET.FIELDS_PS_SEQ, BASE_POWERSET.FIELDS_POWERSET_KEY });
            //repositoryItemGridlue_powerset_key.View.Columns[BASE_POWERSET.FIELDS_POWERSET_KEY].Visible = false;
            repositoryItemGridlue_powerset_key.DataSource = dtLueGridBind;
            repositoryItemGridlue_powerset_key.DisplayMember = BASE_POWERSET.FIELDS_PS_CODE;
            repositoryItemGridlue_powerset_key.ValueMember = BASE_POWERSET.FIELDS_POWERSET_KEY;

            repositoryItemGridlue_PowerControl.DataSource = dtLueGridBind;
            repositoryItemGridlue_PowerControl.DisplayMember = BASE_POWERSET.FIELDS_PS_CODE;
            repositoryItemGridlue_PowerControl.ValueMember = BASE_POWERSET.FIELDS_POWERSET_KEY;
            DataTable dtDecay = new DecayCoeffiEntity().GetDecayCoeffiData().Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_NAME];
            DataTable dtDecayBind = dtDecay.DefaultView.ToTable(true, new string[] { BASE_DECAYCOEFFI.FIELDS_D_CODE, 
                                                                                    BASE_DECAYCOEFFI.FIELDS_D_CODE_DESC, 
                                                                                    BASE_DECAYCOEFFI.FIELDS_D_NAME, 
                                                                                    BASE_DECAYCOEFFI.FIELDS_DIT, 
                                                                                    BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY });

            repositoryItemGridlue_DecoeffiKey.DisplayMember = BASE_DECAYCOEFFI.FIELDS_D_CODE;
            repositoryItemGridlue_DecoeffiKey.ValueMember = BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY;
            repositoryItemGridlue_DecoeffiKey.DataSource = dtDecayBind;

            DataSet dsPrint = _baseTestRuleEntity.GetPrintData();
            DataTable dtPrint = dsPrint.Tables[0];
            if (dtPrint.Columns.Contains(CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER))
                dtPrint.Columns.Remove(CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER);
            riglueLabelData.DisplayMember = "LABEL_NAME";
            riglueLabelData.ValueMember = "LABEL_ID";
            riglueLabelData.DataSource = dtPrint;
            
        }
       


        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="dtTestRule"></param>
        private void BindTestRuleData(DataTable dtTestRule)
        {
            _dtTestRule = dtTestRule;
            if (dtTestRule == null || dtTestRule.Rows.Count < 1) return;
            DataRow drTestRule=dtTestRule.Rows[0];
            this.txtTestRule_Code.Text = drTestRule[BASE_TESTRULE.FIELDS_TESTRULE_CODE].ToString();
            this.txtTestRule_Name.Text = drTestRule[BASE_TESTRULE.FIELDS_TESTRULE_NAME].ToString();
            this.mMemo.Text = drTestRule[BASE_TESTRULE.FIELDS_MEMO].ToString();
            this.luePs_Code.EditValue = drTestRule[BASE_TESTRULE.FIELDS_PS_CODE].ToString();
            this.lueLast_Test_Type.EditValue = drTestRule[BASE_TESTRULE.FIELDS_LAST_TEST_TYPE].ToString();
            this.txtPower_Degree.Text = drTestRule[BASE_TESTRULE.FIELDS_POWER_DEGREE].ToString();
            this.txtFull_Pallet_Qty.Text = drTestRule[BASE_TESTRULE.FIELDS_FULL_PALLET_QTY].ToString();
        }
        /// <summary>
        /// 控制参数
        /// </summary>
        /// <param name="dtControlPara"></param>
        private void BindControlPara(DataTable dtControlPara)
        {
            if (dtControlPara == null) return;
            this.gcCtlPara.DataSource = dtControlPara;
        }
        /// <summary>
        /// 平均功率
        /// </summary>
        /// <param name="dtAvgPower"></param>
        private void BindAvgPower(DataTable dtAvgPower)
        {
            if (dtAvgPower == null) return;
            this.gcAvgPower.DataSource = dtAvgPower;
        }
        /// <summary>
        /// 功率控制
        /// </summary>
        /// <param name="dtPowerControl"></param>
        private void BindPowerControl(DataTable dtPowerControl)
        {
            if (dtPowerControl == null) return;
            this.gcPowerControl.DataSource = dtPowerControl;
        }
        /// <summary>
        /// 产品等级
        /// </summary>
        /// <param name="dtProductLevel"></param>
        private void BindProductLevel(DataTable dtProductLevel)
        {
            if (dtProductLevel == null) return;
            this.gcProductLevel.DataSource = dtProductLevel;
        }
        /// <summary>
        /// 衰减系数
        /// </summary>
        /// <param name="dtDecay"></param>
        private void BindDecay(DataTable dtDecay)
        {
            if (dtDecay == null) return;
            this.gcDecay.DataSource = dtDecay;
        }       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataSet dsSaveTestRuleAllData = new DataSet();
            //
            if (!SaveTestRuleData(dsSaveTestRuleAllData)) return;
            //
            if (!SaveControlParaData(dsSaveTestRuleAllData)) return;
            //
            if (!SaveAvgPowerData(dsSaveTestRuleAllData)) return;
            //
            if (!SavePowerControlData(dsSaveTestRuleAllData)) return;
            //
            if (!SaveProductLevelData(dsSaveTestRuleAllData)) return;
            //
            if (!SaveDecayData(dsSaveTestRuleAllData)) return;
            //
            if (!SavePrintSetData(dsSaveTestRuleAllData)) return;

            bool bl_bak = _baseTestRuleEntity.SavePowerSetData(dsSaveTestRuleAllData);
            if (!string.IsNullOrEmpty(_baseTestRuleEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_baseTestRuleEntity.ErrorMsg);
            }
            else
            {
                //MessageService.ShowMessage("保存成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private bool IsValidRuleData()
        {
            if (string.IsNullOrEmpty(txtTestRule_Code.Text.Trim()))
            {
                //MessageService.ShowMessage("规则代码不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtTestRule_Code.Focus();
                return false;
            }
            if (!isEdit)
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add(BASE_TESTRULE.FIELDS_TESTRULE_CODE, txtTestRule_Code.Text.Trim());
                DataSet dsIsExistTestCode = _baseTestRuleEntity.GetTestRuleMainData(hsTable);
                if (!string.IsNullOrEmpty(_baseTestRuleEntity.ErrorMsg))
                {
                    MessageService.ShowError(_baseTestRuleEntity.ErrorMsg);
                    this.txtTestRule_Code.Focus();
                    this.txtTestRule_Code.SelectAll();
                    return false;
                }
                DataTable dtIsExistTestCode = dsIsExistTestCode.Tables[BASE_TESTRULE.DATABASE_TABLE_NAME];
                if (dtIsExistTestCode.Rows.Count > 0)
                {
                    //MessageService.ShowMessage("规则代码已经存在，不能重复!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.txtTestRule_Code.Focus();
                    this.txtTestRule_Code.SelectAll();
                    return false;
                }
            }

            if (string.IsNullOrEmpty(txtTestRule_Name.Text.Trim()))
            {
                //MessageService.ShowMessage("规则名称不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtTestRule_Name.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPower_Degree.Text.Trim()))
            {
                //MessageService.ShowMessage("功率精度不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtPower_Degree.Focus();
                return false;
            }
            if (luePs_Code.EditValue == null || string.IsNullOrEmpty(luePs_Code.EditValue.ToString()))
            {
                //MessageService.ShowMessage("功率分档不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtPower_Degree.Focus();
                return false;
            }
            if (lueLast_Test_Type.EditValue == null || string.IsNullOrEmpty(lueLast_Test_Type.EditValue.ToString()))
            {
                //MessageService.ShowMessage("终检类型不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtPower_Degree.Focus();
                return false;
            }

            return true;
        }
        /// <summary>
        /// 保存测试规则主数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SaveTestRuleData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidRuleData();
            if (!bl_bak) return false;

            string tableName = string.Empty;
            DataTable dtTestRule = _dtTestRule.Clone();
            DataRow drNew = dtTestRule.NewRow();
            drNew[BASE_TESTRULE.FIELDS_TESTRULE_KEY] = testRule_key;
            drNew[BASE_TESTRULE.FIELDS_TESTRULE_CODE] = txtTestRule_Code.Text.Trim();
            drNew[BASE_TESTRULE.FIELDS_TESTRULE_NAME] = txtTestRule_Name.Text.Trim();
            drNew[BASE_TESTRULE.FIELDS_MEMO] = mMemo.Text.Trim();
            drNew[BASE_TESTRULE.FIELDS_PS_CODE] = luePs_Code.EditValue == null ? "" : luePs_Code.EditValue.ToString();
            drNew[BASE_TESTRULE.FIELDS_LAST_TEST_TYPE] = lueLast_Test_Type.EditValue == null ? "" : lueLast_Test_Type.EditValue.ToString();
            drNew[BASE_TESTRULE.FIELDS_POWER_DEGREE] = txtPower_Degree.Text.Trim();
            drNew[BASE_TESTRULE.FIELDS_FULL_PALLET_QTY] = txtFull_Pallet_Qty.Text.Trim();

            if(isEdit)
                drNew[BASE_TESTRULE.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            else
             drNew[BASE_TESTRULE.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            dtTestRule.Rows.Add(drNew);
            if (isEdit)
                tableName = BASE_TESTRULE.DATABASE_TABLE_FORUPDATE;
            else
                tableName = BASE_TESTRULE.DATABASE_TABLE_FORINSERT;
            SetTableName(dsTestRuleAllData, dtTestRule, tableName);

            return true;
        }
        private bool IsValidCtrlParaData()
        {
            bool bck = true;
            if (gvCtlPara.RowCount < 1) return true;
            for (int i = 0; i < gvCtlPara.RowCount; i++)
            {
                string s_obj = this.gvCtlPara.GetRowCellValue(i, BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_OBJ).ToString();
                if (string.IsNullOrEmpty(s_obj))
                {                    
                    //MessageService.ShowMessage("控制对象不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_type = this.gvCtlPara.GetRowCellValue(i, BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_TYPE).ToString();
                if (string.IsNullOrEmpty(s_type))
                {
                    //MessageService.ShowMessage("控制类型不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_value = this.gvCtlPara.GetRowCellValue(i, BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_VALUE).ToString();
                if (string.IsNullOrEmpty(s_value))
                {
                    //MessageService.ShowMessage("控制参数值不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
            }
            return bck;
        }
        /// <summary>
        /// 保存控制参数数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SaveControlParaData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidCtrlParaData();
            if (!bl_bak) return false;

            DataTable dtControlPara = ((DataView)gvCtlPara.DataSource).Table;
            DataTable dtControlPara_Update = dtControlPara.GetChanges(DataRowState.Modified);
            DataTable dtControlPara_Insert = dtControlPara.GetChanges(DataRowState.Added);
            DataTable dtControlPara_Delete = dtControlPara.GetChanges(DataRowState.Deleted);
            DataTable dtUpdate = null;
            if (dtControlPara_Update != null && dtControlPara_Update.Rows.Count > 0)
            {
                 dtUpdate = dtControlPara_Update.Clone();
                foreach (DataRow dr in dtControlPara_Update.Rows)
                {
                    DataRow[] drUpdates = dtControlPara.Select(string.Format(BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY + "='{0}'", dr[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[BASE_TESTRULE_CTLPARA.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }               
            }
            if (dtControlPara_Delete != null && dtControlPara_Delete.Rows.Count > 0)
            {
                if (dtUpdate == null) dtUpdate = dtControlPara_Delete.Clone();
                foreach (DataRow dr in dtControlPara_Delete.Rows)
                {
                    dr[BASE_TESTRULE_CTLPARA.FIELDS_ISFLAG] = 0;
                    dr[BASE_TESTRULE_CTLPARA.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(dr.ItemArray);
                }          
            }
            if (dtUpdate != null)
                SetTableName(dsTestRuleAllData, dtUpdate, BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORUPDATE);

            if (dtControlPara_Insert != null && dtControlPara_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtControlPara_Insert.Clone();
                foreach (DataRow dr in dtControlPara_Insert.Rows)
                {
                    DataRow[] drInserts = dtControlPara.Select(string.Format(BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY + "='{0}'", dr[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    drNew[BASE_TESTRULE_CTLPARA.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                SetTableName(dsTestRuleAllData, dtInsert, BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORINSERT);                
            }

            return true;
        }

        private bool IsValidAvgPowerData()
        {
            bool bck = true;
            if (gvAvgPower.RowCount < 1) return true;
            for (int i = 0; i < gvAvgPower.RowCount; i++)
            {
                string s_type = this.gvAvgPower.GetRowCellValue(i, BASE_TESTRULE_AVGPOWER.FIELDS_POWERSET_KEY).ToString();
                if (string.IsNullOrEmpty(s_type))
                {
                    //MessageService.ShowMessage("功率类型不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0011}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_seq = this.gvAvgPower.GetRowCellValue(i, BASE_TESTRULE_AVGPOWER.FIELDS_PS_SEQ).ToString();
                if (string.IsNullOrEmpty(s_seq))
                {
                    //MessageService.ShowMessage("序号不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0012}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_minpower = this.gvAvgPower.GetRowCellValue(i, BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MIN).ToString();
                if (string.IsNullOrEmpty(s_minpower))
                {
                    //MessageService.ShowMessage("最小功率不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0013}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_maxpower = this.gvAvgPower.GetRowCellValue(i, BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MAX).ToString();
                if (string.IsNullOrEmpty(s_maxpower))
                {
                    //MessageService.ShowMessage("最大功率不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0014}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
            }
            return bck;
        }
        /// <summary>
        /// 保存平均功率数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SaveAvgPowerData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidAvgPowerData();
            if (!bl_bak) return false;

            DataTable dtAvgPower = ((DataView)gvAvgPower.DataSource).Table;
            DataTable dtAvgPower_Update = dtAvgPower.GetChanges(DataRowState.Modified);
            DataTable dtAvgPower_Insert = dtAvgPower.GetChanges(DataRowState.Added);
            DataTable dtAvgPower_Delete = dtAvgPower.GetChanges(DataRowState.Deleted);
            DataTable dtUpdate = null;
            if (dtAvgPower_Update != null && dtAvgPower_Update.Rows.Count > 0)
            {
                 dtUpdate = dtAvgPower_Update.Clone();
                foreach (DataRow dr in dtAvgPower_Update.Rows)
                {
                    DataRow[] drUpdates = dtAvgPower.Select(string.Format(BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY + "='{0}'", dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[BASE_TESTRULE_AVGPOWER.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }                
            }
            if (dtAvgPower_Delete != null && dtAvgPower_Delete.Rows.Count > 0)
            {
                if (dtUpdate == null) dtUpdate = dtAvgPower_Delete.Clone();
                foreach (DataRow dr in dtAvgPower_Delete.Rows)
                {
                    dr[BASE_TESTRULE_AVGPOWER.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dr[BASE_TESTRULE_AVGPOWER.FIELDS_ISFLAG] = 0;
                    dtUpdate.Rows.Add(dr.ItemArray);
                }
            }
            if (dtUpdate != null)
                SetTableName(dsTestRuleAllData, dtUpdate, BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORUPDATE);

            if (dtAvgPower_Insert != null && dtAvgPower_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtAvgPower_Insert.Clone();
                foreach (DataRow dr in dtAvgPower_Insert.Rows)
                {
                    DataRow[] drInserts = dtAvgPower.Select(string.Format(BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY + "='{0}'", dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    drNew[BASE_TESTRULE_AVGPOWER.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                SetTableName(dsTestRuleAllData, dtInsert, BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORINSERT);
            }
            return true;
        }

        private bool IsValidPowerControlData()
        {
            bool bck = true;
            if (gvPowerControl.RowCount < 1) return true;
            for (int i = 0; i < gvPowerControl.RowCount; i++)
            {
                string s_seq = this.gvPowerControl.GetRowCellValue(i, BASE_TESTRULE_POWERCTL.FIELDS_SEQ).ToString();
                if (string.IsNullOrEmpty(s_seq))
                {
                    //MessageService.ShowMessage("功率控制序号不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0015}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_type = this.gvPowerControl.GetRowCellValue(i, BASE_TESTRULE_POWERCTL.FIELDS_POWERCTL_TYPE).ToString();
                if (string.IsNullOrEmpty(s_type))
                {
                    //MessageService.ShowMessage("功率控制类型不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0016}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_ps_sql = this.gvPowerControl.GetRowCellValue(i, BASE_TESTRULE_POWERCTL.FIELDS_PS_SEQ).ToString();
                if (string.IsNullOrEmpty(s_ps_sql))
                {
                    //MessageService.ShowMessage("功率序号不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0017}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_obj = this.gvPowerControl.GetRowCellValue(i, BASE_TESTRULE_POWERCTL.FIELDS_POWERCTL_OBJ).ToString();
                if (string.IsNullOrEmpty(s_obj))
                {
                    //MessageService.ShowMessage("控制对象不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0018}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_sign = this.gvPowerControl.GetRowCellValue(i, BASE_TESTRULE_POWERCTL.FIELDS_POWERCTL_TYPE).ToString();
                if (string.IsNullOrEmpty(s_sign))
                {
                    //MessageService.ShowMessage("运算符不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0019}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_value = this.gvPowerControl.GetRowCellValue(i, BASE_TESTRULE_POWERCTL.FIELDS_POWERCTL_VALUE).ToString();
                if (string.IsNullOrEmpty(s_value))
                {
                    //MessageService.ShowMessage("功率控制值不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0020}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
            }
            return bck;
        }
        /// <summary>
        /// 保存功率控制数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SavePowerControlData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidPowerControlData();
            if (!bl_bak) return false;

            DataTable dtPowerControl = ((DataView)gvPowerControl.DataSource).Table;
            DataTable dtPowerControl_Update = dtPowerControl.GetChanges(DataRowState.Modified);
            DataTable dtPowerControl_Insert = dtPowerControl.GetChanges(DataRowState.Added);
            DataTable dtPowerControl_Delete = dtPowerControl.GetChanges(DataRowState.Deleted);
            DataTable dtUpdate = null;
            if (dtPowerControl_Update != null && dtPowerControl_Update.Rows.Count > 0)
            {
                 dtUpdate = dtPowerControl_Update.Clone();
                foreach (DataRow dr in dtPowerControl_Update.Rows)
                {
                    DataRow[] drUpdates = dtPowerControl.Select(string.Format(BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY + "='{0}'", dr[BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[BASE_TESTRULE_POWERCTL.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }               
            }
            if (dtPowerControl_Delete != null && dtPowerControl_Delete.Rows.Count > 0)
            {
                if (dtUpdate == null) dtUpdate = dtPowerControl_Delete.Clone();
                foreach (DataRow dr in dtPowerControl_Delete.Rows)
                {
                    dr[BASE_TESTRULE_POWERCTL.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dr[BASE_TESTRULE_POWERCTL.FIELDS_ISFLAG] = 0;
                    dtUpdate.Rows.Add(dr.ItemArray);
                }
            }
            if (dtUpdate != null)
                SetTableName(dsTestRuleAllData, dtUpdate, BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORUPDATE);

            if (dtPowerControl_Insert != null && dtPowerControl_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtPowerControl_Insert.Clone();
                foreach (DataRow dr in dtPowerControl_Insert.Rows)
                {
                    DataRow[] drInserts = dtPowerControl.Select(string.Format(BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY + "='{0}'", dr[BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    drNew[BASE_TESTRULE_POWERCTL.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                SetTableName(dsTestRuleAllData, dtInsert, BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORINSERT);
            }
            return true;
        }
        private bool IsValidProductLevelData()
        {
            bool bck = true;
            if (gvProductLevel.RowCount < 1) return true;
            for (int i = 0; i < gvProductLevel.RowCount; i++)
            {
                string s_seq = this.gvProductLevel.GetRowCellValue(i, BASE_TESTRULE_PROLEVEL.FIELDS_PROLEVEL_SEQ).ToString();
                if (string.IsNullOrEmpty(s_seq))
                {
                    //MessageService.ShowMessage("产品等级序号不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0021}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_grade = this.gvProductLevel.GetRowCellValue(i, BASE_TESTRULE_PROLEVEL.FIELDS_GRADE).ToString();
                if (string.IsNullOrEmpty(s_grade))
                {
                    //MessageService.ShowMessage("产品等级不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0022}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                else
                {
                    int qty = 0;
                    for (int j = 0; j < gvProductLevel.RowCount; j++)
                    {
                        string grade = this.gvProductLevel.GetRowCellValue(j, BASE_TESTRULE_PROLEVEL.FIELDS_GRADE).ToString();
                        if (grade.Equals(s_grade))
                            qty++;
                    }
                    if (qty > 1)
                    {
                        //MessageService.ShowMessage("【产品等级】不能设定重复!", "提示");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0023}"), StringParser.Parse("${res:Global.SystemInfo}"));
                        bck = false;
                        break;
                    }
                }
            }
            return bck;
        }
        /// <summary>
        /// 保存产品等级数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SaveProductLevelData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidProductLevelData();
            if (!bl_bak) return false;


            DataTable dtProductLevel = ((DataView)gvProductLevel.DataSource).Table;
            DataTable dtProductLevel_Update = dtProductLevel.GetChanges(DataRowState.Modified);
            DataTable dtProductLevel_Insert = dtProductLevel.GetChanges(DataRowState.Added);
            DataTable dtProductLevel_Delete = dtProductLevel.GetChanges(DataRowState.Deleted);
            DataTable dtUpdate = null;
            if (dtProductLevel_Update != null && dtProductLevel_Update.Rows.Count > 0)
            {
                 dtUpdate = dtProductLevel_Update.Clone();
                foreach (DataRow dr in dtProductLevel_Update.Rows)
                {
                    DataRow[] drUpdates = dtProductLevel.Select(string.Format(BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY + "='{0}'", dr[BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[BASE_TESTRULE_DECAY.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }              
            }
            if (dtProductLevel_Delete != null && dtProductLevel_Delete.Rows.Count > 0)
            {
                if (dtUpdate == null) dtUpdate = dtProductLevel_Delete.Clone();
                foreach (DataRow dr in dtProductLevel_Delete.Rows)
                {
                    dr[BASE_TESTRULE_DECAY.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dr[BASE_TESTRULE_PROLEVEL.FIELDS_ISFLAG] = 0;
                    dtUpdate.Rows.Add(dr.ItemArray);
                }
            }
            if (dtUpdate != null)
                SetTableName(dsTestRuleAllData, dtUpdate, BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORUPDATE);

            if (dtProductLevel_Insert != null && dtProductLevel_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtProductLevel_Insert.Clone();
                foreach (DataRow dr in dtProductLevel_Insert.Rows)
                {
                    DataRow[] drInserts = dtProductLevel.Select(string.Format(BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY + "='{0}'", dr[BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    drNew[BASE_TESTRULE_DECAY.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                SetTableName(dsTestRuleAllData, dtInsert, BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORINSERT);
            }
            return true;
        }
        private bool IsValidDecayData()
        {
            bool bck = true;
            if (gvDecay.RowCount < 1) return true;
            for (int i = 0; i < gvDecay.RowCount; i++)
            {
                string s_seq = this.gvDecay.GetRowCellValue(i, BASE_TESTRULE_DECAY.FIELDS_DECAY_SQL).ToString();
                if (string.IsNullOrEmpty(s_seq))
                {
                    //MessageService.ShowMessage("衰减序号不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0024}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_decay = this.gvDecay.GetRowCellValue(i, BASE_TESTRULE_DECAY.FIELDS_DECOEFFI_KEY).ToString();
                if (string.IsNullOrEmpty(s_decay))
                {
                    //MessageService.ShowMessage("衰减系数不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0025}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_minpower = this.gvDecay.GetRowCellValue(i, BASE_TESTRULE_DECAY.FIELDS_DECAY_POWER_MIN).ToString();
                if (string.IsNullOrEmpty(s_minpower))
                {
                    //MessageService.ShowMessage("功率下线不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0026}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
                string s_maxpower = this.gvDecay.GetRowCellValue(i, BASE_TESTRULE_DECAY.FIELDS_DECAY_POWER_MAX).ToString();
                if (string.IsNullOrEmpty(s_maxpower))
                {
                    //MessageService.ShowMessage("功率上线不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0027}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    bck = false;
                    break;
                }
            }
            return bck;
        }
        /// <summary>
        /// 保存衰减功率设定数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SaveDecayData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidDecayData();
            if (!bl_bak) return false;

            DataTable dtDecay = ((DataView)gvDecay.DataSource).Table;
            DataTable dtDecay_Update = dtDecay.GetChanges(DataRowState.Modified);
            DataTable dtDecay_Insert = dtDecay.GetChanges(DataRowState.Added);
            DataTable dtDecay_Delete= dtDecay.GetChanges(DataRowState.Deleted);
            DataTable dtUpdate = null;
            if (dtDecay_Update != null && dtDecay_Update.Rows.Count > 0)
            {
                 dtUpdate = dtDecay_Update.Clone();
                foreach (DataRow dr in dtDecay_Update.Rows)
                {
                    DataRow[] drUpdates = dtDecay.Select(string.Format(BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY + "='{0}'", dr[BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[BASE_TESTRULE_DECAY.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }               
            }
            if (dtDecay_Delete != null && dtDecay_Delete.Rows.Count > 0)
            {
                if (dtUpdate == null) dtUpdate = dtDecay_Delete.Clone();
                foreach (DataRow dr in dtDecay_Delete.Rows)
                {
                    dr[BASE_TESTRULE_DECAY.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dr[BASE_TESTRULE_DECAY.FIELDS_ISFLAG] = 0;
                    dtUpdate.Rows.Add(dr.ItemArray);
                }
            }
            if (dtUpdate != null)
                SetTableName(dsTestRuleAllData, dtUpdate, BASE_TESTRULE_DECAY.DATABASE_TABLE_FORUPDATE);

            if (dtDecay_Insert != null && dtDecay_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtDecay_Insert.Clone();
                foreach (DataRow dr in dtDecay_Insert.Rows)
                {
                    DataRow[] drInserts = dtDecay.Select(string.Format(BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY + "='{0}'", dr[BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    drNew[BASE_TESTRULE_DECAY.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                SetTableName(dsTestRuleAllData, dtInsert, BASE_TESTRULE_DECAY.DATABASE_TABLE_FORINSERT);
            }
            return true;
        }

        private bool IsValidPrintSetData()
        {
            bool bck = true;
            if (gvDecay.RowCount < 1) return true;
            //打印主标签设定
            for (int i = 0; i < gvPrintSet.RowCount; i++)
            {
                string s_ismain = this.gvPrintSet.GetRowCellValue(i,BASE_TESTRULE_PRINTSET.FIELDS_ISMAIN).ToString();
                if (!bool.TryParse(s_ismain, out bck))
                {                   
                    bck = false;                
                }

                if (bck == true)
                    break;
            }

            if (!bck)
            {
                //MessageService.ShowMessage("请设定打印主标签!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0028}"), StringParser.Parse("${res:Global.SystemInfo}"));
            }
            else
            {
                //包装工序是否打印设定
                for (int i = 0; i < gvPrintSet.RowCount; i++)
                {
                    string s_isPackage = this.gvPrintSet.GetRowCellValue(i, BASE_TESTRULE_PRINTSET.FIELDS_ISPACKAGEPRINT).ToString();
                    s_isPackage = s_isPackage == string.Empty ? "false" : s_isPackage;

                    if (bool.Parse(s_isPackage))
                    {
                        string packageQty = this.gvPrintSet.GetRowCellValue(i, BASE_TESTRULE_PRINTSET.FIELDS_PRINT_QTY).ToString();
                        if (packageQty.Trim().Equals(string.Empty) || Convert.ToInt16(packageQty) < 2)
                        {
                            //MessageService.ShowMessage("包装打印需要设置大于【1】张，请确认!");
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0029}"), StringParser.Parse("${res:Global.SystemInfo}"));
                            bck = false;
                            break;
                        }
                    }
                }               
            }
            return bck;
        }
        /// <summary>
        /// 保存打印设定数据
        /// </summary>
        /// <param name="dsTestRuleAllData"></param>
        private bool SavePrintSetData(DataSet dsTestRuleAllData)
        {
            bool bl_bak = IsValidPrintSetData();
            if (!bl_bak) return false;

            try
            {               
                DataTable dtPrintSet = ((DataView)gvPrintSet.DataSource).Table;
                DataTable dtPrintSet_Update = dtPrintSet.GetChanges(DataRowState.Modified);
                DataTable dtPrintSet_Insert = dtPrintSet.GetChanges(DataRowState.Added);
                DataTable dtPrintSet_Delete = dtPrintSet.GetChanges(DataRowState.Deleted);
                DataTable dtUpdate = null;
                if (dtPrintSet_Update != null && dtPrintSet_Update.Rows.Count > 0)
                {
                    dtUpdate = dtPrintSet_Update.Clone();
                    foreach (DataRow dr in dtPrintSet_Update.Rows)
                    {
                        DataRow[] drUpdates = dtPrintSet.Select(string.Format(BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY + "='{0}'", dr[BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY].ToString()));
                        DataRow drNew = dtUpdate.NewRow();
                        for (int i = 0; i < dtUpdate.Columns.Count; i++)
                        {
                            drNew[i] = drUpdates[0][i];
                        }
                        drNew[BASE_TESTRULE_PRINTSET.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        dtUpdate.Rows.Add(drNew);
                    }
                }
                if (dtPrintSet_Delete != null && dtPrintSet_Delete.Rows.Count > 0)
                {
                    if (dtUpdate == null) dtUpdate = dtPrintSet_Delete.Clone();
                    foreach (DataRow dr in dtPrintSet_Delete.Rows)
                    {
                        dr[BASE_TESTRULE_PRINTSET.FIELDS_ISFLAG] = 0;
                        dtUpdate.Rows.Add(dr.ItemArray);
                    }
                }
                if (dtUpdate != null)
                    SetTableName(dsTestRuleAllData, dtUpdate, BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORUPDATE);

                if (dtPrintSet_Insert != null && dtPrintSet_Insert.Rows.Count > 0)
                {
                    DataTable dtInsert = dtPrintSet_Insert.Clone();
                    foreach (DataRow dr in dtPrintSet_Insert.Rows)
                    {
                        DataRow[] drInserts = dtPrintSet.Select(string.Format(BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY + "='{0}'", dr[BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY].ToString()));
                        DataRow drNew = dtInsert.NewRow();
                        for (int i = 0; i < dtInsert.Columns.Count; i++)
                        {
                            drNew[i] = drInserts[0][i];
                        }
                        drNew[BASE_TESTRULE_PRINTSET.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        dtInsert.Rows.Add(drNew);
                    }
                    SetTableName(dsTestRuleAllData, dtInsert, BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORINSERT);
                }
            }
            catch //(Exception ex) 
            { }

            return bl_bak;
        }
        private void SetTableName(DataSet ds, DataTable dt, string dtName)
        {
            dt.TableName = dtName;
            ds.Merge(dt, true, MissingSchemaAction.Add);
        }

        private void gvCtlPara_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(controlParakey.Trim()))
            {
                try
                {
                    DataTable dtCommon = ((DataView)gvCtlPara.DataSource).Table;
                    DataRow[] drs = dtCommon.Select(string.Format(BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY + "='{0}'", controlParakey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch //(Exception ex) 
                { }
            }
        }

        private void gvCtlPara_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
                controlParakey = gvCtlPara.GetRowCellValue(e.FocusedRowHandle, BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY).ToString();
        }

        private void gvAvgPower_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
                avgPowerkey = gvAvgPower.GetRowCellValue(e.FocusedRowHandle, BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY).ToString();
        }

        private void gvAvgPower_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(controlParakey.Trim()))
            {
                try
                {
                    DataTable dtCommon = ((DataView)gvAvgPower.DataSource).Table;
                    DataRow[] drs = dtCommon.Select(string.Format(BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY + "='{0}'", avgPowerkey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;

                    if (e.Column.FieldName == BASE_TESTRULE_AVGPOWER.FIELDS_POWERSET_KEY)
                    {
                        DataTable dtPowerSet = repositoryItemGridlue_powerset_key.DataSource as DataTable;
                        DataRow[] drSeq = dtPowerSet.Select(string.Format(BASE_POWERSET.FIELDS_POWERSET_KEY + "='{0}'", e.Value));
                        drs[0][BASE_TESTRULE_AVGPOWER.FIELDS_PS_CODE] = drSeq[0][BASE_POWERSET.FIELDS_PS_CODE];
                        gvAvgPower.SetRowCellValue(e.RowHandle, BASE_TESTRULE_AVGPOWER.FIELDS_PS_SEQ, drSeq[0][BASE_POWERSET.FIELDS_PS_SEQ]);
                    }
                }
                catch //(Exception ex)
                { }
            }
        }

        private void gvPowerControl_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
                powerControlkey = gvPowerControl.GetRowCellValue(e.FocusedRowHandle, BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY).ToString();
        }

        private void gvPowerControl_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(controlParakey.Trim()))
            {
                try
                {
                    DataTable dtCommon = ((DataView)gvPowerControl.DataSource).Table;
                    DataRow[] drs = dtCommon.Select(string.Format(BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY + "='{0}'", powerControlkey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;

                    if (e.Column.FieldName == BASE_TESTRULE_POWERCTL.FIELDS_POWERSET_KEY)
                    {
                        DataTable dtPowerSet = repositoryItemGridlue_PowerControl.DataSource as DataTable;
                        DataRow[] drSeq = dtPowerSet.Select(string.Format(BASE_POWERSET.FIELDS_POWERSET_KEY + "='{0}'", e.Value));
                        drs[0][BASE_TESTRULE_POWERCTL.FIELDS_PS_CODE] = drSeq[0][BASE_POWERSET.FIELDS_PS_CODE];
                        gvPowerControl.SetRowCellValue(e.RowHandle, BASE_TESTRULE_POWERCTL.FIELDS_PS_SEQ, drSeq[0][BASE_POWERSET.FIELDS_PS_SEQ]);
                    }
                }
                catch //(Exception ex)
                { }
            }
        }

        private void gvProductLevel_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
                productLevelkey = gvProductLevel.GetRowCellValue(e.FocusedRowHandle, BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY).ToString();
        }

        private void gvProductLevel_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(controlParakey.Trim()))
            {
                try
                {
                    DataTable dtCommon = ((DataView)gvProductLevel.DataSource).Table;
                    DataRow[] drs = dtCommon.Select(string.Format(BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY + "='{0}'", productLevelkey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch //(Exception ex) 
                { }
            }
        }

        private void gvDecay_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            InitPrintSetData();
        }

        private void gvDecay_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(decaykey.Trim()))
            {
                try
                {
                    DataTable dtCommon = ((DataView)gvDecay.DataSource).Table;
                    DataRow[] drs = dtCommon.Select(string.Format(BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY + "='{0}'", decaykey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch //(Exception ex) 
                { }
            }
        }


        private void InitPrintSetData()
        {
            try
            {
                if (gvDecay.FocusedRowHandle > -1)
                {
                    decaykey = gvDecay.GetRowCellValue(gvDecay.FocusedRowHandle, BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY).ToString();
                    DataRow[] drPrints = dtPrintSet.Select(string.Format(BASE_TESTRULE_PRINTSET.FIELDS_DECAY_KEY + "='{0}'", decaykey));
                    DataTable dtPrintSet_For_SelectData = dtPrintSet.Clone();
                    foreach (DataRow dr in drPrints)
                        dtPrintSet_For_SelectData.ImportRow(dr);
                    dtPrintSet_For_SelectData.AcceptChanges();

                    this.gcPrintSet.DataSource = dtPrintSet_For_SelectData;
                }
                else
                    this.gcPrintSet.DataSource = null;
            }
            catch //(Exception ex)
            { }
        }

        private void btnAdd_Ctlpara_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCtlPara = ((DataView)gvCtlPara.DataSource).Table;
                if (dtCtlPara != null && dtCtlPara.Rows.Count < 1)
                {
                    bool isExistPname = false;
                    string proModelStr = txtTestRule_Name.Text.Trim() != "" ? txtTestRule_Name.Text.Trim() : (txtTestRule_Code.Text.Trim() != "" ? txtTestRule_Code.Text.Trim() : "");
                    DataTable dtProModel = dsProModel.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_NAME];
                    foreach (DataRow dr in dtProModel.Rows)
                    {
                        string pname = dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME].ToString();
                        if (proModelStr.Contains(pname))
                        {
                            string pnKey = dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY].ToString();
                            isExistPname = true;
                            DataTable dtCtrlPara = dsProModel.Tables[BASE_PRODUCTMODEL_CP.DATABASE_TABLE_NAME];
                            DataRow[] drCtrlParas = dtCtrlPara.Select(string.Format(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_KEY + "='{0}'", pnKey));
                            foreach (DataRow dr01 in drCtrlParas)
                            {
                                DataRow drNew = dtCtlPara.NewRow();
                                drNew[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY] = CommonUtils.GenerateNewKey(0);
                                drNew[BASE_TESTRULE_CTLPARA.FIELDS_TESTRULE_KEY] = testRule_key;
                                drNew[BASE_TESTRULE_CTLPARA.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                                drNew[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_OBJ] = dr01[BASE_PRODUCTMODEL_CP.FIELDS_CONTROL_OBJ];
                                drNew[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_TYPE] = dr01[BASE_PRODUCTMODEL_CP.FIELDS_CONTROL_TYPE];
                                drNew[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_VALUE] = dr01[BASE_PRODUCTMODEL_CP.FIELDS_CONTROL_VALUE];
                                dtCtlPara.Rows.Add(drNew);
                            }
                            break;
                        }
                    }
                    if (!isExistPname)
                    {
                        //MessageService.ShowMessage("规则代码中需要包含产品类型!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0030}"), StringParser.Parse("${res:Global.SystemInfo}"));
                        return;
                    }
                }
                else
                {
                    DataRow drNew = dtCtlPara.NewRow();
                    drNew[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY] = CommonUtils.GenerateNewKey(0);
                    drNew[BASE_TESTRULE_CTLPARA.FIELDS_TESTRULE_KEY] = testRule_key;
                    drNew[BASE_TESTRULE_CTLPARA.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtCtlPara.Rows.Add(drNew);                   
                }
                this.gcCtlPara.DataSource = dtCtlPara;
            }
            catch//(Exception ex)
            {}
        }

        private void btnDel_Ctlpara_Click(object sender, EventArgs e)
        {
            if (gvCtlPara.FocusedRowHandle > -1&&!string.IsNullOrEmpty(controlParakey))
            {
                //if (MessageService.AskQuestion("确定要删除选定的数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {                           
                    DataTable dtCtlPara = ((DataView)gvCtlPara.DataSource).Table;
                    DataRow[] drtCtlParas = dtCtlPara.Select(string.Format(BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY + "='{0}'", controlParakey));
                    DataTable dtDel = dtCtlPara.Clone();
                    drtCtlParas[0][BASE_TESTRULE_CTLPARA.FIELDS_ISFLAG] = 0;
                    dtDel.Rows.Add(drtCtlParas[0].ItemArray);
                    bool bck = DeleteGridViewCommonData(dtDel, BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORUPDATE);
                    if (bck)
                    {
                        dtCtlPara.Rows.Remove(drtCtlParas[0]);
                        this.gcCtlPara.MainView = gvCtlPara;
                        this.gcCtlPara.DataSource = null;
                        this.gvCtlPara.FocusedRowHandle = -1;
                        this.gcCtlPara.DataSource = dtCtlPara;
                    }
                }
            }          
        }

        private bool DeleteGridViewCommonData( DataTable dtDel,string tablename)
        {
            DataSet dsDel = new DataSet();
            dtDel.TableName = tablename;
            dsDel.Merge(dtDel, true, MissingSchemaAction.Add);

            bool bl_bak = _baseTestRuleEntity.SavePowerSetData(dsDel);
            if (!bl_bak && !string.IsNullOrEmpty(_baseTestRuleEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_baseTestRuleEntity.ErrorMsg);
            }
            return bl_bak;
        }

        private void btnAdd_AvgPower_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtAvgPower = ((DataView)gvAvgPower.DataSource).Table;
                DataRow drNew = dtAvgPower.NewRow();
                drNew[BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[BASE_TESTRULE_AVGPOWER.FIELDS_TESTRULE_KEY] = testRule_key;
                dtAvgPower.Rows.Add(drNew);
                this.gcAvgPower.DataSource = dtAvgPower;
            }
            catch//(Exception ex) 
            { }
        }

        private void btnDel_AvgPower_Click(object sender, EventArgs e)
        {
            if (gvAvgPower.FocusedRowHandle > -1 && !string.IsNullOrEmpty(avgPowerkey))
            {
                //if (MessageService.AskQuestion("确定要删除选定的数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataTable dtAvgPower = ((DataView)gvAvgPower.DataSource).Table;
                    DataRow[] drAvgPowers = dtAvgPower.Select(string.Format(BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY + "='{0}'", avgPowerkey));
                    DataTable dtDel = dtAvgPower.Clone();
                    drAvgPowers[0][BASE_TESTRULE_AVGPOWER.FIELDS_ISFLAG] = 0;
                    dtDel.Rows.Add(drAvgPowers[0].ItemArray);
                    bool bck = DeleteGridViewCommonData(dtDel, BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORUPDATE);
                    if (bck)
                    {
                        dtAvgPower.Rows.Remove(drAvgPowers[0]);
                        this.gcAvgPower.MainView = gvAvgPower;
                        this.gcAvgPower.DataSource = null;
                        this.gvAvgPower.FocusedRowHandle = -1;
                        this.gcAvgPower.DataSource = dtAvgPower;
                    }                                       
                }
            }
        }

        private void btnAdd_PowerControl_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPowerControl = ((DataView)gvPowerControl.DataSource).Table;
                DataRow drNew = dtPowerControl.NewRow();
                drNew[BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[BASE_TESTRULE_POWERCTL.FIELDS_TESTRULE_KEY] = testRule_key;
                dtPowerControl.Rows.Add(drNew);
                this.gcPowerControl.DataSource = dtPowerControl;
            }
            catch //(Exception ex)
            { }
        }

        private void btnDel_PowerControl_Click(object sender, EventArgs e)
        {
            if (gvPowerControl.FocusedRowHandle > -1 && !string.IsNullOrEmpty(powerControlkey))
            {
                //if (MessageService.AskQuestion("确定要删除选定的数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataTable dtPowerControl = ((DataView)gvPowerControl.DataSource).Table;
                    DataRow[] drPowerControls = dtPowerControl.Select(string.Format(BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY + "='{0}'", powerControlkey));
                    DataTable dtDel = dtPowerControl.Clone();
                    drPowerControls[0][BASE_TESTRULE_POWERCTL.FIELDS_ISFLAG] = 0;
                    dtDel.Rows.Add(drPowerControls[0].ItemArray);

                    bool bck = DeleteGridViewCommonData(dtDel, BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORUPDATE);
                    if (bck)
                    {
                        dtPowerControl.Rows.Remove(drPowerControls[0]);
                        this.gcPowerControl.MainView = gvPowerControl;
                        this.gcPowerControl.DataSource = null;
                        this.gvPowerControl.FocusedRowHandle = -1;
                        this.gcPowerControl.DataSource = dtPowerControl;
                    }   
                    
                }
            }
        }

        private void btnAdd_ProductLevel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtProductLevel = ((DataView)gvProductLevel.DataSource).Table;
                DataRow drNew = dtProductLevel.NewRow();
                drNew[BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[BASE_TESTRULE_PROLEVEL.FIELDS_TESTRULE_KEY] = testRule_key;
                drNew[BASE_TESTRULE_PROLEVEL.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dtProductLevel.Rows.Add(drNew);
                this.gcProductLevel.DataSource = dtProductLevel;
            }
            catch //(Exception ex) 
            { }
        }

        private void btnDel_ProductLevel_Click(object sender, EventArgs e)
        {
            if (gvProductLevel.FocusedRowHandle > -1 && !string.IsNullOrEmpty(productLevelkey))
            {
                //if (MessageService.AskQuestion("确定要删除选定的数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataTable dtProductLevel = ((DataView)gvProductLevel.DataSource).Table;
                    DataRow[] drPowerControls = dtProductLevel.Select(string.Format(BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY + "='{0}'", productLevelkey));
                    DataTable dtDel = dtProductLevel.Clone();
                    drPowerControls[0][BASE_TESTRULE_PROLEVEL.FIELDS_ISFLAG] = 0;
                    dtDel.Rows.Add(drPowerControls[0].ItemArray);

                    bool bck = DeleteGridViewCommonData(dtDel, BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORUPDATE);
                    if (bck)
                    {
                        dtProductLevel.Rows.Remove(drPowerControls[0]);
                        this.gcProductLevel.MainView = gvProductLevel;
                        this.gcProductLevel.DataSource = null;
                        this.gvProductLevel.FocusedRowHandle = -1;
                        this.gcProductLevel.DataSource = dtProductLevel;
                    }
                }
            }
        }

        private void btnAdd_Decay_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDecay = ((DataView)gvDecay.DataSource).Table;
                DataRow drNew = dtDecay.NewRow();
                drNew[BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[BASE_TESTRULE_DECAY.FIELDS_TESTRULE_KEY] = testRule_key;
                drNew[BASE_TESTRULE_DECAY.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dtDecay.Rows.Add(drNew);
                this.gcDecay.DataSource = dtDecay;
            }
            catch //(Exception ex) 
            { }
        }

        private void btnDel_Decay_Click(object sender, EventArgs e)
        {
            if (gvDecay.FocusedRowHandle > -1 && !string.IsNullOrEmpty(decaykey))
            {
                //if (MessageService.AskQuestion("确定要删除选定的数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataTable dtPrintSet = ((DataView)gvPrintSet.DataSource).Table;
                    if (dtPrintSet.Rows.Count > 0)
                    {
                        //MessageBox.Show("请先删除其对应的打印标签设置！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0032}"), 
                            StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0033}"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DataTable dtDecay = ((DataView)gvDecay.DataSource).Table;
                    DataRow[] drPowerControls = dtDecay.Select(string.Format(BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY + "='{0}'", decaykey));
                    DataTable dtDel = dtDecay.Clone();
                    drPowerControls[0][BASE_TESTRULE_DECAY.FIELDS_ISFLAG] = 0;
                    dtDel.Rows.Add(drPowerControls[0].ItemArray);

                    bool bck = DeleteGridViewCommonData(dtDel, BASE_TESTRULE_DECAY.DATABASE_TABLE_FORUPDATE);
                    if (bck)
                    {
                        dtDecay.Rows.Remove(drPowerControls[0]);
                        this.gcDecay.MainView = gvDecay;
                        this.gcDecay.DataSource = null;
                        this.gvDecay.FocusedRowHandle = -1;
                        this.gcDecay.DataSource = dtDecay;
                    }

                    InitPrintSetData();
                }              
            }
        }

        private void btnAdd_PrintSet_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPrintSet = ((DataView)gvPrintSet.DataSource).Table;
                DataRow drNew = dtPrintSet.NewRow();
                drNew[BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[BASE_TESTRULE_PRINTSET.FIELDS_TESTRULE_KEY] = testRule_key;
                drNew[BASE_TESTRULE_PRINTSET.FIELDS_DECAY_KEY] = decaykey;
                drNew[BASE_TESTRULE_PRINTSET.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                drNew[BASE_TESTRULE_PRINTSET.FIELDS_ISPACKAGEPRINT] = 0;
                dtPrintSet.Rows.Add(drNew);
                this.gcPrintSet.DataSource = dtPrintSet;
            }
            catch //(Exception ex)
            { }
        }

        private void btnDel_PrintSet_Click(object sender, EventArgs e)
        {
            if (gvPrintSet.FocusedRowHandle > -1 && !string.IsNullOrEmpty(printsetkey))
            {
                //if (MessageService.AskQuestion("确定要删除选定的数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRuleForm.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    printsetkey = Convert.ToString(gvPrintSet.GetRowCellValue(gvPrintSet.FocusedRowHandle, BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY));
                    DataTable dtPrintSet = ((DataView)gvPrintSet.DataSource).Table;
                    DataRow[] drPrintSet = dtPrintSet.Select(string.Format(BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY + "='{0}'", printsetkey));
                    if (drPrintSet.Length > 0)
                    {
                        DataTable dtDel = dtPrintSet.Clone();
                        drPrintSet[0][BASE_TESTRULE_PRINTSET.FIELDS_ISFLAG] = 0;
                        dtDel.Rows.Add(drPrintSet[0].ItemArray);

                        bool bck = DeleteGridViewCommonData(dtDel, BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORUPDATE);
                        if (bck)
                        {
                            dtPrintSet.Rows.Remove(drPrintSet[0]);
                            this.gvPrintSet.FocusedRowHandle = -1;
                            this.gcPrintSet.DataSource = gvPrintSet;
                        }
                    }
                }
            }
        }

        private void gvPrintSet_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //主标签只能有一个
            if (e.Column.Name == BASE_TESTRULE_PRINTSET.FIELDS_ISMAIN)
            {
                if (Convert.ToBoolean(e.Value))
                {
                    for (int i = 0; i < gvPrintSet.RowCount; i++)
                    {
                        if (e.RowHandle != i)
                            gvPrintSet.SetRowCellValue(i, BASE_TESTRULE_PRINTSET.FIELDS_ISMAIN, false);
                    }
                }
            }
            //包装打印设置只能有一个
            if (e.Column.Name == BASE_TESTRULE_PRINTSET.FIELDS_ISPACKAGEPRINT)
            {
                if (Convert.ToBoolean(e.Value))
                {
                    for (int i = 0; i < gvPrintSet.RowCount; i++)
                    {
                        if (e.RowHandle != i)
                            gvPrintSet.SetRowCellValue(i, BASE_TESTRULE_PRINTSET.FIELDS_ISPACKAGEPRINT, false);
                    }
                }
            }
        }

        private void gvPrintSet_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
            {
                printsetkey = Convert.ToString(gvPrintSet.GetRowCellValue(e.FocusedRowHandle, BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY));
            }
        }     
    }
}