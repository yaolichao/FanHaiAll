using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts.Printing;
using DevExpress.XtraNavBar;
using DevExpress.XtraPrinting;
//using DevExpress.XtraCharts.Wizard;
using DevExpress.XtraCharts;
using System.Data;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Addins.SPC {
    /// <summary>
    /// 图形控件导出助手。
    /// </summary>
    public class ChartControlExportHelper 
    {
        public static ChartControl chartControl;

        public static void CommExport(ChartControl inputChart, string exportType, DataTable inputDt)
        {
            inputChart.Titles[1].TextColor = Color.Blue;
            inputChart.Titles[1].Visible = true;
            inputChart.Titles[1].Text = "指标数据:";
            foreach (DataRow dr in inputDt.Rows)
            {
                inputChart.Titles[1].Text += ",(" + dr[COMMON_FIELDS.FIELD_COMMON_KEY].ToString();
                inputChart.Titles[1].Text += ":" + dr[COMMON_FIELDS.FIELD_COMMON_VALUE].ToString() + ")";
            }
            chartControl = inputChart;
            if (exportType.Equals(ExportType.Excel))
                ExportToXls();
            if (exportType.Equals(ExportType.Img))
                ExportToImage(ImageFormat.Jpeg);
            if (exportType.Equals(ExportType.PrintPreview))
                PrintPreview();
            inputChart.Titles[1].Visible = false;
        }

		public static void ExportToHtml() 
        {
            ExportTo(ExportType.TitleHtml, ExportType.FilterHtml,ExportType.ExportFormatHtml);
		}
		public static void ExportToMht() 
        {
			ExportTo(ExportType.TitleMht,ExportType.FilterMht, ExportType.ExportFormatMht);
		}
		public static void ExportToPdf() 
        {
			ExportTo(ExportType.TitlePdf, ExportType.FilterPdf, ExportType.ExportFormatPdf);
		}
		public static void ExportToXls() 
        {
			ExportTo(ExportType.TitleXLS,ExportType.FilterXLS, ExportType.ExportFormatXLS);
		}
		public static void ExportToImage(ImageFormat format) //ImageCodecInfo imageCodecInfo
        {
            ExportTo(ExportType.TitleImg, ExportType.FilterImg, ExportType.ExportFormatImg, format, false);
		}

        /// <summary>
        /// 预览
        /// </summary>
		public static void PrintPreview() 
        {
            ChartControl chart = chartControl;
			if (chart != null) {
				chart.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom;
				chart.ShowPrintPreview();
			}
		}

        
        /// <summary>
        /// 保存文件对话框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
		static string ShowSaveFileDialog(string title, string filter) {
			SaveFileDialog dlg = new SaveFileDialog();
			string name = Application.ProductName;
			int n = name.LastIndexOf(".") + 1;
			if (n > 0) name = name.Substring(n, name.Length - n);
			dlg.Title = ExportType.ExportTo + title;
			dlg.FileName = name;
			dlg.Filter = filter;
			if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
			return "";
		}

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="fileName"></param>
		static void OpenFile(string fileName) {
			if (XtraMessageBox.Show(ExportType.ConfirmOpenFiled, ExportType.ExportTo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				try {
					System.Diagnostics.Process process = new System.Diagnostics.Process();
					process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = ExportType.Open;
					process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
					process.Start();
				}
				catch {
					XtraMessageBox.Show(ExportType.AlertError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		static void ExportTo(string title, string filter, string exportFormat) {
			ExportTo(title, filter, exportFormat, null, true);
		}
        /// <summary>
        /// 导出各种类型文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <param name="exportFormat"></param>
        /// <param name="format"></param>
        /// <param name="checkPrinterAvailable"></param>
		static void ExportTo(string title, string filter, string exportFormat, ImageFormat format, bool checkPrinterAvailable) 
        {
            ChartControl chart = chartControl;
			if (chart == null)
				return;
			string fileName = ShowSaveFileDialog(title, filter);
			if (fileName != "") {
				Cursor currentCursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
                switch(exportFormat) {
                    case ExportType.ExportFormatHtml:
                        chart.ExportToHtml(fileName);
                        break;
                    case ExportType.ExportFormatMht:
                        chart.ExportToMht(fileName);
                        break;
                    case ExportType.ExportFormatPdf:
                        PrintSizeMode sizeMode = chart.OptionsPrint.SizeMode;
                        chart.OptionsPrint.SizeMode = PrintSizeMode.Zoom;
                        try {
                            chart.ExportToPdf(fileName);
                        }
                        finally {
                            chart.OptionsPrint.SizeMode = sizeMode;
                        }
                        break;
                    case ExportType.ExportFormatXLS:
                        chart.ExportToXls(fileName);
                        break;
                    case ExportType.ExportFormatImg:
                        chart.ExportToImage(fileName, format);
                        break;
                }
				Cursor.Current = currentCursor;
				OpenFile(fileName);
			}
		}
	}
}
