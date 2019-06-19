//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang            2014-12-1             新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 工单转换标志枚举。
    /// </summary>
    public enum ExchangeFlag
    {
        /// <summary>
        /// 发料
        /// </summary>
        Sending,
        /// <summary>
        /// 退料
        /// </summary>
        SendingBack
    }
    /// <summary>
    /// 原材料发料的菜单命令。
    /// </summary>
    public class SendingMaterialCommand : AbstractMenuCommand
    {
        ///// <summary>
        ///// 执行命令。
        ///// </summary>
        //public override void Run()
        //{
        //    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
        //    {
        //        if (viewContent is SendingMaterialContent)
        //        {
        //            viewContent.WorkbenchWindow.SelectWindow();
        //            return;
        //        }
        //    }
        //    WorkbenchSingleton.Workbench.ShowView(new SendingMaterialContent());
        //}

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
                SendingMaterialContent openView = viewContent as SendingMaterialContent;
                if (openView != null && openView.exchangeType == ExchangeFlag.Sending)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            SendingMaterialContent view = new SendingMaterialContent(ExchangeFlag.Sending);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }

    /// <summary>
    /// 原材料退料料的菜单命令。
    /// </summary>
    public class SendingBackMaterialCommand : AbstractMenuCommand
    {
        ///// <summary>
        ///// 执行命令。
        ///// </summary>
        //public override void Run()
        //{
        //    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
        //    {
        //        if (viewContent is SendingMaterialContent)
        //        {
        //            viewContent.WorkbenchWindow.SelectWindow();
        //            return;
        //        }
        //    }
        //    WorkbenchSingleton.Workbench.ShowView(new SendingMaterialContent());
        //}


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
                SendingMaterialContent openView = viewContent as SendingMaterialContent;
                if (openView != null && openView.exchangeType == ExchangeFlag.SendingBack)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            SendingMaterialContent view = new SendingMaterialContent(ExchangeFlag.SendingBack);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
