using System;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Gui.Core.WinForms;
namespace FanHai.Gui.Framework.Gui
{
    public class ErrorListPad : AbstractPadContent, IClipboardHandler
    {
        static ErrorListPad instance;
        public static ErrorListPad Instance
        {
            get
            {
                return instance;
            }
        }

        ToolStrip toolStrip;
        Panel contentPanel = new Panel();

        Core.Properties properties;

        public bool ShowErrors
        {
            get
            {
                return properties.Get<bool>("ShowErrors", true);
            }
            set
            {
                properties.Set<bool>("ShowErrors", value);
                InternalShowResults();
            }
        }

        public bool ShowMessages
        {
            get
            {
                return properties.Get<bool>("ShowMessages", true);
            }
            set
            {
                properties.Set<bool>("ShowMessages", value);
                InternalShowResults();
            }
        }

        public bool ShowWarnings
        {
            get
            {
                return properties.Get<bool>("ShowWarnings", true);
            }
            set
            {
                properties.Set<bool>("ShowWarnings", value);
                InternalShowResults();
            }
        }

        public static bool ShowAfterBuild
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public override Control Control
        {
            get
            {
                return contentPanel;
            }
        }

        public ErrorListPad()
        {
            instance = this;
            properties = PropertyService.Get("ErrorListPad", new Core.Properties());

            RedrawContent();


            toolStrip = ToolbarService.CreateToolStrip(this, "/FanHaiFramework/Pads/ErrorList/Toolbar");
            toolStrip.Stretch = true;
            toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;

            contentPanel.Controls.Add(toolStrip);

            InternalShowResults();
        }

        public override void RedrawContent()
        {
            
        }


        void OnSolutionClosed(object sender, EventArgs e)
        {
            try
            {
                UpdateToolstripStatus();
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        void ProjectServiceEndBuild(object sender, EventArgs e)
        {
            UpdateToolstripStatus();
        }

        void TaskServiceCleared(object sender, EventArgs e)
        {
            UpdateToolstripStatus();
        }

        void UpdateToolstripStatus()
        {
            ToolbarService.UpdateToolbar(toolStrip);
            ToolbarService.UpdateToolbarText(toolStrip);
        }

        void InternalShowResults()
        {
            // listView.CreateControl is called in the constructor now.
            UpdateToolstripStatus();
        }

        #region IClipboardHandler interface implementation
        public bool EnableCut
        {
            get { return false; }
        }
        public bool EnableCopy
        {
            get { return false; }
        }
        public bool EnablePaste
        {
            get { return false; }
        }
        public bool EnableDelete
        {
            get { return false; }
        }
        public bool EnableSelectAll
        {
            get { return true; }
        }

        public void Cut() { }
        public void Paste() { }
        public void Delete() { }

        public void Copy()
        {
            
        }
        public void SelectAll()
        {
            
        }
        #endregion
    }
}
