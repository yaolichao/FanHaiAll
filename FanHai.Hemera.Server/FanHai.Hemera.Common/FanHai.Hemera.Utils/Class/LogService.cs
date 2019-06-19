/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using 
using System;
using System.Text;

using log4net;
using log4net.Config;
using log4net.Appender;
using log4net.Core;
#endregion

namespace FanHai.Hemera.Utils
{

    public class LogService
    {
        public static log4net.ILog iLog = null;

        public static void InitLogService(string appenderName)
        {
            log4net.Config.XmlConfigurator.Configure();
            iLog = LogManager.GetLogger(appenderName);
        }

        public static void LogInfo(object message)
        {
            InitLogService("RollingFile");
            iLog.Info(message);
        }

        public static void LogError(object message)
        {
            InitLogService("ErrorFile");
            iLog.Error(message);
        }
    }
}
