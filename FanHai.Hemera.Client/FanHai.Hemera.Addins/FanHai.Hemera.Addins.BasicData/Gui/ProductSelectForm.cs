using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SolarViewer.Hemera.Share.CommonControls.Dialogs;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Hemera.Share.Constants;
using System.Collections;
using SolarViewer.Gui.Core;

namespace SolarViewer.Hemera.Addins.BasicData
{
    public partial class ProductSelectForm : BaseDialog
    {
        PorProductEntity _porProductEntity = new PorProductEntity();
        public DataTable _dtProduct = new DataTable();
        public string productKey = string.Empty;
        public DataRow drProduct = null;

        public ProductSelectForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Hashtable hstable = new Hashtable();
            if (!string.IsNullOrEmpty(teProCode.Text.Trim()))
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = teProCode.Text.Trim();
            if (!string.IsNullOrEmpty(teTestRuleCode.Text.Trim()))
                hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE] = teTestRuleCode.Text.Trim();

            DataSet dsReturn = _porProductEntity.GetPorProductData(hstable);

            _dtProduct = dsReturn.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
            gcProduct.DataSource = _dtProduct;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void gvProduct_DoubleClick(object sender, EventArgs e)
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
            if (gvProduct.FocusedRowHandle < 0) return;

            DataRow drFocused = gvProduct.GetFocusedDataRow();
            productKey = drFocused[POR_PRODUCT.FIELDS_PRODUCT_KEY].ToString();
            
             drProduct = _dtProduct.Select(string.Format("{0} = '{1}' ",POR_PRODUCT.FIELDS_PRODUCT_KEY,productKey))[0];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}