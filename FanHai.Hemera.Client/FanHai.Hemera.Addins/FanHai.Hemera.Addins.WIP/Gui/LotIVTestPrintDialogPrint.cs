using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotIVTestPrintDialog4 : UserControl
    {
        public LotIVTestPrintDialog4()
        {
            InitializeComponent();
        }

        private void LotIVTestPrintDialog4_Load(object sender, EventArgs e)
        {
            LotIVTestPrintDialog LotIVTestPrintDialog = new LotIVTestPrintDialog();

            LotIVTestPrintDialog.FormBorderStyle = FormBorderStyle.None;
            LotIVTestPrintDialog.TopLevel = false;    //设置子窗体为非顶级窗体                       

            LotIVTestPrintDialog.Dock = System.Windows.Forms.DockStyle.Fill;//设置样式是否填充整个panel                 

           // panel1.Controls.Add(LotIVTestPrintDialog);
            LotIVTestPrintDialog.Parent = this.panel1;
            LotIVTestPrintDialog.Show();
        }
    }
}
