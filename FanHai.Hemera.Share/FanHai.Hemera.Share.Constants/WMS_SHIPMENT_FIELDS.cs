using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示出货表及其字段的实体类。
    /// </summary>
    public class WMS_SHIPMENT_FIELDS : TableFields
    {
        /// <summary>
        /// 客检。
        /// </summary>
        public const string FIELDS_CUSTCHECK = "CUSTCHECK";
        /// <summary> 
        /// 主键。
        /// </summary>
        public const string FIELDS_SHIPMENT_KEY = "SHIPMENT_KEY";
        /// <summary>
        /// CI号。
        /// </summary>
        public const string FIELDS_CI_NO = "CI_NO";
        /// <summary>
        /// 出货单号。
        /// </summary>
        public const string FIELDS_SHIPMENT_NO = "SHIPMENT_NO";
        /// <summary>
        /// 货柜号。
        /// </summary>
        public const string FIELDS_CONTAINER_NO = "CONTAINER_NO";
        /// <summary>
        /// 托盘号(虚拟托盘号)。
        /// </summary>
        public const string FIELDS_PALLET_NO = "PALLET_NO";
        /// <summary>
        /// 运输类型。
        /// </summary>
        public const string FIELDS_SHIPMENT_TYPE = "SHIPMENT_TYPE";
        /// <summary>
        /// 组件数量。
        /// </summary>
        public const string FIELDS_QUANTITY = "QUANTITY";
        /// <summary>
        /// 出货日期。
        /// </summary>
        public const string FIELDS_SHIPMENT_DATE = "SHIPMENT_DATE";
        /// <summary>
        /// 出货人。
        /// </summary>
        public const string FIELDS_SHIPMENT_OPERATOR = "SHIPMENT_OPERATOR";
        /// 创建人。
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 创建时间。
        /// </summary>
        public const string FIELDS_CREATE_TIMEZONE = "CREATE_TIMEZONE";
        /// <summary>
        /// 编辑人。
        /// </summary>
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时间。
        /// </summary>
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        /// <summary>
        /// 状态。
        /// </summary>
        public const string FIELDS_STATUS = "STATUS";
        /// <summary>
        /// 出货地。  0：其他  1：印度
        /// </summary>
        public const string FIELDS_SHIP_GOTO = "SHIP_GOTO";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WMS_SHIPMENT";
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
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_SHIPMENT_KEY, new FieldProperties()},
                                                                  {FIELDS_CI_NO, new FieldProperties()},
                                                                  {FIELDS_SHIPMENT_NO, new FieldProperties()},
                                                                  {FIELDS_CONTAINER_NO, new FieldProperties()},
                                                                  {FIELDS_PALLET_NO, new FieldProperties()},
                                                                  {FIELDS_QUANTITY,new FieldProperties() }, 
                                                                  {FIELDS_SHIPMENT_TYPE,new FieldProperties()}, 
                                                                  {FIELDS_SHIPMENT_DATE,new FieldProperties()}, 
                                                                  {FIELDS_SHIPMENT_OPERATOR,new FieldProperties() },
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_CREATE_TIMEZONE,new FieldProperties() },             
                                                                  {FIELDS_EDITOR, new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_EDIT_TIMEZONE,new FieldProperties() }, 
                                                                  {FIELDS_SHIP_GOTO,new FieldProperties() } 
                                                                };
                                                                       
    }
}
