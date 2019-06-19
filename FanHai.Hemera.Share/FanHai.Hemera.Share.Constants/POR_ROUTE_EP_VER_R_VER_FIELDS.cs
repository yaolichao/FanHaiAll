using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class POR_ROUTE_EP_VER_R_VER_FIELDS : TableFields
    {
        public const string FIELDS_ROUTE_SEQUENCE_KEY = "ROUTE_SEQUENCE_KEY";
        public const string FIELDS_ROUTE_ENTERPRISE_VER_KEY = "ROUTE_ENTERPRISE_VER_KEY";
        public const string FIELDS_ROUTE_ROUTE_VER_KEY = "ROUTE_ROUTE_VER_KEY";
        public const string FIELDS_ROUTE_SEQ = "ROUTE_SEQ";
     
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_EP_VER_R_VER";

        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_ROUTE_EP_VER_R_VER";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_ROUTE_SEQUENCE_KEY,new FieldProperties() },
                                                                  {FIELDS_ROUTE_ENTERPRISE_VER_KEY, new FieldProperties() },
                                                                  {FIELDS_ROUTE_ROUTE_VER_KEY, new FieldProperties() },
                                                                  {FIELDS_ROUTE_SEQ, new FieldProperties() }
                                                                };
    }
}

  
