#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Gui.Core;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.LookAndFeel;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
#endregion 


namespace FanHai.Hemera.Share.CommonControls.Dialogs
{
    public partial class BaseDialog : XtraForm
    {

        public BaseDialog()
        {
            InitializeComponent();
        }
        public BaseDialog(string titleName)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Text = titleName;
        }

        private void BaseDialog_Load(object sender, EventArgs e)
        {

        }

    }
}