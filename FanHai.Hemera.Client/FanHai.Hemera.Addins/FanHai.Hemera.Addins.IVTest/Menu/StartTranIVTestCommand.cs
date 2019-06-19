using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using System.Threading;
using FanHai.Hemera.Share.Constants;
using System.IO;
using System.Data.OleDb;
using System.Data;
using FanHai.Hemera.Utils.Entities;
using System.ComponentModel;
using Npgsql;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// 启动IV测试数据转置线程。
    /// </summary>
    public class StartTranIVTestCommand : AbstractMenuCommand
    {
        private static readonly object objLock = new object();
        private static IVTestDataTransferThreadWrapper _wrapper = null;
        /// <summary>
        /// 获取IV测试数据转置的线程对象。
        /// </summary>
        private static IVTestDataTransferThreadWrapper ThreadWrapper
        {
            get
            {
                if (_wrapper == null)
                {
                    lock (objLock)
                    {
                        if (_wrapper == null)
                        {
                            //主窗体关闭时清理IV测试线程资源。
                            WorkbenchSingleton.Workbench.MainForm.Disposed += new EventHandler((sender, args) =>
                                                                                {
                                                                                    if (_wrapper != null)
                                                                                    {
                                                                                        _wrapper.Stop();
                                                                                        _wrapper.Dispose();
                                                                                        _wrapper = null;
                                                                                    }
                                                                                });
                            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(TransferData);
                            _wrapper = new IVTestDataTransferThreadWrapper(threadStart);
                        }
                    }
                }
                return _wrapper;
            }
        }
        /// <summary>
        /// 数据转置
        /// </summary>
        private static void TransferData(object obj)
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
        private static void TransferData(IVTestDataTransferThreadWrapper wrapper)
        {
            while (wrapper.Loop)
            {
                IVTestConfigElement cfg = wrapper.IVTestConfig;
                IVTestDeviceType tpye = cfg.Type;
                DateTime dtStartTime = DateTime.Now;
                string accConString = string.Empty;
                string msg = string.Empty;

                try
                {
                    //通过获取的设备类型进行上传方式的
                    switch (tpye)
                    {
                        case IVTestDeviceType.Pasan:
                        case IVTestDeviceType.Results:
                        case IVTestDeviceType.SunData:
                            if (!string.IsNullOrEmpty(cfg.Path))
                            {
                                accConString = string.Format("Provider={0};Data Source={1}", cfg.AccessProvider, cfg.Path);
                                int count = ExecuteDataTransfer(accConString, cfg);
                                DateTime dtEndTime = DateTime.Now;
                                if (count > 0)
                                {
                                    msg = string.Format("IVTestDataTransfer--开始时间:{0};结束时间:{1};耗用时间:{2}秒;转置数据数量:{3}。{4}",
                                                         dtStartTime, dtEndTime, (dtEndTime - dtStartTime).TotalSeconds, count, cfg.Path);
                                    LoggingService.Info(msg);
                                }
                            }
                            else
                            {
                                msg = string.Format("IVTestDataTransfer--开始时间:{0};获取ACCESS数据库文件失败。", dtStartTime);
                                LoggingService.Info(msg);
                            }
                            break;
                        case IVTestDeviceType.PasanPostGre:
                            if (!string.IsNullOrEmpty(cfg.DatabaseAddress) &&
                                !string.IsNullOrEmpty(cfg.DatabaseName) &&
                                !string.IsNullOrEmpty(cfg.DatabasePort) &&
                                !string.IsNullOrEmpty(cfg.DatabaseLoginName) &&
                                !string.IsNullOrEmpty(cfg.DatabaseLoginPassword))
                            {
                                accConString = string.Format("Server={0};Port={1};UserId={2};Password={3};Database={4};",
                                                              cfg.DatabaseAddress,
                                                              cfg.DatabasePort,
                                                              cfg.DatabaseLoginName,
                                                              cfg.DatabaseLoginPassword,
                                                              cfg.DatabaseName);

                                int count = ExecuteDataTransfer(accConString, cfg);
                                DateTime dtEndTime = DateTime.Now;
                                if (count > 0)
                                {
                                    msg = string.Format("IVTestDataTransfer--开始时间:{0};结束时间:{1};耗用时间:{2}秒;转置数据数量:{3}。{4}",
                                                         dtStartTime, dtEndTime, (dtEndTime - dtStartTime).TotalSeconds, count, cfg.Path);
                                    LoggingService.Info(msg);
                                }
                            }
                            else
                            {
                                msg = string.Format("IVTestDataTransfer--开始时间:{0};连接数据库配置信息异常。", dtStartTime);
                                LoggingService.Info(msg);
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    LoggingService.Error(string.Format("IVTestDataTransfer--Error:{0}", ex.Message));
                }
                if (wrapper.Loop)
                {
                    Thread.Sleep(cfg.Millisecond);
                }
            }
            wrapper.AutoResetEvent.Set();
        }
        /// <summary>
        /// 执行IV测试数据转置。
        /// </summary>
        /// <returns>新增IV测试数据的个数。</returns>
        private static int ExecuteDataTransfer(string accessConnectionString, IVTestConfigElement cfg)
        {
            int count = 0;
            IVTestDataTransferEntity entity = new IVTestDataTransferEntity();
            //根据设备代码获取SQL Server数据库中最大的测试时间值。
            DateTime dtMaxTestTime = entity.GetMaxIVTestTime(cfg.DeviceNo);
            IVTestDeviceType type = cfg.Type;
            DataSet dsIVTestData = null;

            //根据设备类型进行数据库连接的区分
            switch (type)
            {
                case IVTestDeviceType.Pasan:
                case IVTestDeviceType.Results:
                case IVTestDeviceType.SunData:
                    //创建 Access的连接对象。
                    using (OleDbConnection oleCon = new OleDbConnection(accessConnectionString))
                    {
                        oleCon.Open();
                        using (OleDbCommand oleCmd = oleCon.CreateCommand())
                        {
                            //组织查询IV测试数据的SQL语句
                            StringBuilder sbSql = new StringBuilder();
                            string sql = GetQueryTestDataSql(cfg.Type);
                            sbSql.AppendFormat("SELECT TOP 100 '{2}' AS DeviceNo,a.* FROM ({0}) a WHERE a.TTIME>'{1}' ORDER BY a.TTIME ASC",
                                                sql,
                                                dtMaxTestTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                cfg.DeviceNo);
                            //从Access数据库获取>开始日期和开始时间的数据。
                            oleCmd.CommandType = CommandType.Text;
                            oleCmd.CommandText = sbSql.ToString();
                            OleDbDataAdapter oleAdapter = new OleDbDataAdapter(oleCmd);
                            dsIVTestData = new DataSet();
                            oleAdapter.Fill(dsIVTestData);
                        }
                        oleCon.Close();
                    }
                    break;
                case IVTestDeviceType.PasanPostGre:
                    //创建 PostGre 的连接对象。
                    using (NpgsqlConnection npgCon = new NpgsqlConnection(accessConnectionString))
                    {
                        npgCon.Open();
                        using (NpgsqlCommand npgCmd = npgCon.CreateCommand())
                        {
                            //组织查询IV测试数据的SQL语句
                            StringBuilder sbSql = new StringBuilder();
                            string sql = GetQueryTestDataSql(cfg.Type);
                            sbSql.AppendFormat("SELECT  '{2}' AS DeviceNo,a.* FROM ({0}) a WHERE a.TTIME>'{1}' ORDER BY a.TTIME ASC limit 100",
                                                sql,
                                                dtMaxTestTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                cfg.DeviceNo);
                            //从Access数据库获取>开始日期和开始时间的数据。
                            npgCmd.CommandType = CommandType.Text;
                            npgCmd.CommandText = sbSql.ToString();
                            NpgsqlDataAdapter npgAdapter = new NpgsqlDataAdapter(npgCmd);
                            dsIVTestData = new DataSet();
                            npgAdapter.Fill(dsIVTestData);
                        }
                        npgCon.Close();
                    }
                    break;
                default:
                    break;
            }


            //新增IV测试数据
            if (dsIVTestData != null && dsIVTestData.Tables.Count > 0 && dsIVTestData.Tables[0].Rows.Count > 0)
            {
                string operationName = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_OPERATION_NAME).Trim();
                string userId = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dsIVTestData.ExtendedProperties.Add(PROPERTY_FIELDS.IVTEST_DATA_OPERATION_NAME, operationName);
                dsIVTestData.ExtendedProperties.Add(PROPERTY_FIELDS.USER_NAME, userId);
                count = entity.InsertIVTestData(dsIVTestData);
            }

            return count;
        }
        /// <summary>
        /// 获取查询测试数据的SQL字符串。
        /// </summary>
        /// <param name="type">设备类型。</param>
        /// <returns>查询测试数据的SQL字符串。</returns>
        private static string GetQueryTestDataSql(IVTestDeviceType type)
        {
            //--TTIME 测试时间
            //--LOT_NUM 批次号
            //--FF 填充因子
            //--ISC 测试短路电流
            //--EFF 组件转换效率
            //--RSH 串联电阻
            //--RS 并联电阻
            //--VOC 测试开路电压
            //--IPM 测试最大电流
            //--PM 测试最大功率
            //--AMBIENTTEMP 测度温度
            //--SENSORTEMP 环境温度
            //--INTENSITY 光强

            string sql = string.Empty;
            switch (type)
            {
                case IVTestDeviceType.Pasan:
                    sql = @"SELECT  Format(Mes.DtCr,'yyyy-MM-dd HH:mm:ss') AS TTIME,
	                        Mes.SerN AS LOT_NUM,
	                        MesRes.RsEff AS FF,
	                        MesRes.RsIsc AS ISC,
	                        MesRes.RsMme AS EFF,
	                        MesRes.RsRsh AS RSH,
	                        MesRes.RsRsr AS RS,
	                        MesRes.RsVoc AS VOC,
	                        MesRes.RsIpm AS IPM,
	                        MesRes.RsVpm AS VPM,
	                        MesRes.RsMxp AS PM,
	                        MesT.T4 AS AMBIENTTEMP,  
	                        MesT.T1 AS SENSORTEMP,
	                        MesRes.Irm AS INTENSITY
                    FROM Mes, MesRes, MesT
                    WHERE Mes.Id = MesT.MesId
                    AND Mes.ResId = MesRes.Id";
                    break;
                case IVTestDeviceType.PasanPostGre:
                    StringBuilder strbsql = new StringBuilder();

                    strbsql.Append("SELECT to_char(A.\"@creationtimestamp\",'yyyy-MM-dd HH24:mi:ss') AS TTIME,");
                    strbsql.Append("       B.\"dutId\" AS LOT_NUM,");
                    strbsql.Append("       COALESCE(A.\"keyData.FF\",0) AS FF,");
                    strbsql.Append("       COALESCE(A.\"keyData.Isc\",0) AS ISC,");
                    strbsql.Append("       COALESCE(A.\"keyData.Eff\",0) AS EFF,");
                    strbsql.Append("       COALESCE(A.\"keyData.Rsh\",0) AS RSH,");
                    strbsql.Append("       COALESCE(A.\"keyData.Rs\",0)  AS RS,");
                    strbsql.Append("       COALESCE(A.\"keyData.Voc\",0) AS VOC,");
                    strbsql.Append("       COALESCE(A.\"keyData.Ipmax\",0) AS IPM,");
                    strbsql.Append("       COALESCE(A.\"keyData.Vpmax\",0) AS VPM,");
                    strbsql.Append("       COALESCE(A.\"keyData.Pmax\",0) AS PM,");
                    strbsql.Append("       COALESCE(A.\"keyData.TempDUT\",0) AS AMBIENTTEMP,");
                    strbsql.Append("       COALESCE(A.\"keyData.TempMC\",0) AS SENSORTEMP,");
                    strbsql.Append("       COALESCE(A.\"keyData.Gavg\",0) AS INTENSITY ");
                    strbsql.Append("FROM \"DataServer\".\"ComputationDone\" A ");
                    strbsql.Append("INNER JOIN \"DataServer\".\"MeasurementSession\" B ON B.\"sessionId\" = A.\"sessionId\"");

                    sql = strbsql.ToString();
                    break;
                case IVTestDeviceType.Results:
                    sql = @"SELECT  Format(Results.Test_Date,'yyyy-MM-dd ')+Format(Results.Test_Time,'HH:mm:ss') AS TTIME,
	                        Results.ID AS LOT_NUM,
	                        Results.FF AS FF,
	                        Results.Isc AS ISC,
	                        Results.ModEff AS EFF,
	                        Results.Rsh AS RSH,
	                        Results.Rs AS RS,
	                        Results.Voc AS VOC,
	                        Results.Ipm AS IPM,
	                        Results.Vpm AS VPM,
	                        Results.Pmax AS PM,
	                        Results.TMod AS AMBIENTTEMP,
	                        Results.TRef1 AS SENSORTEMP,
	                        Results.CIrr AS INTENSITY
                        FROM Results";
                    break;
                case IVTestDeviceType.SunData:
                    sql = @"SELECT Format(SunData.[DateTime],'yyyy-MM-dd HH:mm:ss') AS TTIME,
	                        SunData.[Serial] AS LOT_NUM,
	                        SunData.[FF] AS FF,
	                        SunData.[Isc] AS ISC,
	                        SunData.[Eff] AS EFF,
	                        SunData.[Rsh] AS RSH,
	                        SunData.[Rs] AS RS,
	                        SunData.[Voc] AS VOC,
	                        SunData.[Imax] AS IPM,
	                        SunData.[Vmax] AS VPM,
	                        SunData.[Pmax] AS PM,
	                        SunData.[SurfTemp] AS AMBIENTTEMP,
	                        SunData.[EnvTemp] AS SENSORTEMP, 
	                        SunData.[Sun] AS INTENSITY
                        FROM SunData";
                    break;
            }
            return sql;
        }
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            IVTestConfigElement cfg = null;

            string type = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_TYPE).Trim();
            string device = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DEVICE).Trim();

            if (!string.IsNullOrEmpty(type) & !string.IsNullOrEmpty(device))
            {
                IVTestDeviceType dvType = (IVTestDeviceType)Enum.Parse(typeof(IVTestDeviceType), type, true);

                switch (type)
                {
                    case "1":
                    case "2":
                    case "3":
                        string path = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_PATH).Trim();

                        if (!string.IsNullOrEmpty(path))
                        {
                            cfg = new IVTestConfigElement(device, path, dvType);
                            //重新启动IV测试数据转置线程。
                            if (ThreadWrapper.Loop)
                            {
                                ThreadWrapper.Stop();
                            }
                            ThreadWrapper.Start(cfg);
                        }
                        break;
                    case "4":
                        string databaseAddress = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_ADDRESS).Trim();
                        string databaseName = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_NAME).Trim();
                        string databasePort = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_PORT).Trim();
                        string databaseLoginName = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_LGNAME).Trim();
                        string databaseLoginPassword = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_LGPW).Trim();

                        if (!string.IsNullOrEmpty(databaseAddress) &&
                            !string.IsNullOrEmpty(databaseName) &&
                            !string.IsNullOrEmpty(databasePort) &&
                            !string.IsNullOrEmpty(databaseLoginName) &&
                            !string.IsNullOrEmpty(databaseLoginPassword)
                            )
                        {
                            cfg = new IVTestConfigElement(device,
                                                          databaseAddress,
                                                          databaseName,
                                                          databasePort,
                                                          databaseLoginName,
                                                          databaseLoginPassword,
                                                          dvType);
                            //重新启动IV测试数据转置线程。
                            if (ThreadWrapper.Loop)
                            {
                                ThreadWrapper.Stop();
                            }
                            ThreadWrapper.Start(cfg);
                        }

                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (_wrapper != null)
                {
                    _wrapper.Stop();
                    _wrapper.Dispose();
                    _wrapper = null;
                }
            }
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
        public IVTestConfigElement IVTestConfig { get; set; }

        private ParameterizedThreadStart _threadStart = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public IVTestDataTransferThreadWrapper(ParameterizedThreadStart threadStart)
        {
            this.Loop = false;
            this.AutoResetEvent = new AutoResetEvent(false);
            this._threadStart = threadStart;
        }
        /// <summary>
        /// 启动线程。
        /// </summary>
        public void Start(IVTestConfigElement cfg)
        {
            LoggingService.Info(string.Format("IVTestDataTransfer--Start {0}", DateTime.Now));
            this.Loop = true;
            this.IVTestConfig = cfg;
            this.Thread = new Thread(this._threadStart);
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
            LoggingService.Info(string.Format("IVTestDataTransfer--Stop {0}", DateTime.Now));
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
    /// <summary>
    /// IV测试数据配置。
    /// </summary>
    public class IVTestConfigElement
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="deviceNo">设备代码名称。</param>
        /// <param name="path">设备数据所在文件夹路径。</param>
        /// <param name="type">设备类型。</param>
        public IVTestConfigElement(string deviceNo, string path, IVTestDeviceType type)
        {
            this.DeviceNo = deviceNo;
            this.Path = path;
            this.Type = type;
            this.AccessProvider = "Microsoft.Jet.OleDb.4.0";
            this.Millisecond = 500;

        }

        public IVTestConfigElement(string deviceNo,
                                   string databaseAddress,
                                   string databaseName,
                                   string databasePort,
                                   string databaseLoginName,
                                   string databaseLoginPassword,
                                   IVTestDeviceType type)
        {
            this.DeviceNo = deviceNo;
            this.Type = type;
            this.Millisecond = 500;
            //设定数据库对应访问变量
            this.DatabaseAddress = databaseAddress;
            this.DatabaseName = databaseName;
            this.DatabasePort = databasePort;
            this.DatabaseLoginName = databaseLoginName;
            this.DatabaseLoginPassword = databaseLoginPassword;
        }
        /// <summary>
        /// 设备代码。唯一标识设备的属性。
        /// </summary>
        public string DeviceNo
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据文件路径。
        /// </summary>
        public string Path
        {
            get;
            set;
        }
        /// <summary>
        /// 设备类型。
        /// </summary>
        public IVTestDeviceType Type
        {
            get;
            set;
        }
        /// <summary>
        /// 间隔的毫秒数。
        /// </summary>
        public int Millisecond
        {
            get;
            set;
        }
        /// <summary>
        /// Access数据库访问程序。
        /// </summary>
        /// ACCESS数据库连接字符串
        ///Microsoft.Jet.OleDb.4.0
        ///Microsoft.ACE.OLEDB.12.0
        public string AccessProvider
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据所在文件的格式化字符串。
        /// </summary>
        public string Format
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据库地址。
        /// </summary>
        public string DatabaseAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据库名称。
        /// </summary>
        public string DatabaseName
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据库端口。
        /// </summary>
        public string DatabasePort
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据库登录名。
        /// </summary>
        public string DatabaseLoginName
        {
            get;
            set;
        }
        /// <summary>
        /// 设备数据库登录密码。
        /// </summary>
        public string DatabaseLoginPassword
        {
            get;
            set;
        }

    }
    /// <summary>
    /// IV测试设备类型。
    /// </summary>
    public enum IVTestDeviceType
    {
        /// <summary>
        /// GSolar设备 
        /// </summary>
        [DescriptionAttribute("GSolar设备")]
        SunData = 1,
        /// <summary>
        /// Spire设备
        /// </summary>
        [DescriptionAttribute("Spire设备")]
        Results = 2,
        /// <summary>
        /// Pasan设备
        /// </summary>
        [DescriptionAttribute("Pasan设备")]
        Pasan = 3,
        /// <summary>
        /// Pasan设备(PostGre)
        /// </summary>
        [DescriptionAttribute("Pasan设备(PostGre)")]
        PasanPostGre = 4
    }
}
