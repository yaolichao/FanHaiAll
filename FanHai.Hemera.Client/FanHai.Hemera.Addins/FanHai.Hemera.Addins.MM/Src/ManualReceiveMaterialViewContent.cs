//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 张振东               2012-05-11            新建，用于手工输入来料数据。
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示领料的视图界面。
    /// </summary>
    public class ManualReceiveMaterialViewContent : AbstractViewContent
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

        public ManualReceiveMaterialViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ManualReceiveMaterialCtrl.name}"); ;//"领料"

            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            ManualReceiveMaterialCtrl ctrl = new ManualReceiveMaterialCtrl();
            ctrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
