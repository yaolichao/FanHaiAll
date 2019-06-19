using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentLayoutViewContent : AbstractViewContent
    {
        private Control control = null;
        private EquipmentLayoutCtrl equipmentLayout = null;

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
        public EquipmentLayoutViewContent() : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.title}");//设备看板
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            equipmentLayout = new EquipmentLayoutCtrl();
            equipmentLayout.Dock = DockStyle.Fill;
            panel.Controls.Add(equipmentLayout);
            this.control = panel;
        }
    }
}
