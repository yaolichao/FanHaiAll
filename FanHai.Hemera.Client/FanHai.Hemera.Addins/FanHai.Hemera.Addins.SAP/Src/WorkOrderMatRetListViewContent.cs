using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;

using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    public class WorkOrderMatRetListViewContent : AbstractViewContent
    {
        //定义一个属性
        Control control = null;
        public override Control Control
        {
            get
            {
                return control;
            }
        }
        //实现抽象方法
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


        public WorkOrderMaterialReturnCtrl parent2
        {
            get;
            set;
        }
        //构造函数
        public WorkOrderMatRetListViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WorkOrderMatRetListViewContent.Title01}");//"工单退料清单";

            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            WorkOrderMatRetListCtrl workerRetMatListCtrl = new WorkOrderMatRetListCtrl();
            workerRetMatListCtrl.parent = this;
            workerRetMatListCtrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(workerRetMatListCtrl);
            //set panel to view content
            this.control = panel;
         }
    }
}
