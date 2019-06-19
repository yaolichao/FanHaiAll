using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class WIP_JOB_FIELDS : TableFields
    {
        public const string FIELDS_ROW_KEY = "ROW_KEY";
        public const string FIELDS_LOT_NUMBER = "LOT_NUMBER";
        public const string FIELDS_LINE_NAME = "LINE_NAME";
        public const string FIELDS_WORKORDER_NUMBER = "WORKORDER_NUMBER";
        public const string FIELDS_ENTERPRISE_KEY = "ENTERPRISE_KEY";
        public const string FIELDS_ROUTE_KEY = "ROUTE_KEY";
        public const string FIELDS_STEP_KEY = "STEP_KEY";
        public const string FIELDS_JOB_TYPE = "JOB_TYPE";
        public const string FIELDS_JOB_STATUS = "JOB_STATUS";
        public const string FIELDS_JOB_CLOSETYPE = "JOB_CLOSETYPE";
        public const string FIELDS_JOB_RUNACCOUNT = "JOB_RUNACCOUNT";
        public const string FIELDS_JOB_NEXTRUNTIME = "JOB_NEXTRUNTIME";
        public const string FIELDS_JOB_CREATETIME = "JOB_CREATETIME";
        public const string FIELDS_NOTIFY_USER = "NOTIFY_USER";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string FIELDS_LOT_KEY = "LOT_KEY";
        public const string FIELDS_EQUIPMENT_KEY = "EQUIPMENT_KEY";

        public const string DATABASE_TABLE_NAME = "WIP_JOB";

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
                                                                  {FIELDS_LOT_NUMBER, new FieldProperties() },
                                                                  {FIELDS_LINE_NAME, new FieldProperties() },
                                                                  {FIELDS_WORKORDER_NUMBER, new FieldProperties() },
                                                                  {FIELDS_ENTERPRISE_KEY, new FieldProperties() },
                                                                  {FIELDS_ROUTE_KEY, new FieldProperties() },
                                                                  {FIELDS_STEP_KEY, new FieldProperties() },
                                                                  {FIELDS_JOB_TYPE, new FieldProperties() },
                                                                  {FIELDS_JOB_STATUS, new FieldProperties() },
                                                                  {FIELDS_JOB_CLOSETYPE, new FieldProperties() },
                                                                  {FIELDS_JOB_RUNACCOUNT, new FieldProperties() },
                                                                  {FIELDS_JOB_NEXTRUNTIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_JOB_CREATETIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_NOTIFY_USER, new FieldProperties() },
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_EDIT_TIMEZONE, new FieldProperties() },
                                                                  {FIELDS_LOT_KEY, new FieldProperties() },
                                                                  {FIELDS_EQUIPMENT_KEY, new FieldProperties() }
                                                                };
    }
}
