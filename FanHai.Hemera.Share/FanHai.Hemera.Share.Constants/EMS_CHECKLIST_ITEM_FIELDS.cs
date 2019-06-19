using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_CHECKLIST_ITEM Fields
    /// </summary>
    /// Owner:Andy Gao 2011-07-14 10:31:16
    public class EMS_CHECKLIST_ITEM_FIELDS : TableFields
    {
        public const string FIELD_CHECKLIST_KEY = "CHECKLIST_KEY";
        public const string FIELD_CHECKITEM_KEY = "CHECKITEM_KEY";
        public const string FIELD_SEQUENCE = "SEQUENCE";
        public const string FIELD_STANDARD = "STANDARD";
        public const string FIELD_OPTIONAL = "OPTIONAL";
        

        public const string DATABASE_TABLE_NAME = "EMS_CHECKLIST_ITEM";

        public EMS_CHECKLIST_ITEM_FIELDS()
        {

        }

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
                                                                        { FIELD_CHECKLIST_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_CHECKITEM_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_SEQUENCE, new FieldProperties(typeof(int), false, 0) }, 
                                                                        { FIELD_STANDARD, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { FIELD_OPTIONAL, new FieldProperties(typeof(int), false, 1) }
                                                                    };

        public static DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable(DATABASE_TABLE_NAME);

            dataTable.Columns.Clear();

            foreach (KeyValuePair<string, FieldProperties> keyValue in _fields)
            {
                dataTable.Columns.Add(keyValue.Key);
            }

            return dataTable;
        }
    }
}
