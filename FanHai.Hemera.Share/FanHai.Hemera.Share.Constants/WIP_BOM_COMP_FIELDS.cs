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
    public class WIP_BOM_COMP_FIELDS : TableFields
    {
        public const string FIELDS_ROW_KEY = "ROW_KEY";
        public const string FIELDS_OWNER_TYPE = "OWNER_TYPE";
        public const string FIELDS_BOM_ROW_KEY = "BOM_ROW_KEY";
        public const string FIELDS_IS_MAND_OPN = "IS_MAND_OPN";
        public const string FIELDS_IS_SUBSTITUTE = "IS_SUBSTITUTE";
        public const string FIELDS_SUB_SEQUENCE = "SUB_SEQUENCE";
        public const string FIELDS_PROD_NAME = "PROD_NAME";
        public const string FIELDS_PROD_VERSION = "PROD_VERSION";
        public const string FIELDS_PROD_QTY = "PROD_QTY";
        public const string FIELDS_PROD_QTY_TYPE = "PROD_QTY_TYPE";
        public const string FIELDS_PROD_UOM = "PROD_UOM";
        public const string FIELDS_PROD_UOM_TYPE = "PROD_UOM_TYPE";
        public const string FIELDS_PROD_SELF_CONS = "PROD_SELF_CONS";
        public const string FIELDS_PROD_TARGET_UOM = "PROD_TARGET_UOM";
        public const string FIELDS_PROD_TARGET_UOM_TYPE = "PROD_TARGET_UOM_TYPE";
        public const string FIELDS_PROD_ASSY_SEQ = "PROD_ASSY_SEQ";
        public const string FIELDS_SUB_PARENT_ROW_KEY = "SUB_PARENT_ROW_KEY";

        public const string DATABASE_TABLE_NAME = "WIP_BOM_COMP";

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
                                                                  {FIELDS_OWNER_TYPE, new FieldProperties() },
                                                                  {FIELDS_BOM_ROW_KEY, new FieldProperties() },
                                                                  {FIELDS_IS_MAND_OPN, new FieldProperties() },
                                                                  {FIELDS_IS_SUBSTITUTE, new FieldProperties() },
                                                                  {FIELDS_SUB_SEQUENCE, new FieldProperties() },
                                                                  {FIELDS_PROD_NAME, new FieldProperties() },
                                                                  {FIELDS_PROD_VERSION, new FieldProperties() },
                                                                  {FIELDS_PROD_QTY, new FieldProperties() },
                                                                  {FIELDS_PROD_QTY_TYPE, new FieldProperties() },
                                                                  {FIELDS_PROD_UOM, new FieldProperties() },
                                                                  {FIELDS_PROD_UOM_TYPE, new FieldProperties() },
                                                                  {FIELDS_PROD_SELF_CONS, new FieldProperties() },
                                                                  {FIELDS_PROD_TARGET_UOM, new FieldProperties() },
                                                                  {FIELDS_PROD_TARGET_UOM_TYPE, new FieldProperties() },
                                                                  {FIELDS_PROD_ASSY_SEQ, new FieldProperties() },
                                                                  {FIELDS_SUB_PARENT_ROW_KEY, new FieldProperties() }
                                                                };
    }
}

  

