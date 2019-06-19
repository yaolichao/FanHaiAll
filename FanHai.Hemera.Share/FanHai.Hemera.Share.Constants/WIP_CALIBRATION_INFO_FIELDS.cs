using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 描述设备对应校准板信息数据库表字段实体类。
    /// </summary>
    public class WIP_CALIBRATION_INFO_FIELDS : TableFields
    {
        /// <summary>
        /// 设备代码。
        /// </summary>
        public const string FIELDS_MACHINE_NO = "MACHINE_NO";
        /// <summary>
        /// 校准板号。
        /// </summary>
        public const string FIELDS_CALIBRATION_NO = "CALIBRATION_NO";
        /// <summary>
        /// 校准时间。
        /// </summary>
        public const string FIELDS_CALIBRATION_TIME = "CALIBRATION_TIME";
        /// <summary>
        /// 数据库表名。
        /// </summary>
        public const string DATABASE_TABLE_NAME = "WIP_CALIBRATION_INFO"; 
        /// <summary>
        /// 返回数据库表名。
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        /// <summary>
        /// 返回数据库表字段集合。
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                 {FIELDS_MACHINE_NO , new FieldProperties() },                     
                                                                 {FIELDS_CALIBRATION_NO , new FieldProperties() },
                                                                 {FIELDS_CALIBRATION_TIME , new FieldProperties() }
                                                               };
    }   
}
