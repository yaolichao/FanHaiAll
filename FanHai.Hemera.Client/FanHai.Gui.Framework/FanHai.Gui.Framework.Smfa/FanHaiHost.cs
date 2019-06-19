using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace FanHai.Gui.Framework.Smfa
{
    /// <summary>
    /// This class can host an instance of FanHai Framework inside another
    /// AppDomain.
    /// </summary>
    public sealed class SolarViewerHost
    {
        #region CreateDomain
        /// <summary>
        /// Create an AppDomain capable of hosting FanHai Framework.
        /// </summary>
        public static AppDomain CreateDomain()
        {
            return AppDomain.CreateDomain("FanHai.Gui.Framework.Smfa", null, CreateDomainSetup());
        }

        /// <summary>
        /// Creates an AppDomainSetup specifying properties for an AppDomain capable of
        /// hosting FanHai Framework.
        /// </summary>
        public static AppDomainSetup CreateDomainSetup()
        {
            AppDomainSetup s = new AppDomainSetup();
            s.ApplicationBase = Path.GetDirectoryName(SmfaAssembly.Location);
            s.ConfigurationFile = SmfaAssembly.Location + ".config";
            s.ApplicationName = "FanHai.Gui.Framework.Smfa";
            return s;
        }
        #endregion

        #region Static helpers
        //获得SolarViewerHost应用程序集
        internal static Assembly SmfaAssembly
        {
            get
            {
                return typeof(SolarViewerHost).Assembly;
            }
        }
        #endregion

        #region SMFInitStatus enum
        enum SMFInitStatus
        {
            None,
            CoreInitialized,
            WorkbenchInitialized,
            Busy,
            AppDomainUnloaded
        }
        #endregion

        AppDomain appDomain;
        CallHelper helper;
        SMFInitStatus initStatus;

        #region Constructors
        /// <summary>
        /// Create a new AppDomain to host FanHai GUI Framework.
        /// </summary>
        public SolarViewerHost(StartupSettings startup)
        {
            if (startup == null)
            {
                throw new ArgumentNullException("startup");
            }
            this.appDomain = CreateDomain();
            helper = (CallHelper)appDomain.CreateInstanceAndUnwrap(SmfaAssembly.FullName, typeof(CallHelper).FullName);
            helper.InitMESCore(new CallbackHelper(this), startup);
            initStatus = SMFInitStatus.CoreInitialized;
        }

        /// <summary>
        /// 把设定的startup对象参数加载到应用程序域中
        /// </summary>
        public SolarViewerHost(AppDomain appDomain, StartupSettings startup)
        {
            if (appDomain == null)
            {
                throw new ArgumentNullException("appDomain");
            }
            if (startup == null)
            {
                throw new ArgumentNullException("startup");
            }
            this.appDomain = appDomain;
            //创建CallHelper实例
            helper = (CallHelper)appDomain.CreateInstanceAndUnwrap(SmfaAssembly.FullName, typeof(CallHelper).FullName);
            helper.InitMESCore(new CallbackHelper(this), startup);
            initStatus = SMFInitStatus.CoreInitialized;
        }
        #endregion

        #region Workbench Initialization and startup
        /// <summary>
        /// Initializes the workbench (create the MainForm instance, construct menu from AddInTree etc.)
        /// and runs it using the supplied settings.
        /// This starts a new message loop for the workbench. By default the message loop
        /// is created on a new thread, but you can change the settings so that
        /// it is created on the thread calling RunWorkbench.
        /// In that case, RunWorkbench will block until FanHai GUI Framework is shut down!
        /// </summary>
        public void RunWorkbench(WorkbenchSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (initStatus == SMFInitStatus.CoreInitialized)
            {
                initStatus = SMFInitStatus.Busy;
                helper.RunWorkbench(settings);
                if (settings.RunOnNewThread)
                {
                    initStatus = SMFInitStatus.WorkbenchInitialized;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        #endregion

        #region Application control
       

        /// <summary>
        /// Gets/Sets whether the workbench is visible.
        /// Requires that the workbench is running.
        /// </summary>
        public bool WorkbenchVisible
        {
            get
            {
                if (initStatus != SMFInitStatus.WorkbenchInitialized)
                {
                    return false;
                }
                else
                {
                    return helper.WorkbenchVisible;
                }
            }
            set
            {
                if (initStatus != SMFInitStatus.WorkbenchInitialized)
                {
                    throw new InvalidOperationException();
                }
                helper.WorkbenchVisible = value;
            }
        }

        /// <summary>
        /// Closes and unloads the workbench. The user is asked to save his work
        /// and can abort closing.
        /// Requires that the workbench is running.
        /// </summary>
        /// <param name="force">When force is used (=true), unsaved changes to documents
        /// are lost, but FanHai GUI Framework still terminates correctly and saves changed
        /// settings.</param>
        /// <returns>True when the workbench was closed.</returns>
        public bool CloseWorkbench(bool force)
        {
            if (initStatus == SMFInitStatus.CoreInitialized)
            {
                // Workbench not loaded/already closed: do nothing
                return true;
            }
            if (initStatus != SMFInitStatus.WorkbenchInitialized)
            {
                throw new InvalidOperationException();
            }
            return helper.CloseWorkbench(force);
        }

        /// <summary>
        /// Unload the FanHai GUI Framework AppDomain. This will force FanHai GUI Framework
        /// to close without saving changed settings.
        /// Call CloseWorkbench before UnloadDomain to prompt the user to save documents and settings.
        /// </summary>
        public void UnloadDomain()
        {
            if (initStatus != SMFInitStatus.AppDomainUnloaded)
            {
                if (initStatus == SMFInitStatus.WorkbenchInitialized)
                {
                    helper.KillWorkbench();
                }
                AppDomain.Unload(appDomain);
                initStatus = SMFInitStatus.AppDomainUnloaded;
            }
        }
        #endregion

        #region CreateInstanceInTargetDomain
        /// <summary>
        /// Gets the AppDomain used to host FanHai GUI Framework.
        /// </summary>
        public AppDomain AppDomain
        {
            get
            {
                return appDomain;
            }
        }

        /// <summary>
        /// Creates an instance of the specified type argument in the target AppDomain.
        /// </summary>
        /// <param name="arguments">Arguments to pass to the constructor of <paramref name="type"/>.</param>
        /// <returns>The constructed object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T CreateInstanceInTargetDomain<T>(params object[] arguments) where T : MarshalByRefObject
        {
            Type type = typeof(T);
            return (T)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, arguments);
        }

        /// <summary>
        /// Creates an instance of the specified type in the target AppDomain.
        /// </summary>
        /// <param name="type">Type to create an instance of.</param>
        /// <param name="arguments">Arguments to pass to the constructor of <paramref name="type"/>.</param>
        /// <returns>The constructed object.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public object CreateInstanceInTargetDomain(Type type, params object[] arguments)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (typeof(MarshalByRefObject).IsAssignableFrom(type) == false)
                throw new ArgumentException("type does not inherit from MarshalByRefObject", "type");
            return appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, arguments);
        }
        #endregion

        #region Callback Events
        System.ComponentModel.ISynchronizeInvoke invokeTarget;
        /// <summary>
        /// Gets/Sets an object to use to synchronize all events with a thread.
        /// Use null (default) to handle all events on the thread they were
        /// raised on.
        /// </summary>
        public System.ComponentModel.ISynchronizeInvoke InvokeTarget
        {
            get
            {
                return invokeTarget;
            }
            set
            {
                invokeTarget = value;
            }
        }
        /// <summary>
        /// Event before the workbench starts running.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix")]
        public event EventHandler BeforeRunWorkbench;
        /// <summary>
        /// Event after the workbench has been unloaded.
        /// </summary>
        public event EventHandler WorkbenchClosed;

        internal sealed class CallbackHelper : MarshalByRefObject
        {
            private static readonly object[] emptyObjectArray = new object[0];

            readonly SolarViewerHost host;

            public CallbackHelper(SolarViewerHost host)
            {
                this.host = host;
            }

            public override object InitializeLifetimeService()
            {
                return null;
            }

            private bool InvokeRequired
            {
                get
                {
                    return host.invokeTarget != null && host.invokeTarget.InvokeRequired;
                }
            }

            private void Invoke(System.Windows.Forms.MethodInvoker method)
            {
                host.invokeTarget.BeginInvoke(method, emptyObjectArray);
            }

            private void Invoke(Action<string> method, string argument)
            {
                host.invokeTarget.BeginInvoke(method, new object[] { argument });
            }

            internal void BeforeRunWorkbench()
            {
                if (InvokeRequired) { Invoke(BeforeRunWorkbench); return; }
                host.initStatus = SMFInitStatus.WorkbenchInitialized;
                if (host.BeforeRunWorkbench != null) host.BeforeRunWorkbench(host, EventArgs.Empty);
            }

            internal void WorkbenchClosed()
            {
                if (InvokeRequired) { Invoke(WorkbenchClosed); return; }
                host.initStatus = SMFInitStatus.CoreInitialized;
                if (host.WorkbenchClosed != null) host.WorkbenchClosed(host, EventArgs.Empty);
            }
        }
        #endregion
    }
}
