using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class FMM_LOCATION_LINE_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_LOCATION_KEY = "LOCATION_KEY";
        public const string FIELD_LINE_KEY = "LINE_KEY";        
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "FMM_LOCATION_LINE";       //TABLE NAME
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
                                                                  {FIELD_LOCATION_KEY , new FieldProperties() },       
                                                                  {FIELD_LINE_KEY , new FieldProperties() } 
                                                               
                                                                };
        #endregion
    }
}