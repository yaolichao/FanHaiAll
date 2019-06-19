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
        /// （电池片/组件）不良操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLDEFECT"/>和<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_DEFECT"/>。
        /// </remarks>
        /// <param name="dsParams">包含不良信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotDefect(DataSet dsParams)
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
                    || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)            //存放操作数据
                    | dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0)          
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
                Hashtable htTransaction = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtTransaction);
                Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                string opEditTime = Convert.ToString(htParams[COMMON_FIELDS.FIELD_COMMON_EDIT_TIME]);   //操作时编辑时间
                string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);
                string editTimeZone= Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
                string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
                double leftQty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);
                string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                int deletedTermFlag = 0;
                //检查记录是否过期。防止重复修改。
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                    return dsReturn;
                }
                LotDefect(dsParams, dbTran);
                //更新批次数量。
                string sql = string.Format(@"UPDATE POR_LOT 
                                            SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',DELETED_TERM_FLAG={4}
                                            WHERE LOT_KEY='{3}'",
                                            leftQty,
                                            editor.PreventSQLInjection(),
                                            editTimeZone.PreventSQLInjection(),
                                            lotKey.PreventSQLInjection(),
                                            deletedTermFlag);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotDefect Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        ///（电池片/组件）不良操作
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLDEFECT"/>和<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_DEFECT"/>。
        /// </remarks>
        /// <param name="dsParams">包含不良信息的数据集对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        private void LotDefect(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                || !dsParams.Tables.Contains(WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME)
                || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //批次操作记录不能为空记录。
               )
            {
                throw new Exception("传入参数不正确，请检查。");
            }
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
            //操作动作必须是 DEFECT 或 CELLDEFECT
            if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLDEFECT
                && activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_DEFECT)
            {
                throw new Exception("传入参数的不良操作动作不正确，请检查。");
            }
            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);
            //向WIP_TRANSACTION表插入批次不良的操作记录。
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            }
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //如果数据集中包含名称WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME的数据表对象。
            if (dsParams.Tables.Contains(WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME))
            {
                WIP_DEFECT_FIELDS defectFields = new WIP_DEFECT_FIELDS();
                DataTable dtDefect = dsParams.Tables[WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME];              //存放不良明细数据
                //遍历批次的不良数据。
                for (int i = 0; i < dtDefect.Rows.Count; i++)
                {
                    DataRow drDefect = dtDefect.Rows[i];
                    Hashtable htDefect = CommonUtils.ConvertRowToHashtable(drDefect);
                    if (!htDefect.ContainsKey(WIP_DEFECT_FIELDS.FIELD_TRANSACTION_KEY))
                    {
                        htDefect.Add(WIP_DEFECT_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                    }
                    htDefect[WIP_DEFECT_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                    //插入一笔批次不良数据。
                    sql = DatabaseTable.BuildInsertSqlStatement(defectFields, htDefect, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
            }
        }
    }
}
