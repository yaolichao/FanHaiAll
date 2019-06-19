using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示代码管理的菜单命令类。通过该类显示代码管理的窗口界面。
    /// </summary>
    public class ReasonCodeCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                ReasonCodeCrtlViewContent openView = viewContent as ReasonCodeCrtlViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            ReasonCodeCrtlViewContent view = new ReasonCodeCrtlViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 表示代码组管理的菜单命令类。通过该类显示代码组管理的窗口界面。
    /// </summary>
    public class ReasonCodeCategoryCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                ReasonCodeCategoryCtrlViewContent openView = viewContent as ReasonCodeCategoryCtrlViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            ReasonCodeCategoryCtrlViewContent view = new ReasonCodeCategoryCtrlViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示线别管理的菜单命令类。通过该类显示线别管理的窗口界面。
    /// </summary>
    public class LineCommandEx :  AbstractMenuCommand
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
                LineConfViewContent openView = viewContent as LineConfViewContent;
                if (openView != null && StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderManagement.Name}") == openView.TitleName)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LineConfViewContent view = new LineConfViewContent(new UdaEntity());
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
