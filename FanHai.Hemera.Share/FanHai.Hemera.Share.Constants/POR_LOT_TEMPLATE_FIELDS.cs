using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_LOT_TEMPLATE_FIELDS:TableFields
    {
        public const string FIELD_TEMPLATE_KEY = "TEMPLATE_KEY";
        public const string FIELD_TEMPLATE_NAME = "TEMPLATE_NAME";
        public const string FIELD_TEMPLATE_VERSION = "TEMPLATE_VERSION";
        public const string FIELD_TEMPLATE_FROM_VERSION = "TEMPLATE_FROM_VERSION";
        public const string FIELD_TEMPLATE_DESCRIPTIONS = "TEMPLATE_DESCRIPTIONS";
        public const string FIELD_TEMPLATE_STATUS = "TEMPLATE_STATUS";

        public const string DATABASE_TABLE_NAME = "POR_LOT_TEMPLATE";
        
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_LOT_TEMPLATE";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_TEMPLATE_KEY, new FieldProperties() },
                                                                  {FIELD_TEMPLATE_NAME, new FieldProperties() },
                                                                  {FIELD_TEMPLATE_VERSION, new FieldProperties() },
                                                                  {FIELD_TEMPLATE_FROM_VERSION, new FieldProperties() },
                                                                  {FIELD_TEMPLATE_DESCRIPTIONS, new FieldProperties() },
                                                                  {FIELD_TEMPLATE_STATUS, new FieldProperties() },                                                                  
                                                                };
    }
}
