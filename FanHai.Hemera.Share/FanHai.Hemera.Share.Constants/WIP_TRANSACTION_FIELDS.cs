using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示批次操作的数据库表字段实体类。
    /// </summary>
    public class WIP_TRANSACTION_FIELDS:TableFields
    {
        /// <summary>
        /// 操作主键
        /// </summary>
        public const string FIELD_TRANSACTION_KEY     = "TRANSACTION_KEY";
        /// <summary>
        /// 对象类型：0
        /// </summary>
        public const string FIELD_PIECE_TYPE          = "PIECE_TYPE";
        /// <summary>
        /// 对象主键（批次主键）
        /// </summary>
        public const string FIELD_PIECE_KEY           = "PIECE_KEY";
        /// <summary>
        /// 操作动作名称。
        /// </summary>
        public const string FIELD_ACTIVITY            = "ACTIVITY";
        /// <summary>
        /// 操作前对象数量。
        /// </summary>
        public const string FIELD_QUANTITY_IN = "QUANTITY_IN";
        /// <summary>
        /// 操作后对象数量。
        /// </summary>
        public const string FIELD_QUANTITY_OUT = "QUANTITY_OUT";
        /// <summary>
        /// 工艺流程组主键。
        /// </summary>
        public const string FIELD_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        /// <summary>
        /// 工艺流程组名称
        /// </summary>
        public const string FIELD_ENTERPRISE_NAME = "ENTERPRISE_NAME";
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_ROUTE_KEY = "ROUTE_KEY";
        /// <summary>
        /// 工艺流程名。
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
        /// 工单主键。
        /// </summary>
        public const string FIELD_WORK_ORDER_KEY = "WORK_ORDER_KEY";
        /// <summary>
        /// 班别主键。
        /// </summary>
        public const string FIELD_SHIFT_KEY = "SHIFT_KEY";
        /// <summary>
        /// 班别名称。
        /// </summary>
        public const string FIELD_SHIFT_NAME = "SHIFT_NAME";
        /// <summary>
        /// 操作时对象状态。
        /// </summary>
        public const string FIELD_STATE_FLAG = "STATE_FLAG";
        /// <summary>
        /// 操作时对象重工标记。
        /// </summary>
        public const string FIELD_REWORK_FLAG = "REWORK_FLAG";
        /// <summary>
        /// 操作人。
        /// </summary>
        public const string FIELD_OPERATOR = "OPERATOR";
        /// <summary>
        /// 操作时的客户端计算机名称。
        /// </summary>
        public const string FIELD_OPR_COMPUTER = "OPR_COMPUTER";
        /// <summary>
        /// 操作时线别主键。
        /// </summary>
        public const string FIELD_LINE_KEY = "LINE_KEY";
        /// <summary>
        /// 操作线别名称。
        /// </summary>
        public const string FIELD_OPR_LINE = "OPR_LINE";
        /// <summary>
        /// 操作前一线别名称。
        /// </summary>
        public const string FIELD_OPR_LINE_PRE = "OPR_LINE_PRE";
        /// <summary>
        /// 子批次主键。
        /// </summary>
        public const string FIELD_CHILD_LOT_KEY = "CHILD_LOT_KEY";
        /// <summary>
        /// 数据采集主键。
        /// </summary>
        public const string FIELD_EDC_INS_KEY = "EDC_INS_KEY";
        /// <summary>
        /// 操作时设备主键。
        /// </summary>
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 下一工步主键。
        /// </summary>
        public const string FIELD_TO_STEP_KEY = "TO_STEP_KEY";
        /// <summary>
        /// 操作备注主键。
        /// </summary>
        public const string FIELD_COMMENT_KEY         = "COMMENT_KEY";
        /// <summary>
        /// 操作撤销标记。
        /// </summary>
        public const string FIELD_UNDO_FLAG = "UNDO_FLAG";
        /// <summary>
        /// 操作对应的操作撤销主键。
        /// </summary>
        public const string FIELD_UNDO_TRANSACTION_KEY = "UNDO_TRANSACTION_KEY";
        /// <summary>
        /// 舟号。
        /// </summary>
        public const string FILED_SHIPNUMBER          = "SHIP_NUMBER";
        /// <summary>
        /// 备注。
        /// </summary>
        public const string FIELD_ACTIVITY_COMMENT = "ACTIVITY_COMMENT";
        /// <summary>
        /// 报工标记。
        /// </summary>
        public const string FIELD_REPORT_FLAG = "REPORT_FLAG";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR              = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME           = "EDIT_TIME";
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE_KEY   = "EDIT_TIMEZONE_KEY";
        /// <summary>
        /// 操作创建时间。
        /// </summary>
        public const string FIELD_TIME_STAMP          = "TIME_STAMP";
        /// <summary>
        /// 批次操作数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME    = "WIP_TRANSACTION";
        /// <summary>
        /// 批次操作数据库表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
              return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 批次操作数据库表字段集合。
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
                                                                  {FIELD_TRANSACTION_KEY, new FieldProperties()     },
                                                                  {FIELD_PIECE_TYPE, new FieldProperties(typeof(int),false,0)},
                                                                  {FIELD_PIECE_KEY, new FieldProperties()           },
                                                                  {FIELD_ACTIVITY, new FieldProperties()            },
                                                                  {FIELD_QUANTITY_IN, new FieldProperties()         },
                                                                  {FIELD_QUANTITY_OUT, new FieldProperties()        },
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties()       },
                                                                  {FIELD_ENTERPRISE_NAME,new FieldProperties()      },
                                                                  {FIELD_ROUTE_KEY,new FieldProperties()            },
                                                                  {FIELD_ROUTE_NAME,new FieldProperties()           },
                                                                  {FIELD_STEP_KEY,new FieldProperties()             },
                                                                  {FIELD_STEP_NAME,new FieldProperties()            },
                                                                  {FIELD_WORK_ORDER_KEY, new FieldProperties()      },
                                                                  {FIELD_SHIFT_KEY, new FieldProperties()           },
                                                                  {FIELD_SHIFT_NAME, new FieldProperties()          },
                                                                  {FIELD_STATE_FLAG,new FieldProperties()           },
                                                                  {FIELD_REWORK_FLAG,new FieldProperties()          }, 
                                                                  {FIELD_OPERATOR,new FieldProperties()             },
                                                                  {FIELD_OPR_COMPUTER,new FieldProperties()         },
                                                                  {FIELD_LINE_KEY, new FieldProperties()            },
                                                                  {FIELD_OPR_LINE,new FieldProperties()             },
                                                                  {FIELD_OPR_LINE_PRE,new FieldProperties()         },
                                                                  {FIELD_CHILD_LOT_KEY,new FieldProperties()        },
                                                                  {FIELD_EDC_INS_KEY,new FieldProperties()          }, 
                                                                  {FIELD_EQUIPMENT_KEY,new FieldProperties()        },
                                                                  {FIELD_TO_STEP_KEY,new FieldProperties()          },
                                                                  {FIELD_COMMENT_KEY, new FieldProperties()         },
                                                                  {FIELD_UNDO_FLAG,new FieldProperties(typeof(int),true,0)            },
                                                                  {FIELD_UNDO_TRANSACTION_KEY,new FieldProperties() },
                                                                  {FILED_SHIPNUMBER,new FieldProperties()           },
                                                                  {FIELD_ACTIVITY_COMMENT, new FieldProperties()    },
                                                                  {FIELD_REPORT_FLAG, new FieldProperties(typeof(string),true,"N")    },
                                                                  {FIELD_EDITOR , new FieldProperties()             },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now) },
                                                                  {FIELD_EDIT_TIMEZONE_KEY, new FieldProperties()   },
                                                                  {FIELD_TIME_STAMP, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                };
    } 
}     
