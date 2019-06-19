using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class BATTERY_PACKAGE_COMBINED_FIELDS : TableFields 
    {
        public const string FIELDS_PACKETNUMBER="PACKETNUMBER";
        public const string FIELDS_LOT="LOT";
        public const string FIELDS_NUMS="NUMS";
        public const string FIELDS_DATES="DATES";

        public const string DATABASE_TABLE_NAME = "BATTERY_PACKAGE_COMBINED";

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
                                                                  {FIELDS_PACKETNUMBER, new FieldProperties() },
                                                                  {FIELDS_LOT, new FieldProperties() },
                                                                  {FIELDS_NUMS, new FieldProperties() },                                                                  
                                                                  {FIELDS_DATES, new FieldProperties(typeof(DateTime),true,DateTime.Now) }
                                                                };

    }
}
