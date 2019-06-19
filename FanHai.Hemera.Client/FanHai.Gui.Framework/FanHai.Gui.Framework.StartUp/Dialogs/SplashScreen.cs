
//启动画面，版本，应用程序启动时传入的参数集合，保存应用程序请求的文件集合
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using FanHai.Gui.Core;

namespace FanHai.Hemera
{
    /// <summary>
    /// 应用程序启动画面。
    /// </summary>
    /// <remarks>显示版本字符串、保存应用程序启动时传入的参数集合、保存应用程序请求的文件集合</remarks>
    public class SplashScreenForm : Form
    {
        /// <summary>
        /// 版本消息
        /// </summary>
        public const string VersionText = "MES: " + RevisionClass.FullVersion;

        static SplashScreenForm splashScreen;
        static List<string> requestedFileList = new List<string>();
        static List<string> parameterList = new List<string>();
        Bitmap bitmap;
        /// <summary>
        /// 获取启动画面窗体对象。
        /// </summary>
        public static SplashScreenForm SplashScreen
        {
            get
            {
                return splashScreen;
            }
            set
            {
                splashScreen = value;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SplashScreenForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
#if DEBUG
            string versionText = VersionText + " (debug)";
#else
			string versionText = VersionText;
#endif

            string[] names = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            bitmap = new Bitmap(typeof(SplashScreenForm).Assembly.GetManifestResourceStream("FanHai.Gui.Framework.StartUp.Resources.SplashScreen.jpg"));
            this.ClientSize = bitmap.Size;
            using (Font font = new Font("Sans Serif", 8,FontStyle.Italic|FontStyle.Bold))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawString(versionText, font, Brushes.White, 225, 300);
                }
            }
            BackgroundImage = bitmap;
        }
        /// <summary>
        /// 显示启动画面。
        /// </summary>
        public static void ShowSplashScreen()
        {
            splashScreen = new SplashScreenForm();
            splashScreen.Show();
        }

        /// <summary>
        /// 释放由 <see cref="T:System.Windows.Forms.Form"></see> 占用的资源（内存除外）。
        /// </summary>
        /// <param name="disposing">为 true 则释放托管资源和非托管资源；为 false 则仅释放非托管资源。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// 获取应用程序启动传入的参数集合。
        /// </summary>
        /// <returns>应用程序启动参数集合。</returns>
        public static string[] GetParameterList()
        {
            return parameterList.ToArray();
        }
        /// <summary>
        /// 获取应用程序启动请求的文件集合。
        /// </summary>
        /// <returns>应用程序启动文件集合。</returns>
        public static string[] GetRequestedFileList()
        {
            return requestedFileList.ToArray();
        }
        /// <summary>
        /// 设置启动参数和请求文件的集合。
        /// </summary>
        /// <param name="args">应用程序启动时的参数集合。</param>
        public static void SetCommandLineArgs(string[] args)
        {
            requestedFileList.Clear();
            parameterList.Clear();

            foreach (string arg in args)
            {
                if (arg.Length == 0) continue;
                if (arg[0] == '-' || arg[0] == '/')
                {
                    int markerLength = 1;

                    if (arg.Length >= 2 && arg[0] == '-' && arg[1] == '-')
                    {
                        markerLength = 2;
                    }

                    parameterList.Add(arg.Substring(markerLength));
                }
                else
                {
                    requestedFileList.Add(arg);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SplashScreenForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "SplashScreenForm";
            this.Shown += new System.EventHandler(this.SplashScreenForm_Shown);
            this.ResumeLayout(false);

        }

        private void SplashScreenForm_Shown(object sender, EventArgs e)
        {
           
        }
    }
}
