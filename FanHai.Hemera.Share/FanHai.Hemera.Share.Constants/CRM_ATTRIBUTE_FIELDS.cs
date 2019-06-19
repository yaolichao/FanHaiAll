using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库表CRM_ATTRIBUTE（属性数据表）中字段名称的实体类。
    /// </summary>
    public class CRM_ATTRIBUTE_FIELDS : TableFields
    {
        /// <summary>
        /// 属性键。
        /// </summary>
        public const string FIELDS_ATTRIBUTE_KEY    = "ATTRIBUTE_KEY";
        /// <summary>
        /// 属性名称。
        /// </summary>
        public const string FIELDS_ATTRIBUTE_NAME   = "ATTRIBUTE_NAME";
        /// <summary>
        /// 属性值。
        /// </summary>
        public const string FIELDS_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";
        /// <summary>
        /// 属性顺序号。
        /// </summary>
        public const string FIELDS_ITEM_ORDER = "ITEM_ORDER";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "CRM_ATTRIBUTE";
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
        /// 获取数据库表中的字段集合
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_ATTRIBUTE_KEY, new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_NAME, new FieldProperties() },
                                                                  {FIELDS_ATTRIBUTE_VALUE, new FieldProperties() },
                                                                  {FIELDS_ITEM_ORDER, new FieldProperties() },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties() }
                                                                };
    }
}

  