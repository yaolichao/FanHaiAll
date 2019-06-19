using System;
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
/// 直通率报表明细数据。
/// </summary>
public partial class Quality_PassYieldDataDetail : System.Web.UI.Page
{
    private PassYieldDataAccess _passYield = new PassYieldDataAccess();//调用逻辑

    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"CY_TRACKOUT_QTY",                         "层压出站数量"},
        {"CY_REWORK_QTY",                           "层压返修数量"},
        {"CY_REWORK_1_QTY",                         "层压返工数量"},
        {"LJ_CY_TRACKOUT_QTY",                      "累计层压出站数量"},
        {"LJ_CY_REWORK_QTY",                        "累计层压返修数量"},
        {"LJ_CY_REWORK_1_QTY",                      "累计层压返工数量"},

        {"ZK_TRACKOUT_QTY",                         "装框出站数量"},
        {"ZK_REWORK_QTY",                           "装框返修数量"},
        {"ZK_REWORK_1_QTY",                         "装框返工数量"},
        {"LJ_ZK_TRACKOUT_QTY",                      "累计装框出站数量"},
        {"LJ_ZK_REWORK_QTY",                        "累计装框返修数量"},
        {"LJ_ZK_REWORK_1_QTY",                      "累计装框返工数量"},

        {"QJ_TRACKOUT_QTY",                         "清洁出站数量"},
        {"QJ_REWORK_QTY",                           "清洁返修数量"},
        {"QJ_REWORK_1_QTY",                         "清洁返工数量"},
        {"LJ_QJ_TRACKOUT_QTY",                      "累计清洁出站数量"},
        {"LJ_QJ_REWORK_QTY",                        "累计清洁返修数量"},
        {"LJ_QJ_REWORK_1_QTY",                      "累计清洁返工数量"},

        {"ZJCS_TRACKOUT_QTY",                       "组件测试出站数量"},
        {"ZJCS_REWORK_QTY",                         "组件测试返修数量"},
        {"ZJCS_REWORK_1_QTY",                       "组件测试返工数量"},
        {"LJ_ZJCS_TRACKOUT_QTY",                    "累计组件测试出站数量"},
        {"LJ_ZJCS_REWORK_QTY",                      "累计组件测试返修数量"},
        {"LJ_ZJCS_REWORK_1_QTY",                    "累计组件测试返工数量"},

        {"ZJ_TRACKOUT_QTY",                         "终检出站数量"},
        {"ZJ_REWORK_QTY",                           "终检返修数量"},
        {"ZJ_REWORK_1_QTY",                         "终检返工数量"},
        {"LJ_ZJ_TRACKOUT_QTY",                      "累计终检出站数量"},
        {"LJ_ZJ_REWORK_QTY",                        "累计终检返修数量"},
        {"LJ_ZJ_REWORK_1_QTY",                      "累计终检返工数量"}
    };

    Dictionary<string, string> dicRate = new Dictionary<string, string>()
    {
        {"PASS_YIELD_0_RATE",                     "直通率"},
        {"LJ_PASS_YIELD_0_RATE",                  "累计直通率"},
    };

    Dictionary<string, string> dicMethod = new Dictionary<string, string>()
    {
        {"PASS_YIELD_0_RATE",                     "从层压开始到终检的5个工段的良率(1-（返修+返工+报废）的数量/本站总产出数量）相乘获得直通率"},
        {"LJ_PASS_YIELD_0_RATE",                  "从层压开始到终检的5个工段累计的良率相乘获得直通率"},
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
        
        int queryType=3;
        if(!string.IsNullOrEmpty(sQueryType))
        {
            int.TryParse(sQueryType,out queryType);
        }
        if (roomKey == "ALL" || string.IsNullOrEmpty(roomKey)) roomKey = string.Empty;
        if (workOrderNo == "ALL" || string.IsNullOrEmpty(workOrderNo)) workOrderNo = string.Empty;
        if (productModel == "ALL" || string.IsNullOrEmpty(productModel)) productModel = string.Empty;
        if (customer == "ALL" || string.IsNullOrEmpty(customer)) customer = string.Empty;

        DataSet ds = this._passYield.Get(queryType, startTime, endTime, roomKey, customer, productModel, string.Empty, workOrderNo, partNumber);
        if (ds == null) return;
        DataTable dt = this._passYield.TransferDatatable(ds.Tables[0], queryType, dic);
        if (queryType == 3 && dt.Columns.Count>2)
        {
            string colunName = string.Format("{0}至{1}", startTime, endTime);
            dt.Columns[2].ColumnName = colunName;
        }
        this.gvResults.Columns.Clear();
        this.gvResults.AutoGenerateColumns = true;
        this.gvResults.DataSource = dt;
        this.gvResults.DataBind();
        this.gvResults.AutoGenerateColumns = false;
    }


    /// <summary>
    ///  绑定优品率明细数据。
    /// </summary>
    private void BindDetailData(ASPxGridView detailGrid, string productModel, string dataType)
    {
        DataSet ds=Cache[Session.SessionID + dataType] as DataSet;
        if (ds == null)
        {
            string startTime = Server.UrlDecode(Request.QueryString["startTime"]);
            string endTime = Server.UrlDecode(Request.QueryString["endTime"]);
            string trxType = Server.UrlDecode(Request.QueryString["trxType"]);
            string roomKey = Server.UrlDecode(Request.QueryString["roomKey"]);
            string customer = Server.UrlDecode(Request.QueryString["customer"]);
            //string productModel = Server.UrlDecode(Request.QueryString["productModel"]);
            string workOrderNo = Server.UrlDecode(Request.QueryString["workOrderNo"]);
            string sQueryType = Server.UrlDecode(Request.QueryString["queryType"]);
            string partNumber = Server.UrlDecode(Request.QueryString["partNumber"]);

            int queryType = 3;
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
            if (!dataType.StartsWith("LJ_") && queryType!=3)
            {
                startTime = DateTime.Parse(endTime).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (roomKey == "ALL" || string.IsNullOrEmpty(roomKey)) roomKey = string.Empty;
            if (workOrderNo == "ALL" || string.IsNullOrEmpty(workOrderNo)) workOrderNo = string.Empty;
            if (productModel == "ALL" || string.IsNullOrEmpty(productModel)) productModel = string.Empty;
            if (customer == "ALL" || string.IsNullOrEmpty(customer)) customer = string.Empty;

            ds = this._passYield.GetDetail(dataType, startTime, endTime, roomKey, customer, productModel, string.Empty, workOrderNo, partNumber);
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
            string productModel = keyValue.Split('$')[0];
            string dataType = keyValue.Split('$')[1];

            DataSet ds = Cache[Session.SessionID + dataType] as DataSet;
            if (ds == null)
            {
                BindDetailData(detailGrid, productModel, dataType);
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
            gv.Columns[0].Caption = "产品型号";
            gv.Columns[1].Caption = " ";
            gv.Columns[1].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            gv.Columns["KEY_VALUE"].Visible = false;
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
        if (e.Column.Index == 1 && dic.ContainsKey(val))
        {
            e.DisplayText = dic[Convert.ToString(e.Value)];
        }
        else if (e.Column.Index == 0 && Convert.ToString(e.Value) == "ALL")
        {
            e.DisplayText = "总计";
        }
    }

    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        ASPxGridView gv = (sender as ASPxGridView);
        string keyValue = Convert.ToString(gv.GetMasterRowKeyValue());
        string productModel = keyValue.Split('$')[0];
        string dataType = keyValue.Split('$')[1];
        BindDetailData(gv,productModel, dataType);
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
