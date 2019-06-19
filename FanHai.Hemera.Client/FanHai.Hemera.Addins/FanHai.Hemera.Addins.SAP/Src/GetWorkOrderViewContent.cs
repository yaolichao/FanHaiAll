//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// =================================================================================
// 创建人               修改时间              说明
// ---------------------------------------------------------------------------------
// Jing.Xie          2012-02-20            添加注释 
// =================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    public class GetWorkOrderViewContent : AbstractViewContent
    {
        Control control = null;                      //从SAP进行工单信息获取的控件对象。
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
        public GetWorkOrderViewContent()
            : base()
        {
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderViewContent.Title}"); //SAP工单获取  //视图标题。
            Panel oP = new Panel();
            oP.Dock = DockStyle.Fill;
            oP.BorderStyle = BorderStyle.FixedSingle;
            //创建新的从SAP获取工单信息的控件对象。
            GetWorkOrderCtrl oGetWorkOrderCtrl = new GetWorkOrderCtrl();
            oGetWorkOrderCtrl.Dock = DockStyle.Fill;
            //将从SAP获取工单的控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            oP.Controls.Add(oGetWorkOrderCtrl);
            this.control = oP;
        }
    }
}
