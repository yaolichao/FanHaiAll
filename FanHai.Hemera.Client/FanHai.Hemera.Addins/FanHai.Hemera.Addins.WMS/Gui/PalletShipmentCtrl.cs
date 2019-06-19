
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
using FanHai.Hemera.Utils.Dialogs;
using System.Linq;
using FanHai.Hemera.Share.Common;
using System.Threading;
namespace FanHai.Hemera.Addins.WMS
{
    /// <summary>
    /// 表示托盘出货作业的窗体类。
    /// </summary>
    public partial class PalletShipmentCtrl : BaseUserCtrl
    {
        IViewContent _view = null;                                      //当前视图。
        /// <summary>
        /// 出货对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public PalletShipmentCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletShipmentCtrl_Load(object sender, EventArgs e)
        {
            BindShipmentType();
            ResetControlValue();
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            this.teShipmentNo.Text = string.Empty;
            this.teContainerNo.Text = string.Empty;
            this.lueShipmentType.EditValue = string.Empty;
            this.teContainerQty.Text = string.Empty;
            this.teShipmentQty.Text = string.Empty;
            this.teCINumber.Text = string.Empty;
            this.teCIQty.Text = string.Empty;
            this.deShipment.DateTime = DateTime.Now;
            //this.deShipment.Properties.MaxValue = DateTime.Now;
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
            }
        }
        /// <summary>
        /// 绑定出货类型。
        /// </summary>
        private void BindShipmentType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.Basic_ShipmentType);
            this.lueShipmentType.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShipmentType.Properties.DisplayMember="NAME";
            this.lueShipmentType.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 出货单号值改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teShipmentNo_EditValueChanged(object sender, EventArgs e)
        {
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            //出货单号为空。
            if (string.IsNullOrEmpty(shipmentNo))
            {
                this.teShipmentQty.Text = string.Empty;
                this.teContainerQty.Text = string.Empty;
                return;
            }

            //获取数据库中出货单号的数量
            double shipmentQty = this._entity.GetShipmentQuantity(shipmentNo);
            this.teShipmentQty.Text = shipmentQty.ToString("0");
            //货柜号为空。
            if (string.IsNullOrEmpty(containerNo))
            {
                this.teContainerQty.Text = string.Empty;
                return;
            }
            //获取数据库中已入货柜的组件数量
            double containerQty = this._entity.GetContainerQuantity(shipmentNo, containerNo);
            this.teContainerQty.Text = containerQty.ToString("0");
        }
        /// <summary>
        /// 货柜号值改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teContainerNo_EditValueChanged(object sender, EventArgs e)
        {
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            //货柜号为空或出货单号为空。
            if (string.IsNullOrEmpty(containerNo) || string.IsNullOrEmpty(shipmentNo))
            {
                this.teContainerQty.Text = string.Empty;
                return;
            }
            //获取数据库中已入货柜的组件数量
            double qty = this._entity.GetContainerQuantity(shipmentNo, containerNo);
            this.teContainerQty.Text = qty.ToString("0");
        }
        /// <summary>
        /// CI号改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teCINumber_EditValueChanged(object sender, EventArgs e)
        {
            string ciNumber = this.teCINumber.Text.Trim();
            //CI号为空。
            if (string.IsNullOrEmpty(ciNumber))
            {
                this.teCIQty.Text = string.Empty;
                return;
            }
            //获取数据库中CI已出货的组件数量
            double qty = this._entity.GetCIQuantity(ciNumber);
            this.teCIQty.Text = qty.ToString("0");
        }
        /// <summary>
        /// 新增出货信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string palletNo = this.bePalletNo.Text.Trim();
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            string ciNo = this.teCINumber.Text.Trim();
            string shipmentType = Convert.ToString(this.lueShipmentType.EditValue);
            //出货单号不能为空。
            if (string.IsNullOrEmpty(shipmentNo))
            {
                MessageService.ShowMessage("请输入出货单号。", "提示");
                this.teShipmentNo.Select();
                return;
            }
            //货柜号不能为空。
            //if (string.IsNullOrEmpty(containerNo))
            //{
            //    MessageService.ShowMessage("请输入货柜号。", "提示");
            //    this.teContainerNo.Select();
            //    return;
            //}

            //CI号不能为空。
            if (string.IsNullOrEmpty(ciNo))
            {
                MessageService.ShowMessage("请输入CI号。", "提示");
                this.teCINumber.Select();
                return;
            }
            //运输类型不能为空。
            if (string.IsNullOrEmpty(shipmentType))
            {
                MessageService.ShowMessage("请选择运输类型。", "提示");
                this.lueShipmentType.Select();
                return;
            }
            //托盘号不能为空。
            if (string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage("请输入托盘号。", "提示");
                this.bePalletNo.Select();
                return;
            }
            AddShipment(palletNo);

            this.bePalletNo.Select();
            this.bePalletNo.SelectAll();
        }
        /// <summary>
        /// 增加出货信息。
        /// </summary>
        private string AddShipment(string palletNo)
        {
            if (string.IsNullOrEmpty(palletNo)) return string.Empty;
            DataTable dtList = this.gcList.DataSource as DataTable;
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            string ciNo = this.teCINumber.Text.Trim();
            string shipmentType = Convert.ToString(this.lueShipmentType.EditValue);
            if (dtList != null)
            {
                //出货记录不能超过1000
                if (dtList.Rows.Count >= 1000)
                {
                    return "出货信息记录不能超过1000托。";
                }
                //托盘号不能在出货信息列表中存在。
                DataRow drShipment = dtList.AsEnumerable()
                              .Where(dr => Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]) == palletNo)
                              .SingleOrDefault();
                if (drShipment != null)
                {
                    this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(drShipment);
                    return string.Format("托盘信息({0})在出货列表中已经存在。",palletNo);
                }
            }
            else
            {
                dtList = CommonUtils.CreateDataTable(new WMS_SHIPMENT_FIELDS());
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER);
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO);
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL);
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CUST_CHECK,typeof(int));  //add by chao.pang  20130712
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE);//晶硅功率范围 add by chao.pang 20140514
                this.gcList.DataSource = dtList;
                this.gcList.MainView = this.gvList;
            }
            //获取托盘信息记录
            DataSet dsPalletReturn = this._entity.GetPalletInfo(palletNo);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                return this._entity.ErrorMsg;
            }
            if (null == dsPalletReturn
                || dsPalletReturn.Tables.Count < 1
                || dsPalletReturn.Tables[0].Rows.Count < 1
                )
            {
                return string.Format("托盘号({0})不存在。",palletNo);
            }
            if (dsPalletReturn.Tables[0].Rows.Count > 1)
            {
                return string.Format("托盘（{0}）多于一条记录，请检查。",palletNo);
            }

            DataRow drPalletReturn = dsPalletReturn.Tables[0].Rows[0];
            //托盘记录必须是已入库的
            int checkFlag = Convert.ToInt32(drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
            if (checkFlag != 3)
            {
                if (checkFlag == 4)
                {
                    return string.Format("托盘({0})已经出货，请确认。", palletNo);
                }
                else
                {
                    return string.Format("托盘({0})未做入库，不能出货。", palletNo);
                }
            }
            DataRow drList = dtList.NewRow();
            drList[WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO] = containerNo;
            drList[WMS_SHIPMENT_FIELDS.FIELDS_CREATE_TIME] = DBNull.Value;
            drList[WMS_SHIPMENT_FIELDS.FIELDS_CREATE_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            drList[WMS_SHIPMENT_FIELDS.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drList[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIME] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME];
            drList[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            drList[WMS_SHIPMENT_FIELDS.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drList[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO] = palletNo;
            drList[WMS_SHIPMENT_FIELDS.FIELDS_QUANTITY] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY];
            drList[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE] = this.deShipment.Text;
            drList[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO] = shipmentNo;
            drList[WMS_SHIPMENT_FIELDS.FIELDS_CI_NO] = ciNo;
            drList[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_OPERATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drList[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE] = shipmentType;
            drList[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL];
            drList[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO];
            drList[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER];
            drList[WIP_CONSIGNMENT_FIELDS.FIELDS_CUST_CHECK] = 0;
            drList[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE];//add by chao.pang 20140514
            dtList.Rows.Add(drList);
            return string.Empty;
        }
        /// <summary>
        /// 移除出货信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int rowHandle=this.gvList.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MessageService.ShowMessage("请选择要移除的出货信息。", "提示");
                return;
            }
           int rowIndex=this.gvList.GetDataSourceRowIndex(rowHandle);
           DataTable dtList = this.gcList.DataSource as DataTable;
           dtList.Rows.RemoveAt(rowIndex);
        }
        /// <summary>
        /// 选择托盘信息事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bePalletNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PalletQueryHelpModel model = new PalletQueryHelpModel();
            model.RoomKey = string.Empty;
            model.PalletState = PalletState.Warehouse;
            PalletQueryHelpDialog dlg = new PalletQueryHelpDialog(model);
            dlg.OnValueSelected += new PalletQueryValueSelectedEventHandler(PalletQueryHelpDialog_OnValueSelected);
            Point i = bePalletNo.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = bePalletNo.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + bePalletNo.Height);

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
                    dlg.Location = new Point(i.X + bePalletNo.Width - dlg.Width, i.Y + bePalletNo.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + bePalletNo.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 选中托盘值后的事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PalletQueryHelpDialog_OnValueSelected(object sender, PalletQueryValueSelectedEventArgs args)
        {
            this.bePalletNo.Text = args.PalletNo;
        }
        /// <summary>
        /// 托盘号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bePalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnAdd_Click(sender, e);
                e.Handled = true;
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
                tsbSave_Click(null, null);
                args.Handled = true;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// 保存按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList == null || dtList.Rows.Count <= 0)
            {
                MessageService.ShowMessage("请先添加出货信息。", "提示");
                this.bePalletNo.Select();
                this.bePalletNo.SelectAll();
                return;
            }
            dtList.TableName = WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME;
            DataSet dsParams = new DataSet();
            DataTable dtParams = dtList.Copy();
            dtParams.Columns.Remove(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER);
            dtParams.Columns.Remove(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO);
            dtParams.Columns.Remove(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL);
            dsParams.Merge(dtParams);

            this._entity.PalletShipment(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                ResetControlValue();
            }
        }
        /// <summary>
        /// 绘制自定义单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gclShipmentType)
            {
                DataTable dtShipmentType = this.lueShipmentType.Properties.DataSource as DataTable;
                DataRow []drs=dtShipmentType.Select(string.Format("CODE='{0}'",e.CellValue));
                if(drs.Length>0){
                    e.DisplayText=Convert.ToString(drs[0]["NAME"]);
                }
            }
        }
        /// <summary>
        /// 货柜号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teContainerNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.teContainerNo_EditValueChanged(sender, e);
                this.teCINumber.Select();
            }
        }
        /// <summary>
        /// 出货单号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teShipmentNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.teShipmentNo_EditValueChanged(sender, e);
                this.teContainerNo.Select();
            }
        }
        /// <summary>
        /// CI单号回车时间。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teCINumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.teCINumber_EditValueChanged(sender, e);
                this.lueShipmentType.Select();
            }
        }
        /// <summary>
        /// 选择上传文件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            string ciNo = this.teCINumber.Text.Trim();
            string shipmentType = Convert.ToString(this.lueShipmentType.EditValue);
            //出货单号不能为空。
            if (string.IsNullOrEmpty(shipmentNo))
            {
                MessageService.ShowMessage("请输入出货单号。", "提示");
                this.teShipmentNo.Select();
                return;
            }
            //货柜号不能为空。
            //if (string.IsNullOrEmpty(containerNo))
            //{
            //    MessageService.ShowMessage("请输入货柜号。", "提示");
            //    this.teContainerNo.Select();
            //    return;
            //}

            //CI号不能为空。
            if (string.IsNullOrEmpty(ciNo))
            {
                MessageService.ShowMessage("请输入CI号。", "提示");
                this.teCINumber.Select();
                return;
            }
            //运输类型不能为空。
            if (string.IsNullOrEmpty(shipmentType))
            {
                MessageService.ShowMessage("请选择运输类型。", "提示");
                this.lueShipmentType.Select();
                return;
            }

            this.ofdlgUpload.Filter = "文本文件(*.txt)|*.txt";
            this.ofdlgUpload.FilterIndex = 1;
            this.ofdlgUpload.RestoreDirectory = true;
            if (this.ofdlgUpload.ShowDialog() == DialogResult.OK)
            {
                string fileName = this.ofdlgUpload.FileName;
                if (string.IsNullOrEmpty(fileName)) return;
                this.lblMsg.Visible = true;
                this.lblMsg.Text = string.Format("正在执行上传操作，请勿关闭界面，等待...");
                this.tableLayoutPanelMain.Enabled = false;
                //执行上传作业。
                ParameterizedThreadStart start = new ParameterizedThreadStart(UploadPalletNo);
                Thread t = new Thread(start);
                t.Start(fileName);
            }
        }
        /// <summary>
        /// 上传托盘号。
        /// </summary>
        /// <param name="obj"></param>
        private void UploadPalletNo(object obj)
        {

            try
            {
                string fileName = obj as string;
                if (string.IsNullOrEmpty(fileName)) return;
                StringBuilder sbPalletNo = new StringBuilder();
                string line=null;
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName,Encoding.Default))
                {
                    while ((line = reader.ReadLine()) != null) 
                    {
                        sbPalletNo.AppendLine(line);
                    }
                }
                string palletNo = sbPalletNo.ToString();
                string[] palletNos = palletNo.Split(new char[] { '\n', ',', '$' });
                StringBuilder sbMsg = new StringBuilder();
                //循环获取托盘信息。
                for (int i = 0; i < palletNos.Length; i++)
                {
                    string p = palletNos[i].Trim();
                    if (string.IsNullOrEmpty(p)) continue;
                    //显示上传进度
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.lblMsg.Visible = true;
                        this.lblMsg.Text = string.Format("正在获取托盘[{0}]信息，请勿关闭界面，等待...({1}/{2})", p, i + 1, palletNos.Length);
                        string msg = AddShipment(p);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            sbMsg.AppendLine(msg);
                        }
                    }));
                }
                //部分托盘上传失败提示。
                if (sbMsg.Length > 0)
                {
                    MessageService.ShowError(sbMsg.ToString());
                }
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.lblMsg.Visible = false;
                    this.lblMsg.Text = string.Empty;
                    this.tableLayoutPanelMain.Enabled = true;
                }));
            }
        }

    }
}
