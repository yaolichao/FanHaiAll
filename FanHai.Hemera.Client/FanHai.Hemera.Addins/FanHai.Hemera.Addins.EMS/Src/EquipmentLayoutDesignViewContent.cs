using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentLayoutDesignViewContent : AbstractViewContent
    {
        private Control control = null;
        private EquipmentLayoutDesignCtrl LayoutDesignCtrl;

        public override Control Control
        {
            get
            {
                return control;
            }
        }

        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        public override void Dispose()
        {
            control.Dispose();

            base.Dispose();
        }

        public EquipmentLayoutDesignViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.title}");//设备看板设计

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            LayoutDesignCtrl = new EquipmentLayoutDesignCtrl();

            LayoutDesignCtrl.Dock = DockStyle.Fill;

            panel.Controls.Add(LayoutDesignCtrl);

            this.control = panel;
        }
    }
}
