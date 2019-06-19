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
    public class BasicSettingsDatViewContent : AbstractViewContent
    {
       
        #region variable define

        Control control = null; //define control
        TreeNode tn = null;     //receive paramter 
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

        public BasicSettingsDatViewContent(TreeNode tr) : base()
        {
            //get parameter treenode
            tn = tr;
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //define usercontrol BasicSettingsDetail
            BasicSettingsDatDetail basicSettingsdatDetail = new BasicSettingsDatDetail(tn);
            basicSettingsdatDetail.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(basicSettingsdatDetail);
            //set panel to view content
            this.control = panel;
            //set viewcontent's title name
            this.TitleName = tr.Text.ToString() + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatViewContent.ViewContentPartTitle}");
        }

        #endregion
    }
}

