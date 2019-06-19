//调整只有数据采集按钮
#region using
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraLayout.Utils;
#endregion

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 表示设备数据采集的主控件类。用于输入设备数据采集的批次号、工单号、线别以及操作的员工号等信息。
    /// </summary>
    public partial class EDCMainCtrl : BaseUserCtrl
    {
        bool bIsOnLoading = true;                                               //是否是正在加载中。
        EdcGatherData _edcData = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EDCMainCtrl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据采集按钮Click事件处理方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        /// comment by peter 2012-2-23
        private void btnMTData_Click(object sender, EventArgs e)
        {
            //车间是否输入?
            if (lueFactoryRoom.Text == string.Empty)
            {
                MessageService.ShowMessage("车间不能为空");
                this.lueFactoryRoom.Focus();
                return;
            }
            //工序是否输入?
            if (cbOperation.Text == string.Empty)
            {
                MessageService.ShowMessage("工序不能为空");
                this.cbOperation.Focus();
                return;
            }
            //采集项目不能为空
            if (string.IsNullOrEmpty(this.lueEDCItem.Text))
            {
                MessageService.ShowMessage("采集项目不能为空。");
                this.lueEDCItem.Focus();
                return;
            }
            string selectedEquipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            //设备不能为空
            if (string.IsNullOrEmpty(selectedEquipmentKey))
            {
                MessageService.ShowMessage("设备不能为空。");
                this.lueEquipment.Focus();
                return;
            }
            string actionName = this.lueEDCItem.GetColumnValue("ACTION_NAME").ToString();        //数据采集项目触发时的动作。
            string equipmentKey = this.lueEDCItem.GetColumnValue("EQUIPMENT_KEY").ToString();    //数据采集项目指定的设备主键。
            string operationName = this.lueEDCItem.GetColumnValue("OPERATION_NAME").ToString();  //数据采集项目指定的工序名称。
            string sField = Convert.ToString(this.lueEDCItem.GetColumnValue(EDC_POINT_FIELDS.FIELD_MUST_INPUT_FIELD));  //数据采集项目必须输入的栏位。
            int field = 0;
            if (!int.TryParse(sField, out field))
            {
                field = 0;
            }
            bool bMustInputLotNo = ((EDCPointMustInputField)field & EDCPointMustInputField.LotNo) == EDCPointMustInputField.LotNo;
            string edckey = this.lueEDCItem.GetColumnValue("EDC_KEY").ToString();
            string spkey = this.lueEDCItem.GetColumnValue("SP_KEY").ToString();

            if (!string.IsNullOrEmpty(equipmentKey) && equipmentKey != selectedEquipmentKey)
            {
                MessageService.ShowMessage("采集项目指定的设备和当前选中设备不一致。");
                this.lueEquipment.Focus();
                return;
            }
            if (operationName != cbOperation.Text)
            {
                MessageService.ShowMessage("采集项目指定的工序和当前选中工序不一致。");
                this.lueEquipment.Focus();
                return;
            }
            if (actionName != "NONE")
            {
                //批次号是否输入?
                if (this.txtLotNo.Text == string.Empty)
                {
                    MessageService.ShowMessage("在线采集项目，批号必须输入。", "提示");
                    this.txtLotNo.Focus();
                    return;
                }
                //检查是否对该批次号具有权限。
                if (!CheckLotNo(txtLotNo.Text.Trim(), ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT))
                {
                    return;
                }
            }
            else if (bMustInputLotNo)//离线采集项目且必须输入批次号。
            {
                if (string.IsNullOrEmpty(this.txtLotNo.Text.Trim()))
                {
                    MessageService.ShowMessage("指定的数据采集项目，批号必须输入。", "提示");
                    this.txtLotNo.Focus();
                    return;
                }
                //检查是否对该批次号具有权限。
                if (!CheckLotNo(txtLotNo.Text.Trim(), ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_NONE))
                {
                    return;
                }
            }
            //如果有输入批次，则获取批次信息。
            if (!string.IsNullOrEmpty(this.txtLotNo.Text))
            {
                //如果没有成功获取到批次信息。
                if (GetLotInfomation() == false)
                {
                    return;
                }
            }
            //获取当前班别名称。
            Shift _shift = new Shift();
            string defaultShift = _shift.GetCurrShiftName();
            _edcData = new EdcGatherData();
            _edcData.Operator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            _edcData.LotNumber = this.txtLotNo.Text.ToUpper();
            _edcData.EDCKey = edckey;
            _edcData.LineName = this.lueLine.Text;
            _edcData.OperationName = operationName;
            _edcData.EquipmentKey = selectedEquipmentKey;
            _edcData.MaterialLot = this.txtMaterialLot.Text;
            _edcData.OrderNumber = this.txtWorkOrder.Text;
            _edcData.PartNumber = this.txtPartNumber.Text;
            _edcData.SupplierName = this.txtSupplier.Text;
            _edcData.ShiftName = defaultShift;
            _edcData.EquipmentName = this.lueEquipment.Text;
            _edcData.EDCName = this.lueEDCItem.Text;
            _edcData.FactoryRoomName = this.lueFactoryRoom.Text;
            _edcData.FactoryRoomKey = this.lueFactoryRoom.EditValue.ToString();
            _edcData.EDCActionName = actionName;
            _edcData.EDCPointKey = this.lueEDCItem.EditValue.ToString();
            _edcData.EDCSPKey = spkey;
            _edcData.PartType = Convert.ToString(this.luePartType.EditValue);
            //遍历工作台中的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is EDCData04WViewContent)
                {
                    viewContent.WorkbenchWindow.CloseWindow(true);
                    break;
                }
            }
            //创建新的视图对象并显示。
            EDCData04WViewContent view = new EDCData04WViewContent(_edcData);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 检查进行数据采集时是否对该批次号有权限。
        /// </summary>
        /// <param name="lotNumber">批次号码。</param>
        /// <param name="actionName">NONE：离线数据采集。TRACKOUT:在线数据采集。</param>
        /// <returns>true或者false,true表示有权限，false表示没有权限。</returns>
        /// comment by peter 2012-2-23
        private bool CheckLotNo(string lotNumber, string actionName)
        {
            LotQueryEntity entity = new LotQueryEntity();
            DataSet ds = entity.GetLotInfo(lotNumber);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageService.ShowMessage("指定的批号不存在。", "提示");
                    return false;//如果获取0行记录。
                }
                string factoryRoomKey = ds.Tables[0].Rows[0]["FACTORYROOM_KEY"].ToString();
                string equipmentKey = ds.Tables[0].Rows[0]["EQUIPMENT_KEY"].ToString();
                string stepName = ds.Tables[0].Rows[0]["ROUTE_STEP_NAME"].ToString();
                int state = Convert.ToInt32(ds.Tables[0].Rows[0]["STATE_FLAG"]);
                string orderNumber = Convert.ToString(ds.Tables[0].Rows[0]["WORK_ORDER_NO"]);
                string materialLot = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_LOT"]);
                string partNumber = Convert.ToString(ds.Tables[0].Rows[0]["PART_NUMBER"]);
                string partType = Convert.ToString(ds.Tables[0].Rows[0]["LOT_TYPE"]);
                string edcInsKey = Convert.ToString(ds.Tables[0].Rows[0]["EDC_INS_KEY"]);
                string routeKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);


                string curRoomKey = this.lueFactoryRoom.EditValue.ToString();
                string curEquipmentKey = this.lueEquipment.EditValue.ToString();
                string operationName = this.cbOperation.Text;
                string curPartNumber = Convert.ToString(this.lueEDCItem.GetColumnValue(EDC_POINT_FIELDS.FIELD_TOPRODUCT));
                string curPartType = Convert.ToString(this.lueEDCItem.GetColumnValue(EDC_POINT_FIELDS.FIELD_PART_TYPE));
                string curRouteKey = Convert.ToString(this.lueEDCItem.GetColumnValue(EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY));

                if (string.IsNullOrEmpty(factoryRoomKey) || factoryRoomKey != curRoomKey)
                {
                    MessageService.ShowMessage("指定的批号在当前选中车间不存在。", "提示");
                    return false;
                }
                //if (!string.IsNullOrEmpty(curRouteKey) && curRouteKey != routeKey)
                //{
                //    MessageService.ShowMessage("指定批号和指定数据采集项的工艺流程不一致。", "提示");
                //    return false;
                //}
                if (!string.IsNullOrEmpty(curPartNumber) && curPartNumber != partNumber)
                {
                    MessageService.ShowMessage("指定批号的成品料号不能使用当前采集项目。", "提示");
                    return false;
                }
                //else if (string.IsNullOrEmpty(curPartNumber) && !string.IsNullOrEmpty(curPartType) && curPartType != partType)
                //{
                //    MessageService.ShowMessage("指定批号的成品类型不能使用当前采集项目。", "提示");
                //    return false;
                //}
                //在线采集还需要继续判断。
                //if (actionName != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_NONE)
                //{
                //    if (stepName != operationName)
                //    {
                //        MessageService.ShowMessage("指定批号未在当前选中工序上进行加工。", "提示");
                //        return false;
                //    }
                //    if (curEquipmentKey != equipmentKey)
                //    {
                //        MessageService.ShowMessage("指定批号未在当前选中设备上进行加工。", "提示");
                //        return false;
                //    }

                //    if (string.IsNullOrEmpty(edcInsKey))
                //    {
                //        MessageService.ShowMessage("指定批号不能进行数据采集。", "提示");
                //        return false;
                //    }
                //}

                this.txtWorkOrder.Text = orderNumber;
                this.txtMaterialLot.Text = materialLot;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 窗体载入事件方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        ///  comment by peter 2012-2-23
        private void EDCMainCtrl_Load(object sender, EventArgs e)
        {
            bIsOnLoading = true;
            BindFactoryRoom();
            BindOperations();
            BindEquipment();
            BindPartType();
            BindLine();
            BindEDCItem();
            bIsOnLoading = false;
            HideControl();


            //this.TitleName = "数据采集查询";  //视图标题。
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建新的设备数据控件对象。
            EDCQueryCtrl ctrl = new EDCQueryCtrl();
            ctrl.Dock = DockStyle.Fill;
            //将设备数据采集的控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            //panel.Controls.Add(ctrl);
            this.splitContainerControl1.Panel2.Controls.Add(ctrl);
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
        /// 绑定成品类型数据。
        /// </summary>
        private void BindPartType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "PART_TYPE");
            DataTable lotTypeTable = BaseData.Get(columns, category);
            lotTypeTable.Rows.InsertAt(lotTypeTable.NewRow(), 0);
            this.luePartType.Properties.DataSource = lotTypeTable;
            this.luePartType.Properties.DisplayMember = "NAME";
            this.luePartType.Properties.ValueMember = "CODE";
            this.luePartType.ItemIndex = 0;
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
        /// 绑定工厂车间。
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
                    this.lueFactoryRoom.EditValue = dt.Rows[0]["LOCATION_KEY"].ToString();
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
        }
        /// <summary>
        /// 绑定设备。
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cbOperation.Text;
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
            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, string.Empty);
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                //ds.Tables[0].Rows.InsertAt(ds.Tables[0].NewRow(),0);
                this.lueEquipment.Properties.DataSource = ds.Tables[0];
                this.lueEquipment.Properties.DisplayMember = "EQUIPMENT_NAME";
                this.lueEquipment.Properties.ValueMember = "EQUIPMENT_KEY";
                this.lueEquipment.ItemIndex = 0;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
            SetLineValue();
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
        /// 绑定采集项目。
        /// </summary>
        private void BindEDCItem()
        {
            string strOperation = this.cbOperation.Text;
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue == null ? string.Empty : this.lueFactoryRoom.EditValue.ToString();
            string equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            string partType = Convert.ToString(this.luePartType.EditValue);

            this.lueEDCItem.EditValue = string.Empty;
            string strParam = string.Empty;
            EdcPoint entity = new EdcPoint();
            DataSet ds = entity.GetEDCPoint(strFactoryRoomKey, strOperation, partType, equipmentKey);
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueEDCItem.Properties.DataSource = ds.Tables[0];
                this.lueEDCItem.Properties.DisplayMember = "EDC_NAME";
                this.lueEDCItem.Properties.ValueMember = "ROW_KEY";//EDCPoint主键。
                                                                   //if (ds.Tables[0].Rows.Count > 0)
                                                                   //{
                                                                   //this.lueEDCItem.ItemIndex = 0;
                                                                   //}
            }
            //SetEDCChangedData();
        }
        /// <summary>
        /// EDC项目改变后修改数据。
        /// </summary>
        private void SetEDCChangedData()
        {
            string partNo = Convert.ToString(this.lueEDCItem.GetColumnValue("TOPRODUCT"));
            string equipmentKey = Convert.ToString(this.lueEDCItem.GetColumnValue("EQUIPMENT_KEY"));
            string partType = Convert.ToString(this.lueEDCItem.GetColumnValue("PART_TYPE"));
            this.txtPartNumber.Text = partNo;
            if (string.IsNullOrEmpty(Convert.ToString(this.lueEquipment.EditValue)))
            {
                this.lueEquipment.EditValue = equipmentKey;
                SetLineValue();
            }
            if (partType.CompareTo((Convert.ToString(this.luePartType.EditValue))) != 0)
            {
                this.luePartType.EditValue = partType;
            }
        }
        /// <summary>
        /// 工厂车间改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            BindLine();
            //重新绑定设备控件
            BindEquipment();
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 设备改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            SetLineValue();
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 工序改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            //重新绑定设备控件
            BindEquipment();
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 批次号文本框的键盘按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                GetLotInfomation();
            }
        }
        /// <summary>
        /// 获取批次信息。
        /// </summary>
        /// <returns>true：成功获取批次信息。false：获取批次信息失败。</returns>
        private bool GetLotInfomation()
        {
            string lotNo = this.txtLotNo.Text;
            this.txtMaterialLot.Text = string.Empty;
            this.txtWorkOrder.Text = string.Empty;
            this.txtPartNumber.Text = string.Empty;
            this.txtSupplier.Text = string.Empty;
            if (string.IsNullOrEmpty(lotNo)) return true;
            LotQueryEntity entity = new LotQueryEntity();
            DataSet ds = entity.GetLotInfo(lotNo);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageService.ShowMessage("指定的批号不存在。");
                    return false;//如果获取0行记录。
                }
                string factoryRoomKey = ds.Tables[0].Rows[0]["FACTORYROOM_KEY"].ToString();
                string equipmentKey = ds.Tables[0].Rows[0]["EQUIPMENT_KEY"].ToString();
                string curRoomKey = this.lueFactoryRoom.EditValue.ToString();
                string curEquipmentKey = this.lueEquipment.EditValue.ToString();

                if (string.IsNullOrEmpty(factoryRoomKey) || factoryRoomKey != curRoomKey)
                {
                    MessageService.ShowMessage("指定的批号在当前选中车间不存在。");
                    return false;
                }

                this.txtMaterialLot.Text = ds.Tables[0].Rows[0]["MATERIAL_LOT"].ToString();
                this.txtWorkOrder.Text = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                this.txtPartNumber.Text = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                this.txtSupplier.Text = ds.Tables[0].Rows[0]["SUPPLIER_NAME"].ToString();
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭当前视图界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 采集项目改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEDCItem_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            SetEDCChangedData();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 成品类型改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPartType_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 转到查询界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //遍历工作台中的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果有数据采集查询视图，则选中该视图显示，返回以结束该方法的运行。
                EDCQueryViewContent openView = viewContent as EDCQueryViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象并显示。
            EDCQueryViewContent view = new EDCQueryViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);


        }

        private void lueFactoryRoom1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
