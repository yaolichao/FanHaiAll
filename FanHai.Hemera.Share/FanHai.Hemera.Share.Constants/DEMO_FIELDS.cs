using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SolarViewer.Hemera.Share.Constants
{
    public class DEMO_FIELDS : TableFields
    {
        public const string FIELD_PRODUCTION_LINE_KEY = "PRODUCTION_LINE_KEY";
        public const string FIELD_LINE_NAME = "LINE_NAME";
        public const string FIELD_LINE_CODE = "LINE_CODE";
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";
        public const string FIELD_CREATOR = "CREATOR";
        public const string FIELD_CREATE_TIME = "CREATE_TIME";
        public const string FIELD_CREATE_TIMEZONE_KEY = "CREATE_TIMEZONE_KEY";
        public const string FIELD_EDITOR = "EDITOR";
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE_KEY = "EDIT_TIMEZONE_KEY";
        public const string DATABASE_TABLE_NAME = "FMM_PRODUCTION_LINE";


        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "EMS_EQUIPMENT";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_PRODUCTION_LINE_KEY, new FieldProperties(typeof(string), false,"") },
                                                                  {FIELD_LINE_NAME, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_LINE_CODE, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties(typeof(string),true,"") },
                                                                  {FIELD_CREATOR, new FieldProperties(typeof(string),false,"") },
                                                                  //{FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime),false,0) },
                                                                  //{FIELD_CREATE_TIMEZONE_KEY, new FieldProperties(typeof(string),false,"") },
                                                                  {FIELD_EDITOR, new FieldProperties(typeof(string),false,"") },
                                                                  //{FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),false,0) },
                                                                  //{FIELD_EDIT_TIMEZONE_KEY, new FieldProperties(typeof(string),false,0) },                                       
                                                                };

        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(DEMO_FIELDS.DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys)
            {
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn); 
            }
            return dTable;
        }

    }
}
