using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 表示工单管理（创建、修改、删除）的窗体类。
    /// </summary>
    public partial class WorkOrderManage : BaseUserCtrl
    {
        private IViewContent _view = null;
        WorkOrderEntity _entity = new WorkOrderEntity();
        DataTable _dtWorkOrderNumber = null;
        DataTable _dtWorkOrderBom = null;
        DataTable _dtPartNumber = null;
        DataTable _dtMaterialCode = null;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderManage(IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            InitializeLanguage();
        }


        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工单管理 > 工单维护";
            GridViewHelper.SetGridView(gvBOM);

            this.btnRemoveBom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.btnRemoveBom}");// "移除";
            this.btnAddBom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.btnAddBom}");//"新增";
            this.gcolSeqNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gcolSeqNo}");// "序号";
            this.gclMaterialCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gclMaterialCode}");//"物料编码";
            this.gcolMaterialDescription.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gcolMaterialDescription}");//"物料描述";
            this.gcolReqQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gcolReqQty}");//"数量";
            this.gcolMaterilUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gcolMaterilUnit}");//"单位";
            this.gcMatkl.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gcMatkl}");//"参数组";
            this.lcgTop.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lcgTop}");//"基本信息";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciFactoryRoom}");//"车间名称";
            this.lciOrderNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciOrderNumber}");//"工单号";
            this.lciQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciQty}");//"工单计划数量";
            this.lciOrderState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciOrderState}");//"工单状态";
            this.lciStartDateTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciStartDateTime}");//"工单开始时间";
            this.lciEndDateTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciEndDateTime}");//"工单结束时间";
            this.lciOrderType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciOrderType}");//"工单类型";
            this.lciComment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciComment}");//"备注";
            this.lciPriority.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciPriority}");//"优先级";
            this.lciRevenueType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciRevenueType}");//"保税手册号";
            this.lciProId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciProId}");//"产品ID号";
            this.lciPartDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciPartDescription}");//"产品描述";
            this.lciPartNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciPartNumber}");//"产品料号";
            //this.lcgBOM.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lcgBOM}");//"工单物料";
            //this.lciAddBom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciAddBom}");//"新增工单物料";
            //this.lciRemoveBom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciRemoveBom}");//"移除工单物料";
            //this.lciBOM.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.lciBOM}");//"工单BOM";
            this.tsbSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.tsbSave}");//"保存";
            this.tsbNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.tsbNew}");//"新建";
            this.tsbModify.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.tsbModify}");//"修改";
            this.tsbOrderinfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.tsbOrderinfo}");//"工单信息清单";
            this.gcolUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.gcolUnit}");//"单位";
        }



        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkOrderManage_Load(object sender, EventArgs e)
        {
            //this.lblApplicationTitle.Text = this._view.TitleName;
            //绑定车间名称。
            BindFactoryRoom();
            //绑定产品ID
            //BindProId();
            BindOrderState();
            //绑定产品料号
            BindPartNumber();
            BindMaterialCode();
            //初始化控件值。
            InitControlValue();
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            this.teOrderNumber.Text = string.Empty;
            this.teQty.Text = "0";
            this.lueOrderState.EditValue = string.Empty;
            this.lueProId.EditValue = string.Empty;
            this.cmbPartNumber.Text = string.Empty;
            this.cmbRevenueType.Text = string.Empty;
            this.cmbOrderType.Text = string.Empty;
            this.cmbPriority.Text = "1";
            this.deStartDateTime.DateTime = DateTime.Now;
            this.deEndDateTime.DateTime = DateTime.Now;
            this.meComment.Text = string.Empty;
            this.tePartDescription.Text = string.Empty;

            POR_WORK_ORDER_FIELDS order = new POR_WORK_ORDER_FIELDS();
            this._dtWorkOrderNumber = CommonUtils.CreateDataTable(order);
            POR_WORK_ORDER_BOM_FIELDS orderBom = new POR_WORK_ORDER_BOM_FIELDS();
            this._dtWorkOrderBom = CommonUtils.CreateDataTable(orderBom);
            this.gcBOM.DataSource = this._dtWorkOrderBom;

            
        }

        private void SetControlReadOnly(bool bReadOnly)
        {
            this.teOrderNumber.Properties.ReadOnly = bReadOnly;
            this.teQty.Properties.ReadOnly = bReadOnly;
            this.lueOrderState.Properties.ReadOnly = bReadOnly;
            this.lueProId.Properties.ReadOnly = bReadOnly;
            this.cmbPartNumber.Properties.ReadOnly = bReadOnly;
            this.cmbRevenueType.Properties.ReadOnly = bReadOnly;
            this.cmbOrderType.Properties.ReadOnly = bReadOnly;
            this.cmbPriority.Properties.ReadOnly = bReadOnly;
            this.deStartDateTime.Properties.ReadOnly = bReadOnly;
            this.deEndDateTime.Properties.ReadOnly = bReadOnly;
            this.meComment.Properties.ReadOnly = bReadOnly;
            this.tePartDescription.Properties.ReadOnly = bReadOnly;
            this.gvBOM.OptionsBehavior.ReadOnly = bReadOnly;
        }
        /// <summary>
        /// 绑定工厂车间名称
        /// </summary>
        private void BindFactoryRoom()
        {
            string lines = PropertyService.Get(PROPERTY_FIELDS.LINES);//拥有权限的线上仓。
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(lines);
            //绑定工厂车间数据到窗体控件。
            if (dt != null)
            {
                this.cbFactoryRoom.Properties.DataSource = dt;
                this.cbFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.cbFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.cbFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.cbFactoryRoom.Properties.DataSource = null;
                this.cbFactoryRoom.EditValue = string.Empty;
            }
            //禁用车间
            if (dt != null && dt.Rows.Count == 1)
            {
                this.cbFactoryRoom.Properties.ReadOnly = true;
                this.cbFactoryRoom.Visible = false;
                this.lciFactoryRoom.Visibility = LayoutVisibility.Never;
            }
        }
        /// <summary>
        /// 绑定工单状态。
        /// </summary>
        private void BindOrderState()
        {
            DataTable dtOrderState = new DataTable();
            dtOrderState.Columns.Add("NAME");
            dtOrderState.Columns.Add("VALUE");
            dtOrderState.Rows.Add("下达", "REL");
            dtOrderState.Rows.Add("关闭", "TECO");
            dtOrderState.Rows.Add("删除", "删除");
            this.lueOrderState.Properties.DataSource = dtOrderState;
            this.lueOrderState.Properties.DisplayMember = "NAME";
            this.lueOrderState.Properties.ValueMember = "VALUE";
        }
        /// <summary>
        /// 绑定产品ID号。
        /// </summary>
        private void BindProId()
        {
            DataSet ds = _entity.GetProdId();
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                this.lueProId.Properties.DataSource = ds.Tables[0];
                this.lueProId.Properties.DisplayMember = "PRODUCT_CODE";
                this.lueProId.Properties.ValueMember = "PRODUCT_KEY";
            }
            else
            {
                this.lueProId.Properties.DataSource = null;
                this.lueProId.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定物料编码。
        /// </summary>
        private void BindMaterialCode()
        {
            DataSet ds = _entity.GetMaterialCode();
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                this._dtMaterialCode = ds.Tables[0];
                this.ricmbMaterialCode.Items.Clear();
                foreach (DataRow dr in this._dtMaterialCode.Rows)
                {
                    this.ricmbMaterialCode.Items.Add(dr["MATERIAL_CODE"]);
                }
            }
        }
        /// <summary>
        /// 绑定产品料号。
        /// </summary>
        private void BindPartNumber()
        {
            DataSet ds = _entity.GetPartNumber();
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                this._dtPartNumber = ds.Tables[0];
                this.cmbPartNumber.Properties.Items.Clear();
                foreach (DataRow dr in this._dtPartNumber.Rows)
                {
                    this.cmbPartNumber.Properties.Items.Add(dr["PART_ID"]);
                }
            }
        }
        /// <summary>
        /// 新建按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            InitControlValue();
            SetControlReadOnly(false);
            this.tsbSave.Enabled = true;
            this.tsbModify.Enabled = false;
            this.btnAddBom.Enabled = true;
            this.btnRemoveBom.Enabled = true;
        }
        /// <summary>
        /// 工单号回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teOrderNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)13)
            {
                string orderNumber=this.teOrderNumber.Text;
                DataSet dsOrderNumber = this._entity.GetWorkorderInfo(orderNumber);
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                    return;
                }
                if (dsOrderNumber.Tables.Count==0
                    || dsOrderNumber.Tables[0].Rows.Count==0)
                {
                    MessageService.ShowError(string.Format("工单({0})不存在，请确认。",orderNumber));
                    return;
                }
                this._dtWorkOrderNumber = dsOrderNumber.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                DataRow drWorkOrderNumber = this._dtWorkOrderNumber.Rows[0];
                //设置控件值。
                this.cbFactoryRoom.SelectedText = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_FACTORY_NAME]);
                this.teOrderNumber.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                this.teQty.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_ORDERED]);
                this.lueOrderState.EditValue = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_STATE]);
                this.lueProId.EditValue = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_PRO_ID]);
                this.cmbPartNumber.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER]);
                this.cmbRevenueType.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_REVENUE_TYPE]);
                this.cmbOrderType.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_TYPE]);
                this.cmbPriority.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_PRIORITY]);
                this.deStartDateTime.DateTime = Convert.ToDateTime(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_START_TIME]);
                this.deEndDateTime.DateTime =  Convert.ToDateTime(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_EDIT_TIME]);
                this.meComment.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_COMMENTS]);
                if (dsOrderNumber.Tables.Contains(POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME))
                {
                    this._dtWorkOrderBom = dsOrderNumber.Tables[POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME];
                }
                else
                {
                    POR_WORK_ORDER_BOM_FIELDS orderBom = new POR_WORK_ORDER_BOM_FIELDS();
                    this._dtWorkOrderBom = CommonUtils.CreateDataTable(orderBom);
                }
                this.gcBOM.DataSource = this._dtWorkOrderBom;

                SetControlReadOnly(true);

                this.tsbSave.Enabled = false;
                this.tsbModify.Enabled = true;
                this.btnAddBom.Enabled = false;
                this.btnRemoveBom.Enabled = false;

                e.Handled = true;
            }
        }
        /// <summary>
        /// 修改按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbModify_Click(object sender, EventArgs e)
        {
            SetControlReadOnly(false);
            this.teOrderNumber.Properties.ReadOnly = true;
            //this.lueProId.Properties.ReadOnly = true;
            //this.cmbPartNumber.Properties.ReadOnly = true;

            this.tsbSave.Enabled = true;
            this.btnAddBom.Enabled = true;
            this.btnRemoveBom.Enabled = true;
        }
        /// <summary>
        /// 保存按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.gvBOM.State == GridState.Editing
                && this.gvBOM.IsEditorFocused
                && this.gvBOM.EditingValueModified)
            {
                this.gvBOM.SetFocusedRowCellValue(this.gvBOM.FocusedColumn, this.gvBOM.EditingValue);
            }
            this.gvBOM.UpdateCurrentRow();

            string roomName = this.cbFactoryRoom.Text.Trim();
            if (string.IsNullOrEmpty(roomName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                this.cbFactoryRoom.Select();
                return;
            }

            string workOrderNumber = this.teOrderNumber.Text.Trim();
            if (string.IsNullOrEmpty(workOrderNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //MessageService.ShowMessage("工单号不能为空。", "提示");
                this.teOrderNumber.Select();
                return;
            }

            string qty = this.teQty.Text.Trim();
            if (string.IsNullOrEmpty(qty))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg002}"), MESSAGEBOX_CAPTION);//工单计划数量不能为空
                //MessageService.ShowMessage("工单计划数量不能为空。", "提示");
                this.teQty.Select();
                return;
            }

            string orderState = Convert.ToString(this.lueOrderState.EditValue);
            if (string.IsNullOrEmpty(orderState))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg003}"), MESSAGEBOX_CAPTION);//工单状态不能为空
                //MessageService.ShowMessage("工单状态不能为空。", "提示");
                this.lueOrderState.Select();
                return;
            }
            string proId = Convert.ToString(this.lueProId.EditValue);
            //if (string.IsNullOrEmpty(proId))
            //{
            //    MessageService.ShowMessage("产品ID不能为空。", "提示");
            //    this.lueProId.Select();
            //    return;
            //}

            string partNumber = this.cmbPartNumber.Text;
            if (string.IsNullOrEmpty(partNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg004}"), MESSAGEBOX_CAPTION);//产品料号不能为空
                //MessageService.ShowMessage("产品料号不能为空。", "提示");
                this.cmbPartNumber.Select();
                return;
            }

            string partDescription = this.tePartDescription.Text;
            if (string.IsNullOrEmpty(partDescription))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg005}"), MESSAGEBOX_CAPTION);//产品描述不能为空
                //MessageService.ShowMessage("产品描述不能为空。", "提示");
                this.tePartDescription.Select();
                return;
            }

            string revenueType = this.cmbRevenueType.Text.Trim();
            if (string.IsNullOrEmpty(revenueType))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg006}"), MESSAGEBOX_CAPTION);//保税手册号不能为空
                //MessageService.ShowMessage("保税手册号不能为空。", "提示");
                this.cmbRevenueType.Select();
                return;
            }


            string orderType =this.cmbOrderType.Text.Trim();
            if (string.IsNullOrEmpty(orderType))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg007}"), MESSAGEBOX_CAPTION);//工单类型不能为空
                //MessageService.ShowMessage("工单类型不能为空。", "提示");
                this.cmbOrderType.Select();
                return;
            }

            string priority =this.cmbPriority.Text.Trim();
            if (string.IsNullOrEmpty(priority))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg008}"), MESSAGEBOX_CAPTION);//工单优先级不能为空
                //MessageService.ShowMessage("工单优先级不能为空。", "提示");
                this.cmbPriority.Select();
                return;
            }

            string startTime = this.deStartDateTime.Text.Trim();
            if (string.IsNullOrEmpty(startTime))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg009}"), MESSAGEBOX_CAPTION);//工单开始时间不能为空
                //MessageService.ShowMessage("工单开始时间不能为空。", "提示");
                this.deStartDateTime.Select();
                return;
            }

            string endTime = this.deEndDateTime.Text.Trim();
            if (string.IsNullOrEmpty(endTime))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg010}"), MESSAGEBOX_CAPTION);//工单结束时间不能为空
                //MessageService.ShowMessage("工单结束时间不能为空。", "提示");
                this.deEndDateTime.Select();
                return;
            }
            //工单物料的数量必须输入。
            EnumerableRowCollection<DataRow> drs=this._dtWorkOrderBom
                        .AsEnumerable()
                        .Where(dr=>string.IsNullOrEmpty(Convert.ToString(dr[POR_WORK_ORDER_BOM_FIELDS.FIELD_REQ_QTY]))
                                   || Convert.ToDouble(dr[POR_WORK_ORDER_BOM_FIELDS.FIELD_REQ_QTY])<0);
            if(drs.Count()>0)
            {
                DataRow dr = drs.First();
                string materialCode = Convert.ToString(dr[POR_WORK_ORDER_BOM_FIELDS.FIELD_MATERIAL_CODE]);
                MessageService.ShowMessage(string.Format("工单物料({0})的数量必须大于等于0。",materialCode), MESSAGEBOX_CAPTION);
                this.gvBOM.FocusedColumn = this.gcolReqQty;
                this.gvBOM.FocusedRowHandle = this._dtWorkOrderBom.Rows.IndexOf(dr);
                return;
            }
            //移除物料号码为空的工单物料记录。
            drs = this._dtWorkOrderBom
                      .AsEnumerable()
                      .Where(dr => string.IsNullOrEmpty(Convert.ToString(dr[POR_WORK_ORDER_BOM_FIELDS.FIELD_MATERIAL_CODE])));
            foreach(DataRow dr in drs)
            {
                this._dtWorkOrderBom.Rows.Remove(dr);
            }

            if (this._dtWorkOrderNumber.Rows.Count == 0)
            {
                this._dtWorkOrderNumber.Rows.Add(this._dtWorkOrderNumber.NewRow());
            }
            DataRow drWorkorderNumber = this._dtWorkOrderNumber.Rows[0];
            string workOrderKey = Convert.ToString(drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]);

                //保存前判断该工单号是否存在
            WorkOrderEntity workOrderEntity = new WorkOrderEntity();
            DataSet dsIsExistOrder = new DataSet();
            dsIsExistOrder = workOrderEntity.GetWorkorderInfo(teOrderNumber.Text);
            bool isExistOrder = true;
            if (!string.IsNullOrEmpty(workOrderEntity.ErrorMsg))
            {
                MessageService.ShowError(workOrderEntity.ErrorMsg);
                return;
            }
            if (dsIsExistOrder.Tables.Count == 0 || dsIsExistOrder.Tables[0].Rows.Count == 0)
            {
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY] = "";
                isExistOrder = false;
            }
            else
            {
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY] = dsIsExistOrder.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY];
            }


            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_FACTORY_NAME] = roomName;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = workOrderNumber;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_ORDERED] = qty;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_STATE] = orderState;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_PRO_ID] = proId;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER] = partNumber;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_REVENUE_TYPE] = revenueType;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_TYPE] = orderType;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_PRIORITY] = priority;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_START_TIME] = startTime;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_EDIT_TIME] = endTime;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_PLANNED_START_TIME] = startTime;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_PLANNED_FINISH_TIME] = endTime;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_COMMENTS] = this.meComment.Text;
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_DESCRIPTIONS] = partDescription;
            //新增工单状态下。
            if (isExistOrder)
            {
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_PART_REVISION] = "1";
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_CLOSE_TYPE] = "0";
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_NEXT_SEQ] = "0";
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_LEFT] = qty;
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_MODULE] = "007";
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_LINENAME] = "07";
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_CREATE_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            }
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drWorkorderNumber[POR_WORK_ORDER_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataSet dsParam = new DataSet();
            dsParam.Merge(this._dtWorkOrderNumber);
            dsParam.Merge(this._dtWorkOrderBom);

            this._entity.Save(dsParam);

            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                SetControlReadOnly(true);
                this.btnAddBom.Enabled = false;
                this.btnRemoveBom.Enabled = false;
                this.tsbSave.Enabled = false;
                this.tsbModify.Enabled = true;

                //绑定产品料号
                BindPartNumber();
                BindMaterialCode();
            }
        }

        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 新增工单物料按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBom_Click(object sender, EventArgs e)
        {
            DataRow drBom = this._dtWorkOrderBom.NewRow();
            int seqNo = 1;
            if (this._dtWorkOrderBom.Rows.Count > 0)
            {
                object objSeqNo = this._dtWorkOrderBom
                                 .AsEnumerable()
                                 .Max(dr => dr[POR_WORK_ORDER_BOM_FIELDS.FIELD_SEQ_NO] != DBNull.Value
                                            ? Convert.ToInt32(dr[POR_WORK_ORDER_BOM_FIELDS.FIELD_SEQ_NO])
                                            : 0);

                if (objSeqNo != null && objSeqNo != DBNull.Value)
                {
                    seqNo = Convert.ToInt32(objSeqNo) + 1;
                }
            }
            drBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_SEQ_NO] = seqNo;
            drBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_ITEM_NO] = seqNo;
            this._dtWorkOrderBom.Rows.Add(drBom);
        }
        /// <summary>
        /// 移除工单物料按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveBom_Click(object sender, EventArgs e)
        {
            int index = this.gvBOM.FocusedRowHandle;
            if (index >= 0)
            {
                string orderNumber=Convert.ToString(this._dtWorkOrderBom.Rows[index][POR_WORK_ORDER_BOM_FIELDS.FIELD_ORDER_NUMBER]);
                if(!string.IsNullOrEmpty(orderNumber))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.WorkOrderManage.Msg011}"));//该记录不可删除，只能删除本次修改添加的物料。
                    //MessageService.ShowMessage("该记录不可删除，只能删除本次修改添加的物料。");
                    return;
                }
                this._dtWorkOrderBom.Rows.RemoveAt(index);
            }
        }
        /// <summary>
        /// 自定义绘制单元格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBOM_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && this._dtWorkOrderBom!=null)
            {
                string orderNumber = Convert.ToString(this._dtWorkOrderBom.Rows[e.RowHandle][POR_WORK_ORDER_BOM_FIELDS.FIELD_ORDER_NUMBER]);
                //不是本次修改添加的工单BOM，显示为灰色。
                if (!string.IsNullOrEmpty(orderNumber) && e.Column!=this.gcolReqQty)
                {
                    e.Appearance.BackColor = System.Drawing.Color.Gray;
                }
            }
        }
        /// <summary>
        /// 自定义显示编辑器。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBOM_ShowingEditor(object sender, CancelEventArgs e)
        {
            int index = this.gvBOM.FocusedRowHandle;
            if (this.gvBOM.FocusedColumn == this.gclMaterialCode)
            {
                string orderNumber = Convert.ToString(this._dtWorkOrderBom.Rows[index][POR_WORK_ORDER_BOM_FIELDS.FIELD_ORDER_NUMBER]);
                //不是本次修改添加的工单BOM，不能修改物料编码。
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    e.Cancel = true;
                }
            }
            else if (this.gvBOM.FocusedColumn == this.gcolMaterialDescription 
                     || this.gvBOM.FocusedColumn == this.gcolMaterilUnit)
            {
                string materialCode = Convert.ToString(this._dtWorkOrderBom.Rows[index][POR_WORK_ORDER_BOM_FIELDS.FIELD_MATERIAL_CODE]);
                EnumerableRowCollection<DataRow> drs = this._dtMaterialCode
                                                           .AsEnumerable()
                                                           .Where(dr => Convert.ToString(dr["MATERIAL_CODE"]) == materialCode);
                if (drs.Count() > 0)
                {
                    e.Cancel = true;
                }
            }
        }
        /// <summary>
        /// 工单BOM单元值改变时触发事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBOM_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //物料编码。
            if (e.Column == this.gclMaterialCode)
            {
                string materialCode = Convert.ToString(e.Value);
                EnumerableRowCollection<DataRow> drs = this._dtMaterialCode
                                                           .AsEnumerable()
                                                           .Where(dr => Convert.ToString(dr["MATERIAL_CODE"]) == materialCode);
                if (drs.Count() > 0)
                {
                    DataRow dr = drs.First();
                    this._dtWorkOrderBom.Rows[e.RowHandle][POR_WORK_ORDER_BOM_FIELDS.FIELD_DESCRIPTION] = dr["MATERIAL_NAME"];
                    this._dtWorkOrderBom.Rows[e.RowHandle][POR_WORK_ORDER_BOM_FIELDS.FIELD_MATERIAL_UNIT] = dr["UNIT"];
                    this.gvBOM.FocusedColumn = this.gcolReqQty;
                }
            }
        }
        /// <summary>
        /// 显示工单清单。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOrderinfo_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is WorkOrderInfoListViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkOrderInfoListViewContent workOrderInfoListViewContent = new WorkOrderInfoListViewContent();
            WorkbenchSingleton.Workbench.ShowView(workOrderInfoListViewContent);
        }
        /// <summary>
        /// 产品料号值变化时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPartNumber_SelectedValueChanged(object sender, EventArgs e)
        {
            string partNumber = this.cmbPartNumber.Text;
            EnumerableRowCollection<DataRow> drs=this._dtPartNumber.AsEnumerable()
                                                     .Where(dr => Convert.ToString(dr[POR_PART_FIELDS.FIELD_PART_ID]) == partNumber);
            if (drs.Count() > 0)
            {
                this.tePartDescription.Text = Convert.ToString(drs.First()[POR_PART_FIELDS.FIELD_PART_DESC]);
                this.tePartDescription.Properties.ReadOnly = true;
            }
            else
            {
                this.tePartDescription.Text = string.Empty;
                this.tePartDescription.Properties.ReadOnly = false;
            }
        }

        private void gvBOM_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
