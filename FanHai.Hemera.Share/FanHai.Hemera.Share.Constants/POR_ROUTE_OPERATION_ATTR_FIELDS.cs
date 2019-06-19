using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工序自定义属性数据库表及其字段的实体类。
    /// </summary>
    public class POR_ROUTE_OPERATION_ATTR_FIELDS : TableFields
    {
        /// <summary>
        /// 工序主键。
        /// </summary>
        public const string FIELD_OPERATION_VER_KEY = "ROUTE_OPERATION_VER_KEY";
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
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        /// <summary>
        /// 工序自定义属性表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_OPERATION_ATTR";

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
                                                                  {FIELD_OPERATION_VER_KEY , new FieldProperties() },         //OPERATION ID
                                                                  {FIELD_ATTRIBUTE_KEY , new FieldProperties() },   //属性ID
                                                                  {FIELD_ATTRIBUTE_NAME , new FieldProperties() },  //属性名称
                                                                  {FIELD_ATTRIBUTE_VALUE , new FieldProperties() },  //属性值
                                                                  {FIELD_EDITOR , new FieldProperties() },  //editor
                                                                  {FIELD_EDIT_TIME ,new FieldProperties(typeof(DateTime), true, DateTime.Now)},  //edit time
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() }  //edit time zone
                                                                };
    }
}

