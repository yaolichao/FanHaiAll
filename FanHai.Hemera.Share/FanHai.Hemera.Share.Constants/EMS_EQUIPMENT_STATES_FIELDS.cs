using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENT_STATES Fields
    /// </summary>
    /// Owner:Andy Gao 2010-07-09 11:23:21
    public class EMS_EQUIPMENT_STATES_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_EQUIPMENT_STATE_KEY = "EQUIPMENT_STATE_KEY";
        public const string FIELD_EQUIPMENT_STATE_NAME = "EQUIPMENT_STATE_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_EQUIPMENT_STATE_TYPE = "EQUIPMENT_STATE_TYPE";
        public const string FIELD_EQUIPMENT_STATE_CATEGORY = "EQUIPMENT_STATE_CATEGORY";

        public const string FIELD_EQUIPMENT_STATE_ISENABLED = "EQUIPMENT_STATE_ISENABLED";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENT_STATES";

        public EMS_EQUIPMENT_STATES_FIELDS()
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
                                                                        { FIELD_EQUIPMENT_STATE_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_EQUIPMENT_STATE_NAME, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_EQUIPMENT_STATE_TYPE, new FieldProperties(typeof(string), false, "") },
                                                                        { FIELD_EQUIPMENT_STATE_CATEGORY, new FieldProperties(typeof(string), false, "") },
                                                                        { FIELD_CREATOR, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_CREATE_TIMEZONE_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EDITOR, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_EDIT_TIMEZONE_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        {FIELD_EQUIPMENT_STATE_ISENABLED,new FieldProperties(typeof(string),true,"1")}
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
