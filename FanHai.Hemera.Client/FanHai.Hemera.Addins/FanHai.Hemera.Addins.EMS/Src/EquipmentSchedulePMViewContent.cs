using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentSchedulePMViewContent : AbstractViewContent
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

        public EquipmentSchedulePMViewContent()
            : base()
        {
            this.TitleName = "设备计划PM";

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            EquipmentSchedulePMCtrl equipmentSchedulePMCtrl = new EquipmentSchedulePMCtrl();
            equipmentSchedulePMCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(equipmentSchedulePMCtrl);

            //set panel to view content
            this.control = panel;
        }
    }
}
