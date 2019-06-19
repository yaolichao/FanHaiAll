using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicProductSet : BaseUserCtrl
    {
        PorProductEntity _porProductEntity = new PorProductEntity();
        DataTable dtPorProduct_dtl = null;
        /// <summary>
        /// 定位修改的行数据
        /// </summary>
        string _loadKey = string.Empty;
        public BasicProductSet()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工艺参数设置 > 组件设置";
            GridViewHelper.SetGridView(gvPorProduct);
            GridViewHelper.SetGridView(gvProductDtl);

            this.btnAdd.Text = StringParser.Parse("${res:Global.New}");//新增
            this.btnModify.Text = StringParser.Parse("${res:Global.Update}");//修改
            this.btnDel.Text = StringParser.Parse("${res:Global.Delete}");//删除

            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.lbl.0002}");//产品代码
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.lbl.0003}");//测试规则
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.lbl.0004}");//创建日期
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.btn.0001}");//查询

            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.lbl.0005}");//产品设置
            PRODUCT_CODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0001}");//产品代码
            PRODUCT_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0002}");//产品说明
            QUANTITY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0003}");//数量
            MAXPOWER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0004}");//最大功率
            MINPOWER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0005}");//最小功率
            PRO_TEST_RULE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0006}");//测试规则
            TESTRULE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0007}");//测试规则名称
            CODEMARK.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0008}");//生产规则编码
            CUSTMARK.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0009}");//客户规则编码
            CREATE_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0010}");//创建日期
            PROMODEL_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0011}");//产品类型
            LABELTYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0012}");//认证类型
            CERTIFICATION.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0013}");//认证
            LABELCHECK.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0014}");//检验铭牌
            SHIP_QTY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0015}");//满柜数量
            TOLERANCE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0016}");//分档方式
            JUNCTION_BOX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0017}");//接线盒
            CALIBRATION_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0018}");//校准版类型
            CALIBRATION_CYCLE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0019}");//校准周期
            FIX_CYCLE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0020}");//固化周期
            CONSTANT_TEMPERTATURE_CYCLE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0021}");//恒温周期
            ISEXPERIMENT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0022}");//实验工单ID
            ENTERPRISE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0023}");//工艺流程组名称
            ROUTE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0024}");//工艺流程名称
            ROUTE_STEP_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0025}");//工序名称
            gcKingLine.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0026}");//金刚线
            gridColumn1.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0027}");//组件尺寸
            NameTemplate.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0028}");//铭牌打印模板
            
            PRODUCT_GRADE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0029}");//等级
            gridColumn4.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0030}");//Sap料号
            ISMAIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.GridControl.0031}");//优等级
            //
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitBindData();
        }

        private void BasicProductSet_Load(object sender, EventArgs e)
        {           
            BindProLevel();
            InitBindData();
        }

        private void BindProLevel()
        {
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtCommon = BaseData.Get(l_s, category);

            DataTable dtLevel = dtCommon.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCTGRADE));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";

            repositoryItemLookUpEdit1_PRODUCT_GRADE.DisplayMember = "Column_Name";
            repositoryItemLookUpEdit1_PRODUCT_GRADE.ValueMember = "Column_code";
            repositoryItemLookUpEdit1_PRODUCT_GRADE.DataSource = dview.Table;

            repositoryItemLookUpEdit_TESTRULE_NAME.DisplayMember = BASE_TESTRULE.FIELDS_TESTRULE_NAME;
            repositoryItemLookUpEdit_TESTRULE_NAME.ValueMember = BASE_TESTRULE.FIELDS_TESTRULE_CODE;
            repositoryItemLookUpEdit_TESTRULE_NAME.DataSource = new BaseTestRuleEntity().GetTestRuleMainData(new Hashtable()).Tables[BASE_TESTRULE.DATABASE_TABLE_NAME];     
        }

        private void InitBindData()
        {
            gvPorProduct.FocusedRowHandle = -1;

            Hashtable hstable = new Hashtable();
            if (!string.IsNullOrEmpty(txtPRODUCT_CODE.Text.Trim()))
                hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE] = txtPRODUCT_CODE.Text.Trim();
            if (!string.IsNullOrEmpty(txtPRO_TEST_RULE.Text.Trim()))
                hstable[POR_PRODUCT.FIELDS_PRO_TEST_RULE] = txtPRO_TEST_RULE.Text.Trim();
            if (!string.IsNullOrEmpty(dateStart.Text.Trim()))
                hstable[POR_PRODUCT.FIELDS_CREATE_TIME_START] = dateStart.Text.Trim();
            if (!string.IsNullOrEmpty(dateEnd.Text.Trim()))
                hstable[POR_PRODUCT.FIELDS_CREATE_TIME_END] = dateEnd.Text.Trim();

            DataSet dsReturn = _porProductEntity.GetPorProductData(hstable);
            if (string.IsNullOrEmpty(_porProductEntity.ErrorMsg))
            {
                dtPorProduct_dtl = dsReturn.Tables[POR_PRODUCT_DTL.DATABASE_TABLE_NAME];
                dtPorProduct_dtl.AcceptChanges();
                this.gcProductDtl.DataSource = null;

                DataTable dtPorProduct = dsReturn.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
                this.gcPorProduct.DataSource = dtPorProduct;
                this.gvPorProduct.BestFitColumns();

                if (!string.IsNullOrEmpty(_loadKey))
                {
                    for (int i = 0; i < gvPorProduct.RowCount; i++)
                    {
                        string sk = Convert.ToString(((DataRowView)(this.gvPorProduct.GetRow(i))).Row[POR_PRODUCT.FIELDS_PRODUCT_KEY]);
                        if (_loadKey.Equals(sk.Trim()))
                        {
                            this.gvPorProduct.FocusedRowHandle = i;
                            break;
                        }
                    }
                }

                BindDtlData();
            }
            else
                MessageService.ShowError(_porProductEntity.ErrorMsg);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvPorProduct.FocusedRowHandle < 0 || gvPorProduct.RowCount < 1)
            {
                //MessageService.ShowMessage("请选择编辑的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            DataTable dtDtlEdit = ((DataView)gvProductDtl.DataSource).Table;
            DataRow drEdit = gvPorProduct.GetFocusedDataRow();
            this._loadKey = Convert.ToString(drEdit[POR_PRODUCT.FIELDS_PRODUCT_KEY]);
            BasicProductSetForm bpsf = new BasicProductSetForm();
            bpsf.isEdit = true;
            bpsf.drCommon = drEdit;
            bpsf.dtDtl = dtDtlEdit;
            if (DialogResult.OK == bpsf.ShowDialog())
            {
                InitBindData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtDtlNew = ((DataView)gvProductDtl.DataSource).Table;

            DataTable dtNew = ((DataView)gvPorProduct.DataSource).Table;
            DataRow drNew = dtNew.NewRow();
            this._loadKey = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            drNew[POR_PRODUCT.FIELDS_PRODUCT_KEY] = this._loadKey;
            drNew[POR_PRODUCT.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            BasicProductSetForm bpsf = new BasicProductSetForm();
            bpsf.isEdit = false;
            bpsf.drCommon = drNew;
            bpsf.dtDtl = dtDtlNew.Clone();

            if (DialogResult.OK == bpsf.ShowDialog())
            {
                InitBindData();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gvPorProduct.FocusedRowHandle < 0 || gvPorProduct.RowCount < 1)
            {
                //MessageService.ShowMessage("请选择删除的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            DataSet dsDel = new DataSet();
            DataRow drDel = gvPorProduct.GetFocusedDataRow();
            DataTable dtDel = drDel.Table.Clone();
            drDel[POR_PRODUCT.FIELDS_ISFLAG] = 0;
            dtDel.Rows.Add(drDel.ItemArray);
            dtDel.TableName = POR_PRODUCT.DATABASE_TABLE_FORUPDATE;
            dsDel.Merge(dtDel, true, MissingSchemaAction.Add);

            DataTable dtBind = ((DataView)gvPorProduct.DataSource).Table;
            dtBind.Rows.Remove(drDel);
            dtBind.AcceptChanges();
            gcPorProduct.DataSource = dtBind;

            bool bl_bak = _porProductEntity.SavePorProductData(dsDel);
            if (!bl_bak)
            {
                MessageService.ShowMessage(_porProductEntity.ErrorMsg);
            }
            else
            {
                //MessageService.ShowMessage("删除成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicProductSet.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }

        private void gvPorProduct_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            BindDtlData();
        }
        private void BindDtlData()
        {
            if (gvPorProduct.FocusedRowHandle > -1)
            {
                try
                {
                    gcProductDtl.DataSource = null;
                    //DataTable dtMain=gcPorProduct.DataSource as DataTable;
                    string productkey = gvPorProduct.GetRowCellValue(gvPorProduct.FocusedRowHandle, POR_PRODUCT.FIELDS_PRODUCT_KEY).ToString();
                    DataRow[] drs = dtPorProduct_dtl.Select(string.Format(POR_PRODUCT_DTL.FIELDS_PRODUCT_KEY + "='{0}'", productkey));
                    DataTable dtBind = dtPorProduct_dtl.Clone();
                    foreach (DataRow dr in drs)
                        dtBind.ImportRow(dr);

                    gcProductDtl.DataSource = dtBind;
                }
                catch //(Exception ex)
                { }
            }
        }

        private void gvPorProduct_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvProductDtl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
