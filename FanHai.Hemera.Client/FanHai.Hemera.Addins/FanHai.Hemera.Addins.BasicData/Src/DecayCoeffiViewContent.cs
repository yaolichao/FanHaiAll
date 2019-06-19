/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Hemera.Addins.BasicData;

namespace FanHai.Hemera.Addins.BasicData
{
    public class DecayCoeffiViewContent:AbstractViewContent
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

        public DecayCoeffiViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.DecayCoeffi.title}");//衰减系数设置

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            DecayCoeffi abNormalRules = new DecayCoeffi();

            abNormalRules.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(abNormalRules);

            //set panel to view content
            this.control = panel;
        }
    }
}
