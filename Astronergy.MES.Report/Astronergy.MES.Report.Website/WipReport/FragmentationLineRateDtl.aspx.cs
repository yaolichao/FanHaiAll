using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Astronergy.MES.Report.DataAccess;
using System.Data;
using DevExpress.Web;
using DevExpress.Data;
using DevExpress.XtraCharts;
using System.Drawing;

public partial class WipReport_FragmentationLineRateDtl : BasePage
{
    string stime = string.Empty, etime = string.Empty, sType = string.Empty, locationkey = string.Empty, sName = string.Empty, shiftName = string.Empty, lineName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sType = Server.UrlDecode(Request.Params["sType"].ToString());
            sName = Server.UrlDecode(Request.Params["sName"].ToString());
            shiftName = Server.UrlDecode(Request.Params["shiftname"].ToString());
            stime = Server.UrlDecode(Request.Params["paraDate01"].ToString());
            etime = Server.UrlDecode(Request.Params["paraDate02"].ToString());
            locationkey = Server.UrlDecode(Request.Params["locationKey"].ToString());
            lineName = Server.UrlDecode(Request.Params["lineName"].ToString());


            BindGvAndChart();
        }
    }

    private void BindGvAndChart()
    {
        try
        {

            DataSet dsReturn = FragmentationLineRateData.QueryPatchLineDataDtl(sType, sName, locationkey, lineName, shiftName, stime, etime);

            DataTable dtGv = LoadColumnsResource(dsReturn.Tables["PATCH_DATA"]);
            ViewState["grid"] = dtGv;

            ASPxSummaryItem item = new ASPxSummaryItem();
            item.FieldName = "PATCH_QUANTITY";
            item.SummaryType = SummaryItemType.Sum;
            item.DisplayFormat = "不良总计：{0}";
            this.grid.TotalSummary.Add(item);
            this.grid.Settings.ShowFooter = true;

            this.grid.DataSource = dtGv;
            //this.grid.SummaryText = "PATCH_QUANTITY";           
            this.grid.DataBind();

            if (dsReturn.Tables["PATCH_COUNT"].Rows.Count > 0)
            {
               DataTable dt= seriesTable(dsReturn.Tables["PATCH_COUNT"]);
               CreateChart(dt);
            }
        }
        catch (Exception ex)
        {

        }
    }


    public void SingleColumnChart(DataTable dt)
    {
        Series series = new Series("不良数目", ViewType.Bar);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SeriesPoint point = new SeriesPoint(dt.Rows[i]["REASON_CODE_NAME"].ToString(), Convert.ToDouble(dt.Rows[i]["PATCH_TOT"].ToString()));
            series.Points.Add(point);
        }
        this.WebChartControl1.Series.Add(series);
    }


    #region 绘制图表

    private DataTable seriesTable(DataTable dt)
    {


        int nRow;

        if (dt.Rows.Count > 10)
        {
            nRow = 10;
        }
        else
        {
            nRow = dt.Rows.Count;
        }

        DataTable seriesTable = new DataTable();

        seriesTable.Columns.Add("类型", typeof(string));

        for (int i = 0; i < nRow; i++)
        {
            seriesTable.Columns.Add(Convert.ToString(dt.Rows[i]["REASON_CODE_NAME"]), typeof(decimal));
        }


        for (int j = 0; j < dt.Columns.Count - 1; j++)
        {

            seriesTable.Rows.Add();

            if (j == 0)
            {
                seriesTable.Rows[j][0] = "不良数量";
            }
            if (j == 1)
            {
                seriesTable.Rows[j][0] = "不良比例";
            }

            for (int i = 0; i < nRow; i++)
            {
                seriesTable.Rows[j][i+1] = dt.Rows[i][j + 1];
            }
        }

        this.gdvPatchTop.DataSource = null;
        this.gdvPatchTop.Columns.Clear();
        this.gdvPatchTop.AutoGenerateColumns = true;

        this.gdvPatchTop.DataSource = seriesTable;
        this.gdvPatchTop.DataBind();

        return seriesTable;
    }


    private void CreateChart(DataTable dt)
    {
        #region Series
        //创建几个图形的对象
        Series series1 = CreateSeries("不良数量", ViewType.Bar, dt, 0);
        series1.View.Color = Color.Blue;

        #endregion

        List<Series> list = new List<Series>() { series1 };
        WebChartControl1.Series.AddRange(list.ToArray());
        WebChartControl1.Legend.Visible = false;
        //WebChartControl1.SeriesTemplate.LabelsVisibility = DefaultBoolean.True;

        CreateAxisY(series1, "Bar");
    }

    /// <summary>
    /// 根据数据创建一个图形展现
    /// </summary>
    /// <param name="caption">图形标题</param>
    /// <param name="viewType">图形类型</param>
    /// <param name="dt">数据DataTable</param>
    /// <param name="rowIndex">图形数据的行序号</param>
    /// <returns></returns>
    private Series CreateSeries(string caption, ViewType viewType, DataTable dt, int rowIndex)
    {
        Series series = new Series(caption, viewType);
        for (int i = 1; i < dt.Columns.Count; i++)
        {
            string argument = dt.Columns[i].ColumnName;//参数名称
            decimal value = (decimal)dt.Rows[rowIndex][i];//参数值
            series.Points.Add(new SeriesPoint(argument, value));
        }

        //必须设置ArgumentScaleType的类型，否则显示会转换为日期格式，导致不是希望的格式显示
        //也就是说，显示字符串的参数，必须设置类型为ScaleType.Qualitative
        series.ArgumentScaleType = ScaleType.Qualitative;
        //series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;//显示标注标签

        return series;
    }

    /// <summary>
    /// 创建图表的第二坐标系
    /// </summary>
    /// <param name="series">Series对象</param>
    /// <returns></returns>
    private SecondaryAxisY CreateAxisY(Series series, string viewType)
    {
        SecondaryAxisY myAxis = new SecondaryAxisY(series.Name);
        ((XYDiagram)WebChartControl1.Diagram).SecondaryAxesY.Add(myAxis);

        switch (viewType)
        {
            case "Bar":
                ((BarSeriesView)series.View).AxisY = myAxis;
                break;
            case "Line":
                ((LineSeriesView)series.View).AxisY = myAxis;
                break;
            default:
                break;
        }

        myAxis.Title.Text = series.Name;
        myAxis.Title.Alignment = StringAlignment.Far; //顶部对齐
        myAxis.Title.Visible = true; //显示标题
        myAxis.Title.Font = new Font("宋体", 9.0f);

        Color color = series.View.Color;//设置坐标的颜色和图表线条颜色一致

        myAxis.Title.TextColor = color;
        myAxis.Label.TextColor = color;
        myAxis.Color = color;

        return myAxis;
    }

    #endregion


    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dtExpor = (DataTable)ViewState["grid"];
        if (dtExpor.Rows.Count < 1) return;
        if (dtExpor.Columns.Contains("PRO_NAME"))
            dtExpor.Columns.Remove("PRO_NAME");

        Session["grid"] = dtExpor;
        //Response.Redirect("../Master/TableToExcelTemple.aspx", true);        
        Response.Redirect("../Master/ExcelTemplate.aspx", true);
    }

    protected void grid_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        this.grid.DataSource = (DataTable)ViewState["grid"];
        this.grid.DataBind();
    }
}
