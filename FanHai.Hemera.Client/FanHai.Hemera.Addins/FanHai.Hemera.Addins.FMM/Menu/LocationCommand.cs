using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示区域管理的菜单命令类。通过该类显示区域管理的窗口界面。
    /// </summary>
    public class LocationCommand:AbstractMenuCommand
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
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationViewContent.ViewContentPartTitle}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }

            }
            //创建新的视图对象，并显示该视图界面。  
            LocationViewContent view = new LocationViewContent("",null);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }

    /// <summary>
    /// 表示班次管理的菜单命令类。通过该类显示班次管理的窗口界面。
    /// </summary>
    public class ScheduleCommand : AbstractMenuCommand
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
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }

            }
            //创建新的视图对象，并显示该视图界面。  
            ScheduleViewContent view = new ScheduleViewContent("",new Schedule());
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 表示排班管理的菜单命令类。通过该类显示排班管理的窗口界面。
    /// </summary>
    public class ShiftManagementCommand : AbstractMenuCommand
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
                if (viewContent.TitleName =="排班管理")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }

            }
            //创建新的视图对象，并显示该视图界面。  
            ShiftViewContent view = new ShiftViewContent(new Shift());            
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
