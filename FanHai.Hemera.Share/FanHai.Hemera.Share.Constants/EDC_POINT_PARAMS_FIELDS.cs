using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库表EDC_POINT_PARAMS（设备数据采集点参数）中字段名称的实体类。
    /// </summary>
    public class EDC_POINT_PARAMS_FIELDS : TableFields
    {

        #region columns define
        /// <summary>
        /// 主键。
        /// </summary>
        public const string FIELD_ROW_KEY = "ROW_KEY";
        /// <summary>
        /// 采集点主键。
        /// </summary>
        public const string FIELD_EDC_POINT_ROWKEY = "EDC_POINT_ROWKEY";
        /// <summary>
        /// 参数个数。
        /// </summary>
        public const string FIELD_PARAM_COUNT = "PARAM_COUNT";
        /// <summary>
        /// 参数组名称。
        /// </summary>
        public const string FIELD_EDC_NAME = "EDC_NAME";
        public const string FIELD_EDC_VERSION = "EDC_VERSION";
        /// <summary>
        /// 参数名称。
        /// </summary>
        public const string FIELD_PARAM_NAME = "PARAM_NAME";
        /// <summary>
        /// 上规格线。USL
        /// </summary>
        public const string FIELD_UPPER_BOUNDARY = "UPPER_BOUNDARY";
        /// <summary>
        /// 上控制线。UCL
        /// </summary>
        public const string FIELD_UPPER_SPEC = "UPPER_SPEC";
        /// <summary>
        /// 暂时未使用。
        /// </summary>
        public const string FIELD_UPPER_CONTROL = "UPPER_CONTROL";
        /// <summary>
        /// 目标值
        /// </summary>
        public const string FIELD_TARGET = "TARGET";
        /// <summary>
        /// 暂时未使用。
        /// </summary>
        public const string FIELD_LOWER_CONTROL = "LOWER_CONTROL";
        /// <summary>
        /// 下控制线。LCL
        /// </summary>
        public const string FIELD_LOWER_SPEC = "LOWER_SPEC";
        /// <summary>
        /// 下规格线。LSL
        /// </summary>
        public const string FIELD_LOWER_BOUNDARY = "LOWER_BOUNDARY";
        /// <summary>
        /// 参数主键。
        /// </summary>
        public const string FIELD_PARAM_KEY = "PARAM_KEY";
        /// <summary>
        /// 参数索引。
        /// </summary>
        public const string FIELD_PARAM_INDEX = "PARAM_INDEX";
        /// <summary>
        /// 参数类型。R-只读 W-设备可写 N-正常 C-计算
        /// </summary>
        public const string FIELD_PARAM_TYPE = "PARAM_TYPE";
        /// <summary>
        /// 公式。
        /// </summary>
        public const string FIELD_PARAM_FORMULA = "PARAM_FORMULA";
        /// <summary>
        /// 允许输入的最小值，包含最小值，如果为空不进行限制。
        /// </summary>
        public const string FIELD_PARAM_ALLOW_MIN_VALUE = "ALLOW_MIN_VALUE";
        /// <summary>
        /// 允许输入的最大值，包含最大值，如果为空不进行限制。
        /// </summary>
        public const string FIELD_PARAM_ALLOW_MAX_VALUE = "ALLOW_MAX_VALUE";
        #endregion

        #region table name define
        /// <summary>
        /// 表名。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "EDC_POINT_PARAMS";                                //TABLE NAME
        #endregion

        #region get TABLE_NAME
        /// <summary>
        /// 表名。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        #endregion

        #region get FIELDS
        /// <summary>
        /// 字段字典。
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties> { 
            {FIELD_ROW_KEY , new FieldProperties() },
            {FIELD_EDC_POINT_ROWKEY , new FieldProperties() },
            {FIELD_EDC_NAME , new FieldProperties() },
            {FIELD_EDC_VERSION , new FieldProperties() },
            {FIELD_PARAM_NAME , new FieldProperties() },
            {FIELD_UPPER_BOUNDARY , new FieldProperties() },
            {FIELD_UPPER_SPEC , new FieldProperties() },
            {FIELD_UPPER_CONTROL , new FieldProperties() },
            {FIELD_TARGET , new FieldProperties() },
            {FIELD_LOWER_CONTROL , new FieldProperties() },
            {FIELD_LOWER_SPEC , new FieldProperties() },
            {FIELD_LOWER_BOUNDARY , new FieldProperties()},
            {FIELD_PARAM_COUNT , new FieldProperties() },
            {FIELD_PARAM_KEY , new FieldProperties() },
            {FIELD_PARAM_INDEX , new FieldProperties() },
            {FIELD_PARAM_TYPE , new FieldProperties()},
            {FIELD_PARAM_FORMULA , new FieldProperties() }
        };
    }
}
