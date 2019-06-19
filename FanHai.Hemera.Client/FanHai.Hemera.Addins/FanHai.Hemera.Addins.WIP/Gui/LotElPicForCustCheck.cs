using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.IO;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotElPicForCustCheck : BaseDialog
    {
        public string picel_address = string.Empty;
        public string lotnumber = string.Empty;
        public string picTitle = string.Empty;

        public LotElPicForCustCheck()
        {
            InitializeComponent();          
        }

        private void LotElPicForCustCheck_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(picel_address))
            {
                if (!File.Exists(picel_address))
                {
                    MessageService.ShowError(string.Format("【EL图片名称{0}】不存在，请确认!", lotnumber));                    
                    return;
                }

                Image imgel = Image.FromFile(picel_address);

                picel.Image = imgel;
                //picel.Width = panel1.Width;
                //picel.Height = panel1.Height;
                this.lblPicAddressAndTime.Text = this.picTitle;
                picel.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void LotElPicForCustCheck_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)(Keys.Space)) || e.KeyChar.Equals((char)(Keys.Escape)) || e.KeyChar.Equals((char)(Keys.Enter)))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}