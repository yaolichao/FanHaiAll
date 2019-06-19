using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Framework.Gui;
using System.Windows.Forms;
using Astronergy.Addins.WMS.Gui;

namespace Astronergy.Addins.WMS.Src
{
    class OutBoundManagementVeiwContent : AbstractViewContent
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
        public OutBoundManagementVeiwContent()
        {
            this.TitleName = "出货管理"; //标题
            Panel Pal = new Panel();//创建一个Panel对象Pal
            Pal.Dock = DockStyle.Fill; // 设置 Pal的DockStyle为Fill
            Pal.BorderStyle = BorderStyle.FixedSingle;
            OutBoundManagementControl Ctrl = new OutBoundManagementControl(this);
            Ctrl.Dock = DockStyle.Fill;
            Pal.Controls.Add(Ctrl);
            this.control = Ctrl;
        }
    }
}
