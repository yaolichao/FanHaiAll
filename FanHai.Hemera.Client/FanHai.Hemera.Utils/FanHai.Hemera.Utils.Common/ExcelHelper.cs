using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using org.in2bits.MyXls;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FanHai.Hemera.Utils.Common
{
    /// <summary>
    /// 【Description : Excel文件操作类(只支持xls，MyXls不支持xlsx格式)】
    /// 【Author      : Sean】
    /// 【Create Date : 2017-10-19】
    /// </summary> 
    public class ExcelHelper
    {
        #region Fields
        private const string ErrorFilePath = "Excel文件路径不能为空.";

        /// <summary>
        /// 作者
        /// </summary>
        public static string Author = "Astronergy";
        /// <summary>
        /// 主题
        /// </summary>
        public static string Subject = "";
        /// <summary>
        /// 备注
        /// </summary>
        public static string Comments = "Created By MyXls";
        /// <summary>
        /// 公司
        /// </summary>
        public static string Company = "";
        #endregion

        #region Methods
        private ExcelHelper() { }

        /// <summary>
        /// DataTable转Excel（SheetName使用对应DataTable的TableName）。 
        /// 不支持在已存在的Excel文件中添加新的sheet（如果文件已存在，则会覆盖）。
        /// </summary>
        /// <param name="dt">待写入Excel的DataTable对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式</param>
        public static void ToExcel(DataTable dt, string excelFilePath, bool isColumnWritten, CellStyle cellStyle)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            ToExcel(ds, excelFilePath, isColumnWritten, cellStyle);
        }
        /// <summary>
        /// DataSet转Excel（SheetName使用对应DataTable的TableName）。
        /// 不支持在已存在的Excel文件中添加新的sheet（如果文件已存在，则会覆盖）。
        /// </summary>
        /// <param name="ds">待写入Excel的DataSet对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式</param>
        public static void ToExcel(DataSet ds, string excelFilePath, bool isColumnWritten, CellStyle cellStyle)
        {
            if (string.IsNullOrEmpty(excelFilePath)) throw new ArgumentException(ErrorFilePath);
            if (ds == null || ds.Tables.Count <= 0) return;

            //创建Excel文档
            XlsDocument xls = new XlsDocument();
            xls.SummaryInformation.Author = Author;
            xls.SummaryInformation.Subject = Subject;
            xls.SummaryInformation.Comments = Comments;
            xls.DocumentSummaryInformation.Company = Company;

            foreach (DataTable dt in ds.Tables)
            {
                if (dt == null || dt.Rows.Count <= 0) continue;

                int cellRow = 1;//从第几行开始写数据
                string sheetName = GetValidSheetName(dt.TableName);

                if (string.IsNullOrEmpty(sheetName) || xls.Workbook.Worksheets.Any(worksheet => worksheet.Name == sheetName))
                    sheetName = string.Format("Sheet{0}", xls.Workbook.Worksheets.Count + 1);

                Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);
                sheet.SheetType = WorksheetTypes.Worksheet;

                Cells cells = sheet.Cells;

                //写标题
                if (isColumnWritten)
                {
                    #region 设置标题格式
                    XF xfDataHead = xls.NewXF();
                    if (cellStyle != null)
                    {
                        xfDataHead.HorizontalAlignment = cellStyle.TitleFontCenter ? HorizontalAlignments.Centered : HorizontalAlignments.Left;
                        xfDataHead.Font.FontName = cellStyle.TitleFontName;
                        xfDataHead.Font.Bold = cellStyle.TitleFontBold;
                        xfDataHead.Font.Height = cellStyle.TitleFontSize;
                        if (cellStyle.Border)
                        {
                            xfDataHead.UseBorder = true;
                            xfDataHead.TopLineStyle = 1;
                            xfDataHead.BottomLineStyle = 1;
                            xfDataHead.LeftLineStyle = 1;
                            xfDataHead.RightLineStyle = 1;
                        }
                        xfDataHead.CellLocked = false;
                        xfDataHead.UseProtection = false;
                        xfDataHead.UseNumber = true;
                        xfDataHead.UseMisc = true;
                    }
                    #endregion

                    for (int cellColumn = 1; cellColumn <= dt.Columns.Count; cellColumn++)
                    {
                        if (cellStyle != null)
                            cells.Add(cellRow, cellColumn, dt.Columns[cellColumn - 1].Caption, xfDataHead);
                        else
                            cells.Add(cellRow, cellColumn, dt.Columns[cellColumn - 1].Caption);
                    }
                    cellRow++;
                }

                #region 设置内容格式
                XF xfDataContent = xls.NewXF();
                if (cellStyle != null)
                {
                    xfDataContent.HorizontalAlignment = HorizontalAlignments.Left;
                    xfDataContent.Font.FontName = cellStyle.ContentFontName;
                    xfDataContent.Font.Height = cellStyle.ContentFontSize;
                    if (cellStyle.Border)
                    {
                        xfDataContent.UseBorder = true;
                        xfDataContent.TopLineStyle = 1;
                        xfDataContent.BottomLineStyle = 1;
                        xfDataContent.LeftLineStyle = 1;
                        xfDataContent.RightLineStyle = 1;
                    }
                    xfDataContent.CellLocked = false;
                    xfDataContent.UseProtection = false;
                    xfDataContent.UseNumber = true;
                    xfDataContent.UseMisc = true;
                }
                #endregion

                //写内容
                foreach (DataRow dr in dt.Rows)
                {
                    for (int cellColumn = 1; cellColumn <= dt.Columns.Count; cellColumn++)
                    {
                        if (cellStyle != null)
                        {
                            ColumnInfo colInfo = new ColumnInfo(xls, sheet)
                            {
                                ColumnIndexStart = (ushort)(cellColumn - 1),
                                ColumnIndexEnd = (ushort)(cellColumn - 1),
                                Width = cellStyle.ColumnWidth
                            };
                            sheet.AddColumnInfo(colInfo);
                        }

                        object obj = dr[cellColumn - 1] != DBNull.Value ? (dt.Columns[cellColumn - 1].DataType == typeof(DateTime)
                                                                            ? Convert.ToDateTime(dr[cellColumn - 1]).ToString("yyyy-MM-dd HH:mm:ss")
                                                                            : dr[cellColumn - 1])
                                                                        : string.Empty;
                        if (cellStyle != null)
                            cells.Add(cellRow, cellColumn, obj, xfDataContent);
                        else
                            cells.Add(cellRow, cellColumn, obj);
                    }
                    cellRow++;
                }
            }

            xls.FileName = Path.GetFileName(excelFilePath);
            xls.Save(Path.GetDirectoryName(excelFilePath), true);
            //xls.Send(XlsDocument.SendMethods.Attachment);//浏览器保存文件
        }

        /// <summary>
        /// 获取合法sheet名称
        /// </summary>
        /// <param name="sheetName">可能包含非法字符的sheet名称</param>
        /// <returns></returns>
        private static string GetValidSheetName(string sheetName)
        {
            if (!string.IsNullOrEmpty(sheetName))
            {
                //名称不多于31个字符
                if (sheetName.Length > 31)
                    sheetName = sheetName.Substring(0, 31);

                //名称不包含下列任一字符
                char[] invalidSheetNameChars = { ':', '\\', '/', '?', '*', '[', ']' };

                StringBuilder builder = new StringBuilder(sheetName);
                foreach (char invalidChar in invalidSheetNameChars)
                    builder.Replace(invalidChar, ' ');
                sheetName = builder.ToString();
            }
            return sheetName;
        }
        #endregion

        /// <summary>
        /// 获取Workbook
        /// </summary>
        /// <param name="path">Excel文件路径</param>
        /// <returns></returns>
        public static IWorkbook GetWorkbook(string path)
        {
            return File.Exists(path) ? GetOrCreateWorkbook(path, true) : null;
        }

        /// <summary>
        /// 创建Workbook
        /// </summary>
        /// <param name="path">Excel文件路径</param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook(string path)
        {
            return GetOrCreateWorkbook(path, false);
        }

        /// <summary>
        /// 保存（本地）
        /// </summary>
        /// <param name="workbook">Workbook对象</param>
        /// <param name="savePath">保存文件路径</param>
        public static void Save(IWorkbook workbook, string savePath)
        {
            if (workbook == null) return;

            using (var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
                workbook.Close();
            }
        }

        /// <summary>
        /// 保存（浏览器）
        /// </summary>
        /// <param name="workbook">Workbook对象</param>
        /// <param name="saveFileName">保存文件名称</param>
        //public static void SaveByBrowser(IWorkbook workbook, string saveFileName)
        //{
        //    if (workbook == null) return;

        //    using (var stream = new MemoryStream())
        //    {
        //        workbook.Write(stream);
        //        workbook.Close();

        //        HttpContext httpContext = HttpContext.Current;
        //        httpContext.Response.Clear();
        //        httpContext.Response.Buffer = true;
        //        //httpContext.Response.ContentType = "application/ms-excel";
        //        httpContext.Response.ContentEncoding = Encoding.UTF8;
        //        //httpContext.Response.Charset = "UTF-8";
        //        httpContext.Response.AddHeader("Content-Disposition", "attachment; filename={saveFileName}");
        //        httpContext.Response.BinaryWrite(stream.ToArray());
        //        httpContext.Response.End();
        //    }
        //}

        /// <summary>
        /// 获取或创建Workbook
        /// </summary>
        /// <param name="path">Excel文件路径</param>
        /// <param name="flag">true：获取；false：创建</param>
        /// <returns></returns>
        private static IWorkbook GetOrCreateWorkbook(string path, bool flag)
        {
            string extension = Path.GetExtension(path);
            try
            {
                FileStream fs = flag ? new FileStream(path, FileMode.Open, FileAccess.ReadWrite) : null;
                IWorkbook workbook = null;
                if (!string.IsNullOrEmpty(extension))
                {
                    switch (extension.ToLower())
                    {
                        case ".xls":
                            workbook = flag ? new HSSFWorkbook(fs) : new HSSFWorkbook(); //HSSF适用2007以前的版本
                            break;
                        case ".xlsx":
                            workbook = flag ? new XSSFWorkbook(fs) : new XSSFWorkbook(); //XSSF适用2007版本及其以上的
                            break;
                    }
                }
                fs.Close();
                return workbook;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }

    /// <summary>
    /// 单元格格式
    /// </summary>
    public class CellStyle
    {
        #region Fields
        /// <summary>
        /// 是否加边框（标题和内容）
        /// </summary>
        private bool _border = false;
        /// <summary>
        /// 标题字体是否居中
        /// </summary>
        private bool _titleFontCenter = false;
        /// <summary>
        /// 标题字体是否加粗
        /// </summary>
        private bool _titleFontBold = false;
        /// <summary>
        /// 标题字体名称
        /// </summary>
        private string _titleFontName = "Arial";
        /// <summary>
        /// 标题字体大小
        /// </summary>
        private ushort _titleFontSize = 10 * 20;
        /// <summary>
        /// 内容字体名称
        /// </summary>
        private string _contentFontName = "Arial";
        /// <summary>
        /// 内容字体大小
        /// </summary>
        private ushort _contentFontSize = 10 * 20;
        /// <summary>
        /// 列宽
        /// </summary>
        private ushort _columnWidth = 10 * 256;
        #endregion

        #region Properties
        /// <summary>
        /// 是否加边框（标题和内容）
        /// </summary>
        public bool Border
        {
            get { return _border; }
            set { _border = value; }
        }
        /// <summary>
        /// 标题字体是否居中
        /// </summary>
        public bool TitleFontCenter
        {
            get { return _titleFontCenter; }
            set { _titleFontCenter = value; }
        }
        /// <summary>
        /// 标题字体是否加粗
        /// </summary>
        public bool TitleFontBold
        {
            get { return _titleFontBold; }
            set { _titleFontBold = value; }
        }
        /// <summary>
        /// 标题字体名称
        /// </summary>
        public string TitleFontName
        {
            get { return _titleFontName; }
            set { _titleFontName = value; }
        }
        /// <summary>
        /// 标题字体大小
        /// </summary>
        public ushort TitleFontSize
        {
            get { return _titleFontSize; }
            set { _titleFontSize = (ushort)(value * 20); }
        }
        /// <summary>
        /// 内容字体名称
        /// </summary>
        public string ContentFontName
        {
            get { return _contentFontName; }
            set { _contentFontName = value; }
        }
        /// <summary>
        /// 内容字体大小
        /// </summary>
        public ushort ContentFontSize
        {
            get { return _contentFontSize; }
            set { _contentFontSize = (ushort)(value * 20); }
        }
        /// <summary>
        /// 列宽
        /// </summary>
        public ushort ColumnWidth
        {
            get { return _columnWidth; }
            set { _columnWidth = (ushort)(value * 256); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 设置自定义值
        /// </summary>
        public void SetCustomValue()
        {
            _border = true;
            _titleFontCenter = false;
            _titleFontBold = true;
            _titleFontName = "Tahoma";
            _titleFontSize = 10 * 20;
            _contentFontName = "Tahoma";
            _contentFontSize = 10 * 20;
            _columnWidth = 15 * 256;
        }
        #endregion
    }


}

#region Help
/*示例：
CellStyle cellStyle = new CellStyle();
cellStyle.SetCustomValue();
ExcelHelper.ToExcel(dt, excelFilePath, true, cellStyle);
MessageBox.Show("导出Excel结束.", "提示");*/
#endregion