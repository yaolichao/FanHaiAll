
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{

    public class COMPUTER_FIELDS : TableFields
    {
        public const string FIELDS_CODE_KEY         = "ROW_KEY";
        public const string FIELDS_COMPUTER_NAME    = "COMPUTER_NAME";
        public const string FIELDS_DESCRIPTION      = "DESCRIPTION"; //add by qym 20120612 10:48
        public const string FIELDS_EDITOR           = "EDITOR";
        public const string FIELDS_EDIT_TIME        = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE    = "EDIT_TIMEZONE";


        public const string DATABASE_TABLE_NAME = "COMPUTER_CONFIG";

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
                                                                {FIELDS_CODE_KEY, new FieldProperties() },
                                                                {FIELDS_COMPUTER_NAME , new FieldProperties() },
                                                                {FIELDS_EDITOR,           new FieldProperties() },
                                                                {FIELDS_EDIT_TIME,        new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELDS_EDIT_TIMEZONE,    new FieldProperties() } 
                                                              };
    }


    public class COMPUTER_ATTR_FIELDS : TableFields
    {
        public const string FIELDS_COMPUTER_KEY = "COMPUTER_KEY";
        //public const string FIELDS_OBJECT_TYPE = "OBJECT_TYPE";
        public const string FIELDS_ATTRIBUTE_KEY = "ATTRIBUTE_KEY";
        public const string FIELDS_ATTRIBUTE_NAME = "ATTRIBUTE_NAME";
        public const string FIELDS_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";

        public const string DATABASE_TABLE_NAME = "COMPUTER_CONFIG_ATTR";

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
                                                                  {FIELDS_COMPUTER_KEY,   new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_KEY,    new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_NAME,   new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_VALUE,  new FieldProperties() },
                                                                  {FIELDS_EDITOR,           new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME,        new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE,    new FieldProperties() } 
                                                                };
    }


    public class BASE_OBJECT_ATTR_FIELDS : TableFields
    {
        public const string FIELDS_OBJECT_KEY       = "OBJECT_KEY";
        public const string FIELDS_OBJECT_TYPE      = "OBJECT_TYPE";
        public const string FIELDS_ATTRIBUTE_KEY    = "ATTRIBUTE_KEY";
        public const string FIELDS_ATTRIBUTE_NAME   = "ATTRIBUTE_NAME";
        public const string FIELDS_ATTRIBUTE_VALUE  = "ATTRIBUTE_VALUE";
        public const string FIELDS_EDITOR           = "EDITOR";
        public const string FIELDS_EDIT_TIME        = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE    = "EDIT_TIMEZONE";

        public const string DATABASE_TABLE_NAME     = "BASE_ATTRIBUTE_VALUE";
        
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
                                                                  {FIELDS_OBJECT_KEY,   new FieldProperties() },
                                                                  {FIELDS_OBJECT_TYPE,   new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_KEY,    new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_NAME,   new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_VALUE,  new FieldProperties() },
                                                                  {FIELDS_EDITOR,           new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME,        new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE,    new FieldProperties() } 
                                                                };
    }
}

