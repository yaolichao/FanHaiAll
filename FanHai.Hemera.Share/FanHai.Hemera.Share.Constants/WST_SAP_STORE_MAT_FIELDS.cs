using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class WST_SAP_STORE_MAT_FIELDS: TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_ROW_MAT_ID = "ROW_MAT_ID";
        /// <summary>
        /// 配料单号
        /// </summary>
        public const string FIELD_MBLNR = "MBLNR";
        /// <summary>
        /// 工厂ID
        /// </summary>
        public const string FIELD_WERKS_KEY = "WERKS_KEY";
        /// <summary>
        /// 工厂名称
        /// </summary>
        public const string FIELD_WERKS = "WERKS";
        /// <summary>
        /// 物料移动类型
        /// </summary>
        public const string FIELD_BWART = "BWART";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CDATE = "CDATE";
        /// <summary>
        /// 领料时间
        /// </summary>
        public const string FIELD_ODATE = "ODATE";
        /// <summary>
        /// 过账时间
        /// </summary>
        public const string FIELD_IDATE = "IDATE";
        /// <summary>
        /// 工单号码
        /// </summary>
        public const string FIELD_AUFNR = "AUFNR";
        /// <summary>
        /// 物料编码
        /// </summary>
        public const string FIELD_MATNR = "MATNR";
        /// <summary>
        /// 物料描述
        /// </summary>
        public const string FIELD_MATXT = "MATXT";
        /// <summary>
        /// 线边仓
        /// </summary>
        public const string FIELD_STORE_KEY = "STORE_KEY";
        /// <summary>
        /// 领料人员工号
        /// </summary>
        public const string FIELD_RECNO = "RECNO";
        /// <summary>
        /// 发料员工号
        /// </summary>
        public const string FIELD_SENDNO = "SENDNO";
        /// <summary>
        /// 线主键
        /// </summary>
        public const string FIELD_LINE_KEY = "LINE_KEY";
        /// <summary>
        /// 流程组主键
        /// </summary>
        public const string FIELD_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        /// <summary>
        /// 流程主键
        /// </summary>
        public const string FIELD_ROUTE_KEY = "ROUTE_KEY";
        /// <summary>
        /// 工步主键(工序名称)
        /// </summary>
        public const string FIELD_STEP_KEY = "STEP_KEY";
        /// <summary>
        /// 班名
        /// </summary>
        public const string FIELD_SHIFT_NAME = "SHIFT_NAME";
        /// <summary>
        /// 编辑人员
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";

        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "WST_SAP_STORE_MAT";      //TABLE NAME
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
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                 {FIELD_AUFNR ,new FieldProperties()},      
                                                                 {FIELD_BWART,new FieldProperties()},     
                                                                 {FIELD_CDATE,new FieldProperties(typeof(DateTime), true, DateTime.Now)},     
                                                                 {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},  
                                                                 {FIELD_EDITOR,new FieldProperties()},   
                                                                 {FIELD_ENTERPRISE_KEY,new FieldProperties()},        
                                                                 {FIELD_IDATE, new FieldProperties(typeof(DateTime), true, DateTime.Now)},    
                                                                 {FIELD_LINE_KEY,new FieldProperties()},
                                                                 {FIELD_MATNR,new FieldProperties()},         
                                                                 {FIELD_MATXT, new FieldProperties() },     
                                                                 {FIELD_MBLNR,new FieldProperties()},
                                                                 {FIELD_ODATE,new FieldProperties(typeof(DateTime), true, DateTime.Now)},
                                                                 {FIELD_RECNO,new FieldProperties()},
                                                                 {FIELD_ROUTE_KEY,new FieldProperties()},
                                                                 {FIELD_ROW_MAT_ID,new FieldProperties()},                                                                 
                                                                 {FIELD_SENDNO,new FieldProperties()},
                                                                 {FIELD_SHIFT_NAME,new FieldProperties()},
                                                                 {FIELD_STEP_KEY,new FieldProperties()},
                                                                 {FIELD_STORE_KEY,new FieldProperties()}, 
                                                                 {FIELD_WERKS,new FieldProperties()},
                                                                 {FIELD_WERKS_KEY,new FieldProperties()} 
                                                              };
        #endregion
    }
}
