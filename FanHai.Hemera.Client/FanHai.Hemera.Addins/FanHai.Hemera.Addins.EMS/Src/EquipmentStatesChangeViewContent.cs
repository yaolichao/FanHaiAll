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
    public class EquipmentStatesChangeViewContent : AbstractViewContent
    {
        private Control control = null;
        private EquipmentStatesChange equipmentStatesChange;

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

        public EquipmentStatesChangeViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStatesChange.Name}");

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            equipmentStatesChange = new EquipmentStatesChange();

            equipmentStatesChange.Dock = DockStyle.Fill;

            panel.Controls.Add(equipmentStatesChange);

            this.control = panel;
        }
    }
}
