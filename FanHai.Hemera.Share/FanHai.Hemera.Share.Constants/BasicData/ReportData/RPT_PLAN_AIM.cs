
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class RPT_PLAN_AIM : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_PLANID = "PLANID";
        /// <summary>
        /// 计划输入开始时间
        /// </summary>
        public const string FIELDS_PLAN_DATE_START = "PLAN_DATE_START";
        /// <summary>
        /// 计划输入结束时间
        /// </summary>
        public const string FIELDS_PLAN_DATE_END = "PLAN_DATE_END";
        /// <summary>
        /// 班别（值）
        /// </summary>
        public const string FIELDS_SHIFT_VALUE = "SHIFT_VALUE";
        /// <summary>
        /// 班别名称
        /// </summary>
        public const string FIELDS_SHIFT_NAME = "SHIFT_NAME";
        /// <summary>
        /// 车间名称
        /// </summary>
        public const string FIELDS_LOCATION_NAME = "LOCATION_NAME";
        /// <summary>
        /// 产品型号
        /// </summary>
        public const string FIELDS_PART_TYPE = "PART_TYPE";
        /// <summary>
        /// 批次类型
        /// </summary>
        public const string FIELDS_LOT_TYPE = "LOT_TYPE";
        /// <summary>
        /// 投入数量
        /// </summary>
        public const string FIELDS_QUANTITY_INPUT = "QUANTITY_INPUT";
        /// <summary>
        /// 产出数量
        /// </summary>
        public const string FIELDS_QUANTITY_OUTPUT = "QUANTITY_OUTPUT";
        /// <summary>
        /// 产品ID号
        /// </summary>
        public const string FIELDS_PRO_ID = "PRO_ID";
        /// <summary>
        /// 工单号
        /// </summary>
        public const string FIELDS_WORK_ORDER_NO = "WORK_ORDER_NO";
        public const string FIELDS_GREATER_EFFI = "GREATER_EFFI";

        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATOR = "CREATOR";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 编辑人员
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 线别编号
        /// </summary>
        public const string FIELDS_LINE_CODE = "LINE_CODE";
        /// <summary>
        /// 线别名称
        /// </summary>
        public const string FIELDS_LINE_NAME = "LINE_NAME";
        /// <summary>
        /// For Query
        /// </summary>
        public const string FIELDS_CREATE_TIME_START = "CREATE_TIME_START";
        public const string FIELDS_CREATE_TIME_END = "CREATE_TIME_END";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "RPT_PLAN_AIM";
        public const string DATABASE_TABLE_FORUPDATE = "RPT_PLAN_AIM_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "RPT_PLAN_AIM_INSERT";


        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

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
                                                                  {FIELDS_PLANID, new FieldProperties() },
                                                                  {FIELDS_PLAN_DATE_START, new FieldProperties() },
                                                                  {FIELDS_PLAN_DATE_END, new FieldProperties() },
                                                                  {FIELDS_SHIFT_VALUE, new FieldProperties() },
                                                                  {FIELDS_SHIFT_NAME, new FieldProperties() },
                                                                  {FIELDS_LOCATION_NAME, new FieldProperties() },   
                                                                  {FIELDS_LINE_NAME, new FieldProperties() }, 
                                                                  {FIELDS_LINE_CODE, new FieldProperties() }, 
                                                                  {FIELDS_PART_TYPE, new FieldProperties() },
                                                                  {FIELDS_LOT_TYPE, new FieldProperties() },
                                                                  {FIELDS_QUANTITY_INPUT, new FieldProperties() },
                                                                  {FIELDS_QUANTITY_OUTPUT, new FieldProperties() },
                                                                  {FIELDS_PRO_ID, new FieldProperties() },
                                                                  {FIELDS_WORK_ORDER_NO, new FieldProperties() },
                                                                  {FIELDS_GREATER_EFFI, new FieldProperties() },
                                                                  {FIELDS_CREATOR, new FieldProperties() },                                                                       
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),false,DateTime.Now)  }, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),false,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  