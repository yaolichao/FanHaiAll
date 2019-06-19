using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库表FMM_REASON_CODE（原因代码）及其字段的实体类。
    /// </summary>
    public class FMM_REASON_CODE_FIELDS : TableFields
    {
        /// <summary>
        /// 主键。
        /// </summary>
        public const string FIELD_REASON_CODE_KEY = "REASON_CODE_KEY";                   //reason code ID
        /// <summary>
        /// 原因代码名称。
        /// </summary>
        public const string FIELD_REASON_CODE_NAME = "REASON_CODE_NAME";                 //reason code名称
        /// <summary>
        /// 原因代码类型。D:不良 H：暂停 S：报废 T：终结 SD:差异 TK：退库 DLR：超时 RC:回收
        /// </summary>
        public const string FIELD_REASON_CODE_TYPE = "REASON_CODE_TYPE";
        /// <summary>
        /// 原因代码分类。
        /// </summary>
        public const string FIELD_REASON_CODE_CLASS = "REASON_CODE_CLASS";
        /// <summary>
        /// 原因代码描述。
        /// </summary>
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";                         //描述
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";                                     //编辑者
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";                               //编辑时间
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";                       //编辑时间时区
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "FMM_REASON_CODE";       //TABLE NAME
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
                                                                  {FIELD_REASON_CODE_KEY , new FieldProperties() },
                                                                  {FIELD_REASON_CODE_NAME , new FieldProperties() },
                                                                  {FIELD_REASON_CODE_TYPE, new FieldProperties() },
                                                                  {FIELD_REASON_CODE_CLASS, new FieldProperties() },
                                                                  {FIELD_DESCRIPTIONS , new FieldProperties() },
                                                                  {FIELD_EDITOR , new FieldProperties() },     
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime), true, DateTime.Now) }, 
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties() }
                                                              };
    }
}
