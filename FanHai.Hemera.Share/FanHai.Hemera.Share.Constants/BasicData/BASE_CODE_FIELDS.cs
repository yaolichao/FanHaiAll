using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class BASE_CODE_FIELDS: TableFields 
    {
        /// <summary>
        /// 以下const常量值与数据表名对应
        /// </summary>
        public const string FIELDS_CODE_KEY = "CODE_KEY";
        public const string FIELDS_CODE_ID = "CODE_ID";
        public const string FIELDS_CODE_NAME = "CODE_NAME";
        public const string FIELDS_CODE_STATE = "CODE_STATE";
        public const string FIELDS_COMPUTER_NAME = "COMPUTER_NAME";
        public const string FIELDS_AREA_NO = "AREA_NO";
        public const string FIELDS_LINE_ID = "LINE_ID";
        public const string FIELDS_EQPT_ID = "EQPT_ID";
        public const string FIELDS_EXT_1 = "EXT_1";
        public const string FIELDS_EXT_2 = "EXT_2";
        public const string FIELDS_EXT_3 = "EXT_3";
        public const string FIELDS_EXT_4 = "EXT_4";
        public const string FIELDS_EXT_5 = "EXT_5";
        public const string FIELDS_EXT_6 = "EXT_6";
        public const string FIELDS_EXT_7 = "EXT_7";
        public const string FIELDS_EXT_8 = "EXT_8";
        public const string FIELDS_EXT_9 = "EXT_9";
        public const string FIELDS_EXT_10= "EXT_10";
        public const string FIELDS_EXT_11= "EXT_11";
        public const string FIELDS_EXT_12= "EXT_12";
        public const string FIELDS_EXT_13= "EXT_13";
        public const string FIELDS_EXT_14= "EXT_14";//打印机端口
        public const string FIELDS_EXT_15 = "EXT_15";//打印机类型

        public const string FIELDS_CODE_TIME = "CODE_TIME";

        public const string DATABASE_TABLE_NAME = "BASE_CODE";

        /// <summary>
        /// 实体类表明
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        /// <summary>
        /// 实体类字段集合
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        /// <summary>
        /// 私有变量，实体类字段集合
        /// </summary>
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                {FIELDS_CODE_KEY, new FieldProperties() },
                                                                {FIELDS_CODE_ID, new FieldProperties() },
                                                                {FIELDS_CODE_NAME, new FieldProperties() },
                                                                {FIELDS_CODE_STATE , new FieldProperties() },
                                                                {FIELDS_COMPUTER_NAME , new FieldProperties() },
                                                                {FIELDS_AREA_NO , new FieldProperties()  },
                                                                {FIELDS_LINE_ID , new FieldProperties() },
                                                                {FIELDS_EQPT_ID , new FieldProperties() },
                                                                {FIELDS_EXT_1 , new FieldProperties() },
                                                                {FIELDS_EXT_2 , new FieldProperties() },
                                                                {FIELDS_EXT_3 , new FieldProperties() },
                                                                {FIELDS_EXT_4 , new FieldProperties() },
                                                                {FIELDS_EXT_5 , new FieldProperties() },
                                                                {FIELDS_EXT_6 , new FieldProperties() },
                                                                {FIELDS_EXT_7 , new FieldProperties() },
                                                                {FIELDS_EXT_8 , new FieldProperties() },
                                                                {FIELDS_EXT_9,new FieldProperties()},
                                                                {FIELDS_EXT_10,new FieldProperties()},
                                                                {FIELDS_EXT_11,new FieldProperties()},
                                                                {FIELDS_EXT_12,new FieldProperties()},
                                                                {FIELDS_EXT_13,new FieldProperties()},
                                                                {FIELDS_EXT_14,new FieldProperties()},
                                                                {FIELDS_EXT_15,new FieldProperties()},
                                                                {FIELDS_CODE_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) }
                                                              };
    }
}
