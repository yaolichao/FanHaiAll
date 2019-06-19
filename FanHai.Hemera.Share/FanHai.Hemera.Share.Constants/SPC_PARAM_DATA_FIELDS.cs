using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据表SPC_PARAM_DATA的信息。
    /// </summary>
    public class SPC_PARAM_DATA_FIELDS : TableFields
    {
        /// <summary>
        /// 参数主键
        /// </summary>
        public const string PARAM_KEY = "PARAM_KEY";
        /// <summary>
        /// 具体批次进行数据采集的主键（EDC_MAIN_INS）
        /// </summary>       
        public const string EDC_INS_KEY = "EDC_INS_KEY";
        /// <summary>
        /// 参数值
        /// </summary>   
        public const string PARAM_VALUE = "PARAM_VALUE";
        /// <summary>
        /// 参数是否有效(0:无效，1:有效，2:已处理)
        /// </summary>    
        public const string VALID_FLAG = "VALID_FLAG";
        /// <summary>
        /// 删除标记， 1:删除 默认为0
        /// </summary>
        public const string DELETED_FLAG = "DELETED_FLAG";
        /// <summary>
        /// 主键
        /// </summary>
        public const string COL_KEY = "COL_KEY";
        /// <summary>
        /// 创建者
        /// </summary>
        public const string CREATOR = "CREATOR";
        /// <summary>
        /// 创建日期
        /// </summary>
        public const string CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 数据是否被修正，1修正，默认为0
        /// </summary>
        public const string EDIT_FLAG = "EDIT_FLAG";
        /// <summary>
        /// 修正者
        /// </summary>
        public const string EDITOR = "EDITOR";
        /// <summary>
        /// 修正原因
        /// </summary>
        public const string EDIT_REASON = "EDIT_REASON";
        /// <summary>
        /// 工单
        /// </summary>
        public const string ORDER_NUMBER = "ORDER_NUMBER";
        /// <summary>
        /// 设备主键
        /// </summary>
        public const string EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 班别
        /// </summary>
        public const string SHIFT_VALUE = "SHIFT_VALUE";

        //Add by Genchille.yang
        /// <summary>
        /// 工厂车间
        /// </summary>
        public const string LOCATION_KEY = "LOCATION_KEY";
        /// <summary>
        /// 工步名称
        /// </summary>
        public const string STEP_NAME = "STEP_NAME";
        /// <summary>
        /// 成品料号
        /// </summary>
        public const string PART_NO = "PART_NO";
        /// <summary>
        /// 供应商批次/物料批次
        /// </summary>
        public const string MATERIAL_LOT = "MATERIAL_LOT";
        /// <summary>
        /// 供应商名称
        /// </summary>
        public const string SUPPLIER = "SUPPLIER";
        /// <summary>
        /// 成品类型
        /// </summary>
        public const string PART_TYPE = "PART_TYPE";

        /// <summary>
        /// 批次号
        /// </summary>
        public const string LOT_NUMBER = "LOT_NUMBER";

        /// <summary>
        /// 数据表名称
        /// </summary>
        public const string DATABASE_TABLE_NAME = "SPC_PARAM_DATA";

        //----------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SPC呈现的数据表
        /// </summary>
        public const string DB_FOR_POINTS = "DATA_POINTS";
        /// <summary>
        /// SPC标准规格表
        /// </summary>
        public const string DB_FOR_STANDARD = "DATA_STANDARD";

        public const string V_VALUE = "V_VALUE";
        public const string S_VALUE = "S_VALUE";
        public const string R_VALUE = "R_VALUE";
        public const string P_COUNT = "P_COUNT";
        public const string A_VALUE = "A_VALUE";
        public const string MIN_V_VALUE = "MIN_V_VALUE";
        public const string MAX_V_VALUE = "MAX_V_VALUE";
        public const string MIN_S_VALUE = "MIN_S_VALUE";
        public const string MAX_S_VALUE = "MAX_S_VALUE";
        public const string MIN_R_VALUE = "MIN_R_VALUE";
        public const string MAX_R_VALUE = "MAX_R_VALUE";

        public const string V_AVG = "V_AVG";
        public const string R_AVG = "R_AVG";
        public const string S_AVG = "S_AVG";
        public const string P_SIGMA = "P_SIGMA";
        /// <summary>
        /// 某一个点的明细数据量
        /// </summary>
        public const string P_DTL_COUNT = "P_DTL_COUNT";
        /// <summary>
        /// 点的个数
        /// </summary>
        public const string P_A_COUNT = "P_A_COUNT";
        public const string POINT_KEY = "POINT_KEY";
        //----------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 数据表名称。
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
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                 
                                                                  {PARAM_KEY, new FieldProperties() },
                                                                  {EDC_INS_KEY, new FieldProperties() },
                                                                  {PARAM_VALUE, new FieldProperties()},                                                                 
                                                                  {VALID_FLAG, new FieldProperties() },
                                                                  {DELETED_FLAG, new FieldProperties() },
                                                                  {COL_KEY,new FieldProperties()}
                                                                };

    }
}
