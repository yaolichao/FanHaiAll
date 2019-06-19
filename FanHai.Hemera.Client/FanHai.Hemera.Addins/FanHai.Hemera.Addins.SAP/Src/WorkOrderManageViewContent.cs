using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 表示工单管理（创建、修改、删除）的视图类。
    /// </summary>
    public class WorkOrderManageViewContent : AbstractViewContent
    {
        Control control = null; //用于显示界面的控件对象。
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
        public WorkOrderManageViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WorkOrderManageViewContent.Title01}");//"工单管理";   //视图标题。
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            WorkOrderManage ctrl = new WorkOrderManage(this);
            ctrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(ctrl);
            this.control = panel;
        }
    }
}
