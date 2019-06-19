using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;


namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次查询的视图类。
    /// </summary>
    public class LotSearchViewContent : AbstractViewContent
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
        /// 视图内容是否仅允许查看（不能被保存）。
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
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
        /// 构造函数。
        /// </summary>
        public LotSearchViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotSearchViewContent.Title}");
            this.TabPageText = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotSearchViewContent.Title}");
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            LotQueryCtrl wk = new LotQueryCtrl(this);
            wk.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(wk);
            //set panel to view content
            this.control = panel;
        }
    }
}
