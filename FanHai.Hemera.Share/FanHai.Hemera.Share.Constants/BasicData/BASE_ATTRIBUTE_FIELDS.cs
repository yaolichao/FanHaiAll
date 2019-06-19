using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_ATTRIBUTE_FIELDS : TableFields
    {
        public const string FIELDS_ATTRIBUTE_KEY    = "ATTRIBUTE_KEY";
        public const string FIELDS_ATTRIBUTE_NAME   = "ATTRIBUTE_NAME";
        public const string FIELDS_DESCRIPTIONS      = "DESCRIPTION";
        public const string FIELDS_DATA_TYPE        = "DATA_TYPE";
        public const string FIELDS_DEFAULT_VALUE    = "DEFAULT_VALUE";
        public const string FIELDS_ATTRIBUTE_UNIT   = "ATTRIBUTE_UNIT";
        public const string FIELDS_VALIDATION_TYPE  = "VALIDATION_TYPE";
        public const string FIELDS_ATTRIBUTE_ORDER  = "ATTRIBUTE_ORDER";
        public const string FIELDS_IS_PRIMARY_KEY   = "IS_PRIMARY_KEY";
        public const string FIELDS_CATEGORY_KEY = "CATEGORY_KEY";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string FIELDS_CREATOR = "CREATOR";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_CREATE_TIMEZONE = "CREATE_TIMEZONE";
        

        public const string DATABASE_TABLE_NAME = "BASE_ATTRIBUTE";

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
                                                                  {FIELDS_ATTRIBUTE_KEY, new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_NAME, new FieldProperties() },
                                                                  {FIELDS_DESCRIPTIONS, new FieldProperties() },
                                                                  {FIELDS_DATA_TYPE, new FieldProperties() },
                                                                  {FIELDS_DEFAULT_VALUE, new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_UNIT, new FieldProperties() },
                                                                  {FIELDS_VALIDATION_TYPE, new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_ORDER, new FieldProperties() },
                                                                  {FIELDS_IS_PRIMARY_KEY, new FieldProperties() },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties() },
                                                                  {FIELDS_CREATOR, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_CREATE_TIMEZONE, new FieldProperties() },
                                                                  {FIELDS_CATEGORY_KEY, new FieldProperties() }
                                                                };
    }
}

  