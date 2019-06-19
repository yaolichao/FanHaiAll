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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
#endregion

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示代码管理的视图类。
    /// </summary>
    public class ReasonCodeCrtlViewContent:AbstractViewContent
    {
        private ReasonCodeCtrl reasonCodeCtrl = null;
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
        public ReasonCodeCrtlViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCode.Name}");
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            reasonCodeCtrl = new ReasonCodeCtrl();
            reasonCodeCtrl.CtrlState = ControlState.Empty;

            reasonCodeCtrl.afterStateChanged += new ReasonCodeCtrl.AfterStateChanged(OnAfterStateChange);

            reasonCodeCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(reasonCodeCtrl);
            //set panel to view content
            this.control = panel;
        }

        private void OnAfterStateChange(ControlState state)
        {
            if (state == ControlState.New)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCode.Name}") + "-" +
                    StringParser.Parse("${res:Global.New}");
            }
            else if (state == ControlState.Edit)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCode.Name}") + "-" +
                    StringParser.Parse("${res:Global.Update}");
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCode.Name}");
            }
        }
    }
}
