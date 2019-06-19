using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_CHECKITEMS Fields
    /// </summary>
    /// Owner:Andy Gao 2011-07-08 10:18:33
    public class EMS_CHECKITEMS_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_CHECKITEM_KEY = "CHECKITEM_KEY";
        public const string FIELD_CHECKITEM_NAME = "CHECKITEM_NAME";
        public const string FIELD_CHECKITEM_TYPE = "CHECKITEM_TYPE";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";

        public const string DATABASE_TABLE_NAME = "EMS_CHECKITEMS";

        public EMS_CHECKITEMS_FIELDS()
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
                                                                        { FIELD_CHECKITEM_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_CHECKITEM_NAME, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_CHECKITEM_TYPE, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, string.Empty) },
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
