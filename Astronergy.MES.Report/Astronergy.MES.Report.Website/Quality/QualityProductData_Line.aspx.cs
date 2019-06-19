﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DevExpress.Web;
using Astronergy.MES.Report.DataAccess;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// 优品率报表
/// </summary>
public partial class Quality_QualityProductData_Line : BasePage
{
    private QualityProductDataAccess qualityProduct = new QualityProductDataAccess();
    private const string START_TIME = "08:00:00";
    private const string END_TIME = "08:00:00";
    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        //{"TRACKOUT_QTY",                            "终检出站数量"},
        //{"LJ_TRACKOUT_QTY",                         "累计终检出站数量"},

        //{"A0J_QTY",                                 "A0数量"},
        //{"LJ_A0J_QTY",                              "累计A0数量"},
        //{"DIRECT_A0J_QTY",                          "直判A0数量"},
        //{"LJ_DIRECT_A0J_QTY",                       "累计直判A0数量"},
        //{"REWORK_A0J_QTY",                          "返修后A0级数量"},
        //{"LJ_REWORK_A0J_QTY",                       "累计返修后A0级数量"},
        //{"PRE_A0J_QTY",                             "预判A0数量"},
        //{"LJ_PRE_A0J_QTY",                          "累计预判A0数量"},

        //{"ERSANJ_QTY",                              "二三级数量"},
        //{"LJ_ERSANJ_QTY",                           "累计二三级数量"},
        ////{"NOREWORK_ERSANJ_QTY",                     "二三级<非返修>数量"},
        ////{"LJ_NOREWORK_ERSANJ_QTY",                  "累计二三级<非返修>数量"},
        //{"REWORK_ERSANJ_QTY",                       "二三级<返修>数量"},
        //{"LJ_REWORK_ERSANJ_QTY",                    "累计二三级<返修>数量"},
        //{"NO_CY_REWORK_ERSANJ_QTY",                 "二三级<非层压返修>数量"},
        //{"LJ_NO_CY_REWORK_ERSANJ_QTY",              "累计二三级<非层压返修>数量"},
        
