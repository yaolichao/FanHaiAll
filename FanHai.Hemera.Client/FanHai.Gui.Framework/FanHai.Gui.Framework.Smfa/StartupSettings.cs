using System;
using System.Collections.Generic;

namespace FanHai.Gui.Framework.Smfa
{
    /// <summary>
    /// 包含控制平台如何启动的设置数据
    /// </summary>
    [Serializable]
    public sealed class StartupSettings
    {
        bool useSolarViewerFrameworkErrorHandler = true;
        string applicationName = "FanHaiMES";
        string applicationRootPath;
        bool allowAddInConfigurationAndExternalAddIns = true;
        bool allowUserAddIns;
        string propertiesName;
        string configDirectory;
        string dataDirectory;
        string domPersistencePath;
        string resourceAssemblyName = "FanHai.Gui.Framework.StartUp";
        //添加Addin功能使用
        internal List<string> addInDirectories = new List<string>();
        internal List<string> addInFiles = new List<string>();

        /// <summary>
        /// Gets/Sets the name of the assembly to load the BitmapResources
        /// and English StringResources from.
        /// </summary>
        public string ResourceAssemblyName
        {
            get { return resourceAssemblyName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                resourceAssemblyName = value;
            }
        }

        /// <summary>
        /// Gets/Sets whether the SolarViewerFramework exception box should be used for
        /// 默认是True
        /// </summary>
        public bool UseSolarViewerFrameworkErrorHandler
        {
            get { return useSolarViewerFrameworkErrorHandler; }
            set { useSolarViewerFrameworkErrorHandler = value; }
        }

        /// <summary>
        /// Use the file <see cref="ConfigDirectory"/>\AddIns.xml to maintain
        /// a list of deactivated AddIns and list of AddIns to load from
        /// external locations.
        /// The default value is true.
        /// </summary>
        public bool AllowAddInConfigurationAndExternalAddIns
        {
            get { return allowAddInConfigurationAndExternalAddIns; }
            set { allowAddInConfigurationAndExternalAddIns = value; }
        }

        /// <summary>
        /// 允许用户插件存储在"application data"目录下。
        /// 默认值是false.
        /// </summary>
        public bool AllowUserAddIns
        {
            get { return allowUserAddIns; }
            set { allowUserAddIns = value; }
        }

        /// <summary>
        /// 获取或设置平台名称，该名称会被<see cref="MessageService"/>和窗口使用。
        /// 默认值 "SolarViewerFramework".
        /// </summary>
        public string ApplicationName
        {
            get { return applicationName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                applicationName = value;
            }
        }

        /// <summary>
        /// 获取或设置平台使用的根目录。
        /// 如果为ull（默认值)则用当前应用程序集目录。
        /// </summary>
        public string ApplicationRootPath
        {
            get { return applicationRootPath; }
            set { applicationRootPath = value; }
        }

        /// <summary>
        /// 获取或设置平台配置数据所在目录。该目录放置设置数据和用户插件。
        /// 如果为null(默认值)则用"ApplicationData\ApplicationName"存放配置数据。
        /// </summary>
        public string ConfigDirectory
        {
            get { return configDirectory; }
            set { configDirectory = value; }
        }

        /// <summary>
        /// Sets the data directory used to load resources.
        /// Use null (default) to use the default path "ApplicationRootPath\data".
        /// </summary>
        public string DataDirectory
        {
            get { return dataDirectory; }
            set { dataDirectory = value; }
        }

        /// <summary>
        /// Sets the name used for the properties file (without path or extension).
        /// Use null (default) to use the default name.
        /// </summary>
        public string PropertiesName
        {
            get { return propertiesName; }
            set { propertiesName = value; }
        }

        /// <summary>
        /// 设置用存储"代码完成缓存"的目录。
        /// 使用null（默认值）禁用代码完成缓存功能。
        /// </summary>
        public string DomPersistencePath
        {
            // Sets the directory used to store the code completion cache.
            //Use null (default) to disable the code completion cache.
            get { return domPersistencePath; }
            set { domPersistencePath = value; }
        }

        /// <summary>
        /// 从文件夹中添加插件，递归搜索<paramref name="addInDir"/>查找所有.addin文件。
        /// </summary>
        public void AddAddInsFromDirectory(string addInDir)
        {
            if (addInDir == null)//抛出空参数导致的异常
                throw new ArgumentNullException("addInDir");

            addInDirectories.Add(addInDir);
        }

        /// <summary>
        /// 添加指定的 .addin 文件。
        /// </summary>
        public void AddAddInFile(string addInFile)
        {
            if (addInFile == null)
                throw new ArgumentNullException("addInFile");
            addInFiles.Add(addInFile);
        }
    }
}
