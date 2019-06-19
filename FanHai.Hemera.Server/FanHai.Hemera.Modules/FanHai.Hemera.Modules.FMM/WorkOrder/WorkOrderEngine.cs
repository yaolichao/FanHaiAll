using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.StaticFuncsUtils;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Utils.Comm;

namespace SolarViewer.Hemera.Modules.FMM
{
    public class WorkOrderEngine : AbstractEngine, IWorkOrderEngine
    {
        public DataSet WorkOrderInsert(DataSet dataSet)
        {
            //dataSet.WriteXml(@"d:\WorkOrderInsert.xml");
            DataSet retDS = new DataSet();
            if (null != dataSet)
            {
                List<string> sqlCommandList = new List<string>();
                Dictionary<string, string> sqlCommandUpdateSalesOrderItem = new Dictionary<string,string>();
                if (dataSet.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                        new POR_WORK_ORDER_FIELDS(),
                                                        dataSet.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        { 
                                                            {POR_WORK_ORDER_FIELDS.FIELD_NEXT_SEQ, "001"}, 
                                                            {POR_WORK_ORDER_FIELDS.FIELD_CREATE_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_WORK_ORDER_FIELDS.FIELD_CREATE_TIMEZONE, "CN-ZH"},
                                                            {POR_WORK_ORDER_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_WORK_ORDER_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"},
                                                        },
                                                        new List<string>());
                    if (1 == sqlCommandList.Count && sqlCommandList[0].Length > 20)
                    {
                        // Work Order Attributes
                        if (dataSet.Tables.Contains(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_WORK_ORDER_ATTR_FIELDS(),
                                                                   dataSet.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                   new Dictionary<string, string>() 
                                                                   { 
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                        }

                        // Work Order Items
                        if (dataSet.Tables.Contains(POR_WORK_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DataTable dtWorkOrderItems = dataSet.Tables[POR_WORK_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME];
                            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                   new POR_WORK_ORDER_ITEM_FIELDS(),
                                                                   dtWorkOrderItems,
                                                                   new Dictionary<string, string>() 
                                                                   {
                                                                       {POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_JOIN_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                       {POR_WORK_ORDER_ITEM_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                       {POR_WORK_ORDER_ITEM_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                   new List<string>() 
                                                                   {
                                                                       POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_JOIN_TIME,
                                                                       COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION 
                                                                   });
                            // Should Build SQL statements for update POR_SALES_ORDER_ITEM table, eg: order quantity
                            string sqlCommand = "";
                            foreach (DataRow dataRow in dtWorkOrderItems.Rows)
                            {
                                if (GlobalEnums.OperationAction.New == (GlobalEnums.OperationAction)Convert.ToInt32(dataRow[COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]))
                                {
                                    sqlCommand = "UPDATE " + POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME + 
                                                 "   SET " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER + " = " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER + " + " +  Convert.ToInt32(dataRow[POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY]) +
                                                 " WHERE " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY + " = '" + dataRow[POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY].ToString() + "'";
                                    sqlCommandUpdateSalesOrderItem.Add(dataRow[POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY].ToString(), sqlCommand);
                                }
                            }
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
                                if (sqlCommandUpdateSalesOrderItem.Count > 0)
                                {
                                    string sqlCommand = "";
                                    int remainOrderedQuantity = 0;
                                    foreach(KeyValuePair<string, string> keyValue in sqlCommandUpdateSalesOrderItem)
                                    {
                                        database.ExecuteNonQuery(databaseTrans, CommandType.Text, keyValue.Value);
                                        // Validation
                                        sqlCommand = "SELECT " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_ORDERED + ", "  +
                                                                 POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER +  ", " + 
                                                                 POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_ORDERED + " - " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER + " as GAP" + 
                                                     "  FROM " + POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME + 
                                                     " WHERE " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY + " = '" + keyValue.Key + "'";
                                        DataSet ds = database.ExecuteDataSet(databaseTrans, CommandType.Text, sqlCommand);
                                        if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            remainOrderedQuantity = Convert.ToInt32(ds.Tables[0].Rows[0]["GAP"]);
                                            if (0 == remainOrderedQuantity)
                                            {
                                                //sqlCommand = "UPDATE " + POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME +
                                                //             "   SET " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_ORDER_ITEM_STATE + " = 2" +
                                                //             " WHERE " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY + " = '" + keyValue.Key + "'";
                                                //database.ExecuteNonQuery(databaseTrans, CommandType.Text, sqlCommand);
                                            }
                                            else if (remainOrderedQuantity < 0)
                                            {
                                                databaseTrans.Rollback();
                                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "No enough order quantity in Sales Order Item.");
                                                return retDS;
                                            }
                                        }
                                    }
                                }
                                databaseTrans.Commit();
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "");
                            }
                            catch (Exception ex)
                            {
                                databaseTrans.Rollback();
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                                LogService.LogError("WorkOrderInsert Error: " + ex.Message);
                            }
                            finally
                            {
                                databaseConn.Close();
                            }
                        }
                    }
                    else
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "More than one Work Order in input parameter");
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Can NOT find Work Order Parameters DataTable");
                }
            }
            else
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "No Work Order Tables in input paremter.");
            }
            return retDS;
        }

