//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang           2014-12-1              新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.MM
{

    /// <summary>
    /// 表示原材料发料的视图界面。
    /// </summary>
    public class SendingMaterialContent : AbstractViewContent
    {    
        internal  ExchangeFlag exchangeType;
        private Control control = null;
        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return this.control;
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
        }

        public SendingMaterialContent(ExchangeFlag flag)
            : base()
        {
            //this.TitleName = "原材料发料";
            ////define panel 
            //Panel panel = new Panel();
            ////set panel dock style
            //panel.Dock = DockStyle.Fill;
            ////set panel BorderStyle
            //panel.BorderStyle = BorderStyle.FixedSingle;
            //SendingMaterialCtrl ctrl = new SendingMaterialCtrl();
            //ctrl.Dock = DockStyle.Fill;
            ////add control to panle
            //panel.Controls.Add(ctrl);
            ////set panel to view content
            //this.control = panel;

            exchangeType = flag;
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            SendingMaterialCtrl commonCtrl = new SendingMaterialCtrl(flag);
            SendingBackMaterialCtrl commonBackCtrl = new SendingBackMaterialCtrl(flag);
            if (exchangeType == ExchangeFlag.Sending)
            {  
                //this.TitleName = "原材料发料";  //视图标题。  
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingMaterialCtrl.name}");
                commonCtrl.Dock = DockStyle.Fill;
                panel.Controls.Add(commonCtrl);

            }
            else if (exchangeType == ExchangeFlag.SendingBack)
            {
                //this.TitleName = "原材料退料";
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.SendingBackMaterialCtrl.name}");
                commonBackCtrl.Dock = DockStyle.Fill;
                panel.Controls.Add(commonBackCtrl);

            }
            this.control = panel;
        }
    }
}
