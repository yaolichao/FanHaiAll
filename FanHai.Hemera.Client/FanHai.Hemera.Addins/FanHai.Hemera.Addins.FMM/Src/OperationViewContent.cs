//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            添加注释 
// =================================================================================

#region using
using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
#endregion

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示工序管理的视图类。
    /// </summary>
    public class OperationViewContent : AbstractViewContent
    {
        private OperationCtrl operationCtrl = null;
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
        /// 重绘界面内容。
        /// </summary>
        public override void RedrawContent()
        {
            operationCtrl.InitUIResources();
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
        /// <param name="operation">表示工序信息的实体对象。</param>
        public OperationViewContent(OperationEntity operation)
            : base()
        {
            if (null != operation && operation.OperationName.Length > 0)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}")
                               + "_" + operation.OperationName + "." + operation.OperationVersion;
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}");
            }

            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            operationCtrl = new OperationCtrl(operation);

            operationCtrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(operationCtrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
