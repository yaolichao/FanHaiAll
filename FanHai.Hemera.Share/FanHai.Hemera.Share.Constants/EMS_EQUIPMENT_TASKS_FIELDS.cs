using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENT_TASKS Fields
    /// </summary>
    /// Owner:Andy Gao 2011-08-17 15:28:57
    public class EMS_EQUIPMENT_TASKS_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_EQUIPMENT_TASK_KEY = "EQUIPMENT_TASK_KEY";
        public const string FIELD_EQUIPMENT_TASK_NAME = "EQUIPMENT_TASK_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_EQUIPMENT_TASK_STATE = "EQUIPMENT_TASK_STATE";
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        public const string FIELD_CREATE_USER_KEY = "CREATE_USER_KEY";
        public const string FIELD_CREATE_TIMESTAMP = "CREATE_TIMESTAMP";
        public const string FIELD_START_USER_KEY = "START_USER_KEY";
        public const string FIELD_START_TIMESTAMP = "START_TIMESTAMP";
        public const string FIELD_COMPLETE_USER_KEY = "COMPLETE_USER_KEY";
        public const string FIELD_COMPLETE_TIMESTAMP = "COMPLETE_TIMESTAMP";
        public const string FIELD_EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";
        public const string FIELD_EQUIPMENT_CHANGE_REASON_KEY = "EQUIPMENT_CHANGE_REASON_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENT_TASKS";

        public EMS_EQUIPMENT_TASKS_FIELDS()
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
                                                                        { FIELD_EQUIPMENT_TASK_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_TASK_NAME, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_TASK_STATE, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_EQUIPMENT_KEY, new FieldProperties(typeof(string),  false, string.Empty) },
                                                                        { FIELD_CREATE_USER_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_CREATE_TIMESTAMP, new FieldProperties(typeof(DateTime), false, string.Empty) },
                                                                        { FIELD_START_USER_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_START_TIMESTAMP, new FieldProperties(typeof(DateTime), true, string.Empty) },
                                                                        { FIELD_COMPLETE_USER_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_COMPLETE_TIMESTAMP, new FieldProperties(typeof(DateTime), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_CHANGE_STATE_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_CHANGE_REASON_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_CREATOR, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, string.Empty) },
                                                                        { FIELD_CREATE_TIMEZONE_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EDITOR, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, string.Empty) },
                                                                        { FIELD_EDIT_TIMEZONE_KEY, new FieldProperties(typeof(string), true, string.Empty) }
                                                                    };

        public static DataTable CreateDataTable(bool isPaging)
        {
            DataTable dataTable = new DataTable(DATABASE_TABLE_NAME);

            dataTable.Columns.Clear();

            if (isPaging)
            {
                dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);
            }

            foreach (KeyValuePair<string, FieldProperties> keyValue in _fields)
            {
                dataTable.Columns.Add(keyValue.Key);
            }

            return dataTable;
        }
    }
}
