using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 描述批次调整或（批次返工）信息的数据库表字段实体类。
    /// </summary>
    public class WIP_COMMENT_FIELDS : TableFields
    {
        /// <summary>
        /// 批次操作主键。
        /// </summary>
        public const string FIELDS_TRANSACTION_KEY = "TRANSACTION_KEY"; //TRANSACTION_KEY
        /// <summary>
        /// 批次操作后工艺流程组主键。
        /// </summary>
        public const string FIELDS_ENTERPRISE_KEY = "ENTERPRISE_KEY";   //ENTERPRISE_KEY
        /// <summary>
        /// 批次操作后工艺流程主键。
        /// </summary>
        public const string FIELDS_ROUTE_KEY = "ROUTE_KEY";             //ROUTE_KEY
        /// <summary>
        /// 批次操作后工步主键。
        /// </summary>
        public const string FIELDS_STEP_KEY = "STEP_KEY";               //STEP_KEY
        /// <summary>
        /// 被修改的批次栏位内容。
        /// </summary>
        public const string FIELDS_BEFORE_CONTENT = "BEFORE_CONTENT";                 //CONTENT
        /// <summary>
        /// 被修改的批次栏位修改后内容。
        /// </summary>
        public const string FIELDS_AFTER_CONTENT = "AFTER_CONTENT";                 //CONTENT
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";     
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";             //EDIT_TIME   
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";     //EDIT_TIMEZONE

        /// <summary>
        /// 数据库表名。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_COMMENT";                                //TABLE NAME

        /// <summary>
        /// 返回数据库表名。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 返回数据库表字段集合。
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_TRANSACTION_KEY , new FieldProperties() },                     
                                                                  {FIELDS_ENTERPRISE_KEY , new FieldProperties() },
                                                                  {FIELDS_ROUTE_KEY , new FieldProperties() }, 
                                                                  {FIELDS_STEP_KEY , new FieldProperties() },           
                                                                  {FIELDS_BEFORE_CONTENT , new FieldProperties() },
                                                                  {FIELDS_AFTER_CONTENT , new FieldProperties() },
                                                                  {FIELDS_EDITOR , new FieldProperties() },                  
                                                                  {FIELDS_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now)  },             
                                                                  {FIELDS_EDIT_TIMEZONE , new FieldProperties() }
                                                                };
    }   
}
