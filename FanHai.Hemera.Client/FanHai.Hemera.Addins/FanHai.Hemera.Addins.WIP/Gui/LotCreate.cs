using System;
using System.Collections.Generic;
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
using DevExpress.XtraLayout.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次创建的窗体类。
    /// </summary>
    public partial class LotCreate : BaseUserCtrl
    {
        private bool IsBinding = false;
        private LotCreateEntity _entity = new LotCreateEntity();
        private bool _isBatch = false; //是否是批量创建批次。
        private IViewContent _view = null;
        private LotCreateDetailModel _model = null;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示



        /// <summary>
        /// 构造函数
        /// </summary>
        public LotCreate(LotCreateDetailModel model,bool isBatch,IViewContent view)
        {
            InitializeComponent();
            this._isBatch = isBatch;
            this._view = view;
            this._model = model;
            InitializeLanguage();
        }



        public void InitializeLanguage()
        {
            this.btnReset.Text = "重置";// "重置";
            this.btnSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.btnSave}");//"下一步";
            this.lcgTop.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lcgTop}");//"表头";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciFactoryRoom}");//"车间名称";
            this.lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciOperation}");//"工序名称";
            this.lcgContent.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lcgContent}");//"内容";
            this.lciOrderNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciOrderNumber}");//"工单号";
            this.lciReceiveItemNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciReceiveItemNo}");//"领料项目号";
            this.lciProId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciProId}");//"产品ID号";
            this.lciCreateType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciCreateType}");//"创建类别";
            this.lciCount.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciCount}");//"投批数量";
            this.lcgHidden.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lcgHidden}");//"隐藏";
            this.lciUserId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciUserId}");//"员工号";
            this.lciShiftName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciShiftName}");//"班别";
            this.lcgCommands.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lcgCommands}");//"命令";
            this.lciBtnClose.Text = "重置";//"重置";
            this.lciBtnSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.lciBtnSave}");//"保存";

        }




        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotCreate_Load(object sender, EventArgs e)
        {
            
            this.IsBinding=true;
            //绑定工厂车间
            BindFactoryRoom();
            //绑定工序名
            BindOperation();
            //绑定工单号
            BindWorkOrder();
            BindProId();
            BindReceiveItemNo();
            BindLotCreateType();
            //绑定班别
            BindShiftName();
            this.IsBinding=false;
            InitControlValue();
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            if (this._model != null)
            {
                this.cbFactoryRoom.EditValue = this._model.RoomKey;
                this.cbOperation.Text = this._model.OperationName;
                this.lueReceiveItemNo.EditValue = this._model.StoreMaterialDetailKey;
                this.teCount.Text = this._model.Count.ToString();
                this.lueCreateType.EditValue = this._model.CreateTypeCode;
            }
            //绑定员工号
            this.txtUserId.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //如果是非批量创建批次，不显示投批数量，默认为1.
            if (this._isBatch == false)
            {
                this.lciCount.Visibility = LayoutVisibility.Never;
                this.lblMenu.Text = "生产管理>过站管理>组件补片";
            }
            else
            {
                this.lblMenu.Text = "生产管理>过站管理>组件创建";
            }
        }
        /// <summary>
        /// 绑定工厂车间名称
        /// </summary>
        private void BindFactoryRoom()
        {
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);//拥有权限的线上仓。
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);
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
            //禁用领料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.cbFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定登陆用户拥有权限的工序名称到下拉控件中。
        /// </summary>
        private void BindOperation()
        {
            //获取登陆用户已经拥有权限的工序
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)
            {
                string[] strOperations = operations.Split(',');
                //将工序添加到下拉控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    this.cbOperation.Properties.Items.Add(strOperations[i]);
                }
                this.cbOperation.SelectedIndex = 0;
            }
            //禁用工序
            if (string.IsNullOrEmpty(operations)
                || this.cbOperation.Properties.Items.Count <= 1)
            {
                this.cbOperation.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定工单名称
        /// </summary>
        private void BindWorkOrder()
        {
            string roomKey = Convert.ToString(this.cbFactoryRoom.EditValue);
            DataSet ds = _entity.GetWorkOrderByFactoryRoomKey(roomKey);
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                //绑定工单号数据到窗体控件。
                this.cbOrderNumber.Properties.DataSource = ds.Tables[0];
                this.cbOrderNumber.Properties.DisplayMember = "ORDER_NUMBER";
                this.cbOrderNumber.Properties.ValueMember = "ORDER_NUMBER";
            }
            else
            {
                this.cbOrderNumber.Properties.DataSource = null;        
            }
            this.cbOrderNumber.EditValue = string.Empty;
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
                this.lueProId.Properties.ValueMember = "PRODUCT_CODE";
            }
            else
            {
                this.lueProId.Properties.DataSource = null;
                this.lueProId.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定领料项目号信息。
        /// </summary>
        private void BindReceiveItemNo()
        {
            string orderNo = Convert.ToString(this.cbOrderNumber.EditValue);
            string operationName = this.cbOperation.Text.Trim();
            string roomKey = Convert.ToString(this.cbFactoryRoom.EditValue);
            string proId = Convert.ToString(this.lueProId.EditValue);

            DataSet ds = _entity.GetReceiveMaterialInfo(roomKey, operationName, orderNo, proId);
            if (string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                this.lueReceiveItemNo.Properties.DataSource = ds.Tables[0];
                this.lueReceiveItemNo.Properties.DisplayMember = "MATERIAL_LOT";
                this.lueReceiveItemNo.Properties.ValueMember = "STORE_MATERIAL_DETAIL_KEY";
            }
            else
            {
                this.lueReceiveItemNo.Properties.DataSource = null;
            }
            this.lueReceiveItemNo.EditValue =null;
        }
        /// <summary>
        /// 绑定批次创建类别。
        /// </summary>
        private void BindLotCreateType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Lot_CreateType");

            this.lueCreateType.Properties.DataSource = BaseData.Get(columns, category);
            this.lueCreateType.Properties.DisplayMember = "NAME";
            this.lueCreateType.Properties.ValueMember = "CODE";
            this.lueCreateType.ItemIndex = 0;
        }
        /// <summary>
        /// 绑定班别信息
        /// </summary>
        private void BindShiftName()
        {
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");

            this.cbShiftName.Properties.DataSource = BaseData.Get(columns, category);
            this.cbShiftName.Properties.DisplayMember = "CODE";
            this.cbShiftName.Properties.ValueMember = "CODE";

            Shift shift = new Shift();
            string defaultShiftName = shift.GetCurrShiftName();
            this.cbShiftName.EditValue = defaultShiftName;
        }
        /// <summary>
        /// 确认按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            this._model = new LotCreateDetailModel();
            this._model.RoomKey = Convert.ToString(this.cbFactoryRoom.EditValue);
            this._model.RoomName = this.cbFactoryRoom.Text;
            this._model.OperationName = this.cbOperation.Text;
            this._model.StoreMaterialDetailKey = Convert.ToString(this.lueReceiveItemNo.EditValue);
            this._model.ReceiveItemNo = Convert.ToString(this.lueReceiveItemNo.Text);
            this._model.ProId = Convert.ToString(this.lueReceiveItemNo.GetColumnValue("PRO_ID"));
            this._model.MaterialCode = Convert.ToString(this.lueReceiveItemNo.GetColumnValue("MATNR"));
            this._model.SupplierName = Convert.ToString(this.lueReceiveItemNo.GetColumnValue("SUPPLIER_NAME"));
            this._model.OrderNo = Convert.ToString(this.cbOrderNumber.EditValue);
            this._model.OrderKey = Convert.ToString(this.cbOrderNumber.GetColumnValue("WORK_ORDER_KEY"));
            this._model.PartKey = Convert.ToString(this.cbOrderNumber.GetColumnValue("PART_KEY"));
            this._model.PartNumber = Convert.ToString(this.cbOrderNumber.GetColumnValue("PART_NUMBER"));
            this._model.Count = int.Parse(string.IsNullOrEmpty(this.teCount.Text) ? "0" : this.teCount.Text);
            this._model.CreateTypeCode = Convert.ToString(this.lueCreateType.EditValue);
            this._model.CreateTypeName = Convert.ToString(this.lueCreateType.Text);
            this._model.ShiftName = Convert.ToString(this.cbShiftName.EditValue);
            this._model.UserName = this.txtUserId.Text;

            if (string.IsNullOrEmpty(this._model.RoomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                this.cbFactoryRoom.Select();
                return;
            }
            if (string.IsNullOrEmpty(this._model.OperationName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg005}"), MESSAGEBOX_CAPTION);//工序不能为空
                this.cbOperation.Select();
                return;
            }
            if (string.IsNullOrEmpty(this._model.ReceiveItemNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.Msg001}"), MESSAGEBOX_CAPTION);//领料项目号不能为空
                //MessageService.ShowMessage("领料项目号不能为空。", "提示");
                this.lueReceiveItemNo.Select();
                return;
            }
            if (string.IsNullOrEmpty(this._model.OrderNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                this.lueReceiveItemNo.Select();
                return;
            }
            if (this._model.Count == 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.Msg002}"), MESSAGEBOX_CAPTION);//投批数量不能为0
                //MessageService.ShowMessage("投批数量不能为0。", "提示");
                this.teCount.Select();
                return;
            }
            if (string.IsNullOrEmpty(this._model.CreateTypeCode))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreate.Msg003}"), MESSAGEBOX_CAPTION);//创建类别不能为空
                //MessageService.ShowMessage("创建类别不能为空。", "提示");
                this.lueCreateType.Select();
                return;
            }
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotCreateDetailViewContent view = new LotCreateDetailViewContent(this._model, this._isBatch);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 重置按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            this.cbOrderNumber.EditValue = null;
            this.lueReceiveItemNo.EditValue = null;
            this.lueProId.EditValue = null;
            this.teCount.Text = "1";
        }
        /// <summary>
        /// 关闭当前窗口。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 车间名称改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            if (this.IsBinding) return;
            this.IsBinding = true;
            try
            {
                BindWorkOrder();
                this.cbOrderNumber.EditValue = null;
                this.lueProId.EditValue = null;
                this.lueReceiveItemNo.EditValue = string.Empty;
                BindReceiveItemNo();
            }
            finally
            {
                this.IsBinding = false;
            }
        }
        /// <summary>
        /// 工序名称改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsBinding) return;
            this.IsBinding = true;
            try
            {
                this.cbOrderNumber.EditValue = null;
                this.lueProId.EditValue = null;
                BindReceiveItemNo();
            }
            finally
            {
                this.IsBinding = false;
            }
        }
        /// <summary>
        /// 工单号改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOrderNumber_EditValueChanged(object sender, EventArgs e)
        {
            if (this.IsBinding) return;
            this.IsBinding = true;
            try
            {
                this.lueProId.EditValue = null;
                BindReceiveItemNo();
            }
            finally
            {
                this.IsBinding = false;
            }
        }
        /// <summary>
        /// 产品号改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueProId_EditValueChanged(object sender, EventArgs e)
        {
            if (this.IsBinding) return;
            this.IsBinding = true;
            try
            {
                BindReceiveItemNo();
            }
            finally
            {
                this.IsBinding = false;
            }
        }
        /// <summary>
        /// 领料项目号改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueReceiveItemNo_EditValueChanged(object sender, EventArgs e)
        {
            if (this.IsBinding) return;
            this.IsBinding = true;
            try
            {
                string proId = Convert.ToString(this.lueReceiveItemNo.GetColumnValue("PRO_ID"));
                string orderNo = Convert.ToString(this.lueReceiveItemNo.GetColumnValue("AUFNR"));
                if (!string.IsNullOrEmpty(proId))
                {
                    this.lueProId.EditValue = proId;
                }

                if (!string.IsNullOrEmpty(orderNo))
                {
                    this.cbOrderNumber.EditValue = orderNo;
                }
                this.teCount.Select();
            }
            finally
            {
                this.IsBinding = false;
            }
        }
        /// <summary>
        /// 处理输入的不存在工单号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOrderNumber_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            this.cbOrderNumber.EditValue = null;
            e.Handled = true;
        }
        /// <summary>
        /// 处理输入的不存在产品ID。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueProId_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            this.lueProId.EditValue = null;
            e.Handled = true;
        }
        /// <summary>
        /// 处理输入的不存在领料项目号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueReceiveItemNo_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            this.lueReceiveItemNo.EditValue = null;
            e.Handled = true;
        }
        
    }
}
