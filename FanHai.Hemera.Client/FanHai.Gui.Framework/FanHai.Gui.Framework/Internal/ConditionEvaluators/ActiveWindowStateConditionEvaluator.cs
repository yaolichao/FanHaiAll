using System;
using System.Linq;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Gui.Framework
{
    public enum WindowState
    {
        None = 0,
        Untitled = 1,
        Dirty = 2,
        ViewOnly = 4
    }

    /// <summary>
    /// Tests the window state of the active workbench window.
    /// </summary>
    public class ActiveWindowStateConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition)
        {
            if (WorkbenchSingleton.Workbench == null ||
                WorkbenchSingleton.Workbench.ActiveViewContent == null)
            {
                return false;
            }

            WindowState windowState = condition.Properties.Get("windowstate", WindowState.None);
            WindowState nowindowState = condition.Properties.Get("nowindowstate", WindowState.None);


            bool isWindowStateOk = false;
            if (windowState != WindowState.None)
            {
                if ((windowState & WindowState.Dirty) > 0)
                {
                    isWindowStateOk |= WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContents.Any(vc => vc.IsDirty);
                }
                if ((windowState & WindowState.Untitled) > 0)
                {
                    isWindowStateOk |= IsUntitled(WorkbenchSingleton.Workbench.ActiveViewContent);
                }
                if ((windowState & WindowState.ViewOnly) > 0)
                {
                    isWindowStateOk |= WorkbenchSingleton.Workbench.ActiveViewContent.IsViewOnly;
                }
            }
            else
            {
                isWindowStateOk = true;
            }

            if (nowindowState != WindowState.None)
            {
                if ((nowindowState & WindowState.Dirty) > 0)
                {
                    isWindowStateOk &= !WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContents.Any(vc => vc.IsDirty);
                }

                if ((nowindowState & WindowState.Untitled) > 0)
                {
                    isWindowStateOk &= !IsUntitled(WorkbenchSingleton.Workbench.ActiveViewContent);
                }

                if ((nowindowState & WindowState.ViewOnly) > 0)
                {
                    isWindowStateOk &= !WorkbenchSingleton.Workbench.ActiveViewContent.IsViewOnly;
                }
            }
            return isWindowStateOk;
        }

        static bool IsUntitled(IViewContent viewContent)
        {
            return false;
            //OpenedFile file = viewContent.PrimaryFile;
            //if (file == null)
            //    return false;
            //else
            //    return file.IsUntitled;
        }
    }
}
