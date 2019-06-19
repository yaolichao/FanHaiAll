using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Astronergy.MES.Report.DataAccess;

public partial class Quality_ModuleDefectDataDetail : BasePage
{
    string pro_id = string.Empty, workorder = string.Empty, partype = string.Empty, stime = string.Empty;
    string etime = string.Empty, sType = string.Empty, locationkey = string.Empty, reason_code_class = string.Empty,partNumber = string.Empty;
    DefectDisplayAccess _entity = new DefectDisplayAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            stime = Server.UrlDecode(Request.Params["paraDate01"].ToString());
            etime = Server.UrlDecode(Request.Params["paraDate02"].ToString());
            locationkey = Server.UrlDecode(Request.Params["locationKey"].ToString());
            partype = Server.UrlDecode(Request.Params["partType"].ToString());
            pro_id = Server.UrlDecode(Request.Params["Pro_id"].ToString());
            workorder = Server.UrlDecode(Request.Params["workorder"].ToString());
            reason_code_class = Server.UrlDecode(Request.Params["reason_code_class"].ToString());
            sType = Server.UrlDecode(Request.Params["sType"].ToString());
            partNumber = Server.UrlDecode(Request.Params["partNumber"].ToString());
            BindGvAndChart();
        }
    }
    private void BindGvAndChart()
    {
        if (!string.IsNullOrEmpty(partype))
            _entity.ProTypes = partype;
        if (!string.IsNullOrEmpty(stime))
            _entity.StartDate = stime;
        if (!string.IsNullOrEmpty(etime))
            _entity.EndDate = etime;
        if (!string.IsNullOrEmpty(locationkey))
            _entity.LocationKey = locationkey;
        if (!string.IsNullOrEmpty(reason_code_class))
            _entity.Reason_Code_Class = reason_code_class;
        if (!string.IsNullOrEmpty(pro_id))
            _entity.Pro_Ids = pro_id;
        if (!string.IsNullOrEmpty(workorder))
            _entity.WorkOrders = workorder;
        if (!string.IsNullOrEmpty(sType))
            _entity.sType = sType;
        if (!string.IsNullOrEmpty(partNumber))
            _entity.PartNumber = partNumber;
       DataTable dtReturn = new DataTable();
       if (!string.IsNullOrEmpty(sType))
           dtReturn = _entity.GetStatisByReasonCode();
       else
           dtReturn = _entity.GetDefectByReasonCode();

        if (!string.IsNullOrEmpty(_entity.ErrorMsg))
        {
            ShowMessageBox(this.Page, _entity.ErrorMsg);
            return;
        }
        DataTable dtGv = base.LoadColumnsResource(dtReturn);
        this.gvPatchDisplay.DataSource = dtGv;
        this.gvPatchDisplay.DataBind();
        ViewState["grid"] = dtReturn;

       
    }

    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dtExpor = (DataTable)ViewState["grid"];
        if (dtExpor.Rows.Count < 1) return;

        Session["grid"] = dtExpor;
        //Response.Redirect("../Master/TableToExcelTemple.aspx", true);        
        Response.Redirect("../Master/ExcelTemplate.aspx", true);
    }

    protected void gvPatchDisplay_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        if (ViewState["grid"] != null)
        {
            this.gvPatchDisplay.DataSource = ViewState["grid"] as DataTable;
        }
    }
}
