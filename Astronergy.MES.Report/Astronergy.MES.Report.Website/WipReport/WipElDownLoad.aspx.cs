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
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing.Imaging;

/// <summary>
/// 批次数据清单。
/// </summary>
public partial class WipElDownLoad : BasePage
{
    private WipElDownLoadDataAccess wipDownLoadAccess = new WipElDownLoadDataAccess();

    /// <summary>
    /// 页面载入事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
        if (this.IsCallback)
        {
            string callbackId = Convert.ToString(Request["__CALLBACKID"]);
            if (callbackId == this.grid.UniqueID)
            {
                DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_ELIVDATA"];
                if (dtLotData != null)
                {
                    //this.grid.Columns.Clear();
                    //this.grid.AutoGenerateColumns = true;
                    this.grid.DataSource = dtLotData;
                    this.grid.DataBind();
                    //this.grid.AutoGenerateColumns = false;
                }
                else
                {
                    btnSelect_Click(sender, e);
                }
            }
        }
    }

    protected void ASPTXTGUINUM_TextChanged(object sender, EventArgs e)
    {
        //$("#<%=this.btnGuiNumQuery.ClientID%>").click();
        //string guinum = this.ASPTXTGUINUM.Text;
        //base.ShowMessageBox(this.Page, s);
        //wipDownLoadAccess.GetInfByGuiNumPalletNumXuelieNum(guinum, "", "");
    }

    protected void ASPTXTPALLETNO_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ASPMEMOXULIENUM_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        if (this.ASPTXTPALLETNO.Text.Trim() == "" && this.ASPTXTGUINUM.Text.Trim() == "" && this.ASPMEMOXULIENUM.Text.Trim() == "" && this.txbBegin.Text.Trim().Length <= 0)
        {
            base.ShowMessageBox(this.Page, "出货单号,托号,组件序列号,时间范围必须输入一项,以防止查询数据量过大,影响系统速度");
            return;
        }

        string guiNum = this.ASPTXTGUINUM.Text;
        string palletNum = this.ASPTXTPALLETNO.Text;
        string xuelieNum = this.ASPMEMOXULIENUM.Text;
        //-----------添加工单号 入库时间段
        string strWorkOrderNumber = string.Empty;
        string strBegin = string.Empty;
        string strEnd = string.Empty;

        strBegin = txbBegin.Text.Trim();
        strEnd = txbEnd.Text.Trim();

        if (strBegin.Length > 0 & strEnd.Length > 0)
        {
            DateTime dtBegin = Convert.ToDateTime(strBegin);
            DateTime dtEnd = Convert.ToDateTime(strEnd);
            if (DateTime.Compare(dtBegin, dtEnd) > 0)
            {
                base.ShowMessageBox(this.Page, "结束时间不能早于开始时间");
                return;
            }
        }


        strWorkOrderNumber = txbWorkOrderNumber.Text.Trim();

        DataSet ds = wipDownLoadAccess.GetInfByGuiNumPalletNumXuelieNum(guiNum, palletNum, xuelieNum, strWorkOrderNumber, strBegin, strEnd);
        DataTable dt = ds.Tables[0];

        DataSet dsPath = wipDownLoadAccess.GetPicAddressRootPath();
        if (dsPath == null
            || ds.Tables.Count < 0
            || ds.Tables[0].Rows.Count < 0)
        {
            //给出提示。
            return;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string el = string.Empty;

            DataRow dr = dt.Rows[i];
            DateTime ttime = Convert.ToDateTime(dr["测试时间"]);
            string lotNo = Convert.ToString(dr["组件序列号"]);
            //找EL图片
            var lnq = from item in dsPath.Tables[0].AsEnumerable()
                      where Convert.ToString(item["PIC_TYPE"]) == "EL"
                      select item;

            foreach (DataRow drELRootPath in lnq)
            {
                string rootPath = Convert.ToString(drELRootPath["PIC_ADDRESS"]);
                string picDateFormat = Convert.ToString(drELRootPath["PIC_DATE_FORMAT"]);
                if (string.IsNullOrEmpty(picDateFormat))
                {
                    picDateFormat = "yyyy-M-d";
                }
                string picRelationPath = string.Format("{0}\\{1}\\{2}\\{3}.jpg",
                                               ttime.ToString("yyyy年"),
                                               ttime.ToString("M月"),
                                               ttime.ToString(picDateFormat),
                                               lotNo.Trim());

                string filePath = Path.Combine(rootPath, picRelationPath);
                string elDir = System.IO.Path.GetDirectoryName(filePath);
                string userName = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_username"];
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_password"];
                uint r = WNetConnectionHelper.WNetAddConnection(userName, password, elDir, null);

                if (File.Exists(filePath))
                {
                    dr["EL图片地址"] = filePath;
                    break;
                }
            }

            //找IV测试图片
            var lnqIv = from item in dsPath.Tables[0].AsEnumerable()
                        where Convert.ToString(item["PIC_TYPE"]) == "IV"
                        select item;

            foreach (DataRow drIVRootPath in lnqIv)
            {
                string rootPath = Convert.ToString(drIVRootPath["PIC_ADDRESS"]);
                string picDateFormat = Convert.ToString(drIVRootPath["PIC_DATE_FORMAT"]);
                if (string.IsNullOrEmpty(picDateFormat))
                {
                    picDateFormat = "yyyy-M-d";
                }
                string picRelationPath = string.Format("{0}\\{1}\\{2}\\{3}.gif",
                                               ttime.ToString("yyyy年"),
                                               ttime.ToString("M月"),
                                               ttime.ToString(picDateFormat),
                                               lotNo.Trim());

                string filePath = Path.Combine(rootPath, picRelationPath);
                string elDir = System.IO.Path.GetDirectoryName(filePath);
                string userName = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_username"];
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_password"];
                uint r = WNetConnectionHelper.WNetAddConnection(userName, password, elDir, null);
                if (File.Exists(filePath))
                {
                    dr["IV测试图片地址"] = filePath;
                    break;
                }
            }

            //找TCard图片(1)
            var lnqTCard = from item in dsPath.Tables[0].AsEnumerable()
                           where Convert.ToString(item["PIC_TYPE"]) == "TCard"
                           select item;

            foreach (DataRow drTCardRootPath in lnqTCard)
            {
                string rootPath = Convert.ToString(drTCardRootPath["PIC_ADDRESS"]);
                string picDateFormat = Convert.ToString(drTCardRootPath["PIC_DATE_FORMAT"]);
                if (string.IsNullOrEmpty(picDateFormat))
                {
                    picDateFormat = "yyyy-M-d";
                }
                string picRelationPath1 = string.Format("{0}\\{1}\\{2}\\{3}(1).jpeg",
                                               ttime.ToString("yyyy年"),
                                               ttime.ToString("M月"),
                                               ttime.ToString(picDateFormat),
                                               lotNo.Trim());
                string picRelationPath2 = string.Format("{0}\\{1}\\{2}\\{3}(2).jpeg",
                                               ttime.ToString("yyyy年"),
                                               ttime.ToString("M月"),
                                               ttime.ToString(picDateFormat),
                                               lotNo.Trim());
                string filePath1 = Path.Combine(rootPath, picRelationPath1);
                string filePath2 = Path.Combine(rootPath, picRelationPath2);
                string elDir1 = System.IO.Path.GetDirectoryName(filePath1);
                string elDir2 = System.IO.Path.GetDirectoryName(filePath2);
                string userName = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_username"];
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_password"];
                uint r1 = WNetConnectionHelper.WNetAddConnection(userName, password, elDir1, null);
                uint r2 = WNetConnectionHelper.WNetAddConnection(userName, password, elDir2, null);
                if (File.Exists(filePath1))
                {
                    dr["TCard图片地址(1)"] = filePath1;
                }
                if (File.Exists(filePath2))
                {
                    dr["TCard图片地址(2)"] = filePath2;
                }
            }

            BindData(ds);

        }
        if (ds.Tables[0].Rows.Count < 1)
        {
            this.grid.DataSource = ds.Tables[0];
            this.grid.DataBind();
        }
        this.UpdatePanelResult.Update();
    }

    public void BindData(DataSet ds)
    {
        if (ds != null)
        {
            Cache[Session.SessionID + "_ELIVDATA"] = ds.Tables[0];
            //this.grid.Columns.Clear();
            //this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = ds.Tables[0];
            this.grid.DataBind();
            //this.grid.AutoGenerateColumns = false;
        }
        else
        {
            this.grid.DataSource = null;
            this.grid.DataBind();
        }
    }

    /// <summary>     
    /// 压缩文件    
    /// /// </summary>     
    /// <param name="dir">文件目录</param>     
    /// <param name="zipfilename">zip文件名</param>     
    public void compressFiles(string[] filenames, string zipfilename)
    {

        System.Drawing.Imaging.ImageCodecInfo myImageCodecInfo;
        System.Drawing.Imaging.Encoder myEncoder;
        System.Drawing.Imaging.EncoderParameter myEncoderParameter;
        System.Drawing.Imaging.EncoderParameters myEncoderParameters;
        // Get an ImageCodecInfo object that represents the JPEG codec.

        myImageCodecInfo = GetEncoderInfo("image/jpeg");
        // Create an Encoder object based on the GUID
        // for the Quality parameter category.
        myEncoder = System.Drawing.Imaging.Encoder.Quality;
        // Create an EncoderParameters object.
        // An EncoderParameters object has an array of EncoderParameter
        // objects. In this case, there is only one
        // EncoderParameter object in the array.
        myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
        // Save the bitmap as a JPEG file with 给定的 quality level
        //压缩等级，0到100，0 最差质量，100 最佳</param>

        myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
        myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 30L);
        myEncoderParameters.Param[0] = myEncoderParameter;

        try
        {
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipfilename)))
            {
                s.SetLevel(9); // 0 - store only to 9 - means best compression  压缩等级                
                foreach (string file in filenames)
                {
                    string elDir = System.IO.Path.GetDirectoryName(file);
                    string userName = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_username"];
                    string password = System.Web.Configuration.WebConfigurationManager.AppSettings["el_share_authenticate_password"];
                    uint r = WNetConnectionHelper.WNetAddConnection(userName, password, elDir, null);


                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);

                    //byte[] file1 = File.ReadAllBytes(file);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromFile(file))
                        {


                            byte[] buffer=new byte[1];
                           

                            if (CheckBox1.Checked == false)
                            {
                                img.Save(ms, myImageCodecInfo, myEncoderParameters);
                                buffer = ms.GetBuffer();                                
                            }
                            else if (CheckBox1.Checked == true)
                            {
                                FileInfo MyFileInfo = new FileInfo(file);
                                float MyFileSize = (float)MyFileInfo.Length / 1024;
                                if (MyFileSize <= 650)
                                {
                                    buffer = File.ReadAllBytes(file);
                                }
                                else if (MyFileSize > 650 && MyFileSize < 900)
                                {
                                    int param = Convert.ToInt32(75 + 600 / MyFileSize * 22);
                                    buffer = getThumImage(file, param).ToArray();
                                }
                                else if (MyFileSize > 900 && MyFileSize < 1100)
                                {
                                    int param = Convert.ToInt32(75 + 600 / MyFileSize * 20);
                                    buffer = getThumImage(file, param).ToArray();
                                }
                                else if (1100 < MyFileSize && MyFileSize < 1200)
                                {
                                    buffer = File.ReadAllBytes(file);     
                                }
                                else if (1200 < MyFileSize && MyFileSize < 1400)
                                {
                                    int param = Convert.ToInt32(70 + 600 / MyFileSize * 20);
                                    buffer = getThumImage(file, param).ToArray();
                                }
                                else if (MyFileSize > 1400&&MyFileSize<1900)
                                {
                                    int param = Convert.ToInt32(1900 / MyFileSize * 15);
                                    buffer = getThumImage(file, param).ToArray();
                                }
                                else if (MyFileSize > 1900)
                                {
                                    int param = Convert.ToInt32(10 + 1900 / MyFileSize * 10);
                                    buffer = getThumImage(file, param).ToArray();
                                }
                            }

                            long byteLength = buffer.Length;
                            long leftByterLength = byteLength;
                            int startIndex = 0;
                            int wirteByteLength = 0;
                            while (leftByterLength > 0)
                            {
                                wirteByteLength = leftByterLength >= 4096 ? 4096 : (int)leftByterLength;
                                s.Write(buffer, startIndex, wirteByteLength);
                                leftByterLength = leftByterLength - wirteByteLength;
                                startIndex = startIndex + wirteByteLength;
                            }
                        }
                        #region
                        //using (System.Drawing.Image img = System.Drawing.Image.FromFile(file))
                        //{
                        //    FileInfo MyFileInfo = new FileInfo(file);
                        //    float MyFileSize = (float)MyFileInfo.Length / 1024;
                        //    if (MyFileSize <= 650)
                        //    {
                        //        img.Save(ms, ImageFormat.Jpeg);
                        //    }
                        //    else if (MyFileSize > 650)
                        //    {
                        //        img.Save(ms, ImageFormat.Jpeg);
                        //    }
                        //    //img.Save(ms, myImageCodecInfo, myEncoderParameters);
                        //    byte[] buffer = ms.GetBuffer();
                        //    long byteLength = buffer.Length;
                        //    long leftByterLength = byteLength;
                        //    int startIndex = 0;
                        //    int wirteByteLength = 0;
                        //    while (leftByterLength > 0)
                        //    {
                        //        wirteByteLength = leftByterLength >= 4096 ? 4096 : (int)leftByterLength;
                        //        s.Write(buffer, startIndex, wirteByteLength);
                        //        leftByterLength = leftByterLength - wirteByteLength;
                        //        startIndex = startIndex + wirteByteLength;
                        //    }
                        //}
                        #endregion
                    }

                    WNetConnectionHelper.WNetCancelConnection(userName, 1, true);
                }

                s.Finish();
                s.Close();
                }
            }

        catch (Exception EX)
        {
        }
    } 

    private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        System.Drawing.Imaging.ImageCodecInfo[] encoders;
        encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
        {
            if (encoders[j].MimeType == mimeType)
                return encoders[j];
        }
        return null;
    }

    //生成缩略图
    //</summary>
    //<param name="sourceFile">原始图片文件</param>
    //<param name="quality">质量压缩比</param>
    //<param name="outputFile">输出文件名</param>
    //<returns>成功返回true,失败则返回false</returns>
    public static MemoryStream getThumImage(String sourceFile, long quality)
    {
        MemoryStream ms = new MemoryStream();
        try
        {
            long imageQuality = quality;
            Bitmap sourceImage = new Bitmap(sourceFile);
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            sourceImage.Save(ms, jgpEncoder, myEncoderParameters);
        }
        catch(Exception ce)
        {
           
        }

        return ms;
    }

    /// <summary>
    /// 获取图片编码信息
    /// </summary>
    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {

        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }


    protected void btnDownLoad_Click(object sender, EventArgs e)
    {
        DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_ELIVDATA"];
        if (dtLotData != null)
        {
            //this.grid.Columns.Clear();
            //this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = dtLotData;
            this.grid.DataBind();
            //this.grid.AutoGenerateColumns = false;
        }
        else
        {
            btnSelect_Click(sender, e);
        }

        DataTable dt = grid.DataSource as DataTable;
        if (dt == null
                || dt.Rows.Count < 0)
        {
            base.ShowMessageBox(this.Page, "没有查询数据！");
            return;
        }
        //string[] filenames = new string[] { };
        List<string> filenames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strElPath = Convert.ToString(dt.Rows[i]["EL图片地址"]);
            if (!string.IsNullOrEmpty(strElPath))
                filenames.Add(strElPath);
        }
        //随机生成一个文件夹
        //在该文件夹夹下生成压缩包。
        string dirctoryPath = Server.MapPath(Guid.NewGuid().ToString());
        if (!Directory.Exists(dirctoryPath))
        {
            Directory.CreateDirectory(dirctoryPath);
        }
        string fileName = string.Format("{0}.zip", Guid.NewGuid().ToString());
        string filePath = Path.Combine(dirctoryPath, fileName);

        try
            {
                compressFiles(filenames.ToArray(), filePath);

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=EL.ZIP");
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
                HttpContext.Current.Response.ContentType = "application/zip";
                using (FileStream f = File.OpenRead(filePath))
                {
                    byte[] bytes = new byte[f.Length];
                    f.Read(bytes, 0, (int)f.Length);
                    HttpContext.Current.Response.BinaryWrite(bytes);
                }
                HttpContext.Current.Response.End();

                GC.Collect();
            }
            finally
            {
                if (Directory.Exists(dirctoryPath))
                {
                    Directory.Delete(dirctoryPath, true);
                }
            }

    }



    protected void btnIvDownLoad_Click(object sender, EventArgs e)
    {
        DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_ELIVDATA"];
        if (dtLotData != null)
        {
            //this.grid.Columns.Clear();
            //this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = dtLotData;
            this.grid.DataBind();
            //this.grid.AutoGenerateColumns = false;
        }
        else
        {
            btnSelect_Click(sender, e);
        }

        DataTable dt = grid.DataSource as DataTable;
        if (dt == null
                || dt.Rows.Count < 0)
        {
            base.ShowMessageBox(this.Page, "没有查询数据！");
            return;
        }
        //string[] filenames = new string[] { };
        List<string> filenames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strElPath = Convert.ToString(dt.Rows[i]["IV测试图片地址"]);
            if (!string.IsNullOrEmpty(strElPath))
                filenames.Add(strElPath);
        }
        //随机生成一个文件夹
        //在该文件夹夹下生成压缩包。
        string dirctoryPath = Server.MapPath(Guid.NewGuid().ToString());
        if (!Directory.Exists(dirctoryPath))
        {
            Directory.CreateDirectory(dirctoryPath);
        }
        string fileName = string.Format("{0}.zip", Guid.NewGuid().ToString());
        string filePath = Path.Combine(dirctoryPath, fileName);

        try
        {
            compressFiles(filenames.ToArray(), filePath);

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=IV.ZIP");
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
            HttpContext.Current.Response.ContentType = "application/zip";
            using (FileStream f = File.OpenRead(filePath))
            {
                byte[] bytes = new byte[f.Length];
                f.Read(bytes, 0, (int)f.Length);
                HttpContext.Current.Response.BinaryWrite(bytes);
            }
            HttpContext.Current.Response.End();

            GC.Collect();
        }
        finally
        {
            if (Directory.Exists(dirctoryPath))
            {
                Directory.Delete(dirctoryPath, true);
            }
        }
    }
    //TCard下载按钮Click事件
    protected void btnTCardDownLoad_Click(object sender, EventArgs e)
    {
        DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_ELIVDATA"];
        if (dtLotData != null)
        {
            //this.grid.Columns.Clear();
            //this.grid.AutoGenerateColumns = true;
            this.grid.DataSource = dtLotData;
            this.grid.DataBind();
            //this.grid.AutoGenerateColumns = false;
        }
        else
        {
            btnSelect_Click(sender, e);
        }

        DataTable dt = grid.DataSource as DataTable;
        if (dt == null
                || dt.Rows.Count < 0)
        {
            base.ShowMessageBox(this.Page, "没有查询数据！");
            return;
        }
        //string[] filenames = new string[] { };
        List<string> filenames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strElPath = Convert.ToString(dt.Rows[i]["TCard图片地址(1)"]);
            if (!string.IsNullOrEmpty(strElPath))
                filenames.Add(strElPath);

            string strElPath2 = Convert.ToString(dt.Rows[i]["TCard图片地址(2)"]);
            if (!string.IsNullOrEmpty(strElPath2))
                filenames.Add(strElPath2);
        }
        //随机生成一个文件夹
        //在该文件夹夹下生成压缩包。
        string dirctoryPath = Server.MapPath(Guid.NewGuid().ToString());
        if (!Directory.Exists(dirctoryPath))
        {
            Directory.CreateDirectory(dirctoryPath);
        }
        string fileName = string.Format("{0}.zip", Guid.NewGuid().ToString());
        string filePath = Path.Combine(dirctoryPath, fileName);

        try
        {
            compressFiles(filenames.ToArray(), filePath);

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=TCard.ZIP");
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
            HttpContext.Current.Response.ContentType = "application/zip";
            using (FileStream f = File.OpenRead(filePath))
            {
                byte[] bytes = new byte[f.Length];
                f.Read(bytes, 0, (int)f.Length);
                HttpContext.Current.Response.BinaryWrite(bytes);
            }
            HttpContext.Current.Response.End();

            GC.Collect();
        }
        finally
        {
            if (Directory.Exists(dirctoryPath))
            {
                Directory.Delete(dirctoryPath, true);
            }
        }
    }
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        DataTable dtLotData = (DataTable)Cache[Session.SessionID + "_ELIVDATA"];
        if (dtLotData == null)
        {
            btnSelect_Click(sender, e);
            dtLotData = (DataTable)Cache[Session.SessionID + "_ELIVDATA"];
        }
        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("content-disposition", "attachment;filename=\"ELIVData.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToExcel(Response.OutputStream, dtLotData);
        Response.End();
    }


    protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (string.IsNullOrEmpty(Convert.ToString(e.GetValue("EL图片地址"))))
        {
            e.Row.BackColor = System.Drawing.Color.Red;
        }
        if (string.IsNullOrEmpty(Convert.ToString(e.GetValue("IV测试图片地址"))))
        {
            e.Row.BackColor = System.Drawing.Color.Red;
        }
        if (string.IsNullOrEmpty(Convert.ToString(e.GetValue("TCard图片地址(1)"))))
        {
            e.Row.BackColor = System.Drawing.Color.Red;
        }
        if (string.IsNullOrEmpty(Convert.ToString(e.GetValue("TCard图片地址(2)"))))
        {
            e.Row.BackColor = System.Drawing.Color.Red;
        }
    }
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        ASPxGridView gv = sender as ASPxGridView;
        DataTable dt = gv.DataSource as DataTable;
        if (dt != null && e.Column.Index == 3)
        {
            string lotKey = Convert.ToString(e.Value);
            string url = string.Format(@"LotDataDetail.aspx?lotkey={0}",
                                      Server.UrlEncode(lotKey));

            e.DisplayText = string.Format("<a href=\"{1}\" onmouseover=\"status='明细';return true\" title=\"明细\" target=\"blank\" class=\"dxgv\" style=\"text-decoration:underline;\">{0}</a>",
                                          e.Value, url);
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {

    }
}

