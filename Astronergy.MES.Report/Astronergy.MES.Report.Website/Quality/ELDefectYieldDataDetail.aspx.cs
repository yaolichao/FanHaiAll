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
using System.Collections;
/// <summary>
/// EL不良分布明细数据。
/// </summary>
public partial class Quality_ELDefectYieldDataDetail : System.Web.UI.Page
{
    private ELDefectYieldDataAccess _elDefectYield = new ELDefectYieldDataAccess();//调用逻辑

    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"ZJ_DEFECT_QTY",                 "成品EL不良数量"},
        {"ZJ_QTY",                        "终检检验数量"},
        {"FS_DEFECT_QTY",                 "半成品EL不良数量"},
        {"FS_QTY",                        "敷设EL站数量"}
    };

    Dictionary<string, string> dicRate = new Dictionary<string, string>()
    {
        {"ZJ_DEFECT_RATE",                 "成品EL不良率"},
        {"FS_DEFECT_RATE",                 "半成品EL不良率"},
    };

    Dictionary<string, string> dicMethod = new Dictionary<string, string>()
    {
        {"ZJ_DEFECT_RATE",     "终检判定不良数/终检检验量"},
        {"FS_DEFECT_RATE",     "敷设EL站判定不良/敷设站EL测试的数量"},
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
            string trxType = Server.UrlDecode(Request.QueryString["trxType"]);
            if (dicRate.ContainsKey(trxType))
            {
                this.PagetTitle = dicRate[trxType];
                this.lblType.Text = this.PagetTitle;
            }
            if (dicMethod.ContainsKey(trxType))
            {
                this.lblCalcMethod.Text = dicMethod[trxType];
            }
            BindData();

            foreach (DictionaryEntry entry in Cache)
            {
                string key = Convert.ToString(entry.Key);
                if (key.StartsWith(Session.SessionID))
                {
                    Cache.Remove(key);    
                }
            }
        }
    }
    /// <summary>
    ///  绑定明细数据。
    /// </summary>
    private void BindData()
    {
        string startTime = Server.UrlDecode(Request.QueryString["startTime"]);
        string endTime = Server.UrlDecode(Request.QueryString["endTime"]);
        string trxType = Server.UrlDecode(Request.QueryString["trxType"]);
        string roomKey = Server.UrlDecode(Request.QueryString["roomKey"]);
        string customer = Server.UrlDecode(Request.QueryString["customer"]);
        string productModel = Server.UrlDecode(Request.QueryString["productModel"]);
        string workOrderNo = Server.UrlDecode(Request.QueryString["workOrderNo"]);
        string sQueryType = Server.UrlDecode(Request.QueryString["queryType"]);
        string partNumber = Server.UrlDecode(Request.QueryString["partNumber"]);
        
        int queryType=0;
        if(!string.IsNullOrEmpty(sQueryType))
        {
            int.TryParse(sQueryType,out queryType);
        }
        if (roomKey == "ALL" || string.IsNullOrEmpty(roomKey)) roomKey = string.Empty;
        if (workOrderNo == "ALL" || string.IsNullOrEmpty(workOrderNo)) workOrderNo = string.Empty;
        if (productModel == "ALL" || string.IsNullOrEmpty(productModel)) productModel = string.Empty;
        if (customer == "ALL" || string.IsNullOrEmpty(customer)) customer = string.Empty;

        DataSet ds = this._elDefectYield.Get(queryType, startTime, endTime, roomKey, customer, productModel, string.Empty, workOrderNo, partNumber);
        if (ds == null) return;
        DataTable dt = this._elDefectYield.TransferDatatable(ds.Tables[0], queryType, dic);
        if (queryType == 0 && dt.Columns.Count>2)
        {
            string colunName = string.Format("{0}至{1}", startTime, endTime);
            dt.Columns[dt.Columns.Count-2].ColumnName = colunName;
        }
        this.gvResults.Columns.Clear();
        this.gvResults.AutoGenerateColumns = true;
        this.gvResults.DataSource = dt;
        this.gvResults.DataBind();
        this.gvResults.AutoGenerateColumns = false;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string colVal = Convert.ToString(dt.Rows[i]["COL_VALUE"]);
            if (colVal.EndsWith("DEFECT_QTY"))
            {
                this.gvResults.DetailRows.ExpandRow(i);
            }
        }
    }


    /// <summary>
    ///  绑定优品率明细数据。
    /// </summary>
    private void BindDetailData(ASPxGridView detailGrid, string roomKey, string dataType)
    {
        DataSet ds=Cache[Session.SessionID + dataType] as DataSet;
        if (ds == null)
        {
            string startTime = Server.UrlDecode(Request.QueryString["startTime"]);
            string endTime = Server.UrlDecode(Request.QueryString["endTime"]);
            string trxType = Server.UrlDecode(Request.QueryString["trxType"]);
            //string roomKey = Server.UrlDecode(Request.QueryString["roomKey"]);
            string customer = Server.UrlDecode(Request.QueryString["customer"]);
            string productModel = Server.UrlDecode(Request.QueryString["productModel"]);
            string workOrderNo = Server.UrlDecode(Request.QueryString["workOrderNo"]);
            string sQueryType = Server.UrlDecode(Request.QueryString["queryType"]);
            string partNumber = Server.UrlDecode(Request.QueryString["partNumber"]);

            int queryType = 0;
            if (!string.IsNullOrEmpty(sQueryType))
            {
                int.TryParse(sQueryType, out queryType);
            }
            if (queryType == 2)
            {
                startTime = string.Format("{0} {1}", startTime, "08:00:00");
                endTime = DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM-dd 08:00:00");
            }

            //不是累计则只查当前日期的。
            if (!dataType.StartsWith("LJ_") && queryType!=0)
            {
                startTime = DateTime.Parse(endTime).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (roomKey == "ALL" || string.IsNullOrEmpty(roomKey)) roomKey = string.Empty;
            if (workOrderNo == "ALL" || string.IsNullOrEmpty(workOrderNo)) workOrderNo = string.Empty;
            if (productModel == "ALL" || string.IsNullOrEmpty(productModel)) productModel = string.Empty;
            if (customer == "ALL" || string.IsNullOrEmpty(customer)) customer = string.Empty;

            ds = this._elDefectYield.GetDetail(dataType, startTime, endTime, roomKey, customer, productModel, string.Empty, workOrderNo, partNumber);
            Cache[Session.SessionID + dataType] = ds;
        }
        detailGrid.DataSource = ds.Tables[0];

    }
    /// <summary>
    /// 导出概要数据到EXCEL表。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        BindData();
        string fileName = this.lblType.Text;
        exporter.WriteXlsToResponse(fileName);
    }
    /// <summary>
    /// 导出明细数据到EXCEL表。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDetailXlsExport_Click(object sender, EventArgs e)
    {
        ASPxButton btn=(ASPxButton)sender;
        ASPxGridView detailGrid = (btn.Parent.FindControl("detailGrid") as ASPxGridView);
        if (detailGrid != null)
        {
            string keyValue = Convert.ToString(detailGrid.GetMasterRowKeyValue());
            string roomKey = keyValue.Split('$')[0];
            string dataType = keyValue.Split('$')[1];
            DataSet ds = Cache[Session.SessionID + dataType] as DataSet;
            if (ds == null)
            {
                BindDetailData(detailGrid, roomKey, dataType);
                ds = Cache[Session.SessionID + dataType] as DataSet;
            }
            string fileName = this.lblType.Text;
            Response.Clear();
            Response.Buffer = true;
            Response.AppendHeader("content-disposition", "attachment;filename=\"" +
                                            System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ".xls\"");
            Response.ContentType = "Application/ms-excel";
            Export.ExportToExcel(Response.OutputStream, ds.Tables[0]);
            Response.End();
        }
    }
    

    /// <summary>
    /// 数据绑定时触发事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        if (gv != null && gv.Columns.Count > 1)
        {
            gv.Columns["ROOM_NAME"].Caption = "车间名称";
            gv.Columns["COL_VALUE"].Caption = " ";
            gv.Columns["COL_VALUE"].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            gv.Columns["KEY_VALUE"].Visible = false;
            gv.Columns["ROOM_KEY"].Visible = false;
        }
    }
    /// <summary>
    /// 自定义文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        string val = Convert.ToString(e.Value);
        if (e.Column.FieldName=="COL_VALUE" && dic.ContainsKey(val))
        {
            e.DisplayText = dic[Convert.ToString(e.Value)];
        }
        else if (e.Column.FieldName=="ROOM_NAME" && Convert.ToString(e.Value) == "ALL")
        {
            e.DisplayText = "总计";
        }
    }

    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        ASPxGridView gv = (sender as ASPxGridView);
        string keyValue = Convert.ToString(gv.GetMasterRowKeyValue());
        string roomKey = keyValue.Split('$')[0];
        string dataType = keyValue.Split('$')[1];
        BindDetailData(gv, roomKey, dataType);
    }
    /// <summary>
    /// 自定义文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void detailGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        DataTable dt = gv.DataSource as DataTable;
        if (dt != null && e.Column.Index == 2)
        {
            string lotKey = Convert.ToString(e.Value);
            string url = string.Format(@"../WipReport/LotDataDetail.aspx?lotkey={0}", Server.UrlEncode(lotKey));
            e.DisplayText = string.Format("<a href=\"{1}\" onmouseover=\"status='明细';return true\" title=\"明细\" target=\"_blank\" class=\"dxgv\" style=\"text-decoration:underline;\">{0}</a>",
                                          e.Value, url);
        }
    }
}