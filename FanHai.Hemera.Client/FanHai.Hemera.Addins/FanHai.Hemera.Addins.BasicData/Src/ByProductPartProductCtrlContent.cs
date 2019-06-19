using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    public class ByProductPartProductCtrlContent : AbstractViewContent
    {
        private ByProductPartProductCtrl byProductPartProductCtrl = null;
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
        public ByProductPartProductCtrlContent(string titleName)
            : base()
        {
            //this.TitleName = "主副产品管理";
            if (titleName != "")
            {
                this.TitleName = StringParser.Parse("主副产品管理") + "-" + titleName;
            }
            else
            {
                this.TitleName = StringParser.Parse("主副产品管理");
            }           
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            byProductPartProductCtrl = new ByProductPartProductCtrl();


            byProductPartProductCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(byProductPartProductCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
