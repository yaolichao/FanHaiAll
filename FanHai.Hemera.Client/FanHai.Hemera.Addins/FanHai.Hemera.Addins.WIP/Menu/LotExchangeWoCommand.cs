using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 工单转换标志枚举。
    /// </summary>
    public enum ExchangeWoFlag
    {
        /// <summary>
        /// 返工工单作业
        /// </summary>
        Repair,
        /// <summary>
        /// 批次转工单作业
        /// </summary>
        Exchange,
        /// <summary>
        /// 批量转工单
        /// </summary>
        MultiExchange
    }
    /// <summary>
    /// 转工单命令类。
    /// </summary>
    public class LotExchangeWoCommand : AbstractMenuCommand
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
                LotExchangeWoViewContent openView = viewContent as LotExchangeWoViewContent;
                if (openView != null && openView.exchangeType == ExchangeWoFlag.Exchange)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotExchangeWoViewContent view = new LotExchangeWoViewContent(ExchangeWoFlag.Exchange);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }

    /// <summary>
    /// 返工单命令类。
    /// </summary>
    public class LotRepairExchangeWoCommand : AbstractMenuCommand
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
                LotExchangeWoViewContent openView = viewContent as LotExchangeWoViewContent;
                if (openView != null && openView.exchangeType == ExchangeWoFlag.Repair)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotExchangeWoViewContent view = new LotExchangeWoViewContent(ExchangeWoFlag.Repair);

            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }

    /// <summary>
    /// 批量转工单命令类。
    /// </summary>
    public class LotMultiExchangeWoCommand : AbstractMenuCommand
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
                LotExchangeWoViewContent openView = viewContent as LotExchangeWoViewContent;
                if (openView != null && openView.exchangeType == ExchangeWoFlag.MultiExchange)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotExchangeWoViewContent view = new LotExchangeWoViewContent(ExchangeWoFlag.MultiExchange);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
