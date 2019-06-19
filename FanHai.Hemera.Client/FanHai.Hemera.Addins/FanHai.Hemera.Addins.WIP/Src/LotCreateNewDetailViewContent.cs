using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.WIP;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次创建明细的参数类。
    /// </summary>
    public class LotCreateNewDetailModel
    {
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string RoomKey{get;set;}
        /// <summary>
        /// 车间名称。
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 原材料编码。
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 领料项目号。
        /// </summary>
        public string ReceiveItemNo { get; set; }
        /// <summary>
        /// 原材料线上仓存储明细主键。
        /// </summary>
        public string StoreMaterialDetailKey { get; set; }
        /// <summary>
        /// 产品ID号。
        /// </summary>
        public string ProId { get; set; }
        /// <summary>
        /// 工单主键。
        /// </summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 工单号。
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 成品编码。
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 成品主键。
        /// </summary>
        public string PartKey { get; set; }
        /// <summary>
        /// 绑定线别主键。
        /// </summary>
        public string LotLineKey { get; set; }
        /// <summary>
        /// 绑定线别代码。
        /// </summary>
        public string LotLineCode { get; set; }
        /// <summary>
        /// 投批数量。
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 批次创建类别编码。
        /// </summary>
        public string CreateTypeCode { get; set; }
        /// <summary>
        /// 批次创建类别名称。
        /// </summary>
        public string CreateTypeName { get; set; }
        /// <summary>
        /// 班次名称。
        /// </summary>
        public string ShiftName { get; set; }
        /// <summary>
        /// 用户名称。
        /// </summary>
        public string UserName{get;set;}
        /// <summary>
        /// 供应商名称。
        /// </summary>
        public string SupplierName { get; set; }
    }
    /// <summary>
    /// 表示创建批次明细的视图类。
    /// </summary>
    public class LotCreateNewDetailViewContent : AbstractViewContent
    {
         //define control
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
        public LotCreateNewDetailViewContent(LotCreateNewDetailModel model, bool isBatch)
            : base()
        {
            if (isBatch)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreateNewDetailViewContent.Title01}");//"创建生产批次";   //视图标题。
            }
            else
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotCreateNewDetailViewContent.Title02}");//"创建补片批次";   //视图标题。
            }
            //define panel 
            Panel panel = new Panel();
            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;
            //创建批次创建的控件对象
            LotCreateNewDetail ctrl = new LotCreateNewDetail(model, isBatch, this);
            ctrl.Dock = DockStyle.Fill;
            //将控件对象加入到Panel中。
            //设置Panel为该视图对象的控件对象，用于在应用程序平台上显示可视化的视图界面。
            panel.Controls.Add(ctrl);
            //set panel to view content
            this.control = panel;
        }
    }
}
