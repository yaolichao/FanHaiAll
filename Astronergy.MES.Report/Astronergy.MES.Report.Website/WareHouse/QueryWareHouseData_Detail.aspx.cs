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

public partial class WareHouse_QueryWareHouseData_Detail : System.Web.UI.Page
{
    private Astronergy.MES.Report.DataAccess.WareHouse.WareHouseDate whrep = new Astronergy.MES.Report.DataAccess.WareHouse.WareHouseDate();
    string sGrade = string.Empty, sDate = string.Empty, sType = string.Empty, sFactory = string.Empty, sQstartdate = string.Empty, sQenddate = string.Empty
        , sWO = string.Empty, sPowerLevel = string.Empty,partNumber=string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sGrade = Server.UrlDecode(Request.Params["Grad"].ToString());
            sDate = Server.UrlDecode(Request.Params["Qdate"].ToString().Trim());
            sType = Server.UrlDecode(Request.Params["Type"].ToString());
            sFactory = Server.UrlDecode(Request.Params["Factory"].ToString());
            sQstartdate = Server.UrlDecode(Request.Params["Qstartdate"].ToString());
            sQenddate = Server.UrlDecode(Request.Params["Qenddate"].ToString());
            sWO = Server.UrlDecode(Request.Params["WO"].ToString());
            sPowerLevel = Server.UrlDecode(Request.Params["PowerLevel"].ToString());
            partNumber = Server.UrlDecode(Convert.ToString(Request.Params["partNumber"]));
            Bind_Detail();
        }
    }

    private void Bind_Detail()
    {
        string sPowerLevelName, sItemValue;
        DataSet ds = (DataSet)Session["WareHouseDateDtl"];
        DataTable dt = new DataTable();
        dt.Columns.Add("LEVEL");
        dt.Columns.Add("VALUE");

        sPowerLevelName = "档位";
        sItemValue = "";
        if (sType == "A")
        {
            sItemValue = "组件数量(块)";
        }
        if (sType == "B")
        {
            sItemValue = "衰减功率(瓦数)";
        }
        if (sType == "C")
        {
            sItemValue = "实测功率(瓦数)";
        }

        DataSet dsWareHouseDateDtl = whrep.GetWareHouseDataNewDtl(sFactory, sQstartdate, sQenddate, sWO, sPowerLevel, sGrade, sDate, sType, partNumber);

        gvDetail.DataSource = null;
        gvDetail.Columns.Clear();
        gvDetail.AutoGenerateColumns = true;
        gvDetail.DataSource = dsWareHouseDateDtl.Tables[0];
        gvDetail.DataBind();

        gvDetail.Columns[0].Caption = sPowerLevelName;
        gvDetail.Columns[1].Caption = sItemValue;
    }
}
