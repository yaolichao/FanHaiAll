using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.BasicData
{
    public class BomMaterialBandViewContent : AbstractViewContent
    {
        private BomMaterialBandCtrl bomMaterialBandCtrl = null;
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
        public BomMaterialBandViewContent()
            : base()
        {
            //this.TitleName = "物料维护";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.title.0001}");
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            bomMaterialBandCtrl = new BomMaterialBandCtrl();
            bomMaterialBandCtrl.CtrlState = ControlState.Empty;

            bomMaterialBandCtrl.afterStateChanged += new BomMaterialBandCtrl.AfterStateChanged(OnAfterStateChange);

            bomMaterialBandCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(bomMaterialBandCtrl);
            //set panel to view content
            this.control = panel;
        }

        private void OnAfterStateChange(ControlState state)
        {
            if (state == ControlState.New)
            {
                //this.TitleName = "物料_新增";
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.title.0002}");
            }
            else if (state == ControlState.Edit)
            {
                //this.TitleName = "物料_修改";
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.title.0003}");
            }
            else
            {
                //this.TitleName = "物料维护";
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.title.0004}");
            }
        }
    }
}
