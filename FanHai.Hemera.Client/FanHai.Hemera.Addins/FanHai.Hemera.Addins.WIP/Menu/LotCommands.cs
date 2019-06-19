using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批量调整批次的菜单命令类。通过该类显示批量调整批次的窗口界面。
    /// </summary>
    class LotBatchAdjustCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.BatchAdjust)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.BatchAdjust);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批次调整的菜单命令类。通过该类显示批次调整的窗口界面。
    /// </summary>
    class LotAdjustCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Adjust)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Adjust);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示电池片报废的菜单命令类。通过该类显示电池片报废的窗口界面。
    /// </summary>
    class LotCellScrapCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType==LotOperationType.CellScrap)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            LotOperationDetailModel model = new LotOperationDetailModel();
            model.OperationType = LotOperationType.CellScrap;
            //显示电池片操作明细界面。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);

            ////创建新的视图对象，并显示该视图界面。
            //LotOperationViewContent view = new LotOperationViewContent(LotOperationType.CellScrap);
            //WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示组件报废的菜单命令类。通过该类显示组件报废的窗口界面。
    /// </summary>
    class LotModuleScrapCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Scrap)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            LotOperationDetailModel model = new LotOperationDetailModel();
            model.OperationType = LotOperationType.Scrap;
            //显示电池片操作明细界面。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);
            ////创建新的视图对象，并显示该视图界面。
            //LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Scrap);
            //WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示电池片补片的菜单命令类。通过该类显示电池片补片的窗口界面。
    /// </summary>
    class LotCellPatchCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.CellPatch)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.CellPatch);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示电池片回收（撤销电池片报废或者撤销电池片补片）的菜单命令类。通过该类显示电池片回收的窗口界面。
    /// </summary>
    class LotCellRecoveredCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.CellRecovered)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.CellRecovered);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批次暂停的菜单命令类。通过该类显示批次暂停的窗口界面。
    /// </summary>
    class LotHoldCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Hold)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Hold);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批量暂停批次的菜单命令类。通过该类显示批量暂停批次的窗口界面。
    /// </summary>
    class LotBatchHoldCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.BatchHold)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.BatchHold);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批次释放的菜单命令类。通过该类显示批次释放的窗口界面。
    /// </summary>
    class LotReleaseCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Release)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Release);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批量释放批次的菜单命令类。通过该类显示批量释放批次的窗口界面。
    /// </summary>
    class LotBatchReleaseCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.BatchRelease)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.BatchRelease);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批次返修的菜单命令类。通过该类显示批次返工的窗口界面。
    /// </summary>
    class LotReworkCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Rework)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            LotOperationDetailModel model = new LotOperationDetailModel();
            model.OperationType = LotOperationType.Rework;
            //显示电池片操作明细界面。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);

            ////创建新的视图对象，并显示该视图界面。
            //LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Rework);
            //WorkbenchSingleton.Workbench.ShowView(view);


        }
    }
    /// <summary>
    /// 表示批量返工批次的菜单命令类。通过该类显示批量返工批次的窗口界面。
    /// </summary>
    class LotBatchReworkCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.BatchRework)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            LotOperationDetailModel model = new LotOperationDetailModel();
            model.OperationType = LotOperationType.BatchRework;
            //显示电池片操作明细界面。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);

            ////创建新的视图对象，并显示该视图界面。
            //LotOperationViewContent view = new LotOperationViewContent(LotOperationType.BatchRework);
            //WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示电池片不良操作的菜单单命令类。通过该类显示批次不良操作的窗口界面。
    /// </summary>
    class LotCellDefectCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.CellDefect)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.CellDefect);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 表示组件不良操作的菜单单命令类。通过该类显示批次不良操作的窗口界面。
    /// </summary>
    class LotDefectCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Defect)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            LotOperationDetailModel model = new LotOperationDetailModel();
            model.OperationType = LotOperationType.Defect;
            //显示电池片操作明细界面。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //创建新的视图并显示
            LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            WorkbenchSingleton.Workbench.ShowView(view);

            ////创建新的视图对象，并显示该视图界面。
            //LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Defect);
            //WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    ///表示退料操作的菜单单命令类。通过该类显示批次退料操作的窗口界面。
    /// </summary>
    class LotReturnMaterialCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.ReturnMaterial)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.ReturnMaterial);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    ///表示结束批次操作的菜单单命令类。通过该类显示批次结束操作的窗口界面。
    /// </summary>
    class LotTerminalCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Terminal)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Terminal);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 表示合并批次的菜单命令类。通过该类显示合并批次的窗口界面。
    /// </summary>
    class LotMergeCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Merge)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Merge);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 表示拆分批次的菜单命令类。通过该类显示拆分批次的窗口界面。
    /// </summary>
    class LotSplitCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationViewContent openView = viewContent as LotOperationViewContent;
                if (openView != null && openView.OperationType == LotOperationType.Split)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotOperationViewContent view = new LotOperationViewContent(LotOperationType.Split);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示预设暂停的菜单命令类。
    /// </summary>
    class LotFutureHoldCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotFutureHoldViewContent openView = viewContent as LotFutureHoldViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotFutureHoldViewContent view = new LotFutureHoldViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
    /// <summary>
    /// 表示批次查询的菜单命令类。通过该类显示批次查询的窗口界面。
    /// </summary>
    class LotSearchCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotSearchViewContent openView = viewContent as LotSearchViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotSearchViewContent view = new LotSearchViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }

    /// <summary>
    /// 表示E工单包装的菜单命令类。通过该类显示E工单包装的窗口界面。
    /// </summary>
    class LotDispatchFor_eWoPalletCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                LotDispatchFor_eWoPalletViewContent openView = viewContent as LotDispatchFor_eWoPalletViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotDispatchFor_eWoPalletViewContent view = new LotDispatchFor_eWoPalletViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
}
