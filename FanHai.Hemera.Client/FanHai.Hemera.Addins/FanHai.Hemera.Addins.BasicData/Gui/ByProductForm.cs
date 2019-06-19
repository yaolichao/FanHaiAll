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
using FanHai.Hemera.Utils.Entities.BasicData;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ByProductForm : BaseDialog
    {
        ByProductEntity byProductEntity = new ByProductEntity();
        public string por_part = string.Empty;
        DataSet dsReturn = new DataSet();
        public ByProductForm()
        {
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)
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
            if (gvByProduct.FocusedRowHandle < 0) return;

            DataRow drFocused = gvByProduct.GetFocusedDataRow();
            por_part = drFocused["PART_NAME"].ToString();
            if (string.IsNullOrEmpty(por_part.Trim()))
            {
                MessageService.ShowMessage("未读取产品料号,请与管理员联系!");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btSeach_Click(object sender, EventArgs e)
        {
            string strPartName = txtPartName.Text;
            DataSet ds = byProductEntity.GetLotPartInf(strPartName);
            DataTable dtByProduct = ds.Tables[0];
            gcByProduct.DataSource = dtByProduct;
        }

        private void ByProductForm_Load(object sender, EventArgs e)
        {
            DataSet ds = byProductEntity.GetLotPartInf("");
            DataTable dtByProduct = ds.Tables[0];
            gcByProduct.DataSource = dtByProduct;
        }


    }
}