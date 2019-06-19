using System;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Reflection;
using FanHai.Hemera.Utils.Entities;
using System.Data;
using System.IO;
using System.Collections.Generic;
using FanHai.Hemera.Utils.Common;


namespace FanHai.Hemera.Utils.Barcode
{

    /// <summary>
    /// 将待打印数据立像打印机的帮助类。
    /// </summary>

    public sealed class ArgobarPrinterHelper
    {
        #region//立象打印机DLL文件引用
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_Maxi(int x, int y, int cl, int cc, int pc, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_PDF417(int x, int y, int w, int v, int s, int c, int px, int py, int r, int l, int t, int o, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_PDF417_N(int x, int y, int w, string para, string data);
        [DllImport("Winpplb.dll")]
        private static extern void B_ClosePrn();
        [DllImport("Winpplb.dll")]
        private static extern int B_CreatePrn(int selection, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Del_Form(string formname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Del_Pcx(string pcxname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Draw_Box(int x, int y, int thickness, int hor_dots, int ver_dots);
        [DllImport("Winpplb.dll")]
        private static extern int B_Draw_Line(char mode, int x, int y, int hor_dots, int ver_dots);
        [DllImport("Winpplb.dll")]
        private static extern int B_Error_Reporting(char option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_DLL_VersionA(int nShowMessage);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Graphic_ColorBMP(int x, int y, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Pcx(int x, int y, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Initial_Setting(int Type, string Source);
        [DllImport("Winpplb.dll")]
        private static extern int B_Load_Pcx(int x, int y, string pcxname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Open_ChineseFont(string path);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_Form(int labset, int copies, string form_out, string var);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_MCopy(int labset, int copies);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_Out(int labset);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Barcode(int x, int y, int ori, string type, int narrow, int width, int height, char human, string data);
        [DllImport("Winpplb.dll")]
        private static extern void B_Prn_Configuration();
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text(int x, int y, int ori, int font, int hor_factor, int ver_factor, char mode, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_Chinese(int x, int y, int fonttype, string id_name, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType(int x, int y, int FSize, string FType, int Fspin, int FWeight, int FItalic,
                                        int FUnline, int FStrikeOut, string id_name, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_W(int x, int y, int FHeight, int FWidth, string FType, int Fspin, int FWeight,
                                          int FItalic, int FUnline, int FStrikeOut, string id_name, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Option(int option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Symbol(int num_bit, int symbol, int country);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Backfeed(char option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_BMPSave(int nSave, string strBMPFName);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Darkness(int darkness);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_DebugDialog(int nEnable);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Direction(char direction);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Form(string formfile);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Labgap(int lablength, int gaplength);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Labwidth(int labwidth);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Originpoint(int hor, int ver);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Prncomport(int baud, char parity, int data, int stop);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Prncomport_PC(int nBaudRate, int nByteSize, int nParity, int nStopBits, int nDsr, int nCts, int nXonXoff);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Speed(int speed);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_ProcessDlg(int nShow);
        [DllImport("Winpplb.dll")]
        private static extern int B_GetUSBBufferLen();
        [DllImport("Winpplb.dll")]
        private static extern int B_EnumUSB(byte[] buf);
        [DllImport("Winpplb.dll")]
        private static extern int B_CreateUSBPort(int nPort);
        [DllImport("Winpplb.dll")]
        private static extern int B_ResetPrinter();
        [DllImport("Winpplb.dll")]
        private static extern int B_GetPrinterResponse(string pbuf, int nMax);
        [DllImport("Winpplb.dll")]
        private static extern int B_TFeedMode(int nMode);
        [DllImport("Winpplb.dll")]
        private static extern int B_TFeedTest();
        [DllImport("Winpplb.dll")]
        private static extern int B_CreatePort(int nPortType, int nPort, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Execute_Form(string form_out, string var);
        #endregion

        public static bool wf_print_errorlable(PrintLabelParameterData data)
        {
            try
            {
                decimal pm = data.PM;
                int pnum = 1;
                int x = data.X;
                int y = data.Y;
                int darkness = 12;
                if (data.Darkness > 0)
                {
                    darkness = data.Darkness;
                }
                //打开打印接口
                int ret = B_CreatePrn(1, null);
                //设置打印浓度
                ret = B_Set_Darkness(darkness);
                //打印方向
                ret = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                ret = B_Initial_Setting(0, "N\r\n\0");
                ret = B_Del_Pcx("*");

                ret = B_Prn_Barcode(3 + x, 8 + y, 0, "1", 2, 1, 40, Convert.ToChar(66), data.LotNo);
                string sPm = pm.ToString("#,##0.00");
                ret = B_Prn_Text_TrueType(255 + x, 5 + y, 36, "Arial", 1, 400, 0, 0, 0, "A1", sPm);
                ret = B_Prn_Text_TrueType(3 + x, 77 + y, 26, "Arial", 1, 400, 0, 0, 0, "A2", data.ErrorMessage);

                //列印所有資料
                ret = B_Print_Out(pnum);
                //关闭打印
                B_ClosePrn();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool PrintLabel(PrintLabelParameterData data)
        {
            MethodInfo method = null;
            string methodNameWithOutDpi = "wf_printlable" + data.LabelNo.PadLeft(2, '0');
            if (data.Dpi == 300)
            {
                string methodName = string.Format("{0}_{1}", methodNameWithOutDpi, data.Dpi);
                method = typeof(ArgobarPrinterHelper).GetMethod(methodName);
            }

            if (method == null)
            {
                method = typeof(ArgobarPrinterHelper).GetMethod(methodNameWithOutDpi);
            }

            if (method != null)
            {
                int parameterLength = method.GetParameters().Length;
                object[] objs = new object[parameterLength];
                if (parameterLength == 5)
                {
                    string[] sPrintPara = new string[30];
                    sPrintPara[1] = data.CoefPM.ToString();
                    sPrintPara[2] = data.CoefISC.ToString().Trim();
                    sPrintPara[3] = data.CoefIPM.ToString().Trim();
                    sPrintPara[4] = data.CoefVOC.ToString().Trim();
                    sPrintPara[5] = data.CoefVPM.ToString().Trim();
                    sPrintPara[6] = data.PrintQty.ToString().Trim();
                    sPrintPara[7] = data.PowersetModuleCode;
                    sPrintPara[8] = data.PowersetModuleName;
                    sPrintPara[9] = data.PowersetStandardPM;
                    sPrintPara[10] = data.PowersetStandardISC.ToString().Trim();
                    sPrintPara[11] = data.PowersetStandardVOC.ToString().Trim();
                    sPrintPara[12] = data.PowersetStandardIPM.ToString().Trim();
                    sPrintPara[13] = data.PowersetStandardVPM.ToString().Trim();
                    sPrintPara[14] = data.PM.ToString().Trim();
                    sPrintPara[15] = data.ISC.ToString().Trim();
                    sPrintPara[16] = data.IPM.ToString().Trim();
                    sPrintPara[17] = data.VOC.ToString().Trim();
                    sPrintPara[18] = data.VPM.ToString().Trim();
                    sPrintPara[19] = data.TestTime.ToString("yyyy-MM-dd");
                    sPrintPara[20] = data.PowersetStandardFuse.ToString();
                    sPrintPara[21] = data.PowersetPowerDifferent;
                    sPrintPara[22] = data.PowersetSubPowerLevel ?? string.Empty;
                    sPrintPara[23] = data.ProductModel;

                    objs[0] = data.LotNo;
                    objs[1] = data.Darkness;
                    objs[2] = data.X;
                    objs[3] = data.Y;
                    objs[4] = sPrintPara;
                }
                else
                {
                    objs[0] = data;
                }
                object retObj = method.Invoke(null, objs);
                if (retObj != null)
                {
                    return Convert.ToBoolean(retObj);
                }
            }
            return false;
        }

        public static bool wf_printlable01(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            string s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;
            int i_pnum, i_result, l_len;
            bool b_result;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 15;
                }

                //打开打印接口
                i_result = B_CreatePrn(1, null);
                //设置打印浓度
                i_result = B_Set_Darkness(i_darkness);
                //打印方向
                i_result = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_result = B_Initial_Setting(0, "N\r\n\0");
                i_result = B_Del_Pcx("*");

                i_result = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(78), s_barcode);
                i_result = B_Prn_Text_TrueType(3 + i_x, 50 + i_y, 26, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                i_result = B_Prn_Text_TrueType(3 + i_x, 77 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A1", s_value);

                i_result = B_Prn_Text_TrueType(255 + i_x, 5 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_result = B_Prn_Text_TrueType(300 + i_x, 5 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_result = B_Prn_Text_TrueType(255 + i_x, 31 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_result = B_Prn_Text_TrueType(300 + i_x, 31 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_result = B_Prn_Text_TrueType(255 + i_x, 59 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A41", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_result = B_Prn_Text_TrueType(300 + i_x, 59 + i_y, 22, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_result = B_Prn_Text_TrueType(255 + i_x, 85 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_result = B_Prn_Text_TrueType(300 + i_x, 85 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_result = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable01_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            int i_return, i_pnum, l_len;
            string s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;
            bool b_result = false;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 3, 4, 64, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 8 + i_y, 35, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 8 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A32", s_value);
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 92 + i_y, 33, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 92 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A91", "Vmp");

                i_return = B_Prn_Text_TrueType(392 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable02(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            int i_return, i_pnum, l_len;
            string s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;
            bool b_result = false;

            try
            {
                dc_pm = decimal.Parse(s_array[14].ToString().Trim());
                dc_isc = decimal.Parse(s_array[15].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[16].ToString().Trim());
                dc_voc = decimal.Parse(s_array[17].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[18].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(66), s_barcode);

                s_value = "Pm=" + dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(3 + i_x, 77 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 5 + i_y, 26, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(300 + i_x, 5 + i_y, 24, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 31 + i_y, 24, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(300 + i_x, 31 + i_y, 24, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 59 + i_y, 24, "Arial", 1, 500, 0, 0, 0, "A41", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(300 + i_x, 59 + i_y, 24, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 85 + i_y, 24, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(300 + i_x, 85 + i_y, 24, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum * 2);

                i_return = B_Prn_Barcode(30 + i_x, 20 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(66), s_barcode);
                i_return = B_Print_Out(i_pnum * 3);
                //关闭打印
                B_ClosePrn();

                b_result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable02_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            int i_return, i_pnum, l_len;
            string s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;
            bool b_result = false;

            try
            {
                dc_pm = decimal.Parse(s_array[14].ToString().Trim());
                dc_isc = decimal.Parse(s_array[15].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[16].ToString().Trim());
                dc_voc = decimal.Parse(s_array[17].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[18].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 3, 4, 70, Convert.ToChar(66), s_barcode);

                s_value = "Pm=" + dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(3 + i_x, 120 + i_y, 54, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(355 + i_x, 5 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(420 + i_x, 5 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(355 + i_x, 46 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(420 + i_x, 46 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(355 + i_x, 89 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A41", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(420 + i_x, 89 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(355 + i_x, 130 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(420 + i_x, 130 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum * 2);

                i_return = B_Prn_Barcode(10 + i_x, 20 + i_y, 0, "1", 4, 5, 90, Convert.ToChar(66), s_barcode);
                i_return = B_Print_Out(i_pnum * 3);
                //关闭打印
                B_ClosePrn();

                b_result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable03(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            int i_return, i_pnum;
            string s_value;
            string s_plantcode, s_part, s_ratedpower, s_model, s_mftdate;
            bool b_result = false;

            try
            {
                s_plantcode = "7100103";
                s_part = s_barcode.Substring(1, 6);
                if (s_part == "")
                {
                    b_result = false;
                    return b_result;
                }
                s_model = s_barcode.Substring(5, 2);
                switch (s_model)
                {
                    case "87":
                        s_ratedpower = "175";
                        s_model = "GM572";
                        break;
                    case "88":
                        s_ratedpower = "180";
                        s_model = "GM572";
                        break;
                    case "89":
                        s_ratedpower = "220";
                        s_model = "GP660";
                        break;
                    case "90":
                        s_ratedpower = "225";
                        s_model = "GP660";
                        break;
                    case "91":
                        s_ratedpower = "230";
                        s_model = "GP660";
                        break;
                    default:
                        s_ratedpower = "";
                        b_result = false;
                        break;
                }
                s_mftdate = s_barcode.Substring(10, 4) + "20" + s_barcode.Substring(14, 2);
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Draw_Box(40 + i_x, 10 + i_y, 2, 605, 310 + i_y);

                i_return = B_Draw_Line(Convert.ToChar(79), 40 + i_x, 85 + i_y, 565, 2);

                i_return = B_Draw_Line(Convert.ToChar(79), 40 + i_x, 160 + i_y, 565, 2);

                i_return = B_Draw_Line(Convert.ToChar(79), 40 + i_x, 235 + i_y, 565, 2);

                i_return = B_Draw_Box(270 + i_x, 10 + i_y, 1, 271, 310 + i_y);

                i_return = B_Prn_Text_TrueType(50 + i_x, 35 + i_y, 32, "Arial Black", 1, 600, 0, 0, 0, "A1", "Solpower GmbH");

                i_return = B_Prn_Text_TrueType(280 + i_x, 20 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A2", "Part #:");
                i_return = B_Prn_Text_TrueType(280 + i_x, 50 + i_y, 28, "Arial", 1, 400, 0, 0, 0, "A3", s_part);

                i_return = B_Prn_Barcode(450 + i_x, 15 + i_y, 0, "1", 2, 2, 60, Convert.ToChar(78), s_part);

                i_return = B_Prn_Text_TrueType(50 + i_x, 95 + i_y, 32, "@MS UI Gothic", 1, 400, 0, 0, 0, "A4", "Model:");
                s_value = "SOLPOWER " + s_model;

                i_return = B_Prn_Text_TrueType(50 + i_x, 125 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(280 + i_x, 95 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A6", "Plant code:");
                i_return = B_Prn_Text_TrueType(280 + i_x, 125 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A7", s_plantcode);

                i_return = B_Prn_Barcode(420 + i_x, 95 + i_y, 0, "1", 2, 2, 60, Convert.ToChar(78), s_plantcode);

                i_return = B_Prn_Text_TrueType(50 + i_x, 170 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A8", "Rated power:");
                i_return = B_Prn_Text_TrueType(50 + i_x, 200 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A9", s_ratedpower);

                i_return = B_Prn_Text_TrueType(280 + i_x, 170 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A10", "Quantity:");
                i_return = B_Prn_Text_TrueType(280 + i_x, 200 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A11", "1");

                i_return = B_Prn_Text_TrueType(50 + i_x, 245 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A12", "Mft.date:");
                i_return = B_Prn_Text_TrueType(50 + i_x, 275 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A13", s_mftdate);

                i_return = B_Prn_Text_TrueType(280 + i_x, 245 + i_y, 28, "Arial", 1, 400, 0, 0, 0, "A14", "Serial number:");
                i_return = B_Prn_Text_TrueType(280 + i_x, 275 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A15", s_barcode);

                i_return = B_Prn_Barcode(450 + i_x, 245 + i_y, 0, "1", 1, 1, 30, Convert.ToChar(78), s_barcode);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable03_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            int i_return, i_pnum;
            string s_value;
            string s_plantcode, s_part, s_ratedpower, s_model, s_mftdate;
            bool b_result = false;

            try
            {
                s_plantcode = "7100103";
                s_part = s_barcode.Substring(1, 6);
                if (s_part == "")
                {
                    b_result = false;
                    return b_result;
                }
                s_model = s_barcode.Substring(5, 2);
                switch (s_model)
                {
                    case "87":
                        s_ratedpower = "175";
                        s_model = "GM572";
                        break;
                    case "88":
                        s_ratedpower = "180";
                        s_model = "GM572";
                        break;
                    case "89":
                        s_ratedpower = "220";
                        s_model = "GP660";
                        break;
                    case "90":
                        s_ratedpower = "225";
                        s_model = "GP660";
                        break;
                    case "91":
                        s_ratedpower = "230";
                        s_model = "GP660";
                        break;
                    default:
                        s_ratedpower = "";
                        b_result = false;
                        break;
                }
                s_mftdate = s_barcode.Substring(10, 4) + "20" + s_barcode.Substring(14, 2);
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Draw_Box(40 + i_x, 10 + i_y, 2, 880, 460 + i_y);

                i_return = B_Draw_Line(Convert.ToChar(79), 40 + i_x, 122 + i_y, 842, 2);

                i_return = B_Draw_Line(Convert.ToChar(79), 40 + i_x, 235 + i_y, 842, 2);

                i_return = B_Draw_Line(Convert.ToChar(79), 40 + i_x, 347 + i_y, 842, 2);

                i_return = B_Draw_Box(380 + i_x, 10 + i_y, 1, 380 + i_x, 460 + i_y);

                i_return = B_Prn_Text_TrueType(50 + i_x, 45 + i_y, 46, "Arial Black", 1, 600, 0, 0, 0, "A1", "Solpower GmbH");

                i_return = B_Prn_Text_TrueType(390 + i_x, 30 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A2", "Part #:");
                i_return = B_Prn_Text_TrueType(390 + i_x, 75 + i_y, 42, "Arial", 1, 400, 0, 0, 0, "A3", s_part);

                i_return = B_Prn_Barcode(650 + i_x, 20 + i_y, 0, "1", 3, 4, 80, Convert.ToChar(78), s_part);

                i_return = B_Prn_Text_TrueType(50 + i_x, 135 + i_y, 46, "@MS UI Gothic", 1, 400, 0, 0, 0, "A4", "Model:");

                s_value = "SOLPOWER " + s_model;
                i_return = B_Prn_Text_TrueType(50 + i_x, 180 + i_y, 39, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(390 + i_x, 135 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A6", "Plant code:");
                i_return = B_Prn_Text_TrueType(390 + i_x, 180 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A7", s_plantcode);

                i_return = B_Prn_Barcode(595 + i_x, 130 + i_y, 0, "1", 3, 4, 80, Convert.ToChar(78), s_plantcode);

                i_return = B_Prn_Text_TrueType(50 + i_x, 250 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A8", "Rated power:");

                i_return = B_Prn_Text_TrueType(50 + i_x, 295 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A9", s_ratedpower);

                i_return = B_Prn_Text_TrueType(390 + i_x, 250 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A10", "Quantity:");
                i_return = B_Prn_Text_TrueType(390 + i_x, 295 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A11", "1");

                i_return = B_Prn_Text_TrueType(50 + i_x, 360 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A12", "Mft.date:");
                i_return = B_Prn_Text_TrueType(50 + i_x, 405 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A13", s_mftdate);

                i_return = B_Prn_Text_TrueType(390 + i_x, 360 + i_y, 42, "Arial", 1, 400, 0, 0, 0, "A14", "Serial number:");
                i_return = B_Prn_Text_TrueType(390 + i_x, 405 + i_y, 46, "Arial", 1, 400, 0, 0, 0, "A15", s_barcode);

                i_return = B_Prn_Barcode(640 + i_x, 360 + i_y, 0, "1", 2, 2, 50, Convert.ToChar(78), s_barcode);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable04(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //i_return = B_Prn_Barcode(65 + i_x, 20 + i_y, 0, "1", 3, 4, 50, Convert.ToChar(78), s_barcode);

                //add by genchille.yang
                //2013-05-18 18:46:00
                int narrow = 0;
                int width = 0;
                try
                {
                    if (Convert.ToUInt64(s_barcode) > 0)
                    {
                        narrow = 3;
                        width = 4;
                    }
                }
                catch (Exception ex)
                {
                    narrow = 2;
                    width = 3;
                }
                i_return = B_Prn_Barcode(65 + i_x, 20 + i_y, 0, "1", narrow, width, 50, Convert.ToChar(78), s_barcode);
                // end

                i_return = B_Prn_Text_TrueType(65 + i_x, 70 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A1", s_barcode);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable04_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //i_return = B_Prn_Barcode(65 + i_x, 20 + i_y, 0, "1", 4, 5, 90, Convert.ToChar(66), s_barcode);

                //add by genchille.yang
                //2013-05-18 18:46:00
                int narrow = 0;
                int width = 0;
                try
                {
                    if (Convert.ToUInt64(s_barcode) > 0)
                    {
                        narrow = 4;
                        width = 5;
                    }
                }
                catch (Exception ex)
                {
                    narrow = 3;
                    width = 6;
                }
                i_return = B_Prn_Barcode(65 + i_x, 20 + i_y, 0, "1", narrow, width, 90, Convert.ToChar(66), s_barcode);
                // end

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable05(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;

            try
            {
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Text_TrueType(200 + i_x, 55 + i_y, 48, "Arial", 1, 400, 0, 0, 0, "A1", s_barcode);
                i_return = B_Prn_Barcode(50 + i_x, 100 + i_y, 0, "1E", 5, 4, 60, Convert.ToChar(78), s_barcode);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable06(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;

            try
            {
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date;
                i_return = B_Prn_Barcode(130 + i_x, 50 + i_y, 0, "1E", 2, 10, 180, Convert.ToChar(78), s_printcode);

                s_value = "S/N: " + s_barcode;
                i_return = B_Prn_Text_TrueType(130 + i_x, 230 + i_y, 75, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable07(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;
            string s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);

                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);

                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Umpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);

                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);

                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Uoc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);

                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);

                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);

                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);

                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);

                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);

                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);

                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);

                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);

                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);

                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);

                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);

                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);

                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable08(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;
            string s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");

                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);

                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);

                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Umpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);

                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);

                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Uoc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);

                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);

                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);

                s_value = "600V(UL)";
                i_return = B_Prn_Text_TrueType(703 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);

                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);

                s_value = "UL 1703";
                i_return = B_Prn_Text_TrueType_W(720 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);

                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);

                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);

                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);

                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);

                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);

                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);

                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);

                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConUL.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Factory ID:S";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 52, "Basemic Times", 1, 700, 0, 0, 0, "A213", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable09(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "S/N:";
                i_return = B_Prn_Text_TrueType(540 + i_x, 5 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                s_value = s_modulecode + s_barcode;
                i_return = B_Prn_Barcode(200 + i_x, 40 + i_y, 0, "1", 5, 4, 75, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(395 + i_x, 115 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A2", s_value);

                s_value = "Measured Values (at STC)";
                i_return = B_Prn_Text_TrueType(390 + i_x, 165 + i_y, 38, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                s_value = "(1000W/m ; AM 1.5; 25℃)";
                i_return = B_Prn_Text_TrueType(390 + i_x, 205 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                i_return = B_Prn_Text_TrueType(540 + i_x, 200 + i_y, 24, "Arial", 1, 400, 0, 0, 0, "A41", "2");

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 240 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(410 + i_x, 260 + i_y, 18, "Arial", 1, 600, 0, 0, 0, "A51", "MPP");

                s_value = dc_pm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 240 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                s_value = "Wp";
                i_return = B_Prn_Text_TrueType(770 + i_x, 240 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A53", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 282 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A6", s_value);

                i_return = B_Prn_Text_TrueType(415 + i_x, 302 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A61", "MPP");
                s_value = dc_vpm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 282 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A62", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType(770 + i_x, 282 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A63", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 324 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A7", s_value);
                i_return = B_Prn_Text_TrueType(400 + i_x, 344 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A71", "MPP");
                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 324 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A72", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(770 + i_x, 324 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A73", s_value);

                s_value = "U   :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 363 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A8", s_value);

                i_return = B_Prn_Text_TrueType(413 + i_x, 383 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A81", "OC");
                s_value = dc_voc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 363 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A82", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType(770 + i_x, 363 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A83", s_value);

                s_value = "I   :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 405 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(400 + i_x, 425 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A91", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 405 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A92", s_value);

                s_value = "A";
                i_return = B_Prn_Text_TrueType(770 + i_x, 405 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A93", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable10(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(990 + i_x, 20 + i_y, s_resule + "Schu01.bmp");

                s_value = "Schüco USA L.P.";
                i_return = B_Prn_Text_TrueType(940 + i_x, 65 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A1", s_value);

                s_value = "Photovoltaic Module";
                i_return = B_Prn_Text_TrueType(937 + i_x, 450 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);

                s_value = "240 Pane Road RD";
                i_return = B_Prn_Text_TrueType(910 + i_x, 65 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A3", s_value);

                s_value = "Newington, CT 06111";
                i_return = B_Prn_Text_TrueType(880 + i_x, 65 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A4", s_value);

                s_value = "USA";
                i_return = B_Prn_Text_TrueType(850 + i_x, 65 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A5", s_value);

                s_value = "MPE " + s_module + " PS 09";
                i_return = B_Prn_Text_TrueType(892 + i_x, 450 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);

                s_value = "Art.-no.:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(849 + i_x, 450 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(776 + i_x, 190 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                s_value = dc_pm.ToString("#,##0");
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A82", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType_W(785 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                s_value = "(TolerancesP     : -0/+5%)";
                i_return = B_Prn_Text_TrueType_W(730 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(728 + i_x, 385 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                i_return = B_Prn_Text_TrueType(676 + i_x, 189 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                s_value = dc_vpm.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A102", s_value);

                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(685 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                i_return = B_Prn_Text_TrueType(626 + i_x, 182 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");

                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A112", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(635 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                i_return = B_Prn_Text_TrueType(576 + i_x, 188 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                s_value = dc_voc.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A122", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(585 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                s_value = "I    :";
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                i_return = B_Prn_Text_TrueType(526 + i_x, 180 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A132", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(535 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);

                s_value = "Data measured at STC:(1000W/m  ; AM 1.5; 25℃)";
                i_return = B_Prn_Text_TrueType(490 + i_x, 132 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                i_return = B_Prn_Text_TrueType(505 + i_x, 508 + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A151", "2");

                i_return = B_Get_Graphic_ColorBMP(132 + i_x, 120 + i_y, s_resule + "Schu03.bmp");

                i_return = B_Draw_Box(480 + i_x, 35 + i_y, 3, 133 + i_x, 760 + i_y);

                s_value = "Max. System Voltage:";
                i_return = B_Prn_Text_TrueType(430 + i_x, 50 + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);
                s_value = "600 VDC";
                i_return = B_Prn_Text_TrueType(430 + i_x, 570 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);

                s_value = "Fire Rating:";
                i_return = B_Prn_Text_TrueType(390 + i_x, 50 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(390 + i_x, 570 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A171", s_value);

                s_value = "Maximum Series Fuse Rating:";
                i_return = B_Prn_Text_TrueType(350 + i_x, 50 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A18", s_value);
                s_value = "15 A";
                i_return = B_Prn_Text_TrueType(350 + i_x, 570 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A181", s_value);
                s_value = "For field connections, use minimum No.12";
                i_return = B_Prn_Text_TrueType(310 + i_x, 50 + i_y, 37, "Arial", 2, 400, 0, 0, 0, "A182", s_value);
                s_value = "AWG copper wires insulated for a minimum of";
                i_return = B_Prn_Text_TrueType(270 + i_x, 50 + i_y, 37, "Arial", 2, 400, 0, 0, 0, "A183", s_value);
                s_value = "90°C";
                i_return = B_Prn_Text_TrueType(230 + i_x, 50 + i_y, 36, "Arial", 2, 400, 0, 0, 0, "A185", s_value);

                s_value = "Warning - Electrical Hazard:";
                i_return = B_Prn_Text_TrueType(95 + i_x, 265 + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                s_value = "HAZARDOUS ELECTRICITY CAN";
                i_return = B_Prn_Text_TrueType(65 + i_x, 205 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                s_value = "SHOCK, BURN OR DEATH!";
                i_return = B_Prn_Text_TrueType(35 + i_x, 247 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);
                s_value = "DO NOT TOUCH TERMINAL";
                i_return = B_Prn_Text_TrueType(5 + i_x, 235 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A193", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable11(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_module = s_module.Substring(1, s_module.Length - 1);
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(990 + i_x, 20 + i_y, s_resule + "Schu01.bmp");
                s_value = "Photovoltaic module";
                i_return = B_Prn_Text_TrueType(937 + i_x, 235 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);

                s_value = "MPE " + s_module + " PS 09";
                i_return = B_Prn_Text_TrueType(892 + i_x, 270 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);

                s_value = "Art.-no.:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(849 + i_x, 275 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(776 + i_x, 185 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                s_value = dc_pm.ToString("#,##0");
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A82", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType_W(785 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                s_value = "(TolerancesP     : -0/+5%)";
                i_return = B_Prn_Text_TrueType_W(730 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(728 + i_x, 385 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                i_return = B_Prn_Text_TrueType(676 + i_x, 189 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                s_value = dc_vpm.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A102", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(685 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                i_return = B_Prn_Text_TrueType(626 + i_x, 182 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");
                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A112", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(635 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                i_return = B_Prn_Text_TrueType(576 + i_x, 188 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                s_value = dc_voc.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A122", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(585 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                s_value = "I    :";
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                i_return = B_Prn_Text_TrueType(526 + i_x, 180 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A132", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(535 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);

                s_value = "Data measured at STC:";
                i_return = B_Prn_Text_TrueType(490 + i_x, 280 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                s_value = "(1000W/m  ; AM 1.5; 25";
                i_return = B_Prn_Text_TrueType(460 + i_x, 260 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A151", s_value);
                i_return = B_Prn_Text_TrueType(470 + i_x, 375 + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A152", "2");
                i_return = B_Prn_Text_TrueType(453 + i_x, 525 + i_y, 32, "Arial", 2, 600, 0, 0, 0, "A153", "℃");
                i_return = B_Prn_Text_TrueType(460 + i_x, 555 + i_y, 28, "Arial", 2, 500, 0, 0, 0, "A154", ")");

                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 220 + i_y, s_resule + "Schu05.bmp");
                s_value = "Produced in accordance";
                i_return = B_Prn_Text_TrueType(390 + i_x, 220 + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);
                s_value = "with IEC 61215";
                i_return = B_Prn_Text_TrueType(350 + i_x, 260 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);

                s_value = "Max.System";
                i_return = B_Prn_Text_TrueType(310 + i_x, 280 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);
                s_value = "Voltage 1000V";
                i_return = B_Prn_Text_TrueType(270 + i_x, 260 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A171", s_value);

                s_value = "Warning - Electrical Hazard:";
                i_return = B_Prn_Text_TrueType(110 + i_x, 290 + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                s_value = "Module generates DC current when";
                i_return = B_Prn_Text_TrueType(80 + i_x, 250 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                s_value = "exposed to sunlight or other light sources";
                i_return = B_Prn_Text_TrueType(50 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable12(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(495 + i_x, 10 + i_y, s_resule + "astr.bmp");

                i_return = B_Prn_Text_TrueType(28 + i_x, 90 + i_y, 56, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 155 + i_y, 480, 10);

                i_return = B_Prn_Text_TrueType(28 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(515 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 217 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(515 + i_x, 217 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A5", s_value);
                i_return = B_Prn_Text_TrueType(28 + i_x, 249 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 249 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 281 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 281 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 313 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 313 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 345 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 345 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 377 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 377 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A15", "10A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 409 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 409 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A17", "DC1000V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 441 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 441 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");

                i_return = B_Prn_Text_TrueType(28 + i_x, 471 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(515 + i_x, 471 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");

                i_return = B_Prn_Text_TrueType(28 + i_x, 503 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 503 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A23", "Mono-Si");

                i_return = B_Prn_Text_TrueType(28 + i_x, 535 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A24", "Module Application:Class A");
                i_return = B_Prn_Text_TrueType(490 + i_x, 535 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A25", "Module Safety Class Ⅱ");

                i_return = B_Draw_Box(730 + i_x, 535 + i_y, 2, 755 + i_x, 560 + i_y);
                i_return = B_Draw_Box(735 + i_x, 540 + i_y, 2, 750 + i_x, 555 + i_y);

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 575 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 585 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A251", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 620 + i_y, 800, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 635 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 675 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 710 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 745 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 780 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(710 + i_x, 750 + i_y, s_resule + "power.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 820 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 830 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 865 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 900 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 935 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 970 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(385 + i_x, 900 + i_y, s_resule + "TUV.bmp");

                i_return = B_Get_Graphic_ColorBMP(525 + i_x, 900 + i_y, s_resule + "ce.bmp");

                i_return = B_Get_Graphic_ColorBMP(680 + i_x, 895 + i_y, s_resule + "pass.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable12_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(760 + i_x, 3 + i_y, s_resule + "astr3.bmp");

                i_return = B_Prn_Text_TrueType(38 + i_x, 120 + i_y, 69, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 195 + i_y, 700, 12);

                i_return = B_Prn_Text_TrueType(28 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(820 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(820 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A15", "10A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A17", "DC1000V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");
                i_return = B_Prn_Text_TrueType(28 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(820 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");
                i_return = B_Prn_Text_TrueType(28 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A23", "Mono-Si");
                i_return = B_Prn_Text_TrueType(28 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A24", "Module Application:Class A");
                i_return = B_Prn_Text_TrueType(740 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A25", "Module Safety Class Ⅱ");

                i_return = B_Draw_Box(1085 + i_x, 715 + i_y, 2, 1135 + i_x, 765 + i_y);
                i_return = B_Draw_Box(1097 + i_x, 727 + i_y, 2, 1123 + i_x, 753 + i_y);
                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 780 + i_y, 1170, 4);
                i_return = B_Prn_Text_TrueType(28 + i_x, 800 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A251", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");
                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 850 + i_y, 1170, 4);
                i_return = B_Prn_Text_TrueType(28 + i_x, 875 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 930 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 980 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1030 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1080 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(1025 + i_x, 1038 + i_y, s_resule + "flash.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 1150 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 1180 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1300 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1360 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1420 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(545 + i_x, 1300 + i_y, s_resule + "TUV3.bmp");
                i_return = B_Get_Graphic_ColorBMP(715 + i_x, 1310 + i_y, s_resule + "ce3.bmp");
                i_return = B_Get_Graphic_ColorBMP(960 + i_x, 1300 + i_y, s_resule + "pass3.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable13(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(495 + i_x, 10 + i_y, s_resule + "astr.bmp");

                i_return = B_Prn_Text_TrueType(28 + i_x, 90 + i_y, 56, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 155 + i_y, 480, 10);

                i_return = B_Prn_Text_TrueType(28 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(515 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 217 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(515 + i_x, 217 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 249 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 249 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 281 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 281 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 313 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 313 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 345 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 345 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 377 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 377 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 409 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 409 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A17", "DC1000V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 441 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 441 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");
                i_return = B_Prn_Text_TrueType(28 + i_x, 471 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(515 + i_x, 471 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");
                i_return = B_Prn_Text_TrueType(28 + i_x, 503 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 503 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A23", "Poly-Si");
                i_return = B_Prn_Text_TrueType(28 + i_x, 535 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A24", "Module Application:Class A");
                i_return = B_Prn_Text_TrueType(490 + i_x, 535 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A25", "Module Safety Class Ⅱ");

                i_return = B_Draw_Box(730 + i_x, 535 + i_y, 2, 755 + i_x, 560 + i_y);
                i_return = B_Draw_Box(735 + i_x, 540 + i_y, 2, 750 + i_x, 555 + i_y);

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 575 + i_y, 795, 1);
                i_return = B_Prn_Text_TrueType(28 + i_x, 585 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A251", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");
                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 620 + i_y, 800, 1);
                i_return = B_Prn_Text_TrueType(28 + i_x, 635 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 675 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 710 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 745 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 780 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(710 + i_x, 750 + i_y, s_resule + "power.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 820 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 830 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 865 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");

                i_return = B_Prn_Text_TrueType(28 + i_x, 900 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 935 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 970 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(385 + i_x, 900 + i_y, s_resule + "TUV.bmp");
                i_return = B_Get_Graphic_ColorBMP(525 + i_x, 900 + i_y, s_resule + "ce.bmp");
                i_return = B_Get_Graphic_ColorBMP(680 + i_x, 895 + i_y, s_resule + "pass.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable13_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(760 + i_x, 3 + i_y, s_resule + "astr3.bmp");

                i_return = B_Prn_Text_TrueType(38 + i_x, 120 + i_y, 69, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 195 + i_y, 700, 12);

                i_return = B_Prn_Text_TrueType(28 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(820 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(820 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A17", "DC1000V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");
                i_return = B_Prn_Text_TrueType(28 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(820 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");
                i_return = B_Prn_Text_TrueType(28 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A23", "Poly-Si");
                i_return = B_Prn_Text_TrueType(28 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A24", "Module Application:Class A");
                i_return = B_Prn_Text_TrueType(740 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A25", "Module Safety Class Ⅱ");

                i_return = B_Draw_Box(1085 + i_x, 715 + i_y, 3, 1135 + i_x, 765 + i_y);
                i_return = B_Draw_Box(1097 + i_x, 727 + i_y, 3, 1123 + i_x, 753 + i_y);

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 780 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 800 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A251", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 850 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 875 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 930 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 980 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");

                i_return = B_Prn_Text_TrueType(28 + i_x, 1030 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1080 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(1025 + i_x, 1038 + i_y, s_resule + "flash.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 1150 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 1180 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1300 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1360 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1420 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(545 + i_x, 1300 + i_y, s_resule + "TUV3.bmp");
                i_return = B_Get_Graphic_ColorBMP(715 + i_x, 1310 + i_y, s_resule + "ce3.bmp");
                i_return = B_Get_Graphic_ColorBMP(960 + i_x, 1300 + i_y, s_resule + "pass3.bmp");
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable14(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(495 + i_x, 10 + i_y, s_resule + "astr.bmp");

                i_return = B_Prn_Text_TrueType(28 + i_x, 90 + i_y, 56, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 155 + i_y, 480, 10);

                i_return = B_Prn_Text_TrueType(28 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(515 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 215 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(515 + i_x, 215 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 245 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 245 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 275 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 275 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 305 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 305 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 335 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 335 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 365 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 365 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 395 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 395 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A17", "DC600V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 425 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 425 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");

                i_return = B_Prn_Text_TrueType(28 + i_x, 455 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(515 + i_x, 455 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");

                i_return = B_Prn_Text_TrueType(28 + i_x, 485 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 485 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A23", "Poly-Si");

                i_return = B_Prn_Text_TrueType(28 + i_x, 515 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A24", "Fire Rating:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 515 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A25", "Class C");

                i_return = B_Prn_Text_TrueType(28 + i_x, 545 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A251", "Field Connection:Min. 12AWG copper wires insulated for a Min. of 90°C");
                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 575 + i_y, 795, 1);
                i_return = B_Prn_Text_TrueType(28 + i_x, 585 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A252", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 620 + i_y, 800, 1);
                i_return = B_Prn_Text_TrueType(28 + i_x, 635 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 675 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 710 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 745 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 780 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(710 + i_x, 750 + i_y, s_resule + "power.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 820 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 830 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 865 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 900 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 935 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 970 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(365 + i_x, 895 + i_y, s_resule + "UL.bmp");
                i_return = B_Get_Graphic_ColorBMP(525 + i_x, 900 + i_y, s_resule + "ce.bmp");
                i_return = B_Get_Graphic_ColorBMP(680 + i_x, 895 + i_y, s_resule + "pass.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable14_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(760 + i_x, 3 + i_y, s_resule + "astr3.bmp");

                i_return = B_Prn_Text_TrueType(38 + i_x, 120 + i_y, 69, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 195 + i_y, 700, 12);

                i_return = B_Prn_Text_TrueType(28 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(820 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(820 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A17", "DC600V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");
                i_return = B_Prn_Text_TrueType(28 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(820 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");
                i_return = B_Prn_Text_TrueType(28 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A23", "Poly-Si");
                i_return = B_Prn_Text_TrueType(28 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A24", "Fire Rating:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A25", "Class C");
                i_return = B_Prn_Text_TrueType(28 + i_x, 768 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A251", "Field Connection:Min. 12AWG copper wires insulated for a Min. of 90°C");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 820 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 840 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A252", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 890 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 910 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 960 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1010 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1060 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1110 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(1030 + i_x, 1063 + i_y, s_resule + "flash.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 1170 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 1200 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1260 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1320 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1380 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1440 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(480 + i_x, 1300 + i_y, s_resule + "UL3.bmp");
                i_return = B_Get_Graphic_ColorBMP(715 + i_x, 1330 + i_y, s_resule + "ce3.bmp");
                i_return = B_Get_Graphic_ColorBMP(960 + i_x, 1320 + i_y, s_resule + "pass3.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable15(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(495 + i_x, 10 + i_y, s_resule + "astr.bmp");

                i_return = B_Prn_Text_TrueType(28 + i_x, 90 + i_y, 56, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 155 + i_y, 480, 10);

                i_return = B_Prn_Text_TrueType(28 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(515 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 217 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(515 + i_x, 217 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 249 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 249 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 281 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 281 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 313 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 313 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 345 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 345 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 377 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 377 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 409 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 409 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A17", "DC1000V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 441 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 441 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");

                i_return = B_Prn_Text_TrueType(28 + i_x, 471 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(515 + i_x, 471 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");

                i_return = B_Prn_Text_TrueType(28 + i_x, 503 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 503 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A23", "Mono-Si");

                i_return = B_Prn_Text_TrueType(28 + i_x, 535 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A24", "Module Application:Class A");
                i_return = B_Prn_Text_TrueType(490 + i_x, 535 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A25", "Module Safety Class Ⅱ");

                i_return = B_Draw_Box(730 + i_x, 535 + i_y, 2, 755 + i_x, 560 + i_y);
                i_return = B_Draw_Box(735 + i_x, 540 + i_y, 2, 750 + i_x, 555 + i_y);

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 575 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 585 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A251", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 620 + i_y, 800, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 635 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 675 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 710 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 745 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 780 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(710 + i_x, 750 + i_y, s_resule + "power.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 820 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 830 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 865 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 900 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 935 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 970 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(385 + i_x, 900 + i_y, s_resule + "TUV.bmp");
                i_return = B_Get_Graphic_ColorBMP(525 + i_x, 900 + i_y, s_resule + "ce.bmp");
                i_return = B_Get_Graphic_ColorBMP(680 + i_x, 895 + i_y, s_resule + "pass.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable15_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(760 + i_x, 3 + i_y, s_resule + "astr3.bmp");
                i_return = B_Prn_Text_TrueType(38 + i_x, 120 + i_y, 69, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 195 + i_y, 700, 12);

                i_return = B_Prn_Text_TrueType(28 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(820 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(820 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A17", "DC1000V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");

                i_return = B_Prn_Text_TrueType(28 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(820 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");

                i_return = B_Prn_Text_TrueType(28 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A23", "Mono-Si");

                i_return = B_Prn_Text_TrueType(28 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A24", "Module Application:Class A");
                i_return = B_Prn_Text_TrueType(740 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A25", "Module Safety Class Ⅱ");

                i_return = B_Draw_Box(1085 + i_x, 715 + i_y, 3, 1135 + i_x, 765 + i_y); ;
                i_return = B_Draw_Box(1097 + i_x, 727 + i_y, 3, 1123 + i_x, 753 + i_y); ;

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 780 + i_y, 1170, 4);
                i_return = B_Prn_Text_TrueType(28 + i_x, 800 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A251", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 850 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 875 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 930 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 980 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1030 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1080 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(1025 + i_x, 1038 + i_y, s_resule + "flash.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 1150 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 1180 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1300 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1360 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1420 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(545 + i_x, 1300 + i_y, s_resule + "TUV3.bmp");
                i_return = B_Get_Graphic_ColorBMP(715 + i_x, 1310 + i_y, s_resule + "ce3.bmp");
                i_return = B_Get_Graphic_ColorBMP(960 + i_x, 1300 + i_y, s_resule + "pass3.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable16(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(495 + i_x, 10 + i_y, s_resule + "astr.bmp");

                i_return = B_Prn_Text_TrueType(28 + i_x, 90 + i_y, 56, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 155 + i_y, 480, 10);

                i_return = B_Prn_Text_TrueType(28 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(515 + i_x, 185 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 215 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(515 + i_x, 215 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 245 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 245 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 275 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 275 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 305 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(515 + i_x, 305 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 335 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(515 + i_x, 335 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 365 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 365 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 395 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 395 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A17", "DC600V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 425 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(515 + i_x, 425 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");

                i_return = B_Prn_Text_TrueType(28 + i_x, 455 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(515 + i_x, 455 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");

                i_return = B_Prn_Text_TrueType(28 + i_x, 485 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 485 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A23", "Mono-Si");

                i_return = B_Prn_Text_TrueType(28 + i_x, 515 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A24", "Fire Rating:");
                i_return = B_Prn_Text_TrueType(515 + i_x, 515 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A25", "Class C");

                i_return = B_Prn_Text_TrueType(28 + i_x, 545 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A251", "Field Connection:Min. 12AWG copper wires insulated for a Min. of 90°C");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 575 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 585 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A252", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 620 + i_y, 800, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 635 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 675 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");

                i_return = B_Prn_Text_TrueType(28 + i_x, 710 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 745 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 780 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(710 + i_x, 750 + i_y, s_resule + "power.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 820 + i_y, 795, 1);

                i_return = B_Prn_Text_TrueType(28 + i_x, 830 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 865 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 900 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 935 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 970 + i_y, 26, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(365 + i_x, 895 + i_y, s_resule + "UL.bmp");
                i_return = B_Get_Graphic_ColorBMP(525 + i_x, 900 + i_y, s_resule + "ce.bmp");
                i_return = B_Get_Graphic_ColorBMP(680 + i_x, 895 + i_y, s_resule + "pass.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable16_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(760 + i_x, 3 + i_y, s_resule + "astr3.bmp");

                i_return = B_Prn_Text_TrueType(38 + i_x, 120 + i_y, 69, "Arial", 1, 400, 0, 0, 0, "A1", "Photovoltaic Module");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 195 + i_y, 700, 12);

                i_return = B_Prn_Text_TrueType(28 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A2", "Module Name:");
                s_value = s_module;
                i_return = B_Prn_Text_TrueType(820 + i_x, 240 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A4", "Maximum Power:");
                s_value = dc_pm.ToString("#,##0.0") + "Wp";
                i_return = B_Prn_Text_TrueType(820 + i_x, 284 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A5", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A6", "Open Circuit Voltage(Voc):");
                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 328 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A7", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A8", "Short Circuit Current(Isc):");
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 372 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A10", "Voltage at Pmax(Vmp):");
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(820 + i_x, 416 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A11", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A12", "Current at Pmax(Imp):");
                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 460 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A13", s_value);

                i_return = B_Prn_Text_TrueType(28 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A14", "Fuse Rating: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 504 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A15", "15A");

                i_return = B_Prn_Text_TrueType(28 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A16", "Maximum System Voltage: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 548 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A17", "DC600V");

                i_return = B_Prn_Text_TrueType(28 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A18", "Power Tolerance: ");
                i_return = B_Prn_Text_TrueType(820 + i_x, 592 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A19", "±3%");

                i_return = B_Prn_Text_TrueType(28 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A20", "Nominal Operating Cell Temp(NOCT):");
                i_return = B_Prn_Text_TrueType(820 + i_x, 636 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A21", "47 ±2℃");

                i_return = B_Prn_Text_TrueType(28 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A22", "Cell Technology:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 680 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A23", "Mono-Si");

                i_return = B_Prn_Text_TrueType(28 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A24", "Fire Rating:");
                i_return = B_Prn_Text_TrueType(820 + i_x, 724 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A25", "Class C");

                i_return = B_Prn_Text_TrueType(28 + i_x, 768 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A251", "Field Connection:Min. 12AWG copper wires insulated for a Min. of 90°C");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 820 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 840 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A252", "All technical data at standard test condition(STC):(AM1.5,1000W/㎡,25℃)");

                i_return = B_Draw_Line(Convert.ToChar(79), 5 + i_x, 890 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 910 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A26", "Warning:");
                i_return = B_Prn_Text_TrueType(28 + i_x, 960 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A27", "Solar modules generate electricity as soon as they are exposed to light.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1010 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A28", "One module on its own is below the safety extra low volt level,but multiple");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1060 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A281", "modules connected in series(summing the voltage)or in parallel");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1110 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A282", "(summing the current)represent a danger.");

                i_return = B_Get_Graphic_ColorBMP(1030 + i_x, 1063 + i_y, s_resule + "flash.bmp");

                i_return = B_Draw_Line(Convert.ToChar(79), 10 + i_x, 1170 + i_y, 1170, 4);

                i_return = B_Prn_Text_TrueType(28 + i_x, 1200 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A29", "Company Name:CHINT SOLAR(ZHEJIANG) Co.,Ltd.");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1260 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A30", "Add:1335 Binan Rd,Binjiang District,Hangzhou,310053,China");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1320 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A31", "Tel :+86-571-56031888");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1380 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A32", "Fax:+86-571-56031800");
                i_return = B_Prn_Text_TrueType(28 + i_x, 1440 + i_y, 36, "Arial", 1, 400, 0, 0, 0, "A33", "http://www.astronergy.com");

                i_return = B_Get_Graphic_ColorBMP(480 + i_x, 1300 + i_y, s_resule + "UL3.bmp");
                i_return = B_Get_Graphic_ColorBMP(715 + i_x, 1330 + i_y, s_resule + "ce3.bmp");
                i_return = B_Get_Graphic_ColorBMP(960 + i_x, 1320 + i_y, s_resule + "pass3.bmp");

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable17(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(990 + i_x, 20 + i_y, s_resule + "Schu01.bmp");

                s_value = "Photovoltaic module";
                i_return = B_Prn_Text_TrueType(937 + i_x, 165 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);
                s_value = "MPE " + s_module + " PS 09";
                i_return = B_Prn_Text_TrueType(892 + i_x, 165 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);

                s_value = "Art.-no.:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(849 + i_x, 165 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);

                i_return = B_Prn_Text_TrueType(776 + i_x, 185 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                s_value = dc_pm.ToString("#,##0");
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A82", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType_W(785 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                s_value = "(TolerancesP     : -0/+5%)";
                i_return = B_Prn_Text_TrueType_W(730 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(728 + i_x, 385 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                i_return = B_Prn_Text_TrueType(676 + i_x, 189 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                s_value = dc_vpm.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A102", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(685 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                i_return = B_Prn_Text_TrueType(626 + i_x, 182 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");
                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A112", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(635 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                i_return = B_Prn_Text_TrueType(576 + i_x, 188 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                s_value = dc_voc.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A122", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(585 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                s_value = "I    :";
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                i_return = B_Prn_Text_TrueType(526 + i_x, 180 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A132", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(535 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);

                s_value = "Data measured at STC:";
                i_return = B_Prn_Text_TrueType(490 + i_x, 260 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                s_value = "(1000W/m  ; AM 1.5; 25℃)";
                i_return = B_Prn_Text_TrueType(460 + i_x, 260 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A151", s_value);
                i_return = B_Prn_Text_TrueType(475 + i_x, 375 + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A152", "2");

                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 220 + i_y, s_resule + "Schu05.bmp");

                i_return = B_Draw_Box(440 + i_x, 180 + i_y, 3, 160 + i_x, 600 + i_y);

                s_value = "Produced in accordance";
                i_return = B_Prn_Text_TrueType(390 + i_x, 220 + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);
                s_value = "with IEC 61215-2";
                i_return = B_Prn_Text_TrueType(350 + i_x, 220 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);
                s_value = "Max.System";
                i_return = B_Prn_Text_TrueType(310 + i_x, 220 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);
                s_value = "Voltage 1000V";
                i_return = B_Prn_Text_TrueType(270 + i_x, 220 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A171", s_value);
                s_value = "Warning - Electrical Hazard:";
                i_return = B_Prn_Text_TrueType(110 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                s_value = "Module generates DC current when";
                i_return = B_Prn_Text_TrueType(80 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                s_value = "exposed to sunlight or other light sources";
                i_return = B_Prn_Text_TrueType(50 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable18(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(990 + i_x, 20 + i_y, s_resule + "Schu01.bmp");

                s_value = "Photovoltaic module";
                i_return = B_Prn_Text_TrueType(937 + i_x, 165 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);
                s_value = "MPE " + s_module + " PS 09";
                i_return = B_Prn_Text_TrueType(892 + i_x, 165 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);
                s_value = "Art.-no.:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(849 + i_x, 165 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(776 + i_x, 185 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                s_value = dc_pm.ToString("#,##0");
                i_return = B_Prn_Text_TrueType_W(780 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A82", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType_W(785 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                s_value = "(TolerancesP     : -0/+5%)";
                i_return = B_Prn_Text_TrueType_W(730 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(728 + i_x, 385 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                i_return = B_Prn_Text_TrueType(676 + i_x, 189 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                s_value = dc_vpm.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(680 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A102", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(685 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                i_return = B_Prn_Text_TrueType(626 + i_x, 182 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");
                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(630 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A112", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(635 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                i_return = B_Prn_Text_TrueType(576 + i_x, 188 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                s_value = dc_voc.ToString("#,##0.0");
                i_return = B_Prn_Text_TrueType_W(580 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A122", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(585 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                s_value = "I    :";
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                i_return = B_Prn_Text_TrueType(526 + i_x, 180 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 485 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A132", s_value);
                s_value = "A";

                i_return = B_Prn_Text_TrueType_W(535 + i_x, 600 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);
                s_value = "Data measured at STC:(1000W/m  ; AM 1.5; 25℃)";
                i_return = B_Prn_Text_TrueType(490 + i_x, 132 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                i_return = B_Prn_Text_TrueType(505 + i_x, 508 + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A151", "2");

                i_return = B_Get_Graphic_ColorBMP(132 + i_x, 120 + i_y, s_resule + "Schu03.bmp");

                i_return = B_Draw_Box(480 + i_x, 35 + i_y, 3, 133 + i_x, 760 + i_y);

                s_value = "Max. System Voltage:";
                i_return = B_Prn_Text_TrueType(430 + i_x, 50 + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);
                s_value = "600 VDC";
                i_return = B_Prn_Text_TrueType(430 + i_x, 570 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);
                s_value = "Fire Rating:";
                i_return = B_Prn_Text_TrueType(390 + i_x, 50 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(390 + i_x, 570 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A171", s_value);
                s_value = "Maximum Series Fuse Rating:";
                i_return = B_Prn_Text_TrueType(350 + i_x, 50 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A18", s_value);
                s_value = "15 A";
                i_return = B_Prn_Text_TrueType(350 + i_x, 570 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A181", s_value);
                s_value = "For field connections, use minimum No.12";
                i_return = B_Prn_Text_TrueType(310 + i_x, 50 + i_y, 37, "Arial", 2, 400, 0, 0, 0, "A182", s_value);
                s_value = "AWG copper wires insulated for a minimum of";
                i_return = B_Prn_Text_TrueType(270 + i_x, 50 + i_y, 37, "Arial", 2, 400, 0, 0, 0, "A183", s_value);
                s_value = "90°C";
                i_return = B_Prn_Text_TrueType(230 + i_x, 50 + i_y, 36, "Arial", 2, 400, 0, 0, 0, "A185", s_value);
                s_value = "Warning - Electrical Hazard:";
                i_return = B_Prn_Text_TrueType(95 + i_x, 265 + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                s_value = "HAZARDOUS ELECTRICITY CAN";
                i_return = B_Prn_Text_TrueType(65 + i_x, 205 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                s_value = "SHOCK, BURN OR DEATH!";
                i_return = B_Prn_Text_TrueType(35 + i_x, 247 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);
                s_value = "DO NOT TOUCH TERMINAL";
                i_return = B_Prn_Text_TrueType(5 + i_x, 235 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A193", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable19(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_module, s_modulecode;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_value = "S/N:";
                //-----------------------------------------------------------------------------------------------------
                //i_return = B_Prn_Text_TrueType(540 + i_x, 5 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
                //i_return = B_Prn_Text_TrueType(540 + i_x + 50, 5 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
                //------------------------------------------------------------------------------------------------------
                s_value = s_modulecode + s_barcode;
                //---------------------------------------------------------------------------------------------------
                //i_return = B_Prn_Barcode(167 + i_x, 40 + i_y, 0, "1", 4, 4, 120, Convert.ToChar(78), s_value);
                i_return = B_Prn_Barcode(167 + i_x + 125, 40 + i_y, 0, "1", 4, 4, 120, Convert.ToChar(78), s_value);
                //--------------------------------------------------------------------------------------------------
                i_return = B_Prn_Text_TrueType(435 + i_x, 166 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A2", s_value);

                s_value = "Measured Values (at STC)";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 190 + i_y, 38, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                s_value = "(1000W/m ; AM 1.5; 25";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 230 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                i_return = B_Prn_Text_TrueType(725 + i_x + 45, 230 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A42", "℃");
                i_return = B_Prn_Text_TrueType(760 + i_x + 45, 230 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A43", ")");
                i_return = B_Prn_Text_TrueType(540 + i_x + 45, 225 + i_y, 24, "Arial", 1, 400, 0, 0, 0, "A41", "2");

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 265 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A5", s_value);
                i_return = B_Prn_Text_TrueType(410 + i_x + 45, 285 + i_y, 18, "Arial", 1, 600, 0, 0, 0, "A51", "MPP");
                s_value = dc_pm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x + 45, 265 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A52", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType(770 + i_x + 45, 265 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A53", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 307 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A6", s_value);
                i_return = B_Prn_Text_TrueType(415 + i_x + 45, 327 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A61", "MPP");
                s_value = dc_vpm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x + 45, 307 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A62", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType(770 + i_x + 45, 307 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A63", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 349 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A7", s_value);
                i_return = B_Prn_Text_TrueType(400 + i_x + 45, 369 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A71", "MPP");
                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x + 45, 349 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A72", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(770 + i_x + 45, 349 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A73", s_value);

                s_value = "U   :";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 388 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(413 + i_x + 45, 408 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A81", "OC");
                s_value = dc_voc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x + 45, 388 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A82", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType(770 + i_x + 45, 388 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A83", s_value);

                s_value = "I   :";
                i_return = B_Prn_Text_TrueType(390 + i_x + 45, 430 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(400 + i_x + 45, 450 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A91", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x + 45, 430 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A92", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(770 + i_x + 45, 430 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A93", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable20(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "S/N:";
                i_return = B_Prn_Text_TrueType(540 + i_x, 5 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
                s_value = s_modulecode + s_barcode;
                i_return = B_Prn_Barcode(167 + i_x, 40 + i_y, 0, "1", 4, 4, 100, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(435 + i_x, 140 + i_y, 31, "Arial", 1, 500, 0, 0, 0, "A2", s_value);

                s_value = "Measured Values (at STC)";
                i_return = B_Prn_Text_TrueType(390 + i_x, 190 + i_y, 38, "Arial", 1, 400, 0, 0, 0, "A3", s_value);
                s_value = "(1000W/m ; AM 1.5; 25℃)";
                i_return = B_Prn_Text_TrueType(390 + i_x, 230 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                i_return = B_Prn_Text_TrueType(540 + i_x, 225 + i_y, 24, "Arial", 1, 400, 0, 0, 0, "A41", "2");

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 265 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A5", s_value);
                i_return = B_Prn_Text_TrueType(410 + i_x, 285 + i_y, 18, "Arial", 1, 600, 0, 0, 0, "A51", "MPP");
                s_value = dc_pm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 265 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A52", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType(770 + i_x, 265 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A53", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 307 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A6", s_value);
                i_return = B_Prn_Text_TrueType(415 + i_x, 327 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A61", "MPP");
                s_value = dc_vpm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 307 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A62", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType(770 + i_x, 307 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A63", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 349 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A7", s_value);
                i_return = B_Prn_Text_TrueType(400 + i_x, 369 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A71", "MPP");
                s_value = dc_ipm.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 349 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A72", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(770 + i_x, 349 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A73", s_value);

                s_value = "U   :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 388 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(413 + i_x, 408 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A81", "OC");
                s_value = dc_voc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 388 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A82", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType(770 + i_x, 388 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A83", s_value);

                s_value = "I   :";
                i_return = B_Prn_Text_TrueType(390 + i_x, 430 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(400 + i_x, 450 + i_y, 18, "Arial", 1, 800, 0, 0, 0, "A91", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType(660 + i_x, 430 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A92", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(770 + i_x, 430 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A93", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable21_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum, l_len;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(13 + i_x, 8 + i_y, 0, "1", 2, 3, 68, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(13 + i_x, 78 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                i_return = B_Prn_Text_TrueType(13 + i_x, 120 + i_y, 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
                i_return = B_Prn_Text_TrueType(385 + i_x, 5 + i_y, 35, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight(5 - l_len, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(453 + i_x, 5 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(385 + i_x, 46 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight(5 - l_len, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(453 + i_x, 46 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight(5 - l_len, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 89 + i_y, 33, "Arial", 1, 400, 0, 0, 0, "A42", s_value);
                i_return = B_Prn_Text_TrueType(385 + i_x, 89 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A91", "Vmp");

                i_return = B_Prn_Text_TrueType(385 + i_x, 130 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight(5 - l_len, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(453 + i_x, 130 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable22(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module, s_name;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;
            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 13;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_name = "Q_ok_" + s_module + ".bmp";
                i_return = B_Get_Graphic_ColorBMP(20 + i_x, 20 + i_y, s_resule + s_name);

                s_value = "Serial No. " + s_barcode;
                i_return = B_Prn_Text_TrueType(625 + i_x, 975 + i_y, 34, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
                s_value = s_barcode;
                i_return = B_Prn_Barcode(625 + i_x, 1015 + i_y, 0, "1", 4, 6, 76, Convert.ToChar(78), s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable23(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value;

            try
            {
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                if (i_darkness <= 0)
                {
                    i_darkness = 13;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = s_barcode;
                i_return = B_Prn_Text_TrueType(20 + i_x, 10 + i_y, 53, "Arial", 3, 500, 0, 0, 0, "A1", s_value);
                i_return = B_Prn_Barcode(60 + i_x, 70 + i_y, 0, "1", 3, 22, 66, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(20 + i_x, 140 + i_y, 53, "Arial", 1, 500, 0, 0, 0, "A2", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable24(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module, s_name;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;
            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 13;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_name = "Q_ng_" + s_module + ".bmp";
                i_return = B_Get_Graphic_ColorBMP(20 + i_x, 20 + i_y, s_resule + s_name);

                s_value = "Serial No. " + s_barcode;
                i_return = B_Prn_Text_TrueType(625 + i_x, 975 + i_y, 34, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
                s_value = s_barcode;
                i_return = B_Prn_Barcode(625 + i_x, 1015 + i_y, 0, "1", 4, 6, 76, Convert.ToChar(78), s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable25(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");

                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);
                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1180 + i_y, s_resule + "ConCE-E.bmp");
                i_return = B_Get_Graphic_ColorBMP(170 + i_x, 1175 + i_y, s_resule + "ConTUV-E.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Germany"; ;
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable26(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_module = s_module.Substring(1, s_module.Length - 1);
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(1011 + i_x, 20 + i_y, s_resule + "Schu01.bmp");

                i_return = B_Get_Graphic_ColorBMP(30 + i_x, 107 + i_y, s_resule + "flash-90.bmp");

                s_value = "Schüco International KG";
                i_return = B_Prn_Text_TrueType(980 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A1", s_value);

                s_value = "Photovoltaic module";
                i_return = B_Prn_Text_TrueType(980 + i_x, 500 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);

                s_value = "Karolinenstr.1-15";
                i_return = B_Prn_Text_TrueType(950 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A3", s_value);

                s_value = "33609 Bielefeld";
                i_return = B_Prn_Text_TrueType(920 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A4", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(890 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A5", s_value);

                s_value = "MPE " + s_module + " PG  09";
                i_return = B_Prn_Text_TrueType(940 + i_x, 500 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);

                s_value = "Art.-no.:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(900 + i_x, 500 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType_W(840 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(836 + i_x, 185 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                s_value = dc_pm.ToString("#,##0");
                i_return = B_Prn_Text_TrueType_W(820 + i_x, 615 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A82", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType_W(825 + i_x, 714 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                s_value = "(Tolerances P     : -0/+5%)";
                i_return = B_Prn_Text_TrueType_W(790 + i_x, 160 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(788 + i_x, 390 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(740 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                i_return = B_Prn_Text_TrueType(736 + i_x, 189 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                s_value = dc_vpm.ToString("#,##00.00");
                i_return = B_Prn_Text_TrueType_W(740 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A102", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(745 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType_W(690 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                i_return = B_Prn_Text_TrueType(686 + i_x, 182 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");
                //s_value = dc_ipm.ToString("#,##0.00");
                s_value = dc_voc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(690 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A112", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(695 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(640 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                i_return = B_Prn_Text_TrueType(636 + i_x, 188 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                //s_value = dc_voc.ToString("#,##00.00");
                s_value = dc_ipm.ToString("#,##00.00");
                i_return = B_Prn_Text_TrueType_W(640 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A122", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(645 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                s_value = "I    :";
                i_return = B_Prn_Text_TrueType_W(590 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                i_return = B_Prn_Text_TrueType(586 + i_x, 180 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(590 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A132", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(595 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);

                s_value = "Fusing Rating:";
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 170 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A14", s_value);
                s_value = "15";
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 619 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A141", s_value);
                i_return = B_Prn_Text_TrueType_W(530 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A142", "A");
                s_value = "Application Class:";
                i_return = B_Prn_Text_TrueType_W(490 + i_x, 170 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A143", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(490 + i_x, 628 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A144", s_value);

                s_value = "Data measured at STC:";
                i_return = B_Prn_Text_TrueType(464 + i_x, 280 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                s_value = "(1000W/m  ; AM 1.5; 25";
                i_return = B_Prn_Text_TrueType(428 + i_x, 260 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A151", s_value);
                i_return = B_Prn_Text_TrueType(439 + i_x, 375 + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A152", "2");
                i_return = B_Prn_Text_TrueType(423 + i_x, 525 + i_y, 32, "Arial", 2, 600, 0, 0, 0, "A153", "℃");
                i_return = B_Prn_Text_TrueType(428 + i_x, 555 + i_y, 28, "Arial", 2, 500, 0, 0, 0, "A154", ")");

                i_return = B_Get_Graphic_ColorBMP(130 + i_x, 170 + i_y, s_resule + "Schu06.bmp");

                s_value = "Produced in accordance";
                i_return = B_Prn_Text_TrueType(370 + i_x, 214 + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);
                s_value = "with IEC 61215";
                i_return = B_Prn_Text_TrueType(330 + i_x, 214 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);

                s_value = "Max.System Voltage 1000V";
                i_return = B_Prn_Text_TrueType(290 + i_x, 214 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);

                s_value = "Warning - Electrical Hazard:";
                i_return = B_Prn_Text_TrueType(80 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                s_value = "Module generates DC current when";
                i_return = B_Prn_Text_TrueType(50 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                s_value = "exposed to sunlight or other light sources";
                i_return = B_Prn_Text_TrueType(20 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);

                i_return = B_Draw_Box(119 + i_x, 130 + i_y, 3, 420 + i_x, 700 + i_y);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable27(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_modulecode, s_module;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                //dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                //dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                //dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                //dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                //dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                //i_pnum = int.Parse(s_array[6].ToString().Trim());
                //s_modulecode = s_array[7].ToString().Trim();
                //s_module = s_array[8].ToString().Trim();

                ////s_module = s_module.Substring(1, s_module.Length - 1);
                ////---------------------------------------------------------------------------------
                //s_module = s_module.Substring(0, 3);
                ////---------------------------------------------------------------------------------
                //s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                //if (i_darkness <= 0)
                //{
                //    i_darkness = 12;
                //}

                ////打开打印接口
                //i_return = B_CreatePrn(1, null);
                ////设置打印浓度
                //i_return = B_Set_Darkness(i_darkness);
                ////打印方向
                //i_return = B_Set_Direction(Convert.ToChar(66));
                ////清除内存图形
                //i_return = B_Initial_Setting(0, "N\r\n\0");
                //i_return = B_Del_Pcx("*");

                //i_return = B_Get_Graphic_ColorBMP(1011 + i_x, 20 + i_y, s_resule + "Schu01.bmp");
                //i_return = B_Get_Graphic_ColorBMP(30 + i_x, 107 + i_y, s_resule + "flash-90.bmp");

                //s_value = "Schüco International KG";
                //i_return = B_Prn_Text_TrueType(980 + i_x, 65  + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A1", s_value);

                //s_value = "Photovoltaic module";
                //i_return = B_Prn_Text_TrueType(980 + i_x, 500  + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);


                //s_value = "Karolinenstr.1-15";
                //i_return = B_Prn_Text_TrueType(950 + i_x, 65  + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A3", s_value);

                //s_value = "33609 Bielefeld";
                //i_return = B_Prn_Text_TrueType(920 + i_x, 65  + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A4", s_value);

                //s_value = "Germany";
                //i_return = B_Prn_Text_TrueType(890 + i_x, 65  + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A5",s_value);
                ////--------------------------------------------------------------------------------
                ////s_value = "MPE " + s_module + " PS 09";
                //s_value = "MPE " + s_module + " PG 09";
                ////--------------------------------------------------------------------------------
                //i_return = B_Prn_Text_TrueType(940 + i_x, 500  + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);

                //s_value = "Art.-no.:" + s_modulecode;
                //i_return = B_Prn_Text_TrueType(900 + i_x, 500  + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                //s_value = "P    :";
                //i_return = B_Prn_Text_TrueType_W(840 + i_x, 165  + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);
                //i_return = B_Prn_Text_TrueType(836 + i_x, 185  + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                //s_value = dc_pm.ToString("#,##0");
                //i_return = B_Prn_Text_TrueType_W(820 + i_x, 619 + i_y, 48, 17,"Arial", 2, 400, 0, 0, 0, "A82", s_value);
                //s_value = "Wp";
                //i_return = B_Prn_Text_TrueType_W(825 + i_x, 714  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                //s_value = "(Tolerances P     : -0/+5%)";
                //i_return = B_Prn_Text_TrueType_W(790 + i_x, 160  + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                //i_return = B_Prn_Text_TrueType(788 + i_x, 390  + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                //s_value = "U    :";
                //i_return = B_Prn_Text_TrueType_W(740 + i_x, 165  + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                //i_return = B_Prn_Text_TrueType(736 + i_x, 189  + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                //s_value = dc_vpm.ToString("#,##00.00");
                //i_return = B_Prn_Text_TrueType_W(740 + i_x, 619 + i_y, 48, 17,"Arial", 2, 400, 0, 0, 0, "A102", s_value);
                //s_value = "V";
                //i_return = B_Prn_Text_TrueType_W(745 + i_x, 715  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                //s_value = "I     :";
                //i_return = B_Prn_Text_TrueType_W(690 + i_x, 170  + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                //i_return = B_Prn_Text_TrueType(686 + i_x, 182  + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");
                ////s_value = dc_ipm.ToString("#,##0.00");
                //s_value = dc_voc.ToString("#,##0.00");
                //i_return = B_Prn_Text_TrueType_W(690 + i_x, 619 + i_y, 48, 17,"Arial", 2, 400, 0, 0, 0, "A112", s_value);
                //s_value = "A";
                //i_return = B_Prn_Text_TrueType_W(695 + i_x, 715  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                //s_value = "U    :";
                //i_return = B_Prn_Text_TrueType_W(640 + i_x, 165  + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                //i_return = B_Prn_Text_TrueType(636 + i_x, 188  + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                ////s_value = dc_voc.ToString("#,##00.00");
                //s_value = dc_ipm.ToString("#,##00.00");
                //i_return = B_Prn_Text_TrueType_W(640 + i_x, 619 + i_y, 48, 17,"Arial", 2, 400, 0, 0, 0, "A122", s_value);
                //s_value = "V";
                //i_return = B_Prn_Text_TrueType_W(645 + i_x, 715  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                //s_value = "I    :";
                //i_return = B_Prn_Text_TrueType_W(590 + i_x, 170  + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                //i_return = B_Prn_Text_TrueType(586 + i_x, 180  + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                //s_value = dc_isc.ToString("#,##0.00");
                //i_return = B_Prn_Text_TrueType_W(590 + i_x, 619 + i_y, 48, 17,"Arial", 2, 400, 0, 0, 0, "A132", s_value);
                //s_value = "A";
                //i_return = B_Prn_Text_TrueType_W(595 + i_x, 715  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);

                ////s_value="Fusing Rating:";
                //s_value = "Fuse Rating:";
                //i_return = B_Prn_Text_TrueType_W(530 + i_x, 170  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A14", s_value);
                //s_value="15";
                //i_return = B_Prn_Text_TrueType_W(530 + i_x, 619  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A141", s_value);
                //i_return = B_Prn_Text_TrueType_W(530 + i_x, 715  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A142", "A");
                //s_value="Application Class:";
                //i_return = B_Prn_Text_TrueType_W(490 + i_x, 170  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A143", s_value);
                //s_value="A";
                //i_return = B_Prn_Text_TrueType_W(490 + i_x, 628  + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A144", s_value);

                //s_value = "Data measured at STC:";
                //i_return = B_Prn_Text_TrueType(464 + i_x, 280  + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                //s_value = "(1000W/m  ; AM 1.5; 25";
                //i_return = B_Prn_Text_TrueType(428 + i_x, 260  + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A151", s_value);
                //i_return = B_Prn_Text_TrueType(439 + i_x, 375  + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A152", "2");
                //i_return = B_Prn_Text_TrueType(423 + i_x, 525  + i_y, 32, "Arial", 2, 600, 0, 0, 0, "A153", "℃");
                //i_return = B_Prn_Text_TrueType(428 + i_x, 555  + i_y, 28, "Arial", 2, 500, 0, 0, 0, "A154", ")");

                //i_return = B_Get_Graphic_ColorBMP(130 + i_x,200 + i_y,s_resule + "Schu06.bmp");

                //s_value = "Produced in accordance";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 214  + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);

                //s_value = "with IEC 61215";
                ////---------------------------------------------------------------------------------------------------
                ////i_return = B_Prn_Text_TrueType(330 + i_x, 214 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);
                //i_return = B_Prn_Text_TrueType(330 + i_x, 214 + i_y + 70, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);
                ////---------------------------------------------------------------------------------------------------
                //s_value = "Max.System Voltage 1000V";
                //i_return = B_Prn_Text_TrueType(290 + i_x, 214  + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);
                //s_value = "Warning - Electrical Hazard:";
                //i_return = B_Prn_Text_TrueType(80 + i_x, 210  + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                //s_value = "Module generates DC current when";
                //i_return = B_Prn_Text_TrueType(50 + i_x, 210  + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                //s_value = "exposed to sunlight or other light sources";
                //i_return = B_Prn_Text_TrueType(20 + i_x, 210  + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);

                //i_return = B_Draw_Box (119+i_x, 130+i_y,3,420+i_x,700+i_y);

                ////列印所有資料
                //i_return = B_Print_Out(i_pnum);
                ////关闭打印
                //B_ClosePrn();

                //b_result = true;

                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();

                //s_module = s_module.Substring(1, s_module.Length - 1);
                //---------------------------------------------------------------------------------
                s_module = s_module.Substring(0, 3);
                //---------------------------------------------------------------------------------
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Get_Graphic_ColorBMP(1011 + i_x, 20 + i_y + 20, s_resule + "Schu01.bmp");
                i_return = B_Get_Graphic_ColorBMP(30 + i_x, 107 + i_y, s_resule + "flash-90.bmp");

                s_value = "Schüco International KG";
                i_return = B_Prn_Text_TrueType(980 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A1", s_value);

                s_value = "Photovoltaic module";
                i_return = B_Prn_Text_TrueType(980 + i_x, 500 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A2", s_value);


                s_value = "Karolinenstr.1-15";
                i_return = B_Prn_Text_TrueType(950 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A3", s_value);

                s_value = "33609 Bielefeld";
                i_return = B_Prn_Text_TrueType(920 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A4", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(890 + i_x, 65 + i_y, 32, "Arial", 2, 400, 0, 0, 0, "A5", s_value);
                //--------------------------------------------------------------------------------
                //s_value = "MPE " + s_module + " PS 09";
                s_value = "MPE " + s_module + " PG 09";
                //--------------------------------------------------------------------------------
                i_return = B_Prn_Text_TrueType(940 + i_x, 500 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A6", s_value);

                s_value = "Art.-no.:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(900 + i_x, 500 + i_y, 38, "Arial", 2, 500, 0, 0, 0, "A7", s_value);

                s_value = "P    :";
                i_return = B_Prn_Text_TrueType_W(840 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A8", s_value);
                i_return = B_Prn_Text_TrueType(836 + i_x, 185 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A81", "MPP");
                s_value = dc_pm.ToString("#,##0");
                i_return = B_Prn_Text_TrueType_W(820 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A82", s_value);
                s_value = "Wp";
                i_return = B_Prn_Text_TrueType_W(825 + i_x, 714 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A83", s_value);

                //s_value = "(Tolerances P     : -0/+5%)";
                s_value = "(Tolerances P     : -0/+5Wp)";
                i_return = B_Prn_Text_TrueType_W(785 + i_x, 160 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A9", s_value);
                i_return = B_Prn_Text_TrueType(783 + i_x, 390 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A91", "MPP");

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(735 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A10", s_value);
                i_return = B_Prn_Text_TrueType(731 + i_x, 189 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A101", "MPP");
                s_value = dc_vpm.ToString("#,##00.00");
                i_return = B_Prn_Text_TrueType_W(735 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A102", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(740 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A103", s_value);

                s_value = "I     :";
                i_return = B_Prn_Text_TrueType_W(685 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A11", s_value);
                i_return = B_Prn_Text_TrueType(681 + i_x, 182 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A111", "MPP");
                //s_value = dc_ipm.ToString("#,##0.00");
                s_value = dc_voc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(685 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A112", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(690 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A113", s_value);

                s_value = "U    :";
                i_return = B_Prn_Text_TrueType_W(635 + i_x, 165 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A12", s_value);
                i_return = B_Prn_Text_TrueType(631 + i_x, 188 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A121", "OC");
                //s_value = dc_voc.ToString("#,##00.00");
                s_value = dc_ipm.ToString("#,##00.00");
                i_return = B_Prn_Text_TrueType_W(635 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A122", s_value);
                s_value = "V";
                i_return = B_Prn_Text_TrueType_W(640 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A123", s_value);

                s_value = "I    :";
                i_return = B_Prn_Text_TrueType_W(585 + i_x, 170 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A13", s_value);
                i_return = B_Prn_Text_TrueType(581 + i_x, 180 + i_y, 25, "Arial", 2, 400, 0, 0, 0, "A131", "SC");
                s_value = dc_isc.ToString("#,##0.00");
                i_return = B_Prn_Text_TrueType_W(585 + i_x, 619 + i_y, 48, 17, "Arial", 2, 400, 0, 0, 0, "A132", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(590 + i_x, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A133", s_value);

                //s_value="Fusing Rating:";
                s_value = "Fuse Rating:";
                i_return = B_Prn_Text_TrueType_W(527 + i_x + 10, 170 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A14", s_value);
                s_value = "15";
                i_return = B_Prn_Text_TrueType_W(527 + i_x + 10, 619 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A141", s_value);
                i_return = B_Prn_Text_TrueType_W(527 + i_x + 10, 715 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A142", "A");
                s_value = "Application Class:";
                i_return = B_Prn_Text_TrueType_W(490 + i_x, 170 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A143", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType_W(490 + i_x, 628 + i_y, 45, 17, "Arial", 2, 400, 0, 0, 0, "A144", s_value);

                s_value = "Data measured at STC:";
                i_return = B_Prn_Text_TrueType(464 + i_x, 280 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A15", s_value);
                s_value = "(1000W/m  ; AM 1.5; 25";
                i_return = B_Prn_Text_TrueType(428 + i_x, 260 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A151", s_value);
                i_return = B_Prn_Text_TrueType(439 + i_x, 375 + i_y, 16, "Arial", 2, 500, 0, 0, 0, "A152", "2");
                i_return = B_Prn_Text_TrueType(423 + i_x, 525 + i_y, 32, "Arial", 2, 600, 0, 0, 0, "A153", "℃");
                i_return = B_Prn_Text_TrueType(428 + i_x, 555 + i_y, 28, "Arial", 2, 500, 0, 0, 0, "A154", ")");

                //i_return = B_Get_Graphic_ColorBMP(130 + i_x, 200 + i_y - 20, s_resule + "Schu06.bmp");
                i_return = B_Get_Graphic_ColorBMP(130 + i_x, 200 + i_y - 20, s_resule + "Schutest.bmp");
                //i_return = B_Get_Graphic_ColorBMP(120 + i_x, 530 + i_y - 20, s_resule + "Sch2.bmp"); 
                i_return = B_Get_Graphic_ColorBMP(120 + i_x, 530 + i_y - 20, s_resule + "Sch3.bmp");

                s_value = "Produced in accordance";
                i_return = B_Prn_Text_TrueType(370 + i_x, 214 + i_y, 37, "Arial", 2, 460, 0, 0, 0, "A16", s_value);

                s_value = "with IEC 61215";
                //---------------------------------------------------------------------------------------------------
                //i_return = B_Prn_Text_TrueType(330 + i_x, 214 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);
                i_return = B_Prn_Text_TrueType(330 + i_x, 214 + i_y + 70, 37, "Arial", 2, 500, 0, 0, 0, "A161", s_value);
                //---------------------------------------------------------------------------------------------------
                s_value = "Max.System Voltage 1000V";
                i_return = B_Prn_Text_TrueType(290 + i_x, 214 + i_y, 37, "Arial", 2, 500, 0, 0, 0, "A17", s_value);
                s_value = "Warning - Electrical Hazard:";
                i_return = B_Prn_Text_TrueType(80 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 1, 0, "A19", s_value);
                s_value = "Module generates DC current when";
                i_return = B_Prn_Text_TrueType(50 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A191", s_value);
                s_value = "exposed to sunlight or other light sources";
                i_return = B_Prn_Text_TrueType(20 + i_x, 210 + i_y, 28, "Arial", 2, 400, 0, 0, 0, "A192", s_value);

                i_return = B_Draw_Box(119 + i_x, 130 + i_y, 3, 420 + i_x, 700 + i_y);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //SH-Conergy-铭牌CSA(600)
        public static bool wf_printlable28(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "600V(UL)";
                i_return = B_Prn_Text_TrueType(703 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "UL 1703";
                i_return = B_Prn_Text_TrueType_W(720 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);
                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConCSA.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Factory ID:S"; ;
                i_return = B_Prn_Text_TrueType(520 + i_x, 1260 + i_y, 52, "Basemic Times", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A215", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable29(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_module = s_module.Trim().Substring(0, 3) + "M";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);

                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);
                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";

                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);
                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);

                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);

                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);
                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1180 + i_y, s_resule + "ConCE-E.bmp");
                i_return = B_Get_Graphic_ColorBMP(170 + i_x, 1175 + i_y, s_resule + "ConTUVe2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,"; ;
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable30(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1180 + i_y, s_resule + "ConCE-E.bmp");
                i_return = B_Get_Graphic_ColorBMP(170 + i_x, 1175 + i_y, s_resule + "ConTUV-E.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable31(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_module = s_module.Trim().Substring(0, 3) + "M";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1180 + i_y, s_resule + "ConCE-E.bmp");
                i_return = B_Get_Graphic_ColorBMP(170 + i_x, 1175 + i_y, s_resule + "ConTUVe2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable32(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum, l_len;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());

                i_pnum = int.Parse(s_array[6].ToString());
                s_modulecode = Convert.ToString(s_array[7]);
                s_module = Convert.ToString(s_array[8]);
                s_pktypename = Convert.ToString(s_array[22]);
                if (i_darkness <= 0)
                {
                    i_darkness = 15;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(3 + i_x, 50 + i_y, 26, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(3 + i_x, 77 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 5 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(300 + i_x, 5 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 31 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(300 + i_x, 31 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 59 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A41", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(300 + i_x, 59 + i_y, 22, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(255 + i_x, 85 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(300 + i_x, 85 + i_y, 22, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable321(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, l_len, i_darkness, i_x, i_y;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(data.CoefPM.ToString());
                dc_isc = decimal.Parse(data.CoefISC.ToString().Trim());
                dc_ipm = decimal.Parse(data.CoefIPM.ToString().Trim());
                dc_voc = decimal.Parse(data.CoefVOC.ToString().Trim());
                dc_vpm = decimal.Parse(data.CoefVPM.ToString().Trim());

                i_pnum = int.Parse(data.PrintQty.ToString().Trim());
                s_modulecode = Convert.ToString(data.PowersetModuleCode);
                s_module = Convert.ToString(data.PowersetModuleName);
                s_pktypename = Convert.ToString(data.PowersetSubPowerLevel ?? string.Empty);

                i_darkness = data.Darkness;

                i_x = data.X;
                i_y = data.Y;

                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 2, 4, 64, Convert.ToChar(78), data.LotNo);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A0", data.LotNo);

                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                }
                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 8 + i_y, 35, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 8 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 92 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A91", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 92 + i_y, 33, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable32_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum, l_len;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_pktypename = s_array[22].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 3, 4, 64, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);
                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 8 + i_y, 35, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 8 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 92 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A91", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 92 + i_y, 33, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 根据临时需求因增加条码字段长度新增打印模版 add by yongbing.yang 20130827
        /// </summary>
        /// <param name="s_barcode">条码序列</param>
        /// <param name="i_darkness">打印浓度</param>
        /// <param name="i_x">起始位置</param>
        /// <param name="i_y">起始位置</param>
        /// <param name="s_array"></param>
        /// <returns>是否成功</returns>
        public static bool wf_printlable321_300(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, l_len, i_darkness, i_x, i_y;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(data.CoefPM.ToString());
                dc_isc = decimal.Parse(data.CoefISC.ToString().Trim());
                dc_ipm = decimal.Parse(data.CoefIPM.ToString().Trim());
                dc_voc = decimal.Parse(data.CoefVOC.ToString().Trim());
                dc_vpm = decimal.Parse(data.CoefVPM.ToString().Trim());

                i_pnum = int.Parse(data.PrintQty.ToString().Trim());
                s_modulecode = Convert.ToString(data.PowersetModuleCode);
                s_module = Convert.ToString(data.PowersetModuleName);
                s_pktypename = Convert.ToString(data.PowersetSubPowerLevel ?? string.Empty);

                i_darkness = data.Darkness;

                i_x = data.X;
                i_y = data.Y;

                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 2, 4, 64, Convert.ToChar(78), data.LotNo);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 36, "Arial", 1, 500, 0, 0, 0, "A0", data.LotNo);

                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp";
                }
                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 8 + i_y, 35, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 8 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 92 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A91", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 92 + i_y, 33, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20150309新增不体现五大性能参数
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable322(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());

                i_pnum = int.Parse(s_array[6].ToString());
                s_modulecode = Convert.ToString(s_array[7]);
                s_module = Convert.ToString(s_array[8]);
                s_pktypename = Convert.ToString(s_array[22]);
                if (i_darkness <= 0)
                {
                    i_darkness = 15;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(3 + i_x, 50 + i_y, 26, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);

                    //s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(3 + i_x, 77 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20150309新增不体现五大性能参数300dpi
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable322_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_pktypename = s_array[22].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 3, 4, 64, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);
                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);

                    //s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20160615陕西国力同不体现五大性能参数，条码宽度缩短
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable3221(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());

                i_pnum = int.Parse(s_array[6].ToString());
                s_modulecode = Convert.ToString(s_array[7]);
                s_module = Convert.ToString(s_array[8]);
                s_pktypename = Convert.ToString(s_array[22]);
                if (i_darkness <= 0)
                {
                    i_darkness = 15;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(3 + i_x, 50 + i_y, 26, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);

                    //s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(3 + i_x, 77 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20150309新增不体现五大性能参数300dpi
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable3221_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_pktypename = s_array[22].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 2, 4, 64, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);
                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);

                    //s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                }
                else
                {
                    s_value = "Nominal Power=" + dc_pm.ToString("#,##0.0") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }

        /// <summary>
        /// 20150608新增不体现五大性能参数且功率为实测功率
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable323(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(s_barcode);
                if (!string.IsNullOrEmpty(_testDataEntity.ErrorMsg) || ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }
                DataRow drLotInfo = ds.Tables[0].Rows[0];
                decimal coff = decimal.Parse(drLotInfo["COEF_PMAX"].ToString());
                i_pnum = int.Parse(s_array[6].ToString());
                s_modulecode = Convert.ToString(s_array[7]);
                s_module = Convert.ToString(s_array[8]);
                s_pktypename = Convert.ToString(s_array[22]);
                if (i_darkness <= 0)
                {
                    i_darkness = 15;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(3 + i_x, 8 + i_y, 0, "1", 2, 1, 40, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(3 + i_x, 50 + i_y, 26, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    s_value = "Pm=" + coff.ToString("#,##0.00") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);
                }
                else
                {
                    s_value = "Pm=" + coff.ToString("#,##0.00") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(3 + i_x, 77 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20150608新增不体现五大性能参数且功率为实测功率300dpi
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable323_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(s_barcode);
                if (!string.IsNullOrEmpty(_testDataEntity.ErrorMsg) || ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }
                DataRow drLotInfo = ds.Tables[0].Rows[0];
                decimal coff = decimal.Parse(drLotInfo["COEF_PMAX"].ToString());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_pktypename = s_array[22].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 3, 4, 64, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);
                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    s_value = "Pm=" + coff.ToString("#,##0.00") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);
                }
                else
                {
                    s_value = "Pm=" + coff.ToString("#,##0.00") + "Wp";
                }

                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 38, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20150608新增不体现五大性能参数且功率为实测功率
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable324(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(s_barcode);
                if (!string.IsNullOrEmpty(_testDataEntity.ErrorMsg) || ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }
                DataRow drLotInfo = ds.Tables[0].Rows[0];
                decimal coff = decimal.Parse(drLotInfo["COEF_PMAX"].ToString());
                i_pnum = int.Parse(s_array[6].ToString());
                s_modulecode = Convert.ToString(s_array[7]);
                s_module = Convert.ToString(s_array[8]);
                s_pktypename = Convert.ToString(s_array[22]);
                if (i_darkness <= 0)
                {
                    i_darkness = 15;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 15 + i_y, 0, "1", 3, 4, 66, Convert.ToChar(78), s_barcode);
                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    i_return = B_Prn_Text_TrueType(15 + i_x, 87 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode + "-" + Convert.ToString(strSplit[strSplit.Length - 1]));
                }
                else
                {
                    i_return = B_Prn_Text_TrueType(15 + i_x, 87 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode + "-H");
                }
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// 20150608新增不体现五大性能参数且功率为实测功率300dpi
        /// </summary>
        /// <param name="s_barcode"></param>
        /// <param name="i_darkness"></param>
        /// <param name="i_x"></param>
        /// <param name="i_y"></param>
        /// <param name="s_array"></param>
        /// <returns></returns>
        public static bool wf_printlable324_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(s_barcode);
                if (!string.IsNullOrEmpty(_testDataEntity.ErrorMsg) || ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }
                DataRow drLotInfo = ds.Tables[0].Rows[0];
                decimal coff = decimal.Parse(drLotInfo["COEF_PMAX"].ToString());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_pktypename = s_array[22].ToString().Trim();
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 15 + i_y, 0, "1", 3, 4,66, Convert.ToChar(78), s_barcode);
                if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
                {
                    string[] strSplit = s_pktypename.Split('-');

                    i_return = B_Prn_Text_TrueType(15 + i_x, 87 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode +"-" + Convert.ToString(strSplit[strSplit.Length - 1]));
                }
                else
                {
                    i_return = B_Prn_Text_TrueType(15 + i_x, 87 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode + "-H");
                }
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable33(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                //s_value = dc_ipm.ToString("#,##0.00") + "A";
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                //s_value = dc_voc.ToString("#,##0.00") + "V";
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1180 + i_y, s_resule + "ConCE-E.bmp");
                i_return = B_Get_Graphic_ColorBMP(170 + i_x, 1165 + i_y, s_resule + "ConE.bmp");

                s_value = "Certificate Number MCS";
                i_return = B_Prn_Text_TrueType(170 + i_x, 1310 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A215", s_value);
                s_value = "PV0074 Photovoltaic Systems";
                i_return = B_Prn_Text_TrueType(170 + i_x, 1340 + i_y, 30, "Arial", 1, 400, 0, 0, 0, "A216", s_value);

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable34(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;
            string s_modulecode, s_module, s_ptolerance, s_productmodel, s_modeltype;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_productmodel = s_array[23].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);

                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);

                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);

                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);

                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);

                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);

                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);

                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);

                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);

                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);

                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);

                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);

                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);

                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);

                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);

                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);

                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);

                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                //i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV.bmp");

                s_modeltype = s_module.Substring(4, 1);
                if (s_modeltype == "P" && s_productmodel == "6610P")
                {
                    i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV1.bmp");
                }
                else if (s_modeltype == "M" && s_productmodel == "5612M")
                {
                    i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV0.bmp");
                }
                else
                { }

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable35(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;
            string s_modulecode, s_module, s_ptolerance, s_productmodel, s_modeltype;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_productmodel = s_array[23].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");

                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);

                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);

                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);

                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);

                s_value = dc_ipm.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);

                s_value = dc_voc.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);

                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);

                s_value = "600V(UL)";
                i_return = B_Prn_Text_TrueType(703 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);

                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);

                s_value = "UL 1703";
                i_return = B_Prn_Text_TrueType_W(720 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);

                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);

                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);

                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);

                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);

                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);

                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);

                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);

                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                //i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConUL.bmp");

                s_modeltype = s_module.Substring(4, 1);
                if (s_modeltype == "P" && s_productmodel == "6610P")
                {
                    i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV1.bmp");
                }
                else if (s_modeltype == "M" && s_productmodel == "5612M")
                {
                    i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV0.bmp");
                }
                else
                { }

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                //s_value = "Factory ID:S";
                //i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 52, "Basemic Times", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Factory ID:A";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1260 + i_y, 52, "Basemic Times", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A215", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable36(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day;
            string s_modulecode, s_module, s_ptolerance, s_productmodel, s_modeltype;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_productmodel = s_array[23].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(IEC)";
                i_return = B_Prn_Text_TrueType(650 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "IEC 61215 Ed.2,IEC 61730";
                i_return = B_Prn_Text_TrueType_W(480 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1180 + i_y, s_resule + "ConCE-E.bmp");
                //i_return = B_Get_Graphic_ColorBMP(170 + i_x, 1175 + i_y, s_resule + "ConTUV-E.bmp");

                s_modeltype = s_module.Substring(4, 1);
                if (s_modeltype == "P" && s_productmodel == "6610P")
                {
                    i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV1.bmp");
                }
                else if (s_modeltype == "M" && s_productmodel == "5612M")
                {
                    i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConTUV0.bmp");
                }
                else
                { }

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //HZ-Conergy-铭牌CSA(1000)
        public static bool wf_printlable37(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);


                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(CSA)";
                i_return = B_Prn_Text_TrueType(703 + i_x - 25, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "UL 1703";
                i_return = B_Prn_Text_TrueType_W(720 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);
                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConCSA.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Factory ID:A"; ;
                i_return = B_Prn_Text_TrueType(520 + i_x, 1260 + i_y, 52, "Basemic Times", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A215", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //SH-Conergy-铭牌CSA(1000)
        public static bool wf_printlable38(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 10 + i_y, 58, "Arial", 1, 600, 0, 0, 0, "A1", s_value);

                s_value = "S/N:" + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 72 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

                s_value = "P/N:" + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 118 + i_y, 50, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(580 + i_x, 15 + i_y, s_resule + "ConLogo.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 200 + i_y, s_resule + "ConLine.bmp");

                s_value = "Maximum Power(Pmax)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A4", s_value);
                s_value = dc_pm.ToString("#,##0") + "W";
                i_return = B_Prn_Text_TrueType(740 + i_x, 275 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A41", s_value);

                s_value = "Power Tolerance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                //i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 700, 0, 0, 0, "A51", s_value);
                i_return = B_Prn_Text_TrueType(735 + i_x, 323 + i_y, 42, "Arial", 1, 800, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(Vmpp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A6", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 366 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(Impp)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A7", s_value);
                s_value = dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(Voc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A8", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 452 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(Isc)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A9", s_value);
                s_value = dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 495 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A10", s_value);
                s_value = "1000V(CSA)";
                i_return = B_Prn_Text_TrueType(703 + i_x - 25, 538 + i_y, 40, "Arial", 1, 700, 0, 0, 0, "A101", s_value);
                s_value = "Certified to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 600 + i_y, 46, "Arial", 1, 700, 0, 0, 0, "A102", s_value);
                s_value = "UL 1703";
                i_return = B_Prn_Text_TrueType_W(720 + i_x, 605 + i_y, 43, 15, "Arial", 1, 700, 0, 0, 0, "A103", s_value);
                s_value = "Electrical data at standard test condition";
                i_return = B_Prn_Text_TrueType(10 + i_x, 665 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A104", s_value);
                s_value = "STC:irradiance of 1000W/";
                i_return = B_Prn_Text_TrueType(10 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A105", s_value);
                i_return = B_Prn_Text_TrueType(326 + i_x, 700 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A106", "㎡");
                i_return = B_Prn_Text_TrueType(357 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A107", ", cell temperature 25");
                i_return = B_Prn_Text_TrueType(605 + i_x, 705 + i_y, 36, "Arial", 1, 600, 0, 0, 0, "A108", "℃");
                i_return = B_Prn_Text_TrueType(640 + i_x, 705 + i_y, 32, "Arial", 1, 400, 0, 0, 0, "A109", ", AM1.5");

                s_value = "Fire resistance rating Class C";
                i_return = B_Prn_Text_TrueType(150 + i_x, 754 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A201", s_value);
                s_value = "Series Fuse " + dc_fuse.ToString("####") + "A";
                i_return = B_Prn_Text_TrueType(150 + i_x, 786 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A202", s_value);
                s_value = "For field connection,use min.No.12 wires suitable";
                i_return = B_Prn_Text_TrueType(150 + i_x, 818 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A203", s_value);
                s_value = "for a minimum of 90℃.Use copper wire only.";
                i_return = B_Prn_Text_TrueType(150 + i_x, 850 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 895 + i_y, 48, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "Refer to the user Manual before";
                i_return = B_Prn_Text_TrueType(150 + i_x, 935 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "installing, operating or servicing this unit";
                i_return = B_Prn_Text_TrueType(150 + i_x, 968 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1010 + i_y, 50, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DO NOT connect or disconnect plug contacts while";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1058 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "system is under load current.Failure to comply can";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1088 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "result in a hazardous situation!";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1118 + i_y, 32, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Made In China";
                i_return = B_Prn_Text_TrueType(520 + i_x, 1320 + i_y, 52, "Arial", 1, 700, 0, 0, 0, "A212", s_value);

                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 754 + i_y, s_resule + "Con1.bmp");
                i_return = B_Get_Graphic_ColorBMP(5 + i_x, 1180 + i_y, s_resule + "ConCE.bmp");
                i_return = B_Get_Graphic_ColorBMP(160 + i_x, 1158 + i_y, s_resule + "ConCSA.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(20 + i_x, 1380 + i_y, 0, "1E", 2, 2, 150, Convert.ToChar(78), s_printcode);

                s_value = "Factory ID:S"; ;
                i_return = B_Prn_Text_TrueType(520 + i_x, 1260 + i_y, 52, "Basemic Times", 1, 700, 0, 0, 0, "A213", s_value);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1540 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "Germany";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1590 + i_y, 38, "Arial", 1, 700, 0, 0, 0, "A215", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable39(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum, i_arrlen;
            string s_value, s_printcode, s_date, s_year, s_month, s_day, sDetailPL, sDetailPL1, sDetailPL2;
            DateTime d_date;
            sDetailPL1 = "";
            sDetailPL2 = "";

            try
            {
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                sDetailPL = s_array[22].ToString().Trim();
                string[] sarr = sDetailPL.Split('#');
                i_arrlen = sarr.Length;
                switch (i_arrlen.ToString("0#"))
                {
                    case "01":
                        sDetailPL1 = sarr[0].ToString().Trim();
                        break;
                    case "02":
                        sDetailPL1 = sarr[0].ToString().Trim();
                        sDetailPL2 = sarr[1].ToString().Trim();
                        break;
                    default:
                        break;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date;
                i_return = B_Prn_Barcode(130 + i_x, 50 + i_y, 0, "1E", 2, 10, 140, Convert.ToChar(78), s_printcode);

                s_value = "S/N: " + s_barcode;
                i_return = B_Prn_Text_TrueType(130 + i_x + 80, 230 + i_y - 40, 60, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                s_value = sDetailPL1;
                i_return = B_Prn_Text_TrueType(130 + i_x + 50, 310 + i_y - 70, 75, "Arial", 1, 500, 0, 0, 0, "A2", s_value);

                s_value = sDetailPL2;
                i_return = B_Prn_Text_TrueType(130 + i_x + 40, 390 + i_y - 85, 75, "Arial", 1, 500, 0, 0, 0, "A3", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //-------------------------------------------------------------------------------------------------
        public static bool wf_printlable320_300(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum, l_len;
            string s_resule, s_value, s_modulecode, s_module, s_pktypename;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm;

            try
            {
                dc_pm = decimal.Parse(s_array[1].ToString().Trim());
                dc_isc = decimal.Parse(s_array[2].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[3].ToString().Trim());
                dc_voc = decimal.Parse(s_array[4].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[5].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                s_pktypename = s_array[22].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                i_return = B_Prn_Barcode(15 + i_x, 11 + i_y, 0, "1", 2, 10, 64, Convert.ToChar(78), s_barcode);
                i_return = B_Prn_Text_TrueType(15 + i_x, 81 + i_y, 40, "Arial", 1, 500, 0, 0, 0, "A0", s_barcode);

                s_value = "Pm=" + dc_pm.ToString("#,##0.00") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
                i_return = B_Prn_Text_TrueType(15 + i_x, 123 + i_y, 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 8 + i_y, 35, "Arial", 1, 500, 0, 0, 0, "A21", "Voc");
                s_value = dc_voc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 8 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A22", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A31", " I sc");
                s_value = dc_isc.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 49 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A32", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 92 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A91", "Vmp");
                s_value = dc_vpm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "V";
                i_return = B_Prn_Text_TrueType(460 + i_x, 92 + i_y, 33, "Arial", 1, 400, 0, 0, 0, "A42", s_value);

                i_return = B_Prn_Text_TrueType(392 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A51", " Imp");
                s_value = dc_ipm.ToString("#,##0.00");
                l_len = s_value.Length;
                if (l_len < 5)
                {
                    s_value = s_value.PadRight((5 - l_len) * 2, ' ');
                }
                s_value = "=" + s_value + "A";
                i_return = B_Prn_Text_TrueType(460 + i_x, 133 + i_y, 33, "Arial", 1, 500, 0, 0, 0, "A52", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by chao.pang 20130624 HZ-CSA
        public static bool wf_printlable50(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                #region
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(343 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(294 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //s_value = "Maximum Power(P";
                //i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                ////s_value = "MPP";
                ////i_return = B_Prn_Text_TrueType(294 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                ////s_value = ")";
                ////i_return = B_Prn_Text_TrueType(343 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                //s_value = dc_pm.ToString("#,##0") + " Wp";
                //i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                //s_value = "Power Sorting";
                //i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                //s_value = s_ptolerance;
                //i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(471 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(421 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = "Maximum Power Voltage(V";
                //i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                ////s_value = "MPP";
                ////i_return = B_Prn_Text_TrueType(421 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                ////s_value = ")";
                ////i_return = B_Prn_Text_TrueType(471 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                //s_value = dc_vpm.ToString("#,##0.00") + " V";
                //i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(463 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(413 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = "Maximum Power Current(I";
                //i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                ////s_value = "MPP";
                ////i_return = B_Prn_Text_TrueType(413 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                ////s_value = ")";
                ////i_return = B_Prn_Text_TrueType(463 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                //s_value = dc_voc.ToString("#,##0.00") + " A";
                //i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(400 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = "Open Circuit Voltage(V";
                //i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                ////s_value = "OC";
                ////i_return = B_Prn_Text_TrueType(362 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                ////s_value = ")";
                ////i_return = B_Prn_Text_TrueType(397 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                //s_value = dc_ipm.ToString("#,##0.00") + " V";
                //i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(393 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = "Short Circuit Current(I";
                ////i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                ////s_value = "SC";
                ////i_return = B_Prn_Text_TrueType(353 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                ////s_value = ")";
                ////i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                //s_value = dc_isc.ToString("#,##0.00") + " A";
                //i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);
                //--------
                #endregion
                s_value = ")";
                i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power(P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = dc_pm.ToString("#,##0") + " Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(425 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = dc_voc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = dc_isc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = "1000 V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(795 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = "A";
                i_return = B_Prn_Text_TrueType(822 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions(STC:irradiance 1000W/㎡,cell temperature 25℃,AM 1.5)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A106", s_value);
                s_value = "Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 587 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A109", s_value);

                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ATTENTION!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "Se reporter aux manuels d'installation et d'utilisation avant";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "toute installation , utilisation  ou mise  en service. Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(800 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "connecter  ou   déconnecter   les   connecteurs  lorsque";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "le";
                i_return = B_Prn_Text_TrueType(823 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "système  est en charge. Ne pas  respecter ces";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "instructions";
                i_return = B_Prn_Text_TrueType(700 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "expose à de graves dangers!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:A";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");                

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(15 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,Germany";
                i_return = B_Prn_Text_TrueType(37 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20131024 对原模版50 按照客户需求对模版进行修改
        public static bool wf_printlable501(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(801 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(781 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = "A";
                i_return = B_Prn_Text_TrueType(822 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为20 
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小为20 高度为587

                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ATTENTION!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "Se reporter aux manuels d'installation et d'utilisation avant";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "toute installation , utilisation  ou mise  en service. Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(800 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "connecter  ou   déconnecter   les   connecteurs  lorsque";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "le";
                i_return = B_Prn_Text_TrueType(823 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "système  est en charge. Ne pas  respecter ces";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "instructions";
                i_return = B_Prn_Text_TrueType(700 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "expose à de graves dangers!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:A";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");                

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(15 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Inc., 2460 W 26  Ave Denver, CO 80211 USA";
                i_return = B_Prn_Text_TrueType(70 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                s_value = "th";
                i_return = B_Prn_Text_TrueType(395 + i_x, 1360 + i_y, 20, "Arial", 1, 700, 0, 0, 0, "A221", s_value);

                //原底行信息          
                //s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,Germany";
                //i_return = B_Prn_Text_TrueType(37 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20140408 对原模版501 按照客户需求对内容文本对语言进行修改
        public static bool wf_printlable502(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------

                //s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(801 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(781 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = "A";
                i_return = B_Prn_Text_TrueType(822 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为20 
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小为20 高度为587

                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "AVERTISSEMENT!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "Reportez-vous   au   manuel   d'installation   et";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "d'utilisation";
                i_return = B_Prn_Text_TrueType(704 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2101", s_value);

                s_value = "avant d'installer, utiliser  ou réparer  cet appareil.   Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(798 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2111", s_value);

                s_value = "brancher  ou  débrancher  les  contacts  enfichables";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "tant";
                i_return = B_Prn_Text_TrueType(795 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2121", s_value);

                s_value = "que système est sous courant de charge.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Le Non-respect";
                i_return = B_Prn_Text_TrueType(658 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2131", s_value);

                s_value = "peut vous ramener vers une situation dangereuse!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:A";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");                

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(15 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Inc., 2460 W 26  Ave Denver, CO 80211 USA";
                i_return = B_Prn_Text_TrueType(70 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                s_value = "th";
                i_return = B_Prn_Text_TrueType(395 + i_x, 1360 + i_y, 20, "Arial", 1, 700, 0, 0, 0, "A221", s_value);

                //原底行信息          
                //s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,Germany";
                //i_return = B_Prn_Text_TrueType(37 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20150107 对原模版502 按照客户需求把铭牌中“Fire Resistance Rating: Class C”改为“Module Fire Performance: Type 1”
        public static bool wf_printlable503(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);

                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(801 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(781 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = " A";
                i_return = B_Prn_Text_TrueType(812 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);
                //s_value = Convert.ToInt32(dc_fuse) + " A";
                //i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Module Fire Performance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "  Type 1";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value); //原来字体大小20 
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小20  高度592


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);


                s_value = "AVERTISSEMENT!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "Reportez-vous   au   manuel   d'installation   et";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "d'utilisation";
                i_return = B_Prn_Text_TrueType(704 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2101", s_value);

                s_value = "avant d'installer, utiliser  ou réparer  cet appareil.   Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(798 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2111", s_value);

                s_value = "brancher  ou  débrancher  les  contacts  enfichables";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "tant";
                i_return = B_Prn_Text_TrueType(795 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2121", s_value);

                s_value = "que système est sous courant de charge.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Le Non-respect";
                i_return = B_Prn_Text_TrueType(658 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2131", s_value);

                s_value = "peut vous ramener vers une situation dangereuse!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:A";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(5 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Inc., 2460 W 26  Ave Denver, CO 80211 USA";
                i_return = B_Prn_Text_TrueType(70 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                s_value = "th";
                i_return = B_Prn_Text_TrueType(395 + i_x, 1360 + i_y, 20, "Arial", 1, 700, 0, 0, 0, "A221", s_value);

                //s_value = "Conergy AG, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                //i_return = B_Prn_Text_TrueType(5 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }

        //add by chao.pang 20130624 TUV欧洲铭牌
        public static bool wf_printlable51(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(360 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power(P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(305 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = dc_pm.ToString("#,##0") + " Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(437 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(487 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(425 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = dc_voc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(405 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(397 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = dc_isc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = "1000 V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215,IEC61730";
                i_return = B_Prn_Text_TrueType_W(520 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions(STC:irradiance 1000W/㎡,cell temperature 25℃,AM 1.5)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A106", s_value);
                s_value = "Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 588 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A109", s_value);

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(5 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,Germany";
                i_return = B_Prn_Text_TrueType(37 + i_x, 1370 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20131024 对原模版51 按照客户需求对模版进行修改
        public static bool wf_printlable511(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(365 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(305 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(437 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(487 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(425 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(405 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(397 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(510 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为 20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value); //原来字体大小为 20 高度为 588

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(8 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                i_return = B_Prn_Text_TrueType(8 + i_x, 1370 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20131205 对原模版511 按照客户需求对模版进行垃圾桶的新增
        public static bool wf_printlable512(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(365 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(305 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(437 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(487 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(425 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(405 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(397 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(510 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为 20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value); //原来字体大小为 20 高度为 588

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");
                i_return = B_Get_Graphic_ColorBMP(400 + i_x, 1055 + i_y, s_resule + "Recycle.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(8 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                i_return = B_Prn_Text_TrueType(8 + i_x, 1370 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20140113 对原模版512 按照客户需求对名牌地址进行变更
        public static bool wf_printlable513(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(365 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(305 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(437 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(487 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(425 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(405 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(397 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(510 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为 20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value); //原来字体大小为 20 高度为 588

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");
                i_return = B_Get_Graphic_ColorBMP(400 + i_x, 1055 + i_y, s_resule + "Recycle.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(8 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Deutschland GmbH, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                i_return = B_Prn_Text_TrueType(8 + i_x, 1370 + i_y, 28, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20150109 对原模版513 按照客户需求修改 MCS logo，并在logo下添加“Certificate Number MCS PVXXXX Photovoltaic Systems
        public static bool wf_printlable514(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(365 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(305 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(437 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(487 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(425 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(405 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(397 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(510 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为 20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value); //原来字体大小为 20 高度为 588

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(410 + i_x, 1070 + i_y, s_resule + "Recycle.bmp");


                //调整MSClogo 位置和内容
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1035 + i_y, s_resule + "ConE3.bmp");
                s_value = "Certificate Number MCS";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1170 + i_y, 25, "Arial", 1, 400, 0, 0, 0, "A2151", s_value);
                s_value = "PVXXXX Photovoltaic";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1200 + i_y, 25, "Arial", 1, 400, 0, 0, 0, "A2152", s_value);
                s_value = "Systems";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1230 + i_y, 25, "Arial", 1, 400, 0, 0, 0, "A2153", s_value);

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(8 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Deutschland GmbH, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                i_return = B_Prn_Text_TrueType(8 + i_x, 1370 + i_y, 28, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20150109 对原模版513 按照客户需求修改 MCS logo，并在logo下添加“Certificate Number MCS PVXXXX Photovoltaic Systems
        public static bool wf_printlable515(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(365 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(305 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(437 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(487 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(425 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(370 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(405 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(362 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(397 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(510 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为 20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value); //原来字体大小为 20 高度为 588

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(410 + i_x, 1070 + i_y, s_resule + "Recycle.bmp");


                //调整MSClogo 位置和内容
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1125 + i_y, s_resule + "ConE3.bmp");
                s_value = "Certificate Number MCS";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1035 + i_y, 25, "Arial", 1, 400, 0, 0, 0, "A2151", s_value);
                s_value = "PVXXXX Photovoltaic";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1065 + i_y, 25, "Arial", 1, 400, 0, 0, 0, "A2152", s_value);
                s_value = "Systems";
                i_return = B_Prn_Text_TrueType(150 + i_x, 1095 + i_y, 25, "Arial", 1, 400, 0, 0, 0, "A2153", s_value);

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(8 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Deutschland GmbH, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                i_return = B_Prn_Text_TrueType(8 + i_x, 1370 + i_y, 28, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by chao.pang 20150413 对原模版512 按照客户需求删除 MCS logo，将垃圾桶标记左移对齐,修改最下面地址为Kaufmannshaus, Bleichenbrücke 10,D-20354 Hamburg
        public static bool wf_printlable516(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(663 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(365 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(310 + i_x, 205 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A4", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 190 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 230 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(492 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(442 + i_x, 285 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(723 + i_x, 270 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(480 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(430 + i_x, 325 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(740 + i_x, 310 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A71", s_value);


                s_value = ")";
                i_return = B_Prn_Text_TrueType(416 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(377 + i_x, 365 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(720 + i_x, 350 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(402 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 405 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(742 + i_x, 390 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(732 + i_x, 430 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(820 + i_x, 470 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 510 + i_y, 39, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(510 + i_x, 510 + i_y, 39, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为 20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value); //原来字体大小为 20 高度为 588

                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(712 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb213.bmp");
                i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1070 + i_y, s_resule + "Recycle.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode( i_x - 13, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                s_value = "Conergy Deutschland GmbH, Kaufmannshaus, Bleichenbrücke 10,D-20354 Hamburg";
                i_return = B_Prn_Text_TrueType( i_x + 4 , 1370 + i_y, 25, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by chao.pang 20130624 TUV澳洲铭牌
        public static bool wf_printlable52(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //------------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power(P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = dc_pm.ToString("#,##0") + " Wp";
                i_return = B_Prn_Text_TrueType(728 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(740 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(425 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(730 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = dc_voc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(750 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(730 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = dc_isc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(750 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = "1000 V";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(827 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(727 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215,IEC61730";
                i_return = B_Prn_Text_TrueType_W(532 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions(STC:irradiance 1000W/㎡,cell temperature 25℃,AM 1.5)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A106", s_value);
                s_value = "Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 588 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A109", s_value);


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(713 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb214.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                int leng = s_modulecode.Length;

                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;

                if (leng >= 10)
                {
                    i_return = B_Prn_Barcode(5 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                }
                else
                {
                    i_return = B_Prn_Barcode(20 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                }
                s_value = "Conergy Pty Ltd. Unit 1, 2/26 Scouts Crossing Rd, BRENDALE 4500, Australia";
                i_return = B_Prn_Text_TrueType(37 + i_x, 1370 + i_y, 26, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //--------------------------------------------------------------------------------------------------------------
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20131024 对原模版52 按照客户需求对模版进行修改
        public static bool wf_printlable521(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //------------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(827 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(727 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(522 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小为20 高度为588


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(713 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb214.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                int leng = s_modulecode.Length;

                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;

                if (leng >= 10)
                {
                    i_return = B_Prn_Barcode(5 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                }
                else
                {
                    i_return = B_Prn_Barcode(20 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                }
                s_value = "Conergy Pty Ltd. Unit 1, 2/26 Scouts Crossing Rd, BRENDALE 4500, Australia";
                i_return = B_Prn_Text_TrueType(37 + i_x, 1370 + i_y, 26, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //--------------------------------------------------------------------------------------------------------------
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20131206 对原模版52 按照技术部要求把地址字体进行放大
        public static bool wf_printlable522(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //------------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(740 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);
                s_value = "Application Class";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                s_value = "A";
                i_return = B_Prn_Text_TrueType(827 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(727 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "IEC61215, IEC61730";
                i_return = B_Prn_Text_TrueType_W(522 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);
                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value);//原来字体大小为20
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小为20 高度为588


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(713 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ACHTUNG!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A208", s_value);
                s_value = "LEBENSGEFAHR!";
                i_return = B_Prn_Text_TrueType(600 + i_x, 845 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A209", s_value);
                s_value = "Bei    Installation,    Inbetriebnahme und  Wartung   ist";
                i_return = B_Prn_Text_TrueType(145 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "die";
                i_return = B_Prn_Text_TrueType(810 + i_x, 880 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A113", s_value);


                s_value = "Installations- und  Bedienungsanleitung  zu  befolgen.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "Die";
                i_return = B_Prn_Text_TrueType(807 + i_x, 910 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "Steckkontakte   niemals   unter  Laststrom   stecken";
                i_return = B_Prn_Text_TrueType(145 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "oder";
                i_return = B_Prn_Text_TrueType(792 + i_x, 940 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "ziehen.    Bei    Nichtbeachtung   dieser   Warnung";
                i_return = B_Prn_Text_TrueType(145 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "besteht";
                i_return = B_Prn_Text_TrueType(755 + i_x, 970 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "Lebensgefahr!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1000 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1210 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Con13.bmp");
                //i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb13.bmp");
                i_return = B_Get_Graphic_ColorBMP(12 + i_x, 1103 + i_y, s_resule + "Conb214.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "ConE2.bmp");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                int leng = s_modulecode.Length;

                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;

                if (leng >= 10)
                {
                    i_return = B_Prn_Barcode(5 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                }
                else
                {
                    i_return = B_Prn_Barcode(20 + i_x, 1266 + i_y, 0, "1E", 2, 2, 100, Convert.ToChar(78), s_printcode);
                }
                s_value = "Conergy Pty Ltd. Unit 1, 2/26 Scouts Crossing Rd, BRENDALE 4500, Australia";
                i_return = B_Prn_Text_TrueType(20 + i_x, 1370 + i_y, 27, "Arial", 1, 700, 0, 0, 0, "A216", s_value);
                //--------------------------------------------------------------------------------------------------------------
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by chao.pang 20130624 SH-CSA
        public static bool wf_printlable53(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = "Maximum Power(P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(294 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = ")";
                i_return = B_Prn_Text_TrueType(343 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = dc_pm.ToString("#,##0") + " Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = "Maximum Power Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(421 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = ")";
                i_return = B_Prn_Text_TrueType(471 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = dc_vpm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = "Maximum Power Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(413 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = ")";
                i_return = B_Prn_Text_TrueType(463 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = dc_voc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = "Open Circuit Voltage(V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(362 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = ")";
                i_return = B_Prn_Text_TrueType(397 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = dc_ipm.ToString("#,##0.00") + " V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = "Short Circuit Current(I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(353 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = ")";
                i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = dc_isc.ToString("#,##0.00") + " A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = "1000 V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);

                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(795 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = " A";
                i_return = B_Prn_Text_TrueType(812 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);
                //s_value = Convert.ToInt32(dc_fuse) + " A";
                //i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions(STC:irradiance 1000W/㎡,cell temperature 25℃,AM 1.5)";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A106", s_value);
                s_value = "Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 587 + i_y, 20, "Arial", 1, 400, 0, 0, 0, "A109", s_value);


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ATTENTION!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "Se reporter aux manuels d'installation et d'utilisation avant";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "toute installation , utilisation  ou mise  en service. Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(800 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "connecter  ou   déconnecter   les   connecteurs  lorsque";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "le";
                i_return = B_Prn_Text_TrueType(823 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "système  est en charge. Ne pas  respecter ces";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "instructions";
                i_return = B_Prn_Text_TrueType(700 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "expose à de graves dangers!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:S";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(15 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy AG,Anckelmannsplatz 1,20537 Hamburg,Germany";
                i_return = B_Prn_Text_TrueType(37 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20131024 对原模版53 按照客户需求对模版进行修改
        public static bool wf_printlable531(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);

                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(801 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(781 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = " A";
                i_return = B_Prn_Text_TrueType(812 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);
                //s_value = Convert.ToInt32(dc_fuse) + " A";
                //i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value); //原来字体大小20 
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小20  高度592


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "ATTENTION!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);
                s_value = "Se reporter aux manuels d'installation et d'utilisation avant";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);

                s_value = "toute installation , utilisation  ou mise  en service. Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(800 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A114", s_value);

                s_value = "connecter  ou   déconnecter   les   connecteurs  lorsque";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "le";
                i_return = B_Prn_Text_TrueType(823 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A115", s_value);


                s_value = "système  est en charge. Ne pas  respecter ces";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "instructions";
                i_return = B_Prn_Text_TrueType(700 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A116", s_value);

                s_value = "expose à de graves dangers!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:S";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(5 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Inc., 2460 W 26  Ave Denver, CO 80211 USA";
                i_return = B_Prn_Text_TrueType(70 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                s_value = "th";
                i_return = B_Prn_Text_TrueType(395 + i_x, 1360 + i_y, 20, "Arial", 1, 700, 0, 0, 0, "A221", s_value);

                //s_value = "Conergy AG, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                //i_return = B_Prn_Text_TrueType(5 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20140415 对原模版531 按照客户需求对内容文本对语言进行修改
        public static bool wf_printlable532(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);

                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(801 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(781 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = " A";
                i_return = B_Prn_Text_TrueType(812 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);
                //s_value = Convert.ToInt32(dc_fuse) + " A";
                //i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Fire Resistance Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value); //原来字体大小20 
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小20  高度592


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);


                s_value = "AVERTISSEMENT!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "Reportez-vous   au   manuel   d'installation   et";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "d'utilisation";
                i_return = B_Prn_Text_TrueType(704 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2101", s_value);

                s_value = "avant d'installer, utiliser  ou réparer  cet appareil.   Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(798 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2111", s_value);

                s_value = "brancher  ou  débrancher  les  contacts  enfichables";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "tant";
                i_return = B_Prn_Text_TrueType(795 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2121", s_value);

                s_value = "que système est sous courant de charge.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Le Non-respect";
                i_return = B_Prn_Text_TrueType(658 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2131", s_value);

                s_value = "peut vous ramener vers une situation dangereuse!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:S";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(5 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Inc., 2460 W 26  Ave Denver, CO 80211 USA";
                i_return = B_Prn_Text_TrueType(70 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                s_value = "th";
                i_return = B_Prn_Text_TrueType(395 + i_x, 1360 + i_y, 20, "Arial", 1, 700, 0, 0, 0, "A221", s_value);

                //s_value = "Conergy AG, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                //i_return = B_Prn_Text_TrueType(5 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //add by yongbing.yang 20150107 对原模版532 按照客户需求把铭牌中“Fire Resistance Rating: Class C”改为“Module Fire Performance: Type 1”
        public static bool wf_printlable533(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_resule, s_value, s_printcode, s_date, s_year, s_month, s_day, s_modulecode, s_module, s_ptolerance;
            decimal dc_pm, dc_isc, dc_ipm, dc_voc, dc_vpm, dc_fuse;
            DateTime d_date;

            try
            {
                dc_pm = decimal.Parse(s_array[9].ToString().Trim());
                dc_isc = decimal.Parse(s_array[10].ToString().Trim());
                dc_ipm = decimal.Parse(s_array[11].ToString().Trim());
                dc_voc = decimal.Parse(s_array[12].ToString().Trim());
                dc_vpm = decimal.Parse(s_array[13].ToString().Trim());
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                s_modulecode = s_array[7].ToString().Trim();
                s_module = s_array[8].ToString().Trim();
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                dc_fuse = decimal.Parse(s_array[20].ToString().Trim());
                s_ptolerance = s_array[21].ToString().Trim();
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                //-------------------------------------------------------------------------------------------------------
                s_module = s_module.Trim().Substring(0, 3) + "P";
                s_value = "Conergy PH " + s_module;
                i_return = B_Prn_Text_TrueType(10 + i_x, 12 + i_y, 58, "Arial", 1, 900, 0, 0, 0, "A1", s_value);

                s_value = "S/N " + s_barcode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 90 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A2", s_value);

                s_value = "P/N " + s_modulecode;
                i_return = B_Prn_Text_TrueType(10 + i_x, 120 + i_y, 36, "Arial", 1, 450, 0, 0, 0, "A3", s_value);

                i_return = B_Get_Graphic_ColorBMP(665 + i_x, 13 + i_y, s_resule + "ConLogo2.bmp");
                i_return = B_Draw_Line('O', 10 + i_x, 170 + i_y, 840, 4);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(349 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(301 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                s_value = "Maximum Power (P";
                i_return = B_Prn_Text_TrueType(10 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A4", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(297 + i_x, 202 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A11", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(347 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A12", s_value);

                s_value = " " + dc_pm.ToString("#,##0") + "Wp";
                i_return = B_Prn_Text_TrueType(725 + i_x, 187 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A41", s_value);

                s_value = "Power Sorting";
                i_return = B_Prn_Text_TrueType(10 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A5", s_value);
                s_value = s_ptolerance;
                i_return = B_Prn_Text_TrueType(735 + i_x, 224 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A51", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(477 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(427 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                s_value = "Maximum Power Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A6", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(423 + i_x, 276 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A13", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(475 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A14", s_value);
                s_value = " " + dc_vpm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 261 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A61", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(473 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = "MPP";
                i_return = B_Prn_Text_TrueType(420 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                s_value = "Maximum Power Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A7", s_value);
                //s_value = "MPP";
                //i_return = B_Prn_Text_TrueType(417 + i_x, 313 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A15", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(470 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A16", s_value);
                s_value = " " + dc_voc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 298 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A71", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(405 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = "OC";
                i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                s_value = "Open Circuit Voltage (V";
                i_return = B_Prn_Text_TrueType(10 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A8", s_value);
                //s_value = "OC";
                //i_return = B_Prn_Text_TrueType(367 + i_x, 350 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A17", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(402 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A21", s_value);
                s_value = " " + dc_ipm.ToString("#,##0.00") + "V";
                i_return = B_Prn_Text_TrueType(725 + i_x, 335 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A81", s_value);

                s_value = ")";
                i_return = B_Prn_Text_TrueType(395 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = "SC";
                i_return = B_Prn_Text_TrueType(360 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                s_value = "Short Circuit Current (I";
                i_return = B_Prn_Text_TrueType(10 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A9", s_value);
                //s_value = "SC";
                //i_return = B_Prn_Text_TrueType(355 + i_x, 387 + i_y, 24, "Arial", 1, 600, 0, 0, 0, "A19", s_value);
                //s_value = ")";
                //i_return = B_Prn_Text_TrueType(390 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A20", s_value);
                s_value = " " + dc_isc.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(745 + i_x, 372 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A91", s_value);

                s_value = "Maximum System Voltage";
                i_return = B_Prn_Text_TrueType(10 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A10", s_value);
                s_value = " 1000V";
                i_return = B_Prn_Text_TrueType(735 + i_x, 409 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A101", s_value);

                s_value = "Series Fuse Rating";
                i_return = B_Prn_Text_TrueType(10 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A102", s_value);
                int number = Convert.ToInt32(dc_fuse);
                if (number < 10)
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(801 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                else
                {
                    s_value = Convert.ToInt32(dc_fuse).ToString();
                    i_return = B_Prn_Text_TrueType(781 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);
                }
                s_value = " A";
                i_return = B_Prn_Text_TrueType(812 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A140", s_value);
                //s_value = Convert.ToInt32(dc_fuse) + " A";
                //i_return = B_Prn_Text_TrueType(775 + i_x, 446 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A103", s_value);

                s_value = "Module Fire Performance";
                i_return = B_Prn_Text_TrueType(10 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A22", s_value);
                s_value = "  Type 1";
                i_return = B_Prn_Text_TrueType(725 + i_x, 483 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A23", s_value);

                s_value = "Certified according to";
                i_return = B_Prn_Text_TrueType(10 + i_x, 520 + i_y, 38, "Arial", 1, 600, 0, 0, 0, "A104", s_value);
                s_value = "UL1703";
                i_return = B_Prn_Text_TrueType_W(725 + i_x, 520 + i_y, 38, 15, "Arial", 1, 600, 0, 0, 0, "A105", s_value);

                s_value = "Nominal electrical data at standard test conditions (STC: irradiance 1000W/㎡,";
                i_return = B_Prn_Text_TrueType(10 + i_x, 570 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A106", s_value); //原来字体大小20 
                s_value = "cell temperature 25℃, AM1.5)                           Measurement accuracy +/-3%";
                i_return = B_Prn_Text_TrueType(10 + i_x, 592 + i_y, 27, "Arial", 1, 400, 0, 0, 0, "A109", s_value);//原来字体大小20  高度592


                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);
                s_value = "WARNING!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A201", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(710 + i_x, 645 + i_y, 34, "Arial", 1, 850, 0, 0, 0, "A202", s_value);
                s_value = "Refer to installation and operation manual before installing,";
                i_return = B_Prn_Text_TrueType(145 + i_x, 680 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A203", s_value);

                s_value = "operating   or   servicing   this   unit.  Do   not  connect";
                i_return = B_Prn_Text_TrueType(145 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A204", s_value);
                s_value = "or";
                i_return = B_Prn_Text_TrueType(820 + i_x, 710 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A110", s_value);

                s_value = "disconnect  plug    contacts  while  system  is   under";
                i_return = B_Prn_Text_TrueType(145 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A205", s_value);
                s_value = "load";
                i_return = B_Prn_Text_TrueType(795 + i_x, 740 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A111", s_value);

                s_value = "current.      Failure to comply  can  result in   a";
                i_return = B_Prn_Text_TrueType(145 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A206", s_value);
                s_value = "hazardous";
                i_return = B_Prn_Text_TrueType(721 + i_x, 770 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A112", s_value);

                s_value = "situation!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 800 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A207", s_value);


                s_value = "AVERTISSEMENT!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A208", s_value);
                s_value = "DANGER!";
                i_return = B_Prn_Text_TrueType(708 + i_x, 850 + i_y, 34, "Arial", 1, 700, 0, 0, 0, "A209", s_value);

                s_value = "Reportez-vous   au   manuel   d'installation   et";
                i_return = B_Prn_Text_TrueType(145 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A210", s_value);
                s_value = "d'utilisation";
                i_return = B_Prn_Text_TrueType(704 + i_x, 885 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2101", s_value);

                s_value = "avant d'installer, utiliser  ou réparer  cet appareil.   Ne";
                i_return = B_Prn_Text_TrueType(145 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A211", s_value);
                s_value = "pas";
                i_return = B_Prn_Text_TrueType(798 + i_x, 915 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2111", s_value);

                s_value = "brancher  ou  débrancher  les  contacts  enfichables";
                i_return = B_Prn_Text_TrueType(145 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A212", s_value);
                s_value = "tant";
                i_return = B_Prn_Text_TrueType(795 + i_x, 945 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2121", s_value);

                s_value = "que système est sous courant de charge.";
                i_return = B_Prn_Text_TrueType(145 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A213", s_value);
                s_value = "Le Non-respect";
                i_return = B_Prn_Text_TrueType(658 + i_x, 975 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A2131", s_value);

                s_value = "peut vous ramener vers une situation dangereuse!";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1005 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A214", s_value);

                s_value = "For field  connections,  use 12 AWG  wires insulated   for";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A217", s_value);
                s_value = "a";
                i_return = B_Prn_Text_TrueType(827 + i_x, 1055 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A118", s_value);

                s_value = "minimum of  90°.rated  for  wet conditions  and  resistant to";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1085 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A218", s_value);

                s_value = "ultra  violet radiation (where exposed)";
                i_return = B_Prn_Text_TrueType(145 + i_x, 1115 + i_y, 30, "Arial", 1, 700, 0, 0, 0, "A220", s_value);

                s_value = "Factory ID:S";
                i_return = B_Prn_Text_TrueType(660 + i_x, 1160 + i_y, 41, "Roman", 1, 900, 0, 0, 0, "A219", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(615 + i_x, 1200 + i_y, 41, "Arial", 1, 900, 0, 0, 0, "A215", s_value);

                i_return = B_Get_Graphic_ColorBMP(7 + i_x, 625 + i_y, s_resule + "Con10.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 750 + i_y, s_resule + "Con11.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 872 + i_y, s_resule + "Con12.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 978 + i_y, s_resule + "Con14.bmp");
                i_return = B_Get_Graphic_ColorBMP(10 + i_x, 1103 + i_y, s_resule + "CSA1.bmp");
                //i_return = B_Get_Graphic_ColorBMP(150 + i_x, 1103 + i_y, s_resule + "");

                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date + "94" + s_modulecode;
                i_return = B_Prn_Barcode(5 + i_x, 1246 + i_y, 0, "1E", 2, 2, 110, Convert.ToChar(78), s_printcode);

                s_value = "Conergy Inc., 2460 W 26  Ave Denver, CO 80211 USA";
                i_return = B_Prn_Text_TrueType(70 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                s_value = "th";
                i_return = B_Prn_Text_TrueType(395 + i_x, 1360 + i_y, 20, "Arial", 1, 700, 0, 0, 0, "A221", s_value);

                //s_value = "Conergy AG, Anckelmannsplatz 1, 20537 Hamburg, Germany";
                //i_return = B_Prn_Text_TrueType(5 + i_x, 1360 + i_y, 33, "Arial", 1, 700, 0, 0, 0, "A216", s_value);

                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        public static bool wf_printlable60(string s_barcode, int i_darkness, int i_x, int i_y, string[] s_array)
        {
            bool b_result = false;
            int i_return, i_pnum;
            string s_value, s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;

            try
            {
                i_pnum = int.Parse(s_array[6].ToString().Trim());
                d_date = DateTime.Parse(s_array[19].ToString().Trim());
                s_year = d_date.ToString("yy");
                s_month = d_date.ToString("MM");
                s_day = d_date.ToString("dd");
                if (i_darkness <= 0)
                {
                    i_darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(i_darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");

                s_date = s_year + s_month + s_day;
                s_printcode = "21" + s_barcode + "<FNC1>" + "11" + s_date;
                i_return = B_Prn_Barcode(110 + i_x, 50 + i_y, 0, "1E", 2, 10, 180, Convert.ToChar(78), s_printcode);               
                s_value = "S/N: " + s_barcode;
                if (s_barcode.Length >= 15)
                {
                    i_return = B_Prn_Text_TrueType(108 + i_x, 230 + i_y, 64, "Arial", 1, 600, 0, 0, 0, "A1", s_value);
                }
                if (s_barcode.Length < 15)
                {
                    i_return = B_Prn_Text_TrueType(122 + i_x, 230 + i_y, 64, "Arial", 1, 600, 0, 0, 0, "A1", s_value);
                }
 
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// MEMCSunEdison 铭牌打印
        /// </summary>
        /// <param name="data">组件对应的参数信息</param>
        /// <returns>返回打印结果（成功:True 失败:False）</returns>
        public static bool wf_printlable54(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, s_x, s_y;
            string s_resule, s_value, productModule, partNumber;

            try
            {

                #region 获取托盘对应工单的OEM信息

                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();


                DataSet dsOEMInfo = IVTestDateObject.GetWorkOrderOEMByOrderNumberOrLotNumber(string.Empty, data.LotNo);

                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    data.ErrorMessage = IVTestDateObject.ErrorMsg;
                    return false;
                }
                if (dsOEMInfo.Tables[0].Rows.Count == 0)
                {
                    data.ErrorMessage = string.Format(@"工单【{0}】未设置对用的OEM信息，
                                                        请联系工艺进行设定！", dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["ORDER_NUMBER"].ToString());
                    return false;
                }

                string cellType = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_TYPE"].ToString();

                productModule = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CUSROMER"].ToString()
                                + "-"
                                + cellType
                                + data.PowersetStandardPM
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["STRUCTURE_PARAM"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["GLASS_TYPE"].ToString()
                                + "-"
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_AUTHENTICATION_CODE"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["JUNCTION_BOX"].ToString();
                partNumber = "M"
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_SUPPLIER"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_MODEL"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_DESIGN"].ToString();

                #endregion

                i_pnum = data.PrintQty;
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";

                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                //测量纸张像素点数 宽度：987 、高度：1208

                s_x = data.X + 40;
                s_y = data.Y + 12;
                i_return = B_Get_Graphic_ColorBMP(s_x - 10, s_y + 5, s_resule + "SE_Logo.bmp");

                s_value = "SunEdison";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 0, 40, "Arial", 1, 600, 0, 0, 0, "A1", s_value);
                s_value = "www.sunedison.com";
                i_return = B_Prn_Text_TrueType(s_x + 680, s_y + 35, 32, "Arial", 1, 450, 0, 0, 0, "A11", s_value);
                s_value = "Designed in United States of America";
                i_return = B_Prn_Text_TrueType(s_x + 482, s_y + 71, 32, "Arial", 1, 450, 0, 0, 0, "A12", s_value);

                s_value = string.Format("Silvantis     {0}330 Family", cellType);
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 136, 35, "Arial", 1, 450, 0, 0, 0, "A13", s_value);
                s_value = "TM";
                i_return = B_Prn_Text_TrueType(s_x + 119, s_y + 136, 22, "Arial", 1, 450, 0, 0, 0, "A131", s_value);

                s_value = productModule;
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 177, 55, "Arial", 1, 700, 0, 0, 0, "A14", s_value);
                s_value = "Mono-crystalline PV Module";
                i_return = B_Prn_Text_TrueType(s_x + 537, s_y + 195, 35, "Arial", 1, 450, 0, 0, 0, "A15", s_value);

                //五大参数及表格

                i_return = B_Draw_Line('O', s_x + 0, 236 + s_y, 927, 4);

                i_return = B_Draw_Line('O', s_x + 9, 248 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 323 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 388 + s_y, 907, 4);

                i_return = B_Draw_Line('O', 9 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 194 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 372 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 566 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 723 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 913 + s_x, 248 + s_y, 4, 140);


                s_value = "Pmax";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 260, 55, "Arial", 1, 700, 0, 0, 0, "A2", s_value);
                s_value = data.PowersetStandardPM + "W";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 330, 55, "Arial", 1, 700, 0, 0, 0, "A21", s_value);
                s_value = "Impp";
                i_return = B_Prn_Text_TrueType(s_x + 236, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A22", s_value);
                s_value = data.PowersetStandardIPM.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 224, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A23", s_value);
                s_value = "Vmpp";
                i_return = B_Prn_Text_TrueType(s_x + 407, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A24", s_value);
                s_value = data.PowersetStandardVPM.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 396, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A25", s_value);
                s_value = "Isc";
                i_return = B_Prn_Text_TrueType(s_x + 613, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A26", s_value);
                s_value = data.PowersetStandardISC.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 587, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A27", s_value);
                s_value = "Voc";
                i_return = B_Prn_Text_TrueType(s_x + 780, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A28", s_value);
                s_value = data.PowersetStandardVOC.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 760, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A29", s_value);

                //参数环境描述
                s_value = "All Specifications at STC:25 C,1000w/m ,AM 1.5";
                i_return = B_Prn_Text_TrueType(s_x + 165, s_y + 390, 32, "Arial", 1, 450, 0, 0, 0, "A3", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 165 + 338, s_y + 387, 20, "Arial", 1, 450, 0, 0, 0, "A302", s_value);
                s_value = "2";
                i_return = B_Prn_Text_TrueType(s_x + 650, s_y + 390, 18, "Arial", 1, 450, 0, 0, 0, "A301", s_value);

                s_value = "SN:" + data.LotNo;
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 449, 40, "Arial", 1, 500, 0, 0, 0, "A31", s_value);
                s_value = data.LotNo;
                i_return = B_Prn_Barcode(s_x + 590, s_y + 430, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);

                //B_Prn_Barcode(3 + i_x, 8 + 1200, 0, "1", 1, 2, 40, Convert.ToChar(78), s_value);

                s_value = "Power Selection:0/+5W";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 494, 40, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                s_value = "Power Tolerance：+/-3%";
                i_return = B_Prn_Text_TrueType(s_x + 508, s_y + 494, 40, "Arial", 1, 500, 0, 0, 0, "A41", s_value);
                s_value = "Max.System Voltage:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 539, 40, "Arial", 1, 500, 0, 0, 0, "A42", s_value);
                s_value = "1000V(IEC),1000V(UL)";
                i_return = B_Prn_Text_TrueType(s_x + 520, s_y + 539, 40, "Arial", 1, 500, 0, 0, 0, "A43", s_value);
                s_value = "Fire Resistance Rating:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 584, 40, "Arial", 1, 500, 0, 0, 0, "A44", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 584, 40, "Arial", 1, 500, 0, 0, 0, "A45", s_value);
                s_value = "Application Class:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 629, 40, "Arial", 1, 500, 0, 0, 0, "A46", s_value);
                s_value = "Class A";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 629, 40, "Arial", 1, 500, 0, 0, 0, "A47", s_value);
                s_value = "Fuse Rating：15A";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 674, 40, "Arial", 1, 500, 0, 0, 0, "A48", s_value);
                s_value = "Weight：22 Kg";
                i_return = B_Prn_Text_TrueType(s_x + 644, s_y + 674, 40, "Arial", 1, 500, 0, 0, 0, "A49", s_value);


                s_value = data.LotNo.Substring(3, 6)
                          + " "
                          + data.CoefPM.ToString().Substring(0, 6)
                          + "W "
                          + data.CoefIPM.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVPM.ToString().Substring(0, 4)
                          + "V "
                          + data.CoefISC.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVOC.ToString().Substring(0, 4)
                          + "V";

                i_return = B_Prn_Barcode(s_x + 30, s_y + 720, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(s_x + 136, s_y + 770, 40, "Arial", 1, 450, 0, 0, 0, "A5", s_value);

                i_return = B_Draw_Line('O', s_x + 0, s_y + 810, 927, 4);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness - 5);
                i_return = B_Get_Graphic_ColorBMP(s_x + 46, s_y + 818, s_resule + "PVSIEC.bmp");
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);

                i_return = B_Get_Graphic_ColorBMP(s_x + 242, s_y + 828, s_resule + "SEC.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 438, s_y + 840, s_resule + "SEE.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 600, s_y + 840, s_resule + "SED.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 770, s_y + 830, s_resule + "SER.bmp");

                i_return = B_Draw_Line('O', s_x + 0, s_y + 990, 927, 4);


                //底部
                i_return = B_Get_Graphic_ColorBMP(s_x + 12, s_y + 994, s_resule + "SEPower.bmp");

                s_value = "WARNING - ELECTRICAL HAZARD";
                i_return = B_Prn_Text_TrueType(s_x + 224 + 80, s_y + 998, 40, "Arial", 1, 700, 1, 0, 0, "A6", s_value);
                s_value = "High Voltage in Sunlight-Authorized Personnel Only.Use";
                i_return = B_Prn_Text_TrueType(s_x + 177 + 80, s_y + 1039, 30, "Arial", 1, 500, 1, 0, 0, "A61", s_value);
                s_value = "12AWG wires insulated for a minimum of 90 C.CU only.";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80, s_y + 1071, 30, "Arial", 1, 500, 1, 0, 0, "A62", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80 + 501, s_y + 1068, 20, "Arial", 1, 500, 1, 0, 0, "A621", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(s_x + 419 + 80, s_y + 1112, 40, "Arial", 1, 700, 1, 0, 0, "A63", s_value);



                //------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// MEMCSunEdison 铭牌打印
        /// </summary>
        /// <param name="data">组件对应的参数信息</param>
        /// <returns>返回打印结果（成功:True 失败:False）</returns>
        public static bool wf_printlable541(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, s_x, s_y;
            string s_resule, s_value, productModule, partNumber;

            try
            {

                #region 获取托盘对应工单的OEM信息

                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();


                DataSet dsOEMInfo = IVTestDateObject.GetWorkOrderOEMByOrderNumberOrLotNumber(string.Empty, data.LotNo);

                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    data.ErrorMessage = IVTestDateObject.ErrorMsg;
                    return false;
                }
                if (dsOEMInfo.Tables[0].Rows.Count == 0)
                {
                    data.ErrorMessage = string.Format(@"工单【{0}】未设置对用的OEM信息，
                                                        请联系工艺进行设定！", dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["ORDER_NUMBER"].ToString());
                    return false;
                }

                string cellType = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_TYPE"].ToString();

                productModule = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CUSROMER"].ToString()
                                + "-"
                                + cellType
                                + data.PowersetStandardPM
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["STRUCTURE_PARAM"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["GLASS_TYPE"].ToString()
                                + "-"
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_AUTHENTICATION_CODE"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["JUNCTION_BOX"].ToString();
                partNumber = "M"
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_SUPPLIER"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_MODEL"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_DESIGN"].ToString();

                #endregion

                i_pnum = data.PrintQty;
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";

                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                //测量纸张像素点数 宽度：987 、高度：1208

                s_x = data.X + 40;
                s_y = data.Y + 12;
                i_return = B_Get_Graphic_ColorBMP(s_x - 10, s_y + 5, s_resule + "SE_Logo.bmp");

                s_value = "SunEdison";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 0, 40, "Arial", 1, 600, 0, 0, 0, "A1", s_value);
                s_value = "www.sunedison.com";
                i_return = B_Prn_Text_TrueType(s_x + 685, s_y + 35, 32, "Arial", 1, 450, 0, 0, 0, "A11", s_value);
                s_value = "Designed in United States of America";
                i_return = B_Prn_Text_TrueType(s_x + 482, s_y + 71, 32, "Arial", 1, 450, 0, 0, 0, "A12", s_value);

                s_value = string.Format("Silvantis     {0} Series PID Free", cellType);
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 136, 35, "Arial", 1, 450, 0, 0, 0, "A13", s_value);
                s_value = "TM";
                i_return = B_Prn_Text_TrueType(s_x + 119, s_y + 136, 22, "Arial", 1, 450, 0, 0, 0, "A131", s_value);

                s_value = productModule;
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 177, 55, "Arial", 1, 700, 0, 0, 0, "A14", s_value);
                s_value = "Mono-crystalline PV Module";
                i_return = B_Prn_Text_TrueType(s_x + 537, s_y + 195, 35, "Arial", 1, 450, 0, 0, 0, "A15", s_value);

                //五大参数及表格

                i_return = B_Draw_Line('O', s_x + 0, 236 + s_y, 927, 4);

                i_return = B_Draw_Line('O', s_x + 9, 248 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 323 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 388 + s_y, 907, 4);

                i_return = B_Draw_Line('O', 9 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 194 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 372 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 566 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 723 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 913 + s_x, 248 + s_y, 4, 140);


                s_value = "Pmax";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 260, 55, "Arial", 1, 700, 0, 0, 0, "A2", s_value);
                s_value = data.PowersetStandardPM + "W";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 330, 55, "Arial", 1, 700, 0, 0, 0, "A21", s_value);
                s_value = "Impp";
                i_return = B_Prn_Text_TrueType(s_x + 236, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A22", s_value);
                s_value = data.PowersetStandardIPM.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 224, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A23", s_value);
                s_value = "Vmpp";
                i_return = B_Prn_Text_TrueType(s_x + 407, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A24", s_value);
                s_value = data.PowersetStandardVPM.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 396, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A25", s_value);
                s_value = "Isc";
                i_return = B_Prn_Text_TrueType(s_x + 613, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A26", s_value);
                s_value = data.PowersetStandardISC.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 587, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A27", s_value);
                s_value = "Voc";
                i_return = B_Prn_Text_TrueType(s_x + 780, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A28", s_value);
                s_value = data.PowersetStandardVOC.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 760, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A29", s_value);

                //参数环境描述
                s_value = "All Specifications at STC:25 C,1000w/m ,AM 1.5";
                i_return = B_Prn_Text_TrueType(s_x + 165, s_y + 390, 32, "Arial", 1, 450, 0, 0, 0, "A3", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 165 + 338, s_y + 387, 20, "Arial", 1, 450, 0, 0, 0, "A302", s_value);
                s_value = "2";
                i_return = B_Prn_Text_TrueType(s_x + 650, s_y + 390, 18, "Arial", 1, 450, 0, 0, 0, "A301", s_value);

                s_value = "SN:" + data.LotNo;
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 449, 40, "Arial", 1, 500, 0, 0, 0, "A31", s_value);
                s_value = data.LotNo;
                i_return = B_Prn_Barcode(s_x + 590, s_y + 430, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);

                //B_Prn_Barcode(3 + i_x, 8 + 1200, 0, "1", 1, 2, 40, Convert.ToChar(78), s_value);

                s_value = "Power Selection:0/+5W";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 494, 40, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                s_value = "Power Tolerance：+/-3%";
                i_return = B_Prn_Text_TrueType(s_x + 508, s_y + 494, 40, "Arial", 1, 500, 0, 0, 0, "A41", s_value);
                s_value = "Max.System Voltage:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 539, 40, "Arial", 1, 500, 0, 0, 0, "A42", s_value);
                s_value = "1000V(IEC),1000V(UL)";
                i_return = B_Prn_Text_TrueType(s_x + 520, s_y + 539, 40, "Arial", 1, 500, 0, 0, 0, "A43", s_value);
                s_value = "Fire Resistance Rating:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 584, 40, "Arial", 1, 500, 0, 0, 0, "A44", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 584, 40, "Arial", 1, 500, 0, 0, 0, "A45", s_value);
                s_value = "Application Class:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 629, 40, "Arial", 1, 500, 0, 0, 0, "A46", s_value);
                s_value = "Class A";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 629, 40, "Arial", 1, 500, 0, 0, 0, "A47", s_value);
                s_value = "Fuse Rating：15A";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 674, 40, "Arial", 1, 500, 0, 0, 0, "A48", s_value);
                s_value = "Weight：22 Kg";
                i_return = B_Prn_Text_TrueType(s_x + 644, s_y + 674, 40, "Arial", 1, 500, 0, 0, 0, "A49", s_value);


                s_value = data.LotNo.Substring(3, 6)
                          + " "
                          + data.CoefPM.ToString().Substring(0, 6)
                          + "W "
                          + data.CoefIPM.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVPM.ToString().Substring(0, 4)
                          + "V "
                          + data.CoefISC.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVOC.ToString().Substring(0, 4)
                          + "V";

                i_return = B_Prn_Barcode(s_x + 30, s_y + 720, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(s_x + 136, s_y + 770, 40, "Arial", 1, 450, 0, 0, 0, "A5", s_value);

                i_return = B_Draw_Line('O', s_x + 0, s_y + 810, 927, 4);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness - 5);
                i_return = B_Get_Graphic_ColorBMP(s_x + 46, s_y + 818, s_resule + "PVSIEC.bmp");
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);

                i_return = B_Get_Graphic_ColorBMP(s_x + 242, s_y + 828, s_resule + "SEC.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 438, s_y + 840, s_resule + "SEE.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 600, s_y + 840, s_resule + "SED.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 770, s_y + 830, s_resule + "SER.bmp");

                i_return = B_Draw_Line('O', s_x + 0, s_y + 990, 927, 4);


                //底部
                i_return = B_Get_Graphic_ColorBMP(s_x + 12, s_y + 994, s_resule + "SEPower.bmp");

                s_value = "WARNING - ELECTRICAL HAZARD";
                i_return = B_Prn_Text_TrueType(s_x + 224 + 80, s_y + 998, 40, "Arial", 1, 700, 1, 0, 0, "A6", s_value);
                s_value = "High Voltage in Sunlight-Authorized Personnel Only.Use";
                i_return = B_Prn_Text_TrueType(s_x + 177 + 80, s_y + 1039, 30, "Arial", 1, 500, 1, 0, 0, "A61", s_value);
                s_value = "12AWG wires insulated for a minimum of 90 C.CU only.";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80, s_y + 1071, 30, "Arial", 1, 500, 1, 0, 0, "A62", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80 + 501, s_y + 1068, 20, "Arial", 1, 500, 1, 0, 0, "A621", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(s_x + 419 + 80, s_y + 1112, 40, "Arial", 1, 700, 1, 0, 0, "A63", s_value);



                //------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// MEMCSunEdison 铭牌打印
        /// </summary>
        /// <param name="data">组件对应的参数信息</param>
        /// <returns>返回打印结果（成功:True 失败:False）</returns>
        public static bool wf_printlable542(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, s_x, s_y;
            string s_resule, s_value, productModule, partNumber;

            try
            {

                #region 获取托盘对应工单的OEM信息

                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();


                DataSet dsOEMInfo = IVTestDateObject.GetWorkOrderOEMByOrderNumberOrLotNumber(string.Empty, data.LotNo);

                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    data.ErrorMessage = IVTestDateObject.ErrorMsg;
                    return false;
                }
                if (dsOEMInfo.Tables[0].Rows.Count == 0)
                {
                    data.ErrorMessage = string.Format(@"工单【{0}】未设置对用的OEM信息，
                                                        请联系工艺进行设定！", dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["ORDER_NUMBER"].ToString());
                    return false;
                }

                string cellType = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_TYPE"].ToString();

                productModule = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CUSROMER"].ToString()
                                + "-"
                                + cellType
                                + data.PowersetStandardPM
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["STRUCTURE_PARAM"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["GLASS_TYPE"].ToString()
                                + "-"
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_AUTHENTICATION_CODE"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["JUNCTION_BOX"].ToString();
                partNumber = "M"
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_SUPPLIER"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_MODEL"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                            + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_DESIGN"].ToString();

                #endregion

                i_pnum = data.PrintQty;
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";

                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                //测量纸张像素点数 宽度：987 、高度：1208

                s_x = data.X + 40;
                s_y = data.Y + 12;
                i_return = B_Get_Graphic_ColorBMP(s_x - 10, s_y + 5, s_resule + "SE_Logo.bmp");

                s_value = "SunEdison";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 0, 40, "Arial", 1, 600, 0, 0, 0, "A1", s_value);
                s_value = "www.sunedison.com";
                i_return = B_Prn_Text_TrueType(s_x + 685, s_y + 35, 32, "Arial", 1, 450, 0, 0, 0, "A11", s_value);
                s_value = "Designed in The United States of America";
                i_return = B_Prn_Text_TrueType(s_x + 435, s_y + 71, 32, "Arial", 1, 450, 0, 0, 0, "A12", s_value);

                s_value = string.Format("Silvantis    {0} Series PID Free", cellType);
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 136, 35, "Arial", 1, 450, 0, 0, 0, "A13", s_value);

                i_return = B_Get_Graphic_ColorBMP(s_x + 120, s_y + 142, s_resule + "SER_Logo.bmp");

                s_value = productModule;
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 177, 55, "Arial", 1, 700, 0, 0, 0, "A14", s_value);
                s_value = "Monocrystalline PV Module";
                i_return = B_Prn_Text_TrueType(s_x + 537, s_y + 195, 35, "Arial", 1, 450, 0, 0, 0, "A15", s_value);

                //五大参数及表格

                i_return = B_Draw_Line('O', s_x + 0, 236 + s_y, 927, 4);

                i_return = B_Draw_Line('O', s_x + 9, 248 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 323 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 388 + s_y, 907, 4);

                i_return = B_Draw_Line('O', 9 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 194 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 372 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 566 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 723 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 913 + s_x, 248 + s_y, 4, 140);


                s_value = "Pmax";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 260, 55, "Arial", 1, 700, 0, 0, 0, "A2", s_value);
                s_value = data.PowersetStandardPM + "W";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 330, 55, "Arial", 1, 700, 0, 0, 0, "A21", s_value);
                s_value = "Impp";
                i_return = B_Prn_Text_TrueType(s_x + 236, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A22", s_value);
                s_value = data.PowersetStandardIPM.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 224, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A23", s_value);
                s_value = "Vmpp";
                i_return = B_Prn_Text_TrueType(s_x + 407, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A24", s_value);
                s_value = data.PowersetStandardVPM.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 396, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A25", s_value);
                s_value = "Isc";
                i_return = B_Prn_Text_TrueType(s_x + 613, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A26", s_value);
                s_value = data.PowersetStandardISC.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 587, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A27", s_value);
                s_value = "Voc";
                i_return = B_Prn_Text_TrueType(s_x + 780, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A28", s_value);
                s_value = data.PowersetStandardVOC.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 760, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A29", s_value);

                //参数环境描述
                s_value = "All Specifications at STC:25 C,1000 W/m ,AM 1.5";
                i_return = B_Prn_Text_TrueType(s_x + 165, s_y + 390, 32, "Arial", 1, 450, 0, 0, 0, "A3", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 165 + 338, s_y + 387, 20, "Arial", 1, 450, 0, 0, 0, "A302", s_value);
                s_value = "2";
                i_return = B_Prn_Text_TrueType(s_x + 663, s_y + 390, 18, "Arial", 1, 450, 0, 0, 0, "A301", s_value);

                s_value = "SN:" + data.LotNo;
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 449, 40, "Arial", 1, 500, 0, 0, 0, "A31", s_value);
                s_value = data.LotNo;
                i_return = B_Prn_Barcode(s_x + 590, s_y + 430, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);

                //B_Prn_Barcode(3 + i_x, 8 + 1200, 0, "1", 1, 2, 40, Convert.ToChar(78), s_value);

                s_value = "Production Tolerance:0 W to +5 W";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 494, 40, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                s_value = "Max.System Voltage:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 539, 40, "Arial", 1, 500, 0, 0, 0, "A42", s_value);
                s_value = "1000V(IEC),1000V(UL)";
                i_return = B_Prn_Text_TrueType(s_x + 520, s_y + 539, 40, "Arial", 1, 500, 0, 0, 0, "A43", s_value);
                s_value = "Fire Resistance Rating:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 584, 40, "Arial", 1, 500, 0, 0, 0, "A44", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(s_x + 758, s_y + 584, 40, "Arial", 1, 500, 0, 0, 0, "A45", s_value);
                s_value = "Application Class:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 629, 40, "Arial", 1, 500, 0, 0, 0, "A46", s_value);
                s_value = "Class A (IEC)";
                i_return = B_Prn_Text_TrueType(s_x + 664, s_y + 629, 40, "Arial", 1, 500, 0, 0, 0, "A47", s_value);
                s_value = "Fuse Rating：15 A";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 674, 40, "Arial", 1, 500, 0, 0, 0, "A48", s_value);
                s_value = "Weight：22 kg";
                i_return = B_Prn_Text_TrueType(s_x + 646, s_y + 674, 40, "Arial", 1, 500, 0, 0, 0, "A49", s_value);


                s_value = data.LotNo.Substring(3, 6)
                          + " "
                          + data.CoefPM.ToString().Substring(0, 6)
                          + "W "
                          + data.CoefIPM.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVPM.ToString().Substring(0, 4)
                          + "V "
                          + data.CoefISC.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVOC.ToString().Substring(0, 4)
                          + "V";

                i_return = B_Prn_Barcode(s_x + 30, s_y + 720, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(s_x + 136, s_y + 770, 40, "Arial", 1, 450, 0, 0, 0, "A5", s_value);

                i_return = B_Draw_Line('O', s_x + 0, s_y + 810, 927, 4);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness - 5);
                i_return = B_Get_Graphic_ColorBMP(s_x + 46, s_y + 818, s_resule + "PVSIEC.bmp");
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);

                i_return = B_Get_Graphic_ColorBMP(s_x + 242, s_y + 828, s_resule + "SEC.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 438, s_y + 840, s_resule + "SEE.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 600, s_y + 840, s_resule + "SED.bmp");
                i_return = B_Get_Graphic_ColorBMP(s_x + 770, s_y + 830, s_resule + "SER.bmp");

                i_return = B_Draw_Line('O', s_x + 0, s_y + 990, 927, 4);


                //底部
                i_return = B_Get_Graphic_ColorBMP(s_x + 12, s_y + 994, s_resule + "SEPower.bmp");

                s_value = "WARNING - ELECTRICAL HAZARD";
                i_return = B_Prn_Text_TrueType(s_x + 224 + 80, s_y + 998, 40, "Arial", 1, 700, 1, 0, 0, "A6", s_value);
                s_value = "High Voltage in Sunlight-Authorized Personnel Only.Use";
                i_return = B_Prn_Text_TrueType(s_x + 177 + 80, s_y + 1039, 30, "Arial", 1, 500, 1, 0, 0, "A61", s_value);
                s_value = "12 AWG wires insulated for a minimum of 90 C. Cu only.";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80, s_y + 1071, 30, "Arial", 1, 500, 1, 0, 0, "A62", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80 + 509, s_y + 1068, 20, "Arial", 1, 500, 1, 0, 0, "A621", s_value);
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(s_x + 419 + 80, s_y + 1112, 40, "Arial", 1, 700, 1, 0, 0, "A63", s_value);

                //------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        /// <summary>
        /// add by chao.pang MEMCSunEdison 铭牌打印  20150604 张华
        /// </summary>
        /// <param name="data">组件对应的参数信息</param>
        /// <returns>返回打印结果（成功:True 失败:False）</returns>
        public static bool wf_printlable543(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, s_x, s_y;
            string s_resule, s_value, productModule, partNumber;

            try
            {

                #region 获取托盘对应工单的OEM信息

                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();


                DataSet dsOEMInfo = IVTestDateObject.GetWorkOrderOEMByOrderNumberOrLotNumber(string.Empty, data.LotNo);

                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    data.ErrorMessage = IVTestDateObject.ErrorMsg;
                    return false;
                }
                if (dsOEMInfo.Tables[0].Rows.Count == 0)
                {
                    data.ErrorMessage = string.Format(@"工单【{0}】未设置对用的OEM信息，
                                                        请联系工艺进行设定！", dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["ORDER_NUMBER"].ToString());
                    return false;
                }

                string cellType = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_TYPE"].ToString();

                productModule = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CUSROMER"].ToString()
                                + "-"
                                + cellType
                                + data.PowersetStandardPM
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["STRUCTURE_PARAM"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["GLASS_TYPE"].ToString()
                                + "-"
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_AUTHENTICATION_CODE"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["JUNCTION_BOX"].ToString();
                partNumber = partNumber = "M"
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_SUPPLIER"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_MODEL"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["SE_MODULE_TYPE"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_DESIGN"].ToString().ToUpper();

                #endregion

                i_pnum = data.PrintQty;
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";

                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                //测量纸张像素点数 宽度：987 、高度：1208

                s_x = data.X + 40;
                s_y = data.Y + 12;
                i_return = B_Get_Graphic_ColorBMP(s_x - 10, s_y + 5, s_resule + "SE_Logo.bmp");

                s_value = "SunEdison";
                i_return = B_Prn_Text_TrueType(s_x + 762, s_y + 0, 40, "Arial", 1, 600, 0, 0, 0, "A1", s_value);
                s_value = "www.sunedison.com";
                i_return = B_Prn_Text_TrueType(s_x + 686, s_y + 35, 32, "Arial", 1, 450, 0, 0, 0, "A11", s_value);
                s_value = "Designed in The United States of America";
                i_return = B_Prn_Text_TrueType(s_x + 430, s_y + 71, 32, "Arial", 1, 450, 0, 0, 0, "A12", s_value);

                s_value = string.Format("Silvantis    {0}-Series PID Free", cellType);
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 136, 35, "Arial", 1, 450, 0, 0, 0, "A13", s_value);
                i_return = B_Get_Graphic_ColorBMP(s_x + 120, s_y + 138, s_resule + "SER_Logo.bmp");
                s_value = productModule;
                i_return = B_Prn_Text_TrueType(s_x + 9, s_y + 177, 55, "Arial", 1, 700, 0, 0, 0, "A14", s_value);
                s_value = "Monocrystalline PV Module";
                i_return = B_Prn_Text_TrueType(s_x + 537, s_y + 195, 35, "Arial", 1, 450, 0, 0, 0, "A15", s_value);

                //五大参数及表格

                i_return = B_Draw_Line('O', s_x + 0, 236 + s_y, 927, 4);

                i_return = B_Draw_Line('O', s_x + 9, 248 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 323 + s_y, 907, 4);
                i_return = B_Draw_Line('O', s_x + 9, 388 + s_y, 907, 4);

                i_return = B_Draw_Line('O', 9 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 194 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 372 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 566 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 723 + s_x, 248 + s_y, 4, 140);
                i_return = B_Draw_Line('O', 913 + s_x, 248 + s_y, 4, 140);


                s_value = "Pmax";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 260, 55, "Arial", 1, 700, 0, 0, 0, "A2", s_value);
                s_value = data.PowersetStandardPM + "W";
                i_return = B_Prn_Text_TrueType(s_x + 35, s_y + 330, 55, "Arial", 1, 700, 0, 0, 0, "A21", s_value);
                s_value = "Impp";
                i_return = B_Prn_Text_TrueType(s_x + 236, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A22", s_value);
                s_value = data.PowersetStandardIPM.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 224, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A23", s_value);
                s_value = "Vmpp";
                i_return = B_Prn_Text_TrueType(s_x + 407, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A24", s_value);
                s_value = data.PowersetStandardVPM.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 396, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A25", s_value);
                s_value = "Isc";
                i_return = B_Prn_Text_TrueType(s_x + 613, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A26", s_value);
                s_value = data.PowersetStandardISC.ToString("#,##0.00") + "A";
                i_return = B_Prn_Text_TrueType(s_x + 587, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A27", s_value);
                s_value = "Voc";
                i_return = B_Prn_Text_TrueType(s_x + 780, s_y + 260, 55, "Arial", 1, 500, 0, 0, 0, "A28", s_value);
                s_value = data.PowersetStandardVOC.ToString("#,##00.0") + "V";
                i_return = B_Prn_Text_TrueType(s_x + 760, s_y + 330, 55, "Arial", 1, 500, 0, 0, 0, "A29", s_value);

                //参数环境描述
                s_value = "All Specifications at STC: 25 C,1000W/m , AM 1.5";
                i_return = B_Prn_Text_TrueType(s_x + 165, s_y + 393, 32, "Arial", 1, 450, 0, 0, 0, "A3", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 165 + 344, s_y + 390, 20, "Arial", 1, 450, 0, 0, 0, "A302", s_value);
                s_value = "2";
                i_return = B_Prn_Text_TrueType(s_x + 663, s_y + 391, 18, "Arial", 1, 450, 0, 0, 0, "A301", s_value);

                s_value = "SN: " + data.LotNo;
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 449, 30, "Arial", 1, 500, 0, 0, 0, "A31", s_value);
                s_value = data.LotNo;
                i_return = B_Prn_Barcode(s_x + 590, s_y + 430, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);

                //B_Prn_Barcode(3 + i_x, 8 + 1200, 0, "1", 1, 2, 40, Convert.ToChar(78), s_value);

                s_value = "Production Tolerance: 0 W to +5 W";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 481, 30, "Arial", 1, 500, 0, 0, 0, "A4", s_value);
                s_value = "Max. System Voltage:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 513, 30, "Arial", 1, 500, 0, 0, 0, "A42", s_value);
                s_value = "1000V(IEC), 1000V (UL)";
                i_return = B_Prn_Text_TrueType(s_x + 610, s_y + 513, 30, "Arial", 1, 500, 0, 0, 0, "A43", s_value);
                s_value = "Module Fire Performance:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 545, 30, "Arial", 1, 500, 0, 0, 0, "A44", s_value);
                s_value = "Type 1";
                i_return = B_Prn_Text_TrueType(s_x + 805, s_y + 545, 30, "Arial", 1, 500, 0, 0, 0, "A45", s_value);
                s_value = "Application Class:";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 577, 30, "Arial", 1, 500, 0, 0, 0, "A46", s_value);
                s_value = "Class A (IEC)";
                i_return = B_Prn_Text_TrueType(s_x + 725, s_y + 577, 30, "Arial", 1, 500, 0, 0, 0, "A47", s_value);
                s_value = "Fuse Rating: 15 A";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 609, 30, "Arial", 1, 500, 0, 0, 0, "A48", s_value);
                s_value = "Weight: 22 kg";
                i_return = B_Prn_Text_TrueType(s_x + 715, s_y + 609, 30, "Arial", 1, 500, 0, 0, 0, "A49", s_value);

                s_value = "System Fire Class Rating: See Installation Instructions for Installation";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 642, 25, "Arial", 1, 700, 0, 0, 0, "A50", s_value);
                s_value = "Requirements to Achieve a Specified System Fire Class Rating with this Product";
                i_return = B_Prn_Text_TrueType(s_x + 47, s_y + 670, 25, "Arial", 1, 700, 0, 0, 0, "A51", s_value);

                s_value = data.LotNo.Substring(3, 6)
                          + " "
                          + data.CoefPM.ToString().Substring(0, 6)
                          + "W "
                          + data.CoefIPM.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVPM.ToString().Substring(0, 4)
                          + "V "
                          + data.CoefISC.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVOC.ToString().Substring(0, 4)
                          + "V";

                i_return = B_Prn_Barcode(s_x + 30, s_y + 702, 0, "1", 2, 2, 51, Convert.ToChar(78), s_value);
                i_return = B_Prn_Text_TrueType(s_x + 136, s_y + 752, 40, "Arial", 1, 450, 0, 0, 0, "A5", s_value);

                //底部
                i_return = B_Get_Graphic_ColorBMP(s_x + 12, s_y + 986, s_resule + "SEPower.bmp");
                i_return = B_Draw_Line('O', s_x + 0, s_y + 792, 927, 4);
                //设置打印浓度
                //i_return = B_Set_Darkness(data.Darkness - 5);
                //i_return = B_Get_Graphic_ColorBMP(s_x + 46, s_y + 800, s_resule + "PVSIEC.bmp");
                //设置打印浓度
                //i_return = B_Set_Darkness(data.Darkness);

                //i_return = B_Get_Graphic_ColorBMP(s_x + 242, s_y + 810, s_resule + "SEC.bmp");
                //i_return = B_Get_Graphic_ColorBMP(s_x + 438, s_y + 822, s_resule + "SEE.bmp");
                //i_return = B_Get_Graphic_ColorBMP(s_x + 600, s_y + 822, s_resule + "SED.bmp");
                //i_return = B_Get_Graphic_ColorBMP(s_x + 770, s_y + 812, s_resule + "SER.bmp");

                i_return = B_Draw_Line('O', s_x + 0, s_y + 972, 927, 4);


                s_value = "WARNING - ELECTRICAL HAZARD";
                i_return = B_Prn_Text_TrueType(s_x + 224 + 80, s_y + 980, 40, "Arial", 1, 700, 1, 0, 0, "A6", s_value);
                s_value = "High Voltage in Sunlight-Authorized Personnel Only. Use";
                i_return = B_Prn_Text_TrueType(s_x + 177 + 80, s_y + 1021, 30, "Arial", 1, 500, 1, 0, 0, "A61", s_value);
                s_value = "12 AWG wires insulated for a minimum of 90 C. Cu only.";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80, s_y + 1053, 30, "Arial", 1, 500, 1, 0, 0, "A62", s_value);
                s_value = "o";
                i_return = B_Prn_Text_TrueType(s_x + 195 + 80 + 507, s_y + 1050, 20, "Arial", 1, 500, 1, 0, 0, "A621", s_value);

                i_return = B_Prn_Barcode(s_x + 570, s_y + 1090, 0, "1", 2, 2, 45, Convert.ToChar(78), partNumber);
                i_return = B_Prn_Text_TrueType(s_x + 630, s_y + 1135, 30, "Arial", 1, 700, 0, 0, 0, "A64", partNumber); 
                
                s_value = "Made in China";
                i_return = B_Prn_Text_TrueType(s_x + 280 + 80, s_y + 1136, 32, "Arial", 1, 700, 1, 0, 0, "A63", s_value);



                //------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //SE 侧板标签
        public static bool wf_printlable55(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return;

            try
            {


                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");


                //data.LotNo = "FXX11081712345";
                //----------------------------------------------------------------------------------------------------------------
                i_return = B_Prn_Barcode(75 + data.X, 8 + data.Y, 0, "1", 4, 4, 71, Convert.ToChar(78), data.LotNo);
                i_return = B_Prn_Text_TrueType(240 + data.X, 75 + data.Y, 40, "Arial", 1, 500, 0, 0, 0, "A0", data.LotNo);

                //-----------------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(data.PrintQty);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }
        //SE 侧板标签
        public static bool wf_printlable58(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return;

            try
            {


                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");


                data.LotNo = data.LotNo;
                //----------------------------------------------------------------------------------------------------------------
                i_return = B_Prn_Barcode(75 + data.X, 8 + data.Y, 0, "1", 3, 3, 71, Convert.ToChar(78), data.LotNo);
                i_return = B_Prn_Text_TrueType(150 + data.X, 75 + data.Y, 40, "Arial", 1, 500, 0, 0, 0, "A0", data.LotNo);
                i_return = B_Prn_Text_TrueType_W(520 + data.X, 4 + data.Y, 90, 32, "Arial", 1, 500, 0, 0, 0, "A1", "P");
                //i_return = B_Prn_Barcode(40 + data.X, 8 + data.Y, 0, "1", 4, 4, 71, Convert.ToChar(78), data.LotNo);
                //i_return = B_Prn_Text_TrueType(205 + data.X, 75 + data.Y, 40, "Arial", 1, 500, 0, 0, 0, "A0", data.LotNo);
                //i_return = B_Prn_Text_TrueType_W(635 + data.X, 4 + data.Y, 90, 32, "Arial", 1, 500, 0, 0, 0, "A1", "P");
                //-----------------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(data.PrintQty);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }

        /// <summary>
        /// JapanSolar 铭牌打印
        /// </summary>
        /// <param name="data">组件对应的参数信息</param>
        /// <returns>返回打印结果（成功:True 失败:False）</returns>
        public static bool wf_printlable56(PrintLabelParameterData data)
        {
            bool b_result = false;
            int i_return, i_pnum, s_x, s_y;
            string s_resule, s_value, moduleType, moduleWeight = string.Empty, moduleDimension = string.Empty;


            #region 产品和边框、重量的对应关系

            string[] columns = new string[] { "ProductModule", "FullPalletQty", "ModuleWeight", "ModuleDimension" };
            DataTable dtModuleDismensionWeight = BaseData.Get(columns, "Basic_ModuleDimensionWeight");

            DataRow[] drs = dtModuleDismensionWeight.Select(string.Format("  ProductModule ='{0}' AND   FullPalletQty = '{1}'", data.ProductModel.Substring(0, 4), data.FullPalletQty));

            if (drs.Length == 1)
            {
                foreach (DataRow dr in drs)
                {
                    moduleWeight = Convert.ToString(dr["ModuleWeight"]);
                    moduleDimension = Convert.ToString(dr["ModuleDimension"]);
                }
            }
            else
            {
                data.ErrorMessage = data.ErrorMessage + string.Format("请工艺确认工单【{0}】中产品【{1}】的满托数是否正确！", data.WorkOrderNumber, data.ProductCode);
                return false;
            }

            #endregion

            #region ModuleType 获取

            moduleType = "JS"
                       + "-"
                       + data.PowersetStandardPM;
            if (data.ProductModel.Substring(4, 1) == "P")
            {
                moduleType = moduleType + "U";
            }
            else
            {
                moduleType = moduleType + "M";
            }

            moduleType = moduleType
                       + "-"
                       + "OI"
                       + data.LotCellQty;

            #endregion

            try
            {
                i_pnum = data.PrintQty;
                s_resule = AppDomain.CurrentDomain.BaseDirectory + @"Resource\";

                if (data.Darkness <= 0)
                {
                    data.Darkness = 12;
                }

                //打开打印接口
                i_return = B_CreatePrn(1, null);
                //设置打印浓度
                i_return = B_Set_Darkness(data.Darkness);
                //打印方向
                i_return = B_Set_Direction(Convert.ToChar(66));
                //清除内存图形
                i_return = B_Initial_Setting(0, "N\r\n\0");
                i_return = B_Del_Pcx("*");
                //-----------------------------------------------------------------------------------------------------
                //测量纸张像素点数 宽度：987 、高度：1208

                s_x = data.X - 20;
                s_y = data.Y;

                string filePath = string.Empty;

                filePath = s_resule + "JapanL.bmp";
                if (File.Exists(filePath))
                {
                    i_return = B_Get_Graphic_ColorBMP(s_x + 80, s_y + 55, filePath);
                }
                filePath = s_resule + "JapanE.bmp";
                if (File.Exists(filePath))
                {
                    i_return = B_Get_Graphic_ColorBMP(s_x + 80, s_y + 835, filePath);
                }
                filePath = s_resule + "JapanC.bmp";
                if (File.Exists(filePath))
                {
                    i_return = B_Get_Graphic_ColorBMP(s_x + 80, s_y + 975, filePath);
                }
                filePath = s_resule + "JapanD.bmp";
                if (File.Exists(filePath))
                {
                    i_return = B_Get_Graphic_ColorBMP(s_x + 270, s_y + 975, filePath);
                }
                filePath = s_resule + "JapanTh.bmp";
                if (File.Exists(filePath))
                {
                    i_return = B_Get_Graphic_ColorBMP(s_x + 830, s_y + 1130, filePath);
                }


                s_value = "PV Module";
                i_return = B_Prn_Text_TrueType(s_x + 710, s_y + 85, 80, "Arial", 1, 600, 0, 0, 0, "A1", s_value);


                s_value = "Module Type";
                i_return = B_Prn_Text_TrueType(s_x + 250, s_y + 200, 60, "Arial", 1, 500, 0, 0, 0, "A11", s_value);
                //moduleType = "JS-260U-OI60";
                s_value = moduleType;
                i_return = B_Prn_Text_TrueType(s_x + 590, s_y + 200, 60, "Arial", 1, 500, 0, 0, 0, "A12", s_value);

                s_value = "Maximum Power (PmPP)：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 306, 41, "Arial", 1, 400, 0, 0, 0, "A2", s_value);
                s_value = "260.0 W";
                s_value = decimal.Parse(data.PowersetStandardPM).ToString() + " W";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 306, 41, "Arial", 1, 400, 0, 0, 0, "A201", s_value);

                s_value = "Open Circuit Voltage (Voc)：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 355, 41, "Arial", 1, 400, 0, 0, 0, "A21", s_value);
                s_value = "38.53 V";
                s_value = data.PowersetStandardVOC.ToString() + " V";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 355, 41, "Arial", 1, 400, 0, 0, 0, "A211", s_value);

                s_value = "Short Circuit Current (Isc)：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 404, 41, "Arial", 1, 400, 0, 0, 0, "A22", s_value);
                s_value = "8.72 A";
                s_value = data.PowersetStandardISC.ToString() + " A";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 404, 41, "Arial", 1, 400, 0, 0, 0, "A221", s_value);

                s_value = "Operating Voltage (Vmpp)：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 453, 41, "Arial", 1, 400, 0, 0, 0, "A23", s_value);
                s_value = "31.05 V";
                s_value = data.PowersetStandardVPM.ToString() + " V";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 453, 41, "Arial", 1, 400, 0, 0, 0, "A231", s_value);

                s_value = "Operating Current (Impp)：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 502, 41, "Arial", 1, 400, 0, 0, 0, "A24", s_value);
                s_value = "8.39 A";
                s_value = data.PowersetStandardIPM.ToString() + " A";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 502, 41, "Arial", 1, 400, 0, 0, 0, "A241", s_value);

                s_value = "Module Weight：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 551, 41, "Arial", 1, 400, 0, 0, 0, "A25", s_value);
                s_value = "19.5 kg";
                s_value = string.Format("{0} kg", moduleWeight);
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 551, 41, "Arial", 1, 400, 0, 0, 0, "A251", s_value);

                s_value = "Fire Resistance Rating：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 600, 41, "Arial", 1, 400, 0, 0, 0, "A26", s_value);
                s_value = "Class C";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 600, 41, "Arial", 1, 400, 0, 0, 0, "A261", s_value);

                s_value = "Module Dimension(L×W×H)mm：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 649, 41, "Arial", 1, 400, 0, 0, 0, "A27", s_value);
                s_value = "1652×994×40";
                s_value = moduleDimension;
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 649, 41, "Arial", 1, 400, 0, 0, 0, "A271", s_value);

                s_value = "Tolerance of Pmax：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 698, 41, "Arial", 1, 400, 0, 0, 0, "A28", s_value);
                s_value = "+0~3%";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 698, 41, "Arial", 1, 400, 0, 0, 0, "A281", s_value);

                s_value = "Standard Test Condition：";
                i_return = B_Prn_Text_TrueType(s_x + 96, s_y + 747, 41, "Arial", 1, 400, 0, 0, 0, "A29", s_value);
                s_value = "1000W/㎡ , 25°C , AM1.5";
                i_return = B_Prn_Text_TrueType(s_x + 718, s_y + 747, 41, "Arial", 1, 400, 0, 0, 0, "A291", s_value);

                //表格

                i_return = B_Draw_Line('O', s_x + 64, s_y + 181, 1071, 4);
                i_return = B_Draw_Line('O', s_x + 64, s_y + 274, 1071, 4);
                i_return = B_Draw_Line('O', s_x + 64, s_y + 811, 1071, 4);
                i_return = B_Draw_Line('O', s_x + 64, s_y + 952, 1071, 4);
                i_return = B_Draw_Line('O', s_x + 64, s_y + 1098, 1071, 4);

                i_return = B_Draw_Line('O', s_x + 64, s_y + 181, 4, 917);
                i_return = B_Draw_Line('O', s_x + 1135, s_y + 181, 4, 917);


                s_value = "Hazadous Electricity Can Shock , Burn , or Cause Death Do not Touch";
                i_return = B_Prn_Text_TrueType(s_x + 250, s_y + 830, 32, "Arial", 1, 450, 0, 0, 0, "A3", s_value);
                s_value = "Terminals.";
                i_return = B_Prn_Text_TrueType(s_x + 250, s_y + 865, 32, "Arial", 1, 450, 0, 0, 0, "A31", s_value);
                s_value = "For field connections , use 12 AWG Wire insulated for minimum of 90℃";
                i_return = B_Prn_Text_TrueType(s_x + 250, s_y + 893, 32, "Arial", 1, 450, 0, 0, 0, "A32", s_value);

                s_value = "Produce in accordance with IEC61215,IEC61730.";
                i_return = B_Prn_Text_TrueType(s_x + 430, s_y + 1000, 34, "Arial", 1, 450, 0, 0, 0, "A4", s_value);
                s_value = "Max System Voltage 1000V:  Application class A";
                i_return = B_Prn_Text_TrueType(s_x + 430, s_y + 1030, 34, "Arial", 1, 450, 0, 0, 0, "A41", s_value);

                s_value = "INFINI JAPAN SOLAR CO., LTD.";
                i_return = B_Prn_Text_TrueType(s_x + 85, s_y + 1127, 41, "Arial", 1, 450, 0, 0, 0, "A5", s_value);

                s_value = string.Format("Serial Number : {0}", data.LotNo);
                i_return = B_Prn_Text_TrueType(s_x + 85, s_y + 1198, 38, "Arial", 1, 450, 0, 0, 0, "6", s_value);
                s_value = data.LotNo;
                i_return = B_Prn_Barcode(s_x + 125, s_y + 1252, 0, "1", 4, 4, 99, Convert.ToChar(78), s_value);

                //------------------------------------------------------------------------------------------------------
                //列印所有資料
                i_return = B_Print_Out(i_pnum);
                //关闭打印
                B_ClosePrn();

                b_result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b_result;
        }

    }
}