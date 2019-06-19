/*
<FileInfo>
  <Author>ZhangHao FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using FanHai.Hemera.Share.Interface;
using System.Reflection;
using FanHai.Hemera.Share.Common;
using System.Data;
#endregion

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 远程调用服务类。
    /// </summary>
    public class CallRemotingService : MarshalByRefObject
    {
        /// <summary>
        /// 远程服务器的IP地址。
        /// </summary>
        private const string NODE_IP = "ip";              //xml node ip
        /// <summary>
        /// 远程调用的协议类型。
        /// </summary>
        private const string NODE_TYPE = "type";          //xml node protocal
        /// <summary>
        /// 远程调用的端口号。
        /// </summary>
        private const string NODE_PORT = "port";          //xml node port
        /// <summary>
        /// 远程对象的Url地址。
        /// </summary>
        private const string NODE_URL = "url";            //xml node url
        /// <summary>
        /// 应用程序集路径。
        /// </summary>
        private const string NODE_PATH = "path";          //xml node path
        /// <summary>
        /// 应用程序集文件名称。
        /// </summary>
        private const string NODE_ASSEMBLY = "assembly";  //xml node assembly
        /// <summary>
        /// 远程对象接口名。
        /// </summary>
        private const string NODE_INTERFACE = "interface";//xml node interface
        /// <summary>
        /// 服务器端消息广播对象。
        /// </summary>
        private static INotifier n = null;
        /// <summary>
        /// 创建远程对象。
        /// </summary>
        /// <returns>实现<see cref="IServerObjFactory"/>的远程对象实例。</returns>
        public static IServerObjFactory GetRemoteObject()
        {
            IChannel[] channels = ChannelServices.RegisteredChannels;
            if (channels.Length <= 0)
            {
                RemotingHelper.EventModeClientRegister();
            }
            return (IServerObjFactory)RemotingHelper.GetObject(typeof(IServerObjFactory));
        }
        /// <summary>
        /// 注册应用程序切换事件。
        /// </summary>
        /// <returns>true：注册成功。false：注册失败。</returns>
        public static bool RegisterAppSwitchEvent()
        {
            try
            {
                n = new NotifierProxy();
                n.Notify += new NotifyEventHandler(BroadCasting);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 注销应用程序切换事件。
        /// </summary>
        /// <returns>true：注销成功。false：注销失败。</returns>
        public static bool UnRegisterAppSwitchEvent()
        {
            try
            {
                n.Notify -= new NotifyEventHandler(BroadCasting);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 接收应用程序切换消息，用于切换服务器。
        /// </summary>
        /// <param name="info">应用程序切换消息。</param>
        private static void BroadCasting(string info)
        {
            if (info != null)
            {
                string computerName = string.Empty;

                switch (info.Trim())
                {
                    case "peter":
                        computerName = "HZCNN-008014";
                        break;
                    default:
                        break;
                }
                n.Notify -= new NotifyEventHandler(BroadCasting);
                SelectServerSite(computerName);
                RegisterAppSwitchEvent();
            }
        }
        /// <summary>
        /// 注册广播消息接收事件，用于接收广播消息。
        /// </summary>
        /// <returns>true：注册成功。false：注册失败。</returns>
        public static bool RegisterReceiveMessageEvent()
        {
            try
            {
                n = new NotifierProxy();
                n.Notify += new NotifyEventHandler(ReceiveMessageBrodCasting);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 接收广播消息的方法。
        /// </summary>
        /// <param name="info">接收到的广播消息。</param>
        private static void ReceiveMessageBrodCasting(string info)
        {
            if (info != null)
            {
                try
                {
                    string strMsg = "";
                    string computerName = string.Empty;

                    string[] split = info.Split(new Char[] { ':' });
                    if (split[0].ToUpper() == "SENDMSG")
                    {
                        if (split.Length > 1)
                        {
                            strMsg = split[1];
                            MessageService.ShowMessage(strMsg);
                        }
                    }
                }
                catch
                {

                }
            }
        }
        /// <summary>
        /// 获取登录时可以选择的服务端站点数据。
        /// </summary>
        /// <returns>包含服务端站点数据的键值对集合对象。</returns>
        public static Dictionary<string, string> QueryServerSite()
        {
            XmlNode serverNode = GetXmlServerNode("/Servers/Server");
            Dictionary<string, string> site = new Dictionary<string, string>();
            List<XmlNode> lst = new List<XmlNode>();
            if (serverNode != null)
            {
                XmlNodeList componentNodes = serverNode.SelectNodes("Sites/Site");
               
                foreach (XmlNode componentNode in componentNodes)
                {
                    lst.Add(componentNode);
                }
                IOrderedEnumerable<XmlNode> orderEnums=lst
                    .OrderBy(node => node.ChildNodes[0].InnerText)
                    .OrderBy(node => node.ChildNodes.Count > 2 ? node.ChildNodes[2].InnerText : string.Empty);
                foreach (XmlNode node in orderEnums)
                {
                    string siteName = node.ChildNodes[0].InnerText;
                    string siteIp = node.ChildNodes[1].InnerText;
                    site.Add(siteName, siteIp);
                }
            }
            return site;
        }
        /// <summary>
        /// 获取登录时可选择的语言数据。
        /// </summary>
        /// <returns>包含语言数据的键值对集合对象。</returns>
        public static IDictionary QueryLanguageOption()
        {
            XmlNode serverNode = GetXmlServerNode("/Servers/Server");
            Dictionary<string, string> language = new Dictionary<string, string>();

            if (serverNode != null)
            {
                XmlNodeList componentNodes = serverNode.SelectNodes("Languages/Language");
                foreach (XmlNode componentNode in componentNodes)
                {
                    string languageName = componentNode.ChildNodes[0].InnerText;
                    string languageSign = componentNode.ChildNodes[1].InnerText;
                    language.Add(languageName, languageSign);
                }
            }

            return language;
        }
        /// <summary>
        /// 根据服务器名称选择远程服务器站点的IP地址。
        /// </summary>
        /// <param name="name">服务器名称。</param>
        public static void SelectServerSite(string name)
        {
            RemotingHelper.SelectServerSite(name);
        }
        /// <summary>
        /// 设置远程服务器站点的IP地址。
        /// </summary>
        /// <param name="name">服务器站点IP地址。</param>
        public static void SelectServerIP(string IP)
        {
            RemotingHelper.SelectServerIP(IP);
        }
        /// <summary>
        /// 根据服务器名称获取远程服务器站点的IP地址。
        /// </summary>
        /// <param name="name">服务器名称。</param>
        /// <returns>服务器站点的IP地址。</returns>
        public static string GetServerIP(string name)
        {
            string serverIP = RemotingHelper.GetServerIP(name);
            return serverIP;
        }
        /// <summary>
        /// 获取正在使用的服务器站点的IP地址。
        /// </summary>
        /// <returns>服务器站点的IP地址。</returns>
        public static string GetUsingServerIP()
        {
            string usingServerIP = RemotingHelper.GetUsingServerIP();
            return usingServerIP;
        }
        /// <summary>
        /// 更新配置文件中的服务器站点数据。
        /// </summary>
        /// <param name="dictionary">包含服务器站点数据的键值对集合对象。</param>
        public static void UpdateConfigureSiteXml(IDictionary dictionary)
        {
            string strXmlFile = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["EngineService"];
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(strXmlFile);
            XmlNode serverNode = xDoc.SelectSingleNode("/Servers/Server");
            if (serverNode != null)
            {
                XmlNode sitesNode = serverNode.SelectSingleNode("Sites");
                sitesNode.RemoveAll();
                foreach (string node in dictionary.Keys)
                {
                    XmlElement siteNode = xDoc.CreateElement("Site");
                    XmlElement name = xDoc.CreateElement("name");
                    name.InnerText = node.Trim();
                    siteNode.AppendChild(name);

                    string[] values = dictionary[node] as string[];
                    XmlElement ip = xDoc.CreateElement("ip");
                    ip.InnerText = values[0].Trim();
                    siteNode.AppendChild(ip);

                    XmlElement code = xDoc.CreateElement("factoryCode");
                    code.InnerText = values[1].Trim();
                    siteNode.AppendChild(code);

                    sitesNode.AppendChild(siteNode);
                }
                serverNode.AppendChild(sitesNode);
            }
            xDoc.Save(strXmlFile);
        }
        /// <summary>
        /// 更新配置文件中的语言数据。
        /// </summary>
        /// <param name="dictionary">包含语言数据的键值对集合对象。</param>
        public static void UpdateConfigureLanguageXml(IDictionary dictionary)
        {
            string strXmlFile = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["EngineService"];
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(strXmlFile);
            XmlNode serverNode = xDoc.SelectSingleNode("/Servers/Server");
            if (serverNode != null)
            {
                XmlNode languagesNode = serverNode.SelectSingleNode("Languages");
                languagesNode.RemoveAll();
                foreach (string node in dictionary.Keys)
                {
                    XmlElement languageNode = xDoc.CreateElement("Language");
                    XmlElement name = xDoc.CreateElement("name");
                    name.InnerText = node;
                    languageNode.AppendChild(name);
                    XmlElement ip = xDoc.CreateElement("sign");
                    ip.InnerText = dictionary[node].ToString();
                    languageNode.AppendChild(ip);
                    languagesNode.AppendChild(languageNode);
                }
                serverNode.AppendChild(languagesNode);
            }
            xDoc.Save(strXmlFile);
        }
        /// <summary>
        /// 注销信道。
        /// </summary>
        /// <returns>true：注销成功。false：注销失败。</returns>
        public static bool UnregisterChannel()
        {
            //IChannel[] channels = ChannelServices.RegisteredChannels;
            //foreach (IChannel eachChannel in channels)
            //{
            //    if (eachChannel.ChannelName != string.Empty)
            //    {
            //        ChannelServices.UnregisterChannel(eachChannel);
            //        return true;
            //    }
            //}
            return true;
        }
        /// <summary>
        /// 读取远程调用的配置文件（XML格式）。
        /// </summary>
        /// <param name="strNode">节点路径。</param>
        /// <returns>节点路径指定的XML节点对象。</returns>
        private static XmlNode GetXmlServerNode(string strNode)
        {
            try
            {
                string strXmlFile = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["EngineService"];
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(strXmlFile);
                XmlNode serverNode = xDoc.SelectSingleNode(strNode);
                return serverNode;
            }
            catch (Exception e)
            {
                LoggingService.Error("GetXmlServerNode",e);
            }
            return null;
        }
        /// <summary>
        /// 获取控制此实例生存期策略的生存期服务对象。
        /// </summary>
        /// <returns>控制此实例生存期策略的生存期服务对象。</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

}
