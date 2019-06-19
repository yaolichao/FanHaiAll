using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class POR_PART_ATTR_FIELDS : TableFields
    {
        public const string FIELDS_PART_KEY         = "PART_KEY";
        public const string FIELDS_ATTRIBUTE_KEY    = "ATTRIBUTE_KEY";
        public const string FIELDS_ATTRIBUTE_NAME   = "ATTRIBUTE_NAME";
        public const string FIELDS_ATTRIBUTE_VALUE  = "ATTRIBUTE_VALUE";
        public const string FIELDS_EDITOR           = "EDITOR";
        public const string FIELDS_EDIT_TIME        = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE    = "EDIT_TIMEZONE";

        public const string DATABASE_TABLE_NAME = "POR_PART_ATTR";
        
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
                                                                  {FIELDS_ATTRIBUTE_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_ATTRIBUTE_NAME, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_ATTRIBUTE_VALUE, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_PART_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_EDITOR, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties(typeof(string), false, "") } 
                                                                };
    }
}
