#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工艺流程数据库表及其字段的实体类。
    /// </summary>
    public class POR_ROUTE_ROUTE_VER_FIELDS:TableFields
    {
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public const string FIELD_ROUTE_ROUTE_VER_KEY = "ROUTE_ROUTE_VER_KEY";
        /// <summary>
        /// 工艺流程名称。
        /// </summary>
        public const string FIELD_ROUTE_NAME = "ROUTE_NAME";                      //RouteName
        /// <summary>
        /// 版本号。
        /// </summary>
        public const string FIELD_ROUTE_VERSION = "ROUTE_VERSION";                //版本
        /// <summary>
        /// 状态：1 = Active, 0 = InActive, 2 = Archive
        /// </summary>
        public const string FIELD_ROUTE_STATUS = "ROUTE_STATUS";
        /// <summary>
        /// 有效期开始时间。
        /// </summary>
        public const string FIELD_EFFECTIVITY_START = "EFFECTIVITY_START";        //有效期开始于
        /// <summary>
        /// 有效期结束时间。
        /// </summary>
        public const string FIELD_EFFECTIVITY_END = "EFFECTIVITY_END";            //有效期结束于
        /// <summary>
        /// 描述。
        /// </summary>
        public const string FIELD_DESCRIPTION = "DESCRIPTION";                    //描述
        /// <summary>
        /// （不用）站点
        /// </summary>
        public const string FIELD_SITE_NUMBER = "SITE_NUMBER";                    //站点
        /// <summary>
        /// 创建人。
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";                            //创建者
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";                    //创建时间
        /// <summary>
        /// 创建时间时区。
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";            //创建时间时区
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";				              //编辑者
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";				          //编辑时间
        /// <summary>
        /// 编辑时区。
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";				  //编辑时间时区
        /// <summary>
        /// 该版本记录来自于的版本号。
        /// </summary>
        public const string FIELD_ROUTE_FROM_VERSION = "ROUTE_FROM_VERSION"; 
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_ROUTE_VER";       //TABLE NAME
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
        /// 数据库表字段集合。
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
                                                                    {FIELD_ROUTE_ROUTE_VER_KEY, new FieldProperties() },
                                                                    {FIELD_ROUTE_NAME, new FieldProperties() },
                                                                    {FIELD_ROUTE_STATUS, new FieldProperties() },
                                                                    {FIELD_ROUTE_VERSION, new FieldProperties() }, 
                                                                    {FIELD_EFFECTIVITY_START, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                    {FIELD_EFFECTIVITY_END, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                    {FIELD_DESCRIPTION, new FieldProperties() }, 
                                                                    {FIELD_SITE_NUMBER, new FieldProperties() },
                                                                    {FIELD_CREATOR, new FieldProperties() },
                                                                    {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) }, 
                                                                    {FIELD_CREATE_TIMEZONE, new FieldProperties() },
                                                                    {FIELD_EDITOR, new FieldProperties() },
                                                                    {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) }, 
                                                                    {FIELD_EDIT_TIMEZONE, new FieldProperties() },
                                                                    {FIELD_ROUTE_FROM_VERSION, new FieldProperties() }   
                                                              };
    }
}
