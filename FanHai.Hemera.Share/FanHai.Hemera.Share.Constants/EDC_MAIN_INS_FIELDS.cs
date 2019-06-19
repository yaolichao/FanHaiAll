using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库表（EDC_MAIN_INS）的信息。
    /// </summary>
    public class EDC_MAIN_INS_FIELDS:TableFields
    {
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_EDC_INS_KEY="EDC_INS_KEY";
        /// <summary>
        /// 管控项目组主键（EDC_MAIN）
        /// </summary>
        public const string FIELD_EDC_KEY	  ="EDC_KEY";
        /// <summary>
        /// 管控项目名称。
        /// </summary>
        public const string FIELD_EDC_NAME  ="EDC_NAME";
        /// <summary>
        /// 批次号。
        /// </summary>
        public const string FIELD_LOT_NUMBER  ="LOT_NUMBER";
        /// <summary>
        /// 工艺流程组主键。
        /// </summary>
        public const string FIELD_ENTERPRISE_KEY	  ="ENTERPRISE_KEY";
	    /// <summary>
	    /// 工艺流程主键。
	    /// </summary>
        public const string FIELD_ROUTE_KEY		  ="ROUTE_KEY";
	    /// <summary>
	    /// 工步主键。
	    /// </summary>
        public const string FIELD_STEP_KEY		  ="STEP_KEY";
        /// <summary>
        /// 工单主键。
        /// </summary>
        public const string FIELD_WORK_ORDER_KEY  ="WORK_ORDER_KEY";
        /// <summary>
        /// 成品主键。
        /// </summary>
        public const string FIELD_PART_KEY	  ="PART_KEY";
        /// <summary>
        /// 数据抽样规则主键。
        /// </summary>
        public const string FIELD_EDC_SP_KEY	  ="EDC_SP_KEY";
        /// <summary>
        /// 抽样数量。
        /// </summary>
        public const string FIELD_SP_COUNT = "SP_COUNT";
        /// <summary>
        /// 采集开始时间。
        /// </summary>
        public const string FIELD_COL_START_TIME = "COL_START_TIME";
        /// <summary>
        /// 采集结束时间。
        /// </summary>
        public const string FIELD_COL_END_TIME = "COL_END_TIME";
        /// <summary>
        /// 采集单位。
        /// </summary>
        public const string FIELD_SP_UNITS = "SP_UNITS";
        /// <summary>
        /// 批次主键。
        /// </summary>
        public const string FIELD_LOT_KEY = "LOT_KEY";
        /// <summary>
        /// 抽检点设置主键。
        /// </summary>
        public const string FIELD_EDC_POINT_KEY = "EDC_POINT_KEY";
        /// <summary>
        /// 设备主键。
        /// </summary>
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 物料批号。
        /// </summary>
        public const string FIELD_MATERIAL_LOT = "MATERIAL_LOT";
        /// <summary>
        /// 工步名称。
        /// </summary>
        public const string FIELD_STEP_NAME = "STEP_NAME";
        /// <summary>
        /// 供应商名称。
        /// </summary>
        public const string FIELD_SUPPLIER = "SUPPLIER";
        /// <summary>
        /// 成品料号。
        /// </summary>
        public const string FIELD_PART_NO = "PART_NO";
        /// <summary>
        /// 工厂车间主键。
        /// </summary>
        public const string FIELD_LOCATION_KEY = "LOCATION_KEY";
        /// <summary>
        /// 成品类型。
        /// </summary>
        public const string FIELD_PART_TYPE = "PART_TYPE";
        /// <summary>
        /// 创建人。
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 备注
        /// </summary>
        public const string FIELD_EDC_COMMENT= "EDC_COMMENT";
        /// <summary>
        /// 数据表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "EDC_MAIN_INS";
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
                                                                  {FIELD_EDC_INS_KEY, new FieldProperties() },
                                                                  {FIELD_EDC_KEY,new FieldProperties() },
                                                                  {FIELD_EDC_NAME,new FieldProperties() },
                                                                  {FIELD_LOT_NUMBER, new FieldProperties() },
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties() },
                                                                  {FIELD_ROUTE_KEY,		new FieldProperties() },
                                                                  {FIELD_STEP_KEY,		new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_KEY,new FieldProperties()},
                                                                  {FIELD_PART_KEY,new FieldProperties() },
                                                                  {FIELD_EDC_SP_KEY,new FieldProperties()  },
                                                                  {FIELD_SP_COUNT,new FieldProperties()},
                                                                  {FIELD_COL_START_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_COL_END_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_SP_UNITS ,new FieldProperties()},
                                                                  {FIELD_LOT_KEY ,new FieldProperties()},
                                                                  {FIELD_EDC_POINT_KEY,new FieldProperties()},
                                                                  {FIELD_EQUIPMENT_KEY,new FieldProperties()},
                                                                  {FIELD_MATERIAL_LOT,new FieldProperties()},
                                                                  {FIELD_SUPPLIER,new FieldProperties()},
                                                                  {FIELD_STEP_NAME,new FieldProperties()},
                                                                  {FIELD_PART_NO,new FieldProperties()},
                                                                  {FIELD_LOCATION_KEY,new FieldProperties()},
                                                                  {FIELD_CREATE_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_CREATOR,new FieldProperties()},
                                                                  {FIELD_EDITOR,new FieldProperties()},
                                                                  {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_EDC_COMMENT,new FieldProperties()}
                                                                }; 
    }
}
                                                                   
                                                                   