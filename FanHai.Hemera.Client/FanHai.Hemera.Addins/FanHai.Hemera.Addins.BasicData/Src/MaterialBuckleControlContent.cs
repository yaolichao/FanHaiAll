﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    public class MaterialBuckleControlContent : AbstractViewContent
    {
        private MaterialBuckleControlCtrl materialBuckleControlCtrl = null;
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
        public MaterialBuckleControlContent()
            : base()
        {
            //this.TitleName = "原材料扣料配置";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0001}");//原材料扣料配置
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            materialBuckleControlCtrl = new MaterialBuckleControlCtrl();

            materialBuckleControlCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(materialBuckleControlCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
