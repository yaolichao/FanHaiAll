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
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示领料数据的窗体类。
    /// </summary>
    public partial class SendingBackMaterialCtrl : BaseUserCtrl
    {
        private ExchangeFlag exchangeType;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SendingBackMaterialCtrl(ExchangeFlag flag)
        {
            InitializeComponent();
            exchangeType = flag;
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0006}");//车间
            lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0007}");//工序名称
            lciStoreName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0008}");//设备
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0009}");//线别
            layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0010}");//参数类型
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0011}");//创建人
            lciMaterialLot.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0012}");//发料单号
            lciWorkorderNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0013}");//工单号
            layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0014}");//领料单号
            lciMaterialCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0015}");//原材料编码
            lciMaterialDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0016}");//原材料描述

            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0017}");//材料批次
            lciSupplierName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0018}");//原材料供应商

            lciIssueQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0019}");//退料数量
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0020}");//单位
            lblCanSendingBack.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0023}");//可退数量
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0021}");//备注
            layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0022}");//转换数量
            layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.lbl.0020}");//单位
        }
        public string _status = "Empty";                 //状态表明是新增的还是修改还是查询删除
        SendingMaterialEntity sendingMaterialEntity = new SendingMaterialEntity();
        private void Status(string _status)
        {
            if (_status == "EMPTY")
            {
                BindFactory();
                BindOperations();
                Bind_People();
                BindEquipment();
                BindLine();
                BindWorkOrderNo();
                BindParameter();
                BindMblnr();
                txtIssueQty.Text = "";
                txtDesc.Text = "";
                txtCQty.Text = "";
                this.lueMblnr.Properties.DataSource = null;
                this.lueMblnr.EditValue = string.Empty;
                this.lueMblnr.Text = string.Empty;
                this.lueWorkOrderNo.Properties.DataSource = null;
                this.lueWorkOrderNo.EditValue = string.Empty;
                this.lueWorkOrderNo.Text = string.Empty;
                this.lueMaterialCode.Properties.DataSource = null;
                this.lueMaterialCode.EditValue = string.Empty;
                this.txtSupplierCode.Properties.DataSource = null;
                this.txtSupplierCode.EditValue = string.Empty;
                this.txtSupplierCode.Text = string.Empty;
            }
        }
        /// <summary>窗体载入事件。
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualReceiveMaterialCtrl_Load(object sender, EventArgs e)
        {
            _status = "EMPTY";
            Status(_status);
        }
        #region 绑定数据
        /// <summary>绑定创建人信息
        /// 绑定创建人信息
        /// </summary>
        private void Bind_People()
        {
            try
            {
                string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                txtCreator.Text = name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }
        /// <summary>绑定车间
        /// 绑定车间
        /// </summary>
        private void BindFactory()
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
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        private void BindMblnr()
        {
            DataSet ds = GetMatEquData("BindMblnr");
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                //绑定工单号数据到窗体控件。
                this.lueMblnr.Properties.DataSource = ds.Tables[0];
                this.lueMblnr.Properties.DisplayMember = "MBLNR";
                this.lueMblnr.Properties.ValueMember = "MBLNR";
            }
            else
            {
                this.lueMblnr.Properties.DataSource = null;
                this.lueMblnr.EditValue = string.Empty;
                this.lueMblnr.Text = string.Empty;
            }
        }
        /// <summary>绑定工序。
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
        /// <summary>绑定设备
        /// 绑定设备
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.lueOperation.Text.Trim();
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
            SetLineValue();
        }
        /// <summary>绑定线别。
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
        /// <summary>绑定工单。
        /// 绑定工单。
        /// </summary>
        private void BindWorkOrderNo()
        {
            //string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            //WorkOrders wo = new WorkOrders();
            //DataSet ds = wo.GetWorkOrderByFactoryRoomKey(roomKey);
            //if (ds != null || ds.Tables[0].Rows.Count > 0)
            //{
            //    //绑定工单号数据到窗体控件。
            //    this.lueWorkOrderNo.Properties.DataSource = ds.Tables[0];
            //    this.lueWorkOrderNo.Properties.DisplayMember = "ORDER_NUMBER";
            //    this.lueWorkOrderNo.Properties.ValueMember = "ORDER_NUMBER";
            //    //this.lueWorkOrderNo.ItemIndex = 0;
            //}
            //else
            //{
            //    this.lueWorkOrderNo.Properties.DataSource = null;
            //    this.lueWorkOrderNo.EditValue = string.Empty;
            //    this.lueWorkOrderNo.Text = string.Empty;
            //}
            DataSet ds = GetMatEquData("BindWorkOrder");
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                //绑定工单号数据到窗体控件。
                this.lueWorkOrderNo.Properties.DataSource = ds.Tables[0];
                this.lueWorkOrderNo.Properties.DisplayMember = "ORDER_NUMBER";
                this.lueWorkOrderNo.Properties.ValueMember = "ORDER_NUMBER";
            }
            else
            {
                this.lueWorkOrderNo.Properties.DataSource = null;
                this.lueWorkOrderNo.EditValue = string.Empty;
                this.lueWorkOrderNo.Text = string.Empty;
            }
        }
        /// <summary>设置线别的值。
        /// 设置线别的值。
        /// </summary>
        private void SetLineValue()
        {
            string lineKey = Convert.ToString(this.lueEquipment.GetColumnValue("LINE_KEY"));
            this.lueLine.EditValue = lineKey;
        }
        /// <summary>绑定物料编码。
        /// 绑定物料编码。
        /// </summary>
        private void BindMaterialCode()
        {
            DataSet ds = GetMatEquData("BindMatCode");
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                this.lueMaterialCode.Properties.DataSource = ds.Tables[0];
                this.lueMaterialCode.Properties.DisplayMember = "MATERIAL_CODE";
                this.lueMaterialCode.Properties.ValueMember = "MATERIAL_CODE";
            }
            else
            {
                this.lueMaterialCode.Properties.DataSource = null;
                this.lueMaterialCode.EditValue = string.Empty;
            }
        }

        private DataSet GetMatEquData(string _type)
        {
            string orderNumber = this.lueWorkOrderNo.Text;
            string facKey = this.lueFactoryRoom.EditValue == null ? "" : this.lueFactoryRoom.EditValue.ToString();
            string equipmentKey = this.lueEquipment.EditValue == null ? "" : this.lueEquipment.EditValue.ToString();
            string operationName = this.lueOperation.Text.Trim();
            string lineKey = this.lueLine.EditValue == null ? "" : this.lueLine.EditValue.ToString();
            string parameterKey = this.lueType.EditValue == null ? "" : this.lueType.EditValue.ToString();
            string matCode = this.lueMaterialCode.Text.Trim();
            return sendingMaterialEntity.GetMatEquipmentStore(facKey, equipmentKey, operationName, lineKey, parameterKey, matCode, orderNumber,_type);

        }
        /// <summary>绑定参数  只绑定维护了扣料设置有效的参数信息
        /// 绑定参数
        /// </summary>
        private void BindParameter()
        {
            DataSet dsParameters = sendingMaterialEntity.GetParameters();
            if (dsParameters != null || dsParameters.Tables[0].Rows.Count > 0)
            {
                lueType.Properties.DataSource = dsParameters.Tables[0];
                this.lueType.Properties.DisplayMember = "PARAMETER";
                this.lueType.Properties.ValueMember = "PARAMETER_KEY";
                this.lueType.ItemIndex = 0;
                this.lueType.Properties.PopupFormSize = new Size(220, 140); 
            }
            else
            {
                this.lueType.Properties.DataSource = null;
                this.lueType.EditValue = string.Empty;
                this.lueType.Text = string.Empty;
            }
        }
        /// <summary>绑定参数  只绑定维护了扣料设置有效的参数信息
        /// 绑定参数
        /// </summary>
        private void BindSupplierCode()
        {
            DataSet ds = GetMatEquData("BindSupplierCode");
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                txtSupplierCode.Properties.DataSource = ds.Tables[0];
                this.txtSupplierCode.Properties.DisplayMember = "SUPPLIER_CODE";
                this.txtSupplierCode.Properties.ValueMember = "SUPPLIER_CODE";
            }
            else
            {
                this.txtSupplierCode.Properties.DataSource = null;
                this.txtSupplierCode.EditValue = string.Empty;
                this.txtSupplierCode.Text = string.Empty;
            }
        }


        #endregion
        #region change事件
        /// <summary>车间变更值绑定工序
        /// 车间变更值绑定工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            BindOperations();
            BindLine();
        }

        private void lueOperation_EditValueChanged(object sender, EventArgs e)
        {
            BindEquipment();
            BindMaterialCode();
        }

        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            BindLine(); 
            SetLineValue();
            BindWorkOrderNo();
            BindMaterialCode();
        }

        private void lueWorkOrderNo_EditValueChanged(object sender, EventArgs e)
        {
            BindMaterialCode();
            BindMblnr();
        }

        private void lueMaterialCode_EditValueChanged(object sender, EventArgs e)
        {
            BindSupplierCode();
            BindMblnr();
            string desc = Convert.ToString(this.lueMaterialCode.GetColumnValue("DESCRIPTION"));
            txtMaterialDescription.Text = desc;
        }

        private void lueType_EditValueChanged(object sender, EventArgs e)
        {
            string unit = Convert.ToString(this.lueType.GetColumnValue("USE_UNIT"));
            this.txtUnit.EditValue = unit;
            string cunit = Convert.ToString(this.lueType.GetColumnValue("USE_CONRTAST_UNIT"));
            this.txtCunit.EditValue = cunit;
            BindMaterialCode();
        }

        #endregion
        private void tspNew_Click(object sender, EventArgs e)
        {
            _status = "EMPTY";
            Status(_status);
        }

        private void tspSave_Click(object sender, EventArgs e)
        {
            bool IsTrue = false;
            //if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")))
               
            {
                if (string.IsNullOrEmpty(lueFactoryRoom.Text.Trim()))
                {
                    //MessageService.ShowMessage("工厂车间不能为空", "保存"); 
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                if (string.IsNullOrEmpty(lueOperation.Text.Trim()))
                {
                    //MessageService.ShowMessage("工序不能为空", "保存"); 
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(lueEquipment.Text.Trim()))
                {
                    //MessageService.ShowMessage("设备名称不能为空", "保存");    
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(lueLine.Text.Trim()))
                {
                    //MessageService.ShowMessage("线别不能为空", "保存");  
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(lueType.Text.Trim()))
                {
                    //MessageService.ShowMessage("采集参数不能为空", "保存");    
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(lueWorkOrderNo.Text.Trim()))
                {
                    //MessageService.ShowMessage("工单号不能为空", "保存");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(lueMblnr.Text.Trim()))
                {
                    //MessageService.ShowMessage("领料单号不能为空", "保存");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(lueMaterialCode.Text.Trim()))
                {
                    //MessageService.ShowMessage("物料名称不能为空", "保存");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(txtSupplierCode.Text.Trim()))
                {
                    //MessageService.ShowMessage("材料批号不能为空", "保存");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (string.IsNullOrEmpty(txtIssueQty.Text.Trim()))
                {
                    //MessageService.ShowMessage("扣料数量不能为空", "保存");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0011}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                string facName = lueFactoryRoom.Text.Trim();
                string opration = lueOperation.Text.Trim();
                string equipment = lueEquipment.Text.Trim();
                string line = lueLine.Text.Trim();
                string type = lueType.Text.Trim();
                string workOrder = lueWorkOrderNo.Text.Trim();
                string mblnr = lueMblnr.Text.Trim();
                string materialCode = lueMaterialCode.Text.Trim();
                string supplierCode = txtSupplierCode.Text.Trim();
                string issueQty = txtIssueQty.Text.Trim();
                string name = txtCreator.Text.Trim();
                string materialDesc = txtMaterialDescription.Text.Trim();
                string supplierName= lueSupplierName.Text.Trim();
                string unit = txtUnit.Text.Trim();
                string cQty = txtCQty.Text.Trim();
                string cUnit = txtCunit.Text.Trim();
                string desc = txtDesc.Text.Trim();
                string useqty = Convert.ToString(lueType.GetColumnValue("USE_QTY"));
                string useCqty =Convert.ToString(lueType.GetColumnValue("USE_CONRTAST_QTY"));
                string resulte = "0";
                if (!string.IsNullOrEmpty(useqty) && !string.IsNullOrEmpty(useCqty) && !string.IsNullOrEmpty(issueQty) &&
                    !useqty.Equals("0") && !useCqty.Equals("0") && !issueQty.Equals("0"))
                {
                    resulte =Convert.ToString(Convert.ToDecimal(issueQty) * Convert.ToDecimal(useCqty) / Convert.ToDecimal(useqty));
                }
                //数据绑定
                DataTable dtInf = new DataTable();
                dtInf.Columns.Add("SENDING_NUMBER");
                dtInf.Columns.Add("FACTORY_NAME");
                dtInf.Columns.Add("FACTORY_KEY");
                dtInf.Columns.Add("OPERATION_NAME");
                dtInf.Columns.Add("EQUIPMENT_NAME");
                dtInf.Columns.Add("EQUIPMENT_KEY");
                dtInf.Columns.Add("LING_NAME");
                dtInf.Columns.Add("LINE_KEY");
                dtInf.Columns.Add("PARAMETER");
                dtInf.Columns.Add("PARAMETER_KEY");
                dtInf.Columns.Add("WORK_ORDER"); 
                dtInf.Columns.Add("MATERIAL_CODE");
                dtInf.Columns.Add("MATERIAL_DESC");
                dtInf.Columns.Add("MBLNR");
                dtInf.Columns.Add("SUPPLIER");
                dtInf.Columns.Add("SUPPLIER_CODE");
                dtInf.Columns.Add("SENDING_QTY");
                dtInf.Columns.Add("SENDING_UNIT");
                dtInf.Columns.Add("CONRTAST_QTY");
                dtInf.Columns.Add("CONRTAST_UNIT");
                dtInf.Columns.Add("CREATOR");
                dtInf.Columns.Add("MEMO");
                DataRow dr = dtInf.NewRow();
                dr["SENDING_NUMBER"] = "";
                dr["FACTORY_NAME"] = facName;
                dr["FACTORY_KEY"] = lueFactoryRoom.EditValue.ToString();
                dr["OPERATION_NAME"] = opration;
                dr["EQUIPMENT_NAME"] = equipment;
                dr["EQUIPMENT_KEY"] = lueEquipment.EditValue.ToString();
                dr["LING_NAME"] = line;
                dr["LINE_KEY"] = lueLine.EditValue.ToString();
                dr["PARAMETER"] = type;
                dr["PARAMETER_KEY"] = lueType.EditValue.ToString();
                dr["WORK_ORDER"] = workOrder;
                dr["MATERIAL_CODE"] = materialCode;
                dr["MATERIAL_DESC"] = materialDesc;
                dr["MBLNR"] = mblnr;
                dr["SUPPLIER"] = supplierName;
                dr["SUPPLIER_CODE"] = supplierCode;
                dr["SENDING_QTY"] = issueQty;
                dr["SENDING_UNIT"] = unit;
                dr["CONRTAST_QTY"] = Convert.ToString(resulte);
                dr["CONRTAST_UNIT"] = cUnit;
                dr["CREATOR"] = name;
                dr["MEMO"] = desc;

                dtInf.Rows.Add(dr);
                dtInf.TableName = "MM_SENDINGLIST";
                if (exchangeType == ExchangeFlag.Sending)
                {//状态为new
                    if (sendingMaterialEntity.InsertNewInf(dtInf))
                    {//新增成功
                        IsTrue = true;
                    }
                }
                else
                {//状态不为new
                    if (sendingMaterialEntity.UpdateParameterInf(dtInf))
                    {//修改成功
                        IsTrue = true;
                    }
                }
                if (IsTrue == true)
                {
                    _status = "EMPTY";
                    Status(_status);
                }          
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        private void txtSupplierCode_EditValueChanged(object sender, EventArgs e)
        {
            string sumQty = Convert.ToString(this.txtSupplierCode.GetColumnValue("SUM_QTY"));
            string supplierName = Convert.ToString(this.txtSupplierCode.GetColumnValue("SUPPLIER"));
            this.txtCanSendingBack.Text = sumQty;
            this.lueSupplierName.Text = supplierName;
        }

        private void tspSelect_Click(object sender, EventArgs e)
        {
            SendingMaterialEntity sendingMaterial = new SendingMaterialEntity();
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //if (viewContent.TitleName == StringParser.Parse("发料-退料清单"))
                if (viewContent.TitleName == StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.msg.0012}")))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            SendMaterialListContent vc = new SendMaterialListContent();
            WorkbenchSingleton.Workbench.ShowView(vc);
        }

    }
}
