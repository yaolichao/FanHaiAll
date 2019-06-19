using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;


namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次操作明细的参数类。
    /// </summary>
    public class LotOperationDetailModel
    {
        /// <summary>
        /// 批次操作类型。
        /// </summary>
        public LotOperationType OperationType { get; set; }
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string RoomKey { get; set; }
        /// <summary>
        /// 车间名称。
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 批次号。
        /// </summary>
        public string LotNumber { get; set; }
        /// <summary>
        /// 批次记录最后编辑时间。
        /// </summary>
        public string LotEditTime { get; set; }
        /// <summary>
        /// 班次名称。
        /// </summary>
        public string ShiftName { get; set; }
        /// <summary>
        /// 用户名称。
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 标题名称。
        /// </summary>
        public string TitleName { get; set; }
    }
    /// <summary>
    /// 批次操作明细视图类。
    /// </summary>
    public class LotOperationDetailViewContent : AbstractViewContent
    {
        Control control = null; //用于显示批次创建界面的控件对象。
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
        /// 批次操作类型。
        /// </summary>
        public LotOperationType OperationType
        {
            get;
            private set;
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
        /// 构造函数。
        /// </summary>
        public LotOperationDetailViewContent(LotOperationDetailModel model)
            : base()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            this.OperationType = model.OperationType;
            this.TitleName = model.TitleName;
            if (model.OperationType == LotOperationType.CellScrap || model.OperationType == LotOperationType.Scrap)
            {
                LotOperationScrap wk = new LotOperationScrap(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.Defect || model.OperationType == LotOperationType.CellDefect)
            {
                LotOperationDefect wk = new LotOperationDefect(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.CellPatch)
            {
                LotOperationPatch wk = new LotOperationPatch(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.CellRecovered)
            {
                LotOperationRecovered wk = new LotOperationRecovered(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.Adjust || model.OperationType == LotOperationType.BatchAdjust)
            {
                LotOperationAdjust wk = new LotOperationAdjust(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.Hold || model.OperationType == LotOperationType.BatchHold)
            {
                LotOperationHold wk = new LotOperationHold(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.Release || model.OperationType == LotOperationType.BatchRelease)
            {
                LotOperationRelease wk = new LotOperationRelease(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.Rework || model.OperationType == LotOperationType.BatchRework)
            {
                LotOperationRework wk = new LotOperationRework(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            else if (model.OperationType == LotOperationType.ReturnMaterial)
            {
                LotOperationReturnMaterial wk = new LotOperationReturnMaterial(model, this);
                wk.Dock = DockStyle.Fill;
                panel.Controls.Add(wk);
            }
            this.control = panel;
        }
    }
}
