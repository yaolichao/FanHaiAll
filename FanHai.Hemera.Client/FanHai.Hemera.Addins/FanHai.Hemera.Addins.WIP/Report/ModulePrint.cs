using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using FanHai.Hemera.Utils.Barcode;
using FanHai.Hemera.Utils.Common;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public class ModulePrint
    {

     



        public static void BindDataSource(PrintLabelParameterData data, ModulePrintDataSet print,ModulePrintDataSetNew printNew)
        {
            print.ModulePrint.Rows.Add(new object[] {
                data.Darkness,
                data.X,
                data.Y,
                data.Dpi,
                data.LablePrinterType,
                data.LablePrinterName,
                data.LabelPrinterIP,
                data.LablePrinterPort,
                data.IsPrintNameplate,
                data.IsEnableByProduct,
                data.IVTestDataKey,
                data.WorkOrderKey,
                data.ProductKey,
                data.PowersetKey,
                data.FactoryName,
                data.ProductModel,
                data.LabelNo,
                data.PSign,
                data.LotNo,
                data.WorkOrderNumber,
                data.PartNumber,
                data.ProductCode,
                data.FullPalletQty,
                data.LotCellQty,
                data.ArticleNo,
                data.Celleff,
                data.CalibrationNo,
                data.StandCalibration,
                data.DeviceNum,
                data.TestRuleCode,
                data.MinPower,
                data.MaxPower,
                data.Digits,
                data.PrintQty,
                data.PM,
                data.IPM,
                data.ISC,
                data.VOC,
                data.VPM,
                data.FF,
                data.CTM,
                (!string.IsNullOrEmpty(data.PowersetSubPowerLevel ?? string.Empty) && (data.PowersetSubPowerLevel ?? string.Empty).Length >= 2 && (data.PowersetSubPowerLevel ?? string.Empty).LastIndexOf('-') >= 0)?data.CoefPM.ToString("#,##0.00") + (data.PowersetSubPowerLevel ?? string.Empty).Substring((data.PowersetSubPowerLevel ?? string.Empty).LastIndexOf('-'), 2):data.CoefPM.ToString("#,##0.00"),
                data.CoefISC.ToString("#,##0.00"),
                data.CoefIPM.ToString("#,##0.00"),
                data.CoefVOC.ToString("#,##0.00"),
                data.CoefVPM.ToString("#,##0.00"),
                data.CoefFF,
                data.PowerDifferent,
                data.TestTemperature,
                data.ErrorMessage,
                data.IsPrintErrorMessage,
                data.TestTime,
                data.CalibrationCycle,
                data.IsBCPData,
                data.PowersetCode,
                data.PowersetSeq,
                data.PowersetDemandQty,
                data.PowersetModuleName,
                data.PowersetModuleCode,
                data.PowersetStandardPM,
                data.PowersetStandardISC,
                data.PowersetStandardIPM,
                data.PowersetStandardVOC,
                data.PowersetStandardVPM,
                data.PowersetStandardFuse,
                data.PowersetPowerDifferent,
                data.PowersetSubWay,
                data.PowersetSubPowerLevel,
                data.PowersetSubCode,
                data.CustomerCode,
                data.SlideCode});

            printNew.ModulePrintTable.Rows.Add(new object[] { 
            (!string.IsNullOrEmpty(data.PowersetSubPowerLevel ?? string.Empty) 
            && (data.PowersetSubPowerLevel ?? string.Empty).Length >= 2 
            && (data.PowersetSubPowerLevel ?? string.Empty).LastIndexOf('-') >= 0)
            ?data.CoefPM.ToString("#,##0.00") + (data.PowersetSubPowerLevel ?? string.Empty).Substring((data.PowersetSubPowerLevel ?? string.Empty).LastIndexOf('-'), 2) + "Wp"
            :data.CoefPM.ToString("#,##0.00") + "Wp",
            data.CoefISC.ToString("#,##0.00").Trim() + "A",
            data.CoefIPM.ToString("#,##0.00").Trim() + "A",
            data.CoefVOC.ToString("#,##0.00").Trim() + "V",
            data.CoefVPM.ToString("#,##0.00").Trim() + "V",
            data.PrintQty.ToString().Trim(),
            data.PowersetModuleCode,
            data.PowersetModuleName,
            data.PowersetStandardPM + "Wp",
            data.PowersetStandardISC.ToString().Trim() + "A",
            data.PowersetStandardVOC.ToString().Trim() + "V",
            data.PowersetStandardIPM.ToString().Trim() + "A",
            data.PowersetStandardVPM.ToString().Trim() + "V",
            data.PM.ToString("#,##0.00").Trim(),
            data.ISC.ToString("#,##0.00").Trim() + "A",
            data.IPM.ToString("#,##0.00").Trim() + "A",
            data.VOC.ToString("#,##0.00").Trim() + "V",
            data.VPM.ToString("#,##0.00").Trim() + "V",
            data.TestTime.ToString("yyyy-MM-dd"),
            data.PowersetStandardFuse.ToString(),
            data.PowersetPowerDifferent,
            data.PowersetSubPowerLevel ?? string.Empty,
            data.ProductModel,
            data.LotNo
            });
        }
        public static bool PrintLabel(PrintLabelParameterData data)
        {
            ModulePrintDataSet print = new ModulePrintDataSet();
            ModulePrintDataSetNew printNew = new ModulePrintDataSetNew();
            //BindDataSource(data,print,printNew);
            MethodInfo method = null;
            string methodNameWithOutDpi = "wf_printlable" + data.LabelNo.PadLeft(2, '0');
            if (method == null)
            {
                method = typeof(ModulePrint).GetMethod(methodNameWithOutDpi);
            }
            if (method != null)
            {
                object[] objs = new object[1];
                //objs[0] = printNew;
                objs[0] = data;
                object retObj = method.Invoke(null, objs);
                if (retObj != null)
                {
                    return Convert.ToBoolean(retObj);
                }
            }
            return false;
        }
        public static bool wf_printlable32(PrintLabelParameterData data)
        {
            Label_32 reportL32 = new Label_32(data);
            //xreport = XtraReport.FromFile(Application.StartupPath + "\\rpt\\" + "VehConfirmPrintRpt.repx", true);//报表绑定
            //xreport.PrintingSystem.ShowMarginsWarning = false;//是否显示超出边框验证
            //xreport.DataSource = PrintData.GetDataSetSchema(data; //数据源绑定
            // Print the report.
            //reportL32.ShowPreview();
            reportL32.Print();
            return true;
        }
        public static bool wf_printlable321(PrintLabelParameterData data)
        {
            Label_321 reportL321 = new Label_321(data);
            if (data.IsChoosePrint == true)
                reportL321.PrintDialog();                              //选择打印机
            else
                reportL321.Print();
            return true;
        }
        public static bool wf_printlable322(PrintLabelParameterData data)
        {
            Label_322 reportL322 = new Label_322(data);
            if(data.IsChoosePrint == true)
                reportL322.PrintDialog();                              //选择打印机
            else
                reportL322.Print();
            return true;
        }
        public static bool wf_printlable3221(PrintLabelParameterData data)
        {
            Label_3221 reportL3221 = new Label_3221(data);
            if (data.IsChoosePrint == true)
                reportL3221.PrintDialog();                              //选择打印机
            else
                reportL3221.Print();
            return true;
        }
        public static bool wf_printlable323(PrintLabelParameterData data)
        {
            Label_323 reportL323 = new Label_323(data);
            if (data.IsChoosePrint == true)
                reportL323.PrintDialog();                              //选择打印机
            else
                reportL323.Print();
            return true;
        }
        public static bool wf_printlable323Malai(PrintLabelParameterData data)
        {
            Label_323MaLai reportL323 = new Label_323MaLai(data);
            if (data.IsChoosePrint == true)
                reportL323.PrintDialog();                              //选择打印机
            else
                reportL323.Print();
            return true;
        }
        public static bool wf_printlable324(PrintLabelParameterData data)
        {
            Label_324 reportL324 = new Label_324(data);
            if (data.IsChoosePrint == true)
                reportL324.PrintDialog();                              //选择打印机
            else
                reportL324.Print();
            return true;
        }
        public static bool wf_printlable04(PrintLabelParameterData data)
        {
            Label_4 reportL4 = new Label_4(data);
            reportL4.Print();
            return true;
        }
        /// <summary>
        /// 使用  常规TUV模板
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <returns></returns>
        public static bool wf_print04(string lotNumber,int x,int y,int count)
        {
            Label_4 reportL4 = new Label_4(lotNumber,x,y);
            int i = 1;
            while (i <= count)
            {
                reportL4.Print();
                i++;
            }
            return true;
        }
        
        /// <summary>
        /// 使用  陕西国力序列号模板
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <returns></returns>
        public static bool wf_print05(string lotNumber, int x, int y)
        {
            Label_5 reportL5 = new Label_5(lotNumber, x, y);
            int i = 1;
            while (i <= 2)
            {
                reportL5.Print();
                i++;
            }
            return true;
        }

        /// <summary>
        /// 使用 westholdings序列号模板
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <returns></returns>
        public static bool wf_print06(string lotNumber, int x, int y)
        {
            Label_4 reportL4 = new Label_4(lotNumber, x, y);
            int i = 1;
            while (i <= 2)
            {
                reportL4.Print();
                i++;
            }
            return true;
        }
        /// <summary>
        /// TUV 非英国16位序列号模板
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool wf_print08(string lotNumber, int x, int y)
        {
            Label_8 reportL8 = new Label_8(lotNumber, x, y);
            int i = 1;
            while (i <= 2)
            {
                reportL8.Print();
                i++;
            }
            return true;
        }

        public static bool wf_printlable516(PrintLabelParameterData data)
        {
            NameplateLabel_516 reportp516 = new NameplateLabel_516(data);
            if (data.IsChoosePrint == true)
                reportp516.PrintDialog();                              //选择打印机
            else
                reportp516.Print();
            return true;
        }
        public static bool wf_printlable56(PrintLabelParameterData data)
        {

            NameplateLabel_56 reportp56 = new NameplateLabel_56(data);
            if (data.IsChoosePrint == true)
                reportp56.PrintDialog();                              //选择打印机
            else
                reportp56.Print();
            return true;
            //reportp56.ShowPreview();
        }
        public static bool wf_printlable60(PrintLabelParameterData data)
        {
            Label_60 reportL60 = new Label_60(data);
            if (data.IsChoosePrint == true)
                reportL60.PrintDialog();                              //选择打印机
            else
                reportL60.Print();
            return true;
        }
        /// <summary>
        /// 河北赞皇铭牌设计
        /// </summary>
        /// <param name="data">批次对应参数集合</param>
        /// <returns>打印是否成功</returns>
        public static bool wf_printlable61(PrintLabelParameterData data)
        {
            NameplateLabel_61 nameplateLable = new NameplateLabel_61(data);
            nameplateLable.Print();
            return true;
        }
        public static bool wf_printlable543(PrintLabelParameterData data)
        {
            NameplateLabel_543 reportL543 = new NameplateLabel_543(data);
            if (data.IsChoosePrint == true)
                reportL543.PrintDialog();                              //选择打印机
            else
                reportL543.Print();
            return true;
        }
        public static bool wf_printlable58(PrintLabelParameterData data)
        {
            Label_58 reportL58 = new Label_58(data);
            //reportL58.Print();
            if (data.IsChoosePrint == true)
                reportL58.PrintDialog();                              //选择打印机
            else
                reportL58.Print();
            return true;
        }
        public static bool wf_printlable100(PrintLabelParameterData data)
        {
            NameplateLabel_TUV report100 = new NameplateLabel_TUV(data);
            //reportL58.Print();
            report100.ShowPreview();
            return true;
        }

        //public static bool PrintLabel(string code, string _voc, string _isc, 
        //    string _vmp, string _imp, string _fuse, 
        //    string _max, string _noct, string _cellType, 
        //    string pscode, string _power,string dif,
        //    int x, int y, string typeTUV)
        //{
            
        //    return true;
        //}
        public static bool PrintLabel(string code, string _voc, string _isc,
           string _vmp, string _imp, string _fuse,
           string _max, string _noct, string _cellType,
           string pscode, string _power, string dif,
           int x, int y,string asm)
        {
            if (asm.ToUpper() == "ASM")
            {
                NameplateLabel_ASM report = new NameplateLabel_ASM(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y,false);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "ASM_NEW")
            {
                NameplateLabel_ASM report = new NameplateLabel_ASM(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y, true);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "CQC")
            {
                NameplateLabel_CQC report = new NameplateLabel_CQC(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "CEC1000")
            {
                NameplateLabel_TUVFH_CEC1000 report = new NameplateLabel_TUVFH_CEC1000(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y, true);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "CEC1500")
            {
                NameplateLabel_TUVFH_CEC1500 report = new NameplateLabel_TUVFH_CEC1500(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y, true);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "CGCLPZ")
            {
                NameplateLabel_CGClpz report = new NameplateLabel_CGClpz(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report.ShowPreview();
            }
              //yibin.fei  安能铭牌
            else if (asm.ToUpper() == "CQC_AN")
            {
                NameplateLabel_CQC_AnNeng report = new NameplateLabel_CQC_AnNeng(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report.ShowPreview();
            }

            else if (asm.ToUpper() == "CSA")
            {
                NameplateLabel_CSA report = new NameplateLabel_CSA(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "QT")
            {
                NameplateLabel_QT report = new NameplateLabel_QT(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "QX")
            {
                NameplateLabel_QiXin report = new NameplateLabel_QiXin(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report.ShowPreview();
            }
            else if (asm.ToUpper() == "常规1000")
            {
                NameplateLabel_TUVFH_NEW reportTUVFH_NEW = new NameplateLabel_TUVFH_NEW(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y,true);
                //NameplateLabel_TUV report100 = new NameplateLabel_TUV(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportTUVFH_NEW.ShowPreview();
            }
            else if (asm.ToUpper() == "常规1000(无产地标识)")
            {
                NameplateLabel_TUVFH_NEW reportTUVFH_NEW = new NameplateLabel_TUVFH_NEW(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y, false);
                //NameplateLabel_TUV report100 = new NameplateLabel_TUV(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportTUVFH_NEW.ShowPreview();
            }
            else if (asm.ToUpper() == "SOLAR_JUICE1000")
            {
                NameplateLabel_SOLAR_JUICE reportSOLAR_JUICE = new NameplateLabel_SOLAR_JUICE(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                //NameplateLabel_TUV report100 = new NameplateLabel_TUV(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportSOLAR_JUICE.ShowPreview();
            }
            else if (asm.ToUpper() == "SOLAR_JUICE1500")
            {
                NameplateLabel_SOLAR_JUICE reportSOLAR_JUICE = new NameplateLabel_SOLAR_JUICE(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                //NameplateLabel_TUV report100 = new NameplateLabel_TUV(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportSOLAR_JUICE.ShowPreview();
            }

            else if (asm.ToUpper() == "常规1000(新)")
            {
               // NameplateLabel_TUV_PVLINE report100 = new NameplateLabel_TUV_PVLINE(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                //NameplateLabel_TUV_New report100 = new NameplateLabel_TUV_New(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                //report100.ShowPreview();
                NameplateLabel_TUVFH_NEW reportTUVFH_NEW = new NameplateLabel_TUVFH_NEW(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y, true);

                reportTUVFH_NEW.ShowPreview();
            }
            else if (asm.ToUpper() == "常规1500")
            {
                NameplateLabel_TUV01 reportTUV01 = new NameplateLabel_TUV01(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportTUV01.ShowPreview();
            }
             //PVLINE模板铭牌打印 yibin.fei 2017.11.07
            else if(asm.ToUpper()=="PVLINE1000")
            {

                string[] l_s = new string[] { "OriginalProductType", "modifyProductType" };
                string category = "Packing_List_Print_PVLine";
                System.Data.DataTable dt_PVLine = BaseData.Get(l_s, category);
                DataRow[] drModifyProductType = dt_PVLine.Select(string.Format("OriginalProductType='{0}'", pscode));
                if (drModifyProductType.Length > 0)
                {

                    pscode = drModifyProductType[0]["modifyProductType"].ToString();



                }
                else
                {
                    MessageBox.Show(string.Format("【{0}】未维护PVLINE铭牌模板，请联系IT",pscode));
                    
                }

                NameplateLabel_TUV_PVLINE report100PVLINE = new NameplateLabel_TUV_PVLINE(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                report100PVLINE.ShowPreview();
            }

            else if (asm.ToUpper() == "双玻")
            {
                NameplateLabel_TUV02 reportTUV02 = new NameplateLabel_TUV02(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportTUV02.ShowPreview();
            }
            //else if (asm.ToUpper() == "澳洲防火")
            //{
            //    NameplateLabel_TUVFH_NEW reportTUVFH_NEW = new NameplateLabel_TUVFH_NEW(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y, true);
            //    reportTUVFH_NEW.ShowPreview();
            //    //NameplateLabel_TUVFH reportTUVFH = new NameplateLabel_TUVFH(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
            //    //reportTUVFH.ShowPreview();
            //}
            else if (asm.ToUpper() == "TUVCSA1500")
            {
                NameplateLabel_TUVCSA reportTUVCSA = new NameplateLabel_TUVCSA(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportTUVCSA.ShowPreview();
            }
            else if (asm.ToUpper() == "TUVCSA1000")
            {
                NameplateLabel_TUVCSA1500 reportTUVCSA = new NameplateLabel_TUVCSA1500(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, dif, x, y);
                reportTUVCSA.ShowPreview();
            }
            return true;
        }
        public static bool PrintLabel(string pscode,string _powerCQClpz,string _effCQClpz,string _mianjiCQClpz, 
            string _qtyCQClpz,int x, int y,
            string asm)
        {
            if (asm.ToUpper() == "CQCLPZ")
            {
                NameplateLabel_CQClpz report = new NameplateLabel_CQClpz(pscode, _powerCQClpz, _effCQClpz, _mianjiCQClpz, _qtyCQClpz, x, y);
                report.ShowPreview();
            }
            
            return true;
        }
        
        /// <summary>
        /// 组件序列号标签模板，传入序列号和id 根据id 选取不同模板
        /// </summary>
        /// <param name="lotNumber">序列号</param>
        /// <param name="id">ID号</param>
        /// <returns></returns>
        public static bool PrintLabel(string lotNumber,string id,int x,int y)
        {
            MethodInfo method = null;
            string methodNameWithOutDpi = "wf_print" + id.PadLeft(2, '0');
            if (method == null)
            {
                method = typeof(ModulePrint).GetMethod(methodNameWithOutDpi);
            }
            if (method != null)
            {
                object[] objs = new object[3];
                //objs[0] = printNew;
                objs[0] = lotNumber;
                objs[1] = x;
                objs[2] = y;
                object retObj = method.Invoke(null, objs);
                if (retObj != null)
                {
                    return Convert.ToBoolean(retObj);
                }
            }
            return false;
        }

        /// <summary>
        /// 组件序列号标签模板，传入序列号和id 根据id 选取不同模板
        /// </summary>
        /// <param name="lotNumber">序列号</param>
        /// <param name="id">ID号</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count">打印份数</param>
        /// <returns></returns>
        public static bool PrintLabel(string lotNumber, string id, int x, int y,int count)
        {
            MethodInfo method = null;
            string methodNameWithOutDpi = "wf_print" + id.PadLeft(2, '0');
            if (method == null)
            {
                method = typeof(ModulePrint).GetMethod(methodNameWithOutDpi);
            }
            if (method != null)
            {
                object[] objs = new object[4];
                //objs[0] = printNew;
                objs[0] = lotNumber;
                objs[1] = x;
                objs[2] = y;
                objs[3] = count;
                object retObj = method.Invoke(null, objs);
                if (retObj != null)
                {
                    return Convert.ToBoolean(retObj);
                }
            }
            return false;
        }

        public static bool PrintLabel(string pscode, string _maxNe, string _impNe, string _vmpNe, string _iscNe, string _vocNe, string _noctNe, string p, string p_9, string p_10,string _cellTypeNe,int x, int y, string ne)
        {
            if (ne.ToUpper() == "NE")
            {
                NameplateLabel_NE report = new NameplateLabel_NE(pscode, _maxNe, _impNe, _vmpNe, _iscNe, _vocNe, _noctNe, p, p_9, p_10, _cellTypeNe, x, y);
                report.ShowPreview();
            }

            return true;
        }

        public static bool PrintLabel(string pscode,string p, string p_2, string p_3, string p_4, string p_5, string p_6, string p_7, string p_8, string p_9, string p_10, string p_14, int x, int y, string p_13,string code)
        {
            if (p_13 == "Korea")
            {
                NameplateLabel_Korea report = new NameplateLabel_Korea(pscode,p, p_2, p_3, p_4, p_5, p_6, p_7, p_8, p_9, p_10, p_14, x, y,code);
               
                report.ShowPreview();
            }
            else if (p_13 == "KoreaKS")//韩国KS认证铭牌 add by yibin.fei 
            {
                NameplateLabel_Korea_KS report = new NameplateLabel_Korea_KS(pscode, p, p_2, p_3, p_4, p_5, p_6, p_7, p_8, p_9, p_10, p_14, x, y, code);
                report.ShowPreview();
            }


            return true;
        }

        public static bool PrintLabel(string _vocQT, string _iscQT, string _vmpQT, string _impQT, string _fuseQT, string _maxQT, string _noctQT, string _cellTypeQT, string pscode, string _powerQT, string not, string weight, string cc,string dif, int x, int y, string p_17)
        {
            NameplateLabel_QTXX report = new NameplateLabel_QTXX(_vocQT, _iscQT, _vmpQT, _impQT, _fuseQT, _maxQT, _noctQT, _cellTypeQT, pscode, _powerQT, not, weight, cc,dif, x, y);
            report.ShowPreview();
            return true;
        }

        internal static bool PrintLabel(string code, string _powerNe, string _impNe, string _vmpNe, string _iscNe, string _vocNe, string _fengdnag, string _ckg, string _dianya, string _cellTypeNe, int x, int y, string ne)
        {
            if (ne.ToUpper() == "NE01")
            {
                NameplateLabel_NE01 report = new NameplateLabel_NE01(code, _powerNe, _impNe, _vmpNe, _iscNe, _vocNe, _fengdnag, _ckg, _dianya, _cellTypeNe, x, y);
                report.ShowPreview();
            }

            return true;
        }
    }
}
