using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.in2bits.MyXls;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

namespace Astronergy.MES.Report.DataAccess
{
    public class Export
    {

        /// <summary>
        /// 导出EXCEL数据到压缩文件。
        /// </summary>
        public static void ExportToCompressExcel(DataTable dt, string excelFilePath, string zipFileName)
        {
            try
            {
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName)))
                {
                    s.SetLevel(9); // 0 - store only to 9 - means best compression  压缩等级                
                    byte[] buffer = new byte[4096];
                    ZipEntry entry = new ZipEntry(Path.GetFileName(excelFilePath));
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);
                    using (FileStream fs = new FileStream(excelFilePath, FileMode.OpenOrCreate))
                    {
                        ExportToExcel(fs, dt);
                    }
                    using (FileStream fs = new FileStream(excelFilePath, FileMode.OpenOrCreate))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 导出数据到Excel中
        /// </summary>
        /// <param name="filePath">文件路径名称。</param>
        /// <param name="dt">数据表对象。</param>
        public static void ExportToExcel(System.IO.Stream stream, DataTable dt)
        {
            if (stream == null)
                return;
            if (dt == null)
                return;
            IWorkbook wb = new HSSFWorkbook();
            ExportToExcel(wb, dt);
            wb.Write(stream);
        }
        /// <summary>
        /// 导出数据到Excel中
        /// </summary>
        /// <param name="filePath">文件路径名称。</param>
        /// <param name="dt">数据表对象。</param>
        public static void ExportToExcel(string filePath, DataTable dt)
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            if (dt == null)
                return;
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                ExportToExcel(fs, dt);
            }
        }

        private static void ExportToExcel(IWorkbook wb, DataTable dt)
        {
            //设置EXCEL格式
            ICellStyle style = wb.CreateCellStyle();
            style.FillForegroundColor = 10;
            //有边框
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.Alignment = HorizontalAlignment.CENTER;
            IFont font = wb.CreateFont();
            font.Boldweight = 10;
            style.SetFont(font);

            ISheet ws = null;

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (j % 65535 == 0)
                {
                    ws = wb.CreateSheet();
                    IRow row = ws.CreateRow(0);
                    //循环放入列名
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dt.Columns[i].Caption);
                        cell.CellStyle = style;
                    }
                    font.Boldweight = 5;
                }
                DataRow dr = dt.Rows[j];
                IRow rowData = ws.CreateRow(j + 1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn dc = dt.Columns[i];
                    ICell cell = rowData.CreateCell(i);
                    if (dr[i] != DBNull.Value)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            DateTime dtValue = Convert.ToDateTime(dr[i]);
                            cell.SetCellValue(dtValue);
                        }
                        else if (dc.DataType == typeof(double)
                            || dc.DataType == typeof(int)
                            || dc.DataType == typeof(double)
                            || dc.DataType == typeof(float)
                            || dc.DataType == typeof(decimal)
                            || dc.DataType == typeof(uint)
                            || dc.DataType == typeof(long)
                            || dc.DataType == typeof(ulong))
                        {
                            cell.SetCellValue(Convert.ToDouble(dr[i]));
                        }
                        else if (dc.DataType == typeof(bool))
                        {
                            cell.SetCellValue(Convert.ToBoolean(dr[i]));
                        }
                        else
                        {
                            cell.SetCellValue(Convert.ToString(dr[i]));
                        }
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    cell.CellStyle = style;
                }
            }
        }


        /// <summary>
        /// 导出SE Daily Production数据到Excel中
        /// </summary>
        /// <param name="filePath">文件路径名称。</param>
        /// <param name="dtSource">数据表对象。</param>
        public static void ExportToSEDailyProductionExcel(System.IO.Stream stream, DataTable dtSource)
        {
            if (stream == null)
                return;
            if (dtSource == null)
                return;
            IWorkbook wb = new HSSFWorkbook();

            //设置EXCEL格式
            ICellStyle style = wb.CreateCellStyle();
            style.FillForegroundColor = 10;
            //有边框
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            IFont font = wb.CreateFont();
            font.Boldweight = 10;
            style.SetFont(font);
            //列头样式
            ICellStyle styleHeader = wb.CreateCellStyle();
            styleHeader.BorderBottom = BorderStyle.THIN;
            styleHeader.BorderLeft = BorderStyle.THIN;
            styleHeader.BorderRight = BorderStyle.THIN;
            styleHeader.BorderTop = BorderStyle.THIN;
            styleHeader.Alignment = HorizontalAlignment.CENTER;
            styleHeader.VerticalAlignment = VerticalAlignment.CENTER;
            styleHeader.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillPattern = FillPatternType.SOLID_FOREGROUND;
            IFont fontHeader = wb.CreateFont();
            fontHeader.Boldweight = (short)FontBoldWeight.BOLD;
            fontHeader.FontHeightInPoints = 12;
            styleHeader.SetFont(fontHeader);
            ISheet ws = null;

            for (int j = 0; j < dtSource.Rows.Count; j++)
            {
                if (j % 65535 == 0)
                {
                    ws = wb.CreateSheet("Daily production");

                    for (int k = 0; k < 3; k++)
                    {
                        IRow row = ws.CreateRow(k);

                        //循环放入列名
                        for (int i = 0; i < dtSource.Columns.Count; i++)
                        {
                            string colName = dtSource.Columns[i].ColumnName;
                            string[] colNameItems = colName.Split('_');
                            ICell cell = row.CreateCell(i);
                            cell.CellStyle = styleHeader;
                            int rowspan = 0;
                            int colspan = 0;
                            if (k < 2 && colNameItems[0] == "Day")
                            {
                                colspan = 2;
                                if (k == 0)
                                {
                                    cell.SetCellValue(colNameItems[1]);
                                    if (colNameItems[1].StartsWith("WK"))
                                    {
                                        colspan = 3;
                                        rowspan = 1;
                                        cell.SetCellValue(colNameItems[1] + " Total");
                                    }

                                    CellRangeAddress cellRangeAddress = new CellRangeAddress(k, k + rowspan, i, i + colspan);
                                    ws.AddMergedRegion(cellRangeAddress);
                                }
                                else if (k == 1)
                                {
                                    if (!colNameItems[1].StartsWith("WK"))
                                    {
                                        cell.SetCellValue(Enum.GetName(typeof(DayOfWeek), DateTime.Parse(colNameItems[1]).DayOfWeek));

                                        CellRangeAddress cellRangeAddress = new CellRangeAddress(k, k + rowspan, i, i + colspan);
                                        ws.AddMergedRegion(cellRangeAddress);
                                    }
                                }
                            }
                            else if (k == 0 && i == 0)
                            {
                                rowspan = 2;
                                cell.SetCellValue("Working station");
                                ws.SetColumnWidth(0, 20 * 256);
                                CellRangeAddress cellRangeAddress = new CellRangeAddress(k, k + rowspan, i, i + colspan);
                                ws.AddMergedRegion(cellRangeAddress);


                            }
                            else if (k >= 2 && i > 0)
                            {
                                cell.SetCellValue(colNameItems[0]);
                            }

                        }

                        //增加Comments
                        int commentCol = dtSource.Columns.Count;
                        ICell cellComments = row.CreateCell(dtSource.Columns.Count);
                        cellComments.CellStyle = styleHeader;
                        cellComments.SetCellValue("Comments");
                        ws.SetColumnWidth(commentCol, 60 * 256);
                        CellRangeAddress cellCommentsRangeAddress = new CellRangeAddress(0, 2, commentCol, commentCol);
                        ws.AddMergedRegion(cellCommentsRangeAddress);

                        font.Boldweight = 5;
                    }
                }


                DataRow dr = dtSource.Rows[j];
                IRow rowData = ws.CreateRow(j + 3);
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {

                    DataColumn dc = dtSource.Columns[i];
                    ICell cell = rowData.CreateCell(i);
                    string colName = dtSource.Columns[i].ColumnName;
                    string[] colNameItems = colName.Split('_');
                    int rowspan = 0;
                    int colspan = 0;
                    if (j == 5)
                    {
                        if (colNameItems[0] == "Day")
                        {
                            colspan = 1;
                            dr[i] = dtSource.Rows[j][i + 2];
                            CellRangeAddress cellRangeAddress = new CellRangeAddress(j + 3, j + 3 + rowspan, i, i + colspan);
                            ws.AddMergedRegion(cellRangeAddress);
                        }
                    }
                    else if (j >= 6 && j <= 7)
                    {
                        if (colNameItems[0] == "Day")
                        {
                            colspan = 2;
                            dr[i] = dtSource.Rows[j][i + 2];

                            if (j == 6 && colNameItems[1].StartsWith("WK"))
                            {
                                rowspan = 1;
                                dr[i] = dtSource.Rows[j + 1][i + 2];
                            }
                            CellRangeAddress cellRangeAddress = new CellRangeAddress(j + 3, j + 3 + rowspan, i, i + colspan);
                            ws.AddMergedRegion(cellRangeAddress);
                        }
                        else if (j == 6 && colNameItems[0] == "Gap" && colNameItems[1].StartsWith("WK"))
                        {
                            rowspan = 1;
                            dr[i] = "/";
                            CellRangeAddress cellRangeAddress = new CellRangeAddress(j + 3, j + 3 + rowspan, i, i + colspan);
                            ws.AddMergedRegion(cellRangeAddress);
                        }
                    }
                    if (dr[i] != DBNull.Value)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            DateTime dtValue = Convert.ToDateTime(dr[i]);
                            cell.SetCellValue(dtValue);
                        }
                        else if (dc.DataType == typeof(double)
                            || dc.DataType == typeof(int)
                            || dc.DataType == typeof(double)
                            || dc.DataType == typeof(float)
                            || dc.DataType == typeof(decimal)
                            || dc.DataType == typeof(uint)
                            || dc.DataType == typeof(long)
                            || dc.DataType == typeof(ulong))
                        {
                            cell.SetCellValue(Convert.ToDouble(dr[i]));
                        }
                        else if (dc.DataType == typeof(bool))
                        {
                            cell.SetCellValue(Convert.ToBoolean(dr[i]));
                        }
                        else
                        {
                            cell.SetCellValue(Convert.ToString(dr[i]));
                        }
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    cell.CellStyle = style;
                }
                ICell cellRowComments = rowData.CreateCell(dtSource.Columns.Count);
                cellRowComments.CellStyle = style;
                cellRowComments.SetCellValue(string.Empty);
            }
            //增加REMARK行
            int rowIndex = dtSource.Rows.Count + 3;
            IRow rowRemark = ws.CreateRow(rowIndex);
            rowRemark.Height = 50 * 20;
            for (int i = 0; i < dtSource.Columns.Count; i++)
            {
                DataColumn dc = dtSource.Columns[i];
                ICell cell = rowRemark.CreateCell(i);
                string colName = dtSource.Columns[i].ColumnName;
                string[] colNameItems = colName.Split('_');
                int rowspan = 0;
                int colspan = 0;
                if (i > 0 && colNameItems[0] == "Day")
                {
                    colspan = 2;
                    cell.SetCellValue(string.Empty);
                    if (colNameItems[1].StartsWith("WK"))
                    {
                        colspan = 3;
                    }
                    CellRangeAddress cellRangeAddress = new CellRangeAddress(rowIndex, rowIndex + rowspan, i, i + colspan);
                    ws.AddMergedRegion(cellRangeAddress);
                }
                else if (i == 0)
                {
                    cell.SetCellValue("Remark");
                }
                cell.CellStyle = style;
            }

            wb.Write(stream);
        }
        /// <summary>
        /// 导出SE VIBreakdown数据到Excel中
        /// </summary>
        /// <param name="filePath">文件路径名称。</param>
        /// <param name="dtSource">数据表对象。</param>
        public static void ExportToSEVIBreakdownExcel(System.IO.Stream stream, DataTable dtSource)
        {
            if (stream == null)
                return;
            if (dtSource == null)
                return;
            IWorkbook wb = new HSSFWorkbook();
            ISheet ws = null;
            DataView dv = dtSource.DefaultView;
            int j = 0;


            //内容单元格样式
            ICellStyle style = wb.CreateCellStyle();
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            IFont font = wb.CreateFont();
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            //标题样式
            ICellStyle styleTitle = wb.CreateCellStyle();
            styleTitle.BorderBottom = BorderStyle.THIN;
            styleTitle.BorderLeft = BorderStyle.THIN;
            styleTitle.BorderRight = BorderStyle.THIN;
            styleTitle.BorderTop = BorderStyle.THIN;
            styleTitle.Alignment = HorizontalAlignment.LEFT;
            styleTitle.VerticalAlignment = VerticalAlignment.CENTER;
            IFont fontTitle = wb.CreateFont();
            fontTitle.Boldweight = (short)FontBoldWeight.BOLD;
            fontTitle.FontHeightInPoints = 16;
            styleTitle.SetFont(fontTitle);
            //列头样式
            ICellStyle styleHeader = wb.CreateCellStyle();
            styleHeader.BorderBottom = BorderStyle.THIN;
            styleHeader.BorderLeft = BorderStyle.THIN;
            styleHeader.BorderRight = BorderStyle.THIN;
            styleHeader.BorderTop = BorderStyle.THIN;
            styleHeader.Alignment = HorizontalAlignment.CENTER;
            styleHeader.VerticalAlignment = VerticalAlignment.CENTER;
            styleHeader.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillPattern = FillPatternType.SOLID_FOREGROUND;
            IFont fontHeader = wb.CreateFont();
            fontHeader.Boldweight = (short)FontBoldWeight.BOLD;
            fontHeader.FontHeightInPoints = 12;
            styleHeader.SetFont(fontHeader);

            //有数值单元格样式。
            ICellStyle styleRed = wb.CreateCellStyle();
            styleRed.BorderBottom = BorderStyle.THIN;
            styleRed.BorderLeft = BorderStyle.THIN;
            styleRed.BorderRight = BorderStyle.THIN;
            styleRed.BorderTop = BorderStyle.THIN;
            styleRed.Alignment = HorizontalAlignment.CENTER;
            styleRed.VerticalAlignment = VerticalAlignment.CENTER;
            styleRed.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            styleRed.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            styleRed.FillPattern = FillPatternType.SOLID_FOREGROUND;
            IFont fontRed = wb.CreateFont();
            fontRed.Boldweight = (short)FontBoldWeight.BOLD;
            fontRed.FontHeightInPoints = 12;
            fontRed.Color = NPOI.HSSF.Util.HSSFColor.RED.index;
            styleRed.SetFont(fontRed);

            DataRowView drvPre = null;
            foreach (DataRowView drv in dv)
            {
                if (j % 65535 == 0)
                {
                    ws = wb.CreateSheet("VI Breakdown");

                    IRow rowTitle = ws.CreateRow(0);
                    ICell cellTitle = rowTitle.CreateCell(0);
                    cellTitle.SetCellValue("VI Failure Breakdown - Laminate & Module Level");
                    cellTitle.CellStyle = styleTitle;
                    CellRangeAddress cellRangeAddress = new CellRangeAddress(0, 2, 0, dtSource.Columns.Count);
                    ws.AddMergedRegion(cellRangeAddress);

                    IRow row = ws.CreateRow(3);
                    //循环放入列名
                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.CellStyle = styleHeader;
                        cell.SetCellValue(dtSource.Columns[i].Caption);
                        if (i == 1)
                        {
                            ws.SetColumnWidth(i, 20 * 256);
                        }
                        else
                        {
                            ws.SetColumnWidth(i, 15 * 256);
                        }
                    }
                    //增加Comments
                    int commentCol = dtSource.Columns.Count;
                    ICell cellComments = row.CreateCell(dtSource.Columns.Count);
                    cellComments.CellStyle = styleHeader;
                    cellComments.SetCellValue("Comments");
                    ws.SetColumnWidth(commentCol, 20 * 256);
                    j = j + 4;
                }
                IRow rowData = ws.CreateRow(j);
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    DataColumn dc = dtSource.Columns[i];
                    ICell cell = rowData.CreateCell(i);
                    bool isSetValue = false;
                    if (i == 0)
                    {
                        string type = Convert.ToString(drv["Type"]);
                        string preType = string.Empty;
                        if (drvPre != null)
                        {
                            preType = Convert.ToString(drvPre["Type"]);
                        }
                        //合并类型单元格。
                        if (preType != type)
                        {
                            int rowspan = dv.Table.Select(string.Format("Type='{0}'", type)).Count();
                            int firstRow = j;
                            int lastRow = j + rowspan - 1;
                            lastRow = lastRow < firstRow ? firstRow : lastRow;
                            CellRangeAddress cellRangeAddress = new CellRangeAddress(firstRow, lastRow, i, i);
                            ws.AddMergedRegion(cellRangeAddress);
                            isSetValue = true;
                        }
                    }
                    else
                    {
                        isSetValue = true;
                    }
                    cell.CellStyle = style;
                    if (drv[i] != DBNull.Value && isSetValue)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            DateTime dtValue = Convert.ToDateTime(drv[i]);
                            cell.SetCellValue(dtValue);
                        }
                        else if (dc.DataType == typeof(double)
                            || dc.DataType == typeof(int)
                            || dc.DataType == typeof(double)
                            || dc.DataType == typeof(float)
                            || dc.DataType == typeof(decimal)
                            || dc.DataType == typeof(uint)
                            || dc.DataType == typeof(long)
                            || dc.DataType == typeof(ulong))
                        {
                            double val = Convert.ToDouble(drv[i]);
                            cell.SetCellValue(val);
                            if (val > 0)
                            {
                                cell.CellStyle = styleRed;
                            }
                        }
                        else if (dc.DataType == typeof(bool))
                        {
                            cell.SetCellValue(Convert.ToBoolean(drv[i]));
                        }
                        else
                        {
                            cell.SetCellValue(Convert.ToString(drv[i]));
                        }
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    drvPre = drv;
                }
                ICell cellRowComments = rowData.CreateCell(dtSource.Columns.Count);
                cellRowComments.CellStyle = style;
                cellRowComments.SetCellValue(string.Empty);
                j++;
            }
            wb.Write(stream);
        }
        /// <summary>
        /// 导出SE ProductionYield数据到Excel中
        /// </summary>
        /// <param name="filePath">文件路径名称。</param>
        /// <param name="dtSource">数据表对象。</param>
        public static void ExportToSEProductionYieldExcel(System.IO.Stream stream, DataTable dtSource)
        {
            if (stream == null)
                return;
            if (dtSource == null)
                return;
            IWorkbook wb = new HSSFWorkbook();
            CreateProductionYieldSheet(wb, dtSource);
            CreateOverallYieldSheet(wb, dtSource);
            wb.Write(stream);
        }

        private static void CreateProductionYieldSheet(IWorkbook wb, DataTable dtSource)
        {
            ISheet ws = null;
            DataView dv = dtSource.DefaultView;
            int j = 0;
            //内容单元格样式
            ICellStyle style = wb.CreateCellStyle();
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            style.WrapText = true;
            IFont font = wb.CreateFont();
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            //标题样式
            ICellStyle styleTitle = wb.CreateCellStyle();
            styleTitle.BorderBottom = BorderStyle.THIN;
            styleTitle.BorderLeft = BorderStyle.THIN;
            styleTitle.BorderRight = BorderStyle.THIN;
            styleTitle.BorderTop = BorderStyle.THIN;
            styleTitle.Alignment = HorizontalAlignment.LEFT;
            styleTitle.VerticalAlignment = VerticalAlignment.CENTER;
            IFont fontTitle = wb.CreateFont();
            fontTitle.Boldweight = (short)FontBoldWeight.BOLD;
            fontTitle.FontHeightInPoints = 16;
            styleTitle.SetFont(fontTitle);
            //有良率单元格样式。
            ICellStyle styleYield = wb.CreateCellStyle();
            styleYield.BorderBottom = BorderStyle.THIN;
            styleYield.BorderLeft = BorderStyle.THIN;
            styleYield.BorderRight = BorderStyle.THIN;
            styleYield.BorderTop = BorderStyle.THIN;
            styleYield.Alignment = HorizontalAlignment.CENTER;
            styleYield.VerticalAlignment = VerticalAlignment.CENTER;
            //styleYield.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            //styleYield.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            //styleYield.FillPattern = FillPatternType.SOLID_FOREGROUND;
            styleYield.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
            //IFont fontYied = wb.CreateFont();/
            //fontRed.Boldweight = (short)FontBoldWeight.BOLD;
            //fontRed.FontHeightInPoints = 12;
            //fontRed.Color = NPOI.HSSF.Util.HSSFColor.RED.index;
            styleYield.SetFont(font);

            int colCount = dtSource.Columns.Count;
            ws = wb.CreateSheet("Production Yield");
            //增加标题行
            IRow rowTitle = ws.CreateRow(0);
            ICell cellTitle = rowTitle.CreateCell(0);
            cellTitle.SetCellValue("SOLAR LINE STATION YIELD");
            cellTitle.CellStyle = styleTitle;
            CellRangeAddress cellRangeTitle = new CellRangeAddress(j, j + 1, 0, colCount);
            ws.AddMergedRegion(cellRangeTitle);
            j = j + 2;
            //增加表头
            CreateProductionYieldTableHeader(ws, dtSource, j, j + 1);
            j = j + 2;
            //增加表行
            DataRowView drvPre = null;
            int cellCount = 0;
            foreach (DataRowView drv in dv)
            {
                int type = Convert.ToInt32(drv["Type"]);
                int preType = 0;
                if (drvPre != null)
                {
                    preType = Convert.ToInt32(drvPre["Type"]);
                }
                //创建备注类，新增表头
                if (type == 1 && preType != type)
                {
                    //增加备注行。
                    IRow rowRemarkData = ws.CreateRow(j);
                    cellCount = 0;
                    for (int i = 1; i < colCount; i++)
                    {
                        ICell cell = rowRemarkData.CreateCell(cellCount);
                        cell.CellStyle = style;
                        if (i == 1)
                        {
                            cell.SetCellValue("Remark");
                        }
                        else
                        {
                            cell.SetCellValue(string.Empty);
                        }
                        cellCount++;
                    }
                    j++;
                    //增加空白行
                    IRow rowSpaceData = ws.CreateRow(j);
                    j++;
                    //增加表头
                    CreateProductionYieldTableHeader(ws, dtSource, j, j + 1);
                    j = j + 2;
                }

                IRow rowData = ws.CreateRow(j);
                cellCount = 0;
                for (int i = 1; i < colCount; i++)
                {
                    DataColumn dc = dtSource.Columns[i];
                    ICell cell = rowData.CreateCell(cellCount);
                    bool isSetValue = false;
                    if (i == 1)
                    {
                        string station = Convert.ToString(drv["Station"]);
                        string preStation = string.Empty;
                        if (drvPre != null)
                        {
                            preStation = Convert.ToString(drvPre["Station"]);
                        }
                        //合并类型单元格。
                        if (station != preStation)
                        {
                            int rowspan = dv.Table.Select(string.Format("Station='{0}' AND Type={1}", station, type)).Count();
                            int firstRow = j;
                            int lastRow = j + rowspan - 1;
                            lastRow = lastRow < firstRow ? firstRow : lastRow;
                            CellRangeAddress cellRangeStation = new CellRangeAddress(firstRow, lastRow, i - 1, i - 1);
                            ws.AddMergedRegion(cellRangeStation);
                            CellRangeAddress cellRangeTarget = new CellRangeAddress(firstRow, lastRow, i, i);
                            ws.AddMergedRegion(cellRangeTarget);
                            isSetValue = true;
                        }
                    }
                    else
                    {
                        isSetValue = true;
                    }

                    cell.CellStyle = style;
                    if (drv[i] != DBNull.Value && isSetValue)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            DateTime dtValue = Convert.ToDateTime(drv[i]);
                            cell.SetCellValue(dtValue);
                        }
                        else if (dc.DataType == typeof(double)
                            || dc.DataType == typeof(int)
                            || dc.DataType == typeof(double)
                            || dc.DataType == typeof(float)
                            || dc.DataType == typeof(decimal)
                            || dc.DataType == typeof(uint)
                            || dc.DataType == typeof(long)
                            || dc.DataType == typeof(ulong))
                        {
                            double val = Convert.ToDouble(drv[i]);
                            cell.SetCellValue(val);
                            string itemName = Convert.ToString(drv["ItemName"]);
                            if (string.IsNullOrEmpty(itemName) || itemName == "Yield")
                            {
                                cell.CellStyle = styleYield;
                            }
                        }
                        else if (dc.DataType == typeof(bool))
                        {
                            cell.SetCellValue(Convert.ToBoolean(drv[i]));
                        }
                        else
                        {
                            cell.SetCellValue(Convert.ToString(drv[i]));
                        }
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    cellCount++;
                }
                ICell cellRowComments = rowData.CreateCell(cellCount);
                cellRowComments.CellStyle = style;
                cellRowComments.SetCellValue(string.Empty);
                drvPre = drv;
                j++;
            }
        }

        private static void CreateOverallYieldSheet(IWorkbook wb, DataTable dtSource)
        {
            ISheet ws = null;
            DataView dv = dtSource.DefaultView;
            dv.RowFilter = "Type>-1 AND Station in ('FPY rate','Manufacturing Yield')";
            int j = 0;
            //内容单元格样式
            ICellStyle style = wb.CreateCellStyle();
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            style.WrapText = true;
            IFont font = wb.CreateFont();
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            //标题样式
            ICellStyle styleTitle = wb.CreateCellStyle();
            styleTitle.BorderBottom = BorderStyle.THIN;
            styleTitle.BorderLeft = BorderStyle.THIN;
            styleTitle.BorderRight = BorderStyle.THIN;
            styleTitle.BorderTop = BorderStyle.THIN;
            styleTitle.Alignment = HorizontalAlignment.LEFT;
            styleTitle.VerticalAlignment = VerticalAlignment.CENTER;
            IFont fontTitle = wb.CreateFont();
            fontTitle.Boldweight = (short)FontBoldWeight.BOLD;
            fontTitle.FontHeightInPoints = 16;
            styleTitle.SetFont(fontTitle);
            //良率单元格样式。
            ICellStyle styleYield = wb.CreateCellStyle();
            styleYield.BorderBottom = BorderStyle.THIN;
            styleYield.BorderLeft = BorderStyle.THIN;
            styleYield.BorderRight = BorderStyle.THIN;
            styleYield.BorderTop = BorderStyle.THIN;
            styleYield.Alignment = HorizontalAlignment.CENTER;
            styleYield.VerticalAlignment = VerticalAlignment.CENTER;
            //styleYield.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            //styleYield.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            //styleYield.FillPattern = FillPatternType.SOLID_FOREGROUND;
            styleYield.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
            //IFont fontYied = wb.CreateFont();/
            //fontRed.Boldweight = (short)FontBoldWeight.BOLD;
            //fontRed.FontHeightInPoints = 12;
            //fontRed.Color = NPOI.HSSF.Util.HSSFColor.RED.index;
            styleYield.SetFont(font);

            int colCount = dtSource.Columns.Count;
            ws = wb.CreateSheet("Yield");
            //增加标题行
            IRow rowTitle = ws.CreateRow(0);
            ICell cellTitle = rowTitle.CreateCell(0);
            cellTitle.SetCellValue("Overall Flow Yield Status");
            cellTitle.CellStyle = styleTitle;
            CellRangeAddress cellRangeTitle = new CellRangeAddress(j, j + 1, 0, colCount - 3);
            ws.AddMergedRegion(cellRangeTitle);
            j = j + 2;
            //增加表头
            //列头样式
            ICellStyle styleHeader = wb.CreateCellStyle();
            styleHeader.BorderBottom = BorderStyle.THIN;
            styleHeader.BorderLeft = BorderStyle.THIN;
            styleHeader.BorderRight = BorderStyle.THIN;
            styleHeader.BorderTop = BorderStyle.THIN;
            styleHeader.Alignment = HorizontalAlignment.CENTER;
            styleHeader.VerticalAlignment = VerticalAlignment.CENTER;
            styleHeader.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillPattern = FillPatternType.SOLID_FOREGROUND;
            IFont fontHeader = wb.CreateFont();
            fontHeader.Boldweight = (short)FontBoldWeight.BOLD;
            fontHeader.FontHeightInPoints = 12;
            styleHeader.SetFont(fontHeader);
            IRow row = ws.CreateRow(j);
            //循环放入列名
            int cellCount = 0;
            for (int i = 0; i < colCount; i++)
            {
                if (i == 0 || i == 2 || i == 3)
                {
                    continue;
                }

                DataColumn dc = dtSource.Columns[i];
                ICell cell = row.CreateCell(cellCount);
                cell.CellStyle = styleHeader;
                if (dc.ColumnName.StartsWith("WK"))
                {
                    cell.SetCellValue(dc.Caption);
                    ws.SetColumnWidth(cellCount, 15 * 256);
                }
                else if (i > 2)
                {
                    cell.SetCellValue(DateTime.Parse(dc.ColumnName).ToString("MM.dd"));
                    ws.SetColumnWidth(cellCount, 15 * 256);
                }
                else
                {
                    cell.SetCellValue(string.Empty);
                    ws.SetColumnWidth(cellCount, 30 * 256);
                }
                cellCount++;
            }
            //增加Comments
            ICell cellComments = row.CreateCell(cellCount);
            cellComments.CellStyle = styleHeader;
            cellComments.SetCellValue("Remark");
            ws.SetColumnWidth(cellCount, 30 * 256);
            j = j + 1;
            //增加表行
            foreach (DataRowView drv in dv)
            {
                IRow rowData = ws.CreateRow(j);
                cellCount = 0;
                for (int i = 0; i < colCount; i++)
                {
                    if (i == 0 || i == 2 || i == 3)
                    {
                        continue;
                    }
                    DataColumn dc = dtSource.Columns[i];
                    ICell cell = rowData.CreateCell(cellCount);
                    cell.CellStyle = style;
                    if (drv[i] != DBNull.Value)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            DateTime dtValue = Convert.ToDateTime(drv[i]);
                            cell.SetCellValue(dtValue);
                        }
                        else if (dc.DataType == typeof(double)
                            || dc.DataType == typeof(int)
                            || dc.DataType == typeof(double)
                            || dc.DataType == typeof(float)
                            || dc.DataType == typeof(decimal)
                            || dc.DataType == typeof(uint)
                            || dc.DataType == typeof(long)
                            || dc.DataType == typeof(ulong))
                        {
                            double val = Convert.ToDouble(drv[i]);
                            cell.SetCellValue(val);
                            cell.CellStyle = styleYield;
                        }
                        else if (dc.DataType == typeof(bool))
                        {
                            cell.SetCellValue(Convert.ToBoolean(drv[i]));
                        }
                        else
                        {
                            cell.SetCellValue(Convert.ToString(drv[i]));
                        }
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    cellCount++;
                }
                ICell cellRowComments = rowData.CreateCell(cellCount);
                cellRowComments.CellStyle = style;
                cellRowComments.SetCellValue(string.Empty);
                j++;
            }
            //增加Target行。
            cellCount = 0;
            IRow rowTargetData = ws.CreateRow(j);
            for (int i = 0; i <= colCount; i++)
            {
                if (i == 0 || i == 2 || i == 3)
                {
                    continue;
                }
                ICell cell = rowTargetData.CreateCell(cellCount);
                cell.CellStyle = style;
                if (i == 1)
                {
                    cell.SetCellValue("Target FPY");
                }
                else
                {
                    cell.SetCellValue(string.Empty);
                }
                cellCount++;
            }
        }

        private static void CreateProductionYieldTableHeader(ISheet ws, DataTable dtSource, int startRow, int endRow)
        {
            IWorkbook wb = ws.Workbook;
            //列头样式
            ICellStyle styleHeader = wb.CreateCellStyle();
            styleHeader.BorderBottom = BorderStyle.THIN;
            styleHeader.BorderLeft = BorderStyle.THIN;
            styleHeader.BorderRight = BorderStyle.THIN;
            styleHeader.BorderTop = BorderStyle.THIN;
            styleHeader.Alignment = HorizontalAlignment.CENTER;
            styleHeader.VerticalAlignment = VerticalAlignment.CENTER;
            styleHeader.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_40_PERCENT.index;
            styleHeader.FillPattern = FillPatternType.SOLID_FOREGROUND;
            IFont fontHeader = wb.CreateFont();
            fontHeader.Boldweight = (short)FontBoldWeight.BOLD;
            fontHeader.FontHeightInPoints = 12;
            styleHeader.SetFont(fontHeader);
            int colCount = dtSource.Columns.Count;
            for (int k = startRow; k <= endRow; k++)
            {
                IRow row = ws.CreateRow(k);
                //循环放入列名
                for (int i = 1; i < colCount; i++)
                {
                    DataColumn dc = dtSource.Columns[i];
                    ICell cell = row.CreateCell(i - 1);
                    cell.CellStyle = styleHeader;
                    if (k == startRow && (i <= 3 || dc.ColumnName.StartsWith("WK")))
                    {
                        cell.SetCellValue(dc.Caption);
                        CellRangeAddress cellRangeTemp = new CellRangeAddress(k, k + 1, i - 1, i - 1);
                        ws.AddMergedRegion(cellRangeTemp);
                    }
                    else if (i > 3 && !dc.ColumnName.StartsWith("WK"))
                    {
                        cell.CellStyle = styleHeader;
                        if (k == startRow)
                        {
                            cell.SetCellValue(Enum.GetName(typeof(DayOfWeek),
                                                        DateTime.Parse(dc.ColumnName).DayOfWeek));
                        }
                        else
                        {
                            cell.SetCellValue(DateTime.Parse(dc.ColumnName).ToString("dd-MM"));
                        }
                    }

                    if (i == 1)
                    {
                        ws.SetColumnWidth(i - 1, 20 * 256);
                    }
                    else
                    {
                        ws.SetColumnWidth(i, 15 * 256);
                    }
                }

                if (k == startRow)
                {
                    //增加Comments
                    ICell cellComments = row.CreateCell(colCount - 1);
                    cellComments.CellStyle = styleHeader;
                    cellComments.SetCellValue("Comments");
                    ws.SetColumnWidth(colCount, 20 * 256);
                    CellRangeAddress cellRangeComments = new CellRangeAddress(k, k + 1, colCount - 1, colCount - 1);
                    ws.AddMergedRegion(cellRangeComments);
                }
            }
        }

        public static void ExportToConergyFlashDataExcel(System.IO.Stream stream, DataTable dtSource, string sheetName)
        {

            if (stream == null)
                return;
            if (dtSource == null)
                return;

            if (dtSource.Columns.Contains("InvoiceNumber"))
            {
                dtSource.Columns.Remove("InvoiceNumber");
            }

            IWorkbook wb = new HSSFWorkbook();


            //标题样式
            ICellStyle styleTitle = wb.CreateCellStyle();
            styleTitle.BorderBottom = BorderStyle.THIN;
            styleTitle.BorderLeft = BorderStyle.THIN;
            styleTitle.BorderRight = BorderStyle.THIN;
            styleTitle.BorderTop = BorderStyle.THIN;
            styleTitle.Alignment = HorizontalAlignment.CENTER;
            styleTitle.VerticalAlignment = VerticalAlignment.CENTER;
            styleTitle.FillForegroundColor = HSSFColor.GREY_40_PERCENT.index;
            styleTitle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            IFont fontTitle = wb.CreateFont();
            fontTitle.FontHeightInPoints = 10;
            styleTitle.SetFont(fontTitle);

            //设置EXCEL格式
            ICellStyle style = wb.CreateCellStyle();
            style.FillForegroundColor = 10;
            //有边框
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            IFont font = wb.CreateFont();
            font.Boldweight = 10;
            style.SetFont(font);

            ISheet ws = null;

            //新建Sheet工作表
            ws = wb.CreateSheet("Flash");
            //设定每一列的宽度
            ws.SetColumnWidth(0, 18 * 256);
            ws.SetColumnWidth(1, 18 * 256);
            ws.SetColumnWidth(2, 14 * 256);
            ws.SetColumnWidth(3, 18 * 256);
            ws.SetColumnWidth(4, 10 * 256);
            ws.SetColumnWidth(5, 17 * 256);
            ws.SetColumnWidth(6, 17 * 256);
            ws.SetColumnWidth(7, 18 * 256);
            ws.SetColumnWidth(8, 18 * 256);
            ws.SetColumnWidth(9, 8 * 256);
            ws.SetColumnWidth(10, 17 * 256);
            ws.SetColumnWidth(11, 15 * 256);
            ws.SetColumnWidth(12, 14 * 256);
            ws.SetColumnWidth(13, 17 * 256);
            ws.SetColumnWidth(14, 15 * 256);
            ws.SetColumnWidth(15, 15 * 256);
            ws.SetColumnWidth(16, 16 * 256);
            ws.SetColumnWidth(17, 15 * 256);
            ws.SetColumnWidth(18, 30 * 256);
            ws.SetColumnWidth(19, 18 * 256);
            ws.SetColumnWidth(20, 30 * 256);
            ws.SetColumnWidth(21, 16 * 256);
            ws.SetColumnWidth(22, 32 * 256);

            for (int j = 0; j < dtSource.Rows.Count; j++)
            {
                if (j % 65535 == 0)
                {
                    IRow row = ws.CreateRow(0);
                    //循环放入列名
                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dtSource.Columns[i].Caption);
                        cell.CellStyle = styleTitle;
                    }
                    font.Boldweight = 5;
                }
                DataRow dr = dtSource.Rows[j];
                IRow rowData = ws.CreateRow(j + 1);
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    DataColumn dc = dtSource.Columns[i];
                    ICell cell = rowData.CreateCell(i);
                    if (dr[i] != DBNull.Value)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            DateTime dtValue = Convert.ToDateTime(dr[i]);
                            cell.SetCellValue(dtValue.ToString("yyyy-MM-dd HH:mm"));
                        }
                        else if (dc.DataType == typeof(double)
                            || dc.DataType == typeof(int)
                            || dc.DataType == typeof(double)
                            || dc.DataType == typeof(float)
                            || dc.DataType == typeof(decimal)
                            || dc.DataType == typeof(uint)
                            || dc.DataType == typeof(long)
                            || dc.DataType == typeof(ulong))
                        {
                            cell.SetCellValue(Math.Round(Convert.ToDouble(dr[i]), 2).ToString(".00"));
                        }
                        else if (dc.DataType == typeof(bool))
                        {
                            cell.SetCellValue(Convert.ToBoolean(dr[i]));
                        }
                        else
                        {
                            cell.SetCellValue(Convert.ToString(dr[i]));
                        }
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    cell.CellStyle = style;
                }
            }

            wb.Write(stream);

        }
    }
}
