using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_TESTRULE : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_TESTRULE_KEY = "TESTRULE_KEY";
        /// <summary>
        /// 测试规则代码
        /// </summary>
        public const string FIELDS_TESTRULE_CODE = "TESTRULE_CODE";
        /// <summary>
        /// 测试规则名称
        /// </summary>
        public const string FIELDS_TESTRULE_NAME = "TESTRULE_NAME";
        /// <summary>
        /// 备注
        /// </summary>
        public const string FIELDS_MEMO = "MEMO";
        /// <summary>
        /// 功率分档（分档设置表:BASE_POWERSET）
        /// </summary>
        public const string FIELDS_PS_CODE = "PS_CODE";
        /// <summary>
        /// 终检类型
        /// </summary>
        public const string FIELDS_LAST_TEST_TYPE = "LAST_TEST_TYPE";
        /// <summary>
        /// 功率精度
        /// </summary>
        public const string FIELDS_POWER_DEGREE = "POWER_DEGREE";
        /// <summary>
        /// 满托数
        /// </summary>
        public const string FIELDS_FULL_PALLET_QTY = "FULL_PALLET_QTY";
        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
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

        public const string DATABASE_TABLE_NAME = "BASE_TESTRULE";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_TESTRULE_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_TESTRULE_INSERT";
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
                                                                  {FIELDS_TESTRULE_KEY, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_CODE, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_NAME, new FieldProperties() },
                                                                  {FIELDS_MEMO, new FieldProperties() },
                                                                  {FIELDS_PS_CODE, new FieldProperties() },
                                                                  {FIELDS_LAST_TEST_TYPE, new FieldProperties() },                                           
                                                                  {FIELDS_POWER_DEGREE, new FieldProperties() },
                                                                  {FIELDS_FULL_PALLET_QTY, new FieldProperties() },
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME,  new FieldProperties(typeof(DateTime),false,DateTime.Now)}, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),false,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  