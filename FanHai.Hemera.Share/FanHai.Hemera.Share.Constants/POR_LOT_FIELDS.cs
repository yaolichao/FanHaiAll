using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库表POR_LOT（批次信息）及其字段的实体类。
    /// </summary>
    public class POR_LOT_FIELDS : TableFields
    {
        /// <summary>
        /// 批次ID
        /// </summary>
        public const string FIELD_LOT_KEY = "LOT_KEY";                                      //批次ID
        /// <summary>
        /// 小包条码 add by yongbing.yang 20130808
        /// </summary>
        public const string FIELD_SMALL_PACK_NUMBER = "SMALL_PACK_NUMBER";                  //小包条码
        /// <summary>
        /// 批次编码
        /// </summary>
        public const string FIELD_LOT_NUMBER = "LOT_NUMBER";                                //批次编码
        /// <summary>
        /// 原工单号。
        /// </summary>
        public const string FIELD_ORG_WORK_ORDER_NO = "ORG_WORK_ORDER_NO";
        /// <summary>
        /// 工单主键。
        /// </summary>
        public const string FIELD_WORK_ORDER_KEY = "WORK_ORDER_KEY";                        //工单ID
        /// <summary>
        /// 工单号。
        /// </summary>
        public const string FIELD_WORK_ORDER_NO = "WORK_ORDER_NO";
        /// <summary>
        /// 新工单号。
        /// </summary>
        public const string FIELD_NEW_WORK_ORDER_NO = "NEW_WORK_ORDER_NO";
        /// <summary>
        /// 当前批次在WorkOrder的序列号
        /// </summary>
        public const string FIELD_WORK_ORDER_SEQ = "WORK_ORDER_SEQ";                //当前批次在WorkOrder的序列号
        /// <summary>
        /// 成品主键。
        /// </summary>
        public const string FIELD_PART_VER_KEY = "PART_VER_KEY";
        /// <summary>
        /// 成品号。
        /// </summary>
        public const string FIELD_PART_NUMBER = "PART_NUMBER";
        /// <summary>
        /// 绑定线别主键。
        /// </summary>
        public const string FIELD_LOT_LINE_KEY = "LOT_LINE_KEY";
        /// <summary>
        /// 绑定线别代码。
        /// </summary>
        public const string FIELD_LOT_LINE_CODE = "LOT_LINE_CODE";
        /// <summary>
        /// 线别切换动作。
        /// </summary>
        public const string FIELD_LOT_LINE_EXCHANGE = "CHANGE_LINE";
        /// <summary>
        /// 产品ID号
        /// </summary>
        public const string FIELD_PRO_ID = "PRO_ID";
        /// <summary>
        /// 产品等级。
        /// </summary>
        public const string FIELD_PRO_LEVEL = "PRO_LEVEL";
        /// <summary>
        /// 优先级，最高 1 -> 10 最低，默认 5
        /// </summary>
        public const string FIELD_PRIORITY = "PRIORITY";                                    //优先级，最高 1 -> 10 最低，默认 5
        /// <summary>
        /// 创建批次时数量。
        /// </summary>
        public const string FIELD_QUANTITY_INITIAL = "QUANTITY_INITIAL";
        /// <summary>
        /// 批次当前数量。
        /// </summary>
        public const string FIELD_QUANTITY = "QUANTITY";
        /// <summary>
        /// 工艺流程组主键
        /// </summary>
        public const string FIELD_ROUTE_ENTERPRISE_VER_KEY = "ROUTE_ENTERPRISE_VER_KEY";    //集团途程版本ID
        /// <summary>
        /// 当前工艺流程主键。
        /// </summary>
        public const string FIELD_CUR_ROUTE_VER_KEY = "CUR_ROUTE_VER_KEY";                  //当前途程版本ID
        /// <summary>
        /// 当前工步主键。
        /// </summary>
        public const string FIELD_CUR_STEP_VER_KEY = "CUR_STEP_VER_KEY";                    //当前工序版本ID
        /// <summary>
        /// 当前产线主键
        /// </summary>
        public const string FIELD_CUR_PRODUCTION_LINE_KEY = "CUR_PRODUCTION_LINE_KEY";      //当前产线ID
        /// <summary>
        /// 线别名称
        /// </summary>
        public const string FIELD_LINE_NAME = "LINE_NAME";                                  //线别名称
        /// <summary>
        /// 等待进站时间。
        /// </summary>
        public const string FIELD_START_WAIT_TIME = "START_WAIT_TIME";			        //开始等待进站的时间
        /// <summary>
        /// 开始处理时间。
        /// </summary>
        public const string FIELD_START_PROCESS_TIME = "START_PROCESS_TIME";				//开始进站的时间
        /// <summary>
        /// 当前数据采集主键，用于关联EDC_MAIN_INS表
        /// </summary>
        public const string FIELD_EDC_INS_KEY = "EDC_INS_KEY";
        /// <summary>
        /// 批次当前状态。0：等待进站,4：等待数据采集,5：出站数据采集中,9：等待出站,10：已完成 11：已入库
        /// </summary>
        public const string FIELD_STATE_FLAG = "STATE_FLAG";
        /// <summary>
        /// 是否是主批次，0：否,1：是
        /// </summary>
        public const string FIELD_IS_MAIN_LOT = "IS_MAIN_LOT";                              //0:否,1:是
        /// <summary>
        /// 拆批标记 0：否,1：是
        /// </summary>
        public const string FIELD_IS_SPLITED = "SPLIT_FLAG";                                //分批标记
        /// <summary>
        /// 拆批的序号。
        /// </summary>
        public const string FIELD_LOT_SEQ = "LOT_SEQ";
        /// <summary>
        /// 返工标记 0:否,1:是
        /// </summary>
        public const string FIELD_IS_REWORKED = "REWORK_FLAG";                              //返工标记
        /// <summary>
        /// 锁定标记，0:否,1:是
        /// </summary>
        public const string FIELD_HOLD_FLAG = "HOLD_FLAG";                            //0:否,1:是
        /// <summary>
        /// 出货标记。0:否,1:是
        /// </summary>
        public const string FIELD_SHIPPED_FLAG = "SHIPPED_FLAG";
        /// <summary>
        /// 删除标记。0：正常,1：已结束 2：已删除
        /// </summary>
        public const string FIELD_DELETED_TERM_FLAG = "DELETED_TERM_FLAG";                  //删除标记
        /// <summary>
        /// 打印标签标记。 0：否 >0:打印次数
        /// </summary>
        public const string FIELD_IS_PRINT = "IS_PRINT";
        /// <summary>
        /// 批次类型。N:生产批次 L:组件补片批次
        /// </summary>
        public const string FIELD_LOT_TYPE = "LOT_TYPE";
        /// <summary>
        /// 批次创建类别。
        /// </summary>
        public const string FIELD_CREATE_TYPE = "CREATE_TYPE";
        /// <summary>
        /// 托盘号。
        /// </summary>
        public const string FIELD_PALLET_NO = "PALLET_NO";
        /// <summary>
        /// 包装时间。
        /// </summary>
        public const string FIELD_PALLET_TIME = "PALLET_TIME";
        /// <summary>
        /// 批次状态：1 = Active, 0 = InActive, 2 = Archive
        /// </summary>
        public const string FIELD_STATUS = "STATUS";                                        //批次状态：1 = Active, 0 = InActive, 2 = Archive 
        /// <summary>
        /// 操作人。
        /// </summary>
        public const string FIELD_OPERATOR = "OPERATOR";
        /// <summary>
        /// 操作线别。
        /// </summary>
        public const string FIELD_OPR_LINE = "OPR_LINE";
        /// <summary>
        /// 子线别。
        /// </summary>
        public const string FIELD_CHILD_LINE = "CHILD_LINE";
        /// <summary>
        /// 操作上一线别。
        /// </summary>
        public const string FIELD_OPR_LINE_PRE = "OPR_LINE_PRE";
        /// <summary>
        /// 操作计算机名称。
        /// </summary>
        public const string FIELD_OPR_COMPUTER = "OPR_COMPUTER";
        /// <summary>
        /// 物料编码。
        /// </summary>
        public const string FIELD_MATERIAL_CODE = "MATERIAL_CODE";
        /// <summary>
        /// 物料批号。
        /// </summary>
        public const string FIELD_MATERIAL_LOT = "MATERIAL_LOT";                            //物料号
        /// <summary>
        /// 供应商名称。
        /// </summary>
        public const string FIELD_SUPPLIER_NAME = "SUPPLIER_NAME";
        /// <summary>
        /// 转换效率。
        /// </summary>
        public const string FIELD_EFFICIENCY = "EFFICIENCY";
        /// <summary>
        /// 侧板编码
        /// </summary>
        public const string FIELD_LOT_SIDECODE = "LOT_SIDECODE";
        /// <summary>
        /// 客户编码
        /// </summary>
        public const string FIELD_LOT_CUSTOMERCODE = "LOT_CUSTOMERCODE";
        /// <summary>
        /// 创建批次的工序名称。
        /// </summary>
        public const string FIELD_CREATE_OPERTION_NAME = "CREATE_OPERTION_NAME";
        /// <summary>
        /// 工厂车间主键。
        /// </summary>
        public const string FIELD_FACTORYROOM_KEY = "FACTORYROOM_KEY";                      //工厂车间KEY
        /// <summary>
        /// 工厂车间名称。
        /// </summary>
        public const string FIELD_FACTORYROOM_NAME = "FACTORYROOM_NAME";                    //工厂车间
        /// <summary>
        /// 硅片批次。
        /// </summary>
        public const string FIELD_SI_LOT = "SI_LOT";                                        //硅片批次
        /// <summary>
        /// 颜色。
        /// </summary>
        public const string FIELD_COLOR = "COLOR";
        /// <summary>
        /// 创建者
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";                                      //创建者
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";                              //创建时间
        /// <summary>
        /// 创建时间时区
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE_KEY = "CREATE_TIMEZONE_KEY";              //创建时间时区
        /// <summary>
        /// 编辑者
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";                                        //编辑者
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";                                  //编辑时间
        /// <summary>
        /// 编辑时间时区
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";                  //编辑时间时区
        /// <summary>
        /// 班别名称
        /// </summary>
        public const string FIELD_SHIFT_NAME = "SHIFT_NAME";                                //班别名称
        /// <summary>
        /// 备注
        /// </summary>
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";                            //备注
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_LOT";                                //TABLE NAME

        public const string DATABASE_TABLE_NAME_FORUPDATE = "POR_LOT_UPDATE";  

        /// <summary>
        /// 获取数据库表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 获取数据库表字段集合。
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
                                                                  {FIELD_LOT_KEY , new FieldProperties() },                     //批次ID
                                                                  {FIELD_LOT_NUMBER , new FieldProperties() },                  //批次编码
                                                                  {FIELD_WORK_ORDER_KEY , new FieldProperties() },              //工单主键
                                                                  {FIELD_WORK_ORDER_NO,new FieldProperties()},
                                                                  {FIELD_NEW_WORK_ORDER_NO,new FieldProperties()},              //新工单号
                                                                  {FIELD_WORK_ORDER_SEQ , new FieldProperties() }, 
                                                                  {FIELD_PART_VER_KEY , new FieldProperties() },                //成品主键
                                                                  {FIELD_PART_NUMBER,new FieldProperties()},
                                                                  {FIELD_PRO_ID,new FieldProperties()},
                                                                  {FIELD_PRO_LEVEL,new FieldProperties()},
                                                                  {FIELD_PRIORITY , new FieldProperties() },                    //优先级，最高 1 -> 10 最低，默认 5
                                                                  {FIELD_QUANTITY_INITIAL,new FieldProperties()},
                                                                  {FIELD_QUANTITY  , new FieldProperties() },                     //数量
                                                                  {FIELD_ROUTE_ENTERPRISE_VER_KEY  , new FieldProperties() },   //集团途程版本ID
                                                                  {FIELD_CUR_ROUTE_VER_KEY , new FieldProperties() },           //当前途程版本ID
                                                                  {FIELD_CUR_STEP_VER_KEY , new FieldProperties() },            //当前工序版本ID
                                                                  {FIELD_CUR_PRODUCTION_LINE_KEY , new FieldProperties() },     //当前产线ID
                                                                  {FIELD_LINE_NAME  , new FieldProperties() }  ,                 //线别名称
                                                                  {FIELD_START_WAIT_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)	},
                                                                  {FIELD_START_PROCESS_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELD_EDC_INS_KEY, new FieldProperties() },  
                                                                  {FIELD_STATE_FLAG , new FieldProperties() },
                                                                  {FIELD_IS_MAIN_LOT  , new FieldProperties() },
                                                                  {FIELD_IS_SPLITED , new FieldProperties() }, 
                                                                  {FIELD_LOT_SEQ , new FieldProperties() },
                                                                  {FIELD_IS_REWORKED , new FieldProperties() },                 //是否曾经返工
                                                                  {FIELD_HOLD_FLAG , new FieldProperties() },
                                                                  {FIELD_SHIPPED_FLAG,new FieldProperties()},
                                                                  {FIELD_DELETED_TERM_FLAG,new FieldProperties()},
                                                                  {FIELD_IS_PRINT,new FieldProperties()},
                                                                  {FIELD_LOT_TYPE,new FieldProperties()},
                                                                  {FIELD_CREATE_TYPE,new FieldProperties()},
                                                                  {FIELD_PALLET_NO,new FieldProperties()},
                                                                  {FIELD_LOT_SIDECODE,new FieldProperties()},
                                                                  {FIELD_LOT_CUSTOMERCODE,new FieldProperties()},
                                                                  {FIELD_PALLET_TIME,new FieldProperties()},
                                                                  {FIELD_STATUS  , new FieldProperties() }  ,
                                                                  {FIELD_OPERATOR,new FieldProperties()},
                                                                  {FIELD_OPR_LINE,new FieldProperties()},
                                                                  {FIELD_CHILD_LINE,new FieldProperties()},
                                                                  {FIELD_OPR_COMPUTER,new FieldProperties()},
                                                                  {FIELD_EFFICIENCY,new FieldProperties()},
                                                                  {FIELD_OPR_LINE_PRE,new FieldProperties()},
                                                                  {FIELD_MATERIAL_CODE,new FieldProperties()},                      //物料号
                                                                  {FIELD_MATERIAL_LOT,new FieldProperties()},                       //物料批号
                                                                  {FIELD_SUPPLIER_NAME,new FieldProperties()},                      //物料供应商名称
                                                                  {FIELD_SI_LOT,new FieldProperties()},
                                                                  {FIELD_CREATE_OPERTION_NAME,new FieldProperties()},               //创建批次时的工序名称
                                                                  {FIELD_FACTORYROOM_NAME,new FieldProperties()},                   //工厂车间
                                                                  {FIELD_FACTORYROOM_KEY,new FieldProperties()},                    //工厂车间KEY
                                                                  {FIELD_CREATOR , new FieldProperties() },                         //创建者
                                                                  {FIELD_CREATE_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) },                 //创建时间
                                                                  {FIELD_CREATE_TIMEZONE_KEY , new FieldProperties() },         //创建时间时区
                                                                  {FIELD_EDITOR , new FieldProperties() },                      //编辑者
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) }, 
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() },                //编辑时间时区
                                                                  {FIELD_SHIFT_NAME  , new FieldProperties() }  ,                //班别名称
                                                                  {FIELD_DESCRIPTIONS  , new FieldProperties() },                  //备注
                                                                  {FIELD_ORG_WORK_ORDER_NO , new FieldProperties()}
                                                              };
    }
}
