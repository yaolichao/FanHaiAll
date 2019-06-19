using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.WIP.Gui;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 组件序列号打印 -chao.pang
    /// </summary>
    public class LotNumPrintContent : AbstractViewContent
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


        public LotNumPrintContent()
            : base()
        {


            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrintContent.Title01}");//"序列号打印";
           
           
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            LotNumPrint lotNumPrint = new LotNumPrint();
            lotNumPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotNumPrint);
            //set panel to view content
            this.control = panel;
           
        }
    }
}
