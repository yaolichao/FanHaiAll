using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENT_CHANGE_REASONS Fields
    /// </summary>
    /// Owner:Andy Gao 2010-07-19 15:55:24
    public class EMS_EQUIPMENT_CHANGE_REASONS_FIELDS : TableFields
    {
        public const string FIELD_EQUIPMENT_CHANGE_REASON_KEY = "EQUIPMENT_CHANGE_REASON_KEY";
        public const string FIELD_EQUIPMENT_CHANGE_REASON_NAME = "EQUIPMENT_CHANGE_REASON_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENT_CHANGE_REASONS";

        public EMS_EQUIPMENT_CHANGE_REASONS_FIELDS()
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
                                                                        { FIELD_EQUIPMENT_CHANGE_REASON_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_EQUIPMENT_CHANGE_REASON_NAME, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_EQUIPMENT_CHANGE_STATE_KEY, new FieldProperties(typeof(string), false, "") }
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
