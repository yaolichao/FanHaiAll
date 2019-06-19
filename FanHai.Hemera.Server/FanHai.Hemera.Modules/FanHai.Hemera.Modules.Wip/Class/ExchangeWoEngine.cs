using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.Wip
{  
    /// <summary>
    /// 转工单操作类。
    /// </summary>
    public class ExchangeWoEngine : AbstractEngine, IExchangeWoEngine
    {
        private Database db = null; //数据库操作对象。
        private Database dbHis = null;//历史数据库底层操作对象
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ExchangeWoEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            dbHis = DatabaseFactory.CreateDatabase("SQLServerHis");
        }
        /// <summary>
        /// 获得转工单信息查询界面数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetExchangeWoData(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records, Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            pages = 0;
            records = 0;
            try
            {
                #region sql text

                sqlCommand = string.Format(@"SELECT  t2.TRANSACTION_KEY, 
                                                 t.LOT_KEY,t.LOT_NUMBER,
                                                ISNULL((SELECT TOP 1 b.WORK_ORDER_NO
                                                 FROM WIP_TRANSACTION a
                                                 INNER JOIN WIP_LOT b ON b.TRANSACTION_KEY=a.TRANSACTION_KEY
                                                 WHERE a.PIECE_KEY=t2.PIECE_KEY
                                                 AND a.TIME_STAMP>t2.TIME_STAMP
                                                 ORDER BY TIME_STAMP ASC ),t.WORK_ORDER_NO) WORK_ORDER_NO,

                                                ISNULL((SELECT TOP 1 b.PART_NUMBER
                                                 FROM WIP_TRANSACTION a
                                                 INNER JOIN WIP_LOT b ON b.TRANSACTION_KEY=a.TRANSACTION_KEY
                                                 WHERE a.PIECE_KEY=t2.PIECE_KEY
                                                 AND a.TIME_STAMP>t2.TIME_STAMP
                                                 ORDER BY TIME_STAMP ASC ),t.PART_NUMBER) PART_NUMBER,

                                                ISNULL((SELECT TOP 1 b.PRO_ID
                                                 FROM WIP_TRANSACTION a
                                                 INNER JOIN WIP_LOT b ON b.TRANSACTION_KEY=a.TRANSACTION_KEY
                                                 WHERE a.PIECE_KEY=t2.PIECE_KEY
                                                 AND a.TIME_STAMP>t2.TIME_STAMP
                                                 ORDER BY TIME_STAMP ASC ),t.PRO_ID) PRO_ID,
                                                 t1.WORK_ORDER_NO WORK_ORDER_NO2,t1.PART_NUMBER PART_NUMBER2,t1.PRO_ID PRO_ID2,
                                                 t2.EDITOR EDITOR2,T2.TIME_STAMP EDIT_TIME2,t2.ACTIVITY_COMMENT
                                            FROM WIP_TRANSACTION t2
                                            INNER JOIN POR_LOT t ON t2.PIECE_KEY=t.LOT_KEY
                                            INNER JOIN WIP_LOT t1 ON t2.TRANSACTION_KEY=t1.TRANSACTION_KEY");

                #endregion
                if (hstable.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY))
                    sqlCommand += string.Format(" and  t2." + WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY + "='{0}'", hstable[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY].ToString());

                if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                    sqlCommand += string.Format(" and T." + POR_LOT_FIELDS.FIELD_LOT_NUMBER + "='{0}'", hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString());
                if (hstable.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER))
                    sqlCommand += string.Format(" and T." + POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "='{0}'", hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString());

                if (hstable.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + "2"))
                    sqlCommand += string.Format(" and T1." + POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "='{0}'", hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + "2"].ToString());

                if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TIME) && hstable.ContainsKey(POR_LOT_FIELDS.FIELD_EDIT_TIME))
                {
                    sqlCommand += string.Format(" and T2." + WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP + " BETWEEN CONVERT(datetime,'{0}') AND CONVERT(datetime,'{1}')",
                        hstable[POR_LOT_FIELDS.FIELD_CREATE_TIME].ToString(), hstable[POR_LOT_FIELDS.FIELD_EDIT_TIME].ToString());
                }
                else if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TIME))
                    sqlCommand += string.Format(" and T2." + WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP + ">= CONVERT(datetime,'{0}')", hstable[POR_LOT_FIELDS.FIELD_CREATE_TIME].ToString());
                else if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_EDIT_TIME))
                    sqlCommand += string.Format(" and T2." + WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP + "<= CONVERT(datetime,'{0}')", hstable[POR_LOT_FIELDS.FIELD_EDIT_TIME].ToString());


                if (pageNo > 0 && pageSize > 0)
                {
                    //分页查询。
                    AllCommonFunctions.CommonPagingData(sqlCommand, pageNo, pageSize, out pages,
                        out records, db, dsReturn, POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME, POR_LOT_FIELDS.FIELD_LOT_NUMBER);
                }
                else
                {
                    db.LoadDataSet(CommandType.Text, sqlCommand, dsReturn, new string[] { POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME });
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetExchangeWoData Error: " + ex.Message);
            }

            return dsReturn;
        }

        public DataSet GetExchangeByFilter(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtCommon = new DataTable();
            string sqlCommand = string.Empty;
            string errorMsg = string.Empty;
            try
            {
                string flag = hstable["flag"].ToString();
                string sRepair = string.Empty;
                //是否
                string isFromOperation = string.Empty;
                if (hstable.ContainsKey("ToStore"))
                    sRepair = "ToStore";
                if (hstable.ContainsKey("isFromOperation"))
                    isFromOperation = "isFromOperation";

                if (flag.Equals("l"))
                {
                    //线上转工单
                    #region  批次号
                    if (!sRepair.Equals("ToStore"))
                    {
                        sqlCommand = string.Format(@"select t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ,t.EDITOR,t.EDIT_TIME,t.ROUTE_ENTERPRISE_VER_KEY
                                                    ,t.SHIFT_NAME
                                                    from POR_LOT t 
                                                    LEFT JOIN POR_WORK_ORDER t1  ON t.WORK_ORDER_KEY=t1.WORK_ORDER_KEY 
                                                    LEFT JOIN POR_PRODUCT t2  ON t.PRO_ID=t2.PRODUCT_CODE
                                                    left join WIP_CONSIGNMENT t3 on t.PALLET_NO=t3.PALLET_NO and t3.ISFLAG=1
                                                    where t.DELETED_TERM_FLAG<>2 and t.LOT_NUMBER='{0}' and t.FACTORYROOM_KEY='{1}'",
                                                                                hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString(),
                                                                                hstable[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY].ToString());
                        dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        dtCommon.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dtCommon, true, MissingSchemaAction.Add);
                    }
                    #endregion
                    //从仓库返工单
                    #region
                    else
                    {
                        string palletno = string.Empty, lotno = string.Empty;

                        if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                            palletno = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
                        if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                            lotno = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();

                        DataTable dtConSigment = new DataTable(), dtPorLot = new DataTable();
                        //返工工单 1，先从正式库中查找资料；2，再从历史库中查找资料；3，最后从BCP中查找资料

                        //托号资料
                        if (!string.IsNullOrEmpty(palletno))
                        {
                            #region
                            sqlCommand = @"select t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                        t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                        t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                        t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                        t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER 
                                                        from WIP_CONSIGNMENT t where t.ISFLAG=1";

                            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                                sqlCommand += string.Format(@" and t.PALLET_NO='{0}'", Convert.ToString(hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO]));                           

                            dtConSigment = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                         
                            if (dtConSigment.Rows.Count < 1)
                            {
                                //从历史数据库中查找包装资料
                                sqlCommand = string.Format(@"select  t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                        t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                        t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                        t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                        t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER  
                                                        from WIP_CONSIGNMENT_HIS t where t.PALLET_NO='{0}' and t.ISFLAG=1",
                                  hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString());

                                dtConSigment = dbHis.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];                         
                            }
                            #endregion
                            //从BCP中读取数据
                            #region
                            if (isFromOperation.Equals("isFromOperation") && dtConSigment.Rows.Count < 1)
                            {
                                using (DbConnection dbConn = db.CreateConnection())
                                {
                                    //Open Connection
                                    dbConn.Open();
                                    //Create Transaction
                                    DbTransaction dbTran = dbConn.BeginTransaction();

                                    try
                                    {
                                        string roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
                                        string creator = Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_OPERATOR]);
                                        string computer = Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);

                                         errorMsg = ExportDataToAmesFromBcp(db, dbTran, palletno, lotno, roomkey, creator, computer);
                                        if (!string.IsNullOrEmpty(errorMsg))
                                        {
                                            dbTran.Rollback();
                                        }
                                        else
                                        {
                                            dbTran.Commit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        dbTran.Rollback();
                                    }
                                }
                                #region

                                sqlCommand = string.Format(@"select  t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                        t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                        t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                        t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                        t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER  
                                                        from WIP_CONSIGNMENT t where t.PALLET_NO='{0}' and t.ROOM_KEY='{1}' and t.ISFLAG=1",
                                   palletno, hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString());
                                dtConSigment = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];                                                        
                                #endregion
                            }
                            #endregion
                            //-----------------------------------------------------------------------------------
                            dtConSigment.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
                            dsReturn.Merge(dtConSigment, true, MissingSchemaAction.Add);

                            //先从正式库查找批次数据
                            #region
                            sqlCommand = string.Format(@"select  t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,t.STATE_FLAG,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,t.SHIFT_NAME,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ ,t.EDITOR,t.EDIT_TIME,t.ROUTE_ENTERPRISE_VER_KEY
                                                    from POR_LOT t where t.PALLET_NO='{0}' and t.DELETED_TERM_FLAG='1'",
                                 hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString());
                            dtPorLot = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                            if (dtPorLot.Rows.Count < 1)
                            {
                                //从历史库查找批次数据
                                sqlCommand = string.Format(@"select  t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,t.STATE_FLAG,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,t.SHIFT_NAME,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ,t.EDITOR,t.EDIT_TIME,t.ROUTE_ENTERPRISE_VER_KEY
                                                    from POR_LOT_HIS t where t.PALLET_NO='{0}' and t.DELETED_TERM_FLAG='1'",
                                  hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString());

                                dtPorLot = dbHis.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                            }
                            dtPorLot.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;


                            if (!dtPorLot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO))
                                dtPorLot.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO);
                            if (!dtPorLot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT))
                                dtPorLot.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT);
                            if (!dtPorLot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP))
                                dtPorLot.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP);
                            //在批次表中，添加SAP料号，包装班别，包装类别
                            if (dtConSigment.Rows.Count > 0)
                            {
                                DataRow drConsigment = dtConSigment.Rows[0];
                                foreach (DataRow drPorLot in dtPorLot.Rows)
                                {
                                    drPorLot[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO];
                                    drPorLot[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT];
                                    drPorLot[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP];
                                }
                            }
                            #endregion

                            dsReturn.Merge(dtPorLot, true, MissingSchemaAction.Add);
                        }
                        //根据批次号
                        else
                        {
                            //从正式库查找批次数据
                            #region
                            sqlCommand = string.Format(@"select  t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,t.STATE_FLAG,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,t.SHIFT_NAME,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ  ,t.EDITOR,t.EDIT_TIME,t.ROUTE_ENTERPRISE_VER_KEY
                                                    from POR_LOT t where t.LOT_NUMBER='{0}' and t.DELETED_TERM_FLAG='1'",lotno);
                            dtPorLot = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                            //从历史库查找批次数据
                            if (dtPorLot.Rows.Count < 1)
                            {
                                sqlCommand = string.Format(@"select  t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,t.STATE_FLAG,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,t.SHIFT_NAME,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ  ,t.EDITOR,t.EDIT_TIME,t.ROUTE_ENTERPRISE_VER_KEY
                                                    from POR_LOT_HIS t where t.LOT_NUMBER='{0}' and t.DELETED_TERM_FLAG='1'", lotno);
                                dtPorLot = dbHis.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                            }
                           
                            //从BCP中读取数据
                            #region
                            if (isFromOperation.Equals("isFromOperation") && dtPorLot.Rows.Count < 1)
                            {
                                using (DbConnection dbConn = db.CreateConnection())
                                {
                                    //Open Connection
                                    dbConn.Open();
                                    //Create Transaction
                                    DbTransaction dbTran = dbConn.BeginTransaction();

                                    try
                                    {
                                        string roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
                                        string creator = Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_OPERATOR]);
                                        string computer = Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);

                                         errorMsg = ExportDataToAmesFromBcp(db, dbTran, palletno, lotno, roomkey, creator, computer);
                                        if (!string.IsNullOrEmpty(errorMsg))
                                        {
                                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, errorMsg);

                                            dbTran.Rollback();
                                        }
                                        else
                                        {
                                            dbTran.Commit();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        dbTran.Rollback();
                                    }
                                }
                                #region
                                sqlCommand = string.Format(@"select  t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,t.SHIFT_NAME,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ ,t.EDITOR,t.EDIT_TIME,t.ROUTE_ENTERPRISE_VER_KEY
                                                    from POR_LOT t where t.LOT_NUMBER='{0}' and t.FACTORYROOM_KEY='{1}' and t.DELETED_TERM_FLAG='1'",
                                lotno, hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString());
                                dtPorLot = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                                #endregion
                            }
                            #endregion

                            #region 根据批次获得包装数据
                            sqlCommand = string.Format(@"select top 1   t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                        t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                        t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                        t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                        t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER 
                                                    from WIP_CONSIGNMENT t inner join POR_LOT t1 on t.PALLET_NO=t1.PALLET_NO
                                                    where t1.LOT_NUMBER='{0}' and t.ISFLAG=1
                                                    and t1.DELETED_TERM_FLAG=1 and t.ISFLAG=1",
                                                    lotno);
                            dtConSigment = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                            if (dtConSigment.Rows.Count < 1)
                            {
                                //从历史数据库中查找包装资料WIP_CONSIGNMENT_HIS
                                sqlCommand = string.Format(@"select top 1   t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                        t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                        t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                        t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                        t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER 
                                                    from WIP_CONSIGNMENT_HIS t inner join POR_LOT_HIS t1 on t.PALLET_NO=t1.PALLET_NO
                                                    where t1.LOT_NUMBER='{0}' and t.ISFLAG=1
                                                    and t1.DELETED_TERM_FLAG=1 and t.ISFLAG=1",lotno);

                                dtConSigment = dbHis.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];                              
                            }
                            dtConSigment.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
                            dsReturn.Merge(dtConSigment, true, MissingSchemaAction.Add);

                            #endregion

                            if (!dtPorLot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO))
                                dtPorLot.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO);
                            if (!dtPorLot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT))
                                dtPorLot.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT);
                            if (!dtPorLot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP))
                                dtPorLot.Columns.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP);
                            //在批次表中，添加SAP料号，包装班别，包装类别
                            if (dtConSigment.Rows.Count > 0)
                            {
                                DataRow drConsigment = dtConSigment.Rows[0];
                                foreach (DataRow drPorLot in dtPorLot.Rows)
                                {
                                    drPorLot[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO];
                                    drPorLot[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT];
                                    drPorLot[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP];
                                }
                            }
                            #endregion
                            dtPorLot.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                            dsReturn.Merge(dtPorLot, true, MissingSchemaAction.Add);
                        }
                    }
                    #endregion
                }
    

                #region 工单号
                if (flag.Equals("w"))
                {
                    //工单号
                    sqlCommand = string.Format(@"select T.*,T1.PRODUCT_CODE
                                                from POR_WORK_ORDER T 
                                                LEFT JOIN POR_PRODUCT T1 
                                                ON T.PRO_ID=T1.PRODUCT_KEY
                                                WHERE T.ORDER_NUMBER='{0}'", hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString());
                    dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtCommon.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtCommon, true, MissingSchemaAction.Add);
                }
                #endregion

                #region 产品ID号
                if (flag.Equals("p"))
                {

//                    sqlCommand = string.Format(@"select T.PRODUCT_CODE from POR_PRODUCT T
//                                                 where upper(t.PRODUCT_CODE)=upper('{0}')", hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString());
//                    dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
//                    dtCommon.TableName = POR_PRODUCT.DATABASE_TABLE_NAME;
//                    dsReturn.Merge(dtCommon, true, MissingSchemaAction.Add);
                    //根据组件序列号+pro_id号，查看原工单是否包含该Pro_ID号。

                    sqlCommand = string.Format(@"SELECT * FROM (select distinct t0.ORDER_NUMBER,t1.ATTRIBUTE_VALUE  PRODUCT_CODE
                                                from POR_WORK_ORDER t0 
                                                left join POR_PRODUCT t on t0.PRO_ID=t.PRODUCT_KEY
                                                left join POR_WORK_ORDER_ATTR t1 on t0.WORK_ORDER_KEY=t1.WORK_ORDER_KEY
                                                left join BASE_ATTRIBUTE t2 on t1.ATTRIBUTE_KEY=t2.ATTRIBUTE_KEY
                                                left join POR_WORK_ORDER t3 on t1.WORK_ORDER_KEY=t3.WORK_ORDER_KEY
                                                where t2.ATTRIBUTE_NAME like 'Pro_ID%'
                                                --and t.PRODUCT_CODE='{0}' 
                                                and t1.ATTRIBUTE_VALUE='{0}'
                                                and t0.ORDER_NUMBER='{1}'
                                                union all
                                                select a.ORDER_NUMBER,b.PRODUCT_CODE from POR_WORK_ORDER a inner join POR_PRODUCT b on a.PRO_ID=b.PRODUCT_KEY
                                                and a.ORDER_NUMBER='{1}' and b.PRODUCT_CODE='{0}') T WHERE ISNULL(T.ORDER_NUMBER,'0')<>'0'",
                                                hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString(),
                                                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString());

                    DataTable dtContainWo = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtContainWo.TableName = POR_PRODUCT.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtContainWo, true, MissingSchemaAction.Add);

