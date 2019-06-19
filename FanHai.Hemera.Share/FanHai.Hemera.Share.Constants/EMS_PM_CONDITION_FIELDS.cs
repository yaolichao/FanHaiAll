using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_PM_CONDITION Fields
    /// </summary>
    /// Owner:Andy Gao 2011-08-08 15:16:08
    public class EMS_PM_CONDITION_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_CONDITION_KEY = "CONDITION_KEY";
        public const string FIELD_CONDITION_NAME = "CONDITION_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_COUNTER_TYPE = "COUNTER_TYPE";
        public const string FIELD_COUNTER_WARNNING = "COUNTER_WARNNING";
        public const string FIELD_COUNTER_TARGET = "COUNTER_TARGET";
        public const string FIELD_COUNTER_TOTAL = "COUNTER_TOTAL";
        public const string FIELD_NOTIFY_USER_KEY = "NOTIFY_USER_KEY";
        public const string FIELD_NOTIFY_USER_NAME = "NOTIFY_USER_NAME";
        public const string FIELD_NOTIFY_CC_USER_KEY = "NOTIFY_CC_USER_KEY";
        public const string FIELD_NOTIFY_CC_USER_NAME = "NOTIFY_CC_USER_NAME";
        public const string FIELD_EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";
        public const string FIELD_EQUIPMENT_CHANGE_REASON_KEY = "EQUIPMENT_CHANGE_REASON_KEY";
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        public const string FIELD_CHECKLIST_KEY = "CHECKLIST_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_PM_CONDITION";

        public EMS_PM_CONDITION_FIELDS()
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
                                                                        { FIELD_CONDITION_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_CONDITION_NAME, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_COUNTER_TYPE, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_COUNTER_WARNNING, new FieldProperties(typeof(int), false, 0) },
                                                                        { FIELD_COUNTER_TARGET, new FieldProperties(typeof(int), false, 0) },
                                                                        { FIELD_COUNTER_TOTAL, new FieldProperties(typeof(int), false, 0) },
                                                                        { FIELD_NOTIFY_USER_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_NOTIFY_CC_USER_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_CHANGE_STATE_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_CHANGE_REASON_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_CHECKLIST_KEY, new FieldProperties(typeof(string), false, string.Empty) },
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
