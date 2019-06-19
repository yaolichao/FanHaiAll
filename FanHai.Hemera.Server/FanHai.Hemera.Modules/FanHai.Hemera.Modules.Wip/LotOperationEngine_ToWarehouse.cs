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
        /// 根据托盘号获取托盘信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含托盘信息的数据集对象。</returns>
        public DataSet GetPalletInfo(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT A.*
                                    FROM WIP_CONSIGNMENT A
                                    WHERE A.VIRTUAL_PALLET_NO='{0}'
                                    AND A.ISFLAG=1",
                                    palletNo.PreventSQLInjection());
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据托盘号获取托盘上的批次信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含批次信息的数据集对象。</returns>
        public DataSet GetPalletLotInfo(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                using(DbConnection dbCon=db.CreateConnection())
                {
                    dbCon.Open();
                    using (DbTransaction dbTran = dbCon.BeginTransaction())
                    {
                        dsReturn = this.GetPalletLotInfo(dbTran, palletNo);
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                    dbCon.Close();
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletLotInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 托盘进站。
        /// </summary>
        /// <param name="dsParams">包含托盘过站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet PalletTrackIn(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                    || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                    )
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
                DataTable dtPalletInfo = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];  //存放托盘数据
                
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);       //编辑时区

                StringBuilder sbMsg = new StringBuilder();
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    //循环托盘
                    foreach (DataRow drPallet in dtPalletInfo.Rows)
                    {
                        string palletNo = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                        string palletKey = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]); //托盘主键
                        //根据托盘号获取托盘中当前的批次信息。
                        DataSet dsLotInfo = this.GetPalletLotInfo(null, palletNo);
                        if (dsLotInfo != null && dsLotInfo.Tables.Count > 0)
                        {
                            DataTable dtLotInfo = dsLotInfo.Tables[0];
                            //循环托盘中的批次信息
                            foreach (DataRow drLotInfo in dtLotInfo.Rows)
                            {
                                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                                using (DbTransaction dbTran = dbConn.BeginTransaction())
                                {
                                    if (stateFlag == 0)
                                    {
                                        //批次进站
                                        LotToWarehouseTrackIn(dbTran, drLotInfo, htParams);
                                    }
                                    dbTran.Commit();
                                }
                            }//结束循环
                        }
                    }
                    dbConn.Close();
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PalletTrackIn Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 托盘出站。
        /// </summary>
        /// <param name="dsParams">包含托盘过站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet PalletTrackOut(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                    || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                    )
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
                DataTable dtPalletInfo = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];  //存放托盘数据

                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);       //编辑时区

                StringBuilder sbMsg = new StringBuilder();
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    //循环托盘
                    foreach (DataRow drPallet in dtPalletInfo.Rows)
                    {
                        string palletNo = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                        string palletKey = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]); //托盘主键
                        //根据托盘号获取托盘中当前的批次信息。
                        DataSet dsLotInfo = this.GetPalletLotInfo(null, palletNo);
                        if (dsLotInfo != null && dsLotInfo.Tables.Count > 0)
                        {
                            DataTable dtLotInfo = dsLotInfo.Tables[0];
                            //循环托盘中的批次信息
                            foreach (DataRow drLotInfo in dtLotInfo.Rows)
                            {
                                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                                using (DbTransaction dbTran = dbConn.BeginTransaction())
                                {
                                    if (stateFlag > 0 && stateFlag<=9)
                                    {
                                        //批次出站
                                        LotToWarehouseTrackOut(dbTran, drLotInfo, htParams);
                                    }
                                    dbTran.Commit();
                                }
                            }//结束循环
                        }
                    }
                    dbConn.Close();
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PalletTrackOut Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 托盘入库作业。
        /// </summary>
        /// <param name="dsParams">包含托盘入库信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        public DataSet PalletToWarehouse(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //参数数据。
                if (dsParams == null
                    || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                    || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                    )
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];                         //存放附加参数数据
                DataTable dtPalletInfo = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];  //存放托盘数据
                //检查是否存在重复的托盘记录。
                var lnq = from item in dtPalletInfo.AsEnumerable()
                          group item by item[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复托盘记录，请检查。");
                    return dsReturn;
                }
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);       //编辑时区
                //验证托盘信息是否过期，防止重复修改。
                foreach (DataRow drPallet in dtPalletInfo.Rows)
                {
                    string opEditTime = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME]);   //操作前编辑时间
                    string palletKey = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]); //托盘主键
                    //检查记录是否过期。防止重复修改。
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, palletKey);
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                    if (UtilHelper.CheckRecordExpired(db, WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                    {
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "信息已过期，请关闭该界面后重试。");
                        return dsReturn;
                    }
                }
                //入库采集的参数数据。
                DataTable dtOperationParam=null;
                if(dsParams.Tables.Contains(WIP_PARAM_FIELDS.DATABASE_TABLE_NAME))
                {
                    dtOperationParam=dsParams.Tables[WIP_PARAM_FIELDS.DATABASE_TABLE_NAME];
                }
                StringBuilder sbMsg = new StringBuilder();
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    //循环托盘
                    foreach (DataRow drPallet in dtPalletInfo.Rows)
                    {
                        string palletNo = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                        string palletKey = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]); //托盘主键
                        //根据托盘号获取托盘中当前的批次信息。
                        DataSet dsLotInfo = this.GetPalletLotInfo(null, palletNo);
                        using (DbTransaction dbTran = dbConn.BeginTransaction())
                        {
                            try
                            {
                                if (dsLotInfo != null && dsLotInfo.Tables.Count > 0)
                                {
                                    DataTable dtLotInfo = dsLotInfo.Tables[0];
                                    //循环托盘中的批次信息
                                    foreach (DataRow drLotInfo in dtLotInfo.Rows)
                                    {
                                        int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                                        if (stateFlag == 0)
                                        {
                                            //批次进站
                                            LotToWarehouseTrackIn(dbTran, drLotInfo, htParams);
                                        }
                                        if (stateFlag <= 9)
                                        {
                                            //批次出站
                                            LotToWarehouseTrackOut(dbTran, drLotInfo, htParams);
                                        }
                                        //批次入库
                                        LotToWarehouse(dbTran, drLotInfo, dtOperationParam, htParams);
                                    }//结束循环
                                }
                                //更新托盘信息为入库。
                                StringBuilder sbUpdateSql = new StringBuilder();
                                sbUpdateSql.AppendFormat(@"UPDATE WIP_CONSIGNMENT 
                                                           SET CS_DATA_GROUP=3,EDITOR='{1}',EDIT_TIME=GETDATE(),TO_WH='{1}',TO_WH_TIME=GETDATE()
                                                           WHERE CONSIGNMENT_KEY='{0}'",
                                                           palletKey.PreventSQLInjection(),
                                                           editor.PreventSQLInjection());
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateSql.ToString());
                                dbTran.Commit();
                            }
                            catch (Exception ex)
                            {
                                sbMsg.AppendFormat("托号:{0}入库失败，请重新入库，失败原因：{1}。\r\n", palletNo, ex.Message);
                                dbTran.Rollback();
                            }
                        }
                    }
                    dbConn.Close();
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PalletToWarehouse Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 批次入库进站。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="drLotInfo">批次信息。</param>
        /// <param name="htParams">附近参数数据。</param>
        private void LotToWarehouseTrackIn(DbTransaction dbTran, DataRow drLotInfo, Hashtable htParams)
        {
            string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE,0);
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
            DataTable dtTrackInTransaction=CommonUtils.ParseToDataTable(htTransaction);
            dtTrackInTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

            Hashtable htTrackInParams = new Hashtable();
            htTrackInParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
                                htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            DataTable dtTrackInParams=CommonUtils.ParseToDataTable(htTrackInParams);
            dtTrackInParams.TableName = TRANS_TABLES.TABLE_PARAM;

            DataSet dsTrackInParams = new DataSet();
            dsTrackInParams.Tables.Add(dtTrackInTransaction);
            dsTrackInParams.Tables.Add(dtTrackInParams);

            LotTrackIn(dbTran, dsTrackInParams);
            //更新批次状态,9-等待出站
            string sql = string.Format(@"UPDATE POR_LOT SET STATE_FLAG=9
                                        WHERE LOT_KEY='{0}'",
                                        lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
        }
        /// <summary>
        /// 批次入库出站。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="drLotInfo">批次信息。</param>
        /// <param name="htParams">附近参数数据。</param>
        private void LotToWarehouseTrackOut(DbTransaction dbTran, DataRow drLotInfo, Hashtable htParams)
        {
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
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
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, 9);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            DataTable dtTrackOutTransaction = CommonUtils.ParseToDataTable(htTransaction);
            dtTrackOutTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

            Hashtable htTrackOutParams = new Hashtable();
            htTrackOutParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
                                htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            DataTable dtTrackOutParams = CommonUtils.ParseToDataTable(htTrackOutParams);
            dtTrackOutParams.TableName = TRANS_TABLES.TABLE_PARAM;

            Hashtable htStepTransaction = new Hashtable();
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            DataTable dtTrackOutStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
            dtTrackOutStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

            DataSet dsTrackOutParams = new DataSet();
            dsTrackOutParams.Tables.Add(dtTrackOutTransaction);
            dsTrackOutParams.Tables.Add(dtTrackOutParams);
            dsTrackOutParams.Tables.Add(dtTrackOutStepTransaction);

            LotTrackOut(dbTran, dsTrackOutParams,true);
        }
        /// <summary>
        /// 批次入库。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="drLotInfo">批次信息。</param>
        /// <param name="dtOperationParam">工序入库参数数据。</param>
        /// <param name="htParams">附近参数数据。</param>
        private void LotToWarehouse(DbTransaction dbTran, DataRow drLotInfo,DataTable dtOperationParam, Hashtable htParams)
        {
            string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TO_WAREHOUSE);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
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
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, 9);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);

            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);
            //向WIP_TRANSACTION表插入批次入库的操作记录。
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            if (!htTransaction.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            }
            htTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
            string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //工步采集参数数据。
            //如果数据集中包含名称WIP_PARAM_FIELDS.DATABASE_TABLE_NAME的数据表对象。
            if (dtOperationParam!=null)
            {
                WIP_PARAM_FIELDS wipParamFields = new WIP_PARAM_FIELDS();
                //遍历批次的工步采集参数数据。
                for (int i = 0; i < dtOperationParam.Rows.Count; i++)
                {
                    DataRow drWIPParam = dtOperationParam.Rows[i];
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
            //更新批次状态,11-已入库
            sql = string.Format(@"UPDATE POR_LOT SET STATE_FLAG=11,EDIT_TIME=GETDATE()
                                WHERE LOT_KEY='{0}'",
                                lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
        }
        /// <summary>
        /// 根据托盘号获取托盘上的批次信息。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含批次信息的数据集对象。</returns>
        private DataSet GetPalletLotInfo(DbTransaction dbTran,string palletNo)
        {
            string sql = @"SELECT A.*,
                            B.ENTERPRISE_NAME,
                            B.ENTERPRISE_VERSION,
                            C.ROUTE_NAME ,
                            D.ROUTE_STEP_NAME
                        FROM POR_LOT A
                        LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                        LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                        LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                        WHERE A.STATUS < 2
                        AND A.PALLET_NO='{0}'";
            if (dbTran != null)
            {
                return db.ExecuteDataSet(dbTran, CommandType.Text, string.Format(sql, palletNo.PreventSQLInjection()));
            }
            else
            {
                return db.ExecuteDataSet(CommandType.Text, string.Format(sql, palletNo.PreventSQLInjection()));
            }
        }
        
    }
}
