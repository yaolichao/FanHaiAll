using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Interface.EquipmentManagement;
using FanHai.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 设备组数据管理类。
    /// </summary>
    public class EquipmentGroups : AbstractEngine, IEquipmentGroups
    {
        private Database db;//数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentGroups()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 获取设备组信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备组信息的数据集对象。</returns>
        public DataSet GetEquipmentGroups(DataSet dsParams)
        {
            DataSet resDS = new DataSet();

            try
            {
                EMS_EQUIPMENT_GROUPS_FIELDS equipmentGroupsFields = new EMS_EQUIPMENT_GROUPS_FIELDS();
                //创建列表存放设备组的字段信息
                List<string> interestColumns = new List<string>();

                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY);     //EQUIPMENT_GROUP_KEY
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME);    //EQUIPMENT_GROUP_NAME
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC);                    //SPEC
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION);             //DESCRIPTION

                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATOR);                 //CREATOR
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);     //CREATE_TIMEZONE_KEY
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIME);             //CREATE_TIME
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR);                  //EDITOR
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);       //EDIT_TIMEZONE_KEY
                interestColumns.Add(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME);               //FIELD_EDIT_TIME

                Conditions conditions = null;

                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))    //INPUT_PARAM_TABLE
                {
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    if (inputParamDataTable != null && inputParamDataTable.Columns.Contains(PARAMETERS_INPUT.FIELD_KEY))
                    {
                        conditions = new Conditions();

                        foreach (DataRow row in inputParamDataTable.Rows)
                        {
                            object key = row[PARAMETERS_INPUT.FIELD_KEY];

                            if (key == null || key == DBNull.Value)
                            {
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, DatabaseCompareOperator.Null, string.Empty);
                            }
                            else
                            {
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, DatabaseCompareOperator.Equal, key.ToString());
                            }
                        }
                    }
                }

                string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentGroupsFields, interestColumns, conditions);

                db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("GetEquipmentGroups Error: " + ex.Message);
            }

            return resDS;
        }

        /// <summary>
        /// 新增设备组。
        /// </summary>
        /// <param name="dsParams">包含设备组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet InsertEquipmentGroups(DataSet dsParams)
        {
            DataSet resDS = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME) && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable equipmentGroupsDataTable = dsParams.Tables[EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME];
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    EMS_EQUIPMENT_GROUPS_FIELDS equipmentGroupsFields = new EMS_EQUIPMENT_GROUPS_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(equipmentGroupsFields, equipmentGroupsDataTable);

                    string equipmentGroupKey = string.Empty;
                    string equipmentGroupName = string.Empty;
                    string sqlString = string.Empty;
                    string createTime = string.Empty;

                    if (sqlStringList.Count > 0 && inputParamDataTable.Rows.Count > 0 && equipmentGroupsDataTable.Rows.Count > 0)
                    {
                        equipmentGroupKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                        equipmentGroupName = equipmentGroupsDataTable.Rows[0][EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Check Equipment Group Name

                                string returnData = AllCommonFunctions.GetSpecifyTableColumnData(equipmentGroupsFields, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, equipmentGroupName, transaction);

                                if (!string.IsNullOrEmpty(returnData))
                                {
                                    throw new Exception("${res:FanHai.Hemera.Modules.EMS.EquipmentGroups.M0001}");
                                }

                                #endregion

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    createTime = AllCommonFunctions.GetSpecifyTableColumnData(equipmentGroupsFields, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIME, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY,
                                        equipmentGroupKey, transaction);
                                }
                                else
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                transaction.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("InsertEquipmentGroups Error: " + ex.Message);
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty, createTime);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("InsertEquipmentGroups Error: " + ex.Message);
            }

            return resDS;
        }

        /// <summary>
        /// 更新设备组数据。
        /// </summary>
        /// <param name="dsParams">包含设备组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateEquipmentGroups(DataSet dsParams)
        {
            DataSet resDS = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME) && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable equipmentGroupsDataTable = dsParams.Tables[EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME];
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    #region Build Conditions List

                    List<Conditions> conditionsList = new List<Conditions>();

                    foreach (DataRow row in inputParamDataTable.Rows)
                    {
                        Conditions conditions = new Conditions();

                        Condition keyCondition;

                        if (row[PARAMETERS_INPUT.FIELD_KEY] != null && row[PARAMETERS_INPUT.FIELD_KEY] != DBNull.Value)
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_KEY].ToString());
                        }
                        else
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(keyCondition);

                        Condition editorCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDITOR] != null && row[PARAMETERS_INPUT.FIELD_EDITOR] != DBNull.Value)
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDITOR].ToString());
                        }
                        else
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editorCondition);

                        Condition editTimeCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != null && row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != DBNull.Value)
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDIT_TIME].ToString());
                        }
                        else
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editTimeCondition);

                        conditionsList.Add(conditions);
                    }

                    #endregion

                    EMS_EQUIPMENT_GROUPS_FIELDS equipmentGroupsFields = new EMS_EQUIPMENT_GROUPS_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(equipmentGroupsFields, equipmentGroupsDataTable, conditionsList);

                    string equipmentGroupKey = string.Empty;
                    string equipmentGroupName = string.Empty;
                    string sqlString = string.Empty;
                    string editTime = string.Empty;

                    if (sqlStringList.Count > 0 && inputParamDataTable.Rows.Count > 0 && equipmentGroupsDataTable.Rows.Count > 0)
                    {
                        equipmentGroupKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                        equipmentGroupName = equipmentGroupsDataTable.Rows[0][EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Check Equipment Group Name

                                if (!string.IsNullOrEmpty(equipmentGroupName))
                                {
                                    Conditions conditions = new Conditions();

                                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, DatabaseCompareOperator.Equal, equipmentGroupName);

                                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, DatabaseCompareOperator.NotEqual, equipmentGroupKey);

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(equipmentGroupsFields, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, conditions, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("${res:FanHai.Hemera.Modules.EMS.EquipmentGroups.M0001}");
                                    }
                                }

                                #endregion

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    editTime = AllCommonFunctions.GetSpecifyTableColumnData(equipmentGroupsFields, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY,
                                        equipmentGroupKey, transaction);
                                }
                                else
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                transaction.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("UpdateEquipmentGroups Error: " + ex.Message);
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty, editTime);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("UpdateEquipmentGroups Error: " + ex.Message);
            }

            return resDS;
        }

        /// <summary>
        /// 删除设备组数据。
        /// </summary>
        /// <param name="dsParams">包含删除条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteEquipmentGroups(DataSet reqDS)
        {
            DataSet resDS = new DataSet();

            try
            {
                if (reqDS != null && reqDS.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable inputParamDataTable = reqDS.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    #region Build Conditions List

                    List<Conditions> conditionsList = new List<Conditions>();

                    foreach (DataRow row in inputParamDataTable.Rows)
                    {
                        Conditions conditions = new Conditions();

                        Condition keyCondition;

                        if (row[PARAMETERS_INPUT.FIELD_KEY] != null && row[PARAMETERS_INPUT.FIELD_KEY] != DBNull.Value)
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_KEY].ToString());
                        }
                        else
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(keyCondition);

                        Condition editorCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDITOR] != null && row[PARAMETERS_INPUT.FIELD_EDITOR] != DBNull.Value)
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDITOR].ToString());
                        }
                        else
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editorCondition);

                        Condition editTimeCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != null && row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != DBNull.Value)
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDIT_TIME].ToString());
                        }
                        else
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editTimeCondition);

                        conditionsList.Add(conditions);
                    }

                    #endregion

                    EMS_EQUIPMENT_GROUPS_FIELDS equipmentGroupsFields = new EMS_EQUIPMENT_GROUPS_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(equipmentGroupsFields, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0)
                    {
                        sqlString = sqlStringList[0];
                    }

                    #region Check Equipments Reference

                    string equipmentGroupKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                    Conditions checkConditions = new Conditions();

                    checkConditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, DatabaseCompareOperator.Equal, equipmentGroupKey);

                    EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

                    List<string> interestColumns = new List<string>() { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY };

                    string checkSqlString = DatabaseTable.BuildQuerySqlStatement(equipmentsFields, interestColumns, checkConditions);

                    object scalar = db.ExecuteScalar(CommandType.Text, checkSqlString);

                    if (scalar != null && scalar != DBNull.Value)
                    {
                        throw new Exception("${res:FanHai.Hemera.Modules.EMS.EquipmentGroups.M0002}");
                    }

                    #endregion

                    if (db.ExecuteNonQuery(CommandType.Text, sqlString) > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
                    }
                    else
                    {
                        throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("DeleteEquipmentGroups Error: " + ex.Message);
            }

            return resDS;
        }

    }
}
