using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Barcode;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Utils.Common;
using System.Text.RegularExpressions;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Addins.WIP.Report;
using System.Reflection;
using System.Threading;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotIVTestPrintDialog : BaseDialog
    {
        public string sFlag = "F";
        public DataSet dsEquipments;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示

        LotOperationEntity _entity = new LotOperationEntity();
        IVTestDataEntity _testDataEntity = new IVTestDataEntity();
        WorkOrders _workOrderEntity = new WorkOrders();
        ComputerEntity _computerEntity = new ComputerEntity();
        LotAfterIvTestEntity _lotAfterIVTest = new LotAfterIvTestEntity();

        NameplateLabelAutoPrint nameplate = new NameplateLabelAutoPrint();
        /// <summary>
        /// 是否自动打印包装托号
        /// </summary>
        public bool isPrintPalletNo = false;
        /// <summary>
        /// 按托号打印的批次数据
        /// </summary>
        public DataTable dtPalletNo = null;
        /// <summary>
        /// 包装用的产品ID号
        /// </summary>
        public string PalletNoProId = string.Empty;

        public LotIVTestPrintDialog()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.ckPackAttenuation.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.ckPackAttenuation}");//"包装衰减";
            this.ckRemote.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.ckRemote}");//"远程铭牌";
            this.chkAutoPrint.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.chkAutoPrint}");//"自动打印";
            this.xtraTPLabelL.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.xtraTPLabelL}");//"标签打印";
            this.gcolLabelDataLabelName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolLabelDataLabelName}");//"名称";
            this.gcolLabelDataProductModel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolLabelDataProductModel}");//"产品型号";
            this.gcolLabelDataCertificateType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolLabelDataCertificateType}");//"认证类型";
            this.gcolLabelDataPowersetType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolLabelDataPowersetType}");//"分档方式";
            this.chkAutoPrint01.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.chkAutoPrint01}");//"自动打印";
            this.btnLablePrint01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.btnLablePrint01}");//"打印";
            this.checkLabel.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.checkLabel}");//"选择打印机";
            this.lciPrintLabel01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciPrintLabel01}");//"选择标签";
            this.lciLabelPrint01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciLabelPrint01}");//" 打印";
            this.lciPrintQty01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciPrintQty01}");//"打印数量";
            this.lcgPrinterSet.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lcgPrinterSet}");//"边距校准";
            this.lciLeftRight01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciLeftRight01}");//"左右调整";
            this.lciUpDown01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciUpDown01}");//"上下调整";
            this.lciTemperature01.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciTemperature01}");//"打印温度";
            this.xtraTPLabelP.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.xtraTPLabelP}");//"铭牌打印";
            this.chkAutoPrint02.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.chkAutoPrint02}");//"自动打印";
            this.btnLablePrint02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.btnLablePrint02}");//"打印";
            this.gcolPLabelName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolPLabelName}");//"名称";
            this.gcolPProductModel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolPProductModel}");//"产品型号";
            this.gcolPCertificateType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolPCertificateType}");//"认证类型";
            this.gcolPPowersetType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.gcolPPowersetType}");//"分档方式";
            this.checkMP.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.checkMP}");//"选择打印机";
            this.lcgNameplateBottom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lcgNameplateBottom}");//"边距校准";
            this.lciLeftRight02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciLeftRight02}");//"左右调整：";
            this.lciUpDown02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciUpDown02}");//"上下调整：";
            this.lciTemperature02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciTemperature02}");//"打印温度";
            this.lciPrintLable02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciPrintLable02}");//"选择铭牌";
            this.lciLabelPrint02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciLabelPrint02}");//"打印";
            this.lciPrintQty02.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciPrintQty02}");//"打印数量";
            this.checkGeneral.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.checkGeneral}");//"通用打印";
            this.lciLotNum.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciLotNum}");//"组件序列号：";
            this.lciPrintResults.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.lciPrintResults}");//"打印结果";
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Text}");//"标签打印";
        }





        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="lotNumber"></param>
        public LotIVTestPrintDialog(string lotNumber)
        {
            InitializeComponent();
            this.txtLotNum.Text = lotNumber;
            this.txtLotNum.Enabled = false;
            this.xtraTPLabelP.PageVisible = false;
            if (!string.IsNullOrEmpty(lotNumber))
            {
                sFlag = "T";
            }
        }

        private void LotIVTestPrintDialog_Load(object sender, EventArgs e)
        {
            lblInfoList.Text = "\t姓名：" + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ)
                  + "\n\t工号：" + PropertyService.Get("USER_NAME")
                  + "\n\t操作时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EquipmentEntity equipmentEntity = new EquipmentEntity();
            dsEquipments = equipmentEntity.GetEquipments();


            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();

            DataSet dsLabelLInfo = IVTestDateObject.GetLabelInfo("L");
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsLabelLInfo != null)
                {
                    this.luePrintLable01.Properties.DataSource = dsLabelLInfo.Tables[0];
                    this.luePrintLable01.Properties.DisplayMember = "LABEL_NAME";
                    this.luePrintLable01.Properties.ValueMember = "LABEL_ID";
                }
                else
                {
                    this.luePrintLable01.Properties.DataSource = null;
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                this.luePrintLable01.Properties.DataSource = null;
                return;
            }

            //打印包装数据或者手动打印批次数据
            if (this.isPrintPalletNo)
            {
                //移除铭牌打印tabPage
                this.xtabPrintLabel.TabPages.Remove(this.xtraTPLabelP);
                this.ckPackAttenuation.Enabled = true;
                this.ckRemote.Enabled = false;
                // return;
            }

            #region
            DataSet dsLabelPInfo = IVTestDateObject.GetLabelInfo("P");
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsLabelPInfo != null)
                {
                    this.luePrintLable02.Properties.DataSource = dsLabelPInfo.Tables[0];
                    this.luePrintLable02.Properties.DisplayMember = "LABEL_NAME";
                    this.luePrintLable02.Properties.ValueMember = "LABEL_ID";
                }
                else
                {
                    this.luePrintLable02.Properties.DataSource = null;
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                this.luePrintLable01.Properties.DataSource = null;
                return;
            }

            try
            {
                string sAutoPrint, sLAutoPrint, sPAutoPrint, sLLabelID, sPLabelID, sLX, sLY, sPX, sPY, sLDarkness, sPDarkness, sL300DPI, sP300DPI;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("PrintConfig.xml");
                sAutoPrint = xmlDoc.SelectSingleNode("//UI/AUTO_PRINT").InnerText;
                sLAutoPrint = xmlDoc.SelectSingleNode("//UI/L_AUTO_PRINT").InnerText;
                sPAutoPrint = xmlDoc.SelectSingleNode("//UI/P_AUTO_PRINT").InnerText;
                sLLabelID = xmlDoc.SelectSingleNode("//UI/L_LABEL_ID").InnerText;
                sPLabelID = xmlDoc.SelectSingleNode("//UI/P_LABEL_ID").InnerText;
                sLX = xmlDoc.SelectSingleNode("//UI/L_X").InnerText;
                sLY = xmlDoc.SelectSingleNode("//UI/L_Y").InnerText;
                sPX = xmlDoc.SelectSingleNode("//UI/P_X").InnerText;
                sPY = xmlDoc.SelectSingleNode("//UI/P_Y").InnerText;
                sLDarkness = xmlDoc.SelectSingleNode("//UI/L_DARKNESS").InnerText;
                sPDarkness = xmlDoc.SelectSingleNode("//UI/P_DARKNESS").InnerText;
                sL300DPI = xmlDoc.SelectSingleNode("//UI/L_300DPI").InnerText;
                sP300DPI = xmlDoc.SelectSingleNode("//UI/P_300DPI").InnerText;
                if (sAutoPrint == "T")
                {
                    chkAutoPrint.Checked = true;
                }
                else
                {
                    chkAutoPrint.Checked = false;
                }
                if (sLAutoPrint == "T")
                {
                    chkAutoPrint01.Checked = true;
                }
                else
                {
                    chkAutoPrint01.Checked = false;
                }
                if (sPAutoPrint == "T")
                {
                    chkAutoPrint02.Checked = true;
                }
                else
                {
                    chkAutoPrint02.Checked = false;
                }
                if (!string.IsNullOrEmpty(sLLabelID))
                {
                    luePrintLable01.EditValue = sLLabelID;
                }
                if (!string.IsNullOrEmpty(sPLabelID))
                {
                    luePrintLable02.EditValue = sPLabelID;
                }
                if (!string.IsNullOrEmpty(sLX))
                {
                    speLeftRight01.Text = sLX;
                }
                if (!string.IsNullOrEmpty(sLY))
                {
                    speUpDown01.Text = sLY;
                }
                if (!string.IsNullOrEmpty(sPX))
                {
                    speLeftRight02.Text = sPX;
                }
                if (!string.IsNullOrEmpty(sPY))
                {
                    speUpDown02.Text = sPY;
                }
                if (!string.IsNullOrEmpty(sLDarkness))
                {
                    speTemperature01.Text = sLDarkness;
                }
                if (!string.IsNullOrEmpty(sPDarkness))
                {
                    speTemperature02.Text = sPDarkness;
                }
                if (sL300DPI == "T")
                {
                    chkDpi01.Checked = true;
                }
                else
                {
                    chkDpi01.Checked = false;
                }
                if (sP300DPI == "T")
                {
                    chkDpi02.Checked = true;
                }
                else
                {
                    chkDpi02.Checked = false;
                }
            }
            catch//(Exception ex)
            {
            }
            #endregion
        }

        bool isContinuePrintEvent = false;
        /// <summary>
        /// 按托号自动列印包装数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotIVTestPrintDialog_Paint(object sender, PaintEventArgs e)
        {
            if (!isContinuePrintEvent && isPrintPalletNo && this.dtPalletNo != null && this.dtPalletNo.Rows.Count > 0)
            {
                this.txtLotNum.Properties.ReadOnly = true;
                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
                try
                {
                    DataSet dsPrintLabelSetInfo = IVTestDateObject.GetPrintLabelSetInfo(this.PalletNoProId);

                    if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                    {
                        MessageService.ShowError(IVTestDateObject.ErrorMsg);
                        isContinuePrintEvent = true;
                        return;
                    }

                    DataRow[] drs = dsPrintLabelSetInfo.Tables[0].Select(string.Format(BASE_TESTRULE_PRINTSET.FIELDS_ISMAIN + "=1 and " + BASE_TESTRULE_PRINTSET.FIELDS_ISPACKAGEPRINT + "=1"));
                    if (drs != null && drs.Length > 0)
                    {
                        string l_value = Convert.ToString(drs[0][BASE_TESTRULE_PRINTSET.FIELDS_VIEW_NAME]);
                        luePrintLable01.EditValue = l_value;
                    }
                    else
                    {
                        MessageService.ShowError(string.Format("没有找到产品ID号【{0}】需要包装打印设置，请与工艺人员联系!", this.PalletNoProId));
                        isContinuePrintEvent = true;
                        return;
                    }

                    for (int i = 0; i < this.dtPalletNo.Rows.Count; i++)
                    {
                        this.txtLotNum.Text = string.Empty;
                        this.txtLotNum.Text = Convert.ToString(dtPalletNo.Rows[i][POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                        if (i == dtPalletNo.Rows.Count - 1)
                            this.sFlag = "T";

                        btnLablePrint01_Click(null, null);
                    }

                    isContinuePrintEvent = true;
                }
                catch //(Exception ex)
                {
                    isContinuePrintEvent = true;
                }
            }
        }

        private void txtLotNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (chkAutoPrint.Checked == true)
                {
                    if (xtabPrintLabel.SelectedTabPage.Name == "xtraTPLabelL")
                    {
                        if (chkAutoPrint01.Checked == true)
                        {
                            btnLablePrint01_Click(sender, e);
                        }
                    }
                    else if (xtabPrintLabel.SelectedTabPage.Name == "xtraTPLabelP")
                    {
                        if (chkAutoPrint02.Checked == true)
                        {
                            btnLablePrint02_Click(sender, e);
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        private void chkAutoPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoPrint.Checked == true)
            {
                chkAutoPrint01.Checked = true;
                chkAutoPrint02.Checked = true;
            }
            else
            {
                chkAutoPrint01.Checked = false;
                chkAutoPrint02.Checked = false;
            }
        }

        private void btnLablePrint01_Click(object sender, EventArgs e)
        {
            string lotNo = this.txtLotNum.Text.ToUpper();
            string labelNo = Convert.ToString(this.luePrintLable01.EditValue);

            if (string.IsNullOrEmpty(lotNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg001}"), MESSAGEBOX_CAPTION);//组件序列号不能为空，请确认！
                //MessageService.ShowMessage("组件序列号不能为空，请确认！", "提示");
                this.txtLotNum.Focus();
                return;
            }
            if (string.IsNullOrEmpty(labelNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg002}"), MESSAGEBOX_CAPTION);//没有选择标签，请确认！
                //MessageService.ShowMessage("没有选择标签，请确认！", "提示");
                this.luePrintLable01.Focus();
                return;
            }
            int nDarkness = int.Parse(this.speTemperature01.Text.Trim());
            int nX = int.Parse(this.speLeftRight01.Text.Trim());
            int nY = int.Parse(this.speUpDown01.Text.Trim());

            PrintLabelParameterData data = new PrintLabelParameterData();
            data.ErrorMessage = string.Empty;
            data.IsPrintErrorMessage = false;
            data.LotNo = lotNo;
            data.LabelNo = labelNo;
            data.Darkness = nDarkness;
            data.X = nX;
            data.Y = nY;
            data.IsPrintNameplate = false;
            data.IsChoosePrint = this.checkLabel.Checked;
            if (chkDpi01.Checked)
            {
                data.Dpi = 300;
            }
            //获取批次IV测试数据
            DataSet dsLotInfo = this._testDataEntity.GetIVTestData(data.LotNo);
            //检查组件序号是否存在
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件({0})获取IV测试数据出错：{1}", data.LotNo, this._testDataEntity.ErrorMsg);
                return;
            }

            if (dsLotInfo.Tables[0].Rows.Count <= 0)
            {
                data.ErrorMessage = string.Format("组件({0})不存在或已失效，请确认！", data.LotNo);
                return;
            }
            DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];
            data.WorkOrderNumber = Convert.ToString(drLotInfo["WORK_ORDER_NO"]);
            data.PM = Convert.ToDecimal(drLotInfo["PM"]);
            //判定工单产品属性中功率上下线卡控
            //DataTable dtUpLowRule = _workOrderEntity.GetUpLowRule(data.WorkOrderNumber);
            //if (dtUpLowRule != null || dtUpLowRule.Rows.Count > 0)
            //{
            //    //获取选中的行的信息
            //    DataRow[] drRows = dtUpLowRule.Select(string.Format(" UPLOW_RULE_UPLINE >='{0}' AND UPLOW_RULE_LOWLINE <='{0}'", data.PM));
            //    if (drRows.Length > 0)
            //    {
            //        MessageService.ShowMessage("该组件实测功率在工单功率管控上下限内不能答应条码，请确认！", "提示");
            //        this.txtLotNum.Select();
            //        return;
            //    }
            //}
            ExecutePrint(data);


            this.luePrintLable01.EditValue = data.LabelNo;
            this.txtLotNum.SelectAll();
            this.txtLotNum.Focus();
            ///打印铭牌
            if (ckRemote.Checked)
            {
                string printer = PropertyService.Get(PROPERTY_FIELDS.NAMEPLATE_PRINTER, "");
                int x = int.Parse(PropertyService.Get(PROPERTY_FIELDS.NAMEPLATE_PRINTER_X, "0"));
                int y = int.Parse(PropertyService.Get(PROPERTY_FIELDS.NAMEPLATE_PRINTER_Y, "0"));
                if (string.IsNullOrEmpty(printer))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg003}"), MESSAGEBOX_CAPTION);//请在【生产管理】-【铭牌自动打印】功能中设置铭牌打印机!
                    //MessageBox.Show("请在【生产管理】-【铭牌自动打印】功能中设置铭牌打印机!");
                    return;
                }
                Thread.Sleep(2000);
                nameplate.Print(lotNo, printer, x, y);
            }
        }

        private void btnLablePrint02_Click(object sender, EventArgs e)
        {
            string lotNo = this.txtLotNum.Text.ToUpper();
            string labelNo = Convert.ToString(this.luePrintLable02.EditValue);
            if (string.IsNullOrEmpty(lotNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg001}"), MESSAGEBOX_CAPTION);//组件序列号不能为空，请确认！
                //MessageService.ShowMessage("组件序列号不能为空，请确认！", "提示");
                this.txtLotNum.Select();
                return;
            }
            if (string.IsNullOrEmpty(labelNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg002}"), MESSAGEBOX_CAPTION);//没有选择标签，请确认！
                //MessageService.ShowMessage("没有选择标签，请确认！", "提示");
                this.luePrintLable02.Select();
                return;
            }
            int nDarkness = int.Parse(this.speTemperature02.Text.Trim());
            int nX = int.Parse(this.speLeftRight02.Text.Trim());
            int nY = int.Parse(this.speUpDown02.Text.Trim());

            PrintLabelParameterData data = new PrintLabelParameterData();
            data.LotNo = lotNo;
            data.LabelNo = labelNo;
            data.Darkness = nDarkness;
            data.X = nX;
            data.Y = nY;
            data.IsPrintNameplate = true;
            data.IsChoosePrint = this.checkMP.Checked;
            if (chkDpi02.Checked)
            {
                data.Dpi = 300;
            }

            ExecutePrint(data);
            this.luePrintLable02.EditValue = data.LabelNo;
            this.txtLotNum.SelectAll();
            this.txtLotNum.Focus();
        }

        private void ExecutePrint(PrintLabelParameterData data)
        {
            bool bCheck = IsAllowPrintLabel(data)  //判断组件是否允许打印标签。
                          && InitPrintLabelParameterData(data)  //获取批次数据&&获取批次号测试数据
                                                                //&& CheckIVImage(data)               //检查IV图片
                          && InitProductData(data)           //初始化产品数据、计算衰减、检查产品数据、初始化功率分档数据，检查功率分档数据
                          && CheckSubPowerset(data)          //检查子分档数据。
                          && CheckColor(data)                //检查花色
                          && CheckPrintLable(data)           //检查标签数据
                          && CheckCalibrationNo(data)        //校准版检验
                          && CalcCTM(data)                   //计算CTM
                          && CheckCTMByEff(data)             //CTM上下限管控检验
                          && CalcPowerDifferent(data)        //计算前后两次测试的功率差值
                          && CheckPowerControlData(data)     //产品控制检验
                          && CheckControlParaData(data)      //功率控制检验
                          && SavePrintDataToDatabase(data)   //保存打印数据
                          && IsConfigConputerInfo(data);     //检查打印相关配置是完善


            if (!string.IsNullOrEmpty(data.PowersetModuleName))
            {
                data.ErrorMessage = "功率档位:" + data.PowersetModuleName + ";";
            }

            this.mePrintResults.Text = data.ErrorMessage;
            //判定失败 提示错误信息
            if (!bCheck)
            {
                MessageService.ShowMessage(data.ErrorMessage, "提示");
                if (checkGeneral.Checked != true)
                {
                    #region 注释掉 by chao.pang 2015.10.19
                    if (data.IsPrintErrorMessage)
                    {
                        if (string.IsNullOrEmpty(data.LablePrinterType))
                        {
                            switch (data.LablePrinterType)
                            {
                                case "0":
                                    ArgobarPrinterHelper.wf_print_errorlable(data);
                                    break;
                                case "1":
                                    if (!string.IsNullOrEmpty(data.LabelPrinterIP)
                                        && !string.IsNullOrEmpty(data.LablePrinterPort))
                                    {
                                        ZebraNetPrinterHelper.zb_print_errorlable(data);
                                    }
                                    else
                                    {
                                        MessageBox.Show(@"打印机信息配置缺失，
                    请参照提示信息进行调整！");
                                    }
                                    break;
                                default:
                                    MessageBox.Show("不存在对应的打印类型！");

                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format(@"标签【{0}】打印机类型缺失，
                    请联系IT进行确认！", data.LabelNo));
                        }
                    }
                    #endregion
                }
                return;
            }
            //组件过站
            if (!LotTrack(data.LotNo, data.DeviceNum, data.PSign))
            {
                return; //过站失败。
            }

            //标签打印 old
            #region 注释 by chao.pang 2015.10.19
            if (checkGeneral.Checked != true)
            {
                switch (data.LablePrinterType)
                {
                    case "0":
                        if (!ArgobarPrinterHelper.PrintLabel(data))
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg004}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                            //MessageService.ShowMessage("打印标签失败，请重试！", "提示");
                            return;
                        }
                        break;
                    case "2":
                        if (!ZebraNetPrinterHelper.PrintLabel(data))
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg004}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                            //MessageService.ShowMessage("打印标签失败，请重试！", "提示");
                            return;
                        }
                        break;
                    default:
                        MessageBox.Show(string.Format(@"请联系IT确认标签【{0}】对应的打印类型是否实现！", data.LabelNo));
                        break;
                }
            }
            else
            {
                #region 标签打印 new create by chao.pang 2015.10.19
                if (!ModulePrint.PrintLabel(data))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg004}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                    //MessageService.ShowMessage("打印标签失败，请重试！", "提示");
                    return;
                }
                #endregion
            }
            #endregion

            #region 注释
            //if (chkCodeSoft.Checked == true)
            //{
            //    #region//斑马打印机
            //    if (CodeSoftPrint(data) == false)
            //    {
            //        MessageService.ShowMessage("打印标签失败，请重试！", "提示");
            //        return;
            //    }
            //    #endregion
            //}
            //else
            //{
            //    #region//立象打印机
            //    if (!ArgobarPrinterHelper.PrintLabel(data))
            //    {
            //        MessageService.ShowMessage("打印标签失败，请重试！", "提示");
            //        return;
            //    }
            //    #endregion
            //}
            #endregion
            SavePrintParam();

            if (sFlag == "T")
            {
                this.Close();
            }
        }

        /// <summary>
        /// 打印配置信息是否齐全
        /// </summary>
        /// <param name="data">LableID</param>
        /// <returns>True：存在对应的配置信息；False：不存在对应的配置信息或信息缺失</returns>
        private bool IsConfigConputerInfo(PrintLabelParameterData data)
        {

            #region 添加标签铭牌打印PrinterType的获取

            string printerType = string.Empty;

            printerType = _workOrderEntity.GetLablePrinterType(data.LabelNo);

            data.LablePrinterType = printerType;

            if (data.LablePrinterType == "Not Exists")
            {
                data.ErrorMessage = string.Format("请联系IT确认标签【{0}】是否在系统中维护", data.LabelNo);
                return false;
            }


            #endregion

            #region  本地电脑 打印机 IP 、端口 信息的获取

            if (data.LablePrinterType == "2")
            {
                if (_computerEntity.GetComputerPrinterInfo(PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME)))
                {
                    if (_computerEntity.PrinterPort.Length == 0)
                    {
                        data.ErrorMessage = "请配置打印机端口";
                        return false;
                    }
                    if (_computerEntity.PrinterIP.Length == 0)
                    {
                        data.ErrorMessage = "请配置打印机Ip地址";
                        return false;
                    }

                    data.LablePrinterName = _computerEntity.PrinterName;
                    data.LabelPrinterIP = _computerEntity.PrinterIP;
                    data.LablePrinterPort = _computerEntity.PrinterPort;
                }
                else
                {
                    data.ErrorMessage = string.Format("请配置当前电脑【{0}】对应的打印机信息", PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME));
                    return false;
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 是否允许打印标签。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：允许；false：不允许。</returns>
        private bool IsAllowPrintLabel(PrintLabelParameterData data)
        {
            if (this._testDataEntity.IsAllowPrintLabel(data.LotNo) == false)
            {
                data.ErrorMessage = this._testDataEntity.ErrorMsg;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 初始化打印参数数据对象。
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true:成功；false:失败。</returns>
        private bool InitPrintLabelParameterData(PrintLabelParameterData data)
        {
            //获取批次IV测试数据
            DataSet dsLotInfo = this._testDataEntity.GetIVTestData(data.LotNo);
            //检查组件序号是否存在
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件({0})获取IV测试数据出错：{1}", data.LotNo, this._testDataEntity.ErrorMsg);
                return false;
            }

            if (dsLotInfo.Tables[0].Rows.Count <= 0)
            {
                data.ErrorMessage = string.Format("组件({0})不存在或已失效，请确认！", data.LotNo);
                return false;
            }

            DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];

            if (drLotInfo["TTIME"] == DBNull.Value ||
                drLotInfo["TTIME"] == null)
            {
                data.ErrorMessage = string.Format("组件({0})无有效测试数据，请确认！", data.LotNo);
                return false;
            }
            data.PartNumber = Convert.ToString(drLotInfo["PART_NUMBER"]);
            data.ProductCode = Convert.ToString(drLotInfo["PRO_ID"]);
            data.WorkOrderNumber = Convert.ToString(drLotInfo["WORK_ORDER_NO"]);
            data.WorkOrderKey = Convert.ToString(drLotInfo["WORK_ORDER_KEY"]);
            data.FactoryName = Convert.ToString(drLotInfo["FACTORYROOM_NAME"]);
            data.IsBCPData = Convert.ToString(drLotInfo["CREATE_OPERTION_NAME"]) == "BCP";

            data.LotCellQty = Convert.ToInt32(drLotInfo["QUANTITY_INITIAL"]);

            data.CalibrationNo = Convert.ToString(drLotInfo["CALIBRATION_NO"]).Trim();
            data.PSign = Convert.ToString(drLotInfo["VC_PSIGN"]).Trim();
            data.DeviceNum = Convert.ToString(drLotInfo["DEVICENUM"]).Trim();
            data.TestTime = Convert.ToDateTime(drLotInfo["TTIME"]);
            data.PM = Convert.ToDecimal(drLotInfo["PM"]);
            data.Celleff = Convert.ToString(drLotInfo["VC_CELLEFF"]).Trim();
            data.ISC = Convert.ToDecimal(drLotInfo["ISC"]);
            data.IPM = Convert.ToDecimal(drLotInfo["IPM"]);
            data.VOC = Convert.ToDecimal(drLotInfo["VOC"]);
            data.VPM = Convert.ToDecimal(drLotInfo["VPM"]);
            data.FF = Convert.ToDecimal(drLotInfo["FF"]);
            data.Imp_Isc = drLotInfo["Imp_Isc"].ToString() == "" ? 0 : Convert.ToDecimal(drLotInfo["Imp_Isc"]);
            data.ImpIsc_Control = Convert.ToString(drLotInfo["ImpIsc_Control"]).Trim();
            data.Cretate_Time = Convert.ToDateTime(drLotInfo["CREATE_TIME"]);//
            data.TestTemperature = Convert.ToDecimal(drLotInfo["AMBIENTTEMP"]);
            data.IVTestDataKey = Convert.ToString(drLotInfo["IV_TEST_KEY"]);
            if (drLotInfo["COEF_PMAX"] != DBNull.Value
                && drLotInfo["COEF_PMAX"] != null)
            {
                data.CoefPM = Convert.ToDecimal(drLotInfo["COEF_PMAX"]);
            }
            if (drLotInfo["COEF_ISC"] != DBNull.Value
               && drLotInfo["COEF_ISC"] != null)
            {
                data.CoefISC = Convert.ToDecimal(drLotInfo["COEF_ISC"]);
            }
            if (drLotInfo["COEF_IMAX"] != DBNull.Value
               && drLotInfo["COEF_IMAX"] != null)
            {
                data.CoefIPM = Convert.ToDecimal(drLotInfo["COEF_IMAX"]);
            }
            if (drLotInfo["COEF_VOC"] != DBNull.Value
               && drLotInfo["COEF_VOC"] != null)
            {
                data.CoefVOC = Convert.ToDecimal(drLotInfo["COEF_VOC"]);
            }
            if (drLotInfo["COEF_VMAX"] != DBNull.Value
               && drLotInfo["COEF_VMAX"] != null)
            {
                data.CoefVPM = Convert.ToDecimal(drLotInfo["COEF_VMAX"]);
            }
            if (drLotInfo["COEF_FF"] != DBNull.Value
               && drLotInfo["COEF_FF"] != null)
            {
                data.CoefFF = Convert.ToDecimal(drLotInfo["COEF_FF"]);
            }
            return true;
        }
        /// <summary>
        /// 检查IV图片。
        /// 输入：车间名称、组件序列号。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckIVImage(PrintLabelParameterData data)
        {
            if (data.PSign != "Y" && data.IsBCPData == false)
            {
                //获取IV图片检测配置
                string[] cols = new string[] { "PIC_ADDRESS", "PIC_DATE_FORMAT", "PIC_FACTORY_CODE", "PIC_TYPE", "PIC_ADDRESS_NAME", "PIC_ISCHECK" };
                string categoryName = "Uda_Pic_Address";
                List<KeyValuePair<string, string>> lstConditions = new List<KeyValuePair<string, string>>();
                lstConditions.Add(new KeyValuePair<string, string>("PIC_TYPE", "IV"));
                DataTable dtPicAddress = BaseData.GetBasicDataByCondition(cols, categoryName, lstConditions);

                //判断当前车间是否需要检查IV图片。
                var lnq = from item in dtPicAddress.AsEnumerable()
                          where Convert.ToString(item["PIC_FACTORY_CODE"]) == data.FactoryName && Convert.ToString(item["PIC_ISCHECK"]).ToLower() == "true"
                          select item;
                if (lnq.GetEnumerator().MoveNext())
                {
                    //检查IV图片的所有配置路径
                    foreach (DataRow dr in dtPicAddress.Rows)
                    {
                        string sPicPath = Convert.ToString(dr["PIC_ADDRESS"]);
                        string dateFormat = Convert.ToString(dr["PIC_DATE_FORMAT"]);
                        string date = data.TestTime.ToString("yyyy-M-d");
                        if (!string.IsNullOrEmpty(dateFormat))
                        {
                            date = data.TestTime.ToString(dateFormat);
                        }

                        string sFilePath = sPicPath + @"\" + data.TestTime.ToString("yyyy") + @"年\"
                                                              + data.TestTime.ToString("MM") + @"月\"
                                                              + date + @"\"
                                                              + data.LotNo + ".GIF";
                        if (File.Exists(sFilePath))
                        {
                            return true;
                        }
                    }
                    //没有找到IV图片。
                    data.ErrorMessage = string.Format("组件（{0}）无IV图片，请确认！", data.LotNo);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查产品数据。初始化产品规则数据（标准校准版，产品型号，测试规则代码，功率小数位数）
        /// 输入：工单号、产品ID，
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool InitProductData(PrintLabelParameterData data)
        {
            //通过组件序列号抓对应工单产品中是否有数据。
            DataSet dsProductData = this._testDataEntity.GetWoProductData(data.WorkOrderNumber);
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件({0})获取工单({1})产品数据出错：{2}",
                                                data.LotNo,
                                                data.WorkOrderNumber,
                                                this._testDataEntity.ErrorMsg);
                return false;
            }
            //如果有数据，则表示组件序列号对应工单启用了联副产品入库。
            data.IsEnableByProduct = dsProductData != null
                                     && dsProductData.Tables.Count > 0
                                     && dsProductData.Tables[0].Rows.Count > 0;



            bool bLoopCalc = true;
            //是否启用了联副产品入库
            if (data.IsEnableByProduct)
            {
                //根据组件产品料号获取产品数据
                DataView dv = dsProductData.Tables[0].DefaultView;
                dv.Sort = "ITEM_NO ASC,PART_NUMBER ASC";
                dv.RowFilter = string.Format("PART_NUMBER='{0}'", data.PartNumber);
                //如果没有组件产品料号对应的产品数据，则将产品设置为工单主产品ID
                if (dv.Count <= 0)
                {
                    dv.RowFilter = "IS_MAIN='Y'";
                    if (dv.Count <= 0)
                    {
                        data.ErrorMessage = string.Format("组件({0})工单({1})没有设置主产品ID，请联系工艺人员设置。",
                                                          data.LotNo,
                                                          data.WorkOrderNumber);
                        return false;
                    }
                    data.PartNumber = Convert.ToString(dv[0]["PART_NUMBER"]);
                    data.ProductCode = Convert.ToString(dv[0]["PRODUCT_CODE"]);
                    dv.RowFilter = string.Format("PART_NUMBER='{0}'", data.PartNumber);
                }
                bool bSuccess = false;
                while (dv.Count > 0)
                {
                    int itemNo = Convert.ToInt32(dv[0]["ITEM_NO"]);
                    data.PartNumber = Convert.ToString(dv[0]["PART_NUMBER"]);
                    data.ProductKey = Convert.ToString(dv[0]["PRODUCT_KEY"]);
                    data.ProductCode = Convert.ToString(dv[0]["PRODUCT_CODE"]);
                    data.StandCalibration = Convert.ToString(dv[0]["CALIBRATION_TYPE"]);
                    data.CalibrationCycle = Convert.ToDouble(dv[0]["CALIBRATION_CYCLE"]);
                    data.ProductModel = Convert.ToString(dv[0]["PROMODEL_NAME"]);
                    data.FullPalletQty = Convert.ToInt32(dv[0]["FULL_PALLET_QTY"]);

                    data.Digits = Convert.ToInt32(dv[0]["POWER_DEGREE"]);
                    data.TestRuleCode = Convert.ToString(dv[0]["PRO_TEST_RULE"]);

                    data.MinPower = Convert.ToDecimal(dv[0]["MINPOWER"]);
                    data.MaxPower = Convert.ToDecimal(dv[0]["MAXPOWER"]);

                    //获取衰减系数
                    int decayCount = 0;

                    DataView dvDecoeffiData = null;
                    //判断是否允许重新计算衰减数据。没打印过标签，或允许重新计算衰减数据，则根据新的数据系数进行计算。
                    if (data.PSign != "Y" || this._testDataEntity.IsRecalcDecayData(data.LotNo))
                    {
                        DataSet dsDecoeffiData = this._testDataEntity.GetDecayCoefficient(data.WorkOrderNumber, data.PartNumber, data.PM);
                        if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                        {
                            data.ErrorMessage = string.Format("组件({0})获取工单({1})产品({2},{3})衰减数据出错：{4}",
                                                       data.LotNo,
                                                       data.WorkOrderNumber,
                                                       data.PartNumber,
                                                       data.ProductCode,
                                                       this._testDataEntity.ErrorMsg);
                            return false;
                        }
                        dvDecoeffiData = dsDecoeffiData.Tables[0].DefaultView;
                    }

                    while (bLoopCalc)
                    {
                        bSuccess = CalcCoefIVTestData(data, dvDecoeffiData, decayCount, out bLoopCalc)  //计算衰减数据
                                   && CheckProductData(data)               //检查是否符合产品最大最小功率。
                                   && CheckPowersetData(data)              //检查是否符合指定产品的分档要求
                                   && CheckDemandQty(data);                //检查是否满足需求数量。
                        if (bSuccess)
                        {
                            break;
                        }
                        decayCount++;
                    }
                    if (bSuccess)
                    {
                        data.ErrorMessage = string.Format("组件符合产品（{0}-{1}）的分档要求。",
                                                          data.PartNumber, data.ProductCode);
                        break;
                    }
                    else
                    {
                        data.ErrorMessage = string.Format("组件不符合产品（{0}-{1}）的分档要求，尝试检查下一个产品的分档要求。",
                                                          data.PartNumber, data.ProductCode);
                        //下一个产品,判断相同序号下是否还有另外的料号
                        dv.RowFilter = string.Format("ITEM_NO={0} AND PART_NUMBER>'{1}'", itemNo, data.PartNumber);
                        if (dv.Count == 0)
                        {//相同序号下没有另外的料号，找下一个序号的产品料号。
                            dv.RowFilter = string.Format("ITEM_NO>{0}", itemNo);
                        }
                        bLoopCalc = true;
                    }
                }

                if (bSuccess == false)
                {
                    data.ErrorMessage = string.Format("组件（{0}）功率({1})在工单({2})中没有找到符合要求的产品，请联系工艺人员设置。",
                                                       data.LotNo, data.PM, data.WorkOrderNumber);
                    return false;
                }
                return bSuccess;
            }
            else
            {
                DataSet dsPorProductData = this._testDataEntity.GetPorProductData(data.ProductCode);
                if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                {
                    data.ErrorMessage = string.Format("组件({0})获取产品({1})数据出错：{2}",
                                                data.LotNo,
                                                data.ProductCode,
                                                this._testDataEntity.ErrorMsg);
                    return false;
                }

                if (dsPorProductData.Tables[0].Rows.Count <= 0)
                {
                    data.IsPrintErrorMessage = true;
                    data.ErrorMessage = string.Format("组件({0})产品({1})无有效的产品信息，请确认！", data.LotNo, data.ProductCode);
                    return false;
                }
                DataRow dr = dsPorProductData.Tables[0].Rows[0];
                data.ProductKey = Convert.ToString(dr["PRODUCT_KEY"]);
                data.StandCalibration = Convert.ToString(dr["CALIBRATION_TYPE"]);
                data.CalibrationCycle = Convert.ToDouble(dr["CALIBRATION_CYCLE"]);
                data.ProductModel = Convert.ToString(dr["PROMODEL_NAME"]);
                data.MaxPower = Convert.ToDecimal(dr["MAXPOWER"]);
                data.MinPower = Convert.ToDecimal(dr["MINPOWER"]);
                //获取产品({1})规则数据
                DataSet dsTestRule = this._testDataEntity.GetTestRuleData(data.ProductCode);
                if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                {
                    data.ErrorMessage = string.Format("组件({0})获取产品({1})规则数据出错：{2}",
                                                data.LotNo,
                                                data.ProductCode,
                                                this._testDataEntity.ErrorMsg);
                    return false;
                }
                if (dsTestRule.Tables[0].Rows.Count <= 0)
                {
                    data.ErrorMessage = string.Format("产品ID（{0}）无对应的测试规则，请联系工艺！", data.ProductCode);
                    return false;
                }
                data.Digits = Convert.ToInt32(dsTestRule.Tables[0].Rows[0]["POWER_DEGREE"]);
                data.TestRuleCode = dsTestRule.Tables[0].Rows[0]["TESTRULE_CODE"].ToString().Trim();

                DataView dvDecoeffiData = null;
                //判断是否允许重新计算衰减数据。没打印过标签，或允许重新计算衰减数据，则根据新的数据系数进行计算。
                if (data.PSign != "Y" || this._testDataEntity.IsRecalcDecayData(data.LotNo))
                {
                    DataSet dsDecoeffiData = this._testDataEntity.GetDecoeffiData(data.ProductCode, "", data.PM.ToString());
                    if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                    {
                        data.ErrorMessage = string.Format("组件({0})获取工单({1})产品({2})衰减数据出错：{3}",
                                                   data.LotNo,
                                                   data.WorkOrderNumber,
                                                   data.ProductCode,
                                                   this._testDataEntity.ErrorMsg);
                        return false;
                    }
                    dvDecoeffiData = dsDecoeffiData.Tables[0].DefaultView;
                }
                return CalcCoefIVTestData(data, dvDecoeffiData, 0, out bLoopCalc)        //计算衰减数据
                       && CheckProductData(data)       //检查产品信息
                       && CheckPowersetData(data);     //分档检验
            }
        }
        /// <summary>
        /// 检查是否满足产品分档的需求数量。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckDemandQty(PrintLabelParameterData data)
        {
            //没有设置需求数量，不用进行检查。
            if (data.PowersetDemandQty == decimal.MaxValue)
            {
                return true;
            }

            //工单主键、产品料号和分档主键获取工单已生产的产品分档数量。
            decimal productQty = this._testDataEntity.GetWOProductPowersetQty(data.WorkOrderKey, data.PartNumber, data.PowersetKey);
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("获取工单({0})产品({1},{2})分档（{3}）已产出数量时出错：{4}",
                                                data.WorkOrderNumber,
                                                data.PartNumber,
                                                data.ProductCode,
                                                data.PowersetModuleName,
                                                this._testDataEntity.ErrorMsg);
                return false;
            }
            if (productQty >= data.PowersetDemandQty)
            {
                data.ErrorMessage = string.Format("工单({0})产品({1},{2})分档（{3}）已产出数量（{4}）已满足需求数量({5})",
                                                  data.WorkOrderNumber,
                                                  data.PartNumber,
                                                  data.ProductCode,
                                                  data.PowersetModuleName,
                                                  productQty,
                                                  data.PowersetDemandQty);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 计算衰减数据。
        /// 输入：组件序列号、产品ID、功率
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CalcCoefIVTestData(PrintLabelParameterData data, DataView dv, int rowIndex, out bool bLoopCalc)
        {
            //不需要重新计算衰减数据。 add by yibin.fei 2018.1.9
            if (dv == null || (ckPackAttenuation.Enabled == true && ckPackAttenuation.Checked == false))
            {
                bLoopCalc = false;
                return true;
            }
            bLoopCalc = true;
            dv.Sort = "COEFFICIENT DESC";
            dv.RowFilter = "D_NAME='PMAX'";

            if (rowIndex > 0 && rowIndex >= dv.Count)
            {
                bLoopCalc = false;
                return false;
            }

            //如果只有一条衰减系数，当不满足需求时，不再重复计算衰减
            if (rowIndex == 0 && dv.Count == 1)
            {
                bLoopCalc = false;
            }

            if (dv.Count < 1)
            {
                data.CoefPM = data.Round(data.PM);
            }
            else
            {
                string type = Convert.ToString(dv[rowIndex]["DECOEFFI_TYPE"]);
                decimal dCoeff = Convert.ToDecimal(dv[rowIndex]["COEFFICIENT"]);
                //按指定值衰减功率
                if (type == "1")
                {
                    data.CoefPM = dCoeff;
                }
                else //按比例衰减功率
                {
                    data.CoefPM = data.Round(data.PM * dCoeff);
                }
            }

            dv.RowFilter = "D_NAME='ISC'";
            if (dv.Count < 1)
            {
                data.CoefISC = data.ISC;
            }
            else
            {
                string type = Convert.ToString(dv[rowIndex]["DECOEFFI_TYPE"]);
                //按指定值衰减ISC
                if (type == "1")
                {
                    data.CoefISC = data.ISC * (data.CoefPM / data.PM);
                }
                else //按比例衰减ISC
                {
                    decimal dCoeff = Convert.ToDecimal(dv[rowIndex]["COEFFICIENT"]);
                    data.CoefISC = data.ISC * dCoeff;
                }
            }

            dv.RowFilter = "D_NAME='IMAX'";
            if (dv.Count < 1)
            {
                data.CoefIPM = data.IPM;
            }
            else
            {
                string type = Convert.ToString(dv[rowIndex]["DECOEFFI_TYPE"]);
                //按指定值衰减IMAX
                if (type == "1")
                {
                    data.CoefIPM = data.IPM * (data.CoefPM / data.PM);
                }
                else //按比例衰减IMAX
                {
                    decimal dCoeff = Convert.ToDecimal(dv[rowIndex]["COEFFICIENT"]);
                    data.CoefIPM = data.IPM * dCoeff;
                }
            }

            dv.RowFilter = "D_NAME='VOC'";
            if (dv.Count < 1)
            {
                data.CoefVOC = data.VOC;
            }
            else
            {
                string type = Convert.ToString(dv[rowIndex]["DECOEFFI_TYPE"]);
                //按指定值衰减VOC
                if (type == "1")
                {
                    data.CoefVOC = data.VOC * (data.CoefPM / data.PM);
                }
                else //按比例衰减VOC
                {
                    decimal dCoeff = Convert.ToDecimal(dv[rowIndex]["COEFFICIENT"]);
                    data.CoefVOC = data.VOC * dCoeff;
                }
            }

            dv.RowFilter = "D_NAME='VMAX'";
            if (dv.Count < 1)
            {
                data.CoefVPM = data.VPM;
            }
            else
            {
                string type = Convert.ToString(dv[rowIndex]["DECOEFFI_TYPE"]);
                //按指定值衰减VMAX
                if (type == "1")
                {
                    data.CoefVPM = data.VPM * (data.CoefPM / data.PM);
                }
                else //按比例衰减VMAX
                {
                    decimal dCoeff = Convert.ToDecimal(dv[rowIndex]["COEFFICIENT"]);
                    data.CoefVPM = data.VPM * dCoeff;
                }
            }

            dv.RowFilter = "D_NAME='FF'";
            if (dv.Count < 1)
            {
                data.CoefFF = data.FF;
            }
            else
            {
                string type = Convert.ToString(dv[rowIndex]["DECOEFFI_TYPE"]);
                //按指定值衰减FF
                if (type == "1")
                {
                    data.CoefFF = data.FF * (data.CoefPM / data.PM);
                }
                else //按比例衰减FF
                {
                    decimal dCoeff = Convert.ToDecimal(dv[rowIndex]["COEFFICIENT"]);
                    data.CoefFF = data.FF * dCoeff;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查产品数据。
        /// 输入：衰减功率，产品功率范围
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckProductData(PrintLabelParameterData data)
        {
            if (data.CoefPM < data.MinPower || data.CoefPM > data.MaxPower)
            {
                data.IsPrintErrorMessage = true;
                data.ErrorMessage = string.Format("组件（{0})功率({1})不满足产品({2},{3})要求，请确认！",
                                                   data.LotNo, data.CoefPM, data.PartNumber, data.ProductCode);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查并初始化功率分档数据。
        /// 输入：组件序列号、产品ID、衰减后功率，衰减后电流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckPowersetData(PrintLabelParameterData data)
        {
            DataSet dsPowerSet = null;
            if (data.IsEnableByProduct)
            {
                dsPowerSet = this._testDataEntity.GetWOPowerSetData(data.WorkOrderNumber, data.PartNumber, data.LotNo, data.CoefPM);
            }
            else
            {
                dsPowerSet = this._testDataEntity.GetPowerSetData(data.LotNo, data.ProductCode, data.CoefPM.ToString(), "");
            }

            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件({0})获取产品({1},{2})分档数据出错：{3}",
                                               data.LotNo,
                                               data.PartNumber,
                                               data.ProductCode,
                                               this._testDataEntity.ErrorMsg);
                return false;
            }

            if (dsPowerSet.Tables[0].Rows.Count <= 0)
            {
                data.ErrorMessage = string.Format("组件({0})功率({1})在产品（{2},{3}）中无对应分档信息！",
                                                   data.LotNo, data.CoefPM, data.PartNumber, data.ProductCode);
                return false;
            }
            DataRow dr = dsPowerSet.Tables[0].Rows[0];
            if (dr["DEMAND_QTY"] != DBNull.Value && dr["DEMAND_QTY"] != null)
            {
                data.PowersetDemandQty = Convert.ToDecimal(dr["DEMAND_QTY"]);
            }
            else
            {
                data.PowersetDemandQty = decimal.MaxValue;
            }
            data.PowersetKey = Convert.ToString(dr["POWERSET_KEY"]);
            data.PowersetModuleCode = Convert.ToString(dr["PS_SUBCODE"]);
            data.PowersetModuleName = Convert.ToString(dr["MODULE_NAME"]);
            data.PowersetStandardPM = Convert.ToString(dr["PMAXSTAB"]);
            data.PowersetStandardISC = Convert.ToDecimal(dr["ISCSTAB"]);
            data.PowersetStandardIPM = Convert.ToDecimal(dr["IMPPSTAB"]);
            data.PowersetStandardVOC = Convert.ToDecimal(dr["VOCSTAB"]);
            data.PowersetStandardVPM = Convert.ToDecimal(dr["VMPPSTAB"]);
            data.PowersetStandardFuse = Convert.ToDecimal(dr["FUSE"]);
            data.PowersetPowerDifferent = Convert.ToString(dr["POWER_DIFFERENCE"]);
            data.PowersetSeq = Convert.ToInt32(dr["PS_SEQ"]);
            data.PowersetCode = Convert.ToString(dr["PS_CODE"]);
            data.PowersetSubWay = Convert.ToString(dr["SUB_PS_WAY"]);

            data.ArticleNo = string.Empty;
            if (dsPowerSet.ExtendedProperties.ContainsKey("articno"))
            {
                data.ArticleNo = Convert.ToString(dsPowerSet.ExtendedProperties["articno"]);
                data.PowersetModuleCode = data.ArticleNo;
            }
            return true;
        }
        /// <summary>
        /// 检查子分档。
        /// 输入：组件数据，分档数据，衰减后数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckSubPowerset(PrintLabelParameterData data)
        {
            DataSet dsPowersetDetail = null;
            bool isCalibrationPowerDetail = false; //是否校准子分档测试数据

            //获取工单产品属性
            DataSet dsAttr = _lotAfterIVTest.GetOrderAttrByOrderNumber(data.WorkOrderNumber);
            DataRow[] drsCalibration = dsAttr.Tables[0].Select(string.Format(@" ATTRIBUTE_NAME = '{0}' ", "IsCalibrationPowerDetail"));

            //判断进行子分档数据的校准
            if (drsCalibration.Length > 0)
            {
                bool.TryParse(Convert.ToString(drsCalibration[0]["ATTRIBUTE_VALUE"]), out isCalibrationPowerDetail);
            }


            //子分档检验
            switch (data.PowersetSubWay)
            {
                case "功率":
                    if (data.IsEnableByProduct)
                    {
                        dsPowersetDetail = this._testDataEntity.GetWOPowerSetDetailData(data.WorkOrderKey, data.PartNumber, data.PowersetKey, data.CoefPM);
                    }
                    else
                    {
                        dsPowersetDetail = this._testDataEntity.GetPowerSetDetailData(data.ProductCode, data.CoefPM.ToString(), "", "");
                    }
                    if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                    {
                        data.ErrorMessage = string.Format("组件({0})获取产品({1},{2})子分档数据出错：{3}",
                                               data.LotNo,
                                               data.PartNumber,
                                               data.ProductCode,
                                               this._testDataEntity.ErrorMsg);
                        return false;
                    }
                    break;
                case "电流":
                    if (data.IsEnableByProduct)
                    {
                        //判断是否需要进行电流及电压的校准
                        if (isCalibrationPowerDetail)
                        {
                            CalibrationPowerDetail(ref data, "电流");
                        }

                        dsPowersetDetail = this._testDataEntity.GetWOPowerSetDetailData(data.WorkOrderKey, data.PartNumber, data.PowersetKey, data.CoefIPM);
                    }
                    else
                    {
                        dsPowersetDetail = this._testDataEntity.GetPowerSetDetailDataByIMP(data.ProductCode,
                                                                                           data.CoefPM.ToString(),
                                                                                           data.CoefIPM.ToString("##0.00"), "", "");
                    }
                    if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                    {
                        data.ErrorMessage = string.Format("组件({0})获取产品({1},{2})子分档数据出错：{3}",
                                               data.LotNo,
                                               data.PartNumber,
                                               data.ProductCode,
                                               this._testDataEntity.ErrorMsg);
                        return false;
                    }
                    if (dsPowersetDetail.Tables[0].Rows.Count <= 0)
                    {
                        data.ErrorMessage = string.Format("电流[{0}]不满足产品（{1},{2}）功率({3})对应电流子分档要求，请联系巡检确认！",
                                                          data.CoefIPM.ToString("##0.00"),
                                                          data.PartNumber,
                                                          data.ProductCode,
                                                          data.CoefPM);
                        return false;
                    }
                    break;
                default:
                    break;
            }
            if (dsPowersetDetail != null &&
                dsPowersetDetail.Tables.Count > 0 &&
                dsPowersetDetail.Tables[0].Rows.Count > 0)
            {
                data.PowersetSubPowerLevel = Convert.ToString(dsPowersetDetail.Tables[0].Rows[0]["POWERLEVEL"]);
                data.PowersetSubCode = data.IsEnableByProduct
                                        ? Convert.ToString(dsPowersetDetail.Tables[0].Rows[0]["PS_SUB_CODE"])
                                        : Convert.ToString(dsPowersetDetail.Tables[0].Rows[0]["PS_DTL_SUBCODE"]);
            }
            return true;
        }

        /// <summary>
        /// 按照子分档接收要求进行测试数据的修正
        /// </summary>
        /// <param name="data">批次数据</param>
        /// <param name="subWay">子分档方式</param>
        private void CalibrationPowerDetail(ref PrintLabelParameterData data, string subWay)
        {
            DataSet dsPowersetDetailRang = null;
            dsPowersetDetailRang = this._testDataEntity.GetWOPowerSetDetailDataRang(data.WorkOrderKey, data.PartNumber, data.PowersetKey);

            if (dsPowersetDetailRang != null &&
                dsPowersetDetailRang.Tables.Count > 0 &&
                dsPowersetDetailRang.Tables[0].Rows.Count > 0)
            {
                bool isTrueValue = false;

                decimal minValue = 0.00M;
                decimal maxValue = 0.00M;

                decimal.TryParse(Convert.ToString(dsPowersetDetailRang.Tables[0].Rows[0]["PTL_MIN"]), out minValue);
                decimal.TryParse(Convert.ToString(dsPowersetDetailRang.Tables[0].Rows[0]["PTL_MAX"]), out maxValue);

                switch (subWay)
                {
                    case "电流":
                        data.CoefIPM = UpdatePowerDetail(minValue, maxValue, data.IPM);
                        data.CoefVPM = decimal.Round(data.CoefPM / (data.CoefIPM > 0.00M ? data.CoefIPM : 1.00M), 6);
                        break;
                    case "功率":
                        break;
                    default:
                        break;
                }

            }

        }

        /// <summary>
        /// 检查测试数据和上下限的对比，并返回修正后的测试值
        /// </summary>
        /// <param name="minValue">标准下限</param>
        /// <param name="maxValue">标准上限</param>
        /// <param name="testValue">测试数据</param>
        /// <returns>修正后的值</returns>
        private decimal UpdatePowerDetail(decimal minValue, decimal maxValue, decimal testValue)
        {
            //检查如果测试数据满足要求直接返回原测试结果
            if (minValue <= testValue && testValue < maxValue)
            {
                return testValue;
            }
            //如果测试数据小于标准下限返回标准下限
            if (testValue < minValue)
            {
                return minValue;
            }
            //如果测试数据大于标准上限返回（标准上限-0.01）
            if (testValue > maxValue)
            {
                return maxValue - 0.01M;
            }

            return 0.00M;
        }

        /// <summary>
        /// 检查花色ArticleNo。
        /// 输入：组件序列号，ArticleNo
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckColor(PrintLabelParameterData data)
        {
            //花色校验
            DataSet dsWOAttribute = this._testDataEntity.GetWOAttributeValueByLotNum(data.LotNo, "isMustInputModuleColorByCleanOpt");
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件({0})获取工单属性数据出错：{1}",
                                               data.LotNo,
                                               this._testDataEntity.ErrorMsg);
                return false;
            }

            if (dsWOAttribute.Tables[0].Rows.Count > 0)
            {
                bool isMustInputModuleColorByCleanOpt = Convert.ToBoolean(dsWOAttribute.Tables[0].Rows[0]["ATTRIBUTE_VALUE"]);
                if (isMustInputModuleColorByCleanOpt && string.IsNullOrEmpty(data.ArticleNo))
                {
                    data.IsPrintErrorMessage = true;
                    data.ErrorMessage = string.Format("组件（{0}）对应产品({1},{2})分档({3})没有设置花色对应的ArtNo，请联系工艺！",
                                                      data.LotNo, data.PartNumber, data.ProductCode, data.CoefPM);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查标签。初始化标签打印数量
        /// 输入：产品ID，组件序列号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckPrintLable(PrintLabelParameterData data)
        {
            DataSet dsPrintLabelData = null;
            DataView dv = null;
            if (data.IsEnableByProduct)
            {
                //初始化打印标签
                dsPrintLabelData = this._testDataEntity.GetWOPrintLabelData(data.WorkOrderKey, data.PartNumber);
                if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                {
                    data.ErrorMessage = string.Format("组件({0})获取产品({1},{2})标签设置数据出错：{3}",
                                                       data.LotNo,
                                                       data.PartNumber,
                                                       data.ProductCode,
                                                       this._testDataEntity.ErrorMsg);
                    return false;
                }
                dv = dsPrintLabelData.Tables[0].DefaultView;
                dv.Sort = "SEQ DESC";
                //判断当前选择的标签是否是工单产品设置的标签
                dv.RowFilter = "PRINTLABEL_ID='" + data.LabelNo + "'";
                //如果不是，重设选择的标签，以工单产品中设置的第一个功率标签或铭牌进行默认打印。
                if (dv.Count <= 0)
                {
                    //打印铭牌中，从设置中找到的第一个铭牌数据为打印标签。
                    if (data.IsPrintNameplate)
                    {
                        dv.RowFilter = "ISLABEL=1";
                        if (dv.Count <= 0)
                        {
                            data.ErrorMessage = string.Format("产品({0},{1})没有设定打印铭牌，请联系工艺！",
                                                            data.PartNumber, data.ProductCode);
                            return false;
                        }
                    }
                    else
                    {
                        dv.RowFilter = "ISLABEL=0";
                        if (dv.Count <= 0)
                        {
                            data.ErrorMessage = string.Format("产品({0},{1})没有设定打印功率标签，请联系工艺！",
                                                            data.PartNumber, data.ProductCode);
                            return false;
                        }
                    }
                    string oldLableNo = data.LabelNo;
                    string newLableNo = Convert.ToString(dv[0]["PRINTLABEL_ID"]);
                    //data.LabelNo = Convert.ToString(dv[0]["PRINTLABEL_ID"]);
                    data.ErrorMessage = string.Format("选择标签ID({0})不符合组件（{1}）产品({2},{3}）的要求，工艺设置的标签ID是（{4}）,请确认。",
                                                       oldLableNo, data.LotNo, data.PartNumber, data.ProductCode, newLableNo);
                    return false;

                }
            }
            else
            {
                dsPrintLabelData = this._testDataEntity.GetPrintLabelSetInfo(data.ProductCode);
                if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                {
                    data.ErrorMessage = string.Format("组件({0})获取产品({1})标签设置数据出错：{2}",
                                                       data.LotNo,
                                                       data.ProductCode,
                                                       this._testDataEntity.ErrorMsg);
                }
                dv = dsPrintLabelData.Tables[0].DefaultView;
                dv.RowFilter = "VIEW_NAME='" + data.LabelNo + "'";
                if (dv.Count <= 0)
                {
                    data.ErrorMessage = string.Format("当前选择的标签（{0}）非产品设定的打印标签，请联系工艺确认！", data.LabelNo);
                    return false;
                }
            }

            int nPrintQty = Convert.ToInt32(dv[0]["PRINT_QTY"]);
            //包装过来的打印
            if (this.isPrintPalletNo)
            {
                //判断包装打印
                bool isPackageOpt = Convert.ToBoolean(dv[0]["ISPACKAGEPRINT"]);
                //判断是否允许包装工序打印
                if (!isPackageOpt || nPrintQty < 2)
                {
                    data.ErrorMessage = string.Format(@"组件({0})产品({1},{2})打印标签至少设置2张，包装才能打印，请联系工艺！",
                                                      data.LotNo,
                                                      data.PartNumber,
                                                      data.ProductCode);
                    return false;
                }

                if (nPrintQty > 1)
                {
                    nPrintQty = 1;
                }
            }
            //检查主标签是否已打印
            if (data.PSign != "Y")
            {
                dv.RowFilter = "ISMAIN='True'";

                string[] lsMainLableNo = null;

                if (dv.Count > 0)
                {
                    lsMainLableNo = new string[dv.Count];
                    for (int i = 0; i < dv.Count; i++)
                    {
                        lsMainLableNo[i] = data.IsEnableByProduct
                                        ? Convert.ToString(dv[i]["PRINTLABEL_ID"])
                                      : Convert.ToString(dv[i]["VIEW_NAME"]);
                    }
                }
                else
                {
                    data.ErrorMessage = string.Format("组件({0})产品({1},{2})未设定主标签，请联系工艺！",
                                                      data.LotNo, data.PartNumber, data.ProductCode);
                    return false;
                }
                if (lsMainLableNo.Length > 0)
                {
                    bool isMainPrintID = false;

                    //检查标签ID是否为主标签
                    foreach (string sMainLabelNo in lsMainLableNo)
                    {
                        if (data.LabelNo == sMainLabelNo)
                        {
                            isMainPrintID = true;
                        }
                    }

                    if (!isMainPrintID)
                    {
                        bool isPrinted = false;
                        DataSet dsPrintLabelLog = null;

                        foreach (string sMainLabelNo in lsMainLableNo)
                        {
                            //添加 DataSet 判断 如果 dsPrintLabelLog 不为空的话 清空 add by yongbing.yang 2015年6月17日 10:47:38
                            if (dsPrintLabelLog != null)
                            {
                                dsPrintLabelLog.Clear();
                            }

                            dsPrintLabelLog = this._testDataEntity.GetPrintLabelLogData(data.LotNo, sMainLabelNo);
                            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                            {
                                data.ErrorMessage = this._testDataEntity.ErrorMsg;
                                return false;
                            }
                            if (dsPrintLabelLog.Tables[0].Rows.Count > 1)
                            {
                                isPrinted = true;
                            }
                        }

                        if (!isPrinted)
                        {
                            data.ErrorMessage = string.Format("组件({0})产品({1},{2})主标签还未打印，请确认！",
                                                                                             data.LotNo, data.PartNumber, data.ProductCode);
                            return false;
                        }

                    }
                }

            }
            data.PrintQty = nPrintQty;
            //判断是否设置打印CONNERGY侧板标签
            dv.RowFilter = "PRINTLABEL_ID IN ('6','60','39')"; //6,60,39是侧板标签。
            if (dv.Count > 0)
            {
                string s_year = data.TestTime.ToString("yy");
                string s_month = data.TestTime.ToString("MM");
                string s_day = data.TestTime.ToString("dd");
                string s_date = s_year + s_month + s_day;
                data.SlideCode = "21" + data.LotNo + "<FNC1>" + "11" + s_date;
            }
            else
            {
                data.SlideCode = data.LotNo;
            }
            return true;
        }
        /// <summary>
        /// 检查校准信息。校准版类型、校准版信息、校准周期
        /// 输入：标准校准版（产品数据），校准周期（产品数据）、校准版号、设备号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckCalibrationNo(PrintLabelParameterData data)
        {
            if (data.PSign != "Y")
            {
                if (string.IsNullOrEmpty(data.CalibrationNo))
                {
                    data.ErrorMessage = string.Format("组件({0})无对应的校准板信息，请确认！", data.LotNo);
                    return false;
                }
                //string sCalibrationType = data.CalibrationNo.Substring(0, data.CalibrationNo.LastIndexOf('-'));
                if (!data.CalibrationNo.ToUpper().StartsWith(data.StandCalibration.ToUpper()))
                {
                    data.ErrorMessage = string.Format("组件（{0}）校准板类型({2})与工单（{4}）产品({1},{3})设置的校准板类型不符，请联系工艺确认！",
                                                       data.LotNo,
                                                       data.PartNumber,
                                                       data.CalibrationNo,
                                                       data.ProductCode,
                                                       data.WorkOrderNumber);
                    return false;
                }
                DataSet dsCalibrationTTime = this._testDataEntity.GetCalibrationMaxTTime(data.CalibrationNo, data.DeviceNum);
                if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
                {
                    data.ErrorMessage = string.Format("组件（{0}）获取校准版时间出错：{1}",
                                                       data.LotNo,
                                                       this._testDataEntity.ErrorMsg);
                    return false;
                }
                string sMaxTTime = Convert.ToString(dsCalibrationTTime.Tables[0].Rows[0]["TTIME"]);
                if (string.IsNullOrEmpty(sMaxTTime))
                {
                    data.ErrorMessage = string.Format("组件({0})对应的校准板({1})无校准信息，请确认！", data.LotNo, data.CalibrationNo);
                    return false;
                }
                DateTime dtTTime = Convert.ToDateTime(sMaxTTime);
                TimeSpan tsDiftime = data.TestTime.Subtract(dtTTime);
                if (tsDiftime.TotalMinutes > data.CalibrationCycle)
                {
                    data.ErrorMessage = string.Format("组件({0})对应的校准板({1})已过校准周期，请确认！", data.LotNo, data.CalibrationNo);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 计算功率差。
        /// 输入：组件序列号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CalcPowerDifferent(PrintLabelParameterData data)
        {
            DataSet dsIVTestAllData = this._testDataEntity.GetIVTestDateInfo(data.LotNo, "");
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件（{0}）获取测试数据出错：{1}",
                                                   data.LotNo,
                                                   this._testDataEntity.ErrorMsg);
                return false;
            }
            DataView dv = dsIVTestAllData.Tables[0].DefaultView;
            dv.RowFilter = string.Format("TTIME<='{0}'", data.TestTime);
            dv.Sort = "TTIME DESC";
            if (dv.Count > 1)
            {
                decimal dPMLast = Convert.ToDecimal(dv[0]["PM"]);
                decimal dPMPre = Convert.ToDecimal(dv[1]["PM"]);
                data.PowerDifferent = dPMLast - dPMPre;
            }
            return true;
        }
        /// <summary>
        /// 计算CTM值，初始化CTM值
        /// 输入：产品ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CalcCTM(PrintLabelParameterData data)
        {
            DataSet dsProductModel = this._testDataEntity.GetProductModelData(data.ProductCode);
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件（{0}）获取产品型号数据出错：{1}",
                                                   data.LotNo,
                                                   this._testDataEntity.ErrorMsg);
                return false;
            }
            if (dsProductModel.Tables[0].Rows.Count <= 0)
            {
                data.ErrorMessage = string.Format("组件({0})产品型号({1})无数据，请联系工艺确认！", data.LotNo, data.ProductModel);
                return false;
            }
            DataRow drProductModel = dsProductModel.Tables[0].Rows[0];
            decimal dCellArea = Convert.ToDecimal(drProductModel["CELL_AREA"]);
            int nCellNum = Convert.ToInt32(drProductModel["CELL_NUM"]);
            int nCelleffIndex0 = Regex.Match(data.Celleff, @"\d").Index;
            int nCelleffIndex1 = data.Celleff.IndexOf('%');
            int nCelleffIndex2 = data.Celleff.IndexOf('-');
            int nCelleffIndex3 = data.Celleff.LastIndexOf('%');

            decimal lowCelleff = 0;
            if (nCelleffIndex0 >= 0 && nCelleffIndex1 >= 0 && nCelleffIndex1 > nCelleffIndex0)
            {
                lowCelleff = Convert.ToDecimal(data.Celleff.Substring(nCelleffIndex0, nCelleffIndex1 - nCelleffIndex0));
            }
            decimal highCelleff = lowCelleff;
            if (nCelleffIndex3 >= 0 && nCelleffIndex2 >= 0 && nCelleffIndex3 > nCelleffIndex2)
            {
                highCelleff = Convert.ToDecimal(data.Celleff.Substring(nCelleffIndex2 + 1, nCelleffIndex3 - nCelleffIndex2 - 1));
            }
            decimal dCelleff = (lowCelleff + highCelleff) / 2;
            data.CTM = (data.PM / ((dCelleff * dCellArea * nCellNum) / 1000)) * 100;

            return true;
        }
        /// <summary>
        /// 检查控制参数数据
        /// 输入：产品ID、衰减数据、CTM、功率差、测试温度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckControlParaData(PrintLabelParameterData data)
        {
            //获取控制参数数据。
            DataSet dsTestRuleCtlPara = this._testDataEntity.GetTestRuleCtlParaData(data.ProductCode);
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件（{0}）获取控制参数数据出错：{1}",
                                                   data.LotNo,
                                                   this._testDataEntity.ErrorMsg);
                return false;
            }
            //遍历控制参数数据，进行比对校验。
            for (int i = 0; i < dsTestRuleCtlPara.Tables[0].Rows.Count; i++)
            {
                bool bControl = false;
                string sControlObj = Convert.ToString(dsTestRuleCtlPara.Tables[0].Rows[i]["CONTROL_OBJ"]).Trim();
                string sControlType = Convert.ToString(dsTestRuleCtlPara.Tables[0].Rows[i]["CONTROL_TYPE"]).Trim();
                decimal dStanderValue = Convert.ToDecimal(dsTestRuleCtlPara.Tables[0].Rows[i]["CONTROL_VALUE"]);
                switch (sControlObj)
                {
                    case "FF":
                        bControl = CompareValues(sControlType, data.CoefFF, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})FF({1})不满足产品（{2}）测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.CoefFF, data.ProductCode);
                            return false;
                        }
                        break;
                    case "RedLevel":
                        break;
                    case "Isc":
                        bControl = CompareValues(sControlType, data.CoefISC, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})ISC({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.CoefISC, data.ProductCode);
                            return false;
                        }
                        break;
                    case "Voc":
                        bControl = CompareValues(sControlType, data.CoefVOC, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})VOC({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.CoefVOC, data.ProductCode);
                            return false;
                        }
                        break;
                    case "Imp":
                        bControl = CompareValues(sControlType, data.CoefIPM, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})IPM({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.CoefIPM, data.ProductCode);
                            return false;
                        }
                        break;
                    case "Vmp":
                        bControl = CompareValues(sControlType, data.CoefVPM, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})VPM({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.CoefVPM, data.ProductCode);
                            return false;
                        }
                        break;
                    //case "CTM":
                    //    bControl = CompareValues(sControlType, data.CTM, dStanderValue);
                    //    if (bControl == false)
                    //    {
                    //        data.IsPrintErrorMessage = true;
                    //        data.ErrorMessage = string.Format("组件({0})CTM({1})不满足产品（{2})测试规则控制参数设置，请联系工艺确认！",
                    //                                          data.LotNo, data.CTM, data.ProductCode);
                    //        return false;
                    //    }
                    //    break;
                    case "PowerDifferent":
                        bControl = CompareValues(sControlType, data.PowerDifferent, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})功率差({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                               data.LotNo, data.PowerDifferent, data.ProductCode);
                            return false;
                        }
                        break;
                    case "TestTemperature":
                        bControl = CompareValues(sControlType, data.TestTemperature, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})测试温度({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.TestTemperature, data.ProductCode);
                            return false;
                        }
                        break;
                    case "Imp/Isc":
                        decimal dImpIsc = data.CoefIPM / data.CoefISC;
                        bControl = CompareValues(sControlType, dImpIsc, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})测试电流比({1})不满足产品（{2})测试规则控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.ISC, data.ProductCode);
                            return false;
                        }
                        #region 按产品规则设定值
                        //string[] column = new string[] { "name", "IsCheck" };
                        //KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_CheckIscImp");
                        //DataTable lots = BaseData.Get(column, category);
                        //if (lots.Rows[0]["IsCheck"].ToString() == "TRUE")//是否卡控
                        //{
                        //    decimal dImpIsc = data.CoefIPM / data.CoefISC;
                        //    bControl = CompareValues(sControlType, dImpIsc, dStanderValue);
                        //    if (bControl == false)//小于电流比设定值
                        //    {
                        //        DataSet papm = _testDataEntity.GetInefficientPARAM(data.LotNo);//POR_LOT_CELL_PARAM
                        //        DataSet pacp = _testDataEntity.GetProductCp(data.ProductCode, "Inefficient");//BASE_PRODUCTMODEL_CP设定值
                        //        if (papm == null)
                        //        {
                        //            data.IsPrintErrorMessage = true;
                        //            data.ErrorMessage = "没有设定该电流比！";
                        //            return false;
                        //        }
                        //        if (pacp == null)
                        //        {
                        //            data.IsPrintErrorMessage = true;
                        //            data.ErrorMessage = "没有设置该低效片！";
                        //            return false;
                        //        }
                        //        if (papm.Tables[0].Rows[0]["CELLEFFICIENCY"] != null)
                        //        {
                        //            string papmcecy = papm.Tables[0].Rows[0]["CELLEFFICIENCY"].ToString();//Inefficient
                        //            string[] pcrow = papmcecy.Substring(1, papmcecy.Length - 2).Split('-');
                        //            decimal pcrowUp = Convert.ToDecimal(pcrow[1].Substring(0, pcrow[1].Length - 1));//上限
                        //            decimal pcrowDown = Convert.ToDecimal(pcrow[0].Substring(0, pcrow[0].Length - 1));//下限

                        //            bControl = CompareValues(pacp.Tables[0].Rows[0]["CONTROL_TYPE"].ToString(), pcrowUp, Convert.ToDecimal(pacp.Tables[0].Rows[0]["CONTROL_VALUE"]));
                        //            if (bControl == false)//非低效片
                        //            {
                        //                //bControl = CompareValues(pacp.Tables[0].Rows[0]["CONTROL_TYPE"].ToString(), Convert.ToDecimal(pacp.Tables[0].Rows[0]["CONTROL_TYPE"]), pcrowDown);
                        //                data.IsPrintErrorMessage = true;
                        //                data.ErrorMessage = string.Format("组件({0})低效片({1})不满足产品（{2})测试规则控制参数设置，请联系工艺确认！",
                        //                                                  data.LotNo, papmcecy, data.ProductCode);
                        //                return false;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            data.IsPrintErrorMessage = true;
                        //            data.ErrorMessage = string.Format("未设置效率");
                        //            return false;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    data.ErrorMessage = string.Format("卡控设置无效，请重新设置！");
                        //}
                        #endregion
                        break;
                    default:
                        break;
                }
            }
            #region 按产品型号卡控电流比
            bool kbControl = false;
            string[] kcolumn = new string[] { "name", "IsCheck" };
            KeyValuePair<string, string> kcategory = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_CheckIscImp");
            DataTable klots = BaseData.Get(kcolumn, kcategory);
            DateTime dtno = Convert.ToDateTime("2016-01-01");
            if (klots.Rows[0]["IsCheck"].ToString() == "TRUE" && data.ImpIsc_Control != "Y" && data.Cretate_Time > dtno)//是否卡控  是否释放 是否过期返工
            {
                DataSet papmi = _testDataEntity.GetProductCp(data.ProductCode, "Imp/Isc");//BASE_PRODUCTMODEL_CP设定值
                if (papmi == null)
                {
                    data.IsPrintErrorMessage = true;
                    data.ErrorMessage = "没有设定该电流比！";
                    return false;
                }
                decimal dImpIsc = data.Imp_Isc;//data.CoefIPM / data.CoefISC;
                string ksControlType = Convert.ToString(papmi.Tables[0].Rows[0]["CONTROL_TYPE"]).Trim();//Convert.ToString(dsTestRuleCtlPara.Tables[0].Rows[0]["CONTROL_TYPE"]).Trim();
                decimal kdStanderValue = Convert.ToDecimal(papmi.Tables[0].Rows[0]["CONTROL_VALUE"]);//Convert.ToDecimal(dsTestRuleCtlPara.Tables[0].Rows[0]["CONTROL_VALUE"]);
                kbControl = CompareValues(ksControlType, dImpIsc, kdStanderValue);
                if (kbControl == false)//小于电流比设定值
                {
                    DataSet papmn = _testDataEntity.GetInefficientPARAM(data.LotNo);//POR_LOT_CELL_PARAM
                    DataSet pacp = _testDataEntity.GetProductCp(data.ProductCode, "Inefficient");//BASE_PRODUCTMODEL_CP设定值

                    if (pacp == null)
                    {
                        data.IsPrintErrorMessage = true;
                        data.ErrorMessage = "没有设置该低效片！";
                        return false;
                    }
                    if (papmn.Tables[0].Rows[0]["CELLEFFICIENCY"] != null && pacp.Tables[0].Rows.Count > 0)
                    {
                        string papmcecy = papmn.Tables[0].Rows[0]["CELLEFFICIENCY"].ToString().Replace("(", "").Replace(")", "");//Inefficient
                        string[] pcrow = papmcecy.Substring(0, papmcecy.Length).Split('-');//Regex.Matches(papmcecy,"%").Count // papmcecy.replace("(","");papmcecy.Substring(1, papmcecy.Length-2).Split('-');
                        //if (pcrow[0].Length != Encoding.Default.GetByteCount(pcrow[0]) || pcrow[1].Length != Encoding.Default.GetByteCount(pcrow[1])) 
                        //{
                        //    data.IsPrintErrorMessage = true;
                        //    data.ErrorMessage = "转换效率输入存在全角字符，请确认！";
                        //    return false; 
                        //} 
                        decimal pcrowUp = Convert.ToDecimal(pcrow[1].Substring(0, pcrow[1].Length - 1));//上限
                        decimal pcrowDown = Convert.ToDecimal(pcrow[0].Substring(0, pcrow[0].Length - 1));//下限

                        kbControl = CompareValues(pacp.Tables[0].Rows[0]["CONTROL_TYPE"].ToString(), pcrowUp, Convert.ToDecimal(pacp.Tables[0].Rows[0]["CONTROL_VALUE"]));//上限
                        if (kbControl == false)//非低效片
                        {
                            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
                            if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                            {
                                data.IsPrintErrorMessage = true;
                                data.ErrorMessage = IVTestDateObject.ErrorMsg;
                                return false;
                            }
                            string sql = "UPDATE WIP_IV_TEST SET ImpIsc_Control='N' WHERE LOT_NUM='" + data.LotNo + "'";
                            DataSet dsSetDefult = IVTestDateObject.UpdateData(sql, "UpdateIVTestData");
                            int nSetDefult = int.Parse(dsSetDefult.ExtendedProperties["rows"].ToString());
                            if (nSetDefult < 1)
                            {
                                data.IsPrintErrorMessage = true;
                                data.ErrorMessage = "更新低效片信息，无更新记录！";
                                return false;
                            }
                            //bControl = CompareValues(pacp.Tables[0].Rows[0]["CONTROL_TYPE"].ToString(), Convert.ToDecimal(pacp.Tables[0].Rows[0]["CONTROL_TYPE"]), pcrowDown);
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})电流比({1})转换效率({2})不满足产品（{3})测试控制参数设置，请联系巡检确认！",
                                                              data.LotNo, data.Imp_Isc, papmcecy, data.ProductModel);
                            return false;
                        }
                    }
                    else
                    {
                        data.IsPrintErrorMessage = true;
                        data.ErrorMessage = string.Format("未设置效率");
                        return false;
                    }
                }
            }
            else
            {
                //data.ErrorMessage = string.Format("卡控设置无效，请重新设置！");
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 检查功率控制数据。
        /// 输入：产品ID && 衰减数据 && CTM
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckPowerControlData(PrintLabelParameterData data)
        {
            //根据产品获取功率控制数据。
            DataSet dsTestRulePowerCtl = this._testDataEntity.GetTestRulePowerCtlData(data.ProductCode, data.CoefPM.ToString(), "", "");
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件（{0}）获取功率控制数据出错：{1}",
                                                  data.LotNo,
                                                  this._testDataEntity.ErrorMsg);
                return false;
            }
            //遍历功率控制数据，进行校验。
            for (int i = 0; i < dsTestRulePowerCtl.Tables[0].Rows.Count; i++)
            {
                string sPowerCtlObj = Convert.ToString(dsTestRulePowerCtl.Tables[0].Rows[i]["POWERCTL_OBJ"]).Trim();
                string sControlType = Convert.ToString(dsTestRulePowerCtl.Tables[0].Rows[i]["POWERCTL_TYPE"]).Trim();
                decimal dStanderValue = Convert.ToDecimal(dsTestRulePowerCtl.Tables[0].Rows[i]["POWERCTL_VALUE"]);
                switch (sPowerCtlObj)
                {
                    case "CTM":
                        bool bControl = CompareValues(sControlType, data.CTM, dStanderValue);
                        if (bControl == false)
                        {
                            data.IsPrintErrorMessage = true;
                            data.ErrorMessage = string.Format("组件({0})功率({1})对应CTM[{2}]不满足产品({3})测试规则功率参数设置，请联系工艺！",
                                                              data.LotNo,
                                                              data.CoefPM,
                                                              data.CTM,
                                                              data.ProductCode);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        /// <summary>
        /// 组件过站。
        /// </summary>
        /// <returns>true：成功。false：失败。</returns>
        private bool LotTrack(string lotNo, string deviceNum, string psign)
        {
            bool isAutoTrack = false;
            bool isFirstPrintCheckOperationName = false;
            string[] cols = new string[] { "IsAutoTrack", "IsFirstPrintCheckOperationName" };
            DataTable dtPrintLableConfig = BaseData.Get(cols, BASEDATA_CATEGORY_NAME.PrintLabelConfig);
            if (dtPrintLableConfig != null && dtPrintLableConfig.Rows.Count > 0)
            {
                string autoTrack = Convert.ToString(dtPrintLableConfig.Rows[0]["IsAutoTrack"]);
                string firstPrintCheckOperationName = Convert.ToString(dtPrintLableConfig.Rows[0]["IsFirstPrintCheckOperationName"]);
                if (!string.IsNullOrEmpty(autoTrack))
                {
                    isAutoTrack = Convert.ToBoolean(autoTrack);
                }
                if (!string.IsNullOrEmpty(firstPrintCheckOperationName))
                {
                    isFirstPrintCheckOperationName = Convert.ToBoolean(firstPrintCheckOperationName);
                }
            }
            if (isAutoTrack)
            {
                string operationName = string.Empty;
                //根据设备编码获取设备及其所在工序信息。

                if (null != dsEquipments
                    && dsEquipments.Tables.Count > 0
                    && dsEquipments.Tables[0].Rows.Count > 0)
                {
                    operationName = Convert.ToString(dsEquipments.Tables[0].Rows[0]["ROUTE_OPERATION_NAME"]);
                }
                string stepName = string.Empty;
                LotQueryEntity queryEntity = new LotQueryEntity();
                DataSet dsLotInfo = queryEntity.GetLotInfo(lotNo);
                if (string.IsNullOrEmpty(queryEntity.ErrorMsg)
                    && null != dsLotInfo
                    && dsLotInfo.Tables.Count > 0
                    && dsLotInfo.Tables[0].Rows.Count > 0)
                {
                    stepName = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                }

                if (psign != "Y" && isFirstPrintCheckOperationName && stepName != operationName)
                {
                    MessageBox.Show(string.Format("首次打印标签时组件必须在（{2}）工序，目前组件({0})在（{1})工序。", lotNo, stepName, operationName),
                                    "提示");
                    return false;
                }

                //设备所在工序和批次所在工序一致，才进行过站作业。
                if (!string.IsNullOrEmpty(stepName) && stepName == operationName)
                {
                    LotOperationEntity entity = new LotOperationEntity(dsEquipments);
                    bool bReturn = entity.LotTrackInAndLotTrackOut(lotNo, deviceNum);
                    string msg = string.Format("组件过站失败，请重测试或重新过站！\n{0}", entity.ErrorMsg);
                    if (!string.IsNullOrEmpty(entity.ErrorMsg))
                    {
                        LoggingService.Error(msg);
                    }
                    if (bReturn == false)
                    {
                        MessageService.ShowMessage(msg, "提示");
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 保存打印数据到数据库。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SavePrintDataToDatabase(PrintLabelParameterData data)
        {
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            DataSet dsParams = new DataSet();
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("SQL_COL");
            string sql = string.Empty;
            //如果是包装批次列印，则不需要再更新批次数据
            if (!this.isPrintPalletNo || ckPackAttenuation.Checked)
            {
                //更新测试信息
                sql = string.Format(@"UPDATE WIP_IV_TEST 
                                     SET VC_PSIGN='Y',DT_PRINTDT=GETDATE(),C_PSTATE='1',VC_CUSTCODE='{0}',P_NUM=ISNULL(P_NUM,0)+1,COEF_PMAX='{1}',
                                         COEF_ISC='{2}',COEF_VOC='{3}',COEF_IMAX='{4}',
                                         COEF_VMAX='{5}',COEF_FF='{6}',VC_TYPE='{7}',I_IDE='{8}',
                                         VC_MODNAME='{9}',VC_PRINTLABELID='{10}',VC_COGCODE='{11}',DEC_CTM='{12}',
                                         I_PKID='{13}',DEC_PMCHANGE='{14}',VC_WORKORDER='{15}',PRINTEDLABLE=ISNULL(PRINTEDLABLE,0)+1
                                     WHERE VC_DEFAULT='1' AND LOT_NUM='{16}'",
                                    data.CustomerCode, data.CoefPM, data.CoefISC, data.CoefVOC, data.CoefIPM, data.CoefVPM, data.CoefFF,
                                    data.PowersetCode, data.PowersetSeq, data.PowersetModuleName,
                                    data.LabelNo, data.TestRuleCode, data.CTM, data.PowersetSubCode, data.PowerDifferent, data.WorkOrderNumber, data.LotNo);

                dtParams.Rows.Add(sql);
                //更新批次信息
                sql = string.Format(@"UPDATE POR_LOT 
                                   SET LOT_CUSTOMERCODE='{0}',LOT_SIDECODE='{1}',PRO_ID='{3}',PART_NUMBER='{4}',EDIT_TIME=GETDATE(),EDITOR='{5}'
                                   WHERE DELETED_TERM_FLAG!='2' AND LOT_NUMBER='{2}'",
                                    data.CustomerCode,
                                    data.SlideCode,
                                    data.LotNo,
                                    data.ProductCode,
                                    data.PartNumber,
                                    userName);
                dtParams.Rows.Add(sql);

            }
            //记录打印LOG
            string key = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            sql = string.Format(@"INSERT INTO WIP_IV_TEST_PRINTLOG(IV_TEST_DTL_KEY,IV_TEST_KEY,DC_PM,DC_VOC,DC_ISC,DC_VMP,DC_IMP,PRINTDATE,LABELNO,CREATER)
                                 VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE(),'{7}','{8}')",
                                 key, data.IVTestDataKey, data.CoefPM, data.CoefVOC, data.CoefISC, data.CoefVPM, data.CoefIPM, data.LabelNo, userName);
            dtParams.Rows.Add(sql);
            dsParams.Tables.Add(dtParams);
            DataSet dsReturn = this._testDataEntity.SavePrintData(dsParams);
            if (!string.IsNullOrEmpty(this._testDataEntity.ErrorMsg))
            {
                data.ErrorMessage = string.Format("组件（{0}）保存打印数据时出错：{1}",
                                                   data.LotNo,
                                                   this._testDataEntity.ErrorMsg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 比对数字值。
        /// </summary>
        /// <param name="sCalculateType"></param>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public bool CompareValues(string sCalculateType, decimal val1, decimal val2)
        {
            bool bReturn = false;
            try
            {
                switch (sCalculateType)
                {
                    case ">":
                    case "GreaterThan":
                        if (val1 > val2)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            bReturn = false;
                        }
                        break;
                    case "<":
                    case "LessThan":
                        if (val1 < val2)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            bReturn = false;
                        }
                        break;
                    case "=":
                    case "Equal":
                        if (val1 == val2)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            bReturn = false;
                        }
                        break;
                    case ">=":
                    case "GreaterThanEqual":
                        if (val1 >= val2)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            bReturn = false;
                        }
                        break;
                    case "<=":
                    case "LessThanEqual":
                        if (val1 <= val2)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            bReturn = false;
                        }
                        break;
                    case "!=":
                    case "<>":
                    case "NotEqual":
                        if (val1 != val2)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            bReturn = false;
                        }
                        break;
                    default:
                        bReturn = false;
                        break;
                }
            }
            catch //(Exception ex)
            {
                bReturn = false;
            }
            return bReturn;
        }

        private void SavePrintParam()
        {
            //如果是包装工序打印序列号，则不需要客户端配置数据
            //if (isPrintPalletNo) return;

            string sAutoPrint, sLAutoPrint, sPAutoPrint, sLLabelID, sPLabelID, sLX, sLY, sPX, sPY, sLDarkness, sPDarkness, sL300DPI, sP300DPI;
            if (chkAutoPrint.Checked == true)
            {
                sAutoPrint = "T";
            }
            else
            {
                sAutoPrint = "F";
            }
            if (chkAutoPrint01.Checked == true)
            {
                sLAutoPrint = "T";
            }
            else
            {
                sLAutoPrint = "F";
            }
            if (chkAutoPrint02.Checked == true)
            {
                sPAutoPrint = "T";
            }
            else
            {
                sPAutoPrint = "F";
            }
            if (chkDpi01.Checked == true)
            {
                sL300DPI = "T";
            }
            else
            {
                sL300DPI = "F";
            }
            if (chkDpi02.Checked == true)
            {
                sP300DPI = "T";
            }
            else
            {
                sP300DPI = "F";
            }
            sLLabelID = Convert.ToString(luePrintLable01.EditValue);
            sPLabelID = Convert.ToString(luePrintLable02.EditValue);
            sLX = speLeftRight01.Text.Trim();
            sLY = speUpDown01.Text.Trim();
            sPX = speLeftRight02.Text.Trim();
            sPY = speUpDown02.Text.Trim();
            sLDarkness = speTemperature01.Text.Trim();
            sPDarkness = speTemperature02.Text.Trim();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("PrintConfig.xml");
                xmlDoc.SelectSingleNode("//UI/AUTO_PRINT").InnerText = sAutoPrint;
                xmlDoc.SelectSingleNode("//UI/L_AUTO_PRINT").InnerText = sLAutoPrint;
                xmlDoc.SelectSingleNode("//UI/P_AUTO_PRINT").InnerText = sPAutoPrint;
                xmlDoc.SelectSingleNode("//UI/L_LABEL_ID").InnerText = sLLabelID;
                xmlDoc.SelectSingleNode("//UI/P_LABEL_ID").InnerText = sPLabelID;
                xmlDoc.SelectSingleNode("//UI/L_X").InnerText = sLX;
                xmlDoc.SelectSingleNode("//UI/L_Y").InnerText = sLY;
                xmlDoc.SelectSingleNode("//UI/P_X").InnerText = sPX;
                xmlDoc.SelectSingleNode("//UI/P_Y").InnerText = sPY;
                xmlDoc.SelectSingleNode("//UI/L_DARKNESS").InnerText = sLDarkness;
                xmlDoc.SelectSingleNode("//UI/P_DARKNESS").InnerText = sPDarkness;
                xmlDoc.SelectSingleNode("//UI/L_300DPI").InnerText = sL300DPI;
                xmlDoc.SelectSingleNode("//UI/P_300DPI").InnerText = sP300DPI;
                xmlDoc.Save("PrintConfig.xml");
            }
            catch//(Exception ex)
            {
                File.Delete("PrintConfig.xml");
                StreamWriter sw = new StreamWriter("PrintConfig.xml");
                string sParam;
                sParam = "<UI>";
                sParam += "<AUTO_PRINT>F</AUTO_PRINT>";
                sParam += "<L_AUTO_PRINT>F</L_AUTO_PRINT>";
                sParam += "<P_AUTO_PRINT>F</P_AUTO_PRINT>";
                sParam += "<L_LABEL_ID></L_LABEL_ID>";
                sParam += "<P_LABEL_ID></P_LABEL_ID>";
                sParam += "<L_X>0</L_X>";
                sParam += "<L_Y>0</L_Y>";
                sParam += "<P_X>0</P_X>";
                sParam += "<P_Y>0</P_Y>";
                sParam += "<L_DARKNESS>12</L_DARKNESS>";
                sParam += "<P_DARKNESS>12</P_DARKNESS>";
                sParam += "<L_300DPI>F</L_300DPI>";
                sParam += "<P_300DPI>F</P_300DPI>";
                sParam += "<OQA_START_DATE></OQA_START_DATE>";
                sParam += "<OQA_END_DATE></OQA_END_DATE>";
                sParam += "</UI>";
                sw.Write(sParam);
                sw.Close();
            }
            XmlDocument xmlNew = new XmlDocument();
            xmlNew.Load("PrintConfig.xml");
            xmlNew.SelectSingleNode("//UI/AUTO_PRINT").InnerText = sAutoPrint;
            xmlNew.SelectSingleNode("//UI/L_AUTO_PRINT").InnerText = sLAutoPrint;
            xmlNew.SelectSingleNode("//UI/P_AUTO_PRINT").InnerText = sPAutoPrint;
            xmlNew.SelectSingleNode("//UI/L_LABEL_ID").InnerText = sLLabelID;
            xmlNew.SelectSingleNode("//UI/P_LABEL_ID").InnerText = sPLabelID;
            xmlNew.SelectSingleNode("//UI/L_X").InnerText = sLX;
            xmlNew.SelectSingleNode("//UI/L_Y").InnerText = sLY;
            xmlNew.SelectSingleNode("//UI/P_X").InnerText = sPX;
            xmlNew.SelectSingleNode("//UI/P_Y").InnerText = sPY;
            xmlNew.SelectSingleNode("//UI/L_DARKNESS").InnerText = sLDarkness;
            xmlNew.SelectSingleNode("//UI/P_DARKNESS").InnerText = sPDarkness;
            xmlNew.SelectSingleNode("//UI/L_300DPI").InnerText = sL300DPI;
            xmlNew.SelectSingleNode("//UI/P_300DPI").InnerText = sP300DPI;
            xmlNew.Save("PrintConfig.xml");
        }

        public bool CodeSoftPrint(PrintLabelParameterData data)
        {
            IVTestDataEntity entity = new IVTestDataEntity();
            string sLabelPath = AppDomain.CurrentDomain.BaseDirectory + @"CodeSoftLabels\";
            LabelManager2.ApplicationClass lbl = new LabelManager2.ApplicationClass();
            LabelManager2.Document doc;
            try
            {
                lbl.Documents.Open(sLabelPath + data.LabelNo + ".Lab", false);
            }
            catch (Exception ex)
            {
                throw new Exception("无法打开[" + data.LabelNo + ".Lab]请检查CodeSoft 模版文件(" + ex.Message + ")");
            }
            doc = lbl.ActiveDocument;

            try
            {
                DataSet dsCodeSoftLabel = entity.GetCodeSoftLabelSet(data.LabelNo);
                if (dsCodeSoftLabel.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsCodeSoftLabel.Tables[0].Rows.Count; i++)
                    {
                        string sParam = Convert.ToString(dsCodeSoftLabel.Tables[0].Rows[i]["PARAMETER"]);
                        string sParaType = Convert.ToString(dsCodeSoftLabel.Tables[0].Rows[i]["PARAMETER_TYPE"]);

                        switch (sParam)
                        {
                            case "SN":
                                doc.Variables.FormVariables.Item("SN").Value = data.LotNo;
                                break;
                            case "COEF_PMAX":
                                doc.Variables.FormVariables.Item("COEF_PMAX").Value = data.CoefPM.ToString(sParaType);
                                break;
                            case "COEF_ISC":
                                doc.Variables.FormVariables.Item("COEF_ISC").Value = data.CoefISC.ToString(sParaType);
                                break;
                            case "COEF_VOC":
                                doc.Variables.FormVariables.Item("COEF_VOC").Value = data.CoefVOC.ToString(sParaType);
                                break;
                            case "COEF_IMAX":
                                doc.Variables.FormVariables.Item("COEF_IMAX").Value = data.CoefIPM.ToString(sParaType);
                                break;
                            case "COEF_VMAX":
                                doc.Variables.FormVariables.Item("COEF_VMAX").Value = data.CoefVPM.ToString(sParaType);
                                break;
                            case "MODULE_NAME":
                                doc.Variables.FormVariables.Item("MODULE_NAME").Value = data.ProductModel.ToString();
                                break;
                            case "PS_SUBCODE":
                                doc.Variables.FormVariables.Item("PS_SUBCODE").Value = data.PowersetModuleCode.ToString();
                                break;
                            case "PMAXSTAB":
                                doc.Variables.FormVariables.Item("PMAXSTAB").Value = data.PowersetStandardPM.ToString();
                                break;
                            case "ISCSTAB":
                                doc.Variables.FormVariables.Item("ISCSTAB").Value = data.PowersetStandardISC.ToString(sParaType);
                                break;
                            case "VOCSTAB":
                                doc.Variables.FormVariables.Item("VOCSTAB").Value = data.PowersetStandardVOC.ToString(sParaType);
                                break;
                            case "IMPPSTAB":
                                doc.Variables.FormVariables.Item("IMPPSTAB").Value = data.PowersetStandardIPM.ToString(sParaType);
                                break;
                            case "VMPPSTAB":
                                doc.Variables.FormVariables.Item("VMPPSTAB").Value = data.PowersetStandardVPM.ToString(sParaType);
                                break;
                            case "FUSE":
                                doc.Variables.FormVariables.Item("FUSE").Value = data.PowersetStandardFuse.ToString(sParaType);
                                break;
                            case "PM":
                                doc.Variables.FormVariables.Item("PM").Value = data.PM.ToString(sParaType);
                                break;
                            case "ISC":
                                doc.Variables.FormVariables.Item("ISC").Value = data.ISC.ToString(sParaType);
                                break;
                            case "IPM":
                                doc.Variables.FormVariables.Item("IPM").Value = data.IPM.ToString(sParaType);
                                break;
                            case "VOC":
                                doc.Variables.FormVariables.Item("VOC").Value = data.VOC.ToString(sParaType);
                                break;
                            case "VPM":
                                doc.Variables.FormVariables.Item("VPM").Value = data.VPM.ToString(sParaType);
                                break;
                            case "T_DATE":
                                doc.Variables.FormVariables.Item("T_DATE").Value = data.TestTime.Date.ToString(sParaType);
                                break;
                            case "POWER_DIFFERENCE":
                                doc.Variables.FormVariables.Item("POWER_DIFFERENCE").Value = data.PowersetPowerDifferent;
                                break;
                            case "POWERLEVEL":
                                doc.Variables.FormVariables.Item("POWERLEVEL").Value = data.PowersetSubPowerLevel;
                                break;
                            case "PROMODEL_NAME":
                                doc.Variables.FormVariables.Item("PROMODEL_NAME").Value = data.ProductModel;
                                break;
                            default:
                                break;
                        }
                    }
                }
                doc.PrintDocument(data.PrintQty);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                lbl.Quit();
            }
            return false;
        }

        /// <summary>
        /// 打印标签时判定Ctm值是否在工单设定的范围内，如果相同则还需打印则锁定批次，如果不打印不产生任何结果
        /// 输入：产品ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckCTMByEff(PrintLabelParameterData data)
        {
            if (!CheckFacTrueOrFalse(data))
                return true;
            int nCelleffIndex0 = Regex.Match(data.Celleff, @"\d").Index;
            int nCelleffIndex1 = data.Celleff.IndexOf('%');
            int nCelleffIndex2 = data.Celleff.IndexOf('-');
            int nCelleffIndex3 = data.Celleff.LastIndexOf('%');

            decimal lowCelleff = 0;
            if (nCelleffIndex0 >= 0 && nCelleffIndex1 >= 0 && nCelleffIndex1 > nCelleffIndex0)
            {
                lowCelleff = Convert.ToDecimal(data.Celleff.Substring(nCelleffIndex0, nCelleffIndex1 - nCelleffIndex0));
            }
            decimal highCelleff = lowCelleff;
            if (nCelleffIndex3 >= 0 && nCelleffIndex2 >= 0 && nCelleffIndex3 > nCelleffIndex2)
            {
                highCelleff = Convert.ToDecimal(data.Celleff.Substring(nCelleffIndex2 + 1, nCelleffIndex3 - nCelleffIndex2 - 1));
            }

            DataSet dsProWoCtm = this._testDataEntity.GetCtmInfByWorderEffCtm(data.WorkOrderKey, Convert.ToDecimal(lowCelleff.ToString("0.00")), Convert.ToDecimal(highCelleff.ToString("0.00")), Convert.ToDecimal(data.CTM.ToString("#0.00")));
            if (dsProWoCtm != null && dsProWoCtm.Tables.Count > 0)
            {
                if (Convert.ToInt32(dsProWoCtm.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    #region 锁定批次
                    //if (MessageBox.Show(StringParser.Parse("CTM值不在工艺维护范围或工艺为对该工单做Ctm值维护，确认需要打印？"),
                    //   StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                    //{
                    //    //锁定批次
                    //    #region/// 绑定批次信息。
                    //    LotQueryEntity queryEntity = new LotQueryEntity();
                    //    DataSet dsReturn = queryEntity.GetLotInfo(this.txtLotNum.Text.Trim());
                    //    #endregion
                    //    string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
                    //    string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                    //    DataSet dsParams = new DataSet();
                    //    //存放待暂停的批次的操作数据
                    //    WIP_TRANSACTION_FIELDS transFields = new WIP_TRANSACTION_FIELDS();
                    //    DataTable dtTransaction = CommonUtils.CreateDataTable(transFields);
                    //    foreach (DataRow dr in dsReturn.Tables[0].Rows)
                    //    {
                    //        //组织待暂停的批次的操作数据
                    //        DataRow drTransaction = dtTransaction.NewRow();
                    //        dtTransaction.Rows.Add(drTransaction);
                    //        string transKey = CommonUtils.GenerateNewKey(0);
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = dr[POR_LOT_FIELDS.FIELD_LOT_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD;
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = dr[POR_LOT_FIELDS.FIELD_QUANTITY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = dr[POR_LOT_FIELDS.FIELD_QUANTITY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = dr[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = dr[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = dr[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = dr[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY] = string.Empty;
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = "A";
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = dr[POR_LOT_FIELDS.FIELD_STATE_FLAG];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = dr[POR_LOT_FIELDS.FIELD_IS_REWORKED];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR] = "系统工艺(CTM自动卡控)";
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER] = oprComputer;
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = dr[POR_LOT_FIELDS.FIELD_OPR_LINE];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE] = dr[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY] = dr[POR_LOT_FIELDS.FIELD_EDC_INS_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = "CTM实测值与工艺设定工单设定的值不匹配";
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = "系统工艺(CTM自动卡控)";
                    //        //用于暂存序列号批次信息最后的编辑时间，以便判断序列号信息是否过期。
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = dr[POR_LOT_FIELDS.FIELD_EDIT_TIME];
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
                    //        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP] = DBNull.Value;
                    //    }
                    //    //存放暂停操作的明细记录。
                    //    LotOperationEntity _entityReason = new LotOperationEntity();
                    //    DataSet dsReturnholeReason = _entityReason.GetHoldReasonCodeCategory();
                    //    if (dsReturnholeReason == null || dsReturnholeReason.Tables.Count < 1 || dsReturnholeReason.Tables[0].Rows.Count < 1)
                    //    {
                    //        MessageService.ShowError("请工艺维护原因代码");
                    //        return false;
                    //    }
                    //    DataSet dsReasonCode = _entityReason.GetReasonCode(dsReturnholeReason.Tables[0].Rows[0]["REASON_CODE_CATEGORY_KEY"].ToString());

                    //    DataTable dtHoldParams = new DataTable();
                    //    dtHoldParams.Columns.Add("TRANSACTION_KEY", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("HOLD_TIME", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("IS_RELEASE", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("RELEASE_TRANSACTION_KEY", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("RELEASE_OPERATOR", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("RELEASE_TIME", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("RELEASE_TIMEZONE", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("RELEASE_DESCRIPTION", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("EDIT_TIME", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("REASON_CODE_CATEGORY_KEY", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("REASON_CODE_CATEGORY_NAME", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("REASON_CODE_KEY", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("REASON_CODE_NAME", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("HOLD_DESCRIPTION", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add("HOLD_PASSWORD", Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add(WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_OPERATOR, Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add(WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIMEZONE, Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add(WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR, Type.GetType("System.String"));
                    //    dtHoldParams.Columns.Add(WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE, Type.GetType("System.String"));
                    //    DataRow drhold = dtHoldParams.NewRow();
                    //    drhold["TRANSACTION_KEY"] = "";
                    //    drhold["HOLD_TIME"] = "";
                    //    drhold["IS_RELEASE"] = 0;
                    //    drhold["RELEASE_TRANSACTION_KEY"] = "";
                    //    drhold["RELEASE_OPERATOR"] = "";
                    //    drhold["HOLD_PASSWORD"] = "";
                    //    drhold["RELEASE_TIME"] = "";
                    //    drhold["RELEASE_TIMEZONE"] = "";
                    //    drhold["RELEASE_DESCRIPTION"] = "";
                    //    drhold["EDIT_TIME"] = "";
                    //    drhold["REASON_CODE_CATEGORY_KEY"] = dsReturnholeReason.Tables[0].Rows[0]["REASON_CODE_CATEGORY_KEY"].ToString();
                    //    drhold["REASON_CODE_CATEGORY_NAME"] = dsReturnholeReason.Tables[0].Rows[0]["REASON_CODE_CATEGORY_NAME"].ToString();
                    //    drhold["REASON_CODE_KEY"] = dsReasonCode.Tables[0].Rows[0]["REASON_CODE_KEY"].ToString();
                    //    drhold["REASON_CODE_KEY"] = dsReasonCode.Tables[0].Rows[0]["REASON_CODE_NAME"].ToString();
                    //    drhold["HOLD_DESCRIPTION"] = "CTM实测值与工艺设定工单设定的值不匹配";
                    //    drhold["HOLD_PASSWORD"] = "";
                    //    drhold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_OPERATOR] = "系统工艺(CTM自动卡控)";
                    //    drhold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIMEZONE] = timezone;
                    //    drhold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR] = "系统工艺(CTM自动卡控)";
                    //    drhold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
                    //    dtHoldParams.Rows.Add(drhold);

                    //    dsParams.Tables.Add(dtTransaction);
                    //    dsParams.Tables.Add(dtHoldParams);
                    //    //执行暂停批次。
                    //    this._entity.LotHold(dsParams);
                    //    if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                    //    {
                    //        MessageService.ShowError(this._entity.ErrorMsg);
                    //        return false;
                    //    }
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    #endregion
                    data.ErrorMessage = string.Format("CTM值不在工艺维护范围或工艺为对该工单做Ctm值维护,再打印！（效率档:{0}; CTM实际值:{1}）", data.Celleff, data.CTM.ToString("#0.00"));
                    //MessageBox.Show("CTM值不在工艺维护范围或工艺为对该工单做Ctm值维护,再打印！", "系统提示");
                    return false;
                }
            }

            return true;
        }
        private bool CheckFacTrueOrFalse(PrintLabelParameterData data)
        {
            //1.判定基础数据工厂是否设定自动上料：是 继续  否 return
            string[] columns = new string[] { "FAC_NAME", "FAC_CODE", "STATUS" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Wip_IV_TEST_CTM");
            DataTable dtFac = BaseData.Get(columns, category);
            DataRow[] drs = dtFac.Select(string.Format("FAC_CODE='{0}'", data.FactoryName));
            DataTable dt = dtFac.Clone();
            foreach (DataRow dr in drs)
                dt.ImportRow(dr);
            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["STATUS"].ToString().ToUpper() == "TRUE")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

    }
}