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
    public partial class SendMaterialListCtrl : BaseUserCtrl
    {
        private DataTable paramTable;   //包含查询条件的数据表。
        MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public SendMaterialListCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gdvMaterialList);
        }
        public void InitializeLanguage()
        {
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0004}");//工厂
            layoutControlItem14.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0005}");//工序
            layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0006}");//设备
            layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0007}");//参数
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0008}");//工单
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0009}");//料号
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0010}");//单号
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0011}");//状态
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0012}");//创建时间-起
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.lbl.0013}");//创建时间-止

            gcRow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0001}");//序号
            gcStatus.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0002}");//状态
            gcFac.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0003}");//工厂
            gcOpName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0004}");//工序
            gcEquName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0005}");//   设备         
            gcLineName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0006}");//线别
            gcParameter.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0007}");//参数
            gcWordOrder.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0008}");//工单
            gcMatCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0009}");//物料
            gcMblnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0010}");//单号
            gcSendingQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0011}");//数量
            gcCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0012}");//创建人
            gcCreaterTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SendMaterialListCtrl.GridControl.0013}");//创建时间
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
            DataSet dsReturn = materialReqOrReturnEntity.GetMaterialSendingList(paramTable, ref config);
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
        /// <summary>绑定工序。
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                lueOperation.Properties.Items.Add("");
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    lueOperation.Properties.Items.Add(strOperations[i]);
                }
                this.lueOperation.SelectedIndex = 0;
            }
            //禁用工序
            if (string.IsNullOrEmpty(operations)
                || this.lueOperation.Properties.Items.Count <= 1)
            {
                this.lueOperation.Properties.ReadOnly = true;
            }
        }
        /// <summary>绑定设备
        /// 绑定设备
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.lueOperation.Text.Trim();
            string strFactoryRoomName = this.lueFactoryRoom.Text;
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue == null ? string.Empty : this.lueFactoryRoom.EditValue.ToString();
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
        /// <summary>绑定参数  只绑定维护了扣料设置有效的参数信息
        /// 绑定参数
        /// </summary>
        private void BindParameter()
        {
            SendingMaterialEntity sendingMaterialEntity = new SendingMaterialEntity();
            DataSet dsParameters = sendingMaterialEntity.GetParameters();
            DataTable dt = dsParameters.Tables[0];
            DataRow dr = dt.NewRow();
            dr["PARAMETER"] = "";
            dt.Rows.Add(dr);
            if (string.IsNullOrEmpty(sendingMaterialEntity.ErrorMsg))
            {
                lueType.Properties.DataSource = dt;
                this.lueType.Properties.DisplayMember = "PARAMETER";
                this.lueType.Properties.ValueMember = "PARAMETER_KEY";
                this.lueType.Properties.PopupFormSize = new Size(220, 140);
            }
            else
            {
                this.lueType.Properties.DataSource = null;
                this.lueType.EditValue = string.Empty;
                this.lueType.Text = string.Empty;
            }
        }
        /// <summary>
        /// 查询按钮点击弹出查询条件选择
        /// </summary>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            this.paramTable.Rows[0]["FACTORY_NAME"] = this.lueFactoryRoom.Text.Trim();
            this.paramTable.Rows[0]["OPERATION_NAME"] = this.lueOperation.Text.Trim();
            this.paramTable.Rows[0]["EQUIPMENT_NAME"] = this.lueEquipment.Text.Trim();
            this.paramTable.Rows[0]["PARAMETER"] = this.lueType.Text.Trim();
            this.paramTable.Rows[0]["MATERIAL_CODE"] = this.txtMat.Text.Trim();
            this.paramTable.Rows[0]["MBLNR"] = this.txtMblnr.Text.Trim();
            switch (this.cmbStatus.Text.Trim())
            {
                case "发料记录": this.paramTable.Rows[0]["STATUS"] = "1"; break;
                case "退料记录": this.paramTable.Rows[0]["STATUS"] = "0"; break;
                default: this.paramTable.Rows[0]["STATUS"] = ""; break;
            }
            this.paramTable.Rows[0]["CREATE_TIME_START"] = string.IsNullOrEmpty(this.timeStart.Text.ToString()) ? "1990-01-01 08:00:00" : this.timeStart.EditValue.ToString();
            this.paramTable.Rows[0]["CREATE_TIME_END"] = string.IsNullOrEmpty(this.timeEnd.Text.ToString()) ? DateTime.Now.ToString() : this.timeEnd.EditValue.ToString();
            this.paramTable.Rows[0]["WORK_ORDER"] = this.lueWorkOrderNo.Text.ToString();
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
            dtParams.Columns.Add("FACTORY_NAME");
            dtParams.Columns.Add("OPERATION_NAME");
            dtParams.Columns.Add("EQUIPMENT_NAME");
            dtParams.Columns.Add("PARAMETER");
            dtParams.Columns.Add("MATERIAL_CODE");
            dtParams.Columns.Add("MBLNR");
            dtParams.Columns.Add("STATUS");
            dtParams.Columns.Add("CREATE_TIME_START");
            dtParams.Columns.Add("CREATE_TIME_END");
            dtParams.Columns.Add("WORK_ORDER");
            return dtParams;
        }

        private void MaterialRequisitionListCtrl_Load(object sender, EventArgs e)
        {
            this.paramTable = CreatParamTabel();
            this.paramTable.Rows.Add();
            BindMaterialList(this.paramTable);
            BindFactoryRoom();
            BindOperations();
            BindParameter();
            BindWorkOrderNo();
        }

        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            BindOperations();
            BindWorkOrderNo();
        }

        private void lueOperation_EditValueChanged(object sender, EventArgs e)
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

        private void txtMblnr_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
