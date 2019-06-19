using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 表示数据库表CAL_SHIFT（班次表）中字段名称的实体类。
    /// </summary>
    public class CAL_SHIFT : TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_SHIFT_KEY = "SHIFT_KEY";
        /// <summary>
        /// 班别名。
        /// </summary>
        public const string FIELD_SHIFT_NAME = "SHIFT_NAME";
        /// <summary>
        /// 开始时间。
        /// </summary>
        public const string FIELD_START_TIME = "START_TIME";
        /// <summary>
        /// 结束时间。
        /// </summary>
        public const string FIELD_END_TIME = "END_TIME";
        /// <summary>
        /// 是否跨天。0：不跨天,1:跨天。
        /// </summary>
        public const string FIELD_OVER_DAY = "OVER_DAY";
        /// <summary>
        /// 描述
        /// </summary>
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";
        /// <summary>
        /// 创建者
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";                                      //创建者
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";                              //创建时间
        /// <summary>
        /// 编辑者
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";                                        //编辑者
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";                                  //编辑时间
        /// <summary>
        /// 创建时间时区
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";              //创建时间时区
        /// <summary>
        /// 编辑时间时区
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";                  //编辑时间时区
        /// <summary>
        /// SHIFT的开始日期(暂不用)
        /// </summary>
        public const string FIELD_START_DAY_OF_SHIFT = "START_DAY_OF_SHIFT";
        /// <summary>
        /// 周期时间(暂不用)
        /// </summary>
        public const string FIELD_CYCLE_DAYS = "CYCLE_DAYS";
        /// <summary>
        /// SHIFT的类型,PRODUCTIONTIME etc.(暂不用)
        /// </summary>
        public const string FIELD_SHIFT_TYPE = "SHIFT_TYPE";
       

        #endregion

        #region table name define
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "CAL_SHIFT";       //TABLE NAME
        #endregion

        #region get TABLE_NAME

        /// <summary>
        /// 数据库表名称
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
        /// 数据库表字段名称集合
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
                                                                {FIELD_SHIFT_KEY, new FieldProperties() },
                                                                {FIELD_SHIFT_NAME, new FieldProperties() },
                                                                {FIELD_START_TIME, new FieldProperties() },
                                                                {FIELD_END_TIME, new FieldProperties() },
                                                                {FIELD_OVER_DAY, new FieldProperties() },
                                                                {FIELD_DESCRIPTIONS, new FieldProperties() },
                                                                {FIELD_CREATOR, new FieldProperties(typeof(string),true,"") },                                                                
                                                                {FIELD_CREATE_TIMEZONE, new FieldProperties(typeof(string),true,"") },
                                                                {FIELD_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                {FIELD_EDITOR, new FieldProperties(typeof(string),true,"") },                                                               
                                                                {FIELD_EDIT_TIMEZONE, new FieldProperties(typeof(string),true,"")},
                                                                {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now)},

                                                          };
        #endregion


        /// <summary>
        /// 创建数据表对象，仅包含数据表结构。
        /// </summary>
        /// <returns>数据表对象。</returns>
        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(CAL_SHIFT.DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys)
            {
                if (key == FIELD_EDIT_TIME || key == FIELD_CREATE_TIME)
                    continue;                
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn);
            }
            dColumn = new DataColumn(CAL_SCHEDULE.FIELD_SCHEDULE_KEY);
            dTable.Columns.Add(dColumn);
            return dTable;
        }

    }
    /// <summary>
    /// 表示数据库表CAL_SCHEDULE（排班计划）中字段名称的实体类
    /// </summary>
    public class CAL_SCHEDULE : TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_SCHEDULE_KEY = "SCHEDULE_KEY";
        /// <summary>
        /// 计划名称
        /// </summary>
        public const string FIELD_SCHEDULE_NAME = "SCHEDULE_NAME";
        /// <summary>
        /// 描述
        /// </summary>
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";
        /// <summary>
        /// 最大跨度（不用）
        /// </summary>
        public const string FIELD_MAXOVERLAPTIME = "MAXOVERLAPTIME";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELD_CREATOR = "CREATOR";                                      //创建者
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELD_CREATE_TIME = "CREATE_TIME";                              //创建时间
        /// <summary>
        /// 编辑人
        /// </summary>
        public const string FIELD_EDITOR = "EDITOR";                                        //编辑者
        /// <summary>
        /// 编辑时间
        /// </summary>
        public const string FIELD_EDIT_TIME = "EDIT_TIME";                                  //编辑时间
        /// <summary>
        /// 创建时间区域
        /// </summary>
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";              //创建时间时区
        /// <summary>
        /// 编辑时间区域
        /// </summary>
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";                  //编辑时间时区    


        #endregion

        #region table name define
        /// <summary>
        /// 数据库表名。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "CAL_SCHEDULE";       //TABLE NAME
        #endregion

        #region get TABLE_NAME

        /// <summary>
        /// 数据库表名称
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
        /// 数据库表字段名称集合
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
                                                            {FIELD_SCHEDULE_KEY, new FieldProperties() },
                                                            {FIELD_SCHEDULE_NAME, new FieldProperties() },
                                                            {FIELD_DESCRIPTIONS, new FieldProperties() },
                                                            {FIELD_MAXOVERLAPTIME, new FieldProperties(typeof(int),false,0) },
                                                            {FIELD_CREATOR, new FieldProperties(typeof(string),true,"") },
                                                            {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                            {FIELD_CREATE_TIMEZONE, new FieldProperties(typeof(string),true,"") },
                                                            {FIELD_EDITOR, new FieldProperties(typeof(string),true,"") },
                                                            {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                            {FIELD_EDIT_TIMEZONE, new FieldProperties(typeof(string),true,"")},
                                                          };
        #endregion

        /// <summary>
        /// 创建数据表对象，仅包含数据表结构。
        /// </summary>
        /// <returns>数据表对象。</returns>
        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(CAL_SCHEDULE.DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys)
            {
                if (key == FIELD_EDIT_TIME || key == FIELD_CREATE_TIME)
                    continue;
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn);
            }
            return dTable;
        }

    }
    /// <summary>
    /// 表示数据库表CAL_SCHEDULE_MONTH（存放每月排班计划）中字段名称的实体类
    /// </summary>
    public class CAL_SCHEDULE_MONTH : TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_MKEY = "MKEY";
        /// <summary>
        /// 年，2012
        /// </summary>
        public const string FIELD_CUR_YEAR = "CUR_YEAR";
        /// <summary>
        /// 月，02
        /// </summary>
        public const string FIELD_CUR_MONTH = "CUR_MONTH ";
        /// <summary>
        /// 排班计划主键。
        /// </summary>
        public const string FIELD_SCHEDULE_KEY = "SCHEDULE_KEY";    
        #endregion

        #region table name define
        /// <summary>
        /// 数据库表名称。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "CAL_SCHEDULE_MONTH";       //TABLE NAME
        #endregion

        #region get TABLE_NAME

        /// <summary>
        /// 数据库表名称
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
        /// 数据库表字段名称集合
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
                                                                {FIELD_MKEY, new FieldProperties() },
                                                                {FIELD_CUR_YEAR, new FieldProperties() },
                                                                {FIELD_CUR_MONTH, new FieldProperties() },
                                                                {FIELD_SCHEDULE_KEY, new FieldProperties() },                                                             
                                                          };
        #endregion
        /// <summary>
        /// 创建数据表对象，仅包含数据表结构。
        /// </summary>
        /// <returns>数据表对象。</returns>
        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(CAL_SCHEDULE_MONTH.DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys)
            {               
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn);
            }
            return dTable;
        }
    }
    /// <summary>
    /// 表示数据库表CAL_SCHEDULE_DAY（存放每天排班计划）中字段名称的实体类
    /// </summary>
    public class CAL_SCHEDULE_DAY : TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_DKEY = "DKEY";
        /// <summary>
        /// 所属月份的主键
        /// </summary>
        public const string FIELD_MKEY = "MKEY";
        /// <summary>
        /// 开始时间，8:00
        /// </summary>
        public const string FIELD_STARTTIME = "STARTTIME ";
        /// <summary>
        /// 结束时间，20:00
        /// </summary>
        public const string FIELD_ENDTIME = "ENDTIME";
        /// <summary>
        /// 班别值
        /// </summary>
        public const string FIELD_SHIFT_VALUE = "SHIFT_VALUE";
        /// <summary>
        /// 序号
        /// </summary>
        public const string FIELD_SEQNO = "SEQNO";
        /// <summary>
        /// 班别主键
        /// </summary>
        public const string FIELD_SHIFT_KEY = "SHIFT_KEY";
        /// <summary>
        /// 日期，2012-01-27
        /// </summary>
        public const string FIELD_DAY = "DAY";
        #endregion

        #region table name define
        /// <summary>
        /// 数据库表名
        /// </summary>
        public const string DATABASE_TABLE_NAME = "CAL_SCHEDULE_DAY";       //TABLE NAME
        #endregion

        #region get TABLE_NAME

        /// <summary>
        /// 数据库表名称
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
        /// 数据库表字段名称集合
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
                                                                {FIELD_DKEY, new FieldProperties() },
                                                                {FIELD_MKEY, new FieldProperties() },
                                                                {FIELD_STARTTIME,new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                {FIELD_ENDTIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }, 
                                                                {FIELD_SHIFT_VALUE, new FieldProperties() },
                                                                {FIELD_SEQNO, new FieldProperties() },
                                                                {FIELD_SHIFT_KEY, new FieldProperties() },
                                                                {FIELD_DAY, new FieldProperties() },
                                                          };
        #endregion

        /// <summary>
        /// 创建数据表对象，仅包含数据表结构。
        /// </summary>
        /// <returns>数据表对象。</returns>
        public static DataTable CreateDataTable()
        {
            DataTable dTable = new DataTable(CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME);
            DataColumn dColumn;
            foreach (string key in _fields.Keys)
            {
                if (key == FIELD_MKEY)
                    continue;
                dColumn = new DataColumn(key);
                dTable.Columns.Add(dColumn);
            }
            return dTable;
        }
    }
}
