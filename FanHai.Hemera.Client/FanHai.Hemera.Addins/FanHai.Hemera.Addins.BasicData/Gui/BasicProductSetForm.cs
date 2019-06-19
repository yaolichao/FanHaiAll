using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Dialogs;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicProductSetForm : BaseDialog
    {        
        public DataRow drCommon = null;
        public DataTable dtDtl = null;
        public bool isEdit = false;
        BaseTestRuleEntity _testRuleEntity = new BaseTestRuleEntity();
        PorProductEntity _porProductEntity = new PorProductEntity();
        public BasicProductSetForm()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.Name = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0001}");//产品属性设置
            xtraTabPage1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0002}");//基本属性设置
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0003}");//产品代码
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0004}");//名称
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0005}");//最大功率
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0006}");//最小功率
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0007}");//测试规则
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0008}");//生产编码规则
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0009}");//客户编码规则
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0010}");//产品型号
            chkLABELCHECK.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0011}");//检验铭牌
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0012}");//认证类型
            layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0013}");//铭牌版本
            layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0014}");//满柜数量
            layoutControlItem15.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0015}");//认证
            layoutControlItem14.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0016}");//PS/PG备注
            layoutControlItem16.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0017}");//分档方式
            chkForExperiment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0018}");//实验-产品ID号
            chk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0019}");//电池片信息
            layoutControlItem17.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0020}");//接线盒批号
            layoutControlItem18.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0021}");//校准版类型
            layoutControlItem20.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0022}");//校准周期(分钟)
            layoutControlItem19.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0023}");//固化周期(分钟)
            lciConstantTemperatureCycle.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0024}");//恒温周期
            layoutControlItem26.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0025}");//组件尺寸
            layoutControlItem27.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0026}");//铭牌打印模板
            lblEnterName.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0027}");//工艺流程组
            lblRoutName.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0028}");//工艺流程
            lblStepName.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0029}");//工序
            btnSave.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.btn.0001}");//保存
            btnCancel.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.btn.0002}");//取消

            xtraTabPage2.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.lbl.0030}");//等级设定
            btnAdd.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.btn.0003}");//新增
            btnDel.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.btn.0004}");//删除
            PRODUCT_GRADE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.GridControl.0001}");//等级
            SAP_PN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.GridControl.0002}");//SAP料号
            ISMAIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.GridControl.0003}");//主等级
           
        }
   
        private void BasicProductSetForm_Load(object sender, EventArgs e)
        {
            if (isEdit)
            {
                txtPRODUCT_CODE.Properties.ReadOnly = true;
                InitBindRoute();
            }
            else
            {
                InitControlValue();
            }

            InitBindLUE();
            InitData();


            this.teRouteEnterprise.Enabled = false;
            this.teRouteName.Enabled = false;
        }
        private void initCellSize()
        {

        }

        /// <summary>
        /// 默认工单工序绑定
        /// </summary>
        private void InitControlValue()
        {
            RouteQueryEntity routeQueryEntity = new RouteQueryEntity();
            DataSet dsRouteFirstOperation = routeQueryEntity.GetProcessPlanFirstOperation("", string.Empty, false);
            //有获取到首工序工艺流程。
            if (string.IsNullOrEmpty(routeQueryEntity.ErrorMsg)
                && null != dsRouteFirstOperation
                && dsRouteFirstOperation.Tables.Count > 0
                && dsRouteFirstOperation.Tables[0].Rows.Count > 0)
            {
                DataRow drRouteFirstOperation = dsRouteFirstOperation.Tables[0].Rows[0];
                this.teRouteEnterprise.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                this.teRouteEnterprise.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                this.teRouteName.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                this.teRouteName.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                this.beStepName.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                this.beStepName.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            }
            else
            {
                this.teRouteEnterprise.Tag = string.Empty;
                this.teRouteEnterprise.Text = string.Empty;
                this.teRouteName.Tag = string.Empty;
                this.teRouteName.Text = string.Empty;
                this.beStepName.Tag = string.Empty;
                this.beStepName.Text = string.Empty;
            }
        }

        /// <summary>
        /// 绑定工序信息
        /// </summary>
        private void InitBindRoute()
        {
            this.teRouteEnterprise.Tag = drCommon["ROUTE_ENTERPRISE_VER_KEY"];
            this.teRouteEnterprise.Text = drCommon["ENTERPRISE_NAME"].ToString();
            this.teRouteName.Tag = drCommon["ROUTE_ROUTE_VER_KEY"];
            this.teRouteName.Text = drCommon["ROUTE_NAME"].ToString();
            this.beStepName.Tag = drCommon["ROUTE_STEP_KEY"];
            this.beStepName.Text = drCommon["ROUTE_STEP_NAME"].ToString();
        }

        private void InitBindLUE()
        {
            Hashtable hsTable = new Hashtable();
            DataSet dsTestRule = _testRuleEntity.GetTestRuleMainData(hsTable);
            DataTable dtTestRule = dsTestRule.Tables[BASE_TESTRULE.DATABASE_TABLE_NAME];
            luePRO_TEST_RULE.Properties.DisplayMember = BASE_TESTRULE.FIELDS_TESTRULE_CODE;
            luePRO_TEST_RULE.Properties.ValueMember = BASE_TESTRULE.FIELDS_TESTRULE_CODE;
            luePRO_TEST_RULE.Properties.DataSource = dtTestRule;
            luePRO_TEST_RULE.ItemIndex = -1;

            //DataTable dtCommon = new BasePowerSetEntity().GetBasicPowerSetEngine_CommonData("").Tables[BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME];

            //绑定自定义配置数据
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtCommon = BaseData.Get(l_s, category);

            //DataRow[] drCommons = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCTLABELTYPE));
            //DataTable dtLabelType = dtCommon.Clone();
            //foreach (DataRow dr in drCommons)
            //    dtLabelType.ImportRow(dr);
            //dtLabelType.AcceptChanges();
            //lueLABELTYPE.Properties.DisplayMember = "Column_Name";
            //lueLABELTYPE.Properties.ValueMember = "Column_code";
            //lueLABELTYPE.Properties.DataSource = dtLabelType;
            //lueLABELTYPE.ItemIndex = -1;

            //DataSet dsLabelType = new ProductModelEntity().GetCertification();
            //if (dsLabelType != null && dsLabelType.Tables.Count > 0)
            //{
            //    lueLABELTYPE.Properties.DisplayMember = "CERTIFICATION_TYPE";
            //    lueLABELTYPE.Properties.ValueMember = "CERTIFICATION_KEY";
            //    lueLABELTYPE.Properties.DataSource = dsLabelType.Tables[0];
            //    lueLABELTYPE.ItemIndex = -1;
            //}

            DataTable dtProModel = new ProductModelEntity().GetProductModelAndCP().Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_NAME];
            luePROMODEL_NAME.Properties.DisplayMember = BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME;
            luePROMODEL_NAME.Properties.ValueMember = BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME;
            luePROMODEL_NAME.Properties.DataSource = dtProModel;
            luePROMODEL_NAME.ItemIndex = -1;

            DataTable dtLevel = dtCommon.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCTGRADE));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";
            repositoryItemLookUpEdit_Grade.ValueMember = "Column_code";
            repositoryItemLookUpEdit_Grade.DisplayMember = "Column_Name";
            repositoryItemLookUpEdit_Grade.DataSource = dview.Table;

            DataTable dtMemo = dtCommon.Clone();
            dtMemo.TableName = "Memo1";
            DataRow[] drsMemo = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.MEMO_TYPE));
            foreach (DataRow dr in drsMemo)
                dtMemo.ImportRow(dr);
            DataView dviewMemo = dtMemo.DefaultView;
            dviewMemo.Sort = "Column_Index asc";
            luePsPg.Properties.ValueMember = "Column_code";
            luePsPg.Properties.DisplayMember = "Column_Name";
            luePsPg.Properties.DataSource = dviewMemo.Table;
            luePsPg.ItemIndex = -1;

            string[] ls = new string[] { "SIZE" };
            string categorySize = "NameplateLabelAutoprint_QTX";
            DataTable dtSize = BaseData.Get(ls, categorySize);

            foreach (DataRow row in dtSize.Rows)
            {
                if (!cmbSize.Properties.Items.Contains(row["SIZE"]))
                {
                    cmbSize.Properties.Items.Add(row["SIZE"]);
                }
            }

            string[] ls1 = new string[] { "TEMPLATE","TEMPLATE_DESC" };
            string categoryTemplate = "Name_Templates";
            DataTable dtTemplate = BaseData.Get(ls1, categoryTemplate);

            luTemplate.Properties.DataSource = dtTemplate;
            luTemplate.Properties.ValueMember = "TEMPLATE_DESC";
            luTemplate.Properties.DisplayMember = "TEMPLATE";

        }

        private void InitData()
        {

            this.txtCALIBRATION_CYCLE.Text = drCommon[POR_PRODUCT.FIELDS_CALIBRATION_CYCLE].ToString();
            this.txtCALIBRATION_TYPE.Text = drCommon[POR_PRODUCT.FIELDS_CALIBRATION_TYPE].ToString();
            this.txtCERTIFICATION.Text = drCommon[POR_PRODUCT.FIELDS_CERTIFICATION].ToString();
            this.txtCODEMARK.Text = drCommon[POR_PRODUCT.FIELDS_CODEMARK].ToString();
            this.txtCUSTMARK.Text = drCommon[POR_PRODUCT.FIELDS_CUSTMARK].ToString();
            this.txtFIX_CYCLE.Text = drCommon[POR_PRODUCT.FIELDS_FIX_CYCLE].ToString();
            this.txtConstantTemperatureCycle.Text = drCommon[POR_PRODUCT.FIELDS_CONSTANT_TEMPERTATURE_CYCLE].ToString();
            this.txtJUNCTION_BOX.Text = drCommon[POR_PRODUCT.FIELDS_JUNCTION_BOX].ToString();
            this.txtLABELVAR.Text = drCommon[POR_PRODUCT.FIELDS_LABELVAR].ToString();
            this.txtMAXPOWER.Text = drCommon[POR_PRODUCT.FIELDS_MAXPOWER].ToString();
            this.txtMINPOWER.Text = drCommon[POR_PRODUCT.FIELDS_MINPOWER].ToString();
            //this.luePRO_LEVEL.Text = drCommon[POR_PRODUCT.FIELDS_PRO_LEVEL].ToString();
            this.txtPRODUCT_CODE.Text = drCommon[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString();
            this.txtPRODUCT_NAME.Text = drCommon[POR_PRODUCT.FIELDS_PRODUCT_NAME].ToString();
            //this.txtSAP_PN.Text = drCommon[POR_PRODUCT.FIELDS_SAP_PN].ToString();
            this.txtSHIP_QTY.Text = drCommon[POR_PRODUCT.FIELDS_SHIP_QTY].ToString();
            this.txtTOLERANCE.Text = drCommon[POR_PRODUCT.FIELDS_TOLERANCE].ToString();

            if (drCommon.Table.Columns.Contains(POR_PRODUCT.FIELDS_CERTIFICATION_KEY) && drCommon[POR_PRODUCT.FIELDS_CERTIFICATION_KEY] != DBNull.Value)
                this.lueLABELTYPE.EditValue = drCommon[POR_PRODUCT.FIELDS_CERTIFICATION_KEY].ToString();
            else
                this.lueLABELTYPE.Text = drCommon[POR_PRODUCT.FIELDS_LABELTYPE].ToString();


            this.luePRO_TEST_RULE.EditValue = drCommon[POR_PRODUCT.FIELDS_PRO_TEST_RULE].ToString();
            this.luePROMODEL_NAME.EditValue = drCommon[POR_PRODUCT.FIELDS_PROMODEL_NAME].ToString();
            this.luePsPg.EditValue = drCommon[POR_PRODUCT.FIELDS_MEMO1].ToString();
            this.cmbSize.Text = drCommon[POR_PRODUCT.FIELDS_CELL_SIZE].ToString();
            this.luTemplate.Text = drCommon[POR_PRODUCT.FIELDS_NAME_TEMPLATE].ToString();

            if (drCommon[POR_PRODUCT.FIELDS_LABELCHECK] != null && drCommon[POR_PRODUCT.FIELDS_LABELCHECK].ToString() == "1")
                this.chkLABELCHECK.Checked = true;
            else
                this.chkLABELCHECK.Checked = false;

            if (drCommon[POR_PRODUCT.FIELDS_ISKingLine] != null && drCommon[POR_PRODUCT.FIELDS_ISKingLine].ToString() == "1")
            {

                this.cbchk.SelectedText = "金刚线";
            }
            else if (drCommon[POR_PRODUCT.FIELDS_ISKingLine] != null && drCommon[POR_PRODUCT.FIELDS_ISKingLine].ToString() == "2")
            {
                this.cbchk.SelectedText = "黑硅片";
            }
          
            if (drCommon[POR_PRODUCT.FIELDS_ISEXPERIMENT] != null && drCommon[POR_PRODUCT.FIELDS_ISEXPERIMENT].ToString() == "1")
                this.chkForExperiment.Checked = true;
            else
                this.chkForExperiment.Checked = false;

            if (dtDtl != null)
            {
                dtDtl.AcceptChanges();
                gcProductDtl.DataSource = dtDtl;
            }
        }

        private bool IsValidData()
        {
            if (string.IsNullOrEmpty(txtPRODUCT_CODE.Text.Trim()))
            {
                //MessageService.ShowMessage("【产品代码】不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtPRODUCT_CODE.Focus();
                return false;
            }
            //判断产品代码是否存在
            if (!isEdit)
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add(POR_PRODUCT.FIELDS_PRODUCT_CODE, txtPRODUCT_CODE.Text.Trim());
                DataSet dsIsExistProductCode = _porProductEntity.GetPorProductData(hsTable);
                if (!string.IsNullOrEmpty(_porProductEntity.ErrorMsg))
                {
                    MessageService.ShowError(_porProductEntity.ErrorMsg);
                    this.txtPRODUCT_CODE.Focus();
                    this.txtPRODUCT_CODE.SelectAll();
                    return false;
                }
                DataTable dtIsExistProductCode = dsIsExistProductCode.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
                if (dtIsExistProductCode.Rows.Count > 0)
                {
                    //MessageService.ShowError(string.Format(@"产品ID号【{0}】已经存在，不能重复!", txtPRODUCT_CODE.Text.Trim()));
                    MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0002}") 
                        + txtPRODUCT_CODE.Text.Trim()
                        + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0003}"));
                    this.txtPRODUCT_CODE.Focus();
                    this.txtPRODUCT_CODE.SelectAll();
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtPRODUCT_NAME.Text.Trim()))
            {
                //MessageService.ShowMessage("【产品名称】不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtPRODUCT_NAME.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMINPOWER.Text.Trim()))
            {
                //MessageService.ShowMessage("【最小功率】不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtPRODUCT_NAME.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMAXPOWER.Text.Trim()))
            {
                //MessageService.ShowMessage("【最大功率】不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtMAXPOWER.Focus();
                return false;
            }        
            if (luePRO_TEST_RULE.EditValue==null)
            {
                //MessageService.ShowMessage("请选择【测试规则】!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (luePROMODEL_NAME.EditValue == null)
            {
                //MessageService.ShowMessage("请选择【产品型号】!");       
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            //if (lueLABELTYPE.EditValue == null)
            //{
            //    //MessageService.ShowMessage("请选择【认证类型】!");   
            //    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
            //    return false;
            //}
            if (string.IsNullOrEmpty(txtSHIP_QTY.Text.Trim()))
            {
                //MessageService.ShowMessage("请设定【满柜数量】!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtSHIP_QTY.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCALIBRATION_TYPE.Text.Trim()))
            {
                //MessageService.ShowMessage("请设定【校准版类型】!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0011}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtCALIBRATION_TYPE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCALIBRATION_CYCLE.Text.Trim()))
            {
                //MessageService.ShowMessage("请设定【校准版周期】!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0012}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtCALIBRATION_CYCLE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtFIX_CYCLE.Text.Trim()))
            {
                //MessageService.ShowMessage("请设定【固化周期】!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0013}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtFIX_CYCLE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtConstantTemperatureCycle.Text.Trim()))
            {
                //MessageService.ShowMessage("请设定【恒温周期】!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0014}"), StringParser.Parse("${res:Global.SystemInfo}"));
                txtConstantTemperatureCycle.Focus();
                return false;
            }
            bool bl = true;
            int countMainGrade = 0;
            //if (gvProductDtl.RowCount < 1)
            //{
            //    MessageService.ShowMessage("【等级设定】不能为空!", "提示");
            //    return false;
            //}
            //else
            //{
              
                for (int i = 0; i < gvProductDtl.RowCount; i++)
                {
                    string grade = gvProductDtl.GetRowCellValue(i, POR_PRODUCT_DTL.FIELDS_PRODUCT_GRADE).ToString();
                    if (string.IsNullOrEmpty(grade))
                    {
                        bl = false;
                        //MessageService.ShowMessage("表格中【产品等级】不能为空!", "提示");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0015}"), StringParser.Parse("${res:Global.SystemInfo}"));
                        break;
                    }

                    string maingrade = gvProductDtl.GetRowCellValue(i, POR_PRODUCT_DTL.FIELDS_ISMAIN).ToString();
                    if (bool.Parse(maingrade))
                    {
                        countMainGrade++;
                    }
                }
                if (countMainGrade > 1)
                {
                    //MessageService.ShowMessage("表格中【主等级】只能有一笔!", "提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0016}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return false;
                }
                if (bl)
                {
                    DataTable dtCompare = gcProductDtl.DataSource as DataTable;
                    foreach (DataRow dr in dtCompare.Rows)
                    {
                        string level = dr[POR_PRODUCT_DTL.FIELDS_PRODUCT_GRADE].ToString();
                        DataRow[] drs = dtCompare.Select(string.Format(POR_PRODUCT_DTL.FIELDS_PRODUCT_GRADE + "='{0}'", level));
                        if (drs != null && drs.Length > 1)
                        {
                            bl = false;
                            //MessageService.ShowMessage("表格中【产品等级】不能重复，请确认!", "提示");
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0017}"), StringParser.Parse("${res:Global.SystemInfo}"));
                            break;
                        }
                    }
                }
               
            //}
            return bl;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidData()) return;
            DataSet dsSave = new DataSet();

            drCommon[POR_PRODUCT.FIELDS_CALIBRATION_CYCLE]= this.txtCALIBRATION_CYCLE.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_CALIBRATION_TYPE] = this.txtCALIBRATION_TYPE.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_CERTIFICATION] = this.txtCERTIFICATION.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_CODEMARK] = this.txtCODEMARK.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_CUSTMARK] = this.txtCUSTMARK.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_FIX_CYCLE] = this.txtFIX_CYCLE.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_CONSTANT_TEMPERTATURE_CYCLE] = this.txtConstantTemperatureCycle.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_JUNCTION_BOX]=this.txtJUNCTION_BOX.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_LABELVAR] = this.txtLABELVAR.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_MAXPOWER] = this.txtMAXPOWER.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_MINPOWER] = this.txtMINPOWER.Text.Trim();
            //drCommon[POR_PRODUCT.FIELDS_PRO_LEVEL] = this.luePRO_LEVEL.EditValue.ToString();
            drCommon[POR_PRODUCT.FIELDS_PRODUCT_CODE] = this.txtPRODUCT_CODE.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_PRODUCT_NAME] = this.txtPRODUCT_NAME.Text.Trim();
            //drCommon[POR_PRODUCT.FIELDS_SAP_PN] = this.txtSAP_PN.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_SHIP_QTY] = this.txtSHIP_QTY.Text.Trim();
            drCommon[POR_PRODUCT.FIELDS_TOLERANCE] = this.txtTOLERANCE.Text.Trim();

            drCommon[POR_PRODUCT.FIELDS_MEMO1] = this.luePsPg.EditValue == null ? "" : this.luePsPg.EditValue.ToString();

            drCommon[POR_PRODUCT.FIELDS_LABELTYPE] = this.lueLABELTYPE.Text;
            drCommon[POR_PRODUCT.FIELDS_CERTIFICATION_KEY] = Convert.ToString(this.lueLABELTYPE.EditValue);

            drCommon[POR_PRODUCT.FIELDS_PRO_TEST_RULE] = this.luePRO_TEST_RULE.EditValue == null ? "" : this.luePRO_TEST_RULE.EditValue.ToString();
            drCommon[POR_PRODUCT.FIELDS_PROMODEL_NAME] = this.luePROMODEL_NAME.EditValue == null ? "" : this.luePROMODEL_NAME.EditValue.ToString();
            drCommon[POR_PRODUCT.FIELDS_LABELCHECK] = this.chkLABELCHECK.Checked == false ? "0" : "1";
            drCommon[POR_PRODUCT.FIELDS_ISEXPERIMENT] = this.chkForExperiment.Checked == false ? "0" : "1";

            drCommon[POR_PRODUCT.FIELDS_ROUTE_ENTERPRISE_VER_KEY] = this.teRouteEnterprise.Tag;
            drCommon[POR_PRODUCT.FIELDS_ENTERPRISE_NAME] = this.teRouteEnterprise.Text;
            drCommon[POR_PRODUCT.FIELDS_ROUTE_ROUTE_VER_KEY] = this.teRouteName.Tag;
            drCommon[POR_PRODUCT.FIELDS_ROUTE_NAME] = this.teRouteName.Text;
            drCommon[POR_PRODUCT.FIELDS_ROUTE_STEP_KEY] = this.beStepName.Tag;
            drCommon[POR_PRODUCT.FIELDS_ROUTE_STEP_NAME] = this.beStepName.Text;
            drCommon[POR_PRODUCT.FIELDS_CELL_SIZE] = this.cmbSize.Text;
            drCommon[POR_PRODUCT.FIELDS_NAME_TEMPLATE] = this.luTemplate.Text;
            if (this.cbchk.Text == "金刚线")
            {
                drCommon[POR_PRODUCT.FIELDS_ISKingLine] = "1";
            }
            else if (this.cbchk.Text == "黑硅片")
            {
                drCommon[POR_PRODUCT.FIELDS_ISKingLine] = "2";
            }
            else
            {
                drCommon[POR_PRODUCT.FIELDS_ISKingLine] = "0";
            }
            if (isEdit)
                drCommon[POR_PRODUCT.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            else
                drCommon[POR_PRODUCT.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            //drCommon[POR_PRODUCT.FIELDS_EDIT_TIME] = string.Empty;
            DataTable dtSave = drCommon.Table.Clone();
            dtSave.Rows.Add(drCommon.ItemArray);
            if (isEdit)
                dtSave.TableName = POR_PRODUCT.DATABASE_TABLE_FORUPDATE;
            else
                dtSave.TableName = POR_PRODUCT.DATABASE_TABLE_FORINSERT;
            dsSave.Merge(dtSave, true, MissingSchemaAction.Add);

            DataTable dtProductDtl = ((DataView)gvProductDtl.DataSource).Table;
            DataTable dtProductDtl_Update = dtProductDtl.GetChanges(DataRowState.Modified);
            DataTable dtProductDtl_Insert = dtProductDtl.GetChanges(DataRowState.Added);

            if (dtProductDtl_Update != null && dtProductDtl_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtProductDtl_Update.Clone();
                foreach (DataRow dr in dtProductDtl_Update.Rows)
                {
                    DataRow[] drUpdates = dtProductDtl.Select(string.Format(POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY + "='{0}'", dr[POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[POR_PRODUCT_DTL.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = POR_PRODUCT_DTL.DATABASE_TABLE_FORUPDATE;
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (dtProductDtl_Insert != null && dtProductDtl_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtProductDtl_Insert.Clone();
                foreach (DataRow dr in dtProductDtl_Insert.Rows)
                {
                    DataRow[] drUpdates = dtProductDtl.Select(string.Format(POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY + "='{0}'", dr[POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[POR_PRODUCT_DTL.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }               

                dtInsert.TableName = POR_PRODUCT_DTL.DATABASE_TABLE_FORINSERT;
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            

            bool bl_bak = _porProductEntity.SavePorProductData(dsSave);
            if (!bl_bak)
            {
                MessageService.ShowMessage(_porProductEntity.ErrorMsg);
            }
            else
            {
                //MessageService.ShowMessage("保存成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0018}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtDtl = this.gcProductDtl.DataSource as DataTable;
            DataRow drNew = dtDtl.NewRow();
            string seq = dtDtl.Compute("max(ORDER_INDEX)", "").ToString();
            drNew[POR_PRODUCT_DTL.FIELDS_ORDER_INDEX] = string.IsNullOrEmpty(seq) == true ? 1 : Convert.ToInt16(seq) + 1;
            drNew[POR_PRODUCT_DTL.FIELDS_PRODUCT_KEY] = drCommon[POR_PRODUCT.FIELDS_PRODUCT_KEY];
            drNew[POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);                
            drNew[POR_PRODUCT.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drNew[POR_PRODUCT_DTL.FIELDS_ISMAIN] = "false";
            dtDtl.Rows.Add(drNew);
            gcProductDtl.DataSource = dtDtl;

            //DataView dv = dtDtl.DefaultView;
            //dv.Sort = POR_PRODUCT_DTL.FIELDS_ORDER_INDEX + " asc";
            //gcProductDtl.DataSource = dv.Table;           
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DeleteGvdtl();
        }

        private void DeleteGvdtl()
        {
            if (gvProductDtl.FocusedRowHandle < 0) return;
            string level = string.Empty;
            DataSet dsSave = new DataSet();
            DataTable dtDel = gcProductDtl.DataSource as DataTable;
            DataTable dtDel01 = dtDel.Clone();
            dtDel01.TableName = POR_PRODUCT_DTL.DATABASE_TABLE_FORUPDATE;
            DataRow dr = gvProductDtl.GetFocusedDataRow();
            if (dr.RowState == DataRowState.Added)
            {
                dtDel.Rows.Remove(dr);
                gcProductDtl.DataSource = null;
                gcProductDtl.DataSource = dtDel;
                return;
            }

            dr[POR_PRODUCT_DTL.FIELDS_ISFLAG] = "0";
            level = dr[POR_PRODUCT_DTL.FIELDS_PRODUCT_GRADE].ToString();
            dtDel01.Rows.Add(dr.ItemArray);
            dsSave.Merge(dtDel01, true, MissingSchemaAction.Add);
           
            bool bl_bak = _porProductEntity.SavePorProductData(dsSave);
            if (!bl_bak)
            {
                MessageService.ShowMessage(_porProductEntity.ErrorMsg);
            }
            else
            {
                //MessageService.ShowMessage(string.Format("等级【{0}】已删除", level), "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0019}") 
                    + level
                    + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSetForm.msg.0020}"), StringParser.Parse("${res:Global.SystemInfo}"));
                dtDel.Rows.Remove(dr);
                gcProductDtl.DataSource = null;
                gcProductDtl.DataSource = dtDel;
            }
        }

        private void beStepName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            OperationHelpDialog dlg = new OperationHelpDialog();
            dlg.FactoryRoom = string.Empty;
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = teRouteEnterprise;
            dlg.RouteName = teRouteName;
            dlg.StepName = beStepName;
            dlg.dtWorkOrderRoute = null;
            dlg.IsRework = false;
            Point i = beStepName.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + beStepName.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + beStepName.Width - dlg.Width, i.Y + beStepName.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + beStepName.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
       
    }
}