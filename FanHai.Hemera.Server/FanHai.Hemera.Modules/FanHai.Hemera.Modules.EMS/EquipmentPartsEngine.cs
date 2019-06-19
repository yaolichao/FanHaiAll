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
    public class EquipmentPartsEngine : AbstractEngine
    {
        private Database db;

        public EquipmentPartsEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        ~EquipmentPartsEngine()
        {

        }

        public override void Initialize()
        {

        }

        public DataSet GetParts(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string partName = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    EMS_EQUIPMENT_PARTS_FIELDS partsFields = new EMS_EQUIPMENT_PARTS_FIELDS();

                    List<string> interestColumns = new List<string>();

                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT);

                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    interestColumns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME);

                    Conditions conditions = new Conditions();

                    if (!string.IsNullOrEmpty(partName))
                    {
                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, GlobalEnums.DatabaseCompareOperator.Like, string.Format("%{0}%", partName));
                    }

                    string sqlString = DatabaseTable.BuildQuerySqlStatement(partsFields, interestColumns, conditions);

                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetParts Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return resDS;
        }

        public DataSet InsertPart(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null && reqDS.Tables.Contains(EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable partsDataTable = reqDS.Tables[EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME];

                if (partsDataTable.Rows.Count > 0)
                {
                    try
                    {
                        EMS_EQUIPMENT_PARTS_FIELDS partsFields = new EMS_EQUIPMENT_PARTS_FIELDS();

                        List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(partsFields, partsDataTable);

                        string partKey = partsDataTable.Rows[0][EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY].ToString();
                        string partName = partsDataTable.Rows[0][EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME].ToString();
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
                                    #region Validate Part Name

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(partsFields, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, partName, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("备件名称已存在!");
                                    }

                                    #endregion

                                    #region Insert Part Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                    {
                                        createTime = AllCommonFunctions.GetSpecifyTableColumnData(partsFields, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY, partKey, transaction);
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

                        LogService.LogError("InsertPart Error: " + ex.Message);
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

        public DataSet UpdatePart(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.Tables.Contains(EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                DataTable partsDataTable = reqDS.Tables[EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME];

                string partKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                if (partsDataTable.Rows.Count > 0)
                {
                    string partName = partsDataTable.Rows[0][EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME].ToString();

                    try
                    {
                        EMS_EQUIPMENT_PARTS_FIELDS partsFields = new EMS_EQUIPMENT_PARTS_FIELDS();

                        #region Build Update SQL Conditions

                        Conditions conditions = new Conditions();

                        Condition condition;

                        if (string.IsNullOrEmpty(partKey))
                        {
                            condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY,
                                 GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                        }
                        else
                        {
                            condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY,
                                 GlobalEnums.DatabaseCompareOperator.Equal, partKey);
                        }

                        conditions.Add(condition);

                        if (string.IsNullOrEmpty(editor))
                        {
                            condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR,
                                 GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                        }
                        else
                        {
                            condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR,
                                 GlobalEnums.DatabaseCompareOperator.Equal, editor);
                        }

                        conditions.Add(condition);

                        if (string.IsNullOrEmpty(editTime))
                        {
                            condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME,
                                 GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                        }
                        else
                        {
                            condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME,
                                 GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                        }

                        conditions.Add(condition);

                        List<Conditions> conditionsList = new List<Conditions>() { conditions };

                        #endregion

                        List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(partsFields, partsDataTable, conditionsList);

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
                                    #region Validate Part Name

                                    if (!string.IsNullOrEmpty(partName))
                                    {
                                        conditions = new Conditions();

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, GlobalEnums.DatabaseCompareOperator.Equal, partName);

                                        conditions.Add(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY, GlobalEnums.DatabaseCompareOperator.NotEqual, partKey);

                                        string returnData = AllCommonFunctions.GetSpecifyTableColumnData(partsFields, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME, conditions, transaction);

                                        if (!string.IsNullOrEmpty(returnData))
                                        {
                                            throw new Exception("备件名称已存在!");
                                        }
                                    }

                                    #endregion

                                    #region Update Part Data

                                    if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                    {
                                        editTime = AllCommonFunctions.GetSpecifyTableColumnData(partsFields, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY,
                                            partKey, transaction);
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

                        LogService.LogError("UpdatePart Error: " + ex.Message);
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

        public DataSet DeletePart(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDITOR) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_EDIT_TIME))
            {
                string partKey = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                string editor = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDITOR].ToString();
                string editTime = reqDS.ExtendedProperties[PARAMETERS.INPUT_EDIT_TIME].ToString();

                try
                {
                    EMS_EQUIPMENT_PARTS_FIELDS partsFields = new EMS_EQUIPMENT_PARTS_FIELDS();

                    #region Build Delete SQL Conditions

                    Conditions conditions = new Conditions();

                    Condition condition;

                    if (string.IsNullOrEmpty(partKey))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY,
                             GlobalEnums.DatabaseCompareOperator.Equal, partKey);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editor))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR,
                             GlobalEnums.DatabaseCompareOperator.Equal, editor);
                    }

                    conditions.Add(condition);

                    if (string.IsNullOrEmpty(editTime))
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Null, string.Empty);
                    }
                    else
                    {
                        condition = new Condition(GlobalEnums.DatabaseLogicOperator.And, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME,
                             GlobalEnums.DatabaseCompareOperator.Equal, editTime);
                    }

                    conditions.Add(condition);

                    List<Conditions> conditionsList = new List<Conditions>() { conditions };

                    #endregion

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(partsFields, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0)
                    {
                        sqlString = sqlStringList[0];
                    }

                    #region Validate Equipment Task Reference



                    #endregion

                    #region Delete Part Data

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

                    LogService.LogError("DeletePart Error: " + ex.Message);
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
