using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using FanHai.Hemera.Share.Constants;
using System.Data;

namespace FanHai.Hemera.Utils.DatabaseHelper
{
    /// <summary>
    /// 为SQL扩展<see cref="String"/>方法。
    /// </summary>
    public static class StringForSQLExtend
    {
        /// <summary>
        /// 防止拼凑SQL字符串的参数值进行SQL注入攻击。
        /// </summary>
        /// <param name="val">用于拼凑SQL字符串的参数值字符串。</param>
        /// <returns>过滤或替换过SQL关键字的字符串。</returns>
        public static string PreventSQLInjection(this string val)
        {
            if (string.IsNullOrEmpty(val)) return val;
            return val.Replace("'", "''");
        }
    }
    /// <summary>
    /// 数据库表。
    /// </summary>
    public class DatabaseTable
    {
        /// <summary>
        /// SELECT语句。
        /// </summary>
        private const string KEY_WORDS_SELECT = @"SELECT ";
        /// <summary>
        /// INSERT INTO语句。
        /// </summary>
        private const string KEY_WORDS_INSERT = @"INSERT INTO ";
        /// <summary>
        /// UPDATE语句。
        /// </summary>
        private const string KEY_WORDS_UPDATE = @"UPDATE ";
        /// <summary>
        /// DELETE FROM语句。
        /// </summary>
        private const string KEY_WORDS_DELETE = @"DELETE FROM ";
        /// <summary>
        /// WHERE语句。
        /// </summary>
        private const string KEY_WORDS_WHERE = @"WHERE ";
        /// <summary>
        /// 创建INSERT SQL语句。
        /// </summary>
        /// <param name="tableFields">数据库表字段对象。</param>
        /// <param name="dtDataFields"></param>
        /// <param name="addFields"></param>
        /// <param name="ignoreFields"></param>
        /// <returns></returns>
        public static string[] BuildInsertSqlStatements(TableFields tableFields, 
                                                        DataTable dtDataFields, 
                                                        Dictionary<string, string> addFields,
                                                        List<string> ignoreFields)
        {
            if (null == tableFields || null == dtDataFields || null == ignoreFields || 0 == dtDataFields.Rows.Count)
            {
                return null;
            }
            string sqlInsertFields = "";
            string sqlAddValues = "";
            string[] sqlCommand = new string[dtDataFields.Rows.Count];
            string[] sqlInsertValues = new string[dtDataFields.Rows.Count];
            foreach (DataColumn column in dtDataFields.Columns)
            {
                if (ignoreFields.Contains(column.ColumnName))
                {
                    continue;
                }
                sqlInsertFields += column.ColumnName + ", ";
                for (int i = 0; i < dtDataFields.Rows.Count; i++)
                {
                    sqlInsertValues[i] += GetStringFromObject(tableFields, column.ColumnName, dtDataFields.Rows[i][column].ToString()) + ", ";
                }
            }
            if (addFields.Count > 0)
            {
                bool blAddFlag = false;
                foreach (KeyValuePair<string, string> keyValue in addFields)
                {
                    if (dtDataFields.Columns.Contains(keyValue.Key) == false)
                    {
                        blAddFlag = true;
                        sqlInsertFields += keyValue.Key + ", ";
                        sqlAddValues += GetStringFromObject(tableFields, keyValue.Key, keyValue.Value) + ", ";
                    }
                }
                if (blAddFlag)
                {
                    sqlAddValues = sqlAddValues.Substring(0, sqlAddValues.Length - 2);
                    blAddFlag = false;
                }
            }

            if (sqlInsertFields.Length < 2)
            {
                return null;
            }
            sqlInsertFields = sqlInsertFields.Substring(0, sqlInsertFields.Length - 2);
            for (int i = 0; i < sqlInsertValues.Length; i++)
            {
                sqlCommand[i] = "INSERT INTO " + dtDataFields.TableName + "(" + sqlInsertFields + ") ";
                if (sqlAddValues.Length > 0)
                {
                    sqlCommand[i] += "VALUES(" + sqlInsertValues[i] + sqlAddValues + ")";
                }
                else
                {
                    sqlCommand[i] += "VALUES(" + sqlInsertValues[i].Substring(0, sqlInsertValues[i].Length - 2) + sqlAddValues + ")";
                }
            }
            return sqlCommand;
        }
        /// <summary>
        /// 创建INSERT SQL字符串集合。
        /// </summary>
        /// <param name="sqlCommandList">存储INSERT SQL的字符串列表对象。</param>
        /// <param name="tableFields">数据库表字段对象。</param>
        /// <param name="dtDataFields">包含待INSERT数据的数据库表对象。</param>
        /// <param name="addFields">附加的数据库表字段。</param>
        /// <param name="ignoreFields">忽略的数据库表字段。</param>
        public static void BuildInsertSqlStatements(ref List<string> sqlCommandList,
                                                    TableFields tableFields, 
                                                    DataTable dtDataFields, 
                                                    Dictionary<string, string> addFields,
                                                    List<string> ignoreFields)
        {
            if (null == sqlCommandList 
                || null == tableFields 
                || null == dtDataFields 
                || 0 == dtDataFields.Rows.Count 
                || null == ignoreFields)
            {
                return;
            }
            string sqlInsertFields = "";
            string sqlAddValues = "";
            string[] sqlInsertValues = new string[dtDataFields.Rows.Count];
            foreach (DataColumn column in dtDataFields.Columns)
            {
                if (ignoreFields.Contains(column.ColumnName) 
                    || addFields.ContainsKey(column.ColumnName))
                {
                    continue;
                }
                sqlInsertFields += column.ColumnName + ", ";
                for (int i = 0; i < dtDataFields.Rows.Count; i++)
                {
                    sqlInsertValues[i] += GetStringFromObject(tableFields, column.ColumnName, dtDataFields.Rows[i][column].ToString()) + ", ";
                }
            }

            if (addFields.Count > 0)
            {
                bool blAddFlag = false;
                foreach (KeyValuePair<string, string> keyValue in addFields)
                {
                    blAddFlag = true;
                    sqlInsertFields += keyValue.Key + ", ";
                    sqlAddValues += GetStringFromObject(tableFields, keyValue.Key, keyValue.Value) + ", ";
                }
                if (blAddFlag)
                {
                    sqlAddValues = sqlAddValues.Substring(0, sqlAddValues.Length - 2);
                    blAddFlag = false;
                }
            }
            if (sqlInsertFields.Length < 2)
            {
                return;
            }
            sqlInsertFields = sqlInsertFields.Substring(0, sqlInsertFields.Length - 2);
            string sqlCommand = "";
            //循环插入数据行数
            for (int i = 0; i < sqlInsertValues.Length; i++)
            {
                sqlCommand = "INSERT INTO " + dtDataFields.TableName + "(" + sqlInsertFields + ") ";
                if (sqlAddValues.Length > 0)
                {
                    sqlCommand += "VALUES(" + sqlInsertValues[i] + sqlAddValues + ")";
                }
                else
                {
                    sqlCommand += "VALUES(" + sqlInsertValues[i].Substring(0, sqlInsertValues[i].Length - 2) + sqlAddValues + ")";
                }
                sqlCommandList.Add(sqlCommand);
            }
        }


