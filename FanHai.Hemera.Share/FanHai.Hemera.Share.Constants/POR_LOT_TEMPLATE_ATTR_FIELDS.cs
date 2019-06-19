/*
<FileInfo>
  <Author>ZhangHao SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_LOT_TEMPLATE_ATTR_FIELDS : TableFields
    {
        #region columns define
        public const string FIELD_LOT_TEMPLATE_KEY = "LOT_TEMPLATE_KEY";                  //批次ID
        public const string FIELD_ATTRIBUTE_KEY = "ATTRIBUTE_KEY";         //属性ID
        public const string FIELD_ATTRIBUTE_NAME = "ATTRIBUTE_NAME";    //属性名称
        public const string FIELD_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";     //属性值
        public const string FIELD_EDITOR = "EDITOR";     //editor
        public const string FIELD_EDIT_TIME = "EDIT_TIME";     //edit time
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";     //edit time zone
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "POR_LOT_TEMPLATE_ATTR";       //TABLE NAME
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
                                                                  {FIELD_LOT_TEMPLATE_KEY , new FieldProperties() },         //批次模板ID
                                                                  {FIELD_ATTRIBUTE_KEY , new FieldProperties() },   //属性ID
                                                                  {FIELD_ATTRIBUTE_NAME , new FieldProperties() },  //属性名称
                                                                  {FIELD_ATTRIBUTE_VALUE , new FieldProperties() },  //属性值
                                                                  {FIELD_EDITOR , new FieldProperties() },  //editor
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) },  //edit time
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() }  //edit time zone
                                                                };
        #endregion
    }
}
