using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using Microsoft.Office.Interop.Excel;
using FanHai.Gui.Framework.Gui;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotIVTestPrint : BaseUserCtrl
    {
        public DataSet dsIVTest;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示

        public LotIVTestPrint()
        {
            InitializeComponent();
            WorkbenchSingleton.Workbench.ViewOpened += new ViewContentEventHandler(Workbench_ViewOpened);
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            GridViewHelper.SetGridView(gvTestData);
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.lblTitle}");//"测试打印作业";
            this.btnExcel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.btnExcel}");//"Excel导出";
            this.chDefault.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.chDefault}");//"有效值";
            this.btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.btnQuery}");//"查询";
            this.tsmSetDefult.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.tsmSetDefult}");//"设为有效数据";
            this.VC_CONTROLToolStripMenuItem.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.VC_CONTROLToolStripMenuItem}");//"更新卡控数据";
            this.T_DATE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.T_DATE}");//"测试时间";
            this.LOT_NUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.LOT_NUM}");//"组件序列号";
            this.VC_WORKORDER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.VC_WORKORDER}");//"工单号";
            this.AMBIENTTEMP.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.AMBIENTTEMP}");//"测试温度";
            this.INTENSITY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.INTENSITY}");//"光强";
            this.FF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.FF}");//"填充因子(%)";
            this.EFF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.EFF}");//"组件转换效率(%)";
            this.DEVICENUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.DEVICENUM}");//"设备编码";
            this.VC_PSIGN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.VC_PSIGN}");//"打印标志";
            this.DT_PRINTDT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.DT_PRINTDT}");//"打印时间";
            this.P_NUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.P_NUM}");//"打印次数";

            this.VC_DEFAULT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.VC_DEFAULT}");//"有效数据";
            this.SENSORTEMP.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.SENSORTEMP}");//"环境温度";
            this.VC_CUSTCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.VC_CUSTCODE}");//"客户代码";
            this.C_USERID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.C_USERID}");//"操作员";
            this.COEF_PMAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.COEF_PMAX}");//"衰减最大功率";

            this.COEF_ISC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.COEF_ISC}");//"衰减短路电流";
            this.COEF_VOC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.COEF_VOC}");//"衰减开路电压";
            this.COEF_IMAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.COEF_IMAX}");//"衰减最大工作电流";
            this.COEF_VMAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.COEF_VMAX}");//"衰减最大工作电压";
            this.COEF_FF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.COEF_FF}");//"衰减填充因子";
            this.VC_CELLEFF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.VC_CELLEFF}");//"电池片效率";
            this.DEC_CTM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.DEC_CTM}");//"CTM转换";
            this.CALIBRATION_NO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.CALIBRATION_NO}");//"校准板编号";

            this.Imp_Isc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.Imp_Isc}");//"Imp/Isc(电流比)";
            this.CONTROL_VALUE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.CONTROL_VALUE}");//"比例";
            //this.lciQueryResults.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.lciQueryResults}");//"查询结果";
            this.lblWorkOrder.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.lblWorkOrder}");//"组件序列号";
            this.lciStartTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.lciStartTime}");//"测试日期";
            this.lciEndTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.lciEndTime}");//"创建时间-止";
            this.lblLotNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.lblLotNumber}");//"批次号";
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.layoutControlItem5}");//"测试设备号";
            this.btnPrint.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.btnPrint}");//"打印标签/铭牌";
        }




        void Workbench_ViewOpened(object sender, ViewContentEventArgs e)
        {
            WorkbenchSingleton.Workbench.ViewOpened -= Workbench_ViewOpened;
            //btnPrint_Click(null, null);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //LotIVTestPrintCommand2 LotIVTestPrintCommand2 = new LotIVTestPrintCommand2();
            //LotIVTestPrintCommand2.Run();
            LotIVTestPrintDialog frmPrintDialog = new LotIVTestPrintDialog();
            frmPrintDialog.ShowDialog();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sSLotNum, sELotNum, sSTestTime, sETestTime, sSDevice, sEDevice, sDefult, sVC_CONTROL, sWorkNum;

            sSLotNum = txtSLotNum.Text.Trim();
            sELotNum = txtELotNum.Text.Trim();
            sSTestTime = deStartTime.Text;
            sETestTime = deEndTime.Text;
            sSDevice = txtDeviceS.Text.Trim();
            sEDevice = txtDeviceE.Text.Trim();
            sDefult = "";
            sVC_CONTROL = "";
            sWorkNum = "";
            if (chDefault.Checked == true)
            {
                sDefult = "1";
            }

            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            dsIVTest = IVTestDateObject.GetIVTestData2(sWorkNum,sSLotNum, sELotNum, sSDevice, sEDevice, sSTestTime, sETestTime, sDefult, sVC_CONTROL);
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                gcTestData.DataSource = null;
                gcTestData.MainView = gvTestData;
                gcTestData.DataSource = dsIVTest.Tables[0];
                gvTestData.BestFitColumns();//自动调整列宽度
                gvTestData.IndicatorWidth = 50;//自动调整行容器宽度
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }
        }

        private void tsmSetDefult_Click(object sender, EventArgs e)
        {
            string sRowKey = string.Empty;
            string sLotNum = string.Empty;
            string sql;
            
            if (gvTestData.FocusedRowHandle < 0 || gvTestData.RowCount < 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.Msg001}"), MESSAGEBOX_CAPTION);//请选择编辑的数据!
                //MessageService.ShowMessage("请选择编辑的数据!", "提示");
                return;
            }

            DataRow drEdit = gvTestData.GetFocusedDataRow();
            sRowKey = drEdit["IV_TEST_KEY"].ToString().Trim();
            sLotNum = drEdit["LOT_NUM"].ToString().Trim();
            if (!string.IsNullOrEmpty(sRowKey) && !string.IsNullOrEmpty(sLotNum))
            {
                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();

                sql = "UPDATE WIP_IV_TEST SET VC_DEFAULT='0' WHERE VC_DEFAULT='1' AND LOT_NUM='" + sLotNum + "'";
                DataSet dsSetNDefult= IVTestDateObject.UpdateData(sql, "UpdateIVTestData");
                int nSetNDefult = int.Parse(dsSetNDefult.ExtendedProperties["rows"].ToString());
                //if (nSetNDefult < 1)
                //{
                //    MessageService.ShowMessage("无数据更新！", "提示");
                //    return;
                //}
                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    MessageService.ShowError(IVTestDateObject.ErrorMsg);
                    return;
                }

                sql = "UPDATE WIP_IV_TEST SET VC_DEFAULT='1' WHERE IV_TEST_KEY='" + sRowKey + "'";
                DataSet dsSetDefult = IVTestDateObject.UpdateData(sql, "UpdateIVTestData");
                int nSetDefult = int.Parse(dsSetDefult.ExtendedProperties["rows"].ToString());
                if (nSetDefult < 1)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.Msg002}"), MESSAGEBOX_CAPTION);//无数据更新！
                    //MessageService.ShowMessage("无数据更新！", "提示");
                    return;
                }
                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    MessageService.ShowError(IVTestDateObject.ErrorMsg);
                    return;
                }

            }
        }

        private void LotIVTestPrint_Load(object sender, EventArgs e)
        {
            tsmSetDefult.Visible = true;
            deStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            deEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            string privilegeStr = PropertyService.Get("UserPrivilege");
            //if (privilegeStr.IndexOf(privilegeCode) == -1)
            //{
                
            //}
            //else
            //{
                
            //} 
            VC_CONTROLToolStripMenuItem.Enabled = false;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            int nColumn, nRow;

            if (gvTestData.RowCount > 0)
            {
                try
                {
                    nColumn = gvTestData.Columns.Count;
                    nRow = gvTestData.RowCount;

                    Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                    oExcel.Visible = false;
                    Microsoft.Office.Interop.Excel.Workbook oWorkbook = oExcel.Workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet oWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)oWorkbook.Worksheets[1];
                    //oWorksheet.Name = txtStockNo.Text.Trim();
                    for (int c = 1; c < nColumn; c++)
                    {
                        oWorksheet.Cells[1, c] = gvTestData.Columns[c].Caption.ToString().Trim();
                    }
                    for (int r = 0; r < nRow; r++)
                    {
                        for (int c = 1; c < nColumn; c++)
                        {
                            if (c == 2 || c == 19)
                            {
                                oWorksheet.Cells[r + 2, c] = "'" + dsIVTest.Tables[0].Rows[r][c].ToString();
                            }
                            else
                            {
                                oWorksheet.Cells[r + 2, c] = dsIVTest.Tables[0].Rows[r][c].ToString();
                            }
                        }
                    }
                    nRow++;
                    oWorksheet.get_Range("A1", "AE1").Interior.ColorIndex = 48;
                    oWorksheet.get_Range("A1", "AE" + nRow.ToString()).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    oWorksheet.Cells.get_Range("A1", "AE" + nRow.ToString()).Borders.LineStyle = 1;
                    //oWorksheet.get_Range("N1", "N" + nRow.ToString()).EntireColumn.NumberFormat = "yyyy-MM-dd";
                    oWorksheet.get_Range("B1", "B" + nRow.ToString()).EntireColumn.NumberFormat = "@";
                    oWorksheet.get_Range("S1", "S" + nRow.ToString()).EntireColumn.NumberFormat = "@";
                    //oWorksheet.get_Range("G1", "L" + nRow.ToString()).EntireColumn.NumberFormat = "##0.00";
                    //oWorksheet.Cells.Font.Name = "Verdana";
                    //oWorksheet.Cells.Font.Size = 10;
                    //oWorksheet.Cells.AutoFit();
                    oExcel.Visible = true;
                    oExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
                    System.GC.Collect();
                }
                catch //(Exception ex)
                {
                    
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrint.Msg003}"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);//创建Excel失败，请确认是否有安装Excel应用程序！
                    return;
                }
            }
        }

        private void LotIVTestPrint_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSLotNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtELotNum.SelectAll();
                txtELotNum.Focus();
            }
        }

        private void txtELotNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                deStartTime.Focus();
            }
        }

        private void txtDeviceS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtDeviceE.SelectAll();
                txtDeviceE.Focus();
            }
        }

        private void txtDeviceE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnQuery.Focus();
            }
        }

        //private void gvTestData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        //{
        //    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;//行号居中
        //    if (e.Info.IsRowIndicator)
        //    {
        //        if (e.RowHandle >= 0)
        //        {
        //            e.Info.DisplayText = (e.RowHandle + 1).ToString();//添加行号
        //        }
        //        else if (e.RowHandle < 0 && e.RowHandle > -1000)
        //        {
        //            e.Info.Appearance.BackColor = System.Drawing.Color.AntiqueWhite;
        //            e.Info.DisplayText = "G" + e.RowHandle.ToString();
        //        }
        //    }
        //}

        private void gcTestData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //int hand = e.RowHandle;
            //if (hand < 0) return;
            ////DataRow dr = this.gcTestData(hand);
            ////if (dr== null) return;
            ////decimal Isc = Convert.ToDecimal(this.gvTestData.GetRowCellValue(hand, "Isc"));

            //if (hand == 10)
            //{
            //    DataRow Dr = gvTestData.GetDataRow(hand);
            //    e.Appearance.ForeColor = Color.Red;// 改变行字体颜色
            //    //e.Appearance.BackColor = Color.Red;// 改变行背景颜色
            //    e.Appearance.BackColor2 = Color.Blue;// 添加渐变颜色
            //}
        }

        private void VC_CONTROLToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
