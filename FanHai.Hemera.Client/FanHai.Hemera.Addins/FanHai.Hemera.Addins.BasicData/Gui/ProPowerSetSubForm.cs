using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ProPowerSetSubForm : BaseDialog
    {
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();

        public string powerSubCode = string.Empty;
        public string powerSubLevel = string.Empty;
        public string powerSubPmin = string.Empty;
        public string powerSubPmax = string.Empty;


        public ProPowerSetSubForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void ProPowerSetSubForm_Load(object sender, EventArgs e)
        {
            
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            powerSubCode = txtDtlSubcode.Text.Trim();
            powerSubLevel = txtPowerLevel.Text.Trim();
            powerSubPmin = txtPdtlmin.Text.Trim();
            powerSubPmax = txtPdtlmax.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}