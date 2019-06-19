using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// EMS_EQUIPMENTS FIELDS
    /// </summary>
    /// Owner:Andy Gao 2010-08-04 17:37:17
    public class EMS_EQUIPMENTS_FIELDS : COMMONABLE_FIELDS
    {
        public const string FIELD_EQUIPMENT_KEY = "EQUIPMENT_KEY";
        public const string FIELD_EQUIPMENT_NAME = "EQUIPMENT_NAME";
        public const string FIELD_DESCRIPTION = "DESCRIPTION";
        public const string FIELD_EQUIPMENT_TYPE = "EQUIPMENT_TYPE";
        public const string FIELD_EQUIPMENT_CODE = "EQUIPMENT_CODE";
        public const string FIELD_EQUIPMENT_MODE = "EQUIPMENT_MODE";
        public const string FIELD_MINQUANTITY = "MINQUANTITY";
        public const string FIELD_MAXQUANTITY = "MAXQUANTITY";
        public const string FIELD_EQUIPMENT_STATE_KEY = "EQUIPMENT_STATE_KEY";
        public const string FIELD_EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";
        public const string FIELD_EQUIPMENT_GROUP_KEY = "EQUIPMENT_GROUP_KEY";
        public const string FIELD_LOCATION_KEY = "LOCATION_KEY";
        public const string FIELD_PARENT_EQUIPMENT_KEY = "PARENT_EQUIPMENT_KEY";
        /// <summary>
        /// 是否腔体
        /// </summary>
        public const string FIELD_ISCHAMBER = "ISCHAMBER";
        public const string FIELD_ISBATCH = "ISBATCH"; 
        /// <summary>
        /// 是否多腔体设备
        /// </summary>
        public const string FIELD_ISMULTICHAMBER = "ISMULTICHAMBER";
        /// <summary>
        /// 腔体总数
        /// </summary>
        public const string FIELD_CHAMBER_TOTAL = "CHAMBER_TOTAL";
        /// <summary>
        /// 腔体编号
        /// </summary>
        public const string FIELD_CHAMBER_INDEX = "CHAMBER_INDEX";
        /// <summary>
        /// 设备WPH值（目标片数）
        /// </summary>
        public const string FIELD_EQUIPMENT_WPH = "EQUIPMENT_WPH";
        /// <summary>
        /// 设备目标AV Time
        /// </summary>
        public const string FIELD_EQUIPMENT_AV_TIME = "EQUIPMENT_AV_TIME";
        /// <summary>
        /// 该设备的TRACT_TIME
        /// </summary>
        public const string FIELD_EQUIPMENT_TRACT_TIME = "EQUIPMENT_TRACT_TIME";
        /// <summary>
        /// 固定资产编号
        /// </summary>
        public const string FIELD_EQUIPMENT_ASSETSNO = "EQUIPMENT_ASSETSNO";
        /// <summary>
        /// 虚拟设备对应的物理设备主键。
        /// </summary>
        public const string FIELD_EQUIPMENT_REAL_KEY = "EQUIPMENT_REAL_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_EQUIPMENTS";

        public EMS_EQUIPMENTS_FIELDS()
        {

        }

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
                                                                        { FIELD_EQUIPMENT_KEY, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_EQUIPMENT_NAME, new FieldProperties(typeof(string), false, "") }, 
                                                                        { FIELD_DESCRIPTION, new FieldProperties(typeof(string), true, "") }, 
                                                                        { FIELD_EQUIPMENT_TYPE, new FieldProperties(typeof(string), false, "") },
                                                                        { FIELD_EQUIPMENT_CODE, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EQUIPMENT_MODE, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_MINQUANTITY, new FieldProperties(typeof(int), true, 0) },
                                                                        { FIELD_MAXQUANTITY, new FieldProperties(typeof(int), true, 0) },
                                                                        { FIELD_EQUIPMENT_STATE_KEY, new FieldProperties(typeof(string), false, "") },
                                                                        { FIELD_EQUIPMENT_CHANGE_STATE_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EQUIPMENT_GROUP_KEY, new FieldProperties(typeof(string), false, "") },
                                                                        { FIELD_LOCATION_KEY, new FieldProperties(typeof(string), false, "") },
                                                                        { FIELD_PARENT_EQUIPMENT_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_ISCHAMBER, new FieldProperties(typeof(int), true, 0) },
                                                                        { FIELD_ISBATCH, new FieldProperties(typeof(int), true, 0) },
                                                                        { FIELD_CREATOR, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_CREATE_TIMEZONE_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EDITOR, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, "") },
                                                                        { FIELD_EDIT_TIMEZONE_KEY, new FieldProperties(typeof(string), true, "") },
                                                                        { FIELD_ISMULTICHAMBER ,new FieldProperties(typeof(int),true,0)},
                                                                        { FIELD_CHAMBER_TOTAL,new FieldProperties(typeof(int),true,0)},
                                                                        { FIELD_CHAMBER_INDEX,new FieldProperties(typeof(int),true,0)},
                                                                        { FIELD_EQUIPMENT_ASSETSNO,new FieldProperties(typeof(string),true,"")},
                                                                        { FIELD_EQUIPMENT_AV_TIME,new FieldProperties(typeof(string),true,"")},
                                                                        { FIELD_EQUIPMENT_TRACT_TIME,new FieldProperties(typeof(string),true,"")},
                                                                        { FIELD_EQUIPMENT_WPH,new FieldProperties(typeof(string),true,"")},
                                                                        { FIELD_EQUIPMENT_REAL_KEY,new FieldProperties(typeof(string),true,null)}
                                                                    };

        public static DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable(DATABASE_TABLE_NAME);

            dataTable.Columns.Clear();

            foreach (KeyValuePair<string, FieldProperties> keyValue in _fields)
            {
                dataTable.Columns.Add(keyValue.Key);
            }

            return dataTable;
        }
    }
}
