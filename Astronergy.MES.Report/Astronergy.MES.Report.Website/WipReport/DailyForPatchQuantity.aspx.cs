using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraCharts.Web;
using System.Data;
using DevExpress.XtraCharts;
using System.Collections;
using Astronergy.MES.Report.DataAccess;

public partial class WipReport_DailyForPatchQuantity : BasePage
{
    string pro_id = string.Empty, workorder = string.Empty, partype = string.Empty, stime = string.Empty, qtyCells = string.Empty;
    string etime = string.Empty, sType = string.Empty, locationkey = string.Empty, reason_code_class = string.Empty, shift = string.Empty, daytype = string.Empty;
    DailyReportEntity _entity = new DailyReportEntity();
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
            reason_code_class = Server.UrlEncode(Request.Params["reason_code_class"].ToString());
            shift = Server.UrlDecode(Request.Params["shift"].ToString());
            daytype = Server.UrlDecode(Request.Params["daytype"].ToString());

            qtyCells = Server.UrlDecode(Request.Params["cells"].ToString());
            BindGvAndChart();
        }
    }

    private void BindGvAndChart()
    {
        if (!string.IsNullOrEmpty(partype))
            _entity.Pro_Type = partype;      
        if (!string.IsNullOrEmpty(locationkey))
            _entity.LocationKeys = locationkey;
        if (!string.IsNullOrEmpty(reason_code_class))
            _entity.Reason_Code_Class = reason_code_class;
        if (!string.IsNullOrEmpty(pro_id))
            _entity.Pro_Ids = pro_id;
        if (!string.IsNullOrEmpty(workorder))
            _entity.WoNumbers = workorder;
        if (daytype.Equals("1"))
        {
            if (!string.IsNullOrEmpty(stime))
                _entity.Start_Time = stime;
            if (!string.IsNullOrEmpty(etime))
                _entity.End_Time = etime;
        }

        if (daytype.Equals("0"))
        {
            string errMsg = string.Empty;
            string[] lst = CommonFunction.GetOptShiftDate(locationkey, "物料工序", shift, out errMsg, Convert.ToDateTime(stime).ToString("yyyy-MM-dd"));
          
            if (!string.IsNullOrEmpty(errMsg))
            {
                base.ShowMessageBox(this.Page, errMsg);
                return;
            }
            _entity.Start_Time = lst[0];
            _entity.End_Time = lst[1];
        }

        DataSet dsReturn = _entity.GetDailyForPatchData();
        if (!string.IsNullOrEmpty(_entity.ErrorMsg))
        {
            ShowMessageBox(this.Page, _entity.ErrorMsg);
            return;
        }      
        DataTable dtGv = dsReturn.Tables[0];

        if (dtGv.Rows.Count < 1) return;

        decimal qtySubPatch=Convert.ToDecimal( dtGv.Compute("sum(PATCH_QUANTITY)",null));
        decimal QtyPressCells = Convert.ToDecimal(qtyCells);
        QtyPressCells += qtySubPatch;

        DataTable dtChart = dtGv.Copy();
        //添加所占用比例
        #region
        if (!dtGv.Columns.Contains(LayoutViewType.ReportPressCells))
            dtGv.Columns.Add(LayoutViewType.ReportPressCells);
        foreach (DataRow dr in dtGv.Rows)
        {
            decimal QtyDtlCells = Convert.ToInt16(dr["PATCH_QUANTITY"]);
            dr[LayoutViewType.ReportPressCells] = Math.Round(QtyDtlCells / QtyPressCells, 4).ToString("P");
        }
        DataRow drNew = dtGv.NewRow();
        drNew["REASON_CODE_NAME"] = "合计";
        drNew["PATCH_QUANTITY"] = qtySubPatch;
        drNew[LayoutViewType.ReportPressCells] = Math.Round(qtySubPatch / QtyPressCells, 4).ToString("P");
        dtGv.Rows.Add(drNew);

        DataRow drNewTotal = dtGv.NewRow();
        drNewTotal["REASON_CODE_NAME"] = "总计";
        drNewTotal["PATCH_QUANTITY"] = QtyPressCells;
        drNewTotal[LayoutViewType.ReportPressCells] = "(投入数量+不良数量)";
        dtGv.Rows.Add(drNewTotal);
        #endregion
        DataTable dtGrid = base.LoadColumnsResource(dtGv);
        this.gvPatchDisplay.DataSource = dtGrid;
        this.gvPatchDisplay.DataBind();
        ViewState["grid"] = dtGrid;

        BindChart(dtChart, this.chart);
    }
    /// <summary>
    /// 给Chart绑定数据
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="chart"></param>
    private void BindChart(DataTable dt, WebChartControl chart)
    {
        chart.Series.Clear();

        foreach (DataRow dr in dt.Rows)
        {
            Series s = new Series(Convert.ToString(dr["REASON_CODE_NAME"]), ViewType.Bar);
            s.Points.Add(new SeriesPoint(dr["REASON_CODE_NAME"], dr["PATCH_QUANTITY"]));
            chart.Series.Add(s);
        }
               
    }

    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        if (ViewState["grid"] == null) return;

        DataTable dtExpor = (DataTable)ViewState["grid"];
        if (dtExpor.Rows.Count < 1) return;
        if (dtExpor.Columns.Contains("PRO_NAME"))
            dtExpor.Columns.Remove("PRO_NAME");

        Session["grid"] = dtExpor;
        //Response.Redirect("../Master/TableToExcelTemple.aspx", true);        
        Response.Redirect("../Master/ExcelTemplate.aspx", true);
    }
    protected void gvPatchDisplay_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.Index != 0)
        {
            try
            {
                //stime = Server.UrlDecode(Request.Params["paraDate01"].ToString());
                //etime = Server.UrlDecode(Request.Params["paraDate02"].ToString());
                //locationkey = Server.UrlDecode(Request.Params["locationKey"].ToString());
                //partype = Server.UrlDecode(Request.Params["partType"].ToString());
                //pro_id = Server.UrlDecode(Request.Params["Pro_id"].ToString());
                //workorder = Server.UrlDecode(Request.Params["workorder"].ToString());
                //reason_code_class = Server.UrlEncode(Request.Params["reason_code_class"].ToString());

                string startDate = string.Empty, endDate = string.Empty;
                string headColumnName = e.Column.FieldName;
                DataRow dr = (DataRow)gvPatchDisplay.GetDataRow(e.VisibleRowIndex);
                string rowKey = dr[gvPatchDisplay.KeyFieldName].ToString();

                if (headColumnName.ToUpper().Trim() == "REASON_CODE_NAME" || headColumnName.ToUpper().Trim() == "PressCells")
                    return;

                if (rowKey.Equals("合计") || rowKey.Equals("总计")) return;

                e.DisplayText = string.Format("<a  target='_blank' href=\"CustCheckAndWarehouse.aspx?locationKey={1}&partType={2}&paraDate01={3}&paraDate02={4}&sType={5}&Pro_id={6}&opt={8}&workorder={7}&reason_code_class={9}\" class=\'dxgv\' style=\'text-decoration:underline;\'>{0}</a>", e.Value,
                Server.UrlEncode(_entity.LocationKeys), Server.UrlEncode(_entity.Pro_Type),
                Server.UrlEncode(_entity.Start_Time), Server.UrlEncode(_entity.End_Time),
                Server.UrlEncode(Resources.Lang.PATCH_QUANTITY), Server.UrlEncode(_entity.Pro_Ids),
                Server.UrlEncode(_entity.WoNumbers), Server.UrlEncode(rowKey),
                Server.UrlEncode(_entity.Reason_Code_Class));

            }
            catch (Exception ex)
            { }
        }
    }
}
