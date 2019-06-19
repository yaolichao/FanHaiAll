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
using System.Windows.Forms;

using SolarViewer.Gui.Core;
using SolarViewer.Gui.Framework.Gui;

namespace SolarViewer.Hemera.Addins.SPC
{
    public class SpcParameterViewContent : AbstractViewContent
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

        public SpcParameterViewContent()
            : base()
        {
            this.TitleName="Spc 参数设置";

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            SpcParameterCtrl spcParameterCtrl = new SpcParameterCtrl();

            spcParameterCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(spcParameterCtrl);

            //set panel to view content
            this.control = panel;
        }
    }
}
