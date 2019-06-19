/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SPC
{
    public partial class PointInformation : BaseDialog
    {
        private string _pointkey = null;
        SpcEntity spcEntity = new SpcEntity();
        string _edcInsKey = string.Empty;
        public PointInformation():base("详细信息")
        {
            InitializeComponent();
        }

        public PointInformation(string pointkey)
            : base("详细信息")
        {
            if (pointkey.Length < 1)
            {
                MessageService.ShowMessage("该采集点数据有错，请与系统管理员联系！");
                return;
            }

            _pointkey = pointkey;
           
            InitializeComponent();           
        }

        private void PointInformation_Load(object sender, EventArgs e)
        {
            if (_pointkey.Length < 0)
                return;

            DataTable dtInformation = spcEntity.GetPointInformation(_pointkey);
            gcControl.MainView = gvControl;
            gcControl.DataSource = dtInformation;
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

      
    }
}
