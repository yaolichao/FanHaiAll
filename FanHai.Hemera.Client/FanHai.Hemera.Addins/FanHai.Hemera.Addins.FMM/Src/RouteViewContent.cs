//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            添加注释 
// =================================================================================

#region using
using System;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
#endregion

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示途程管理的视图类。
    /// </summary>
    public class RouteViewContent : AbstractViewContent
    {
        //define control
        Control control = null;

        /// <summary>
        /// 控件对象，用于在应用程序平台上显示可视化的视图界面。
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// 释放使用的所有资源。
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="route">表示途程信息的实体对象。</param>
        public RouteViewContent(RouteEntity route)
            : base()
        {
            if (null != route && route.RouteName.Length > 0)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}") 
                               + "_" + route.RouteName + "." + route.RouteVersion;
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}");
            }

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            RouteCtrl routeCtrl = new RouteCtrl(route);

            routeCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(routeCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}

