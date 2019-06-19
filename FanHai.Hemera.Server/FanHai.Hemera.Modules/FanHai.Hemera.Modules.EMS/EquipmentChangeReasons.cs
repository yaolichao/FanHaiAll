using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface.EquipmentManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Constants;
using System.Data.Common;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 设备状态转变原因的数据管理类。
    /// </summary>
    public class EquipmentChangeReasons : AbstractEngine, IEquipmentChangeReasons
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentChangeReasons()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 获取设备状态转变原因。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备状态转变原因的数据集对象。</returns>
        public DataSet GetEquipmentChangeReasons(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                EMS_EQUIPMENT_CHANGE_REASONS_FIELDS equipmentChangeReasonsFields = new EMS_EQUIPMENT_CHANGE_REASONS_FIELDS();

                List<string> interestColumns = new List<string>();

                interestColumns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_DESCRIPTION);
                interestColumns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);

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
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DatabaseCompareOperator.Null, string.Empty);
                            }
                            else
                            {
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DatabaseCompareOperator.Equal, key.ToString());
                            }
                        }
                    }
                }

                string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentChangeReasonsFields, interestColumns, conditions);

                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipmentChangeReasons Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 更新设备状态转变原因。
        /// </summary>
        /// <param name="dsParams">包含设备状态转变原因的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateEquipmentChangeReasons(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME))
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
                                insertCommand.CommandText = @"INSERT INTO EMS_EQUIPMENT_CHANGE_REASONS(EQUIPMENT_CHANGE_REASON_KEY,EQUIPMENT_CHANGE_REASON_NAME,DESCRIPTION,EQUIPMENT_CHANGE_STATE_KEY)
                                                              VALUES(@P1,@P2,@P3,@P4)";

                                db.AddInParameter(insertCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P2", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P3", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_DESCRIPTION, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P4", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, DataRowVersion.Current);

                                DbCommand updateCommand = dbConn.CreateCommand();

                                updateCommand.CommandType = CommandType.Text;
                                updateCommand.CommandText = @"UPDATE EMS_EQUIPMENT_CHANGE_REASONS 
                                                              SET EQUIPMENT_CHANGE_REASON_NAME = @P2, DESCRIPTION = @P3 
                                                              WHERE EQUIPMENT_CHANGE_REASON_KEY = @P1";

                                db.AddInParameter(updateCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, DataRowVersion.Current);
                                db.AddInParameter(updateCommand, "P2", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME, DataRowVersion.Current);
                                db.AddInParameter(updateCommand, "P3", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_DESCRIPTION, DataRowVersion.Current);

                                DbCommand deleteCommand = dbConn.CreateCommand();

                                deleteCommand.CommandType = CommandType.Text;
                                deleteCommand.CommandText = @"DELETE FROM EMS_EQUIPMENT_CHANGE_REASONS WHERE EQUIPMENT_CHANGE_REASON_KEY = @P1";

                                db.AddInParameter(deleteCommand, "P1", DbType.String, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, DataRowVersion.Current);

                                if (db.UpdateDataSet(dsParams, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME, insertCommand, updateCommand, deleteCommand, dbTrans) <= 0)
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                dbTrans.Commit();
                            }
                            catch(Exception ex)
                            {
                                dbTrans.Rollback();
                                LogService.LogError("UpdateEquipmentChangeReasons Error: " + ex.Message);
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
                LogService.LogError("UpdateEquipmentChangeReasons Error: " + ex.Message);
            }
            return dsReturn;
        }

    }
}
