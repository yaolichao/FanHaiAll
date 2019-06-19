using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// IV测试数据FIELDS
    /// </summary>
    /// genchille.yang 2012-11-19 16:51:28
    public class WIP_IV_TEST_FIELDS : TableFields
    {
        #region fields
        /// <summary>
        /// IV测试主键值
        /// </summary>
        public const string FIELDS_IV_TEST_KEY = "IV_TEST_KEY";
        /// <summary>
        /// 设备号
        /// </summary>
        public const string FIELDS_DEVICENUM = "DEVICENUM";
        /// <summary>
        /// 批次号
        /// </summary>
        public const string FIELDS_LOT_NUM = "LOT_NUM";
        /// <summary>
        /// 功率档位
        /// </summary>
        public const string FIELDS_PM = "PM";
        /// <summary>
        /// 短路电流
        /// </summary>
        public const string FIELDS_ISC = "ISC";
        /// <summary>
        /// 最大电流
        /// </summary>
        public const string FIELDS_IPM = "IPM";
        /// <summary>
        /// 开路电压
        /// </summary>
        public const string FIELDS_VOC = "VOC";
        /// <summary>
        /// 最大电压
        /// </summary>
        public const string FIELDS_VPM = "VPM";
        /// <summary>
        /// 测试日期
        /// </summary>
        public const string FIELDS_T_DATE = "T_DATE";
        /// <summary>
        /// 测试时间
        /// </summary>
        public const string FIELDS_TTIME = "TTIME";
        /// <summary>
        /// 打印标志
        /// </summary>
        public const string FIELDS_VC_PSIGN = "VC_PSIGN";
        /// <summary>
        /// 打印日期
        /// </summary>
        public const string FIELDS_DT_PRINTDT = "DT_PRINTDT";
        /// <summary>
        /// 有效值
        /// </summary>
        public const string FIELDS_VC_DEFAULT = "VC_DEFAULT";
        /// <summary>
        /// 测试温度
        /// </summary>
        public const string FIELDS_AMBIENTTEMP = "AMBIENTTEMP";
        /// <summary>
        /// 环境温度
        /// </summary>
        public const string FIELDS_SENSORTEMP = "SENSORTEMP";
        /// <summary>
        /// 填充因子
        /// </summary>
        public const string FIELDS_FF = "FF";
        /// <summary>
        /// 组件转换效率
        /// </summary>
        public const string FIELDS_EFF = "EFF";
        /// <summary>
        /// 并联电阻
        /// </summary>
        public const string FIELDS_RS = "RS";
        /// <summary>
        /// 串联电阻
        /// </summary>
        public const string FIELDS_RSH = "RSH";
        /// <summary>
        /// 光强
        /// </summary>
        public const string FIELDS_INTENSITY = "INTENSITY";

        public const string FIELDS_VC_INICUSTCODE = "VC_INICUSTCODE";
        /// <summary>
        /// 客户代码
        /// </summary>
        public const string FIELDS_VC_CUSTCODE = "VC_CUSTCODE";
        /// <summary>
        /// 打印次数
        /// </summary>
        public const string FIELDS_P_NUM = "P_NUM";
        /// <summary>
        /// 用户账号
        /// </summary>
        public const string FIELDS_C_USERID = "C_USERID";
        /// <summary>
        /// 数据记录序号
        /// </summary>
        public const string FIELDS_L_ID = "L_ID";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_DT_CREATE = "DT_CREATE";
        public const string FIELDS_C_PSTATE = "C_PSTATE";
        public const string FIELDS_VC_DATAGROUP = "VC_DATAGROUP";
        /// <summary>
        /// 衰减最大功率
        /// </summary>
        public const string FIELDS_COEF_PMAX = "COEF_PMAX";
        /// <summary>
        /// 衰减短路电流
        /// </summary>
        public const string FIELDS_COEF_ISC = "COEF_ISC";
        /// <summary>
        /// 衰减开路电压
        /// </summary>
        public const string FIELDS_COEF_VOC = "COEF_VOC";
        /// <summary>
        /// 衰减最大电流
        /// </summary>
        public const string FIELDS_COEF_IMAX = "COEF_IMAX";
        /// <summary>
        /// 衰减最大工作电压
        /// </summary>
        public const string FIELDS_COEF_VMAX = "COEF_VMAX";
        /// <summary>
        /// 衰减填充因子
        /// </summary>
        public const string FIELDS_COEF_FF = "COEF_FF";
        /// <summary>
        /// 分档规则代码
        /// </summary>
        public const string FIELDS_VC_TYPE = "VC_TYPE";
        /// <summary>
        /// 档位序号
        /// </summary>
        public const string FIELDS_I_IDE = "I_IDE";
        /// <summary>
        /// 档位名称
        /// </summary>
        public const string FIELDS_VC_MODNAME = "VC_MODNAME";
        public const string FIELDS_C_MAINPED = "C_MAINPED";
        public const string FIELDS_I_PSTATE = "I_PSTATE";
        public const string FIELDS_VC_PSTATE = "VC_PSTATE";
        /// <summary>
        /// 标签代码
        /// </summary>
        public const string FIELDS_VC_PRINTLABELID = "VC_PRINTLABELID";
        /// <summary>
        /// 测试规则代码
        /// </summary>
        public const string FIELDS_VC_COGCODE = "VC_COGCODE";
        /// <summary>
        /// 电池片转换效率
        /// </summary>
        public const string FIELDS_VC_CELLEFF = "VC_CELLEFF";
        /// <summary>
        /// CTM转换
        /// </summary>
        public const string FIELDS_DEC_CTM = "DEC_CTM";
        /// <summary>
        /// 子分档序号
        /// </summary>
        public const string FIELDS_I_PKID = "I_PKID";
        /// <summary>
        /// 功率差
        /// </summary>
        public const string FIELDS_DEC_PMCHANGE = "DEC_PMCHANGE";

        /// <summary>
        /// 工单号
        /// </summary>
        public const string FIELDS_VC_WORKORDER = "VC_WORKORDER";
        /// <summary>
        /// 组件花色
        /// </summary>
        public const string FIELDS_VC_COLOR = "VC_COLOR";
        /// <summary>
        /// 标签打印
        /// </summary>
        public const string FIELDS_PRINTEDLABLE = "PRINTEDLABLE";
        /// <summary>
        /// 铭牌打印
        /// </summary>
        public const string FIELDS_PRINTED_NP = "PRINTED_NP";

        /// <summary>
        /// 校准板序号
        /// </summary>
        public const string FIELDS_CALIBRATION_NO = "CALIBRATION_NO";
        /// <summary>
        /// 接线盒体料号
        /// </summary>
        public const string FIELDS_PARAM_VALUE = "PARAM_VALUE";
        /// <summary>
        /// 电流分档
        /// </summary>
        public const string FIELDS_POWERLEVEL = "POWERLEVEL";


        public const string DATABASE_TABLE_NAME = " WIP_IV_TEST";
        public const string DATABASE_TABLE_FORUPDATE = " WIP_IV_TESTUPDATE";
        public const string DATABASE_TABLE_FORINSERT = " WIP_IV_TEST_INSERT";

        public const string SaveFlag = "saveflag";
        public const string InsertFlag = "insert";
        public const string UpdateFlag = "update";

        #endregion

        #region table_name
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

        #region fields

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_IV_TEST_KEY, new FieldProperties() },
                                                                  {FIELDS_DEVICENUM, new FieldProperties() },
                                                                  {FIELDS_LOT_NUM, new FieldProperties() },
                                                                  {FIELDS_PM, new FieldProperties() },
                                                                  {FIELDS_ISC, new FieldProperties() },                                                                
                                                                  {FIELDS_IPM, new FieldProperties() },
                                                                  {FIELDS_VOC, new FieldProperties() },                                            
                                                                  {FIELDS_VPM, new FieldProperties() },
                                                                  {FIELDS_T_DATE, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_TTIME, new FieldProperties() },
                                                                  {FIELDS_VC_PSIGN,new FieldProperties()},
                                                                  {FIELDS_DT_PRINTDT,new FieldProperties()},
                                                                  {FIELDS_VC_DEFAULT,new FieldProperties()},
                                                                  {FIELDS_AMBIENTTEMP,new FieldProperties()},
                                                                  {FIELDS_SENSORTEMP,new FieldProperties()},
                                                                  {FIELDS_FF,new FieldProperties()},
                                                                  {FIELDS_EFF,new FieldProperties()},
                                                                  {FIELDS_RS,new FieldProperties()},
                                                                  {FIELDS_RSH,new FieldProperties()},
                                                                  {FIELDS_INTENSITY,new FieldProperties()},
                                                                  {FIELDS_VC_INICUSTCODE,new FieldProperties()},
                                                                  {FIELDS_VC_CUSTCODE,new FieldProperties()},
                                                                  {FIELDS_P_NUM,new FieldProperties()},
                                                                  {FIELDS_C_USERID,new FieldProperties()},
                                                                  {FIELDS_L_ID,new FieldProperties()},
                                                                  {FIELDS_DT_CREATE,new FieldProperties()},
                                                                  {FIELDS_C_PSTATE,new FieldProperties()},
                                                                  {FIELDS_VC_DATAGROUP,new FieldProperties()},
                                                                  {FIELDS_COEF_PMAX,new FieldProperties()},
                                                                  {FIELDS_COEF_ISC,new FieldProperties()},
                                                                  {FIELDS_COEF_VOC,new FieldProperties()},
                                                                  {FIELDS_COEF_IMAX,new FieldProperties()},
                                                                  {FIELDS_COEF_VMAX,new FieldProperties()},
                                                                  {FIELDS_COEF_FF,new FieldProperties()},
                                                                  {FIELDS_VC_TYPE,new FieldProperties()},
                                                                  {FIELDS_I_IDE,new FieldProperties()},
                                                                  {FIELDS_VC_MODNAME,new FieldProperties()},
                                                                  {FIELDS_C_MAINPED,new FieldProperties()},
                                                                  {FIELDS_I_PSTATE,new FieldProperties()},
                                                                  {FIELDS_VC_PSTATE,new FieldProperties()},
                                                                  {FIELDS_VC_PRINTLABELID,new FieldProperties()},
                                                                  {FIELDS_VC_COGCODE,new FieldProperties()},
                                                                  {FIELDS_VC_CELLEFF,new FieldProperties()},
                                                                  {FIELDS_DEC_CTM,new FieldProperties()},
                                                                  {FIELDS_I_PKID,new FieldProperties()},
                                                                  {FIELDS_DEC_PMCHANGE,new FieldProperties()},
                                                                  {FIELDS_VC_WORKORDER,new FieldProperties()},
                                                                  {FIELDS_VC_COLOR,new FieldProperties()},
                                                                  {FIELDS_PRINTEDLABLE,new FieldProperties()},
                                                                  {FIELDS_PRINTED_NP,new FieldProperties()},
                                                                  {FIELDS_CALIBRATION_NO,new FieldProperties()},
                                                                  {FIELDS_PARAM_VALUE,new FieldProperties()},
                                                                  {FIELDS_POWERLEVEL,new FieldProperties()},
                                                                };
        #endregion
    }

    public class WIP_IV_TEST_PRINTLOG_FIELDS : TableFields
    {
        #region

        public const string FIELDS_IV_TEST_DTL_KEY = "IV_TEST_DTL_KEY";

        public const string FIELDS_IV_TEST_KEY = "IV_TEST_KEY";
        /// <summary>
        /// 打印最大功率
        /// </summary>
        public const string FIELDS_DC_PM = "DC_PM";
        /// <summary>
        /// 打印开路电压
        /// </summary>
        public const string FIELDS_DC_VOC = "DC_VOC";
        /// <summary>
        /// 打印短路电流
        /// </summary>
        public const string FIELDS_DC_ISC = "DC_ISC";
        /// <summary>
        /// 打印最大电压
        /// </summary>
        public const string FIELDS_DC_VMP = "DC_VMP";
        /// <summary>
        /// 打印最大电流
        /// </summary>
        public const string FIELDS_DC_IMP = "DC_IMP";
        /// <summary>
        /// 打印时间
        /// </summary>
        public const string FIELDS_PRINTDATE = "PRINTDATE";
        /// <summary>
        /// 标签序号
        /// </summary>
        public const string FIELDS_LABELNO = "LABELNO";
        /// <summary>
        /// 创建人员
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";

        #endregion

        #region

        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_IV_TEST_PRINTLOG";
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        #endregion

        #region
        /// <summary>
        /// 获取数据库表中的字段集合
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
                                                                  {FIELDS_IV_TEST_DTL_KEY, new FieldProperties() },
                                                                  {FIELDS_IV_TEST_KEY, new FieldProperties() },
                                                                  {FIELDS_DC_PM, new FieldProperties() },
                                                                  {FIELDS_DC_VOC, new FieldProperties() },
                                                                  {FIELDS_DC_ISC, new FieldProperties() },
                                                                  {FIELDS_DC_VMP, new FieldProperties() },
                                                                  {FIELDS_DC_IMP, new FieldProperties() },
                                                                  {FIELDS_PRINTDATE, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_LABELNO, new FieldProperties() },
                                                                  {FIELDS_CREATER, new FieldProperties() }
                                                                };
        #endregion
    }
}
