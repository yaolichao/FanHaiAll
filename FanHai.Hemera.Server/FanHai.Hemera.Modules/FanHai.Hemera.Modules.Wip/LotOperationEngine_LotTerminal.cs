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
        /// 终止批次操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT"/>。
        /// </remarks>
        /// <param name="dsParams">包含终止批次信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotTerminal(DataSet dsParams)
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
                string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
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
                LotTerminal(dsParams, dbTran);
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
        /// 批次终止操作
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次终止信息的数据集对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        private void LotTerminal(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)
                || dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0      //批次操作记录不能为空记录。
               )
            {
                throw new Exception("传入参数不正确，请检查。");
            }
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
            string stepKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);
            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);
            string editTimeZone = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);

            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);
            //操作动作必须是 TERMINALLOT
            if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT)
            {
                throw new Exception("传入参数的不良操作动作不正确，请检查。");
            }
            //向WIP_TRANSACTION表插入批次不良的操作记录。
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            }
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

            //更新批次终止状态。
            sql = string.Format(@"UPDATE POR_LOT 
                                 SET DELETED_TERM_FLAG=1,EDITOR='{0}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{1}'
                                 WHERE LOT_KEY='{0}'",
                                 lotKey.PreventSQLInjection(),
                                 editor.PreventSQLInjection(),
                                 editTimeZone.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

            //更新批次设备信息表
            sql = string.Format(@"UPDATE EMS_LOT_EQUIPMENT
                                SET END_TIMESTAMP =  GETDATE(),USER_KEY='{2}' 
                                WHERE LOT_KEY = '{0}' AND STEP_KEY = '{1}' AND END_TIMESTAMP IS NULL",
                                lotKey.PreventSQLInjection(),
                                stepKey.PreventSQLInjection(),
                                editor.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
        }
    }
}
