using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENT_GROUPS Fields
    /// </summary>
    /// Owner:Andy Gao 2010-08-02 13:56:57
    public class EMS_EQUIPMENT_GROUPS_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_EQUIPMENT_GROUP_KEY = "EQUIPMENT_GROUP_KEY";
        public const string FIELD_EQUIPMENT_GROUP_NAME = "EQUIPMENT_GROUP_NAME";
        public const string FIELD_SPEC = "SPEC";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENT_GROUPS";

        public EMS_EQUIPMENT_GROUPS_FIELDS()
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
                                                                        { FIELD_EQUIPMENT_GROUP_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_EQUIPMENT_GROUP_NAME, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_SPEC, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, string.Empty) },
                                                                        { FIELD_CREATOR, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_CREATE_TIMEZONE_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EDITOR, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_EDIT_TIMEZONE_KEY, new FieldProperties(typeof(string), true, "") }
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
