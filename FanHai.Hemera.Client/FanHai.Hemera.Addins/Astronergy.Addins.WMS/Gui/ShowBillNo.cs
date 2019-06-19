using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SolarViewer.Hemera.Share.CommonControls.Dialogs;
using DevExpress.XtraGrid.Views.Grid;

namespace Astronergy.Addins.WMS.Gui
{
    public partial class ShowBillNo : BaseDialog
    {
        public ShowBillNo(DataTable dt)
        {
            InitializeComponent();
            gcShowBillNo.DataSource = dt;
            gvShowBillNo.RefreshData();
        }
       
        public OutboundQCControl pOutboundQCControl
        {
            get;
            set;
        }

        private void gvShowBillNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridView gv = sender as GridView;
            if (e.KeyChar == (char)13 && gv.SelectedRowsCount > 0)
            {              
                pOutboundQCControl.retIdx = gv.GetSelectedRows()[0];             
                this.Close();
            }
        }

        private void gvShowBillNo_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                pOutboundQCControl.retIdx = e.RowHandle;
                this.Close();
            }
        }
    }
}
