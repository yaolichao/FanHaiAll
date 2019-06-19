using FanHai.MES.IVTest.BLL;
using FanHai.MES.IVTest.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace FanHai.MES.IVTest.Service
{
    /// <summary>
    /// 将IV测试数据转移到SQL Server数据库中的服务程序。
    /// </summary>
    public partial class DataTransferService : ServiceBase
    {
        private string ACCESS_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["ACCESS_STRING"].ConnectionString;
        private string SQLSERVER_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["SQLSERVER_STRING"].ConnectionString;
        private int _sleepTime = 100000;
        IVTestConfigurationSection _section = null;
        IList<IVTestDataTransferThreadWrapper> lstWrapper = new List<IVTestDataTransferThreadWrapper>();
        private EventLog _evnetLog = null;
        public DataTransferService()
        {
            InitializeComponent();
            _evnetLog = new System.Diagnostics.EventLog();
            // Turn off autologging
            this.AutoLog = false;
            // create an event source, specifying the name of a log that
            // does not currently exist to create a new, custom log
          
            if (!System.Diagnostics.EventLog.SourceExists("IVTestDataTransfer"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "IVTestDataTransfer", "IVTestDataTransferService");
            }

            // configure the event log instance to use this source name
            _evnetLog.Source = "IVTestDataTransfer";
        }
        /// <summary>
        /// 启动服务。
        /// </summary>
        protected override void OnStart(string[] args)
        {
            _evnetLog.WriteEntry("FanHai.MES.IVTest.Service服务启动");
            //获取配置节信息
            this._section = (IVTestConfigurationSection)ConfigurationManager.GetSection("ivtest");
            string sleepTime = System.Configuration.ConfigurationManager.AppSettings["SLEEP_TIME"];
            if (!string.IsNullOrEmpty(sleepTime))
            {
                _sleepTime = Convert.ToInt32(sleepTime);
            }
            //增加线程个数。
            foreach (DeviceElement element in this._section.Devices)
            {
                ParameterizedThreadStart threadStart = new ParameterizedThreadStart(TransferData);
                IVTestDataTransferThreadWrapper wrapper = new IVTestDataTransferThreadWrapper(element, threadStart);
                lstWrapper.Add(wrapper);
            }
            //启动线程。
            foreach (IVTestDataTransferThreadWrapper wrapper in lstWrapper)
            {
                Thread.Sleep(100);
                wrapper.Start();
            }
        }
        /// <summary>
        /// 数据转置
        /// </summary>
        private void TransferData(object obj)
        {
            IVTestDataTransferThreadWrapper wrapper = obj as IVTestDataTransferThreadWrapper;
            if (wrapper == null)
            {
                return;
            }
            TransferData(wrapper);
        }
        /// <summary>
        /// 数据转置
        /// </summary>
        private void TransferData(IVTestDataTransferThreadWrapper wrapper)
        {
            while (wrapper.Loop)
            {
                try
                {
                    DeviceElement device = wrapper.Device;
                    DateTime dtStartTime = DateTime.Now;
                    string strFileFullName = DataTransfer.GetFullFile(device.Path,device.Format);
                    string msg = string.Empty;
                    if (!string.IsNullOrEmpty(strFileFullName))
                    {
                        string accConString = string.Format(ACCESS_STRING, strFileFullName);
                        string sqlConString = SQLSERVER_STRING;
                        DataTransfer sdgData = new DataTransfer(accConString, sqlConString);
                        sdgData.AccessToSqlServer(device.Name,device.Type,string.Empty);
                        DateTime dtEndTime = DateTime.Now;
                        if (sdgData.TransferCount > 0)
                        {
                            msg = string.Format("开始时间:{0};结束时间:{1};耗用时间:{2}秒;转置数据数量:{3}。{4}",
                                dtStartTime, dtEndTime, (dtEndTime - dtStartTime).TotalSeconds, sdgData.TransferCount, strFileFullName
                                );
                            _evnetLog.WriteEntry(msg);
                        }
                    }
                    else
                    {
                        msg = string.Format("开始时间:{0};获取ACCESS数据库文件失败。", dtStartTime);
                        _evnetLog.WriteEntry(msg);
                    }
                }
                catch (Exception ex)
                {
                    _evnetLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
                if (wrapper.Loop)
                {
                    Thread.Sleep(_sleepTime);
                }
            }
            wrapper.AutoResetEvent.Set();
        }
        /// <summary>
        /// 停止服务。
        /// </summary>
        protected override void OnStop()
        {
            foreach (IVTestDataTransferThreadWrapper wrapper in lstWrapper)
            {
                wrapper.Stop();
                wrapper.Dispose();
            }
            lstWrapper.Clear();
            lstWrapper = null;
            _evnetLog.WriteEntry("FanHai.MES.IVTest.Service服务停止");
        }
    }

    /// <summary>
    /// IV测试数据转置线程封装类。
    /// </summary>
    public class IVTestDataTransferThreadWrapper : IDisposable
    {
        /// <summary>
        /// 获取线程执行的循环标志。
        /// </summary>
        public bool Loop { get; private set; }
        /// <summary>
        /// 获取线程执行异步事件。
        /// </summary>
        public AutoResetEvent AutoResetEvent { get; private set; }
        /// <summary>
        /// 获取线程对象。
        /// </summary>
        public Thread Thread { get; private set; }
        /// <summary>
        /// 获取IV测试设备对象。
        /// </summary>
        public DeviceElement Device { get; private set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public IVTestDataTransferThreadWrapper(DeviceElement device, ParameterizedThreadStart threadStart)
        {
            this.Loop = true;
            this.Device = device;
            this.AutoResetEvent = new AutoResetEvent(false);
            this.Thread = new Thread(threadStart);
        }
        /// <summary>
        /// 启动线程。
        /// </summary>
        public void Start()
        {
            this.Thread.Start(this);
        }
        /// <summary>
        /// 停止线程。
        /// </summary>
        public void Stop()
        {
            this.Loop = false;
            if (!this.AutoResetEvent.WaitOne(10000))
            {
                this.Thread.Abort();
            }
            this.Thread = null;
        }
        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            this.AutoResetEvent.Close();
            this.AutoResetEvent = null;
            this.Thread = null;
        }
    }
}
