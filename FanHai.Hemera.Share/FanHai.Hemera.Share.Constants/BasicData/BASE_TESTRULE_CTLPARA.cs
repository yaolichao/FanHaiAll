using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_TESTRULE_CTLPARA : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_CONTROL_PARAM_KEY = "CONTROL_PARAM_KEY";
        /// <summary>
        /// 主键值（测试规则主表ID）
        /// </summary>
        public const string FIELDS_TESTRULE_KEY = "TESTRULE_KEY";
        /// <summary>
        /// 控制对象
        /// </summary>
        public const string FIELDS_CONTROL_OBJ = "CONTROL_OBJ";
        /// <summary>
        /// 控制类型
        /// </summary>
        public const string FIELDS_CONTROL_TYPE = "CONTROL_TYPE";
        /// <summary>
        /// 值
        /// </summary>
        public const string FIELDS_CONTROL_VALUE = "CONTROL_VALUE";        
        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_TESTRULE_CTLPARA";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_TESTRULE_CTLPARA_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_TESTRULE_CTLPARA_INSERT";
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
                                                                  {FIELDS_CONTROL_PARAM_KEY, new FieldProperties() },
                                                                  {FIELDS_TESTRULE_KEY, new FieldProperties() },
                                                                  {FIELDS_CONTROL_OBJ, new FieldProperties() },
                                                                  {FIELDS_CONTROL_TYPE, new FieldProperties() },
                                                                  {FIELDS_CONTROL_VALUE, new FieldProperties() },                                                                 
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now) }, 
                                                                  {FIELDS_EDITOR, new FieldProperties() }, 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  