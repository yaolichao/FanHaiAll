using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using org.in2bits.MyXls;
using org.in2bits.MyXls.ByteUtil;

namespace FanHai.Hemera.Utils.Common
{
    public class Export
    {
        /// <summary>
        /// 导出数据到Excel中
        /// </summary>
        /// <param name="filePath">文件路径名称。</param>
        /// <param name="dt">数据表对象。</param>
        public static void ExportToExcel(string filePath,DataTable dt)
        {
             if (string.IsNullOrEmpty(filePath))
                 return;
             if (dt == null)
                 return;
             //创建Excel文档。
             XlsDocument xls = new XlsDocument();
             Worksheet sheet = xls.Workbook.Worksheets.Add(dt.TableName);
             sheet.SheetType = WorksheetTypes.Worksheet;

             Cells cells = sheet.Cells;
             #region 设置Excel数据列标题的格式
             XF xfDataHead = xls.NewXF();
             xfDataHead.HorizontalAlignment = HorizontalAlignments.Left;
             xfDataHead.Font.FontName = "Tahoma";
             xfDataHead.Font.Bold = true;
             xfDataHead.UseBorder = true;
             xfDataHead.BottomLineStyle = 1;
             xfDataHead.TopLineStyle = 1;
             xfDataHead.LeftLineStyle = 1;
             xfDataHead.RightLineStyle = 1;
             xfDataHead.CellLocked = false;
             xfDataHead.UseProtection = false;
             xfDataHead.UseNumber = true;
             #endregion
             //循环放入列名
             for (int i = 1; i <= dt.Columns.Count; i++)
             {
                 ColumnInfo colInfo = new ColumnInfo(xls, sheet);
                 colInfo.ColumnIndexStart = (ushort)(i - 1);
                 colInfo.ColumnIndexEnd = (ushort)i;
                 colInfo.Width = 15 * 256;
                 sheet.AddColumnInfo(colInfo);
                 cells.Add(1, i, dt.Columns[i - 1].Caption, xfDataHead);
             }
             #region 设置各数据列的格式
             XF xfData = xls.NewXF();
             xfData.UseBorder = true;
             xfData.CellLocked = false;
             xfData.BottomLineStyle = 1;
             xfData.TopLineStyle = 1;
             xfData.LeftLineStyle = 1;
             xfData.RightLineStyle = 1;
             xfData.UseProtection = false;
             xfData.UseMisc = true;
             xfData.UseNumber = true;
             xfData.HorizontalAlignment = HorizontalAlignments.Left;
             #endregion
             //循环放入所有数据
             int iRow = 2;
             foreach (DataRow dr in dt.Rows)
             {
                 for (int iCol = 1; iCol <= dt.Columns.Count; iCol++)
                 {
                     DataColumn dc = dt.Columns[iCol - 1];
                     if (dr[iCol - 1] != DBNull.Value)
                     {
                         if (dc.DataType == typeof(DateTime))
                         {
                             DateTime dtValue = Convert.ToDateTime(dr[iCol - 1]);
                             //向单元格中插入数据
                             cells.Add(iRow, iCol, dtValue.ToString("yyyy-MM-dd HH:mm:ss"), xfData);
                         }
                         else
                         {
                             //向单元格中插入数据
                             cells.Add(iRow, iCol, dr[iCol - 1], xfData);
                         }
                     }
                     else
                     {
                         cells.Add(iRow, iCol, string.Empty, xfData);
                     }
                 }
                 iRow++;
             }
             xls.FileName = System.IO.Path.GetFileName(filePath);
             string fileWay = System.IO.Path.GetDirectoryName(filePath);
             xls.Save(fileWay, true);
        }

    }
}
