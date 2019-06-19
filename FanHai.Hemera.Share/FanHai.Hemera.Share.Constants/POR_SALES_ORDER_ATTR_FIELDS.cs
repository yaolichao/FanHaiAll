using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_SALES_ORDER_ATTR_FIELDS : TableFields
    {
        public const string FIELDS_ATTRIBUTE_KEY = "ATTRIBUTE_KEY";
        public const string FIELDS_ATTRIBUTE_NAME = "ATTRIBUTE_NAME";
        public const string FIELDS_SALES_ORDER_KEY = "SALES_ORDER_KEY";
        public const string FIELDS_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";

        public const string DATABASE_TABLE_NAME = "POR_SALES_ORDER_ATTR";
        
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
                                                                  {FIELDS_ATTRIBUTE_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_ATTRIBUTE_NAME, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_ATTRIBUTE_VALUE, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_SALES_ORDER_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_EDITOR, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties(typeof(string), false, "") } 
                                                                };
    }
}
