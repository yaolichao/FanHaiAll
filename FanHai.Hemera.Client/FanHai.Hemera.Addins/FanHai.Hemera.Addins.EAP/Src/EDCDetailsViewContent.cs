//----------------------------------------------------------------------------------
// Copyright (c) SolarViewer
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
#region using
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Gui.Framework.Gui;
#endregion 

namespace SolarViewer.Hemera.Addins.EAP
{
    public class EDCDetailsViewContent : AbstractViewContent
    {
        private EDCDetailsCtrl edcDetailsCtrl = null;
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
        /// read only
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return false;
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

        public EDCDetailsViewContent(EdcGatherData edcData)
            : base()
        {
            if (null != edcData)
            {
                this.TitleName = StringParser.Parse("${res:SolarViewer.Hemera.Addins.EAP.EDCDetailsViewContent}");
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:SolarViewer.Hemera.Addins.EAP.EDCDetailsViewContent}");
            }

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            edcDetailsCtrl = new EDCDetailsCtrl(edcData);

            edcDetailsCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(edcDetailsCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
