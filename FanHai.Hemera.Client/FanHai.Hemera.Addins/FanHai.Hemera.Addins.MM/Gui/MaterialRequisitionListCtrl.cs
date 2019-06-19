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
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示领料退料清单的窗体类。
    /// </summary>
    public partial class MaterialRequisitionListCtrl : BaseUserCtrl
    {
        private DataTable paramTable;   //包含查询条件的数据表。
        MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialRequisitionListCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gdvMaterialList);
        }
        public void InitializeLanguage()
        {
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0004}");//工厂
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0005}");//单号
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0006}");//工单
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0007}");//料号
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0008}");//状态
            layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0009}");//单类型
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0010}");//创建时间-起
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.lbl.0011}");//创建时间-止

            gcRow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0001}");//序号
            gcStatusToRestect.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0002}");//单号类型
            gcMblnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0003}");//单号
            gcMaterial.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0004}");//料号
            gcMatxt.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0005}");//物料描述
            gcQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0006}");//数量
            gcErfme.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0007}");//单位
            gcLlief.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0008}");//供应商
            gcAufnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0009}");//工单
            gcMemo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0010}");//备注
            gcCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0011}");//创建人
            gcCreattime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionListCtrl.GridControl.0012}");//创建时间
           
        }

        /// <summary>
        /// 绑定领料退料清单列表。
        /// </summary>
        /// <param name="paramTable"></param>
        private void BindMaterialList(DataTable paramTable)
        {
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = materialReqOrReturnEntity.GetMaterialRequisitionList(paramTable, ref config);
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
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            DataRow dr = dt.NewRow();
            dr["LOCATION_NAME"] = "";
            dt.Rows.Add(dr);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom.ItemIndex = 0;
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }
        /// <summary>绑定工单。
        /// 绑定工单。
        /// </summary>
        private void BindWorkOrderNo()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            WorkOrders wo = new WorkOrders();
            DataSet ds = wo.GetWorkOrderByFactoryRoomKey(roomKey);
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.NewRow();
            dr["ORDER_NUMBER"] = "";
            dt.Rows.Add(dr);
            if (string.IsNullOrEmpty(wo.ErrorMsg))
            {
                //绑定工单号数据到窗体控件。
                this.lueWorkOrderNo.Properties.DataSource = dt;
                this.lueWorkOrderNo.Properties.DisplayMember = "ORDER_NUMBER";
                this.lueWorkOrderNo.Properties.ValueMember = "ORDER_NUMBER";
            }
            else
            {
                this.lueWorkOrderNo.Properties.DataSource = null;
                this.lueWorkOrderNo.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 查询按钮点击弹出查询条件选择
        /// </summary>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            this.paramTable.Rows[0]["MBLNR"] = this.txtMblnr.Text.Trim();
            this.paramTable.Rows[0]["MATNR"] = this.txtMat.Text.Trim();
            this.paramTable.Rows[0]["AUFNR"] = this.lueWorkOrderNo.Text.Trim();
            switch (this.cmbStatus.Text.Trim())
            {
                case "已创建": this.paramTable.Rows[0]["STATUS"] = "";  break;
                case "已过账": this.paramTable.Rows[0]["STATUS"] = "T";  break;
                case  "ALL": this.paramTable.Rows[0]["STATUS"] = "ALL";  break;
                default: this.paramTable.Rows[0]["STATUS"] = "ALL"; break;
            }
            this.paramTable.Rows[0]["FACTORYNAME"] = this.lueFactoryRoom.Text.Trim();
            this.paramTable.Rows[0]["STATUSTORESTECT"] = this.cmbType.Text.Trim();
            switch (this.cmbType.Text.Trim())
            {
                case "领料记录": this.paramTable.Rows[0]["STATUSTORESTECT"] = "1"; break;
                case "退料记录": this.paramTable.Rows[0]["STATUSTORESTECT"] = "0"; break;
                case "ALL": this.paramTable.Rows[0]["STATUSTORESTECT"] = ""; break;
                default: this.paramTable.Rows[0]["STATUSTORESTECT"] = ""; break;
            }
            this.paramTable.Rows[0]["CREATE_TIME_START"] = string.IsNullOrEmpty(this.timeStart.Text.ToString()) ? "1990-01-01 08:00:00" : this.timeStart.EditValue.ToString();
            this.paramTable.Rows[0]["CREATE_TIME_END"] = string.IsNullOrEmpty(this.timeEnd.Text.ToString()) ? DateTime.Now.ToString() : this.timeEnd.EditValue.ToString();
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


        private DataTable CreatParamTabel()
        {
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("MBLNR");
            dtParams.Columns.Add("MATNR");
            dtParams.Columns.Add("AUFNR");
            dtParams.Columns.Add("CREATE_TIME_START");
            dtParams.Columns.Add("CREATE_TIME_END");
            dtParams.Columns.Add("FACTORYNAME");
            dtParams.Columns.Add("STATUSTORESTECT");
            dtParams.Columns.Add("STATUS");
            return dtParams;
        }

        private void MaterialRequisitionListCtrl_Load(object sender, EventArgs e)
        {
            this.paramTable = CreatParamTabel();
            this.paramTable.Rows.Add();
            this.paramTable.Rows[0]["STATUS"] = "ALL";
            BindMaterialList(this.paramTable);
            BindFactoryRoom();
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
