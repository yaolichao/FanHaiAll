using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_TESTRULE_PROLEVEL : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_PRODUCT_LEVEL_KEY = "PRODUCT_LEVEL_KEY";
        /// <summary>
        /// 主键值（测试规则主表ID）
        /// </summary>
        public const string FIELDS_TESTRULE_KEY = "TESTRULE_KEY";
        /// <summary>
        /// 序号
        /// </summary>
        public const string FIELDS_PROLEVEL_SEQ = "PROLEVEL_SEQ";
        /// <summary>
        /// 等级
        /// </summary>
        public const string FIELDS_GRADE = "GRADE";
        /// <summary>
        /// 混档--主分档(0：默认，1：混档)
        /// </summary>
        public const string FIELDS_MIN_LEVEL = "MIN_LEVEL";
        /// <summary>
        /// 混色(0：默认，1：混托)
        /// </summary>
        public const string FIELDS_MIN_COLOR = "MIN_COLOR";
        /// <summary>
        /// 包装组
        /// </summary>
        public const string FIELDS_PALLET_GROUP = "PALLET_GROUP";
        /// <summary>
        /// 混档--子分档(0：默认，1：混档)
        /// </summary>
        public const string FIELDS_MIN_LEVEL_DETAIL = "MIN_LEVEL_DETAIL";

        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_TESTRULE_PROLEVEL";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_TESTRULE_PROLEVEL_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_TESTRULE_PROLEVEL_INSERT";
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
                                                                  {FIELDS_PRODUCT_LEVEL_KEY, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_KEY, new FieldProperties() },
                                                                  {FIELDS_PROLEVEL_SEQ, new FieldProperties() },
                                                                  {FIELDS_GRADE, new FieldProperties() },
                                                                  {FIELDS_MIN_LEVEL, new FieldProperties() },
                                                                  {FIELDS_MIN_LEVEL_DETAIL,new FieldProperties()},
                                                                  {FIELDS_MIN_COLOR, new FieldProperties() },                                           
                                                                  {FIELDS_PALLET_GROUP, new FieldProperties() },                                                         
                                                                  {FIELDS_CREATER, new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now)  }, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  