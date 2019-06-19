using System;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Gui.Framework.Commands
{
    public class OptionsCommand : AbstractMenuCommand
    {

        public override void Run()
        {
            using (TreeViewOptions optionsDialog = new TreeViewOptions(AddInTree.GetTreeNode("/FanHaiFramework/Dialogs/OptionsDialog")))
            {
                optionsDialog.FormBorderStyle = FormBorderStyle.FixedDialog;

                optionsDialog.Owner = WorkbenchSingleton.MainForm;
                if (optionsDialog.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                {
                    PropertyService.Save();
                }
            }
        }
    }

    public class ToggleFullscreenCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            ((DefaultWorkbench)WorkbenchSingleton.Workbench).FullScreen = !((DefaultWorkbench)WorkbenchSingleton.Workbench).FullScreen;
        }
    }


}
