using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

public partial class MasterPage : System.Web.UI.MasterPage
{


    /* Page Load */
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            Session["userid"] = Request.QueryString["id"].ToString();
        }

    }







    /* Skins */
    //protected void cbSkins_DataBound(object sender, EventArgs e)
    //{
    //    //if (!IsPostBack)
    //    //{
    //        ListEditItem item = cbSkins.Items.FindByValue(Page.Theme);
    //        if (item == null)
    //            item = cbSkins.Items.FindByValue(BasePage.DefaultThemeName);
    //        if (item != null)
    //            cbSkins.SelectedItem = item;
    //    //}
    //}

    /* Main NavBar */




}
