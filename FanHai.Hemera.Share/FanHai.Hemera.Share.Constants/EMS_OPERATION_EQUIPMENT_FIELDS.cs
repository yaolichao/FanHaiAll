using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_OPERATION_EQUIPMENT Fields
    /// </summary>
    /// Owner:Andy Gao 2010-08-11 14:21:27
    public class EMS_OPERATION_EQUIPMENT_FIELDS : TableFields
    {
        public const string FIELD_OPERATION_EQUIPMENT_KEY = "OPERATION_EQUIPMENT_KEY";
        public const string FIELD_OPERATION_KEY = "OPERATION_KEY";
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_OPERATION_EQUIPMENT";

        public EMS_OPERATION_EQUIPMENT_FIELDS()
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
                                                                        { FIELD_OPERATION_EQUIPMENT_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_OPERATION_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_EQUIPMENT_KEY, new FieldProperties(typeof(string), false, "") }
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
