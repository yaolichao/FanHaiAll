using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示预设暂停表（WIP_FUTUREHOLD）的字段类。
    /// </summary>
    public class WIP_FUTUREHOLD_FIELDS : TableFields
    {
        /// <summary>
        /// 主键。
        /// </summary>
        public const string FIELDS_ROW_KEY          ="ROW_KEY";
        /// <summary>
        /// 工单号。
        /// </summary>
        public const string FIELDS_WORKORDER_NUMBER ="WORKORDER_NUMBER";
        /// <summary>
        /// 批次号。
        /// </summary>
        public const string FIELDS_LOT_NUMBER       ="LOT_NUMBER";
        /// <summary>
        /// 暂停等级。默认为 1
        /// </summary>
        public const string FIELDS_HOLD_LEVEL       ="HOLD_LEVEL";
        /// <summary>
        /// 暂停密码。
        /// </summary>
        public const string FIELDS_HOLD_PASSWORD = "HOLD_PASSWORD";
        /// <summary>
        /// 暂停动作。
        /// </summary>
        public const string FIELDS_ACTION_NAME      ="ACTION_NAME";
        /// <summary>
        /// 状态。
        /// </summary>
        public const string FIELDS_STATUS           ="STATUS";
        /// <summary>
        /// 备注。
        /// </summary>
        public const string FIELDS_REMARK           ="REMARK";
        /// <summary>
        /// 暂停时的工步主键。
        /// </summary>
        public const string FIELDS_STEP_KEY         ="STEP_KEY";
        /// <summary>
        /// 暂停时的工艺流程主键。
        /// </summary>
        public const string FIELDS_ROUTE_KEY        ="ROUTE_KEY";
        /// <summary>
        /// 暂停时的工艺流程组主键。
        /// </summary>
        public const string FIELDS_ENTERPRISE_KEY   ="ENTERPRISE_KEY";
        /// <summary>
        /// 暂停时的工序名称。
        /// </summary>
        public const string FIELDS_OPERATION_NAME   = "OPERATION_NAME";
        /// <summary>
        /// 预设暂停的批次主键。
        /// </summary>
        public const string FIELDS_LOT_KEY          = "LOT_KEY";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELDS_EDITOR           = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIME        = "EDIT_TIME";
        /// <summary>
        /// 创建人。
        /// </summary>
        public const string FIELDS_CREATOR = "CREATOR";
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 删除标记 0：正常 1：已删除。
        /// </summary>
        public const string FIELDS_DELETE_FLAG = "DELETE_FLAG";
        /// <summary>
        /// 设置预设暂停时的工步主键。
        /// </summary>
        public const string FIELDS_SET_STEP_KEY     = "SET_STEP_KEY";
        /// <summary>
        /// 设置预设暂停时的工艺流程主键。
        /// </summary>
        public const string FIELDS_SET_ROUTE_KEY    = "SET_ROUTE_KEY";
        /// <summary>
        /// 设置预设暂停时的工艺流程组主键。
        /// </summary>
        public const string FIELDS_SET_ENTERPRISE_KEY        = "SET_ENTERPRISE_KEY";
        /// <summary>
        /// 暂停组别主键。
        /// </summary>
        public const string FIELDS_REASON_CODE_CATEGORY_KEY  = "REASON_CODE_CATEGORY_KEY";
        /// <summary>
        /// 暂停组别名称。
        /// </summary>
        public const string FIELDS_REASON_CODE_CATEGORY_NAME = "REASON_CODE_CATEGORY_NAME";
        /// <summary>
        /// 暂停代码主键。
        /// </summary>
        public const string FIELDS_REASON_CODE_KEY          = "REASON_CODE_KEY";
        /// <summary>
        /// 暂停代码名称。
        /// </summary>
        public const string FIELDS_REASON_CODE               = "REASON_CODE";
        /// <summary>
        /// 预设暂停的数据表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_FUTUREHOLD";
        /// <summary>
        /// 预设暂停的数据表名称。
        /// </summary>
        public override string TABLE_NAME
        {
           get
           {
               return DATABASE_TABLE_NAME;
           }
        }
        /// <summary>
        /// 预设暂停的数据字段字典。
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
                                                              {FIELDS_ROW_KEY , new FieldProperties() },        
                                                              {FIELDS_WORKORDER_NUMBER, new FieldProperties() },
                                                              {FIELDS_LOT_NUMBER , new FieldProperties() },     
                                                              {FIELDS_HOLD_LEVEL , new FieldProperties() },
                                                              {FIELDS_HOLD_PASSWORD , new FieldProperties() },
                                                              {FIELDS_ACTION_NAME , new FieldProperties() },    
                                                              {FIELDS_STATUS , new FieldProperties() },         
                                                              {FIELDS_REMARK , new FieldProperties() },         
                                                              {FIELDS_STEP_KEY , new FieldProperties() },       
                                                              {FIELDS_ROUTE_KEY  , new FieldProperties() },     
                                                              {FIELDS_ENTERPRISE_KEY  , new FieldProperties() },
                                                              {FIELDS_OPERATION_NAME , new FieldProperties() },
                                                              {FIELDS_LOT_KEY,new FieldProperties()},
                                                              {FIELDS_EDITOR,new FieldProperties()},
                                                              {FIELDS_EDIT_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                              {FIELDS_SET_ENTERPRISE_KEY,new FieldProperties()},
                                                              {FIELDS_SET_ROUTE_KEY,new FieldProperties()},
                                                              {FIELDS_SET_STEP_KEY,new FieldProperties()},
                                                              {FIELDS_REASON_CODE_CATEGORY_KEY,new FieldProperties()},
                                                              {FIELDS_REASON_CODE_CATEGORY_NAME,new FieldProperties()},
                                                              {FIELDS_REASON_CODE_KEY,new FieldProperties()},
                                                              {FIELDS_REASON_CODE , new FieldProperties() },
                                                              {FIELDS_CREATOR , new FieldProperties() },
                                                              {FIELDS_CREATE_TIME , new FieldProperties() },
                                                              {FIELDS_DELETE_FLAG , new FieldProperties() }
                                                            };
    }
}
