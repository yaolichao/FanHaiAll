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
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
#endregion

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 表示抽样点设置的视图类。
    /// </summary>
    public class EDCPointCtlViewContent : AbstractViewContent
    {
        //define control
        Control control = null;   //用于显示抽样点设置界面的控件对象。

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
        /// 视图内容是否仅允许查看（不能被保存）。
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return false;
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
        /// 构造函数
        /// </summary>
        public EDCPointCtlViewContent()
            : base()
        {
            this.TitleName = "抽检点设置";  //视图标题。

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建抽样点设置控件对象。
            EDCPointCtl edcManageCtl = new EDCPointCtl();
            edcManageCtl.Dock = DockStyle.Fill;
            //将抽样点设置控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(edcManageCtl);
            //set panel to view content
            this.control = panel;
        }
    }
}
