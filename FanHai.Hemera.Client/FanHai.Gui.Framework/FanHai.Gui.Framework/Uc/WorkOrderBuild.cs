using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FanHai.MES.Framework
{
    public partial class WorkOrderBuild : Form
    {
        public WorkOrderBuild()
        {
            InitializeComponent();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            WorkOrderItemSetting wis = new WorkOrderItemSetting();
            wis.ShowDialog();
            //this.Hide();

        }

    
    }
}
