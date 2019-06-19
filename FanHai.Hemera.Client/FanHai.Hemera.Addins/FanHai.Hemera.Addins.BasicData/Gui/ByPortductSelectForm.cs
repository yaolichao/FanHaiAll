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
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ByPortductSelectForm : BaseDialog
    {
        public string strM = string.Empty;
        public string strB2 = string.Empty;
        public string strB3 = string.Empty;
        public string strType = string.Empty;
        public string strCreater = string.Empty;
        public DateTime strCtstart = DateTime.Parse(DateTime.Parse("1999-01-01").ToString());
        public DateTime strCtsend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        public string strEditer = string.Empty;
        //public DateTime strEtstart = new DateTime();
        //public DateTime strEtsend = new DateTime();
        public ByPortductSelectForm()
        {
            InitializeComponent();
        }

        private void spbCanel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void spbOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcs.Text.ToString()) && !string.IsNullOrEmpty(txtce.Text.ToString()))
            {
                if (DateTime.Compare(Convert.ToDateTime(txtcs.Text), Convert.ToDateTime(txtce.Text)) > 0)
                {
                    MessageService.ShowMessage("截止时间不能小于起始时间！", "系统提示！");
                    txtcs.EditValue = DateTime.Now.ToString("yyyy-MM-01");
                    txtce.EditValue = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                }
                //else if (DateTime.Compare(Convert.ToDateTime(txtes.Text), Convert.ToDateTime(txtee.Text)) > 0)
                //{
                //    MessageService.ShowMessage("截止时间不能小于起始时间！", "系统提示！");
                //    txtes.EditValue = DateTime.Now.ToString("yyyy-MM-01");
                //    txtee.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                //}
                else
                {
                    strM = txtm.Text;
                    strB2 = txtb2.Text;
                    strB3 = txtb3.Text;
                    strType = txttype.Text;
                    strCreater = txtcreater.Text;
                    strEditer = txtediter.Text;
                    strCtstart = DateTime.Parse(txtcs.EditValue.ToString());
                    strCtsend = DateTime.Parse(txtce.EditValue.ToString());
                    //strEtstart = DateTime.Parse(txtes.EditValue.ToString());
                    //strEtsend = DateTime.Parse(txtee.EditValue.ToString());
                    this.DialogResult = DialogResult.OK;
                    this.Close();                    
                }
            }
            else
            {
                strM = txtm.Text;
                strB2 = txtb2.Text;
                strB3 = txtb3.Text;
                strType = txttype.Text;
                strCreater = txtcreater.Text;
                strEditer = txtediter.Text;
                if (string.IsNullOrEmpty(txtcs.Text.ToString()))
                {
                    strCtstart = DateTime.Parse(DateTime.Parse("1999-01-01").ToString());
                }
                else
                {
                    strCtstart = DateTime.Parse(txtcs.EditValue.ToString());
                }
                if (string.IsNullOrEmpty(txtce.Text.ToString()))
                {
                    strCtsend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }
                else
                {
                    strCtsend = DateTime.Parse(txtce.EditValue.ToString());
                }


                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }
        /// <summary>
        /// 绑定组件类型
        /// </summary>
        public void BindSupplierCode()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.PART_TYPE);

            DataTable dt = BaseData.Get(columns, category);
            DataRow dr = dt.NewRow();
            dr["NAME"] = string.Empty;
            dt.Rows.Add(dr);

            this.txttype.Properties.DataSource = dt;
            this.txttype.Properties.DisplayMember = "NAME";
            this.txttype.Properties.ValueMember = "CODE";
            this.txttype.ItemIndex = 0;
        }

        private void ByPortductSelectForm_Load(object sender, EventArgs e)
        {
            BindSupplierCode();
            txtcs.EditValue = DateTime.Now.ToString("yyyy-MM-01"); 
            txtce.EditValue = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //txtee.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            //txtes.EditValue = DateTime.Now.ToString("yyyy-MM-01"); 
        }

    }
}