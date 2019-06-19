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
using DevExpress.Web;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System.Drawing;

public partial class WipReport_CTMReport : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindFactory();
            BindPowerType();
            BindProMode();
            deStartDate.Date = DateTime.Now;
            deEndDate.Date = DateTime.Now;
            GetWOPID();
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string sFactory, sPowerType, sWO, sProID, sStartDate, sEndDate, sDeviceNum, sGlass, sEva, sSILot, sBusBar, sSingle, sJunctionbox,sModleCode;
        string sQWO, sQProID,sQDeviceNum,sQModleCode;
        string[] WO, ProID,DeviceNum,ModleCode;
        int nCellEffQty, nPowerLevel, nRowQty, nTotQty, nEstimateTot, nInputTot, nTestTot,nDays;
        string sCellEff, sPowerLevel;
        double dRowSum,dTotPower;
        DateTime dtStartDay, dtEndDay, dtNowDay;
        sQWO = "";
        sQProID = "";
        sQDeviceNum = "";
        nCellEffQty = 0;
        nPowerLevel = 0;
        sCellEff = "";
        sPowerLevel = "";
        nRowQty = 0;
        dRowSum = 0.0;
        nTotQty = 0;
        dTotPower = 0.0;
        nEstimateTot = 0;
        nInputTot = 0;
        nTestTot = 0;
        sModleCode = "";
        sQModleCode = "";
        nDays = 0;

        sFactory = Convert.ToString(cboFactory.Value);
        sPowerType = Convert.ToString(cboPowerType.Value);
        sModleCode = ddeProMode.Text.Trim();
        sWO = ddeWO.Text.Trim();
        sProID = ddeProID.Text.Trim();
        sDeviceNum = txtDeviceNum.Text.Trim();
        dtStartDay = DateTime.Parse(deStartDate.Text);
        dtEndDay = DateTime.Parse(deEndDate.Text);
        sStartDate = Convert.ToDateTime(deStartDate.Text).ToString("yyyy-MM-dd") + " 00:00:00";
        sEndDate = Convert.ToDateTime(deEndDate.Text).ToString("yyyy-MM-dd") + " 23:59:59";
        sGlass = txtGlass.Text.Trim();
        sEva = txtEVA.Text.Trim();
        sSILot = txtSILOT.Text.Trim();
        sSingle = txtSingle.Text.Trim();
        sBusBar = txtBusbar.Text.Trim();
        sJunctionbox = txtJunctionbox.Text.Trim();

        if (sWO != "")
        {
            WO = sWO.Split('#');
            for (int w = 0; w < WO.Length; w++)
            {
                if (sQWO == "")
                {
                    sQWO = WO[w].ToString().Trim();
                }
                else
                {
                    sQWO = sQWO + "," + WO[w].ToString().Trim();
                }
            }
        }
        if (sProID != "")
        {
            ProID = sProID.Split('#');
            for (int p = 0; p < ProID.Length; p++)
            {
                if (sQProID == "")
                {
                    sQProID = ProID[p].ToString().Trim();
                }
                else
                {
                    sQProID = sQProID + "," + ProID[p].ToString().Trim();
                }
            }
        }
        if (sDeviceNum != "")
        {
            DeviceNum = sDeviceNum.Split('#');
            for (int d = 0; d < DeviceNum.Length; d++)
            {
                if (sQDeviceNum == "")
                {
                    sQDeviceNum = DeviceNum[d].ToString().Trim();
                }
                else
                {
                    sQDeviceNum = sQDeviceNum + "," + DeviceNum[d].ToString().Trim();
                }
            }
        }
        if (sModleCode != "")
        {
            ModleCode = sModleCode.Split('#');
            for (int m = 0; m < ModleCode.Length; m++)
            {
                if (sQModleCode == "")
                {
                    sQModleCode = ModleCode[m].ToString().Trim();
                }
                else
                {
                    sQModleCode = sQModleCode + "," + ModleCode[m].ToString().Trim();
                }
            }
        }
        string partNumber = this.txtPartNumber.Text.Trim().Replace('#', ',');

        CTMReportFunction ctmRPTFun = new CTMReportFunction();

        DataSet dsPower = ctmRPTFun.GetPowerDate(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, sGlass, sEva, sSILot, sSingle, sBusBar, sJunctionbox, partNumber);
        DataSet dsPowerLevel = ctmRPTFun.GetPowerLevelDate(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, sGlass, sEva, sSILot, sSingle, sBusBar, sJunctionbox, partNumber);
        DataSet dsCellEff = ctmRPTFun.GetCellEffDate(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, sGlass, sEva, sSILot, sSingle, sBusBar, sJunctionbox, partNumber);
        DataSet dsPorLot = ctmRPTFun.GetCreateLot(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, partNumber);
        DataSet dsIVTest = ctmRPTFun.GetIVDate(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, sGlass, sEva, sSILot, sSingle, sBusBar, sJunctionbox, partNumber);
        DataSet dsCTM = ctmRPTFun.GetCTMData(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, sGlass, sEva, sSILot, sSingle, sBusBar, sJunctionbox, partNumber);
        DataSet dsCTMDay = ctmRPTFun.GetCTMDataDay(sFactory, sQModleCode, sPowerType, sQWO, sQProID, sQDeviceNum, sStartDate, sEndDate, sGlass, sEva, sSILot, sSingle, sBusBar, sJunctionbox, partNumber);

        if (dsCellEff.Tables[0].Rows.Count > 0)
        {
            #region//CTM分布数据源
            DataTable dtCTMReport = new DataTable();
            nCellEffQty = dsCellEff.Tables[0].Rows.Count;
            dtCTMReport.Columns.Add("功率档位");
            for (int c = 0; c < nCellEffQty; c++)
            {
                dtCTMReport.Columns.Add(dsCellEff.Tables[0].Rows[c]["VC_CELLEFF"].ToString().Trim());
            }
            dtCTMReport.Columns.Add("总计");

            nPowerLevel = dsPowerLevel.Tables[0].Rows.Count;
            nRowQty = 0;
            dRowSum = 0.0;
            nTotQty = 0;
            var totqty = (from t in dsPower.Tables[0].AsEnumerable()
                          select t.Field<int>("QTY")).Sum();
            nTotQty = int.Parse(totqty.ToString());
            for (int l = 0; l < nPowerLevel; l++)
            {
                sPowerLevel = "";
                nRowQty = 0;
                dRowSum = 0;
                nEstimateTot = 0;
                sPowerLevel = dsPowerLevel.Tables[0].Rows[l]["POWER_LEVEL"].ToString().Trim();
                object[] objrow1 = new object[nCellEffQty];
                objrow1[0] = (object)(sPowerLevel + "档位");
                object[] objrow2 = new object[nCellEffQty + 2];
                objrow2[0] = (object)"组件数量";
                object[] objrow3 = new object[nCellEffQty + 2];
                objrow3[0] = (object)"平均功率";
                object[] objrow4 = new object[nCellEffQty + 2];
                objrow4[0] = (object)"百分比";
                object[] objrow5 = new object[nCellEffQty + 2];
                objrow5[0] = (object)"预估产出数量";
                for (int c = 0; c < nCellEffQty; c++)
                {
                    sCellEff = "";
                    sCellEff = dsCellEff.Tables[0].Rows[c]["VC_CELLEFF"].ToString().Trim();
                    var row1 = (from r1 in dsPorLot.Tables[0].AsEnumerable()
                                where r1.Field<string>("EFFICIENCY") == sCellEff
                                select r1.Field<int>("QTY")).Sum();
                    int nrow1 = int.Parse(row1.ToString());
                    var row4 = (from r4 in dsPower.Tables[0].AsEnumerable()
                                where r4.Field<string>("VC_CELLEFF") == sCellEff
                                select r4.Field<int>("QTY")).Sum();
                    int nrow4 = int.Parse(row4.ToString());

                    var row2 = (from r2 in dsPower.Tables[0].AsEnumerable()
                                where r2.Field<string>("POWER_LEVEL") == sPowerLevel
                                && r2.Field<string>("VC_CELLEFF") == sCellEff
                                select r2.Field<int>("QTY")).Sum();
                    int nrow2 = int.Parse(row2.ToString());
                    var row3 = (from r3 in dsPower.Tables[0].AsEnumerable()
                                where r3.Field<string>("POWER_LEVEL") == sPowerLevel
                                && r3.Field<string>("VC_CELLEFF") == sCellEff
                                select r3.Field<decimal>("SUM_POWER")).Sum();
                    double drow3 = double.Parse(row3.ToString());
                    nRowQty = nRowQty + nrow2;
                    dRowSum = dRowSum + drow3;
                    objrow2[c + 1] = (object)nrow2.ToString();
                    //objrow3[c + 1] = (object)(drow3 / nrow2).ToString("##0.00");
                    if (nrow2 == 0)
                    {
                        objrow3[c + 1] = (object)0.ToString("##0.00");
                    }
                    else
                    {
                        objrow3[c + 1] = (object)(drow3 / nrow2).ToString("##0.00");
                    }
                    //objrow4[c + 1] = (object)((nrow2 * 100.0 / nrow4).ToString("##0.00") + "%");
                    //objrow5[c + 1] = (object)(nrow2 * nrow1 / nrow4).ToString("##0");
                    if (nrow4 == 0)
                    {
                        objrow4[c + 1] = (object)"0.00%";
                        objrow5[c + 1] = (object)"0";
                        nEstimateTot = nEstimateTot + 0;
                    }
                    else
                    {
                        objrow4[c + 1] = (object)((nrow2 * 100.0 / nrow4).ToString("##0.00") + "%");
                        objrow5[c + 1] = (object)(nrow2 * nrow1 / nrow4).ToString("##0");
                        nEstimateTot = nEstimateTot + Convert.ToInt32((nrow2 * nrow1 / nrow4).ToString("##0"));
                    }
                }
                objrow2[nCellEffQty + 1] = (object)nRowQty.ToString();
                objrow3[nCellEffQty + 1] = (object)(dRowSum / nRowQty).ToString("##0.00");
                objrow4[nCellEffQty + 1] = (object)((nRowQty * 100.0 / nTotQty).ToString("##0.00") + "%");
                objrow5[nCellEffQty + 1] = (object)nEstimateTot.ToString("##0");
                dtCTMReport.Rows.Add(objrow1);
                dtCTMReport.Rows.Add(objrow2);
                dtCTMReport.Rows.Add(objrow3);
                dtCTMReport.Rows.Add(objrow4);
                dtCTMReport.Rows.Add(objrow5);
            }

            object[] objtot1 = new object[nCellEffQty + 2];
            objtot1[0] = (object)"组件数量汇总";
            object[] objtot2 = new object[nCellEffQty + 2];
            objtot2[0] = (object)"平均功率汇总";
            object[] objtot3 = new object[nCellEffQty + 2];
            objtot3[0] = (object)"百分比汇总";
            for (int t = 0; t < nCellEffQty; t++)
            {
                sCellEff = "";
                sCellEff = dsCellEff.Tables[0].Rows[t]["VC_CELLEFF"].ToString().Trim();
                var tot1 = (from tr1 in dsPower.Tables[0].AsEnumerable()
                            where tr1.Field<string>("VC_CELLEFF") == sCellEff
                            select tr1.Field<int>("QTY")).Sum();
                int ntot1 = int.Parse(tot1.ToString());
                var tot2 = (from tr2 in dsPower.Tables[0].AsEnumerable()
                            where tr2.Field<string>("VC_CELLEFF") == sCellEff
                            select tr2.Field<decimal>("SUM_POWER")).Sum();
                double dtot2 = double.Parse(tot2.ToString());

                objtot1[t + 1] = (object)ntot1.ToString();
                //objtot2[t + 1] = (object)(dtot2 / ntot1).ToString("##0.00");
                if (ntot1 == 0)
                {
                    objtot2[t + 1] = (object)"0.00";
                }
                else
                {
                    objtot2[t + 1] = (object)(dtot2 / ntot1).ToString("##0.00");
                }
                objtot3[t + 1] = (object)"100.00%";
                dTotPower = dTotPower + dtot2;
            }
            if (nTotQty == 0)
            {
                objtot1[nCellEffQty + 1] = (object)nTotQty.ToString("##0");
                objtot2[nCellEffQty + 1] = (object)"##0.00";
                objtot3[nCellEffQty + 1] = (object)"0.00%";
            }
            else
            {
                objtot1[nCellEffQty + 1] = (object)nTotQty.ToString("##0");
                objtot2[nCellEffQty + 1] = (object)(dTotPower / nTotQty).ToString("##0.00");
                objtot3[nCellEffQty + 1] = (object)"100.00%";
            }
            dtCTMReport.Rows.Add(objtot1);
            dtCTMReport.Rows.Add(objtot2);
            dtCTMReport.Rows.Add(objtot3);

            object[] objctm = new object[nCellEffQty + 2];
            objctm[0] = (object)"CTM";
            object[] objinput = new object[nCellEffQty + 2];
            objinput[0] = (object)"投入数量";
            object[] objiv = new object[nCellEffQty + 2];
            objiv[0] = (object)"测试数量";
            for (int c = 0; c < nCellEffQty; c++)
            {
                sCellEff = "";
                sCellEff = dsCellEff.Tables[0].Rows[c]["VC_CELLEFF"].ToString().Trim();
                var ctm = (from m in dsCTM.Tables[0].AsEnumerable()
                           where m.Field<string>("VC_CELLEFF") == sCellEff
                           select m.Field<decimal>("CTM")).Sum();
                double dctm = double.Parse(ctm.ToString());
                objctm[c + 1] = (object)(dctm * 100).ToString("##0.00") + "%";

                var input = (from p in dsPorLot.Tables[0].AsEnumerable()
                             where p.Field<string>("EFFICIENCY") == sCellEff
                             select p.Field<int>("QTY")).Sum();
                int inputqty = Convert.ToInt32(input.ToString());
                objinput[c + 1] = (object)inputqty.ToString();

                var ivtest = (from iv in dsIVTest.Tables[0].AsEnumerable()
                              where iv.Field<string>("VC_CELLEFF") == sCellEff
                              select iv.Field<int>("QTY")).Sum();
                int ivtestqty = Convert.ToInt32(ivtest.ToString());
                objiv[c + 1] = (object)ivtestqty.ToString();

                nInputTot = nInputTot + inputqty;
                nTestTot = nTestTot + ivtestqty;
            }
            objinput[nCellEffQty + 1] = (object)nInputTot.ToString("##0");
            objiv[nCellEffQty + 1] = (object)nTestTot.ToString("##0");

            dtCTMReport.Rows.Add(objctm);
            dtCTMReport.Rows.Add(objinput);
            dtCTMReport.Rows.Add(objiv);
            #endregion

            #region//CTM推移数据源
            dtNowDay = dtStartDay;
            DataTable dtCTMReport2 = new DataTable();
            dtCTMReport2.Columns.Add("转换效率");
            nDays = nDays + 1;
            while (dtNowDay <= dtEndDay)
            {
                dtCTMReport2.Columns.Add(dtNowDay.ToString("yyyy-MM-dd"));
                dtNowDay = dtNowDay.AddDays(1);
                nDays = nDays + 1;
            }
            for (int k = 0; k < nCellEffQty; k++)
            {
                sCellEff = "";
                sCellEff = dsCellEff.Tables[0].Rows[k]["VC_CELLEFF"].ToString().Trim();
                object[] objCTMDay = new object[nDays];
                objCTMDay[0] = (object)sCellEff;
                dtNowDay = dtStartDay;
                for (int d = 0; d < nDays - 1; d++)
                {
                    var ctmday = (from cd in dsCTMDay.Tables[0].AsEnumerable()
                                 where cd.Field<string>("VC_CELLEFF") == sCellEff
                                 && cd.Field<string>("T_DATE") == dtNowDay.ToString("yyyy-MM-dd")
                                 select cd.Field<decimal>("CTM")).Sum();

                    double dtd = Convert.ToDouble(ctmday.ToString());
                    objCTMDay[d + 1] = (object)(dtd * 100).ToString("##0.00");
                    dtNowDay = dtNowDay.AddDays(1);
                }
                dtCTMReport2.Rows.Add(objCTMDay);
            }
            #endregion

            Cache[Session.SessionID + "CTMREP"] = dtCTMReport;
            gvCTMReport.DataSource = null;
            gvCTMReport.Columns.Clear();
            gvCTMReport.AutoGenerateColumns = true;
            gvCTMReport.DataSource = dtCTMReport;
            gvCTMReport.DataBind();


            Cache[Session.SessionID + "CTMREP2"] = dtCTMReport2;
            gvCTMReport2.DataSource = null;
            gvCTMReport2.Columns.Clear();
            gvCTMReport2.AutoGenerateColumns = true;
            gvCTMReport2.DataSource = dtCTMReport2;
            gvCTMReport2.DataBind();

            #region//CTM推移图
            XYDiagram diagram = this.chartCTMDay.Diagram as XYDiagram;
            diagram.AxisX.Label.Angle = -45;
            diagram.AxisY.Range.Auto = false;
            diagram.AxisY.Range.MinValue = 0.96;
            diagram.AxisY.NumericOptions.Format = NumericFormat.Percent;
            this.chartCTMDay.Series.Clear();
            foreach (DataRow dr in dtCTMReport2.Rows)
            {
                string sname = dr[0].ToString();
                Series s = new Series(sname,ViewType.Line);
                s.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                dtNowDay = dtStartDay;
                for (int d = 0; d < nDays - 1; d++)
                {
                    string sY = dtNowDay.ToString("yyyy-MM-dd");
                    //double dX = Convert.ToDouble(dr[d+1].ToString());
                    SeriesPoint p = null;
                    string test = dr[d + 1].ToString();
                    if (Convert.ToDouble(dr[d + 1]) <= 0.0)
                    {
                        p = new SeriesPoint(sY);
                        p.IsEmpty = true;
                    }
                    else
                    {
                        double val = Convert.ToDouble(dr[d + 1]);
                        p = new SeriesPoint(sY, val/100);
                    }
                    s.Points.Add(p);
                    dtNowDay = dtNowDay.AddDays(1);
                }
                chartCTMDay.Series.Add(s);
            }
            #endregion

        }
        else
        {
            DataTable dtCTMReport = new DataTable();
            Cache[Session.SessionID + "CTMREP"] = dtCTMReport;
            gvCTMReport.DataSource = null;
            gvCTMReport.DataBind();

            DataTable dtCTMReport2 = new DataTable();
            Cache[Session.SessionID + "CTMREP2"] = dtCTMReport2;
            gvCTMReport2.DataSource = null;
            gvCTMReport2.DataBind();
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable dtExcel = (DataTable)Cache[Session.SessionID + "CTMREP"];
        if (dtExcel == null)
        {
            btnQuery_Click(sender, e);
        }
        else
        {
            gvCTMReport.DataSource = dtExcel;
            gvCTMReport.DataBind();
        }
        this.expCTMReport.WriteXlsToResponse();
    }

    public void BindFactory()
    {
        DataSet dsFactory = CTMReportFunction.GetFactoryWorkPlace();
        cboFactory.DataSource = dsFactory.Tables[0];
        cboFactory.TextField = "LOCATION_NAME";
        cboFactory.ValueField = "LOCATION_KEY";
        cboFactory.DataBind();
        cboFactory.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        cboFactory.SelectedIndex = 0;
    }

    public void BindPowerType()
    {
        cboPowerType.Items.Insert(0, new ListEditItem("5W", "A"));
        cboPowerType.Items.Insert(1, new ListEditItem("2.5W", "B"));
        cboPowerType.SelectedIndex = 0;
    }

    protected void gvCTMReport_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        if (gv != null && gv.Columns.Count > 1)
        {
            gv.Columns[0].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        }
    }

    protected void deEndDate_DateChanged(object sender, EventArgs e)
    {
        BindProMode();
        GetWOPID();
    }

    protected void deStartDate_DateChanged(object sender, EventArgs e)
    {
        BindProMode();
        GetWOPID();
    }

    public void GetWOPID()
    {
        string sFactory,sStartDate, sEndDate;
        sFactory = Convert.ToString(cboFactory.Value);
        sStartDate = Convert.ToDateTime(deStartDate.Text).ToString("yyyy-MM-dd") + " 00:00:00";
        sEndDate = Convert.ToDateTime(deEndDate.Text).ToString("yyyy-MM-dd") + " 23:59:59";

        ddeProMode.Text = "";
        ddeProID.Text = "";
        ddeWO.Text = "";

        DataSet dsWO = CTMReportFunction.GetWO(sFactory, sStartDate, sEndDate);
        DataSet dsPID = CTMReportFunction.GetPID(sFactory, sStartDate, sEndDate);
        
        ASPxListBox lstPID = this.ddeProID.FindControl("lstProID") as ASPxListBox;
        if (lstPID != null)
        {
            lstPID.DataSource = dsPID.Tables[0];
            lstPID.TextField = "PRO_ID";
            lstPID.ValueField = "PRO_ID";
            lstPID.DataBind();
            lstPID.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
        ASPxListBox lstWO = this.ddeWO.FindControl("lstWO") as ASPxListBox;
        if (lstWO != null)
        {
            lstWO.DataSource = dsWO.Tables[0];
            lstWO.TextField = "WORK_ORDER_NO";
            lstWO.ValueField = "WORK_ORDER_NO";
            lstWO.DataBind();
            lstWO.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
    }

    public void BindProMode()
    {
        DataSet dsProMode = CTMReportFunction.GetProMode("");
        ASPxListBox lst = this.ddeProMode.FindControl("lstProMode") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dsProMode.Tables[0];
            lst.TextField = "PROMODEL_NAME";
            lst.ValueField = "PROMODEL_NAME";
            lst.DataBind();
            lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
    }

    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        DataTable dtExcel2 = (DataTable)Cache[Session.SessionID + "CTMREP2"];
        if (dtExcel2 == null)
        {
            btnQuery_Click(sender, e);
        }
        else
        {
            gvCTMReport2.DataSource = dtExcel2;
            gvCTMReport2.DataBind();
        }
        this.expCTMReport2.WriteXlsToResponse();
    }

    protected void gvCTMReport2_DataBound(object sender, EventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        if (gv != null && gv.Columns.Count > 1)
        {
            gv.Columns[0].CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
        }
    }
}
