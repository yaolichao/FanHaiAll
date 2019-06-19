using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示电池片回收的数据表字段实体类。
    /// </summary>
    public class WIP_RECOVERED_FIELDS : TableFields
    {
        /// <summary>
        /// 回收批次的操作主键。
        /// </summary>
        public const string FIELD_TRANSACTION_KEY = "TRANSACTION_KEY";
        /// <summary>
        /// 被回收批次的操作记录主键，回收类型为报废时该栏位为空白值
        /// </summary>
        public const string FIELD_BERECOVERED_TRANSACTION_KEY = "BERECOVERED_TRANSACTION_KEY";
        /// <summary>
        /// 回收批次主键。
        /// </summary>
        public const string FIELD_RECOVERED_LOT_KEY = "RECOVERED_LOT_KEY";
        /// <summary>
        /// 被回收批次主键，回收类型为报废时该栏位为空白值
        /// </summary>
        public const string FIELD_BERECOVERED_LOT_KEY = "BERECOVERED_LOT_KEY";
        /// <summary>
        /// 回收对应的操作类型，撤销电池片报废(CELLSCRAP)，撤销电池片补片(PATCH)
        /// </summary>
        public const string FIELD_RECOVERED_TYPE = "RECOVERED_TYPE";
        /// <summary>
        /// 被回收原因主键。
        /// </summary>
        public const string FIELD_BERECOVERED_REASON_CODE_KEY = "BERECOVERED_REASON_CODE_KEY";
        /// <summary>
        /// 原因代码名称。
        /// </summary>
        public const string FIELD_BERECOVERED_REASON_CODE_NAME = "BERECOVERED_REASON_CODE_NAME";
        /// <summary>
        /// 原因代码主键。
        /// </summary>
        public const string FIELD_REASON_CODE_KEY = "REASON_CODE_KEY";
        /// <summary>
        /// 原因代码名称。
        /// </summary>
        public const string FIELD_REASON_CODE_NAME = "REASON_CODE_NAME";
        /// <summary>
        /// 回收数量。
        /// </summary>
        public const string FIELD_RECOVERED_QUANTITY = "RECOVERED_QUANTITY";
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
        public const string DATABASE_TABLE_NAME = "WIP_RECOVERED";
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
                                                                  {FIELD_BERECOVERED_TRANSACTION_KEY, new FieldProperties()},
                                                                  {FIELD_RECOVERED_LOT_KEY, new FieldProperties()},
                                                                  {FIELD_BERECOVERED_LOT_KEY, new FieldProperties()},
                                                                  {FIELD_RECOVERED_TYPE, new FieldProperties()},
                                                                  {FIELD_BERECOVERED_REASON_CODE_KEY,new FieldProperties() }, 
                                                                  {FIELD_BERECOVERED_REASON_CODE_NAME,new FieldProperties()}, 
                                                                  {FIELD_REASON_CODE_KEY,new FieldProperties() }, 
                                                                  {FIELD_REASON_CODE_NAME,new FieldProperties()}, 
                                                                  {FIELD_RECOVERED_QUANTITY,new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties() }, 
                                                                  {FIELD_ENTERPRISE_NAME,new FieldProperties() },
                                                                  {FIELD_ROUTE_KEY,new FieldProperties()},
                                                                  {FIELD_ROUTE_NAME,new FieldProperties()},
                                                                  {FIELD_STEP_KEY,new FieldProperties()},
                                                                  {FIELD_STEP_NAME,new FieldProperties()},
                                                                  {FIELD_DESCRIPTION,new FieldProperties()},
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE_KEY,new FieldProperties() },                                                  
                                                                };
                                                                       
    }
}
