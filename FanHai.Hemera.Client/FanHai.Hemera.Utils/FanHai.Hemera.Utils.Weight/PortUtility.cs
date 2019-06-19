using System;
using System.Text;
using System.IO.Ports;
using System.Collections.Generic;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Hemera.Utils.Entities.Exceptions;
using System.Threading;


namespace SolarViewer.Hemera.Utils.Weight
{
    public class PortUtility
    {
         ComPort _comPort;
         SerialPort _serialPort;
         Thread _receivedThread;
         bool  _blComOpen = false;

         public PortUtility()
         {
             _comPort = new ComPort();
             _comPort.GetComputerConfigByPCName(Environment.MachineName);
             //_comPort.GetPortConfigByPCName(Environment.MachineName);  //liufu's design             
         }

        private void InitializeSerialPort()
        {
            try
            {
                // Create a new SerialPort object with default settings.
                _serialPort = new SerialPort();

                // Allow the user to set the appropriate properties.
                _serialPort.PortName = SetPortName(_serialPort.PortName);
                _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
                _serialPort.Parity = SetPortParity(_serialPort.Parity);
                _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
                _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
                _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 1000;
                _serialPort.WriteTimeout = 1000;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(100);
                int bytes = _serialPort.BytesToRead;
                byte[] buffer = new byte[bytes];
                if (bytes == 0)
                { return; }
                _serialPort.Read(buffer, 0, bytes);

                _receivedThread = new Thread(() => WeighValue = System.Text.Encoding.Default.GetString(buffer));
                _receivedThread.Start();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _serialPort.Close();
            }
        }


        public void Start()
        {
            InitializeSerialPort();

            try
            {
                if (!_serialPort.IsOpen)
                {
                    Thread.Sleep(100);
                    _serialPort.Open();
                }
                _blComOpen = true;
            }
            catch (Exception ex)
            {
                _blComOpen = false;
                throw ex;
            }
            _serialPort.WriteLine(CONST_STARTCHAR);
        }

        public void ParserValue()
        {
            try
            {
                int nTryCount = 0;
                while (_receivedThread == null || _receivedThread.IsAlive)
                {
                    Thread.Sleep(10);
                    nTryCount = nTryCount + 1;

                    if (nTryCount > 10)
                    {
                        nTryCount = 0;
                        throw new WeighException("${res:Global.DeviceIsNotOpen}", null);
                    }
                }

                if (WeighValue != null)
                {
                    if (WeighValue.StartsWith(CONST_COLSECHAR))
                    {
                        //throw new Exception("${res:Global.DeviceIsNotOpen}");
                        throw new WeighException("${res:Global.DeviceIsNotOpen}", null);
                    }

                    if (WeighValue.StartsWith(CONST_STEADYCHAR))
                    {
                        WeighValue = WeighValue.Replace(CONST_STEADYCHAR, string.Empty);
                        WeighValue = WeighValue.Replace(CONST_UNITCHAR, string.Empty);
                        WeighValue = WeighValue.Trim();
                    }
                }
                else
                {
                    WeighValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool PortOpen
        {
            get
            {
                return _blComOpen;
            }
        }

        public List<string> GetPortNames()
        {
            List<string> portList = new List<string>();
            foreach (string port in SerialPort.GetPortNames())
            {
                portList.Add(port);
            }

            return portList;
        }

        private string SetPortName(string defaultPortName)
        {
            string portName;

            portName = _comPort.PortName;

            if (portName == "")
            {
                portName = defaultPortName;
            }
            return portName;
        }

        private int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            baudRate = _comPort.BaudRate;

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        private Parity SetPortParity(Parity defaultPortParity)
        {
            string parity = string.Empty;

            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                break;
            }

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity);
        }

        private int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits = string.Empty;

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits);
        }

        private StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits = string.Empty;

            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                break;
            }

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits);
        }

        private Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handShake = string.Empty;

            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                break;
            }

            if (handShake == "")
            {
                handShake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handShake);
        }

        public string WeighValue { get; set; }

        public const string CONST_STEADYCHAR = "S S";
        public const string CONST_COLSECHAR = "EL";
        public const string CONST_STARTCHAR = "S";
        public const string CONST_RESETCHAR = "Z";
        public const string CONST_UNITCHAR = "g";
    }


   

}
