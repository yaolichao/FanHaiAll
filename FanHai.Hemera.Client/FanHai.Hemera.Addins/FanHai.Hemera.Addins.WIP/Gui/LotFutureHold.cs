using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core.Services;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.CommonControls;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Framework.Gui;
using CommonUtil = FanHai.Hemera.Utils.Common.Utils;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    
    /// <summary>
    /// 表示预设暂停的用户控件。
    /// </summary>
    public partial class LotFutureHold : BaseUserCtrl
    {
        /// <summary>
        /// 批次号。
        /// </summary>
        private string _lotNo = string.Empty;
        /// <summary>
        /// 工单号。
        /// </summary>
        private string _workOrderNo = string.Empty;
        /// <summary>
        /// 工序名。
        /// </summary>
        private string _operationName = string.Empty;
        /// <summary>
        /// 动作名。
        /// </summary>
        private string _action = string.Empty;
        /// <summary>
        /// 当前所在的工厂车间主键。
        /// </summary>
        private string _factoryRoomKey = string.Empty;

        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示

        /// <summary>
        /// 构造函数
        /// </summary>
        public LotFutureHold()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.gcolIndex.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolIndex}");//"序号";
            this.gcolLotNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolLotNumber}");//"序列号";
            this.gcolQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolQuantity}");//"当前数量";
            this.gcolRouteName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolRouteName}");//"工艺流程";
            this.gcolRouteStepName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolRouteStepName}");//"当前工序";
            this.gcolWorkOrderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolWorkOrderNo}");//"工单号";
            this.gcolProId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolProId}");//"产品ID号";
            this.gcolFutureHoldStep.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolFutureHoldStep}");//"预设暂停工序";
            this.gcolFutureHoldAction.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolFutureHoldAction}");//"预设暂停动作";
            this.gcolRemark.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.gcolRemark}");//"备注";
            this.lciQueryResult.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.lciQueryResult}");//"查询结果";
            this.lciPaging.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.lciPaging}");//"分页";
            this.btnSearch.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.btnSearch}");//"查询";
            this.btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.btnAdd}");//"新增";
            this.btnModify.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.btnModify}");//"修改";
            this.btnDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.btnDelete}");//"删除";
        }




        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotFutureHold_Load(object sender, EventArgs e)
        {
            SetFactoryRoomKey();
            BindFutureHoldLotToControl();
            //GridViewHelper.SetGridView(this.gvLot);
            this.lblMenu.Text = "生产管理>组件管理>提前暂停";
            BindQueryInfo();
        }
        /// <summary>
        /// 绑定查询条件的信息
        /// </summary>
        private void BindQueryInfo()
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
            this.txtLotNo.Text = this._lotNo;
            this.txtWorkOrderNo.Text = this._workOrderNo;
            this.lueOperationName.EditValue = this._operationName;
            this.lueAction.EditValue = this._action;
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
                dsOperation.Tables[0].Rows.InsertAt(dsOperation.Tables[0].NewRow(), 0);
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
        /// <summary>
        /// 设置工厂车间主键。
        /// </summary>
        private void SetFactoryRoomKey()
        {
            string code = PropertyService.Get(PROPERTY_FIELDS.FACTORY_CODE);
            LocationEntity entity = new LocationEntity();
            if (!string.IsNullOrEmpty(code))
            {
                DataSet ds=entity.GetFactoryRoom(code);
                if (string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        _factoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0][FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);
                    }
                    else
                    {
                        _factoryRoomKey = code;
                    }
                }
                else
                {
                    MessageBox.Show(entity.ErrorMsg);
                    _factoryRoomKey = code;
                }
            }
        }
        /// <summary>
        /// 批次查询Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //LotFutureHoldQueryDialog dlg = new LotFutureHoldQueryDialog();
            //dlg.LotNo = this._lotNo;
            //dlg.WorkOrderNo = this._workOrderNo;
            //dlg.OperationName = this._operationName;
            //dlg.ActionName = this._action;

            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    this._operationName = dlg.OperationName;
            //    this._lotNo = dlg.LotNo;
            //    this._action = dlg.ActionName;
            //    this._workOrderNo = dlg.WorkOrderNo;
            //    BindFutureHoldLotToControl();
            //}

            this._lotNo = this.txtLotNo.Text.Trim();
            this._workOrderNo = this.txtWorkOrderNo.Text.Trim();
            this._operationName = Convert.ToString(this.lueOperationName.EditValue);
            this._action = Convert.ToString(this.lueAction.EditValue);
            BindFutureHoldLotToControl();
        }
        /// <summary>
        /// 绑定预设暂停的批次数据到控件中。
        /// </summary>
        private void BindFutureHoldLotToControl()
        {
            FutureHoldEntity entity = new FutureHoldEntity();
            DataSet dsParams = new DataSet();

            Hashtable htParams = new Hashtable();
            if (!string.IsNullOrEmpty(this._factoryRoomKey))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, this._factoryRoomKey);
            }
            if (!string.IsNullOrEmpty(this._lotNo))
            {
                htParams.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER, this._lotNo);
            }
            if (!string.IsNullOrEmpty(this._operationName))
            {
                htParams.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME, this._operationName);
            }
            if (!string.IsNullOrEmpty(this._workOrderNo))
            {
                htParams.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER, this._workOrderNo);
            }
            if (!string.IsNullOrEmpty(this._action))
            {
                htParams.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME, this._action);
            }
            DataTable dtParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParams);
            dsParams.Tables.Add(dtParams);
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet ds = entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.gcLot.DataSource = ds.Tables[0];
                gvLot.BestFitColumns();
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg,MESSAGEBOX_CAPTION);
            }
        }
        /// <summary>
        /// 新增按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            LotFutureHoldAddDialog dlg = new LotFutureHoldAddDialog(_factoryRoomKey);
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this._workOrderNo = string.Empty;
                this._operationName = string.Empty;
                this._lotNo = string.Empty;
                this._action = string.Empty;
                BindFutureHoldLotToControl();
            }
        }
        /// <summary>
        /// 修改按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (this.gvLot.RowCount == 0)
            {
                return;
            }
            if (this.gvLot.SelectedRowsCount != 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.Msg001}"), MESSAGEBOX_CAPTION);//请必须选择一行
                //MessageService.ShowMessage("请必须选择一行。", "提示");
                return;
            }
            int[] rows = this.gvLot.GetSelectedRows();
            int row = rows[0];
            string rowKey = Convert.ToString(this.gvLot.GetRowCellValue(row, "ROW_KEY"));

            LotFutureHoldDetailDialog dlg = new LotFutureHoldDetailDialog(rowKey);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this._workOrderNo = string.Empty;
                this._operationName = string.Empty;
                this._lotNo = string.Empty;
                this._action = string.Empty;
                BindFutureHoldLotToControl();
            }
        }
        /// <summary>
        /// 删除按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.gvLot.RowCount == 0)
            {
                return;
            }
            if (this.gvLot.SelectedRowsCount != 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.Msg001}"), MESSAGEBOX_CAPTION);//请必须选择一行
                //MessageService.ShowMessage("请必须选择一行。", "提示");
                return;
            }
            int[] rows = this.gvLot.GetSelectedRows();
            int row = rows[0];
            string rowKey = Convert.ToString(this.gvLot.GetRowCellValue(row, "ROW_KEY"));
            string deletor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //if (MessageService.AskQuestion("确定要删除选中的记录？", "提示") == true) //确定要删除选中的记录？
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.Msg002}"), MESSAGEBOX_CAPTION) == true) //确定要删除选中的记录？
                {
                FutureHoldEntity entity = new FutureHoldEntity();
                entity.Delete(rowKey, deletor);
                if (!string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    MessageService.ShowMessage(entity.ErrorMsg, MESSAGEBOX_CAPTION);
                }
                else
                {
                    BindFutureHoldLotToControl();
                }
            }
        }
        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindFutureHoldLotToControl();
        }
 
        /// <summary>
        /// 自定义绘制单元格内容。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLot_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolIndex)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column==this.gcolFutureHoldAction)
            {
                if (e.CellValue.ToString() == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN)
                {
                    e.DisplayText = "进站后暂停";
                }
                else if (e.CellValue.ToString() == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT)
                {
                    e.DisplayText = "出站后暂停";
                }
            }
        }

    }
}
