using System;
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
using System.Threading;
using DevExpress.Web.Rendering;

/// <summary>
/// SE VIBreakdown
/// </summary>
public partial class Customer_SE_VIBreakdown : BasePage
{
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
            BindCustomerType();
            DateTime dtNow = CommonFunction.GetCurrentDateTime();
            this.dateStart.Date = DateTimeHelper.GetWeekFirstDayMon(dtNow);
            this.dateEnd.Date = DateTimeHelper.GetWeekLastDaySun(dtNow);
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
    /// 查询SE VIBreakdown 数据。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataTable dtData = null;
        if (GetData(out dtData))
        {
            dtData.DefaultView.Sort = "Type ASC";
            this.grid.Columns.Clear();
            this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = dtData.DefaultView;
            this.grid.DataBind();
            this.grid.AutoGenerateColumns = false;
            this.UpdatePanelResult.Update();
        }
    }

    public bool GetData(out DataTable dt)
    {
        dt = null;
        if (string.IsNullOrEmpty(this.dateStart.Text)
            || string.IsNullOrEmpty(this.dateEnd.Text))
        {
            return false;
        }
        if ((this.dateEnd.Date - this.dateStart.Date).TotalDays < 0
            || (this.dateEnd.Date - this.dateStart.Date).TotalDays > 31)
        {
            return false;
        }
        string roomName = this.cmbWorkPlace.Text;
        string customer = this.ddeCustomerType.Text.Trim();
        string workOrderNo = this.txtWorkOrderNumber.Text;
        string partNumber = this.textPartNumber.Text.ToString().Trim();

        string startTime = this.dateStart.Date.ToString("yyyy-MM-dd");
        string endTime = this.dateEnd.Date.ToString("yyyy-MM-dd");

        if (roomName.ToUpper() == "ALL") roomName = string.Empty;
        if (customer.ToUpper() == "ALL") customer = string.Empty;

        DataSet ds = SEReportDataAccess.GetSEVIBreakdownData(startTime, endTime, roomName, workOrderNo, partNumber, customer);
        dt = ds.Tables[0];
        Cache[Session.SessionID + "_SEVIBreakdown"] = dt;
        return true;
    }
    /// <summary>
    /// 导出EXCEL。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {

        DataTable dt = Cache[Session.SessionID + "_SEVIBreakdown"]  as DataTable;
        if (dt == null)
        {
            if (!GetData(out dt))
            {
                return;
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        Response.AppendHeader("content-disposition", "attachment;filename=\"SEVIBreakdown.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToSEVIBreakdownExcel(Response.OutputStream,dt);
        Response.Flush();
        Response.Close();
        Response.End();
    }
    /// <summary>
    /// 自定义文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        
    }

    /// <summary>
    /// 行创建时触发事件，用于合并单元格。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        DataView dv = grid.DataSource as DataView;
        if (e.RowType == GridViewRowType.Data && dv != null)
        {

            for (int i = 2; i < dv.Table.Columns.Count; i++)
            {
                double val = Convert.ToDouble(dv[e.VisibleIndex][i]);
                if (val > 0)
                {
                    e.Row.Cells[i].Attributes.CssStyle.Add(HtmlTextWriterStyle.BackgroundColor, "yellow");
                    e.Row.Cells[i].Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "red");
                }
            }

            string type = Convert.ToString(dv[e.VisibleIndex]["Type"]);
            string preType = string.Empty;
            if (e.VisibleIndex > 0)
            {
                preType = Convert.ToString(dv[e.VisibleIndex - 1]["Type"]);
            }
            //合并类型单元格。
            if (preType != type)
            {
                int mergeTypeCellCount = dv.Table.Select(string.Format("Type='{0}'", type)).Count();
                e.Row.Cells[0].RowSpan = mergeTypeCellCount;
            }
            else
            {
                e.Row.Cells[0].Visible = false;
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
        //    && e.Column != null 
        //    && e.Column.Index > 1 
        //    && e.Column.Index < dt.Columns.Count-1)
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
        //ASPxGridView gv = sender as ASPxGridView;
        //if (gv != null && gv.Columns.Count > 1)
        //{
        //    gv.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
        //    gv.Columns[0].CellStyle.BackColor = Color.FromName("#ffffd6");
        //}
    }
}

