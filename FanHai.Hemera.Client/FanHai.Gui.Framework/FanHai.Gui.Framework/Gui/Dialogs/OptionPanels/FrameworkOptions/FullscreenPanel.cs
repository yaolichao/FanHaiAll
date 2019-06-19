using System;
using System.Windows.Forms;


using FanHai.Gui.Core;

namespace FanHai.Gui.Framework.Gui.OptionPanels
{
    public class FullscreenPanel : AbstractOptionPanel
    {
        static readonly string fullscreenProperty = "FanHai.Gui.Framework.Gui.FullscreenOptions";

        public override void LoadPanelContents()
        {
            SetupFromXmlStream(this.GetType().Assembly.GetManifestResourceStream("FanHai.Gui.Framework.Resources.FullscreenPanel.xfrm"));

            Core.Properties properties = PropertyService.Get(fullscreenProperty, new Core.Properties());

            Get<CheckBox>("HideMainMenu").Checked = properties.Get("HideMainMenu", false);
            Get<CheckBox>("ShowMainMenuOnMouseMove").Checked = properties.Get("ShowMainMenuOnMouseMove", true);
            Get<CheckBox>("HideToolbars").Checked = properties.Get("HideToolbars", true);
            Get<CheckBox>("HideTabs").Checked = properties.Get("HideTabs", false);
            Get<CheckBox>("HideVerticalScrollbar").Checked = properties.Get("HideVerticalScrollbar", false);
            Get<CheckBox>("HideHorizontalScrollbar").Checked = properties.Get("HideHorizontalScrollbar", false);
            Get<CheckBox>("HideStatusBar").Checked = properties.Get("HideStatusBar", true);
            Get<CheckBox>("ShowStatusBarOnMouseMove").Checked = properties.Get("ShowStatusBarOnMouseMove", true);
            Get<CheckBox>("HideWindowsTaskbar").Checked = properties.Get("HideWindowsTaskbar", true);

            Get<CheckBox>("HideMainMenu").CheckedChanged += delegate { RefreshStatus(); };
            Get<CheckBox>("HideStatusBar").CheckedChanged += delegate { RefreshStatus(); };

            RefreshStatus();
        }

        void RefreshStatus()
        {
            Get<CheckBox>("ShowMainMenuOnMouseMove").Enabled = Get<CheckBox>("HideMainMenu").Checked;
            Get<CheckBox>("ShowStatusBarOnMouseMove").Enabled = Get<CheckBox>("HideStatusBar").Checked;
        }

        public override bool StorePanelContents()
        {
            Core.Properties properties = PropertyService.Get(fullscreenProperty, new Core.Properties());

            properties.Set("HideMainMenu", Get<CheckBox>("HideMainMenu").Checked);
            properties.Set("ShowMainMenuOnMouseMove", Get<CheckBox>("ShowMainMenuOnMouseMove").Checked);
            properties.Set("HideToolbars", Get<CheckBox>("HideToolbars").Checked);
            properties.Set("HideTabs", Get<CheckBox>("HideTabs").Checked);
            properties.Set("HideVerticalScrollbar", Get<CheckBox>("HideVerticalScrollbar").Checked);
            properties.Set("HideHorizontalScrollbar", Get<CheckBox>("HideHorizontalScrollbar").Checked);
            properties.Set("HideStatusBar", Get<CheckBox>("HideStatusBar").Checked);
            properties.Set("ShowStatusBarOnMouseMove", Get<CheckBox>("ShowStatusBarOnMouseMove").Checked);
            properties.Set("HideWindowsTaskbar", Get<CheckBox>("HideWindowsTaskbar").Checked);

            PropertyService.Set(fullscreenProperty, properties);

            return true;
        }
    }
}
