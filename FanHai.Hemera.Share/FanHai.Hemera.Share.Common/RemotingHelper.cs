#region using
using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization.Formatters;
#endregion


namespace FanHai.Hemera.Share.Common
{
    /// <summary>
    /// 远程调用帮助类。
    /// </summary>
    public class RemotingHelper
    {
        /// <summary>
        /// 远程服务端IP地址。
        /// </summary>
        private const string NODE_IP = "ip";
        /// <summary>
        /// 信道类型。
        /// </summary>
        private const string NODE_TYPE = "type";
        /// <summary>
        /// 远程服务端端口号。
        /// </summary>
        private const string NODE_PORT = "port";
        /// <summary>
        /// 远程服务端信道名称。
        /// </summary>
        private const string NODE_NAME = "name";
        /// <summary>
        /// 应用程序集路径。
        /// </summary>
        private const string NODE_PATH = "path";
        /// <summary>
        /// 应用程序集文件名称。
        /// </summary>
        private const string NODE_ASSEMBLY = "assembly";
        /// <summary>
        /// 应用程序集类名。包含命名空间。
        /// </summary>
        private const string NODE_CLASS = "class";
        /// <summary>
        /// 远程对象接口名。
        /// </summary>
        private const string NODE_INTERFACE = "interface";
        /// <summary>
        /// 远程对象的Url。
        /// </summary>
        private const string NODE_URL = "url";
        /// <summary>
        /// 远程对象注册模式。
        /// </summary>
        private const string NODE_MODEL = "model";
        /// <summary>
        /// 客户端默认IP地址。
        /// </summary>
        private const string NODE_CLIENT_IP = "clientIp";
        /// <summary>
        /// 客户端默认端口。
        /// </summary>
        private const string NODE_CLIENT_PORT = "clientPort";
        /// <summary>
        /// 远程服务器IP地址。
        /// </summary>
        private static string remoteIp = string.Empty;
        /// <summary>
        /// 注册Remoting信道。
        /// </summary>
        /// <param name="props">信道属性集合</param>
        /// <param name="portType">信道类别</param>
        public static void EventModeRegister(IDictionary props, string portType)
        {
            try
            {
                IChannel channel = null;
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                string name = portType;
                if (props.Contains("name"))
                {
                    name = Convert.ToString(props["name"]);
                }
                else if (props.Contains("port"))
                {
                    name = string.Format("{0}{1}",name,Convert.ToString(props["port"]));
                    props.Add("name", name);
                }
                if (!IsExistChannel(name))
                {
                    switch (portType.ToLower())
                    {
                        case "tcp":  //注册TCP信道                           
                            channel = new TcpChannel(props, clientProvider, serverProvider);
                            ChannelServices.RegisterChannel(channel, false);
                            break;
                        case "ipc": //注册IPC信道                          
                            channel = new IpcChannel(props, clientProvider, serverProvider);
                            ChannelServices.RegisterChannel(channel, false);
                            break;
                        case "http"://注册HTTP信道                            
                            channel = new HttpChannel(props, clientProvider, serverProvider);
                            ChannelServices.RegisterChannel(channel, false);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 注册Remoting信道。
        /// </summary>
        /// <param name="name">信道名称</param>
        /// <param name="portType">信道类别</param>
        /// <param name="port">信道使用的端口号</param>
        public static void EventModeRegister(string name, string portType, int port)
        {
            IDictionary props = new Hashtable();
            props["name"] = name;
            props["port"] = port;
            EventModeRegister(props, portType);
        }
        /// <summary>
        /// 通过读取Remoting配置文件(XML格式)注册服务端信道。
        /// </summary>
        public static void EventModeServerRegister()
        {
            try
            {
                //获得RemotAPIServer.XML第一个节点信息
                XmlNode serverNode = GetXmlServerNode("/Servers/Server");
                if (serverNode != null)
                {
                    //获得RemotAPIServer.XML第一节点的子节点信息
                    XmlNodeList channelNodes = serverNode.SelectNodes("Channels/Channel");

                    foreach (XmlNode channelNode in channelNodes)
                    {
                        string strName = channelNode.SelectSingleNode(NODE_NAME).InnerText;
                        string strType = channelNode.SelectSingleNode(NODE_TYPE).InnerText;
                        string ip = string.Empty;
                        XmlNode ipNode=channelNode.SelectSingleNode(NODE_IP);
                        if (ipNode != null)
                        {
                            ip = ipNode.InnerText;
                        }
                        int strPort = Int32.Parse(channelNode.SelectSingleNode(NODE_PORT).InnerText);

                        IDictionary props = new Hashtable();
                        if (!string.IsNullOrEmpty(strName))
                        {
                            props["name"] = strName;
                        }
                        if (!string.IsNullOrEmpty(strType))
                        {
                            props["port"] = strPort;
                        }
                        if (!string.IsNullOrEmpty(ip))
                        {
                            props["bindTo"] = ip;
                        }
                        //根据子节点信息，循环注册配置的通道
                        RemotingHelper.EventModeRegister(props, strType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取远程对象实例。
        /// </summary>
        /// <param name="t">远程对象类型。</param>
        /// <returns>远程对象实例。</returns>
        public static object GetObject(Type t)
        {
            try
            {
                string RemotingURI = GetRemotingUrl() + t.FullName;
                object o = Activator.GetObject(t, RemotingURI);
                return o;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///通过读取Remoting配置文件(XML格式)注册客户端信道。
        /// </summary>
        public static void EventModeClientRegister()
        {
            try
            {
                XmlNode serverNode = GetXmlServerNode("/Servers/Server");

                if (serverNode != null)
                {
                    string type = serverNode.Attributes[NODE_TYPE].Value;
                    string clientIp = string.Empty;
                    XmlAttribute clientIpAttr = serverNode.Attributes[NODE_CLIENT_IP];
                    if (clientIpAttr != null)
                    {
                        clientIp = clientIpAttr.Value;
                    }
                    string port = "0";
                    XmlAttribute clientPortAttr =serverNode.Attributes[NODE_CLIENT_PORT];
                    if (clientPortAttr != null)
                    {
                        port = clientPortAttr.Value;
                    }
                    IDictionary props = new Hashtable();
                    props["name"] = string.Format("{0}{1}",type,port);
                    if (!string.IsNullOrEmpty(port))
                    {
                        props["port"] = port;
                    }
                    if (!string.IsNullOrEmpty(clientIp))
                    {
                        props["bindTo"] = clientIp;
                    }
                    RemotingHelper.EventModeRegister(props, type);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 判断是否存在相同名称的信道。
        /// </summary>
        /// <returns>true:已存在相同名称的信道。false:不存在相同名称的信道。</returns>
        public static bool IsExistChannel(string channelName)
        {
            try
            {
                IChannel[] channels = ChannelServices.RegisteredChannels;
                foreach (IChannel eachChannel in channels)
                {
                    if (eachChannel.ChannelName == channelName)
                        return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
        /// <summary>
        /// 通过配置文件（XML格式）注册远程对象。
        /// </summary>
        public static void RegisterRemoteObjectViaXmlConfig()
        {
            try
            {
                XmlNode serverNode = GetXmlServerNode("/Servers/Server");
                if (serverNode != null)
                {
                    XmlNodeList componentNodes = serverNode.SelectNodes("Components/Component");
                    foreach (XmlNode componentNode in componentNodes)
                    {
                        string strMod = componentNode.SelectSingleNode(NODE_MODEL).InnerText;
                        string strUrl = componentNode.SelectSingleNode(NODE_URL).InnerText;
                        string strClass = componentNode.SelectSingleNode(NODE_CLASS).InnerText;
                        RegisterRemoteObject(strClass, strUrl, strMod);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 注册远程对象。
        /// </summary>
        /// <param name="type">远程对象类型。</param>
        /// <param name="url">远程对象的Url。</param>
        /// <param name="model">远程对象的注册模式。SingleCall 或者 Singleton</param>
        /// <returns>true：注册成功。false：注册失败。</returns>
        public static bool RegisterRemoteObject(string type, string url, string model)
        {
            try
            {
                WellKnownObjectMode objMod = (WellKnownObjectMode)Enum.Parse(typeof(WellKnownObjectMode), model);
                RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType(type), url, objMod);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        /// <summary>
        /// 根据服务端站点名称选择服务器端IP地址。
        /// </summary>
        /// <param name="name">服务端站点名称。</param>
        public static void SelectServerSite(string name)
        {
            try
            {
                XmlNode serverNode = GetXmlServerNode("/Servers/Server");

                if (serverNode != null)
                {
                    XmlNodeList componentNodes = serverNode.SelectNodes("Sites/Site");
                    foreach (XmlNode componentNode in componentNodes)
                    {
                        string siteName = componentNode.ChildNodes[0].InnerText;
                        string siteIp = componentNode.ChildNodes[1].InnerText;
                        if (siteName.Trim() == name.Trim())
                        {
                            remoteIp = siteIp;
                            return;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据服务端站点名称获取服务器端IP地址。
        /// </summary>
        /// <param name="name">服务端站点名称。</param>
        /// <returns>服务器端IP地址。</returns>
        public static string GetServerIP(string name)
        {
            string remoteIP = string.Empty;
            try
            {
                XmlNode serverNode = GetXmlServerNode("/Servers/Server");

                if (serverNode != null)
                {
                    XmlNodeList componentNodes = serverNode.SelectNodes("Sites/Site");
                    foreach (XmlNode componentNode in componentNodes)
                    {
                        string siteName = componentNode.ChildNodes[0].InnerText;
                        string siteIp = componentNode.ChildNodes[1].InnerText;
                        if (siteName.Trim() == name.Trim())
                        {
                            remoteIP = siteIp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return remoteIP;
        }
        /// <summary>
        /// 选择的服务器端IP地址。
        /// </summary>
        /// <param name="IP">指定的服务端IP地址。</param>
        public static void SelectServerIP(string IP)
        {
            try
            {
                remoteIp = IP;
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取正在使用的服务器IP地址。
        /// </summary>
        /// <returns>服务器IP地址字符串。</returns>
        public static string GetUsingServerIP()
        {
            return remoteIp;
        }
        /// <summary>
        /// 读取远程调用的配置文件（XML格式）。
        /// </summary>
        /// <param name="strNode">节点路径。</param>
        /// <returns>节点路径指定的XML节点对象。</returns>
        public static XmlNode GetXmlServerNode(string strNode)
        {
            try
            {
                //指定配置文件为RemoteAPIServer.xml
                string strXmlFile = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["EngineService"];
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(strXmlFile);
                //得到第一个节点，server的信息
                XmlNode serverNode = xDoc.SelectSingleNode(strNode);
                return serverNode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通过XML文件获取远程对象的Url字符串。
        /// </summary>
        /// <returns>Url字符串。</returns>
        public static string GetRemotingUrl()
        {
            try
            {
                XmlNode serverNode = GetXmlServerNode("/Servers/Server");
                if (serverNode != null)
                {
                    //默认远程地址。
                    string strIp = serverNode.Attributes[NODE_IP].Value;
                    string portType = serverNode.Attributes[NODE_TYPE].Value;
                      int nPort = int.Parse(serverNode.Attributes[NODE_PORT].Value);
                    //选择的远程地址。
                    if (remoteIp != string.Empty)
                    {
                        return portType + "://" + remoteIp + ":" + nPort + "/";
                    }
                    else
                    {
                        return portType + "://" + strIp + ":" + nPort + "/";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
