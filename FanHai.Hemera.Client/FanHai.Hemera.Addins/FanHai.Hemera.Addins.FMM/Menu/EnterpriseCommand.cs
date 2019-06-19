
using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示工艺流程组管理的菜单命令类。通过该类显示工艺流程组管理的窗口界面。
    /// </summary>
    public class EnterpriseCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开该视图，则选中该视图显示，返回以结束该方法的运行。
                EnterpriseViewContent openView = viewContent as EnterpriseViewContent;
                if (openView != null && 
                    StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseViewContent.TitleName}") == openView.TitleName)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。 
            EnterpriseViewContent view = new EnterpriseViewContent(new EnterpriseEntity());
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
