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
        /// 获取批次暂停信息。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次暂停信息的数据集对象。</returns>
        public DataSet GetLotHoldInfo(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT a.*,c.LOT_NUMBER,c.LOT_KEY
                                            FROM WIP_HOLD_RELEASE a
                                            INNER JOIN WIP_TRANSACTION b on a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                            INNER JOIN POR_LOT c ON c.LOT_KEY=b.PIECE_KEY AND b.PIECE_TYPE=0
                                            WHERE b.ACTIVITY='HOLD' 
                                            AND b.UNDO_FLAG=0
                                            AND a.IS_RELEASE=0
                                            AND c.LOT_NUMBER='{0}'
                                            ORDER BY a.EDIT_TIME DESC", 
                                            lotNumber.PreventSQLInjection());
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotHoldInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 批次释放操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次释放信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotRelease(DataSet dsParams)
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
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                    || !dsParams.Tables.Contains(WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME)
                    || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //暂停批次操作记录不能为空记录。
                    || dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0     //暂停信息不能为空
                    )         
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
                DataTable dtHold = dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME];         //存放批次暂停原因明细数据
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
                //释放批次。
                LotRelease(dsParams, dbTran);
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotRelease Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 释放批次。
        /// </summary>
        /// <param name="dsParams">包含批次释放信息的数据集对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        private void LotRelease(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                || !dsParams.Tables.Contains(WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME)
                || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //暂停批次操作记录不能为空记录。
                || dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0     //暂停信息不能为空
                )
            {
                throw new Exception("传入参数不正确，请检查。");
            }

            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
            DataTable dtHold = dsParams.Tables[WIP_HOLD_RELEASE_FIELDS.DATABASE_TABLE_NAME];         //存放批次暂停原因明细数据
            //插入操作记录
            foreach (DataRow drTransaction in dtTransaction.Rows)
            {
                string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);       //批次主键
                string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);            //编辑人
                string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
                if (Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]) != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE)
                {
                    throw new Exception("传入的操作名称不正确，请检查。");
                }
                string transactionKey =Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
                AddWIPLot(dbTran, transactionKey, lotKey);
                //向WIP_TRANSACTION表插入批次调整的操作记录。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, drTransaction, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                //更新批次数据
                StringBuilder sbUpdateSql = new StringBuilder();
                sbUpdateSql.AppendFormat(@"UPDATE POR_LOT 
                                           SET HOLD_FLAG=0,EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
                                           WHERE LOT_KEY='{0}'",
                                           lotKey.PreventSQLInjection(),
                                           editor.PreventSQLInjection(),
                                           timeZone.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateSql.ToString());
            }
            //更新暂停记录,用于释放批次。
            foreach (DataRow drHold in dtHold.Rows)
            {
                string transKey = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_TRANSACTION_KEY]);
                string releaseTransKey = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_TRANSACTION_KEY]);
                string releaseOperator = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_OPERATOR]);
                string releaseTimezone = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_TIMEZONE]);
                string releaseDescription = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_RELEASE_DESCRIPTION]);
                string editTimezone = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE]);
                string editor = Convert.ToString(drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR]);
                string sql = string.Format(@"UPDATE WIP_HOLD_RELEASE
                                             SET IS_RELEASE=1,
                                                 RELEASE_TRANSACTION_KEY='{1}',
                                                 RELEASE_OPERATOR='{2}',
                                                 RELEASE_TIME=GETDATE(),
                                                 RELEASE_TIMEZONE='{3}',
                                                 RELEASE_DESCRIPTION='{4}',
                                                 EDIT_TIME=GETDATE(),
                                                 EDIT_TIMEZONE='{5}',
                                                 EDITOR='{6}'
                                             WHERE TRANSACTION_KEY='{0}'",
                                             transKey.PreventSQLInjection(),
                                             releaseTransKey.PreventSQLInjection(),
                                             releaseOperator.PreventSQLInjection(),
                                             releaseTimezone.PreventSQLInjection(),
                                             releaseDescription.PreventSQLInjection(),
                                             editTimezone.PreventSQLInjection(),
                                             editor.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            }
        }

    }
}
