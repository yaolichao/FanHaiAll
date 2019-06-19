using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_PM_SCHEDULE Fields
    /// </summary>
    /// Owner:Andy Gao 2011-08-03 16:12:14
    public class EMS_PM_SCHEDULE_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_SCHEDULE_KEY = "SCHEDULE_KEY";
        public const string FIELD_SCHEDULE_NAME = "SCHEDULE_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_FREQUENCE = "FREQUENCE";
        public const string FIELD_FREQUENCE_UNIT = "FREQUENCE_UNIT";
        public const string FIELD_NOTIFY_USER_KEY = "NOTIFY_USER_KEY";
        public const string FIELD_NOTIFY_USER_NAME = "NOTIFY_USER_NAME";
        public const string FIELD_NOTIFY_CC_USER_KEY = "NOTIFY_CC_USER_KEY";
        public const string FIELD_NOTIFY_CC_USER_NAME = "NOTIFY_CC_USER_NAME";
        public const string FIELD_NOTIFY_ADVANCED_TIME = "NOTIFY_ADVANCED_TIME";
        public const string FIELD_NEXT_EVENT_TIME = "NEXT_EVENT_TIME";
        public const string FIELD_BASE_ACTUAL_FINISH_TIME = "BASE_ACTUAL_FINISH_TIME";
        public const string FIELD_EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";
        public const string FIELD_EQUIPMENT_CHANGE_REASON_KEY = "EQUIPMENT_CHANGE_REASON_KEY";
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        public const string FIELD_CHECKLIST_KEY = "CHECKLIST_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_PM_SCHEDULE";

        public EMS_PM_SCHEDULE_FIELDS()
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
                                                                        { FIELD_SCHEDULE_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_SCHEDULE_NAME, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { FIELD_FREQUENCE, new FieldProperties(typeof(int), false, 0) },
                                                                        { FIELD_FREQUENCE_UNIT, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_NOTIFY_USER_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_NOTIFY_CC_USER_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_NOTIFY_ADVANCED_TIME, new FieldProperties(typeof(decimal), false, 0) },
                                                                        { FIELD_NEXT_EVENT_TIME, new FieldProperties(typeof(DateTime), false, string.Empty) },
                                                                        { FIELD_BASE_ACTUAL_FINISH_TIME, new FieldProperties(typeof(int), false, 0) },
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
