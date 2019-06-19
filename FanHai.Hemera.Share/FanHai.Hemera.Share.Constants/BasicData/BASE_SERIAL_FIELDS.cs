using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class BASE_SERIAL_FIELDS:TableFields 
    {
        public const string FIELDS_SERIAL_KEY = "SERIAL_KEY";
        public const string FIELDS_BARCODECODE = "BARCODECODE";
        public const string FIELDS_WORKORDER = "WORKORDER";
        public const string FIELDS_LINE = "LINE";
        public const string FIELDS_SERIAL = "SERIAL";
        public const string FIELDS_EDITDATE = "EDITDATE";
        public const string DATABASE_TABLE_NAME = "BASE_SERIAL";

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
                                                                {FIELDS_SERIAL_KEY, new FieldProperties() },
                                                                {FIELDS_BARCODECODE, new FieldProperties() },
                                                                {FIELDS_WORKORDER, new FieldProperties() },
                                                                {FIELDS_LINE, new FieldProperties() },
                                                                {FIELDS_SERIAL , new FieldProperties() },                                                   
                                                                {FIELDS_EDITDATE , new FieldProperties(typeof(DateTime),true,DateTime.Now) }
                                                              };
    }
}
