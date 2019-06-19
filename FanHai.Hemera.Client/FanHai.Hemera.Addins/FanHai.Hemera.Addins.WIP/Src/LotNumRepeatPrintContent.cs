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
    /// 组件序列号补打 -chao.pang
    /// </summary>
    public class LotNumRepeatPrintContent : AbstractViewContent
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


        public LotNumRepeatPrintContent()
            : base()
        {

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrintContent.Title01}");//"序列号补打";
                     
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            LotNumRepeatPrint lotNumRepeatPrint = new LotNumRepeatPrint();
            lotNumRepeatPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotNumRepeatPrint);
            //set panel to view content
            this.control = panel;
           
        }
    }
}