//                    sqlCommand = string.Format(@"select t1.WORK_ORDER_KEY,t1.ORDER_NUMBER
//                                                                    from POR_PRODUCT t inner join POR_WORK_ORDER t1 on t.PRODUCT_CODE=t1.PRO_ID
//                                                                    where t.PRODUCT_CODE='{0}'", hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString());
//                    DataTable dtWo = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
//                    dtWo.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
//                    dsReturn.Merge(dtWo, true, MissingSchemaAction.Add);
                }
                #endregion

                #region 转工单前判断作业
                if (flag.Equals("save"))
                {
                    sqlCommand = string.Format(@"select t.*,t1.PRODUCT_CODE  from POR_WORK_ORDER t left join POR_PRODUCT t1 on t.PRO_ID=t1.PRODUCT_KEY where 1=1 ");
                    if (hstable.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER))
                        sqlCommand += string.Format(" and t.ORDER_NUMBER='{0}' ", hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString());
                    if (hstable.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID))
                        sqlCommand += string.Format(" and t1.PRODUCT_CODE='{0}'", hstable[POR_WORK_ORDER_FIELDS.FIELD_PRO_ID].ToString());
                    DataTable dtWo = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtWo.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtWo, true, MissingSchemaAction.Add);

                    sqlCommand = string.Format(@"select t.*
                                                from POR_WORK_ORDER_ATTR t inner join POR_WORK_ORDER t1 on t.WORK_ORDER_KEY=t1.WORK_ORDER_KEY
                                                left join BASE_ATTRIBUTE t2 on t.ATTRIBUTE_KEY=t2.ATTRIBUTE_KEY
                                                where 1=1 and t2.ATTRIBUTE_NAME like 'Pro_ID%' ");

                    sqlCommand += string.Format(" and t1.ORDER_NUMBER='{0}' ", hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString());
                    if (hstable.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID))
                        sqlCommand += string.Format(" and t.ATTRIBUTE_VALUE='{0}'", hstable[POR_WORK_ORDER_FIELDS.FIELD_PRO_ID].ToString());
                    DataTable dtWoAttr = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtWoAttr.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtWoAttr, true, MissingSchemaAction.Add);
                }
                #endregion

                #region 判断托号是否存在
                if (flag.Equals("Pallet"))
                {
                    sqlCommand = string.Format(@"select t.CS_DATA_GROUP, t.SHIFT,t.SAP_NO, t1.* 
                                                from WIP_CONSIGNMENT t  left join POR_LOT t1 on t.PALLET_NO=t1.PALLET_NO 
                                                where t.ISFLAG=1", 
                                                 hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString());
                    if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                        sqlCommand += string.Format(" and t.PALLET_NO='{0}'", hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString());

                    DataTable dtConsigment = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtConsigment.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtConsigment, true, MissingSchemaAction.Add);
                }
                #endregion

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, errorMsg);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetExchangeByFilter Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 按托号或者批次号从BCP中转数据到MES中
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbTran"></param>
        /// <param name="palletNo">托号</param>
        /// <param name="lotNo">批次号</param>
        /// <returns></returns>
        private string ExportDataToAmesFromBcp(Database db, DbTransaction dbTran, 
            string palletNo, string lotNo,string roomkey,string creator,string computer)
        {
            string sqlCommand = string.Empty;
            string errorMsg = string.Empty;
            bool isContinue = true;
            DataTable dtBcpLot = new DataTable();
            DataTable dtPor_lot_insert = CommonUtils.CreateDataTable(new POR_LOT_FIELDS());
            DataTable dtIvTest_insert = CommonUtils.CreateDataTable(new WIP_IV_TEST_FIELDS());
            DataTable dtCustCheck_insert = CommonUtils.CreateDataTable(new WIP_CUSTCHECK_FIELDS());
            DataTable dtPallet_insert = CommonUtils.CreateDataTable(new WIP_CONSIGNMENT_FIELDS());
            DateTime dtime = new LotEngine().GetSysdate();
            Database dbBcp = DatabaseFactory.CreateDatabase("SQLServerBcp");
            //根据托号获得BCP数据
            #region
            if (!string.IsNullOrEmpty(palletNo))
            {
                sqlCommand = string.Format(@"select distinct t.* ,t3.coef_pmax ,t3.vc_workorder work_order_no,t3.vc_celleff,t4.ames_code pro_level,t1.vc_wo pro_id
                                            from bcp_consignment t inner join bcp_lotinwo t1 on t.serialno=t1.serialno
                                            inner join cust_codecheck t2 on t.serialno=t2.vc_customcode
                                            inner join testdata t3 on t.serialno=t3.serialno
                                            left join bcp_values t4 on t.grade=t4.vc_code
                                            where t1.vc_default='1' 
                                            and t2.vc_default='1' 
                                            and t2.vc_datagroup='1'
                                            and t3.vc_default='1'
                                            and t3.vc_workorder is not null
                                            and t.vc_stock='{0}'", palletNo);
                dtBcpLot = dbBcp.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                if (dtBcpLot.Rows.Count < 1)
                {
                    errorMsg = string.Format(@"BCP数据中未找到有效地托号【{0}】，请确认托号！", palletNo);
                    return errorMsg;
                }
            }
            #endregion

            //根据批次获得BCP数据
            #region
            if (!string.IsNullOrEmpty(lotNo))
            {
                sqlCommand = string.Format(@"select distinct t.* ,t3.coef_pmax ,t3.vc_workorder work_order_no,t3.vc_celleff,t4.ames_code pro_level,t1.vc_wo pro_id
                                            from bcp_consignment t inner join bcp_lotinwo t1 on t.serialno=t1.serialno
                                            inner join cust_codecheck t2 on t.serialno=t2.vc_customcode
                                            inner join testdata t3 on t.serialno=t3.serialno
                                            left join bcp_values t4 on t.grade=t4.vc_code
                                            where t1.vc_default='1' 
                                            and t2.vc_default='1' 
                                            and t2.vc_datagroup='1'
                                            and t3.vc_default='1'
                                            and t3.vc_workorder is not null
                                            and t.serialno='{0}'", lotNo);
                dtBcpLot = dbBcp.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                if (dtBcpLot.Rows.Count < 1)
                {
                    errorMsg = string.Format(@"BCP数据中未找到有效批次【{0}】数据，请确认批次号！", lotNo);
                    return errorMsg;
                }
            }
            #endregion

            #region 获得投批工序
            string routekey = string.Empty, stepkey = string.Empty, enterisekey = string.Empty;
            sqlCommand = string.Format(@"select t.ROUTE_STEP_KEY,t1.ROUTE_ROUTE_VER_KEY,t2.ROUTE_ENTERPRISE_VER_KEY 
                                        from POR_ROUTE_STEP t inner join POR_ROUTE_ROUTE_VER t1 on t.ROUTE_ROUTE_VER_KEY=t1.ROUTE_ROUTE_VER_KEY
                                        left join POR_ROUTE_EP_VER_R_VER t2 on t1.ROUTE_ROUTE_VER_KEY=t2.ROUTE_ROUTE_VER_KEY
                                        where t1.ROUTE_NAME='SH-ZTSM-MAIN' and t1.ROUTE_STATUS=1
                                        and t.ROUTE_STEP_NAME=N'单串焊'");
            DataTable dtRoute = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
            stepkey = Convert.ToString(dtRoute.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
            routekey = Convert.ToString(dtRoute.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
            enterisekey = Convert.ToString(dtRoute.Rows[0]["ROUTE_ENTERPRISE_VER_KEY"]);
            #endregion

            //新增批次信息
            #region
            foreach (DataRow drPallet in dtBcpLot.Rows)
            {
                sqlCommand = string.Format(@"select t.LOT_NUMBER from POR_LOT t where t.LOT_NUMBER='{0}'", Convert.ToString(drPallet["serialno"]));
                DataTable dtisExistLot = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dtisExistLot.Rows.Count > 0)
                {
                    errorMsg = string.Format(@"MES系统中已存在批次号【{0}】，BCP系统批次数据不能添加进来，请与系统管理员联系！", Convert.ToString(drPallet["serialno"]));
                    isContinue = false;
                    break;
                }
               
                //新增批次数据  
             
                DataRow drPor_lot = dtPor_lot_insert.NewRow();
                drPor_lot[POR_LOT_FIELDS.FIELD_STATUS] = 1;
                drPor_lot[POR_LOT_FIELDS.FIELD_QUANTITY] = 60;
                drPor_lot[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL] = 60;
                drPor_lot[POR_LOT_FIELDS.FIELD_LOT_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                drPor_lot[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = Convert.ToString(drPallet["serialno"]);
                drPor_lot[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY] = Convert.ToString(drPallet["work_order_no"]);
                drPor_lot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO] = Convert.ToString(drPallet["work_order_no"]);
                drPor_lot[POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_PALLET_NO] = drPallet["vc_stock"];
                drPor_lot[POR_LOT_FIELDS.FIELD_PALLET_TIME] = drPallet["dt_shpttime"];
                drPor_lot[POR_LOT_FIELDS.FIELD_OPR_COMPUTER] = computer; //客户端名称不能为空
                drPor_lot[POR_LOT_FIELDS.FIELD_CREATE_TIME] = dtime;
                drPor_lot[POR_LOT_FIELDS.FIELD_CREATE_TYPE] = "N";
                drPor_lot[POR_LOT_FIELDS.FIELD_LOT_TYPE] = "N";
                drPor_lot[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY] = routekey;
                drPor_lot[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY] = stepkey;
                drPor_lot[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY] = enterisekey;
                drPor_lot[POR_LOT_FIELDS.FIELD_STATE_FLAG] = 0;
                drPor_lot[POR_LOT_FIELDS.FIELD_IS_MAIN_LOT] = 1;
                drPor_lot[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG] = 1;
                drPor_lot[POR_LOT_FIELDS.FIELD_CREATOR] = creator;    //创批人员
                drPor_lot[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME] = "BCP";
                drPor_lot[POR_LOT_FIELDS.FIELD_DESCRIPTIONS] = "该批次是从BCP返工转工单，新增到MES数据中";
                drPor_lot[POR_LOT_FIELDS.FIELD_EFFICIENCY] = Convert.ToString(drPallet["vc_celleff"]);
                drPor_lot[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY] = roomkey;
                drPor_lot[POR_LOT_FIELDS.FIELD_HOLD_FLAG] = 0;
                drPor_lot[POR_LOT_FIELDS.FIELD_IS_PRINT] = 0;
                drPor_lot[POR_LOT_FIELDS.FIELD_IS_REWORKED] = 0;
                drPor_lot[POR_LOT_FIELDS.FIELD_IS_SPLITED] = 0;
                drPor_lot[POR_LOT_FIELDS.FIELD_LINE_NAME] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_LOT_SEQ] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_LOT_SIDECODE] = "";  //侧板编码
                drPor_lot[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE] = "";//客户编码
                drPor_lot[POR_LOT_FIELDS.FIELD_MATERIAL_CODE] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_MATERIAL_LOT] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_OPERATOR] = creator;    //操作人
                drPor_lot[POR_LOT_FIELDS.FIELD_PART_NUMBER] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_PART_VER_KEY] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_PRIORITY] = 5;
                drPor_lot[POR_LOT_FIELDS.FIELD_PRO_ID] = drPallet["pro_id"];
                drPor_lot[POR_LOT_FIELDS.FIELD_PART_NUMBER] = "";
                drPor_lot[POR_LOT_FIELDS.FIELD_PRO_LEVEL] = Convert.ToString(drPallet["pro_level"]);
                dtPor_lot_insert.Rows.Add(drPor_lot);
            }
            #endregion

            if (!isContinue)
                return errorMsg;
               
            //新增IV测试数据
            #region
            foreach (DataRow drIvTest in dtBcpLot.Rows)
            {
                string slot = Convert.ToString(drIvTest["serialno"]);
                sqlCommand = string.Format(@"select * from testdata t 
                                                 where t.serialno='{0}' and t.vc_default='1'", slot);

                DataTable dtLot = dbBcp.ExecuteDataSet( CommandType.Text, sqlCommand).Tables[0];
                if (dtLot.Rows.Count < 1)
                {
                    errorMsg = string.Format(@"未找到BCP中批次【{0}】测试数据，请确认！", slot);
                    isContinue = false;
                    break;
                }

                DataRow drBcpIvTest = dtLot.Rows[0];
                DataRow drIvTest_insert = dtIvTest_insert.NewRow();

                //获得衰减后的功率，分档及子分档
                int i_ide = 0, i_pkid = 0;
//                sqlCommand = string.Format(@"  select  t.PS_CODE,t.PS_RULE,t.PS_SEQ,t.P_MIN,t.P_MAX,t.PMAXSTAB ,t1.PS_DTL_SUBCODE,t1.P_DTL_MIN, t1.P_DTL_MAX
//                                             from BASE_POWERSET t left join BASE_POWERSET_DETAIL t1 on t.POWERSET_KEY=t1.POWERSET_KEY and t1.ISFLAG=1
//                                             where t.ISFLAG=1 and t.PS_CODE='{0}'
//                                             and CONVERT(decimal,'{1}') between t.P_MIN and t.P_MAX"
//                    , Convert.ToString(drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE])
//                    , Convert.ToString(drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]));
//                DataTable dtPowerSet = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
//                if (dtPowerSet.Rows.Count < 1)
//                {
//                    errorMsg = string.Format(@"未找到分档规则【{0}】中的功率【{1}】档位，请与工艺人员联系！"
//                        , Convert.ToString(drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE])
//                        , Convert.ToString(drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]));
//                    isContinue = false;
//                    break;
//                }
//                else
//                {
//                    i_ide = Convert.ToInt16(dtPowerSet.Rows[0][BASE_POWERSET.FIELDS_PS_SEQ]);
//                    decimal coef_pmax = Convert.ToDecimal(drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX]);
//                    DataRow[] drPowerDtl = dtPowerSet.Select(@"PS_DTL_SUBCODE is not null");
//                    foreach (DataRow dr in drPowerDtl)
//                    {                      
//                        decimal dtl_min = Convert.ToDecimal(dr[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MIN]);
//                        decimal dtl_max = Convert.ToDecimal(dr[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MAX]);
//                        if (coef_pmax <= dtl_max && coef_pmax >= dtl_min)
//                        {
//                            i_pkid = Convert.ToInt16(dr[BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE]);
//                            break;
//                        }
//                    }
//                }

                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_AMBIENTTEMP] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_AMBIENTTEMP];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_C_MAINPED] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_C_MAINPED];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_C_PSTATE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_C_PSTATE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_C_USERID] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_C_USERID];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_CALIBRATION_NO] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_CALIBRATION_NO];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_COEF_FF] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_FF];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_COEF_IMAX] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_IMAX];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_COEF_ISC] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_ISC];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_PMAX];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_COEF_VMAX] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_VMAX];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_COEF_VOC] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_COEF_VOC];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_DEC_CTM] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_DEC_CTM];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_DEC_PMCHANGE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_DEC_PMCHANGE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_DT_CREATE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_DT_CREATE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_DT_PRINTDT] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_DT_PRINTDT];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_EFF] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_EFF];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_FF] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_FF];

                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_TYPE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_I_IDE] = i_ide;
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_I_PKID] = i_pkid;
                //drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_I_IDE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_I_IDE];
                //drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_I_PKID] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_I_PKID];

                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_I_PSTATE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_I_PSTATE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_INTENSITY] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_INTENSITY];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_IPM] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_IPM];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_ISC] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_ISC];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_IV_TEST_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_L_ID] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_L_ID];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM] = drBcpIvTest["serialno"];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_P_NUM] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_P_NUM];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_PM] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_PM];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_PRINTED_NP] = "";
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_PRINTEDLABLE] = drBcpIvTest["vc_printlabelid"];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_RS] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_RS];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_RSH] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_RSH];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_SENSORTEMP] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_SENSORTEMP];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_T_DATE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_T_DATE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_TTIME] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_TTIME];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_CELLEFF] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_CELLEFF];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_COGCODE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_COGCODE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_COLOR] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_COLOR];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_CUSTCODE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_CUSTCODE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_DATAGROUP] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_DATAGROUP];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_INICUSTCODE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_INICUSTCODE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_MODNAME] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_MODNAME];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_PRINTLABELID] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_PRINTLABELID];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_PSIGN] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_PSIGN];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_PSTATE] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_PSTATE];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VC_WORKORDER] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VC_WORKORDER];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VOC] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VOC];
                drIvTest_insert[WIP_IV_TEST_FIELDS.FIELDS_VPM] = drBcpIvTest[WIP_IV_TEST_FIELDS.FIELDS_VPM];
                dtIvTest_insert.Rows.Add(drIvTest_insert);
            }
            #endregion
            if (!isContinue)
                return errorMsg;

            //新增终检数据                           
            #region
            foreach (DataRow drPallet in dtBcpLot.Rows)
            {
                string slot = Convert.ToString(drPallet["serialno"]);
                sqlCommand = string.Format(@"select t.* ,t1.ames_code pro_level,t3.coef_pmax
                                        from cust_codecheck t inner join bcp_values t1 on t.vc_prolevel=t1.vc_code
                                        inner join testdata t3 on t.vc_customcode=t3.serialno
                                        where  t.vc_customcode='{0}' and
                                         t.vc_datagroup='1' and t3.vc_default='1'  and t.vc_default='1'  ", slot);

                DataTable dtBcpCustCheck = dbBcp.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dtBcpCustCheck.Rows.Count < 1)
                {
                    errorMsg = string.Format(@"批次【{0}】在BCP中的终检数据不存在！", slot);
                    isContinue = false;
                    break;
                }

                DataRow drBcpCustCheck = dtBcpCustCheck.Rows[0];

                DataRow drCustCheck = dtCustCheck_insert.NewRow();
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_DATA_GROUP] = drBcpCustCheck["vc_datagroup"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE] = drBcpCustCheck["vc_customcode"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1] = drBcpCustCheck["vc_customcode"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE2] = drBcpCustCheck["vc_customcode"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE3] = drBcpCustCheck["vc_customcode"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE4] = drBcpCustCheck["vc_factorycode4"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE5] = drBcpCustCheck["vc_factorycode5"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE6] = drBcpCustCheck["vc_factorycode6"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_POWER] =drBcpCustCheck["coef_pmax"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_TIME] = drBcpCustCheck["dt_checktime"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME] = dtime;
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CREATER] = creator;
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CUSTCHECK_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_CUSTOMCODE] = drBcpCustCheck["vc_customcode"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM] = drBcpCustCheck["devicenum"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_EDIT_TIME] = dtime;
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_EDITOR] = creator;
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_ISFLAG] = 1;
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_l_ID] = drBcpCustCheck["l_id"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR] = drBcpCustCheck["vc_color"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_TYPE] = drBcpCustCheck["vc_factoryline"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_NAMEPLATENO] = drBcpCustCheck["vc_factorycode1"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_OPERATERS] = drBcpCustCheck["devicenum"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_ID] = drBcpCustCheck["vc_fcode"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_LEVEL] = drBcpCustCheck["pro_level"];
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_REASON_CODE_CATEGORY_NAME] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_REASON_CODE_KEY] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_REASON_CODE_NAME] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_REMARK] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY] = roomkey;
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_KEY] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME] = "";
                drCustCheck[WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER] = drBcpCustCheck["vc_stockcode"];
                dtCustCheck_insert.Rows.Add(drCustCheck);
            }
            #endregion
            if (!isContinue)
                return errorMsg;

            //新增包装数据          
            #region

            DataRow drBcpPallet = dtBcpLot.Rows[0];
            string packageid = Convert.ToString(drBcpPallet["vc_stock"]);
            if (!string.IsNullOrEmpty(packageid))
            {
                string sql = string.Format(@"select t.PALLET_NO from WIP_CONSIGNMENT t where t.ISFLAG=1 and t.PALLET_NO='{0}'", packageid);
                DataTable dtSql = db.ExecuteDataSet(dbTran, CommandType.Text, sql).Tables[0];
                if (dtSql.Rows.Count < 1)
                {
                    DataRow drPallet_insert = dtPallet_insert.NewRow();
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_AVG_POWER_RANGE] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME] = drBcpPallet["d_shptdate"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECKER] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE] = 0;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME] = dtime;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATER] = creator;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 3;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_CUSTOMER_NO] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME] = dtime;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR] = creator;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = drBcpPallet["pro_level"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_ISFLAG] = 1;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_LAST_PALLET] = 2;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_KEY] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_NAME] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_COLOR] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW] = drBcpPallet["vc_stock"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO] = drBcpPallet["vc_stock"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_TYPE] = 0;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = drBcpPallet["pro_level"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID] = drBcpPallet["pro_id"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_CODE] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_DTL_SUBCODE] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_PS_SEQ] = "";
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY] = roomkey;
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO] = drBcpPallet["sap_pn"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT] = drBcpPallet["shift"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO] = drBcpPallet["vc_stock"];
                    drPallet_insert[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER] = "";
                    dtPallet_insert.Rows.Add(drPallet_insert);
                }
            }
           
           
            #endregion

            POR_LOT_FIELDS por_lot_fields = new POR_LOT_FIELDS();
            WIP_IV_TEST_FIELDS wip_ivtest_fields = new WIP_IV_TEST_FIELDS();
            WIP_CUSTCHECK_FIELDS wip_custcheck_fields = new WIP_CUSTCHECK_FIELDS();
            WIP_CONSIGNMENT_FIELDS wip_consigment_fields = new WIP_CONSIGNMENT_FIELDS();

            #region  插入数据
            //插入批次数据
            foreach (DataRow dr in dtPor_lot_insert.Rows)
            {
                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);                
                sqlCommand = DatabaseTable.BuildInsertSqlStatement(por_lot_fields, hashTable, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            //插入测试数据
            foreach (DataRow dr in dtIvTest_insert.Rows)
            {
                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                sqlCommand = DatabaseTable.BuildInsertSqlStatement(wip_ivtest_fields, hashTable, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            //插入终检数据
            foreach (DataRow dr in dtCustCheck_insert.Rows)
            {
                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                sqlCommand = DatabaseTable.BuildInsertSqlStatement(wip_custcheck_fields, hashTable, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            //插入包装数据
            foreach (DataRow dr in dtPallet_insert.Rows)
            {
                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                sqlCommand = DatabaseTable.BuildInsertSqlStatement(wip_consigment_fields, hashTable, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            }
            #endregion

            return errorMsg;

        }

        public DataSet SaveExchangeWo(DataSet dsSave)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty, activity = string.Empty, errMsg = string.Empty;
            string route_enterprise_ver_key = string.Empty, 
                   route_route_ver_key = string.Empty, 
                   route_operation_key = string.Empty, 
                   route_step_key = string.Empty,
                   route_step_name = string.Empty;
            string creator = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_CREATOR]);
            string workorder = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            string workorderkey = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string proid = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_PRO_ID]);
            string computer = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            string shiftkey = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            string shiftname = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            string dateType = Convert.ToString(dsSave.ExtendedProperties[CHECKTYPE.DATA_TYPE]);
            string factory_key = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            string factory_name = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME]);

            DataTable dtwip_lot_insert = null, dtpor_lot_update = null, dtwip_transaction_insert = null;
            //-----------------------------------------------------------------------------------------------

            DataTable dtRoute = dsSave.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME];
            DataRow drRoute = dtRoute.Rows[0];
            route_enterprise_ver_key = GetAboutRouteKey(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, drRoute[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME].ToString());
            route_route_ver_key = GetAboutRouteKey(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME].ToString());
            route_operation_key = GetAboutRouteKey(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, drRoute[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString());

            sqlCommand = string.Format(@"SELECT t.ROUTE_STEP_KEY,t.ROUTE_STEP_NAME 
                                        FROM POR_ROUTE_STEP T 
                                        WHERE T.ROUTE_ROUTE_VER_KEY='{0}' 
                                        AND t.ROUTE_OPERATION_VER_KEY='{1}'", 
                                        route_route_ver_key,
                                        route_operation_key);
            DataTable dtStep = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            route_step_key = Convert.ToString(dtStep.Rows[0]["ROUTE_STEP_KEY"]);
            route_step_name = Convert.ToString(dtStep.Rows[0]["ROUTE_STEP_NAME"]);

            if (dsSave.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME_FORUPDATE))
                dtpor_lot_update = dsSave.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME_FORUPDATE];

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();

                POR_LOT_FIELDS por_lot_fields = new POR_LOT_FIELDS();
                WIP_LOT_FIELDS wip_lot_fields = new WIP_LOT_FIELDS();
                WIP_TRANSACTION_FIELDS wip_transaction_fields = new WIP_TRANSACTION_FIELDS();

                try
                {
                    int i_experiment = 0;
                    //返工工单，需要把数据重新整理
                    if (dateType.Equals("Repair"))
                    {
                        errMsg = SaveMutiExchangeWo(db, dbTran, dsSave, factory_key, factory_name, out i_experiment);
                    }

                    if (string.IsNullOrEmpty(errMsg))
                    {
                        #region 转工单作业
                        bool blContinue = true;
                        foreach (DataRow drpor_lot_update in dtpor_lot_update.Rows)
                        {
                            if (dtpor_lot_update.Columns.Contains(POR_LOT_FIELDS.FIELD_PALLET_NO) && blContinue)
                            {
                                string palletno = Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                                sqlCommand = string.Format(@"UPDATE WIP_CONSIGNMENT 
                                                            SET PALLET_TYPE=0 ,EDITOR='{0}',EDIT_TIME=GETDATE(),SHIFT='{1}' ,ISFLAG=0
                                                            WHERE PALLET_NO='{2}' AND ISFLAG=1 ",
                                                            creator, shiftname, palletno);
                                int iRow = db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                if (iRow > 0)
                                    blContinue = false;
                            }
                            string transactionkey = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                            DateTime dtime = new LotEngine().GetSysdate();
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY] = route_enterprise_ver_key;
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY] = route_route_ver_key;
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY] = route_step_key;
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_STATE_FLAG] = 0;
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_EDITOR] = creator;
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_EDIT_TIME] = dtime.ToString();
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_PALLET_NO] = string.Empty;
                            string ivtestproid = string.Empty;
                            if (dateType.Equals("Repair"))
                            {
                                drpor_lot_update[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY] = drpor_lot_update[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY + "2"];
                                drpor_lot_update[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO] = drpor_lot_update[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "2"];
                                drpor_lot_update[POR_LOT_FIELDS.FIELD_PRO_ID] = drpor_lot_update[POR_LOT_FIELDS.FIELD_PRO_ID + "2"];
                                ivtestproid = Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_PRO_ID + "2"]);

                            }
                            else
                            {
                                drpor_lot_update[POR_LOT_FIELDS.FIELD_PRO_ID] = proid;
                                drpor_lot_update[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY] = workorderkey;
                                drpor_lot_update[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO] = workorder;
                                ivtestproid = proid;
                            }
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG] = 0;
                            drpor_lot_update[POR_LOT_FIELDS.FIELD_SHIFT_NAME] = shiftname;

                            #region 重新计算衰减后产品ID
                            if (dateType.Equals("Repair"))
                            {
                                sqlCommand = string.Format(@"select t.COEF_PMAX from WIP_IV_TEST t where t.LOT_NUM='{0}' and t.VC_DEFAULT='1'",
                                    Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                                DataTable dtPmax = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                                if (dtPmax.Rows.Count > 0)
                                {
                                    //获得衰减后的功率，分档及子分档
                                    int i_ide = 0, i_pkid = 0;
                                    string vc_type = string.Empty;
                                    string pmax = Convert.ToString(dtPmax.Rows[0][0]);

                                    if (!pmax.Equals(string.Empty))
                                    {
                                        sqlCommand = string.Format(@"select  t.PS_CODE,t.PS_RULE,t.PS_SEQ,t.P_MIN,t.P_MAX,t.PMAXSTAB ,
                                                            t1.PS_DTL_SUBCODE,t1.P_DTL_MIN, t1.P_DTL_MAX
                                                            from BASE_POWERSET t left join BASE_POWERSET_DETAIL t1 on t.POWERSET_KEY=t1.POWERSET_KEY  and t1.ISFLAG=1
                                                            left join BASE_TESTRULE t2 on t.PS_CODE=t2.PS_CODE 
                                                            left join POR_PRODUCT t3 on t2.TESTRULE_CODE=t3.PRO_TEST_RULE
                                                            where t.ISFLAG=1                                                           
                                                            and t2.ISFLAG=1
                                                            and t3.ISFLAG=1
                                                            and t3.PRODUCT_CODE='{0}'
                                                            and CONVERT(numeric(18,2),'{1}') between t.P_MIN and t.P_MAX"
                                                                                                   , ivtestproid, pmax);

                                        DataTable dtPowerSet = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                                        if (dtPowerSet.Rows.Count > 0)
                                        {
                                            vc_type = Convert.ToString(dtPowerSet.Rows[0][BASE_POWERSET.FIELDS_PS_CODE]);
                                            i_ide = Convert.ToInt16(dtPowerSet.Rows[0][BASE_POWERSET.FIELDS_PS_SEQ]);
                                            decimal coef_pmax = Convert.ToDecimal(pmax);
                                            DataRow[] drPowerDtl = dtPowerSet.Select(@"PS_DTL_SUBCODE is not null");
                                            foreach (DataRow dr in drPowerDtl)
                                            {
                                                decimal dtl_min = Convert.ToDecimal(dr[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MIN]);
                                                decimal dtl_max = Convert.ToDecimal(dr[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MAX]);
                                                if (coef_pmax <= dtl_max && coef_pmax >= dtl_min)
                                                {
                                                    i_pkid = Convert.ToInt16(dr[BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE]);
                                                    break;
                                                }
                                            }

                                            sqlCommand = string.Format(@"UPDATE t 
                                                                         SET t.VC_TYPE='{0}',t.I_IDE='{1}',t.I_PKID='{2}'
                                                                         FROM WIP_IV_TEST  t  
                                                                         WHERE t.LOT_NUM='{3}' AND t.VC_DEFAULT='1'",
                                       vc_type, i_ide, i_pkid, Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));

                                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                        }
                                        else
                                        {
                                            errMsg = string.Format("系统未发现批次【{0}】有效功率数据，请与系统管理员联系!", Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                                        }
                                    }
                                    else
                                    {
                                        errMsg = string.Format("系统未发现批次【{0}】分档数据，请与系统管理员联系!", Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                                    }
                                }
                                else
                                {
                                    if (i_experiment < 2)
                                    {
                                        errMsg = string.Format("系统未发现批次【{0}】IV测试数据，请与系统管理员联系!", Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                                    }
                                }
                            }
                            #endregion


                            //-----------------------------------------------------------------------------------------------                    
                            activity = Convert.ToString(dsSave.ExtendedProperties[ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_WO]);

                            sqlCommand = string.Format(@"SELECT t.*,t1.ROUTE_STEP_KEY,t1.ROUTE_STEP_NAME 
                                                        FROM POR_LOT t 
                                                        INNER JOIN POR_ROUTE_STEP t1 ON t.CUR_STEP_VER_KEY=t1.ROUTE_STEP_KEY
                                                        WHERE t.LOT_NUMBER='{0}'", 
                                                        Convert.ToString(drpor_lot_update[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                            dtwip_lot_insert = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];

                            dtwip_lot_insert.TableName = WIP_LOT_FIELDS.DATABASE_TABLE_NAME;
                            dtwip_lot_insert.Columns.Add(WIP_LOT_FIELDS.FIELD_TRANSACTION_KEY);
                            dtwip_lot_insert.Rows[0][WIP_LOT_FIELDS.FIELD_TRANSACTION_KEY] = transactionkey;
                            dtwip_lot_insert.Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME] = dtime.ToString();
                            dtwip_lot_insert.Rows[0][POR_LOT_FIELDS.FIELD_EDITOR] = creator;
                            dtwip_lot_insert.AcceptChanges();

                            DataRow drLot = dtwip_lot_insert.Rows[0];
                            dtwip_transaction_insert = CommonUtils.CreateDataTable(new WIP_TRANSACTION_FIELDS());
                            DataRow drTransaction = dtwip_transaction_insert.NewRow();
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionkey;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = activity;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = drLot[POR_LOT_FIELDS.FIELD_LOT_KEY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = drLot[POR_LOT_FIELDS.FIELD_EDITOR];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = dtime.ToString();
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = drLot[POR_LOT_FIELDS.FIELD_QUANTITY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = drLot[POR_LOT_FIELDS.FIELD_QUANTITY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = drLot[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = drLot[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = drLot[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = "";
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = drLot[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = "";
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = drLot[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = drLot[POR_LOT_FIELDS.FIELD_LINE_NAME];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY] = shiftkey;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = shiftname;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_UNDO_FLAG] = 0;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = 0;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = 0;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP] = dtime.ToString();
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR] = creator;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = drLot[POR_LOT_FIELDS.FIELD_LINE_NAME];
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY] = route_step_key;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER] = computer;
                            drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = "";
                            dtwip_transaction_insert.Rows.Add(drTransaction);

                            #region insert
                            if (dtwip_transaction_insert != null && dtwip_transaction_insert.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtwip_transaction_insert.Rows)
                                {
                                    Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                    sqlCommand = DatabaseTable.BuildInsertSqlStatement(wip_transaction_fields, hashTable, null);
                                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                }
                            }

                            if (dtwip_lot_insert != null && dtwip_lot_insert.Rows.Count > 0)
                            {

                                foreach (DataRow dr in dtwip_lot_insert.Rows)
                                {
                                    // t1.ROUTE_STEP_KEY,t1.ROUTE_STEP_NAME 

                                    Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                    if (hashTable.ContainsKey("ROUTE_STEP_KEY"))
                                        hashTable.Remove("ROUTE_STEP_KEY");
                                    if (hashTable.ContainsKey("ROUTE_STEP_NAME"))
                                        hashTable.Remove("ROUTE_STEP_NAME");
                                    if (hashTable.ContainsKey("ETL_FLAG"))
                                        hashTable.Remove("ETL_FLAG");
                                    sqlCommand = DatabaseTable.BuildInsertSqlStatement(wip_lot_fields, hashTable, null);
                                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }

                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        dbTran.Rollback();
                    }
                    else
                    {
                        #region update
                        if (dtpor_lot_update != null && dtpor_lot_update.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtpor_lot_update.Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                if (hashTable.ContainsKey("CS_DATA_GROUP"))
                                    hashTable.Remove("CS_DATA_GROUP");

                                WhereConditions wc = new WhereConditions(POR_LOT_FIELDS.FIELD_LOT_NUMBER, hashTable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString());
                                hashTable.Remove(POR_LOT_FIELDS.FIELD_LOT_NUMBER);

                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY + "2"))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY + "2");
                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "2"))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "2");
                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID + "2"))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_PRO_ID + "2");
                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY);
                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME);
                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_START_WAIT_TIME))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_START_WAIT_TIME);
                                if (hashTable.ContainsKey(POR_LOT_FIELDS.FIELD_START_PROCESS_TIME))
                                    hashTable.Remove(POR_LOT_FIELDS.FIELD_START_PROCESS_TIME);
                                hashTable[POR_LOT_FIELDS.FIELD_HOLD_FLAG] = 0;


                                sqlCommand = DatabaseTable.BuildUpdateSqlStatement(por_lot_fields, hashTable, wc);

                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        #endregion

                        //Commit Transaction
                        dbTran.Commit();
                    }
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, errMsg);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveExchangeWo Error: " + ex);
                    LogService.LogError(ex);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
                return dsReturn;
            }
        }
        /// <summary>
        /// 返工工单，需要整理正式数据库中的数据
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbTran"></param>
        /// <param name="dsSave">dsSave: 需要整理的源数据</param>
        private string SaveMutiExchangeWo(Database db, DbTransaction dbTran, DataSet dsSave, string factory_key, string factory_name, out int i_experiment)
        {
            string sqlCommand = string.Empty, errMsg = string.Empty;
            DataTable dtConsigment = dsSave.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtPorLot = dsSave.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME_FORUPDATE];
            i_experiment = 0;

            foreach (DataRow drConsigment in dtConsigment.Rows)
            {
                //先判断正式数据库-包装表
                sqlCommand = string.Format(@"select t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                            t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                            t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                            t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                            t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER  from WIP_CONSIGNMENT t where t.CONSIGNMENT_KEY='{0}'",
                                            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString());
                DataTable dt01 = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                if (dt01.Rows.Count > 0)
                {
                    sqlCommand = string.Format(@"update WIP_CONSIGNMENT set ISFLAG=1,ROOM_KEY='{1}' where CONSIGNMENT_KEY='{0}'",
                        drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString(), factory_key.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                }
                else
                {
                    //在新增数据从历史数据表
                    sqlCommand = string.Format(@"INSERT INTO WIP_CONSIGNMENT
                                                ([CONSIGNMENT_KEY],[VIRTUAL_PALLET_NO],[PALLET_NO],[WORKNUMBER],[CS_DATA_GROUP],[SAP_NO]
                                                ,[POWER_LEVEL],[GRADE],[SHIFT],[PS_CODE],[PS_DTL_SUBCODE],[LAST_PALLET],[CREATER]
                                                ,[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG],[ROOM_KEY],[CUSTOMER_NO],[LOT_NUMBER_QTY]
                                                ,[TOTLE_POWER],[AVG_POWER],[PRO_ID],[PALLET_NO_NEW],[PALLET_TYPE],[CODE_TYPE],[LINE_NAME]
                                                ,[LINE_KEY],[EQUIPMENT_KEY],[EQUIPMENT_NAME],[AVG_POWER_RANGE],[LOT_COLOR],[PS_SEQ]
                                                ,[CHECKER],[CHECK_TIME],[TO_WH],[TO_WH_TIME],[OUT_WH],[OUT_WH_TIME])
                                                SELECT [CONSIGNMENT_KEY],[VIRTUAL_PALLET_NO],[PALLET_NO],[WORKNUMBER],[CS_DATA_GROUP],[SAP_NO]
                                                ,[POWER_LEVEL],[GRADE],[SHIFT],[PS_CODE],[PS_DTL_SUBCODE],[LAST_PALLET],[CREATER]
                                                ,[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG],'{1}',[CUSTOMER_NO],[LOT_NUMBER_QTY]
                                                ,[TOTLE_POWER],[AVG_POWER],[PRO_ID],[PALLET_NO_NEW],[PALLET_TYPE],[CODE_TYPE],[LINE_NAME]
                                                ,[LINE_KEY],[EQUIPMENT_KEY],[EQUIPMENT_NAME],[AVG_POWER_RANGE],[LOT_COLOR],[PS_SEQ]
                                                ,[CHECKER],[CHECK_TIME],[TO_WH],[TO_WH_TIME],[OUT_WH],[OUT_WH_TIME]
                                                 from WIP_CONSIGNMENT_HIS where CONSIGNMENT_KEY='{0}'",
                                                 drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString(), 
                                                 factory_key.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    sqlCommand = string.Format(@"UPDATE WIP_CONSIGNMENT SET ISFLAG=1 WHERE CONSIGNMENT_KEY='{0}'", drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                }

                sqlCommand = string.Format(@"SELECT t.LAST_PALLET FROM WIP_CONSIGNMENT t 
                                            WHERE t.CONSIGNMENT_KEY='{0}' AND t.ISFLAG=1 ",
                                            drConsigment[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString());
                DataTable dtLastPallet = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                if (dtLastPallet.Rows.Count > 0)
                {
                    i_experiment = Convert.ToInt16(dtLastPallet.Rows[0][0]);
                }
            }

            foreach (DataRow drPorLot in dtPorLot.Rows)
            {
                string lotKey=Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                #region 批次数据
                sqlCommand = string.Format(@"SELECT t.LOT_KEY,t.LOT_NUMBER,t.FACTORYROOM_KEY,t.WORK_ORDER_KEY,
                                                    t.WORK_ORDER_NO,t.PRO_ID,t.PALLET_NO,t.PALLET_TIME 
                                            FROM POR_LOT t 
                                            WHERE t.LOT_KEY='{0}'",
                                            lotKey);
                DataTable dt02 = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                if (dt02.Rows.Count > 0)
                {
                    //判断有效批次是否存在
                    sqlCommand = string.Format(@"SELECT * 
                                                FROM POR_LOT t 
                                                WHERE t.DELETED_TERM_FLAG=0 and t.LOT_KEY='{0}'",lotKey);
                    DataTable dtExistLotno = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                    if (dtExistLotno.Rows.Count > 0)
                    {
                        errMsg = string.Format("批次【{0}】还没有入库，不能返工，请确认!", dtExistLotno.Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString());
                        break;
                    }
                    sqlCommand = string.Format(@"UPDATE t 
                                                SET t.CREATE_OPERTION_NAME='Repair',T.DELETED_TERM_FLAG=0,FACTORYROOM_KEY='{1}',FACTORYROOM_NAME='{2}'
                                                FROM POR_LOT t 
                                                WHERE T.LOT_KEY='{0}'",
                                                lotKey, factory_key.PreventSQLInjection(), factory_name);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                }
                else
                {
                    sqlCommand = string.Format(@"INSERT INTO POR_LOT
                                                (
                                                    [LOT_KEY],[LOT_NUMBER],[WORK_ORDER_KEY] ,[WORK_ORDER_NO],[WORK_ORDER_SEQ],[PART_VER_KEY]
                                                    ,[PART_NUMBER],[PRO_ID],[PRO_LEVEL],[PRIORITY],[QUANTITY_INITIAL],[QUANTITY],[ROUTE_ENTERPRISE_VER_KEY]
                                                    ,[CUR_ROUTE_VER_KEY],[CUR_STEP_VER_KEY],[CUR_PRODUCTION_LINE_KEY],[LINE_NAME],[START_WAIT_TIME]
                                                    ,[START_PROCESS_TIME],[EDC_INS_KEY],[STATE_FLAG],[IS_MAIN_LOT],[SPLIT_FLAG],[LOT_SEQ],[REWORK_FLAG]
                                                    ,[HOLD_FLAG],[SHIPPED_FLAG],[DELETED_TERM_FLAG],[IS_PRINT],[LOT_TYPE],[CREATE_TYPE],[PALLET_NO]
                                                    ,[PALLET_TIME],[STATUS],[OPERATOR],[OPR_LINE],[OPR_COMPUTER],[OPR_LINE_PRE],[CHILD_LINE],[MATERIAL_CODE]
                                                    ,[MATERIAL_LOT],[SUPPLIER_NAME],[SI_LOT],[EFFICIENCY],[FACTORYROOM_KEY],[FACTORYROOM_NAME],[CREATE_OPERTION_NAME]
                                                    ,[CREATOR],[CREATE_TIME],[CREATE_TIMEZONE_KEY],[EDITOR],[EDIT_TIME],[EDIT_TIMEZONE],[SHIFT_NAME]
                                                    ,[DESCRIPTIONS],[LOT_SIDECODE],[LOT_CUSTOMERCODE]
                                                )
                                                SELECT [LOT_KEY],[LOT_NUMBER],[WORK_ORDER_KEY] ,[WORK_ORDER_NO],[WORK_ORDER_SEQ],[PART_VER_KEY]
                                                    ,[PART_NUMBER],[PRO_ID],[PRO_LEVEL],[PRIORITY],[QUANTITY_INITIAL],[QUANTITY],[ROUTE_ENTERPRISE_VER_KEY]
                                                    ,[CUR_ROUTE_VER_KEY],[CUR_STEP_VER_KEY],[CUR_PRODUCTION_LINE_KEY],[LINE_NAME],GETDATE()
                                                    ,NULL,[EDC_INS_KEY],[STATE_FLAG],[IS_MAIN_LOT],[SPLIT_FLAG],[LOT_SEQ],[REWORK_FLAG]
                                                    ,[HOLD_FLAG],[SHIPPED_FLAG],0,[IS_PRINT],[LOT_TYPE],[CREATE_TYPE],[PALLET_NO]
                                                    ,[PALLET_TIME],[STATUS],[OPERATOR],[OPR_LINE],[OPR_COMPUTER],[OPR_LINE_PRE],[CHILD_LINE],[MATERIAL_CODE]
                                                    ,[MATERIAL_LOT],[SUPPLIER_NAME],[SI_LOT],[EFFICIENCY],'{1}','{2}','Repair'
                                                    ,[CREATOR],[CREATE_TIME],[CREATE_TIMEZONE_KEY],[EDITOR],[EDIT_TIME],[EDIT_TIMEZONE],[SHIFT_NAME]
                                                    ,[DESCRIPTIONS],[LOT_SIDECODE],[LOT_CUSTOMERCODE]
                                                 FROM POR_LOT_HIS 
                                                 WHERE LOT_KEY='{0}'",
                                                 lotKey.PreventSQLInjection(),
                                                 factory_key.PreventSQLInjection(),
                                                 factory_name.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                }
                #endregion

                #region IV数据
                sqlCommand = string.Format(@"select t.COEF_PMAX from WIP_IV_TEST t,POR_LOT t1  where t.LOT_NUM=t1.LOT_NUMBER and 
                                             t1.LOT_KEY='{0}' and t.VC_DEFAULT='1'", drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                DataTable dt03 = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                if (dt03.Rows.Count < 1 || Convert.ToString(dt03.Rows[0][0]).Equals(string.Empty))
                {
                    sqlCommand = string.Format(@"select t.COEF_PMAX from WIP_IV_TEST_HIS t,POR_LOT_HIS t1  where t.LOT_NUM=t1.LOT_NUMBER and 
                                             t1.LOT_KEY='{0}' and t.VC_DEFAULT='1'  and t.COEF_PMAX is not null", drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                    DataTable dtHisIvtest = dbHis.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (dtHisIvtest.Rows.Count > 0)
                    {
                        sqlCommand = string.Format(@" INSERT INTO WIP_IV_TEST
                                    ( [IV_TEST_KEY],[DEVICENUM],[LOT_NUM],[PM],[ISC],[IPM],[VOC],[VPM],[T_DATE],[TTIME]
                                    ,[VC_PSIGN],[DT_PRINTDT],[VC_DEFAULT],[AMBIENTTEMP],[SENSORTEMP],[FF],[EFF],[RS],[RSH],[INTENSITY]
                                    ,[VC_INICUSTCODE],[VC_CUSTCODE],[P_NUM],[C_USERID],[L_ID],[DT_CREATE],[C_PSTATE],[VC_DATAGROUP]
                                    ,[COEF_PMAX],[COEF_ISC],[COEF_VOC],[COEF_IMAX],[COEF_VMAX],[COEF_FF],[VC_TYPE],[I_IDE],[VC_MODNAME]
                                    ,[C_MAINPED],[I_PSTATE],[VC_PSTATE],[VC_PRINTLABELID],[VC_COGCODE],[I_ID],[VC_CELLEFF],[DEC_CTM]
                                    ,[I_PKID],[DEC_PMCHANGE],[VC_WORKORDER],[VC_COLOR],[CALIBRATION_NO],[PRINTEDLABLE],[PRINTED_NP]
                                    ,[TIME_STAMP],[Imp_Isc],[ImpIsc_Control])
                                    SELECT top 1 [IV_TEST_KEY],[DEVICENUM],[LOT_NUM],[PM],[ISC],[IPM],[VOC],[VPM],[T_DATE],[TTIME]
                                    ,[VC_PSIGN],[DT_PRINTDT],[VC_DEFAULT],[AMBIENTTEMP],[SENSORTEMP],[FF],[EFF],[RS],[RSH],[INTENSITY]
                                    ,[VC_INICUSTCODE],[VC_CUSTCODE],[P_NUM],[C_USERID],[L_ID],[DT_CREATE],[C_PSTATE],[VC_DATAGROUP]
                                    ,[COEF_PMAX],[COEF_ISC],[COEF_VOC],[COEF_IMAX],[COEF_VMAX],[COEF_FF],[VC_TYPE],[I_IDE],[VC_MODNAME]
                                    ,[C_MAINPED],[I_PSTATE],[VC_PSTATE],[VC_PRINTLABELID],[VC_COGCODE],[I_ID],[VC_CELLEFF],[DEC_CTM]
                                    ,[I_PKID],[DEC_PMCHANGE],[VC_WORKORDER],[VC_COLOR],[CALIBRATION_NO],[PRINTEDLABLE],[PRINTED_NP]
                                    ,[TIME_STAMP],[Imp_Isc],[ImpIsc_Control]
                                    FROM [dbo].[WIP_IV_TEST_HIS] T,POR_LOT_HIS T1
                                    WHERE T.LOT_NUM=T1.LOT_NUMBER AND T.VC_DEFAULT='1'  AND T1.LOT_KEY='{0}' and t.COEF_PMAX is not null ", drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                        dbHis.ExecuteNonQuery(CommandType.Text, sqlCommand);
                    }
                }
                #endregion

                #region 检验终检数据
                sqlCommand = string.Format(@"select t.CC_FCODE1 from WIP_CUSTCHECK t ,POR_LOT t1 
                                            where t.CC_FCODE1=t1.LOT_NUMBER and t1.LOT_KEY='{0}' and t.ISFLAG=1", 
                                            drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                DataTable dt04 = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                if (dt04.Rows.Count > 0)
                {
                    sqlCommand = string.Format(@"update t set t.ROOM_KEY='{1}'
                                                from WIP_CUSTCHECK t ,POR_LOT t1 
                                                where t.CC_FCODE1=t1.LOT_NUMBER and t1.LOT_KEY='{0}' and t.ISFLAG=1",
                                                drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString(), 
                                                factory_key.PreventSQLInjection());
                }
                if (dt04.Rows.Count < 1 || Convert.ToString(dt04.Rows[0][0]).Equals(string.Empty))
                {
                    sqlCommand = string.Format(@"INSERT INTO [WIP_CUSTCHECK]
                                                ([CUSTCHECK_KEY],[LOT_TYPE],[CC_DATA_GROUP],[CUSTOMCODE],[l_ID]
                                                ,[CHECK_TIME],[CC_FCODE],[CC_FCODE1],[CC_FCODE2],[CC_FCODE3],[CC_FCODE4],[CC_FCODE5]
                                                ,[CC_FCODE6],[WORKNUMBER],[OPERATERS],[DEVICENUM],[PRO_ID],[CHECK_POWER],[LOT_COLOR]
                                                ,[PRO_LEVEL],[CREATER],[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG],[NAMEPLATENO],[ROOM_KEY]
                                                ,[SHIFT_KEY],[SHIFT_NAME],[REMARK],[REASON_CODE_NAME],[REASON_CODE_KEY],[REASON_CODE_CATEGORY_KEY]
                                                ,[REASON_CODE_CATEGORY_NAME])
                                              SELECT top 1 [CUSTCHECK_KEY],T.[LOT_TYPE],[CC_DATA_GROUP],[CUSTOMCODE],[l_ID]
                                                ,[CHECK_TIME],[CC_FCODE],[CC_FCODE1],[CC_FCODE2],[CC_FCODE3],[CC_FCODE4],[CC_FCODE5]
                                                ,[CC_FCODE6],[WORKNUMBER],[OPERATERS],[DEVICENUM],t.[PRO_ID],[CHECK_POWER],[LOT_COLOR]
                                                ,t.[PRO_LEVEL],[CREATER],t.[CREATE_TIME],T.[EDITOR],t.[EDIT_TIME],[ISFLAG],[NAMEPLATENO],'{1}'
                                                ,[SHIFT_KEY],T.[SHIFT_NAME],[REMARK],[REASON_CODE_NAME],[REASON_CODE_KEY],[REASON_CODE_CATEGORY_KEY]
                                                ,[REASON_CODE_CATEGORY_NAME]
                                                FROM WIP_CUSTCHECK_HIS t, POR_LOT_HIS t1
                                                where t.CC_FCODE1=t1.LOT_NUMBER and t.PRO_LEVEL is not null
                                                and t.CC_DATA_GROUP='1'  and t1.LOT_KEY='{0}'  and t.ISFLAG=1 ", 
                                                drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString(), factory_key.PreventSQLInjection());
                    dbHis.ExecuteNonQuery(CommandType.Text, sqlCommand);
                }


                #endregion


            }

            return errMsg;
        }

        private string GetAboutRouteKey(string type, string s1)
        {
            string sqlCommand = string.Empty;
            string s_bak = string.Empty;
            if (POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME==type)
            {
                sqlCommand = string.Format(@"SELECT T.ROUTE_ENTERPRISE_VER_KEY FROM POR_ROUTE_ENTERPRISE_VER T WHERE T.ENTERPRISE_NAME='{0}'",s1);
                DataTable dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dtCommon.Rows.Count > 0)
                    s_bak = dtCommon.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
            }
            if (POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME==type)
            {
                sqlCommand = string.Format(@"select t.ROUTE_ROUTE_VER_KEY from POR_ROUTE_ROUTE_VER t where t.ROUTE_NAME='{0}'", s1);
                DataTable dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dtCommon.Rows.Count > 0)
                    s_bak = dtCommon.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
            }
            if (POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME == type)
            {
                sqlCommand = string.Format(@"select t.ROUTE_OPERATION_VER_KEY from POR_ROUTE_OPERATION_VER t where t.ROUTE_OPERATION_NAME='{0}'", s1);
                DataTable dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dtCommon.Rows.Count > 0)
                    s_bak = dtCommon.Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY].ToString();
            }

            return s_bak;
        }

    }
}

