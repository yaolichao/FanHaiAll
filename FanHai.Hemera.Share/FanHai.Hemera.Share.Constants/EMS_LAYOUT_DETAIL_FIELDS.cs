using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class EMS_LAYOUT_DETAIL_FIELDS:TableFields
    {
        public const string LAYOUT_KEY = "LAYOUT_KEY";
        public const string EQUIPMENT_KEY = "EQUIPMENT_KEY";
        public const string EQUIPMENT_NAME = "EQUIPMENT_NAME";
        public const string PIC_LEFT = "PIC_LEFT";
        public const string PIC_TOP = "PIC_TOP";
        public const string PIC_WIDTH = "PIC_WIDTH";
        public const string PIC_HEIGHT = "PIC_HEIGHT";
        public const string PIC_TYPE = "PIC_TYPE";
        public const string FLAG = "FLAG";
        public const string EDITOR = "EDITOR";
        //public const string EDIT_TIME = "EDIT_TIME";
        /// <summary>
        /// 明细表主键
        /// </summary>
        public const string DETAIL_COL_KEY = "DETAIL_COL_KEY";

        public const string DATABASE_TABLE_NAME = "EMS_LAYOUT_DETAIL";

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
                                                                        {LAYOUT_KEY,new FieldProperties() }, 
                                                                        {EQUIPMENT_NAME,new FieldProperties()},
                                                                        {EQUIPMENT_KEY,new FieldProperties() }, 
                                                                        {PIC_LEFT,new FieldProperties() }, 
                                                                        {PIC_TOP,new FieldProperties() }, 
                                                                        {PIC_WIDTH,new FieldProperties() }, 
                                                                        {PIC_HEIGHT ,new FieldProperties()}, 
                                                                        {PIC_TYPE,new FieldProperties() },
                                                                        {DETAIL_COL_KEY,new FieldProperties()},
                                                                        {FLAG,new FieldProperties()}
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
