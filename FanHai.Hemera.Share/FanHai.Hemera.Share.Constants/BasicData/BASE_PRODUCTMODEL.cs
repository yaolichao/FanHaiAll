using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class BASE_PRODUCTMODEL : TableFields
    {
        public const string FIELDS_PROMODEL_KEY = "PROMODEL_KEY";
        public const string FIELDS_PROMODEL_NAME = "PROMODEL_NAME";
        public const string FIELDS_MEMO = "MEMO";
        public const string FIELDS_CELL_AREA = "CELL_AREA";
        public const string FIELDS_CELL_NUM = "CELL_NUM";
        public const string FIELDS_CREATER = "CREATER";
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        public const string FIELDS_EDITOR = "EDITOR";
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";
        public const string FIELDS_ISFLAG = "ISFLAG";
        public const string FIELDS_SEQ = "SEQ";
        public const string FIELDS_ISNEW = "ISNEW";

        public const string DATABASE_TABLE_NAME = "BASE_PRODUCTMODEL";
        public const string DATABASE_TABLE_FORUPDATE = "BASE_PRODUCTMODEL_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "BASE_PRODUCTMODEL_INSERT";

        public const string SaveFlag = "saveflag";
        public const string InsertFlag = "insert";
        public const string UpdateFlag = "update";

        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }

        //public static string TABLE_NAME = "POR_WORK_ORDER";
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }

        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELDS_PROMODEL_KEY, new FieldProperties() },
                                                                  {FIELDS_PROMODEL_NAME, new FieldProperties() },
                                                                  {FIELDS_MEMO, new FieldProperties() },
                                                                  {FIELDS_CELL_AREA, new FieldProperties() },
                                                                  {FIELDS_CELL_NUM, new FieldProperties() },                                                                
                                                                  {FIELDS_EDITOR, new FieldProperties() },
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },                                            
                                                                  {FIELDS_CREATER, new FieldProperties() },
                                                                  {FIELDS_CREATE_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) },
                                                                  {FIELDS_ISFLAG, new FieldProperties(typeof(Int16),true,1) },
                                                                  {FIELDS_SEQ,new FieldProperties()}
                                                                };
    }
}

  