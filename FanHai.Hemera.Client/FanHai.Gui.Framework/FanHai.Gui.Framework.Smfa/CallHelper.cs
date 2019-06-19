using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;

using FanHai.Gui.Core;
using FanHai.Gui.Core.WinForms;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Commands;
using FanHai.Gui.Core.Services;

namespace FanHai.Gui.Framework.Smfa
{
    /// <summary>
    /// 远程调用，可远程调用CallHelper
    /// </summary>
    internal sealed class CallHelper : MarshalByRefObject
    {
        SolarViewerHost.CallbackHelper callback;
        bool useSolarViewerFrameworkErrorHandler;


        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region Initialize Core
        public void InitMESCore(SolarViewerHost.CallbackHelper callback, StartupSettings properties)
        {
            //初始化ILoggingService接口
            FanHai.Gui.Core.Services.ServiceManager.LoggingService = new log4netLoggingService();
            //初始化IMessageService接口
            FanHai.Gui.Core.Services.ServiceManager.MessageService = WinFormsMessageService.Instance;
            //记录字符串信息
            LoggingService.Info("Init FanHai Framework Core...");
            this.callback = callback;

            CoreStartup startup = new CoreStartup(properties.ApplicationName);
            if (properties.UseSolarViewerFrameworkErrorHandler)
            {
                this.useSolarViewerFrameworkErrorHandler = true;
                ExceptionBox.RegisterExceptionBoxForUnhandledExceptions();
            }
            //设定启动路径
             startup.ConfigDirectory = properties.ConfigDirectory;
            //设定数据路径
            startup.DataDirectory = properties.DataDirectory;
            if (properties.PropertiesName != null)
            {
                //把StarupSettings类中PropertiesName属性传递给CoreStartup
                startup.PropertiesName = properties.PropertiesName;
            }
            //ParserService.DomPersistencePath = properties.DomPersistencePath;
            // disable RTL: translations for the RTL languages are inactive
            RightToLeftConverter.RightToLeftLanguages = new string[0];
            //设定App启动路径
            if (properties.ApplicationRootPath != null)
            {
                FileUtility.ApplicationRootPath = properties.ApplicationRootPath;
            }
            startup.StartCoreServices();

            Assembly exe = Assembly.Load(properties.ResourceAssemblyName);
            ResourceService.RegisterNeutralStrings(new ResourceManager("FanHai.Gui.Framework.StartUp.Resources.StringResources", exe));
            ResourceService.RegisterNeutralImages(new ResourceManager("FanHai.Gui.Framework.StartUp.Resources.BitmapResources", exe));

            LoggingService.Info("Looking for AddIns...");
            foreach (string file in properties.addInFiles)
            {
                startup.AddAddInFile(file);
            }
            foreach (string dir in properties.addInDirectories)
            {
                startup.AddAddInsFromDirectory(dir);
            }

            if (properties.AllowAddInConfigurationAndExternalAddIns)
            {
                startup.ConfigureExternalAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddIns.xml"));
            }
            if (properties.AllowUserAddIns)
            {
                startup.ConfigureUserAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddInInstallTemp"),
                                            Path.Combine(PropertyService.ConfigDirectory, "AddIns"));
            }

            LoggingService.Info("Loading AddInTree...");
            startup.RunInitialization();

            LoggingService.Info("Init FanHai Framework Core finished");
        }
        #endregion

        #region Initialize and run Workbench
        public void RunWorkbench(WorkbenchSettings settings)
        {
            if (settings.RunOnNewThread)
            {
                Thread t = new Thread(RunWorkbenchInternal);
                t.SetApartmentState(ApartmentState.STA);
                t.Name = "SMFmain";
                t.Start(settings);
            }
            else
            {
                RunWorkbenchInternal(settings);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void RunWorkbenchInternal(object settings)
        {
            WorkbenchSettings wbSettings = (WorkbenchSettings)settings;

            LoggingService.Info("Initializing workbench...");
            WorkbenchSingleton.InitializeWorkbench();

            LoggingService.Info("Starting workbench...");
            Exception exception = null;
            // finally start the workbench.
            try
            {
                StartWorkbenchCommand wbc = new StartWorkbenchCommand();
                callback.BeforeRunWorkbench();
                if (Debugger.IsAttached)
                {
                    wbc.Run(wbSettings.InitialFileList);
                }
                else
                {
                    try
                    {
                        wbc.Run(wbSettings.InitialFileList);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }
            }
            finally
            {
                LoggingService.Info("Unloading services...");
                try
                {
                    WorkbenchSingleton.OnWorkbenchUnloaded();
                    PropertyService.Save();
                }
                catch (Exception ex)
                {
                    LoggingService.Warn("Exception during unloading", ex);
                    if (exception == null)
                    {
                        exception = ex;
                    }
                }
            }
            LoggingService.Info("Finished running workbench.");
            callback.WorkbenchClosed();
            if (exception != null)
            {
                const string errorText = "Unhandled exception terminated the workbench";
                LoggingService.Fatal(exception);
                if (useSolarViewerFrameworkErrorHandler)
                {
                    System.Windows.Forms.Application.Run(new ExceptionBox(exception, errorText, true));
                }
                else
                {
                    throw new RunWorkbenchException(errorText, exception);
                }
            }
        }
        #endregion

        public bool CloseWorkbench(bool force)
        {
            if (WorkbenchSingleton.InvokeRequired)
            {
                return WorkbenchSingleton.SafeThreadFunction<bool, bool>(CloseWorkbenchInternal, force);
            }
            else
            {
                return CloseWorkbenchInternal(force);
            }
        }
        bool CloseWorkbenchInternal(bool force)
        {
            if (force)
            {
                foreach (IWorkbenchWindow window in WorkbenchSingleton.Workbench.WorkbenchWindowCollection.ToArray())
                {
                    window.CloseWindow(true);
                }
            }
            WorkbenchSingleton.MainForm.Close();
            return WorkbenchSingleton.MainForm.IsDisposed;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void KillWorkbench()
        {
            System.Windows.Forms.Application.Exit();
        }

        public bool WorkbenchVisible
        {
            get
            {
                if (WorkbenchSingleton.InvokeRequired)
                {
                    return WorkbenchSingleton.SafeThreadFunction<bool>(GetWorkbenchVisibleInternal);
                }
                else
                {
                    return GetWorkbenchVisibleInternal();
                }
            }
            set
            {
                if (WorkbenchSingleton.InvokeRequired)
                {
                    WorkbenchSingleton.SafeThreadCall(SetWorkbenchVisibleInternal, value);
                }
                else
                {
                    SetWorkbenchVisibleInternal(value);
                }
            }
        }
        bool GetWorkbenchVisibleInternal()
        {
            return WorkbenchSingleton.MainForm.Visible;
        }
        void SetWorkbenchVisibleInternal(bool value)
        {
            WorkbenchSingleton.MainForm.Visible = value;
        }
    }
}


