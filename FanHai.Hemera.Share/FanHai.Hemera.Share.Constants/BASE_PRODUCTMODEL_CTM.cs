using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class BASE_PRODUCTMODEL_CTM : TableFields
    {
        #region fields
        public const string FIELDS_PROMODEL_KEY = "PROMODEL_KEY";
        public const string FIELDS_CTM_KEY  = "CTM_KEY";
        public const string FIELDS_EFF_UP = "EFF_UP";
        public const string FIELDS_EFF_LOW = "EFF_LOW";
        public const string FIELDS_CTM_UP = "CTM_UP";
        public const string FIELDS_CTM_LOW = "CTM_LOW";
        public const string FIELDS_ISFLAG = "ISFLAG";

        public const string DATABASE_TABLE_NAME = "BASE_PRODUCTMODEL_CTM";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_PRODUCTMODEL_POWER_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_PRODUCTMODEL_POWER_INSERT";
        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "BASE_PRODUCTMODEL_CTM";
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
                                                                  {FIELDS_PROMODEL_KEY, new FieldProperties() },
                                                                  {FIELDS_CTM_KEY, new FieldProperties() },
                                                                  {FIELDS_EFF_UP, new FieldProperties() },
                                                                  {FIELDS_EFF_LOW, new FieldProperties() },
                                                                  {FIELDS_CTM_UP, new FieldProperties() },
                                                                  {FIELDS_CTM_LOW, new FieldProperties() }                                                           
                                                                };
        #endregion
    }
}
