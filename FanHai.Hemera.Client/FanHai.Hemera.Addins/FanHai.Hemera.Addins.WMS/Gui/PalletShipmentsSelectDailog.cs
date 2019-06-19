using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class PalletShipmentsSelectDailog : BaseDialog
    {
        string werks;
        public string ShipmentNum = string.Empty;
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        public PalletShipmentsSelectDailog()
        {
            InitializeComponent();
        }

        private void sbtSelect_Click(object sender, EventArgs e)
        {
            this.ShipmentNum = txtShipmentNum.Text.Trim();
            if (!string.IsNullOrEmpty(ShipmentNum))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("查询条件出货单号不能为空！", "系统提示");
            }
        }


    }
}
