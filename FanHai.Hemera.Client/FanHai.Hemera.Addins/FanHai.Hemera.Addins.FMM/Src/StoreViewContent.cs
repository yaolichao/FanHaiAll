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
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.FMM
{
    public class StoreViewContent:AbstractViewContent
    {
         private StoreCtrl storeCtrl = null;
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
        /// read only
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return false;
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

        public StoreViewContent()
            : base()
        {
           
            this.TitleName =StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreManagement.Name}");
           
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            storeCtrl = new StoreCtrl();
            storeCtrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(storeCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
