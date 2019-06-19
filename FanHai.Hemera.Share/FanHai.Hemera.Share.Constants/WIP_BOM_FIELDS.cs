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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace SolarViewer.Hemera.Share.Constants
{
    public class WIP_BOM_FIELDS : TableFields
    {
        public const string FIELDS_ROW_KEY = "ROW_KEY";
        public const string FIELDS_BOM_NAME = "BOM_NAME";
        public const string FIELDS_BOM_VERSION = "BOM_VERSION";
        public const string FIELDS_BOM_STATUS = "BOM_STATUS";
        public const string FIELDS_BOM_DESC = "BOM_DESC";
        public const string FIELDS_BOM_TYPE = "BOM_TYPE";
        public const string FIELDS_EFFECTIVITY_START = "EFFECTIVITY_START";
        public const string FIELDS_EFFECTIVITY_END = "EFFECTIVITY_END";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string FIELDS_CREATOR = "CREATOR";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_CREATE_TIMEZONE = "CREATE_TIMEZONE";
        public const string FIELDS_PARENT_ROW_KEY = "PARENT_ROW_KEY";

        public const string DATABASE_TABLE_NAME = "WIP_BOM";

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
                                                                  {FIELDS_ROW_KEY, new FieldProperties() },
                                                                  {FIELDS_BOM_NAME, new FieldProperties() },
                                                                  {FIELDS_BOM_VERSION, new FieldProperties() },
                                                                  {FIELDS_BOM_STATUS, new FieldProperties() },
                                                                  {FIELDS_BOM_DESC, new FieldProperties() },
                                                                  {FIELDS_BOM_TYPE, new FieldProperties() },
                                                                  {FIELDS_EFFECTIVITY_START, new FieldProperties(typeof(DateTime),true,null) },
                                                                  {FIELDS_EFFECTIVITY_END, new FieldProperties(typeof(DateTime),true,null) },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_PARENT_ROW_KEY, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties() },
                                                                  {FIELDS_CREATOR, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_CREATE_TIMEZONE, new FieldProperties() }
                                                                };
    }
}

  
