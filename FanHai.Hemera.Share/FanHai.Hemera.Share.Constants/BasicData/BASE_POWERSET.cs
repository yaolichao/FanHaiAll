using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_POWERSET : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_POWERSET_KEY = "POWERSET_KEY";
        /// <summary>
        /// 分档代码
        /// </summary>
        public const string FIELDS_PS_CODE = "PS_CODE";
        /// <summary>
        /// 分档规则
        /// </summary>
        public const string FIELDS_PS_RULE = "PS_RULE";
        /// <summary>
        /// 序号
        /// </summary>
        public const string FIELDS_PS_SEQ = "PS_SEQ";
        /// <summary>
        /// 功率下线
        /// </summary>
        public const string FIELDS_P_MIN = "P_MIN";
        /// <summary>
        /// 功率上线
        /// </summary>
        public const string FIELDS_P_MAX = "P_MAX";
        /// <summary>
        /// 产品类型
        /// </summary>
        public const string FIELDS_PROMODEL_KEY = "PROMODEL_KEY";
        /// <summary>
        /// 档位名称
        /// </summary>
        public const string FIELDS_MODULE_NAME = "MODULE_NAME";
        /// <summary>
        /// 功率档位
        /// </summary>
        public const string FIELDS_PMAXSTAB = "PMAXSTAB";
        /// <summary>
        /// 短路电流
        /// </summary>
        public const string FIELDS_ISCSTAB = "ISCSTAB";
        /// <summary>
        /// 开路电压
        /// </summary>
        public const string FIELDS_VOCSTAB = "VOCSTAB";
        /// <summary>
        /// 最大额定电流
        /// </summary>
        public const string FIELDS_IMPPSTAB = "IMPPSTAB";
        /// <summary>
        /// 最大额定电压
        /// </summary>
        public const string FIELDS_VMPPSTAB = "VMPPSTAB";
        /// <summary>
        /// Fuse
        /// </summary>
        public const string FIELDS_FUSE = "FUSE";
        /// <summary>
        /// 分档编码
        /// </summary>
        public const string FIELDS_PS_SUBCODE = "PS_SUBCODE";
        /// <summary>
        /// 分档编码描述
        /// </summary>
        public const string FIELDS_PS_SUBCODE_DESC = "PS_SUBCODE_DESC";
        /// <summary>
        /// 子分档方式
        /// </summary>
        public const string FIELDS_SUB_PS_WAY = "SUB_PS_WAY";
        /// <summary>
        /// 功率差
        /// </summary>
        public const string FIELDS_POWER_DIFFERENCE = "POWER_DIFFERENCE";
        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_ISFLAG = "ISFLAG";

        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_POWERSET";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_POWERSET_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_POWERSET_INSERT";

        #region 常用属性参数设置
        public const string TESTRULE_LASTTESTTYPE = "TestRule_LastTestType";
        public const string POWERSET_SUBTYPE = "PowerSet_SubType";
        public const string TESTRULE_CONTROLPARA = "TestRule_ControlPara";
        public const string TESTRULE_POWERCONTROL = "TestRule_PowerControl";
        public const string TESTRULE_SIGN = "TestRule_Sign";
        public const string PRODUCTLABELTYPE = "LabelType";
        public const string PRODUCT_GRADE = "ProductGrade";
        public const string BASE_DECAYSETTING = "BaseDecaySetting";
        public const string PRODUCTGRADE = "ProductGrade";
        public const string MODELCOLOR = "ModelColor";
        public const string MEMO_TYPE = "MEMO_TYPE";
        #endregion
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
                                                                  {FIELDS_POWERSET_KEY, new FieldProperties() },
                                                                  {FIELDS_PS_CODE, new FieldProperties() },
                                                                  {FIELDS_PS_RULE, new FieldProperties() },
                                                                  {FIELDS_PS_SEQ, new FieldProperties() },
                                                                  {FIELDS_P_MIN, new FieldProperties() },
                                                                  {FIELDS_P_MAX, new FieldProperties() },
                                                                  {FIELDS_PROMODEL_KEY, new FieldProperties() },
                                                                  {FIELDS_MODULE_NAME, new FieldProperties() },                                                                    
                                                                  {FIELDS_PMAXSTAB, new FieldProperties() },
                                                                  {FIELDS_ISCSTAB, new FieldProperties() },
                                                                  {FIELDS_VOCSTAB, new FieldProperties() },
                                                                  {FIELDS_IMPPSTAB, new FieldProperties() },
                                                                  {FIELDS_VMPPSTAB, new FieldProperties() },
                                                                  {FIELDS_FUSE, new FieldProperties() }, 
                                                                  {FIELDS_PS_SUBCODE, new FieldProperties() },
                                                                  {FIELDS_PS_SUBCODE_DESC, new FieldProperties() },
                                                                  {FIELDS_SUB_PS_WAY, new FieldProperties() },
                                                                  {FIELDS_POWER_DIFFERENCE, new FieldProperties() }, 
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(int),false,1) }                                                                 
                                                                };
        #endregion
    }
}

  