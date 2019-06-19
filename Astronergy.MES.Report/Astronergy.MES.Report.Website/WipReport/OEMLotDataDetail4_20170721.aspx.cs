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
using System.Security.Permissions;
/// <summary>
/// 批次明细数据。
/// </summary>
public partial class WipReport_OEMLotDataDetail4_20170721 : System.Web.UI.Page
{
    private OEMLotData4_20170721Access _lot = new OEMLotData4_20170721Access();
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
        if (this.IsCallback || !this.IsPostBack)
        {
            BindDetailData(this.pcLotData.ActiveTabPage.Index);
        }
    }
    /// <summary>
    /// 绑定批次数据。
    /// </summary>
    private void BindDetailData(int type)
    {
        string lotKey = Convert.ToString(Request["lotkey"]);
        DataSet dsReturn = this._lot.GetLotDetail(lotKey, type);

        if (type == 0)
        {
            this.gvLotBaseInfo.DataSource = dsReturn.Tables[0];
            if (dsReturn.Tables[0].Rows.Count > 0)
            {
                this.lblLotNo.Text = Convert.ToString(dsReturn.Tables[0].Rows[0][1]);
                this.PagetTitle = this.lblLotNo.Text;
            }

        }
        else if (type == 1)
        {
            this.gvHistory.DataSource = dsReturn.Tables[0];
            this.gvHistory.DataBind();
        }
        else if (type == 2)
        {
            //获取工序参数数据集
            DataTable dt = dsReturn.Tables[0];
            DataRow[] drs = null;

            //定义 接线盒、背板、硅胶、灌封胶 对应变量
            string strJunctionBoxMaterial = string.Empty;
            string strJunctionSupplierCode = string.Empty;
            string strBackBoardMaterial = string.Empty;
            string strBackBoardSupplierCode = string.Empty;
            string strSilicaGelMaterial = string.Empty;
            string strSilicaGelSupplierCode = string.Empty;
            string strPouringSealantMaterial = string.Empty;
            string strPouringSealantSupplierCode = string.Empty;

            string strMaterialType = string.Empty;

            //获取接线盒和背板的信息
            strMaterialType = "接线盒";
            drs = dt.Select(string.Format(" 物料类型 = '{0}'", strMaterialType));
            if (drs.Length > 0)
            {
                strJunctionBoxMaterial = Convert.ToString(drs[0]["料号"]);
                strJunctionSupplierCode = Convert.ToString(drs[0]["供方编码"]);
            }
            strMaterialType = "背板";
            drs = dt.Select(string.Format(" 物料类型 = '{0}'", strMaterialType));
            if (drs.Length > 0)
            {
                strBackBoardMaterial = Convert.ToString(drs[0]["料号"]);
                strBackBoardSupplierCode = Convert.ToString(drs[0]["供方编码"]);
            }

            //获取 灌封胶 对应在 dt 中的信息 如果存在 进行灌封胶的检查
            drs = dt.Select(string.Format(" 物料类型  = '灌封胶-A' OR 物料类型 = '灌封胶-B' "));
            if (drs.Length > 0)
            {
                //获取 接线盒 和 灌封胶 的组合关系
                DataTable dtDouble = this._lot.GetDoubleRelationship().Tables[0];
                //循环判断
                foreach (DataRow dr in drs)
                {
                    strPouringSealantMaterial = Convert.ToString(dr["料号"]);
                    strPouringSealantSupplierCode = Convert.ToString(dr["供方编码"]);

                    strMaterialType = Convert.ToString(dr["物料类型"]);
                    DataRow[] drsDouble = dtDouble.Select(string.Format(" 物料类型 = '{0}' AND 接线盒料号 = '{1}' ", strMaterialType, strJunctionBoxMaterial));

                    if (drsDouble.Length > 0)
                    {
                        string strOldPouringSealantMaterial = Convert.ToString(drsDouble[0]["料号"]);
                        string strOldPouringSealantSupplierCode = Convert.ToString(drsDouble[0]["供方编码"]);
                        if (!strOldPouringSealantMaterial.Equals(strPouringSealantMaterial))
                        {
                            strPouringSealantMaterial = strOldPouringSealantMaterial;
                            strPouringSealantSupplierCode = strOldPouringSealantSupplierCode;
                        }
                    }

                    dr["参数值"] = string.Format("{0}A{1}{2}", strPouringSealantSupplierCode, strPouringSealantMaterial, Convert.ToString(dr["参数值"]).Length < 20 ? Convert.ToString(dr["参数值"]) : Convert.ToString(dr["参数值"]).Substring(19));

                }
            }

            //获取 硅胶 对应在 dt 中的信息 如果存在 进行 硅胶信息的检查
            //strMaterialType = "硅胶";
            //drs = dt.Select(string.Format(" 物料类型 = '{0}'", strMaterialType));
            //if (drs.Length > 0)
            //{
            //    //获取 接线盒、背板、硅胶的组合关系
            //    DataTable dtTriple = this._lot.GetTripleRelationship().Tables[0];

            //    //循环判断
            //    foreach (DataRow dr in drs)
            //    {
            //        strSilicaGelMaterial = Convert.ToString(dr["料号"]);
            //        strSilicaGelSupplierCode = Convert.ToString(dr["供方编码"]);

            //        strMaterialType = "硅胶";
            //        DataRow[] drsTriple = dtTriple.Select(string.Format(" 物料类型 = '{0}' AND 接线盒料号 = '{1}' AND 背板料号 = '{2}' ", strMaterialType, strJunctionBoxMaterial, strBackBoardMaterial));

            //        if (drsTriple.Length > 0)
            //        {
            //            string strOldSilicaGelMaterial = Convert.ToString(drsTriple[0]["料号"]);
            //            string strOldSilicaGelSupplierCode = Convert.ToString(drsTriple[0]["供方编码"]);
            //            if (!strOldSilicaGelMaterial.Equals(strPouringSealantMaterial))
            //            {
            //                strSilicaGelMaterial = strOldSilicaGelMaterial;
            //                strSilicaGelSupplierCode = strOldSilicaGelSupplierCode;
            //            }
            //        }

            //        dr["参数值"] = string.Format("{0}A{1}{2}", strSilicaGelSupplierCode, strSilicaGelMaterial, Convert.ToString(dr["参数值"]).Length < 20 ? Convert.ToString(dr["参数值"]) : Convert.ToString(dr["参数值"]).Substring(19));

            //    }
            //}

            dt.Columns.Remove("物料类型");
            dt.Columns.Remove("料号");
            dt.Columns.Remove("供方编码");

            this.gvParamData.DataSource = dsReturn.Tables[0];
            this.gvParamData.DataBind();
        }
        else if (type == 3)
        {
            this.gvDefectScrap.DataSource = dsReturn.Tables[0];
            this.gvDefectScrap.DataBind();
        }
        else if (type == 4)
        {
            this.gvIVTestData.DataSource = dsReturn.Tables[0];
            this.gvIVTestData.DataBind();
        }
        else if (type == 5)
        {

            if (dsReturn == null ||
                dsReturn.Tables.Count <= 0 ||
                dsReturn.Tables[0].Rows.Count <= 0)
            {
                this.lblELPicturePath.Text = "无EL图片。";
                this.imgEL.Visible = false;
                return;
            }

            DataRow dr = dsReturn.Tables[0].Rows[0];
            string lotNo = Convert.ToString(dr["LOT_NUM"]);
            string picAddress = Convert.ToString(dr["PIC_ADDRESS"]);
            DateTime dtPicTime = Convert.ToDateTime(dr["TTIME"]);
            string picDateFormat = Convert.ToString(dr["PIC_DATE_FORMAT"]);
            if (string.IsNullOrEmpty(picDateFormat))
            {
                picDateFormat = "yyyy-M-d";
            }
            //根据规则生成图片路径。
            // picAddress\2012年\1月\2012-1-12
            string picPath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}.jpg",
                                            picAddress,
                                            dtPicTime.ToString("yyyy年"),
                                            dtPicTime.ToString("M月"),
                                            dtPicTime.ToString(picDateFormat),
                                            lotNo.Trim());
            this.lblELPicturePath.Text = string.Format("EL图片路径：{0}", picPath);
            string elPicGuid = string.Format("{0}", System.Guid.NewGuid().ToString());
            Cache[elPicGuid] = picPath;
            this.imgEL.Visible = true;
            this.imgEL.ImageUrl = "../Handler/ELPictureHandler.ashx?id=" + elPicGuid;
        }
    }
    /// <summary>
    /// 加工历史-自定义显示文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvHistory_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        DataTable dtSource = this.gvHistory.DataSource as DataTable;
        if (dtSource != null && dtSource.Columns[e.Column.FieldName].DataType == typeof(DateTime))
        {
            if (e.Value != null && e.Value != DBNull.Value)
            {
                e.DisplayText = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
    /// <summary>
    /// 加工历史行创建时触发事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvHistory_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        DataTable dt = gvHistory.DataSource as DataTable;
        if (e.RowType == GridViewRowType.Data && dt != null)
        {
            //如果操作被撤销，则设置背景色为红色。
            string undo = Convert.ToString(dt.Rows[e.VisibleIndex][7]);
            if (undo == "1")
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.CssStyle.Add(HtmlTextWriterStyle.BackgroundColor, "red");
                    e.Row.Cells[i].Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "white");
                }
            }
        }
    }
    /// <summary>
    /// 基础数据-自定义显示文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLotBaseInfo_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        DataTable dtSource = this.gvLotBaseInfo.DataSource as DataTable;
        if (dtSource != null && dtSource.Columns[e.Column.FieldName].DataType == typeof(DateTime))
        {
            if (e.Value != null && e.Value != DBNull.Value)
            {
                e.DisplayText = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
    /// <summary>
    /// 工序参数-自定义显示文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvParamData_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        DataTable dtSource = this.gvParamData.DataSource as DataTable;
        if (dtSource != null && dtSource.Columns[e.Column.FieldName].DataType == typeof(DateTime))
        {
            if (e.Value != null && e.Value != DBNull.Value)
            {
                e.DisplayText = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
    /// <summary>
    /// 报废不良-自定义显示文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDefectScrap_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        DataTable dtSource = this.gvDefectScrap.DataSource as DataTable;
        if (dtSource != null && dtSource.Columns[e.Column.FieldName].DataType == typeof(DateTime))
        {
            if (e.Value != null && e.Value != DBNull.Value)
            {
                e.DisplayText = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
    /// <summary>
    /// IV测试数据-自定义显示文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvIVTestData_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        DataTable dtSource = this.gvIVTestData.DataSource as DataTable;
        if (dtSource != null && dtSource.Columns[e.Column.FieldName].DataType == typeof(DateTime))
        {
            if (e.Value != null && e.Value != DBNull.Value)
            {
                e.DisplayText = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
