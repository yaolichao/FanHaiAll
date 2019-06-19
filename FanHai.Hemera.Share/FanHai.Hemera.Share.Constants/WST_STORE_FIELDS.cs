using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class WST_STORE_FIELDS:TableFields
    {
        #region columns define
        public const string FIELD_STORE_KEY          ="STORE_KEY";
        public const string FIELD_STORE_NAME         ="STORE_NAME";
        public const string FIELD_STORE_TYPE         ="STORE_TYPE";
        public const string FIELD_OBJECT_STATUS      ="OBJECT_STATUS";
        public const string FIELD_LOCATION_KEY       ="LOCATION_KEY";
        public const string FIELD_CREATOR            ="CREATOR";
        public const string FIELD_CREATE_TIME        ="CREATE_TIME";
        public const string FIELD_CREATE_TIMEZONE    ="CREATE_TIMEZONE";
        public const string FIELD_EDITOR             = "EDITOR";
        public const string FIELD_EDIT_TIME          = "EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE      = "EDIT_TIMEZONE";
        public const string FIELD_DESCRIPTION        = "DESCRIPTION";
        public const string FIELD_TYPE_NAME = "TYPE_NAME";
        public const string FIELD_REQUEST_FLAG = "REQUEST_FLAG";
        public const string FIELD_OPERATION_NAME = "OPERATION_NAME";
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "WST_STORE";      //TABLE NAME
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
                                                                 {FIELD_STORE_KEY ,new FieldProperties()},      
                                                                 {FIELD_STORE_NAME,new FieldProperties()},     
                                                                 {FIELD_STORE_TYPE,new FieldProperties()},     
                                                                 {FIELD_OBJECT_STATUS,new FieldProperties()},  
                                                                 {FIELD_LOCATION_KEY,new FieldProperties()},   
                                                                 {FIELD_CREATOR,new FieldProperties()},        
                                                                 {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)},    
                                                                 {FIELD_CREATE_TIMEZONE,new FieldProperties()},
                                                                 {FIELD_EDITOR,new FieldProperties()},         
                                                                 {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },     
                                                                 {FIELD_EDIT_TIMEZONE,new FieldProperties()},
                                                                 {FIELD_DESCRIPTION,new FieldProperties()},
                                                                 {FIELD_TYPE_NAME,new FieldProperties()},
                                                                 {FIELD_REQUEST_FLAG,new FieldProperties()},
                                                                 {FIELD_OPERATION_NAME,new FieldProperties()}                                                                 
                                                              };
        #endregion
    }
}
