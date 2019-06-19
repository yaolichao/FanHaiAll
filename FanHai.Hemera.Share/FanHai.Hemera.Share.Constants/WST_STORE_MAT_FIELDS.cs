using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class WST_STORE_MAT_FIELDS:TableFields
    {
        #region columns define
        public const string FIELD_ROW_KEY                ="ROW_KEY";
        public const string FIELD_ITEM_NO                ="ITEM_NO";
        public const string FIELD_ITEM_DESCRIPTION       ="ITEM_DESCRIPTION";
        public const string FIELD_ITEM_TYPE              ="ITEM_TYPE";
        public const string FIELD_ITEM_QTY               ="ITEM_QTY";
        public const string FIELD_ITEM_UNIT              ="ITEM_UNIT";
        public const string FIELD_OBJECT_STATUS          ="OBJECT_STATUS";
        public const string FIELD_STORE_KEY              ="STORE_KEY";
        public const string FIELD_BILL_NUMBER            ="BILL_NUMBER";
        public const string FIELD_WORKORDER_NUMBER       ="WORKORDER_NUMBER";
        public const string FIELD_EDITOR                 ="EDITOR";
        public const string FIELD_EDIT_TIME              ="EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE          ="EDIT_TIMEZONE";
        public const string FIELD_SERIAL_NUMBER          ="SERIAL_NUMBER";
        public const string FIELD_BAR_CODE               = "BAR_CODE";
        public const string FIELD_LINE_KEY = "LINE_KEY";
        public const string FIELD_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        public const string FIELD_ROUTE_KEY = "ROUTE_KEY";
        public const string FIELD_STEP_KEY = "STEP_KEY";
        public const string FIELD_BALANCE_QTY = "BALANCE_QTY";
        public const string FIELD_BALANCE_EDITOR = "BALANCE_EDITOR";
        public const string FIELD_SHIFT_NAME = "SHIFT_NAME";

        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "WST_STORE_MAT";      //TABLE NAME
        #endregion

        #region get TABLE_NAME
        /// <summary>
        /// override TABLE_NAME
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        #endregion

        #region get FIELDS
        /// <summary>
        /// override fields
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        #region define dictionary of containing column's fields
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                 {FIELD_ROW_KEY ,new FieldProperties()},        
                                                                 {FIELD_ITEM_NO ,new FieldProperties()},           
                                                                 {FIELD_ITEM_DESCRIPTION,new FieldProperties()},   
                                                                 {FIELD_ITEM_TYPE ,new FieldProperties()},         
                                                                 {FIELD_ITEM_QTY ,new FieldProperties()},          
                                                                 {FIELD_ITEM_UNIT,new FieldProperties()},          
                                                                 {FIELD_OBJECT_STATUS,new FieldProperties()},          
                                                                 {FIELD_STORE_KEY ,new FieldProperties()},         
                                                                 {FIELD_BILL_NUMBER ,new FieldProperties()},       
                                                                 {FIELD_WORKORDER_NUMBER ,new FieldProperties()},      
                                                                 {FIELD_EDITOR   ,new FieldProperties()},          
                                                                 {FIELD_EDIT_TIME ,new FieldProperties(typeof(DateTime), true, DateTime.Now) },   
                                                                 {FIELD_EDIT_TIMEZONE,new FieldProperties()},   
                                                                 {FIELD_SERIAL_NUMBER,new FieldProperties()},   
                                                                 {FIELD_BAR_CODE  ,new FieldProperties()},
                                                                 {FIELD_LINE_KEY,new FieldProperties()},   
                                                                 {FIELD_ENTERPRISE_KEY,new FieldProperties()},   
                                                                 {FIELD_ROUTE_KEY  ,new FieldProperties()},
                                                                 {FIELD_STEP_KEY  ,new FieldProperties()},
                                                                 {FIELD_BALANCE_QTY,new FieldProperties()},
                                                                 {FIELD_BALANCE_EDITOR,new FieldProperties()},
                                                                 {FIELD_SHIFT_NAME,new FieldProperties()}
                                                              };     
        #endregion                                                   
    }
}
