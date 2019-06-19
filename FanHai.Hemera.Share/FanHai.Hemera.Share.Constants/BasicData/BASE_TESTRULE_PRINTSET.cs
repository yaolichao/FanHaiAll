using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_TESTRULE_PRINTSET : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_PRINTSET_KEY = "PRINTSET_KEY";
        /// <summary>
        /// 主键值(测试规则衰减设置表ID:BASE_TESTRULE_DECAY)
        /// </summary>
        public const string FIELDS_DECAY_KEY = "DECAY_KEY";
        /// <summary>
        /// 主键值（测试规则主表ID）
        /// </summary>
        public const string FIELDS_TESTRULE_KEY = "TESTRULE_KEY";
        /// <summary>
        /// 打印张数
        /// </summary>
        public const string FIELDS_PRINT_QTY = "PRINT_QTY";
        /// <summary>
        /// 打印名称(标签/铭牌)
        /// </summary>
        public const string FIELDS_VIEW_NAME = "VIEW_NAME";
        /// <summary>
        /// 打印地址(标签/铭牌)
        /// </summary>
        public const string FIELDS_VIEW_ADDRESS = "VIEW_ADDRESS";
        /// <summary>
        /// 是否标签
        /// </summary>
        public const string FIELDS_ISLABEL = "ISLABEL";
        /// <summary>
        /// 是否主标签
        /// </summary>
        public const string FIELDS_ISMAIN = "ISMAIN";

        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
        /// <summary>
        /// 包装工序是否列印侧板标签 1：列印，0：不列印
        /// </summary>
        public const string FIELDS_ISPACKAGEPRINT = "ISPACKAGEPRINT";


        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_TESTRULE_PRINTSET";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_TESTRULE_PRINTSET_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_TESTRULE_PRINTSET_INSERT";
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
                                                                  {FIELDS_PRINTSET_KEY, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_KEY, new FieldProperties() },
                                                                  {FIELDS_DECAY_KEY, new FieldProperties() },
                                                                  {FIELDS_PRINT_QTY,  new FieldProperties(typeof(Int16),false,1)},
                                                                  {FIELDS_VIEW_NAME, new FieldProperties() },
                                                                  {FIELDS_VIEW_ADDRESS, new FieldProperties() },
                                                                  {FIELDS_ISLABEL, new FieldProperties() },
                                                                  {FIELDS_ISMAIN, new FieldProperties() }, 
                                                                  {FIELDS_ISPACKAGEPRINT,new FieldProperties()},                                                                                       
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME,  new FieldProperties(typeof(DateTime),true,DateTime.Now)  }, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  