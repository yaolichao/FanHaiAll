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
using DevExpress.XtraLayout.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 预设暂停批次数据明细对话框。用于修改/查看预设暂停批次数据
    /// </summary>
    public partial class LotFutureHoldDetailDialog : BaseDialog
    {
        private LotBaseInfoCtrl _lotInfoCtrl = new LotBaseInfoCtrl();
        /// <summary>
        /// 预设暂停的主键。
        /// </summary>
        private string _futureHoldKey = string.Empty;
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        private string _routeKey = string.Empty;
        /// <summary>
        /// 工艺流程组主键。
        /// </summary>
        private string _enterpriseKey = string.Empty;
        private DataSet _dsParams = null;
        private DataSet _dsLot = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="futureHoldKey"></param>
        public LotFutureHoldDetailDialog(string futureHoldKey)
        {
            InitializeComponent();
            this._futureHoldKey = futureHoldKey;
            _lotInfoCtrl.Dock = DockStyle.Fill;
            this.pcLotInformation.Controls.Add(_lotInfoCtrl);
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotFutureHoldDetailDialog_Load(object sender, EventArgs e)
        {
            FutureHoldEntity entity = new FutureHoldEntity();
            _dsParams = entity.Get(this._futureHoldKey);
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowMessage(entity.ErrorMsg);
                _dsParams = null;
            }

            if (_dsParams != null   
                && _dsParams.Tables.Count > 0 
                && _dsParams.Tables[0].Rows.Count>0)
            {
                DataTable dt = _dsParams.Tables[0];
                this._enterpriseKey = Convert.ToString(dt.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_ENTERPRISE_KEY]);
                this._routeKey = Convert.ToString(dt.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_ROUTE_KEY]);
                BindLotBaseInfoToControl();
                BindReasonCodeGroupDataToControl();
                BindActionToControl();
                BindStepToControl();
                InitControlValue();
            }
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            DataTable dtFutureHold=_dsParams.Tables[0];
            int status = Convert.ToInt32(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_STATUS]);
            string stepKey = Convert.ToString(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_STEP_KEY]);
            string holdGroupKey = Convert.ToString(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY]);
            string holdCodeKey = Convert.ToString(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY]);
            string password = Convert.ToString(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD]);
            string remark = Convert.ToString(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK]);
            string action = Convert.ToString(dtFutureHold.Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME]);
            string enterpriseName = Convert.ToString(dtFutureHold.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            string routeName = Convert.ToString(dtFutureHold.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            this.lueAction.EditValue = action;
            this.lueStep.EditValue = stepKey;
            this.lueHoldGroup.EditValue = holdGroupKey;
            this.lueHoldCode.EditValue = holdCodeKey;
            this.meRemark.Text = remark;
            this.txtEnterprise.Text = enterpriseName;
            this.txtRoute.Text = routeName;
            this.teHoldPassword.Text = password;
            this.teConfirmPassword.Text = password;
            this.lciOkButton.Visibility = status == 0 ? LayoutVisibility.Never : LayoutVisibility.Always;
            this.lueAction.Properties.ReadOnly = (status == 0);
            this.lueStep.Properties.ReadOnly = (status == 0);
            this.lueHoldGroup.Properties.ReadOnly = (status == 0);
            this.lueHoldCode.Properties.ReadOnly = (status == 0);
            this.meRemark.Properties.ReadOnly = (status == 0);
        }

        /// <summary>
        /// 绑定批次的基础数据。
        /// </summary>
        private void BindLotBaseInfoToControl()
        {
            string lotKey = Convert.ToString(_dsParams.Tables[0].Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_KEY]);
            LotQueryEntity entity = new LotQueryEntity();
            _dsLot = entity.GetLotInfo(lotKey);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                _lotInfoCtrl.SetValueToControl(_dsLot);
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg, "错误");
            }
        }
        /// <summary>
        /// 绑定HOLD原因代码组数据。
        /// </summary>        
        private void BindReasonCodeGroupDataToControl()
        {
            LotOperationEntity entity = new LotOperationEntity();
            DataSet ds = entity.GetHoldReasonCodeCategory();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                this.lueHoldGroup.Properties.DataSource = null;
            }
            else
            {
                if (ds != null)
                {
                    this.lueHoldGroup.Properties.DataSource = ds.Tables[0];
                    this.lueHoldGroup.Properties.DisplayMember = "REASON_CODE_CATEGORY_NAME";
                    this.lueHoldGroup.Properties.ValueMember = "REASON_CODE_CATEGORY_KEY";
                }
                else
                {
                    this.lueHoldGroup.Properties.DataSource = null;
                }
            }
        }

        /// <summary>
        /// 绑定工序数据到控件。
        /// </summary>
        private void BindStepToControl()
        {
            RouteQueryEntity entity = new RouteQueryEntity();
            DataSet ds = entity.GetRouteStepDataByRouteKey(this._routeKey);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueStep.Properties.DataSource = ds.Tables[0];
                this.lueStep.Properties.ValueMember = "ROUTE_STEP_KEY";
                this.lueStep.Properties.DisplayMember = "ROUTE_STEP_NAME";
            }
            else
            {
                this.lueStep.Properties.DataSource = null;
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
            dtAction.Rows.Add("进站后暂停", ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
            dtAction.Rows.Add("出站后暂停", ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);

            this.lueAction.Properties.DataSource = dtAction;
            this.lueAction.Properties.DisplayMember = "ACTION_NAME";
            this.lueAction.Properties.ValueMember = "ACTION_VALUE";
        }
        /// <summary>
        /// 暂停组别文本改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueHoldGroup_EditValueChanged(object sender, EventArgs e)
        {
            LotOperationEntity entity = new LotOperationEntity();
            string categoryKey = Convert.ToString(this.lueHoldGroup.EditValue);
            DataSet ds = entity.GetReasonCode(categoryKey);
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowMessage(entity.ErrorMsg, "提示");
                this.lueHoldCode.Properties.DataSource = null;
            }
            else
            {
                if (ds != null)
                {
                    this.lueHoldCode.Properties.DataSource = ds.Tables[0];
                    this.lueHoldCode.Properties.DisplayMember = "REASON_CODE_NAME";
                    this.lueHoldCode.Properties.ValueMember = "REASON_CODE_KEY";
                }
                else
                {
                    this.lueHoldCode.Properties.DataSource = null;
                }
            }
        }
        /// <summary>
        /// 取消按钮Click事件函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 确定按钮Click事件函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_dsLot == null) return;
            if (_dsParams == null) return;

            DataTable dtLots = _dsLot.Tables[0];
            if (dtLots == null || dtLots.Rows.Count == 0)
            {
                MessageService.ShowMessage("记录为空，不能进行修改。", "提示");
                return;
            }

            string enterpriseKey = this._enterpriseKey;
            string routeKey = this._routeKey;
            string stepKey = Convert.ToString(this.lueStep.EditValue);
            string stepName = Convert.ToString(this.lueStep.Text);
            string action = Convert.ToString(this.lueAction.EditValue);
            string holdGroupName = Convert.ToString(this.lueHoldGroup.Text);
            string holdGroupKey = Convert.ToString(this.lueHoldGroup.EditValue);
            string holdCode = Convert.ToString(this.lueHoldCode.Text);
            string holdCodeKey = Convert.ToString(this.lueHoldCode.EditValue);
            string remark = this.meRemark.Text.Trim();
            string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string holdPassword = this.teHoldPassword.Text;
            string confirmPassword = this.teConfirmPassword.Text;

            if (string.IsNullOrEmpty(stepKey))
            {
                MessageService.ShowMessage("请先选择暂停工序。", "提示");
                this.lueStep.Select();
                return;
            }
            if (string.IsNullOrEmpty(action))
            {
                MessageService.ShowMessage("请先选择暂停动作。", "提示");
                this.lueAction.Select();
                return;
            }
            if (string.IsNullOrEmpty(holdGroupKey))
            {
                MessageService.ShowMessage("请先选择暂停组别。", "提示");
                this.lueHoldGroup.Select();
                return;
            }
            if (string.IsNullOrEmpty(holdCodeKey))
            {
                MessageService.ShowMessage("请先选择暂停原因。", "提示");
                this.lueHoldCode.Select();
                return;
            }
            //if (string.IsNullOrEmpty(holdPassword))
            //{
            //    MessageService.ShowMessage("请先输入暂停密码。", "提示");
            //    this.teHoldPassword.Select();
            //    return;
            //}
            if (holdPassword != confirmPassword)
            {
                MessageService.ShowMessage("输入的密码不一致，请重新输入。", "提示");
                this.teConfirmPassword.Select();
                return;
            }

            if (string.IsNullOrEmpty(remark))
            {
                MessageService.ShowMessage("请必须输入备注。", "提示");
                this.meRemark.Select();
                return;
            }

            DataSet dsParams = new DataSet();
            DataTable dtParams = new DataTable();
            dtParams.TableName = WIP_FUTUREHOLD_FIELDS.DATABASE_TABLE_NAME;
            WIP_FUTUREHOLD_FIELDS field = new WIP_FUTUREHOLD_FIELDS();
            foreach (string key in field.FIELDS.Keys)
            {
                dtParams.Columns.Add(key);
            }
            foreach (DataRow dr in dtLots.Rows)
            {
                DataRow drNew = dtParams.NewRow();
                string rowkey = this._futureHoldKey;
                string lotNo = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                string lotKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                string setEnterpriseKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string setRouteKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string setStepKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                string orderNumber = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                string editTime = Convert.ToString(_dsParams.Tables[0].Rows[0][WIP_FUTUREHOLD_FIELDS.FIELDS_EDIT_TIME]);
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME] = action;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_EDITOR] = editor;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_ENTERPRISE_KEY] = enterpriseKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_KEY] = lotKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER] = lotNo;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME] = stepName;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE] = holdCode;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY] = holdGroupKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_NAME] = holdGroupName;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY] = holdCodeKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK] = remark;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_ROUTE_KEY] = routeKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ENTERPRISE_KEY] = setEnterpriseKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ROUTE_KEY] = setRouteKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_STEP_KEY] = setStepKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_STEP_KEY] = stepKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER] = orderNumber;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_ROW_KEY] = rowkey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD] = holdPassword; 
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_EDIT_TIME] = editTime; 
                dtParams.Rows.Add(drNew);
            }
            dsParams.Tables.Add(dtParams);

            FutureHoldEntity entity = new FutureHoldEntity();
            entity.Update(dsParams);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg, "错误");
            }
        }
    }
}
