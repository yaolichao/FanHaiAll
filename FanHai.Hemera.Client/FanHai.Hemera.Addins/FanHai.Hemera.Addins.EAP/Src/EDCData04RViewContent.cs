// ----------------------------------------------------------------------------------
// Copyright (c) SolarViewer
// ----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-02-22            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Framework.Gui;
using SolarViewer.Hemera.Utils.Entities;
using System.Windows.Forms;

namespace SolarViewer.Hemera.Addins.EAP
{
    /// <summary>
    /// 表示方块电阻设备数据采集的视图类。
    /// </summary>
    public class EDCData04RViewContent : AbstractViewContent
    {
        private EDCData04R edc04RCtrl = null;                           //设备数据采集的控件对象。
        //define control
        Control control = null;                                         //控件对象，用于在平台上显示可视化的视图界面。

        /// <summary>
        /// 控件对象，用于在平台上显示可视化的视图界面。
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
        public EDCData04RViewContent(EdcGatherData edcData)
            : base()
        {
            this.TitleName ="方块电阻";
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建新的设备数据控件对象。
            edc04RCtrl = new EDCData04R(edcData);
            edc04RCtrl.Dock = DockStyle.Fill;

            //将设备数据采集的控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于返回该平台显示设备数据采集的界面。
            //add control to panle
            panel.Controls.Add(edc04RCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
