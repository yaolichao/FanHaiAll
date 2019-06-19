/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using LabelManager2;
using FanHai.Hemera.Utils;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace FanHai.Hemera.Utils
{
    public class PrintPool
    {
        static Queue<PrintLibrary> printQueue = new Queue<PrintLibrary>(5);

        public static int BarCodePrint(DataTable barCodes, string labelName, string printerIP, string printerName)
        {
            PrintLibrary printLibrary = null;

            if (printQueue.Count < 5)
            {
                printLibrary = new PrintLibrary();
                printQueue.Enqueue(printLibrary);
            }

            printLibrary = printQueue.Dequeue();
            printQueue.Enqueue(printLibrary);

            try
            {
                lock (printLibrary)
                {
                    List<string> printedBarCodes = new List<string>();

                    string labelPath = Environment.CurrentDirectory + @"\Labels\" + labelName + ".Lab";
                    IPAddress[] localIPAddress = Dns.GetHostAddresses(Environment.MachineName);

                    SetPrinter(printerName);

                    if (printLibrary.Documents.Count < 1 || printLibrary.ActiveDocument == null ||
                        printLibrary.ActiveDocument.Name != labelName + ".Lab")
                    {
                        printLibrary.Documents.CloseAll(false);
                        printLibrary.Documents.Open(labelPath, false);
                    }

                    printLibrary.EnableEvents = true;

                    Document activeDoc = printLibrary.ActiveDocument;

                    foreach (DataRow codeRow in barCodes.Rows)
                    {

                        activeDoc.Variables.FreeVariables.Item("vBarCode").Value = codeRow["batteryCellCode"].ToString();

                        if (activeDoc.PrintDocument(1) <= 0)
                        {
                            return printedBarCodes.Count;
                        }

                        printedBarCodes.Add(codeRow["batteryCellCode"].ToString());

                    }

                    return printedBarCodes.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int BarCodePrint(DataTable barCodes, string labelName, string printerIP, int printerPort)
        {
            try
            {
                List<string> printedBarCodes = new List<string>();

                string labelPath = Environment.CurrentDirectory + @"\Labels\" + labelName + ".txt";
                StreamReader streamReader = new StreamReader(labelPath);
                string strTemplate = streamReader.ReadToEnd();
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(printerIP, printerPort);

                foreach (DataRow codeRow in barCodes.Rows)
                {
                    lock (socket)
                    {
                        try
                        {
                            string strDictate = strTemplate.Replace("#BARCODE#", codeRow["batteryCellCode"].ToString());
                            socket.Send(Encoding.ASCII.GetBytes(strDictate));
                            printedBarCodes.Add(codeRow["batteryCellCode"].ToString());
                        }
                        catch (SocketException)
                        {
                            return printedBarCodes.Count;
                        }
                    }
                }

                streamReader.Close();
                streamReader.Dispose();

                return printedBarCodes.Count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void PrinterPoolClear()
        {
            printQueue.Clear();

            Process[] printerProcesses = Process.GetProcessesByName("lppa");

            foreach (Process process in printerProcesses)
            {
                process.Kill();
            }
        }

        private static void SetPrinter(string printerName)
        {
            bool printerIsExist = false;
            string sqlCommand = "SELECT * FROM Win32_Printer";
            ManagementObjectSearcher query = new ManagementObjectSearcher(sqlCommand);
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject printer in queryCollection)
            {
                if (string.Compare(printer["Name"].ToString(), printerName, true) == 0)
                {
                    printer.InvokeMethod("SetDefaultPrinter", null);
                    printerIsExist = true;
                    break;
                }
            }

            if (!printerIsExist)
            {
                if (AddPrinterConnection(printerName))
                {
                    SetDefaultPrinter(printerName);
                }
                else
                {
                    throw new Exception("添加打印机失败!");
                }
            }
        }

        [DllImport("winspool.drv")]
        private static extern bool AddPrinterConnection(string printerName);

        [DllImport("winspool.drv")]
        private static extern bool SetDefaultPrinter(string printerName);
    }

    public class PrintLibrary : ApplicationClass, IDisposable
    {
        public PrintLibrary()
        {
            //Note: Don't delete this class 
        }

        public void Dispose()
        {
            this.Quit();
            this.Dispose();
        }
    }
}