        public DataSet WorkOrderUpdate(DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            return retDS;
        }

        public DataSet WorkOrderDelete(string workOrderKey)
        {
            DataSet retDS = new DataSet();
            List<string> sqlCommandList = new List<string>();
            string sqlCommand = "";

            // 1. Validate, is link to lot 
            sqlCommand = "SELECT " + POR_LOT_FIELDS.FIELD_LOT_NUMBER +
                         "  FROM " + POR_LOT_FIELDS.DATABASE_TABLE_NAME +
                         " WHERE " + POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY + " = '" + workOrderKey + "'";

            Database database = DatabaseFactory.CreateDatabase();
            DataSet validateDS = database.ExecuteDataSet(CommandType.Text, sqlCommand);
            if (null != validateDS && validateDS.Tables.Count > 0 && validateDS.Tables[0].Rows.Count > 0)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Has already generated lot for this Work Order, can NOT delete anymore");
            }
            else
            {
                // 1. update sales order item field: quantity_joined_workorder
                sqlCommand = "SELECT " + POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY + ", " 
                                       + POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY +
                             "  FROM " + POR_WORK_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME +
                             " WHERE " + POR_WORK_ORDER_ITEM_FIELDS.FIELD_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                DataSet woiDS = database.ExecuteDataSet(CommandType.Text, sqlCommand);
                if (null != woiDS && woiDS.Tables.Count > 0 && woiDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in woiDS.Tables[0].Rows)
                    {
                        sqlCommand = "UPDATE " + POR_SALES_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME +
                                     "   SET " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER + " = " + POR_SALES_ORDER_ITEM_FIELDS.FIELD_QUANTITY_JOINED_WORKORDER + " - " + dataRow[POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY].ToString() +
                                     " WHERE " + POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY + " = '" + dataRow[POR_SALES_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_KEY].ToString() + "'";
                        sqlCommandList.Add(sqlCommand);
                    }
                    sqlCommand = "DELETE FROM " + POR_WORK_ORDER_ITEM_FIELDS.DATABASE_TABLE_NAME +
                                 " WHERE " + POR_WORK_ORDER_ITEM_FIELDS.FIELD_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                    sqlCommandList.Add(sqlCommand);
                }

                // 2. delete work order attributes
                sqlCommand = "DELETE FROM " + POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME +
                             " WHERE " + POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                sqlCommandList.Add(sqlCommand);
                
