using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class BATTERY_ABNORMITY_MEMORY_FIELDS :TableFields 
    {
        public const string FIELDS_COMPUTER_NAME = "COMPUTER_NAME";
        public const string FIELDS_STATUS = "STATUS";
        public const string FIELDS_BATTERY_COLOR = "BATTERY_COLOR";
        public const string FIELDS_BATTERY_COLOR_CODE = "BATTERY_COLOR_CODE";
        public const string FIELDS_BATTERY_THICKNESS = "BATTERY_THICKNESS";
        public const string FIELDS_BATTERY_THICKNESS_CODE = "BATTERY_THICKNESS_CODE";
        public const string FIELDS_BATTERY_LB_PACKAGE = "BATTERY_LB_PACKAGE";
        public const string FIELDS_BATTERY_LINE = "BATTERY_LINE";
        public const string FIELDS_BATTERY_WORKER_ORDER = "BATTERY_WORKER_ORDER";
        public const string FIELDS_BATTERY_TYPE = "BATTERY_TYPE";
        public const string FIELDS_BATTERY_SERIAL = "BATTERY_SERIAL";
        public const string FIELDS_BATTERY_QTY = "BATTERY_QTY";
        public const string FIELDS_BATTERY_COMBINATION_FLAG = "BATTERY_COMBINATION_FLAG";
        public const string FIELDS_BATTERY_BARCODE = "BATTERY_BARCODE";
        public const string FIELDS_BATTERY_IF_PACKAGE = "BATTERY_IF_PACKAGE";
        public const string FIELDS_DATES = "DATES";
        public const string FIELDS_BATTERY_TEST_TYPE = "BATTERY_TEST_TYPE";
        //public const string FIELDS_LAST_TIME = "LAST_TIME";

        public const string DATABASE_TABLE_NAME = "BATTERY_ABNORMITY_MEMORY";

        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_COMPUTER_NAME, new FieldProperties() },
                                                                  {FIELDS_STATUS, new FieldProperties() }, 
                                                                  {FIELDS_BATTERY_COLOR, new FieldProperties() }, 
                                                                  {FIELDS_BATTERY_COLOR_CODE, new FieldProperties() },                                                                
                                                                  {FIELDS_BATTERY_THICKNESS, new FieldProperties() },
                                                                  {FIELDS_BATTERY_THICKNESS_CODE, new FieldProperties() },
                                                                  {FIELDS_BATTERY_LB_PACKAGE, new FieldProperties() }, 
                                                                  {FIELDS_BATTERY_LINE, new FieldProperties() },
                                                                  {FIELDS_BATTERY_WORKER_ORDER, new FieldProperties() }, 
                                                                  {FIELDS_BATTERY_TYPE, new FieldProperties() },  
                                                                  {FIELDS_BATTERY_SERIAL, new FieldProperties() },                                                                  
                                                                  {FIELDS_BATTERY_QTY, new FieldProperties() },                                                                  
                                                                  {FIELDS_BATTERY_COMBINATION_FLAG, new FieldProperties() }, 
                                                                  {FIELDS_BATTERY_BARCODE, new FieldProperties() }, 
                                                                  {FIELDS_BATTERY_IF_PACKAGE, new FieldProperties() },  
                                                                  {FIELDS_DATES, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_BATTERY_TEST_TYPE,new FieldProperties()}
                                                                  //{FIELDS_LAST_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }
                                                              };                                                  
    }
}
