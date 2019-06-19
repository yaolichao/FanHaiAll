using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.Threading;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Modules;
using FanHai.Hemera.Modules.WipJob;
using FanHai.Hemera.Modules.Wip;
using FanHai.Hemera.Services.WipJobAutoTrack;

using FanHai.Hemera.Utils;

namespace FanHai.Hemera.Services.WipJobAutoTrack
{
    public partial class WipJobAutoTrackService : ServiceBase
    {
        private static bool servicePaused = false;
        private static int AutoTrackTime = 10 * 1000;
        private static List<string> lstKey = new List<string>();

        public WipJobAutoTrackService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
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
        }

        protected override void OnStop()
        {
            servicePaused = true;
        }

        protected override void OnPause()
        {
            servicePaused = true;
        }

        protected override void OnContinue()
        {
            servicePaused = false;
        }
 



        private static IServerObjFactory isof = new ServerObjFactory();
        private static List<string> StepKey = new List<string>();

        static void DoWork(object state)
        {
            string strKey = (string)state;
            try
            {
                if (strKey != null && strKey.Length > 0)
                {
                    DataSet dsReturn = new DataSet();
                    LotOperationEngine we = new LotOperationEngine();
                    dsReturn = we.AutoTrackOut(strKey);
                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);


                    lock (lstKey)
                    {
                        lstKey.Remove(strKey);
                    }
                    if (msg.Length > 0)
                    {
                        Thread threadLog= new Thread(new ParameterizedThreadStart(LogService.LogInfo));
                        threadLog.Start(msg);
                    }
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
