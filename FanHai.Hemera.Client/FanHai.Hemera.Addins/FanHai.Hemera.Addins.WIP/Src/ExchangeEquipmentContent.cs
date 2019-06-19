using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.WIP.Gui;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 组件序列号打印 -chao.pang
    /// </summary>
    public class ExchangeEquipmentContent : AbstractViewContent
    {
         //define control
        Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// IsViewOnly
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
            control = null;
        }


        public ExchangeEquipmentContent()
            : base()
        {


            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipmentContent.Title}"); //批次号转单串焊设备  //视图标题。
           
           
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            ExchangeEquipment exchangeEquipment = new ExchangeEquipment();
            exchangeEquipment.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(exchangeEquipment);
            //set panel to view content
            this.control = panel;
           
        }
    }
}
