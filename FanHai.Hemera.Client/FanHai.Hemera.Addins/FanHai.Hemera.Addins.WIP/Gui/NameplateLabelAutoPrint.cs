using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Addins.WIP.Report;
using FanHai.Gui.Core;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraReports.UI;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class NameplateLabelAutoPrint : UserControl
    {
        //LotQueryEntity _entity = new LotQueryEntity();
        //LotAfterIvTestEntity _lotAfterIvTestEntity = new LotAfterIvTestEntity();
        NameplateLabelAutoPrintEntity namePlateLabelAutoPrint = new NameplateLabelAutoPrintEntity();
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示


        StringBuilder sb = new StringBuilder();

        string NamePlatePrinter = string.Empty;
        string LabelPrinter = string.Empty;
    

        public NameplateLabelAutoPrint()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.btnPrint.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.btnPrint}");//"打印";
            this.layoutControlItem3.Text = StringParser.Parse("${res:Global.XuLieHao}");// 序列号
            this.labelControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.labelControl1}");//"铭牌自动打印";
            this.layoutControlGroup2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.layoutControlGroup2}");//"基础配置";
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.layoutControlItem1}");//"横向X坐标";
            this.layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.layoutControlItem2}");//"纵向Y坐标";
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.layoutControlItem5}");//"铭牌打印机";
        }




        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strX = txtX.Text.Trim();
            string strY = txtY.Text.Trim();
            int X = 0;
            int Y = 0;

            if (!string.IsNullOrEmpty(strX) || !string.IsNullOrEmpty(strY))
            {
                X = Convert.ToInt32(strX);
                Y = Convert.ToInt32(strY);
            }
            string lotNum = txtLotNum.Text.Trim();
            Print(lotNum,NamePlatePrinter,X,Y);
        }

        public void Print(string lotNum,string printer,int X,int Y){

            string pordId = string.Empty; //id
            string _orderNumner = string.Empty; //工单
            string _partName = string.Empty;  //料号

            string cellType = string.Empty; //电池片类型
            string noct = string.Empty; //Noct值
            string maxPower = string.Empty; //系统最大电压值
            string power = string.Empty; //额定功率
            string Voc = string.Empty; //开路电压
            string Vmp = string.Empty; //额定电压
            string Isc = string.Empty; //短路电流
            string Imp = string.Empty; //额定电流
            string Fuse = string.Empty; //填充因子
            string toleRance = string.Empty; //分档方式
            string proModelName = string.Empty;//产品类型
            string labelType = string.Empty; //认证类型
            string version = string.Empty; //认证版本
            string template = string.Empty; //模板



            DataRowCollection drcInfo  = null;
            DataSet dsInfo = null;
 

            if (string.IsNullOrEmpty(lotNum))
            {
                MessageBox.Show(string.Format("【{0}】: 序列号不能为空", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                //sb.AppendLine(string.Format("【{0}】: 序列号不能为空", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                //txtLog.Text = sb.ToString();
                return;
            }

            //try
            //{
            //    DataSet dsInfo = namePlateLabelAutoPrint.GetInfoForNamepalteLabelAutoPrint(lotNum);

             
            //    drcInfo = dsInfo.Tables[0].Rows;
            //    if (drcInfo.Count <= 0)
            //    {
            //        sb.AppendLine(string.Format("【{0}】: 序列号【{1}】无对应信息，请确认序列号正确"));
            //        txtLog.Text = sb.ToString();
            //        return;
            //    }
            //    pordId = drcInfo[0]["PRODUCT_NAME"].ToString();
            //    _orderNumner = drcInfo[0]["ORDER_NUMBER"].ToString();
            //    _partName = drcInfo[0]["PART_NUMBER"].ToString();
            //    power = drcInfo[0]["PMAXSTAB"].ToString();
            //    noct = drcInfo[0]["TemNoct"].ToString();
            //    cellType = drcInfo[0]["CELLTYPE"].ToString();
            //    maxPower = drcInfo[0]["MAXPOWER"].ToString();
            //    Voc = drcInfo[0]["VOCSTAB"].ToString();
            //    Vmp = drcInfo[0]["VMPPSTAB"].ToString();
            //    Isc = drcInfo[0]["ISCSTAB"].ToString();
            //    Imp = drcInfo[0]["IMPPSTAB"].ToString();
            //    Fuse = drcInfo[0]["FUSE"].ToString();
            //    toleRance = drcInfo[0]["TOLERANCE"].ToString();
            //    proModelName = drcInfo[0]["PROMODEL_NAME"].ToString();
            //    labelType = drcInfo[0]["LABELTYPE"].ToString();
            //    version = drcInfo[0]["LABELVAR"].ToString();

            //    DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(pordId);
            //    template = dsTemplate.Tables[0].Rows[0]["TEMPLATE"].ToString();
            //}
            //catch (Exception ex)
            //{
            //    sb.AppendLine(ex.Message);
            //    txtLog.Text = sb.ToString();
            //    return;
            //}

            //铭牌体现功率档位 yibi.fei 2017.10.26
            try
            {
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(lotNum);
                dsInfo = namePlateLabelAutoPrint.GetInfoForNamepalteLabelAutoPrint(lotNum);
                drcInfo = dsInfo.Tables[0].Rows;
                power = drcInfo[0]["PMAXSTAB"].ToString();
                string strWorkNumber = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                string strSAP_NO = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                DataSet ds_powershow = _testDataEntity.GetPowerShowData(strWorkNumber, strSAP_NO);
                if (ds_powershow.Tables[0].Rows.Count > 0)
                {

                    DataRow[] drPowerShow = ds_powershow.Tables[0].Select(string.Format("BEFORE_POWER={0}", power));
                    if (drPowerShow.Count() > 0 && power == drPowerShow[0]["BEFORE_POWER"].ToString())
                    {

                        power = drPowerShow[0]["AFTER_POWER"].ToString();
                        //获取五大参数
                        dsInfo = namePlateLabelAutoPrint.getInfoForNamepalteLabelAutoPrintForPowerShow(lotNum, power);

                    }

                }

                drcInfo = dsInfo.Tables[0].Rows;
                if (drcInfo.Count <= 0)
                {
                    //sb.AppendLine(string.Format("【{0}】: 序列号【{1}】无对应信息，请确认序列号正确"));
                    MessageBox.Show(string.Format("序列号【{0}】无对应信息，请确认序列号正确",lotNum));
                    //txtLog.Text = sb.ToString();
                    return;
                }
                pordId = drcInfo[0]["PRODUCT_NAME"].ToString();
                _orderNumner = drcInfo[0]["ORDER_NUMBER"].ToString();
                _partName = drcInfo[0]["PART_NUMBER"].ToString();
                power = drcInfo[0]["PMAXSTAB"].ToString();
                noct = drcInfo[0]["TemNoct"].ToString();
                cellType = drcInfo[0]["CELLTYPE"].ToString();
                maxPower = drcInfo[0]["MAXPOWER"].ToString();
                Voc = drcInfo[0]["VOCSTAB"].ToString();
                Vmp = drcInfo[0]["VMPPSTAB"].ToString();
                Isc = drcInfo[0]["ISCSTAB"].ToString();
                Imp = drcInfo[0]["IMPPSTAB"].ToString();
                Fuse = drcInfo[0]["FUSE"].ToString();
                toleRance = drcInfo[0]["TOLERANCE"].ToString();
                proModelName = drcInfo[0]["PROMODEL_NAME"].ToString();
                labelType = drcInfo[0]["LABELTYPE"].ToString();
                version = drcInfo[0]["LABELVAR"].ToString();

                DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(pordId);
                template = dsTemplate.Tables[0].Rows[0][POR_PRODUCT.FIELDS_NAME_TEMPLATE].ToString();

               
            }
            catch (Exception ex)
            {
                //sb.AppendLine(ex.Message);
                //txtLog.Text = sb.ToString();
                MessageBox.Show(string.Format("【{0}】未维护打印模板，请联系NPI", pordId));
                return;
            }






            string pscode = string.Empty; 
            string code = string.Empty;
            try
            {


                

             if ("SOLAR".Equals(template))
                {
                    pscode = pordId;
                    int i = pscode.LastIndexOf("-");
                    pscode = pscode.Substring(0, i);


                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
                + version;

                    NameplateLabel_SOLAR_JUICE reportSOLAR_JUICE = new NameplateLabel_SOLAR_JUICE(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    reportSOLAR_JUICE.PrinterName = printer;
                    reportSOLAR_JUICE.Print();
                   


                }


                if ("TUV_OLD".Equals(template))
                {
                    pscode = pordId;
                    int i = pscode.LastIndexOf("-");
                    pscode = pscode.Substring(0, i);


                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
                + version;

                    NameplateLabel_TUVFH_NEW reportTUVFH_NEW = new NameplateLabel_TUVFH_NEW(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y, false);
                    reportTUVFH_NEW.PrinterName = printer;
                    reportTUVFH_NEW.Print();
                    //NameplateLabel_TUV reportTUV = new NameplateLabel_TUV(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    //reportTUV.PrinterName = printer;
                    //reportTUV.Print();


                }
                if ("CEC".Equals(template))
                {
                    pscode = pordId;
                    int i = pscode.LastIndexOf("-");
                    pscode = pscode.Substring(0, i);


                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
                + version;

                    if (pordId.Contains("HV"))
                    {
                        NameplateLabel_TUVFH_CEC1500 reportTUVFH_CEC1500 = new NameplateLabel_TUVFH_CEC1500(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y, true);
                        reportTUVFH_CEC1500.PrinterName = printer;
                        reportTUVFH_CEC1500.Print();
                    }
                    else
                    {

                        NameplateLabel_TUVFH_CEC1000 reportTUVFH_CEC1000 = new NameplateLabel_TUVFH_CEC1000(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y, true);
                        reportTUVFH_CEC1000.PrinterName = printer;
                        reportTUVFH_CEC1000.Print();
                    }
                    //NameplateLabel_TUV reportTUV = new NameplateLabel_TUV(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    //reportTUV.PrinterName = printer;
                    //reportTUV.Print();
                

                }
                if ("TUV".Equals(template))
                {
                    pscode = pordId;
            int i = pscode.LastIndexOf("-");
            pscode = pscode.Substring(0, i);


                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
                + version;

                    //string typeTUV = string.Empty;
                    if (pordId.Contains("HV"))
                    {
                        NameplateLabel_TUV01 reportTUV01 = new NameplateLabel_TUV01(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                        reportTUV01.PrinterName = printer;
                        reportTUV01.Print();
                    }
                    else if (pordId.Contains("HC"))
                    {
                        
                        NameplateLabel_TUV01 reportTUV01 = new NameplateLabel_TUV01(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                        reportTUV01.PrinterName = printer;
                        reportTUV01.Print();
                    }
                    else if (pordId.Contains("DG") || pordId.Contains("DGT"))
                    {
                        NameplateLabel_TUV02 reportTUV02 = new NameplateLabel_TUV02(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                        reportTUV02.PrinterName = printer;
                        reportTUV02.Print();
                    }
                    //else if (pordId.Contains("FR"))
                    //{
                    //    NameplateLabel_TUVFH reportTUVFH = new NameplateLabel_TUVFH(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    //    reportTUVFH.PrinterName = printer;
                    //    reportTUVFH.Print();
                    //}
                     
                    else
                    {
                        NameplateLabel_TUVFH_NEW reportTUVFH_NEW = new NameplateLabel_TUVFH_NEW(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y,true);
                        reportTUVFH_NEW.PrinterName = printer;
                        reportTUVFH_NEW.Print();
                        //NameplateLabel_TUV_New report100 = new NameplateLabel_TUV_New(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                        //report100.PrinterName = printer;
                        //report100.Print();
                    }
                }


                if ("ASM".Equals(template))
                {

                    pscode = "ASM" + proModelName.ToUpper().Trim() + "C-" + power.ToUpper().Trim();
                    code = proModelName.ToUpper().Trim() + "C"
               + power.ToUpper().Trim()
               + labelType.ToUpper().Trim()
               + version;

                    NameplateLabel_ASM report = new NameplateLabel_ASM(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y,false);
                    report.PrinterName = printer;
                    report.Print();

                }

                if ("ASM_NEW".Equals(template))
                {

                    pscode = "ASM" + proModelName.ToUpper().Trim() + "C-" + power.ToUpper().Trim();
                    code = proModelName.ToUpper().Trim() + "C"
               + power.ToUpper().Trim()
               + labelType.ToUpper().Trim()
               + version;

                    NameplateLabel_ASM report = new NameplateLabel_ASM(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y, true);
                    report.PrinterName = printer;
                    report.Print();

                }
                if ("CQC".Equals(template))
                {
                    pscode = "CHSM" + proModelName.ToUpper().Trim() + "-" + power.ToUpper().Trim();

                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
              + version;

                    NameplateLabel_CQC report = new NameplateLabel_CQC(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    report.PrinterName = printer;
                    report.Print();

                }
                if ("CGCLPZ".Equals(template))
                {
                    //pscode = "CHSM" + proModelName.ToUpper().Trim() + "-" + power.ToUpper().Trim();
                    pscode = pordId;
                    int i = pscode.LastIndexOf("-");
                    pscode = pscode.Substring(0, i) + "-" + power.ToUpper().Trim();

                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
              + version;

                    NameplateLabel_CGClpz report = new NameplateLabel_CGClpz(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    report.PrinterName = printer;
                    report.Print();

                }
                //安能铭牌打印  yibin.fei 2017.11.14
                if ("CQC_AN".Equals(template))
                {
                    pscode = "CHSM" + proModelName.ToUpper().Trim() + "-" + power.ToUpper().Trim();

                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
              + version;

                    NameplateLabel_CQC_AnNeng report = new NameplateLabel_CQC_AnNeng(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    report.PrinterName = printer;
                    report.Print();

                }

                if ("CQCLPZ".Equals(template))
                {
                    pscode = "CHSM" + proModelName.ToUpper().Trim() + "-" +power;

                    
                    string _effCQClpz = string.Empty;     //转换效率
                    string _mianjiCQClpz = string.Empty;    //组件面积
                    string _qtyCQClpz = string.Empty;         //电池片数量

                    string[] l_s = new string[] { "ProModelName", "Power", "CellQuantity", "Area", "Efficiency" };
                    string category = "NameplateLabelAutoPrint_CQCLPZ";
                    DataTable dtCommon = BaseData.Get(l_s, category);
                    DataRow[] dr = dtCommon.Select(string.Format("ProModelName='{0}' AND Power='{1}'",proModelName,power));
                    if (dr.Count() > 0)
                    {
                        _effCQClpz = dr[0]["Efficiency"].ToString();
                        _mianjiCQClpz = dr[0]["Area"].ToString();
                        _qtyCQClpz = dr[0]["CellQuantity"].ToString();
                    }
                    else
                    {
                        
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.Msg001}"));//请联系MES小组在基础数据维护--【NameplateLabelAutoPrint_CQCLPZ】添加相应的信息
                        //MessageBox.Show("请联系MES小组在基础数据维护--【NameplateLabelAutoPrint_CQCLPZ】添加相应的信息");
                        return;
                    }

                    NameplateLabel_CQClpz report = new NameplateLabel_CQClpz(pscode, power, _effCQClpz, _mianjiCQClpz, _qtyCQClpz, X, Y);
                    report.PrinterName = printer;
                    report.Print();
                }

                if ("NE01".Equals(template))
                {
                    
                    string ckg = string.Empty; //长宽高
                    //if (proModelName.Contains("6610"))
                    //{
                    //    ckg = "1650mm×991mm×40mm";
                    //}
                    //else if (proModelName.Contains("6612"))
                    //{

                    //    ckg = "1956mm×991mm×45mm";
                    //}



                    //if (proModelName.Contains("M"))
                    //{
                    //    cellType = "単結晶Si";
                    //}
                    //else if (proModelName.Contains("P"))
                    //{
                    //    cellType = "多結晶Si";
                    //}


                    NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                    DataSet ds = namePlateLabelPrint.GetCellTypeByWorkOrderNumber(_orderNumner);
                    //
                  
                    string category = "AutoPrint_NE01";
                    string[] l_s = new string[] { "Type", "CKG", "CODE", "PROMODEL_NAME", "CELL_TYPE" };
                    DataTable dtCommon = BaseData.Get(l_s, category);
                   

                    //





                    string flagType = string.Empty;
                    if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                    {
                        //if (ds.Tables[0].Rows[0]["CELL_TYPE"].ToString().Contains("-N"))
                        //{
                        //    flagType = "-N";
                        //}
                        //if (ds.Tables[0].Rows[0]["CELL_TYPE"].ToString().Contains("-P"))
                        //{
                        //    flagType = "-P";
                        //}
                        string CELL_TYPE = ds.Tables[0].Rows[0]["CELL_TYPE"].ToString().Substring(4,2);

                         DataRow[] dr = dtCommon.Select(string.Format("CELL_TYPE='{0}' AND PROMODEL_NAME='{1}'", CELL_TYPE, proModelName));
                         if (dr.Count() > 0)
                         {
                             ckg = dr[0]["CKG"].ToString();
                             cellType = dr[0]["Type"].ToString();
                             code = dr[0]["CODE"].ToString() + power + (CELL_TYPE == "-N" ? "W" : "");
                         }
                         else
                         {
                             sb.AppendLine(string.Format("【{0}】:该组件所对产品类型【{1}】下无数据，请在基础数据AutoPrint_NE01中维护数据", DateTime.Now.ToString("MM-dd HH:mm:ss"), ds.Tables[0].Rows[0]["CELL_TYPE"].ToString()));
                             txtLog.Text = sb.ToString();
                             return;
                         }
                        #region --无用
                        //}

                    //if (string.IsNullOrEmpty(flagType))
                    //{
                    //    sb.AppendLine(string.Format("【{0}】: 当前工单【{1}】中没有体现电池是否为PERC,请联系IT在POR_WORK_ORDER中添加CELL_TYPE数据",
                    //        DateTime.Now.ToString("MM-dd HH:mm:ss"),
                    //        _orderNumner));
                    //    txtLog.Text = sb.ToString();
                    //    return;

                    //}

                    //if (flagType == "-N")
                    //{
                    //    if (proModelName.Contains("6610"))
                    //    {
                    //        if (proModelName.Contains("M"))
                    //        {
                    //            code = "NERM156×156- -M SI " + power.Trim() + "W";
                    //        }
                    //        else if (proModelName.Contains("P"))
                    //        {
                    //            code = "NERP156×156-60-P SI " + power.Trim() + "W";
                    //        }
                    //        else {
                    //             sb.AppendLine(string.Format("【{0}】: 产品类型不对，为匹配单晶或多晶", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                    //            txtLog.Text = sb.ToString();
                    //            return;
                    //              }

                    //    }
                    //    else if (proModelName.Contains("6612"))
                    //    {
                    //        if (proModelName.Contains("M"))
                    //        {
                    //            code = "NERM-CS6612M-" + power.Trim() + "W";
                    //        }
                    //        else if (proModelName.Contains("P"))
                    //        {
                    //            code = "NERP-CS6612P-" + power.Trim() + "W";
                    //        }
                    //        else {
                    //            sb.AppendLine(string.Format("【{0}】: 产品类型不对，为匹配单晶或多晶", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                    //            txtLog.Text = sb.ToString();
                    //            return;
                    //              }

                    //    }
                    //    else { 
                    //        sb.AppendLine(string.Format("【{0}】: 产品类型不对，为匹配6610或6612", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                    //        txtLog.Text = sb.ToString();
                    //        return;
                    //          }
                    //}
                    //else if (flagType == "-P")
                    //{
                    //    if (proModelName.Contains("6610"))
                    //    {
                    //        if (proModelName.Contains("M"))
                    //        {
                    //            code = "NER660M" + power.Trim();
                    //        }
                    //        else if (proModelName.Contains("P"))
                    //        {
                    //            code = "NER660P" + power.Trim();
                    //        }
                    //        else {
                    //            sb.AppendLine(string.Format("【{0}】: 产品类型不对，为匹配单晶或多晶", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                    //            txtLog.Text = sb.ToString();
                    //            return;
                    //              }

                    //    }
                    //    else if (proModelName.Contains("6612"))
                    //    {
                    //        if (proModelName.Contains("M"))
                    //        {
                    //            code = "NER672M" + power.Trim();
                    //        }
                    //        else if (proModelName.Contains("P"))
                    //        {
                    //            code = "NER672P" + power.Trim();
                    //        }
                    //        else {
                    //               sb.AppendLine(string.Format("【{0}】: 产品类型不对，为匹配单晶或多晶", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                    //            txtLog.Text = sb.ToString();
                    //            return; 
                    //              }

                    //    }
                    //    else {
                    //        sb.AppendLine(string.Format("【{0}】: 产品类型不对，为匹配6610或6612", DateTime.Now.ToString("MM-dd HH:mm:ss")));
                    //        txtLog.Text = sb.ToString();
                    //        return; 
                        //          }
                        #endregion
                    }
                    NameplateLabel_NE01 report = new NameplateLabel_NE01(code, power, Imp, Vmp, Isc, Voc, toleRance, ckg, maxPower, cellType, X, Y);
                    report.PrinterName = printer;
                    report.Print();
                }
                if ("Korea".Equals(template))
                {

                    string typeSuffix = string.Empty; //型号后缀
                    typeSuffix = drcInfo[0]["MODULE_TYPE_SUFFIX"].ToString();

                    string type = string.Empty;
                    string cNo = string.Empty;
                    string cDate = string.Empty;
                    string[] l_s = new string[] { "Certificate_Date", "Type", "Certificate_No" };
                    string category = "NameplateLabelPrint-01";
                    DataTable dtCommon = BaseData.Get(l_s, category);
                    //DataTable dttype = dtCommon.Clone();

                    if (string.IsNullOrEmpty(typeSuffix))
                    {
                        sb.AppendLine(string.Format("该工单【{0}】未维护后缀类型，请联系NPI在工单产品属性设置中设置后缀类型",_orderNumner));
                        txtLog.Text = sb.ToString();
                        return;
                    }
                    DataRow[] drType = dtCommon.Select(string.Format("Type like '%{0}%' and Type like '%{1}%'and Type like '%{2}%' ", proModelName, typeSuffix,power));
                    if (drType.Count() > 0)
                    {
                        type = drType[0]["Type"].ToString();
                        cNo = drType[0]["Certificate_No"].ToString();
                        cDate = drType[0]["Certificate_Date"].ToString();
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.Msg002}"));//未维护类型，请联系IT在基础数据维护中NameplateLabelPrint-01表中维护相应字段
                        //sb.AppendLine("未维护类型，请联系IT在基础数据维护中NameplateLabelPrint-01表中维护相应字段");
                        txtLog.Text = sb.ToString();
                        return;
                    }

                    pscode = proModelName.Trim() + typeSuffix.Trim() + power.Trim() + labelType.Trim() + version;
                    code = type;

                    NameplateLabel_Korea report = new NameplateLabel_Korea(pscode, power, Voc, Isc, Vmp, Imp, maxPower, toleRance, cellType, cNo, cDate, Fuse, X, Y, code);
                    report.PrinterName = printer;
                    report.Print();
                    
                }

                //韩国KS铭牌自动打印  add by yibin.fei 2017.12.15

                if ("KoreaKS".Equals(template))
                {

                    string typeSuffix = string.Empty; //型号后缀
                   //typeSuffix = drcInfo[0]["MODULE_TYPE_SUFFIX"].ToString();

                    string type = string.Empty;
                    string cNo = string.Empty;
                    string cDate = string.Empty;
                    string[] l_s = new string[] { "Certificate_Date", "Type", "Certificate_No" };
                    string category = "NameplateLabelPrint-01";
                    DataTable dtCommon = BaseData.Get(l_s, category);
                 //   DataTable dttype = dtCommon.Clone();

                   // if (string.IsNullOrEmpty(typeSuffix))
                    //{
                      //  sb.AppendLine(string.Format("该工单【{0}】未维护后缀类型，请联系NPI在工单产品属性设置中设置后缀类型", _orderNumner));
                     //   txtLog.Text = sb.ToString();
                     //   return;
                   // }
                    DataRow[] drType = dtCommon.Select(string.Format("Type like '%{0}%' and Type like '%{1}%' ", proModelName, power));
                    if (drType.Count() > 0)
                    {
                        type = drType[0]["Type"].ToString();
                        cNo = drType[0]["Certificate_No"].ToString();
                        cDate = drType[0]["Certificate_Date"].ToString();
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.Msg002}"));//未维护类型，请联系IT在基础数据维护中NameplateLabelPrint-01表中维护相应字段
                        //sb.AppendLine("未维护类型，请联系IT在基础数据维护中NameplateLabelPrint-01表中维护相应字段");
                        txtLog.Text = sb.ToString();
                        return;
                    }
                    pscode = proModelName.Trim() + typeSuffix.Trim() + power.Trim() + labelType.Trim() + version;
                    code = type;

                    NameplateLabel_Korea_KS report = new NameplateLabel_Korea_KS(pscode, power, Voc, Isc, Vmp, Imp, maxPower, toleRance, cellType, cNo, cDate, Fuse, X, Y, code);
                    report.PrinterName = printer;
                    report.Print();

                }


                if ("CSA".Equals(template))
                {
                    pscode = pordId;
                    int i = pscode.IndexOf("-");
                    pscode = pscode.Substring(0, i);
                    code = proModelName.ToUpper().Trim()
                       + power.ToUpper().Trim()
                       + labelType.ToUpper().Trim()
                      + version;
                    NameplateLabel_CSA report = new NameplateLabel_CSA(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    report.PrinterName = printer;
                    report.Print();
                }
                if ("QT".Equals(template))
                {

                    pscode = "QTSM" +  proModelName .Trim() + "-" + power.Trim();

                    code = "QTSM" + proModelName.Trim()
                        + power.Trim()
                        + labelType.Trim()
                        + version;
                    NameplateLabel_QT report = new NameplateLabel_QT(code, Voc, Isc, Vmp, Imp, Fuse , maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    report.PrinterName = printer;
                    report.Print();
                }
                if ("QTX".Equals(template))
                {
                    string not = string.Empty;
                    string weight = string.Empty;
                    pscode = "QTSM" + proModelName + "-" + power;

                    DataSet dsSize = namePlateLabelAutoPrint.GetSizeForQTX(pordId);
                    string size = dsSize.Tables[0].Rows[0]["CELL_SIZE"].ToString();

                    string[] l_s = new string[] { "NOCT", "SIZE", "WEIGHT","CELLTYPE" };
                    string category = "NameplateLabelAutoprint_QTX";
                    DataTable dt = BaseData.Get(l_s, category);

                    DataRow[] drs = dt.Select(string.Format("SIZE='{0}' AND CELLTYPE='{1}'", size, proModelName));
                    if (drs.Count() > 0)
                    {
                        not = drs[0]["NOCT"].ToString();
                        weight = drs[0]["WEIGHT"].ToString();

                    }
                    NameplateLabel_QTXX report = new NameplateLabel_QTXX(Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, not, weight, size, toleRance, X, Y);

                    report.PrinterName = printer;
                    report.Print();
                }
                if ("QX".Equals(template))
                {

                    pscode = string.Format("SL{0}TU-{1}P", power, map[proModelName.Trim()]);


                    code = proModelName.Trim()
                        + power.Trim()
                        + labelType.Trim()
                        + version;

                    NameplateLabel_QiXin report = new NameplateLabel_QiXin(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    report.PrinterName = printer;
                    report.Print();
                }
                if ("TUV/CSA".Equals(template))
                {
                    pscode = pordId;
                    int i = pscode.IndexOf("-");
                    pscode = pscode.Substring(0, i);

                    code = string.Empty;
                    if (pordId.Contains("HV"))
                    {

                        NameplateLabel_TUVCSA report = new NameplateLabel_TUVCSA(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                        report.PrinterName = printer;
                        report.Print();
                    }
                    else
                    {
                        NameplateLabel_TUVCSA1500 report = new NameplateLabel_TUVCSA1500(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);

                        report.PrinterName = printer;
                        report.Print();
                    }
                }
                //yibin.fei 2017.11.07
                if ("PVLINE".Equals(template))
                {
                    pscode = pordId;
                    int i = pscode.IndexOf("-");
                    pscode = pscode.Substring(0, i);


                    code = proModelName.ToUpper().Trim()
                + power.ToUpper().Trim()
                + labelType.ToUpper().Trim()
                + version;
                   
                    string[] l_s = new string[] { "OriginalProductType", "modifyProductType" };
                    string category = "Packing_List_Print_PVLine";
                    System.Data.DataTable dt_PVLine = BaseData.Get(l_s, category);
                    DataRow[] drModifyProductType = dt_PVLine.Select(string.Format("OriginalProductType='{0}'", pscode));
                    if (drModifyProductType.Length > 0)
                    {

                        pscode = drModifyProductType[0]["modifyProductType"].ToString();



                    }


                    NameplateLabel_TUV_PVLINE reportPVLINE = new NameplateLabel_TUV_PVLINE(code, Voc, Isc, Vmp, Imp, Fuse, maxPower, noct, cellType, pscode, power, toleRance, X, Y);
                    reportPVLINE.PrinterName = printer;
                    reportPVLINE.Print();

                }

            }
            catch
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.Msg003}"),MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                //MessageService.ShowMessage("打印标签失败，请重试！", "提示");
                return;
            }
        }
        Dictionary<string, string> map = new Dictionary<string, string>
       {
           {"6610P","30"},
           {"6612P","36"}
       };

        private void NameplateLabelAutoPrint_Load(object sender, EventArgs e)
        {
            InitprinterComboBox();

            NamePlatePrinter = PropertyService.Get(PROPERTY_FIELDS.NAMEPLATE_PRINTER,"");
            txtNamepaltePrinter.Text = NamePlatePrinter;

            if (string.IsNullOrEmpty(txtNamepaltePrinter.Text))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.Msg004}"), MESSAGEBOX_CAPTION);//请选择铭牌打印机！
                //MessageBox.Show("请选择铭牌打印机！");
            }
            else
            {
                if (!txtNamepaltePrinter.Properties.Items.Contains(NamePlatePrinter))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelAutoPrint.Msg005}"), MESSAGEBOX_CAPTION);//铭牌打印机不在本地打印机列表中，请重新选择
                    //MessageBox.Show("铭牌打印机不在本地打印机列表中，请重新选择");

                }
            }
        }
        private void InitprinterComboBox()
        {
            List<String> list = LocalPrinter.GetLocalPrinters(); //获得系统中的打印机列表
            foreach (String s in list)
            {
                txtNamepaltePrinter.Properties.Items.Add(s); //将打印机名称添加到下拉框中

            }
        }

        private void txtNamepaltePrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            NamePlatePrinter = txtNamepaltePrinter.Text.Trim();
            PropertyService.Set(PROPERTY_FIELDS.NAMEPLATE_PRINTER, NamePlatePrinter);
            PropertyService.Save();
        }

        private void txtX_EditValueChanged(object sender, EventArgs e)
        {
            int x = int.Parse(txtX.Text.Trim());
            PropertyService.Set(PROPERTY_FIELDS.NAMEPLATE_PRINTER_X, x);
            PropertyService.Save();
        }

        private void txtY_EditValueChanged(object sender, EventArgs e)
        {
            int y = int.Parse(txtY.Text.Trim());
            PropertyService.Set(PROPERTY_FIELDS.NAMEPLATE_PRINTER_Y, y);
            PropertyService.Save();
        }

        private void txtLotNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnPrint.PerformClick();
            }
        }

    }
    public class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
    }

    public class LocalPrinter
    {
        private static PrintDocument fPrintDocument = new PrintDocument();
        //获取本机默认打印机名称
        public static String DefaultPrinter()
        {
            return fPrintDocument.PrinterSettings.PrinterName;
        }
        public static List<String> GetLocalPrinters()
        {
            List<String> fPrinters = new List<String>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }
    }
}
