using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System.Net;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotDispatchForPltChkDialog : BaseDialog
    {
        private int typeCode = 0;
        private string roomkey = string.Empty;
        private LotAfterIvTestEntity lotEntity = new LotAfterIvTestEntity();
        LotOperationEntity _lotEntity = new LotOperationEntity();
        public DataTable dtCommon = new DataTable();
        DataTable dtPallet = new DataTable();
        LotDispatchDetailModel _model = null;
        string _title = string.Empty;
        string spalletno = string.Empty;
        public LotDispatchForPltChkDialog()
        {
            InitializeComponent();
        }

        public LotDispatchForPltChkDialog(int itypecode, LotDispatchDetailModel model, string subTitle)
        {
            this.typeCode = itypecode;
            _title = subTitle;
            _model = model;

            InitializeComponent();

            if (typeCode == 10 || typeCode == 80)
            {
                this.Size = new Size(722, 471);
            }
            else if (typeCode < 90)
            {
                this.Size = new Size(665, 315);
            }
        }

        private void LotDispatchForPltChkDialog_Load(object sender, EventArgs e)
        {
            this.Text = _title;
            this.roomkey = _model.RoomKey;
            //整托出托
            if (typeCode == 10)
            {
                BindFactoryRoom(this.lueFactoryRoom);
                tabControlMain.TabPages.Remove(tab_query);
                tabControlMain.TabPages.Remove(tab_PalletCheck);
                tabControlMain.TabPages.Remove(tab_Back2Package);
                tabControlMain.TabPages.Remove(tab_PalletNoExchange);
                txtPalletNo.Select();                
            }
            //包装作业查询
            if (typeCode == 20)
            {
                //tabControlMain.TabPages.Remove(tab_wholepalletout);
                //tabControlMain.TabPages.Remove(tab_PalletCheck);
                //tabControlMain.TabPages.Remove(tab_Back2Package);
                //tabControlMain.TabPages.Remove(tab_PalletNoExchange);
                //LoadPackageEquipments();
                //txtQ_PalletNo.Select();

                //LotDispatchForPallet lotdispatchforpallet = new LotDispatchForPallet();
                //txtQ_PalletNo.Text = lotdispatchforpallet.palletime01;
                //txtQ_PalletNo2.Text = lotdispatchforpallet.palletime02;
                //txtQ_LotNum.Text = lotdispatchforpallet.lotnom01;
                //txtQ_LotNum2.Text = lotdispatchforpallet.lotnum02;
                //betEquipment.EditValue = lotdispatchforpallet.equipmentkeys;
                //dtStart.Text = lotdispatchforpallet.palletime01;
                //dtEnd.Text = lotdispatchforpallet.palletime02;

            }
            //入库检查询
            if (typeCode == 0)
            {
                tabControlMain.TabPages.Remove(tab_wholepalletout);
                tabControlMain.TabPages.Remove(tab_query);
                tabControlMain.TabPages.Remove(tab_Back2Package);
                tabControlMain.TabPages.Remove(tab_PalletNoExchange);
                txtChk_palletNo.Select();
            }
            //返到包装工序
            if (typeCode == 90)
            {
                BindFactoryRoom(this.lueFactoryRoom2);
                tabControlMain.TabPages.Remove(tab_wholepalletout);
                tabControlMain.TabPages.Remove(tab_PalletCheck);
                tabControlMain.TabPages.Remove(tab_query);
                tabControlMain.TabPages.Remove(tab_PalletNoExchange);

                txtPalletNo2.Select();
            }
            //托号变更数据
            if (typeCode == 80)
            {
                BindFactoryRoom(this.lueFactoryRoom3);
                if (!string.IsNullOrEmpty(_model.RoomKey))
                    this.lueFactoryRoom3.EditValue = _model.RoomKey;
                this.lueFactoryRoom3.Properties.ReadOnly = true;

                tabControlMain.TabPages.Remove(tab_wholepalletout);
                tabControlMain.TabPages.Remove(tab_PalletCheck);
                tabControlMain.TabPages.Remove(tab_query);
                tabControlMain.TabPages.Remove(tab_Back2Package);
              
            }
        }
        private void LoadPackageEquipments()
        {
            #region 加载设备
            DataTable dtEquipment = new EquipmentEntity().GetEquipments(_model.RoomKey, "包装").Tables[0];

            if (dtEquipment != null && dtEquipment.Rows.Count > 0)
            {
                this.betEquipment.Properties.Items.Clear();
                foreach (DataRow dr in dtEquipment.Rows)
                {
                    string equipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
                    //string equipmentCode = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);
                    string equipmentKey = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);

                    this.betEquipment.Properties.Items.Add(equipmentKey.Trim(), equipmentName);
                }
            }
            else
            {
                this.betEquipment.Properties.Items.Clear();
            }
            #endregion
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

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom2()
        {
            this.lueFactoryRoom2.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueFactoryRoom2.Properties.DataSource = dt;
                this.lueFactoryRoom2.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom2.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom2.ItemIndex = 0;
                }
            }
            else
            {
                this.lueFactoryRoom2.Properties.DataSource = null;
                this.lueFactoryRoom2.EditValue = string.Empty;
            }
        }

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom(LookUpEdit lueFactory)
        {
            lueFactory.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                lueFactory.Properties.DataSource = dt;
                lueFactory.Properties.DisplayMember = "LOCATION_NAME";
                lueFactory.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    lueFactory.ItemIndex = 0;
                }
               
            }
            else
            {
                lueFactory.Properties.DataSource = null;
                lueFactory.EditValue = string.Empty;
            }
        }

        /// <summary>
        /// 整托出托作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_whole_ok_Click(object sender, EventArgs e)
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
            dsSave.ExtendedProperties.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, _model.OperationName);
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, Dns.GetHostName());
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, _model.ShiftKey);
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, _model.ShiftName);

            DataSet dsReturn = lotEntity.SavePalletLotData(dsSave);

            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                return;
            }
            else
            {
                MessageService.ShowMessage(string.Format("托号【{0}】出托成功!", txtPalletNo.Text.Trim()), "提示");
                this.DialogResult = DialogResult.OK;
                this.Close();
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
                _model.RoomKey = Convert.ToString(lueFactoryRoom.EditValue);

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
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _model.RoomKey;
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
            

            btn_whole_ok.Focus();
        }

     
        private void btn_Q_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_whole_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 包装作业查询功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Q_OK_Click(object sender, EventArgs e)
        {
            Hashtable hsParams = new Hashtable();
            hsParams["flag"] = "pallet";
            hsParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtQ_LotNum.Text.Trim();
            hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = roomkey;
            hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = txtQ_PalletNo.Text.Trim();
            hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME] = dtStart.Text.Trim();
            hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME] = dtEnd.Text.Trim();

            hsParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER + "2"] = txtQ_LotNum2.Text.Trim();
            hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO + "2"] = txtQ_PalletNo2.Text.Trim();

            LotDispatchForPallet lotdispatchforpallet = new LotDispatchForPallet();
            //lotdispatchforpallet.palletime01 = txtQ_PalletNo.Text;
            //lotdispatchforpallet.palletime02 = txtQ_PalletNo2.Text;
            //lotdispatchforpallet.lotnom01 = txtQ_LotNum.Text;
            //lotdispatchforpallet.lotnum02 = txtQ_LotNum2.Text;
            //lotdispatchforpallet.equipmentkeys = betEquipment.EditValue.ToString();
            //lotdispatchforpallet.palletime01 = dtStart.Text;
            //lotdispatchforpallet.palletime02 = dtEnd.Text;

            if (!string.IsNullOrEmpty(betEquipment.EditValue.ToString()))
            {
                hsParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY, betEquipment.EditValue.ToString());
            }

            DataSet dsReturn = lotEntity.GetQueryPalletData(hsParams);

            DataTable dtconsigment = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            if (dtconsigment.Rows.Count < 1)
            {
                MessageService.ShowMessage("没有查询到数据", "提示");
                return;
            }
            dtCommon = dtconsigment;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnChk_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsValidForPalletCheck()
        {
            bool bl_bak = true;
            if (string.IsNullOrEmpty(txtChk_lotnum.Text.Trim())
                && string.IsNullOrEmpty(txtChk_palletNo.Text.Trim())
                && string.IsNullOrEmpty(txtChk_Wo.Text.Trim())
                && string.IsNullOrEmpty(dteChk_start.Text.Trim())
                && string.IsNullOrEmpty(dteChk_end.Text.Trim()))
            {
                MessageService.ShowMessage("【查询条件】不能为空!", "提示");
                bl_bak = false;
            }
           
            return bl_bak;
        }
        /// <summary>
        /// 入库检验查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChk_ok_Click(object sender, EventArgs e)
        {
            if (!IsValidForPalletCheck()) return;

            Hashtable hstable = new Hashtable();
            hstable["flag"] = "query";
            if (!string.IsNullOrEmpty(txtChk_lotnum.Text.Trim()))
                hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtChk_lotnum.Text.Trim();
            if (!string.IsNullOrEmpty(txtChk_palletNo.Text.Trim()))
                hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = txtChk_palletNo.Text.Trim();
            if (!string.IsNullOrEmpty(dteChk_start.Text.Trim()))
                hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME] = dteChk_start.Text.Trim();
            if (!string.IsNullOrEmpty(dteChk_end.Text.Trim()))
                hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME] = dteChk_end.Text.Trim();
            if (!string.IsNullOrEmpty(txtChk_Wo.Text.Trim()))
                hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = txtChk_Wo.Text.Trim();

            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = roomkey;

            DataSet dsBak = lotEntity.GetToWarehouseCheckData(hstable);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowMessage(lotEntity.ErrorMsg);
                return;
            }
            if (dsBak == null
                || dsBak.Tables.Count < 1
                || !dsBak.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME))
            {
                MessageService.ShowMessage("未找到数据！", "提示");
                return;
            }

            this.dtCommon = dsBak.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            this.DialogResult = DialogResult.OK;
            this.Close();


        }

        private void txtQ_PalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetPalletNoQueryFocused();
            }
        }       

        private void dtStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetPalletNoQueryFocused();
            }
        }

        private void txtQ_LotNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetPalletNoQueryFocused();
            }
        }

        private void dtEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetPalletNoQueryFocused();
            }
        }

        private void SetPalletNoQueryFocused()
        {
            if (this.txtQ_PalletNo.Focused)
            {
                this.txtQ_LotNum.SelectAll();
                this.txtQ_LotNum.Focus();
                return;
            }
            if (txtQ_LotNum.Focused)
            {
                this.dtStart.SelectAll();
                this.dtStart.Focus();
                return;
            }
            if (dtStart.Focused)
            {
                this.dtEnd.SelectAll();
                this.dtEnd.Focus();
                return;
            }
            if (dtEnd.Focused)
            {
                btn_Q_OK.Focus();
            }
        }

        private void txtChk_lotnum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetToWarehouseFocus();
            }
        }

        private void txtChk_palletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetToWarehouseFocus();
            }
        }

        private void txtChk_Wo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetToWarehouseFocus();
            }
        }

        private void dteChk_start_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetToWarehouseFocus();
            }
        }

        private void dteChk_end_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetToWarehouseFocus();
            }
        }

        private void SetToWarehouseFocus()
        {
            if (this.txtChk_lotnum.Focused)
            {
                this.txtChk_palletNo.SelectAll();
                this.txtChk_palletNo.Focus();
                return;
            }
            if (this.txtChk_palletNo.Focused)
            {
                this.txtChk_Wo.SelectAll();
                this.txtChk_Wo.Focus();
                return;
            }
            if (this.txtChk_Wo.Focused)
            {
                this.dteChk_start.SelectAll();
                this.dteChk_start.Focus();
                return;
            }
            if (this.dteChk_start.Focused)
            {
                this.dteChk_end.SelectAll();
                this.dteChk_end.Focus();
                return;
            }
            if (this.dteChk_end.Focused)
            {
                this.btnChk_ok.Focus();
            }
        }

        /// <summary>
        /// 返到包装工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPalletNo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.txtPalletNo2.BackColor = Color.White;
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(Convert.ToString(lueFactoryRoom2.EditValue)))
                {
                    MessageService.ShowError("请选择【工厂车间】!");
                    return;
                }
                _model.RoomKey = Convert.ToString(lueFactoryRoom2.EditValue);

                CheckPalletNoForBackPackage();
            }
        }
        private bool isBackToPalletNo = true;
        /// <summary>
        /// 检验入库检作业，返到包装工序的合法性
        /// </summary>
        private void CheckPalletNoForBackPackage()
        {
            if (string.IsNullOrEmpty(txtPalletNo2.Text.Trim()))
            {
                this.txtLotNum_qty2.Text = string.Empty;
                this.txtPalletNo2.BackColor = Color.YellowGreen;
                isBackToPalletNo = false;
                return;
            }
            spalletno = txtPalletNo2.Text.Trim();
            Hashtable hstable = new Hashtable();
            hstable["flag"] = "pallet";
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = txtPalletNo2.Text.Trim();
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _model.RoomKey;
            DataSet dsBak = lotEntity.GetPalletOrLotData(hstable);
            dtPallet = dsBak.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];

            if (dtPallet.Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("【托号{0}】不存在!", spalletno));
                this.txtPalletNo2.Focus();
                this.txtPalletNo2.SelectAll();
                this.txtLotNum_qty2.Text = string.Empty;
                dtPallet = new DataTable();
                isBackToPalletNo = false;
                return;
            }

            DataRow drPallet = dtPallet.Rows[0];
            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("3"))
            {
                MessageService.ShowError(string.Format("【托号{0}】已入库!", spalletno));
                this.txtPalletNo2.Focus();
                this.txtPalletNo2.SelectAll();
                this.txtLotNum_qty2.Text = string.Empty;
                isBackToPalletNo = false;
                dtPallet = new DataTable();
                return;
            }
            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("10"))
            {
                MessageService.ShowError(string.Format("【托号{0}】已经返到包装，请确认!", spalletno));
                this.txtPalletNo2.Focus();
                this.txtPalletNo2.SelectAll();
                this.txtLotNum_qty2.Text = string.Empty;
                isBackToPalletNo = false;
                dtPallet = new DataTable();
                return;
            }
            if (drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP].ToString().Equals("4"))
            {
                MessageService.ShowError(string.Format("【托号{0}】已出货!", spalletno));
                this.txtPalletNo2.Focus();
                this.txtPalletNo2.SelectAll();
                this.txtLotNum_qty2.Text = string.Empty;
                isBackToPalletNo = false;
                dtPallet = new DataTable();
                return;
            }

            spalletno = txtPalletNo2.Text.Trim();
            txtLotNum_qty2.Text = drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY].ToString();
            DataTable dtPalletLot = dsBak.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
            this.gcOutPallet2.MainView = this.gvOutPallet;
            this.gcOutPallet2.DataSource = dtPalletLot;
            isBackToPalletNo = true;

            btn_whole_ok2.Focus();
        }

        private void btn_whole_ok2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPalletNo2.Text.Trim()))
            {
                MessageService.ShowMessage("【托号】不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(txtLotNum_qty2.Text))
            {
                MessageService.ShowMessage("【托号数量】不能为空!");
                return;
            }
            CheckPalletNoForBackPackage();
            if (!isBackToPalletNo) return;

            bool bl_bak = false;
            if (MessageService.AskQuestion(string.Format("确定要返托号【{0}】到包装么?", txtPalletNo2.Text.Trim()), "提示"))
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
            dsSave.ExtendedProperties.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO, spalletno);
            dsSave.ExtendedProperties.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT, _model.ShiftName);            
            dsSave.ExtendedProperties.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            dsSave.ExtendedProperties.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, Convert.ToString(lueFactoryRoom2.EditValue));
            DataSet dsReturn = lotEntity.SavePallet2Package(dsSave);

            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                return;
            }
            else
            {
                MessageService.ShowMessage(string.Format("托号【{0}】成功返到包装!", spalletno), "提示");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_whole_Cancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnCancelExchgPallet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtExchgOldPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SetExchgGridData();
            }
        }

        private void txtExchgNewPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
           string palletNo=  txtExchgNewPalletNo.Text;
     
            if (e.KeyChar == 13)
            {

                if (this._lotEntity.SurePalletNo(palletNo))
                {
                    MessageBox.Show(string.Format("【托号{0}已存在，请检查输入托号的有效性！】", palletNo));
                    return;
                }
                else
                {
                    SetExchgGridDataForNewPalletno();
                }
            }
        }

        private void btnSaveExchgPallet_Click(object sender, EventArgs e)
        {
            SaveExchgPalletNo();
        }

        private void SetExchgGridData()
        {
            //工厂车间
            if (string.IsNullOrEmpty(Convert.ToString(lueFactoryRoom3.EditValue)))
            {
                MessageService.ShowError("请选择【工厂车间】!");
                return;
            }
            _model.RoomKey = Convert.ToString(lueFactoryRoom3.EditValue);
            //判断是否输入托号
            if (string.IsNullOrEmpty(txtExchgOldPalletNo.Text.Trim()))
            {
                this.txtExchgOldPalletNo.Focus();
                return;
            }

            Hashtable hstable = new Hashtable();
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO, txtExchgOldPalletNo.Text.Trim());
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, _model.RoomKey);
            DataSet dsReturn = lotEntity.GetPalletCustLotData(hstable);

            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                txtExchgOldPalletNo.SelectAll();
                txtExchgOldPalletNo.Focus();
                return;
            }

            DataTable dtExchgConsigment = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            //托号不存在
            if (dtExchgConsigment.Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("托号【{0}】不存在，请确认!", txtExchgOldPalletNo.Text.Trim()));
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }

            DataRow drExchg = dtExchgConsigment.Rows[0];
            //已经入库检，不能变更托号
            if (Convert.ToString(drExchg[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("2"))
            {
                MessageService.ShowError(string.Format("托号【{0}】已经入库检，不能变更托号，请确认!", txtExchgOldPalletNo.Text.Trim()));
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }
            //已经入库，不能变更托号
            if (Convert.ToString(drExchg[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("3"))
            {
                MessageService.ShowError(string.Format("托号【{0}】已经入库，不能变更托号，请确认!", txtExchgOldPalletNo.Text.Trim()));
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }
            //已经出货，不能变更托号
            if (Convert.ToString(drExchg[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("4"))
            {
                MessageService.ShowError(string.Format("托号【{0}】已经出货，不能变更托号，请确认!", txtExchgOldPalletNo.Text.Trim()));
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }
            //不需要变更托号，请直接出托
            if (Convert.ToString(drExchg[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("0"))
            {
                MessageService.ShowError(string.Format("托号【{0}】请直接出拖后，重新包装!", txtExchgOldPalletNo.Text.Trim()));
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }
            //1，100满足托号变更要求

            //清除数据源
            this.gcExchangePalletNo.DataSource = null;

            this.gcExchangePalletNo.DataSource = dtExchgConsigment;
            this.gvExchangePalletNo.BestFitColumns();

        }

        private void SetExchgGridDataForNewPalletno()
        {
            //工厂车间
            if (string.IsNullOrEmpty(Convert.ToString(lueFactoryRoom3.EditValue)))
            {
                MessageService.ShowError("请选择【工厂车间】!");
                return;
            }
            //判断是否输入托号
            if (string.IsNullOrEmpty(txtExchgNewPalletNo.Text.Trim()))
            {
                this.txtExchgNewPalletNo.Focus();
                return;
            }
            //判断表格中是否有数据
            if (this.gvExchangePalletNo.RowCount < 1)
            {
                MessageService.ShowError("表格中不存在变更的数据，请确认!");
                this.txtExchgNewPalletNo.SelectAll();
                this.txtExchgNewPalletNo.Focus();
                return;
            }
            //判断新托号是否存在
            Hashtable hstable = new Hashtable();
            hstable.Add("flag", "pallet");
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO, txtExchgNewPalletNo.Text.Trim());
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, _model.RoomKey);
            DataSet dsReturn = lotEntity.GetPalletOrLotData(hstable);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                this.txtExchgNewPalletNo.SelectAll();
                this.txtExchgNewPalletNo.Focus();
                return;
            }

            DataTable dtExchgPallet = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];

            //新托号已经存在，变更托号不能使用
            if (dtExchgPallet.Rows.Count > 0)
            {
                MessageService.ShowError(string.Format("新托号【{0}】已经存在，变更托号不能使用，请确认!", txtExchgOldPalletNo.Text.Trim()));
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }
            //新托号添加到表格中
            for (int i = 0; i < this.gvExchangePalletNo.RowCount; i++)
            {
                this.gvExchangePalletNo.SetRowCellValue(i, WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW, this.txtExchgNewPalletNo.Text.Trim());
            }            
        }
        /// <summary>
        /// 保存托号变更数据
        /// </summary>
        private void SaveExchgPalletNo()
        {
            //确认要保存的数据源
            if (this.gvExchangePalletNo.RowCount < 1)
            {
                MessageService.ShowError("请确认需要变更的托号!");
                this.txtExchgOldPalletNo.SelectAll();
                this.txtExchgOldPalletNo.Focus();
                return;
            }
            //确认表格中的新托号是否存在
            bool blContinue = true;
            for (int i = 0; i < this.gvExchangePalletNo.RowCount; i++)
            {
                string newPalletNo = Convert.ToString(this.gvExchangePalletNo.GetRowCellValue(i, WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW));
                if (string.IsNullOrEmpty(newPalletNo.Trim()))
                {
                    MessageService.ShowError("表格中新托号不能为空，请确认!");
                    this.txtExchgNewPalletNo.SelectAll();
                    this.txtExchgNewPalletNo.Focus();
                    blContinue = false;
                    break;
                }
            }
            if (!blContinue) return;
            //获取表格数据
            DataTable dtGvData = this.gcExchangePalletNo.DataSource as DataTable;
            DataRow drUpdate=dtGvData.Rows[0];
           
            //提取表格中数据
            Hashtable hstable = new Hashtable();
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO, drUpdate[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW, drUpdate[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW]);
            hstable.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, _model.RoomKey);
            //保存托号变更数据
            DataSet dsReturn = lotEntity.SaveExchgPalletNumber(hstable);

            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                this.btnCancelExchgPallet.Focus();
                return;
            }
            else
            {
                MessageService.ShowMessage(string.Format("托号【{0}】成功变更为【{1}】!", 
                    Convert.ToString(drUpdate[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]), 
                    Convert.ToString(drUpdate[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW])));

                this.Close();
            }          
        }
    }
}