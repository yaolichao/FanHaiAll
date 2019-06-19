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
        /// 批次线别调整。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_LINE"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次线别调整信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotExchangeLine(DataSet dsParams)
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
                    || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)
                    || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0  //批次操作记录不能为空记录。
                    || dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0          //需要线别调整的批次清单
                    || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count != dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows.Count
                    )
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
                DataTable dtLotInfo = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];               //存放线别调整的批次清单
                //检查是否存在重复的批次主键。
                var lnq = from item in dtTransaction.AsEnumerable()
                          group item by item[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
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
                    string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);            //编辑人
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
                LotExchangeLine(dbTran, dsParams);

                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotExchangeLine Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 批次线别调整操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_LINE"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次线别调整信息的数据集对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        public void LotExchangeLine(DbTransaction dbTran, DataSet dsParams)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)
                || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0  //线别调整批次操作记录不能为空记录。
                || dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0          //线别调整批次清单
                || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count != dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows.Count
                )
            {
                throw new Exception("传入参数不正确，请检查。");
            }
            IDbConnection dbConn = dbTran.Connection;
            IDbCommand dbCmd = dbConn.CreateCommand();
            dbCmd.Transaction = dbTran;
            dbCmd.CommandType = CommandType.Text;
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
            DataTable dtLotInfo = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];               //线别调整批次清单
            //插入操作记录
            foreach (DataRow drTransaction in dtTransaction.Rows)
            {
                string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);       //批次主键
                string activity = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);            //编辑人
                string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
                //操作动作必须是 HOLD 
                if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_LINE)
                {
                    throw new Exception("传入参数的操作动作不正确，请检查。");
                }
                string transKey = UtilHelper.GenerateNewKey(0);

                //向Wip_Lot中插入批次参数记录
                AddWIPLot(dbTran, transKey, lotKey);

                //向WIP_TRANSACTION表插入批次调整的操作记录。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, drTransaction, null);
                dbCmd.CommandText = sql;
                dbCmd.ExecuteNonQuery();

                //更新批次数据
                DataRow drLotInfo = dtLotInfo.Select(string.Format(" LOT_KEY = '{0}'", lotKey))[0];

                StringBuilder sbUpdateSql = new StringBuilder();
                sbUpdateSql.AppendFormat(@"UPDATE POR_LOT 
                                           SET LOT_LINE_KEY = '{1}',
                                               LOT_LINE_CODE = '{2}',
                                               EDITOR = '{3}',
                                               EDIT_TIME = GETDATE(),
                                               EDIT_TIMEZONE = '{4}'
                                           WHERE LOT_KEY = '{0}'",
                                           lotKey.PreventSQLInjection(),
                                           drLotInfo[POR_LOT_FIELDS.FIELD_LOT_LINE_KEY],
                                           drLotInfo[POR_LOT_FIELDS.FIELD_LOT_LINE_CODE],
                                           drLotInfo[POR_LOT_FIELDS.FIELD_EDITOR],
                                           drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateSql.ToString());
            }
        }
    }
}
