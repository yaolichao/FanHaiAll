using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui.Workbench.Layouts;
using FanHai.Gui.Framework.Widgets.AutoHide;
using WeifenLuo.WinFormsUI.Docking;

namespace FanHai.Gui.Framework.Gui
{
    /// <summary>
    /// This is the a Workspace with a single document interface.
    /// </summary>
    internal sealed class SdiWorkbenchLayout : IWorkbenchLayout
    {
        private const int DOCKPANEL_FONT_SIZE = 16;

        DefaultWorkbench wbForm;

        DockPanel dockPanel;
        Dictionary<string, PadContentWrapper> contentHash = new Dictionary<string, PadContentWrapper>();
        AutoHideMenuStripContainer mainMenuContainer;
        AutoHideStatusStripContainer statusStripContainer;
        ToolStripPanel toolBarPanel;

#if DEBUG
        static bool firstTimeError = true;
#endif

        public IWorkbenchWindow ActiveWorkbenchWindow
        {
            get
            {
                if (dockPanel == null)
                {
                    return null;
                }

                // TODO: Debug statements only, remove me
#if DEBUG
                if (dockPanel.ActiveDocument != null && !(dockPanel.ActiveDocument is IWorkbenchWindow))
                {
                    if (firstTimeError)
                    {
                        MessageBox.Show("ActiveDocument was " + dockPanel.ActiveDocument.GetType().FullName);
                        firstTimeError = false;
                    }
                }
#endif

                IWorkbenchWindow window = dockPanel.ActiveDocument as IWorkbenchWindow;
                if (window == null || window.IsDisposed)
                {
                    return null;
                }
                return window;
            }
        }

        // prevent setting ActiveContent to null when application loses focus (e.g. because of context menu popup)
        IDockContent lastActiveContent;

        public object ActiveContent
        {
            get
            {
                IDockContent activeContent;
                if (dockPanel == null)
                {
                    activeContent = lastActiveContent;
                }
                else
                {
                    activeContent = dockPanel.ActiveContent ?? lastActiveContent;
                }

                lastActiveContent = activeContent;

                if (activeContent is IWorkbenchWindow)
                    return ((IWorkbenchWindow)activeContent).ActiveViewContent;
                if (activeContent is PadContentWrapper)
                    return ((PadContentWrapper)activeContent).PadContent;

                return activeContent;
            }
        }


        public void Attach(IWorkbench workbench)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "logo.ico";

            wbForm = (DefaultWorkbench)workbench;
            wbForm.Icon = IconService.GetIcon(file);
            wbForm.ShowIcon = true;
            wbForm.SuspendLayout();
            wbForm.Controls.Clear();

            mainMenuContainer = new AutoHideMenuStripContainer(wbForm.TopMenu);
            mainMenuContainer.Dock = DockStyle.Left;

            wbForm.TopMenu.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;

            statusStripContainer = new AutoHideStatusStripContainer((StatusStrip)StatusBarService.Control);
            statusStripContainer.Dock = DockStyle.Bottom;

            toolBarPanel = new ToolStripPanel();
            if (wbForm.ToolBars != null)
            {
                toolBarPanel.Controls.AddRange(wbForm.ToolBars);
            }
            toolBarPanel.Dock = DockStyle.Top;
            dockPanel = new DockPanel();
            dockPanel.Dock = DockStyle.Fill;

            dockPanel.RightToLeftLayout = false;

            //navbar            
            MyMent myMent = new MyMent(wbForm.TopMenu);
            NavBarControl navBarControl = myMent.CreateControl();
            navBarControl.Dock = DockStyle.Left;

