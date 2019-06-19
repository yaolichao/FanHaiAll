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
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraPrinting;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    
    /// <summary>
    /// 表示批次查询的用户控件。
    /// </summary>
    public partial class LotQueryCtrl :BaseUserCtrl
    {
        IViewContent _viewContent;
        //LotQueryConditionDialog _queryConditionDlg = null;
        DataTable _dtLotType = null;
        DataTable _dtProductGrade = null;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示

        private LotQueryConditionModel Model = new LotQueryConditionModel();

        /// <summary>
        /// 构造函数
        /// </summary>
        public LotQueryCtrl(IViewContent viewContent)
        {
            InitializeComponent();
            _viewContent = viewContent;
            InitializeLanguage();
        }

        private void InitializeLanguage()
        {
            this.gcolRowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolRowNum}");//"序号";
            this.gcolLotNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolLotNumber}");//"序列号";
            this.gcolLotType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolLotType}");//"批次类型";
            this.gcolOrgWorkOrderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolOrgPartNumber}");//"原产品料号";
            this.gcolWorkOrderNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolWorkOrderNumber}");//"工单号";
            this.gcolPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolPartNumber}");//"产品料号";
            this.gcolProId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolProId}");//"产品ID号";
            this.gcolLotLineCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolLotLineCode}");//"主线名称";
            this.gcolEfficiency.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolEfficiency}");//"转换效率";
            this.gcolQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolQuantity}");//"数量";
            this.gcolProLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolProLevel}");//"产品等级";
            this.gcolRouteName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolRouteName}");//"工艺流程";
            this.gcolStepName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolStepName}");//"工序";
            this.gcolStartWaitTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolStartWaitTime}");//"开始等待时间";
            this.gcolStartProcessTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolStartProcessTime}");//"开始处理时间";
            this.gcolStateFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolStateFlag}");//"状态";
            this.gcolMaterialLot.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolMaterialLot}");//"领料批号";
            this.gcolSupplier.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolSupplier}");//"供应商";
            this.gcolHoldFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolHoldFlag}");//"暂停状态";
            this.gcolReworkFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolReworkFlag}");//"重工状态";
            this.gcolDeletedFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolDeletedFlag}");//"批次标记";
            this.gcolShippedFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolShippedFlag}");//"出货状态";
            this.gcolCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolCreator}");//"创建人";
            this.gcolCreateTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolCreateTime}");//"创建时间";
            this.gclLine.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gclLine}");//"当前线别";
            this.gclEquipment.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gclEquipment}");//"当前设备";
            this.gcolPalletNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolPalletNo}");//"托盘号";
            this.gcolPalletTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.gcolPalletTime}");//"包装时间";
            this.lciQueryResult.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.lciQueryResult}");//"查询结果";
            this.lciPaging.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.lciPaging}");//"分页";
            //this.btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.btnQuery}");//"查询"; 
        }




        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotQueryCtrl_Load(object sender, EventArgs e)
        {
            BindLotType();
            //GridViewHelper.SetGridView(this.gvLot);
            lblMenu.Text = "生产管理>过站管理>组件查询";
            BindQueryCondition();
        }
        /// <summary>
        /// 绑定批次类型。
        /// </summary>
        private void BindLotType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.Lot_Type);
            _dtLotType = BaseData.Get(columns, category);
        }
        /// <summary>
        /// 绑定查询条件的数据信息
        /// </summary>
        private void BindQueryCondition()
        {
            BindFactoryRoom();
            BindOpeartion();
            BindHoldFlag();
            BindReworkFlag();
            BindDeletedFlag();
            BindShippedFlag();
            BindLotTypeQuery();
            BindLotState();
            InitControlValue();
        }

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom.EditValue = Convert.ToString(dt.Rows[0]["LOCATION_KEY"]);
                    this.Model.RoomKey = Convert.ToString(dt.Rows[0]["LOCATION_KEY"]);
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定工序名称到控件。
        /// </summary>
        private void BindOpeartion()
        {
            RouteQueryEntity entity = new RouteQueryEntity();
            DataSet dsOperation = entity.GetDistinctOperationNameList();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                DataRow dr = dsOperation.Tables[0].NewRow();
                dr["ROUTE_OPERATION_NAME"] = string.Empty;
                dsOperation.Tables[0].Rows.InsertAt(dr, 0);
                this.lueOperation.Properties.DataSource = dsOperation.Tables[0];
                this.lueOperation.Properties.ValueMember = "ROUTE_OPERATION_NAME";
                this.lueOperation.Properties.DisplayMember = "ROUTE_OPERATION_NAME";
            }
            else
            {
                this.lueOperation.Properties.DataSource = null;
                MessageService.ShowMessage(entity.ErrorMsg, "提示");
            }
        }
        /// <summary>
        /// 绑定暂停标记。
        /// </summary>
        private void BindHoldFlag()
        {
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("VALUE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add("未暂停", "0");
            dtState.Rows.Add("已暂停", "1");
            lueHoldFlag.Properties.DataSource = dtState;
            lueHoldFlag.Properties.DisplayMember = "NAME";
            lueHoldFlag.Properties.ValueMember = "VALUE";
            lueHoldFlag.ItemIndex = 0;
        }
        /// <summary>
        /// 绑定重工标记。
        /// </summary>
        private void BindReworkFlag()
        {
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("VALUE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add("正常", "0");
            dtState.Rows.Add("重工", "1");
            lueReworkFlag.Properties.DataSource = dtState;
            lueReworkFlag.Properties.DisplayMember = "NAME";
            lueReworkFlag.Properties.ValueMember = "VALUE";
            lueReworkFlag.ItemIndex = 0;
        }
        /// <summary>
        /// 绑定删除标记。
        /// </summary>
        private void BindDeletedFlag()
        {
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("VALUE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add("正常", "0");
            dtState.Rows.Add("已结束", "1");
            dtState.Rows.Add("已删除", "2");
            lueDeletedFlag.Properties.DataSource = dtState;
            lueDeletedFlag.Properties.DisplayMember = "NAME";
            lueDeletedFlag.Properties.ValueMember = "VALUE";
            lueDeletedFlag.ItemIndex = 0;
        }
        /// <summary>
        /// 绑定出货标记。
        /// </summary>
        private void BindShippedFlag()
        {
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("VALUE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add("未出货", "0");
            dtState.Rows.Add("已出货", "1");
            lueShippedFlag.Properties.DataSource = dtState;
            lueShippedFlag.Properties.DisplayMember = "NAME";
            lueShippedFlag.Properties.ValueMember = "VALUE";
            lueShippedFlag.ItemIndex = 0;
        }
        /// <summary>
        /// 绑定批次类型。
        /// </summary>
        private void BindLotTypeQuery()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Lot_Type");
            DataTable dtLotType = BaseData.Get(columns, category);
            DataRow dr = dtLotType.NewRow();
            dr["CODE"] = string.Empty;
            dr["NAME"] = "全部";
            dtLotType.Rows.InsertAt(dr, 0);
            this.lueLotType.Properties.DataSource = dtLotType;
            this.lueLotType.Properties.DisplayMember = "NAME";
            this.lueLotType.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 绑定批次状态。
        /// </summary>
        private void BindLotState()
        {
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("CODE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add(CommonUtils.GetEnumValueDescription(LotStateFlag.WaitintForTrackIn), (int)LotStateFlag.WaitintForTrackIn);
            dtState.Rows.Add(CommonUtils.GetEnumValueDescription(LotStateFlag.OutEDC), (int)LotStateFlag.OutEDC);
            dtState.Rows.Add(CommonUtils.GetEnumValueDescription(LotStateFlag.WaitingForTrackout), (int)LotStateFlag.WaitingForTrackout);
            dtState.Rows.Add(CommonUtils.GetEnumValueDescription(LotStateFlag.Finished), (int)LotStateFlag.Finished);
            dtState.Rows.Add(CommonUtils.GetEnumValueDescription(LotStateFlag.ToStore), (int)LotStateFlag.ToStore);
            lueState.Properties.DataSource = dtState;
            lueState.Properties.DisplayMember = "NAME";
            lueState.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            if (!string.IsNullOrEmpty(this.Model.RoomKey))
            {
                this.lueFactoryRoom.EditValue = this.Model.RoomKey;
            }
            this.teLotNumber.Text = this.Model.LotNumber;
            this.teWorkOrderNumber.Text = this.Model.WorkOrderNumber;
            this.teProId.Text = this.Model.ProId;
            this.lueOperation.EditValue = this.Model.OperationName;
            this.lueHoldFlag.EditValue = this.Model.HoldFlag;
            this.lueReworkFlag.EditValue = this.Model.ReworkFlag;
            this.lueDeletedFlag.EditValue = this.Model.DeletedFlag;
            this.lueShippedFlag.EditValue = this.Model.ShippedFlag;
            this.lueLotType.EditValue = this.Model.LotType;
            this.lueState.EditValue = this.Model.StateFlag;
            this.teLotNumberEnd.Text = this.Model.LotNumber1;
            this.teCreator.Text = this.Model.Creator;
            this.deStartCreateDate.DateTime = DateTime.Parse(this.Model.StartCreateDate);
            this.deEndCreateDate.DateTime = DateTime.Parse(this.Model.EndCreateDate);
        }

        /// <summary>
        /// 获取产品等级的显示值。
        /// </summary>
        /// <returns>产品等级的显示值</returns>
        private string GetProductGradeDisplayText(string value)
        {
            string displayText = value;
            try
            {
                if (this._dtProductGrade == null)
                {
                    string[] columns = new string[] { "Column_Name","Column_code" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                    List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                    whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
                    this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                }
                if (null != this._dtProductGrade)
                {
                    DataRow [] drs = this._dtProductGrade.Select(string.Format("Column_code='{0}'", value));
                    if (drs.Length > 0)
                    {
                        displayText = Convert.ToString(drs[0]["Column_Name"]);
                    }
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            string factoryRoomKey = Convert.ToString(this.lueFactoryRoom.EditValue); ;
            if (string.IsNullOrEmpty(factoryRoomKey))
            {
                MessageService.ShowMessage("请选择车间。", "系统提示");
                this.lueFactoryRoom.Select();
                return;
            }
            //开始时间>结束时间
            if (this.deStartCreateDate.DateTime > this.deEndCreateDate.DateTime)
            {
                MessageService.ShowMessage("开始时间必须小于结束时间。", "系统提示");
                this.deStartCreateDate.Select();
                return;
            }
            string DeletedFlag = Convert.ToString(this.lueDeletedFlag.EditValue);
            string HoldFlag = Convert.ToString(this.lueHoldFlag.EditValue);
            string LotNumber = this.teLotNumber.Text.ToUpper().Trim();
            string LotType = Convert.ToString(this.lueLotType.EditValue);
            string OperationName = Convert.ToString(this.lueOperation.EditValue);
            string ProId = this.teProId.Text.ToUpper().Trim();
            string ReworkFlag = Convert.ToString(this.lueReworkFlag.EditValue);
            string RoomKey = factoryRoomKey;
            string RoomName = Convert.ToString(this.lueFactoryRoom.Text);
            string ShippedFlag = Convert.ToString(this.lueShippedFlag.EditValue);
            string StateFlag = Convert.ToString(this.lueState.EditValue);
            string PalletNo = this.tePalletNo.Text.Trim();
            string WorkOrderNumber = this.teWorkOrderNumber.Text.ToUpper().Trim();
            string StartCreateDate = this.deStartCreateDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string EndCreateDate = this.deEndCreateDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string Creator = this.teCreator.Text.Trim();
            string LotNumber1 = this.teLotNumberEnd.Text.Trim();
            string PartNumber = this.tePartNumber.Text.Trim();
            string OrgOrderNumber = this.teOrgOrderNumber.Text.Trim();
            Hashtable htParams = new Hashtable();
            DataSet dsParams = new DataSet();
            DataSet dsReturn = new DataSet();
            //车间主键。
            htParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, RoomKey);
            //工序名称不为空
            if (!string.IsNullOrEmpty(OperationName))
            {
                htParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, OperationName);
            }
            //批次号不为空
            if (!string.IsNullOrEmpty(LotNumber1) && !string.IsNullOrEmpty(LotNumber))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_START", LotNumber);
                htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_END", LotNumber1);
            }
            //批次号不为空
            else if (!string.IsNullOrEmpty(LotNumber))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, LotNumber);
            }
            //工单号不为空
            if (!string.IsNullOrEmpty(WorkOrderNumber))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, WorkOrderNumber);
            }
            //工单号不为空
            if (!string.IsNullOrEmpty(OrgOrderNumber))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO, OrgOrderNumber);
            }
            //产品ID号不为空
            if (!string.IsNullOrEmpty(ProId))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_PRO_ID, ProId);
            }

            //产品料不为空
            if (!string.IsNullOrEmpty(PartNumber))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, PartNumber);
            }
            //托盘号不为空
            if (!string.IsNullOrEmpty(PalletNo))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_PALLET_NO, PalletNo);
            }
            //暂停标记不为空
            if (!string.IsNullOrEmpty(HoldFlag))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, HoldFlag);
            }
            //重工标记不为空
            if (!string.IsNullOrEmpty(ReworkFlag))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_IS_REWORKED, ReworkFlag);
            }
            //删除标记不为空
            if (!string.IsNullOrEmpty(DeletedFlag))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, DeletedFlag);
            }
            //出货标记不为空
            if (!string.IsNullOrEmpty(ShippedFlag))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG, ShippedFlag);
            }
            //状态不为空
            if (!string.IsNullOrEmpty(StateFlag))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG, StateFlag);
            }
            //批次类型不为空
            if (!string.IsNullOrEmpty(LotType))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE,LotType);
            }
            //创建人
            if (!string.IsNullOrEmpty(Creator))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_CREATOR, Creator);
            }
            //创建日期-起不为空
            if (!string.IsNullOrEmpty(StartCreateDate))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_CREATE_TIME + "_START", StartCreateDate);
            }
            //创建日期-止不为空
            if (!string.IsNullOrEmpty(EndCreateDate))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_CREATE_TIME + "_END", EndCreateDate);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            //查询批次
            LotQueryEntity entity = new LotQueryEntity();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            dsReturn = entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowMessage(entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                gcLot.DataSource = dsReturn.Tables[0];
                gcLot.MainView = gvLot;
                gvLot.BestFitColumns();
            }
        }

        /// <summary>
        /// 批次查询Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                
                BindLotInfo();
                //if (_queryConditionDlg == null)
                //{
                //    _queryConditionDlg = new LotQueryConditionDialog();
                //}
                //if (_queryConditionDlg.ShowDialog() == DialogResult.OK)
                //{
                //    BindLotInfo();
                //}
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 批次信息按钮事件处理函数。用于显示批次详细信息。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        private void btnDetails_Click(object sender, EventArgs e)
        {
            int index = this.gvLot.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.Msg001}"), MESSAGEBOX_CAPTION);//请选择批次记录
                //MessageService.ShowMessage("请选择批次记录。", "系统提示");
                return;
            }
            DataRow dr = this.gvLot.GetDataRow(index);
            string lotNumber = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            //显示批次事务处理历史的对话框。
            LotTransactionHistoryDialog transDlg = new LotTransactionHistoryDialog(lotNumber);
            if (DialogResult.OK == transDlg.ShowDialog())
            {

            }
            transDlg.Dispose();
            transDlg = null;
        }
        /// <summary>
        /// 工艺参数查询事件处理方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        private void btnParamSearch_Click(object sender, EventArgs e)
        {
            int index = this.gvLot.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.Msg001}"), MESSAGEBOX_CAPTION);//请选择批次记录
                //MessageService.ShowMessage("请选择批次记录。", "系统提示");
                return;
            }
            DataRow dr = this.gvLot.GetDataRow(index);
            string lotKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            string workOrderKey = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string lotNumber = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);

            EdcManage edcManage = new EdcManage();
            DataSet dsParam = edcManage.GetLotParamsCollection(lotNumber.ToUpper());
            //获取批次数据采集信息成功。
            if (dsParam != null && dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
            {
                //显示批次数据采集信息的对话框。
                LotParamSearchDialog dialog = new LotParamSearchDialog(dsParam, lotNumber.ToUpper());
                dialog.ShowDialog();
            }
            else
            {
                //获取批次数据采集信息失败，给出相应的提示。
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCtrl.Msg002}"), MESSAGEBOX_CAPTION);//该批次不存在相关的工艺参数信息
                //MessageService.ShowMessage("该批次不存在相关的工艺参数信息", "系统提示");
            }
        }
        /// <summary>
        /// 绘制自定义单元格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLot_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView gv = sender as GridView;
            //判读是否锁定了批次。
            string holdFlag = Convert.ToString(gv.GetRowCellValue(e.RowHandle,this.gcolHoldFlag));
            int stateFlag = Convert.ToInt32(gv.GetRowCellValue(e.RowHandle, this.gcolStateFlag));
            bool bHold = holdFlag == "1" ? true : false;
            if (bHold)
            {
                e.Appearance.BackColor = System.Drawing.Color.Red;
            }
            if (this.gcolRowNum == e.Column)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (this.gcolStateFlag == e.Column)
            {
                e.DisplayText = Utils.Common.Utils.GetEnumValueDescription(Enum.Parse(typeof(LotStateFlag), e.CellValue.ToString()));
            }
            else if (this.gcolHoldFlag == e.Column)
            {
                e.DisplayText = bHold? "暂停" : "正常";
            }
            else if (this.gcolDeletedFlag == e.Column)
            {
                int deleted = Convert.ToInt32(e.CellValue);
                e.DisplayText = (deleted == 2) ? "已删除" : (deleted == 1 ? "已结束" : "正常");
            }
            else if (this.gcolStartProcessTime == e.Column && stateFlag < 4)
            {
                e.DisplayText = string.Empty;
            }
            else if (this.gcolLotType == e.Column)
            {
                DataView dvLotType = _dtLotType.AsEnumerable()
                                          .Where(dr => Convert.ToString(dr["CODE"]) == Convert.ToString(e.CellValue))
                                          .AsDataView();
                e.DisplayText = Convert.ToString(dvLotType[0]["NAME"]);
            }
            else if (this.gcolReworkFlag == e.Column)
            {
                e.DisplayText = Convert.ToInt32(e.CellValue) > 0 ? "重工" : "正常";
            }
            else if (this.gcolShippedFlag == e.Column)
            {
                e.DisplayText = Convert.ToInt32(e.CellValue) == 0 ? "未出货" : "已出货";
            }
            else if (this.gcolProLevel == e.Column)
            {
                e.DisplayText = GetProductGradeDisplayText(Convert.ToString(e.CellValue));
            }
        }

        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindLotInfo();
        }
        /// <summary>
        /// 批次记录双击事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLot_DoubleClick(object sender, EventArgs e)
        {
            int index = this.gvLot.FocusedRowHandle;
            if (index < 0)
            {
                return;
            }
            btnDetails_Click(sender, e);
        }



        /// <summary>
        /// 导出为EXCEL。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件(*.xls)|*.xls";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                XlsExportOptions options = new XlsExportOptions();
                this.gvLot.ExportToXls(dlg.FileName, options);
            }
        }

        //private void gvLot_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        //{
        //    if (e.Info.IsRowIndicator && e.RowHandle >= 0)
        //    {
        //        e.Info.DisplayText = (e.RowHandle + 1).ToString();
        //    }
        //}
    }
}