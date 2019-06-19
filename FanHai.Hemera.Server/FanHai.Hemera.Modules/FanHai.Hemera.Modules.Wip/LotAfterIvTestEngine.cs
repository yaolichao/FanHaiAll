using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;

using FanHai.Hemera.Utils.DatabaseHelper;
using Microsoft.Practices.EnterpriseLibrary.Data;



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 批次数据的操作类。
    /// </summary>
    public partial class LotEngine : AbstractEngine, ILotEngine
    {

        /// <summary>
        /// 保存终检数据的方法，如果已经保存了数据，则需要先更新数据，再保存。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet SaveLot2CustCheckData(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dt_wip_custcheck = null;
                dt_wip_custcheck = dsParams.Tables[WIP_CUSTCHECK_FIELDS.DATABASE_TABLE_NAME];
                WIP_CUSTCHECK_FIELDS custcheck_fields = new WIP_CUSTCHECK_FIELDS();
                string checktype = Convert.ToString(dsParams.ExtendedProperties[CHECKTYPE.DATA_TYPE]);
                bool bNeedReprint=Convert.ToBoolean(dsParams.ExtendedProperties["REPRINT"]);
                string partNumber = null;
                if (dsParams.ExtendedProperties.Contains(POR_LOT_FIELDS.FIELD_PART_NUMBER))
                {
                    partNumber = Convert.ToString(dsParams.ExtendedProperties[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                }

                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        foreach (DataRow dr in dt_wip_custcheck.Rows)
                        {
                            string fcode1=Convert.ToString(dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1]);
                            string sqlCommand = string.Format(@"SELECT MAX(t.l_ID) MAX_ID FROM WIP_CUSTCHECK t WHERE t.CC_FCODE1='{0}'",
                                                               dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1]);
                            object maxId = db.ExecuteScalar(CommandType.Text, sqlCommand);
                            if (maxId != DBNull.Value && maxId != null)
                            {
                                dr[WIP_CUSTCHECK_FIELDS.FIELDS_l_ID] = Convert.ToInt32(maxId) + 1;
                            }
                            else
                            {
                                dr[WIP_CUSTCHECK_FIELDS.FIELDS_l_ID] = 1;
                            }
                            //终检
                            if (checktype.Equals(CHECKTYPE.DATA_GROUP_ENDCHECK))
                            {
                                sqlCommand = string.Format(@"UPDATE POR_LOT 
                                                            SET PRO_LEVEL='{0}',{1}PRO_ID='{2}',COLOR='{4}'
                                                            WHERE LOT_NUMBER='{3}' AND DELETED_TERM_FLAG<2",
                                                            dr[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_LEVEL],
                                                            partNumber==null?string.Empty:string.Format("PART_NUMBER='{0}',",partNumber),
                                                            dr[WIP_CUSTCHECK_FIELDS.FIELDS_PRO_ID],
                                                            dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1],
                                                            dr[WIP_CUSTCHECK_FIELDS.FIELDS_LOT_COLOR]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                                string sql_update = string.Format(@"UPDATE WIP_CUSTCHECK  
                                                                    SET ISFLAG=0 
                                                                    WHERE CC_FCODE1='{0}' AND CC_DATA_GROUP='1' and ISFLAG=1 ", 
                                                                    dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql_update);
                                dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_DATA_GROUP] = "1";
                            }
                            else
                            {//客检
                                string sql_update = string.Format(@"UPDATE WIP_CUSTCHECK  
                                                                    SET ISFLAG=0 
                                                                    WHERE CC_FCODE1='{0}' AND CC_DATA_GROUP='2' AND ISFLAG=1 ", 
                                                                    dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql_update);
                                dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_DATA_GROUP] = "2";
                            }
                            if (bNeedReprint)
                            {
                                sqlCommand = string.Format(@"UPDATE WIP_IV_TEST 
                                                            SET VC_PSIGN='0'
                                                            WHERE LOT_NUM='{0}' AND VC_DEFAULT='1'",
                                                           dr[WIP_CUSTCHECK_FIELDS.FIELDS_CC_FCODE1]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(custcheck_fields, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                        //Commit Transaction
                        dbTran.Commit();
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SaveLot2CustCheckData Error: " + ex.Message);
            }
            //返回结果集。
            return dsReturn;
        }

        /// <summary>
        /// 根据批次号或者批次ID获得工单号，工单ID和产品ID号信息
        /// </summary>
        /// <param name="s_lot"></param>
        /// <returns>Data Columns:
        /// ORDER_NUMBER,WORK_ORDER_KEY,PRODUCT_CODE,PRODUCT_KEY,PRODUCT_NAME,
        /// LOT_KEY,LOT_NUMBER,PALLET_NO,PRO_LEVEL,PALLET_TIME,FACTORYROOM_KEY,LINE_NAME</returns>
        public DataSet GetWOProductByLotNum(string s_lot, string roomkey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommon = string.Empty;
            try
            {
                sqlCommon = string.Format(@"SELECT B.ORDER_NUMBER,B.WORK_ORDER_KEY,C.PRODUCT_CODE,
                                                    C.PRODUCT_KEY,C.PRODUCT_NAME,C.LABELCHECK,
                                                    C.PROMODEL_NAME,C.LABELTYPE,C.LABELVAR,
                                                    A.LOT_KEY,A.LOT_NUMBER,A.PALLET_NO,
                                                    C.MODULE_TYPE_PREFIX,C.MODULE_TYPE_SUFFIX,
                                                    A.PRO_LEVEL,A.PALLET_TIME,A.FACTORYROOM_KEY,
                                                    A.LINE_NAME,A.LOT_TYPE,A.PART_NUMBER 
                                                    FROM POR_LOT A 
                                                    INNER JOIN POR_WORK_ORDER B ON  A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                    INNER JOIN POR_WO_PRD C ON C.IS_USED = 'Y' AND C.PART_NUMBER = A.PART_NUMBER 
	                                                      AND  B.WORK_ORDER_KEY = C.WORK_ORDER_KEY
                                                    WHERE A.LOT_NUMBER = '{0}'
                                                    AND A.DELETED_TERM_FLAG<>2 
                                                    AND A.FACTORYROOM_KEY='{1}';", s_lot, roomkey);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);

                if (dsReturn.Tables[0].Rows.Count == 0)
                {
                    sqlCommon = string.Format(@"SELECT B.ORDER_NUMBER,B.WORK_ORDER_KEY,C.PRODUCT_CODE,
                                                    C.PRODUCT_KEY,C.PRODUCT_NAME,C.LABELCHECK,
                                                    C.PROMODEL_NAME,C.LABELTYPE,C.LABELVAR,
                                                    A.LOT_KEY,A.LOT_NUMBER,A.PALLET_NO,
                                                    C.MODULE_TYPE_PREFIX,C.MODULE_TYPE_SUFFIX,
                                                    A.PRO_LEVEL,A.PALLET_TIME,A.FACTORYROOM_KEY,
                                                    A.LINE_NAME,A.LOT_TYPE,A.PART_NUMBER 
                                                    FROM POR_LOT A 
                                                    INNER JOIN POR_WORK_ORDER B ON  A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                    INNER JOIN POR_WO_PRD C ON C.IS_USED = 'Y' AND C.PART_NUMBER = A.PART_NUMBER 
	                                                      AND  B.WORK_ORDER_KEY = C.WORK_ORDER_KEY
                                                    WHERE A.LOT_KEY = '{0}'
                                                    AND A.DELETED_TERM_FLAG<>2 
                                                    AND A.FACTORYROOM_KEY='{1}';", s_lot, roomkey);

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                }

                dsReturn.Tables[0].TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                if (dsReturn.Tables[0].Rows.Count < 1)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "未找到工单和产品ID号信息！");
                }
                else
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWOProductByLotNum Error: " + ex.Message);
            }
            return dsReturn;

        }
        /// <summary>
        /// 获取批次档位名称及其子分档名称。
        /// </summary>
        /// <param name="Lot_Number"></param>
        /// <returns></returns>
        public DataSet GetModulePowerInfo(string Lot_Number)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommon = string.Empty;

            try
            {
                sqlCommon = string.Format(@"DECLARE @isEnableByProduct INT
                                            SET @isEnableByProduct=0;
                                            --是否启用联副产品入库
                                            IF EXISTS(SELECT 1 FROM POR_LOT a
                                                      INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                      WHERE a.LOT_NUMBER='{0}')
                                            BEGIN
                                                SET @isEnableByProduct=1
                                            END;

                                            DECLARE @MODULE_NAME NVARCHAR(50),@POWERSET_KEY NVARCHAR(50),@PS_SUB_SEQ NVARCHAR(50),@PART_NUMBER NVARCHAR(50),@WORK_ORDER_KEY NVARCHAR(50)
                                            IF @isEnableByProduct=0
                                            BEGIN
                                                SELECT @MODULE_NAME=CONVERT(VARCHAR(10),t.PS_SEQ,0)+':'+t.MODULE_NAME,@POWERSET_KEY=t.POWERSET_KEY,@PS_SUB_SEQ=t2.I_PKID 
                                                FROM BASE_POWERSET t 
                                                RIGHT JOIN WIP_IV_TEST t2 ON t.PS_CODE=t2.VC_TYPE AND t.PS_SEQ=t2.I_IDE
                                                WHERE t2.LOT_NUM='{0}' 
                                                AND t2.COEF_PMAX BETWEEN t.P_MIN AND t.P_MAX
                                                AND t2.VC_DEFAULT='1' 
                                                AND t.ISFLAG=1;

                                                SELECT @MODULE_NAME AS MODULE_NAME
                                                SELECT  CONVERT(VARCHAR(10),PS_DTL_SUBCODE,0)+':'+POWERLEVEL AS POWERLEVEL
                                                FROM BASE_POWERSET_DETAIL 
                                                WHERE POWERSET_KEY=@POWERSET_KEY AND PS_DTL_SUBCODE=@PS_SUB_SEQ;
                                            END
                                            ELSE
                                            BEGIN
                                                SELECT @MODULE_NAME=CONVERT(VARCHAR(10),t.PS_SEQ,0)+':'+t.MODULE_NAME,@POWERSET_KEY=t.POWERSET_KEY,
                                                       @PS_SUB_SEQ=t2.I_PKID ,@PART_NUMBER = T.PART_NUMBER,@WORK_ORDER_KEY = T1.WORK_ORDER_KEY
                                                FROM POR_WO_PRD_PS t 
                                                INNER JOIN POR_LOT t1 ON T.WORK_ORDER_KEY = T1.WORK_ORDER_KEY AND T1.PART_NUMBER = T.PART_NUMBER
                                                RIGHT JOIN WIP_IV_TEST t2 ON t.PS_CODE=t2.VC_TYPE AND t.PS_SEQ=t2.I_IDE AND T1.LOT_NUMBER = T2.LOT_NUM
                                                WHERE t1.LOT_NUMBER='{0}' 
                                                AND t2.COEF_PMAX BETWEEN t.P_MIN AND t.P_MAX
                                                AND t2.VC_DEFAULT='1' 
                                                AND t.IS_USED='Y';

                                                SELECT @MODULE_NAME AS MODULE_NAME
                                                SELECT CONVERT(VARCHAR(10),PS_SUB_CODE,0)+':'+POWERLEVEL AS POWERLEVEL
                                                FROM POR_WO_PRD_PS_SUB
                                                WHERE POWERSET_KEY=@POWERSET_KEY 
                                                AND PS_SUB_CODE=@PS_SUB_SEQ 
                                                AND PART_NUMBER = @PART_NUMBER 
                                                AND WORK_ORDER_KEY = @WORK_ORDER_KEY
                                                AND IS_USED = 'Y';
                                            END", 
                                            Lot_Number);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetModulePowerInfo Error: " + ex.Message);
            }

            return dsReturn;

        }
        /// <summary>
        /// 判断终检过账未完成的数据是否已经存在。
        /// 主要是用来设定该数据是否已经做过终检，如果已经终检过而没有出站，则带出相应数据，否则返回数据为空
        /// </summary>
        /// <param name="s_lot"></param>
        /// <returns>返回已经终检但未出站的信息</returns>
        public DataSet GetCustCheckDataGroupByZero(string s_lot, string roomkey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommon = string.Empty;

            try
            {
                sqlCommon = string.Format(@"select TOP 1 T.*,T1.LOT_KEY from WIP_CUSTCHECK t 
                                            inner join POR_LOT t1 on t.CC_FCODE1=t1.LOT_NUMBER
                                            where t1.DELETED_TERM_FLAG<>2 AND T1.STATE_FLAG<10
                                            AND t.CC_DATA_GROUP='0' AND t.CC_FCODE1='{0}' and t.ROOM_KEY='{1}'  ORDER BY T.CREATE_TIME DESC ", s_lot, roomkey);

                DataTable dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtCommon.TableName = WIP_CUSTCHECK_FIELDS.DATABASE_TABLE_NAME;

                dsReturn.Merge(dtCommon, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCustCheckDataGroupByZero Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 获得服务端时间，防止PC时间被修改，而导致客户端时间判断有误
        /// </summary>
        /// <returns>数据库当前时间</returns>
        public DateTime GetSysdate()
        {
            return UtilHelper.GetSysdate(db);
        }

        /// <summary>
        /// 获得包装数据
        /// </summary>
        /// <param name="s_lot"></param>
        /// <param name="roomkey"></param>
        /// <returns>1，返回工单产品信息表；2，返回分档数据表；3，返回分档明细表</returns>
        public DataSet GetLotPorWOForPallet(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtCommon = new DataTable();
            string s_lot = string.Empty, roomkey = string.Empty, s_palletNo = string.Empty, s_wo = string.Empty, s_date = string.Empty, e_date = string.Empty;
            string s_lot2 = string.Empty, s_palletNo2 = string.Empty;

            string flag = Convert.ToString(hstable["flag"]);
            if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                s_lot = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY))
                roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                s_palletNo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME))
                s_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME))
                e_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER))
                s_wo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO + "2"))
                s_palletNo2 = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO + "2"].ToString();
            if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER + "2"))
                s_lot2 = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER + "2"].ToString();

            string sqlCommand = string.Empty;

            if (flag.Equals("consigment"))
            {
                sqlCommand = @"select  t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER  from WIP_CONSIGNMENT t where t.ISFLAG=1";
                if (!string.IsNullOrEmpty(s_palletNo))
                    sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO + "='{0}'", s_palletNo);
                if (!string.IsNullOrEmpty(roomkey))
                    sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY + "='{0}'", roomkey);

                if (!string.IsNullOrEmpty(s_wo))
                    sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER + "='{0}'", s_wo);
                if (!string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
                    sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + " between  CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')", s_date, e_date);
                else if (!string.IsNullOrEmpty(s_date))
                    sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + " >= CONVERT(datetime,'{0}')", s_date);
                else if (!string.IsNullOrEmpty(e_date))
                    sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + " <= CONVERT(datetime,'{0}')", e_date);

                dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtCommon.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
            }

            if (flag.Equals("pallet"))
            {
                sqlCommand = @"SELECT ROW_NUMBER() OVER (ORDER BY T1.PALLET_TIME asc)  SEQ, 
                                t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CREATER,t.CREATE_TIME,
                                t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EDITOR,t.EDIT_TIME,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,t.GRADE,t.LAST_PALLET,
                                t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO,t.PALLET_NO_NEW,t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,
                                t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER,
                                 t1.LOT_NUMBER,t1.LOT_CUSTOMERCODE,t2.CHECK_POWER,t1.LOT_SIDECODE,t1.PALLET_TIME  
                                from WIP_CONSIGNMENT t inner join POR_LOT t1 on t.PALLET_NO=t1.PALLET_NO
                                left join WIP_CUSTCHECK t2 on t1.LOT_NUMBER=t2.CC_FCODE1 and t2.ISFLAG=1
                                where t.ISFLAG=1 and  t2.CC_DATA_GROUP='1' ";

                if (!string.IsNullOrEmpty(s_lot) && !string.IsNullOrEmpty(s_lot2))
                    sqlCommand += string.Format(@" and t1.LOT_NUMBER>='{0}' and t1.LOT_NUMBER<='{1}'", s_lot, s_lot2);
                else if (!string.IsNullOrEmpty(s_lot))
                    sqlCommand += string.Format(@" and t1.LOT_NUMBER='{0}' ", s_lot);
                else if (!string.IsNullOrEmpty(s_lot2))
                    sqlCommand += string.Format(@" and t1.LOT_NUMBER='{0}' ", s_lot2);

                if (!string.IsNullOrEmpty(s_palletNo) && !string.IsNullOrEmpty(s_palletNo2))
                    sqlCommand += string.Format("  and t.PALLET_NO>='{0}' and t.PALLET_NO<='{1}'", s_palletNo, s_palletNo2);
                else if (!string.IsNullOrEmpty(s_palletNo))
                    sqlCommand += string.Format("  and t.PALLET_NO='{0}'", s_palletNo);
                else if (!string.IsNullOrEmpty(s_palletNo2))
                    sqlCommand += string.Format("  and t.PALLET_NO='{0}'", s_palletNo2);

                if (!string.IsNullOrEmpty(roomkey))
                    sqlCommand += string.Format(" and t.ROOM_KEY='{0}'", roomkey);

                if (!string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
                    sqlCommand += string.Format(" and T1." + POR_LOT_FIELDS.FIELD_PALLET_TIME + " between  CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')", s_date, e_date);
                else if (!string.IsNullOrEmpty(s_date))
                    sqlCommand += string.Format(" and T1." + POR_LOT_FIELDS.FIELD_PALLET_TIME + " >= CONVERT(datetime,'{0}')", s_date);
                else if (!string.IsNullOrEmpty(e_date))
                    sqlCommand += string.Format(" and T1." + POR_LOT_FIELDS.FIELD_PALLET_TIME + " <= CONVERT(datetime,'{0}')", e_date);

                //多选包装设备
                if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY))
                {
                    string[] s_array = Convert.ToString(hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY]).Split(',');
                    string equipmentkey = string.Empty;
                    foreach (string s in s_array)
                    {
                        equipmentkey += "'" + s.Trim() + "',";
                    }
                    if (!string.IsNullOrEmpty(equipmentkey))
                    {
                        equipmentkey = equipmentkey.TrimEnd(',');
                        sqlCommand += string.Format(" and t." + WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY + " in ({0})", equipmentkey);
                    }                   
                }

                sqlCommand += " order by t1.PALLET_TIME asc ";

                dtCommon = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtCommon.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
            }

            dsReturn.Merge(dtCommon, true, MissingSchemaAction.Add);

            return dsReturn;

        }

        private DataTable GetIvTestData(string s_lot)
        {
            //获取IV测试数据
            string sqlCommand = string.Format(@"SELECT T.VC_TYPE,T.I_IDE,T.I_ID,T.I_PKID,T.LOT_NUM,T.COEF_PMAX,
                                                       a.POWERSET_KEY,a.PMAXSTAB,
                                                       a.PS_CODE,
                                                       a.PS_CODE+':'+a.PS_RULE AS PS_RULE1,
                                                       CAST(a.PS_SEQ AS VARCHAR)+':'+a.MODULE_NAME AS MODULE_NAME1,
                                                       b.POWERLEVEL
                                                FROM WIP_IV_TEST t
                                                LEFT JOIN BASE_POWERSET a ON a.PS_CODE=t.VC_TYPE AND a.PS_SEQ=t.I_IDE AND a.ISFLAG=1
                                                LEFT JOIN BASE_POWERSET_DETAIL b ON b.POWERSET_KEY=a.POWERSET_KEY AND b.PS_DTL_SUBCODE=t.I_PKID
                                                WHERE t.LOT_NUM='{0}' AND ISNULL(t.COEF_PMAX,0)<>0 and t.VC_DEFAULT='1'
                                                ORDER BY t.DT_CREATE DESC", s_lot);
            return db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
        }
        private DataTable GetAvgPowerRangeData(string s_lot)
        {
            string sqlCommand = string.Format(@"SELECT t3.AVGPOWER_MAX,t3.AVGPOWER_MIN,t3.AVG_POWER_KEY,t3.POWERSET_KEY,t3.PS_CODE,t3.PS_SEQ,t3.TESTRULE_KEY
                                                FROM POR_LOT t 
                                                INNER JOIN POR_PRODUCT t1 ON t.PRO_ID=t1.PRODUCT_CODE
                                                LEFT JOIN BASE_TESTRULE t2 ON t1.PRO_TEST_RULE=t2.TESTRULE_CODE
                                                LEFT JOIN BASE_TESTRULE_AVGPOWER t3 on t2.TESTRULE_KEY=t3.TESTRULE_KEY  
                                                where t.LOT_NUMBER='{0}' 
                                                AND t.DELETED_TERM_FLAG<2 
                                                AND t1.ISFLAG=1 
                                                AND t2.ISFLAG=1 
                                                AND t3.ISFLAG=1", 
                                                s_lot.PreventSQLInjection());
            return db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
        }
        /// <summary>
        /// 根据批次等级获得工单或者产品ID号设定的SAP料号
        /// </summary>
        /// <param name="s_lot"></param>
        /// <returns></returns>
        private DataTable GetSapMaterial(string s_lot)
        {
            string sqlCommand = string.Empty;
            sqlCommand = string.Format(@"SELECT t2.ATTRIBUTE_VALUE,t2.ATTRIBUTE_NAME,t.PRO_LEVEL
                                        FROM POR_LOT t 
                                        INNER JOIN POR_WORK_ORDER t1 ON t.WORK_ORDER_KEY=t1.WORK_ORDER_KEY AND t.WORK_ORDER_NO=t1.ORDER_NUMBER
                                        INNER JOIN POR_WORK_ORDER_ATTR t2 ON t1.WORK_ORDER_KEY=t2.WORK_ORDER_KEY AND t2.ISFLAG=1
                                        WHERE T.LOT_NUMBER='{0}'", s_lot);
            DataTable dtAttribute = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            DataTable dtSapNo = new DataTable();
            
            if (dtAttribute.Rows.Count > 0)
            {
                string pro_level = Convert.ToString(dtAttribute.Rows[0]["PRO_LEVEL"]);
                foreach (DataRow dr in dtAttribute.Rows)
                {
                    //没有等级，不在遍历工单属性设置
                    if (string.IsNullOrEmpty(pro_level)) break;

                    if (Convert.ToString(dr["ATTRIBUTE_NAME"]).Contains(pro_level))
                    {
                        dtSapNo.Columns.Add(POR_PRODUCT_DTL.FIELDS_SAP_PN);
                        DataRow drNew = dtSapNo.NewRow();
                        drNew[POR_PRODUCT_DTL.FIELDS_SAP_PN] = dr["ATTRIBUTE_VALUE"];
                        dtSapNo.Rows.Add(drNew);
                        dtSapNo.AcceptChanges();
                        break;
                    }
                }
            }
            if (dtSapNo.Rows.Count < 1)
            {
                sqlCommand = string.Format(@"SELECT t2.SAP_PN
                                            FROM POR_LOT t
                                            INNER JOIN POR_PRODUCT t1 ON t.PRO_ID=t1.PRODUCT_CODE
                                            INNER JOIN POR_PRODUCT_DTL t2 ON t1.PRODUCT_KEY=t2.PRODUCT_KEY 
                                            WHERE T.LOT_NUMBER='{0}' AND T.PRO_LEVEL=T2.PRODUCT_GRADE AND t2.ISFLAG=1 AND t1.ISFLAG=1 ", s_lot);
                dtSapNo = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            }

            return dtSapNo;
        }

        public DataSet GetPalletOrLotData(Hashtable hstable)
        {
            #region
            DataSet dsReturn = new DataSet();
            string flag = hstable["flag"].ToString();
            string check_type = string.Empty;
            string s_lot = string.Empty;
            string roomkey = string.Empty;
            string s_palletNo = string.Empty;
            string s_date = string.Empty;
            string e_date = string.Empty;
            string pro_id = string.Empty;
            if(hstable.ContainsKey("check"))
                check_type = Convert.ToString(hstable["check"]);
            if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                s_lot = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY))
                roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                s_palletNo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME))
                s_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME))
                e_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME].ToString();
            if (hstable.ContainsKey(POR_PRODUCT.FIELDS_PRODUCT_CODE))
                pro_id = Convert.ToString(hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE]);
            string sqlCommand = string.Empty;
            #endregion
            try
            {
                //查询入库检验时批次信息

                if (flag == "custcheck")
                {
                    #region
                    //获得批次数据
                    sqlCommand = string.Format(@"SELECT t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,
                                                        t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,
                                                        t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,
                                                        t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,
                                                        t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,
                                                        t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                        t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,
                                                        t.WORK_ORDER_NO,t.WORK_ORDER_SEQ 
                                                FROM POR_LOT t 
                                                WHERE t.LOT_NUMBER='{0}' 
                                                AND t.FACTORYROOM_KEY='{1}'
                                                AND t.DELETED_TERM_FLAG<2",
                                                s_lot, roomkey);
                    DataTable dtCommon01 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtCommon01.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtCommon01, true, MissingSchemaAction.Add);
                    //获取IV测试数据
                    sqlCommand = string.Format(@"SELECT TOP 1 [IV_TEST_KEY],[DEVICENUM],[LOT_NUM],[PM],[ISC]
                                                    ,[IPM],[VOC],[VPM],[T_DATE],[TTIME],[VC_PSIGN],[DT_PRINTDT],[VC_DEFAULT]
                                                    ,[AMBIENTTEMP],[SENSORTEMP],[FF],[EFF],[RS],[RSH],[INTENSITY],[VC_INICUSTCODE]
                                                    ,[VC_CUSTCODE],[P_NUM],[C_USERID],[L_ID],[DT_CREATE],[C_PSTATE],[VC_DATAGROUP]
                                                    ,[COEF_PMAX],[COEF_ISC],[COEF_VOC],[COEF_IMAX],[COEF_VMAX],[COEF_FF],[VC_TYPE]
                                                    ,[I_IDE],[VC_MODNAME],[C_MAINPED],[I_PSTATE],[VC_PSTATE],[VC_PRINTLABELID]
                                                    ,[VC_COGCODE],[I_ID],[VC_CELLEFF],[DEC_CTM],[I_PKID],[DEC_PMCHANGE],[VC_WORKORDER]
                                                    ,[VC_COLOR],[CALIBRATION_NO],[PRINTEDLABLE],[PRINTED_NP],[TIME_STAMP]
                                                FROM WIP_IV_TEST t 
                                                WHERE t.LOT_NUM='{0}' AND t.VC_DEFAULT='1' AND ISNULL(t.COEF_PMAX,0)>0 
                                                ORDER BY t.DT_CREATE DESC", 
                                                s_lot);
                    DataTable dtCommon02 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtCommon02.TableName = WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtCommon02, true, MissingSchemaAction.Add);
                    //获得功率档位
                    sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                          INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                          WHERE a.LOT_NUMBER='{0}')
                                                BEGIN
                                                    SELECT t.PMAXSTAB
                                                    FROM POR_LOT a
                                                    INNER JOIN WIP_IV_TEST b ON b.LOT_NUM=a.LOT_NUMBER AND b.VC_DEFAULT='1'
                                                    INNER JOIN POR_WO_PRD_PS t ON t.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                                               AND t.PART_NUMBER=a.PART_NUMBER 
                                                                               AND t.PS_CODE=b.VC_TYPE 
                                                                               AND t.PS_SEQ=b.I_IDE
                                                    WHERE a.LOT_NUMBER='{0}'
                                                    AND t.IS_USED='Y';
                                                END
                                                ELSE
                                                BEGIN
                                                    SELECT t.PMAXSTAB
                                                    FROM BASE_POWERSET t 
                                                    LEFT JOIN( SELECT TOP 1 a.LOT_NUM,a.I_IDE,A.VC_TYPE,A.COEF_PMAX 
                                                               FROM WIP_IV_TEST a 
                                                               WHERE ISNULL(a.COEF_PMAX,0)<>0 
                                                               AND a.VC_DEFAULT='1' 
                                                               AND a.LOT_NUM='{0}'
                                                               ORDER BY a.DT_CREATE DESC)t2 ON t.PS_CODE=t2.VC_TYPE AND t.PS_SEQ=t2.I_IDE
                                                    WHERE t2.LOT_NUM='{0}' 
                                                    AND t2.COEF_PMAX BETWEEN t.P_MIN AND t.P_MAX 
                                                    AND t.ISFLAG=1;
                                                END", s_lot);
                    DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtPowerSet.TableName = BASE_POWERSET.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);             
                    #endregion
                }
                //校验包装时批次信息
                else if (flag == "lot")
                {
                    #region
                    //获取IV测试数据  
                    if (check_type == "iv_test")
                    {
                        DataTable dtCommon01 = GetIvTestData(s_lot);
                        dtCommon01.TableName = WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dtCommon01, true, MissingSchemaAction.Add);
                    }
                    //获取平均功率范围
                    else if (check_type == "avg_power")
                    {
                        DataTable dtCommon02 = GetAvgPowerRangeData(s_lot);
                        dtCommon02.TableName = BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dtCommon02, true, MissingSchemaAction.Add);
                    }
                    //获取SAP料号
                    else if (check_type == "sap_material")
                    {
                        DataTable dtCommon03 = GetSapMaterial(s_lot);
                        dtCommon03.TableName = POR_PRODUCT_DTL.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dtCommon03, true, MissingSchemaAction.Add);
                    }              
                    //获取满托数
                    else if (check_type == "fullpallet_qty")
                    {
                        sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.LOT_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT e.FULL_PALLET_QTY
                                                        FROM POR_LOT a
                                                        LEFT JOIN POR_PRODUCT b ON b.PRODUCT_CODE=a.PRO_ID AND b.ISFLAG=1
                                                        LEFT JOIN POR_WO_PRD e ON e.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
                                                                                        AND e.PRODUCT_KEY=b.PRODUCT_KEY 
                                                                                        AND e.IS_USED='Y'
                                                        WHERE a.LOT_NUMBER='{0}';
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        SELECT t2.FULL_PALLET_QTY
                                                        FROM POR_PRODUCT t1 
                                                        INNER JOIN BASE_TESTRULE t2 ON t1.PRO_TEST_RULE=t2.TESTRULE_CODE
                                                        WHERE t1.ISFLAG=1 
                                                        AND t2.ISFLAG=1 
                                                        AND t1.PRODUCT_CODE='{1}';
                                                    END ",
                                                    s_lot.PreventSQLInjection(),
                                                    pro_id.PreventSQLInjection());
                        DataTable dtCommon05 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        dtCommon05.TableName = BASE_TESTRULE.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dtCommon05, true, MissingSchemaAction.Add);
                    }
                    //批次等级，花色
                    else if (check_type == "check_lot_level")
                    {
                        sqlCommand = string.Format(@"SELECT T.CC_FCODE1,T.CC_FCODE2,T.WORKNUMBER,T.PRO_ID,T.CHECK_POWER,T.LOT_COLOR,T.ROOM_KEY,
                                                           T.SHIFT_KEY,T.PRO_LEVEL,a.GRADE_NAME
                                                    FROM WIP_CUSTCHECK t 
                                                    LEFT JOIN V_PRODUCTGRADE a ON t.PRO_LEVEL=a.GRADE_CODE
                                                    WHERE t.CC_FCODE1='{0}'                                                     
                                                    AND t.CC_DATA_GROUP='1'
                                                    AND t.ISFLAG=1 
                                                    AND t.PRO_LEVEL IS NOT NULL", s_lot);
                        DataTable dtCommon06 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        dtCommon06.TableName = WIP_CUSTCHECK_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dtCommon06, true, MissingSchemaAction.Add);
                    }
                    #endregion
                }
                else if (flag == "pallet")
                {
                    //查询托盘信息
                    #region
                    sqlCommand = @"select [CONSIGNMENT_KEY],[VIRTUAL_PALLET_NO],[PALLET_NO],[WORKNUMBER],[CS_DATA_GROUP]
                                    ,[SAP_NO],[POWER_LEVEL],[GRADE],[SHIFT],[PS_CODE],[PS_DTL_SUBCODE],[LAST_PALLET]
                                    ,[CREATER],[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG],[ROOM_KEY],[CUSTOMER_NO],[LOT_NUMBER_QTY]
                                    ,[TOTLE_POWER],[AVG_POWER],[PRO_ID],[PALLET_NO_NEW],[PALLET_TYPE],[CODE_TYPE],[LINE_NAME]
                                    ,[LINE_KEY],[EQUIPMENT_KEY],[EQUIPMENT_NAME],[AVG_POWER_RANGE],[LOT_COLOR],[PS_SEQ],[CHECKER]
                                    ,[CHECK_TIME],[TO_WH],[TO_WH_TIME],[OUT_WH],[OUT_WH_TIME],[MEMO1]  
                                  from WIP_CONSIGNMENT t 
                                  where t.CS_DATA_GROUP<>'4' and t.ISFLAG=1 and ";
                    string sfilter = " 1=1";
                    if (!string.IsNullOrEmpty(s_palletNo))
                        sfilter += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO + "='{0}'", s_palletNo);
                    if (!string.IsNullOrEmpty(roomkey))
                        sfilter += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY + "='{0}'", roomkey);
                    if (!string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
                        sfilter += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + " between '{0}' and {1}", s_date, e_date);
                    else if (!string.IsNullOrEmpty(s_date))
                        sfilter += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + " > '{0}'", s_date);
                    else if (!string.IsNullOrEmpty(e_date))
                        sfilter += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + " < '{0}'", e_date);

                    DataTable dtCommon01 = db.ExecuteDataSet(CommandType.Text, sqlCommand + sfilter).Tables[0];
                    if (dtCommon01.Rows.Count < 1)
                    {
                        sqlCommand = @"select [CONSIGNMENT_KEY],[VIRTUAL_PALLET_NO],[PALLET_NO],[WORKNUMBER],[CS_DATA_GROUP]
                                    ,[SAP_NO],[POWER_LEVEL],[GRADE],[SHIFT],[PS_CODE],[PS_DTL_SUBCODE],[LAST_PALLET]
                                    ,[CREATER],[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG],[ROOM_KEY],[CUSTOMER_NO],[LOT_NUMBER_QTY]
                                    ,[TOTLE_POWER],[AVG_POWER],[PRO_ID],[PALLET_NO_NEW],[PALLET_TYPE],[CODE_TYPE],[LINE_NAME]
                                    ,[LINE_KEY],[EQUIPMENT_KEY],[EQUIPMENT_NAME],[AVG_POWER_RANGE],[LOT_COLOR],[PS_SEQ],[CHECKER]
                                    ,[CHECK_TIME],[TO_WH],[TO_WH_TIME],[OUT_WH],[OUT_WH_TIME],[MEMO1]   from WIP_CONSIGNMENT_HIS t where t.CS_DATA_GROUP<>'4' and t.ISFLAG=1 and ";
                        dtCommon01 = db.ExecuteDataSet(CommandType.Text, sqlCommand + sfilter).Tables[0];
                    }

                    dtCommon01.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtCommon01, true, MissingSchemaAction.Add);

                    sqlCommand = @"select  t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,
                                t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,
                                t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ,
                                t1.AVG_POWER,t1.AVG_POWER_RANGE,t1.CHECKER,t1.CHECK_TIME,
                                t1.CODE_TYPE,t1.CONSIGNMENT_KEY,t1.CS_DATA_GROUP,t1.CUSTOMER_NO,t1.EQUIPMENT_KEY,t1.EQUIPMENT_NAME,
                                t1.GRADE,t1.LAST_PALLET,t1.LINE_KEY,t1.LINE_NAME,t1.LOT_COLOR,t1.LOT_NUMBER_QTY,t1.PALLET_NO_NEW,t1.PALLET_NO_NEW,
                                t1.PALLET_TYPE,t1.POWER_LEVEL,t1.PRO_ID,t1.PS_CODE,t1.PS_DTL_SUBCODE,t1.PS_SEQ,t1.ROOM_KEY,t1.SAP_NO,
                                t1.SHIFT,t1.TOTLE_POWER,t1.VIRTUAL_PALLET_NO,t1.WORKNUMBER
                                 from POR_LOT t inner join WIP_CONSIGNMENT t1 on t.PALLET_NO=t1.PALLET_NO where t.DELETED_TERM_FLAG<2  and t1.ISFLAG=1 ";
                    if (!string.IsNullOrEmpty(s_palletNo))
                        sqlCommand += string.Format(" and t1." + WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO + "='{0}'", s_palletNo);
                    if (!string.IsNullOrEmpty(roomkey))
                        sqlCommand += string.Format(" and " + WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY + "='{0}'", roomkey);

                    DataTable dtCommon02 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtCommon02.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtCommon02, true, MissingSchemaAction.Add);
                    #endregion
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);

                LogService.LogError("GetPalletOrLotData Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 从入库检返托号到包装
        /// </summary>
        /// <param name="dsPamas"></param>
        /// <returns></returns>
        public DataSet SavePallet2Package(DataSet dsPamas)
        {
            DataSet dsReturn = new DataSet();
            string palletno = Convert.ToString(dsPamas.ExtendedProperties[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO]);
            string roomkey = Convert.ToString(dsPamas.ExtendedProperties[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY]);
            try
            {
                string sqlCommand = string.Format(@"update WIP_CONSIGNMENT set CS_DATA_GROUP='10' 
                                                where ISFLAG=1 and PALLET_NO='{0}' and ROOM_KEY='{1}'
                                                and CS_DATA_GROUP in ('1','2')", palletno, roomkey);
                db.ExecuteNonQuery(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);

                LogService.LogError("SavePallet2Package Error: " + ex.Message);
            }

            return dsReturn;
        }

        public DataSet SavePalletLotData(DataSet dsParams01)
        {
            DataSet dsReturn = new DataSet();
            DataTable dt_wip_consigment = null;
            DataTable dt_por_lot_update = null;

            dt_wip_consigment = dsParams01.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            dt_por_lot_update = dsParams01.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];

            WIP_CONSIGNMENT_FIELDS consigment_fields = new WIP_CONSIGNMENT_FIELDS();
            POR_LOT_FIELDS lot_fields = new POR_LOT_FIELDS();
            WIP_CONSIGNMENT_TEMP_FIELDS consigment_temp_fields = new WIP_CONSIGNMENT_TEMP_FIELDS();
            string sqlCommand = string.Empty;
            string _errorMsg = string.Empty;
            string _creator = string.Empty;
            string palletno = string.Empty;
            string palletkey = string.Empty;
            string palletNo = string.Empty;
            string rooomkey = string.Empty;
            string operationName = string.Empty, opr_computer = string.Empty;
            string equipmentkey = string.Empty, equipmentname = string.Empty;
            string savetype = dsParams01.ExtendedProperties["savetype"].ToString();
            string linekey=string.Empty,linename=string.Empty;
            string shiftKey = string.Empty, shiftValue = string.Empty;
            string enterpriseKey = string.Empty, routeKey = string.Empty, stepKey = string.Empty;
            string toEnterpriseKey = string.Empty, toRouteKey = string.Empty, toStepKey = string.Empty;
            string toEnterpriseName = string.Empty, toRouteName = string.Empty, toStepName = string.Empty;

            if (dsParams01.ExtendedProperties.ContainsKey(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME))
                operationName = dsParams01.ExtendedProperties[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString();
            if (dsParams01.ExtendedProperties.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER))
                opr_computer = dsParams01.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER].ToString();
            if (dsParams01.ExtendedProperties.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY))
                shiftKey = dsParams01.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY].ToString();
            if (dsParams01.ExtendedProperties.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME))
                shiftValue = dsParams01.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME].ToString();

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {

                    #region update consigment
                    if (dt_wip_consigment != null && dt_wip_consigment.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_wip_consigment.Rows)
                        {
                            palletno = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
                            _creator = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATER].ToString();
                            palletkey = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString();
                            palletNo = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO].ToString();
                            rooomkey = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
                            linekey = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_KEY].ToString();
                            linename = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_LINE_NAME].ToString();
                            equipmentkey = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY].ToString();
                            equipmentname = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME].ToString();

                            sqlCommand = string.Format(@"select  t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                        t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                        t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                        t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                        t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER  
                                                        from WIP_CONSIGNMENT t 
                                                        where t.VIRTUAL_PALLET_NO='{0}' and t.ISFLAG=1",
                                                        dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO].ToString());

                            DataTable dt = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                palletkey = dt.Rows[0][WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString();
                                palletNo = dt.Rows[0][WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO].ToString();
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] = palletkey;
                               
                                WhereConditions wc = new WhereConditions(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString());

                                //出托作业
                                if (savetype.Equals("out"))
                                {
                                    string strDelSigmentHis = string.Format("delete from WIP_CONSIGNMENT_HIS where CONSIGNMENT_KEY='{0}'",
                                        hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY].ToString());
                                    db.ExecuteNonQuery(CommandType.Text, strDelSigmentHis);

                                    hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_ISFLAG] = 0;
                                    hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_TYPE] = 1;


                                    if (dsParams01.ExtendedProperties.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY))
                                        equipmentkey = dsParams01.ExtendedProperties[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_KEY].ToString();
                                    if (dsParams01.ExtendedProperties.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME))
                                        equipmentname = dsParams01.ExtendedProperties[WIP_CONSIGNMENT_FIELDS.FIELDS_EQUIPMENT_NAME].ToString();

                                    string sqlCommand01 = string.Format(@"select  t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                                                            t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                                                            t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                                                                            t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                                                            t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER ,t1.LOT_NUMBER 
                                                    from WIP_CONSIGNMENT t inner join POR_LOT t1 on t.PALLET_NO=t1.PALLET_NO
                                                    where t.VIRTUAL_PALLET_NO='{0}' and t.ISFLAG=1 and t.ROOM_KEY='{1}'", palletno, rooomkey);
                                    dt_por_lot_update = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand01).Tables[0];

                                }
                                else if (savetype.Equals("onlysave"))
                                {
                                    hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 0;
                                }
                                else
                                {
                                    hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 1;
                                }

                              

                                hashTable.Remove(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY);
                                sqlCommand = DatabaseTable.BuildUpdateSqlStatement(consigment_fields, hashTable, wc);
                                db.ExecuteNonQuery(dbTran,CommandType.Text, sqlCommand);

                            }
                            else
                            {                               
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                                if (savetype.Equals("onlysave"))
                                    hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 0;
                                else
                                    hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 1;

                                hashTable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_TYPE] = 0;

                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(consigment_fields, hashTable, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            }
                        }
                    }
                    #endregion
                   
                    //包装过账
                    if (savetype.Equals("real"))
                    {
                        #region 更新批次表
                        foreach (DataRow dr in dt_por_lot_update.Rows)
                        {
                            //更新批次信息
                            sqlCommand = string.Format(@"UPDATE POR_LOT 
                                                        SET LINE_NAME='{0}',CUR_PRODUCTION_LINE_KEY='{1}',EDIT_TIME=GETDATE(),EDITOR='{2}',STATE_FLAG=9
                                                        WHERE LOT_NUMBER='{3}' AND FACTORYROOM_KEY='{4}'",
                                                        linename.PreventSQLInjection(),
                                                        linekey.PreventSQLInjection(),
                                                        _creator.PreventSQLInjection(),
                                                        Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]),
                                                        rooomkey);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            sqlCommand = string.Format(@"select t.LOT_KEY,t.LOT_NUMBER,t.QUANTITY,t.QUANTITY_INITIAL,t.CUR_ROUTE_VER_KEY,
                                                        t.CUR_STEP_VER_KEY,t.ROUTE_ENTERPRISE_VER_KEY 
                                                        from POR_LOT t
                                                        where t.LOT_NUMBER='{0}'
                                                        and t.FACTORYROOM_KEY='{1}' 
                                                        and t.DELETED_TERM_FLAG<2  ",
                                                        Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]).PreventSQLInjection(),
                                                        rooomkey.PreventSQLInjection());
                            DataRow drLot = db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0].Rows[0];
                            enterpriseKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                            routeKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                            stepKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                            string lotkey=Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                            string qty = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_QUANTITY]);

                           DataSet dsStep = GeNextRouteAndStep(db, dbTran, enterpriseKey, routeKey, stepKey);
                            _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsStep);
                            if (!string.IsNullOrEmpty(_errorMsg))
                                break;
                            
                        }
                        #endregion

                        _errorMsg = SaveConfigmentDataBySaveType(db, dbTran,
                                        savetype, _creator, palletNo,palletkey, rooomkey,
                                        operationName,opr_computer, dt_por_lot_update,shiftKey,shiftValue,equipmentkey);
                    }
                    //出托过账
                    else if (savetype.Equals("out"))
                    {
                        #region 更新批次表
                        foreach (DataRow dr in dt_por_lot_update.Rows)
                        {
                            //更新批次信息
                            sqlCommand = string.Format(@"update POR_LOT set LINE_NAME='{0}',CUR_PRODUCTION_LINE_KEY='{1}',EDIT_TIME=GETDATE(),EDITOR='{2}'
                                                        where LOT_NUMBER='{3}' and FACTORYROOM_KEY='{4}'",
                                                        linename.PreventSQLInjection(),
                                                        linekey.PreventSQLInjection(),
                                                        _creator.PreventSQLInjection(),
                                                       Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]),
                                                       rooomkey);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                        #endregion

                        _errorMsg = SaveConfigmentDataBySaveType(db, dbTran,
                                       savetype, _creator, palletNo,palletkey, rooomkey,
                                       operationName, opr_computer, dt_por_lot_update, shiftKey, shiftValue, equipmentkey);
                    }
                    //保存不过帐
                    else if (savetype.Equals("onlysave"))
                    {
                        foreach (DataRow dr in dt_por_lot_update.Rows)
                        {
                            sqlCommand = string.Format(@"update POR_LOT 
                                                        set LINE_NAME='{0}',PALLET_NO='{5}',PALLET_TIME='{6}', CUR_PRODUCTION_LINE_KEY='{1}',EDIT_TIME=GETDATE(),EDITOR='{2}'
                                                        where LOT_NUMBER='{3}' and FACTORYROOM_KEY='{4}'",
                                                        linename.PreventSQLInjection(),
                                                        linekey.PreventSQLInjection(),
                                                        _creator.PreventSQLInjection(),
                                                        Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]),
                                                        rooomkey,
                                                        Convert.ToString(dr[POR_LOT_FIELDS.FIELD_PALLET_NO]),
                                                        Convert.ToString(dr[POR_LOT_FIELDS.FIELD_PALLET_TIME]));

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                        }
                    }
                    else
                    {
                        sqlCommand = string.Format(@"  DELETE FROM WIP_CONSIGNMENT_TEMP WHERE VIRTUAL_PALLET_NO='{0}'", palletno);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                        if (dt_por_lot_update != null && dt_por_lot_update.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_por_lot_update.Rows)
                            {
                                Hashtable hashTable = new Hashtable();
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CONSIGNMENT_KEY] = palletkey;
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_PALLET_NO] = palletno;
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CONSIGNMENT_KEY_TEMP] = CommonUtils.GenerateNewKey(0);
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_LOT_NUMBER] = dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER];
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CREATE_TIME] = "";
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CREATER] = _creator;
                                hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_ROOM_KEY] = rooomkey;

                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(consigment_temp_fields, hashTable, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                    }

                    //更新包装表的总功率
                    UpdateConsigmentTotlePower(db, dbTran, palletNo, rooomkey);


                    if (!string.IsNullOrEmpty(_errorMsg))
                    {
                        dbTran.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, _errorMsg);
                    }
                    else
                    {
                        //Commit Transaction
                        dbTran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SavePalletLotData Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }


            //返回结果集。
            return dsReturn;
        }

        private string SaveConfigmentDataBySaveType(Database db, DbTransaction dbTran,
            string saveType, string _creator, string palletNo,string palletkey, string rooomkey,
            string operationName, string opr_computer, DataTable dt_por_lot, string shiftKey, string shiftValue, string equipmentkey)
        {
            string _errorMsg = string.Empty;
            #region
            foreach (DataRow dr in dt_por_lot.Rows)
            {
                //DataSet dsParams = new DataSet();
                string s_lot = dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
                string pallet_time = string.Empty;
                string s_palletno = string.Empty;
                if (dt_por_lot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO))
                    s_palletno = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO].ToString();
                else if (dt_por_lot.Columns.Contains(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                    s_palletno = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
                if (dt_por_lot.Columns.Contains(POR_LOT_FIELDS.FIELD_PALLET_TIME))
                    pallet_time = dr[POR_LOT_FIELDS.FIELD_PALLET_TIME].ToString();

                //获得批次信息
                DataSet _dsLotInfo = GetLotInfoForConSigment(db, dbTran, s_lot);
                DataRow drLotInfo = _dsLotInfo.Tables[0].Rows[0];
                string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                string lineKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                string lineName = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                string workOrderKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                //string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                string equipmentKey = equipmentkey;
                string edcInsKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
                string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                int qty = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                int leftqty = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                string lot_sidecode = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_SIDECODE]);
                string lot_custcode = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]);

                DataSet dsStep = new DataSet();
                string toEnterpriseKey = string.Empty;
                string toRouteKey = string.Empty;
                string toStepKey = string.Empty;
                string toEnterpriseName = string.Empty;
                string toRouteName = string.Empty;
                string toStepName = string.Empty;
                string sql = string.Empty;
                string sqlCommand = string.Empty;
                #region
                //出站
                if (saveType.Equals("real"))
                {
                    #region
                    string sqlLotEquipment = string.Format(@"INSERT INTO EMS_LOT_EQUIPMENT
                                                    (LOT_EQUIPMENT_KEY,LOT_KEY,OPERATION_KEY,EQUIPMENT_KEY,START_TIMESTAMP,END_TIMESTAMP,
                                                     USER_KEY,QUANTITY,STEP_KEY)
                                                    VALUES('{6}','{0}','{1}','{2}',GETDATE(),null,'{3}','{4}','{5}')",
                                                       lotKey.PreventSQLInjection(),
                                                       routeKey.PreventSQLInjection(),
                                                       equipmentkey.PreventSQLInjection(),
                                                       _creator.PreventSQLInjection(),
                                                       qty,
                                                       stepKey.PreventSQLInjection(),
                                                      UtilHelper.GenerateNewKey(0));
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlLotEquipment);
                    #endregion

                    #region
                    //获得下一工步
                    dsStep = GeNextRouteAndStep(db, dbTran, enterpriseKey, routeKey, stepKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsStep);
                    if (!string.IsNullOrEmpty(_errorMsg))
                        break;

                    DataRow drRouteNextStep = dsStep.Tables[0].Rows[0];
                    toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                    toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                    toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                    toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                    //判断下一工步的合法性

                    //下一工艺流程步骤为空
                    if (string.IsNullOrEmpty(toEnterpriseKey) || string.IsNullOrEmpty(toRouteKey) || string.IsNullOrEmpty(toStepKey))
                    {
                        _errorMsg = "获取下一工艺步骤失败，请重试操作。";
                        break;
                    }
                    //根据当前工艺流程和下一个工艺流程是否匹配
                    //工艺流程组不匹配
                    if (!toEnterpriseKey.Equals(enterpriseKey))
                    {
                        _errorMsg = "工艺流程组不匹配，请重试操作。";
                        break;
                    }

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
                            _errorMsg = "工艺流程不匹配，请重试操作。";
                            break;
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
                    bool isFinish = maxRowNumber == curRowNumber;  //当前工步是最后一个工步。
                    //如果是最后一站。但是两个工步的序号差不等于0
                    if (isFinish && diff != 0)
                    {
                        _errorMsg = "获取下一工步失败(最后一站但两个工步的序号差不等于0)，请重试操作。";
                        break;
                    }
                    //如果是最后一站。但是下一个工步的序号不等于最大工步序号。
                    else if (isFinish && maxRowNumber != nxtRowNumber)
                    {
                        _errorMsg = "获取下一工步失败（最后一站但下一个工步的序号不等于最大工步序号），请重试操作。";
                        break;
                    }
                    //如果不是最后一站。但是两个工步的序号差不等于1
                    else if (!isFinish && diff != 1)
                    {
                        _errorMsg = "获取下一工步失败（两个工步的序号差不等于1），请重试操作。";
                        break;
                    }
                    //更新批次信息。
                    StringBuilder sbUpdateLot = new StringBuilder();
                    sbUpdateLot.AppendFormat(@"UPDATE POR_LOT 
                                              SET EDITOR='{0}',EDIT_TIME=GETDATE(),OPERATOR='{1}',OPR_LINE='{2}', OPR_COMPUTER='{3}'",
                                               _creator.PreventSQLInjection(),
                                               _creator.PreventSQLInjection(),
                                               lineName.PreventSQLInjection(),"");
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
                        if (leftqty == 0)
                        {
                            sbUpdateLot.AppendFormat(@",DELETED_TERM_FLAG=1");
                        }
                    }
                    else
                    {
                        //如果是最后一个工步，需要更新更新状态为完成，更新结束状态为已结束。
                        sbUpdateLot.AppendFormat(@",STATE_FLAG=10,DELETED_TERM_FLAG=1");
                    }
                    //入托作业
                    if (!string.IsNullOrEmpty(pallet_time))
                        sbUpdateLot.AppendFormat(@",PALLET_NO='{0}' ,PALLET_TIME=CONVERT(DATETIME,'{1}')", palletNo, pallet_time);
                    else
                        sbUpdateLot.AppendFormat(@",PALLET_NO='{0}' ,PALLET_TIME=GETDATE()", palletNo);


                    sbUpdateLot.AppendFormat(@" WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateLot.ToString());

                    //更新WIP_JOB信息。更新自动出站JOB
                    sqlCommand = string.Format(@"UPDATE WIP_JOB 
                                                SET JOB_STATUS=1 
                                                WHERE LOT_KEY='{0}' AND ENTERPRISE_KEY='{1}' 
                                                AND ROUTE_KEY='{2}' AND STEP_KEY='{3}'",
                                                lotKey.PreventSQLInjection(),
                                                enterpriseKey.PreventSQLInjection(),
                                                routeKey.PreventSQLInjection(),
                                                stepKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    #endregion
                }
                //整托出托
                #region
                else if (saveType.Equals("out"))
                {
                    //获得包装工步
                    dsStep = GetSigmentStep(db, dbTran, enterpriseKey, operationName);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsStep);
                    if (!string.IsNullOrEmpty(_errorMsg))
                        break;

                    DataRow drRouteNextStep = dsStep.Tables[0].Rows[0];
                    toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                    toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                    toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                    toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                    //判断下一工步的合法性
                    #region
                    //包装工艺流程步骤为空
                    if (string.IsNullOrEmpty(toEnterpriseKey) || string.IsNullOrEmpty(toRouteKey) || string.IsNullOrEmpty(toStepKey))
                    {
                        _errorMsg = string.Format("获取{0}工艺步骤失败，请重试操作。", operationName);
                        break;
                    }
                    //根据当前工艺流程和下一个工艺流程是否匹配
                    //工艺流程组不匹配
                    if (!toEnterpriseKey.Equals(enterpriseKey))
                    {
                        _errorMsg = "工艺流程组不匹配，请重试操作。";
                        break;
                    }

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
                            _errorMsg = "工艺流程不匹配，请重试操作。";
                            break;
                        }
                    }

                    #region 更新批次信息
                    //更新批次信息。
                    StringBuilder sbUpdateLot = new StringBuilder();
                    sbUpdateLot.AppendFormat(@"UPDATE POR_LOT 
                                       SET EDITOR='{0}',EDIT_TIME=GETDATE(),
                                           OPERATOR='{1}',OPR_LINE='{2}', OPR_COMPUTER='{3}'",
                                               _creator.PreventSQLInjection(),
                                               _creator.PreventSQLInjection(),
                                               lineName.PreventSQLInjection(),
                                           "");

                        sbUpdateLot.AppendFormat(@",CUR_ROUTE_VER_KEY='{0}',
                                                CUR_STEP_VER_KEY='{1}',
                                                ROUTE_ENTERPRISE_VER_KEY='{2}',
                                                START_WAIT_TIME=GETDATE(),
                                                STATE_FLAG=0",
                                                    toRouteKey.PreventSQLInjection(),
                                                    toStepKey.PreventSQLInjection(),
                                                    toEnterpriseKey.PreventSQLInjection());
                        //出账后数量为0，则结束批次。
                        if (leftqty == 0)
                        {
                            sbUpdateLot.AppendFormat(@",DELETED_TERM_FLAG=1");
                        }
                    
                    

                    //出托作业
                    sbUpdateLot.AppendFormat(@",PALLET_NO='' ,PALLET_TIME=GETDATE()");
                    sbUpdateLot.AppendFormat(@" WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sbUpdateLot.ToString());

                    #endregion

                    #region

                    //包装出托作业历史记录
                    WIP_CONSIGNMENT_TEMP_FIELDS consigment_temp_fields = new WIP_CONSIGNMENT_TEMP_FIELDS();
                    Hashtable hashTable = new Hashtable();
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CONSIGNMENT_KEY] = palletkey;
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_PALLET_NO] = s_palletno;
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CONSIGNMENT_KEY_TEMP] = CommonUtils.GenerateNewKey(0);
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_LOT_NUMBER] = dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER];
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CREATE_TIME] = "";
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_CREATER] = _creator;
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_PALLET_TYPE] = 3;//出托作业
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_LOT_SIDECODE] = lot_sidecode;
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_LOT_CUSTOMERCODE] = lot_custcode;
                    hashTable[WIP_CONSIGNMENT_TEMP_FIELDS.FIELDS_ROOM_KEY] = rooomkey;

                    sqlCommand = DatabaseTable.BuildInsertSqlStatement(consigment_temp_fields, hashTable, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                    #endregion

                    #endregion
                }
                #endregion
               
                //设备主键不为空，对设备进行操作。
                if (!string.IsNullOrEmpty(equipmentKey) && saveType.Equals("real"))
                {
                    WipManagement.TrackOutForEquipment(db,dbTran,lotKey, stepKey, equipmentKey, _creator);
                }


                #endregion
                //保存出站记录的数据
                Hashtable htStepTransaction = new Hashtable();
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, _creator);
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
                //dsParams.Tables.Add(dtStepTransaction);

                //组织操作数据。
                #region
                Hashtable htTransaction_TrackIn = new Hashtable();
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);              
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftqty);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftValue);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, _creator);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, opr_computer);

                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, lineName);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);

                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                //htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, _creator);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                //htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                if (saveType.Equals("out"))
                    htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PALLETOUT);
                else
                    htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction_TrackIn);
                dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                //dsParams.Tables.Add(dtTransaction);

                //向WIP_TRANSACTION表插入批次进站的操作记录。
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                string transactionKey = UtilHelper.GenerateNewKey(0);
                if (!htTransaction_TrackIn.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
                {
                    htTransaction_TrackIn.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                }
                htTransaction_TrackIn[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey;
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction_TrackIn, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                Hashtable htWip_Lot_TrackIn = new Hashtable();
                DataRow drPorLot=_dsLotInfo.Tables[0].Rows[0];
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CHILD_LINE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CHILD_LINE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CREATE_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_TIME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CREATE_TIMEZONE_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_TIMEZONE_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CREATE_TYPE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_TYPE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CREATOR, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATOR]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_DESCRIPTIONS, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_DESCRIPTIONS]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_EDC_INS_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_EDIT_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_EDIT_TIMEZONE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_EDITOR, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDITOR]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_EFFICIENCY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EFFICIENCY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_FACTORYROOM_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_FACTORYROOM_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_HOLD_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_HOLD_FLAG]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_IS_MAIN_LOT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_MAIN_LOT]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_IS_PRINT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_PRINT]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_IS_REWORKED, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_REWORKED]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_IS_SPLITED, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_SPLITED]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_LINE_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LINE_NAME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_LOT_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_LOT_NUMBER, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_LOT_SEQ, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_SEQ]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_LOT_TYPE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_TYPE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_MATERIAL_CODE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_MATERIAL_CODE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_MATERIAL_LOT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_MATERIAL_LOT]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_OPERATOR, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPERATOR]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_OPR_COMPUTER, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_OPR_LINE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPR_LINE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_OPR_LINE_PRE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_PART_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PART_VER_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_PART_NUMBER, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PART_NUMBER]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_PRIORITY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PRIORITY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_PRO_ID, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PRO_ID]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_QUANTITY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_QUANTITY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_QUANTITY_INITIAL, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_SHIFT_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SHIFT_NAME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_SHIPPED_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SHIPPED_FLAG]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_SI_LOT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SI_LOT]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_START_PROCESS_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_START_WAIT_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_START_WAIT_TIME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_STATE_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_STATE_FLAG]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_STATUS, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_STATUS]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_SUPPLIER_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SUPPLIER_NAME]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_WORK_ORDER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_WORK_ORDER_NO, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_WORK_ORDER_SEQ, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ]));
                htWip_Lot_TrackIn.Add(WIP_LOT_FIELDS.FIELD_PRO_LEVEL, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PRO_LEVEL]));

                //向WIP_LOT表插入批次进站的操作记录。
                WIP_LOT_FIELDS wipLotFields = new WIP_LOT_FIELDS();

                sql = DatabaseTable.BuildInsertSqlStatement(wipLotFields, htWip_Lot_TrackIn, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                #endregion

                //---------------------------------------------------------------------------------------------------------------------
                if (saveType.Equals("real"))
                {
                    #region
                    Hashtable htTransaction_TrackOut = new Hashtable();
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftqty);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftValue);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, _creator);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, opr_computer);

                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, lineName);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);

                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                    //htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, _creator);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                    //htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                    htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                    DataTable dtTransactionOut = CommonUtils.ParseToDataTable(htTransaction_TrackIn);
                    dtTransactionOut.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                    //dsParams.Tables.Add(dtTransactionOut);

                    //向WIP_TRANSACTION表插入批次出站的操作记录。
                    WIP_TRANSACTION_FIELDS wipFields_out = new WIP_TRANSACTION_FIELDS();
                    string transactionKey_out = UtilHelper.GenerateNewKey(0);
                    if (!htTransaction_TrackOut.ContainsKey(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY))
                    {
                        htTransaction_TrackOut.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey_out);
                    }
                    htTransaction_TrackOut[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey_out;
                    sql = DatabaseTable.BuildInsertSqlStatement(wipFields_out, htTransaction_TrackOut, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    Hashtable htWip_Lot_TrackOut = new Hashtable();
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CHILD_LINE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CHILD_LINE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CREATE_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_TIME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CREATE_TIMEZONE_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_TIMEZONE_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CREATE_TYPE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATE_TYPE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CREATOR, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CREATOR]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_DESCRIPTIONS, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_DESCRIPTIONS]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_EDC_INS_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_EDIT_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_EDIT_TIMEZONE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_EDITOR, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EDITOR]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_EFFICIENCY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_EFFICIENCY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_FACTORYROOM_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_FACTORYROOM_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_HOLD_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_HOLD_FLAG]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_IS_MAIN_LOT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_MAIN_LOT]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_IS_PRINT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_PRINT]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_IS_REWORKED, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_REWORKED]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_IS_SPLITED, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_IS_SPLITED]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_LINE_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LINE_NAME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_LOT_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_LOT_NUMBER, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_LOT_SEQ, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_SEQ]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_LOT_TYPE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_LOT_TYPE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_MATERIAL_CODE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_MATERIAL_CODE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_MATERIAL_LOT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_MATERIAL_LOT]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_OPERATOR, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPERATOR]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_OPR_COMPUTER, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_OPR_LINE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPR_LINE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_OPR_LINE_PRE, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_PART_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PART_VER_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_PART_NUMBER, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PART_NUMBER]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_PRIORITY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PRIORITY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_PRO_ID, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PRO_ID]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_QUANTITY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_QUANTITY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_QUANTITY_INITIAL, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_SHIFT_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SHIFT_NAME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_SHIPPED_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SHIPPED_FLAG]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_SI_LOT, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SI_LOT]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_START_PROCESS_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_START_WAIT_TIME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_START_WAIT_TIME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_STATE_FLAG, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_STATE_FLAG]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_STATUS, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_STATUS]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_SUPPLIER_NAME, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_SUPPLIER_NAME]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_TRANSACTION_KEY, transactionKey_out);
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_WORK_ORDER_KEY, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_WORK_ORDER_NO, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_WORK_ORDER_SEQ, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ]));
                    htWip_Lot_TrackOut.Add(WIP_LOT_FIELDS.FIELD_PRO_LEVEL, Convert.ToString(drPorLot[POR_LOT_FIELDS.FIELD_PRO_LEVEL]));

                    //向WIP_LOT表插入批次出站的操作记录。
                    wipLotFields = new WIP_LOT_FIELDS();

                    sql = DatabaseTable.BuildInsertSqlStatement(wipLotFields, htWip_Lot_TrackOut, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //------------------------------------------------------------------------------------------------------------
                    #endregion

                    //向WIP_STEP_TRANSACTION表插入批次出站的操作记录。
                    WIP_STEP_TRANSACTION_FIELDS wipStepTransFields = new WIP_STEP_TRANSACTION_FIELDS();
                    htStepTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transactionKey_out;
                    sql = DatabaseTable.BuildInsertSqlStatement(wipStepTransFields, htStepTransaction, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
            }
            #endregion


            return _errorMsg;
        }
        /// <summary>
        /// 更新包装表的总功率
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbTran"></param>
        /// <param name="palletNo"></param>
        /// <param name="Roomkey"></param>
        private void UpdateConsigmentTotlePower(Database db, DbTransaction dbTran, string palletNo, string Roomkey)
        {
            string sqlCommand = string.Format(@"UPDATE a
                                                SET a.LOT_NUMBER_QTY=(SELECT COUNT(1) FROM POR_LOT aa WHERE aa.PALLET_NO=a.VIRTUAL_PALLET_NO),
                                                    a.TOTLE_POWER=(SELECT ISNULL(SUM(bb.COEF_PMAX),0)
                                                                  FROM POR_LOT aa
                                                                  INNER JOIN WIP_IV_TEST bb ON bb.LOT_NUM=aa.LOT_NUMBER AND bb.VC_DEFAULT='1'
                                                                  WHERE aa.PALLET_NO=a.VIRTUAL_PALLET_NO
                                                                  AND aa.STATUS<2)
                                                FROM WIP_CONSIGNMENT a
                                                WHERE a.VIRTUAL_PALLET_NO='{0}' AND a.ISFLAG='1'", 
                                                palletNo.PreventSQLInjection());
            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
            sqlCommand = string.Format(@"UPDATE a
                                        SET    a.AVG_POWER=CASE WHEN a.LOT_NUMBER_QTY=0 THEN 0 ELSE a.TOTLE_POWER/a.LOT_NUMBER_QTY END
                                        FROM WIP_CONSIGNMENT a
                                        WHERE a.VIRTUAL_PALLET_NO='{0}' AND a.ISFLAG='1'",
                                        palletNo.PreventSQLInjection());
            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
        }
    

        private bool IsUpdatePorLot(Database db, DbTransaction dbTran, string lotnum, string palletno)
        {
            string sqlCommand = string.Format(@" select t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,
                                                t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,
                                                t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ from POR_LOT t 
                                                     where t.LOT_NUMBER='{0}' and
                                                      ISNULL( t.PALLET_NO,'0')<>'{1}'", lotnum, palletno);
            DataTable dt = db.ExecuteDataSet(dbTran,CommandType.Text, sqlCommand).Tables[0];
            if (dt.Rows.Count > 0)
                return true;

            return false;
        }

        public DataSet GetLotInfoForConSigment( Database db, DbTransaction dbTran,string lotNo)
        {
            DataSet dsReturn = new DataSet();
            string sql = @"SELECT TOP 1 A.[LOT_KEY],A.[LOT_NUMBER],A.[WORK_ORDER_KEY],A.[WORK_ORDER_NO],A.[WORK_ORDER_SEQ]
                            ,A.[PART_VER_KEY],A.[PART_NUMBER],A.[PRO_ID],A.[PRO_LEVEL],A.[PRIORITY],A.[QUANTITY_INITIAL]
                            ,A.[QUANTITY],A.[ROUTE_ENTERPRISE_VER_KEY],A.[CUR_ROUTE_VER_KEY],A.[CUR_STEP_VER_KEY]
                            ,A.[CUR_PRODUCTION_LINE_KEY],A.[LINE_NAME],A.[START_WAIT_TIME],A.[START_PROCESS_TIME]
                            ,A.[EDC_INS_KEY],A.[STATE_FLAG],A.[IS_MAIN_LOT],A.[SPLIT_FLAG],A.[LOT_SEQ],A.[REWORK_FLAG]
                            ,A.[HOLD_FLAG],A.[SHIPPED_FLAG],A.[DELETED_TERM_FLAG],A.[IS_PRINT],A.[LOT_TYPE],A.[CREATE_TYPE]
                            ,A.[COLOR],A.[PALLET_NO],A.[PALLET_TIME],A.[STATUS],A.[OPERATOR],A.[OPR_LINE],A.[OPR_COMPUTER]
                            ,A.[OPR_LINE_PRE],A.[CHILD_LINE],A.[MATERIAL_CODE],A.[MATERIAL_LOT],A.[SUPPLIER_NAME],A.[SI_LOT]
                            ,A.[EFFICIENCY],A.[FACTORYROOM_KEY],A.[FACTORYROOM_NAME],A.[CREATE_OPERTION_NAME],A.[CREATOR]
                            ,A.[CREATE_TIME],A.[CREATE_TIMEZONE_KEY],A.[EDITOR],A.[EDIT_TIME],A.[EDIT_TIMEZONE],A.[SHIFT_NAME]
                            ,A.[DESCRIPTIONS],A.[LOT_SIDECODE],A.[LOT_CUSTOMERCODE],
                            B.ENTERPRISE_NAME,
                            B.ENTERPRISE_VERSION,
                            C.ROUTE_NAME ,
                            D.ROUTE_STEP_NAME,
                            F.EQUIPMENT_KEY
                        FROM POR_LOT A
                        LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                        LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                        LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                        LEFT JOIN (SELECT aa.LOT_KEY,aa.STEP_KEY,aa.EQUIPMENT_KEY,aa.START_TIMESTAMP,aa.END_TIMESTAMP,bb.EQUIPMENT_NAME
                                   FROM EMS_LOT_EQUIPMENT aa
                                   INNER JOIN EMS_EQUIPMENTS bb ON aa.EQUIPMENT_KEY=bb.EQUIPMENT_KEY
                                   WHERE aa.END_TIMESTAMP IS NULL) F ON F.LOT_KEY=A.LOT_KEY AND F.STEP_KEY=A.CUR_STEP_VER_KEY
                        WHERE A.STATUS<> 2 
                        AND (A.LOT_NUMBER='{0}' or A.LOT_CUSTOMERCODE='{0}'
                            or A.LOT_SIDECODE='{0}' OR A.LOT_KEY='{0}')
                        ORDER BY F.START_TIMESTAMP DESC";
            try
            {
                dsReturn = db.ExecuteDataSet(dbTran, CommandType.Text, string.Format(sql, lotNo.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotInfo Error: " + ex.Message);
            }
            //返回结果集。
            return dsReturn;
        }
       

        private DataSet GeNextRouteAndStep(Database db, DbTransaction dbTran,string enterpriseKey, string routeKey, string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (string.IsNullOrEmpty(enterpriseKey) || string.IsNullOrEmpty(routeKey) || string.IsNullOrEmpty(stepKey))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数为空，请检查。");
                    LogService.LogError(
                                        string.Format(
                                        "GetEnterpriseNextRouteAndStep Error: 传入参数为空，请检查。EnterpriseKey:{0};RouteKey:{1};StepKey:{2}。",
                                        enterpriseKey, routeKey, stepKey)
                                       );
                    return dsReturn;
                }

                string sql = string.Format(@"WITH ROUTE AS
                                            (
                                                SELECT ROW_NUMBER() OVER (ORDER BY ROUTE_SEQ,ROUTE_STEP_SEQ) AS ROWNUMBER,
                                                        ROUTE_ENTERPRISE_VER_KEY,
                                                        ROUTE_ROUTE_VER_KEY,
                                                        ROUTE_STEP_KEY,
                                                        ENTERPRISE_NAME,
                                                        ROUTE_NAME,
                                                        ROUTE_STEP_NAME
                                                FROM V_PROCESS_PLAN 
                                                WHERE ROUTE_ENTERPRISE_VER_KEY='{0}'
                                            )
                                            SELECT ROUTE_ENTERPRISE_VER_KEY,ENTERPRISE_NAME,
	                                               ROUTE_ROUTE_VER_KEY,ROUTE_NAME,
	                                               ROUTE_STEP_KEY,ROUTE_STEP_NAME  
                                            FROM ROUTE a WHERE a.ROWNUMBER=1+(SELECT b.ROWNUMBER 
                                                                              FROM ROUTE b 
                                                                              WHERE b.ROUTE_ROUTE_VER_KEY='{1}'
                                                                              AND b.ROUTE_STEP_KEY='{2}')",
                                            enterpriseKey.PreventSQLInjection(),
                                            routeKey.PreventSQLInjection(),
                                            stepKey.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(dbTran, CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GeNextRouteAndStep Error: " + ex.Message);
            }
            return dsReturn;
        }

        private DataSet GetSigmentStep(Database db, DbTransaction dbTran,string enterpriseKey)
        {
            return GetSigmentStep(db, dbTran, enterpriseKey, "");
        }
        private DataSet GetSigmentStep(Database db, DbTransaction dbTran,string enterpriseKey, string operationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (string.IsNullOrEmpty(enterpriseKey))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数为空，请检查。");
                    LogService.LogError(string.Format("GetEnterpriseNextRouteAndStep Error: 传入参数为空，请检查。EnterpriseKey:{0}。", enterpriseKey));
                    return dsReturn;
                }
                string sql = string.Empty;
                if (!string.IsNullOrEmpty(operationName))
                    sql = string.Format(@"select * from V_PROCESS_PLAN where ROUTE_ENTERPRISE_VER_KEY='{0}' and ROUTE_STEP_NAME='{1}' ",
                        enterpriseKey.PreventSQLInjection(),
                        operationName.PreventSQLInjection());
                else
                    sql = string.Format(@"select * from V_PROCESS_PLAN where ROUTE_ENTERPRISE_VER_KEY='{0}' and ROUTE_STEP_NAME='包装' ", enterpriseKey.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSigmentStep Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 单笔出托作业
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet UpdateLotOutPallet(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string s_lot = string.Empty;
            string roomkey = string.Empty;
            string pallet = string.Empty;
            if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                s_lot = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY))
                roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                pallet = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    dsReturn = GetWOProductByLotNum(s_lot, roomkey);


                    string sqlCommand = string.Format(@" select t.CUR_PRODUCTION_LINE_KEY,t.CUR_ROUTE_VER_KEY,t.CUR_STEP_VER_KEY,t.DELETED_TERM_FLAG,t.DESCRIPTIONS,t.EFFICIENCY,t.FACTORYROOM_KEY,
                                                    t.FACTORYROOM_NAME,t.HOLD_FLAG,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.LOT_NUMBER,t.LOT_SIDECODE,t.LOT_TYPE,t.MATERIAL_CODE,t.MATERIAL_LOT,t.OPERATOR,
                                                    t.OPR_COMPUTER,t.OPR_LINE,t.PALLET_NO,t.PALLET_TIME,t.PART_NUMBER,t.PART_VER_KEY,t.PRO_ID,t.PRO_LEVEL,t.QUANTITY,t.QUANTITY_INITIAL,t.REWORK_FLAG,
                                                    t.SHIPPED_FLAG,t.SI_LOT,t.SPLIT_FLAG,t.SUPPLIER_NAME,t.WORK_ORDER_KEY,t.WORK_ORDER_NO,t.WORK_ORDER_SEQ from POR_LOT t 
                                                 where t.LOT_NUMBER='{0}' and t.FACTORYROOM_KEY='{1}' 
                                                 and t.PALLET_NO='{2}' and t.DELETED_TERM_FLAG<2", s_lot, roomkey, pallet);
                    DataTable dtLot = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtLot.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;

                    if (dtLot.Rows.Count > 0)
                    {
                        DataRow dr = dtLot.Rows[0];
                        sqlCommand = string.Format(@"update POR_LOT set PALLET_NO=NULL,PALLET_TIME=null WHERE LOT_NUMBER='{0}' AND FACTORYROOM_KEY='{1}' AND DELETED_TERM_FLAG<2", s_lot, roomkey);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
          
                        sqlCommand = string.Format(@"update WIP_CONSIGNMENT_TEMP set PALLET_TYPE=3 where LOT_NUMBER='{0}' and ROOM_KEY='{1}'", s_lot, roomkey);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    }

                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("UpdateLotOutPallet Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        public DataSet IsExistPalletLotNum(Hashtable hsParams)
        {
            DataSet dsReturn = new DataSet();          
            string flag = Convert.ToString(hsParams["flag"]);           
            string sqlCommand = string.Empty;
            if (flag.Equals("pallet"))
            {
                sqlCommand = @"select t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                            t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                            t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t.PALLET_NO_NEW,
                            t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                            t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER  from WIP_CONSIGNMENT t where t.ISFLAG=1 and t.CS_DATA_GROUP=1";
                if (hsParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO))
                    sqlCommand += string.Format(" and t.PALLET_NO='{0}'", Convert.ToString(hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]));

                DataTable dt01 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dt01.Rows.Count > 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format(@"【托号{0}】已经存在",
                        Convert.ToString(hsParams[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO])));
                }


            }
            else if (flag.Equals("lot"))
            {
                sqlCommand = @"select t.PALLET_NO,t1.LOT_NUMBER from WIP_CONSIGNMENT t inner join POR_LOT t1 on t.VIRTUAL_PALLET_NO=t1.PALLET_NO
                                                    and t.ISFLAG=1 and t1.DELETED_TERM_FLAG<2  and t.CS_DATA_GROUP<>'0' ";
                if(hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                {
                    sqlCommand += string.Format(@" and (t1.LOT_NUMBER='{0}' or t1.LOT_CUSTOMERCODE='{0}' or t1.LOT_SIDECODE='{0}')",
                        Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]));
                }

                DataTable dt01 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                if (dt01.Rows.Count > 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format(@"批号【{0}】在托号【{1}】中存在，请确认",
                                                               Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER])
                                                               , dt01.Rows[0][WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString()));
                }
            }
            else if (flag.Equals("lotcustsidecode"))
            {//根据侧板标签或者客户标签获取批次号。
                sqlCommand = string.Format(@"SELECT t.LOT_NUMBER FROM POR_LOT t WHERE t.DELETED_TERM_FLAG<2 ");
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE))
                {
                    sqlCommand += string.Format(" AND  t.LOT_CUSTOMERCODE='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]));
                }
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_SIDECODE))
                {
                    sqlCommand += string.Format(" AND  t.LOT_SIDECODE='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_LOT_SIDECODE]));
                }
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                {
                    sqlCommand += string.Format(" AND  t.FACTORYROOM_KEY='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]));
                }
                dsReturn= db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            return dsReturn;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetPalletCustLotData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string s_lot = string.Empty;
            string roomkey = string.Empty;
            string s_palletNo = string.Empty;
            string s_wo = string.Empty;
            string s_date = string.Empty;
            string e_date = string.Empty;
            string s_custno = string.Empty;
            string cust_type = string.Empty;
            if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                s_lot = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY))
                roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                s_palletNo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME))
                s_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME))
                e_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER))
                s_wo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE))
                cust_type = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE].ToString();

            string sqlCommand = string.Empty;

            try
            {
                sqlCommand = string.Format(@"select t.AVG_POWER,t.AVG_POWER_RANGE,t.CHECKER,t.CHECK_TIME,
                                            t.CODE_TYPE,t.CONSIGNMENT_KEY,t.CS_DATA_GROUP,t.CUSTOMER_NO,t.EQUIPMENT_KEY,t.EQUIPMENT_NAME,
                                            t.GRADE,t.LAST_PALLET,t.LINE_KEY,t.LINE_NAME,t.LOT_COLOR,t.LOT_NUMBER_QTY,t.PALLET_NO_NEW,t1.PALLET_TIME,
                                            t.PALLET_TYPE,t.POWER_LEVEL,t.PRO_ID,t.PS_CODE,t.PS_DTL_SUBCODE,t.PS_SEQ,t.ROOM_KEY,t.SAP_NO,
                                            t.SHIFT,t.TOTLE_POWER,t.VIRTUAL_PALLET_NO,t.WORKNUMBER ,t1.LOT_CUSTOMERCODE,t1.LOT_NUMBER,t2.LOT_COLOR,t2.PRO_LEVEL
                                            from WIP_CONSIGNMENT t left join POR_LOT t1 on t.PALLET_NO=t1.PALLET_NO
                                            inner join WIP_CUSTCHECK t2 on t1.LOT_NUMBER=t2.CC_FCODE1
                                            where t.ISFLAG=1 ");

                if (!string.IsNullOrEmpty(s_palletNo))
                    sqlCommand += string.Format(" and t." + WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "='{0}'", s_palletNo);
                if (!string.IsNullOrEmpty(roomkey))
                    sqlCommand += string.Format(" and t." + WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY + "='{0}'", roomkey);              

                DataTable dtToWarehouseCheck = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtToWarehouseCheck.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtToWarehouseCheck, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletCustLotData Error: " + ex.Message);
            }
            return dsReturn;
        }
        //-----------------------------------------------------------------------------------------
        public DataSet GetToWarehouseCheckData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();           
            string s_lot = string.Empty;
            string roomkey = string.Empty;
            string s_palletNo = string.Empty;
            string s_wo = string.Empty;
            string s_date = string.Empty;
            string e_date = string.Empty;
            string s_custno = string.Empty;
            string cust_type = string.Empty;
            string flag = string.Empty;
            if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                s_lot = hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();          
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY))
                roomkey = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO))
                s_palletNo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME))
                s_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME))
                e_date = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER))
                s_wo = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER].ToString();
            if (hstable.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE))
                cust_type = hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_CODE_TYPE].ToString();
            if (hstable.ContainsKey("flag"))
                flag = hstable["flag"].ToString();
            string sqlCommand = string.Empty;


            try
            {

                if (flag.Equals("query"))
                    sqlCommand = "select ROW_NUMBER() over(order by t3.CHECK_TIME asc) SEQ, ";
                else
                    sqlCommand = "select ROW_NUMBER() over(order by t3.EDIT_TIME asc) SEQ, ";

                sqlCommand += string.Format(@" t.LOT_NUMBER,t2.EDIT_TIME,
                                                t1.DEVICENUM,t1.VC_DEFAULT,t.WORK_ORDER_NO,t.PRO_ID,
                                                t3.PALLET_NO,t.LOT_CUSTOMERCODE,t2.LOT_COLOR, t3.CHECK_TIME ,t4.USERNAME CHECKER,
                                                t1.T_DATE,t2.OPERATERS,t.PRO_LEVEL,t1.COEF_PMAX,t1.COEF_FF,t1.COEF_IMAX,t1.COEF_ISC,t1.COEF_VMAX,t1.COEF_VOC
                                                from POR_LOT t left join WIP_IV_TEST t1  on t.LOT_NUMBER=t1.LOT_NUM and t1.VC_DEFAULT='1'
                                                left join(select  A.EDIT_TIME,A.LOT_COLOR,A.OPERATERS,A.CC_FCODE1,CREATE_TIME 
                                                from  WIP_CUSTCHECK a  where a.ISFLAG=1 and a.CC_DATA_GROUP='1' )t2 on t.LOT_NUMBER=t2.CC_FCODE1 
                                                inner join ( select EDIT_TIME,PALLET_NO,CHECK_TIME,CHECKER,PALLET_NO_NEW,VIRTUAL_PALLET_NO,CREATE_TIME,
                                                CREATER from WIP_CONSIGNMENT where ISFLAG=1 ");
                if (flag == "query")
                    sqlCommand += "  and CS_DATA_GROUP not in('0','10')  and CHECK_TIME is not null ";
                else
                    sqlCommand += "  and CS_DATA_GROUP='1' ";

                sqlCommand += string.Format(@" ) t3 on t.PALLET_NO=t3.PALLET_NO
                                                left join RBAC_USER t4 on t3.CHECKER=t4.BADGE 
                                                where t.DELETED_TERM_FLAG<2 and  t.FACTORYROOM_KEY='{0}'", roomkey);

                if (!string.IsNullOrEmpty(s_palletNo))
                    sqlCommand += string.Format(" and t3." + WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "='{0}'", s_palletNo.PreventSQLInjection());

                if (!string.IsNullOrEmpty(cust_type) && cust_type.Trim().Equals("1") && !string.IsNullOrEmpty(s_lot))
                    sqlCommand += string.Format(" and t." + POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE + "='{0}'", s_lot.PreventSQLInjection());
                else if (!string.IsNullOrEmpty(s_lot))
                    sqlCommand += string.Format(" and t." + POR_LOT_FIELDS.FIELD_LOT_NUMBER + "='{0}'", s_lot.PreventSQLInjection());
               
                if (!string.IsNullOrEmpty(s_wo))
                    sqlCommand += string.Format(" and t." + POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "='{0}'", s_wo.PreventSQLInjection());

                if (flag.Equals("query"))
                {
                    if (!string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
                        sqlCommand += string.Format(" and t3." + WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + " between CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')",
                            s_date.PreventSQLInjection(), e_date.PreventSQLInjection());
                    else if (!string.IsNullOrEmpty(s_date))
                        sqlCommand += string.Format(" and t3." + WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + " >= CONVERT(datetime,'{0}')", s_date.PreventSQLInjection());
                    else if (!string.IsNullOrEmpty(e_date))
                        sqlCommand += string.Format(" and t3." + WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + " <= CONVERT(datetime,'{0}')", e_date.PreventSQLInjection());
                }
                else
                {
                    if (!string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
                        sqlCommand += string.Format(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_EDIT_TIME + " between CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')",
                            s_date.PreventSQLInjection(), e_date.PreventSQLInjection());
                    else if (!string.IsNullOrEmpty(s_date))
                        sqlCommand += string.Format(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_EDIT_TIME + " >= CONVERT(datetime,'{0}')", s_date.PreventSQLInjection());
                    else if (!string.IsNullOrEmpty(e_date))
                        sqlCommand += string.Format(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_EDIT_TIME + " <= CONVERT(datetime,'{0}')", e_date.PreventSQLInjection());
                }
                //sqlCommand += " order by t2.EDIT_TIME desc ,t3.EDIT_TIME desc";

                DataTable dtToWarehouseCheck = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtToWarehouseCheck.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
                dtToWarehouseCheck.Columns[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;

                dsReturn.Merge(dtToWarehouseCheck, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetToWarehouseCheckData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 入库检验作业出站
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
        public DataSet SaveToWarehouseCheckData(DataSet dsSave)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            string operationName = string.Empty;
            string palletkey = string.Empty;          
            //string enterpriseKey = string.Empty, routeKey = string.Empty, stepKey = string.Empty;
            //string toEnterpriseKey = string.Empty, toRouteKey = string.Empty, toStepKey = string.Empty;
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    DataTable dtWoarehouseCheckData = dsSave.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                    if (dtWoarehouseCheckData.Rows.Count < 1)
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "服务端没有找到入库检的数据，请确认!");
                    else
                    {
                        string palletno = Convert.ToString(dtWoarehouseCheckData.Rows[0][WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO]);
                        string checker = Convert.ToString(dsSave.ExtendedProperties[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECKER]);
                        string roomkey = Convert.ToString(dsSave.ExtendedProperties[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY]);
                        string opr_computer = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
                        string equipmentkey = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
                        string linename = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                        string linekey = Convert.ToString(dsSave.ExtendedProperties[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                        string shiftKey = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
                        string shiftValue = Convert.ToString(dsSave.ExtendedProperties[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);

                        //入库检验作业
                        string saveType = "real";
                        //更新托号数据
                        sqlCommand = string.Format(@"update WIP_CONSIGNMENT 
                                                     set CS_DATA_GROUP=2 ,CHECK_TIME=getdate(),CHECKER='{1}'
                                                     where ISFLAG=1 and VIRTUAL_PALLET_NO='{0}' and ROOM_KEY='{2}'",
                                                     palletno, checker, roomkey);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                        sqlCommand = string.Format(@"select CONSIGNMENT_KEY 
                                                     from WIP_CONSIGNMENT 
                                                     where CS_DATA_GROUP=2 and ISFLAG=1 
                                                     and VIRTUAL_PALLET_NO='{0}' and ROOM_KEY='{1}'", 
                                                     palletno, roomkey);
                        palletkey = Convert.ToString(db.ExecuteDataSet(dbTran, CommandType.Text, sqlCommand).Tables[0].Rows[0][0]);

                        //更新批次
                        sqlCommand = string.Format(@"update t 
                                                    set t.LINE_NAME='{0}',t.CUR_PRODUCTION_LINE_KEY='{1}',
                                                        t.EDIT_TIME=GETDATE(),t.EDITOR='{2}',STATE_FLAG=9                                       
                                                    from por_lot t
                                                    where t.DELETED_TERM_FLAG<1 and t.HOLD_FLAG=0 
                                                    and t.PALLET_NO='{3}' and t.FACTORYROOM_KEY='{4}'",
                                                    linename.PreventSQLInjection(),
                                                    linekey.PreventSQLInjection(),
                                                    checker.PreventSQLInjection(),
                                                    palletno.PreventSQLInjection(),
                                                    roomkey);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        //-----------------------------------------------------------------------------------------------------------------------------
                        SaveConfigmentDataBySaveType(db, dbTran, saveType, checker, palletno, palletkey,roomkey, "", opr_computer, dtWoarehouseCheckData, shiftKey, shiftValue, equipmentkey);
                    }
                    //Commit Transaction
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("SaveToWarehouseCheckData Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }

            return dsReturn;
        }
        /// <summary>
        /// 获取测试规则包装等级设置。
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetTestRulePackageLevel(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string productCode=Convert.ToString(hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE]);
                string lotNumber=Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.LOT_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT a.PRO_ID AS PRODUCT_CODE,e.GRADE,
                                                               e.MIX_LEVEL AS MIN_LEVEL,
                                                               e.MIX_LEVEL_DETAIL AS MIN_LEVEL_DETAIL,
                                                               e.MIX_COLOR AS MIN_COLOR,e.PALLET_GROUP
                                                        FROM POR_LOT a
                                                        LEFT JOIN POR_PRODUCT b ON b.PRODUCT_CODE=a.PRO_ID AND b.ISFLAG=1
                                                        LEFT JOIN POR_WO_PRD_LEVEL e ON e.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
                                                                                        AND e.PRODUCT_KEY=b.PRODUCT_KEY 
                                                                                        AND e.IS_USED='Y'
                                                        WHERE a.LOT_NUMBER='{0}';
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        SELECT t.PRODUCT_CODE,t2.GRADE,t2.MIN_LEVEL,t2.MIN_LEVEL_DETAIL,t2.MIN_COLOR,t2.PALLET_GROUP
                                                        FROM POR_PRODUCT t 
                                                        LEFT JOIN BASE_TESTRULE t1 ON t.PRO_TEST_RULE=t1.TESTRULE_CODE
                                                        LEFT JOIN BASE_TESTRULE_PROLEVEL t2 ON t1.TESTRULE_KEY=t2.TESTRULE_KEY
                                                        WHERE t.ISFLAG=1
                                                        AND t1.ISFLAG=1
                                                        AND t2.ISFLAG=1
                                                        AND t.PRODUCT_CODE='{1}'
                                                    END",
                                                    lotNumber.PreventSQLInjection(),
                                                    productCode.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRulePackageLevel Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 判断流程卡画面是否保存
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet IsExistModulePic(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string slot = Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                string sqlCommand = string.Format(@"select t.LOT_NUMBER,t.LOT_CUSTOMERCODE,t.LOT_KEY,t.CREATE_OPERTION_NAME,t1.create_date,t1.update_date,t1.serialno
                                                    from POR_LOT t 
                                                    left join SERIALNO_CARD_SAVE t1 on t.LOT_NUMBER=t1.serialno
                                                    where (t.LOT_NUMBER='{0}' or t.LOT_CUSTOMERCODE='{0}')", 
                                                    slot.PreventSQLInjection());
                DataTable dtIsExistModulePic = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtIsExistModulePic.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtIsExistModulePic, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistModulePic Error: " + ex.Message);
            }
            return dsReturn;   
        }
        /// <summary>
        /// 托号变更作业
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
        public DataSet SaveExchgPalletNumber(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    string newPalletNumber = Convert.ToString(hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO_NEW]);
                    string oldPalletNumber = Convert.ToString(hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO]);
                    string editer = Convert.ToString(hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR]);
                    string roomkey = Convert.ToString(hstable[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY]);
                    string newPalletKey=CommonUtils.GenerateNewKey(0);
                    //新增托号数据
                    string sql_Consigment = string.Format(@"INSERT INTO WIP_CONSIGNMENT
                                                                (CONSIGNMENT_KEY,VIRTUAL_PALLET_NO,PALLET_NO,WORKNUMBER,CS_DATA_GROUP,SAP_NO
                                                                ,POWER_LEVEL,GRADE,SHIFT,PS_CODE,PS_DTL_SUBCODE,LAST_PALLET
                                                                ,CREATER,CREATE_TIME,EDITOR,EDIT_TIME,ISFLAG,ROOM_KEY,CUSTOMER_NO
                                                                ,LOT_NUMBER_QTY,TOTLE_POWER,AVG_POWER,PRO_ID,PALLET_NO_NEW,PALLET_TYPE
                                                                ,CODE_TYPE,LINE_NAME,LINE_KEY,EQUIPMENT_KEY,EQUIPMENT_NAME,AVG_POWER_RANGE
                                                                ,LOT_COLOR,PS_SEQ,CHECKER,CHECK_TIME,TO_WH,TO_WH_TIME,OUT_WH
                                                                ,OUT_WH_TIME,FULL_QTY,ARK_FLAG)
                                                            SELECT '{3}','{2}','{2}',WORKNUMBER,'1',SAP_NO
                                                                ,POWER_LEVEL,GRADE,SHIFT,PS_CODE,PS_DTL_SUBCODE,LAST_PALLET
                                                                ,CREATER,CREATE_TIME,'{0}',GETDATE(),ISFLAG,ROOM_KEY,CUSTOMER_NO
                                                                ,LOT_NUMBER_QTY,TOTLE_POWER,AVG_POWER,PRO_ID,PALLET_NO_NEW,PALLET_TYPE
                                                                ,CODE_TYPE,LINE_NAME,LINE_KEY,EQUIPMENT_KEY,EQUIPMENT_NAME,AVG_POWER_RANGE
                                                                ,LOT_COLOR,PS_SEQ,CHECKER,CHECK_TIME,TO_WH,TO_WH_TIME,OUT_WH
                                                                ,OUT_WH_TIME,FULL_QTY,ARK_FLAG
                                                            FROM WIP_CONSIGNMENT t 
                                                            WHERE t.VIRTUAL_PALLET_NO='{1}'
                                                            AND t.ISFLAG=1",
                                                            editer,
                                                            oldPalletNumber.PreventSQLInjection(), 
                                                            newPalletNumber.PreventSQLInjection(),
                                                            newPalletKey);

                    //新增包装明细数据
                    string sql = string.Format(@"INSERT INTO WIP_CONSIGNMENT_DETAIL
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
                                                       '{2}',
                                                       GETDATE()
                                                FROM WIP_CONSIGNMENT a
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL b ON b.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY
                                                INNER JOIN POR_LOT c ON c.PALLET_NO=a.VIRTUAL_PALLET_NO AND c.LOT_NUMBER=b.LOT_NUMBER 
                                                WHERE a.VIRTUAL_PALLET_NO='{1}'
                                                AND a.ISFLAG=1",
                                                newPalletKey,
                                                oldPalletNumber.PreventSQLInjection(),
                                                editer.PreventSQLInjection());
                    this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //更新批次托号
                    string sql_Lot = string.Format(@"UPDATE t 
                                                    SET t.PALLET_NO='{0}',t.EDIT_TIME=GETDATE(),t.EDITOR='{1}'
                                                    FROM POR_LOT t 
                                                    WHERE t.PALLET_NO='{2}'", 
                                                    newPalletNumber.PreventSQLInjection(),
                                                    editer.PreventSQLInjection(),
                                                    oldPalletNumber.PreventSQLInjection());
                    //更新托号数据
                    string sql_Consigment2 = string.Format(@"UPDATE t 
                                                          SET t.PALLET_NO_NEW='{0}',t.EDITOR='{1}',t.EDIT_TIME=GETDATE(),t.ISFLAG=0
                                                          FROM WIP_CONSIGNMENT t 
                                                          WHERE t.VIRTUAL_PALLET_NO='{2}' and t.ISFLAG=1",
                                                          newPalletNumber.PreventSQLInjection(),
                                                          editer.PreventSQLInjection(), 
                                                          oldPalletNumber.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql_Consigment);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql_Consigment2);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql_Lot);

                    dbTran.Commit();

                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

                }
                catch (Exception ex)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveExchgPalletNumber Error: " + ex.Message);
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
        /// 获得批次的产品属性，工单属性
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetLotWoProAttribute(Hashtable hstable)
        {
            string sqlCommand = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                //获取批次工单属性数据
                sqlCommand = @"select t1.ORDER_NUMBER,t1.WORK_ORDER_KEY,t2.PRODUCT_CODE,
                                      t2.PRODUCT_KEY,t2.PRODUCT_NAME,t2.LABELCHECK,t2.PROMODEL_NAME,t2.LABELTYPE,t2.LABELVAR,
                                      t.LOT_KEY,t.LOT_NUMBER,t.PALLET_NO,t.PRO_LEVEL,t.PALLET_TIME,FACTORYROOM_KEY,t.LINE_NAME,t.LOT_TYPE,
                                      t2.ISEXPERIMENT,t3.ATTRIBUTE_NAME,t3.ATTRIBUTE_VALUE
                                from POR_LOT t
                                inner join POR_WORK_ORDER t1 on t.WORK_ORDER_KEY=t1.WORK_ORDER_KEY 
                                inner join POR_PRODUCT t2 on t.PRO_ID=t2.PRODUCT_CODE  and t2.ISFLAG=1
                                left join POR_WORK_ORDER_ATTR t3 on t1.WORK_ORDER_KEY=t3.WORK_ORDER_KEY and t3.ISFLAG=1
                                where  t.DELETED_TERM_FLAG<>2 ";
                if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                {
                    sqlCommand += string.Format(@" and t.LOT_NUMBER='{0}'", Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER]).PreventSQLInjection());
                }
                if (hstable.ContainsKey(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                {
                    sqlCommand += string.Format(@" and t.FACTORYROOM_KEY='{0}'", Convert.ToString(hstable[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]).PreventSQLInjection());
                }
                DataTable dtAttribute= db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtAttribute.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtAttribute, true, MissingSchemaAction.Add);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotWoProAttribute Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次号及指定等级获取批次对应工单中符合指定等级条件的推荐产品数据。
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <param name="grade">等级。</param>
        /// <returns>包含产品数据的数据集对象。</returns>
        public DataSet GetLotProductData(string lotNo, string grade)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取批次工单属性数据
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.LOT_NUMBER='{0}')
                                                    BEGIN
                                                        DECLARE @levelCount INT;
                                                        SELECT @levelCount=COUNT(1)
                                                        FROM POR_LOT a
                                                        INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                        INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                        INNER JOIN POR_WO_PRD_LEVEL d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY 
                                                                                      AND d.PART_NUMBER=c.PART_NUMBER 
                                                                                      AND d.IS_USED='Y'
                                                        WHERE a.LOT_NUMBER='{0}';
                                                        --批次对应的工单没有设置需要等级，默认为全等级
                                                        IF @levelCount<=0
                                                        BEGIN
                                                            SELECT c.PRODUCT_CODE,'{1}' AS PRODUCT_GRADE,c.PART_NUMBER,c.ITEM_NO
                                                            FROM POR_LOT a
                                                            INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                            INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                            WHERE a.LOT_NUMBER='{0}';
                                                        END
                                                        ELSE
                                                        BEGIN
                                                            SELECT c.PRODUCT_CODE,d.GRADE AS PRODUCT_GRADE,c.PART_NUMBER,c.ITEM_NO
                                                            FROM POR_LOT a
                                                            INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                            INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                            INNER JOIN POR_WO_PRD_LEVEL d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY 
                                                                                          AND d.PART_NUMBER=c.PART_NUMBER 
                                                                                          AND d.IS_USED='Y'
                                                            WHERE a.LOT_NUMBER='{0}'
                                                            AND d.GRADE='{1}';
                                                        END
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        
                                                        SELECT b.PRODUCT_CODE,d.PRODUCT_GRADE,d.SAP_PN AS PART_NUMBER,0 AS ITEM_NO
                                                        FROM POR_LOT a
                                                        LEFT JOIN POR_PRODUCT b ON b.PRODUCT_CODE=a.PRO_ID AND b.ISFLAG=1
                                                        LEFT JOIN POR_PRODUCT_DTL d ON d.PRODUCT_KEY=b.PRODUCT_KEY AND d.ISFLAG=1
                                                        WHERE a.LOT_NUMBER='{0}'
                                                        AND d.PRODUCT_GRADE='{1}';
                                                    END",
                                                    lotNo.PreventSQLInjection(),
                                                    grade.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotEngine.GetLotProductData Error: " + ex.Message);
            }

            return dsReturn;
        }

         /// <summary>
        /// 根据工单号获取工单对应的属性信息
        /// </summary>
        /// <param name="orderNumber">工单号</param>
        /// <returns>工单对应的属性信息</returns>
        public DataSet GetOrderAttrByOrderNumber(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取批次工单产品属性数据
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_WORK_ORDER
                                                              WHERE ORDER_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT ATTRIBUTE_NAME,ATTRIBUTE_VALUE 
	                                                    FROM POR_WORK_ORDER_ATTR
	                                                    WHERE WORK_ORDER_KEY = (SELECT TOP 1 WORK_ORDER_KEY 
							                                                    FROM POR_WORK_ORDER
							                                                    WHERE ORDER_NUMBER = '{0}'
                                                                                AND ISFLAG =1 )
                                                    END",
                                            orderNumber);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotEngine.GetOrderAttrByOrderNumber Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 根据组件序列号获取组件对应的铭牌检查类型
        /// </summary>
        /// <param name="LotNo">组件序列号</param>
        /// <returns>组件对应的检查类型的数据集</returns>
        public DataSet GetLotCustCheckType(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //获取批次检验类型数据
                string sqlCommand = string.Format(@"  IF EXISTS(SELECT 1 
			                                                    FROM POR_LOT A
			                                                    INNER JOIN POR_WO_PRD_PRINTSET B ON B.WORK_ORDER_KEY = A.WORK_ORDER_KEY
											                                                    AND B.PART_NUMBER = A.PART_NUMBER
											                                                    AND B.IS_USED = 'Y'
			                                                    INNER JOIN BASE_PRINTLABEL C ON C.LABEL_ID = B.PRINTLABEL_ID
										                                                    AND C.IS_USED = 'Y'
										                                                    AND C.DATA_TYPE = 'P'
										                                                    AND C.CUSTCHECK_TYPE IS NOT NULL
			                                                    WHERE A.STATUS < 2
			                                                    AND LOT_NUMBER = '{0}')
                                                      BEGIN
	                                                    SELECT C.CUSTCHECK_TYPE
	                                                    FROM POR_LOT A
	                                                    INNER JOIN POR_WO_PRD_PRINTSET B ON B.WORK_ORDER_KEY = A.WORK_ORDER_KEY
									                                                    AND B.PART_NUMBER = A.PART_NUMBER
									                                                    AND B.IS_USED = 'Y'
	                                                    INNER JOIN BASE_PRINTLABEL C ON C.LABEL_ID = B.PRINTLABEL_ID
								                                                    AND C.IS_USED = 'Y'
								                                                    AND C.DATA_TYPE = 'P'
								                                                    AND C.CUSTCHECK_TYPE IS NOT NULL
	                                                    WHERE A.STATUS < 2
	                                                    AND LOT_NUMBER = '{0}'
                                                      END
                                                      ELSE
                                                      BEGIN
	                                                    SELECT B.CUSTCHECK_TYPE
	                                                    FROM POR_LOT A
	                                                    INNER JOIN POR_WO_PRD B ON B.WORK_ORDER_KEY = A.WORK_ORDER_KEY
						                                                       AND B.PART_NUMBER = A.PART_NUMBER
						                                                       AND B.IS_USED = 'Y'
	                                                    WHERE A.STATUS < 2
	                                                    AND LOT_NUMBER = '{0}'
                                                      END",
                                                      lotNo.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotEngine.GetLotCustCheckType Error: " + ex.Message);
            }

            return dsReturn;
        }

    }
}
