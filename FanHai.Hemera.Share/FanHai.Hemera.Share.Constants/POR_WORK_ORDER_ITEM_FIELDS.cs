using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_WORK_ORDER_ITEM_FIELDS : TableFields
    {
        public const string FIELD_WORK_ORDER_KEY                = "WORK_ORDER_KEY";
        public const string FIELD_SALES_ORDER_ITEM_KEY          = "SALES_ORDER_ITEM_KEY";
        public const string FIELD_SALES_ORDER_ITEM_QUANTITY     = "SALES_ORDER_ITEM_QUANTITY";
        public const string FIELD_SALES_ORDER_ITEM_JOIN_TIME    = "SALES_ORDER_ITEM_JOIN_TIME";
        public const string FIELD_EDITOR                        = "EDITOR";
        public const string FIELD_EDIT_TIME                     = "EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE                 = "EDIT_TIMEZONE";

        public const string DATABASE_TABLE_NAME                 = "POR_WORK_ORDER_ITEM";

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
                                                                  {FIELD_WORK_ORDER_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELD_SALES_ORDER_ITEM_KEY, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_SALES_ORDER_ITEM_QUANTITY, new FieldProperties(typeof(Int64),true,0) },
                                                                  {FIELD_SALES_ORDER_ITEM_JOIN_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_EDITOR, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties(typeof(string),true,"") }
                                                                };
    }
}
