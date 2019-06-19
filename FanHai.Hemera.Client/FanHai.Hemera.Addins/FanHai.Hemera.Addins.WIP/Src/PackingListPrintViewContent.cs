using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// IV测试数据铭牌标签打印类
    /// </summary>
    public class PackingListPrintViewContent : AbstractViewContent
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


        public PackingListPrintViewContent()
            : base()
        {


            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrintViewContent.Title01}");//""包装清单打印";
           
           
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            //LotIVTestPrint lotIvTestPrint = new LotIVTestPrint();
            Gui.PackingListPrint lotIvTestPrint = new Gui.PackingListPrint();
            lotIvTestPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotIvTestPrint);
            //set panel to view content
            this.control = panel;
           
        }
    }
}
