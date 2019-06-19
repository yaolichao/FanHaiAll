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
using System.Threading;

using FanHai.Hemera.Addins.WIP.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotDispatchForPallet : BaseUserCtrl
    {
        LotOperationEntity _lotEntity = new LotOperationEntity();

        WorkOrderEntity workOrderEntity = new WorkOrderEntity();
        /// <summary>
        /// 存放包装数据。
        /// </summary>
        DataTable dtPackage = null;
        /// <summary>
        /// 存放包装明细数据。
        /// </summary>
        DataTable dtPackageDetail = null;
        /// <summary>
        /// 存放混包数据规则。
        /// </summary>
        DataTable dtPackageMixRule = null;
        /// <summary>
        /// 列名 GRADE_NAME
        /// </summary>
        private const string COLNAME_GRADE_NAME="GRADE_NAME";
        /// <summary>
        /// 列名 SEQ
        /// </summary>
        private const string COLNAME_SEQ = "SEQ";
        /// <summary>
        /// 存放工序主键。
        /// </summary>
        private string _operationKey = string.Empty;
        /// <summary>
        /// 是否检查IV测试数据。
        /// </summary>
        private bool _isCheckIVTestdata = false;
        /// <summary>
        /// 包装时间。
        /// </summary>
        private DateTime _packageTime = DateTime.Now;
        LotDispatchDetailModel _model = null;
        IViewContent _view = null; 
        /// <summary>
        /// 是否可以混工单包装(默认可以混工单包装)
        /// </summary>
        bool IsReceiveMixWosByPackage = true;
        /// <summary>
        /// 尾单通过设置可以进行混产品ID包装(默认不可以混产品ID包装)。
        /// 非尾单不可以进行混产品ID包装。
        /// </summary>
        bool IsReceiveMixProIdByPackage =false;
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotDispatchForPallet()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="view"></param>
        public LotDispatchForPallet(IViewContent view):this()
        {
            this._view = view;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="view"></param>
        /// <param name="model"></param>
        public LotDispatchForPallet(IViewContent view,LotDispatchDetailModel model):this(view)
        {
            _model = model;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotDispatchForPallet_Load(object sender, EventArgs e)
        {
            BindShift();
            SetIsMixProId();
            SetOperationKey();
            PriviledgeLastWo();
            InitControlsValue();
            //this.txtPalletNo.Select();
            //绑定线别，站别和设备
            lblLine.Text = this._model.LineName;
            lblWork.Text = this._model.OperationName;
            lblEquipment.Text = this._model.EquipmentName;
            this.txtLot_Num.Select();
            this.txtLot_Num.Focus();
            lblMenu.Text = "生产管理>过站管理>过站作业-包装";
        }
        /// <summary>
        /// 绑定班次数据。
        /// </summary>
        private void BindShift()
        {
            DataTable dtShift = BaseData.Get(new string[] { "CODE" }, BASEDATA_CATEGORY_NAME.Basic_Shift);
            this.cmbShift.Properties.Items.Clear();
            foreach (DataRow dr in dtShift.Rows)
            {
                this.cmbShift.Properties.Items.Add(dr["CODE"]);
            }
            this.cmbShift.Text = this._model.ShiftName;
        }
        /// <summary>
        /// 根据工序名称设置工序主键。
        /// </summary>
        private void SetOperationKey()
        {
            this._operationKey= this._lotEntity.GetOperationKey(this._model.OperationName);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
            }
        }
        /// <summary>
        /// 判断车间名称设定是否由系统自动生成托盘号。
        /// </summary>
        /// <param name="factoryRoom">车间名称</param>
        /// <returns>true:是 false：否 默认为否</returns>
        private bool IsAutoGeneratePalletNo(string roomName)
        {
            bool bAutoGeneratePalletNo = false;
            string[] columns = new string[] { "STATUS" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "PalletNo_CreateByAMES");
            KeyValuePair<string, string> condition = new KeyValuePair<string, string>("FACTARYROOM", roomName);
            List<KeyValuePair<string, string>> lstCondition = new List<KeyValuePair<string, string>>();
            lstCondition.Add(condition);
            
            DataTable dt = BaseData.GetBasicDataByCondition(columns, category, lstCondition);
            if (dt != null && dt.Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(dt.Rows[0]["STATUS"]), out bAutoGeneratePalletNo);
            }
            return bAutoGeneratePalletNo;
        }
        
        //---------------------------------------------------------------------------------------
        /// <summary>
        /// 尾单是否可以混产品ID包装。
        /// </summary>
        private void SetIsMixProId()
        {
            //是否可以混产品包装
            string[] l_mixproid = new string[] { "FACTORY_CODE", "IS_ValidData", "CONTROL_ITEM" };
            string category_proid = BASEDATA_CATEGORY_NAME.Basic_MixPackage_ByFactory;
            List<KeyValuePair<string, string>> lstConditions = new List<KeyValuePair<string, string>>();
            lstConditions.Add(new KeyValuePair<string, string>("FACTORY_CODE", _model.RoomName));
            lstConditions.Add(new KeyValuePair<string, string>("CONTROL_ITEM", "PRO_ID"));
            DataTable dtCommon2 = BaseData.GetBasicDataByCondition(l_mixproid, category_proid, lstConditions);
            if (dtCommon2 != null && dtCommon2.Rows.Count > 0)
            {
                string isValidData = Convert.ToString(dtCommon2.Rows[0]["IS_ValidData"]);
                if (!bool.TryParse(isValidData, out this.IsReceiveMixProIdByPackage))
                {
                    this.IsReceiveMixProIdByPackage = false;
                    MessageService.ShowError("基础数据表混包装需要设置为【true/false】,请与系统管理员联系");
                }
            }
        }
        /// <summary>
        /// 判断用户尾单权限。
        /// </summary>
        private void PriviledgeLastWo()
        {
            User user = new User();
            if (user.CheckLastPackageOperator())
            {
                layctrl_lastwo.Visibility = LayoutVisibility.Always;
            }
            else
            {
                layctrl_lastwo.Visibility = LayoutVisibility.Never;
            }
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlsValue()
        {
            this._packageTime = Utils.Common.Utils.GetCurrentDateTime();
            this.btnOK.Enabled = true;
            this.btnSave.Enabled = true;
            this.btnSingleOutPallet.Enabled = true;

            //this.tsbSave.Enabled = true;
            //this.tsbOK.Enabled = true;
            //this.tsbSingleOutPallet.Enabled = true;
            //this.cmbShift.Text =this._model.ShiftName;
            this.lblTotlePower.Text = string.Empty;
            this.txtLot_Num.Text = string.Empty;
            this.txtWo.Text = string.Empty;
            this.txtPro_ID.Text = string.Empty;
            this.txtAvgRange.Text = string.Empty;
            this.txtAvgPower.Text = string.Empty;
            this.txtEnterPallet.Text = string.Empty;
            this.txtFullPallet.Text = string.Empty;
            this.txtSapNo.Text = string.Empty;
            this.luePalletLevel.EditValue = string.Empty;
            this.lueQcLevel.EditValue = string.Empty;
            this.luePowerType.EditValue = string.Empty;
            this.luePowerSet.EditValue = string.Empty;
            this.luePalletLevel.EditValue = string.Empty;
            this.txtColor.Text = string.Empty;
            this.txtPalletNo.Properties.ReadOnly = false;
            this.luePalletPowerSet.Text = string.Empty;

            //this.chkLastPallet.CheckedChanged -= new EventHandler(chkLastPallet_CheckedChanged);
            //this.chkLastPallet.Checked = false;
            //this.chkLastPallet.CheckedChanged += new EventHandler(chkLastPallet_CheckedChanged);

            if (dtPackage != null)
            {
                dtPackage.Clear();
                dtPackage = null;
            }
            if (dtPackageDetail != null)
            {
                dtPackageDetail.Clear();
                dtPackageDetail = null;
            }
            if (dtPackageMixRule != null)
            {
                dtPackageMixRule.Clear();
                dtPackageMixRule=null;
            }
        }
        /// <summary>
        /// 拖盘号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            lock (this)
            {
                if (e.KeyChar != 13 || this.txtPalletNo.Properties.ReadOnly == true)
                {
                    return;
                }
                PalletNoCheck();
            }
        }
        /// <summary>
        /// 初始化包装数据
        /// </summary>
        private void PalletNoCheck()
        {
            if (!InitPackage())
            {
                InitControlsValue();
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }
            this.txtLot_Num.Select();
            this.txtLot_Num.SelectAll();

        }

        /// <summary>
        /// 根据托盘号初始化包装数据。
        /// </summary>
        /// <returns>true：初始化成功。false：初始化失败。</returns>
        private bool InitPackage()
        {
            string palletNo = this.txtPalletNo.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(palletNo))
            {//判定托号是否为空
                MessageService.ShowMessage("托盘号不能为空。", "提示");
                return false;
            }
            this.txtPalletNo.Text = palletNo;
            //如果是尾单托号，必须是以字母P或者字母M结尾
            if (this.chkLastPallet.Checked && !palletNo.EndsWith("P") && !palletNo.EndsWith("M"))
            {
                MessageService.ShowMessage("【尾单托号】必须以'P'或者'M'结尾!", "提示");
                return false;
            }
            //获取包装数据。
            DataSet dsReturn = this._lotEntity.GetPackageData(palletNo);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return false;
            }
            //查询包装数据出错
            if (dsReturn == null
                || dsReturn.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME) == false
                || dsReturn.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME) == false)
            {
                MessageService.ShowError(string.Format("查询包装数据出错，请重试", palletNo));
                return false;
            }

            dtPackage = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            //绑定明细数据。
            dtPackageDetail = dsReturn.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
            dtPackageDetail.PrimaryKey = new DataColumn[] { dtPackageDetail.Columns[POR_LOT_FIELDS.FIELD_LOT_NUMBER] };
            if (dtPackage.Rows.Count > 0)
            {//存在托盘数据，检查相关权限。
                DataRow drPackage = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME].Rows[0];
                int csDataGroup = Convert.ToInt32(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                string codeType = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE]);
                string lastPallet = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET]);
                string roomKey = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY]);
                if (!CheckDetail(palletNo, roomKey, lastPallet, codeType, csDataGroup))
                {
                    return false;
                }

                this.chkCusterCode.CheckedChanged -= new EventHandler(chkCusterCode_CheckedChanged);
                this.chkSideCode.CheckedChanged -= new EventHandler(chkSideCode_CheckedChanged);
                //检验类型。
                if (codeType.Equals("0"))
                {//组件序列号检验。
                    this.chkCusterCode.Checked = false;
                    this.chkSideCode.Checked = false;
                }
                else if (codeType.Equals("1"))
                {//侧板编码检验。
                    this.layout_lotTitle.Text = "侧板编码号";
                    this.chkCusterCode.Checked = false;
                    this.chkSideCode.Checked = true;
                }
                else if (codeType.Equals("2"))
                {//客户编码检验
                    this.layout_lotTitle.Text = "客户编码号";
                    this.chkCusterCode.Checked = true;
                    this.chkSideCode.Checked = false;
                }
                this.chkCusterCode.CheckedChanged += new EventHandler(chkCusterCode_CheckedChanged);
                this.chkSideCode.CheckedChanged += new EventHandler(chkSideCode_CheckedChanged);

                this.txtWo.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER]);
                this.txtPro_ID.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID]);
                string avgPowerRange = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE]);
                if (!string.IsNullOrEmpty(avgPowerRange))
                {
                    decimal avgMin = decimal.MinValue;
                    decimal avgMax = decimal.MaxValue;
                    string[] avgRanage = avgPowerRange.Split('~');
                    if (avgRanage.Length >= 1)
                    {
                        decimal.TryParse(avgRanage[0], out avgMin);
                    }
                    if (avgRanage.Length >= 2)
                    {
                        decimal.TryParse(avgRanage[1], out avgMax);
                    }
                    if (avgMin != decimal.MinValue || avgMax != decimal.MaxValue)
                    {
                        avgPowerRange = string.Format("{0}~{1}",
                                                      avgMin == decimal.MinValue ? "Min" : avgMin.ToString(),
                                                      avgMax == decimal.MinValue ? "Max" : avgMax.ToString());
                    }
                    this.txtAvgRange.Text = avgPowerRange;
                    this.txtAvgRange.Tag = new decimal[] { avgMin, avgMax };
                }
                this.txtSapNo.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO]);
                this.lblTotlePower.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER]);
                this.txtEnterPallet.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY]);
                string fullQty = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_FULL_QTY]);
                if (string.IsNullOrEmpty(fullQty)
                    && dtPackageDetail.Rows.Count > 0)
                {
                    string lotNumber = Convert.ToString(dtPackageDetail.Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                    fullQty = Convert.ToString(this._lotEntity.GetPackageFullQty(lotNumber));
                }
                this.txtFullPallet.Text = fullQty;

                this.txtAvgPower.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER]);
                this.cmbShift.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT]);
                this.txtColor.Text = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_COLOR]);
                //根据托等级查询等级名称。
                string proLevel = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                //根据等级值，查显示名称。
                string gradeName = this._lotEntity.GetGradeName(proLevel);
                this.luePalletLevel.Tag = proLevel;
                this.luePalletLevel.Text = gradeName;
                this.lueQcLevel.Tag = proLevel;
                this.lueQcLevel.Text = gradeName;
                //根据分档主键查询分档数据
                string psKey = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_SEQ]);
                string powerLevel = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL]);
                string psCode = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_CODE]);
                string psSubCode = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_DTL_SUBCODE]);
                DataSet ds = this._lotEntity.GetWOProductPowersetData(this.txtWo.Text, this.txtSapNo.Text, psKey);
                if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                {
                    MessageService.ShowError(this._lotEntity.ErrorMsg);
                    return false;
                }
                string psTypeName = psCode;
                string psName = powerLevel;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drPowersetData = ds.Tables[0].Rows[0];
                    psTypeName = string.Format("{0}:{1}", drPowersetData[BASE_POWERSET.FIELDS_PS_CODE], drPowersetData[BASE_POWERSET.FIELDS_PS_RULE]);
                    psName = string.Format("{0}:{1}", drPowersetData[BASE_POWERSET.FIELDS_PS_SEQ], drPowersetData[BASE_POWERSET.FIELDS_MODULE_NAME]);
                }

                //设置分档类型。
                luePowerType.Tag = psCode;
                luePowerType.Text = psTypeName;
                //设置分档。
                luePowerSet.Tag = psKey;
                luePowerSet.Text = psName;
                //设置子分档。
                luePalletPowerSet.Tag = powerLevel;
                luePalletPowerSet.Text = psSubCode;
            }
            else
            {//不存在托盘数据，新增一笔托盘数据。
                dtPackage.Rows.Add(dtPackage.NewRow());
            }
            this.gcConSigment.DataSource = dtPackageDetail;
            this.gvConSigment.BestFitColumns();
            //如果有一条包装明细数据，则托盘号不允许修改。
            if (dtPackageDetail.Rows.Count > 0)
            {
                this.txtPalletNo.Properties.ReadOnly = true;
            }
            return true;
        }

        private bool CheckDetail(string palletNo, string roomKey, string lastPallet, string codeType, int csDataGroup)
        {
            if (this._model.RoomKey != roomKey)
            {
                MessageService.ShowError(string.Format("托号【{0}】不在车间({1})，请确认。", palletNo, this._model.RoomName));
                return false;
            }
            //判断是否尾单 //是否有权限操作尾单
            if (lastPallet.Equals("1")
                && this.layctrl_lastwo.Visibility.Equals(LayoutVisibility.Never))
            {
                MessageService.ShowError(string.Format("托号【{0}】是尾单，没有权限操作，请确认并与相关人员联系!", palletNo));
                return false;
            }
            //判断是否E工单
            else if (lastPallet.Equals("2"))
            {
                MessageService.ShowError(string.Format("非法操作，【{0}】是E工单托，请确认并与相关人员联系!", palletNo));
                return false;
            }
            this.chkLastPallet.CheckedChanged -= new EventHandler(chkLastPallet_CheckedChanged);
            //尾单。
            if (!this.layctrl_lastwo.Visibility.Equals(LayoutVisibility.Never))
            {
                this.chkLastPallet.Checked = lastPallet.Equals("1");
            }
            this.chkLastPallet.CheckedChanged += new EventHandler(chkLastPallet_CheckedChanged);
            //检查包装状态。
            if (csDataGroup == 1)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经等待【入库检验】，请确认", palletNo));
                return false;
            }
            else if (csDataGroup == 10)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经从入库检返到【包装工序】，请出托检验!", palletNo));
                return false;
            }
            else if (csDataGroup == 2)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经等待【入库】，请确认", palletNo));
                return false;
            }
            else if (csDataGroup == 3)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经【入库】，请确认", palletNo));
                return false;
            }
            else if (csDataGroup == 4)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经【出货】，请确认", palletNo));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 组件序列号的回车事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLot_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtLot_Num.BackColor = Color.White;
            if (e.KeyChar == 13)
            {
                LotNumCheckAndSave();
            }
        }
        /// <summary>
        /// 检验批次是否有效并添加到包装明细列表中。
        /// </summary>
        public void LotNumCheckAndSave()
        {
            string lotNum=this.txtLot_Num.Text.Trim();
            if (string.IsNullOrEmpty(lotNum))
            {
                return;
            }
            string palletNo = txtPalletNo.Text.Trim();
            //托盘号为空
            if (string.IsNullOrEmpty(palletNo))
            {
               
                bool isAutoGeneratePalletNo = IsAutoGeneratePalletNo(this._model.RoomName);
                //判断是否启用自动生成托盘号。
                if (isAutoGeneratePalletNo == false)
                {
                    MessageBox.Show("托盘号不能为空！", "系统错误提示");
                    this.txtLot_Num.Text = string.Empty;
                    this.txtPalletNo.Select();
                    return;
                }
                //自动生成托盘号。

                //根据工单号判断是否为安能单 更改托号 yibin.fei 2017.11.23
                string nPalletNo = CreatePalletNo(lotNum);
                string WorkOrder = GetPackageLotInfo().Rows[0]["WORK_ORDER_NO"].ToString();
                DataSet dsAnNeng = this._lotEntity.GetAnNeng(WorkOrder);
                string AnNeng = string.Empty;
                if (dsAnNeng.Tables[0] != null && dsAnNeng.Tables[0].Rows.Count > 0)
                {
                    AnNeng = dsAnNeng.Tables[0].Rows[0]["ATTRIBUTE_VALUE"].ToString();


                    string[] l_s = new string[] { "PALLETNO_ADD", "NO" };
                    string category = "Basic_AnNengPalletNo";
                    System.Data.DataTable dt_PalletNo = BaseData.Get(l_s, category);
                    DataRow[] drPalletNo = dt_PalletNo.Select(string.Format("NO='{0}'", "1"));
                    string PalletNoADD = drPalletNo[0]["PALLETNO_ADD"].ToString().Trim();
                    nPalletNo = nPalletNo.Substring(0, nPalletNo.Length - 4) + AnNeng.ToUpper() + nPalletNo.Substring(nPalletNo.Length - 4, 4);

                }
                this.txtPalletNo.Text = nPalletNo;
                //yibin.fei 2017.11.23
                PalletNoCheck();
            }
            
           
          
            this._isCheckIVTestdata = this._lotEntity.IsCheckIVTestData(lotNum);
           // 包装数据没有初始化并且初始包装数据失败。
            if (dtPackage == null && InitPackage() == false)
            {
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }
            //进行组件序列号校验。
            if (!CheckValid())
            {
                this.txtLot_Num.SelectAll();
                this.txtLot_Num.Focus();
                return;
            }
            //获取批次数据。
            DataTable dtLotInfo = GetPackageLotInfo();
            if (dtLotInfo == null)
            {
                this.txtLot_Num.BackColor = Color.Red;
                this.txtLot_Num.SelectAll();
                this.txtLot_Num.Focus();
                return;
            }
            //新增包装明细数据
            DataRow drLotInfo = dtLotInfo.Rows[0];
            DataRow drNew = dtPackageDetail.NewRow();

            DataTable dtPack = gcConSigment.DataSource as DataTable;
            if (dtPack.Rows.Count == 0 && !this.chkLastPallet.Checked )
            {
                string gradename = drLotInfo[COLNAME_GRADE_NAME].ToString();
                string gradenameID = string.Empty;
                switch (gradename)
                {
                    case "客级":
                           gradenameID = "1";
                        break;
                    case "CA":
                        gradenameID = "2";
                        break;
                    case "A":
                        gradenameID = "3";
                        break;
                    case "A02":
                        gradenameID = "4";
                        break;
                    case "二级品(性能)":
                        gradenameID = "5";
                        break;
                    case "二级品(外观)":
                        gradenameID = "5";
                        break;
                    case "三级品(性能)":
                        gradenameID = "5";
                        break;
                    case "三级品(外观)":
                        gradenameID = "5";
                        break;
                    case "R级": 
                        gradenameID = "5";
                        break;
                    case "A01": 
                        gradenameID = "7";
                        break;
                    case "P": 
                        gradenameID = "8";
                        break;
                    case "C": 
                        gradenameID = "9";
                        break;

                }
                if (!string.IsNullOrEmpty(gradenameID))
                {
                    palletNo = txtPalletNo.Text;
                    if (palletNo.Substring(10, 1) != gradenameID)
                    {
                        palletNo = txtPalletNo.Text;
                        txtPalletNo.Text = palletNo.Replace(palletNo.Substring(10, 1), gradenameID);
                        palletNo = palletNo.Replace(palletNo.Substring(10, 1), gradenameID);
                    }
                }
            }


            drNew[POR_LOT_FIELDS.FIELD_LOT_KEY] = drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY];
            drNew[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = drLotInfo[POR_LOT_FIELDS.FIELD_LOT_NUMBER];
            drNew[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO] = drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO];
            drNew[POR_LOT_FIELDS.FIELD_PRO_ID] = drLotInfo[POR_LOT_FIELDS.FIELD_PRO_ID];
            drNew[POR_LOT_FIELDS.FIELD_PART_NUMBER] = drLotInfo[POR_LOT_FIELDS.FIELD_PART_NUMBER];
            drNew[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE] = drLotInfo[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE];
            drNew[POR_LOT_FIELDS.FIELD_LOT_SIDECODE] = drLotInfo[POR_LOT_FIELDS.FIELD_LOT_SIDECODE];
            drNew[POR_LOT_FIELDS.FIELD_EDIT_TIME] = drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME];
            drNew[COLNAME_GRADE_NAME] = drLotInfo[COLNAME_GRADE_NAME];
            drNew[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] = drLotInfo[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX];
            drNew[POR_LOT_FIELDS.FIELD_PRO_LEVEL] = drLotInfo[POR_LOT_FIELDS.FIELD_PRO_LEVEL];
            drNew[POR_LOT_FIELDS.FIELD_COLOR] = drLotInfo[POR_LOT_FIELDS.FIELD_COLOR];

            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = cmbShift.Text;
            drNew[POR_LOT_FIELDS.FIELD_PALLET_NO] = this.txtPalletNo.Text;
            drNew[POR_LOT_FIELDS.FIELD_PALLET_TIME] = this._packageTime;
            drNew[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME] = this._model.EquipmentName;
            //检查批次数据。
            bool bCheck = CheckPackageLot(dtLotInfo)                   //判断批次是否合法
                          && CheckWorkOrderNumber(dtLotInfo)           //检查批次工单
                          && CheckProductId(dtLotInfo)                 //检查批次产品ID
                          && CheckPartNumber(dtLotInfo)                //检验产品料号
                          && CheckFullPalletQty(dtLotInfo, ref drNew)             //检查满托数量
                          && CheckAvgPower(dtLotInfo, ref drNew)                   //检验平均功率
                          && CheckMixPackage(dtLotInfo, ref drNew);            //检验混包规则，是否混花，混档，混等级判定 校验终检等级，花色
            this.gcConSigment.DataSource = dtPackageDetail;
            if (!bCheck)
            {
                this.txtLot_Num.BackColor = Color.Red;
                this.txtLot_Num.SelectAll();
                this.txtLot_Num.Focus();
                return;
            }
            dtPackageDetail.Rows.Add(drNew);
            //计算包装数据。
            CalculatePackageData();
            gvConSigment.BestFitColumns();

            //判断等级是否一样
            if (!GradeIsSame())
            {
                palletNo = txtPalletNo.Text;
                txtPalletNo.Text = palletNo.Replace(palletNo.Substring(10,1),"6");
            }
            
            //非尾单且已包满自动保存过站。
            if (!this.chkLastPallet.Checked  
                && Convert.ToInt32(this.txtFullPallet.Text) == gvConSigment.DataRowCount)
            {
                SavePallet(1);
            }
            txtLot_Num.Focus();
            txtLot_Num.SelectAll();
        }
 
        /// <summary>
        /// 创建托盘号。
        /// </summary>
        /// <returns></returns>
        private string CreatePalletNo(string lotNum)
        {
            DataSet dsReturnP = new DataSet();
            //根据组件序列号判断创建托盘规则。
            //bool isSELot = this._lotEntity.GetOutLotForSe(lotNum);
            //EnumPalletNoRule rule = isSELot ? EnumPalletNoRule.SE : EnumPalletNoRule.Normal;
            //生成托盘号。
            //string palletNo=string.Empty;
            //switch (rule)
            //{
            //    case EnumPalletNoRule.SE:
            //        //se客户按se的生产托盘号
            //        //自动生成托号  CHYYWWXXXX
            //        //CH为chint  YY 年份2014 就为 14  WW周 第几周  XXXX 流水号
            //        dsReturnP = this._lotEntity.NewSEPalletNo(lotNum);
            //        if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            //        {
            //            MessageBox.Show(this._lotEntity.ErrorMsg, "系统错误提示");
            //            break;
            //        }
            //        palletNo = Convert.ToString(dsReturnP.Tables["RETURN"].Rows[0]["PALLETNO"]);
            //        this.txtPalletNo.Properties.ReadOnly = true;
            //        break;
            //    case EnumPalletNoRule.Normal:
            //    default:
            //        //自动生成托号
            //        //主副产品料号1位（主产品为0 第一副产品为1 第二副产品为2 依次类推）
            //        //如 001-210000189-001 产品编码号 + 柜号 + - + 工单号 + - + 流水码
            //        //后台处理得到新的托盘编码
            //        dsReturnP = this._lotEntity.NewPalletNo(lotNum);
            //        if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            //        {
            //            MessageBox.Show(this._lotEntity.ErrorMsg, "系统错误提示");
            //            break;
            //        }
            //        palletNo =Convert.ToString(dsReturnP.Tables["RETURN"].Rows[0]["PALLETNO"]);
            //        break;
            //}

            //工单号(9位)-等级代码(1位)-流水号(4位)
            string palletNo = this._lotEntity.GetNewPalletNum(lotNum);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageBox.Show(this._lotEntity.ErrorMsg, "系统错误提示");
                
            }
           
             return palletNo;
            
        }
      

        /// <summary>
        /// 判断同一托盘内是否同等级 add by ruhu 
        /// </summary>
        /// <returns></returns>
        public bool GradeIsSame()
        {
            DataTable dtPack = gcConSigment.DataSource as DataTable;
            string grade = lueQcLevel.Text.Trim();
            foreach (DataRow row in dtPack.Rows)
            {
                if (grade != row["GRADE_NAME"].ToString().Trim())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检查输入的值的有效性。
        /// </summary>
        /// <returns>true:有效；false：无效。</returns>
        private bool CheckValid()
        {
            string lotNum = this.txtLot_Num.Text.Trim();
            //校验客户编码
            if (this.chkCusterCode.Checked)
            {
                DataRow[] drs = dtPackageDetail.Select(string.Format(@"LOT_CUSTOMERCODE='{0}'", lotNum));
                if (drs != null && drs.Length > 0)
                {
                    MessageService.ShowError(string.Format("【客户编码{0}】已在【托号{1}】中", lotNum, this.txtPalletNo.Text));
                    return false;
                }
            }
            //校验侧板编码
            else if (this.chkSideCode.Checked)
            {
                DataRow[] drs = dtPackageDetail.Select(string.Format(@"LOT_SIDECODE='{0}'", lotNum));
                if (drs != null && drs.Length > 0)
                {
                    MessageService.ShowError(string.Format("【侧板编码{0}】已在【托号{1}】中", lotNum, this.txtPalletNo.Text));
                    return false;
                }
            }
            //判断批次是否已经在包装中
            else
            {
                DataRow[] drs = dtPackageDetail.Select(string.Format(@"LOT_NUMBER='{0}'", lotNum));
                if (drs != null && drs.Length > 0)
                {
                    MessageService.ShowError(string.Format("【批次{0}】已在【托号{1}】中", lotNum, this.txtPalletNo.Text));
                    return false;
                }
            }
            //非尾单。检查已入托数是否和满托数一致。
            if (!this.chkLastPallet.Checked
                && !string.IsNullOrEmpty(txtFullPallet.Text)
                && Convert.ToInt32(txtFullPallet.Text)==gvConSigment.RowCount)
            {
                MessageService.ShowError(string.Format("【托号{0}】已经包满，请保存", txtPalletNo.Text.Trim()));
                return false;
            }

            if (dtPackageDetail.Rows.Count > 0)
            {
                bool isSELot = this._lotEntity.GetOutLotForSe(lotNum);
                string palletFirstLotNo = Convert.ToString(dtPackageDetail.Rows[0]["LOT_NUMBER"]);
                bool isSEPallet = this._lotEntity.GetOutLotForSe(palletFirstLotNo);
                //组件不是SE组件 但托盘是SE托盘，给出提示。
                if (isSELot == false && isSEPallet == true)
                {
                    MessageBox.Show("该组件不是SE客户，不能和目前的混包！", "系统错误提示");
                    return false;
                }
                //组件是iSE组件，但托盘不是SE托盘，给出提示。
                else if (isSELot == true && isSEPallet == false)
                {
                    MessageBox.Show("该组件为SE客户不能和其他混包！", "系统错误提示");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查侧板标签或者客户标签。初始化批次号。
        /// </summary>
        /// <returns></returns>
        private DataTable GetPackageLotInfo()
        {
            string lotNum = this.txtLot_Num.Text.Trim();
            string msgTitle = string.Empty;
            Hashtable hsParams = new Hashtable();
            if (this.chkCusterCode.Checked)
            {
                msgTitle = "客户编码";
                hsParams.Add(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE, lotNum);

            }
            else if (this.chkSideCode.Checked)
            {
                msgTitle = "侧板编码";
                hsParams.Add(POR_LOT_FIELDS.FIELD_LOT_SIDECODE, lotNum);
            }
            else
            {
                msgTitle = "组件序列号";
                hsParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNum);
            }

            hsParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY,this._model.RoomKey);
            //获取包装批次数据。
            DataSet dsLotNo= this._lotEntity.GetPackageLotInfo(hsParams);
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
            //检查批次托盘号。
            string lotPalletNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            if (!string.IsNullOrEmpty(lotPalletNo)
                && lotPalletNo!=this.txtPalletNo.Text.Trim())
            {
                MessageService.ShowMessage(string.Format("该批次已包装到托盘({0})，请确认。",lotPalletNo), "提示");
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
        /// 检查批次工单。如果是本托第一块组件，则将工单设置为包装工单。
        /// </summary>
        /// <param name="dtLot"></param>
        /// <returns></returns>
        private bool CheckWorkOrderNumber(DataTable dtLot)
        {
            DataRow drLot = dtLot.Rows[0];
            string orderNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            //本拖第一块组件，设置当前包装工单,设置包装工单是否可以混工单。
            if (this.gvConSigment.RowCount < 1)
            {
                this.txtWo.Text = orderNumber;
                //判断工单设置，是否可以进行混工单包装。
                WorkOrders workordersEntity = new WorkOrders();
                Hashtable hsparams = new Hashtable();
                hsparams.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, orderNumber);
                hsparams.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME, WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage);
                DataSet dsWoAttribute = workordersEntity.GetWorkOrderAttributeValue(hsparams);
                if (!string.IsNullOrEmpty(workordersEntity.ErrorMsg))
                {
                    MessageService.ShowError(workordersEntity.ErrorMsg);
                    return false;
                }
                DataTable dtAttribute = dsWoAttribute.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                if (dtAttribute.Rows.Count > 0)
                {
                    this.IsReceiveMixWosByPackage = Convert.ToBoolean(dtAttribute.Rows[0][POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]);
                }
            }
            bool isMixWoByPackage = true;
            //当前工单和包装工单不符合。判断当前批次工单是否可以混工单。
            if (orderNumber != this.txtWo.Text)
            {
                //判断工单设置，是否可以进行混工单包装。
                WorkOrders workordersEntity = new WorkOrders();
                Hashtable hsparams = new Hashtable();
                hsparams.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, orderNumber);
                hsparams.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME, WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage);
                DataSet dsWoAttribute = workordersEntity.GetWorkOrderAttributeValue(hsparams);
                if (!string.IsNullOrEmpty(workordersEntity.ErrorMsg))
                {
                    MessageService.ShowError(workordersEntity.ErrorMsg);
                    return false;
                }
                DataTable dtAttribute = dsWoAttribute.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                if (dtAttribute.Rows.Count > 0)
                {
                    isMixWoByPackage = Convert.ToBoolean(dtAttribute.Rows[0][POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]);
                }
            }
            string packageOrderNumber = this.txtWo.Text;
            //包装工单不可以混工单包装或者当前批次工单不可以混工单，进行工单判断。
            if ((this.IsReceiveMixWosByPackage==false || isMixWoByPackage==false) && !orderNumber.Equals(packageOrderNumber))
            {//工单和包装工单不一致。
                if (this.IsReceiveMixWosByPackage == false)
                {
                    MessageService.ShowError(string.Format("工单({0})不能混工单包装，组件({1})工单号({2})不一致。",
                                                            packageOrderNumber,
                                                            drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER],
                                                            orderNumber));
                }
                else
                {
                    MessageService.ShowError(string.Format("组件({0})工单号({1})不能混工单包装，和包装工单（{2}）不一致。",
                                                            drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER],
                                                            orderNumber,
                                                            packageOrderNumber));
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查批次产品ID。如果是本托第一块组件，则将产品ID设置为包装产品ID。
        /// </summary>
        /// <param name="dtLot"></param>
        /// <returns></returns>
        private bool CheckProductId(DataTable dtLot)
        {
            DataRow drLot = dtLot.Rows[0];
            string proId = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PRO_ID]);
            //本拖第一块组件，设置包装产品ID。
            if (this.gvConSigment.RowCount<1)
            {
                this.txtPro_ID.Text = proId;
            }
            string packageProId = this.txtPro_ID.Text;
            if (this.chkLastPallet.Checked)
            {
                //检查是否可以混产品ID包装。
                if (!this.IsReceiveMixProIdByPackage && !proId.Equals(packageProId))
                {
                    MessageService.ShowError(string.Format("产品（{0}）尾单不可以混包，组件（{1}）产品{2} 不一致！",
                                                           packageProId,
                                                           drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER],
                                                           proId));
                    return false;
                }
            }
            else
            {//不是尾单。
                if (!proId.Equals(packageProId))
                {
                    MessageService.ShowError(string.Format("产品（{0}）不可以混包，组件（{1}）产品{2} 不一致！",
                                                          packageProId,
                                                          drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER],
                                                          proId));
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查批次满托数量。如果是本托第一块组件，则将满托数量设置为包装满托数量。
        /// </summary>
        /// <returns></returns>
        private bool CheckFullPalletQty(DataTable dtLot,ref DataRow drPackageDetail)
        {
            string lotNumber = Convert.ToString(dtLot.Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            //获取批次满托数量。
            int nFullQty = this._lotEntity.GetPackageFullQty(lotNumber);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return false;
            }
            if (nFullQty == 0)
            {
                MessageService.ShowError(string.Format("批次【{0}】未获取到满托数量！", lotNumber));
                return false;
            }
            //本托第一块组件,设置包装的满托数量。
            if (this.gvConSigment.RowCount < 1)
            {
                this.txtFullPallet.Text = nFullQty.ToString();
            }
            int packageFullQty = Convert.ToInt32(this.txtFullPallet.Text);
            //尾单不进行处理。
            //如果不是尾单。检查满托数量是否一致，如果不一致给出提示。
            if (!this.chkLastPallet.Checked && !nFullQty.Equals(packageFullQty))
            {
                MessageService.ShowError(string.Format("组件【{0}】满托数量（{1}）不一致！", lotNumber,nFullQty));
                return false;
            }
            //包装明细的满托数量。
            drPackageDetail[WIP_CONSIGNMENT_FIELDS.FIELDS_FULL_QTY] = nFullQty;
            return true;
        }
        /// <summary>
        /// 检查成品料号。如果是本托第一块组件，则将成品料号设置为包装料号。
        /// </summary>
        /// <returns>true：成功。false：失败。</returns>
        private bool CheckPartNumber(DataTable dtLot)
        {
            DataRow drLot = dtLot.Rows[0];
            string partNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
            //本托第一块组件。成品料号设置为包装料号
            if (this.gvConSigment.RowCount < 1)
            {
                this.txtSapNo.Text = partNumber;
            }
            string packagePartNumber = this.txtSapNo.Text;
            //SAP料号不同给出提示。
            if (!packagePartNumber.Equals(partNumber))
            {
                MessageService.ShowError(string.Format("组件（{0}）的产品料号（{1}）不符合。",
                                                        drLot[POR_LOT_FIELDS.FIELD_PART_NUMBER],
                                                        partNumber));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查平均功率。如果是本托第一块组件，则将平均功率范围设置为包装平均功率范围。
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        private bool CheckAvgPower(DataTable dtLot,ref DataRow drPackageDetail)
        {
            //不需要检查IV测试数据。
            if (this._isCheckIVTestdata == false)
            {
                return true;
            }
            DataRow drLot=dtLot.Rows[0];
            string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            string checkPower = Convert.ToString(drLot[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]);
            if (string.IsNullOrEmpty(checkPower))
            {
                MessageService.ShowError(string.Format("组件【{0}】功率为空，请确认！",lotNumber));
                return false;
            }
            decimal avgMin = decimal.MinValue;
            decimal avgMax = decimal.MaxValue;
            string avgPowerRange = string.Empty;
            //获取平均功率控制规则。
            DataSet dsAvgPower = this._lotEntity.GetPackageAvgPowerRangeData(lotNumber);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return false;
            }
            if (dsAvgPower != null && dsAvgPower.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsAvgPower.Tables[0].Rows[0];
                if (dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MIN] != DBNull.Value
                    && dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MIN] != null)
                {
                    avgMin = Convert.ToDecimal(dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MIN]);
                }

                if (dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MAX] != DBNull.Value
                   && dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MAX] != null)
                {
                    avgMax = Convert.ToDecimal(dr[BASE_TESTRULE_AVGPOWER.FIELDS_AVGPOWER_MAX]);
                }

                if (avgMin != decimal.MinValue || avgMax != decimal.MaxValue)
                {
                    avgPowerRange = string.Format("{0}~{1}",
                                                  avgMin == decimal.MinValue ? "Min" : avgMin.ToString(),
                                                  avgMax == decimal.MaxValue ? "Max" : avgMax.ToString());
                }
            }
            //本托第一块组件。将平均功率范围设置为包装平均功率范围。
            if (this.gvConSigment.RowCount < 1)
            {
                this.txtAvgRange.Text = avgPowerRange;
                this.txtAvgRange.Tag = new decimal[] { avgMin, avgMax };
            }
            //如果有进行平均功率范围控制。
            if (string.IsNullOrEmpty(this.txtAvgRange.Text) && this.txtAvgRange.Tag != null)
            {
                decimal[] avg = this.txtAvgRange.Tag as decimal[];
                avgMin = avg[0];
                avgMax = avg[1];
                decimal power = Convert.ToDecimal(checkPower);
                //如果不是尾单 并且平均功率不符合当前平均功率范围 或者 平均功率控制规则不同。  20150424 by chao.pang 注销
                if (!this.chkLastPallet.Checked && (power < avgMin || power > avgMax))
                {
                    MessageService.ShowError(string.Format("批次【{0}】功率超出平均功率控制范围！", lotNumber));
                    return false;
                }
            }
            drPackageDetail[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE] = avgPowerRange;
            return true;
        }
        /// <summary>
        /// 检查混包规则。本托组件的第一块组件，设置为包装的等级、颜色、分档类型、档位、子分档
        /// </summary>
        /// <param name="dtLot"></param>
        /// <returns></returns>
        private bool CheckMixPackage(DataTable dtLot,ref DataRow drPackageDetail)
        {
            DataRow drLot = dtLot.Rows[0];
            string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            string productId = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PRO_ID]);
            string proLevel = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PRO_LEVEL]);
            string gradeName = Convert.ToString(drLot[COLNAME_GRADE_NAME]);
            string lotColor = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_COLOR]);
            string ps_code = string.Empty;
            string ps_seq = string.Empty;
            string ps_sub_seq = string.Empty;
            string ps_key = string.Empty;
            string psTypeName = string.Empty;
            string psName = string.Empty;
            string pmaxStab = string.Empty;
            string subPowerlevel = string.Empty;
            //需要检查IV测试数据。
            if (this._isCheckIVTestdata == true)
            {
                //获取功率分档数据。
                DataSet dsPowersetData = this._lotEntity.GetLotPowersetData(lotNumber);
                if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                {
                    MessageService.ShowError(this._lotEntity.ErrorMsg);
                    return false;
                }
                if (dsPowersetData == null || dsPowersetData.Tables[0].Rows.Count < 1)
                {
                    MessageService.ShowError(string.Format("批次【{0}】未获取包装档位数据！", lotNumber));
                    return false;
                }
                DataRow drPowersetData = dsPowersetData.Tables[0].Rows[0];
                ps_code = Convert.ToString(drPowersetData[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE]);
                ps_seq = Convert.ToString(drPowersetData[WIP_IV_TEST_FIELDS.FIELDS_I_IDE]);
                ps_sub_seq = Convert.ToString(drPowersetData[WIP_IV_TEST_FIELDS.FIELDS_I_PKID]);
                ps_key = Convert.ToString(drPowersetData[BASE_POWERSET.FIELDS_POWERSET_KEY]);
                psTypeName = Convert.ToString(drPowersetData[BASE_POWERSET.FIELDS_PS_RULE + "1"]);
                psName = Convert.ToString(drPowersetData[BASE_POWERSET.FIELDS_MODULE_NAME + "1"]);
                pmaxStab = Convert.ToString(drPowersetData[BASE_POWERSET.FIELDS_PMAXSTAB]);
                subPowerlevel = Convert.ToString(drPowersetData[BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL]);
                if (string.IsNullOrEmpty(ps_key))
                {
                    MessageService.ShowError(string.Format("批次【{0}】未找到功率档数据！", lotNumber));
                    return false;
                } 
            }
            //本托组件的第一块组件，设置为包装的等级、颜色、分档类型、档位、子分档
            //设置混花包装、混档包装、混等级包装要求。
            if (gvConSigment.RowCount < 1)
            {
                //设置分档类型。
                luePowerType.Tag = ps_code;
                luePowerType.Text = psTypeName;
                //设置分档。
                luePowerSet.Tag = ps_key;
                luePowerSet.Text = psName;
                //设置子分档。
                luePalletPowerSet.Tag = pmaxStab;
                luePalletPowerSet.Text = subPowerlevel;

                txtColor.Text = lotColor;
                lueQcLevel.Tag = proLevel;
                luePalletLevel.Tag = proLevel;
                lueQcLevel.Text = gradeName;
                luePalletLevel.Text = gradeName;
            }
            //获取对应等级的混花包装、混档包装、混等级包装要求。
            DataSet dsPackageMixLevel = this._lotEntity.GetPackageMixRule(lotNumber);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return false;
            }
            dtPackageMixRule = dsPackageMixLevel.Tables[0];

            string packageLevel = Convert.ToString(this.luePalletLevel.Tag);
            string packageColor = txtColor.Text;
            bool isMixColorPackage = false;
            bool isMixPowerPackage = false;
            bool isMixSubPowerPackage = false;
            string packageGroup = string.Empty;
            //获取包装混包规则。
            if (dtPackageMixRule != null && dtPackageMixRule.Rows.Count > 0)
            {
                DataRow drPackageMixRule=dtPackageMixRule.AsEnumerable()
                                                         .SingleOrDefault(dr => Convert.ToString(dr[BASE_TESTRULE_PROLEVEL.FIELDS_GRADE]) == packageLevel);
                if (drPackageMixRule != null)
                {
                    //包装组，是否允许混等级包装，同一包装组的等级才可以混等级包装。
                    packageGroup = Convert.ToString(drPackageMixRule[BASE_TESTRULE_PROLEVEL.FIELDS_PALLET_GROUP]);
                    if (string.IsNullOrEmpty(packageGroup))
                    {
                        packageGroup = packageLevel;
                    }
                    //是否允许混花色包装。
                    string color = Convert.ToString(drPackageMixRule[BASE_TESTRULE_PROLEVEL.FIELDS_MIN_COLOR]);
                    if (!string.IsNullOrEmpty(color))
                    {
                        isMixColorPackage = bool.Parse(color);
                    }
                    //是否允许混分档包装。
                    string level = Convert.ToString(drPackageMixRule[BASE_TESTRULE_PROLEVEL.FIELDS_MIN_LEVEL]);
                    if (!string.IsNullOrEmpty(level))
                    {
                        isMixPowerPackage = bool.Parse(level);
                    }
                    //是否允许混子分档包装。
                    string leveldetail = Convert.ToString(drPackageMixRule[BASE_TESTRULE_PROLEVEL.FIELDS_MIN_LEVEL_DETAIL]);
                    if (!string.IsNullOrEmpty(leveldetail))
                    {
                        isMixSubPowerPackage = bool.Parse(leveldetail);
                    }
                }
            }
            //非尾单。
            if (!this.chkLastPallet.Checked)
            {
                //判断-当前组件与包装组件等级不一致，判断是否可以混等级包装。
                if (!proLevel.Equals(packageLevel))
                {
                    //获取当前组件的等级包装分组。
                    string curPackageGroup = proLevel;
                    DataRow rowLevel = (from v in dtPackageMixRule.AsEnumerable()
                                        where v.Field<string>(BASE_TESTRULE_PROLEVEL.FIELDS_GRADE) == proLevel
                                        select v).SingleOrDefault();
                    if (rowLevel != null)
                    {
                        curPackageGroup = Convert.ToString(rowLevel[BASE_TESTRULE_PROLEVEL.FIELDS_PALLET_GROUP]);
                    }
                    //等级包装组不一致，则不能混等级包装。
                    if (!packageGroup.Equals(curPackageGroup))
                    {
                        MessageService.ShowError(string.Format("组件【{0}】等级({1})不符合混包要求！", lotNumber, gradeName));
                        return false;
                    }
                }
                //判断-当前组件花色与包装花色不一致，判断是否可以混花色包装。
                if (!lotColor.Equals(packageColor) && isMixColorPackage == false)
                {
                    MessageService.ShowError(string.Format("组件【{0}】花色({1})不一致！", lotNumber, lotColor));
                    return false;
                }
                string packagePSKey = Convert.ToString(this.luePowerSet.Tag);
                //不允许混档包装，并且分档类型不同，或者分档不同。给出提示。
                if (!isMixPowerPackage && !packagePSKey.Equals(ps_key))
                {
                    MessageService.ShowError(string.Format("批次【{0}】功率档({1})不在包装范围内！", lotNumber, psName));
                    return false;
                }
                string packageSubPowerLevel = this.luePalletPowerSet.Text;
                //不允许混子分档包装，并且当前组件子分档与包装子分档不一致。
                if (!isMixSubPowerPackage && packageSubPowerLevel != subPowerlevel)
                {
                    MessageService.ShowError(string.Format("批次【{0}】包装子分档({1})不在托范围内！", lotNumber, subPowerlevel));
                    return false;
                }
            }
                //尾单 by yibin.fei 2017.12.07
            else
            {
                string packagePSKey = Convert.ToString(this.luePowerSet.Tag);
                //不允许混档包装，并且分档类型不同，或者分档不同。给出提示。
                if (!isMixPowerPackage && !packagePSKey.Equals(ps_key))
                {
                    MessageService.ShowError(string.Format("批次【{0}】功率档({1})不在包装范围内！", lotNumber, psName));
                    return false;
                }
             
            }

            drPackageDetail["PS_CODE"] = ps_code;
            drPackageDetail["PS_SEQ"] = ps_key;
            drPackageDetail["PS_DTL_CODE"] = subPowerlevel;
            drPackageDetail["POWER_LEVEL"] = pmaxStab;
            return true;
        }
        /// <summary>
        /// 计算总功率\平均功率\入托数\设置包装明细序号。
        /// </summary>
        private void CalculatePackageData()
        {
            if (dtPackageDetail == null) return;
            int digits = 2;
            if (!string.IsNullOrEmpty(this.speDigNum.Text))
            {
                digits =Convert.ToInt32(this.speDigNum.Text);
            }
            //计算总功率。
            decimal totalPower = dtPackageDetail.AsEnumerable().Sum(dr => dr[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]==DBNull.Value
                                                                            ?0 
                                                                            :Convert.ToDecimal(dr[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]));
            this.lblTotlePower.Text = Math.Round(totalPower,digits,MidpointRounding.AwayFromZero).ToString();
            //计算平均功率。
            decimal avgPower = 0;
            if (dtPackageDetail.Rows.Count > 0)
            {
                //this.txtPalletNo.Properties.ReadOnly = true;
                avgPower = totalPower / dtPackageDetail.Rows.Count;
            }
            this.txtAvgPower.Text = Math.Round(avgPower, digits, MidpointRounding.AwayFromZero).ToString();
            this.txtEnterPallet.Text = dtPackageDetail.Rows.Count.ToString();
            //计算序号。
            int n = 1;
            foreach (DataRow dr in dtPackageDetail.Rows)
            {
                dr[COLNAME_SEQ] = n;
                n++;
            }
        }

        /// <summary>
        /// 批次过账作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            int fullQty=-1;
            if(!string.IsNullOrEmpty(this.txtFullPallet.Text))
            {
                fullQty=Convert.ToInt32(this.txtFullPallet.Text);
            }
            //非尾单，必须满托才能保存过账。
            if (!this.chkLastPallet.Checked && fullQty != gvConSigment.RowCount)
            {
                MessageService.ShowMessage("必须满托才能保存过账。", "提示");
                return;
            }
            SavePallet(1);
        }
        /// <summary>
        /// 批次保存作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            SavePallet(0);
        }
        /// <summary>
        /// 过账/保存作业，1：过账；0：保存
        /// </summary>
        /// <param name="saveType">1:过账;0:保存（不过帐）</param>
        private void SavePallet(int saveType)
        {
            //无需保存数据。
            if (dtPackage == null||dtPackageDetail==null || dtPackage.Rows.Count<1 || dtPackageDetail.Rows.Count<1)
            {
                MessageService.ShowMessage(string.Format("托号【{0}】没有需要保存的数据!", txtPalletNo.Text.Trim()), "提示");
                return;
            }

            string palletNo = this.txtPalletNo.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(palletNo))
            {//判定托号是否为空
                MessageService.ShowMessage("托盘号不能为空。", "提示");
                return;
            }

            int fullQty = -1;
            if (!string.IsNullOrEmpty(this.txtFullPallet.Text))
            {
                fullQty = Convert.ToInt32(this.txtFullPallet.Text);
            }
            if (fullQty != dtPackageDetail.Rows.Count)
            {
                if (!MessageService.AskQuestion(string.Format("【满托数{0}】和【已包装{1}】不一致，确认【{2}】么?", 
                                                                txtFullPallet.Text,
                                                                txtEnterPallet.Text,
                                                                saveType == 0 ? "保存" : "过站"),"提示"))
                {
                    return;
                }
            }

            //设置班别。
            string shiftName = this.cmbShift.Text;
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
           
            //组织保存数据。
            //将包装明细添加到数据集中。
            DataSet dsParams = new DataSet();
            dtPackageDetail.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            foreach (DataRow dr in dtPackageDetail.Rows)
            {//重新修改托盘号
                dr[POR_LOT_FIELDS.FIELD_PALLET_NO] = palletNo;
            }
            dsParams.Merge(dtPackageDetail, true, MissingSchemaAction.Add);
            if (dtPackage.Rows.Count <= 0)
            {
                dtPackage.Rows.Add(dtPackage.NewRow());
            }
            CalculatePackageData();
            //设置包装数据。
            DataRow drPackage = dtPackage.Rows[0];

            if (drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] == DBNull.Value
                || drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] == null)
            {
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME] = this._packageTime;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATER] = this._model.UserName;
            }
            //设置尾单类型。
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET] = this.chkLastPallet.Checked ? 1 : 0;
            //设置检验类型。
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = this.chkCusterCode.Checked ? 2 : (this.chkSideCode.Checked ? 1 : 0);
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER] = this.txtAvgPower.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = this._model.RoomKey;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY] = this._model.EquipmentKey;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME] = this._model.EquipmentName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = shiftName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_KEY] = this._model.LineKey;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_NAME] = this._model.LineName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = this.txtWo.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID] = this.txtPro_ID.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE] = this.txtAvgRange.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER] = this.lblTotlePower.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = this.txtSapNo.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_CODE] = Convert.ToString(this.luePowerType.Tag);
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_SEQ] = Convert.ToString(this.luePowerSet.Tag);
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_DTL_SUBCODE] = this.luePalletPowerSet.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_COLOR] = this.txtColor.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = Convert.ToString(this.luePalletLevel.Tag);
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = palletNo;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO] = palletNo;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL] = Convert.ToString(luePalletPowerSet.Tag);
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY] = dtPackageDetail.Rows.Count;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_FULL_QTY] = this.txtFullPallet.Text;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = saveType;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR] = this._model.UserName;
            dsParams.Merge(dtPackage, true, MissingSchemaAction.Add);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,this._operationKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, this._model.EquipmentKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, this._model.LineKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, this._model.LineName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, this._model.ShiftKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Merge(dtParams, true, MissingSchemaAction.Add);
            //执行包装作业。
            ParameterizedThreadStart start = new ParameterizedThreadStart(Package);
            Thread t = new Thread(start);
            t.Start(new object[]{saveType,dsParams});

        }

        private void Package(object obj)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                //this.lblMsg1.Visible = true;
                this.lblMsg.Visibility = LayoutVisibility.Always;
                this.lblMsg.Text = string.Format("正在执行包装操作，请勿关闭界面，等待...");
                this.tableLayoutPanelMain.Enabled = false;
            }));
            try
            {

                object[] objs = obj as object[];
                int saveType = Convert.ToInt32(objs[0]);
                DataSet dsParam = objs[1] as DataSet;
                DataSet dsReturn = this._lotEntity.LotPackage(dsParam);
                this.Invoke(new MethodInvoker(() =>
                {
                    if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                    {
                        MessageService.ShowError(this._lotEntity.ErrorMsg);
                        return;
                    }
                    if (saveType == 1)
                    { 
                        string[] l_s = new string[] { "AutoControl" };
                        string category = "Packing_List_Print_Control";
                        DataTable dtCommon = BaseData.Get(l_s, category);

                        if (dtCommon.Rows[0]["AutoControl"].ToString().ToUpper().Equals("ON"))
                        {
                            FlashAutoPrint();
                        }
                        //MessageService.ShowMessage(string.Format("托号【{0}】成功入托!", txtPalletNo.Text.Trim()), "系统提示");

                        #region//工艺设置的是否自动打印 暂不用
                        //IVTestDataEntity ivtestDataEntity = new IVTestDataEntity();
                        //DataSet dsPrintLabelSetInfo = ivtestDataEntity.GetWOPrintLabelDataByNo(this.txtWo.Text, this.txtSapNo.Text,this.txtPro_ID.Text);
                        //if (!string.IsNullOrEmpty(ivtestDataEntity.ErrorMsg))
                        //{
                        //    MessageService.ShowError(ivtestDataEntity.ErrorMsg);
                        //    return;
                        //}
                        //dsPrintLabelSetInfo.Tables[0].DefaultView.RowFilter = " ISMAIN=1 AND ISPACKAGEPRINT=1";
                        //if (dsPrintLabelSetInfo.Tables[0].DefaultView.Count > 0)
                        //{
                        //    int nPrintQty = Convert.ToInt32(dsPrintLabelSetInfo.Tables[0].DefaultView[0]["PRINT_QTY"]);
                        //    //判断包装打印
                        //    bool isPackageOpt = Convert.ToBoolean(dsPrintLabelSetInfo.Tables[0].DefaultView[0]["ISPACKAGEPRINT"]);
                        //    //判断是否包装工序打印
                        //    if (isPackageOpt && nPrintQty > 1)
                        //    {
                        //        LotIVTestPrintDialog packagePrintDialog = new LotIVTestPrintDialog();
                        //        packagePrintDialog.isPrintPalletNo = true;
                        //        packagePrintDialog.dtPalletNo = dtPackageDetail;
                        //        packagePrintDialog.PalletNoProId = this.txtPro_ID.Text;
                        //        if (packagePrintDialog.ShowDialog() == DialogResult.OK)
                        //        {

                        //        }
                        //    }
                        //}
                        #endregion

                    }
                    else
                    {
                        MessageService.ShowMessage(string.Format("托号【{0}】保存成功!", txtPalletNo.Text.Trim()), "系统提示");
                    }
                    //this.lblMsg.Visible = false;
                    this.lblMsg.Visibility = LayoutVisibility.Never;
                    this.lblMsg.Text = string.Empty;
                    this.tableLayoutPanelMain.Enabled = true;
                    InitControlsValue();
                    //this.txtPalletNo.Select();
                    //this.txtPalletNo.SelectAll();
                    this.txtPalletNo.Text = "";
                    this.txtLot_Num.Select();
                    this.txtLot_Num.SelectAll();
                    this.txtLot_Num.Focus();
                }));
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    //this.lblMsg.Visible = false;
                    this.lblMsg.Visibility = LayoutVisibility.Never;
                    this.lblMsg.Text = string.Empty;
                    this.tableLayoutPanelMain.Enabled = true;
                }));
            }
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
            LotDispathViewContent view = new LotDispathViewContent(_model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 单拖出拖事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSingleOutPallet_Click(object sender, EventArgs e)
        {
            //非包装作业进行中，不能进行单拖出拖。
            if (dtPackage == null || dtPackage.Rows.Count<=0)
            {
                return;
            }
            //没有选中包装列表中的任何一行。
            if (this.gvConSigment.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择需要出托的组件!", "提示");
                return;
            }
            bool bSuccess=LotUnpack(false);

            //如果包装列表中没有组件，则启用包装拖号可输入。初始化控件。
            if (bSuccess && dtPackageDetail.Rows.Count < 1)
            {
                InitControlsValue();
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
            }
        }
        /// <summary>
        /// 整托出托。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAllUnpack_Click(object sender, EventArgs e)
        {
            //非包装作业进行中，不能进行出拖。
            if (dtPackage == null || dtPackage.Rows.Count <= 0)
            {
                return;
            }
            //询问确定整托出托？
            if (MessageService.AskQuestionSpecifyNoButton("确定整托出托？", "询问"))
            {

                bool bSuccess = LotUnpack(true);
                //如果包装列表中没有组件，则启用包装拖号可输入。初始化控件。
                if (bSuccess && dtPackageDetail.Rows.Count < 1)
                {
                    InitControlsValue();
                    this.txtPalletNo.Select();
                    this.txtPalletNo.SelectAll();
                }
            }
        }
        /// <summary>
        /// 重设包装数据。
        /// </summary>
        private bool ResetPackageData()
        {
            if (gvConSigment.DataRowCount > 0)
            {
                DataRow drPacageDetail = dtPackageDetail.Rows[0];
                string orderNumber = Convert.ToString(drPacageDetail[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                string partNumber = Convert.ToString(drPacageDetail[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                //根据分档主键查询分档数据
                string psKey = Convert.ToString(drPacageDetail["PS_SEQ"]);
                string powerLevel = Convert.ToString(drPacageDetail["POWER_LEVEL"]);
                string psCode = Convert.ToString(drPacageDetail["PS_CODE"]);
                string psSubCode = Convert.ToString(drPacageDetail["PS_DTL_CODE"]);
                DataSet ds = this._lotEntity.GetWOProductPowersetData(orderNumber, partNumber, psKey);
                if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                {
                    throw new Exception(this._lotEntity.ErrorMsg);
                }
                string psTypeName = psCode;
                string psName = powerLevel;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drPowersetData = ds.Tables[0].Rows[0];
                    psTypeName = string.Format("{0}:{1}", drPowersetData[BASE_POWERSET.FIELDS_PS_CODE], drPowersetData[BASE_POWERSET.FIELDS_PS_RULE]);
                    psName = string.Format("{0}:{1}", drPowersetData[BASE_POWERSET.FIELDS_PS_SEQ], drPowersetData[BASE_POWERSET.FIELDS_MODULE_NAME]);
                }

                //设置分档类型。
                this.luePowerType.Tag = psCode;
                this.luePowerType.Text = psTypeName;
                //设置分档。
                this.luePowerSet.Tag = psKey;
                this.luePowerSet.Text = psName;
                //设置子分档。
                this.luePalletPowerSet.Tag = powerLevel;
                this.luePalletPowerSet.Text = psSubCode;
                //设置平均功率范围。
                string avgPowerRange = Convert.ToString(drPacageDetail[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE]);
                if (!string.IsNullOrEmpty(avgPowerRange))
                {
                    decimal avgMin = decimal.MinValue;
                    decimal avgMax = decimal.MaxValue;
                    string[] avgRanage = avgPowerRange.Split('~');
                    if (avgRanage.Length >= 1)
                    {
                        decimal.TryParse(avgRanage[0], out avgMin);
                    }
                    if (avgRanage.Length >= 2)
                    {
                        decimal.TryParse(avgRanage[1], out avgMax);
                    }
                    if (avgMin != decimal.MinValue || avgMax != decimal.MaxValue)
                    {
                        avgPowerRange = string.Format("{0}~{1}",
                                                      avgMin == decimal.MinValue ? "Min" : avgMin.ToString(),
                                                      avgMax == decimal.MinValue ? "Max" : avgMax.ToString());
                    }
                    this.txtAvgRange.Text = avgPowerRange;
                    this.txtAvgRange.Tag = new decimal[] { avgMin, avgMax };
                }

                this.txtColor.Text = Convert.ToString(drPacageDetail[POR_LOT_FIELDS.FIELD_COLOR]);
                string proLevel = Convert.ToString(drPacageDetail[POR_LOT_FIELDS.FIELD_PRO_LEVEL]);
                string gradeName = Convert.ToString(drPacageDetail[COLNAME_GRADE_NAME]);
                this.lueQcLevel.Tag = proLevel;
                this.luePalletLevel.Tag = proLevel;
                this.lueQcLevel.Text = gradeName;
                this.luePalletLevel.Text = gradeName;
                this.txtPro_ID.Text = Convert.ToString(drPacageDetail[POR_LOT_FIELDS.FIELD_PRO_ID]); ;
                this.txtFullPallet.Text = Convert.ToString(drPacageDetail[WIP_CONSIGNMENT_FIELDS.FIELDS_FULL_QTY]);
                this.txtSapNo.Text = Convert.ToString(drPacageDetail[POR_LOT_FIELDS.FIELD_PART_NUMBER]); ;
            }
            return true;
        }
        /// <summary>
        /// 批次拆包操作。
        /// </summary>
        /// <param name="bAll">true:整托出托 false：单托出托</param>
        private bool LotUnpack(bool bAll)
        {
            try
            {
                DataTable dtTempPackageDetail = dtPackageDetail.Clone();
                dtTempPackageDetail.Merge(dtPackageDetail);
                int index = gvConSigment.FocusedRowHandle;

                DataRow drRow = gvConSigment.GetFocusedDataRow();
                DataTable dtUnpackDetail = dtPackageDetail.Clone();
                dtUnpackDetail.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                //非整托出托作业。
                if (!bAll)
                {
                    //如果该行不是新增的行，需要进行数据库操作。
                    if (drRow.RowState != DataRowState.Added)
                    {
                        dtUnpackDetail.ImportRow(drRow);
                    }
                    dtPackageDetail.Rows.Remove(drRow);
                    if (index == 0)
                    {
                        ResetPackageData();
                    }
                }
                else
                {
                    foreach (DataRow drPackageDetail in dtPackageDetail.Rows)
                    {
                        if (drPackageDetail.RowState != DataRowState.Added)
                        {
                            dtUnpackDetail.ImportRow(drPackageDetail);
                        }
                    }
                    dtPackageDetail.Clear();
                }
                CalculatePackageData();
                //不需要进行数据库操作，直接返回
                if (dtUnpackDetail.Rows.Count == 0 && dtPackageDetail.Rows.Count>0)
                {
                    return true;
                }
                //设置包装数据。
                DataRow drPackage = dtPackage.Rows[0];
                string palletKey = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                //未保存数据，不需要更新数据库数据。
                if (string.IsNullOrEmpty(palletKey))
                {
                    return true;
                }
                //设置班别。
                string shiftName = this.cmbShift.Text;
                string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
                string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                //组织保存数据。
                DataSet dsParams = new DataSet();
                //设置尾单类型。
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET] = this.chkLastPallet.Checked ? 1 : 0;
                //设置检验类型。
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = this.chkCusterCode.Checked ? 2 : (this.chkSideCode.Checked ? 1 : 0);
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = this._model.RoomKey;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY] = this._model.EquipmentKey;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME] = this._model.EquipmentName;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = shiftName;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_KEY] = this._model.LineKey;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_NAME] = this._model.LineName;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = this.txtWo.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID] = this.txtPro_ID.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE] = this.txtAvgRange.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER] = this.lblTotlePower.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = this.txtSapNo.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_CODE] = Convert.ToString(this.luePowerType.Tag);
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_SEQ] = Convert.ToString(this.luePowerSet.Tag);
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_DTL_SUBCODE] = this.luePalletPowerSet.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_COLOR] = this.txtColor.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = Convert.ToString(this.luePalletLevel.Tag);
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL] = Convert.ToString(luePalletPowerSet.Tag);
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY] = dtPackageDetail.Rows.Count;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER] = this.txtAvgPower.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_FULL_QTY] = this.txtFullPallet.Text;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 0;
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR] = this._model.UserName;
                //如果已全部出托，则更新包装标记为无效。
                drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_ISFLAG] = dtPackageDetail.Rows.Count < 1 ? 0 : 1;
                dsParams.Merge(dtPackage, true, MissingSchemaAction.Add);
                //将拆包明细添加到数据集中。
                dsParams.Merge(dtUnpackDetail, true, MissingSchemaAction.Add);
                //组织其他附加参数数据
                Hashtable htMaindata = new Hashtable();
                htMaindata.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME, this._packageTime);
                htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, this._operationKey);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, this._model.EquipmentKey);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, this._model.LineKey);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, this._model.LineName);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, this._model.ShiftKey);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
                DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
                dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
                dsParams.Merge(dtParams, true, MissingSchemaAction.Add);

                DataSet dsReturn = this._lotEntity.LotUnpack(dsParams);
                if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                {
                    MessageService.ShowError(this._lotEntity.ErrorMsg);
                    dtPackageDetail.Merge(dtTempPackageDetail);
                    if (!bAll && index == 0)
                    {
                        ResetPackageData();
                    }
                    CalculatePackageData();
                    return false;
                }
                //重置包装主数据。
                dtPackage = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
                return true;
            }
            catch(Exception ex)
            {
                MessageService.ShowError("出托出现无法处理的异常，系统将会重新初始化托盘数据。错误:\r\n" + ex.Message);
                InitPackage();
                return false;
            }
        }

        /// <summary>
        /// 侧板编码选择改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSideCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSideCode.Checked == true)
            {
                this.chkCusterCode.Checked = false;     
                this.layout_lotTitle.Text = "侧板编码号";
            }
            else
            {
                this.layout_lotTitle.Text = "组件序列号"; 
            }
            InitControlsValue();
            this.txtPalletNo.Select();
            this.txtPalletNo.SelectAll();
        }
        /// <summary>
        /// 客户编码选择改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCusterCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCusterCode.Checked == true)
            {
                this.chkSideCode.Checked = false;
                this.layout_lotTitle.Text = "客户编码号";
            }
            else
            {
                this.layout_lotTitle.Text = "组件序列号";
            }
            InitControlsValue();
            this.txtPalletNo.Select();
            this.txtPalletNo.SelectAll();
        }
        /// <summary>
        /// 拖号变更对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExchgPalletNo_Click(object sender, EventArgs e)
        {
            string subTitle = "托号变更作业";
            LotDispatchForPltChkDialog ldfp = new LotDispatchForPltChkDialog(80, _model, subTitle);
            //托号变更    
            if (DialogResult.OK == ldfp.ShowDialog())
            {

            }
        }
        /// <summary>
        /// 尾单选择改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLastPallet_CheckedChanged(object sender, EventArgs e)
        {
            InitControlsValue();
            this.txtPalletNo.Select();
            this.txtPalletNo.SelectAll();
        }
        /// <summary>
        /// 手动打印标签数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrintLabel_Click(object sender, EventArgs e)
        {
            LotIVTestPrintDialog lotIvTestPrintDialog = new LotIVTestPrintDialog();
            lotIvTestPrintDialog.isPrintPalletNo = true;
            lotIvTestPrintDialog.ShowDialog();
        }
        /// <summary>
        /// 重置按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            InitControlsValue();
            this.txtPalletNo.Select();
            this.txtPalletNo.SelectAll();
        }



        public void FlashAutoPrint()
        {
           string workOrderNumber = txtWo.Text;
            string partNumber = txtSapNo.Text;
            string palletNo = txtPalletNo.Text;
         

            //string workOrderNumber = "650000045";
            //string partNumber = "2010004927";
            //string palletNo = "650000045-3-0023";

            DataSet dsFlashAutoPrin = workOrderEntity.GetFlashAutoPrintData(workOrderNumber);

            DataRow[]  drPrints = dsFlashAutoPrin.Tables[0].Select(string.Format("PART_NUMBER='{0}'", partNumber));
            if (drPrints.Count() > 0)
            {
                string printTypes = drPrints[0]["PRINT_TYPE"].ToString();
                
                string printCopys = string.Empty;
                
                int fullQty = -1;
                if (!string.IsNullOrEmpty(this.txtFullPallet.Text))
                {
                    fullQty = Convert.ToInt32(this.txtFullPallet.Text);
                }

                printCopys = drPrints[0]["PRINT_COPY"].ToString();
               
                string PrintStyle = string.Empty;
                string QuanlityGrade = string.Empty;
                string sQty = string.Empty;
                sQty = "28";
                PrintStyle = "AutoPrint";
                QuanlityGrade = "E";
                PackingListPrint _Print = new PackingListPrint(PrintStyle, QuanlityGrade);
                string[] pts = printTypes.Split(',');
                string[] pcs = printCopys.Split(',');
                for (int i = 0; i < pts.Count(); i++)
                {
                    string printType = pts[i].Trim();
                    string printCopy = string.Empty;
                    if (fullQty != gvConSigment.RowCount)
                    {
                        printCopy = "2";
                    }
                    else
                    {
                        printCopy = pcs[i].Trim();
                    }
                    

                    //执行common打印
                    if (printType.Equals("Common"))
                    {
                        _Print.CommonPrint(palletNo, sQty, printCopy);
                    }
                    //执行Conergy打印
                    else if (printType.Equals("Conergy"))
                    {
                        _Print.Conergy16Print(palletNo, sQty, printCopy);
                    }
                    //执行NoPower打印
                    else if (printType.Equals("NoPower"))
                    {
                        _Print.NPowerPrint(palletNo, sQty, printCopy);
                    }
                    //执行BJPower打印
                    else if (printType.Equals("BJPower"))
                    {
                        _Print.BJPowerPrint(palletNo, sQty, printCopy);
                    }
                    //执行Com打印
                    else if (printType.Equals("Com"))
                    {
                        _Print.ComPrint(palletNo, sQty, printCopy);
                    }
                    //执行双玻打印 
                    else if (printType.Equals("ShuangBo33"))
                    {
                        _Print.ShuangBo33Print(palletNo, sQty, printCopy);
                    }
                    //执行NE打印
                    else if (printType.Equals("NE(New)"))
                    {
                        _Print.NeNewPrint(palletNo, sQty, printCopy);
                    }
                    //执行QX打印
                    else if (printType.Equals("QX"))
                    {
                        _Print.QXPrint(palletNo, sQty, printCopy);
                    }
                    //执行QingTian打印
                    else if (printType.Equals("QingTian"))
                    {
                        _Print.QingTianPrint(palletNo, sQty, printCopy);
                    }
                    //执行jingke打印
                    else if (printType.Equals("JingKe"))
                    {
                        _Print.JingKePrint(palletNo, sQty, printCopy);
                    }
                    //执行NE(非PERC)打印
                    else if (printType.Equals("NE(NEW)02"))
                    {
                        _Print.NENew02Print(palletNo, sQty, printCopy);
                    }
                    //执行NE(最新)打印
                    else if (printType.Equals("NE(NEW)03"))
                    {
                        _Print.NENew03Print(palletNo, sQty, printCopy);
                    }
                    //执行NE（5BB）(非PERC)打印 
                    else if (printType.Equals("NE5BB"))
                    {
                        _Print.NE5BBPrint(palletNo, sQty, printCopy);
                    }
                    //执行摩洛哥打印
                    else if (printType.Equals("Mologe"))
                    {
                        _Print.MologePrint(palletNo, sQty, printCopy);
                    }

                    //执行SolarJucie打印
                    else if (printType.Equals("SolarJucie"))
                    {
                        _Print.SolarJuciePrint(palletNo,sQty,printCopy);
                    }
                    //执行二维码打印
                    else if (printType.Equals("QrCode"))
                    {
                        _Print.QrCodePrint(palletNo,printCopy);
                        
                    }
                    //执行PVLINE清单打印 
                    else if (printType.Equals("PVLINE"))
                    {
                        _Print.PVLinePrint(palletNo, sQty, printCopy);
                    }
                    //执行安能清单打印 
                    else if (printType.Equals("AnNeng"))
                    {
                        _Print.AnNengPrint(palletNo, sQty, printCopy);
                    }
                    //执行ASM清单打印
                    else if (printType.Equals("ASM"))
                    {
                        _Print.ASMPrint(palletNo, sQty, printCopy);
                    }
                    //执行Common2清单打印 
                    else if (printType.Equals("Common2"))
                    {
                        _Print.Common2Print(palletNo, sQty, printCopy);
                    }
                    //WEST清单打印
                    else if (printType.Equals("West"))
                    {
                        _Print.WestPrint(palletNo, sQty, printCopy);
                    }
                    //WEST货物标识打印
                    else if (printType.Equals("WestFlag"))
                    {
                        _Print.WestFlagPrint(palletNo, sQty, printCopy);
                    }



                }

            }
            else
            {
                MessageBox.Show("该工单料号未维护Flash模板，请联系NPI维护");
            }
        }

        
    }

    /// <summary>
    /// 托盘号生成规则。
    /// </summary>
    public enum EnumPalletNoRule
    {
        /// <summary>
        /// 正常。
        /// </summary>
        Normal=0,
        /// <summary>
        /// SE托盘号规则。
        /// </summary>
        SE=1
    }


    public class LotDispatchDetailForPalletModel : LotDispatchDetailModel
    {
        public LotDispatchDetailForPalletModel(LotDispatchDetailModel model)
        {
            this.EquipmentKey = model.EquipmentKey;
            this.EquipmentName = model.EquipmentName;
            this.LineKey = model.LineKey;
            this.LineName = model.LineName;
            this.LotEditTime = model.LotEditTime;
            this.LotNumber = model.LotNumber;
            this.OperationType = model.OperationType;
            this.RoomKey = model.RoomKey;
            this.RoomName = model.RoomName;
            this.ShiftKey = model.ShiftKey;
            this.ShiftName = model.ShiftName;
            this.TitleName = model.TitleName;
            this.UserName = model.UserName;
            this.OperationName = model.OperationName;
            this.IsCheckSILot = model.IsCheckSILot;
            this.ShiftKey = model.ShiftKey;
            this.OperationName = model.OperationName;
            this.EquipmentKey = model.EquipmentKey;
            this.LineKey = model.LineKey;
            this.LineName = model.LineName;
            this.IsLastWo = false;
        }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// 产品ID号
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 托号
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>
        /// 侧板标签
        /// </summary>
        public string LotSideCode { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string LotCustCode { get; set; }
        /// <summary>
        /// 平均功率控制范围
        /// </summary>
        public string AvgPowerRange { get; set; }
        /// <summary>
        /// 平均功率
        /// </summary>
        public decimal AvgPower { get; set; }
        /// <summary>
        /// 当前组件功率
        /// </summary>
        public decimal CurrentPower { get; set; }
        /// <summary>
        /// SAP料号
        /// </summary>
        public string SapMaterial { get; set; }
        /// <summary>
        /// 满托数
        /// </summary>
        public int FullPalletQty { get; set; }
        /// <summary>
        /// 托等级
        /// </summary>
        public string PalletLevel { get; set; }
        /// <summary>
        /// 质量等级
        /// </summary>
        public string QcLevel { get; set; }
        /// <summary>
        /// 组件花色
        /// </summary>
        public string ModuelColor { get; set; }
        /// <summary>
        /// 包装分档类型
        /// </summary>
        public string PowerLevelType { get; set; }
        /// <summary>
        /// 功率档
        /// </summary>
        public string PowerLevel { get; set; }
        /// <summary>
        /// 包装档位
        /// </summary>
        public string SigmentLevel{ get; set; }
        /// <summary>
        /// 是否尾单
        /// </summary>
        public bool IsLastWo { get; set; }
        /// <summary>
        /// 包装分档
        /// </summary>
        public string PowerPackageLevel
        {
            get { return _powerPackageLevel; }
            set { _powerPackageLevel = value; }
        }
        /// <summary>
        /// 校验花色，针对Pro_ID测试规则设定要求
        /// </summary>
        public bool CheckColor 
        {
            get { return _checkColor; }
            set { _checkColor = value; }
        }
        /// <summary>
        /// 校验档位，针对Pro_ID测试规则要求，是否可以混档位包装
        /// </summary>
        public bool CheckPowerLevel 
        { 
            get { return _checkPowerLevel; }
            set { _checkPowerLevel = value; }
        }
        /// <summary>
        /// 包装组，针对Pro_ID测试规则要求，是否可以混等级包装
        /// </summary>
        public string PackageGroup
        {
            get { return _packageGroup; }
            set { _packageGroup = value; }
        }
        /// <summary>
        /// 不允许混等级
        /// </summary>
        public readonly string MinGroup = "NoPromitMinGradePackage";
        /// <summary>
        /// 校验子档位，针对Pro_ID测试规则要求，是否可以混子档位包装
        /// </summary>
        public bool CheckPowerDetailLevel
        {
            get { return _checkPowerDetailLevel; }
            set { _checkPowerDetailLevel = value; }
        }

        string _packageGroup = string.Empty;
        bool _checkPowerLevel = false;
        bool _checkPowerDetailLevel = false;
        bool _checkColor = false;
        string _powerPackageLevel = string.Empty;


        
    }
   
}
