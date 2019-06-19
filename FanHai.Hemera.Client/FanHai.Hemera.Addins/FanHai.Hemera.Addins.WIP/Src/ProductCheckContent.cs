using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Hemera.Addins.WIP;

namespace FanHai.Hemera.Addins.WIP
{
    public class ProductCheckContent : AbstractViewContent
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

        public ProductCheckContent()
            : base()
        {
            this.TitleName = StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ProductCheckContent.Title01}"));//"电流比释放";

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            ProductPrintCheck abNormalRules = new ProductPrintCheck();

            abNormalRules.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(abNormalRules);

            //set panel to view content
            this.control = panel;
        }
    }
}
