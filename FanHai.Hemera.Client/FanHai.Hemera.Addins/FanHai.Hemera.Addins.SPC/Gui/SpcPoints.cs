using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraCharts;
using FanHai.Hemera.Share.Constants;
using System.Data;

namespace FanHai.Hemera.Addins.SPC
{
    /// <summary>
    /// SPC分组参数
    /// </summary>
    /// Owner by genchille.yang 2012-04-14 10:11:35
    public class SpcPoints
    {
        /// <summary>
        /// 子分组的平均值
        /// </summary>
        public double value = 0;
        /// <summary>
        /// 采样点数值统计
        /// </summary>
        public List<KeyValuePair<string, decimal>> listPoint = new List<KeyValuePair<string, decimal>>();
        /// <summary>
        /// 删除标记， 1:删除 默认为0
        /// </summary>
        public int deleteFlag = 0;
        /// <summary>
        /// 参数是否有效(0:无效，1:有效，2:已处理)
        /// </summary>
        public int validateFlag = 0;
        /// <summary>
        /// 数据是否被修正，1异常修正，2,标注信息，默认为0
        /// </summary>
        public int editFlag = 0;
        /// <summary>
        /// 该点是否已经为红色
        /// </summary>
        public int redFlag = 0;
        /// <summary>
        /// SPC数据采集日期或者修正后的日期
        /// </summary>
        public string createTime = string.Empty;
        /// <summary>
        /// SPC数据采集批次的供应商
        /// </summary>
        public string supplier = string.Empty;
        //---------------------------------------------------
        /// <summary>
        /// Point key
        /// </summary>
        public string pointkeys = string.Empty;
        /// <summary>
        /// SPC数据采集批次号
        /// </summary>
        public string lotNumber = string.Empty;
        /// <summary>
        /// 数据采集键值
        /// </summary>
        public string edc_ins_key = string.Empty;
        /// <summary>
        /// 异常规则描述
        /// </summary>
        public string abnormalRules = string.Empty;
    }

    public class ChartType
    {
        public ChartType()
        { 
        }
        public static DataTable GetChartType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TYPECODE");
            dt.Rows.Add("XBAR-S");
            dt.Rows.Add("XBAR-R");
            dt.Rows.Add("XBAR-MR");
            dt.Rows.Add("XBAR");
            dt.AcceptChanges();

            return dt;
        }

        public const string XBAR_S = "XBAR-S";
        public const string XBAR_R = "XBAR-R";
        public const string XBAR_MR = "XBAR-MR";
        public const string XBAR = "XBAR";

        public const string ExportExcel = "ExportExcel";
        public const string ExportImg = "ExportImg";
        public const string PrintPreview = "PrintPreview";
    }

    public class ExportType
    {
        public const string Excel = "ExportExcel";
        public const string Img = "ExportImg";
        public const string PrintPreview = "PrintPreview";

        public const string TitleHtml = "HTML Document";
        public const string FilterHtml = "HTML Documents (*.htm; *.html)|*.htm; *.html";
        public const string ExportFormatHtml = "HTML";

        public const string TitleMht = "MHT Document";
        public const string FilterMht = "MHT Documents (*.mht)|*.mht";
        public const string ExportFormatMht = "MHT";

        public const string TitlePdf = "PDF Document";
        public const string FilterPdf = "PDF Documents (*.pdf)|*.pdf";
        public const string ExportFormatPdf = "PDF";

        public const string TitleXLS = "XLS Document";
        public const string FilterXLS = "XLS Documents (*.xls)|*.xls";
        public const string ExportFormatXLS = "XLS";

        public const string TitleImg = "JPEG image";
        public const string FilterImg = "JPEG image (*.JPG;*.JPEG;*.JPE;*.JFIF)|*.JPG;*.JPEG;*.JPE;*.JFIF";
        public const string ExportFormatImg = "IMAGE";

        public const string ConfirmOpenFiled = "Do you want to open this file?";
        public const string ExportTo = "Export To.";
        public const string Open = "open";

        public const string AlertError = "Cannot find an application on your system suitable for openning the file with exported data.";
        
    }
    public class X_PIE
    {
        private string _Date = string.Empty;
        private string _Suppler = string.Empty;
        private string _LotNumber = string.Empty;
        private bool _blDate = true;

        public string sDate {
            get { return _Date; }
            set { _Date = value; }
        }
        public string sSuppler
        {
            get { return _Suppler; }
            set { _Suppler = value; }
        }
        public string sLotNumber
        {
            get { return _LotNumber; }
            set { _LotNumber = value; }
        }
        public bool blDate
        {
            get { return _blDate; }
            set { _blDate = value; }
        }

        public override string ToString()
        {
            if (_blDate)
                return this._Date;
            else
                return this._Suppler;
        }
    }

    public class ConstantSPC
    {
        string _type = string.Empty;
        public ConstantSPC(string type)
        {
            _type = type;
        }
        public const string C4 = "C4";
        public const string D2 = "D2";
        public const string D3 = "D3";


        private static List<ConstantItem> c4 = new List<ConstantItem>() { 
            new ConstantItem(2,0.7979), 
            new ConstantItem(3,0.8862) ,
            new ConstantItem(4,0.9213),
            new ConstantItem(5,0.9400),
            new ConstantItem(6,0.9515),
            new ConstantItem(7,0.9594),
            new ConstantItem(8,0.9650),
            new ConstantItem(9,0.9693),
            new ConstantItem(10,0.9727)
        };

        private static List<ConstantItem> d2 = new List<ConstantItem>() { 
            new ConstantItem(2,1.128), 
            new ConstantItem(3,1.693) ,
            new ConstantItem(4,2.059),
            new ConstantItem(5,2.326),
            new ConstantItem(6,2.534),
            new ConstantItem(7,2.704),
            new ConstantItem(8,2.847),
            new ConstantItem(9,2.970),
            new ConstantItem(10,3.078)
        };
        private static List<ConstantItem> d3 = new List<ConstantItem>() { 
            new ConstantItem(2,0.853), 
            new ConstantItem(3,0.888) ,
            new ConstantItem(4,0.880),
            new ConstantItem(5,0.864),
            new ConstantItem(6,0.848),
            new ConstantItem(7,0.833),
            new ConstantItem(8,0.820),
            new ConstantItem(9,0.808),
            new ConstantItem(10,0.797)
        };

        private Dictionary<string, List<ConstantItem>> constant_Fields = new Dictionary<string, List<ConstantItem>>{
                                                                                {C4,c4},
                                                                                {D2,d2}, 
                                                                                {D3,d3}
                                                                                };

        public ConstantItem GetConstantItem(int v)
        {
            ConstantItem cItem = new ConstantItem();

            foreach (KeyValuePair<string, List<ConstantItem>> kvp in constant_Fields)
            {
                if (_type == kvp.Key)
                {
                    foreach (ConstantItem _item in kvp.Value)
                    {
                        if (_item.Key == v)
                        {
                            cItem = _item;
                            break;
                        }
                    }
                }
            }
            return cItem;
        }

    }
    public class ConstantItem
    {
        private int _key = 0;
        private double _value = 0;
        public ConstantItem()
        { }
        public ConstantItem(int key, double value)
        {
            _key = key;
            _value = value;
        }

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
}
