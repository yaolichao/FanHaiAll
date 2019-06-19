using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraLayout;


namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示领料数据的窗体类。
    /// </summary>
    public partial class ManualReceiveMaterialCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 是否更新之前的领料项目号信息？
        /// </summary>
        private bool IsUpdateOldReceiveMaterialInfo = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManualReceiveMaterialCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0002}");//领料车间
            lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0003}");//工序名称
            lciStoreName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0004}");//线上仓名称
            lciMaterialLot.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0005}");//领料项目号
            lciWorkorderNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0006}");//工单号
            lciProId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0007}");//产品ID号
            lciEfficiency.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0008}");//转换效率
            lciGrade.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0009}");//等级
            lciSupplierName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0010}");//原材料供应商
            lciSupplierCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0011}");//供应商编号
            lciMaterialCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0012}");//原材料编码
            btnOwnMaterial.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0013}");//自备料
            lciMaterialDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0014}");//原材料描述
            lciIssueQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0015}");//数量
            lciMemo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0016}");//备注

            lciIssueNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0020}");//配料单号
            lciShiftName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0021}");//班别
            lciOperatorNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0022}");//员工号
            lciUnit.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0023}");//单位
            lciSupplierLot.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0024}");//供应商批号
            lciIssueStore.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.lbl.0025}");//领料库位
        }

        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualReceiveMaterialCtrl_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindOperations();
            //BindStores();
            //BindWorkOrderNo();
            BindProId();
            BindSupplierName();
            BindEfficiency();

            BindUserName();
            BindShiftName();
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.lueFactoryRoom.Properties.ReadOnly = false;
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);
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
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            this.lueStoreName.Properties.ReadOnly = false;
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    lueOperation.Properties.Items.Add(strOperations[i]);
                }
                this.lueOperation.SelectedIndex = 0;
            }
            //禁用工序
            if (string.IsNullOrEmpty(operations)
                || this.lueOperation.Properties.Items.Count <= 1)
            {
                this.lueOperation.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定线上仓。
        /// </summary>
        private void BindStores()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string operation = Convert.ToString(this.lueOperation.Text);
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.lueStoreName.Properties.ReadOnly = false;
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetStores(operation, roomKey, stores);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueStoreName.Properties.DataSource = ds.Tables[0];
                this.lueStoreName.Properties.DisplayMember = "STORE_NAME";
                this.lueStoreName.Properties.ValueMember = "STORE_KEY";
                this.lueStoreName.ItemIndex = 0;
            }
            else
            {
                this.lueStoreName.Properties.DataSource = null;
                this.lueStoreName.EditValue = string.Empty;
            }
            //禁用线上仓
            if (this.lueStoreName.Properties.DataSource == null
                || ds.Tables[0].Rows.Count <= 1)
            {
                this.lueStoreName.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定工单。
        /// </summary>
        private void BindWorkOrderNo()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            WorkOrders wo = new WorkOrders();
            DataSet ds = wo.GetWorkOrderByFactoryRoomKey(roomKey);
            if (string.IsNullOrEmpty(wo.ErrorMsg))
            {
                //绑定工单号数据到窗体控件。
                this.lueWorkOrderNo.Properties.DataSource = ds.Tables[0];
                this.lueWorkOrderNo.Properties.DisplayMember = "ORDER_NUMBER";
                this.lueWorkOrderNo.Properties.ValueMember = "ORDER_NUMBER";
                //this.lueWorkOrderNo.ItemIndex = 0;
            }
            else
            {
                this.lueWorkOrderNo.Properties.DataSource = null;
                this.lueWorkOrderNo.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定产品ID号。
        /// </summary>
        private void BindProId()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetProdId();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
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
        /// 绑定转换效率。
        /// </summary>
        private void BindEfficiency()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetEfficiency();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
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
        /// 绑定物料编码。
        /// </summary>
        private void BindMaterialCode()
        {
            string orderNumber = this.lueWorkOrderNo.Text;
            this.lueMaterialCode.EditValue = string.Empty;
            this.txtMaterialDescription.Text = string.Empty;
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetMaterials(orderNumber);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueMaterialCode.Properties.DataSource = ds.Tables[0];
                this.lueMaterialCode.Properties.DisplayMember = "MATERIAL_CODE";
                this.lueMaterialCode.Properties.ValueMember = "MATERIAL_KEY";
                this.lueMaterialCode.ItemIndex = 0;
            }
            else
            {
                this.lueMaterialCode.Properties.DataSource = null;
                this.lueMaterialCode.EditValue = string.Empty;
                this.txtMaterialDescription.Text = string.Empty;
            }
        }
        /// <summary>
        /// 绑定供应商名称。
        /// </summary>
        private void BindSupplierName()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetSuppliers();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueSupplierName.Properties.DataSource = ds.Tables[0];
                this.lueSupplierName.Properties.DisplayMember = "NAME";
                this.lueSupplierName.Properties.ValueMember = "NAME";
            }
            else
            {
                this.lueSupplierName.Properties.DataSource = null;
                this.lueSupplierName.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定领料线边仓库位
        /// </summary>
        private void BindIssueStore()
        {
            string roomName = this.lueFactoryRoom.Text;
            this.lueIssueStore.Properties.Items.Clear();
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataTable dt = entity.GetIssueStores(roomName);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string store = Convert.ToString(dt.Rows[i][0]);
                    this.lueIssueStore.Properties.Items.Add(store);
                }
            }
            this.lueIssueStore.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定用户名。
        /// </summary>
        private void BindUserName()
        {
            this.txtOperatorNumber.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.txtOperatorNumber.Properties.ReadOnly = true;
        }
        /// <summary>
        /// 绑定班别信息
        /// </summary>
        public void BindShiftName()
        {
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");

            this.lueShiftName.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShiftName.Properties.DisplayMember = "CODE";
            this.lueShiftName.Properties.ValueMember = "CODE";

            Shift shift = new Shift();
            this.lueShiftName.EditValue = shift.GetCurrShiftName();
        }
        /// <summary>
        /// 车间名称改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            //重新绑定线上仓
            BindStores();
            //重新绑定工单
            BindWorkOrderNo();
            //绑定领料线边仓库位
            BindIssueStore();
        }
        /// <summary>
        /// 工序名称改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //重新绑定线上仓
            BindStores();
        }
        /// <summary>
        /// 物料编码改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueMaterialCode_EditValueChanged(object sender, EventArgs e)
        {
            string materialName = Convert.ToString(this.lueMaterialCode.GetColumnValue("MATERIAL_NAME"));
            string unit = Convert.ToString(this.lueMaterialCode.GetColumnValue("UNIT"));
            this.txtMaterialDescription.Text = materialName;
            this.txtUnit.Text = unit;
        }
        /// <summary>
        /// 保存按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //判断是否通过验证。
            if (!this.Validate())
            {
                return;
            }
            string factoryRoomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string operationName = this.lueOperation.Text;
            string storeKey = Convert.ToString(this.lueStoreName.EditValue);

            string materialLot = this.txtMaterialLot.Text.Trim().ToUpper();
            string orderNumber = Convert.ToString(this.lueWorkOrderNo.Text);
            string proId = Convert.ToString(this.lueProId.Text);
            string efficiency = Convert.ToString(this.lueEfficiency.Text);
            string grade = Convert.ToString(this.teGrade.Text);
            string supplierName = Convert.ToString(this.lueSupplierName.Text);
            string supplierCode = this.teSupplierCode.Text.Trim();
            string materialKey = Convert.ToString(this.lueMaterialCode.EditValue);
            string materialCode = this.lueMaterialCode.Text.Trim();
            string materialDescription = this.txtMaterialDescription.Text.Trim();
            string issueQty = this.txtIssueQty.Text.Trim();
            string memo = Convert.ToString(this.meMemo.Text);

            string issueNo = this.txtIssueNo.Text.Trim();
            if (string.IsNullOrEmpty(issueNo))
            {
                issueNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            string supplierLot = this.txtSupplierLot.Text.Trim();
            string issueStore = Convert.ToString(this.lueIssueStore.EditValue);
            string unit = this.txtUnit.Text;
            string userName = this.txtOperatorNumber.Text;
            string timeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            string shiftName = Convert.ToString(this.lueShiftName.EditValue);
            if (string.IsNullOrEmpty(shiftName))
            {
                //MessageService.ShowMessage("没有进行排班，请先排班。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                
                this.lueShiftName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(factoryRoomKey))
            {
                //MessageService.ShowMessage("车间名称必须选择。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueFactoryRoom.Focus();
                return;
            }
            if (string.IsNullOrEmpty(operationName))
            {
                //MessageService.ShowMessage("工序名称必须选择。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueOperation.Focus();
                return;
            }
            if (string.IsNullOrEmpty(storeKey))
            {
                //MessageService.ShowMessage("线上仓名称必须选择。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueStoreName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(materialLot))
            {
                //MessageService.ShowMessage("领料项目号必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtMaterialLot.Focus();
                return;
            }
            if (string.IsNullOrEmpty(orderNumber))
            {
                //MessageService.ShowMessage("工单号必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueWorkOrderNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(proId))
            {
                //MessageService.ShowMessage("产品ID号必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueProId.Focus();
                return;
            }
            if (string.IsNullOrEmpty(efficiency))
            {
                //MessageService.ShowMessage("转换效率必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueEfficiency.Focus();
                return;
            }
            if (string.IsNullOrEmpty(grade))
            {
                //MessageService.ShowMessage("等级必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.teGrade.Focus();
                return;
            }
            if (string.IsNullOrEmpty(supplierName))
            {
                //MessageService.ShowMessage("原材料供应商必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueSupplierName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(supplierCode))
            {
                //MessageService.ShowMessage("供应商编号必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0011}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.teSupplierCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(materialKey))
            {
                //MessageService.ShowMessage("原材料编码必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0012}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.lueMaterialCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(issueQty))
            {
                //MessageService.ShowMessage("数量必须输入。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0013}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.txtIssueQty.Focus();
                return;
            }
            if (!ValidMaterialLotIsMatch())
            {
                return;
            }

            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("FACTORYROOM_KEY", factoryRoomKey);
                ht.Add("OPERATION_NAME", operationName);
                ht.Add("STORE_KEY", storeKey);
                ht.Add("MATERIAL_LOT", materialLot);
                ht.Add("ORDER_NUMBER", orderNumber);
                ht.Add("PRO_ID", proId);
                ht.Add("EFFICIENCY", efficiency);
                ht.Add("GRADE", grade);
                ht.Add("SUPPLIER_NAME", supplierName);
                ht.Add("SUPPLIER_CODE", supplierCode);
                ht.Add("MATERIAL_KEY", materialKey);
                ht.Add("MATERIAL_CODE", materialCode);
                ht.Add("MATERIAL_DESCRIPTION", materialDescription);
                ht.Add("ISSUE_QTY", issueQty);
                ht.Add("MEMO", memo);
                ht.Add("ISSUE_NO", issueNo);
                ht.Add("SUPPLIER_LOT", supplierLot);
                ht.Add("ISSUE_STORE", issueStore);
                ht.Add("UNIT", unit);
                ht.Add("USER_NAME", userName);
                ht.Add("TIME_ZONE", timeZone);
                ht.Add("SHIFT_NAME", shiftName);
                ht.Add("OEM", string.Empty);
                //ht.Add("IsUpdateOldReceiveMaterialInfo", IsUpdateOldReceiveMaterialInfo);
                ht.Add("IsUpdateOldReceiveMaterialInfo", true);
                entity.ManualSaveReceiveMaterial(ht);
                if (!string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    MessageBox.Show(entity.ErrorMsg);
                }
                else
                {
                    //MessageService.ShowMessage("保存成功。", "提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0014}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    ResetTextValue();
                    this.txtMaterialLot.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 工单号码改变时触发事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueWorkOrderNo_EditValueChanged(object sender, EventArgs e)
        {
            BindMaterialCode();
            //根据工单号获取PRO_ID
            string proId = Convert.ToString(this.lueWorkOrderNo.GetColumnValue("PRO_ID"));
            if (!string.IsNullOrEmpty(proId))
            {
                this.lueProId.EditValue = proId;
                if (this.lueProId.ItemIndex >= 0)
                {
                    this.lueProId.Properties.ReadOnly = true;
                }
            }
            else
            {
                this.lueProId.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 供应商名称发生改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueSupplierName_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.lueSupplierName.Text))
            {
                string nickname = Convert.ToString(this.lueSupplierName.GetColumnValue("NICKNAME"));
                this.teSupplierCode.Text = nickname;
            }
        }
        /// <summary>
        /// 验证领料项目号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaterialLot_Properties_Validating(object sender, CancelEventArgs e)
        {
            EnableControl();
            this.IsUpdateOldReceiveMaterialInfo = false;

            //领料日期（6） 电池供应商简码（3） 电池片效率（4） 电池片等级（1~2）- 电池片对角线（3）- 工单号（9）
            //string formatMessage = "领料项目号格式：领料日期（6码） 电池供应商简码（3码） 电池片效率（4码） 电池片等级（1~2码）- 电池片对角线（3码)-工单号（9码）。";
            string formatMessage = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0015}");
            string val = txtMaterialLot.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(val))
            {
                return;
            }
            //长度等于27或者28
            if (val.Length < 27 && val.Length > 32)
            {
                e.Cancel = true;
                txtMaterialLot.ErrorText = formatMessage;
                return;
            }
            string[] items = val.Split('-');
            if (items.Length != 3
                || items[0].Length < 13
                || items[1].Length < 3
                || items[2].Length < 9)
            {
                e.Cancel = true;
                txtMaterialLot.ErrorText = formatMessage;
                return;
            }
            AutoInputValueByMaterialLot();
        }
        /// <summary>
        /// 根据领料项目号自动填充工单号、等级和供应商名称。
        /// </summary>
        /// 领料日期（6） 电池供应商简码（3） 电池片效率（4） 电池片等级（1~2）- 电池片对角线（3）- 工单号（9）
        private void AutoInputValueByMaterialLot()
        {
            string val = txtMaterialLot.Text.Trim().ToUpper();
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet dsReturn = entity.GetReceiveMaterialLotInfo(val);
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowMessage(entity.ErrorMsg, StringParser.Parse("${res:Global.SystemInfo}"));
            }
            else
            {
                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    DataTable dtReturn = dsReturn.Tables[0];
                    string workOrder = Convert.ToString(dtReturn.Rows[0]["AUFNR"]);
                    string grade = Convert.ToString(dtReturn.Rows[0]["GRADE"]);
                    string supplierCode = Convert.ToString(dtReturn.Rows[0]["SUPPLIER_CODE"]);
                    string efficiency = Convert.ToString(dtReturn.Rows[0]["EFFICIENCY"]);
                    //string proId = Convert.ToString(dtReturn.Rows[0]["PRO_ID"]);
                    string supplierName = Convert.ToString(dtReturn.Rows[0]["LLIEF"]);
                    string materialCode = Convert.ToString(dtReturn.Rows[0]["MATNR"]);
                    string materialDesc = Convert.ToString(dtReturn.Rows[0]["MATXT"]);
                    string materialKey = Convert.ToString(dtReturn.Rows[0]["MATERIAL_KEY"]);
                    this.lueWorkOrderNo.EditValue = workOrder;
                    //this.lueProId.EditValue = proId;
                    this.lueEfficiency.EditValue = efficiency;
                    this.teGrade.Text = grade;
                    this.lueSupplierName.EditValue = supplierName;
                    this.teSupplierCode.Text = supplierCode;
                    this.lueMaterialCode.EditValue = materialKey;
                    this.txtMaterialDescription.Text = materialDesc;

                    DisableControl();
                }
                else
                {
                    string[] items = val.Split('-');
                    this.lueWorkOrderNo.EditValue = items[2];                           //工单号
                }
            }
        }
        /// <summary>
        /// 启用控件。
        /// </summary>
        private void EnableControl()
        {
            this.lueWorkOrderNo.Properties.ReadOnly = false;
            //this.lueProId.Properties.ReadOnly = false;
            this.lueEfficiency.Properties.ReadOnly = false;
            this.teGrade.Properties.ReadOnly = false;
            this.lueSupplierName.Properties.ReadOnly = false;
            //this.lueMaterialCode.Properties.ReadOnly = false;
        }
        /// <summary>
        /// 禁用控件。
        /// </summary>
        private void DisableControl()
        {
            this.lueWorkOrderNo.Properties.ReadOnly = true;
            //this.lueProId.Properties.ReadOnly = true;
            this.lueEfficiency.Properties.ReadOnly = true;
            this.teGrade.Properties.ReadOnly = true;
            this.lueSupplierName.Properties.ReadOnly = true;
            //this.lueMaterialCode.Properties.ReadOnly = true;
        }
        /// <summary>
        /// 验证领料项目号是否匹配对应的输入信息。
        /// </summary>
        /// 领料日期（6） 电池供应商简码（3） 电池片效率（4） 电池片等级（1~2）- 电池片对角线（3）- 工单号（9）
        private bool ValidMaterialLotIsMatch()
        {
            string val = txtMaterialLot.Text.Trim().ToUpper();
            string[] items = val.Split('-');
            string workOrder = Convert.ToString(this.lueWorkOrderNo.EditValue);
            string supplierCode = this.teSupplierCode.Text.Trim();
            string grade = this.teGrade.Text.Trim();
            ////工单号不匹配
            //if (items[2] != workOrder)
            //{
            //    MessageService.ShowMessage("工单号不匹配。", "提示");
            //    return false;
            //}
            //供应商简码不匹配
            if (items[0].IndexOf(supplierCode) < 0)
            {
                //MessageService.ShowMessage("供应商简码不匹配。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0016}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            //电池片等级不匹配
            if (items[0].IndexOf(grade)<0)
            {
                //MessageService.ShowMessage("电池片等级不匹配。", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0016}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            ResetTextValue();
        }
        /// <summary>
        /// 关闭按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 编辑领料项目信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            EnableControl();
            this.IsUpdateOldReceiveMaterialInfo = true;
        }
        /// <summary>
        /// 重置文本值。
        /// </summary>
        private void ResetTextValue()
        {
            this.txtMaterialLot.Text = string.Empty;
            this.lueWorkOrderNo.EditValue = string.Empty;
            this.lueProId.EditValue = string.Empty;
            this.lueProId.Properties.ReadOnly = false;
            this.lueEfficiency.EditValue = string.Empty;
            this.teGrade.Text = string.Empty;
            this.lueSupplierName.EditValue = string.Empty;
            this.teSupplierCode.Text = string.Empty;
            this.lueMaterialCode.EditValue = string.Empty;
            this.txtMaterialDescription.Text = string.Empty;
            this.txtIssueQty.Text = string.Empty;
            this.meMemo.Text = string.Empty;
            EnableControl();
            this.IsUpdateOldReceiveMaterialInfo = false;
        }
        /// <summary>
        /// 打开物料清单页面
        /// </summary>
        private void tsbReceiveList_Click(object sender, EventArgs e)
        {
            ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.Title}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            ReceiveMaterialListViewContent vc = new ReceiveMaterialListViewContent();
            WorkbenchSingleton.Workbench.ShowView(vc);
        }
        /// <summary>
        /// 供应商栏位回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueSupplierName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.lueSupplierName_EditValueChanged(sender, e);
                this.teSupplierCode.Select();
            }
        }
        /// <summary>
        /// 车间栏位回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.lueFactoryRoom_EditValueChanged(sender, e);
                this.lueOperation.Select();
            }
        }
        /// <summary>
        /// 工序栏位回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueOperation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.lueOperation_SelectedIndexChanged(sender, e);
                this.lueStoreName.Select();
            }
        }
        /// <summary>
        /// 原材料编码栏位回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueMaterialCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.lueMaterialCode_EditValueChanged(sender, e);
                this.txtMaterialDescription.Select();
            }
        }
        /// <summary>
        /// 工单栏位回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueWorkOrderNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.lueWorkOrderNo_EditValueChanged(sender, e);
                this.lueProId.Select();
            }
        }
        /// <summary>
        /// 自备料对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOwnMaterial_Click(object sender, EventArgs e)
        {
            string orderNumber = Convert.ToString(this.lueWorkOrderNo.EditValue);
            if (string.IsNullOrEmpty(orderNumber))
            {
                //MessageService.ShowMessage("请先输入工单号。");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.msg.0017}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            BomMaterialInputDialog dlg = new BomMaterialInputDialog(orderNumber);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                BindMaterialCode();
                this.lueMaterialCode.Text = dlg.MaterialCode;
            }
            dlg.Dispose();
            dlg = null;
        }
    }
}
