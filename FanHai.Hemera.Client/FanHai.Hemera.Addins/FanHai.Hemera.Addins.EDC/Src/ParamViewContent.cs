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
    /// 表示参数管理的视图类。
    /// </summary>
    public class ParamViewContent : AbstractViewContent
    {
        private ParamCtl paramCtl = null;
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
        /// <param name="param">表示参数管理信息的实体对象</param>
        public ParamViewContent(Param param)
            : base()
        {
            if (null != param && param.ParamName.Length > 0)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamViewContent}") + "_" + param.ParamName;
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamViewContent}");
            }

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            paramCtl = new ParamCtl(param);

            paramCtl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(paramCtl);
            //set panel to view content
            this.control = panel;
        }

    }
}
