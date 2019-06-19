#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;
using System.Data;
using System.Reflection;
using System.Collections;
using Autofac;
using Autofac.Builder;
using Autofac.Configuration;
using Autofac.Core;

using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
#endregion

namespace FanHai.Hemera.Modules
{
    /// <summary>
    /// 创建远程调用对象的工厂类。
    /// </summary>
    public class ServerObjFactory : AbstractEngine, IServerObjFactory
    {
        /// <summary>
        /// 容器
        /// </summary>
        IContainer container = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ServerObjFactory()
        {
            LogService.LogInfo("Create ServerObjFactory Object.");
            var builder = new ContainerBuilder();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string config = System.IO.Path.Combine(path, "config\\autofac-config.xml");
            if (System.IO.File.Exists(config))
            {
                IModule mConfig = new ConfigurationSettingsReader(ConfigurationSettingsReader.DefaultSectionName, config);
                builder.RegisterModule(mConfig);
            }
            container = builder.Build(ContainerBuildOptions.Default);
        }
        /// <summary>
        /// 析构函数。
        /// </summary>
        ~ServerObjFactory()
        {
            LogService.LogInfo("Collect ServerObjFactory Object.");
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 获取指定类型的对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>指定类型的对象。如果为NULL则代表没有进行注册。</returns>
        public T Get<T>()
        {
            if(container.IsRegistered(typeof(T))){
                return container.Resolve<T>();
            }
            else{
                return default(T);
            }
        }
        /// <summary>
        /// 执行指定类的指定方法。
        /// </summary>
        /// <param name="engineName">指定类的名称。</param>
        /// <param name="engineMethod">指定类中的方法。</param>
        /// <param name="engineParamValue">指定方法使用的参数。</param>
        /// <returns>包含执行结果的数据及对象。</returns>
        public DataSet ExecuteEngineMethod(string engineName, string engineMethod, DataSet engineParamValue)
        {
            Type engineType = Type.GetType(engineName, true, true);
            object engineObject = GetEngineObject(engineType);
            object engineReturnValue = engineType.InvokeMember(engineMethod, BindingFlags.InvokeMethod, null, engineObject, new object[] { engineParamValue });
            return engineReturnValue as DataSet;
        }

        private readonly Dictionary<string, object> engineObjectList = new Dictionary<string, object>();
        private readonly object endgineObjectListLock = new object();
        /// <summary>
        /// Get Specified Factory Engine Object
        /// </summary>
        /// <param name="engineType"></param>
        /// <returns></returns>
        private object GetEngineObject(Type engineType)
        {
            lock (endgineObjectListLock)
            {
                if (engineObjectList.ContainsKey(engineType.FullName))
                {
                    return engineObjectList[engineType.FullName];
                }
                else
                {
                    object engineObject = engineType.InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
                    engineObjectList.Add(engineType.FullName, engineObject);
                    return engineObject;
                }
            }
        }
    }
}
