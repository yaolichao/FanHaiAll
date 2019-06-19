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
using FanHai.Hemera.Utils.Dialogs;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次操作(批次返修)明细的控件类。
    /// </summary>
    public partial class LotOperationRework : BaseUserCtrl
    {
        /// <summary>
        /// 临时存放释放批次密码的字段名称。
        /// </summary>
        private const string TEMP_HOLD_LIST_RELEASE_PASSWORD = "RELEASE_PASSWORD";

        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        private LotQueryEntity _queryEntity;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationRework(LotOperationDetailModel model, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;
            this._queryEntity = new LotQueryEntity();
            GridViewHelper.SetGridView(gvList);
            GridViewHelper.SetGridView(gvHoldInfoList);
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotOperationRework_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindShiftName();
            this.teUserNumber.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.teUserNumber.Enabled = false;
            this.beLotNumber.Select();

            //BindLotInfo();
            //BindHoldInfo();
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
            //如果是返工批次。
            if (this._model.OperationType == LotOperationType.Rework)
            {
                this.lblMenu.Text = "质量管理>不良品处理>单件返修";
                this.lciSearchLotNumber.Visibility = LayoutVisibility.Never;
                this.lciCommandAdd.Visibility = LayoutVisibility.Never;
                this.lciCommandRemove.Visibility = LayoutVisibility.Never;
                this.lciList.SizeConstraintsType = SizeConstraintsType.Default;
                this.lciHoldInfoList.SizeConstraintsType = SizeConstraintsType.Default;
            }
            //如果是批量返工批次。
            else
            {
                this.lblMenu.Text = "质量管理>不良品处理>多件返修";
                this.lciSearchLotNumber.Visibility = LayoutVisibility.Always;
                this.lciCommandAdd.Visibility = LayoutVisibility.Always;
                this.lciCommandAdd.SizeConstraintsType = SizeConstraintsType.Custom;
                this.lciCommandAdd.MaxSize = new Size(70, 0);
                this.lciCommandRemove.Visibility = LayoutVisibility.Always;
                this.lciCommandRemove.SizeConstraintsType = SizeConstraintsType.Custom;
                this.lciCommandRemove.MaxSize = new Size(70, 0);
                this.lciList.SizeConstraintsType = SizeConstraintsType.Default;
                this.lciHoldInfoList.SizeConstraintsType = SizeConstraintsType.Default;
            }
            this.beLotNumber.Text = string.Empty;
            this.teRemark.Text = string.Empty;
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCancle_Click(object sender, EventArgs e)
        {
            //WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            ////遍历工作台中已经打开的视图对象。
            //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //{
            //    //如果已打开创建批次的视图，则选中该视图显示，返回以结束该方法的运行。
            //    if (viewContent is LotOperation && viewContent.TitleName==this.lblMenu.Name)
            //    {
            //        viewContent.WorkbenchWindow.SelectWindow();
            //        return;
            //    }
            //}
            ////重新打开批次创建视图。
            //LotOperationViewContent view = new LotOperationViewContent(this._model.OperationType);
            //WorkbenchSingleton.Workbench.ShowView(view);

            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Rework)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            LotOperationDetailModel model = new LotOperationDetailModel();
            if (_model.OperationType == LotOperationType.Rework)
            {
                model.OperationType = LotOperationType.Rework;
            }
            else
            {
                if (_model.OperationType == LotOperationType.BatchRework)
                {
                    model.OperationType = LotOperationType.BatchRework;
                }
            }
            //显示电池片操作明细界面。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);
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
        /// 添加待返工的批次信息。
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
            if (dtList == null)
            {
                MessageService.ShowMessage(string.Format("【{0}】在列表中已存在，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            int count = dtList.AsEnumerable()
                              .Count(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]) == lotNumber);
            if (count > 0)
            {
                MessageService.ShowMessage(string.Format("【{0}】在列表中已存在，请确认。", lotNumber), "提示");
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
            if (this._model.RoomKey != currentRoomKey)
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
            //已包装，出托后才能返修。
            string palletNo = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PALLET_NO]);
            if (!string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage(string.Format("【{0}】已包装，出托后才能返修。", lotNumber), "提示");
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
        /// 删除待返工的批次信息。
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
            string lotKey = Convert.ToString(dtList.Rows[index][POR_LOT_FIELDS.FIELD_LOT_KEY]);
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
        /// 选择新的工艺流程。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beEnterpriseName_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OperationHelpDialog dlg = new OperationHelpDialog();
            dlg.FactoryRoom = this._model.RoomName;
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = beEnterpriseName;
            dlg.RouteName = teRouteName;
            dlg.StepName = teStepName;
            dlg.IsRework = true;
            Point i = teStepName.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + teStepName.Height);

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
                    dlg.Location = new Point(i.X + teStepName.Width - dlg.Width, i.Y + teStepName.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + teStepName.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
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
                DateTime holdTime = Convert.ToDateTime(e.CellValue);
                e.DisplayText = holdTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            if (e.Column == this.gclPassword)
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
        /// <summary>
        /// 保存，返修批次。
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
            //待返工批次信息
            DataTable dtLotInfo = this.gcLotList.DataSource as DataTable;
            if (dtLotInfo == null || dtLotInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage("待返工批次信息列表至少要有一条记录。", "提示");
                return;
            }
            //暂停信息
            DataTable dtHoldInfo = this.gcHoldInfoList.DataSource as DataTable;
            if (dtHoldInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage("暂停信息列表至少要有一条记录。", "提示");
                return;
            }
            string newEnterpriseName = this.beEnterpriseName.Text;
            string newEnterpriseKey = Convert.ToString(this.beEnterpriseName.Tag);
            string newRouteName = this.teRouteName.Text;
            string newRouteKey = Convert.ToString(this.teRouteName.Tag);
            string newStepName = this.teStepName.Text;
            string newStepKey = Convert.ToString(this.teStepName.Tag);
            //返工工艺流程必须选择
            if (string.IsNullOrEmpty(newStepKey))
            {
                MessageService.ShowMessage("请选择工艺流程。", "提示");
                this.beEnterpriseName_ButtonClick(this.teStepName, new ButtonPressedEventArgs(new EditorButton()));
                return;
            }
            string remark = this.teRemark.Text.Trim();
            if (string.IsNullOrEmpty(remark))
            {
                MessageService.ShowMessage("备注必须输入。", "提示");
                this.teRemark.Select();
                return;
            }

            StringBuilder afterContent = new StringBuilder();
            afterContent.AppendFormat("工艺流程组:{0};", newEnterpriseName);
            afterContent.AppendFormat("工艺流程:{0};", newRouteName);
            afterContent.AppendFormat("工序:{0};", newStepName);
            //密码和释放密码不一致
            var lnq = from item in dtHoldInfo.AsEnumerable()
                      where Convert.ToString(item[TEMP_HOLD_LIST_RELEASE_PASSWORD]) != Convert.ToString(item[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD])
                      select item;
            foreach (var item in lnq)
            {
                MessageService.ShowMessage("释放密码和暂停密码不一致，请确认。", "提示");
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
            //存放批次返工信息。
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, newEnterpriseKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY, newRouteKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY, newStepKey);
            DataTable dtMaindata = CommonUtils.ParseToDataTable(htMaindata);
            dtMaindata.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            //存放待返工的批次的操作数据
            WIP_TRANSACTION_FIELDS transFields = new WIP_TRANSACTION_FIELDS();
            DataTable dtTransaction = CommonUtils.CreateDataTable(transFields);
            //存在批次返工操作的明细记录。
            WIP_COMMENT_FIELDS commentFileds = new WIP_COMMENT_FIELDS();
            DataTable dtComment = CommonUtils.CreateDataTable(commentFileds);
            foreach (DataRow dr in dtLotInfo.Rows)
            {
                string transKey = CommonUtils.GenerateNewKey(0);
                string lotKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                //组织待返工的批次的操作数据
                DataRow drTransaction = dtTransaction.NewRow();
                dtTransaction.Rows.Add(drTransaction);
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = lotKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_REWORK;
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
                //组织新的工艺流程数据
                DataRow drComment = dtComment.NewRow();
                dtComment.Rows.Add(drComment);
                drComment[WIP_COMMENT_FIELDS.FIELDS_TRANSACTION_KEY] = transKey;
                drComment[WIP_COMMENT_FIELDS.FIELDS_EDIT_TIMEZONE] = timezone;
                drComment[WIP_COMMENT_FIELDS.FIELDS_EDIT_TIME] = DBNull.Value;
                drComment[WIP_COMMENT_FIELDS.FIELDS_EDITOR] = this._model.UserName;
                drComment[WIP_COMMENT_FIELDS.FIELDS_ENTERPRISE_KEY] = newEnterpriseKey;
                drComment[WIP_COMMENT_FIELDS.FIELDS_ROUTE_KEY] = newRouteKey;
                drComment[WIP_COMMENT_FIELDS.FIELDS_STEP_KEY] = newStepKey;
                StringBuilder beforeContent = new StringBuilder();
                beforeContent.AppendFormat("工艺流程组:{0};", dr[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                beforeContent.AppendFormat("工艺流程:{0};", dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                beforeContent.AppendFormat("工序:{0};", dr[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                drComment[WIP_COMMENT_FIELDS.FIELDS_BEFORE_CONTENT] = beforeContent;
                drComment[WIP_COMMENT_FIELDS.FIELDS_AFTER_CONTENT] = afterContent;
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
            //存放暂停明细。
            DataTable dtHoldParams = dtHoldInfo.Copy();
            dtHoldParams.Columns.Remove(TEMP_HOLD_LIST_RELEASE_PASSWORD);     //移除临时列。
            dtHoldParams.Columns.Remove(POR_LOT_FIELDS.FIELD_LOT_KEY);        //移除批次主键列。
            dtHoldParams.Columns.Remove(POR_LOT_FIELDS.FIELD_LOT_NUMBER);     //移除批次号列。

            dsParams.Tables.Add(dtMaindata);
            dsParams.Tables.Add(dtTransaction);
            dsParams.Tables.Add(dtHoldParams);
            dsParams.Tables.Add(dtComment);
            //执行(返工/返修)批次。
            this._entity.LotRework(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                this.tsbCancle_Click(sender, e);
            }
            dsParams.Tables.Clear();
            dtTransaction = null;
            dtHoldParams = null;
            dtComment = null;
            dsParams = null;
        }

        private void gvList_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvHoldInfoList_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            //绑定工厂车间数据到窗体控件。
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用领料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }

        /// <summary>
        /// 绑定班别数据。
        /// </summary>
        private void BindShiftName()
        {
            //获取当前班别名称。
            Shift _shift = new Shift();
            string defaultShift = _shift.GetCurrShiftName();

            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            //获取班别代码
            DataTable shiftTable = BaseData.Get(columns, category);
            //获取班别代码成功。
            if (null != shiftTable && shiftTable.Rows.Count > 0)
            {
                this.lueShift.Properties.DataSource = shiftTable;
                this.lueShift.Properties.DisplayMember = "CODE";
                this.lueShift.Properties.ValueMember = "CODE";
                this.lueShift.EditValue = defaultShift;
            }
        }

        private void btnLotNumber_ButtonClick(object sender, ButtonPressedEventArgs e)
        {

            LotQueryHelpModel model = new LotQueryHelpModel();
            model.RoomKey = Convert.ToString(this.lueFactoryRoom.EditValue);

            model.OperationType = LotOperationType.Scrap;

            LotQueryHelpDialog dlg = new LotQueryHelpDialog(model);
            dlg.OnValueSelected += new LotQueryValueSelectedEventHandler(btnLotQueryHelpDialog_OnValueSelected);
            Point i = btnLotNumber.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = btnLotNumber.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + btnLotNumber.Height);

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
                    dlg.Location = new Point(i.X + btnLotNumber.Width - dlg.Width, i.Y + btnLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + btnLotNumber.Width - dlg.Width, i.Y - dlg.Height);
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
        private void btnLotQueryHelpDialog_OnValueSelected(object sender, LotQueryValueSelectedEventArgs args)
        {
            this.btnLotNumber.Text = args.LotNumber;
        }
        private void btnLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (Operate())
                {
                    BindLotInfo();
                    BindHoldInfo();
                    ResetControlValue();
                }
            }
        }

        /// <summary>
        /// 进行批次操作。
        /// </summary>
        private bool Operate()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string roomName = this.lueFactoryRoom.Text.Trim();
            string lotNumber = this.btnLotNumber.Text.Trim();
            //车间没有选择，给出提示。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                //MessageService.ShowMessage("车间名称不能为空","提示");
                this.lueFactoryRoom.Select();
                return false; ;
            }
            //批号没有输入，给出提示。
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg008}"), MESSAGEBOX_CAPTION);//序列号不能为空
                //MessageService.ShowMessage("序列号不能为空","提示");
                this.btnLotNumber.SelectAll();
                return false;
            }

            DataSet dsLot = this._queryEntity.GetLotInfo(lotNumber);
            if (!string.IsNullOrEmpty(this._queryEntity.ErrorMsg))
            {
                MessageService.ShowMessage(this._queryEntity.ErrorMsg, "提示");
                return false;
            }
            if (dsLot == null || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg010}"), MESSAGEBOX_CAPTION);//序列号不存在
                //MessageService.ShowMessage("序列号不存在。", "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            DataRow drLotInfo = dsLot.Tables[0].Rows[0];
            //判断批次号在指定车间中是否存在。
            string currentRoomKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            if (roomKey != currentRoomKey)
            {
                MessageService.ShowMessage(string.Format("【{0}】在当前车间中不存在，请确认。", lotNumber), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            //判断批次是否被暂停
            int holdFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            if (holdFlag == 1 && this._model.OperationType != LotOperationType.BatchRelease
                && this._model.OperationType != LotOperationType.Release
                && this._model.OperationType != LotOperationType.Rework
                && this._model.OperationType != LotOperationType.BatchRework)

            {
                MessageService.ShowMessage(string.Format("【{0}】已被暂停，请确认。", lotNumber), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            if (holdFlag == 0 && (this._model.OperationType == LotOperationType.BatchRelease
                               || this._model.OperationType == LotOperationType.Release
                               || this._model.OperationType == LotOperationType.Rework
                               || this._model.OperationType == LotOperationType.BatchRework))

            {
                MessageService.ShowMessage(string.Format("【{0}】未暂停，请确认。", lotNumber), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            //判断批次是否被暂停
            string lotType = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
            if (lotType == "N" && (this._model.OperationType == LotOperationType.CellPatch
                               || this._model.OperationType == LotOperationType.CellRecovered))
            {
                MessageService.ShowMessage(string.Format("批次必须是组件补片批次，请确认。"), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            //判断批次是否被删除
            int deleteFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            if (deleteFlag == 1)
            {
                MessageService.ShowMessage(string.Format("【{0}】已结束，请确认。", lotNumber), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            //判断批次是否已结束
            if (deleteFlag == 2)
            {
                MessageService.ShowMessage(string.Format("【{0}】已删除，请确认。", lotNumber), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }
            //判断批次是否完成。
            int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            if (stateFlag >= 10)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.Msg001}"));//批次已完成，请确认
                //MessageBox.Show("批次已完成，请确认。");
                this.btnLotNumber.SelectAll();
                return false;
            }
            string palletNo = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            //如果是返修、不良、报废、批次退料、调整批次、拆分批次、合并批次或者终结批次操作。
            //批次不能被包装才能进行对应操作。


            if ((this._model.OperationType == LotOperationType.Scrap
                || this._model.OperationType == LotOperationType.Defect
                || this._model.OperationType == LotOperationType.BatchRework
                || this._model.OperationType == LotOperationType.ReturnMaterial
                || this._model.OperationType == LotOperationType.Rework
                || this._model.OperationType == LotOperationType.Adjust
                || this._model.OperationType == LotOperationType.BatchAdjust
                || this._model.OperationType == LotOperationType.Terminal
                || this._model.OperationType == LotOperationType.Split
                || this._model.OperationType == LotOperationType.Merge)
                && !string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage(string.Format("【{0}】已包装，出托后才能进行对应操作。", lotNumber), "提示");
                this.btnLotNumber.SelectAll();
                return false;
            }

            //LotOperationDetailModel model = new LotOperationDetailModel();
            //this._model.OperationType = LotOperationType.Scrap;
            this._model.LotNumber = lotNumber;
            this._model.RoomKey = roomKey;
            this._model.RoomName = roomName;
            this._model.ShiftName = Convert.ToString(this.lueShift.EditValue);
            this._model.UserName = this.teUserNumber.Text;
            this._model.LotEditTime = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME]);
            //model.TitleName = this.lblApplicationTitle.Text;

            //显示结束批次对话框。
            //if (this._model.OperationType == LotOperationType.Terminal)
            //{
            //    TerminalLotDialog terminalLot = new TerminalLotDialog(model);
            //    //显示结束批次的对话框。
            //    terminalLot.ShowDialog();
            //}
            //else
            //{
            ////显示电池片操作明细界面。
            //WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            ////创建新的视图并显示
            //LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            //WorkbenchSingleton.Workbench.ShowView(view);
            //}
            return true;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.btnLotNumber.Text = string.Empty;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate())
                {
                    BindLotInfo();
                    BindHoldInfo();
                    ResetControlValue();
                }
            }
            finally
            {
                this.btnLotNumber.Select();
                this.btnLotNumber.SelectAll();
            }
        }
    }
}
