// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Chao.Pang           2012-03-05            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// 表示代码管理的视图类。
    /// </summary>
    public class EquipmentGroupsViewContent : AbstractViewContent
    {
        private Control control = null;
        private EquipmentGroups equipmentGroups;

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
            control.Dispose();

            base.Dispose();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentGroupsViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.Name}");

            Panel panel = new Panel();

            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            equipmentGroups = new EquipmentGroups();

            equipmentGroups.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(equipmentGroups);

            this.control = panel;
        }
    }
}
