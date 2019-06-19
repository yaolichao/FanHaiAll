using System;
using System.Linq;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Gui.Framework.Commands
{
    public class SelectNextWindow : AbstractMenuCommand
    {
        public override void Run()
        {
            if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null)
            {
                return;
            }
            int index = WorkbenchSingleton.Workbench.WorkbenchWindowCollection.IndexOf(WorkbenchSingleton.Workbench.ActiveWorkbenchWindow);
            WorkbenchSingleton.Workbench.WorkbenchWindowCollection[(index + 1) % WorkbenchSingleton.Workbench.WorkbenchWindowCollection.Count].SelectWindow();
        }
    }

    public class SelectPrevWindow : AbstractMenuCommand
    {
        public override void Run()
        {
            if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null)
            {
                return;
            }
            int index = WorkbenchSingleton.Workbench.WorkbenchWindowCollection.IndexOf(WorkbenchSingleton.Workbench.ActiveWorkbenchWindow);
            WorkbenchSingleton.Workbench.WorkbenchWindowCollection[(index + WorkbenchSingleton.Workbench.WorkbenchWindowCollection.Count - 1) % WorkbenchSingleton.Workbench.WorkbenchWindowCollection.Count].SelectWindow();
        }
    }

    public class CloseAllWindows : AbstractMenuCommand
    {
        public override void Run()
        {
            WorkbenchSingleton.Workbench.CloseAllViews();
        }
    }

    public class CloseFileTab : AbstractMenuCommand
    {
        public override void Run()
        {
            IWorkbenchWindow window = Owner as IWorkbenchWindow;

            if (window != null)
            {
                window.CloseWindow(false);
            }
        }
    }

    public class CloseAllButThisFileTab : AbstractMenuCommand
    {
        public override void Run()
        {
            IWorkbenchWindow thisWindow = Owner as IWorkbenchWindow;

            foreach (IWorkbenchWindow window in WorkbenchSingleton.Workbench.WorkbenchWindowCollection.ToArray())
            {
                if (window != thisWindow)
                {
                    if (!window.CloseWindow(false))
                        break;
                }
            }
        }
    }
}
