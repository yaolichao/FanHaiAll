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
using System.ServiceProcess;
#endregion

namespace FanHai.Hemera.Services.Main
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new AppService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
