using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentStatesViewContent : AbstractViewContent
    {
        private Control control = null;
        private EquipmentStates equipmentStates;

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

        public EquipmentStatesViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.Name}");

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            equipmentStates = new EquipmentStates();

            equipmentStates.Dock = DockStyle.Fill;

            panel.Controls.Add(equipmentStates);

            this.control = panel;
        }
    }
}
