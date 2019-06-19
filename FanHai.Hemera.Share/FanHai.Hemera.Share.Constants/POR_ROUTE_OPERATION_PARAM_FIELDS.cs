using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工序参数的数据库表及其字段的实体类。
    /// </summary>
    public class POR_ROUTE_OPERATION_PARAM_FIELDS : TableFields
    {
        /// <summary>
        /// 工序参数主键。
        /// </summary>
        public const string FIELD_ROUTE_OPERATION_PARAM_KEY = "ROUTE_OPERATION_PARAM_KEY";
        /// <summary>
        /// 工序主键。
        /// </summary>
        public const string FIELD_OPERATION_VER_KEY = "ROUTE_OPERATION_VER_KEY";
        /// <summary>
        /// 参数主键,BASE_PARAMTER中PARAM_CATEGORY=5的参数。
        /// </summary>
        public const string FIELD_PARAM_KEY = "PARAM_KEY";
        /// <summary>
        /// 参数名称。
        /// </summary>
        public const string FIELD_PARAM_NAME = "PARAM_NAME";
        /// <summary>
        /// 数据类型1：整型 2：日期 3：日期时间 4：布尔 5：字符串 6：浮点型 7：设置 8：关联。
        /// </summary>
        public const string FIELD_DATA_TYPE = "DATA_TYPE";
        /// <summary>
        /// 是否必须输入 0：否 1：是。
        /// </summary>
        public const string FIELD_IS_MUSTINPUT = "IS_MUSTINPUT";
        /// <summary>
        /// 是否上料 0：否 1：是。
        /// </summary>
        public const string FIELD_IS_FEEDING = "IS_FEEDING";
        /// <summary>
        /// 数据来源 0：手工输入 1：设备接口 2：上料功能。
        /// </summary>
        public const string FIELD_DATA_FROM = "DATA_FROM";
        /// <summary>
        /// 是否只读  0：否 1：是。
        /// </summary>
        public const string FIELD_IS_READONLY = "IS_READONLY";
        /// <summary>
        /// 是否自动填充前一次的值 0 否 1是
        /// </summary>
        public const string FIELD_IS_COMPLETE_PREVALUE = "IS_COMPLETE_PREVALUE";
        /// <summary>
        /// 采集时刻 0：进站时采集 1：出站时采集。
        /// </summary>
        public const string FIELD_DC_TYPE = "DC_TYPE";
        /// <summary>
        /// 验证规则  0：不验证 1：前缀匹配工单设定的内容2：后缀匹配工单设定的内容 3：内容包含工单设定的数据 4：完全匹配工单设定的数据。
        /// </summary>
        public const string FIELD_VALIDATE_RULE = "VALIDATE_RULE";
        /// <summary>
        /// 验证失败规则 0：提示验证失败。
        /// </summary>
        public const string FIELD_VALIDATE_FAILED_RULE = "VALIDATE_FAILED_RULE";
        /// <summary>
        /// 长度计算规则
        /// </summary>
        public const string FIELD_CALCULATE_RULE = "CALCULATERULE";
        /// <summary>
        /// 长度设定
        /// </summary>
        public const string FIELD_FIELD_LENGTH = "FIELDLENGTH";
        /// <summary>
        /// 验证失败提示信息。
        /// </summary>
        public const string FIELD_VALIDATE_FAILED_MSG = "VALIDATE_FAILED_MSG";
        /// <summary>
        /// 是否已删除。0：删除 1：未删除
        /// </summary>
        public const string FIELD_IS_DELETED = "IS_DELETED";
        /// <summary>
        /// 序号。
        /// </summary>
        public const string FIELD_PARAM_INDEX = "PARAM_INDEX";
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
        /// 工序参数表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_OPERATION_PARAM";
        /// <summary>
        /// 验证料号。
        /// </summary>
        public const string FIELD_MAT_RULE = "VALIDATE_MAT_RULE";
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
                                                                  {FIELD_ROUTE_OPERATION_PARAM_KEY , new FieldProperties() },
                                                                  {FIELD_OPERATION_VER_KEY , new FieldProperties() },
                                                                  {FIELD_PARAM_KEY, new FieldProperties() }, 
                                                                  {FIELD_PARAM_NAME , new FieldProperties() },
                                                                  {FIELD_DATA_TYPE , new FieldProperties(typeof(decimal),true,5) },
                                                                  {FIELD_DATA_FROM , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_DC_TYPE , new FieldProperties(typeof(decimal),true,1) },
                                                                  {FIELD_IS_MUSTINPUT , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_IS_FEEDING , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_IS_READONLY , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_IS_COMPLETE_PREVALUE , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_PARAM_INDEX , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_VALIDATE_RULE , new FieldProperties(typeof(decimal),true,1) },
                                                                  {FIELD_CALCULATE_RULE , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_FIELD_LENGTH , new FieldProperties() },
                                                                  {FIELD_MAT_RULE , new FieldProperties() },
                                                                  {FIELD_VALIDATE_FAILED_RULE , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_VALIDATE_FAILED_MSG , new FieldProperties() },
                                                                  {FIELD_IS_DELETED , new FieldProperties(typeof(decimal),true,0) },
                                                                  {FIELD_EDITOR , new FieldProperties() },  //editor
                                                                  {FIELD_EDIT_TIME ,new FieldProperties(typeof(DateTime), true, DateTime.Now)},  //edit time
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() }  //edit time zone
                                                                };
    }
}

