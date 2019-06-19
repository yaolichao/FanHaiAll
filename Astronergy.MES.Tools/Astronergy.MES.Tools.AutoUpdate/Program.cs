using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
namespace Astronergy.MES.Tools.AutoUpdate
{
    class Program
    {
        static int Main(string[] args)
        {
            string exeDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "AutoUpdate" + exeDir.GetHashCode(), out createdNew))
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
                    return 0;
                }

                bool bOnlyOutput = false;
                string curDir = string.Empty;
                string baseUrl = string.Empty;
                string xmlName = "autoupdate.xml";
                int exitCode = 0;
                try
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "--workdirectory" && i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                            curDir = args[i + 1];
                        else if (args[i] == "/o")
                            bOnlyOutput = true;
                        else if (args[i] == "--baseurl" && i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                        {
                            baseUrl = args[i + 1];
                        }
                        else if (args[i] == "--xmlname" && i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                        {
                            xmlName = args[i + 1];
                        }
                    }
                    xmlName = "autoupdate.xml";
                    Console.WriteLine("开始更新：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //如果没有设置工作文件夹
                    if (string.IsNullOrEmpty(curDir))
                        curDir = Directory.GetCurrentDirectory();
                    else
                    {
                        curDir = Path.GetFullPath(curDir);
                        Directory.SetCurrentDirectory(curDir);
                    }
                    //仅输出自动更行XML文件。
                    if (bOnlyOutput)
                    {
                        AutoUpdateXmlSerializerUtil.Write(curDir, xmlName);
                        return exitCode;
                    }

                    AutoUpdater updater = new AutoUpdater(curDir, baseUrl);
                    AutoUpdateXmlFile oldFile = AutoUpdateXmlSerializerUtil.Get(curDir);
                    Console.WriteLine("正在连接服务器...");
                    updater.DownloadFile(xmlName);
                    AutoUpdateXmlFile newFile = AutoUpdateXmlSerializerUtil.Read(curDir, xmlName);
                    if (updater.Update(newFile, oldFile))
                    {
                        Console.WriteLine("更新成功。" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        Console.WriteLine("更新失败。" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        exitCode = 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("更新失败。" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    exitCode = 2;
                }
                return exitCode;
            }
        }

    }
}