            DockPaneStripSkin dockPaneSkin = new DockPaneStripSkin();
            // 244,247,252 163, 186, 239
            dockPaneSkin.DocumentGradient.DockStripGradient.StartColor = System.Drawing.Color.FromArgb(251, 248, 240);
            dockPaneSkin.DocumentGradient.DockStripGradient.EndColor = System.Drawing.Color.FromArgb(251, 248, 240);
            // RGB: 217,234,250
            dockPaneSkin.DocumentGradient.ActiveTabGradient.StartColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.GradientActiveCaption);
            dockPaneSkin.DocumentGradient.ActiveTabGradient.EndColor = dockPaneSkin.DocumentGradient.ActiveTabGradient.StartColor;
            //RGB:221,234,244
            dockPaneSkin.DocumentGradient.InactiveTabGradient.StartColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.GradientInactiveCaption);
            dockPaneSkin.DocumentGradient.InactiveTabGradient.StartColor = System.Drawing.Color.FromArgb(251, 248, 240);
            dockPaneSkin.DocumentGradient.InactiveTabGradient.EndColor = dockPaneSkin.DocumentGradient.InactiveTabGradient.StartColor;
            dockPaneSkin.TextFont = new System.Drawing.Font(dockPaneSkin.TextFont.FontFamily, DOCKPANEL_FONT_SIZE);
            dockPanel.Skin.DockPaneStripSkin = dockPaneSkin;

            //dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            dockPanel.DocumentStyle = DocumentStyle.DockingSdi;

            wbForm.Controls.Add(dockPanel);
            //wbForm.Controls.Add(toolBarPanel);
            wbForm.Controls.Add(navBarControl);
            //wbForm.Controls.Add(statusStripContainer); // TODO 状态栏
            //wbForm.MainMenuStrip = wbForm.TopMenu;
            // dock panel has to be added to the form before LoadLayoutConfiguration is called to fix SD2-463

            LoadLayoutConfiguration();
            ShowPads();

            ShowViewContents();

            RedrawAllComponents();
            dockPanel.ActiveDocumentChanged += new EventHandler(ActiveMdiChanged);
            dockPanel.ActiveContentChanged += new EventHandler(ActiveContentChanged);

            ActiveMdiChanged(this, EventArgs.Empty);
            wbForm.FormBorderStyle = FormBorderStyle.Fixed3D;

            wbForm.ResumeLayout(false);
            Core.Properties fullscreenProperties = PropertyService.Get("FanHai.Gui.Framework.Gui.FullscreenOptions", new Core.Properties());
            fullscreenProperties.PropertyChanged += TrackFullscreenPropertyChanges;
        }

        void TrackFullscreenPropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            if (!Boolean.Equals(e.OldValue, e.NewValue) && wbForm.FullScreen)
            {
                switch (e.Key)
                {
                    case "HideMainMenu":
                    case "ShowMainMenuOnMouseMove":
                        RedrawMainMenu();
                        break;
                    case "HideToolbars":
                        RedrawToolbars();
                        break;
                    //case "HideTabs":
                    //case "HideVerticalScrollbar":
                    //case "HideHorizontalScrollbar":
                    case "HideStatusBar":
                    case "ShowStatusBarOnMouseMove":
                        RedrawStatusBar();
                        break;
                        //case "HideWindowsTaskbar":
                }
            }
        }

        void ShowPads()
        {
            foreach (PadDescriptor content in WorkbenchSingleton.Workbench.PadContentCollection)
            {
                if (!contentHash.ContainsKey(content.Class))
                {
                    ShowPad(content);
                }
            }
            // ShowPads could create new pads if new addins have been installed, so we
            // need to call AllowInitialize here instead of in LoadLayoutConfiguration
            foreach (PadContentWrapper content in contentHash.Values)
            {
                content.AllowInitialize();
            }
        }
        void ShowViewContents()
        {
            foreach (IViewContent content in WorkbenchSingleton.Workbench.PrimaryViewContents)
            {
                ShowView(content, true);
            }
        }

        void LoadLayoutConfiguration()
        {
            try
            {
                if (File.Exists(LayoutConfiguration.CurrentLayoutFileName))
                {
                    LoadDockPanelLayout(LayoutConfiguration.CurrentLayoutFileName);
                }
                else
                {
                    LoadDefaultLayoutConfiguration();
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                // ignore errors loading configuration
            }
        }

        void LoadDefaultLayoutConfiguration()
        {
            if (File.Exists(LayoutConfiguration.CurrentLayoutTemplateFileName))
            {
                LoadDockPanelLayout(LayoutConfiguration.CurrentLayoutTemplateFileName);
            }
        }

        void LoadDockPanelLayout(string fileName)
        {
            // LoadFromXml(fileName, ...) locks the file against simultanous read access
            // -> we would loose the layout when starting two SolarViewerFramework instances
            //    at the same time => open stream with shared read access.
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                dockPanel.LoadFromXml(fs, new DeserializeDockContent(GetContent));
            }
        }

        DockContent GetContent(string padTypeName)
        {
            foreach (PadDescriptor content in WorkbenchSingleton.Workbench.PadContentCollection)
            {
                if (content.Class == padTypeName)
                {
                    return CreateContent(content);
                }
            }
            return null;
        }

        public void LoadConfiguration()
        {
            if (dockPanel != null)
            {
                NativeMethods.SetWindowRedraw(wbForm.Handle, false);
                try
                {
                    IWorkbenchWindow activeWindow = this.ActiveWorkbenchWindow;
                    dockPanel.ActiveDocumentChanged -= new EventHandler(ActiveMdiChanged);
                    dockPanel.ActiveContentChanged -= new EventHandler(ActiveContentChanged);

                    DetachPadContents(false);
                    DetachViewContents(false);
                    dockPanel.ActiveDocumentChanged += new EventHandler(ActiveMdiChanged);
                    dockPanel.ActiveContentChanged += new EventHandler(ActiveContentChanged);

                    LoadLayoutConfiguration();
                    ShowPads();
                    ShowViewContents();
                    if (activeWindow != null)
                    {
                        activeWindow.SelectWindow();
                    }
                }
                finally
                {
                    NativeMethods.SetWindowRedraw(wbForm.Handle, true);
                    wbForm.Refresh();
                }
            }
        }

        public void StoreConfiguration()
        {
            try
            {
                if (dockPanel != null)
                {
                    LayoutConfiguration current = LayoutConfiguration.CurrentLayout;
                    if (current != null && !current.ReadOnly)
                    {

                        string configPath = Path.Combine(PropertyService.ConfigDirectory, "layouts");
                        if (!Directory.Exists(configPath))
                            Directory.CreateDirectory(configPath);
                        dockPanel.SaveAsXml(Path.Combine(configPath, current.FileName), System.Text.Encoding.UTF8);
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
        }

        void DetachPadContents(bool dispose)
        {
            foreach (PadContentWrapper padContentWrapper in contentHash.Values)
            {
                padContentWrapper.allowInitialize = false;
            }
            foreach (PadDescriptor content in ((DefaultWorkbench)wbForm).PadContentCollection)
            {
                try
                {
                    PadContentWrapper padContentWrapper = contentHash[content.Class];
                    padContentWrapper.DockPanel = null;
                    if (dispose)
                    {
                        padContentWrapper.DetachContent();
                        padContentWrapper.Dispose();
                    }
                }
                catch (Exception e) { MessageService.ShowError(e); }
            }
            if (dispose)
            {
                contentHash.Clear();
            }
        }

        void DetachViewContents(bool dispose)
        {
            foreach (SdiWorkspaceWindow f in WorkbenchSingleton.Workbench.WorkbenchWindowCollection)
            {
                try
                {
                    f.DockPanel = null;
                    if (dispose)
                    {
                        f.CloseEvent -= CloseWindowEvent;
                        f.Dispose();
                    }
                }
                catch (Exception e) { MessageService.ShowError(e); }
            }
        }
        public void Detach()
        {
            StoreConfiguration();

            dockPanel.ActiveDocumentChanged -= new EventHandler(ActiveMdiChanged);

            DetachPadContents(true);
            DetachViewContents(true);

            try
            {
                if (dockPanel != null)
                {
                    dockPanel.Dispose();
                    dockPanel = null;
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            if (contentHash != null)
            {
                contentHash.Clear();
            }

            wbForm.Controls.Clear();
        }

        class PadContentWrapper : DockContent
        {
            PadDescriptor padDescriptor;
            bool isInitialized = false;
            internal bool allowInitialize = false;

            public IPadContent PadContent
            {
                get
                {
                    return padDescriptor.PadContent;
                }
            }

            public PadContentWrapper(PadDescriptor padDescriptor)
            {
                if (padDescriptor == null)
                    throw new ArgumentNullException("padDescriptor");
                this.padDescriptor = padDescriptor;
                this.DockAreas = DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom;
                HideOnClose = true;
            }

            public void DetachContent()
            {
                Controls.Clear();
                padDescriptor = null;
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                if (Visible && Width > 0)
                    ActivateContent();
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                if (Visible && Width > 0)
                    ActivateContent();
            }

            /// <summary>
            /// Enables initializing the content. This is used to prevent initializing all view
            /// contents when the layout configuration is changed.
            /// </summary>
            public void AllowInitialize()
            {
                allowInitialize = true;
                if (Visible && Width > 0)
                    ActivateContent();
            }

            void ActivateContent()
            {
                if (!allowInitialize)
                    return;
                if (!isInitialized)
                {
                    isInitialized = true;
                    IPadContent content = padDescriptor.PadContent;
                    if (content == null)
                        return;
                    try
                    {
                        Control control = content.Control;
                        control.Dock = DockStyle.Fill;
                        Controls.Add(control);
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowError(ex, "Error in IPadContent.Control");
                    }
                }
            }

            protected override string GetPersistString()
            {
                return padDescriptor.Class;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    if (padDescriptor != null)
                    {
                        padDescriptor.Dispose();
                        padDescriptor = null;
                    }
                }
            }

            public override string ToString()
            {
                return "[PadContentWrapper " + padDescriptor.Class + "]";
            }
        }

        PadContentWrapper CreateContent(PadDescriptor content)
        {
            if (contentHash.ContainsKey(content.Class))
            {
                return contentHash[content.Class];
            }

            PadContentWrapper newContent = new PadContentWrapper(content);
            if (!string.IsNullOrEmpty(content.Icon))
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + "logo.ico";

                newContent.Icon = IconService.GetIcon(file);
            }
            newContent.Text = StringParser.Parse(content.Title);
            contentHash[content.Class] = newContent;
            return newContent;
        }

        public void ShowPad(PadDescriptor content)
        {
            if (content == null)
            {
                return;
            }
            PadContentWrapper dockContent;
            if (!contentHash.TryGetValue(content.Class, out dockContent))
            {
                dockContent = CreateContent(content);
                // TODO: read the default dock state from the PadDescriptor
                // we'll also need to allow for default-hidden (HideOnClose) contents
                // which seems to be not possible using any Show overload.
                dockContent.Show(dockPanel);
            }
            else if (dockContent.VisibleState == DockState.Unknown)
            {
                dockContent.Show(dockPanel);
            }
            else
            {
                dockContent.Show();
            }
        }

        public bool IsVisible(PadDescriptor padContent)
        {
            if (padContent != null && contentHash.ContainsKey(padContent.Class))
            {
                PadContentWrapper dockContent = contentHash[padContent.Class];
                return !dockContent.IsHidden && dockContent.VisibleState != DockState.Unknown;
            }
            return false;
        }

        public void HidePad(PadDescriptor padContent)
        {
            if (padContent != null && contentHash.ContainsKey(padContent.Class))
            {
                contentHash[padContent.Class].Hide();
            }
        }

        public void UnloadPad(PadDescriptor padContent)
        {
            if (padContent != null && contentHash.ContainsKey(padContent.Class))
            {
                contentHash[padContent.Class].Close();
                contentHash[padContent.Class].Dispose();
                contentHash.Remove(padContent.Class);
            }
        }

        public void ActivatePad(PadDescriptor padContent)
        {
            if (padContent != null && contentHash.ContainsKey(padContent.Class))
            {
                //contentHash[padContent.Class].ActivateContent();
                contentHash[padContent.Class].Show();
            }
        }
        public void ActivatePad(string fullyQualifiedTypeName)
        {
            //contentHash[fullyQualifiedTypeName].ActivateContent();
            contentHash[fullyQualifiedTypeName].Show();
        }


        public void RedrawAllComponents()
        {
            // redraw correct pad content names (language changed).
            foreach (PadDescriptor padDescriptor in ((IWorkbench)wbForm).PadContentCollection)
            {
                DockContent c = contentHash[padDescriptor.Class];
                if (c != null)
                {
                    c.Text = StringParser.Parse(padDescriptor.Title);
                }
            }

            RedrawMainMenu();
            RedrawToolbars();
            RedrawStatusBar();
        }

        void RedrawMainMenu()
        {
            Core.Properties fullscreenProperties = PropertyService.Get("FanHai.Gui.Framework.Gui.FullscreenOptions", new Core.Properties());
            bool hideInFullscreen = fullscreenProperties.Get("HideMainMenu", false);
            bool showOnMouseMove = fullscreenProperties.Get("ShowMainMenuOnMouseMove", true);

            mainMenuContainer.AutoHide = wbForm.FullScreen && hideInFullscreen;
            mainMenuContainer.ShowOnMouseDown = true;
            mainMenuContainer.ShowOnMouseMove = showOnMouseMove;
        }

        void RedrawToolbars()
        {
            Core.Properties fullscreenProperties = PropertyService.Get("FanHai.Gui.Framework.Gui.FullscreenOptions", new Core.Properties());
            bool hideInFullscreen = fullscreenProperties.Get("HideToolbars", true);
            bool toolBarVisible = PropertyService.Get("FanHai.Gui.Framework.Gui.ToolBarVisible", true);

            toolBarPanel.Visible = toolBarVisible && !(wbForm.FullScreen && hideInFullscreen);
        }

        void RedrawStatusBar()
        {
            Core.Properties fullscreenProperties = PropertyService.Get("FanHai.Gui.Framework.Gui.FullscreenOptions", new Core.Properties());
            bool hideInFullscreen = fullscreenProperties.Get("HideStatusBar", true);
            bool showOnMouseMove = fullscreenProperties.Get("ShowStatusBarOnMouseMove", true);
            bool statusBarVisible = PropertyService.Get("FanHai.Gui.Framework.Gui.StatusBarVisible", false);

            statusStripContainer.AutoHide = wbForm.FullScreen && hideInFullscreen;
            statusStripContainer.ShowOnMouseDown = true;
            statusStripContainer.ShowOnMouseMove = showOnMouseMove;
            statusStripContainer.Visible = statusBarVisible;
        }

        void CloseWindowEvent(object sender, EventArgs e)
        {
            SdiWorkspaceWindow f = (SdiWorkspaceWindow)sender;
            f.CloseEvent -= CloseWindowEvent;
            foreach (IViewContent vc in f.ViewContents.ToArray())
            {
                ((IWorkbench)wbForm).CloseContent(vc);
            }
            if (f == oldSelectedWindow)
            {
                oldSelectedWindow = null;
            }
            ActiveMdiChanged(this, null);
        }

        public IWorkbenchWindow ShowView(IViewContent content, bool switchToOpenedView)
        {
            if (content.WorkbenchWindow is SdiWorkspaceWindow)
            {
                SdiWorkspaceWindow oldSdiWindow = (SdiWorkspaceWindow)content.WorkbenchWindow;
                if (!oldSdiWindow.IsDisposed)
                {
                    if (switchToOpenedView)
                    {
                        oldSdiWindow.Show(dockPanel);
                    }
                    else
                    {
                        this.AddWindowToDockPanelWithoutSwitching(oldSdiWindow);
                    }
                    return oldSdiWindow;
                }
            }
            content.Control.Dock = DockStyle.Fill;
            SdiWorkspaceWindow sdiWorkspaceWindow = new SdiWorkspaceWindow();
            sdiWorkspaceWindow.ViewContents.Add(content);
            //sdiWorkspaceWindow.ViewContents.AddRange(content.SecondaryViewContents);
            sdiWorkspaceWindow.CloseEvent += new EventHandler(CloseWindowEvent);
            if (dockPanel != null)
            {
                if (switchToOpenedView)
                {
                    sdiWorkspaceWindow.Show(dockPanel);
                }
                else
                {
                    this.AddWindowToDockPanelWithoutSwitching(sdiWorkspaceWindow);
                }
            }

            return sdiWorkspaceWindow;
        }

        void AddWindowToDockPanelWithoutSwitching(SdiWorkspaceWindow sdiWorkspaceWindow)
        {
            sdiWorkspaceWindow.DockPanel = dockPanel;
            SdiWorkspaceWindow otherWindow = dockPanel.ActiveContent as SdiWorkspaceWindow;
            if (otherWindow == null)
            {
                otherWindow = dockPanel.Contents.OfType<SdiWorkspaceWindow>().FirstOrDefault(c => c.Pane != null);
            }
            if (otherWindow != null)
            {
                sdiWorkspaceWindow.Pane = otherWindow.Pane;
            }
            sdiWorkspaceWindow.DockState = DockState.Document;
        }

        void ActiveMdiChanged(object sender, EventArgs e)
        {
            OnActiveWorkbenchWindowChanged(e);
        }

        void ActiveContentChanged(object sender, EventArgs e)
        {
            OnActiveWorkbenchWindowChanged(e);
        }

        IWorkbenchWindow oldSelectedWindow = null;

        internal void OnActiveWorkbenchWindowChanged(EventArgs e)
        {
            IWorkbenchWindow newWindow = this.ActiveWorkbenchWindow;
            if (newWindow == null || newWindow.ActiveViewContent != null)
            {
                if (ActiveWorkbenchWindowChanged != null)
                {
                    ActiveWorkbenchWindowChanged(this, e);
                }
                //if (newWindow == null)
                //	LoggingService.Debug("window change to null");
                //else
                //	LoggingService.Debug("window change to " + newWindow);
            }
            else
            {
                //LoggingService.Debug("ignore window change to disposed window");
            }
            if (oldSelectedWindow != null && oldSelectedWindow.ActiveViewContent != null)
            {
                oldSelectedWindow.OnWindowDeselected(EventArgs.Empty);
            }
            oldSelectedWindow = newWindow;
            if (newWindow != null && newWindow.ActiveViewContent != null)
            {
                newWindow.OnWindowSelected(EventArgs.Empty);
            }
        }

        public event EventHandler ActiveWorkbenchWindowChanged;
    }
}
