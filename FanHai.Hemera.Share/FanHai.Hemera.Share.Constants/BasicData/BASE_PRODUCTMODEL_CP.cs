using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_PRODUCTMODEL_CP : TableFields
    {
        #region fields
        public const string FIELDS_PROMODEL_DTL_KEY = "PROMODEL_DTL_KEY";
        public const string FIELDS_PROMODEL_KEY = "PROMODEL_KEY";
        public const string FIELDS_CONTROL_OBJ = "CONTROL_OBJ";
        public const string FIELDS_CONTROL_TYPE = "CONTROL_TYPE";
        public const string FIELDS_CONTROL_VALUE = "CONTROL_VALUE";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_ISFLAG = "ISFLAG";
        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_PRODUCTMODEL_CP";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_PRODUCTMODEL_CP_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_PRODUCTMODEL_CP_INSERT";
        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_PROMODEL_DTL_KEY, new FieldProperties() },
                                                                  {FIELDS_PROMODEL_KEY, new FieldProperties() },
                                                                  {FIELDS_CONTROL_OBJ, new FieldProperties() },
                                                                  {FIELDS_CONTROL_TYPE, new FieldProperties() },
                                                                  {FIELDS_CONTROL_VALUE, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(int),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  