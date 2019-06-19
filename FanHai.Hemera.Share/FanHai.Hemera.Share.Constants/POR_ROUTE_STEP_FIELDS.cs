using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工步数据库表及其字段的实体类。
    /// </summary>
    public class POR_ROUTE_STEP_FIELDS:TableFields
    {
        /// <summary>
        /// 工步主键。
        /// </summary>
        public const string FIELD_ROUTE_STEP_KEY="ROUTE_STEP_KEY";
        /// <summary>
        /// 名称。
        /// </summary>
        public const string FIELD_ROUTE_STEP_NAME = "ROUTE_STEP_NAME";
        /// <summary>
        /// 版本号。
        /// </summary>
        public const string FIELD_STEP_VERSION = "STEP_VERSION";
        /// <summary>
        /// 描述。
        /// </summary>
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";
        /// <summary>
        /// 标准时长。
        /// </summary>
        public const string FIELD_DURATION = "DURATION";	                                           //	工序标准时长
        /// <summary>
        /// 状态：1 = Active, 0 = InActive, 2 = Archive
        /// </summary>
        public const string FIELD_STEP_STATUS = "STEP_STATUS";
        /// <summary>
        /// 工序版本。
        /// </summary>
        public const string FIELD_ROUTE_OPERATION_VER_KEY = "ROUTE_OPERATION_VER_KEY";
        /// <summary>
        /// 工序名称
        /// </summary>
        public const string FIELD_ROUTE_OPERATION_NAME = "ROUTE_OPERATION_NAME";
        /// <summary>
        /// 工步信息来自于的版本号。
        /// </summary>
        public const string FIELD_ROUTE_STEP_FROM_VER_KEY = "ROUTE_STEP_FROM_VER_KEY";
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_ROUTE_ROUTE_VER_KEY = "ROUTE_ROUTE_VER_KEY";
        /// <summary>
        /// 序号。
        /// </summary>
        public const string FIELD_ROUTE_STEP_SEQ = "ROUTE_STEP_SEQ";
        /// <summary>
        /// 参数排列顺序 0：先行后列 1：先列后行
        /// </summary>
        public const string FIELD_PARAM_ORDER_TYPE = "PARAM_ORDER_TYPE";
        /// <summary>
        /// 每行参数个数。
        /// </summary>
        public const string FIELD_PARAM_COUNT_PER_ROW = "PARAM_COUNT_PER_ROW";
        /// <summary>
        /// 报废原因代码类别主键。
        /// </summary>
        public const string FIELD_SCRAP_REASON_CODE_CATEGORY_KEY = "SCRAP_REASON_CODE_CATEGORY_KEY";
        /// <summary>
        /// 不良原因代码类别主键。
        /// </summary>
        public const string FIELD_DEFECT_REASON_CODE_CATEGORY_KEY = "DEFECT_REASON_CODE_CATEGORY_KEY";
        /// <summary>
        /// 创建人。
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";	                                           //	创建者
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";	                                   //	创建时间
        /// <summary>
        /// 创建时间时区。
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";	                           //	创建时间时区
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";				                                   //   编辑者
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";				                           //   编辑时间
        /// <summary>
        /// 编辑时间时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";				                   //   编辑时间时区
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_SAMPLING_KEY = "SAMPLING_KEY";	                                   //	关联到EDC_SP表的主键
        /// <summary>
        /// （不用）类型：开始、结束、标准工序
        /// </summary>
        public const string FIELD_STEP_TYPE="STEP_TYPE";	                                           //	类型：开始、结束、标准工序
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_CHECKLIST_KEY="CHECKLIST_KEY";	                                   //	checkList ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_CYCLE_DURATION="CYCLE_DURATION";	                               //	暂不用
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_CYCLE_DURATION_TYPE="CYCLE_DURATION_TYPE";	                       //   暂不用
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_DEPENDENCIES="DEPENDENCIES";	                                   //   暂不用
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_WORK_CENTER_ASSIGNED="WORK_CENTER_ASSIGNED";	                   //	暂不用
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_SITE_NUMBER="SITE_NUMBER";	                                       //	站点
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_PIXEL_X = "PIXEL_X";	                                           //	gui显示时X坐标
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_PIXEL_Y = "PIXEL_Y";                                               //	gui显示时Y坐标
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_INSTRUCTION_LIST_KEY="INSTRUCTION_LIST_KEY";	                   //	该工序标准操作流程集合ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_IMAGE_KEY="IMAGE_KEY";	                                           //	工序站点图像ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_EDC_LIST_KEY="EDC_LIST_KEY";	                                   //	数据收集集合ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_IS_REWORKABLE="IS_REWORKABLE";	                                   //	是否可以返工
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_RE_START_ROUTE_VER_KEY="RE_START_ROUTE_VER_KEY";	               //	返工进入途程ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_RE_START_STEP_KEY="RE_START_STEP_KEY";                             //	返工进入工序ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_RE_END_STEP_KEY="RE_END_STEP_KEY";	                               //	返工退出工序ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_IS_AUTO_GOBACK="IS_AUTO_GOBACK";	                               //	返工结束，是否自动回到当前途程
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_RE_END_ROUTE_VER_KEY="RE_END_ROUTE_VER_KEY";	                   //	返工退出工序ID
        /// <summary>
        /// （不用）
        /// </summary>
        public const string FIELD_RE_ROUTE_ENTERPRISE_VER_KEY="RE_ROUTE_ENTERPRISE_VER_Key";	       //	返工集团途程ID
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_STEP";
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
                                                                  {FIELD_ROUTE_STEP_KEY, new FieldProperties()},
                                                                  {FIELD_ROUTE_STEP_NAME, new FieldProperties()},
                                                                  {FIELD_STEP_VERSION, new FieldProperties()},	
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties()},	
                                                                  {FIELD_DURATION, new FieldProperties()},	   
                                                                  {FIELD_STEP_STATUS, new FieldProperties()},
                                                                  {FIELD_ROUTE_OPERATION_VER_KEY, new FieldProperties()},
                                                                  {FIELD_ROUTE_OPERATION_NAME, new FieldProperties()},
                                                                  {FIELD_ROUTE_STEP_FROM_VER_KEY, new FieldProperties()},
                                                                  {FIELD_ROUTE_ROUTE_VER_KEY, new FieldProperties()},
                                                                  {FIELD_ROUTE_STEP_SEQ, new FieldProperties()},
                                                                  {FIELD_PARAM_ORDER_TYPE, new FieldProperties(typeof(decimal),true,0)},
                                                                  {FIELD_PARAM_COUNT_PER_ROW, new FieldProperties(typeof(decimal),true,2)},
                                                                  {FIELD_DEFECT_REASON_CODE_CATEGORY_KEY, new FieldProperties()},
                                                                  {FIELD_SCRAP_REASON_CODE_CATEGORY_KEY, new FieldProperties()},
                                                                  {FIELD_CREATOR, new FieldProperties()	},  
                                                                  {FIELD_CREATE_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},  
                                                                  {FIELD_CREATE_TIMEZONE, new FieldProperties()},
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties()},
                                                                   //---------------暂时不用-------------------------
                                                                  {FIELD_STEP_TYPE, new FieldProperties()},       
                                                                  {FIELD_CHECKLIST_KEY, new FieldProperties()},	
                                                                  {FIELD_CYCLE_DURATION, new FieldProperties()},
                                                                  {FIELD_CYCLE_DURATION_TYPE, new FieldProperties()},
                                                                  {FIELD_DEPENDENCIES, new FieldProperties()},
                                                                  {FIELD_WORK_CENTER_ASSIGNED, new FieldProperties()},
                                                                  {FIELD_SITE_NUMBER, new FieldProperties()},
                                                                  {FIELD_PIXEL_X, new FieldProperties()	},	       
                                                                  {FIELD_PIXEL_Y, new FieldProperties()},
                                                                  {FIELD_INSTRUCTION_LIST_KEY, new FieldProperties()},
                                                                  {FIELD_IMAGE_KEY, new FieldProperties()},       
                                                                  {FIELD_EDC_LIST_KEY, new FieldProperties()},
                                                                  {FIELD_IS_REWORKABLE, new FieldProperties()},
                                                                  {FIELD_RE_START_ROUTE_VER_KEY, new FieldProperties()},
                                                                  {FIELD_RE_START_STEP_KEY, new FieldProperties()},
                                                                  {FIELD_RE_END_STEP_KEY, new FieldProperties()},
                                                                  {FIELD_IS_AUTO_GOBACK, new FieldProperties()},
                                                                  {FIELD_RE_END_ROUTE_VER_KEY, new FieldProperties()},
                                                                  {FIELD_RE_ROUTE_ENTERPRISE_VER_KEY, new FieldProperties()},
                                                                  {FIELD_SAMPLING_KEY, new FieldProperties()}
                                                                };                                                 
                                                                   
    }                                        
}                                                                  
                                                                   