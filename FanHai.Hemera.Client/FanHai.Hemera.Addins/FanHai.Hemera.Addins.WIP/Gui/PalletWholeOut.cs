using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using System.Net;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class PalletWholeOut : BaseUserCtrl
    {
        private int typeCode = 0;
        private string roomkey = string.Empty;
        private LotAfterIvTestEntity lotEntity = new LotAfterIvTestEntity();
        public DataTable dtCommon = new DataTable();
        DataTable dtPallet = new DataTable();
        string roomKey = string.Empty, shiftkey = string.Empty, shiftname = string.Empty;
        string _title = string.Empty;
        IViewContent _view = null;
        public PalletWholeOut()
        {
            InitializeComponent();
        }
        public PalletWholeOut(IViewContent view)
        {
            this._view = view;
            InitializeComponent();
        }

        /// <summary>
        /// 整托出托作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnWholePalletOut_Click(object sender, EventArgs e)
        {
            WholePalletOut();
        }
        private void WholePalletOut()
        {
            if (string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
            {
                MessageService.ShowMessage("【托号】不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(txtLotNum_qty.Text))
            {
                MessageService.ShowMessage("【托号数量】不能为空!");
                return;
            }
            bool bl_bak = false;
            if (MessageService.AskQuestion(string.Format("确定要出托【{0}】么?", txtPalletNo.Text.Trim()), "提示"))
            {
                bl_bak = true;
            }
            if (!bl_bak) return;

            DataSet dsSave = new DataSet();
            if (dtPallet.Rows.Count < 1)
            {
                MessageService.ShowMessage("托号数据出现异常,请取消重试!", "提示");
                return;
            }
            dsSave.Merge(dtPallet, true, MissingSchemaAction.Add);
            dsSave.ExtendedProperties.Add("savetype", "out");
            dsSave.ExtendedProperties.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, "包装");
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, Dns.GetHostName());
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftkey);
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftname);

            DataSet dsReturn = lotEntity.SavePalletLotData(dsSave);

            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                return;
            }
            else
            {
                MessageService.ShowMessage(string.Format("托号【{0}】出托成功!", txtPalletNo.Text.Trim()), "提示");
                this.gcOutPallet.DataSource = null;
                this.txtLotNum_qty.Text = string.Empty;
                this.txtPalletNo.SelectAll();
                this.txtPalletNo.Focus();
            }
        }

        /// <summary>
        /// 整托出托作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.txtPalletNo.BackColor = Color.White;
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(Convert.ToString(lueFactoryRoom.EditValue)))
                {
                    MessageService.ShowError("请选择【工厂车间】!");
                    return;
                }
                roomKey = Convert.ToString(lueFactoryRoom.EditValue);

                CheckPalletNo();
            }
        }

        /// <summary>
        /// 检验整托出托，托号的合法性
        /// </summary>
        private void CheckPalletNo()
        {
            if (string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
            {
                this.txtLotNum_qty.Text = string.Empty;
                this.txtPalletNo.BackColor = Color.YellowGreen;
                return;
            }
            string palletno = txtPalletNo.Text.Trim();
            Hashtable hstable = new Hashtable();
            hstable["flag"] = "pallet";
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = txtPalletNo.Text.Trim();
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = roomKey;
            DataSet dsBak = lotEntity.GetPalletOrLotData(hstable);
            dtPallet = dsBak.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];

            if (dtPallet.Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("【托号{0}】不存在!", palletno));
                this.txtPalletNo.Focus();
                this.txtPalletNo.SelectAll();
                this.txtLotNum_qty.Text = string.Empty;
                dtPallet = new DataTable();
                return;
            }
            DataRow drPallet = dtPallet.Rows[0];

            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("0"))
            {
                MessageService.ShowError(string.Format("【托号{0}】还未过站，不能做整托出托!", palletno));
                this.txtPalletNo.Focus();
                this.txtPalletNo.SelectAll();
                this.txtLotNum_qty.Text = string.Empty;
                dtPallet = new DataTable();
                return;
            }
            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("2"))
            {
                MessageService.ShowError(string.Format("【托号{0}】已经【入库检验】完成，请入库检验人员返到包装后才能出托!", palletno));
                this.txtPalletNo.Focus();
                this.txtPalletNo.SelectAll();
                this.txtLotNum_qty.Text = string.Empty;
                dtPallet = new DataTable();
                return;
            }
            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("3"))
            {
                MessageService.ShowError(string.Format("【托号{0}】已入库!", palletno));
                this.txtPalletNo.Focus();
                this.txtPalletNo.SelectAll();
                this.txtLotNum_qty.Text = string.Empty;
                dtPallet = new DataTable();
                return;
            }
            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("4"))
            {
                MessageService.ShowError(string.Format("【托号{0}】已出货!", palletno));
                this.txtPalletNo.Focus();
                this.txtPalletNo.SelectAll();
                this.txtLotNum_qty.Text = string.Empty;
                dtPallet = new DataTable();
                return;
            }

            txtLotNum_qty.Text = drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY].ToString();
            DataTable dtPalletLot = dsBak.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
            this.gcOutPallet.MainView = this.gvOutPallet;
            this.gcOutPallet.DataSource = dtPalletLot;
            this.sbtnWholePalletOut.Select();
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
                    this.lueFactoryRoom.ItemIndex = 0;
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }

        //绑定批次等级
        private void BindProLevel()
        {
            //绑定自定义配置数据
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtCommon = BaseData.Get(l_s, category);

            DataTable dtLevel = dtCommon.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCTGRADE));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";

            repositoryItemLookUpEdit_PRO_LEVEL.DisplayMember = "Column_Name";
            repositoryItemLookUpEdit_PRO_LEVEL.ValueMember = "Column_code";
            repositoryItemLookUpEdit_PRO_LEVEL.DataSource = dview.Table;

        }

        private void PalletWholeOut_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindProLevel();
            txtPalletNo.Select();

            //获取班别。
            Shift shift = new Shift();
            shiftname = shift.GetCurrShiftName();
        }

        private void sbtnCancel_Click(object sender, EventArgs e)
        {
            this.gcOutPallet.DataSource = null;
            this.txtLotNum_qty.Text = string.Empty;
            this.txtPalletNo.Text = string.Empty;
        }

        private void sbtnClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
        }

        private void PalletWholeOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if(e.KeyChar==13)
        }



    }
}
