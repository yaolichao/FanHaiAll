﻿//----------------------------------------------------------------------------------
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
using System.Data;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 领料清单。
    /// </summary>
    public class ReceiveMaterialListViewContent : AbstractViewContent
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

        public ReceiveMaterialListViewContent()
            : base()
        {
            //this.TitleName = "领料清单";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReceiveMaterialListCtrl.Title}");
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            ReceiveMaterialListCtrl ctrl = new ReceiveMaterialListCtrl();
            ctrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
