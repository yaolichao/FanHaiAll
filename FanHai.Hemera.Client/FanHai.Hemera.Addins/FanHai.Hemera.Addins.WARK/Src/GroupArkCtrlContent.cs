﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Framework.Gui;
using System.Windows.Forms;

namespace SolarViewer.Hemera.Addins.WARK
{
    public class GroupArkCtrlContent : AbstractViewContent
    {
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
            control = null;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GroupArkCtrlContent()
            : base()
        {
            this.TitleName = "组柜_管理";             //视图标题。
            Panel oP = new Panel();
            oP.Dock = DockStyle.Fill;
            oP.BorderStyle = BorderStyle.FixedSingle;
            GroupArkCtrl ctrl = new GroupArkCtrl();
            ctrl.Dock = DockStyle.Fill;
            oP.Controls.Add(ctrl);
            this.control = oP;
        }
    }
}
