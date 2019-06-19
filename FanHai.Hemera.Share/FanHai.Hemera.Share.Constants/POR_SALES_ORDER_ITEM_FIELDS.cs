using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class POR_SALES_ORDER_ITEM_FIELDS : TableFields
    {

        public const string FIELD_SALES_ORDER_ITEM_KEY   = "SALES_ORDER_ITEM_KEY";
        public const string FIELD_SALES_ORDER_KEY        = "SALES_ORDER_KEY";
        public const string FIELD_ORDER_ITEM_NAME        = "ORDER_ITEM_NAME";
        public const string FIELD_QUANTITY_ORDERED       = "QUANTITY_ORDERED";
        public const string FIELD_QUANTITY_IN_PROGRESS   = "QUANTITY_IN_PROGRESS";
        public const string FIELD_QUANTITY_SCRAPPED      = "QUANTITY_SCRAPPED";
        public const string FIELD_QUANTITY_FINISHED      = "QUANTITY_FINISHED";
        public const string FIELD_QUANTITY_SHIPPED       = "QUANTITY_SHIPPED";
        public const string FIELD_QUANTITY_CLOSED        = "QUANTITY_CLOSED";
        public const string FIELD_QUANTITY_JOINED_WORKORDER = "QUANTITY_JOINED_WORKORDER";
        public const string FIELD_START_TIME             = "START_TIME";
        public const string FIELD_FINISH_TIME            = "FINISH_TIME";


        public const string FIELD_TIME_ITEM_CREATE          = "TIME_ITEM_CREATE";
        public const string FIELD_TIME_ITEM_JOIN_WORK_ORDER = "TIME_ITEM_JOIN_WORK_ORDER";
        public const string FIELD_TIME_PROMISED_SHIP        = "TIME_PROMISED_SHIP";
        public const string FIELD_TIME_ACTUAL_SHIP          = "TIME_ACTUAL_SHIP";
        public const string FIELD_CREATOR                = "CREATOR";
        public const string FIELD_CREATE_TIME            = "CREATE_TIME";
        public const string FIELD_EDITOR                 = "EDITOR";
        public const string FIELD_EDIT_TIME              = "EDIT_TIME";
        public const string FIELD_BILL_CONTACT_KEY       = "BILL_CONTACT_KEY";
        public const string FIELD_BUILD_CONTACT_KEY      = "BUILD_CONTACT_KEY";
        public const string FIELD_SHIP_CONTACT_KEY       = "SHIP_CONTACT_KEY";
        public const string FIELD_ACCOUNT_KEY            = "ACCOUNT_KEY";
        public const string FIELD_PART_NAME              = "PART_NAME";
        public const string FIELD_PART_NUMBER            = "PART_NUMBER";
        public const string FIELD_PART_REVISION          = "PART_REVISION";
        public const string FIELD_CREATE_TIMEZONE        = "CREATE_TIMEZONE";
        public const string FIELD_EDIT_TIMEZONE          = "EDIT_TIMEZONE";
        public const string FIELD_ORDER_ITEM_STATE       = "ORDER_ITEM_STATE";
        public const string FIELD_ORDER_ITEM_PRIORITY    = "ORDER_ITEM_PRIORITY";


        public const string DATABASE_TABLE_NAME = "POR_SALES_ORDER_ITEM";


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
                                                                {FIELD_SALES_ORDER_ITEM_KEY, new FieldProperties(typeof(string), false, "") },
                                                                {FIELD_SALES_ORDER_KEY     , new FieldProperties(typeof(string), false, "") },
                                                                {FIELD_ORDER_ITEM_NAME     , new FieldProperties(typeof(string), false, "") },
                                                                {FIELD_QUANTITY_ORDERED    , new FieldProperties(typeof(int), false, 0) },
                                                                {FIELD_QUANTITY_IN_PROGRESS, new FieldProperties(typeof(int), false, 0) },
                                                                {FIELD_QUANTITY_SCRAPPED   , new FieldProperties(typeof(int), false, 0) },
                                                                {FIELD_QUANTITY_FINISHED   , new FieldProperties(typeof(int), false, 0) },
                                                                {FIELD_QUANTITY_SHIPPED    , new FieldProperties(typeof(int), false, 0) },
                                                                {FIELD_QUANTITY_CLOSED     , new FieldProperties(typeof(int), false, 0) },
                                                                {FIELD_QUANTITY_JOINED_WORKORDER, new FieldProperties(typeof(int), false, 0)},
                                                                {FIELD_START_TIME          , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_FINISH_TIME         , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_ORDER_ITEM_STATE    , new FieldProperties() },
                                                                {FIELD_ORDER_ITEM_PRIORITY , new FieldProperties() },
                                                                {FIELD_CREATOR             , new FieldProperties() },
                                                                {FIELD_CREATE_TIME         , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_EDITOR              , new FieldProperties() },
                                                                {FIELD_EDIT_TIME           , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_BILL_CONTACT_KEY    , new FieldProperties() },
                                                                {FIELD_BUILD_CONTACT_KEY   , new FieldProperties() },
                                                                {FIELD_SHIP_CONTACT_KEY    , new FieldProperties() },
                                                                {FIELD_ACCOUNT_KEY         , new FieldProperties() },
                                                                {FIELD_PART_NUMBER         , new FieldProperties() },
                                                                {FIELD_PART_REVISION       , new FieldProperties() },
                                                                {FIELD_TIME_ITEM_CREATE    , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_TIME_ITEM_JOIN_WORK_ORDER , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_TIME_PROMISED_SHIP  , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_TIME_ACTUAL_SHIP    , new FieldProperties(typeof(DateTime), true, DateTime.Now) },
                                                                {FIELD_CREATE_TIMEZONE     , new FieldProperties() },
                                                                {FIELD_EDIT_TIMEZONE       , new FieldProperties() }
                                                              };
    }
}
