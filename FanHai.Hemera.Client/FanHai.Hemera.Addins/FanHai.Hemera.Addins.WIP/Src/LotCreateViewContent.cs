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
    public class LotCreateViewContent : AbstractViewContent
    {
        Control control = null; //用于显示批次创建界面的控件对象。
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
        /// 是否是批量创建批次。
        /// </summary>
        public bool IsBatch
        {
            get;
            private set;
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
        public LotCreateViewContent(LotCreateDetailModel model, bool isBatch)
            : base()
        {
            this.IsBatch = isBatch;
            if (isBatch)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreateViewContent.Title01}");//"创建生产批次";   //视图标题。
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreateViewContent.Title02}");//"创建补片批次";   //视图标题。
            }
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建批次创建的控件对象
            LotCreate ctrl = new LotCreate(model, isBatch, this);
            ctrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
