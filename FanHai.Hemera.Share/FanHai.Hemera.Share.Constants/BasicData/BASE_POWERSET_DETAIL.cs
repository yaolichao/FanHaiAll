using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_POWERSET_DETAIL : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_POWERSET_KEY_DTL = "POWERSET_KEY_DTL";
        /// <summary>
        /// 外键值
        /// </summary>
        public const string FIELDS_POWERSET_KEY = "POWERSET_KEY";
        /// <summary>
        /// 分档代码
        /// </summary>
        public const string FIELDS_PS_DTL_SUBCODE = "PS_DTL_SUBCODE";
        /// <summary>
        /// 档位
        /// </summary>
        public const string FIELDS_POWERLEVEL = "POWERLEVEL";
        /// <summary>
        /// 最小值
        /// </summary>
        public const string FIELDS_P_DTL_MIN = "P_DTL_MIN";
        /// <summary>
        /// 最大值
        /// </summary>
        public const string FIELDS_P_DTL_MAX = "P_DTL_MAX";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
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
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";
       

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_POWERSET_DETAIL";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_POWERSET_DETAIL_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_POWERSET_DETAIL_INSERT";
        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
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
                                                                  {FIELDS_POWERSET_KEY_DTL, new FieldProperties() },
                                                                  {FIELDS_POWERSET_KEY, new FieldProperties() },
                                                                  {FIELDS_PS_DTL_SUBCODE, new FieldProperties() },
                                                                  {FIELDS_POWERLEVEL, new FieldProperties() },
                                                                  {FIELDS_P_DTL_MIN, new FieldProperties() },
                                                                  {FIELDS_P_DTL_MAX, new FieldProperties() },  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(int),false,1) }                                                                 
                                                                };
        #endregion
    }

    public class BASE_POWERSET_COLORATCNO : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_POWERSET_KEY_ATC = "POWERSET_KEY_ATC";
        /// <summary>
        /// 外键值
        /// </summary>
        public const string FIELDS_POWERSET_KEY = "POWERSET_KEY";
        /// <summary>
        /// 花色代码
        /// </summary>
        public const string FIELDS_COLOR_CODE = "COLOR_CODE";
        /// <summary>
        /// 花色名称
        /// </summary>
        public const string FIELDS_COLOR_NAME = "COLOR_NAME";
        /// <summary>
        /// ARTICNO
        /// </summary>
        public const string FIELDS_ARTICNO = "ARTICNO";
        /// <summary>
        /// 描述
        /// </summary>
        public const string FIELDS_DESCRIPTION = "DESCRIPTION";
        /// <summary>
        /// 等级（备用）
        /// </summary>
        public const string FIELDS_PRO_LEVEL = "PRO_LEVEL";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITER";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否可用(1:可用，0:不可用)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";


        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_POWERSET_COLORATCNO";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_POWERSET_COLORATCNO_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_POWERSET_COLORATCNO_INSERT";
        #endregion

        #region table
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
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
                                                                  {FIELDS_POWERSET_KEY_ATC, new FieldProperties() },
                                                                  {FIELDS_POWERSET_KEY, new FieldProperties() },
                                                                  {FIELDS_COLOR_CODE, new FieldProperties() },
                                                                  {FIELDS_COLOR_NAME, new FieldProperties() },
                                                                  {FIELDS_ARTICNO, new FieldProperties() },
                                                                  {FIELDS_DESCRIPTION, new FieldProperties() }, 
                                                                  {FIELDS_PRO_LEVEL,new FieldProperties()},
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},                                                                 
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                                              
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(int),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  