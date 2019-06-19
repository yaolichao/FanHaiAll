using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.StaticFuncs;
using SAP.Middleware.Connector;
using FanHai.Hemera.Share.Interface.RFC;

namespace FanHai.Hemera.Modules.WMS
{
    class WarehouseWarrantOperationEngine : AbstractEngine, IWarehouseWarrantOperationEngine
    {
        private Database db;                              //用于数据库访问的变量  
        private Database db1;                             //用于数据库访问的变量  
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WarehouseWarrantOperationEngine()
        {
            db = DatabaseFactory.CreateDatabase("SQLServerAwms");
            db1 = DatabaseFactory.CreateDatabase("SQLServer");
        }

        public override void Initialize(){}

        #region IWarehouseWarrantOperationEngine 成员

        /// <summary>
        /// 获取订单类型对应的特性名称
        /// </summary>
        /// <param name="ZMMTYP"></param>
        /// <returns></returns>
        public DataSet GetATNAM(string ZMMTYP)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sbSql = string.Format(@"SELECT ZMMTYP, ITEMNO, ATNAM, ZINPUT, DEC 
                                                FROM DBO.AWMS_WH_ENTRY_TYPE 
                                                WHERE ZMMTYP = '{0}' AND ZINPUT = 'X' 
                                                ORDER BY ITEMNO ", ZMMTYP);
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("WarehouseWarrant Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据工单号获取工单明细
        /// </summary>
        /// <param name="WorkNO"></param>
        /// <returns></returns>
        public DataSet GetWorkItems(string PALNO)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                using (DbConnection dbConn = db1.CreateConnection())
                {
                    dbConn.Open();
                    DbCommand dbCmd = dbConn.CreateCommand();
                    dbCmd.CommandType = CommandType.StoredProcedure;
                    dbCmd.CommandText = "Process_PALLET_DATA";

                    dbCmd.Parameters.Clear();
                    this.db1.AddInParameter(dbCmd, "PALNO", DbType.String, PALNO);
                    this.db1.AddOutParameter(dbCmd, "RET", DbType.Int32, 100);
                    dsReturn = db1.ExecuteDataSet(dbCmd);
                    int RET = Convert.ToInt32(this.db.GetParameterValue(dbCmd, "RET").ToString());
                    if(RET == -1)
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "托盘号对应的明细不存在！");
                    else if(RET == -2)
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "工单中物料编号不存在！");
                    else if(RET == -3)
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, "物料信息有误或不存在！");
                    else
                        ReturnMessageUtils.AddServerReturnMessage(dsReturn, String.Empty);
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("WarehouseWarrant Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 保存入库单
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public int SaveWarehouseWarrant(DataTable dtHead, DataTable dtItem, out string returnInfo)
        {
            string sql;
            int returnVal = 0;  //成功 0， 失败 非0
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                DataRow drHead = dtHead.Rows[0];
                string ZMBLNR = drHead["ZMBLNR"].ToString();            //入库单号
                string WERKS = drHead["WERKS"].ToString();              //工厂号
                string ZMMTYP = drHead["ZMMTYP"].ToString();            //订单类别
                string AUFNR = drHead["AUFNR"].ToString();              //工单号码
                string VBELN_OEM = drHead["VBELN_OEM"].ToString();      //OEM发货单
                string DEPT = drHead["DEPT"].ToString();                //部门
                string CREATOR = drHead["CREATOR"].ToString();          //创建人

                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();

                if (String.IsNullOrEmpty(ZMBLNR))   //新增
                {
                    //获取入库单号
                    DbCommand dbCmd = dbConn.CreateCommand();
                    dbCmd.CommandType = CommandType.StoredProcedure;
                    dbCmd.CommandText = "DBO.CreateNextNO";
                    dbCmd.Parameters.Clear();
                    this.db.AddInParameter(dbCmd, "TABNAME", DbType.String, "AWMS_WH_ENTRY");
                    this.db.AddOutParameter(dbCmd, "NO", DbType.String, 14);
                    db.ExecuteNonQuery(dbCmd, dbTran);
                    ZMBLNR = this.db.GetParameterValue(dbCmd, "NO").ToString();

                    sql = String.Format(@"INSERT INTO dbo.AWMS_WH_ENTRY 
                                            ( ZMBLNR, WERKS, ZMMTYP, AUFNR, VBELN_OEM, DEPT, CDATE, CREATOR, ISSYN, STATUS ) 
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '正常')",
                                                ZMBLNR.PreventSQLInjection(),
                                                WERKS.PreventSQLInjection(),
                                                ZMMTYP.PreventSQLInjection(),
                                                AUFNR.PreventSQLInjection(),
                                                VBELN_OEM.PreventSQLInjection(),
                                                DEPT.PreventSQLInjection(),
                                                DateTime.Now,
                                                CREATOR.PreventSQLInjection(),
                                                "0");
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    returnInfo = "入库单新建成功，入库单号：" + ZMBLNR;
                }
                else //修改
                {
                    sql = String.Format(@"UPDATE dbo.AWMS_WH_ENTRY 
                                            SET WERKS = '{0}', VBELN_OEM = '{1}', DEPT = '{2}' 
                                                WHERE ZMBLNR = '{3}' ",
                                                 WERKS.PreventSQLInjection(),
                                                 VBELN_OEM.PreventSQLInjection(),
                                                 DEPT.PreventSQLInjection(),
                                                 ZMBLNR.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    returnInfo = "入库单修改成功";
                }


                sql = String.Format(@"UPDATE dbo.AWMS_WH_ENTRY_DETAIL 
                                        SET ITEMSTATUS = '处理中', ZEILE = 0 
                                            WHERE ZMBLNR = '{0}' AND ITEMSTATUS = '正常'", 
                                            ZMBLNR.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    DataRow dr = dtItem.Rows[i];
                    string ITEMSTATUS = dr["ITEMSTATUS"].ToString();
                    string ZEILE = dr["ZEILE"].ToString();
                    string BWART = dr["BWART"].ToString();
                    string MATNR = dr["MATNR"].ToString();
                    string LGORT = dr["LGORT"].ToString();
                    string MENGE = dr["MENGE"].ToString();
                    string XP001 = dr["XP001"].ToString();
                    string XP003 = dr["XP003"].ToString();
                    string XP004 = dr["XP004"].ToString();
                    string XP005 = dr["XP005"].ToString();
                    string XP006 = dr["XP006"].ToString();
                    string KEYNO = dr["KEYNO"].ToString();
                    string REMARK = dr["REMARK"].ToString();

                    sql = String.Format(@"SELECT COUNT(*) 
                                            FROM AWMS_WH_ENTRY_DETAIL 
                                            WHERE ZMBLNR = '{0}' AND KEYNO = '{1}' AND ITEMSTATUS = '处理中'",
                                            ZMBLNR.PreventSQLInjection(),
                                            KEYNO.PreventSQLInjection());
                    Object cnt = db.ExecuteScalar(dbTran, CommandType.Text, sql);

                    if (Convert.ToInt32(cnt) == 0)
                    {
                        sql = String.Format(@"INSERT INTO dbo.AWMS_WH_ENTRY_DETAIL 
                                              (ZMBLNR, ZEILE, BWART, MATNR, LGORT, MENGE, XP001, XP003, XP004, XP005, XP006, ITEMSTATUS, KEYNO, REMARK) 
                                              VALUES 
                                              ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}')",
                                        ZMBLNR.PreventSQLInjection(),
                                        ZEILE.ToString().PreventSQLInjection(),
                                        BWART.PreventSQLInjection(),
                                        MATNR.PreventSQLInjection(),
                                        LGORT.PreventSQLInjection(),
                                        MENGE.PreventSQLInjection(),
                                        XP001.PreventSQLInjection(),
                                        XP003.PreventSQLInjection(),
                                        XP004.PreventSQLInjection(),
                                        XP005.PreventSQLInjection(),
                                        XP006.PreventSQLInjection(),
                                        ITEMSTATUS.PreventSQLInjection(),
                                        KEYNO.PreventSQLInjection(),
                                        REMARK.PreventSQLInjection());
                    }
                    else
                    {
                        sql = String.Format(@"UPDATE dbo.AWMS_WH_ENTRY_DETAIL SET ZEILE = '{0}', LGORT = '{1}', BWART = '{2}', MATNR = '{3}', 
                                                MENGE = '{4}', XP001 = '{5}', XP003 = '{6}', XP005 = '{7}', XP006 = '{8}', ITEMSTATUS = '{9}',
                                                KEYNO = '{10}', REMARK = '{11}' 
                                                WHERE ZMBLNR = '{12}' AND KEYNO = '{13}' AND ITEMSTATUS = '处理中' ", 
                                                ZEILE.PreventSQLInjection(),
                                                LGORT.PreventSQLInjection(),
                                                BWART.PreventSQLInjection(),
                                                MATNR.PreventSQLInjection(),
                                                MENGE.PreventSQLInjection(),
                                                XP001.PreventSQLInjection(),
                                                XP003.PreventSQLInjection(),
                                                XP005.PreventSQLInjection(),
                                                XP006.PreventSQLInjection(),
                                                ITEMSTATUS.PreventSQLInjection(),
                                                KEYNO.PreventSQLInjection(),
                                                REMARK.PreventSQLInjection(),
                                                ZMBLNR.PreventSQLInjection(),
                                                KEYNO.PreventSQLInjection());
                    }
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    sql = String.Format(@"UPDATE dbo.WIP_CONSIGNMENT 
                                            SET CS_DATA_GROUP = '5' 
                                            WHERE CONSIGNMENT_KEY = '{0}'", 
                                            KEYNO.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                sql = String.Format(@"UPDATE dbo.WIP_CONSIGNMENT SET CS_DATA_GROUP = '2' WHERE CONSIGNMENT_KEY IN (
                                        SELECT DISTINCT KEYNO FROM AWMS.DBO.AWMS_WH_ENTRY_DETAIL WHERE ZMBLNR = '{0}' AND ITEMSTATUS = '处理中')", ZMBLNR.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                sql = String.Format(@"UPDATE dbo.AWMS_WH_ENTRY_DETAIL SET ITEMSTATUS = '删除' WHERE ZMBLNR = '{0}' AND ITEMSTATUS = '处理中'", ZMBLNR.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();
            }
            catch (Exception ex)
            {
                returnInfo = "数据有误，保存失败！";
                LogService.LogError("WarehouseWarrant Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
                returnVal = 1;
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }

            return returnVal;
        }


        /// <summary>
        /// 入库单查询
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        public DataSet QueryWarehouseWarrant(DataSet dsParams, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                sBuilder.Append(@"SELECT ZMBLNR, WERKS, STATUS, ZMMTYP, AUFNR, VBELN_OEM, DEPT, 
                                    CDATE, CREATOR, MBLNR, MBLNR1, CAST(ISSYN AS VARCHAR(10)) AS ISSYN, SYNMAN, SYNDATE
                                    FROM AWMS_WH_ENTRY a 
                                    WHERE STATUS = '正常' ");

                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);

                    if (htParams.ContainsKey("ZMBLNR"))
                    {
                        string ZMBLNR = Convert.ToString(htParams["ZMBLNR"]);
                        sBuilder.AppendFormat(" AND ZMBLNR LIKE '{0}%'", ZMBLNR.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey("AUFNR"))
                    {
                        string AUFNR = Convert.ToString(htParams["AUFNR"]);
                        sBuilder.AppendFormat(" AND AUFNR LIKE '{0}%'", AUFNR.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey("MATNR"))
                    {
                        string MATNR = Convert.ToString(htParams["MATNR"]);
                        sBuilder.AppendFormat(" AND EXISTS (SELECT ZMBLNR FROM AWMS_WH_ENTRY_DETAIL b WHERE a.ZMBLNR = b.ZMBLNR AND MATNR LIKE '{0}%')", 
                                                MATNR.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey("CREATOR"))
                    {
                        string CREATOR = Convert.ToString(htParams["CREATOR"]);
                        sBuilder.AppendFormat(" AND CREATOR LIKE '{0}%'", CREATOR.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey("BDATE"))
                    {
                        string BDATE = Convert.ToString(htParams["BDATE"]);
                        sBuilder.AppendFormat(" AND CDATE >= '{0}'", BDATE.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey("EDATE"))
                    {
                        string EDATE = Convert.ToString(htParams["EDATE"]);
                        sBuilder.AppendFormat(" AND CDATE <= '{0}'", EDATE.PreventSQLInjection());
                    }
                }

                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sBuilder.ToString(),
                    pconfig.PageNo,
                    pconfig.PageSize,
                    out pages,
                    out records,
                    db,
                    dsReturn,
                    POR_LOT_FIELDS.DATABASE_TABLE_NAME,
                    "ZMBLNR ASC");
                pconfig.Pages = pages;
                pconfig.Records = records;
                if(records == 0)
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "订单信息不存在！");
                else
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, String.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("WarehouseWarrant Query Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 查询抬头表
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        public DataSet QueryWarehouseWarrantHead(string ZMBLNR, string ISSYN, bool isQueryForSyn)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = String.Format(@"SELECT ZMBLNR, WERKS, STATUS, ZMMTYP, AUFNR,
                                                    VBELN_OEM, DEPT, CDATE, CREATOR, ISSYN
                                             FROM DBO.AWMS_WH_ENTRY a
                                             WHERE ZMBLNR LIKE '{0}%' AND STATUS = '正常' ", 
                                             ZMBLNR.PreventSQLInjection());
                if (!String.IsNullOrEmpty(ISSYN))
                    sql += @" AND ISSYN = '"+ISSYN+"' ";
                if (isQueryForSyn)
                    sql += @" AND EXISTS (SELECT ORDER_NUMBER FROM DBO.POR_WORK_ORDER  
                                            WHERE ORDER_NUMBER = a.AUFNR AND ORDER_STATE = 'REL')";
                sql += " ORDER BY CDATE ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("WarehouseWarrant Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 查询明细表
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        public DataSet QueryWarehouseWarrantItems(string ZMBLNR)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = String.Format(@"SELECT a.ZMBLNR, ZEILE, BWART, MATNR, LGORT, MENGE, a.MBLNR, CHARG, KEYNO, XP001, 
                                                XP002, XP003, XP004, XP005, XP006, XP007, XP008, XP009, XP010, XP011, XP012, 
                                                ITEMSTATUS, MEINS, REMARK, b.AUFNR
                                            FROM AWMS_WH_ENTRY_DETAIL a 
                                            LEFT JOIN AWMS_WH_ENTRY b ON a.ZMBLNR = b.ZMBLNR
                                            WHERE a.ZMBLNR = '{0}' AND ITEMSTATUS = '正常'
                                            ORDER BY ZEILE", 
                                            ZMBLNR.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("WarehouseWarrant Error: " + ex.Message);
            }
            return dsReturn;
        } 

        /// <summary>
        /// 入库单加删除标记
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        public bool AddLvorm(string ZMBLNR)
        {
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                string sql = string.Format(@"UPDATE AWMS_WH_ENTRY 
                                                SET STATUS = '删除' 
                                                WHERE ZMBLNR = '{0}'", ZMBLNR.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                
                sql = string.Format(@"UPDATE DBO.WIP_CONSIGNMENT 
                                        SET CS_DATA_GROUP = '2' 
                                        WHERE CONSIGNMENT_KEY 
                                        IN (SELECT KEYNO FROM AWMS_WH_ENTRY_DETAIL 
                                            WHERE ITEMSTATUS = '正常' AND ZMBLNR = '{0}')", ZMBLNR.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                sql = string.Format(@"UPDATE AWMS_WH_ENTRY_DETAIL 
                                        SET ITEMSTATUS = '删除' 
                                        WHERE ZMBLNR = '{0}'", ZMBLNR.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                
                dbTran.Commit();
            }
            catch (Exception ex)
            {
                LogService.LogError("WarehouseWarrant Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
                return false;
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return true;
        }
    

        /// <summary>
        /// 同步过账
        /// </summary>
        public bool SynSAP(string ZMBLNR, string SYNMAN, out string returnStr)
        {
            returnStr = String.Empty;
            string sql = String.Format(@"SELECT ZMBLNR, WERKS, ZMMTYP, AUFNR 
                                            FROM AWMS_WH_ENTRY 
                                            WHERE ZMBLNR = '{0}'", ZMBLNR.PreventSQLInjection());
            DataSet dsHead = db.ExecuteDataSet(CommandType.Text, sql);
            DataTable dtHead = dsHead.Tables[0];
            DataRow drHead = dtHead.Rows[0];

            string WERKS = drHead["WERKS"].ToString();
            string ZMMTYP = drHead["ZMMTYP"].ToString();
            string AUFNR = drHead["AUFNR"].ToString();

            ServerObjFactory factory=new ServerObjFactory();
            IRFCEngine rfcEngine=factory.Get<IRFCEngine>();

            sql = String.Format(@"SELECT ZMBLNR, ZEILE, BWART, MATNR, LGORT, MENGE, CHARG, XP001 AS CHR01, XP002 AS CHR02, XP003 AS CHR03, XP004 AS CHR04,
                                    XP005 AS CHR05, XP006 AS CHR06, XP007 AS CHR07, XP008 AS CHR08, XP009 AS CHR09, XP010 AS CHR10, XP011 AS CHR11, XP012 AS CHR12
                                    FROM AWMS_WH_ENTRY_DETAIL 
                                    WHERE ZMBLNR = '{0}' AND ITEMSTATUS = '正常' ORDER BY ZEILE",
                                  ZMBLNR.PreventSQLInjection());
            DataSet dsParams = db.ExecuteDataSet(CommandType.Text, sql);
            dsParams.Tables[0].TableName = "MSEGITEM";

            //字段
            dsParams.ExtendedProperties.Add("ZMBLNR", ZMBLNR);
            dsParams.ExtendedProperties.Add("WERKS", WERKS);
            dsParams.ExtendedProperties.Add("ZMMTYP", ZMMTYP);
            dsParams.ExtendedProperties.Add("AUFNR", AUFNR);
            dsParams.ExtendedProperties.Add("CDATE", String.Format("{0:yyyyMMdd}", DateTime.Now));
            
            DataSet dsReturn = rfcEngine.ExecuteRFC("ZMM_GOODSMVT_CREATE",dsParams);

            string MBLNR = dsReturn.ExtendedProperties["MBLNR"].ToString();
            string MBLNR1 = dsReturn.ExtendedProperties["MBLNR1"].ToString();

            if (!String.IsNullOrEmpty(MBLNR) || !String.IsNullOrEmpty(MBLNR1))
            {
                DbConnection dbConn = null;
                DbTransaction dbTran = null;
                try
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    
                    sql = String.Format(@"UPDATE AWMS_WH_ENTRY SET MBLNR = '{0}', MBLNR1 = '{1}', SYNMAN = '{2}', SYNDATE = '{3}', ISSYN = '{4}'
                                            WHERE ZMBLNR = '{5}'",
                                          MBLNR, MBLNR1, SYNMAN, DateTime.Now, "1", ZMBLNR);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    DataTable dtResultItem = dsReturn.Tables["MSEGITEM"];
                    for (int i = 0; i < dtResultItem.Rows.Count; i++)
                    {
                        DataRow dr = dtResultItem.Rows[i];
                        string retZMBLNR = dr["ZMBLNR"].ToString();
                        string retBWART = dr["BWART"].ToString();
                        string retCHARG = dr["CHARG"].ToString();
                        string retMEINS = dr["MEINS"].ToString();
                        string retXP002 = dr["CHR02"].ToString();
                        string retXP004 = dr["CHR04"].ToString();
                        string retXP007 = dr["CHR07"].ToString();
                        string retXP008 = dr["CHR08"].ToString();
                        string retXP009 = dr["CHR09"].ToString();
                        string retXP010 = dr["CHR10"].ToString();
                        string retXP011 = dr["CHR11"].ToString();
                        string retXP012 = dr["CHR12"].ToString();

                        string retXP001 = dr["CHR01"].ToString();
                        string retXP003 = dr["CHR03"].ToString();
                        string retXP005 = dr["CHR05"].ToString();
                        string retXP006 = dr["CHR06"].ToString();

                        string retMBLNR = retBWART.Equals("101") ? MBLNR : MBLNR1;
                        sql = String.Format(@"UPDATE AWMS_WH_ENTRY_DETAIL SET CHARG = '{0}', MEINS = '{1}', XP002 = '{2}',
                                                XP007 = '{3}', XP008 = '{4}', XP009 = '{5}', XP010 = '{6}', XP011 = '{7}',
                                                XP012 = '{8}', MBLNR = '{9}', XP001 = '{10}', XP003 = '{11}', XP005 = '{12}', 
                                                XP006 = '{13}' 
                                                WHERE ZMBLNR = '{14}' AND XP004 = '{15}' AND ITEMSTATUS = '正常'",
                                                retCHARG, retMEINS, retXP002, retXP007, retXP008, retXP009, retXP010,
                                                retXP011, retXP012, retMBLNR, retXP001, retXP003, retXP005, retXP006,
                                                retZMBLNR, retXP004);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                        sql = String.Format(@"UPDATE DBO.WIP_CONSIGNMENT 
                                                SET CS_DATA_GROUP = '3' 
                                                WHERE CONSIGNMENT_KEY IN (
                                                    SELECT KEYNO 
                                                    FROM AWMS_WH_ENTRY_DETAIL
                                                    WHERE ZMBLNR = '{0}' AND XP004 = '{1}' AND ITEMSTATUS = '正常')", retZMBLNR, retXP004);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    LogService.LogError("WarehouseWarrant Error: " + ex.Message);
                    returnStr = ex.Message;
                    if (dbTran != null)
                    {
                        dbTran.Rollback();
                    }
                    return false;
                }
                finally
                {
                    if (dbConn != null)
                    {
                        dbConn.Close();
                    }
                    dbConn = null;
                    dbTran = null;
                }
                returnStr = String.Format("过账成功！101移动凭证号：{0},531移动凭证号：{1}",
                    String.IsNullOrEmpty(MBLNR)? "无" : MBLNR,
                    String.IsNullOrEmpty(MBLNR1)? "无" : MBLNR1);
                return true;
            }
            else
            {
                returnStr += "过账失败！";
                DataTable dtResult = dsReturn.Tables["TBAPIRET"];
                for (int i = 0; i < dtResult.Rows.Count; i++ )
                {
                    DataRow dr = dtResult.Rows[i];
                    string type = dr["TYPE"].ToString();
                    string id = dr["id"].ToString();
                    int number = Convert.ToInt32(dr["NUMBER"]);
                    string message = dr["MESSAGE"].ToString();
                    string system = dr["SYSTEM"].ToString();
                    string REF_EXT1 = dr["MESSAGE_V1"].ToString();
                    string REF_EXT2 = dr["MESSAGE_V2"].ToString();
                    string REF_EXT3 = dr["MESSAGE_V3"].ToString();
                    returnStr += '\n'+type + ' ' + id + ' ' + number.ToString() + ' ' + message + ' ' + system + ' ' + REF_EXT1 + ' ' + REF_EXT2 + ' ' + REF_EXT3 ;
                }
                return false;
            }
        }
        #endregion

    }
}
