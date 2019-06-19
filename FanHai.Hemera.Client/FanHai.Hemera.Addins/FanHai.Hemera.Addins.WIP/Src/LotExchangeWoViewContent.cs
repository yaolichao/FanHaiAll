using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示转工单\返工单作业相关操作的视图类。
    /// </summary>
    public class LotExchangeWoViewContent : AbstractViewContent
    {
        internal  ExchangeWoFlag exchangeType;
        //用于显示批次创建界面的控件对象。
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
        /// 视图内容是否仅允许查看（不能被保存）。
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
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
        public LotExchangeWoViewContent(ExchangeWoFlag flag)
            : base()
        {
            exchangeType = flag;
            if (exchangeType == ExchangeWoFlag.Exchange)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWoViewContent.Title01}");//"转工单作业";  //视图标题。
            }
            else if (exchangeType == ExchangeWoFlag.Repair)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWoViewContent.Title02}");//"返工单作业";
            }
            else if (exchangeType == ExchangeWoFlag.MultiExchange)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWoViewContent.Title03}");//"批量转工单";
            }
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            LotExchangeWo commonCtrl = new LotExchangeWo(flag);
            commonCtrl.Dock = DockStyle.Fill;
            panel.Controls.Add(commonCtrl);
            this.control = panel;
        }
    }

}
