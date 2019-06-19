using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Utils.StaticFuncs;
using System.Data.Common;

namespace SolarViewer.Hemera.Modules.EMS
{
    public class EquipmentCheckListEngine : AbstractEngine
    {
        private Database db;

        public EquipmentCheckListEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentCheckListEngine()
        {

        }

        public override void Initialize()
        {

        }

        public DataSet GetCheckList(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string checkListName = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    EMS_CHECKLIST_FIELDS checkListFields = new EMS_CHECKLIST_FIELDS();

                    List<string> interestColumns = new List<string>();

                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION);

                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATOR);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDITOR);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    interestColumns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME);

                    Conditions conditions = new Conditions();

                    if (!string.IsNullOrEmpty(checkListName))
                    {
                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, GlobalEnums.DatabaseCompareOperator.Like, string.Format("%{0}%", checkListName));
                    }

                    string sqlString = DatabaseTable.BuildQuerySqlStatement(checkListFields, interestColumns, conditions);

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetCheckList Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet GetCheckListItems(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY))
            {
                string checkListKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();

                string sqlString = string.Format(@"SELECT CLI.CHECKLIST_KEY,
                                   CLI.CHECKITEM_KEY,
                                   CLI.SEQUENCE,
                                   CI.CHECKITEM_NAME,
                                   CI.CHECKITEM_TYPE,
                                   CI.DESCRIPTION,
                                   CLI.STANDARD,
                                   CLI.OPTIONAL
                              FROM EMS_CHECKLIST_ITEM CLI, EMS_CHECKITEMS CI, EMS_CHECKLIST C
                             WHERE C.CHECKLIST_KEY = CLI.CHECKLIST_KEY
                               AND CI.CHECKITEM_KEY = CLI.CHECKITEM_KEY
                               AND C.CHECKLIST_KEY = '{0}' ORDER BY CLI.SEQUENCE ASC", checkListKey);

                try
                {
                    db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME });

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetCheckListItems Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertCheckList(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable checkListDataTable = reqDS.Tables[EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME];

                try
                {
                    EMS_CHECKLIST_FIELDS checkListFields = new EMS_CHECKLIST_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(checkListFields, checkListDataTable);

                    string checkListKey = string.Empty;
                    string checkListName = string.Empty;
                    string createTime = string.Empty;
                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0 && checkListDataTable.Rows.Count > 0)
                    {
                        checkListKey = checkListDataTable.Rows[0][EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY].ToString();
                        checkListName = checkListDataTable.Rows[0][EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Validate Check List Name

                                string returnData = AllCommonFunctions.GetSpecifyTableColumnData(checkListFields, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, checkListName, transaction);

                                if (!string.IsNullOrEmpty(returnData))
                                {
                                    throw new Exception("检查表单名称已存在!");
                                }

                                #endregion

                                #region Insert Check List Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    createTime = AllCommonFunctions.GetSpecifyTableColumnData(checkListFields, EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY, checkListKey, transaction);
                                }
                                else
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Insert Check List Items Data

                                if (reqDS.Tables[EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME] != null && reqDS.Tables[EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                                {
                                    DbCommand insertCommand = connection.CreateCommand();

                                    insertCommand.CommandType = CommandType.Text;
                                    insertCommand.CommandText = @"INSERT INTO EMS_CHECKLIST_ITEM
                                                                      (CHECKLIST_KEY, CHECKITEM_KEY, SEQUENCE, STANDARD, OPTIONAL)
                                                                    VALUES
                                                                      (:P1, :P2, :P3, :P4, :P5)";

                                    db.AddInParameter(insertCommand, "P1", DbType.String, checkListKey);
                                    db.AddInParameter(insertCommand, "P2", DbType.String, EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P3", DbType.Int32, EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P4", DbType.String, EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P5", DbType.Int32, EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL, DataRowVersion.Current);

                                    if (db.UpdateDataSet(reqDS, EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME, insertCommand, null, null, transaction) <= 0)
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }
                                }

                                #endregion

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, createTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("InsertCheckList Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateCheckList(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                DataTable checkListDataTable = reqDS.Tables[EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME];

                string checkListKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_CHECKLIST_FIELDS checkListFields = new EMS_CHECKLIST_FIELDS();

                    #region Build Update SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(checkListKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, checkListKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(checkListFields, checkListDataTable, conditionsList);

                    string checkListName = string.Empty;
                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0 && checkListDataTable.Rows.Count > 0)
                    {
                        checkListName = checkListDataTable.Rows[0][EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Validate Check List Name

                                if (!string.IsNullOrEmpty(checkListName))
                                {
                                    conditions = new Conditions();

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, GlobalEnums.DatabaseCompareOperator.Equal, checkListName);

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY, GlobalEnums.DatabaseCompareOperator.NotEqual, checkListKey);

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(checkListFields, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, conditions, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("检查表单名称已存在!");
                                    }
                                }

                                #endregion

                                #region Update Check List Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    editTime = AllCommonFunctions.GetSpecifyTableColumnData(checkListFields, EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY,
                                        checkListKey, transaction);
                                }
                                else
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                #region Insert Check List Items Data

                                if (reqDS.Tables[EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME] != null && reqDS.Tables[EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                                {
                                    #region Delete Check List Items Data

                                    sqlString = string.Format("DELETE EMS_CHECKLIST_ITEM WHERE CHECKLIST_KEY = '{0}'", checkListKey);

                                    db.ExecuteNonQuery(transaction, CommandType.Text, sqlString);

                                    #endregion

                                    DbCommand insertCommand = connection.CreateCommand();

                                    insertCommand.CommandType = CommandType.Text;
                                    insertCommand.CommandText = @"INSERT INTO EMS_CHECKLIST_ITEM
                                                                      (CHECKLIST_KEY, CHECKITEM_KEY, SEQUENCE, STANDARD, OPTIONAL)
                                                                    VALUES
                                                                      (:P1, :P2, :P3, :P4, :P5)";

                                    db.AddInParameter(insertCommand, "P1", DbType.String, checkListKey);
                                    db.AddInParameter(insertCommand, "P2", DbType.String, EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P3", DbType.Int32, EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P4", DbType.String, EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD, DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P5", DbType.Int32, EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL, DataRowVersion.Current);

                                    if (db.UpdateDataSet(reqDS, EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME, insertCommand, null, null, transaction) <= 0)
                                    {
                                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                    }
                                }

                                #endregion

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateCheckList Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet DeleteCheckList(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string checkListKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_CHECKLIST_FIELDS checkListFields = new EMS_CHECKLIST_FIELDS();

                    #region Build Delete SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(checkListKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, checkListKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(checkListFields, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0)
                    {
                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Validate Check List Jobs Reference

                                conditions = new Conditions();

                                conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkListKey);

                                EMS_CHECKLIST_JOBS_FIELDS checkListJobsFields = new EMS_CHECKLIST_JOBS_FIELDS();

                                List<string> interestColumns = new List<string>() { EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY };

                                string checkSqlString = DatabaseTable.BuildQuerySqlStatement(checkListJobsFields, interestColumns, conditions);

                                object scalar = db.ExecuteScalar(transaction, CommandType.Text, checkSqlString);

                                if (scalar != null && scalar != DBNull.Value)
                                {
                                    throw new Exception("检查表单已经关联检查表单任务,不能删除!");
                                }

                                #endregion

                                #region Validate PM Schedule Reference

                                conditions = new Conditions();

                                conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_SCHEDULE_FIELDS.FIELD_CHECKLIST_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkListKey);

                                EMS_PM_SCHEDULE_FIELDS schedulePMFields = new EMS_PM_SCHEDULE_FIELDS();

                                interestColumns = new List<string>() { EMS_PM_SCHEDULE_FIELDS.FIELD_SCHEDULE_KEY };

                                checkSqlString = DatabaseTable.BuildQuerySqlStatement(schedulePMFields, interestColumns, conditions);

                                scalar = db.ExecuteScalar(transaction, CommandType.Text, checkSqlString);

                                if (scalar != null && scalar != DBNull.Value)
                                {
                                    throw new Exception("检查表单已经关联计划PM,不能删除!");
                                }

                                #endregion

                                #region Validate PM Condition Reference

                                conditions = new Conditions();

                                conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CHECKLIST_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkListKey);

                                EMS_PM_CONDITION_FIELDS conditionPMFields = new EMS_PM_CONDITION_FIELDS();

                                interestColumns = new List<string>() { EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY };

                                checkSqlString = DatabaseTable.BuildQuerySqlStatement(conditionPMFields, interestColumns, conditions);

                                scalar = db.ExecuteScalar(transaction, CommandType.Text, checkSqlString);

                                if (scalar != null && scalar != DBNull.Value)
                                {
                                    throw new Exception("检查表单已经关联条件PM,不能删除!");
                                }

                                #endregion

                                #region Delete Check List Items Data

                                db.ExecuteNonQuery(transaction, CommandType.Text, string.Format("DELETE EMS_CHECKLIST_ITEM WHERE CHECKLIST_KEY = '{0}'", checkListKey));

                                #endregion

                                #region Delete Check List Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) <= 0)
                                {
                                    throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                                }

                                #endregion

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("DeleteCheckList Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }
    }
}
