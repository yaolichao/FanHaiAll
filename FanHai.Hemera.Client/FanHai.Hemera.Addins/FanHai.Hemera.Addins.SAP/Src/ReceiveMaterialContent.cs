//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  yongbing.yang
//----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    public class ReceiveMaterialContent : AbstractViewContent
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
            control = null;
        }

        public ReceiveMaterialContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ReceiveMaterialContent.Title01}");//"来料接收";

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            ReceiveMaterialCtrl ReceiveMaterialCtrl = new ReceiveMaterialCtrl();

            ReceiveMaterialCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(ReceiveMaterialCtrl);

            //set panel to view content
            this.control = panel;
        }
    }
}
