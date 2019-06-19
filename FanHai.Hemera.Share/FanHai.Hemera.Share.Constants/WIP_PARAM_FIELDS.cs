using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示批次采集参数的数据表及其字段实体类。
    /// </summary>
    public class WIP_PARAM_FIELDS : TableFields
    {
        /// <summary>
        /// 操作主键。
        /// </summary>
        public const string FIELD_TRANSACTION_KEY = "TRANSACTION_KEY";
        /// <summary>
        /// 参数主键。
        /// </summary>
        public const string FIELD_PARAM_KEY = "PARAM_KEY";
        /// <summary>
        /// 参数名称。
        /// </summary>
        public const string FIELD_PARAM_NAME = "PARAM_NAME";
        /// <summary>
        /// 参数索引。
        /// </summary>
        public const string FIELD_PARAM_INDEX = "PARAM_INDEX";
        /// <summary>
        /// 参数值。
        /// </summary>
        public const string FIELD_PARAM_VALUE = "PARAM_VALUE";
        /// <summary>
        /// 批次主键。
        /// </summary>
        public const string FIELD_LOT_KEY = "LOT_KEY";
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
        public const string DATABASE_TABLE_NAME = "WIP_PARAM";
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
                                                                  {FIELD_PARAM_KEY,new FieldProperties() }, 
                                                                  {FIELD_PARAM_NAME,new FieldProperties() }, 
                                                                  {FIELD_PARAM_INDEX,new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_PARAM_VALUE,new FieldProperties() }, 
                                                                  {FIELD_LOT_KEY,new FieldProperties() }, 
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties() }, 
                                                                  {FIELD_ENTERPRISE_NAME,new FieldProperties() },
                                                                  {FIELD_ROUTE_KEY,new FieldProperties()},
                                                                  {FIELD_ROUTE_NAME,new FieldProperties()},
                                                                  {FIELD_STEP_KEY,new FieldProperties()},
                                                                  {FIELD_STEP_NAME,new FieldProperties()},
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE,new FieldProperties() },        
                                                               };
    }
}
