//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  冯旭
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭               2012-02-16             新建 
// =================================================================================
using System;
using System.Windows.Forms;


using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.MM
{
    class HandOverTheWatchListContent : AbstractViewContent
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
        }

        public HandOverTheWatchListContent()
            : base()
        {
            this.TitleName = "工序交接班清单";

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            HandOverTheWatchCtrlList handOverTheWatchCtrl = new HandOverTheWatchCtrlList();

            handOverTheWatchCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(handOverTheWatchCtrl);
            //set panel to view content
            this.control = panel;
        }
       
    }
}
