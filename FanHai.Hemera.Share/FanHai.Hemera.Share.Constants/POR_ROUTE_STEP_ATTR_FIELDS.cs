using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工步自定义属性数据库表及其字段的实体类。
    /// </summary>
    public class POR_ROUTE_STEP_ATTR_FIELDS : TableFields
    {
        /// <summary>
        /// 工步主键。
        /// </summary>
        public const string FIELD_ROUTE_STEP_KEY = "ROUTE_STEP_KEY";
        /// <summary>
        /// 自定义属性主键。
        /// </summary>
        public const string FIELD_ATTRIBUTE_KEY = "ATTRIBUTE_KEY";
        /// <summary>
        /// 自定义属性名称。
        /// </summary>
        public const string FIELD_ATTRIBUTE_NAME = "ATTRIBUTE_NAME";
        /// <summary>
        /// 自定义属性值。
        /// </summary>
        public const string FIELD_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE_KEY = "EDIT_TIMEZONE_KEY";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_STEP_ATTR";
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
                                                                  {FIELD_ROUTE_STEP_KEY, new FieldProperties() },         
                                                                  {FIELD_ATTRIBUTE_KEY,new FieldProperties() },  
                                                                  {FIELD_ATTRIBUTE_NAME,new FieldProperties() },  
                                                                  {FIELD_ATTRIBUTE_VALUE,new FieldProperties() }, 
                                                                  {FIELD_EDITOR ,new FieldProperties() },
                                                                  {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE_KEY,new FieldProperties() }
                                                                };
    }
}
