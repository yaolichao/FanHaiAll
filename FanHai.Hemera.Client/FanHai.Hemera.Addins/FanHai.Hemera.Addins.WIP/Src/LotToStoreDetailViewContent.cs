using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;


namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 入库作业明细视图类。
    /// </summary>
    public class LotToStoreDetailViewContent : AbstractViewContent
    {
        Control control = null; //用于显示批次创建界面的控件对象。
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
            control = null;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotToStoreDetailViewContent(LotDispatchDetailModel model)
            : base()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName;
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotToStoreDetailViewContent.Title01}");//"入库作业";
            LotToWarehouseDetail wk = new LotToWarehouseDetail(model, this);
            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }
}
