using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_PRODUCTMODEL_POWER : TableFields
    {
        #region fields
        public const string FIELDS_PROMODEL_POWER_KEY = "PROMODEL_POWER_KEY";
        public const string FIELDS_PROMODEL_KEY = "PROMODEL_KEY";
        public const string FIELDS_PM = "PM";
        public const string FIELDS_ISC = "ISC";
        public const string FIELDS_VOC = "VOC";
        public const string FIELDS_IMP = "IMP";
        public const string FIELDS_VMP = "VMP";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_ISFLAG = "ISFLAG";
        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_PRODUCTMODEL_POWER";
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
                                                                  {FIELDS_PROMODEL_POWER_KEY, new FieldProperties() },
                                                                  {FIELDS_PROMODEL_KEY, new FieldProperties() },
                                                                  {FIELDS_PM, new FieldProperties() },
                                                                  {FIELDS_ISC, new FieldProperties() },
                                                                  {FIELDS_VOC, new FieldProperties() },
                                                                  {FIELDS_IMP, new FieldProperties() },
                                                                  {FIELDS_VMP, new FieldProperties() },                                                                    
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  