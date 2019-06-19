#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace FanHai.Hemera.Share.Constants
{
    public class BASE_PARAMETER_FIELDS : TableFields
    {

        #region columns define
        public const string FIELD_PARAM_KEY = "PARAM_KEY";
        public const string FIELD_PARAM_NAME = "PARAM_NAME";
        public const string FIELD_PARAM_CATEGORY = "PARAM_CATEGORY";
        public const string FIELD_DATA_TYPE = "DATA_TYPE";
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";
        public const string FIELD_DEFAULT_VALUE = "DEFAULT_VALUE";
        public const string FIELD_DEFAULT_UOM = "DEFAULT_UOM";
        public const string FIELD_UPPER_BOUNDARY = "UPPER_BOUNDARY";
        public const string FIELD_UPPER_SPEC = "UPPER_SPEC";
        public const string FIELD_UPPER_CONTROL = "UPPER_CONTROL";
        public const string FIELD_TARGET = "TARGET";
        public const string FIELD_LOWER_CONTROL = "LOWER_CONTROL";
        public const string FIELD_LOWER_SPEC = "LOWER_SPEC";
        public const string FIELD_LOWER_BOUNDARY = "LOWER_BOUNDARY";
        public const string FIELD_MANDATORY = "MANDATORY";
        public const string FIELD_SITE_NUMBER = "SITE_NUMBER";
        public const string FIELD_STATUS = "STATUS";
        public const string FIELD_ISDERIVED = "ISDERIVED";
        public const string FIELD_DERIVATION_KEY = "DERIVATION_KEY";
        public const string FIELD_CALCULATE_TYPE = "CALCULATE_TYPE";
        public const string FIELD_CALCULATE_FORMULA = "CALCULATE_FORMULA";
        public const string FIELD_CREATE_TIME = "CREATE_TIME";
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        public const string FIELD_DEVICE_TYPE = "DEVICE_TYPE";

        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "BASE_PARAMETER";       //TABLE NAME

        public const string DATABASE_TABLE_FORUPDATE = "BASE_PARAMETER_UPDATE";       //TABLE NAME
        public const string DATABASE_TABLE_FORINSERT = "BASE_PARAMETER_INSERT";       //TABLE NAME
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
                                                                    {FIELD_PARAM_KEY, new FieldProperties() },
                                                                    {FIELD_PARAM_NAME, new FieldProperties() },
                                                                    {FIELD_PARAM_CATEGORY, new FieldProperties() },
                                                                    {FIELD_DATA_TYPE, new FieldProperties() },
                                                                    {FIELD_DESCRIPTIONS, new FieldProperties() },
                                                                    {FIELD_DEFAULT_VALUE, new FieldProperties() },
                                                                    {FIELD_DEFAULT_UOM, new FieldProperties() },
                                                                    {FIELD_UPPER_BOUNDARY, new FieldProperties() },
                                                                    {FIELD_UPPER_SPEC, new FieldProperties() },
                                                                    {FIELD_UPPER_CONTROL, new FieldProperties() },
                                                                    {FIELD_TARGET, new FieldProperties() },
                                                                    {FIELD_LOWER_CONTROL, new FieldProperties() },
                                                                    {FIELD_LOWER_SPEC, new FieldProperties() },
                                                                    {FIELD_LOWER_BOUNDARY, new FieldProperties() },
                                                                    {FIELD_MANDATORY, new FieldProperties() },
                                                                    {FIELD_SITE_NUMBER, new FieldProperties() },
                                                                    {FIELD_STATUS, new FieldProperties() },
                                                                    {FIELD_ISDERIVED, new FieldProperties() }, 
                                                                    {FIELD_DERIVATION_KEY, new FieldProperties() },
                                                                    {FIELD_CALCULATE_TYPE, new FieldProperties() }, 
                                                                    {FIELD_CALCULATE_FORMULA, new FieldProperties() },
                                                                    {FIELD_DEVICE_TYPE, new FieldProperties() },
                                                                    {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)},   
                                                                    {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)} 
                                                              };
        #endregion

    }
}
