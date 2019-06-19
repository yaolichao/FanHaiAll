using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Modules.EMS;

namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含在制品静态方法的类。
    /// </summary>
    public partial class WipManagement
    {
//        /// <summary>
//        /// 根据父批号获取拆分后批号。
//        /// </summary>
//        /// <param name="db">数据库对象。</param>
//        /// <param name="strParentLotNumber">父批号。</param>
//        /// <param name="index"></param>
//        /// <returns>拆分后的子批号。</returns>
//        private static string GetChildLotNumber(Database db, DbTransaction dbTrans, string strParentLotNumber, int index)
//        {
//            string strChildLotNumber = string.Empty;
//            string strMaxNum = string.Empty;
//            string sql = string.Empty;
//            string strPrepositive = strParentLotNumber;
//            sql = string.Format(@"SELECT ISNULL(MAX(SUBSTRING(A.LOT_NUMBER, LEN('{0}')+2,2)),0) AS MAX_NUM 
//                                FROM POR_LOT A 
//                                WHERE A.LOT_NUMBER LIKE '{0}%'",
//                                strPrepositive.PreventSQLInjection());
//            object objMaxNum = db.ExecuteScalar(dbTrans, CommandType.Text, sql);
//            int maxNum = 0;
//            if (objMaxNum == null || objMaxNum == DBNull.Value)
//            {
//                maxNum = Convert.ToInt32(maxNum);
//            }
//            maxNum = maxNum + 1;
//            strMaxNum = maxNum.ToString().PadLeft(2, '0');
//            strChildLotNumber = strPrepositive + "-" + strMaxNum;
//            return strChildLotNumber;
//        }
        /// <summary>
        /// 记录批次自动进站或自动出站错误消息。
        /// </summary>
        /// <param name="title">消息标题。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="toUser">消息到达用户。</param>
        /// <param name="group">消息到达角色。</param>
        /// <param name="user">发送消息用户。</param>
        /// <param name="timeZone">时区。</param>
        /// <param name="objectKey">消息类型的对象主键。</param>
        /// <param name="objectType">消息类型</param>
        internal static void RecordErrorMessage(Database db,
                                        string title, string message, string toUser, string group,
                                        string user, string timeZone, string objectKey, string objectType)
        {
            WIP_MESSAGE_FIELDS wef = new WIP_MESSAGE_FIELDS();
            string rowKey = string.Empty;
            string sql = string.Empty;

            Hashtable hsTable = new Hashtable();
            rowKey = UtilHelper.GenerateNewKey(0);

            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_ROW_KEY, rowKey);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_TITLE, title);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CONTEXT, message);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CRITICAL_LEVEL, "0");
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_PUBLIC_LEVEL, "0");
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_TO_USER, toUser);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_TO_GROUP, group);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_STATUS, "0");
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CREATOR, user);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CREATE_TIME, null);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CREATE_TIMEZONE, timeZone);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_OBJECTKEY, objectKey);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_OBJECTTYPE, objectType);

            sql = DatabaseTable.BuildInsertSqlStatement(wef, hsTable, null);
            db.ExecuteNonQuery(CommandType.Text, sql);
        }
        /// <summary>
        /// 批次进站时更新设备信息。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbTrans">数据库操作事务对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="lotCurrentQuantity">批次当前数量。</param>
        /// <param name="operationKey">工序主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="userName">用户。</param>
        /// <returns>true：更新设备信息成功。false：更新设备信息失败。</returns>
        internal static bool TrackInForEquipment(Database db, DbTransaction dbTrans, 
                                                string lotKey, 
                                                double lotCurrentQuantity, 
                                                string operationKey,
                                                string stepKey, 
                                                string equipmentKey, 
                                                string userName)
        {
            const string RUN_STATE_NAME="RUN";
            string lostStateKey=string.Empty;
            string runStateKey=string.Empty;
            string lostToRunChangeStateKey=string.Empty;
            int n = 0;

            if (db != null && dbTrans != null)
            {
                string sqlString = string.Empty;                
                //获取设备当前状态的主键。
                sqlString = string.Format("SELECT EQUIPMENT_STATE_KEY FROM EMS_EQUIPMENTS WHERE EQUIPMENT_KEY='{0}'", equipmentKey.PreventSQLInjection());
                lostStateKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlString));
                //获取设备RUN的主键。
                sqlString=string.Format("SELECT EQUIPMENT_STATE_KEY FROM EMS_EQUIPMENT_STATES WHERE EQUIPMENT_STATE_NAME='{0}'",RUN_STATE_NAME);
                runStateKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlString));
                //获取设备当前状态->RUN的事件更改主键。
                sqlString=string.Format(@"SELECT EQUIPMENT_CHANGE_STATE_KEY FROM EMS_EQUIPMENT_CHANGE_STATES
                                          WHERE EQUIPMENT_FROM_STATE_KEY='{0}'
                                          AND EQUIPMENT_TO_STATE_KEY='{1}'", 
                                          lostStateKey.PreventSQLInjection(), 
                                          runStateKey.PreventSQLInjection());
                lostToRunChangeStateKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlString));

                #region 新增批次设备数据

                sqlString = string.Format("INSERT INTO EMS_LOT_EQUIPMENT(LOT_EQUIPMENT_KEY, LOT_KEY, OPERATION_KEY, EQUIPMENT_KEY, START_TIMESTAMP, USER_KEY, QUANTITY,STEP_KEY) " +
                                           "VALUES('{0}','{1}','{2}','{3}', GETDATE(), '{4}', {5},'{6}')",
                                           UtilHelper.GenerateNewKey(0), 
                                           lotKey.PreventSQLInjection(),
                                           operationKey.PreventSQLInjection(),
                                           equipmentKey.PreventSQLInjection(), 
                                           userName.PreventSQLInjection(), 
                                           lotCurrentQuantity, 
                                           stepKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                //if (count <= 0)
                //{
                //    throw new Exception("${res:FanHai.Hemera.Module.Common.M0003}");
                //}

                #endregion

                //如果设备状态转变记录不为空。
                if (!string.IsNullOrEmpty(lostToRunChangeStateKey))
                {
                    #region 多腔体设备，需要更新父设备的状态和状态切换日志。

                    sqlString = string.Format("SELECT PARENT_EQUIPMENT_KEY FROM EMS_EQUIPMENTS WHERE EQUIPMENT_KEY = '{0}' AND ISCHAMBER = 1", equipmentKey.PreventSQLInjection());
                    object scalar = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);
                    //多腔体设备。
                    if (scalar != null && scalar != DBNull.Value) //Multi Chamber Equipment
                    {
                        string parentEquipmentKey = scalar.ToString();

                        #region 更新父设备状态。

                        sqlString = string.Format(@"UPDATE EMS_EQUIPMENTS
                                                SET EQUIPMENT_STATE_KEY = CASE EQUIPMENT_STATE_KEY WHEN '{3}' THEN '{1}' ELSE EQUIPMENT_STATE_KEY END, 
                                                EQUIPMENT_CHANGE_STATE_KEY = '{2}' 
                                                WHERE EQUIPMENT_KEY = '{0}' AND EQUIPMENT_STATE_KEY = '{3}'",
                                                parentEquipmentKey.PreventSQLInjection(),
                                                runStateKey.PreventSQLInjection(),
                                                lostToRunChangeStateKey.PreventSQLInjection(),
                                                lostStateKey.PreventSQLInjection());
                        n=db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                        if (n > 0)
                        {
                            #region 更新和插入父设备日志

                            //获取状态切换事件日志的最大值。
                            sqlString = string.Format(@"SELECT MAX(ISCURRENT) 
                                                    FROM EMS_STATE_EVENT
                                                    WHERE EQUIPMENT_KEY='{0}'",
                                                    parentEquipmentKey.PreventSQLInjection());
                            object objCount = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);
                            int count = 1;
                            if (objCount != null && objCount != DBNull.Value)
                            {
                                count = Convert.ToInt16(objCount) + 1;
                            }
                            //更新状态切换事件日志
                            sqlString = string.Format(@"UPDATE EMS_STATE_EVENT 
                                                    SET EQUIPMENT_TO_STATE_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{1}'
                                                    WHERE EQUIPMENT_KEY='{2}' 
                                                    AND EQUIPMENT_FROM_STATE_KEY='{3}'
                                                    AND ISCURRENT={4}",
                                                    runStateKey.PreventSQLInjection(),
                                                    userName.PreventSQLInjection(), 
                                                    parentEquipmentKey.PreventSQLInjection(), 
                                                    lostStateKey.PreventSQLInjection(),
                                                    count-1);
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);

                            //新增一笔状态切换事件日志
                            sqlString = string.Format(@"INSERT INTO EMS_STATE_EVENT
                                                    (EVENT_KEY,EQUIPMENT_KEY,EQUIPMENT_CHANGE_STATE_KEY,EQUIPMENT_FROM_STATE_KEY,ISCURRENT,CREATE_TIME,CREATOR)
                                                    VALUES('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE(),'{5}')",
                                                    UtilHelper.GenerateNewKey(0),
                                                    parentEquipmentKey.PreventSQLInjection(),
                                                    lostToRunChangeStateKey.PreventSQLInjection(),
                                                    runStateKey.PreventSQLInjection(),
                                                    count,
                                                    userName.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                            //if (command.ExecuteNonQuery() <= 0)
                            //{
                            //    throw new Exception("${res:FanHai.Hemera.Module.Common.M0004}");
                            //}
                            #endregion
                        }

                        #endregion
                    }

                    #endregion

                    #region 更新设备状态
                    sqlString = string.Format(@"UPDATE EMS_EQUIPMENTS 
                                            SET EQUIPMENT_STATE_KEY = CASE EQUIPMENT_STATE_KEY WHEN '{3}' THEN '{1}' ELSE EQUIPMENT_STATE_KEY END,
                                            EQUIPMENT_CHANGE_STATE_KEY = '{2}'
                                            WHERE EQUIPMENT_KEY = '{0}' AND EQUIPMENT_STATE_KEY = '{3}'",
                                            equipmentKey.PreventSQLInjection(),
                                            runStateKey.PreventSQLInjection(),
                                            lostToRunChangeStateKey.PreventSQLInjection(),
                                            lostStateKey.PreventSQLInjection());
                    n=db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                    if (n > 0)
                    {
                        #region 更新设备日志状态，插入设备日志。

                        //获取状态切换事件日志的最大值。
                        sqlString = string.Format(@"SELECT MAX(ISCURRENT) 
                                                FROM EMS_STATE_EVENT
                                                WHERE EQUIPMENT_KEY='{0}'",
                                                equipmentKey.PreventSQLInjection());
                        object objCount = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);
                        int count = 1;
                        if (objCount != null && objCount != DBNull.Value)
                        {
                            count = Convert.ToInt32(objCount) + 1;
                        }
                        //更新状态切换事件日志
                        sqlString = string.Format(@"UPDATE EMS_STATE_EVENT 
                                                SET EQUIPMENT_TO_STATE_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{1}'
                                                WHERE EQUIPMENT_KEY='{2}' 
                                                AND EQUIPMENT_FROM_STATE_KEY='{3}' 
                                                AND ISCURRENT ={4}",
                                                runStateKey.PreventSQLInjection(),
                                                userName.PreventSQLInjection(),
                                                equipmentKey.PreventSQLInjection(),
                                                lostStateKey.PreventSQLInjection(),
                                                count-1);
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);

                        //新增一笔状态切换事件日志
                        sqlString = string.Format(@"INSERT INTO EMS_STATE_EVENT
                                                    (EVENT_KEY,EQUIPMENT_KEY,EQUIPMENT_CHANGE_STATE_KEY,EQUIPMENT_FROM_STATE_KEY,ISCURRENT,CREATE_TIME,CREATOR)
                                                    VALUES('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE(),'{5}')",
                                                    UtilHelper.GenerateNewKey(0),
                                                    equipmentKey.PreventSQLInjection(),
                                                    lostToRunChangeStateKey.PreventSQLInjection(),
                                                    runStateKey.PreventSQLInjection(),
                                                    count,
                                                    userName.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                        //if (command.ExecuteNonQuery() <= 0)
                        //{
                        //    throw new Exception("${res:FanHai.Hemera.Module.Common.M0004}");
                        //}
                        #endregion
                    }

                    #endregion
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批次出站时更新设备信息。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbTrans">数据库操作事务对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="userName">用户。</param>
        /// <returns>true：更新设备信息成功。false：更新设备信息失败。</returns>
        internal static bool TrackOutForEquipment(Database db, DbTransaction dbTrans,
                                                string lotKey,
                                                string stepKey,
                                                string equipmentKey, 
                                                string userName)
        {
            const string LOST_STATE_NAME = "LOST";
            //const string RUN_STATE_NAME = "RUN";
            string lostStateKey = string.Empty;
            string runStateKey = string.Empty;
            string runToLostChangeStateKey = string.Empty;
            int n = 0;

            if (db != null && dbTrans != null)
            {
                string sqlString = string.Empty;
                //获取设备LOST的主键。
                sqlString = string.Format("SELECT EQUIPMENT_STATE_KEY FROM EMS_EQUIPMENT_STATES WHERE EQUIPMENT_STATE_NAME='{0}'", LOST_STATE_NAME);
                lostStateKey = Convert.ToString(db.ExecuteScalar(dbTrans,CommandType.Text,sqlString));
                //获取设备当前状态的主键。
                sqlString = string.Format("SELECT EQUIPMENT_STATE_KEY FROM EMS_EQUIPMENTS WHERE EQUIPMENT_KEY='{0}'", equipmentKey.PreventSQLInjection());
                runStateKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlString));
                //获取设备当前状态->LOST的事件更改主键。
                sqlString = string.Format(@"SELECT EQUIPMENT_CHANGE_STATE_KEY FROM EMS_EQUIPMENT_CHANGE_STATES
                                          WHERE EQUIPMENT_FROM_STATE_KEY='{0}'
                                          AND EQUIPMENT_TO_STATE_KEY='{1}'", 
                                          runStateKey.PreventSQLInjection(), 
                                          lostStateKey.PreventSQLInjection());
                runToLostChangeStateKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlString));

                #region 更新批次设备信息表

                sqlString = string.Format(@"UPDATE EMS_LOT_EQUIPMENT
                                            SET END_TIMESTAMP =  GETDATE(),USER_KEY='{3}' 
                                            WHERE LOT_KEY = '{0}' AND STEP_KEY = '{1}' AND EQUIPMENT_KEY = '{2}' AND END_TIMESTAMP IS NULL",
                                            lotKey.PreventSQLInjection(),
                                            stepKey.PreventSQLInjection(),
                                            equipmentKey.PreventSQLInjection(),
                                            userName.PreventSQLInjection());
                n = db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                if (n <= 0)
                {
                    LogService.LogError(sqlString);
                    //throw new Exception("更新批次设备信息数据失败。");
                }

                #endregion

                #region 检查设备当前是否有处理的批次。
                //检查设备当前是否有处理的批次。
                sqlString = string.Format(@"SELECT LOT_EQUIPMENT_KEY
                                           FROM EMS_LOT_EQUIPMENT 
                                           WHERE EQUIPMENT_KEY = '{0}' AND END_TIMESTAMP IS NULL", 
                                           equipmentKey.PreventSQLInjection());
                string lotEquipmentKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sqlString));
                if (!string.IsNullOrEmpty(runToLostChangeStateKey)  //可以转变为LOST
                    && string.IsNullOrEmpty(lotEquipmentKey))             //Equipment Currently Without Processing Lot
                {
                    #region 多腔体设备，需要更新父设备的状态和状态切换日志。

                    sqlString = string.Format("SELECT PARENT_EQUIPMENT_KEY FROM EMS_EQUIPMENTS WHERE EQUIPMENT_KEY = '{0}' AND ISCHAMBER = 1",
                                              equipmentKey.PreventSQLInjection());
                    object scalar = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);

                    if (scalar != null && scalar != DBNull.Value) //多腔体设备。
                    {
                        string parentEquipmentKey = scalar.ToString();
                        //检查父设备所有的子设备是否有正在处理的批次。
                        sqlString = string.Format(@"SELECT a.LOT_EQUIPMENT_KEY
                                                    FROM EMS_LOT_EQUIPMENT a
                                                    LEFT JOIN EMS_EQUIPMENTS b ON a.EQUIPMENT_KEY=b.EQUIPMENT_KEY
                                                    WHERE a.EQUIPMENT_KEY = '{0}' 
                                                    AND b.PARENT_EQUIPMENT_KEY='{1}'
                                                    AND a.END_TIMESTAMP IS NULL",
                                                    equipmentKey.PreventSQLInjection(),
                                                    parentEquipmentKey.PreventSQLInjection());
                        scalar = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);

                        if (scalar != null && scalar != DBNull.Value)
                        {
                            #region 更新父设备状态。

                            sqlString = string.Format(@"UPDATE EMS_EQUIPMENTS 
                                                   SET EQUIPMENT_STATE_KEY = CASE EQUIPMENT_STATE_KEY WHEN '{3}' THEN '{1}' ELSE EQUIPMENT_STATE_KEY END, 
                                                   EQUIPMENT_CHANGE_STATE_KEY = '{2}' 
                                                   WHERE EQUIPMENT_KEY = '{0}' AND EQUIPMENT_STATE_KEY = '{3}'",
                                                   parentEquipmentKey.PreventSQLInjection(),
                                                   lostStateKey.PreventSQLInjection(),
                                                   runToLostChangeStateKey.PreventSQLInjection(),
                                                   runStateKey.PreventSQLInjection());
                            n = db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                            if (n > 0)
                            {
                                #region 更新和插入父设备日志

                                //获取状态切换事件日志的最大值。
                                sqlString = string.Format(@"SELECT MAX(ISCURRENT) 
                                                    FROM EMS_STATE_EVENT
                                                    WHERE EQUIPMENT_KEY='{0}'",
                                                    parentEquipmentKey.PreventSQLInjection(),
                                                    runStateKey.PreventSQLInjection());
                                object objCount = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);
                                int count = 1;
                                if (objCount != null && objCount != DBNull.Value)
                                {
                                    count = Convert.ToInt32(objCount) + 1;
                                }
                                //更新状态切换事件日志
                                sqlString = string.Format(@"UPDATE EMS_STATE_EVENT 
                                                    SET EQUIPMENT_TO_STATE_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{1}'
                                                    WHERE EQUIPMENT_KEY='{2}' 
                                                    AND EQUIPMENT_FROM_STATE_KEY='{3}' 
                                                    AND ISCURRENT={4}",
                                                    lostStateKey.PreventSQLInjection(),
                                                    userName.PreventSQLInjection(),
                                                    parentEquipmentKey.PreventSQLInjection(),
                                                    runStateKey.PreventSQLInjection(),
                                                    count-1);
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);

                                //新增一笔状态切换事件日志
                                sqlString = string.Format(@"INSERT INTO EMS_STATE_EVENT
                                                    (EVENT_KEY,EQUIPMENT_KEY,EQUIPMENT_CHANGE_STATE_KEY,EQUIPMENT_FROM_STATE_KEY,ISCURRENT,CREATE_TIME,CREATOR)
                                                    VALUES('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE(),'{5}')",
                                                    UtilHelper.GenerateNewKey(0),
                                                    parentEquipmentKey.PreventSQLInjection(),
                                                    runToLostChangeStateKey.PreventSQLInjection(),
                                                    lostStateKey.PreventSQLInjection(),
                                                    count,
                                                    userName.PreventSQLInjection());
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                                //if (command.ExecuteNonQuery() <= 0)
                                //{
                                //    throw new Exception("${res:FanHai.Hemera.Module.Common.M0005}");
                                //}

                                #endregion
                            }

                            #endregion
                        }
                    }

                    #endregion

                    #region 更新设备状态

                    sqlString = string.Format(@"UPDATE EMS_EQUIPMENTS 
                                                SET EQUIPMENT_STATE_KEY = CASE EQUIPMENT_STATE_KEY WHEN '{3}' THEN '{1}' ELSE EQUIPMENT_STATE_KEY END,
                                                EQUIPMENT_CHANGE_STATE_KEY = '{2}' 
                                                WHERE EQUIPMENT_KEY = '{0}' AND EQUIPMENT_STATE_KEY = '{3}'",
                                                equipmentKey.PreventSQLInjection(),
                                                lostStateKey.PreventSQLInjection(),
                                                runToLostChangeStateKey.PreventSQLInjection(),
                                                runStateKey.PreventSQLInjection());
                    n=db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                    if (n > 0)
                    {
                        #region 更新设备日志状态，插入设备日志。

                        //获取状态切换事件日志的最大值。
                        sqlString = string.Format(@"SELECT MAX(ISCURRENT) 
                                                FROM EMS_STATE_EVENT
                                                WHERE EQUIPMENT_KEY='{0}'", 
                                                equipmentKey.PreventSQLInjection(),
                                                runStateKey.PreventSQLInjection());
                        object objCount = db.ExecuteScalar(dbTrans, CommandType.Text, sqlString);
                        double count = 1;
                        if (objCount != null && objCount != DBNull.Value)
                        {
                            count = Convert.ToDouble(objCount) + 1;
                        }

                        //更新状态切换事件日志
                        sqlString = string.Format(@"UPDATE EMS_STATE_EVENT 
                                                    SET EQUIPMENT_TO_STATE_KEY='{0}',EDIT_TIME=GETDATE(),EDITOR='{1}'
                                                    WHERE EQUIPMENT_KEY='{2}' 
                                                    AND EQUIPMENT_FROM_STATE_KEY='{3}' 
                                                    AND ISCURRENT={4}",
                                                    lostStateKey.PreventSQLInjection(),
                                                    userName.PreventSQLInjection(),
                                                    equipmentKey.PreventSQLInjection(),
                                                    runStateKey.PreventSQLInjection(),
                                                    count-1);
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);

                        //新增一笔状态切换事件日志
                        sqlString = string.Format(@"INSERT INTO EMS_STATE_EVENT
                                                    (EVENT_KEY,EQUIPMENT_KEY,EQUIPMENT_CHANGE_STATE_KEY,EQUIPMENT_FROM_STATE_KEY,ISCURRENT,CREATE_TIME,CREATOR)
                                                    VALUES('{0}', '{1}', '{2}', '{3}', '{4}', GETDATE(),'{5}')",
                                                    UtilHelper.GenerateNewKey(0),
                                                    equipmentKey.PreventSQLInjection(),
                                                    runToLostChangeStateKey.PreventSQLInjection(),
                                                    lostStateKey.PreventSQLInjection(),
                                                    count,
                                                    userName.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString);
                        //if (command.ExecuteNonQuery() <= 0)
                        //{
                        //    throw new Exception("${res:FanHai.Hemera.Module.Common.M0004}");
                        //}
                        #endregion
                    }

                    #endregion
                }

                #endregion
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
