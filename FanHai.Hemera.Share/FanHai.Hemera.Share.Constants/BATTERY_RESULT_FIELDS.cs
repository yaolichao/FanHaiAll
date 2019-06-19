using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class BATTERY_RESULT_FIELDS : TableFields
    {
           public const string SERIALNUMBER  = "SERIALNUMBER";
           public const string CHARGE        = "CHARGE";
           public const string DATES         = "DATES";
           public const string CELLAREA      = "CELLAREA";
           public const string CLASSIFICATION= "CLASSIFICATION";
           public const string TMONICELL     = "TMONICELL";
           public const string OPERATOR      = "OPERATOR";
           public const string BIN           = "BIN";
           public const string TEMPERATURE   = "TEMPERATURE";
           public const string E             = "E";
           public const string PMPP          = "PMPP";
           public const string UMPP          = "UMPP";
           public const string IMPP          = "IMPP";
           public const string JMPP          = "JMPP";
           public const string UOC           = "UOC";
           public const string ISC           = "ISC";
           public const string RSER          = "RSER";
           public const string RSH           = "RSH";
           public const string FF            = "FF";
           public const string EFF           = "EFF";
           public const string JSC           = "JSC";
           public const string UREV1         = "UREV1";
           public const string IREV1         = "IREV1";
           public const string UREV2         = "UREV2";
           public const string IREV2         = "IREV2";
           public const string IVLD1         = "IVLD1";
           public const string UVLD1         = "UVLD1";
           public const string PVLD1         = "PVLD1";
           public const string IVLD2         = "IVLD2";
           public const string UVLD2         = "UVLD2";
           public const string PVLD2         = "PVLD2";
           public const string IREVMAX       = "IREVMAX";
           public const string UREVMAX       = "UREVMAX";
           public const string CELLTYP       = "CELLTYP";
           public const string RSERLFDFIEC   = "RSERLFDFIEC";

           public const string DATABASE_TABLE_NAME = "BATTERY_RESULTS";

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
                                                                {SERIALNUMBER   , new FieldProperties() },
                                                                {CHARGE         , new FieldProperties() },    
                                                                {DATES          , new FieldProperties(typeof(DateTime),true,DateTime.Now) },  
                                                                {CELLAREA       , new FieldProperties() },      
                                                                {CLASSIFICATION, new FieldProperties() }, 
                                                                {TMONICELL     ,new FieldProperties()},   
                                                                {OPERATOR      , new FieldProperties() },   
                                                                {BIN           , new FieldProperties() },   
                                                                {TEMPERATURE   , new FieldProperties() }, 
                                                                {E             , new FieldProperties() },   
                                                                {PMPP          , new FieldProperties() },   
                                                                {UMPP          , new FieldProperties() },   
                                                                {IMPP           , new FieldProperties() }, 
                                                                {JMPP           , new FieldProperties() },   
                                                                {UOC            , new FieldProperties() },   
                                                                {ISC            , new FieldProperties() },   
                                                                {RSER          , new FieldProperties() },  
                                                                {RSH           , new FieldProperties() },    
                                                                {FF            , new FieldProperties() },    
                                                                {EFF           , new FieldProperties() },    
                                                                {JSC           , new FieldProperties() },  
                                                                {UREV1         , new FieldProperties() },    
                                                                {IREV1         , new FieldProperties() },    
                                                                {UREV2         , new FieldProperties() },    
                                                                {IREV2          , new FieldProperties() }, 
                                                                {IVLD1          , new FieldProperties() },   
                                                                {UVLD1          , new FieldProperties() },   
                                                                {PVLD1          , new FieldProperties() },   
                                                                {IVLD2         , new FieldProperties() },  
                                                                {UVLD2         , new FieldProperties() },    
                                                                {PVLD2         , new FieldProperties() },    
                                                                {IREVMAX       , new FieldProperties() },    
                                                                {UREVMAX       , new FieldProperties() },  
                                                                {CELLTYP       , new FieldProperties() },    
                                                                {RSERLFDFIEC   , new FieldProperties() }           
                                                              };
    }
}
