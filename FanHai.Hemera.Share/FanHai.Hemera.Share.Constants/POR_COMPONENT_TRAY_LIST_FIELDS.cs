using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库虚拟托盘表及其子信息
    /// </summary>
    public class POR_COMPONENT_TRAY_LIST_FIELDS : TableFields
    {
        /// <summary>
        /// ID
        /// </summary>
        public const string FIELD_ID = "ID";
        /// <summary>
        /// 托盘名称
        /// </summary>
        public const string FIELD_TRAY_NUMBER = "TrayNumber";
        /// <summary>
        /// 托盘编号
        /// </summary>
        public const string FIELD_TRAY_VALUE = "TrayValue";
        /// <summary>
        /// 组件序列号
        /// </summary>
        public const string FIELD_LOT_NUMBER = "LotNumber";
        /// <summary>
        /// 线别编号
        /// </summary>
        public const string FIELD_LINE_KEY = "LineKey";
        /// <summary>
        /// 线别名称
        /// </summary>
        public const string FIELD_LINE_NAME = "LineName";
        /// <summary>
        /// 包装数量
        /// </summary>
        public const string FIELD_PACKAGE_NUMBER = "PackageNumber";
        /// <summary>
        /// 当前托盘数量
        /// </summary>
        public const string FIELD_NUMBER = "Number";
        /// <summary>
        /// 颜色
        /// </summary>
        public const string FIELD_COLOR = "Color";
        /// <summary>
        /// 工单号
        /// </summary>
        public const string FIELD_WORK_ORDER_NO = "WorkOrderNo";
        /// <summary>
        /// 成品料号
        /// </summary>
        public const string FIELD_PATR_NUMBER = "PatrNumber";
        /// <summary>
        /// 等级
        /// </summary>
        public const string FIELD_GRADE_NAME = "GradeName";
        /// <summary>
        /// 功率档
        /// </summary>
        public const string FIELD_PS_KEY = "PsKey";
        /// <summary>
        /// 档位
        /// </summary>
        public const string FIELD_SUB_POWER_LEVEL = "SubPowerlevel";

        public const string FIELD_ISFLIP = "IsFlip";

        /// <summary>
        /// 创建时间/记录时间
        /// </summary>
        public const string FIELD_CREATE_TIME = "CreateTime";

        public const string DATABASE_TABLE_NAME = "POR_COMPONENT_TRAY_LIST";                                //TABLE NAME

        public const string DATABASE_TABLE_NAME_FORUPDATE = "POR_LOT_UPDATE";

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
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_ID , new FieldProperties() },                     
                                                                  {FIELD_TRAY_NUMBER , new FieldProperties() },                
                                                                  {FIELD_TRAY_VALUE , new FieldProperties() },              
                                                                  {FIELD_LOT_NUMBER,new FieldProperties()},
                                                                  {FIELD_LINE_KEY , new FieldProperties() }, 
                                                                  {FIELD_LINE_NAME , new FieldProperties() },              
                                                                  {FIELD_PACKAGE_NUMBER,new FieldProperties()},
                                                                  {FIELD_NUMBER,new FieldProperties()},
                                                                  {FIELD_COLOR,new FieldProperties()},
                                                                  {FIELD_WORK_ORDER_NO , new FieldProperties() },                  
                                                                  {FIELD_PATR_NUMBER,new FieldProperties()},
                                                                  {FIELD_GRADE_NAME  , new FieldProperties() },                 
                                                                  {FIELD_CREATE_TIME  , new FieldProperties() },
                                                                  {FIELD_ISFLIP,new FieldProperties() }
                                                              };

    }
}
