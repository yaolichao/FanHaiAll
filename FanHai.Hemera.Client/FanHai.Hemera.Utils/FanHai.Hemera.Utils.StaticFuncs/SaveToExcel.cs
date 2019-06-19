using System;
using System.Collections.Generic;
using System.Text;
//引入命名空间
//using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Diagnostics;
using org.in2bits.MyXls;
using org.in2bits.MyXls.ByteUtil;

namespace FanHai.Hemera.Utils.StaticFuncs
{
    public class SaveToExcel
    {
        /// <summary>
        /// 保存信息到Excel中
        /// </summary>
        /// <param name="table">保存在Datatable中的数据对象</param>
        /// <param name="name">想要保存为的文件名</param>
        /// <param name="saveWay">保存路径</param>
        /// <param name="tableHead">标题。</param>
        /// <param name="title">每列显示的内容名数组</param>
        public static string SaveExcel(System.Data.DataTable table, string name,
                                       string saveWay, string tableHead,
                                       string[] title)
        {
            List<string> lst = new List<string>();
            lst.Add("PARAM_VALUE");
            lst.Add("AV_VALUE");
            return SaveExcel(table, name, saveWay, title, lst);
        }
        /// <summary>
        /// 保存信息到Excel中
        /// </summary>
        /// <param name="table">保存在Datatable中的数据对象</param>
        /// <param name="name">想要保存为的文件名</param>
        /// <param name="saveWay">保存路径</param>
        /// <param name="title">每列显示的内容名数组</param>
        public static string SaveExcel(System.Data.DataTable table, string name,
                                       string saveWay, string[] title, IList<string> valueColumnNames)
        {
            string message = "";
            if (table.Rows.Count == 0)
                message = "没有数据传入，无需保存！";
            else
            {
                //创建Excel文档。
                XlsDocument xls = new XlsDocument();
                Worksheet sheet = xls.Workbook.Worksheets.Add("Sheet1");
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
                for (int i = 1; i <= title.Length; i++)
                {
                    ColumnInfo colInfo = new ColumnInfo(xls, sheet);
                    colInfo.ColumnIndexStart = (ushort)(i-1);
                    colInfo.ColumnIndexEnd = (ushort)i;
                    colInfo.Width = 15 * 256;
                    sheet.AddColumnInfo(colInfo);
                    cells.Add(1, i, title[i - 1], xfDataHead);
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
                string date01 = string.Empty;
                int ipoints = 1;
                foreach (DataRow dr in table.Rows)
                {
                    for(int iCol = 1; iCol <= table.Columns.Count; iCol++)
                    {
                        if (valueColumnNames.Contains(table.Columns[iCol - 1].ColumnName))
                        {
                            double val=0;
                            if (double.TryParse(dr[iCol - 1].ToString(), out val))
                            {
                                //向单元格中插入数据
                                cells.Add(iRow, iCol, val, xfData);
                            }
                            else
                            {
                                cells.Add(iRow, iCol, dr[iCol - 1].ToString(), xfData);
                            }
                        }
                        else if (table.Columns[iCol - 1].ColumnName.ToUpper().Equals("CREATE_TIME"))
                        {
                            string sdate = dr[iCol - 1].ToString().Trim().ToUpper();
                            if (date01.Equals(string.Empty))
                            {
                                date01 = sdate;
                                cells.Add(iRow, iCol, sdate + "_" + ipoints.ToString(), xfData);                                
                            }
                            else if (!date01.Equals(sdate))
                            {
                                date01 = sdate;
                                ipoints++;
                                cells.Add(iRow, iCol, sdate + "_" + ipoints.ToString(), xfData);                           
                            }
                            else
                                cells.Add(iRow, iCol, sdate + "_" + ipoints.ToString(), xfData);                           
                        }
                        else
                        {
                            //向单元格中插入数据
                            cells.Add(iRow, iCol, dr[iCol - 1].ToString(), xfData);
                        }
                        
                    }
                    iRow++;
                }
                xls.FileName=name.ToString();
                string fileWay = saveWay;//得到完整的文件路径
                xls.Save(fileWay,true);
                message = "已将信息保存到“" + saveWay + "”文件夹下";
            }           
            return message;
        }
    }
}
