using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class FMM_LOCATION_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_LOCATION_KEY = "LOCATION_KEY";              
        public const string FIELD_LOCATION_NAME = "LOCATION_NAME";        
        public const string FIELD_DESCRIPTIONS = "DESCRIPTIONS";   
        public const string FIELD_CREATOR = "CREATOR";     
        public const string FIELD_CREATE_TIME = "CREATE_TIME";    
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";   
        public const string FIELD_EDITOR = "EDITOR";    
        public const string FIELD_EDIT_TIME = "EDIT_TIME";     
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";     
        public const string FIELD_SITE_NUMBER = "SITE_NUMBER";    
        public const string FIELD_LOCATION_LEVEL = "LOCATION_LEVEL";     
        
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "FMM_LOCATION";       //TABLE NAME
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
                                                                  {FIELD_LOCATION_KEY , new FieldProperties() },       
                                                                  {FIELD_LOCATION_NAME , new FieldProperties() },   
                                                                  {FIELD_DESCRIPTIONS , new FieldProperties() },  
                                                                  {FIELD_LOCATION_LEVEL , new FieldProperties() },  
                                                                  {FIELD_SITE_NUMBER , new FieldProperties() },  
                                                                  {FIELD_EDITOR , new FieldProperties() },  //editor
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) },  //edit time
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() } , //edit time zone
                                                                    {FIELD_CREATOR , new FieldProperties() },  //editor
                                                                  {FIELD_CREATE_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) },  //edit time
                                                                  {FIELD_CREATE_TIMEZONE , new FieldProperties() }  //edit time zone
                                                                };
        #endregion
    }
}