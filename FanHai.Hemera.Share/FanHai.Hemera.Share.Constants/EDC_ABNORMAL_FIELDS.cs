#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion


namespace FanHai.Hemera.Share.Constants
{
    public class EDC_ABNORMAL_FIELDS : TableFields
    {
        #region columns define
        /// <summary>
        /// 异常规则ID
        /// </summary>
        public const string FIELD_ABNORMALID = "ABNORMALID";
        /// <summary>
        /// 异常代码
        /// </summary>
        public const string FIELD_ARULECODE      = "ARULECODE";
        /// <summary>
        /// 异常代码描述
        /// </summary>
        public const string FIELD_ABNORMALDESC   = "ABNORMALDESC";
        /// <summary>
        /// 异常规则设定的颜色
        /// </summary>
        public const string FIELD_ABNORMALCOLOR = "ABNORMALCOLOR";
        /// <summary>
        /// 是否可用
        /// </summary>
        public const string FIELD_LVORM          = "LVORM";
        /// <summary>
        /// 创建者
        /// </summary>
        public const string FIELD_CREATOR        = "CREATOR";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CREATE_TIME    = "CREATE_TIME";
        /// <summary>
        /// 编辑者
        /// </summary>
        public const string FIELD_EDITOR         = "EDITOR";
        /// <summary>
        /// 编辑人员
        /// </summary>
        public const string FIELD_EDIT_TIME      = "EDIT_TIME";

        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "EDC_ABNORMALRULE";       //TABLE NAME

        public const string DATABASE_TABLE_NAME_FORUPDATE = "EDC_ABNORMALRULE_FORUPDATE";       //TABLE NAME
        #endregion

        #region get TABLE_NAME
        /// <summary>
        /// override TABLE_NAME
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        #endregion

        #region get FIELDS
        /// <summary>
        /// override fields
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        #region define dictionary of containing column's fields
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                    {FIELD_ABNORMALID, new FieldProperties() },        
                                                                    {FIELD_ARULECODE, new FieldProperties() },       
                                                                    {FIELD_ABNORMALDESC, new FieldProperties() },  
                                                                    {FIELD_ABNORMALCOLOR,new FieldProperties()},    
                                                                    {FIELD_LVORM, new FieldProperties() },  
                                                                    {FIELD_CREATOR, new FieldProperties() },    
                                                                    {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },   
                                                                    {FIELD_EDITOR, new FieldProperties() }, 
                                                                    {FIELD_EDIT_TIME,  new FieldProperties(typeof(DateTime), true, DateTime.Now)  }         
                                                              };
        #endregion
    }
}
