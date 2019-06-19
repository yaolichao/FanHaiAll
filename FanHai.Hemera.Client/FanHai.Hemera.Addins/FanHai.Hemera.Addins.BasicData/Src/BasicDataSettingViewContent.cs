using FanHai.Gui.Framework.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.BasicData
{
    public class BasicDataSettingViewContent : AbstractViewContent
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

        public BasicDataSettingViewContent()
            : base()
        {

            this.TitleName = "参数设置";
 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            BasicDataSetting viewCommon = new BasicDataSetting();

            viewCommon.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(viewCommon);

            //set panel to view content
            this.control = panel;
        }
    }
}
