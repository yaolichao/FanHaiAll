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
        /// 获取退料原因信息代码
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含退料原因代码的数据集对象。</returns>
        public DataSet GetReturnMaterialReasonCode(string stepKey)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT T2.REASON_CODE_KEY,T2.REASON_CODE_NAME,T2.REASON_CODE_TYPE
                               FROM FMM_REASON_CODE T2 
                               WHERE REASON_CODE_TYPE='TK'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReturnMaterialReasonCode Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 退料操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL"/>。
        /// </remarks>
        /// <param name="dsParams">包含退料信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotReturnMaterial(DataSet dsParams)
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
                double qty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN]);
                double leftQty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);
                string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                //操作动作必须是 RETURN_MATERIAL
                if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数的退料操作动作不正确，请检查。");
                    return dsReturn;
                }
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
                string transactionKey = UtilHelper.GenerateNewKey(0);
                AddWIPLot(dbTran, transactionKey, lotKey);
                //更新批次数量。
                string sql = string.Format(@"UPDATE POR_LOT 
                                            SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',DELETED_TERM_FLAG={3}
                                            WHERE LOT_KEY='{4}'",
                                            leftQty,
                                            editor.PreventSQLInjection(),
                                            editTimeZone.PreventSQLInjection(),
                                            leftQty > 0 ? 0 : 1,
                                            lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                //更新线上仓物料
                //查询工单主键，批次数量，物料批号，车间主键,工序名称。
                string sqlCommand = string.Format(@"SELECT a.MATERIAL_CODE,a.MATERIAL_LOT,a.FACTORYROOM_KEY,a.CREATE_OPERTION_NAME
                                                    FROM POR_LOT a
                                                    WHERE a.LOT_KEY='{0}'",
                                                    lotKey.PreventSQLInjection());
                DataSet ds = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string materialLot = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_LOT]);
                    string materialCode = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_CODE]);
                    string factoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                    string createOperationName = Convert.ToString(ds.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]);
                    //根据车间主键+工序+仓库类型获取线上仓主键。
                    sqlCommand = string.Format(@"SELECT STORE_KEY,STORE_NAME
                                                FROM WST_STORE 
                                                WHERE STORE_TYPE='9' AND LOCATION_KEY='{0}' AND OPERATION_NAME='{1}'",
                                                factoryRoomKey.PreventSQLInjection(),
                                                createOperationName.PreventSQLInjection());
                    ds = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand);
                    string storeKey = string.Empty;
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
                    }
                    string storeMaterialDetailKey = string.Empty;
                    //根据线上仓主键 + 物料批号键获取线上仓物料明细主键。
                    sqlCommand = string.Format(@"SELECT b.STORE_MATERIAL_DETAIL_KEY
                                        FROM WST_STORE_MATERIAL a
                                        LEFT JOIN POR_MATERIAL c ON a.MATERIAL_KEY=c.MATERIAL_KEY
                                        LEFT JOIN WST_STORE_MATERIAL_DETAIL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
                                        WHERE a.STORE_KEY='{0}' AND b.MATERIAL_LOT='{1}' AND c.MATERIAL_CODE='{2}'",
                                        storeKey.PreventSQLInjection(),
                                        materialLot.PreventSQLInjection(),
                                        materialCode.PreventSQLInjection());
                    ds = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        storeMaterialDetailKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"]);
                    }
                    //更新线上仓+物料批次数量。
                    sqlCommand = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                                SET CURRENT_QTY=CURRENT_QTY+{0}
                                                WHERE STORE_MATERIAL_DETAIL_KEY='{1}'",
                                                qty-leftQty,
                                                storeMaterialDetailKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                }
                //向WIP_TRANSACTION表插入批次退料的操作记录。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
                {
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                }
                htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields,htTransaction, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                //如果数据集中包含名称WIP_RETURN_MAT_FIELDS.DATABASE_TABLE_NAME的数据表对象。
                if (dsParams.Tables.Contains(WIP_RETURN_MAT_FIELDS.DATABASE_TABLE_NAME))
                {
                    WIP_RETURN_MAT_FIELDS returnFields = new WIP_RETURN_MAT_FIELDS();
                    DataTable dtReturn = dsParams.Tables[WIP_RETURN_MAT_FIELDS.DATABASE_TABLE_NAME];              //存放退料明细数据
                    //遍历批次的退料数据。
                    for (int i = 0; i < dtReturn.Rows.Count; i++)
                    {
                        DataRow drReturn=dtReturn.Rows[i];
                        Hashtable htReturn=CommonUtils.ConvertRowToHashtable(drReturn);
                        if (!htReturn.ContainsKey(WIP_RETURN_MAT_FIELDS.FIELD_TRANSACTION_KEY))
                        {
                            htReturn.Add(WIP_RETURN_MAT_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                        }
                        htReturn[WIP_RETURN_MAT_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                        //插入一笔批次退料数据。
                        sql = DatabaseTable.BuildInsertSqlStatement(returnFields, htReturn, null);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotReturnMaterial Error: " + ex.Message);
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
