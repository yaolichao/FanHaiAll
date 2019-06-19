using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示领料清单的窗体类。
    /// </summary>
    public partial class ReceiveMaterialListCtrl : BaseUserCtrl
    {
        private DataTable paramTable;   //包含查询条件的数据表。
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReceiveMaterialListCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gdvReceiveMaterialList);
        }
        public void InitializeLanguage()
        {
            gcROWNUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0001}");//序号
            gcMatLot.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0002}");//领料项目号
            gcAUFNR.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0003}");//工单号
            gcProID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0004}");//产品ID号
            gcEfficiency.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0005}");//转换效率
            gcGrade.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0006}");//等级
            gcLLIEF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0007}");//原材料供应商
            gcSupplierCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0008}");//供应商编号
            gcERFMG.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0009}");//数量
            gcMATNR.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0010}");//原材料编码
            gcMatDp.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0011}");//原材料描述
            gcOpName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0012}");//工序名称
            gcStoreName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0013}");//线上仓名称
            gcFactory.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0014}");//领料车间
            gcReceiveTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0015}");//领料时间
            gcShiftName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0016}");//班次
            gcOperator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0017}");//领料员工
            gcMemo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.GridControl.0018}");//备注
        }

        /// <summary>
        /// 绑定领料项目列表。
        /// </summary>
        /// <param name="paramTable"></param>
        private void BindReceiveMaterialList(DataTable paramTable)
        {
            ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = receiveMaterial.GetReceiveMaterialHistory(paramTable, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;

            if (string.IsNullOrEmpty(receiveMaterial.ErrorMsg))
            {
                gdcView.MainView = gdvReceiveMaterialList;
                gdcView.DataSource = dsReturn.Tables[0];
                gdvReceiveMaterialList.Invalidate();
                gdvReceiveMaterialList.OptionsView.ColumnAutoWidth = false;
                gdvReceiveMaterialList.BestFitColumns();
            }
            else
            {
                MessageBox.Show(receiveMaterial.ErrorMsg);
            }
        }
        /// <summary>
        /// 查询按钮点击弹出查询条件选择
        /// </summary>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            //ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            ////打开参数选择页面
            //ReceiveMaterialQryDialog receiveMaterialQryDialog = new ReceiveMaterialQryDialog(paramTable);

            paramTable.Rows.Add();
            paramTable.Rows[0]["CHARG"] = txtCHARG.Text.ToString().Trim();
            paramTable.Rows[0]["AUFNR"] = Convert.ToString(this.lueWorkOrderNo.EditValue);
            paramTable.Rows[0]["PRO_ID"] = Convert.ToString(this.lueProId.EditValue);
            paramTable.Rows[0]["EFFICIENCY"] = Convert.ToString(this.lueEfficiency.EditValue);
            paramTable.Rows[0]["GRADE"] = this.teGrade.Text;
            paramTable.Rows[0]["LLIEF"] = Convert.ToString(this.lueSupplierName.EditValue);
            paramTable.Rows[0]["RECEIVE_TIME_END"] = deReceiveMaterialEnd.Text.ToString();
            paramTable.Rows[0]["RECEIVE_TIME_START"] = deReceiveMaterialStart.Text.ToString();
            paramTable.Rows[0]["DO"] = "Query";

            BindReceiveMaterialList(paramTable);
        }

        /// <summary>
        /// 关闭当前页面
        /// </summary>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 自定义显示序号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvReceiveMaterialList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "ROWNUM": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindReceiveMaterialList(paramTable);
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveMaterialListCtrl_Load(object sender, EventArgs e)
        {
            ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            this.paramTable = receiveMaterial.CreatParamTable();
            this.paramTable.Rows.Add();
            this.paramTable.Rows[0]["STORE_NAME"] = PropertyService.Get(PROPERTY_FIELDS.STORES);
            BindReceiveMaterialList(this.paramTable);

            BindFactoryRoom();
            BindProId();
            BindSupplierName();
            BindEfficiency();

            this.deReceiveMaterialStart.DateTime = DateTime.Now.AddDays(-30);
            this.deReceiveMaterialEnd.DateTime = DateTime.Now;
        }

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }


        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.lueFactoryRoom.Properties.ReadOnly = false;
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用领料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定工单。
        /// </summary>
        private void BindWorkOrderNo()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            WorkOrders wo = new WorkOrders();
            DataSet ds = wo.GetWorkOrderByFactoryRoomKey(roomKey);
            if (string.IsNullOrEmpty(wo.ErrorMsg))
            {
                //绑定工单号数据到窗体控件。
                this.lueWorkOrderNo.Properties.DataSource = ds.Tables[0];
                this.lueWorkOrderNo.Properties.DisplayMember = "ORDER_NUMBER";
                this.lueWorkOrderNo.Properties.ValueMember = "ORDER_NUMBER";
                //this.lueWorkOrderNo.ItemIndex = 0;
            }
            else
            {
                this.lueWorkOrderNo.Properties.DataSource = null;
                this.lueWorkOrderNo.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定产品ID号。
        /// </summary>
        private void BindProId()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetProdId();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueProId.Properties.DataSource = ds.Tables[0];
                this.lueProId.Properties.DisplayMember = "PRODUCT_CODE";
                this.lueProId.Properties.ValueMember = "PRODUCT_CODE";
            }
            else
            {
                this.lueProId.Properties.DataSource = null;
                this.lueProId.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定转换效率。
        /// </summary>
        private void BindEfficiency()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetEfficiency();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueEfficiency.Properties.DataSource = ds.Tables[0];
                this.lueEfficiency.Properties.DisplayMember = "EFFICIENCY_NAME";
                this.lueEfficiency.Properties.ValueMember = "EFFICIENCY_NAME";
            }
            else
            {
                this.lueEfficiency.Properties.DataSource = null;
                this.lueEfficiency.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定供应商名称。
        /// </summary>
        private void BindSupplierName()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetSuppliers();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueSupplierName.Properties.DataSource = ds.Tables[0];
                this.lueSupplierName.Properties.DisplayMember = "NAME";
                this.lueSupplierName.Properties.ValueMember = "NAME";
            }
            else
            {
                this.lueSupplierName.Properties.DataSource = null;
                this.lueSupplierName.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 领料车间改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            //重新绑定工单
            BindWorkOrderNo();
        }
    }
}
