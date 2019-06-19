using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Framework.Gui;
using System.IO;
using System.Collections;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Addins.WIP.Gui;
using DevExpress.XtraEditors.Controls;
using System.Reflection;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotDispatchForCustCheck : BaseUserCtrl
    {
        public string checktype = string.Empty;
        private int _width = 0, _height = 0;
        private int _width_iv = 0, _height_iv = 0;
        private Hashtable hsColor = new Hashtable();
        LotAfterIvTestEntity _lotAfterIvTestEntity = new LotAfterIvTestEntity();
        LotComponentTrayEntity _lotComponentTrayEntity = new LotComponentTrayEntity();
        LotOperationEntity _lotEntity = new LotOperationEntity();
        LotQueryEntity _entity = new LotQueryEntity();
        LotDispatchDetailModel _model = null;                           //参数数据。
        IViewContent _view = null;
        DataTable dtIvTest = null;
        bool isChangePowerShow = false;
        string strChangePowerShow;


        /// <summary>
        /// 非特殊托盘位置不可以设置混合工单
        /// </summary>
        bool IsReceiveMixProIdByPackage = false;

        #region
        /// <summary>
        /// 是否校验铭牌 1：校验 0：不校验。
        /// </summary>
        string _labelCheck = string.Empty;
        /// <summary>
        /// 认证类型
        /// </summary>
        string _labelType = string.Empty;

        string _level = string.Empty;
        /// <summary>
        /// 认证版本
        /// </summary>
        string _lableVar = string.Empty;
        /// <summary>
        /// 产品型号
        /// </summary>
        string _proModel_Name = string.Empty;
        /// <summary>
        /// 产品型号前缀
        /// </summary>
        string _proModel_Name_Prefix = string.Empty;
        /// <summary>
        /// 产品型号后缀
        /// </summary>
        string _proModel_Name_Suffix = string.Empty;
        /// <summary>
        /// 主分档ArticNo
        /// </summary>
        string _psSub_Code = string.Empty;
        /// <summary>
        /// 批次测试日期
        /// </summary>
        string _lotIvTestDate = string.Empty;
        ///// <summary>
        ///// 是否继续作业
        ///// </summary>
        //bool isContinut = false;
        /// <summary>
        /// 清洁工序判定的花色
        /// </summary>
        string _moduleColorFromCleanOpt = string.Empty;
        /// <summary>
        /// 是否校验EL图片
        /// </summary>
        bool isCheckELPic = false;
        /// <summary>
        /// EL图片地址
        /// </summary>
        string _pic_eladdress = string.Empty;
        /// <summary>
        /// 图片地址+图片测试日期
        /// </summary>
        string _pic_Title = string.Empty;
        /// <summary>
        /// 是否校验IV图片
        /// </summary>
        bool isCheckIVPic = false;
        /// <summary>
        /// IV图片地址
        /// </summary>
        string _pic_lvaddress = string.Empty;
        /// <summary>
        /// IV图片地址+图片测试日期
        /// </summary>
        string _pic_Title_iv = string.Empty;
        /// <summary>
        /// 是否检查IV测试数据。
        /// </summary>
        private bool _isCheckIVTestdata = false;
        #endregion

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="view"></param>
        public LotDispatchForCustCheck(LotDispatchDetailModel model, IViewContent view)
            : this()
        {
            this._model = model;
            this._view = view;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotDispatchForCustCheck()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体载入事件函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotDispatchForCustCheck_Load(object sender, EventArgs e)
        {
            
            if (_model == null)
            {
                return;
            }
            //初始化功率等级、花色         
            BindPowerLevelAndColor();
            //绑定档位数据
            BindComponentTray();
            if (!IsValidAndBindLotNumWoProid())
            {
                sbtnReturn.Select();
                return;
            }
            //是否对组件工单进行ELIV的校验
            IsCheckELIV();

            //绑定图片地址
            BindLueLotPic();
            //判断是终检还是客检
            InitCheckType();
            //绑定档位名称及其子分档。
            BindPowerInfo();

            this.txtOperator.Text = WIP_CUSTCHECK_FIELDS.TrackInOperation;
            this.txtCode2.Select();

        }
        /// <summary>
        /// 绑定产品等级和花色
        /// </summary>
        private void BindPowerLevelAndColor()
        {
            //绑定产品等级
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code", Column_name_Desc }; //add Column_Name_Desc 
            string category = "Basic_TestRule_PowerSet";
            DataTable dtProLevel = BaseData.Get(l_s, category);
            DataTable dtLevel = dtProLevel.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtProLevel.Select(string.Format("Column_type='{0}' and Column_code<>'Grade_SCRAP'", BASE_POWERSET.PRODUCT_GRADE));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);

            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";
            luejudge01.Properties.DisplayMember = "Column_Name";
            luejudge01.Properties.ValueMember = "Column_code";
            luejudge01.Properties.DataSource = dview.Table;

            //绑定花色
            DataTable dtColor = dtProLevel.Clone();
            dtColor.TableName = "color";
            drs = dtProLevel.Select(string.Format(string.Format("Column_type='{0}'", "ColorJudge")));
            foreach (DataRow dr in drs)
                dtColor.ImportRow(dr);

            if (drs != null)
            {
                foreach (DataRow dr in drs)
                {
                    hsColor[Convert.ToString(dr["Column_Name"])] = Convert.ToString(dr["Column_code"]);
                }
            }
        }



        private void BindComponentTray()
        {
            //绑定产品等级
            string[] l_s = new string[] { "ComponentTrayValue", "ComponentTrayName", "LineName", "LineKey", "IsBuffer" };
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            string category = "Base_Component_Tray";
            DataTable dtProLevel = BaseData.Get(l_s, category);
            dtProLevel.TableName = "Level";

            var q1 = (from dt1 in dtProLevel.AsEnumerable()//查询
                      orderby Convert.ToInt32(dt1.Field<string>("ComponentTrayValue")) ascending//排序
                      where dt1.Field<string>("LineKey") == _model.LineKey //条件
                      select dt1).AsDataView();

            lueComponentTray.Properties.DisplayMember = "ComponentTrayName";
            lueComponentTray.Properties.ValueMember = "ComponentTrayValue";
            lueComponentTray.Properties.DataSource = q1;

        }

        /// <summary>
        /// 绑定批次号及相应信息，同时检查序列号的测试数据是否存在
        /// </summary>
        /// <returns></returns>
        private bool IsValidAndBindLotNumWoProid()
        {
            if (string.IsNullOrEmpty(_model.LotNumber))
            {
                return true;
            }

            try
            {
                //是否需要检查IV测试数据。
                LotOperationEntity lotEntity = new LotOperationEntity();
                this._isCheckIVTestdata = lotEntity.IsCheckIVTestData(_model.LotNumber);

                txtCode1.Text = _model.LotNumber;
                //检查批次数据是否存在，检查IV测试数据是否存在，获取对应的功率档位。
                if (!IsUsableCode())
                    return false;
                //-----------------------------------------------------------------------------------------
                //获取批次数据及其对应工单、产品数据。
                DataSet dsReturn = new DataSet();
                dsReturn = _lotAfterIvTestEntity.GetWOProductByLotNum(_model.LotNumber, _model.RoomKey);
                if (!string.IsNullOrEmpty(_lotAfterIvTestEntity.ErrorMsg))
                {
                    MessageService.ShowError(_lotAfterIvTestEntity.ErrorMsg);
                    return false;
                }
                DataTable dtWoProduct = dsReturn.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                string proLevel = Convert.ToString(dtWoProduct.Rows[0][POR_LOT_FIELDS.FIELD_PRO_LEVEL]);
                this.txtWorder.Text = Convert.ToString(dtWoProduct.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                this.tePartNumber.Text = Convert.ToString(dtWoProduct.Rows[0][POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                this.txtPro_id.Text = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_PRODUCT_CODE]);
                this.txtPro_id.Tag = Convert.ToString(dtWoProduct.Rows[0][POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                this.txtCode1.Tag = Convert.ToString(dtWoProduct.Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY]);
                this._labelCheck = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_LABELCHECK]);
                this._labelType = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_LABELTYPE]);
                this._lableVar = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_LABELVAR]);
                this._proModel_Name = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_PROMODEL_NAME]);
                this._proModel_Name_Prefix = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_MODULE_TYPE_PREFIX]);
                this._proModel_Name_Suffix = Convert.ToString(dtWoProduct.Rows[0][POR_PRODUCT.FIELDS_MODULE_TYPE_SUFFIX]);

                if (string.IsNullOrEmpty(txtPro_id.Text.Trim()) || string.IsNullOrEmpty(txtWorder.Text.Trim()))
                {
                    while (MessageBox.Show(string.Format("批次【{0}】的【产品ID号/工单号】不能为空！", _model.LotNumber),
                                           "提示",
                                           MessageBoxButtons.OKCancel,
                                           MessageBoxIcon.Error,
                                           MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    return false;
                }

                //组件产品等级为null，第一次检验。
                if (string.IsNullOrEmpty(proLevel))
                {
                    //获取产品数据表中设置的主等级。
                    PorProductEntity porproductEntity = new PorProductEntity();
                    DataSet dsGrade = porproductEntity.GetProductDtlGrade(txtPro_id.Text.Trim());
                    if (!string.IsNullOrEmpty(porproductEntity.ErrorMsg))
                    {
                        MessageService.ShowError(porproductEntity.ErrorMsg);
                        return false;
                    }
                    DataTable dtGrade = dsGrade.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
                    if (dtGrade.Rows.Count > 0)
                    {
                        luejudge01.EditValue = Convert.ToString(dtGrade.Rows[0][POR_PRODUCT_DTL.FIELDS_PRODUCT_GRADE]);
                    }
                }
                else
                {//组件产品等级不为null，非首次检验。
                    luejudge01.EditValue = proLevel;
                }

                //获取工单客户。
                WorkOrders workordersEntity = new WorkOrders();
                Hashtable hsparams = new Hashtable();
                hsparams.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, txtWorder.Text.Trim());
                hsparams.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME, WORKORDER_SETTING_ATTRIBUTE.Customer);
                DataSet dsWoAttribute = workordersEntity.GetWorkOrderAttributeValue(hsparams);
                DataTable dtWoAttribute = dsWoAttribute.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                if (dtWoAttribute.Rows.Count < 1)
                {//如果没有设置工单客户，则客户类型默认为常规类型。
                    _model.CustomerType = WORKORDER_SETTING_ATTRIBUTE.CommondCustomerType;
                }
                else
                {
                    //根据设置的工单客户，获取客户类型。
                    hsparams = new Hashtable();
                    hsparams.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, txtWorder.Text.Trim());
                    hsparams.Add(WORKORDER_SETTING_ATTRIBUTE.Customer, Convert.ToString(dtWoAttribute.Rows[0][POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]));
                    DataSet dsCustomerType = workordersEntity.GetViewForWorkOrder(hsparams);

                    DataTable dtCustomerType = dsCustomerType.Tables["V_WORK_ORDER_ATTR"];
                    if (dtCustomerType.Rows.Count > 0)
                    {
                        _model.CustomerType = Convert.ToString(dtCustomerType.Rows[0]["CUSTOMER_TYPE"]);
                    }
                    else
                    {//如果没有抓到记录，则客户类型默认为常规类型。
                        _model.CustomerType = WORKORDER_SETTING_ATTRIBUTE.CommondCustomerType;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取批次档位名称及其子分档名称。
        /// </summary>
        private void BindPowerInfo()
        {
            DataSet dsPower = new DataSet();

            dsPower = _lotAfterIvTestEntity.GetModulePowerInfo(this.txtCode1.Text.ToString().Trim());

            if (dsPower.Tables[0].Rows.Count > 0)
            {
                this.txtPowrName.Text = dsPower.Tables[0].Rows[0]["MODULE_NAME"].ToString().Trim();
            }
            else
            {
                this.txtPowrName.Text = "";
            }
            if (dsPower.Tables[1].Rows.Count > 0)
            {
                this.txtPowerSub.Text = dsPower.Tables[1].Rows[0]["POWERLEVEL"].ToString().Trim();
            }
            else
            {
                this.txtPowerSub.Text = "";
            }
        }
        /// <summary>
        /// 根据检验类型初始化操作界面。
        /// </summary>
        private void InitCheckType()
        {
            //终检作业
            if (checktype.Equals(CHECKTYPE.DATA_GROUP_ENDCHECK))
            {
                lblMenu.Text = "生产管理>过站管理>过站作业—终检";
                layoutControlItem_customercheck.Visibility = LayoutVisibility.Never;
                //layoutControlItem_elivcheck.Visibility = LayoutVisibility.Never;   

                if (!this.isCheckIVPic)
                {
                    layoutControlItem_lvPic.Visibility = LayoutVisibility.Never;
                    tabControlMain.TabPages.Remove(tabPage_Iv);
                }
            }
            //客检作业
            else if (checktype.Equals(CHECKTYPE.DATA_GROUP_CUSTOMERCHECK))
            {
                lblMenu.Text = "生产管理>过站管理>过站作业—客检";
                this.txtCode2.Visible = false;
                this.txtCode3.Visible = false;
                this.txtCode4.Visible = false;
                layoutControlItem_txtcode1.Text = "客户编码";
                layout_code2.Text = "生产编码";
                layoutControlItem_customercheck.Visibility = LayoutVisibility.Always;
                //layoutControlItem_elivcheck.Visibility = LayoutVisibility.Always;
                layoutControlItem_lvPic.Visibility = LayoutVisibility.Always;

                layout_code2.Visibility = LayoutVisibility.Always;
                layout_code3.Visibility = LayoutVisibility.Never;
                layout_code4.Visibility = LayoutVisibility.Never;
                layout_color.Visibility = LayoutVisibility.Never;
                layout_NP.Visibility = LayoutVisibility.Never;
                layout_proid.Visibility = LayoutVisibility.Never;
                layout_wo.Visibility = LayoutVisibility.Never;
                layout_powercheck.Visibility = LayoutVisibility.Never;


                sbtnNext.Text = "确定";
                luejudge01.Visible = false;
                lblLevel.Visible = false;
            }
        }
        /// <summary>
        /// 校验批次及其测试数据是否存在，获取批次对应的功率档位。
        /// </summary>
        /// <returns></returns>
        private bool IsUsableCode()
        {
            //获取批次数据、IV测试数据、功率档位
            Hashtable hs = new Hashtable();
            hs["flag"] = "custcheck";
            hs[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtCode1.Text.Trim();
            hs[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = _model.RoomKey;
            DataSet dsReturn = _lotAfterIvTestEntity.GetPalletOrLotData(hs);

            if (!string.IsNullOrEmpty(_lotAfterIvTestEntity.ErrorMsg))
            {
                MessageService.ShowError(_lotAfterIvTestEntity.ErrorMsg);
                txtPower.Text = string.Empty;
                return false;
            }
            //检查批次数据是否存在。
            DataTable dtPorlot = new DataTable();
            if (dsReturn.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME))
                dtPorlot = dsReturn.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];

            if (dtPorlot.Rows.Count < 1)
            {
                while (MessageBox.Show(string.Format("不存在序列号【{0}】批次数据,请确认。", txtCode1.Text.Trim()), "提示", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                txtPower.Text = string.Empty;
                return false;
            }
            //检查IV测试数据是否存在。
            if (dsReturn.Tables.Contains(WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME))
            {
                dtIvTest = dsReturn.Tables[WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME];
            }
            //检查IV测试数据。 并且测试数据<1
            if (this._isCheckIVTestdata && dtIvTest.Rows.Count < 1)
            {
                while (MessageBox.Show(string.Format("不存在序列号【{0}】测试数据,请确认。", txtCode1.Text.Trim()), "提示", MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    continue;
                }
                SetProLevelEmpty();
                this.luejudge01.Properties.DataSource = null;
                return false;
            }
            else if (dtIvTest.Rows.Count > 0)
            {
                txtPower.Text = Convert.ToString(dtIvTest.Rows[0][WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]);
                txtPower.Properties.ReadOnly = true;
            }
            //设置功率档位。
            if (dsReturn.Tables.Contains(BASE_POWERSET.DATABASE_TABLE_NAME)
                && dsReturn.Tables[BASE_POWERSET.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                _level = Convert.ToString(dsReturn.Tables[BASE_POWERSET.DATABASE_TABLE_NAME].Rows[0][BASE_POWERSET.FIELDS_PMAXSTAB]);
            }
            //检查IV测试数据。标称功率有错误给出提示。
            if (this._isCheckIVTestdata && string.IsNullOrEmpty(_level))
            {
                while (MessageBox.Show(string.Format("序列号【{0}】标称功率有误，请重打标签或联系工艺解决。", txtCode1.Text.Trim()), "提示", MessageBoxButtons.OKCancel,
                              MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;
            }
            luejudge01.Properties.ReadOnly = false;
            return true;
        }

        /// <summary>
        /// 对ELIV的检查进行处理
        /// </summary>
        private void IsCheckELIV()
        {
            string work_Order_Number = this.txtWorder.Text.Trim().ToString();

            LotAfterIvTestEntity lotAfterIVTest = new LotAfterIvTestEntity();

            DataSet dsAttr = lotAfterIVTest.GetOrderAttrByOrderNumber(work_Order_Number);

            DataRow[] drsEL = dsAttr.Tables[0].Select(string.Format(@" ATTRIBUTE_NAME = '{0}' ", "IsCheckEL"));

            DataRow[] drsIV = dsAttr.Tables[0].Select(string.Format(@" ATTRIBUTE_NAME = '{0}' ", "IsCheckIV"));

            string[] l_s01 = new string[] { "PIC_ADDRESS", "PIC_DATE_FORMAT", "PIC_FACTORY_CODE", "PIC_TYPE", "PIC_ADDRESS_NAME", "PIC_ISCHECK" };
            string category01 = "Uda_Pic_Address";
            DataTable dtPicAddress = BaseData.Get(l_s01, category01);

            if (drsEL.Length > 0)
            {
                this.isCheckELPic = bool.Parse(drsEL[0]["ATTRIBUTE_VALUE"].ToString());
            }
            else
            {
                DataRow[] drs01 = dtPicAddress.Select(string.Format("PIC_FACTORY_CODE='{0}' AND PIC_TYPE='{1}' ", _model.RoomName.ToUpper(), "EL"));

                foreach (DataRow dr in drs01)
                {
                    try
                    {
                        this.isCheckELPic = bool.Parse(Convert.ToString(dr["PIC_ISCHECK"]));
                    }
                    catch //(Exception ex)
                    {
                        while (MessageBox.Show("【红外图片显示控制需设置为：Bool型True/False】，请与系统管理员联系!", "提示", MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            continue;

                        return;
                    }
                }
            }

            if (drsIV.Length > 0)
            {
                this.isCheckIVPic = bool.Parse(drsIV[0]["ATTRIBUTE_VALUE"].ToString());
            }
            else
            {
                DataRow[] drs01 = dtPicAddress.Select(string.Format("PIC_FACTORY_CODE='{0}' AND PIC_TYPE='{1}' ", _model.RoomName.ToUpper(), "IV"));

                foreach (DataRow dr in drs01)
                {
                    try
                    {
                        this.isCheckIVPic = bool.Parse(Convert.ToString(dr["PIC_ISCHECK"]));
                    }
                    catch //(Exception ex)
                    {
                        while (MessageBox.Show("【IV曲线显示控制需设置为：Bool型True/False】，请与系统管理员联系!", "提示", MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            continue;

                        return;
                    }
                }
            }

        }
        /// <summary>
        /// 绑定图片地址
        /// </summary>
        private void BindLueLotPic()
        {
            //绑定图片地址作业
            string[] l_s01 = new string[] { "PIC_ADDRESS", "PIC_DATE_FORMAT", "PIC_FACTORY_CODE", "PIC_TYPE", "PIC_ADDRESS_NAME", "PIC_ISCHECK" };
            string category01 = "Uda_Pic_Address";
            DataTable dtPicAddress = BaseData.Get(l_s01, category01);
            DataTable dtEl = dtPicAddress.Clone();
            DataTable dtIv = dtPicAddress.Clone();
            DataTable dtELNG = dtPicAddress.Clone();
            dtEl.TableName = "EL";
            dtIv.TableName = "IV";
            dtELNG.TableName = "ELNG";

            //去除对工厂的限制 
            //DataRow[] drs01 = dtPicAddress.Select(string.Format("PIC_FACTORY_CODE='{0}' AND PIC_TYPE='{1}' ", _model.RoomName.ToUpper(), "EL"));

            DataRow[] drs01 = dtPicAddress.Select(string.Format(" PIC_TYPE IN ('{0}','{1}') AND PIC_FACTORY_CODE = '{2}'", "EL", "ELNG", _model.RoomName.ToString().Trim()));
            foreach (DataRow dr in drs01)
            {
                try
                {
                    if (this.isCheckELPic)
                        dtEl.ImportRow(dr);
                }
                catch //(Exception ex)
                {
                    while (MessageBox.Show("【红外图片显示控制需设置为：Bool型True/False】，请与系统管理员联系!", "提示", MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    return;
                }
            }
            dtEl.DefaultView.Sort = "PIC_TYPE DESC";
            //去除对工厂的限制
            //DataRow[] drs02 = dtPicAddress.Select(string.Format("PIC_FACTORY_CODE='{0}' AND PIC_TYPE='{1}' ", _model.RoomName.ToUpper(), "IV"));
            DataRow[] drs02 = dtPicAddress.Select(string.Format(" PIC_TYPE='{0}' AND PIC_FACTORY_CODE = '{1}'", "IV", _model.RoomName.ToString().Trim()));
            foreach (DataRow dr in drs02)
            {
                try
                {
                    if (this.isCheckIVPic)
                        dtIv.ImportRow(dr);
                }
                catch// (Exception ex)
                {
                    while (MessageBox.Show("【IV图片显示控制需设置为：Bool型True/False】，请与系统管理员联系!", "提示", MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    return;
                }
            }
            this.lueEl.Properties.DataSource = dtEl;
            if (dtEl.Rows.Count > 0)
                this.lueEl.ItemIndex = 0;

            this.lueIv.Properties.DataSource = dtIv;
            if (dtIv.Rows.Count > 0)
                this.lueIv.ItemIndex = 0;

        }
        /// <summary>
        /// 显示组件序列号EL和IV图片
        /// </summary>
        private void LoadLotPic(string pic, bool iselpic, string dateFormat)
        {
            try
            {
                bool isExistFieldFold = true;
                string picAddress = string.Empty;
                if (iselpic)
                {
                    picAddress = CombAddress(pic, iselpic, dateFormat, out isExistFieldFold);
                    if (!isExistFieldFold)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)lueEl.Properties.DataSource;
                        if (dt.Rows.Count - 1 > lueEl.ItemIndex)
                        {
                            lueEl.ItemIndex++;
                            return;
                        }
                        else
                        {
                            while (MessageBox.Show(string.Format("【图片路径】-{0}，不存在", picAddress), "提示", MessageBoxButtons.OKCancel,
                              MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                                continue;
                            SetProLevelEmpty();
                            return;
                        }
                    }
                    if (!File.Exists(picAddress))
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)lueEl.Properties.DataSource;
                        if (dt.Rows.Count - 1 > lueEl.ItemIndex)
                        {
                            lueEl.ItemIndex++;
                            return;
                        }
                        else
                        {
                            while (MessageBox.Show(string.Format("【EL图片名称{0}】不存在，请确认!", _model.LotNumber),
                                                   "提示", MessageBoxButtons.OKCancel,
                                                   MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            {
                                continue;
                            }
                            SetProLevelEmpty();
                        }
                        return;
                    }

                    Image cloneImage = Image.FromFile(picAddress);
                    Image imgel = new Bitmap(cloneImage);
                    cloneImage.Dispose();

                    this._pic_Title = picAddress + "---" + this._pic_Title;
                    this._pic_eladdress = picAddress;
                    this.lblPicAddressAndTime.Text = this._pic_Title;

                    _width = imgel.Width;
                    _height = imgel.Height;
                    pic_el.Image = imgel;
                    pic_el.Width = panel1.Width;
                    pic_el.Height = panel1.Height;

                    pic_el.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    picAddress = CombAddress(pic, iselpic, dateFormat, out isExistFieldFold);
                    if (!isExistFieldFold)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)lueIv.Properties.DataSource;
                        if (dt.Rows.Count - 1 > lueIv.ItemIndex)
                        {
                            lueIv.ItemIndex++;
                            return;
                        }
                        else
                        {
                            while (MessageBox.Show(string.Format("【图片路径】-{0}，不存在", picAddress), "提示", MessageBoxButtons.OKCancel,
                              MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                                continue;
                            SetProLevelEmpty();
                            return;
                        }
                    }
                    if (!File.Exists(picAddress))
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)lueIv.Properties.DataSource;
                        if (dt.Rows.Count - 1 > lueIv.ItemIndex)
                        {
                            lueIv.ItemIndex++;
                            return;
                        }
                        else
                        {
                            while (MessageBox.Show(string.Format("【IV图片{0}】不存在，请确认!", _model.LotNumber),
                                                    "提示", MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            {
                                continue;
                            }
                        }
                        return;
                    }

                    Image imgiv = Image.FromFile(picAddress);
                    this._pic_Title_iv = picAddress + "---" + this._pic_Title;
                    this._pic_lvaddress = picAddress;
                    this.lblPicAddressAndTime.Text = this._pic_Title;

                    _width_iv = imgiv.Width;
                    _height_iv = imgiv.Height;

                    pic_iv.Image = imgiv;
                    pic_iv.Width = panel2.Width;
                    pic_iv.Height = panel2.Height;

                    pic_iv.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(string.Format(ex.Message));
                SetProLevelEmpty();
                return;
            }

        }
        /// <summary>
        /// 返回图片地址
        /// </summary>
        /// <param name="address">配置的图片路径</param>
        /// <param name="factoryname">配置的图片路径—图片所在工厂</param>
        /// year—图片路径中的年
        /// month—图片路径中的月
        /// sdate—图片路径中的年月日
        /// lot_num—图片目录中的文件名称
        /// <returns>返回图片路径地址</returns>
        private string CombAddress(string address, bool iselpic, string dateFormat, out bool isExistFieldFold)
        {
            isExistFieldFold = true;
            string address_Return = string.Empty;
            if (dtIvTest == null
                || dtIvTest.Rows.Count < 1
                || string.IsNullOrEmpty(dtIvTest.Rows[0][WIP_IV_TEST_FIELDS.FIELDS_T_DATE].ToString()))
            {
                if (this._isCheckIVTestdata)
                {
                    while (MessageBox.Show(string.Format(@"未找到批次【{0}】测试数据!", _model.LotNumber), "提示", MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    {
                        continue;
                    }
                    isExistFieldFold = false;
                }
                return address_Return;
            }

            DateTime dtime = Convert.ToDateTime(dtIvTest.Rows[0][WIP_IV_TEST_FIELDS.FIELDS_TTIME]);
            string sdate = string.Empty;
            if (!string.IsNullOrEmpty(dateFormat))
            {
                sdate = dtime.ToString(dateFormat);
            }
            else
            {
                sdate = dtime.ToString("yyyy-M-d");
            }
            this._pic_Title = "测试时间:" + dtime.ToString("yyyy-M-d HH:mm:ss");

            _lotIvTestDate = dtime.ToString("yyMMdd");
            string month = dtime.Month.ToString() + DATETIME_CLASS.DATETIME_MONTH;
            string year = dtime.Year.ToString() + DATETIME_CLASS.DATETIME_YEAR;
            address_Return = address + "\\" + year + "\\" + month + "\\" + sdate;

            if (!Directory.Exists(address_Return))
            {
                if (this._isCheckIVTestdata)
                {
                    isExistFieldFold = false;
                }
                return address_Return;
            }
            address_Return = address + "\\" + year + "\\" + month + "\\" + sdate + "\\" + _model.LotNumber;

            if (iselpic)
                address_Return += ".jpg";
            else
                address_Return += ".gif";

            if (!File.Exists(address_Return))
            {
                if (this._isCheckIVTestdata)
                {
                    isExistFieldFold = false;
                }
                return address_Return;
            }

            return address_Return;
        }
        /// <summary>
        /// 清空等级判定
        /// </summary>
        private void SetProLevelEmpty()
        {
            this.luejudge01.ItemIndex = -1;
            this.luejudge01.Properties.DataSource = null;
        }

        private bool _isPaint = false;
        /// <summary>
        /// 如果图片不存在，就返回到主界面--未用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotDispatchForCustCheck_Paint(object sender, PaintEventArgs e)
        {
            if (!_isPaint)
            {
                if (!this._pic_eladdress.Equals(string.Empty))
                {
                    LotElPicForCustCheck elpicdialog = new LotElPicForCustCheck();
                    elpicdialog.lotnumber = _model.LotNumber;
                    elpicdialog.picel_address = this._pic_eladdress;
                    elpicdialog.picTitle = this._pic_Title;
                    if (DialogResult.OK == elpicdialog.ShowDialog())
                    {

                    }
                }
                _isPaint = true;
            }
        }
        /// <summary>
        /// 图片放大作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnMagnify_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(speSize.Text.Trim()) < 1)
            {
                MessageService.ShowMessage("请设定图片【放大范围】", "提示");
                return;
            }
            int width = 0, height = 0;
            if (pic_el.Image != null)
            {
                width = pic_el.Width + Convert.ToInt16(speSize.Text.Trim());
                height = pic_el.Height + Convert.ToInt16(speSize.Text.Trim());
                if (width > _width + 1000)
                {
                    MessageService.ShowMessage("EL图片尺寸【超出范围】", "提示");
                    return;
                }
                pic_el.Width = width;
                pic_el.Height = height;
            }

            if (pic_iv.Image != null)
            {
                width = pic_iv.Width + Convert.ToInt16(speSize.Text.Trim());
                height = pic_iv.Height + Convert.ToInt16(speSize.Text.Trim());
                if (width > _width + 1000)
                {
                    MessageService.ShowMessage("IV图片尺寸【超出范围】", "提示");
                    return;
                }
                pic_iv.Width = width;
                pic_iv.Height = height;
            }
        }
        /// <summary>
        /// 图片缩小作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnReduce_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(speSize.Text.Trim()) < 1)
            {
                MessageService.ShowMessage("请设定图片【缩小范围】", "提示");
                return;
            }
            int width = 0, height = 0;
            if (pic_el.Image != null)
            {
                width = pic_el.Width - Convert.ToInt16(speSize.Text.Trim());
                height = pic_el.Height - Convert.ToInt16(speSize.Text.Trim());
                if (width > _width)
                {
                    MessageService.ShowMessage("EL图片尺寸【最小范围】", "提示");
                    return;
                }
                pic_el.Width = width;
                pic_el.Height = height;
            }

            if (pic_iv.Image != null)
            {
                width = pic_iv.Width - Convert.ToInt16(speSize.Text.Trim());
                height = pic_iv.Height - Convert.ToInt16(speSize.Text.Trim());
                if (width > _width)
                {
                    MessageService.ShowMessage("IV图片尺寸【最小范围】", "提示");
                    return;
                }
                pic_iv.Width = width;
                pic_iv.Height = height;
            }
        }
        /// <summary>
        /// 适合尺寸/原始尺寸 调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup_EditValueChanged(object sender, EventArgs e)
        {
            SetElPic();

            SetIvPic();
        }
        /// <summary>
        /// 设定EL图片
        /// </summary>
        private void SetElPic()
        {
            //适合尺寸
            if (radioGroup.EditValue.ToString() == "0")
            {
                pic_el.Width = panel1.Width;
                pic_el.Height = panel1.Height;
            }
            //原始尺寸
            if (radioGroup.EditValue.ToString() == "1")
            {
                pic_el.Width = _width;
                pic_el.Height = _height;
            }
        }
        /// <summary>
        /// 设定IV图片
        /// </summary>
        private void SetIvPic()
        {
            //适合尺寸
            if (radioGroup.EditValue.ToString() == "0")
            {
                if (pic_iv.Image != null)
                {
                    pic_iv.Width = panel2.Width;
                    pic_iv.Height = panel2.Height;
                }
            }
            //原始尺寸
            if (radioGroup.EditValue.ToString() == "1")
            {
                if (pic_iv.Image != null)
                {
                    pic_iv.Width = _width_iv;
                    pic_iv.Height = _height_iv;
                }
            }
        }
        /// <summary>
        /// 编码1和编码2是否相等。
        /// </summary>
        bool isEqualscode1to2 = false;
        #region 比较条码刷入是否一致
        /// <summary>
        /// 比较编码2和编码1条码是否一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtCode2.Text.Trim()))
                {
                    txtCode2.SelectAll();
                    txtCode2.Focus();
                    isEqualscode1to2 = false;
                    return;
                }
                if (string.IsNullOrEmpty(txtCode1.Text.Trim()))
                {
                    while (MessageBox.Show("【组件编码】不能为空!",
                                            "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    txtCode2.SelectAll();
                    txtCode2.Focus();
                    isEqualscode1to2 = false;
                    return;
                }
                if (!IsValiedLotOrCustomerOrSideNo(txtCode1.Text.Trim(), txtCode2.Text.Trim()))
                {
                    txtCode2.SelectAll();
                    txtCode2.Focus();
                    isEqualscode1to2 = false;
                    return;
                }
                isEqualscode1to2 = true;
                if (checktype.Equals(CHECKTYPE.DATA_GROUP_CUSTOMERCHECK))
                {
                    sbtnNext.Focus();
                }
                else
                {
                    txtCode3.Focus();
                    txtCode3.SelectAll();
                }
            }
        }
        /// <summary>
        /// 比较编码3和编码1条码是否一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        /// <summary>
        /// 比较编码4和编码1条码是否一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode4_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtCode4.Text.Trim()))
                {
                    txtCode4.SelectAll();
                    txtCode4.Focus();
                    isEqualscode1to2 = false;
                    return;
                }

                if (string.IsNullOrEmpty(txtCode1.Text.Trim()))
                {
                    while (MessageBox.Show("【组件编码】不能为空!", "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    txtCode2.SelectAll();
                    txtCode2.Focus();
                    isEqualscode1to2 = false;
                    return;
                }
                if (!IsValiedLotOrCustomerOrSideNo(txtCode1.Text.Trim(), txtCode4.Text.Trim()))
                {
                    txtCode4.Focus();
                    txtCode4.Text = string.Empty;
                    isEqualscode1to2 = false;
                    return;
                }


                txtNp.Focus();
                txtNp.SelectAll();
                //txtColor.Focus();
                //txtColor.SelectAll();
            }
        }
        /// <summary>
        /// 校验编码是否有效
        /// </summary>
        /// <param name="code1"></param>
        /// <param name="code2"></param>
        /// <returns></returns>
        private bool IsValiedLotOrCustomerOrSideNo(string code1, string code2)
        {
            bool bl_bak = true;
            //判断批次是否合法    
            //DataSet dsParams = new DataSet();
            //Hashtable htParams = new Hashtable();
            //htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, code1);
            //htParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, _model.RoomKey);
            //htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);                //0:未暂停的批次；1：暂停的批次。
            //htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, "N");               //N：生产批次 L：组件补片批次
            //htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);        //0:未结束未删除 1：已结束 2：已删除
            //htParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, _model.OperationName);//工序名称

            //DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            //dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            //dsParams.Tables.Add(dtParams);
            DataSet dsReturn = _entity.GetLotInfo(code1);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                bl_bak = false;
            }
            if (!bl_bak) return bl_bak;
            if (dsReturn.Tables.Count < 1 || dsReturn.Tables[0].Rows.Count < 1)
            {
                while (MessageBox.Show(string.Format("未找到过站【批次{0}】,请确认!", code1),
                                        "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;
                bl_bak = false;
            }
            if (!bl_bak) return bl_bak;
            DataTable dtLot = dsReturn.Tables[0];
            var query = from v in dtLot.AsEnumerable()
                        where v.Field<string>(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE) == code2
                            || v.Field<string>(POR_LOT_FIELDS.FIELD_LOT_SIDECODE) == code2
                            || v.Field<string>(POR_LOT_FIELDS.FIELD_LOT_NUMBER) == code2
                        select v;
            try
            {
                DataTable dt_lot = query.CopyToDataTable<DataRow>();
                this._moduleColorFromCleanOpt = Convert.ToString(dt_lot.Rows[0][POR_LOT_FIELDS.FIELD_COLOR]);
            }
            //捕捉 InvalidOperationException异常
            catch //(InvalidOperationException ex)
            {
                //MessageService.ShowError(string.Format("条码【{0}】不正确，请确认!", code2));
                while (MessageBox.Show(string.Format("条码【{0}】不正确，请确认!", code2),
                                        "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                bl_bak = false;
            }

            return bl_bak;
        }
        #endregion
        /// <summary>
        /// EL图片值改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEl_EditValueChanged(object sender, EventArgs e)
        {
            string val = Convert.ToString(lueEl.EditValue);
            if (string.IsNullOrEmpty(val))
            {
                return;
            }
            DataRowView dr = this.lueEl.Properties.GetDataSourceRowByKeyValue(val) as DataRowView;
            string dateFormat = string.Empty;
            if (dr != null)
            {
                dateFormat = Convert.ToString(dr["PIC_DATE_FORMAT"]);
            }
            LoadLotPic(val, true, dateFormat);
            //有elng图片时显示该按钮，用于重命名elng图片  
            string _strBtnVisible = lueEl.Text.ToUpper();
            if (_strBtnVisible.Contains("ELNG"))
            {
                sbtOKELNG.Visible = true;
            }
            else
            {
                sbtOKELNG.Visible = false;
            }
        }
        /// <summary>
        /// IV图片值改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueIv_EditValueChanged(object sender, EventArgs e)
        {
            string val = Convert.ToString(lueIv.EditValue);
            if (string.IsNullOrEmpty(val))
            {
                return;
            }
            DataRowView dr = this.lueIv.Properties.GetDataSourceRowByKeyValue(val) as DataRowView;
            string dateFormat = string.Empty;
            if (dr != null)
            {
                dateFormat = Convert.ToString(dr["PIC_DATE_FORMAT"]);
            }
            LoadLotPic(val, false, dateFormat);
        }
        /// <summary>
        /// 保存之前需要做的数据校验
        /// </summary>
        /// <returns>true:有效数据,
        /// false:无效数据</returns>
        private bool IsValidData()
        {
            //检查组件编码
            if (string.IsNullOrEmpty(txtCode1.Text.Trim()))
            {
                while (MessageBox.Show("【组件编码】不能为空!", "提示",
                                       MessageBoxButtons.OKCancel,
                                       MessageBoxIcon.Error,
                                       MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                return false;
            }
            //检查组件编码1
            if (string.IsNullOrEmpty(txtCode2.Text.Trim()))
            {
                while (MessageBox.Show("【组件编码1】不能为空!", "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                return false;
            }
            //检查组件编码2
            if (string.IsNullOrEmpty(txtCode3.Text.Trim()))
            {
                while (MessageBox.Show("【组件编码2】不能为空!", "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;
                return false;
            }
            //检查花色
            if (string.IsNullOrEmpty(txtColor.Text.Trim()))
            {
                while (MessageBox.Show("【花色】不能为空!", "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                return false;
            }
            //检查等级。
            if (luejudge01.EditValue == null || string.IsNullOrEmpty(luejudge01.EditValue.ToString()))
            {
                while (MessageBox.Show("【等级判定】不能为空!", "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                return false;
            }

            //检查IV测试数据，检查功率。
            if (this._isCheckIVTestdata && string.IsNullOrEmpty(txtPower.Text.Trim()))
            {
                while (MessageBox.Show("【检验功率】不能为空!", "提示",
                                       MessageBoxButtons.OKCancel,
                                       MessageBoxIcon.Error,
                                       MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;
                return false;
            }
            //检查IV测试数据，检查EL图片。
            if (this.isCheckELPic)
            {
                if (this._isCheckIVTestdata && pic_el.Image == null)
                {
                    while (MessageBox.Show("【EL图片】不能为空!", "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;
                    return false;
                }
            }
            //检查IV测试数据，检查IV图片。
            if (this.isCheckIVPic)
            {
                if (this._isCheckIVTestdata && pic_iv.Image == null)
                {
                    while (MessageBox.Show("【IV图片】不能为空!", "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;
                    return false;
                }
            }
            //检查花色。
            string color = txtColor.Text.Trim();
            if (!hsColor.ContainsValue(color) && !hsColor.ContainsKey(color))
            {
                while (MessageBox.Show(string.Format("花色代码【{0}】不存在，请确认!", color), "提示",
                                       MessageBoxButtons.OKCancel,
                                       MessageBoxIcon.Error,
                                       MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;
                txtColor.Focus();
                txtColor.SelectAll();
                return false;
            }
            //根据输入的花色代码或名称设置花色名称。
            txtColor.Tag = null;
            foreach (DictionaryEntry k in hsColor)
            {
                if (color.Equals(k.Value))
                {
                    txtColor.Text = Convert.ToString(k.Key);
                    txtColor.Tag = k.Value;
                    break;
                }
                if (color.Equals(k.Key))
                {
                    txtColor.Tag = k.Value;
                    break;
                }
            }
            if (txtColor.Tag == null)
            {
                while (MessageBox.Show(string.Format("花色代码【{0}】不存在，请确认!", color), "提示",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Error,
                                                    MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;
                txtColor.Focus();
                txtColor.SelectAll();
                return false;
            }
            //检查在清洁工序是否必须输入花色。
            WorkOrders workordersEntity = new WorkOrders();
            Hashtable hsparams = new Hashtable();
            hsparams.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, txtWorder.Text.Trim());
            hsparams.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME, WORKORDER_SETTING_ATTRIBUTE.IsMustInputModuleColorByCleanOpt);
            DataSet dsWoAttribute = workordersEntity.GetWorkOrderAttributeValue(hsparams);
            if (!string.IsNullOrEmpty(workordersEntity.ErrorMsg))
            {
                MessageService.ShowError(workordersEntity.ErrorMsg);
                sbtnReturn.Focus();
                return false;
            }
            DataTable dtAttribute = dsWoAttribute.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];

            if (dtAttribute.Rows.Count > 0 && bool.Parse(Convert.ToString(dtAttribute.Rows[0][POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE])))
            {//清洁工序必须输入花色。
                if (string.IsNullOrEmpty(this._moduleColorFromCleanOpt))
                {//清洁工序没有输入花色。
                    while (MessageBox.Show(string.Format(@"未获取到【批次{0}】过清洁站判定的花色", _model.LotNumber),
                                           "提示",
                                           MessageBoxButtons.OKCancel,
                                           MessageBoxIcon.Error,
                                           MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;
                    sbtnReturn.Focus();
                    return false;
                }
                else if (!this._moduleColorFromCleanOpt.Equals(txtColor.Text.Trim())
                         && !this._moduleColorFromCleanOpt.Equals(Convert.ToString(txtColor.Tag)))
                {//清洁判定的花色和终检判断的花色不一致。
                    while (MessageBox.Show(string.Format(@"清洁判定的【花色{0}】与终检判定的【花色{1}】不一致，请确认!", _moduleColorFromCleanOpt, this.txtColor.Text.Trim()),
                                            "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;
                    sbtnReturn.Focus();
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 校验铭牌信息是否一致
        /// </summary>
        /// <returns></returns>
        private bool IsValiedNamePlate()
        {
            bool bl_bak = true;
            #region--针对漏失名牌的取消校验铭牌，当等级判定为NG(Repair),NG(Rework)时取消校验铭牌 
            string JudgeValue = string.Empty;
            JudgeValue = this.luejudge01.EditValue.ToString();
            string[] NotCheckNpGradeArray = new string[] { "Grade_NG_Repair", "Grade_NG_Rework" };
            foreach (string CheckJudgeValue in NotCheckNpGradeArray)
            {
                if (CheckJudgeValue == JudgeValue)
                {
                    _labelCheck = "0";
                }
            }
            #endregion
            //不检查IV测试数据，不需要校验铭牌。
            if (this._isCheckIVTestdata == false)
            {
                return bl_bak;
            }
            //1:表示校验铭牌信息
            if (_labelCheck.Equals("1"))
            {
                //铭牌条码
                string snp = txtNp.Text.Trim();

                if (string.IsNullOrEmpty(snp))
                {
                    while (MessageBox.Show("【铭牌信息】不能为空!",
                                           "提示",
                                           MessageBoxButtons.OKCancel,
                                           MessageBoxIcon.Error,
                                           MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    bl_bak = false;
                }

                #region 获取 _psSub_Code 信息的更新
                BasePowerSetEntity psEntity = new BasePowerSetEntity();
                DataSet dsPsSubCode = psEntity.GetPowerLevelByLotNum(_model.LotNumber);
                if (!string.IsNullOrEmpty(psEntity.ErrorMsg))
                {
                    MessageService.ShowError(psEntity.ErrorMsg);
                    return false;
                }
                DataTable dtSubCode = dsPsSubCode.Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
                if (dtSubCode.Rows.Count > 0)
                {
                    _psSub_Code = Convert.ToString(dtSubCode.Rows[0][BASE_POWERSET.FIELDS_PS_SUBCODE]);
                }
                #endregion

                //检验规则对应条码
                string sn = _model.LotNumber;
                string comn;
                comn = _proModel_Name_Prefix + _proModel_Name + _proModel_Name_Suffix + string.Format("{0}", _level) + _labelType + _lableVar;
                if (isChangePowerShow)
                {
                    comn = _proModel_Name_Prefix + _proModel_Name + _proModel_Name_Suffix + string.Format("{0}", strChangePowerShow) + _labelType + _lableVar;

                }

                //'21' + SN + "<FNC1>" + "11" + yymmdd + '94' + s_modulecode
                string conergy = "21" + _model.LotNumber + "<FNC1>11" + _lotIvTestDate + "94" + _psSub_Code;


                //判断铭牌检验状态是否为True
                if (bl_bak)
                {
                    //获取检验规则类型
                    DataSet dsLotCustCheckType = _entity.GetLotCustCheckType(sn);
                    if (!string.IsNullOrEmpty(_entity.ErrorMsg))
                    {
                        MessageService.ShowError(_entity.ErrorMsg);
                        return false;
                    }

                    //通过类型来进行比对
                    string custCheckType = string.Empty;

                    if (dsLotCustCheckType.Tables[0].Rows.Count > 0)
                    {
                        custCheckType = Convert.ToString(dsLotCustCheckType.Tables[0].Rows[0][0]);
                    }


                    //判断
                    switch (custCheckType)
                    {
                        case "0":
                            bl_bak = snp.Equals(sn);

                            //判断铭牌检查的结果为False的话弹窗进行提醒
                            if (!bl_bak)
                            {
                                while (MessageBox.Show(@"【铭牌信息】不一致，请确认。正确铭牌信息规则：
SN。",
                                                                                   "提示",
                                                                                   MessageBoxButtons.OKCancel,
                                                                                   MessageBoxIcon.Error,
                                                                                   MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                                    continue;
                            }
                            break;
                        case "1":
                            bl_bak = snp.Equals(comn);

                            //判断铭牌检查的结果为False的话弹窗进行提醒
                            if (!bl_bak)
                            {
                                while (MessageBox.Show(@"【铭牌信息】不一致，请确认。正确铭牌信息规则：
产品型号前缀+产品型号+产品型号后缀+功率档位+认证类型+版本号。",
                                                    "提示",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Error,
                                                    MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                                    continue;
                            }
                            break;
                        case "2":
                            bl_bak = snp.Equals(conergy);

                            //判断铭牌检查的结果为False的话弹窗进行提醒
                            if (!bl_bak)
                            {
                                while (MessageBox.Show(@"【铭牌信息】不一致，请确认。正确铭牌信息规则：
21+SN+<FUNC1>+11+yymmdd+94+主分档ArticleNo。",
                                                    "提示",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Error,
                                                    MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                                    continue;
                            }
                            break;
                        default:
                            bl_bak = snp.Equals(sn);
                            if (!bl_bak)
                            {
                                bl_bak = snp.Equals(comn);
                            }
                            if (!bl_bak)
                            {
                                bl_bak = snp.Equals(conergy);
                            }

                            //判断铭牌检查的结果为False的话弹窗进行提醒
                            if (!bl_bak)
                            {
                                while (MessageBox.Show(@"【铭牌信息】不一致，请确认。正确铭牌信息规则：
(1) SN
(2) 产品型号前缀+产品型号+产品型号后缀+功率档位+认证类型+版本号
(3) 21+SN+<FUNC1>+11+yymmdd+94+主分档ArticleNo。",
                                                    "提示",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Error,
                                                    MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                                    continue;
                            }
                            break;
                    }

                }


            }

            //韩苗、生产要求，对不检验铭牌的组件防止铭牌贴错进行卡控 
            else if (_labelCheck.Equals("0"))
            {
                if (!txtNp.Text.Equals(txtCode1.Text))
                {
                    MessageBox.Show("铭牌粘贴错误，请确认铭牌类型,不检验铭牌请扫描侧板标签");
                    txtNp.SelectAll();
                    txtNp.Focus();
                    bl_bak = false;
                }
            }




            return bl_bak;
        }

        private void sbtnNext_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                sbtnNext_Click(null, null);
            }
        }
        private void sbtnNext_Click(object sender, EventArgs e)
        {
            //体现功率档位绑定了工单物料和档位，铭牌编号与维护的体现档位不同，体现档位卡控 
            if (isEqualspowerPowerSubtoNp)
            {
                MessageBox.Show("铭牌的档位不是体现档位，请联系NPI");
                return;
            }
            //获取等级判定值
            luejudge01_EditValueChanged(sender, e);
            //比较编码1和编码2是否一致。
            txtCode2_KeyPress(null, new KeyPressEventArgs((char)(13)));
            //编码1和编码2不一致
            if (!isEqualscode1to2) return;




            //检查作业员信息。
            if (this.txtOperator.Text.Trim().Equals(string.Empty))
            {
                while (MessageBox.Show("【作业员】信息不能为空!",
                                        "提示",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    continue;

                txtOperator.Focus();
                return;
            }
            else
            {
                WIP_CUSTCHECK_FIELDS.TrackInOperation = txtOperator.Text.Trim();
            }
            string proId = string.Empty;    //新的产品ID。
            string partNumber = string.Empty;    //新的产品料号。
            bool bNeedReprint = false;      //是否需要重新打印标签。
            bool bReturn = false;           //不允许过站。
            //终检
            if (checktype.Equals(CHECKTYPE.DATA_GROUP_ENDCHECK))
            {
                //比较编码3和编码1是否一致。
                txtCode3_KeyPress(null, new KeyPressEventArgs((char)(13)));
                //编码3和编码1不一致。
                if (!isEqualscode1to2) return;
                //比较编码4和编码1是否一致。
                txtCode4_KeyPress(null, new KeyPressEventArgs((char)(13)));
                //编码4和编码1不一致。
                if (!isEqualscode1to2) return;
                //校验采集参数
                if (!IsValidData()) return;
                //校验铭牌编码
                if (!IsValiedNamePlate()) return;


                //需要检查IV测试数据，检查标签是否打印。
                if (this._isCheckIVTestdata)
                {
                    //检查标签是否打印。
                    bool bIsPrintLabel = Convert.ToString(dtIvTest.Rows[0]["VC_PSIGN"]) == "Y";
                    if (bIsPrintLabel == false)
                    {
                        while (MessageBox.Show(string.Format("组件（{0}）标签或铭牌未打印，请联系组件测试人员打印。", this.txtCode1.Text),
                                                "提示",
                                                MessageBoxButtons.OKCancel,
                                                MessageBoxIcon.Error,
                                                MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            continue;
                        return;
                    }
                }
                //根据当前产品ID、选择的等级判断是否符合当前产品的等级要求，并返回满足等级要求的第一个产品ID、等级及其对应料号。
                DataSet dsLotProductInfo = _lotAfterIvTestEntity.GetLotProductData(this.txtCode1.Text, Convert.ToString(this.luejudge01.EditValue));
                if (!string.IsNullOrEmpty(_lotAfterIvTestEntity.ErrorMsg))
                {
                    while (MessageBox.Show(_lotAfterIvTestEntity.ErrorMsg, "错误",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;
                    return;
                }
                string currentProId = this.txtPro_id.Text;
                string currentPartNumber = Convert.ToString(this.txtPro_id.Tag);
                proId = currentProId;
                partNumber = currentPartNumber;
                DataView dvLotProductInfo = dsLotProductInfo.Tables[0].DefaultView;
                if (!IsAutoUnload) //NG品不卡控
                    if (dvLotProductInfo.Count <= 0)
                    {
                        while (MessageBox.Show(string.Format(@"组件（{0}）指定等级（{1}）不符合工单产品要求，确定继续？
确定：将无法过站，需要按照计划人员安排操作（转工单或者暂扣终检区域）。
取消：重新选择等级。",
                                                this.txtCode1.Text, this.luejudge01.Text),
                                                "错误",
                                                MessageBoxButtons.OKCancel,
                                                MessageBoxIcon.Error,
                                                MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        {
                            return;
                        }
                        bNeedReprint = false;
                        bReturn = true;
                    }
                    else
                    {
                        dvLotProductInfo.Sort = "ITEM_NO ASC";
                        dvLotProductInfo.RowFilter = string.Format("PART_NUMBER='{0}'", currentPartNumber);
                        //返回的指定等级产品数据中不包含当前产品ID，需要换产品ID
                        if (dvLotProductInfo.Count == 0)
                        {
                            dvLotProductInfo.RowFilter = string.Empty;
                            if (MessageBox.Show(string.Format(@"组件（{0}）因设定等级不符合，将从产品（{1}，{2}）转为产品({3}，{4})。
确定：继续，需要重新打印标签后再次判定。
取消：重新选择等级。",
                                                                this.txtCode1.Text,
                                                                currentPartNumber,
                                                                currentProId,
                                                                dvLotProductInfo[0]["PART_NUMBER"],
                                                                dvLotProductInfo[0]["PRODUCT_CODE"]),
                                               "提示",
                                               MessageBoxButtons.OKCancel,
                                               MessageBoxIcon.Error,
                                               MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            {
                                return;
                            }
                            bNeedReprint = true;
                            bReturn = true;
                        }
                        proId = Convert.ToString(dvLotProductInfo[0]["PRODUCT_CODE"]);
                        partNumber = Convert.ToString(dvLotProductInfo[0]["PART_NUMBER"]);
                    }
            }
            DataSet dsSave = new DataSet();
            WIP_CUSTCHECK_FIELDS custcheck = new WIP_CUSTCHECK_FIELDS();
            DataTable dtCustCheck = FanHai.Hemera.Share.Common.CommonUtils.CreateDataTable(custcheck);
            if (!dtCustCheck.Columns.Contains(WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_KEY))
                dtCustCheck.Columns.Add(WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_KEY);
            if (!dtCustCheck.Columns.Contains(WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME))
                dtCustCheck.Columns.Add(WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME);

            DataRow drCustCheck_NewRow = dtCustCheck.NewRow();
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM] = _model.EquipmentKey;
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_TYPE] = string.Empty;
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1] = txtCode1.Text.Trim();
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE2] = txtCode2.Text.Trim();
            //终检
            if (checktype.Equals(CHECKTYPE.DATA_GROUP_ENDCHECK))
            {
                _model.ModuleGrade = Convert.ToString(luejudge01.EditValue);
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE3] = txtCode3.Text.Trim();
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE4] = txtCode4.Text.Trim();
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR] = txtColor.Text.Trim();
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_NAMEPLATENO] = txtNp.Text.Trim();
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER] = txtPower.Text.Trim();
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER] = txtWorder.Text.Trim();
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_LEVEL] = _model.ModuleGrade;
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CC_DATA_GROUP] = 1;

                //更新批次表中的产品ID和等级对应料号。
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_ID] = proId;
                dsSave.ExtendedProperties.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, partNumber);
            }
            else
            {
                drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CC_DATA_GROUP] = 2;
            }


            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY] = _model.RoomKey;
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_KEY] = _model.ShiftKey;
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME] = _model.ShiftName;

            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CUSTCHECK_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME] = DBNull.Value;//设置为以默认数据库时间填充
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_TIME] = DBNull.Value;
            drCustCheck_NewRow[WIP_CUSTCHECK_FIELDS.FIELDS_OPERATERS] = txtOperator.Text.Trim();

            dtCustCheck.Rows.Add(drCustCheck_NewRow);
            dsSave.Merge(dtCustCheck, true, MissingSchemaAction.Add);
            dsSave.ExtendedProperties.Add(CHECKTYPE.DATA_TYPE, checktype);
            dsSave.ExtendedProperties.Add("REPRINT", bNeedReprint);
            DataSet dsReturn = _lotAfterIvTestEntity.SaveLot2CustCheckData(dsSave);
            if (!string.IsNullOrEmpty(_lotAfterIvTestEntity.ErrorMsg))
            {
                MessageService.ShowError(_lotAfterIvTestEntity.ErrorMsg);
                return;
            }

            //终检 && 并且需要重新打印标签。或者等级不符合产品要求。
            if (checktype.Equals(CHECKTYPE.DATA_GROUP_ENDCHECK) && bReturn)
            {
                //如果当前产品ID != 符合产品ID
                //给出重打标签和铭牌提示，返回到上一个界面。
                sbtnReturn_Click(sender, e);
                return;
            }

            #region 虚拟托盘验证及分档数据封装
            //------------------To Change Begin------------------------------------------------------
            //------------------添加虚拟托盘---------------------------------------------------
            //------------------添加包装卡控---------------------------------------------------


            int _componentTrayCount = 0;  //托盘组件数量
            int _packageFullQty = 0;      //满包装托盘组件数量
            //string _TrayNumber = string.Empty; //托盘号
            bool bCheck = false;
            DataTable dtComponentTray = new DataTable();

            //获取当前组件批次数据。
            DataTable dtLotInfo = GetPackageLotInfo();
            if (dtLotInfo == null)
            {
                return;
            }
            //获取当前组件等级,花色,主\子分档信息
            GetMixPackage(dtLotInfo);
            //根据托盘号获取托盘信息
            //dtComponentTray = GetComponentTray(_model.TrayValue);
            //Jiabao线别获取修复异常
            dtComponentTray = GetComponentTrayLine(_model.TrayValue, _model.LineKey);
            //获取满托包装数
            _model.PackageNumber = GetPackageFullQty(_model.LotNumber);
            _packageFullQty = _model.PackageNumber;

            //判断托盘信息
            if (dtComponentTray != null && dtComponentTray.Rows.Count > 0 && !string.IsNullOrEmpty(dtComponentTray.Rows[0]["VirtualCustomerNumber"].ToString())) //如果有数据获取数据
            {
                string _strCount = string.Empty;
                _strCount = dtComponentTray.Rows[0]["Number"].ToString();
                int.TryParse(_strCount, out _componentTrayCount);

                if (_componentTrayCount < _model.PackageNumber)  //如果小于,验证包装逻辑
                {
                    //判断批次是否合法 && 检查批次工单
                    bCheck = CheckPackageLot(dtLotInfo) && CheckWorkOrderInfo(dtComponentTray);

                    if (!bCheck)
                    {
                        return;
                    }

                    _model.VirtualCustomerNumber = dtComponentTray.Rows[0]["VirtualCustomerNumber"].ToString(); //小于则继承原有虚拟托盘号
                    _model.Number = _componentTrayCount + 1;  //当前托盘数量加1;  
                    _model.IsFlip = "1";
                    //判断当前块数是否需要包护角
                    if (_model.Number % 2 == 1 || _model.Number == _model.PackageNumber) //判断当前托盘数是否为奇数或者是否为最后一块，是则包1，否则不包0,
                    {
                        _model.IsPack = "1";
                    }
                    else
                    {
                        _model.IsPack = "0";
                    }
                }
                else
                {
                    if (_componentTrayCount == _packageFullQty) //如果等于,新建托盘,第一块组件添加信息到虚拟托盘表
                    {
                        _model.VirtualCustomerNumber = GetShgCod("托盘号", true);
                        _model.Number = 1;
                        _model.IsFlip = "2";
                        _model.IsPack = "1";
                    }

                }
            }
            else //如果没有数据则新建托盘并查询组件信息
            {
                //新建托盘
                _model.VirtualCustomerNumber = GetShgCod("托盘号", true);
                _model.Number = 1;
                _model.IsFlip = "2";
                //第一块组件添加信息到虚拟托盘表
                _model.IsPack = "1";
            }

            //弹出框再次确认
            if (checktype.Equals(CHECKTYPE.DATA_GROUP_ENDCHECK) && IsAutoUnload)
            {
                if ((MessageBox.Show(string.Format("您选择的等级为:{0}", luejudge01.Text),
                                                 "提示",
                                                 MessageBoxButtons.OKCancel,
                                                 MessageBoxIcon.Information,
                                                 MessageBoxDefaultButton.Button2) == DialogResult.OK)
            )
                {
                    if (AutoUnloadProcess())
                        sbtnReturn_Click(sender, e);
                }
                return;
            }



            //------------------To Change End------------------------------------------------------
            #endregion

            DisposeImage();//释放图片资源

            //跳到下一个过账界面
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示工作站作业明细界面。
            LotDispatchDetailViewContent view = new LotDispatchDetailViewContent(_model, null);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 返回到工作站进站画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnReturn_Click(object sender, EventArgs e)
        {
            DisposeImage();
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示工作站作业界面。
            LotDispathViewContent view = new LotDispathViewContent(_model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 释放图片所占用内存资源
        /// </summary>
        private void DisposeImage()
        {
            if (this.pic_el.Image != null)
            {
                this.pic_el.Image.Dispose();
                this.pic_el.Image = null;
            }

            if (this.pic_iv.Image != null)
            {
                this.pic_iv.Image.Dispose();
                this.pic_iv.Image = null;
            }
        }
        /// <summary>
        /// 校验花色是否存在
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtColor.BackColor = Color.White;
            if (e.KeyChar == 13)
            {
                string color = txtColor.Text.Trim().ToUpper();
                //花色不能为空。
                if (string.IsNullOrEmpty(color))
                {
                    while (MessageBox.Show("【花色】不能为空",
                                           "提示",
                                           MessageBoxButtons.OKCancel,
                                           MessageBoxIcon.Error,
                                           MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    txtColor.Focus();
                    txtColor.SelectAll();
                    return;
                }
                //判断花色代码是否存在。
                //if (!hsColor.ContainsValue(color) && !hsColor.ContainsKey(color))
                if (!hsColor.ContainsValue(color))
                {
                    while (MessageBox.Show(string.Format("花色代码【{0}】不存在，请确认!", color),
                                          "提示",
                                          MessageBoxButtons.OKCancel,
                                          MessageBoxIcon.Error,
                                          MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    txtColor.Focus();
                    txtColor.SelectAll();
                    return;
                }
                DataSet dsReturn = _lotAfterIvTestEntity.GetKingLineInf(this.txtCode1.Text.ToString());
                if (dsReturn.Tables[0].Rows.Count <= 0)
                {

                    MessageBox.Show("未获取到该批次对应工单的金刚线维护，请火速联系工艺在产品Id中维护该信息内容！");
                    txtColor.SelectAll();
                    return;
                }
                //p判断是否是金刚线或黑硅片
                string jingLine = string.Empty;
                string isKingLine = dsReturn.Tables[0].Rows[0]["ISKINGLING"].ToString();
                if (isKingLine == "0")
                {
                    jingLine = "蓝";
                }
                else if (isKingLine == "1")
                {
                    jingLine = "花";
                }
                else if (isKingLine == "2")
                {
                    jingLine = "色";
                }


                //遍历颜色表。
                foreach (DictionaryEntry k in hsColor)
                {
                    if (color.Equals(k.Value))
                    {
                        txtColor.Text = Convert.ToString(k.Key) + jingLine;
                        txtColor.Tag = k.Value;
                        break;
                    }
                    //else if (color.Equals(k.Key.ToString()))
                    //{
                    //    txtColor.Text = txtColor.Text + jingLine;
                    //    txtColor.Tag = k.Value ;
                    //    break;
                    //}
                }

                txtCode4.Focus();
                txtCode4.SelectAll();
            }
        }

        public bool isEqualspowerPowerSubtoNp = false;//yibin.fei 2017.10.26
        private void txtNp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                isEqualspowerPowerSubtoNp = false;
                isChangePowerShow = false;
                strChangePowerShow = string.Empty;
                string strPowerSub = txtPowrName.Text;
                string strNp = txtNp.Text;
                int i = txtPowrName.Text.IndexOf(":");
                string strPowerName = txtPowrName.Text.Substring(i + 1, 3);

                //标签/包装清单打印体现功率 fyb
                try
                {
                    IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                    DataSet ds = _testDataEntity.GetIVTestData(_model.LotNumber);
                    string strWorkNumber = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                    string strSAP_NO = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                    DataSet ds_powershow = _testDataEntity.GetPowerShowData(strWorkNumber, strSAP_NO);
                    DataRow[] drPowerShow = ds_powershow.Tables[0].Select(string.Format("BEFORE_POWER={0}", strPowerName));
                    if (drPowerShow.Length > 0 && strPowerName == drPowerShow[0]["BEFORE_POWER"].ToString())
                    {
                        if (strPowerSub.Contains(drPowerShow[0]["BEFORE_POWER"].ToString()) && strNp.Contains(drPowerShow[0]["AFTER_POWER"].ToString()))
                        {
                            isEqualspowerPowerSubtoNp = false;
                            isChangePowerShow = true;
                            strChangePowerShow = drPowerShow[0]["AFTER_POWER"].ToString();

                        }
                        else
                        {
                            isEqualspowerPowerSubtoNp = true;
                        }
                    }
                    ds_powershow = null;
                    ds = null;
                    drPowerShow = null;
                }
                catch
                { }


                luejudge01.Focus();
            }
        }

        private void luejudge01_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                lueComponentTray.Focus();
            }
        }


        private void lueComponentTray_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                sbtnNext.Focus();
            }
        }

        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked == true)
            {
                this.layout_code2.Text = "客户编码";
                this.txtCode2.SelectAll();
                this.txtCode2.Focus();
            }
            if (chkCustomer.Checked == false)
            {
                this.layout_code2.Text = "生产编码";
                this.txtCode2.SelectAll();
                this.txtCode2.Focus();
            }
        }


        private void sbtOKELNG_Click_1(object sender, EventArgs e)
        {
            string picFileAddress = string.Empty;
            string pic = string.Empty;
            string picAddress = string.Empty;
            string newFtpFilePicAddress = string.Empty;
            string newFtpPicAddress = string.Empty;

            DateTime dtime = Convert.ToDateTime(dtIvTest.Rows[0][WIP_IV_TEST_FIELDS.FIELDS_TTIME]);
            string sdate = string.Empty;
            string month = dtime.Month.ToString() + DATETIME_CLASS.DATETIME_MONTH;
            string year = dtime.Year.ToString() + DATETIME_CLASS.DATETIME_YEAR;

            //绑定图片地址作业
            string[] l_s01 = new string[] { "PIC_ADDRESS", "PIC_DATE_FORMAT", "PIC_FACTORY_CODE", "PIC_TYPE", "PIC_ADDRESS_NAME", "PIC_ISCHECK" };
            string category01 = "Uda_Pic_Address";
            DataTable dtLocPicAddress = BaseData.Get(l_s01, category01);
            DataTable dtElngLoc = dtLocPicAddress.Clone();
            dtElngLoc.TableName = "ELNG";

            DataRow[] drs01 = dtLocPicAddress.Select(string.Format(" PIC_TYPE = '{0}' AND PIC_FACTORY_CODE = '{1}'", "ELNG", _model.RoomName.ToString().Trim()));
            foreach (DataRow dr in drs01)
            {
                try
                {
                    if (this.isCheckELPic)
                        dtElngLoc.ImportRow(dr);
                }
                catch //(Exception ex)
                {
                    while (MessageBox.Show("【红外图片显示控制需设置为：Bool型True/False】，请与系统管理员联系!", "提示", MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        continue;

                    return;
                }
            }
            if (!string.IsNullOrEmpty(dtElngLoc.Rows[0]["PIC_DATE_FORMAT"].ToString()))
            {
                sdate = dtime.ToString(dtElngLoc.Rows[0]["PIC_DATE_FORMAT"].ToString());
            }
            else
            {
                sdate = dtime.ToString("yyyy-M-d");
            }
            pic = dtElngLoc.Rows[0]["PIC_ADDRESS"].ToString();
            picFileAddress = pic + "\\" + year + "\\" + month + "\\" + sdate;
            picAddress = picFileAddress + "/" + _model.LotNumber + ".jpg";
            //-------------------------------------------------------------------------------------------------------------------
            string[] l_s02 = new string[] { "USERNAME", "PASSWORD", "FTPADDRESS", "FTPBAKADDRESS", "FACNAME" };
            string category02 = "FtpFileUpLode";
            DataTable dtFtpPicAddress = BaseData.Get(l_s02, category02);
            DataTable dtFtp = dtFtpPicAddress.Clone();
            dtFtp.TableName = "FTPINF";

            DataRow[] drs02 = dtFtpPicAddress.Select(string.Format(" FACNAME = '{0}'", _model.RoomName.ToUpper()));
            foreach (DataRow dr in drs02)
            {
                dtFtp.ImportRow(dr);
            }
            if (dtFtp.Rows.Count <= 0)
            {
                MessageBox.Show("没有设定基础数据指明FTP相关信息,请维护", "系统提示");
                return;
            }
            string userName = dtFtp.Rows[0]["USERNAME"].ToString();
            string passWord = dtFtp.Rows[0]["PASSWORD"].ToString();
            string ftpAddress = dtFtp.Rows[0]["FTPADDRESS"].ToString();
            string ftpBakAddress = dtFtp.Rows[0]["FTPBAKADDRESS"].ToString();
            newFtpFilePicAddress = ftpAddress + "\\" + year + "/" + month + "/" + sdate;
            newFtpPicAddress = newFtpFilePicAddress + "/" + _model.LotNumber + ".jpg";

            string newFtpBakFilePicAddress = ftpBakAddress + "\\" + year + "/" + month + "/" + sdate;
            string newFtpBakPicAddress = newFtpFilePicAddress + "/" + _model.LotNumber + ".jpg";
            //判断源文件是否存在？
            if (!File.Exists(picAddress))
            {
                while (MessageBox.Show(string.Format("【ELNG图片{0}】不存在，请确认!", _model.LotNumber),
                                                      "提示", MessageBoxButtons.OKCancel,
                                                      MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    continue;
                }
                return;
            }
            FtpFileHelper ftpFile = new FtpFileHelper(userName, passWord);
            //判定目标文件是否存在？
            //不存在就新建目标文件目录
            ftpFile.MakeDir(ftpBakAddress, "\\" + year + "/" + month + "/" + sdate);
            //有了目标文件目录就判定目标文件名是否重名
            string reFileName = ftpFile.ExistFile(picAddress, newFtpBakFilePicAddress);
            if (!string.IsNullOrEmpty(reFileName))
            {
                ftpFile.ReFileName(picAddress, newFtpBakFilePicAddress, reFileName);
            }
            //上传文件到目标文件目录下
            if (!ftpFile.UploadFile(picAddress, newFtpBakFilePicAddress))
            {
                return;
            }
            //删除源目录文件
            else
            {
                ftpFile.DeleteFile(newFtpPicAddress);

                this.sbtOKELNG.Visible = false;
                DataTable dtEl = dtLocPicAddress.Clone();
                dtEl.TableName = "EL";
                DataRow[] drs03 = dtLocPicAddress.Select(string.Format("PIC_TYPE ='{0}' AND PIC_FACTORY_CODE = '{1}'", "EL", _model.RoomName.ToString().Trim()));
                foreach (DataRow dr in drs03)
                {
                    try
                    {
                        dtEl.ImportRow(dr);
                    }
                    catch //(Exception ex)
                    {
                        while (MessageBox.Show("【红外图片显示控制需设置为：Bool型True/False】，请与系统管理员联系!", "提示", MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                            continue;

                        return;
                    }
                }
                lueEl.Properties.DataSource = dtEl;
                if (dtEl.Rows.Count > 0)
                    this.lueEl.ItemIndex = 0;
            }

        }

        //--------------------------------2019年1月15日--------------------
        //--------------------------------终检判断包装数据-----------------
        //--------------------------------wx-------------------------------
        #region 终检判断包装数据

        /// <summary>
        /// 检查批次数据。
        /// </summary>
        /// <returns></returns>
        private bool CheckPackageLot(DataTable dtLot)
        {
            DataRow drLot = dtLot.Rows[0];
            string curRoomKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            string curOperationName = Convert.ToString(drLot[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            int deleteFlag = Convert.ToInt32(drLot[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            int holdFlag = Convert.ToInt32(drLot[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            double quantity = Convert.ToDouble(drLot[POR_LOT_FIELDS.FIELD_QUANTITY]);
            double initQuantity = Convert.ToDouble(drLot[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
            string stateFlag = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            //批次状态为空。
            if (string.IsNullOrEmpty(stateFlag))
            {
                MessageService.ShowMessage("批次状态为空。", "提示");
                return false;
            }
            LotStateFlag lotStateFlag = (LotStateFlag)(Convert.ToInt32(stateFlag));

            //检查批次所在的工序是否是当前选定工序。
            if (curRoomKey != this._model.RoomKey || curOperationName != this._model.OperationName)
            {
                MessageService.ShowMessage("批次号在指定车间的工序中不存在。", "提示");
                return false;
            }
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);//获取有权限的所有工序
            //检查登录用户对批次所在的工序是否拥有权限。 
            if ((operations + ",").IndexOf(curOperationName + ",") == -1)
            {
                MessageService.ShowMessage("对不起，您没有权限操作", "提示");
                return false;
            }
            //判断批次是否结束或被删除。
            if (deleteFlag == 1 || deleteFlag == 2)
            {
                MessageService.ShowMessage("该批次已经结束或已删除", "提示");
                return false;
            }
            //判断批次是否暂停
            if (holdFlag == 1)
            {
                MessageService.ShowMessage("该批次已暂停。", "提示");
                return false;
            }
            //批次状态已完成
            if (lotStateFlag >= LotStateFlag.Finished)
            {
                MessageService.ShowMessage("该批次已完成。", "提示");
                return false;
            }
            //如果数量为空，则结束批次。
            if (quantity == 0)
            {
                _model.OperationType = LotOperationType.Terminal;
                TerminalLotDialog terminalLot = new TerminalLotDialog(_model);
                //显示结束批次的对话框。
                terminalLot.ShowDialog();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查批次工单。
        /// 检查工单，成品料号，花色，档位，等级，分档，子分档是否相同
        /// </summary>
        /// <param name="dtLot"></param>
        /// <param name="isbool">是否第一块</param>
        /// <returns></returns>
        private bool CheckWorkOrderInfo(DataTable dtLot)
        {
            DataRow drLot = dtLot.Rows[0];
            string orderNumber = Convert.ToString(drLot[POR_COMPONENT_TRAY_LIST_FIELDS.FIELD_WORK_ORDER_NO]);//托盘组件工单
            string partMumber = Convert.ToString(drLot[POR_COMPONENT_TRAY_LIST_FIELDS.FIELD_PATR_NUMBER]);   //成品料号
            string color = Convert.ToString(drLot[POR_COMPONENT_TRAY_LIST_FIELDS.FIELD_COLOR]);              //花色
            string Grade = Convert.ToString(drLot[POR_COMPONENT_TRAY_LIST_FIELDS.FIELD_GRADE_NAME]);  //等级
            string subPowerlevel = Convert.ToString(drLot[POR_COMPONENT_TRAY_LIST_FIELDS.FIELD_SUB_POWER_LEVEL]); //档位
            string ps_key = Convert.ToString(drLot[POR_COMPONENT_TRAY_LIST_FIELDS.FIELD_PS_KEY]);    //功率档

            //如果是缓存工位,不校验
            if (IsReceiveMixProIdByPackage)
            {
                return true;
            }

            //混工单不需要卡控
            ////检查混工单,不同工单不能混合包装
            //if (!orderNumber.Equals(_model.WorkOrderNo))
            //{
            //    MessageService.ShowError(string.Format("工单({0})不能混工单包装，组件({1})工单号({2})不一致。", orderNumber, _model.LotNumber, _model.WorkOrderNo));
            //    return false;
            //}

            //检查成品料号,不同料号不能混合包装
            if (!partMumber.Equals(_model.PatrNumber))
            {
                MessageService.ShowError(string.Format("组件（{0}）的产品料号（{1}）不符合。", _model.PatrNumber, _model.LotNumber));
                return false;
            }

            //检查花色,不同花色不能混合包装
            if (!color.Equals(_model.Color))
            {
                MessageService.ShowError(string.Format("组件【{0}】花色({1})不一致！", _model.LotNumber, _model.Color));
                return false;
            }

            if (!Grade.Equals(_model.GradeName))
            {
                MessageService.ShowError(string.Format("组件【{0}】等级({1})不符合混包要求！", _model.LotNumber, Grade));
                return false;
            }

            //不允许混档包装，并且分档类型不同，或者分档不同。给出提示。
            if (!ps_key.Equals(_model.PsKey))
            {
                MessageService.ShowError(string.Format("批次【{0}】功率档不在包装范围内！", _model.LotNumber));
                return false;
            }

            //不允许混子分档包装，并且当前组件子分档与包装子分档不一致。
            if (!subPowerlevel.Equals(_model.SubPowerlevel))
            {
                MessageService.ShowError(string.Format("批次【{0}】包装子分档({1})不在托范围内！", _model.LotNumber, subPowerlevel));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取等级、颜色、分档类型、档位、子分档
        /// </summary>
        private void GetMixPackage(DataTable dtLot)
        {
            DataRow drLot = dtLot.Rows[0];
            string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);

            //获取功率分档数据。
            DataSet dsPowersetData = this._lotEntity.GetLotPowersetData(lotNumber);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return;
            }
            if (dsPowersetData == null || dsPowersetData.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("批次【{0}】未获取包装档位数据！", lotNumber));
                return;
            }

            DataRow drPowersetData = dsPowersetData.Tables[0].Rows[0];

            _model.LotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            _model.GradeName = Convert.ToString(drLot["GRADE_NAME"]);
            _model.PatrNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
            _model.WorkOrderNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            _model.Color = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_COLOR]);
            _model.PsKey = Convert.ToString(drPowersetData[BASE_POWERSET.FIELDS_POWERSET_KEY]);
            _model.SubPowerlevel = Convert.ToString(drPowersetData[BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL]);

            if (string.IsNullOrEmpty(_model.PsKey))
            {
                MessageService.ShowError(string.Format("批次【{0}】未找到功率档数据！", lotNumber));
                return;
            }

        }

        /// <summary>
        /// 检查侧板标签或者客户标签。初始化批次号。
        /// </summary>
        /// <returns></returns>
        private DataTable GetPackageLotInfo()
        {
            string lotNum = _model.LotNumber;
            string msgTitle = string.Empty;
            Hashtable hsParams = new Hashtable();

            msgTitle = "组件序列号";
            hsParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNum);
            hsParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, this._model.RoomKey);

            //获取包装批次数据。
            DataSet dsLotNo = this._lotEntity.GetPackageLotInfo(hsParams);

            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return null;
            }
            DataTable dtLot = dsLotNo.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
            if (dtLot.Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("{1}【{0}】不存在，请确认", lotNum, msgTitle));
                return null;
            }
            else if (dtLot.Rows.Count > 1)
            {
                MessageService.ShowError(string.Format("{1}【{0}】大于1笔，不能入托，请确认", lotNum, msgTitle));
                return null;
            }
            return dtLot;
        }
        #endregion
        #region 托盘操作
        /// <summary>
        /// 获取托盘当前组件信息
        /// </summary>
        /// <param name="trayName"></param>
        /// <returns></returns>
        private DataTable GetComponentTray(string trayValue)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtReturn = new DataTable();

            dsReturn = _lotComponentTrayEntity.SelectComponentTray(trayValue); //获取托盘组件详细信息

            if (dsReturn != null && dsReturn.Tables[0] != null && dsReturn.Tables[0].Rows.Count > 0) //如果为不为空则赋值到table 
            {
                dtReturn = dsReturn.Tables[0];
            }

            return dtReturn;
        }

        /// <summary>
        /// 获取托盘当前组件信息
        /// </summary>
        /// <param name="trayName"></param>
        /// <returns></returns>
        private DataTable GetComponentTrayLine(string trayValue, string linekey)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtReturn = new DataTable();

            dsReturn = _lotComponentTrayEntity.SelectComponentTrayLine(trayValue, linekey); //获取托盘组件详细信息

            if (dsReturn != null && dsReturn.Tables[0] != null && dsReturn.Tables[0].Rows.Count > 0) //如果为不为空则赋值到table 
            {
                dtReturn = dsReturn.Tables[0];
            }

            return dtReturn;
        }

        /// <summary>
        /// 获取满托盘包装数量
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        private int GetPackageFullQty(string lotNumber)
        {
            return _lotEntity.GetPackageFullQty(lotNumber);
        }

        /// <summary>
        /// 生成新托盘号
        /// </summary>
        /// <param name="strTxt"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        private string GetShgCod(string strTxt, bool isAdd)
        {
            return _lotComponentTrayEntity.GetShgCod(strTxt, isAdd);
        }

        #endregion

        private void lueComponentTray_EditValueChanged(object sender, EventArgs e)
        {
            if (lueComponentTray.EditValue != null)
            {
                //取资料行，数据源为DataTable, 数据行是DataRowView对象。 
                object o = lueComponentTray.Properties.GetDataSourceRowByKeyValue(lueComponentTray.EditValue);

                if (o is DataRowView)
                {
                    DataRowView rv = o as DataRowView;
                    _model.TrayValue = lueComponentTray.EditValue.ToString();
                    _model.TrayText = lueComponentTray.Text.ToString();

                    IsReceiveMixProIdByPackage = Convert.ToBoolean(Convert.ToInt16(rv.Row["isbuffer"]));//加载档位属性 

                }
            }
            else
            {

            }
        }

        #region 
        private const string Column_name_Desc = "Column_name_Desc";
        private const string AutoUnload = "AutoUnload";
        private bool IsAutoUnload = false;
        private void luejudge01_EditValueChanged(object sender, EventArgs e)
        {
            IsAutoUnload = false;
            string GradeValue = string.Empty;
            if (luejudge01.EditValue != null)
            {
                //取资料行，数据源为DataTable, 数据行是DataRowView对象。 
                object o = luejudge01.Properties.GetDataSourceRowByKeyValue(luejudge01.EditValue);

                if (o is DataRowView)
                {
                    DataRowView rv = o as DataRowView;
                    IsAutoUnload = string.Compare(rv.Row[Column_name_Desc].ToString(), AutoUnload, true) == 0;
                }
            }
            GradeValue = luejudge01.EditValue.ToString();
            switch (GradeValue)
            {
                case "Grade_NG_Repair":
                    FillCodeAuto();
                    lueComponentTray.Text = "14-返修";
                    lueComponentTray.EditValue = "14";
                    break;
                case "Grade_NG_Rework":
                    FillCodeAuto();
                    lueComponentTray.Text = "7-返工";
                    lueComponentTray.EditValue = "7";
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 根据等级选择（NG(Rework),(NG(Repair))）时自动填充code代码和花色值，填充目的防止因为空值被MES档掉
        /// </summary>
        private void FillCodeAuto()
        {
            txtCode2.Text = txtCode1.Text.Trim();
            txtCode3.Text = txtCode1.Text.Trim();
            txtCode4.Text = txtCode1.Text;
            //txtColor.Text = "L";
        }

        private bool AutoUnloadProcess()
        {
            bool isBool = false;
            try
            {
                isBool = _lotComponentTrayEntity.InsertComponentTray(CopyModel());

                if (!isBool)
                {
                    MessageBox.Show("数据保存失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ce)
            {
                MessageBox.Show("数据保存失败:" + ce.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isBool;
        }

        private LotCustomerModel CopyModel()
        {
            LotCustomerModel constantsModel = new LotCustomerModel();

            constantsModel.TrayText = _model.TrayText;
            constantsModel.TrayValue = _model.TrayValue;
            constantsModel.LotNumber = _model.LotNumber;
            constantsModel.LineKey = _model.LineKey;
            constantsModel.LineName = _model.LineName;
            constantsModel.PackageNumber = _model.PackageNumber;
            constantsModel.Number = _model.Number;
            constantsModel.Color = _model.Color;
            constantsModel.PsKey = _model.PsKey;
            constantsModel.SubPowerlevel = _model.SubPowerlevel;
            constantsModel.WorkOrderNo = _model.WorkOrderNo;
            constantsModel.PatrNumber = _model.PatrNumber;
            constantsModel.GradeName = _model.GradeName;
            constantsModel.VirtualCustomerNumber = _model.VirtualCustomerNumber;
            constantsModel.IsFlip = _model.IsFlip;
            constantsModel.IsPack = _model.IsPack;
            return constantsModel;
        }
        #endregion


    }
}