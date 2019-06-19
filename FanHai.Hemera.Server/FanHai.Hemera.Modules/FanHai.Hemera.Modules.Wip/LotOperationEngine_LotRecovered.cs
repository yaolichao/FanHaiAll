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
        /// 获取电池片回收的可选问题工序。
        /// </summary>
        /// <param name="lotKey">回收电池片的批次主键。。</param>
        /// <param name="operations">拥有权限的工序名称，使用逗号(,)分隔。</param>
        /// <returns>包含问题工序的数据集对象。</returns>
        public DataSet GetRecoveredTroubleStepInfo(string lotKey, string operations)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT *
                                    FROM V_PROCESS_PLAN 
                                    WHERE ENTERPRISE_STATUS=1 AND ROUTE_STATUS=1
                                    AND ROUTE_STEP_KEY IN(
				                                    SELECT DISTINCT a.STEP_KEY
				                                    FROM WIP_PATCH a
				                                    WHERE a.PATCH_LOT_KEY='{0}'
				                                    UNION ALL
				                                    SELECT DISTINCT a.STEP_KEY
				                                    FROM WIP_SCRAP a
				                                    LEFT JOIN WIP_TRANSACTION b ON a.TRANSACTION_KEY=b.TRANSACTION_KEY
				                                    WHERE b.PIECE_KEY='{0}'
				                                    )
                                    AND ROUTE_ENTERPRISE_VER_KEY IN ( 
                                                    SELECT DISTINCT a.ENTERPRISE_KEY
                                                    FROM WIP_PATCH a
                                                    WHERE a.PATCH_LOT_KEY='{0}'
                                                    UNION ALL
                                                    SELECT DISTINCT a.ENTERPRISE_KEY
                                                    FROM WIP_SCRAP a
                                                    LEFT JOIN WIP_TRANSACTION b ON a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                                    WHERE b.PIECE_KEY='{0}') ",
                                    lotKey.PreventSQLInjection());
                sbSql.Append(UtilHelper.BuilderWhereConditionString("ROUTE_STEP_NAME", operations.PreventSQLInjection().Split(',')));
                sbSql.Append(" ORDER BY ((ROUTE_SEQ+1)*10000000)+ROUTE_STEP_SEQ");
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTroubleStep Error: " + ex.Message);
            }
            return dsReturn;
        }
         /// <summary>
        /// 获取待回收的批次信息（包含电池片报废和被电池片补片的批次数据）。
        /// </summary>
        /// <param name="recoverdLotKey">回收批次主键。</param>
        /// <param name="operationKey">问题工序主键。</param>
        /// <returns>包含待回收批次信息的数据集对象。</returns>
        public DataSet GetBeRecoverdLotNumber(string recoverdLotKey, string operationKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT P_KEY,LOT_KEY,LOT_NUMBER,ACTIVITY,REASON_CODE_KEY,REASON_CODE_NAME,SCRAP_PATCH_SUM_QUANTITY,
                                                    ISNULL(RECOVERED_SUM_QUANTITY,0) RECOVERED_SUM_QUANTITY,EDIT_TIME
                                            FROM
                                            ( 
                                                --查询批次报废信息
                                                SELECT c.LOT_KEY+'_'+b.ACTIVITY+'_'+a.REASON_CODE_KEY P_KEY,
                                                       c.LOT_KEY,c.LOT_NUMBER,b.ACTIVITY,a.REASON_CODE_KEY,a.REASON_CODE_NAME,
                                                       SUM(a.SCRAP_QUANTITY) SCRAP_PATCH_SUM_QUANTITY,
                                                       (
				                                            SELECT SUM(ISNULL(d.RECOVERED_QUANTITY,0))  
				                                            FROM WIP_RECOVERED d 
				                                            WHERE d.RECOVERED_LOT_KEY=c.LOT_KEY
				                                            AND d.BERECOVERED_REASON_CODE_KEY=a.REASON_CODE_KEY 
				                                            AND d.STEP_KEY='{1}'
			                                            ) RECOVERED_SUM_QUANTITY,
                                                       c.EDIT_TIME
                                                FROM WIP_SCRAP a
                                                INNER JOIN WIP_TRANSACTION b ON a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                                INNER JOIN POR_LOT c ON b.PIECE_KEY=c.LOT_KEY AND b.PIECE_TYPE=0
                                                WHERE b.UNDO_FLAG=0 AND c.DELETED_TERM_FLAG=0 AND c.HOLD_FLAG=0 AND c.QUANTITY_INITIAL>c.QUANTITY
                                                AND b.ACTIVITY='{0}' 
                                                AND a.STEP_KEY='{1}'
                                                AND b.PIECE_KEY='{2}'
                                                GROUP BY c.LOT_KEY,c.LOT_NUMBER,b.ACTIVITY,a.REASON_CODE_KEY,a.REASON_CODE_NAME,c.EDIT_TIME
                                                UNION ALL
                                                --查询被补片的批次信息
                                                SELECT c.LOT_KEY+'_'+b.ACTIVITY+'_'+a.REASON_CODE_KEY P_KEY,
                                                       c.LOT_KEY,c.LOT_NUMBER,b.ACTIVITY,a.REASON_CODE_KEY,a.REASON_CODE_NAME,
                                                       SUM(a.PATCH_QUANTITY) SCRAP_PATCH_SUM_QUANTITY,
                                                       (
				                                            SELECT SUM(ISNULL(d.RECOVERED_QUANTITY,0))  
				                                            FROM WIP_RECOVERED d 
				                                            WHERE d.RECOVERED_LOT_KEY='{2}'
				                                            AND d.BERECOVERED_LOT_KEY=c.LOT_KEY
				                                            AND d.BERECOVERED_REASON_CODE_KEY=a.REASON_CODE_KEY 
				                                            AND d.STEP_KEY='{1}'
			                                            ) RECOVERED_SUM_QUANTITY,
                                                       c.EDIT_TIME
                                                FROM WIP_PATCH a
                                                INNER JOIN WIP_TRANSACTION b ON a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                                INNER JOIN POR_LOT c ON c.LOT_KEY=a.PATCHED_LOT_KEY
                                                WHERE b.UNDO_FLAG=0 AND c.DELETED_TERM_FLAG=0 AND c.HOLD_FLAG=0 --AND c.QUANTITY_INITIAL>c.QUANTITY
                                                AND b.ACTIVITY='{3}' 
                                                AND a.STEP_KEY='{1}'
                                                AND b.PIECE_KEY='{2}'
                                                GROUP BY c.LOT_KEY,c.LOT_NUMBER,b.ACTIVITY,a.REASON_CODE_KEY,a.REASON_CODE_NAME,c.EDIT_TIME
                                            ) T
                                            WHERE SCRAP_PATCH_SUM_QUANTITY>ISNULL(RECOVERED_SUM_QUANTITY,0)",
                                            ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP,
                                            operationKey.PreventSQLInjection(),
                                            recoverdLotKey.PreventSQLInjection(),
                                            ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH);
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBeRecoverdLotNumber Error: " + ex.Message);
            }
            return dsReturn;
        }
       /// <summary>
        /// 电池片回收操作，用于撤销电池片报废和电池片补片。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RECOVERED"/>。
        /// </remarks>
        /// <param name="dsParams">包含回收信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotRecovered(DataSet dsParams)
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
                    || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME))           //存放操作数据
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
                //操作动作必须是 RECOVERED(电池片回收) 
                if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RECOVERED)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数的电池片回收操作动作不正确，请检查。");
                    return dsReturn;
                }
                //检查记录是否过期。防止重复修改。
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn,"信息已过期，请关闭该界面后重试。");
                    return dsReturn;
                }
                string transactionKey = UtilHelper.GenerateNewKey(0);
                AddWIPLot(dbTran, transactionKey, lotKey);

                //更新批次数量。
                string sql = string.Format(@"UPDATE POR_LOT 
                                            SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
                                            WHERE LOT_KEY='{3}'",
                                            leftQty,
                                            editor.PreventSQLInjection(),
                                            editTimeZone.PreventSQLInjection(),
                                            lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                //向WIP_TRANSACTION表插入批次回收的操作记录。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
                {
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                }
                htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields,htTransaction, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                //如果数据集中包含名称WIP_RECOVERED_FIELDS.DATABASE_TABLE_NAME的数据表对象。
                if (dsParams.Tables.Contains(WIP_RECOVERED_FIELDS.DATABASE_TABLE_NAME))
                {
                    WIP_RECOVERED_FIELDS recoveredFields = new WIP_RECOVERED_FIELDS();
                    DataTable dtRecovered = dsParams.Tables[WIP_RECOVERED_FIELDS.DATABASE_TABLE_NAME];              //存放回收明细数据
                    //获取不重复的被回收批次及其对应的编辑时间，只有在电池片回收是撤销补片操作时才发生。
                    var distinctRecoveredLot = from item in dtRecovered.AsEnumerable()
                                               where Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_LOT_KEY])!=lotKey //排除回收批次
                                               group item by new
                                               {
                                                   LotKey = Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_LOT_KEY]),
                                                   EditTime = Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_EDIT_TIME])
                                               } into g
                                               select new { LotKey = g.Key.LotKey, EditTime = g.Key.EditTime, Count = g.Count() };

                    foreach (var item in distinctRecoveredLot)
                    {
                        //检查记录是否过期。防止重复修改。
                        kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, item.LotKey);
                        listCondition = new List<KeyValuePair<string, string>>();
                        listCondition.Add(kvp);
                        //如果记录过期，编辑时间<数据库中的记录编辑时间。结束方法执行。
                        if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, item.EditTime))
                        {
                            string msg = string.Format("信息已过期，请关闭该界面后重试。");
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                            return dsReturn;
                        }
                    }

                    //遍历批次的回收数据。
                    for (int i = 0; i < dtRecovered.Rows.Count; i++)
                    {
                        DataRow drRecovered = dtRecovered.Rows[i];
                        Hashtable htRecovered = CommonUtils.ConvertRowToHashtable(drRecovered);
                        string beRecoveredTransactionKey =string.Empty;
                        string recoveredType = Convert.ToString(htRecovered[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_TYPE]);
                        //回收类型为撤销电池片补片（PATCH）操作,需要新增被回收批次的操作记录。
                        if (recoveredType == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH)
                        {
                            string beRecoveredLotKey = Convert.ToString(htRecovered[WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY]);
                            double recoveredQuantity = Convert.ToDouble(htRecovered[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY]);
                            //获取被回收批次的信息
                            DataSet dsPatchLotInfo = LotManagement.GetLotBasicInfo(db, dbTran, beRecoveredLotKey);
                            DataRow drPatchLotInfo = dsPatchLotInfo.Tables[0].Rows[0];
                            double quantityIn = Convert.ToDouble(drPatchLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                            //double quantityOut = quantityIn - recoveredQuantity; //电池片补片时不再回加被补片批次的数量，所以在不需要减去回收数量
                            double quantityOut = quantityIn;
                            string enterpriseKey = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                            string enterpriseName = Convert.ToString(drPatchLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                            string routeKey = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                            string routeName = Convert.ToString(drPatchLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                            string stepKey = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                            string stepName = Convert.ToString(drPatchLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                            string workOrderKey = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                            string stateFlag = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                            string reworkFlag = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                            string lineKey = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                            string lineName = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_OPR_LINE]);
                            string edcInsKey = Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
                            string equipmentKey = Convert.ToString(drPatchLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);

                            beRecoveredTransactionKey = UtilHelper.GenerateNewKey(0);
                            AddWIPLot(dbTran, beRecoveredTransactionKey, beRecoveredLotKey);
                            //更新被回收批次的数量
                            sql = string.Format(@"UPDATE POR_LOT 
                                                SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
                                                WHERE LOT_KEY='{3}'",
                                                quantityOut,
                                                editor.PreventSQLInjection(),
                                                editTimeZone.PreventSQLInjection(),
                                                beRecoveredLotKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //插入被回收批次的操作记录
                            Hashtable htBeRecoveredTransaction = new Hashtable(htTransaction);
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = beRecoveredTransactionKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = beRecoveredLotKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_BE_RECOVERED;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = quantityIn;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = quantityOut;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = enterpriseName;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = routeName;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = stepKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = stepName;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = workOrderKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = stateFlag;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = reworkFlag;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = lineKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = lineName;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE] = lineName;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY] = edcInsKey;
                            htBeRecoveredTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = equipmentKey;
                            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htBeRecoveredTransaction, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        //插入一笔批次回收明细数据
                        //回收操作记录主键。
                        if (!htRecovered.ContainsKey(WIP_RECOVERED_FIELDS.FIELD_TRANSACTION_KEY))
                        {
                            htRecovered.Add(WIP_RECOVERED_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                        }
                        htRecovered[WIP_RECOVERED_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                        //被回收批次操作记录主键，只是回收操作是撤销电池片补片时才有值，否则为空白值。
                        if (!htRecovered.ContainsKey(WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_TRANSACTION_KEY))
                        {
                            htRecovered.Add(WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_TRANSACTION_KEY, beRecoveredTransactionKey);
                        }
                        htRecovered[WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_TRANSACTION_KEY] = beRecoveredTransactionKey;
                        //重置回收明细的编辑时间为当前时间。
                        htRecovered[WIP_RECOVERED_FIELDS.FIELD_EDIT_TIME] = null;
                        sql = DatabaseTable.BuildInsertSqlStatement(recoveredFields, htRecovered, null);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotRecovered Error: " + ex.Message);
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
