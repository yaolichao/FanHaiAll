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
    public class WIP_BOM_ATTACHMENT_FIELDS : TableFields
    {
        public const string FIELDS_ROW_KEY = "ROW_KEY";
        public const string FIELDS_PROD_NAME = "PROD_NAME";
        public const string FIELDS_PROD_VERSION = "PROD_VERSION";
        public const string FIELDS_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        public const string FIELDS_ROUTE_KEY = "ROUTE_KEY";
        public const string FIELDS_STEP_KEY = "STEP_KEY";
        public const string FIELDS_STEP_NAME = "STEP_NAME";
        public const string FIELDS_BOM_KEY = "BOM_KEY";

        public const string DATABASE_TABLE_NAME = "WIP_BOM_ATTACHMENT";

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
                                                                  {FIELDS_PROD_NAME, new FieldProperties() },
                                                                  {FIELDS_PROD_VERSION, new FieldProperties() },
                                                                  {FIELDS_ENTERPRISE_KEY, new FieldProperties() },
                                                                  {FIELDS_ROUTE_KEY, new FieldProperties() },
                                                                  {FIELDS_STEP_KEY, new FieldProperties() },
                                                                  {FIELDS_STEP_NAME, new FieldProperties() },
                                                                  {FIELDS_BOM_KEY, new FieldProperties() }
                                                                };
    }
}

  

