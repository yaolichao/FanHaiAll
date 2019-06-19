using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示批次出站工步操作记录的数据表及其字段实体类。
    /// </summary>
    public class WIP_STEP_TRANSACTION_FIELDS:TableFields
    {
        /// <summary>
        /// 操作主键。
        /// </summary>
        public const string FIELD_TRANSACTION_KEY = "TRANSACTION_KEY";
        /// <summary>
        /// 工艺流程组主键。
        /// </summary>
        public const string FIELD_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_ROUTE_KEY = "ROUTE_KEY";
        /// <summary>
        /// 工步主键。
        /// </summary>
        public const string FIELD_STEP_KEY="STEP_KEY";
        /// <summary>
        /// 下一工艺流程组主键。
        /// </summary>
        public const string FIELD_TO_ENTERPRISE_KEY = "TO_ENTERPRISE_KEY";
        /// <summary>
        /// 下一工艺流程组名称。
        /// </summary>
        public const string FIELD_TO_ENTERPRISE_NAME = "TO_ENTERPRISE_NAME";
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_TO_ROUTE_KEY = "TO_ROUTE_KEY";
        /// <summary>
        /// 工艺流程组名称。
        /// </summary>
        public const string FIELD_TO_ROUTE_NAME = "TO_ROUTE_NAME";
        /// <summary>
        /// 工步主键。
        /// </summary>
        public const string FIELD_TO_STEP_KEY = "TO_STEP_KEY";
        /// <summary>
        /// 工步名称。
        /// </summary>
        public const string FIELD_TO_STEP_NAME = "TO_STEP_NAME";
        //public const string FIELD_STATUS="STATUS";
        //public const string FIELD_CURRENT_RULE_KEY="CURRENT_RULE_KEY";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR="EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME="EDIT_TIME";
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE="EDIT_TIMEZONE";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_STEP_TRANSACTION";
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
                                                                        {FIELD_ENTERPRISE_KEY, new FieldProperties()},
                                                                        {FIELD_ROUTE_KEY, new FieldProperties()},
                                                                        {FIELD_STEP_KEY, new FieldProperties()},
                                                                        {FIELD_TO_ENTERPRISE_KEY, new FieldProperties()},
                                                                        {FIELD_TO_ENTERPRISE_NAME, new FieldProperties()},
                                                                        {FIELD_TO_ROUTE_KEY , new FieldProperties()},
                                                                        {FIELD_TO_ROUTE_NAME , new FieldProperties()},
                                                                        {FIELD_TO_STEP_KEY, new FieldProperties()},
                                                                        {FIELD_TO_STEP_NAME, new FieldProperties()},
                                                                        {FIELD_EDITOR, new FieldProperties()},
                                                                        {FIELD_EDIT_TIME,  new FieldProperties(typeof(DateTime),false, DateTime.Now) },
                                                                        {FIELD_EDIT_TIMEZONE, new FieldProperties() }
                                                                    }; 
        }
}
