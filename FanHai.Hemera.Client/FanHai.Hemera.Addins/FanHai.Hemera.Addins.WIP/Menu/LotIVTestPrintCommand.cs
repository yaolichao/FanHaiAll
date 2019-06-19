using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    public class LotIVTestPrintCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            // Switch to previously opened view.
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                LotIVTestPrintViewContent openView = viewContent as LotIVTestPrintViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotIVTestPrintViewContent view = new LotIVTestPrintViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    public class LotIVTestPrintCommand2 : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            // Switch to previously opened view.
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                LotIVTestPrintViewContent2 openView = viewContent as LotIVTestPrintViewContent2;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotIVTestPrintViewContent2 view = new LotIVTestPrintViewContent2();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 包装入库检验操作类
    /// </summary>
    public class LotDispatchForPalletCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            // Switch to previously opened view.
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                LotDispatchForPalletViewContent openView = viewContent as LotDispatchForPalletViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotDispatchForPalletViewContent view = new LotDispatchForPalletViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    ///终检数据查询操作类
    /// </summary>
    public class LotCustCheckQueryCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            // Switch to previously opened view.
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                LotCustCheckQueryViewContent openView = viewContent as LotCustCheckQueryViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotCustCheckQueryViewContent view = new LotCustCheckQueryViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }

    /// <summary>
    ///包装出托类作业
    /// </summary>
    public class PalletWholeOutCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            // Switch to previously opened view.
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                PalletWholeOutViewContent openView = viewContent as PalletWholeOutViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            PalletWholeOutViewContent view = new PalletWholeOutViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
