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
using Astronergy.MES.Report.DataAccess;
using DevExpress.Web;

public partial class QueryNormalClientRpt : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deStartDate.Date = DateTime.Now;
            deEndDate.Date = DateTime.Now;
            BindFactory();
        }
    }
    protected void btnQuey_Click(object sender, EventArgs e)
    {
        string sStartDate, sEndDate, sGradCode, sGradName, sNO, sItem, sItem1, sItem2, sQstartdate, sQenddate, sDateParam, sItem3, sFactory;
        int i = 0;
        DateTime dtStartDate, dtEndDate, dtParam;

        sStartDate = deStartDate.Text.Trim();
        sEndDate = deEndDate.Text.Trim();
        dtStartDate = Convert.ToDateTime(sStartDate);
        dtEndDate = Convert.ToDateTime(sEndDate);
        //sFactory = cboFactory.Text.Trim();
        sFactory = cboFactory.Value.ToString();
        if (sFactory == "ALL")
        {
            sFactory = "";
        }
        sNO = "序号";
        sItem = "项目";
        sItem1 = "块数";
        sItem2 = "瓦数";
        sItem3 = "实际功率";

        if (sStartDate == "" || sEndDate == "")
        {
            base.ShowMessageBox(this.Page, "查询参数不能都为空！");
            return;
        }
        if (Convert.ToDateTime(sStartDate) > Convert.ToDateTime(sEndDate))
        {
            base.ShowMessageBox(this.Page, "起始日期不能大于结止日期！");
            return;
        }

        DataTable dtWareHouse = new DataTable();
        dtWareHouse.Columns.Add("WHNO");
        dtWareHouse.Columns.Add("WHITEM");

        dtParam = dtStartDate;
        while (dtParam <= dtEndDate)
        {
            dtWareHouse.Columns.Add("WH" + (i * 3 + 2).ToString().Trim());
            dtWareHouse.Columns.Add("WH" + (i * 3 + 2 + 1).ToString().Trim());
            dtWareHouse.Columns.Add("WH" + (i * 3 + 2 + 2).ToString().Trim());
            i = i + 1;
            dtParam = dtStartDate.AddDays(i);
        }
        object[] objrow1 = new object[i * 3 + 2];
        object[] objrow2 = new object[i * 3 + 2];
        objrow1[0] = (object)sNO;
        objrow1[1] = (object)sItem;
        objrow2[0] = (object)sNO;
        objrow2[1] = (object)sItem;
        for (int r = 0; r < i; r++)
        {
            objrow1[r * 3 + 2] = (object)dtStartDate.AddDays(r).ToString("yyyy-MM-dd");
            objrow1[r * 3 + 2 + 1] = (object)dtStartDate.AddDays(r).ToString("yyyy-MM-dd");
            objrow1[r * 3 + 2 + 2] = (object)dtStartDate.AddDays(r).ToString("yyyy-MM-dd");
            objrow2[r * 3 + 2] = (object)sItem1;
            objrow2[r * 3 + 2 + 1] = (object)sItem2;
            objrow2[r * 3 + 2 + 2] = (object)sItem3;
        }
        dtWareHouse.Rows.Add(objrow1);
        dtWareHouse.Rows.Add(objrow2);

        sQstartdate = sStartDate + " 08:00:00";
        sQenddate = Convert.ToDateTime(sEndDate).AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00";

        DataSet dsGradDate = Astronergy.MES.Report.DataAccess.WareHouse.WareHouseDate.GetGrade();
        DataSet dsWareHouseDate = Astronergy.MES.Report.DataAccess.WareHouse.WareHouseDate.GetWareHouseData(sFactory, sQstartdate, sQenddate);

        for (int g = 0; g < dsGradDate.Tables[0].Rows.Count; g++)
        {
            sGradCode = dsGradDate.Tables[0].Rows[g]["COLUMN_CODE"].ToString().Trim();
            sGradName = dsGradDate.Tables[0].Rows[g]["COLUMN_NAME"].ToString().Trim();
            dtStartDate = Convert.ToDateTime(sStartDate);
            dtEndDate = Convert.ToDateTime(sEndDate);
            dtParam = dtStartDate;
            object[] objrow = new object[i * 3 + 2];
            objrow[0] = (object)(g + 1).ToString();
            objrow[1] = (object)sGradName;
            int d = 0;
            string sQty, sPowerSum, sPMSum;
            while (dtParam <= dtEndDate)
            {
                sDateParam = dtParam.ToString("yyyy-MM-dd");
                dsWareHouseDate.Tables[0].DefaultView.RowFilter = "GRADE='" + sGradCode + "' AND  WH_DTAE='" + sDateParam + "'";
                if (dsWareHouseDate.Tables[0].DefaultView.Count > 0)
                {
                    sQty = dsWareHouseDate.Tables[0].DefaultView[0]["WH_QTY"].ToString();
                    sPowerSum = Convert.ToDouble(dsWareHouseDate.Tables[0].DefaultView[0]["WH_POWER"].ToString()).ToString("##0.00");
                    sPMSum = Convert.ToDouble(dsWareHouseDate.Tables[0].DefaultView[0]["WH_PM"].ToString()).ToString("##0.00");
                    objrow[d * 3 + 2] = (object)sQty;
                    objrow[d * 3 + 2 + 1] = (object)sPowerSum;
                    objrow[d * 3 + 2 + 2] = (object)sPMSum;
                }
                d = d + 1;
                dtParam = dtStartDate.AddDays(d);
            }
            dtWareHouse.Rows.Add(objrow);
        }

        object[] objtot = new object[i * 3 + 2];
        int ntot = dsGradDate.Tables[0].Rows.Count + 1;
        objtot[0] = (object)ntot.ToString();
        objtot[1] = (object)"合计";
        int totd = 0;
        dtStartDate = Convert.ToDateTime(sStartDate);
        dtEndDate = Convert.ToDateTime(sEndDate);
        dtParam = dtStartDate;
        while (dtParam <= dtEndDate)
        {
            sDateParam = dtParam.ToString("yyyy-MM-dd");
            var totqty = (from qty in dsWareHouseDate.Tables[0].AsEnumerable()
                          where qty.Field<string>("WH_DTAE") == sDateParam
                          select qty.Field<int>("WH_QTY")).Sum();
            var totpower = (from power in dsWareHouseDate.Tables[0].AsEnumerable()
                            where power.Field<string>("WH_DTAE") == sDateParam
                            select power.Field<decimal>("WH_POWER")).Sum();
            var totpm = (from pm in dsWareHouseDate.Tables[0].AsEnumerable()
                        where pm.Field<string>("WH_DTAE") == sDateParam
                        select pm.Field<decimal>("WH_PM")).Sum();
            //int dtotqty = int.Parse(totqty.ToString());
            //decimal dtotpower = decimal.Parse(totpower.ToString());
            objtot[totd * 3 + 2] = (object)totqty.ToString();
            objtot[totd * 3 + 2 + 1] = (object)totpower.ToString("##0.00");
            objtot[totd * 3 + 2 + 2] = (object)totpm.ToString("##0.00");

            totd = totd + 1;
            dtParam = dtStartDate.AddDays(totd);
        }

        dtWareHouse.Rows.Add(objtot);

        //Cache[Session.SessionID + "WareHouse"] = dtWareHouse;
        gvWareHouse.DataSource = null;
        gvWareHouse.Columns.Clear();
        gvWareHouse.AutoGenerateColumns = true;
        gvWareHouse.DataSource = dtWareHouse;
        gvWareHouse.DataBind();
    }
    protected void gvWareHouse_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        if (gv != null && gv.Columns.Count > 1)
        {
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                gv.Columns[i].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            }
        }
    }
    protected void gvWareHouse_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        DataTable dt = gvWareHouse.DataSource as DataTable;
        if (e.RowType == GridViewRowType.Data && dt != null)
        {

            string productModelName = Convert.ToString(dt.Rows[e.VisibleIndex]["WHNO"]);
            int mergeCellCount = dt.Select(string.Format("WHNO='{0}'", productModelName)).Count();
            //合并序号单元格。
            if (e.VisibleIndex % mergeCellCount == 0)
            {
                e.Row.Cells[0].RowSpan = mergeCellCount;
            }
            else
            {
                e.Row.Cells[0].Visible = false;
            }
        }
        if (e.RowType == GridViewRowType.Data && dt != null)
        {

            string productModelName = Convert.ToString(dt.Rows[e.VisibleIndex]["WHITEM"]);
            int mergeCellCount = dt.Select(string.Format("WHITEM='{0}'", productModelName)).Count();
            //合并项目单元格。
            if (e.VisibleIndex % mergeCellCount == 0)
            {
                e.Row.Cells[1].RowSpan = mergeCellCount;
            }
            else
            {
                e.Row.Cells[1].Visible = false;
            }
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable dtExcel = (DataTable)Cache[Session.SessionID + "WareHouse"];
        if (dtExcel == null)
        {
            btnQuey_Click(sender, e);
        }
        else
        {
            gvWareHouse.DataSource = dtExcel;
            gvWareHouse.DataBind();
        }
        this.exporter.WriteXlsToResponse();
    }

    public void BindFactory()
    {
        DataSet dsFactory = Astronergy.MES.Report.DataAccess.WareHouse.WareHouseDate.GetFactoryWorkPlace();
        cboFactory.DataSource = dsFactory.Tables[0];
        cboFactory.TextField = "LOCATION_NAME";
        cboFactory.ValueField = "LOCATION_KEY";
        cboFactory.DataBind();
        cboFactory.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        cboFactory.SelectedIndex = 0;
    }
}
