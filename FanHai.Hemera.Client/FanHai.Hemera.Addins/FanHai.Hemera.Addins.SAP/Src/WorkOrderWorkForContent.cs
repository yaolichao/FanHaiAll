using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    class WorkOrderWorkForContent : AbstractViewContent
    {
        //define control
        Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
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
            control = null;
        }

        public WorkOrderWorkForContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WorkOrderWorkForContent.Title01}");//"工单报工";

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            WorkOrderWorkForCtrl workOrderWorkForCtrl = new WorkOrderWorkForCtrl();

            workOrderWorkForCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(workOrderWorkForCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
