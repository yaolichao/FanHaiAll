using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing.Printing;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class WarehouseWarrantQueryCtrl : BaseUserCtrl
    {
        IViewContent _view = null;
        WarehouseWarrantOperationEntity _entity = new WarehouseWarrantOperationEntity();

        public WarehouseWarrantQueryCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            GridViewHelper.SetGridView(gridView1);
            GridViewHelper.SetGridView(gridView2);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryBtn_Click(object sender, EventArgs e)
        {
            DataSet dsParams = this.GetQueryCondition();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = this._entity.Query(dsParams, ref config);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (dsReturn.Tables.Count > 0)
            {
                DataTable dt = dsReturn.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["ISSYN"].ToString() == "0")
                    {
                        dt.Rows[i]["ISSYN"] = "未同步";
                    }
                    else 
                    {
                        dt.Rows[i]["ISSYN"] = "已同步";
                    }
                }

                gridControl1.DataSource = dt;
                this.gridControl1.MainView = gridView1;
            }
            if (gridView1.RowCount > 0)
            {
                gridView1.SelectRow(0);
                SelectedRowChanged(gridView1);
            }
            else
            {
                gridControl2.DataSource = null;
            }
        }

        /// <summary>
        /// 获取查询条件。
        /// </summary>
        /// <returns>包含查询条件的数据集对象。</returns>
        private DataSet GetQueryCondition()
        {
            string ZMBLNR   = this.teZMBLNR.Text.Trim();
            string AUFNR    = this.teAUFNR.Text.Trim();
            string MATNR    = this.teMATNR.Text.Trim();
            string CREATOR  = this.teCreator.Text.Trim();
            string BDATE    = this.deBDate.Text.Trim();
            string EDATE    = this.deEDate.Text.Trim();

            Hashtable htParams = new Hashtable();
            if (!string.IsNullOrEmpty(ZMBLNR))
            {
                htParams.Add("ZMBLNR", ZMBLNR);
            }
            if (!string.IsNullOrEmpty(AUFNR))
            {
                htParams.Add("AUFNR", AUFNR);
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                htParams.Add("MATNR", MATNR);
            }
            if (!string.IsNullOrEmpty(CREATOR))
            {
                htParams.Add("CREATOR", CREATOR);
            }
            if (!string.IsNullOrEmpty(BDATE))
            {
                htParams.Add("BDATE", BDATE);
            }
            if (!string.IsNullOrEmpty(EDATE))
            {
                htParams.Add("EDATE", EDATE);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            DataSet dsParams = new DataSet();
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            return dsParams;
        }

        private void SelectedRowChanged(GridView View)
        {
            if (View.GetSelectedRows().Length <= 0)
            {
                gridControl2.DataSource = null;
                return;
            }
            int selectedRow = View.GetSelectedRows()[0];
            if (View.GetSelectedRows()[0] >= View.RowCount)
            {
                View.SelectRow(0);
                selectedRow = 0;
            }
            string cellValue = View.GetRowCellValue(selectedRow, colZMBLNR).ToString();

            DataSet dsReturn = this._entity.QueryWarehouseWarrantItems(cellValue);
            if (dsReturn.Tables.Count > 0)
                gridControl2.DataSource = dsReturn.Tables[0];
        }

        /// <summary>
        /// 打印入库单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

            PrintDialog dlg = new PrintDialog();
            dlg.AllowSomePages = true;
            dlg.Document = printDoc;
           
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PrintPreviewDialog dlgPrintPreview = new PrintPreviewDialog();
                dlgPrintPreview.Document = printDoc;
                dlgPrintPreview.UseAntiAlias = true;
                if (dlgPrintPreview.ShowDialog() == DialogResult.OK)
                {
                    //调用Print方法，激发PrintPage 事件处理函数
                    printDoc.Print();
                }
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintDocument printDoc = sender as PrintDocument;

            int Height = e.PageSettings.PaperSize.Height;
            int Width = e.PageSettings.PaperSize.Width;
            //使用 GDI+ 对象绘制，已完成打印
            //----------------页眉
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            e.Graphics.DrawString("山东泛海阳光能源有限公司", new Font("宋体", 10), Brushes.Black, new Point(Width / 2, 10), sfCenter);

            e.Graphics.DrawString("Q/ZTIS Z03080108-02", new Font("宋体", 6), Brushes.Black, e.PageBounds.Left+5, 20);
            e.Graphics.DrawString("版本 5/0", new Font("宋体", 6), Brushes.Black, e.PageBounds.Right - 40, 20);
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left, 35), new Point(e.PageBounds.Right, 35));

            //----------------标题
            e.Graphics.DrawString("晶硅组件入库单", new Font("宋体", 12), Brushes.Black, new Point(Width / 2, 50), sfCenter);

            e.Graphics.DrawString("入库部门：", new Font("宋体", 7), Brushes.Black, e.PageBounds.Left, 70);
            e.Graphics.DrawString("入库单号：", new Font("宋体", 7), Brushes.Black, e.PageBounds.Right - 120, 70);
            e.Graphics.DrawString("生产工单号：", new Font("宋体", 7), Brushes.Black, e.PageBounds.Left, 90);
            e.Graphics.DrawString("入库日期：", new Font("宋体", 7), Brushes.Black, e.PageBounds.Right - 120, 90);

            //--------------数据
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Left + 10, 120));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 100), new Point(e.PageBounds.Right - 10, 100));
            e.Graphics.DrawLine(Pens.Black, new Point(e.PageBounds.Left + 10, 120), new Point(e.PageBounds.Right - 10, 120));
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gv = sender as GridView;
            SelectedRowChanged(gv);
        }

        /// <summary>
        /// 分页事件
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            QueryBtn_Click(null, null);
        }

        #region 回车事件
        private void teZMBLNR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                QueryBtn_Click(null, null);
        }

        private void teAUFNR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                QueryBtn_Click(null, null);
        }

        private void teMATNR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                QueryBtn_Click(null, null);
        }

        private void teCreator_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                QueryBtn_Click(null, null);
        }

        private void deBDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                QueryBtn_Click(null, null);
        }

        private void deEDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                QueryBtn_Click(null, null);
        }
        #endregion

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
