<%@ WebHandler Language="C#" Class="ELPictureHandler" %>

using System;
using System.Web;

public class ELPictureHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string elPicGuid = context.Request["id"];
        string elPicPath = Convert.ToString(context.Cache[elPicGuid]);
        if (string.IsNullOrEmpty(elPicPath))
        {
            return;
        }
        string elDir = System.IO.Path.GetDirectoryName(elPicPath);
        string userName = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_username"];
        string password = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_password"];
        uint r = WNetConnectionHelper.WNetAddConnection(userName, password, elDir, null);
        
        try
        {
            context.Response.ContentType = "image/jpg";
            System.IO.FileStream stream = new System.IO.FileStream(elPicPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(stream);
            b.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(ex.Message);
        }
        finally
        {
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}