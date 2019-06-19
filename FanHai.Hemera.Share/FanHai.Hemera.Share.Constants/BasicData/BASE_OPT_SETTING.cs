using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_OPT_SETTING : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_OPTSETTING_KEY = "OPTSETTING_KEY";
        /// <summary>
        /// 车间KEY
        /// </summary>
        public const string FIELDS_LOCATION_KEY = "LOCATION_KEY";
        /// <summary>
        /// 车间名称
        /// </summary>
        public const string FIELDS_LOCATION_NAME = "LOCATION_NAME";
        /// <summary>
        /// 工序KEY
        /// </summary>
        public const string FIELDS_OPERATION_KEY = "OPERATION_KEY";
        /// <summary>
        /// 工序名称
        /// </summary>
        public const string FIELDS_OPERATION_NAME = "OPERATION_NAME";
        /// <summary>
        /// 工序序号
        /// </summary>
        public const string FIELDS_SORT_SEQ = "SORT_SEQ";
        /// <summary>
        /// 班别主键
        /// </summary>
        public const string FIELDS_SHIFT_KEY = "SHIFT_KEY";
        /// <summary>
        /// 班别
        /// </summary>
        public const string FIELDS_SHIFT_NAME = "SHIFT_NAME";
        /// <summary>
        /// 开始时间
        /// </summary>
        public const string FIELDS_START_TIME = "START_TIME";
        /// <summary>
        /// 结束时间
        /// </summary>
        public const string FIELDS_END_TIME = "END_TIME";
        /// <summary>
        /// 是否跨天
        /// </summary>
        public const string FIELDS_OVER_DAY = "OVER_DAY";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 创建人员
        /// </summary>
        public const string FIELDS_CREATOR = "CREATOR";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑人员
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
        /// <summary>
        /// 备注
        /// </summary>
        public const string FIELDS_REMARK = "REMARK";

        /// <summary>
        /// For Query
        /// </summary>
        public const string FIELDS_CREATE_TIME_START = "CREATE_TIME_START";
        public const string FIELDS_CREATE_TIME_END = "CREATE_TIME_END";

       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_OPT_SETTING";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_OPT_SETTING_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_OPT_SETTING_INSERT";
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
                                                                  {FIELDS_OPTSETTING_KEY, new FieldProperties() },
                                                                  {FIELDS_LOCATION_KEY, new FieldProperties() },
                                                                  {FIELDS_LOCATION_NAME, new FieldProperties() },
                                                                  {FIELDS_OPERATION_KEY, new FieldProperties() },
                                                                  {FIELDS_OPERATION_NAME, new FieldProperties() },
                                                                  {FIELDS_SORT_SEQ, new FieldProperties()},
                                                                  {FIELDS_SHIFT_NAME, new FieldProperties() },
                                                                  {FIELDS_SHIFT_KEY, new FieldProperties()},
                                                                  {FIELDS_START_TIME, new FieldProperties() },
                                                                  {FIELDS_END_TIME, new FieldProperties() },
                                                                  {FIELDS_OVER_DAY, new FieldProperties() },
                                                                  {FIELDS_CREATOR, new FieldProperties() },
                                                                  {FIELDS_REMARK, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME,  new FieldProperties(typeof(DateTime),false,DateTime.Now)}, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),false,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  