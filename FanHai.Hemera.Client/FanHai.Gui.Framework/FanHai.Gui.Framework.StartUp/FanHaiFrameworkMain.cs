//应用程序入口 

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Smfa;
using FanHai.Gui.Framework.Gui;
using System.Threading;
using FanHai.Gui.Framework;
using System.Net;
using System.Runtime.Remoting.Channels;
using Astronergy.AMES.Resource;


namespace FanHai.Hemera
{

    /// <summary>
    /// 应用程序入口点类
    /// </summary>
    public class SolarViewerFrameworkMain
    {
        /// <summary>
        /// 应用程序命令行参数。
        /// </summary>
        static string[] _commandLineArgs = null;
        /// <summary>
        /// 获取应用程序命令行参数。
        /// </summary>
        public static string[] CommandLineArgs
        {
            get
            {
                return _commandLineArgs;
            }
        }

        /// <summary>
        /// 应用程序入口点方法，启动应用程序。
        /// </summary>
        [STAThread()]
        public static void Main(string[] args)
        {
#pragma warning disable
            ResourceUtility.InitResources();
#if DEBUG
            //为应用程序启用可是样式
            Application.EnableVisualStyles();
            //是否以Debug模式运行
            if (Debugger.IsAttached)
            {
                Run(args);
                return;
            }
#endif
#if !DEBUG
            string exeDir = Path.GetDirectoryName(typeof(SolarViewerFrameworkMain).Assembly.Location);
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "StartUp" + exeDir.GetHashCode(), out createdNew))
            {
                if (!createdNew)
                {
                    return;
                }
            
                //判断参数是否包含 "/noupdate"。
                bool bStartAutoUpdate = true;
                foreach (string arg in args)
                {
                    if (arg == "/noupdate")
                    {
                        bStartAutoUpdate = false;
                        break;
                    }
                }
                //需要更新。
                if (bStartAutoUpdate)
                {
                    StartAutoUpdate();
                    return;
                }
#endif
            // Do not use LoggingService here (see comment in Run(string[]))
            try
            {
                Run(args);
            }
            catch (Exception ex)
            {
                try
                {
                    HandleMainException(ex);
                }
                catch (Exception loadError)
                {
                    MessageBox.Show(loadError.ToString(), "Critical error (Logging service defect?)");
                }
            }
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex">异常对象.</param>
        static void HandleMainException(Exception ex)
        {
            LoggingService.Fatal(ex);
            try
            {
                Application.Run(new ExceptionBox(ex, "Unhandled exception terminated FanHai Framework", true));
            }
            catch
            {
                MessageBox.Show(ex.ToString(), "Critical error (cannot use ExceptionBox)");
            }
        }

        /// <summary>
        /// 使用指定参数运行程序。
        /// </summary>
        /// <param name="args">The args.</param>
        static void Run(string[] args)
        {

#if DEBUG
            Control.CheckForIllegalCrossThreadCalls = true;
#endif
            _commandLineArgs = args;
            bool noLogo = false;
            //用于新控件的默认值。如果为 true，则支持 UseCompatibleTextRendering 的新控件将使用 GDI 进行文本呈现；如果为 false，则新控件使用 GDI+。
            Application.SetCompatibleTextRenderingDefault(false);

            SplashScreenForm.SetCommandLineArgs(args);
            //是否显示SplashScreenForm
            foreach (string parameter in SplashScreenForm.GetParameterList())
            {
                if ("nologo".Equals(parameter, StringComparison.OrdinalIgnoreCase))
                    noLogo = true;
            }

            //显示启动画面
            if (!noLogo)
            {
                SplashScreenForm.ShowSplashScreen();
                Thread.Sleep(2000);
            }

            try
            {
                //System.Diagnostics.Process.Start(FileUtility.ApplicationRootPath + @"\UpdateFile\AppAutoUpdater.exe");
                RunApplication();
            }
            finally
            {
                if (SplashScreenForm.SplashScreen != null)
                {
                    SplashScreenForm.SplashScreen.Dispose();
                }
            }
        }

        /// <summary>
        /// 运行应用程序。
        /// </summary>
        static void RunApplication()
        {
            // The output encoding differs based on whether FanHai Gui Framework is a console app (debug mode)
            // or Windows app (release mode). Because this flag also affects the default encoding
            // when reading from other processes' standard output, we explicitly set the encoding to get
            // consistent behaviour in debug and release builds of SolarViewerFramework.

#if DEBUG
            // Console apps use the system's OEM codepage, windows apps the ANSI codepage.
            // We'll always use the Windows (ANSI) codepage.
            try
            {
                Console.OutputEncoding = System.Text.Encoding.Default;
            }
            catch (IOException)
            {
                // can happen if FanHai Gui Framework doesn't have a console
            }
#endif

            LoggingService.Info("Starting MES Gui Framework...");
            try
            {
                StartupSettings startup = new StartupSettings();
#if DEBUG
                startup.UseSolarViewerFrameworkErrorHandler = !Debugger.IsAttached;
#endif
                Assembly exe = typeof(SolarViewerFrameworkMain).Assembly;

                startup.ApplicationRootPath = Path.GetDirectoryName(exe.Location);
                startup.AllowUserAddIns = true;
                string configDirectory = ConfigurationManager.AppSettings["settingsPath"];
                //如果在config文件中没有配置settingsPath或配置的字符串为空。
                if (String.IsNullOrEmpty(configDirectory))
                {
                    startup.ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                           "CHINT/AMES.1.0");
                }
                else
                {
                    startup.ConfigDirectory = Path.Combine(Path.GetDirectoryName(exe.Location), configDirectory);
                }

                startup.DomPersistencePath = ConfigurationManager.AppSettings["domPersistencePath"];
                if (string.IsNullOrEmpty(startup.DomPersistencePath))
                {
                    startup.DomPersistencePath = Path.Combine(Path.GetTempPath(), "FanHai.Hemera.Gui.StartUp" + RevisionClass.MainVersion);
#if DEBUG
                    startup.DomPersistencePath = Path.Combine(startup.DomPersistencePath, "Debug");
#endif
                }
                else if (startup.DomPersistencePath == "none")
                {
                    startup.DomPersistencePath = null;
                }

                //添加插件
                startup.AddAddInsFromDirectory(Path.Combine(startup.ApplicationRootPath, "AddIns"));

                foreach (string parameter in SplashScreenForm.GetParameterList())
                {//如果参数以addindir:开头,将同样载入插件
                    if (parameter.StartsWith("addindir:", StringComparison.OrdinalIgnoreCase))
                    {
                        startup.AddAddInsFromDirectory(parameter.Substring(9));
                    }
                }
                startup.ResourceAssemblyName = exe.FullName;
                SolarViewerHost host = new SolarViewerHost(AppDomain.CurrentDomain, startup);

                //隐藏启动画面
                if (SplashScreenForm.SplashScreen != null)
                {
                    SplashScreenForm.SplashScreen.Hide();
                }
                string[] fileList = SplashScreenForm.GetRequestedFileList();
                if (fileList.Length > 0)
                {
                }

                host.BeforeRunWorkbench += delegate
                {
                    if (SplashScreenForm.SplashScreen != null)
                    {
                        SplashScreenForm.SplashScreen.BeginInvoke(new MethodInvoker(SplashScreenForm.SplashScreen.Dispose));
                        SplashScreenForm.SplashScreen = null;
                    }
                };

                WorkbenchSettings workbenchSettings = new WorkbenchSettings();
                workbenchSettings.RunOnNewThread = false;
                for (int i = 0; i < fileList.Length; i++)
                {
                    workbenchSettings.InitialFileList.Add(fileList[i]);
                }
                //登陆对话框
                LoginDialog login = new LoginDialog();
                login.ShowDialog();

                if (LoginDialog.flag)
                {
                    LoggingService.CloseCmd();
                    return;
                }
                host.RunWorkbench(workbenchSettings);
            }
            finally
            {
                //清除注册时间。
                IChannel[] channels = ChannelServices.RegisteredChannels;
                foreach (IChannel eachChannel in channels)
                {
                    ChannelServices.UnregisterChannel(eachChannel);
                }
                LoggingService.Info("Leaving RunApplication()");
            }
        }

