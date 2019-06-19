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

/// <summary>
/// 托盘数据清单。
/// </summary>
public partial class WipReport_PvelData : BasePage
{
    private PevlDataAccess dataAccess = new PevlDataAccess();
    /// <summary>
    /// 页面载入事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtSerialNo.Focus();
        }
    }

    /// <summary>
    /// 查询条码数据。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataTable dt = null;
        string serialNo = txtSerialNo.Text.Trim();
        if (string.IsNullOrEmpty(serialNo))
        {
            Response.Write("<script language='javascript'>alert('请输入条码！');</script>");
            return;
        }
        dt = dataAccess.QueryPevl(serialNo);


        this.grid.Columns.Clear();
        this.grid.AutoGenerateColumns = true;

        this.grid.DataSource = dt;
        this.grid.DataBind();
        this.grid.AutoGenerateColumns = false;

        if (dt != null)
        {
            Cache[Session.SessionID + "_PVEL"] = dt;
            this.grid.Columns[0].Width = 50;
            this.grid.Columns[1].Width = 200;
            this.grid.Columns[2].Width = 350;
            this.grid.Columns[3].Width = 100;
            this.grid.Columns[4].Width = 50;
        }
        else
        {
            Response.Write("<script language='javascript'>alert('条码不存在！');</script>");
            txtSerialNo.Text = "";
            return;
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
        DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_PVEL"];
        if (dtLotData == null)
        {
            btnQuery_Click(sender, e);
            dtLotData = (DataTable)Cache[Session.SessionID + "_PVEL"];
        }
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("content-disposition", "attachment;filename=\"PvelData.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToExcel(Response.OutputStream, dtLotData);
        Response.End();
    }
    /// <summary>
    /// 自定义文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        /*
        ASPxGridView gv = sender as ASPxGridView;
        DataTable dt = gv.DataSource as DataTable;
        if (dt != null && e.Column.Index == 1)
        {
            string palletNo = Convert.ToString(e.Value);
            string url = string.Format(@"LotData.aspx?palletNo={0}", Server.UrlEncode(palletNo));

            e.DisplayText = string.Format("<a href=\"{1}\" onmouseover=\"status='明细';return true\" title=\"明细\" target=\"_blank\" class=\"dxgv\" style=\"text-decoration:underline;\">{0}</a>",
                                          e.Value, url);
        }

        if (dt != null && dt.Columns[e.Column.FieldName].DataType == typeof(DateTime))
        {
            if (e.Value != null && e.Value != DBNull.Value)
            {
                e.DisplayText = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
         * */
    }

    /// <summary>
    /// 行创建时触发事件，用于合并单元格。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {

    }
    /// <summary>
    /// 数据绑定时触发事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        if (gv.Columns.Count > 0)
        {
            foreach (GridViewColumn gvc in gv.Columns)
            {
                gvc.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            }
            gv.Columns[0].Width = Unit.Pixel(80);
            gv.Columns[1].Width = Unit.Pixel(150);
            gv.Columns[3].Width = Unit.Pixel(150);
        }
    }
}

