using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;//DLL
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LabelManager2;
using Microsoft.Office.Interop.Excel;
using QCCheck.PrintUtility;
using ZHDSpace;
using System.Xml;

namespace QCCheck
{
    public partial class frmQCCheck : Form
    {
        private SqlDataAdapter da;
        private SqlConnection conn;
        BindingSource bsource = new BindingSource();
        DataSet ds = null;
        string serialno = string.Empty;
        DBUtility db = new DBUtility();

        string[] lotNumbers = null;

        int? sleepTime = null;

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

        private int BoxNo = 0;
        //private int lineNo = 0; //存放当前要打印行的行号 
        //private int lineQty = 0; //存放总共要打印的行数，可以是一个估算值，略大于实际行数
        private int printingPageNo = 0; //当前打印的页号

        public frmQCCheck()
        {
            InitializeComponent();
        }

        private void frmQCCheck_Load(object sender, EventArgs e)
        {
            this.tabSNPrint.Controls["tpgMemc"].Parent = null;
            this.tabSNPrint.Controls["tpOut"].Parent = null;
            this.tabSNPrint.Controls["tpPallet"].Parent = null;
            this.tabSNPrint.Controls["tpbQcell"].Parent = null;
            this.tabSNPrint.Controls["tpgConergy"].Parent = null;
            this.tabSNPrint.Controls["tpgSchueco"].Parent = null;
            this.tabSNPrint.Controls["tpgPSNByWo"].Parent = null;
            this.tabSNPrint.Controls["tpgLinkCSN"].Parent = null;
            this.tabSNPrint.Controls["tabCustSNPrint"].Parent = null;
            this.tabSNPrint.Controls["tpgCommon"].Parent = null;
            this.tabSNPrint.Controls["tabSunEdison"].Parent = null;
            this.tabSNPrint.Controls["tabJapanSolar"].Parent = null;
            //this.tabSNPrint.Controls[""].Parent = null; 
            string sVersion, sAppVersion, sql;
            InitListView();
            InitComboBox();
            //BindListView(); 2011-09-15 By Zhang Di
            //BindDataGrid();
            lblCount.Text = lvLeft.Items.Count.ToString();
            this.tabSNPrint.Controls.Remove(tpOut);
            this.tabSNPrint.Controls.Remove(tpPallet);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("Config.xml");
            DBUtility.connectionString = xmldoc.SelectSingleNode("//UI/DB_CON").InnerText.Trim();
            DBUtility.nSleepTime = int.Parse(xmldoc.SelectSingleNode("//UI/SLEEP_TIME").InnerText.Trim());

            sleepTime = int.Parse(xmldoc.SelectSingleNode("//UI/SLEEP_TIME").InnerText.Trim());

            //sVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //sql = "select version from bcp_app_version where app_name='SN-Print'";
            //DataSet dsVersion = db.Query(sql);
            //if (dsVersion.Tables[0].Rows.Count > 0)
            //{
            //    sAppVersion = dsVersion.Tables[0].Rows[0]["version"].ToString().Trim();
            //}
            //else
            //{
            //    sAppVersion = "";
            //}
            //if (sVersion != sAppVersion)
            //{
            //    MessageBox.Show("程序当前版本非最新[" + sAppVersion + "]版本，请更新程序！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    System.Windows.Forms.Application.Exit();
            //}
            //this.Text = this.Text + "[" + sVersion + "]";

            LogIn frmLogIn = new LogIn();
            frmLogIn.ShowDialog();
            frmLogIn.Dispose();

            if (DBUtility.sUserRight == "P")
            {
                this.tabSNPrint.Controls.Remove(tpgLinkCSN);
            }
            if (DBUtility.sUserRight == "U")
            {
                this.tabSNPrint.Controls.Remove(tpgMemc);
                this.tabSNPrint.Controls.Remove(tpbQcell);
                this.tabSNPrint.Controls.Remove(tpgCommon);
                this.tabSNPrint.Controls.Remove(tpgConergy);
                this.tabSNPrint.Controls.Remove(tpgSchueco);
                this.tabSNPrint.Controls.Remove(tpgPSNByWo);
                this.toolStripMenuItem8.Enabled = true;
            }
            if (DBUtility.sUserRight == "A")
            {
                this.toolStripMenuItem7.Enabled = true;
                this.toolStripMenuItem8.Enabled = true;
            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //打印内容
            string partNo, mtlName, mtlType, cellLevel, cellGrade, cellCur, cellUnit, shpCount, cellCount, shpDate, shpNo;
            int tmpCount = 0;
            float lr, ud = 0;
            lr = 20;

            string strLine;//用于存放当前行打印的信息　　　　　　
            string strTemp;
            float leftMargin = (e.MarginBounds.Left) * 3 / 4;　 //左边距
            float topMargin = e.MarginBounds.Top * 1 / 3;　　　 //顶边距
            float verticalPosition = topMargin;　　　　　　　　 //初始化垂直位置，设为顶边距
            System.Drawing.Font mainFont = new System.Drawing.Font("Courier New", 10);	//打印的字体
            //每页的行数，当打印行数超过这个时，要换页(1.05这个值是根据实际情况中设定的，可以不要)
            int linesPerPage = (int)(e.MarginBounds.Height * 1.05 / mainFont.GetHeight(e.Graphics));

            if (printingPageNo == 0) //打印第一页时，需要打印以下头信息
            {
                mainFont = new System.Drawing.Font("华文隶书", 22, FontStyle.Bold);
                strTemp = DateTime.Now.ToLongDateString().Substring(5, DateTime.Now.ToLongDateString().Length - 5);
                strLine = String.Format("{0,22}", strTemp + "发往上海正泰电池片清单");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, leftMargin + lr, verticalPosition + ud, new StringFormat());
                //正泰料号	物料名称	规格	档位	质量等级	电流	单位	发货数量
                verticalPosition = verticalPosition + mainFont.GetHeight(e.Graphics) * 2;
                mainFont = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
                strLine = String.Format("{0,4}", "正泰料号");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 10 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,5}", "物料名称");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 90 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,12}", "规格");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 200 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,6}", "档位");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 390 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,2}", "质量等级");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 490 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,2}", "电流");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 560 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,2}", "单位");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 630 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,4}", "发货数量");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 690 + lr, verticalPosition, new StringFormat());

                string strSQL = "Select * From PrintList Where ship_no = '" + cmbShipNo.Text.Trim().ToString() + "'";

                ZHDSpace.DBUtility queryData = new DBUtility();
                SqlDataReader dr = queryData.ExecuteReader(strSQL);

                while (dr.Read())
                {
                    partNo = dr["part_no"].ToString();              //61104001
                    mtlName = dr["material_name"].ToString();       //多晶156电池片
                    mtlType = dr["material_type"].ToString();       //156P三栅线自制 65根副栅线
                    cellLevel = dr["cell_level"].ToString();        //16.5---16.75
                    cellGrade = dr["cell_grade"].ToString();        //B1
                    cellCur = dr["cell_current"].ToString();        //A2
                    cellUnit = dr["cell_unit"].ToString();          //包
                    cellCount = dr["cell_count"].ToString();        //11520
                    tmpCount = tmpCount + Convert.ToInt32(cellCount);

                    verticalPosition = verticalPosition + mainFont.GetHeight(e.Graphics) * 2;
                    mainFont = new System.Drawing.Font("宋体", 9, FontStyle.Bold);
                    strLine = String.Format("{0,4}", partNo);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 10 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,4}", mtlName);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 90 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,2}", mtlType);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 200 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,2}", cellLevel);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 390 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,5}", cellGrade);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 490 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,3}", cellCur);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 560 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,2}", cellUnit);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 630 + lr, verticalPosition, new StringFormat());
                    strLine = String.Format("{0,2}", cellCount);
                    e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 700 + lr, verticalPosition, new StringFormat());
                }

                shpCount = tmpCount.ToString();
                verticalPosition = verticalPosition + mainFont.GetHeight(e.Graphics) * 2;
                strLine = String.Format("{0,4}", "合计：");
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 10 + lr, verticalPosition, new StringFormat());
                strLine = String.Format("{0,2}", shpCount);
                e.Graphics.DrawString(strLine, mainFont, Brushes.Black, 690 + lr, verticalPosition, new StringFormat());
            }
        }

        //private void btnPrintA4_Click(object sender, EventArgs e)
        //{
        //    PrintDocument printDocument = new PrintDocument();

        //    printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        //    //printDocument.Print();

        //    PrintPreviewDialog ppd = new PrintPreviewDialog();
        //    ppd.Document = printDocument;
        //    ppd.ShowDialog();
        //}

        private void rbtnM_CheckedChanged(object sender, EventArgs e)
        {
            cmbCurrent.Items.Clear();
            cmbPower.Items.Clear();

            if (rbtnM.Checked)
            {
                cmbCurrent.Text = "H";
                cmbCurrent.Items.Add("H");
                cmbCurrent.Items.Add("L");
                cmbCurrent.Items.Add("N");
            }
            else
            {
                cmbCurrent.Text = "A1";
                cmbCurrent.Items.Add("A1");
                cmbCurrent.Items.Add("A2");
                cmbCurrent.Items.Add("A3");
                cmbCurrent.Items.Add("B1");
                cmbCurrent.Items.Add("B2");
                cmbCurrent.Items.Add("B3");
                cmbCurrent.Items.Add("C1");
                cmbCurrent.Items.Add("C2");
                cmbCurrent.Items.Add("C3");
                cmbCurrent.Items.Add("C4");
                cmbCurrent.Items.Add("D1");
                cmbCurrent.Items.Add("D2");
                cmbCurrent.Items.Add("D3");
                cmbCurrent.Items.Add("D4");
                cmbCurrent.Items.Add("E1");
                cmbCurrent.Items.Add("E2");
                cmbCurrent.Items.Add("E3");
                cmbCurrent.Items.Add("F1");
                cmbCurrent.Items.Add("F2");
                cmbCurrent.Items.Add("G1");
                cmbCurrent.Items.Add("G2");
                cmbCurrent.Items.Add("H");
            }

            if (rbtnM.Checked)
            {
                cmbPower.Items.Add("18.25 ~ 18.50");
                cmbPower.Items.Add("18.00 ~ 18.25");
                cmbPower.Items.Add("17.75 ~ 18.00");
                cmbPower.Items.Add("17.50 ~ 17.75");
                cmbPower.Items.Add("17.25 ~ 17.50");
                cmbPower.Items.Add("17.00 ~ 17.25");
                cmbPower.Items.Add("16.75 ~ 17.00");
                cmbPower.Items.Add("16.50 ~ 16.75");
                cmbPower.Items.Add("16.25 ~ 16.50");
                cmbPower.Items.Add("16.00 ~ 16.25");
            }
            else
            {
                cmbPower.Items.Add("17.00 ~ 00.00");
                cmbPower.Items.Add("17.00 ~ 17.25");
                cmbPower.Items.Add("16.75 ~ 17.00");
                cmbPower.Items.Add("16.50 ~ 16.75");
                cmbPower.Items.Add("16.25 ~ 16.50");
                cmbPower.Items.Add("16.00 ~ 16.25");
                cmbPower.Items.Add("15.75 ~ 16.00");
                cmbPower.Items.Add("15.50 ~ 15.75");
                cmbPower.Items.Add("15.25 ~ 15.50");
            }

            cmbPower.Text = cmbPower.Items[0].ToString();
        }

        private void printPower(string contentToPrint)
        {
            int i_return;

            if (cmbPrint.Text == "Arogx")
            {
                //打开打印接口
                i_return = B_CreatePrn(1, null);

                //设置打印浓度
                i_return = B_Set_Darkness(12);

                //打印方向
                B_Set_Direction(Convert.ToChar(66));

                //创建条码
                i_return = B_Prn_Barcode(10, 10, 0, "1", 2, 2, 70, Convert.ToChar(66), contentToPrint);
                //4,5为控制条码宽度大小，70为条码高度

                //列印所有資料
                i_return = B_Print_Out(1);

                //关闭打印
                B_ClosePrn();
            }
            else if (cmbPrint.Text == "Zebra")
            {

                string strPrint = "";

                StreamReader objReader = new StreamReader(System.Windows.Forms.Application.StartupPath + @"\Print.txt");
                string strLine = "";

                while (strLine != null)
                {
                    strLine = objReader.ReadLine();
                    if (strLine != "")
                        strPrint = strPrint + strLine;
                }

                strPrint = strPrint.Replace("PrintContent", contentToPrint);

                PrintFactory prtFun = new PrintFactory();
                prtFun.sendTextToLPT1(strPrint);
            }
        }

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string prnContent = "";
                prnContent = txtBarCode.Text.Trim().ToString().ToUpper();
                printPower(prnContent);
                txtBarCode.SelectAll();
            }
        }

        private void btnCodePrint_Click(object sender, EventArgs e)
        {
            printPower(txtBarCode.Text.Trim().ToString().ToUpper());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BoxNo = 0;
            txtEnter.Text = "";
            lvCheckBox.BackColor = System.Drawing.Color.White;
            lvCheckBox.Items.Clear();
            lblPM.Text = "";
            lblPF.Text = "";
            lblPT.Text = "";
            lblCurrent.Text = "";
            lblBoxID.Text = "";
            txtEnter.Select();
        }

        private void InitListView()
        {
            //lvCheckBox初始化
            lvCheckBox.GridLines = true;
            lvCheckBox.FullRowSelect = true;
            lvCheckBox.View = View.Details;
            lvCheckBox.Scrollable = true;
            lvCheckBox.MultiSelect = false;
            lvCheckBox.CheckBoxes = true;
            lvCheckBox.AllowColumnReorder = true;
            lvCheckBox.Dock = System.Windows.Forms.DockStyle.None;
            lvCheckBox.ForeColor = System.Drawing.Color.Blue;
            lvCheckBox.View = System.Windows.Forms.View.Details;
            lvCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            lvCheckBox.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            lvCheckBox.Columns.Add("序号", 102, HorizontalAlignment.Right);
            lvCheckBox.Columns.Add("晶体", 60, HorizontalAlignment.Left);
            lvCheckBox.Columns.Add("电流", 100, HorizontalAlignment.Left);
            lvCheckBox.Columns.Add("效率>", 110, HorizontalAlignment.Left);
            lvCheckBox.Columns.Add("效率<", 110, HorizontalAlignment.Left);

            //
            lvLeft.GridLines = true;
            lvLeft.FullRowSelect = true;
            lvLeft.View = View.Details;
            lvLeft.Scrollable = true;
            lvLeft.MultiSelect = false;
            lvLeft.CheckBoxes = true;
            lvLeft.AllowColumnReorder = true;
            lvLeft.Dock = System.Windows.Forms.DockStyle.None;
            lvLeft.ForeColor = System.Drawing.Color.Blue;
            lvLeft.View = System.Windows.Forms.View.Details;
            lvLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            //lvLeft.Size = new System.Drawing.Size(600,300);
            lvLeft.HeaderStyle = ColumnHeaderStyle.Nonclickable;


            lvLeft.Columns.Add("序列号", 70, HorizontalAlignment.Center);
            lvLeft.Columns.Add("箱号", 150, HorizontalAlignment.Center);
            lvLeft.Columns.Add("属性", 200, HorizontalAlignment.Left);
            lvLeft.Columns.Add("检测标志", 120, HorizontalAlignment.Left);
        }

        private void InitComboBox()
        {
            cmbType.Items.Add("A");
            cmbType.Items.Add("B");
            cmbType.Items.Add("B1");
            cmbType.Text = "A";
            cmbLine.Items.Add("两删线");
            cmbLine.Items.Add("三删线");
            cmbLine.Text = "两删线";
            if (rbtnM.Checked)
            {
                cmbCurrent.Text = "H";
                cmbCurrent.Items.Add("H");
                cmbCurrent.Items.Add("L");
                cmbCurrent.Items.Add("N");
            }
            else
            {
                cmbCurrent.Text = "A1";
                cmbCurrent.Items.Add("A1");
                cmbCurrent.Items.Add("A2");
                cmbCurrent.Items.Add("A3");
                cmbCurrent.Items.Add("B1");
                cmbCurrent.Items.Add("B2");
                cmbCurrent.Items.Add("B3");
                cmbCurrent.Items.Add("C1");
                cmbCurrent.Items.Add("C2");
                cmbCurrent.Items.Add("C3");
                cmbCurrent.Items.Add("C4");
                cmbCurrent.Items.Add("D1");
                cmbCurrent.Items.Add("D2");
                cmbCurrent.Items.Add("D3");
                cmbCurrent.Items.Add("D4");
                cmbCurrent.Items.Add("E1");
                cmbCurrent.Items.Add("E2");
                cmbCurrent.Items.Add("E3");
                cmbCurrent.Items.Add("F1");
                cmbCurrent.Items.Add("F2");
                cmbCurrent.Items.Add("G1");
                cmbCurrent.Items.Add("G2");
                cmbCurrent.Items.Add("H");
            }

            cmbStyle.Items.Add("125");
            cmbStyle.Items.Add("156");
            cmbStyle.Text = "125";

            if (rbtnM.Checked)
            {
                cmbPower.Items.Add("18.25 ~ 18.50");
                cmbPower.Items.Add("18.00 ~ 18.25");
                cmbPower.Items.Add("17.75 ~ 18.00");
                cmbPower.Items.Add("17.50 ~ 17.75");
                cmbPower.Items.Add("17.25 ~ 17.50");
                cmbPower.Items.Add("17.00 ~ 17.25");
                cmbPower.Items.Add("16.75 ~ 17.00");
                cmbPower.Items.Add("16.50 ~ 16.75");
                cmbPower.Items.Add("16.25 ~ 16.50");
                cmbPower.Items.Add("16.00 ~ 16.25");
            }
            else
            {
                cmbPower.Items.Add("17.00 ~ 00.00");
                cmbPower.Items.Add("17.00 ~ 17.25");
                cmbPower.Items.Add("16.75 ~ 17.00");
                cmbPower.Items.Add("16.50 ~ 16.75");
                cmbPower.Items.Add("16.25 ~ 16.50");
                cmbPower.Items.Add("16.00 ~ 16.25");
                cmbPower.Items.Add("15.75 ~ 16.00");
                cmbPower.Items.Add("15.50 ~ 15.75");
                cmbPower.Items.Add("15.25 ~ 15.50");
            }
            cmbPower.Text = cmbPower.Items[0].ToString();

            cmbPrint.Items.Add("Zebra");
            cmbPrint.Items.Add("Arogx");
            cmbPrint.Text = "Arogx";

            cmbNo.Items.Add("08");
            cmbNo.Items.Add("09");
            cmbNo.Items.Add("10");
            cmbNo.Items.Add("11");
            cmbNo.Items.Add("12");
            cmbNo.Items.Add("13");
            cmbNo.Items.Add("14");
            cmbNo.Items.Add("15");
            cmbNo.Items.Add("16");
            cmbNo.Items.Add("17");
            cmbNo.Items.Add("18");
            cmbNo.Items.Add("19");
            cmbNo.Items.Add("20");
            cmbNo.Items.Add("21");
            cmbNo.Items.Add("22");
            cmbNo.Items.Add("23");
            cmbNo.Items.Add("24");
            cmbNo.Items.Add("25");
            cmbNo.Text = "12";
            cmbNo.Enabled = false;
            cmbQty.Items.Add("1");
            cmbQty.Items.Add("2");
            cmbQty.Items.Add("3");
            cmbQty.Items.Add("4");
            cmbQty.Items.Add("5");
            cmbQty.Items.Add("6");
            cmbQty.Items.Add("7");
            cmbQty.Items.Add("8");
            cmbQty.Items.Add("9");
            cmbQty.Items.Add("10");
            cmbQty.Items.Add("11");
            cmbQty.Items.Add("12");
            cmbQty.Items.Add("13");
            cmbQty.Items.Add("14");
            cmbQty.Items.Add("15");
            cmbQty.Items.Add("16");
            cmbQty.Items.Add("17");
            cmbQty.Items.Add("18");
            cmbQty.Items.Add("19");
            cmbQty.Items.Add("20");
            cmbQty.Items.Add("21");
            cmbQty.Items.Add("22");
            cmbQty.Text = "1";

            cmbAB.Items.Add("A");
            cmbAB.Items.Add("B");
            cmbAB.Items.Add("B1");
            cmbAB.Text = "A";
            cmbAB.Enabled = false;

            cmbPartNo.Items.Add("61104001");
            cmbPartNo.Text = "61104001";
            //initShipNo(); 2011-09-15 By Zhang Di

            //线别
            cmbLineNo.Items.Add("1 线");
            cmbLineNo.Items.Add("2 线");
            cmbLineNo.Items.Add("3 线");
            cmbLineNo.Items.Add("4 线");
            cmbLineNo.Items.Add("5 线");
            cmbLineNo.Items.Add("6 线");
            cmbLineNo.Items.Add("7 线");
            cmbLineNo.Items.Add("8 线");
            cmbLineNo.Items.Add("9 线");
            cmbLineNo.Items.Add("10线");
            cmbLineNo.Items.Add("11线");
            cmbLineNo.Items.Add("12线");
            cmbLineNo.Items.Add("13线");
            cmbLineNo.Items.Add("14线");
            cmbLineNo.Items.Add("15线");
            cmbLineNo.Items.Add("16线");
            cmbLineNo.Items.Add("17线");
            cmbLineNo.Items.Add("18线");
            cmbLineNo.Text = "1 线";

            //白夜班别
            cmbShift.Items.Add("白班");
            cmbShift.Items.Add("夜班");
            cmbShift.Text = "白班";

            //车间
            cmbFab.Items.Add("电池A车间");
            cmbFab.Items.Add("电池B车间");
            cmbFab.Items.Add("电池C车间");
            cmbFab.Text = "电池A车间";

            //打印数量
            cmbPrintCount.Items.Add("1");
            cmbPrintCount.Items.Add("2");
            cmbPrintCount.Items.Add("3");
            cmbPrintCount.Items.Add("4");
            cmbPrintCount.Text = "1";

            cmbFabID.Items.Add("5厂7号车间");
            cmbFabID.Items.Add("5厂9号车间");

        }

        public void BindListView()
        {
            lvLeft.Items.Clear();
            string strSQL = "Select * From QCWarehouse.dbo.PrintBox Where ship_no = '" + cmbShipNo.Text + "' ";
            ZHDSpace.DBUtility queryData = new DBUtility();
            SqlDataReader dr = queryData.ExecuteReader(strSQL);

            int i = 0;
            while (dr.Read())
            {
                ListViewItem lvItem = new ListViewItem();
                ListViewItem.ListViewSubItem lvSubItem1 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem lvSubItem2 = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem lvSubItem3 = new ListViewItem.ListViewSubItem();

                lvItem.Text = (++i).ToString();
                lvSubItem1.Text = dr["box_id"].ToString();
                lvSubItem2.Text = dr["box_style"].ToString();
                lvSubItem3.Text = dr["check_flag"].ToString();
                lvItem.SubItems.Add(lvSubItem1);
                lvItem.SubItems.Add(lvSubItem2);
                lvItem.SubItems.Add(lvSubItem3);
                lvLeft.Items.Add(lvItem);
            }
        }

        public void BindDataGrid()
        {
            conn = new SqlConnection("user id=fly;password=sky;initial catalog=QCWarehouse;data source=10.20.30.25");
            string strQuerySQL = "Select part_no as 正泰料号, material_name as 物料名称, material_type as 规格, cell_level as 档位, cell_grade as 质量等级, cell_current as 电流, cell_unit as 单位, cell_count as 发货数量 " +
                "From QCWarehouse.dbo.PrintList Where ship_no = '" + cmbShipNo.Text + "'";
            da = new SqlDataAdapter(strQuerySQL, conn);
            ds = new DataSet();
            da.Fill(ds, "PrintList");
            dgvShipList.DataSource = ds.Tables["PrintList"];
        }

        public void initShipNo()
        {
            cmbShipNo.Items.Clear();
            string strSQL = "Select * From ShipNoList Where ship_date > '" + DateTime.Now.ToShortDateString() + " 00:00:00.000" + "' And ship_date < '" + DateTime.Now.ToShortDateString() + " 23:59:59.000" + "'";
            ZHDSpace.DBUtility queryData = new DBUtility();
            SqlDataReader dr = queryData.ExecuteReader(strSQL);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    cmbShipNo.Items.Add(dr["ship_no"].ToString());
                    cmbShipNo.Text = dr["ship_no"].ToString();
                }
            }
        }

        private void txtEnter_TextChanged(object sender, EventArgs e)
        {
            txtEnter.Text = txtEnter.Text.ToUpper();
        }

        private void txtEnter_KeyDown(object sender, KeyEventArgs e)
        {
            int MaxNum = Convert.ToInt32(cmbNo.Text.ToString());
            lvCheckBox.BackColor = System.Drawing.Color.White;
            string boxStyle;
            string strSQL = "";

            if (e.KeyCode == Keys.Enter)
            {
                if (txtEnter.Text == "")
                {
                    MessageBox.Show("请扫描标签条码！");
                    return;
                }

                if (lblPM.Text == "" && (txtEnter.Text.Trim().Substring(0, 1).ToUpper() == "H"))
                {
                    ZHDSpace.DBUtility queryData = new DBUtility();
                    strSQL = "Select * From QCWarehouse.dbo.BoxInfor Where box_id='" + txtEnter.Text.Trim().ToString() + "'";
                    SqlDataReader dr = queryData.ExecuteReader(strSQL);

                    if (dr.Read())
                    {
                        boxStyle = dr["box_style"].ToString();

                        lblBoxID.Text = txtEnter.Text.Trim().ToString();
                        lblPM.Text = boxStyle.Substring(0, 4);
                        lblPF.Text = boxStyle.Substring(4, 4);
                        lblPT.Text = boxStyle.Substring(8, 4);
                        if (boxStyle.Substring(13, 1) == "0")
                            lblCurrent.Text = boxStyle.Substring(12, 1);
                        else
                            lblCurrent.Text = boxStyle.Substring(12, 2);
                    }

                }
                else
                {
                    if (txtEnter.Text.Trim().Length != 17)
                    {
                        MessageBox.Show("请检测效率标签条码长度！");
                        return;
                    }

                    if (Convert.ToInt32(txtEnter.Text.Substring(4, 4).ToUpper()) >= Convert.ToInt32(txtEnter.Text.Substring(8, 4).ToUpper()))
                    {
                        MessageBox.Show("该箱效率>小于效率<！请重新打印！");
                        return;
                    }

                    if (lblPM.Text != txtEnter.Text.Substring(0, 4).ToUpper())
                    {
                        MessageBox.Show("该箱电池片的产品与外箱不一致！");
                        return;
                    }

                    string tmpCmp;

                    if (lblCurrent.Text.Length == 1)
                    {
                        tmpCmp = txtEnter.Text.Substring(12, 1).ToUpper();
                    }
                    else
                    {
                        tmpCmp = txtEnter.Text.Substring(12, 2).ToUpper();
                    }

                    if (lblCurrent.Text != tmpCmp)
                    {
                        MessageBox.Show("该箱电池片的电流与外箱不一致！");
                        return;
                    }

                    if (lblPF.Text != txtEnter.Text.Substring(4, 4).ToUpper() || lblPT.Text != txtEnter.Text.Substring(8, 4).ToUpper())
                    {
                        MessageBox.Show("内外箱效率不匹配！");
                        return;
                    }

                    #region
                    if (lblPT.Text == "0000")
                    {
                        if (Convert.ToInt32(txtEnter.Text.Substring(4, 4)) > Convert.ToInt32(lblPF.Text))
                        {
                            if (BoxNo >= MaxNum)
                            {
                                MessageBox.Show("已装满，请确认！");
                            }
                            else
                            {
                                ListViewItem lvItem = new ListViewItem();
                                ListViewItem.ListViewSubItem lvSubItem1 = new ListViewItem.ListViewSubItem();
                                ListViewItem.ListViewSubItem lvSubItem2 = new ListViewItem.ListViewSubItem();
                                ListViewItem.ListViewSubItem lvSubItem3 = new ListViewItem.ListViewSubItem();
                                ListViewItem.ListViewSubItem lvSubItem4 = new ListViewItem.ListViewSubItem();

                                lvItem.Text = txtEnter.Text;
                                lvSubItem1.Text = txtEnter.Text.Substring(3, 1);
                                if (txtEnter.Text.Substring(13, 1) == "0")
                                    lvSubItem2.Text = txtEnter.Text.Substring(12, 1);
                                else
                                    lvSubItem2.Text = txtEnter.Text.Substring(12, 2);
                                lvSubItem3.Text = txtEnter.Text.Substring(4, 4);
                                lvSubItem4.Text = txtEnter.Text.Substring(8, 4);
                                lvItem.SubItems.Add(lvSubItem1);
                                lvItem.SubItems.Add(lvSubItem2);
                                lvItem.SubItems.Add(lvSubItem3);
                                lvItem.SubItems.Add(lvSubItem4);
                                lvCheckBox.Items.Add(lvItem);

                                BoxNo++;

                                if (BoxNo == Convert.ToInt32(cmbNo.Text))
                                {
                                    strSQL = "Update QCWarehouse.dbo.BoxInfor Set check_flag = 'T' Where box_id = '" + lblBoxID.Text.Trim().ToString() + "'";
                                    ZHDSpace.DBUtility sqlAction = new DBUtility();
                                    int returnValue = sqlAction.ExecuteSql(strSQL);
                                    if (returnValue == 1)
                                    {
                                        dosomething();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("效率<小于外箱最低效率！");
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(txtEnter.Text.Substring(4, 4)) < Convert.ToInt32(lblPF.Text) || Convert.ToInt32(txtEnter.Text.Substring(8, 4)) > Convert.ToInt32(lblPT.Text))
                        {
                            MessageBox.Show("效率不在外箱效率范围内！");
                        }
                        else
                        {
                            if (BoxNo >= MaxNum)
                            {
                                MessageBox.Show("已装满，请确认！");
                            }
                            else
                            {
                                ListViewItem lvItem = new ListViewItem();
                                ListViewItem.ListViewSubItem lvSubItem1 = new ListViewItem.ListViewSubItem();
                                ListViewItem.ListViewSubItem lvSubItem2 = new ListViewItem.ListViewSubItem();
                                ListViewItem.ListViewSubItem lvSubItem3 = new ListViewItem.ListViewSubItem();
                                ListViewItem.ListViewSubItem lvSubItem4 = new ListViewItem.ListViewSubItem();

                                lvItem.Text = txtEnter.Text;
                                lvSubItem1.Text = txtEnter.Text.Substring(3, 1);
                                if (txtEnter.Text.Substring(13, 1) == "0")
                                    lvSubItem2.Text = txtEnter.Text.Substring(12, 1);
                                else
                                    lvSubItem2.Text = txtEnter.Text.Substring(12, 2);
                                lvSubItem3.Text = txtEnter.Text.Substring(4, 4);
                                lvSubItem4.Text = txtEnter.Text.Substring(8, 4);
                                lvItem.SubItems.Add(lvSubItem1);
                                lvItem.SubItems.Add(lvSubItem2);
                                lvItem.SubItems.Add(lvSubItem3);
                                lvItem.SubItems.Add(lvSubItem4);
                                lvCheckBox.Items.Add(lvItem);

                                BoxNo++;

                                if (BoxNo == Convert.ToInt32(cmbNo.Text))
                                {
                                    strSQL = "Update QCWarehouse.dbo.BoxInfor Set check_flag = 'T' Where box_id = '" + lblBoxID.Text.Trim().ToString() + "'";
                                    ZHDSpace.DBUtility sqlAction = new DBUtility();
                                    int returnValue = sqlAction.ExecuteSql(strSQL);
                                    if (returnValue == 1)
                                    {
                                        dosomething();
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                txtEnter.Text = "";
            }
        }

        public void dosomething()
        {
            lvCheckBox.BackColor = System.Drawing.Color.Green;
            MessageBox.Show("已经检测完毕！");
        }

        public void checkBoxID()
        {
            string strDate;
            int countR;

            ZHDSpace.DBUtility execSQL = new DBUtility();

            for (int i = 0; i < Convert.ToInt32(cmbQty.Text); i++)
            {

                strDate = DateTime.Now.ToShortDateString() + " 00:00:00";
                string strSQL = "Select count(*) From QCWarehouse.dbo.BoxInfor Where create_time > '" + strDate + "'";
                countR = Convert.ToInt32(execSQL.GetSingle(strSQL));

                string boxStyle, crystalType, boxID, powerFrom, powerTo, currentValue, gradeValue, cellCut;

                powerFrom = cmbPower.Text.Substring(0, 2) + cmbPower.Text.Substring(3, 2);
                powerTo = cmbPower.Text.Substring(8, 2) + cmbPower.Text.Substring(11, 2);

                if (cmbCurrent.Text.Length > 1)
                {
                    currentValue = cmbCurrent.Text.ToString();
                }
                else
                {
                    currentValue = cmbCurrent.Text.ToString() + "0";
                }
                if (cmbType.Text.Length > 1)
                {
                    gradeValue = cmbType.Text.ToString();
                }
                else
                {
                    gradeValue = cmbType.Text.ToString() + "0";
                }

                if (rbtnM.Checked == true)
                    crystalType = "M";
                else
                    crystalType = "P";

                if (cmbLine.Text == "两删线")
                    cellCut = "2";
                else
                    cellCut = "3";

                boxStyle = cmbStyle.Text.Trim().ToString() + crystalType + powerFrom + powerTo + currentValue + gradeValue + cellCut;
                boxStyle = boxStyle.ToUpper();

                if (countR > 0)
                {
                    string temp = string.Format("{0:0000}", ++countR);
                    boxID = "H" + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + temp;
                }
                else
                {
                    countR++;
                    boxID = "H" + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "0001";
                }

                strSQL = "Insert Into QCWarehouse.dbo.BoxInfor(box_id,serial_no,box_style,create_time,cell_type,cell_cut) Values('" + boxID + "'," + countR.ToString() + ",'" + boxStyle + "','" + DateTime.Now.ToString() + "','" + cmbType.Text.ToString() + "','" + cmbLine.Text.ToString() + "')";

                int affectCnt = execSQL.ExecuteSql(strSQL);

                if (affectCnt > 0)
                {
                    boxPrint(boxID, boxStyle, crystalType);
                }
            }
        }

        public void boxPrint(string boxID, string boxStyle, string crystalTypes)
        {
            int i_return;
            string s_value;
            //string crystalType;

            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            i_return = B_Set_Darkness(14);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));

            /*合格品*/
            s_value = "合格品" + cmbType.Text.ToString() + "类";
            i_return = B_Prn_Text_TrueType(50, 10, 40, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

            /*型号*/
            s_value = "型号：";
            i_return = B_Prn_Text_TrueType(10, 50, 35, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

            /*型号值*/
            s_value = cmbStyle.Text.Trim().ToString() + crystalTypes + cmbLine.Text.ToString();
            i_return = B_Prn_Text_TrueType(100, 57, 25, "Arial", 1, 400, 0, 0, 0, "A8", s_value);

            /*效率*/
            s_value = "效率：";
            i_return = B_Prn_Text_TrueType(10, 90, 35, "Arial", 1, 400, 0, 0, 0, "A3", s_value);

            /*效率值*/
            s_value = cmbPower.Text.ToString() + " " + cmbCurrent.Text.ToString();
            i_return = B_Prn_Text_TrueType(100, 97, 25, "Arial", 1, 400, 0, 0, 0, "A9", s_value);

            /*数量*/
            s_value = "数量：";
            i_return = B_Prn_Text_TrueType(10, 130, 35, "Arial", 1, 400, 0, 0, 0, "A4", s_value);

            /*数量值*/
            s_value = txtCellCount.Text.Trim().ToString();
            i_return = B_Prn_Text_TrueType(100, 137, 25, "Arial", 1, 400, 0, 0, 0, "A10", s_value);

            //创建条码
            i_return = B_Prn_Barcode(30, 190, 0, "1", 2, 2, 30, Convert.ToChar(66), boxID);
            //4,5为控制条码宽度大小，70为条码高度

            /*箱子属性数值*/
            i_return = B_Prn_Text_TrueType(30, 260, 32, "Arial", 1, 400, 0, 0, 0, "A5", boxStyle); ;

            /*浙江正泰太阳能有限公司*/
            s_value = "浙江正泰太阳能有限公司";
            i_return = B_Prn_Text_TrueType(20, 300, 25, "Arial", 1, 400, 0, 0, 0, "A6", s_value); ;

            /*Chint solar(zhejiang) Co.,Ltd*/
            s_value = "Chint solar(zhejiang) Co.,Ltd";
            i_return = B_Prn_Text_TrueType(20, 330, 23, "Arial", 1, 400, 0, 0, 0, "A7", s_value);


            ////创建条码
            //i_return = B_Prn_Barcode(10, 40, 0, "1", 2, 2, 30, Convert.ToChar(66), boxID);
            ////4,5为控制条码宽度大小，70为条码高度

            ///*箱子属性数值*/
            //i_return = B_Prn_Text_TrueType(10, 100,25, "Arial", 1,500, 0, 0, 0, "A1", boxStyle); ;

            //列印所有資料
            i_return = B_Print_Out(1);

            //关闭打印
            B_ClosePrn();
        }

        private void btnPrn_Click(object sender, EventArgs e)
        {
            checkBoxID();
        }

        private void txtBoxID_KeyDown(object sender, KeyEventArgs e)
        {
            bool rnValue;

            if (e.KeyCode == Keys.Enter)
            {
                if (cmbShipNo.Text == "")
                {
                    ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                    MessageBox.Show("请选择出货单号");
                    return;
                }

                rnValue = doStatisticsCheck();
                if (rnValue)
                {
                    rnValue = doBoxCheck();
                    if (rnValue)
                    {
                        BindListView();
                        BindDataGrid();
                        ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\pass.wav", 1);
                    }
                }

                txtBoxID.SelectAll();
                txtBoxID.Text = "";
            }
        }

        public bool doStatisticsCheck()
        {
            string strSQL = "Select count(*) From QCWarehouse.dbo.PrintBox Where box_id = '" + txtBoxID.Text.Trim().ToUpper().ToString() + "'";
            ZHDSpace.DBUtility queryData = new DBUtility();
            int rtnValue = Convert.ToInt32(queryData.GetSingle(strSQL));
            if (rtnValue > 0)
            {
                ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                MessageBox.Show("此箱号已经做过统计");
                return false;
            }
            else
                return true;
        }

        public bool doBoxCheck()
        {
            bool rtnExitBox = chkExitBox();
            if (rtnExitBox)
            {
                rtnExitBox = chkConsistent();
                if (rtnExitBox)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool chkExitBox()
        {
            string strSQL = "";
            if (chkAB.Checked == true)
            {
                strSQL = "Select count(*) From QCWarehouse.dbo.BoxInfor Where box_id = '" + txtBoxID.Text.Trim().ToString().ToUpper()
                    + "' And cell_type = '" + cmbAB.Text.ToString() + "'";
            }
            else
            {
                strSQL = "Select count(*) From QCWarehouse.dbo.BoxInfor Where box_id = '" + txtBoxID.Text.Trim().ToString().ToUpper() + "'";
            }
            ZHDSpace.DBUtility queryData = new DBUtility();
            int rtnValue = Convert.ToInt32(queryData.GetSingle(strSQL));
            if (rtnValue == 1)
                return true;
            else
            {
                ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                MessageBox.Show("没有此箱号，请重新检测");
                return false;
            }
        }

        public bool chkConsistent()
        {
            string strSQL;
            if (chkAB.Checked == true)
            {
                strSQL = "Select * From QCWarehouse.dbo.BoxInfor Where box_id = '" + txtBoxID.Text.Trim().ToString().ToUpper()
                    + "' And cell_type = '" + cmbAB.Text.ToString() + "'";
            }
            else
            {
                strSQL = "Select * From QCWarehouse.dbo.BoxInfor Where box_id = '" + txtBoxID.Text.Trim().ToString().ToUpper() + "'";
            }
            ZHDSpace.DBUtility queryData = new DBUtility();
            SqlDataReader dr = queryData.ExecuteReader(strSQL);
            if (dr.Read())
            {
                //如果此箱号做过内外箱匹配检测，则执行下面内容
                //if (dr["check_flag"].ToString() == "T")
                if (true)
                {
                    strSQL = "Select * From QCWarehouse.dbo.PrintList Where box_style = '" + dr["box_style"].ToString() + "'";
                    SqlDataReader sr = queryData.ExecuteReader(strSQL);
                    if (sr.HasRows)
                    {
                        sr.Read();
                        int tmpCnt;
                        tmpCnt = (Convert.ToInt32(sr["cell_count"]) / 12 + 1) * 12;
                        strSQL = "Update QCWarehouse.dbo.PrintList Set cell_count = " + tmpCnt.ToString() + "  Where box_style = '" + dr["box_style"].ToString() + "' And ship_no = '" + cmbShipNo.Text.Trim().ToString() + "' ";
                    }
                    else
                    {
                        //125M17501775B2A0
                        string checkString = dr["box_style"].ToString();
                        string strPartNo, strMtlName, strType, strLevel, strGrade, strCurrent, strCut, strUnit, strCellCnt, strBoxStyle;
                        strPartNo = cmbPartNo.Text.ToString();

                        if (checkString.Substring(3, 1) == "M")
                        {
                            strMtlName = "多晶" + checkString.Substring(0, 3) + "电池片";
                        }
                        else
                        {
                            strMtlName = "单晶" + checkString.Substring(0, 3) + "电池片";
                        }
                        if (checkString.Substring(16, 1) == "2")
                            strCut = "两删线";
                        else
                            strCut = "三删线";

                        strType = checkString.Substring(0, 4) + strCut + "自制 65根副删线";

                        if (checkString.Substring(4, 4) == "0000")
                        {
                            strLevel = "< " + Convert.ToDouble(checkString.Substring(8, 2) + "." + checkString.Substring(10, 2)).ToString();
                        }
                        else if (checkString.Substring(8, 4) == "0000")
                        {
                            strLevel = "> " + Convert.ToDouble(checkString.Substring(4, 2) + "." + checkString.Substring(6, 2)).ToString();
                        }
                        else
                        {
                            strLevel = Convert.ToDouble(checkString.Substring(4, 2) + "." + checkString.Substring(6, 2)).ToString() + "---"
                                + Convert.ToDouble(checkString.Substring(8, 2) + "." + checkString.Substring(10, 2)).ToString();
                        }
                        if (checkString.Substring(15, 1) == "0")
                        {
                            strGrade = checkString.Substring(14, 1);
                        }
                        else
                        {
                            strGrade = checkString.Substring(14, 2);
                        }
                        if (checkString.Substring(13, 1) == "0")
                        {
                            strCurrent = checkString.Substring(12, 1);
                        }
                        else
                        {
                            strCurrent = checkString.Substring(12, 2);
                        }
                        strUnit = "包";
                        strCellCnt = "12";
                        strBoxStyle = dr["box_style"].ToString();

                        string strDate = DateTime.Now.ToShortTimeString();
                        strSQL = "Insert Into QCWarehouse.dbo.PrintList(part_no,material_name,material_type,cell_level,cell_grade,cell_current,cell_unit,cell_count,ship_date,box_style,ship_no) "
                        + " Values('" + strPartNo + "','" + strMtlName + "','" + strType + "','" + strLevel + "','" + strGrade + "','" + strCurrent + "','" + strUnit + "','" + strCellCnt + "','" + strDate + "','" + strBoxStyle + "','" + cmbShipNo.Text + "') ";
                    }
                    int iReturn = queryData.ExecuteSql(strSQL);
                    //新增一笔箱号信息到PrintBox表
                    strSQL = "Insert Into QCWarehouse.dbo.PrintBox(box_id,box_style,check_flag,ship_no) Values('" + dr["box_id"] + "','" + dr["box_style"] + "','" + dr["check_flag"] + "','" + cmbShipNo.Text.Trim().ToString() + "')";

                    if (iReturn > 0)
                    {
                        int rtnExecCnt = queryData.ExecuteSql(strSQL);
                        if (rtnExecCnt > 0)
                            return true;
                        else
                        {
                            ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                            MessageBox.Show("数据保存失败！");
                            return false;
                        }
                    }
                    else
                    {
                        ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                        MessageBox.Show("数据保存失败！");
                        return false;
                    }

                }
                else
                {
                    ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                    MessageBox.Show("没有检测内外箱一致！");
                    return false;
                }
            }
            else
            {
                ZHDSpace.PlaySound.Sound.Play(System.Windows.Forms.Application.StartupPath + @"\NoteSounds\err.wav", 1);
                MessageBox.Show("没有找到CheckFlag信息");
                return false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            lvCheckBox.Items[0].SubItems[2].Text = "testdddd";
        }

        private void btnClnAll_Click(object sender, EventArgs e)
        {
            lblCount.Text = lvLeft.Items.Count.ToString();
            txtBoxID.Text = "";

            lvLeft.Items.Clear();
        }

        private void chkAB_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAB.Checked == true)
                cmbAB.Enabled = true;
            else
                cmbAB.Enabled = false;
        }

        private void btnPntShip_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();

            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            //printDocument.Print();

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = printDocument;
            ppd.ShowDialog();
        }

        private void btnAddShipNo_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmShipNo frmAdd = new frmShipNo();
            frmAdd.ShowDialog();
            initShipNo();
            this.Show();
            //BindDataGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to delete this row?", "Delete confirmation",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void dtpShip_ValueChanged(object sender, EventArgs e)
        {
            ZHDSpace.DBUtility queryData = new DBUtility();
            string strSQL = "Select * From ShipNoList Where ship_date > '" + dtpShip.Value.ToShortDateString() + " 00:00:00" + "' And ship_date < '" + dtpShip.Value.ToShortDateString() + " 23:59:59" + "'";
            SqlDataReader dr = queryData.ExecuteReader(strSQL);
            cmbShipNo.Items.Clear();
            cmbShipNo.Text = "";
            if (dr.HasRows)
                while (dr.Read())
                {
                    cmbShipNo.Items.Add(dr["ship_no"]);
                    cmbShipNo.Text = dr["ship_no"].ToString();
                }
            else
            {
                BindListView();
                BindDataGrid();
                lblCount.Text = lvLeft.Items.Count.ToString();
            }
        }

        private void cmbShipNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindListView();
            BindDataGrid();
            lblCount.Text = lvLeft.Items.Count.ToString();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //建立Excel对象
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);
            //生成字段名称
            for (int i = 0; i < dgvShipList.ColumnCount; i++)
            {
                excel.Cells[1, i + 1] = dgvShipList.Columns[i].HeaderText;
            }

            for (int i = 0; i < dgvShipList.RowCount - 1; i++)
            {
                for (int j = 0; j < dgvShipList.ColumnCount; j++)
                {
                    //string ss = dgvShipList[j, i].Value.ToString();
                    //if (dgvShipList[j, i].Value == typeof(string))
                    //    excel.Cells[i + 2, j + 1] = "" + dgvShipList[i, j].Value.ToString();
                    //else
                    excel.Cells[i + 2, j + 1] = dgvShipList[j, i].Value.ToString();
                }

            }
            excel.Visible = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindListView();
            BindDataGrid();
            lblCount.Text = lvLeft.Items.Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZHDSpace.PlaySound.Sound.Play(@"E:\Escape_Success.wav", 2);
        }

        private void btnPrintVendor_Click(object sender, EventArgs e)
        {
            if (txtVendor.Text == "" || cmbLineNo.Text == "" || cmbShift.Text == "" || cmbFab.Text == "" || txtDeliverNo.Text == "" || cmbPrintCount.Text == "")
            {
                MessageBox.Show("信息不全，请填写完整！", "Waining", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string printContent = string.Empty;
                string printMtrlCode = string.Empty;
                string curDate = string.Empty;
                string curShift = string.Empty;

                if (cmbShift.Text == "白班")
                {
                    curShift = "D";
                }
                else
                {
                    curShift = "N";
                }

                printContent = txtPrintRandom.Text.Trim();

                curDate = string.Format("{0:00}", int.Parse(dtpDate.Value.Month.ToString())) + string.Format("{0:00}", int.Parse(dtpDate.Value.Day.ToString()));

                printContent = cmbFab.Text.Substring(2, 1) + cmbLineNo.Text.Substring(0, 2).Trim() + txtVendor.Text + curDate + curShift + txtDeliverNo.Text.Trim();
                printMtrlCode = "物料码" + txtMtrl.Text.Trim() + txtName.Text.Trim() + "-" + cmbFab.Text.Trim().Substring(2, 1) + "栋";

                for (int i = 0; i < int.Parse(cmbPrintCount.Text); i++)
                {
                    printVendor(printMtrlCode, printContent);
                }
            }
        }

        public void printVendor(string matreialCode, string printContent)
        {
            int i_return;
            string s_value;
            int ix = int.Parse(txtX.Text.Trim()), iy = int.Parse(txtY.Text.Trim());
            if (txtX.Text == "" || txtY.Text == "")
            {
                ix = 0; iy = 0;
            }

            //Basic Settings
            i_return = B_CreatePrn(1, null);//打开打印接口
            i_return = B_Set_Darkness(12);//设置打印浓度
            B_Set_Direction('B');//打印方向

            s_value = matreialCode;
            i_return = B_Prn_Text_TrueType(220 + ix, 80 + iy, 30, "Arial", 2, 500, 0, 0, 0, "A1", s_value);

            //i_return = B_Prn_Barcode(210, 40, 1, "1", 2, 2, 90, Convert.ToChar(78), printContent);
            //i_return = B_Prn_Text_TrueType(90, 120, 26, "Arial", 2, 500, 0, 0, 0, "A2", printContent);
            i_return = B_Prn_Barcode(210 + ix, 110 + iy, 1, "1", 1, 2, 90, Convert.ToChar(78), printContent);
            i_return = B_Prn_Text_TrueType(100 + ix, 110 + iy, 22, "Arial", 2, 500, 0, 0, 0, "A2", printContent);

            i_return = B_Print_Out(1);//列印所有資料

            B_ClosePrn();//关闭打印
        }

        private void txtX_TextChanged(object sender, EventArgs e)
        {
            if (txtX.Text.Trim() == "")
            {
                MessageBox.Show("坐标值不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtX.Text = "0";
            }
        }

        private void txtMEMC_KeyDown(object sender, KeyEventArgs e)
        {
            string deliverNo = string.Empty;
            string strSQL = string.Empty;
            string packcode = string.Empty;
            string tdate = string.Empty;
            string leftSN = string.Empty;
            string sCust = string.Empty;
            int partNo, ExecCnt;

            if (e.KeyCode == Keys.Enter)
            {
                txtMEMC.Text = txtMEMC.Text.Trim().ToUpper();
                if (txtMEMC.Text.Trim() != "")
                {
                    DBUtility dbu = new DBUtility();
                    deliverNo = txtMEMC.Text.Substring(txtMEMC.Text.Trim().Length - 8, 8);
                    sCust = txtMEMC.Text.Substring(txtMEMC.Text.Trim().Length - 1, 1);
                    packcode = txtMEMC.Text.Trim();
                    strSQL = "Select * From MEMCCODE Where DeliverNo='" + deliverNo + "'";
                    SqlDataReader dr = dbu.ExecuteReader(strSQL);
                    if (dr.Read())
                    {
                        if (cmbFabID.Text.Trim() == "")
                        {
                            MessageBox.Show("请输入车间！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        strSQL = "SELECT SUBSTRING(CONVERT(VARCHAR(8),GETDATE(),112),3,6) as TDATE";
                        DataSet dsTdate = dbu.Query(strSQL);
                        if (dsTdate.Tables[0].Rows.Count < 1)
                        {
                            MessageBox.Show("获取日期失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            tdate = dsTdate.Tables[0].Rows[0]["TDATE"].ToString().Trim();
                        }

                        if (cmbFabID.Text.Trim() == "5厂7号车间")
                        {

                            leftSN = "C" + sCust + tdate + "7";
                        }
                        else if (cmbFabID.Text.Trim() == "5厂9号车间")
                        {
                            leftSN = "C" + sCust + tdate + "9";
                        }
                        else
                        {
                            MessageBox.Show("请输入车间！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        strSQL = "select * from memclot where serialno like '" + leftSN + "%' order by part desc";
                        DataSet ds = dbu.Query(strSQL);

                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            partNo = 1;
                        }
                        else
                        {
                            partNo = int.Parse(ds.Tables[0].Rows[0]["part"].ToString().Trim());
                            partNo = partNo + 1;
                        }

                        if (partNo > 9999)
                        {
                            MessageBox.Show("DeliverNo：" + deliverNo + "\n流水号超出最大限制，请联系IT！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        serialno = leftSN + string.Format("{0:0000}", partNo);

                        strSQL = "Insert Into MEMCLOT(SERIALNO,PART,PACKCODE,DELIVERNO,TDATE) Values('" + serialno + "'," + partNo.ToString() + ",'" + packcode + "','" + deliverNo + "','" + tdate + "')";
                        ExecCnt = dbu.ExecuteSql(strSQL);
                        if (ExecCnt > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(serialno);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(serialno);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    printSerialNo(serialno);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有此Deliverary No", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                txtMEMC.Text = "";
            }

        }

        public void printSerialNo(string sn)
        {
            int i_return;
            string s_value;

            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            i_return = B_Set_Darkness(14);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));
            //B_Set_Direction('T');

            s_value = sn;
            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 40 + int.Parse(txtY.Text.Trim()), 0, "1", 2, 2, 60, Convert.ToChar(78), s_value);

            i_return = B_Prn_Text_TrueType(20 + int.Parse(txtX.Text.Trim()), 100 + int.Parse(txtY.Text.Trim()), 32, "Arial", 1, 400, 0, 0, 0, "A2", s_value);

            i_return = B_Print_Out(1);//列印所有資料

            B_ClosePrn();//关闭打印
        }

        private void btnPPrint_Click(object sender, EventArgs e)
        {
            if (serialno != null || serialno != "")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno(serialno);
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno(serialno);
                }
                else if (rbtCommon.Checked == true)
                {
                    printSerialNo(serialno);
                }


                txtMEMC.Text = "";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string sDate, sPlant, sBomver, sYear, sWeek, sDay, sDigit, sql, sSN, sNumber;
            int nTotday, ExecCnt, nPart;
            sPlant = "";
            sBomver = "";
            sNumber = "";
            sDay = "";
            sDigit = "";
            nPart = 0;

            DBUtility dbu = new DBUtility();
            for (int i = 0; i < int.Parse(txtPrtqty.Text.Trim()); i++)
            {
                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("BOM版本号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.SelectAll();
                    textBox1.Focus();
                    return;
                }
                if (textBox1.Text.Trim().Length != 4)
                {
                    MessageBox.Show("BOM版本号长度只能为4！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.SelectAll();
                    textBox1.Focus();
                    return;
                }
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("请选择厂别！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.Now < DateTime.Parse(sDate + " 08:00:00"))
                {
                    sDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
                sYear = DateTime.Parse(sDate).Year.ToString();
                //sql = "select week,days from bcp_year_week where year='" + sYear + "' and  start_day<=" + sDate + "' and end_day>='" + sDate + "'";
                //DataSet oDS = dbu.Query(sql);
                //if (oDS.Tables[0].Rows.Count < 1)
                //{
                //    MessageBox.Show("无周别资料，请联系IT！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                //    return;
                //}
                //sWeek = oDS.Tables[0].Rows[0]["week"].ToString();
                //nTotday = int.Parse(oDS.Tables[0].Rows[0]["days"].ToString());
                //if (nTotday < 3)
                //{
                //    sWeek = (int.Parse(sWeek) - 1).ToString("0#");
                //}
                nTotday = DateTime.Parse(sDate).DayOfYear;
                sWeek = ((nTotday - 2) / 7 + 1).ToString("0#").Trim();
                string strday = DateTime.Parse(sDate).DayOfWeek.ToString().ToLower();
                switch (strday)
                {
                    case "monday":
                        sDay = "1";
                        break;
                    case "tuesday":
                        sDay = "2";
                        break;
                    case "wednesday":
                        sDay = "3";
                        break;
                    case "thursday":
                        sDay = "4";
                        break;
                    case "friday":
                        sDay = "5";
                        break;
                    case "saturday":
                        sDay = "6";
                        break;
                    case "sunday":
                        sDay = "7";
                        break;
                }
                sBomver = textBox1.Text.Trim();
                if (comboBox1.Text == "7号车间")
                {
                    sPlant = "7";
                }
                else if (comboBox1.Text == "9号车间")
                {
                    sPlant = "9";
                }
                else
                {
                }

                //sql = "select part  from MEMCLOT where PACKCODE='" + sPlant + "' and DELIVERNO='" + sBomver + "' and TDATE='" + sDate + "'";
                sql = "select isnull(max(part),0) part from MEMCLOT where PACKCODE='" + sPlant + "' and TDATE='" + sDate + "'";
                DataSet dsSerial = dbu.Query(sql);
                if (dsSerial.Tables[0].Rows.Count > 0)
                {
                    sNumber = dsSerial.Tables[0].Rows[0]["part"].ToString();
                    if (sNumber != "0")
                    {
                        sNumber = (int.Parse(sNumber) + 1).ToString();
                    }
                    else
                    {
                        if (comboBox1.Text == "7号车间")
                        {
                            sNumber = "1";
                        }
                        else if (comboBox1.Text == "9号车间")
                        {
                            sNumber = "50001";
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    if (comboBox1.Text == "7号车间")
                    {
                        sNumber = "1";
                    }
                    else if (comboBox1.Text == "9号车间")
                    {
                        sNumber = "50001";
                    }
                    else
                    {
                    }
                }
                nPart = int.Parse(sNumber);
                sNumber = int.Parse(sNumber).ToString("0000#");
                sSN = "740X" + sYear.Substring(2, 2) + sWeek + sDay + sBomver + sNumber;
                sDigit = ((10 - ((int.Parse(sSN.Substring(0, 1)) + int.Parse(sSN.Substring(2, 1)) + int.Parse(sSN.Substring(4, 1)) + int.Parse(sSN.Substring(6, 1)) + int.Parse(sSN.Substring(8, 1)) + int.Parse(sSN.Substring(10, 1)) + int.Parse(sSN.Substring(12, 1)) + int.Parse(sSN.Substring(14, 1)) + int.Parse(sSN.Substring(16, 1)) + 3 * (int.Parse(sSN.Substring(1, 1)) + int.Parse(sSN.Substring(5, 1)) + int.Parse(sSN.Substring(7, 1)) + int.Parse(sSN.Substring(9, 1)) + int.Parse(sSN.Substring(11, 1)) + int.Parse(sSN.Substring(13, 1)) + int.Parse(sSN.Substring(15, 1)) + int.Parse(sSN.Substring(17, 1)))) % 10)) % 10).ToString();
                sSN = "740" + sDigit + sYear.Substring(2, 2) + sWeek + sDay + sBomver + sNumber;

                sql = "Insert Into MEMCLOT(SERIALNO,PART,PACKCODE,DELIVERNO,TDATE) Values('" + sSN + "'," + nPart + ",'" + sPlant + "','" + sBomver + "','" + sDate + "')";
                ExecCnt = dbu.ExecuteSql(sql);
                if (ExecCnt > 0)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno(sSN);
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno(sSN);
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        printQcellSerialNo(sSN);
                    }


                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnPrint_Click(sender, e);
            }
        }

        public void printQcellSerialNo(string sn)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            s_value = sn;
            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", 5, 6, 50, Convert.ToChar(78), s_value);

            i_return = B_Prn_Text_TrueType(100 + int.Parse(txtX.Text.Trim()), 70 + int.Parse(txtY.Text.Trim()), 52, "Arial", 1, 500, 0, 0, 0, "A2", s_value);

            i_return = B_Print_Out(i_labqty);//列印所有資料

            B_ClosePrn();//关闭打印
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            if (txtResn.Text.Trim() == "")
            {
                MessageBox.Show("补印标签不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.SelectAll();
                textBox1.Focus();
                return;
            }
            string sql = "select * from MEMCLOT where serialno='" + txtResn.Text.Trim() + "'";
            DBUtility dbu = new DBUtility();
            DataSet dsSerialno = dbu.Query(sql);
            if (dsSerialno.Tables[0].Rows.Count < 1)
            {
                MessageBox.Show("序列号不存在，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.SelectAll();
                textBox1.Focus();
                return;
            }

            if (rbtAgox.Checked == true)
            {
                PrintSerialno(txtResn.Text.Trim());
            }
            else if (rbtZero.Checked == true)
            {
                PrintSerialno(txtResn.Text.Trim());
            }
            else if (rbtCommon.Checked == true)
            {
                printQcellSerialNo(txtResn.Text.Trim());
            }

        }

        private void txtResn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnReprint_Click(sender, e);
                txtResn.SelectAll();
                txtResn.Focus();
            }
        }

        private void btnComPrint_Click(object sender, EventArgs e)
        {
            btnComPrint.Enabled = false;

            string sql, sDate, sWO, sType, sLabelType, sSN1, sSN2, sSN3, sProductType;
            int nQty, nYear, nMonth, nNum, nResult;

            //测试
            //PrintCommonLabel_C("3404430212900001", "3404430212900002", "3404430212900003");
            //return;

            if (txtComWO.Text.Trim() == "")
            {
                MessageBox.Show("工单号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComWO.SelectAll();
                txtComWO.Focus();
                return;
            }
            if (txtComType.Text.Trim() == "")
            {
                MessageBox.Show("产品型号代码不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComType.SelectAll();
                txtComType.Focus();
                return;
            }
            try
            {
                nQty = int.Parse(txtComQty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComQty.SelectAll();
                txtComQty.Focus();
                return;
            }
            if (cboComLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sLabelType = cboComLT.Text.Trim();

            #region//调用设定的编码规则生成SN
            sWO = txtComWO.Text.Trim();
            sType = txtComType.Text.Trim();
            nYear = 0;
            nMonth = 0;
            if (txtComYear.Text.Trim() != "")
            {
                nYear = int.Parse(txtComYear.Text.Trim());
            }
            if (txtComMonth.Text.Trim() != "")
            {
                nMonth = int.Parse(txtComMonth.Text.Trim());
            }
            PrintSNByCustomer("common", sWO, "", sType, nYear, nMonth, 0, nQty, sLabelType);
            #endregion

            btnComPrint.Enabled = true;
        }



        private void PrintSunEdison_S(string serialNumber)
        {
            int i_return, nDarkness, n_Narrow;
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 3;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(125 + int.Parse(txtX.Text.Trim()), 8 + int.Parse(txtStringY.Text.Trim()), 0, "1", n_Narrow, 4, 71, Convert.ToChar(78), serialNumber);
            i_return = B_Prn_Text_TrueType(205 + int.Parse(txtX.Text.Trim()), 75 + int.Parse(txtStringY.Text.Trim()), 40, "Arial", 1, 500, 0, 0, 0, "A", serialNumber);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintCommonLabel_S(string serialNumber)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(66 + int.Parse(txtX.Text.Trim()), 15 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), serialNumber);

            i_return = B_Prn_Text_TrueType(66 + int.Parse(txtStringX.Text.Trim()), 115 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", serialNumber);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }


        /// <summary>
        /// 批量进行双排的打印
        /// </summary>
        /// <param name="lotNumbers">需要打印的条码的集合</param>
        private void PrintCommonLabel_A(string[] lotNumbers)
        {
            int printed = 0;
            int i_return, i_labqty, nDarkness, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            while (printed < (lotNumbers.Length / 2))
            {
                i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), lotNumbers[printed * 2]);

                i_return = B_Prn_Text_TrueType(20 + int.Parse(txtStringX.Text.Trim()), 120 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", lotNumbers[printed * 2]);

                i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), lotNumbers[printed * 2 + 1]);

                i_return = B_Prn_Text_TrueType(640 + int.Parse(txtStringX.Text.Trim()), 120 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A2", lotNumbers[printed * 2 + 1]);

                i_return = B_Print_Out(1);

                printed += 1;
            }

            B_ClosePrn();//关闭打印
        }



        /// <summary>
        /// 立像条码打印
        /// </summary>
        /// <param name="sn1"></param>
        /// <param name="sn2"></param>
        private void PrintCommonLabel_A(string sn1, string sn2)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), sn1);

            i_return = B_Prn_Text_TrueType(20 + int.Parse(txtStringX.Text.Trim()), 120 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), sn2);

            i_return = B_Prn_Text_TrueType(640 + int.Parse(txtStringX.Text.Trim()), 120 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintCommonLabel_B(string sn1, string sn2, string sn3)
        {
            int i_return, i_labqty, nDarkness, nSpeed, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            nSpeed = int.Parse(txtSpeed.Text.Trim());
            n_Narrow = 2;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            //i_return = B_Set_Darkness(14);
            i_return = B_Set_Darkness(nDarkness);

            //i_return = B_Set_Speed(nSpeed);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));
            //B_Set_Direction('T');

            //清除内存图形
            i_return = B_Initial_Setting(0, "N\r\n\0");
            i_return = B_Del_Pcx("*");

            i_return = B_Prn_Barcode(40 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 80, Convert.ToChar(78), sn1);

            i_return = B_Prn_Text_TrueType(20 + int.Parse(txtStringX.Text.Trim()), 110 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Barcode(40 + 380 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 80, Convert.ToChar(78), sn2);

            i_return = B_Prn_Text_TrueType(20 + 380 + int.Parse(txtStringX.Text.Trim()), 110 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            i_return = B_Prn_Barcode(40 + 760 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 80, Convert.ToChar(78), sn3);

            i_return = B_Prn_Text_TrueType(20 + 760 + int.Parse(txtStringX.Text.Trim()), 110 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A3", sn3);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintCommonLabel_C(string sn1, string sn2, string sn3)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 2;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(40 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 50, Convert.ToChar(78), sn1);

            i_return = B_Prn_Text_TrueType(20 + int.Parse(txtStringX.Text.Trim()), 110 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Barcode(40 + 380 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 50, Convert.ToChar(78), sn2);

            i_return = B_Prn_Text_TrueType(20 + 380 + int.Parse(txtStringX.Text.Trim()), 110 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            i_return = B_Prn_Barcode(40 + 760 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 50, Convert.ToChar(78), sn3);

            i_return = B_Prn_Text_TrueType(20 + 760 + int.Parse(txtStringX.Text.Trim()), 110 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A3", sn3);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintConergyLabel_S(string sn1)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            //i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn1);

            //i_return = B_Prn_Text_TrueType(80 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            //i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn2);

            //i_return = B_Prn_Text_TrueType(700 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            i_return = B_Prn_Text_TrueType(156 + int.Parse(txtX.Text.Trim()), 18 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A1", sn1);
            i_return = B_Prn_Barcode(85 + int.Parse(txtX.Text.Trim()), 58 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn1);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintConergyLabel_A(string sn1, string sn2)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            //i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn1);

            //i_return = B_Prn_Text_TrueType(80 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            //i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn2);

            //i_return = B_Prn_Text_TrueType(700 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            i_return = B_Prn_Text_TrueType(80 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A1", sn1);
            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn1);
            i_return = B_Prn_Text_TrueType(700 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 46, "Arial", 1, 500, 0, 0, 0, "A2", sn2);
            i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 4, 8, 70, Convert.ToChar(78), sn2);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintConergyLabel_B(string sn1, string sn2, string sn3)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            //i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 80, Convert.ToChar(78), sn1);

            //i_return = B_Prn_Text_TrueType(60 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 36, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            //i_return = B_Prn_Barcode(20 + 380 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 80, Convert.ToChar(78), sn2);

            //i_return = B_Prn_Text_TrueType(60 + 380 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 36, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            //i_return = B_Prn_Barcode(20 + 760 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 80, Convert.ToChar(78), sn3);

            //i_return = B_Prn_Text_TrueType(60 + 760 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 36, "Arial", 1, 500, 0, 0, 0, "A3", sn3);

            i_return = B_Prn_Text_TrueType(60 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 36, "Arial", 1, 500, 0, 0, 0, "A1", sn1);
            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 80, Convert.ToChar(78), sn1);
            i_return = B_Prn_Text_TrueType(60 + 380 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 36, "Arial", 1, 500, 0, 0, 0, "A2", sn2);
            i_return = B_Prn_Barcode(20 + 380 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 80, Convert.ToChar(78), sn2);
            i_return = B_Prn_Text_TrueType(60 + 760 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 36, "Arial", 1, 500, 0, 0, 0, "A3", sn3);
            i_return = B_Prn_Barcode(20 + 760 + int.Parse(txtX.Text.Trim()), 60 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 80, Convert.ToChar(78), sn3);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintSchuecoLabel_A(string sn1)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            s_value = sn1;

            //i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 40 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 90, Convert.ToChar(78), s_value);

            //i_return = B_Prn_Text_TrueType(140 + int.Parse(txtX.Text.Trim()), 10 + int.Parse(txtY.Text.Trim()), 32, "Arial", 1, 500, 0, 0, 0, "A1", s_value);

            i_return = B_Prn_Text_TrueType(140 + int.Parse(txtX.Text.Trim()), 10 + int.Parse(txtY.Text.Trim()), 32, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 40 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 90, Convert.ToChar(78), s_value);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintSchuecoLabel_B(string sn1)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            //i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 45 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 100, Convert.ToChar(78), sn1);

            //i_return = B_Prn_Text_TrueType(140 + int.Parse(txtX.Text.Trim()), 10 + int.Parse(txtY.Text.Trim()), 34, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Text_TrueType(140 + int.Parse(txtX.Text.Trim()), 10 + int.Parse(txtY.Text.Trim()), 34, "Arial", 1, 500, 0, 0, 0, "A1", sn1);
            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 45 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 100, Convert.ToChar(78), sn1);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void btnComReprint_Click(object sender, EventArgs e)
        {
            string sql;
            int nResult, nLabelQty;
            nLabelQty = 0;

            if (txtComSN.Text.Trim() == "" || txtComSN2.Text.Trim() == "")
            {
                MessageBox.Show("组件序列号为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComSN.SelectAll();
                txtComSN.Focus();
                return;
            }
            if (cboComRLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboComRLT.Text.Trim() == "单排")
            {
                sql = "select sn from sn_print where label_type='S' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty == 0)
                    {
                        MessageBox.Show("补打标签数量不能为0，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 1)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='S' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);


                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_S(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboComRLT.Text.Trim() == "双排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty % 2 != 0)
                    {
                        MessageBox.Show("补打标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lotNumbers = new string[nLabelQty];

                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 2)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        lotNumbers[i] = dsReprint.Tables[0].Rows[i]["sn"].ToString();
                        lotNumbers[i + 1] = dsReprint.Tables[0].Rows[i + 1]["sn"].ToString();


                        //if (rbtAgox.Checked == true)
                        //{
                        //    PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        //}
                        //else if (rbtZero.Checked == true)
                        //{
                        //    PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        //}
                        //else if (rbtCommon.Checked == true)
                        //{
                        //    PrintCommonLabel_A(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        //}
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }

                    PrintCommonLabel_A(lotNumbers);
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboComRLT.Text.Trim() == "三排")
            {
                sql = "select sn from sn_print where label_type='B' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty % 3 != 0)
                    {
                        MessageBox.Show("补打标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboComRLT.Text.Trim() == "三排无BARCODE")
            {
                sql = "select sn from sn_print where label_type='C' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty % 2 != 0)
                    {
                        MessageBox.Show("补打标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }


                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
        }

        private void txtComSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnComReprint_Click(sender, e);
                txtComSN.SelectAll();
                txtComSN.Focus();
            }
        }

        private void btnConPrint_Click(object sender, EventArgs e)
        {
            int nQty, nSNstart, nSNend, nNownum, nResult;
            string sql, sPID, sPtype, sLabelType, sDate, sSN1, sSN2, sSN3;
            string sYear, sWeek;
            int nYear, nWeek;

            sYear = "";
            sWeek = "";
            nYear = 0;
            nWeek = 0;
            //测试
            //PrintConergyLabel_A("0120348801", "0120348802");
            //return;

            if (txtConPID.Text.Trim() == "")
            {
                MessageBox.Show("产品ID不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConPID.SelectAll();
                txtConPID.Focus();
                return;
            }
            if (txtConType.Text.Trim() == "")
            {
                MessageBox.Show("产品型号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConType.SelectAll();
                txtConType.Focus();
                return;
            }
            try
            {
                nQty = int.Parse(txtConQty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConQty.SelectAll();
                txtConQty.Focus();
                return;
            }
            if (cboConLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sPID = txtConPID.Text.Trim();
            sPtype = txtConType.Text.Trim();
            sLabelType = cboConLT.Text.Trim();
            sDate = DateTime.Now.ToString("yyyy-MM-dd");
            #region
            if (txtConYear.Text.Trim() != "")
            {
                nYear = int.Parse(txtConYear.Text.Trim());
            }
            if (txtConWeek.Text.Trim() != "")
            {
                nWeek = int.Parse(txtConWeek.Text.Trim());
            }
            PrintSNByCustomer("conergy", "", sPID, sPtype, nYear, 0, nWeek, nQty, sLabelType);
            #endregion

            ////sql = "select * from sn_print_set where product_id='" + sPID + "' and product_type='" + sPtype + "'";
            ////DataSet dsSNset = db.Query(sql);
            ////if (dsSNset.Tables[0].Rows.Count > 0)
            ////{
            ////    nSNstart = int.Parse(dsSNset.Tables[0].Rows[0]["start_num"].ToString().Trim());
            ////    nSNend = int.Parse(dsSNset.Tables[0].Rows[0]["end_num"].ToString().Trim());
            ////}
            ////else
            ////{
            ////    MessageBox.Show("产品对应的组件序号范围未设定，请联系工艺！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ////    return;
            ////}

            //sql = "select getdate() as 'now_time',datepart(year,getdate()) as 'now_year',datepart(month,getdate()) as 'now_month'";
            //sql += ",datepart(week,getdate()) as 'now_week',datepart(day,getdate()) as 'now_day'";
            //DataSet dsDate = db.Query(sql);
            //if (dsDate.Tables[0].Rows.Count > 0)
            //{
            //    sYear = dsDate.Tables[0].Rows[0]["now_year"].ToString().Trim();
            //    sYear = sYear.Substring(2, 2);
            //    if (txtConYear.Text.Trim() != "")
            //    {
            //        sYear = (int.Parse(sYear) + int.Parse(txtConYear.Text.Trim())).ToString("0#");
            //        nYear = int.Parse(txtConYear.Text.Trim());
            //    }
            //    sWeek = dsDate.Tables[0].Rows[0]["now_week"].ToString().Trim();
            //    sWeek = int.Parse(sWeek).ToString("0#");
            //    if (txtConWeek.Text.Trim() != "")
            //    {
            //        sWeek = (int.Parse(sWeek) + int.Parse(txtConWeek.Text.Trim())).ToString("0#");
            //    }
            //    if (nYear > 0)
            //    {
            //        sWeek = "01";
            //    }
            //    sDate = Convert.ToDateTime(dsDate.Tables[0].Rows[0]["now_time"].ToString()).ToString("yyyy-MM-dd");
            //}

            //if (sLabelType == "双排")
            //{
            //    if (nQty % 2 != 0)
            //    {
            //        MessageBox.Show("打印标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        txtConQty.SelectAll();
            //        txtConQty.Focus();
            //        return;
            //    }
            //    for (int i = 0; i < nQty / 2; i++)
            //    {
            //        //sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and  product_id='" + sPID + "' and product_type='" + sPtype + "'";
            //        //sql += " and num>=" + nSNstart + " and num<=" + nSNend;

            //        sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_id='" + sPID + "'";
            //        sql += " and product_type='" + sPtype + "' and year='" + sYear + "' and week='" + sWeek + "'";
            //        DataSet dsNowNnum = db.Query(sql);
            //        if (dsNowNnum.Tables[0].Rows.Count > 0)
            //        {
            //            nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
            //        }
            //        else
            //        {
            //            nNownum = 0;           
            //        }
            //        //if (nNownum < nSNstart)
            //        //{
            //        //    nNownum = nSNstart - 1;
            //        //}
            //        //if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
            //        //{
            //        //    MessageBox.Show("组件序号超出工艺设定范围！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //        //    return;
            //        //}
            //        //sSN1 = "0121" + (nNownum + 1).ToString("00000#");
            //        //sSN2 = "0121" + (nNownum + 2).ToString("00000#");
            //        sSN1 = "101" + sPtype + sWeek + sYear + (nNownum + 1).ToString("0000#");
            //        sSN2 = "101" + sPtype + sWeek + sYear + (nNownum + 2).ToString("0000#");
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN1 + "','A'," + nNownum + ",'" + sPID + "','" + sPtype + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN2 + "','A'," + nNownum + ",'" + sPID + "','" + sPtype + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        if (nResult > 0)
            //        {
            //            PrintConergyLabel_A(sSN1, sSN2);
            //        }
            //        //Thread.Sleep(DBUtility.nSleepTime);
            //    }
            //}
            //else if (sLabelType == "三排")
            //{
            //    if (nQty % 3 != 0)
            //    {
            //        MessageBox.Show("打印标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        txtConQty.SelectAll();
            //        txtConQty.Focus();
            //        return;
            //    }
            //    for (int i = 0; i < nQty / 3; i++)
            //    {
            //        //sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and  product_id='" + sPID + "' and product_type='" + sPtype + "'";
            //        //sql += " and num>=" + nSNstart + " and num<=" + nSNend;

            //        sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and product_id='" + sPID + "'";
            //        sql += " and product_type='" + sPtype + "' and year='" + sYear + "' and week='" + sWeek + "'";
            //        DataSet dsNowNnum = db.Query(sql);
            //        if (dsNowNnum.Tables[0].Rows.Count > 0)
            //        {
            //            nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
            //        }
            //        else
            //        {
            //            nNownum = 0;
            //        }
            //        //if (nNownum < nSNstart)
            //        //{
            //        //    nNownum = nSNstart - 1;
            //        //}
            //        //if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend || nNownum + 3 < nSNstart || nNownum + 3 > nSNend)
            //        //{
            //        //    MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        //    return;
            //        //}
            //        //sSN1 = "0121" + (nNownum + 1).ToString("00000#");
            //        //sSN2 = "0121" + (nNownum + 2).ToString("00000#");
            //        //sSN3 = "0121" + (nNownum + 3).ToString("00000#");
            //        sSN1 = "101" + sPtype + sWeek + sYear + (nNownum + 1).ToString("0000#");
            //        sSN2 = "101" + sPtype + sWeek + sYear + (nNownum + 2).ToString("0000#");
            //        sSN3 = "101" + sPtype + sWeek + sYear + (nNownum + 3).ToString("0000#");
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN1 + "','B'," + nNownum + ",'" + sPID + "','" + sPtype + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN2 + "','B'," + nNownum + ",'" + sPID + "','" + sPtype + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN3 + "','B'," + nNownum + ",'" + sPID + "','" + sPtype + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        if (nResult > 0)
            //        {
            //            PrintConergyLabel_B(sSN1, sSN2,sSN3);
            //        }
            //        //Thread.Sleep(DBUtility.nSleepTime);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("无对应的标签类型！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //}
        }

        private void btnConReprint_Click(object sender, EventArgs e)
        {
            string sLabelType, sql, sSN1, sSN2, sSN3;
            int nResult, nLabelQty;
            nLabelQty = 0;

            if (txtConSN.Text.Trim() == "" || txtConSN2.Text.Trim() == "")
            {
                MessageBox.Show("组件序号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConSN.SelectAll();
                txtConSN.Focus();
                return;
            }
            if (cboConRLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //sSN1 = txtConSN.Text.Trim();
            sLabelType = cboConRLT.Text.Trim();


            if (sLabelType == "单排")
            {
                sql = "select * from sn_print where label_type='A' and sn>='" + txtConSN.Text.Trim() + "' and sn<='" + txtConSN2.Text.Trim() + "'";
                DataSet dsRsn = db.Query(sql);
                if (dsRsn.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsRsn.Tables[0].Rows.Count;
                    if (nLabelQty == 0)
                    {
                        MessageBox.Show("补打标签数量不能为0，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsRsn.Tables[0].Rows.Count; i++)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsRsn.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintConergyLabel_S(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConSN.SelectAll();
                    txtConSN.Focus();
                    return;
                }
            }
            else if (sLabelType == "双排")
            {
                sql = "select * from sn_print where label_type='A' and sn>='" + txtConSN.Text.Trim() + "' and sn<='" + txtConSN2.Text.Trim() + "'";
                DataSet dsRsn = db.Query(sql);
                if (dsRsn.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsRsn.Tables[0].Rows.Count;
                    if (nLabelQty % 2 != 0)
                    {
                        MessageBox.Show("补打标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsRsn.Tables[0].Rows.Count; i = i + 2)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsRsn.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsRsn.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintConergyLabel_A(dsRsn.Tables[0].Rows[i]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConSN.SelectAll();
                    txtConSN.Focus();
                    return;
                }
            }
            else if (sLabelType == "三排")
            {
                sql = "select * from sn_print where label_type='B' and sn>='" + txtConSN.Text.Trim() + "' and sn<='" + txtConSN2.Text.Trim() + "'";
                DataSet dsRsn = db.Query(sql);
                if (dsRsn.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsRsn.Tables[0].Rows.Count;
                    if (nLabelQty % 3 != 0)
                    {
                        MessageBox.Show("补打标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsRsn.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsRsn.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsRsn.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsRsn.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 1]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 1]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintConergyLabel_B(dsRsn.Tables[0].Rows[i]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 1]["sn"].ToString(), dsRsn.Tables[0].Rows[i + 2]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConSN.SelectAll();
                    txtConSN.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("标签类型不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtConSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnConReprint_Click(sender, e);
                txtConSN.SelectAll();
                txtConSN.Focus();
            }
        }

        private void btnSchPrint_Click(object sender, EventArgs e)
        {
            string sql, sLabelType, sCPCode, sYear, sWeek, sType, sSN1, sSN2, sSN3, sDate;
            int nResult, nQty, nSNstart, nSNend, nNownum;
            int nYear, nWeek;

            //测试
            //PrintSchuecoLabel_B("09KALM60121533301");
            //return;

            sCPCode = txtCPcode.Text.Trim().ToUpper();
            sLabelType = cboSchLT.Text.Trim();
            sType = txtSchType.Text.Trim();

            try
            {
                nQty = int.Parse(txtSchQty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSchQty.SelectAll();
                txtSchQty.Focus();
                return;
            }
            if (sCPCode == "")
            {
                MessageBox.Show("客供代号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCPcode.SelectAll();
                txtCPcode.Focus();
                return;
            }
            if (sLabelType == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (sType == "")
            {
                MessageBox.Show("产品类型不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSchType.SelectAll();
                txtSchType.Focus();
                return;
            }

            #region
            nYear = 0;
            nWeek = 0;
            if (txtSchYear.Text.Trim() != "")
            {
                nYear = int.Parse(txtSchYear.Text.Trim());
            }
            if (txtSchWeek.Text.Trim() != "")
            {
                nWeek = int.Parse(txtSchWeek.Text.Trim());
            }
            PrintSNByCustomer("schueco", "", sCPCode, sType, nYear, 0, nWeek, nQty, sLabelType);
            #endregion

            //sql = "select SUBSTRING(CONVERT(CHAR(4),DATEPART(YEAR,GETDATE())),3,2) as 'year',DATEPART(WEEK,GETDATE()) + 1 as 'week',CONVERT(VARCHAR(10),GETDATE(),23) as 'date'";
            //DataSet dsDate = db.Query(sql);
            //sYear = dsDate.Tables[0].Rows[0]["year"].ToString().Trim();
            //sWeek = int.Parse(dsDate.Tables[0].Rows[0]["week"].ToString().Trim()).ToString("0#");
            //if (txtSchYear.Text.Trim() != "")
            //{
            //    sYear = (int.Parse(sYear) + int.Parse(txtSchYear.Text.Trim())).ToString("0#");
            //}
            //if (txtSchWeek.Text.Trim() != "")
            //{
            //    sWeek = (int.Parse(sWeek) + int.Parse(txtSchWeek.Text.Trim())).ToString("0#");
            //}
            //sDate = dsDate.Tables[0].Rows[0]["date"].ToString().Trim();
            //sql = "select * from sn_print_set where product_id='" + sCPCode + "' and product_type='" + sType + "'";
            //DataSet dsPrintSet = db.Query(sql);
            //if(dsPrintSet.Tables[0].Rows.Count > 0)
            //{
            //    nSNstart = int.Parse(dsPrintSet.Tables[0].Rows[0]["start_num"].ToString().Trim());
            //    nSNend = int.Parse(dsPrintSet.Tables[0].Rows[0]["end_num"].ToString().Trim());
            //}
            //else
            //{
            //    MessageBox.Show("组件序号区间未设定，请联系工艺！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //    return;
            //}

            //if (sLabelType == "单排压银")
            //{
            //    for (int i = 0; i < nQty; i++)
            //    {
            //        sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_id='" + sCPCode + "' and year='" + sYear + "' and week='" + sWeek + "'";
            //        DataSet dsNownum = db.Query(sql);
            //        if (dsNownum.Tables[0].Rows.Count > 0)
            //        {
            //            nNownum = int.Parse(dsNownum.Tables[0].Rows[0]["num"].ToString().Trim());
            //        }
            //        else
            //        {
            //            nNownum = 0;
            //        }
            //        if (nNownum + 1 < nSNstart)
            //        {
            //            nNownum = nSNstart -1;
            //        }
            //        if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend)
            //        {
            //            MessageBox.Show("组件序号超过设定范围！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //            return;
            //        }
            //        sSN1 = "09" + sCPCode + "0" + sYear + sWeek + (nNownum + 1).ToString("0000#");
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN1 + "','A'," + nNownum + ",'" + sCPCode + "','" + sType + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        if (nResult > 0)
            //        {
            //            PrintSchuecoLabel_A(sSN1);
            //        }
            //        //Thread.Sleep(DBUtility.nSleepTime);
            //    }
            //}
            //else if (sLabelType == "单排铜版")
            //{
            //    for (int i = 0; i < nQty; i++)
            //    {
            //        sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and product_id='" + sCPCode + "' and year='" + sYear + "' and week='" + sWeek + "'";
            //        DataSet dsNownum = db.Query(sql);
            //        if (dsNownum.Tables[0].Rows.Count > 0)
            //        {
            //            nNownum = int.Parse(dsNownum.Tables[0].Rows[0]["num"].ToString().Trim());
            //        }
            //        else
            //        {
            //            nNownum = 0;
            //        }
            //        if (nNownum + 1 < nSNstart)
            //        {
            //            nNownum = nSNstart - 1;
            //        }
            //        if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend)
            //        {
            //            MessageBox.Show("组件序号超过设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //        sSN1 = "09" + sCPCode + "0" + sYear + sWeek + (nNownum + 1).ToString("0000#");
            //        nNownum = nNownum + 1;
            //        sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
            //        sql += " values('" + sSN1 + "','B'," + nNownum + ",'" + sCPCode + "','" + sType + "','" + sDate + "','" + sYear + "','" + sWeek + "')";
            //        nResult = db.ExecuteSql(sql);
            //        if (nResult > 0)
            //        {
            //            PrintSchuecoLabel_B(sSN1);
            //        }
            //        //Thread.Sleep(DBUtility.nSleepTime);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("无对应的标签类型！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //}

        }

        private void btnSchReprint_Click(object sender, EventArgs e)
        {
            string sql, sSN, sLabelType;
            int nResult;

            //sSN = txtSchSN.Text.Trim();
            sLabelType = cboSchRLT.Text.Trim();

            if (txtSchSN.Text.Trim() == "" || txtSchSN2.Text.Trim() == "")
            {
                MessageBox.Show("组件序号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSchSN.SelectAll();
                txtSchSN.Focus();
                return;
            }
            if (sLabelType == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sLabelType == "单排压银")
            {
                sql = "select * from sn_print where label_type='A' and sn>='" + txtSchSN.Text.Trim() + "' and sn<='" + txtSchSN2.Text.Trim() + "'";
                DataSet dsRsn = db.Query(sql);
                if (dsRsn.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsRsn.Tables[0].Rows.Count; i++)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsRsn.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintSchuecoLabel_A(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSchSN.SelectAll();
                    txtSchSN.Focus();
                    return;
                }
            }
            else if (sLabelType == "单排铜版")
            {
                sql = "select * from sn_print where label_type='B' and sn>='" + txtSchSN.Text.Trim() + "' and sn<='" + txtSchSN2.Text.Trim() + "'";
                DataSet dsRsn = db.Query(sql);
                if (dsRsn.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsRsn.Tables[0].Rows.Count; i++)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsRsn.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintSchuecoLabel_B(dsRsn.Tables[0].Rows[i]["sn"].ToString());
                        }
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSchSN.SelectAll();
                    txtSchSN.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("标签类型不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSchSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnSchReprint_Click(sender, e);
                txtSchSN.SelectAll();
                txtSchSN.Focus();
            }
        }

        private void btnComCP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboComLT.Text.Trim();


            if (sLabelType == "单排")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_S("0000000000000000");
                    }
                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_S("0000000000000000");
                    }
                }
            }
            else if (sLabelType == "双排")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_A("0000000000000000", "0000000000000000");
                    }
                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_A("0000000000000000", "0000000000000000");
                    }
                }
            }
            else if (sLabelType == "三排")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnComCRP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboComRLT.Text.Trim();

            if (sLabelType == "单排")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_S("0000000000000000");
                    }

                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_S("0000000000000000");
                    }
                }
            }
            else if (sLabelType == "双排")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_A("0000000000000000", "0000000000000000");
                    }

                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_A("0000000000000000", "0000000000000000");
                    }
                }
            }
            else if (sLabelType == "三排")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (cboxNarrow.Checked == true)
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
                else
                {
                    if (rbtAgox.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtZero.Checked == true)
                    {
                        PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                    else if (rbtCommon.Checked == true)
                    {
                        PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConCP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboConLT.Text.Trim();

            if (sLabelType == "单排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintConergyLabel_S("00000000000000");
                }
            }
            else if (sLabelType == "双排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintConergyLabel_A("00000000000000", "00000000000000");
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000", "00000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000", "00000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintConergyLabel_B("00000000000000", "00000000000000", "00000000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConCRP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboConRLT.Text.Trim();

            if (sLabelType == "单排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintConergyLabel_S("00000000000000");
                }

            }
            if (sLabelType == "双排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintConergyLabel_A("00000000000000", "00000000000000");
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000", "00000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00000000000000", "00000000000000", "00000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintConergyLabel_B("00000000000000", "00000000000000", "00000000000000");
                }

            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSchCP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboSchLT.Text.Trim();

            if (sLabelType == "单排压银")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintSchuecoLabel_A("00XXXX00000000000");
                }
            }
            else if (sLabelType == "单排铜版")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintSchuecoLabel_B("00XXXX00000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSchCRP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboSchRLT.Text.Trim();

            if (sLabelType == "单排压银")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintSchuecoLabel_A("00XXXX00000000000");
                }
            }
            else if (sLabelType == "单排铜版")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("00XXXX00000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintSchuecoLabel_B("00XXXX00000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnWOCP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboWOLT.Text.Trim();
            if (sLabelType == "双排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_A("0000000000000000", "0000000000000000");
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                }
            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnWOPrint_Click(object sender, EventArgs e)
        {
            string sql, sDate, sWO, sType, sLabelType, sSN1, sSN2, sSN3, sPower;
            int nQty, nYear, nMonth, nNum, nResult, nSNstart, nSNend, nNownum;

            if (txtWO.Text.Trim() == "")
            {
                MessageBox.Show("工单号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWO.SelectAll();
                txtWO.Focus();
                return;
            }
            if (txtProType.Text.Trim() == "")
            {
                MessageBox.Show("产品型号代码不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProType.SelectAll();
                txtProType.Focus();
                return;
            }
            if (txtPower.Text.Trim() == "")
            {
                MessageBox.Show("功率档位不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPower.SelectAll();
                txtPower.Focus();
                return;
            }
            try
            {
                nQty = int.Parse(txtWOQty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWOQty.SelectAll();
                txtWOQty.Focus();
                return;
            }
            if (cboWOLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sLabelType = cboWOLT.Text.Trim();
            sDate = DateTime.Now.ToString("yyyy-MM-dd");
            nYear = int.Parse(sDate.Substring(2, 2)) + 18;
            nMonth = int.Parse(sDate.Substring(5, 2)) + 18;
            if (txtWOYear.Text.Trim() != "")
            {
                nYear = nYear + int.Parse(txtWOYear.Text.Trim());
            }
            if (txtWOMonth.Text.Trim() != "")
            {
                nMonth = nMonth + int.Parse(txtWOMonth.Text.Trim());
                if (nMonth > 30)
                {
                    nMonth = 19;
                }
            }

            sWO = txtWO.Text.Trim();
            sType = txtProType.Text.Trim();
            sPower = txtPower.Text.Trim();

            sql = "select * from sn_print_set where product_id='" + sWO + "' and product_type='" + sPower + "'";
            DataSet dsSNset = db.Query(sql);
            if (dsSNset.Tables[0].Rows.Count > 0)
            {
                nSNstart = int.Parse(dsSNset.Tables[0].Rows[0]["start_num"].ToString().Trim());
                nSNend = int.Parse(dsSNset.Tables[0].Rows[0]["end_num"].ToString().Trim());
            }
            else
            {
                MessageBox.Show("产品对应的组件序号范围未设定，请联系工艺！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sLabelType == "单排")
            {
                for (int i = 0; i < nQty; i++)
                {
                    //sWO = txtWO.Text.Trim();
                    //sType = txtProType.Text.Trim();
                    //sPower = txtPower.Text.Trim();
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and wo='" + sWO + "' and power='" + sPower + "'";
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sSN1 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 1).ToString("0000#");
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN1 + "','A'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql); ;
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_S(sSN1);
                        }
                    }
                    //Thread.Sleep(DBUtility.nSleepTime);
                }
            }
            if (sLabelType == "双排")
            {
                for (int i = 0; i < nQty / 2; i++)
                {
                    //sWO = txtWO.Text.Trim();
                    //sType = txtProType.Text.Trim();
                    //sPower = txtPower.Text.Trim();
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and wo='" + sWO + "' and power='" + sPower + "'";
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sSN1 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 1).ToString("0000#");
                    sSN2 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 2).ToString("0000#");
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN1 + "','A'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN2 + "','A'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_A(sSN1, sSN2);
                        }
                    }
                    //Thread.Sleep(DBUtility.nSleepTime);
                }
            }
            if (sLabelType == "三排")
            {
                for (int i = 0; i < nQty / 3; i++)
                {
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and  wo='" + sWO + "' and power='" + sPower + "'";
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend || nNownum + 3 < nSNstart || nNownum + 3 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sSN1 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 1).ToString("0000#");
                    sSN2 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 2).ToString("0000#");
                    sSN3 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 3).ToString("0000#");
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN1 + "','B'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN2 + "','B'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN3 + "','B'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(sSN1, sSN2, sSN3);
                        }

                    }
                    //Thread.Sleep(DBUtility.nSleepTime);
                }
            }
            if (sLabelType == "三排无BARCODE")
            {
                for (int i = 0; i < nQty / 3; i++)
                {
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='C' and  wo='" + sWO + "' and power='" + sPower + "'";
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend || nNownum + 3 < nSNstart || nNownum + 3 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sSN1 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 1).ToString("0000#");
                    sSN2 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 2).ToString("0000#");
                    sSN3 = sWO + nYear.ToString("0#") + nMonth.ToString("0#") + sType + (nNownum + 3).ToString("0000#");
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN1 + "','C'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN2 + "','C'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month,power)";
                    sql += " values('" + sSN3 + "','C'," + nNownum + ",'" + sWO + "','" + sType + "','" + sDate + "','" + nYear.ToString("0#") + "','" + nMonth.ToString("0#") + "','" + sPower + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(sSN1, sSN2, sSN3);
                        }
                    }
                    //Thread.Sleep(DBUtility.nSleepTime);
                }
            }
        }

        private void btnWORCP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboWORLT.Text.Trim();

            if (sLabelType == "单排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_S("0000000000000000");
                }
            }
            else if (sLabelType == "双排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_A("0000000000000000", "0000000000000000");
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                }
            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnWORPT_Click(object sender, EventArgs e)
        {
            string sql;
            int nResult;

            if (txtWOSN.Text.Trim() == "" || txtWOSN2.Text.Trim() == "")
            {
                MessageBox.Show("组件序列号为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWOSN.SelectAll();
                txtWOSN.Focus();
                return;
            }
            if (cboWORLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboWORLT.Text.Trim() == "单排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtWOSN.Text.Trim() + "' and sn <='" + txtWOSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 1)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_S(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWOSN.Text.Trim();
                    txtWOSN.Focus();
                    return;
                }
            }
            if (cboWORLT.Text.Trim() == "双排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtWOSN.Text.Trim() + "' and sn <='" + txtWOSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 2)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_A(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWOSN.Text.Trim();
                    txtWOSN.Focus();
                    return;
                }
            }
            if (cboWORLT.Text.Trim() == "三排")
            {
                sql = "select sn from sn_print where label_type='B' and sn>='" + txtWOSN.Text.Trim() + "' and sn <='" + txtWOSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }


                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWOSN.SelectAll();
                    txtWOSN.Focus();
                    return;
                }
            }
            if (cboWORLT.Text.Trim() == "三排无BARCODE")
            {
                sql = "select sn from sn_print where label_type='C' and sn>='" + txtWOSN.Text.Trim() + "' and sn <='" + txtWOSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }


                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWOSN.SelectAll();
                    txtWOSN.Focus();
                    return;
                }
            }
        }

        private void txtFacSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            string sql, SN;

            if (e.KeyChar == 13)
            {
                if (txtFacSN.Text.Trim() == "")
                {
                    MessageBox.Show("厂内组件序号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFacSN.SelectAll();
                    txtFacSN.Focus();
                    return;
                }
                SN = txtFacSN.Text.Trim();
                sql = "select * from testdata where vc_default='1' and vc_psign='1' and serialno='" + SN + "'";
                DataSet dsTestData = db.Query(sql);
                if (dsTestData.Tables[0].Rows.Count > 0)
                {
                    lblPwoer.Text = dsTestData.Tables[0].Rows[0]["vc_modname"].ToString().Trim();
                    txtCusSN.SelectAll();
                    txtCusSN.Focus();
                }
                else
                {
                    MessageBox.Show("组件未测试或未打印主标签！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFacSN.SelectAll();
                    txtFacSN.Focus();
                    return;
                }
            }

        }

        private void txtCusSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnFCSNLink_Click(sender, e);
            }
        }

        private void btnFCSNLink_Click(object sender, EventArgs e)
        {
            string sql, sOldSN, sNewSN, sSN, sPmp, sVoc, sIsc, sVmp, sImp, sOldCusSN;
            int nQty, nResult;

            if (txtFacSN.Text.Trim() == "")
            {
                MessageBox.Show("厂内组件序号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFacSN.SelectAll();
                txtFacSN.Focus();
                return;
            }
            if (txtCusSN.Text.Trim() == "")
            {
                MessageBox.Show("客户序号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCusSN.SelectAll();
                txtCusSN.Focus();
                return;
            }
            sOldSN = txtFacSN.Text.Trim();
            sNewSN = txtCusSN.Text.Trim();

            sql = "select * from testdata where vc_default='1' and vc_psign='1' and serialno='" + sOldSN + "'";
            DataSet dsTestData = db.Query(sql);
            if (dsTestData.Tables[0].Rows.Count > 0)
            {
                lblPwoer.Text = dsTestData.Tables[0].Rows[0]["vc_modname"].ToString().Trim();
                //sql = "select distinct(vc_custcode) as 'vc_custcode' from testdata where serialno='" + sOldSN + "'";
                sql = "select vc_custcode from testdata where isnull(vc_custcode,'')!='' and serialno='" + sOldSN + "' group by vc_custcode";
                DataSet dsOldCusSN = db.Query(sql);
                if (dsOldCusSN.Tables[0].Rows.Count > 0)
                {
                    if (dsOldCusSN.Tables[0].Rows.Count > 1)
                    {
                        MessageBox.Show("该组件有多个客户序号请联系IT", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFacSN.SelectAll();
                        txtFacSN.Focus();
                        return;
                    }
                    else
                    {
                        sOldCusSN = dsTestData.Tables[0].Rows[0]["vc_custcode"].ToString().Trim();
                    }
                }
                else
                {
                    sOldCusSN = "";
                }
                if (sOldCusSN != "" && sOldCusSN != sNewSN)
                {
                    MessageBox.Show("该有组件已对应客户序号：" + sOldCusSN, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCusSN.SelectAll();
                    txtCusSN.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("组件未测试或未打印主标签！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFacSN.SelectAll();
                txtFacSN.Focus();
                return;
            }

            sql = "select distinct(serialno) as 'serialno' from testdata where vc_custcode='" + sNewSN + "'";
            DataSet dsCusSN = db.Query(sql);
            if (dsCusSN.Tables[0].Rows.Count > 0)
            {
                if (dsCusSN.Tables[0].Rows.Count > 1)
                {
                    MessageBox.Show("该客户序号已被重复使用，请联系IT", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCusSN.SelectAll();
                    txtCusSN.Focus();
                    return;
                }
                else
                {
                    if (dsCusSN.Tables[0].Rows[0]["serialno"].ToString().Trim().ToUpper() != sOldSN.ToUpper())
                    {
                        MessageBox.Show("该客户序号已被" + dsCusSN.Tables[0].Rows[0]["serialno"].ToString().Trim().ToUpper() + "使用！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCusSN.SelectAll();
                        txtCusSN.Focus();
                        return;
                    }
                }
            }

            sql = "select * from sn_print where wo='" + sNewSN.Substring(0, 5) + "' and power='" + lblPwoer.Text.Trim() + "' and sn='" + sNewSN + "'";
            DataSet dsPrintSN = db.Query(sql);
            if (dsPrintSN.Tables[0].Rows.Count < 1)
            {
                MessageBox.Show("客户序号未在系统中生成或无此功率档对应的客户序号范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCusSN.SelectAll();
                txtCusSN.Focus();
                return;
            }

            sql = "update testdata set vc_custcode='" + sNewSN + "' where vc_default='1' and vc_psign='1' and serialno='" + sOldSN + "'";
            nResult = db.ExecuteSql(sql);
            if (nResult > 0)
            {
                sSN = sNewSN;
                sPmp = double.Parse(dsTestData.Tables[0].Rows[0]["coef_pmax"].ToString().Trim()).ToString("#,###.0");
                sVoc = double.Parse(dsTestData.Tables[0].Rows[0]["coef_voc"].ToString().Trim()).ToString("###.00");
                sIsc = double.Parse(dsTestData.Tables[0].Rows[0]["coef_isc"].ToString().Trim()).ToString("###.00");
                sVmp = double.Parse(dsTestData.Tables[0].Rows[0]["coef_vmax"].ToString().Trim()).ToString("###.00");
                sImp = double.Parse(dsTestData.Tables[0].Rows[0]["coef_imax"].ToString().Trim()).ToString("###.00");
                PrintPwoerLabel(sSN, sPmp, sVoc, sIsc, sVmp, sImp);
            }
        }

        private void PrintPwoerLabel(string SN, string Pmp, string Voc, string Isc, string Vmp, string Imp)
        {
            int i_return, i_labqty, nDarkness;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());

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

            s_value = SN;
            i_return = B_Prn_Barcode(15 + int.Parse(txtX.Text.Trim()), 11 + int.Parse(txtY.Text.Trim()), 0, "1", 3, 4, 64, Convert.ToChar(78), s_value);
            i_return = B_Prn_Text_TrueType(15 + int.Parse(txtX.Text.Trim()), 81 + int.Parse(txtY.Text.Trim()), 40, "Arial", 1, 500, 0, 0, 0, "A0", s_value);
            s_value = "Pm=" + Pmp + "Wp";
            i_return = B_Prn_Text_TrueType(15 + int.Parse(txtX.Text.Trim()), 123 + int.Parse(txtY.Text.Trim()), 42, "Arial", 1, 500, 0, 0, 0, "A1", s_value);
            s_value = "Voc =" + Voc + "V";
            i_return = B_Prn_Text_TrueType(392 + int.Parse(txtX.Text.Trim()), 8 + int.Parse(txtY.Text.Trim()), 35, "Arial", 1, 500, 0, 0, 0, "A21", s_value);
            s_value = "I sc =" + Isc + "A";
            i_return = B_Prn_Text_TrueType(397 + int.Parse(txtX.Text.Trim()), 49 + int.Parse(txtY.Text.Trim()), 33, "Arial", 1, 500, 0, 0, 0, "A31", s_value);
            s_value = "Vmp=" + Vmp + "V";
            i_return = B_Prn_Text_TrueType(392 + int.Parse(txtX.Text.Trim()), 92 + int.Parse(txtY.Text.Trim()), 33, "Arial", 1, 500, 0, 0, 0, "A91", s_value);
            s_value = "Imp =" + Imp + "A";
            i_return = B_Prn_Text_TrueType(397 + int.Parse(txtX.Text.Trim()), 133 + int.Parse(txtY.Text.Trim()), 33, "Arial", 1, 500, 0, 0, 0, "A51", s_value);

            i_return = B_Print_Out(i_labqty);//列印所有資料

            B_ClosePrn();//关闭打印
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SNQuery.frmSNPrintedQuery frmSNPrintedQuery = new SNQuery.frmSNPrintedQuery();
            frmSNPrintedQuery.ShowDialog();
        }

        private void btnCustPrint_Click(object sender, EventArgs e)
        {
            string sql, sDate, sWO, sType, sLabelType, sSN1, sSN2, sSN3, sProductType;
            int nQty, nYear, nMonth, nNum, nResult, nStartNum, nEndNum;

            if (txtCustWO.Text.Trim() == "")
            {
                MessageBox.Show("工单号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustWO.SelectAll();
                txtCustWO.Focus();
                return;
            }
            if (txtCustPriNo.Text.Trim() == "")
            {
                MessageBox.Show("客供代码不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustPriNo.SelectAll();
                txtCustPriNo.Focus();
                return;
            }
            try
            {
                nQty = int.Parse(txtCustQty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustQty.SelectAll();
                txtCustQty.Focus();
                return;
            }
            if (cboCustLabelType.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sLabelType = cboCustLabelType.Text.Trim();
            sDate = DateTime.Now.ToString("yyyy-MM-dd");

            //------------------------检查一个工单号只能有一个产品型号代码----------------------------------------------------------------------
            sql = "select wo,product_type from sn_print";
            sql += " where wo='" + txtCustWO.Text.Trim() + "'";
            sql += " group by wo,product_type";
            DataSet dsProductType = db.Query(sql);
            if (dsProductType.Tables[0].Rows.Count > 0)
            {
                sProductType = dsProductType.Tables[0].Rows[0]["product_type"].ToString().Trim();
                if (sProductType != txtCustPriNo.Text.Trim())
                {
                    MessageBox.Show("工单对应的客供代码[" + sProductType + "]", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustPriNo.SelectAll();
                    txtCustPriNo.Focus();
                    return;
                }
            }
            //-------------------------------------------------------------------------------------------------------------------------------
            sql = "select start_num,end_num from sn_print_set where product_id='" + txtCustPriNo.Text.Trim() + "' and product_type='" + txtCustPriNo.Text.Trim() + "'";
            DataSet dsCheckSN = db.Query(sql);
            if (dsCheckSN.Tables[0].Rows.Count < 1)
            {
                MessageBox.Show("工艺未设定该客供代码对应的组件序号范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            nStartNum = int.Parse(dsCheckSN.Tables[0].Rows[0]["start_num"].ToString().Trim());
            nEndNum = int.Parse(dsCheckSN.Tables[0].Rows[0]["end_num"].ToString().Trim());

            if (sLabelType == "单排")
            {
                for (int i = 0; i < nQty; i++)
                {
                    sWO = txtCustWO.Text.Trim();
                    sProductType = txtCustPriNo.Text.Trim();
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_type='" + sProductType + "'";
                    DataSet dsComP = db.Query(sql);
                    nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());

                    if (nNum < nStartNum)
                    {
                        nNum = nStartNum - 1;
                    }
                    if (nNum + 2 > nEndNum)
                    {
                        MessageBox.Show("组件序号超过工艺设定的组件序号范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sSN1 = sProductType + (nNum + 1).ToString("0000#");
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    nNum = nNum + 1;
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_S(sSN1);
                        }
                    }
                }
            }
            if (sLabelType == "双排")
            {
                for (int i = 0; i < nQty / 2; i++)
                {
                    sWO = txtCustWO.Text.Trim();
                    sProductType = txtCustPriNo.Text.Trim();
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_type='" + sProductType + "'";
                    DataSet dsComP = db.Query(sql);
                    nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());

                    if (nNum < nStartNum)
                    {
                        nNum = nStartNum - 1;
                    }
                    if (nNum + 2 > nEndNum)
                    {
                        MessageBox.Show("组件序号超过工艺设定的组件序号范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sSN1 = sProductType + (nNum + 1).ToString("0000#");
                    sSN2 = sProductType + (nNum + 2).ToString("0000#");
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN2 + "','A'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_A(sSN1, sSN2);
                        }
                    }
                }
            }
            if (sLabelType == "三排")
            {
                for (int i = 0; i < nQty / 3; i++)
                {
                    sWO = txtCustWO.Text.Trim();
                    sProductType = txtCustPriNo.Text.Trim();
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and product_type='" + sProductType + "'";
                    DataSet dsComP = db.Query(sql);
                    nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());

                    if (nNum < nStartNum)
                    {
                        nNum = nStartNum - 1;
                    }
                    if (nNum + 3 > nEndNum)
                    {
                        MessageBox.Show("组件序号超过工艺设定的组件序号范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sSN1 = sProductType + (nNum + 1).ToString("0000#");
                    sSN2 = sProductType + (nNum + 2).ToString("0000#");
                    sSN3 = sProductType + (nNum + 3).ToString("0000#");
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN1 + "','B'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN2 + "','B'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN3 + "','B'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(sSN1, sSN2, sSN3);
                        }
                    }
                }
            }
            if (sLabelType == "三排无BARCODE")
            {
                for (int i = 0; i < nQty / 3; i++)
                {
                    sWO = txtCustWO.Text.Trim();
                    sProductType = txtCustPriNo.Text.Trim();
                    sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='C' and product_type='" + sProductType + "'";
                    DataSet dsComP = db.Query(sql);
                    nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());

                    if (nNum < nStartNum)
                    {
                        nNum = nStartNum - 1;
                    }
                    if (nNum + 2 > nEndNum)
                    {
                        MessageBox.Show("组件序号超过工艺设定的组件序号范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sSN1 = sProductType + (nNum + 1).ToString("0000#");
                    sSN2 = sProductType + (nNum + 2).ToString("0000#");
                    sSN3 = sProductType + (nNum + 3).ToString("0000#");
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN1 + "','C'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN2 + "','C'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    nNum = nNum + 1;
                    sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date)";
                    sql += " values('" + sSN3 + "','C'," + nNum + ",'" + sWO + "','" + sProductType + "','" + sDate + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(sSN1, sSN2, sSN3);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(sSN1, sSN2, sSN3);
                        }

                    }
                }
            }
            B_ClosePrn();//关闭打印
        }

        private void btnCustPrintPC_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboCustLabelType.Text.Trim();

            if (sLabelType == "单排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_S("0000000000000000");
                }
            }
            if (sLabelType == "双排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_A("0000000000000000", "0000000000000000");
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                }

            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCustRePrintCP_Click(object sender, EventArgs e)
        {
            string sLabelType;
            sLabelType = cboCustLabelType.Text.Trim();

            if (sLabelType == "单排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_S("0000000000000000");
                }
            }
            if (sLabelType == "双排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_A("0000000000000000", "0000000000000000");
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                }

            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (rbtAgox.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtZero.Checked == true)
                {
                    PrintSerialno("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCustRePrint_Click(object sender, EventArgs e)
        {
            string sql;
            int nResult;

            if (txtCustSNStart.Text.Trim() == "" || txtCustSNEnd.Text.Trim() == "")
            {
                MessageBox.Show("组件序列号为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustSNStart.SelectAll();
                txtCustSNStart.Focus();
                return;
            }
            if (cboCustLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboCustLT.Text.Trim() == "单排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtCustSNStart.Text.Trim() + "' and sn <='" + txtCustSNEnd.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 1)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_S(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboCustLT.Text.Trim() == "双排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtCustSNStart.Text.Trim() + "' and sn <='" + txtCustSNEnd.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 2)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_A(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboCustLT.Text.Trim() == "三排")
            {
                sql = "select sn from sn_print where label_type='B' and sn>='" + txtCustSNStart.Text.Trim() + "' and sn <='" + txtCustSNEnd.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboCustLT.Text.Trim() == "三排无BARCODE")
            {
                sql = "select sn from sn_print where label_type='C' and sn>='" + txtCustSNStart.Text.Trim() + "' and sn <='" + txtCustSNEnd.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustSNStart.SelectAll();
                    txtCustSNStart.Focus();
                    return;
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            SNSet.frmSNRangerSet frmSNRangerSet = new SNSet.frmSNRangerSet();
            frmSNRangerSet.ShowDialog();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            SNSet.frmSNFormatSet frmSNFormatSet = new SNSet.frmSNFormatSet();
            frmSNFormatSet.ShowDialog();
        }

        public DataSet GetSNFormatByCustomer(string sCustomer)
        {
            DataSet dsResult = new DataSet();
            string sql;
            sql = "select * from sn_format_set where customer='" + sCustomer + "'";
            sql += " order by customer,sequence asc";
            dsResult = db.Query(sql);
            return dsResult;
        }

        public DataSet GetServerDate()
        {
            DataSet dsResult = new DataSet();
            string sql;
            sql = "select getdate() as 'now_time',datepart(year,getdate()) as 'now_year',datepart(month,getdate()) as 'now_month'";
            sql += ",datepart(week,getdate()) as 'now_week',DATEPART(DAYOFYEAR,GETDATE()) as 'dayofyear'";
            sql += ",DATEPART(DAY,GETDATE()) as 'dayofmonth',DATEPART(WEEKDAY,GETDATE()) as 'dayofweek'";
            dsResult = db.Query(sql);
            return dsResult;
        }

        public void PrintSNByCustomer(string sCustomer, string sWO, string sProID, string sProType, int nYear, int nMonth, int nWeek, int nLabelQty, string sLabelType)
        {
            string sql = string.Empty;
            string sSN, sType, sName, sAdjustType, sAdjustValue, sStartIndex, sLength, sFormat, sSNFormat;
            string sParamer, sQYear, sQMonth, sQWeek, sDate;
            string sSN1, sSN2, sSN3;
            int nNum, nResult;
            string sNowTime, sNowYear, sNowMonth, sNowWeek;
            DataSet dsServerDate, dsSNFormat;
            int nSNstart, nSNend;

            switch (sCustomer)
            {
                #region
                case "common":
                    sql = "select wo,product_type from sn_print";
                    sql += " where wo='" + sWO + "'";
                    sql += " group by wo,product_type";
                    DataSet dsProductType = db.Query(sql);
                    if (dsProductType.Tables[0].Rows.Count > 0)
                    {
                        string sProductType = dsProductType.Tables[0].Rows[0]["product_type"].ToString().Trim();
                        if (sProductType.ToUpper() != sProType.ToUpper())
                        {
                            MessageBox.Show("工单号[" + sWO + "]已对应产品型号[" + sProductType + "]，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    dsServerDate = GetServerDate();
                    dsSNFormat = GetSNFormatByCustomer("common");
                    if (dsSNFormat.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("客户[" + sCustomer + "]的组件序号编码规则没有设定，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sNowTime = dsServerDate.Tables[0].Rows[0]["now_time"].ToString();
                    sNowYear = dsServerDate.Tables[0].Rows[0]["now_year"].ToString();
                    sNowMonth = int.Parse(dsServerDate.Tables[0].Rows[0]["now_month"].ToString()).ToString("0#");
                    sNowWeek = int.Parse(dsServerDate.Tables[0].Rows[0]["now_week"].ToString()).ToString("0#");
                    sDate = DateTime.Parse(sNowTime).ToString("yyyy-MM-dd");
                    sSN = "";
                    sQYear = "";
                    sQMonth = "";
                    sQWeek = "";
                    sSNFormat = "";
                    for (int i = 0; i < dsSNFormat.Tables[0].Rows.Count; i++)
                    {
                        sType = "";
                        sName = "";
                        sAdjustType = "";
                        sAdjustValue = "";
                        sStartIndex = "";
                        sLength = "";
                        sFormat = "";
                        sParamer = "";
                        sType = dsSNFormat.Tables[0].Rows[i]["parameter_type"].ToString().Trim();
                        sName = dsSNFormat.Tables[0].Rows[i]["parameter"].ToString().Trim();
                        sAdjustType = dsSNFormat.Tables[0].Rows[i]["adjust_type"].ToString().Trim();
                        sAdjustValue = dsSNFormat.Tables[0].Rows[i]["adjust_value"].ToString().Trim();
                        sStartIndex = dsSNFormat.Tables[0].Rows[i]["start_index"].ToString().Trim();
                        sLength = dsSNFormat.Tables[0].Rows[i]["length"].ToString().Trim();
                        sFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                        if (sType.ToLower() == "input")
                        {
                            if (sName.ToLower() == "wo")
                            {
                                sSN = sSN + sWO.Substring(0, 2) + sWO.Substring(sWO.Length - 3, 3);
                            }
                            if (sName.ToLower() == "protype")
                            {
                                sSN = sSN + sProType;
                            }
                            if (sName.ToLower() == "proid")
                            {
                                sSN = sSN + sProID;
                            }
                        }
                        else
                        {
                            if (sName.ToLower() == "year")
                            {
                                sParamer = sNowYear.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                }
                                sQYear = (int.Parse(sParamer) + nYear).ToString(sFormat);
                                sSN = sSN + sQYear;
                            }
                            else if (sName.ToLower() == "month")
                            {
                                sParamer = sNowMonth.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQMonth = (int.Parse(sParamer) + nMonth).ToString(sFormat);
                                sSN = sSN + sQMonth;
                            }
                            else if (sName.ToLower() == "week")
                            {
                                sParamer = sNowWeek.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQWeek = (int.Parse(sParamer) + nWeek).ToString(sFormat);
                                sSN = sSN + sQWeek;
                            }
                            else if (sName.ToLower() == "sn")
                            {
                                sSNFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                            }
                            else
                            {
                                sSN = sSN + dsSNFormat.Tables[0].Rows[i]["parameter_value"].ToString().Trim();
                            }
                        }
                    }

                    if (sLabelType == "单排")
                    {
                        lotNumbers = new string[nLabelQty];

                        for (int i = 0; i < nLabelQty; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='S' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','S'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);


                            lotNumbers[i] = sSN1;

                            //if (nResult > 0)
                            //{
                            //    if (rbtAgox.Checked == true)
                            //    {
                            //        PrintSerialno(sSN1);
                            //    }
                            //    else if (rbtZero.Checked == true)
                            //    {
                            //        PrintSerialno(sSN1);
                            //    }
                            //    else if (rbtCommon.Checked == true)
                            //    {
                            //        PrintCommonLabel_S(sSN1);

                            //    }
                            //}
                            PrintCommonLabel_S(sSN1);
                        }
                    }
                    if (sLabelType == "双排")
                    {
                        if (nLabelQty % 2 != 0)
                        {
                            MessageBox.Show("打印标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        lotNumbers = new string[nLabelQty];

                        for (int i = 0; i < nLabelQty / 2; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN2 + "','A'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);


                            lotNumbers[i * 2] = sSN1;
                            lotNumbers[i * 2 + 1] = sSN2;


                            //if (nResult > 0)
                            //{
                            //    if (rbtAgox.Checked == true)
                            //    {
                            //        PrintSerialno(sSN1, sSN2);
                            //    }
                            //    else if (rbtZero.Checked == true)
                            //    {
                            //        PrintSerialno(sSN1, sSN2);
                            //    }
                            //    else if (rbtCommon.Checked == true)
                            //    {
                            //        PrintCommonLabel_A(sSN1, sSN2);
                            //    }
                            //}
                        }
                        //调整条码打印在处理后集中进行打印
                        PrintCommonLabel_A(lotNumbers);

                    }
                    if (sLabelType == "三排")
                    {
                        if (nLabelQty % 3 != 0)
                        {
                            MessageBox.Show("打印标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        for (int i = 0; i < nLabelQty / 3; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN3 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            sSN3 = sSN + (nNum + 3).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','B'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN2 + "','B'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN3 + "','B'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);

                            //设定打印间隔 减少 打印机缓存
                            Thread.Sleep(DBUtility.nSleepTime);

                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintCommonLabel_B(sSN1, sSN2, sSN3);
                                }

                            }
                        }
                    }
                    if (sLabelType == "三排无BARCODE")
                    {
                        if (nLabelQty % 3 != 0)
                        {
                            MessageBox.Show("打印标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        for (int i = 0; i < nLabelQty / 3; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='C' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN3 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            sSN3 = sSN + (nNum + 3).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','C'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN2 + "','C'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN3 + "','C'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);

                            //设定打印间隔 减少 打印机缓存
                            Thread.Sleep(DBUtility.nSleepTime);

                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintCommonLabel_C(sSN1, sSN2, sSN3);
                                }
                            }
                        }
                    }
                    break;
#endregion
                #region
                case "conergy":
                    dsServerDate = GetServerDate();
                    dsSNFormat = GetSNFormatByCustomer("conergy");
                    if (dsSNFormat.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("客户[" + sCustomer + "]的组件序号编码规则没有设定，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sNowTime = dsServerDate.Tables[0].Rows[0]["now_time"].ToString();
                    sNowYear = dsServerDate.Tables[0].Rows[0]["now_year"].ToString();
                    sNowMonth = int.Parse(dsServerDate.Tables[0].Rows[0]["now_month"].ToString()).ToString("0#");
                    sNowWeek = int.Parse(dsServerDate.Tables[0].Rows[0]["now_week"].ToString()).ToString("0#");
                    sDate = DateTime.Parse(sNowTime).ToString("yyyy-MM-dd");
                    sSN = "";
                    sQYear = "";
                    sQMonth = "";
                    sQWeek = "";
                    sSNFormat = "";
                    for (int i = 0; i < dsSNFormat.Tables[0].Rows.Count; i++)
                    {
                        sType = "";
                        sName = "";
                        sAdjustType = "";
                        sAdjustValue = "";
                        sStartIndex = "";
                        sLength = "";
                        sFormat = "";
                        sParamer = "";
                        sType = dsSNFormat.Tables[0].Rows[i]["parameter_type"].ToString().Trim();
                        sName = dsSNFormat.Tables[0].Rows[i]["parameter"].ToString().Trim();
                        sAdjustType = dsSNFormat.Tables[0].Rows[i]["adjust_type"].ToString().Trim();
                        sAdjustValue = dsSNFormat.Tables[0].Rows[i]["adjust_value"].ToString().Trim();
                        sStartIndex = dsSNFormat.Tables[0].Rows[i]["start_index"].ToString().Trim();
                        sLength = dsSNFormat.Tables[0].Rows[i]["length"].ToString().Trim();
                        sFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                        if (sType.ToLower() == "input")
                        {
                            if (sName.ToLower() == "wo")
                            {
                                sSN = sSN + sWO;
                            }
                            if (sName.ToLower() == "protype")
                            {
                                sSN = sSN + sProType;
                            }
                            if (sName.ToLower() == "proid")
                            {
                                sSN = sSN + sProID;
                            }
                        }
                        else
                        {
                            if (sName.ToLower() == "year")
                            {
                                sParamer = sNowYear.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                }
                                sQYear = (int.Parse(sParamer) + nYear).ToString(sFormat);
                                sSN = sSN + sQYear;
                            }
                            else if (sName.ToLower() == "month")
                            {
                                sParamer = sNowMonth.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQMonth = (int.Parse(sParamer) + nMonth).ToString(sFormat);
                                sSN = sSN + sQMonth;
                            }
                            else if (sName.ToLower() == "week")
                            {
                                sParamer = sNowWeek.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQWeek = (int.Parse(sParamer) + nWeek).ToString(sFormat);
                                sSN = sSN + sQWeek;
                            }
                            else if (sName.ToLower() == "sn")
                            {
                                sSNFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                            }
                            else
                            {
                                sSN = sSN + dsSNFormat.Tables[0].Rows[i]["parameter_value"].ToString().Trim();
                            }
                        }
                    }
                    if (sLabelType == "单排")
                    {
                        for (int i = 0; i < nLabelQty; i++)
                        {
                            //sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_id='" + sProID + "'";
                            sql += " and product_type='" + sProType + "' and year='" + sQYear + "' and week='" + sQWeek + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);

                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintConergyLabel_S(sSN1);
                                }
                            }
                        }
                    }
                    if (sLabelType == "双排")
                    {
                        if (nLabelQty % 2 != 0)
                        {
                            MessageBox.Show("打印标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        for (int i = 0; i < nLabelQty / 2; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_id='" + sProID + "'";
                            sql += " and product_type='" + sProType + "' and year='" + sQYear + "' and week='" + sQWeek + "'";
                            DataSet dsConP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsConP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN2 + "','A'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintConergyLabel_A(sSN1, sSN2);
                                }

                            }
                        }
                    }
                    if (sLabelType == "三排")
                    {
                        if (nLabelQty % 3 != 0)
                        {
                            MessageBox.Show("打印标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        for (int i = 0; i < nLabelQty / 3; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_id='" + sProID + "'";
                            sql += " and product_type='" + sProType + "' and year='" + sQYear + "' and week='" + sQWeek + "'";
                            DataSet dsConP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsConP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN3 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            sSN3 = sSN + (nNum + 3).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN1 + "','B'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN2 + "','B'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN3 + "','B'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintConergyLabel_B(sSN1, sSN2, sSN3);
                                }
                            }
                        }
                    }
                    break;
                    #endregion
                #region
                case "schueco":
                    nSNstart = 0;
                    nSNend = 0;
                    sql = "select * from sn_print_set where product_id='" + sProID + "' and product_type='" + sProType + "'";
                    DataSet dsPrintSet = db.Query(sql);
                    if (dsPrintSet.Tables[0].Rows.Count > 0)
                    {
                        nSNstart = int.Parse(dsPrintSet.Tables[0].Rows[0]["start_num"].ToString().Trim());
                        nSNend = int.Parse(dsPrintSet.Tables[0].Rows[0]["end_num"].ToString().Trim());
                    }
                    else
                    {
                        MessageBox.Show("组件序号区间未设定，请联系工艺！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    dsServerDate = GetServerDate();
                    dsSNFormat = GetSNFormatByCustomer("schueco");
                    if (dsSNFormat.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("客户[" + sCustomer + "]的组件序号编码规则没有设定，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sNowTime = dsServerDate.Tables[0].Rows[0]["now_time"].ToString();
                    sNowYear = dsServerDate.Tables[0].Rows[0]["now_year"].ToString();
                    sNowMonth = int.Parse(dsServerDate.Tables[0].Rows[0]["now_month"].ToString()).ToString("0#");
                    sNowWeek = int.Parse(dsServerDate.Tables[0].Rows[0]["now_week"].ToString()).ToString("0#");
                    sDate = DateTime.Parse(sNowTime).ToString("yyyy-MM-dd");
                    sSN = "";
                    sQYear = "";
                    sQMonth = "";
                    sQWeek = "";
                    sSNFormat = "";
                    for (int i = 0; i < dsSNFormat.Tables[0].Rows.Count; i++)
                    {
                        sType = "";
                        sName = "";
                        sAdjustType = "";
                        sAdjustValue = "";
                        sStartIndex = "";
                        sLength = "";
                        sFormat = "";
                        sParamer = "";
                        sType = dsSNFormat.Tables[0].Rows[i]["parameter_type"].ToString().Trim();
                        sName = dsSNFormat.Tables[0].Rows[i]["parameter"].ToString().Trim();
                        sAdjustType = dsSNFormat.Tables[0].Rows[i]["adjust_type"].ToString().Trim();
                        sAdjustValue = dsSNFormat.Tables[0].Rows[i]["adjust_value"].ToString().Trim();
                        sStartIndex = dsSNFormat.Tables[0].Rows[i]["start_index"].ToString().Trim();
                        sLength = dsSNFormat.Tables[0].Rows[i]["length"].ToString().Trim();
                        sFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                        if (sType.ToLower() == "input")
                        {
                            if (sName.ToLower() == "wo")
                            {
                                sSN = sSN + sWO;
                            }
                            if (sName.ToLower() == "protype")
                            {
                                sSN = sSN + sProType;
                            }
                            if (sName.ToLower() == "proid")
                            {
                                sSN = sSN + sProID;
                            }
                        }
                        else
                        {
                            if (sName.ToLower() == "year")
                            {
                                sParamer = sNowYear.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                }
                                sQYear = (int.Parse(sParamer) + nYear).ToString(sFormat);
                                sSN = sSN + sQYear;
                            }
                            else if (sName.ToLower() == "month")
                            {
                                sParamer = sNowMonth.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQMonth = (int.Parse(sParamer) + nMonth).ToString(sFormat);
                                sSN = sSN + sQMonth;
                            }
                            else if (sName.ToLower() == "week")
                            {
                                sParamer = sNowWeek.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQWeek = (int.Parse(sParamer) + nWeek).ToString(sFormat);
                                sSN = sSN + sQWeek;
                            }
                            else if (sName.ToLower() == "sn")
                            {
                                sSNFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                            }
                            else
                            {
                                sSN = sSN + dsSNFormat.Tables[0].Rows[i]["parameter_value"].ToString().Trim();
                            }
                        }
                    }
                    if (sLabelType == "单排压银")
                    {
                        for (int i = 0; i < nLabelQty; i++)
                        {
                            nNum = 0;
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and product_id='" + sProID + "' and year='" + sQYear + "' and week='" + sQWeek + "'";
                            DataSet dsNownum = db.Query(sql);
                            if (dsNownum.Tables[0].Rows.Count > 0)
                            {
                                nNum = int.Parse(dsNownum.Tables[0].Rows[0]["num"].ToString().Trim());
                            }
                            else
                            {
                                nNum = 0;
                            }
                            if (nNum + 1 < nSNstart)
                            {
                                nNum = nSNstart - 1;
                            }
                            if (nNum + 1 < nSNstart || nNum + 1 > nSNend)
                            {
                                MessageBox.Show("组件序号超过设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            sSN1 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintSchuecoLabel_A(sSN1);
                                }
                            }
                        }
                    }
                    if (sLabelType == "单排铜版")
                    {
                        for (int i = 0; i < nLabelQty; i++)
                        {
                            nNum = 0;
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and product_id='" + sProID + "' and year='" + sQYear + "' and week='" + sQWeek + "'";
                            DataSet dsNownum = db.Query(sql);
                            if (dsNownum.Tables[0].Rows.Count > 0)
                            {
                                nNum = int.Parse(dsNownum.Tables[0].Rows[0]["num"].ToString().Trim());
                            }
                            else
                            {
                                nNum = 0;
                            }
                            if (nNum + 1 < nSNstart)
                            {
                                nNum = nSNstart - 1;
                            }
                            if (nNum + 1 < nSNstart || nNum + 1 > nSNend)
                            {
                                MessageBox.Show("组件序号超过设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            sSN1 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,week)";
                            sql += " values('" + sSN1 + "','B'," + nNum + ",'" + sProID + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQWeek + "')";
                            nResult = db.ExecuteSql(sql);
                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintSchuecoLabel_B(sSN1);
                                }
                            }
                        }
                    }
                    break;

                #endregion
                #region
                case "Conergycommon":
                    sql = "select wo,product_type from sn_print";
                    sql += " where wo='" + sWO + "'";
                    sql += " group by wo,product_type";
                    DataSet dsProductType01 = db.Query(sql);
                    if (dsProductType01.Tables[0].Rows.Count > 0)
                    {
                        string sProductType = dsProductType01.Tables[0].Rows[0]["product_type"].ToString().Trim();
                        if (sProductType.ToUpper() != sProType.ToUpper())
                        {
                            MessageBox.Show("工单号[" + sWO + "]已对应产品型号[" + sProductType + "]，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    dsServerDate = GetServerDate();
                    dsSNFormat = GetSNFormatByCustomer("common");
                    if (dsSNFormat.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("客户[" + sCustomer + "]的组件序号编码规则没有设定，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sNowTime = dsServerDate.Tables[0].Rows[0]["now_time"].ToString();
                    sNowYear = dsServerDate.Tables[0].Rows[0]["now_year"].ToString();
                    sNowMonth = int.Parse(dsServerDate.Tables[0].Rows[0]["now_month"].ToString()).ToString("0#");
                    sNowWeek = int.Parse(dsServerDate.Tables[0].Rows[0]["now_week"].ToString()).ToString("0#");
                    sDate = DateTime.Parse(sNowTime).ToString("yyyy-MM-dd");
                    sSN = "";
                    sQYear = "";
                    sQMonth = "";
                    sQWeek = "";
                    sSNFormat = "";
                    #region
                    for (int i = 0; i < dsSNFormat.Tables[0].Rows.Count; i++)
                    {
                        sType = "";
                        sName = "";
                        sAdjustType = "";
                        sAdjustValue = "";
                        sStartIndex = "";
                        sLength = "";
                        sFormat = "";
                        sParamer = "";
                        sType = dsSNFormat.Tables[0].Rows[i]["parameter_type"].ToString().Trim();
                        sName = dsSNFormat.Tables[0].Rows[i]["parameter"].ToString().Trim();
                        sAdjustType = dsSNFormat.Tables[0].Rows[i]["adjust_type"].ToString().Trim();
                        sAdjustValue = dsSNFormat.Tables[0].Rows[i]["adjust_value"].ToString().Trim();
                        sStartIndex = dsSNFormat.Tables[0].Rows[i]["start_index"].ToString().Trim();
                        sLength = dsSNFormat.Tables[0].Rows[i]["length"].ToString().Trim();
                        sFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();

                        if (sType.ToLower() == "input")
                        {
                            if (sName.ToLower() == "wo")
                            {
                                sSN = sSN + sWO.Substring(0, 2) + sWO.Substring(sWO.Length - 3, 3);
                            }
                            if (sName.ToLower() == "protype")
                            {
                                sSN = sSN + sProType;
                            }
                            if (sName.ToLower() == "proid")
                            {
                                sSN = sSN + sProID;
                            }
                        }
                        else
                        {
                            if (sName.ToLower() == "year")
                            {
                                sParamer = sNowYear.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                }
                                sQYear = (int.Parse(sParamer) + nYear).ToString(sFormat);
                                sSN = sSN + sQYear;
                            }
                            else if (sName.ToLower() == "month")
                            {
                                sParamer = sNowMonth.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQMonth = (int.Parse(sParamer) + nMonth).ToString(sFormat);
                                sSN = sSN + sQMonth;
                            }
                            else if (sName.ToLower() == "week")
                            {
                                sParamer = sNowWeek.Substring(int.Parse(sStartIndex) - 1, int.Parse(sLength));
                                if (sAdjustType == "+")
                                {
                                    sParamer = (int.Parse(sParamer) + int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 + int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                if (sAdjustType == "-")
                                {
                                    sParamer = (int.Parse(sParamer) - int.Parse(sAdjustValue)).ToString();
                                    if (nYear >= 1)
                                    {
                                        sParamer = (1 - int.Parse(sAdjustValue)).ToString();
                                    }
                                }
                                sQWeek = (int.Parse(sParamer) + nWeek).ToString(sFormat);
                                sSN = sSN + sQWeek;
                            }
                            else if (sName.ToLower() == "sn")
                            {
                                sSNFormat = dsSNFormat.Tables[0].Rows[i]["format"].ToString().Trim();
                            }
                            else
                            {
                                sSN = sSN + dsSNFormat.Tables[0].Rows[i]["parameter_value"].ToString().Trim();
                            }
                        }
                    }
                    #endregion
                    #region
                    if (sLabelType == "单排")
                    {
                        lotNumbers = new string[nLabelQty];

                        for (int i = 0; i < nLabelQty; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='S' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','S'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);


                            lotNumbers[i] = sSN1;

                            PrintCommonConergyLabel_S(sSN1);
                        }
                    }
                    #endregion
                    #region
                    if (sLabelType == "双排")
                    {
                        if (nLabelQty % 2 != 0)
                        {
                            MessageBox.Show("打印标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        lotNumbers = new string[nLabelQty];

                        for (int i = 0; i < nLabelQty / 2; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='A' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','A'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN2 + "','A'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);


                            lotNumbers[i * 2] = sSN1;
                            lotNumbers[i * 2 + 1] = sSN2;

                            

                        }
                        PrintCommonConergyLabel_A(lotNumbers);
                    }
                    #endregion
                    #region
                    if (sLabelType == "三排")
                    {
                        if (nLabelQty % 3 != 0)
                        {
                            MessageBox.Show("打印标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        for (int i = 0; i < nLabelQty / 3; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='B' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN3 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            sSN3 = sSN + (nNum + 3).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','B'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN2 + "','B'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN3 + "','B'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);

                            //设定打印间隔 减少 打印机缓存
                            Thread.Sleep(DBUtility.nSleepTime);
                            #region
                            if (nResult > 0)
                            {
                                //PrintCommonConergyLabel_B(sSN1, sSN2, sSN3);
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region
                    if (sLabelType == "三排无BARCODE")
                    {
                        if (nLabelQty % 3 != 0)
                        {
                            MessageBox.Show("打印标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        for (int i = 0; i < nLabelQty / 3; i++)
                        {
                            sql = "select ISNULL(MAX(num),0) as 'num' from sn_print where label_type='C' and wo='" + sWO + "' and year='" + sQYear + "' and month='" + sQMonth + "'";
                            DataSet dsComP = db.Query(sql);
                            nNum = 0;
                            nNum = int.Parse(dsComP.Tables[0].Rows[0]["num"].ToString().Trim());
                            sSN1 = "";
                            sSN2 = "";
                            sSN3 = "";
                            sSN1 = sSN + (nNum + 1).ToString(sSNFormat);
                            sSN2 = sSN + (nNum + 2).ToString(sSNFormat);
                            sSN3 = sSN + (nNum + 3).ToString(sSNFormat);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN1 + "','C'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN2 + "','C'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);
                            nNum = nNum + 1;
                            sql = "insert into sn_print(sn,label_type,num,wo,product_type,print_date,year,month)";
                            sql += " values('" + sSN3 + "','C'," + nNum + ",'" + sWO + "','" + sProType + "','" + sDate + "','" + sQYear + "','" + sQMonth + "')";
                            nResult = db.ExecuteSql(sql);

                            //设定打印间隔 减少 打印机缓存
                            Thread.Sleep(DBUtility.nSleepTime);

                            if (nResult > 0)
                            {
                                if (rbtAgox.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtZero.Checked == true)
                                {
                                    PrintSerialno(sSN1, sSN2, sSN3);
                                }
                                else if (rbtCommon.Checked == true)
                                {
                                    PrintCommonLabel_C(sSN1, sSN2, sSN3);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void btnSEPrint_Click(object sender, EventArgs e)
        {
            BackgroundWorker woker = new BackgroundWorker();
            woker.DoWork += new DoWorkEventHandler(woker_DoWork);
            woker.RunWorkerAsync();
        }

        void woker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int nLabelQty = int.Parse(tePrintQty.Text.ToString());

                string prefix = tePrefix.Text.ToString().Trim();
                string date = teData.Text.ToString().Trim();
                string fatoryLine = cbbFactoryLine.Text.ToString().Trim();
                string bomCode = txtBOMCode.Text.ToString().Trim();
                string strStart = teStart.Text.ToString().Trim();

                //判断客户信息长度
                if (prefix.Length != 3)
                {
                    MessageBox.Show("请确认客户信息部分！");
                    return;
                }

                if (string.IsNullOrEmpty(date))
                {
                    MessageBox.Show("时间信息不能为空！");
                    return;
                }

                if (date.Length != 6)
                {
                    MessageBox.Show("请确认时间信息是否正确（长度异常）！");
                    return;
                }

                if (string.IsNullOrEmpty(fatoryLine))
                {
                    MessageBox.Show("线别信息不能为空！");
                    return;
                }

                //需添加 BOM代码 判断 及 流水码长度卡控
                if (fatoryLine == "3")
                {
                    if (string.IsNullOrEmpty(strStart))
                    {
                        MessageBox.Show(string.Format("流水码不能为空！", fatoryLine));
                        return;
                    }

                    if (strStart.Length != 4)
                    {
                        MessageBox.Show("请确流水码长度！");
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(bomCode))
                    {
                        MessageBox.Show(string.Format("选中线别【{}】,对应的BOM代码不能为空！", fatoryLine));
                        return;
                    }

                    if (bomCode.Length != 2)
                    {
                        MessageBox.Show("请确认BOM代码的长度！");
                        return;
                    }

                    if (string.IsNullOrEmpty(strStart))
                    {
                        MessageBox.Show(string.Format("流水码不能为空！", fatoryLine));
                        return;
                    }

                    if (strStart.Length != 2)
                    {
                        MessageBox.Show("请确流水码长度！");
                        return;
                    }

                    int maxNumber = nLabelQty + int.Parse(strStart);

                    if (maxNumber > 100)
                    {
                        MessageBox.Show("请确流水码最大不超过99！");
                        return;
                    }

                }

                int start = int.Parse(strStart);
                string i_return = string.Empty;

                IList<string> lstSN = new List<string>();
                for (int i = 0; i < nLabelQty; i++)
                {
                    if (fatoryLine == "3")
                    {
                        i_return = prefix + date + fatoryLine +
                                   string.Format("{0:0000}", start + i);
                    }
                    else
                    {
                        i_return = prefix + date + fatoryLine + bomCode +
                                   string.Format("{0:00}", start + i);
                    }

                    if (i_return.Length == 14)
                    {
                        if (rbtAgox.Checked == true)
                        {
                            //BartenderPrintSunEdisonSerialno(i_return);
                        }
                        else if (rbtZero.Checked == true)
                        {
                            lstSN.Add(i_return);
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintSunEdison_S(i_return);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请确认序列号信息！");
                    }
                }

                if (rbtZero.Checked == true)
                {
                    ZebraCodePrintSunEdisonSerialno(lstSN);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSERePrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbtAgox.Checked == true)
                {
                    //BartenderPrintSunEdisonSerialno(teSEReSN.Text.ToString().Trim());
                }
                else if (rbtZero.Checked == true)
                {
                    IList<string> lstSN = new List<string>();
                    if (teSEReSN.Text.ToString().Trim().Length == 14)
                    {
                        lstSN.Add(teSEReSN.Text.ToString().Trim());
                        ZebraCodePrintSunEdisonSerialno(lstSN);
                    }
                    else
                    {
                        MessageBox.Show("请确认条码信息！");
                    }
                }
                else if (rbtCommon.Checked == true)
                {
                    PrintSunEdison_S(teSEReSN.Text.ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void tabSNPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            string year = string.Empty;
            string month = string.Empty;
            string day = string.Empty;

            if (tabSNPrint.SelectedTab == tabSunEdison)
            {
                tePrefix.Text = "FPA";

                //year = DateTime.Now.Year.ToString().Substring(2, 2);
                //month = string.Format("{0:00}", int.Parse(DateTime.Now.Month.ToString()));
                //day = string.Format("{0:00}", int.Parse(DateTime.Now.Day.ToString()));
                //teData.Text = year + month + day;

                teData.Focus();

            }
        }

        private void cbbFactoryLine_SelectedValueChanged(object sender, EventArgs e)
        {
            string factoryLine = string.Empty;

            factoryLine = cbbFactoryLine.Text.ToString().Trim();
            if (factoryLine == "3")
            {
                teStart.Focus();
            }
            else
            {
                txtBOMCode.Focus();
            }

        }

        private void btnSECP_Click(object sender, EventArgs e)
        {
            if (rbtAgox.Checked == true)
            {

                //BartenderPrintSunEdisonSerialno("FXX14040200001");
            }
            else if (rbtZero.Checked == true)
            {
                IList<string> lstSN = new List<string>();
                lstSN.Add("FXX14040200001");
                ZebraCodePrintSunEdisonSerialno(lstSN);
            }
            else if (rbtCommon.Checked == true)
            {
                PrintSunEdison_S("FXX14040200001");
            }

        }

        private void btnSERePrintCP_Click(object sender, EventArgs e)
        {
            if (rbtAgox.Checked == true)
            {

                //BartenderPrintSunEdisonSerialno("FXX14040200001");
            }
            else if (rbtZero.Checked == true)
            {
                IList<string> lstSN = new List<string>();
                lstSN.Add("FXX14040200001");
                ZebraCodePrintSunEdisonSerialno(lstSN);
            }
            else if (rbtCommon.Checked == true)
            {
                PrintSunEdison_S("FXX14040200001");
            }
        }

        #region CodeSoft 模版打印

        private void PrintSerialno(string serialno)
        {
            LabelManager2.ApplicationClass lbl = new LabelManager2.ApplicationClass();

            try
            {
                string fileName = "CommonLableSingle.Lab";
                string fileFirstPath = AppDomain.CurrentDomain.BaseDirectory;
                string fileMiddlePath = string.Empty;

                if (rbtAgox.Checked == true)
                {
                    fileMiddlePath = @"Resources\Agox\";
                }
                else if (rbtZero.Checked == true)
                {
                    fileMiddlePath = @"Resources\Zero\";
                }

                string filePath = fileFirstPath + fileMiddlePath + fileName;

                if (File.Exists(filePath))
                {
                    lbl.Documents.Open(filePath, false);// 调用设计好的label文件
                    Document doc = lbl.ActiveDocument;
                    doc.Variables.FormVariables.Item("Var0").Value = serialno; //给参数传值

                    int Num = Convert.ToInt32(1);        //打印数量
                    doc.PrintDocument(Num);              //打印
                }
                else
                {
                    MessageBox.Show(string.Format("路径【{0}】不存在模版文件【{1}】", filePath, fileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lbl.Quit();                              //退出
            }
        }

        private void PrintSerialno(string serialno1, string serialno2)
        {
            LabelManager2.ApplicationClass lbl = new LabelManager2.ApplicationClass();

            try
            {
                string fileName = "CommonLableDouble.Lab";
                string fileFirstPath = AppDomain.CurrentDomain.BaseDirectory;
                string fileMiddlePath = string.Empty;

                if (rbtAgox.Checked == true)
                {
                    fileMiddlePath = @"Resources\Agox\";
                }
                else if (rbtZero.Checked == true)
                {
                    fileMiddlePath = @"Resources\Zero\";
                }

                string filePath = fileFirstPath + fileMiddlePath + fileName;

                if (File.Exists(filePath))
                {
                    lbl.Documents.Open(filePath, false);// 调用设计好的label文件
                    Document doc = lbl.ActiveDocument;
                    doc.Variables.FormVariables.Item("Var0").Value = serialno1; //给参数传值
                    doc.Variables.FormVariables.Item("Var1").Value = serialno2; //给参数传值

                    int Num = Convert.ToInt32(1);        //打印数量
                    doc.PrintDocument(Num);              //打印
                }
                else
                {
                    MessageBox.Show(string.Format("路径【{0}】不存在模版文件【{1}】", filePath, fileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lbl.Quit();                              //退出
            }

        }

        private void PrintSerialno(string serialno1, string serialno2, string serialno3)
        {
            LabelManager2.ApplicationClass lbl = new LabelManager2.ApplicationClass();

            try
            {
                string fileName = "CommonLableThree.Lab";
                string fileFirstPath = AppDomain.CurrentDomain.BaseDirectory;
                string fileMiddlePath = string.Empty;

                if (rbtAgox.Checked == true)
                {
                    fileMiddlePath = @"Resources\Agox\";
                }
                else if (rbtZero.Checked == true)
                {
                    fileMiddlePath = @"Resources\Zero\";
                }

                string filePath = fileFirstPath + fileMiddlePath + fileName;

                if (File.Exists(filePath))
                {
                    lbl.Documents.Open(filePath, false);// 调用设计好的label文件
                    Document doc = lbl.ActiveDocument;
                    doc.Variables.FormVariables.Item("Var0").Value = serialno1; //给参数传值
                    doc.Variables.FormVariables.Item("Var1").Value = serialno2; //给参数传值
                    doc.Variables.FormVariables.Item("Var2").Value = serialno3; //给参数传值

                    int Num = Convert.ToInt32(1);        //打印数量
                    doc.PrintDocument(Num);              //打印
                }
                else
                {
                    MessageBox.Show(string.Format("路径【{0}】不存在模版文件【{1}】", filePath, fileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lbl.Quit();                              //退出
            }

        }

        private void PrintSunEdisonSerialno(string serialno)
        {
            LabelManager2.ApplicationClass lbl = new LabelManager2.ApplicationClass();

            try
            {
                string fileName = "SunEdisonLable.Lab";
                string fileFirstPath = AppDomain.CurrentDomain.BaseDirectory;
                string fileMiddlePath = string.Empty;

                if (rbtAgox.Checked == true)
                {
                    fileMiddlePath = @"Resources\Agox\";
                }
                else if (rbtZero.Checked == true)
                {
                    fileMiddlePath = @"Resources\Zero\";
                }

                string filePath = fileFirstPath + fileMiddlePath + fileName;

                if (File.Exists(filePath))
                {
                    lbl.Documents.Open(filePath, false);// 调用设计好的label文件
                    Document doc = lbl.ActiveDocument;
                    doc.Variables.FormVariables.Item("Var0").Value = serialno; //给参数传值

                    int Num = Convert.ToInt32(1);        //打印数量
                    doc.PrintDocument(Num);              //打印
                }
                else
                {
                    MessageBox.Show(string.Format("路径【{0}】不存在模版文件【{1}】", filePath, fileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lbl.Quit();                              //退出
            }
        }

        #endregion


        #region Bartender Demo Print

        private void BartenderPrintSunEdisonSerialno(string serialno)
        {
            BarTender.Application btApp = new BarTender.Application();
            BarTender.Format btFormat = new BarTender.Format();

            try
            {
                string fileName = "SunEdisonLable.btw";
                string fileFirstPath = AppDomain.CurrentDomain.BaseDirectory;
                string fileMiddlePath = string.Empty;

                if (rbtAgox.Checked == true)
                {
                    fileMiddlePath = @"Resources\Agox\";
                }
                else if (rbtZero.Checked == true)
                {
                    fileMiddlePath = @"Resources\Zero\";
                }

                string filePath = fileFirstPath + fileMiddlePath + fileName;


                if (File.Exists(filePath))
                {
                    btFormat = btApp.Formats.Open(filePath, false, "");//加载文件摸板

                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                    btApp.Visible = true;

                    btFormat.PrintSetup.NumberSerializedLabels = 1;

                    btFormat.SetNamedSubStringValue("SN", serialno);
                    btFormat.PrintOut(false, false);//第2个参数是 是否显示打印机属性的。可以设置打印机路径
                }
                else
                {
                    MessageBox.Show(string.Format("路径【{0}】不存在模版文件【{1}】", filePath, fileName));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //关闭摸板文件，并且关闭文件流
                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
            }
        }

        #endregion

        #region Bartender Demo Print

        private void ZebraCodePrintSunEdisonSerialno(IList<string> lst)
        {
            NetPOSPrinter netPrint = new NetPOSPrinter();
            try
            {
                //打开打印机连接
                netPrint.IPPortOpen();
                //执行打印
                netPrint.PrintLine(lst);
                //关闭打印机连接
                netPrint.IPPortClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        #endregion

        private void btnPrintJS_Click(object sender, EventArgs e)
        {
            string _numPrint = string.Empty;
            string _numPrintSJ = string.Empty;
            string _numPrintSJ_01 = string.Empty;
            string _numPrintSJ_02 = string.Empty;
            string _facName = string.Empty;//工厂名
            string _timeYear = string.Empty;//年份后2位
            string _timeMouth = string.Empty;//月份
            string _timeMouthToWorld = string.Empty;//将月份转为A B C....
            string sLabelType = string.Empty;//标签类型
            int nSNstart = 0;
            int nSNend = 0;
            int nNownum = 0;
            int nResult = 0;
            string sql = string.Empty;
            int nQty = 0;
            string _const = "JSAO";
            if (cmbFacJS.Text.Trim() == "")
            {
                MessageBox.Show("请选择工厂！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbFacJS.SelectAll();
                cmbFacJS.Focus();
                return;
            }
            if (cmbdtJS.Text.Trim() == "")
            {
                MessageBox.Show("请选择时间！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbdtJS.Focus();
                return;
            }
            if (txtQtyJS.Text.Trim() == "")
            {
                MessageBox.Show("请输入数量！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQtyJS.SelectAll();
                txtQtyJS.Focus();
                return;
            }
            try
            {
                nQty = int.Parse(txtQtyJS.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQtyJS.SelectAll();
                txtQtyJS.Focus();
                return;
            }
            if (cmbTypeJS.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sLabelType = cmbTypeJS.Text.Trim();
            sql = "select * from sn_print_set where product_id='6610POR6612P' and product_type='TUV'";
            DataSet dsSNset = db.Query(sql);
            if (dsSNset.Tables[0].Rows.Count > 0)
            {
                nSNstart = int.Parse(dsSNset.Tables[0].Rows[0]["start_num"].ToString().Trim());
                nSNend = int.Parse(dsSNset.Tables[0].Rows[0]["end_num"].ToString().Trim());
            }
            else
            {
                MessageBox.Show("产品对应的组件序号范围未设定，请联系工艺！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _timeYear = Convert.ToDateTime(cmbdtJS.Value).Year.ToString();
            _timeMouth = Convert.ToDateTime(cmbdtJS.Value).Month.ToString();//月份

            int asciiForMouth = 65;
            for (int i = 1; i <= 12; i++)
            {
                if (_timeMouth.Equals(i.ToString()))
                {
                    _timeMouthToWorld = Convert.ToString((char)asciiForMouth);
                    break;
                }
                asciiForMouth++;
            }
            _facName = cmbFacJS.Text.Substring(cmbFacJS.Text.Length - 2, 1);
            _numPrint = _const + _timeMouthToWorld + _facName + _timeYear.Substring(_timeYear.Length - 2, 2);

            if (sLabelType == "单排")
            {
                for (int i = 0; i < nQty; i++)
                {
                    sql = string.Format(@"select ISNULL(MAX(num),0) as 'num' from sn_print 
                                            where label_type='S' 
                                            and product_id='6610POR6612P' 
                                            and product_type='TUV' 
                                            and sn like '{0}%'", _numPrint);
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    _numPrintSJ = _numPrint + (nNownum + 1).ToString().PadLeft(8, '0');
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ + "','S'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql); ;
                    if (nResult > 0)
                    {

                        if (rbtCommon.Checked == true)
                        {
                            PrintJapanSolar(_numPrintSJ);
                        }
                        else
                        {
                            MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            if (sLabelType == "双排")
            {
                for (int i = 0; i < nQty / 2; i++)
                {
                    sql = string.Format(@"select ISNULL(MAX(num),0) as 'num' from sn_print 
                                            where label_type='A' 
                                            and product_id='6610POR6612P' 
                                            and product_type='TUV' 
                                            and sn like '{0}%'", _numPrint);
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    _numPrintSJ_01 = _numPrint + (nNownum + 1).ToString().PadLeft(8, '0');
                    _numPrintSJ_02 = _numPrint + (nNownum + 2).ToString().PadLeft(8, '0');
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ_01 + "','A'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ_02 + "','A'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtCommon.Checked == true)
                        {
                            PrintJapanSolar_A(_numPrintSJ_01, _numPrintSJ_02);
                        }
                        else
                        {
                            MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            if (sLabelType == "三排")
            {
                for (int i = 0; i < nQty / 3; i++)
                {
                    sql = string.Format(@"select ISNULL(MAX(num),0) as 'num' from sn_print 
                                            where label_type='B' 
                                            and product_id='6610POR6612P' 
                                            and product_type='TUV' 
                                            and sn like '{0}%'", _numPrint);
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _numPrintSJ = _numPrint + (nNownum + 1).ToString().PadLeft(8, '0');
                    _numPrintSJ_01 = _numPrint + (nNownum + 2).ToString().PadLeft(8, '0');
                    _numPrintSJ_02 = _numPrint + (nNownum + 3).ToString().PadLeft(8, '0');
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ + "','B'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ_01 + "','B'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ_02 + "','B'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(_numPrintSJ, _numPrintSJ_01, _numPrintSJ_02);
                        }
                        else
                        {
                            MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            if (sLabelType == "三排无BARCODE")
            {
                for (int i = 0; i < nQty / 3; i++)
                {
                    sql = string.Format(@"select ISNULL(MAX(num),0) as 'num' from sn_print 
                                            where label_type='C' 
                                            and product_id='6610POR6612P' 
                                            and product_type='TUV' 
                                            and sn like '{0}%'", _numPrint);
                    DataSet dsNowNnum = db.Query(sql);
                    if (dsNowNnum.Tables[0].Rows.Count > 0)
                    {
                        nNownum = int.Parse(dsNowNnum.Tables[0].Rows[0]["num"].ToString().Trim());
                    }
                    else
                    {
                        nNownum = 0;
                    }
                    if (nNownum < nSNstart)
                    {
                        nNownum = nSNstart - 1;
                    }
                    if (nNownum + 1 < nSNstart || nNownum + 1 > nSNend || nNownum + 2 < nSNstart || nNownum + 2 > nSNend)
                    {
                        MessageBox.Show("组件序号超出工艺设定范围！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _numPrintSJ = _numPrint + (nNownum + 1).ToString().PadLeft(8, '0');
                    _numPrintSJ_01 = _numPrint + (nNownum + 2).ToString().PadLeft(8, '0');
                    _numPrintSJ_02 = _numPrint + (nNownum + 3).ToString().PadLeft(8, '0');
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ + "','C'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ_01 + "','C'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    nNownum = nNownum + 1;
                    sql = "insert into sn_print(sn,label_type,num,product_id,product_type,print_date,year,month)";
                    sql += " values('" + _numPrintSJ_02 + "','C'," + nNownum + ",'6610POR6612P','TUV','" + cmbdtJS.Value.ToString("yyyy-MM-dd") + "','" + _timeYear + "','" + _timeMouth + "')";
                    nResult = db.ExecuteSql(sql);
                    if (nResult > 0)
                    {
                        if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(_numPrintSJ, _numPrintSJ_01, _numPrintSJ_02);
                        }
                        else
                        {
                            MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }

        }

        private void PrintJapanSolar(string serialNumber)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(40 + int.Parse(txtX.Text.Trim()), 15 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), serialNumber);

            i_return = B_Prn_Text_TrueType(50 + int.Parse(txtStringX.Text.Trim()), 115 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", serialNumber);

            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void PrintJapanSolar_A(string sn1, string sn2)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), sn1);

            i_return = B_Prn_Text_TrueType(20 + int.Parse(txtStringX.Text.Trim()), 120 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Barcode(640 + int.Parse(txtX.Text.Trim()), 20 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), sn2);

            i_return = B_Prn_Text_TrueType(640 + int.Parse(txtStringX.Text.Trim()), 120 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

        private void btnJZjs_Click(object sender, EventArgs e)
        {
            PrintJSJz();
        }

        private void btnJZjsBu_Click(object sender, EventArgs e)
        {
            PrintJSJz();
        }

        private void PrintJSJz()
        {
            string sLabelType;
            sLabelType = cmbTypeJS.Text.Trim();
            if (sLabelType == "单排")
            {
                if (rbtCommon.Checked == true)
                {
                    PrintJapanSolar("0000000000000000");
                }
                else
                {
                    MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            else if (sLabelType == "双排")
            {
                if (rbtCommon.Checked == true)
                {
                    PrintJapanSolar_A("0000000000000000", "0000000000000000");
                }
                else
                {
                    MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (sLabelType == "三排")
            {
                if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_B("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else
                {
                    MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (sLabelType == "三排无BARCODE")
            {
                if (rbtCommon.Checked == true)
                {
                    PrintCommonLabel_C("0000000000000000", "0000000000000000", "0000000000000000");
                }
                else
                {
                    MessageBox.Show("请在首页确认打印方式！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBuPrintJS_Click(object sender, EventArgs e)
        {
            string sql;
            int nResult;

            if (txtStartNumJS.Text.Trim() == "" || txtEndNumJS.Text.Trim() == "")
            {
                MessageBox.Show("组件序列号为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStartNumJS.SelectAll();
                txtStartNumJS.Focus();
                return;
            }
            if (cmbTypeBuJS.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbTypeBuJS.Text.Trim() == "单排")
            {
                sql = "select sn from sn_print where label_type='S' and sn>='" + txtStartNumJS.Text.Trim() + "' and sn <='" + txtEndNumJS.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 1)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='S' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtCommon.Checked == true)
                        {
                            PrintJapanSolar(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStartNumJS.Text.Trim();
                    txtStartNumJS.Focus();
                    return;
                }
            }
            if (cmbTypeBuJS.Text.Trim() == "双排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtStartNumJS.Text.Trim() + "' and sn <='" + txtEndNumJS.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 2)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtCommon.Checked == true)
                        {
                            PrintJapanSolar_A(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStartNumJS.Text.Trim();
                    txtStartNumJS.Focus();
                    return;
                }
            }
            if (cmbTypeBuJS.Text.Trim() == "三排")
            {
                sql = "select sn from sn_print where label_type='B' and sn>='" + txtStartNumJS.Text.Trim() + "' and sn <='" + txtEndNumJS.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStartNumJS.SelectAll();
                    txtStartNumJS.Focus();
                    return;
                }
            }
            if (cmbTypeBuJS.Text.Trim() == "三排无BARCODE")
            {
                sql = "select sn from sn_print where label_type='C' and sn>='" + txtStartNumJS.Text.Trim() + "' and sn <='" + txtEndNumJS.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStartNumJS.SelectAll();
                    txtStartNumJS.Focus();
                    return;
                }
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            string sql = string.Format(@"select row_number() over (order by sn) as 序号,sn as 组件序列号,label_type as 标签类型,print_date as 打印日期 from sn_print where sn like 'JSAO%' ");
            if (!string.IsNullOrEmpty(cmbTypeSel.Text))
            {
                sql += string.Format(@"and label_type = '{0}'", cmbTypeSel.Text.Substring(0, 1));
            }
            if (!string.IsNullOrEmpty(txtstartLotNum.Text.Trim()))
            {
                sql += string.Format(@"and sn like '%{0}%'", txtstartLotNum.Text.Trim());
            }
            if (!string.IsNullOrEmpty(dateTimePicker1.Value.ToString()))
            {
                sql += string.Format(@"and print_date = '{0}'", Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy-MM-dd"));
            }
            sql += "order by sn desc";
            DataSet dsNowNnum = db.Query(sql);
            dgvShow.DataSource = dsNowNnum.Tables[0];
        }

        private void btnconergyprint_Click(object sender, EventArgs e)
        {
            btnComPrint.Enabled = false;

            string sql, sDate, sWO, sType, sLabelType, sSN1, sSN2, sSN3, sProductType;
            int nQty, nYear, nMonth, nNum, nResult;

            if (txtworkorder.Text.Trim() == "")
            {
                MessageBox.Show("工单号不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtworkorder.SelectAll();
                txtworkorder.Focus();
                return;
            }
            if (txtcode.Text.Trim() == "")
            {
                MessageBox.Show("产品型号代码不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtcode.SelectAll();
                txtcode.Focus();
                return;
            }
            try
            {
                nQty = int.Parse(txtQty.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印数量应为整数！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQty.SelectAll();
                txtQty.Focus();
                return;
            }
            if (cmbsntype.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sLabelType = cmbsntype.Text.Trim();

            #region//调用设定的编码规则生成SN
            sWO = txtworkorder.Text.Trim();
            sType = txtcode.Text.Trim();
            nYear = 0;
            nMonth = 0;
            if (txtYearUpdate.Text.Trim() != "")
            {
                nYear = int.Parse(txtYearUpdate.Text.Trim());
            }
            if (txtmouthUpdate.Text.Trim() != "")
            {
                nMonth = int.Parse(txtmouthUpdate.Text.Trim());
            }
            PrintSNByCustomer("Conergycommon", sWO, "", sType, nYear, nMonth, 0, nQty, sLabelType);
            #endregion

            btnComPrint.Enabled = true;
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            string sql;
            int nResult, nLabelQty;
            nLabelQty = 0;

            if (txtComSN.Text.Trim() == "" || txtComSN2.Text.Trim() == "")
            {
                MessageBox.Show("组件序列号为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComSN.SelectAll();
                txtComSN.Focus();
                return;
            }
            if (cboComRLT.Text.Trim() == "")
            {
                MessageBox.Show("请选择标签类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboComRLT.Text.Trim() == "单排")
            {
                sql = "select sn from sn_print where label_type='S' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty == 0)
                    {
                        MessageBox.Show("补打标签数量不能为0，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 1)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='S' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);


                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_S(dsReprint.Tables[0].Rows[i]["sn"].ToString());
                        }
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboComRLT.Text.Trim() == "双排")
            {
                sql = "select sn from sn_print where label_type='A' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty % 2 != 0)
                    {
                        MessageBox.Show("补打标签数量非2的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lotNumbers = new string[nLabelQty];

                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 2)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='A' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        lotNumbers[i] = dsReprint.Tables[0].Rows[i]["sn"].ToString();
                        lotNumbers[i + 1] = dsReprint.Tables[0].Rows[i + 1]["sn"].ToString();


                        //if (rbtAgox.Checked == true)
                        //{
                        //    PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        //}
                        //else if (rbtZero.Checked == true)
                        //{
                        //    PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        //}
                        //else if (rbtCommon.Checked == true)
                        //{
                        //    PrintCommonLabel_A(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString());
                        //}
                        //Thread.Sleep(DBUtility.nSleepTime);
                    }

                    PrintCommonLabel_A(lotNumbers);
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboComRLT.Text.Trim() == "三排")
            {
                sql = "select sn from sn_print where label_type='B' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty % 3 != 0)
                    {
                        MessageBox.Show("补打标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='B' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_B(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }

                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
            if (cboComRLT.Text.Trim() == "三排无BARCODE")
            {
                sql = "select sn from sn_print where label_type='C' and sn>='" + txtComSN.Text.Trim() + "' and sn <='" + txtComSN2.Text.Trim() + "' order by sn";
                DataSet dsReprint = db.Query(sql);
                if (dsReprint.Tables[0].Rows.Count > 0)
                {
                    nLabelQty = dsReprint.Tables[0].Rows.Count;
                    if (nLabelQty % 2 != 0)
                    {
                        MessageBox.Show("补打标签数量非3的倍数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < dsReprint.Tables[0].Rows.Count; i = i + 3)
                    {
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 1]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);
                        sql = "update sn_print set print_user='" + DBUtility.sUserId + "',reprint_num=isnull(reprint_num,0)+1 where label_type='C' and sn='" + dsReprint.Tables[0].Rows[i + 2]["sn"].ToString() + "'";
                        nResult = db.ExecuteSql(sql);

                        if (rbtAgox.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtZero.Checked == true)
                        {
                            PrintSerialno(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }
                        else if (rbtCommon.Checked == true)
                        {
                            PrintCommonLabel_C(dsReprint.Tables[0].Rows[i]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 1]["sn"].ToString(), dsReprint.Tables[0].Rows[i + 2]["sn"].ToString());
                        }


                        //Thread.Sleep(DBUtility.nSleepTime);
                    }
                }
                else
                {
                    MessageBox.Show("组件序号不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtComSN.SelectAll();
                    txtComSN.Focus();
                    return;
                }
            }
        }

        private void PrintCommonConergyLabel_S(string serialNumber)
        {
            int i_return, i_labqty, nDarkness, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            i_return = B_Prn_Barcode(40 + int.Parse(txtX.Text.Trim()), 66 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), serialNumber);

            i_return = B_Prn_Text_TrueType(105+ int.Parse(txtStringX.Text.Trim()), 15 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", serialNumber);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }
        /// <summary>
        /// 批量进行双排的打印
        /// </summary>
        /// <param name="lotNumbers">需要打印的条码的集合</param>
        private void PrintCommonConergyLabel_A(string[] lotNumbers)
        {
            int printed = 0;
            int i_return, i_labqty, nDarkness, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            n_Narrow = 4;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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

            while (printed < (lotNumbers.Length / 2))
            {
                int a = printed * 2;
                i_return = B_Prn_Barcode(40 + int.Parse(txtX.Text.Trim()), 66 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), lotNumbers[printed * 2]);
                int b = printed * 2;
                i_return = B_Prn_Text_TrueType(105 + int.Parse(txtStringX.Text.Trim()), 15 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A1", lotNumbers[printed *2]);
                int c = printed * 2;
                i_return = B_Prn_Barcode(660 + int.Parse(txtX.Text.Trim()), 66 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 8, 100, Convert.ToChar(78), lotNumbers[printed * 2 + 1]);
                int d = printed * 2 + 1;
                i_return = B_Prn_Text_TrueType(725 + int.Parse(txtStringX.Text.Trim()), 15 + int.Parse(txtStringY.Text.Trim()), 48, "Arial", 1, 500, 0, 0, 0, "A2", lotNumbers[printed * 2 + 1]);
                int e = printed * 2 + 1;
                i_return = B_Print_Out(1);

                printed += 1;
            }

            B_ClosePrn();//关闭打印

        }
        private void PrintCommonConergyLabel_B(string sn1, string sn2, string sn3)
        {
            int i_return, i_labqty, nDarkness, nSpeed, n_Narrow;
            string s_value;
            i_labqty = int.Parse(txtlabqty.Text.Trim());
            nDarkness = int.Parse(txtDarkness.Text.Trim());
            nSpeed = int.Parse(txtSpeed.Text.Trim());
            n_Narrow = 2;
            if (cboxNarrow.Checked == true)
            {
                try
                {
                    n_Narrow = int.Parse(txtNarrow.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("条码宽度必须为整数，请确认！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //打开打印接口
            i_return = B_CreatePrn(1, null);

            //设置打印浓度
            //i_return = B_Set_Darkness(14);
            i_return = B_Set_Darkness(nDarkness);

            //i_return = B_Set_Speed(nSpeed);

            //打印方向
            B_Set_Direction(Convert.ToChar(66));
            //B_Set_Direction('T');

            //清除内存图形
            i_return = B_Initial_Setting(0, "N\r\n\0");
            i_return = B_Del_Pcx("*");

            i_return = B_Prn_Barcode(20 + int.Parse(txtX.Text.Trim()), 110 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 80, Convert.ToChar(78), sn1);

            i_return = B_Prn_Text_TrueType(40 + int.Parse(txtStringX.Text.Trim()), 20 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A1", sn1);

            i_return = B_Prn_Barcode(20 + 380 + int.Parse(txtX.Text.Trim()), 110 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 80, Convert.ToChar(78), sn2);

            i_return = B_Prn_Text_TrueType(40 + 380 + int.Parse(txtStringX.Text.Trim()), 20 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A2", sn2);

            i_return = B_Prn_Barcode(20 + 760 + int.Parse(txtX.Text.Trim()), 110 + int.Parse(txtY.Text.Trim()), 0, "1", n_Narrow, 6, 80, Convert.ToChar(78), sn3);

            i_return = B_Prn_Text_TrueType(40 + 760 + int.Parse(txtStringX.Text.Trim()), 20 + int.Parse(txtStringY.Text.Trim()), 38, "Arial", 1, 500, 0, 0, 0, "A3", sn3);

            //i_return = B_Print_Out(i_labqty);//列印所有資料
            i_return = B_Print_Out(1);

            B_ClosePrn();//关闭打印
        }

    }
}
