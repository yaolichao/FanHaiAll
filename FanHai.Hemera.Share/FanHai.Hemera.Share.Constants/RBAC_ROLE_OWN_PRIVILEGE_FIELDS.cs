using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class RBAC_ROLE_OWN_PRIVILEGE_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_ROLE_KEY = "ROLE_KEY";      //ROLE_KEY
        public const string FIELD_PRIVILEGE_KEY = "PRIVILEGE_KEY";         //PRIVILEGE_KEY
        public const string FIELD_EDITOR = "EDITOR";     //editor
        public const string FIELD_EDIT_TIME = "EDIT_TIME";     //edit time
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";     //EDIT_TIMEZONE
      
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "RBAC_ROLE_OWN_PRIVILEGE";       //TABLE NAME
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
                                                                  {FIELD_ROLE_KEY , new FieldProperties() },   //RESOURCE_GROUP_KEY
                                                                  {FIELD_PRIVILEGE_KEY , new FieldProperties() },  //GROUP_NAME
                                                                  {FIELD_EDITOR , new FieldProperties() }, //editor
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) },  //edit time
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() } //EDIT_TIMEZONE
                                                                  
                                                                };
        #endregion
    }
}



