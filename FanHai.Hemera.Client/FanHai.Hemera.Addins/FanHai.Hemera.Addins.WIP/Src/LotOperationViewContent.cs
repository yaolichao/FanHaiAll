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
    /// 批次操作类型枚举。
    /// </summary>
    public enum LotOperationType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 电池片报废操作。
        /// </summary>
        CellScrap,
        /// <summary>
        /// 组件报废操作。
        /// </summary>
        Scrap,
        /// <summary>
        /// 组件不良操作。
        /// </summary>
        Defect,
        /// <summary>
        /// 电池片不良操作。
        /// </summary>
        CellDefect,
        /// <summary>
        /// 电池片补片。
        /// </summary>
        CellPatch,
        /// <summary>
        /// 电池片回收，用于撤销电池片报废或撤销电池片补片。
        /// </summary>
        CellRecovered,
        /// <summary>
        /// 批次调整。
        /// </summary>
        Adjust,
        /// <summary>
        /// 批量调整批次。
        /// </summary>
        BatchAdjust,
        /// <summary>
        /// 批次暂停。
        /// </summary>
        Hold,
        /// <summary>
        /// 批量暂停批次。
        /// </summary>
        BatchHold,
        /// <summary>
        /// 批次释放。
        /// </summary>
        Release,
        /// <summary>
        /// 批量释放批次。
        /// </summary>
        BatchRelease,
         /// <summary>
        /// 返工批次。
        /// </summary>
        Rework,
        /// <summary>
        /// 批量返工批次。
        /// </summary>
        BatchRework,
        /// <summary>
        /// 退料操作。
        /// </summary>
        ReturnMaterial,
        /// <summary>
        /// 工作站作业。
        /// </summary>
        Dispatch,
        /// <summary>
        /// 进站操作。
        /// </summary>
        TrackIn,
        /// <summary>
        /// 出站操作。
        /// </summary>
        TrackOut,
        /// <summary>
        /// 结束批次操作。
        /// </summary>
        Terminal,
        /// <summary>
        /// 拆分批次。
        /// </summary>
        Split,
        /// <summary>
        /// 合并批次。
        /// </summary>
        Merge,
        /// <summary>
        /// 入库作业。
        /// </summary>
        ToWarehouse
    }
    /// <summary>
    /// 批次操作视图类。
    /// </summary>
    public class LotOperationViewContent : AbstractViewContent
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
        public LotOperationViewContent(LotOperationType operationType)
            : base()
        {
            this.OperationType = operationType;
            if (this.OperationType == LotOperationType.CellScrap)
            {
                this.TitleName=StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title001}"); //电池片报废
                //this.TitleName = "电池片报废";
            }
            else if (this.OperationType == LotOperationType.Scrap)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title002}"); //组件报废
                //this.TitleName = "组件报废";
            }
            else if (this.OperationType == LotOperationType.CellDefect)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title003}"); //电池片不良
                //this.TitleName = "电池片不良";
            }
            else if (this.OperationType == LotOperationType.Defect)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title004}"); //组件不良
                //this.TitleName = "组件不良";
            }
            else if (this.OperationType == LotOperationType.CellPatch)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title005}"); //电池片补片
                //this.TitleName = "电池片补片";
            }
            else if (this.OperationType == LotOperationType.CellRecovered)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title006}"); //电池片回收
                //this.TitleName = "电池片回收";
            }
            else if (this.OperationType == LotOperationType.Adjust)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title007}"); //调整批次
                //this.TitleName = "调整批次";
            }
            else if (this.OperationType == LotOperationType.BatchAdjust)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title008}"); //批量调整批次
                //this.TitleName = "批量调整批次";
            }
            else if (this.OperationType == LotOperationType.Hold)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title009}"); //暂停批次
                //this.TitleName = "暂停批次";
            }
            else if (this.OperationType == LotOperationType.BatchHold)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title010}"); //批量暂停批次
                //this.TitleName = "批量暂停批次";
            }
            else if (this.OperationType == LotOperationType.Release)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title011}"); //释放批次
                //this.TitleName = "释放批次";
            }
            else if (this.OperationType == LotOperationType.BatchRelease)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title012}"); //批量释放批次
                //this.TitleName = "批量释放批次";
            }
            else if (this.OperationType == LotOperationType.Rework)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title013}"); //返修
                //this.TitleName = "返修";
            }
            else if (this.OperationType == LotOperationType.BatchRework)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title014}"); //批量返修
                //this.TitleName = "批量返修";
            }
            else if (this.OperationType == LotOperationType.ReturnMaterial)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title015}"); //退料操作
                //this.TitleName = "退料操作";
            }
            else if (this.OperationType == LotOperationType.Terminal)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title016}"); //结批操作
                //this.TitleName = "结批操作";
            }
            else if (this.OperationType == LotOperationType.Merge)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title017}"); //合并批次
                //this.TitleName = "合并批次";
            }
            else if (this.OperationType == LotOperationType.Split)
            {
                this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title018}"); //拆分批次
                //this.TitleName = "拆分批次";
            }
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            LotOperation wk = new LotOperation(operationType, this);
            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }
}
