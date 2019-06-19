using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENT_PARTS Fields
    /// </summary>
    /// Owner:Andy Gao 2011-08-12 09:24:31
    public class EMS_EQUIPMENT_PARTS_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_EQUIPMENT_PART_KEY = "EQUIPMENT_PART_KEY";
        public const string FIELD_EQUIPMENT_PART_NAME = "EQUIPMENT_PART_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_EQUIPMENT_PART_TYPE = "EQUIPMENT_PART_TYPE";
        public const string FIELD_EQUIPMENT_PART_MODE = "EQUIPMENT_PART_MODE";
        public const string FIELD_EQUIPMENT_PART_UNIT = "EQUIPMENT_PART_UNIT";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENT_PARTS";

        public EMS_EQUIPMENT_PARTS_FIELDS()
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
                                                                        { FIELD_EQUIPMENT_PART_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_PART_NAME, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_EQUIPMENT_PART_TYPE, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_PART_MODE, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { FIELD_EQUIPMENT_PART_UNIT, new FieldProperties(typeof(string), false, string.Empty) }, 
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
