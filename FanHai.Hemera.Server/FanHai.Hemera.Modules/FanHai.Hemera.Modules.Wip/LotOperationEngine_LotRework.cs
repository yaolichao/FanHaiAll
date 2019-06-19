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
using FanHai.Hemera.Modules.FMM;
using FanHai.Hemera.Share.Common;



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// 批次(返工/返修)操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_REWORK"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次（返工/返修）信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotRework(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA)
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                    || !dsParams.Tables.Contains(WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME)
                    || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //返工批次操作记录不能为空记录。
                    || dsParams.Tables[WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0)         //返工信息不能为空       
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据

                
                DataTable dtHold = null;
                if (dsParams.Tables.Contains(WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME)
                    && dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                {
                    dtHold = dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME];           //存放暂停信息明细数据
                }
                //检查是否存在重复的批次主键。
                var lnq = from item in dtTransaction.AsEnumerable()
                          group item by item[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] into g
                          where g.Count()>1
                          select g.Count();
                if (lnq.Count()>0)
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复记录，请检查。");
                    return dsReturn;
                }
                //检查记录是否过期。防止重复修改。
                foreach (DataRow drTransaction in dtTransaction.Rows)
                {
                    string opEditTime = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME]);   //操作前批次编辑时间
                    string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);       //批次主键
                    string editor=Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);            //编辑人
                    string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
                    string stepKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);       //当前工步主键
                    //检查记录是否过期。防止重复修改。
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                    if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                    {
                        dbTran.Rollback();
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "信息已过期，请关闭该界面后重试。");
                        return dsReturn;
                    }
                   
                }
                //如果有暂停批次，先记录批次释放操作。
                if (dtHold != null)
                {
                    DataSet dsReleaseParams = new DataSet();
                    DataTable dtReleaseTransaction = dtTransaction.Copy();
                    DataTable dtReleaseHold = dtHold.Copy();
                    dsReleaseParams.Tables.Add(dtReleaseTransaction);
                    dsReleaseParams.Tables.Add(dtReleaseHold);
                    //组织释放批次的数据。
                    foreach (DataRow drTransaction in dtReleaseTransaction.Rows)
                    {
                        string curReleaseTransKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
                        string transKey = UtilHelper.GenerateNewKey(0);
                        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                        drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE;
                        //组织暂停批次信息。
                        var lnqHold = from item in dtReleaseHold.AsEnumerable()
                                      where Convert.ToString(item[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_TRANSACTION_KEY]) == curReleaseTransKey
                                      select item;
                        foreach (DataRow drHoldInfo in lnqHold)
                        {
                            drHoldInfo[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_TRANSACTION_KEY] = transKey;
                        }
                    }
                    //释放批次。
                    LotRelease(dsReleaseParams, dbTran);
                    dsReleaseParams.Tables.Clear();
                    dsReleaseParams = null;
                }
                //返工/返修批次
                LotRework(dsParams, dbTran);
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotRework Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            return dsReturn;
        }

        /// <summary>
        /// 返工/返修批次。
        /// </summary>
        /// <param name="dsParams">包含批次返工/返修信息的数据集对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        private void LotRework(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                    || !dsParams.Tables.Contains(WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME)
                    || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //返工批次操作记录不能为空记录。
                    || dsParams.Tables[WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0)         //返工信息不能为空     
            {
                throw new Exception("传入参数不正确，请检查。");
            }

            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
            DataTable dtComment = dsParams.Tables[WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME];           //存放返工信息明细数据
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
            string reworkEnterpriseKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            string reworkRouteKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            string reworkStepKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            DataTable dtHold = null;
            if (dsParams.Tables.Contains(WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME)
                && dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                dtHold = dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME];           //存放暂停信息明细数据
            }

            //插入操作记录
            foreach (DataRow drTransaction in dtTransaction.Rows)
            {
                string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);       //批次主键
                string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);            //编辑人
                string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
                string stepKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);       //当前工步主键
                if (Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]) != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_REWORK)
                {
                    throw new Exception("传入的操作名称不正确，请检查。");
                }
                string transactionKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
                AddWIPLot(dbTran, transactionKey, lotKey);
                //向WIP_TRANSACTION表插入批次调整的操作记录。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, drTransaction, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                //更新批次数据
                StringBuilder sbUpdateSql = new StringBuilder();
                sbUpdateSql.AppendFormat(@"UPDATE POR_LOT 
                                        SET STATE_FLAG=0,START_WAIT_TIME=GETDATE(),REWORK_FLAG=REWORK_FLAG+1,
                                        ROUTE_ENTERPRISE_VER_KEY='{0}',CUR_ROUTE_VER_KEY='{1}',CUR_STEP_VER_KEY='{2}',
                                        EDITOR='{3}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{4}'",
                                        reworkEnterpriseKey.PreventSQLInjection(),
                                        reworkRouteKey.PreventSQLInjection(),
                                        reworkStepKey.PreventSQLInjection(),
                                        editor.PreventSQLInjection(),
                                        timeZone.PreventSQLInjection());
                //如果当前有HOLD批次，则释放批次。
                if (dtHold != null)
                {
                    sbUpdateSql.Append(",HOLD_FLAG=0");
                }
                sbUpdateSql.AppendFormat(" WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateSql.ToString());

                //更新设备数据，以完成设备出站，切换设备状态。
                sql = string.Format(@"SELECT B.EQUIPMENT_KEY,A.EQUIPMENT_NAME,A.EQUIPMENT_STATE_KEY
                                    FROM EMS_EQUIPMENTS A,EMS_LOT_EQUIPMENT B
                                    WHERE A.EQUIPMENT_KEY = B.EQUIPMENT_KEY 
                                    AND B.STEP_KEY = '{0}' AND B.LOT_KEY='{1}' 
                                    AND B.END_TIMESTAMP IS NULL",
                                    stepKey.PreventSQLInjection(),
                                    lotKey.PreventSQLInjection());
                DataSet dsResult = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    string equipmentKey = Convert.ToString(dsResult.Tables[0].Rows[0]["EQUIPMENT_KEY"]);
                    WipManagement.TrackOutForEquipment(db,dbTran,lotKey, stepKey, equipmentKey, editor);
                }
            }
            //插入批次返工明细记录
            foreach (DataRow drComment in dtComment.Rows)
            {
                //向WIP_COMMENT表插入批次调整的操作记录。
                WIP_COMMENT_FIELDS commentFields = new WIP_COMMENT_FIELDS();
                string sql = DatabaseTable.BuildInsertSqlStatement(commentFields, drComment, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            }
        }
    }
}
