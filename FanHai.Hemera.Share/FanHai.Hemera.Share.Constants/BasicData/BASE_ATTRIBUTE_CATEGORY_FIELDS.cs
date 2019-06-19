using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_ATTRIBUTE_CATEGORY_FIELDS : TableFields
    {
        public const string FIELDS_CATEGORY_KEY = "CATEGORY_KEY";
        public const string FIELDS_CATEGORY_NAME = "CATEGORY_NAME";
        public const string FIELDS_DESCRIPTIONS = "DESCRIPTION";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string FIELDS_CREATOR = "CREATOR";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_CREATE_TIMEZONE = "CREATE_TIMEZONE";


        public const string DATABASE_TABLE_NAME = "BASE_ATTRIBUTE_CATEGORY";

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
                                                                  {FIELDS_CATEGORY_KEY, new FieldProperties() },
                                                                  {FIELDS_CATEGORY_NAME, new FieldProperties() },
                                                                  {FIELDS_DESCRIPTIONS, new FieldProperties() },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties() },
                                                                  {FIELDS_CREATOR, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_CREATE_TIMEZONE, new FieldProperties() }
                                                                };
    }
}

  