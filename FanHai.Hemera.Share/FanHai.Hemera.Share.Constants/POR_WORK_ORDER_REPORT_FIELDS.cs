using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class POR_WORK_ORDER_REPORT_FIELDS : TableFields
    {


        public const string FIELD_WORK_ORDER_REPORT_AUFNR = "AUFNR"; 
        public const string FIELD_WORK_ORDER_REPORT_APLFL = "APLFL";
        public const string FIELD_WORK_ORDER_REPORT_GMNGA = "GMNGA";
        public const string FIELD_WORK_ORDER_REPORT_GMEIN = "GMEIN";
        public const string FIELD_WORK_ORDER_REPORT_XMNGA = "XMNGA";
        public const string FIELD_WORK_ORDER_REPORT_ISM04 = "ISM04";
        public const string FIELD_WORK_ORDER_REPORT_ILE04 = "ILE04";
        public const string FIELD_WORK_ORDER_REPORT_ISM05 = "ISM05";
        public const string FIELD_WORK_ORDER_REPORT_ILE05 = "ILE05";
        public const string FIELD_WORK_ORDER_REPORT_ISM06 = "ISM06";
        public const string FIELD_WORK_ORDER_REPORT_ILE06 = "ILE06";
        public const string FIELD_WORK_ORDER_REPORT_RMNGA = "RMNGA";
        public const string FIELD_WORK_ORDER_REPORT_RMZHL = "RMZHL";
        public const string FIELD_WORK_ORDER_REPORT_ACK_DATE = "ACK_DATA";
        public const string FIELD_WORK_ORDER_REPORT_ACK_TIME = "ACK_TIME";
        public const string FIELD_WORK_ORDER_REPORT_ROW_KEY = "ROW_KEY";
        public const string FIELD_WORK_ORDER_REPORT_TYPE = "TYPE";



        public const string DATABASE_TABLE_NAME = "POR_WORK_ORDER_REPORT";


        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_WORK_ORDER_REPORT_AUFNR, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_APLFL, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_GMNGA, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_GMEIN, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_XMNGA, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_ISM04, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_ILE04, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_ISM05, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_ILE05, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_ISM06, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_ILE06, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_RMNGA, new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_RMZHL, new FieldProperties() },



                                                                  {FIELD_WORK_ORDER_REPORT_ACK_DATE, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELD_WORK_ORDER_REPORT_ACK_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                 
                                                                  {FIELD_WORK_ORDER_REPORT_ROW_KEY,new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_REPORT_TYPE,new FieldProperties() },
                                                               
                                                                };
    }
}
