using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class RBAC_RESOURCE_GROUP_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_RESOURCE_GROUP_KEY = "RESOURCE_GROUP_KEY";      //RESOURCE_GROUP_KEY
        public const string FIELD_GROUP_NAME = "GROUP_NAME";         //GROUP_NAME
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";    //DESCRIPTIONS
        public const string FIELD_CREATOR = "CREATOR";     //CREATOR
        public const string FIELD_CREATE_TIME = "CREATE_TIME";     //CREATE_TIME
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";     //CREATE_TIMEZONE
        public const string FIELD_EDITOR = "EDITOR";     //editor
        public const string FIELD_EDIT_TIME = "EDIT_TIME";     //edit time
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";     //EDIT_TIMEZONE
        public const string FIELD_REMARK = "REMARK";     //REMARK
        public const string FIELD_RESOURCE_GROUP_CODE = "RESOURCE_GROUP_CODE";
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "RBAC_RESOURCE_GROUP";       //TABLE NAME
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
                                                                  {FIELD_RESOURCE_GROUP_KEY , new FieldProperties() },   //RESOURCE_GROUP_KEY
                                                                  {FIELD_GROUP_NAME , new FieldProperties() },  //GROUP_NAME
                                                                  {FIELD_DESCRIPTIONS , new FieldProperties() },  //DESCRIPTIONS
                                                                  {FIELD_CREATOR , new FieldProperties() },  //CREATOR
                                                                  {FIELD_CREATE_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now)},  //CREATE_TIME
                                                                  {FIELD_CREATE_TIMEZONE , new FieldProperties() },  //CREATE_TIMEZONE
                                                                  {FIELD_EDITOR , new FieldProperties() }, //editor
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) },  //edit time
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() },  //EDIT_TIMEZONE
                                                                  {FIELD_REMARK , new FieldProperties() },  //REMARK
                                                                  {FIELD_RESOURCE_GROUP_CODE,new FieldProperties()}
                                                                };
        #endregion
    }
}


