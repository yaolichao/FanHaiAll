using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public class SupplierViewContent : AbstractViewContent
    {
        private SupplierCtrl supplierCtrl = null;
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
        public SupplierViewContent()
            : base()
        {
            this.TitleName = "供应商管理";
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            supplierCtrl = new SupplierCtrl();
            supplierCtrl.CtrlState = ControlState.Empty;

            supplierCtrl.afterStateChanged += new SupplierCtrl.AfterStateChanged(OnAfterStateChange);

            supplierCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(supplierCtrl);
            //set panel to view content
            this.control = panel;
        }

        private void OnAfterStateChange(ControlState state)
        {
            if (state == ControlState.New)
            {
                this.TitleName = "供应商_新增";
            }
            else if (state == ControlState.Edit)
            {
                this.TitleName = "供应商_修改";
            }
            else
            {
                this.TitleName = "供应商管理";
            }
        }
    }
}
