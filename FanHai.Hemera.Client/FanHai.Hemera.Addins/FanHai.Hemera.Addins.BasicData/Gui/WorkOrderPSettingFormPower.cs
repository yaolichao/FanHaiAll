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
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    public partial class WorkOrderPSettingFormPower : BaseDialog
    {
        WorkOrders workOrderEntity = new WorkOrders();
        public string por_rule_code = string.Empty;
        public string por_before_power = string.Empty;
        public string por_after_power = string.Empty;
        public DataRow drProPowerSet = null;
        public DataTable _dtPowerShowData = new DataTable();
        public WorkOrderPSettingFormPower()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }




        private void GetRowData()
        {
            if (gvPowerShow.FocusedRowHandle < 0) return;

            DataRow drFocused = gvPowerShow.GetFocusedDataRow();
            por_rule_code = drFocused["RULE_CODE"].ToString();
            por_before_power = drFocused["BEFORE_POWER"].ToString();
            por_after_power = drFocused["AFTER_POWER"].ToString();
            drProPowerSet = _dtPowerShowData.Select(string.Format(" RULE_CODE = '{0}' ", por_rule_code))[0];

            if (string.IsNullOrEmpty(por_rule_code.Trim()))
            {
                MessageService.ShowMessage("未读取到规则信息RULE_CODE，请与管理员联系!");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void WorkOrderPSettingFormPower_Load(object sender, EventArgs e)
        {
            string[] l_s = new string[] { "RULE_CODE", "BEFORE_POWER", "AFTER_POWER" };
            string category = "Basic_WorkOrderProductSetting";
            DataTable dtWorkOrderPower = BaseData.Get(l_s, category);
            dtWorkOrderPower.TableName = "PowerShow";
            _dtPowerShowData = dtWorkOrderPower;
            DataView dview = dtWorkOrderPower.DefaultView;
            dview.Sort = "BEFORE_POWER asc";

            gcPowerShow.DataSource = dtWorkOrderPower;
        }

       

      

      

    }
}
