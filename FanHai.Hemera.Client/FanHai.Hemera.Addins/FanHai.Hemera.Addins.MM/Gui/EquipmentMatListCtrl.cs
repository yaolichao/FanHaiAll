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
    public partial class EquipmentMatListCtrl : BaseUserCtrl
    {
        private DataTable paramTable;   //包含查询条件的数据表。
        MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentMatListCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gdvMaterialList);
        }
        public void InitializeLanguage()
        {
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.lbl.0004}");//工厂
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.lbl.0005}");//工序
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.lbl.0006}");//设备
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.lbl.0007}");//工单
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.lbl.0008}");//物料
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.lbl.0009}");//材料批次

            gcRownum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0001}");//序号
            gcFac.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0002}");//工厂
            gcOperationName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0003}");//工序
            gcEquipmentName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0004}");//设备
            gcLine.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0005}");//线别
            gcWorkOrder.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0006}");//工单
            gcMaterialCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0007}");//物料
            gcParameter.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0008}");//参数
            gcSupplierCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0009}");//材料批次
            gcSumQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0010}");//在线余量
            gcSendingQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0011}");//已发数量
            gcSendingBackQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0012}");//已退数量
            gcUsedQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0013}");//已耗数量
            gcUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0014}");//单位
            gcSupplier.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.GridControl.0015}");//供应商
        }

        /// <summary>
        /// 绑定设备物料看板。
        /// </summary>
        /// <param name="paramTable"></param>
        private void BindMaterialList(DataTable paramTable)
        {
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = materialReqOrReturnEntity.GetEquMaterialInf(paramTable, ref config);
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
            this.lueFac.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            DataRow dr = dt.NewRow();
            dr["LOCATION_NAME"] = "";
            dt.Rows.Add(dr);
            if (dt != null)
            {
                this.lueFac.Properties.DataSource = dt;
                this.lueFac.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFac.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFac.ItemIndex = 0;
                }
            }
            else
            {
                this.lueFac.Properties.DataSource = null;
                this.lueFac.EditValue = string.Empty;
            }
        }
        /// <summary>绑定工序。
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            this.cmbOprition.Properties.ReadOnly = false;
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                cmbOprition.Properties.Items.Add("");
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cmbOprition.Properties.Items.Add(strOperations[i]);
                }
                this.cmbOprition.SelectedIndex = 0;
            }
            //禁用工序
            if (string.IsNullOrEmpty(operations)
                || this.cmbOprition.Properties.Items.Count <= 1)
            {
                this.cmbOprition.Properties.ReadOnly = true;
            }
        }
        /// <summary>绑定设备
        /// 绑定设备
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cmbOprition.Text.Trim();
            string strFactoryRoomName = this.lueFac.Text;
            string strFactoryRoomKey = this.lueFac.EditValue == null ? string.Empty : this.lueFac.EditValue.ToString();
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            //如果工厂车间或者工序或者线别主键为空。
            if (string.IsNullOrEmpty(strFactoryRoomName)
                || string.IsNullOrEmpty(strOperation)
                || string.IsNullOrEmpty(strLines))
            {
                return;
            }
            this.lueEquipment.EditValue = string.Empty;
            this.lueEquipment.Properties.ReadOnly = false;

            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, strLines.Split(','));
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueEquipment.Properties.DataSource = ds.Tables[0];
                this.lueEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE;
                this.lueEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
        }
        /// <summary>绑定工单。
        /// 绑定工单。
        /// </summary>
        private void BindWorkOrderNo()
        {
            string roomKey = Convert.ToString(this.lueFac.EditValue);
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
            this.paramTable.Rows[0]["FACTORY_NAME"] = this.lueFac.Text.Trim();
            this.paramTable.Rows[0]["OPERATION_NAME"] = this.cmbOprition.Text.Trim();
            this.paramTable.Rows[0]["EQUIPMENT_NAME"] = this.lueEquipment.Text.Trim();
            this.paramTable.Rows[0]["ORDER_NUMBER"] = this.lueWorkOrderNo.Text.Trim();
            this.paramTable.Rows[0]["MATERIAL_CODE"] = this.txtMat.Text.Trim();
            this.paramTable.Rows[0]["SUPPLIER_CODE"] = this.txtMatCode.Text.Trim();
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
            this.paramTable.Rows.Add();
            BindMaterialList(this.paramTable);
            BindFactoryRoom();
            BindOperations();
            BindEquipment();
        }

        private DataTable CreatParamTabel()
        {
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("FACTORY_NAME");
            dtParams.Columns.Add("OPERATION_NAME");
            dtParams.Columns.Add("EQUIPMENT_NAME");
            dtParams.Columns.Add("ORDER_NUMBER");
            dtParams.Columns.Add("MATERIAL_CODE");
            dtParams.Columns.Add("SUPPLIER_CODE");
            return dtParams;
        }
        /// <summary>
        /// 自定义显示序号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvMaterialList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
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

        private void lueFac_EditValueChanged(object sender, EventArgs e)
        {
            BindOperations();
            BindEquipment();
            BindWorkOrderNo();
        }

        private void cmbOprition_EditValueChanged(object sender, EventArgs e)
        {
            BindEquipment();
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
