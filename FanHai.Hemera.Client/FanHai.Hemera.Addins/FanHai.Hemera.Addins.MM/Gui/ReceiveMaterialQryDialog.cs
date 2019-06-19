using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.MM
{
    public partial class ReceiveMaterialQryDialog : BaseDialog
    {
        private DataTable dtParams=null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReceiveMaterialQryDialog()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0001}");//查询
            lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0002}");//领料车间
            lcMaterialLot.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0003}");//领料项目号
            lciWorkOrderNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0004}");//工单号
            lciProId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0005}");//产品ID号
            lciEfficiency.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0006}");//转换效率
            lciGrade.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0007}");//等级
            lciSupplierName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0008}");//原材料供应商
            lcMaterialReceiveTimeStar.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0009}");//领料时间-起：
            lcMaterialReceiveTimeEnd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0010}");//领料时间-末：
            smbtnConfirm.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0011}");//确定
            smbtnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.lbl.0012}");//取消
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReceiveMaterialQryDialog(DataTable dataParamTable)
        {
            InitializeComponent();
            this.dtParams = dataParamTable;
            InitializeLanguage();
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
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
                this.lueProId.Properties.ValueMember = "PRODUCT_CODE";
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
        /// 点击确定提交选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smbtnConfirm_Click(object sender, EventArgs e)
        {
            if (deReceiveMaterialStart.DateTime < deReceiveMaterialEnd.DateTime)
            {
                MapControlValueToItems();
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                //MessageService.ShowMessage("截止时间不能早于起始时间！","提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialQryDialog.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
            }
            
        }
        /// <summary>
        /// 关闭事件函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 把控件的值进行存储
        /// </summary>
        private void MapControlValueToItems()
        {
            dtParams.Rows.Add();
            dtParams.Rows[0]["CHARG"] = txtCHARG.Text.ToString().Trim();
            dtParams.Rows[0]["AUFNR"] = Convert.ToString(this.lueWorkOrderNo.EditValue);
            dtParams.Rows[0]["PRO_ID"] = Convert.ToString(this.lueProId.EditValue);
            dtParams.Rows[0]["EFFICIENCY"] = Convert.ToString(this.lueEfficiency.EditValue);
            dtParams.Rows[0]["GRADE"] = this.teGrade.Text;
            dtParams.Rows[0]["LLIEF"] = Convert.ToString(this.lueSupplierName.EditValue);
            dtParams.Rows[0]["RECEIVE_TIME_END"] = deReceiveMaterialEnd.Text.ToString();
            dtParams.Rows[0]["RECEIVE_TIME_START"] = deReceiveMaterialStart.Text.ToString();
            dtParams.Rows[0]["DO"] = "Query";
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveMaterialQryDialog_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindProId();
            BindSupplierName();
            BindEfficiency();

            this.deReceiveMaterialStart.DateTime = DateTime.Now.AddDays(-30);
            this.deReceiveMaterialEnd.DateTime = DateTime.Now;
            
        }
        /// <summary>
        /// 领料车间改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            //重新绑定工单
            BindWorkOrderNo();
        }
    }
}
