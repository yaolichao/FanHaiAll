using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    class WorkOrderInfoListViewContent : AbstractViewContent
    {
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
        /// 构造函数
        /// </summary>
        public WorkOrderInfoListViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WorkOrderInfoListViewContent.Title01}");//"工单信息清单";             //视图标题。
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            WorkOrderInfoListCtrl workOrderInfoListCtrl = new WorkOrderInfoListCtrl();
            workOrderInfoListCtrl.Dock = DockStyle.Fill;
            panel.Controls.Add(workOrderInfoListCtrl);
            this.control = panel;
        }
    }
}
