using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工序数据库表及其字段的实体类。
    /// </summary>
    public class POR_ROUTE_OPERATION_VER_FIELDS:TableFields
    {
        /// <summary>
        /// 工序主键。
        /// </summary>
        public const string FIELD_ROUTE_OPERATION_VER_KEY        = "ROUTE_OPERATION_VER_KEY";                   //工序ID
        /// <summary>
        /// 工序名称。
        /// </summary>
        public const string FIELD_ROUTE_OPERATION_NAME = "ROUTE_OPERATION_NAME";			            //工序名称
        /// <summary>
        /// 版本号。
        /// </summary>
        public const string FIELD_OPERATION_VERSION = "OPERATION_VERSION";			                //版本
        /// <summary>
        /// 时长。
        /// </summary>
        public const string FIELD_DURATION = "DURATION";				                    //工序标准时长
        /// <summary>
        /// 工序描述。
        /// </summary>
        public const string FIELD_DESCRIPTIONS				     = "DESCRIPTIONS";				                //工序描述
        /// <summary>
        /// 报废代码组主键。
        /// </summary>
        public const string FIELD_SCRAP_REASON_CODE_CATEGORY_KEY = "SCRAP_REASON_CODE_CATEGORY_KEY";	        //报废列表ID
        /// <summary>
        /// 不良代码组主键。
        /// </summary>
        public const string FIELD_DEFECT_REASON_CODE_CATEGORY_KEY = "DEFECT_REASON_CODE_CATEGORY_KEY";	                        //不良列表ID
        /// <summary>
        /// 继承自的工序版本。
        /// </summary>
        public const string FIELD_OPERATION_FROM_VERSION = "OPERATION_FROM_VERSION";			        //从那个Version继承过来
        /// <summary>
        /// 工序状态，1 = 激活, 0 = 未激活, 2 = 存档
        /// </summary>
        public const string FIELD_OPERATION_STATUS = "OPERATION_STATUS";				            //状态：1 = Active, 0 = InActive, 2 = Archive
        /// <summary>
        /// 排序序号。
        /// </summary>
        public const string FIELD_SORT_SEQ = "SORT_SEQ";
        /// <summary>
        /// 参数排列顺序 0：先行后列 1：先列后行
        /// </summary>
        public const string FIELD_PARAM_ORDER_TYPE = "PARAM_ORDER_TYPE";
        /// <summary>
        /// 每行参数个数。
        /// </summary>
        public const string FIELD_PARAM_COUNT_PER_ROW = "PARAM_COUNT_PER_ROW";
        /// <summary>
        /// 创建者。
        /// </summary>
        public const string FIELD_CREATOR				         = "CREATOR";				                    //创建者
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELD_CREATE_TIME				     = "CREATE_TIME";				                //创建时间
        /// <summary>
        /// 创建时间时区。
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE                = "CREATE_TIMEZONE";				            //创建时间时区
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR                         = "EDITOR";				                    //编辑者
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME                      = "EDIT_TIME";				                    //编辑时间
        /// <summary>
        /// 编辑时间时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE                  = "EDIT_TIMEZONE";				                //编辑时间时区
        /// <summary>
        /// （不用)
        /// </summary>
        public const string FIELD_IMAGE_KEY = "IMAGE_KEY";				                    //工序图像ID
        /// <summary>
        /// （不用)
        /// </summary>
        public const string FIELD_INSTRUCTION_LIST_KEY = "INSTRUCTION_LIST_KEY";		                //该工序标准操作流程集合ID
        /// <summary>
        /// （不用)
        /// </summary>
        public const string FIELD_EDC_LIST_KEY = "EDC_LIST_KEY";				                //数据收集集合ID
        /// <summary>
        /// （不用)
        /// </summary>
        public const string FIELD_CHECKLIST_KEY = "CHECKLIST_KEY";				                //checkList ID
        /// <summary>
        /// （不用)
        /// </summary>
        public const string FIELD_CHECKOUT_USER_KEY = "CHECKOUT_USER_KEY";			                //正在编辑该记录的编辑者ID
        /// <summary>
        /// （不用)
        /// </summary>
        public const string FIELD_SITE_NUMBER = "SITE_NUMBER";				                //站点
        /// <summary>
        /// (不用)，是否可以返工。
        /// </summary>
        public const string FIELD_IS_REWORKABLE				     = "IS_REWORKABLE";				                //是否可以返工
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_RE_ROUTE_ENTERPRISE_VER_KEY = "RE_ROUTE_ENTERPRISE_VER_KEY";	        	//返工集团途程ID
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_RE_START_ROUTE_VER_KEY		 = "RE_START_ROUTE_VER_KEY";			        //返工开始途程ID
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_RE_START_STEP_KEY			     = "RE_START_STEP_KEY";			                //返工开始工序ID
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_RE_END_ROUTE_VER_KEY = "RE_END_ROUTE_VER_KEY";			            //返工结束途程ID
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_RE_END_STEP_KEY			     = "RE_END_STEP_KEY";			                //返工结束工序ID
        /// <summary>
        /// (不用)
        /// </summary>
        public const string FIELD_IS_AUTO_GOBACK			     = "IS_AUTO_GOBACK";			                //返工结束，是否自动回到当前途程
        /// <summary>
        /// (暂时不用)
        /// </summary>
        public const string FIELD_INSTRUCTION_LIST_NAME			 = "INSTRUCTION_LIST_NAME";			            //(暂时不用)
        /// <summary>
        /// (暂时不用)
        /// </summary>
        public const string FIELD_EDC_LIST_NAME				     = "EDC_LIST_NAME";				                //(暂时不用)
        /// <summary>
        /// (暂时不用)
        /// </summary>
        public const string FIELD_CHECKLIST_NAME				 = "CHECKLIST_NAME";				            //(暂时不用)
        /// <summary>
        /// (暂时不用)
        /// </summary>
        public const string FIELD_SAMPLING_KEY                   = "SAMPLING_KEY";				                //关联到EDC_SP表的主键
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_OPERATION_VER";       //TABLE NAME
        

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

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_ROUTE_OPERATION_VER_KEY, new FieldProperties()},   
                                                                  {FIELD_ROUTE_OPERATION_NAME, new FieldProperties(typeof(string), true, "")},
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties()},
                                                                  {FIELD_DURATION, new FieldProperties()},
                                                                  {FIELD_OPERATION_VERSION, new FieldProperties()},	
                                                                  {FIELD_OPERATION_FROM_VERSION, new FieldProperties()},
                                                                  {FIELD_PARAM_ORDER_TYPE, new FieldProperties(typeof(decimal),true,0)},
                                                                  {FIELD_PARAM_COUNT_PER_ROW, new FieldProperties(typeof(decimal),true,2)},
	                                                              {FIELD_SCRAP_REASON_CODE_CATEGORY_KEY, new FieldProperties()},
                                                                  {FIELD_DEFECT_REASON_CODE_CATEGORY_KEY, new FieldProperties()},
                                                                  {FIELD_OPERATION_STATUS, new FieldProperties(typeof(decimal),true,0)},
                                                                  {FIELD_SORT_SEQ, new FieldProperties(typeof(decimal),true,900)},
                                                                  {FIELD_CREATOR, new FieldProperties()	},
                                                                  {FIELD_CREATE_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                  {FIELD_CREATE_TIMEZONE, new FieldProperties()},
                                                                  {FIELD_EDITOR, new FieldProperties()},         
                                                                  {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},    
                                                                  {FIELD_EDIT_TIMEZONE, new FieldProperties()},
                                                                   //-------------------------不用字段列表-----------------------
                                                                  {FIELD_SITE_NUMBER, new FieldProperties()},
                                                                  {FIELD_IMAGE_KEY, new FieldProperties()},				         
                                                                  {FIELD_INSTRUCTION_LIST_KEY, new FieldProperties()},		     
                                                                  {FIELD_EDC_LIST_KEY, new FieldProperties()},				     
                                                                  {FIELD_CHECKLIST_KEY, new FieldProperties()},			
                                                                  {FIELD_CHECKOUT_USER_KEY, new FieldProperties()},	
                                                                  {FIELD_IS_REWORKABLE, new FieldProperties()},				     
                                                                  {FIELD_RE_START_ROUTE_VER_KEY, new FieldProperties()},		
                                                                  {FIELD_RE_START_STEP_KEY, new FieldProperties()},			
                                                                  {FIELD_RE_END_STEP_KEY, new FieldProperties()},			
                                                                  {FIELD_IS_AUTO_GOBACK, new FieldProperties()},			     
                                                                  {FIELD_RE_END_ROUTE_VER_KEY, new FieldProperties()	},		
                                                                  {FIELD_RE_ROUTE_ENTERPRISE_VER_KEY, new FieldProperties()},
                                                                  {FIELD_INSTRUCTION_LIST_NAME, new FieldProperties()},			
                                                                  {FIELD_EDC_LIST_NAME, new FieldProperties()},				     
                                                                  {FIELD_CHECKLIST_NAME, new FieldProperties()},		
                                                                  {FIELD_SAMPLING_KEY, new FieldProperties()	}
                                                                  //-------------------------不用字段列表-----------------------
                                                              };  
    }                                                              		     
}                                                                	         
                                                                   		     
    