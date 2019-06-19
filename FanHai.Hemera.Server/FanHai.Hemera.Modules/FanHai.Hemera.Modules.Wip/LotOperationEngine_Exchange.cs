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
    /// 包含批次操作方法的类-转返工单。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// 比对等级是否符合工单和产品的要求。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="grade">等级。</param>
        /// <returns>true：符合;false:不符合。</returns>
        public bool CompareExchangeGrade(string orderNumber, string partNumber, string grade)
        {
            try
            {
                //转工单的时候，如果工单没有设置产品等级，默认为全等级都要。不判断产品等级
                string sqlCommand = string.Format(@"SELECT COUNT(1)
                                                    FROM POR_WO_PRD_LEVEL a
                                                    INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                    WHERE a.IS_USED='Y'
                                                    AND a.PART_NUMBER='{0}'
                                                    AND b.ORDER_NUMBER='{1}'",
                                                    partNumber.PreventSQLInjection(),
                                                    orderNumber.PreventSQLInjection());
                int cnt = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (cnt == 0)
                {
                    return true;
                }
                //如果有设置等级，则卡控等级是否符合工单产品要求。
                sqlCommand = string.Format(@"SELECT COUNT(1)
                                            FROM POR_WO_PRD_LEVEL a
                                            INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                            WHERE a.IS_USED='Y'
                                            AND a.GRADE='{0}'
                                            AND a.PART_NUMBER='{1}'
                                            AND b.ORDER_NUMBER='{2}'",
                                            grade.PreventSQLInjection(),
                                            partNumber.PreventSQLInjection(),
                                            orderNumber.PreventSQLInjection());
                cnt = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                return cnt > 0;
            }
            catch (Exception ex)
            {
                LogService.LogError("LotOperationEngine.CompareExchangeGrade Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据托盘号和批次号获取返工数据。
        /// </summary>
        /// <remarks>
        /// 只输入托盘号，根据托盘号获取返工明细数据。
        /// 只输入批次号，根据批次号获取返工明细数据。
        /// 同时输入托盘号和批次号，获取返工明细数据。
        /// </remarks>
        /// <returns>包含返工明细数据的数据集对象。</returns>
        public DataSet GetExchangeData(string packageNo, string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT ROW_NUMBER() OVER(ORDER BY t.LOT_NUMBER) SEQ,
                                               t.LOT_KEY,
                                               t.LOT_NUMBER,
                                               t.WORK_ORDER_NO,
                                               t.PRO_ID,
                                               t.PALLET_NO,
                                               t.PALLET_TIME,
                                               t.PART_NUMBER,
                                               t.LOT_SIDECODE,
                                               t.LOT_CUSTOMERCODE,
                                               t.COLOR,
                                               t.EDIT_TIME,
                                               t.PRO_LEVEL,
                                               t.STATE_FLAG,
                                               t.FACTORYROOM_KEY,
                                               t.HOLD_FLAG,
                                               t.FACTORYROOM_NAME,
                                               b.GRADE_NAME,
                                               a.DEVICENUM,a.VC_DEFAULT,a.T_DATE,a.VC_TYPE,a.I_IDE,a.VC_MODNAME,a.I_PKID,
                                               a.PM,a.FF,a.IPM,a.ISC,a.VPM,a.VOC,
                                               a.COEF_PMAX,a.COEF_FF,a.COEF_IMAX,a.COEF_ISC,a.COEF_VMAX,a.COEF_VOC
                                        FROM POR_LOT t 
                                        LEFT JOIN WIP_IV_TEST a ON a.LOT_NUM=t.LOT_NUMBER AND a.VC_DEFAULT=1
                                        LEFT JOIN V_ProductGrade b ON b.GRADE_CODE=t.PRO_LEVEL
                                        WHERE t.DELETED_TERM_FLAG<2");

                if (!string.IsNullOrEmpty(packageNo))
                {
                    sbSql.AppendFormat("AND t.PALLET_NO='{0}'", packageNo.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(lotNo))
                {
                    sbSql.AppendFormat("AND t.LOT_NUMBER='{0}'", lotNo.PreventSQLInjection());
                }

                DataTable dtExchangeDetail = db.ExecuteDataSet(CommandType.Text, sbSql.ToString()).Tables[0];
                dtExchangeDetail.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtExchangeDetail, true, MissingSchemaAction.Add);

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetExchangeData Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取组件对应的工艺流程中组件所在工序的属性信息
        /// </summary>
        /// <param name="lotNo">批次号</param>
        /// <returns>工序对应的属性信息</returns>
        public DataSet GetLotRouteAttrData(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;

                sql = string.Format(@"DECLARE @routeKey  VARCHAR(MAX);

                                      SET @routeKey = (SELECT CUR_STEP_VER_KEY 
				                                       FROM POR_LOT
				                                       WHERE LOT_NUMBER = '{0}'
                                                       AND STATUS<2);
                                      				 
                                      SELECT ATTRIBUTE_NAME,ATTRIBUTE_VALUE 
                                      FROM POR_ROUTE_STEP_ATTR 
                                      WHERE ROUTE_STEP_KEY = @routeKey;", 
                                      lotNo.PreventSQLInjection());

                DataTable dtLotRouteAttrData = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtLotRouteAttrData.TableName = "LOT_ROUTE_ATTR";
                dsReturn.Merge(dtLotRouteAttrData, true, MissingSchemaAction.Add);

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetLotRouteAttrData Error: " + ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// 获取工单产品数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单及其产品数据的数据集对象。</returns>
        public DataSet GetWoProductData(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT *
                                    FROM POR_WORK_ORDER 
                                    WHERE ORDER_NUMBER='{0}'
                                    AND (ORDER_STATE='REL' OR ORDER_STATE='TECO')",
                                    orderNumber.PreventSQLInjection());
                DataTable dtWorkorder = db.ExecuteDataSet(CommandType.Text, sbSql.ToString()).Tables[0];
                dtWorkorder.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;


                string sqlCommand = string.Format(@"SELECT a.ITEM_NO,b.WORK_ORDER_KEY,b.ORDER_NUMBER,a.PART_NUMBER,c.PART_DESC,a.PRODUCT_CODE,a.MINPOWER,a.MAXPOWER,a.POWER_DEGREE
                                                FROM POR_WO_PRD a
                                                INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                LEFT JOIN POR_PART c ON c.PART_ID=a.PART_NUMBER AND c.PART_STATUS=1 
                                                WHERE b.ORDER_NUMBER='{0}'
                                                AND a.IS_USED='Y'
                                                ORDER BY a.ITEM_NO",
                                                orderNumber.PreventSQLInjection());

                DataTable dtProduct = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtProduct.TableName = POR_PART_FIELDS.DATABASE_TABLE_NAME;

                dsReturn.Merge(dtWorkorder, true, MissingSchemaAction.Add);
                dsReturn.Merge(dtProduct, true, MissingSchemaAction.Add);

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetWoProductData Error: " + ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// 转返工单作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotExchange(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)            //存放批次数据
                )
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                return dsReturn;
            }

            try
            {
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                DataTable dtLots = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_EDITOR]);         //编辑人
                string opComputer = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);         //编辑人
                string enterpriseKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string routeKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string stepKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                string activity= Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        foreach (DataRow dr in dtLots.Rows)
                        {
                            string lotKey=Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                            string lotNumber=Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                            string ttime = Convert.ToString(dr[WIP_IV_TEST_FIELDS.FIELDS_T_DATE]);
                            string palletNo = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_PALLET_NO]);

                            //检查记录是否过期。防止重复修改。
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                            List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                            listCondition.Add(kvp);
                            string opEditTime = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                            //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                            if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("组件{0}信息已过期，请确认。", lotNumber));
                                return dsReturn;
                            }
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
                            string transactionKey = CommonUtils.GenerateNewKey(0);
                            AddWIPLot(dbTran, transactionKey, lotKey);
                            //向WIP_TRANSACTION表插入批次转返工单的操作记录。
                            Hashtable htTransaction = new Hashtable();
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY,activity);
                            if (htParams.Contains(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT))
                            {
                                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
                            }
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, null);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, null);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, editor);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, opComputer);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, null);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, 0);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                            string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //更新批次数据
                            string updateLot =string.Format(@"UPDATE POR_LOT 
                                                              SET STATE_FLAG=0,
                                                                  {12}
                                                                  DELETED_TERM_FLAG=0,
                                                                  ROUTE_ENTERPRISE_VER_KEY='{1}',
                                                                  CUR_ROUTE_VER_KEY='{2}',
                                                                  CUR_STEP_VER_KEY='{3}',
                                                                  EDITOR='{4}',
                                                                  EDIT_TIME=GETDATE(),
                                                                  FACTORYROOM_KEY='{5}',
                                                                  FACTORYROOM_NAME='{6}',
                                                                  WORK_ORDER_NO='{7}',
                                                                  WORK_ORDER_KEY='{8}',
                                                                  PART_NUMBER='{9}',
                                                                  PRO_ID='{10}',
                                                                  CUR_PRODUCTION_LINE_KEY=NULL,
                                                                  LINE_NAME=NULL,
                                                                  START_WAIT_TIME=GETDATE(),
                                                                  START_PROCESS_TIME=NULL,
                                                                  PALLET_NO=NULL,
                                                                  PALLET_TIME=NULL,
                                                                  OPR_COMPUTER='{4}',
                                                                  OPR_LINE=NULL,
                                                                  OPERATOR='{11}'
                                                              WHERE LOT_KEY='{0}'",
                                                              lotKey.PreventSQLInjection(),
                                                              enterpriseKey.PreventSQLInjection(),
                                                              routeKey.PreventSQLInjection(),
                                                              stepKey.PreventSQLInjection(),
                                                              editor.PreventSQLInjection(),
                                                              dr[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY],
                                                              dr[POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME],
                                                              dr["NEW_WORK_ORDER_NO"],
                                                              dr["NEW_WORK_ORDER_KEY"],
                                                              dr["NEW_PART_NUMBER"],
                                                              dr["NEW_PRO_ID"],
                                                              opComputer.PreventSQLInjection(),
                                                              activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_PROID?  "REWORK_FLAG=REWORK_FLAG+1," : string.Empty);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, updateLot);
                            //如果有有效测试数据，更新测试数据
                            if (!string.IsNullOrEmpty(ttime))
                            {
                                string updateIVTest = string.Format(@"UPDATE WIP_IV_TEST 
                                                              SET VC_TYPE='{1}',I_IDE='{2}',I_PKID='{3}',VC_MODNAME='{4}',VC_WORKORDER='{5}'
                                                              WHERE LOT_NUM='{0}'
                                                              AND VC_DEFAULT='1'",
                                                              lotNumber.PreventSQLInjection(),
                                                              dr[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE],
                                                              dr[WIP_IV_TEST_FIELDS.FIELDS_I_IDE],
                                                              dr[WIP_IV_TEST_FIELDS.FIELDS_I_PKID],
                                                              dr[WIP_IV_TEST_FIELDS.FIELDS_VC_MODNAME],
                                                              dr["NEW_WORK_ORDER_NO"]);
                                this.db.ExecuteNonQuery(dbTran, CommandType.Text, updateIVTest);
                            }
                        }
                        //更新包装数据。
                        var lnq = dtLots.AsEnumerable().Where(dr=>string.IsNullOrEmpty(Convert.ToString(dr[POR_LOT_FIELDS.FIELD_PALLET_NO]))==false)
                                                       .Select(dr=>Convert.ToString(dr[POR_LOT_FIELDS.FIELD_PALLET_NO]))
                                                       .Distinct();
                        foreach(string palletNo in lnq)
                        {
                            string consignmentKey=CommonUtils.GenerateNewKey(0);
                            //新增包装明细数据
                            string sql=string.Format(@"INSERT INTO WIP_CONSIGNMENT_DETAIL
                                                        (CONSIGNMENT_KEY,ITEM_NO,LOT_NUMBER,WORK_NUMBER,PART_NUMBER,PRO_ID,PRO_LEVEL,
                                                        COLOR,POWER_LEVEL,PS_CODE,PS_DTL_CODE,FULL_QTY,PS_SEQ,AVG_POWER_RANGE,CREATOR,CREATE_TIME)
                                                       SELECT '{0}',
                                                               ROW_NUMBER() OVER(PARTITION BY a.CONSIGNMENT_KEY ORDER BY b.ITEM_NO) ITEM_NO,
                                                               b.LOT_NUMBER,
                                                               b.WORK_NUMBER,
                                                               b.PART_NUMBER,
                                                               b.PRO_ID,
                                                               b.PRO_LEVEL,
                                                               b.COLOR,
                                                               b.POWER_LEVEL,
                                                               b.PS_CODE,
                                                               b.PS_DTL_CODE,
                                                               b.FULL_QTY,
                                                               b.PS_SEQ,
                                                               b.AVG_POWER_RANGE,
                                                               b.CREATOR,
                                                               b.CREATE_TIME
                                                        FROM WIP_CONSIGNMENT a
                                                        INNER JOIN WIP_CONSIGNMENT_DETAIL b ON b.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY
                                                        INNER JOIN POR_LOT c ON c.PALLET_NO=a.VIRTUAL_PALLET_NO AND c.LOT_NUMBER=b.LOT_NUMBER 
                                                        WHERE a.ISFLAG=1
                                                        AND a.PALLET_NO='{1}'",
                                                        consignmentKey,
                                                        palletNo.PreventSQLInjection());
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //新增包装数据
                            sql = string.Format(@"INSERT INTO WIP_CONSIGNMENT(CONSIGNMENT_KEY,VIRTUAL_PALLET_NO,PALLET_NO,WORKNUMBER,CS_DATA_GROUP,
                                                    SAP_NO,POWER_LEVEL,GRADE,SHIFT,PS_CODE,PS_DTL_SUBCODE,
                                                    LAST_PALLET,CREATER,CREATE_TIME,EDITOR,EDIT_TIME,ISFLAG,
                                                    ROOM_KEY,CUSTOMER_NO,LOT_NUMBER_QTY,FULL_QTY,TOTLE_POWER,
                                                    AVG_POWER,PRO_ID,PALLET_NO_NEW,PALLET_TYPE,
                                                    CODE_TYPE,LINE_NAME,LINE_KEY,EQUIPMENT_KEY,EQUIPMENT_NAME,
                                                    AVG_POWER_RANGE,LOT_COLOR,PS_SEQ,CHECKER,CHECK_TIME,
                                                    TO_WH,TO_WH_TIME,OUT_WH,OUT_WH_TIME,MEMO1,ARK_FLAG)
                                                SELECT '{0}', a.VIRTUAL_PALLET_NO,a.PALLET_NO,a.WORKNUMBER,a.CS_DATA_GROUP,
                                                       a.SAP_NO,a.POWER_LEVEL,a.GRADE,a.SHIFT,a.PS_CODE,a.PS_DTL_SUBCODE,
                                                       a.LAST_PALLET,a.CREATER,a.CREATE_TIME,'{2}',GETDATE(),a.ISFLAG,
                                                       a.ROOM_KEY,a.CUSTOMER_NO,a.LOT_NUMBER_QTY,a.FULL_QTY,a.TOTLE_POWER,
                                                       a.AVG_POWER,a.PRO_ID,a.PALLET_NO_NEW,a.PALLET_TYPE,
                                                       a.CODE_TYPE,a.LINE_NAME,a.LINE_KEY,a.EQUIPMENT_KEY,a.EQUIPMENT_NAME,
                                                       a.AVG_POWER_RANGE,a.LOT_COLOR,a.PS_SEQ,a.CHECKER,a.CHECK_TIME,
                                                       a.TO_WH,a.TO_WH_TIME,a.OUT_WH,a.OUT_WH_TIME,a.MEMO1,a.ARK_FLAG
                                                FROM WIP_CONSIGNMENT a
                                                WHERE a.ISFLAG=1
                                                AND a.PALLET_NO='{1}'",
                                                consignmentKey,
                                                palletNo.PreventSQLInjection(),
                                                editor.PreventSQLInjection());
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            //更新原来的包装数据为无效
                            sql=string.Format(@"UPDATE WIP_CONSIGNMENT
                                            SET ISFLAG=0,EDITOR='{2}',EDIT_TIME=GETDATE(),MEMO1='转/返工单 {3} {4}'
                                            WHERE ISFLAG=1
                                            AND PALLET_NO='{1}'
                                            AND CONSIGNMENT_KEY!='{0}'", 
                                            consignmentKey,
                                            palletNo.PreventSQLInjection(),
                                            editor.PreventSQLInjection(),
                                            activity,
                                            htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //更新现有的包装数据，为其第一条数据的基本数据
                            sql = string.Format(@"UPDATE a
                                                SET    a.WORKNUMBER=b.WORK_NUMBER,
                                                       a.SAP_NO=b.PART_NUMBER,
                                                       a.POWER_LEVEL=b.POWER_LEVEL,
                                                       a.GRADE=b.PRO_LEVEL,
                                                       a.PS_CODE=b.PS_CODE,
                                                       a.PS_DTL_SUBCODE=b.PS_DTL_CODE,
                                                       a.LOT_NUMBER_QTY=(SELECT COUNT(1) FROM WIP_CONSIGNMENT_DETAIL aa WHERE aa.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY),
                                                       a.FULL_QTY=b.FULL_QTY,
                                                       a.TOTLE_POWER=(SELECT ISNULL(SUM(bb.COEF_PMAX),0)
                                                                      FROM WIP_CONSIGNMENT_DETAIL aa
                                                                      INNER JOIN WIP_IV_TEST bb ON bb.LOT_NUM=aa.LOT_NUMBER AND bb.VC_DEFAULT='1'
                                                                      WHERE aa.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY),
                                                       a.PRO_ID=b.PRO_ID,
                                                       a.LOT_COLOR=b.COLOR,
                                                       a.PS_SEQ=b.PS_SEQ,
                                                       a.AVG_POWER_RANGE=b.AVG_POWER_RANGE
                                                FROM WIP_CONSIGNMENT a
                                                LEFT JOIN WIP_CONSIGNMENT_DETAIL b ON b.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY AND b.ITEM_NO=1
                                                WHERE a.CONSIGNMENT_KEY='{0}'",
                                                consignmentKey);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            sql = string.Format(@"UPDATE a
                                                SET    a.AVG_POWER=CASE WHEN a.LOT_NUMBER_QTY=0 THEN 0 ELSE a.TOTLE_POWER/a.LOT_NUMBER_QTY END,
                                                       a.ISFLAG=CASE WHEN a.LOT_NUMBER_QTY>0 THEN 1 ELSE 0 END
                                                FROM WIP_CONSIGNMENT a
                                                WHERE a.CONSIGNMENT_KEY='{0}'",
                                                consignmentKey);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.LotExchange Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
