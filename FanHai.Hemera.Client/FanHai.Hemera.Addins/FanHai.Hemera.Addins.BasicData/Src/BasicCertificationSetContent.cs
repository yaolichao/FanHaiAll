using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Hemera.Addins.BasicData;
using FanHai.Hemera.Addins.BasicData.Gui;

namespace FanHai.Hemera.Addins.BasicData
{
    public class BasicCertificationSetContent : AbstractViewContent
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

        public BasicCertificationSetContent()
            : base()
        {
            this.TitleName = StringParser.Parse("产品认证维护");

            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;

            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            BasicCertificationSet basicCertificationSet = new BasicCertificationSet();

            basicCertificationSet.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(basicCertificationSet);

            //set panel to view content
            this.control = panel;
        }
    }
}
