using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class BATTERY_PACKET_FIELDS:TableFields 
    {
        public const string FIELDS_PACKETNUMBER = "PACKETNUMBER";
        public const string FIELDS_NUMS = "NUMS";
        public const string FIELDS_LOT = "LOT";
        public const string FIELDS_PACKETTIME = "  PACKETTIME";
        public const string FIELDS_COLOR = "COLOR";
        public const string FIELDS_USED = "USED";
        public const string FIELDS_CLASS = "CLASS";
        public const string FIELDS_REMAINING = "REMAINING";
        public const string FIELDS_LINE = "LINE";
        public const string FIELDS_COMMENTS = "COMMENTS";
        public const string FIELDS_RULENUMBER = "RULENUMBER";
        public const string FIELDS_THICKNESS = "THICKNESS";
        public const string FIELD_EDIT_TIME = "EDIT_TIME";
        public const string FIELD_PRINT_FLAG = "PRINT_FLAG";
        public const string FIELD_PACKET_TYPE = "PACKET_TYPE";
        public const string FIELD_PRINT_TYPE = "PRINT_TYPE";

        public const string DATABASE_TABLE_NAME = "BATTERY_PACKET";

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
                                                                  {FIELDS_PACKETNUMBER, new FieldProperties() },
                                                                  {FIELDS_NUMS, new FieldProperties() }, 
                                                                  {FIELDS_LOT, new FieldProperties() },                                                                                                                                   
                                                                  {FIELDS_PACKETTIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_COLOR, new FieldProperties() },
                                                                  {FIELDS_USED, new FieldProperties() }, 
                                                                  {FIELDS_CLASS, new FieldProperties() },  
                                                                  {FIELDS_REMAINING, new FieldProperties() },                                                                  
                                                                  {FIELDS_LINE, new FieldProperties() },                                                                  
                                                                  {FIELDS_COMMENTS, new FieldProperties() }, 
                                                                  {FIELDS_RULENUMBER, new FieldProperties() }, 
                                                                  {FIELDS_THICKNESS, new FieldProperties() },
                                                                  {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELD_PRINT_FLAG,new FieldProperties()},
                                                                  {FIELD_PACKET_TYPE,new FieldProperties()},
                                                                  {FIELD_PRINT_TYPE,new FieldProperties()}
                                                              };

    }

}
