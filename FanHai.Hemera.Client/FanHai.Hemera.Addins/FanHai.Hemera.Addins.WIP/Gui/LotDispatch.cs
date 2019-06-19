using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EAP;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using System.Collections;
using System.Linq;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Base;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示工作站作业的控件类。
    /// </summary>
    public partial class LotDispatch : BaseUserCtrl
    {
        InputDevice id;
        string device;

        private  string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        string FINAL_CHECK = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.FINAL_CHECK}"); //终检
        string CUSTOMER_CHECK = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.CUSTOMER_CHECK}"); //客检

        IViewContent _view = null;
        LotDispatchDetailModel _model = null;
        bool _isOnLoading = true;                                               //是否是正在加载中。

        LotOperationEntity _entity = new LotOperationEntity();
        ComputerEntity _computerEntity = new ComputerEntity();

        WorkOrders _workOrder = new WorkOrders();
        /// <summary>
        /// 是否显示选择设备的对话框。
        /// </summary>
        bool _isShowPickEquipmetDialog = false;
        /// <summary>
        /// 是否检查固化周期。
        /// </summary>
        bool _isCheckFixCycle = false;
        /// <summary>
        /// 是否检查恒温周期。
        /// </summary>
        bool _isCheckConstantTemperatureCycle = false;
        /// <summary>
        /// 是否检查校准板周期。
        /// </summary>
        bool _isCheckCalibrationCycle = false;
        /// <summary>
        /// 进站时检查数量是否和创批时数量一样，不满足条件不允许进站。
        /// </summary>
        bool _isCheckQuantityAtTrackIn = false;
        /// <summary>
        /// 出站时检查数量是否和创批时数量一样，不满足条件不允许出站。
        /// </summary>
        bool _isCheckQuantityAtTrackOut = false;
        /// <summary>
        /// 是否是入库工序。
        /// </summary>
        bool _isToWarehouseOperation = false;
        /// <summary>
        /// 是否为入库检验工序
        /// </summary>
        bool _IsToWarehouseCheckOperation = false;
        //是否为包装工序
        bool _IsToPackageOperation = false;
        /// <summary>
        /// 出站时是否检查IV测试数据，如果不满足不允许出站。
        /// </summary>
        bool _isCheckIVTestData = false;
        /// <summary>
        /// 出站时是否检查EL图片，如果没有EL图片不允许出站。
        /// </summary>
        bool _isCheckELPicture = false;
        /// <summary>
        /// 是否检查电池片信息，如果电池片信息没有输入，则在出站时必须输入电池片信息。
        /// </summary>
        bool _isCheckSILot = false;
        /// <summary>
        /// 是否对批次线别绑定进行检查，如果检查绑定线别和当前线别不一致，则不允许进站
        /// </summary>
        bool _isCheckLotLine = false;
        /// <summary>
        /// 是否根据工单检查颜色。如果颜色信息没有输入且工单设置了必须输入颜色，则颜色必须输入。
        /// </summary>
        bool _isCheckColorByWorkOrder = false;
        /// <summary>
        /// 是否必须输入花色。
        /// </summary>
        bool _isMustInputColor = false;
        /// <summary>
        /// 是否显示设备下拉框。默认为true 显示
        /// </summary>
        bool _isShowEquipmentDropdownList = true;
        /// <summary>
        /// 是否显示设置新的工艺流程选择的栏位。
        /// </summary>
        bool _isShowSetNewRoute = false;
        /// <summary>
        /// 是否显示打印标签的对话框。
        /// </summary>
        bool _isShowPrintLabelDialog = false;
        /// <summary>
        /// 是否检查组件工单和前一次刷入工单的异同，如果不相同给出提示，如果相同不做任何提示。
        /// </summary>
        bool _isCheckWorkOrderNoDifferent = false;
        /// <summary>
        /// 允许的创建批次的最大周期时间。
        /// </summary>
        double _createLotTimeMaxCycle = double.MaxValue;
        /// <summary>
        /// 批量过站数量
        /// </summary>
        int _batchTrackCount = 1;
        /// <summary>
        /// 是否卡条码录入方式。
        /// </summary>
        bool bCheckbarcode = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">工作站作业参数数据。。</param>
        /// <param name="view">视图对象。</param>
        public LotDispatch(LotDispatchDetailModel model, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;

            id = new InputDevice(Handle);
            id.EnumerateDevices();
            id.KeyPressed += new InputDevice.DeviceEventHandler(m_KeyPressed);

            InitializeLanguage();
        }


        public void InitializeLanguage()
        {
            this.btnRemove.Text =StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.btnRemove}");//移除
            //this.btnClose.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.btnClose}"); //退出作业
            //this.btnOk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.btnOk}");//开始作业
            this.btnClose.Text = "关闭"; //
            this.btnOk.Text = "过账";//
            this.gcolSeqNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolSeqNum}");//序号
            this.gcolLotNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolLotNumber}");//序列号
            this.gcolWorkorderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolWorkorderNo}");//工单号
            this.gcolProID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolProID}");//产品ID号
            this.gcolRouteName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolRouteName}");//工艺流程名
            this.gcolQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolQty}");//电池片数量
            this.gcolStepName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolStepName}"); //工序名
            this.gcolEfficency.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolEfficency}");//转换效率
            this.gcolSILot.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolSILot}");//电池片信息
            this.gcolStateFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.gcolStateFlag}");//批次状态
            this.lciLotNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciLotNumber}");//序列号
            this.lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciOperation}");//工序
            this.lciEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciEquipment}");//设备
            this.lciEquipmentState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciEquipmentState}");//设备状态
            this.lciLine.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciLine}");//线别
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciFactoryRoom}");//工厂车间
            this.lciLotInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciLotInfo}");//批次列表";
            this.lciRemove.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lciRemove}");//移除按钮";
        }

        
        
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        private void LotDispatch_Load(object sender, EventArgs e)
        {
            this._isOnLoading = true;
            ShowHintMessage(string.Empty);
            BindFactoryRoom();
            BindOperations();
            BindLine();
            BindEquipment();
            ClearLotInfo();
            this._isOnLoading = false;
            HideControl();
            InitializeComponentText();
            lblMenu.Text = "生产管理>过站管理>过站";

            string sFactoryName = lueFactoryRoom.Text.Trim();
            LotQueryEntity LotQueryObject = new LotQueryEntity();
            DataSet dsCheckbarcodeInfo = LotQueryObject.GetCheckbarcodeInputType(sFactoryName);
            if (dsCheckbarcodeInfo.Tables[0].Rows.Count > 0)
            {
                string sCheckType = dsCheckbarcodeInfo.Tables[0].Rows[0]["IsCheckBarcodeInput"].ToString().Trim();
                if (sCheckType.ToUpper() == "TRUE")
                {
                    bCheckbarcode = true;
                }
            }
        }
        /// <summary>
        /// 初始化窗体组件上的文本。
        /// </summary>
        private void InitializeComponentText()
        {
            if (this._model != null)
            {
                this.lueFactoryRoom.EditValue = this._model.RoomKey;
                this.cbOperation.EditValue = this._model.OperationName;
                this.lueEquipment.EditValue = this._model.EquipmentKey;
                this.teLotNumber.Select();
            }
            else
            {
                this._model = new LotDispatchDetailModel();
                if (this.cbOperation.Properties.Items.Count > 1)
                {
                    this.cbOperation.Select();
                }
                else
                {
                    if (this.lueEquipment.EditValue != null)
                    {
                        this.teLotNumber.Select();
                    }
                    else
                    {
                        this.lueEquipment.Select();
                    }
                }
            }
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
                    this.lueFactoryRoom.ItemIndex = 0;
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cbOperation.Properties.Items.Add(strOperations[i]);
                }
                this.cbOperation.SelectedIndex = 0;
            }
            SetOperationAtrributeValue();
        }
        /// <summary>
        /// 绑定线别。
        /// </summary>
        private void BindLine()
        {
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue.ToString();
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            this.lueLine.EditValue = string.Empty;
            Line entity = new Line();
            DataSet ds = entity.GetLinesInfo(strFactoryRoomKey, strLines);
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueLine.Properties.DataSource = ds.Tables[0];
                this.lueLine.Properties.DisplayMember = "LINE_NAME";
                this.lueLine.Properties.ValueMember = "PRODUCTION_LINE_KEY";
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
        }
        /// <summary>
        /// 绑定设备。
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cbOperation.Text.Trim();
            string strFactoryRoomName = this.lueFactoryRoom.Text;
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue == null ? string.Empty : this.lueFactoryRoom.EditValue.ToString();
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            //如果工厂车间或者工序或者线别主键为空。
            if (string.IsNullOrEmpty(strFactoryRoomName)
                || string.IsNullOrEmpty(strOperation)
                || string.IsNullOrEmpty(strLines))
            {
                return;
            }
            this.lueEquipment.EditValue = string.Empty;
            this.lueEquipment.Properties.ReadOnly = false;

            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, strLines.Split(','));
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueEquipment.Properties.DataSource = ds.Tables[0];
                this.lueEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE;
                this.lueEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;

            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
            if (this._isShowPickEquipmetDialog)
            {
                ShowEquipmentPickDialog(ds);
            }
            SetEquipmentState();
            SetLineValue();
        }
        /// <summary>
        /// 隐藏控件。
        /// </summary>
        private void HideControl()
        {
            if (this.lueFactoryRoom.Properties.DataSource != null)
            {
                DataTable dt = this.lueFactoryRoom.Properties.DataSource as DataTable;
                if (dt != null && dt.Rows.Count == 1)
                {
                    this.lciFactoryRoom.Visibility = LayoutVisibility.Never;
                }
            }
            if (this.cbOperation.Properties.Items.Count <= 1)
            {
                this.cbOperation.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 显示提示信息。
        /// </summary>
        /// <param name="message">提示信息。</param>
        private void ShowHintMessage(string message)
        {
            this.lblHintMessage.Text = message;
            if (string.IsNullOrEmpty(message))
            {
                this.lciHintMessage.Visibility = LayoutVisibility.Never;
            }
            else
            {
                this.lciHintMessage.Visibility = LayoutVisibility.Always;
            }
        }
        /// <summary>
        /// 工序改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._isOnLoading) return;
            this._isOnLoading = true;
            SetOperationAtrributeValue();
            //重新绑定设备控件
            BindEquipment();
            ClearLotInfo();
            this._isOnLoading = false;
        }
        /// <summary>
        /// 设置工序自定义属性值。
        /// </summary>
        private void SetOperationAtrributeValue()
        {
            string operationName = this.cbOperation.Text.Trim();
            if (string.IsNullOrEmpty(operationName)) return;
            this._isShowPickEquipmetDialog = false;
            this._isCheckFixCycle = false;
            this._isCheckConstantTemperatureCycle = false;
            this._isCheckCalibrationCycle = false;
            this._isCheckQuantityAtTrackIn = false;
            this._isCheckQuantityAtTrackOut = false;
            this._isToWarehouseOperation = false;
            this._IsToWarehouseCheckOperation = false;
            this._IsToPackageOperation = false;
            this._isCheckSILot = false;
            this._isCheckELPicture = false;
            this._isCheckIVTestData = false;
            this._isShowSetNewRoute = false;
            this._isShowPrintLabelDialog = false;
            this._isCheckColorByWorkOrder = false;
            this._isMustInputColor = false;
            this._isCheckWorkOrderNoDifferent = false;
            this._isShowEquipmentDropdownList = true;
            this._batchTrackCount = 1;

            RouteQueryEntity routeEntity = new RouteQueryEntity();
            DataSet dsReturn = routeEntity.GetMaxVersionOperationAttrInfo(operationName, string.Empty);
            if (string.IsNullOrEmpty(routeEntity.ErrorMsg)
                && dsReturn.Tables.Count > 0
                && dsReturn.Tables[0].Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> rowCollection = dsReturn.Tables[0].AsEnumerable();
                //是否显示选择设备对话框。
                DataRow drCurrent = rowCollection
                                    .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                                    == ROUTE_OPERATION_ATTRIBUTE.IsShowPickEquipmentDialog)
                                    .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isShowPickEquipmetDialog))
                    {
                        this._isShowPickEquipmetDialog = false;
                    }
                }
                //是否检查校准版周期。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckCalibrationCycle)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckCalibrationCycle))
                    {
                        this._isCheckCalibrationCycle = false;
                    }
                }
                //是否检查固化周期。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckFixCycle)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckFixCycle))
                    {
                        this._isCheckFixCycle = false;
                    }
                }
                //是否检查恒温周期。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckConstantTemperatureCycle)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckConstantTemperatureCycle))
                    {
                        this._isCheckConstantTemperatureCycle = false;
                    }
                }
                //进站时检查数量是否和创批时数量一样，不满足条件不允许进站。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckQuantityAtTrackIn)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckQuantityAtTrackIn))
                    {
                        this._isCheckQuantityAtTrackIn = false;
                    }
                }
                //出站时检查数量是否和创批时数量一样，不满足条件不允许出站。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckQuantityAtTrackOut)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckQuantityAtTrackOut))
                    {
                        this._isCheckQuantityAtTrackOut = false;
                    }
                }
                //是否是包装工序。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsToPackageOperation)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._IsToPackageOperation))
                    {
                        this._IsToPackageOperation = false;
                    }
                }
                //是否是入库检验工序。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsToWarehouseCheckOperation)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._IsToWarehouseCheckOperation))
                    {
                        this._IsToWarehouseCheckOperation = false;
                    }
                }
                //是否是入库工序。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsToWarehouseOperation)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isToWarehouseOperation))
                    {
                        this._isToWarehouseOperation = false;
                    }
                }

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
                //是否根据工单检查颜色。如果颜色信息没有输入且工单设置了必须输入颜色，则颜色必须输入。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckColorByWorkOrder)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckColorByWorkOrder))
                    {
                        this._isCheckColorByWorkOrder = false;
                    }
                }
                //出站时是否检查IV测试数据，如果不满足不允许出站。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckIVTestData)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckIVTestData))
                    {
                        this._isCheckIVTestData = false;
                    }
                }
                //出站时是否检查EL图片，如果没有EL图片不允许出站。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckELPicture)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckELPicture))
                    {
                        this._isCheckELPicture = false;
                    }
                }
                //出站时可以设置新的工艺流程
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsShowSetNewRoute)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isShowSetNewRoute))
                    {
                        this._isShowSetNewRoute = false;
                    }
                }
                //出站显示打印标签的对话框
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsShowPrintLabelDialog)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isShowPrintLabelDialog))
                    {
                        this._isShowPrintLabelDialog = false;
                    }
                }
                //批量过站数量
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.BatchTrackCount)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !int.TryParse(attrValue, out this._batchTrackCount))
                    {
                        this._batchTrackCount = 1;
                    }
                }
                //是否检查创建批次时间。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.CreateLotMaxCycle)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !double.TryParse(attrValue, out this._createLotTimeMaxCycle))
                    {
                        this._createLotTimeMaxCycle = double.MaxValue;
                    }
                }

                //是否检查组件工单和前一次刷入工单的异同，如果不相同给出提示，如果相同不做任何提示。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsCheckWorkOrderNoDifferent)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isCheckWorkOrderNoDifferent))
                    {
                        this._isCheckWorkOrderNoDifferent = false;
                    }
                }
                //是否显示设备选择下拉框。
                drCurrent = rowCollection
                            .Where(dr => Convert.ToString(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME])
                                                            == ROUTE_OPERATION_ATTRIBUTE.IsShowEquipmentDropdownList)
                            .SingleOrDefault();
                if (drCurrent != null)
                {
                    string attrValue = Convert.ToString(drCurrent[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                    if (string.IsNullOrEmpty(attrValue)
                        || !bool.TryParse(attrValue, out this._isShowEquipmentDropdownList))
                    {
                        this._isShowEquipmentDropdownList = true;
                    }
                }
            }
            SetVisibilityLayout();
        }

        /// <summary>
        /// 根据包装，入库检，入库 作业是否显示批次录入框
        /// 根据属性设置决定是否显示控件。
        /// </summary>
        private void SetVisibilityLayout()
        {
            bool bv = false;
            if (this._isToWarehouseOperation)
                bv = this._isToWarehouseOperation;
            if (this._IsToWarehouseCheckOperation)
                bv = this._IsToWarehouseCheckOperation;
            if (this._IsToPackageOperation)
                bv = this._IsToPackageOperation;

            if (this._batchTrackCount > 1)
            {
                this.lciRemove.Visibility = LayoutVisibility.Always;
                this.btnRemove.Visible = true;
                this.lciLotInfo.Visibility = LayoutVisibility.Always;
                this.gcLotInfo.Visible = true;
            }
            else
            {
                this.lciRemove.Visibility = LayoutVisibility.Never;
                this.btnRemove.Visible = false;
                this.lciLotInfo.Visibility = LayoutVisibility.Never;
                this.gcLotInfo.Visible = false;
            }

            this.teLotNumber.Visible = !bv;
            this.lciLotNumber.Visibility = bv ? LayoutVisibility.Never : LayoutVisibility.Always;
            //this.lcgLotNumber.Visibility = bv ? LayoutVisibility.Never : LayoutVisibility.Always;
            this.lueEquipment.Properties.Buttons[0].Visible = this._isShowEquipmentDropdownList;
        }
        /// <summary>
        /// 工厂车间改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            if (this._isOnLoading) return;
            this._isOnLoading = true;
            BindLine();
            //重新绑定设备控件
            BindEquipment();
            ClearLotInfo();
            this._isOnLoading = false;

            string sFactoryName = lueFactoryRoom.Text.Trim();
            LotQueryEntity LotQueryObject = new LotQueryEntity();
            DataSet dsCheckbarcodeInfo = LotQueryObject.GetCheckbarcodeInputType(sFactoryName);
            if (dsCheckbarcodeInfo.Tables[0].Rows.Count > 0)
            {
                string sCheckType = dsCheckbarcodeInfo.Tables[0].Rows[0]["IsCheckBarcodeInput"].ToString().Trim();
                if (sCheckType.ToUpper() == "TRUE")
                {
                    bCheckbarcode = true;
                }
            }
        }
        /// <summary>
        /// 设备改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            if (this._isOnLoading) return;
            this._isOnLoading = true;
            //设置设备状态。
            SetEquipmentState();
            SetLineValue();
            ClearLotInfo();
            //this.teLotNumber.Select();
            //this.teLotNumber.SelectAll();
            this._isOnLoading = false;
        }
        /// <summary>
        /// 设置设备状态。
        /// </summary>
        private void SetEquipmentState()
        {
            string equipmentKey = Convert.ToString(this.lueEquipment.GetColumnValue("EQUIPMENT_REAL_KEY"));
            if (string.IsNullOrEmpty(equipmentKey))
            {
                equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            }
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                EquipmentEntity entity = new EquipmentEntity();
                DataSet ds = entity.GetEquipmentState(equipmentKey);
                if (string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    string description = Convert.ToString(ds.Tables[0].Rows[0]["DESCRIPTION"]);
                    string stateName = Convert.ToString(ds.Tables[0].Rows[0]["EQUIPMENT_STATE_NAME"]);
                    this.txtEquipmentState.Text = ds.Tables[0].Rows.Count > 0
                            ? string.Format("{0}({1})", stateName, description)
                            : string.Empty;
                    System.Drawing.Color backColor = FanHai.Hemera.Utils.Common.Utils.GetEquipmentStateColor(stateName);
                    this.txtEquipmentState.BackColor = backColor;
                }
                else
                {
                    this.txtEquipmentState.Text = string.Empty;
                    this.txtEquipmentState.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                this.txtEquipmentState.Text = string.Empty;
            }
        }
        /// <summary>
        /// 设置线别的值。
        /// </summary>
        private void SetLineValue()
        {
            string lineKey = Convert.ToString(this.lueEquipment.GetColumnValue("LINE_KEY"));
            this.lueLine.EditValue = lineKey;
        }
        /// <summary>
        /// 设备单击时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_Click(object sender, EventArgs e)
        {
            this.lueEquipment.SelectAll();
            //工序自定义属性 IS_SHOW_PICK_EQUIPMENT_DIALOG 确定是否显示设备选择对话框。
            if (this._isShowPickEquipmetDialog)
            {
                DataTable dt = this.lueEquipment.Properties.DataSource as DataTable;
                DataSet ds = new DataSet();
                ds.Merge(dt);
                ShowEquipmentPickDialog(ds);
                SetEquipmentState();
                SetLineValue();
            }
        }
        /// <summary>
        /// 显示设备选择对话框。
        /// </summary>
        private void ShowEquipmentPickDialog(DataSet ds)
        {
            this.lueEquipment.Properties.ReadOnly = true;
            //显示选择设备的对话框
            EquipmentPickDialog dlg = new EquipmentPickDialog(ds);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.SelectedEquipmentData != null && dlg.SelectedEquipmentData.Length > 0)
                {
                    string equipmentKey = Convert.ToString(dlg.SelectedEquipmentData[0]); //设备主键
                    this.lueEquipment.EditValue = equipmentKey;
                }
            }
            this.teLotNumber.Select();
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 显示批次选择对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teLotNumber_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            LotQueryHelpModel model = new LotQueryHelpModel();
            model.RoomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            model.OperationType = LotOperationType.Dispatch;
            model.OperationName = this.cbOperation.Text;
            LotQueryHelpDialog dlg = new LotQueryHelpDialog(model);
            dlg.OnValueSelected += new LotQueryValueSelectedEventHandler(LotQueryHelpDialog_OnValueSelected);
            Point i = teLotNumber.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = teLotNumber.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + teLotNumber.Height);

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
                    dlg.Location = new Point(i.X + teLotNumber.Width - dlg.Width, i.Y + teLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + teLotNumber.Width - dlg.Width, i.Y - dlg.Height);
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
            this.teLotNumber.Text = args.LotNumber;
        }
        /// <summary>
        /// 批次号按下去时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (device != "HID Keyboard Device")
            //{
            //    e.Handled = true;
            //}
            if (bCheckbarcode)
            {
                if (device != "HID Keyboard Device")
                {
                    e.Handled = true;
                }
            }

            //如果是回车按键。
            if (e.KeyChar == 13)
            {
                try
                {
                    DispatchLot(false);
                }
                finally
                {
                    this.teLotNumber.Select();
                    this.teLotNumber.SelectAll();
                }
            }
        }
        /// <summary>
        /// 开始作业按钮的事件处理方法。用于调度批次作业，包括批次的TrackIn，TrackOut等。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDispatch_Click(object sender, EventArgs e)
        {
            try
            {
                DispatchLot(true);
            }
            finally
            {
                this.teLotNumber.Select();
                this.teLotNumber.SelectAll();
            }
        }
        /// <summary>
        /// 根据批次状态转到不同的操作，如果状态是等待进站则显示进站对话框，如果是等待出站则显示出站对话框。
        /// </summary>
        /// <param name="bForce">true:是。false：否。是否强制开始作业。</param>
        private void DispatchLot(bool bForce)
        {
            ShowHintMessage(string.Empty);

            //检查当前电脑是否和选择设备进行绑定
            if (!CheckComputerEquipment())
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg001}"), MESSAGEBOX_CAPTION); //该计算机未绑定当前选中线别，请确认！
                lueLine.Select();
                return;
            }


            #region 批次操作前检查
            //检查服务器端是否停止服务或者程序版本是否有效。
            switch (FanHai.Hemera.Utils.Common.Utils.CheckServerStopServiceOrVersionInvalid())
            {
                case 0:
                    break;
                case 1:
                    //盘点卡控，不能执行进出站操作
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg002}"), MESSAGEBOX_CAPTION);//盘点时间，不能进行操作
                    return;
                case 2:
                    //程序版本太低
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg003}"), MESSAGEBOX_CAPTION);//版本太低，不能进行操作
                    return;
                default:
                    break;
            }

            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string roomName = Convert.ToString(this.lueFactoryRoom.Text);
            string operationName = this.cbOperation.Text;
            string equipmentKey = Convert.ToString(this.lueEquipment.GetColumnValue("EQUIPMENT_REAL_KEY"));
            if (string.IsNullOrEmpty(equipmentKey))
            {
                equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            }
            //车间不能为空。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                return;
            }
            //工序不能为空。
            if (string.IsNullOrEmpty(operationName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg005}"), MESSAGEBOX_CAPTION);//工序不能为空
                return;
            }
            //获取班别。
            Shift shift = new Shift();
            string shiftName = shift.GetCurrShiftName();
            string shiftKey = string.Empty;

            #endregion

            this._model.OperationName = operationName;
            this._model.RoomKey = roomKey;
            this._model.RoomName = roomName;
            this._model.ShiftName = shiftName;
            this._model.EquipmentKey = equipmentKey;
            this._model.EquipmentName = this.lueEquipment.Text;
            this._model.LineKey = Convert.ToString(this.lueLine.EditValue);
            this._model.LineName = this.lueLine.Text;
            this._model.UserName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this._model.IsCheckSILot = this._isCheckSILot;
            this._model.IsCheckColorByWorkOrder = this._isMustInputColor;
            this._model.IsShowSetNewRoute = this._isShowSetNewRoute;
            this._model.IsShowPrintLabelDialog = this._isShowPrintLabelDialog;
            //包装\入库检验\入库作业时必须需要选择设备。
            if ((this._IsToPackageOperation
                || this._IsToWarehouseCheckOperation
                || this._isToWarehouseOperation)
                && string.IsNullOrEmpty(this._model.EquipmentKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg006}"), MESSAGEBOX_CAPTION);//设备不能为空
                this.lueEquipment.Select();
                return;
            }

            #region 工单线别绑定查询判断

            bool isAllowLine = true;
            isAllowLine = _workOrder.CheckWorkOrderLineBind(_model.LotNumber, _model.LineKey);

            if (!isAllowLine)
            {
 
                MessageService.ShowMessage(_model.LotNumber+ StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg007}"), MESSAGEBOX_CAPTION);//对应工单未绑定选定线别，如果需要请联系工艺进行工单对应线别维护

                this.lueEquipment.Select();
                return;
            }

            #endregion

            //包装工序作业
            if (this._IsToPackageOperation)
            {
                ToPackage(this._model);
                return;
            }
            //入库检验工序
            if (this._IsToWarehouseCheckOperation)
            {
                ToStoreCheck(this._model);
                return;
            }
            //入库作业。显示入库作业详细界面。
            if (this._isToWarehouseOperation)
            {
                ToStore(this._model);
                return;
            }
            DataTable dtLotInfo = this.gcLotInfo.DataSource as DataTable;
            string lotNumber = this.teLotNumber.Text.Trim();

            //序列号为空，批次列表中有数据，且强制开始作业。
            if (string.IsNullOrEmpty(lotNumber)
                && dtLotInfo != null
                && dtLotInfo.Rows.Count > 0
                && bForce)
            {
                ShowDispatchViewContent(this._model, dtLotInfo);
                return;
            }
            //序列号是否为空。
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg008}"), MESSAGEBOX_CAPTION);//序列号不能为空
                return;
            }
            //批量过站，判断批次信息是否在过站列表中存在。
            if (this._batchTrackCount > 1
                && dtLotInfo != null
                && dtLotInfo.Rows.Count > 0)
            {
                DataRow[] drs = dtLotInfo.Select(string.Format("{0}='{1}'", POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNumber));
                if (drs.Length > 0)
                {
                    this.gvLotInfo.FocusedRowHandle = dtLotInfo.Rows.IndexOf(drs[0]);
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg009}"), MESSAGEBOX_CAPTION);//序列号在列表中已存在，请重试
                    return;
                }
            }
            //获取序列号信息。
            LotQueryEntity queryEntity = new LotQueryEntity();
            DataSet dsLot = queryEntity.GetLotInfo(lotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                return;
            }
            if (null == dsLot || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg010}"), MESSAGEBOX_CAPTION);//序列号不存在
                teLotNumber.SelectAll();
                return;
            }

            this._model.LotEditTime = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME]);
            this._model.LotNumber = lotNumber;
            string curRoomKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            string curRoomName = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME]);
            string curOperationName = Convert.ToString(dsLot.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);

            string lotMainLineKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_LINE_KEY]);
            string lotLineCode = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_LINE_CODE]);

            int deleteFlag = Convert.ToInt32(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            int holdFlag = Convert.ToInt32(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            double quantity = Convert.ToDouble(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);
            double initQuantity = Convert.ToDouble(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
            string stateFlag = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG]);

            #region 批次过站前检查
            //批次状态为空。
            if (string.IsNullOrEmpty(stateFlag))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg011}"), MESSAGEBOX_CAPTION);//批次状态为空
                return;
            }
            //判断批次是否结束或被删除。
            if (deleteFlag == 1 || deleteFlag == 2)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg012}"), MESSAGEBOX_CAPTION);//批次已经结束或已删除
                return;
            }
            //判断批次是否暂停
            if (holdFlag == 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg013}"), MESSAGEBOX_CAPTION);//批次已暂停
                return;
            }
            LotStateFlag lotStateFlag = (LotStateFlag)(Convert.ToInt32(stateFlag));
            //批次状态已完成
            if (lotStateFlag >= LotStateFlag.Finished)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg014}"), MESSAGEBOX_CAPTION);//该批次已完成
                return;
            }
            //检查批次所在的工序是否是当前选定工序。
            if (curRoomKey != roomKey || curOperationName != operationName)
            {
                MessageService.ShowMessage(string.Format("组件当前在[{0}]的[{1}]工序，请确认。", curRoomName, curOperationName), MESSAGEBOX_CAPTION);
                return;
            }
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);//获取有权限的所有工序
            //检查登录用户对批次所在的工序是否拥有权限。 
            if ((operations + ",").IndexOf(curOperationName + ",") == -1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg015}"), MESSAGEBOX_CAPTION);//对不起，您没有权限操作
                return;
            }
            //如果数量为空，则结束批次。
            if (quantity == 0)
            {
                this._model.OperationType = LotOperationType.Terminal;
                TerminalLotDialog terminalLot = new TerminalLotDialog(this._model);
                //显示结束批次的对话框。
                terminalLot.ShowDialog();
                return;
            }
            if (lotStateFlag == LotStateFlag.Finished)//批次状态是完成
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg016}"), MESSAGEBOX_CAPTION);//该批次已经结束
                return;
            }

            #region 批次线别绑定检查
            //判断是否绑定了线别
            if (!string.IsNullOrEmpty(lotMainLineKey))
            {
                //判断绑定线别是否和当前选择线别一致
                if (!lotMainLineKey.Equals(_model.LineKey))
                {
                    LotOperationEntity lotOperationEntity = new LotOperationEntity();

                    //获取组件对应工步的属性信息
                    DataSet dsLotStepAttr = lotOperationEntity.GetLotRouteAttrData(lotNumber);

                    //获取组件工步中 IsCheckLotOperationLine 的设定值
                    DataRow[] drsLotStepAttr = dsLotStepAttr.Tables["LOT_ROUTE_ATTR"].Select(" ATTRIBUTE_NAME = 'IsCheckLotOperationLine'");

                    //设置批次工步线别检查的默认值
                    bool isCheckLotOperationLine = true;

                    //检查是否有设定批次工步检查，存在的话 对设定进行更新
                    if (drsLotStepAttr.Length == 1)
                    {
                        if (bool.TryParse(drsLotStepAttr[0][POR_LOT_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString(), out this._isCheckLotLine))
                        {
                            isCheckLotOperationLine = bool.Parse(drsLotStepAttr[0][POR_LOT_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString());
                        }
                    }

                    //判断当前组件对应的工序是否进行批次线别的卡控
                    if (isCheckLotOperationLine)
                    {

                        string[] columns = new string[] { "FactoryName", "IsCheckLotLine" };
                        DataRow[] drsCheckLotLine = BaseData.Get(columns, "Lot_Line").Select(string.Format("FactoryName = '{0}'", _model.RoomName));

                        if (drsCheckLotLine.Length == 1)
                        {
                            if (bool.TryParse(drsCheckLotLine[0][LOT_LINE.IsCheckLotLine].ToString(), out this._isCheckLotLine))
                            {
                                _isCheckLotLine = bool.Parse(drsCheckLotLine[0][LOT_LINE.IsCheckLotLine].ToString());
                            }
                        }

                        //判断是否设定线别检查
                        if (_isCheckLotLine)
                        {
                            LineSettingEntity lineSettingEntity = new LineSettingEntity();

                            DataSet dsSubLine = lineSettingEntity.CheckLotLine(lotMainLineKey, _model.LineKey, _model.OperationName);

                            //判断查询结构是否正常
                            if (string.IsNullOrEmpty(lineSettingEntity.ErrorMsg))
                            {
                                //判断组件都否可以在非设定的主线上进行过站
                                if (dsSubLine.Tables[FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_NAME] == null
                                    || dsSubLine.Tables[FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0)
                                {
                                    MessageService.ShowMessage(string.Format(@"该组件帮定线别为:【{0}】,请选择正确的线别进行过站！", lotLineCode), MESSAGEBOX_CAPTION);
                                    return;
                                }
                            }
                            else
                            {
                                MessageService.ShowMessage(lineSettingEntity.ErrorMsg, MESSAGEBOX_CAPTION);
                                return;
                            }

                        }
                    }
                }
            }
            #endregion

            #endregion

            //进站作业或者出站作业。
            if (lotStateFlag == LotStateFlag.WaitintForTrackIn || lotStateFlag == LotStateFlag.WaitingForTrackout)
            {
                //等待出站
                if (lotStateFlag == LotStateFlag.WaitingForTrackout)
                {
                    #region 批次出站前检查
                    if (!TrackOutPreCheck(dsLot.Tables[0].Rows[0]))
                    {
                        return;
                    }
                    #endregion

                    this._model.OperationType = LotOperationType.TrackOut;
                }
                else if (lotStateFlag == LotStateFlag.WaitintForTrackIn)
                {
                    #region 批次进站前检查
                    //设备是否为空。
                    if (string.IsNullOrEmpty(this._model.EquipmentKey))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg006}"), MESSAGEBOX_CAPTION);//设备不能为空
                        return;
                    }
                    //线别是否为空。
                    if (string.IsNullOrEmpty(this._model.LineKey))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg017}"), MESSAGEBOX_CAPTION);//线别不能为空
                        return;
                    }
                    //检查进站数量
                    if (this._isCheckQuantityAtTrackIn && initQuantity != quantity)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg018}") + initQuantity, MESSAGEBOX_CAPTION);//进站数量必须等于
                        return;
                    }
                    //检查固化周期。
                    if (this._isCheckFixCycle)
                    {
                        string lineKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                        string proId = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PRO_ID]);
                        string waitTime = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_START_WAIT_TIME]);
                        DateTime waitTrackInTime = DateTime.MinValue;
                        if (!string.IsNullOrEmpty(waitTime))
                        {
                            waitTrackInTime = DateTime.Parse(waitTime);
                        }
                        //检查是否已超过固化周期。
                        bool checkFixCycle = this._entity.CheckFixCycle(lotNumber, lineKey, proId, waitTrackInTime);
                        if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                        {
                            MessageService.ShowError(this._entity.ErrorMsg);
                            return;
                        }
                        //如果没有超过固化周期
                        if (!checkFixCycle)
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg019}"), MESSAGEBOX_CAPTION);//固化时间不足，不能进站
                            return;
                        }
                    }
                    //检查恒温周期。
                    if (this._isCheckConstantTemperatureCycle)
                    {
                        string lineKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                        string proId = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PRO_ID]);
                        string waitTime = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_START_WAIT_TIME]);
                        DateTime waitTrackInTime = DateTime.MinValue;
                        if (!string.IsNullOrEmpty(waitTime))
                        {
                            waitTrackInTime = DateTime.Parse(waitTime);
                        }
                        //检查是否已超过恒温周期。
                        bool checkConstantTemperatureCycle = this._entity.CheckConstantTemperatureCycle(lotNumber, lineKey, proId, waitTrackInTime);
                        if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                        {
                            MessageService.ShowError(this._entity.ErrorMsg);
                            return;
                        }
                        //如果没有超过恒温周期
                        if (!checkConstantTemperatureCycle)
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg020}"), MESSAGEBOX_CAPTION);//恒温时间不足，不能进站
                            return;
                        }
                    }

                    //检查校准板周期。
                    if (this._isCheckCalibrationCycle)
                    {
                        string proId = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PRO_ID]);
                        string deviceCode = Convert.ToString(lueEquipment.GetColumnValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE));
                        //检查是否已超过校准板周期。
                        bool checkCalibrationCycle = this._entity.CheckCalibrationCycle(lotNumber, proId, deviceCode);
                        if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                        {
                            MessageService.ShowError(this._entity.ErrorMsg);
                            return;
                        }
                        //如果已超过校准板周期。
                        if (checkCalibrationCycle)
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg021}"), MESSAGEBOX_CAPTION);//需要测试新的校准板，不能进站
                            return;
                        }
                    }
                    //检查创建批次的时间,如果创建批次的时间超过了设定值（开发时要求24小时）则不能过站 上海裴玉花 2014/1/4
                    if (this._createLotTimeMaxCycle != double.MaxValue)
                    {
                        DateTime dtCurrentTime = Utils.Common.Utils.GetCurrentDateTime(); //数据库中的当前时间
                        DateTime dtCreateLotTime = Convert.ToDateTime(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CREATE_TIME]);//创建批次时间。
                        //检查创建批次的时间是否超过了设定值
                        double minutes = (dtCurrentTime - dtCreateLotTime).TotalMinutes;
                        //如果已超过超过了设定值
                        if (minutes > this._createLotTimeMaxCycle)
                        {
                            string msg = string.Format("组件({0})创建批次的时间({1})距离当前时间超过了的设定值({2})分钟，不允许过站。",
                                                      lotNumber,
                                                      dtCreateLotTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                      this._createLotTimeMaxCycle);
                            MessageService.ShowMessage(msg, MESSAGEBOX_CAPTION);
                            return;
                        }
                    }
                    #endregion

                    //如果有设置工序进站参数、或者等待作业时间超时、或者设置了必须显示进站界面。
                    //则显示工序进站界面。
                    this._model.OperationType = LotOperationType.TrackIn;
                    int ret = this.LotTrackIn(dsLot, this._model);

                    #region 批次进站后检查
                    //批次进站操作成功或失败。
                    if (ret == 0)
                    {
                        //ShowHintMessage(string.Format("[{0}]进站成功。", lotNumber));
                        ShowHintMessage(lotNumber + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg028}")); //进站成功
                        SetEquipmentState();
                        this.teLotNumber.Text = string.Empty;
                        return;
                    }
                    //批次进站操作失败。
                    if (ret == -1)
                    {
                        //ShowHintMessage(string.Format("[{0}]进站失败。", lotNumber));
                        ShowHintMessage(lotNumber + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg029}")); //进站失败
                        return;
                    }
                    //序列号需要抽检
                    if (ret == 1)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg022}"), MESSAGEBOX_CAPTION);//批次需要检验
                        return;
                    }
                    //需要显示出站界面。
                    if (ret == 3)
                    {
                        //重新获取批次信息。
                        dsLot = queryEntity.GetLotInfo(lotNumber);
                        //获取进入出站界面前批次的暂停状态信息
                        holdFlag = Convert.ToInt32(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_HOLD_FLAG]);

                        if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                        {
                            MessageService.ShowError(queryEntity.ErrorMsg);
                            return;
                        }
                        if (null == dsLot || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg010}"), MESSAGEBOX_CAPTION);//序列号不存在
                            teLotNumber.SelectAll();
                            return;
                        }
                        if (!TrackOutPreCheck(dsLot.Tables[0].Rows[0]))
                        {
                            return;
                        }
                        this._model.OperationType = LotOperationType.TrackOut;
                    }
                    //批次进站自动出站成功。
                    if (ret == 4)
                    {
                        //ShowHintMessage(string.Format("[{0}]自动出站成功。", lotNumber));
                        ShowHintMessage(lotNumber + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg023}"));//自动出站成功
                        this.teLotNumber.Text = string.Empty;
                        this.teLotNumber.Focus();
                        return;
                    }
                    #endregion
                }
                //add by genchille.yang 2012-11-24 16:31:20
                #region 终检和客检的出站操作
                if (this._model.OperationType == LotOperationType.TrackOut && (this._model.OperationName == FINAL_CHECK || this._model.OperationName == CUSTOMER_CHECK))
                {
                    //自动进站前批次状态是否正常
                    if (holdFlag == 1)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg024}"), MESSAGEBOX_CAPTION);//批次已暂停
                        return;
                    }
                    else
                    {
                        string check_type = string.Empty;
                        if (this._model.OperationName == FINAL_CHECK)
                            check_type = CHECKTYPE.DATA_GROUP_ENDCHECK;
                        if (this._model.OperationName == CUSTOMER_CHECK)
                            check_type = CHECKTYPE.DATA_GROUP_CUSTOMERCHECK;

                        this._view.WorkbenchWindow.CloseWindow(false);
                        //显示工作站终检/客检的作业界面。
                        LotDispatchForCustCheckViewContent view01 = new LotDispatchForCustCheckViewContent(this._model, check_type);
                        WorkbenchSingleton.Workbench.ShowView(view01);
                        return;
                    }
                }
                #endregion

                //end
                bool bShowDetailView = bForce;
                //批量过站
                if (this._batchTrackCount > 1)
                {
                    stateFlag = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                    lotStateFlag = (LotStateFlag)(Convert.ToInt32(stateFlag));
                    //将批次信息添加到列表中。
                    if (dtLotInfo == null || dtLotInfo.Rows.Count <= 0)
                    {
                        dtLotInfo = dsLot.Tables[0];
                        this.gcLotInfo.DataSource = dtLotInfo;
                    }
                    else
                    {
                        //判断状态是否和批次列表中已有批次的状态一致。
                        int rowCount = dtLotInfo.Select(string.Format("{0}={1}", POR_LOT_FIELDS.FIELD_STATE_FLAG, stateFlag))
                                                .Count();
                        if (rowCount == 0)
                        {
                            string msg = string.Format("当前批次状态[{0}]和已入批次列表中的状态不一致，请确认。",
                                                      CommonUtils.GetEnumValueDescription(lotStateFlag));
                            MessageService.ShowMessage(msg, MESSAGEBOX_CAPTION);
                            return;
                        }
                        dtLotInfo.Merge(dsLot.Tables[0]);
                        dsLot = null;
                    }
                    bShowDetailView = bForce || this._batchTrackCount == this.gvLotInfo.RowCount;
                }
                else
                {
                    //非批量过站
                    dtLotInfo = dsLot.Tables[0];
                    bShowDetailView = true;
                }
                this.teLotNumber.Text = string.Empty;
                //显示工作站作业明细界面。
                if (bShowDetailView)
                {
                    ShowDispatchViewContent(this._model, dtLotInfo);
                }
                return;
            }

            #region 出站数据采集。
            //等待数据采集
            if (lotStateFlag == LotStateFlag.WaitingForOutEDC)
            {
                //....
            }
            //出站数据采集。
            if (lotStateFlag == LotStateFlag.OutEDC)
            {
                //提示用户是否进行数据采集。
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg024}"), MESSAGEBOX_CAPTION)) //抽检未完成，是否执行抽检
                {
                    EdcGatherData _edcData = new EdcGatherData();
                    _edcData.EquipmentKey = equipmentKey;
                    _edcData.LineName = this.lueLine.Text;
                    _edcData.LotNumber = lotNumber;
                    _edcData.MaterialLot = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_LOT]);
                    _edcData.OperationName = operationName;
                    _edcData.OrderNumber = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                    _edcData.PartNumber = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                    _edcData.ShiftName = shiftName;
                    _edcData.Operator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    //遍历工作台视图对象。
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        //如果视图对象名称是"数据采集明细",则强制关闭。
                        if (viewContent is EDCData04WViewContent)
                        {
                            viewContent.WorkbenchWindow.CloseWindow(true);
                            break;
                        }
                    }
                    //显示数据采集的视图界面。
                    EDCData04WViewContent view = new EDCData04WViewContent(_edcData);
                    WorkbenchSingleton.Workbench.ShowView(view);
                }
                return;
            }
            #endregion
        }

        /// <summary>
        /// 检查当前计算机的设备绑定
        /// </summary>
        private bool CheckComputerEquipment()
        {
            //获取当前电脑的Mac地址
            string computerMAC = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_MAC);
            bool isAllow = false;

            //获取当前电脑对应的配置信息
            DataSet dsComputerUda = _computerEntity.GetComputerUda(computerMAC);

            if (string.IsNullOrEmpty(_computerEntity.ErrorMsg))
            {
                //判断查询的结果中是否包含对应的表信息
                if (dsComputerUda.Tables.Contains(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtComputerUda = dsComputerUda.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME];

                    //获取绑定的设备代码信息
                    DataRow[] drsComputerEquipment = dtComputerUda.Select(" ATTRIBUTE_NAME = 'EQUIPMENT_CODE'");

                    //判断是否对当前电脑进行设备的绑定 如果存在进行检测，否则直接设定 isAllow 为 true
                    if (drsComputerEquipment.Length != 0)
                    {
                        //获取绑定的设备代码信息
                        string computerEquipments = Convert.ToString(drsComputerEquipment[0][COMPUTER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]);

                        //对获取绑定的设备代码信息进行分隔成单个的设备信息
                        string[] computerEquipmentList = computerEquipments.Split(',');

                        //获取设备下拉列表对应的数据集
                        DataTable dtEquipmentBind = (DataTable)lueEquipment.Properties.DataSource;

                        //获取当前选中的设备对应的行信息
                        DataRow drEquipmentSelected = dtEquipmentBind.Select(string.Format(" EQUIPMENT_KEY =  '{0}'", lueEquipment.EditValue))[0];

                        //获取选中的设备对应的设备代码
                        string curEquipmentCode = Convert.ToString(drEquipmentSelected[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);

                        //遍历判断选中的设备是否为当前电脑允许过站的设备
                        foreach (string computerEquipment in computerEquipmentList)
                        {
                            //若允许设定 isAllow 为 true 并结束遍历
                            if (computerEquipment.Equals(curEquipmentCode))
                            {
                                isAllow = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        isAllow = true;
                    }
                }
            }
            else
            {
                //查询电脑对应设定属性失败后进行错误提示
                MessageService.ShowMessage(_computerEntity.ErrorMsg, MESSAGEBOX_CAPTION);
            }

            //返回结果
            return isAllow;

        }

        /// <summary>
        /// 批次出站前检查。
        /// </summary>
        /// <param name="drLot">批次信息。</param>
        /// <returns>true：检查通过。false：检查失败。</returns>
        private bool TrackOutPreCheck(DataRow drLot)
        {
            string roomName = Convert.ToString(this.lueFactoryRoom.Text);
            DateTime trackInTime = Convert.ToDateTime(drLot[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME]);
            string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            double quantity = Convert.ToDouble(drLot[POR_LOT_FIELDS.FIELD_QUANTITY]);
            double initQuantity = Convert.ToDouble(drLot[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
            //检查IV测试数据是否已经抓取。
            if (this._isCheckIVTestData && !CheckIVTestData(lotNumber, trackInTime))
            {
                //测试数据插入到数据库的时间>入站时间+组件序列号判断
                return false;
            }
            //检查EL图片是否存在。
            if (this._isCheckELPicture && !CheckELPicture(lotNumber, roomName))
            {
                return false;
            }

            //检查组件工单和前一次刷入工单的异同，如果不相同给出提示，如果相同不做任何提示。
            if (this._isCheckWorkOrderNoDifferent)
            {
                string preWorkOrderNo = PropertyService.Get(PROPERTY_FIELDS.PRE_WORK_ORDER_NO);
                string orgWorkOrderNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO]);
                if (preWorkOrderNo != orgWorkOrderNo)
                {
                    PropertyService.Set<string>(PROPERTY_FIELDS.PRE_WORK_ORDER_NO, orgWorkOrderNo);
                    PropertyService.Save();

                    if (!string.IsNullOrEmpty(preWorkOrderNo))
                    {
                        MessageService.ShowWarning(string.Format("请注意，组件（{2}）原工单号（{0}）与前一次刷入组件的原工单号（{1}）不同。",
                                                    orgWorkOrderNo,
                                                    preWorkOrderNo,
                                                    lotNumber));
                    }
                }
            }
            //根据工单检查颜色。如果有一个组件序列号需要输入花色，则全部需要输入花色。
            if (this._isCheckColorByWorkOrder && !this._isMustInputColor)
            {
                string workOrderNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                //检查该工单是否必须输入颜色。
                this._isMustInputColor = this._entity.IsCheckColorByWorkOrder(workOrderNo);
                this._model.IsCheckColorByWorkOrder = this._isMustInputColor;
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                    return false;
                }
            }
            string lines = PropertyService.Get(PROPERTY_FIELDS.LINES) + ",";
            string operationLine = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_OPR_LINE]);
            if (lines.IndexOf(operationLine + ",") == -1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg025}"), MESSAGEBOX_CAPTION);//对该批次的进站线别没有操作权限,无法出站

                return false;
            }
            //检查出站数量。
            if (this._isCheckQuantityAtTrackOut && initQuantity != quantity)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg026}") + initQuantity, MESSAGEBOX_CAPTION);//出站数量必须等于
                //MessageService.ShowMessage(string.Format("出站数量必须等于({0})。", initQuantity), MESSAGEBOX_CAPTION);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 显示批次过站明细界面。
        /// </summary>
        /// <param name="model"></param>
        private void ShowDispatchViewContent(LotDispatchDetailModel model, DataTable dtLotInfo)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            LotDispatchDetailViewContent view = new LotDispatchDetailViewContent(model, dtLotInfo);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 包装作业，显示包装作业详细界面。
        /// </summary>
        private void ToPackage(LotDispatchDetailModel model)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示包装作业明细界面。
            LotDispatchForPalletViewContent view = new LotDispatchForPalletViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 入库检验作业，显示入库检验详细界面。
        /// </summary>
        private void ToStoreCheck(LotDispatchDetailModel model)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示入库检验作业明细界面。
            LotToWarehouseCHeckViewContent view = new LotToWarehouseCHeckViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 入库作业，显示入库详细界面。
        /// </summary>
        private void ToStore(LotDispatchDetailModel model)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示入库作业明细界面。
            LotToStoreDetailViewContent view = new LotToStoreDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 批次进站。
        /// </summary>
        /// <param name="dsLotInfo">批次信息。</param>
        /// <returns>
        /// -1：操作失败。0:进站成功。1:需要抽检。2：需要显示进站界面。3：需要显示出站界面。
        /// </returns>
        private int LotTrackIn(DataSet dsLotInfo, LotDispatchDetailModel model)
        {
            DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];
            string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            double qty = Convert.ToDouble(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            double leftQty = qty;
            string lineKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
            string lineName = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            string workOrderKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
            string edcInsKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
            string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            bool isTrackInDelay = _entity.GetLotTrackInIsDelay(lotKey);
            //如果已经超时，需要显示进站界面。
            if (isTrackInDelay)
            {
                return 2;
            }
            RouteQueryEntity queryEntity = new RouteQueryEntity();
            DataSet dsStepData = queryEntity.GetStepBaseDataAndParamDataByKey(stepKey, OperationParamDCType.TrackIn);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                return -1;
            }
            //获取到工步参数数据且有设置工步参数，则需要显示进站界面。
            if (null != dsStepData
                && dsStepData.Tables.Contains(POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME)
                && dsStepData.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                return 2;
            }
            DataTable dtStepBaseData = dsStepData.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtStepParamData = dsStepData.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME];
            //判断获取工步信息是否失败。
            if (dtStepBaseData == null || dtStepBaseData.Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("获取【{0}】信息失败，请重试。", stepName));
                return -1;
            }
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            DataSet dsParams = new DataSet();
            //组织操作数据。
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftQty);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, model.ShiftKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, model.ShiftName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, model.UserName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, model.LineKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, model.LineName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, model.EquipmentKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, model.UserName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
            dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
            dsParams.Tables.Add(dtTransaction);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            string operationKey = Convert.ToString(dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            string duration = Convert.ToString(dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DURATION]);
            string partNumber = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
            string workOrderNo = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
            htMaindata.Add(POR_ROUTE_STEP_FIELDS.FIELD_DURATION, duration);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, model.LotNumber);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, workOrderNo);
            htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, partNumber);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            //加载数据时的编辑时间。
            dtParams.ExtendedProperties.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Tables.Add(dtParams);
            //执行工作站作业。
            int code = _entity.LotTrackIn(dsParams);
            //需要抽检。
            if (code == 2)
            {
                return 1;
            }

            //异常信息。
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return -1;
            }
            else if (code == 4) //自动进站后出站成功
            {
                return 4;
            }
            else
            {
                //进站成功。获取是否返回主界面的设置。
                bool bReturnMainview = _entity.GetTrackInIsReturnMainView(stepKey);
                if (!bReturnMainview)
                {
                    return 3;
                }
                return 0;
            }
        }
        /// <summary>
        /// 检查IV测试数据是否存在。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="trackInTime">进站时间。</param>
        /// <returns>false:不存在。true：存在。</returns>
        private bool CheckIVTestData(string lotNumber, DateTime trackInTime)
        {
            bool isExists = this._entity.CheckIVTestData(lotNumber, trackInTime);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                return false;
            }
            if (!isExists)
            {
                MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg030}")); //组件的IV测试数据不存在，不允许出站
            }
            return isExists;
        }
        /// <summary>
        /// 检查EL图片是否存在。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>false:不存在。true：存在。</returns>
        private bool CheckELPicture(string lotNumber, string roomName)
        {
            bool isExists = false;
            //根据车间名称获取EL图片所在文件夹路径。
            //PIC_ADDRESS PIC_FACTORY_CODE PIC_TYPE
            string[] columnNames = { "PIC_ADDRESS", "PIC_DATE_FORMAT" };//图片所在共享文件夹路径。
            List<KeyValuePair<string, string>> conditions = new List<KeyValuePair<string, string>>();
            conditions.Add(new KeyValuePair<string, string>("PIC_FACTORY_CODE", roomName)); //车间代码==车间名称
            conditions.Add(new KeyValuePair<string, string>("PIC_TYPE", "EL")); //表示是EL图片。
            try
            {
                DataTable dtReturn = BaseData.GetBasicDataByCondition(columnNames, BASEDATA_CATEGORY_NAME.Uda_Pic_Address, conditions);
                //没有设置EL图片路径
                if (dtReturn == null || dtReturn.Rows.Count <= 0)
                {
                    MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg031}")); //没有设置EL图片路径，不允许出站
                    return false;
                }
                string picAddress = Convert.ToString(dtReturn.Rows[0]["PIC_ADDRESS"]);
                string picDateFormat = Convert.ToString(dtReturn.Rows[0]["PIC_DATE_FORMAT"]).Trim();
                if (string.IsNullOrEmpty(picDateFormat))
                {
                    picDateFormat = "yyyy-M-d";
                }
                //抓取组件有效的IV测试时间。
                DateTime dtPicTime = this._entity.GetLotValidIVTestTime(lotNumber);
                //根据规则生成图片路径。
                // picAddress\2012年\1月\2012-1-12
                string picPath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}.jpg",
                                                picAddress,
                                                dtPicTime.ToString("yyyy年"),
                                                dtPicTime.ToString("M月"),
                                                dtPicTime.ToString(picDateFormat),
                                                lotNumber);
                //判断图片是否存在。
                isExists = System.IO.File.Exists(picPath);
                if (!isExists)
                {
                    MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg032}")); //组件的EL图片不存在，不允许出站
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
                return false;
            }

            return isExists;
        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLotInfo_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolSeqNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gcolStateFlag)
            {
                LotStateFlag lotStateFlag = (LotStateFlag)(Convert.ToInt32(e.CellValue));
                e.DisplayText = CommonUtils.GetEnumValueDescription(lotStateFlag);
            }
        }
        /// <summary>
        /// 清空批次信息列表。
        /// </summary>
        private void ClearLotInfo()
        {
            DataTable dt = this.gcLotInfo.DataSource as DataTable;
            if (dt != null)
            {
                dt.Rows.Clear();
            }
        }
        /// <summary>
        /// 移除批次列表中的信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int rowHandle = this.gvLotInfo.FocusedRowHandle;
            //没有选中行，给出提示。
            if (rowHandle < 0)
            {
                MessageService.ShowMessage(StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg027}")), MESSAGEBOX_CAPTION);//请选择要移除的批次信息
                return;
            }
            DataRow dr = this.gvLotInfo.GetDataRow(rowHandle);
            DataTable dt = this.gcLotInfo.DataSource as DataTable;
            if (dt != null && dr != null)
            {
                dt.Rows.Remove(dr);
                dt.AcceptChanges();
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
                tsbDispatch_Click(null, null);
                args.Handled = true;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LotDispatch_VisibleChanged(object sender, EventArgs e)
        {
            if (this._model != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(this.lueEquipment.EditValue)))
                {
                    this.teLotNumber.Select();
                }
                else
                {
                    this.lueEquipment.Select();
                }
            }
        }

 
        protected override void WndProc(ref Message message)
        {
            if (id != null)
            {
                id.ProcessMessage(message);
            }
            base.WndProc(ref message);
        }

        private void m_KeyPressed(object sender, InputDevice.KeyControlEventArgs e)
        {
            device = e.Keyboard.Name;
        }

        private void LotDispatch_Paint(object sender, PaintEventArgs e)
        {
            if (this._model != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(this.lueEquipment.EditValue)))
                {
                    this.teLotNumber.Select();
                }
                else
                {
                    this.lueEquipment.Select();
                }
            }
        }
        /// <summary>
        /// 自定义显示设备名称。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        {
            DataRowView drv = this.lueEquipment.GetSelectedDataRow() as DataRowView;
            if (drv != null)
            {
                e.DisplayText = Convert.ToString(drv[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
            }
        }
        /// <summary>
        /// 处理设备列表中不存在的设备编号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_ProcessNewValue(object sender, ProcessNewValueEventArgs e)
        {
            this.lueEquipment.EditValue = null;
            e.Handled = true;
        }
        /// <summary>
        /// 设备按键事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.teLotNumber.Select();
                this.teLotNumber.SelectAll();
            }
        }
    }
}
