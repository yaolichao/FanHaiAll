using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_DECAYCOEFFI : TableFields
    {
        #region
        public const string FIELDS_DECOEFFI_KEY = "DECOEFFI_KEY";
        public const string FIELDS_D_NAME = "D_NAME";
        public const string FIELDS_D_CODE = "D_CODE";
        public const string FIELDS_D_CODE_DESC = "D_CODE_DESC";
        public const string FIELDS_COEFFICIENT = "COEFFICIENT";
        public const string FIELDS_COEFFICIENT_DESC = "COEFFICIENT_DESC";
        public const string FIELDS_DIT = "DIT";
        public const string FIELDS_DECOEFFI_TYPE = "DECOEFFI_TYPE";
        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_ISFLAG = "ISFLAG";
        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_DECAYCOEFFI";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_DECAYCOEFFI_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_DECAYCOEFFI_INSERT";

        public const string SaveFlag = "saveflag";
        public const string InsertFlag = "insert";
        public const string UpdateFlag = "update";
        #endregion

        #region
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

        #region
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_DECOEFFI_KEY, new FieldProperties() },
                                                                  {FIELDS_D_NAME, new FieldProperties() },
                                                                  {FIELDS_D_CODE, new FieldProperties() },
                                                                  {FIELDS_D_CODE_DESC, new FieldProperties() },
                                                                  {FIELDS_COEFFICIENT, new FieldProperties() },                                                                
                                                                  {FIELDS_COEFFICIENT_DESC, new FieldProperties() },                                                                                                  
                                                                  {FIELDS_DIT, new FieldProperties() },
                                                                  {FIELDS_DECOEFFI_TYPE, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_ISFLAG,new FieldProperties(typeof(int),true,1)}
                                                                };
        #endregion
    }
}

  