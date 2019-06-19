using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_CHECKLIST_JOB_DATA Fields
    /// </summary>
    /// Owner:Andy Gao 2011-07-19 14:00:15
    public class EMS_CHECKLIST_JOB_DATA_FIELDS : TableFields
    {
        public const string FIELD_CHECKLIST_JOB_KEY = "CHECKLIST_JOB_KEY";
        public const string FIELD_CHECKITEM_KEY = "CHECKITEM_KEY";
        public const string FIELD_CHECKITEM_VALUE = "CHECKITEM_VALUE";
        public const string FIELD_COMPLETE_TIMESTAMP = "COMPLETE_TIMESTAMP";
        public const string FIELD_REMARK = "REMARK";


        public const string DATABASE_TABLE_NAME = "EMS_CHECKLIST_JOB_DATA";

        public EMS_CHECKLIST_JOB_DATA_FIELDS()
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
                                                                        { FIELD_CHECKLIST_JOB_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_CHECKITEM_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_CHECKITEM_VALUE, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { FIELD_COMPLETE_TIMESTAMP, new FieldProperties(typeof(DateTime), true, string.Empty) }, 
                                                                        { FIELD_REMARK, new FieldProperties(typeof(string), true, string.Empty) }
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
