using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 终检数据表
    /// </summary>
    /// genchille.yang 2012-11-19 16:51:28
    public class WIP_CUSTCHECK_FIELDS : TableFields
    {
        #region fields
        /// <summary>
        /// 主键值
        /// </summary>
        public const string FIELDS_CUSTCHECK_KEY = "CUSTCHECK_KEY";
        /// <summary>
        /// 批次类型
        /// </summary>
        public const string FIELDS_LOT_TYPE = "LOT_TYPE";
        /// <summary>
        /// 组别
        /// </summary>
        public const string FIELDS_CC_DATA_GROUP = "CC_DATA_GROUP";
        /// <summary>
        /// 客户序号
        /// </summary>
        public const string FIELDS_CUSTOMCODE = "CUSTOMCODE";
        /// <summary>
        /// 记录序号
        /// </summary>
        public const string FIELDS_l_ID = "l_ID";
        /// <summary>
        /// 检验时间
        /// </summary>
        public const string FIELDS_CHECK_TIME = "CHECK_TIME";
        /// <summary>
        /// 内部工厂编号
        /// </summary>
        public const string FIELDS_CC_FCODE = "CC_FCODE";
        /// <summary>
        /// 内部工厂编号1
        /// </summary>
        public const string FIELDS_CC_FCODE1 = "CC_FCODE1";
        /// <summary>
        /// 内部工厂编号2
        /// </summary>
        public const string FIELDS_CC_FCODE2 = "CC_FCODE2";
        /// <summary>
        /// 内部工厂编号3
        /// </summary>
        public const string FIELDS_CC_FCODE3 = "CC_FCODE3";
        /// <summary>
        /// 内部工厂编号4
        /// </summary>
        public const string FIELDS_CC_FCODE4 = "CC_FCODE4";
        /// <summary>
        /// 内部工厂编号5
        /// </summary>
        public const string FIELDS_CC_FCODE5 = "CC_FCODE5";
        /// <summary>
        /// 内部工厂编号6
        /// </summary>
        public const string FIELDS_CC_FCODE6 = "CC_FCODE6";
        /// <summary>
        /// 工单号
        /// </summary>
        public const string FIELDS_WORKNUMBER = "WORKNUMBER";
        /// <summary>
        /// 铭牌编码
        /// </summary>
        public const string FIELDS_NAMEPLATENO = "NAMEPLATENO";
        /// <summary>
        /// 检验功率
        /// </summary>
        public const string FIELDS_CHECK_POWER = "CHECK_POWER";
        /// <summary>
        /// 作业人员
        /// </summary>
        public const string FIELDS_OPERATERS = "OPERATERS";
        /// <summary>
        /// 机台号
        /// </summary>
        public const string FIELDS_DEVICENUM = "DEVICENUM";
        /// <summary>
        /// 产品ID号
        /// </summary>
        public const string FIELDS_PRO_ID = "PRO_ID";
        /// <summary>
        /// 组件花色
        /// </summary>
        public const string FIELDS_LOT_COLOR = "LOT_COLOR";
        /// <summary>
        /// 产品等级
        /// </summary>
        public const string FIELDS_PRO_LEVEL = "PRO_LEVEL";
        /// <summary>
        /// 车间ID
        /// </summary>
        public const string FIELDS_ROOM_KEY = "ROOM_KEY";
        /// <summary>
        /// 班别主键
        /// </summary>
        public const string FIELDS_SHIFT_KEY = "SHIFT_KEY";
        /// <summary>
        /// 班别名称
        /// </summary>
        public const string FIELDS_SHIFT_NAME = "SHIFT_NAME";
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
        public const string FIELDS_EDITOR = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 是否有效(1:有效，0:无效)
        /// </summary>
        public const string FIELDS_ISFLAG = "ISFLAG";

        public static string TrackInOperation = string.Empty;

        public const string FIELDS_REMARK = "REMARK";
        public const string FIELDS_REASON_CODE_NAME = "REASON_CODE_NAME";
        public const string FIELDS_REASON_CODE_KEY = "REASON_CODE_KEY";
        public const string FIELDS_REASON_CODE_CATEGORY_KEY = "REASON_CODE_CATEGORY_KEY";
        public const string FIELDS_REASON_CODE_CATEGORY_NAME = "REASON_CODE_CATEGORY_NAME";



        public const string DATABASE_TABLE_NAME = " WIP_CUSTCHECK";
        public const string DATABASE_TABLE_FORUPDATE = " WIP_CUSTCHECK_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = " WIP_CUSTCHECK_INSERT";

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
                                                                  {FIELDS_CUSTCHECK_KEY,new FieldProperties()},
                                                                  {FIELDS_LOT_TYPE,new FieldProperties()},
                                                                  {FIELDS_CC_DATA_GROUP,new FieldProperties(typeof(Int16),false,0)},
                                                                  {FIELDS_CUSTOMCODE,new FieldProperties()},
                                                                  {FIELDS_l_ID,new FieldProperties()},
                                                                  {FIELDS_CHECK_TIME,new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_CC_FCODE,new FieldProperties()},
                                                                  {FIELDS_CC_FCODE1,new FieldProperties()},
                                                                  {FIELDS_CC_FCODE2,new FieldProperties()},
                                                                  {FIELDS_CC_FCODE3,new FieldProperties()},
                                                                  {FIELDS_CC_FCODE4,new FieldProperties()},
                                                                  {FIELDS_CC_FCODE5,new FieldProperties()},
                                                                  {FIELDS_CC_FCODE6,new FieldProperties()},
                                                                  {FIELDS_NAMEPLATENO,new FieldProperties()},
                                                                  {FIELDS_CHECK_POWER,new FieldProperties()},
                                                                  {FIELDS_WORKNUMBER,new FieldProperties()},
                                                                  {FIELDS_OPERATERS,new FieldProperties()},
                                                                  {FIELDS_DEVICENUM,new FieldProperties()},
                                                                  {FIELDS_PRO_ID,new FieldProperties()},
                                                                  {FIELDS_ROOM_KEY,new FieldProperties()},
                                                                  {FIELDS_LOT_COLOR,new FieldProperties()},
                                                                  {FIELDS_PRO_LEVEL,new FieldProperties()},
                                                                  {FIELDS_SHIFT_KEY,new FieldProperties()},
                                                                  {FIELDS_SHIFT_NAME,new FieldProperties()},
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_EDITOR,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME,new FieldProperties(typeof(DateTime),true, DateTime.Now)},
                                                                  {FIELDS_ISFLAG,new FieldProperties(typeof(Int16),false,1)},
                                                                  {FIELDS_REMARK,new FieldProperties()},
                                                                  {FIELDS_REASON_CODE_NAME,new FieldProperties()},
                                                                  {FIELDS_REASON_CODE_KEY,new FieldProperties()},
                                                                  {FIELDS_REASON_CODE_CATEGORY_KEY,new FieldProperties()},
                                                                  {FIELDS_REASON_CODE_CATEGORY_NAME,new FieldProperties()}
                                                                };
        #endregion
    }


}
