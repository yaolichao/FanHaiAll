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
using DevExpress.XtraEditors.Mask;
using FanHai.Hemera.Utils.Dialogs;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Text.RegularExpressions;
using DevExpress.XtraLayout.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示创建批次的控件类。
    /// </summary>
    public partial class LotCreateDetail : BaseUserCtrl
    {
        LotCreateEntity _entity;

        LineSettingEntity _lineSettingEntity = new LineSettingEntity();

        LotCreateDetailModel _model;                    //批次创建的参数数据。
        bool _isBatch = false;
        IViewContent _view = null;
        private int gvRowNumber = -1;
        /// <summary>
        /// 是否检查电池片信息，如果电池片信息没有输入，则在出站时必须输入电池片信息。
        /// </summary>
        bool _isCheckSILot = false;
        /// <summary>
        /// 是否检查电池片转换效率是否和领料转换效率一致。
        /// </summary>
        bool _isCheckEfficiency = false;
        /// <summary>
        /// 是否检查组件线别绑定。
        /// </summary>
        bool _isCheckLotLine = false;

        /// <summary>
        /// MessageBox Title
        /// </summary>
        private const string MESSAGEBOX_CAPTION = "提示";

        private static DataSet dsWorkOrderBom;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotCreateDetail(LotCreateDetailModel model, bool isBatch, IViewContent view)
        {
            InitializeComponent();
            this._entity = new LotCreateEntity();
            this._model = model;
            this._isBatch = isBatch;
            this._view = view;
        }
        /// <summary>
        /// 设置工序自定义属性值。
        /// </summary>
        private void SetOperationAtrributeValue()
        {
            if (string.IsNullOrEmpty(this._model.OperationName)) return;
            RouteQueryEntity routeEntity = new RouteQueryEntity();
            DataSet dsReturn = routeEntity.GetMaxVersionOperationAttrInfo(this._model.OperationName, ROUTE_OPERATION_ATTRIBUTE.IsCheckSILot);
            if (string.IsNullOrEmpty(routeEntity.ErrorMsg)
                && dsReturn.Tables.Count > 0
                && dsReturn.Tables[0].Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> rowCollection = dsReturn.Tables[0].AsEnumerable();
                DataRow drCurrent = null;
                //是否检查电池片信息。如果电池片信息没有输入，则在出站时必须输入电池片信息。
                drCurrent = rowCollection
                                .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                                == ROUTE_OPERATION_ATTRIBUTE.IsCheckSILot)
                                .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckSILot))
                    {
                        this._isCheckSILot = false;
                    }
                }

            }

            dsReturn = routeEntity.GetMaxVersionOperationAttrInfo(this._model.OperationName, ROUTE_OPERATION_ATTRIBUTE.IsCheckEfficiency);
            if (string.IsNullOrEmpty(routeEntity.ErrorMsg)
                && dsReturn.Tables.Count > 0
                && dsReturn.Tables[0].Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> rowCollection = dsReturn.Tables[0].AsEnumerable();
                DataRow drCurrent = null;

                drCurrent = rowCollection
                                .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                                == ROUTE_OPERATION_ATTRIBUTE.IsCheckEfficiency)
                                .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckEfficiency))
                    {
                        this._isCheckEfficiency = false;
                    }
                }

            }
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotCreateDetail_Load(object sender, EventArgs e)
        {
            BindMaterialInfo();
            BindBaseInfo();
            BindLotInfo();
            BindLotList();
            InitControlValue();
            SetOperationAtrributeValue();
            BindWorkOrderBom();
            if (this._isBatch)
            {
                this.lblMenu.Text = "生产管理>过站管理>组件创建";   //视图标题。
            }
            else
            {
                this.lblMenu.Text = "生产管理>过站管理>组件补片";   //视图标题。
            }
        }
        /// <summary>
        /// 绑定领料单信息。
        /// </summary>
        private void BindMaterialInfo()
        {
            DataSet dsReturn = this._entity.GetReceiveMaterialInfo(this._model.StoreMaterialDetailKey);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg, "提示");
                return;
            }
            this.gcMaterialLotList.MainView = this.gvMaterialLotList;
            this.gcMaterialLotList.DataSource = dsReturn.Tables[0];
            this.txtMaterialLot.Text = dsReturn.Tables[0].Rows[0]["MATERIAL_LOT"].ToString();
            this.txtQty.Text = dsReturn.Tables[0].Rows[0]["RECEIVE_QTY"].ToString();
            this.txtEfficiency.Text = dsReturn.Tables[0].Rows[0]["EFFICIENCY"].ToString();
            this.txtWorkorderNo.Text = dsReturn.Tables[0].Rows[0]["AUFNR"].ToString();
            this.txtProductID.Text = dsReturn.Tables[0].Rows[0]["PRO_ID"].ToString();
            this.txtSupplierName.Text = dsReturn.Tables[0].Rows[0]["SUPPLIER_NAME"].ToString();
        }
        /// <summary>
        /// 绑定基本信息。
        /// </summary>
        private void BindBaseInfo()
        {
            this.txtWorkOrder.Text = this._model.OrderNo;
            this.txtProId.Text = this._model.ProId;
            this.txtCreateType.Text = this._model.CreateTypeName;
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            BindLotType();
            BindPriority();
            checkLotLine();
        }
        /// <summary>
        /// 绑定批次类型。
        /// </summary>
        private void BindLotType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            this.lueLotType.Properties.DataSource = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Lot_Type);
            this.lueLotType.Properties.DisplayMember = "NAME";
            this.lueLotType.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 绑定优先级。
        /// </summary>
        private void BindPriority()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            this.luePriority.Properties.DataSource = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Lot_Priority);
            this.luePriority.Properties.DisplayMember = "NAME";
            this.luePriority.Properties.ValueMember = "CODE";
        }

        /// <summary>
        /// 获取工单BOM
        /// </summary>
        private void BindWorkOrderBom()
        {
            string strTxtWorkOrder = string.Empty;
            strTxtWorkOrder = txtWorkOrder.Text.Trim();

            LotCreateEntity lotCreatEntity = new LotCreateEntity();
            dsWorkOrderBom = lotCreatEntity.GetWorkOrderBom(strTxtWorkOrder);
        }

        /// <summary>
        /// 检查批次线别卡控。
        /// </summary>
        private void checkLotLine()
        {

            string[] columns = new string[] { "FactoryName", "IsCheckLotLine" };
            DataRow[] drs = BaseData.Get(columns, "Lot_Line").Select(string.Format("FactoryName = '{0}'", _model.RoomName));

            if (drs.Length == 1)
            {
                if (bool.TryParse(drs[0][LOT_LINE.IsCheckLotLine].ToString(), out this._isCheckLotLine))
                {
                    _isCheckLotLine = bool.Parse(drs[0][LOT_LINE.IsCheckLotLine].ToString());
                }
            }

            if (_isCheckLotLine)
            {
                lciLotLineInfo.Visibility = LayoutVisibility.Always;
                //绑定控件线别信息
                BindSubLineList();
            }
            else
            {
                lciLotLineInfo.Visibility = LayoutVisibility.Never;
            }

        }

        /// <summary>
        /// 线别下拉绑定
        /// </summary>
        private void BindSubLineList()
        {
            string userName = string.Empty;

            DataSet dsSubLine = null;

            try
            {
                userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);

                dsSubLine = _lineSettingEntity.GetLineByUserNameAndLineName(userName, string.Empty);

                if (string.IsNullOrEmpty(_lineSettingEntity.ErrorMsg))
                {
                    //绑定子线下拉数据

                    DataTable dtSubLineBind = dsSubLine.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];

                    lueLotLineInfo.Properties.DisplayMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME;
                    lueLotLineInfo.Properties.ValueMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY;
                    lueLotLineInfo.Properties.DataSource = dtSubLineBind;
                }

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        /// <summary>
        /// 绑定批次列表。
        /// </summary>
        private void BindLotList()
        {
            DataTable dtNewLot = new DataTable();
            dtNewLot.Columns.Add("INDEX", typeof(int));
            dtNewLot.Columns.Add("LOT_NUMBER", typeof(string));
            dtNewLot.Columns.Add("CELLLOT1", typeof(string));
            dtNewLot.Columns.Add("CELLPN1", typeof(string));
            dtNewLot.Columns.Add("CELLLOT2", typeof(string));
            dtNewLot.Columns.Add("CELLPN2", typeof(string));
            dtNewLot.Columns.Add("PACKAGEQTY", typeof(decimal));
            dtNewLot.Columns.Add("CELLSUPPLIER", typeof(string));
            dtNewLot.Columns.Add("CELLFACTORY", typeof(string));
            dtNewLot.Columns.Add("CELLLINE", typeof(string));
            dtNewLot.Columns.Add("CELLCOLOR", typeof(string));
            dtNewLot.Columns.Add("CELLEFFICIENCY", typeof(string));
            dtNewLot.Columns.Add("CELLPOWER", typeof(decimal));
            dtNewLot.Columns.Add("SI_SUPPLIER_LOT", typeof(string));
            dtNewLot.Columns.Add("SMALL_PACK_NUMBER", typeof(string));
            dtNewLot.Columns.Add("CREATOR", typeof(string));
            dtNewLot.Columns.Add("CREATE_TIME", typeof(DateTime));
            dtNewLot.Columns.Add("CREATE_TIMEZONE_KEY", typeof(string));
            dtNewLot.Columns.Add("EDITOR", typeof(string));
            dtNewLot.Columns.Add("EDIT_TIME", typeof(DateTime));
            dtNewLot.Columns.Add("EDIT_TIMEZONE", typeof(string));
            dtNewLot.Columns.Add("ETL_FLAG", typeof(string));
            dtNewLot.Columns.Add("CHECK_SMALL_PACK_NUMBER", typeof(string));
            dtNewLot.Columns.Add("MATERIAL_TIME", typeof(string));

            for (int i = 0; i < this._model.Count; i++)
            {
                DataRow dr = dtNewLot.NewRow();

                dr["PACKAGEQTY"] = 0;
                dr["CELLPOWER"] = 0;
                dr["INDEX"] = i + 1;

                dtNewLot.Rows.Add(dr);
            }
            this.gcNewLot.MainView = this.gvNewLot;
            this.gcNewLot.DataSource = dtNewLot;
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            //批量创建批次。
            if (this._isBatch)
            {
                this.lueLotType.EditValue = "N";//生产批次
                RouteQueryEntity routeQueryEntity = new RouteQueryEntity();
                DataSet dsRouteFirstOperation = routeQueryEntity.GetProcessPlanFirstOperation(this._model.RoomName, string.Empty, false);
                //有获取到首工序工艺流程。
                if (string.IsNullOrEmpty(routeQueryEntity.ErrorMsg)
                    && null != dsRouteFirstOperation
                    && dsRouteFirstOperation.Tables.Count > 0
                    && dsRouteFirstOperation.Tables[0].Rows.Count > 0)
                {
                    WorkOrders workOrdersEntity = new WorkOrders();
                    DataSet dsReturn = workOrdersEntity.GetWorkOrderRouteInfo(_model.OrderNo);

                    DataRow drRouteFirstOperation = null;

                    if (dsReturn.Tables[0].Rows.Count == 1)
                    {
                        drRouteFirstOperation = dsReturn.Tables[0].Rows[0];
                    }
                    else
                    {
                        drRouteFirstOperation = dsRouteFirstOperation.Tables[0].Rows[0];
                    }

                    this.beRouteEnterprise.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                    this.beRouteEnterprise.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    this.teRouteName.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                    this.teRouteName.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    this.teStepName.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                    this.teStepName.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                }
                else
                {
                    this.beRouteEnterprise.Tag = string.Empty;
                    this.beRouteEnterprise.Text = string.Empty;
                    this.teRouteName.Tag = string.Empty;
                    this.teRouteName.Text = string.Empty;
                    this.teStepName.Tag = string.Empty;
                    this.teStepName.Text = string.Empty;
                }
            }
            else//非批量创建批次。
            {
                this.lueLotType.EditValue = "L";//组件补片批次。
                this.beRouteEnterprise.Tag = string.Empty;
                this.beRouteEnterprise.Text = string.Empty;
                this.teRouteName.Tag = string.Empty;
                this.teRouteName.Text = string.Empty;
                this.teStepName.Tag = string.Empty;
                this.teStepName.Text = string.Empty;
            }
            this.lueLotType.Properties.ReadOnly = true;
            this.luePriority.EditValue = "5"; //默认值
            this.txtMaterialQty.Text = this._entity.GetCellNumber(this._model.ProId).ToString("##0");
            this.teRemark.Text = string.Empty;

            DataTable dtNewLot = this.gcNewLot.DataSource as DataTable;
            foreach (DataRow dr in dtNewLot.Rows)
            {
                dr[1] = string.Empty;
                dr[2] = string.Empty;
            }
            string lotType = Convert.ToString(this.lueLotType.EditValue);
            //非批量创建批次。创建补片批次。
            if (!this._isBatch)
            {
                this.lcgLotList.Text = "批次号信息";
                this.LOT_NUMBER.Caption = "批次号";
            }
            //生产批次，需要限制输入的字符。
            if (lotType == "N")
            {
                this.rteLotNumber.Mask.MaskType = MaskType.RegEx;
                this.rteLotNumber.Mask.EditMask = @"[0-9a-zA-Z.-]{0,50}";
                this.rteLotNumber.Mask.ShowPlaceHolders = false;
            }
            else
            {//组件补片批次。
                this.rteLotNumber.Mask.MaskType = MaskType.RegEx;
                this.rteLotNumber.Mask.EditMask = @"[0-9a-zA-Z.-]{0,50}";
                this.rteLotNumber.Mask.ShowPlaceHolders = false;
            }
            this.gcNewLot.Select();
            this.gvNewLot.FocusedColumn = this.LOT_NUMBER;
            this.gvNewLot.FocusedRowHandle = 0;
            this.gvNewLot.ShowEditor();
            SendKeys.Send(" ");
        }
        /// <summary>
        /// 选择工艺流程。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beOperations_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OperationHelpDialog dlg = new OperationHelpDialog();
            dlg.FactoryRoom = this._model.RoomName;
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = beRouteEnterprise;
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
        /// 重置文本值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            InitControlValue();
        }
        /// <summary>
        /// 关闭按钮的Click事件处理方法。用于关闭当前视图。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开创建批次的视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent is LotCreateViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //重新打开批次创建视图。
            LotCreateViewContent view = new LotCreateViewContent(this._model, this._isBatch);
            WorkbenchSingleton.Workbench.ShowView(view);
        }

        /// <summary>
        /// 确认按钮Click事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            //重置指向的行号
            gvRowNumber = -1;

            if (this.gvNewLot.State == GridState.Editing && this.gvNewLot.IsEditorFocused && this.gvNewLot.EditingValueModified)
            {
                this.gvNewLot.SetFocusedRowCellValue(this.gvNewLot.FocusedColumn, this.gvNewLot.EditingValue);
            }
            this.gvNewLot.UpdateCurrentRow();
            string enterpriseKey = Convert.ToString(this.beRouteEnterprise.Tag);
            string enterpriseName = Convert.ToString(this.beRouteEnterprise.Text);
            string routeKey = Convert.ToString(this.teRouteName.Tag);
            string routeName = Convert.ToString(this.teRouteName.Text);
            string stepKey = Convert.ToString(this.teStepName.Tag);
            string stepName = Convert.ToString(this.teStepName.Text);
            string lotLineKey = Convert.ToString(this.lueLotLineInfo.EditValue);
            string lotLineCode = Convert.ToString(this.lueLotLineInfo.Text);
            string lotType = Convert.ToString(this.lueLotType.EditValue);
            string priority = Convert.ToString(this.luePriority.EditValue);
            string mQty = this.txtMaterialQty.Text.Trim();
            string sProID = this.txtProId.Text.Trim();
            int nSNLongth = 0;
            string sSNLongth = string.Empty;

            if (_isCheckLotLine)
            {
                if (string.IsNullOrEmpty(lotLineKey))
                {
                    MessageService.ShowMessage("请选择要创建的线别！", "提示");
                    this.lueLotLineInfo.Select();
                    return;
                }
            }

            if (string.IsNullOrEmpty(mQty))
            {
                MessageService.ShowMessage("请输入原材料数量。", "提示");
                this.txtMaterialQty.Focus();
                return;
            }
            double materialQty = Convert.ToDouble(mQty);
            string remark = this.teRemark.Text;
            DataTable dtNewLot = this.gcNewLot.DataSource as DataTable;
            DataTable dtMaterialLot = this.gcMaterialLotList.DataSource as DataTable;
            //批次号必须输入。
            var lots = from item in dtNewLot.AsEnumerable()
                       where string.IsNullOrEmpty(Convert.ToString(item["LOT_NUMBER"]))
                       select item["INDEX"];
            foreach (int index in lots)
            {
                MessageService.ShowMessage(string.Format("序号[{0}]中的序列号必须输入。", index), "提示");
                this.gvNewLot.FocusedColumn = this.LOT_NUMBER;
                this.gvNewLot.FocusedRowHandle = index - 1;
                this.gvNewLot.ShowEditor();
                return;
            }
            //领料电池片转换效率是否和小包条码转换效率一致
            if (_isCheckEfficiency)
            {
                lots = from item in dtNewLot.AsEnumerable()
                       where item["CHECK_SMALL_PACK_NUMBER"].ToString() == "N"
                       select item["INDEX"];
                foreach (int index in lots)
                {
                    MessageService.ShowMessage(string.Format("序号[{0}]中的转换效率和领料转换效率不一致，请检查。", index), "提示");
                    this.gvNewLot.FocusedColumn = this.LOT_NUMBER;
                    this.gvNewLot.FocusedRowHandle = index - 1;
                    this.gvNewLot.ShowEditor();
                    return;
                }
            }


            //批次号中如有小写字母给出提示。
            lots = from item in dtNewLot.AsEnumerable()
                   where Regex.IsMatch(Convert.ToString(item["LOT_NUMBER"]), "[a-z]")
                   select item["INDEX"];
            foreach (int index in lots)
            {
                MessageService.ShowMessage(string.Format("序号[{0}]中的序列号包含小写字母，请检查。", index), "提示");
                this.gvNewLot.FocusedColumn = this.LOT_NUMBER;
                this.gvNewLot.FocusedRowHandle = index - 1;
                this.gvNewLot.ShowEditor();
                return;
            }
            //判断批次号在当前列表中是否重复
            var lotGroup = from item in dtNewLot.AsEnumerable()
                           group item by item["LOT_NUMBER"] into lotNumberGroup
                           where lotNumberGroup.Count() > 1
                           select lotNumberGroup.Key;
            foreach (string lotNumber in lotGroup)
            {
                MessageService.ShowMessage(string.Format("序列号[{0}]存在重复值，请确认。", lotNumber), "提示");
                return;
            }
            //判断批次号长度是否正确
            DataSet dsProductMode = _entity.GetProductModeByPID(sProID);
            if (dsProductMode.Tables[0].Rows.Count > 0)
            {
                sSNLongth = dsProductMode.Tables[0].Rows[0]["SN_LONGTH"].ToString().Trim();
            }
            if (!string.IsNullOrEmpty(sSNLongth))
            {
                nSNLongth = Convert.ToInt32(sSNLongth);
            }
            if (lotType == "N" && nSNLongth > 0)
            {
                lots = from item in dtNewLot.AsEnumerable()
                       where item["LOT_NUMBER"].ToString().Length != nSNLongth
                       select item["INDEX"];
                foreach (int index in lots)
                {
                    MessageService.ShowMessage(string.Format("序号[{0}]中的序列号长度不符合设定要求[" + nSNLongth.ToString() + "]，请检查。", index), "提示");
                    this.gvNewLot.FocusedColumn = this.LOT_NUMBER;
                    this.gvNewLot.FocusedRowHandle = index - 1;
                    this.gvNewLot.ShowEditor();
                    return;
                }
            }

            //判断常规工单序列号和工单是否关联
            string orderNumber = this.txtWorkOrder.Text.Trim();
            DataSet dsOrderNoType = _entity.GetOrderNoType(orderNumber);
            if (dsOrderNoType.Tables[0].Rows.Count > 0 || dsOrderNoType.Tables[1].Rows.Count > 0)
            {
                string type = orderNumber.Length > 2 ? orderNumber.Substring(0, 2) : "Normal";
                //获取工单读取规则
                string[] columns = new string[] { "No", "Type", "Wotype", "StringStart", "StringLength", "FlagEnable" };
                List<KeyValuePair<string, string>> lstConditions = new List<KeyValuePair<string, string>>();
                lstConditions.Add(new KeyValuePair<string, string>("Wotype", type));
                lstConditions.Add(new KeyValuePair<string, string>("FlagEnable", "Y"));
                DataTable dtwotype = BaseData.GetBasicDataByCondition(columns, BASEDATA_CATEGORY_NAME.WorkOrderRuleConfig, lstConditions);

                if (dtwotype.Rows.Count == 0 && type != "Normal")
                {
                    lstConditions.Clear();
                    lstConditions.Add(new KeyValuePair<string, string>("Wotype", "Normal"));
                    lstConditions.Add(new KeyValuePair<string, string>("FlagEnable", "Y"));
                    dtwotype = BaseData.GetBasicDataByCondition(columns, BASEDATA_CATEGORY_NAME.WorkOrderRuleConfig, lstConditions);
                }

                if (dtwotype.Rows.Count > 0)
                {
                    string woRule = string.Empty;

                    DataRow[] drsLeft = dtwotype.Select(string.Format(" [No]='1'"));
                    if (drsLeft.Length > 0)
                    {
                        int StringLeftStart = Convert.ToInt32(drsLeft[0]["StringStart"]);
                        int StringLeftLength = Convert.ToInt32(drsLeft[0]["StringLength"]);
                        woRule += orderNumber.Substring(StringLeftStart, StringLeftLength);
                    }

                    DataRow[] drsRight = dtwotype.Select(string.Format("[No]='2'"));
                    if (drsLeft.Length > 0)
                    {
                        int StringRightStart = Convert.ToInt32(drsRight[0]["StringStart"]);
                        int StringRightLength = Convert.ToInt32(drsRight[0]["StringLength"]);
                        woRule += orderNumber.Substring(StringRightStart, StringRightLength);
                    }

                    lots = from item in dtNewLot.AsEnumerable()
                           where Convert.ToString(item["LOT_NUMBER"]).StartsWith(woRule) == false
                           select item["INDEX"];

                    foreach (int index in lots)
                    {
                        MessageService.ShowMessage(string.Format("序号[{0}]中的序列号与工单不符，请检查。", index), "提示");
                        this.gvNewLot.FocusedColumn = this.LOT_NUMBER;
                        this.gvNewLot.FocusedRowHandle = index - 1;
                        this.gvNewLot.ShowEditor();
                        return;
                    }
                }

            }

            if (this._isCheckSILot)
            {
                lots = from item in dtNewLot.AsEnumerable()
                       where string.IsNullOrEmpty(Convert.ToString(item["CELLSUPPLIER"]))
                       select item["INDEX"];
                foreach (int index in lots)
                {
                    MessageService.ShowMessage(string.Format("序号[{0}]中的供应商信息必须输入。", index), "提示");
                    this.gvNewLot.FocusedColumn = this.CELLSUPPLIER;
                    this.gvNewLot.FocusedRowHandle = index - 1;
                    this.gvNewLot.ShowEditor();
                    return;
                }
            }


            WorkOrders workOrder = new WorkOrders();

            DataSet dsOEMReturn = workOrder.GetWorkOrderOEMCustomer(_model.OrderNo);

            if (dsOEMReturn.Tables.Count > 0 && dsOEMReturn.Tables[0].Rows.Count > 0)
            {
                string customer = dsOEMReturn.Tables[0].Rows[0]["CUSROMER"].ToString();

                if (customer == "SE")
                {
                    DataRow[] drs = dtNewLot.Select();
                    foreach (DataRow dr in drs)
                    {
                        if (dr["CELLEFFICIENCY"].ToString() == "")
                        {
                            MessageService.ShowMessage(string.Format("序号[{0}]中的效率信息必须输入。", dr["INDEX"].ToString()), "提示");
                            this.gvNewLot.FocusedColumn = this.CELLEFFICIENCY;
                            this.gvNewLot.FocusedRowHandle = Convert.ToInt32(dr["INDEX"]) - 1;
                            this.gvNewLot.ShowEditor();
                            return;
                        }
                        if (dr["CELLPOWER"].ToString() == "0")
                        {
                            MessageService.ShowMessage(string.Format("序号[{0}]中的电池片功率信息必须输入。", dr["INDEX"].ToString()), "提示");
                            this.gvNewLot.FocusedColumn = this.CELLPOWER;
                            this.gvNewLot.FocusedRowHandle = Convert.ToInt32(dr["INDEX"]) - 1;
                            this.gvNewLot.ShowEditor();
                            return;
                        }
                        if (dr["CELLCOLOR"].ToString() == "")
                        {
                            MessageService.ShowMessage(string.Format("序号[{0}]中的花色信息必须输入。", dr["INDEX"].ToString()), "提示");
                            this.gvNewLot.FocusedColumn = this.CELLCOLOR;
                            this.gvNewLot.FocusedRowHandle = Convert.ToInt32(dr["INDEX"]) - 1;
                            this.gvNewLot.ShowEditor();
                            return;
                        }
                    }

                }
            }

            //原材料剩余数量是否满足创建批次需要
            double materialSumQty = dtNewLot.Rows.Count * materialQty;
            double currentMaterialSumQty = dtMaterialLot.AsEnumerable().Sum(dr => Convert.ToDouble(dr["CURRENT_QTY"]));

            if (currentMaterialSumQty < materialSumQty)
            {
                MessageService.ShowMessage("原材料数量不足，请确认。", "提示");
                return;
            }
            if (string.IsNullOrEmpty(lotType))
            {
                MessageService.ShowMessage("请选择批次类型。", "提示");
                this.lueLotType.Select();
                return;
            }
            //没有选择工艺流程。
            if (lotType == "N" && string.IsNullOrEmpty(stepKey))
            {
                MessageService.ShowMessage("请选择工艺流程。", "提示");
                this.beOperations_ButtonClick(this.beRouteEnterprise, new ButtonPressedEventArgs(new EditorButton()));
                return;
            }
            string efficiency = Convert.ToString(dtMaterialLot.Rows[0]["EFFICIENCY"]);
            string timeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            string computerName = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME, _model.OperationName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CREATE_TIME, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CREATE_TIMEZONE_KEY, timeZone);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CREATE_TYPE, _model.CreateTypeCode);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CREATOR, _model.UserName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY, routeKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY, stepKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_DESCRIPTIONS, remark);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_EDC_INS_KEY, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_EDIT_TIME, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE, timeZone);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_EDITOR, _model.UserName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, _model.RoomKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME, _model.RoomName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT, 1);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_IS_PRINT, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_IS_REWORKED, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_IS_SPLITED, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LINE_NAME, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_SEQ, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, lotType);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_MATERIAL_CODE, _model.MaterialCode);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_MATERIAL_LOT, _model.ReceiveItemNo);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_OPERATOR, _model.UserName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_OPR_COMPUTER, computerName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_OPR_LINE, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_OPR_LINE_PRE, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, _model.PartNumber);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_VER_KEY, _model.PartKey);

            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_LINE_KEY, lotLineKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_LINE_CODE, lotLineCode);

            htMaindata.Add(POR_LOT_FIELDS.FIELD_PRIORITY, priority);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_PRO_ID, _model.ProId);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_QUANTITY, materialQty);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL, materialQty);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, enterpriseKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_SHIFT_NAME, _model.ShiftName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_SI_LOT, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_START_PROCESS_TIME, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_START_WAIT_TIME, null);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG, 0);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_STATUS, 1);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_SUPPLIER_NAME, _model.SupplierName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_EFFICIENCY, efficiency);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY, _model.OrderKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, _model.OrderNo);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ, 0);
            DataTable dtMaindata = CommonUtils.ParseToDataTable(htMaindata);
            dtMaindata.TableName = TRANS_TABLES.TABLE_MAIN_DATA;

            Hashtable htAddtiondata = new Hashtable();
            htAddtiondata.Add("STORE_MATERIAL_DETAIL_KEY", this._model.StoreMaterialDetailKey);
            htAddtiondata.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
            htAddtiondata.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
            htAddtiondata.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
            DataTable dtAddtiondata = CommonUtils.ParseToDataTable(htAddtiondata);
            dtAddtiondata.TableName = TRANS_TABLES.TABLE_PARAM;

            DataSet dsParams = new DataSet();
            dsParams.Tables.Add(dtMaindata);
            dsParams.Tables.Add(dtAddtiondata);

            dsParams.Tables.Add(dtNewLot);
            DataSet dsReturn = _entity.CeateLot(dsParams);
            dsParams.Tables.Clear();
            dsParams = null;
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowMessage("创建批次成功。", "提示");
                this.btnClose_Click(sender, e);
            }
            else
            {
                MessageService.ShowMessage(_entity.ErrorMsg, "错误");
            }

        }
        /// <summary>
        /// 处理按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcNewLot_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (gvNewLot.IsGroupRow(gvNewLot.FocusedRowHandle)) return;
            if (e.KeyCode == Keys.Enter)
            {
                if (ceIsOEM.Checked)
                {
                    if (gvNewLot.FocusedColumn.VisibleIndex < gvNewLot.Columns["CELLCOLOR"].VisibleIndex)
                    {
                        gvNewLot.FocusedColumn = this.gvNewLot.Columns[gvNewLot.FocusedColumn.VisibleIndex + 1];
                        gvNewLot.ShowEditor();
                        e.Handled = true;
                    }
                    else
                    {
                        gvNewLot.FocusedColumn = gvNewLot.Columns["LOT_NUMBER"];

                        if (gvNewLot.FocusedRowHandle < gvNewLot.DataRowCount - 1)
                        {
                            gvNewLot.FocusedRowHandle += 1;
                        }
                        else
                        {
                            gvNewLot.FocusedRowHandle = 0;
                        }

                        e.Handled = true;
                    }
                }
                else
                {
                    if (gvNewLot.FocusedRowHandle < gvNewLot.RowCount - 1)
                    {
                        gvNewLot.FocusedRowHandle += 1;
                        gvNewLot.ShowEditor();
                        e.Handled = true;
                    }
                    else
                    {
                        gvNewLot.FocusedColumn = this.CELLPN1;

                        gvNewLot.FocusedRowHandle = 0;
                        e.Handled = true;
                    }
                }
            }
            else
            {
                if (e.Modifiers == Keys.Control && this.gvNewLot.FocusedColumn != this.LOT_NUMBER && this.gvNewLot.FocusedColumn != this.ROWNUM)
                {
                    if (this.gvNewLot.State == GridState.Editing && this.gvNewLot.IsEditorFocused && this.gvNewLot.EditingValueModified)
                    {
                        this.gvNewLot.SetFocusedRowCellValue(this.gvNewLot.FocusedColumn, this.gvNewLot.EditingValue);
                    }
                    this.gvNewLot.UpdateCurrentRow();
                    this.gvNewLot.HideEditor();
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 控件响应Ctrl+Enter事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs args = new KeyEventArgs(keyData);
            if (args.Control && args.KeyCode == Keys.Enter)
            {
                tsbOK_Click(null, null);
                args.Handled = true;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private GridHitInfo _preHitInfo = null;
        /// <summary>
        /// 开始复制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcNewLot_MouseDown(object sender, MouseEventArgs e)
        {
            this._preHitInfo = null;
            GridHitInfo hitInfo = gvNewLot.CalcHitInfo(new Point(e.X, e.Y));
            //开始复制电池片信息值。
            if (Control.ModifierKeys == Keys.Control
                && e.Button == MouseButtons.Left
                && hitInfo.RowHandle >= 0
                && hitInfo.Column != this.LOT_NUMBER
                && hitInfo.Column != this.ROWNUM)
            {
                this._preHitInfo = hitInfo;
                DataRow drSource = gvNewLot.GetDataRow(hitInfo.RowHandle);
                gvNewLot.GridControl.DoDragDrop(drSource, DragDropEffects.Copy);
                DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
        }
        /// <summary>
        /// 复制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcNewLot_DragOver(object sender, DragEventArgs e)
        {
            DataRow drSource = e.Data.GetData(typeof(DataRow)) as DataRow;
            if (drSource != null && this._preHitInfo != null)
            {

                Point targetPoint = gcNewLot.PointToClient(new Point(e.X, e.Y));
                GridHitInfo hitInfo = gvNewLot.CalcHitInfo(targetPoint);
                //复制单元格值。
                if (hitInfo.RowHandle != this._preHitInfo.RowHandle
                    && hitInfo.RowHandle >= 0
                    && hitInfo.Column != this.LOT_NUMBER
                    && hitInfo.Column != this.ROWNUM)
                {
                    e.Effect = DragDropEffects.Copy;
                    if (hitInfo.RowHandle > this._preHitInfo.RowHandle)
                    {
                        gvNewLot.FocusedRowHandle = hitInfo.RowHandle + 1;
                    }
                    else
                    {
                        gvNewLot.FocusedRowHandle = hitInfo.RowHandle - 1;
                    }
                    DataRow drCurrent = gvNewLot.GetDataRow(hitInfo.RowHandle);
                    drCurrent[this.gvNewLot.FocusedColumn.FieldName] = drSource[this.gvNewLot.FocusedColumn.FieldName];
                    this._preHitInfo = hitInfo;
                }
                else if (hitInfo.Column != this.SI_SUPPLIER_LOT)
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void gvNewLot_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.Name.Equals("CELLPN1"))
            {
                //  gvRowNumber = this.gvNewLot.FocusedRowHandle;
                string smallPackNumber = this.gvNewLot.FocusedValue.ToString();
                string cellInformation = "";
                string outcellSupplierName = string.Empty;
                LotCreateEntity lotCreatEntity = new LotCreateEntity();
                DataSet dsReturn = lotCreatEntity.GetCellInformation(smallPackNumber);  //小包条码信息
                string smallOut = smallPackNumber + "0000000000";
                DataSet dsOutcell = lotCreatEntity.GetOutCellSupplier(smallOut.Substring(0, 8).ToString());  //外购供应商条码信息

                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    //2015年7月21日 wx 添加bom工单判断 BEGIN
                    if (dsWorkOrderBom != null && dsWorkOrderBom.Tables[0].Rows.Count > 0)
                    {
                        if (dsWorkOrderBom.Tables[0].Select(string.Format("MATERIAL_CODE='{0}'", dsReturn.Tables[0].Rows[0]["PART_ID"].ToString())).Count() <= 0)
                        {
                            this.gvNewLot.SetFocusedRowCellValue(this.CELLPN1, "");
                            ClearRowData(gvNewLot.GetRow(e.RowHandle),e.RowHandle);
                            MessageService.ShowMessage("工单BOM未检索到该条码信息!", MESSAGEBOX_CAPTION);
                            return;
                        }
                    }
                    //2015年7月21日 wx 添加bom工单判断 END

                    cellInformation = string.Format(@"{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}",
                                                 dsReturn.Tables[0].Rows[0]["SUPPLIER"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["FACTORY_NAME"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["LINE"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["EFFICIENCY_NAME"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["MO"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["PART_ID"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["CDATE"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["GRADE"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["COLOR"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["CHECKER"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["BATERYTYPE"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["SEGMENTEDLOCATION"].ToString()
                                                , dsReturn.Tables[0].Rows[0]["SMALL_PACK_NUMBER"].ToString());


                    this.gvNewLot.SetFocusedRowCellValue(this.CELLSUPPLIER, dsReturn.Tables[0].Rows[0]["SUPPLIER"].ToString());
                    this.gvNewLot.SetFocusedRowCellValue(this.CELLFACTORY, dsReturn.Tables[0].Rows[0]["FACTORY_NAME"].ToString());
                    this.gvNewLot.SetFocusedRowCellValue(this.CELLLINE, dsReturn.Tables[0].Rows[0]["LINE"].ToString());
                    this.gvNewLot.SetFocusedRowCellValue(this.CELLCOLOR, dsReturn.Tables[0].Rows[0]["COLOR"].ToString());
                    this.gvNewLot.SetFocusedRowCellValue(this.CELLEFFICIENCY, dsReturn.Tables[0].Rows[0]["EFFICIENCY_NAME"].ToString());
                    this.gvNewLot.SetFocusedRowCellValue(this.MATERIAL_TIME, Convert.ToDateTime(dsReturn.Tables[0].Rows[0]["CREATE_DATE"]).ToString("yyyy-MM-dd HH:mm:ss"));//电池片生产时间

                    if (CheckEfficiency(dsReturn.Tables[0].Rows[0]["EFFICIENCY_NAME"].ToString()))
                    {
                        this.gvNewLot.SetFocusedRowCellValue("CHECK_SMALL_PACK_NUMBER", "Y");
                    }
                    else
                    {
                        this.gvNewLot.SetFocusedRowCellValue("CHECK_SMALL_PACK_NUMBER", "N");
                    }
                }
                else
                {                    
                    if (dsOutcell != null && dsOutcell.Tables.Count > 0 && dsOutcell.Tables[0].Rows.Count > 0)
                    {
                        outcellSupplierName = dsOutcell.Tables[0].Rows[0][0].ToString();
                        cellInformation = string.Format(@"{0}-{1}-{2}",
                                                            outcellSupplierName, smallOut.Substring(9, 10), smallOut.Substring(19, 6));
                    }
                    this.gvNewLot.SetFocusedRowCellValue("CHECK_SMALL_PACK_NUMBER", "Y");
                    this.gvNewLot.SetFocusedRowCellValue(this.MATERIAL_TIME, FanHai.Hemera.Utils.Common.Utils.GetCurrentDateTime().AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"));//电池片生产时间
                }
                this.gvNewLot.SetFocusedRowCellValue(this.SI_SUPPLIER_LOT, cellInformation);
                this.gvNewLot.SetFocusedRowCellValue(this.SMALL_PACK_NUMBER, smallPackNumber);

                this.gvNewLot.UpdateCurrentRow();
            }
        }

        /// <summary>
        ///  清空GridControl行数据
        /// </summary>
        /// <param name="dvRow"></param>
        private void ClearRowData(object dvRow,int rowCount)
        {
            DataRowView drv = (DataRowView)dvRow;
            DataColumnCollection dcc  = drv.Row.Table.Columns;
            int colCount = drv.Row.Table.Columns.Count;


            try
            {
                for (int i = 0; i < colCount; i++)
                {
                    this.gvNewLot.SetRowCellValue(rowCount, dcc[i].ColumnName, null);
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }

        }

        /// <summary>
        /// 检查电池片转换效率是否和领料转换效率一致
        /// </summary>
        /// <param name="cellEfficiency">电池片对应的转换效率</param>
        /// <returns>False：电池片转换效率和领料的不一致。 True：电池片的转换效率和领料的一致</returns>
        private bool CheckEfficiency(string cellEfficiency)
        {
            string workOrderEfficiency = Convert.ToString(this.gvMaterialLotList.GetDataRow(0)["EFFICIENCY"]);

            string[] lsWorkOrderEfficiency = workOrderEfficiency.TrimEnd('%').Trim().Replace("-", "").Replace("<", "").Replace("=", "").Split('%');
            string[] lsCellEfficiency = cellEfficiency.Replace("(", "").Replace(")", "").TrimEnd('%').Trim().Replace("-", "").Replace("<", "").Replace("=", "").Split('%');

            if (lsWorkOrderEfficiency.Length == lsCellEfficiency.Length)
            {
                string woEfficiency = string.Empty;
                string ceEfficiency = string.Empty;
                for (int i = 0; i < lsWorkOrderEfficiency.Length; i++)
                {
                    woEfficiency = decimal.Parse(lsWorkOrderEfficiency[i]).ToString("#,##0.00");
                    ceEfficiency = decimal.Parse(lsCellEfficiency[i]).ToString("#,##0.00");
                    if (woEfficiency != ceEfficiency)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
