using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;
using FanHai.Hemera.Share.Constants;
using System.Data;


namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次工作站作业明细的参数类。
    /// </summary>
    public class LotDispatchDetailModel : LotOperationDetailModel
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotDispatchDetailModel()
        {
            this.IsCheckSILot = false;
            this.IsShowSetNewRoute = false;
            this.IsShowPrintLabelDialog = false;
            this.IsCheckColorByWorkOrder = false;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model">用于复制的类。</param>
        public LotDispatchDetailModel(LotDispatchDetailModel model)
        {
            this.EquipmentKey = model.EquipmentKey;
            this.EquipmentName = model.EquipmentName;
            this.LineKey = model.LineKey;
            this.LineName = model.LineName;
            this.LotEditTime = model.LotEditTime;
            this.LotNumber = model.LotNumber;
            this.OperationType = model.OperationType;
            this.RoomKey = model.RoomKey;
            this.RoomName = model.RoomName;
            this.ShiftKey = model.ShiftKey;
            this.ShiftName = model.ShiftName;
            this.TitleName = model.TitleName;
            this.UserName = model.UserName;
            this.OperationName = model.OperationName;
            this.IsCheckSILot = model.IsCheckSILot;
            this.IsCheckColorByWorkOrder = model.IsCheckColorByWorkOrder;
            this.IsShowSetNewRoute = model.IsShowSetNewRoute;
            this.IsShowPrintLabelDialog = model.IsShowPrintLabelDialog;
            //------------------添加分档相关字段-----------------
            this.VirtualCustomerNumber = model.VirtualCustomerNumber;                           //分档档位信息
            this.TrayText = model.TrayText;
            this.TrayValue = model.TrayValue;
            this.Number = model.Number;
            this.PackageNumber = model.PackageNumber;
            this.Color = model.Color;
            this.WorkOrderNo = model.WorkOrderNo;
            this.PatrNumber = model.PatrNumber;
            this.GradeName = model.GradeName;
            this.PsKey = model.PsKey;
            this.SubPowerlevel = model.SubPowerlevel;
            this.IsFlip = model.IsFlip;
            //------------------添加是否包护角字段-----------------
            this.IsPack = model.IsPack;
        }
        /// <summary>
        /// 班次主键。
        /// </summary>
        public string ShiftKey { get; set; }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 设备主键。
        /// </summary>
        public string EquipmentKey { get; set; }
        /// <summary>
        /// 设备名称。
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// 线别主键。
        /// </summary>
        public string LineKey { get; set; }
        /// <summary>
        /// 线别名称。
        /// </summary>
        public string LineName { get; set; }
        /// <summary>
        /// 是否检查电池片信息。
        /// </summary>
        public bool IsCheckSILot { get; set; }
        /// <summary>
        /// 是否根据工单检查颜色。如果颜色信息没有输入且工单设置了必须输入颜色，则颜色必须输入。
        /// </summary>
        public bool IsCheckColorByWorkOrder { get; set; }
        /// <summary>
        /// 是否显示设置新的工艺流程选择的栏位。
        /// </summary>
        public bool IsShowSetNewRoute { get; set; }
        /// <summary>
        /// 是否显示打印标签的对话框。
        /// </summary>
        public bool IsShowPrintLabelDialog { get; set; }
        /// <summary>
        /// 判定终检等级是否为客级/A级品
        /// </summary>
        public string ModuleGrade { get; set; }
        /// <summary>
        /// 根据工单在终检工序获得客户类别
        /// </summary>
        public string CustomerType { get; set; }
        /// <summary>
        /// 虚拟托盘
        /// </summary>
        public string VirtualCustomerNumber { get; set; }
        /// <summary>
        /// 托盘名称
        /// </summary>
        public string TrayText { get; set; }
        /// <summary>
        /// 托盘编号
        /// </summary>
        public string TrayValue { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNumber { get; set; }
        /// <summary>
        /// 满托包装数
        /// </summary>
        public int PackageNumber { get; set; }
        /// <summary>
        /// 花色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 当前托盘组件数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// 产品料号
        /// </summary>
        public string PatrNumber { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string GradeName { get; set; }
        /// <summary>
        /// 功率档
        /// </summary>
        public string PsKey { get; set; }
        /// <summary>
        /// 档位
        /// </summary>
        public string SubPowerlevel { get; set; }

        public string IsFlip { get; set; }
        /// <summary>
        /// 是否包护角
        /// </summary>
        public string IsPack { get; set; }
    }
    /// <summary>
    /// 批次工作站作业明细视图类。
    /// </summary>
    public class LotDispatchDetailViewContent : AbstractViewContent
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
        public LotDispatchDetailViewContent(LotDispatchDetailModel model, DataTable dtLotInfo)
            : base()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            this.TitleName = model.TitleName;
            //工作站作业-进站
            if (model.OperationType == LotOperationType.TrackIn)
            {
                this.TitleName = string.Format("{0}" + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchListCtl.TrackIn}"), this.TitleName);  //进站
            }
            else
            {
                this.TitleName = string.Format("{0}" + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchListCtl.TrackOut}"), this.TitleName);  //出站
            }
            LotDispatchDetail wk = new LotDispatchDetail(model,dtLotInfo, this);
            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }
    //====================================================================
  
    /// <summary>
    /// 批次工作站作业终检视图类。
    /// </summary>
    public class LotDispatchForCustCheckViewContent : AbstractViewContent
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
        public LotDispatchForCustCheckViewContent(LotDispatchDetailModel model, string check_type)
            : base()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName; 

            if (check_type == CHECKTYPE.DATA_GROUP_ENDCHECK)
                this.TitleName = string.Format("{0}_"+StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.FINAL_CHECK}"), model.TitleName);//终检
            else
                this.TitleName = string.Format("{0}" + StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.CUSTOMER_CHECK}"), model.TitleName);//客检

            LotDispatchForCustCheck wk = new LotDispatchForCustCheck(model, this);
            wk.checktype = check_type;
            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }
    /// <summary>
    /// 批次工作站作业包装类。
    /// </summary>
    public class LotDispatchForPalletViewContent : AbstractViewContent
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
        /// 批次工作站作业包装类
        public LotDispatchForPalletViewContent(LotDispatchDetailModel model)
            : base()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName;          

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchForPalletViewContent.Title01}");//"过站作业_包装";

            LotDispatchForPallet wk = new LotDispatchForPallet(this, model);

            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
        public LotDispatchForPalletViewContent()
            : base()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName;          

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchForPalletViewContent.Title01}");//"过站作业_包装";

            LotDispatchForPallet wk = new LotDispatchForPallet(this);

            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }
    /// <summary>
    /// 批次工作站作业入库检验类。
    /// </summary>
    public class LotToWarehouseCHeckViewContent : AbstractViewContent
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
        public LotToWarehouseCHeckViewContent(LotDispatchDetailModel model)
            : base()
        {
             
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName;          

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotToWarehouseCHeckViewContent.Title01}");//"过站作业_入库检";

            LotToWarehouseCheck wk = new LotToWarehouseCheck(model, this);

            wk.Dock = DockStyle.Fill;
            panel.Controls.Add(wk);
            this.control = panel;
        }
    }

    /// <summary>
    /// 批次工作站作业包装出托类。
    /// </summary>
    public class PalletWholeOutViewContent : AbstractViewContent
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
        public PalletWholeOutViewContent()
            : base()
        {

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName;          

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletWholeOutViewContent.Title01}");//"包装出托作业";

            PalletWholeOut pwo = new PalletWholeOut(this);

            pwo.Dock = DockStyle.Fill;
            panel.Controls.Add(pwo);
            this.control = panel;
        }
    }

    /// <summary>
    /// E工单批次包装作业
    /// </summary>
    public class LotDispatchFor_eWoPalletViewContent : AbstractViewContent
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
        public LotDispatchFor_eWoPalletViewContent()
            : base()
        {

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            //this.TitleName = model.TitleName;          

            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchFor_eWoPalletViewContent.Title01}");//"E工单-包装作业";
            LotDispatchDetailModel model = new LotDispatchDetailModel();
            LotDispatchFor_eWoPallet pwo = new LotDispatchFor_eWoPallet(this, model);

            pwo.Dock = DockStyle.Fill;
            panel.Controls.Add(pwo);
            this.control = panel;
        }
    }
}

