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
using SolarViewer.Hemera.Share.Common;

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 批次自动过站。
        /// </summary>
        /// <param name="rowKey">WIP_JOB表主键。</param>
        /// <returns>包含自动过站执行结果的数据集。</returns>
        public DataSet AutoTrackOut(string rowKey)
        {
            EnterpriseEngine enterpriseEngine = new EnterpriseEngine();
            DataSet dsReturn = new DataSet();
            DataSet dsAutoActionReturn = new DataSet();
            DataSet dsLotInfo = new DataSet();
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainDataTable = new DataTable();
            DataSet dataSet = new DataSet();
            string equipmentKey = "", enterpriseKey = "", routeKey = "", stepKey = "", lotKey = "";
            string nextEnterpriseKey = "", nextRouteKey = "", nextStepKey = "", editor = "", editTimeZone = "";
            string isFinished = "False", strToUser = "";
            string msg = "", strAutoReturnMsg = "";
            try
            {
                #region 获取批次自动过站任务记录
                string sql = string.Format(@"SELECT * FROM WIP_JOB WHERE ROW_KEY='{0}'", rowKey);
                dsLotInfo = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsLotInfo.Tables.Count > 0 && dsLotInfo.Tables[0].Rows.Count > 0)
                {
                    equipmentKey = dsLotInfo.Tables[0].Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                    enterpriseKey = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_ENTERPRISE_KEY].ToString();
                    routeKey = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_ROUTE_KEY].ToString();
                    stepKey = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_STEP_KEY].ToString();
                    lotKey = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_LOT_KEY].ToString();
                    editor = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_EDITOR].ToString();
                    editTimeZone = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_EDIT_TIMEZONE].ToString();
                    strToUser = dsLotInfo.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_EDITOR].ToString();
                }
                #endregion

                #region TrackOutLot
                DataSet dsLots = GetLotsInfo(lotKey);
                if (dsLots != null && dsLots.Tables.Count > 0 && dsLots.Tables[0].Rows.Count > 0)
                {
                    string stateFage = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                    if (stateFage == "9" || stateFage == "4")
                    {
                        #region get lot detail information form por_lot
                        mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                        //mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_MODULE, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MODULE].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_SHIFT_NAME].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);

                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPERATOR].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_COMPUTER].ToString());

                        #endregion

                        #region equipment information
                        if (equipmentKey != string.Empty)
                        {
                            Hashtable equHashData = new Hashtable();
                            DataTable equDataTable = new DataTable();
                            equHashData.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                            equDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(equHashData);
                            equDataTable.TableName = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                            dataSet.Tables.Add(equDataTable);
                        }
                        #endregion

                        #region get next route and step
                        if (enterpriseKey != string.Empty && routeKey != string.Empty && stepKey != string.Empty)
                        {
                            Hashtable hashTable = new Hashtable();
                            DataTable dataTable = new DataTable();
                            DataSet dsOperation = new DataSet();
                            hashTable.Add("ROUTE_ENTERPRISE_VER_KEY", enterpriseKey);
                            hashTable.Add("ROUTE_ROUTE_VER_KEY", routeKey);
                            hashTable.Add("ROUTE_STEP_KEY", stepKey);
                            dataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                            dsOperation.Tables.Add(dataTable);
                            DataSet dsReturnOperation = enterpriseEngine.GetEnterpriseNextRouteAndStep(dsOperation);
                            string errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturnOperation);
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsAutoActionReturn, errorMsg);
                                RecordErrorMessage("批次出站异常：获取下一工艺流程失败，", errorMsg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                                return dsAutoActionReturn;
                            }
                            else
                            {
                                if (null == dsReturnOperation 
                                    || dsReturnOperation.Tables.Count==0
                                    || dsReturnOperation.Tables[0].Rows.Count==0)
                                {
                                    nextEnterpriseKey = enterpriseKey;
                                    nextRouteKey = routeKey;
                                    nextStepKey = stepKey;
                                    isFinished = "True";
                                }
                                else
                                {
                                    if (dsReturnOperation.Tables.Count > 0 && dsReturnOperation.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME))
                                    {
                                        DataTable dtRouteStep = dsReturnOperation.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME];
                                        if (dtRouteStep.Rows.Count > 0)
                                        {
                                            nextStepKey = dtRouteStep.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY].ToString();
                                            nextRouteKey = dtRouteStep.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
                                            nextEnterpriseKey = dtRouteStep.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                                        }
                                    }
                                }
                            }
                            mainDataHashTable.Add(WIP_FIELDS.FIELDS_TO_ROUTE_VER_KEY, nextRouteKey);
                            mainDataHashTable.Add(WIP_FIELDS.FIELDS_TO_STEP_VER_KEY, nextStepKey);
                            mainDataHashTable.Add(WIP_FIELDS.FIELDS_TO_ENTERPRISE_VER_KEY, nextEnterpriseKey);

                            mainDataHashTable.Add("LAST_STEP", isFinished);
                            //mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_QUANTITY_REWORK, "0");

                        }
                        #endregion

                        #region 执行出站操作
                        mainDataTable.TableName = TRANS_TABLES.TABLE_PARAM;
                        mainDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
                        dataSet.Tables.Add(mainDataTable);
                        dsReturn = TrackOutLot(dataSet);
                        msg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg == string.Empty)
                        {
                            string strSql = @"UPDATE WIP_JOB SET JOB_STATUS=1 WHERE ROW_KEY='" + rowKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(CommandType.Text, strSql);
                        }
                        else
                        {
                            //throw new Exception(msg);
                            RecordErrorMessage("批次出站异常", msg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                        }

                        #endregion
                    }
                    else
                    {

                        string content = "批次" + dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString() + "抽检未完成";
                        msg = content;
                        RecordErrorMessage("批次出站异常", content, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                        //update wip_job
                        sql = string.Format(@"UPDATE WIP_JOB
                                            SET JOB_NEXTRUNTIME = DATEADD(mi,5,GETDATE()),JOB_RUNACCOUNT=JOB_RUNACCOUNT +1
                                            WHERE ROW_KEY = '{0}'", rowKey.PreventSQLInjection());
                        db.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    if (msg.Length > 0)
                    {
                        strAutoReturnMsg = "批次[" + dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString() + "] 自动出站失败.\n\t" +
                            "原因:" + msg;

                    }
                    else
                    {
                        strAutoReturnMsg = "批次[" + dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString() + "] 自动出站成功.";
                    }
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsAutoActionReturn, strAutoReturnMsg);
                }

                #endregion
            }
            catch (Exception ex)
            {
                strAutoReturnMsg = "批次自动出站失败.\n\t" +
                           "原因:" + ex.Message;
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsAutoActionReturn, strAutoReturnMsg);

                LogService.LogError("AutoTrackOutLot Error: " + ex.Message);
                //inser wip message
                RecordErrorMessage("自动出站异常", ex.Message, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "JOB");
            }
            return dsAutoActionReturn;
        }
    }
}
