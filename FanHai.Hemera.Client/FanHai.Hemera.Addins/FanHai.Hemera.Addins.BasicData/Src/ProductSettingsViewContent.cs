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
    public class ProductSettingsViewContent : AbstractViewContent
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

        public ProductSettingsViewContent()
            : base()
        {
            //this.TitleName = StringParser.Parse("产品设置");
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicProductSet.title}");//产品设置
            //define panel 
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            BasicProductSet viewCommon = new BasicProductSet();

            viewCommon.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(viewCommon);

            //set panel to view content
            this.control = panel;
        }
    }
}
