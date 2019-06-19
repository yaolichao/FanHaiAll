#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace FanHai.Hemera.Share.Constants
{
    public class BASE_PARAMETER_DERIVTION_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_ROW_KEY = "ROW_KEY";
        public const string FIELD_DERIVATION_KEY = "DERIVATION_KEY";
        public const string FIELD_PARAM_KEY = "PARAM_KEY";
        public const string FIELD_PARAM_NAME = "PARAM_NAME";
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "BASE_PARAMETER_DERIVTION";       //TABLE NAME
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
                                                                  {FIELD_ROW_KEY, new FieldProperties() },       
                                                                  {FIELD_DERIVATION_KEY, new FieldProperties() },
                                                                  {FIELD_PARAM_KEY, new FieldProperties() },     
                                                                  {FIELD_PARAM_NAME, new FieldProperties() },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)} 
                                                              };
        #endregion
    }
}
