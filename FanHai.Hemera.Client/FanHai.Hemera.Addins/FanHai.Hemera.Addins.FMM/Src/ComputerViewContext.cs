//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;


namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示计算机配置的视图类。
    /// </summary>
    public class ComputerViewContext : AbstractViewContent
    {
        private ComputerConfCtrl _computerConfCtrl = null;
        //define control
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
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="computerEntity">表示计算机配置信息的实体对象。</param>
        public ComputerViewContext(ComputerEntity computerEntity)
            : base()
        {
            if (null != computerEntity && computerEntity.ComputerName.Length > 0)
            {
                //TitleName = "计算机配置" + "_" + computerEntity.ComputerName;
                TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ComputerConfCtrl.lbl.0001}") + "_" + computerEntity.ComputerName;
            }
            else
            {
                TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ComputerConfCtrl.lbl.0001}");
            }

            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            _computerConfCtrl = new ComputerConfCtrl(computerEntity);
            _computerConfCtrl.Dock = DockStyle.Fill;
            panel.Controls.Add(_computerConfCtrl);
            //set panel to view content
            this.control =panel;
        }
    }
}
