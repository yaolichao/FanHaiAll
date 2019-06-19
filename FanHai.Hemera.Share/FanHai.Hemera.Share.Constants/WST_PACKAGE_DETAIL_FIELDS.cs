/*
<FileInfo>
  <Author>Alfred.Liu SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace SolarViewer.Hemera.Share.Constants
{
    public class WST_PACKAGE_DETAIL_FIELDS : TableFields
    {
        #region columns define
         public const string FIELD_ROW_KEY            = "ROW_KEY";    
         public const string FIELD_LOT_NUMBER         = "LOT_NUMBER"; 
         public const string FIELD_QTY                = "QTY";        
         public const string FIELD_ISDEFECT           = "ISDEFECT";   
         public const string FIELD_CLASS              = "CLASS";      
         public const string FIELD_CREATOR            = "CREATOR";    
         public const string FIELD_CREATE_TIME        = "CREATE_TIME";
         public const string FIELD_PACKAGE_KEY        = "PACKAGE_KEY";
         public const string FIELD_STATUS             = "STATUS";     
        #endregion

        #region table name define
         public const string DATABASE_TABLE_NAME = "WST_PACKAGE_DETAIL";       //TABLE NAME
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
                                                                    {FIELD_ROW_KEY, new FieldProperties() },     
                                                                    {FIELD_LOT_NUMBER, new FieldProperties() },  
                                                                    {FIELD_QTY, new FieldProperties() },         
                                                                    {FIELD_ISDEFECT, new FieldProperties() },    
                                                                    {FIELD_CLASS, new FieldProperties() },       
                                                                    {FIELD_CREATOR, new FieldProperties() },     
                                                                    {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                    {FIELD_PACKAGE_KEY, new FieldProperties() }, 
                                                                    {FIELD_STATUS, new FieldProperties() }      
                                                              };
        #endregion
    }
}
