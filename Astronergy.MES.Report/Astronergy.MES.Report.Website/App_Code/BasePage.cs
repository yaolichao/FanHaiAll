using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;


/// <summary>
/// Summary description for BasePage
/// Create by Genchill.Yng
/// DateTime:2010/11/22 15:30:20
/// </summary>
public partial class BasePage : System.Web.UI.Page
{
    public BasePage()
    {

    }
    #region 自定义消息提示框

    #region ShowMessageBox

    /// <summary>
    /// 自定义消息提示框
    /// </summary>

    public void ShowMessageBox(Page page, string message)
    {
        ShowMessageBox(page, message, MessageType.None);
    }

    public void ShowMessageBox(Page page, string message, MessageType messageType)
    {
        ShowMessageBox(page, message, messageType, "");
    }

    public void ShowMessageBox(Page page, string message, MessageType messageType, string returnUrl)
    {
        ShowMessageBox(page, message, messageType, returnUrl, false);
    }

    public void ShowMessageBox(Page page, string message, MessageType messageType, string returnUrl, bool isScriptBlock)
    {
        ShowMessageBox(page, message, messageType, returnUrl, isScriptBlock, "");
    }

    public void ShowMessageBox(Page page, string message, MessageType messageType, string returnUrl, bool isScriptBlock, string title)
    {
        message = message.Replace("'", "\\'");
        message = message.Replace("\"", "");
        message = message.Replace("\r", "");
        message = message.Replace("\n", "");

        if (isScriptBlock)
        {
            if (!ClientScript.IsClientScriptBlockRegistered("jAlert"))
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "jAlert", "showAlert('" + message + "', '" + page.ResolveClientUrl(returnUrl) + "');", true);
        }
        else
        {
            if (!ClientScript.IsStartupScriptRegistered("jAlert"))
                ScriptManager.RegisterStartupScript(page, page.GetType(), "jAlert", "showAlert('" + message + "', '" + page.ResolveClientUrl(returnUrl) + "');", true);
        }
    }

    #endregion

    public enum MessageType
    {
        None,
        Right,
        Error,
        Warning
    }
    #endregion

    public DataTable LoadResource(DataTable dtReport)
    {
        DataTable dtColumns = dtReport.Clone();
        string strProName = string.Empty;
        string k = string.Empty;
        foreach (DataRow dr in dtReport.Rows)
        {
            k = string.Empty;
            strProName = dr["PRO_NAME"].ToString().ToUpper();

            if (strProName.Equals(Resources.Lang.D100))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D100_F, Resources.Lang.D100_D);
            if (strProName.Equals(Resources.Lang.D101))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D101_F, Resources.Lang.D101_D);
            if (strProName.Equals(Resources.Lang.D102))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D102_F, Resources.Lang.D102_D);
            if (strProName.Equals(Resources.Lang.D103))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D103_F, Resources.Lang.D103_D);
            if (strProName.Equals(Resources.Lang.D104))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D104_F, Resources.Lang.D104_D);
            if (strProName.Equals(Resources.Lang.D105))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D105_F, Resources.Lang.D105_D);
            if (strProName.Equals(Resources.Lang.D106))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D106_F, Resources.Lang.D106_D);
            if (strProName.Equals(Resources.Lang.D107))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D107_F, Resources.Lang.D107_D);
            if (strProName.Equals(Resources.Lang.D108))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D108_F, Resources.Lang.D108_D);
            if (strProName.Equals(Resources.Lang.D109))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D109_F, Resources.Lang.D109_D);
            if (strProName.Equals(Resources.Lang.D110))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D110_F, Resources.Lang.D110_D);
            if (strProName.Equals(Resources.Lang.D111))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D111_F, Resources.Lang.D111_D);
            if (strProName.Equals(Resources.Lang.D112))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D112_F, Resources.Lang.D112_D);
            if (strProName.Equals(Resources.Lang.D113))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.D113_F, Resources.Lang.D113_D);

            if (strProName.Equals(Resources.Lang.E100))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E100_F, Resources.Lang.E100_D);
            if (strProName.Equals(Resources.Lang.E101))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E101_F, Resources.Lang.E101_D);
            if (strProName.Equals(Resources.Lang.E102))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E102_F, Resources.Lang.E102_D);
            if (strProName.Equals(Resources.Lang.E103))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E103_F, Resources.Lang.E103_D);
            if (strProName.Equals(Resources.Lang.E104))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E104_F, Resources.Lang.E104_D);
            if (strProName.Equals(Resources.Lang.E105))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E105_F, Resources.Lang.E105_D);
            if (strProName.Equals(Resources.Lang.E106))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E106_F, Resources.Lang.E106_D);
            if (strProName.Equals(Resources.Lang.E107))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E107_F, Resources.Lang.E107_D);
            if (strProName.Equals(Resources.Lang.E108))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E108_F, Resources.Lang.E108_D);
            if (strProName.Equals(Resources.Lang.E109))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E109_F, Resources.Lang.E109_D);
            if (strProName.Equals(Resources.Lang.E110))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E110_F, Resources.Lang.E110_D);
            if (strProName.Equals(Resources.Lang.E111))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E111_F, Resources.Lang.E111_D);
            if (strProName.Equals(Resources.Lang.E112))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E112_F, Resources.Lang.E112_D);
            if (strProName.Equals(Resources.Lang.E113))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.E113_F, Resources.Lang.E113_D);

            if (strProName.Equals(Resources.Lang.REL_PRESS))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_PRESS_F, Resources.Lang.REL_PRESS_D);
            if (strProName.Equals(Resources.Lang.REL_INPUT))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_INPUT_F, Resources.Lang.REL_INPUT_D);
            if (strProName.Equals(Resources.Lang.REL_EXCHG_IN_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_EXCHG_IN_QTY_F, Resources.Lang.REL_EXCHG_IN_QTY_D);
            if (strProName.Equals(Resources.Lang.REL_EXCHG_OUT_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_EXCHG_OUT_QTY_F, Resources.Lang.REL_EXCHG_OUT_QTY_D);
            if (strProName.Equals(Resources.Lang.REL_OUT))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_OUT_F, Resources.Lang.REL_OUT_D);
            if (strProName.Equals(Resources.Lang.REL_TOSTORE_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_TOSTORE_QTY_F, Resources.Lang.REL_TOSTORE_QTY_D);
            if (strProName.Equals(Resources.Lang.REL_TOSTORE_POWER))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_TOSTORE_POWER_F, Resources.Lang.REL_TOSTORE_POWER_D);
            k = "%";
            if (strProName.Equals(Resources.Lang.PER_GR_A0J))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_GR_A0J_F, Resources.Lang.PER_GR_A0J_D);
            if (strProName.Equals(Resources.Lang.PER_GR_AJ))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_GR_AJ_F, Resources.Lang.PER_GR_AJ_D);
            if (strProName.Equals(Resources.Lang.PER_GR_KJ))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_GR_KJ_F, Resources.Lang.PER_GR_KJ_D);
            if (strProName.Equals(Resources.Lang.PER_CELL_CRUSH))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_CELL_CRUSH_F, Resources.Lang.PER_CELL_CRUSH_D);
            if (strProName.Equals(Resources.Lang.REL_IQC_CELL_CRUSH))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_IQC_CELL_CRUSH_F, Resources.Lang.REL_IQC_CELL_CRUSH_D);
            if (strProName.Equals(Resources.Lang.REL_PRESS_CELL_CRUSH))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.REL_PRESS_CELL_CRUSH_F, Resources.Lang.REL_PRESS_CELL_CRUSH_D);
            if (strProName.Equals(Resources.Lang.WEIGHTING_EFFI))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.WEIGHTING_EFFI_F, Resources.Lang.WEIGHTING_EFFI_D);
            if (strProName.Equals(Resources.Lang.WEIGHTING_CTM))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.WEIGHTING_CTM_F, Resources.Lang.WEIGHTING_CTM_D);
            //new add
            if (strProName.Equals(Resources.Lang.PER_GR_A0J_B1511))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_GR_A0J_B1511_F, Resources.Lang.PER_GR_A0J_B1511_D);
            if (strProName.Equals(Resources.Lang.PER_GR_AJ_B1512))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_GR_AJ_B1512_F, Resources.Lang.PER_GR_AJ_B1512_D);
            if (strProName.Equals(Resources.Lang.PER_GR_KJ_B1513))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PER_GR_KJ_B1513_F, Resources.Lang.PER_GR_KJ_B1513_D);

            k = string.Empty;
            if (strProName.Equals(Resources.Lang.ERJ_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.ERJ_QTY_F, Resources.Lang.ERJ_QTY_D);
            if (strProName.Equals(Resources.Lang.SANJ_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.SANJ_QTY_F, Resources.Lang.SANJ_QTY_D);
            if (strProName.Equals(Resources.Lang.SCRAP_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.SCRAP_QTY_F, Resources.Lang.SCRAP_QTY_D);
            if (strProName.Equals(Resources.Lang.KJ_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.KJ_QTY_F, Resources.Lang.KJ_QTY_D);
            if (strProName.Equals(Resources.Lang.AJ_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.AJ_QTY_F, Resources.Lang.AJ_QTY_D);
            if (strProName.Equals(Resources.Lang.A0J_QTY))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.A0J_QTY_F, Resources.Lang.A0J_QTY_D);
            if (strProName.Equals(Resources.Lang.PLAN_INPUT))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PLAN_INPUT_F, Resources.Lang.PLAN_INPUT_D);
            if (strProName.Equals(Resources.Lang.PLAN_TOSTORE))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.PLAN_TOSTORE_F, Resources.Lang.PLAN_TOSTORE_D);
            //new add 
            if (strProName.Equals(Resources.Lang.KJ_QTY_B1501))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.KJ_QTY_B1501_F, Resources.Lang.KJ_QTY_B1501_D);
            if (strProName.Equals(Resources.Lang.AJ_QTY_B1502))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.AJ_QTY_B1502_F, Resources.Lang.AJ_QTY_B1502_D);
            if (strProName.Equals(Resources.Lang.A0J_QTY_B1503))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.A0J_QTY_B1503_F, Resources.Lang.A0J_QTY_B1503_D);
            if (strProName.Equals(Resources.Lang.ERJ_QTY_B1504))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.ERJ_QTY_B1504_F, Resources.Lang.ERJ_QTY_B1504_D);
            if (strProName.Equals(Resources.Lang.SANJ_QTY_B1505))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.SANJ_QTY_B1505_F, Resources.Lang.SANJ_QTY_B1505_D);
            if (strProName.Equals(Resources.Lang.SCRAP_QTY_B1506))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.SCRAP_QTY_B1506_F, Resources.Lang.SCRAP_QTY_B1506_D);
            if (strProName.Equals(Resources.Lang.TO_STORE_KJ))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.TO_STORE_KJ_F, Resources.Lang.TO_STORE_KJ_D);
            if (strProName.Equals(Resources.Lang.TO_STORE_AJ))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.TO_STORE_AJ_F, Resources.Lang.TO_STORE_AJ_D);
            if (strProName.Equals(Resources.Lang.TO_STORE_A0J))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.TO_STORE_A0J_F, Resources.Lang.TO_STORE_A0J_D);
            if (strProName.Equals(Resources.Lang.TO_STORE_SANJ))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.TO_STORE_SANJ_F, Resources.Lang.TO_STORE_SANJ_D);
            if (strProName.Equals(Resources.Lang.TO_STORE_ERJ))
                SetRowTowValue(dtColumns, k, dr, Resources.Lang.TO_STORE_ERJ_F, Resources.Lang.TO_STORE_ERJ_D);


        }

        if (dtReport.Columns.Contains("SEQ"))
        {
            if (dtReport.Rows.Count > 0)
            {
                DataRow dr01 = dtReport.NewRow();
                dr01["PRO_NAME"] = Resources.Lang.B_TITLE;
                dr01["PROJECT"] = Resources.Lang.B_TITLE_F;
                dr01["SEQ"] = "B00";
                dtReport.Rows.Add(dr01);

                DataRow dr02 = dtReport.NewRow();
                dr02["PRO_NAME"] = Resources.Lang.C_TITLE;
                dr02["PROJECT"] = Resources.Lang.C_TITLE_F;
                dr02["SEQ"] = "C00";
                dtReport.Rows.Add(dr02);

                DataRow dr03 = dtReport.NewRow();
                dr03["PRO_NAME"] = Resources.Lang.D_TITLE;
                dr03["PROJECT"] = Resources.Lang.D_TITLE_F;
                dr03["SEQ"] = "D00";
                dtReport.Rows.Add(dr03);

                DataRow dr04 = dtReport.NewRow();
                dr04["PRO_NAME"] = Resources.Lang.E_TITLE;
                dr04["PROJECT"] = Resources.Lang.E_TITLE_F;
                dr04["SEQ"] = "E00";
                dtReport.Rows.Add(dr04);

                DataRow dr05 = dtReport.NewRow();
                dr05["PRO_NAME"] = Resources.Lang.F_TITLE;
                dr05["PROJECT"] = Resources.Lang.F_TITLE_F;
                dr05["SEQ"] = "F00";
                dtReport.Rows.Add(dr05);

                DataRow dr06 = dtReport.NewRow();
                dr06["PRO_NAME"] = Resources.Lang.B1_TITLE;
                dr06["PROJECT"] = Resources.Lang.B1_TITLE_F;
                dr06["SEQ"] = "B1510";
                dtReport.Rows.Add(dr06);

                DataRow dr07 = dtReport.NewRow();
                dr07["PRO_NAME"] = Resources.Lang.B2_TITLE;
                dr07["PROJECT"] = Resources.Lang.B2_TITLE_F;
                dr07["SEQ"] = "B220";
                dtReport.Rows.Add(dr07);

                DataRow dr08 = dtReport.NewRow();
                dr08["PRO_NAME"] = Resources.Lang.B3_TITLE;
                dr08["PROJECT"] = Resources.Lang.B3_TITLE_F;
                dr08["SEQ"] = "B230";
                dtReport.Rows.Add(dr08);
            }                    
        }

        DataRow[] drsProject = dtReport.Select("PROJECT is null");
        if (drsProject != null && drsProject.Length > 0)
        {
            foreach (DataRow dr in drsProject)
            {
                dr["PROJECT"] = dr["PRO_NAME"];
                if (Convert.ToString(dr["SEQ"]).Equals(Resources.Lang.B222))
                {
                    dr["DESCRIPTION"] = Resources.Lang.TOWAREHOUSELEVELDISPLAY;
                }
                else if (Convert.ToString(dr["SEQ"]).Equals(Resources.Lang.B233))
                {
                    dr["DESCRIPTION"] = Resources.Lang.TOCUSTCHECKLEVELDISPLAY;
                }
            }
        }

        return dtReport;
    }

    private void SetRowTowValue(DataTable dtColumns, string calculate, DataRow dr, string project, string description)
    {
        dr["PROJECT"] = project;
        dr["DESCRIPTION"] = description;
        foreach (DataColumn dc in dtColumns.Columns)
        {
            if (dc.ColumnName.ToUpper() != "PROJECT" && dc.ColumnName.ToUpper() != "SEQ" && dc.ColumnName.ToUpper() != "DESCRIPTION" && dc.ColumnName.ToUpper() != "PRO_NAME")
            {
                try
                {
                    if (calculate.Trim() == string.Empty)
                    {
                        if (project.Equals(Resources.Lang.REL_TOSTORE_POWER_F) && Convert.ToDouble(dr[dc.ColumnName]) > 0)
                            dr[dc.ColumnName] = Convert.ToDouble(dr[dc.ColumnName]) / 1000000;
                        else
                            dr[dc.ColumnName] = Convert.ToDouble(dr[dc.ColumnName]).ToString();
                    }
                    if (calculate.Equals("%"))
                    {
                        dr[dc.ColumnName] = Convert.ToDouble(dr[dc.ColumnName]).ToString("P");
                    }
                }
                catch (Exception ex)
                { }
            }
        }
    }


    public DataTable LoadColumnsResource(DataTable dtReport)
    {  
        foreach (DataColumn dc in dtReport.Columns)
        {
            if (dc.ColumnName.Equals(Resources.Lang.PRO_ID))
                dc.Caption = Resources.Lang.PRO_ID_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.LOT_NUMBER))
                dc.Caption = Resources.Lang.LOT_NUMBER_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.LOT_CUSTOMERCODE))
                dc.Caption = Resources.Lang.LOT_CUSTOMERCODE_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.LOT_SIDECODE))
                dc.Caption = Resources.Lang.LOT_SIDECODE_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PALLET_NO))
                dc.Caption = Resources.Lang.PALLET_NO_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PALLET_TIME))
                dc.Caption = Resources.Lang.PALLET_TIME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.CHECK_POWER))
                dc.Caption = Resources.Lang.CHECK_POWER_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.LOT_COLOR))
                dc.Caption = Resources.Lang.LOT_COLOR_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.OPERATERS))
                dc.Caption = Resources.Lang.OPERATERS_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.EQUIPMENT_NAME))
                dc.Caption = Resources.Lang.EQUIPMENT_NAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.TO_WH))
                dc.Caption = Resources.Lang.TO_WH_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.TO_WH_TIME))
                dc.Caption = Resources.Lang.TO_WH_TIME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.SAP_NO))
                dc.Caption = Resources.Lang.SAP_NO_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.AVG_POWER))
                dc.Caption = Resources.Lang.AVG_POWER_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.TOTLE_POWER))
                dc.Caption = Resources.Lang.TOTLE_POWER_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PRO_LEVEL))
                dc.Caption = Resources.Lang.PRO_LEVEL_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.TIME_STAMP))
                dc.Caption = Resources.Lang.TIME_STAMP_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.STEP_NAME))
                dc.Caption = Resources.Lang.STEP_NAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.USERNAME))
                dc.Caption = Resources.Lang.USERNAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.SHIFT_NAME))
                dc.Caption = Resources.Lang.SHIFT_NAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.CUSTOMER))
                dc.Caption = Resources.Lang.CUSTOMER_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.CUSTOMER_TYPE))
                dc.Caption = Resources.Lang.CUSTOMER_TYPE_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PROMODEL_NAME))
                dc.Caption = Resources.Lang.PROMODEL_NAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.LOT_NUMBER_DEFECT))
                dc.Caption = Resources.Lang.LOT_NUMBER_DEFECT_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.REASON_CODE_NAME))
                dc.Caption = Resources.Lang.REASON_CODE_NAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.DESCRIPTION))
                dc.Caption = Resources.Lang.DESCRIPTION_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.DEFECT_NAME))
                dc.Caption = Resources.Lang.DEFECT_NAME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.WORK_ORDER_NO))
                dc.Caption = Resources.Lang.WORK_ORDER_NO_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL01))
                dc.Caption = Resources.Lang.MAITRIAL01_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL03))
                dc.Caption = Resources.Lang.MAITRIAL03_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL05))
                dc.Caption = Resources.Lang.MAITRIAL05_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL07))
                dc.Caption = Resources.Lang.MAITRIAL07_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL09))
                dc.Caption = Resources.Lang.MAITRIAL09_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL11))
                dc.Caption = Resources.Lang.MAITRIAL11_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL13))
                dc.Caption = Resources.Lang.MAITRIAL13_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL15))
                dc.Caption = Resources.Lang.MAITRIAL15_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL17))
                dc.Caption = Resources.Lang.MAITRIAL17_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL19))
                dc.Caption = Resources.Lang.MAITRIAL19_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL21))
                dc.Caption = Resources.Lang.MAITRIAL21_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL23))
                dc.Caption = Resources.Lang.MAITRIAL23_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.MAITRIAL25))
                dc.Caption = Resources.Lang.MAITRIAL25_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.SEQ))
                dc.Caption = Resources.Lang.SEQ_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PATCH_QUANTITY))
                dc.Caption = Resources.Lang.PATCH_QUANTITY_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PressCells))
                dc.Caption = Resources.Lang.PressCells_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.CUSTCHECK_TIME))
                dc.Caption = Resources.Lang.CUSTCHECK_TIME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.WORK_ORDER_NO2))
                dc.Caption = Resources.Lang.WORK_ORDER_NO2_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PRO_ID2))
                dc.Caption = Resources.Lang.PRO_ID2_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.USERNAME2))
                dc.Caption = Resources.Lang.USERNAME2_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.TIME_STAMP2))
                dc.Caption = Resources.Lang.TIME_STAMP2_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.TTIME))
                dc.Caption = Resources.Lang.TTIME_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.COEF_PMAX))
                dc.Caption = Resources.Lang.COEF_PMAX_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.PM))
                dc.Caption = Resources.Lang.PM_CAPTION;

            if (dc.ColumnName.Equals(Resources.Lang.CellGrade))
                dc.Caption = Resources.Lang.CellGrade;


        }

        return dtReport;
    }
}
