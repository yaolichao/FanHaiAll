/*
<FileInfo>
  <Author>Jack.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.ServiceProcess;
using System.Collections.Generic;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Modules;
#endregion

namespace FanHai.Hemera.Services.Main
{
    public partial class AppService : ServiceBase
    {
        public AppService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Thread thread = new Thread(new ThreadStart(RemotingServer.Start));
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
        }
    }
}
