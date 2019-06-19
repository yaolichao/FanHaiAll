using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class WST_SAP_STORE_DTL_FIELDS: TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_ROW_DTL_ID = "ROW_DTL_ID";
        /// <summary>
        /// 外键
        /// </summary>
        public const string FIELD_ROW_MAT_ID = "ROW_MAT_ID";
        /// <summary>
        /// 批号
        /// </summary>
        public const string FIELD_CHARG = "CHARG";
        /// <summary>
        /// 评估类型
        /// </summary>
        public const string FIELD_BWTAR = "BWTAR";
        /// <summary>
        /// 领料库位
        /// </summary>
        public const string FIELD_LGORT_KEY = "LGORT_KEY";
        /// <summary>
        /// 应领数量
        /// </summary>
        public const string FIELD_BDMNG = "BDMNG";
        /// <summary>
        /// 已领数量
        /// </summary>
        public const string FIELD_ENMNG = "ENMNG";
        /// <summary>
        /// 实领数量
        /// </summary>
        public const string FIELD_ERFMG = "ERFMG";
        /// <summary>
        /// 计量单位
        /// </summary>
        public const string FIELD_ERFME = "ERFME";
        /// <summary>
        /// 批次供应商
        /// </summary>
        public const string FIELD_LLIEF = "LLIEF";
        /// <summary>
        /// 内部供应商批次
        /// </summary>
        public const string FIELD_LICHA = "LICHA";
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
        public const string DATABASE_TABLE_NAME = "WST_SAP_STORE_DTL";      //TABLE NAME
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
                                                                 {FIELD_BDMNG,new FieldProperties()},      
                                                                 {FIELD_BWTAR,new FieldProperties()},     
                                                                 {FIELD_CHARG,new FieldProperties()},     
                                                                 {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime), true, DateTime.Now)},  
                                                                 {FIELD_EDITOR,new FieldProperties()},   
                                                                 {FIELD_ENMNG,new FieldProperties()},        
                                                                 {FIELD_ERFME, new FieldProperties()},    
                                                                 {FIELD_ERFMG,new FieldProperties()},
                                                                 {FIELD_LGORT_KEY,new FieldProperties()},         
                                                                 {FIELD_LICHA, new FieldProperties() },     
                                                                 {FIELD_LLIEF,new FieldProperties()},
                                                                 {FIELD_ROW_DTL_ID,new FieldProperties()},
                                                                 {FIELD_ROW_MAT_ID,new FieldProperties()}                                                                                                                             
                                                              };
        #endregion
    }
}
