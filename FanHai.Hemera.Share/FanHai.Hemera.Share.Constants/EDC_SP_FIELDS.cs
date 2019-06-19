using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class EDC_SP_FIELDS:TableFields
    {
      #region columns define
      public const string FIELD_SP_KEY = "SP_KEY";		                //主键
      public const string FIELD_SP_NAME = "SP_NAME";		                //sampling名称
      public const string FIELD_SP_VERSION = "SP_VERSION";		            //sampling版本(默认1)，预留字段
      public const string FIELD_STATUS = "STATUS";	                    //0:InActive,1:Active,2:archive
      public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";	            //描述
      public const string FIELD_SAMPLING_TYPE = "SAMPLING_TYPE";	            //interLot:0 ,intraLot:1
      public const string FIELD_SAMPLING_MODE = "SAMPLING_MODE";	            //抽样类型:1:按抽样比例 2:指定一批抽多少个
      public const string FIELD_SAMPLING_SIZE = "SAMPLING_SIZE";	            //抽查数量或比例
      public const string FIELD_UNIT_SAMPLING_MODE = "UNIT_SAMPLING_MODE";	        //Wafer的模式
      public const string FIELD_UNIT_SAMPLING_SIZE = "UNIT_SAMPLING_SIZE";	        //Wafer需要抽查的点数
      public const string FIELD_PERCENT_FLAG = "PERCENT_FLAG";	            //Percent flag 0:No ,1:Yes
      public const string FIELD_MAX_SAMPLING_SIZE = "MAX_SAMPLING_SIZE";	        //最多抽几片
      public const string FIELD_STRATEGY_MODE = "STRATEGY_MODE";	            //抽样策略(同一订单，每隔多久抽查一次)，正常可以是每批都需要抽查
      public const string FIELD_STRATEGY_SIZE = "STRATEGY_SIZE";	            //抽样策略大小
      #endregion

      #region table name define
      public const string DATABASE_TABLE_NAME = "EDC_SP";      //TABLE NAME
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
                                                                 {FIELD_SP_KEY, new FieldProperties() },
                                                                 {FIELD_SP_NAME, new FieldProperties() },
                                                                 {FIELD_SP_VERSION, new FieldProperties() },
                                                                 {FIELD_STATUS, new FieldProperties() },
                                                                 {FIELD_DESCRIPTIONS, new FieldProperties() },
                                                                 {FIELD_SAMPLING_TYPE, new FieldProperties() },
                                                                 {FIELD_SAMPLING_MODE, new FieldProperties() },
                                                                 {FIELD_SAMPLING_SIZE, new FieldProperties() },
                                                                 {FIELD_UNIT_SAMPLING_MODE, new FieldProperties() },
                                                                 {FIELD_UNIT_SAMPLING_SIZE, new FieldProperties() },
                                                                 {FIELD_PERCENT_FLAG, new FieldProperties() },
                                                                 {FIELD_MAX_SAMPLING_SIZE, new FieldProperties() }, 
                                                                 {FIELD_STRATEGY_MODE, new FieldProperties() },
                                                                 {FIELD_STRATEGY_SIZE, new FieldProperties() }
                                                              };
      #endregion
    } 
}
