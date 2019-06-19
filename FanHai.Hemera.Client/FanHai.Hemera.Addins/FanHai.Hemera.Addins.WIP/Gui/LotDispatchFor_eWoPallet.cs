using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using FanHai.Hemera.Share.Common;
using FanHai.Gui.Framework.Gui;
using DevExpress.XtraLayout.Utils;
using System.Net;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotDispatchFor_eWoPallet : BaseUserCtrl
    {
        private LotAfterIvTestEntity lotEntity = new LotAfterIvTestEntity();
        private string _roomkey = string.Empty;
        private string _operationkey = string.Empty;
        private LotQueryEntity _entity = new LotQueryEntity();
        private BasePowerSetEntity _powerEntity = new BasePowerSetEntity();
        private DataTable dtPowerSetType = new DataTable(), dtPowerSet = new DataTable(), dtPowerSet_Dtl = null, dtBaseTestProLevel = null;
        private string p_type = string.Empty, p_set = string.Empty, p_dtl = string.Empty;
        public string palletno01 = string.Empty, palletno02 = string.Empty, lotnom01 = string.Empty, lotnum02 = string.Empty, equipmentkeys = string.Empty, palletime01 = string.Empty, palletime02 = string.Empty;
        IViewContent _view = null;
        LotDispatchDetailForPalletModel _package_model = null;
        decimal total_power = 0, current_power = 0, reduce_power = 0;

        /// <summary>
        /// 包装作业正在进行
        /// </summary>
        bool _isOnLoading = false;
        /// <summary>
        /// 是否校验侧板标签
        /// </summary>
        bool _chkSideCode = false;
        /// <summary>
        /// 是否校验客户编码
        /// </summary>
        bool _chkCustCode = false;
        /// <summary>
        /// 是否为已经保存过的托号
        /// </summary>
        bool _isSavedPalletNo = false;
        /// <summary>
        /// 是否可以混工单包装(默认可以混包装)
        /// </summary>
        bool IsReceiveMixWosByPackage = true;
        /// <summary>
        /// 是否可以混产品ID包装(默认不可以混包装)
        /// </summary>
        bool IsReceiveMixProIdByPackage = false;

        public LotDispatchFor_eWoPallet()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        public void InitializeLanguage()
        {

            this.chkCusterCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.chkCusterCode}");//检验客户编码
            this.chkSideCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.chkSideCode}");//Conergy侧板编码
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem5}");//产品ID号
            this.layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem6}");//工单号
            this.layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem4}");//SAP料号
            this.layoutControlItem17.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem17}");//托平均功率
            this.layoutControlItem18.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem18}");//花色
            this.layoutControlItem23.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem23}");//功率小数位
            this.layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem9}");//已入托数
            this.layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem11}");//包装分档
            this.layoutControlItem27.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem27}");//质量等级
            this.layoutControlItem19.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem19}");//托等级
            this.layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem7}");//功率分档类型
            this.layoutControlItem22.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem22}");//功率分档类型
            this.layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem8}");//托号
            this.layout_lotTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layout_lotTitle}");//组件序列号
            this.txtShift.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.txtShift}");//班别
            this.layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem10}");//备注
            this.SEQ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.SEQ}");//序号
            this.LOT_NUMBER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.LOT_NUMBER}");//组件序列号
            this.WORKNUMBER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.WORKNUMBER}");//所属工单
            this.PALLET_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.PALLET_TIME}");//包装时间
            this.VIRTUAL_PALLET_NO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.VIRTUAL_PALLET_NO}");//托号
            this.GRADE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.GRADE}");//质量等级
            this.SAP_NO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.SAP_NO}");//SAP料号
            this.EQUIPMENT_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.EQUIPMENT_NAME}");//包装台
            this.SHIFT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.SHIFT}");//班别
            this.CHECK_POWER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.CHECK_POWER}");//检验功率
            this.LOT_SIDECODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.LOT_SIDECODE}");//侧板编码
            this.LOT_CUSTOMERCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.LOT_CUSTOMERCODE}");//客户编码
            this.CONSIGNMENT_KEY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.CONSIGNMENT_KEY}");//包装主键
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem1}");//设备
            this.layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem12}");//线别
            this.layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem13}");//设备状态
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem3}");//工厂车间
            this.layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layoutControlItem2}");//工序


        }


        public LotDispatchFor_eWoPallet(IViewContent view)
        {
            this._view = view;
            InitializeComponent();
            InitializeLanguage();
        }



        public LotDispatchFor_eWoPallet(IViewContent view, LotDispatchDetailModel model)
        {
            _package_model = new LotDispatchDetailForPalletModel(model);
            this._view = view;
            InitializeComponent();
        }

        private void LotDispatchForPallet_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindOperations();
            BindLine();
            SetComponentEnabled(false);
            BindEquipment();

            this.lueShift.Text = _package_model.ShiftName;
            BindPowerSet();
            BindPalletQcLevel();

            BindGridData();
            BindShift();
            lblMenu.Text = "生产管理>过站管理>过站作业";
        }





        /// <summary>
        /// 获取班别
        /// </summary>
        private void GetShift()
        {
            //获取班别。
            Shift shift = new Shift();
            string shiftName = shift.GetCurrShiftName();
            this._package_model.ShiftName = shiftName;
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
                    this._package_model.RoomKey = this.lueFactoryRoom.EditValue.ToString();
                    this._package_model.RoomName = this.lueFactoryRoom.Text;
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }
        /// <summary>
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
                bool isExistOperationForPallet = false;
                for (int i = 0; i < strOperations.Length; i++)
                {
                    if (strOperations[i].Equals("包装"))
                    {
                        cbOperation.Properties.Items.Add(strOperations[i]);
                        isExistOperationForPallet = true;
                        break;
                    }
                }
                if (isExistOperationForPallet)
                {
                    this.cbOperation.SelectedIndex = 0;
                    _package_model.OperationName = this.cbOperation.Text;
                }
                else
                {
                    MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg001}"));//请确认是否有包装权限
                    SetComponentEnabled(false);
                    return;
                }
            }

        }
        /// <summary>
        /// 设定托号及批次输入框是否可用
        /// </summary>
        /// <param name="usabled"></param>
        private void SetComponentEnabled(bool usabled)
        {
            if (usabled)
            {
                this.txtLot_Num.Properties.ReadOnly = false;
                this.txtPalletNo.Properties.ReadOnly = false;
            }
            if (!usabled)
            {
                this.txtLot_Num.Properties.ReadOnly = true;
                this.txtPalletNo.Properties.ReadOnly = true;
            }
        }
        /// <summary>
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
        /// <summary>
        /// 绑定设备。
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cbOperation.Text.Trim();
            string strFactoryRoomName = this.lueFactoryRoom.Text;
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue == null ? string.Empty : this.lueFactoryRoom.EditValue.ToString();
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            //如果工厂车间或者工序或者线别主键为空。
            if (string.IsNullOrEmpty(strFactoryRoomName)
                || string.IsNullOrEmpty(strOperation)
                || string.IsNullOrEmpty(strLines))
            {
                SetComponentEnabled(false);
                return;
            }
            this.lueEquipment.EditValue = string.Empty;
            this.lueEquipment.Properties.ReadOnly = false;

            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, strLines.Split(','));
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueEquipment.Properties.DataSource = ds.Tables[0];
                this.lueEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME;
                this.lueEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }

            SetEquipmentState();
            SetLineValue();
        }

        private void BindShift()
        {
            //Shift shift = new Shift();
            //string shiftName = shift.GetCurrShiftName();
            //string shiftKey = string.Empty;// shift.IsShiftValueExists(shiftName);
            //this.txtShift.Text = shiftName;
            //this.txtShift.Tag = shiftKey;


            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            this.lueShift.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShift.Properties.DisplayMember = "CODE";
            this.lueShift.Properties.ValueMember = "CODE";
            this.lueShift.ItemIndex = 0;
            Shift shift = new Shift();
            string defaultShift = shift.GetCurrShiftName();
            lueShift.EditValue = defaultShift;

        }
        private void BindGridData()
        {
            DataTable dtconsigment = CommonUtils.CreateDataTable(new WIP_CONSIGNMENT_FIELDS());
            dtconsigment.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SEQ, typeof(Int16));

            if (!dtconsigment.Columns.Contains(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                dtconsigment.Columns.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER);
            if (!dtconsigment.Columns.Contains(WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER))
                dtconsigment.Columns.Add(WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER);
            if (!dtconsigment.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY))
                dtconsigment.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY);
            if (!dtconsigment.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME))
                dtconsigment.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME);
            if (!dtconsigment.Columns.Contains(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE))
                dtconsigment.Columns.Add(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE);
            if (!dtconsigment.Columns.Contains(POR_LOT_FIELDS.FIELD_LOT_SIDECODE))
                dtconsigment.Columns.Add(POR_LOT_FIELDS.FIELD_LOT_SIDECODE);
            if (!dtconsigment.Columns.Contains(POR_LOT_FIELDS.FIELD_PALLET_TIME))
                dtconsigment.Columns.Add(POR_LOT_FIELDS.FIELD_PALLET_TIME);

            gcConSigment.DataSource = dtconsigment;
        }

        private void BindPalletQcLevel()
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

            luePalletLevel.ItemIndex = -1;
            luePalletLevel.Properties.ValueMember = "Column_code";
            luePalletLevel.Properties.DisplayMember = "Column_Name";
            luePalletLevel.Properties.DataSource = dview.Table;

            lueQcLevel.ItemIndex = -1;
            lueQcLevel.Properties.ValueMember = "Column_code";
            lueQcLevel.Properties.DisplayMember = "Column_Name";
            lueQcLevel.Properties.DataSource = dview.Table;

            repositoryItemLookUpEdit_GRADE.DisplayMember = "Column_Name";
            repositoryItemLookUpEdit_GRADE.ValueMember = "Column_code";
            repositoryItemLookUpEdit_GRADE.DataSource = dview.Table;

            //是否可以混产品包装
            string[] l_mixproid = new string[] { "FACTORY_CODE", "IS_ValidData", "CONTROL_ITEM" };
            string category_proid = BASEDATA_CATEGORY_NAME.Basic_MixPackage_ByFactory;
            DataTable dtCommon2 = BaseData.Get(l_mixproid, category_proid);

            DataRow[] drs2 = dtCommon2.Select(string.Format("FACTORY_CODE='{0}' and CONTROL_ITEM='{1}'", _package_model.RoomName, "PRO_ID"));
            if (drs2 != null && drs2.Length > 0)
            {
                string bl_proid = Convert.ToString(drs2[0]["IS_ValidData"]);
                try
                {
                    this.IsReceiveMixProIdByPackage = bool.Parse(bl_proid);
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg002}"));//基础数据表混包装需要设置为【true/false】,请与系统管理员联系
                }
            }
        }

        /// <summary>
        /// 初始化功率档位-功率分档类型，功率档，包装分档
        /// </summary>
        private void BindPowerSet()
        {
            DataSet dsReturn = _powerEntity.GetPowerSetDtl(new Hashtable());
            if (dsReturn.Tables.Contains(BASE_POWERSET_DETAIL.DATABASE_TABLE_NAME))
                dtPowerSet_Dtl = dsReturn.Tables[BASE_POWERSET_DETAIL.DATABASE_TABLE_NAME];

            if (dsReturn.Tables.Contains(BASE_POWERSET.DATABASE_TABLE_NAME))
            {
                DataTable dtPowerSetMain = dsReturn.Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
                DataView dv01 = dtPowerSetMain.DefaultView;
                dtPowerSetType = dv01.ToTable(true, new string[] { BASE_POWERSET.FIELDS_PS_RULE + "1", BASE_POWERSET.FIELDS_PS_CODE });
                dtPowerSet = dv01.ToTable(true, new string[] { BASE_POWERSET.FIELDS_MODULE_NAME + "1", BASE_POWERSET.FIELDS_POWERSET_KEY, BASE_POWERSET.FIELDS_PS_CODE, BASE_POWERSET.FIELDS_PS_SEQ, BASE_POWERSET.FIELDS_PMAXSTAB });

                luePowerType.Properties.ValueMember = BASE_POWERSET.FIELDS_PS_CODE;
                luePowerType.Properties.DisplayMember = BASE_POWERSET.FIELDS_PS_RULE + "1";
                luePowerType.Properties.DataSource = dtPowerSetType;
                if (dtPowerSetType.Rows.Count > 0)
                    luePowerType.ItemIndex = 0;
                if (!string.IsNullOrEmpty(p_type))
                    luePowerType.EditValue = p_type;

                luePowerType_EditValueChanged(null, null);
            }
        }
        /// <summary>
        /// 选择功率分档类型，筛选功率分档数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void luePowerType_EditValueChanged(object sender, EventArgs e)
        {
            if (luePowerType.EditValue != null && !string.IsNullOrEmpty(luePowerType.EditValue.ToString()))
            {
                DataTable dtMiddle = dtPowerSet.Clone();
                DataRow[] drs = dtPowerSet.Select(string.Format(BASE_POWERSET.FIELDS_PS_CODE + "='{0}'", luePowerType.EditValue.ToString()));
                foreach (DataRow dr in drs)
                    dtMiddle.ImportRow(dr);

                DataView dv = dtMiddle.DefaultView;
                dv.Sort = BASE_POWERSET.FIELDS_PS_SEQ + " asc";
                luePowerSet.Properties.ValueMember = BASE_POWERSET.FIELDS_POWERSET_KEY;
                luePowerSet.Properties.DisplayMember = BASE_POWERSET.FIELDS_MODULE_NAME + "1";
                luePowerSet.Properties.DataSource = dv.ToTable(); ;

                if (dtMiddle.Rows.Count > 0)
                    luePowerSet.ItemIndex = 0;
                if (!string.IsNullOrEmpty(p_set))
                    luePowerSet.EditValue = p_set;

                luePowerSet_EditValueChanged(null, null);
            }
        }
        /// <summary>
        /// 选择功率分档数据，筛选功率子分档数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void luePowerSet_EditValueChanged(object sender, EventArgs e)
        {
            if (luePowerSet.EditValue != null && !string.IsNullOrEmpty(luePowerSet.EditValue.ToString()))
            {
                luePalletPowerSet.ItemIndex = -1;
                luePalletPowerSet.Properties.DataSource = null;
                if (dtPowerSet_Dtl == null || dtPowerSet_Dtl.Rows.Count < 1) return;

                DataTable dtDtl = dtPowerSet_Dtl.Clone();
                DataRow[] drs = dtPowerSet_Dtl.Select(string.Format(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY + "='{0}'", luePowerSet.EditValue.ToString()));
                foreach (DataRow dr in drs)
                    dtDtl.ImportRow(dr);
                DataView dv = dtDtl.DefaultView;
                dv.Sort = BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL + " asc";
                luePalletPowerSet.Properties.ValueMember = BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL;
                luePalletPowerSet.Properties.DisplayMember = BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL;
                luePalletPowerSet.Properties.DataSource = dv.ToTable();

                if (!string.IsNullOrEmpty(p_dtl))
                    luePalletPowerSet.EditValue = p_dtl;
            }
        }
        private void txtLot_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtLot_Num.BackColor = Color.White;
            if (e.KeyChar == 13)
            {
                if (!this._isOnLoading)
                {
                    DataTable dtTemp = gcConSigment.DataSource as DataTable;
                    gcConSigment.DataSource = null;
                    gcConSigment.DataSource = dtTemp.Clone();
                }
                this._isOnLoading = true;
                LotNumCheckAndSave();
            }
        }
        /// <summary>
        /// 检验批次信息是否有效并进站作业
        /// </summary>
        public void LotNumCheckAndSave()
        {
            if (string.IsNullOrEmpty(txtLot_Num.Text.Trim())) return;

            bool bl_bak = true;
            IsValidData(out bl_bak);
            if (!bl_bak) return;


            _package_model.PalletNo = txtPalletNo.Text.Trim();

            //侧板编码或客户编码校验
            #region
            if (this._chkCustCode || this._chkSideCode)
            {
                string msgTitle = string.Empty;
                Hashtable hs = new Hashtable();
                hs.Add("flag", "lotcustsidecode");
                if (this._chkCustCode)
                {
                    msgTitle = "客户编码";
                    hs.Add(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE, txtLot_Num.Text.Trim());

                }
                if (this._chkSideCode)
                {
                    msgTitle = "侧板编码";
                    hs.Add(POR_LOT_FIELDS.FIELD_LOT_SIDECODE, txtLot_Num.Text.Trim());
                }
                hs.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, _package_model.RoomKey);

                DataSet dsBack = lotEntity.GetExistPalletLotNum(hs);
                if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
                {
                    MessageService.ShowError(lotEntity.ErrorMsg);
                    txtLot_Num.Focus();
                    txtLot_Num.SelectAll();
                    return;
                }
                DataTable dtLot = dsBack.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                if (dtLot.Rows.Count < 1)
                {
                    MessageService.ShowError(string.Format("{1}【{0}】不存在，请确认", _package_model.LotNumber, msgTitle));
                    txtLot_Num.Focus();
                    txtLot_Num.SelectAll();
                    return;
                }

                if (dtLot.Rows.Count > 1)
                {
                    MessageService.ShowError(string.Format("{1}【{0}】大于1笔，不能入托，请确认", _package_model.LotNumber, msgTitle));
                    txtLot_Num.Focus();
                    txtLot_Num.SelectAll();
                    return;
                }
                _package_model.LotNumber = Convert.ToString(dtLot.Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            }
            else
            {
                _package_model.LotNumber = txtLot_Num.Text.Trim();
            }
            #endregion

            //判断批次是否合法   
            DataSet dsParams = new DataSet();
            Hashtable htParams = new Hashtable();
            htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, _package_model.LotNumber);
            htParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, _package_model.RoomKey);
            htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);                //0:未暂停的批次；1：暂停的批次。
            htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, "N");               //N：生产批次 L：组件补片批次
            htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);        //0:未结束未删除 1：已结束 2：已删除
            htParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, _package_model.OperationName);//工序名称

            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            //进行查询
            DataSet dsReturn = _entity.Query(dsParams);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }

            if (dsReturn.Tables.Count < 1 || dsReturn.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("未找到过站【批次{0}】,请确认!", txtLot_Num.Text.Trim()));
                txtLot_Num.SelectAll();
                txtLot_Num.Focus();
                return;
            }

            //----------------------------------------------------------------------------------------------------
            //校验批次 有效性
            #region
            LotQueryEntity queryEntity = new LotQueryEntity();
            DataSet dsLot = queryEntity.GetLotInfo(_package_model.LotNumber);

            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                return;
            }
            if (null == dsLot || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
            {

                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg003}"),StringParser.Parse("${res:Global.SystemInfo}"));//序列号不存在, 系统提示
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            _package_model.LotSideCode = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_SIDECODE]);
            _package_model.LotCustCode = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]);
            string curRoomKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            string curOperationName = Convert.ToString(dsLot.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            int deleteFlag = Convert.ToInt32(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            int holdFlag = Convert.ToInt32(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            double quantity = Convert.ToDouble(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);
            double initQuantity = Convert.ToDouble(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
            string stateFlag = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG]);

            //批次状态为空。
            if (string.IsNullOrEmpty(stateFlag))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg004}"), StringParser.Parse("${res:Global.SystemInfo}"));//批次状态为空, 系统提示
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            LotStateFlag lotStateFlag = (LotStateFlag)(Convert.ToInt32(stateFlag));
            //检查批次所在的工序是否是当前选定工序。
            if (curRoomKey != _package_model.RoomKey || curOperationName != _package_model.OperationName)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg005}"), StringParser.Parse("${res:Global.SystemInfo}"));//批次号在指定车间的工序中不存在, 系统提示
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);//获取有权限的所有工序
            //检查登录用户对批次所在的工序是否拥有权限。 
            if ((operations + ",").IndexOf(curOperationName + ",") == -1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg006}"), StringParser.Parse("${res:Global.SystemInfo}"));//对不起，您没有权限操作, 系统提示
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            //判断批次是否结束或被删除。
            if (deleteFlag == 1 || deleteFlag == 2)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg007}"), StringParser.Parse("${res:Global.SystemInfo}"));//批次已经结束或已删除 系统提示
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            //判断批次是否暂停
            if (holdFlag == 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg008}"), StringParser.Parse("${res:Global.SystemInfo}"));//批次已暂停 系统提示
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            //批次状态已完成
            if (lotStateFlag >= LotStateFlag.Finished)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg009}"), StringParser.Parse("${res:Global.SystemInfo}"));//该批次已完成 系统提示
                //MessageService.ShowMessage("该批次已完成。", "系统提示");
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                return;
            }
            //如果数量为空，则结束批次。
            if (quantity == 0)
            {
                _package_model.OperationType = LotOperationType.Terminal;
                TerminalLotDialog terminalLot = new TerminalLotDialog(_package_model);
                //显示结束批次的对话框。
                terminalLot.ShowDialog();
                return;
            }
            #endregion
            //----------------------------------------------------------------------------------------------------------------------           

            #region 获得校验数据
            Hashtable hstable = new Hashtable();
            hstable["flag"] = "lot";
            hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = _package_model.LotNumber;
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _package_model.RoomKey;
            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = _package_model.PalletNo;
            #endregion
            //获得关于批次信息的数据记录      
            // DataSet dsReturn01 = lotEntity.GetPalletOrLotData(hstable);

            //工单和产品ID号
            #region
            DataSet ds01 = lotEntity.GetWOProductByLotNum(_package_model.LotNumber, _package_model.RoomKey);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                this.txtLot_Num.Focus();
                this.txtLot_Num.SelectAll();
                return;
            }

            DataTable dt01 = ds01.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            if (dt01.Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("获取【{0}】工单和产品ID号失败！", _package_model.LotNumber));
                InitTextControlByLotNum();
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(_package_model.WorkOrder))
                {
                    _package_model.WorkOrder = Convert.ToString(dt01.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                    txtWo.Text = _package_model.WorkOrder;

                    WorkOrders workordersEntity = new WorkOrders();
                    Hashtable hsparams = new Hashtable();
                    hsparams.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, _package_model.WorkOrder);
                    hsparams.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME, WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage);
                    DataSet dsWoAttribute = workordersEntity.GetWorkOrderAttributeValue(hsparams);
                    if (!string.IsNullOrEmpty(workordersEntity.ErrorMsg))
                    {
                        MessageService.ShowError(workordersEntity.ErrorMsg);
                        return;
                    }
                    DataTable dtAttribute = dsWoAttribute.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                    if (dtAttribute.Rows.Count > 0)
                    {
                        //是否可以混工单设定
                        this.IsReceiveMixWosByPackage = bool.Parse(Convert.ToString(dtAttribute.Rows[0][POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]));
                    }
                }

                //默认可以混工单
                if (!this.IsReceiveMixWosByPackage && !Convert.ToString(dt01.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]).Equals(_package_model.WorkOrder))
                {
                    MessageService.ShowError(string.Format("组件【{0}】工单号{1}不一致,不能混工单包装!",
                     _package_model.LotNumber,
                     Convert.ToString(dt01.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER])));
                    InitTextControlByLotNum();
                    return;
                }

                if (string.IsNullOrEmpty(_package_model.ProductID))
                {
                    _package_model.ProductID = Convert.ToString(dt01.Rows[0][POR_PRODUCT.FIELDS_PRODUCT_CODE]);
                    txtPro_ID.Text = _package_model.ProductID;
                }

                if (!Convert.ToString(dt01.Rows[0][POR_PRODUCT.FIELDS_PRODUCT_CODE]).Equals(_package_model.ProductID))
                {
                    MessageService.ShowError(string.Format("组件【{0}】产品ID号{1}不一致！",
                        _package_model.LotNumber,
                        Convert.ToString(dt01.Rows[0][POR_PRODUCT.FIELDS_PRODUCT_CODE])));
                    InitTextControlByLotNum();
                    return;
                }
            }


            #endregion

            //是否混花，混档，混等级判定
            #region

            if (gvConSigment.RowCount < 1)
            {
                Hashtable hsLevel = new Hashtable();
                hsLevel.Add(POR_PRODUCT.FIELDS_PRODUCT_CODE, _package_model.ProductID);
                DataSet dsProLevel = lotEntity.GetTestRulePackageLevel(hsLevel);
                if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
                {
                    MessageService.ShowError(lotEntity.ErrorMsg);
                    return;
                }
                dtBaseTestProLevel = dsProLevel.Tables[BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME];
            }

            #endregion


            //校验终检等级，花色
            #region
            hstable["check"] = "check_lot_level";
            DataSet ds02 = lotEntity.GetPalletOrLotData(hstable);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                txtLot_Num.BackColor = Color.Red;
                return;
            }
            if (ds02 != null && ds02.Tables.Contains(WIP_CUSTCHECK_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dt02 = ds02.Tables[WIP_CUSTCHECK_FIELDS.DATABASE_TABLE_NAME];
                if (dt02.Rows.Count < 1)
                {
                    MessageService.ShowError(string.Format("获取【{0}】终检等级信息失败！", _package_model.LotNumber));
                    InitTextControlByLotNum();
                    return;
                }
                else
                {
                    DataRow dr02 = dt02.Rows[0];

                    if (string.IsNullOrEmpty(_package_model.QcLevel))
                    {
                        _package_model.QcLevel = Convert.ToString(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_LEVEL]);
                        lueQcLevel.EditValue = _package_model.QcLevel;
                        //获得混花，混档，混等级判定数据
                        #region

                        if (gvConSigment.RowCount < 1 && dtBaseTestProLevel != null && dtBaseTestProLevel.Rows.Count > 0)
                        {
                            DataRow rowLevel = (from v in dtBaseTestProLevel.AsEnumerable()
                                                where v.Field<string>(BASE_TESTRULE_PROLEVEL.FIELDS_GRADE) == _package_model.QcLevel
                                                select v).SingleOrDefault();
                            if (rowLevel != null)
                            {
                                //混花色
                                bool minColor = false;
                                //混档位
                                bool minLevel = false;
                                //等级设定组
                                _package_model.PackageGroup = Convert.ToString(rowLevel[BASE_TESTRULE_PROLEVEL.FIELDS_PALLET_GROUP]);

                                string color = Convert.ToString(rowLevel[BASE_TESTRULE_PROLEVEL.FIELDS_MIN_COLOR]);
                                if (!string.IsNullOrEmpty(color) || !bool.TryParse(color, out minColor))
                                {
                                    minColor = false;
                                }
                                _package_model.CheckColor = minColor;

                                string level = Convert.ToString(rowLevel[BASE_TESTRULE_PROLEVEL.FIELDS_MIN_LEVEL]);
                                if (!string.IsNullOrEmpty(level) || !bool.TryParse(level, out minLevel))
                                {
                                    minLevel = false;
                                }
                                _package_model.CheckPowerLevel = minLevel;
                            }
                        }
                        else
                        {
                            _package_model.CheckColor = false;
                            _package_model.CheckPowerLevel = false;
                            _package_model.PackageGroup = _package_model.MinGroup;
                        }


                    }

                    if (string.IsNullOrEmpty(_package_model.PalletLevel))
                    {
                        _package_model.PalletLevel = _package_model.QcLevel;
                        luePalletLevel.EditValue = _package_model.PalletLevel;
                    }
                    if (string.IsNullOrEmpty(_package_model.ModuelColor))
                    {
                        _package_model.ModuelColor = Convert.ToString(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR]);
                        txtColor.Text = _package_model.ModuelColor;
                    }
                    #endregion

                    //判断-可以混等级-按批次校验
                    #region
                    if (dtBaseTestProLevel != null && dtBaseTestProLevel.Rows.Count > 0 && !string.IsNullOrEmpty(_package_model.PackageGroup))
                    {
                        string minGroup = string.Empty;
                        DataRow rowLevel = (from v in dtBaseTestProLevel.AsEnumerable()
                                            where v.Field<string>(BASE_TESTRULE_PROLEVEL.FIELDS_GRADE) == Convert.ToString(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_LEVEL])
                                            select v).SingleOrDefault();
                        if (rowLevel != null)
                        {
                            minGroup = Convert.ToString(rowLevel[BASE_TESTRULE_PROLEVEL.FIELDS_PALLET_GROUP]);

                            if (string.IsNullOrEmpty(minGroup))
                                minGroup = _package_model.MinGroup;
                        }


                        if (!_package_model.PackageGroup.Equals(minGroup))
                        {
                            MessageService.ShowError(string.Format("组件【{0}】等级不符合混包要求！", _package_model.LotNumber));
                            InitTextControlByLotNum();
                            return;
                        }
                    }

                    #endregion
                    //判断-不可以混等级-按托校验
                    #region
                    else
                    {
                        if (!Convert.ToString(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_LEVEL]).Equals(_package_model.PalletLevel))
                        {
                            MessageService.ShowError(string.Format("组件【{0}】等级不一致！", _package_model.LotNumber));
                            InitTextControlByLotNum();
                            return;
                        }
                    }
                    #endregion



                    //判断-不可以混花色
                    #region
                    if (!_package_model.CheckColor)
                    {
                        if (!Convert.ToString(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR]).Equals(_package_model.ModuelColor))
                        {
                            MessageService.ShowError(string.Format("组件【{0}】花色不一致！", _package_model.LotNumber));
                            InitTextControlByLotNum();
                            return;
                        }
                    }
                    #endregion
                    //判断-可以混花色
                    #region
                    else
                    {
                        txtColor.Text = Convert.ToString(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR]);
                    }
                    #endregion



                    if (!dr02[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER].ToString().Trim().Equals(string.Empty))
                    {
                        current_power = Math.Round(Convert.ToDecimal(dr02[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER].ToString()), 2);
                        reduce_power = 0;
                        _package_model.AvgPower = current_power;
                    }
                }
            }
            #endregion

            #region 检验SAP料号
            hstable["check"] = "sap_material";
            DataSet ds06 = lotEntity.GetPalletOrLotData(hstable);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
                txtLot_Num.BackColor = Color.Red;
                return;
            }

            if (ds06 != null && ds06.Tables.Contains(POR_PRODUCT_DTL.DATABASE_TABLE_NAME))
            {
                if (ds06.Tables[POR_PRODUCT_DTL.DATABASE_TABLE_NAME].Rows.Count < 1)
                {
                    MessageService.ShowError(string.Format("批次【{0}】SAP料号没有设定，请找工艺设置!", txtLot_Num.Text.Trim()));
                    InitTextControlByLotNum();
                    return;
                }
                DataRow dr06 = ds06.Tables[POR_PRODUCT_DTL.DATABASE_TABLE_NAME].Rows[0];
                if (string.IsNullOrEmpty(_package_model.SapMaterial))
                {
                    string sapno_tmp = Convert.ToString(dr06[POR_PRODUCT_DTL.FIELDS_SAP_PN]);
                    if (string.IsNullOrEmpty(sapno_tmp.Trim()))
                    {
                        MessageService.ShowError(string.Format("批次【{0}】SAP料号没有设定，请找工艺设置!", txtLot_Num.Text.Trim()));
                        InitTextControlByLotNum();
                        return;
                    }

                    _package_model.SapMaterial = Convert.ToString(dr06[POR_PRODUCT_DTL.FIELDS_SAP_PN]);
                    txtSapNo.Text = _package_model.SapMaterial;
                }

                if (!_package_model.SapMaterial.Equals(Convert.ToString(dr06[POR_PRODUCT_DTL.FIELDS_SAP_PN])))
                {
                    MessageService.ShowError(string.Format("【SAP料号】组件{0}的SAP料号{1}不一致!", txtLot_Num.Text.Trim(), Convert.ToString(dr06[POR_PRODUCT_DTL.FIELDS_SAP_PN])));
                    InitTextControlByLotNum();
                    return;
                }
            }
            else
            {
                MessageService.ShowError(string.Format("没有获取到批次【{0}】SAP料号，请与系统管理员联系!", txtLot_Num.Text.Trim()));
                InitTextControlByLotNum();
                return;
            }

            #endregion


            DataTable dtGrid = gcConSigment.DataSource as DataTable;
            string seq = dtGrid.Compute("max(SEQ)", "").ToString();

            DataRow drNew = dtGrid.NewRow();
            drNew["SEQ"] = seq == string.Empty ? "1" : (Convert.ToInt16(seq) + 1).ToString();
            drNew[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = _package_model.LotNumber;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = txtWo.Text;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID] = txtPro_ID.Text;
            //drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME] = lotEntity.GetSysdate();
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = txtPalletNo.Text;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO] = txtPalletNo.Text;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = lueQcLevel.EditValue.ToString();
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = txtSapNo.Text;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = this.lueShift.Text.ToString();
            drNew[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER] = current_power.ToString();
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME] = _package_model.EquipmentName;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY] = _package_model.EquipmentKey;
            drNew[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE] = _package_model.LotCustCode;
            drNew[POR_LOT_FIELDS.FIELD_LOT_SIDECODE] = _package_model.LotSideCode;
            drNew[POR_LOT_FIELDS.FIELD_PALLET_TIME] = lotEntity.GetSysdate();

            dtGrid.Rows.Add(drNew);

            DataView dv = dtGrid.DefaultView;
            dv.Sort = "SEQ asc";
            gcConSigment.DataSource = dv.ToTable();
            this.gvConSigment.BestFitColumns();

            //如果包装托号表中有数据，则不允许修改托号
            if (gvConSigment.RowCount > 0)
            {
                this.txtPalletNo.Properties.ReadOnly = true;
            }


            txtEnterPallet.Text = dtGrid.Rows.Count.ToString();
            CalCulatePower();

            txtLot_Num.Focus();
            txtLot_Num.SelectAll();
        }

        /// <summary>
        /// 入托校验数据
        /// </summary>
        /// <param name="bl_bak"></param>
        private void IsValidData(out bool bl_bak)
        {
            bl_bak = true;
            if (string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
            {
                MessageService.ShowError(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg010}"));//【托盘号】不能为空!
                this.txtPalletNo.SelectAll();
                this.txtPalletNo.Focus();
                bl_bak = false;
                return;
            }
            DataTable dtExistLotNum = this.gcConSigment.DataSource as DataTable;
            //校验客户编码
            if (this._chkCustCode)
            {
                DataRow[] drs = dtExistLotNum.Select(string.Format(@"LOT_CUSTOMERCODE='{0}'", txtLot_Num.Text.Trim()));
                if (drs != null && drs.Length > 0)
                {
                    MessageService.ShowError(string.Format("【客户编码{0}】已在【托号{1}】中", txtLot_Num.Text.Trim(), txtPalletNo.Text.Trim()));
                    this.txtLot_Num.SelectAll();
                    this.txtLot_Num.Focus();
                    bl_bak = false;
                }
            }
            //校验侧板编码
            else if (this._chkSideCode)
            {
                DataRow[] drs = dtExistLotNum.Select(string.Format(@"LOT_SIDECODE='{0}'", txtLot_Num.Text.Trim()));
                if (drs != null && drs.Length > 0)
                {
                    MessageService.ShowError(string.Format("【侧板编码{0}】已在【托号{1}】中", txtLot_Num.Text.Trim(), txtPalletNo.Text.Trim()));
                    this.txtLot_Num.SelectAll();
                    this.txtLot_Num.Focus();
                    bl_bak = false;
                }
            }
            //判断批次是否已经在包装中
            else
            {

                DataRow[] drs = dtExistLotNum.Select(string.Format(@"LOT_NUMBER='{0}'", txtLot_Num.Text.Trim()));
                if (drs != null && drs.Length > 0)
                {
                    MessageService.ShowError(string.Format("【批次{0}】已在【托号{0}】中", txtLot_Num.Text.Trim(), txtPalletNo.Text.Trim()));
                    this.txtLot_Num.SelectAll();
                    this.txtLot_Num.Focus();
                    bl_bak = false;
                }
            }
            if (!bl_bak) return;

            //检验托号是否为E工单
            Hashtable hstable = new Hashtable();
            hstable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, txtLot_Num.Text.Trim());
            hstable.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, _package_model.RoomKey);
            bool blewo = lotEntity.IsExperimentLot(hstable);
            if (!blewo)
            {
                MessageService.ShowError(string.Format(@"【批次{0}】不是E工单批次，不能做E工单包装！", txtLot_Num.Text.Trim()));
                bl_bak = false;
            }
            if (!bl_bak) return;

            //再次检验托号的合法性
            if (gvConSigment.RowCount < 1)
            {
                if (!IsExistPalletNo())
                {
                    bl_bak = false;
                    return;
                }
            }

            Hashtable hsParams = new Hashtable();
            hsParams["flag"] = "lot";
            hsParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtLot_Num.Text;
            hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO] = txtPalletNo.Text;

            DataSet dsBack = lotEntity.GetExistPalletLotNum(hsParams);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                this.txtLot_Num.SelectAll();
                this.txtLot_Num.Focus();
                bl_bak = false;
            }

            //判断托号是否在包装表格中存在,如果不存在，则清空表格中数据
            for (int i = 0; i < gvConSigment.RowCount; i++)
            {
                string s_palletno = gvConSigment.GetRowCellValue(i, WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO).ToString();
                if (!txtPalletNo.Text.Trim().Equals(s_palletno))
                {
                    DataTable dtTemp = gcConSigment.DataSource as DataTable;
                    gcConSigment.DataSource = null;
                    gcConSigment.DataSource = dtTemp.Clone();
                    InitTextControlByLotNum();
                    this.txtPalletNo.Properties.ReadOnly = false;
                    break;
                }
            }
        }

        /// <summary>
        /// 批次过账作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            bool checkDataSouce = false;
            DataTable dtSave = gcConSigment.DataSource as DataTable;
            //数据源是否和包装托数量一致

            if (!string.IsNullOrEmpty(txtEnterPallet.Text.Trim())
                      && Convert.ToInt16(txtEnterPallet.Text.Trim()) == gvConSigment.RowCount
                && dtSave.Rows.Count == gvConSigment.RowCount)
            {
                checkDataSouce = true;
            }
            else
            {
                checkDataSouce = false;
            }

            if (!checkDataSouce) return;


            SavePallet(1);
        }
        /// <summary>
        /// 批次保存作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            bool checkDataSouce = false;
            DataTable dtSave = gcConSigment.DataSource as DataTable;
            //数据源是否和包装托数量一致

            if (!string.IsNullOrEmpty(txtEnterPallet.Text.Trim())
                      && Convert.ToInt16(txtEnterPallet.Text.Trim()) == gvConSigment.RowCount
                && dtSave.Rows.Count == gvConSigment.RowCount)
            {
                checkDataSouce = true;
            }
            else
            {
                checkDataSouce = false;
            }

            if (!checkDataSouce) return;

            SavePallet(0);
        }
        /// <summary>
        /// 过账/保存作业，1：过账；0：保存
        /// </summary>
        /// <param name="saveType">1:过账;0:保存（不过帐）</param>
        private void SavePallet(int saveType)
        {
            if (string.IsNullOrEmpty(txtPro_ID.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg011}"), StringParser.Parse("${res:Global.SystemInfo}"));//产品ID号不能为空! 系统提示
                //MessageService.ShowMessage(string.Format("产品ID号不能为空!", txtPalletNo.Text.Trim()), "提示");
                return;
            }
            //判断托号是否存在
            if (!IsExistPalletNo()) return;

            if (this.gcConSigment.DataSource == null)
            {
                MessageService.ShowMessage(string.Format("托号【{0}】没有需要保存的数据!", txtPalletNo.Text.Trim()), "提示");
                return;
            }
            DataTable dtGv = this.gcConSigment.DataSource as DataTable;
            if (dtGv.Rows.Count == 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg012}"), StringParser.Parse("${res:Global.SystemInfo}"));//请确认需要保存的数据! 系统提示
                return;
            }

            if (saveType == 0)
            {
                if (!MessageService.AskQuestion(string.Format("E工单包装数量【{0}】，确认【保存】么?", txtEnterPallet.Text)))
                {
                    return;
                }
            }
            if (saveType == 1)
            {
                if (!MessageService.AskQuestion(string.Format("E工单包装数量【{0}】，确认【过站作业】么?", txtEnterPallet.Text)))
                {
                    return;
                }
            }

            //检查服务器端是否停止服务或者程序版本是否有效。
            #region
            switch (FanHai.Hemera.Utils.Common.Utils.CheckServerStopServiceOrVersionInvalid())
            {
                case 0:
                    break;
                case 1:
                    //盘点卡控，不能执行进出站操作
                    MessageService.ShowMessage("盘点时间，不能进行操作", "系统提示");
                    return;
                case 2:
                    //程序版本太低
                    MessageService.ShowMessage("版本太低，不能进行操作", "系统提示");
                    return;
                default:
                    break;
            }
            #endregion


            DataSet dsSave = new DataSet();
            dtGv.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            dsSave.Merge(dtGv, true, MissingSchemaAction.Add);
            int pallet_lot_qty = dtGv.Rows.Count;
            DataTable dtconsigment = CommonUtils.CreateDataTable(new WIP_CONSIGNMENT_FIELDS());
            if (dtconsigment.Columns.Contains("CHECKER_TIME"))
                dtconsigment.Columns.Remove("CHECKER_TIME");
            if (!dtconsigment.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME))
                dtconsigment.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME);
            if (!dtconsigment.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET))
                dtconsigment.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET);


            DataRow drConsigment = dtconsigment.NewRow();

            string roomKey = _package_model.RoomKey;
            string roomName = _package_model.RoomName;
            string operationName = _package_model.OperationName;
            string equipmentKey = _package_model.EquipmentKey;

            //车间不能为空。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg013}"), StringParser.Parse("${res:Global.SystemInfo}"));//车间名称不能为空。 系统提示
                return;
            }
            //工序不能为空。
            if (string.IsNullOrEmpty(operationName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg014}"), StringParser.Parse("${res:Global.SystemInfo}"));//工序不能为空。 系统提示
                return;
            }

            string shiftName = this._package_model.ShiftName;
            string shiftKey = string.Empty;                           //班别主键          

            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);//获取有权限的所有工序
            string curOperationName = _package_model.OperationName;
            //检查登录用户对批次所在的工序是否拥有权限。 
            if ((operations + ",").IndexOf(curOperationName + ",") == -1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg015}"), StringParser.Parse("${res:Global.SystemInfo}"));//对不起，您没有权限操作。 系统提示
                return;
            }

            //E工单托号
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET] = 2;

            if (this._chkCustCode)
                drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = 2;
            else if (this._chkSideCode)
                drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = 1;
            else
                drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = 0;

            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER] = _package_model.AvgPower;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _package_model.RoomKey;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY] = _package_model.EquipmentKey;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME] = _package_model.EquipmentName;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = shiftName;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_KEY] = _package_model.LineKey;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_NAME] = _package_model.LineName;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = _package_model.WorkOrder;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID] = _package_model.ProductID;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE] = _package_model.AvgPowerRange;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER] = "";// lblTotlePower.Text;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = txtSapNo.Text;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_CODE] = Convert.ToString(luePowerType.EditValue);
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_SEQ] = luePowerSet.GetColumnValue(BASE_POWERSET.FIELDS_POWERSET_KEY);
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_DTL_SUBCODE] = Convert.ToString(luePalletPowerSet.EditValue);
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = _package_model.ShiftName;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_COLOR] = txtColor.Text;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = luePalletLevel.EditValue.ToString();
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] = CommonUtils.GenerateNewKey(0);
            //drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = chkCusterCode.Checked == true ? 2 : chkSideCode.Checked == true ? 1 : 0;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = txtPalletNo.Text;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO] = txtPalletNo.Text;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL] = _package_model.SigmentLevel;
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY] = pallet_lot_qty.ToString();
            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_MEMO1] = this.txtmemo.Text.ToString().Trim();
            dtconsigment.Rows.Add(drConsigment);

            dsSave.Merge(dtconsigment, true, MissingSchemaAction.Add);
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, Dns.GetHostName());
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, _package_model.ShiftKey);
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, _package_model.ShiftName);

            #region 
            string save_type = string.Empty;
            //保存作业
            if (saveType == 0)
            {
                save_type = "onlysave";
            }
            //过账作业
            else if (saveType == 1)
            {
                save_type = "real";
            }
            else
            {
                MessageService.ShowError("非法操作,不能过站");
                return;
            }
            #endregion

            dsSave.ExtendedProperties.Add("savetype", save_type);
            DataSet dsReturn = lotEntity.SavePalletLotData(dsSave);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                return;
            }
            else
            {
                if (save_type.Equals("real"))
                    MessageService.ShowMessage(string.Format("E工单托号【{0}】成功入托!", txtPalletNo.Text.Trim()), "系统提示");
                else
                    MessageService.ShowMessage(string.Format("E工单托号【{0}】保存成功!", txtPalletNo.Text.Trim()), "系统提示");
                this._isSavedPalletNo = false;
                this.txtPalletNo.Properties.ReadOnly = false;
            }

            //初始化Model
            _package_model = new LotDispatchDetailForPalletModel(_package_model);
            string palletno = txtPalletNo.Text.Trim();
            InitControlsValue();
            txtPalletNo.Text = palletno;

        }
        /// <summary>
        /// 打开包装清单打印列表
        /// </summary>
        private void ShowPackageList()
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //创建新的视图对象，并显示该视图界面。 -打开包装清单 
            PackingListPrintViewContent view = new PackingListPrintViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 变更托号
        /// </summary>
        private void InitControlsValue()
        {
            //lblTotlePower.Text = "0";
            txtLot_Num.Text = string.Empty;
            txtWo.Text = string.Empty;
            txtPro_ID.Text = string.Empty;
            txtAvgPower.Text = string.Empty;
            txtPalletNo.Text = string.Empty;
            txtEnterPallet.Text = string.Empty;
            txtSapNo.Text = string.Empty;
            this.lueShift.Text = _package_model.ShiftName;
            luePalletLevel.EditValue = string.Empty;
            lueQcLevel.EditValue = string.Empty;
            luePowerType.EditValue = string.Empty;
            luePowerSet.EditValue = string.Empty;
            luePalletLevel.EditValue = string.Empty;
            txtColor.Text = string.Empty;
            DataTable dtNew = (this.gcConSigment.DataSource as DataTable).Clone();

            this.gcConSigment.DataSource = null;
            this.gcConSigment.DataSource = dtNew;
            this.txtPalletNo.Properties.ReadOnly = false;
            this.txtmemo.Text = string.Empty;
            txtPalletNo.SelectAll();
            txtPalletNo.Focus();
        }

        /// <summary>
        /// 初始托号
        /// </summary>
        private void InitTextControl()
        {
            if (gvConSigment.RowCount < 1)
            {
                this.txtWo.Text = string.Empty;
                this.txtPro_ID.Text = string.Empty;
                this.luePalletLevel.EditValue = null;
                this.txtEnterPallet.Text = string.Empty;
                this.txtAvgPower.Text = string.Empty;
                //this.lblTotlePower.Text = string.Empty;
                this.txtPalletNo.SelectAll();
                this.txtPalletNo.Focus();
            }
            txtPalletNo.SelectAll();
            txtPalletNo.Focus();
        }
        /// <summary>
        /// 初始序列号
        /// </summary>
        private void InitTextControlByLotNum()
        {
            if (gvConSigment.RowCount < 1)
            {
                this.txtLot_Num.Text = string.Empty;
                this.txtWo.Text = string.Empty;
                this.txtPro_ID.Text = string.Empty;
                this.txtSapNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                lueQcLevel.EditValue = string.Empty;
                luePalletLevel.EditValue = string.Empty;
                luePowerType.EditValue = string.Empty;
                luePowerSet.EditValue = string.Empty;
                luePalletPowerSet.EditValue = string.Empty;
                this.txtmemo.Text = string.Empty;

                _package_model = new LotDispatchDetailForPalletModel(_package_model);
            }
            this.txtLot_Num.SelectAll();
            this.txtLot_Num.Focus();
        }

        /// <summary>
        /// 返回到工作站进站画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示工作站作业界面。
            LotDispathViewContent view = new LotDispathViewContent(_package_model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }

        private void txtPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPalletNo.BackColor = Color.White;
            if (e.KeyChar != 13) return;
            if (string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
            {
                InitTextControl();
            }
            else
            {
                _package_model = new LotDispatchDetailForPalletModel(_package_model);
                _package_model.PalletNo = txtPalletNo.Text.Trim();
                //判断托号是否存在
                if (!IsExistPalletNo()) return;

                txtLot_Num.Focus();
                txtLot_Num.SelectAll();
            }
        }
        /// <summary>
        /// 判断托号是否存在
        /// </summary>
        /// <returns></returns>
        private bool IsExistPalletNo()
        {
            if (string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg010}"), StringParser.Parse("${res:Global.SystemInfo}"));//托号不能为空。 系统提示
                txtPalletNo.Focus();
                return false;
            }
            else
            {
                txtPalletNo.Text = txtPalletNo.Text.Trim().ToUpper();
            }

            Hashtable hstable = new Hashtable();
            hstable["flag"] = "pallet";
            //hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtLot_Num.Text.Trim();

            //不添加车间查询条件
            //hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _package_model.RoomKey;
            if (_package_model.PalletNo == null)
                _package_model.PalletNo = this.txtPalletNo.Text.Trim();

            hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = _package_model.PalletNo;
            //判断托号是否存在
            DataSet dsReturn = lotEntity.GetPalletOrLotData(hstable);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                txtPalletNo.Focus();
                txtPalletNo.SelectAll();
                return false;
            }
            if (dsReturn != null
                && dsReturn.Tables.Count > 0
               && dsReturn.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)
                && dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                DataRow drComm = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME].Rows[0];
                if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("1"))
                {
                    MessageService.ShowError(string.Format("托号【{0}】已经等待【入库检验】，请确认", _package_model.PalletNo));
                    InitTextControl();
                    return false;
                }
                if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("100"))
                {
                    MessageService.ShowError(string.Format("托号【{0}】已经从入库检返到【包装工序】，请出托检验!", _package_model.PalletNo));
                    InitTextControl();
                    return false;
                }
                if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("2"))
                {
                    MessageService.ShowError(string.Format("托号【{0}】已经等待【入库】，请确认", _package_model.PalletNo));
                    InitTextControl();
                    return false;
                }
                if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("3"))
                {
                    MessageService.ShowError(string.Format("托号【{0}】已经【入库】，请确认", _package_model.PalletNo));
                    InitTextControl();
                    return false;
                }
                if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("4"))
                {
                    MessageService.ShowError(string.Format("托号【{0}】已经【出货】，请确认", _package_model.PalletNo));
                    InitTextControl();
                    return false;
                }
                if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]).Equals("0") && !this._isSavedPalletNo)
                {
                    this._isSavedPalletNo = true;
                    string codeType = Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE]);
                    string palletSaveNo = _package_model.PalletNo;
                    if (codeType.Equals("0"))
                    {
                        this.chkCusterCode.Checked = false;
                        this.chkSideCode.Checked = false;
                    }
                    if (codeType.Equals("1"))
                    {
                        this.layout_lotTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layout_lotTitle}");//侧板编码号
                        this.chkCusterCode.Checked = false;
                        this.chkSideCode.Checked = true;
                        this.txtPalletNo.Text = palletSaveNo;
                    }
                    if (codeType.Equals("2"))
                    {
                        this.layout_lotTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.layout_lotTitle2}");//客户编码号
                        //this.layout_lotTitle.Text = "客户编码号";
                        this.chkCusterCode.Checked = true;
                        this.chkSideCode.Checked = false;
                        this.txtPalletNo.Text = palletSaveNo;
                    }
                    //判断是正常托
                    if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET]).Equals("0"))
                    {
                        MessageService.ShowError(string.Format("非法操作，【{0}】不是E工单托号，请确认并与相关人员联系!", _package_model.PalletNo));
                        InitTextControl();
                        return false;

                        //this.chkLastPallet.Checked = true;
                    }

                    //判断是否尾单
                    if (Convert.ToString(drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET]).Equals("1"))
                    {
                        MessageService.ShowError(string.Format("非法操作，【{0}】是尾单，请确认并与相关人员联系!", _package_model.PalletNo));
                        InitTextControl();
                        return false;

                        //this.chkLastPallet.Checked = true;
                    }
                    this.txtmemo.Text = drComm[WIP_CONSIGNMENT_FIELDS.FIELDS_MEMO1].ToString();
                    DataTable dtLot = dsReturn.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                    if (dtLot.Rows.Count > 0)
                    {

                        foreach (DataRow drLot in dtLot.Rows)
                        {
                            //侧板编码
                            if (codeType.Equals("1"))
                            {
                                txtLot_Num.Text = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_SIDECODE]);
                            }
                            //客户编码
                            if (codeType.Equals("2"))
                            {
                                txtLot_Num.Text = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]);
                            }
                            //批次条码
                            if (codeType.Equals("0"))
                            {
                                txtLot_Num.Text = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                            }

                            txtLot_Num_KeyPress(null, new KeyPressEventArgs((char)(13)));
                        }
                    }
                }
            }
            return true;
        }

        private void tsbSingleOutPallet_Click(object sender, EventArgs e)
        {
            if (!this._isOnLoading) return;

            if (gvConSigment.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPallet.Msg016}"), StringParser.Parse("${res:Global.SystemInfo}"));//请选择需要出托的组件!。 系统提示
                return;
            }
            Hashtable hs = new Hashtable();
            hs[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = gvConSigment.GetRowCellValue(gvConSigment.FocusedRowHandle, POR_LOT_FIELDS.FIELD_LOT_NUMBER).ToString();
            hs[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _package_model.RoomKey;
            hs[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = gvConSigment.GetRowCellValue(gvConSigment.FocusedRowHandle, WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO).ToString();
            reduce_power = Convert.ToDecimal(gvConSigment.GetRowCellValue(gvConSigment.FocusedRowHandle, WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER));
            current_power = 0;
            DataSet dsReturn = lotEntity.UpdateLotOutPallet(hs);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                return;
            }


            DataTable dtGv = gcConSigment.DataSource as DataTable;
            dtGv.Rows.Remove(gvConSigment.GetFocusedDataRow());
            this.gcConSigment.DataSource = dtGv;
            txtEnterPallet.Text = (Convert.ToInt16(txtEnterPallet.Text.Trim() == string.Empty ? "1" : txtEnterPallet.Text.Trim()) - 1).ToString();
            CalCulatePower();
            if (gvConSigment.RowCount < 1)
            {
                this.txtPalletNo.Properties.ReadOnly = false;
                InitTextControlByLotNum();
            }

        }

        private void CalCulatePower()
        {
            DataTable dtGrid = gcConSigment.DataSource as DataTable;
            if (dtGrid == null) return;

            #region 当前功率-总功率
            decimal totlePower = 0;
            string s_avgpower = string.Empty;
            foreach (DataRow drPower in dtGrid.Rows)
            {
                totlePower += Convert.ToDecimal(drPower[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER] == null ? "0" : drPower[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER]);
            }
            if (totlePower < 1)
            {
                s_avgpower = "0";
            }
            else
            {
                s_avgpower = Math.Round(total_power / dtGrid.Rows.Count, 2).ToString();
            }

            this.txtAvgPower.Text = s_avgpower;
            #endregion


        }

        private void chkSideCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSideCode.Checked == true)
            {
                this.chkCusterCode.Checked = false;
                this.layout_lotTitle.Text = "侧板编码号";
                this._chkSideCode = true;
                this._chkCustCode = false;
            }
            if (chkSideCode.Checked == false)
            {
                this.layout_lotTitle.Text = "组件序列号";
                this._chkSideCode = false;
            }
            InitControlsValue();

        }

        private void chkCusterCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCusterCode.Checked == true)
            {
                this.chkSideCode.Checked = false;
                this.layout_lotTitle.Text = "客户编码号";
                this._chkCustCode = true;
                this._chkSideCode = false;
            }
            if (chkCusterCode.Checked == false)
            {
                this.layout_lotTitle.Text = "组件序列号";
                this._chkCustCode = false;
            }
            InitControlsValue();

        }

        private void tsbWholeOutPallet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_roomkey))
                _roomkey = _package_model.RoomKey;
            string subTitle = "整托出托作业";
            LotDispatchForPltChkDialog ldfp = new LotDispatchForPltChkDialog(10, _package_model, subTitle);
            //整托出托    
            if (DialogResult.OK == ldfp.ShowDialog())
            {

            }
        }

        private void tsbQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_roomkey))
                _roomkey = _package_model.RoomKey;
            string subTitle = "包装查询";
            LotDispatchForPltChkDialog ldfp = new LotDispatchForPltChkDialog(20, _package_model, subTitle);
            //查询托号            
            if (DialogResult.OK == ldfp.ShowDialog())
            {
                DataTable dtGv = ldfp.dtCommon;
                this.gcConSigment.DataSource = null;
                this.gcConSigment.DataSource = dtGv;
                this.gvConSigment.BestFitColumns();
                InitTextControl();
                this._isOnLoading = false;
            }
        }

        private void tsbExchgPalletNo_Click(object sender, EventArgs e)
        {

        }

        private void gvConSigment_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (e.FocusedRowHandle > -1)
            //BindViewTopData();
        }

        private void BindViewTopData()
        {
            Hashtable hsParam = new Hashtable();
            string lotnum = gvConSigment.GetFocusedRowCellValue(POR_LOT_FIELDS.FIELD_LOT_NUMBER).ToString();
            string palletno = gvConSigment.GetFocusedRowCellValue(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO).ToString();
            string roomkey = _package_model.RoomKey;
            hsParam.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotnum);
            hsParam.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO, palletno);
            hsParam.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, roomkey);

            DataSet dsReturn = lotEntity.GetPalletCustLotData(hsParam);
            if (!string.IsNullOrEmpty(lotEntity.ErrorMsg))
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
                return;
            }

            DataTable dtConsigment = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            if (dtConsigment.Rows.Count < 1) return;

            DataRow drConsigment = dtConsigment.Rows[0];

            txtPalletNo.Text = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO]);
            txtWo.Text = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER]);
            txtPro_ID.Text = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID]);
            txtSapNo.Text = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO]);

            txtLot_Num.Text = Convert.ToString(drConsigment[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            txtEnterPallet.Text = string.Empty;
            this.lueShift.Text = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT]);
            txtAvgPower.Text = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER]);

            lueQcLevel.EditValue = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL]);
            luePalletLevel.EditValue = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL]);
            txtColor.Text = Convert.ToString(drConsigment[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR]);

            luePowerType.EditValue = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_CODE]);
            luePowerSet.EditValue = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_SEQ]);
            luePalletPowerSet.EditValue = Convert.ToString(drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_DTL_SUBCODE]);
        }

        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            if (this._isOnLoading) return;
            this._isOnLoading = true;

            this._package_model.RoomKey = this.lueFactoryRoom.EditValue.ToString();
            this._package_model.RoomName = this.lueFactoryRoom.Text;

            BindLine();
            //重新绑定设备控件
            BindEquipment();

            this._isOnLoading = false;
        }
        /// <summary>
        /// 工序调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._isOnLoading) return;
            this._isOnLoading = true;
            //重新绑定设备控件
            BindEquipment();

            this._isOnLoading = false;
        }

        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            if (this._isOnLoading) return;
            this._isOnLoading = true;

            //设置设备状态。
            SetEquipmentState();
            SetLineValue();

            if (lueEquipment.ItemIndex == -1)
            {
                InitControlsValue();
                SetComponentEnabled(false);

            }
            else
            {
                this._package_model.EquipmentKey = this.lueEquipment.EditValue.ToString();
                this._package_model.EquipmentName = this.lueEquipment.Text;

                SetComponentEnabled(true);
            }
            //this.teLotNumber.Select();
            //this.teLotNumber.SelectAll();
            this._isOnLoading = false;
        }

        private void lueEquipment_Click(object sender, EventArgs e)
        {
            ////工序自定义属性 IS_SHOW_PICK_EQUIPMENT_DIALOG 确定是否显示设备选择对话框。
            //if (this._isShowPickEquipmetDialog)
            //{
            //    DataTable dt = this.lueEquipment.Properties.DataSource as DataTable;
            //    DataSet ds = new DataSet();
            //    ds.Merge(dt);
            //    ShowEquipmentPickDialog(ds);
            //    SetEquipmentState();
            //    SetLineValue();
            //}
        }
        /// <summary>
        /// 设置设备状态。
        /// </summary>
        private void SetEquipmentState()
        {
            string equipmentKey = Convert.ToString(this.lueEquipment.GetColumnValue("EQUIPMENT_REAL_KEY"));
            if (string.IsNullOrEmpty(equipmentKey))
            {
                equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            }
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                EquipmentEntity entity = new EquipmentEntity();
                DataSet ds = entity.GetEquipmentState(equipmentKey);
                if (string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    string description = Convert.ToString(ds.Tables[0].Rows[0]["DESCRIPTION"]);
                    string stateName = Convert.ToString(ds.Tables[0].Rows[0]["EQUIPMENT_STATE_NAME"]);
                    this.txtEquipmentState.Text = ds.Tables[0].Rows.Count > 0
                            ? string.Format("{0}({1})", stateName, description)
                            : string.Empty;
                    System.Drawing.Color backColor = FanHai.Hemera.Utils.Common.Utils.GetEquipmentStateColor(stateName);
                    this.txtEquipmentState.BackColor = backColor;
                }
                else
                {
                    this.txtEquipmentState.Text = string.Empty;
                    this.txtEquipmentState.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                this.txtEquipmentState.Text = string.Empty;
            }
        }
        /// <summary>
        /// 设置线别的值。
        /// </summary>
        private void SetLineValue()
        {
            string lineKey = Convert.ToString(this.lueEquipment.GetColumnValue("LINE_KEY"));
            this.lueLine.EditValue = lineKey;
        }
    }

}
