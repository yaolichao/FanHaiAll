using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class POR_PART_FIELDS : TableFields
    {
        public const string FIELD_PART_KEY          = "PART_KEY";
        public const string FIELD_PART_ID           = "PART_ID";
        public const string FIELD_PART_VERSION      = "PART_VERSION";
        //public const string FIELD_UNIT              = "UNIT";
        public const string FIELD_PART_DESC         = "PART_DESC";
        public const string FIELD_EFFECTIVITY_START = "EFFECTIVITY_START";
        public const string FIELD_EFFECTIVITY_END   = "EFFECTIVITY_END";
        //public const string FIELD_INSTRUCTIONS_KEY  = "INSTRUCTIONS_KEY";
        public const string FIELD_PART_STATUS       = "PART_STATUS";
        public const string FIELD_CREATOR           = "CREATOR";
        public const string FIELD_CREATE_TIME       = "CREATE_TIME";
        public const string FIELD_CREATE_TIMEZONE   = "CREATE_TIMEZONE";
        public const string FIELD_EDITOR            = "EDITOR";
        public const string FIELD_EDIT_TIME         = "EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE     = "EDIT_TIMEZONE";
        public const string FIELD_PART_NAME         = "PART_NAME";
        public const string FIELD_PART_TYPE         = "PART_TYPE";	    //类型
        public const string FIELD_PART_MODULE       = "PART_MODULE";	//规格
        public const string FIELD_CUR_ENTERPRISE_VER_KEY = "CUR_ENTERPRISE_VER_KEY";	//集团途程版本ID
        public const string FIELD_CUR_ROUTE_VER_KEY      = "CUR_ROUTE_VER_KEY";	        //当前途程版本ID
        public const string FIELD_CUR_STEP_VER_KEY       = "CUR_STEP_VER_KEY";	        //当前工序版本ID

        public const string FIELD_PART_CLASS = "PART_CLASS";	        //产品的级别

        public const string DATABASE_TABLE_NAME = "POR_PART";

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
                                                                  {FIELD_PART_KEY, new FieldProperties(typeof(string), false, "") },
                                                                  {FIELD_PART_ID, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_PART_VERSION, new FieldProperties(typeof(Int32),true,0) },
                                                                  //{FIELD_UNIT, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_PART_DESC, new FieldProperties(typeof(string),true,0) },
                                                                  {FIELD_EFFECTIVITY_START, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_EFFECTIVITY_END, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  //{FIELD_INSTRUCTIONS_KEY, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_PART_STATUS, new FieldProperties(typeof(Int32),true,0) },
                                                                  {FIELD_CREATOR, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_CREATE_TIMEZONE, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_EDITOR, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties(typeof(string),true,"")},
                                                                  {FIELD_PART_NAME, new FieldProperties(typeof(string),true,"")},
                                                                  {FIELD_PART_TYPE,new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_PART_MODULE,new FieldProperties()},
                                                                  {FIELD_CUR_ENTERPRISE_VER_KEY,new FieldProperties()},
                                                                  {FIELD_CUR_ROUTE_VER_KEY,new FieldProperties()},
                                                                  {FIELD_CUR_STEP_VER_KEY,new FieldProperties()},
                                                                  {FIELD_PART_CLASS, new FieldProperties(typeof(string),true,"") }
                                                                };
    }
}
