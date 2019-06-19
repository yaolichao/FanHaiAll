/*
<FileInfo>
  <Author>ZhangHao FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.Common;
using System.Collections.Generic;

using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;

using Microsoft.Practices.EnterpriseLibrary.Data;
using SAP.Middleware.Connector;
using System.Configuration;
using Domino;
using System.Runtime.InteropServices;


namespace FanHai.Hemera.Utils.StaticFuncs
{
    public static class AllCommonFunctions
    {
        static RfcConfigParameters rfcConfigParams = new RfcConfigParameters();

        static string lotusServer = string.Empty;
        static string lotusFile = string.Empty;
        static string lotusPassword = string.Empty;

        const string LOTUS_SENDTO = "SendTo";
        const string LOTUS_COPYTO = "CopyTo";
        const string LOTUS_SUBJECT = "Subject";
        const string LOTUS_BODY = "Body";

        static AllCommonFunctions()
        {
            #region Initial SAP Remote Function Call Config Parameters Data

            rfcConfigParams.Add(RfcConfigParameters.Name, ConfigurationManager.AppSettings["Name"]);
            rfcConfigParams.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["AppServerHost"]);
            rfcConfigParams.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SystemNumber"]);
            rfcConfigParams.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["Client"]);
            rfcConfigParams.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["User"]);
            rfcConfigParams.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["Password"]);
            rfcConfigParams.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["Language"]);

            #endregion

            #region Initial Lotus Send E-Mail Configuration Data

            lotusServer = ConfigurationManager.AppSettings["LotusServer"];
            lotusFile = ConfigurationManager.AppSettings["LotusFile"];
            lotusPassword = ConfigurationManager.AppSettings["LotusPassword"];

            #endregion
        }

        #region variable define

        //define dataset
        static DataSet dataSet = new DataSet();
        //define datatable
        static DataTable dataTable = new DataTable();
        //define datacolumn
        static DataColumn dataColumnKey = new DataColumn("ATTRIBUTE_KEY");
        //define datacolumn
        static DataColumn dataColumnValue = new DataColumn("ATTRIBUTE_VALUE");

        #endregion
        /// <summary>
        /// get max id+1 from table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns>id+1</returns>
        public static string GetMaxIdFromTable(string tableName, string columnName)
        {
            try
            {
                DataSet ds = new DataSet();  //dataSet to accept the data sended from dbs
                string sql = "select case when max(" + columnName + ") is null then 0  when max(" + columnName + ") is not null then max(" + columnName + ")+1 end from " + tableName;
                //create db engine
                Database db = DatabaseFactory.CreateDatabase();
                ds = db.ExecuteDataSet(sql);
                //return dataset
                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Get Dynamic constructor DataSet
        /// </summary>
        /// <returns>dataset</returns>
        public static DataSet GetDynamicTwoColumnsDataSet()
        {
            dataTable.Columns.Clear();
            dataSet.Tables.Clear();
            //add datacolumn to datatable
            dataTable.Columns.Add(dataColumnKey);
            dataTable.Columns.Add(dataColumnValue);
            //add datatable to dataset
            dataSet.Tables.Add(dataTable);
            //return dataset
            return dataSet;
        }
        
        /// <summary>
        /// Get Specify Table Column Data
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="field"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-08 11:14:01
        public static string GetSpecifyTableColumnData(TableFields tableFields, string field, string key, object value, DbTransaction transaction)
        {
            List<string> interestColumns = new List<string>() { field };

            Conditions conditions = new Conditions();

            if (value == null || value == DBNull.Value)
            {
                conditions.Add(DatabaseLogicOperator.And, key, DatabaseCompareOperator.Null, string.Empty);
            }
            else
            {
                conditions.Add(DatabaseLogicOperator.And, key, DatabaseCompareOperator.Equal, value.ToString());
            }

            string sqlString = DatabaseTable.BuildQuerySqlStatement(tableFields, interestColumns, conditions);

            if (transaction != null && transaction.Connection != null)
            {
                DbCommand command = transaction.Connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = sqlString;
                command.Transaction = transaction;

                object scalar = command.ExecuteScalar();

                if (scalar != null && scalar != DBNull.Value)
                {
                    return scalar.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Specify Table Column Data
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="field"></param>
        /// <param name="conditions"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-13 12:12:27
        public static string GetSpecifyTableColumnData(TableFields tableFields, string field, Conditions conditions, DbTransaction transaction)
        {
            List<string> interestColumns = new List<string>() { field };

            string sqlString = DatabaseTable.BuildQuerySqlStatement(tableFields, interestColumns, conditions);

            if (transaction != null && transaction.Connection != null)
            {
                DbCommand command = transaction.Connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = sqlString;
                command.Transaction = transaction;

                object scalar = command.ExecuteScalar();

                if (scalar != null && scalar != DBNull.Value)
                {
                    return scalar.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 通用的分页查询方法。
        /// </summary>
        /// <param name="sqlString">分页查询使用的SQL语句。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="records">总记录数。</param>
        /// <param name="db">数据库对象。</param>
        /// <param name="ds">数据集对象。</param>
        /// <param name="tableName">数据表名称。</param>
        public static void CommonPagingData(string sqlString, int pageNo, int pageSize, out int pages, out int records,
                                            Database db, DataSet ds, string tableName, params string[] orderByFields)
        {
            pages = GetSqlCommandPages(sqlString, ref pageNo, pageSize, out records, db);
            string orderby =string.Empty;
            if (orderByFields != null && orderByFields.Length > 0)
            {
                foreach (string filed in orderByFields)
                {
                    orderby += string.Format("T1.{0},", filed);
                }
                orderby = orderby.TrimEnd(',');
            }
            else{
                orderby = "GETDATE()";
            }
            int maxNum=(pageNo-1)*pageSize+pageSize;
            int minNum=(pageNo-1)*pageSize+1;
            string pagingSqlString = string.Format(@"WITH a AS
                                                    (
                                                        SELECT TOP {1} ROW_NUMBER() OVER(ORDER BY {3}) RN,* 
                                                        FROM ({0}) T1
                                                    )
                                                    SELECT * FROM a WHERE RN >= {2} ORDER BY RN",
                                                    sqlString,
                                                    maxNum,
                                                    minNum,
                                                    orderby);
            db.LoadDataSet(CommandType.Text, pagingSqlString, ds, new string[] { tableName });
        }

        /// <summary>
        /// 通用的分页查询方法。
        /// </summary>
        /// <param name="sqlString">分页查询使用的SQL语句。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="records">总记录数。</param>
        /// <param name="db">数据库对象。</param>
        /// <param name="ds">数据集对象。</param>
        /// <param name="tableName">数据表名称。</param>
        /// <param name="sort">排序规则。</param>
        public static void CommonPagingData(string sqlString, int pageNo, int pageSize, out int pages, out int records,
                                         Database db, DataSet ds, string tableName, string sort, params string[] orderByFields)
        {
            pages = GetSqlCommandPages(sqlString, ref pageNo, pageSize, out records, db);
            string orderby = string.Empty;
            if (orderByFields != null && orderByFields.Length > 0)
            {
                foreach (string filed in orderByFields)
                {
                    orderby += string.Format("T1.{0} {1},", filed, sort);
                }
                orderby = orderby.TrimEnd(',');
            }
            else
            {
                orderby = "GETDATE()";
            }
            int maxNum = (pageNo - 1) * pageSize + pageSize;
            int minNum = (pageNo - 1) * pageSize + 1;
            string pagingSqlString = string.Format(@"WITH a AS
                                                    (
                                                        SELECT TOP {1} ROW_NUMBER() OVER(ORDER BY {3}) RN,* 
                                                        FROM ({0}) T1
                                                    )
                                                    SELECT * FROM a WHERE RN >= {2} ORDER BY RN",
                                                    sqlString,
                                                    maxNum,
                                                    minNum,
                                                    orderby);
            db.LoadDataSet(CommandType.Text, pagingSqlString, ds, new string[] { tableName });
        }
        /// <summary>
        /// Get Sql Command Records
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-08-04 12:24:18
        public static int GetSqlCommandRecords(string sqlString, Database db)
        {
            if (db != null)
            {
                object scalar = db.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(*) AS RECORDS FROM ({0}) T", sqlString));

                if (scalar != null && scalar != DBNull.Value)
                {
                    return Convert.ToInt32(scalar);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get Sql Command Pages
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="records"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-08-04 12:32:42
        public static int GetSqlCommandPages(string sqlString, ref int pageNo, int pageSize, out int records, Database db)
        {
            int pages = 0;

            records = GetSqlCommandRecords(sqlString, db);

            if (records > 0 && pageSize > 0)
            {
                if (records % pageSize == 0)
                {
                    pages = records / pageSize;
                }
                else
                {
                    pages = records / pageSize + 1;
                }
            }

            if (pageNo <= 0 || pageNo > pages)
            {
                pageNo = pages;
            }

            return pages;
        }

        /// <summary>
        /// Process SAP RFC Structure Input Data
        /// </summary>
        /// <param name="rfcFunc"></param>
        /// <param name="paramIndex"></param>
        /// <param name="structData"></param>
        /// Owner:Andy Gao 2011-05-11 13:16:41
        private static void ProcessSAPRFCStructureInputData(IRfcFunction rfcFunc, int paramIndex, DataTable structData)
        {
            IRfcStructure rfcStruct = rfcFunc.GetStructure(paramIndex);
 
            DataRow structRow = structData.Rows[0]; //Default First Row is Structure Data

            foreach (DataColumn column in structData.Columns)
            {
                int structIndex = rfcStruct.Metadata.TryNameToIndex(column.ColumnName);

                if (structIndex >= 0)
                {
                    object value = structRow[column];

                    if (value != null && value != DBNull.Value)
                    {
                        rfcStruct.SetValue(structIndex, value);
                    }
                }
            }

            rfcFunc.SetValue(paramIndex, rfcStruct);
        }

        /// <summary>
        /// Process SAP RFC Table Input Data
        /// </summary>
        /// <param name="rfcFunc"></param>
        /// <param name="paramIndex"></param>
        /// <param name="tableData"></param>
        /// Owner:Andy Gao 2011-05-11 13:16:47
        private static void ProcessSAPRFCTableInputData(IRfcFunction rfcFunc, int paramIndex, DataTable tableData)
        {
            IRfcTable rfcTable = rfcFunc.GetTable(paramIndex);

            foreach (DataRow tableRow in tableData.Rows)
            {
                rfcTable.Append();

                IRfcStructure rfcStructRow = rfcTable.CurrentRow;

                foreach (DataColumn column in tableData.Columns)
                {
                    int tableIndex = rfcStructRow.Metadata.TryNameToIndex(column.ColumnName);

                    if (tableIndex >= 0)
                    {
                        object value = tableRow[column];

                        if (value != null && value != DBNull.Value)
                        {
                            rfcStructRow.SetValue(tableIndex, value);
                        }
                    }
                }
            }

            rfcFunc.SetValue(paramIndex, rfcTable);
        }

        /// <summary>
        /// Process SAP RFC Other Input Data
        /// </summary>
        /// <param name="rfcFunc"></param>
        /// <param name="paramIndex"></param>
        /// <param name="value"></param>
        /// Owner:Andy Gao 2011-05-11 13:17:11
        private static void ProcessSAPRFCOtherInputData(IRfcFunction rfcFunc, int paramIndex, object value)
        {
            if (value != null && value != DBNull.Value)
            {
                rfcFunc.SetValue(paramIndex, value);
            }
        }

        /// <summary>
        /// Process SAP RFC Structure Output Data
        /// </summary>
        /// <param name="rfcFunc"></param>
        /// <param name="paramName"></param>
        /// <param name="outputData"></param>
        /// Owner:Andy Gao 2011-05-11 13:17:38
        private static void ProcessSAPRFCStructureOutputData(IRfcFunction rfcFunc, string paramName, DataSet outputData)
        {
            IRfcStructure rfcStruct = rfcFunc.GetStructure(paramName);

            DataTable tableData = new DataTable(paramName);

            for (int fieldIndex = 0; fieldIndex < rfcStruct.Metadata.FieldCount; fieldIndex++)
            {
                tableData.Columns.Add(rfcStruct.Metadata[fieldIndex].Name);
            }

            DataRow tableRow = tableData.NewRow();

            for (int fieldIndex = 0; fieldIndex < rfcStruct.ElementCount; fieldIndex++)
            {
                tableRow[fieldIndex] = rfcStruct[fieldIndex].GetString();
            }

            tableData.Rows.Add(tableRow);

            tableData.AcceptChanges();

            outputData.Tables.Add(tableData);
        }

        /// <summary>
        /// Process SAP RFC Table Output Data
        /// </summary>
        /// <param name="rfcFunc"></param>
        /// <param name="paramName"></param>
        /// <param name="outputData"></param>
        /// Owner:Andy Gao 2011-05-11 13:17:54
        private static void ProcessSAPRFCTableOutputData(IRfcFunction rfcFunc, string paramName, DataSet outputData)
        {
            IRfcTable rfcTable = rfcFunc.GetTable(paramName);

            DataTable tableData = new DataTable(paramName);

            for (int fieldIndex = 0; fieldIndex < rfcTable.Metadata.LineType.FieldCount; fieldIndex++)
            {
                tableData.Columns.Add(rfcTable.Metadata.LineType[fieldIndex].Name);
            }

            for (int rowIndex = 0; rowIndex < rfcTable.RowCount; rowIndex++)
            {
                rfcTable.CurrentIndex = rowIndex;

                IRfcStructure rfcStructRow = rfcTable.CurrentRow;

                DataRow tableRow = tableData.NewRow();

                for (int fieldIndex = 0; fieldIndex < rfcStructRow.ElementCount; fieldIndex++)
                {
                    tableRow[fieldIndex] = rfcStructRow[fieldIndex].GetString();
                }

                tableData.Rows.Add(tableRow);
            }

            tableData.AcceptChanges();

            outputData.Tables.Add(tableData);
        }

        /// <summary>
        /// Process SAP RFC Other Output Data
        /// </summary>
        /// <param name="rfcFunc"></param>
        /// <param name="paramName"></param>
        /// <param name="outputData"></param>
        /// Owner:Andy Gao 2011-05-11 13:18:10
        private static void ProcessSAPRFCOtherOutputData(IRfcFunction rfcFunc, string paramName, DataSet outputData)
        {
            object value = rfcFunc.GetValue(paramName);

            outputData.ExtendedProperties.Add(paramName, value);
        }

        /// <summary>
        /// SAP Common Remote Function Call Interface
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="inputData"></param>
        /// <param name="outputData"></param>
        /// Owner:Andy Gao 2011-05-11 13:15:40
        public static void SAPRemoteFunctionCall(string functionName, DataSet inputData, out DataSet outputData)
        {
            outputData = new DataSet();

            RfcDestination rfcDest = RfcDestinationManager.GetDestination(rfcConfigParams);

            RfcRepository rfcRep = rfcDest.Repository;

            IRfcFunction rfcFunc = rfcRep.CreateFunction(functionName);

            List<string> inputNames = new List<string>();

            if (inputData != null)
            {
                if (inputData.Tables.Count > 0)
                {
                    #region Process SAP Structure And Table Type

                    foreach (DataTable dataTable in inputData.Tables)
                    {
                        if (dataTable == null || dataTable.Rows.Count <= 0) continue;

                        string paramName = dataTable.TableName;

                        int paramIndex = rfcFunc.Metadata.TryNameToIndex(paramName);

                        if (paramIndex < 0) continue;

                        switch (rfcFunc.Metadata[paramIndex].DataType)
                        {
                            case RfcDataType.STRUCTURE:

                                ProcessSAPRFCStructureInputData(rfcFunc, paramIndex, dataTable);

                                break;
                            case RfcDataType.TABLE:

                                ProcessSAPRFCTableInputData(rfcFunc, paramIndex, dataTable);

                                break;
                            default:

                                continue;
                        }

                        inputNames.Add(paramName);
                    }

                    #endregion
                }

                if (inputData.ExtendedProperties.Count > 0)
                {
                    #region Process SAP Other Type

                    foreach (string paramName in inputData.ExtendedProperties.Keys)
                    {
                        int paramIndex = rfcFunc.Metadata.TryNameToIndex(paramName);

                        if (paramIndex < 0) continue;

                        ProcessSAPRFCOtherInputData(rfcFunc, paramIndex, inputData.ExtendedProperties[paramName]);

                        inputNames.Add(paramName);
                    }

                    #endregion
                }
            }

            rfcFunc.Invoke(rfcDest); //Execute RFC Interface

            for (int paramIndex = 0; paramIndex < rfcFunc.Metadata.ParameterCount; paramIndex++)
            {
                string paramName = rfcFunc.Metadata[paramIndex].Name;

                //if (!inputNames.Contains(paramName))
                //{
                    switch (rfcFunc.Metadata[paramIndex].DataType)
                    {
                        case RfcDataType.STRUCTURE:

                            ProcessSAPRFCStructureOutputData(rfcFunc, paramName, outputData);

                            break;
                        case RfcDataType.TABLE:

                            ProcessSAPRFCTableOutputData(rfcFunc, paramName, outputData);

                            break;
                        default:

                            ProcessSAPRFCOtherOutputData(rfcFunc, paramName, outputData);

                            break;
                    }
                //}
            }
        }

        /// <summary>
        /// Lotus Send E-Mail
        /// </summary>
        /// <param name="sendTo"></param>
        /// <param name="copyTo"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// Owner:Andy Gao 2011-08-22 09:32:50
        public static void LotusSendEMail(string[] sendTo, string[] copyTo, string subject, string content)
        {
            NotesSession ns = new NotesSession();

            ns.Initialize(lotusPassword);

            NotesDatabase ndb = ns.GetDatabase(lotusServer, lotusFile, false);

            NotesDocument doc = ndb.CreateDocument();

            doc.ReplaceItemValue(LOTUS_SENDTO, sendTo);
            doc.ReplaceItemValue(LOTUS_COPYTO, copyTo);
            doc.ReplaceItemValue(LOTUS_SUBJECT, subject);

            NotesRichTextItem rti = doc.CreateRichTextItem(LOTUS_BODY);

            rti.AppendText(content);

            object recipients = doc.GetItemValue(LOTUS_SENDTO);

            doc.Send(false, ref recipients);

            Marshal.ReleaseComObject(rti);
            Marshal.ReleaseComObject(doc);
            Marshal.ReleaseComObject(ndb);
            Marshal.ReleaseComObject(ns);

            rti = null;
            doc = null;
            ndb = null;
            ns = null;
        }
    }

    /// <summary>
    /// SAP Destination Configuration Class
    /// </summary>
    /// Owner:Andy Gao 2011-05-10 16:26:13
    public class SAPDestinationConfig : IDestinationConfiguration
    {
        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters rfcConfigParams = new RfcConfigParameters();

            rfcConfigParams.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["AppServerHost"]);
            rfcConfigParams.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SystemNumber"]);
            rfcConfigParams.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["Client"]);
            rfcConfigParams.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["User"]);
            rfcConfigParams.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["Password"]);
            rfcConfigParams.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["Language"]);

            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(destinationName, new RfcConfigurationEventArgs(RfcConfigParameters.EventType.CHANGED, rfcConfigParams));
            }

            return rfcConfigParams;
        }
    }

    //SAP Destination Configuration Class
    /// Owner:shaoxq 2013-10-11 
    public class SapRemoteConnect : IDestinationConfiguration
    {

        public RfcConfigParameters GetParameters(String destinationName)
        {
            if ("TEST".Equals(destinationName))
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                parms.Add(RfcConfigParameters.AppServerHost, "192.168.0.167");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.User, "ckuser");
                parms.Add(RfcConfigParameters.Password, "songhui");
                parms.Add(RfcConfigParameters.Client, "900");
                parms.Add(RfcConfigParameters.Language, "ZH");
                parms.Add(RfcConfigParameters.PoolSize, "5");
                parms.Add(RfcConfigParameters.MaxPoolSize, "10");
                parms.Add(RfcConfigParameters.IdleTimeout, "60");
                return parms;
            }
            else if ("DEV".Equals(destinationName))
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                parms.Add(RfcConfigParameters.AppServerHost, "192.168.0.88");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.User, "XIAYU");
                parms.Add(RfcConfigParameters.Password, "86471050");
                parms.Add(RfcConfigParameters.Client, "900");
                parms.Add(RfcConfigParameters.Language, "ZH");
                parms.Add(RfcConfigParameters.PoolSize, "5");
                parms.Add(RfcConfigParameters.MaxPoolSize, "10");
                parms.Add(RfcConfigParameters.IdleTimeout, "60");
                return parms;
            }
            else if ("PROD".Equals(destinationName))
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                parms.Add(RfcConfigParameters.AppServerHost, "192.168.0.137");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.User, "ckuser");
                parms.Add(RfcConfigParameters.Password, "songhui");
                parms.Add(RfcConfigParameters.Client, "900");
                parms.Add(RfcConfigParameters.Language, "ZH");
                parms.Add(RfcConfigParameters.PoolSize, "5");
                parms.Add(RfcConfigParameters.MaxPoolSize, "10");
                parms.Add(RfcConfigParameters.IdleTimeout, "60");
                return parms;
            }
            if ("ASRS_TEST".Equals(destinationName))
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                parms.Add(RfcConfigParameters.AppServerHost, "10.20.30.107");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.User, "shaoxq");
                parms.Add(RfcConfigParameters.Password, "123456");
                parms.Add(RfcConfigParameters.Client, "400");
                parms.Add(RfcConfigParameters.Language, "ZH");
                parms.Add(RfcConfigParameters.PoolSize, "5");
                parms.Add(RfcConfigParameters.MaxPoolSize, "10");
                parms.Add(RfcConfigParameters.IdleTimeout, "60");
                return parms;
            }
            else
                return null;
        }

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

    }
}