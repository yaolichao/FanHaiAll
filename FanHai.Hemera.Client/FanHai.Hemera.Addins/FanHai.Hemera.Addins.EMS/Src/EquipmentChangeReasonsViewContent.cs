using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Addins.EMS;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentChangeReasonsViewContent : AbstractViewContent
    {
        private Control control = null;
        private EquipmentChangeReasons equipmentChangeReasons;

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

        public EquipmentChangeReasonsViewContent() : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.Name}");

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            equipmentChangeReasons = new EquipmentChangeReasons();

            equipmentChangeReasons.Dock = DockStyle.Fill;

            panel.Controls.Add(equipmentChangeReasons);

            this.control = panel;
        }
    }
}
