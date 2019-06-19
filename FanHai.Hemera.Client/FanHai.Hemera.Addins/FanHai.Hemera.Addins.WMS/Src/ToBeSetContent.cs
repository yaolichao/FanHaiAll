using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.WMS
{
    public class ToBeSetContent : AbstractViewContent
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
        public ToBeSetContent()
              : base()
        {
            //this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrlContent.Title01}");//"入库单生成";             //视图标题。
            this.TitleName = "待定";//"入库单生成";             //视图标题。
            Panel oP = new Panel();
            oP.Dock = DockStyle.Fill;
            oP.BorderStyle = BorderStyle.FixedSingle;
            ToBeSet ctrl = new ToBeSet();
            ctrl.Dock = DockStyle.Fill;
            oP.Controls.Add(ctrl);
            this.control = oP;
        }
    }
}
