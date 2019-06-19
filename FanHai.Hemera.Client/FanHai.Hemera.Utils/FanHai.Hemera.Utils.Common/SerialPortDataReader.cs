//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-04-13            新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace FanHai.Hemera.Utils.Common
{
    /// <summary>
    /// 解析串行端口数据的接口。
    /// </summary>
    /// <remarks>
    /// public class ConcreteDataParser:ISerialPortDataParser
    /// {
    ///     public bool Parser(string orginalData, out string data)
    ///     {
    ///         if (orginalData.Length != 3) return false;
    ///         data = orginalData;
    ///         return true;
    ///     }
    /// }
    /// </remarks>
    public interface ISerialPortDataParser
    {
        /// <summary>
        /// 解析串行端口数据。
        /// </summary>
        /// <param name="orginalData">原始数据。</param>
        /// <param name="data">解析后的数据。</param>
        /// <returns>解析成功返回true，解析失败返回false。</returns>
        bool Parser(string orginalData, out string data);
    }
    /// <summary>
    /// 串行端口数据读取类。
    /// </summary>
    /// <remarks>
    /// SerialPortDataReader reader = new SerialPortDataReader();
    /// reader.SerialPort.BaudRate = 1200;
    /// reader.SerialPort.PortName = "COM1";
    /// reader.SerialPort.StopBits = System.IO.Ports.StopBits.One;
    /// reader.SerialPort.Parity = System.IO.Ports.Parity.None;
    /// reader.SerialPort.DataBits = 8;
    /// string data=reader.StartRead();
    /// if(reader.IsReadDataSuccess){
    ///     string errmsg=reader.ErrorMessage;
    /// }
    /// string nextData1=reader.DataValue;
    /// string nextData2=reader.DataValue;
    /// </remarks>
    public class SerialPortDataReader : IDisposable
    {
        private int DEFAULT_TIME_OUT = 1000;
        private SerialPort _serialPort;
        private AutoResetEvent _syncEvent = null;
        private string _dataValue = string.Empty;
        private ISerialPortDataParser _parser = null;
        /// <summary>
        /// 超时时间(毫秒)，默认为：1000毫秒。
        /// </summary>
        public int TimeOut
        {
            get;
            set;
        }
        /// <summary>
        /// 读取的数据值。
        /// </summary>
        public string DataValue
        {
            get
            {
                if (!this._serialPort.IsOpen)
                {
                    return string.Empty;
                }
                else
                {
                    this._syncEvent.WaitOne(this.TimeOut);
                    return this._dataValue;
                }
            }
        }
        /// <summary>
        /// 读取数据是否成功。
        /// </summary>
        public bool IsReadDataSuccess
        {
            get;
            private set;
        }
        /// <summary>
        /// 最后一次读取时发生的错误消息。
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// 获取或设置串行端口资源。
        /// </summary>
        public SerialPort SerialPort
        {
            get
            {
                return _serialPort;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SerialPortDataReader() : this(new SerialPort(), null) { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SerialPortDataReader(SerialPort serialport) : this(serialport, null) { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SerialPortDataReader(SerialPort serialport, ISerialPortDataParser parser)
        {
            this._serialPort = serialport;
            this._serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            this._serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
            this._syncEvent = new AutoResetEvent(false);
            this.TimeOut = DEFAULT_TIME_OUT;
            this._parser = parser;
        }
        /// <summary>
        /// 处理SerialPort对象错误事件的方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            this.ErrorMessage = string.Format("接收数据时发生错误：{0}", e.EventType.ToString());
            this.IsReadDataSuccess = false;
            this._syncEvent.Set();
        }
        /// <summary>
        /// 处理SerialPort对象数据接收事件的方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                string orginalData = this._serialPort.ReadLine();
                if (this._parser != null)
                {
                    string parsedData = string.Empty;
                    if (!this._parser.Parser(orginalData, out parsedData))
                    {
                        this.ErrorMessage = string.Format("对接收数据解析时发生错误：{0}", orginalData);
                        this.IsReadDataSuccess = false;
                        this._dataValue = orginalData;
                    }
                    else
                    {
                        this._dataValue = parsedData;
                        this.ErrorMessage = string.Empty; ;
                        this.IsReadDataSuccess = true;
                    }
                }
                else
                {
                    this._dataValue = orginalData;
                    this.ErrorMessage = string.Empty; ;
                    this.IsReadDataSuccess = true;
                }
            }
            this._syncEvent.Set();
        }
        /// <summary>
        /// 打开串口，开始读取。
        /// </summary>
        public string StartRead()
        {
            //this.IsReadDataSuccess = true;
            //string parsedData = string.Empty;
            //this._parser.Parser("L  +8.179 g", out parsedData);
            //return parsedData;
            //try
            //{
                if (!this._serialPort.IsOpen)
                {
                    this._serialPort.Open();
                    this._serialPort.DiscardInBuffer();
                }
                return this.DataValue;
            //}
            //finally
            //{
            //    this.Close();
            //}
        }
        /// <summary>
        /// 关闭串口。
        /// </summary>
        public void Close()
        {
            if (this._serialPort.IsOpen)
            {
                this._serialPort.Close();
            }
        }
        /// <summary>
        /// 释放占用的资源。
        /// </summary>
        public void Dispose()
        {
            this.Close();
            if (this._serialPort != null)
            {
                this._serialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                this._serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
                this._serialPort.Dispose();
                this._serialPort = null;
            }
            if (this._syncEvent != null)
            {
                this._syncEvent.Close();
                this._syncEvent = null;
            }
        }
    }
}
