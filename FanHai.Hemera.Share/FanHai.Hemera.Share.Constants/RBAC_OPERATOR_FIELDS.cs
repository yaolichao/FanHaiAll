using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    public class RBAC_OPERATOR_FIELDS : TableFields
    {
        public const string COL_KEY = "COL_KEY";//主键
        public const string BADGE = "BADGE"; //工号
        public const string USER_NAME = "USER_NAME";//用户名
        public const string STATUS = "STATUS";//用户状态
        public const string CREATOR = "CREATOR";//创建者
        public const string ROUTE_STEP = "ROUTE_STEP";//岗位
        public const string CREATE_TIME = "CREATE_TIME";//创建时间

        public const string DATABASE_TABLE_NAME = "RBAC_OPERATOR";
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
                                                                        { COL_KEY, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { BADGE, new FieldProperties(typeof(string), false, string.Empty) }, 
                                                                        { USER_NAME, new FieldProperties(typeof(string), true, string.Empty) }, 
                                                                        { STATUS, new FieldProperties(typeof(int), false, 0) },
                                                                        {CREATOR,new FieldProperties()},
                                                                        {ROUTE_STEP,new FieldProperties()},
                                                                        {CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)}
                                                                    };

        public static DataTable CreateDataTable(bool isPaging)
        {
            DataTable dataTable = new DataTable(DATABASE_TABLE_NAME);

            dataTable.Columns.Clear();

            if (isPaging)
            {
                dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);
            }

            foreach (KeyValuePair<string, FieldProperties> keyValue in _fields)
            {
                dataTable.Columns.Add(keyValue.Key);
            }

            return dataTable;
        }
    }
}
