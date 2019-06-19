using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示存储采集设备数据的数据表名及其字段名的实体类。
    /// </summary>
    public class EDC_COLLECTION_DATA_FIELDS:TableFields
    {
      /// <summary>
      /// Position的序号(3片5点的点位置)，默认为0。
      /// </summary>
      public const string FIELD_SP_UNIT_SEQ	  ="SP_UNIT_SEQ";
      /// <summary>
      /// 参数编号。
      /// </summary>
      public const string FIELD_PARAM_KEY	  ="PARAM_KEY";
      /// <summary>
      /// EDC的事例序号。
      /// </summary>
      public const string FIELD_EDC_INS_KEY   ="EDC_INS_KEY";            
      /// <summary>
      /// 参数值。
      /// </summary>
      public const string FIELD_PARAM_VALUE   ="PARAM_VALUE";
      /// <summary>
      /// 字段主键。
      /// </summary>
      public const string FIELD_COL_KEY       ="COL_KEY";
      /// <summary>
      /// 参数是否有效。
      /// </summary>
      public const string FIELD_VALID_FLAG	  ="VALID_FLAG";
      /// <summary>
      /// 参数无效，触发的规则。
      /// </summary>
      public const string FIELD_FAILED_RULES  ="FAILED_RULES";
      /// <summary>
      /// EDC的事例的记录序号(sampling 记录的编号)。
      /// </summary>
      public const string FIELD_SP_SAMP_SEQ = "SP_SAMP_SEQ";
      /// <summary>
      /// 编辑人。
      /// </summary>
      public const string FIELD_EDITOR = "EDITOR";
      /// <summary>
      /// 编辑时间。
      /// </summary>
      public const string FIELD_EDIT_TIME = "EDIT_TIME";
      /// <summary>
      /// 数据库中的数据表名称。
      /// </summary>
      public const string DATABASE_TABLE_NAME = "EDC_COLLECTION_DATA";
      /// <summary>
      /// 在数据库中存储采集得到的设备数据的数据表名称。
      /// </summary>
      public override string TABLE_NAME
      {
          get
          {
              return DATABASE_TABLE_NAME;
          }
      }
      /// <summary>
      /// 数据库表中的字段集合
      /// </summary>
      public override Dictionary<string, FieldProperties> FIELDS
      {
          get
          {
              return _fields;
          }
      }
      /// <summary>
      /// 数据库表中的字段集合
      /// </summary>
      private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_SP_UNIT_SEQ, new FieldProperties() },
                                                                  {FIELD_PARAM_KEY, new FieldProperties() },
                                                                  {FIELD_EDC_INS_KEY, new FieldProperties() },
                                                                  {FIELD_PARAM_VALUE, new FieldProperties()},
                                                                  {FIELD_COL_KEY, new FieldProperties()},
                                                                  {FIELD_VALID_FLAG, new FieldProperties() },
                                                                  {FIELD_FAILED_RULES, new FieldProperties() },
                                                                  {FIELD_SP_SAMP_SEQ, new FieldProperties() },
                                                                  {FIELD_EDITOR, new FieldProperties() },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now)},                                                                  
                                                               };
    
    }
}
