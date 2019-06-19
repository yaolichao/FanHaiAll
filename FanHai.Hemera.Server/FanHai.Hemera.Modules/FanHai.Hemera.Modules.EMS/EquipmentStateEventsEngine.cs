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
    /// 设备状态事件登记数据的管理类。
    /// </summary>
    public class EquipmentStateEventsEngine : AbstractEngine, IEquipmentStateEvents
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentStateEventsEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据设备主键获取指定设备及其状态事件登记数据。。
        /// </summary>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>包含设备及其状态事件登记数据的数据集对象。</returns>
        public DataSet GetCurrentEquipment(string equipmentKey)
        {
            DataSet dsReturn = new DataSet();
            string[] strArr = new string[3];
            string[] strName = new string[3];
            try
            {
                //选择的当前设备
                strArr[0] = string.Format(@"SELECT T.EQUIPMENT_CODE,T.EQUIPMENT_NAME,T.EQUIPMENT_KEY,T1.EQUIPMENT_GROUP_NAME,
                                                   T2.EQUIPMENT_STATE_NAME,T2.EQUIPMENT_STATE_TYPE, T2.DESCRIPTION
                                            FROM EMS_EQUIPMENTS T,EMS_EQUIPMENT_GROUPS T1,EMS_EQUIPMENT_STATES T2
                                            WHERE T.EQUIPMENT_GROUP_KEY = T1.EQUIPMENT_GROUP_KEY
                                            AND T.EQUIPMENT_STATE_KEY = T2.EQUIPMENT_STATE_KEY
                                            AND T.EQUIPMENT_KEY = '{0}'", 
                                            equipmentKey.PreventSQLInjection());
                strName[0] = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                //选择当前设备事件的上次事件
                strArr[1] = string.Format(@"SELECT T2.EQUIPMENT_GROUP_NAME,T.EQUIPMENT_CODE,T1.DESCRIPTION,
                                                   T3.EQUIPMENT_STATE_NAME,T5.EQUIPMENT_CHANGE_STATE_NAME,
                                                   T1.EDIT_TIME, CONVERT(VARCHAR,GETDATE(),120) CREATE_TIME
                                            FROM EMS_EQUIPMENTS T,EMS_STATE_EVENT T1,EMS_EQUIPMENT_GROUPS T2,EMS_EQUIPMENT_STATES T3,
                                               (SELECT ISNULL(MAX(B.ISCURRENT) - 1, 0) ISCURRENT
                                                FROM EMS_STATE_EVENT B
                                                WHERE B.EQUIPMENT_KEY = '{0}') T4,
                                               EMS_EQUIPMENT_CHANGE_STATES T5
                                            WHERE T.EQUIPMENT_GROUP_KEY = T2.EQUIPMENT_GROUP_KEY
                                            AND T.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                                            AND T1.ISCURRENT = T4.ISCURRENT
                                            AND T.EQUIPMENT_STATE_KEY = T3.EQUIPMENT_STATE_KEY
                                            AND T1.EQUIPMENT_CHANGE_STATE_KEY = T5.EQUIPMENT_CHANGE_STATE_KEY
                                            AND T.EQUIPMENT_KEY = '{0}'", 
                                            equipmentKey.PreventSQLInjection());
                strName[1] = EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME;
                //选择设备from_to事件
                strArr[2] = string.Format(@"SELECT DISTINCT 
                                                  T3.EQUIPMENT_STATE_NAME EQUIPMENT_STATE_NAME_FROM,T1.EQUIPMENT_STATE_NAME,T1.EQUIPMENT_STATE_TYPE,T1.DESCRIPTION,
                                                  T.EQUIPMENT_CHANGE_STATE_NAME + '(' + T3.DESCRIPTION + '->'+T1.DESCRIPTION + ')' AS EQUIPMENT_CHANGE_STATE_NAME,
                                                  T.EQUIPMENT_CHANGE_STATE_KEY,T.EQUIPMENT_FROM_STATE_KEY,T.EQUIPMENT_TO_STATE_KEY,T2.EQUIPMENT_KEY
                                            FROM EMS_EQUIPMENT_CHANGE_STATES T,
                                                 EMS_EQUIPMENT_STATES        T1,
                                                 EMS_EQUIPMENTS              T2,
                                                 EMS_EQUIPMENT_STATES        T3
                                            WHERE T.EQUIPMENT_TO_STATE_KEY = T1.EQUIPMENT_STATE_KEY
                                            AND T.EQUIPMENT_FROM_STATE_KEY = T3.EQUIPMENT_STATE_KEY
                                            AND T2.EQUIPMENT_STATE_KEY = T.EQUIPMENT_FROM_STATE_KEY
                                            AND T2.EQUIPMENT_KEY = '{0}' ",
                                            equipmentKey.PreventSQLInjection());
                strName[2] = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME;

                for (int i = 0; i < strArr.Length; i++)
                {
                    DataTable dt = db.ExecuteDataSet(CommandType.Text, strArr[i]).Tables[0];
                    dt.TableName = strName[i];
                    dsReturn.Tables.Add(dt.Copy());
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCurrentEquipment Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据设备主键和用户工号获取指定设备及其状态事件登记数据。
        /// </summary>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="userId">用户工号。</param>
        /// <returns>包含设备及其状态事件登记数据的数据集对象。</returns>
        public DataSet GetCurrentEquipment2 (string equipmentKey,string userId)
        {
            DataSet dsReturn = new DataSet();
            string[] strArr = new string[3];
            string[] strName = new string[3];
            try
            {
                //选择的当前设备
                strArr[0] = string.Format(@"SELECT T.EQUIPMENT_CODE,T.EQUIPMENT_NAME,T.EQUIPMENT_KEY,T1.EQUIPMENT_GROUP_NAME,
                                               T2.EQUIPMENT_STATE_NAME,T2.EQUIPMENT_STATE_TYPE,T2.DESCRIPTION
                                            FROM EMS_EQUIPMENTS T,EMS_EQUIPMENT_GROUPS T1,EMS_EQUIPMENT_STATES T2
                                            WHERE T.EQUIPMENT_GROUP_KEY = T1.EQUIPMENT_GROUP_KEY
                                            AND T.EQUIPMENT_STATE_KEY = T2.EQUIPMENT_STATE_KEY
                                            AND T.EQUIPMENT_KEY = '{0}'", 
                                            equipmentKey.PreventSQLInjection());
                strName[0] = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                //选择当前设备事件的上次事件
                strArr[1] = string.Format(@"SELECT T2.EQUIPMENT_GROUP_NAME,T.EQUIPMENT_CODE,T1.DESCRIPTION,T3.EQUIPMENT_STATE_NAME,
                                                   T5.EQUIPMENT_CHANGE_STATE_NAME,T1.EDIT_TIME,CONVERT(VARCHAR,T1.CREATE_TIME,120) CREATE_TIME
                                            FROM EMS_EQUIPMENTS T,
                                               EMS_STATE_EVENT T1,
                                               EMS_EQUIPMENT_GROUPS T2,
                                               EMS_EQUIPMENT_STATES T3,
                                               (SELECT ISNULL(MAX(B.ISCURRENT) - 1, 0) ISCURRENT
                                                FROM EMS_STATE_EVENT B
                                                WHERE B.EQUIPMENT_KEY = '{0}') T4,
                                               EMS_EQUIPMENT_CHANGE_STATES T5
                                            WHERE T.EQUIPMENT_GROUP_KEY = T2.EQUIPMENT_GROUP_KEY
                                            AND T.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                                            AND T1.ISCURRENT = T4.ISCURRENT
                                            AND T.EQUIPMENT_STATE_KEY = T3.EQUIPMENT_STATE_KEY
                                            AND T1.EQUIPMENT_CHANGE_STATE_KEY = T5.EQUIPMENT_CHANGE_STATE_KEY
                                            AND T.EQUIPMENT_KEY = '{0}'",
                                            equipmentKey.PreventSQLInjection());
                strName[1] = EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME;
                //选择设备from_to事件
                strArr[2] = string.Format(@"SELECT DISTINCT T3.EQUIPMENT_STATE_NAME EQUIPMENT_STATE_NAME_FROM,
                                                      T1.EQUIPMENT_STATE_NAME,
                                                      T1.EQUIPMENT_STATE_TYPE,
                                                      T1.DESCRIPTION,
                                                      T.EQUIPMENT_CHANGE_STATE_NAME + '(' + T3.DESCRIPTION + '->'+T1.DESCRIPTION+ ')' AS EQUIPMENT_CHANGE_STATE_NAME,
                                                      T.EQUIPMENT_CHANGE_STATE_KEY,
                                                      T.EQUIPMENT_FROM_STATE_KEY,
                                                      T.EQUIPMENT_TO_STATE_KEY,
                                                      T2.EQUIPMENT_KEY
                                        FROM EMS_EQUIPMENT_CHANGE_STATES T,
                                             EMS_EQUIPMENT_STATES        T1,
                                             EMS_EQUIPMENTS              T2,
                                             EMS_EQUIPMENT_STATES        T3
                                        WHERE T.EQUIPMENT_TO_STATE_KEY = T1.EQUIPMENT_STATE_KEY
                                         AND T.EQUIPMENT_FROM_STATE_KEY = T3.EQUIPMENT_STATE_KEY
                                         AND T2.EQUIPMENT_STATE_KEY = T.EQUIPMENT_FROM_STATE_KEY 
                                         AND T3.EQUIPMENT_STATE_NAME IN (SELECT EQUIPMENT_STATE_NAME
                                                                         FROM rbac_role_own_status
                                                                         WHERE role_key IN (SELECT role_key
                                                                                             FROM rbac_user_in_role
                                                                                             WHERE user_key IN (SELECT user_key
                                                                                             FROM rbac_user
                                                                                             WHERE badge = '{0}')
                                                                                            )
                                                                        )
                                         AND T2.EQUIPMENT_KEY = '{1}'", 
                                         userId.PreventSQLInjection(), 
                                         equipmentKey.PreventSQLInjection());
                strName[2] = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME;

                for (int i = 0; i < strArr.Length; i++)
                {
                    DataTable dt = db.ExecuteDataSet(CommandType.Text, strArr[i]).Tables[0];
                    dt.TableName = strName[i];
                    dsReturn.Tables.Add(dt.Copy());
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCurrentEquipment Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增设备状态事件登记记录。
        /// </summary>
        /// <param name="dsParams">包含设备状态事件登记数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet InsertEquipmentStateEvents(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommand = new List<string>();
            string s = string.Empty;
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    EMS_STATE_EVENT_FIELDS ese = new EMS_STATE_EVENT_FIELDS();
                    //更新设备状态事件。
                    if (dsParams.Tables.Contains(EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME_UPDATE))
                    {
                        DataRow dr = dsParams.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME_UPDATE].Rows[0];
                        s = string.Format(@"UPDATE EMS_STATE_EVENT
                                            SET EQUIPMENT_TO_STATE_KEY     = '{0}',
                                               DESCRIPTION                = '{1}',
                                               EDIT_TIME                  = GETDATE(),
                                               EDITOR                     = '{2}',
                                               EQUIPMENT_CHANGE_STATE_KEY = '{3}'
                                            WHERE EQUIPMENT_TO_STATE_KEY IS NULL    
                                            AND EQUIPMENT_KEY ='{4}'                                
                                            AND ISCURRENT = (SELECT MAX(ISCURRENT) FROM EMS_STATE_EVENT WHERE EQUIPMENT_KEY ='{5}')",                                                                    
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_TO_STATE_KEY].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.DESCRIPTION].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.EDITOR].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_CHANGE_STATE_KEY].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString().PreventSQLInjection());
                        sqlCommand.Add(s);

                        s = string.Format(@"UPDATE EMS_EQUIPMENTS
                                            SET EQUIPMENT_CHANGE_STATE_KEY = '{0}', EQUIPMENT_STATE_KEY = '{1}'
                                            WHERE EQUIPMENT_KEY = '{2}'", 
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_CHANGE_STATE_KEY].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_TO_STATE_KEY].ToString().PreventSQLInjection(),
                                            dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString().PreventSQLInjection());
                        sqlCommand.Add(s);
                    }

                    //新增设备状态事件。
                    if (dsParams.Tables.Contains(EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME))
                    {
                        foreach (DataRow dr in dsParams.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME].Rows)
                        {
                            string s01 = string.Format(@"SELECT ISNULL(MAX(T.ISCURRENT), 0)+1 ISCURRENT
                                                         FROM EMS_STATE_EVENT T
                                                         WHERE T.EQUIPMENT_KEY = '{0}'",
                                                         dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString().PreventSQLInjection());
                            int iscurrent = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, s01));
                            s = string.Format(@"INSERT INTO EMS_STATE_EVENT(EVENT_KEY,EQUIPMENT_KEY,EQUIPMENT_FROM_STATE_KEY,ISCURRENT,CREATOR,CREATE_TIME,STATE)
                                                VALUES('{0}', '{1}', '{2}','{3}','{4}',GETDATE(),1)", 
                                                dr[EMS_STATE_EVENT_FIELDS.EVENT_KEY].ToString().PreventSQLInjection(),
                                                dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString().PreventSQLInjection(),
                                                dr[EMS_STATE_EVENT_FIELDS.EQUIPMENT_FROM_STATE_KEY].ToString().PreventSQLInjection(),
                                                iscurrent.ToString().PreventSQLInjection(),
                                                dr[EMS_STATE_EVENT_FIELDS.CREATOR].ToString().PreventSQLInjection());
                            sqlCommand.Add(s);
                        }
                    }
                    //执行数据操作。
                    foreach (string str in sqlCommand)
                    {
                        db.ExecuteNonQuery(dbTran, CommandType.Text, str);
                    }

                    //Commit Transaction
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("InsertEquipmentStateEvents Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取当前面板中设备的当前状态
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -----------------------------
        /// {EQUIPMENT_KEY}
        /// -----------------------------
        /// </param>
        /// <returns>
        /// 包含设备状态数据的数据集对象。
        /// EQUIPMENT_KEY,EQUIPMENT_NAME,EQUIPMENT_CODE,EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME,EQUIPMENT_STATE_TYPE
        /// </returns>
        public DataSet GetLayoutEquipmentCurrStates(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string keys = string.Empty;
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME))
                {
                    foreach (DataRow dr in dsParams.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME].Rows)
                    {
                        string equipmentKey = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                        keys += "'" + equipmentKey.PreventSQLInjection() + "',";
                    }
                }
                
                if (keys.Length > 0)
                    keys = keys.TrimEnd(',');
                else
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "layout中未有设备!");
                    return dsReturn;
                }

                string s = string.Format(@"SELECT T.EQUIPMENT_KEY,
	                                           T.EQUIPMENT_NAME,
	                                           T.EQUIPMENT_CODE,
	                                           T1.EQUIPMENT_STATE_KEY,
	                                           T1.EQUIPMENT_STATE_NAME,
	                                           T1.EQUIPMENT_STATE_TYPE
                                        FROM EMS_EQUIPMENTS T, EMS_EQUIPMENT_STATES T1
                                        WHERE T.EQUIPMENT_STATE_KEY = T1.EQUIPMENT_STATE_KEY
                                        AND T.EQUIPMENT_KEY IN ({0})", keys);

                DataTable dt = db.ExecuteDataSet(CommandType.Text, s).Tables[0];
                dt.TableName = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Tables.Add(dt.Copy());
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
            }
            catch (Exception ex)
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                LogService.LogError("GetInitEquipmentChangeState Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
