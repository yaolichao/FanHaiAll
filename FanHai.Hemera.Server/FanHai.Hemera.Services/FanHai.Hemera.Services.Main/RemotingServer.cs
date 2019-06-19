/*
<FileInfo>
  <Author>Jack.Liu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;

using SolarViewer.Hemera.Share.Common;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Modules.Common;
using SolarViewer.Hemera.Modules;
using System.Runtime.Remoting;
using SolarViewer.Hemera.Utils;
#endregion


namespace SolarViewer.Hemera.Services.Main
{
    public class RemotingServer
    {
        //Define static Notifier object
        public static Notifier n = null;
        public static ServerObjFactory f = null;

        /// <summary>
        /// Remoting Start
        /// </summary>
        public static void Start()
        {
            try
            {
                if (RegisterChannel())
                {
                    RegisterRemoteObject();
                    Run();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
            }
        }


        /// <summary>
        /// Register remoting channel
        /// </summary>
        /// <returns>bool</returns>
        public static bool RegisterChannel()
        {
            try
            {
                RemotingHelper.EventModeServerRegister();
                return true;
            }
            catch (Exception ex)
            {
                LogService.LogError("Register remoting server channel exception : " + ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Register remoting object
        /// </summary>
        /// <returns>bool</returns>
        public static bool RegisterRemoteObject()
        {
            try
            {
                if (RemotingConfiguration.CustomErrorsMode != CustomErrorsModes.Off)
                {
                    RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                }

                //RemotingHelper.RegisterRemoteObjectViaXmlConfig();
                f = new ServerObjFactory();
                RemotingServices.Marshal(f, typeof(IServerObjFactory).FullName);

                n = new Notifier();
                RemotingServices.Marshal(n, typeof(INotifier).FullName);
                return true;
            }
            catch (Exception ex)
            {
                LogService.LogError("Register remoting server object exception : " + ex.ToString());
            }

            return false;
        }

       
        /// <summary>
        /// UnRegister remoting object via Remoting url
        /// </summary>
        /// <param name="url">Remoting url</param>
        /// <returns>bool</returns>
        public bool UnRegisterRemoteObject(string url)
        {
            Type type = RemotingServices.GetServerTypeForUri(url);
            if (type != null)
            {
                MarshalByRefObject obj = (MarshalByRefObject)Activator.CreateInstance(type);
                RemotingServices.Disconnect(obj);
            }
            return true;
        }

        /// <summary>
        /// UnRegister remoting object via Remoting object type
        /// </summary>
        /// <param name="type">Remoting object type</param>
        /// <returns>bool</returns>
        public bool UnRegisterRemoteObject(Type type)
        {
            if (type != null)
            {
                MarshalByRefObject obj = (MarshalByRefObject)Activator.CreateInstance(type);
                RemotingServices.Disconnect(obj);
            }
            return true;
        }


        /// <summary>
        /// close channel which specify the name
        /// </summary>
        /// <param name="channelName">channelName</param>
        /// <returns>bool</returns>
        public bool UnregisterChannelByName(string channelName)
        {
            IChannel[] channels = ChannelServices.RegisteredChannels;

            foreach (IChannel eachChannel in channels)
            {
                if (eachChannel.ChannelName == channelName)
                {
                    ChannelServices.UnregisterChannel(eachChannel);
                }
            }

            return true;

        }

        /// <summary>
        /// Remoting server run
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Remoting Server is Ready ...");
            string s;
            while ((s = Console.ReadLine()) != "")
            {
                n.BroadCast(s);
            }
            Console.WriteLine("Server shutdown ...");
        }
    }
}
