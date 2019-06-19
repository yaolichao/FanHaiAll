using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Framework.Gui;
using System.Windows.Forms;

namespace Astronergy.Addins.WMS.Src
{
    public class OutboundQCViewContent : AbstractViewContent
    {
        Control control = null;      
        public override System.Windows.Forms.Control Control
        {   
            get
            {
                return control;
            }
        }
        //构造函数
        public OutboundQCViewContent()
        {
            this.TitleName = "出货检验"; //标题
            Panel Pal = new Panel();//创建一个Panel对象Pal
            Pal.Dock = DockStyle.Fill; // 设置 Pal的DockStyle为Fill
            Pal.BorderStyle = BorderStyle.FixedSingle;
            OutboundQCControl Ctrl = new OutboundQCControl(this);
            Ctrl.Dock = DockStyle.Fill;
            Pal.Controls.Add(Ctrl);
            this.control = Ctrl;
        }
    }
}
