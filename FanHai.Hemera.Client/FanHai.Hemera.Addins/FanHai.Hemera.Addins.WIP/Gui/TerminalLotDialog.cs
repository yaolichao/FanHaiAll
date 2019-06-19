using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using System.Collections;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次结束对话框。
    /// </summary>
    public partial class TerminalLotDialog : BaseDialog
    {
        LotBaseInfoCtrl lotBaseInfo = new LotBaseInfoCtrl();
        LotOperationEntity _entity = new LotOperationEntity();
        DataSet dsLotInfo = null;
        LotOperationDetailModel _model = null;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public TerminalLotDialog(LotOperationDetailModel model)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.TerminalLotDialog.Title}"))
        {
            InitializeComponent();
            this._model = model;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TerminalLotDialog_Load(object sender, EventArgs e)
        {
            this.LotGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotGroup}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");

            LotQueryEntity queryEntity = new LotQueryEntity();
            dsLotInfo = queryEntity.GetLotInfo(this._model.LotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                dsLotInfo = null;
                return;
            }
            if (dsLotInfo.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。", this._model.LotNumber));
                dsLotInfo = null;
                return;
            }
            this.lotInfoLayout.Controls.Add(lotBaseInfo);
            lotBaseInfo.SetValueToControl(dsLotInfo); 
        }
        /// <summary>
        /// 取消Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 确认Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //提示是否结束批次
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.WIP.Msg.TerminalRemaind}", "${res:Global.SystemInfo}"))
            {
                DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];
                string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                string lineKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                string lineName = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                string workOrderKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                string qty = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                string edcInsKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
                string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                string remark = this.meRemark.Text;
                if (string.IsNullOrEmpty(remark))
                {
                    MessageService.ShowMessage("请输入备注信息。", "提示");
                    this.meRemark.Select();
                    return;
                }
                string shiftName = this._model.ShiftName;
                string shiftKey = string.Empty;
                //Shift shiftEntity = new Shift();
                //string shiftKey = shiftEntity.IsShiftValueExists(shiftName);//班次主键。
                ////获取班次主键失败。
                //if (!string.IsNullOrEmpty(shiftEntity.ErrorMsg))
                //{
                //    MessageService.ShowError(shiftEntity.ErrorMsg);
                //    return;
                //}
                ////没有排班。
                //if (string.IsNullOrEmpty(shiftKey))
                //{
                //    MessageService.ShowMessage("请先在系统中进行排班。", "提示");
                //    return;
                //}
                string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
                string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                DataSet dsParams = new DataSet();
                //组织不良数据。
                Hashtable htTransaction = new Hashtable();
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, qty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, lineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
                dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtTransaction);
                //组织其他附加参数数据
                Hashtable htMaindata = new Hashtable();
                htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, this._model.LotEditTime);
                DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
                dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
                dsParams.Tables.Add(dtParams);
                //执行结束。
                this._entity.LotTerminal(dsParams);
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                dsParams.Tables.Clear();
                dtTransaction = null;
                dtParams = null;
                dsParams = null;
            }
        }
       
    }
}
