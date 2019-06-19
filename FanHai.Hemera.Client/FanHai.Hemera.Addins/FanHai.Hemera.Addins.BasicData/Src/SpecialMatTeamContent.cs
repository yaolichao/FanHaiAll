using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public class SpecialMatTeamContent : AbstractViewContent
    {
        private SpecialMatTeamCtrl specialMatTeamCtrl = null;
        //define control
        Control control = null;

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
        public SpecialMatTeamContent()
            : base()
        {
            this.TitleName = "特殊物料管控";
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            specialMatTeamCtrl = new SpecialMatTeamCtrl();
            specialMatTeamCtrl.CtrlState = ControlState.Empty;

            specialMatTeamCtrl.afterStateChanged += new SpecialMatTeamCtrl.AfterStateChanged(OnAfterStateChange);

            specialMatTeamCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(specialMatTeamCtrl);
            //set panel to view content
            this.control = panel;
        }

        private void OnAfterStateChange(ControlState state)
        {
            if (state == ControlState.New)
            {
                this.TitleName = "特殊物料管控_新增";
            }
            else if (state == ControlState.Edit)
            {
                this.TitleName = "特殊物料管控_修改";
            }
            else
            {
                this.TitleName = "特殊物料管控";
            }
        }
    }
}
