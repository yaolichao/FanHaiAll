using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;
using FanHai.Hemera.Utils.Entities;


namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示工作站作业(简单)的视图类。
    /// </summary>
    public class LotDispathViewContent : AbstractViewContent
    {
        Control control = null;    //用于显示工作站作业(简单)界面的控件对象。
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
        /// <param name="model">工作站作业的参数数据。</param>
        public LotDispathViewContent(LotDispatchDetailModel model)
            : base()
        {

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.lblApplicationTitle}");//视图标题---过站作业
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.AutoScroll = true;
            LotDispatch wk = new LotDispatch(model,this);
            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }
}
