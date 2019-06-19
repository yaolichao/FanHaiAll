//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-09-13            重构 迁移到SQL Server数据库
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Share.Constants;
using System.Data.Common;
using System.Collections;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Modules.WipJob;
using SolarViewer.Hemera.Modules.FMM;
namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 执行批次出站或仅采集批次报废数量。
        /// </summary>
        /// <param name="dataset">包含批次出站信息的数据集对象。
        /// 必须包含名称为<see cref=" TRANS_TABLES.TABLE_PARAM"/>的数据表，用于存储批次及批出站主信息的数据。
        /// 包含名称为<see cref=" WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME"/>的数据表（可选），用于存储批次报废的数据。
        /// 包含名称为<see cref="WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME"/>的数据表（可选），用于存储批次返工或退库的数据。
        /// 包含名称为<see cref="EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME"/>的数据表（可选），用于存储设备数据。</param>
        /// <returns>
        /// 包含方法执行结果的数据集。
        /// </returns>
        public DataSet TrackOutLot(DataSet dsParams)
        {
            System.DateTime startTime = System.DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbtran = null;
            DataTable dtParams = new DataTable();
            Hashtable htParams = new Hashtable();
            //string module = "";
            int quantityOut = 0;
            int quantityToMerge = 0;
            string lotKeyForMerge = string.Empty;
            string lotKey = string.Empty;
            string workOrderKey =  string.Empty,lineKey = "",editor = "",opUser = "",editTime = string.Empty;
            int workOrderStep = 0;                                            //工单序号
            string isAutoTrackIn =  string.Empty, oprLine = string.Empty;     //是否自动进站，操作线别
            string shiftName = string.Empty;                                  //班次名称
            bool isFinished = false;                                          //批次完成状态。
            string toStepKey = string.Empty;                                  //工步主键
            string toRouteKey = string.Empty;                                 //工艺流程主键
            string toEnterpriseKey = string.Empty;                            //工艺流程组主键
            string editTimeZone = string.Empty;                               //编辑时间时区
            string operateCompName =string.Empty;                             //操作计算机名称
            string shiftKey = string.Empty;                                   //班次主键
            string stepKey = string.Empty;                                    //当前工步主键
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                //Create Transaction  
                dbtran = dbConn.BeginTransaction();

                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    htParams = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    lotKey =  Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                    lotKeyForMerge = lotKey;
                    workOrderKey =  Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY]);
                    lineKey =  Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
                    oprLine =  Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
                    editor =  Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
                    opUser =  Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
                    shiftName =  Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
                    editTime = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME]);
                    toStepKey = Convert.ToString(htParams[WIP_FIELDS.FIELDS_TO_STEP_VER_KEY]);
                    toRouteKey = Convert.ToString(htParams[WIP_FIELDS.FIELDS_TO_ROUTE_VER_KEY]);
                    toEnterpriseKey = Convert.ToString(htParams[WIP_FIELDS.FIELDS_TO_ENTERPRISE_VER_KEY]);
                    //module = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_MODULE]);
                    editTimeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
                    operateCompName = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
                    shiftKey = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
                    stepKey = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);
                    //如果哈希表中包含最后一步的字段。表示该工步为最后一个工步，设置批次完成状态为true
                    if (htParams.ContainsKey("LAST_STEP"))
                    {
                        isFinished = Boolean.Parse(htParams["LAST_STEP"].ToString());
                    }
                    quantityOut = Convert.ToInt32(htParams[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);
                    quantityToMerge = quantityOut;
                }

                #region 检查记录是否过期。防止重复修改。
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                //如果数据记录过期，则返回结束方法执行。
                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, editTime))
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                    return dsReturn;
                }
                #endregion

                //如果数据集中包含批次报废数据表。
                if (dsParams.Tables.Contains(WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME))
                {
                    WipManagement.SetLossBonus(db, dbtran, dsParams);
                }
                //如果数据集中包含批次返工或退库的数据表。
                if (dsParams.Tables.Contains(WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME))
                {
                    SetDefect(dbtran, dsParams);
                }
                //插入批次出站的记录信息。
                WipManagement.TrackOutOrReworkLot(db, dbtran, dsParams, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
                //更新批次出站和设备关联的记录信息。
                if (dsParams.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME))
                {
                    TrackOutOrReworkForEquipment(dbtran, dsParams);
                }

                #region 确定是否自动分批或自动合批
                //如果不是完成状态且数量>0
                if (isFinished == false && quantityOut > 0)
                {
                    int maxBoxQuantity = -1;
                    string autoMerge = "";
                    string autoSplit = "";
                    //获取下一工步的自定义属性。
                    DataSet dsStepUda = GetStepUda(toStepKey);
                    if (dsStepUda.Tables[0].Rows.Count > 0)
                    {
                        //获取下一工步的自定义属性成功。
                        for (int i = 0; i < dsStepUda.Tables[0].Rows.Count; i++)
                        {
                            string szStepUdaName = dsStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME].ToString();
                            string szStepUdaValue = dsStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString();
                            //箱的最大数量
                            if (szStepUdaName == "MaxBoxQuantity")
                            {
                                maxBoxQuantity = Convert.ToInt32(szStepUdaValue);
                            }
                            //自动合批
                            if (szStepUdaName == "AutoMerge")
                            {
                                autoMerge = szStepUdaValue;
                            }
                            //自动分批
                            if (szStepUdaName == "AutoSplit")
                            {
                                autoSplit = szStepUdaValue;
                            }
                            //自动出站
                            if (szStepUdaName == "AutoTrackIn")
                            {
                                isAutoTrackIn = szStepUdaValue;
                            }
                        }
                        //确定自动分批
                        if (autoSplit.ToLower() == "true")
                        {
                            //箱最大数量
                            if (maxBoxQuantity == -1)
                            {
                                throw new Exception("${res:SolarViewer.Hemera.Addins.WIP.AutoSplit.Exception}");
                            }
                            else if (quantityOut > maxBoxQuantity)//出站数量>箱最大数量。
                            {
                                #region splitLot
                                #region MainDataTable
                                if (htParams.Contains(POR_LOT_FIELDS.FIELD_QUANTITY))
                                {
                                    htParams.Remove(POR_LOT_FIELDS.FIELD_QUANTITY);
                                }
                                htParams.Add(POR_LOT_FIELDS.FIELD_QUANTITY, maxBoxQuantity.ToString());
                                htParams[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = quantityOut;
                                htParams[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY] = toEnterpriseKey;
                                htParams[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY] = toRouteKey;
                                htParams[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY] = toStepKey;
                                htParams.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG, "0");
                                if (workOrderStep != 0)
                                {
                                    htParams.Add(COMMON_FIELDS.FIELD_WORK_ORDER_STEP, workOrderStep);
                                }
                                DataTable splitTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParams);
                                splitTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                                DataSet splitDS = new DataSet();
                                splitDS.Tables.Add(splitTable);
                                #endregion
                                int remainderQty = quantityOut - maxBoxQuantity;
                                #region ChildTable
                                DataTable childTable = new DataTable();
                                //childTable.Columns.Add("LOT_NUMBER", Type.GetType("System.String"));
                                childTable.Columns.Add("QUANTITY", Type.GetType("System.String"));
                                childTable.Columns.Add("SPLIT_SEQ", Type.GetType("System.String"));
                                childTable.Columns.Add("LOT_KEY", Type.GetType("System.String"));
                                //string childLotNumber = "";
                                string lastChildLotKey = "";
                                int i = 0;
                                //剩余数量-箱最大数量>0,继续分批
                                while ((remainderQty - maxBoxQuantity) >= 0)
                                {
                                    #region GenerateChildLotNumber
                                    i++;
                                    #endregion
                                    //childTable.Rows.Add(childLotNumber, maxBoxQuantity.ToString(), i.ToString("00"), Utils.GenerateNewKey(0));
                                    childTable.Rows.Add(maxBoxQuantity.ToString(), i.ToString("00"), UtilHelper.GenerateNewKey(0));
                                    remainderQty = remainderQty - maxBoxQuantity;
                                }
                                //如果剩余数量>0,作为最后一批。
                                if (remainderQty > 0)
                                {
                                    i++;
                                    lastChildLotKey = UtilHelper.GenerateNewKey(0);
                                    childTable.Rows.Add(remainderQty.ToString(), i.ToString("00"), lastChildLotKey);
                                }
                                childTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                                splitDS.Tables.Add(childTable);
                                #endregion
                                //Excute Split
                                //SplitLot(db, dbtran, splitDS); 
                                DataSet childLotReturn = new DataSet();
                                //执行分批操作。
                                SplitLotTransact(db, dbtran, splitDS, ref childLotReturn);
                                quantityToMerge = remainderQty;
                                lotKeyForMerge = lastChildLotKey;
                                #endregion
                            }
                        }
                        #region AutoMerge
                        //如果自动合批。
                        if (autoMerge.ToLower() == "true")
                        {
                            //执行自动合批操作。
                            AutoMerge(db, dbtran, lotKeyForMerge, workOrderKey, toStepKey, lineKey, quantityToMerge.ToString(), maxBoxQuantity, 0, editor, false, oprLine, shiftName);
                        }
                        #endregion
                    }
                }
                #endregion

                //检查是否需要进行锁定。
                int nHoldCount = 0;
                if (isFinished == false)
                {
                    nHoldCount = CheckAndUpdateFutureHold(dbtran, lotKey, stepKey, shiftName, shiftKey, editor,
                                            operateCompName, editTimeZone, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);

                }
                dbtran.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

                #region AutoTrackIn 确定是否自动进站。
                //如果自动进站
                if (isFinished == false && isAutoTrackIn.ToLower() == "true" && nHoldCount == 0)
                {
                    //执行自动进站作业。
                    AutoTrackIn(lotKey, string.Empty, shiftName);
                }
                #endregion
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("TrackOutLot Error: " + ex.Message);
                dbtran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }
            //记录操作时间 记录。
            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("TrackOut Lot Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
        /// <summary>
        /// 批次出站或者重工操作更新设备信息。
        /// </summary>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dataset">包含批次出站数据的数据集对象。</param>
        private void TrackOutOrReworkForEquipment(DbTransaction dbtran, DataSet dataset)
        {
            string lotKey = "", stepKey = "", userKey = "";
            string equipmentKey = "";
            if (dataset.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
            {
                DataTable mainTable = dataset.Tables[TRANS_TABLES.TABLE_PARAM];
                Hashtable hashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(mainTable);
                lotKey = hashData[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                stepKey = hashData[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY].ToString();
                userKey = hashData[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
            }
            if (dataset.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable equTable = dataset.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                Hashtable equHashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(equTable);
                equipmentKey = equHashData[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
            }
            WipManagement.TrackOutForEquipment(lotKey, stepKey, equipmentKey, userKey, dbtran);
        }
        /// <summary>
        /// 采集不良数据。
        /// </summary>
        /// <param name="dbTran">数据操作事务对象。</param>
        /// <param name="ds">包含报废数据的数据集对象。</param>
        private void SetDefect(DbTransaction dbTran, DataSet ds)
        {
            DataTable dataTable = ds.Tables[TRANS_TABLES.TABLE_PARAM];
            Hashtable hashData = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
            string lotKey           = Convert.ToString(hashData[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            string strWorkOrderKey  = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY]);
            string strLineKey       = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            string strOprLine       = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
            string strEditor        = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            string strOperator      = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            string strShiftName     = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            string strEditTime      = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME]);
            string shiftKey         = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            string computeName      = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            string stateFlag        = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG]);
            string reworkFlag       = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG]);
            //string comment        = Convert.ToString(hashData[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]); //Q.001
            string comment = Convert.ToString(hashData["DEFECTCOMMET"]); //Q.001

            DataTable table = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME];
            string sql = string.Format(@"SELECT LOT_SEQ,LOT_NUMBER FROM POR_LOT WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
            IDataReader readerSEQ = db.ExecuteReader(CommandType.Text, sql);
            int seq = 0;
            //获取当前批次对应的返工和退库记录数。
            string lotNumber = string.Empty;
            if (readerSEQ.Read())
            {
                if (readerSEQ["LOT_SEQ"].ToString() != "")
                {
                    seq = Int32.Parse(readerSEQ["LOT_SEQ"].ToString());
                }
                lotNumber = readerSEQ[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
            }
            readerSEQ.Close();
            readerSEQ.Dispose();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i][WST_STORE_MAT_FIELDS.FIELD_ITEM_NO] = lotNumber + (seq + i + 1).ToString("00");
                table.Rows[i][WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = strShiftName;
            }

            //插入批次对应的返工和退库记录。
            WipManagement.InsertIntoStoreMat(db, dbTran, ds);

            //更新批次对应的返工和退库记录数。
            sql = string.Format(@"UPDATE POR_LOT SET LOT_SEQ='{0}' WHERE LOT_KEY='{1}'", 
                                seq + table.Rows.Count, lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

            //如果包含缺陷代码数据表。设置了先区分不良代码再入库。
            if (ds.Tables.Contains(WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtStoreMat = ds.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtDefect = ds.Tables[WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME];

                string rowKey = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_ROW_KEY].ToString();
                string storeType = dtStoreMat.Rows[0][WST_STORE_FIELDS.FIELD_STORE_TYPE].ToString();
                string lineKey = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_LINE_KEY].ToString();
                string stepKey = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_STEP_KEY].ToString();
                string routeKey = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_ROUTE_KEY].ToString();
                string enterpriseKey = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_ENTERPRISE_KEY].ToString();
                string quantity = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_ITEM_QTY].ToString();
                string editor = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_EDITOR].ToString();
                string editTimeZone = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                string workorderNumber = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_WORKORDER_NUMBER].ToString();
                string storeKey = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_STORE_KEY].ToString();
                string storeName = dtStoreMat.Rows[0][WST_STORE_FIELDS.FIELD_STORE_NAME].ToString();
                string objectStatus = dtStoreMat.Rows[0][WST_STORE_MAT_FIELDS.FIELD_OBJECT_STATUS].ToString();

                #region RW_STORE_TRANSACTION
                Hashtable htReworkLotInStore = new Hashtable();
                DataTable dtReworkLotInStore = new DataTable();

                htReworkLotInStore.Add(WST_STORE_FIELDS.FIELD_STORE_TYPE, storeType);
                htReworkLotInStore.Add(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                htReworkLotInStore.Add(WST_STORE_MAT_FIELDS.FIELD_ROW_KEY, rowKey);
                htReworkLotInStore.Add(WST_STORE_MAT_FIELDS.FIELD_LINE_KEY, lineKey);
                htReworkLotInStore.Add(WST_STORE_MAT_FIELDS.FIELD_STEP_KEY, stepKey);
                htReworkLotInStore.Add(WST_STORE_MAT_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htReworkLotInStore.Add(WST_STORE_MAT_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htReworkLotInStore.Add(WST_STORE_MAT_FIELDS.FIELD_ITEM_QTY, quantity);

                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, strOperator);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, computeName);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, strOprLine);
                htReworkLotInStore.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, comment);

                dtReworkLotInStore = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(htReworkLotInStore);
                dtReworkLotInStore.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                DataSet dsReworkLotInStore = new DataSet();
                dsReworkLotInStore.Tables.Add(dtReworkLotInStore);
                dsReworkLotInStore.Merge(dtDefect);
                StoreEngine.ReworkLotInStore(db, dbTran, dsReworkLotInStore);
                #endregion

                #region RWOUTSTORE
                //ReworkLot
                DataTable dtReworkLotDetail = new DataTable();
                dtReworkLotDetail.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dtReworkLotDetail.Columns.Add("ROW_KEY");
                dtReworkLotDetail.Columns.Add("LOT_NUMBER");
                dtReworkLotDetail.Columns.Add("LINE_KEY");
                dtReworkLotDetail.Columns.Add("LINE_NAME");
                dtReworkLotDetail.Columns.Add("ITEM_QTY");
                dtReworkLotDetail.Columns.Add("EDIT_TIME");
                dtReworkLotDetail.Columns.Add("STEP_KEY");
                dtReworkLotDetail.Columns.Add("ROUTE_KEY");
                dtReworkLotDetail.Columns.Add("ENTERPRISE_KEY");
                dtReworkLotDetail.Columns.Add(WST_STORE_MAT_FIELDS.FIELD_BALANCE_QTY);
                dtReworkLotDetail.Columns.Add(WST_STORE_MAT_FIELDS.FIELD_BALANCE_EDITOR);
                dtReworkLotDetail.Columns.Add(POR_LOT_FIELDS.FIELD_LOT_KEY);
                dtReworkLotDetail.Rows.Add(rowKey, lotNumber, lineKey, strOprLine, quantity, strEditTime,
                        stepKey, routeKey, enterpriseKey, 0, string.Empty, lotKey);
                DataSet dsReworkLot = new DataSet();
                dsReworkLot.Tables.Add(dtReworkLotDetail);

                Hashtable htReworkLot = new Hashtable();
                DataTable dtReworkLot = new DataTable();

                htReworkLot.Add(WST_STORE_FIELDS.FIELD_STORE_NAME, storeName);
                htReworkLot.Add(WST_STORE_FIELDS.FIELD_STORE_TYPE, storeType);
                htReworkLot.Add(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY, strWorkOrderKey);
                htReworkLot.Add(WST_STORE_MAT_FIELDS.FIELD_STORE_KEY, storeKey);
                htReworkLot.Add(WST_STORE_MAT_FIELDS.FIELD_WORKORDER_NUMBER, workorderNumber);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, string.Empty);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, strOperator);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, strShiftName);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, computeName);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, strOprLine);
                htReworkLot.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, comment);
                htReworkLot.Add("REWORK_QTY", quantity);
                dtReworkLot = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(htReworkLot);
                dtReworkLot.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                dsReworkLot.Tables.Add(dtReworkLot);
                StoreEngine.ReworkLot(db, dbTran, dsReworkLot);
                #endregion
            }
        }
    }
}
