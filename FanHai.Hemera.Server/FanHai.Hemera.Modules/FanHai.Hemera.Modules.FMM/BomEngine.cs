/*
<FileInfo>
  <Author>Hao.Zhang SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

using SolarViewer.Hemera.Utils.Comm;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.StaticFuncsUtils;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Modules.Databases;

using Microsoft.Practices.EnterpriseLibrary.Data;


namespace SolarViewer.Hemera.Modules.FMM
{
    public class BomEngine : AbstractEngine,IBomEngine
    {
        #region database define
        Database db;
        #endregion

        #region constructor
        public BomEngine()
        {
            //initialize db
            db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        #endregion

        #region Insert Bom
        /// <summary>
        /// Insert Bom
        /// </summary>
        /// <param name="dataSet">dataset for add items cloumns and values</param>
        /// <returns>dataset for excute result</returns>
        public DataSet BomInsert(DataSet dataSet)
        {
            //dataSet.WriteXml(@"d:\BomInsert.xml");
            DataSet retDS = new DataSet();
            try
            {
                if (null != dataSet)
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    if (dataSet.Tables.Contains(WIP_BOM_FIELDS.DATABASE_TABLE_NAME))
                    {
                        string strVersion = "1";
                        DataTable dataTable = dataSet.Tables[WIP_BOM_FIELDS.DATABASE_TABLE_NAME];
                        if (dataTable.Rows.Count == 1)
                        {
                            sqlCommand = @"SELECT MAX(BOM_VERSION)+1 AS BOM_VERSION 
                                               FROM WIP_BOM 
                                             WHERE BOM_NAME ='" + dataTable.Rows[0][WIP_BOM_FIELDS.FIELDS_BOM_NAME].ToString() + "'";

                            IDataReader readerVersion = db.ExecuteReader(CommandType.Text, sqlCommand);
                            if (readerVersion.Read())
                            {
                                if (readerVersion[WIP_BOM_FIELDS.FIELDS_BOM_VERSION].ToString() != "")
                                {
                                    strVersion = readerVersion[WIP_BOM_FIELDS.FIELDS_BOM_VERSION].ToString();
                                }
                            }
                            else
                            {
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Obtain bom version fail!");
                                return retDS;
                            }

                            readerVersion.Close();
                            readerVersion.Dispose();
                        }

                        List<string> sqlCommandBom = new List<string>();
                        DatabaseTable.BuildInsertSqlStatements(ref sqlCommandBom,
                                                            new WIP_BOM_FIELDS(),
                                                            dataTable,
                                                            new Dictionary<string, string>() 
                                                                { 
                                                                    {WIP_BOM_FIELDS.FIELDS_CREATE_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                    {WIP_BOM_FIELDS.FIELDS_CREATE_TIMEZONE, "CN-ZH"},
                                                                    {WIP_BOM_FIELDS.FIELDS_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                    {WIP_BOM_FIELDS.FIELDS_EDIT_TIMEZONE, "CN-ZH"},
                                                                    {WIP_BOM_FIELDS.FIELDS_BOM_VERSION, strVersion}
                                                                },
                                                            new List<string>());

                        if (1 == sqlCommandBom.Count && sqlCommandBom[0].Length > 20)
                        {
                            sqlCommandList.Add(sqlCommandBom[0]);

                            if (dataSet.Tables.Contains(WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME))
                            {
                                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                       new WIP_BOM_COMP_FIELDS(),
                                                                       dataSet.Tables[WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME],
                                                                       new Dictionary<string, string>(),
                                                                       new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });

                                if (dataSet.Tables.Contains(WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME))
                                {
                                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                           new WIP_BOM_PROCESS_FIELDS(),
                                                                           dataSet.Tables[WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME],
                                                                           new Dictionary<string, string>(),
                                                                           new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                                }
                            }

                            if (dataSet.Tables.Contains(WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME))
                            {
                                DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                       new WIP_BOM_ATTACHMENT_FIELDS(),
                                                                       dataSet.Tables[WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME],
                                                                       new Dictionary<string, string>(),
                                                                       new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                            }

                            if (sqlCommandList.Count > 0)
                            {
                                DbConnection dbConn = db.CreateConnection();
                                dbConn.Open();
                                DbTransaction dbTrans = dbConn.BeginTransaction();
                                try
                                {
                                    foreach (string sql in sqlCommandList)
                                    {
                                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                                    }
                                    dbTrans.Commit();
                                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, 0, strVersion);
                                }
                                catch (Exception e)
                                {
                                    dbTrans.Rollback();
                                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, -1, e.Message);
                                }
                                finally
                                {
                                    dbConn.Close();
                                }
                            }
                        }
                        else
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "More than one Bom in input parameter");
                        }
                    }
                    else
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "Can not find Bom parameters");
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "No Bom table in input paremter.");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("BomInsert Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region Update Bom
        /// <summary>
        /// Update Bom
        /// </summary>
        /// <param name="dataSet">dataset for update items cloumns and values</param>
        /// <returns>dataset for excute result</returns>
        public DataSet BomUpdate(DataSet dataSet)
        {
            //dataSet.WriteXml(@"d:\BomUpdate.xml");
            DataSet retDS = new DataSet();
            try
            {
                if (null != dataSet)
                {
                    string sqlCommand = string.Empty;
                    List<string> sqlCommandList = new List<string>();
                    if (dataSet.Tables.Contains(WIP_BOM_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new WIP_BOM_FIELDS(),
                            dataSet.Tables[WIP_BOM_FIELDS.DATABASE_TABLE_NAME],
                            new Dictionary<string, string>() { 
                                                                {WIP_BOM_FIELDS.FIELDS_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                {WIP_BOM_FIELDS.FIELDS_EDIT_TIMEZONE, "CN-ZH"}
                                                             },
                            new List<string>() { WIP_BOM_FIELDS.FIELDS_ROW_KEY },
                            WIP_BOM_FIELDS.FIELDS_ROW_KEY);
                    }

                    if (dataSet.Tables.Contains(WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildSqlStatementsForDML(ref sqlCommandList, new WIP_BOM_COMP_FIELDS(), null,
                            dataSet.Tables[WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME], WIP_BOM_COMP_FIELDS.FIELDS_ROW_KEY);
                    }
                    if (dataSet.Tables.Contains(WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new WIP_BOM_COMP_FIELDS(),
                            dataSet.Tables[WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"],
                            new Dictionary<string, string>(),
                            new List<string>() { WIP_BOM_COMP_FIELDS.FIELDS_ROW_KEY },
                            WIP_BOM_COMP_FIELDS.FIELDS_ROW_KEY);
                    }

                    if (dataSet.Tables.Contains(WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildSqlStatementsForDML(ref sqlCommandList, new WIP_BOM_PROCESS_FIELDS(), null,
                            dataSet.Tables[WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME], WIP_BOM_PROCESS_FIELDS.FIELDS_COMP_ROW_KEY);
                    }
                    if (dataSet.Tables.Contains(WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new WIP_BOM_PROCESS_FIELDS(),
                            dataSet.Tables[WIP_BOM_PROCESS_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"],
                            new Dictionary<string, string>(),
                            new List<string>() { WIP_BOM_PROCESS_FIELDS.FIELDS_ROW_KEY },
                            WIP_BOM_PROCESS_FIELDS.FIELDS_ROW_KEY);
                    }

                    if (dataSet.Tables.Contains(WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DatabaseTable.BuildSqlStatementsForDML(ref sqlCommandList, new WIP_BOM_ATTACHMENT_FIELDS(), null,
                            dataSet.Tables[WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME], WIP_BOM_ATTACHMENT_FIELDS.FIELDS_ROW_KEY);
                    }
                    if (dataSet.Tables.Contains(WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"))
                    {
                        DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new WIP_BOM_ATTACHMENT_FIELDS(),
                            dataSet.Tables[WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME + "_UPDATE"],
                            new Dictionary<string, string>(),
                            new List<string>() { WIP_BOM_ATTACHMENT_FIELDS.FIELDS_ROW_KEY },
                            WIP_BOM_ATTACHMENT_FIELDS.FIELDS_ROW_KEY);
                    }

                    if (sqlCommandList.Count > 0)
                    {
                        DbConnection dbConn = db.CreateConnection();
                        dbConn.Open();
                        DbTransaction dbTrans = dbConn.BeginTransaction();
                        try
                        {
                            foreach (string sql in sqlCommandList)
                            {
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                            }
                            dbTrans.Commit();
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "");
                        }
                        catch (Exception e)
                        {
                            dbTrans.Rollback();
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, e.Message);
                        }
                        finally
                        {
                            dbConn.Close();
                        }
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "No bom update information sent to Server");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("BomUpdate Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region SearchParentOrSapBom
        /// <summary>
        /// SearchParentOrSapBom
        /// </summary>
        /// <param name="dataSet">condition--name</param>
        /// <returns></returns>
        public DataSet SearchParentOrSapBom(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\SearchParentOrSapBom.xml");
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) 
                                    && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    sql = @" SELECT A.* FROM "+WIP_BOM_FIELDS.DATABASE_TABLE_NAME+" A";
                    sql += " WHERE A." + WIP_BOM_FIELDS.FIELDS_BOM_NAME + " LIKE '%" + hashData[WIP_BOM_FIELDS.FIELDS_BOM_NAME] + "%'";
                    sql += "  AND A." + WIP_BOM_FIELDS.FIELDS_PARENT_ROW_KEY + " IS NULL AND A." + WIP_BOM_FIELDS.FIELDS_BOM_STATUS + " <> 2";
                }
                else
                {
                    sql = @" SELECT A.* FROM " + WIP_BOM_FIELDS.DATABASE_TABLE_NAME + " A";
                    sql += " WHERE A." + WIP_BOM_FIELDS.FIELDS_PARENT_ROW_KEY + "='' OR " + WIP_BOM_FIELDS.FIELDS_PARENT_ROW_KEY + " IS NULL";
                    sql += " AND A." + WIP_BOM_FIELDS.FIELDS_BOM_STATUS + " <> 2";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchParentOrSapBom Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region GetBomByKey
        /// <summary>
        ///  GetBomByKey
        /// </summary>
        /// <param name="dataSet">condition</param>
        /// <returns></returns>
        public DataSet GetBomByKey(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\GetBomByKey.xml");
            DataSet dsReturn = new DataSet();
            DataSet dsATTACHReturn = new DataSet();
            DataSet dsComponentReturn = new DataSet();
            DataSet dsSubBomReturn = new DataSet();
            string sql = "";

            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) 
                                    && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                    //GET BOM TABLE'S INFO
                    sql = @" SELECT A.* FROM WIP_BOM A WHERE A.ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "' AND BOM_STATUS<>2";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = WIP_BOM_FIELDS.DATABASE_TABLE_NAME;

                    //GET ATTACHMENT INFO
                    sql = @"SELECT B.* FROM WIP_BOM_ATTACHMENT B WHERE B.BOM_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                    dsATTACHReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsATTACHReturn.Tables[0].TableName = WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME;

                    //GET COMPONENT INFO
                    sql = @"SELECT C.*,D.* FROM WIP_BOM_COMP C LEFT JOIN WIP_BOM_PROCESS D ON C.ROW_KEY=D.COMP_ROW_KEY";
                    sql += " WHERE C.BOM_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                    dsComponentReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsComponentReturn.Tables[0].TableName = WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME;
                  
                    //GET sub BOM TABLE'S INFO
                    sql = @" SELECT A.* FROM WIP_BOM A WHERE A.PARENT_ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                    dsSubBomReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsSubBomReturn.Tables[0].TableName = "SUB_" + WIP_BOM_FIELDS.DATABASE_TABLE_NAME;

                    //MERGE TABLE TO DATASET
                    dsReturn.Merge(dsATTACHReturn.Tables[WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME], true, MissingSchemaAction.Add);
                    dsReturn.Merge(dsComponentReturn.Tables[WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME], true, MissingSchemaAction.Add);
                    dsReturn.Merge(dsSubBomReturn.Tables["SUB_" + WIP_BOM_FIELDS.DATABASE_TABLE_NAME], true, MissingSchemaAction.Add);
                }

                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBomByKey Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region delete sub bom
        /// <summary>
        /// DeleteSubBom
        /// </summary>
        /// <param name="dataSet">bom key</param>
        /// <returns></returns>
        public DataSet DeleteSubBom(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            DbTransaction dbtran = null;
            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    using (DbConnection dbconn = db.CreateConnection())
                    {
                        dbconn.Open();
                        dbtran = dbconn.BeginTransaction();
                        DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                        Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                        //DELETE BOM TABLE'S INFO
                        sql = @" DELETE FROM  WIP_BOM  WHERE ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //DELETE ATTACHMENT INFO
                        sql = @"DELETE  FROM WIP_BOM_ATTACHMENT  WHERE BOM_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //DELETE PROCESS INFO
                        sql = @"DELETE  FROM WIP_BOM_PROCESS WHERE COMP_ROW_KEY IN (SELECT ROW_KEY FROM WIP_BOM_COMP";
                        sql += " WHERE BOM_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "')";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //DELETE COMPONENT INFO AND PROCESS INFO
                        sql = @"DELETE  FROM WIP_BOM_COMP";
                        sql += " WHERE BOM_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                     
                        dbtran.Commit();
                    }
                    
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteSubBom Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region UpdateSubBomStatus
        /// <summary>
        /// UpdateSubBomStatus
        /// </summary>
        /// <param name="dataSet">key and status</param>
        /// <returns></returns>
        public DataSet UpdateSubBomStatus(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\UpdateSubBomStatus.xml");
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                    //update  sub bom status 
                    sql = @" UPDATE WIP_BOM SET BOM_STATUS= '" + hashData[WIP_BOM_FIELDS.FIELDS_BOM_STATUS] + "'";
                    sql += " WHERE ROW_KEY = '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                    db.ExecuteNonQuery(CommandType.Text, sql);
                }
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateSubBomStatus Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region  SearchProduct
        /// <summary>
        /// SearchProduct
        /// </summary>
        /// <param name="dataSet">part name</param>
        /// <returns></returns>
        public DataSet SearchProduct(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\SearchProduct.xml");
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    //GET PRODUCT INFO
                    sql = "SELECT * FROM POR_PART WHERE PART_NAME LIKE '%" + hashData["PART_NAME"] + "%'";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = POR_PART_FIELDS.DATABASE_TABLE_NAME;
                }
                else
                {
                    //GET PRODUCT INFO
                    sql = "SELECT * FROM POR_PART ";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = POR_PART_FIELDS.DATABASE_TABLE_NAME;
                }

                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchProduct Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region GetBom
        /// <summary>
        ///  GetBom
        /// </summary>
        /// <param name="dataSet">bom key</param>
        /// <returns></returns>
        public DataSet GetBom(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\GetBom.xml");
            DataSet dsReturn = new DataSet();
            DataSet dsATTACHReturn = new DataSet();
            DataSet dsComponentReturn = new DataSet();
            DataSet dsSubBomReturn = new DataSet();
            DataSet dsSBCompReturn = new DataSet();
            string sql = "";

            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                    //GET BOM TABLE'S INFO
                    sql = @" SELECT A.* FROM WIP_BOM A WHERE A.ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "' AND BOM_STATUS <> 2";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = WIP_BOM_FIELDS.DATABASE_TABLE_NAME;

                    //GET ATTACHMENT INFO
                    sql = @"SELECT B.* FROM WIP_BOM_ATTACHMENT B WHERE B.BOM_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                    dsATTACHReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsATTACHReturn.Tables[0].TableName = WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME;

                    //GET COMPONENT INFO
                    sql = @"SELECT C.*,D.* FROM WIP_BOM_COMP C LEFT JOIN WIP_BOM_PROCESS D ON C.ROW_KEY=D.COMP_ROW_KEY";
                    sql += " WHERE C.BOM_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                    dsComponentReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsComponentReturn.Tables[0].TableName = WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME;

                    //GET sub BOM TABLE'S INFO
                    sql = @" SELECT A.* FROM WIP_BOM A WHERE A.PARENT_ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "' AND BOM_STATUS <> 2";
                    dsSubBomReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsSubBomReturn.Tables[0].TableName = "SUB_" + WIP_BOM_FIELDS.DATABASE_TABLE_NAME;

                    //get sub bom table's component info
                    sql = @"SELECT SUM(C.PROD_QTY) AS SUM_PROD_QTY,C.PROD_NAME FROM WIP_BOM_COMP C";
                    sql += " WHERE C.BOM_ROW_KEY IN (SELECT ROW_KEY FROM WIP_BOM WHERE PARENT_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "' )";
                    sql += " GROUP BY C.PROD_NAME";
                    dsSBCompReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsSBCompReturn.Tables[0].TableName = "SUB_" + WIP_BOM_FIELDS.DATABASE_TABLE_NAME+"_COMP";
                    //MERGE TABLE TO DATASET
                    dsReturn.Merge(dsATTACHReturn.Tables[WIP_BOM_ATTACHMENT_FIELDS.DATABASE_TABLE_NAME], true, MissingSchemaAction.Add);
                    dsReturn.Merge(dsComponentReturn.Tables[WIP_BOM_COMP_FIELDS.DATABASE_TABLE_NAME], true, MissingSchemaAction.Add);
                    dsReturn.Merge(dsSubBomReturn.Tables["SUB_" + WIP_BOM_FIELDS.DATABASE_TABLE_NAME], true, MissingSchemaAction.Add);
                    dsReturn.Merge(dsSBCompReturn.Tables["SUB_" + WIP_BOM_FIELDS.DATABASE_TABLE_NAME + "_COMP"], true, MissingSchemaAction.Add);
                }

                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBom Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region Delete bom
        /// <summary>
        ///  DeleteBom
        /// </summary>
        /// <param name="dataSet">bom key</param>
        /// <returns></returns>
        public DataSet DeleteBom(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\DeleteBom.xml");
            DataSet dsReturn = new DataSet();
            DataSet dsSubBomReturn = new DataSet();
            string sql = "";
            DbTransaction dbtran = null;
            try
            {
                if (dataset != null && dataset.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) && dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count > 0)
                {
                    using (DbConnection dbconn = db.CreateConnection())
                    {
                        dbconn.Open();
                        dbtran = dbconn.BeginTransaction();
                        DataTable dataTable = dataset.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                        Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                        //DELETE BOM TABLE'S INFO
                        sql = @" SELECT ROW_KEY FROM  WIP_BOM  WHERE PARENT_ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        dsSubBomReturn = db.ExecuteDataSet(CommandType.Text, sql);

                        if (dsSubBomReturn.Tables.Count > 0)
                        {
                            if (dsSubBomReturn.Tables[0].Rows.Count > 0)
                            {
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsSubBomReturn, "父物料清单含有子物料清单，不可以删除！");
                                return dsSubBomReturn;
                            }
                        }


                        //DELETE BOM TABLE'S INFO
                        sql = @" DELETE FROM  WIP_BOM  WHERE ROW_KEY= '" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //DELETE ATTACHMENT INFO
                        sql = @"DELETE  FROM WIP_BOM_ATTACHMENT  WHERE BOM_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //DELETE PROCESS INFO
                        sql = @"DELETE  FROM WIP_BOM_PROCESS WHERE COMP_ROW_KEY IN (SELECT ROW_KEY FROM WIP_BOM_COMP";
                        sql += " WHERE BOM_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "')";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        //DELETE COMPONENT INFO AND PROCESS INFO
                        sql = @"DELETE  FROM WIP_BOM_COMP";
                        sql += " WHERE BOM_ROW_KEY='" + hashData[WIP_BOM_FIELDS.FIELDS_ROW_KEY] + "'";
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                        dbtran.Commit();
                    }

                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteBom Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion
    }
}
