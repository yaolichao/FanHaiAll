using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentConditionPMViewContent : AbstractViewContent
    {
        private Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return this.control;
            }
        }

        /// <summary>
        /// IsViewOnly
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
        }

        public EquipmentConditionPMViewContent()
            : base()
        {
            this.TitleName = "设备条件PM";

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            EquipmentConditionPMCtrl equipmentConditionPMCtrl = new EquipmentConditionPMCtrl();
            equipmentConditionPMCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(equipmentConditionPMCtrl);

            //set panel to view content
            this.control = panel;
        }
    }
}
