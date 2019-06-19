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
    /// 表示原材料领料的视图界面。
    /// </summary>
    public class MaterialRequisitionViewContent : AbstractViewContent
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

        public MaterialRequisitionViewContent()
            : base()
        {
            //this.TitleName = "领料单-领料(仓库->线上仓)";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.name}");
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            MaterialRequisitionCtrl ctrl = new MaterialRequisitionCtrl();
            ctrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
