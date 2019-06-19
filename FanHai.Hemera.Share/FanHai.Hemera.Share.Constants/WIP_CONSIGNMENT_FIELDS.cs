using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示组件包装表及其字段的实体类。
    /// </summary>
    public class WIP_CONSIGNMENT_FIELDS : TableFields
    {
        /// <summary>
        /// 客检。
        /// </summary>
        public const string FIELDS_CUST_CHECK = "CUSTCHECK";
        /// <summary>
        /// 功率范围。
        /// </summary>
        public const string FIELDS_POWER_RANGE = "POWER_RANGE";
        /// <summary>
        /// 主键。
        /// </summary>
        public const string FIELDS_CONSIGNMENT_KEY = "CONSIGNMENT_KEY";
        /// <summary>
        /// 托盘号。
        /// </summary>
        public const string FIELDS_PALLET_NO = "PALLET_NO";
        /// <summary>
        /// 虚拟托盘号。
        /// </summary>
        public const string FIELDS_VIRTUAL_PALLET_NO = "VIRTUAL_PALLET_NO";
        /// <summary>
        /// 组别（0：保存；1：包装；2：入库检；3:已入库）。
        /// </summary>
        public const string FIELDS_CS_DATA_GROUP = "CS_DATA_GROUP";
        /// <summary>
        /// SAP料号。
        /// </summary>
        public const string FIELDS_SAP_NO = "SAP_NO";
        /// <summary>
        /// 功率档位。
        /// </summary>
        public const string FIELDS_POWER_LEVEL = "POWER_LEVEL";
        /// <summary>
        /// 托等级。
        /// </summary>
        public const string FIELDS_GRADE = "GRADE";
        /// <summary>
        /// 包装分档代码。
        /// </summary>
        public const string FIELDS_PS_CODE = "PS_CODE";
        /// <summary>
        /// 包装分档序号
        /// </summary>
        public const string FIELDS_PS_SEQ = "PS_SEQ";
        /// <summary>
        /// 包装子分档代码。
        /// </summary>
        public const string FIELDS_PS_DTL_SUBCODE = "PS_DTL_SUBCODE";
        /// <summary>
        /// 班别。
        /// </summary>
        public const string FIELDS_SHIFT = "SHIFT";
        /// <summary>
        /// 工单号。
        /// </summary>
        public const string FIELDS_WORKNUMBER = "WORKNUMBER";
        /// <summary>
        /// 产品ID号
        /// </summary>
        public const string FIELDS_PRO_ID = "PRO_ID";
        /// <summary>
        /// 平均功率
        /// </summary>
        public const string FIELDS_AVG_POWER = "AVG_POWER";
        /// <summary>
        /// 平均功率范围
        /// </summary>
        public const string FIELDS_AVG_POWER_RANGE = "AVG_POWER_RANGE";
        /// <summary>
        /// 花色
        /// </summary>
        public const string FIELDS_LOT_COLOR = "LOT_COLOR";
        /// <summary>
        /// 总功率
        /// </summary>
        public const string FIELDS_TOTLE_POWER = "TOTLE_POWER";
        /// <summary>
        /// 客户编码
        /// </summary>
        public const string FIELDS_CUSTOMER_NO = "CUSTOMER_NO";
        /// <summary>
        /// 组件数量
        /// </summary>
        public const string FIELDS_LOT_NUMBER_QTY = "LOT_NUMBER_QTY";
        /// <summary>
        /// 满托数量
        /// </summary>
        public const string FIELDS_FULL_QTY = "FULL_QTY";
        /// <summary>
        /// 是否尾单(0：正常工单，1:尾单，2:BCP的返工托)。
        /// </summary>
        public const string FIELDS_LAST_PALLET = "LAST_PALLET";
        /// <summary>
        /// 车间主键。
        /// </summary>
        public const string FIELDS_ROOM_KEY = "ROOM_KEY";
        /// <summary>
        /// 包装台ID
        /// </summary>
        public const string FIELDS_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 包装台名称
        /// </summary>
        public const string FIELDS_EQUIPMENT_NAME = "EQUIPMENT_NAME";
        /// <summary>
        /// 线别ID
        /// </summary>
        public const string FIELDS_LINE_KEY = "LINE_KEY";
        /// <summary>
        /// 线别名称
        /// </summary>
        public const string FIELDS_LINE_NAME = "LINE_NAME";
        /// <summary>
        /// 变更托号（新托号）
        /// </summary>
        public const string FIELDS_PALLET_NO_NEW = "PALLET_NO_NEW";
        /// <summary>
        /// 托盘类别（0:有效托，1：整托出托，2：托号变更）
        /// </summary>
        public const string FIELDS_PALLET_TYPE = "PALLET_TYPE";
        /// <summary>
        /// 编码类别(0:普通，1：侧板，2：客户)
        /// </summary>
        public const string FIELDS_CODE_TYPE = "CODE_TYPE";
        /// <summary>
        /// 创建人(入库人)。
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间(入库时间)。
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 检验人(入库检)。
        /// </summary>
        public const string FIELDS_CHECKER = "CHECKER";
        /// <summary>
        /// 检验时间。
        /// </summary>
        public const string FIELDS_CHECK_TIME = "CHECK_TIME";
        /// <summary>
        /// 入库人(入库)。
        /// </summary>
        public const string FIELDS_TO_WH = "TO_WH";
        /// <summary>
        /// 入库时间。
        /// </summary>
        public const string FIELDS_TO_WH_TIME = "TO_WH_TIME";
        /// <summary>
        /// 出货人
        /// </summary>
        public const string FIELDS_OUT_WH = "OUT_WH";
        /// <summary>
        /// 出货时间
        /// </summary>
        public const string FIELDS_OUT_WH_TIME = "OUT_WH_TIME";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否有效(1:有效，0:无效)。
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
        /// <summary>
        /// 序号
        /// </summary>
        public const string FIELDS_SEQ = "SEQ";

        /// <summary>
        /// 备注
        /// </summary>
        public const string FIELDS_MEMO1 = "MEMO1";
        /// <summary>
        /// 入库单号
        /// </summary>
        public const string FIELDS_INWHORDER = "INWHORDER";
        /// <summary>
        /// 行号
        /// </summary>
        public const string FIELDS_ROWNUMBER = "ROWNUMBER";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_CONSIGNMENT";
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
                                                                  {FIELDS_CONSIGNMENT_KEY, new FieldProperties()},
                                                                  {FIELDS_PALLET_NO, new FieldProperties()},
                                                                  {FIELDS_VIRTUAL_PALLET_NO, new FieldProperties()},
                                                                  {FIELDS_CS_DATA_GROUP, new FieldProperties()},
                                                                  {FIELDS_SAP_NO,new FieldProperties() }, 
                                                                  {FIELDS_POWER_LEVEL,new FieldProperties()}, 
                                                                  {FIELDS_GRADE,new FieldProperties() },
                                                                  {FIELDS_PS_CODE,new FieldProperties() }, 
                                                                  {FIELDS_PS_SEQ,new FieldProperties()},
                                                                  {FIELDS_PS_DTL_SUBCODE,new FieldProperties() },
                                                                  {FIELDS_SHIFT,new FieldProperties()},
                                                                  {FIELDS_WORKNUMBER,new FieldProperties()},
                                                                  {FIELDS_EQUIPMENT_KEY,new FieldProperties()},
                                                                  {FIELDS_EQUIPMENT_NAME,new FieldProperties()},
                                                                  {FIELDS_LINE_KEY,new FieldProperties()},
                                                                  {FIELDS_LINE_NAME,new FieldProperties()},
                                                                  {FIELDS_PRO_ID,new FieldProperties()},
                                                                  {FIELDS_AVG_POWER,new FieldProperties()},
                                                                  {FIELDS_AVG_POWER_RANGE,new FieldProperties()},
                                                                  {FIELDS_CHECKER,new FieldProperties()},
                                                                  {FIELDS_CHECK_TIME,new FieldProperties()},
                                                                  {FIELDS_TO_WH,new FieldProperties()},
                                                                  {FIELDS_TO_WH_TIME,new FieldProperties()},
                                                                  {FIELDS_OUT_WH,new FieldProperties()},
                                                                  {FIELDS_OUT_WH_TIME,new FieldProperties()},
                                                                  {FIELDS_LOT_COLOR,new FieldProperties()},
                                                                  {FIELDS_TOTLE_POWER,new FieldProperties()},
                                                                  {FIELDS_CUSTOMER_NO,new FieldProperties()},
                                                                  {FIELDS_LOT_NUMBER_QTY,new FieldProperties()},
                                                                  {FIELDS_FULL_QTY,new FieldProperties()},
                                                                  {FIELDS_PALLET_NO_NEW,new FieldProperties()},
                                                                  {FIELDS_CODE_TYPE,new FieldProperties()},
                                                                  {FIELDS_PALLET_TYPE,new FieldProperties()},
                                                                  {FIELDS_LAST_PALLET,new FieldProperties()},
                                                                  {FIELDS_ROOM_KEY,new FieldProperties()},
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_EDITOR, new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_ISFLAG,new FieldProperties(typeof(Int16),true,1) },
                                                                  {FIELDS_MEMO1,new FieldProperties()},
                                                                  {FIELDS_INWHORDER,new FieldProperties()},
                                                                };
                                                                       
    }

    /// <summary>
    /// 表示组件包装表及其字段的实体类。
    /// </summary>
    public class WIP_CONSIGNMENT_TEMP_FIELDS : TableFields
    {
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELDS_CONSIGNMENT_KEY_TEMP = "CONSIGNMENT_KEY_TEMP";
        /// <summary>
        /// 主键。
        /// </summary>
        public const string FIELDS_CONSIGNMENT_KEY = "CONSIGNMENT_KEY";
        /// <summary>
        /// 托盘号。
        /// </summary>
        public const string FIELDS_PALLET_NO = "PALLET_NO";
        /// <summary>
        /// 批次条码
        /// </summary>
        public const string FIELDS_LOT_NUMBER = "LOT_NUMBER";
        /// <summary>
        /// 侧板编码
        /// </summary>
        public const string FIELDS_LOT_SIDECODE = "LOT_SIDECODE";
        /// <summary>
        /// 客户编码
        /// </summary>
        public const string FIELDS_LOT_CUSTOMERCODE = "LOT_CUSTOMERCODE";
        /// <summary>
        /// 车间ID
        /// </summary>
        public const string FIELDS_ROOM_KEY = "ROOM_KEY";
        /// <summary>
        /// 托盘类别（1：整托出托，2：托号变更）
        /// </summary>
        public const string FIELDS_PALLET_TYPE = "PALLET_TYPE";
        /// <summary>
        /// 创建人。
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_CONSIGNMENT_TEMP";
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
                                                                  {FIELDS_CONSIGNMENT_KEY, new FieldProperties()},
                                                                  {FIELDS_PALLET_NO, new FieldProperties()},
                                                                  {FIELDS_CONSIGNMENT_KEY_TEMP, new FieldProperties()},
                                                                  {FIELDS_LOT_NUMBER, new FieldProperties()},
                                                                  {FIELDS_LOT_SIDECODE,new FieldProperties() }, 
                                                                  {FIELDS_LOT_CUSTOMERCODE,new FieldProperties()}, 
                                                                  {FIELDS_PALLET_TYPE,new FieldProperties() },  
                                                                  {FIELDS_ROOM_KEY,new FieldProperties()},
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_EDITOR, new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)}                                                                                                                 
                                                                };

    }
}
