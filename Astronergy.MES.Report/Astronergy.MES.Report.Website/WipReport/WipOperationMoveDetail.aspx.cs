﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Data;
using DevExpress.Web;
using Astronergy.MES.Report.DataAccess;
/// <summary>
/// 获取工序MOVE量明细数据。
/// </summary>
public partial class WipReport_WipOperationMoveDetail : System.Web.UI.Page
{
    private WipMoveDataAccess wipMove = new WipMoveDataAccess();//调用逻辑
    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"IN_QTY",                      "送入数明细"},
        {"OUT_QTY",                     "送出数明细"},
        {"SCRAP_QTY",                   "组件报废数明细"},
        {"DEFECT_QTY",                  "组件不良数明细"},
        {"REWORK_QTY",                  "组件返修数明细"},
        {"CELL_SCRAP_QTY",              "电池片报废数明细"},
        {"CELL_BERECOVERED_QTY",        "电池片回收数明细"},
        {"WIP_QTY",                     "当前在制品明细"}
    };
    /// <summary>
    /// 页面标题。
    /// </summary>
    public string PagetTitle
    {
        get;
        private set;
    }
    /// <summary>
    /// 页面加载事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string date = Server.UrlDecode(Request.QueryString["date"]);
            string shiftName = Server.UrlDecode(Request.QueryString["shiftName"]);
            string trxType = Server.UrlDecode(Request.QueryString["trxType"]);
            lblDate.Text = date;
            lblShiftName.Text = shiftName;
            if (dic.ContainsKey(trxType))
            {
                this.PagetTitle = dic[trxType];
            }
        }
        BindData();
    }
    /// <summary>
    ///  绑定明细数据。
    /// </summary>
    private void BindData()
    {
        if (string.IsNullOrEmpty(this.hiddenCacheId.Value))
        {
            this.hiddenCacheId.Value = Guid.NewGuid().ToString();
        }
        DataTable dt = (DataTable)Cache[this.hiddenCacheId.Value];
        if (dt == null)
        {
            string startTime = Server.UrlDecode(Request.QueryString["startTime"]);
            string endTime = Server.UrlDecode(Request.QueryString["endTime"]);
            string trxType = Server.UrlDecode(Request.QueryString["trxType"]);
            string stepName = Server.UrlDecode(Request.QueryString["stepName"]);
            string roomName = Server.UrlDecode(Request.QueryString["roomName"]);
            string proId = Server.UrlDecode(Request.QueryString["proId"]);
            string workOrderNo = Server.UrlDecode(Request.QueryString["workOrderNo"]);
            string isHistory = Server.UrlDecode(Request.QueryString["isHistory"]);
            string shiftName = Server.UrlDecode(Request.QueryString["shiftName"]);
            string partNumber = Server.UrlDecode(Request.QueryString["partNumber"]);

            if (stepName == "ALL" || string.IsNullOrEmpty(stepName)) stepName = string.Empty;
            if (workOrderNo == "ALL" || string.IsNullOrEmpty(workOrderNo)) workOrderNo = string.Empty;
            if (proId == "ALL" || string.IsNullOrEmpty(proId)) proId = string.Empty;
            if (shiftName == "总计" || string.IsNullOrEmpty(shiftName)) shiftName = string.Empty;
            if (partNumber == "ALL" || string.IsNullOrEmpty(partNumber)) partNumber = string.Empty;

            DataSet  ds = wipMove.GetMoveDataDetail(startTime, endTime, roomName,stepName,
                                                    proId, workOrderNo, trxType, shiftName, isHistory, partNumber);
            if (ds != null)
            {
                Cache[this.hiddenCacheId.Value] = ds.Tables[0];
            }
            dt = (DataTable)Cache[this.hiddenCacheId.Value];
        }
        this.gvResults.DataSource = dt;
        this.gvResults.DataBind();
    }
    /// <summary>
    /// 导出明细数据到EXCEL表。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Cache[this.hiddenCacheId.Value];
        if (dt == null)
        {
            BindData();
            dt = (DataTable)Cache[this.hiddenCacheId.Value];
        }
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("content-disposition", "attachment;filename=\"OperationMoveDetail.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToExcel(Response.OutputStream, dt);
        Response.End();
    }
    /// <summary>
    /// 自定义显示文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvResults_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        DataTable dt = gv.DataSource as DataTable;
        if (dt != null && e.Column.Index == 0)
        {
            string lotKey = Convert.ToString(e.Value);
            string url = string.Format(@"LotDataDetail.aspx?lotkey={0}", Server.UrlEncode(lotKey));
            e.DisplayText = string.Format("<a href=\"{1}\" onmouseover=\"status='明细';return true\" title=\"明细\" target=\"_blank\" class=\"dxgv\" style=\"text-decoration:underline;\">{0}</a>",
                                          e.Value, url);
        }
    }

    private void AddSummaryToGridView(string trxType)
    {
        gvResults.TotalSummary.Clear();
        ASPxSummaryItem lotSummary = new ASPxSummaryItem();
        lotSummary.FieldName = "批次号";
        lotSummary.ShowInColumn = "批次号";
        lotSummary.DisplayFormat = "组件块数合计:{0}";
        lotSummary.SummaryType = SummaryItemType.Count;
        gvResults.TotalSummary.Add(lotSummary);

        ASPxSummaryItem cellSummary = new ASPxSummaryItem();
        if (trxType == "CELL_SCRAP_QTY")
        {
            cellSummary.FieldName = "电池片报废数量";
            cellSummary.ShowInColumn = "电池片报废数量";
            cellSummary.DisplayFormat = "电池片报废数量合计:{0}";
        }
        else if (trxType == "CELL_BERECOVERED_QTY")
        {
            cellSummary.FieldName = "电池片回收数量";
            cellSummary.ShowInColumn = "电池片回收数量";
            cellSummary.DisplayFormat = "电池片回收数量合计:{0}";
        }
        else
        {
            cellSummary.FieldName = "电池片数量";
            cellSummary.ShowInColumn = "电池片数量";
            cellSummary.DisplayFormat = "电池片数量合计:{0}";
        }
        cellSummary.SummaryType = SummaryItemType.Sum;
        gvResults.TotalSummary.Add(cellSummary);
    }

}