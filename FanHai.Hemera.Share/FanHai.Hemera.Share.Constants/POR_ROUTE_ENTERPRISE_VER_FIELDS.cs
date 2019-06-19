#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace FanHai.Hemera.Share.Constants
{
    public class POR_ROUTE_ENTERPRISE_VER_FIELDS:TableFields
    {
        #region columns define

        public const string FIELD_ROUTE_ENTERPRISE_VER_KEY      ="ROUTE_ENTERPRISE_VER_KEY";    //集团途程版本ID
        public const string FIELD_DESCRIPTION                   ="DESCRIPTION";                 //描述
        public const string FIELD_ENTERPRISE_VERSION            ="ENTERPRISE_VERSION";          //版本
        public const string FIELD_ENTERPRISE_STATUS             ="ENTERPRISE_STATUS";           //状态：1 = Active, 0 = InActive, 2 = Archive
        public const string FIELD_ENTERPRISE_NAME               ="ENTERPRISE_NAME";         
        public const string FIELD_ENTERPRISE_FROM_VERSION       ="ENTERPRISE_FROM_VERSION";        
       
        public const string FIELD_CREATOR                       ="CREATOR";
        public const string FIELD_CREATE_TIME                   = "CREATE_TIME";                 //创建时间
        public const string FIELD_CREATE_TIMEZONE               = "CREATE_TIMEZONE";     

        public const string FIELD_EDITOR                        = "EDITOR";
        public const string FIELD_EDIT_TIME                     = "EDIT_TIME";
        public const string FIELD_EDIT_TIMEZONE                 = "EDIT_TIMEZONE";       

        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "POR_ROUTE_ENTERPRISE_VER";       //TABLE NAME
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
                                                                    {FIELD_ROUTE_ENTERPRISE_VER_KEY, new FieldProperties() },
                                                                    {FIELD_DESCRIPTION, new FieldProperties() },             
                                                                    {FIELD_CREATE_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },             
                                                                    {FIELD_ENTERPRISE_VERSION, new FieldProperties() },      
                                                                    {FIELD_ENTERPRISE_STATUS, new FieldProperties() },       
                                                                    {FIELD_ENTERPRISE_NAME, new FieldProperties() },         
                                                                    {FIELD_ENTERPRISE_FROM_VERSION, new FieldProperties() }, 
                                                                    {FIELD_CREATE_TIMEZONE, new FieldProperties() },     
                                                                    {FIELD_CREATOR, new FieldProperties() } ,    
                                                                    {FIELD_EDITOR, new FieldProperties() }, 
                                                                    {FIELD_EDIT_TIME, new FieldProperties(typeof(DateTime), true, DateTime.Now) },     
                                                                    {FIELD_EDIT_TIMEZONE, new FieldProperties() }                 
                                                              };
        #endregion
    }
}
