using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_TESTRULE_AVGPOWER : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_AVG_POWER_KEY = "AVG_POWER_KEY";
        /// <summary>
        /// 主键值（测试规则主表ID）
        /// </summary>
        public const string FIELDS_TESTRULE_KEY = "TESTRULE_KEY";
        /// <summary>
        /// 分档主键值
        /// </summary>
        public const string FIELDS_POWERSET_KEY = "POWERSET_KEY";
        /// <summary>
        /// 功率类型
        /// </summary>
        public const string FIELDS_PS_CODE = "PS_CODE";
        /// <summary>
        /// 功率序号
        /// </summary>
        public const string FIELDS_PS_SEQ = "PS_SEQ";
        /// <summary>
        /// 最小功率
        /// </summary>
        public const string FIELDS_AVGPOWER_MIN = "AVGPOWER_MIN";
        /// <summary>
        /// 最大功率
        /// </summary>
        public const string FIELDS_AVGPOWER_MAX = "AVGPOWER_MAX";     
        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_TESTRULE_AVGPOWER";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_TESTRULE_AVGPOWER_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_TESTRULE_AVGPOWER_INSERT";
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
                                                                  {FIELDS_AVG_POWER_KEY, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_KEY, new FieldProperties() },
                                                                  {FIELDS_POWERSET_KEY,new FieldProperties()},
                                                                  {FIELDS_PS_CODE, new FieldProperties() },
                                                                  {FIELDS_PS_SEQ, new FieldProperties() },
                                                                  {FIELDS_AVGPOWER_MIN, new FieldProperties() },
                                                                  {FIELDS_AVGPOWER_MAX, new FieldProperties() },                                                                                                           
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME,  new FieldProperties(typeof(DateTime),true,DateTime.Now) }, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  