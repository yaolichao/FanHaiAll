using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Addins.BasicData.Gui;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    class NameplateAutoPrintSettingViewContent : AbstractViewContent
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
            control = null;
        }


        public NameplateAutoPrintSettingViewContent()
            : base()
        {


            //this.TitleName = "铭牌自动打印设置";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.NameplateAutoPrintSetting.title}");

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            NameplateAutoPrintSetting lotIvTestPrint = new NameplateAutoPrintSetting();
            lotIvTestPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotIvTestPrint);
            //set panel to view content
            this.control = panel;

        }
    }
}
