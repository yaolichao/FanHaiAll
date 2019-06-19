using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class SPC_CONTROL_PLAN_FIELD : TableFields
    {        
        /// <summary>
        /// 管控计划ID
        /// </summary>
        public const string FIELD_CONTROLPLANID = "CONTROLPLANID";
        /// <summary>
        /// 管控代码
        /// </summary>
        public const string FIELD_CONTROLCODE = "CONTROLCODE";
        /// <summary>
        /// 代码描述
        /// </summary>
        public const string FIELD_CODEDESC = "CODEDESC";
        /// <summary>
        /// 成品类型
        /// </summary>
        public const string FIELD_PRODUCTCODE = "PRODUCTCODE";
        /// <summary>
        /// 工厂
        /// </summary>
        public const string FIELD_WERKS = "WERKS";
        /// <summary>
        /// 区域ID
        /// </summary>
        public const string FIELD_AREAS_KEY = "AREAS_KEY";
        /// <summary>
        /// 线别ID
        /// </summary>
        public const string FIELD_LINE_KEY = "LINE_KEY";
        /// <summary>
        /// 工序ID
        /// </summary>
        public const string FIELD_STEP_KEY = "STEP_KEY";
        /// <summary>
        /// 参数ID
        /// </summary>
        public const string FIELD_PARAMENTID = "PARAMENTID";
        /// <summary>
        /// 样本控件
        /// </summary>
        public const string FIELD_SPECIMEN = "SPECIMEN";
        /// <summary>
        /// 控制图类型
        /// </summary>
        public const string FIELD_CONTROLTYPE = "CONTROLTYPE";
        /// <summary>
        /// 设备编号，多个设备用“，”分割
        /// </summary>
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 时间
        /// </summary>
        public const string FIELD_CTLDATE = "CTLDATE";
        /// <summary>
        /// 激活状态
        /// </summary>
        public const string FIELD_STATES = "STATES";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";
        /// <summary>
        /// 创建日期
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";
        /// <summary>
        /// 修改日期
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 异常规则代码集合
        /// </summary>
        public const string FIELD_ABNORMALIDS = "ABNORMALIDS";

        // <summary>
        // 是否选中了该笔资料，主要是页面用，和数据表没有联系
        // </summary>
        //public const string FIELD_SELECTKEY = "SELECTKEY";

        /// <summary>
        /// 新增/获得资料用到的表明
        /// </summary>
        public const string DATABASE_TABLE_NAME = "SPC_CONTROL_PLAN";
        /// <summary>
        /// 更新数据用到的表明
        /// </summary>
        public const string DATABASE_TABLE_NAME_FORUPDATE = "SPC_CONTROL_PLAN_FORUPDATE";

        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_AREAS_KEY, new FieldProperties() },
                                                                  {FIELD_CODEDESC, new FieldProperties() },
                                                                  {FIELD_CONTROLCODE, new FieldProperties() },
                                                                  {FIELD_CONTROLPLANID, new FieldProperties() },
                                                                  {FIELD_CONTROLTYPE, new FieldProperties() },
                                                                  {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_CREATOR, new FieldProperties() },
                                                                  {FIELD_CTLDATE, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_EDITOR, new FieldProperties() },
                                                                  {FIELD_LINE_KEY, new FieldProperties() },
                                                                  {FIELD_PARAMENTID, new FieldProperties() },
                                                                  {FIELD_PRODUCTCODE, new FieldProperties() },
                                                                  {FIELD_SPECIMEN, new FieldProperties() },
                                                                  {FIELD_STATES, new FieldProperties() },
                                                                  {FIELD_STEP_KEY, new FieldProperties() },
                                                                  {FIELD_WERKS, new FieldProperties() }                                                                 
                                                                };

    }
}
