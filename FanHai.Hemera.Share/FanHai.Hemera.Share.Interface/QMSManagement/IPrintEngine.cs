using System;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IPrintEngine
    {
        int BarCodePrint(DataTable barCodes, string labelName, string printerIP, string printerName);
    }
}
