using System;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.BasicData
{
    public class ErrorMessagePad : AbstractPadContent
    {
        #region variable define
        //define a panel
        Panel panel=new Panel();

        #endregion

        #region override functions
        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return panel;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            panel.Dispose();
        }

        #endregion

        #region constructor
        /// <summary>
        /// constructor ErrorMessagePad
        /// </summary>
        public ErrorMessagePad()
        {
            //set panel's dock style
            panel.Dock = DockStyle.Fill;
            //define ErrorMessageCtrl
            ErrorMessageCtrl emcl = new ErrorMessageCtrl();
            //set dock stype
            emcl.Dock = DockStyle.Fill;            
            //add
            panel.Controls.Add(emcl);
           // WorkbenchSingleton.Workbench.PadContentCollection.Add(this);
        }

        #endregion
    }
}
