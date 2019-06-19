using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class BASE_CLASSFILE_FIELDS:TableFields
    {

        public const string FIELDS_CODE_KEY = "CODE_KEY";
        public const string FIELDS_CODE_ID = "CODE_ID";
        public const string FIELDS_CODE_TYPE = "CODE_TYPE";
        public const string FIELDS_DESCRIPTION = "DESCRIPTION";
        public const string FIELDS_VERSION = "VERSION";
        public const string FIELDS_STATE = "STATE";
        public const string FIELDS_BATTERY_CLASS = "BATTERY_CLASS";
        public const string FIELDS_BATTERY_BIN = "BATTERY_BIN";
        public const string FIELDS_LOWER = "LOWER";
        public const string FIELDS_UPPER = "UPPER";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_CODE_CODE  = "CODE_CODE";
        public const string FIELDS_CODE_TYPE2 = "CODE_TYPE2";
        public const string FIELDS_LOWER2     = "LOWER2";
        public const string FIELDS_UPPER2     = "UPPER2";
        public const string FIELDS_CODE_TYPE3 = "CODE_TYPE3";
        public const string FIELDS_LOWER3     = "LOWER3";
        public const string FIELDS_UPPER3     = "UPPER3";
        public const string FIELDS_CODE_TYPE4 = "CODE_TYPE4";
        public const string FIELDS_LOWER4     = "LOWER4";
        public const string FIELDS_UPPER4     = "UPPER4";
        public const string COMPUTER_NAME     ="COMPUTER_NAME";
        public const string DATABASE_TABLE_NAME = "BASE_CLASSFILE";

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
                                                                  {FIELDS_CODE_KEY, new FieldProperties() },
                                                                  {FIELDS_CODE_ID, new FieldProperties() },
                                                                  {FIELDS_BATTERY_CLASS, new FieldProperties() },
                                                                  {FIELDS_CODE_CODE, new FieldProperties()  },
                                                                  {FIELDS_BATTERY_BIN, new FieldProperties() },
                                                                  {FIELDS_CODE_TYPE, new FieldProperties() },
                                                                  {FIELDS_LOWER, new FieldProperties() },
                                                                  {FIELDS_UPPER, new FieldProperties() },
                                                                  {FIELDS_DESCRIPTION, new FieldProperties() },
                                                                  {FIELDS_VERSION, new FieldProperties() },
                                                                  {FIELDS_STATE, new FieldProperties() }, 
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {COMPUTER_NAME, new FieldProperties()    },
                                                                  {FIELDS_CODE_TYPE2, new FieldProperties() },
                                                                  {FIELDS_LOWER2, new FieldProperties()     },
                                                                  {FIELDS_UPPER2 , new FieldProperties()    },
                                                                  {FIELDS_CODE_TYPE3, new FieldProperties() },
                                                                  {FIELDS_LOWER3, new FieldProperties()     },
                                                                  {FIELDS_UPPER3 , new FieldProperties()    },
                                                                  {FIELDS_CODE_TYPE4, new FieldProperties() },
                                                                  {FIELDS_LOWER4, new FieldProperties()     },
                                                                  {FIELDS_UPPER4 , new FieldProperties()    }                                                                 
                                                                };  
        
    }   
}       
        