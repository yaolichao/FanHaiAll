using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using BarCodePrint;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using FanHai.Hemera.Share.Common;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Utils.Dialogs;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次操作(调整批次)明细的控件类。
    /// </summary>
    public partial class LotOperationAdjust : BaseUserCtrl
    {
        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationAdjust(LotOperationDetailModel model, IViewContent view)
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
        private void LotOperationAdjust_Load(object sender, EventArgs e)
        {
            //this.lblTitle.Text = this._view.TitleName;
            BindLotInfo();
            BindProdId();
            BindCreateType();
            BindLotType();
            BindPriority();
            BindEfficiency();
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
            this.gcList.MainView = this.gvList;
            this.gcList.DataSource = dsReturn.Tables[0];

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
        /// 绑定产品ID数据。
        /// </summary>
        private void BindProdId()
        {
            DataSet ds = _entity.GetProdId();
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                ds.Tables[0].Rows.InsertAt(ds.Tables[0].NewRow(), 0);
                this.lueProId.Properties.DataSource = ds.Tables[0];
                this.lueProId.Properties.DisplayMember = "PRODUCT_CODE";
                this.lueProId.Properties.ValueMember = "PRODUCT_CODE";
            }
            else
            {
                this.lueProId.Properties.DataSource = null;
                this.lueProId.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定创建类别。
        /// </summary>
        private void BindCreateType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Lot_CreateType");
            DataTable dtReturn = BaseData.Get(columns, category);
            dtReturn.Rows.InsertAt(dtReturn.NewRow(), 0);
            this.lueCreateType.Properties.DataSource = dtReturn;
            this.lueCreateType.Properties.DisplayMember = "NAME";
            this.lueCreateType.Properties.ValueMember = "CODE";
            this.lueCreateType.ItemIndex = 0;
        }
        /// <summary>
        /// 绑定批次类型。
        /// </summary>
        private void BindLotType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Lot_Type");
            DataTable dtReturn = BaseData.Get(columns, category);
            dtReturn.Rows.InsertAt(dtReturn.NewRow(),0);
            this.lueLotType.Properties.DataSource = dtReturn;
            this.lueLotType.Properties.DisplayMember = "NAME";
            this.lueLotType.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 绑定优先级。
        /// </summary>
        private void BindPriority()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Lot_Priority");
            DataTable dtReturn = BaseData.Get(columns, category);
            dtReturn.Rows.InsertAt(dtReturn.NewRow(), 0);
            this.luePriority.Properties.DataSource = dtReturn;
            this.luePriority.Properties.DisplayMember = "NAME";
            this.luePriority.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 绑定转换效率。
        /// </summary>
        private void BindEfficiency()
        {
            DataSet ds = _entity.GetEfficiency();
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                ds.Tables[0].Rows.InsertAt(ds.Tables[0].NewRow(), 0);
                this.lueEfficiency.Properties.DataSource = ds.Tables[0];
                this.lueEfficiency.Properties.DisplayMember = "EFFICIENCY_NAME";
                this.lueEfficiency.Properties.ValueMember = "EFFICIENCY_NAME";
            }
            else
            {
                this.lueEfficiency.Properties.DataSource = null;
                this.lueEfficiency.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            //如果是调整批次。
            if (this._model.OperationType == LotOperationType.Adjust)
            {
                this.lciSearchLotNumber.Visibility = LayoutVisibility.Never;
                this.lciCommandAdd.Visibility = LayoutVisibility.Never;
                this.lciCommandRemove.Visibility = LayoutVisibility.Never;
                this.lciList.SizeConstraintsType = SizeConstraintsType.Custom;
                this.lblMenu.Text = "生产管理>组件管理>单件信息修改";
                this.lcgHold.Visibility = LayoutVisibility.Always;
                this.lciList.Visibility = LayoutVisibility.Never;
            }
            //如果是批量调整批次。
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
                this.lblMenu.Text = "生产管理>组件管理>多件信息修改";
                this.lcgHold.Visibility = LayoutVisibility.Never;
            }
            this.teLotNumber.Text = string.Empty;
            this.lueProId.EditValue = string.Empty;
            this.lueCreateType.EditValue = string.Empty;
            this.lueLotType.EditValue = string.Empty;
            this.luePriority.EditValue = string.Empty;
            this.lueEfficiency.EditValue = string.Empty;
            this.teSILot.Text = string.Empty;
            this.beEnterpriseName.Text = string.Empty;
            this.beEnterpriseName.Tag = string.Empty;
            this.teRouteName.Text = string.Empty;
            this.teRouteName.Tag = string.Empty;
            this.teStepName.Text = string.Empty;
            this.teStepName.Tag = string.Empty;
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
        /// 输入待调整批次号后的回车事件。
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
        /// 添加待调整的批次信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtList = this.gcList.DataSource as DataTable;
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
            //已包装，出托后才能调整批次。
            string palletNo = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PALLET_NO]);
            if (!string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage(string.Format("【{0}】已包装，出托后才能调整。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            
            dtList.Merge(dsReturn.Tables[0]);
            //如果要调整的批次中有多个批次信息，则不能调整批次号。
            if (dtList.Rows.Count > 1)
            {
                this.teLotNumber.Text = string.Empty;
                this.lciLotNumber.Visibility = LayoutVisibility.Never;
            }
            this.beLotNumber.SelectAll();
        }
        /// <summary>
        /// 删除待调整的批次信息。
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
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList.Rows.Count <= 1)
            {
                MessageService.ShowMessage("批次信息列表中必须至少有一条记录。", "提示");
                return;
            }
            dtList.Rows.RemoveAt(index);
            //如果要调整的批次中只有一个批次信息，则可以调整批次号。
            if (dtList.Rows.Count <= 1)
            {
                this.teLotNumber.Text = string.Empty;
                this.lciLotNumber.Visibility = LayoutVisibility.Always;
            }
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
            dlg.IsRework = false;
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
        /// 保存，调整批次。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            DataTable dtLotInfo = this.gcList.DataSource as DataTable;
            if (dtLotInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage("待调整批次信息列表至少要有一条记录。", "提示");
                return;
            }
            string newLotNumber = this.teLotNumber.Text.Trim().ToUpper();
            string newProId = Convert.ToString(this.lueProId.EditValue);
            string newCreateType = Convert.ToString(this.lueCreateType.EditValue);
            string newLotType = Convert.ToString(this.lueLotType.EditValue);
            string newPriority = Convert.ToString(this.luePriority.EditValue);
            string newEfficiency = Convert.ToString(this.lueEfficiency.EditValue);
            string newSiLot = this.teSILot.Text.Trim();
            string newEnterpriseName = this.beEnterpriseName.Text;
            string newEnterpriseKey = Convert.ToString(this.beEnterpriseName.Tag);
            string newRouteName = this.teRouteName.Text;
            string newRouteKey = Convert.ToString(this.teRouteName.Tag);
            string newStepName = this.teStepName.Text;
            string newStepKey = Convert.ToString(this.teStepName.Tag);
            string remark = this.teRemark.Text;

            //必须要输入一个调整项目。
            if (string.IsNullOrEmpty(newLotNumber)
                && string.IsNullOrEmpty(newProId)
                && string.IsNullOrEmpty(newCreateType)
                && string.IsNullOrEmpty(newLotType)
                && string.IsNullOrEmpty(newPriority)
                && string.IsNullOrEmpty(newEfficiency)
                && string.IsNullOrEmpty(newSiLot)
                && string.IsNullOrEmpty(newStepKey))
            {
                MessageService.ShowMessage("必须要输入一个调整项目。", "提示");
                return;
            }
            //如果批次号不为空，则判断批次号是否存在。
            if(!string.IsNullOrEmpty(newLotNumber))
            {
                LotQueryEntity queryEntity = new LotQueryEntity();
                DataSet dsReturn = queryEntity.GetLotInfo(newLotNumber);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(queryEntity.ErrorMsg);
                    dsReturn = null;
                    return;
                }
                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    MessageService.ShowMessage(string.Format("【{0}】在数据库中已存在，请确认。", newLotNumber));
                    dsReturn = null;
                    return;
                }
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
            //存放新批次信息。
            Hashtable htMaindata = new Hashtable();
            //存放待调整的批次的操作数据
            WIP_TRANSACTION_FIELDS transFields = new WIP_TRANSACTION_FIELDS();
            DataTable dtTransaction = CommonUtils.CreateDataTable(transFields);
            //存放调整操作的明细记录。
            WIP_COMMENT_FIELDS commentFileds = new WIP_COMMENT_FIELDS();
            DataTable dtComment = CommonUtils.CreateDataTable(commentFileds);
            StringBuilder afterContent = new StringBuilder();
            foreach (DataRow dr in dtLotInfo.Rows)
            {
                //组织待调整的批次的操作数据
                DataRow drTransaction = dtTransaction.NewRow();
                dtTransaction.Rows.Add(drTransaction);
                string transKey = CommonUtils.GenerateNewKey(0);
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = dr[POR_LOT_FIELDS.FIELD_LOT_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_ADJUST;
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
                //组织待调整的批次的明细操作数据
                DataRow drComment = dtComment.NewRow();
                dtComment.Rows.Add(drComment);
                drComment[WIP_COMMENT_FIELDS.FIELDS_TRANSACTION_KEY] = transKey;
                drComment[WIP_COMMENT_FIELDS.FIELDS_ENTERPRISE_KEY] = dr[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY];
                drComment[WIP_COMMENT_FIELDS.FIELDS_ROUTE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY];
                drComment[WIP_COMMENT_FIELDS.FIELDS_STEP_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY];
                drComment[WIP_COMMENT_FIELDS.FIELDS_EDIT_TIMEZONE] = timezone;
                drComment[WIP_COMMENT_FIELDS.FIELDS_EDIT_TIME] = DBNull.Value;
                drComment[WIP_COMMENT_FIELDS.FIELDS_EDITOR] = this._model.UserName;
                StringBuilder beforeContent = new StringBuilder();
                //新批次号不为空。
                if (!string.IsNullOrEmpty(newLotNumber))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, newLotNumber);
                        afterContent.AppendFormat("批次号:{0};", newLotNumber);
                    }
                    beforeContent.AppendFormat("批次号:{0};", dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                }
                //新产品ID号不为空
                if (!string.IsNullOrEmpty(newProId))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_PRO_ID, newProId);
                        afterContent.AppendFormat("产品ID号:{0};", newProId);
                    }
                    beforeContent.AppendFormat("产品ID号:{0};", dr[POR_LOT_FIELDS.FIELD_PRO_ID]);
                }
                //新的创建类别。
                if (!string.IsNullOrEmpty(newCreateType))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TYPE))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_CREATE_TYPE, newCreateType);
                        afterContent.AppendFormat("创建类别:{0};", newCreateType);
                    }
                    beforeContent.AppendFormat("创建类别:{0};", dr[POR_LOT_FIELDS.FIELD_CREATE_TYPE]);
                }
                //新的批次类别。
                if (!string.IsNullOrEmpty(newLotType))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_TYPE))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, newLotType);
                        afterContent.AppendFormat("批次类别:{0};", newLotType);
                    }
                    beforeContent.AppendFormat("批次类别:{0};", dr[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
                }
                //新的优先级。
                if (!string.IsNullOrEmpty(newPriority))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_PRIORITY))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_PRIORITY, newPriority);
                        afterContent.AppendFormat("优先级:{0};", newPriority);
                    }
                    beforeContent.AppendFormat("优先级:{0};", dr[POR_LOT_FIELDS.FIELD_PRIORITY]);
                }
                //新的转换效率。
                if (!string.IsNullOrEmpty(newEfficiency))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_EFFICIENCY))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_EFFICIENCY, newEfficiency);
                        afterContent.AppendFormat("转换效率:{0};", newEfficiency);
                    }
                    beforeContent.AppendFormat("转换效率:{0};", dr[POR_LOT_FIELDS.FIELD_EFFICIENCY]);
                }
                //新的硅片供应商。
                if (!string.IsNullOrEmpty(newSiLot))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_SI_LOT))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_SI_LOT, newSiLot);
                        afterContent.AppendFormat("硅片供应商:{0};", newSiLot);
                    }
                    beforeContent.AppendFormat("硅片供应商:{0};", dr[POR_LOT_FIELDS.FIELD_SI_LOT]);
                }
                //新的工步主键。
                if (!string.IsNullOrEmpty(newStepKey))
                {
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, newEnterpriseKey);
                        afterContent.AppendFormat("工艺流程组:{0};", newEnterpriseName);
                    }
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY, newRouteKey);
                        afterContent.AppendFormat("工艺流程:{0};", newRouteName);
                    }
                    if (!htMaindata.ContainsKey(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY))
                    {
                        htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY, newStepKey);
                        afterContent.AppendFormat("工序:{0};", newStepName);
                    }
                    drComment[WIP_COMMENT_FIELDS.FIELDS_ENTERPRISE_KEY] = newEnterpriseKey;
                    drComment[WIP_COMMENT_FIELDS.FIELDS_ROUTE_KEY] = newRouteKey;
                    drComment[WIP_COMMENT_FIELDS.FIELDS_STEP_KEY] = newStepKey;
                    beforeContent.AppendFormat("工艺流程组:{0};", dr[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    beforeContent.AppendFormat("工艺流程:{0};", dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    beforeContent.AppendFormat("工序:{0};", dr[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                }
                drComment[WIP_COMMENT_FIELDS.FIELDS_BEFORE_CONTENT] = beforeContent;
                drComment[WIP_COMMENT_FIELDS.FIELDS_AFTER_CONTENT] = afterContent;
            }
            DataTable dtMaindata = CommonUtils.ParseToDataTable(htMaindata);
            dtMaindata.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtMaindata);
            dsParams.Tables.Add(dtTransaction);
            dsParams.Tables.Add(dtComment);
            //执行调整批次。
            this._entity.LotAdjust(dsParams);
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
            dtComment = null;
            dsParams = null;
        }
        
    }
}
