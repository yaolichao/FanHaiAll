using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Collections;
using FanHai.Hemera.Share.Interface.SystemManagement;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 工单报工数据管理类。
    /// </summary>
    public class WorkOrderWorkForEngine : AbstractEngine, IWorkOrderWorkForEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化函数。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WorkOrderWorkForEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 获取工单报工数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="pageNo">第几页。</param>
        /// <param name="pageSize">每页行数。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="records">总记录数。</param>
        /// <returns>包含工单数据的数据集对象。</returns>
        public DataSet GetWorkOrderWorkFor(DataSet dsParams, 
                                           int pageNo, int pageSize, out int pages, out int records)
        {
            DataSet dsReturn = new DataSet();
            pages = 0;
            records = 0;
            try
            {
                string sqlString = @"SELECT (P.ACK_DATA + ' ' + P.ACK_TIME) AS ACKDATETIME,P.AUFNR,P.APLFL,P.GMNGA,P.XMNGA,
                                        P.REPORTOR,P.SHIFT_NAME,P.CON_END,P.WERKS,L.LOCATION_NAME
                                     FROM POR_WORK_ORDER_REPORT P,FMM_LOCATION L
                                     WHERE L.LOCATION_KEY = P.ROOM_KEY";

                if (pageNo > 0 && pageSize > 0)
                {
                    //查询结果翻页数据
                    AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, dsReturn, POR_WORK_ORDER_REPORT_FIELDS.DATABASE_TABLE_NAME);
                }
                else
                {
                    db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { POR_WORK_ORDER_REPORT_FIELDS.DATABASE_TABLE_NAME });
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderWorkFor Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 工单报工。
        /// 1.UPDATE WIP_TRANSACTION
        /// 2.UPDATE POR_QC_DATA
        /// 3.UPDATE WST_RK_ZXBPO
        /// 4.获取工单的报废数量
        /// 5.获取工单的合格品数量
        /// 6.INSERT POR_WORK_ORDER_REPORT
        /// 7.UPDATE WIP_TRANSACTION
        /// 8.UPDATE POR_QC_DATA
        /// 9.UPDATE WST_RK_ZXBPO
        /// 10.调用SAP RFC
        /// </summary>
        /// <param name="tableParam">
        /// 传入参数的值：AUFNR 工单号 ROOM_KEY 工厂主键 CREATOR 创建者 CREATE_TIMEZONE 创建时间 REPORTOR 报工人 SHIFT_NAME 班次
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet GongDanBaoGong(DataTable tableParam)
        {
            const string CONST_UPDATE_WIP_TRANSACTION_1 = @"UPDATE t		
                                                            SET    t.REPORT_FLAG='R'	
                                                            FROM   WIP_TRANSACTION t		
                                                            WHERE  EXISTS(SELECT a.TRANSACTION_KEY			
                                                                          FROM WIP_TRANSACTION a          			
                                                                          INNER JOIN POR_WORK_ORDER c ON a.WORK_ORDER_KEY=c.WORK_ORDER_KEY            			
                                                                          WHERE a.ACTIVITY='SETLOSS' 			
                                                                          AND a.UNDO_FLAG=0 			
                                                                          AND a.REPORT_FLAG='N'			
                                                                          AND c.ORDER_NUMBER='{0}'			
                                                                          AND a.TRANSACTION_KEY=t.TRANSACTION_KEY)";

            const string CONST_UPDATE_POR_QC_DATA_2 = @"UPDATE POR_QC_DATA		
                                                        SET  REPORT_FLAG='R'		
                                                        WHERE STATUS=1 AND REPORT_FLAG='N' 		
                                                        AND ORDER_NUMBER='{0}'";

            const string CONST_UPDATE_WST_RK_ZXBPO_3 = @"UPDATE t			
                                                        SET t.REPORT_FLAG='R'	
                                                        FROM  WST_RK_ZXBPO t			
                                                        WHERE t.AUFNR='{0}' 			
                                                            AND EXISTS (SELECT a.ZXBNO			
                                                                        FROM WST_RK_ZXBPO a			
                                                                        INNER JOIN WST_RK_ZXBKO b ON a.ZXBNO=b.ZXBNO			
                                                                        WHERE b.LVORM=0  AND b.STATE=2 AND a.REPORT_FLAG='N'			
                                                                        AND a.ZXBNO=t.ZXBNO AND a.BARCODE=t.BARCODE AND a.AUFNR='{0}')";

            const string CONST_SELECT_1 = @"SELECT SUM(quantity_out) SCRAP_QTY,c.ORDER_NUMBER            					
                                            FROM WIP_TRANSACTION a            					
                                            INNER JOIN POR_LOT b ON a.PIECE_KEY=b.LOT_KEY AND a.PIECE_TYPE=0            					
                                            INNER JOIN POR_WORK_ORDER c ON b.WORK_ORDER_KEY=c.WORK_ORDER_KEY            					
                                            WHERE a.ACTIVITY='SETLOSS' AND a.UNDO_FLAG=0 AND a.REPORT_FLAG='R' AND c.ORDER_NUMBER='{0}'       					
                                            GROUP BY c.ORDER_NUMBER     					
                                            UNION					
                                            SELECT SUM(SCRAPPED_QTY),ORDER_NUMBER					
                                            FROM POR_QC_DATA					
                                            WHERE STATUS=1 AND REPORT_FLAG='R' AND ORDER_NUMBER='{0}'      					
                                            GROUP BY ORDER_NUMBER";

            const string CONST_SELECT_2 = @"SELECT SUM(MENGE) OK_QTY,a.AUFNR					
                                            FROM WST_RK_ZXBPO a					
                                            INNER JOIN WST_RK_ZXBKO b ON a.ZXBNO=b.ZXBNO					
                                            WHERE b.LVORM=0  AND b.STATE=2 AND a.REPORT_FLAG='R' AND b.ORDER_NUMBER='{0}'   
                                            GROUP BY a.AUFNR";

            const string CONST_INSERT_POR_WORK_ORDER_REPORT_1 = @"INSERT INTO POR_WORK_ORDER_REPORT
                                                                    (ROW_KEY,ACK_DATA,ACK_TIME,AUFNR,APLFL,GMNGA,GMEIN,XMNGA,RMZHL,TYPE,START_TIME_STAMP
                                                                    ,TIME_STAMP,ROOM_KEY,WERKS,CON_END,CREATE_TIME,CREATOR,CREATE_TIMEZONE,REPORTOR,SHIFT_NAME)
                                                                    VALUES('{0}',CONVERT(varchar(10), GETDATE(),126),CONVERT(varchar(10), GETDATE(),108),'{1}','0060',
                                                                    {2},'PC',{3},'0','0','',CAST('{4}' AS Datetime),'{5}','HZ01','N',GETDATE(),'{6}','{7}','{8}','{9}')";

            const string CONST_UPDATE_POR_WORK_WIP_TRANSACTION_4 = @"UPDATE t
                                                                    SET t.REPORT_FLAG='Y'	
                                                                    FROM WIP_TRANSACTION t
                                                                    WHERE EXISTS(SELECT a.TRANSACTION_KEY				
                                                                                  FROM WIP_TRANSACTION a          				
                                                                                  INNER JOIN POR_WORK_ORDER c ON a.WORK_ORDER_KEY=c.WORK_ORDER_KEY            				
                                                                                  WHERE a.ACTIVITY='SETLOSS' 				
                                                                                  AND a.UNDO_FLAG=0 				
                                                                                  AND a.REPORT_FLAG='R'				
                                                                                  AND c.ORDER_NUMBER='{0}'				
                                                                                  AND a.TRANSACTION_KEY=t.TRANSACTION_KEY)";

            const string CONST_UPDATE_POR_QC_DATA_5 = @"UPDATE POR_QC_DATA		
                                                        SET  REPORT_FLAG='Y'		
                                                        WHERE STATUS=1 AND REPORT_FLAG='R' 		
                                                        AND ORDER_NUMBER='{0}'";

            const string CONST_UPDATE_WST_RK_ZXBPO_6 = @"UPDATE t 	
                                                        SET t.REPORT_FLAG='Y'
                                                        FROM WST_RK_ZXBPO t		
                                                        WHERE t.AUFNR='{0}' 			
                                                        AND EXISTS (SELECT a.ZXBNO			
                                                                    FROM WST_RK_ZXBPO a			
                                                                    INNER JOIN WST_RK_ZXBKO b ON a.ZXBNO=b.ZXBNO			
                                                                    WHERE b.LVORM=0  AND b.STATE=2 AND a.REPORT_FLAG='R'			
                                                                    AND a.ZXBNO=t.ZXBNO AND a.BARCODE=t.BARCODE AND a.AUFNR='{0}')";

            DataSet dsReturn = new DataSet();

            DataTable dtHash = tableParam;
            Hashtable hsTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);

            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                string sqlUpdate1 = "";
                string sqlUpdate2 = "";
                string sqlUpdate3 = "";
                string sqlSelect1 = "";
                string sqlSelect2 = "";
                string sqlInsert1 = "";
                string sqlUpdate4 = "";
                string sqlUpdate5 = "";
                string sqlUpdate6 = "";

                DbCommand dbCom = dbCon.CreateCommand();
                dbCom.Transaction = dbTrans;
                string orderNumber=Convert.ToString(hsTable["AUFNR"]).PreventSQLInjection();
                sqlUpdate1 = string.Format(CONST_UPDATE_WIP_TRANSACTION_1, orderNumber);
                dbCom.CommandText = sqlUpdate1;
                int i = dbCom.ExecuteNonQuery();

                sqlUpdate2 = string.Format(CONST_UPDATE_POR_QC_DATA_2, orderNumber);
                dbCom.CommandText = sqlUpdate2;
                int j = dbCom.ExecuteNonQuery();

                sqlUpdate3 = string.Format(CONST_UPDATE_WST_RK_ZXBPO_3, orderNumber);
                dbCom.CommandText = sqlUpdate3;
                int k = dbCom.ExecuteNonQuery();

                sqlSelect1 = string.Format(CONST_SELECT_1, orderNumber);
                DataSet dsBaoFei = db.ExecuteDataSet(dbTrans,CommandType.Text, sqlSelect1);

                sqlSelect2 = string.Format(CONST_SELECT_2, orderNumber);
                DataSet dsHeGe = db.ExecuteDataSet(dbTrans,CommandType.Text, sqlSelect2);


                if (dsHeGe.Tables[0].Rows.Count <= 0 && dsBaoFei.Tables[0].Rows.Count <= 0)
                {
                    dbTrans.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "合格品数量=0 并且报废数量=0，无法报工。");
                }
                else
                {
                    string scrapQty = "0";
                    if (dsBaoFei.Tables[0].Rows.Count > 0)
                    {
                        scrapQty = Convert.ToString(dsBaoFei.Tables[0].Rows[0][0]);
                    }
                    string okQty = "0";
                    if (dsHeGe.Tables[0].Rows.Count > 0)
                    {
                        okQty = Convert.ToString(dsHeGe.Tables[0].Rows[0][0]);
                    }
                    scrapQty = string.IsNullOrEmpty(scrapQty) ? "0" : scrapQty;
                    okQty = string.IsNullOrEmpty(okQty) ? "0" : okQty;
                    if (scrapQty == "0" && okQty == "0")
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "合格品数量=0 并且报废数量=0，无法报工。");
                    }
                    else 
                    {
                        string sql = @"SELECT GETDATE()";
                        string time = Convert.ToString(db.ExecuteScalar(CommandType.Text, sql));

                        string strNewKey = System.Guid.NewGuid().ToString();
                        string roomKey=Convert.ToString(hsTable["ROOM_KEY"]);
                        string creator=Convert.ToString(hsTable["CREATOR"]);
                        string timezone=Convert.ToString(hsTable["CREATE_TIMEZONE"]);
                        string reportor=Convert.ToString(hsTable["REPORTOR"]);
                        string shiftName=Convert.ToString(hsTable["SHIFT_NAME"]);

                        sqlInsert1 = string.Format(CONST_INSERT_POR_WORK_ORDER_REPORT_1,
                                                            strNewKey,                    //主键
                                                            orderNumber,                  //工单号
                                                            okQty,  //合格品数量
                                                            scrapQty,  //报废数量
                                                            time,
                                                            roomKey,         //工厂车间
                                                            creator,          //创建人
                                                            timezone,  //创建时区
                                                            reportor,         //报工人
                                                            shiftName        //班次
                                                        );
                        dbCom.CommandText = sqlInsert1;
                        int p = dbCom.ExecuteNonQuery();

                        sqlUpdate4 = string.Format(CONST_UPDATE_POR_WORK_WIP_TRANSACTION_4, orderNumber);
                        dbCom.CommandText = sqlUpdate4;
                        int q = dbCom.ExecuteNonQuery();

                        sqlUpdate5 = string.Format(CONST_UPDATE_POR_QC_DATA_5, orderNumber);
                        dbCom.CommandText = sqlUpdate5;
                        int w = dbCom.ExecuteNonQuery();

                        sqlUpdate6 = string.Format(CONST_UPDATE_WST_RK_ZXBPO_6, orderNumber);
                        dbCom.CommandText = sqlUpdate6;
                        int e = dbCom.ExecuteNonQuery();

                        ///////////////////////////////////////////////////////////////////////////////////////////////////////
                        //调用RFC
                        DataSet dsOut = new DataSet();
                        DataSet dsIn = new DataSet();
                        DataTable dtWorkOrderWorkFor = new DataTable();
                        dtWorkOrderWorkFor.Columns.Add("ITMNO");
                        dtWorkOrderWorkFor.Columns.Add("AUFNR");
                        dtWorkOrderWorkFor.Columns.Add("WERKS");
                        dtWorkOrderWorkFor.Columns.Add("VORNR");
                        dtWorkOrderWorkFor.Columns.Add("LMNGA");
                        dtWorkOrderWorkFor.Columns.Add("MEINH");
                        dtWorkOrderWorkFor.Columns.Add("XMNGA");
                        dtWorkOrderWorkFor.Columns.Add("CON_END");

                        DataRow dr = dtWorkOrderWorkFor.NewRow();
                        dr["ITMNO"] = 1;
                        dr["AUFNR"] = hsTable["AUFNR"].ToString();//工单号
                        dr["WERKS"] = "HZ01";//杭州晶硅
                        dr["VORNR"] = "0060";//晶硅电池
                        dr["LMNGA"] = okQty;  //合格品数量
                        dr["MEINH"] = "PC";//单位
                        dr["XMNGA"] = scrapQty;  //报废数量
                        dr["CON_END"] = "";//确认
                        dtWorkOrderWorkFor.Rows.Add(dr);


                        dtWorkOrderWorkFor.TableName = "ZPP_WORK";
                        dsIn.Merge(dtWorkOrderWorkFor);

                        //AllCommonFunctions.SAPRemoteFunctionCall("RFC FUNCTION NAME", dsIn, out dsOut);
                        AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_COF", dsIn, out dsOut);
                        bool bRollback = false;
                        if (dsOut == null
                           || dsOut.Tables.Count == 0
                           || dsOut.Tables[0].Rows.Count == 0
                           || int.Parse(dsOut.Tables[0].Rows[0]["NUMBER"].ToString()) < 0)
                        {
                            bRollback = true;
                        }

                        if (!bRollback) //MESSAGE
                        {
                            dbTrans.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "工单报工成功！");
                        }
                        else
                        {
                            dbTrans.Rollback();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, dsOut.Tables[0].Rows[0][1].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("GongDanBaoGong Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }
    }
      
}
