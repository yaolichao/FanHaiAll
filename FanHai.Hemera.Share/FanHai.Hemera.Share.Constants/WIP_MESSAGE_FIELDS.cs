using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示记录批次自动进站或自动出站消息的数据表及其字段实体类。
    /// </summary>
    public class WIP_MESSAGE_FIELDS : TableFields
    {
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_ROW_KEY        ="ROW_KEY";
        /// <summary>
        /// 消息标题
        /// </summary>
        public const string FIELD_TITLE = "TITLE";
        /// <summary>
        /// 消息内容
        /// </summary>
        public const string FIELD_CONTEXT = "CONTEXT";
        /// <summary>
        /// 对象主键
        /// </summary>
        public const string FIELD_OBJECTKEY = "OBJECTKEY";
        /// <summary>
        /// 对象类型:LOT,JOB
        /// </summary>
        public const string FIELD_OBJECTTYPE = "OBJECTTYPE";
        /// <summary>
        /// 消息严重程度(0:小 ,9大)
        /// </summary>
        public const string FIELD_CRITICAL_LEVEL = "CRITICAL_LEVEL";
        /// <summary>
        /// 消息私有等级(0:Public 1:Group 2:Personal)
        /// </summary>
        public const string FIELD_PUBLIC_LEVEL = "PUBLIC_LEVEL";
        /// <summary>
        /// 消息发送给谁
        /// </summary>
        public const string FIELD_TO_USER = "TO_USER";
        /// <summary>
        /// 消息发送给组
        /// </summary>
        public const string FIELD_TO_GROUP = "TO_GROUP";
        /// <summary>
        /// 状态0:未读 1:已读
        /// </summary>
        public const string FIELD_STATUS = "STATUS";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 创建时区
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";

        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_MESSAGE";       //TABLE NAME
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 数据库表中字段集合。
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
                                                                {FIELD_ROW_KEY, new FieldProperties() },
                                                                {FIELD_TITLE, new FieldProperties()},
                                                                {FIELD_CONTEXT, new FieldProperties()},      
                                                                {FIELD_CRITICAL_LEVEL, new FieldProperties()},  
                                                                {FIELD_PUBLIC_LEVEL, new FieldProperties() },
                                                                {FIELD_OBJECTKEY, new FieldProperties()},
                                                                {FIELD_OBJECTTYPE, new FieldProperties()},
                                                                {FIELD_TO_USER, new FieldProperties()},
                                                                {FIELD_TO_GROUP, new FieldProperties()},     
                                                                {FIELD_STATUS, new FieldProperties()},  
                                                                {FIELD_CREATOR , new FieldProperties()},         
                                                                {FIELD_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now) },      
                                                                {FIELD_CREATE_TIMEZONE , new FieldProperties()}
                                                          };

    }
}
