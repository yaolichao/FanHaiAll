using System;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.StartPage
{
    public class StartPageViewContent : AbstractViewContent
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


        public StartPageViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:StartPage.StartPageContentName}");
            this.TabPageText = StringParser.Parse("${res:StartPage.StartPageContentName}");
            //ADD CONTROL 
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //set panel to view content
            this.control = panel;
        }
    }
}


