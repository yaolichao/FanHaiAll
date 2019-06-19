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
using Astronergy.MES.Report.DataAccess;

/// <summary>
/// 日常营运报表目标值输入表
/// </summary>
public partial class WipReport_WIP_PLAN_AIMS : BasePage
{
    DailyReportEntity _entity = new DailyReportEntity();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ComboxBind();
            GetGridData();
            BindWorder();
        }
    }

    public void ComboxBind()
    {
        DataTable dtWorkPlace = CommonFunction.GetFactoryWorkPlace();
        this.cmbWorkPlace.DataSource = dtWorkPlace;
        this.cmbWorkPlace.DataBind();
        this.cmbWorkPlace.SelectedIndex = 0;

        DataTable dtProId = CommonFunction.GetProId();
        this.cmbProId.DataSource = dtProId;
        this.cmbProId.DataBind();
        if (dtProId.Rows.Count > 0)
            this.cmbProId.SelectedIndex = 0;

        DataTable dtProModel = CommonFunction.GetProductModel();
        this.cmbPartType.DataSource = dtProModel;
        this.cmbPartType.TextField = "PROMODEL_NAME";
        this.cmbPartType.ValueField = "PROMODEL_NAME";
        this.cmbPartType.DataBind();
        this.cmbPartType.SelectedIndex = 0;

        startDate.Text = DateTime.Now.AddDays(-3).ToShortDateString();
        endDate.Text = DateTime.Now.AddDays(21).ToShortDateString();
    }

    protected void ibtnSubmit_Click(object sender, EventArgs e)
    {
        DataTable dtPlan = ViewState["dtPlan"] as DataTable;
        DataTable dtPlanNew = dtPlan.Clone();

        if (!GetSaveTable(dtPlanNew)) return;

        bool blReturn = _entity.UpdatePlanAimsData(dtPlanNew);
        if (!string.IsNullOrEmpty(_entity.ErrorMsg))
        {
            ShowMessageBox(this.Page, _entity.ErrorMsg);
            return;
        }

        if (blReturn)
        {
            base.ShowMessageBox(this.Page, "保存成功");
        }
        else
        {
            base.ShowMessageBox(this.Page, "保存失败");
        }
    }

    private bool GetSaveTable(DataTable dtSave)
    {
        bool bl_bak = true;
        foreach (GridViewRow row in this.grid.Rows)
        {
           string rkey=  grid.DataKeys[row.RowIndex].Value.ToString();
            DataRow dr = dtSave.NewRow();
            TextBox tbxInput = ((TextBox)row.FindControl("txtQtyInput"));
            TextBox tbxOutput = ((TextBox)row.FindControl("txtQtyOutput"));
            TextBox tbxPlanDate = ((TextBox)row.FindControl("txtPlanDate"));
            TextBox tbxLocationName = ((TextBox)row.FindControl("txtLocationName"));
            TextBox tbxPartType = ((TextBox)row.FindControl("txtPartType"));
            TextBox tbxWoInput = ((TextBox)row.FindControl("txtWoInput"));
            TextBox tbxProIdInput = ((TextBox)row.FindControl("txtProIdInput"));
            DropDownList ddlShiftValue = ((DropDownList)row.FindControl("ddlShift_Value"));

            #region
            if (string.IsNullOrEmpty(tbxPlanDate.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，计划日期不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(ddlShiftValue.SelectedValue))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，班别不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(tbxLocationName.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，车间名称不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(tbxPartType.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，产品类型不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(tbxWoInput.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，工单号不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(tbxProIdInput.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，产品ID号不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(tbxInput.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，组件投入量不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            if (string.IsNullOrEmpty(tbxOutput.Text.Trim()))
            {
                ShowMessageBox(this.Page, string.Format("第【{0}】行，日出组件量不能为空!", row.RowIndex));
                bl_bak = false;
                break;
            }
            #endregion

            dr[PlanAimField.PLANID] = rkey;
            dr[PlanAimField.LOCATION_NAME] = tbxLocationName.Text.Trim();
            dr[PlanAimField.PART_TYPE] = tbxPartType.Text.Trim();
            dr[PlanAimField.PLAN_DATE] = tbxPlanDate.Text.Trim() + " 08:00:00";
            dr[PlanAimField.WORK_ORDER_NO] = tbxWoInput.Text.Trim();
            dr[PlanAimField.PRO_ID] = tbxProIdInput.Text;
            dr[PlanAimField.SHIFT_VALUE] = ddlShiftValue.SelectedValue;
            
            dr[PlanAimField.QUANTITY_INPUT] = tbxInput.Text.Trim();
            dr[PlanAimField.QUANTITY_OUTPUT] = tbxOutput.Text.Trim();

            dtSave.Rows.Add(dr);
        }

        return bl_bak;
    }

    private void GetGridData()
    {
        string startDate = string.Empty, endDate = string.Empty;

        if (this.cmbWorkPlace.SelectedIndex < 0)
        {
            ShowMessageBox(this.Page, "请选择车间!");
            return;
        }
        if (this.cmbPartType.SelectedIndex < 0)
        {
            ShowMessageBox(this.Page, "请选择产品类型!");
            return;
        }
        if (this.startDate.Text.Trim().Equals(string.Empty))
        {
            ShowMessageBox(this.Page, "开始日期不能为空!");
            return;
        }
        _entity.LocationNames = this.cmbWorkPlace.SelectedItem.Text.Trim();
        _entity.Pro_Type = this.cmbPartType.SelectedItem.Value.ToString();
        _entity.Daily_Start_Time = Convert.ToDateTime(this.startDate.Text.Trim()).ToString("yyyy-MM-dd") ;

        if (this.endDate.Text.Trim().Equals(string.Empty))
            endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        else
            endDate = Convert.ToDateTime(this.endDate.Text.Trim()).ToString("yyyy-MM-dd");
        _entity.Daily_End_Time = endDate ;

        //DataTable dtPlan =_entity.GetPlanAimsData(locationKey, startDate, endDate);
        DataTable dtPlan = _entity.GetPlanAimsData();
        ViewState["dtPlan"] = dtPlan;
        this.grid.DataSource = dtPlan;
        this.grid.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        GetGridData();
    }

    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        if (ViewState["dtPlan"] == null)
        {
            _entity.PlanId = Convert.ToString(Guid.NewGuid());
            DataTable dtPlan = _entity.GetPlanAimsData();

            ViewState["dtPlan"] = dtPlan;
        }

        DataTable dtGvid = ViewState["dtPlan"] as DataTable;

        DataRow drNew = dtGvid.NewRow();
        drNew[PlanAimField.PLAN_DATE] = this.startDate.Text.Trim() + " 08:00:00";
        drNew[PlanAimField.PLANID] = Convert.ToString(Guid.NewGuid());
        drNew[PlanAimField.LOCATION_NAME] = this.cmbWorkPlace.SelectedItem.Text.Trim();
        drNew[PlanAimField.PART_TYPE] = this.cmbPartType.SelectedItem.Text.Trim();
        drNew[PlanAimField.WORK_ORDER_NO] = this.cmbWo.SelectedItem.Text.Trim();
        drNew[PlanAimField.PRO_ID] = this.cmbProId.SelectedItem.Text.Trim();
        dtGvid.Rows.Add(drNew);
        this.grid.DataSource = dtGvid;
        this.grid.DataBind();

        ViewState["dtPlan"] = dtGvid;
    }

    protected void cmbWorkPlace_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWorder();
    } 
    /// <summary>
    /// 绑定工单信息数据
    /// </summary>
    public void BindWorder()
    {
        if (cmbWorkPlace.SelectedIndex < 0)
            return;

        DataTable dtWo = CommonFunction.GetLotWorkOrderNumber(cmbWorkPlace.SelectedItem.Value.ToString());
        this.cmbWo.DataSource = dtWo;
        this.cmbWo.DataBind();
        if (dtWo.Rows.Count > 0)
            this.cmbWo.SelectedIndex = 0;
    }


    protected void grid_DataBound(object sender, EventArgs e)
    {

    }
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataTable dtShift = CommonFunction.GetShift();
            DropDownList ddlist = (DropDownList)e.Row.FindControl("ddlShift_Value");
            ddlist.DataSource = dtShift;
            ddlist.DataBind();

            ddlist.SelectedValue = ((HiddenField)e.Row.FindControl("hiddenShift_Value")).Value;
        }
        catch (Exception ex) { }
    }
}
