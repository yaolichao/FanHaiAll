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
    /// 预设暂停批次数据新增对话框。
    /// </summary>
    public partial class LotFutureHoldAddDialog : BaseDialog
    {
        /// <summary>
        /// 存储预设暂停批次数据的数据表对象。
        /// </summary>
        private DataTable dtLots=null;
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        private string _routeKey=string.Empty;
        /// <summary>
        /// 工艺流程组主键。
        /// </summary>
        private string _enterpriseKey=string.Empty;
        /// <summary>
        /// 工厂车间的主键。
        /// </summary>
        private string _factoryRoomKey = string.Empty;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="roomKey"></param>
        public LotFutureHoldAddDialog(string roomKey)
        {
            InitializeComponent();
            this._factoryRoomKey = roomKey;
        }
        /// <summary>
        /// 窗体载入函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotFutureHoldAddDialog_Load(object sender, EventArgs e)
        {
            BindActionToControl();
            BindReasonCodeGroupDataToControl();
            //BindLotNoToControl();
        }
        /// <summary>
        /// 绑定批次数据到控件。
        /// </summary>
        private void BindLotNoToControl()
        {
            string lotNo = this.lueLotNo.Text;
             LotQueryEntity entity = new LotQueryEntity();
            DataSet ds = entity.QueryUsingLotData(lotNo,this._factoryRoomKey);

            //if (string.IsNullOrEmpty(entity.ErrorMsg))
            //{
            //    this.lueLotNo.Properties.DataSource = ds.Tables[0];
            //    this.lueLotNo.Properties.DisplayMember = "LOT_NUMBER";
            //    this.lueLotNo.Properties.ValueMember = "LOT_NUMBER";
            //}
            //else
            //{
            //    this.lueLotNo.Properties.DataSource = null;
            //    MessageService.ShowMessage(entity.ErrorMsg,"提示");
            //}
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
        /// 新增按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string lotNo = this.lueLotNo.Text;
            if (string.IsNullOrEmpty(lotNo))
            {
                this.lueLotNo.Select();
                return;
            }

            if (dtLots != null && dtLots.Select(string.Format("LOT_NUMBER='{0}'", lotNo)).Length > 0)
            {
                MessageService.ShowMessage("该序列号已经存在于列表中。", "提示");
                this.lueLotNo.SelectAll();
                return;
            }
            if (dtLots != null && dtLots.Rows.Count >= 99)
            {
                MessageService.ShowMessage("预设暂停的数量不能超过99个。", "提示");
                this.lueLotNo.SelectAll();
                return;
            }
            LotQueryEntity entity = new LotQueryEntity();
            DataSet ds = entity.GetLotInfo(lotNo);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    MessageService.ShowMessage("该序列号不存在。", "提示");
                    this.lueLotNo.SelectAll();
                    return;
                }
                if (!string.IsNullOrEmpty(_factoryRoomKey))
                {
                    string curFactoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                    if (_factoryRoomKey != curFactoryRoomKey)
                    {
                        MessageService.ShowMessage("该批号在当前车间中不存在。");
                        this.lueLotNo.SelectAll();
                        return;
                    }
                }
                int stateFlag = Convert.ToInt32(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                int deletedTermFlag = Convert.ToInt32(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
                string routeKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string enterpriseKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);

                if (!string.IsNullOrEmpty(_routeKey) && _routeKey != routeKey)
                {
                    MessageService.ShowMessage(string.Format("该批号不属于【{0}】工艺流程。",this.txtRoute.Text));
                    this.lueLotNo.SelectAll();
                    return;
                }
                if (!string.IsNullOrEmpty(_enterpriseKey) && _enterpriseKey != enterpriseKey)
                {
                    MessageService.ShowMessage(string.Format("该批号不属于【{0}】工艺流程组。", this.txtEnterprise.Text));
                    this.lueLotNo.SelectAll();
                    return;
                }
                if (stateFlag >= 10)
                {
                    MessageService.ShowMessage("该序列号已完成。", "提示");
                    this.lueLotNo.SelectAll();
                    return;
                }
                if (deletedTermFlag!=0)
                {
                    MessageService.ShowMessage("该序列号已删除或已终止。", "提示");
                    this.lueLotNo.SelectAll();
                    return;
                }
                //第一次增加预设暂停的批次。
                if (dtLots == null || dtLots.Rows.Count==0)
                {
                    dtLots = ds.Tables[0].Copy();
                    this._routeKey = routeKey;
                    this._enterpriseKey =enterpriseKey;
                    this.txtEnterprise.Text = Convert.ToString(dtLots.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    this.txtRoute.Text = Convert.ToString(dtLots.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                }
                else
                {
                    dtLots.Merge(ds.Tables[0], true, MissingSchemaAction.Add);
                }
                this.gcLots.DataSource = dtLots;
                this.lueLotNo.EditValue = string.Empty;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg, "提示");
                this.lueLotNo.SelectAll();
            }
            this.lueLotNo.Select();

        }
        /// <summary>
        /// 删除按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtLots == null || dtLots.Rows.Count == 0)
            {
                return;
            }

            if (this.gvLots.SelectedRowsCount <= 0)
            {
                MessageService.ShowMessage("请先选择要从列表中移除的序列号。", "提示");
                return;
            }
            int [] selectedRows=this.gvLots.GetSelectedRows();
            foreach (int i in selectedRows)
            {
                string lotNo = Convert.ToString(this.gvLots.GetRowCellValue(i, POR_LOT_FIELDS.FIELD_LOT_NUMBER));
                if(dtLots!=null){
                    DataRow[] drs = this.dtLots.Select(string.Format("LOT_NUMBER='{0}'", lotNo));
                    foreach (DataRow dr in drs)
                    {
                        dtLots.Rows.Remove(dr);
                    }
                }
            }
            //如果没有预设暂停的批次。
            if (this.dtLots.Rows.Count <= 0)
            {
                this._routeKey = string.Empty;
                this._enterpriseKey = string.Empty;
                this.txtEnterprise.Text = string.Empty;
                this.txtRoute.Text =string.Empty;
                this.lueStep.EditValue = string.Empty;
            }

            this.gcLots.DataSource = dtLots;
        }
        /// <summary>
        /// 工艺流程改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRoute_EditValueChanged(object sender, EventArgs e)
        {
            BindStepToControl();
        }
        /// <summary>
        /// 确定按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtLots == null || dtLots.Rows.Count == 0)
            {
                MessageService.ShowMessage("请必须输入序列号。", "提示");
                this.lueLotNo.Select();
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
            string editor=PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
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
                string lotNo = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                string lotKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                string setEnterpriseKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string setRouteKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string setStepKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                string orderNumber = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
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
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD] = holdPassword;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK] = remark;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_ROUTE_KEY] = routeKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ENTERPRISE_KEY] = setEnterpriseKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ROUTE_KEY] = setRouteKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_STEP_KEY] = setStepKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_STEP_KEY] = stepKey;
                drNew[WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER] = orderNumber;
                dtParams.Rows.Add(drNew);
            }
            dsParams.Tables.Add(dtParams);

            FutureHoldEntity entity = new FutureHoldEntity();
            entity.Insert(dsParams);

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
        /// <summary>
        /// 取消按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 自定义单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLots_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            int row = e.RowHandle;
            if (e.Column.FieldName == "INDEX")
            {
                e.DisplayText = Convert.ToString(row + 1);
            }
        }
        /// <summary>
        /// 批次号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13){
                this.btnAdd_Click(sender, e);
            }
        }
        /// <summary>
        /// 原因代码组文本改变时触发。
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
                MessageService.ShowError(entity.ErrorMsg);
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


       

    }
}
