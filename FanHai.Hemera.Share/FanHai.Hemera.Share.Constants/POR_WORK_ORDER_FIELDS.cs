using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工单（POR_WORK_ORDER）的数据表类。
    /// </summary>
    public class POR_WORK_ORDER_FIELDS : TableFields
    {
        /// <summary>
        /// 工单主键。
        /// </summary>
        public const string FIELD_WORK_ORDER_KEY    = "WORK_ORDER_KEY";
        /// <summary>
        /// 工单号码。
        /// </summary>
        public const string FIELD_ORDER_NUMBER      = "ORDER_NUMBER";
        /// <summary>
        /// 工单状态。
        /// </summary>
        public const string FIELD_ORDER_STATE       = "ORDER_STATE";
        /// <summary>
        /// 工单优先级
        /// </summary>
        public const string FIELD_ORDER_PRIORITY    = "ORDER_PRIORITY";
        /// <summary>
        /// 描述
        /// </summary>
        public const string FIELD_DESCRIPTIONS      = "DESCRIPTIONS";
        /// <summary>
        /// 注释
        /// </summary>
        public const string FIELD_COMMENTS          = "COMMENTS";
        /// <summary>
        /// 工单开始时间
        /// </summary>
        public const string FIELD_ENTERED_TIME      = "ENTERED_TIME";
        /// <summary>
        /// 担保结束时间
        /// </summary>
        public const string FIELD_PROMISED_TIME     = "PROMISED_TIME";
        /// <summary>
        /// 工单结束时间
        /// </summary>
        public const string FIELD_FINISHED_TIME     = "FINISHED_TIME";
        /// <summary>
        /// 工单发货时间
        /// </summary>
        public const string FIELD_SHIPPED_TIME      = "SHIPPED_TIME";
        /// <summary>
        /// 工单关闭时间
        /// </summary>
        public const string FIELD_CLOSED_TIME       = "CLOSED_TIME";
        /// <summary>
        /// 工单请求数量
        /// </summary>
        public const string FIELD_QUANTITY_ORDERED  = "QUANTITY_ORDERED";
        /// <summary>
        /// 工单在制数量
        /// </summary>
        public const string FIELD_QUANTITY_IN_PROGRESS  = "QUANTITY_IN_PROGRESS";
        /// <summary>
        /// 工单报废数量
        /// </summary>
        public const string FIELD_QUANTITY_SCRAPPED     = "QUANTITY_SCRAPPED";
        /// <summary>
        /// 工单完工数量
        /// </summary>
        public const string FIELD_QUANTITY_FINISHED     = "QUANTITY_FINISHED";
        /// <summary>
        /// 工单发货数量
        /// </summary>
        public const string FIELD_QUANTITY_SHIPPED      = "QUANTITY_SHIPPED";
        /// <summary>
        /// 工单关闭数量
        /// </summary>
        public const string FIELD_QUANTITY_CLOSED = "QUANTITY_CLOSED";
        /// <summary>
        /// 开始时间
        /// </summary>
        public const string FIELD_START_TIME = "START_TIME";
        /// <summary>
        /// 结束时间
        /// </summary>
        public const string FIELD_FINISH_TIME = "FINISH_TIME";
        /// <summary>
        /// 计划工艺流程主键。
        /// </summary>
        public const string FIELD_PLANNED_ROUTE_EP_VER_KEY = "PLANNED_ROUTE_EP_VER_KEY";
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public const string FIELD_PLANNED_START_TIME = "PLANNED_START_TIME";
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public const string FIELD_PLANNED_FINISH_TIME = "PLANNED_FINISH_TIME";
        /// <summary>
        /// 排程开始时间
        /// </summary>
        public const string FIELD_SCHEDULE_START_TIME = "SCHEDULE_START_TIME";
        /// <summary>
        /// 排程结束时间
        /// </summary>
        public const string FIELD_SCHEDULE_FINISH_TIME = "SCHEDULE_FINISH_TIME";
        /// <summary>
        /// 成品物料号
        /// </summary>
        public const string FIELD_PART_NUMBER = "PART_NUMBER";
        /// <summary>
        /// 产品版本
        /// </summary>
        public const string FIELD_PART_REVISION = "PART_REVISION";
        /// <summary>
        /// 工单关闭类型
        /// </summary>
        public const string FIELD_ORDER_CLOSE_TYPE = "ORDER_CLOSE_TYPE";
        /// <summary>
        /// 创建人。
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 创建时间时区
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时间时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        /// <summary>
        /// 下次生成批次的序号
        /// </summary>
        public const string FIELD_NEXT_SEQ = "NEXT_SEQ";
        /// <summary>
        /// 工单剩余数量
        /// </summary>
        public const string FIELD_QUANTITY_LEFT = "QUANTITY_LEFT";
        /// <summary>
        /// 类型
        /// </summary>
        public const string FIELD_TYPE = "TYPE";
        /// <summary>
        /// 规格
        /// </summary>
        public const string FIELD_MODULE = "MODULE";
        /// <summary>
        /// 供应商
        /// </summary>
        public const string FIELD_SUPPLIER = "SUPPLIER";
        /// <summary>
        /// 税收类型。
        /// </summary>
        public const string FIELD_REVENUE_TYPE = "REVENUE_TYPE";
        /// <summary>
        /// 成品料号主键。
        /// </summary>
        public const string FIELD_PART_KEY = "PART_KEY";
        /// <summary>
        /// 配料单号。
        /// </summary>
        public const string FIELD_REC_ORDER_NUMBER = "REC_ORDER_NUMBER";
        /// <summary>
        /// 返工数量。
        /// </summary>
        public const string FIELD_QUANTITY_REWORK = "QUANTITY_REWORK";
        /// <summary>
        /// 对应SAP线别名称。
        /// </summary>
        public const string FIELD_LINENAME        = "LINE_NAME";
        /// <summary>
        /// 丢失数量。
        /// </summary>
        public const string FIELD_QUANTITY_LOSS = "QUANTITY_LOSS";
        /// <summary>
        /// 关闭编辑人。
        /// </summary>
        public const string FIELD_CLOSED_EDITOR = "CLOSED_EDITOR";
        /// <summary>
        /// 工单类型。
        /// </summary>
        public const string FIELD_ORDER_TYPE = "ORDER_TYPE";
        /// <summary>
        /// 入库库位。
        /// </summary>
        public const string FIELD_STOCK_LOCATION = "STOCK_LOCATION";
        /// <summary>
        /// 车间名称。
        /// </summary>
        public const string FIELD_FACTORY_NAME = "FACTORY_NAME";
        /// <summary>
        /// 产品ID号
        /// </summary>
        public const string FIELD_PRO_ID = "PRO_ID";
        /// <summary>
        /// 原始工单的加工中心
        /// </summary>
        public const string FIELD_OCRCODE = "OCRCODE";

        /// <summary>
        /// 数据表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_WORK_ORDER";

        public const string DATABASE_TABLE_FORUPDATE = "POR_WORK_ORDER_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WORK_ORDER_INSERT";
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
                                                                  {FIELD_WORK_ORDER_KEY, new FieldProperties() },
                                                                  {FIELD_ORDER_NUMBER, new FieldProperties() },
                                                                  {FIELD_ORDER_STATE, new FieldProperties() },
                                                                  {FIELD_ORDER_PRIORITY, new FieldProperties(typeof(int),true,1) },
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties() },
                                                                  {FIELD_COMMENTS, new FieldProperties() },         
                                                         
                                                                  {FIELD_ENTERED_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_PROMISED_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_FINISHED_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_SHIPPED_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_CLOSED_TIME, new FieldProperties(typeof(DateTime), true, null) },

                                                                  {FIELD_QUANTITY_ORDERED, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_QUANTITY_IN_PROGRESS, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_QUANTITY_SCRAPPED, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_QUANTITY_FINISHED, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_QUANTITY_SHIPPED, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_QUANTITY_CLOSED, new FieldProperties(typeof(double),true,0) },

                                                                  {FIELD_START_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_FINISH_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_PLANNED_ROUTE_EP_VER_KEY, new FieldProperties(typeof(int),true,null) },
                                                                  {FIELD_PLANNED_START_TIME, new FieldProperties(typeof(DateTime), true,null) },
                                                                  {FIELD_PLANNED_FINISH_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_SCHEDULE_START_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_SCHEDULE_FINISH_TIME, new FieldProperties(typeof(DateTime), true, null) },

                                                                  {FIELD_PART_NUMBER, new FieldProperties() },
                                                                  {FIELD_PART_REVISION, new FieldProperties() },

                                                                  {FIELD_ORDER_CLOSE_TYPE, new FieldProperties()},
                                                                  {FIELD_CREATOR, new FieldProperties() },
                                                                  {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, null)},
                                                                  {FIELD_CREATE_TIMEZONE, new FieldProperties() },
                                                                  {FIELD_EDITOR, new FieldProperties() },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, null) },
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties() },

                                                                  {FIELD_NEXT_SEQ,new FieldProperties()},
                                                                  {FIELD_QUANTITY_LEFT, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_TYPE,new FieldProperties()},
                                                                  {FIELD_MODULE,new FieldProperties()},
                                                                  {FIELD_SUPPLIER ,new FieldProperties()},
                                                                  {FIELD_REVENUE_TYPE,new FieldProperties()},
                                                                  {FIELD_PART_KEY,new FieldProperties()},
                                                                  {FIELD_REC_ORDER_NUMBER,new FieldProperties()},
                                                                  {FIELD_QUANTITY_REWORK, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_LINENAME,new FieldProperties()},
                                                                  {FIELD_QUANTITY_LOSS, new FieldProperties(typeof(double),true,0) },
                                                                  {FIELD_CLOSED_EDITOR,new FieldProperties()},

                                                                  {FIELD_ORDER_TYPE,new FieldProperties()},
                                                                  {FIELD_STOCK_LOCATION,new FieldProperties()},
                                                                  {FIELD_FACTORY_NAME,new FieldProperties()},
                                                                  {FIELD_PRO_ID,new FieldProperties()}
                                                                };
    }
}
