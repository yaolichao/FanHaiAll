using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 设备状态切换记录表
    /// </summary>
    public class EMS_STATE_EVENT_FIELDS : TableFields
    {
        /// <summary>
        /// 事件记录主键
        /// </summary>
        public const string EVENT_KEY = "EVENT_KEY";
        /// <summary>
        /// 设备号
        /// </summary>
        public const string EQUIPMENT_KEY = "EQUIPMENT_KEY";
        /// <summary>
        /// 事件号
        /// </summary>
        public const string EQUIPMENT_CHANGE_STATE_KEY = "EQUIPMENT_CHANGE_STATE_KEY";
        /// <summary>
        /// 当前事件
        /// </summary>
        public const string EQUIPMENT_FROM_STATE_KEY = "EQUIPMENT_FROM_STATE_KEY";
        /// <summary>
        /// 目标事件
        /// </summary>
        public const string EQUIPMENT_TO_STATE_KEY = "EQUIPMENT_TO_STATE_KEY";
        /// <summary>
        /// 备注
        /// </summary>
        public const string DESCRIPTION = "DESCRIPTION";
        /// <summary>
        /// 是否为当前状态 1：当前状态，0：历史状态
        /// </summary>
        public const string ISCURRENT = "ISCURRENT";
        /// <summary>
        /// 创建时间/切换开始时间
        /// </summary>
        public const string CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 创建人/状态切入人员
        /// </summary>
        public const string CREATOR = "CREATOR";
        /// <summary>
        /// 编辑时间/切换结束时间
        /// </summary>
        public const string EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 编辑人/状态切出人员
        /// </summary>
        public const string EDITOR = "EDITOR";
        /// <summary>
        /// 是否可用---预留
        /// </summary>
        public const string STATE = "STATE";

        public const string DATABASE_TABLE_NAME = "EMS_STATE_EVENT";

        public const string DATABASE_TABLE_NAME_UPDATE = "EMS_STATE_EVENT_UPDATE";

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
                                                                        {EVENT_KEY,new FieldProperties(typeof(string), false, "") }, 
                                                                        {EQUIPMENT_KEY,new FieldProperties(typeof(string), false, "") }, 
                                                                        {EQUIPMENT_CHANGE_STATE_KEY,new FieldProperties(typeof(string), false, "") }, 
                                                                        {EQUIPMENT_FROM_STATE_KEY,new FieldProperties(typeof(string), false, "") }, 
                                                                        {EQUIPMENT_TO_STATE_KEY,new FieldProperties(typeof(string), false, "") },                                                                         
                                                                        {DESCRIPTION,new FieldProperties(typeof(string), false, "") },
                                                                        {ISCURRENT,new FieldProperties(typeof(string), false, "")},                                                                  
                                                                        {CREATOR,new FieldProperties(typeof(string), false, "")},
                                                                        {EDIT_TIME,new FieldProperties(typeof(DateTime),false,DateTime.Now)},
                                                                        {EDITOR,new FieldProperties(typeof(string), false, "")},
                                                                        {STATE,new FieldProperties(typeof(string), false, "")}
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

    public class ColorType
    {
        public ColorType()
        { }
        /// <summary>
        /// RUN 运行状态 Lime
        /// </summary>
        public const string RUN = "RUN";
        /// <summary>
        /// LOST 空闲状态 Yellow
        /// </summary>
        public const string LOST = "LOST";//Yellow
        /// <summary>
        /// T_MD 跑档控片状态 Green
        /// </summary>
        public const string T_MD = "T_MD";//Green
        /// <summary>
        /// W_MF 等待生产部处理 Blue
        /// </summary>
        public const string W_MF = "W_MF";//Blue
        /// <summary>
        /// DOWN 设备故障停机  Red
        /// </summary>
        public const string DOWN = "DOWN";//Red
        /// <summary>
        /// CIMD 生产自动化控制故障停机 Fuchisia
        /// </summary>
        public const string CIMD = "CIMD";//Fuchsia
        /// <summary>
        /// W_EN 等待工程师处理 Cyan
        /// </summary>
        public const string W_EN = "W_EN";//Cyan
        /// <summary>
        /// FACD 外围造成停机 Olive
        /// </summary>
        public const string FACD = "FACD";//Olive
        /// <summary>
        /// PM 预防保养 Maroon
        /// </summary>
        public const string PM = "PM";//Maroon
        /// <summary>
        /// SETUP 切线 RoyalBlue
        /// </summary>
        public const string SETUP = "SETUP";//RoyalBlue
        /// <summary>
        /// TEST 工艺工程师测试时间 Orchid
        /// </summary>
        public const string TEST = "TEST";//Orchid
        /// <summary>
        /// OFF 设备不启用 DimGray
        /// </summary>
        public const string OFF = "OFF";//DimGray
        /// <summary>
        /// P_DOWN 生产计划停机 DarkOliveGreen
        /// </summary>
        public const string P_DOWN = "P_DOWN";//DarkOliveGreen
        /// <summary>
        /// MON 测机换液 OrangeRed
        /// </summary>
        public const string MON = "MON";//OrangeRed
        /// <summary>
        /// T_DOWN 测试停机 DarkOrange
        /// </summary>
        public const string T_DOWN = "T_DOWN";//DarkOrange
        /// <summary>
        /// 默认设备颜色
        /// </summary>
        public const string DEFAULT_COLOR = "DEFAULT_COLOR";//White

        public Dictionary<string, Color> EColor
        {
            get
            {
                return eColor;
            }
        }

        private Dictionary<string, Color> eColor = new Dictionary<string, Color>{
                                                                                {RUN,Color.Lime},
                                                                                {LOST,Color.Yellow},
                                                                                {T_MD,Color.Green},
                                                                                {W_MF,Color.Blue},
                                                                                {DOWN,Color.Red},
                                                                                {CIMD,Color.Fuchsia},
                                                                                {W_EN,Color.Cyan},
                                                                                {FACD,Color.Olive},
                                                                                {PM,Color.Maroon},
                                                                                {SETUP,Color.RoyalBlue},
                                                                                {TEST,Color.Orchid},
                                                                                {OFF,Color.DimGray},
                                                                                {P_DOWN,Color.DarkOliveGreen},
                                                                                {MON,Color.OrangeRed},
                                                                                {T_DOWN,Color.DarkOrange},
                                                                                {DEFAULT_COLOR,Color.White}
                                                                                };
        public Color GetStateColor(string sColor)
        {
            Color color = Color.White;
            foreach (KeyValuePair<string, Color> kvp in eColor)
            {
                if (kvp.Key == sColor)
                {
                    color = kvp.Value;
                    break;
                }
            }
            return color;                    
        }

    }
}
