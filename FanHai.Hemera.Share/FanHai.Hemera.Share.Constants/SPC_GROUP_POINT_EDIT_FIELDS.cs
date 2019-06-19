using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据表SPC_GROUP_POINT_EDIT_FIELDS的信息。
    /// </summary>
    public class SPC_GROUP_POINT_EDIT_FIELDS : TableFields
    {      
        /// <summary>
        /// 参数主键
        /// </summary>
        public const string POINT_DTL_KEY = "POINT_DTL_KEY";
	      
        public const string POINT_KEY = "POINT_KEY";
    
        public const string EDIT_REASON = "EDIT_REASON"; 
      
        public const string EDIT_TIME = "EDIT_TIME";	 
  
        public const string EDITOR = "EDITOR";
 
        public const string DATABASE_TABLE_NAME = "SPC_GROUP_POINTS_EDIT";

       
        /// <summary>
        /// 数据表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 数据表字段集合。
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
                                                                  {POINT_DTL_KEY, new FieldProperties() },
                                                                  {POINT_KEY, new FieldProperties() },
                                                                  {EDIT_REASON, new FieldProperties()},                                                                 
                                                                  //{EDIT_TIME, new FieldProperties() },
                                                                  {EDITOR, new FieldProperties() }                                                                  
                                                                };
    
    }


}
