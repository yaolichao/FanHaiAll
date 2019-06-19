using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 表示资源管理的视图类。
    /// </summary>
    public class ResourceViewContent : AbstractViewContent
    {
         //define control
        Control control = null;//用于显示视图界面的控件对象。
        ResourceCtrl resourceCtrl = null;

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
        /// 重绘内容。
        /// </summary>
        public override void RedrawContent()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceViewContent.TitleName}");   
            resourceCtrl.InitUIResourcesByCulture();
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
        public ResourceViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceViewContent.TitleName}");        
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            resourceCtrl = new ResourceCtrl();
            //userCtrl.CtrlState = ControlState.New;
            resourceCtrl.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(resourceCtrl);
            //set panel to view content
            this.control = panel;            
        }
    }
}
