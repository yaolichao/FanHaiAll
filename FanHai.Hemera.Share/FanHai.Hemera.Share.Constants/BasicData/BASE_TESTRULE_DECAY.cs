using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_TESTRULE_DECAY : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_DECAY_KEY = "DECAY_KEY";
        /// <summary>
        /// 主键值（测试规则主表ID）
        /// </summary>
        public const string FIELDS_TESTRULE_KEY = "TESTRULE_KEY";
        /// <summary>
        /// 序号
        /// </summary>
        public const string FIELDS_DECAY_SQL = "DECAY_SQL";
        /// <summary>
        /// 衰减系数(衰减系数表ID：BASE_DECAYCOEFFI)
        /// </summary>
        public const string FIELDS_DECOEFFI_KEY = "DECOEFFI_KEY";
        /// <summary>
        /// 功率下线
        /// </summary>
        public const string FIELDS_DECAY_POWER_MIN = "DECAY_POWER_MIN";
        /// <summary>
        /// 功率上线
        /// </summary>
        public const string FIELDS_DECAY_POWER_MAX = "DECAY_POWER_MAX";

        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_TESTRULE_DECAY";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_TESTRULE_DECAY_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_TESTRULE_DECAY_INSERT";
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
                                                                  {FIELDS_DECAY_KEY, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_KEY, new FieldProperties() },
                                                                  {FIELDS_DECAY_SQL, new FieldProperties() },
                                                                  {FIELDS_DECOEFFI_KEY, new FieldProperties() },
                                                                  {FIELDS_DECAY_POWER_MIN, new FieldProperties() },
                                                                  {FIELDS_DECAY_POWER_MAX, new FieldProperties() },                                                                                                                                                          
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now)}, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(int),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  