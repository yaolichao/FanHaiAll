// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 庞超                2012-03-14             新建 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;


namespace FanHai.Hemera.Addins.MM
{
    class UseMaterialListCtrlContent : AbstractViewContent
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

        public UseMaterialListCtrlContent()
            : base()
        {
            this.TitleName = "材料耗用清单";

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            UseMaterialListCtrl useMaterialListCtrl = new UseMaterialListCtrl();

            useMaterialListCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(useMaterialListCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
