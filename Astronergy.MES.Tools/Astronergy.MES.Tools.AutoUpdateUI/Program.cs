using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Astronergy.MES.Tools.AutoUpdateUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string []args)
        {
            string exeDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "AutoUpdateUI" + exeDir.GetHashCode(), out createdNew))
            {
                if (!createdNew)
                {
                    // multiple calls in parallel?
                    // it's sufficient to let one call run, so just wait for the other call to finish
                    try
                    {
                        mutex.WaitOne(10000);
                    }
                    catch (AbandonedMutexException)
                    {
                    }
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                string baseUrl = string.Empty;
                string applicationName = string.Empty;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--baseurl" && i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                    {
                        baseUrl = args[i + 1];
                    }
                }
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--appname" && i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                    {
                        applicationName = args[i + 1];
                    }
                }
                Application.Run(new frmUpdate(baseUrl, applicationName));
            }
        }


    }
}
