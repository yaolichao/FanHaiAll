using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace FanHai.Hemera.Share.Constants
{
    public class POR_WO_PRD : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_PRD_DECAY : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_DECAY";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_DECAY_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_DECAY_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_DECAY_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_PRD_LEVEL : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_LEVEL";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_LEVEL_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_LEVEL_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_LEVEL_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_PRD_PRINTSET : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_PRINTSET";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_PRINTSET_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_PRINTSET_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_PRINTSET_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_PRD_PS : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_PS";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_PS_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_PS_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_PS_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_PRD_PS_CLR : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_PS_CLR";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_PS_CLR_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_PS_CLR_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_PS_CLR_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_PRD_PS_SUB : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_PS_SUB";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_PS_SUB_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_PS_SUB_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_PS_SUB_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_OEM : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_OEM";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_OEM_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_OEM_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_OEM_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_ROUTE : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_ROUTE";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_ROUTE_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_ROUTE_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_ROUTE_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }


    public class POR_WO_LINE : TableFields
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_LINE";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_LINE_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_LINE_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_LINE_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }

    public class POR_WO_FLASH_AUTOPRINT : TableFields
    {
        #region fields
        /// <summary>
        /// 工单号
        /// </summary>
        public const string FIELDS_WORK_ORDER_KEY = "WORK_ORDER_KEY";
        /// <summary>
        /// 料号
        /// </summary>
        public const string FIELDS_PART_NUMBER = "PART_NUMBER";
        /// <summary>
        /// 打印模板
        /// </summary>
        public const string FIELDS_PRINT_TYPE = "PRINT_TYPE";
        /// <summary>
        /// 打印份数
        /// </summary>
        public const string FIELDS_PRINT_COPY = "PRINT_COPY";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_FLASH_AUTOPRINT";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_FLASH_AUTOPRINT_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_FLASH_AUTOPRINT_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_WORK_ORDER_KEY,new FieldProperties()},
                                                                  {FIELDS_PART_NUMBER,new FieldProperties()},
                                                                  {FIELDS_PRINT_TYPE,new FieldProperties()},
                                                                  {FIELDS_PRINT_COPY,new FieldProperties()},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                               
                                                              
                                                              
                                                              };
        #endregion
    }

    public class POR_WO_PRD_PSHOW : TableFields//fyb
    {
        #region fields
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FIELDS_CREATER = "CREATER";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FIELDS_CREATE_TIME = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FIELDS_EDITER = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FIELDS_EDIT_TIME = "EDIT_TIME";

        public const string DATABASE_TABLE_NAME = "POR_WO_PRD_POWERSHOW";
        public const string DATABASE_TABLE_FORDELETE = "POR_WO_PRD_POWERSHOW_DELETE";
        public const string DATABASE_TABLE_FORUPDATE = "POR_WO_PRD_POWERSHOW_UPDATE";
        public const string DATABASE_TABLE_FORINSERT = "POR_WO_PRD_POWERSHOW_INSERT";
        #endregion

        #region table
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
        #endregion

        #region _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {                                                                  
                                                                  {FIELDS_CREATER,new FieldProperties()},
                                                                  {FIELDS_CREATE_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  {FIELDS_EDITER,new FieldProperties()},
                                                                  {FIELDS_EDIT_TIME, new FieldProperties(typeof(DateTime),true,DateTime.Now) }                                                
                                                                };
        #endregion
    }
}

  