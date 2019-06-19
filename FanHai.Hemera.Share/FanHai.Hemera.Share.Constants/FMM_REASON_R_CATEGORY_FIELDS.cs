using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示原因代码类别和原因代码关联关系的数据库表及其字段的实体类。
    /// </summary>
    public class FMM_REASON_R_CATEGORY_FIELDS : TableFields
    {
        /// <summary>
        /// 类别主键。
        /// </summary>
        public const string FIELD_CATEGORY_KEY = "CATEGORY_KEY ";       //reason code category ID 
        /// <summary>
        /// 原因代码主键。
        /// </summary>
        public const string FIELD_REASON_CODE_KEY = "REASON_CODE_KEY";  //reason code ID
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "FMM_REASON_R_CATEGORY";       //TABLE NAME
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 数据库表字段集合。
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                                          {
                                                                              {FIELD_CATEGORY_KEY , new FieldProperties() },
                                                                              {FIELD_REASON_CODE_KEY , new FieldProperties() }
                                                                          };
    }
}
