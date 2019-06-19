using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class FMM_PRODUCTION_LINE_SUB_FIELDS : TableFields
    {
        #region fields
        /// <summary>
        /// 子线记录主键
        /// </summary>
        public const string FIELDS_SUB_LINE_KEY = "SUB_LINE_KEY";
        /// <summary>
        /// 线别主键
        /// </summary>
        public const string FIELDS_PRODUCTION_LINE_KEY = "PRODUCTION_LINE_KEY";
        /// <summary>
        /// 子线别主键
        /// </summary>
        public const string FIELDS_PRODUCTION_LINE_SUB_KEY = "PRODUCTION_LINE_SUB_KEY";
        /// <summary>
        /// 子线别主键
        /// </summary>
        public const string FIELDS_LINE_NAME = "LINE_NAME";
        /// <summary>
        /// 工序名称
        /// </summary>
        public const string FIELDS_OPERATION_NAME = "OPERATION_NAME";
        /// <summary>
        /// 版本号
        /// </summary>
        public const string FIELDS_VERSION_NO = "VERSION_NO";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATOR";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "FMM_PRODUCTION_LINE_SUB";
        public const string DATABASE_TABLE_FORDELETE = "FMM_PRODUCTION_LINE_SUB_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "FMM_PRODUCTION_LINE_SUB_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "FMM_PRODUCTION_LINE_SUB_INSERT";
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

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                          
                                                                  {FIELDS_PRODUCTION_LINE_KEY,new FieldProperties()},
                                                                  {FIELDS_PRODUCTION_LINE_SUB_KEY,new FieldProperties()},
                                                                  {FIELDS_LINE_NAME,new FieldProperties()},
                                                                  {FIELDS_OPERATION_NAME, new FieldProperties() },  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }
}