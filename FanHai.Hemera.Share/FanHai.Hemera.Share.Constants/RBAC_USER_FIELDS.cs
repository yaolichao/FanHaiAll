using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class RBAC_USER_FIELDS:TableFields
    {
       public const string FIELD_USER_KEY="USER_KEY";              
       public const string FIELD_BADGE="BADGE";
       public const string FIELD_USERNAME="USERNAME";
       public const string FIELD_PASSWORD="PASSWORD";
       public const string FIELD_EMAIL="EMAIL";
       public const string FIELD_OFFICE_PHONE="OFFICE_PHONE";
       public const string FIELD_MOBILE_PHONE="MOBILE_PHONE";
       public const string FIELD_IS_LOCKED_OUT="IS_LOCKED_OUT";
       public const string FIELD_IS_ACTIVE="IS_ACTIVE";
       public const string FIELD_LAST_LOGIN_TIME="LAST_LOGIN_TIME";  
       public const string FIELD_LAST_LOGIN_IP="LAST_LOGIN_IP";
       public const string FIELD_EDITOR="EDITOR";
       public const string FIELD_EDIT_TIME="EDIT_TIME";
       public const string FIELD_REMARK="REMARK";
       public const string FIELD_IS_APPROVED="IS_APPROVED";
       public const string FIELD_CREATOR="CREATOR";
       public const string FIELD_CREATE_TIME="CREATE_TIME";
       public const string FIELD_CREATE_TIMEZONE="CREATE_TIMEZONE";
       public const string FIELD_EDIT_TIMEZONE="EDIT_TIMEZONE";
       public const string FIELD_PASSWORD_NEW = "PASSWORD_NEW";
       public const string DATABASE_TABLE_NAME = "RBAC_USER";  
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
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_USER_KEY, new FieldProperties() },
                                                                  {FIELD_BADGE,new FieldProperties(typeof(string), false, "") },
                                                                  {FIELD_USERNAME,new FieldProperties() },
                                                                  {FIELD_PASSWORD,new FieldProperties()},
                                                                  {FIELD_EMAIL,new FieldProperties() },
                                                                  {FIELD_OFFICE_PHONE,new FieldProperties() },
                                                                  {FIELD_MOBILE_PHONE,new FieldProperties() },
                                                                  {FIELD_IS_LOCKED_OUT, new FieldProperties()},
                                                                  {FIELD_IS_ACTIVE,new FieldProperties()},
                                                                  {FIELD_LAST_LOGIN_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_LAST_LOGIN_IP,new FieldProperties()},
                                                                  {FIELD_EDITOR,new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_REMARK,new FieldProperties() },
                                                                  {FIELD_IS_APPROVED,new FieldProperties() },
                                                                  {FIELD_CREATOR,new FieldProperties() },
                                                                  {FIELD_CREATE_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_CREATE_TIMEZONE,new FieldProperties()},
                                                                  {FIELD_EDIT_TIMEZONE,new FieldProperties()}                                                                  
                                                                }; 
                                                                   
    }                                                              
}                                                                   
        