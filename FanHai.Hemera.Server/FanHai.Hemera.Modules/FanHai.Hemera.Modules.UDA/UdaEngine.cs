/*
<FileInfo>
  <Author>Hao.Zhang, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.Comm;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Modules.Databases;


namespace SolarViewer.Hemera.Modules.UDA
{
    public class UdaEngine : AbstractEngine, IUdaEngine
    {
        #region variable define
        //DBEngine object
        private DBEngine dbEngine;
        #endregion

        #region constructor
        /// <summary>
        /// No parameter constructor
        /// </summary>
        public UdaEngine()
        {
            dbEngine = new DBEngine();
        }
        #endregion

        #region initialize

        /// <summary>
        /// Initialize method
        /// </summary>
        public override void Initialize() { }
        #endregion

   
        /// <summary>
        /// Add Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for add items cloumns and values</param>
        /// <returns>dataset</returns>
        public DataSet AddUdaSalesOrderAttr(DataSet dataset)
        {

            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sql 
            string sql = "";
            try
            {
                if (dataset.Tables.Count > 0)
                {
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        sql += "INSERT INTO POR_SALES_ORDER_ATTR(";
                        //repeated check
                        for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                        {
                            if (i == 0)
                                sql += dataset.Tables[0].Rows[i][0].ToString();
                            else
                                sql += ", " + dataset.Tables[0].Rows[i][0].ToString();
                        }
                        sql += ") VALUES (";
                        for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                        {
                            if (i == 0)
                                sql += dataset.Tables[0].Rows[i][1];
                            else
                                sql += ", " + dataset.Tables[0].Rows[i][1];
                        }
                        sql += " )";

                        //excute insert
                        dbEngine.ExecuteNonQuery(sql);
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                    }
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("AddUdaSalesOrderAttr Error: " + ex.Message);
            }

            return dataDs;
        }
        /// <summary>
        /// Delete Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for delete condition</param>
        /// <returns>dataset</returns>
        public DataSet DeleteUdaSalesOrderAttr(DataSet dataset)
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();

            //define sql 
            string sql = "";

            try
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    sql = "DELETE FROM POR_SALES_ORDER_ATTR WHERE ";

                    sql += " SALES_ORDER_KEY = " + dataset.Tables[0].Rows[0]["ATTRIBUTE_VALUE"];

                    //excute insert
                    dbEngine.ExecuteNonQuery(sql);

                    //add paramter table
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

                }
            }
            catch (Exception ex)
            {
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("DeleteUdaSalesOrderAttr Error: " + ex.Message);
            }

            return dataDs;
        }

        /// <summary>
        /// Get Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for select condition</param>
        /// <returns>dataset</returns>
        public DataSet GetUdaSalesOrderAttr(DataSet dataset)
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();

            //define sql 
            string sql = "";

            try
            {
                if (dataset!=null && (dataset.Tables[0].Rows.Count > 0))
                {
                    sql = "SELECT * FROM POR_SALES_ORDER_ATTR WHERE 1=1 ";
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        sql += " AND " + dataset.Tables[0].Rows[i][0].ToString();
                        sql += " =" + dataset.Tables[0].Rows[i][1];
                    }
                    sql += " ORDER BY SALES_ORDER_KEY";
                }
                else
                {
                    sql = "SELECT * FROM POR_SALES_ORDER_ATTR ORDER BY SALES_ORDER_KEY";
                }

                DataTable dataTable = new DataTable();
                dataTable = dbEngine.ExecuteDataSet(sql).Tables[0];
                dataTable.TableName = "POR_SALES_ORDER_ATTR";
                dataDs.Merge(dataTable, false, MissingSchemaAction.Add);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("GetUdaSalesOrderAttr Error: " + ex.Message);
            }

            return dataDs;
        }

        /// <summary>
        /// Update Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for update condition</param>
        /// <returns>dataset</returns>
        public DataSet UpdateUdaSalesOrderAttr(DataSet dataset)
        {
            //define dataset to receive db data
            DataSet dataDs = new DataSet();

            //define sql 
            string sql = "";

            try
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    sql = "UPDATE POR_SALES_ORDER_ATTR SET ";
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        sql += dataset.Tables[0].Rows[i][0].ToString();
                        sql += " =" + dataset.Tables[0].Rows[i][1] + ",";
                    }
                    //get correct sql
                    sql = sql.Substring(0, sql.Length - 1);

                    sql += " WHERE SALES_ORDER_KEY = " + dataset.Tables[0].Rows[0]["ATTRIBUTE_VALUE"];

                    //excute insert
                    dbEngine.ExecuteNonQuery(sql);

                    //add paramter table
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");

                }
            }
            catch (Exception ex)
            {
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("UpdateUdaSalesOrderAttr Error: " + ex.Message);
            }

            return dataDs;
        }
    }
}
