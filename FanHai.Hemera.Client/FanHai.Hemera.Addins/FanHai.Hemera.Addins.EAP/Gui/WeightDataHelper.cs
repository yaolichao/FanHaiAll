using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using System.Data;
using System.IO.Ports;

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 称重数据读取的帮助类。
    /// </summary>
    public class WeightDataReadHelper
    {
        private static SerialPortDataReader _reader=null;
        private static object lockobj = new object();
        /// <summary>
        /// 读取天平秤数据的对象。
        /// </summary>
        public static SerialPortDataReader BalanceDataReader
        {
            get
            {
                if (_reader == null)
                {
                    lock (lockobj)
                    {
                        if (_reader == null)
                        {
                            SerialPort serialPort = GetSerialPort();
                            _reader = new SerialPortDataReader(serialPort,new BalanceDataParser());
                        }
                    }
                }
                return _reader;
            }
        }

        /// <summary>
        /// 获取与当前机器连接的天平秤的串口对象。
        /// </summary>
        /// <returns>串口对象</returns>
        private static SerialPort GetSerialPort()
        {
            //W_PORT_NAME  天平秤端口号
            //W_BAUD_RATE  天平秤串行波特率
            SerialPort obj=new SerialPort();
            ComputerEntity entity = new ComputerEntity();
            DataTable dtComputerUDA = entity.GetComputerInfo();
            string portName = entity.GetComputerUDAAttributeValue(dtComputerUDA, "W_PORT_NAME");
            string baudRate = entity.GetComputerUDAAttributeValue(dtComputerUDA, "W_BAUD_RATE");
            int nBaudRate;
            obj.BaudRate = int.TryParse(baudRate, out nBaudRate) ? nBaudRate : 1200;
            obj.PortName = string.IsNullOrEmpty(portName) ? "COM1" : portName;
            obj.StopBits = System.IO.Ports.StopBits.One;
            obj.Parity = System.IO.Ports.Parity.None;
            obj.DataBits = 8;
            return obj;
        }
    }
    /// <summary>
    /// 天平秤串口数据解析类。
    /// </summary>
    public class BalanceDataParser : ISerialPortDataParser
    {
        private object lockobj = new object();
        /// <summary>
        /// 解析串行端口数据。
        /// </summary>
        /// <param name="orginalData">原始数据。</param>
        /// <param name="data">解析后的数据。</param>
        /// <returns>解析成功返回true，解析失败返回false。</returns>
        public bool Parser(string orginalData, out string data)
        {
            lock (lockobj)
            {
                data = orginalData;
                orginalData = System.Text.RegularExpressions.Regex.Replace(orginalData, " +", " ");
                string[] datas = orginalData.Split(' ');
                if (datas.Length < 3) return false;
                //JA/FA天平秤
                if (datas[0] == "L"){
                    //JA/FA天平秤 第二位是 +开头的数字，取值+后面的数字。
                    if (datas[1].StartsWith("+") && datas[1].Length > 1)
                    {
                        data = datas[1].Substring(1);
                    }
                    else
                    {
                        //JA/FA天平秤 第二位是 + 取第三位数字。
                        if (datas[1] != "+") return false;
                        data = datas[2];
                    }
                    return true;
                }
                //梅特勒天平秤 S表示正常数据，SD表示数据未稳定。
                //建议MES程序不对S与SD作区分，都可抓取，由作业人员判断抓取时机。
                else if (datas[0] == "S" || datas[0]=="SD")
                {
                    data = datas[1];
                    return true;
                }
                return false;
            }
        }
    }
}