        public static string BuildInsertSqlStatement(TableFields tableFields,
                                                    DataTable dtDataFields,
                                                    int nRowNumber, 
                                                    Dictionary<string, string> addFields, 
                                                    List<string> ignoreFields)
        {
            if (null == tableFields || null == dtDataFields || null == ignoreFields || 0 == dtDataFields.Rows.Count)
            {
                return "";
            }
            string sqlCommand ="";
            string sqlInsertFields = "";
            string sqlAddValues = "";
            string sqlInsertValues = "";

            foreach (DataColumn column in dtDataFields.Columns)
            {
                if (ignoreFields.Contains(column.ColumnName))
                {
                    continue;
                }
                sqlInsertFields += column.ColumnName + ", ";

                sqlInsertValues += GetStringFromObject(tableFields, column.ColumnName, dtDataFields.Rows[nRowNumber][column].ToString()) + ", ";
            }

            if (addFields.Count > 0)
            {
                bool blAddFlag = false;
                foreach (KeyValuePair<string, string> keyValue in addFields)
                {
                    if (dtDataFields.Columns.Contains(keyValue.Key) == false)
                    {
                        blAddFlag = true;
                        sqlInsertFields += keyValue.Key + ", ";
                        sqlAddValues += GetStringFromObject(tableFields, keyValue.Key, keyValue.Value) + ", ";
                    }
                }
                if (blAddFlag)
                {
                    sqlAddValues = sqlAddValues.Substring(0, sqlAddValues.Length - 2);
                    blAddFlag = false;
                }
            }

            if (sqlInsertFields.Length < 2)
            {
                return "";
            }

            sqlInsertFields = sqlInsertFields.Substring(0, sqlInsertFields.Length - 2);
            sqlCommand = "INSERT INTO " + dtDataFields.TableName + "(" + sqlInsertFields + ") ";
            if (sqlAddValues.Length > 0)
            {
                sqlCommand += "VALUES(" + sqlInsertValues+ sqlAddValues + ")";
            }
            else
            {
                sqlCommand += "VALUES(" + sqlInsertValues.Substring(0, sqlInsertValues.Length - 2) + sqlAddValues + ")";
            }
            return sqlCommand;
        }

