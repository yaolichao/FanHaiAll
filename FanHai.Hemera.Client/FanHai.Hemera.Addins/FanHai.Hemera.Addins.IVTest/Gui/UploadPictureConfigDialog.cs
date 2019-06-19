using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;
using System.Globalization;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// 上传EL/IV图片的配置对话框。
    /// </summary>
    public partial class UploadPictureConfigDialog : BaseDialog
    {
        private string _pictureType = "EL"; //EL or IV
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public UploadPictureConfigDialog(string pictureType)
        {
            InitializeComponent();
            this._pictureType = pictureType;
            //this.Text = string.Format("{0}图片上传配置", pictureType);
            this.Text = string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.msg.0001}"), pictureType);
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lciDestRootPath.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0001}");//目标文件夹根目录
            lciDestPathFormat.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0002}");//目标文件夹格式化字符串
            lciFileExistension.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0003}");//文件扩展名
            lciUserName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0004}");//目标文件夹用户名
            lciUserPassword.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0005}");//目标文件夹用户密码
            chkDeleteLocalFile.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0006}");//删除本地图片文件
            chkSemimanufactures.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0007}");//半成品图片上传
            btnOk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0008}");//确定
            btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0009}");//取消
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0010}");//EL图片上传配置对话框
            this.lciSrcRootPath.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0011}");//源文件夹根目录
            this.sBtnSrcRootPath.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0012}");//选择文件夹
            this.lciSrcPathFormat.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.lbl.0013}");//源文件夹格式化字符串
            
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadPictureConfigDialog_Load(object sender, EventArgs e)
        {
            InitControlValue();
            AssignmentForLookUpEdit();
        }
        //给下拉框赋值，数据可在主数据维护表Basic_UploadPicture中修改 
        public void AssignmentForLookUpEdit()
        {
            string[] l_s = new string[] { "DisplayValue", "Value", "Categoryflag" };
                string category = "Basic_UploadPicture";
                System.Data.DataTable dt_PVLine = BaseData.Get(l_s, category);
                DataRow[] drDestRootPath = dt_PVLine.Select(string.Format("Categoryflag='{0}'", "DestRootPath"));
                DataRow[] drFileExistention = dt_PVLine.Select(string.Format("Categoryflag='{0}'", "FileExistention"));
                if (drDestRootPath.Length > 0 && drDestRootPath != null)
                {
                    DataTable dt = drDestRootPath[0].Table.Clone();
                    foreach (DataRow dr in drDestRootPath)
                    {
                        dt.ImportRow(dr);
                    }
                    luDestRootPath.Properties.DataSource = dt;
                    luDestRootPath.Properties.DisplayMember = "DisplayValue";
                    luDestRootPath.Properties.ValueMember = "Value";
                    luDestRootPath.Properties.NullText = "";

                }
                if (drFileExistention.Length > 0 && drFileExistention != null)
                {
                    DataTable dt = drFileExistention[0].Table.Clone();
                    foreach (DataRow dr in drFileExistention)
                    {
                        dt.ImportRow(dr);
                    }
                   
                    luFileExistention.Properties.DataSource = dt;
                    luFileExistention.Properties.DisplayMember = "DisplayValue";
                    luFileExistention.Properties.ValueMember = "Value";
                    luFileExistention.Properties.NullText = "";

                }

        }


        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            string srcRootPath = string.Empty;
            string srcPathFormat = string.Empty;
            string destRootPath = string.Empty;
            string destPathFormat = string.Empty;
            string fileExistension = string.Empty;
            string userName = string.Empty;
            string userPassword = string.Empty;
            string isDeleteLocalFile = string.Empty;
            string isSemimanufactures = string.Empty;
            if (this._pictureType == "EL")
            {
                chkSemimanufactures.Visible = true;
                srcRootPath = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_SOURCE_ROOT_PATH).Trim();
                srcPathFormat = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_SOURCE_PATH_FORMAT).Trim();
                destRootPath = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_DEST_ROOT_PATH).Trim();
                destPathFormat = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_DEST_PATH_FORMAT).Trim();
                fileExistension = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_EXISTENSION).Trim();
                userName = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_USER_NAME).Trim();
                userPassword = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_USER_PASSWORD).Trim();
                isDeleteLocalFile = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_IS_DELETE_LOCAL_FILE).Trim();
                isSemimanufactures = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_IS_SEMIMANUFACTURES).Trim();
            }
            else if (this._pictureType == "IV")
            {
                srcRootPath = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_SOURCE_ROOT_PATH).Trim();
                srcPathFormat = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_SOURCE_PATH_FORMAT).Trim();
                destRootPath = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_DEST_ROOT_PATH).Trim();
                destPathFormat = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_DEST_PATH_FORMAT).Trim();
                fileExistension = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_EXISTENSION).Trim();
                userName = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_USER_NAME).Trim();
                userPassword = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_USER_PASSWORD).Trim();
                isDeleteLocalFile = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_IS_DELETE_LOCAL_FILE).Trim();

            }
            else
            {
                srcRootPath = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_SOURCE_ROOT_PATH).Trim();
                srcPathFormat = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_SOURCE_PATH_FORMAT).Trim();
                destRootPath = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_DEST_ROOT_PATH).Trim();
                destPathFormat = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_DEST_PATH_FORMAT).Trim();
                fileExistension = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_EXISTENSION).Trim();
                userName = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_USER_NAME).Trim();
                userPassword = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_USER_PASSWORD).Trim();
                isDeleteLocalFile = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_IS_DELETE_LOCAL_FILE).Trim();
            }

            if (!string.IsNullOrEmpty(isDeleteLocalFile))
            {
                this.chkDeleteLocalFile.Checked = Convert.ToBoolean(isDeleteLocalFile);
            }
            if (!string.IsNullOrEmpty(isSemimanufactures))
            {
                this.chkSemimanufactures.Checked = Convert.ToBoolean(isSemimanufactures);
            }

            this.teDestPathFormat.Text = destPathFormat;
            this.teDestRootPath.Text = destRootPath;
            this.teFileExistention.Text = fileExistension;
            this.teSrcPathFormat.Text = srcPathFormat;
            this.teSrcRootPath.Text = srcRootPath;
            this.teUserPassword.Text = userPassword;
            this.teUserName.Text = userName;

            this.teUserPassword.Enabled = false;
            this.teUserName.Enabled = false;
            //将目标地址设为只读
            //this.teDestRootPath.Enabled = false;
            //this.teFileExistention.Enabled = false;
            this.teDestPathFormat.Enabled = false;
           

            string category = "BASE_UPLOADPICTURE_NAMEANDPASSWORD";
            string[] l_s = new string[] { "NAME", "PASSWORD", "IS_FLAG", "MARKS", "BEMODIFY" };
            DataTable dtCommon = BaseData.Get(l_s, category);
            DataRow[] dr = dtCommon.Select("IS_FLAG='1'");
            if (dr.Count() > 0)
            {

                if (Convert.ToBoolean(dr[0]["BEMODIFY"].ToString()))
                {
                    this.teUserPassword.Enabled = true;
                    this.teUserName.Enabled = true;
                    this.teDestPathFormat.Enabled = true;
                }
                else
                {
                    this.teUserName.Text = dr[0]["NAME"].ToString();
                    this.teUserPassword.Text = dr[0]["PASSWORD"].ToString();
                    this.teDestPathFormat.Text ="{0:yyyy年}/{0:M月}/{0:yyyy-M-d}";
                }
            }
        }
        /// <summary>
        /// 不保存配置数据，关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 保存配置数据，关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string srcRootPath = this.teSrcRootPath.Text.Trim();
            string srcPathFormat = this.teSrcPathFormat.Text.Trim();
            string destRootPath = this.teDestRootPath.Text.Trim();
            string destPathFormat = this.teDestPathFormat.Text.Trim();
            string fileExistension = this.teFileExistention.Text.Trim();
            string userName = this.teUserName.Text.Trim();
            string userPassword = this.teUserPassword.Text.Trim();

            if (!string.IsNullOrEmpty(srcRootPath) && !System.IO.Directory.Exists(srcRootPath))
            {
                //MessageService.ShowMessage(string.Format("目录[{0}]不存在，请确认。", srcRootPath), "提示");
                MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.UploadPictureConfigDialog.msg.0002}"), srcRootPath), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }

            if (this._pictureType == "EL")
            {
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_SOURCE_ROOT_PATH, srcRootPath);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_SOURCE_PATH_FORMAT, srcPathFormat);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_DEST_ROOT_PATH, destRootPath);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_DEST_PATH_FORMAT, destPathFormat);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_EXISTENSION, fileExistension);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_USER_NAME, userName);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_USER_PASSWORD, userPassword);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_IS_DELETE_LOCAL_FILE, this.chkDeleteLocalFile.Checked);
                PropertyService.Set(PROPERTY_FIELDS.EL_PICTURE_IS_SEMIMANUFACTURES, this.chkSemimanufactures.Checked);
            }
            else if (this._pictureType == "IV")
            {
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_SOURCE_ROOT_PATH, srcRootPath);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_SOURCE_PATH_FORMAT, srcPathFormat);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_DEST_ROOT_PATH, destRootPath);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_DEST_PATH_FORMAT, destPathFormat);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_EXISTENSION, fileExistension);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_USER_NAME, userName);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_USER_PASSWORD, userPassword);
                PropertyService.Set(PROPERTY_FIELDS.IV_PICTURE_IS_DELETE_LOCAL_FILE, this.chkDeleteLocalFile.Checked);
            }
            else
            {
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_SOURCE_ROOT_PATH, srcRootPath);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_SOURCE_PATH_FORMAT, srcPathFormat);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_DEST_ROOT_PATH, destRootPath);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_DEST_PATH_FORMAT, destPathFormat);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_EXISTENSION, fileExistension);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_USER_NAME, userName);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_USER_PASSWORD, userPassword);
                PropertyService.Set(PROPERTY_FIELDS.ELNG_PICTURE_IS_DELETE_LOCAL_FILE, this.chkDeleteLocalFile.Checked);
            }

            PropertyService.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void sBtnSrcRootPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            string strPath = string.Empty;
            string strPathFrist = string.Empty;
            string strPathEnd = string.Empty;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                strPath = folder.SelectedPath;
                int i = strPath.LastIndexOf("\\");
                //判断根目录是否含有日期文件，如有添加格式化字符串，反之在格式化字符串中添加最后一个节点文件名
                bool isdate = false;
                try
                {
                    DateTime dt;
                    string str = strPath.Substring(i + 1, strPath.Length - 1 - i);
                    isdate = DateTime.TryParseExact(str, "yyyyMMdd", new CultureInfo("zh-cn"), DateTimeStyles.None, out dt);
                

                }
                catch
                {
                    isdate = false;
                }


                if (isdate)
                {
                    strPath = strPath.Substring(0, i);
                    i = strPath.LastIndexOf("\\");
                    strPathFrist = strPath;
                    strPathEnd = "{0:yyyyMMdd}";


                }
                else
                {
                    strPathFrist = strPath.Substring(0, i + 1);
                    strPathEnd = strPath.Substring(i + 1, strPath.Length - 1 - i);
                }
              
                teSrcRootPath.Text = strPathFrist;
                teSrcPathFormat.Text = strPathEnd;




            }
        }

        private void luDestRootPath_EditValueChanged(object sender, EventArgs e)
        {
            //if (luDestRootPath.EditValue.ToString()=="自定义")
            //{
            //    teDestRootPath.Enabled = true;
            //}
            //else
            //{
            teDestRootPath.Text = luDestRootPath.EditValue.ToString();
            //teDestRootPath.Enabled = false;
            //}

        }

        private void luFileExistention_EditValueChanged(object sender, EventArgs e)
        {
            //if (luFileExistention.EditValue.ToString() == "自定义")
            //{
            //    teFileExistention.Enabled = true;
            //}
            //else
            //{
            teFileExistention.Text = luFileExistention.EditValue.ToString();
            //    teFileExistention.Enabled = false;
            //}
        }
    }
}
