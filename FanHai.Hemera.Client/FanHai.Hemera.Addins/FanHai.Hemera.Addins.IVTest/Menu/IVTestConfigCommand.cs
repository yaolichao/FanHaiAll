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
    /// 启动IV测试配置对话框。
    /// </summary>
    public class IVTestConfigCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            IVTestConfigDialog dlgIVTestConfig = new IVTestConfigDialog();
            if (DialogResult.OK == dlgIVTestConfig.ShowDialog())
            {
                StartTranIVTestCommand cmd = new StartTranIVTestCommand();
                cmd.Run();
            }
            dlgIVTestConfig.Dispose();
            dlgIVTestConfig = null;
        }
    }
}
