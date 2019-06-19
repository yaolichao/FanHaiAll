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
using System.Transactions;


namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类-入库检验。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// 检查流程卡。
        /// </summary>
        /// <returns>true:成功;false:失败。</returns>
        public bool CheckProcessCard(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT t.CREATE_OPERTION_NAME,t1.create_date,t1.update_date,t1.serialno
                                                    FROM POR_LOT t
                                                    LEFT JOIN SERIALNO_CARD_SAVE t1 ON t.LOT_NUMBER=t1.serialno
                                                    WHERE t.LOT_NUMBER='{0}'",
                                                    lotNumber.PreventSQLInjection());
                DataTable dtIsExistModulePic = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                //无批次数据。
                if (dtIsExistModulePic.Rows.Count <= 0)
                {
                    return false;
                }
                string createOperationName =Convert.ToString(dtIsExistModulePic.Rows[0][POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]).Trim().ToUpper();
                if (createOperationName == "BCP" || createOperationName == "REPAIR")
                {
                    return true;
                }
                string serialNo = Convert.ToString(dtIsExistModulePic.Rows[0]["serialno"]);
                if (string.IsNullOrEmpty(serialNo))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.CheckProcessCard Error: " + ex.Message);
            }
            return true;
        }
        
        /// <summary>
        /// 入库检验作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotToWarehouseCheck(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)            //存放批次数据
                )
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                return dsReturn;
            }

            try
            {
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                DataTable dtWipConsignment = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtLots = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);       //编辑时区

                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        WIP_CONSIGNMENT_FIELDS consigmentFields = new WIP_CONSIGNMENT_FIELDS();

                        foreach (DataRow dr in dtWipConsignment.Rows)
                        {
                            #region 更新包装表。
                            int csDataGroup = Convert.ToInt32(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                            string palletNo = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                            string palletKey = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                            //托盘主键为空。给出提示。
                            if (string.IsNullOrEmpty(palletKey))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘记录{0}不存在，请检查。", palletNo));
                                return dsReturn;
                            }
                            //检查记录是否过期。防止重复修改。
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, palletKey);
                            List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                            listCondition.Add(kvp);
                            string opEditTime = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME]);
                            //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                            if (UtilHelper.CheckRecordExpired(db, WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘{0}信息已过期，请确认。", palletNo));
                                return dsReturn;
                            }
                            Hashtable hashTable = CommonUtils.ConvertRowToHashtable(dr);
                            hashTable.Remove(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY);
                            hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME] = null;
                            hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 2;
                            WhereConditions wc = new WhereConditions(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, palletKey);
                            string sql = DatabaseTable.BuildUpdateSqlStatement(consigmentFields, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            #endregion

                            var lnq = from item in dtLots.AsEnumerable()
                                      where Convert.ToString(item[POR_LOT_FIELDS.FIELD_PALLET_NO]) == palletNo
                                      orderby Convert.ToInt32(item[WIP_CONSIGNMENT_FIELDS.FIELDS_SEQ])
                                      select item;

                            Hashtable htStepTransaction = null;//存储下一工步数据。
                            foreach (DataRow drLot in lnq)
                            {
                                string lotKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                                string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                                string lotPalletNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                                string lotPalletTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_TIME]);

                                //检查记录是否过期。防止重复修改。
                                KeyValuePair<string, string> kvpLot = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                                List<KeyValuePair<string, string>> lstLotCondition = new List<KeyValuePair<string, string>>();
                                lstLotCondition.Add(kvpLot);
                                string opLotEditTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, lstLotCondition, opLotEditTime))
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("组件{0}信息已过期，请确认。", lotNumber));
                                    return dsReturn;
                                }

                                #region 批次进站
                                //获取批次数据
                                string sqlQueryLot = string.Format(@"SELECT A.*,
                                                                        B.ENTERPRISE_NAME,
                                                                        B.ENTERPRISE_VERSION,
                                                                        C.ROUTE_NAME ,
                                                                        D.ROUTE_STEP_NAME
                                                                    FROM POR_LOT A
                                                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                                                    WHERE A.STATUS < 2
                                                                    AND A.LOT_NUMBER='{0}'",
                                                                    lotNumber.PreventSQLInjection());
                                DataTable dtTable = db.ExecuteDataSet(dbTran, CommandType.Text, sqlQueryLot).Tables[0];
                                DataRow drLotInfo = dtTable.Rows[0];
                                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                                //如果批次没有进站，先进站。
                                if (stateFlag == 0)
                                {
                                    PackageLotTrackIn(dbTran, drLotInfo, htParams);
                                    stateFlag = 9;
                                }
                                #endregion

                                #region 批次过站
                                //重新获取批次数据。
                                if (stateFlag > 0 && stateFlag <= 9)
                                {
                                    string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                                    string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                                    string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                                    string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                                    string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                                    string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                                    //如果下一个工步主键为空，则获取下一个工步数据。每包抓第一块组件的下一工步为标准工步。从而统一包的工艺流程。
                                    if (htStepTransaction == null)
                                    {
                                        IEnterpriseEngine enterpriseEngine = RemotingServer.ServerObjFactory.Get<IEnterpriseEngine>();
                                        DataSet dsRouteNextStep = enterpriseEngine.GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                                        string toEnterpriseKey = enterpriseKey;
                                        string toRouteKey = routeKey;
                                        string toStepKey = stepKey;
                                        string toEnterpriseName = enterpriseName;
                                        string toRouteName = routeName;
                                        string toStepName = stepName;
                                        if (null != dsRouteNextStep
                                            && dsRouteNextStep.Tables.Count > 0
                                            && dsRouteNextStep.Tables[0].Rows.Count > 0)
                                        {
                                            DataRow drRouteNextStep = dsRouteNextStep.Tables[0].Rows[0];
                                            toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                                            toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                                            toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                                            toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                                            toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                                            toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                                        }
                                        htStepTransaction = new Hashtable();
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, timeZone);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                                    }
                                    else
                                    {
                                        htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                                        htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                                        htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = stepKey;
                                    }

                                    DataTable dtStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                                    dtStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                                    string nextStepKey = Convert.ToString(htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);
                                    bool isFinish = stepKey == nextStepKey; //最后一个工步，则结束批次。
                                    PackageLotTrackOut(dbTran, drLotInfo, dtStepTransaction, htParams, isFinish);
                                }
                                #endregion
                            }
                        }
                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.LotToWarehouseCheck Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 入库检验返回到上一工序作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotToWarehouseCheckReject(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)            //存放批次数据
                )
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                return dsReturn;
            }

            try
            {
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                DataTable dtWipConsignment = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtLots = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);       //编辑时区

                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        WIP_CONSIGNMENT_FIELDS consigmentFields = new WIP_CONSIGNMENT_FIELDS();

                        foreach (DataRow dr in dtWipConsignment.Rows)
                        {
                            //更新包装表。
                            int csDataGroup = Convert.ToInt32(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                            string palletNo = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                            string palletKey = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                            //托盘主键为空。给出提示。
                            if (string.IsNullOrEmpty(palletKey))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘记录{0}不存在，请检查。", palletNo));
                                return dsReturn;
                            }
                            //检查记录是否过期。防止重复修改。
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, palletKey);
                            List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                            listCondition.Add(kvp);
                            string opEditTime = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME]);
                            //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                            if (UtilHelper.CheckRecordExpired(db, WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘{0}信息已过期，请确认。", palletNo));
                                return dsReturn;
                            }
                            //更新包装已保存，但未过包装站
                            string sql = string.Format(@"UPDATE WIP_CONSIGNMENT
                                                         SET CS_DATA_GROUP=0,EDITOR='{1}',EDIT_TIME=GETDATE()
                                                         WHERE CONSIGNMENT_KEY='{0}'",
                                                         palletKey,
                                                         editor.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            var lnq = from item in dtLots.AsEnumerable()
                                      where Convert.ToString(item[POR_LOT_FIELDS.FIELD_PALLET_NO]) == palletNo
                                      orderby Convert.ToInt32(item[WIP_CONSIGNMENT_FIELDS.FIELDS_SEQ])
                                      select item;

                            Hashtable htStepTransaction = null;//存储下一工步数据。
                            foreach (DataRow drLot in lnq)
                            {
                                string lotKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                                string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                                string lotPalletNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                                string lotPalletTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_TIME]);

                                //检查记录是否过期。防止重复修改。
                                KeyValuePair<string, string> kvpLot = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                                List<KeyValuePair<string, string>> lstLotCondition = new List<KeyValuePair<string, string>>();
                                lstLotCondition.Add(kvpLot);
                                string opLotEditTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, lstLotCondition, opLotEditTime))
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("组件{0}信息已过期，请确认。", lotNumber));
                                    return dsReturn;
                                }
                                if (!htParams.Contains(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT))
                                {
                                    htParams.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "入库检验返到包装。");
                                }
                                #region 批次进站
                                //获取批次数据
                                string sqlQueryLot = string.Format(@"SELECT A.*,
                                                                        B.ENTERPRISE_NAME,
                                                                        B.ENTERPRISE_VERSION,
                                                                        C.ROUTE_NAME ,
                                                                        D.ROUTE_STEP_NAME
                                                                    FROM POR_LOT A
                                                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                                                    WHERE A.STATUS < 2
                                                                    AND A.LOT_NUMBER='{0}'",
                                                                    lotNumber.PreventSQLInjection());
                                DataTable dtTable = db.ExecuteDataSet(dbTran, CommandType.Text, sqlQueryLot).Tables[0];
                                DataRow drLotInfo = dtTable.Rows[0];
                                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                                //如果批次没有进站，先进站。
                                if (stateFlag == 0)
                                {
                                    PackageLotTrackIn(dbTran, drLotInfo, htParams);
                                    stateFlag = 9;
                                }
                                #endregion

                                #region 批次过站
                                //重新获取批次数据。
                                if (stateFlag > 0 && stateFlag <= 9)
                                {
                                    string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                                    string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                                    string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                                    string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                                    string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                                    string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                                    //如果下一个工步主键为空，则获取下一个工步数据。每包抓第一块组件的下一工步为标准工步。从而统一包的工艺流程。
                                    if (htStepTransaction == null)
                                    {
                                        IEnterpriseEngine enterpriseEngine = RemotingServer.ServerObjFactory.Get<IEnterpriseEngine>();
                                        DataSet dsRouteNextStep = enterpriseEngine.GetEnterprisePreviousRouteAndStep(enterpriseKey, routeKey, stepKey);
                                        string toEnterpriseKey = enterpriseKey;
                                        string toRouteKey = routeKey;
                                        string toStepKey = stepKey;
                                        string toEnterpriseName = enterpriseName;
                                        string toRouteName = routeName;
                                        string toStepName = stepName;
                                        if (null != dsRouteNextStep
                                            && dsRouteNextStep.Tables.Count > 0
                                            && dsRouteNextStep.Tables[0].Rows.Count > 0)
                                        {
                                            DataRow drRouteNextStep = dsRouteNextStep.Tables[0].Rows[0];
                                            toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                                            toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                                            toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                                            toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                                            toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                                            toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                                        }
                                        htStepTransaction = new Hashtable();
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, timeZone);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                                        htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                                    }
                                    else
                                    {
                                        htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                                        htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                                        htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = stepKey;
                                    }

                                    DataTable dtStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                                    dtStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                                    string nextStepKey = Convert.ToString(htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);
                                    bool isFinish = stepKey == nextStepKey; //最后一个工步，则结束批次。
                                    PackageLotTrackOut(dbTran, drLotInfo, dtStepTransaction, htParams, isFinish);
                                }
                                #endregion
                            }
                        }
                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.LotToWarehouseCheckReject Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
