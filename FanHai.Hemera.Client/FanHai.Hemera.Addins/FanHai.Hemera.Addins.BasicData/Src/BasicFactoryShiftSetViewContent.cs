/*
<FileInfo>
  <Author>Rayna Liu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Framework.Gui;
using System.Windows.Forms;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Addins.BasicData;

namespace SolarViewer.Hemera.Addins.BasicData
{
    public class BasicFactoryShiftSetViewContent : AbstractViewContent
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

        public BasicFactoryShiftSetViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("生产排班维护");

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            BasicFactoryShiftSet abNormalRules = new BasicFactoryShiftSet();

            abNormalRules.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(abNormalRules);

            //set panel to view content
            this.control = panel;
        }
    }
}
