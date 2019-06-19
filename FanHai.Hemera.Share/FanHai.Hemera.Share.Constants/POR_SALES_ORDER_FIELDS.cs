using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_SALES_ORDER_FIELDS : TableFields
    {
        public const string FIELD_SALES_ORDER_KEY   = "SALES_ORDER_KEY";
        public const string FIELD_ORDER_NUMBER      = "ORDER_NUMBER";
        public const string FIELD_ORDER_STATE       = "ORDER_STATE";
        public const string FIELD_ORDER_PRIORITY    = "ORDER_PRIORITY";
        public const string FIELD_DESCRIPTIONS      = "DESCRIPTIONS";
        public const string FIELD_ENTERED_TIME      = "ENTERED_TIME";
        public const string FIELD_PROMISED_TIME     = "PROMISED_TIME";
        public const string FIELD_FINISHED_TIME     = "FINISHED_TIME";
        public const string FIELD_SHIPPED_TIME      = "SHIPPED_TIME";
        public const string FIELD_CLOSED_TIME       = "CLOSED_TIME";

        public const string FIELD_CREATOR           = "CREATOR";
        public const string FIELD_CREATE_TIME       = "CREATE_TIME";
        public const string FIELD_CREATE_TIMEZONE   = "CREATE_TIMEZONE";
        public const string FIELD_EDITOR            = "EDITOR";
        public const string FIELD_EDIT_TIME         = "EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE     = "EDIT_TIMEZONE";

        public const string DATABASE_TABLE_NAME     = "POR_SALES_ORDER";


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
                                                                  {FIELD_SALES_ORDER_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELD_ORDER_NUMBER, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELD_ORDER_STATE, new FieldProperties(typeof(Int32), false, 0) },
                                                                  {FIELD_ORDER_PRIORITY, new FieldProperties(typeof(Int32), false, 0) },
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties(typeof(string), true, 0) },
                                                                  {FIELD_ENTERED_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELD_PROMISED_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELD_FINISHED_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_SHIPPED_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELD_CLOSED_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_CREATOR, new FieldProperties(typeof(string), true, "") },
                                                                  {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELD_CREATE_TIMEZONE, new FieldProperties(typeof(string), true, "") },
                                                                  {FIELD_EDITOR, new FieldProperties(typeof(string), true, "") },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties(typeof(string), true, "") }
                                                                };

    }
}
