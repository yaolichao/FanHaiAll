using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class EDC_MAIN_INS_PARAMS_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_EDC_KEY = "EDC_KEY ";
        public const string FIELD_PARAM_KEY = "PARAM_KEY";
        public const string FIELD_PARAM_SEQ = "PARAM_SEQ";
        public const string FIELD_SEQ_KEY = "SEQ_KEY";
        public const string FIELD_PARAM_NAME = "PARAM_NAME";
        public const string FIELD_SP_SAMP_SEQ = "SP_SAMP_SEQ";//参数个数
        public const string FIELD_EDC_INS_KEY = "EDC_INS_KEY";
        public const string FIELD_PARAM_TYPE = "PARAM_TYPE";
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "EDC_MAIN_INS_PARAMS";       //TABLE NAME
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
                                                                  {FIELD_EDC_KEY , new FieldProperties() },
                                                                  {FIELD_PARAM_KEY , new FieldProperties() },
                                                                  {FIELD_PARAM_SEQ , new FieldProperties() },
                                                                  {FIELD_SEQ_KEY , new FieldProperties() },   
                                                                  {FIELD_PARAM_NAME , new FieldProperties() },
                                                                  {FIELD_SP_SAMP_SEQ, new FieldProperties() },
                                                                  {FIELD_EDC_INS_KEY, new FieldProperties() },
                                                                  {FIELD_PARAM_TYPE,new FieldProperties()}
                                                              };
        #endregion
    }
}
