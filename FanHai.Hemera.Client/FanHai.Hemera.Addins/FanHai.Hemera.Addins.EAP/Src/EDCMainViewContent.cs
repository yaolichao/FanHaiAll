// ----------------------------------------------------------------------------------
// Copyright (c) FanHai
// ----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-02-22            添加注释 
// =================================================================================
#region using
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
#endregion

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 表示设备数据采集的视图类。
    /// </summary>
    public class EDCMainViewContent : AbstractViewContent
    {
        private EDCMainCtrl edcMainCtrl = null;                 //设备数据采集的控件对象。
        //define control
        Control control = null;                                 //控件对象，用于在平台上显示可视化的视图界面。

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
        /// 是否只读。
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return false;
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
        /// 构造函数
        /// </summary>
        /// <param name="edcData">表示设备数据采集的数据对象。</param>
        public EDCMainViewContent(): base()
        {
            this.TitleName = "数据采集";  //视图标题。
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建新的设备数据控件对象。
            edcMainCtrl = new EDCMainCtrl();
            edcMainCtrl.Dock = DockStyle.Fill;
            //将设备数据采集的控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(edcMainCtrl);
            this.control = panel;
        }
    }
}
