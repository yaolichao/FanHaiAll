/*
<FileInfo>
  <Author>ZhangHao and Alfred.Liu SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region
using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Modules.Databases;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.StaticFuncsUtils;
using System.Data.Common;
using SolarViewer.Hemera.Utils.Comm;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using Microsoft.Practices.EnterpriseLibrary.Data;
#endregion


namespace SolarViewer.Hemera.Modules.FMM
{
    public class SalesOrderEngine :AbstractEngine,ISalesOrderEngine
    {
     
        #region constructor
        public SalesOrderEngine()
        {
        }
        #endregion
        public DataSet SalesOrderInsert(DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            if (null != dataSet)
            {
                List<string> sqlCommandList = new List<string>();
                if (dataSet.Tables.Contains(POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME))
                {
                    List<string> sqlCommandSalesOrder = new List<string>();
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandSalesOrder,
                                                        new POR_SALES_ORDER_FIELDS(),
                                                        dataSet.Tables[POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        { 
                                                            {POR_SALES_ORDER_FIELDS.FIELD_CREATE_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_SALES_ORDER_FIELDS.FIELD_CREATE_TIMEZONE, "CN-ZH"},
                                                            {POR_SALES_ORDER_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_SALES_ORDER_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"},
                                                        },
                                                        new List<string>());
                    if (1 == sqlCommandSalesOrder.Count && sqlCommandSalesOrder[0].Length > 20)
                    {
                        sqlCommandList.Add(sqlCommandSalesOrder[0]);

                        if (dataSet.Tables.Contains(POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_SALES_ORDER_ATTR_FIELDS(),
                                                                   dataSet.Tables[POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   { 
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        if (dataSet.Tables.Contains(POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_SALES_ORDER_ITEM_FIELDS(),
                                                                   dataSet.Tables[POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   {
                                                                       {POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER, "0"},
                                                                       {POR_SALES_ORDER_ITEM_FIELDS.FIELD_CREATE_TIMEZONE, "CN-ZH"},
                                                                       {POR_SALES_ORDER_ITEM_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        if (dataSet.Tables.Contains(POR_SALES_ORDER_ITEM_ATTR_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_SALES_ORDER_ITEM_ATTR_FIELDS(),
                                                                   dataSet.Tables[POR_SALES_ORDER_ITEM_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   {  
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }
                        if (sqlCommandList.Count > 0)
                        {
                            Database database = DatabaseFactory.CreateDatabase();
                            DbConnection databaseConn = database.CreateConnection();
                            databaseConn.Open();
                            DbTransaction databaseTrans = databaseConn.BeginTransaction();
                            try
                            {
                                foreach (string sql in sqlCommandList)
                                {
                                    database.ExecuteNonQuery(databaseTrans, CommandType.Text, sql);
                                }
                                databaseTrans.Commit();
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "");
                            }
                            catch (Exception e)
                            {
                                databaseTrans.Rollback();
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, e.Message);
                                LogService.LogError("SalesOrderInsert Error: " + e.Message);
                            }
                            finally
                            {
                                databaseConn.Close();
                            }
                        }
                    }
                    else
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "More than one Sales Order in input parameter");
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Can NOT find Sales Order Parameters DataTable");
                }
            }
            else
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "No Sales Order Table in input paremter.");
            }
            return retDS;
        }
        
        public DataSet SalesOrderUpdate(DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            if (null != dataSet)
            {
                List<string> sqlCommandList = new List<string>();
                if (dataSet.Tables.Contains(POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new POR_SALES_ORDER_FIELDS(),
                        dataSet.Tables[POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME],
                        new Dictionary<string, string>() { 
                                                            {POR_SALES_ORDER_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_SALES_ORDER_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}},
                        new List<string>() { POR_SALES_ORDER_FIELDS.FIELD_SALES_ORDER_KEY },
                        POR_SALES_ORDER_FIELDS.FIELD_SALES_ORDER_KEY);
                }
                if (dataSet.Tables.Contains(POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList, new POR_SALES_ORDER_ATTR_FIELDS(), dataSet.Tables[POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME], POR_SALES_ORDER_ATTR_FIELDS.FIELDS_SALES_ORDER_KEY);
                }
                if (dataSet.Tables.Contains(POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME))
                {
                    foreach (DataRow dataRow in dataSet.Tables[POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME].Rows)
                    {
                        //if (GlobalEnums.OperationAction.New == (GlobalEnums.OperationAction)dtDataFields.Rows[COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION])
                        //{
                        //    // TODO. build insert sql statement
                        //    sqlCommand = "";
                        //    sqlCommandList.Add(sqlCommand);
                        //}
                        //else if (GlobalEnums.OperationAction.Delete == (GlobalEnums.OperationAction)dtDataFields.Rows[COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION])
                        //{
                        //    // TODO. build delete sql statement
                        //    sqlCommand = "DELETE FROM " + tableFields.TABLE_NAME +
                        //                 " WHERE " + tableFiel;
                        //    sqlCommandList.Add(sqlCommand);
                        //}
                    }
                }
                if (dataSet.Tables.Contains(POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"))
                {
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new POR_SALES_ORDER_ITEM_FIELDS(),
                        dataSet.Tables[POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"],
                        new Dictionary<string, string>() { 
                                                            {POR_SALES_ORDER_ITEM_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_SALES_ORDER_ITEM_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}},
                        new List<string>() { POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY },
                        POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY);
                }
                if (dataSet.Tables.Contains(POR_SALES_ORDER_ITEM_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList, new POR_SALES_ORDER_ITEM_ATTR_FIELDS(), dataSet.Tables[POR_SALES_ORDER_ITEM_ATTR_FIELDS.DATABASE_TABLE_NAME], POR_SALES_ORDER_ITEM_ATTR_FIELDS.FIELDS_SALES_ORDER_ITEM_KEY);
                }
                if (sqlCommandList.Count > 0)
                {
                    Database database = DatabaseFactory.CreateDatabase();
                    DbConnection databaseConn = database.CreateConnection();
                    databaseConn.Open();
                    DbTransaction databaseTrans = databaseConn.BeginTransaction();
                    try
                    {
                        foreach (string sql in sqlCommandList)
                        {
                            database.ExecuteNonQuery(databaseTrans, CommandType.Text, sql);
                        }
                        databaseTrans.Commit();
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "");
                    }
                    catch (Exception e)
                    {
                        databaseTrans.Rollback();
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, e.Message);
                        LogService.LogError("SalesOrderUpdate Error: " + e.Message);
                    }
                    finally
                    {
                        databaseConn.Close();
                    }
                }
            }
            else
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "No Sales Order Update information sent to Server");
            }
            return retDS;
        }

        public DataSet SalesOrderDelete(string salesOrderKey)
        {
            DataSet retDS = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = "";
            

            // Please DON'T change the delete sequence, otherwise will get error result
            // 1. DELETE all sales order item's attributes
            sqlCommand = "DELETE FROM " + POR_SALES_ORDER_ITEM_ATTR_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + POR_SALES_ORDER_ITEM_ATTR_FIELDS.FIELDS_SALES_ORDER_ITEM_KEY +
                         " IN(" +
                              "SELECT DISTINCT " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY +
                              " FROM " + POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME +
                              " WHERE " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_KEY + "='" + salesOrderKey + "'" +
                          ")";
            sqlCommandList.Add(sqlCommand);

            // 2. DELETE all sales order item
            sqlCommand = "DELETE FROM " + POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_KEY + "='" + salesOrderKey + "'";
            sqlCommandList.Add(sqlCommand);

            // 3. DELETE all sales order's attributes
            sqlCommand = "DELETE FROM " + POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + POR_SALES_ORDER_ATTR_FIELDS.FIELDS_SALES_ORDER_KEY + "='" + salesOrderKey + "'";
            sqlCommandList.Add(sqlCommand);

            // 4. DELETE sales order
            sqlCommand = "DELETE FROM " + POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + POR_SALES_ORDER_FIELDS.FIELD_SALES_ORDER_KEY + "='" + salesOrderKey + "'";
            sqlCommandList.Add(sqlCommand);

            Database database = DatabaseFactory.CreateDatabase();
            DbConnection databaseConn = database.CreateConnection();
            databaseConn.Open();
            DbTransaction databaseTrans = databaseConn.BeginTransaction();
            try
            {
                foreach (string sql in sqlCommandList)
                {
                    database.ExecuteNonQuery(databaseTrans, CommandType.Text, sql);
                }
                databaseTrans.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "");
            }
            catch (Exception e)
            {
                databaseTrans.Rollback();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, e.Message);
                LogService.LogError("SalesOrderDelete Error: " + e.Message);
            }
            finally
            {
                databaseConn.Close();
            }

            return retDS;
        }
        #region Initialize
        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        #endregion
        #region Get SalesOrder via orderKey
        /// <summary>
        /// Get SalesOrder Via Key
        /// </summary>
        /// <param name="dataset">sale order key</param>
        /// <returns>dataset</returns>
        public DataSet GetSalesOrderByKey(string orderKey)
        {
            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sqlCommand 
            String[] sqlCommand = new String[5];
            try
            {
                if (orderKey != string.Empty)
                {
                    sqlCommand[0] = @"SELECT * FROM POR_SALES_ORDER_ITEM_ATTR
                                       WHERE SALES_ORDER_ITEM_KEY IN
                                             (SELECT DISTINCT SALES_ORDER_ITEM_KEY
                                                FROM POR_SALES_ORDER_ITEM
                                               WHERE SALES_ORDER_KEY = '" + orderKey + "') ORDER BY 1";

                    DataTable dtTable1 = new DataTable();
                    Database db = DatabaseFactory.CreateDatabase();
                    dtTable1 = db.ExecuteDataSet(CommandType.Text, sqlCommand[0]).Tables[0];
                    dtTable1.TableName = POR_SALES_ORDER_ITEM_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    dataDs.Merge(dtTable1, false, MissingSchemaAction.Add);

                    sqlCommand[1] = @"SELECT * FROM POR_SALES_ORDER_ITEM WHERE SALES_ORDER_KEY = '" + orderKey + "' ORDER BY 1";

                    DataTable dtTable2 = new DataTable();
                    dtTable2 = db.ExecuteDataSet(CommandType.Text, sqlCommand[1]).Tables[0];
                    dtTable2.TableName = POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME;
                    dataDs.Merge(dtTable2, false, MissingSchemaAction.Add);

                    sqlCommand[2] = "SELECT a.*, b." + BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE + 
                                    " FROM " + POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME + " a," +
                                    "      " + BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME + " b" + 
                                    " WHERE a." + POR_SALES_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY + " = b." + BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + 
                                    "   AND a." + POR_SALES_ORDER_ATTR_FIELDS.FIELDS_SALES_ORDER_KEY + " = '" + orderKey + "'";
                    //sqlCommand[2] = @"SELECT * FROM POR_SALES_ORDER_ATTR WHERE SALES_ORDER_KEY = '" + orderKey + "' ORDER BY 1";

                    DataTable dtTable3 = new DataTable();
                    dtTable3 = db.ExecuteDataSet(CommandType.Text, sqlCommand[2]).Tables[0];
                    dtTable3.TableName = POR_SALES_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    dataDs.Merge(dtTable3, false, MissingSchemaAction.Add);

                    sqlCommand[3] = @"SELECT * FROM POR_SALES_ORDER WHERE SALES_ORDER_KEY = '" + orderKey + "' ORDER BY 1";

                    DataTable dtTable4 = new DataTable();
                    dtTable4 = db.ExecuteDataSet(CommandType.Text, sqlCommand[3]).Tables[0];
                    dtTable4.TableName = POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME;
                    dataDs.Merge(dtTable4, false, MissingSchemaAction.Add);

                    //add paramter table
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                }

            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("GetSalesOrderByKey Error: " + ex.Message);
            }
            return dataDs;
        }
        #endregion

        #region Search SalesOrders
        /// <summary>
        /// Search SalesOrders
        /// </summary>
        /// <param name="dataset">sale order info</param>
        /// <returns>dataset</returns>
        public DataSet SearchSalesOrders(DataSet dataset)
        {
            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sqlCommand 
            String[] sqlCommand = new String[5];
            try
            {
                if (dataset != null && dataset.Tables.Contains(POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dataset.Tables[POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                    sqlCommand[0] = @"SELECT SALES_ORDER_KEY,
                                   ORDER_NUMBER,
                                   ORDER_STATE,
                                   ORDER_PRIORITY,
                                   ENTERED_TIME,
                                   PROMISED_TIME,
                                   FINISHED_TIME,
                                   SHIPPED_TIME,
                                   CLOSED_TIME,
                                   DESCRIPTIONS
                              FROM POR_SALES_ORDER
                             WHERE ORDER_NUMBER LIKE '%" + hashData["ORDER_NUMBER"] + "%'";

                    if (hashData["ORDER_STATE"] != null)
                    {
                        sqlCommand[0] += "AND ORDER_STATE LIKE '%" + hashData["ORDER_STATE"] + "%'";
                    }

                    if (hashData["ORDER_PRIORITY"] != null)
                    {
                        sqlCommand[0] += "AND ORDER_PRIORITY LIKE '%" + hashData["ORDER_PRIORITY"] + "%'";
                    }

                    sqlCommand[0] += "ORDER BY 1";
                }
                else
                {
                    sqlCommand[0] = "SELECT * FROM POR_SALES_ORDER ORDER BY 1";
                }

                Database database = DatabaseFactory.CreateDatabase();
                DataTable dtTable = database.ExecuteDataSet(CommandType.Text, sqlCommand[0]).Tables[0];
                dtTable.TableName = POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dataDs.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("SearchSalesOrders Error: " + ex.Message);
            }
            return dataDs;
        }
        #endregion

        public DataSet SearchSalesOrderItems(DataSet dataset)
        {
            //get dynamic dataset constructor
            DataSet dataDs = new DataSet();
            //define sqlCommand 
            string sqlCommand = "";
            try
            {
                if (dataset != null && dataset.Tables.Contains(POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dataset.Tables[POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                    sqlCommand = @"SELECT 
                                   a.SALES_ORDER_ITEM_KEY,
                                   b.ORDER_NUMBER,
                                   a.ORDER_ITEM_NAME,
                                   a.QUANTITY_ORDERED,
                                   a.ORDER_ITEM_PRIORITY,
                                   a.ORDER_ITEM_STATE,
                                   a.PART_NUMBER,
                                   a.PART_REVISION
                              FROM POR_SALES_ORDER_ITEM a,
                                   POR_SALES_ORDER b
                             WHERE a.SALES_ORDER_KEY = b.SALES_ORDER_KEY";
                    if (null != hashData["ORDER_NUMBER"])
                    {
                        sqlCommand += "AND b.ORDER_NUMBER LIKE '%" + hashData["ORDER_NUMBER"] + "%'";
                    }
                    if (hashData["ORDER_ITEM_PRIORITY"] != null)
                    {
                        sqlCommand += "AND a.ORDER_ITEM_PRIORITY LIKE =" + hashData["ORDER_PRIORITY"];
                    }
                }
                else
                {
                    sqlCommand = @"SELECT 
                                   a.SALES_ORDER_ITEM_KEY,
                                   b.ORDER_NUMBER,
                                   a.ORDER_ITEM_NAME,
                                   a.QUANTITY_ORDERED,
                                   a.ORDER_ITEM_PRIORITY,
                              FROM POR_SALES_ORDER_ITEM a,
                                   POR_SALES_ORDER b
                             WHERE a.SALES_ORDER_KEY = b.SALES_ORDER_KEY";
                }

                sqlCommand += " AND QUANTITY_ORDERED - QUANTITY_JOINED_WORKORDER > 0";

                Database database = DatabaseFactory.CreateDatabase();
                DataTable dtTable = database.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_SALES_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dataDs.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("SearchSalesOrderItems Error: " + ex.Message);
            }
            //dataDs.WriteXml(@"d:\Server-SearchSalesOrderItems.xml");
            return dataDs;
        }


        /// <summary>
        /// GetColumnsforSalesOrder 
        /// </summary>
        /// <param name="dataset">sales order key</param>
        /// <returns>saleorder's attribute </returns>
        public DataSet GetAttributsColumnsForSalesOrder()
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();
            //define sql 
            string sql = "";

            try
            {
                sql = " SELECT BASE_ATTRIBUTE.ATTRIBUTE_KEY, BASE_ATTRIBUTE.ATTRIBUTE_NAME,BASE_ATTRIBUTE.DESCRIPTION,BASE_ATTRIBUTE.DATA_TYPE,'' AS DATA_TYPESTRING";
                sql += " FROM BASE_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY";
                sql += " WHERE BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY =BASE_ATTRIBUTE.CATEGORY_KEY AND BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME='Uda_sales_order'";
                //excute sql
                Database database = DatabaseFactory.CreateDatabase();
                dataDs = database.ExecuteDataSet(CommandType.Text, sql);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
            }
            catch (Exception ex)
            {
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("GetAttributsColumnsForSalesOrder Error: " + ex.Message);
            }
            return dataDs;
        }

        #region GetAttributsColumnsForSalesOrderItem
        /// <summary>
        /// GetAttributsColumnsForSalesOrderItem
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetAttributsColumnsForSalesOrderItem()
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();
            //define sql 
            string sql = "";

            try
            {
                sql = " SELECT BASE_ATTRIBUTE.ATTRIBUTE_KEY, BASE_ATTRIBUTE.ATTRIBUTE_NAME,BASE_ATTRIBUTE.DESCRIPTION,BASE_ATTRIBUTE.DATA_TYPE,'' AS DATA_TYPESTRING";
                sql += " FROM BASE_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY";
                sql += " WHERE BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY =BASE_ATTRIBUTE.CATEGORY_KEY AND BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME='Uda_sales_order_item'";
                //excute sql

                Database database = DatabaseFactory.CreateDatabase();
                dataDs = database.ExecuteDataSet(CommandType.Text, sql);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

            }
            catch (Exception ex)
            {
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("GetAttributsColumnsForSalesOrderItem Error: " + ex.Message);
            }
            return dataDs;
        }
        #endregion
    }
}
