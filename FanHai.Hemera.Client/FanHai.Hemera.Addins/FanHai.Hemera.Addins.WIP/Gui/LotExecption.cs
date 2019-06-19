using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Dialogs;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using System.Net;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotExecption : BaseDialog
    {
        public string lotNo=string.Empty;
        public string startTime=string.Empty;
        public string endTime=string.Empty;

        public LotExecption()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtLotNo.Text.Trim() != ""||(sTime.Text.Trim()!=""&&eTime.Text.Trim()!=""))
            {
                lotNo = txtLotNo.Text;
                startTime = sTime.Text;
                endTime = eTime.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void LotExecption_Load(object sender, EventArgs e)
        {
            eTime.Text = DateTime.Now.AddDays(1).ToShortDateString();
        }

      
    }
}
