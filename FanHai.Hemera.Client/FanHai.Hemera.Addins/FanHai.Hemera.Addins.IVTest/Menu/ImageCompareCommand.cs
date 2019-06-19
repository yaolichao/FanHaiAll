using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// 表示图片比对的菜单命令类。通过该类显示图片比对的窗口界面。
    /// </summary>
    public class ImageCompareCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {            
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                ImageCompareViewContent openView = viewContent as ImageCompareViewContent;
                if (openView != null)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            ImageCompareViewContent view = new ImageCompareViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
