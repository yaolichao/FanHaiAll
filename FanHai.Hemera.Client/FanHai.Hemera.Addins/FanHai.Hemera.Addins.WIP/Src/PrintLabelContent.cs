using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.WIP;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示创建批次的视图类。
    /// </summary>
    public class PrintLabelContent : AbstractViewContent
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
        public PrintLabelContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PrintLabelContent.Title01}");//"标签打印";             //视图标题。
            Panel oP = new Panel();
            oP.Dock = DockStyle.Fill;
            oP.BorderStyle = BorderStyle.FixedSingle;
            PrintNameplateLabel ctrl = new PrintNameplateLabel();
            ctrl.Dock = DockStyle.Fill;
            oP.Controls.Add(ctrl);
            this.control = oP;
        }
    }
}