        /// <summary>
        /// 启动自动下载。
        /// </summary>
        public static bool StartAutoUpdate()
        {
            string workDirectory = Directory.GetCurrentDirectory();
            string fileName = "AutoUpdateUI.exe";
            string configFileName = "AutoUpdateUI.exe.config";
            string fileFullPath = Path.Combine(workDirectory, fileName);

            string baseUrl = System.Configuration.ConfigurationManager.AppSettings["AutoUpdateBaseUrl"];     //自动更新程序URL
            string version = System.Configuration.ConfigurationManager.AppSettings["AutoUpdateUI.Version"];  //自动更新程序最新版本号。
            bool bFileExists = File.Exists(fileFullPath);
            string curFileVersion = string.Empty;
            if (bFileExists)
            {
                FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(fileFullPath);
                curFileVersion = fileVersion.FileVersion;
            }
            //如果自动更新程序的最新版本>当前文件版本。
            if (version.CompareTo(curFileVersion) > 0)
            {
                string[] urls = baseUrl.Split('|');
                //下载自动更新程序
                if (!DownloadFile(workDirectory, urls[0], fileName))
                {
                    if (!DownloadFile(workDirectory, urls[1], fileName))
                    {
                        MessageBox.Show("下载失败，请检查网络是否已连接。");
                    }
                }
                //下载自动更新程序配置文件
                if (!DownloadFile(workDirectory, urls[0], configFileName))
                {
                    if (!DownloadFile(workDirectory, urls[1], configFileName))
                    {
                        MessageBox.Show("下载失败，请检查网络是否已连接。");
                    }
                }
            }
            //启动自动更新程序。
            using (Process process = new System.Diagnostics.Process())
            {
                string args = string.Format("--baseurl {0}", baseUrl);
                process.StartInfo.FileName = "AutoUpdateUI.exe";
                process.StartInfo.Arguments = args;
                //必须禁用操作系统外壳程序
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                if (process.Start())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 下载更新文件。
        /// </summary>
        /// <param name="url">更新的文件地址。</param>
        /// <param name="downPath">文件存放路径。</param>
        /// <param name="fileName">文件名称。</param>
        public static bool DownloadFile(string workDirectory, string baseUrl, string fileName)
        {
            try
            {
                string filePath = fileName.TrimStart('\\');
                string fileFullPath = Path.Combine(workDirectory, filePath);
                string url = Path.Combine(baseUrl, filePath.Replace('\\', '/'));

                string dir = Path.GetDirectoryName(fileFullPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                WebRequest req = WebRequest.Create(url);
                //局域网内，不使用代理。
                req.Proxy = new WebProxy();
                WebResponse res = req.GetResponse();
                long fileLength = res.ContentLength;
                if (fileLength > 0)
                {
                    using (Stream srm = res.GetResponseStream())
                    {
                        StreamReader srmReader = new StreamReader(srm);
                        byte[] bufferbyte = new byte[fileLength];
                        int allByte = (int)bufferbyte.Length;
                        int startByte = 0;
                        while (fileLength > 0)
                        {
                            int downByte = srm.Read(bufferbyte, startByte, allByte);
                            if (downByte == 0) { break; };
                            startByte += downByte;
                            allByte -= downByte;
                        }
                        using (FileStream fs = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(bufferbyte, 0, bufferbyte.Length);
                            fs.Close();
                        }
                        srmReader.Close();
                        srm.Close();
                    }
                }
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }
    }
}
