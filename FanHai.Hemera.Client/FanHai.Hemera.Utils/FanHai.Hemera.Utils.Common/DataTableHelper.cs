using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Utils.Common
{
    public static class DataTableHelper
    {
        public static DataTable CreateUDAsTable()
        {
            List<string> fields = new List<string>() { 
                                                        COMMON_FIELDS.FIELD_COMMON_LINKED_TABLE_NAME,
                                                        COMMON_FIELDS.FIELD_COMMON_LINKED_ITEM_KEY,
                                                        COMMON_FIELDS.FIELD_COMMON_LINKED_ITEM_SEQ,
                                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                        CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_VALUE,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR,
                                                        COMMON_FIELDS.FIELD_COMMON_EDIT_TIME,
                                                        COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE};
            
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(TRANS_TABLES.TABLE_UDAS, fields);
        }

        #region Generate Purpose
        #region UDAs
        public static DataTable CreateDataTableForUDA(string tableName, string linkedToItemColumnName)
        {
            List<string> fields = new List<string>() { 
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
                                                        linkedToItemColumnName,
                                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                        COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(tableName, fields);
        }

        public static DataTable CreateDataTableForUDAEx(string tableName, string linkedToItemColumnName,string LinkedToTable)
        {
            List<string> fields = new List<string>() { 
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
                                                        linkedToItemColumnName,
                                                        LinkedToTable,
                                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                        COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(tableName, fields);
        }

        #endregion

        #region Update basic Data
        /// <summary>
        /// 为更新基础数据创建数据表对象，仅包含数据表结构。
        /// </summary>
        /// <param name="tableName">数据表名称。</param>
        /// <returns>
        /// 数据表对象。数据表对象包含四列：
        /// 更新主键<see cref="COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY"/>,
        /// 更新名称<see cref="COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME"/>,
        /// 更新原始值<see cref="COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE"/>,
        /// 更新新值<see cref="COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE"/>。
        /// </returns>
        public static DataTable CreateDataTableForUpdateBasicData(string tableName)
        {
            List<string> fields = new List<string>()
            {
                COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY,                  //更新主键
                COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME,                 //更新名称
                COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE,            //更新原始值
                COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE,            //更新新值
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(tableName, fields);
        }
        #endregion

        #region Update Main Data
        public static DataTable CreateDataTableForUpdateMainData(string tableName)
        {
            List<string> fields = new List<string>()
            {
                COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY,
                COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME,
                COMMON_FIELDS.FIELD_COMMON_UPDATE_VALUE
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(tableName, fields);
        }
        #endregion

        #endregion

        //public static DataTable CreateDataTableForInsertSalesOrder()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                                POR_SALES_ORDER_FIELDS.FIELD_SALES_ORDER_KEY, 
        //                                                POR_SALES_ORDER_FIELDS.FIELD_ORDER_NUMBER,
        //                                                POR_SALES_ORDER_FIELDS.FIELD_ORDER_STATE,
        //                                                POR_SALES_ORDER_FIELDS.FIELD_ORDER_PRIORITY,
        //                                                POR_SALES_ORDER_FIELDS.FIELD_DESCRIPTIONS,
        //                                                POR_SALES_ORDER_FIELDS.FIELD_ENTERED_TIME,
        //                                                POR_SALES_ORDER_FIELDS.FIELD_PROMISED_TIME,
        //                                                COMMON_FIELDS.FIELD_COMMON_EDITOR
        //                                            };
        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //public static DataTable CreateDataTableForInsertSalesOrderItem()
        //{
        //    List<string> fields = new List<string>() 
        //                                           { 
        //                                               COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_KEY,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_PART_NAME,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_PART_NUMBER,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_PART_REVISION,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_ORDERED,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_ORDER_ITEM_PRIORITY,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_ORDER_ITEM_STATE,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_TIME_ITEM_CREATE,
        //                                               POR_SALES_ORDER_ITEM_FIELDS.FIELD_TIME_PROMISED_SHIP,
        //                                               COMMON_FIELDS.FIELD_COMMON_EDITOR
        //                                           };
        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME, fields);
        //}

        //public static void AddRowDataToUDA(ref DataTable dt, Dictionary<string, string> keyValues)
        //{
        //}

        //public static DataTable CreateSalesOrderItemTable()
        //{
        //    List<string> fields = new List<string>() 
        //                                            {
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_KEY,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_PART_NUMBER,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_PART_REVISION,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_ORDERED,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_ORDER_ITEM_PRIORITY,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_ORDER_ITEM_STATE,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_TIME_ITEM_CREATE,
        //                                                POR_SALES_ORDER_ITEM_FIELDS.FIELD_TIME_PROMISED_SHIP
        //                                            };
        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(TRANS_TABLES.TABLE_SUB_ITEMS, fields);
        //}
        //public static void AddRowDataToSalesOrderItem(ref DataTable dt, Dictionary<string, string> keyValues)
        //{
        //}

        //DELETE WST_LOT_PACKAGE_FIELDS
        //public static DataTable CreateDataTableForInsertPack()
        //{
        //    List<string> fields = new List<string>() 
        //                                            {  
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_PACKAGE_KEY,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_LOT_NUMBER,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_QTY,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_ISDEFECT,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_CLASS,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_BARCODE,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_STATUS,
        //                                                WST_LOT_PACKAGE_FIELDS.FIELD_CREATOR
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WST_LOT_PACKAGE_FIELDS.DATABASE_TABLE_NAME, fields);
        //}

        //DELETE WST_PACKAGE_DETAIL_FIELDS
        //public static DataTable CreateDataTableForInsertPackDetail()
        //{
        //    List<string> fields = new List<string>() 
        //                                            {  
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_ROW_KEY,
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_LOT_NUMBER,
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_QTY,
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_ISDEFECT,
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_CLASS,
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_STATUS,
        //                                                WST_PACKAGE_DETAIL_FIELDS.FIELD_CREATOR
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WST_PACKAGE_DETAIL_FIELDS.DATABASE_TABLE_NAME, fields);
        //}

        #region Param Table
        public static DataTable CreateDataTableForInsertParam()
        {
            List<string> fields = new List<string>() 
                                                    {     
                                                        BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY,
                                                        BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, 
                                                        BASE_PARAMETER_FIELDS.FIELD_DESCRIPTIONS, 
                                                        BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY,
                                                        BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE, 
                                                        BASE_PARAMETER_FIELDS.FIELD_DEFAULT_UOM,
                                                        BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY,
                                                        BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC, 
                                                        BASE_PARAMETER_FIELDS.FIELD_TARGET, 
                                                        BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY,
                                                        BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC, 
                                                        BASE_PARAMETER_FIELDS.FIELD_MANDATORY,
                                                        BASE_PARAMETER_FIELDS.FIELD_STATUS, 
                                                        BASE_PARAMETER_FIELDS.FIELD_ISDERIVED,
                                                        BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE,
                                                        COMMON_FIELDS.FIELD_COMMON_CREATOR,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR,
                                                        BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        #endregion

        //DELETE WIP_BOM_FIELDS
        //public static DataTable CreateDataTableForInsertBom()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                                WIP_BOM_FIELDS.FIELDS_ROW_KEY,
        //                                                WIP_BOM_FIELDS.FIELDS_PARENT_ROW_KEY,
        //                                                WIP_BOM_FIELDS.FIELDS_BOM_NAME,
        //                                                WIP_BOM_FIELDS.FIELDS_BOM_DESC,
        //                                                WIP_BOM_FIELDS.FIELDS_BOM_STATUS,
        //                                                WIP_BOM_FIELDS.FIELDS_EFFECTIVITY_START,
        //                                                WIP_BOM_FIELDS.FIELDS_EFFECTIVITY_END,
        //                                                COMMON_FIELDS.FIELD_COMMON_CREATOR,
        //                                                COMMON_FIELDS.FIELD_COMMON_EDITOR
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WIP_BOM_FIELDS.DATABASE_TABLE_NAME, fields);
        //}

        //DELETE WIP_BOM_FIELDS
        //public static DataTable CreateDataTableForInsertComponent()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                               COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_ROW_KEY,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_BOM_ROW_KEY,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_PROD_NAME,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_PROD_QTY,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_PROD_UOM,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_PROD_UOM_TYPE,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_IS_SUBSTITUTE,
        //                                               WIP_BOM_COMP_FIELDS.FIELDS_SUB_PARENT_ROW_KEY
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //public static DataTable CreateDataTableForInsertProcess()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                               COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
        //                                               WIP_BOM_PROCESS_FIELDS.FIELDS_ROW_KEY,
        //                                               WIP_BOM_PROCESS_FIELDS.FIELDS_COMP_ROW_KEY,
        //                                               WIP_BOM_PROCESS_FIELDS.FIELDS_STEP_KEY,
        //                                               WIP_BOM_PROCESS_FIELDS.FIELDS_STEP_NAME,
        //                                               WIP_BOM_PROCESS_FIELDS.FIELDS_ROUTE_KEY,
        //                                               WIP_BOM_PROCESS_FIELDS.FIELDS_ENTERPRISE_KEY
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //public static DataTable CreateDataTableForInsertAttachment()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                               COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_ROW_KEY,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_BOM_KEY,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_PROD_NAME,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_STEP_KEY,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_STEP_NAME,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_ROUTE_KEY,
        //                                               WIP_BOM_ATTACHMENT_FIELDS.FIELDS_ENTERPRISE_KEY
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME, fields);
        //}

        //#region Operation Table
        //public static DataTable CreateDataTableForInsertOperation()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IMAGE_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IS_REWORKABLE,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_INSTRUCTION_LIST_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDC_LIST_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_CHECKLIST_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SAMPLING_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY,
        //                                                POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY,
        //                                                COMMON_FIELDS.FIELD_COMMON_CREATOR,
        //                                                COMMON_FIELDS.FIELD_COMMON_EDITOR
        //                                            };
        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //#endregion

        //#region Step Table
        //public static DataTable CreateDataTableForInsertStep()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                                COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_DURATION,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_IMAGE_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_DESCRIPTIONS,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_IS_REWORKABLE,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_INSTRUCTION_LIST_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_EDC_LIST_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_CHECKLIST_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_SAMPLING_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_SEQ,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY,
        //                                                POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY,
        //                                                COMMON_FIELDS.FIELD_COMMON_CREATOR,
        //                                                COMMON_FIELDS.FIELD_COMMON_EDITOR
        //                                            };
        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //#endregion

        //#region Route Table
        //public static DataTable CreateDataTableForInsertRoute()
        //{
        //    List<string> fields = new List<string>() 
        //                                            { 
        //                                                POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY,
        //                                                POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME,
        //                                                POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_START,
        //                                                POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_END,
        //                                                POR_ROUTE_ROUTE_VER_FIELDS.FIELD_DESCRIPTION,
        //                                                POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_STATUS,
        //                                                COMMON_FIELDS.FIELD_COMMON_CREATOR,
        //                                                COMMON_FIELDS.FIELD_COMMON_EDITOR
        //                                            };
        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //#endregion

        #region Enterprise Table
        public static DataTable CreateDataTableForInsertEnterprise()
        {
            List<string> fields = new List<string>() 
                                                    { 
                                                        POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY,
                                                        POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME,
                                                        POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION,
                                                        POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS,
                                                        COMMON_FIELDS.FIELD_COMMON_CREATOR,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR
                                                    };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        #endregion

        #region Enterprise and route relation Table
        public static DataTable CreateDataTableForInsertEpAndRtRelation()
        {
            List<string> fields = new List<string>() 
                                                    { 
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
                                                        POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQUENCE_KEY,
                                                        POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY,
                                                        POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ROUTE_VER_KEY,
                                                        POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQ
                                                    };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        #endregion

        #region Line Table
        public static DataTable CreateDataTableForInsertLine()
        {
            List<string> fields = new List<string>()
                                                    {
                                                        FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,
                                                        FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,
                                                        FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE,
                                                        FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS,
                                                        COMMON_FIELDS.FIELD_COMMON_CREATOR,
                                                        COMMON_FIELDS.FIELD_COMMON_CREATE_TIMEZONE
                                                    };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        #endregion

        #region Store Table
        public static DataTable CreateDataTableForInsertStore()
        {
            List<string> fields = new List<string>()
            {
                WST_STORE_FIELDS.FIELD_CREATOR,
                WST_STORE_FIELDS.FIELD_CREATE_TIMEZONE,
                WST_STORE_FIELDS.FIELD_DESCRIPTION,
                WST_STORE_FIELDS.FIELD_LOCATION_KEY,
                WST_STORE_FIELDS.FIELD_OBJECT_STATUS,
                WST_STORE_FIELDS.FIELD_STORE_KEY,
                WST_STORE_FIELDS.FIELD_STORE_NAME,
                WST_STORE_FIELDS.FIELD_STORE_TYPE,
                WST_STORE_FIELDS.FIELD_TYPE_NAME,
                WST_STORE_FIELDS.FIELD_OPERATION_NAME,
                WST_STORE_FIELDS.FIELD_REQUEST_FLAG,
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WST_STORE_FIELDS.DATABASE_TABLE_NAME,fields);
        }
        #endregion

        #region StoreMat Table
        public static DataTable CreateDataTableForInsertStoreMat()
        {
            List<string> fields = new List<string>() 
                                                    { 
                                                        WST_STORE_MAT_FIELDS.FIELD_ROW_KEY,
                                                        WST_STORE_MAT_FIELDS.FIELD_ITEM_NO,
                                                        WST_STORE_MAT_FIELDS.FIELD_ITEM_DESCRIPTION,
                                                        WST_STORE_MAT_FIELDS.FIELD_ITEM_TYPE,
                                                        WST_STORE_MAT_FIELDS.FIELD_ITEM_QTY,
                                                        WST_STORE_MAT_FIELDS.FIELD_ITEM_UNIT,
                                                        WST_STORE_MAT_FIELDS.FIELD_OBJECT_STATUS,
                                                        WST_STORE_MAT_FIELDS.FIELD_STORE_KEY,
                                                        WST_STORE_MAT_FIELDS.FIELD_BILL_NUMBER,
                                                        WST_STORE_MAT_FIELDS.FIELD_WORKORDER_NUMBER,
                                                        WST_STORE_MAT_FIELDS.FIELD_EDITOR,
                                                        WST_STORE_MAT_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        WST_STORE_MAT_FIELDS.FIELD_SERIAL_NUMBER,
                                                        WST_STORE_MAT_FIELDS.FIELD_BAR_CODE,
                                                        WST_STORE_MAT_FIELDS.FIELD_LINE_KEY,
                                                        WST_STORE_MAT_FIELDS.FIELD_ENTERPRISE_KEY,
                                                        WST_STORE_MAT_FIELDS.FIELD_ROUTE_KEY,
                                                        WST_STORE_MAT_FIELDS.FIELD_STEP_KEY,
                                                        WST_STORE_MAT_FIELDS.FIELD_SHIFT_NAME
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        #endregion

        //#region Reason Code Table
        //public static DataTable CreateDataTableForInsertReasonCode()
        //{
        //    List<string> fields = new List<string>() 
        //                                            {  
        //                                                FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY,
        //                                                FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME,
        //                                                FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE,
        //                                                FMM_REASON_CODE_FIELDS.FIELD_DESCRIPTIONS, 
        //                                                FMM_REASON_CODE_FIELDS.FIELD_EDITOR
        //                                            };

        //    return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME, fields);
        //}
        //#endregion

        #region Reason Code Category Table
        public static DataTable CreateDataTableForInsertCodeCategory()
        {
            List<string> fields = new List<string>() 
                                                    {  
                                                        FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY,
                                                        FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME,
                                                        FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_DESCRIPTIONS,
                                                        FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE,
                                                        FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDITOR
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        #endregion


        public static DataTable CreateDataTableForInsertSupplierCode()
        {
            List<string> fields = new List<string>() 
                                                    {  
                                                        "CODE",
                                                        "NAME",
                                                       "NICKNAME",
                                                        "CREATOR"
                                                       
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("BASE_SUPPLIER", fields);
        }

        public static DataTable CreateDataTableForUpdateSupplierCode()
        {
            List<string> fields = new List<string>() 
                                                    {  
                                                        "CODE",
                                                        "NAME",
                                                       "NICKNAME",
                                                        "CREATOR"
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("BASE_SUPPLIER", fields);
        }
        public static DataTable CreateDataTableForInsertBomMarital()
        {
            List<string> fields = new List<string>() 
                                                    {  
                                                        "MATERIAL_CODE",
                                                        "MATERIAL_NAME",
                                                       "BARCODE",
                                                       "MATERIAL_SPEC",
                                                        "CREATOR"
                                                       
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("POR_MATERIAL", fields);
        }
        public static DataTable CreateDataTableForUpdateBomMarital()
        {
            List<string> fields = new List<string>() 
                                                    {  
                                                       "MATERIAL_CODE",
                                                        "MATERIAL_NAME",
                                                       "BARCODE",
                                                       "MATERIAL_SPEC",
                                                        "EDITOR"
                                                    };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("POR_MATERIAL", fields);
        }
        public static DataTable CreateDataTableForUpdateSpecialMatTeam()
        {
            List<string> fields = new List<string>() 
                                                    {  
                                                        "ORDER_NUMBER",
                                                        "MATERIAL_CODE",
                                                        "DESCRIPTION",
                                                        "MATKL"
                                                    };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("POR_WORK_ORDER_BOM_EXTENSION", fields);
        }
    }
}
