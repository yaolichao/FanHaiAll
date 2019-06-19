using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using FanHai.Hemera.Services.WipJobAutoTrack;

namespace FanHai.Hemera.Services.WipJobAutoTrack
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
				new WipJobAutoTrackService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
