using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls;

using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次操作(暂停批次)明细的控件类。
    /// </summary>
    public partial class LotOperationHold : BaseUserCtrl
    {
        /// <summary>
        /// 用于原因列表中暂存确认密码的字段名称。
        /// </summary>
        private const string TMP_HOLD_INFO_CONFIRM_HOLD_PASSWORD = "CONFIRM_HOLD_PASSWORD";

        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationHold(LotOperationDetailModel model, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotOperationHold_Load(object sender, EventArgs e)
        {
            //this.lblTitle.Text = this._view.TitleName;
            BindLotInfo();
            BindHoldInfo();
            BindHoldReasonCodeCategory();
            ResetControlValue();
            //ResetControlValue();
            
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            LotQueryEntity queryEntity = new LotQueryEntity();
            DataSet dsReturn = queryEntity.GetLotInfo(this._model.LotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                dsReturn = null;
                return;
            }
            if (dsReturn.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。", this._model.LotNumber));
                dsReturn = null;
                return;
            }
            dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            this.gcLotList.MainView = this.gvList;
            this.gcLotList.DataSource = dsReturn.Tables[0];

            this.txtLotNumber.Text = dsReturn.Tables[0].Rows[0]["LOT_NUMBER"].ToString();
            this.txtWorkorderNo.Text = dsReturn.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
            this.txtProID.Text = dsReturn.Tables[0].Rows[0]["PRO_ID"].ToString();
            this.txtEnterpriseName.Text= dsReturn.Tables[0].Rows[0]["ENTERPRISE_NAME"].ToString();
            this.txtRouteName.Text = dsReturn.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
            this.txtStepName.Text = dsReturn.Tables[0].Rows[0]["ROUTE_STEP_NAME"].ToString();
            this.txtQuantity.Text = dsReturn.Tables[0].Rows[0]["QUANTITY"].ToString();
            this.txtEfficiency.Text = dsReturn.Tables[0].Rows[0]["EFFICIENCY"].ToString();
            this.txtSILot.Text = dsReturn.Tables[0].Rows[0]["SI_LOT"].ToString();
        }
        /// <summary>
        /// 初始化暂停原因列表。
        /// </summary>
        private void BindHoldInfo()
        {
            WIP_HOLD_RELEASE_FIELDS holdFields = new WIP_HOLD_RELEASE_FIELDS();
            DataTable dtList = CommonUtils.CreateDataTable(holdFields);
            dtList.Columns.Add(TMP_HOLD_INFO_CONFIRM_HOLD_PASSWORD);
            this.gcHoldInfoList.MainView = this.gvHoldInfoList;
            this.gcHoldInfoList.DataSource = dtList;
        }
        /// <summary>
        /// 绑定原因代码组。
        /// </summary>
        private void BindHoldReasonCodeCategory()
        {
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);  //拥有权限的工序
            DataSet dsReturn = this._entity.GetHoldReasonCodeCategory();
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueReasonCodeGroup.Columns.Clear();
            this.rilueReasonCodeGroup.Columns.Add(new LookUpColumnInfo(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME, "名称"));
            this.rilueReasonCodeGroup.DataSource = dsReturn.Tables[0];
            this.rilueReasonCodeGroup.DisplayMember = FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME;
            this.rilueReasonCodeGroup.ValueMember = FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY;
        }
        /// <summary>
        /// 绑定原因代码组。
        /// </summary>
        private void BindHoldReasonCode(string categoryKey)
        {
            DataSet dsReasonCode = this._entity.GetReasonCode(categoryKey);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueReasonCode.Columns.Clear();
            this.rilueReasonCode.Columns.Add(new LookUpColumnInfo(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME, "名称"));
            this.rilueReasonCode.DataSource = dsReasonCode.Tables[0];
            this.rilueReasonCode.DisplayMember = FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME;
            this.rilueReasonCode.ValueMember = FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY;
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            //如果是暂停批次。
            if (this._model.OperationType == LotOperationType.Hold)
            {
                this.lciSearchLotNumber.Visibility = LayoutVisibility.Never;
                this.lciCommandAdd.Visibility = LayoutVisibility.Never;
                this.lciCommandRemove.Visibility = LayoutVisibility.Never;
                this.lciList.SizeConstraintsType = SizeConstraintsType.Custom;
                lblMenu.Text = "生产管理>组件管理>单件暂停";
                this.lcgHold.Visibility = LayoutVisibility.Always;
                this.lciList.Visibility = LayoutVisibility.Never;
            }
            //如果是批量暂停批次。
            else
            {
                this.lciSearchLotNumber.Visibility = LayoutVisibility.Always;
                this.lciCommandAdd.Visibility = LayoutVisibility.Always;
                this.lciCommandAdd.SizeConstraintsType = SizeConstraintsType.Custom;
                this.lciCommandAdd.MaxSize = new Size(70, 0);
                this.lciCommandRemove.Visibility = LayoutVisibility.Always;
                this.lciCommandRemove.SizeConstraintsType = SizeConstraintsType.Custom;
                this.lciCommandRemove.MaxSize = new Size(70, 0);
                this.lciList.SizeConstraintsType = SizeConstraintsType.Default;
                lblMenu.Text = "生产管理>组件管理>多件暂停";
                this.lcgHold.Visibility = LayoutVisibility.Never;
            }
            DataTable dtList = this.gcHoldInfoList.DataSource as DataTable;
            dtList.Rows.Clear();
            //初始化原因列表。
            DataRow dr = dtList.NewRow();
            dtList.Rows.Add(dr);
            this.teRemark.Text = string.Empty;
        }

        /// <summary>
        /// 显示批次查询对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beLotNumber_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            LotQueryHelpModel model = new LotQueryHelpModel();
            model.RoomKey = this._model.RoomKey;
            model.OperationType = this._model.OperationType;
            LotQueryHelpDialog dlg = new LotQueryHelpDialog(model);
            dlg.OnValueSelected += new LotQueryValueSelectedEventHandler(LotQueryHelpDialog_OnValueSelected);
            Point i = beLotNumber.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = beLotNumber.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + beLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + beLotNumber.Width - dlg.Width, i.Y + beLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + beLotNumber.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 选中批次值后的事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void LotQueryHelpDialog_OnValueSelected(object sender, LotQueryValueSelectedEventArgs args)
        {
            this.beLotNumber.Text = args.LotNumber;
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            ResetControlValue();
        }
        /// <summary>
        /// 输入待暂停批次号后的回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.btnAdd_Click(sender, e);
            }
        }
        /// <summary>
        /// 添加待暂停的批次信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtList = this.gcLotList.DataSource as DataTable;
            string lotNumber = this.beLotNumber.Text.ToUpper().Trim();
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageService.ShowMessage("请输入序列号。", "提示");
                this.beLotNumber.Select();
                return;
            }

            int count = dtList.AsEnumerable()
                              .Count(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]) == lotNumber);
            if (count>0)
            {
                MessageService.ShowMessage(string.Format("【{0}】在列表中已存在，请确认。",lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            LotQueryEntity queryEntity = new LotQueryEntity();
            DataSet dsReturn = queryEntity.GetLotInfo(lotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                dsReturn = null;
                this.beLotNumber.SelectAll();
                return;
            }
            if (dsReturn.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请检查。", lotNumber));
                dsReturn = null;
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次号在指定车间中是否存在。
            string currentRoomKey = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            if (this._model.RoomKey!= currentRoomKey)
            {
                MessageService.ShowMessage(string.Format("【{0}】在当前车间中不存在，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否被锁定
            int holdFlag = Convert.ToInt32(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            if (holdFlag == 1)
            {
                MessageService.ShowMessage(string.Format("【{0}】已被暂停，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否被删除
            int deleteFlag = Convert.ToInt32(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            if (deleteFlag == 1)
            {
                MessageService.ShowMessage(string.Format("【{0}】已结束，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否已结束
            if (deleteFlag == 2)
            {
                MessageService.ShowMessage(string.Format("【{0}】已删除，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            dtList.Merge(dsReturn.Tables[0]);
            this.beLotNumber.SelectAll();
        }
        /// <summary>
        /// 删除待暂停的批次信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = this.gvList.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要移除的批次信息。", "提示");
                return;
            }
            DataTable dtList = this.gcLotList.DataSource as DataTable;
            if (dtList.Rows.Count <= 1)
            {
                MessageService.ShowMessage("批次信息列表中必须至少有一条记录。", "提示");
                return;
            }
            dtList.Rows.RemoveAt(index);
        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHoldInfoList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gclRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gclReasonCode)
            {
                DataTable dtHoldInfo = this.gcHoldInfoList.DataSource as DataTable;
                e.DisplayText=Convert.ToString(dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME]);
            }
        }
        /// <summary>
        /// 原因代码中的单元格值改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHoldInfoList_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DataTable dtHoldInfo = this.gcHoldInfoList.DataSource as DataTable;
            //原因代码组
            if (e.Column == this.gclReasonCodeGroup)
            {
                dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY]=string.Empty;
                dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME]=string.Empty;
                string categoryKey = Convert.ToString(e.Value);
                string categoryName =Convert.ToString(this.rilueReasonCodeGroup.GetDisplayValueByKeyValue(categoryKey));
                dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME] = categoryName;
            }
            //原因代码
            else if (e.Column == this.gclReasonCode)
            {
                string codeKey = Convert.ToString(e.Value);
                string codeName = Convert.ToString(this.rilueReasonCode.GetDisplayValueByKeyValue(codeKey));
                dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME] = codeName;
            }
            //密码或确认密码
            else if (e.Column == this.gclConfirmPassword || e.Column==this.gclPassword)
            {
                string password = Convert.ToString(dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD]);
                string confirmPassword = Convert.ToString(dtHoldInfo.Rows[e.RowHandle][TMP_HOLD_INFO_CONFIRM_HOLD_PASSWORD]);
                //密码为空。确认密码不为空要求必须输入密码
                if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
                {
                    this.gvHoldInfoList.FocusedColumn = this.gclPassword;
                    this.gvHoldInfoList.FocusedRowHandle = e.RowHandle;
                    this.gvHoldInfoList.ShowEditor();
                    return;
                }
                //确认密码为空。密码不为空要求必须输入确认密码
                if (string.IsNullOrEmpty(confirmPassword) && !string.IsNullOrEmpty(password))
                {
                    this.gvHoldInfoList.FocusedColumn = this.gclConfirmPassword;
                    this.gvHoldInfoList.FocusedRowHandle = e.RowHandle;
                    this.gvHoldInfoList.ShowEditor();
                    return;
                }
                //密码和确认密码不一致。
                if (password != confirmPassword)
                {
                    MessageService.ShowMessage("两次输入的密码不一致，请重新输入。", "提示");
                    dtHoldInfo.Rows[e.RowHandle][e.Column.FieldName] = string.Empty;
                    this.gvHoldInfoList.FocusedColumn = e.Column;
                    this.gvHoldInfoList.FocusedRowHandle = e.RowHandle;
                    this.gvHoldInfoList.ShowEditor();
                    return;
                }
            }
        }
        /// <summary>
        /// 自定义显示编辑器。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHoldInfoList_ShowingEditor(object sender, CancelEventArgs e)
        {
            //显示原因代码编辑器
            if (this.gvHoldInfoList.FocusedColumn == this.gclReasonCode
                && this.gvHoldInfoList.FocusedRowHandle >= 0)
            {
                DataTable dtHoldInfo = this.gcHoldInfoList.DataSource as DataTable;
                string categoryKey = Convert.ToString(dtHoldInfo.Rows[this.gvHoldInfoList.FocusedRowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY]);
                if (!string.IsNullOrEmpty(categoryKey))
                {
                    BindHoldReasonCode(categoryKey);
                }
                else
                {
                    this.rilueReasonCode.DataSource = null;
                }
            }
        }
        /// <summary>
        /// 保存，暂停批次。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            if (this.gvHoldInfoList.State == GridState.Editing 
                && this.gvHoldInfoList.IsEditorFocused
                && this.gvHoldInfoList.EditingValueModified)
            {
                this.gvHoldInfoList.SetFocusedRowCellValue(this.gvHoldInfoList.FocusedColumn, this.gvHoldInfoList.EditingValue);
            }
            this.gvHoldInfoList.UpdateCurrentRow();

            DataTable dtLotInfo = this.gcLotList.DataSource as DataTable;
            if (dtLotInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage("待暂停批次信息列表至少要有一条记录。", "提示");
                return;
            }
            DataTable dtHoldInfo = this.gcHoldInfoList.DataSource as DataTable;
            if (dtHoldInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage("暂停信息列表至少要有一条记录。", "提示");
                return;
            }
            string remark = this.teRemark.Text.Trim();
            if (string.IsNullOrEmpty(remark))
            {
                MessageService.ShowMessage("备注必须输入。", "提示");
                this.teRemark.Select();
                return;
            }
            //暂停组代码必须输入
            var lnq = from item in dtHoldInfo.AsEnumerable()
                      where string.IsNullOrEmpty(Convert.ToString(item[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY]))
                      select item;
            foreach (var item in lnq)
            {
                MessageService.ShowMessage("暂停组代码必须输入。", "提示");
                this.gvHoldInfoList.FocusedColumn = this.gclReasonCodeGroup;
                this.gvHoldInfoList.FocusedRowHandle = dtHoldInfo.Rows.IndexOf(item);
                this.gvHoldInfoList.ShowEditor();
                return;
            }
            //暂停名称必须输入
            lnq = from item in dtHoldInfo.AsEnumerable()
                  where string.IsNullOrEmpty(Convert.ToString(item[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY]))
                  select item;
            foreach (var item in lnq)
            {
                MessageService.ShowMessage("暂停名称必须输入。", "提示");
                this.gvHoldInfoList.FocusedColumn = this.gclReasonCode;
                this.gvHoldInfoList.FocusedRowHandle = dtHoldInfo.Rows.IndexOf(item);
                this.gvHoldInfoList.ShowEditor();
                return;
            }
            //密码和确认密码不匹配。
            lnq = from item in dtHoldInfo.AsEnumerable()
                  where Convert.ToString(item[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD]) != Convert.ToString(item[TMP_HOLD_INFO_CONFIRM_HOLD_PASSWORD])
                  select item;
            foreach (var item in lnq)
            {
                MessageService.ShowMessage("暂停密码和确认密码不匹配。", "提示");
                this.gvHoldInfoList.FocusedColumn = this.gclPassword;
                this.gvHoldInfoList.FocusedRowHandle = dtHoldInfo.Rows.IndexOf(item);
                this.gvHoldInfoList.ShowEditor();
                return;
            }

            string shiftName = this._model.ShiftName;
            string shiftKey = string.Empty;
            //string shiftKey = shiftEntity.IsShiftValueExists(shiftName);//班次主键。
            //Shift shiftEntity = new Shift();
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
            //存放待暂停的批次的操作数据
            WIP_TRANSACTION_FIELDS transFields = new WIP_TRANSACTION_FIELDS();
            DataTable dtTransaction = CommonUtils.CreateDataTable(transFields);
            foreach (DataRow dr in dtLotInfo.Rows)
            {
                //组织待暂停的批次的操作数据
                DataRow drTransaction = dtTransaction.NewRow();
                dtTransaction.Rows.Add(drTransaction);
                string transKey = CommonUtils.GenerateNewKey(0);
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = dr[POR_LOT_FIELDS.FIELD_LOT_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = dr[POR_LOT_FIELDS.FIELD_QUANTITY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = dr[POR_LOT_FIELDS.FIELD_QUANTITY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = dr[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = dr[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = dr[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = dr[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY] = shiftKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = shiftName;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = dr[POR_LOT_FIELDS.FIELD_STATE_FLAG];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = dr[POR_LOT_FIELDS.FIELD_IS_REWORKED];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR] = this._model.UserName;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER] = oprComputer;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = dr[POR_LOT_FIELDS.FIELD_OPR_LINE];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE] = dr[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY] = dr[POR_LOT_FIELDS.FIELD_EDC_INS_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = remark;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = this._model.UserName;
                //用于暂存序列号批次信息最后的编辑时间，以便判断序列号信息是否过期。
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = dr[POR_LOT_FIELDS.FIELD_EDIT_TIME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP] = DBNull.Value;
            }
            //存放暂停操作的明细记录。
            DataTable dtHoldParams = dtHoldInfo.Copy();
            dtHoldParams.Columns.Remove(TMP_HOLD_INFO_CONFIRM_HOLD_PASSWORD);     //移除临时列。
            foreach (DataRow dr in dtHoldParams.Rows)
            {
                dr[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_OPERATOR] = this._model.UserName;
                dr[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIMEZONE] = timezone;
                dr[WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR] = this._model.UserName;
                dr[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
            }
            dsParams.Tables.Add(dtTransaction);
            dsParams.Tables.Add(dtHoldParams);
            //执行暂停批次。
            this._entity.LotHold(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                //this.tsbCancle_Click(sender, e);
                MessageService.ShowMessage("保存成功");
                WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
                //重新打开批次创建视图。
                LotOperationViewContent view = new LotOperationViewContent(this._model.OperationType);
                WorkbenchSingleton.Workbench.ShowView(view);
            }
            dsParams.Tables.Clear();
            dtTransaction = null;
            dtHoldParams = null;
            dsParams = null;
        }
    }
}
