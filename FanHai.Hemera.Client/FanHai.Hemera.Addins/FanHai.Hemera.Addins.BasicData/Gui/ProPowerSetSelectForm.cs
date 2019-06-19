using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ProPowerSetSelectForm : BaseDialog
    {
        BasePowerSetEntity _powerSetEntity = new BasePowerSetEntity();
        DataTable _dtPowerSet = new DataTable(), _dtPowerSetSub = new DataTable(), _dtPowerSetColor = new DataTable();
        public string proPowerSetKey = string.Empty;
        public DataRow drProPowerSet = null;
        public DataRow[] drsProPowerSetSub = null;
        public DataRow[] drsProPowerSetColor = null;

        public ProPowerSetSelectForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {

            //分档代码，分档规则，功率上下线，是否存在子分档            
            Hashtable hashTable = new Hashtable();
            if (!string.IsNullOrEmpty(teProPowerSetCode.Text.Trim()))
                hashTable[BASE_POWERSET.FIELDS_PS_CODE] = teProPowerSetCode.Text.Trim();
            if (!string.IsNullOrEmpty(teProPowerSetRule.Text.Trim()))
                hashTable[BASE_POWERSET.FIELDS_PS_RULE] = teProPowerSetRule.Text.Trim();

            DataSet dsDataBind = _powerSetEntity.GetPowerSetData(hashTable);

            if (_powerSetEntity.ErrorMsg.Equals(string.Empty))
            {
                _dtPowerSet = dsDataBind.Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
                _dtPowerSetSub = dsDataBind.Tables[BASE_POWERSET_DETAIL.DATABASE_TABLE_NAME];
                _dtPowerSetColor = dsDataBind.Tables[BASE_POWERSET_COLORATCNO.DATABASE_TABLE_NAME];
                gcProPowerSet.DataSource = _dtPowerSet;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void gvProPowerSet_DoubleClick(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void GetRowData()
        {
            if (gvProPowerSet.FocusedRowHandle < 0) return;

            DataRow drFocused = gvProPowerSet.GetFocusedDataRow();
            proPowerSetKey = drFocused["POWERSET_KEY"].ToString();

            drsProPowerSetSub = _dtPowerSetSub.Select(string.Format(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY + "='{0}'", proPowerSetKey));
            drsProPowerSetColor = _dtPowerSetColor.Select(string.Format(BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY + "='{0}'", proPowerSetKey));
            drProPowerSet = _dtPowerSet.Select(string.Format(" POWERSET_KEY = '{0}' ", proPowerSetKey))[0];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}