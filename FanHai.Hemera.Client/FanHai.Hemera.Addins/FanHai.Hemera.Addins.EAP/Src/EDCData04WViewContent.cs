// ----------------------------------------------------------------------------------
// Copyright (c) FanHai
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
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 表示称重设备数据采集明细的视图类。
    /// </summary>
    public class EDCData04WViewContent : AbstractViewContent
    {
        private EDCData04W edc04WCtrl = null;           //设备数据采集明细的控件对象。
        //define control
        Control control = null;                         //控件对象，用于在平台上显示可视化的视图界面。

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
        public EDCData04WViewContent(EdcGatherData edcData)
            : base()
        {

            this.TitleName = "数据采集明细";

            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建新的设备数据控件对象。
            edc04WCtrl = new EDCData04W(edcData);
            edc04WCtrl.Dock = DockStyle.Fill;
            //将设备数据采集的控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于返回该平台显示设备数据采集的界面。
            panel.Controls.Add(edc04WCtrl);
            this.control = panel;
        }
    }
}
