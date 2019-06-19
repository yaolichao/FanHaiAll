/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.IO;
using System.Text;
using System.IO.Ports;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FanHai.Hemera.Utils.Helper;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
#endregion

namespace BarCodePrint
{
    public class CodePrint
    {
        /// <summary>
        /// Print barcode function
        /// </summary>
        /// <param name="barCodes">Barcode list</param>
        /// <param name="labelName">Label name</param>
        /// <param name="printerName">Printer Name</param>
        /// <param name="printerIP">Printer ip</param>
        /// <param name="printerPort">Printer port</param>
        /// <param name="portType">Port type</param>
        /// <returns>Print succeeded barcode count</returns>
        public static int BarCodePrint(List<BarCode> barCodes, string labelName, string printerName, string printerIP, string printerPort, PortType portType)
        {
            try
            {
                List<BarCode> printedBarCodes = new List<BarCode>();

                string labelPath = AppDomain.CurrentDomain.BaseDirectory + @"Labels\" + labelName + ".txt";
                StreamReader streamReader = new StreamReader(labelPath);
                string labelContent = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();

                switch (portType)
                {
                    case PortType.Network:
                        NetworkPrint(barCodes, labelContent, printerIP, printerPort, ref printedBarCodes);
                        break;
                    case PortType.Local:
                        LocalPrint(barCodes, labelContent, printerName, ref printedBarCodes);
                        break;
                    case PortType.Parallel:
                        ParallelPrint(barCodes, labelContent, printerPort, ref printedBarCodes);
                        break;
                    case PortType.Serial:
                        SerialPrint(barCodes, labelContent, printerPort, ref printedBarCodes);
                        break;
                    default:
                        break;
                }

                return printedBarCodes.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Print barcode via network temp it's delete in the future
        /// <summary>
        /// Print barcode via network ip
        /// </summary>
        /// <param name="barCodes">Barcode list</param>
        /// <param name="labelName">Dictate set name</param>
        /// <param name="printerIP">Printer ip</param>
        /// <param name="printerPort">Printer port</param>
        /// <returns>Print succeeded barcode count</returns>
        public static int BarCodePrint(List<BarCode> barCodes, string labelName, string printerIP, int printerPort)
        {
            try
            {
                object codeLock = new object();
                List<BarCode> printedBarCodes = new List<BarCode>();

                string labelPath = AppDomain.CurrentDomain.BaseDirectory + @"Labels\" + labelName + ".txt";
                StreamReader streamReader = new StreamReader(labelPath);
                string strTemplate = streamReader.ReadToEnd();
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(printerIP, printerPort);
                foreach (BarCode barCode in barCodes)
                {
                    lock (codeLock)
                    {
                        try
                        {
                            string strDictate = strTemplate.Replace("#BARCODE#", barCode.BatteryCellCode);
                            strDictate = strDictate.Replace("#QUANTITY#", barCode.BatteryQty);
                            strDictate = strDictate.Replace("#LINE#",barCode.Line);
                            strDictate = strDictate.Replace("#ORDER#",barCode.OrderNumber);
                            int i = socket.Send(Encoding.ASCII.GetBytes(strDictate));
                            printedBarCodes.Add(barCode);
                        }
                        catch (SocketException ex)
                        {
                            return printedBarCodes.Count;
                            throw ex;
                        }
                    }
                }
                streamReader.Close();
                streamReader.Dispose();
                socket.Disconnect(true);
                return printedBarCodes.Count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Print barcode via network
        /// </summary>
        /// <param name="barCodes">Barcode list</param>
        /// <param name="labelContent">Label content</param>
        /// <param name="printerIP">Printer ip</param>
        /// <param name="printerPort">Printer port</param>
        /// <param name="printedBarCodes">Print succeeded barcode</param>
        /// <returns>Boolean value indication print succeeded or failed</returns>
        private static bool NetworkPrint(List<BarCode> barCodes, string labelContent, string printerIP, string printerPort, ref List<BarCode> printedBarCodes)
        {
            try
            {
                foreach (BarCode barCode in barCodes)
                {
                    try
                    {
                        string myContent = labelContent.Replace("#BARCODE#", barCode.BatteryCellCode);
                        myContent = myContent.Replace("#QUANTITY#", barCode.BatteryQty);
                        myContent = myContent.Replace("#LINE#", barCode.Line);
                        myContent = myContent.Replace("#ORDER#", barCode.OrderNumber);
                        myContent = myContent.Replace("#STEPDESC#", barCode.StepDesc);
                        myContent = myContent.Replace("#EXTENDITEM1#", barCode.ExtendItem1);
                        myContent = myContent.Replace("#EXTENDITEM2#", barCode.ExtendItem2);
                        myContent = myContent.Replace("#EXTENDITEM3#", barCode.ExtendItem3);
                        myContent = myContent.Replace("#EXTENDITEM4#", barCode.ExtendItem4);
                        myContent = myContent.Replace("#EXTENDITEM5#", barCode.ExtendItem5);
                        if (NetWorkPrinterHelper.SendStringToPrinter(printerIP, printerPort, myContent))
                        {
                            printedBarCodes.Add(barCode);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (SocketException ex)
                    {
                        throw ex;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Print Barcode Via Local Printer
        /// </summary>
        /// <param name="barCodes"></param>
        /// <param name="labelContent"></param>
        /// <param name="printerName"></param>
        /// <param name="printedBarCodes"></param>
        /// <returns></returns>
        private static bool LocalPrint(List<BarCode> barCodes, string labelContent, string printerName, ref List<BarCode> printedBarCodes)
        {
            int count = 0;
            while(count<barCodes.Count){
                //foreach (BarCode barCode in barCodes)
                int init = count;
                int max = count + 4;
                string barcodeCommand = labelContent;
                for (int i=init; i < max && i < barCodes.Count; i++)
                {
                    BarCode barCode = barCodes[i];
                    barcodeCommand = barcodeCommand.Replace(string.Format("#BARCODE{0}#", i - init), barCode.BatteryCellCode);
                    barcodeCommand = barcodeCommand.Replace(string.Format("#QUANTITY{0}#", i - init), barCode.BatteryQty);
                    //barcodeCommand = barcodeCommand.Replace("#LINE1#", barCode.Line);
                    //barcodeCommand = barcodeCommand.Replace("#ORDER1#", barCode.OrderNumber);
                    //barcodeCommand = barcodeCommand.Replace("#STEPDESC1#", barCode.StepDesc);
                    //barcodeCommand = barcodeCommand.Replace("#EXTENDITEM11#", barCode.ExtendItem1);
                    //barcodeCommand = barcodeCommand.Replace("#EXTENDITEM12#", barCode.ExtendItem2);
                    //barcodeCommand = barcodeCommand.Replace("#EXTENDITEM13#", barCode.ExtendItem3);
                    //barcodeCommand = barcodeCommand.Replace("#EXTENDITEM14#", barCode.ExtendItem4);
                    //barcodeCommand = barcodeCommand.Replace("#EXTENDITEM15#", barCode.ExtendItem5);
                    count++;
                }
                barcodeCommand=System.Text.RegularExpressions.Regex.Replace(barcodeCommand, "#.+#", string.Empty);
                if (!string.IsNullOrEmpty(barcodeCommand) && RAWPrinterHelper.SendStringToPrinter(printerName, barcodeCommand))
                {
                    for (int i = init; i < max && i < barCodes.Count; i++)
                    {
                        BarCode barCode = barCodes[i];
                        printedBarCodes.Add(barCode);
                    }
                }
            }
            if (printedBarCodes.Count != barCodes.Count)
                return false;
            return true;
        }


        #region Print barcode via serial port
        /// <summary>
        /// Print barcode via serial port
        /// </summary>
        /// <param name="barCodes">Barcode list</param>
        /// <param name="labelContent">Label content</param>
        /// <param name="printerPort">Printer port</param>
        /// <param name="printedBarCodes">Print succeeded barcode</param>
        /// <returns>Boolean value indication print succeeded or failed</returns>
        private static bool SerialPrint(List<BarCode> barCodes, string labelContent, string printerPort, ref List<BarCode> printedBarCodes)
        {
            try
            {
                foreach (BarCode barCode in barCodes)
                {
                    string myContent = labelContent.Replace("#BARCODE#", barCode.BatteryCellCode);
                    myContent = myContent.Replace("#QUANTITY#", barCode.BatteryQty);
                    myContent = myContent.Replace("#LINE#", barCode.Line);
                    myContent = myContent.Replace("#ORDER#", barCode.OrderNumber);
                    myContent = myContent.Replace("#STEPDESC#", barCode.StepDesc);
                    myContent = myContent.Replace("#EXTENDITEM1#", barCode.ExtendItem1);
                    myContent = myContent.Replace("#EXTENDITEM2#", barCode.ExtendItem2);
                    myContent = myContent.Replace("#EXTENDITEM3#", barCode.ExtendItem3);
                    myContent = myContent.Replace("#EXTENDITEM4#", barCode.ExtendItem4);
                    myContent = myContent.Replace("#EXTENDITEM5#", barCode.ExtendItem5);
                    SerialPrinterHelper.SendStringToPrinter(printerPort,myContent);
                    printedBarCodes.Add(barCode);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Print barcode via parallel port
        /// </summary>
        /// <param name="barCodes">Barcode list</param>
        /// <param name="labelContent">Label content</param>
        /// <param name="printerPort">Printer port</param>
        /// <param name="printedBarCodes">Print succeeded barcode</param>
        /// <returns>Boolean value indication print succeeded or failed</returns>
        private static bool ParallelPrint(List<BarCode> barCodes, string labelContent, string printerPort, ref List<BarCode> printedBarCodes)
        {
            try
            {

                foreach (BarCode barCode in barCodes)
                {
                    string myContent = labelContent.Replace("#BARCODE#", barCode.BatteryCellCode);
                    myContent = myContent.Replace("#QUANTITY#", barCode.BatteryQty);
                    myContent = myContent.Replace("#LINE#", barCode.Line);
                    myContent = myContent.Replace("#ORDER#", barCode.OrderNumber);
                    myContent = myContent.Replace("#STEPDESC#", barCode.StepDesc);
                    myContent = myContent.Replace("#EXTENDITEM1#", barCode.ExtendItem1);
                    myContent = myContent.Replace("#EXTENDITEM2#", barCode.ExtendItem2);
                    myContent = myContent.Replace("#EXTENDITEM3#", barCode.ExtendItem3);
                    myContent = myContent.Replace("#EXTENDITEM4#", barCode.ExtendItem4);
                    myContent = myContent.Replace("#EXTENDITEM5#", barCode.ExtendItem5);

                    if (!ParallelPrinterHelper.SendStringToPrinter(printerPort, myContent))
                    {
                        return false;
                    }
                   
                    printedBarCodes.Add(barCode);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
