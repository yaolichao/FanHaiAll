using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentsViewContent : AbstractViewContent
    {
        private Control control = null;
        private Equipments equipments;

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

        public EquipmentsViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.Name}");

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            equipments = new Equipments();

            equipments.Dock = DockStyle.Fill;

            panel.Controls.Add(equipments);

            this.control = panel;
        }
    }
}
