using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Share.Interface.EquipmentManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Utils.StaticFuncs;
using System.Data.Common;

namespace SolarViewer.Hemera.Modules.EMS
{
    public class EquipmentCheckItemsEngine : AbstractEngine
    {
        private Database db;

        public EquipmentCheckItemsEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentCheckItemsEngine()
        {
            
        }

        public override void Initialize()
        {

        }

        public DataSet GetCheckItems(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string checkItemName = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    EMS_CHECKITEMS_FIELDS checkItemsFields = new EMS_CHECKITEMS_FIELDS();

                    List<string> interestColumns = new List<string>();

                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION);

                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATOR);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDITOR);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    interestColumns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME);

                    Conditions conditions = new Conditions();

                    if (!string.IsNullOrEmpty(checkItemName))
                    {
                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, GlobalEnums.DatabaseCompareOperator.Like, string.Format("%{0}%", checkItemName));
                    }

                    string sqlString = DatabaseTable.BuildQuerySqlStatement(checkItemsFields, interestColumns, conditions);

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetCheckItems Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertCheckItem(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable checkItemsDataTable = reqDS.Tables[EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME];

                try
                {
                    EMS_CHECKITEMS_FIELDS checkItemsFields = new EMS_CHECKITEMS_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(checkItemsFields, checkItemsDataTable);

                    string checkItemKey = string.Empty;
                    string checkItemName = string.Empty;
                    string createTime = string.Empty;
                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0 && checkItemsDataTable.Rows.Count > 0)
                    {
                        checkItemKey = checkItemsDataTable.Rows[0][EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY].ToString();
                        checkItemName = checkItemsDataTable.Rows[0][EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Validate Check Item Name

                                string returnData = AllCommonFunctions.GetSpecifyTableColumnData(checkItemsFields, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, checkItemName, transaction);

                                if (!string.IsNullOrEmpty(returnData))
                                {
                                    throw new Exception("检查项名称已存在!");
                                }

                                #endregion

                                #region Insert Check Item Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    createTime = AllCommonFunctions.GetSpecifyTableColumnData(checkItemsFields, EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY, checkItemKey, transaction);
                                }
                                else
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
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, createTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("InsertCheckItem Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateCheckItem(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                DataTable checkItemsDataTable = reqDS.Tables[EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME];

                string checkItemKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_CHECKITEMS_FIELDS checkItemsFields = new EMS_CHECKITEMS_FIELDS();

                    #region Build Update SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(checkItemKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, checkItemKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(checkItemsFields, checkItemsDataTable, conditionsList);

                    string checkItemName = string.Empty;
                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0 && checkItemsDataTable.Rows.Count > 0)
                    {
                        checkItemName = checkItemsDataTable.Rows[0][EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Validate Check Item Name

                                if (!string.IsNullOrEmpty(checkItemName))
                                {
                                    conditions = new Conditions();

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, GlobalEnums.DatabaseCompareOperator.Equal, checkItemName);

                                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY, GlobalEnums.DatabaseCompareOperator.NotEqual, checkItemKey);

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(checkItemsFields, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, conditions, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("检查项名称已存在!");
                                    }
                                }

                                #endregion

                                #region Update Check Item Data

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    editTime = AllCommonFunctions.GetSpecifyTableColumnData(checkItemsFields, EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY,
                                        checkItemKey, transaction);
                                }
                                else
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
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_EDIT_TIME, editTime);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateCheckItem Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet DeleteCheckItem(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string checkItemKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_CHECKITEMS_FIELDS checkItemsFields = new EMS_CHECKITEMS_FIELDS();

                    #region Build Delete SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(checkItemKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, checkItemKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(checkItemsFields, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0)
                    {
                        sqlString = sqlStringList[0];
                    }

                    #region Validate Check List Reference

                    conditions = new Conditions();

                    conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY, GlobalEnums.DatabaseCompareOperator.Equal, checkItemKey);

                    EMS_CHECKLIST_ITEM_FIELDS checkListItemFields = new EMS_CHECKLIST_ITEM_FIELDS();

                    List<string> interestColumns = new List<string>() { EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY };

                    string checkSqlString = DatabaseTable.BuildQuerySqlStatement(checkListItemFields, interestColumns, conditions);

                    object scalar = db.ExecuteScalar(CommandType.Text, checkSqlString);

                    if (scalar != null && scalar != DBNull.Value)
                    {
                        throw new Exception("检查项已经关联检查表单,不能删除!");
                    }

                    #endregion

                    #region Delete Check Item Data

                    if (db.ExecuteNonQuery(CommandType.Text, sqlString) > 0)
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    }
                    else
                    {
                        throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("DeleteCheckItem Error: " + ex.Message);
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
