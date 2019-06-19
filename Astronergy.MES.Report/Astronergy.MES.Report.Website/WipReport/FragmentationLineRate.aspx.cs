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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Astronergy.MES.Report.DataAccess;
using DevExpress.Web;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using System.Drawing;

public partial class WipReport_FragmentationLineRate : BasePage
{
    public string lineName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deStartDate.Date = DateTime.Now;
            deEndDate.Date = DateTime.Now;
            BindShiftName();
            BindFactory();
            BindLineName();
        }
   
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {

        BindLineName();

        string sStartDate, sEndDate, sShiftName, sDateParam1, sDateParam2, sSupplier, sSupplierName, sStepName, sStepName1, sFlowName, sFlowName1, sPorMode, sPorMode1, sFactoryroomName, sLineName ;
        DateTime dtStartDate, dtEndDate, dtParam, dNow;
        int i, nNumerator, nDenominator, nTNumerator, nTDenominator, nRow;
        string[] lineNames;
        decimal dParam;
        i = 0;
        nRow = 0;

        sStartDate = deStartDate.Text.Trim();
        sEndDate = deEndDate.Text.Trim();
        //sShiftName = cboShif.Text.Trim();
        sShiftName = cboShif.SelectedItem.Value.ToString();
        sFactoryroomName = cboFactory.Text.Trim();
        sLineName = ddeWO.Text.Trim();
        lineName = "";

        dNow = DateTime.Now;

      

        if (sLineName != "")
        {
            lineNames = sLineName.Split('#');
            for (int w = 0; w < lineNames.Length; w++)
            {
                if (lineName == "")
                {
                    lineName = "'"+lineNames[w].ToString().Trim()+"'";
                }
                else
                {
                    lineName = lineName + ",'" + lineNames[w].ToString().Trim()+"'";
                }
            }
        }

        if (sStartDate == "" || sEndDate == "")
        {
            base.ShowMessageBox(this.Page, "查询参数不能都为空！");
            return;
        }
        dtStartDate = Convert.ToDateTime(sStartDate);
        dtEndDate = Convert.ToDateTime(sEndDate);
        if (dtStartDate > dtEndDate)
        {
            base.ShowMessageBox(this.Page, "起始日期不能大于结止日期！");
            return;
        }

        if (dtEndDate > dNow)
        {
            base.ShowMessageBox(this.Page, "截止日期不能大于当前日期！");
            return;
        }

        this.hidStartDate.Value = dtStartDate.ToString();
        this.hidEndDate.Value = dtEndDate.ToString();

        DataTable dtFragment = new DataTable();
        dtFragment.Columns.Add("FNO");
        dtFragment.Columns.Add("FTYPE");
        dtFragment.Columns.Add("FITME");
        dtFragment.Columns.Add("FTOT");
        dtFragment.Columns.Add("FPatchTOT");
        dtParam = dtStartDate;
        while (dtParam <= dtEndDate)
        {
            dtParam = dtStartDate.AddDays(i + 1);
            dtFragment.Columns.Add("F" + i.ToString());
            i = i + 1;
        }
        object[] objrow = new object[i + 5];
        objrow[0] = (object)"NO";
        objrow[1] = (object)"类型";
        objrow[2] = (object)"项目";
        objrow[3] = (object)"累计";
        objrow[4] = (object)"碎片数目";
        for (int r = 0; r < i; r++)
        {
            objrow[r + 5] = (object)dtStartDate.AddDays(r).ToString("yyyy-MM-dd");
        }
        dtFragment.Rows.Add(objrow);

        this.hidLoactionKey.Value = sFactoryroomName.Trim();
        this.hidShiftName.Value = sShiftName.Trim();

        DataSet dsPatchData = FragmentationLineRateData.GetLinePatchData(sFactoryroomName, lineName, dtStartDate.ToString("yyyy-MM-dd"), dtEndDate.ToString("yyyy-MM-dd"), sShiftName, "");

        #region//（焊前+焊后）总碎片率
        nNumerator = 0;
        nDenominator = 0;
        nTNumerator = 0;
        nTDenominator = 0;
        dParam = 0;
        object[] objrow1 = new object[i + 5];
        objrow1[0] = (object)"1";
        objrow1[1] = (object)"A";
        objrow1[2] = (object)"(焊前+焊后)总碎片率";

        for (int r = 0; r < i; r++)
        {
            sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
            sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

            //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 09:00:00")))
            if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
            {
                var vpatch1 = (from vp1 in dsPatchData.Tables[0].AsEnumerable()
                               where vp1.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                               && vp1.Field<string>("PATCH_ITEM") == "(焊前+焊后)总碎片率"
                               select vp1.Field<int>("PATCH_QTY")).Sum();

                var vtot1 = (from vt1 in dsPatchData.Tables[0].AsEnumerable()
                             where vt1.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                             && vt1.Field<string>("PATCH_ITEM") == "(焊前+焊后)总碎片率"
                             select vt1.Field<int>("TOT_QTY")).Sum();

                nNumerator = int.Parse(vpatch1.ToString());
                nTNumerator = nTNumerator + nNumerator;

                nDenominator = int.Parse(vtot1.ToString());
                nTDenominator = nTDenominator + nDenominator;
            }
            else
            {
                nNumerator = 0;
                nDenominator = 0;
            }

            if (nDenominator > 0)
            {
                dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
            }
            else if (nNumerator > 0)
            {
                dParam = 100;
            }
            else
            {
                dParam = 0;
            }
            objrow1[r + 5] = (object)(dParam.ToString("##0.00") + "%");
        }
        if (nTDenominator > 0)
        {
            dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
        }
        else if (nTNumerator > 0)
        {
            dParam = 100;
        }
        else
        {
            dParam = 0;
        }
        objrow1[3] = (object)(dParam.ToString("##0.00") + "%");
        objrow1[4] = (object)(nTNumerator);
        dtFragment.Rows.Add(objrow1);
        #endregion

        #region//焊前碎片率
        nNumerator = 0;
        nDenominator = 0;
        nTNumerator = 0;
        nTDenominator = 0;
        dParam = 0;
        object[] objrow2 = new object[i + 5];
        objrow2[0] = (object)"2";
        objrow2[1] = (object)"B";
        objrow2[2] = (object)"焊前碎片率";
        for (int r = 0; r < i; r++)
        {
            sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
            sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";
            //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
            if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
            {
                var vpatch2 = (from vp2 in dsPatchData.Tables[0].AsEnumerable()
                               where vp2.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                               && vp2.Field<string>("PATCH_ITEM") == "焊前碎片率"
                               select vp2.Field<int>("PATCH_QTY")).Sum();

                var vtot2 = (from vt2 in dsPatchData.Tables[0].AsEnumerable()
                             where vt2.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                             && vt2.Field<string>("PATCH_ITEM") == "焊前碎片率"
                             select vt2.Field<int>("TOT_QTY")).Sum();

                nNumerator = int.Parse(vpatch2.ToString());
                nTNumerator = nTNumerator + nNumerator;

                nDenominator = int.Parse(vtot2.ToString());
                nTDenominator = nTDenominator + nDenominator;
            }
            else
            {
                nNumerator = 0;
                nDenominator = 0;
            }

            if (nDenominator > 0)
            {
                dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
            }
            else if (nNumerator > 0)
            {
                dParam = 100;
            }
            else
            {
                dParam = 0;
            }
            objrow2[r + 5] = (object)(dParam.ToString("##0.00") + "%");
            objrow2[4] = (object)(nTNumerator);
        }
        if (nTDenominator > 0)
        {
            dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
        }
        else if (nTNumerator > 0)
        {
            dParam = 100;
        }
        else
        {
            dParam = 0;
        }
        objrow2[3] = (object)(dParam.ToString("##0.00") + "%");
        dtFragment.Rows.Add(objrow2);
        #endregion

        #region//焊后碎片率
        nNumerator = 0;
        nDenominator = 0;
        nTNumerator = 0;
        nTDenominator = 0;
        dParam = 0;
        object[] objrow3 = new object[i + 5];
        objrow3[0] = (object)"3";
        objrow3[1] = (object)"C";
        objrow3[2] = (object)"焊后碎片率";
        for (int r = 0; r < i; r++)
        {
            sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
            sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

            //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
            if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
            {
                var vpatch3 = (from vp3 in dsPatchData.Tables[0].AsEnumerable()
                               where vp3.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                               && vp3.Field<string>("PATCH_ITEM") == "焊后碎片率"
                               select vp3.Field<int>("PATCH_QTY")).Sum();

                var vtot3 = (from vt3 in dsPatchData.Tables[0].AsEnumerable()
                             where vt3.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                             && vt3.Field<string>("PATCH_ITEM") == "焊后碎片率"
                             select vt3.Field<int>("TOT_QTY")).Sum();

                nNumerator = int.Parse(vpatch3.ToString());
                nTNumerator = nTNumerator + nNumerator;

                nDenominator = int.Parse(vtot3.ToString());
                nTDenominator = nTDenominator + nDenominator;
            }
            else
            {
                nNumerator = 0;
                nDenominator = 0;
            }


            if (nDenominator > 0)
            {
                dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
            }
            else if (nNumerator > 0)
            {
                dParam = 100;
            }
            else
            {
                dParam = 0;
            }
            objrow3[r + 5] = (object)(dParam.ToString("##0.00") + "%");
        }
        if (nTDenominator > 0)
        {
            dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
        }
        else if (nTNumerator > 0)
        {
            dParam = 100;
        }
        else
        {
            dParam = 0;
        }
        objrow3[3] = (object)(dParam.ToString("##0.00") + "%");
        objrow3[4] = (object)(nTNumerator);
        dtFragment.Rows.Add(objrow3);
        #endregion

        #region//红外不良碎片率
        nNumerator = 0;
        nDenominator = 0;
        nTNumerator = 0;
        nTDenominator = 0;
        dParam = 0;
        object[] objrow4 = new object[i + 5];
        objrow4[0] = (object)"4";
        objrow4[1] = (object)"D";
        objrow4[2] = (object)"红外不良碎片率";
        for (int r = 0; r < i; r++)
        {
            sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
            sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

            //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
            if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
            {
                var vpatch4 = (from vp4 in dsPatchData.Tables[0].AsEnumerable()
                               where vp4.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                               && vp4.Field<string>("PATCH_ITEM") == "红外不良碎片率"
                               select vp4.Field<int>("PATCH_QTY")).Sum();

                var vtot4 = (from vt4 in dsPatchData.Tables[0].AsEnumerable()
                             where vt4.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                             && vt4.Field<string>("PATCH_ITEM") == "红外不良碎片率"
                             select vt4.Field<int>("TOT_QTY")).Sum();

                nNumerator = int.Parse(vpatch4.ToString());
                nTNumerator = nTNumerator + nNumerator;

                nDenominator = int.Parse(vtot4.ToString());
                nTDenominator = nTDenominator + nDenominator;
            }
            else
            {
                nNumerator = 0;
                nDenominator = 0;
            }


            if (nDenominator > 0)
            {
                dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
            }
            else if (nNumerator > 0)
            {
                dParam = 100;
            }
            else
            {
                dParam = 0;
            }
            objrow4[r + 5] = (object)(dParam.ToString("##0.00") + "%");
        }
        if (nTDenominator > 0)
        {
            dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
        }
        else if (nTNumerator > 0)
        {
            dParam = 100;
        }
        else
        {
            dParam = 0;
        }
        objrow4[3] = (object)(dParam.ToString("##0.00") + "%");
        objrow4[4] = (object)(nTNumerator);
        dtFragment.Rows.Add(objrow4);
        #endregion

        #region//硅片供应商碎片率
        DataSet dsSupplier = FragmentationLineRateData.GetSupplier("", "", "");
        if (dsSupplier.Tables[0].Rows.Count > 0)
        {
            nRow = 5;
            for (int s = 0; s < dsSupplier.Tables[0].Rows.Count; s++)
            {
                sSupplier = dsSupplier.Tables[0].Rows[s]["NAME"].ToString().Trim();
                sSupplierName = "(" + sSupplier + ")" + "碎片率";
                nRow = nRow + s;
                nNumerator = 0;
                nDenominator = 0;
                nTNumerator = 0;
                nTDenominator = 0;
                dParam = 0;
                object[] objrowsup = new object[i + 5];
                objrowsup[0] = (object)nRow;
                objrowsup[1] = (object)"E";
                objrowsup[2] = (object)sSupplierName;
                for (int r = 0; r < i; r++)
                {
                    sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
                    sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

                    //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
                    if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
                    {
                        var vpatch5 = (from vp5 in dsPatchData.Tables[0].AsEnumerable()
                                       where vp5.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                       && vp5.Field<string>("PATCH_ITEM") == sSupplierName
                                       select vp5.Field<int>("PATCH_QTY")).Sum();

                        var vtot5 = (from vt5 in dsPatchData.Tables[0].AsEnumerable()
                                     where vt5.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                     && vt5.Field<string>("PATCH_ITEM") == sSupplierName
                                     select vt5.Field<int>("TOT_QTY")).Sum();

                        nNumerator = int.Parse(vpatch5.ToString());
                        nTNumerator = nTNumerator + nNumerator;

                        nDenominator = int.Parse(vtot5.ToString());
                        nTDenominator = nTDenominator + nDenominator;
                    }
                    else
                    {
                        nNumerator = 0;
                        nDenominator = 0;
                    }


                    if (nDenominator > 0)
                    {
                        dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
                    }
                    else if (nNumerator > 0)
                    {
                        dParam = 100;
                    }
                    else
                    {
                        dParam = 0;
                    }
                    objrowsup[r + 5] = (object)(dParam.ToString("##0.00") + "%");
                }
                if (nTDenominator > 0)
                {
                    dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
                }
                else if (nTNumerator > 0)
                {
                    dParam = 100;
                }
                else
                {
                    dParam = 0;
                }
                objrowsup[3] = (object)(dParam.ToString("##0.00") + "%");
                objrowsup[4] = (object)(nTNumerator);
                dtFragment.Rows.Add(objrowsup);
            }
        }
        #endregion

        #region//各工序碎片率
        DataSet dsStepName = FragmentationLineRateData.GetStepName();
        if (dsStepName.Tables[0].Rows.Count > 0)
        {
            nRow = nRow + 1;
            for (int s = 0; s < dsStepName.Tables[0].Rows.Count; s++)
            {
                sStepName = dsStepName.Tables[0].Rows[s]["ROUTE_OPERATION_NAME"].ToString().Trim();
                sStepName1 = "(" + sStepName + ")" + "碎片率";
                nRow = nRow + s;
                nNumerator = 0;
                nDenominator = 0;
                nTNumerator = 0;
                nTDenominator = 0;
                dParam = 0;
                object[] objrowstep = new object[i + 5];
                objrowstep[0] = (object)nRow;
                objrowstep[1] = (object)"F";
                objrowstep[2] = (object)sStepName1;
                for (int r = 0; r < i; r++)
                {
                    sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
                    sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

                    //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
                    if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
                    {
                        var vpatch6 = (from vp6 in dsPatchData.Tables[0].AsEnumerable()
                                       where vp6.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                       && vp6.Field<string>("PATCH_ITEM") == sStepName1
                                       select vp6.Field<int>("PATCH_QTY")).Sum();

                        var vtot6 = (from vt6 in dsPatchData.Tables[0].AsEnumerable()
                                     where vt6.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                     && vt6.Field<string>("PATCH_ITEM") == sStepName1
                                     select vt6.Field<int>("TOT_QTY")).Sum();

                        nNumerator = int.Parse(vpatch6.ToString());
                        nTNumerator = nTNumerator + nNumerator;

                        nDenominator = int.Parse(vtot6.ToString());
                        nTDenominator = nTDenominator + nDenominator;
                    }
                    else
                    {
                        nNumerator = 0;
                        nDenominator = 0;
                    }


                    if (nDenominator > 0)
                    {
                        dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
                    }
                    else if (nNumerator > 0)
                    {
                        dParam = 100;
                    }
                    else
                    {
                        dParam = 0;
                    }
                    objrowstep[r + 5] = (object)(dParam.ToString("##0.00") + "%");
                }
                if (nTDenominator > 0)
                {
                    dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
                }
                else if (nTNumerator > 0)
                {
                    dParam = 100;
                }
                else
                {
                    dParam = 0;
                }
                objrowstep[3] = (object)(dParam.ToString("##0.00") + "%");
                objrowstep[4] = (object)(nTNumerator);
                dtFragment.Rows.Add(objrowstep);
            }
        }
        #endregion

        #region//杭州各栋别碎片率
        string[] sflow = new string[] { "A", "B", "C" };
        if (sflow.Length > 0)
        {
            nRow = nRow + 1;
            for (int s = 0; s < sflow.Length; s++)
            {
                sFlowName = sflow[s].ToString().Trim();
                sFlowName1 = "杭州(" + sFlowName + ")" + "栋碎片率";
                nRow = nRow + s;
                nNumerator = 0;
                nDenominator = 0;
                nTNumerator = 0;
                nTDenominator = 0;
                dParam = 0;
                object[] objrowflow = new object[i + 5];
                objrowflow[0] = (object)nRow;
                objrowflow[1] = (object)"G";
                objrowflow[2] = (object)sFlowName1;
                for (int r = 0; r < i; r++)
                {
                    sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
                    sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

                    //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
                    if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
                    {
                        var vpatch7 = (from vp7 in dsPatchData.Tables[0].AsEnumerable()
                                       where vp7.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                       && vp7.Field<string>("PATCH_ITEM") == sFlowName1
                                       select vp7.Field<int>("PATCH_QTY")).Sum();

                        var vtot7 = (from vt7 in dsPatchData.Tables[0].AsEnumerable()
                                     where vt7.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                     && vt7.Field<string>("PATCH_ITEM") == sFlowName1
                                     select vt7.Field<int>("TOT_QTY")).Sum();

                        nNumerator = int.Parse(vpatch7.ToString());
                        nTNumerator = nTNumerator + nNumerator;

                        nDenominator = int.Parse(vtot7.ToString());
                        nTDenominator = nTDenominator + nDenominator;
                    }
                    else
                    {
                        nNumerator = 0;
                        nDenominator = 0;
                    }


                    if (nDenominator > 0)
                    {
                        dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
                    }
                    else if (nNumerator > 0)
                    {
                        dParam = 100;
                    }
                    else
                    {
                        dParam = 0;
                    }
                    objrowflow[r + 5] = (object)(dParam.ToString("##0.00") + "%");
                }
                if (nTDenominator > 0)
                {
                    dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
                }
                else if (nTNumerator > 0)
                {
                    dParam = 100;
                }
                else
                {
                    dParam = 0;
                }
                objrowflow[3] = (object)(dParam.ToString("##0.00") + "%");
                objrowflow[4] = (object)(nTNumerator);
                dtFragment.Rows.Add(objrowflow);
            }
        }
        #endregion

        #region//各型号碎片率
        DataSet dsPromode = FragmentationLineRateData.GetProMode();
        if (dsPromode.Tables[0].Rows.Count > 0)
        {
            nRow = nRow + 1;
            for (int s = 0; s < dsPromode.Tables[0].Rows.Count; s++)
            {
                sPorMode = dsPromode.Tables[0].Rows[s]["PROMODEL_NAME"].ToString().Trim();
                sPorMode1 = "(" + sPorMode + ")" + "碎片率";
                nRow = nRow + s;
                nNumerator = 0;
                nDenominator = 0;
                nTNumerator = 0;
                nTDenominator = 0;
                dParam = 0;
                object[] objrowmode = new object[i + 5];
                objrowmode[0] = (object)nRow;
                objrowmode[1] = (object)"H";
                objrowmode[2] = (object)sPorMode1;
                for (int r = 0; r < i; r++)
                {
                    sDateParam1 = dtStartDate.AddDays(r).ToString("yyyy-MM-dd") + " 09:00:00";
                    sDateParam2 = dtStartDate.AddDays(r + 1).ToString("yyyy-MM-dd") + " 09:00:00";

                    //if ((Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd"))) || (Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd") == dNow.ToString("yyyy-MM-dd") && dNow > Convert.ToDateTime(dNow.ToString("yyyy-MM-dd") + " 08:00:00")))
                    if (Convert.ToDateTime(Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")) < Convert.ToDateTime(dNow.ToString("yyyy-MM-dd")) && dNow > Convert.ToDateTime(sDateParam2))
                    {
                        var vpatch8 = (from vp8 in dsPatchData.Tables[0].AsEnumerable()
                                       where vp8.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                       && vp8.Field<string>("PATCH_ITEM") == sPorMode1
                                       select vp8.Field<int>("PATCH_QTY")).Sum();

                        var vtot8 = (from vt8 in dsPatchData.Tables[0].AsEnumerable()
                                     where vt8.Field<string>("PATCH_DATE") == Convert.ToDateTime(sDateParam1).ToString("yyyy-MM-dd")
                                     && vt8.Field<string>("PATCH_ITEM") == sPorMode1
                                     select vt8.Field<int>("TOT_QTY")).Sum();

                        nNumerator = int.Parse(vpatch8.ToString());
                        nTNumerator = nTNumerator + nNumerator;

                        nDenominator = int.Parse(vtot8.ToString());
                        nTDenominator = nTDenominator + nDenominator;
                    }
                    else
                    {
                        nNumerator = 0;
                        nDenominator = 0;
                    }


                    if (nDenominator > 0)
                    {
                        dParam = Convert.ToDecimal(nNumerator) * 100 / Convert.ToDecimal(nDenominator);
                    }
                    else if (nNumerator > 0)
                    {
                        dParam = 100;
                    }
                    else
                    {
                        dParam = 0;
                    }
                    objrowmode[r + 5] = (object)(dParam.ToString("##0.00") + "%");
                }
                if (nTDenominator > 0)
                {
                    dParam = Convert.ToDecimal(nTNumerator) * 100 / Convert.ToDecimal(nTDenominator);
                }
                else if (nTNumerator > 0)
                {
                    dParam = 100;
                }
                else
                {
                    dParam = 0;
                }
                objrowmode[3] = (object)(dParam.ToString("##0.00") + "%");
                objrowmode[4] = (object)(nTNumerator);
                dtFragment.Rows.Add(objrowmode);
            }
        }
        #endregion

        Cache[Session.SessionID + "FRATE"] = dtFragment;
        gvFragmentation.DataSource = null;
        gvFragmentation.Columns.Clear();
        gvFragmentation.AutoGenerateColumns = true;
        gvFragmentation.DataSource = dtFragment;
        gvFragmentation.DataBind();


        seriesTable(dtFragment);




        for (int n = 0; n < gvFragmentation.Columns.Count; n++)
        {
            if (gvFragmentation.Columns[n].ToString().Equals("FTYPE"))
            {
                gvFragmentation.Columns["FTYPE"].Visible = false;
                continue;
            }
        }
    }


    #region 绘制图表

    private void seriesTable(DataTable dt)
    {
        DataTable dtSeries = new DataTable();
        dtSeries.Columns.Add("FITME", typeof(string));
        dtSeries.Columns.Add("FPatchTOT", typeof(decimal));
        dtSeries.Columns.Add("FPatchPER", typeof(decimal));

        string fItem = string.Empty;
        decimal fPatchTOT = 0;
        decimal fPatchPer = 0;

        for (int i = 1; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];

            fItem = Convert.ToString(dr["FITME"]);
            fPatchTOT = Convert.ToDecimal(dr["FPatchTOT"]);
            fPatchPer = Convert.ToDecimal(Convert.ToString(dr["FTOT"]).TrimEnd('%'));

            dtSeries.Rows.Add(new object[] { fItem, fPatchTOT, fPatchPer });

        }

        DataView dv = dtSeries.DefaultView;
        dv.Sort = "FPatchTOT DESC";

        DataTable dtChartData = dv.ToTable();


        int nRow;

        if (dtChartData.Rows.Count > 10)
        {
            nRow = 10;
        }
        else
        {
            nRow = dtChartData.Rows.Count;
        }

        DataTable seriesTable = new DataTable();

        seriesTable.Columns.Add("类型", typeof(string));

        for (int i = 0; i < nRow; i++)
        {
            seriesTable.Columns.Add(Convert.ToString(dtChartData.Rows[i]["FITME"]), typeof(decimal));
        }


        for (int j = 0; j < dtChartData.Columns.Count - 1; j++)
        {
            seriesTable.Rows.Add();

            switch (j)
            {
                case 0:
                    seriesTable.Rows[j][0] = "碎片数量";
                    break;
                case 1:
                    seriesTable.Rows[j][0] = "碎片率(%)";
                    break;
                default:
                    break;
            }

            for (int i = 0; i < nRow; i++)
            {
                seriesTable.Rows[j][i+1] = dtChartData.Rows[i][j + 1];
            }
        }

        this.gdvPatchTop.DataSource = null;
        this.gdvPatchTop.Columns.Clear();
        this.gdvPatchTop.AutoGenerateColumns = true;

        this.gdvPatchTop.DataSource = seriesTable;
        this.gdvPatchTop.DataBind();

        CreateChart(seriesTable);
    }


    private void CreateChart(DataTable dt)
    {
        #region Series
        //创建几个图形的对象
        Series series1 = CreateSeries("碎片数量", ViewType.Bar, dt, 0);
        series1.View.Color = Color.Blue;

        Series series2 = CreateSeries("碎片率", ViewType.Line, dt, 1);
        series2.View.Color = Color.Purple;
        #endregion

        WebChartControl1.Series.Clear();


        List<Series> list = new List<Series>() { series1, series2 };
        WebChartControl1.Series.AddRange(list.ToArray());
        WebChartControl1.Legend.Visible = false;
        //WebChartControl1.SeriesTemplate.LabelsVisibility = DefaultBoolean.True;

        CreateAxisY(series1, "Bar");
        CreateAxisY(series2, "Line");
    }

    /// <summary>
    /// 根据数据创建一个图形展现
    /// </summary>
    /// <param name="caption">图形标题</param>
    /// <param name="viewType">图形类型</param>
    /// <param name="dt">数据DataTable</param>
    /// <param name="rowIndex">图形数据的行序号</param>
    /// <returns></returns>
    private Series CreateSeries(string caption, ViewType viewType, DataTable dt, int rowIndex)
    {
        Series series = new Series(caption, viewType);
        for (int i = 1; i < dt.Columns.Count; i++)
        {
            string argument = dt.Columns[i].ColumnName;//参数名称
            decimal value = (decimal)dt.Rows[rowIndex][i];//参数值
            series.Points.Add(new SeriesPoint(argument, value));
        }

        //必须设置ArgumentScaleType的类型，否则显示会转换为日期格式，导致不是希望的格式显示
        //也就是说，显示字符串的参数，必须设置类型为ScaleType.Qualitative
        series.ArgumentScaleType = ScaleType.Qualitative;
        //series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;//显示标注标签

        return series;
    }

    /// <summary>
    /// 创建图表的第二坐标系
    /// </summary>
    /// <param name="series">Series对象</param>
    /// <returns></returns>
    private SecondaryAxisY CreateAxisY(Series series, string viewType)
    {
        SecondaryAxisY myAxis = new SecondaryAxisY(series.Name);

        if (((XYDiagram)WebChartControl1.Diagram).SecondaryAxesY.Count == 2)
        {
            ((XYDiagram)WebChartControl1.Diagram).SecondaryAxesY.Clear();
        }

        ((XYDiagram)WebChartControl1.Diagram).SecondaryAxesY.Add(myAxis);

        switch (viewType)
        {
            case "Bar":
                ((BarSeriesView)series.View).AxisY = myAxis;
                break;
            case "Line":
                ((LineSeriesView)series.View).AxisY = myAxis;
                break;
            default:
                break;
        }

        myAxis.Title.Text = series.Name;
        myAxis.Title.Alignment = StringAlignment.Far; //顶部对齐
        myAxis.Title.Visible = true; //显示标题
        myAxis.Title.Font = new Font("宋体", 9.0f);

        Color color = series.View.Color;//设置坐标的颜色和图表线条颜色一致

        myAxis.Title.TextColor = color;
        myAxis.Label.TextColor = color;
        myAxis.Color = color;

        return myAxis;
    }

    #endregion

    public void BindShiftName()
    {
        //DataSet dsShiftName = FragmentationLineRateData.GetShifName();
        //cboShif.DataSource = dsShiftName.Tables[0];
        //cboShif.TextField = "SHIFT_NAME";
        //cboShif.ValueField = "SHIFT_NAME";
        //cboShif.DataBind();
        cboShif.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        cboShif.Items.Insert(1, new ListEditItem("白班", "A"));
        cboShif.Items.Insert(2, new ListEditItem("夜班", "B"));
        cboShif.SelectedIndex = 0;
    }

    public void BindFactory()
    {
        DataSet dsFactory = FragmentationLineRateData.GetFactoryWorkPlace();
        cboFactory.DataSource = dsFactory.Tables[0];
        cboFactory.TextField = "LOCATION_NAME";
        cboFactory.ValueField = "LOCATION_KEY";
        cboFactory.DataBind();
        cboFactory.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        cboFactory.SelectedIndex = 0;
    }

    public void BindLineName()
    {
        //DataSet dsLine = FragmentationLineRateData.GetLineName();
        //cboLineName.DataSource = dsLine.Tables[0];
        //cboLineName.TextField = "LINE_NAME";
        //cboLineName.ValueField = "LINE_CODE";
        //cboLineName.DataBind();
        //cboLineName.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        //cboLineName.SelectedIndex = 0;
       
        DataTable dtLine  = CommonFunction.GetLine();

        ASPxListBox lstWO = this.ddeWO.FindControl("lstWO") as ASPxListBox;
        if (lstWO != null)
        {
            lstWO.DataSource = dtLine;
            lstWO.TextField = "LINE_NAME";
            lstWO.ValueField = "LINE_NAME";
            lstWO.DataBind();
            lstWO.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
        
    }

    protected void gvFragmentation_DataBound(object sender, EventArgs e)
    {
        //ASPxGridView gv = sender as ASPxGridView;
        //if (gv != null && gv.Columns.Count > 1)
        //{
        //    for (int i = 0; i < gv.Columns.Count; i++)
        //    {
        //        gv.Columns[i].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        //    }
        //}
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable dtExcel = (DataTable)Cache[Session.SessionID + "FRATE"];
        if (dtExcel == null)
        {
            btnQuery_Click(sender, e);
        }
        else
        {
            gvFragmentation.DataSource = dtExcel;
            gvFragmentation.DataBind();
        }
        this.FrateExporter.WriteXlsToResponse();
    }



    protected void gvFragmentation_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.Index > 1)
        {
            try
            {
                string startDate = string.Empty, endDate = string.Empty, sType = string.Empty, sName = string.Empty;
                string headColumnName = e.Column.FieldName;
                DataRow dr = (DataRow)gvFragmentation.GetDataRow(0);
                DataRow drKey = (DataRow)gvFragmentation.GetDataRow(e.VisibleRowIndex);
                string rowKey = drKey[gvFragmentation.KeyFieldName].ToString();
                sType = Convert.ToString(drKey["FTYPE"]);
                sName = Convert.ToString(drKey["FITME"]);

                if (headColumnName.ToUpper().Trim() == "FNO" || headColumnName.ToUpper().Trim() == "FTYPE"
                    || headColumnName.ToUpper().Trim() == "FITME")
                    return;

                if (!rowKey.Trim().Equals("项目"))
                {
                    //sType :类别
                    //sName :名称
                    if (headColumnName.ToUpper().Trim() == "FTOT")
                    {
                        //startDate = Convert.ToDateTime(this.hidStartDate.Value).ToString("yyyy-MM-dd ") + "09:00:00";
                        //endDate = Convert.ToDateTime(this.hidEndDate.Value).AddDays(1).ToString("yyyy-MM-dd ") + "09:00:00";

                        //e.DisplayText = string.Format("<a  target='_blank' href=\"FragmentationRateDtl.aspx?locationKey={1}&paraDate01={2}&paraDate02={3}&sType={4}&sName={5}&shiftname={6}\" class=\'dxgv\' style=\'text-decoration:underline;\'>{0}</a>", e.Value,
                        //                       Server.UrlEncode(hidLoactionKey.Value), Server.UrlEncode(startDate), Server.UrlEncode(endDate), Server.UrlEncode(sType), Server.UrlEncode(sName), Server.UrlEncode(hidShiftName.Value));
                    }
                    else
                    {
                        startDate = Convert.ToDateTime(Convert.ToString(dr[e.Column.FieldName])).ToString("yyyy-MM-dd ") + "09:00:00";
                        endDate = Convert.ToDateTime(startDate).AddDays(1).ToString("yyyy-MM-dd ") + "09:00:00";

                        e.Value = string.Format("<a  target='_blank' href=\"FragmentationLineRateDtl.aspx?locationKey={1}&paraDate01={2}&paraDate02={3}&sType={4}&sName={5}&shiftname={6}&lineName={7}\" class=\'dxgv\' style=\'text-decoration:underline;\'>{0}</a>", e.Value,
                        Server.UrlEncode(hidLoactionKey.Value), Server.UrlEncode(startDate), Server.UrlEncode(endDate), Server.UrlEncode(sType), Server.UrlEncode(sName), Server.UrlEncode(hidShiftName.Value), Server.UrlEncode(lineName));
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }

}