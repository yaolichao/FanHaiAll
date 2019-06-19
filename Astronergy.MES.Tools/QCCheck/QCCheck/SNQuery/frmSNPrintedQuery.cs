using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QCCheck.PrintUtility;
using ZHDSpace;
using Microsoft.Office.Interop.Excel;

namespace QCCheck.SNQuery
{
    public partial class frmSNPrintedQuery : Form
    {
        DBUtility db = new DBUtility();

        public frmSNPrintedQuery()
        {
            InitializeComponent();
        }

        private void dgvPrintSN_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvPrintSN.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvPrintSN.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvPrintSN.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvPrintSN_Bind()
        {
            dgvPrintSN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvPrintSN.Columns["sn"].HeaderText = "SN";
            dgvPrintSN.Columns["label_type"].HeaderText = "标签类型";
            dgvPrintSN.Columns["num"].HeaderText = "打印系号";
            dgvPrintSN.Columns["wo"].HeaderText = "工单号";
            dgvPrintSN.Columns["product_id"].HeaderText = "产品类型";
            dgvPrintSN.Columns["product_type"].HeaderText = "产品代码";
            dgvPrintSN.Columns["print_date"].HeaderText = "补印日期";
            dgvPrintSN.Columns["print_user"].HeaderText = "补印人员";
            dgvPrintSN.Columns["reprint_num"].HeaderText = "补印次数";
            dgvPrintSN.Columns["year"].HeaderText = "年份";
            dgvPrintSN.Columns["month"].HeaderText = "月份";
            dgvPrintSN.Columns["week"].HeaderText = "周别";
            dgvPrintSN.Columns["power"].HeaderText = "功率档位";
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sql,sSNStart,sSNEnd,sLabelType,sWo,sProductID,sProductType,sPrintDate,sPrintUser,sYear,sMonth,sWeek,sPower;
            sSNStart = ""; sSNEnd = ""; sLabelType = ""; sWo = ""; sProductID = "";sProductType = "";
            sPrintDate = "";sPrintUser = "";sYear = "";sMonth = "";sWeek = ""; sPower = "";

            if (txtSNStart.Text.Trim() != "")
            {
                sSNStart = txtSNStart.Text.Trim();
            }
            if (txtSNEnd.Text.Trim() != "")
            {
                sSNEnd = txtSNEnd.Text.Trim();
            }
            if (cboLabelType.Text.Trim() != "")
            {
                sLabelType = cboLabelType.SelectedValue.ToString();
            }
            if (txtWO.Text.Trim() != "")
            {
                sWo = txtWO.Text.Trim();
            }
            if (txtProductType.Text.Trim() != "")
            {
                sProductID = txtProductType.Text.Trim();
            }
            if (txtProductCode.Text.Trim() != "")
            {
                sProductType = txtProductCode.Text.Trim();
            }
            sPrintDate = dtpPrintDate.Text.ToString();
            if (txtRePrintUser.Text.Trim() != "")
            {
                sPrintUser = txtRePrintUser.Text.Trim();
            }
            if (txtYear.Text.Trim() != "")
            {
                sYear = txtYear.Text.Trim();
            }
            if(txtMonth.Text.Trim() != "")
            {
                sMonth = txtMonth.Text.Trim();
            }
            if(txtWeek.Text.Trim() != "")
            {
                sWeek = txtWeek.Text.Trim();
            }
            if(txtPower.Text.Trim() != "")
            {
                sPower = txtPower.Text.Trim();
            }

            sql = "select '''' + sn as 'sn',";
            sql += " case when label_type='A' then '双排/单排亚银'";
            sql += " when label_type='B' then '三排/单排铜版'";
            sql += " when label_type='C' then '三排无BARCODE' else '' end  as 'label_type'";
            sql += " ,num,wo,product_id,product_type,print_date,print_user,reprint_num,year,month,week,power";
            sql += " from sn_print where 1=1";

            if (sSNStart != "")
            {
                sql += " and sn>='" + sSNStart + "'";
            }
            if(sSNEnd != "")
            {
                sql += " and sn<='" + sSNEnd + "'";
            }
            if (sLabelType != "")
            {
                sql += " and label_type='" + sLabelType + "'";
            }
            if (sWo != "")
            {
                sql += " and wo='" + sWo + "'";
            }
            if (sProductID != "")
            {
                sql += " and product_id='" + sProductID + "'";
            }
            if(sProductType != "")
            {
                sql += " and product_type='" + sProductType + "'";
            }
            if(sPrintDate != "" && cbxPrintDate.Checked == true)
            {
                sql += " and print_date='" + sPrintDate + "'";
            }
            if (sPrintUser != "")
            {
                sql += " and print_user='" + sPrintUser + "'";
            }
            if(sYear != "")
            {
                sql += " and year='" + sYear + "'";
            }
            if (sMonth != "")
            {
                sql += " and month='" + sMonth + "'";
            }
            if(sPower != "")
            {
                sql += " and power='" + sPower + "'";
            }
            sql += " order by num";

            DataSet dsSNPrint = db.Query(sql);
            dgvPrintSN.DataSource = dsSNPrint.Tables[0];
            dgvPrintSN_Bind();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            int nColumn, nRow;

            if (dgvPrintSN.Rows.Count > 0)
            {
                try
                {
                    nColumn = dgvPrintSN.ColumnCount;
                    nRow = dgvPrintSN.Rows.Count;

                    Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                    oExcel.Visible = false;
                    Microsoft.Office.Interop.Excel.Workbook oWorkbook = oExcel.Workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet oWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)oWorkbook.Worksheets[1];
                    //oWorksheet.Name = txtStockNo.Text.Trim();
                    for (int c = 0; c < nColumn; c++)
                    {
                        oWorksheet.Cells[1, c + 1] = dgvPrintSN.Columns[c].HeaderText;
                    }
                    for (int r = 0; r < nRow; r++)
                    {
                        for (int c = 0; c < nColumn; c++)
                        {
                            oWorksheet.Cells[r + 2, c + 1] = dgvPrintSN.Rows[r].Cells[c].Value;
                        }
                    }
                    nRow++;
                    oWorksheet.get_Range("A1", "M1").Interior.ColorIndex = 36;
                    oWorksheet.get_Range("A1", "M" + nRow.ToString()).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    oWorksheet.Cells.get_Range("A1", "M" + nRow.ToString()).Borders.LineStyle = 1;
                    oWorksheet.get_Range("G2", "G" + nRow.ToString()).EntireColumn.NumberFormat = "yyyy-MM-dd";
                    oWorksheet.get_Range("A2", "A" + nRow.ToString()).EntireColumn.NumberFormat = "@";
                    //oWorksheet.Cells.Font.Name = "Verdana";
                    //oWorksheet.Cells.Font.Size = 10;
                    //oWorksheet.Cells.AutoFit();
                    oExcel.Visible = true;
                    oExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
                    System.GC.Collect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建Excel失败，请确认是否有安装Excel应用程序！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void frmSNPrintedQuery_Load(object sender, EventArgs e)
        {
            System.Data.DataTable tbLabelType = new System.Data.DataTable();
            tbLabelType.Columns.Add("label_id");
            tbLabelType.Columns.Add("label_name");
            tbLabelType.Rows.Add("A","双排/单排亚银");
            tbLabelType.Rows.Add("B", "三排/单排铜版");
            tbLabelType.Rows.Add("C", "三排无BARCODE");
            cboLabelType.DataSource = tbLabelType;
            cboLabelType.ValueMember = "label_id";
            cboLabelType.DisplayMember = "label_name";
            cboLabelType.SelectedIndex = -1;
        }
    }
}
