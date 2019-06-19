using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    class LotExceptionContent : AbstractViewContent
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


        public LotExceptionContent()
            : base()
        {

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionContent.Title01}");//"不良信息输入";           
           
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            LotExceptionProcess lotNumberPrint = new LotExceptionProcess();
            lotNumberPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotNumberPrint);
            //set panel to view content
            this.control = panel;
           
        }
    }
}