        //{"SCRAP_QTY",                               "实际报废数量"},
        //{"LJ_SCRAP_QTY",                            "累计实际报废数量"},
        ////{"NOREWORK_SCRAP_QTY",                    "报废<非返修>数量"},
        ////{"LJ_NOREWORK_SCRAP_QTY",                 "累计报废<非返修>数量"},
        //{"NO_CY_REWORK_SCRAP_QTY",                  "报废<非层压返修>数量"},
        //{"LJ_NO_CY_REWORK_SCRAP_QTY",               "累计报废<非层压返修>数量"},
        {"KJ_RATE",                                 "客级品率"},
        {"LJ_KJ_RATE",                              "累计客级品率"},
        {"A_UPON_QUALITY_RATE",                     "A级以上优品率"},
        {"LJ_A_UPON_QUALITY_RATE",                  "累计 A级以上优品率"},
        {"A0_UPON_QUALITY_RATE",                    "A0级以上优品率"},
        {"LJ_A0_UPON_QUALITY_RATE",                 "累计 A0级以上优品率"},
        {"A0_ERSANJ_QUALITY_RATE",                  "A0、二三级品率"},
        {"LJ_A0_ERSANJ_QUALITY_RATE",               "累计A0、二三级品率"},
        {"REWORK_A0_ERSANJ_QUALITY_RATE",           "返修后A0、二三级品率"},
        {"LJ_REWORK_A0_ERSANJ_QUALITY_RATE",        "累计返修后A0、二三级品率"},
        {"PASS_RATE",                               "合格率"},
        {"LJ_PASS_RATE",                            "累计合格率"},
        {"SCRAP_RATE",                              "报废品率"},
        {"LJ_SCRAP_RATE",                           "累计报废品率"}
    };
    /// <summary>
    /// 页面载入事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRoomData();
            BindProductModel();
            BindCustomerType();
            BindLine();
            DateTime dtNow = CommonFunction.GetCurrentDateTime();
            this.dateStart.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.dateEnd.Date = dtNow;
            this.deStartTime.Text = START_TIME;
            this.deEndTime.Text = END_TIME;
        }
    }
    /// <summary>
    /// 工厂车间。
    /// </summary>
    private void BindRoomData()
    {
        DataTable dtWorkPlace = CommonFunction.GetFactoryWorkPlace();
        this.cmbWorkPlace.DataSource = dtWorkPlace;
        this.cmbWorkPlace.DataBind();
        this.cmbWorkPlace.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        this.cmbWorkPlace.SelectedIndex = 0;

        string roomKey = Convert.ToString(this.cmbWorkPlace.Value);
        BindWorkOrderNumber(roomKey);
    }
    /// <summary>
    /// 绑定客户类别。
    /// </summary>
    private void BindCustomerType()
    {
        DataTable dtCustomerType = CommonFunction.GetCustomerType();
        ASPxListBox lst = this.ddeCustomerType.FindControl("lstCustomerType") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dtCustomerType;
            lst.TextField = "CUSTOMER_TYPE";
            lst.ValueField = "CUSTOMER_TYPE";
            lst.DataBind();
            lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }

    }

    /// <summary>
    /// 绑定工单
    /// </summary>
    public void BindWorkOrderNumber(string roomKey)
    {
        if (roomKey == "ALL") roomKey = string.Empty;
        DataTable dtWorkOrderNo = CommonFunction.GetLotWorkOrderNumber(roomKey);
        ASPxListBox lst = this.ddeWorkOrderNumber.FindControl("lstWorkOrderNumber") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dtWorkOrderNo;
            lst.TextField = "WORK_ORDER_NO";
            lst.ValueField = "WORK_ORDER_NO";
            lst.DataBind();
            lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
    }

    ///// <summary>
    ///// 绑定产品料号。
    ///// </summary>
    //public void BindPartNumber()
    //{
    //    DataTable dt = null;
    //    dt = CommonFunction.GetPartNumber();
        
    //    ASPxListBox lst = this.cmbPartNumber.FindControl("lstpartNumber") as ASPxListBox;
    //    if (lst != null)
    //    {
    //        lst.DataSource = dt;
    //        lst.TextField = "PART_NUMBER";
    //        lst.ValueField = "PART_NUMBER";
    //        lst.DataBind();
    //        lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
    //    }
    //}
    /// <summary>
    /// 产品型号
    /// </summary>
    private void BindProductModel()
    {
        DataTable dtProduct = CommonFunction.GetProductModel();
        ASPxListBox lst = this.ddeProductModel.FindControl("lstProductModel") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dtProduct;
            lst.TextField = "PROMODEL_NAME";
            lst.ValueField = "PROMODEL_NAME";
            lst.DataBind();
            lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
    }

    private void BindLine()
    {
        DataTable dtLine = CommonFunction.GetLine();
        ASPxListBox lst = this.ddLine.FindControl("lstLine") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dtLine;
            lst.TextField = "LINE_NAME";
            lst.ValueField = "LINE_NAME";
            lst.DataBind();
            lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
    }

    /// <summary>
    /// 车间值改变时触发。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbWorkPlace_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ddeWorkOrderNumber.Text = string.Empty;
        string roomKey = Convert.ToString(this.cmbWorkPlace.Value);
        BindWorkOrderNumber(roomKey);
        this.UpdatePanelWorkOrderNumber.Update();
    }
    /// <summary>
    /// 查询优品率数据。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        int queryType = this.rbtByDay.Checked ? 1 : 0;
        string roomKey = Convert.ToString(this.cmbWorkPlace.Value);
        string customer = this.ddeCustomerType.Text.Trim();
        string productModel = this.ddeProductModel.Text.Trim();
        string workOrderNo = this.ddeWorkOrderNumber.Text.Trim();
        string partNumber = this.textPartNumber.Text.ToString().Trim();
        string oprline = this.ddLine.Text.Trim();

        string startTime = this.dateStart.Date.ToString("yyyy-MM-dd");
        string endTime = this.dateEnd.Date.ToString("yyyy-MM-dd");

        if (queryType == 0)
        {
            startTime =string.Format("{0} {1}",startTime,this.deStartTime.Text);
            endTime = string.Format("{0} {1}", endTime, this.deEndTime.Text);
        }

        if (roomKey.ToUpper() == "ALL") roomKey = string.Empty;
        if (workOrderNo.ToUpper() == "ALL") workOrderNo = string.Empty;
        if (productModel.ToUpper() == "ALL") productModel = string.Empty;
        if (customer.ToUpper() == "ALL") customer = string.Empty;
        if (oprline.ToUpper() == "ALL") oprline = string.Empty;

        DataSet ds = qualityProduct.GetGood(queryType, startTime, endTime, roomKey, customer, productModel, string.Empty, workOrderNo, partNumber,oprline);
        if (ds != null)
        {
            DataTable dt = qualityProduct.TransferDatatable(ds.Tables[0], queryType, dic);
            if (queryType == 0)
            {
                string colunName = string.Format("{0}至{1}", startTime, endTime);
                dt.Columns[2].ColumnName = colunName;
            }
            DrawChart(dt, queryType);
            if (sender == this.btnXlsExport)
            {
                this.gridExport.Columns.Clear();
                this.gridExport.AutoGenerateColumns = true;
                this.gridExport.DataSource = dt;
                this.gridExport.DataBind();
                this.gridExport.AutoGenerateColumns = false;
            }
            else
            {
                this.grid.Columns.Clear();
                this.grid.AutoGenerateColumns = true;
                this.grid.DataSource = dt;
                this.grid.DataBind();
                this.grid.AutoGenerateColumns = false;
            }
        }
        this.UpdatePanelChart.Update();
        this.UpdatePanelResult.Update();
    }
    /// <summary>
    /// 查询优品率之后绘制分析图形。
    /// </summary>
    /// <param name="dtSource">源数据表。</param>
    private void DrawChart(DataTable dtSource, int queryType)
    {
        XYDiagram diagram = this.chartPassYield.Diagram as XYDiagram;
        if (diagram == null) return;
        diagram.AxisX.Label.Angle = -45;
        this.chartPassYield.Series.Clear();
        if (dtSource.Rows.Count <= 0 || queryType == 0)
        {
            this.chartPassYield.Series.Add(new Series());
            return;
        }
        foreach (DataRow dr in dtSource.Rows)
        {
            string promodelName = Convert.ToString(dr["PROMODEL_NAME"]);
            if (promodelName != "ALL") continue;

            string colValue = Convert.ToString(dr["COL_VALUE"]);
            string colName = dic[colValue];
            string sname = string.Format("{0}{1}", dr["PROMODEL_NAME"], colName);
            Series s = new Series(sname, ViewType.Line);
            //s.PointOptions.Pattern = "{S}:{V}";
            PointSeriesLabel label = s.Label as PointSeriesLabel;
            LineSeriesView sv = s.View as LineSeriesView;
            label.Visible = false;
            s.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            
            for (int i = 2; i < dtSource.Columns.Count-1; i++)
            {
                string argument = dtSource.Columns[i].ColumnName;
                SeriesPoint p = null;
                if (string.IsNullOrEmpty(Convert.ToString(dr[i])))
                {
                    p = new SeriesPoint(argument);
                    p.IsEmpty = true;
                }
                else
                {
                    double val = Convert.ToDouble(dr[i]);
                    p = new SeriesPoint(argument, val);
                }
                s.Points.Add(p);
            }
            chartPassYield.Series.Add(s);
        }
        //重置图形大小。
        if(this.chartPassYield.Series.Count>0)
        {
            int width = this.chartPassYield.Series[0].Points.Count * 20;
            width = width < 1024 ? 1024 : width;
            width += 100;
            this.chartPassYield.Width = Unit.Pixel(width);
            this.UpdatePanelChart.Update();
        }
    }

    /// <summary>
    /// 导出EXCEL。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        btnQuery_Click(sender, e);
        DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
        options.ExportHyperlinks = false;
        this.exporter.WriteXlsToResponse("QualityProductData", true, options);
    }
    /// <summary>
    /// 自定义文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        DataTable dt = gv.DataSource as DataTable;
        if (dt != null && e.Column.Index > 1 && e.Column.Index < dt.Columns.Count - 1  && !string.IsNullOrEmpty(Convert.ToString(e.Value)))
        {
            int queryType = this.rbtByDay.Checked ? 2 : 3;
            string roomKey = Convert.ToString(this.cmbWorkPlace.Value);
            string customer = this.ddeCustomerType.Text.Trim();
            string workOrderNo = this.ddeWorkOrderNumber.Text.Trim();
            string productModel = Convert.ToString(dt.Rows[e.VisibleRowIndex][0]);
            string oprline = this.ddLine.Text.Trim();

            string startTime = this.dateStart.Date.ToString("yyyy-MM-dd");
            string endTime = this.dateEnd.Date.ToString("yyyy-MM-dd");


            if (queryType == 3)
            {
                startTime = string.Format("{0} {1}", startTime, this.deStartTime.Text);
                endTime = string.Format("{0} {1}", endTime, this.deEndTime.Text);
            }
            else
            {
                endTime=e.Column.FieldName;
            }
            string trxType = Convert.ToString(dt.Rows[e.VisibleRowIndex][1]);

            if (sender == this.grid && trxType.EndsWith("RATE"))
            {
                string url = string.Format(@"QualityProductDataDetail_Line.aspx?startTime={0}&endTime={1}&trxType={2}&roomKey={3}&customer={7}&productModel={4}&workOrderNo={5}&queryType={6}&partNumber={7}&oprliner={8}",
                    Server.UrlEncode(startTime),
                    Server.UrlEncode(endTime),
                    Server.UrlEncode(trxType),
                    Server.UrlEncode(roomKey),
                    Server.UrlEncode(productModel),
                    Server.UrlEncode(workOrderNo),
                    queryType,
                    Server.UrlEncode(customer),
                    Server.UrlEncode(this.textPartNumber.Text.ToString().Trim()),
                    Server.UrlEncode(this.ddLine.Text.ToString().Trim())
                    );
                double val = Convert.ToDouble(e.Value);
                e.DisplayText = string.Format("<a href=\"javascript:void(0);\" onclick=\"javascript:window.open('{1}')\" class=\"dxgv\" style=\"text-decoration:underline;\">{0}</a>",
                     val.ToString("#0.##%"), url);
            }
        }
        if (e.Column.Index == 1)
        {
            e.DisplayText = dic[Convert.ToString(e.Value)];
        }
        else if (e.Column.Index == 0 && Convert.ToString(e.Value) == "ALL")
        {
            e.DisplayText = "总计";
        }
    }

    /// <summary>
    /// 行创建时触发事件，用于合并单元格。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        DataTable dt = grid.DataSource as DataTable;
        if (e.RowType == GridViewRowType.Data && dt!=null)
        {
            if (Convert.ToString(e.GetValue("PROMODEL_NAME")) == "ALL")
            {
                for (int i = 2; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.CssStyle.Add(HtmlTextWriterStyle.BackgroundColor, "lightgray");
                }
            }
            string productModelName = Convert.ToString(dt.Rows[e.VisibleIndex]["PROMODEL_NAME"]);
            int mergeCellCount = dt.Select(string.Format("PROMODEL_NAME='{0}'", productModelName)).Count();
            //合并产品型号单元格。
            if (e.VisibleIndex % mergeCellCount == 0)
            {
                e.Row.Cells[0].Text = string.Empty;
                e.Row.Cells[0].Style.Add("border-top-width", "1px");
                e.Row.Cells[0].Style.Add("border-bottom-width", "0px");
            }
            else if ((e.VisibleIndex + 1) == dt.Rows.Count)
            {
                e.Row.Cells[0].Text = string.Empty;
                e.Row.Cells[0].Style.Add("border-top-width", "0px");
                e.Row.Cells[0].Style.Add("border-bottom-width", "1px");
            }
            else if ((e.VisibleIndex + 1) % mergeCellCount == 0)
            {
                e.Row.Cells[0].Text = string.Empty;
                e.Row.Cells[0].Style.Add("border-top-width", "0px");
                e.Row.Cells[0].Style.Add("border-bottom-width", "1px");
            }
            else if (e.VisibleIndex % mergeCellCount == mergeCellCount / 2)
            {
                e.Row.Cells[0].Style.Add("border-top-width", "0px");
                e.Row.Cells[0].Style.Add("border-bottom-width", "0px");
            }
            else
            {
                e.Row.Cells[0].Text = string.Empty;
                e.Row.Cells[0].Style.Add("border-top-width", "0px");
                e.Row.Cells[0].Style.Add("border-bottom-width", "0px");
            }
        }
    }

    /// <summary>
    /// 导出数据时重新设置文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
    {
        DataTable dt = this.gridExport.DataSource as DataTable;
        if (dt!=null
            && e.RowType == DevExpress.Web.GridViewRowType.Data
            && e.Column != null && e.Column.Index > 1 
            && e.Column.Index < dt.Columns.Count-1)
        {
            string trxType = Convert.ToString(dt.Rows[e.VisibleIndex][1]);
            if (trxType.EndsWith("RATE"))
            {
                e.TextValueFormatString = "#0.00%";
            }
        }
    }


    /// <summary>
    /// 数据绑定时触发事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        if (gv != null && gv.Columns.Count > 1)
        {
            gv.Columns[0].Caption = "产品型号";
            gv.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
            gv.Columns[0].CellStyle.BackColor = Color.FromName("#ffffd6");
            gv.Columns[1].Caption = " ";
            gv.Columns[1].FixedStyle = GridViewColumnFixedStyle.Left;
            gv.Columns[1].Width = Unit.Pixel(200);
            gv.Columns[1].CellStyle.BackColor = Color.FromName("#ffffd6");
            //gv.Columns[1].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            gv.Columns["KEY_VALUE"].Visible = false;

        }
    }

}
