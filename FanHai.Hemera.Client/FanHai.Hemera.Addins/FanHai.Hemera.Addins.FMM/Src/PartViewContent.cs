using System;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示产品料号维护的视图类。
    /// </summary>
    public class PartViewContent : AbstractViewContent
    {

        //define control
        Control control = null;//用于显示视图界面的控件对象。


        /// <summary>
        /// 控件对象，用于在应用程序平台上显示可视化的视图界面。
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
            }
        }
        /// <summary>
        /// 释放使用的所有资源。
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="udaEntity">表示产品料号维护的实体对象。</param>
        public PartViewContent(Part part)
            : base()
        {
            //成品物料对象不为空，且物料名称不为空字符串。
            if (null != part && part.PartName.Length >1)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}") + "_" + part.PartName;
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}");
            }
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            PartEditor pEditor = new PartEditor(part);

            pEditor.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(pEditor);
            this.control = panel;
        }
    }
}

