using System;
using System.Collections;
using System.Collections.Generic;
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
using DevExpress.Charts;
using DevExpress.Web;
using System.Text;
using Astronergy.MES.Report.DataAccess;
using System.Drawing;

/// <summary>
/// 组件车间日常营运报表 
/// </summary>
public partial class WipReport_DailyReport: BasePage
{
    private DailyReportDataAccess dailyReport = new DailyReportDataAccess();
    private const string START_TIME = "08:00:00";
    private const string END_TIME = "08:00:00";
    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"TOP",                               "TOP"},
        {"WAREHOUSE",                         "入库统计"},
        {"WAREHOUSE_REWORK",                  "返工入库统计"},
        {"CHECK",                             "终检统计"},
        {"W_BRACKET",                         "入库档位分布"},
        {"T_BRACKET",                         "测试档位分布"},
        {"FRAG",                              "电池片碎片统计"},
        {"WIP",                               "在制品数量统计"},
        {"OUT",                               "产出数量统计"},
        {"CTM",                               "加权计算统计"}
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
            BindProductId();
            BindShift();
            DateTime dtNow = CommonFunction.GetCurrentDateTime();
            this.dateStart.Value = dtNow.AddDays(-1).ToString("yyyy-MM-dd");
            this.dateEnd.Value = dtNow.ToString("yyyy-MM-dd");
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
    /// 绑定产品ID号。
    /// </summary>
    private void BindProductId()
    {
        DataTable dtCustomerType = CommonFunction.GetProId();
        ASPxListBox lst = this.ddeProductId.FindControl("lstProductId") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dtCustomerType;
            lst.TextField = "PRODUCT_CODE";
            lst.ValueField = "PRODUCT_CODE";
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
    /// <summary>
    /// 绑定班别名称。
    /// </summary>
    private void BindShift()
    {
        DataTable dtShift = CommonFunction.GetCAL_SHIFT();
        this.cmbShift.DataSource = dtShift;
        this.cmbShift.DataBind();
        ListEditItem item_shift = new ListEditItem();
        item_shift.Value = "";
        item_shift.Text = "ALL";
        this.cmbShift.Items.Insert(0, item_shift);
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
    /// 查询日运营报表数据。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        int queryType = this.rbtByDay.Checked ? 1 : 0;
        string roomKey = this.cmbWorkPlace.Text;
        string shiftName = Convert.ToString(this.cmbShift.Text);
        string proId = this.ddeProductId.Text.Trim();
        string productModel = this.ddeProductModel.Text.Trim();
        string workOrderNo = this.ddeWorkOrderNumber.Text.Trim();
        string factoryshift = this.cmbFactoryShift.Text.Trim();
        string partNumber = this.txtPartNumber.Text.Trim();
        string startTime = this.dateStart.Value;
        string endTime = this.dateEnd.Value;

        if (roomKey.ToUpper() == "ALL") roomKey = string.Empty;
        if (workOrderNo.ToUpper() == "ALL") workOrderNo = string.Empty;
        if (productModel.ToUpper() == "ALL") productModel = string.Empty;
        if (proId.ToUpper() == "ALL") proId = string.Empty;
        if (partNumber.ToUpper() == "ALL") partNumber = string.Empty;
        if (shiftName.ToUpper() == "ALL") shiftName = string.Empty;
        if (factoryshift.ToUpper() == "ALL") factoryshift = string.Empty;


        DataSet ds = dailyReport.GetDailyReportData(queryType, startTime, endTime, shiftName, roomKey,
                                                    productModel, proId, partNumber, workOrderNo, factoryshift);
        if (ds != null)
        {
            DataTable dt = dailyReport.TransferDatatable(ds.Tables[0], queryType, dic);
            Cache[Session.SessionID + "_DailyReport"] = dt;
            this.grid.Columns.Clear();
            this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = dt;
            this.grid.DataBind();
            this.grid.AutoGenerateColumns = false;
        }
        this.UpdatePanelResult.Update();
    }

    /// <summary>
    /// 导出EXCEL。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dt = Cache[Session.SessionID + "_DailyReport"] as DataTable;
        if (dt == null)
        {
            btnQuery_Click(sender, e);
            dt = Cache[Session.SessionID + "_DailyReport"] as DataTable;
        }
        dt.Columns.Remove("DATA_TYPE");
        dt.Columns.Remove("DATA_FORMAT");
        dt.Columns.Remove("DATA_DETAIL_PAGE");
        dt.Columns["DATA_DESCRIPTION"].Caption = "描述";
        dt.Columns["DATA_NAME"].Caption = "项目";

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("content-disposition", "attachment;filename=\"DailyReport.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToExcel(Response.OutputStream, dt);
        Response.End();       
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
        if (dt != null 
            && e.Column.Index > 3 
            && e.Column.Index < dt.Columns.Count - 1 && 
            !string.IsNullOrEmpty(Convert.ToString(e.Value)))
        {
            int queryType = this.rbtByDay.Checked ? 1 : 0;
            string roomName = this.cmbWorkPlace.Text;
            string proId = this.ddeProductId.Text.Trim();
            string workOrderNo = this.ddeWorkOrderNumber.Text.Trim();
            string productModel = this.ddeProductModel.Text.Trim();
            string shiftName = this.cmbShift.Text;
            DateTime startTime = Convert.ToDateTime(this.dateStart.Value);
            DateTime endTime = Convert.ToDateTime(this.dateEnd.Value);
            string colName = e.Column.FieldName;
            string factoryshift = this.cmbFactoryShift.Text.Trim();
       
            //非ALL 列
            if (e.Column.Index>4)
            {
                startTime =  Convert.ToDateTime(colName);
                endTime =  Convert.ToDateTime(colName);
            }
            string dataStartTime = startTime.ToString("yyyy-MM-dd");
            string dataEndTime = endTime.ToString("yyyy-MM-dd");
            string date = startTime.ToString("yyyy-MM-dd");
            if (queryType == 0)
            {
                endTime = endTime.AddHours(1);
                dataStartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
                dataEndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            int dataFormat = Convert.ToInt32(dt.Rows[e.VisibleRowIndex]["DATA_FORMAT"]);
            string dataDetailPage = Convert.ToString(dt.Rows[e.VisibleRowIndex]["DATA_DETAIL_PAGE"]).Trim();
            string dataType = Convert.ToString(dt.Rows[e.VisibleRowIndex]["DATA_TYPE"]);
            string dataName = Convert.ToString(dt.Rows[e.VisibleRowIndex]["DATA_NAME"]);
            string displayText = Convert.ToString(e.Value);
            if (dataFormat == 1 && e.Value!=null && e.Value!=DBNull.Value)
            {
                double val = Convert.ToDouble(e.Value);
                if(dataType == "FRAG")
                {
                    displayText = val.ToString("#0.00##%");
                }
                else
                {
                    displayText = val.ToString("#0.00%");
                }
                e.DisplayText = displayText;
            }

            if (!string.IsNullOrEmpty(dataDetailPage) && !string.IsNullOrEmpty(displayText))
            {
                //产出数量链接明细。
                if (dataType == "OUT")
                {
                    string url = string.Format(@"WipOperationOutputDetail.aspx?date={0}&shiftName={1}&startTime={2}&endTime={3}&trxType={4}&stepName={5}&roomName={6}&proId={7}&workOrderNo={8}&isHistory={9}&factoryshift={10}&partNumber={11}",
                                  Server.UrlEncode(date),
                                  Server.UrlEncode(shiftName),
                                  Server.UrlEncode(dataStartTime),
                                  Server.UrlEncode(dataEndTime),
                                  Server.UrlEncode(queryType==1?"DAILY_OUT_QTY":"OUT_QTY"),
                                  Server.UrlEncode(dataName.Replace("产出数量", string.Empty)),
                                  Server.UrlEncode(roomName),
                                  Server.UrlEncode(proId),
                                  Server.UrlEncode(workOrderNo),
                                  1,
                                  Server.UrlEncode(factoryshift),
                                  Server.UrlEncode(this.txtPartNumber.Text.Trim()));

                    e.DisplayText = string.Format("<a href=\"javascript:void(0);\" onclick=\"javascript:window.open('{1}')\" class=\"dxgv\" style=\"text-decoration:underline;\">{0}</a>",
                         displayText, url);
                }
            }
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
        if (e.RowType == GridViewRowType.Data && dt != null)
        {
            string dataType = Convert.ToString(e.GetValue("DATA_TYPE"));
            string dataName = Convert.ToString(e.GetValue("DATA_NAME"));
            if(dic[dataType]==dataName)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.CssStyle.Add(HtmlTextWriterStyle.BackgroundColor, "lightblue");
                }
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
        //DataTable dt = this.gridExport.DataSource as DataTable;
        //if (dt != null
        //    && e.RowType == DevExpress.Web.GridViewRowType.Data
        //    && e.Column != null && e.Column.Index > 1
        //    && e.Column.Index < dt.Columns.Count - 1)
        //{
        //    string trxType = Convert.ToString(dt.Rows[e.VisibleIndex][1]);
        //    if (trxType.EndsWith("RATE"))
        //    {
        //        e.TextValueFormatString = "#0.00%";
        //    }
        //}
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
            gv.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
            gv.Columns[0].Visible = false;
            gv.Columns[1].Caption = "项目";
            gv.Columns[1].FixedStyle = GridViewColumnFixedStyle.Left;
            gv.Columns[1].Width = Unit.Pixel(200);
            gv.Columns["DATA_FORMAT"].FixedStyle = GridViewColumnFixedStyle.Left;
            gv.Columns["DATA_FORMAT"].Visible = false;
            gv.Columns["DATA_DETAIL_PAGE"].FixedStyle = GridViewColumnFixedStyle.Left;
            gv.Columns["DATA_DETAIL_PAGE"].Visible = false;
            gv.Columns["DATA_DESCRIPTION"].Caption = "描述";
            gv.Columns["DATA_DESCRIPTION"].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            gv.Columns["DATA_DESCRIPTION"].Width= Unit.Pixel(300);
        }
    }
}

