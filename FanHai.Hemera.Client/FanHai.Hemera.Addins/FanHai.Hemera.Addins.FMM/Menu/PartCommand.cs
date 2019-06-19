using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示产品料号维护的菜单命令类。通过该类显示产品料号维护的窗口界面。
    /// </summary>
    public class PartCommand : AbstractMenuCommand
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
                if (viewContent is PartViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。 
            PartViewContent view = new PartViewContent(new Part());
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
