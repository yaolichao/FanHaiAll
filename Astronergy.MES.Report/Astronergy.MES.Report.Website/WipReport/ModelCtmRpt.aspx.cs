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
using Astronergy.MES.Report.DataAccess;
using DevExpress.Web;
using System.IO;


public partial class WipReport_ModelCtmRpt : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindWO();
            BindFactory();
            BindProID();
            dStartDate.Date = DateTime.Now;
            dEndDate.Date = DateTime.Now;
        }
        if (this.IsCallback)
        {
            string callbackId = Convert.ToString(Request["__CALLBACKID"]);
            if (callbackId == this.gvCTMRport.UniqueID)
            {
                DataTable dt = (DataTable)Cache[Session.SessionID + "CTM"];
                if (dt != null)
                {
                    //this.gvCTMRport.Columns.Clear();
                    //this.gvCTMRport.AutoGenerateColumns = true;
                    this.gvCTMRport.DataSource = dt;
                    this.gvCTMRport.DataBind();
                    //this.gvCTMRport.AutoGenerateColumns = false;
                }
                else
                {
                    btnQuery_Click(sender, e);
                }
            }
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string sFactoryKey, sProID, sWO, sStartSN, sEndSN, sStartDate, sEndDate,sDeviceNo,sDefault;
        sFactoryKey = Convert.ToString(cobFactory.Value);
        sProID = Convert.ToString(cobProID.Value);
        //sWO = Convert.ToString(cobWO.Value);
        sWO = ddeWO.Text.Trim();
        sStartSN = txtStartSN.Text.Trim();
        sEndSN = txtEndSN.Text.Trim();
        sStartDate = dStartDate.Text;
        sEndDate = dEndDate.Text;
        sDeviceNo = txtDeviceNo.Text.Trim();
        if (sFactoryKey == "ALL")
        {
            sFactoryKey = "";
        }
        if (sProID == "ALL")
        {
            sProID = "";
        }
        if (!string.IsNullOrEmpty(sStartDate))
        {
            sStartDate = Convert.ToDateTime(sStartDate).ToString("yyy-MM-dd") + " 00:00:00";
        }
        if (!string.IsNullOrEmpty(sEndDate))
        {
            sEndDate = Convert.ToDateTime(sEndDate).ToString("yyyy-MM-dd") + " 23:59:59";
        }
        if (chDefault.Checked == true)
        {
            sDefault = "1";
        }
        else
        {
            sDefault = "0";
        }
        string partNumber = this.txtPartNumber.Text;
        if (partNumber.ToUpper()=="ALL")
        {
            partNumber = string.Empty;
        }
        if (sFactoryKey == "" && sProID == "" && sWO == "" && sStartSN == "" && sEndSN == "" && sStartDate == "" && sEndDate == "" && sDeviceNo == "")
        {
            base.ShowMessageBox(this.Page,"查询参数不能都为空！");
            return;
        }
        try
        {
            CTMFunction ctmFunction = new CTMFunction();
            DataSet dsCTM = ctmFunction.GetCTMListData(sFactoryKey, sProID, sWO, partNumber, sStartSN, sEndSN, sStartDate, sEndDate, sDeviceNo, sDefault);
            Cache[Session.SessionID + "CTM"] = dsCTM.Tables[0];

            this.gvCTMRport.Columns.Clear();
            this.gvCTMRport.AutoGenerateColumns = true;
            this.gvCTMRport.DataSource = dsCTM.Tables[0];
            this.gvCTMRport.DataBind();
            this.gvCTMRport.AutoGenerateColumns = false;

            this.UpdatePanelResult.Update();
        }
        catch(Exception ex)
        {
            base.ShowMessageBox(this.Page, "错误：" + ex.Message);
            return;
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable dtExcel = (DataTable)Cache[Session.SessionID + "CTM"];
        if (dtExcel == null)
        {
            btnQuery_Click(sender, e);
            dtExcel = (DataTable)Cache[Session.SessionID + "CTM"];
        }
        //foreach (DataColumn dc in dtExcel.Columns)
        //{
        //    foreach (GridViewColumn gc in this.gvCTMRport.Columns)
        //    {
        //        if (gc is GridViewDataTextColumn
        //            && ((GridViewDataTextColumn)gc).FieldName==dc.ColumnName)
        //        {
        //            dc.Caption = gc.Caption;
        //            break;
        //        }
        //    }
        //}
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("content-disposition", "attachment;filename=\"CTM.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToExcel(Response.OutputStream, dtExcel);
        Response.End();

        //System.Web.HttpContext.Current.Response.Clear();
        //System.Web.HttpContext.Current.Response.Buffer = true;
        //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=CTM.ZIP");
        //HttpContext.Current.Response.Charset = "UTF-8";
        //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
        //HttpContext.Current.Response.ContentType = "application/zip";
        ////随机生成一个文件夹
        ////在该文件夹夹下生成压缩包。
        //string dirctoryPath = Server.MapPath(Guid.NewGuid().ToString());
        //if (!Directory.Exists(dirctoryPath))
        //{
        //    Directory.CreateDirectory(dirctoryPath);
        //}
        //string fileName = string.Format("{0}.zip", Guid.NewGuid().ToString());
        //string excelFileName = string.Format("{0}.xlsx", "CTM");
        //string filePath = Path.Combine(dirctoryPath, fileName);
        //string excelFilePath = Path.Combine(dirctoryPath, excelFileName);
        //try
        //{
        //    Export.ExportToCompressExcel(dtExcel, excelFilePath, filePath);
        //    using (FileStream f = File.OpenRead(filePath))
        //    {
        //        byte[] bytes = new byte[f.Length];
        //        f.Read(bytes, 0, (int)f.Length);
        //        HttpContext.Current.Response.BinaryWrite(bytes);
        //    }
        //    Response.End();
        //}
        //finally
        //{
        //    if (Directory.Exists(dirctoryPath))
        //    {
        //        Directory.Delete(dirctoryPath, true);
        //    }
        //}
    }

    public void BindFactory()
    {
        DataSet dsFactory = CTMFunction.GetFactoryWorkPlace();
        cobFactory.DataSource = dsFactory.Tables[0];
        cobFactory.TextField = "LOCATION_NAME";
        cobFactory.ValueField = "LOCATION_KEY";
        cobFactory.DataBind();
        cobFactory.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        cobFactory.SelectedIndex = 0;
    }

    public void BindProID()
    {
        DataSet dsProID = CTMFunction.GetProID();
        cobProID.DataSource = dsProID.Tables[0];
        cobProID.TextField = "PRODUCT_CODE";
        cobProID.ValueField = "PRODUCT_CODE";
        cobProID.DataBind();
        cobProID.Items.Insert(0,new ListEditItem("ALL","ALL"));
        cobProID.SelectedIndex = 0;
    }


    public void BindWO()
    {
        DataSet dsWO = CTMFunction.GetWO();
        //cobWO.DataSource = dsWO.Tables[0];
        //cobWO.TextField = "ORDER_NUMBER";
        //cobWO.ValueField = "ORDER_NUMBER";
        //cobWO.DataBind();
        //cobWO.Items.Insert(0,new ListEditItem("ALL","ALL"));
        //cobWO.SelectedIndex = 0;

        ASPxListBox lstWO = this.ddeWO.FindControl("lstWO") as ASPxListBox;
        if (lstWO != null)
        {
            lstWO.DataSource = dsWO.Tables[0];
            lstWO.TextField = "ORDER_NUMBER";
            lstWO.ValueField = "ORDER_NUMBER";
            lstWO.DataBind();
            lstWO.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
    }

    protected void exporter_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
    {
        if (e.RowType == DevExpress.Web.GridViewRowType.Header)
        {
            e.Text = e.Column.Caption;
        }
        else if (e.RowType == DevExpress.Web.GridViewRowType.Data && e.Column != null)
        {
            e.Text = Convert.ToString(e.TextValue);
        }
    }
    protected void gvCTMRport_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;

        foreach (GridViewColumn gvc in gv.Columns)
        {
            gvc.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        }

        //if (gv != null && gv.Columns.Count > 1)
        //{
        //    gv.Columns[1].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[4].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[16].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[28].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[33].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[34].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[35].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[36].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[37].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[38].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[39].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[40].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[41].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[42].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[43].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[44].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    gv.Columns[45].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //}

    }

}
