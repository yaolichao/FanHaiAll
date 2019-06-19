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
        /// 获取待补片的生产批次（排除组件补片批次）信息。
        /// </summary>
        /// <remarks>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_TYPE"/>
        /// </remarks>
        /// <param name="workorderNo">工单号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="efficiency">转换效率。</param>
        /// <returns>包含待补片批次信息的数据集对象。</returns>
        public DataSet GetPatchedLotNumber(string workorderNo, string proId, string efficiency)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT LOT_KEY,LOT_NUMBER,WORK_ORDER_NO,PRO_ID,EFFICIENCY,QUANTITY_INITIAL,QUANTITY,ROUTE_ENTERPRISE_VER_KEY,CUR_ROUTE_VER_KEY,CUR_STEP_VER_KEY,EDIT_TIME
                                            FROM POR_LOT
                                            WHERE WORK_ORDER_NO='{0}' AND PRO_ID='{1}' AND EFFICIENCY='{2}'
                                            AND {3}='{4}'
                                            AND DELETED_TERM_FLAG=0",
                                            workorderNo.PreventSQLInjection(),
                                            proId.PreventSQLInjection(),
                                            efficiency.PreventSQLInjection(),
                                            POR_LOT_FIELDS.FIELD_LOT_TYPE,
                                            "N");
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPatchedLotNumber Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 电池片补片操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH"/>。
        /// </remarks>
        /// <param name="dsParams">包含补片信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotPatch(DataSet dsParams)
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
                //操作动作必须是 PATCH 
                if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数的补片操作动作不正确，请检查。");
                    return dsReturn;
                }
                //如果数据集中包含名称WIP_PATCH_FIELDS.DATABASE_TABLE_NAME的数据表对象。
                if (dsParams.Tables.Contains(WIP_PATCH_FIELDS.DATABASE_TABLE_NAME))
                {
                    string transactionKey = UtilHelper.GenerateNewKey(0);
                    string sql = string.Empty;
                    double sumPatchedQty = 0;
                    WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                    WIP_PATCH_FIELDS patchFields = new WIP_PATCH_FIELDS();
                    DataTable dtPatch = dsParams.Tables[WIP_PATCH_FIELDS.DATABASE_TABLE_NAME];              //存放补片明细数据
                    //遍历批次的报废数据。
                    for (int i = 0; i < dtPatch.Rows.Count; i++)
                    {
                        DataRow drPatch = dtPatch.Rows[i];
                        Hashtable htPatch = CommonUtils.ConvertRowToHashtable(drPatch);
                        string patchedLotKey = Convert.ToString(htPatch[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY]);
                        double patchedQuantity = Convert.ToDouble(htPatch[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]);
                        sumPatchedQty += patchedQuantity;   //总的补片数量。
                        int isOnlyPatch = Convert.ToInt32(htPatch[WIP_PATCH_FIELDS.FIELD_IS_ONLY_PATCHED]);
                        //获取被补片批次的信息
                        DataSet dsPatchLotInfo=LotManagement.GetLotBasicInfo(db,dbTran,patchedLotKey);
                        DataRow drPatchLotInfo = dsPatchLotInfo.Tables[0].Rows[0];
                        double quantityIn =Convert.ToDouble(drPatchLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                        //isOnlyPatch=0 先报废后补片，所以数量不需要改变。isOnlyPatch=1 仅做补片。
                        double quantityOut = isOnlyPatch==0? quantityIn: quantityIn + patchedQuantity;
                        string enterpriseKey =Convert.ToString(drPatchLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
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
                        string patchedTransactionKey = UtilHelper.GenerateNewKey(0);
                        AddWIPLot(dbTran, patchedTransactionKey, patchedLotKey);
                        //更新被补片批次的数量
                        sql = string.Format(@"UPDATE POR_LOT 
                                            SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
                                            WHERE LOT_KEY='{3}'",
                                            quantityOut,
                                            editor.PreventSQLInjection(),
                                            editTimeZone.PreventSQLInjection(),
                                            patchedLotKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        Hashtable htPatchedTransaction = new Hashtable(htTransaction);
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = patchedLotKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = enterpriseName;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = routeName;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = stepKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = stepName;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = workOrderKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = stateFlag;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = reworkFlag;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = lineKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = lineName;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE] = lineName;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY] = edcInsKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = equipmentKey;
                        //插入报废记录。
                        //isOnlyPatch=0 先报废后补片。isOnlyPatch=1 仅做补片。
                        if (isOnlyPatch == 0)
                        {
                            string scrapTransaction = UtilHelper.GenerateNewKey(0);
                            DataSet dsScrapParams = new DataSet();
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = quantityIn;
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = quantityIn - patchedQuantity;
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = scrapTransaction;
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP;
                            DataTable dtScrapTransaction = CommonUtils.ParseToDataTable(htPatchedTransaction);
                            dtScrapTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                            dsScrapParams.Tables.Add(dtScrapTransaction);
                            DataTable dtScrap = CommonUtils.CreateDataTable(new WIP_SCRAP_FIELDS());
                            DataRow drScrap = dtScrap.NewRow();
                            drScrap[WIP_SCRAP_FIELDS.FIELD_DESCRIPTION] = drPatch[WIP_PATCH_FIELDS.FIELD_DESCRIPTION];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_EDIT_TIME] = drPatch[WIP_PATCH_FIELDS.FIELD_EDIT_TIME];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = drPatch[WIP_PATCH_FIELDS.FIELD_EDIT_TIMEZONE_KEY];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_EDITOR] = drPatch[WIP_PATCH_FIELDS.FIELD_EDITOR];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_KEY] = drPatch[WIP_PATCH_FIELDS.FIELD_ENTERPRISE_KEY];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_NAME] = drPatch[WIP_PATCH_FIELDS.FIELD_ENTERPRISE_NAME];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS] = drPatch[WIP_PATCH_FIELDS.FIELD_REASON_CODE_CLASS];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY] = drPatch[WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = drPatch[WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_RESPONSIBLE_PERSON] = drPatch[WIP_PATCH_FIELDS.FIELD_RESPONSIBLE_PERSON];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_ROUTE_KEY] = drPatch[WIP_PATCH_FIELDS.FIELD_ROUTE_KEY];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_ROUTE_NAME] = drPatch[WIP_PATCH_FIELDS.FIELD_ROUTE_NAME];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = drPatch[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_STEP_KEY] = drPatch[WIP_PATCH_FIELDS.FIELD_STEP_KEY];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_STEP_NAME] = drPatch[WIP_PATCH_FIELDS.FIELD_STEP_NAME];
                            drScrap[WIP_SCRAP_FIELDS.FIELD_TRANSACTION_KEY] = scrapTransaction;
                            dtScrap.Rows.Add(drScrap);
                            dsScrapParams.Tables.Add(dtScrap);
                            LotScrap(dsScrapParams, dbTran);
                        }
                        //插入被补片批次的操作记录
                        //isOnlyPatch=0 先报废后补片。isOnlyPatch=1 仅做补片。
                        if (isOnlyPatch == 0)
                        {
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = quantityIn - patchedQuantity;
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = quantityIn;
                        }
                        else
                        {
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = quantityIn;
                            htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = quantityIn + patchedQuantity;
                        }
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = patchedTransactionKey;
                        htPatchedTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCHED;
                        sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htPatchedTransaction, null);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        //插入一笔批次补片明细数据
                        //补片操作记录主键。
                        if(!htPatch.ContainsKey(WIP_PATCH_FIELDS.FIELD_TRANSACTION_KEY))
                        {
                            htPatch.Add(WIP_PATCH_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                        }
                        htPatch[WIP_PATCH_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                        //被补片批次操作记录主键
                        if (!htPatch.ContainsKey(WIP_PATCH_FIELDS.FIELD_PATCHED_TRANSACTION_KEY))
                        {
                            htPatch.Add(WIP_PATCH_FIELDS.FIELD_PATCHED_TRANSACTION_KEY, patchedTransactionKey);
                        }
                        htPatch[WIP_PATCH_FIELDS.FIELD_PATCHED_TRANSACTION_KEY] = patchedTransactionKey;
                        //重置补片明细的编辑时间为当前时间。
                        htPatch[WIP_PATCH_FIELDS.FIELD_EDIT_TIME] = null;
                        sql = DatabaseTable.BuildInsertSqlStatement(patchFields, htPatch, null);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }

                    AddWIPLot(dbTran, transactionKey, lotKey);
                    //获取组件补片批次当前数量
                    sql = string.Format("SELECT QUANTITY FROM POR_LOT WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
                    double quantity=Convert.ToDouble(db.ExecuteScalar(CommandType.Text, sql));
                    if (quantity < sumPatchedQty)
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("组件补片批次当前实际数量[{0}]小于补片数量[{1}]，请确认。",quantity,sumPatchedQty));
                        dbTran.Rollback();
                        return dsReturn;
                    }
                    //更新批次数量。
                    sql = string.Format(@"UPDATE POR_LOT 
                                        SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',DELETED_TERM_FLAG={3}
                                        WHERE LOT_KEY='{4}'",
                                        quantity - sumPatchedQty,
                                        editor.PreventSQLInjection(),
                                        editTimeZone.PreventSQLInjection(),
                                        (quantity - sumPatchedQty) > 0 ? 0 : 1,
                                        lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //向WIP_TRANSACTION表插入批次补片的操作记录。
                    if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
                    {
                        htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                    }
                    htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                    sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotPatch Error: " + ex.Message);
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
