
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
using DevExpress.XtraGrid.Views.Grid;
using System.Data.OleDb;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WMS
{
    /// <summary>
    /// 表示托盘出货作业的窗体类。
    /// </summary>
    public partial class PalletShipmentsCtrl : BaseUserCtrl
    {
        IViewContent _view = null;                                      //当前视图。
        /// <summary>
        /// 出货对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public PalletShipmentsCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;

            InitUi();
            GridViewHelper.SetGridView(gvList);
        }

        private void InitUi()
        {
            tsbAdd.Text = StringParser.Parse("${res:Global.New}");
            tsbUpdate.Text = StringParser.Parse("${res:Global.Update}");
            tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            tsbPass.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.btn.0001}");
            tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            tsbSelect.Text = StringParser.Parse("${res:Global.Query}");

            btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.btn.0002}");
            btnRemove.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.btn.0003}");
            btnVolumeUpload.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.btn.0004}");
            btnUpload.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.btn.0005}");

            lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0001}");
            lciShipmentNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0002}");
            lciContainerNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0003}");
            lciCINumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0004}");
            lciShipmentQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0005}");
            lciContainerQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0006}");
            lciCIQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0007}");
            lciShipmentType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0008}");
            lciShipmentDate.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0009}");
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0010}");
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0011}");
            lciPalletNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0012}");
            lblMsg.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.lbl.0013}");

            gcRowNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0001}");
            gclShipmentNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0002}");
            gclContainerNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0003}");
            gclPalletNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0004}");
            gclShipmentType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0005}");
            gclQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0006}");
            gclWorkorderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0007}");
            gclSAPNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0008}");
            gclPowerLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0009}");
            gcPowerRange.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0010}");
            gclShipmentDate.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0011}");
            gclShipmentOperator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0012}");
            gclCustCheck.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0013}");
            gcGoto.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.Column.0014}");
            this.lblMenu.Text = "库房管理>出货管理>出货作业";
        }

        public string _status = "Add";                 //状态表明是新增的还是修改还是查询删除

        private void Status(int Flag)
        {
            if (Flag == 0)  //新增
            {
                BindShipmentType();
                ResetControlValue();
                BindGoto();

                //保存，修改，删除
                tsbSave.Enabled = true;
                tsbPass.Enabled = false;  //过账
                tsbDelete.Enabled = false;
                tsbUpdate.Enabled = false;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
                btnUpload.Enabled = true;
                tsbAdd.Enabled = false;
                _status = "Add";

                //输入数据可用
                teShipmentNo.Enabled = true;
                teContainerNo.Enabled = true;
                teCINumber.Enabled = true;
                lueShipmentType.Enabled = true;
                lupGoto.Enabled = true;
                deShipment.Enabled = true;

                gvList.OptionsBehavior.ReadOnly = false;
            }
            if (Flag == 1)     //修改
            {
                //保存，修改，删除
                tsbSave.Enabled = true;
                tsbPass.Enabled = false;  //过账
                tsbDelete.Enabled = true;
                tsbUpdate.Enabled = false;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
                btnUpload.Enabled = true;
                tsbAdd.Enabled = true;
                _status = "Edit";

                //输入数据可用
                teShipmentNo.Enabled = false;
                teContainerNo.Enabled = true;
                teCINumber.Enabled = true;
                lueShipmentType.Enabled = true;
                deShipment.Enabled = true;
                lupGoto.Enabled = true;

                gvList.OptionsBehavior.ReadOnly = false;
            }
            if (Flag == 2)    //查询
            {
                teShipmentNo.Text = "";
                teContainerNo.Text = "";
                teCINumber.Text = "";
                teShipmentQty.Text = "";
                teContainerQty.Text = "";
                teCIQty.Text = "";
                BindShipmentType();
                gcList.DataSource = null;
                //保存，修改，删除
                tsbSave.Enabled = false;
                tsbPass.Enabled = false;  //过账
                tsbDelete.Enabled = false;
                tsbUpdate.Enabled = true;
                tsbAdd.Enabled = true;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                btnUpload.Enabled = false;
                _status = "Select";
                //输入数据可用
                teShipmentNo.Enabled = false;
                teContainerNo.Enabled = false;
                teCINumber.Enabled = false;
                lueShipmentType.Enabled = false;
                deShipment.Enabled = false;
                lupGoto.Enabled = false;

                gvList.OptionsBehavior.ReadOnly = true;

            }
            if (Flag == 3)    //保存
            {
                //保存，修改，删除
                tsbSave.Enabled = false;
                tsbPass.Enabled = true;  //过账
                tsbDelete.Enabled = false;
                tsbUpdate.Enabled = true;
                tsbAdd.Enabled = true;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                btnUpload.Enabled = false;
                _status = "Save";
                //输入数据可用
                teShipmentNo.Enabled = false;
                teContainerNo.Enabled = false;
                teCINumber.Enabled = false;
                lueShipmentType.Enabled = false;
                deShipment.Enabled = false;
                lupGoto.Enabled = false;

                gvList.OptionsBehavior.ReadOnly = true;
            }
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletShipmentCtrl_Load(object sender, EventArgs e)
        {
            Status(0);
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
            this.txtStatus.Text = string.Empty;
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
            this.lueShipmentType.Properties.DisplayMember = "NAME";
            this.lueShipmentType.Properties.ValueMember = "CODE";
        }
        private void BindGoto()
        {
            string[] l_s = new string[] { "CODE", "NAME" };
            string category = "WMS_ShipMents";
            DataTable dtCommon = BaseData.Get(l_s, category);
            lupGoto.Properties.DisplayMember = "NAME";
            lupGoto.Properties.ValueMember = "CODE";
            lupGoto.Properties.DataSource = dtCommon;
            lupGoto.ItemIndex = 0;

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

            string _shipgoto = this._entity.GetShipMentBasicGOTO(ciNumber);
            if (string.IsNullOrEmpty(_shipgoto))
            {
                _shipgoto = "2";
            }
            this.lupGoto.EditValue = _shipgoto;
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
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0017}"), StringParser.Parse("${res:Global.SystemInfo}"));//请输入出货单号。
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
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0018}"), StringParser.Parse("${res:Global.SystemInfo}"));//请输入CI号。
                this.teCINumber.Select();
                return;
            }
            //运输类型不能为空。
            if (string.IsNullOrEmpty(shipmentType))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0019}"), StringParser.Parse("${res:Global.SystemInfo}"));//请选择运输类型。
                this.lueShipmentType.Select();
                return;
            }
            //托盘号不能为空。
            if (string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0020}"), StringParser.Parse("${res:Global.SystemInfo}"));//请输入托盘号。
                this.bePalletNo.Select();
                return;
            }

            if (this.lupGoto.EditValue.ToString() == "2")
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0021}"), StringParser.Parse("${res:Global.SystemInfo}"));//APP不能为空。
                this.bePalletNo.Select();
                return;
            }
            if (!string.IsNullOrEmpty(AddShipment(palletNo)))
                MessageBox.Show(AddShipment(palletNo), StringParser.Parse("${res:Global.SystemInfo}"));
            BindRowNumber();
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
                    return string.Format("托盘信息({0})在出货列表中已经存在。", palletNo);
                }
            }
            else
            {
                dtList = CommonUtils.CreateDataTable(new WMS_SHIPMENT_FIELDS());
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER);
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO);
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL);
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CUST_CHECK, typeof(int));  //add by chao.pang  20130712
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE);//晶硅功率范围 add by chao.pang 20140514
                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROWNUMBER);
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
                return string.Format("托盘号({0})不存在。", palletNo);
            }
            if (dsPalletReturn.Tables[0].Rows.Count > 1)
            {
                return string.Format("托盘（{0}）多于一条记录，请检查。", palletNo);
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
            //判定出货清单中是否存在了该托盘记录
            DataSet dsNum = this._entity.GetShipMentNumByPallet(palletNo);
            if (dsNum != null && dsNum.Tables[0].Rows.Count != 0)
            {
                return string.Format("托盘({0})已经在出货单({1})中，已经保存但未出货，请确认。", palletNo, dsNum.Tables[0].Rows[0]["SHIPMENT_NO"].ToString());
            }
            DataTable dtList01 = this.gcList.DataSource as DataTable;
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
            drList[WMS_SHIPMENT_FIELDS.FIELDS_SHIP_GOTO] = this.lupGoto.EditValue;
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
            int rowHandle = this.gvList.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0022}"), StringParser.Parse("${res:Global.SystemInfo}"));//请选择要移除的出货信息。
                return;
            }
            int rowIndex = this.gvList.GetDataSourceRowIndex(rowHandle);
            DataTable dtList = this.gcList.DataSource as DataTable;
            dtList.Rows.RemoveAt(rowIndex);
            BindRowNumber();
        }

        private void BindRowNumber()
        {
            DataTable dtSource = gcList.DataSource as DataTable;
            if (dtSource != null)
            {
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    dtSource.Rows[i]["ROWNUMBER"] = i + 1;
                }
            }
            gcList.DataSource = dtSource;
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
            if (string.IsNullOrEmpty(teShipmentNo.Text.Trim()))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));//请检查是否输入出货单号！！
                return;
            }
            bool boolen = this._entity.SelectShipmentInf(teShipmentNo.Text.Trim());
            if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
            {
                this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
            }
            try
            {
                ((DataView)gvList.DataSource).Table.AcceptChanges();
            }
            catch (Exception ex)
            { }
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList == null || dtList.Rows.Count <= 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));//请先添加出货信息。
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
            dtParams.Columns.Remove(WIP_CONSIGNMENT_FIELDS.FIELDS_ROWNUMBER);
            dsParams.Merge(dtParams);
            if (MessageBox.Show(StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0007}")),//是否确认要保存该出货单信息？
                     StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (_status == "Add")
                {
                    if (boolen == true)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));//出货单号已经存在，不能对该出货单号进行新增,请重新输入出货单号。
                        return;
                    }
                    else
                    {
                        string shipNum = teShipmentNo.Text.Trim();
                        for (int i = 0; i < dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows.Count; i++)
                        {
                            dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[i]["SHIPMENT_NO"] = shipNum;
                        }
                        this._entity.PalletShipmentAdd(dsParams);
                        if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                        {
                            MessageService.ShowError(this._entity.ErrorMsg);
                        }
                        else
                        {
                            Status(3);
                            SelectInfBysNum(teShipmentNo.Text.Trim());
                            BindRowNumber();
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));//新增成功。
                        }
                    }
                }
                else if (_status == "Edit")
                {
                    string conNo = teContainerNo.Text.Trim();
                    string ci = teCINumber.Text.Trim();
                    string type = lueShipmentType.EditValue.ToString();
                    string time = deShipment.Text.Trim();
                    string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    for (int i = 0; i < dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows.Count; i++)
                    {
                        dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[i]["CONTAINER_NO"] = conNo;
                        dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[i]["CI_NO"] = ci;
                        dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[i]["SHIPMENT_TYPE"] = type;
                        dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[i]["SHIPMENT_DATE"] = time;
                        dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[i]["SHIPMENT_OPERATOR"] = name;
                    }
                    this._entity.PalletShipmentAUpdate(dsParams);
                    if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                    {
                        MessageService.ShowError(this._entity.ErrorMsg);
                    }
                    else
                    {
                        Status(3);
                        SelectInfBysNum(teShipmentNo.Text.Trim());
                        BindRowNumber();
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"));//修改成功。
                    }
                }
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
                DataRow[] drs = dtShipmentType.Select(string.Format("CODE='{0}'", e.CellValue));
                if (drs.Length > 0)
                {
                    e.DisplayText = Convert.ToString(drs[0]["NAME"]);
                }
            }
            if (e.Column == this.gcGoto)
            {
                DataTable dtShipmentGoto = this.lupGoto.Properties.DataSource as DataTable;
                DataRow[] drsGoto = dtShipmentGoto.Select(string.Format("CODE='{0}'", e.CellValue));
                if (drsGoto.Length > 0)
                {
                    e.DisplayText = Convert.ToString(drsGoto[0]["NAME"]);
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
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0017}"), StringParser.Parse("${res:Global.SystemInfo}"));//请输入出货单号。
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
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0018}"), StringParser.Parse("${res:Global.SystemInfo}"));//请输入CI号。
                this.teCINumber.Select();
                return;
            }
            //运输类型不能为空。
            if (string.IsNullOrEmpty(shipmentType))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0019}"), StringParser.Parse("${res:Global.SystemInfo}"));//请选择运输类型。
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
                this.lblMsg.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0034}");//正在执行上传操作，请勿关闭界面，等待...
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
                string line = null;
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName, Encoding.Default))
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

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            Status(0);
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == "已保存未出货")
            {
                Status(1);
            }
            else
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));//该出货单已经出货不能修改！
            }
        }
        /// <summary>
        /// 查询按钮根据出货单号返回出货单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSelect_Click(object sender, EventArgs e)
        {
            //PalletShipmentsSelectDailog form = new PalletShipmentsSelectDailog();
            string ShipmentNum = string.Empty;
            ShipmentNum = teShipmentNo.Text.Trim();
            if (!string.IsNullOrEmpty(ShipmentNum))
            {
                bool boolen = this._entity.SelectShipmentInf(ShipmentNum);
                if (boolen == true)
                {
                    Status(2);
                    SelectInfBysNum(ShipmentNum);
                    if (txtStatus.Text != "已出货")
                    {
                        tsbPass.Enabled = true;  //过账
                    }
                    BindRowNumber();
                }
                else
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0016}"), StringParser.Parse("${res:Global.SystemInfo}"));//出货单号不存在或出货单号存在重复，请确认！
                }
            }
            else
            {
                return;
            }

        }
        /// <summary>
        /// 根据返回的出货单查询出货单的明细信息
        /// </summary>
        /// <param name="sNum"></param>
        public void SelectInfBysNum(string sNum)
        {
            this.teShipmentNo.Text = sNum;
            DataSet dsInf = this._entity.GetShipmentInf(sNum);
            DataTable dt = dsInf.Tables[0];

            teContainerNo.Text = dt.Rows[0]["CONTAINER_NO"].ToString().Trim();
            teCINumber.Text = dt.Rows[0]["CI_NO"].ToString().Trim();
            lueShipmentType.EditValue = dt.Rows[0]["SHIPMENT_TYPE"].ToString().Trim();
            deShipment.EditValue = dt.Rows[0]["SHIPMENT_DATE"].ToString().Trim();
            switch (dt.Rows[0]["SHIPMENT_FLAG"].ToString().Trim())
            {
                case "0":
                    txtStatus.Text = "已保存未出货";
                    break;
                case "1":
                    txtStatus.Text = "已出货";
                    break;
                default:
                    break;
            }
            BindShipmentType();
            gcList.DataSource = dt;
        }
        /// <summary>
        /// 删除出货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.teShipmentNo.Text.Trim()))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0011}"), StringParser.Parse("${res:Global.SystemInfo}"));//出货单号不能为空，请确认！
                return;
            }
            if (gcList.DataSource == null)
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0012}"), StringParser.Parse("${res:Global.SystemInfo}"));//没有出货明细不能删除！
                return;
            }
            if (MessageBox.Show(StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0013}")),//是否确认要删除该出货单信息？
                      StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    if (txtStatus.Text != "已出货" && _status == "Edit")
                    {
                        string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        bool boolen = this._entity.DeleteShipMentInf(this.teShipmentNo.Text.Trim(), name);

                        if (boolen == true)
                        {
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0014}"), StringParser.Parse("${res:Global.SystemInfo}"));//删除成功，请确认！
                            Status(0);
                            return;
                        }
                        else
                        {
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0015}"), StringParser.Parse("${res:Global.SystemInfo}"));//删除失败,请确认！
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
            }

        }
        /// <summary>
        /// 过账
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPass_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0001}"),//是否确认要过账？
                      StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (_status == "Save" || txtStatus.Text != "已出货")
                {
                    DataTable dtList = this.gcList.DataSource as DataTable;
                    if (dtList == null || dtList.Rows.Count <= 0)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));//请先添加出货信息。
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
                    string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this._entity.PassShipMentInf(dsParams, name);
                    if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                    {
                        MessageService.ShowError(this._entity.ErrorMsg);
                    }
                    else
                    {
                        SelectInfBysNum(teShipmentNo.Text.Trim());
                        Status(0);
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));//已经过账！
                    }

                }
            }
        }
        /// <summary>
        /// 批量上传出货数据  add by yibin.fei 2018.03.07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVolumeUpload_Click(object sender, EventArgs e)
        {
            if (lueShipmentType.Text.Trim() == "")
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0023}"), StringParser.Parse("${res:Global.SystemInfo}"));//请选择运货方式！
                return;
            }

            string strUpPath = null;
            DataTable dtExcel = null;

            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                strUpPath = dialog.FileName;
                dtExcel = ExcelToDataTable(strUpPath);
                if (dtExcel.Rows.Count > 1000)
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0024}"), StringParser.Parse("${res:Global.SystemInfo}"));//批量的数据过大，数据量不得超过1000项！
                    return;
                }

                //增加出货信息。
                string ForteShipmentNo = null;
                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    string shipmentNo = dtExcel.Rows[i]["出货单号"].ToString();
                    string containerNo = dtExcel.Rows[i]["货柜号"].ToString();
                    string ciNo = dtExcel.Rows[i]["CI号"].ToString();
                    string palletNo = dtExcel.Rows[i]["托号"].ToString();
                    string shipmentType = Convert.ToString(this.lueShipmentType.EditValue);
                    bool Effectiveness = true;
                    if (!string.IsNullOrEmpty(shipmentNo) && !string.IsNullOrEmpty(containerNo) && !string.IsNullOrEmpty(ciNo) && !string.IsNullOrEmpty(palletNo))
                    {
                        if (shipmentNo != "")
                        {
                            ForteShipmentNo = shipmentNo;
                        }

                        try
                        {
                            DataTable dtList = this.gcList.DataSource as DataTable;


                            if (dtList != null)
                            {
                                //出货记录不能超过1000
                                if (dtList.Rows.Count >= 1000)
                                {
                                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0025}"), StringParser.Parse("${res:Global.SystemInfo}"));//出货信息记录不能超过1000托。
                                }
                                //托盘号不能在出货信息列表中存在。
                                DataRow drShipment = dtList.AsEnumerable()
                                              .Where(dr => Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]) == palletNo)
                                              .SingleOrDefault();
                                if (drShipment != null)
                                {
                                    this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(drShipment);
                                    MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0026}"), palletNo));//托盘信息({0})在出货列表中已经存在。

                                }
                            }
                            else
                            {
                                dtList = CommonUtils.CreateDataTable(new WMS_SHIPMENT_FIELDS());
                                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER);
                                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO);
                                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL);
                                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CUST_CHECK, typeof(int));  //add by chao.pang  20130712
                                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE);//晶硅功率范围 add by chao.pang 20140514
                                dtList.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROWNUMBER);
                                this.gcList.DataSource = dtList;
                                this.gcList.MainView = this.gvList;
                            }
                            //获取托盘信息记录
                            DataSet dsPalletReturn = this._entity.GetPalletInfo(palletNo);
                            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                            {
                                //return this._entity.ErrorMsg;
                            }
                            if (null == dsPalletReturn
                                || dsPalletReturn.Tables.Count < 1
                                || dsPalletReturn.Tables[0].Rows.Count < 1
                                )
                            {
                                MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0027}"), palletNo));//托盘号({0})不存在。
                                Effectiveness = false;
                            }
                            if (dsPalletReturn.Tables[0].Rows.Count > 1)
                            {
                                MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0028}"), palletNo));//托盘（{0}）多于一条记录，请检查。
                                Effectiveness = false;
                            }
                            DataRow drPalletReturn = dsPalletReturn.Tables[0].Rows[0];
                            //托盘记录必须是已入库的
                            int checkFlag = Convert.ToInt32(drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                            if (checkFlag != 3)
                            {
                                if (checkFlag == 4)
                                {
                                    MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0029}"), palletNo));//托盘({0})已经出货，请确认。
                                    Effectiveness = false;
                                }
                                else
                                {
                                    MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0030}"), palletNo));//托盘({0})未做入库，不能出货。
                                    Effectiveness = false;
                                }
                            }
                            //判定出货清单中是否存在了该托盘记录
                            DataSet dsNum = this._entity.GetShipMentNumByPallet(palletNo);
                            if (dsNum != null && dsNum.Tables[0].Rows.Count != 0)
                            {
                                MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0031}"), palletNo, dsNum.Tables[0].Rows[0]["SHIPMENT_NO"].ToString()));//托盘({0})已经在出货单({1})中，已经保存但未出货，请确认。
                                Effectiveness = false;
                            }
                            if (Effectiveness == true)
                            {
                                DataTable dtList01 = this.gcList.DataSource as DataTable;
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
                                drList[WMS_SHIPMENT_FIELDS.FIELDS_SHIP_GOTO] = this.lupGoto.EditValue;
                                drList[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL];
                                drList[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO];
                                drList[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER];
                                drList[WIP_CONSIGNMENT_FIELDS.FIELDS_CUST_CHECK] = 0;
                                drList[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE] = drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE];//add by chao.pang 20140514
                                dtList.Rows.Add(drList);
                            }



                        }
                        catch
                        {
                            MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0032}"), palletNo));//托盘【{0}】未加入列表，请确认

                        }
                    }


                    teShipmentNo.Text = ForteShipmentNo;



                }
            }
            else
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsCtrl.msg.0033}"));//此选项为批量上传，必须指向规定格式的Excel文件
            }
        }
        /// <summary>
        /// 将指定路径的Excel文件导入为DataTable  add by yibin.fei 2018.3.7    Excel版本需要2007及以上版本
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DataTable ExcelToDataTable(string fileName)
        {

            OleDbConnection con = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();

            //con.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; " + "Data Source=" + fileName + ";" + "Extended Properties=\"Excel 8.0;IMEX=1;HDR=YES;\"";

            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";


            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM [Sheet1$] ";
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private void gvList_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnUpload_Click_1(object sender, EventArgs e)
        {

        }
    }

}
