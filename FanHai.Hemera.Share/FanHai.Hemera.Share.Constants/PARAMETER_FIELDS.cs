using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    public class PARAMETERS_INPUT : TableFields
    {
        #region columns define

        public const string FIELD_KEY = "KEY";
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        public const string FIELD_EDITOR = "EDITOR";

        #endregion

        #region table name define

        public const string DATABASE_TABLE_NAME = "INPUT_PARAM_TABLE";       //TABLE NAME

        #endregion

        #region get TABLE_NAME

        /// <summary>
        /// override TABLE_NAME
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        #endregion

        #region get FIELDS

        /// <summary>
        /// override fields
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        #endregion

        #region define dictionary of containing column's fields

        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                            {
                                                                {FIELD_KEY, new FieldProperties() },
                                                                {FIELD_EDITOR, new FieldProperties() },
                                                                {FIELD_EDIT_TIME, new FieldProperties() },
                                                              };

        #endregion

        #region create datatable

        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys )
            {
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn);
            }
            return dTable;
        }

        #endregion
    }

    public class PARAMETERS_OUTPUT : TableFields
    {
        #region columns define

        public const string FIELD_CODE = "CODE";
        public const string FIELD_MESSAGE = "MESSAGE";
        public const string FIELD_EDITOR = "EDITOR";
        public const string FIELD_EDIT_TIME = "EDIT_TIME";

        #endregion

        #region table name define

        public const string DATABASE_TABLE_NAME = "OUTPUT_PARAM_TABLE";       //TABLE NAME

        #endregion

        #region get TABLE_NAME

        /// <summary>
        /// override TABLE_NAME
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        #endregion

        #region get FIELDS

        /// <summary>
        /// override fields
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        #endregion

        #region define dictionary of containing column's fields

        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                            {
                                                                {FIELD_CODE, new FieldProperties() },
                                                                {FIELD_MESSAGE, new FieldProperties() },
                                                                {FIELD_EDITOR, new FieldProperties() },
                                                                {FIELD_EDIT_TIME, new FieldProperties() }
                                                            };

        #endregion

        #region create datatable

        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys)
            {
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn);
            }
            return dTable;
        }

        #endregion
    }
}
