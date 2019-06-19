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
    /// 表示原材料清单的窗体类。
    /// </summary>
    public partial class MaterialRequisitionAndReturnListCtrl : BaseUserCtrl
    {
        private DataTable paramTable;   //包含查询条件的数据表。
        MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialRequisitionAndReturnListCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gdvMaterialList);
        }
        public void InitializeLanguage()
        {
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.lbl.0004}");//工厂
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.lbl.0005}");//工单
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.lbl.0006}");//料号
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.lbl.0007}");//单号
            checkEditStatus.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.lbl.0008}");//有效数据

            gcRow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0001}");//序号
            gcWorkOrder.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0002}");//工单号
            gcMaterial.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0003}");//原材料料号
            gcMblnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0004}");//单号
            gcPickingQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0005}");//领料数量
            gcBackQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0006}");//领料退料数量
            gcSendQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0007}");//发料数量
            gcSendBackQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0008}");//发料退料数量
            gcSumQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0009}");//总量
            gcUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0010}");//单位
            gcLliff.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionAndReturnListCtrl.GridControl.0011}");//供应商
           
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
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
        /// 绑定原材料清单列表。
        /// </summary>
        /// <param name="paramTable"></param>
        private void BindMaterialList(DataTable paramTable)
        {
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = materialReqOrReturnEntity.GetMaterialInf(paramTable, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;

            if (string.IsNullOrEmpty(materialReqOrReturnEntity.ErrorMsg))
            {
                gdcView.MainView = gdvMaterialList;
                gdcView.DataSource = dsReturn.Tables[0];
                gdvMaterialList.Invalidate();
                gdvMaterialList.BestFitColumns();
            }
            else
            {
                MessageBox.Show(materialReqOrReturnEntity.ErrorMsg);
            }
        }
        /// <summary>
        /// 查询按钮点击弹出查询条件选择
        /// </summary>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            this.paramTable.Rows[0]["WORK_ORDER_NUMBER"] = this.txtWorkOrder.Text.Trim();
            this.paramTable.Rows[0]["MATERIAL"] = this.txtMat.Text.Trim();
            this.paramTable.Rows[0]["MBLNR"] = this.txtMblnr.Text.Trim();
            this.paramTable.Rows[0]["FACTORY"] = this.lueFactoryRoom.Text.Trim();
            this.paramTable.Rows[0]["STATUS"] = this.checkEditStatus.Checked;
            
            BindMaterialList(paramTable);
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
            BindMaterialList(paramTable);
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaterialRequisitionAndReturnListCtrl_Load(object sender, EventArgs e)
        {
            this.paramTable = CreatParamTabel();
            DataRow dr = paramTable.NewRow();
            dr["WORK_ORDER_NUMBER"] = string.Empty;
            dr["MATERIAL"] = string.Empty;
            dr["MBLNR"] = string.Empty;
            dr["FACTORY"] = string.Empty;
            dr["STATUS"] = this.checkEditStatus.Checked;
            this.paramTable.Rows.Add(dr);
            BindMaterialList(this.paramTable);
            BindFactoryRoom();
        }

        private DataTable CreatParamTabel()
        {
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("WORK_ORDER_NUMBER");
            dtParams.Columns.Add("MATERIAL");
            dtParams.Columns.Add("MBLNR");
            dtParams.Columns.Add("FACTORY");
            dtParams.Columns.Add("STATUS");
            return dtParams;
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