                // 3. delete work order
                sqlCommand = "DELETE FROM " + POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME +
                             " WHERE " + POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                sqlCommandList.Add(sqlCommand);

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
                catch (Exception ex)
                {
                    databaseTrans.Rollback();
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                    LogService.LogError("WorkOrderDelete Error: " + ex.Message);
                }
                finally
                {
                    databaseConn.Close();
                }
            }

            return retDS;
        }


        private Database db = null;  
        #region Initialize
        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        #endregion

        #region AddWorkOrder
        
        public DataSet AddWorkOrder(DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            if (null == dataSet || !dataSet.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME))
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "WorkOrderEngine::AddWorkOrder - Invalidate Parameter");
                return retDS;
            }
            string sqlCommand = "";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection dbConn = db.CreateConnection())
            {
                DataSet dSet = (DataSet)dataSet;
                DataSet dsReturn = new DataSet();
                try
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    try
                    {
                        Hashtable dtWorkOrder =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataSet.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME]);
                        POR_WORK_ORDER_FIELDS workOrderFields = new POR_WORK_ORDER_FIELDS();
                        sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderFields, dtWorkOrder, null);
                        db = DatabaseFactory.CreateDatabase();
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        if (dataSet.Tables.Contains(TRANS_TABLES.TABLE_UDAS))
                        {
                            POR_WORK_ORDER_ATTR_FIELDS workOrderUDAsFields = new POR_WORK_ORDER_ATTR_FIELDS();
                            foreach (DataRow dataRow in dataSet.Tables[TRANS_TABLES.TABLE_UDAS].Rows)
                            {

                                Hashtable fields = new Hashtable()
                                                     {
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY, dataRow[COMMON_FIELDS.FIELD_COMMON_LINKED_ITEM_KEY]},
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY, dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]},
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME, dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]},
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE, dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]},
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR, dataRow[COMMON_FIELDS.FIELD_COMMON_EDITOR]},
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDIT_TIME, DateTime.Now},
                                                         {POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDIT_TIMEZONE, dataRow[COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE]}
                                                     };
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderUDAsFields, fields, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        dbTran.Commit();
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "");
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                        LogService.LogError("AddWorkOrder Error: " + ex.Message);
                    }
                    finally
                    {
                        dbConn.Close();
                    }
                }
                catch (Exception ex)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                    LogService.LogError("AddWorkOrder Error: " + ex.Message);
                }
            }

                return retDS;
        
        }
        #endregion


        #region GetWorkOrder
        /// <summary>
        /// GetWorkOrder
        /// </summary>
        /// <param name="dataset">work order's info</param>
        /// <returns>dataset</returns>
        public object GetWorkOrder(object dataset)
        {
            string orderNumber = (string)dataset;
            DataSet returnData = new DataSet();
            returnData = null;
            string sql = "";
            try
            {
                db = DatabaseFactory.CreateDatabase();
                if (orderNumber != "")
                {
                    sql = "select * from por_work_order where order_number = '" + orderNumber + "'";
                    returnData = db.ExecuteDataSet(CommandType.Text, sql);
                }
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnData, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnData, ex.Message);
                LogService.LogError("GetWorkOrder Error: " + ex.Message);
            }
            return returnData;
        }
        #endregion

        #region GetWorkOrder
        /// <summary>
        /// 根据工单主键获取工单数据。
        /// </summary>
        /// <param name="orderKey">工单主键值。</param>
        /// <returns>
        /// 查询得到的包含工单信息的数据集对象。该数据集对象包含两个数据表对象。
        /// 一个包含工单信息的数据表对象。
        /// 一个名称为paraTable的数据表对象。
        /// paraTable的数据表对象存放查询的执行结果，
        /// paraTable数据表对象中包含两列“ATTRIBUTE_KEY”和“ATTRIBUTE_VALUE”。
        /// 最多包含两行：“ATTRIBUTE_KEY”列等于“CODE”的行和“ATTRIBUTE_KEY”列等于“MESSAGE”的行。
        /// </returns>
        public object GetWorkOrderByOrderKey(object orderKey)
        {
            System.DateTime startTime = System.DateTime.Now;

            string work_order_key = (string)orderKey;
            DataSet returnData = new DataSet();
            returnData = null;
            string sql = "";
            try
            {
                db = DatabaseFactory.CreateDatabase();
                if (work_order_key != "")
                {
                    sql = "select * from por_work_order where work_order_key = '" + work_order_key + "'";
                    returnData = db.ExecuteDataSet(CommandType.Text, sql);
                }
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnData, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnData, ex.Message);
                LogService.LogError("GetWorkOrderByOrderKey Error: " + ex.Message);
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("GetWorkOrderByOrderKey Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return returnData;
        }
        #endregion

        #region GetWorkOrder
        /// <summary>
        /// GetWorkOrder
        /// </summary>
        /// <param name="dataset">work order's info</param>
        /// <returns>dataset</returns>
        public DataSet GetWorkOrderByKey(object workOrderKey)
        {
            string strWorkOrerKey = (string)workOrderKey;
            DataSet retDataSet = new DataSet();
            string sqlCommand = "";
            try
            {
                db = DatabaseFactory.CreateDatabase();
                if (null != strWorkOrerKey && strWorkOrerKey.Length > 0)
                {
                    // Work Order Basic Properties
                    sqlCommand = "select * from por_work_order where work_order_key = '" + strWorkOrerKey + "'";
                    DataTable dtWorkOrder = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (null != dtWorkOrder && dtWorkOrder.Rows.Count > 0)
                    {
                        dtWorkOrder.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                        retDataSet.Merge(dtWorkOrder, false, MissingSchemaAction.Add);

                        // WorkOrder UDAs
                        sqlCommand = @"SELECT " + POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY + ", " +
                                                  POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY + ", " +
                                                  POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME + ", " +
                                                  POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE +
                                      " FROM " + POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME +
                                      " WHERE " + POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY + " = '" + strWorkOrerKey + "'";
                        DataTable dtWorkOrderAttr = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        if (null != dtWorkOrderAttr && dtWorkOrderAttr.Rows.Count > 0)
                        {
                            dtWorkOrderAttr.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                            retDataSet.Merge(dtWorkOrderAttr, false, MissingSchemaAction.Add);
                        }
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDataSet, "");
                    }
                    else
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDataSet, "WorkOrderEngine::GetWorkOrderByKey - Can NOT found record by key[" + strWorkOrerKey + "]");
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDataSet, "WorkOrderEngine::GetWorkOrderByKey - Empty Parameter");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDataSet, ex.Message);
                LogService.LogError("GetWorkOrderByKey Error: " + ex.Message);
            }

            return retDataSet;
        }
        #endregion

        #region SearchWorkOrder
        /// <summary>
        /// 搜索工单信息。可以使用工单号（ORDER_NUMBER），工单状态（ORDER_STATE)和工单优先级(ORDER_PRIORITY)作为搜索条件。
        /// </summary>
        /// <param name="dataset">
        /// 包含查询条件的数据集对象。若设置查询条件，数据集中必须包含数据表名为<see cref="POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象，
        /// 数据表对象包含两个列分别为NAME和VALUE。 列NAME存放查询条件名，列VALUE存放查询条件值。
        /// 列NAME可以存放如下查询条件：工单号（ORDER_NUMBER）(可选，模糊查询)、
        /// 工单状态（ORDER_STATE)(可选，模糊查询）、工单优先级(ORDER_PRIORITY)(可选，模糊查询）。
        /// </param>
        /// <returns>查询得到的包含工单信息的数据集对象。该数据集对象包含两个数据表对象。
        /// 一个名称为<see cref="POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// 一个名称为paraTable的数据表对象。
        /// POR_WORK_ORDER数据表存放查询得到的工单信息。paraTable的数据表对象存放查询的执行结果，paraTable数据表对象中包含两列“ATTRIBUTE_KEY”和“ATTRIBUTE_VALUE”。最多包含两行：“ATTRIBUTE_KEY”列等于“CODE”的行和“ATTRIBUTE_KEY”列等于“MESSAGE”的行。
        /// </returns>
        public object SearchWorkOrder(DataSet dataSet)
        {
            DataSet returnDS = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            string sqlCommand = "";

            try
            {
                //查询条件数据集不为null，且包含名称为POR_WORK_ORDER的数据表对象。
                if (dataSet != null && dataSet.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dataSet.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                    sqlCommand = @"SELECT WORK_ORDER_KEY,
                                   ORDER_NUMBER,
                                   ORDER_STATE,
                                   ORDER_PRIORITY,
                                   ENTERED_TIME,
                                   PROMISED_TIME
                              FROM POR_WORK_ORDER
                             WHERE ORDER_NUMBER LIKE '%" + hashData["ORDER_NUMBER"] + "%'";
                    //包含工单状态的查询条件
                    if (hashData["ORDER_STATE"] != null)
                    {
                        sqlCommand += "AND ORDER_STATE LIKE '%" + hashData["ORDER_STATE"] + "%'";
                    }
                    //包含工单优先级的查询条件
                    if (hashData["ORDER_PRIORITY"] != null)
                    {
                        sqlCommand += "AND ORDER_PRIORITY LIKE '%" + hashData["ORDER_PRIORITY"] + "%'";
                    }
                }
                else
                {
                     sqlCommand = "SELECT * FROM POR_WORK_ORDER";
                }
                DataTable dtTable = new DataTable();
                db = DatabaseFactory.CreateDatabase();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                //将查询得到的数据合并到数据集中
                returnDS.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnDS, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnDS, ex.Message);
                LogService.LogError("SearchWorkOrder Error: " + ex.Message);
            }
            //返回数据集
            return returnDS;
        }
        #endregion

        #region GetAttributesForWorkOrder
        /// <summary>
        /// update UDA for work order
        /// </summary>
        /// <param name="dataset">work order's info</param>
        /// <returns>dataset</returns>
        public object GetAttributesForWorkOrder(object dataset)
        {
            return null;
        }
        #endregion

        #region GetAttributsForWorkOrder
        /// <summary>
        /// GetAttributsColumnsForWorkOrder
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetAttributsForWorkOrder(DataSet dataset)
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();
            //define sql 
            string sql = "";
            db = DatabaseFactory.CreateDatabase();
            try
            {
                if (dataset != null && (dataset.Tables[0].Rows.Count > 0))
                {
                    sql = " SELECT POR_WORK_ORDER_ATTR.WORK_ORDER_KEY AS ORDERKEY, ";
                    sql += "POR_WORK_ORDER_ATTR.ATTRIBUTE_KEY AS ATTRIBUTE_KEY,    ";
                    sql += "POR_WORK_ORDER_ATTR.ATTRIBUTE_NAME AS ATTRIBUTE_NAME,  ";
                    sql += "POR_WORK_ORDER_ATTR.ATTRIBUTE_VALUE AS ATTRIBUTE_VALUE,";
                    sql += "'' AS LAST_UPDATETIME, ";
                    sql += "BASE_ATTRIBUTE.DATA_TYPE AS DATA_TYPE, ";
                    sql += "'' AS FLAG ";
                    sql += " FROM POR_WORK_ORDER_ATTR,BASE_ATTRIBUTE ";
                    sql += " WHERE BASE_ATTRIBUTE.ATTRIBUTE_KEY = POR_WORK_ORDER_ATTR.ATTRIBUTE_KEY ";

                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        sql += " AND " + dataset.Tables[0].Rows[i][0].ToString();
                        sql += " =" + dataset.Tables[0].Rows[i][1];
                    }
                }
                else
                {
                    sql = " SELECT POR_WORK_ORDER_ATTR.WORK_ORDER_KEY AS ORDERKEY, ";
                    sql += "POR_WORK_ORDER_ATTR.ATTRIBUTE_KEY AS ATTRIBUTE_KEY,    ";
                    sql += "POR_WORK_ORDER_ATTR.ATTRIBUTE_NAME AS ATTRIBUTE_NAME,  ";
                    sql += "POR_WORK_ORDER_ATTR.ATTRIBUTE_VALUE AS ATTRIBUTE_VALUE,";
                    sql += "'' AS LAST_UPDATETIME, ";
                    sql += "BASE_ATTRIBUTE.DATA_TYPE AS DATA_TYPE, ";
                    sql += "'' AS FLAG ";
                    sql += " FROM POR_WORK_ORDER_ATTR,BASE_ATTRIBUTE ";
                    sql += " WHERE BASE_ATTRIBUTE.ATTRIBUTE_KEY = POR_WORK_ORDER_ATTR.ATTRIBUTE_KEY ";

                }
                //excute sql
                dataDs = db.ExecuteDataSet(CommandType.Text, sql);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

            }
            catch (Exception ex)
            {
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("GetAttributsForWorkOrder Error: " + ex.Message);
            }

            return dataDs;
        }
        #endregion

        #region GetAttributsColumnsForWorkOrder
        /// <summary>
        /// GetAttributsColumnsForWorkOrder
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetAttributsColumnsForWorkOrder()
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();
            //define sql 
            string sql = "";
            db = DatabaseFactory.CreateDatabase();
            try
            {
                sql = " SELECT BASE_ATTRIBUTE.ATTRIBUTE_KEY, BASE_ATTRIBUTE.ATTRIBUTE_NAME,BASE_ATTRIBUTE.DESCRIPTION,BASE_ATTRIBUTE.DATA_TYPE,'' AS DATA_TYPESTRING";
                sql += " FROM BASE_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY";
                sql += " WHERE BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY =BASE_ATTRIBUTE.CATEGORY_KEY AND BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME='Uda_work_order'";
                //excute sql
                dataDs = db.ExecuteDataSet(CommandType.Text, sql);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

            }
            catch (Exception ex)
            {
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("GetAttributsColumnsForWorkOrder Error: " + ex.Message);
            }

            return dataDs;
        }
        #endregion


        #region UpdateWorkOrder
        /// <summary>
        /// update work order
        /// </summary>
        /// <param name="dataset">work order's info</param>
        /// <returns>bool</returns>
        public object UpdateWorkOrder(object dataset)
        {
            DataSet dSet = (DataSet)dataset;
            DataSet dsReturn = new DataSet();
            try
            {
                if (dSet.Tables.Contains("param"))
                {
                    DataTable dataTable = dSet.Tables["param"];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    db = DatabaseFactory.CreateDatabase();
                    string sql = "UPDATE POR_WORK_ORDER SET " +
                          "ORDER_STATE=" + hashData[POR_WORK_ORDER_FIELDS.FIELD_ORDER_STATE] + "," +
                          "ORDER_PRIORITY=" + hashData[POR_WORK_ORDER_FIELDS.FIELD_ORDER_PRIORITY].ToString() + "," +
                          "QUANTITY_ORDERED='" + hashData[POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_ORDERED].ToString() + "'," +
                          "PART_NUMBER='" + hashData[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER].ToString() + "'," +
                          "ENTERED_TIME=to_date('" + hashData[POR_WORK_ORDER_FIELDS.FIELD_ENTERED_TIME] + "','yyyy-MM-DD hh24:mi:ss')," +
                          "PLANNED_START_TIME=to_date('" + hashData[POR_WORK_ORDER_FIELDS.FIELD_PLANNED_START_TIME] + "','yyyy-MM-DD hh24:mi:ss')," +
                          "PLANNED_FINISH_TIME=to_date('" + hashData[POR_WORK_ORDER_FIELDS.FIELD_PLANNED_FINISH_TIME] + "','yyyy-MM-DD hh24:mi:ss')," +
                          "PART_REVISION='" + hashData[POR_WORK_ORDER_FIELDS.FIELD_PART_REVISION] + "'," +
                          "DESCRIPTIONS='" + hashData[POR_WORK_ORDER_FIELDS.FIELD_DESCRIPTIONS].ToString() + "' " +
                          "WHERE ORDER_NUMBER='" + hashData[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString() + "'";
                    if (db.ExecuteNonQuery(CommandType.Text, sql) == 1)
                    {
                        //delete attribute rows
                        db.ExecuteNonQuery(CommandType.Text, "DELETE FROM POR_WORK_ORDER_ATTR WHERE WORK_ORDER_KEY=" + hashData["order_number"].ToString());
                        if (dSet.Tables["POR_WORK_ORDER_ATTR"] != null)
                        {
                            //add uda data to table
                            for (int i = 0; i < dSet.Tables["POR_WORK_ORDER_ATTR"].Rows.Count; i++)
                            {
                                sql = " INSERT INTO POR_WORK_ORDER_ATTR(WORK_ORDER_KEY,ATTRIBUTE_KEY,ATTRIBUTE_NAME,ATTRIBUTE_VALUE) VALUES (" + hashData["order_number"].ToString();
                                sql += " , " + dSet.Tables["POR_WORK_ORDER_ATTR"].Rows[i]["ATTRIBUTE_KEY"];
                                sql += " , '" + dSet.Tables["POR_WORK_ORDER_ATTR"].Rows[i]["ATTRIBUTE_NAME"];
                                sql += "' , '" + dSet.Tables["POR_WORK_ORDER_ATTR"].Rows[i]["ATTRIBUTE_VALUE"] + "' )";

                                db.ExecuteNonQuery(CommandType.Text, sql);
                            }
                        }
                    }
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }

            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateWorkOrder Error: " + ex.Message);
            }

            return dsReturn;
        }
        #endregion


        #region deleteWorkOrder
        /// <summary>
        /// delete work order
        /// </summary>
        /// <param name="dataset">work order's info</param>
        /// <returns>dataset</returns>
        public DataSet DeleteWorkOrder(string workOrderKey)
        {
            DataSet retDS = new DataSet();
            string sqlCommand = "";

            if (null != workOrderKey && 0 != workOrderKey.Length)
            {
                Database database = DatabaseFactory.CreateDatabase();
                // Check 
                sqlCommand = "SELECT " + POR_LOT_FIELDS.FIELD_LOT_KEY +
                             " FROM " + POR_LOT_FIELDS.DATABASE_TABLE_NAME +
                             " WHERE " + POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                DataSet lotDS = database.ExecuteDataSet(CommandType.Text, sqlCommand);
                if (null != lotDS && lotDS.Tables.Count > 0 && lotDS.Tables[0].Rows.Count > 0)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Can NOT DELETE due to already has lot generated using this WorkOrder");
                }
                else
                {
                    List<string> sqlCommandList = new List<string>();
                    // 1. Delete Work Order Attributes
                    sqlCommand = "DELETE FROM " + POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME +
                                 " WHERE " + POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                    sqlCommandList.Add(sqlCommand);
                    // 2. Delete Work Order
                    sqlCommand = "DELETE FROM " + POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME +
                                 " WHERE " + POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY + " = '" + workOrderKey + "'";
                    sqlCommandList.Add(sqlCommand);

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
                    catch (Exception ex)
                    {
                        databaseTrans.Rollback();
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                        LogService.LogError("DeleteWorkOrder Error: " + ex.Message);
                    }
                    finally
                    {
                        databaseConn.Close();
                    }
                }
            }
            else
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Error Inpute Parameters");
            }
            return retDS;
        }
        #endregion



        #region DeleteUDAForWorkOrder
        /// <summary>
        /// update UDA for work order
        /// </summary>
        /// <param name="dataset">work order's info</param>
        /// <returns>dataset</returns>
        public object DeleteUDAForWorkOrder(object dataset)
        {
            return null;
        }
        #endregion

        #region CheckOrderNumber
        public bool CheckOrderNumber(string orderNumber)
        {
            bool isExist = false;
            try
            {
                db = DatabaseFactory.CreateDatabase();                
                string sql = "SELECT ORDER_NUMBER FROM POR_WORK_ORDER WHERE ORDER_NUMBER='" + orderNumber + "'";
                DataSet ds = db.ExecuteDataSet(CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    isExist = true;
                }

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                LogService.LogError("CheckOrderNumber Error: " + ex.Message);
            }
            return isExist;
        }
        #endregion       

        #region GetHelpInfoForHelpForm
        /// <summary>
        /// GetHelpInfoForHelpForm
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetHelpInfoForHelpForm(DataSet dataset)
        {
            //define return dataset
            DataSet dataSet = new DataSet();
            dataSet = null;
            try
            {
                //create db
                db = DatabaseFactory.CreateDatabase();
                string sql = " SELECT PART_NAME,PART_VERSION,PART_KEY,PART_TYPE,PART_MODULE FROM POR_PART WHERE PART_NAME LIKE '%" + dataset.Tables[0].Rows[0][1].ToString() + "%'";
                sql += " AND PART_STATUS=1 AND ROWNUM <=" + dataset.Tables[0].Rows[1][1].ToString();
                sql+=" ORDER BY PART_NAME";
                dataSet = db.ExecuteDataSet(CommandType.Text, sql);
                //add parameter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataSet, "");
            }
            catch (Exception ex)
            {
                //add parameter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataSet, ex.Message);
                LogService.LogError("GetHelpInfoForHelpForm Error: " + ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region GetHelpInfoForWorkOrderHelpForm
        /// <summary>
        /// GetHelpInfoForWorkOrderHelpForm
        /// </summary>
        /// <param name="dataset">dataset</param>
        /// <returns></returns>
        public DataSet GetHelpInfoForWorkOrderHelpForm(DataSet dataset)
        {
            //define return dataset
            DataSet dataSet = new DataSet();
            dataSet = null;
            try
            {
                //create db
                db = DatabaseFactory.CreateDatabase();
                //string sql = " SELECT A.WORK_ORDER_KEY,A.QUANTITY_LEFT,A.ORDER_NUMBER,A.NEXT_SEQ,A.PART_NUMBER FROM POR_WORK_ORDER A ";
                //sql += " WHERE A.WORK_ORDER_KEY LIKE '%" + dataset.Tables[0].Rows[0][1].ToString() + "%' ";
                //sql += " AND A.ROWNUM <=" + dataset.Tables[0].Rows[1][1].ToString();
                //sql += " ORDER BY A.PART_NUMBER";
                string sql = @" SELECT A.WORK_ORDER_KEY,
                                   A.QUANTITY_LEFT,
                                   A.ORDER_NUMBER,
                                   A.NEXT_SEQ,
                                   A.PART_NUMBER,
                                   A.Part_Key,
                                   A.Module,
                                   A.Type,
                                   B.Cur_Enterprise_Ver_Key,
                                   B.CUR_ROUTE_VER_KEY,
                                   B.Cur_Step_Ver_Key,
                                   C.Enterprise_Name,
                                   C.ENTERPRISE_VERSION,
                                   D.Route_Name,
                                   E.Route_Step_Name
                              FROM POR_WORK_ORDER A
                              left join por_part B on a.part_key=b.part_key
                              LEFT JOIN POR_ROUTE_ENTERPRISE_VER C ON C.ROUTE_ENTERPRISE_VER_KEY=  B.CUR_ENTERPRISE_VER_KEY
                              LEFT JOIN POR_ROUTE_ROUTE_VER D ON D.ROUTE_ROUTE_VER_KEY=B.CUR_ROUTE_VER_KEY
                              LEFT JOIN POR_ROUTE_STEP E ON E.ROUTE_STEP_KEY=B.CUR_STEP_VER_KEY
                              where A.ORDER_NUMBER like '%" + dataset.Tables[0].Rows[0][1].ToString() + "%' AND A.ORDER_STATE=1 AND ROWNUM<=" + dataset.Tables[0].Rows[1][1].ToString();
                dataSet = db.ExecuteDataSet(CommandType.Text, sql);
                //add parameter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataSet, "");
            }
            catch (Exception ex)
            {
                //add parameter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataSet, ex.Message);
                LogService.LogError("GetHelpInfoForWorkOrderHelpForm Error: " + ex.Message);
            }
            return dataSet;
        }
        #endregion

        #region update number of in process
        /// <summary>
        /// update number of in process
        /// </summary>
        /// <param name="dataset">the number needed to update</param>
        /// <returns></returns>
        public DataSet UpdateNumberOfInProcess(DataSet dataset)
        {
            DataSet dSet = (DataSet)dataset;
            DataSet dsReturn = new DataSet();
            using (DbConnection dbconn = db.CreateConnection())
            {
                //set transaction
                DbTransaction trans = dbconn.BeginTransaction();
                try
                {
                    if (dSet.Tables.Contains("param"))
                    {
                        DataTable dataTable = dSet.Tables["param"];
                        Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                        db = DatabaseFactory.CreateDatabase();
                        string sql = "UPDATE POR_WORK_ORDER SET " +
                              "QUANTITY_IN_PROGRESS= QUANTITY_IN_PROGRESS + " + hashData[POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_IN_PROGRESS].ToString() + "," +
                              "WHERE WORK_ORDER_KEY='" + hashData[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString() + "'";
                        db.ExecuteNonQuery(trans,CommandType.Text, sql);
                        //commit transaction
                        trans.Commit();
                        //add message table
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
                catch (Exception ex)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("UpdateNumberOfInProcess Error: " + ex.Message);
                    //db rollback
                    trans.Rollback();
                }
            }
            return dsReturn;
        }
        #endregion

    }
}
