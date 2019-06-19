using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class POR_PRODUCT : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_PRODUCT_KEY = "PRODUCT_KEY";
        /// <summary>
        /// 产品代码
        /// </summary>
        public const string FIELDS_PRODUCT_CODE = "PRODUCT_CODE";
        /// <summary>
        /// 产品说明
        /// </summary>
        public const string FIELDS_PRODUCT_NAME = "PRODUCT_NAME";
        /// <summary>
        /// 产品说明前缀
        /// </summary>
        public const string FIELDS_MODULE_TYPE_PREFIX = "MODULE_TYPE_PREFIX";
        /// <summary>
        /// 产品说明后缀
        /// </summary>
        public const string FIELDS_MODULE_TYPE_SUFFIX = "MODULE_TYPE_SUFFIX";
        /// <summary>
        /// 产品数量
        /// </summary>
        public const string FIELDS_QUANTITY = "QUANTITY";
        /// <summary>
        /// 最大功率
        /// </summary>
        public const string FIELDS_MAXPOWER = "MAXPOWER";
        /// <summary>
        /// 最小功率
        /// </summary>
        public const string FIELDS_MINPOWER = "MINPOWER";
        /// <summary>
        /// 测试规则
        /// </summary>
        public const string FIELDS_PRO_TEST_RULE = "PRO_TEST_RULE";
        /// <summary>
        /// 产品类型（表：BASE_PRODUCTMODEL）
        /// </summary>
        public const string FIELDS_PROMODEL_NAME = "PROMODEL_NAME";
        /// <summary>
        /// 生产规则编码
        /// </summary>
        public const string FIELDS_CODEMARK = "CODEMARK";
        /// <summary>
        /// 客户规则编码
        /// </summary>
        public const string FIELDS_CUSTMARK = "CUSTMARK";
        /// <summary>
        /// 认证类型
        /// </summary>
        public const string FIELDS_LABELTYPE = "LABELTYPE";
        /// <summary>
        /// 认证主键（认证类型+认证时间）
        /// </summary>
        public const string FIELDS_CERTIFICATION_KEY = "CERTIFICATION_KEY";
        /// <summary>
        /// 铭牌版本
        /// </summary>
        public const string FIELDS_LABELVAR = "LABELVAR";
        /// <summary>
        /// 是否校验铭牌
        /// </summary>
        public const string FIELDS_LABELCHECK = "LABELCHECK";
        /// <summary>
        /// 产品等级
        /// </summary>
        public const string FIELDS_PRO_LEVEL = "PRO_LEVEL";
        /// <summary>
        /// 满柜数量
        /// </summary>
        public const string FIELDS_SHIP_QTY = "SHIP_QTY";
        /// <summary>
        /// SAP料号
        /// </summary>
        public const string FIELDS_SAP_PN = "SAP_PN";
        /// <summary>
        /// 认证(比如：金太阳)
        /// </summary>
        public const string FIELDS_CERTIFICATION = "CERTIFICATION";
        /// <summary>
        /// 分档方式(比如：+~5W)
        /// </summary>
        public const string FIELDS_TOLERANCE = "TOLERANCE";
        /// <summary>
        /// 接线盒批号
        /// </summary>
        public const string FIELDS_JUNCTION_BOX = "JUNCTION_BOX";
        /// <summary>
        /// 校准版类型
        /// </summary>
        public const string FIELDS_CALIBRATION_TYPE = "CALIBRATION_TYPE";
        /// <summary>
        /// 校准周期
        /// </summary>
        public const string FIELDS_CALIBRATION_CYCLE = "CALIBRATION_CYCLE";
        /// <summary>
        /// 固化周期
        /// </summary>
        public const string FIELDS_FIX_CYCLE = "FIX_CYCLE";
        /// <summary>
        /// 恒温周期
        /// </summary>
        public const string FIELDS_CONSTANT_TEMPERTATURE_CYCLE = "CONSTANT_TEMPERTATURE_CYCLE";
        /// <summary>
        /// 工艺流程组主键
        /// </summary>
        public const string FIELDS_ROUTE_ENTERPRISE_VER_KEY = "ROUTE_ENTERPRISE_VER_KEY";
        /// <summary>
        /// 工艺流程组名称
        /// </summary>
        public const string FIELDS_ENTERPRISE_NAME = "ENTERPRISE_NAME";
        /// <summary>
        /// 工艺流程主键
        /// </summary>
        public const string FIELDS_ROUTE_ROUTE_VER_KEY = "ROUTE_ROUTE_VER_KEY";
        /// <summary>
        /// 工艺流程名称
        /// </summary>
        public const string FIELDS_ROUTE_NAME = "ROUTE_NAME";
        /// <summary>
        /// 工序主键
        /// </summary>
        public const string FIELDS_ROUTE_STEP_KEY = "ROUTE_STEP_KEY";
        /// <summary>
        /// 工序名称
        /// </summary>
        public const string FIELDS_ROUTE_STEP_NAME = "ROUTE_STEP_NAME";
        /// <summary>
        /// 组件大小
        /// </summary>
        public const string FIELDS_CELL_SIZE = "CELL_SIZE";
        /// <summary>
        /// 铭牌模板
        /// </summary>
        public const string FIELDS_NAME_TEMPLATE = "NAME_TEMPLATE";

        /// <summary>
        /// 备用字段（PS,PG）栏位
        /// </summary>
        public const string FIELDS_MEMO1 = "MEMO1";
        /// <summary>
        /// 是否是实验工单
        /// </summary>
        public const string FIELDS_ISEXPERIMENT = "ISEXPERIMENT";
        /// <summary>
        /// 是否金刚线
        /// </summary>
        public const string FIELDS_ISKingLine = "ISKINGLING";

        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_ISFLAG = "ISFLAG";

        public const string FIELDS_CREATE_TIME_START = "CREATE_TIME_START";
        public const string FIELDS_CREATE_TIME_END = "CREATE_TIME_END";

        public const string DATABASE_TABLE_NAME = "POR_PRODUCT";
        public const string DATABASE_TABLE_FORUPDATE = "POR_PRODUCT_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_PRODUCT_INSERT";
        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_PRODUCT_KEY, new FieldProperties() },
                                                                  {FIELDS_PRODUCT_CODE, new FieldProperties() },
                                                                  {FIELDS_PRODUCT_NAME, new FieldProperties() },
                                                                  {FIELDS_QUANTITY, new FieldProperties() },
                                                                  {FIELDS_MAXPOWER, new FieldProperties() },
                                                                  {FIELDS_MINPOWER, new FieldProperties() },
                                                                  {FIELDS_PRO_TEST_RULE, new FieldProperties() },                                                                    
                                                                  {FIELDS_PROMODEL_NAME, new FieldProperties() } ,
                                                                  {FIELDS_CODEMARK, new FieldProperties() },
                                                                  {FIELDS_CUSTMARK, new FieldProperties() },
                                                                  {FIELDS_LABELTYPE, new FieldProperties() },
                                                                  {FIELDS_LABELVAR, new FieldProperties() },
                                                                  {FIELDS_LABELCHECK, new FieldProperties() },
                                                                  {FIELDS_PRO_LEVEL, new FieldProperties() },
                                                                  {FIELDS_SHIP_QTY, new FieldProperties() }, 
                                                                  {FIELDS_SAP_PN, new FieldProperties() },
                                                                  {FIELDS_CERTIFICATION, new FieldProperties() },
                                                                  {FIELDS_TOLERANCE, new FieldProperties() },
                                                                  {FIELDS_JUNCTION_BOX, new FieldProperties() },
                                                                  {FIELDS_CALIBRATION_TYPE, new FieldProperties() },
                                                                  {FIELDS_CALIBRATION_CYCLE, new FieldProperties() },
                                                                  {FIELDS_MEMO1, new FieldProperties() },
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_ISEXPERIMENT, new FieldProperties(typeof(Int16),false,0) },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_ISFLAG,  new FieldProperties(typeof(Int16),false,1) } ,
                                                                  {FIELDS_ISKingLine, new FieldProperties() },
                                                                  {FIELDS_CELL_SIZE,new FieldProperties()},
                                                                  {FIELDS_CERTIFICATION_KEY, new FieldProperties() }
                                                                };
        #endregion
    }

    public class POR_PRODUCT_DTL : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_PRODUCT_DTL_KEY = "PRODUCT_DTL_KEY";
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_PRODUCT_KEY = "PRODUCT_KEY";
        /// <summary>
        /// 产品等级
        /// </summary>
        public const string FIELDS_PRODUCT_GRADE = "PRODUCT_GRADE";
        /// <summary>
        /// 产品等级名称
        /// </summary>
        public const string FIELDS_PRODUCT_NAME = "PRODUCT_NAME";
        /// <summary>
        /// SAP料号
        /// </summary>
        public const string FIELDS_SAP_PN = "SAP_PN";
        /// <summary>
        /// 是否客户要求等级
        /// </summary>
        public const string FIELDS_ISMAIN = "ISMAIN";
        /// <summary>
        /// 排序顺序
        /// </summary>
        public const string FIELDS_ORDER_INDEX = "ORDER_INDEX";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 编辑人
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
        public const string FIRLDS_ISKINGLING = "ISKINGLING";

        public const string DATABASE_TABLE_NAME = "POR_PRODUCT_DTL";
        public const string DATABASE_TABLE_FORUPDATE = "POR_PRODUCT_DTL_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_PRODUCT_DTL_INSERT";
        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_PRODUCT_DTL_KEY, new FieldProperties() },
                                                                  {FIELDS_PRODUCT_KEY, new FieldProperties() },
                                                                  {FIELDS_PRODUCT_GRADE, new FieldProperties() },
                                                                  {FIELDS_PRODUCT_NAME,new FieldProperties()},
                                                                  {FIELDS_SAP_PN,new FieldProperties()},
                                                                  {FIELDS_ISMAIN, new FieldProperties() },
                                                                  {FIELDS_ORDER_INDEX, new FieldProperties() },                                                                 
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_ISFLAG,  new FieldProperties(typeof(Int16),false,1)},
                                                                  {FIRLDS_ISKINGLING,  new FieldProperties(typeof(Int16),false,1) }  

                                                                };
        #endregion
    }
}

