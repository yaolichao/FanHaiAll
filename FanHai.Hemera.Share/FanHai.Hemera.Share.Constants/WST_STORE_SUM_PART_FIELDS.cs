using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class WST_STORE_SUM_PART_FIELDS : TableFields
    {
        #region columns define
        /// <summary>
        /// 行主键
        /// </summary>
        public const string FIELD_ROW_KEY = "ROW_KEY";
        /// <summary>
        /// 工单号
        /// </summary>
        public const string FIELD_PART_ID = "PART_ID";
        /// <summary>
        /// 存储类型
        /// </summary>
        public const string FIELD_STORE_TYPE = "STORE_TYPE";
        /// <summary>
        ///  数量
        /// </summary>
        public const string FIELD_SUM_QUANTITY = "SUM_QUANTITY";
        /// <summary>
        /// 编辑人
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑时区
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        /// <summary>
        /// 
        /// </summary>
        public const string FIELD_SUM_QUANTITY_ALL = "SUM_QUANTITY_ALL";
        /// <summary>
        /// 
        /// </summary>
        public const string FIELD_STORE_KEY = "STORE_KEY";
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "WST_STORE_SUM_PART";       //TABLE NAME
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
                                                                {FIELD_ROW_KEY, new FieldProperties() },        
                                                                {FIELD_PART_ID, new FieldProperties()},
                                                                {FIELD_STORE_TYPE, new FieldProperties()},      
                                                                {FIELD_SUM_QUANTITY, new FieldProperties()},    
                                                                {FIELD_EDITOR , new FieldProperties()},         
                                                                {FIELD_EDIT_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now) },      
                                                                {FIELD_EDIT_TIMEZONE , new FieldProperties()}, 
                                                                {FIELD_SUM_QUANTITY_ALL,new FieldProperties()},
                                                                {FIELD_STORE_KEY,new FieldProperties()}
                                                          };
        #endregion

    }
}
