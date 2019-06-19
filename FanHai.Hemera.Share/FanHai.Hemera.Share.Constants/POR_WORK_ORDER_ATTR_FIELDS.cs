using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class POR_WORK_ORDER_ATTR_FIELDS : TableFields
    {
        public const string FIELDS_WORK_ORDER_ATTR_KEY = "WORK_ORDER_ATTR_KEY";
        public const string FIELDS_WORK_ORDER_KEY   = "WORK_ORDER_KEY";
        public const string FIELDS_ATTRIBUTE_KEY    = "ATTRIBUTE_KEY";
        public const string FIELDS_ATTRIBUTE_NAME   = "ATTRIBUTE_NAME";
        public const string FIELDS_ATTRIBUTE_VALUE  = "ATTRIBUTE_VALUE";
        public const string FIELDS_EDITOR           = "EDITOR";
        public const string FIELDS_EDIT_TIME        = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE    = "EDIT_TIMEZONE";

        public const string FIELDS_ATTRIBUTE_TYPE = "ATTRIBUTE_TYPE";
        public const string FIELDS_ISFLAG = "ISFLAG";
        public const string FIELDS_PRO_ID = "PRO_ID";

        public const string DATABASE_TABLE_NAME     = "POR_WORK_ORDER_ATTR";

        public const string DATABASE_TABLE_FORUPDATE = "POR_WORK_ORDER_ATTR_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WORK_ORDER_ATTR_INSERT";
        
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
                                                                  {FIELDS_WORK_ORDER_ATTR_KEY,new FieldProperties()},
                                                                  {FIELDS_WORK_ORDER_KEY,   new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_KEY,    new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_NAME,   new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_VALUE,  new FieldProperties() },
                                                                  {FIELDS_EDITOR,           new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME,        new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE,    new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_TYPE,   new FieldProperties() },
                                                                  {FIELDS_ISFLAG,   new FieldProperties() },
                                                                  {FIELDS_PRO_ID,   new FieldProperties() }
                                                                };
    }
}
