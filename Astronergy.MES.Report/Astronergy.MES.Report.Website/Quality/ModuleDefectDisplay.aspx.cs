using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Astronergy.MES.Report.DataAccess;
using System.Data;
using DevExpress.Web;
using System.Collections;

public partial class Quality_ModuleDefectDisplay : BasePage
{
    DefectDisplayAccess _entity = new DefectDisplayAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ComboxBind();

            this.endTime.Text = CommonFunction.GetCurrentDateTime().ToString("HH:mm");
            this.endDate.Text = CommonFunction.GetCurrentDateTime().ToString("yyyy/MM/dd");
            this.startDate.Text = CommonFunction.GetCurrentDateTime().ToString("yyyy/MM/dd");
            this.startTime.Text = "08:00";           
        }
        BindPartType();
        BindProId();
        BindWorder();
        rbtnLst_SelectedIndexChanged(null, null);
    }
    /// <summary>
    /// 工厂车间和产品类型数据绑定
    /// </summary>
    public void ComboxBind()
    {
        DataTable dtWorkPlace = CommonFunction.GetFactoryWorkPlace();
        this.cmbWorkPlace.DataSource = dtWorkPlace;
        this.cmbWorkPlace.DataBind();
        ListEditItem item = new ListEditItem();
        item.Value = "";
        item.Text = "All";
        this.cmbWorkPlace.Items.Insert(0, item);
    }
    public void BindPartType()
    {
        DataTable dtProModel = CommonFunction.GetProductModel();

        DataTable dtType = CommonFunction.GetProductModel();
        ASPxListBox typeListBox = (ASPxListBox)checkComboBoxForType.FindControl("lbx_type");
        foreach (DataRow drItem in dtType.Rows)
        {
            ListEditItem listEditItem = new ListEditItem();
            listEditItem.Text = Convert.ToString(drItem["PROMODEL_NAME"]);
            listEditItem.Value = Convert.ToString(drItem["PROMODEL_NAME"]);
            typeListBox.Items.Add(listEditItem);
        }
        ViewState["dtType"] = dtType;
        ListEditItem listItem = new ListEditItem();
        listItem.Text = "(Select All)";
        listItem.Value = string.Empty;
        typeListBox.Items.Insert(0, listItem);

    }
    /// <summary>
    /// 绑定工单信息数据
    /// </summary>
    public void BindWorder()
    {
        //DataTable dtWo = CommonFunction.GetLotWorkOrderNumber(cmbWorkPlace.SelectedItem.Value.ToString());
        //this.cmbWo.DataSource = dtWo;
        //this.cmbWo.DataBind();
        DataTable dtWo = CommonFunction.GetWorkOrder();
        ASPxListBox woListBox = (ASPxListBox)checkComboBoxForWo.FindControl("lbx_wo");
        foreach (DataRow drItem in dtWo.Rows)
        {
            ListEditItem listEditItem = new ListEditItem();
            listEditItem.Text = Convert.ToString(drItem["WORK_ORDER_NO"]);
            listEditItem.Value = Convert.ToString(drItem["WORK_ORDER_NO"]);
            woListBox.Items.Add(listEditItem);
        }
        ViewState["dtWo"] = dtWo;
        ListEditItem listItem = new ListEditItem();
        listItem.Text = "(Select All)";
        listItem.Value = string.Empty;
        woListBox.Items.Insert(0, listItem);
    }
    /// <summary>
    /// 绑定产品ID号信息
    /// </summary>
    public void BindProId()
    {
        if (cmbWorkPlace.SelectedIndex < 0)
            return;

        //DataTable dtProId = CommonFunction.GetLotWorkProId(cmbWorkPlace.SelectedItem.Value.ToString()); 
        DataTable dtProId = CommonFunction.GetProId();

        ASPxListBox proidListBox = (ASPxListBox)checkComboBox.FindControl("lbx_proid");
        foreach (DataRow drItem in dtProId.Rows)
        {
            ListEditItem listEditItem = new ListEditItem();
            listEditItem.Text = Convert.ToString(drItem["PRODUCT_CODE"]);
            listEditItem.Value = Convert.ToString(drItem["PRODUCT_CODE"]);
            proidListBox.Items.Add(listEditItem);
        }
        ViewState["dtProId"] = dtProId;
        ListEditItem listItem = new ListEditItem();
        listItem.Text = "(Select All)";
        listItem.Value = string.Empty;
        proidListBox.Items.Insert(0, listItem);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindGvData();
    }
    /// <summary>
    /// 根据查询条件，获得数据
    /// </summary>
    private void BindGvData()
    {
        string locationKey = string.Empty, locationKeys = string.Empty, locationCode = string.Empty,
            partType = string.Empty,
            startDay = string.Empty, endDay = string.Empty, routeKey = string.Empty, partNumber = string.Empty;
        int itype = -1;

        if (this.cmbWorkPlace.Text.Trim().Equals(string.Empty))
        {
            ShowMessageBox(this.Page, "请选择车间!");
            return;
        }
        //if (this.checkComboBoxForType.Text.Trim().Equals(string.Empty))
        //{
        //    ShowMessageBox(this.Page, "请选择型号");
        //    return;
        //}
        //if (this.checkComboBox.Text.Trim().Equals(string.Empty))
        //{
        //    ShowMessageBox(this.Page, "请选择产品ID号!");
        //    return;
        //}
        if (this.startDate.Text.Trim().Equals(string.Empty))
        {
            ShowMessageBox(this.Page, "开始日期不能为空!");
            return;
        }

        if (this.endDate.Text.Trim().Equals(string.Empty))
        {
            ShowMessageBox(this.Page, "结束日期不能为空!");
            return;
        }
        if (rbtnLst.SelectedIndex == 0)
        {
            itype = 0;
            this.hidLayoutType.Value = LayoutViewType.ViewType_Day;
        }
        else if (rbtnLst.SelectedIndex == 1)
        {
            itype = 1;
            this.hidLayoutType.Value = LayoutViewType.ViewType_Hour;
        }
        else
        {
            ShowMessageBox(this.Page, "请选择按【日期查询】或【时间查询】!");
            return;
        }
       
        //获得查询条件
        Hashtable hsParams = new Hashtable();
        DataTable dtLocation = (DataTable)ViewState["dtLocation"];
        _entity.LocationKey = this.cmbWorkPlace.SelectedItem.Value.ToString();
        hidLoactionKey.Value = locationKey;

        //获得产品ID号
        string[] strcode = this.checkComboBox.Text.Trim().Split(';');
        string proids = string.Empty;
        foreach (string s01 in strcode)
        {
            if (s01.Equals(string.Empty)) continue;
            proids += "'" + s01 + "',";
        }
        proids = proids.TrimEnd(',');
        _entity.Pro_Ids = proids;
        this.hidProid.Value = proids;

        //获得工单号
        string[] strWo = this.checkComboBoxForWo.Text.Trim().Split(';');
        string wos = string.Empty;
        foreach (string s in strWo)
        {
            if (s.Equals(string.Empty)) continue;
            wos += "'" + s + "',";
        }
        wos = wos.TrimEnd(',');
        _entity.WorkOrders = wos;
        this.hidWorkorder.Value = wos;

        //获得产品料号
        string[] strPn = this.textPartNumber.Text.Trim().Split(',');
        string pan = string.Empty;
        foreach (string s02 in strPn)
        {
            if (s02.Equals(string.Empty)) continue;
            pan += "'" + s02 + "',";
        }
        pan = pan.TrimEnd(',');
        _entity.PartNumber = pan;
        this.hidPartNumber.Value = pan;

        //获得产品型号
        string[] strtype = this.checkComboBoxForType.Text.Trim().Split(';');
        string types = string.Empty;
        foreach (string s in strtype)
        {
            if (s.Equals(string.Empty)) continue;
            types += "'" + s + "',";
        }
        types = types.TrimEnd(',');
        _entity.ProTypes = types;
        hidPartType.Value = types;

        if (itype== 0)
        {
            //开始日期
            _entity.StartDate = Convert.ToDateTime(this.startDate.Text.Trim()).ToString("yyyy-MM-dd") + " 08:00:00";
            //结束日期
            _entity.EndDate = Convert.ToDateTime(this.endDate.Text.Trim()).ToString("yyyy-MM-dd") + " 08:00:00";
        }
        else if (itype == 1)
        {
            //开始时间
            _entity.StartDate = Convert.ToDateTime(this.startDate.Text.Trim()+" "+this.startTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            //结束时间
            _entity.EndDate = Convert.ToDateTime(this.endDate.Text.Trim() + " " + this.endTime.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
        }
        _entity.QueryByDayOrHour = itype;

        //set Hidden value
        this.hidStartDate.Value = Convert.ToDateTime(this.startDate.Text.Trim()).ToString("yyyy-MM-dd");
        this.hidStartTime.Value = this.startTime.Text.Trim();
        this.hidEndDate.Value = Convert.ToDateTime(this.endDate.Text.Trim()).ToString("yyyy-MM-dd");
        this.hidEndTime.Value = this.endTime.Text.Trim();

        //与服务端通讯，获得服务端数据
        DataSet ds01 = _entity.GetDefectAccessData();
        DataTable dtReport = new DataTable();
        if (!string.IsNullOrEmpty(_entity.ErrorMsg))
        {
            ShowMessageBox(this.Page, _entity.ErrorMsg);
            return;
        }
        dtReport = ds01.Tables[LayoutViewType.ReportTable];

        if (_entity.QueryByDayOrHour == 0)
        {
            foreach (DataColumn dc01 in dtReport.Columns)
            {

                if (!dc01.ColumnName.Equals("CODE") && !dc01.ColumnName.Equals("DEFECT_NAME"))
                {
                    string caption = Convert.ToDateTime(dc01.ColumnName.Trim()).ToString("yyyy/MM/dd ");
                    caption = caption.Replace('-', '/');
                    dc01.Caption = caption;
                }
            }
        }

        DataTable dtGv = base.LoadColumnsResource(dtReport);
        grid.Columns.Clear();
        grid.DataSource = null;
        grid.AutoGenerateColumns = true;
        grid.DataSource = dtGv;
        grid.DataBind();
        ViewState["grid"] = dtGv;

        for (int i = 0; i < grid.Columns.Count; i++)
        {
            if (grid.Columns[i].ToString().Equals("CODE"))
            {
                grid.Columns["CODE"].Visible = false;
                break;
            }          
        }

    }
    /// <summary>
    /// 导出EXCEL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dtExpor = (DataTable)ViewState["grid"];
        if (dtExpor.Rows.Count < 1) return;
        if (dtExpor.Columns.Contains("CODE"))
            dtExpor.Columns.Remove("CODE");

        Session["grid"] = dtExpor;
        //Response.Redirect("../Master/TableToExcelTemple.aspx", true);        
        Response.Redirect("../Master/ExcelTemplate.aspx", true);
    }
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.Index != 0)
        {
            try
            {
                string startDate = string.Empty, endDate = string.Empty;
                string headColumnName = e.Column.FieldName;
                DataRow dr = (DataRow)grid.GetDataRow(e.VisibleRowIndex);
                string rowKey = dr[grid.KeyFieldName].ToString();
                if (headColumnName.ToUpper().Trim() == "CODE" || headColumnName.ToUpper().Trim() == "DEFECT_NAME")
                    return;
                if (this.hidLayoutType.Value.Trim().Equals(LayoutViewType.ViewType_Hour))
                {
                    //startDate = Convert.ToDateTime(e.Column.FieldName).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    //endDate = Convert.ToDateTime(startDate).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");

                    startDate = Convert.ToDateTime(this.hidStartDate.Value + " " + this.hidStartTime.Value).ToString("yyyy-MM-dd HH:mm:ss");
                    endDate = Convert.ToDateTime(this.hidEndDate.Value + " " + this.hidEndTime.Value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (this.hidLayoutType.Value.Trim().Equals(LayoutViewType.ViewType_Day))
                {
                    startDate = e.Column.FieldName;
                    endDate = Convert.ToDateTime(startDate).AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00";
                }
                if (e.DisplayText == "0") return;

                e.DisplayText = string.Format("<a  target='_blank' href=\"ModuleDefectDataDetail.aspx?locationKey={1}&partType={2}&paraDate01={3}&paraDate02={4}&Pro_id={5}&workorder={6}&reason_code_class={7}&partNumber={8}&sType=\" class=\'dxgv\' style=\'text-decoration:underline;\'>{0}</a>", e.Value,
                Server.UrlEncode(hidLoactionKey.Value), Server.UrlEncode(hidPartType.Value), Server.UrlEncode(startDate), Server.UrlEncode(endDate), Server.UrlEncode(this.hidProid.Value), Server.UrlEncode(this.hidWorkorder.Value), rowKey, Server.UrlEncode(this.hidPartNumber.Value));

            }
            catch (Exception ex)
            { }
        }

    }

    protected void rbtnLst_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnLst.SelectedIndex == 0)
        {
            this.startTime.ClientEnabled = false;
            this.endTime.ClientEnabled = false;            
        }
        if (rbtnLst.SelectedIndex == 1)
        {
            this.startTime.ClientEnabled = true;
            this.endTime.ClientEnabled = true;
        }
    }
}
