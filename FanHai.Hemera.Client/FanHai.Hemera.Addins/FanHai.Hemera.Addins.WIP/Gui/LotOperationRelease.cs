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
    /// 表示批次操作(释放批次)明细的控件类。
    /// </summary>
    public partial class LotOperationRelease : BaseUserCtrl
    {
        /// <summary>
        /// 临时存放释放批次密码的字段名称。
        /// </summary>
        private const string TEMP_HOLD_LIST_RELEASE_PASSWORD = "RELEASE_PASSWORD";

        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationRelease(LotOperationDetailModel model, IViewContent view)
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
        private void LotOperationRelease_Load(object sender, EventArgs e)
        {
           // this.lblTitle.Text = this._view.TitleName;
            BindLotInfo();
            BindHoldInfo();
            ResetControlValue();
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
            this.txtEnterpriseName.Text = dsReturn.Tables[0].Rows[0]["ENTERPRISE_NAME"].ToString();
            this.txtRouteName.Text = dsReturn.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
            this.txtStepName.Text = dsReturn.Tables[0].Rows[0]["ROUTE_STEP_NAME"].ToString();
            this.txtQuantity.Text = dsReturn.Tables[0].Rows[0]["QUANTITY"].ToString();
            this.txtEfficiency.Text = dsReturn.Tables[0].Rows[0]["EFFICIENCY"].ToString();
            this.txtSILot.Text = dsReturn.Tables[0].Rows[0]["SI_LOT"].ToString();
        }
        /// <summary>
        /// 绑定暂停信息。
        /// </summary>
        private void BindHoldInfo()
        {
            DataSet dsReturn = this._entity.GetLotHoldInfo(this._model.LotNumber);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                dsReturn = null;
                return;
            }
            if (dsReturn.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】暂停信息失败，请重试。", this._model.LotNumber));
                dsReturn = null;
                return;
            }
            dsReturn.Tables[0].TableName = WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME;
            //添加用于存储密码的临时列
            dsReturn.Tables[0].Columns.Add(TEMP_HOLD_LIST_RELEASE_PASSWORD); 
            this.gcHoldInfoList.MainView = this.gvHoldInfoList;
            this.gcHoldInfoList.DataSource = dsReturn.Tables[0];
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            //如果是释放批次。
            if (this._model.OperationType == LotOperationType.Release)
            {
                this.lciSearchLotNumber.Visibility = LayoutVisibility.Never;
                this.lciCommandAdd.Visibility = LayoutVisibility.Never;
                this.lciCommandRemove.Visibility = LayoutVisibility.Never;
                this.lciList.SizeConstraintsType = SizeConstraintsType.Custom;
                lblMenu.Text = "生产管理>组件管理>单件释放";
                this.lcgHold.Visibility = LayoutVisibility.Always;
                this.lciList.Visibility = LayoutVisibility.Never;
            }
            //如果是批量释放批次。
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
                this.lblMenu.Text = "生产管理>组件管理>多件释放";
                this.lcgHold.Visibility = LayoutVisibility.Never;
            }
            this.beLotNumber.Text = string.Empty;
            this.teRemark.Text = string.Empty;
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void tsbCancle_Click(object sender, EventArgs e)
        //{
        //    WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        //    //遍历工作台中已经打开的视图对象。
        //    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
        //    {
        //        //如果已打开创建批次的视图，则选中该视图显示，返回以结束该方法的运行。
        //        if (viewContent is LotOperation && viewContent.TitleName==this.lblTitle.Name)
        //        {
        //            viewContent.WorkbenchWindow.SelectWindow();
        //            return;
        //        }
        //    }
        //    //重新打开批次创建视图。
        //    LotOperationViewContent view = new LotOperationViewContent(this._model.OperationType);
        //    WorkbenchSingleton.Workbench.ShowView(view);
        //}
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
        /// 输入待释放批次号后的回车事件。
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
        /// 添加待释放的批次信息。
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
            if (holdFlag == 0)
            {
                MessageService.ShowMessage(string.Format("【{0}】未暂停，请确认。", lotNumber), "提示");
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
            //获取暂停信息。
            DataSet dsHoldInfo = this._entity.GetLotHoldInfo(lotNumber);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                dsHoldInfo = null;
                return;
            }
            if (dsHoldInfo.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】暂停信息失败，请重试。", lotNumber));
                dsHoldInfo = null;
                return;
            }
            dsHoldInfo.Tables[0].TableName = WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME;
            //添加用于存储密码的临时列
            dsHoldInfo.Tables[0].Columns.Add(TEMP_HOLD_LIST_RELEASE_PASSWORD);

            //添加批次信息。
            dtList.Merge(dsReturn.Tables[0]);
            //添加暂停信息。
            DataTable dtHoldList = this.gcHoldInfoList.DataSource as DataTable;
            dtHoldList.Merge(dsHoldInfo.Tables[0]);
            this.beLotNumber.SelectAll();
        }
        /// <summary>
        /// 删除待释放的批次信息。
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
            string lotKey =Convert.ToString(dtList.Rows[index][POR_LOT_FIELDS.FIELD_LOT_KEY]);
            //删除暂停信息
            DataTable dtHoldList = this.gcHoldInfoList.DataSource as DataTable;
            var lnq = from item in dtHoldList.AsEnumerable()
                      where Convert.ToString(item[POR_LOT_FIELDS.FIELD_LOT_KEY]) == lotKey
                      select item;
            foreach (DataRow dr in lnq)
            {
                dtHoldList.Rows.Remove(dr);
            }
            //删除批次信息。
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
            else if (e.Column == this.gclHoldTime)
            {
                string holdTime = Convert.ToString(e.CellValue);
                if (!string.IsNullOrEmpty(holdTime))
                {
                    DateTime dtHoldTime = DateTime.Parse(holdTime);
                    e.DisplayText = dtHoldTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
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
            //密码
            if (e.Column==this.gclPassword)
            {
                string password = Convert.ToString(dtHoldInfo.Rows[e.RowHandle][WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD]);
                string confirmPassword = Convert.ToString(dtHoldInfo.Rows[e.RowHandle][TEMP_HOLD_LIST_RELEASE_PASSWORD]);
                //密码和暂停时输入的密码不一致。
                if (password != confirmPassword)
                {
                    MessageService.ShowMessage("密码和暂停时输入的密码不一致，请重新输入。", "提示");
                    dtHoldInfo.Rows[e.RowHandle][e.Column.FieldName] = string.Empty;
                    this.gvHoldInfoList.FocusedColumn = e.Column;
                    this.gvHoldInfoList.FocusedRowHandle = e.RowHandle;
                    this.gvHoldInfoList.ShowEditor();
                    return;
                }
            }
        }
        /// <summary>
        /// 保存，释放批次。
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
                MessageService.ShowMessage("待释放批次信息列表至少要有一条记录。", "提示");
                return;
            }
            DataTable dtHoldInfo = this.gcHoldInfoList.DataSource as DataTable;
            if (dtHoldInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage("暂停信息列表至少要有一条记录。", "提示");
                return;
            }
            string remark = this.teRemark.Text;
            //释放密码和暂停密码是否匹配，如果有不匹配的给出提示。
            var lnq = from item in dtHoldInfo.AsEnumerable()
                      where Convert.ToString(item[TEMP_HOLD_LIST_RELEASE_PASSWORD]) != Convert.ToString(item[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD])
                      select item;
            foreach (var item in lnq)
            {
                MessageService.ShowMessage("释放密码和暂停密码不匹配，不能释放。", "提示");
                this.gvHoldInfoList.FocusedColumn = this.gclPassword;
                this.gvHoldInfoList.FocusedRowHandle = dtHoldInfo.Rows.IndexOf(item);
                this.gvHoldInfoList.ShowEditor();
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
            //存放待释放的批次的操作数据
            WIP_TRANSACTION_FIELDS transFields = new WIP_TRANSACTION_FIELDS();
            DataTable dtTransaction = CommonUtils.CreateDataTable(transFields);
            foreach (DataRow dr in dtLotInfo.Rows)
            {
                //组织待释放的批次的操作数据
                DataRow drTransaction = dtTransaction.NewRow();
                dtTransaction.Rows.Add(drTransaction);
                string transKey = CommonUtils.GenerateNewKey(0);
                string lotKey=Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = lotKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE;
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
                //组织暂停批次信息。
                lnq = from item in dtHoldInfo.AsEnumerable()
                      where Convert.ToString(item[POR_LOT_FIELDS.FIELD_LOT_KEY]) == lotKey
                      select item;
                foreach (DataRow drHoldInfo in lnq)
                {
                    drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_TRANSACTION_KEY] = transKey;
                    drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_DESCRIPTION] = remark;
                    drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_OPERATOR] = this._model.UserName;
                    drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_TIMEZONE] = timezone;
                    drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR] = this._model.UserName;
                    drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
                }
            }
            //存放释放操作的明细记录。
            DataTable dtHoldParams = dtHoldInfo.Copy();
            dtHoldParams.Columns.Remove(TEMP_HOLD_LIST_RELEASE_PASSWORD);     //移除临时列。
            dtHoldParams.Columns.Remove(POR_LOT_FIELDS.FIELD_LOT_KEY);        //移除批次主键列。
            dtHoldParams.Columns.Remove(POR_LOT_FIELDS.FIELD_LOT_NUMBER);     //移除批次号列。

            dsParams.Tables.Add(dtTransaction);
            dsParams.Tables.Add(dtHoldParams);
            //执行释放批次。
            this._entity.LotRelease(dsParams);
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
        /// <summary>
        /// 处理按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcHoldInfoList_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (gvHoldInfoList.IsGroupRow(gvHoldInfoList.FocusedRowHandle)) return;
            if (e.KeyCode == Keys.Enter)
            {
                if (gvHoldInfoList.FocusedRowHandle < gvHoldInfoList.DataRowCount - 1)
                {
                    gvHoldInfoList.FocusedRowHandle += 1;
                }
                else
                {
                    gvHoldInfoList.FocusedRowHandle = 0;
                }
                gvHoldInfoList.ShowEditor();
                e.Handled = true;
            }
        }
    }
}
