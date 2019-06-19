﻿using System;
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
/// 组件入库清单报表。
/// </summary>
public partial class WareHouse_QueryWareHouseRpt : BasePage
{
    private WareHouseRptData dataAccess = new WareHouseRptData();
    /// <summary>
    /// 页面载入事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime dtNow = CommonFunction.GetCurrentDateTime();
            this.dateStart.Value = dtNow.AddDays(-1).ToString("yyyy-MM-dd 10:00:00");
            this.dateEnd.Value = dtNow.ToString("yyyy-MM-dd 10:00:00");
        }
        if (this.IsCallback)
        {
            string callbackId = Convert.ToString(Request["__CALLBACKID"]);
            if (callbackId == this.grid.UniqueID)
            {
                DataTable dtWHData = (DataTable)Cache[Session.SessionID + "_QueryWareHouseRpt"];
                if (dtWHData != null)
                {
                    this.grid.Columns.Clear();
                    this.grid.AutoGenerateColumns = true;
                    this.grid.DataSource = dtWHData;
                    this.grid.DataBind();
                    this.grid.AutoGenerateColumns = false;
                }
                else
                {
                    btnQuery_Click(sender, e);
                }
            }
        }
    }
    /// <summary>
    /// 查询组件入库信息数据。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string workOrderNo = this.ddeWorkOrderNumber.Text.Trim();
        string startTime = this.dateStart.Value;
        string endTime = this.dateEnd.Value;

        DataSet ds = dataAccess.GetWareHouseData(workOrderNo, startTime, endTime);
        if (ds != null)
        {
            Cache[Session.SessionID + "_QueryWareHouseRpt"] = ds.Tables[0];
            this.grid.Columns.Clear();
            this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = ds.Tables[0];
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
        DataTable dtWHData = (DataTable)Cache[Session.SessionID + "_QueryWareHouseRpt"];
        if (dtWHData == null)
        {
            btnQuery_Click(sender, e);
            dtWHData = (DataTable)Cache[Session.SessionID + "_QueryWareHouseRpt"];
        }
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("content-disposition", "attachment;filename=\"QueryWareHouseRpt.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToExcel(Response.OutputStream, dtWHData);
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
            gv.Columns[0].Width = Unit.Pixel(150);
            gv.Columns[1].Width = Unit.Pixel(150);
        }
    }
}
