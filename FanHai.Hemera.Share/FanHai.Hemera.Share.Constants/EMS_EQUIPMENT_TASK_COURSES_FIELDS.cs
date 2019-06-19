using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENT_TASK_COURSES Fields
    /// </summary>
    /// Owner:Andy Gao 2011-08-17 15:46:17
    public class EMS_EQUIPMENT_TASK_COURSES_FIELDS : TableFields
    {
        public const string FIELD_EQUIPMENT_TASK_COURSE_KEY = "EQUIPMENT_TASK_COURSE_KEY";
        public const string FIELD_EQUIPMENT_TASK_KEY = "EQUIPMENT_TASK_KEY";
        public const string FIELD_EQUIPMENT_TASK_STATE = "EQUIPMENT_TASK_STATE";
        public const string FIELD_SEND_USER_KEY = "SEND_USER_KEY";
        public const string FIELD_SEND_USER_NAME = "SEND_USER_NAME";
        public const string FIELD_SEND_TIMESTAMP = "SEND_TIMESTAMP";
        public const string FIELD_RECEIVE_DEPT_KEY = "RECEIVE_DEPT_KEY";
        public const string FIELD_RECEIVE_DEPT_NAME = "RECEIVE_DEPT_NAME";
        public const string FIELD_RECEIVE_USER_KEY = "RECEIVE_USER_KEY";
        public const string FIELD_RECEIVE_USER_NAME = "RECEIVE_USER_NAME";
        public const string FIELD_RECEIVE_TIMESTAMP = "RECEIVE_TIMESTAMP";
        public const string FIELD_HANDLE_USER_KEY = "HANDLE_USER_KEY";
        public const string FIELD_HANDLE_USER_NAME = "HANDLE_USER_NAME";
        public const string FIELD_HANDLE_TIMESTAMP = "HANDLE_TIMESTAMP";
        public const string FIELD_HANDLE_NOTES = "HANDLE_NOTES";
        public const string FIELD_REMARK = "REMARK";
        public const string FIELD_COMMENTS = "COMMENTS";
        public const string FIELD_EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";
        public const string FIELD_EQUIPMENT_CHANGE_REASON_KEY = "EQUIPMENT_CHANGE_REASON_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENT_TASK_COURSES";

        public EMS_EQUIPMENT_TASK_COURSES_FIELDS()
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
                                                                        { FIELD_EQUIPMENT_TASK_COURSE_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_TASK_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_TASK_STATE, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_SEND_USER_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_SEND_TIMESTAMP, new FieldProperties(typeof(DateTime), false, string.Empty) },
                                                                        { FIELD_RECEIVE_DEPT_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_RECEIVE_USER_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_RECEIVE_TIMESTAMP, new FieldProperties(typeof(DateTime), true, string.Empty) },
                                                                        { FIELD_HANDLE_USER_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_HANDLE_TIMESTAMP, new FieldProperties(typeof(DateTime), true, string.Empty) },
                                                                        { FIELD_HANDLE_NOTES, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_REMARK, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_COMMENTS, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_CHANGE_STATE_KEY, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_CHANGE_REASON_KEY, new FieldProperties(typeof(string), true, string.Empty) }
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
