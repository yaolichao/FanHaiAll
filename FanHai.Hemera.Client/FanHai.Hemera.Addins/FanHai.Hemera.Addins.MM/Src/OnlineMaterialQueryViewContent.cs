//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  乔永明
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 乔永明               2012-02-16             新建 
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
    class OnlineMaterialQueryViewContent: AbstractViewContent
    {
        //define control
        Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        //实现抽象属性
        public override Control Control//抽象属性，抽象类，抽象方法，其中抽象类中有抽象方法和抽象属性
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// IsViewOnly
        /// </summary>
        //实现抽象属性
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
            base.Dispose();//调用基类的方法
            control.Dispose();
            control = null;
        }

        public OnlineMaterialQueryViewContent()
            : base()
        {
            //this.TitleName = "在线库存查询";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.name}");
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            OnlineMaterialQueryCtrl onlineMaterialQueryCtrl = new OnlineMaterialQueryCtrl();
            onlineMaterialQueryCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(onlineMaterialQueryCtrl);
            //set panel to view content
            this.control = panel;
         }
    }
}
