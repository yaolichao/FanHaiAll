using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Core.WinForms;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Gui.Framework.Commands
{
    /// <summary>
    /// 隐藏Pad的菜单命令类。
    /// </summary>
    class HidePadCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //隐藏所有的PadContent
            foreach (PadDescriptor padDescriptor in WorkbenchSingleton.Workbench.PadContentCollection)
            {
                WorkbenchSingleton.Workbench.WorkbenchLayout.HidePad(padDescriptor);
            }
        }
    }
}

