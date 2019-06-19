using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示批次暂停释放数据库表及其字段的的实体类。
    /// </summary>
    public class WIP_HOLD_RELEASE_FIELDS : TableFields 
    {
        /// <summary>
        /// 暂停操作交易主键。
        /// </summary>
        public const string FIELD_TRANSACTION_KEY = "TRANSACTION_KEY";
        /// <summary>
        /// 释放操作交易主键。
        /// </summary>
        public const string FIELD_RELEASE_TRANSACTION_KEY = "RELEASE_TRANSACTION_KEY";
        /// <summary>
        /// 暂停组主键。
        /// </summary>
        public const string FIELD_REASON_CODE_CATEGORY_KEY = "REASON_CODE_CATEGORY_KEY";
        /// <summary>
        /// 暂停组名称。
        /// </summary>
        public const string FIELD_REASON_CODE_CATEGORY_NAME = "REASON_CODE_CATEGORY_NAME";
        /// <summary>
        /// 暂停主键。
        /// </summary>
        public const string FIELD_REASON_CODE_KEY = "REASON_CODE_KEY";
        /// <summary>
        /// 暂停名称。
        /// </summary>
        public const string FIELD_REASON_CODE_NAME = "REASON_CODE_NAME";
        /// <summary>
        /// 暂停操作人。
        /// </summary>
        public const string FIELD_HOLD_OPERATOR = "HOLD_OPERATOR";
        /// <summary>
        /// 暂停时间。
        /// </summary>
        public const string FIELD_HOLD_TIME = "HOLD_TIME";
        /// <summary>
        /// 暂停时区。
        /// </summary>
        public const string FIELD_HOLD_TIMEZONE = "HOLD_TIMEZONE";
        /// <summary>
        /// 暂停说明。
        /// </summary>
        public const string FIELD_HOLD_DESCRIPTION = "HOLD_DESCRIPTION";
        /// <summary>
        /// 释放操作人。
        /// </summary>
        public const string FIELD_RELEASE_OPERATOR = "RELEASE_OPERATOR";
        /// <summary>
        /// 释放时间。
        /// </summary>
        public const string FIELD_RELEASE_TIME = "RELEASE_TIME";
        /// <summary>
        /// 释放时区。
        /// </summary>
        public const string FIELD_RELEASE_TIMEZONE = "RELEASE_TIMEZONE";
        /// <summary>
        /// 释放说明。
        /// </summary>
        public const string FIELD_RELEASE_DESCRIPTION = "RELEASE_DESCRIPTION";
        /// <summary>
        /// 是否已释放。0：未释放。1：已释放。
        /// </summary>
        public const string FIELD_IS_RELEASE = "IS_RELEASE";
        /// <summary>
        /// 暂停时密码。
        /// </summary>
        public const string FIELD_HOLD_PASSWORD = "HOLD_PASSWORD";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";  
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_HOLD_RELEASE";
        /// <summary>
        /// 获取数据库表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 获取数据库表字段集合。
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties> 
                                                                    { 
                                                                        {FIELD_TRANSACTION_KEY,new FieldProperties()},
                                                                        {FIELD_REASON_CODE_CATEGORY_KEY,new FieldProperties()},
                                                                        {FIELD_REASON_CODE_CATEGORY_NAME,new FieldProperties()},
                                                                        {FIELD_REASON_CODE_KEY,new FieldProperties()}, 
                                                                        {FIELD_REASON_CODE_NAME,new FieldProperties()},      
                                                                        {FIELD_HOLD_OPERATOR,new FieldProperties()},
                                                                        {FIELD_HOLD_TIME,new FieldProperties(typeof(DateTime),true,DBNull.Value)},
                                                                        {FIELD_HOLD_TIMEZONE,new FieldProperties()},
                                                                        {FIELD_HOLD_DESCRIPTION,new FieldProperties()},
                                                                        {FIELD_HOLD_PASSWORD,new FieldProperties()},
                                                                        {FIELD_IS_RELEASE,new FieldProperties(typeof(int),true,0)},
                                                                        {FIELD_RELEASE_TRANSACTION_KEY,new FieldProperties()},
                                                                        {FIELD_RELEASE_OPERATOR,new FieldProperties()},
                                                                        {FIELD_RELEASE_TIME,new FieldProperties(typeof(DateTime),true,DBNull.Value)},
                                                                        {FIELD_RELEASE_TIMEZONE,new FieldProperties()},
                                                                        {FIELD_RELEASE_DESCRIPTION,new FieldProperties()},
                                                                        {FIELD_EDITOR ,new FieldProperties()},          
                                                                        {FIELD_EDIT_TIME ,new FieldProperties(typeof(DateTime),true,DBNull.Value)},       
                                                                        {FIELD_EDIT_TIMEZONE,new FieldProperties()},
                                                                    };
    } 
}
