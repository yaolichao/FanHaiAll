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
using DevExpress.Web;
using Astronergy.MES.Report.DataAccess;

/// <summary>
/// 设备OEE报表
/// </summary>
/// Owner by Genchille.yang 2012-04-11 10:06:28
public partial class EquReport_EquStates : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ComboxBind();
            dateStart.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            dateEnd.Text = DateTime.Now.AddDays(-1).ToShortDateString();
        }
    }

    /// <summary>
    /// 车间数据绑定
    /// </summary>
    public void ComboxBind()
    {
        DataTable dtWorkPlace = CommonFunction.GetFactoryWorkPlace();
        this.cmbWorkPlace.DataSource = dtWorkPlace;
        this.cmbWorkPlace.DataBind();
        this.cmbWorkPlace.SelectedIndex = 0;
        BindEq();
    }

    /// <summary>
    /// 设备号绑定
    /// </summary>
    private void BindEq()
    {
        if (cmbWorkPlace.SelectedIndex > -1)
        {
            string location_Workplace_Key = cmbWorkPlace.SelectedItem.Value.ToString();
            DataTable dtEquipment = new EquipmentAndOEE().GetFactoryWorkPlaceAreas(location_Workplace_Key);
            cmbEquipmentNo.DataSource = dtEquipment;
            cmbEquipmentNo.DataBind();
            cmbEquipmentNo.Items.Insert(0, new ListEditItem("全部", ""));
            this.cmbEquipmentNo.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// 查询结果，显示在表格中
    /// </summary>
    private void GvBind()
    {
        string sdate = string.Empty, edate = string.Empty;
        if (string.IsNullOrEmpty(cmbWorkPlace.Text.Trim()))
        {
            base.ShowMessageBox(this.Page, "车间不能为空");
            this.cmbWorkPlace.Focus();
            return;
        }
        if (string.IsNullOrEmpty(cmbEquipmentNo.Text.Trim()))
        {
            base.ShowMessageBox(this.Page, "设备不能为空!");
            this.cmbEquipmentNo.Focus();
            return;
        }
        if (string.IsNullOrEmpty(dateStart.Text.Trim()))
        {
            base.ShowMessageBox(this.Page, "开始日期不能为空!");
            this.dateStart.Focus();
            return;
        }
        if (string.IsNullOrEmpty(dateEnd.Text.Trim()))
        {
            base.ShowMessageBox(this.Page, "结束日期不能为空!");
            this.dateEnd.Focus();
            return;
        }
        sdate = Convert.ToDateTime(this.dateStart.Text.Trim()).ToString("yyyy-MM-dd") + " 08:00:00";
        if (this.dateEnd.Text.Trim().Equals(string.Empty))
            edate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        else
            edate = Convert.ToDateTime(this.dateEnd.Text.Trim()).AddDays(1).ToString("yyyy-MM-dd");
        edate = edate + " 08:00:00";

        try
        {
            DataTable dtMain = new EquipmentAndOEE().GetOEE_MainDetail(sdate, edate, cmbEquipmentNo.SelectedItem.Value.ToString(), cmbWorkPlace.SelectedItem.Value.ToString());
            ViewState["dt"] = dtMain;
            grid.DataSource = dtMain;
            grid.DataBind();
        }
        catch (Exception ex)
        {
            base.ShowMessageBox(this.Page, ex.Message);
        }
    }
  
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        GvBind();
    }

    /// <summary>
    /// 车间栏位值修改，联动到设备号，为设备号DataSource赋值
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbWorkPlace_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEq();
    }

    /// <summary>
    /// 表格数据导出到Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dtExport = ViewState["dt"] as DataTable;
        if (dtExport.Columns.Contains("EQUIPMENT_KEY"))
            dtExport.Columns.Remove("EQUIPMENT_KEY");
        if (dtExport.Columns.Contains("AVTIME_TARGET"))
            dtExport.Columns.Remove("AVTIME_TARGET");
        if (dtExport.Columns.Contains("EQUIPMENT_TRACT_TIME"))
            dtExport.Columns.Remove("EQUIPMENT_TRACT_TIME");

        Session["grid"] = dtExport;
        LoadResource(dtExport);
        Response.Redirect("../Master/TableToExcelTemple.aspx", true);
    }

    /// <summary>
    /// 修改列名
    /// </summary>
    /// <param name="dtBind">DataTable</param>
    private void LoadResource(DataTable dtBind)
    {
        if (dtBind.Columns.Count > 0)
        {
            foreach (DataColumn dc in dtBind.Columns)
            {
                if (dc.ColumnName == Resources.Lang.ROWNUM)
                    dc.Caption = Resources.Lang.ROWNUM_CAPTION;
                if (dc.ColumnName == Resources.Lang.EQUIPMENT_NAME)
                    dc.Caption = Resources.Lang.EQUIPMENT_NAME_CAPTION;
                if (dc.ColumnName == Resources.Lang.DESCRIPTION)
                    dc.Caption = Resources.Lang.DESCRIPTION_CAPTION;
                if (dc.ColumnName == Resources.Lang.WPH_TARGET)
                    dc.Caption = Resources.Lang.WPH_TARGET_CAPTION;
                if (dc.ColumnName == Resources.Lang.AVTIME_TARGET2)
                    dc.Caption = Resources.Lang.AVTIME_TARGET2_CAPTION;
                if (dc.ColumnName == Resources.Lang.AVTIME_TARGET)
                    dc.Caption = Resources.Lang.AVTIME_TARGET_CAPTION;
                if (dc.ColumnName == Resources.Lang.EQUIPMENT_AVTIME_TARGET)
                    dc.Caption = Resources.Lang.EQUIPMENT_AVTIME_TARGET_CAPTION;
                if (dc.ColumnName == Resources.Lang.WRKALLCELLS)
                    dc.Caption = Resources.Lang.WRKALLCELLS_CAPTION;
                if (dc.ColumnName == Resources.Lang.WPH_REL)
                    dc.Caption = Resources.Lang.WPH_REL_CAPTION;
                if (dc.ColumnName == Resources.Lang.WRK_OEE)
                    dc.Caption = Resources.Lang.WRK_OEE_CAPTION;
                if (dc.ColumnName == Resources.Lang.P_RUN)
                    dc.Caption = Resources.Lang.PER_RUN;
                if (dc.ColumnName == Resources.Lang.P_LOST)
                    dc.Caption = Resources.Lang.PER_LOST;
                if (dc.ColumnName == Resources.Lang.P_TEST)
                    dc.Caption = Resources.Lang.PER_TEST;
                if (dc.ColumnName == Resources.Lang.P_OTHER)
                    dc.Caption = Resources.Lang.PER_OTHER;
                if (dc.ColumnName == Resources.Lang.P_DOWN)
                    dc.Caption = Resources.Lang.PER_DOWN;
                if (dc.ColumnName == Resources.Lang.P_PM)
                    dc.Caption = Resources.Lang.PER_PM;
                if (dc.ColumnName == Resources.Lang.P_MON)
                    dc.Caption = Resources.Lang.PER_MON;
                if (dc.ColumnName == Resources.Lang.P_T_DOWN)
                    dc.Caption = Resources.Lang.PER_T_DOWN;
                if (dc.ColumnName == Resources.Lang.MTBF)
                    dc.Caption = Resources.Lang.MTBFHR;
                if (dc.ColumnName == Resources.Lang.MTTR)
                    dc.Caption = Resources.Lang.MTTRMIN;
            }
        }
    }

    protected void grid_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        grid.DataSource = ViewState["dt"];
        grid.DataBind();
    }
}

