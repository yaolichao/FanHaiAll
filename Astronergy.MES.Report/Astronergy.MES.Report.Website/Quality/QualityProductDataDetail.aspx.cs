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
/// 优品率报表明细数据。
/// </summary>
public partial class Quality_QualityProductDataDetail : System.Web.UI.Page
{
    private QualityProductDataAccess qualityProduct = new QualityProductDataAccess();//调用逻辑

    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"TRACKOUT_QTY",                            "终检出站数量"},
        {"LJ_TRACKOUT_QTY",                         "累计终检出站数量"},

        {"KJ_QTY",                                  "客级工单客级数量"},
        {"LJ_KJ_QTY",                               "累计客级工单客级数量"},

        {"KJ_AJ_QTY",                               "客级工单A级数量"},
        {"LJ_KJ_AJ_QTY",                            "累计客级工单A级数量"},

        {"KJ_A0J_QTY",                              "客级工单A0级数量"},
        {"LJ_KJ_A0J_QTY",                           "累计客级工单A0级数量"},

        {"KJ_ERSANJI_QTY",                          "客级工单二三级数量"},
        {"LJ_KJ_ERSANJI_QTY",                       "累计客级工单二三级数量"},

        {"KJ_SCRAP_QTY",                            "客级工单报废数量"},
        {"LJ_KJ_SCRAP_QTY",                         "累计客级工单报废数量"},

        {"AJ_QTY",                                  "A级数量"},
        {"LJ_AJ_QTY",                               "累计A级数量"},

        {"A0J_QTY",                                 "A0数量"},
        {"LJ_A0J_QTY",                              "累计A0数量"},
        {"DIRECT_A0J_QTY",                          "直判A0数量"},
        {"LJ_DIRECT_A0J_QTY",                       "累计直判A0数量"},
        {"REWORK_A0J_QTY",                          "返修后A0级数量"},
        {"LJ_REWORK_A0J_QTY",                       "累计返修后A0级数量"},
        {"PRE_A0J_QTY",                             "预判A0数量"},
        {"LJ_PRE_A0J_QTY",                          "累计预判A0数量"},

        {"ERSANJ_QTY",                              "二三级数量"},
        {"LJ_ERSANJ_QTY",                           "累计二三级数量"},
        {"REWORK_ERSANJ_QTY",                       "二三级<返修>数量"},
        {"LJ_REWORK_ERSANJ_QTY",                    "累计二三级<返修>数量"},
        {"NO_CY_REWORK_ERSANJ_QTY",                 "二三级<非层压返修>数量"},
        {"LJ_NO_CY_REWORK_ERSANJ_QTY",              "累计二三级<非层压返修>数量"},
        
        {"SCRAP_QTY",                               "实际报废数量"},
        {"LJ_SCRAP_QTY",                            "累计实际报废数量"},
        {"NO_CY_REWORK_SCRAP_QTY",                  "报废<非层压返修>数量"},
        {"LJ_NO_CY_REWORK_SCRAP_QTY",               "累计报废<非层压返修>数量"},

    };

    Dictionary<string, string> dicRate = new Dictionary<string, string>()
    {
        {"KJ_RATE",                                 "客级品率"},
        {"LJ_KJ_RATE",                              "累计客级品率"},
        {"A_UPON_QUALITY_RATE",                     "A级以上优品率"},
        {"LJ_A_UPON_QUALITY_RATE",                  "累计 A级以上优品率"},
        {"A0_UPON_QUALITY_RATE",                    "A0级以上优品率"},
        {"LJ_A0_UPON_QUALITY_RATE",                 "累计 A0级以上优品率"},
        {"A0_ERSANJ_QUALITY_RATE",                  "A0、二三级品率"},
        {"LJ_A0_ERSANJ_QUALITY_RATE",               "累计A0、二三级品率"},
        {"REWORK_A0_ERSANJ_QUALITY_RATE",           "返修后A0、二三级品率"},
        {"LJ_REWORK_A0_ERSANJ_QUALITY_RATE",        "累计返修后A0、二三级品率"},
        {"PASS_RATE",                               "合格率"},
        {"LJ_PASS_RATE",                            "累计合格率"}
    };

    Dictionary<string, string> dicMethod = new Dictionary<string, string>()
    {
        {"KJ_RATE",                                 "客级工单客级品数/客级工单的（客级+A+A0+二三级+报废数）"},
        {"LJ_KJ_RATE",                              "累计客级工单的客级品数/累计客级工单的（客级+A+A0+二三级+报废数）"},
        {"A_UPON_QUALITY_RATE",                     "1-[(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%"},
        {"LJ_A_UPON_QUALITY_RATE",                  "1-[累计(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/累计（入库数+报废数）]*100%"},
        {"A0_UPON_QUALITY_RATE",                    "1-[(二三级+报废）/（入库数+报废数）]*100%"},
        {"LJ_A0_UPON_QUALITY_RATE",                 "1-[累计(二三级+报废）/累计（入库数+报废数）]*100%"},
        {"A0_ERSANJ_QUALITY_RATE",                  "[（A0级+二三级）/（入库数+报废数）]*100%"},
        {"LJ_A0_ERSANJ_QUALITY_RATE",               "[累计（A0级+二三级）/累计（入库数+报废数）]*100%"},
        {"REWORK_A0_ERSANJ_QUALITY_RATE",           "[（返修后A0级+返修后二三级）/（入库数+报废数）]*100%"},
        {"LJ_REWORK_A0_ERSANJ_QUALITY_RATE",        "[累计（返修后A0级+返修后二三级）/累计（入库数+报废数）]*100%"},
        {"PASS_RATE",                               "1-[报废数/(入库数+报废数)]*100%"},
        {"LJ_PASS_RATE",                            "1-[累计报废数/(入库数+报废数)]*100%"}
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

        DataSet ds = qualityProduct.Get(queryType, startTime, endTime, roomKey, customer, productModel, string.Empty, workOrderNo, partNumber);
        if (ds == null) return;
        DataTable dt = qualityProduct.TransferDatatable(ds.Tables[0], queryType, dic);
        if (queryType == 3 && dt.Columns.Count > 2)
        {
            string colunName = string.Format("{0}至{1}", startTime, endTime);
            dt.Columns[2].ColumnName = colunName;
        }
        this.gvResults.Columns.Clear();
        this.gvResults.AutoGenerateColumns = true;
        this.gvResults.DataSource = dt;
        this.gvResults.DataBind();
        this.gvResults.AutoGenerateColumns = false;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string colVal = Convert.ToString(dt.Rows[i]["COL_VALUE"]);
            if (colVal.StartsWith("LJ_")
                || colVal=="TRACKOUT_QTY"
                || colVal.StartsWith("KJ_"))
            {
                continue;
            }
            this.gvResults.DetailRows.ExpandRow(i);
        }
    }


    /// <summary>
    ///  绑定优品率明细数据。
    /// </summary>
    private void BindDetailData(ASPxGridView detailGrid ,string productModel,string dataType)
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

            ds = qualityProduct.GetDetail(dataType, startTime, endTime,roomKey,customer, productModel, string.Empty, workOrderNo,partNumber);
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
        ASPxButton btn = (ASPxButton)sender;
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
        BindDetailData(gv, productModel,dataType);
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
