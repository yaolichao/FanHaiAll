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
    /// 批次数据的操作类。
    /// </summary>
    public partial class LotEngine : AbstractEngine, ILotEngine
    {
        private Database db = null; //数据库操作对象。
        private Database _dbRead = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            //如果配置文件中有只读数据库连接字符串，则设置只读数据库实例
            if (System.Configuration.ConfigurationManager.ConnectionStrings["SQLServerHis"] != null)
            {
                this._dbRead = DatabaseFactory.CreateDatabase("SQLServerHis");
            }
            else //否则和默认数据库使用同样的实例。
            {
                this._dbRead = this.db;
            }
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据批次主键获取批次的历史操作记录
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 查询得到的包含批次操作信息的数据集对象。
        /// </returns>
        public DataSet GetInfoForLotHistory(object lotKey)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
         
            using (DbConnection dbconn = db.CreateConnection())
            {
                try
                {
                    string sql = string.Format(@"SELECT A.*,S.EQUIPMENT_CODE,U.USERNAME,
                                                       (SELECT ACTIVITY_COMMENT 
                                                        FROM WIP_TRANSACTION 
                                                        WHERE TRANSACTION_KEY=A.UNDO_TRANSACTION_KEY) UNDO_COMMENT 
                                                FROM WIP_TRANSACTION A
                                                LEFT JOIN EMS_EQUIPMENTS S ON A.EQUIPMENT_KEY = S.EQUIPMENT_KEY
                                                LEFT JOIN RBAC_USER U ON A.EDITOR=U.BADGE
                                                WHERE A.PIECE_TYPE = 0 
                                                AND A.PIECE_KEY = '{0}'
                                                ORDER BY A.TIME_STAMP DESC",
                                                Convert.ToString(lotKey).PreventSQLInjection());

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                    dsReturn.Tables[0].Columns["TIME_STAMP"].DateTimeMode = DataSetDateTime.Unspecified;

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);

                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("GetInfoForLotHistory Error: " + ex.Message);
                }
                finally
                {
                    dbconn.Close();
                }
            }
            DateTime endTime = DateTime.Now;
            LogService.LogInfo("GetInfoForLotHistory Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 为批次号打印获取批次信息。
        /// </summary>
        /// <param name="dtParams">包含查询条件的数据表。键值对数据表。
        /// 键值<see cref="POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER"/>,
        /// <see cref="POR_LOT_FIELDS.FIELD_IS_PRINT"/>,
        /// <see cref="POR_LOT_FIELDS.FIELD_IS_REWORKED"/>
        /// CREATE_TIME_START,CREATE_TIME_END
        /// </param>
        /// <returns>
        /// 包含批次信息的数据集。
        /// [LOT_KEY,LOT_NUMBER,IS_PRINT,REWORK_FLAG,QUANTITY,ORDER_NUMBER]
        /// </returns>
        public DataSet GetLotNumberForPrint(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                sql = @"SELECT A.LOT_KEY,A.LOT_NUMBER, A.IS_PRINT,A.REWORK_FLAG,A.QUANTITY,C.ORDER_NUMBER
                        FROM POR_LOT             A,
                             POR_WORK_ORDER      C
                        WHERE A.WORK_ORDER_KEY = C.WORK_ORDER_KEY";
                if (htParams.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER))
                {
                    string orderNumber = Convert.ToString(htParams[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                    sql = sql + " AND C.ORDER_NUMBER='"+orderNumber.PreventSQLInjection()+"'";
                }
                if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                {
                    string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                    sql = sql + " AND A.LOT_NUMBER='" + lotNumber.PreventSQLInjection() + "'";
                }
                if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_IS_REWORKED))
                {
                    string reworkFalg = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                    sql = sql + " AND A.REWORK_FLAG=" + reworkFalg.PreventSQLInjection() + "";
                }
                if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_IS_PRINT))
                {
                    if (htParams[POR_LOT_FIELDS.FIELD_IS_PRINT].ToString() == "0")
                    {
                        sql = sql + " AND A.IS_PRINT='0'";
                    }
                    else
                    {
                        string isPrint = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_IS_PRINT]);
                        sql = sql + " AND A.IS_PRINT>='" + isPrint.PreventSQLInjection()+ "'";
                    }
                }
                if (htParams.ContainsKey("CREATE_TIME_START"))
                {
                    string createTimeStart = Convert.ToString(htParams["CREATE_TIME_START"]);
                    sql = sql + " AND CONVERT(VARCHAR(10),GETDATE(),120)>='" +createTimeStart.PreventSQLInjection() + "'";
                }
                if (htParams.ContainsKey("CREATE_TIME_END"))
                {
                    string createTimeEnd= Convert.ToString(htParams["CREATE_TIME_END"]);
                    sql = sql + " AND CONVERT(VARCHAR(10),GETDATE(),120)<='" + createTimeEnd.PreventSQLInjection()+ "'";
                }

                sql = sql + " ORDER BY A.LOT_NUMBER DESC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,ex.Message);
                LogService.LogError("GetLotNumberForPrint Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新批次号码打印数量。
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        public void UpdatePrintFlag(string lotNumber)
        {
            UpdatePrintFlag(new List<string>(){lotNumber});
        }
        /// <summary>
        /// 批量更新批次号码打印数量。
        /// </summary>
        /// <param name="lotNumber">批次号集合。</param>
        public void UpdatePrintFlag(IList<string> lotNumbers)
        {
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                foreach (string lotNumber in lotNumbers)
                {
                    string sql = string.Format(@"UPDATE POR_LOT SET IS_PRINT=IS_PRINT+1 WHERE LOT_NUMBER='{0}'",
                                               lotNumber.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans,CommandType.Text, sql);
                }
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                throw ex;
            }
            finally
            {
                dbTrans.Dispose();
                dbCon.Close();
                dbCon.Dispose();
            }
        }
        /// <summary>
        /// 根据批次号获取批次报废不良信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次报废不良信息的数据集。
        /// [LOT_NUMBER,ROUTE_OPERATION_NAME,SCRAP_QUANTITY,DEFECT_QUANTITY]
        /// </returns>
        public DataSet GetScrapAndDefectQty(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"(SELECT P.LOT_NUMBER,'不良' AS STYLE,W.STEP_NAME,W.REASON_CODE_CLASS,W.REASON_CODE_NAME,SUM(DEFECT_QUANTITY) AS QTY
                                            FROM WIP_DEFECT W
                                            LEFT JOIN WIP_TRANSACTION T ON W.TRANSACTION_KEY = T.TRANSACTION_KEY
                                            LEFT JOIN POR_LOT P ON P.LOT_KEY = T.PIECE_KEY
                                            WHERE T.UNDO_FLAG=0 AND P.LOT_KEY='{0}'
                                            GROUP BY P.LOT_NUMBER,W.STEP_NAME,W.REASON_CODE_CLASS,W.REASON_CODE_NAME
                                            )
                                            UNION ALL
                                            (SELECT P.LOT_NUMBER,'报废',S.STEP_NAME,S.REASON_CODE_CLASS,S.REASON_CODE_NAME ,SUM(S.SCRAP_QUANTITY)
                                            FROM WIP_SCRAP S
                                            LEFT JOIN WIP_TRANSACTION T ON S.TRANSACTION_KEY = T.TRANSACTION_KEY
                                            LEFT JOIN POR_LOT P ON P.LOT_KEY = T.PIECE_KEY 
                                            WHERE T.UNDO_FLAG=0 AND P.LOT_KEY='{0}'
                                            GROUP BY P.LOT_NUMBER,S.STEP_NAME,S.REASON_CODE_CLASS,S.REASON_CODE_NAME)", 
                                        lotKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetScrapAndDefectQty Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次主键获取批次工序参数信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次工序参数信息的数据集。
        /// </returns>
        public DataSet GetParamInfo(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT p.LOT_NUMBER,a.STEP_NAME,a.PARAM_INDEX,a.PARAM_NAME,a.PARAM_VALUE
                                            FROM WIP_PARAM a 
                                            LEFT JOIN WIP_TRANSACTION b ON a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                            LEFT JOIN POR_LOT p ON a.LOT_KEY=p.LOT_KEY
                                            WHERE b.UNDO_FLAG=0
                                            AND a.LOT_KEY='{0}'
                                            ORDER BY a.EDIT_TIME,a.PARAM_INDEX",
                                            lotKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetParamInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据厂别获取组件录入卡控信息。
        /// </summary>
        /// <param name="sFactoryName">批次主键。</param>
        /// <returns>
        /// 包含组件录入卡控信息的数据集。
        /// </returns>
        public DataSet GetCheckbarcodeInputType(string sFactoryName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = "SELECT A.* ";
                sql += " FROM (SELECT T.ITEM_ORDER,";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'IsCheckBarcodeInput' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'IsCheckBarcodeInput',";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'FactoryName' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'FactoryName'";
                sql += " FROM CRM_ATTRIBUTE T,BASE_ATTRIBUTE T1,BASE_ATTRIBUTE_CATEGORY T2";
                sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
                sql += " AND UPPER(T2.CATEGORY_NAME) = 'Basic_CheckBarcode_InputType'";
                sql += " GROUP BY T.ITEM_ORDER) A";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sFactoryName))
                {
                    sql += " AND A.FactoryName='" + sFactoryName + "'";
                }
                sql += " ORDER BY A.ITEM_ORDER ASC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCheckbarcodeInputType Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
