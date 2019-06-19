using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.WMS.Gui
{
    public partial class OutDeliveryQCQuerry : BaseDialog
    {
        private DataTable dt = null;

        public OutDeliveryQCQuerry(DataTable dt)
        {
            InitializeComponent();
            this.dt = dt;
            gridControl1.DataSource = dt;
        }
    }
}
