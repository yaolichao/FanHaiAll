using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Addins.WMS.Gui;

namespace FanHai.Hemera.Addins.WMS.Src
{
    class PickOrderViewContent : AbstractViewContent
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
        public PickOrderViewContent()
            : base()
        {
            this.TitleName = "外向交货单拣配";             //视图标题。
            Panel oP = new Panel();
            oP.Dock = DockStyle.Fill;
            oP.BorderStyle = BorderStyle.FixedSingle;
            PickOrderCtrl ctrl = new PickOrderCtrl();
            ctrl.Dock = DockStyle.Fill;
            oP.Controls.Add(ctrl);
            this.control = oP;
        }


    }
}
