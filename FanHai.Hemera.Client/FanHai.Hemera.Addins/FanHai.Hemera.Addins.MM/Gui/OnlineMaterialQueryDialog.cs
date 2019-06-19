using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface.WarehouseManagement;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 线上物料查询条件对话框
    /// </summary>
    public partial class OnlineMaterialQueryDialog  : BaseDialog
    {
        /// <summary>
        /// 查询条件。
        /// </summary>
        public OnlineMaterialQueryModel Model
        {
            get;
            private set;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OnlineMaterialQueryDialog()
        {
            InitializeComponent();
            this.Model = new OnlineMaterialQueryModel();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0001}");//在线库存查询
            this.lciRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0002}");//工厂车间
            lciMaterialCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0003}");//物料编码
            this.lciMaterialLot.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0004}");//物料批号
            lciOperationName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0005}");//工序名称
            this.lciSupplierName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0006}");//供应商
            lciStoreName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0007}");//线上仓名称
            this.cmdOK.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0008}");//确定
            cmdCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryDialog.lbl.0009}");//取消
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlineMaterialQueryDialog_Load(object sender, EventArgs e)
        {
            BindRoom();
            BindOpeation();
            BindStore();
            InitControlValue();
        }
        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindRoom()
        {
            //根据PropertyService获取PROPERTY_FIELDS.STORES的值。从WST_STORES,FMM_LOCATION,FMM_LOCATION_RET
            //根据线边仓名称获取用户拥有权限的工厂车间名称集合绑定到窗体控件中，设置空为控件默认值。											
            DataTable dtFacRoom = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            DataRow dr = dtFacRoom.NewRow();
            dr["LOCATION_NAME"] =string.Empty;
            dtFacRoom.Rows.Add(dr);
            lueRoom.Properties.DataSource = dtFacRoom;
            lueRoom.Properties.DisplayMember = "LOCATION_NAME";
            lueRoom.Properties.ValueMember = "LOCATION_KEY";
        }
        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOpeation()
        {
            lueOperationName.Properties.Items.Clear();
            //根据登录用户将登录用户拥有权限的工序名称绑定到窗体控件中，设置空为控件默认值。
            //通过PropertyService获取PROPERTY_FIELDS.OPERATIONS的值"
            string strOperation = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            string[] strArrOperation = strOperation.Split(',');
            List<string> listOperation = strArrOperation.ToList<string>();
            listOperation.Insert(0, string.Empty);
            lueOperationName.Properties.Items.AddRange(listOperation);
        }
        /// <summary>
        /// 绑定线上仓。
        /// </summary>
        private void BindStore()
        {
            lueStoreName.Properties.Items.Clear();
            //根据登录用户将登录用户拥有权限的线边仓名称绑定到窗体控件中，设置空为控件默认值。
            //通过PropertyService获取PROPERTY_FIELDS.STORES的值"	
            string strStore = PropertyService.Get(PROPERTY_FIELDS.STORES);
            string[] strArrStore = strStore.Split(',');
            List<string> listStore = strArrStore.ToList<string>();
            listStore.Insert(0, string.Empty);
            lueStoreName.Properties.Items.AddRange(listStore);
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            this.lueRoom.EditValue = this.Model.RoomKey;
            this.teMaterialCode.Text = this.Model.MaterialCode;
            this.teMaterilLot.Text = this.Model.MaterialLot;
            this.lueOperationName.EditValue = this.Model.OperationName;
            this.teSupplierName.Text = this.Model.SupplierName;
            this.lueStoreName.EditValue = this.Model.StoreName;
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 确定按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Model.MaterialCode = this.teMaterialCode.Text;
            this.Model.MaterialLot = this.teMaterilLot.Text;
            this.Model.OperationName = this.lueOperationName.Text;
            this.Model.RoomKey = Convert.ToString(this.lueRoom.EditValue);
            this.Model.RoomName = this.lueRoom.Text;
            this.Model.StoreName = this.lueStoreName.Text;
            this.Model.SupplierName = this.teSupplierName.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
