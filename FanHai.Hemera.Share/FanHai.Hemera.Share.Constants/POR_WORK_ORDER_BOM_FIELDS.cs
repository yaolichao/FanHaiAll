using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示工单BOM（POR_WORK_ORDER_BOM）的数据表类。
    /// </summary>
    public class POR_WORK_ORDER_BOM_FIELDS : TableFields
    {
        /// <summary>
        /// 工单号码
        /// </summary>
        public const string FIELD_ORDER_NUMBER = "ORDER_NUMBER";
        /// <summary>
        /// 序列号
        /// </summary>
        public const string FIELD_SEQ_NO = "SEQ_NO";
        /// <summary>
        /// 项目号
        /// </summary>
        public const string FIELD_ITEM_NO = "ITEM_NO";
        /// <summary>
        /// 物料号。
        /// </summary>
        public const string FIELD_MATERIAL_CODE = "MATERIAL_CODE";
        /// <summary>
        /// 物料描述
        /// </summary>
        public const string FIELD_DESCRIPTION= "DESCRIPTION";
        /// <summary>
        /// 需求数量。
        /// </summary>
        public const string FIELD_REQ_QTY = "REQ_QTY";
        /// <summary>
        /// 物料单位。
        /// </summary>
        public const string FIELD_MATERIAL_UNIT = "MATERIAL_UNIT";
        /// <summary>
        /// 工作中心。
        /// </summary>
        public const string FIELD_WORK_CENTER = "WORK_CENTER";
        /// <summary>
        /// 物料库位。
        /// </summary>
        public const string FIELD_STORE_LOC = "STORE_LOC";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "POR_WORK_ORDER_BOM";
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
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                                          {
                                                                              {FIELD_ORDER_NUMBER, new FieldProperties() },
                                                                              {FIELD_SEQ_NO, new FieldProperties() },
                                                                              {FIELD_ITEM_NO, new FieldProperties() },
                                                                              {FIELD_MATERIAL_CODE, new FieldProperties() },
                                                                              {FIELD_DESCRIPTION, new FieldProperties() },
                                                                              {FIELD_REQ_QTY, new FieldProperties(typeof(double),true,0) }, 
                                                                              {FIELD_MATERIAL_UNIT, new FieldProperties() },
                                                                              {FIELD_WORK_CENTER, new FieldProperties() },
                                                                              {FIELD_STORE_LOC, new FieldProperties()}
                                                                            };
    }
}
