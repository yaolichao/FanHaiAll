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
    public partial class ProPowerSetColorForm : BaseDialog
    {   
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();
        public string colorCode = string.Empty;
        public string colorName = string.Empty;
        public string colorDes = string.Empty;
        public string articleNo = string.Empty;

        public ProPowerSetColorForm()
        {
            InitializeComponent();
        }

        private void ProPowerSetColorForm_Load(object sender, EventArgs e)
        {
            BindProLevel();
        }
        private void BindProLevel()
        {
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtCommon = BaseData.Get(l_s, category);

            DataTable dtLevel = dtCommon.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.MODELCOLOR));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";

            lueColor.Properties.DisplayMember = "Column_Name";
            lueColor.Properties.ValueMember = "Column_code";
            lueColor.Properties.DataSource = dview.Table;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            DataSet dsPowerSetDtlColor = new DataSet();
            
            colorCode = Convert.ToString(lueColor.EditValue);
            articleNo = txtArticleNo.Text.Trim();
            colorDes = meDescription.Text.Trim();
            colorName = lueColor.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}