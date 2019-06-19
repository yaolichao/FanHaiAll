//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  冯旭
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 乔永明               2012-03-22            新建 
// =================================================================================
using System;
using System.Windows.Forms;


using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.SAP
{
    class WorkOrderReturnViewContent : AbstractViewContent
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

        public WorkOrderReturnViewContent(): base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WorkOrderReturnViewContent.Title01}");//"工单退料";//这个是画面秀出来以后的，显示的标题

            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            WorkOrderMaterialReturnCtrl workOrderReturnCtrl = new WorkOrderMaterialReturnCtrl();
            workOrderReturnCtrl.Dock = DockStyle.Fill;

            //add control to panle
            panel.Controls.Add(workOrderReturnCtrl);
            //set panel to view content
            this.control = panel;
         }
    }
}
