using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQP_TASK_COURSE_PARTS Fields
    /// </summary>
    /// Owner:Andy Gao 2011-08-25 08:57:04
    public class EMS_EQP_TASK_COURSE_PARTS_FIELDS : TableFields
    {
        public const string FIELD_EQP_TASK_COURSE_PART_KEY = "EQP_TASK_COURSE_PART_KEY";
        public const string FIELD_EQP_TASK_COURSE_KEY = "EQP_TASK_COURSE_KEY";
        public const string FIELD_EQP_PART_KEY = "EQP_PART_KEY";
        public const string FIELD_QUANTITY = "QUANTITY";
        public const string FIELD_REMARK = "REMARK";

        public const string DATABASE_TABLE_NAME = "EMS_EQP_TASK_COURSE_PARTS";

        public EMS_EQP_TASK_COURSE_PARTS_FIELDS()
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
                                                                        { FIELD_EQP_TASK_COURSE_PART_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_EQP_TASK_COURSE_KEY, new FieldProperties(typeof(string), false, string.Empty) },
                                                                        { FIELD_EQP_PART_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_QUANTITY, new FieldProperties(typeof(decimal), false, string.Empty) }, 
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
