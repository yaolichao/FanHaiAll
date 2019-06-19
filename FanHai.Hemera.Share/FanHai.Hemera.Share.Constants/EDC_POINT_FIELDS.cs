using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EDC_POINT必须输入的栏位枚举。
    /// </summary>
    [Flags]
    public enum EDCPointMustInputField
    {
        /// <summary>
        /// 没有必须输入的值。
        /// </summary>
        None=0,
        /// <summary>
        /// 生产批次号必须输入。
        /// </summary>
        LotNo=1
    }
    /// <summary>
    /// 表示数据库表EDC_POINT（设备数据采集点）中字段名称的实体类。
    /// </summary>
    public class EDC_POINT_FIELDS : TableFields
    {
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_ROW_KEY = "ROW_KEY";
        /// <summary>
        /// 标识抽检点设置分组的键
        /// </summary>
        public const string FIELD_GROUP_KEY = "GROUP_KEY"; 
        /// <summary>
        /// 成品物料号
        /// </summary> 
        public const string FIELD_TOPRODUCT = "TOPRODUCT";
        /// <summary>
        /// 成品类型
        /// </summary> 
        public const string FIELD_PART_TYPE = "PART_TYPE";
        /// <summary>
        /// 工序名称
        /// </summary>
        public const string FIELD_OPERATION_NAME = "OPERATION_NAME";
        /// <summary>
        /// 设备主键
        /// </summary> 
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 设备数据采集的动作名称，TRACKIN ， TRACKOUT
        /// </summary>
        public const string FIELD_ACTION_NAME = "ACTION_NAME";
        /// <summary>
        /// 设备数据采集的主键，EDC_MAIN（设备数据采集主表）数据库表的外键。
        /// </summary>
        public const string FIELD_EDC_KEY = "EDC_KEY";
        /// <summary>
        /// 设备数据采集的版本。
        /// </summary>
        public const string FIELD_EDC_VERSION = "EDC_VERSION";
        /// <summary>
        /// 抽样规则的主键，EDC_SP（设备数据采集抽样规则）数据库表的外键。
        /// </summary>
        public const string FIELD_SP_KEY = "SP_KEY";                          
        /// <summary>
        /// 抽样规则的版本号，EDC_SP（设备数据采集抽样规则）数据库表的外键。
        /// </summary>
        public const string FIELD_SP_VERSION = "SP_VERSION";
        /// <summary>
        /// 设备数据采集点的状态。
        /// </summary>
        public const string FIELD_POINT_STATUS = "POINT_STATUS";
        /// <summary>
        /// 编辑原因
        /// </summary>
        public const string FIELD_EDIT_DESC = "EDIT_DESC";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑人
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_ROUTE_VER_KEY = "ROUTE_VER_KEY";
        /// <summary>
        /// 工步主键。
        /// </summary>
        public const string FIELD_STEP_KEY = "STEP_KEY";
        /// <summary>
        /// 是否必须输入生产批次号。
        /// </summary>
        public const string FIELD_MUST_INPUT_FIELD = "MUST_INPUT_FIELD";
        /// <summary>
        /// 抽检点设置组名称。
        /// </summary>
        public const string FIELD_GROUP_NAME = "GROUP_NAME";
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public const string DATABASE_TABLE_NAME = "EDC_POINT";                                //TABLE NAME
        /// <summary>
        /// 获取数据库表的名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 获取数据库表中的字段集合。
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties> { 
                {FIELD_ROW_KEY , new FieldProperties() },
                {FIELD_TOPRODUCT , new FieldProperties() },
                {FIELD_PART_TYPE,new FieldProperties()},
                {FIELD_OPERATION_NAME , new FieldProperties() },
                {FIELD_EQUIPMENT_KEY , new FieldProperties() },
                {FIELD_ACTION_NAME , new FieldProperties() },
                {FIELD_EDC_KEY , new FieldProperties() },
                {FIELD_EDC_VERSION , new FieldProperties() },
                {FIELD_SP_KEY , new FieldProperties() },
                {FIELD_SP_VERSION , new FieldProperties() },
                {FIELD_POINT_STATUS , new FieldProperties() },
                {FIELD_GROUP_KEY , new FieldProperties() },
                {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true, DateTime.Now) },
                {FIELD_EDITOR , new FieldProperties() },
                {FIELD_EDIT_DESC , new FieldProperties() }
        };
    }
}
