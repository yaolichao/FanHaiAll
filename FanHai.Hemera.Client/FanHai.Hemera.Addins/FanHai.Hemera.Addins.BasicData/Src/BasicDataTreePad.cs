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
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.BasicData
{
    public class BasicDataTreePad : AbstractPadContent
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
        /// constructor BasicDataTreePad
        /// </summary>
        public BasicDataTreePad()
        {
            //set panel's dock style
            panel.Dock = DockStyle.Fill;
            //define BasicDataSettingTree
            BasicDataSettingTree bdst = new BasicDataSettingTree();
            //set dock stype
            bdst.Dock = DockStyle.Fill;

            //set data to treeview
            TreeView treeView=new TreeView();
            //add
            panel.Controls.Add(bdst);
           // WorkbenchSingleton.Workbench.PadContentCollection.Add(this);
        }

        #endregion
    }
}
