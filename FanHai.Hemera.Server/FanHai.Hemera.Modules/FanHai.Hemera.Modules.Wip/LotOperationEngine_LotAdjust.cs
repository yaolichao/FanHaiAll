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
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format("SELECT DISTINCT PRODUCT_CODE,PRODUCT_NAME FROM POR_PRODUCT ORDER BY PRODUCT_CODE");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetProdId Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取转换效率数据。
        /// </summary>
        /// <returns>包含转换效率数据的数据集对象。</returns>
        public DataSet GetEfficiency()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT DISTINCT EFFICIENCY_NAME,LEFFICIENCY,UEFFICIENCY FROM BASE_EFFICIENCY
                                                    WHERE USED=1
                                                    ORDER BY LEFFICIENCY DESC,UEFFICIENCY DESC");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetEfficiency Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 批次调整操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_ADJUST"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次调整信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotAdjust(DataSet dsParams)
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
                    || !dsParams.Tables.Contains(WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME))           //存放操作数据
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];                      //存放修改后批次数据
                DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];   //存放操作数据
                DataTable dtComment = dsParams.Tables[WIP_COMMENT_FIELDS.DATABASE_TABLE_NAME];       //存放批次调整明细数据
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
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
                //插入操作记录
                foreach (DataRow drTransaction in dtTransaction.Rows)
                {
                    if (Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]) != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_ADJUST)
                    {
                        throw new Exception("传入操作动作名称不正确，请检查。");
                    }
                    string transactionKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
                    string lotKey = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);       //批次主键
                    string editor = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);          //编辑人
                    string timeZone = Convert.ToString(drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);        //编辑时区
                    AddWIPLot(dbTran, transactionKey, lotKey);
                    //向WIP_TRANSACTION表插入批次调整的操作记录。
                    WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                    drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                    string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, drTransaction, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //更新批次信息。
                    StringBuilder sbUpdateSql = new StringBuilder();
                    sbUpdateSql.AppendFormat("UPDATE POR_LOT SET EDITOR='{0}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{1}'",
                                            editor.PreventSQLInjection(),
                                            timeZone.PreventSQLInjection());
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                    {
                        sbUpdateSql.AppendFormat(",LOT_NUMBER='{0}'",
                                                 Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID))
                    {
                        sbUpdateSql.AppendFormat(",PRO_ID='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PRO_ID]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TYPE))
                    {
                        sbUpdateSql.AppendFormat(",CREATE_TYPE='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATE_TYPE]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_TYPE))
                    {
                        sbUpdateSql.AppendFormat(",LOT_TYPE='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_TYPE]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PRIORITY))
                    {
                        sbUpdateSql.AppendFormat(",PRIORITY='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PRIORITY]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_EFFICIENCY))
                    {
                        sbUpdateSql.AppendFormat(",EFFICIENCY='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_EFFICIENCY]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_SI_LOT))
                    {
                        sbUpdateSql.AppendFormat(",SI_LOT='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_SI_LOT]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY))
                    {
                        sbUpdateSql.AppendFormat(",ROUTE_ENTERPRISE_VER_KEY='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY))
                    {
                        sbUpdateSql.AppendFormat(",CUR_ROUTE_VER_KEY='{0}'",
                                                Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]).PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY))
                    {
                        string stepKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                        sbUpdateSql.AppendFormat(",CUR_STEP_VER_KEY='{0}',STATE_FLAG='{1}',START_WAIT_TIME=GETDATE()",
                                                stepKey.PreventSQLInjection(), 0); //更新批次状态为等待进站。
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
                    sbUpdateSql.AppendFormat(" WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateSql.ToString());
                }
                //插入批次调整明细记录
                foreach (DataRow drComment in dtComment.Rows)
                {
                    //向WIP_COMMENT表插入批次调整的操作记录。
                    WIP_COMMENT_FIELDS commentFields = new WIP_COMMENT_FIELDS();
                    string sql = DatabaseTable.BuildInsertSqlStatement(commentFields, drComment, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotAdjust Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            return dsReturn;
        }

    }
}
