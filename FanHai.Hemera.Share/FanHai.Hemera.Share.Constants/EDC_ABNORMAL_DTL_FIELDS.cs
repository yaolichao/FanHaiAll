#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion


namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 异常规则明细
    /// </summary>
    public class EDC_ABNORMAL_DTL_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_ABNORMALID_DTL = "ABNORMALID_DTL";
        public const string FIELD_ABNORMALID = "ABNORMALID";  
        public const string FIELD_OVERRULEPOINTS = "OVERRULEPOINTS";
        public const string FIELD_WATCHPOINTS    = "WATCHPOINTS";
        public const string FIELD_COMPARESIGN    = "COMPARESIGN";
        public const string FIELD_COMPARERULE    = "COMPARERULE";
        public const string FIELD_RULEVALUE      = "RULEVALUE";
        public const string FIELD_LVORM          = "LVORM";
        public const string FIELD_CREATOR        = "CREATOR";
        public const string FIELD_CREATE_TIME    = "CREATE_TIME";
        public const string FIELD_EDITOR         = "EDITOR";
        public const string FIELD_EDIT_TIME      = "EDIT_TIME";

        #endregion
        
        #region table name define
        public const string DATABASE_TABLE_NAME = "EDC_ABNORMALRULE_DTL";       //TABLE NAME

        public const string DATABASE_TABLE_NAME_FORUPDATE = "EDC_ABNORMALRULE_DTL_FORUPDATE";       //TABLE NAME
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
                                                                    {FIELD_ABNORMALID_DTL,new FieldProperties()},
                                                                    {FIELD_ABNORMALID, new FieldProperties() },        
                                                                    {FIELD_OVERRULEPOINTS, new FieldProperties() },
                                                                    {FIELD_WATCHPOINTS, new FieldProperties() }, 
                                                                    {FIELD_COMPARESIGN, new FieldProperties() },    
                                                                    {FIELD_COMPARERULE, new FieldProperties() },         
                                                                    {FIELD_RULEVALUE, new FieldProperties() },      
                                                                    {FIELD_LVORM, new FieldProperties() },  
                                                                    {FIELD_CREATOR, new FieldProperties() },    
                                                                    {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },   
                                                                    {FIELD_EDITOR, new FieldProperties() }, 
                                                                    {FIELD_EDIT_TIME,  new FieldProperties(typeof(DateTime), true, DateTime.Now)  }         
                                                              };
        #endregion
    }
    /// <summary>
    /// 比较符与比较规则定义
    /// </summary>
    /// Owner by Genchille.yang 2012-03-17 09:10:36
    public class SPC_ABNORMAL_COMPARE
    {
        /// <summary>
        /// 比较符 等于
        /// </summary>
        public const string SIGN_EQUAL = "EQUAL";
        /// <summary>
        ///  比较符 不等于
        /// </summary>
        public const string SIGN_NOTEQUAL = "NOTEQUAL";
        /// <summary>
        ///  比较符 大于
        /// </summary>
        public const string SIGN_GREATETHAN = "GREATETHAN";
        /// <summary>
        ///  比较符 大于等于
        /// </summary>
        public const string SIGN_GREATETHANOREQUAL = "GREATETHANOREQUAL";
        /// <summary>
        ///  比较符 小于
        /// </summary>
        public const string SIGN_LESSTHAN = "LESSTHAN";
        /// <summary>
        ///  比较符 小于等于
        /// </summary>
        public const string SIGN_LESSTHANOREQUAL = "LESSTHANOREQUAL";
        /// <summary>
        ///  比较符 递增
        /// </summary>
        public const string SIGN_INCREASING = "INCREASING";
        /// <summary>
        ///  比较符 递减
        /// </summary>
        public const string SIGN_DECREASING = "DECREASING";
        /// <summary>
        ///  比较符 在区间内
        /// </summary>
        public const string SIGN_INSIDE = "INSIDE";
        /// <summary>
        ///  比较符 在区间外
        /// </summary>
        public const string SIGN_OUTSIDE = "OUTSIDE";
        /// <summary>
        ///  比较符 交互
        /// </summary>
        public const string SIGN_ALTERNATING = "ALTERNATING";
        /// <summary>
        ///  比较符 直增
        /// </summary>
        public const string SIGN_STRICTLYINCREASING = "STRICTLYINCREASING";
        /// <summary>
        ///  比较符 直减
        /// </summary>
        public const string SIGN_STRICTLYDECREASING = "STRICTLYDECREASING";

        /// <summary>
        /// 上规格值
        /// </summary>
        public const string STANDARD_USL = "USL";
        /// <summary>
        /// 下规格值
        /// </summary>
        public const string STANDARD_LSL = "LSL";
        /// <summary>
        /// 上控制线
        /// </summary>
        public const string STANDARD_UCL = "UCL";
        /// <summary>
        /// 控制线
        /// </summary>
        public const string STANDARD_CL = "CL";
        /// <summary>
        /// 下控制线
        /// </summary>
        public const string STANDARD_LCL = "LCL";
        /// <summary>
        /// sigema
        /// </summary>
        public const string STANDARD_SIGMA = "SIGMA";
        /// <summary>
        /// A区间 2sigma-3sigma 双向对称
        /// </summary>
        public const string STANDARD_ZONEA = "ZONEA";
        /// <summary>
        /// B区间 sigma-2sigma 双向对称
        /// </summary>
        public const string STANDARD_ZONEB = "ZONEB";
        /// <summary>
        /// C区间 cl-sigma 双向对称
        /// </summary>
        public const string STANDARD_ZONEC = "ZONEC";
    }
}
