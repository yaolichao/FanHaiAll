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
using System.Data;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 设备虚拟仓清单。
    /// </summary>
    public class EquipmentMatListContent : AbstractViewContent
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

        public EquipmentMatListContent()
            : base()
        {
            //this.TitleName = "设备材料看板";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EquipmentMatListCtrl.name}");
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            EquipmentMatListCtrl ctrl = new EquipmentMatListCtrl();
            ctrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
