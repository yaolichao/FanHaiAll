using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Core.WinForms;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Gui.Framework.Commands
{
    public class StartWorkbenchCommand
    {
        const string workbenchMemento = "WorkbenchMemento";

        class FormKeyHandler : IMessageFilter
        {
            const int keyPressedMessage = 0x100;

            void SelectActiveWorkbenchWindow()
            {
                if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null)
                {
                    if (!WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent.Control.ContainsFocus)
                    {
                        if (Form.ActiveForm == WorkbenchSingleton.MainForm)
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent.Control.Focus();
                        }
                    }
                }
            }

            bool PadHasFocus()
            {
                foreach (PadDescriptor padContent in WorkbenchSingleton.Workbench.PadContentCollection)
                {
                    if (padContent.HasFocus)
                    {
                        return true;

                    }
                }
                return false;
            }
            string oldLayout = "Default";
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg != keyPressedMessage)
                {
                    return false;
                }
                Keys keyPressed = (Keys)m.WParam.ToInt32() | Control.ModifierKeys;

                if (keyPressed == Keys.Escape)
                {
                    if (PadHasFocus() && !MenuService.IsContextMenuOpen)
                    {
                        SelectActiveWorkbenchWindow();
                        return true;
                    }
                    return false;
                }

                if (keyPressed == (Keys.Escape | Keys.Shift))
                {
                    if (LayoutConfiguration.CurrentLayoutName == "Plain")
                    {
                        LayoutConfiguration.CurrentLayoutName = oldLayout;
                    }
                    else
                    {
                        WorkbenchSingleton.Workbench.WorkbenchLayout.StoreConfiguration();
                        oldLayout = LayoutConfiguration.CurrentLayoutName;
                        LayoutConfiguration.CurrentLayoutName = "Plain";
                    }
                    SelectActiveWorkbenchWindow();
                    return true;
                }
                return false;
            }
        }

        public void Run(IList<string> fileList)
        {
            //WorkbenchSingleton.MainForm.Show();

            bool didLoadSolutionOrFile = false;

            NavigationService.SuspendLogging();

            foreach (string file in fileList)
            {
                LoggingService.Info("Open file " + file);
                didLoadSolutionOrFile = true;
                try
                {
                    string fullFileName = Path.GetFullPath(file);

                    //IProjectLoader loader = ProjectService.GetProjectLoader(fullFileName);
                    //if (loader != null)
                    //{
                    //    loader.Load(fullFileName);
                    //}
                    //else
                    //{
                    //    FileService.OpenFile(fullFileName);
                    //}
                }
                catch (Exception e)
                {
                    MessageService.ShowError(e, "unable to open file " + file);
                }
            }

            if (!didLoadSolutionOrFile)
            {
                foreach (ICommand command in AddInTree.BuildItems<ICommand>("/Workspace/AutostartNothingLoaded", null, false))
                {
                    try
                    {
                        command.Run();
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowError(ex);
                    }
                }

                ICommand hidePadCmd = new HidePadCommand();
                hidePadCmd.Run();
            }

            NavigationService.ResumeLogging();

            //WorkbenchSingleton.MainForm.Focus(); // windows.forms focus workaround

            ParserService.StartParserThread();

            // finally run the workbench window ...
            Application.AddMessageFilter(new FormKeyHandler());
            //add by zhanghao begin 20100623 for status
            StatusBarService.SetCaretPosition(30, 70, 0);
            //end
            
            //Register app switch event 
            if (!CallRemotingService.RegisterAppSwitchEvent())
            {
                MessageService.ShowError("App switch event register fail!");
            }

            //Register receive message from server event
            if (!CallRemotingService.RegisterReceiveMessageEvent())
            {
                MessageService.ShowError("Receive message fail!");
            }

            Application.Run(WorkbenchSingleton.MainForm);
            

            // save the workbench memento in the ide properties
            try
            {
                PropertyService.Set(workbenchMemento, WorkbenchSingleton.Workbench.CreateMemento());
                
            }
            catch (Exception e)
            {
                MessageService.ShowError(e, "Exception while saving workbench state.");
            }
        }
    }
}