        public static void BuildUpdateSqlStatements(ref List<string> sqlCommandList, 
                                                    TableFields tableFields, 
                                                    DataTable dtDataFields,           
                                                    Dictionary<string, string> addFields,
                                                    List<string> ignoreFields,
                                                    string conditionColumn)
        {
            if (null == sqlCommandList 
                || null == tableFields 
                || null == dtDataFields 
                || 0 == dtDataFields.Rows.Count
                || null == ignoreFields 
                || null == addFields 
                || null == conditionColumn 
                || conditionColumn.Length < 1)
            {
                return;
            }

            DataView dv = new DataView(dtDataFields);
            dv.Sort = COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY;
            DataTable dtKey = dv.ToTable(true, COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY);
            string sqlCommand = "";
            foreach (DataRow rowKey in dtKey.Rows)
            {
                dv.RowFilter = COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY + "= '" + rowKey[COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY] + "'";
                DataTable dtFiltered = dv.ToTable();
                foreach (DataRow dataRow in dtFiltered.Rows)
                {
                    if (dataRow[COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY].ToString() != rowKey[COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY].ToString())
                    {
                        continue;
                    }
                    if (ignoreFields.Contains(dataRow[COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString()))
                    {
                        continue;
                    }
                    if (addFields.Keys.Contains(dataRow[COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString()))
                    {
                        continue;
                    }
                    sqlCommand += dataRow[COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME] + "=" + 
                        GetStringFromObject(tableFields,
                                            Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME]), 
                                            Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE])) + ", ";

                }
                foreach (KeyValuePair<string, string> keyValue in addFields)
                {
                    sqlCommand += keyValue.Key + "=" + GetStringFromObject(tableFields, keyValue.Key, keyValue.Value) + ", ";
                }
                if (sqlCommand.Length > 0)
                {
                    sqlCommand = "UPDATE " + tableFields.TABLE_NAME.PreventSQLInjection() + " SET " + 
                        sqlCommand.Substring(0, sqlCommand.Length - 2) + " WHERE " + conditionColumn + " = '" + 
                        Convert.ToString(rowKey[COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY]).PreventSQLInjection() + "'";
                    sqlCommandList.Add(sqlCommand);
                    sqlCommand = "";
                }
            }
        }

        public static string BuildValueChangeHistoryForUDAs(DataTable dtAttributes)
        {
            string valueChangeHistories = "";
            foreach (DataRow dataRow in dtAttributes.Rows)
            {
                OperationAction operationAction = 
                    (OperationAction)Convert.ToInt32(dataRow[COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                if (OperationAction.New == operationAction)
                {
                    valueChangeHistories += ("NEW: " + dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString() + " = " + 
                                        dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]) + ", ";
                }
                else if (OperationAction.Modified == operationAction)
                {
                    valueChangeHistories += ("MODIFIED: " + dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString() + " = " + 
                        dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]) + ", ";
                }
                else if (OperationAction.Delete == operationAction)
                {
                    valueChangeHistories += ("DELTE: " + dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString() + " = " + 
                        dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]) + ", ";
                }
            }
            if (valueChangeHistories.Length > 2)
            {
                valueChangeHistories = valueChangeHistories.Substring(0, valueChangeHistories.Length - 2);
            }
            return valueChangeHistories;
        }

        public static void BuildSqlStatementsForUDAs(ref List<string> sqlCommandList, TableFields tableFields, 
            DataTable dtAttributes, string keyColumnName)
        {
            if (null == sqlCommandList || null == tableFields || null == dtAttributes || null == keyColumnName || 0 == keyColumnName.Length)
            {
                return;
            }
            string sqlCommand = "";
            DataView dvAttributes = new DataView(dtAttributes);
            dvAttributes.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.New);
            DataTable dtAttributeNew = dvAttributes.ToTable();
            BuildInsertSqlStatements(ref sqlCommandList, tableFields, dtAttributeNew,
                                         new Dictionary<string, string>() { {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                                                            {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}},
                                         new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });

            dvAttributes.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.Modified);
            DataTable dtAttributeUpdate = dvAttributes.ToTable();
            foreach (DataRow dataRow in dtAttributeUpdate.Rows)
            {
                sqlCommand = "UPDATE " + tableFields.TABLE_NAME +
                             " SET " + COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE + " = '" + Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]).PreventSQLInjection() + "'" +
                             " WHERE " + keyColumnName + " = '" + dataRow[keyColumnName] + "'" +
                             " AND " + BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + " = '" + Convert.ToString(dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]).PreventSQLInjection() + "'"; 
                sqlCommandList.Add(sqlCommand);
            }

            dvAttributes.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.Delete);
            DataTable dtAttributeDelete = dvAttributes.ToTable();
            foreach (DataRow dataRow in dtAttributeDelete.Rows)
            {
                sqlCommand = "DELETE FROM " + tableFields.TABLE_NAME +
                             " WHERE " + keyColumnName + " = '" + Convert.ToString(dataRow[1]).PreventSQLInjection() + "'" + 
                             " AND " + BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + " = '" + Convert.ToString(dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]).PreventSQLInjection() + "'";
                sqlCommandList.Add(sqlCommand);
            }
        }

        public static void BuildSqlStatementsForDML(ref List<string> sqlCommandList, TableFields tableFields, TableFields udaTableFields, 
                                                        DataTable dtAttributes, string keyColumnName)
        {
            if (null == sqlCommandList || null == tableFields || null == dtAttributes || null == keyColumnName || 0 == keyColumnName.Length)
            {
                return;
            }
            string sqlCommand = "";
            DataView dvAttributes = new DataView(dtAttributes);
            dvAttributes.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.New);
            DataTable dtAttributeNew = dvAttributes.ToTable();

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (udaTableFields != null)
            {
                dictionary = new Dictionary<string, string>() {{COMMON_FIELDS.FIELD_COMMON_CREATE_TIME,null},
                                                               {COMMON_FIELDS.FIELD_COMMON_CREATE_TIMEZONE, "CN-ZH"},
                                                               {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, null},
                                                               {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}};
            }

            BuildInsertSqlStatements(ref sqlCommandList, tableFields, dtAttributeNew, dictionary,
                                         new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION });

            dvAttributes.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.Modified);
            DataTable dtAttributeUpdate = dvAttributes.ToTable();
            foreach (DataRow dataRow in dtAttributeUpdate.Rows)
            {
                sqlCommand = "UPDATE " + tableFields.TABLE_NAME +
                             " SET " + COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE + " = '" + Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]).PreventSQLInjection() + "'" +
                             " WHERE " + keyColumnName + " = '" + Convert.ToString(dataRow[keyColumnName]).PreventSQLInjection() + "'" +
                             " AND " + BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + " = '" + Convert.ToString(dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]).PreventSQLInjection() + "'";
                sqlCommandList.Add(sqlCommand);
            }

            dvAttributes.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.Delete);
            DataTable dtAttributeDelete = dvAttributes.ToTable();
            foreach (DataRow dr in dtAttributeDelete.Rows)
            {
                if (udaTableFields != null)
                {
                    sqlCommand = "DELETE " + udaTableFields.TABLE_NAME +
                                 " WHERE " + keyColumnName + " = '" + Convert.ToString(dr[keyColumnName]).PreventSQLInjection() + "'";
                    sqlCommandList.Add(sqlCommand);
                }
                //如果是工步表，则删除工步参数。
                if (tableFields.TABLE_NAME == POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME)
                {
                    sqlCommand = string.Format("DELETE FROM {0} WHERE {1}='{2}'",
                                                POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME,
                                                POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_KEY,
                                                Convert.ToString(dr[keyColumnName]).PreventSQLInjection());
                    sqlCommandList.Add(sqlCommand);
                }

                sqlCommand = string.Format("DELETE FROM {0} WHERE {1} = '{2}'",
                                            tableFields.TABLE_NAME,
                                            keyColumnName,
                                            Convert.ToString(dr[keyColumnName]).PreventSQLInjection());
                sqlCommandList.Add(sqlCommand);
            }
        }


        public static void BuildDMLSqlStatements(ref List<string> sqlCommandList, TableFields tableFields, DataTable dataTable, 
                                                    Dictionary<string, string> addFields, List<string> ignoreFields, string conditionColumn)
        {
            if (null == sqlCommandList || null == tableFields || null == dataTable || null == conditionColumn || 0 == conditionColumn.Length)
            {
                return;
            }
            
            DataView dataView = new DataView(dataTable);
            dataView.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.New);
            DataTable dataTableNew = dataView.ToTable();

            BuildInsertSqlStatements(ref sqlCommandList, tableFields, dataTableNew, addFields, ignoreFields);

            dataView.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.Modified);
            DataTable dataTableUpdate = dataView.ToTable();

            BuildUpdateSqlStatements(ref sqlCommandList, tableFields, dataTableUpdate, addFields, ignoreFields, conditionColumn);

            dataView.RowFilter = COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION + "=" + Convert.ToInt32(OperationAction.Delete);
            DataTable dataTableDelete = dataView.ToTable();

            BuildDeleteSqlStatements(ref sqlCommandList, tableFields, dataTableDelete, conditionColumn);
        }

        public static void BuildDeleteSqlStatements(ref List<string> sqlCommandList, TableFields tableFields, DataTable dataTable, string conditionColumn)
        {
            if (null == sqlCommandList || null == tableFields || null == dataTable || 0 == dataTable.Rows.Count 
                                                              || null == conditionColumn || conditionColumn.Length < 1)
            {
                return;
            }

            string sqlCommand = string.Empty;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                sqlCommand = "DELETE " + tableFields.TABLE_NAME +
                             " WHERE " + conditionColumn + " = '" + Convert.ToString(dataRow[conditionColumn]).PreventSQLInjection() + "'";
                sqlCommandList.Add(sqlCommand);
            }
        }

        public static string BuildQuerySqlStatement(TableFields tableFields, Hashtable interestFields, WhereConditions wc)
        {
            if (interestFields.Count < 1) return String.Empty;

            string strSQL = KEY_WORDS_SELECT;
            // add Interesting fields
            foreach (DictionaryEntry field in interestFields)
            {
                strSQL += field.Key.ToString() + ", ";
            }
            strSQL = strSQL.Substring(strSQL.Length - 2);
           
            // add from sub statements
            strSQL = strSQL + "FROM " + tableFields.TABLE_NAME;

            // add where sub statements
            if (null != wc)
            {
            }

            // add final sub statements
            return strSQL;
        }
        
        public static string BuildInsertSqlStatement(TableFields tableFields, Hashtable interestFields, WhereConditions wc)
        {
            if (interestFields.Count < 1) return String.Empty;

            string strSQL = KEY_WORDS_INSERT + tableFields.TABLE_NAME + "(";

            // add Interesting fields: keywords
            foreach (DictionaryEntry field in interestFields)
            {
                strSQL += field.Key.ToString() + ", ";
            }
            strSQL = strSQL.Substring(0,strSQL.Length - 2) + ") VALUES(";

            // add Interesting fields: values
            foreach (DictionaryEntry field in interestFields)
            {
                strSQL += GetStringFromObject(tableFields, field) + ", ";
            }
            strSQL = strSQL.Substring(0,strSQL.Length - 2) + ")";

            // add where sub statements
            if (null != wc)
            {
            }

            // add final sub statements
            return strSQL;
        }

        public static string BuildInsertSqlStatement(TableFields tableFields, DataRow interestFields, WhereConditions wc)
        {
            string strSQL = KEY_WORDS_INSERT + tableFields.TABLE_NAME + "(";
            // add Interesting fields: keywords
            foreach (DataColumn field in interestFields.Table.Columns)
            {
                strSQL += field.ColumnName.ToString().ToUpper() + ", ";
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 2) + ") VALUES(";

            // add Interesting fields: values
            foreach (DataColumn field in interestFields.Table.Columns)
            {
                strSQL += GetStringFromObject(tableFields, interestFields, field) + ", ";
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 2) + ")";

            // add where sub statements
            if (null != wc)
            {
            }

            // add final sub statements
            return strSQL;
        }

        public static string BuildInsertSqlStatement(TableFields tableFields, DataTable interestFields, int rowNumber, WhereConditions wc)
        {
            if (interestFields.Rows.Count < 1) return String.Empty;

            string strSQL = KEY_WORDS_INSERT + tableFields.TABLE_NAME + "(";

            // add Interesting fields: keywords
            foreach (DataColumn field in interestFields.Columns)
            {
                strSQL += field.ColumnName.ToString().ToUpper() + ", ";
            }
            strSQL = strSQL.Substring(0,strSQL.Length - 2) + ") VALUES(";

            // add Interesting fields: values
            foreach (DataColumn field in interestFields.Columns)
            {
                strSQL += GetStringFromObject(tableFields, interestFields.Rows[rowNumber],field) + ", ";
            }
            strSQL = strSQL.Substring(0,strSQL.Length - 2) + ")";

            // add where sub statements
            if (null != wc)
            {
            }

            // add final sub statements
            return strSQL;
        }

        public static string BuildInsertSqlStatement(TableFields tableFields, DataTable interestFields, int rowNumber)
        {
            if (interestFields.Rows.Count < 1) return String.Empty;

            string strSQL = KEY_WORDS_INSERT + tableFields.TABLE_NAME + "(";

            // add Interesting fields: keywords
            foreach (KeyValuePair<string, FieldProperties> field in tableFields.FIELDS)
            {
                if (interestFields.Columns.Contains(field.Key))
                {
                    strSQL += field.Key + ", ";
                }
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 2) + ") VALUES(";

            // add Interesting fields: values
            foreach (DataColumn field in interestFields.Columns)
            {
                if (tableFields.FIELDS.ContainsKey(field.ColumnName))
                {
                    strSQL += GetStringFromObject(tableFields, interestFields.Rows[rowNumber], field) + ", ";
                }
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 2) + ")";

            // add final sub statements
            return strSQL;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="interestFields"></param>
        /// <param name="lwc">(针对单表)多个更新条件</param>
        /// <returns></returns>
        public static string BuildUpdateSqlStatement(TableFields tableFields, Hashtable interestFields, List<WhereConditions> lwc)
        {
            if (interestFields.Count < 1) return String.Empty;

            string strSQL = KEY_WORDS_UPDATE + tableFields.TABLE_NAME + " SET ";
            // add Interesting fields
            foreach (DictionaryEntry field in interestFields)
            {
                strSQL += field.Key.ToString() + " = " + GetStringFromObject(tableFields, field) + ", ";
            }
            if (strSQL.Length > KEY_WORDS_SELECT.Length)
            {
                strSQL = strSQL.Substring(0, strSQL.Length - 2) + " ";
            }

            // add where sub statements
            for (int i = 0; i < lwc.Count; i++)
            {
                if (i == 0)
                    strSQL += KEY_WORDS_WHERE + lwc[i].Key + " = '" + lwc[i].Value.PreventSQLInjection() + "'";
                if (i > 0)
                {
                    strSQL += " AND " + lwc[i].Key + " = '" + lwc[i].Value.PreventSQLInjection() + "'";
                }
            }

            return strSQL;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="interestFields"></param>
        /// <param name="wc">单个更新条件</param>
        /// <returns></returns>
        public static string BuildUpdateSqlStatement(TableFields tableFields, Hashtable interestFields, WhereConditions wc)
        {
            if (interestFields.Count < 1) return String.Empty;

            string strSQL = KEY_WORDS_UPDATE + tableFields.TABLE_NAME + " SET ";
            // add Interesting fields
            foreach (DictionaryEntry field in interestFields)
            {
                strSQL += field.Key.ToString() + " = " + GetStringFromObject(tableFields, field) + ", ";
            }
            if (strSQL.Length > KEY_WORDS_SELECT.Length)
            {
                strSQL = strSQL.Substring(0,strSQL.Length - 2) + " ";
            }

            // add where sub statements
            if (null != wc)
            {
                strSQL += KEY_WORDS_WHERE + wc.Key + " = '" + wc.Value.PreventSQLInjection() + "'";
            }

            // add final sub statements

            return strSQL;
        }

        public static string BuildDeleteSqlStatement(TableFields tableFields, WhereConditions wc)
        {
            
            string strSQL = KEY_WORDS_DELETE + tableFields.TABLE_NAME + " ";

            // add where sub statements
            if (null != wc)
            {
                strSQL += KEY_WORDS_WHERE + wc.Key + " = '" + wc.Value.PreventSQLInjection() + "'";
            }

            return strSQL;
        }

        private static string GetStringFromObject(TableFields tableFields, string key, string value)
        {
            if (tableFields.FIELDS.ContainsKey(key))
            {
                FieldProperties fp = tableFields.FIELDS[key];
                if (Type.GetTypeCode(fp.DATATYPE) == TypeCode.String)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return "DEFAULT";
                    }
                    return string.Format("'{0}'", value.PreventSQLInjection() );
                }
                else if (NumericalTypes.Contains(Type.GetTypeCode(fp.DATATYPE)))
                {                    
                    //if (string.IsNullOrEmpty(value))
                    //{
                    //    return "null";
                    //}
                    //return  value ;
                    if (string.IsNullOrEmpty(value))
                    {
                        if (null == tableFields.FIELDS[key].DefaultValue)
                            return "DEFAULT";
                        else
                            return tableFields.FIELDS[key].DefaultValue.ToString();
                    }
                    else
                        return value;
                }
                else if (Type.GetTypeCode(fp.DATATYPE) == TypeCode.Boolean)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return "DEFAULT";
                    }
                    bool val = Convert.ToBoolean(value);
                    return Convert.ToInt32(val).ToString();
                }
                else if (Type.GetTypeCode(fp.DATATYPE) == TypeCode.DateTime)
                {
                    if (null == value || value.Length == 0)
                    {
                        return ("SYSDATETIME()");
                    }
                    else
                    {
                        DateTime dt = DateTime.Parse(value);
                        return string.Format("'{0}'", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                return "DEFAULT";
            }
            return string.Format("'{0}'", value.PreventSQLInjection());
        }

        private static string GetStringFromObject(TableFields tableFields, DictionaryEntry field)
        {
            if (field.Key == null) return null;

            string key = field.Key.ToString();
            string value = field.Value == null ? string.Empty : Convert.ToString(field.Value).Trim();
            return GetStringFromObject(tableFields, key, value);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="dataRow"></param>
        /// <param name="dataColumn"></param>
        /// <returns></returns>
        private static string GetStringFromObject(TableFields tableFields, DataRow dataRow, DataColumn dataColumn)
        {
            string key = dataColumn.ColumnName.ToUpper();
            string value = dataRow[key].ToString();
            return GetStringFromObject(tableFields, key, value);
        }

        private static List<TypeCode> NumericalTypes = new List<TypeCode>() 
        { 
            TypeCode.Decimal,
            TypeCode.Double, 
            TypeCode.Int16,
            TypeCode.Int32, 
            TypeCode.Int64, 
            TypeCode.Byte, 
            TypeCode.SByte, 
            TypeCode.Single, 
            TypeCode.UInt16,
            TypeCode.UInt32, 
            TypeCode.UInt64 
        };
        /// <summary>
        /// Get value sql string from field type
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-02 12:22:30
        public static string GetValueSqlStringFromFieldType(TableFields tableFields, string key, string value)
        {
            return GetStringFromObject(tableFields, key, value);
        }

        /// <summary>
        /// Get value sql string from field type
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-02 13:21:57
        public static string GetValueSqlStringFromFieldType(TableFields tableFields, DictionaryEntry field)
        {
            string key = field.Key.ToString();
            string value = field.Value.ToString();
            return GetStringFromObject(tableFields, key, value);
        }
        /// <summary>
        /// Build query sql statement
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="interestFields"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-02 13:53:51
        public static string BuildQuerySqlStatement(TableFields tableFields, List<string> interestFields, Conditions conditions)
        {
            StringBuilder sqlString = new StringBuilder(KEY_WORDS_SELECT);    //SELECT
            //判断有无查询字段
            if (interestFields != null && interestFields.Count > 0)
            {//存在查询字段 select 字段1,字段2... 
                foreach (string field in interestFields)
                {
                    sqlString.AppendFormat("{0}, ", field);
                }

                sqlString.Remove(sqlString.Length - 2, 2);
            }
            else
            {//没有查询字段select *
                sqlString.Append(" *");
            }
            //添加sql+ FROM 表名
            sqlString.AppendFormat(" FROM {0} ", tableFields.TABLE_NAME);

            if (conditions != null && conditions.ConditionList.Count > 0)
            {
                sqlString.AppendFormat("{0} 1 = 1", KEY_WORDS_WHERE);          

                for (int i = 0; i < conditions.ConditionList.Count; i++)
                {
                    Condition condition = conditions.ConditionList[i];

                    condition.FieldValue = GetValueSqlStringFromFieldType(tableFields, condition.FieldName, condition.FieldValue);

                    sqlString.Append(condition.ToString());
                }
            }

            return sqlString.ToString();
        }

        /// <summary>
        /// Build Insert Sql Statement
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="interestFields"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-05 13:21:35
        public static string BuildInsertSqlStatement(TableFields tableFields, Hashtable interestFields)
        {
            if (interestFields != null && interestFields.Count > 0)
            {
                StringBuilder sqlString = new StringBuilder(KEY_WORDS_INSERT);

                sqlString.AppendFormat("{0} (", tableFields.TABLE_NAME);

                StringBuilder valuesString = new StringBuilder("VALUES(");

                foreach (DictionaryEntry field in interestFields)
                {
                    sqlString.AppendFormat("{0}, ", field.Key);

                    valuesString.AppendFormat("{0}, ", GetValueSqlStringFromFieldType(tableFields, field));
                }

                sqlString.Remove(sqlString.Length - 2, 2);
                sqlString.Append(")");

                valuesString.Remove(sqlString.Length - 2, 2);
                valuesString.Append(")");

                sqlString.AppendFormat(" {0}", valuesString.ToString());

                return sqlString.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Build Insert Sql Statement（Author：weixian.lu）
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="interestFields"></param>
        /// <returns></returns>
        public static string BuildInsertSqlStatement2(TableFields table, Hashtable interestFields)
        {
            if (interestFields == null || interestFields.Count < 1) return string.Empty;

            StringBuilder sqlString = new StringBuilder(KEY_WORDS_INSERT);
            StringBuilder valuesString = new StringBuilder(" VALUES (");
            bool first = true;

            sqlString.AppendFormat(" {0} (", table.TABLE_NAME);

            foreach (DictionaryEntry field in interestFields)
            {
                if (first)
                {
                    sqlString.AppendFormat("{0}", field.Key);
                    valuesString.AppendFormat("{0}", GetStringFromObject2(table, field));
                    first = false;
                }
                else
                {
                    sqlString.AppendFormat(" ,{0}", field.Key);
                    valuesString.AppendFormat(" ,{0}", GetStringFromObject2(table, field));
                }
            }

            sqlString.Append(")");
            valuesString.Append(")");

            sqlString.AppendFormat(" {0}", valuesString);

            return sqlString.ToString();
        }
        private static string GetStringFromObject2(TableFields table, DictionaryEntry field)
        {
            string key = field.Key.ToString();
            string value = field.Value != null ? field.Value.ToString() : string.Empty;

            if (table.FIELDS.ContainsKey(key))
            {
                FieldProperties fp = table.FIELDS[key];
                if (Type.GetTypeCode(fp.DATATYPE) == TypeCode.String)
                {
                    return string.IsNullOrEmpty(value) ? (fp.CanNull || fp.DefaultValue == null ? "DEFAULT" : string.Format("'{0}'", fp.DefaultValue))
                                                       : string.Format("'{0}'", value.PreventSQLInjection());
                }
                else if (NumericalTypes.Contains(Type.GetTypeCode(fp.DATATYPE)))
                {
                    return string.IsNullOrEmpty(value) ? (fp.CanNull || fp.DefaultValue == null ? "DEFAULT" : fp.DefaultValue.ToString())
                                                       : value.PreventSQLInjection();
                }
                else if (Type.GetTypeCode(fp.DATATYPE) == TypeCode.Boolean)
                {
                    return string.IsNullOrEmpty(value) ? (fp.CanNull || fp.DefaultValue == null ? "DEFAULT" : fp.DefaultValue.ToString())
                                                       : Convert.ToInt32(Convert.ToBoolean(value)).ToString();
                }
                else if (Type.GetTypeCode(fp.DATATYPE) == TypeCode.DateTime)
                {
                    return string.IsNullOrEmpty(value) ? (fp.CanNull ? "DEFAULT" : (fp.DefaultValue == null ? "GETDATE()" : string.Format("'{0}'", DateTime.Parse(fp.DefaultValue.ToString()).ToString("yyyy-MM-dd HH:mm:ss"))))
                                                       : string.Format("'{0}'", DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            return string.IsNullOrEmpty(value) ? "DEFAULT" : string.Format("'{0}'", value.PreventSQLInjection());
        }

        /// <summary>
        /// Build Update Sql Statement
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="interestFields"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-05 14:59:35
        public static string BuildUpdateSqlStatement(TableFields tableFields, Hashtable interestFields, Conditions conditions)
        {
            if (interestFields != null && interestFields.Count > 0)
            {
                StringBuilder sqlString = new StringBuilder(KEY_WORDS_UPDATE);

                sqlString.AppendFormat("{0} SET ", tableFields.TABLE_NAME);

                foreach (DictionaryEntry field in interestFields)
                {
                    sqlString.AppendFormat("{0} = {1}, ", field.Key.ToString(), GetValueSqlStringFromFieldType(tableFields, field));
                }

                sqlString.Remove(sqlString.Length - 2, 2);

                if (conditions != null && conditions.ConditionList.Count > 0)
                {
                    sqlString.AppendFormat(" {0} 1 = 1", KEY_WORDS_WHERE);

                    for (int i = 0; i < conditions.ConditionList.Count; i++)
                    {
                        Condition condition = conditions.ConditionList[i];

                        condition.FieldValue = GetValueSqlStringFromFieldType(tableFields, condition.FieldName, condition.FieldValue);

                        sqlString.Append(condition.ToString());
                    }
                }

                return sqlString.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Build Delete Sql Statement
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-05 15:10:54
        public static string BuildDeleteSqlStatement(TableFields tableFields, Conditions conditions)
        {
            StringBuilder sqlString = new StringBuilder(KEY_WORDS_DELETE);

            sqlString.AppendFormat("{0} ", tableFields.TABLE_NAME);

            if (conditions != null && conditions.ConditionList.Count > 0)
            {
                sqlString.AppendFormat(" {0} 1 = 1", KEY_WORDS_WHERE);

                for (int i = 0; i < conditions.ConditionList.Count; i++)
                {
                    Condition condition = conditions.ConditionList[i];

                    condition.FieldValue = GetValueSqlStringFromFieldType(tableFields, condition.FieldName, condition.FieldValue);

                    sqlString.Append(condition.ToString());
                }
            }

            return sqlString.ToString();
        }

        /// <summary>
        /// Build Insert Sql Statements
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-12 11:13:03
        public static List<string> BuildInsertSqlStatements(TableFields tableFields, DataTable dataTable)
        {
            List<string> sqlStringList = new List<string>();

            if (tableFields != null && dataTable != null && dataTable.Columns.Count > 0 && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    StringBuilder sqlString = new StringBuilder(KEY_WORDS_INSERT); //insert into 
                    sqlString.AppendFormat("{0} (", tableFields.TABLE_NAME);
                    StringBuilder valuesString = new StringBuilder("VALUES(");
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (tableFields.FIELDS.Keys.Contains(column.ColumnName) && row[column] != null && row[column] != DBNull.Value)
                        {
                            sqlString.AppendFormat("{0}, ", column.ColumnName);

                            valuesString.AppendFormat("{0}, ", GetValueSqlStringFromFieldType(tableFields, column.ColumnName, row[column].ToString()));
                        }
                    }

                    sqlString.Remove(sqlString.Length - 2, 2);
                    sqlString.Append(")");

                    valuesString.Remove(valuesString.Length - 2, 2);
                    valuesString.Append(")");

                    sqlString.AppendFormat(" {0}", valuesString.ToString());

                    sqlStringList.Add(sqlString.ToString());
                }
            }

            return sqlStringList;
        }

        /// <summary>
        /// Build Update Sql Statements
        /// </summary>
        /// <param name="tableFields"></param>
        /// <param name="dataTable"></param>
        /// <param name="paramTable"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-12 12:58:09
        public static List<string> BuildUpdateSqlStatements(TableFields tableFields, DataTable dataTable, List<Conditions> conditionsList)
        {
            List<string> sqlStringList = new List<string>();

            if (tableFields != null && dataTable != null && conditionsList != null &&
                dataTable.Columns.Count > 0 && dataTable.Rows.Count > 0 &&
                conditionsList.Count > 0 && dataTable.Rows.Count == conditionsList.Count)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++ )
                {
                    DataRow row = dataTable.Rows[i];
                    Conditions conditions = conditionsList[i];

                    StringBuilder sqlString = new StringBuilder(KEY_WORDS_UPDATE);

                    sqlString.AppendFormat("{0} SET ", tableFields.TABLE_NAME);

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (tableFields.FIELDS.Keys.Contains(column.ColumnName) && row[column] != null && row[column] != DBNull.Value)
                        {
                            sqlString.AppendFormat("{0} = {1}, ", column.ColumnName, GetValueSqlStringFromFieldType(tableFields, column.ColumnName, row[column].ToString()));
                        }
                    }

                    sqlString.Remove(sqlString.Length - 2, 2);

                    if (conditions != null && conditions.ConditionList.Count > 0)
                    {
                        sqlString.AppendFormat(" {0} 1 = 1", KEY_WORDS_WHERE);

                        for (int j = 0; j < conditions.ConditionList.Count; j++)
                        {
                            Condition condition = conditions.ConditionList[j];

                            condition.FieldValue = GetValueSqlStringFromFieldType(tableFields, condition.FieldName, condition.FieldValue);

                            sqlString.Append(condition.ToString());
                        }
                    }

                    sqlStringList.Add(sqlString.ToString());
                }
            }

            return sqlStringList;
        }

        /// <summary>
        /// 创建SQL删除语句。
        /// </summary>
        /// <param name="tableFields">表示数据表的对象。</param>
        /// <param name="conditionsList">删除条件列表。</param>
        /// <returns>包含SQL删除语句的字符串清单。</returns>
        public static List<string> BuildDeleteSqlStatements(TableFields tableFields, List<Conditions> conditionsList)
        {
            List<string> sqlStringList = new List<string>();

            if (tableFields != null && conditionsList != null)
            {
                foreach (Conditions conditions in conditionsList)
                {
                    StringBuilder sqlString = new StringBuilder(KEY_WORDS_DELETE);
                    sqlString.AppendFormat("{0} ", tableFields.TABLE_NAME);
                    if (conditions != null && conditions.ConditionList.Count > 0)
                    {
                        sqlString.AppendFormat(" {0} 1 = 1", KEY_WORDS_WHERE);       
                        for (int i = 0; i < conditions.ConditionList.Count; i++)
                        {
                            Condition condition = conditions.ConditionList[i];
                            condition.FieldValue = GetValueSqlStringFromFieldType(tableFields, condition.FieldName, condition.FieldValue);
                            sqlString.Append(condition.ToString());
                        }
                    }
                    sqlStringList.Add(sqlString.ToString());
                }
            }
            return sqlStringList;
        }

    
    }
}
