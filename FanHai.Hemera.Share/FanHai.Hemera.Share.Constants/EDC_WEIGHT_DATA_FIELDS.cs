using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class EDC_WEIGHT_DATA_FIELDS : TableFields
    {
        public const string FIELDS_WEIGHT_ROW_KEY = "WEIGHT_ROW_KEY";
        public const string FIELDS_WORK_ORDER_KEY = "WORK_ORDER_KEY";
        public const string FIELDS_ORDER_NUMBER = "ORDER_NUMBER";
        public const string FIELDS_ITEM_TYPE = "ITEM_TYPE";
        public const string FIELDS_ITEM_INDEX = "ITEM_INDEX";
        public const string FIELDS_ITEM_STATUS = "ITEM_STATUS";
        public const string FIELDS_LINE_NAME = "LINE_NAME";
        public const string FIELDS_LINE_KEY = "LINE_KEY";
        public const string FIELDS_LOT_KEY = "LOT_KEY";
        public const string FIELDS_LOT_NUMBER = "LOT_NUMBER";
        public const string FIELDS_SHIFT_NAME = "SHIFT_NAME";
        public const string FIELDS_ITEM_WEIGHT = "ITEM_WEIGHT";
        public const string FIELDS_ITEM_UNIT = "ITEM_UNIT";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string FIELDS_OPERATOR = "OPERATOR";
        public const string DATABASE_TABLE_NAME = "EDC_WEIGHT_DATA";

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
                                                                {FIELDS_WEIGHT_ROW_KEY, new FieldProperties() },
                                                                {FIELDS_WORK_ORDER_KEY, new FieldProperties() },
                                                                {FIELDS_ORDER_NUMBER, new FieldProperties() },
                                                                {FIELDS_ITEM_TYPE, new FieldProperties() },
                                                                {FIELDS_ITEM_INDEX , new FieldProperties() },  
                                                                {FIELDS_ITEM_STATUS, new FieldProperties() },
                                                                {FIELDS_LINE_NAME, new FieldProperties() },
                                                                {FIELDS_LINE_KEY, new FieldProperties() },
                                                                {FIELDS_LOT_KEY, new FieldProperties() },
                                                                {FIELDS_LOT_NUMBER, new FieldProperties() },
                                                                {FIELDS_SHIFT_NAME , new FieldProperties() },
                                                                {FIELDS_ITEM_WEIGHT, new FieldProperties() },
                                                                {FIELDS_ITEM_UNIT, new FieldProperties() },
                                                                {FIELDS_EDITOR, new FieldProperties() },
                                                                {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                {FIELDS_EDIT_TIMEZONE ,new FieldProperties() },
                                                                {FIELDS_OPERATOR,new FieldProperties()}
                                                              };                                                              
    }
}
