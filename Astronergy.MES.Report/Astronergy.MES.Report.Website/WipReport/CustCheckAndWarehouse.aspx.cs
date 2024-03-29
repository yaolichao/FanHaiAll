﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Astronergy.MES.Report.DataAccess;
using System.Data;

public partial class WipReport_CustCheckAndWarehouse : BasePage
{
    string pro_id = string.Empty, workorder = string.Empty, partype = string.Empty, stime = string.Empty, operation = string.Empty;
    string etime = string.Empty, sType = string.Empty, locationkey = string.Empty, reason_code_class = string.Empty;
    DailyReportEntity _entity = new DailyReportEntity();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sType = Server.UrlDecode(Request.Params["sType"].ToString());
            stime = Server.UrlDecode(Request.Params["paraDate01"].ToString());
            etime = Server.UrlDecode(Request.Params["paraDate02"].ToString());
            locationkey = Server.UrlDecode(Request.Params["locationKey"].ToString());
            partype = Server.UrlDecode(Request.Params["partType"].ToString());
            pro_id = Server.UrlDecode(Request.Params["Pro_id"].ToString());
            workorder = Server.UrlDecode(Request.Params["workorder"].ToString());
            operation = Server.UrlDecode(Request.Params["opt"].ToString());

            if (Request.Params.AllKeys.Contains("reason_code_class"))
            {
                reason_code_class = Server.UrlDecode(Request.Params["reason_code_class"].ToString());
            }

            BindGvAndChart();
        }
    }

    private void  BindGvAndChart()
    {
        if (!string.IsNullOrEmpty(partype))
            _entity.Pro_Type = partype;
        if (!string.IsNullOrEmpty(stime))
            _entity.Start_Time = stime;
        if (!string.IsNullOrEmpty(etime))
            _entity.End_Time = etime;
        if (!string.IsNullOrEmpty(locationkey))
            _entity.LocationKeys = locationkey;
        if (!string.IsNullOrEmpty(sType))
            _entity.Reason_Code_Class = sType;
        if (!string.IsNullOrEmpty(pro_id))
            _entity.Pro_Ids = pro_id;
        if (!string.IsNullOrEmpty(workorder))
            _entity.WoNumbers = workorder;
        if (!string.IsNullOrEmpty(operation))
            _entity.Operation = operation;

        _entity.Reason_Code_Class = reason_code_class;

        if (!string.IsNullOrEmpty(sType))
        {
            if (sType.Equals(Resources.Lang.KJ_QTY))
                _entity.Grade = Resources.Lang.Grade_KJ;
            if (sType.Equals(Resources.Lang.AJ_QTY))
                _entity.Grade = Resources.Lang.Grade_AJ;
            if (sType.Equals(Resources.Lang.A0J_QTY))
                _entity.Grade = Resources.Lang.Grade_A0J;
            if (sType.Equals(Resources.Lang.ERJ_QTY))
                _entity.Grade = Resources.Lang.Grade_ERJ;
            if (sType.Equals(Resources.Lang.SANJ_QTY))
                _entity.Grade = Resources.Lang.Grade_SANJ;
            if (sType.Equals(Resources.Lang.SCRAP_QTY))
                _entity.Grade = Resources.Lang.Grade_Scrap;
        }
        DataSet dsReturn = new DataSet();
        //B222:入库档位分布情况
        //B233:测试档位分布情况
        if (sType.Equals(Resources.Lang.B222))
        {
            dsReturn = _entity.GetPmaxstabLevel(0);
        }
        else if (sType.Equals(Resources.Lang.B233))
        {
            dsReturn = _entity.GetPmaxstabLevel(1);
        }
        //转进来工单
        else if (sType.Equals(Resources.Lang.REL_EXCHG_IN_QTY))
        {
            dsReturn = _entity.GetRelExchgWo(0);
        }
        //转出去工单
        else if (sType.Equals(Resources.Lang.REL_EXCHG_OUT_QTY))
        {
            dsReturn = _entity.GetRelExchgWo(1);
        }
        //CTM数据
        else if (sType.Equals(Resources.Lang.WEIGHTING_CTM))
        {
            dsReturn = _entity.GetCtmData();
        }
        //不良明细—运营报表碎片率批次明细信息
        else if (sType.Equals(Resources.Lang.PATCH_QUANTITY))
        {
            dsReturn = _entity.GetDepathData();
        }
        //入库终检序列号分布
        else
        {
            dsReturn = _entity.GetMoveOutDataByOperation();
        }

        if (!string.IsNullOrEmpty(_entity.ErrorMsg))
        {
            ShowMessageBox(this.Page, _entity.ErrorMsg);
            return;
        }
        DataTable dtGv = LoadColumnsResource(dsReturn.Tables[0]);
        ViewState["grid"] = dtGv;
        this.grid.DataSource = dtGv;
        this.grid.DataBind();
    }

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
