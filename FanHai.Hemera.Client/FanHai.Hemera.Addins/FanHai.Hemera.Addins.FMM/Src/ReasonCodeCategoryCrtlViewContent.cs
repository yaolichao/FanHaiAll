//----------------------------------------------------------------------------------
// Copyright (c) FanHai
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
    /// 表示代码组管理的视图类。
    /// </summary>
    public class ReasonCodeCategoryCtrlViewContent :AbstractViewContent
    {
         private ReasonCodeCategoryCtrl reasonCodeCategoryCtrl = null;
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
        public ReasonCodeCategoryCtrlViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCategory.Name}");
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            reasonCodeCategoryCtrl = new ReasonCodeCategoryCtrl();
            reasonCodeCategoryCtrl.CtrlState = ControlState.Empty;

            reasonCodeCategoryCtrl.afterStateChanged += new ReasonCodeCategoryCtrl.AfterStateChanged(OnAfterStateChange);

            reasonCodeCategoryCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(reasonCodeCategoryCtrl);
            //set panel to view content
            this.control = panel;
        }

        private void OnAfterStateChange(ControlState state)
        {
            if (state == ControlState.New)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCategory.Name}") + "-" + 
                    StringParser.Parse("${res:Global.New}");
            }
            else if (state == ControlState.Edit)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCategory.Name}") + "-" +
                    StringParser.Parse("${res:Global.Update}");
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCategory.Name}");
            }
        }
    }
}
