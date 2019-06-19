using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{

    public class BATTERYRESULT_FIELDS : TableFields
    {
         public const string FIELDS_FACTORY_NAME = "FACTORY_NAME";
         public const string FIELDS_SERIALNUMBER ="SERIALNUMBER";
         public const string FIELDS_CHARGE       ="CHARGE";
         public const string FIELDS_BIN          ="BIN";
         public const string FIELDS_PACKETNUMBER ="PACKETNUMBER";
         public const string FIELDS_DATES        ="DATES";
         public const string FIELDS_OPERATOR     ="OPERATOR";
         public const string FIELDS_COLUMN_01    ="COLUMN_01";
         public const string FIELDS_COLUMN_02    ="COLUMN_02";
         public const string FIELDS_COLUMN_03    ="COLUMN_03";
         public const string FIELDS_COLUMN_04    ="COLUMN_04";
         public const string FIELDS_COLUMN_05    ="COLUMN_05";
         public const string FIELDS_COLUMN_06    ="COLUMN_06";
         public const string FIELDS_COLUMN_07    ="COLUMN_07";
         public const string FIELDS_COLUMN_08    ="COLUMN_08";
         public const string FIELDS_COLUMN_09    ="COLUMN_09";
         public const string FIELDS_COLUMN_10    ="COLUMN_10";
         public const string FIELDS_COLUMN_11    ="COLUMN_11";
         public const string FIELDS_COLUMN_12    ="COLUMN_12";
         public const string FIELDS_COLUMN_13    ="COLUMN_13";
         public const string FIELDS_COLUMN_14    ="COLUMN_14";
         public const string FIELDS_COLUMN_15    ="COLUMN_15";
         public const string FIELDS_COLUMN_16    ="COLUMN_16";
         public const string FIELDS_COLUMN_17    ="COLUMN_17";
         public const string FIELDS_COLUMN_18    ="COLUMN_18";
         public const string FIELDS_COLUMN_19    ="COLUMN_19";
         public const string FIELDS_COLUMN_20    ="COLUMN_20";
         public const string FIELDS_COLUMN_21    ="COLUMN_21";
         public const string FIELDS_COLUMN_22    ="COLUMN_22";
         public const string FIELDS_COLUMN_23    ="COLUMN_23";
         public const string FIELDS_COLUMN_24    ="COLUMN_24";
         public const string FIELDS_COLUMN_25    ="COLUMN_25";
         public const string FIELDS_COLUMN_26    ="COLUMN_26";
         public const string FIELDS_COLUMN_27    ="COLUMN_27";
         public const string FIELDS_COLUMN_28    ="COLUMN_28";
         public const string FIELDS_COLUMN_29    ="COLUMN_29";
         public const string FIELDS_COLUMN_30    ="COLUMN_30";
         public const string FIELDS_COLUMN_31    ="COLUMN_31";
         public const string FIELDS_COLUMN_32    ="COLUMN_32";
         public const string FIELDS_COLUMN_33    ="COLUMN_33";
         public const string FIELDS_COLUMN_34    ="COLUMN_34";
         public const string FIELDS_COLUMN_35    ="COLUMN_35";
         public const string FIELDS_COLUMN_36    ="COLUMN_36";
         public const string FIELDS_COLUMN_37    ="COLUMN_37";
         public const string FIELDS_COLUMN_38    ="COLUMN_38";
         public const string FIELDS_COLUMN_39    ="COLUMN_39";
         public const string FIELDS_COLUMN_40    ="COLUMN_40";
         public const string FIELDS_COLUMN_41    ="COLUMN_41";
         public const string FIELDS_COLUMN_42    ="COLUMN_42";
         public const string FIELDS_COLUMN_43    ="COLUMN_43";
         public const string FIELDS_COLUMN_44    ="COLUMN_44";
         public const string FIELDS_COLUMN_45    ="COLUMN_45";
         public const string FIELDS_COLUMN_46    ="COLUMN_46";
         public const string FIELDS_COLUMN_47    ="COLUMN_47";
         public const string FIELDS_COLUMN_48    ="COLUMN_48";
         public const string FIELDS_COLUMN_49    ="COLUMN_49";
         public const string FIELDS_COLUMN_50    ="COLUMN_50";
         public const string FIELDS_COLUMN_51    ="COLUMN_51";
         public const string FIELDS_COLUMN_52    ="COLUMN_52";
         public const string FIELDS_COLUMN_53    ="COLUMN_53";
         public const string FIELDS_COLUMN_54    ="COLUMN_54";
         public const string FIELDS_COLUMN_55    ="COLUMN_55";

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
                return _fields; ;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                {FIELDS_FACTORY_NAME,new FieldProperties()},
                                                                {FIELDS_SERIALNUMBER,new FieldProperties()},   
                                                                {FIELDS_CHARGE      , new FieldProperties() },   
                                                                {FIELDS_BIN         , new FieldProperties() },   
                                                                {FIELDS_PACKETNUMBER, new FieldProperties() }, 
                                                                {FIELDS_DATES       , new FieldProperties(typeof(DateTime),true,DateTime.Now) },   
                                                                {FIELDS_OPERATOR    , new FieldProperties() },   
                                                                {FIELDS_COLUMN_01   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_02    , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_03    , new FieldProperties() },  
                                                                {FIELDS_COLUMN_04    , new FieldProperties() },  
                                                                {FIELDS_COLUMN_05    , new FieldProperties() },  
                                                                {FIELDS_COLUMN_06   , new FieldProperties() },  
                                                                {FIELDS_COLUMN_07   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_08   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_09   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_10   , new FieldProperties() },  
                                                                {FIELDS_COLUMN_11   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_12   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_13   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_14    , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_15    , new FieldProperties() },  
                                                                {FIELDS_COLUMN_16    , new FieldProperties() },  
                                                                {FIELDS_COLUMN_17    , new FieldProperties() },  
                                                                {FIELDS_COLUMN_18   , new FieldProperties() },  
                                                                {FIELDS_COLUMN_19   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_20   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_21   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_22   , new FieldProperties() },  
                                                                {FIELDS_COLUMN_23   , new FieldProperties() },   
                                                                {FIELDS_COLUMN_24   , new FieldProperties() },    
                                                                {FIELDS_COLUMN_25  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_26  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_27  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_28   , new FieldProperties() },
                                                                {FIELDS_COLUMN_29   , new FieldProperties() },
                                                                {FIELDS_COLUMN_30   , new FieldProperties() },
                                                                {FIELDS_COLUMN_31   , new FieldProperties() },
                                                                {FIELDS_COLUMN_32  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_33  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_34  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_35  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_36  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_37  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_38  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_39  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_40   , new FieldProperties() },
                                                                {FIELDS_COLUMN_41   , new FieldProperties() },
                                                                {FIELDS_COLUMN_42   , new FieldProperties() },
                                                                {FIELDS_COLUMN_43   , new FieldProperties() },
                                                                {FIELDS_COLUMN_44  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_45  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_46  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_47  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_48  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_49  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_50  , new FieldProperties() }, 
                                                                {FIELDS_COLUMN_51 , new FieldProperties() },
                                                                {FIELDS_COLUMN_52 , new FieldProperties() },
                                                                {FIELDS_COLUMN_53 , new FieldProperties() },
                                                                {FIELDS_COLUMN_54 , new FieldProperties() },
                                                                {FIELDS_COLUMN_55 , new FieldProperties() }
                                                              };  
    }                                                             
}                                                                 
                                                                  
  
  
  
  
  
  
  
  
  