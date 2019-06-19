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


using Astronergy.MES.Report.DataAccess;

public partial class ConergyFlashDataExport : BasePage
{
    private ConergyFlashDataExportDataAccess conergyFlashDataExport = new ConergyFlashDataExportDataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            
            deStartShippingData.Date = dtNow.AddMonths(-1);
            deEndShippingData.Date = dtNow;
        }

        if (this.IsCallback)
        {
            string callbackId = Convert.ToString(Request["__CALLBACKID"]);
            if (callbackId == this.gvConergyFlash.UniqueID)
            {
                DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_ConergyFlashData"];
                if (dtLotData != null)
                {
                    this.gvConergyFlash.Columns.Clear();
                    this.gvConergyFlash.AutoGenerateColumns = true;
                    this.gvConergyFlash.DataSource = dtLotData;
                    this.gvConergyFlash.DataBind();
                    this.gvConergyFlash.AutoGenerateColumns = false;
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
        string invoiceNumber = txtInvoiceNumber.Text;
        string deliveryNumber = txtDeliveryNumber.Text;
        string startShippingData = deStartShippingData.Text == "" ? DateTime.Now.AddMonths(-1).ToString() : deStartShippingData.Text;
        string endShippingData = deEndShippingData.Text == "" ? DateTime.Now.AddDays(1).ToString() : deEndShippingData.Text;



        DataSet ds = conergyFlashDataExport.GetConergyFlashData(invoiceNumber, deliveryNumber, startShippingData, endShippingData);
        if (ds != null)
        {
            DataTable dt = ds.Tables[0];
            Cache[Session.SessionID + "_ConergyFlashData"] = dt;
            this.gvConergyFlash.Columns.Clear();
            this.gvConergyFlash.AutoGenerateColumns = true;
            this.gvConergyFlash.DataSource = dt;
            this.gvConergyFlash.DataBind();
            this.gvConergyFlash.AutoGenerateColumns = false;
        }
        this.UpdatePanelResult.Update();
    }
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtConergyFlashData = (DataTable)Cache[Session.SessionID + "_ConergyFlashData"];
            if (dtConergyFlashData == null)
            {
                btnQuery_Click(sender, e);
                dtConergyFlashData = (DataTable)Cache[Session.SessionID + "_ConergyFlashData"];
            }

            string strName = string.Empty;
            int rowCount = dtConergyFlashData.Rows.Count;
            string strNominalMaxpower = string.Empty;
            string strDeliveryNumber = string.Empty;
            string strInvoiceNumber = string.Empty;
            if (rowCount > 0)
            {
                DataTable dtPower = dtConergyFlashData.DefaultView.ToTable(true, "NominalMaxpower");

                foreach (DataRow drPower in dtPower.Rows)
                {
                    if (string.IsNullOrEmpty(strNominalMaxpower))
                    {
                        strNominalMaxpower = Convert.ToString(drPower["NominalMaxpower"]) + "W";
                    }
                    else
                    {
                        strNominalMaxpower += "&" + Convert.ToString(drPower["NominalMaxpower"]) + "W";
                    }
                }

                DataRow dr = dtConergyFlashData.Rows[0];
                strDeliveryNumber = Convert.ToString(dr["DeliveryNumber"]);
                strInvoiceNumber = Convert.ToString(dr["InvoiceNumber"]);

                //取消对发票号的处理
                //strInvoiceNumber = strInvoiceNumber.Split('-')[0];
            }

            strName = "AGY" + "-" +
                      strNominalMaxpower + "-" +
                      rowCount.ToString() + "PCS-" +
                      strDeliveryNumber + "-" +
                      strInvoiceNumber;

            Response.Clear();
            Response.Charset = "utf-8";
            Response.Buffer = false;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            if (Request.UserAgent.ToLower().IndexOf("firefox") > -1)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + strName + ".xls");
            }
            else
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(strName) + ".xls");
            } 
            
            Response.ContentType = "Application/ms-excel";
            Export.ExportToConergyFlashDataExcel(Response.OutputStream, dtConergyFlashData, strName);
            Response.End();
        }
        catch
        {
 
        }
    }
}