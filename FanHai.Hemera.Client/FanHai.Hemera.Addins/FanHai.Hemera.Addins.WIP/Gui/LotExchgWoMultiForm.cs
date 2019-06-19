using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Dialogs;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using System.Net;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 转/返工单作业窗体对话框。
    /// </summary>
    public partial class LotExchgWoMultiForm : BaseDialog
    {
        LotOperationEntity _lotEntity = new LotOperationEntity();
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示
        /// <summary>
        /// 批次转返工标记。
        /// </summary>
        ExchangeWoFlag _flag = ExchangeWoFlag.Repair;
        /// <summary>
        /// 转返工名称。
        /// </summary>
        string _flagName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm._flagName001}");//"返工";
        /// <summary>
        /// 暂存包装明细数据。
        /// </summary>
        DataTable dtExchangeData = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="repair"></param>
        public LotExchgWoMultiForm(ExchangeWoFlag flag)
        {
            InitializeComponent();
            this._flag = flag;
            InitializeLanguage();
            if (flag == ExchangeWoFlag.Exchange)
            {
                this._flagName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm._flagName002}");//"转工单";
                this.chkIsPallet.Checked = true;
                this.chkIsPallet.Visible = false;
                this.lciIsPalletNo.Visibility = LayoutVisibility.Never;
            }
            else
            {
                this._flagName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm._flagName003}");//"返工单";
            }
            GridViewHelper.SetGridView(gvExchangeWo);

        }


        private void InitializeLanguage()
        {
            this.chkIsPallet.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.chkIsPallet}");//"按批次";
            this.btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.btnCancel}");//"取消";
            this.btnSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.btnSave}");//"确定";
            this.gcolLotNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolLotNumber}");//"批次号";
            this.gcolPalletNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolPalletNo}");//"托号";
            this.gcolWorkOrderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolWorkOrderNo}");//"工单号";
            this.gcolPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolPartNumber}");//"产品料号";

            this.gcolProductId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolProductId}");//"产品ID号";
            this.gcolGradeName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolGradeName}");//"等级";
            this.gcolPower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolPower}");//"功率";
            this.gcolNewWorkorderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolNewWorkorderNo}");//"新工单号";

            this.gcolNewPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolNewPartNumber}");//"新产品料号";
            this.gcolNewProductId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolNewProductId}");//"新产品ID号";
            this.lcgTop.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lcgTop}");//"基本信息";
            this.lciNewWorkorderNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciNewWorkorderNo}");//"工单号";
            this.lciNewPartNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciNewPartNumber}");//"产品料号";

            this.lciNewProductId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciNewProductId}");//"产品ID号";
            this.lciPalletNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciPalletNo}");//"托号";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciFactoryRoom}");//"工厂车间";
            this.lciIsPalletNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciIsPalletNo}");//"按批次返工";

            this.lcgBottom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lcgBottom}");//"明细列表";
            this.lciList.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciList}");//"明细";
            this.lciRemark.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciRemark}");//"备注";
            this.lcgMiddle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lcgMiddle}");//"新工艺路线";
            this.lciEditRoute.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciEditRoute}");//"工艺流程组";
            this.lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciOperation}");//"工艺流程";
            this.lciStep.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciStep}");//"工步";
            this.lcgButtons.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lcgButtons}");//"按钮";
            this.lciOk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciOk}");//"确定";
            this.lciCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.lciCancel}");//"取消";
        }



        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotExchgWoMultiForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0}" + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.Text001}"), this._flagName);//作业
            BindFactoryRoom();
            InitControlsValue();
            this.teNewWorkorderNo.Select();
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
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlsValue()
        {
            this.txtPalletNo.Text = string.Empty;
            this.teNewWorkorderNo.Text = string.Empty;
            this.teNewWorkorderNo.Tag = string.Empty;
            this.lueNewPartNumber.Properties.DataSource = null;
            this.lueNewPartNumber.EditValue = null;
            this.teNewProductId.Text = string.Empty;

            if (dtExchangeData != null)
            {
                dtExchangeData.Clear();
                dtExchangeData = null;
            }
        }
        /// <summary>
        /// 工单号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teNewWorkorderNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }
            this.lueNewPartNumber.EditValueChanged -= new EventHandler(lueNewPartNumber_EditValueChanged);
            InitWorkOrderData();
            this.lueNewPartNumber.EditValueChanged += new EventHandler(lueNewPartNumber_EditValueChanged);
        }
        /// <summary>
        /// 初始化工单数据。
        /// </summary>
        private void InitWorkOrderData()
        {
            //初始化车间、工单、产品料号、产品ID
            this.lueFactoryRoom.Enabled = true;
            this.teNewWorkorderNo.Tag = string.Empty;
            this.lueNewPartNumber.Properties.DataSource = null;
            this.lueNewPartNumber.EditValue = null;
            this.teNewProductId.Text = string.Empty;

            string roomName=this.lueFactoryRoom.Text;
            string roomKey= Convert.ToString(this.lueFactoryRoom.EditValue);
             //必须选择车间。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.Msg001}"), MESSAGEBOX_CAPTION);//请选择工厂车间
                //MessageService.ShowMessage(string.Format("请选择工厂车间。", "提示"));
                lueFactoryRoom.Select();
                return;
            }
            //判断工单号。
            string orderNo = this.teNewWorkorderNo.Text.Trim();
            if (string.IsNullOrEmpty(orderNo))
            {
                return;
            }
            DataSet dsWorkorderData = this._lotEntity.GetWoProductData(orderNo);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return;
            }
            DataTable dtWorkorder = dsWorkorderData.Tables[0];
            DataTable dtWoProcunt = dsWorkorderData.Tables[1];
            //数据库中不存在该工单。
            if (dtWorkorder.Rows.Count <= 0)
            {
                MessageService.ShowMessage(string.Format("未找到工单号：{0}，请确认。", orderNo));
                teNewWorkorderNo.Select();
                teNewWorkorderNo.SelectAll();
                return;
            }
            //判断工单是否属于当前车间。
            string orderRoomName = Convert.ToString(dtWorkorder.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_FACTORY_NAME]);
            if (orderRoomName != roomName)
            {
                MessageService.ShowMessage(string.Format("工单（{0}）不属于车间{1}，请确认。", orderNo,roomName));
                teNewWorkorderNo.Select();
                teNewWorkorderNo.SelectAll();
                return;
            }
            //判断工单是否设置了产品。
            if (dtWoProcunt.Rows.Count <= 0)
            {
                MessageService.ShowMessage(string.Format("工单（{0}）还未设置产品，请确认。", orderNo));
                teNewWorkorderNo.Select();
                teNewWorkorderNo.SelectAll();
                return;
            }
            //设置工单主键。
            string orderKey = Convert.ToString(dtWorkorder.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]);
            this.teNewWorkorderNo.Tag = orderKey;
            //绑定产品料号数据。
            this.lueNewPartNumber.Properties.DataSource = dtWoProcunt;
            this.lueNewPartNumber.Properties.ValueMember = "PART_NUMBER";
            this.lueNewPartNumber.Properties.DisplayMember = "PART_NUMBER";
            //设置产品料号和产品ID号。
            string partNumber = Convert.ToString(dtWoProcunt.Rows[0]["PART_NUMBER"]);
            string proId = Convert.ToString(dtWoProcunt.Rows[0]["PRODUCT_CODE"]);
            this.lueNewPartNumber.EditValue = partNumber;
            this.teNewProductId.Text = proId;
            this.lueNewPartNumber.Select();
            this.lueFactoryRoom.Enabled = false;
        }
        /// <summary>
        /// 产品料号值改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueNewPartNumber_EditValueChanged(object sender, EventArgs e)
        {
            string orderKey = Convert.ToString(this.teNewWorkorderNo.Tag);
            string orderNo = Convert.ToString(this.teNewWorkorderNo.Text);
            string partNumber = Convert.ToString(this.lueNewPartNumber.EditValue);
            string proId = Convert.ToString(this.lueNewPartNumber.GetColumnValue("PRODUCT_CODE"));
            this.teNewProductId.Text = proId;
            if (string.IsNullOrEmpty(orderKey))
            {
                this.teNewWorkorderNo.Select();
                this.teNewWorkorderNo.SelectAll();
            }
            else if (string.IsNullOrEmpty(partNumber))
            {
                this.lueNewPartNumber.Select();
            }
            else
            {
                this.meRemark.Select();
                this.meRemark.SelectAll();
            }
        }
        /// <summary>
        /// 托号文本框回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //回车按钮。
            if (e.KeyChar == 13)
            {
                InitExchangeData();
            }
        }
        /// <summary>
        /// 初始化转返工单数据。
        /// </summary>
        private void InitExchangeData()
        {
            string roomName = this.lueFactoryRoom.Text;
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            //必须选择车间。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.Msg001}"), MESSAGEBOX_CAPTION);//请选择工厂车间
                //MessageService.ShowMessage(string.Format("请选择工厂车间。", "提示"));
                lueFactoryRoom.Select();
                return;
            }

            string orderKey = Convert.ToString(this.teNewWorkorderNo.Tag);
            string orderNo = Convert.ToString(this.teNewWorkorderNo.Text);
            string partNumber = Convert.ToString(this.lueNewPartNumber.EditValue);
            string proId = this.teNewProductId.Text;

            if (string.IsNullOrEmpty(orderKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg001}"), MESSAGEBOX_CAPTION);//请输入工单号！
                //MessageService.ShowMessage(string.Format("请输入工单号。"),"提示");
                this.teNewWorkorderNo.Select();
                this.teNewWorkorderNo.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(partNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.Msg002}"), MESSAGEBOX_CAPTION);//请选择产品
                //MessageService.ShowMessage(string.Format("请选择产品。"), "提示");
                this.lueNewPartNumber.Select();
                return;
            }

            string val = this.txtPalletNo.Text.Trim();
            //托盘号或批次号为空，直接返回。
            if (string.IsNullOrEmpty(val))
            {
                return;
            }
            DataSet dsReturn = null;
            string exchangeName = null;
            //按批次转返工
            if (this.chkIsPallet.Checked == true)
            {
                //转工单操作，判断组件所在当前工序是否允许转工单。
                if (this._flag == ExchangeWoFlag.Exchange)
                {
                    bool isAllowExchangeWO = true;
                    //获取批次对应的工步属性信息
                    dsReturn = this._lotEntity.GetLotRouteAttrData(val);
                    //判断获取的工步属性信息是否存在，若不存在默认为允许转工单
                    if (dsReturn.Tables.Contains("LOT_ROUTE_ATTR")
                        && dsReturn.Tables["LOT_ROUTE_ATTR"].Rows.Count > 0)
                    {
                        DataRow[] drRouteAttr = dsReturn.Tables["LOT_ROUTE_ATTR"]
                                                        .Select(string.Format(" ATTRIBUTE_NAME = '{0}'", "IsAllowExchangeWO"));

                        //判断是否有设置IsAllowExchangeWO属性
                        if (drRouteAttr.Length > 0)
                        {
                            string attrVal = Convert.ToString(drRouteAttr[0]["ATTRIBUTE_VALUE"]);
                            if (!bool.TryParse(attrVal, out isAllowExchangeWO))
                            {
                                isAllowExchangeWO = true;
                            }
                        }
                    }
                    //若设置为true允许进行转工单，反之弹窗提示并返回
                    if (isAllowExchangeWO == false)
                    {
                        MessageService.ShowMessage(string.Format("组件【{0}】当前所在工序不允许转工单！", val), "提示");
                        return;
                    }
                }
                dsReturn = this._lotEntity.GetExchangeData(string.Empty, val);
                exchangeName = "批次号";
            }
            //按托转返工
            else
            {
                dsReturn = this._lotEntity.GetExchangeData(val, string.Empty);
                exchangeName = "托号";
            }
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return;
            }
            //查询转返工数据出错
            if (dsReturn == null
                || dsReturn.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME) == false)
            {
                MessageService.ShowError(string.Format("查询{0}数据出错，请重试", this._flagName));
                return;
            }
            DataTable dtTempExchangeData = dsReturn.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
            if (dtTempExchangeData.Rows.Count <= 0)
            {//不存在数据，给出对应提示。
                MessageService.ShowError(string.Format("{1}【{0}】不存在，请确认!", val, exchangeName));
                return;
            }
            DataRow drTempExchangeData = dtTempExchangeData.Rows[0];
            int stateFalg = Convert.ToInt32(drTempExchangeData[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            string lotOrderNo = Convert.ToString(drTempExchangeData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            string palletNo=Convert.ToString(drTempExchangeData[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            //基础数据配置是否比较工单类型。
            string[] columns = new string[] { "IsCompareWorkOrderType" };
            string category = BASEDATA_CATEGORY_NAME.ExchangeWoConfig;
            DataTable dtExchangeWoConfig = BaseData.Get(columns, category);
            bool isCompareWorkorderType = true;
            if (dtExchangeWoConfig != null && dtExchangeWoConfig.Rows.Count > 0)
            {
                string compareWorkOrderType = Convert.ToString(dtExchangeWoConfig.Rows[0]["IsCompareWorkOrderType"]);
                if (!string.IsNullOrEmpty(compareWorkOrderType))
                {
                    isCompareWorkorderType = Convert.ToBoolean(compareWorkOrderType);
                }
            }
            //转工单：工单类型条件判断
            if (isCompareWorkorderType)
            {
                Hashtable hsOrderType = new Hashtable();
                hsOrderType.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, orderNo);
                hsOrderType.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + "2", lotOrderNo);
                ExchangeWoEntity exchangewoEntity = new ExchangeWoEntity();
                string msg = exchangewoEntity.CompareWorkOrderType(hsOrderType);
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageService.ShowError(msg);
                    teNewWorkorderNo.SelectAll();
                    teNewWorkorderNo.Select();
                    return;
                }
            }
            //未包装数据不能返工单。
            if (this._flag == ExchangeWoFlag.Repair && string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowError(string.Format("{1}【{0}】未包装，不符合{2}作业要求。", val, exchangeName, this._flagName));
                txtPalletNo.SelectAll();
                txtPalletNo.Select();
                return;
            }
            //未入库数据不能返工单。
            else if (this._flag == ExchangeWoFlag.Repair && !string.IsNullOrEmpty(palletNo) && stateFalg<=10)
            {
                MessageService.ShowError(string.Format("{1}【{0}】未入库，不符合{2}作业要求。", val, exchangeName, this._flagName));
                txtPalletNo.SelectAll();
                txtPalletNo.Select();
                return;
            }
            //已包装数据不能转工单。
            else if (this._flag == ExchangeWoFlag.Exchange && !string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowError(string.Format("{1}【{0}】已包装，不符合{2}作业要求。", val, exchangeName, this._flagName));
                txtPalletNo.SelectAll();
                txtPalletNo.Select();
                return;
            }//已入库数据不能转工单。
            else if (this._flag == ExchangeWoFlag.Exchange && stateFalg >= 10)
            {
                MessageService.ShowError(string.Format("{1}【{0}】已入库，不符合{2}作业要求。", val, exchangeName, this._flagName));
                txtPalletNo.SelectAll();
                txtPalletNo.Select();
                return;
            }

            //增加新列，用于设置新产品数据。
            if (!dtTempExchangeData.Columns.Contains("NEW_PRO_ID"))
            {
                dtTempExchangeData.Columns.Add("NEW_PRO_ID");
            }
            if (!dtTempExchangeData.Columns.Contains("NEW_PART_NUMBER"))
            {
                dtTempExchangeData.Columns.Add("NEW_PART_NUMBER");
            }
            if (!dtTempExchangeData.Columns.Contains("NEW_WORK_ORDER_NO"))
            {
                dtTempExchangeData.Columns.Add("NEW_WORK_ORDER_NO");
            }
            if (!dtTempExchangeData.Columns.Contains("NEW_WORK_ORDER_KEY"))
            {
                dtTempExchangeData.Columns.Add("NEW_WORK_ORDER_KEY");
            }
            foreach (DataRow drExchangeDataDetail in dtTempExchangeData.Rows)
            {
                //设置明细数据的新工单、产品
                drExchangeDataDetail["NEW_WORK_ORDER_KEY"] = orderKey;
                drExchangeDataDetail["NEW_WORK_ORDER_NO"] = orderNo;
                drExchangeDataDetail["NEW_PART_NUMBER"] = partNumber;
                drExchangeDataDetail["NEW_PRO_ID"] = proId;

                drExchangeDataDetail[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY] = roomKey;
                drExchangeDataDetail[POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME] = roomName;
            }
            if (!CheckExchangeData(dtTempExchangeData))
            {
                return;
            }
            dtTempExchangeData.PrimaryKey=new DataColumn[]{dtTempExchangeData.Columns[POR_LOT_FIELDS.FIELD_LOT_KEY]};
            if (dtExchangeData == null)
            {
                dtExchangeData = dtTempExchangeData.Clone();
                this.gcExchangeWo.MainView = gvExchangeWo;
                this.gcExchangeWo.DataSource = dtExchangeData;
            }

            dtExchangeData.Merge(dtTempExchangeData, true);
            this.gvExchangeWo.BestFitColumns();

            this.txtPalletNo.Select();
            this.txtPalletNo.SelectAll();
        }
        /// <summary>
        /// 检查返工数据。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool CheckExchangeData(DataTable dt)
        {
            decimal minPower = decimal.MinValue;
            decimal maxPower = decimal.MaxValue;
            int digits = 2;
            DataRowView drWoProduct = this.lueNewPartNumber.GetSelectedDataRow() as DataRowView;
            if (drWoProduct != null)
            {
                string sDigits = Convert.ToString(drWoProduct["POWER_DEGREE"]);
                int.TryParse(sDigits, out digits);
                string sMinPower = Convert.ToString(drWoProduct["MINPOWER"]);
                decimal.TryParse(sMinPower, out minPower);
                string sMaxPower = Convert.ToString(drWoProduct["MAXPOWER"]);
                decimal.TryParse(sMaxPower, out maxPower);
            }

            //检查返工单批次是否符合工单及其产品要求。
            foreach (DataRow drExchangeDetail in dt.Rows)
            {
                string lotNumber = Convert.ToString(drExchangeDetail[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                string newOrderKey = Convert.ToString(drExchangeDetail["NEW_WORK_ORDER_KEY"]);
                string newOrderNumber = Convert.ToString(drExchangeDetail["NEW_WORK_ORDER_NO"]);
                string newPartNumber = Convert.ToString(drExchangeDetail["NEW_PART_NUMBER"]);
                string newProductId = Convert.ToString(drExchangeDetail["NEW_PRO_ID"]);
                string ttime=Convert.ToString(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_T_DATE]);
                string grade = Convert.ToString(drExchangeDetail[POR_LOT_FIELDS.FIELD_PRO_LEVEL]);
                //无等级数据不需要判断等级。
                if (!string.IsNullOrEmpty(grade))
                {
                    #region 判断组件是否符合工单产品的等级要求
                    bool bCompare = this._lotEntity.CompareExchangeGrade(newOrderNumber, newPartNumber, grade);
                    if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                    {
                        MessageService.ShowError(this._lotEntity.ErrorMsg);
                        return false;
                    }
                    if (bCompare == false)
                    {
                        MessageService.ShowError(string.Format("组件({0})不符合工单产品({1}-{2})的等级要求，请联系工艺确认。",
                                                                lotNumber, newOrderNumber, newPartNumber));
                        return false;
                    }
                    #endregion
                }
                //无测试数据，不需要判断分档。
                if (!string.IsNullOrEmpty(ttime))
                {
                    #region 产品功率校验。
                    decimal pm = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_PM]);
                    decimal isc = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_ISC]);
                    decimal voc = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_VOC]);
                    decimal ipm = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_IPM]);
                    decimal vpm = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_VPM]);
                    decimal ff = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_FF]);

                    decimal coefPM = Math.Round(pm, digits, MidpointRounding.AwayFromZero);
                    decimal coefISC = isc;
                    decimal coefVOC = voc;
                    decimal coefIPM = ipm;
                    decimal coefVPM = vpm;
                    decimal coefFF = ff;

                    if (drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] != DBNull.Value
                         && drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] != null)
                    {
                        coefPM = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]);
                    }

                    if (drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_ISC] != DBNull.Value
                        && drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_ISC] != null)
                    {
                        coefISC = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_ISC]);
                    }

                    if (drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VOC] != DBNull.Value
                        && drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VOC] != null)
                    {
                        coefVOC = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VOC]);
                    }

                    if (drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] != DBNull.Value
                        && drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] != null)
                    {
                        coefIPM = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_IMAX]);
                    }

                    if (drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VMAX] != DBNull.Value
                        && drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VMAX] != null)
                    {
                        coefVPM = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VMAX]);
                    }

                    if (drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_FF] != DBNull.Value
                        && drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_FF]!=null)
                    {
                        coefFF = Convert.ToDecimal(drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_FF]);
                    }

                    #region   //判断组件是否符合工单产品的功率要求 //判断组件是否符合工单产品的分档要求
                    //DataView dvDecoeffiData = null;
                    //DataSet dsDecoeffiData = this._lotEntity.GetDecayCoefficient(newOrderNumber, newPartNumber, pm);
                    //if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                    //{
                    //    MessageService.ShowError(string.Format("组件({0})获取工单({1})产品({2}-{3})衰减数据出错：{4}",
                    //                                           lotNumber,
                    //                                           newOrderNumber,
                    //                                           newPartNumber,
                    //                                           newProductId,
                    //                                           this._lotEntity.ErrorMsg));
                    //    return false;
                    //}
                    //dvDecoeffiData = dsDecoeffiData.Tables[0].DefaultView;
                    //dvDecoeffiData.Sort = "COEFFICIENT DESC";
                    //int rowIndex = 0;
                    string powersetSubWay = string.Empty;
                    string psKey = string.Empty;
                    while (true)
                    {
                        //#region 计算衰减数据
                        //dvDecoeffiData.RowFilter = "D_NAME='PMAX'";
                        //if (dvDecoeffiData.Count >= 1)
                        //{
                        //    decimal dCoeff = Convert.ToDecimal(dvDecoeffiData[rowIndex]["COEFFICIENT"]);
                        //    coefPM = Math.Round(pm * dCoeff, digits, MidpointRounding.AwayFromZero);
                        //}
                        //dvDecoeffiData.RowFilter = "D_NAME='ISC'";
                        //if (dvDecoeffiData.Count >= 1)
                        //{
                        //    decimal dCoeff = Convert.ToDecimal(dvDecoeffiData[rowIndex]["COEFFICIENT"]);
                        //    coefISC = isc * dCoeff;
                        //}
                        //dvDecoeffiData.RowFilter = "D_NAME='IMAX'";
                        //if (dvDecoeffiData.Count >= 1)
                        //{
                        //    decimal dCoeff = Convert.ToDecimal(dvDecoeffiData[rowIndex]["COEFFICIENT"]);
                        //    coefIPM = ipm * dCoeff;
                        //}

                        //dvDecoeffiData.RowFilter = "D_NAME='VOC'";
                        //if (dvDecoeffiData.Count >= 1)
                        //{
                        //    decimal dCoeff = Convert.ToDecimal(dvDecoeffiData[rowIndex]["COEFFICIENT"]);
                        //    coefVOC = voc * dCoeff;
                        //}

                        //dvDecoeffiData.RowFilter = "D_NAME='VMAX'";
                        //if (dvDecoeffiData.Count >= 1)
                        //{
                        //    decimal dCoeff = Convert.ToDecimal(dvDecoeffiData[rowIndex]["COEFFICIENT"]);
                        //    coefVPM = vpm * dCoeff;
                        //}

                        //dvDecoeffiData.RowFilter = "D_NAME='FF'";
                        //if (dvDecoeffiData.Count >= 1)
                        //{
                        //    decimal dCoeff = Convert.ToDecimal(dvDecoeffiData[rowIndex]["COEFFICIENT"]);
                        //    coefFF = ff * dCoeff;
                        //}
                        //#endregion

                        //rowIndex++;
                        //判断组件是否符合工单产品的功率要求
                        //if (rowIndex >= dvDecoeffiData.Count && (coefPM < minPower || coefPM > maxPower))
                        if ((coefPM < minPower || coefPM > maxPower))
                        {
                            MessageService.ShowError(string.Format("组件（{0})功率({1})不满足产品({2}-{3})要求，请联系工艺确认！",
                                                                    lotNumber, coefPM, newPartNumber, newProductId));
                            return false;
                        }
                        //else if (rowIndex < dvDecoeffiData.Count && (coefPM < minPower || coefPM > maxPower))
                        //{
                        //    continue;
                        //}
                        //判断组件是否符合工单产品的分档要求
                        DataSet dsPowerSet = this._lotEntity.GetWOPowerSetData(newOrderNumber, newPartNumber, lotNumber, coefPM);
                        if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                        {
                            MessageService.ShowError(string.Format("组件({0})获取产品({1}-{2})分档数据出错：{3}",
                                                           lotNumber,
                                                           newPartNumber,
                                                           newProductId,
                                                           this._lotEntity.ErrorMsg));
                            return false;
                        }
                        //if (rowIndex >= dvDecoeffiData.Count && dsPowerSet.Tables[0].Rows.Count <= 0)
                        if (dsPowerSet.Tables[0].Rows.Count <= 0)
                        {
                            MessageService.ShowError(string.Format("组件({0})功率({1})在产品（{2}-{3}）中无对应分档信息！",
                                                                  lotNumber,
                                                                  coefPM,
                                                                  newPartNumber,
                                                                  newProductId));
                            return false;
                        }
                        //else if (rowIndex < dvDecoeffiData.Count && dsPowerSet.Tables[0].Rows.Count <= 0)
                        //{
                        //    continue;
                        //}
                        DataRow dr = dsPowerSet.Tables[0].Rows[0];
                        drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE] = Convert.ToString(dr["PS_CODE"]);
                        drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_I_IDE] = Convert.ToString(dr["PS_SEQ"]);
                        drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_VC_MODNAME] = Convert.ToString(dr["MODULE_NAME"]);

                        powersetSubWay = Convert.ToString(dr["SUB_PS_WAY"]);
                        psKey=Convert.ToString(dr["POWERSET_KEY"]);

                        break;
                    }
                    #endregion

                    DataSet dsPowersetDetail = null;
                    //子分档检验
                    switch (powersetSubWay)
                    {
                        case "功率":
                            dsPowersetDetail = this._lotEntity.GetWOPowerSetDetailData(newOrderKey, newPartNumber, psKey, coefPM);
                            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                            {
                                MessageService.ShowError(string.Format("组件({0})获取产品({1}-{2})子分档数据出错：{3}",
                                                       lotNumber,
                                                       newPartNumber,
                                                       newProductId,
                                                       this._lotEntity.ErrorMsg));
                                return false;
                            }
                            break;
                        case "电流":
                            dsPowersetDetail = this._lotEntity.GetWOPowerSetDetailData(newOrderKey, newPartNumber, psKey,coefIPM);
                            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                            {
                                MessageService.ShowError(string.Format("组件({0})获取产品({1}-{2})子分档数据出错：{3}",
                                                       lotNumber,
                                                       newPartNumber,
                                                       newProductId,
                                                       this._lotEntity.ErrorMsg));
                                return false;
                            }
                            if (dsPowersetDetail.Tables[0].Rows.Count <= 0)
                            {
                                 MessageService.ShowError(string.Format("组件({0})电流[{1}]不满足产品（{2}-{3}）功率({4})对应电流子分档要求，请联系工艺确认！",
                                                                   lotNumber, 
                                                                   coefIPM.ToString("##0.00"),
                                                                   newPartNumber,
                                                                   newProductId,
                                                                   coefPM));
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                    if (dsPowersetDetail != null &&
                        dsPowersetDetail.Tables.Count > 0 &&
                        dsPowersetDetail.Tables[0].Rows.Count > 0)
                    {
                        drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_I_PKID] = Convert.ToInt32(dsPowersetDetail.Tables[0].Rows[0]["PS_SUB_CODE"]);
                    }
                    else{
                        drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_I_PKID] =DBNull.Value;
                    }

                    drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] = coefPM;
                    drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_ISC] = coefISC;
                    drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_IMAX] = coefIPM;
                    drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VOC] = coefVOC;
                    drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_VMAX] = coefVPM;
                    drExchangeDetail[WIP_IV_TEST_FIELDS.FIELDS_COEF_FF] = coefFF;
                    #endregion
                }
            }
            return true;
        }
        /// <summary>
        /// 选择工艺路线。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditRoute_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OperationHelpDialog dlg = new OperationHelpDialog();
            string factoryName = this.lueFactoryRoom.Text;
            dlg.FactoryRoom = factoryName;
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = btnEditRoute;
            dlg.RouteName = txtOperation;
            dlg.StepName = txtStep;
            dlg.IsRework = false;
            Point i = btnEditRoute.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + btnEditRoute.Height);

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
                    dlg.Location = new Point(i.X + btnEditRoute.Width - dlg.Width, i.Y + btnEditRoute.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + btnEditRoute.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 取消按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 保存按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtExchangeData==null || dtExchangeData.Rows.Count == 0)
            {
                MessageService.ShowMessage(string.Format("明细列表为空，请确认待{0}数据。",this._flagName),"提示");
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }
          
            string roomName = this.lueFactoryRoom.Text;
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string remark=this.meRemark.Text.Trim();

            if (string.IsNullOrEmpty(remark))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.Msg003}"), MESSAGEBOX_CAPTION);//请输入备注原因
                //MessageService.ShowMessage("请输入备注原因!","提示");
                this.meRemark.Select();
                this.meRemark.SelectAll();
                return;
            }

            string enterpirseRouteName = this.btnEditRoute.Text;
            string enterpriseKey = Convert.ToString(this.btnEditRoute.Tag);
            string routeName = this.txtOperation.Text;
            string routeKey = Convert.ToString(this.txtOperation.Tag);
            string stepName = this.txtStep.Text;
            string stepKey = Convert.ToString(this.txtStep.Tag);
            if (string.IsNullOrEmpty(stepKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.Msg004}"), MESSAGEBOX_CAPTION);//请选择工艺流程
                //MessageService.ShowMessage("请选择工艺流程。", "提示");
                btnEditRoute.Focus();
                return;
            }
            if ((stepName == "入库检验" || stepName == "入库"))
            {
                MessageService.ShowMessage(string.Format("只能{0}到包装或其以前工序。", this._flagName));
                this.btnEditRoute.Select();
                return;
            }

           
            //检查工艺流程
            foreach(DataRow dr in dtExchangeData.Rows)
            {
                int srcIndex = dtExchangeData.Rows.IndexOf(dr);
                int rowHandle = this.gvExchangeWo.GetRowHandle(srcIndex)+1;
                string lotNumber = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                //等级不存在，转返工工序不能是包装 、入库检、入库
                string grade=Convert.ToString(dr[POR_LOT_FIELDS.FIELD_PRO_LEVEL]);
                if(string.IsNullOrEmpty(grade) && 
                   (stepName=="包装" || stepName=="入库检验" || stepName=="入库"))
                {
                    MessageService.ShowMessage(string.Format("第{0}行组件({1})等级不存在，只能{2}到终检或其以前工序。",
                                                            rowHandle,lotNumber,this._flagName));
                    this.gvExchangeWo.FocusedRowHandle=rowHandle;
                    return;
                }
                //有效测试数据不存在，转返工工序不能是终检、包装 、入库检、入库
                string ttime=Convert.ToString(dr[WIP_IV_TEST_FIELDS.FIELDS_T_DATE]);
                 if(string.IsNullOrEmpty(ttime) &&
                   (stepName == "终检" || stepName == "包装" || stepName == "入库检验" || stepName == "入库"))
                 {
                    MessageService.ShowMessage(string.Format("第{0}行组件({1})等级不存在，只能{2}到组件测试或其以前工序。",
                                                            rowHandle,lotNumber,this._flagName));
                    this.gvExchangeWo.FocusedRowHandle=rowHandle;
                    return;
                }
            }
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            //组织保存数据。
            //将返工明细添加到数据集中。
            DataSet dsParams = new DataSet();
            dsParams.Merge(dtExchangeData, true, MissingSchemaAction.Add);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, enterpriseKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY, routeKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY, stepKey);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_EDITOR, userName);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
            //转返工单标记。
            if (this._flag == ExchangeWoFlag.Repair)
            {
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_PROID);
            }
            else
            {
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_WO);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Merge(dtParams, true, MissingSchemaAction.Add);

            DataSet dsReturn = this._lotEntity.LotExchange(dsParams);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return;
            }
            MessageService.ShowMessage(string.Format("{0}操作成功。",this._flagName), "提示");
            if (dtExchangeData != null)
            {
                dtExchangeData.Clear();
                dtExchangeData = null;
            }
        }

        /// <summary>
        /// 是以批次返工还是以托号返工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPalletLot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsPallet.Checked == true)
            {
                this.lciPalletNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolLotNumber}");//"批次号";
            }
            if (chkIsPallet.Checked == false)
            {
                lciPalletNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchgWoMultiForm.gcolPalletNo}");//"托号";
            }
        }
        /// <summary>
        /// 自定义显示行号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvExchangeWo_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
               
    }
}