using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示电池片补片数据表及其字段的实体类。
    /// </summary>
    public class WIP_PATCH_FIELDS : TableFields
    {
        /// <summary>
        /// 补片批次的操作主键。
        /// </summary>
        public const string FIELD_TRANSACTION_KEY = "TRANSACTION_KEY";
        /// <summary>
        /// 被补片批次的操作记录主键。
        /// </summary>
        public const string FIELD_PATCHED_TRANSACTION_KEY = "PATCHED_TRANSACTION_KEY";
        /// <summary>
        /// 补片批次主键。
        /// </summary>
        public const string FIELD_PATCH_LOT_KEY = "PATCH_LOT_KEY";
        /// <summary>
        /// 被补片批次主键。
        /// </summary>
        public const string FIELD_PATCHED_LOT_KEY = "PATCHED_LOT_KEY";
        /// <summary>
        /// 原因代码分类。
        /// </summary>
        public const string FIELD_REASON_CODE_CLASS = "REASON_CODE_CLASS";
        /// <summary>
        /// 原因代码主键。
        /// </summary>
        public const string FIELD_REASON_CODE_KEY = "REASON_CODE_KEY";
        /// <summary>
        /// 原因代码名称。
        /// </summary>
        public const string FIELD_REASON_CODE_NAME = "REASON_CODE_NAME";
        /// <summary>
        /// 补片数量。
        /// </summary>
        public const string FIELD_PATCH_QUANTITY = "PATCH_QUANTITY";
        /// <summary>
        /// 工艺流程组主键。
        /// </summary>
        public const string FIELD_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        /// <summary>
        /// 工艺流程组名称。
        /// </summary>
        public const string FIELD_ENTERPRISE_NAME = "ENTERPRISE_NAME";
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_ROUTE_KEY = "ROUTE_KEY";
        /// <summary>
        /// 工艺流程名称。
        /// </summary>
        public const string FIELD_ROUTE_NAME = "ROUTE_NAME";
        /// <summary>
        /// 工步主键。
        /// </summary>
        public const string FIELD_STEP_KEY = "STEP_KEY";
        /// <summary>
        /// 工步名称。
        /// </summary>
        public const string FIELD_STEP_NAME = "STEP_NAME";
        /// <summary>
        /// 原因说明。
        /// </summary>
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        /// <summary>
        /// 责任人。
        /// </summary>
        public const string FIELD_RESPONSIBLE_PERSON = "RESPONSIBLE_PERSON";
        /// <summary>
        /// 标识是否仅仅只做补片操作。 0：先报废再补片， 1：仅做补片。
        /// </summary>
        public const string FIELD_IS_ONLY_PATCHED = "IS_ONLY_PATCHED";
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
        public const string DATABASE_TABLE_NAME = "WIP_PATCH";
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
        /// 数据库表中字段集合。
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
                                                                  {FIELD_TRANSACTION_KEY, new FieldProperties()},
                                                                  {FIELD_PATCHED_TRANSACTION_KEY, new FieldProperties()},
                                                                  {FIELD_PATCH_LOT_KEY, new FieldProperties()},
                                                                  {FIELD_PATCHED_LOT_KEY, new FieldProperties()},
                                                                  {FIELD_REASON_CODE_KEY,new FieldProperties() }, 
                                                                  {FIELD_REASON_CODE_NAME,new FieldProperties()}, 
                                                                  {FIELD_REASON_CODE_CLASS,new FieldProperties()}, 
                                                                  {FIELD_PATCH_QUANTITY,new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties() }, 
                                                                  {FIELD_ENTERPRISE_NAME,new FieldProperties() },
                                                                  {FIELD_ROUTE_KEY,new FieldProperties()},
                                                                  {FIELD_ROUTE_NAME,new FieldProperties()},
                                                                  {FIELD_STEP_KEY,new FieldProperties()},
                                                                  {FIELD_STEP_NAME,new FieldProperties()},
                                                                  {FIELD_DESCRIPTION,new FieldProperties()},
                                                                  {FIELD_RESPONSIBLE_PERSON,new FieldProperties()},
                                                                  {FIELD_IS_ONLY_PATCHED,new FieldProperties(typeof(decimal),true,0)},
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE_KEY,new FieldProperties() },                                                  
                                                                };
                                                                       
    }
}
