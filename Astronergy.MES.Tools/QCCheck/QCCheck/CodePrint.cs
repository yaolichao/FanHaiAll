using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;//DLL
using QCCheck.PrintUtility;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Threading;

namespace QCCheck
{
    public partial class CodePrint : Form
    {
        public CodePrint()
        {
            InitializeComponent();
        }


        #region
        //立像打印机DLL引用
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



        private void btn_CodePrint_Click(object sender, EventArgs e)
        {
            string sn = txt_Number.Text.Trim();

            PrintConergyLabel_A(sn, sn);
            //printSerialNo(sn);
        }



        private void PrintConergyLabel_A(string sn1, string sn2)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;


            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            //i_return = B_Set_Darkness(14);
            i_return = B_Set_Darkness(14);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));
            //B_Set_Direction('T');

            //清除内存图形
            i_return = B_Initial_Setting(0, "N\r\n\0");
            i_return = B_Del_Pcx("*");

            //i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn1);

            //i_return = B_Prn_Text_TrueType(80 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            //i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn2);

            //i_return = B_Prn_Text_TrueType(700 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            i_return = B_Prn_Barcode(5 + int.Parse(txt_X.Text.Trim()), 20 + int.Parse(txt_Y.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn1);
            i_return = B_Prn_Text_TrueType(80 + int.Parse(txt_X.Text.Trim()), 100 + int.Parse(txt_Y.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Text_TrueType(700 + int.Parse(txt_X.Text.Trim()), 20 + int.Parse(txt_Y.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A2", sn2);
            i_return = B_Prn_Barcode(640 + int.Parse(txt_X.Text.Trim()), 60 + int.Parse(txt_Y.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn2);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }


        public void printSerialNo(string sn)
        {
            int i_return;

            int width = int.Parse(txt_Number.Text.Trim());
            int height = int.Parse(txt_Number.Text.Trim());


            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            i_return = B_Set_Darkness(14);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));

            ////清除内存图形
            //i_return = B_Initial_Setting(0, "N\r\n\0");
            //i_return = B_Del_Pcx("*");




            i_return = B_Prn_Text_TrueType(140 + int.Parse(txt_X.Text.Trim()), 10 + int.Parse(txt_Y.Text.Trim()), 32, "Arial", 1, 500, 0, 0, 0, "A1", sn);
            i_return = B_Prn_Barcode(20 + int.Parse(txt_X.Text.Trim()), 40 + int.Parse(txt_Y.Text.Trim()), 0, "1", 3, 4, 90, Convert.ToChar(78), sn);



            /*--条码打印
            i_return = B_Prn_Barcode(20 + int.Parse(txt_X.Text.Trim()), 20 + int.Parse(txt_Y.Text.Trim()), 0, "1", 3, 8, 100, Convert.ToChar(78), sn);
            i_return = B_Prn_Text_TrueType(20 + int.Parse(txt_X.Text.Trim()), 120 + int.Parse(txt_Y.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", sn);
            */
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印


        }


        public void printSerialNo(string sn1 ,string sn2)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txt_Labqty.Text.Trim());
            nDarkness = int.Parse(txt_Darkness.Text.Trim());
            n_Narrow = 4;

            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            //i_return = B_Set_Darkness(14);
            i_return = B_Set_Darkness(nDarkness);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));
            //B_Set_Direction('T');

            //清除内存图形
            i_return = B_Initial_Setting(0, "N\r\n\0");
            i_return = B_Del_Pcx("*");

            i_return = B_Prn_Barcode(20 + int.Parse(txt_X.Text.Trim()), 20 + int.Parse(txt_Y.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), sn1);

            i_return = B_Prn_Text_TrueType(20 + int.Parse(txtString_X.Text.Trim()), 120 + int.Parse(txtString_Y.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Barcode(640 + int.Parse(txt_X.Text.Trim()), 20 + int.Parse(txt_Y.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), sn2);

            i_return = B_Prn_Text_TrueType(640 + int.Parse(txtString_X.Text.Trim()), 120 + int.Parse(txtString_Y.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }
    }
}
