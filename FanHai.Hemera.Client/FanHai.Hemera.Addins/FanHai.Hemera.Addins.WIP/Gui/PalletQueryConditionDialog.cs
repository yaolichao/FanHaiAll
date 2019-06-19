using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次查询条件的对话框。
    /// </summary>
    public partial class PalletQueryConditionDialog : BaseDialog
    {
        /// <summary>
        /// 批次查询的参数数据。
        /// </summary>
        public PalletQueryConditionModel Model
        {
            get;
            private set;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PalletQueryConditionDialog()
        {
            InitializeComponent();
            this.Model = new PalletQueryConditionModel();
        }
        /// <summary>
        /// 关闭查询对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 确定按钮Click事件处理函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string palletNo = this.tePalletNo.Text.Trim();
            string factoryRoomKey = Convert.ToString(this.lueFactoryRoom.EditValue); ;
            if (string.IsNullOrEmpty(factoryRoomKey))
            {
                MessageService.ShowMessage("请选择车间。", "系统提示");
                this.lueFactoryRoom.Select();
                return;
            }
            if (palletNo.Length>2048)
            {
                MessageService.ShowMessage("托盘号长度不能超过2048个字符。", "系统提示");
                this.tePalletNo.Select();
                return;
            }
            //开始时间>结束时间
            if (this.deStartCreateDate.DateTime > this.deEndCreateDate.DateTime)
            {
                MessageService.ShowMessage("开始时间必须小于结束时间。", "系统提示");
                this.deStartCreateDate.Select();
                return;
            }
            this.Model.RoomKey = factoryRoomKey;
            this.Model.RoomName = Convert.ToString(this.lueFactoryRoom.Text);
            this.Model.PalletNo = this.tePalletNo.Text.Trim();
            this.Model.PalletNo1 = this.tePalletNoEnd.Text.Trim();
            this.Model.WorkOrderNumber = this.teWorkOrderNumber.Text.ToUpper().Trim();
            this.Model.ProId = this.teProId.Text.ToUpper().Trim();
            this.Model.StateFlag = Convert.ToString(this.lueState.EditValue);

            this.Model.StartCreateDate = this.deStartCreateDate.Text;
            this.Model.EndCreateDate = this.deEndCreateDate.Text;
            this.Model.PartNumber = this.tePartNumber.Text.Trim();
            this.Model.StartTowarehouseDate = this.deToWarehouseDateStart.Text;
            this.Model.EndTowarehouseDate = this.deToWarehoseDateEnd.Text;

            this.Model.StartCheckDate = this.deToWarehouseCheckDateStart.Text;
            this.Model.EndCheckDate = this.deToWarehouseCheckDateEnd.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotQueryConditionDialog_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindStateFlag();
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
        /// 绑定批次状态。
        /// </summary>
        private void BindStateFlag()
        {
            //0：包装中；1：包装；2：入库检；3:已入库；4：已出货
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("CODE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add("包装中", 0);
            dtState.Rows.Add("待入库检",1);
            dtState.Rows.Add("待入库", 2);
            dtState.Rows.Add("已入库", 3);
            dtState.Rows.Add("已出货", 4);
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
            this.tePalletNo.Text = this.Model.PalletNo;
            this.tePalletNoEnd.Text = this.Model.PalletNo1;
            this.lueState.EditValue = this.Model.StateFlag;
            this.teWorkOrderNumber.Text = this.Model.WorkOrderNumber;
            this.teProId.Text = this.Model.ProId;

            this.deStartCreateDate.Text =this.Model.StartCreateDate;
            this.deEndCreateDate.Text = this.Model.EndCreateDate;

            this.deToWarehouseCheckDateStart.Text = this.Model.StartCheckDate;
            this.deToWarehouseCheckDateEnd.Text = this.Model.EndCheckDate;

            this.deToWarehouseDateStart.Text = this.Model.StartTowarehouseDate;
            this.deToWarehoseDateEnd.Text = this.Model.EndTowarehouseDate;
        }
    }
    /// <summary>
    /// 托盘信息查询条件对话框的的参数数据。
    /// </summary>
    public class PalletQueryConditionModel
    {
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string RoomKey { get; set; }
        /// <summary>
        /// 车间名称。
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 托盘号。用于模糊查询或者用于传入托盘范围的开始托盘号
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>
        /// 第二个托盘号，用于传入托盘范围的最后一个托盘号。
        /// </summary>
        public string PalletNo1 { get; set; }
        /// <summary>
        /// 工单号。
        /// </summary>
        public string WorkOrderNumber { get; set; }
        /// <summary>
        /// 产品ID号。
        /// </summary>
        public string ProId { get; set; }
        /// <summary>
        /// 产品料号。
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 状态标识。组别（0：包装中；1：包装；2：入库检；3:已入库；4：已出货）
        /// </summary>
        public string StateFlag { get; set; }
        /// <summary>
        /// 包装日期-起。
        /// </summary>
        public string StartCreateDate { get; set; }
        /// <summary>
        /// 包装日期-止。
        /// </summary>
        public string EndCreateDate { get; set; }
        /// <summary>
        /// 入库检日期-起。
        /// </summary>
        public string StartCheckDate { get; set; }
        /// <summary>
        /// 入库检日期-止。
        /// </summary>
        public string EndCheckDate { get; set; }
        /// <summary>
        /// 入库日期-起。
        /// </summary>
        public string StartTowarehouseDate { get; set; }
        /// <summary>
        /// 入库日期-止。
        /// </summary>
        public string EndTowarehouseDate { get; set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PalletQueryConditionModel()
        {
            this.RoomKey = string.Empty;
            this.RoomName = string.Empty;
            this.PalletNo = string.Empty;
            this.PalletNo1 = string.Empty;
            this.WorkOrderNumber = string.Empty;
            this.ProId = string.Empty;
            this.StateFlag = string.Empty;
            this.StartCreateDate =string.Empty;
            this.EndCreateDate =string.Empty;
            this.StartCheckDate = string.Empty;
            this.EndCheckDate = string.Empty;
            this.StartTowarehouseDate = string.Empty;
            this.EndTowarehouseDate = string.Empty;
        }
    }
}
