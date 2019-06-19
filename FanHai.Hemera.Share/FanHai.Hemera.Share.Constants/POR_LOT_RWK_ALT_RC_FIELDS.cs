using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_LOT_RWK_ALT_RC_FIELDS:TableFields
    {
       public const string FIELD_LOT_KEY               ="LOT_KEY";
       public const string FIELD_RWK_ALT_FLAG          ="RWK_ALT_FLAG";
       public const string FIELD_RETURN_ENTERPRISE_KEY ="RETURN_ENTERPRISE_KEY";
       public const string FIELD_RETURN_ROUTE_KEY      ="RETURN_ROUTE_KEY";
       public const string FIELD_RETURN_STEP_KEY       ="RETURN_STEP_KEY";
       public const string FIELD_STATUS                ="STATUS";
       public const string FIELD_ENTERPRISE_KEY        ="ENTERPRISE_KEY";
       public const string FIELD_ROUTE_KEY             ="ROUTE_KEY";
       public const string FIELD_STEP_KEY              = "STEP_KEY";

       public const string DATABASE_TABLE_NAME = "POR_LOT_RWK_ALT_RC";

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
                                                                  {FIELD_LOT_KEY,new FieldProperties()},
                                                                  {FIELD_RWK_ALT_FLAG,new FieldProperties() },
                                                                  {FIELD_RETURN_ENTERPRISE_KEY,new FieldProperties()},
                                                                  {FIELD_RETURN_ROUTE_KEY,new FieldProperties()  },
                                                                  {FIELD_RETURN_STEP_KEY,new FieldProperties()      },
                                                                  {FIELD_STATUS,new FieldProperties()               },
                                                                  {FIELD_ENTERPRISE_KEY,new FieldProperties()       },
                                                                  {FIELD_ROUTE_KEY,new FieldProperties()            },
                                                                  {FIELD_STEP_KEY,new FieldProperties()}                                                                  
                                                                };

    }
}
