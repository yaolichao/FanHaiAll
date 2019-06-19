/*
<FileInfo>
  <Author>ZhangHao FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/


using System;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;


namespace FanHai.Hemera.Addins.BasicData
{
    public class BasicSettingsViewContent : AbstractViewContent
    {
        #region variable define
        //define control
        Control control = null;

        #endregion

        #region override functions
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

        #endregion

        #region constructor
        /// <summary>
        /// view content of basic settings of column
        /// </summary>
        /// <param name="tr"></param>
        public BasicSettingsViewContent(TreeNode tn): base()
        {
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //define usercontrol BasicSettingsDetail
            BasicSettingsDetail basicSettingsDetail = new BasicSettingsDetail(tn);
            basicSettingsDetail.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(basicSettingsDetail);
            //set panel to view content
            this.control = panel;

            //if status is edit
            if (tn != null)
            {
                //set viewcontent's title name
                this.TitleName = tn.Text.ToString() + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsViewContent.ViewContentPartTitle}");
            }
            //if status is new
            else
            {
                //set viewcontent's title name
                this.TitleName = "[New Data Type]";
            }
        }

        #endregion
    }
}

