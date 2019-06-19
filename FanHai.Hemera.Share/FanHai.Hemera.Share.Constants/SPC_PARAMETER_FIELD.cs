using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class SPC_PARAMETER_FIELD : TableFields
    {
        public const string PARAMETER_NAME = "PARAM_NAME";
        public const string PARAMETER_KEY = "PARAM_KEY";
        public const string DATABASE_TABLE_NAME = "SPC_PARAMETER";

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
                                                                  {PARAMETER_NAME, new FieldProperties() },
                                                                  {PARAMETER_KEY, new FieldProperties() }                                                                  
                                                                };

    }
}
