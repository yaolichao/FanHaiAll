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
using FanHai.Hemera.Addins.BasicData.Gui;

namespace FanHai.Hemera.Addins.BasicData
{
    public class LineSettingViewContent : AbstractViewContent
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

        public LineSettingViewContent()
            : base()
        {
            //this.TitleName = StringParser.Parse("线别维护");
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.LineSetting.title}");//线别维护
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            LineSetting lineSetting = new LineSetting();

            lineSetting.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(lineSetting);

            //set panel to view content
            this.control = panel;
        }
    }
}
