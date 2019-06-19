using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示电池片不良或组件不良的数据表字段实体类。
    /// </summary>
    public class WIP_DEFECT_FIELDS:TableFields
    {
        /// <summary>
        /// 操作主键。
        /// </summary>
        public const string FIELD_TRANSACTION_KEY = "TRANSACTION_KEY";
        /// <summary>
        /// 原因代码主键。
        /// </summary>
        public const string FIELD_REASON_CODE_KEY = "REASON_CODE_KEY";
        /// <summary>
        /// 原因代码名称。
        /// </summary>
        public const string FIELD_REASON_CODE_NAME = "REASON_CODE_NAME";
        /// <summary>
        /// 原因代码分类。
        /// </summary>
        public const string FIELD_REASON_CODE_CLASS = "REASON_CODE_CLASS";
        /// <summary>
        /// 不良数量。
        /// </summary>
        public const string FIELD_DEFECT_QUANTITY = "DEFECT_QUANTITY";
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
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_DEFECT";
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
                                                                  {FIELD_REASON_CODE_KEY,new FieldProperties() }, 
                                                                  {FIELD_REASON_CODE_NAME,new FieldProperties()},
                                                                  {FIELD_REASON_CODE_CLASS,new FieldProperties()},
                                                                  {FIELD_DEFECT_QUANTITY,new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties() }, 
                                                                  {FIELD_ENTERPRISE_NAME,new FieldProperties() },
                                                                  {FIELD_ROUTE_KEY,new FieldProperties()},
                                                                  {FIELD_ROUTE_NAME,new FieldProperties()},
                                                                  {FIELD_STEP_KEY,new FieldProperties()},
                                                                  {FIELD_STEP_NAME,new FieldProperties()},
                                                                  {FIELD_DESCRIPTION,new FieldProperties()},
                                                                  {FIELD_RESPONSIBLE_PERSON,new FieldProperties()},
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE,new FieldProperties() },        
                                                               };
    }
}
