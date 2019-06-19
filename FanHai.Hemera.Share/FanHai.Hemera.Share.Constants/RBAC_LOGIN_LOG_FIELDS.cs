using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    public class RBAC_LOGIN_LOG_FIELDS : TableFields
    {
        public const string FIELD_LOGIN_LOG_KEY = "LOGIN_LOG_KEY";
        public const string FIELD_USER_KEY = "USER_KEY";
        public const string FIELD_SITE = "SITE";
        public const string FIELD_LANGUAGE = "LANGUAGE";
        public const string FIELD_COMPUTER_NAME = "COMPUTER_NAME";
        public const string FIELD_COMPUTER_IP = "COMPUTER_IP";
        public const string FIELD_VERSION = "VERSION";
        public const string FIELD_LOGIN_TIME = "LOGIN_TIME";
        public const string FIELD_LOGOUT_TIME = "LOGOUT_TIME";
        public const string FIELD_SERVER_IP = "SERVER_IP";
        public const string DATABASE_TABLE_NAME = "RBAC_LOGIN_LOG";

        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties> 
                                                                    { 
                                                                        { FIELD_LOGIN_LOG_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_USER_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_SITE, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_LANGUAGE, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_COMPUTER_NAME, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_COMPUTER_IP, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_VERSION, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_LOGIN_TIME, new FieldProperties(typeof(DateTime), false, "") }, 
                                                                        { FIELD_LOGOUT_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_SERVER_IP, new FieldProperties(typeof(string), true, "") }
                                                                    };

        public static DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable(DATABASE_TABLE_NAME);

            dataTable.Columns.Clear();

            foreach (KeyValuePair<string, FieldProperties> keyValue in _fields)
            {
                dataTable.Columns.Add(keyValue.Key);
            }

            return dataTable;
        }
    }
}
