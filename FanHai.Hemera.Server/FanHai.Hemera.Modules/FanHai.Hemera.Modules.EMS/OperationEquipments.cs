using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Hemera.Share.Interface.EquipmentManagement;
using FanHai.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Constants;
using System.Data.Common;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 工序设备数据管理类。
    /// </summary>
    public class OperationEquipments : AbstractEngine, IOperationEquipments
    {
        private Database db; //数据库操作对象。

        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationEquipments()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
            
        }
        /// <summary>
        /// 根据工序设备主键获取工序和设备的关联关系。
        /// </summary>
        /// <param name="operationKey">工序设备主键。</param>
        /// <returns>包含工序和设备的关联关系的数据集对象。</returns>
        public DataSet GetOperationEquipments(string operationKey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT E.*, OE.OPERATION_EQUIPMENT_KEY, OE.OPERATION_KEY
                                                   FROM EMS_EQUIPMENTS E, EMS_OPERATION_EQUIPMENT OE
                                                   WHERE E.EQUIPMENT_KEY = OE.EQUIPMENT_KEY AND OE.OPERATION_KEY = '{0}'", 
                                                   operationKey.PreventSQLInjection());

                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetOperationEquipments Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取工序下指定线别的设备信息
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        /// <param name="operateLineName">线别名称。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        public DataSet GetOperationEquipment(string operationKey, string operateLineName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlString = string.Format(@"SELECT * FROM V_EQUIPMENT t 
                                                WHERE t.LINE_NAME LIKE '{0}%' 
                                                AND t.OPERATION_KEY='{1}'
                                                ORDER BY LINE_CODE", 
                                                operateLineName.PreventSQLInjection(), 
                                                operationKey.PreventSQLInjection());
                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME });
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetOperationEquipment Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 根据工步主键和设备主键获取设备所在线别名称。
        /// </summary>
        /// <param name="stepKey">工步主键</param>
        /// <param name="equipmentKey">设备主键</param>
        /// <returns>线别名称。</returns>
        public string GetEquipmentLine(string stepKey, string equipmentKey)
        {
            string line = string.Empty;
            try
            {
                string sql = string.Format(@"SELECT t.LINE_NAME
                                            FROM V_EQUIPMENT t,POR_ROUTE_STEP s
                                            WHERE t.OPERATION_NAME=s.ROUTE_STEP_NAME
                                            AND t.EQUIPMENT_KEY = '{0}'
                                            AND s.ROUTE_STEP_KEY='{1}'", 
                                            equipmentKey.PreventSQLInjection(),
                                            stepKey.PreventSQLInjection());
                DataSet dsLines = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsLines != null && dsLines.Tables.Count > 0 && dsLines.Tables[0].Rows.Count > 0)
                {
                    line=dsLines.Tables[0].Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return line;
        }

        /// <summary>
        /// 根据批次主键和工步主键获取批次设备信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>
        /// 包含批次加工设备信息的数据集对象。
        /// [EQUIPMENT_KEY,EQUIPMENT_NAME,EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME]
        /// </returns>
        public DataSet GetLotEquipment(string lotKey, string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlString = string.Format(@"SELECT B.EQUIPMENT_KEY,A.EQUIPMENT_NAME,A.EQUIPMENT_STATE_KEY,C.EQUIPMENT_STATE_NAME
                                                FROM EMS_LOT_EQUIPMENT B
                                                INNER JOIN EMS_EQUIPMENTS A ON A.EQUIPMENT_KEY = B.EQUIPMENT_KEY
                                                LEFT JOIN EMS_EQUIPMENT_STATES C ON A.EQUIPMENT_STATE_KEY=C.EQUIPMENT_STATE_KEY 
                                                WHERE B.STEP_KEY = '{1}'
                                                AND B.LOT_KEY='{0}'
                                                AND B.END_TIMESTAMP IS NULL",
                                                lotKey.PreventSQLInjection(),
                                                stepKey.PreventSQLInjection());
                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, 
                               new string[] { EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetLotEquipment Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
       
        /// <summary>
        /// 更新工序设备数据。
        /// </summary>
        /// <param name="dsParams">包含工序设备数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateOperationEquipments(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME))
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
                                insertCommand.CommandText = "INSERT INTO EMS_OPERATION_EQUIPMENT(OPERATION_EQUIPMENT_KEY,OPERATION_KEY,EQUIPMENT_KEY) VALUES(@P1,@P2,@P3)";

                                db.AddInParameter(insertCommand, "P1", DbType.String, EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_OPERATION_EQUIPMENT_KEY, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P2", DbType.String, EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_OPERATION_KEY, DataRowVersion.Current);
                                db.AddInParameter(insertCommand, "P3", DbType.String, EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_EQUIPMENT_KEY, DataRowVersion.Current);

                                DbCommand updateCommand = null;

                                DbCommand deleteCommand = dbConn.CreateCommand();

                                deleteCommand.CommandType = CommandType.Text;
                                deleteCommand.CommandText = "DELETE FROM EMS_OPERATION_EQUIPMENT WHERE OPERATION_EQUIPMENT_KEY = @P1";

                                db.AddInParameter(deleteCommand, "P1", DbType.String, EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_OPERATION_EQUIPMENT_KEY, DataRowVersion.Current);

                                if (db.UpdateDataSet(dsParams, EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME, insertCommand, updateCommand, deleteCommand, dbTrans) <= 0)
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                dbTrans.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("UpdateOperationEquipments Error: " + ex.Message);
                                dbTrans.Rollback();

                                throw;
                            }
                            finally
                            {
                                dbTrans.Dispose();
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
                LogService.LogError("UpdateOperationEquipments Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
    }
}
