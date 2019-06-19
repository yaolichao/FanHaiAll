using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示线别管理的视图类。
    /// </summary>
    class LineConfViewContent :AbstractViewContent
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
        /// <param name="udaEntity">表示用户自定义属性的实体对象。</param>
        public LineConfViewContent(UdaEntity udaEntity)
            : base()
        {

            if (null != udaEntity && udaEntity.ObjectName.Length > 0)
            {
                //TitleName = "线别维护" + "_" + udaEntity.ObjectName;
                TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.LineConfCtrl.title}") + "_" + udaEntity.ObjectName;
            }
            else
            {
                //TitleName = "线别维护";
                TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.LineConfCtrl.title}");
            }
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            LineConfCtrl _lineConfCtrl = new LineConfCtrl(udaEntity, EntityType.Line);
            _lineConfCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(_lineConfCtrl);
            //set panel to view content
            this.control =panel;
        }      


    }
}
