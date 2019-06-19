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
    public partial class LotQueryConditionDialog : BaseDialog
    {
        /// <summary>
        /// 批次查询的参数数据。
        /// </summary>
        public LotQueryConditionModel Model
        {
            get;
            private set;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotQueryConditionDialog()
        {
            InitializeComponent();
            this.Model = new LotQueryConditionModel();
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
            this.Model.DeletedFlag = Convert.ToString(this.lueDeletedFlag.EditValue);
            this.Model.HoldFlag = Convert.ToString(this.lueHoldFlag.EditValue);
            this.Model.LotNumber = this.teLotNumber.Text.ToUpper().Trim();
            this.Model.LotType = Convert.ToString(this.lueLotType.EditValue);
            this.Model.OperationName = Convert.ToString(this.lueOperation.EditValue);
            this.Model.ProId = this.teProId.Text.ToUpper().Trim();
            this.Model.ReworkFlag = Convert.ToString(this.lueReworkFlag.EditValue);
            this.Model.RoomKey = factoryRoomKey;
            this.Model.RoomName = Convert.ToString(this.lueFactoryRoom.Text);
            this.Model.ShippedFlag = Convert.ToString(this.lueShippedFlag.EditValue);
            this.Model.StateFlag = Convert.ToString(this.lueState.EditValue);
            this.Model.PalletNo = this.tePalletNo.Text.Trim();
            this.Model.WorkOrderNumber = this.teWorkOrderNumber.Text.ToUpper().Trim();
            this.Model.StartCreateDate = this.deStartCreateDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.Model.EndCreateDate = this.deEndCreateDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.Model.Creator = this.teCreator.Text.Trim();
            this.Model.LotNumber1 = this.teLotNumberEnd.Text.Trim();
            this.Model.PartNumber = this.tePartNumber.Text.Trim();
            this.Model.OrgOrderNumber = this.teOrgOrderNumber.Text.Trim();
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
            BindOpeartion();
            BindHoldFlag();
            BindReworkFlag();
            BindDeletedFlag();
            BindShippedFlag();
            BindLotType();
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
            dtState.Columns.Add("NAME",typeof(string));
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
        private void BindLotType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Lot_Type");
            DataTable dtLotType = BaseData.Get(columns, category);
            DataRow dr=dtLotType.NewRow();
            dr["CODE"]=string.Empty;
            dr["NAME"] = "全部";
            dtLotType.Rows.InsertAt(dr,0);
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
    }
    /// <summary>
    /// 批次查询条件对话框的的参数数据。
    /// </summary>
    public class LotQueryConditionModel
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
        /// 批次号。用于模糊查询或者用于传入批次范围的开始批次号
        /// </summary>
        public string LotNumber { get; set; }
        /// <summary>
        /// 第二个批次号，用于传入批次范围的最后一个批次号。
        /// </summary>
        public string LotNumber1 { get; set; }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 工单号。
        /// </summary>
        public string WorkOrderNumber { get; set; }
        /// <summary>
        /// 原工单号。
        /// </summary>
        public string OrgOrderNumber { get; set; }
        /// <summary>
        /// 产品ID号。
        /// </summary>
        public string ProId { get; set; }
        /// <summary>
        /// 产品料号。
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 托盘号。
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>
        /// 暂停标记。0：正常 1：暂停。
        /// </summary>
        public string HoldFlag { get; set; }
        /// <summary>
        /// 重工标记。0：正常 1：重工。
        /// </summary>
        public string ReworkFlag { get; set; }
        /// <summary>
        /// 删除标记。0：未删除 1：已结束 2：已删除
        /// </summary>
        public string DeletedFlag { get; set; }
        /// <summary>
        /// 出货标记。0：未出货 1：已出货
        /// </summary>
        public string ShippedFlag { get; set; }
        /// <summary>
        /// 状态标识。0：等待进站,4：等待数据采集,5：出站数据采集中,9：等待出站,10：已完成 11：已入库
        /// </summary>
        public string StateFlag { get; set; }
        /// <summary>
        /// 暂停标记。N:生产批次 L：组件补片批次。
        /// </summary>
        public string LotType { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建日期-起。
        /// </summary>
        public string StartCreateDate { get; set; }
        /// <summary>
        /// 创建日期-止。
        /// </summary>
        public string EndCreateDate { get; set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotQueryConditionModel()
        {
            this.DeletedFlag = string.Empty;
            this.HoldFlag = string.Empty;
            this.LotNumber = string.Empty;
            this.LotType = string.Empty;
            this.OperationName = string.Empty;
            this.ProId = string.Empty;
            this.ReworkFlag = string.Empty;
            this.RoomKey = string.Empty;
            this.RoomName = string.Empty;
            this.ShippedFlag = string.Empty;
            this.StateFlag = string.Empty;
            this.WorkOrderNumber = string.Empty;
            this.PalletNo = string.Empty;
            DateTime dtNow=FanHai.Hemera.Utils.Common.Utils.GetCurrentDateTime();
            this.StartCreateDate =dtNow.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            this.EndCreateDate = dtNow.ToString("yyyy-MM-dd HH:mm:ss");
            this.Creator = string.Empty;
            this.LotNumber1 = string.Empty;
        }
       
    }
}
