using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示原因代码类别数据表及其字段的实体类。
    /// </summary>
    public class FMM_REASON_CODE_CATEGORY_FIELDS : TableFields
    {
        /// <summary>
        /// 原因代码类别主键。
        /// </summary>
        public const string FIELD_REASON_CODE_CATEGORY_KEY  = "REASON_CODE_CATEGORY_KEY";  //组别ID
        /// <summary>
        /// 原因代码类别名。
        /// </summary>
        public const string FIELD_REASON_CODE_CATEGORY_NAME = "REASON_CODE_CATEGORY_NAME"; //组别名称
        /// <summary>
        /// 原因代码类别的类型。D:不良 H：暂停 S：报废 T：终结 SD:差异 TK：退库 DLR：超时 RC:回收
        /// </summary>
        public const string FIELD_REASON_CODE_CATEGORY_TYPE = "REASON_CODE_CATEGORY_TYPE"; //Category类型S:报废,H:Hold,D:Defect 
        /// <summary>
        /// 原因代码类别描述。
        /// </summary>
        public const string FIELD_DESCRIPTIONS              = "DESCRIPTIONS";              //组别描述
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR                    = "EDITOR";                    //编辑者
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME                 = "EDIT_TIME";                 //编辑时间
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE             = "EDIT_TIMEZONE";             //编辑时间时区
       
        /// <summary>
        /// 数据表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "FMM_REASON_CODE_CATEGORY";       //TABLE NAME

        /// <summary>
        /// 数据表名称
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 数据表字段集合。
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
                                                                  {FIELD_REASON_CODE_CATEGORY_KEY, new FieldProperties() }, 
                                                                  {FIELD_REASON_CODE_CATEGORY_NAME, new FieldProperties() },
                                                                  {FIELD_REASON_CODE_CATEGORY_TYPE, new FieldProperties() },
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties()},
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DBNull.Value) },
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties() }
                                                              };
    }
}
