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
    /// 表示采样管理的视图类。
    /// </summary>
    public class SampViewContent : AbstractViewContent
    {
        private SampManageCtl sampManageCtl = null;
        //define control
        Control control = null;//用于显示视图界面的控件对象。

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
        /// <param name="sp">表示采样管理信息的实体对象。</param>
        public SampViewContent(SampManage sp)
            : base()
        {
            if (null != sp && sp.SpName.Length > 0)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampViewContent}") + "_" + sp.SpName;
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampViewContent}");
            }

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            sampManageCtl = new SampManageCtl(sp);

            sampManageCtl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(sampManageCtl);
            //set panel to view content
            this.control = panel;
        }

    }
}
