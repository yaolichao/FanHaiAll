
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.BasicData;

namespace FanHai.Hemera.Addins.BasicData
{
    /// <summary>
    /// 标签铭牌设置类。
    /// </summary>
    public class PrintLabelCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //string title = "标签铭牌设置";
            string title = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.PrintLabelCtrl.title01}");
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is PrintLabelViewContent && viewContent.TitleName==title)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new PrintLabelViewContent(title, false));
        }
    }

    /// <summary>
    /// 标签铭牌管理类。
    /// </summary>
    public class PrintLabelManageCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //string title = "标签铭牌管理";
            string title = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.PrintLabelCtrl.title02}");
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is PrintLabelViewContent && viewContent.TitleName==title)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new PrintLabelViewContent(title, true));
        }
    }
}
