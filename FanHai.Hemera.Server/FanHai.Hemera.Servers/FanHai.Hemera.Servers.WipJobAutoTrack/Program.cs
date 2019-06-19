using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;


using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Modules;
using FanHai.Hemera.Modules.WipJob;
using FanHai.Hemera.Modules.Wip;
using FanHai.Hemera.Utils;

namespace FanHai.Hemera.Servers.WipJobAutoTrack
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServerHost host = new ServerHost();
            host.Run();
            
        }
    }

    internal class ServerHost
    {
        private static bool servicePaused = false;
        private static int AutoTrackTime = 60 * 1000;
        private static List<string> lstKey = new List<string>();
        static IServerObjFactory isof = new ServerObjFactory();

        public void Run()
        {
            LogService.LogInfo("WipJobAutoTrackServer Begin.");
            try
            {
                AutoTrackTime = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["AutoTrackTime"]);
                servicePaused = false;
                Thread thread = new Thread(new ThreadStart(ThreadGetStepKey));
                thread.Start();
            }
            catch (Exception ex)
            {
                Thread threadError = new Thread(new ParameterizedThreadStart(LogService.LogError));
                threadError.Start(ex);
            }
            Console.ReadLine();
        }
        static void DoWork(object state)
        {
            string strKey = (string)state;
            try
            {
                if (strKey != null && strKey.Length > 0)
                {
                    LotOperationEngine we = new LotOperationEngine();
                    DataSet dt = we.AutoTrackOut(strKey);
                    int code = 0;
                    string strMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dt, ref code);
                    lock (lstKey)
                    {
                        lstKey.Remove(strKey);
                    }
                    if (strMsg.Length > 0)
                    {
                        Console.WriteLine(strMsg);
                    }
                }
                else
                {
                    LogService.LogInfo("Thread reporting for No duty.");
                }
            }
            catch (Exception ex)
            {
                Thread threadError = new Thread(new ParameterizedThreadStart(LogService.LogError));
                threadError.Start(ex);
            }
        }

        static void ThreadGetStepKey()
        {
            while (true)
            {
                while (!servicePaused)
                {
                    try
                    {
                        
                        DataSet ds = new DataSet();
                        List<string> lstNewKey = new List<string>();
                        ds = isof.CreateIWipJobAutoTrack().GetWaitingForTrackOutJobs();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                lstNewKey.Add(ds.Tables[0].Rows[i]["ROW_KEY"].ToString());
                            }
                            var resint = lstNewKey.Except(lstKey);
                            foreach (string item in resint)
                            {
                                string a = item;
                                lock (lstKey)
                                {
                                    lstKey.Add(a);
                                }
                                ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), a);
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Thread threadError = new Thread(new ParameterizedThreadStart(LogService.LogError));
                        threadError.Start(ex);
                    }
                    Thread.Sleep(AutoTrackTime);
                }
            }
        }
    }
}
