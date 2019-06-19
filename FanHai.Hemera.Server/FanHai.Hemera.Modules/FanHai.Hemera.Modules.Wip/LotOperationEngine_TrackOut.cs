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
        /// 获取工单检验允许的等级代码。符合的等级在终检将不需要输入不良和备注。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>工单检验允许的等级代码。多个等级使用逗号（,）分隔。</returns>
        public string GetWOCheckAllowGrade(string lotNumber, out string gradeName)
        {
            string sql=@"SELECT TOP 1 a.ATTRIBUTE_VALUE
                        FROM POR_WORK_ORDER_ATTR a
                        INNER JOIN POR_LOT b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                        WHERE ATTRIBUTE_NAME='CheckAllowGrade'
                        AND b.LOT_NUMBER=@lotNumber
                        AND ISFLAG=1";
            using (DbCommand cmd = this.db.GetSqlStringCommand(sql))
            {
               
                this.db.AddInParameter(cmd, "@lotNumber", DbType.String, lotNumber);
                gradeName = Convert.ToString(this.db.ExecuteScalar(cmd));

                if (string.IsNullOrEmpty(gradeName))
                {
                    return null;
                }
            }

            StringBuilder sbCheckAllowGrade = new StringBuilder();
            sql =string.Format(@"SELECT b.GRADE_CODE
                                FROM dbo.SplitStringToTable('{0}') a
                                INNER JOIN V_ProductGrade b ON b.GRADE_NAME=a.VAL",
                                gradeName);
            
            using (IDataReader dataReader = this.db.ExecuteReader(CommandType.Text, sql))
            {
                while (dataReader.Read())
                {
                    string checkAllowGrade = dataReader.GetString(0);
                    sbCheckAllowGrade.AppendFormat("{0},", checkAllowGrade);
                }
                dataReader.Close();
            }
            return sbCheckAllowGrade.ToString();
        }
        /// <summary>
        /// 根据批次主键获取批次出站的时间控制数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次时间控制数据的数据集对象。数据集中包含两个数据表对象。
        /// 有一个名称为TrackOutStatus的数据表对象，数据表的列名为
        /// TimeStatusFlag（时间状态，0：加工时间没有满足最小加工时间。
        /// 1:加工时间满足最小时间，没有超过报警时间。
        /// 2:加工时间超过报警时间，没有超过最大加工时间。
        /// 3:加工时间超过最大加工时间。） 
        /// TimeControlBaseSubMin（基础加工时间-最小加工时间）。
        /// </returns>
        public DataSet GetTrackOutTimeControlStatus(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                double timeControlBase = 22.5f;
                double timeControlMin = 3;
                double timeControlWarning = 15;
                double timeControlMax = 30;
                int timeStatusFlag = 1;

                string sql = string.Format(@"SELECT SYSDATETIME() AS CURTIME,ISNULL(START_PROCESS_TIME,SYSDATETIME()) START_PROCESS_TIME,QUANTITY,CUR_STEP_VER_KEY
                                             FROM POR_LOT
                                             WHERE LOT_KEY = '{0}'",
                                             lotKey.PreventSQLInjection());
                DataSet dsLotInfo = db.ExecuteDataSet(CommandType.Text, sql);
                //获取批次数据成功。
                if (dsLotInfo.Tables.Count > 0 && dsLotInfo.Tables[0].Rows.Count > 0)
                {
                    DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];
                    DateTime startTime = Convert.ToDateTime(drLotInfo["START_PROCESS_TIME"]);    //批次开始操作时间
                    double quantity = Convert.ToDouble(drLotInfo["QUANTITY"]);              //数量
                    string stepKey = Convert.ToString(drLotInfo["CUR_STEP_VER_KEY"]);       //当前工步主键
                    DateTime curTime = Convert.ToDateTime(drLotInfo["CURTIME"]);            //当前时间

                    if (!string.IsNullOrEmpty(stepKey))
                    {
                        //根据工步主键获取工步的时间控制数据。
                        sql = string.Format(@"SELECT *
                                             FROM V_ROUTE_STEP_TIMECONTROL T 
                                             WHERE T.ROUTE_STEP_KEY='{0}'",
                                             stepKey.PreventSQLInjection());
                        DataSet timeControlData = db.ExecuteDataSet(CommandType.Text, sql);
                        //获取工步的时间控制数据成功。
                        if (timeControlData.Tables.Count > 0 && timeControlData.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTimeControlData = timeControlData.Tables[0].Rows[0];
                            timeControlBase = Convert.ToDouble(drTimeControlData[TIME_CONTROL_FILEDS.TIMECONTROLBASE]);
                            timeControlMin = Convert.ToDouble(drTimeControlData[TIME_CONTROL_FILEDS.TIMECONTROLMIN]);
                            timeControlWarning = Convert.ToDouble(drTimeControlData[TIME_CONTROL_FILEDS.TIMECONTROLWARNING]);
                            timeControlMax = Convert.ToDouble(drTimeControlData[TIME_CONTROL_FILEDS.TIMECONTROLMAX]);
                        }
                    }
                    //加工时间分钟数。
                    double fts = (curTime - startTime).TotalMinutes;
                    //加工时间没有满足最小加工时间。
                    if (fts < timeControlBase - timeControlMin)
                    {
                        timeStatusFlag = 0;
                    }
                    //加工时间满足最小时间，没有超过报警时间。
                    else if (fts < timeControlBase + timeControlWarning
                          && fts >= timeControlBase - timeControlMin)
                    {
                        timeStatusFlag = 1;
                    }
                    //加工时间超过报警时间，没有超过最大加工时间。
                    else if (fts >= timeControlBase + timeControlWarning
                          && fts < timeControlBase + timeControlMax)
                    {
                        timeStatusFlag = 2;
                    }
                    else//加工时间超过最大加工时间。
                    {
                        timeStatusFlag = 3;
                    }
                }
                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add(TIME_CONTROL_FILEDS.TIMESTATUSFLAG);
                dtReturn.Columns.Add(TIME_CONTROL_FILEDS.TIMECONTROLBASESUBMIN);
                dtReturn.Rows.Add(timeStatusFlag, timeControlBase - timeControlMin);
                dtReturn.TableName = TIME_CONTROL_FILEDS.DATATABLE_NAME;
                dsReturn.Merge(dtReturn, false, MissingSchemaAction.Add);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTrackOutTimeControlStatus Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 批次批量出站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次出站信息的数据集对象。</param>
        public DataSet LotBatchTrackOut(IList<DataSet> lstTrackInData,DataTable dtStepParam,int flag)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                using (DbTransaction dbTran = dbConn.BeginTransaction())
                {
                    try
                    {
                        IList<DataSet> lstAutoTrackIn = new List<DataSet>();
                        foreach (DataSet dsParams in lstTrackInData)
                        {
                            int perCode = LotTrackOutPrivate(dsParams, dbTran);
                            //有一个批次需要自动进站，加入自动进站列表中。
                            if (perCode == 2)
                            {
                                lstAutoTrackIn.Add(dsParams);
                            }
                        }
                        //更新上料信息
                        if (flag == 1 && dtStepParam != null)
                        {
                            string _message = string.Empty;
                            foreach (DataRow dr in dtStepParam.Rows)
                            {
                                string facKey = dr["FACTORY_KEY"].ToString();
                                string equKey = dr["EQUIPMENT_KEY"].ToString();
                                string opritionName = dr["OPERATION_NAME"].ToString();
                                string param = dr["PARAMETER"].ToString();
                                string matCode = dr["MATERIAL_CODE"].ToString();
                                string orderNum = dr["ORDER_NUMBER"].ToString();
                                string supplierName = dr["SUPPLIER_CODE"].ToString();
                                
                                if(string.IsNullOrEmpty(facKey) || string.IsNullOrEmpty(equKey) || string.IsNullOrEmpty(opritionName) ||
                                    string.IsNullOrEmpty(param) || string.IsNullOrEmpty(matCode) || string.IsNullOrEmpty(orderNum) || string.IsNullOrEmpty(supplierName))
                                    throw new Exception("上料信息条件存在空值(即部分料无设备发料库存)，请联系相关人员确认发料物料信息！");
                                string sql = string.Format(@"SELECT B.PARAMETER,A.SUPPLIER_CODE,A.SUM_QTY,B.USE_QTY AS COUNTNUM
                                                                    FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE A
                                                                    LEFT JOIN dbo.WST_STORE_MATERIAL_BUCKLE_CONTROL B 
                                                                    ON A.PARAMETER = B.PARAMETER
                                                                    WHERE B.STATUS = 1 AND A.STATUS = 1
                                                                    AND FACTORY_KEY = '{0}'
                                                                    AND EQUIPMENT_KEY = '{1}'
                                                                    AND OPERATION_NAME = '{2}'
                                                                    AND B.PARAMETER = '{3}'
                                                                    AND MATERIAL_CODE = '{4}'
                                                                    AND ORDER_NUMBER = '{5}'
                                                                    AND SUPPLIER_CODE = '{6}'",
                                                                    facKey, equKey, opritionName, param, matCode, orderNum, supplierName);
                                DataSet dsReturnInf = db.ExecuteDataSet(CommandType.Text, sql);
                                #region 判定信息
                                if (dsReturnInf != null || dsReturnInf.Tables[0].Rows.Count > 0)
                                {
                                    if (dsReturnInf.Tables[0].Rows.Count == 1)
                                    {
                                        if (string.IsNullOrEmpty(dsReturnInf.Tables[0].Rows[0]["COUNTNUM"].ToString()))
                                            throw new Exception("未对参数" + param + "进行维护对应扣料信息！请联系相应接口人！");
                                        else
                                        {
                                            if (Convert.ToDecimal(dsReturnInf.Tables[0].Rows[0]["COUNTNUM"].ToString()) > Convert.ToDecimal(dsReturnInf.Tables[0].Rows[0]["SUM_QTY"].ToString()))
                                                throw new Exception("参数" + param + ";材料批次" + supplierName + "的数量小于扣料量,请补料！");
                                            else if (Convert.ToDecimal(dsReturnInf.Tables[0].Rows[0]["COUNTNUM"].ToString()) == Convert.ToDecimal(dsReturnInf.Tables[0].Rows[0]["SUM_QTY"].ToString()))
                                            {
                                                string updateSql = string.Format(@"UPDATE dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE SET
                                                                                            SUM_QTY = SUM_QTY - {7} , STATUS = 0 ,USED_QTY = USED_QTY + {7}
                                                                                            WHERE STATUS = 1 
                                                                                            AND FACTORY_KEY = '{0}'
                                                                                            AND EQUIPMENT_KEY = '{1}'
                                                                                            AND OPERATION_NAME = '{2}'
                                                                                            AND PARAMETER = '{3}'
                                                                                            AND MATERIAL_CODE = '{4}'
                                                                                            AND ORDER_NUMBER = '{5}'
                                                                                            AND SUPPLIER_CODE = '{6}'",
                                                                                    facKey, equKey, opritionName, param,
                                                                                    matCode, orderNum, supplierName,
                                                                                    Convert.ToDecimal(dsReturnInf.Tables[0].Rows[0]["COUNTNUM"].ToString()));
                                                db.ExecuteNonQuery(dbTran, CommandType.Text, updateSql);
                                            }
                                            else
                                            {
                                                string updateSql = string.Format(@"UPDATE dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE SET
                                                                                            SUM_QTY = SUM_QTY - {7} , USED_QTY = USED_QTY + {7}
                                                                                            WHERE STATUS = 1 
                                                                                            AND FACTORY_KEY = '{0}'
                                                                                            AND EQUIPMENT_KEY = '{1}'
                                                                                            AND OPERATION_NAME = '{2}'
                                                                                            AND PARAMETER = '{3}'
                                                                                            AND MATERIAL_CODE = '{4}'
                                                                                            AND ORDER_NUMBER = '{5}'
                                                                                            AND SUPPLIER_CODE = '{6}'",
                                                                                   facKey, equKey, opritionName, param,
                                                                                   matCode, orderNum, supplierName,
                                                                                   Convert.ToDecimal(dsReturnInf.Tables[0].Rows[0]["COUNTNUM"].ToString()));
                                                db.ExecuteNonQuery(dbTran, CommandType.Text, updateSql);
                                            }
                                        }
                                    }
                                    else
                                        throw new Exception("设备虚拟仓存在重复信息，请发料人员确认");
                                }
                                else
                                    throw new Exception("请确认是否对车间" + dr["FACTORY_NAME"].ToString() + "的设备" + dr["EQUIPMENT_NAME"].ToString()
                                        + "进行发料：料号号为" + matCode + ",供应商批次为" + supplierName + ",请及时联系发料人员！");
                                #endregion
                            }
                        }
                        dbTran.Commit();
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn,string.Empty);
                        foreach (DataSet dsParams in lstAutoTrackIn)
                        {
                            //执行自动进站作业。
                            TrackOutAutoTrackIn(dsParams);
                        }
                    }
                    catch (Exception ex)
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, ex.Message);
                        LogService.LogError("LotBatchTrackOut Error: " + ex.Message);
                        dbTran.Rollback();
                    }
                }
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 供批次出站和批次批量出站操作调用。用于批次出站。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <param name="dbTran"></param>
        /// <returns>1：正常 2：需要自动进站</returns>
        private int LotTrackOutPrivate(DataSet dsParams, DbTransaction dbTran)
        {
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME)            //存放操作数据
                || !dsParams.Tables.Contains(WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME))      //存放下一工步信息的数据  
            {
                throw new Exception("传入参数不正确，请检查。");
            }

            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
            DataTable dtStepTransaction = dsParams.Tables[WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放下一工步信息的数据  

            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            Hashtable htStepTransaciont = CommonUtils.ConvertToHashtable(dtStepTransaction);
            //附加参数数据
            string opEditTime = string.Empty;
            if (dtParams.ExtendedProperties.Contains(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME))
            {
                opEditTime = Convert.ToString(dtParams.ExtendedProperties[COMMON_FIELDS.FIELD_COMMON_EDIT_TIME]);   //操作时编辑时间
            }
            else
            {
                opEditTime = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_EDIT_TIME]);   //操作时编辑时间
            }
            bool setNewRoute = Convert.ToBoolean(htParams[ROUTE_OPERATION_ATTRIBUTE.IsShowSetNewRoute]);
            //操作数据
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
            //下一工步数据
            string toEnterpriseKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY]);//下一个工艺流程组主键。
            string toRouteKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY]);//下一个工艺流程主键。
            string toStepKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);//下一个工步主键。
            //下一工艺流程步骤为空
            if (string.IsNullOrEmpty(toEnterpriseKey) || string.IsNullOrEmpty(toRouteKey) || string.IsNullOrEmpty(toStepKey))
            {
                throw new Exception("获取下一工艺步骤失败，请重试操作。");
            }
            bool isFinish = false;
            //如果是系统自动获取的下一工艺流程，则进行下一个工步验证。
            if (!setNewRoute)
            {
                //根据当前工艺流程和下一个工艺流程是否匹配
                //工艺流程组不匹配
                if (!toEnterpriseKey.Equals(enterpriseKey))
                {
                    throw new Exception("工艺流程组不匹配，请重试操作。");
                }
                string sql = string.Empty;
                //工艺流程不匹配
                if (!toRouteKey.Equals(routeKey))
                {
                    //判断是否属于同一个工艺流程组？
                    sql = string.Format(@"SELECT COUNT(*) FROM V_PROCESS_PLAN a
                                          WHERE a.ROUTE_ROUTE_VER_KEY='{0}'
                                          AND EXISTS(SELECT * FROM V_PROCESS_PLAN b 
                                                     WHERE b.ROUTE_ENTERPRISE_VER_KEY=a.ROUTE_ENTERPRISE_VER_KEY
                                                     AND b.ROUTE_ROUTE_VER_KEY='{1}')",
                                          toRouteKey.PreventSQLInjection(),
                                          routeKey.PreventSQLInjection());
                    //不属于同一个工艺流程组。
                    if (Convert.ToInt32(db.ExecuteScalar(dbTran, CommandType.Text, sql)) <= 0)
                    {
                        throw new Exception("工艺流程不匹配，请重试操作。");
                    }
                }
                //判断下一工步主键是否是当前工步的下一个工步。
                //判断一工步主键 和 当前工步主键的序号差。
                sql = string.Format(@"WITH a AS
                                    (
                                        SELECT ROW_NUMBER() OVER (ORDER BY ROUTE_SEQ,ROUTE_STEP_SEQ) AS ROWNUMBER,ROUTE_STEP_KEY
                                        FROM V_PROCESS_PLAN 
                                        WHERE ROUTE_ENTERPRISE_VER_KEY='{2}'
                                    )
                                    SELECT 
                                       ISNULL((SELECT MAX(ROWNUMBER) FROM a),0) MAX_ROW_NUMBER,
                                       ISNULL((SELECT ROWNUMBER FROM a WHERE ROUTE_STEP_KEY='{0}'),0) NXT_ROW_NUMBER,
                                       ISNULL((SELECT ROWNUMBER FROM a WHERE ROUTE_STEP_KEY='{1}'),0) CUR_ROW_NUMBER",
                                    toStepKey.PreventSQLInjection(),
                                    stepKey.PreventSQLInjection(),
                                    enterpriseKey.PreventSQLInjection());
                DataTable dtStep = db.ExecuteDataSet(dbTran, CommandType.Text, sql).Tables[0];
                int maxRowNumber = Convert.ToInt32(dtStep.Rows[0]["MAX_ROW_NUMBER"]);
                int nxtRowNumber = Convert.ToInt32(dtStep.Rows[0]["NXT_ROW_NUMBER"]);
                int curRowNumber = Convert.ToInt32(dtStep.Rows[0]["CUR_ROW_NUMBER"]);
                int diff = nxtRowNumber - curRowNumber;
                isFinish = maxRowNumber == curRowNumber;  //当前工步是最后一个工步。
                //如果是最后一站。但是两个工步的序号差不等于0
                if (isFinish && diff != 0)
                {
                    throw new Exception("获取下一工步失败(最后一站但两个工步的序号差不等于0)，请重试操作。");
                }
                //如果是最后一站。但是下一个工步的序号不等于最大工步序号。
                else if (isFinish && maxRowNumber != nxtRowNumber)
                {
                    throw new Exception("获取下一工步失败（最后一站但下一个工步的序号不等于最大工步序号），请重试操作。");
                }
                //如果不是最后一站。但是两个工步的序号差不等于1
                else if (!isFinish && diff != 1)
                {
                    throw new Exception("获取下一工步失败（两个工步的序号差不等于1），请重试操作。");
                }
            }
            //检查记录是否过期。防止重复修改。
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
            List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
            listCondition.Add(kvp);
            //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
            if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
            {
                throw new Exception("信息已过期，请重新过站。");
            }
            //批次出站
            LotTrackOut(dbTran, dsParams, isFinish);
            //如果非完成状态且数量>0
            //则判断下一工步进站前是否需要自动合批，自动分批。
            bool isAutoTrackIn = false;               //是否自动进站
            if (isFinish == false && leftQty > 0)
            {
                double quantityToMerge = leftQty;     //合批时批次数量。
                string lotKeyForMerge = lotKey;       //合批用的批次主键
                bool isAutoMerge = false;             //是否自动合批次
                bool isAutoSplit = false;             //是否自动分批
                int maxBoxQuantity = -1;              //最大数量，自动合批用。

                #region 获取下一工步的自定义属性
                //获取下一工步的自定义属性。
                DataSet dsNextStepUda = RemotingServer.ServerObjFactory.Get<IRouteEngine>().GetStepUda(toStepKey);
                if (null != dsNextStepUda
                    && dsNextStepUda.Tables.Count > 0
                    && dsNextStepUda.Tables[0].Rows.Count > 0)
                {
                    //获取下一工步的自定义属性成功。
                    for (int i = 0; i < dsNextStepUda.Tables[0].Rows.Count; i++)
                    {
                        string stepUdaName = Convert.ToString(dsNextStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME]);
                        string stepUdaValue = Convert.ToString(dsNextStepUda.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
                        //箱的最大数量
                        if (stepUdaName == "MaxBoxQuantity")
                        {
                            maxBoxQuantity = Convert.ToInt32(stepUdaValue);
                        }
                        //自动合批
                        if (stepUdaName == "AutoMerge")
                        {
                            bool.TryParse(stepUdaValue, out isAutoMerge);
                        }
                        //自动分批
                        if (stepUdaName == "AutoSplit")
                        {
                            bool.TryParse(stepUdaValue, out isAutoSplit);
                        }
                        //自动出站
                        if (stepUdaName == "AutoTrackIn")
                        {
                            bool.TryParse(stepUdaValue, out isAutoTrackIn);
                        }
                    }
                }
                #endregion
            }
            //根据预设暂停记录检查是否需要进行暂停。
            #region 根据预设暂停记录检查是否需要进行暂停
            int nHoldCount = 0;
            if (isFinish == false)
            {
                nHoldCount = CheckAndUpdateFutureHold(dbTran, dsParams, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
            }
            #endregion

            //没有完成、可以自动进站、没有暂停。
            if (isFinish == false && isAutoTrackIn && nHoldCount == 0)
            {
                return 2;
            }
            return 1;
        }

        /// <summary>
        /// 出站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT"/>。
        /// </remarks>
        /// <param name="dsParams">包含出站信息的数据集对象。</param>
        /// <returns>
        /// 包含结果数据的数据集对象。
        /// </returns>
        public DataSet LotTrackOut(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                using (DbTransaction dbTran = dbConn.BeginTransaction())
                {
                    try
                    {
                        int code = LotTrackOutPrivate(dsParams, dbTran);
                        dbTran.Commit();
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                        if (code == 2)
                        {
                            //执行自动进站作业。
                            TrackOutAutoTrackIn(dsParams);
                        }
                    }
                    catch (Exception ex)
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("LotTrackOut Error: " + ex.Message);
                        dbTran.Rollback();
                    }
                }
                dbConn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增批次出站记录。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="dsParams">包含批次出站信息的数据集对象。</param>
        /// <param name="isFinish">是否是最后一站。</param>
        private void LotTrackOut(DbTransaction dbTran, DataSet dsParams,bool isFinish)
        {
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
            DataTable dtStepTransaction = dsParams.Tables[WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放下一工步信息的数据  

            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
            Hashtable htTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            Hashtable htStepTransaciont = CommonUtils.ConvertToHashtable(dtStepTransaction);
            //操作数据
            string lotKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            string enterpriseKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY]);//工艺流程组主键
            string routeKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY]);//工艺流程主键
            string stepKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY]);//工步主键
            string editTimeZone = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);//编辑时区
            string editor = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);//编辑人
            double leftQty = Convert.ToDouble(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);//进站后数量
            string activity = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);//操作名称
            string lineKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);//线别主键
            string lineName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);//线别名称
            string opComputerName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);//操作客户端名称
            string operatorName = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);//操作人
            string equipmentKey = Convert.ToString(htTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);//设备主键。
            //下一工步数据
            string toEnterpriseKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY]);//下一个工艺流程组主键。
            string toRouteKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY]);//下一个工艺流程主键。
            string toStepKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);//下一个工步主键。
            //操作动作必须是 TRACKOUT
            if (activity != ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT)
            {
                throw new Exception("传入参数的操作动作不正确，请检查。");
            }
            //记录批次信息
            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);
            //更新批次信息。
            string sqlCommend01 = string.Format(@"SELECT COUNT(1) AS COUNT FROM dbo.POR_ROUTE_OPERATION_ATTR A
                                                            INNER JOIN dbo.POR_ROUTE_STEP B ON A.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                                WHERE B.ROUTE_STEP_KEY = '{0}' 
                                                            AND A.ATTRIBUTE_NAME = 'IsCheckGetLWeldingOutTime' AND ATTRIBUTE_VALUE = 'true'", stepKey);
            DataSet ds01 = db.ExecuteDataSet(CommandType.Text, sqlCommend01);
            sqlCommend01 = string.Format(@"SELECT COUNT(1) AS COUNT FROM dbo.POR_ROUTE_OPERATION_ATTR A
                                                            INNER JOIN dbo.POR_ROUTE_STEP B ON A.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                                WHERE B.ROUTE_STEP_KEY = '{0}' 
                                                            AND A.ATTRIBUTE_NAME = 'IsCheckGetSingleSeriesWeldingMachineEquCode' AND ATTRIBUTE_VALUE = 'true'", stepKey);
            DataSet ds02 = db.ExecuteDataSet(CommandType.Text, sqlCommend01);

            StringBuilder sbUpdateLot = new StringBuilder();
            sbUpdateLot.AppendFormat(@"UPDATE POR_LOT 
                                       SET QUANTITY={0},EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}',
                                           OPERATOR='{3}',OPR_LINE='{4}', OPR_COMPUTER='{5}'",
                                       leftQty,
                                       editor.PreventSQLInjection(),
                                       editTimeZone.PreventSQLInjection(),
                                       operatorName.PreventSQLInjection(),
                                       lineName.PreventSQLInjection(),
                                       opComputerName.PreventSQLInjection());
            if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_SI_LOT))
            {
                string siLot = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_SI_LOT]);
                sbUpdateLot.AppendFormat(",SI_LOT='{0}'", siLot.PreventSQLInjection());
            }
            if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_COLOR))
            {
                string color = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_COLOR]);
                sbUpdateLot.AppendFormat(",COLOR='{0}'", color.PreventSQLInjection());
            }
            if (ds01 != null && ds01.Tables.Count > 0 && Convert.ToInt32(ds01.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
            {
                sbUpdateLot.AppendFormat(" ,WELDING_TRACKOUT_TIME = getdate() ");
            }
            if (ds02 != null && ds02.Tables.Count > 0 && Convert.ToInt32(ds02.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
            {
                sbUpdateLot.AppendFormat(" ,SINGLE_MACHINE = '{0}' ", equipmentKey);
            }
            //当前工步是否是最后一个工步。
            //如果不是最后一个工步，需要更新当前工艺流程,更新状态。
            if (!isFinish)
            {
                sbUpdateLot.AppendFormat(@",CUR_ROUTE_VER_KEY='{0}',
                                            CUR_STEP_VER_KEY='{1}',
                                            ROUTE_ENTERPRISE_VER_KEY='{2}',
                                            START_WAIT_TIME=GETDATE(),
                                            STATE_FLAG=0",
                                            toRouteKey.PreventSQLInjection(),
                                            toStepKey.PreventSQLInjection(),
                                            toEnterpriseKey.PreventSQLInjection());
                //出账后数量为0，则结束批次。
                if (leftQty == 0)
                {
                    sbUpdateLot.AppendFormat(@",DELETED_TERM_FLAG=1");
                }
            }
            else
            {
                //如果是最后一个工步，需要更新更新状态为完成，更新结束状态为已结束。
                sbUpdateLot.AppendFormat(@",STATE_FLAG=10,DELETED_TERM_FLAG=1");
            }
            sbUpdateLot.AppendFormat(@" WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateLot.ToString());
            //更新WIP_JOB信息。更新自动出站JOB
            string sql = string.Format(@"UPDATE WIP_JOB 
                                        SET JOB_STATUS=1 
                                        WHERE LOT_KEY='{0}' AND ENTERPRISE_KEY='{1}' 
                                        AND ROUTE_KEY='{2}' AND STEP_KEY='{3}'",
                                        lotKey.PreventSQLInjection(),
                                        enterpriseKey.PreventSQLInjection(),
                                        routeKey.PreventSQLInjection(),
                                        stepKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //设备主键不为空，对设备进行操作。
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                WipManagement.TrackOutForEquipment(db,dbTran,lotKey, stepKey, equipmentKey, editor);
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
            //向WIP_TRANSACTION表插入批次出站的操作记录。
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT;
            if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            }
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //向WIP_STEP_TRANSACTION表插入批次出站的操作记录。
            WIP_STEP_TRANSACTION_FIELDS wipStepTransFields = new WIP_STEP_TRANSACTION_FIELDS();
            htStepTransaciont[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            sql = DatabaseTable.BuildInsertSqlStatement(wipStepTransFields, htStepTransaciont, null);
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
        /// 出站后自动进站。
        /// </summary>
        /// <param name="dsParams">包含出站信息的数据集对象。</param>
        private void TrackOutAutoTrackIn(DataSet dsParams)
        {
            const string TITLE="批次进站异常";
            const string OBJECT_TYPE="LOT";
            const string TO_GROUP="EMSGOUT";

            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
            DataTable dtTransaction = dsParams.Tables[WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放操作数据
            DataTable dtStepTransaction = dsParams.Tables[WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME];  //存放下一工步信息的数据 

            Hashtable htAutoTrackInTransaction = CommonUtils.ConvertToHashtable(dtTransaction);
            Hashtable htStepTransaciont = CommonUtils.ConvertToHashtable(dtStepTransaction);
            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);

            string operationKey = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);  //工序主键

            string lotKey = Convert.ToString(htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY]);//批次主键
            double leftQty = Convert.ToDouble(htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT]);//进站后数量
            string editor = Convert.ToString(htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);//编辑人
            string editTimeZone = Convert.ToString(htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);//编辑时区
            //下一工步数据
            string toEnterpriseKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY]);//下一个工艺流程组主键。
            string toRouteKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY]);//下一个工艺流程主键。
            string toStepKey = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);//下一个工步主键。
            string toEnterpriseName = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME]);//下一个工艺流程组名称。
            string toRouteName = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME]);//下一个工艺流程名称。
            string toStepName = Convert.ToString(htStepTransaciont[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME]);//下一个工步名称。
            try
            {
                DataSet dsStepData = RemotingServer.ServerObjFactory.CreateIRouteEngine().GetStepBaseDataAndParamDataByKey(toStepKey, 0);
                string msg = ReturnMessageUtils.GetServerReturnMessage(dsStepData);
                //是否获取到工步基本数据及其工步参数数据。
                if (string.IsNullOrEmpty(msg)
                    && null != dsStepData
                    && dsStepData.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME)
                    && dsStepData.Tables.Contains(POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME)
                    && dsStepData.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0
                    )
                {
                    msg = string.Format("[{0}]进站时需要输入工步参数，自动进站失败。", toStepName);
                    WipManagement.RecordErrorMessage(db,TITLE, msg, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);
                    //工步参数
                    return;
                }
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = toEnterpriseKey;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = toEnterpriseName;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = toRouteKey;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = toRouteName;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = toStepKey;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = toStepName;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY] = string.Empty;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = leftQty;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = leftQty;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = 0;
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = string.Empty;
                //根据工序获取设备。
                htAutoTrackInTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = string.Empty;
                //重新获取批次信息。
                DataSet dsLotInfo = RemotingServer.ServerObjFactory.CreateILotEngine().GetLotInfo(lotKey);
                DataRow drLotInfo=dsLotInfo.Tables[0].Rows[0];

                //组织进站使用的参数
                Hashtable htAutoTrackInParams = CommonUtils.ConvertToHashtable(dtParams);
                htAutoTrackInParams[POR_LOT_FIELDS.FIELD_EDIT_TIME] = drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME];
                DataTable dtAutoTrackInParams = CommonUtils.ParseToDataTable(htAutoTrackInParams);
                dtAutoTrackInParams.TableName = TRANS_TABLES.TABLE_PARAM;

                DataTable dtAutoTrackInTransaction = CommonUtils.ParseToDataTable(htAutoTrackInTransaction);
                dtAutoTrackInTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

                DataSet dsAutoTrackInParams = new DataSet();
                dsAutoTrackInParams.Tables.Add(dtAutoTrackInTransaction);
                dsAutoTrackInParams.Tables.Add(dtAutoTrackInParams);

                DataSet dsReturn = LotTrackIn(dsAutoTrackInParams);
                int code = 0;
                msg = ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                if (!string.IsNullOrEmpty(msg))
                {
                    WipManagement.RecordErrorMessage(db,TITLE , msg, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);
                }
            }
            catch (Exception ex)
            {
                WipManagement.RecordErrorMessage(db, TITLE, ex.Message, editor, TO_GROUP, editor, editTimeZone, lotKey, OBJECT_TYPE);
                LogService.LogError("TrackOutAutoTrackIn Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 检查IV测试数据是否存在。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="trackInTime">进站时间。</param>
        /// <returns>false:不存在。true：存在。</returns>
        public bool CheckIVTestData(string lotNumber, DateTime trackInTime)
        {
            //根据批次号和批次进站时间,抓取测试数据新增时间>批次进站时间的当前有效测试数据的记录数
            string sql = string.Format(@"SELECT COUNT(*) 
                                        FROM WIP_IV_TEST
                                        WHERE LOT_NUM='{0}' 
                                        AND DT_CREATE>='{1}'
                                        AND VC_DEFAULT=1",
                                        lotNumber.PreventSQLInjection(),
                                        trackInTime);
            int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
            //记录数为0
            if (count<=0)
            {
                return false;
            }
            //记录数>0
            return true;
        }

        /// <summary>
        /// 工单在过站时检查花色是否必须输入。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>true：需要检查花色。false：不用检查花色。</returns>
        public bool IsCheckColorByWorkOrder(string workOrderNo)
        {
            string sql = string.Format(@"SELECT a.ATTRIBUTE_VALUE
                                    FROM POR_WORK_ORDER_ATTR a
                                    INNER JOIN POR_WORK_ORDER b ON a.WORK_ORDER_KEY=b.WORK_ORDER_KEY
                                    WHERE a.ATTRIBUTE_NAME='isMustInputModuleColorByCleanOpt'
                                    AND a.ISFLAG=1
                                    AND b.ORDER_NUMBER='{0}'",
                                    workOrderNo.PreventSQLInjection());
            object objMustInput = db.ExecuteScalar(CommandType.Text, sql);
            if (objMustInput == DBNull.Value || objMustInput == null)
            {
                return false;
            }
            bool isMustInput = Convert.ToBoolean(objMustInput);
            return isMustInput;
        }
        /// <summary>
        /// 获取组件有效的IV测试时间。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>组件有效的IV测试时间。</returns>
        public DateTime GetLotValidIVTestTime(string lotNumber)
        {
            //根据批次号抓取当前有效测试数据的测试时间
            string sql = string.Format(@"SELECT TOP 1 TTIME
                                        FROM WIP_IV_TEST
                                        WHERE LOT_NUM='{0}'
                                        AND VC_DEFAULT=1",
                                        lotNumber.PreventSQLInjection());
            object obj = db.ExecuteScalar(CommandType.Text, sql);
            if (obj==null || obj==DBNull.Value)
            {
                return DateTime.MinValue;
            }
            DateTime ttime = Convert.ToDateTime(obj);
            //测试时间。
            return ttime;
        }

    }
}
