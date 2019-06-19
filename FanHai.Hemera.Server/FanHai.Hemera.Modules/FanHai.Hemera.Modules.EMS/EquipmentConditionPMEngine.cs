using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using SolarViewer.Hemera.Utils.DatabaseHelper;

namespace SolarViewer.Hemera.Modules.EMS
{
    public class EquipmentConditionPMEngine : AbstractEngine
    {
        private Database db;

        public EquipmentConditionPMEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentConditionPMEngine(){}

        public override void Initialize(){}

        public DataSet GetConditionPM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string equipmentKey = reqDS.ExtendedProperties.ContainsKey(EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_KEY) ? reqDS.ExtendedProperties[EMS_PM_CONDITION_FIELDS.FIELD_EQUIPMENT_KEY].ToString() : string.Empty;

                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    string sqlString = @"SELECT PC.*,
                                               U.USERNAME AS NOTIFY_USER_NAME,
                                               UC.USERNAME AS NOTIFY_CC_USER_NAME,
                                               ECS.EQUIPMENT_CHANGE_STATE_NAME,
                                               ECR.EQUIPMENT_CHANGE_REASON_NAME,
                                               E.EQUIPMENT_NAME,
                                               C.CHECKLIST_NAME
                                          FROM EMS_PM_CONDITION            PC,
                                               EMS_CHECKLIST                C,
                                               EMS_EQUIPMENTS               E,
                                               RBAC_USER                    U,
                                               RBAC_USER                    UC,
                                               EMS_EQUIPMENT_CHANGE_REASONS ECR,
                                               EMS_EQUIPMENT_CHANGE_STATES  ECS
                                         WHERE PC.CHECKLIST_KEY = C.CHECKLIST_KEY
                                           AND PC.EQUIPMENT_KEY = E.EQUIPMENT_KEY
                                           AND PC.NOTIFY_USER_KEY = U.USER_KEY
                                           AND PC.NOTIFY_CC_USER_KEY = UC.USER_KEY(+)
                                           AND PC.EQUIPMENT_CHANGE_REASON_KEY = ECR.EQUIPMENT_CHANGE_REASON_KEY(+)
                                           AND PC.EQUIPMENT_CHANGE_STATE_KEY = ECS.EQUIPMENT_CHANGE_STATE_KEY(+)";

                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sqlString += string.Format(" AND E.EQUIPMENT_KEY = '{0}'", equipmentKey);
                    }

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetConditionPM Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertConditionPM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable conditionPMDataTable = reqDS.Tables[EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME];

                if (conditionPMDataTable.Rows.Count > 0)
                {
                    try
                    {
                        EMS_PM_CONDITION_FIELDS conditionPMFields = new EMS_PM_CONDITION_FIELDS();

                        List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(conditionPMFields, conditionPMDataTable);

                        string conditionKey = conditionPMDataTable.Rows[0][EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY].ToString();
                        string conditionName = conditionPMDataTable.Rows[0][EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME].ToString();
                        string createTime = string.Empty;
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
                                    #region Validate Condition PM Name

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(conditionPMFields, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME, conditionName, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("条件PM名称已存在!");
                                    }

                                    #endregion

                                    #region Insert Condition PM Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                    {
                                        createTime = AllCommonFunctions.GetSpecifyTableColumnData(conditionPMFields, EMS_PM_CONDITION_FIELDS.FIELD_CREATE_TIME, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY, conditionKey, transaction);
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

                        LogService.LogError("InsertConditionPM Error: " + ex.Message);
                    }
                }
                else
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet UpdateConditionPM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                DataTable conditionPMDataTable = reqDS.Tables[EMS_PM_CONDITION_FIELDS.DATABASE_TABLE_NAME];

                string conditionKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_PM_CONDITION_FIELDS conditionPMFields = new EMS_PM_CONDITION_FIELDS();

                    #region Build Update SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(conditionKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, conditionKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(conditionPMFields, conditionPMDataTable, conditionsList);

                    if (sqlStringList.Count > 0 && conditionPMDataTable.Rows.Count > 0)
                    {
                        string conditionName = conditionPMDataTable.Rows[0][EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME].ToString();
                        string sqlString = sqlStringList[0];

                        using (DbConnection connection = db.CreateConnection())
                        {
                            connection.Open();

                            using (DbTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    #region Validate Condition PM Name

                                    if (!string.IsNullOrEmpty(conditionName))
                                    {
                                        conditions = new Conditions();

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME, GlobalEnums.DatabaseCompareOperator.Equal, conditionName);

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY, GlobalEnums.DatabaseCompareOperator.NotEqual, conditionKey);

                                        string returnData = AllCommonFunctions.GetSpecifyTableColumnData(conditionPMFields, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_NAME, conditions, transaction);

                                        if (!string.IsNullOrEmpty(returnData))
                                        {
                                            throw new Exception("条件PM名称已存在!");
                                        }
                                    }

                                    #endregion

                                    #region Update Condition PM Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                    {
                                        editTime = AllCommonFunctions.GetSpecifyTableColumnData(conditionPMFields, EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY, conditionKey, transaction);
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
                    else
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                    }
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("UpdateConditionPM Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet DeleteConditionPM(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string conditionKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_PM_CONDITION_FIELDS conditionPMFields = new EMS_PM_CONDITION_FIELDS();

                    #region Build Delete SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(conditionKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_CONDITION_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, conditionKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_PM_CONDITION_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(conditionPMFields, conditionsList);

                    if (sqlStringList.Count > 0)
                    {
                        #region Delete Condition PM Data

                        if (db.ExecuteNonQuery(CommandType.Text, sqlStringList[0]) > 0)
                        {
                            resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                        }
                        else
                        {
                            throw new Exception("数据处理失败,请重新刷新数据后再提交!");
                        }

                        #endregion
                    }
                    else
                    {
                        resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
                    }
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("DeleteConditionPM Error: " + ex.Message);
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
