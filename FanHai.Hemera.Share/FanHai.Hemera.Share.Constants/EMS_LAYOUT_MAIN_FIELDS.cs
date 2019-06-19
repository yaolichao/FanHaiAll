using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Constants
{
    public class EMS_LAYOUT_MAIN_FIELDS : TableFields
    {
        public const string LAYOUT_KEY = "LAYOUT_KEY";
        public const string LAYOUT_NAME = "LAYOUT_NAME";
        public const string LAYOUT_DESC = "LAYOUT_DESC";
        public const string LAYOUT_PIC = "LAYOUT_PIC";
        public const string STATE_FLAG = "STATE_FLAG";        
        public const string CREATOR = "CREATOR";       
        public const string EDITOR = "EDITOR";

        public const string DATABASE_TABLE_NAME = "EMS_LAYOUT_MAIN";

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
                                                                        {LAYOUT_NAME,new FieldProperties() }, 
                                                                        {LAYOUT_DESC,new FieldProperties() }, 
                                                                        {LAYOUT_PIC,new FieldProperties() }, 
                                                                        {STATE_FLAG,new FieldProperties() },                                                                         
                                                                        {CREATOR,new FieldProperties() },
                                                                        {EDITOR,new FieldProperties()}
                                                                    };
    }
}
