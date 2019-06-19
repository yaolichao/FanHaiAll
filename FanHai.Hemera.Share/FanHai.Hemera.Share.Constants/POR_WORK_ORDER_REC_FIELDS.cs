using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class POR_WORK_ORDER_REC_FIELDS : TableFields
    {
        /// <summary>
        /// 主键
        /// </summary>
        public static string FIELD_ROW_KEY = "ROW_KEY";
        /// <summary>
        /// 序号
        /// </summary>
        public static string FIELD_IDX_NUMBER = "IDX_NUMBER";
        /// <summary>
        /// 配料单编码
        /// </summary>
        public static string FIELD_REC_ORDER_NUMBER = "REC_ORDER_NUMBER";
        /// <summary>
        /// 生产工单编号
        /// </summary>
        public static string FIELD_WORK_ORDER_NUMBER = "WORK_ORDER_NUMBER";
        /// <summary>
        /// 批次号（部品批号）
        /// </summary>
        public static string FIELD_IN_ORDER_NUMBER = "IN_ORDER_NUMBER";
        /// <summary>
        /// 厚度
        /// </summary>
        public static string FIELD_THICKNESS = "THICKNESS";
        /// <summary>
        /// 数量
        /// </summary>
        public static string FIELD_QUANTITY = "QUANTITY";
        /// <summary>
        /// 硅片等级
        /// </summary>
        public static string FIELD_QTY_LEVEL = "QTY_LEVEL";
        /// <summary>
        /// 供应商编号
        /// </summary>
        public static string FIELD_SUPPLY_NUMBER = "SUPPLY_NUMBER";
        public static string FIELD_EDITOR = "EDITOR";
        public static string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时间时区
        /// </summary>
        public static string FIELD_EDIT_TIMEZONE_KEY = "EDIT_TIMEZONE_KEY";
        /// <summary>
        /// 物料料号(MATNR)
        /// </summary>
        public static string FIELD_MTRL_ID = "MTRL_ID";
        /// <summary>
        /// SAP开立时间(ERDAT)
        /// </summary>
        public static string FIELD_CREATE_DATE = "CREATE_DATE";
        /// <summary>
        /// 作业人员
        /// </summary>
        public static string FIELD_OPERATOR_ID = "OPERATOR_ID";
        /// <summary>
        /// 线别(ATWRT)
        /// </summary>
        public static string FIELD_ATWRT            = "ATWRT";
        /// <summary>
        /// 硅片物料描述(SILICON_MAKTX)
        /// </summary>
        public static string FIELD_SILICON_MAKTX    = "SILICON_MAKTX";
        /// <summary>
        /// 库位(LGORT)
        /// </summary>
        public static string FIELD_LGORT            = "LGORT";
        /// <summary>
        /// 单位(MEINS)
        /// </summary>
        public static string FIELD_MEINS            = "MEINS";
        /// <summary>
        /// 硅片标称厚度公差(SWT004)
        /// </summary>
        public static string FIELD_SWT004           = "SWT004";



        public const string DATABASE_TABLE_NAME = "POR_WORK_ORDER_REC";

        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_ROW_KEY, new FieldProperties()},
                                                                  {FIELD_IDX_NUMBER,new FieldProperties()},
                                                                  {FIELD_REC_ORDER_NUMBER,new FieldProperties() },
                                                                  {FIELD_WORK_ORDER_NUMBER,new FieldProperties() }, 
                                                                  {FIELD_IN_ORDER_NUMBER,new FieldProperties() }, 
                                                                  {FIELD_THICKNESS,new FieldProperties() }, 
                                                                  {FIELD_QUANTITY,new FieldProperties() }, 
                                                                  {FIELD_QTY_LEVEL,new FieldProperties() }, 
                                                                  {FIELD_SUPPLY_NUMBER,new FieldProperties() }, 
                                                                  {FIELD_EDITOR, new FieldProperties()},
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),false, DateTime.Now)},
                                                                  {FIELD_EDIT_TIMEZONE_KEY,new FieldProperties() },
                                                                  {FIELD_OPERATOR_ID,new FieldProperties() },

                                                                  {FIELD_ATWRT,new FieldProperties() },
                                                                  {FIELD_SILICON_MAKTX,new FieldProperties() },
                                                                  {FIELD_LGORT,new FieldProperties() },
                                                                  {FIELD_MEINS,new FieldProperties() },
                                                                  {FIELD_SWT004,new FieldProperties() }
                                                                };
    }
}
