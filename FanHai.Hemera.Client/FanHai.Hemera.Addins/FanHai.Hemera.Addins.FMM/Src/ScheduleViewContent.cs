using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示班次管理的视图类。
    /// </summary>
    class ScheduleViewContent : AbstractViewContent
    {
         //define control
        Control control = null;//用于显示排班管理界面的控件对象。

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
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="titleName">视图标题字符串。</param>
        /// <param name="schedule">表示排班计划的对象。</param>
        public ScheduleViewContent(string titleName,Schedule schedule)
            : base()
        {
            //this.TitleName = "班次管理";
            if (titleName != "" )
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}") + "_" + titleName;
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}");
            }
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            ScheduleCtrl ctrl = new ScheduleCtrl(schedule);

            ctrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
