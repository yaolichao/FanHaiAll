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
using FanHai.Hemera.Share.Interface.EquipmentManagement;



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
         /// <summary>
        /// 批次自动过站。
        /// </summary>
        /// <param name="rowKey">WIP_JOB表主键。</param>
        /// <returns>包含自动过站执行结果的数据集。</returns>
        public DataSet AutoTrackOut(string rowKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string msg = string.Empty;
                //获取批次自动过站任务记录
                string sql = string.Format(@"SELECT * FROM WIP_JOB WHERE ROW_KEY='{0}'", rowKey);
                DataSet dsJob = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsJob==null || dsJob.Tables.Count <= 0 || dsJob.Tables[0].Rows.Count <= 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "WIP_JOB表主键对应的记录不存在。");
                    return dsReturn;
                }
                string equipmentKey = dsJob.Tables[0].Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                string enterpriseKey = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_ENTERPRISE_KEY].ToString();
                string routeKey = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_ROUTE_KEY].ToString();
                string stepKey = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_STEP_KEY].ToString();
                string lotKey = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_LOT_KEY].ToString();
                string editor = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_EDITOR].ToString();
                string editTimeZone = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_EDIT_TIMEZONE].ToString();
                string strToUser = dsJob.Tables[0].Rows[0][WIP_JOB_FIELDS.FIELDS_EDITOR].ToString();
                //获取自动出站的批次信息。
                ILotEngine lotEngine=RemotingServer.ServerObjFactory.Get<ILotEngine>();
                DataSet dsLots = lotEngine.GetLotInfo(lotKey);
                msg = ReturnMessageUtils.GetServerReturnMessage(dsLots);
                if (!string.IsNullOrEmpty(msg)
                    || dsLots==null
                    || dsLots.Tables.Count<=0
                    || dsLots.Tables[0].Rows.Count<=0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                    WipManagement.RecordErrorMessage(db, "批次出站异常：获取批次信息失败，", msg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                    return dsReturn;
                }
                DataRow drLot=dsLots.Tables[0].Rows[0];
                int stateFlag = Convert.ToInt32(drLot[POR_LOT_FIELDS.FIELD_STATE_FLAG]);                    
                if (stateFlag == 9 || stateFlag == 4)
                {
                    DataSet dsParams=new DataSet();
                    //获取下一个工步数据。
                    IEnterpriseEngine enterpriseEngine=RemotingServer.ServerObjFactory.Get<IEnterpriseEngine>();
                    DataSet dsRouteNextStep = enterpriseEngine.GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                    msg = ReturnMessageUtils.GetServerReturnMessage(dsRouteNextStep);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                        WipManagement.RecordErrorMessage(db, "批次出站异常：获取下一工艺流程失败，", msg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                        return dsReturn;
                    }
                    string enterpriseName=Convert.ToString(drLot[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    string routeName=Convert.ToString(drLot[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    string stepName=Convert.ToString( drLot[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                    string toEnterpriseKey=enterpriseKey;
                    string toRouteKey=routeKey;
                    string toStepKey=stepKey;
                    string toEnterpriseName=enterpriseName;
                    string toRouteName=routeName;
                    string toStepName=stepName;
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
                    //组织下一工步数据。
                    Hashtable htStepTransaction = new Hashtable();
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, editTimeZone);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, "SYSTEM");
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);
                    DataTable dtStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                    dtStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                    dsParams.Tables.Add(dtStepTransaction);
                    //组织操作数据。
                    string shiftName = RemotingServer.ServerObjFactory.Get<IShift>().GetShiftNameBySysdate();
                    string shiftKey = RemotingServer.ServerObjFactory.Get<IShift>().IsExistsShift(shiftName);
                    Hashtable htTransaction = new Hashtable();
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, drLot[POR_LOT_FIELDS.FIELD_QUANTITY]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, drLot[POR_LOT_FIELDS.FIELD_QUANTITY]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, drLot[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, editor);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, "SYSTEM");
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, drLot[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, drLot[POR_LOT_FIELDS.FIELD_OPR_LINE]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLot[POR_LOT_FIELDS.FIELD_OPR_LINE]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, drLot[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, "SYSTEM");
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY,editTimeZone);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                    DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
                    dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                    dsParams.Tables.Add(dtTransaction);
                    //组织其他附加参数数据
                    Hashtable htMaindata = new Hashtable();
                    string lotEditTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                    htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, lotEditTime);
                    IRouteEngine routeEngine=RemotingServer.ServerObjFactory.Get<IRouteEngine>();
                    DataSet dsStepBaseData = routeEngine.GetStepBaseDataAndParamDataByKey(stepKey, 0);
                    msg = ReturnMessageUtils.GetServerReturnMessage(dsStepBaseData);
                    if (!string.IsNullOrEmpty(msg)
                        || dsStepBaseData==null
                        || dsStepBaseData.Tables.Count<=0
                        || dsStepBaseData.Tables[0].Rows.Count<=0)
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                        WipManagement.RecordErrorMessage(db, "批次出站异常：获取当前工艺流程数据失败，", msg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                        return dsReturn;
                    }
                    string operationKey = Convert.ToString(dsStepBaseData.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
                    string duration = Convert.ToString(dsStepBaseData.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DURATION]);
                    string partNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                    htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
                    htMaindata.Add(POR_ROUTE_STEP_FIELDS.FIELD_DURATION, duration);
                    htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                    htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                    htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, partNumber);
                    htMaindata.Add(ROUTE_OPERATION_ATTRIBUTE.IsShowSetNewRoute, false);
                    DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
                    dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
                    dsParams.Tables.Add(dtParams);
                    dsReturn = LotTrackOut(dsParams);
                    msg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (string.IsNullOrEmpty(msg))
                    {
                        sql = @"UPDATE WIP_JOB SET JOB_STATUS=1 WHERE ROW_KEY='" + rowKey.PreventSQLInjection() + "'";
                        db.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else
                    {
                        WipManagement.RecordErrorMessage(db, "批次出站异常", msg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                    }
                }
                else
                {
                    msg = "批次" + dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString() + "抽检未完成";
                    WipManagement.RecordErrorMessage(db, "批次出站异常", msg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                    //更新WIP_JOB表。
                    sql = string.Format(@"UPDATE WIP_JOB
                                        SET JOB_NEXTRUNTIME = DATEADD(mi,5,GETDATE()),JOB_RUNACCOUNT=JOB_RUNACCOUNT +1
                                        WHERE ROW_KEY = '{0}'", rowKey.PreventSQLInjection());
                    db.ExecuteNonQuery(CommandType.Text, sql);
                }
                if (msg.Length > 0)
                {
                    msg =string.Format("批次[{0}] 自动出站失败.\n\t原因:{1}",drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER],msg);
                }
                else
                {
                    msg = string.Format("批次[{0}] 自动出站成功.", drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AutoTrackOut Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
