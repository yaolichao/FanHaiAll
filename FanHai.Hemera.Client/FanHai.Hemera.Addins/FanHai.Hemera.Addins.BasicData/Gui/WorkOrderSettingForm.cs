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
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class WorkOrderSettingForm : BaseDialog
    {
        WorkOrders workOrderEntity = new WorkOrders();
        public string por_work_order_key = string.Empty;
        public string por_work_order_Number = string.Empty;
        public string por_ProID = string.Empty;

        public WorkOrderSettingForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Hashtable hstable = new Hashtable();
            if (!string.IsNullOrEmpty(txtWorkOrder.Text.Trim()))
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtWorkOrder.Text.Trim();
            if (!string.IsNullOrEmpty(txtPro_id.Text.Trim()))
                hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE] = txtPro_id.Text.Trim();

            DataSet dsReturn = workOrderEntity.GetWorkOrderByNoOrProid(hstable);

            DataTable dtWorkOrder = dsReturn.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            gcWorkOrder.DataSource = dtWorkOrder;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void gvWorkOrder_DoubleClick(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
       
        private void GetRowData()
        {
            if (gvWorkOrder.FocusedRowHandle < 0) return;

            DataRow drFocused = gvWorkOrder.GetFocusedDataRow();
            por_work_order_key = drFocused[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
            por_work_order_Number = drFocused[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString();
            por_ProID = drFocused[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString();
            if (string.IsNullOrEmpty(por_work_order_key.Trim()))
            {
                MessageService.ShowMessage("未读取到工单ID，请与管理员联系!");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}