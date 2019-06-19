/*
<FileInfo>
  <Author>rayna liu FanHai Hemera</Author>
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
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class ChoiceLayoutPicDialog : BaseDialog
    {
        public string picPath = string.Empty;
        public ChoiceLayoutPicDialog()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(216, 229, 248);

            InitUi();
        }

        private void InitUi()
        {
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ChoiceLayoutPicDialog.title}");
            lblSelectPic.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ChoiceLayoutPicDialog.lbl.0001}");
            btnOk.Text = StringParser.Parse("${res:Global.OKButtonText}");
            btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (tePicPath.Text.Trim().Length == 0)
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ChoiceLayoutPicDialog.msg.0001}"));//请选择图片
                return;
            }
            picPath = tePicPath.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "JPG(*.JPG;*.JPEG);gif文件(*.GIF);png文件(*.png)|*.jpg;*.jpeg;*.gif;*.png";
            ofdPic.FilterIndex = 1;
            ofdPic.FileName = "";
            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                tePicPath.Text = ofdPic.FileName;
            }
        }
    }
}
