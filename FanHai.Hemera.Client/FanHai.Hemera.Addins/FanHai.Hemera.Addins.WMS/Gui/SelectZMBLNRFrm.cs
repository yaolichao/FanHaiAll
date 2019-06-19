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
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class SelectZMBLNRFrm : BaseDialog
    {
        int formType;//调用这个窗口的控件  1、WarehouseWarrantCtrl   2、WarehouseWarrantSynCtrl
        public SelectZMBLNRFrm(DataTable dt, int formType)
        {
            InitializeComponent();
            gcShowZMBLNR.DataSource = dt;
            gvShowZMBLNR.RefreshData();
            this.formType = formType;
        }

        public WarehouseWarrantSynCtrl PWarehouseWarrantSyn
        {
            get;
            set;
        }

        public WarehouseWarrantCtrl PWarehouseWarrant
        {
            get;
            set;
        }

        private void gvShowZMBLNR_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if(formType == 2)
                    PWarehouseWarrantSyn.retIdx = e.RowHandle;
                if (formType == 1)
                    PWarehouseWarrant.retIdx = e.RowHandle;
                this.Close();
            }
        }

        private void gvShowZMBLNR_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridView gv = sender as GridView;
            if (e.KeyChar == (char)13 && gv.SelectedRowsCount > 0)
            {
                if (formType == 2)
                    PWarehouseWarrantSyn.retIdx = gv.GetSelectedRows()[0];
                if (formType == 1)
                    PWarehouseWarrant.retIdx = gv.GetSelectedRows()[0];
                this.Close();
            }
        }
    }
}
