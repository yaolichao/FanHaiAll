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
    public class BasicPlanInputAimViewContent : AbstractViewContent
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
        public BasicPlanInputAimViewContent()
            : base()
        {
            //set panel's dock style
            panel.Dock = DockStyle.Fill;
            //define BasicDataSettingTree
            BasicPlanInputAim aim = new BasicPlanInputAim();
            //set dock stype
            aim.Dock = DockStyle.Fill;
            //this.TitleName = StringParser.Parse("计划目标值输入");
            this.TitleName = StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0001}"));
            //set data to treeview
            TreeView treeView=new TreeView();
            //add
            panel.Controls.Add(aim);
           // WorkbenchSingleton.Workbench.PadContentCollection.Add(this);
        }

        #endregion
    }

    public class BasicOptShiftViewContent : AbstractViewContent
    {
        #region variable define
        //define a panel
        Panel panel = new Panel();

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
        public BasicOptShiftViewContent()
            : base()
        {
            //set panel's dock style
            panel.Dock = DockStyle.Fill;
            //define BasicDataSettingTree
            BasicRptOptSetting aim = new BasicRptOptSetting();
            //set dock stype
            aim.Dock = DockStyle.Fill;
            //this.TitleName = StringParser.Parse("工序班别维护");
            this.TitleName = StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0002}"));
            //set data to treeview
            TreeView treeView = new TreeView();
            //add
            panel.Controls.Add(aim);
            // WorkbenchSingleton.Workbench.PadContentCollection.Add(this);
        }

        #endregion
    }
}
