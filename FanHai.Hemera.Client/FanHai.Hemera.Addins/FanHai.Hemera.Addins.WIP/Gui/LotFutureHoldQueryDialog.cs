using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 查询预设暂停批次数据的对话框。
    /// </summary>
    public partial class LotFutureHoldQueryDialog : BaseDialog
    {

        /// <summary>
        /// 获取或设置批次号。
        /// </summary>
        public string LotNo
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置工单号。
        /// </summary>
        public string WorkOrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置工序名。
        /// </summary>
        public string OperationName
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置动作名。
        /// </summary>
        public string ActionName
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotFutureHoldQueryDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 关闭查询对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 确定按钮Click事件处理函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.LotNo = this.txtLotNo.Text.Trim();
            this.WorkOrderNo = this.txtWorkOrderNo.Text.Trim();
            this.OperationName = Convert.ToString(this.lueOperationName.EditValue);
            this.ActionName = Convert.ToString(this.lueAction.EditValue);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotFutureHoldQueryDialog_Load(object sender, EventArgs e)
        {
            BindOpeartionNameToControl();
            BindActionToControl();
            InitControlValue();
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            this.txtLotNo.Text = this.LotNo;
            this.txtWorkOrderNo.Text = this.WorkOrderNo;
            this.lueOperationName.EditValue = this.OperationName;
            this.lueAction.EditValue = this.ActionName;
        }
        /// <summary>
        /// 绑定工序名称到控件。
        /// </summary>
        private void BindOpeartionNameToControl()
        {
            RouteQueryEntity entity = new RouteQueryEntity();
            DataSet dsOperation = entity.GetDistinctOperationNameList();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                dsOperation.Tables[0].Rows.InsertAt(dsOperation.Tables[0].NewRow(),0);
                this.lueOperationName.Properties.DataSource = dsOperation.Tables[0];
                this.lueOperationName.Properties.ValueMember = "ROUTE_OPERATION_NAME";
                this.lueOperationName.Properties.DisplayMember = "ROUTE_OPERATION_NAME";
            }
            else
            {
                this.lueOperationName.Properties.DataSource = null;
                MessageService.ShowMessage(entity.ErrorMsg, "提示");
            }
        }
        /// <summary>
        /// 绑定动作到控件。
        /// </summary>
        private void BindActionToControl()
        {
            DataTable dtAction = new DataTable();
            dtAction.Columns.Add("ACTION_NAME");
            dtAction.Columns.Add("ACTION_VALUE");
            dtAction.Rows.Add("全部", string.Empty);
            dtAction.Rows.Add("进站后暂停", ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
            dtAction.Rows.Add("出站后暂停", ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);

            this.lueAction.Properties.DataSource = dtAction;
            this.lueAction.Properties.DisplayMember = "ACTION_NAME";
            this.lueAction.Properties.ValueMember = "ACTION_VALUE";
        }
    }
}
