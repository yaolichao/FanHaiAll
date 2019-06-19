using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils.Entities;
using System.IO.Ports;
using System.Windows.Forms;
using SolarViewer.Hemera.Utils.Entities.Exceptions;
using System.Threading;

namespace WeighSerialPort
{
    /// <summary>
    /// Balance Scale Error Enumerable
    /// </summary>
    /// Owner:Andy Gao 2011-05-02 13:30:45
    public enum BalanceScaleError : byte
    {
        NONE = 0,
        TIMEOUT,
        SUCCESS,
        NOTEXECUTABLE,
        OVERLOAD,
        UNDERLOAD,
        SYNTAXERROR,
        TRANSERROR,
        RECEIVEERROR,
        LOGICALERROR,
        DATAERROR,
        UNKNOWNERROR
    }

    /// <summary>
    /// Balance Scale Helper Class
    /// </summary>
    /// Owner:Andy Gao 2011-04-29 12:42:30
    public class BalanceScaleHelper
    {
        private const string CONST_STARTCHAR = "S";
        private const string CONST_STEADYCHAR = "S S";
        private const string CONST_UNITCHAR = "g\r";
        private const string CONST_NOTEXECUTABLECHAR = "S I";
        private const string CONST_OVERLOADCHAR = "S +";
        private const string CONST_UNDERLOADCHAR = "S -";
        private const string CONST_SYNTAXERRORCHAR = "ES";
        private const string CONST_TRANSERRORCHAR = "ET";
        private const string CONST_LOGICALERRORCHAR = "EL";
        private const int CONST_TIMEOUT = 60000;

        private ComPort comPort = null;
        private SerialPort serialPort = null;
        private AutoResetEvent syncEvent = null;

        public string CurrentWeightValue { get; private set; }

        public BalanceScaleError ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public BalanceScaleHelper()
        {
            this.comPort = new ComPort();
            this.comPort.GetComputerConfigByPCName(Environment.MachineName);

            this.serialPort = new SerialPort();

            this.serialPort.PortName = SetPortName(serialPort.PortName);
            this.serialPort.BaudRate = SetPortBaudRate(serialPort.BaudRate);
            this.serialPort.Parity = SetPortParity(serialPort.Parity);
            this.serialPort.DataBits = SetPortDataBits(serialPort.DataBits);
            this.serialPort.StopBits = SetPortStopBits(serialPort.StopBits);
            this.serialPort.Handshake = SetPortHandshake(serialPort.Handshake);
            this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            this.serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);

            this.ErrorCode = BalanceScaleError.NONE;
            this.ErrorMessage = string.Empty;

            this.syncEvent = new AutoResetEvent(false);
        }

        private void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            this.ErrorCode = BalanceScaleError.RECEIVEERROR;
            this.ErrorMessage = string.Format("The balance communication generate receive error is {0}", e.EventType.ToString());

            this.syncEvent.Set();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.serialPort != null)
            {
                if (e.EventType == SerialData.Chars)
                {
                    try
                    {
                        string receivedValue = this.serialPort.ReadLine();

                        if (receivedValue.StartsWith(CONST_SYNTAXERRORCHAR))
                        {
                            this.ErrorCode = BalanceScaleError.SYNTAXERROR;
                            this.ErrorMessage = "The balance has not recognized the received command.";
                        }
                        else if (receivedValue.StartsWith(CONST_TRANSERRORCHAR))
                        {
                            this.ErrorCode = BalanceScaleError.TRANSERROR;
                            this.ErrorMessage = "The balance has received a 'faulty' command.";
                        }
                        else if (receivedValue.StartsWith(CONST_LOGICALERRORCHAR))
                        {
                            this.ErrorCode = BalanceScaleError.LOGICALERROR;
                            this.ErrorMessage = "The balance can not execute the received command.";
                        }
                        else if (receivedValue.StartsWith(CONST_NOTEXECUTABLECHAR))
                        {
                            this.ErrorCode = BalanceScaleError.NOTEXECUTABLE;
                            this.ErrorMessage = "The balance command not executable.";
                        }
                        else if (receivedValue.StartsWith(CONST_OVERLOADCHAR))
                        {
                            this.ErrorCode = BalanceScaleError.OVERLOAD;
                            this.ErrorMessage = "The balance in overload range.";
                        }
                        else if (receivedValue.StartsWith(CONST_UNDERLOADCHAR))
                        {
                            this.ErrorCode = BalanceScaleError.UNDERLOAD;
                            this.ErrorMessage = "The balance in underload range.";
                        }
                        else if (receivedValue.StartsWith(CONST_STEADYCHAR) && receivedValue.EndsWith(CONST_UNITCHAR))
                        {
                            receivedValue = receivedValue.Replace(CONST_STEADYCHAR, string.Empty);
                            receivedValue = receivedValue.Replace(CONST_UNITCHAR, string.Empty);
                            receivedValue = receivedValue.Trim();

                            this.CurrentWeightValue = receivedValue;

                            this.ErrorCode = BalanceScaleError.SUCCESS;
                            this.ErrorMessage = string.Empty;
                        }
                        else
                        {
                            this.ErrorCode = BalanceScaleError.DATAERROR;
                            this.ErrorMessage = string.Format("The balance has received an error '{0}' data.", receivedValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ErrorCode = BalanceScaleError.UNKNOWNERROR;
                        this.ErrorMessage = string.Format("The balance communication generate exception error is {0}.", ex.Message);
                    }
                }
                else
                {
                    this.ErrorCode = BalanceScaleError.RECEIVEERROR;
                    this.ErrorMessage = "The balance has received end of file character.";
                }
            }

            this.syncEvent.Set();
        }

        private string SetPortName(string defaultPortName)
        {
            if (this.comPort == null || string.IsNullOrEmpty(this.comPort.PortName))
            {
                return defaultPortName;
            }
            else
            {
                return this.comPort.PortName;
            }
        }

        private int SetPortBaudRate(int defaultPortBaudRate)
        {
            if (this.comPort == null || string.IsNullOrEmpty(this.comPort.BaudRate))
            {
                return defaultPortBaudRate;
            }
            else
            {
                return Convert.ToInt32(this.comPort.BaudRate);
            }
        }

        private Parity SetPortParity(Parity defaultPortParity)
        {
            return defaultPortParity;
        }

        private int SetPortDataBits(int defaultPortDataBits)
        {
            return defaultPortDataBits;
        }

        private StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            return defaultPortStopBits;
        }

        private Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            return defaultPortHandshake;
        }

        public void Open()
        {
            if (this.serialPort != null && !this.serialPort.IsOpen)
            {
                this.serialPort.Open();
            }
        }

        public void GetStableWeightValue()
        {
            if (this.serialPort != null && this.serialPort.IsOpen)
            {
                this.serialPort.WriteLine(CONST_STARTCHAR);

                this.syncEvent.WaitOne(CONST_TIMEOUT);

                if (this.ErrorCode == BalanceScaleError.NONE)
                {
                    this.ErrorCode = BalanceScaleError.TIMEOUT;
                    this.ErrorMessage = "The balance communication generate timeout.";
                }
            }
        }

        public void Close()
        {
            if (this.serialPort != null && this.serialPort.IsOpen)
            {
                this.serialPort.Close();
            }
        }

        public void Dispose()
        {
            if (this.serialPort != null)
            {
                this.serialPort.Dispose();
            }
        }
    }
}
