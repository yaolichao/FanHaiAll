/*
<FileInfo>
  <Author>Rayna.Liu, SolarViewer Hemera</Author>
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
using System.Linq;
using System.Text;

using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.Comm;
using SolarViewer.Hemera.Utils.StaticFuncsUtils;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SolarViewer.Hemera.Utils.DatabaseHelper;

namespace SolarViewer.Hemera.Modules.Wip
{
    public class LotTemplateEngine:AbstractEngine,ILotTemplate
    {
        private static Database db;

        #region constructor
        public LotTemplateEngine()
        {
            //initialize CreateDatabase
            db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }
        #endregion

        /// <summary>
        /// Add Lot Template
        /// </summary>
        /// <param name="dataset">New Lot Template</param>
        /// <returns>dataset</returns>
       public DataSet AddLotTemplate(DataSet dataset)
        {
            System.DateTime startTime = System.DateTime.Now;

            DataSet dsReturn = new DataSet();
            String[] sql = new String[3];

            List<string> sqlCommandList = new List<string>();
            try
            {
                if (dataset.Tables.Contains("param"))
                {
                    DataTable dataTable = dataset.Tables["param"];
                    Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    //add by zhanghao for uda
                    POR_LOT_TEMPLATE_ATTR_FIELDS lotTempUda = new POR_LOT_TEMPLATE_ATTR_FIELDS();
                    //end
                    sql[0] = "SELECT * FROM POR_LOT_TEMPLATE WHERE TEMPLATE_NAME ='" + hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_NAME] + "'";
                    IDataReader read = db.ExecuteReader(CommandType.Text, sql[0]);

                    if (read.Read())
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.Wip.LotTemplateEngine.TemplateNameAlreadyExist}");
                        return dsReturn;
                    }
                    else
                    {
                        using (DbConnection dbconn = db.CreateConnection())
                        {
                            //Open Connection
                            dbconn.Open();
                            DbTransaction dbtran = dbconn.BeginTransaction();
                            //Create Transaction                            
                            try
                            {
                                int version = 1;
                                sql[1] = @"INSERT INTO POR_LOT_TEMPLATE(TEMPLATE_KEY,
                                                                        TEMPLATE_NAME,
                                                                        TEMPLATE_STATUS,
                                                                        TEMPLATE_DESCRIPTIONS,
                                                                        TEMPLATE_VERSION,
                                                                        CREATOR,
                                                                        CREATE_TIME,
                                                                        CREATE_TIMEZONE_KEY) 
                                                                        VALUES('"+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY]+"','"+
                                                                         hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_NAME]+"','"+
                                                                         hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_STATUS]+"','"+
                                                                         hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_DESCRIPTIONS]+"',"+
                                                                         version+",'"+
                                                                         hashData[COMMON_FIELDS.FIELD_COMMON_CREATOR]+"',"+
                                                                         "to_date('" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-DD hh24:mi:ss'),'" + 
                                                                         hashData[COMMON_FIELDS.FIELD_COMMON_CREATE_TIMEZONE_KEY]+"')";
                                db.ExecuteNonQuery(dbtran,CommandType.Text,sql[1]);

                                //add by zhanghao for uda
                                if (dataset.Tables.Contains(POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME))
                                {
                                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                           new POR_LOT_TEMPLATE_ATTR_FIELDS(),
                                                                           dataset.Tables[POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                           new Dictionary<string, string>() 
                                                                   {  
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                   },
                                                                           new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                                }
                                foreach (string sqlU in sqlCommandList)
                                {
                                    db.ExecuteNonQuery(dbtran, CommandType.Text, sqlU);
                                }

                                //end
                                dbtran.Commit();
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
                            }
                            catch (Exception ex)
                            {
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                                //Rollback Transaction
                                dbtran.Rollback();
                                LogService.LogError("AddLotTemplate Error: " + ex.Message);
                            }
                            finally
                            {
                                //Close Connection
                                dbconn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddLotTemplate Error: " + ex.Message);
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("AddLotTemplate Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }

        /// <summary>
        /// Get Lot Template
        /// </summary>
        /// <param name="dataset">key word of Lot Template</param>
        /// <returns>dataset</returns>
       public DataSet GetLotTemplate(DataSet dataset)
       {
           System.DateTime startTime = System.DateTime.Now;

           DataSet dsReturn = new DataSet();
           
           try
           {
               if (dataset.Tables.Contains("param"))
               {
                   DataTable dataTable = dataset.Tables["param"];
                   Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                   DataTable udaDataTable = new DataTable();
                   string sql="SELECT * FROM POR_LOT_TEMPLATE WHERE TEMPLATE_KEY='"+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY]+"'";
                   dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                   //add by zhanghao for uda
                   sql = @"SELECT A.LOT_TEMPLATE_KEY ," +
                          " A.ATTRIBUTE_KEY,A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE,A.EDIT_TIME ," +
                          " B.DATA_TYPE AS DATA_TYPE,A.EDITOR FROM POR_LOT_TEMPLATE_ATTR A "+
                          " LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY=B.ATTRIBUTE_KEY WHERE A.LOT_TEMPLATE_KEY = '" + hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY] + "' ORDER BY 1";
                   udaDataTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                   udaDataTable.TableName = POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME;
                   //ADD UDA TABLE TO DATASET
                   dsReturn.Merge(udaDataTable, true, MissingSchemaAction.Add);
                   //add by zhanghao for uda end
                   SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
               }
           }
           catch (Exception ex)
           {
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               LogService.LogError("GetLotTemplate Error: " + ex.Message);
           }

           System.DateTime endTime = System.DateTime.Now;
           LogService.LogInfo("GetLotTemplate Time: " + (endTime - startTime).TotalMilliseconds.ToString());

           return dsReturn;
       }

        /// <summary>
        /// Save a new version of Lot Template
        /// </summary>
        /// <param name="dataset">information of Lot Template</param>
        /// <returns>dataset</returns>
       public DataSet SaveAsLotTemplate(DataSet dataset)
       {
           System.DateTime startTime = System.DateTime.Now;

           DataSet dsReturn = new DataSet();
           String[] sql = new String[3];

           List<string> sqlCommandList = new List<string>();
           try
           {
               if (dataset.Tables.Contains("param"))
               {
                   DataTable dataTable = dataset.Tables["param"];
                   Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);  
                   //add by zhanghao for uda
                   POR_LOT_TEMPLATE_ATTR_FIELDS lotTempUda = new POR_LOT_TEMPLATE_ATTR_FIELDS();
                   //end
                   using (DbConnection dbconn = db.CreateConnection())
                     {
                           //Open Connection
                         dbconn.Open();
                         DbTransaction dbtran = dbconn.BeginTransaction();
                           //Create Transaction                            
                         try
                         { 
                             int version = 1;
                             int nextVersion = -1;
                             sql[0] = @"SELECT MAX(TEMPLATE_VERSION) AS TEMPLATE_VERSION FROM POR_LOT_TEMPLATE WHERE TEMPLATE_NAME='"+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_NAME]+"'";
                             IDataReader readerVersion = db.ExecuteReader(CommandType.Text, sql[0]);
                             if (readerVersion.Read())
                             {
                                 if (readerVersion["TEMPLATE_VERSION"].ToString() != "")
                                 {
                                     version = Int32.Parse(readerVersion["TEMPLATE_VERSION"].ToString());
                                     nextVersion = version + 1;
                                 }
                             }
                             else
                             {
                                 SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.Wip.LotTemplateEngine.GetTemplateVersionError}");
                                 return dsReturn;
                             }                            
                             sql[1] = @"INSERT INTO POR_LOT_TEMPLATE(TEMPLATE_KEY,
                                                                     TEMPLATE_NAME,
                                                                     TEMPLATE_STATUS,
                                                                     TEMPLATE_DESCRIPTIONS,
                                                                     TEMPLATE_VERSION,
                                                                     TEMPLATE_FROM_VERSION,
                                                                     CREATOR,
                                                                     CREATE_TIME,
                                                                     CREATE_TIMEZONE_KEY) 
                                                                     VALUES('" + hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY] + "','" +
                                                                     hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_NAME] + "','" +
                                                                     hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_STATUS] + "','" +
                                                                     hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_DESCRIPTIONS] + "'," +
                                                                     nextVersion+ "," +
                                                                     version+",'"+
                                                                     hashData[COMMON_FIELDS.FIELD_COMMON_CREATOR] + "'," +
                                                                     "to_date('" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-DD hh24:mi:ss'),'" +                                                                     
                                                                     hashData[COMMON_FIELDS.FIELD_COMMON_CREATE_TIMEZONE_KEY] + "')";
                             db.ExecuteNonQuery(dbtran, CommandType.Text, sql[1]);

                             //add by zhanghao for uda
                             if (dataset.Tables.Contains(POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME))
                             {
                                 DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                                        new POR_LOT_TEMPLATE_ATTR_FIELDS(),
                                                                        dataset.Tables[POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                                        new Dictionary<string, string>() 
                                                                        {  
                                                                           {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                                           {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}
                                                                        },
                                                                        new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });
                             }
                             foreach (string sqlU in sqlCommandList)
                             {
                                 db.ExecuteNonQuery(dbtran, CommandType.Text, sqlU);
                             }
                             //end


                             dbtran.Commit();
                             SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                         }
                        catch (Exception ex)
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            LogService.LogError("SaveAsLotTemplate Error: " + ex.Message);
                            //Rollback Transaction
                            dbtran.Rollback();
                        }
                         finally
                         {
                             //Close Connection
                             dbconn.Close();
                         }
                   }
               }
           }
           catch (Exception ex)
           {
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               LogService.LogError("SaveAsLotTemplate Error: " + ex.Message);
           }

           System.DateTime endTime = System.DateTime.Now;
           LogService.LogInfo("SaveAsLotTemplate Time: " + (endTime - startTime).TotalMilliseconds.ToString());

           return dsReturn;
       }

        /// <summary>
        /// Update Lot Template
        /// </summary>
        /// <param name="dataset">information of Lot Template</param>
        /// <returns>dataset</returns>
       public DataSet UpdateLotTemplate(DataSet dataset)
       {
           System.DateTime startTime = System.DateTime.Now;

           DataSet dsReturn = new DataSet();
           String[] sql = new String[3];
           List<string> sqlCommandList = new List<string>();

           try
           {
               if (dataset.Tables.Contains("param"))
               {
                   DataTable dataTable = dataset.Tables["param"];
                   Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                   //ADD BY ZHANGHAO FOR UDA
                   POR_LOT_TEMPLATE_ATTR_FIELDS lotTempUda = new POR_LOT_TEMPLATE_ATTR_FIELDS();
                   //END
                   using (DbConnection dbconn = db.CreateConnection())
                   {
                       //Open Connection
                       dbconn.Open();
                       //Create Transaction
                       DbTransaction dbtran = dbconn.BeginTransaction();
                       try
                       {
                           sql[0] = @"UPDATE POR_LOT_TEMPLATE SET
                                      TEMPLATE_STATUS="+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_STATUS]+","+
                                      "TEMPLATE_DESCRIPTIONS='"+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_DESCRIPTIONS]+"',"+
                                      "EDITOR='"+hashData[COMMON_FIELDS.FIELD_COMMON_EDITOR]+"',"+
                                      "EDIT_TIME=to_date('" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-DD hh24:mi:ss')," +
                                      "EDIT_TIMEZONE_KEY='"+hashData[COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE]+"' "+
                                      "WHERE TEMPLATE_KEY='"+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY]+"'";
                           db.ExecuteNonQuery(dbtran,CommandType.Text,sql[0]);

                           //add by zhanghao for uda
                           //add uda
                           if (dataset.Tables.Contains(POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME))
                           {
                               DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList, new POR_LOT_TEMPLATE_ATTR_FIELDS(), dataset.Tables[POR_LOT_TEMPLATE_ATTR_FIELDS.DATABASE_TABLE_NAME], POR_LOT_TEMPLATE_ATTR_FIELDS.FIELD_LOT_TEMPLATE_KEY);
                           }
                           foreach (string sqlU in sqlCommandList)
                           {
                               db.ExecuteNonQuery(dbtran, CommandType.Text, sqlU);
                           }


                           dbtran.Commit();
                           SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
                       }
                       catch (Exception ex)
                       {
                           dbtran.Rollback();
                           SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
                           LogService.LogError("UpdateLotTemplate Error: " + ex.Message);
                       }
                       finally
                       {
                           //Close Connection
                           dbconn.Close();
                       }

                   }
               }
           }
           catch (Exception ex)
           {
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               LogService.LogError("UpdateLotTemplate Error: " + ex.Message);
           }

           System.DateTime endTime = System.DateTime.Now;
           LogService.LogInfo("UpdateLotTemplate Time: " + (endTime - startTime).TotalMilliseconds.ToString());

           return dsReturn;
       }

        /// <summary>
        /// Search Lot Template
        /// </summary>
        /// <param name="dataset">key word of Lot Template</param>
        /// <returns>dateSet</returns>
       public DataSet SearchLotTemplate(DataSet dataset)
       {
           DataSet dsReturn = new DataSet();
           string sql = "";
           try
           {
               if (dataset != null && dataset.Tables.Contains("param"))
               {
                   DataTable dataTable = dataset.Tables["param"];
                   Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                   sql = @"SELECT * FROM POR_LOT_TEMPLATE WHERE TEMPLATE_NAME LIKE '%" + hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_NAME] + "%'";                                      
               }
               else
               {
                   sql = @"SELECT * FROM POR_LOT_TEMPLATE";
               }
               dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
           }
           catch (Exception ex)
           {
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               LogService.LogError("SearchLotTemplate Error: " + ex.Message);
           }
           return dsReturn;
       }

        /// <summary>
        /// Delete Lot Template
        /// </summary>
        /// <param name="dataset">key word of Lot Template</param>
        /// <returns>dataset</returns>
       public DataSet DeleteLotTemplate(DataSet dataset)
       {
           DataSet dsReturn = new DataSet();
           String[] sql = new String[3];
           try
           {
               if (dataset.Tables.Contains("param"))
               {
                   DataTable dataTable = dataset.Tables["param"];
                   Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);

                   using (DbConnection dbconn = db.CreateConnection())
                   {
                       //Open Connection
                       dbconn.Open();
                       //Create Transaction
                       DbTransaction dbtran = dbconn.BeginTransaction();
                       try
                       {
                           sql[0] = @"DELETE FROM POR_LOT_TEMPLATE WHERE TEMPLATE_KEY='"+hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY]+"'";
                           db.ExecuteNonQuery(dbtran,CommandType.Text,sql[0]);
                           sql[1] = @"DELETE FROM POR_LOT_TEMPLATE_ATTR WHERE LOT_TEMPLATE_KEY='" + hashData[POR_LOT_TEMPLATE_FIELDS.FIELD_TEMPLATE_KEY] + "'";
                           db.ExecuteNonQuery(dbtran, CommandType.Text, sql[1]);
                           dbtran.Commit();
                           SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
                       }
                       catch (Exception ex)
                       {
                           SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
                           LogService.LogError("DeleteLotTemplate Error: " + ex.Message);
                           dbtran.Rollback();
                       }
                       finally
                       {
                           //Close Connection
                           dbconn.Close();
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
               LogService.LogError("DeleteLotTemplate Error: " + ex.Message);
           }
           return dsReturn;
       }


       //add by zhanghao 20100525 begin
       /// <summary>
       /// GetAttributsColumnsForLotTemplate
       /// </summary>
       /// <param name="dataset">dataset for Operation key</param>
       /// <returns>DataSet</returns>
       public DataSet GetAttributesColumnsForLotTemplate()
       {
           //define dataset to receive db data
           DataSet dataDs = new DataSet();
           //define sql 
           string sql = "";

           try
           {
               sql = " SELECT BASE_ATTRIBUTE.ATTRIBUTE_KEY, BASE_ATTRIBUTE.ATTRIBUTE_NAME,BASE_ATTRIBUTE.DESCRIPTION,BASE_ATTRIBUTE.DATA_TYPE,'' AS DATA_TYPESTRING";
               sql += " FROM BASE_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY";
               sql += " WHERE BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY =BASE_ATTRIBUTE.CATEGORY_KEY AND BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME='Uda_lot_template'";
               //excute sql
               dataDs = db.ExecuteDataSet( CommandType.Text,sql);
               //add paramter table
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

           }
           catch (Exception ex)
           {
               //add paramter table
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
               LogService.LogError("GetAttributesColumnsForLotTemplate Error: " + ex.Message);
           }

           return dataDs;
       }

       /// <summary>
       /// GetAttributsForOperation
       /// </summary>
       /// <param name="dataset">dataset for Operation key</param>
       /// <returns>DataSet</returns>
       public DataSet GetAttributesForLotTemplate(DataSet dataset)
       {
           //define dataset to receive db data
           DataSet dataDs = new DataSet();
           //define sql 
           string sql = "";

           try
           {
               if (dataset != null && (dataset.Tables[0].Rows.Count > 0))
               {
                   sql = " SELECT POR_LOT_TEMPLATE_ATTR.LOT_TEMPLATE_KEY AS ORDERKEY,                ";
                   sql += "POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_KEY AS ATTRIBUTE_KEY,                    ";
                   sql += "POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_NAME AS ATTRIBUTE_NAME,                  ";
                   sql += "POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_VALUE AS ATTRIBUTE_VALUE,                ";
                   sql += "'' AS LAST_UPDATETIME,                           ";
                   sql += "BASE_ATTRIBUTE.DATA_TYPE AS DATA_TYPE,                                  ";
                   sql += "'' AS FLAG                                                              ";
                   sql += "FROM POR_LOT_TEMPLATE_ATTR,BASE_ATTRIBUTE                ";
                   sql += "WHERE BASE_ATTRIBUTE.ATTRIBUTE_KEY = POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_KEY ";

                   for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                   {
                       sql += " AND " + dataset.Tables[0].Rows[i][0].ToString();
                       sql += " =" + dataset.Tables[0].Rows[i][1];
                   }
               }
               else
               {
                   sql = " SELECT POR_LOT_TEMPLATE_ATTR.LOT_TEMPLATE_KEY AS ORDERKEY,                ";
                   sql += "POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_KEY AS ATTRIBUTE_KEY,                    ";
                   sql += "POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_NAME AS ATTRIBUTE_NAME,                  ";
                   sql += "POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_VALUE AS ATTRIBUTE_VALUE,                ";
                   sql += "'' AS LAST_UPDATETIME,                           ";
                   sql += "BASE_ATTRIBUTE.DATA_TYPE AS DATA_TYPE,                                  ";
                   sql += "'' AS FLAG                                                              ";
                   sql += "FROM POR_LOT_TEMPLATE_ATTR,BASE_ATTRIBUTE                ";
                   sql += "WHERE BASE_ATTRIBUTE.ATTRIBUTE_KEY = POR_LOT_TEMPLATE_ATTR.ATTRIBUTE_KEY ";

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
               LogService.LogError("GetAttributesForLotTemplate Error: " + ex.Message);
           }

           return dataDs;
       }


       #region add by zhanghao 20100603 for GetHelpInfoForLotTemplateHelpForm
       /// <summary>
       /// GetHelpInfoForLineHelpForm
       /// </summary>
       /// <param name="dataset">line_code</param>
       /// <returns></returns>
       public DataSet GetHelpInfoForLotTemplateHelpForm(DataSet dataset)
       {
           //define return dataset
           DataSet dataSet = new DataSet();
           dataSet = null;
           try
           {
               //create db
               db = DatabaseFactory.CreateDatabase();
               string sql = " SELECT A.TEMPLATE_KEY AS TEMPLATE_KEY, B.TEMPLATE_NAME AS TEMPLATE_NAME , B.TEMPLATE_VERSION AS TEMPLATE_VERSION ";
               sql += " FROM POR_LOT_TEMPLATE A RIGHT JOIN";
               sql += " (SELECT TEMPLATE_NAME, MAX(TEMPLATE_VERSION) AS TEMPLATE_VERSION FROM POR_LOT_TEMPLATE GROUP BY TEMPLATE_NAME ) B";
               sql += " ON A.TEMPLATE_VERSION = B.TEMPLATE_VERSION";
               sql += " AND A.TEMPLATE_NAME =B.TEMPLATE_NAME";
               sql += " WHERE B.TEMPLATE_NAME LIKE '%" + dataset.Tables[0].Rows[0][1].ToString() + "%' ";
               sql += " AND ROWNUM <=" + dataset.Tables[0].Rows[1][1].ToString();
               sql += " ORDER BY TEMPLATE_KEY";
               dataSet = db.ExecuteDataSet(CommandType.Text, sql);
               //add parameter table
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataSet, "");
           }
           catch (Exception ex)
           {
               //add parameter table
               SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataSet, ex.Message);
               LogService.LogError("GetHelpInfoForLotTemplateHelpForm Error: " + ex.Message);
           }
           return dataSet;
       }
       #endregion

       #region GetLotDataType
       /// <summary>
       /// GetLotDataType
       /// </summary>
       /// <returns>DataSet</returns>
       public DataSet GetLotTemplateDataType()
       {
           //define dataset to return value
           DataSet dataDs = new DataSet();
           //define sqlCommand 
           String sql = "";
           using (DbConnection dbconn = db.CreateConnection())
           {
               try
               {
                   sql = @"select BASE_ATTRIBUTE.ATTRIBUTE_NAME,BASE_ATTRIBUTE.DATA_TYPE
                            from BASE_ATTRIBUTE,BASE_ATTRIBUTE_CATEGORY
                            where ( BASE_ATTRIBUTE.CATEGORY_KEY=BASE_ATTRIBUTE_CATEGORY.CATEGORY_KEY )
                            and BASE_ATTRIBUTE_CATEGORY.CATEGORY_NAME = 'Uda_lot_template'";
                   dataDs = db.ExecuteDataSet(CommandType.Text, sql);
                   SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
               }
               catch (Exception ex)
               {
                   SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                   LogService.LogError("GetLotTemplateDataType Error: " + ex.Message);
               }
               finally
               {
                   dbconn.Close();
               }
           }
           return dataDs;
       }
       #endregion
    }
}
