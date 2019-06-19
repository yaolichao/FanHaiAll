using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Gui.Framework
{
    public static class StatusBarService
    {
        static SdStatusBar statusBar = null;

        internal static void Initialize()
        {
            statusBar = new SdStatusBar();
        }

        public static bool Visible
        {
            get
            {
                System.Diagnostics.Debug.Assert(statusBar != null);
                return statusBar.Visible;
            }
            set
            {
                System.Diagnostics.Debug.Assert(statusBar != null);
                statusBar.Visible = value;
            }
        }

        public static Control Control
        {
            get
            {   
                System.Diagnostics.Debug.Assert(statusBar != null);
                return statusBar;
            }
        }

        public static void SetCaretPosition(int x, int y, int charOffset)
        {            
            ToolStripStatusLabel lblServer=new ToolStripStatusLabel ();
            ToolStripStatusLabel lblOperation = new ToolStripStatusLabel();
            ToolStripStatusLabel lblDate = new ToolStripStatusLabel();
            ToolStripStatusLabel lblUserName = new ToolStripStatusLabel(); 

            lblServer.Text = StringParser.Parse("${res:Global.RemotingServer}") + ":" + PropertyService.Get("SITE");
            lblOperation.Text = StringParser.Parse("${res:Global.Operator}") + PropertyService.Get("USER_NAME");  //工号          
            lblDate.Text = StringParser.Parse("${res:Global.LoginTime}") + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            lblUserName.Text = StringParser.Parse("${res:Global.Name}") + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);//姓名

            if (statusBar == null)
                statusBar = new SdStatusBar();
            if(statusBar.Items.Count>0)
                statusBar.Items.Clear();
            statusBar.Items.Add(lblServer);          
            statusBar.Items.Add(new ToolStripSeparator());
            statusBar.Items.Add(lblOperation);
            statusBar.Items.Add(new ToolStripSeparator());
            statusBar.Items.Add(lblUserName);
            statusBar.Items.Add(new ToolStripSeparator());
            statusBar.Items.Add(lblDate);
            //statusBar.CursorStatusBarPanel.Text = StringParser.Parse(
            // StringParser.Parse("${res:Global.RemotingServer}") + ":" + PropertyService.Get("Site") + " " + StringParser.Parse("${res:Global.Operator}") +
            // PropertyService.Get(PROPERTY_FIELDS.USER_NAME) + " " + StringParser.Parse("${res:Global.OperateTime}") + DateTime.Now.ToString("yyyy-MM-dd"),
            //   new string[,] {
            //        {"Line", String.Format("{0,-10}", y)},
            //        {"Column", String.Format("{0,-5}", x)},
            //        {"Character", String.Format("{0,-5}", charOffset)}
            //    });
        }

        public static void SetInsertMode(bool insertMode)
        {
            statusBar.ModeStatusBarPanel.Text = insertMode ? StringParser.Parse("${res:StatusBarService.CaretModes.Insert}") : StringParser.Parse("${res:StatusBarService.CaretModes.Overwrite}");
        }

        public static void ShowErrorMessage(string message)
        {
            System.Diagnostics.Debug.Assert(statusBar != null);
            statusBar.ShowErrorMessage(StringParser.Parse(message));
        }

        public static void SetMessage(string message)
        {
            System.Diagnostics.Debug.Assert(statusBar != null);
            lastMessage = message;
            statusBar.SetMessage(StringParser.Parse(message));
        }

        public static void SetMessage(Image image, string message)
        {
            System.Diagnostics.Debug.Assert(statusBar != null);
            statusBar.SetMessage(image, StringParser.Parse(message));
        }

        public static void SetMessage(string message, bool highlighted)
        {
            statusBar.SetMessage(message, highlighted);
        }

        static bool wasError = false;
        static string lastMessage = "";

        public static void RedrawStatusbar()
        {
            if (wasError)
            {
                ShowErrorMessage(lastMessage);
            }
            else
            {
                SetMessage(lastMessage);
            }

            Visible = PropertyService.Get("FanHai.Gui.Framework.Gui.StatusBarVisible", true);
        }

        public static void Update()
        {
            System.Diagnostics.Debug.Assert(statusBar != null);
            /*		statusBar.Panels.Clear();
            statusBar.Controls.Clear();
			
            foreach (StatusBarContributionItem item in Items) {
                if (item.Control != null) {
                    statusBar.Controls.Add(item.Control);
                } else if (item.Panel != null) {
                    statusBar.Panels.Add(item.Panel);
                } else {
                    throw new ApplicationException("StatusBarContributionItem " + item.ItemID + " has no Control or Panel defined.");
                }
            }*/
        }

        #region Progress Monitor
        static HashSet<StatusBarProgressMonitor> activeProgressMonitors = new HashSet<StatusBarProgressMonitor>();
        static StatusBarProgressMonitor currentProgressMonitor;

        public static IProgressMonitor CreateProgressMonitor()
        {
            System.Diagnostics.Debug.Assert(statusBar != null);
            return new StatusBarProgressMonitor();
        }

        sealed class StatusBarProgressMonitor : IProgressMonitor
        {
            int workDone, totalWork;

            public int WorkDone
            {
                get { return workDone; }
                set
                {
                    if (workDone == value)
                        return;
                    workDone = value;
                    lock (activeProgressMonitors)
                    {
                        if (currentProgressMonitor == this)
                        {
                            UpdateDisplay();
                        }
                    }
                }
            }

            void UpdateDisplay()
            {
                statusBar.DisplayProgress(taskName, workDone, totalWork);
            }

            string taskName;

            public string TaskName
            {
                get { return taskName; }
                set
                {
                    if (taskName == value)
                        return;
                    taskName = value;
                    lock (activeProgressMonitors)
                    {
                        if (currentProgressMonitor == this)
                        {
                            UpdateDisplay();
                        }
                    }
                }
            }

            public bool ShowingDialog { get; set; }

            public bool IsCancelled
            {
                get { return false; }
            }

            public void BeginTask(string name, int totalWork, bool allowCancel)
            {
                lock (activeProgressMonitors)
                {
                    activeProgressMonitors.Add(this);
                    currentProgressMonitor = this;
                    this.taskName = name;
                    this.workDone = 0;
                    this.totalWork = totalWork;
                    UpdateDisplay();
                }
            }

            public void Done()
            {
                lock (activeProgressMonitors)
                {
                    activeProgressMonitors.Remove(this);
                    if (currentProgressMonitor == this)
                    {
                        if (activeProgressMonitors.Count > 0)
                        {
                            currentProgressMonitor = activeProgressMonitors.First();
                            currentProgressMonitor.UpdateDisplay();
                        }
                        else
                        {
                            currentProgressMonitor = null;
                            statusBar.HideProgress();
                        }
                    }
                }
            }

            public event EventHandler Cancelled { add { } remove { } }
        }
        #endregion
    }
}
