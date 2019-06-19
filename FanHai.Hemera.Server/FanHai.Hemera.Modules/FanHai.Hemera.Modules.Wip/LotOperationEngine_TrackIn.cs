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
using FanHai.Hemera.Modules.EMS;
using FanHai.Hemera.Modules.WipJob;



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        public bool CheckCalibrationCycle(string proId, string deviceCode)
        {
            using (DbConnection dbCon = db.CreateConnection())
            {
                dbCon.Open();
                using (DbTransaction dbTrans = dbCon.BeginTransaction())
                {
                    return CheckCalibrationCycle(dbTrans, string.Empty, proId, deviceCode);
                }
            }
        }
        /// <summary>
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        public bool CheckCalibrationCycle(string lotNumber, string proId, string deviceCode)
        {
            using (DbConnection dbCon = db.CreateConnection())
            {
                dbCon.Open();
                using (DbTransaction dbTrans = dbCon.BeginTransaction())
                {
                    return CheckCalibrationCycle(dbTrans, lotNumber, proId, deviceCode);
                }
            }
        }
        /// <summary>
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="dbTrans">数据库事务操作对象。</param>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        private bool CheckCalibrationCycle(DbTransaction dbTrans, string lotNumber, string proId, string deviceCode)
        {
            //根据设备代码抓取校准版号、当前时间以及最后测试时间。
            string sql = string.Format(@"SELECT CALIBRATION_NO,CALIBRATION_TIME,GETDATE() CUR_TIME
                                    FROM WIP_CALIBRATION_INFO
                                    WHERE MACHINE_NO='{0}'",
                                    deviceCode.PreventSQLInjection());
            DataSet dsCalibrationCycle = db.ExecuteDataSet(dbTrans, CommandType.Text, sql);
            if (null == dsCalibrationCycle
                || dsCalibrationCycle.Tables.Count <= 0
                || dsCalibrationCycle.Tables[0].Rows.Count <= 0)
            {
                return true;
            }
            DataRow drCalibration = dsCalibrationCycle.Tables[0].Rows[0];
            DateTime dtCalibrationTime = Convert.ToDateTime(drCalibration[WIP_CALIBRATION_INFO_FIELDS.FIELDS_CALIBRATION_TIME]);
            DateTime dtCurTime = Convert.ToDateTime(drCalibration["CUR_TIME"]);
            double curCyclye = (dtCurTime - dtCalibrationTime).TotalMinutes;
            //根据产品ID号抓取校准板时间周期
            sql = string.Format(@"IF EXISTS(SELECT 1 
                                          FROM POR_WO_PRD a
                                          INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND c.PART_NUMBER=a.PART_NUMBER
                                          WHERE c.LOT_NUMBER='{0}'
                                          AND a.IS_USED='Y')
                                BEGIN
                                    SELECT a.CALIBRATION_CYCLE
                                    FROM POR_WO_PRD a
                                    INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND a.PART_NUMBER=c.PART_NUMBER
                                    WHERE c.LOT_NUMBER='{0}'
                                    AND a.IS_USED='Y';    
                                END
                                ELSE
                                BEGIN
                                    SELECT CALIBRATION_CYCLE
                                    FROM POR_PRODUCT
                                    WHERE PRODUCT_CODE='{1}' and ISFLAG=1;
                                END",
                                lotNumber.PreventSQLInjection(),
                                proId.PreventSQLInjection());
            string obj = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
            double cycle = 0;
            if (string.IsNullOrEmpty(obj) || double.TryParse(obj, out cycle) == false)
            {
                cycle = 0;
            }
            //当前时间-最后测试时间<校准板时间周期，没有超过校准板周期。
            if (curCyclye < cycle)
            {
                return false;
            }
            //否则当前时间-最后测试时间>校准板时间周期
            return true;
        }
        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        public bool CheckFixCycle(string lineKey, string proId, DateTime waitTrackInTime)
        {
            using (DbConnection dbCon = db.CreateConnection())
            {
                dbCon.Open();
                using (DbTransaction dbTrans = dbCon.BeginTransaction())
                {
                    return CheckFixCycle(dbTrans, string.Empty, lineKey, proId, waitTrackInTime);
                }
            }
        }
        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        public bool CheckFixCycle(string lotNumber, string lineKey, string proId, DateTime waitTrackInTime)
        {
            using (DbConnection dbCon = db.CreateConnection())
            {
                dbCon.Open();
                using (DbTransaction dbTrans = dbCon.BeginTransaction())
                {
                    return CheckFixCycle(dbTrans, lotNumber, lineKey, proId, waitTrackInTime);
                }
            }
        }

        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="dbTrans">数据库事务操作对象。</param>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        private bool CheckFixCycle(DbTransaction dbTrans, string lotNumber, string lineKey, string proId, DateTime waitTrackInTime)
        {
            //根据LINE KEY抓取对应的固化周期
            string sql = string.Format(@"SELECT ATTRIBUTE_VALUE
                                    FROM BASE_ATTRIBUTE_VALUE
                                    WHERE OBJECT_TYPE='{0}'
                                    AND OBJECT_KEY='{1}'
                                    AND ATTRIBUTE_NAME='{2}'",
                                    FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                    lineKey.PreventSQLInjection(),
                                    LINE_ATTRIBUTE.FixCycle);
            string obj = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
            double cycle = 0;
            if (string.IsNullOrEmpty(obj) || double.TryParse(obj, out cycle) == false)
            {
                cycle = 0;
            }
            //如果没有设置固化周期，根据产品ID号抓取固化周期
            if (cycle == 0)
            {
                //根据产品ID号抓取固化时间周期
                sql = string.Format(@"IF EXISTS(SELECT 1 
                                                  FROM POR_WO_PRD a
                                                  INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND c.PART_NUMBER=a.PART_NUMBER
                                                  WHERE c.LOT_NUMBER='{0}'
                                                  AND a.IS_USED='Y')
                                        BEGIN
                                            SELECT a.FIX_CYCLE
                                            FROM POR_WO_PRD a
                                            INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND a.PART_NUMBER=c.PART_NUMBER
                                            WHERE c.LOT_NUMBER='{0}'
                                            AND a.IS_USED='Y';    
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT FIX_CYCLE
                                            FROM POR_PRODUCT
                                            WHERE PRODUCT_CODE='{1}' and ISFLAG=1;
                                        END",
                                        lotNumber.PreventSQLInjection(),
                                        proId.PreventSQLInjection());
                obj = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                if (string.IsNullOrEmpty(obj) || double.TryParse(obj, out cycle) == false)
                {
                    cycle = 0;
                }
            }
            //当前时间-等待进站时间>=固化周期
            DateTime dtCurTime = UtilHelper.GetSysdate(db);
            double curCycle = (dtCurTime - waitTrackInTime).TotalMinutes;
            if (curCycle >= cycle)
            {
                return true;
            }
            //当前时间-等待进站时间<固化周期
            return false;
        }
        /// <summary>
        /// 根据批次主键获取批次进站是否超时。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// true表示超时，false表示没有超时。
        /// </returns>
        public bool GetLotTrackInIsDelay(string lotKey)
        {
            bool bIsDelay = false;
            try
            {
                long iMaxTrackInTime = 60;
                string sql = string.Format(@"SELECT SYSDATETIME() AS CURTIME,ISNULL(START_WAIT_TIME,SYSDATETIME()) START_WAIT_TIME,CUR_STEP_VER_KEY
                                         FROM POR_LOT
                                         WHERE LOT_KEY = '{0}'",
                                         lotKey.PreventSQLInjection());
                DataSet dsLotInfo = db.ExecuteDataSet(CommandType.Text, sql);
                //获取批次信息成功。
                if (dsLotInfo.Tables.Count > 0 && dsLotInfo.Tables[0].Rows.Count > 0)
                {
                    DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];
                    DateTime startWaitTime = Convert.ToDateTime(drLotInfo["START_WAIT_TIME"]);
                    DateTime curTime = Convert.ToDateTime(drLotInfo["CURTIME"]);
                    //分钟数
                    double its = (curTime - startWaitTime).TotalMinutes;  //unit is minute

                    string stepKey = Convert.ToString(drLotInfo["CUR_STEP_VER_KEY"]);
                    if (!string.IsNullOrEmpty(stepKey))
                    {
                        //查询当前工步的时间控制
                        sql = string.Format(@"SELECT T.MAXTRACKINTIME
                                            FROM V_ROUTE_STEP_TIMECONTROL T 
                                            WHERE T.ROUTE_STEP_KEY='{0}'",
                                            stepKey.PreventSQLInjection());
                        iMaxTrackInTime = Convert.ToInt64(db.ExecuteScalar(CommandType.Text, sql));
                    }
                    //如果批次进站等待分钟数，大于批次最大进站等待时间，表示超时。
                    if (its > iMaxTrackInTime)
                    {
                        bIsDelay = true;
                    }
                    else
                    {//否则，表示没有超时。
                        bIsDelay = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetLotTrackInIsDelay Error: " + ex.Message);
                throw ex;
            }
            return bIsDelay;
        }

        /// <summary>
        /// 进站后自动出站。
        /// </summary>
        /// <param name="dsParams">包含出站信息的数据集对象。</param>
        private string TrackInAutoTrackOut(DataSet dsParams)
        {
            const string TITLE = "批次出站异常";
            const string OBJECT_TYPE = "LOT";
            const string TO_GROUP = "EMSGOUT";
            string msg = string.Empty;

            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据

            Hashtable htAutoTrackOutTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);

            string operationKey = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);  //工序主键

            string lotKey = Convert.ToString(htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            double leftQty = Convert.ToDouble(htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);//进站后数量
            string editor = Convert.ToString(htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);//编辑人
            string editTimeZone = Convert.ToString(htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);//编辑时区

            //重新获取批次信息。
            DataSet dsLotInfo = RemotingServer.ServerObjFactory.CreateILotEngine().GetLotInfo(lotKey);
            DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];

            //获取批次对应信息
            string lineKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);                 //批次线别主键
            string lineName = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);                              //批次线别名称
            string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);          //工艺流程组主键
            string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]); //工艺流程名称
            string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);                      //工艺流程主键
            string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);                //工艺流程名称
            string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);                        //工步主键
            string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);                 //工步名称
            string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);               //设备主键

            //下一工步信息
            string toEnterpriseKey = string.Empty;//下一个工艺流程组主键。
            string toRouteKey = string.Empty;//下一个工艺流程主键。
            string toStepKey = string.Empty;//下一个工步主键。
            string toEnterpriseName = string.Empty;//下一个工艺流程组名称。
            string toRouteName = string.Empty;//下一个工艺流程名称。
            string toStepName = string.Empty;//下一个工步名称。

            try
            {
                //判断是否需要进行工序参数的输入如果有工序参数的输入不允许自动出站
                DataSet dsStepData = RemotingServer.ServerObjFactory.CreateIRouteEngine().GetStepBaseDataAndParamDataByKey(stepKey, 1);
                msg = ReturnMessageUtils.GetServerReturnMessage(dsStepData);
                //是否获取到工步基本数据及其工步参数数据。
                if (string.IsNullOrEmpty(msg)
                    && null != dsStepData
                    && dsStepData.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME)
                    && dsStepData.Tables.Contains(POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME)
                    && dsStepData.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0
                    )
                {
                    msg = string.Format("[{0}]出站时需要输入工步参数，自动出站失败。", stepName);
                    WipManagement.RecordErrorMessage(db, TITLE, msg, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);

                    return msg;
                }

                //获取工艺流程下一工步信息
                DataSet dsRouteNextStep = RemotingServer.ServerObjFactory.CreateIEnterpriseEngine().GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                msg = ReturnMessageUtils.GetServerReturnMessage(dsRouteNextStep);
                if (!string.IsNullOrEmpty(msg))
                {
                    WipManagement.RecordErrorMessage(db, TITLE, msg, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);
                    return msg;
                }
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

                //更新工步信息
                Hashtable htStepTransaction = new Hashtable();
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, null);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, null);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);

                DataTable dtAutoTracOutStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                dtAutoTracOutStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;


                //更新Transaction 信息
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = enterpriseName;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = routeName;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = stepKey;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = stepName;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY] = toStepKey;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = leftQty;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = leftQty;
                htAutoTrackOutTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = 0;

                //组织进站使用的参数
                Hashtable htAutoTrackOutParams = CommonUtils.ConvertToHashtable(dtParams);
                htAutoTrackOutParams[POR_LOT_FIELDS.FIELD_EDIT_TIME] = drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME];
                DataTable dtAutoTrackOutParams = CommonUtils.ParseToDataTable(htAutoTrackOutParams);
                dtAutoTrackOutParams.TableName = TRANS_TABLES.TABLE_PARAM;

                DataTable dtAutoTrackOutTransaction = CommonUtils.ParseToDataTable(htAutoTrackOutTransaction);
                dtAutoTrackOutTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

                DataSet dsAutoTrackInParams = new DataSet();
                dsAutoTrackInParams.Tables.Add(dtAutoTrackOutTransaction);
                dsAutoTrackInParams.Tables.Add(dtAutoTrackOutParams);
                dsAutoTrackInParams.Tables.Add(dtAutoTracOutStepTransaction);

                DataSet dsReturn = LotTrackOut(dsAutoTrackInParams);
                int code = 0;
                msg = ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                if (!string.IsNullOrEmpty(msg))
                {
                    WipManagement.RecordErrorMessage(db, TITLE, msg, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);
                }
            }
            catch (Exception ex)
            {
                WipManagement.RecordErrorMessage(db, TITLE, ex.Message, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);
                LogService.LogError("TrackInAutoTrackOut Error: " + ex.Message);
                msg = ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// 批次批量进站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次进站信息的数据集对象。</param>
        /// <returns>0:进站成功。1:记录过期。2:需要抽检。-1:进站失败。</returns>
        public DataSet LotBatchTrackIn(IList<DataSet> lstTrackInData)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                using (DbTransaction dbTran = dbConn.BeginTransaction())
                {
                    try
                    {
                        int code = 0;

                        //添加队列存储需要自动出站的批次信息
                        IList<DataSet> lstAutoTrackOut = new List<DataSet>();

                        foreach (DataSet dsParams in lstTrackInData)
                        {
                            int perCode = LotTrackInPrivate(dsParams, dbTran);
                            //判断 PerCode：2、需抽检 3、批次自动出站。
                            if (perCode == 2)
                            {
                                code = perCode;
                            }
                            else if (perCode == 3)
                            {
                                lstAutoTrackOut.Add(dsParams);
                            }
                        }
                        dbTran.Commit();

                        string msg = string.Empty;

                        //遍历自动出站的批次列表逐个进行出站作业
                        foreach (DataSet dsParams in lstAutoTrackOut)
                        {
                            //执行自动进站作业。
                            msg += TrackInAutoTrackOut(dsParams);
                        }

                        if (code == 2)
                        {
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, "批次需要抽检。");
                        }
                        else if (code == 3)
                        {
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, 4, msg);
                        }
                        else
                        {
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, ex.Message);
                        LogService.LogError("LotBatchTrackIn Error: " + ex.Message);
                        dbTran.Rollback();
                    }
                }
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 供批次进站和批次批量进站操作调用。用于批次进站。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <param name="dbTran"></param>
        /// <returns>1：正常 2：需要抽检。</returns>
        private int LotTrackInPrivate(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME))           //存放操作数据
            {
                throw new Exception("传入参数不正确，请检查。");
            }

            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
            string opEditTime = string.Empty;
            if (dtParams.ExtendedProperties.Contains(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME))
            {
                opEditTime = Convert.ToString(dtParams.ExtendedProperties[COMMON_FIELDS.FIELD_COMMON_EDIT_TIME]);   //操作时编辑时间
            }
            else
            {
                opEditTime = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_EDIT_TIME]);   //操作时编辑时间
            }
            string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);           //批次号
            string workOrderNo = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);    //工单号
            string operationKey = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);  //工序主键
            string sDuration = Convert.ToString(htParams[POR_ROUTE_STEP_FIELDS.FIELD_DURATION]);    //工步标准时长。

            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            string shiftName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);//班次名称
            string shiftKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);//班次主键
            string enterpriseKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY]);//工艺流程组主键
            string routeKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY]);//工艺流程主键
            string stepName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME]);//工步名称
            string stepKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);//工步主键
            string editTimeZone = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);//编辑时区
            string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);//编辑人
            double quantityIn = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN]);//进站前数量
            double leftQty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);//进站后数量
            string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);//操作名称
            string lineKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);//线别主键
            string lineName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);//线别名称
            string lineNamePre = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE]);//上一线别名称
            string opComputerName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);//操作客户端名称
            string operatorName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);//操作人
            string workOrderKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY]);//工单主键
            string equipmentKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);//设备主键。

            //检查记录是否过期。防止重复修改。
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
            List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
            listCondition.Add(kvp);
            //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
            if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
            {
                throw new Exception("信息已过期，请重新进站。");
            }

            LotTrackIn(dbTran, dsParams);

            //检查是否自动合批
            bool isLastStep = false;              //是否是最后一个工步。
            bool isAutoMerge = false;             //是否自动合批次
            bool isAutoTrackOut = false;          //是否自动出站
            int maxBoxQuantity = -1;              //最大数量，自动合批用。
            string dependSampStep = string.Empty;   //工步抽检的依赖工步名称。
            DataSet dsStepUda = RemotingServer.ServerObjFactory.Get<IRouteEngine>().GetStepUda(stepKey);
            //获取到工步自定义属性数据。
            #region 获取到工步自定义属性数据。
            if (null != dsStepUda
                && dsStepUda.Tables.Count > 0
                && dsStepUda.Tables[0].Rows.Count > 0)
            {
                //遍历工序的自定义属性。
                for (int i = 0; i < dsStepUda.Tables[0].Rows.Count; i++)
                {
                    string szAttriName = dsStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME].ToString();
                    string szAttriValue = dsStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString();
                    //是否是最后一个工步。
                    if (szAttriName == "IsLastStep")
                    {
                        bool.TryParse(szAttriValue, out isLastStep);
                    }
                    //是否自动合批。
                    if (szAttriName == "AutoMerge")
                    {
                        bool.TryParse(szAttriValue, out isAutoMerge);
                    }
                    //箱子最大数量。
                    if (szAttriName == "MaxBoxQuantity")
                    {
                        int.TryParse(szAttriValue, out maxBoxQuantity);
                    }
                    //是否自动出站。
                    if (szAttriName == "AutoTrackOut")
                    {
                        bool.TryParse(szAttriValue, out isAutoTrackOut);
                    }
                    //获取当前工步是否有依赖的抽样工步。
                    if (szAttriName == "DependSampStep")
                    {
                        dependSampStep = szAttriValue;
                    }
                }
            }
            #endregion

            //如果自动出站的话 直接返回 perCode ：3
            if (isAutoTrackOut)
            {
                DateTime dtCurrent = UtilHelper.GetSysdate(db);
                string nextRunTime = string.Empty;
                double duration = 0;
                if (!double.TryParse(sDuration, out duration))
                {
                    duration = 0;
                }
                nextRunTime = dtCurrent.AddMinutes(duration).ToString("yyyy-MM-dd HH:mm:ss");
                Hashtable htWIPJob = new Hashtable();
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_ROW_KEY, UtilHelper.GenerateNewKey(0));
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_LOT_KEY, lotKey);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_LOT_NUMBER, lotNumber);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_WORKORDER_NUMBER, workOrderNo);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_EDIT_TIME, dtCurrent);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_EDIT_TIMEZONE, editTimeZone);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_EDITOR, editor);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_CREATETIME, dtCurrent);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_ENTERPRISE_KEY, enterpriseKey);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_ROUTE_KEY, routeKey);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_EQUIPMENT_KEY, equipmentKey);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_STEP_KEY, stepKey);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_CLOSETYPE, string.Empty);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_NEXTRUNTIME, nextRunTime);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_RUNACCOUNT, "0");
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_STATUS, "0");
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_JOB_TYPE, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_LINE_NAME, lineName);
                htWIPJob.Add(WIP_JOB_FIELDS.FIELDS_NOTIFY_USER, string.Empty);
                //插入一笔自动过站任务。
                WipJobAutoTrack.InsertWipJob(db, dbTran, htWIPJob);

                //更新批次状态为9
                string sql = string.Format(@"UPDATE POR_LOT
                                            SET STATE_FLAG=9
                                            WHERE LOT_KEY='{0}'",
                                            lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                return 3;
            }
            else
            {

                //检查是否需要进行数据采集。
                string edcPointKey = string.Empty;
                string partNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                if (CheckNeedEdc(dbTran, lotKey, routeKey, stepKey,
                                stepName, lineKey, lineName, partNumber,
                                dependSampStep, equipmentKey, out edcPointKey))
                {
                    IEDCEngine edcEngine = RemotingServer.ServerObjFactory.Get<IEDCEngine>();
                    edcEngine.SaveEdcMainInfo(dbTran, lotKey, edcPointKey, editor, equipmentKey, lineName, shiftKey);
                    return 2;
                }
                else
                {
                    //更新批次状态为9
                    string sql = string.Format(@"UPDATE POR_LOT
                                            SET STATE_FLAG=9
                                            WHERE LOT_KEY='{0}'",
                                                lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
            }
            //检查是否需要进行锁定。
            CheckAndUpdateFutureHold(dbTran, dsParams, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
            return 1;
        }

        /// <summary>
        /// 进站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN"/>。
        /// </remarks>
        /// <param name="dsParams">包含进站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet LotTrackIn(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                using (DbTransaction dbTran = dbConn.BeginTransaction())
                {
                    try
                    {
                        int code = LotTrackInPrivate(dsParams, dbTran);
                        dbTran.Commit();

                        string msg = string.Empty;

                        if (code == 2)
                        {
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, "批次需要抽检。");
                        }
                        else if (code == 3)
                        {
                            //执行自动进站作业。
                            msg = TrackInAutoTrackOut(dsParams);
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, 4, msg);
                        }
                        else
                        {
                            ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, ex.Message);
                        LogService.LogError("LotTrackIn Error: " + ex.Message);
                        dbTran.Rollback();
                    }
                }
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增批次进站记录。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="dsParams">包含批次进站信息的数据集对象。</param>
        private void LotTrackIn(DbTransaction dbTran, DataSet dsParams)
        {
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
            string operationKey = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);  //工序主键

            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            string lineKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);//线别主键
            string lineName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);//线别名称
            string lineNamePre = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE]);//上一线别名称
            string opComputerName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);//操作客户端名称
            string operatorName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);//操作人
            string stepKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);//工步主键
            string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);//编辑人
            string editTimeZone = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);//编辑时区
            double leftQty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);//进站后数量
            string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);//操作名称
            string equipmentKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);//设备主键。
            //操作动作必须是 TRACKIN 
            if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN)
            {
                throw new Exception("传入参数的操作动作不正确，请检查。");
            }
            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);
            //更新批次信息。
            string strSql = "";
            string sqlCommend01 = string.Format(@"SELECT COUNT(1) AS COUNT FROM dbo.POR_ROUTE_OPERATION_ATTR A
                                                            INNER JOIN dbo.POR_ROUTE_STEP B ON A.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                                WHERE B.ROUTE_STEP_KEY = '{0}' 
                                                            AND A.ATTRIBUTE_NAME = 'IsCheckGetLAminatingMachineEquCode' AND ATTRIBUTE_VALUE = 'true'", stepKey);
            DataSet ds01 = db.ExecuteDataSet(CommandType.Text, sqlCommend01);
            if (ds01 != null && ds01.Tables.Count > 0 && Convert.ToInt32(ds01.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
            {
                strSql = string.Format(" ,LAMINATING_MACHINE = '{0}' ", equipmentKey);
            }
            //            string sql = string.Format(@"UPDATE POR_LOT 
            //                                        SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',
            //                                            STATE_FLAG='4',START_PROCESS_TIME=GETDATE(),
            //                                            CUR_PRODUCTION_LINE_KEY ='{3}',LINE_NAME ='{4}',OPR_LINE='{4}',
            //                                            OPERATOR='{5}',OPR_LINE_PRE='{6}',OPR_COMPUTER='{7}'
            //                                        WHERE LOT_KEY='{8}'",
            //                                        leftQty,
            //                                        editor.PreventSQLInjection(),
            //                                        editTimeZone.PreventSQLInjection(),
            //                                        lineKey.PreventSQLInjection(),
            //                                        lineName.PreventSQLInjection(),
            //                                        operatorName.PreventSQLInjection(),
            //                                        lineNamePre.PreventSQLInjection(),
            //                                        opComputerName.PreventSQLInjection(),
            //                                        lotKey.PreventSQLInjection());
            string sql01 = @"UPDATE POR_LOT 
                                SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',
                                    STATE_FLAG='4',START_PROCESS_TIME=GETDATE(),
                                    CUR_PRODUCTION_LINE_KEY ='{3}',LINE_NAME ='{4}',OPR_LINE='{4}',
                                    OPERATOR='{5}',OPR_LINE_PRE='{6}',OPR_COMPUTER='{7}'
                                ";
            string sql02 = sql01 + strSql + " WHERE LOT_KEY='{8}'";
            string sql = string.Format(sql02,
                                        leftQty,
                                        editor.PreventSQLInjection(),
                                        editTimeZone.PreventSQLInjection(),
                                        lineKey.PreventSQLInjection(),
                                        lineName.PreventSQLInjection(),
                                        operatorName.PreventSQLInjection(),
                                        lineNamePre.PreventSQLInjection(),
                                        opComputerName.PreventSQLInjection(),
                                        lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //设备主键不为空，对设备进行操作。
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                LotTrackInForEquipment(dbTran, lotKey, equipmentKey, leftQty, operationKey, stepKey, editor);
            }
            //工步组件不良数据
            //如果数据集中包含名称WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME的数据表对象。
            if (dsParams.Tables.Contains(WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME)
                && dsParams.Tables[WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                DataSet dsDefectParams = new DataSet();
                htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_DEFECT;
                DataTable dtDefectTransaction = CommonUtils.ParseToDataTable(htTransaction);
                dtDefectTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsDefectParams.Tables.Add(dtDefectTransaction);
                DataTable dtDefect = dsParams.Tables[WIP_DEFECT_FIELDS.DATABASE_TABLE_NAME].Copy();
                dsDefectParams.Tables.Add(dtDefect);
                LotDefect(dsDefectParams, dbTran);
            }
            //工步组件报废数据
            //如果数据集中包含名称WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME的数据表对象。
            if (dsParams.Tables.Contains(WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME)
                && dsParams.Tables[WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                DataSet dsScrapParams = new DataSet();
                htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP;
                DataTable dtScrapTransaction = CommonUtils.ParseToDataTable(htTransaction);
                dtScrapTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsScrapParams.Tables.Add(dtScrapTransaction);
                DataTable dtScrap = dsParams.Tables[WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME].Copy();
                dsScrapParams.Tables.Add(dtScrap);
                LotScrap(dsScrapParams, dbTran);
            }

            //向WIP_TRANSACTION表插入批次进站的操作记录。
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN;
            if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            }
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //工步采集参数数据。
            //如果数据集中包含名称WIP_PARAM_FIELDS.DATABASE_TABLE_NAME的数据表对象。
            if (dsParams.Tables.Contains(WIP_PARAM_FIELDS.DATABASE_TABLE_NAME))
            {
                WIP_PARAM_FIELDS wipParamFields = new WIP_PARAM_FIELDS();
                DataTable dtWIPParam = dsParams.Tables[WIP_PARAM_FIELDS.DATABASE_TABLE_NAME];
                //遍历批次的工步采集参数数据。
                for (int i = 0; i < dtWIPParam.Rows.Count; i++)
                {
                    DataRow drWIPParam = dtWIPParam.Rows[i];
                    Hashtable htWIPParam = CommonUtils.ConvertRowToHashtable(drWIPParam);
                    if (!htWIPParam.ContainsKey(WIP_PARAM_FIELDS.FIELD_TRANSACTION_KEY))
                    {
                        htWIPParam.Add(WIP_PARAM_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                    }
                    htWIPParam[WIP_PARAM_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                    //插入一笔工序参数数据。
                    sql = DatabaseTable.BuildInsertSqlStatement(wipParamFields, htWIPParam, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
            }
        }
        /// <summary>
        /// 批次进站对设备相关的操作。
        /// </summary>
        /// <param name="dbTrans">数据库事务操作对象</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="qty">批次数量</param>
        /// <param name="operationKey">工序主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="editor">编辑人。</param>
        private void LotTrackInForEquipment(DbTransaction dbTran,
                                            string lotKey,
                                            string equipmentKey,
                                            double qty,
                                            string operationKey,
                                            string stepKey,
                                            string editor)
        {
            string sql = string.Format(@"SELECT a.ISBATCH,a.EQUIPMENT_STATE_KEY,a.MAXQUANTITY ,b.EQUIPMENT_STATE_TYPE,b.EQUIPMENT_STATE_NAME
                                        FROM EMS_EQUIPMENTS a
                                        LEFT JOIN EMS_EQUIPMENT_STATES b ON a.EQUIPMENT_STATE_KEY=b.EQUIPMENT_STATE_KEY
                                        WHERE EQUIPMENT_KEY='{0}'",
                                        equipmentKey.PreventSQLInjection());
            DataSet dsEquipment = db.ExecuteDataSet(dbTran, CommandType.Text, sql);//查询设备
            //如果有数据。
            if (null != dsEquipment && dsEquipment.Tables.Count > 0 && dsEquipment.Tables[0].Rows.Count > 0)
            {
                DataRow drEquipment = dsEquipment.Tables[0].Rows[0];
                string isBatch = Convert.ToString(drEquipment[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH]);                                        //设备是否是批处理设备。
                string equipmentState = Convert.ToString(drEquipment[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY]);                       //设备状态。
                string maxQuantity = Convert.ToString(drEquipment[EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY]);                                   //最大数量。
                string equStateName = Convert.ToString(drEquipment[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME]);  //设备状态名称。
                string equStateType = Convert.ToString(drEquipment[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE]);   //设备状态类型。
                //如果设备状态不是“待产” 
                if (equStateType != "LOST")
                {
                    if (equStateType != "RUN")
                    {
                        //如果设备状态不是在产，结束方法执行。
                        throw new Exception("${res:FanHai.Hemera.Modules.Wip.WipEngine.Msg.EquipmentCanNotUser}");
                    }
                    // 设备状态是“在产” 且是批处理设备。
                    if (equStateType == "RUN" && isBatch == "1")
                    {
                        //获取设备在产数量。
                        sql = string.Format(@"SELECT ISNULL(SUM(T.QUANTITY),0) AS INQUANTITY 
                                            FROM EMS_LOT_EQUIPMENT T
                                            WHERE T.EQUIPMENT_KEY = '{0}' AND T.END_TIMESTAMP IS NULL",
                                            equipmentKey.PreventSQLInjection());
                        DataSet dsLotEquipment = db.ExecuteDataSet(dbTran, CommandType.Text, sql);
                        //获取设备在产数量成功
                        if (null != dsLotEquipment
                            && dsLotEquipment.Tables.Count > 0
                            && dsLotEquipment.Tables[0].Rows.Count > 0
                            && !string.IsNullOrEmpty(maxQuantity))
                        {
                            string inProductQuantity = Convert.ToString(dsLotEquipment.Tables[0].Rows[0]["INQUANTITY"]);
                            //在产数量>设备的最大数量，结束方法执行。
                            if (Convert.ToDouble(inProductQuantity) + qty > Convert.ToDouble(maxQuantity))
                            {
                                throw new Exception("${res:FanHai.Hemera.Modules.Wip.WipEngine.Msg.BigThenMaxQuantity}");
                            }
                        }
                    }
                    else
                    {
                        //如果不是批处理设备，结束方法执行。
                        throw new Exception("${res:FanHai.Hemera.Modules.Wip.WipEngine.Msg.BatchIsFalse}");
                    }
                }
                //批次和设备进行关联。
                WipManagement.TrackInForEquipment(db, dbTran, lotKey, qty, operationKey, stepKey, equipmentKey, editor);
            }
        }

        /// <summary>
        /// 检查批次是否需要进行数据采集。
        /// </summary>
        /// <param name="dbTrans">数据库事务操作对象</param>
        /// <param name="lotKey">唯一标识批次的主键。</param>
        /// <param name="routeKey">工艺流程主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="stepName">工步名称。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <param name="lineName">线别名称。</param>
        /// <param name="partNumber">成品料号。</param>
        /// <param name="partNumber">成品料号。</param>
        /// <param name="dependSampStep">与指定工步有依赖抽检关系的工步名称。</param>
        /// <param name="outEdcPointKey">数据抽检设置主键。输出参数。</param>
        /// <returns>true：批次需要抽检。false：批次不需要抽检。</returns>
        private bool CheckNeedEdc(DbTransaction dbTrans,
                                string lotKey,
                                string routeKey,
                                string stepKey,
                                string stepName,
                                string lineKey,
                                string lineName,
                                string partNumber,
                                string dependSampStep,
                                string equipmentKey,
                                out string outEdcPointKey)
        {
            outEdcPointKey = string.Empty;
            IEDCEngine edcEngine = RemotingServer.ServerObjFactory.Get<IEDCEngine>();
            //获取数据抽检点。
            Hashtable htParams = new Hashtable();
            htParams.Add(POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER, partNumber);
            htParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, stepName);
            htParams.Add(EDC_POINT_FIELDS.FIELD_ACTION_NAME, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
            htParams.Add(EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY, routeKey);
            if (equipmentKey != string.Empty)
            {
                htParams.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            DataSet dsEdcPoint = edcEngine.CheckEdc(dtParams);
            string msg = ReturnMessageUtils.GetServerReturnMessage(dsEdcPoint);
            if (string.IsNullOrEmpty(msg)
                && dsEdcPoint.Tables.Count > 0
                && dsEdcPoint.Tables[0].Rows.Count > 0) //成功获取到数据抽检点集合。
            {
                //获取的数据抽检点集合存在数据。
                string edcPointKey = dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_ROW_KEY].ToString();
                //抽检参数没有满足条件的，则不进行抽检。
                if (!edcEngine.CheckEDCPointParams(edcPointKey))
                {
                    return false;
                }
                string edcKey = dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_EDC_KEY].ToString();
                string samplingKey = string.Empty;
                if (dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_SP_KEY] != null)
                {
                    samplingKey = dsEdcPoint.Tables[0].Rows[0][EDC_POINT_FIELDS.FIELD_SP_KEY].ToString();
                }
                bool blNeedSamp = false;
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
                                        lineKey,
                                        samplingKey,
                                        ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC,
                                        equipmentKey);
                    }
                }
                //上面判断需要进行抽检。
                if (blNeedSamp)
                {
                    outEdcPointKey = edcPointKey;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //批次不需要抽检
            return false;
        }
        /// <summary>
        /// 检查是否要HOLD批次。如果需要HOLD批次，则HOLD批次。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。。</param>
        /// <param name="dsParams">包含进站或出站信息的数据集对象。</param>
        /// <param name="action">动作名称。<see cref="COMMON_FIELDS.FIELD_ACTIVITY_TRACKIN"/>或者<see cref="COMMON_FIELDS.FIELD_ACTIVITY_TRACKOUT"/></param>
        /// <returns>触发预设暂停的个数。0：没有批次被HOLD。>0：该批次被HOLD。</returns>
        private int CheckAndUpdateFutureHold(DbTransaction dbTran, DataSet dsParams, string action)
        {
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);

            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            string editTimeZone = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);//编辑时区
            string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);//编辑人
            string stepKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);//工步主键

            string sql = @"SELECT *
                           FROM WIP_FUTUREHOLD 
                           WHERE LOT_KEY=@lotKey AND STEP_KEY=@stepKey AND STATUS=1 AND ACTION_NAME=@action
                           ORDER BY EDIT_TIME DESC";
            DbCommand dbCmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(dbCmd, "lotKey", DbType.String, lotKey);
            db.AddInParameter(dbCmd, "stepKey", DbType.String, stepKey);
            db.AddInParameter(dbCmd, "action", DbType.String, action);
            DataSet dsFutureHold = db.ExecuteDataSet(dbCmd, dbTran);
            int nRet = 0;
            if (dsFutureHold != null && dsFutureHold.Tables.Count > 0 && dsFutureHold.Tables[0].Rows.Count > 0)
            {
                DataTable dtFutureHold = dsFutureHold.Tables[0];
                DataRow drFutureHold = dtFutureHold.Rows[0];
                string rcCategoryKey = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY]);
                string rcCategoryName = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_NAME]);
                string rcCodeKey = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY]);
                string rcCodeName = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE]);
                string comment = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK]);
                string opUser = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_EDITOR]);
                string holdPassword = Convert.ToString(drFutureHold[WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD]);

                sql = string.Format("SELECT STATE_FLAG FROM POR_LOT WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
                string stateFlag = Convert.ToString(db.ExecuteScalar(dbTran, CommandType.Text, sql));
                //组织暂停操作数据。
                WIP_TRANSACTION_FIELDS transFileds = new WIP_TRANSACTION_FIELDS();
                DataTable dtHoldTransaction = CommonUtils.CreateDataTable(transFileds);
                DataRow drHoldTransaction = dtHoldTransaction.NewRow();
                dtHoldTransaction.Rows.Add(drHoldTransaction);
                foreach (string key in htTransaction.Keys)
                {
                    drHoldTransaction[key] = htTransaction[key];
                }
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = "FH:" + comment;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = opUser;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY] = string.Empty;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = stateFlag;
                if (action == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT)
                {
                    DataTable dtStepTransaction = dsParams.Tables[WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放下一工步信息的数据  
                    Hashtable htStepTransaciont = CommonUtils.ConvertToHashtable(dtStepTransaction);
                    //下一工步数据
                    string toEnterpriseKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY]);//下一个工艺流程组主键。
                    string toEnterpriseName = Convert.ToString(htTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME]);//工艺流程组名称
                    string toRouteKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY]);//下一个工艺流程主键。
                    string toRouteName = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME]);//下一个工艺流程主键。
                    string toStepKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);//下一个工步主键。
                    string toStepName = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME]);//下一个工步主键。
                    drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = toEnterpriseKey;
                    drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = toEnterpriseName;
                    drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = toRouteKey;
                    drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = toRouteName;
                    drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = toStepKey;
                    drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = toStepName;
                }
                //组织暂停数据
                WIP_HOLD_RELEASE_FIELDS holdFields = new WIP_HOLD_RELEASE_FIELDS();
                DataTable dtHold = CommonUtils.CreateDataTable(holdFields);
                DataRow drHold = dtHold.NewRow();
                dtHold.Rows.Add(drHold);
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE] = editTimeZone;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR] = editor;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_DESCRIPTION] = comment;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_OPERATOR] = opUser;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD] = holdPassword;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIME] = DBNull.Value;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIMEZONE] = editTimeZone;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY] = rcCategoryKey;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME] = rcCategoryName;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY] = rcCodeKey;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME] = rcCodeName;
                //组织暂停参数。
                DataSet dsHoldParams = new DataSet();
                dsHoldParams.Tables.Add(dtHoldTransaction);
                dsHoldParams.Tables.Add(dtHold);
                //暂停批次。
                LotHold(dbTran, dsHoldParams);
                //更新预设暂停记录。
                sql = @"UPDATE WIP_FUTUREHOLD 
                        SET STATUS=0,EDITOR=@editor,EDIT_TIME= GETDATE()
                        WHERE LOT_KEY=@lotKey AND STEP_KEY=@stepKey AND STATUS=1 AND ACTION_NAME=@action";
                dbCmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCmd, "lotKey", DbType.String, lotKey);
                db.AddInParameter(dbCmd, "stepKey", DbType.String, stepKey);
                db.AddInParameter(dbCmd, "action", DbType.String, action);
                db.AddInParameter(dbCmd, "editor", DbType.String, editor);
                nRet = db.ExecuteNonQuery(dbCmd, dbTran);
            }
            return nRet;
        }

        /// <summary>
        /// 检查是否超过恒温时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过恒温周期。false:没有超过恒温周期。</returns>
        public bool CheckConstantTemperatureCycle(string lotNumber, string lineKey, string proId, DateTime waitTrackInTime)
        {
            using (DbConnection dbCon = db.CreateConnection())
            {
                dbCon.Open();
                using (DbTransaction dbTrans = dbCon.BeginTransaction())
                {
                    return CheckConstantTemperatureCycle(dbTrans, lotNumber, lineKey, proId, waitTrackInTime);
                }
            }
        }
        /// <summary>
        /// 检查是否超过恒温时间周期。
        /// </summary>
        /// <param name="dbTrans">数据库事务操作对象。</param>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过恒温周期。false:没有超过恒温周期。</returns>
        private bool CheckConstantTemperatureCycle(DbTransaction dbTrans, string lotNumber, string lineKey, string proId, DateTime waitTrackInTime)
        {
            //根据LINE KEY抓取对应的恒温周期
            string sql = string.Format(@"SELECT ATTRIBUTE_VALUE
                                    FROM BASE_ATTRIBUTE_VALUE
                                    WHERE OBJECT_TYPE='{0}'
                                    AND OBJECT_KEY='{1}'
                                    AND ATTRIBUTE_NAME='{2}'",
                                    FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                    lineKey.PreventSQLInjection(),
                                    LINE_ATTRIBUTE.FixCycle);
            string obj = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
            double cycle = 0;
            if (string.IsNullOrEmpty(obj) || double.TryParse(obj, out cycle) == false)
            {
                cycle = 0;
            }
            //如果没有设置恒温周期，根据产品ID号抓取恒温周期
            if (cycle == 0)
            {
                //根据产品ID号抓取固化时间周期
                sql = string.Format(@"IF EXISTS(SELECT 1 
                                                  FROM POR_WO_PRD a
                                                  INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND c.PART_NUMBER=a.PART_NUMBER
                                                  WHERE c.LOT_NUMBER='{0}'
                                                  AND a.IS_USED='Y')
                                        BEGIN
                                            SELECT a.CONSTANT_TEMPERTATURE_CYCLE
                                            FROM POR_WO_PRD a
                                            INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND a.PART_NUMBER=c.PART_NUMBER
                                            WHERE c.LOT_NUMBER='{0}'
                                            AND a.IS_USED='Y';    
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT CONSTANT_TEMPERTATURE_CYCLE
                                            FROM POR_PRODUCT
                                            WHERE PRODUCT_CODE='{1}' and ISFLAG=1;
                                        END",
                                        lotNumber.PreventSQLInjection(),
                                        proId.PreventSQLInjection());
                obj = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                if (string.IsNullOrEmpty(obj) || double.TryParse(obj, out cycle) == false)
                {
                    cycle = 0;
                }
            }
            //当前时间-等待进站时间>=恒温周期
            DateTime dtCurTime = UtilHelper.GetSysdate(db);
            double curCycle = (dtCurTime - waitTrackInTime).TotalMinutes;
            if (curCycle >= cycle)
            {
                return true;
            }
            //当前时间-等待进站时间<恒温周期
            return false;
        }

    }
}
