using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using System.Threading;
using FanHai.Hemera.Share.Constants;
using System.IO;
using System.Data.OleDb;
using System.Data;
using FanHai.Hemera.Utils.Entities;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// 上传EL图片的配置命令类。
    /// </summary>
    public class UploadELPictureConfigCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            UploadPictureConfigDialog dlg = new UploadPictureConfigDialog("EL");
            if (DialogResult.OK == dlg.ShowDialog())
            {
                if (UploadELPictureCommand.CommandObject == null)
                {
                    UploadELPictureCommand cmd = new UploadELPictureCommand();
                    cmd.Run();
                }
                else
                {
                    UploadELPictureCommand.CommandObject.Run();
                }
            }
            dlg.Dispose();
            dlg = null;
        }
    }

    /// <summary>
    /// 上传IV图片的配置命令类。
    /// </summary>
    public class UploadIVPictureConfigCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            UploadPictureConfigDialog dlg = new UploadPictureConfigDialog("IV");
            if (DialogResult.OK == dlg.ShowDialog())
            {
                if (UploadIVPictureCommand.CommandObject == null)
                {
                    UploadIVPictureCommand cmd = new UploadIVPictureCommand();
                    cmd.Run();
                }
                else
                {
                    UploadIVPictureCommand.CommandObject.Run();
                }
            }
            dlg.Dispose();
            dlg = null;
        }
    }


    /// <summary>
    /// 上传ELNG图片的配置命令类。
    /// </summary>
    public class UploadELNGPictureConfigCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            UploadPictureConfigDialog dlg = new UploadPictureConfigDialog("ELNG");
            if (DialogResult.OK == dlg.ShowDialog())
            {
                if (UploadELNGPictureCommand.CommandObject == null)
                {
                    UploadELNGPictureCommand cmd = new UploadELNGPictureCommand();
                    cmd.Run();
                }
                else
                {
                    UploadELNGPictureCommand.CommandObject.Run();
                }
            }
            dlg.Dispose();
            dlg = null;
        }
    }

}
