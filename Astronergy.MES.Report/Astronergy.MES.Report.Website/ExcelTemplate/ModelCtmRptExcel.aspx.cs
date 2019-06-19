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

public partial class ExcelTemplate_ModelCtmRptExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["dsBCPLotWO"];
        DataTable dt = (DataTable)Cache[Session.SessionID + "CTM"];
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();

        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Buffer = true;
        System.Web.HttpContext.Current.Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
        System.Web.HttpContext.Current.Response.Write("<head>");
        System.Web.HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312>");//解决中文乱码问题,导出数据在20行内容易出现乱码
        System.Web.HttpContext.Current.Response.Write("<!--[if gte mso 9]><xml>");
        System.Web.HttpContext.Current.Response.Write("<x:ExcelWorkbook>");
        System.Web.HttpContext.Current.Response.Write("<x:ExcelWorksheets>");
        System.Web.HttpContext.Current.Response.Write("<x:ExcelWorksheet>");
        System.Web.HttpContext.Current.Response.Write("<x:Name>sheet1</x:Name>");
        System.Web.HttpContext.Current.Response.Write("<x:WorksheetOptions>");
        System.Web.HttpContext.Current.Response.Write("<x:Print>");
        System.Web.HttpContext.Current.Response.Write("<x:ValidPrinterInfo/>");
        System.Web.HttpContext.Current.Response.Write("</x:Print>");
        System.Web.HttpContext.Current.Response.Write("</x:WorksheetOptions>");
        System.Web.HttpContext.Current.Response.Write("</x:ExcelWorksheet>");
        System.Web.HttpContext.Current.Response.Write("</x:ExcelWorksheets>");
        System.Web.HttpContext.Current.Response.Write("</x:ExcelWorkbook>");
        System.Web.HttpContext.Current.Response.Write("</xml>");
        System.Web.HttpContext.Current.Response.Write("<![endif]--> ");
        System.Web.HttpContext.Current.Response.Write("</head>");
        System.Web.HttpContext.Current.Response.Write("<body><table><tr><td colspan=46 style='height:20.00pt;font-size:12pt;font-weight:bold;'align='center'>" + "ModelCtmRpt" + "</td></tr></table>");

        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ModelCtmRpt.xls");
        HttpContext.Current.Response.Charset = "UTF-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword
        this.GridView1.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        this.GridView1.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
        tw.Close();
        hw.Close();
        GC.Collect();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }
}
