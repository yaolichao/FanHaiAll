using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Share.Interface.EquipmentManagement;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Data.Common;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 设备转变状态的数据管理类。
    /// </summary>
    public class EquipmentChangeStates : AbstractEngine, IEquipmentChangeStates
    {
        private Database db; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentChangeStates()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 获取设备转变状态数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备转变状态数据的数据集对象。</returns>
        public DataSet GetEquipmentChangeStates(DataSet dsParams)
        {
            DataSet resDS = new DataSet();

            try
            {
                EMS_EQUIPMENT_CHANGE_STATES_FIELDS equipmentChangeStatesFields = new EMS_EQUIPMENT_CHANGE_STATES_FIELDS();

                List<string> interestColumns = new List<string>();

                interestColumns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY);

                Conditions conditions = null;

                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
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
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DatabaseCompareOperator.Null, string.Empty);
                            }
                            else
                            {
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DatabaseCompareOperator.Equal, key.ToString());
                            }
                        }
                    }
                }

                string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentChangeStatesFields, interestColumns, conditions);

                db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("GetEquipmentChangeStates Error: " + ex.Message);
            }

            return resDS;
        }
        /// <summary>
        /// 获取指定设备状态（作为FROM状态）的可转变状态数据。
        /// </summary>
        /// <param name="equipmentChangeStateKey">设备转变状态主键。通过它查找对应的转变到的设备状态（TO状态）作为指定设备状态（FROM状态）。</param>
        /// <returns>包含设备转变状态数据的数据集对象。</returns>
        public DataSet GetEquipmentChangeStates(string equipmentChangeStateKey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT ECS.*
                                                 FROM EMS_EQUIPMENT_CHANGE_STATES ECS
                                                 WHERE ECS.EQUIPMENT_FROM_STATE_KEY =(SELECT ECS.EQUIPMENT_TO_STATE_KEY
                                                                                      FROM EMS_EQUIPMENT_CHANGE_STATES ECS
                                                                                      WHERE ECS.EQUIPMENT_CHANGE_STATE_KEY ='{0}')", 
                                                 equipmentChangeStateKey.PreventSQLInjection());

                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);

                LogService.LogError("GetEquipmentChangeStates Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新设备转变状态数据。
        /// </summary>
        /// <param name="dsParams">包含设备转变状态数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateEquipmentChangeStates(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME))
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        dbConn.Open();

                        using (DbTransaction dbTrans = dbConn.BeginTransaction())
                        {
                            try
                            {
                                DbCommand insertCommand = dbConn.CreateCommand();

                                insertCommand.CommandType = CommandType.Text;
                                insertCommand.CommandText = @"INSERT INTO EMS_EQUIPMENT_CHANGE_STATES(EQUIPMENT_CHANGE_STATE_KEY,EQUIPMENT_CHANGE_STATE_NAME,EQUIPMENT_FROM_STATE_KEY,EQUIPMENT_TO_STATE_KEY)
                                                              VALUES(@P1,@P2,@P3,@P4)";

                                db.AddInParameter(insertCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P2", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P3", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P4", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY, DataRowVersion.Current);

                                DbCommand updateCommand = dbConn.CreateCommand();

                                updateCommand.CommandType = CommandType.Text;
                                updateCommand.CommandText =@"UPDATE EMS_EQUIPMENT_CHANGE_STATES SET DESCRIPTION = @P2 
                                                             WHERE EQUIPMENT_CHANGE_STATE_KEY = @P1";

                                db.AddInParameter(updateCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DataRowVersion.Current);
                                db.AddInParameter(updateCommand, "P2", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION, DataRowVersion.Current);

                                DbCommand deleteCommand = dbConn.CreateCommand();

                                deleteCommand.CommandType = CommandType.Text;
                                deleteCommand.CommandText =@"DELETE FROM EMS_EQUIPMENT_CHANGE_STATES 
                                                             WHERE EQUIPMENT_CHANGE_STATE_KEY = @P1";

                                db.AddInParameter(deleteCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DataRowVersion.Current);

                                #region 删除对应的设备状态转变原因

                                DbCommand deleteChangeReasonsCommand = dbConn.CreateCommand();

                                deleteChangeReasonsCommand.CommandType = CommandType.Text;
                                deleteChangeReasonsCommand.CommandText = "DELETE FROM EMS_EQUIPMENT_CHANGE_REASONS WHERE EQUIPMENT_CHANGE_STATE_KEY = @P1";

                                db.AddInParameter(deleteChangeReasonsCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DataRowVersion.Current);

                                db.UpdateDataSet(dsParams.Copy(), EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME, null, null, deleteChangeReasonsCommand,  UpdateBehavior.Continue);

                                #endregion

                                if (db.UpdateDataSet(dsParams, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME, insertCommand, updateCommand, deleteCommand, dbTrans) <= 0)
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                dbTrans.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("UpdateEquipmentChangeStates Error: " + ex.Message);
                                dbTrans.Rollback();
                                throw ex;
                            }
                            finally
                            {
                                dbConn.Close();
                            }
                        }
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateEquipmentChangeStates Error: " + ex.Message);
            }

            return dsReturn;
        }
    }
}
