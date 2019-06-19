using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Constants;
using System.Data.Common;
using FanHai.Hemera.Share.Interface.EquipmentManagement;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 设备状态数据管理类。
    /// </summary>
    public class EquipmentStates : AbstractEngine, IEquipmentStates
    {
        private Database db;//数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentStates()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 查询设备状态。可以传入指定的设备状态进行查询(<see cref="EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY"/>对应的值)。
        /// </summary>
        /// <param name="dsParams">
        /// 数据集对象。包含名称为<see cref="PARAMETERS_INPUT.DATABASE_TABLE_NAME"/>的数据表。
        /// 数据表中包含列见<see cref="PARAMETERS_INPUT.FILEDS"/>。
        /// 数据表中<see cref="PARAMETERS_INPUT.FIELD_KEY"/>列用来设置查询条件的值。
        /// </param>
        /// <returns>包含设备状态信息的数据集对象。</returns>
        public DataSet GetEquipmentStates(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                EMS_EQUIPMENT_STATES_FIELDS equipmentStatesFields = new EMS_EQUIPMENT_STATES_FIELDS();

                List<string> interestColumns = new List<string>();

                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY);

                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATOR);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIME);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                interestColumns.Add(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME);

                Conditions conditions = null;
                //如果传入的参数数据集包含名称为PARAMETERS_INPUT.DATABASE_TABLE_NAME的数据表名
                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];
                    //数据表不为null，且数据表中有名称为PARAMETERS_INPUT.FIELD_KEY的列。
                    if (inputParamDataTable != null && inputParamDataTable.Columns.Contains(PARAMETERS_INPUT.FIELD_KEY))
                    {
                        conditions = new Conditions();

                        foreach (DataRow row in inputParamDataTable.Rows)
                        {
                            object key = row[PARAMETERS_INPUT.FIELD_KEY];
                            //如果PARAMETERS_INPUT.FIELD_KEY的值为null，设置查询条件
                            if (key == null || key == DBNull.Value)
                            {
                                conditions.Add(DatabaseLogicOperator.And, 
                                    EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                    DatabaseCompareOperator.Null, 
                                    string.Empty);
                            }
                            else
                            {//如果PARAMETERS_INPUT.FIELD_KEY的值不为null，设置查询条件
                                conditions.Add(DatabaseLogicOperator.And, 
                                    EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY, 
                                    DatabaseCompareOperator.Equal,
                                    key.ToString());
                            }
                        }
                    }
                }
                //创建查询语句
                string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentStatesFields, interestColumns, conditions);
                //执行查询语句
                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, 
                    new string[] { EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipmentStates Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 新增设备状态。
        /// </summary>
        /// <param name="dsParams">包含设备状态的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet InsertEquipmentStates(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null 
                    && dsParams.Tables.Contains(EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME) 
                    && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {//判定数据集是否存在表EMS_EQUIPMENT_STATES，INPUT_PARAM_TABLE
                    DataTable equipmentStateDataTable = dsParams.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME];
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    EMS_EQUIPMENT_STATES_FIELDS equipmentStatesFields = new EMS_EQUIPMENT_STATES_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(equipmentStatesFields, equipmentStateDataTable);

                    string equipmentStateKey = string.Empty;
                    string equipmentStateName = string.Empty;
                    string sqlString = string.Empty;
                    string createTime = string.Empty;

                    if (sqlStringList.Count > 0 && inputParamDataTable.Rows.Count > 0 && equipmentStateDataTable.Rows.Count > 0)
                    {
                        equipmentStateKey = Convert.ToString(inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY]);
                        equipmentStateName = Convert.ToString(equipmentStateDataTable.Rows[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME]);
                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        dbConn.Open();

                        using (DbTransaction dbTrans = dbConn.BeginTransaction())
                        {
                            try
                            {
                                #region 检查设备状态名称。
                                string returnData = AllCommonFunctions.GetSpecifyTableColumnData(equipmentStatesFields,
                                                                                                EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME,
                                                                                                EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME,
                                                                                                equipmentStateName,
                                                                                                dbTrans);
                                if (!string.IsNullOrEmpty(returnData))
                                {
                                    throw new Exception("${res:FanHai.Hemera.Modules.EMS.EquipmentStates.M0001}");
                                }
                                #endregion

                                if (db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString) > 0)
                                {
                                    createTime = AllCommonFunctions.GetSpecifyTableColumnData(equipmentStatesFields,
                                                                                EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIME,
                                                                                EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                                                                equipmentStateKey,
                                                                                dbTrans);
                                }
                                else
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                dbTrans.Commit();
                            }
                            catch (Exception ex)
                            {
                                LogService.LogError("InsertEquipmentStates Error: " + ex.Message);
                                dbTrans.Rollback();
                                throw;
                            }
                            finally
                            {
                                dbConn.Close();
                            }
                        }
                    }
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty, createTime);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("InsertEquipmentStates Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 更新设备状态数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含设备状态数据的数据集对象。
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateEquipmentStates(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null
                    && dsParams.Tables.Contains(EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME)
                    && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable equipmentStateDataTable = dsParams.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME];
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    #region Build Conditions List

                    List<Conditions> conditionsList = new List<Conditions>();
                    foreach(DataRow row in inputParamDataTable.Rows)
                    {
                        Conditions conditions = new Conditions();
                        Condition keyCondition;
                        if (row[PARAMETERS_INPUT.FIELD_KEY] != null && row[PARAMETERS_INPUT.FIELD_KEY] != DBNull.Value)
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, 
                                                        EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                                        DatabaseCompareOperator.Equal,
                                                        row[PARAMETERS_INPUT.FIELD_KEY].ToString());
                        }
                        else
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, 
                                                        EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                                        DatabaseCompareOperator.Null, 
                                                        string.Empty);
                        }

                        conditions.Add(keyCondition);

                        Condition editorCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDITOR] != null && row[PARAMETERS_INPUT.FIELD_EDITOR] != DBNull.Value)
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDITOR].ToString());
                        }
                        else
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editorCondition);

                        Condition editTimeCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != null && row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != DBNull.Value)
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDIT_TIME].ToString());
                        }
                        else
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editTimeCondition);

                        conditionsList.Add(conditions);
                    }

                    #endregion

                    EMS_EQUIPMENT_STATES_FIELDS equipmentStatesFields = new EMS_EQUIPMENT_STATES_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(equipmentStatesFields, 
                                                                                        equipmentStateDataTable, 
                                                                                        conditionsList);

                    string equipmentStateKey = string.Empty;
                    string equipmentStateName = string.Empty;
                    string sqlString = string.Empty;
                    string editTime = string.Empty;

                    if (sqlStringList.Count > 0 && inputParamDataTable.Rows.Count > 0 && equipmentStateDataTable.Rows.Count > 0)
                    {
                        equipmentStateKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                        equipmentStateName = equipmentStateDataTable.Rows[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Check Equipment State Name

                                if (!string.IsNullOrEmpty(equipmentStateName))
                                {
                                    Conditions conditions = new Conditions();

                                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME, DatabaseCompareOperator.Equal, equipmentStateName);

                                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY, DatabaseCompareOperator.NotEqual, equipmentStateKey);

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(equipmentStatesFields, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME, conditions, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("${res:FanHai.Hemera.Modules.EMS.EquipmentStates.M0001}");
                                    }
                                }

                                #endregion

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    editTime = AllCommonFunctions.GetSpecifyTableColumnData(equipmentStatesFields, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                        equipmentStateKey, transaction);
                                }
                                else
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                transaction.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("UpdateEquipmentStates Error: " + ex.Message);
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty, editTime);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateEquipmentStates Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 删除设备状态。
        /// </summary>
        /// <param name="dsParams">包含删除条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteEquipmentStates(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    #region Build Conditions List

                    List<Conditions> conditionsList = new List<Conditions>();

                    foreach (DataRow row in inputParamDataTable.Rows)
                    {
                        Conditions conditions = new Conditions();

                        Condition keyCondition;

                        if (row[PARAMETERS_INPUT.FIELD_KEY] != null && row[PARAMETERS_INPUT.FIELD_KEY] != DBNull.Value)
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_KEY].ToString());
                        }
                        else
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(keyCondition);

                        Condition editorCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDITOR] != null && row[PARAMETERS_INPUT.FIELD_EDITOR] != DBNull.Value)
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDITOR].ToString());
                        }
                        else
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editorCondition);

                        Condition editTimeCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != null && row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != DBNull.Value)
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDIT_TIME].ToString());
                        }
                        else
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editTimeCondition);

                        conditionsList.Add(conditions);
                    }

                    #endregion

                    EMS_EQUIPMENT_STATES_FIELDS equipmentStatesFields = new EMS_EQUIPMENT_STATES_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(equipmentStatesFields, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0)
                    {
                        sqlString = sqlStringList[0];
                    }

                    #region Check Equipment Change States Reference

                    string equipmentStateKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                    Conditions checkConditions = new Conditions();

                    checkConditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY, DatabaseCompareOperator.Equal, equipmentStateKey);

                    checkConditions.Add(DatabaseLogicOperator.Or, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY, DatabaseCompareOperator.Equal, equipmentStateKey);

                    EMS_EQUIPMENT_CHANGE_STATES_FIELDS equipmentChangeStatesFields = new EMS_EQUIPMENT_CHANGE_STATES_FIELDS();

                    List<string> interestColumns = new List<string>() { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY };

                    string checkSqlString = DatabaseTable.BuildQuerySqlStatement(equipmentChangeStatesFields, interestColumns, checkConditions);

                    object scalar = db.ExecuteScalar(CommandType.Text, checkSqlString);

                    if (scalar != null && scalar != DBNull.Value)
                    {
                        throw new Exception("${res:FanHai.Hemera.Modules.EMS.EquipmentStates.M0002}");
                    }

                    #endregion

                    if (db.ExecuteNonQuery(CommandType.Text, sqlString) > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                    else
                    {
                        throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteEquipmentStates Error: " + ex.Message);
            }

            return dsReturn;
        }
    }
}
