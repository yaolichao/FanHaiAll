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
using SolarViewer.Hemera.Share.Common;
using SolarViewer.Hemera.Share.Interface;

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 执行批次进站作业。
        /// </summary>
        /// <param name="dsParams">
        /// 数据集对象。包含名称为<see cref="TRANS_TABLES.TABLE_PARAM"/>的数据表。
        /// 数据表中必须包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。</param>
        /// <returns>
        /// 包含执行结果的数据集。
        /// 0：成功。-1或1：失败。 2：需要进行数据采集。
        /// </returns>
        public DataSet TrackInLot(DataSet dsParams)
        {
            DateTime startTime =DateTime.Now;
            DataSet dsReturn = new DataSet();
            string sql = "";
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string lotKey = "",
                lineKey = "",
                workOrderKey = "",
                stepKey = "",
                quantityIn = "",
                user = "",
                editTime = "",
                lotNumber = "",
                strEquKey = "",
                strEquStateKey = "",
                strOperationKey = "",
                strIsBatch = "",
                maxQuantity = "",
                inProductQuantity = "",
                isAutoTrackOut = string.Empty,
                oprLine = string.Empty,
                shiftName = string.Empty,
                lineName = string.Empty,
                editTimeZone = string.Empty,
                shiftKey = string.Empty,
                operateCompName =  string.Empty;
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();

                //如果数据集中包含TRANS_TABLES.TABLE_PARAM的数据表
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    lotKey = htParams[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                    lotNumber = htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
                    lineKey = htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY].ToString();
                    workOrderKey = htParams[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
                    stepKey = htParams[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY].ToString();
                    quantityIn = htParams[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN].ToString();
                    user = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                    editTime = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME].ToString();
                    editTimeZone = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                    oprLine = htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE].ToString();
                    shiftName = htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME].ToString();
                    lineName = htParams[POR_LOT_FIELDS.FIELD_LINE_NAME].ToString();
                    shiftKey = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
                    operateCompName=Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
                }
                
                #region 检查记录是否过期。
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                //判断数据是否过期，如果过期，则返回执行结果为“数据已过期。”，结束方法执行。
                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, editTime))
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "数据已过期");
                    return dsReturn;
                }
                #endregion

                DateTime dtCurrent = UtilHelper.GetSysdate(db);

                #region 检查设备
                //如果数据集合中包含EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME数据表对象。
                if (dsParams.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable equDataTable = dsParams.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable equHashData =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(equDataTable);
                    strEquKey = equHashData[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                    strOperationKey = equHashData[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY].ToString();
                    //如果设备主键有值。
                    if (strEquKey != "" && strEquKey.Length > 0)
                    {
                        sql = @"SELECT a.ISBATCH,a.EQUIPMENT_STATE_KEY,a.MAXQUANTITY ,b.EQUIPMENT_STATE_TYPE,b.EQUIPMENT_STATE_NAME
                                FROM EMS_EQUIPMENTS a
                                LEFT JOIN EMS_EQUIPMENT_STATES b ON a.EQUIPMENT_STATE_KEY=b.EQUIPMENT_STATE_KEY
                                WHERE EQUIPMENT_KEY='" + strEquKey.PreventSQLInjection() + "'";
                        DataSet dsEqu = db.ExecuteDataSet(CommandType.Text, sql);//查询设备
                        //如果有数据。
                        if (dsEqu.Tables[0].Rows.Count > 0)
                        {
                            strIsBatch = Convert.ToString(dsEqu.Tables[0].Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH]);                                        //设备是否是批处理设备。
                            strEquStateKey = Convert.ToString(dsEqu.Tables[0].Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY]);                       //设备状态。
                            maxQuantity = Convert.ToString(dsEqu.Tables[0].Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY]);                                   //最大数量。
                            string strEquStateName = Convert.ToString(dsEqu.Tables[0].Rows[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME]);  //设备状态名称。
                            string strEquStateType= Convert.ToString(dsEqu.Tables[0].Rows[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE]);   //设备状态类型。
                            #region 检查设备状态
                            //如果设备状态不是“待产”
                            if (strEquStateType != "LOST")
                            {
                                //如果设备状态是“在产”
                                if (strEquStateType == "RUN")
                                {
                                    //如果是批处理设备。
                                    if (strIsBatch == "1")
                                    {
                                        //获取设备在产数量。
                                        sql = @"SELECT ISNULL(SUM(T.QUANTITY),0) AS INQUANTITY 
                                                FROM EMS_LOT_EQUIPMENT T
                                                WHERE T.EQUIPMENT_KEY = '" + strEquKey + "' AND T.END_TIMESTAMP IS NULL";
                                        DataSet dsLotEqu = db.ExecuteDataSet(CommandType.Text, sql);
                                        if (dsLotEqu.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(maxQuantity))//获取设备在产数量成功
                                        {
                                            inProductQuantity = dsLotEqu.Tables[0].Rows[0]["inQuantity"].ToString();
                                            //在产数量>设备的最大数量，结束方法执行。
                                            if (Convert.ToInt32(inProductQuantity) + Convert.ToInt32(quantityIn) > Convert.ToInt32(maxQuantity))
                                            {
                                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.Wip.WipEngine.Msg.BigThenMaxQuantity}");
                                                return dsReturn;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //如果不是批处理设备，结束方法执行。
                                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.Wip.WipEngine.Msg.BatchIsFalse}");
                                        return dsReturn;
                                    }
                                }
                                else
                                {
                                    //如果设备状态不是在产，结束方法执行。
                                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.Wip.WipEngine.Msg.EquipmentCanNotUser}");
                                    return dsReturn;
                                }
                            }
                            #endregion
                        }
                        else//获取设备数据失败。
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.Wip.WipEngine.Msg.GetEquipmentError}");
                            return dsReturn;
                        }
                    }
                }
                #endregion

                //批次进站，更新批次数据，插入批次进站操作记录。
                WipManagement.TrackInLot(db, dbtran, dsParams);
                //设备主键不为空。
                if (strEquKey != "")
                {
                    //批次和设备进行关联。
                    WipManagement.TrackInForEquipment(lotKey, Convert.ToInt32(quantityIn), strOperationKey, stepKey, strEquKey, user, dbtran);
                }

                #region 检查自动合批
                //工步主键不为空。
                if (stepKey != "")
                {
                    string isLastStep = "";
                    string isAutoMerge = "";
                    int maxBoxQuantity = -1;
                    DataSet dsStepUda = GetStepUda(stepKey);
                    //如果获取到工步自定义属性数据。
                    if (dsStepUda.Tables[0].Rows.Count > 0)
                    {
                        //遍历工序的自定义属性。
                        for (int i = 0; i < dsStepUda.Tables[0].Rows.Count; i++)
                        {
                            string szAttriName = dsStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME].ToString();
                            string szAttriValue = dsStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString();
                            //是否是最后一个工步。
                            if (szAttriName == "IsLastStep")
                            {
                                isLastStep = szAttriValue;
                            }
                            //是否自动合批。
                            if (szAttriName == "AutoMerge")
                            {
                                isAutoMerge = szAttriValue;
                            }
                            //箱子最大数量。
                            if (szAttriName == "MaxBoxQuantity" && !int.TryParse(szAttriValue, out maxBoxQuantity))
                            {
                                maxBoxQuantity = -1;
                            }
                            //是否自动出站。
                            if (szAttriName == "AutoTrackOut")
                            {
                                isAutoTrackOut = szAttriValue;
                            }
                        }
                        if (isAutoMerge.ToLower() == "true")//进行自动合批
                        {
                            AutoMerge(db, dbtran, lotKey, workOrderKey, stepKey, lineKey,
                                quantityIn, maxBoxQuantity, 4, user, false, oprLine, shiftName);
                        }
                    }
                }

                #endregion

                #region 检查自动出站
                //自动出站
                if (isAutoTrackOut.ToLower() == "true")
                {
                    DataSet dsInfo = GetLotsInfo(dbtran,lotKey);
                    //获取批次信息成功。
                    if (dsInfo.Tables.Count > 0 && dsInfo.Tables[0].Rows.Count > 0)
                    {
                        string nextRunTime = string.Empty;
                        string sDuration =Convert.ToString(dsInfo.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DURATION]);
                        double duration=0;
                        if (!double.TryParse(sDuration, out duration))
                        {
                            duration = 0; 
                        }
                        nextRunTime = dtCurrent.AddMinutes(duration).ToString("yyyy-MM-dd HH:mm:ss");
                        Hashtable hashWipJob = new Hashtable();
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_EDIT_TIMEZONE, editTimeZone);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_EDITOR, user);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_EDIT_TIME, dtCurrent);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_ENTERPRISE_KEY, dsInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString());
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_CREATETIME, dtCurrent);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_EQUIPMENT_KEY, strEquKey);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_CLOSETYPE, string.Empty);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_NEXTRUNTIME, nextRunTime);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_RUNACCOUNT, "0");
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_STATUS, "0");
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_TYPE, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_LINE_NAME, lineName);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_LOT_KEY, lotKey);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_LOT_NUMBER, lotNumber);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_NOTIFY_USER, string.Empty);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_ROUTE_KEY, dsInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_ROW_KEY, UtilHelper.GenerateNewKey(0));
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_STEP_KEY, dsInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                        hashWipJob.Add(WIP_JOB_FIELDS.FIELDS_WORKORDER_NUMBER, dsInfo.Tables[0].Rows[0][POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                        //插入一笔自动过站任务。
                        WipJobAutoTrack.InsertWipJob(db, dbtran, hashWipJob);
                    }
                }
                #endregion

                //检查是否需要进行数据采集。
                string edcPointKey = string.Empty;
                if (CheckNeedEdc(dbtran,lotKey, strEquKey, out edcPointKey))
                {
                    IEDCEngine edcEngine = RemotingServer.ServerObjFactory.Get<IEDCEngine>();
                    edcEngine.SaveEdcMainInfo(dbtran, lotKey, edcPointKey, user, strEquKey, oprLine, shiftKey);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, "批次" + lotNumber + "需要抽检");
                }
                else
                {
                    //更新批次状态为9（WaitingForTrackout）
                    UpdateLotState(dbtran, lotKey, 9);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, string.Empty);
                }
                //检查是否需要进行锁定。
                CheckAndUpdateFutureHold(dbtran, lotKey, stepKey, shiftName, shiftKey, user,
                                        operateCompName, editTimeZone, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);

                dbtran.Commit();
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "批次进站出错" + ex.Message);
                LogService.LogError("TrackInLot Error: " + ex.Message);
                dbtran.Rollback();
            }
            finally
            {
                dbconn.Close();
            }
            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("TrackIn Lot Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 检查批次是否需要进行数据采集。
        /// </summary>
        /// <param name="dbTrans"></param>
        /// <param name="lotKey">唯一标识批次的主键或批次号。</param>
        /// <param name="equipmentKey">唯一表示设备的主键。</param>
        /// <param name="edcpntKey">数据抽检设置主键。输出参数。</param>
        /// <returns>true：批次需要抽检。false：批次不需要抽检。</returns>
        private bool CheckNeedEdc(DbTransaction dbTrans,string lotKey, string equipmentKey, out string edcpntKey)
        {
            edcpntKey = string.Empty;
            DataSet dsLot = GetLotsInfo(dbTrans,lotKey);
            string msg = ReturnMessageUtils.GetServerReturnMessage(dsLot);
            if (!string.IsNullOrEmpty(msg))
            {
                throw new Exception(msg);
            }
            //获取批次信息
            DataTable dtLot = dsLot.Tables[0];
            string routeKey = dtLot.Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();            //工艺流程主键。
            string stepKey = dtLot.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();              //工步主键
            string oprLine = dtLot.Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();                      //操作线别
            string partNumber=dtLot.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER].ToString();           //成品号
            string operationName= dtLot.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();   //工序名称
            IEDCEngine edcEngine = RemotingServer.ServerObjFactory.Get<IEDCEngine>();
            Hashtable hashTable = new Hashtable();
            hashTable.Add(POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER, partNumber);
            hashTable.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME,operationName);
            hashTable.Add(EDC_POINT_FIELDS.FIELD_ACTION_NAME, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
            hashTable.Add(EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY, routeKey);
            if (equipmentKey != string.Empty)
            {
                hashTable.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            }
            DataTable dataTable = new DataTable();
            dataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            //获取数据抽检点。
            DataSet dsEdcPoint = edcEngine.CheckEdc(dataTable);
            msg =SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsEdcPoint);
            if (msg == string.Empty) //成功获取到数据抽检点集合。
            {
                string samplingKey = string.Empty;
                //获取的数据抽检点集合存在数据。
                if (dsEdcPoint.Tables.Count > 0 && dsEdcPoint.Tables[0].Rows.Count > 0)
                {
                    string edcPointKey = dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_ROW_KEY].ToString();
                    //抽检参数没有满足条件的，则不进行抽检。
                    if (!edcEngine.CheckEDCPointParams(edcPointKey))
                    {
                        return false;
                    }
                    string edcKey = dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_EDC_KEY].ToString();
                    if (dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_SP_KEY] != null)
                    {
                        samplingKey = dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_SP_KEY].ToString();
                    }
                    bool blNeedSamp = false;
                    //获取当前工步是否有依赖的抽样工步。
                    string dependSampStep = GetStepUdaValue(db, stepKey, "DependSampStep");
                    //依赖的抽样工步不为空
                    if (dependSampStep != string.Empty && dependSampStep.Length > 0)
                    {
                        //检查是否进行了抽检。true 进行抽检，false没有进行抽检。
                        //如果该批次在依赖的抽样工步进行了抽检，则该批次在当前工步必须抽检。
                        blNeedSamp = edcEngine.CheckDependSampStep(lotKey, dependSampStep);
                    }
                    //依赖的抽样工步没有进行抽检 并且 依赖抽检的工步为空。
                    if (blNeedSamp == false && dependSampStep == string.Empty)
                    {
                        //如果抽样规则主键为空，则需要进行抽检。
                        if (samplingKey == string.Empty)
                        {
                            blNeedSamp = true;
                        }
                        else
                        {//如果抽样规则主键不为空，则需要根据抽检规则进行判断是否进行抽检。
                            blNeedSamp = edcEngine.CheckSampling(
                                            lotKey,
                                            stepKey,
                                            dtLot.Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString(),
                                            samplingKey,
                                            ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC,
                                            equipmentKey);
                        }
                    }
                    //上面判断需要进行抽检。
                    if (blNeedSamp)
                    {
                        edcpntKey = edcPointKey;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //批次不需要抽检
            return false;
        }
        /// <summary>
        /// 检查是否要HOLD批次。如果需要HOLD批次，则HOLD批次。
        /// </summary>
        /// <param name="dbtran">数据库事务对象。。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="shiftName">班别名称。</param>
        /// <param name="shiftKey">班别主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <param name="oprComputer">操作计算机名称。</param>
        /// <param name="editTimeZone">编辑时间时区。</param>
        /// <param name="action">动作名称。<see cref="COMMON_FIELDS.FIELD_ACTIVITY_TRACKIN"/>或者<see cref="COMMON_FIELDS.FIELD_ACTIVITY_TRACKOUT"/></param>
        /// <returns>触发预设暂停的个数。0：没有批次被HOLD。>0：该批次被HOLD。</returns>
        private int CheckAndUpdateFutureHold(DbTransaction dbtran, string lotKey, string stepKey,
                                            string shiftName,string shiftKey,
                                            string editor, string oprComputer, string editTimeZone, string action)
        {
            string sql = @"SELECT * FROM WIP_FUTUREHOLD 
                           WHERE LOT_KEY=@lotKey AND STEP_KEY=@stepKey AND STATUS=1 AND ACTION_NAME=@action
                           ORDER BY EDIT_TIME DESC";
            DbCommand dbCmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(dbCmd, "lotKey", DbType.String, lotKey);
            db.AddInParameter(dbCmd, "stepKey", DbType.String, stepKey);
            db.AddInParameter(dbCmd, "action", DbType.String, action);
            DataSet dsFutureHold = db.ExecuteDataSet(dbCmd, dbtran);
            int nRet = 0;
            if (dsFutureHold != null && dsFutureHold.Tables.Count > 0 && dsFutureHold.Tables[0].Rows.Count > 0)
            {
                DataTable dtFutureHold = dsFutureHold.Tables[0];
                DataRow drFutureHold = dtFutureHold.Rows[0];

                string transactionKey = UtilHelper.GenerateNewKey(0);
                string rcCodeKey = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY]);
                string rcCodeName = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE]);
                string comment = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK]);
                string opUser = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_EDITOR]);

                Hashtable htParams = new Hashtable();
                htParams[POR_LOT_FIELDS.FIELD_LOT_KEY] = lotKey;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                htParams[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY] = rcCodeKey;
                htParams[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME] = rcCodeName;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = comment;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = shiftName;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY] = shiftKey;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER] = oprComputer;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR] = opUser;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = opUser;
                htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = editTimeZone;
                DataTable dtParams = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParams);
                dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                DataSet dsParams = new DataSet();
                dsParams.Tables.Add(dtParams);

                WipManagement.HoldLot(db, dbtran, dsParams);

                sql = @"UPDATE WIP_FUTUREHOLD 
                        SET STATUS=0,EDITOR=@editor,EDIT_TIME= GETDATE()
                        WHERE LOT_KEY=@lotKey AND STEP_KEY=@stepKey AND STATUS=1 AND ACTION_NAME=@action";

                dbCmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCmd, "lotKey", DbType.String, lotKey);
                db.AddInParameter(dbCmd, "stepKey", DbType.String, stepKey);
                db.AddInParameter(dbCmd, "action", DbType.String, action);
                db.AddInParameter(dbCmd, "editor", DbType.String, editor);
                nRet=db.ExecuteNonQuery(dbCmd, dbtran);
            }
            return nRet;
        }
    }
}
