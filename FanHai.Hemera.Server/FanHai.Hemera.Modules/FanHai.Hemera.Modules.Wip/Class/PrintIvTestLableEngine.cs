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
using System.Transactions;

namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 标签打印
    /// </summary>
    public class PrintIvTestLableEngine : AbstractEngine, IPrintIvTestEngine
    {
        private Database db = null; //数据库操作对象。
        private Database _dbRead = null;
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PrintIvTestLableEngine()
        {
            this.db = DatabaseFactory.CreateDatabase();
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

        public DataSet GetIvTestData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetIvTestData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetLabelInfo(string strFilter)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT A.print_lableid,A.print_name FROM (";
                sql += "SELECT T.ITEM_ORDER";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'printlableid' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as print_lableid";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'printname' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as print_name";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'printdatatype' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as print_datatype";
                sql += " FROM CRM_ATTRIBUTE  T,BASE_ATTRIBUTE  T1,BASE_ATTRIBUTE_CATEGORY T2";
                sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY ";
                sql += " AND UPPER(T2.CATEGORY_NAME) = 'BASIC_PRINTLABEL'";
                sql += " GROUP BY T.ITEM_ORDER";
                sql += ") A";
                sql += " WHERE 1=1";

                if (!string.IsNullOrEmpty(strFilter))
                {
                    sql += " AND A.PRINT_DATATYPE='" + strFilter + "'";
                }

                sql += " ORDER BY A.ITEM_ORDER ASC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLabelinfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取标签数据。
        /// </summary>
        /// <param name="type">L:功率标签 P：铭牌</param>
        /// <returns></returns>
        public DataSet GetLabelData(string type)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT LABEL_ID ,LABEL_NAME,DATA_TYPE,PRINTER_TYPE,PRODUCT_MODEL,CERTIFICATE_TYPE,POWERSET_TYPE
                                           FROM BASE_PRINTLABEL 
                                           WHERE IS_USED='Y'
                                           AND IS_VALID='Y'
                                           AND DATA_TYPE='{0}'", type.PreventSQLInjection());
                dsReturn = this.db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetLabelData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPorLotInfo(string sSN)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM POR_LOT WHERE DELETED_TERM_FLAG!=2";

                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND LOT_NUMBER='" + sSN + "'";
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPorLotInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetIVTestDateInfo(string sSN, string sDefault)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM WIP_IV_TEST WHERE 1=1";
                if (!string.IsNullOrEmpty(sDefault))
                {
                    sql += " AND VC_DEFAULT='" + sDefault + "'";
                }
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND LOT_NUM='" + sSN + "'";
                }
                sql += " ORDER BY LOT_NUM DESC,L_ID DESC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetIVTestDateInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPorProductData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM POR_PRODUCT WHERE ISFLAG='1'";
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND PRODUCT_CODE='" + sProductCode + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPorProductData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPrintLabelSetInfo(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                //sql = "SELECT C.*";
                //sql += " FROM POR_PRODUCT A,BASE_TESTRULE B,BASE_TESTRULE_PRINTSET C";
                //sql += " WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE AND B.TESTRULE_KEY=C.TESTRULE_KEY";
                //sql += " AND A.ISFLAG='1' AND B.ISFLAG='1' AND C.ISFLAG='1'";

                sql = "DECLARE @PRODUCT_CODE NVARCHAR(100),@TESTRULE_CODE NVARCHAR(100) ";
                sql += " SET @PRODUCT_CODE='" + sProductCode + "'";
                sql += " SELECT  @TESTRULE_CODE=TESTRULE_KEY FROM POR_PRODUCT A,BASE_TESTRULE B ";
                sql += " WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE  AND A.ISFLAG='1' AND B.ISFLAG='1' AND  A.PRODUCT_CODE=@PRODUCT_CODE";
                sql += " SELECT * FROM BASE_TESTRULE_PRINTSET WHERE TESTRULE_KEY=@TESTRULE_CODE  AND ISFLAG='1' ";
                sql += " AND DECAY_KEY IN (SELECT DECAY_KEY FROM BASE_TESTRULE_DECAY WHERE TESTRULE_KEY=@TESTRULE_CODE  AND ISFLAG='1') ";
                sql += " ORDER BY VIEW_NAME ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrintLabelSetInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPrintLabelLogData(string sSN, string sLabelNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT B.*";
                sql += " FROM WIP_IV_TEST A,WIP_IV_TEST_PRINTLOG B";
                sql += " WHERE A.IV_TEST_KEY=B.IV_TEST_KEY AND A.VC_DEFAULT='1'";
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND A.LOT_NUM='" + sSN + "'";
                }
                if (!string.IsNullOrEmpty(sLabelNo))
                {
                    sql += " AND B.LABELNO='" + sLabelNo + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrintLabelLogData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取校准版测试时间。
        /// </summary>
        /// <param name="sSN">校准版序列号。</param>
        /// <param name="sDeviceNo">设备号。</param>
        /// <returns>包含校准版测试时间的数据集对象。</returns>
        public DataSet GetCalibrationMaxTTime(string sSN, string sDeviceNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT MAX(TTIME) AS 'TTIME'";
                sql += " FROM WIP_IV_TEST";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sDeviceNo))
                {
                    sql += " AND DEVICENUM='" + sDeviceNo + "'";
                }
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND LOT_NUM='" + sSN + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCalibrationMaxTTime Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetTestRuleData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT B.*";
                sql += " FROM POR_PRODUCT A,BASE_TESTRULE B";
                sql += " WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE";
                sql += " AND A.ISFLAG='1' AND B.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRuleData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetDecoeffiData(string sProductCode, string sCoeffCode, string sPM)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT E.*
                            FROM POR_PRODUCT A
                            INNER JOIN BASE_TESTRULE B ON A.PRO_TEST_RULE=B.TESTRULE_CODE
                            INNER JOIN BASE_TESTRULE_DECAY C ON  B.TESTRULE_KEY=C.TESTRULE_KEY 
                            INNER JOIN BASE_DECAYCOEFFI D ON C.DECOEFFI_KEY=D.DECOEFFI_KEY
                            INNER JOIN BASE_DECAYCOEFFI E ON E.D_CODE=D.D_CODE
                            WHERE A.ISFLAG='1' 
                            AND B.ISFLAG='1' 
                            AND C.ISFLAG='1' 
                            AND D.ISFLAG='1'
                            AND E.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sCoeffCode))
                {
                    sql += string.Format(" AND D.D_CODE='{0}'", sCoeffCode);
                }
                if (!string.IsNullOrEmpty(sPM))
                {
                    sql += " AND C.DECAY_POWER_MIN <='" + sPM + "' AND C.DECAY_POWER_MAX>='" + sPM + "'";
                }
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += "AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDecoeffiData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetProductModelData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT B.*";
                sql += " FROM POR_PRODUCT A,BASE_PRODUCTMODEL B";
                sql += " WHERE A.PROMODEL_NAME=B.PROMODEL_NAME";
                sql += " AND A.ISFLAG='1' AND B.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetProductModelData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetTestRuleCtlParaData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT C.*";
                sql += " FROM POR_PRODUCT A,BASE_TESTRULE B,BASE_TESTRULE_CTLPARA C";
                sql += " WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE AND B.TESTRULE_KEY=C.TESTRULE_KEY";
                sql += " AND A.ISFLAG='1' AND B.ISFLAG='1' AND C.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                sql += " ORDER BY C.CONTROL_OBJ ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRuleCtlParaData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPowerSetData(string lotNum, string sProductCode, string sPM, string sPSSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT C.*,0 DEMAND_QTY
                           FROM  POR_PRODUCT A,BASE_TESTRULE B,BASE_POWERSET C
                           WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE 
                           AND B.PS_CODE=C.PS_CODE
                           AND A.ISFLAG='1' AND B.ISFLAG='1' AND C.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sPSSeq))
                {
                    sql += " AND C.PS_SEQ='" + sPSSeq + "'";
                }
                if (!string.IsNullOrEmpty(sPM))
                {
                    sql += " AND C.P_MIN <='" + sPM + "' AND C.P_MAX >='" + sPM + "'";
                }
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                sql += " ORDER BY C.PS_SEQ ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                if (!string.IsNullOrEmpty(lotNum))
                {
                    sql = @"SELECT DISTINCT D.ARTICNO
                            FROM  POR_PRODUCT A,BASE_TESTRULE B,BASE_POWERSET C,BASE_POWERSET_COLORATCNO D,POR_LOT E
                            WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE 
                            AND B.PS_CODE=C.PS_CODE
                            AND A.ISFLAG='1' 
                            AND B.ISFLAG='1' 
                            AND C.ISFLAG='1' 
                            AND D.ISFLAG=1
                            AND C.POWERSET_KEY=D.POWERSET_KEY 
                            AND A.PRODUCT_CODE=E.PRO_ID 
                            AND D.COLOR_CODE=E.COLOR";
                    if (!string.IsNullOrEmpty(lotNum))
                    {
                        sql += " AND E.LOT_NUMBER='" + lotNum + "'";
                    }
                    if (!string.IsNullOrEmpty(sPM))
                    {
                        sql += " AND C.P_MIN <='" + sPM + "' AND C.P_MAX >='" + sPM + "'";
                    }
                    DataTable dtArticno = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    if (dtArticno.Rows.Count > 0)
                        dsReturn.ExtendedProperties.Add("articno", Convert.ToString(dtArticno.Rows[0][0]));
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerSetData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPowerSetDetailData(string sProductCode, string sPM, string sPSSeq, string sPSDSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT D.*
                            FROM  POR_PRODUCT A,BASE_TESTRULE B,BASE_POWERSET C,BASE_POWERSET_DETAIL D
                            WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE 
                            AND B.PS_CODE=C.PS_CODE 
                            AND C.POWERSET_KEY=D.POWERSET_KEY
                            AND A.ISFLAG='1' 
                            AND B.ISFLAG='1' 
                            AND C.ISFLAG='1' 
                            AND D.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sPSSeq))
                {
                    sql += " AND C.PS_SEQ='" + sPSSeq + "'";
                }
                if (!string.IsNullOrEmpty(sPM))
                {
                    sql += " AND C.P_MIN <= '" + sPM + "' AND C.P_MAX >= '" + sPM + "'";
                    sql += " AND D.P_DTL_MIN <= '" + sPM + "' AND D.P_DTL_MAX >= '" + sPM + "'";
                }
                if (!string.IsNullOrEmpty(sPSDSeq))
                {
                    sql += " AND D.PS_DTL_SUBCODE='" + sPSDSeq + "'";
                }
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                sql += " ORDER BY D.PS_DTL_SUBCODE ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerSetDetailData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPowerSetDetailDataByIMP(string sProductCode, string sPM, string sIMP, string sPSSeq, string sPSDSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT D.*
                           FROM  POR_PRODUCT A,BASE_TESTRULE B,BASE_POWERSET C,BASE_POWERSET_DETAIL D
                           WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE AND B.PS_CODE=C.PS_CODE 
                           AND C.POWERSET_KEY=D.POWERSET_KEY
                           AND A.ISFLAG='1' 
                           AND B.ISFLAG='1'
                           AND C.ISFLAG='1' 
                           AND D.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sPSSeq))
                {
                    sql += " AND C.PS_SEQ='" + sPSSeq + "'";
                }
                if (!string.IsNullOrEmpty(sPM))
                {
                    sql += " AND C.P_MIN <= '" + sPM + "' AND C.P_MAX >= '" + sPM + "'";
                    sql += " AND D.P_DTL_MIN <= '" + sIMP + "' AND D.P_DTL_MAX >= '" + sIMP + "'";
                }
                if (!string.IsNullOrEmpty(sPSDSeq))
                {
                    sql += " AND D.PS_DTL_SUBCODE='" + sPSDSeq + "'";
                }
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                sql += " ORDER BY D.PS_DTL_SUBCODE ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerSetDetailDataByIMP Error: " + ex.Message);
            }
            return dsReturn;
        }


        public DataSet GetTestRulePowerCtlData(string sProductCode, string sPM, string sPSSeq, string sSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT D.*";
                sql += " FROM POR_PRODUCT A,BASE_TESTRULE B,BASE_POWERSET C,BASE_TESTRULE_POWERCTL D";
                sql += " WHERE A.PRO_TEST_RULE=B.TESTRULE_CODE AND B.PS_CODE=C.PS_CODE AND B.TESTRULE_KEY=D.TESTRULE_KEY AND C.POWERSET_KEY=D.POWERSET_KEY";
                sql += " AND A.ISFLAG='1' AND B.ISFLAG='1' AND C.ISFLAG='1' AND D.ISFLAG='1'";
                if (!string.IsNullOrEmpty(sPSSeq))
                {
                    sql += " AND C.PS_SEQ='" + sPSSeq + "'";
                }
                if (!string.IsNullOrEmpty(sPM))
                {
                    sql += " AND C.P_MIN <= '" + sPM + "' AND C.P_MAX >= '" + sPM + "'";
                }
                if (!string.IsNullOrEmpty(sSeq))
                {
                    sql += " AND D.SEQ='" + sSeq + "'";
                }
                if (!string.IsNullOrEmpty(sProductCode))
                {
                    sql += " AND A.PRODUCT_CODE='" + sProductCode + "'";
                }
                sql += " ORDER BY D.SEQ ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRulePowerCtlData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet UpdateData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    int irows = 0;
                    irows = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    dbTran.Commit();
                    dsReturn.ExtendedProperties.Add("rows", irows.ToString());
                    //string s = dsReturn.ExtendedProperties["rows"].ToString();
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError(sUpFuntionName + " Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }
            return dsReturn;
        }

        public DataSet AddData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    int irows = 0;
                    irows = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    dbTran.Commit();
                    dsReturn.ExtendedProperties.Add("rows", irows.ToString());
                    //string s = dsReturn.ExtendedProperties["rows"].ToString();
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError(sUpFuntionName + " Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }
            return dsReturn;
        }

        public DataSet GetIVTestData2(string sWorkNum, string sStartSN, string sEndSN, string sStartDevice, string sEndDevice, string sStartDate, string sEndDate, string sDefault, string sVC_CONTROL)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                //sql = "SELECT * ";
                sql = "SELECT A.IV_TEST_KEY,A.TTIME,A.LOT_NUM,A.VC_WORKORDER,A.AMBIENTTEMP,A.INTENSITY,A.FF,A.EFF,A.PM,A.ISC,A.IPM,A.VOC,A.VPM";
                sql += ",A.DEVICENUM,A.VC_PSIGN,A.DT_PRINTDT,A.P_NUM,A.VC_DEFAULT,A.SENSORTEMP,A.VC_CUSTCODE,A.C_USERID,COEF_PMAX,A.COEF_ISC";
                sql += ",A.COEF_VOC,A.COEF_IMAX,A.COEF_VMAX,A.COEF_FF,A.VC_CELLEFF,A.DEC_CTM,RS,RSH,A.CALIBRATION_NO,A.Imp_Isc,A.ImpIsc_Control,E.CONTROL_OBJ,E.CONTROL_VALUE";
                sql += " from WIP_IV_TEST AS A LEFT JOIN POR_LOT AS B ON A.LOT_NUM = B.LOT_NUMBER ";
                sql += " LEFT JOIN POR_WO_PRD AS C ON B.WORK_ORDER_KEY = C.WORK_ORDER_KEY AND C.IS_USED= 'Y' AND C.IS_MAIN='Y'";
                sql += " LEFT JOIN BASE_PRODUCTMODEL AS D ON C.PROMODEL_NAME = D.PROMODEL_NAME";
                sql += " LEFT JOIN BASE_PRODUCTMODEL_CP AS E ON D.PROMODEL_KEY = E.PROMODEL_KEY AND E.CONTROL_OBJ='Imp/Isc' WHERE 1=1";
                if (!string.IsNullOrEmpty(sWorkNum))
                {
                    sql += " AND B.WORK_ORDER_NO = '" + sWorkNum + "'";
                }
                if (!string.IsNullOrEmpty(sStartSN))
                {
                    sql += " AND A.LOT_NUM >= '" + sStartSN + "'";
                }
                if (!string.IsNullOrEmpty(sEndSN))
                {
                    sql += " AND A.LOT_NUM <= '" + sEndSN + "'";
                }
                if (!string.IsNullOrEmpty(sStartDevice))
                {
                    sql += " AND A.DEVICENUM >= '" + sStartDevice + "'";
                }
                if (!string.IsNullOrEmpty(sEndDevice))
                {
                    sql += " AND A.DEVICENUM <= '" + sEndDevice + "'";
                }
                if (!string.IsNullOrEmpty(sStartDate))
                {
                    sql += " AND A.T_DATE >= '" + sStartDate + " 00:00:00'";
                }
                if (!string.IsNullOrEmpty(sEndDate))
                {
                    sql += " AND A.T_DATE <= '" + sEndDate + " 23:59:59'";
                }
                if (!string.IsNullOrEmpty(sDefault))
                {
                    sql += " AND A.VC_DEFAULT='" + sDefault + "'";
                }
                if (!string.IsNullOrEmpty(sVC_CONTROL))
                {
                    sql += " AND A.ImpIsc_Control='" + sVC_CONTROL + "'";
                }
                sql += " ORDER BY A.T_DATE DESC";
                dsReturn = this._dbRead.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRulePowerCtlData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得查询数据
        /// </summary>
        /// <param name="hsParams">根据查询条件-获得IV测试的数据信息</param>
        /// owner genchille.yang 
        /// 2012-12-03 16:07:12
        /// <returns>数据集合</returns>
        public DataSet GetIvTestDataForCustCheckQuery(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records, Hashtable hsParams)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sqlCommand = new StringBuilder();
            pages = 0;
            records = 0;
            bool blCustCheck = false;
            try
            {
                #region
                sqlCommand.Append(@"SELECT t2.SHIFT_NAME,T2.OPERATERS,t2.LOT_COLOR,t2.WORKNUMBER,t2.PRO_LEVEL,t2.CREATE_TIME,
                                    t5.EQUIPMENT_NAME,
                                    t.AMBIENTTEMP,t.INTENSITY,
                                    t.COEF_FF AS 'FF',t.TTIME,t.COEF_PMAX AS 'PM',t.COEF_ISC AS 'ISC',t.COEF_IMAX AS 'IPM' ,
                                    t.COEF_VOC AS 'VOC',t.EFF,t.COEF_VOC , t.VPM,
                                    t.DEVICENUM,t.VC_PSIGN,t.DT_PRINTDT,
                                    t.SENSORTEMP,t.VC_CUSTCODE,
                                    t.COEF_FF,t.COEF_PMAX,t.COEF_ISC,
                                    t.COEF_IMAX,t.COEF_VMAX,t.VC_CELLEFF,t.DEC_CTM,
                                    t.RS,t.RSH,t.CALIBRATION_NO,t.VC_DEFAULT,t.LOT_NUM,t.T_DATE,
                                    t1.LOT_KEY,t1.PALLET_NO,t1.LOT_CUSTOMERCODE,t1.LOT_SIDECODE,T1.PRO_ID,
                                    t1.FACTORYROOM_NAME LOCATION_NAME,
                                    t6.PARAM_VALUE,
                                    E.POWERLEVEL,
                                    T2.NAMEPLATENO,
                                    case isnull(t4.CS_DATA_GROUP,'0')   
                                    when '1' then N'待入库检' 
                                    when '2' then N'待入库' 
                                    when '3' then N'待出货'
                                    when '4' then N'已出货' else '' end CS_DATA_GROUP
                                    FROM  WIP_CUSTCHECK t2
                                    INNER JOIN EMS_EQUIPMENTS t5 ON t2.DEVICENUM=t5.EQUIPMENT_KEY
                                    INNER JOIN WIP_IV_TEST t ON t.LOT_NUM=t2.CC_FCODE1 and t2.ISFLAG=1 AND t.VC_DEFAULT = 1
                                    INNER JOIN POR_LOT t1 ON t1.LOT_NUMBER=t2.CC_FCODE1 and t1.LOT_NUMBER=t.LOT_NUM
                                    LEFT JOIN WIP_CONSIGNMENT t4 ON t4.VIRTUAL_PALLET_NO=t1.PALLET_NO and t4.ISFLAG=1
                                    LEFT JOIN (select * from WIP_PARAM
                                    where PARAM_NAME LIKE '接线盒%料号'
                                    and STEP_NAME='终检'
                                    ) t6 ON t6.LOT_KEY=t1.LOT_KEY AND t6.ENTERPRISE_KEY =t1.ROUTE_ENTERPRISE_VER_KEY
                                    INNER JOIN POR_WO_PRD C ON C.WORK_ORDER_KEY = t1.WORK_ORDER_KEY AND C.PART_NUMBER  = t1.PART_NUMBER AND C.IS_USED = 'Y'
                                    INNER JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY = t1.WORK_ORDER_KEY AND D.PART_NUMBER = t1.PART_NUMBER AND D.MODULE_NAME = VC_MODNAME AND D.IS_USED = 'Y'
                                    LEFT JOIN POR_WO_PRD_PS_SUB E ON E.WORK_ORDER_KEY = t1.WORK_ORDER_KEY AND E.PART_NUMBER = t1.PART_NUMBER AND E.POWERSET_KEY = D.POWERSET_KEY AND E.PS_SUB_CODE = t.I_PKID AND E.IS_USED = 'Y'
                                    WHERE t1.DELETED_TERM_FLAG<2");

                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM))
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM + "='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM]));
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID))
                    sqlCommand.AppendFormat(" and t1." + POR_LOT_FIELDS.FIELD_PRO_ID + "='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_PRO_ID]));
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO))
                    sqlCommand.AppendFormat(" and t1." + POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]));
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_PALLET_NO))
                    sqlCommand.AppendFormat(" and t4." + POR_LOT_FIELDS.FIELD_PALLET_NO + "='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_PALLET_NO]));
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME))
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME + " like '{0}%'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME]));
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY))
                    sqlCommand.AppendFormat(" and t1." + POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY + " = '{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY]));
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER))
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER + "='{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER]));
                //多选终检设备
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM))
                {
                    string[] s_array = Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM]).Split(',');
                    string devicenums = string.Empty;
                    foreach (string s in s_array)
                    {
                        devicenums += "'" + s.Trim() + "',";
                    }
                    if (!string.IsNullOrEmpty(devicenums))
                    {
                        devicenums = devicenums.TrimEnd(',');
                        sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM + " in ({0})", devicenums);
                    }
                    blCustCheck = true;
                }
                //多选测试设备
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + "1"))
                {
                    string[] s_array = Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + "1"]).Split(',');
                    string devicenums = string.Empty;
                    foreach (string s in s_array)
                    {
                        devicenums += "'" + s.Trim() + "',";
                    }
                    if (!string.IsNullOrEmpty(devicenums))
                    {
                        devicenums = devicenums.TrimEnd(',');
                        sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + " in ({0})", devicenums);
                    }
                }

                //终检时间
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "1"))
                {
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_TIME + ">='{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "1"]));
                    blCustCheck = true;
                }
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "2"))
                {
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_TIME + "<='{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "2"]));
                    blCustCheck = true;
                }
                if (blCustCheck)
                {
                    sqlCommand.Append(" and t2.CC_DATA_GROUP='1' ");
                }

                //测试时间
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "1"))
                {
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_TTIME + ">='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_TTIME + "1"]));
                }
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "2"))
                {
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_TTIME + "<='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_TTIME + "2"]));
                }
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT))
                {
                    if (Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT]).Equals("1"))
                    {
                        sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT + "='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT]));
                        blCustCheck = false;
                    }
                }
                string sql = string.Empty;
                if (blCustCheck)
                {
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT + "='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT]));
                }


                if (pageNo > 0 && pageSize > 0)
                {
                    //分页查询。
                    AllCommonFunctions.CommonPagingData(sqlCommand.ToString(),
                                                        pageNo,
                                                        pageSize,
                                                        out pages,
                                                        out records,
                                                        this._dbRead,
                                                        dsReturn,
                                                        WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME,
                                                        "desc",
                                                        new string[] { "CREATE_TIME", " TTIME", "VC_DEFAULT" });
                }
                else
                {
                    this._dbRead.LoadDataSet(CommandType.Text,
                                            sqlCommand.ToString(),
                                            dsReturn,
                                            new string[] { WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME });
                }
                dsReturn.Tables[0].Columns["CREATE_TIME"].DateTimeMode = DataSetDateTime.Unspecified;
                dsReturn.Tables[0].Columns["TTIME"].DateTimeMode = DataSetDateTime.Unspecified;

                #endregion

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetIvTestDataForCustCheckQuery Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取终检查询信息
        /// </summary>
        /// <param name="hsParams">获取满足条件的信息</param>
        /// owner yongbing.yang 
        /// 2013年12月18日 16:05:04
        /// <returns>数据集合</returns>
        public DataSet GetIvTestToCustCheckQueryForImport(Hashtable hsParams)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sqlCommand = new StringBuilder();

            bool blCustCheck = false;
            try
            {
                #region
                sqlCommand.Append(@"SELECT t2.SHIFT_NAME,T2.OPERATERS,t2.LOT_COLOR,t2.WORKNUMBER,t2.PRO_LEVEL,t2.CREATE_TIME,
                                    t5.EQUIPMENT_NAME,
                                    t.AMBIENTTEMP,t.INTENSITY,
                                    t.COEF_FF AS 'FF',t.TTIME,t.COEF_PMAX AS 'PM',t.COEF_ISC AS 'ISC',t.COEF_IMAX AS 'IPM' ,
                                    t.COEF_VOC AS 'VOC',t.EFF,t.COEF_VOC , t.VPM,
                                    t.DEVICENUM,t.VC_PSIGN,t.DT_PRINTDT,
                                    t.SENSORTEMP,t.VC_CUSTCODE,
                                    t.COEF_FF,t.COEF_PMAX,t.COEF_ISC,
                                    t.COEF_IMAX,t.COEF_VMAX,t.VC_CELLEFF,t.DEC_CTM,
                                    t.RS,t.RSH,t.CALIBRATION_NO,t.VC_DEFAULT,t.LOT_NUM,t.T_DATE,
                                    t1.LOT_KEY,t1.PALLET_NO,t1.LOT_CUSTOMERCODE,t1.LOT_SIDECODE,T1.PRO_ID,
                                    t1.FACTORYROOM_NAME LOCATION_NAME,
                                    t6.PARAM_VALUE,
                                    E.POWERLEVEL,
                                    T2.NAMEPLATENO,
                                    case isnull(t4.CS_DATA_GROUP,'0')   
                                    when '1' then N'待入库检' 
                                    when '2' then N'待入库' 
                                    when '3' then N'待出货'
                                    when '4' then N'已出货' else '' end CS_DATA_GROUP
                                    FROM  WIP_CUSTCHECK t2
                                    INNER JOIN EMS_EQUIPMENTS t5 ON t2.DEVICENUM=t5.EQUIPMENT_KEY
                                    INNER JOIN WIP_IV_TEST t ON t.LOT_NUM=t2.CC_FCODE1 and t2.ISFLAG=1 AND t.VC_DEFAULT = 1
                                    INNER JOIN POR_LOT t1 ON t1.LOT_NUMBER=t2.CC_FCODE1 and t1.LOT_NUMBER=t.LOT_NUM
                                    LEFT JOIN WIP_CONSIGNMENT t4 ON t4.VIRTUAL_PALLET_NO=t1.PALLET_NO and t4.ISFLAG=1
                                    LEFT JOIN (select * from WIP_PARAM
                                    where PARAM_NAME LIKE '接线盒%料号'
                                    and STEP_NAME='终检'
                                    ) t6 ON t6.LOT_KEY=t1.LOT_KEY AND t6.ENTERPRISE_KEY =t1.ROUTE_ENTERPRISE_VER_KEY
                                    INNER JOIN POR_WO_PRD C ON C.WORK_ORDER_KEY = t1.WORK_ORDER_KEY AND C.PART_NUMBER  = t1.PART_NUMBER AND C.IS_USED = 'Y'
                                    INNER JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY = t1.WORK_ORDER_KEY AND D.PART_NUMBER = t1.PART_NUMBER AND D.MODULE_NAME = VC_MODNAME AND D.IS_USED = 'Y'
                                    LEFT JOIN POR_WO_PRD_PS_SUB E ON E.WORK_ORDER_KEY = t1.WORK_ORDER_KEY AND E.PART_NUMBER = t1.PART_NUMBER AND E.POWERSET_KEY = D.POWERSET_KEY AND E.PS_SUB_CODE = t.I_PKID AND E.IS_USED = 'Y'
                                    WHERE t1.DELETED_TERM_FLAG<2");

                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM))
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM + "='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM]));
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID))
                    sqlCommand.AppendFormat(" and t1." + POR_LOT_FIELDS.FIELD_PRO_ID + "='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_PRO_ID]));
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO))
                    sqlCommand.AppendFormat(" and t1." + POR_LOT_FIELDS.FIELD_WORK_ORDER_NO + "='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]));
                if (hsParams.ContainsKey(POR_LOT_FIELDS.FIELD_PALLET_NO))
                    sqlCommand.AppendFormat(" and t4." + POR_LOT_FIELDS.FIELD_PALLET_NO + "='{0}'", Convert.ToString(hsParams[POR_LOT_FIELDS.FIELD_PALLET_NO]));
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME))
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME + " like '{0}%'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_SHIFT_NAME]));
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY))
                    sqlCommand.AppendFormat(" and t1." + POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY + " = '{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY]));
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER))
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER + "='{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER]));
                //多选终检设备
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM))
                {
                    string[] s_array = Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM]).Split(',');
                    string devicenums = string.Empty;
                    foreach (string s in s_array)
                    {
                        devicenums += "'" + s.Trim() + "',";
                    }
                    if (!string.IsNullOrEmpty(devicenums))
                    {
                        devicenums = devicenums.TrimEnd(',');
                        sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM + " in ({0})", devicenums);
                    }
                    blCustCheck = true;
                }
                //多选测试设备
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + "1"))
                {
                    string[] s_array = Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + "1"]).Split(',');
                    string devicenums = string.Empty;
                    foreach (string s in s_array)
                    {
                        devicenums += "'" + s.Trim() + "',";
                    }
                    if (!string.IsNullOrEmpty(devicenums))
                    {
                        devicenums = devicenums.TrimEnd(',');
                        sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + " in ({0})", devicenums);
                    }
                }

                //终检时间
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "1"))
                {
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_TIME + ">='{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "1"]));
                    blCustCheck = true;
                }
                if (hsParams.ContainsKey(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "2"))
                {
                    sqlCommand.AppendFormat(" and t2." + WIP_CUSTCHECK_FIELDS.FIELDS_CHECK_TIME + "<='{0}'", Convert.ToString(hsParams[WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "2"]));
                    blCustCheck = true;
                }
                if (blCustCheck)
                {
                    sqlCommand.Append(" and t2.CC_DATA_GROUP='1' ");
                }

                //测试时间
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "1"))
                {
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_TTIME + ">='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_TTIME + "1"]));
                }
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "2"))
                {
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_TTIME + "<='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_TTIME + "2"]));
                }
                if (hsParams.ContainsKey(WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT))
                {
                    if (Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT]).Equals("1"))
                    {
                        sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT + "='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT]));
                        blCustCheck = false;
                    }
                }
                string sql = string.Empty;
                if (blCustCheck)
                {
                    sqlCommand.AppendFormat(" and t." + WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT + "='{0}'", Convert.ToString(hsParams[WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT]));
                }

                this._dbRead.LoadDataSet(CommandType.Text, sqlCommand.ToString(), dsReturn, new string[] { WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME });


                dsReturn.Tables[0].Columns["CREATE_TIME"].DateTimeMode = DataSetDateTime.Unspecified;
                dsReturn.Tables[0].Columns["TTIME"].DateTimeMode = DataSetDateTime.Unspecified;

                #endregion

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetIvTestDataForCustCheckQuery Error: " + ex.Message);
            }
            return dsReturn;
        }


        #region 包装清单打印查询 qym
        //包装清单打印查询1 qym
        public DataSet GetPackingListConergyData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                        @"IF EXISTS(SELECT 1
                                  FROM WIP_CONSIGNMENT a
                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                  WHERE  a.ISFLAG=1
                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                        BEGIN
                            SELECT  'Astronergy' AS SupplierName,
                                     D.PS_SUBCODE AS 'ARTNO',
                                     'Conergy PH ' + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + SUBSTRING(D.PS_CODE,CHARINDEX('-',D.PS_CODE)-1,1) AS 'ARTTXT',
                                     SUBSTRING(D.PS_CODE,1, CHARINDEX('-',D.PS_CODE)-1) AS 'ORIG_TYPE',
                                     '''' + B.LOT_NUMBER as 'SERIALNO',
                                     CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                     CAST(C.COEF_VMAX as decimal(6,2)) as 'VMPP',
                                     CAST(C.COEF_IMAX as decimal(6,2)) as 'IMPP',
                                     CAST(C.COEF_VOC as decimal(6,2)) as 'VOC',
                                     CAST(C.COEF_ISC as decimal(6,2)) as 'ISC',
                                     CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                     CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W' as 'pnom',
                                     F.CONTAINER_NO AS 'Container Number',
                                     CONVERT(char(10),F.SHIPMENT_DATE,120) AS 'SHIPPING_DATE',
                                     A.VIRTUAL_PALLET_NO 
                            FROM WIP_CONSIGNMENT A 
                            LEFT JOIN POR_LOT B ON A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.DELETED_TERM_FLAG<'2' 
                            LEFT JOIN WIP_IV_TEST C ON B.LOT_NUMBER=C.LOT_NUM AND C.VC_DEFAULT='1' 
                            LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                       AND D.PART_NUMBER=B.PART_NUMBER
                                                       AND D.PS_CODE = C.VC_TYPE
                                                       AND D.PS_SEQ=C.I_IDE
                                                       AND D.IS_USED='Y'
                            LEFT JOIN WMS_SHIPMENT F ON B.PALLET_NO=F.PALLET_NO
                            WHERE A.ISFLAG='1'
                            AND A.CS_DATA_GROUP!='0' 
                            AND B.STATUS < 2
                            AND A.VIRTUAL_PALLET_NO IN ({0})
                        END
                        ELSE
                        BEGIN
                            SELECT  'Astronergy' AS SupplierName,
                                     D.PS_SUBCODE AS 'ARTNO',
                                     'Conergy PH ' + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + SUBSTRING(D.PS_CODE,CHARINDEX('-',D.PS_CODE)-1,1) AS 'ARTTXT',
                                     SUBSTRING(D.PS_CODE,1, CHARINDEX('-',D.PS_CODE)-1) AS 'ORIG_TYPE',
                                     '''' + B.LOT_NUMBER as 'SERIALNO',
                                     CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                     CAST(C.COEF_VMAX as decimal(6,2)) as 'VMPP',
                                     CAST(C.COEF_IMAX as decimal(6,2)) as 'IMPP',
                                     CAST(C.COEF_VOC as decimal(6,2)) as 'VOC',
                                     CAST(C.COEF_ISC as decimal(6,2)) as 'ISC',
                                     CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                     CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W' as 'pnom',
                                     F.CONTAINER_NO AS 'Container Number',
                                     CONVERT(char(10),F.SHIPMENT_DATE,120) AS 'SHIPPING_DATE',
                                     A.VIRTUAL_PALLET_NO 
                            FROM WIP_CONSIGNMENT A 
                            LEFT JOIN POR_LOT_HIS B ON A.VIRTUAL_PALLET_NO=B.PALLET_NO AND A.ISFLAG='1' AND B.DELETED_TERM_FLAG!='2' AND A.CS_DATA_GROUP!='0' 
                            LEFT JOIN WIP_IV_TEST C ON B.LOT_NUMBER=C.LOT_NUM AND C.VC_DEFAULT='1' 
                            LEFT JOIN BASE_POWERSET D ON C.I_IDE=D.PS_SEQ AND C.VC_TYPE=D.PS_CODE AND D.ISFLAG='1'
                            LEFT JOIN WMS_SHIPMENT F ON B.PALLET_NO=F.PALLET_NO
                            WHERE A.VIRTUAL_PALLET_NO IN ({0})
                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListConergyData Error: " + ex.Message);
            }
            return dsReturn;
        }
        //包装清单打印查询2 qym
        public DataSet GetPackingListSchuecoData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                        @"IF EXISTS(SELECT 1
                                  FROM WIP_CONSIGNMENT a
                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                  WHERE  a.ISFLAG=1
                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                        BEGIN
                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                                A.VIRTUAL_PALLET_NO,
                                CASE WHEN ISNULL(G.ARTICNO,'')!='' THEN G.ARTICNO ELSE F.PS_SUBCODE END AS 'ARTNO',
                                'MPE ' + CONVERT(varchar,CONVERT(int,F.PMAXSTAB)) + ' ' + ISNULL(E.CUSTMARK,'') + ' 09' as 'ARTTXT',C.VC_CUSTCODE,
                                CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                CAST(C.COEF_ISC as decimal(6,2)) as 'ISC',
                                CAST(C.COEF_VOC as decimal(6,2)) as 'VOC',
                                CAST(C.COEF_IMAX as decimal(6,2)) as 'IMPP',
                                CAST(C.COEF_VMAX as decimal(6,2)) as 'VMPP',
                                CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) FROM POR_LOT H WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE'
                            FROM WIP_CONSIGNMENT A
                            INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<'2'
                            INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT='1'
                            LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                    AND E.PART_NUMBER=B.PART_NUMBER
                                                    AND E.IS_USED='Y'
                            LEFT JOIN POR_WO_PRD_PS F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                       AND F.PART_NUMBER=B.PART_NUMBER
                                                       AND F.PS_CODE = C.VC_TYPE
                                                       AND F.PS_SEQ=C.I_IDE
                                                       AND F.IS_USED='Y'
                            LEFT JOIN POR_WO_PRD_PS_CLR G ON G.POWERSET_KEY=F.POWERSET_KEY 
                                                            AND G.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                            AND G.PART_NUMBER=B.PART_NUMBER
                                                            AND G.COLOR_CODE=B.COLOR 
                                                            AND G.IS_USED='Y'
                            WHERE A.ISFLAG='1' 
                            AND A.CS_DATA_GROUP>'0'
                            AND B.STATUS < 2
                            AND A.VIRTUAL_PALLET_NO IN ({0});
                        END
                        ELSE
                        BEGIN
                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                                A.VIRTUAL_PALLET_NO,
                                CASE WHEN ISNULL(G.ARTICNO,'')!='' THEN G.ARTICNO ELSE F.PS_SUBCODE END AS 'ARTNO',
                                'MPE ' + CONVERT(varchar,CONVERT(int,F.PMAXSTAB)) + ' ' + ISNULL(D.MEMO1,'') + ' 09' as 'ARTTXT',C.VC_CUSTCODE,
                                CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                CAST(C.COEF_ISC as decimal(6,2)) as 'ISC',
                                CAST(C.COEF_VOC as decimal(6,2)) as 'VOC',
                                CAST(C.COEF_IMAX as decimal(6,2)) as 'IMPP',
                                CAST(C.COEF_VMAX as decimal(6,2)) as 'VMPP',
                                CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) FROM POR_LOT H WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE'
                            FROM WIP_CONSIGNMENT A
                            INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG!='2'
                            INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT='1'
                            INNER JOIN POR_PRODUCT D ON D.PRODUCT_CODE=B.PRO_ID AND D.ISFLAG='1'
                            INNER JOIN BASE_TESTRULE E ON E.TESTRULE_CODE=D.PRO_TEST_RULE AND E.ISFLAG='1'
                            INNER JOIN BASE_POWERSET F ON F.PS_CODE=E.PS_CODE AND F.PS_SEQ=C.I_IDE AND F.ISFLAG='1' 
                            LEFT JOIN BASE_POWERSET_COLORATCNO G ON G.POWERSET_KEY=F.POWERSET_KEY AND G.COLOR_CODE=B.COLOR AND G.ISFLAG='1'
                            WHERE A.ISFLAG='1' 
                            AND A.CS_DATA_GROUP!='0'
                            AND A.VIRTUAL_PALLET_NO IN ({0});
                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListSchuecoData Error: " + ex.Message);
            }
            return dsReturn;
        }
        //包装清单打印查询3 qym
        public DataSet GetPackingListCommonData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //                string sql = string.Format(
                //                       @"IF EXISTS(SELECT 1
                //                                  FROM WIP_CONSIGNMENT a
                //                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                //                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                //                                  WHERE  a.ISFLAG=1
                //                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                //                        BEGIN
                //                            SELECT ad.ITEM_NO AS 'NO',
                //                                   A.VIRTUAL_PALLET_NO,
                //                                   SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) AS 'TYPE','''' + B.LOT_NUMBER AS 'SERIALNO',
                //                                   CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                //                                   CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                //                                   CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                //                                   CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                //                                   CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                //                                   CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) AS 'PNOM',
                //                                   K.GRADE_NAME AS 'GRADE',
                //                                   CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                //                                                     FROM POR_LOT H 
                //                                                     WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                //                                   E.JUNCTION_BOX AS 'BOX',
                //                                   RTRIM(LTRIM(F.POWERLEVEL)) AS 'IMPPLEVEL',
                //                                   'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'
                //                            FROM WIP_CONSIGNMENT A
                //                            INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                //                            INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                //                            INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                //                            LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                //                                                    AND E.PART_NUMBER=B.PART_NUMBER
                //                                                    AND E.IS_USED='Y'
                //                            LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                //                                                       AND D.PART_NUMBER=B.PART_NUMBER
                //                                                       AND D.PS_CODE = C.VC_TYPE
                //                                                       AND D.PS_SEQ=C.I_IDE
                //                                                       AND D.IS_USED='Y'
                //                            LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                //                                                       AND F.PART_NUMBER=B.PART_NUMBER
                //                                                       AND F.POWERSET_KEY = D.POWERSET_KEY
                //                                                       AND F.PS_SUB_CODE=C.I_PKID
                //                                                       AND F.IS_USED='Y'
                //                            LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                //                            LEFT JOIN dbo.POR_WO_OEM P ON P.ORDER_NUMBER = A.WORKNUMBER 
                //                            AND P.IS_USED = 'Y'
                //                            WHERE A.ISFLAG='1' 
                //                            AND A.CS_DATA_GROUP>'0'
                //                            AND A.VIRTUAL_PALLET_NO IN ({0})
                //                            AND B.STATUS < 2
                //                            ORDER BY ad.ITEM_NO;
                //                        END
                //                        ELSE
                //                        BEGIN
                //                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                //                                   A.VIRTUAL_PALLET_NO,
                //                                   SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) AS 'TYPE','''' + B.LOT_NUMBER AS 'SERIALNO',
                //                                   CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                //                                   CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                //                                   CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                //                                   CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                //                                   CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                //                                   CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) AS 'PNOM',
                //                                   K.GRADE_NAME AS 'GRADE',
                //                                   CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                //                                                     FROM POR_LOT H 
                //                                                     WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                //                                   E.JUNCTION_BOX AS 'BOX',
                //                                  (SELECT rtrim(ltrim(F.POWERLEVEL)) 
                //                                   FROM BASE_POWERSET_DETAIL F 
                //                                   WHERE F.POWERSET_KEY=D.POWERSET_KEY 
                //                                   AND F.PS_DTL_SUBCODE=C.I_PKID
                //                                   AND F.ISFLAG='1') AS 'IMPPLEVEL',
                //                                   'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'
                //                            FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,POR_PRODUCT E,V_PRODUCTGRADE K,dbo.POR_WO_OEM P
                //                            WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.LOT_NUMBER=C.LOT_NUM AND C.VC_TYPE=D.PS_CODE AND C.I_IDE=D.PS_SEQ
                //                            AND B.PRO_ID=E.PRODUCT_CODE 
                //                            AND B.PRO_LEVEL=K.GRADE_CODE
                //                            AND A.ISFLAG='1' 
                //                            AND A.CS_DATA_GROUP!='0' 
                //                            AND B.DELETED_TERM_FLAG!='2' 
                //                            AND C.VC_DEFAULT='1'
                //                            AND D.ISFLAG='1' 
                //                            AND E.ISFLAG='1'
                //                            AND A.VIRTUAL_PALLET_NO IN ({0})
                //                            AND P.ORDER_NUMBER = A.WORKNUMBER 
                //                            AND P.IS_USED = 'Y'
                //                            ORDER BY NO;
                //                        END",
                //                        sPalltNo);

                //CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) AS 'PNOM',
                string sql = string.Format(
                       @"IF EXISTS(SELECT 1
                                                  FROM WIP_CONSIGNMENT a
                                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                  WHERE  a.ISFLAG=1
                                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                                        BEGIN
                                                 SELECT ad.ITEM_NO AS 'NO',
                                                       A.VIRTUAL_PALLET_NO,
                                                       CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(varchar,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
                                                       + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
                                                       ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                       '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                       CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                       (CASE 
                                                        WHEN D.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 
                                                            THEN  CONVERT(varchar,CONVERT(varchar,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                                WHEN D.SUB_PS_WAY = '功率' AND ISNULL(F.PS_SUB_CODE,0)>0 
														    THEN  CONVERT(varchar,CONVERT(varchar,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
                                                       ELSE CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) END) AS 'PNOM',
                                                       K.GRADE_NAME AS 'GRADE',
                                                       CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                         FROM POR_LOT H 
                                                                         WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                       E.JUNCTION_BOX AS 'BOX',
                                                       RTRIM(LTRIM(F.POWERLEVEL)) AS 'IMPPLEVEL',
                                                       'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'								   
                                                FROM WIP_CONSIGNMENT A
                                                INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                        AND E.PART_NUMBER=B.PART_NUMBER
                                                                        AND E.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND D.PART_NUMBER=B.PART_NUMBER
                                                                           AND D.PS_CODE = C.VC_TYPE
                                                                           AND D.PS_SEQ=C.I_IDE
                                                                           AND D.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND F.PART_NUMBER=B.PART_NUMBER
                                                                           AND F.POWERSET_KEY = D.POWERSET_KEY
                                                                           AND F.PS_SUB_CODE=C.I_PKID
                                                                           AND F.IS_USED='Y'
                                                LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                LEFT JOIN dbo.POR_WO_OEM P ON P.ORDER_NUMBER = A.WORKNUMBER 
                                                AND P.IS_USED = 'Y'
                                                LEFT JOIN dbo.POR_WO_OEM G ON G.ORDER_NUMBER = B.WORK_ORDER_NO
							                                                  AND G.IS_USED = 'Y'
                                                WHERE A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP>'0'
                                                AND A.VIRTUAL_PALLET_NO IN ({0})
                                                AND B.STATUS < 2
                                                ORDER BY ad.ITEM_NO;
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                                                   A.VIRTUAL_PALLET_NO,
                                                   CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(varchar,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
												   + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
												   ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                   '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                   CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                   CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                   CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                   CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                   CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                   CONVERT(varchar,CONVERT(varchar,D.PMAXSTAB)) AS 'PNOM',
                                                   K.GRADE_NAME AS 'GRADE',
                                                   CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                     FROM POR_LOT H 
                                                                     WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                   E.JUNCTION_BOX AS 'BOX',
                                                  (SELECT rtrim(ltrim(F.POWERLEVEL)) 
                                                   FROM BASE_POWERSET_DETAIL F 
                                                   WHERE F.POWERSET_KEY=D.POWERSET_KEY 
                                                   AND F.PS_DTL_SUBCODE=C.I_PKID
                                                   AND F.ISFLAG='1') AS 'IMPPLEVEL',
                                                   'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'
                                            FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,POR_PRODUCT E,V_PRODUCTGRADE K,dbo.POR_WO_OEM P,dbo.POR_WO_OEM G
                                            WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.LOT_NUMBER=C.LOT_NUM AND C.VC_TYPE=D.PS_CODE AND C.I_IDE=D.PS_SEQ
                                            AND B.PRO_ID=E.PRODUCT_CODE 
                                            AND B.PRO_LEVEL=K.GRADE_CODE
                                            AND A.ISFLAG='1' 
                                            AND A.CS_DATA_GROUP!='0' 
                                            AND B.DELETED_TERM_FLAG!='2' 
                                            AND C.VC_DEFAULT='1'
                                            AND D.ISFLAG='1' 
                                            AND E.ISFLAG='1'
                                            AND A.VIRTUAL_PALLET_NO IN ({0})
                                            AND P.ORDER_NUMBER = A.WORKNUMBER 
                                            AND P.IS_USED = 'Y'
                                            AND G.ORDER_NUMBER = B.WORK_ORDER_NO
                                            AND G.IS_USED = 'Y'
                                            ORDER BY NO;
                                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListCommonData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPackingListCommonDataSTS(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                       @"IF EXISTS(SELECT 1
                                                  FROM WIP_CONSIGNMENT a
                                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                  WHERE  a.ISFLAG=1
                                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                                        BEGIN
                                                 SELECT ad.ITEM_NO AS 'NO',
                                                       A.VIRTUAL_PALLET_NO,
                                                       CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
                                                       + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
                                                       ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                       '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                       CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                      
                                                       (CASE
                                                        WHEN D.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 
                                                            THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                                WHEN D.SUB_PS_WAY = '功率' AND ISNULL(F.PS_SUB_CODE,0)>0 
														    THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                               ELSE CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) END) AS 'PNOM',
                                                       K.GRADE_NAME AS 'GRADE',
                                                       CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                         FROM POR_LOT H 
                                                                         WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                       E.JUNCTION_BOX AS 'BOX',
                                                       CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                                       RTRIM(LTRIM(F.POWERLEVEL)) AS 'IMPPLEVEL',
                                                       'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'								   
                                                FROM WIP_CONSIGNMENT A
                                                INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                        AND E.PART_NUMBER=B.PART_NUMBER
                                                                        AND E.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND D.PART_NUMBER=B.PART_NUMBER
                                                                           AND D.PS_CODE = C.VC_TYPE
                                                                           AND D.PS_SEQ=C.I_IDE
                                                                           AND D.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND F.PART_NUMBER=B.PART_NUMBER
                                                                           AND F.POWERSET_KEY = D.POWERSET_KEY
                                                                           AND F.PS_SUB_CODE=C.I_PKID
                                                                           AND F.IS_USED='Y'
                                                LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                LEFT JOIN dbo.POR_WO_OEM P ON P.ORDER_NUMBER = A.WORKNUMBER 
                                                AND P.IS_USED = 'Y'
                                                LEFT JOIN dbo.POR_WO_OEM G ON G.ORDER_NUMBER = B.WORK_ORDER_NO
							                                                  AND G.IS_USED = 'Y'
                                                WHERE A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP>'0'
                                                AND A.VIRTUAL_PALLET_NO IN ({0})
                                                AND B.STATUS < 2
                                                ORDER BY ad.ITEM_NO;
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                                                   A.VIRTUAL_PALLET_NO,
                                                   CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
												   + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
												   ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                   '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                   CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                   CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                   CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                   CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                   CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                   CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) AS 'PNOM',
                                                   K.GRADE_NAME AS 'GRADE',
                                                   CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                     FROM POR_LOT H 
                                                                     WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                   E.JUNCTION_BOX AS 'BOX',
                                                  (SELECT rtrim(ltrim(F.POWERLEVEL)) 
                                                   FROM BASE_POWERSET_DETAIL F 
                                                   WHERE F.POWERSET_KEY=D.POWERSET_KEY 
                                                   AND F.PS_DTL_SUBCODE=C.I_PKID
                                                   AND F.ISFLAG='1') AS 'IMPPLEVEL',
                                                   'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'
                                            FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,POR_PRODUCT E,V_PRODUCTGRADE K,dbo.POR_WO_OEM P,dbo.POR_WO_OEM G
                                            WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.LOT_NUMBER=C.LOT_NUM AND C.VC_TYPE=D.PS_CODE AND C.I_IDE=D.PS_SEQ
                                            AND B.PRO_ID=E.PRODUCT_CODE 
                                            AND B.PRO_LEVEL=K.GRADE_CODE
                                            AND A.ISFLAG='1' 
                                            AND A.CS_DATA_GROUP!='0' 
                                            AND B.DELETED_TERM_FLAG!='2' 
                                            AND C.VC_DEFAULT='1'
                                            AND D.ISFLAG='1' 
                                            AND E.ISFLAG='1'
                                            AND A.VIRTUAL_PALLET_NO IN ({0})
                                            AND P.ORDER_NUMBER = A.WORKNUMBER 
                                            AND P.IS_USED = 'Y'
                                            AND G.ORDER_NUMBER = B.WORK_ORDER_NO
                                            AND G.IS_USED = 'Y'
                                            ORDER BY NO;
                                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListCommonData Error: " + ex.Message);
            }
            return dsReturn;
        }



        public DataSet GetPackingListCommonDataAiji(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                       @"IF EXISTS(SELECT 1
                                                  FROM WIP_CONSIGNMENT a
                                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                  WHERE  a.ISFLAG=1
                                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                                        BEGIN
                                                 SELECT ad.ITEM_NO AS 'NO',
                                                       A.VIRTUAL_PALLET_NO,
                                                       CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
                                                       + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
                                                       ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                       '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                       CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                      
                                                       (CASE
                                                        WHEN D.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 
                                                            THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                                WHEN D.SUB_PS_WAY = '功率' AND ISNULL(F.PS_SUB_CODE,0)>0 
														    THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                               ELSE CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) END) AS 'PNOM',
                                                       CAST(C. RS as decimal(6,2)) AS  'RS' ,
		                                               CAST(C. RSH as decimal(8,2)) AS  'RSH' ,
                                                       K.GRADE_NAME AS 'GRADE',
                                                       CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                         FROM POR_LOT H 
                                                                         WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                       E.JUNCTION_BOX AS 'BOX',
                                                       CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                                       CAST(C.INTENSITY as decimal(6,2)) as 'INTENSITY',
                                                       RTRIM(LTRIM(F.POWERLEVEL)) AS 'IMPPLEVEL',
                                                       'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'								   
                                                FROM WIP_CONSIGNMENT A
                                                INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                        AND E.PART_NUMBER=B.PART_NUMBER
                                                                        AND E.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND D.PART_NUMBER=B.PART_NUMBER
                                                                           AND D.PS_CODE = C.VC_TYPE
                                                                           AND D.PS_SEQ=C.I_IDE
                                                                           AND D.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND F.PART_NUMBER=B.PART_NUMBER
                                                                           AND F.POWERSET_KEY = D.POWERSET_KEY
                                                                           AND F.PS_SUB_CODE=C.I_PKID
                                                                           AND F.IS_USED='Y'
                                                LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                LEFT JOIN dbo.POR_WO_OEM P ON P.ORDER_NUMBER = A.WORKNUMBER 
                                                AND P.IS_USED = 'Y'
                                                LEFT JOIN dbo.POR_WO_OEM G ON G.ORDER_NUMBER = B.WORK_ORDER_NO
							                                                  AND G.IS_USED = 'Y'
                                                WHERE A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP>'0'
                                                AND A.VIRTUAL_PALLET_NO IN ({0})
                                                AND B.STATUS < 2
                                                ORDER BY ad.ITEM_NO;
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                                                   A.VIRTUAL_PALLET_NO,
                                                   CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
												   + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
												   ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                   '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                   CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                   CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                   CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                   CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                   CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                   CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) AS 'PNOM',
                                                   CAST(C. RS as decimal(6,2)) AS  'RS' ,
		                                           CAST(C. RSH as decimal(8,2)) AS  'RSH' ,
                                                   K.GRADE_NAME AS 'GRADE',
                                                   CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                     FROM POR_LOT H 
                                                                     WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                   E.JUNCTION_BOX AS 'BOX',
                                                   CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                                   CAST(C.INTENSITY as decimal(6,2)) as 'INTENSITY',
                                                  (SELECT rtrim(ltrim(F.POWERLEVEL)) 
                                                   FROM BASE_POWERSET_DETAIL F 
                                                   WHERE F.POWERSET_KEY=D.POWERSET_KEY 
                                                   AND F.PS_DTL_SUBCODE=C.I_PKID
                                                   AND F.ISFLAG='1') AS 'IMPPLEVEL',
                                                   'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'
                                            FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,POR_PRODUCT E,V_PRODUCTGRADE K,dbo.POR_WO_OEM P,dbo.POR_WO_OEM G
                                            WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.LOT_NUMBER=C.LOT_NUM AND C.VC_TYPE=D.PS_CODE AND C.I_IDE=D.PS_SEQ
                                            AND B.PRO_ID=E.PRODUCT_CODE 
                                            AND B.PRO_LEVEL=K.GRADE_CODE
                                            AND A.ISFLAG='1' 
                                            AND A.CS_DATA_GROUP!='0' 
                                            AND B.DELETED_TERM_FLAG!='2' 
                                            AND C.VC_DEFAULT='1'
                                            AND D.ISFLAG='1' 
                                            AND E.ISFLAG='1'
                                            AND A.VIRTUAL_PALLET_NO IN ({0})
                                            AND P.ORDER_NUMBER = A.WORKNUMBER 
                                            AND P.IS_USED = 'Y'
                                            AND G.ORDER_NUMBER = B.WORK_ORDER_NO
                                            AND G.IS_USED = 'Y'
                                            ORDER BY NO;
                                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListCommonAiji Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPackingListCommonDataQDCS(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                       @"IF EXISTS(SELECT 1
                                                  FROM WIP_CONSIGNMENT a
                                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                  WHERE  a.ISFLAG=1
                                                  AND a.VIRTUAL_PALLET_NO IN ({0}))
                                        BEGIN
                                                 SELECT ad.ITEM_NO AS 'NO',
                                                       A.VIRTUAL_PALLET_NO,
                                                       CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
                                                       + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
                                                       ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                       '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                       CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                      
                                                       (CASE
                                                        WHEN D.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 
                                                            THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                                WHEN D.SUB_PS_WAY = '功率' AND ISNULL(F.PS_SUB_CODE,0)>0 
														    THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                               ELSE CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) END) AS 'PNOM',
                                                       K.GRADE_NAME AS 'GRADE',
                                                       CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                         FROM POR_LOT H 
                                                                         WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                       E.JUNCTION_BOX AS 'BOX',
                                                       CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                                       CAST(C.INTENSITY as decimal(6,2)) as 'INTENSITY',
                                                       RTRIM(LTRIM(F.POWERLEVEL)) AS 'IMPPLEVEL',
                                                       'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'								   
                                                FROM WIP_CONSIGNMENT A
                                                INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                        AND E.PART_NUMBER=B.PART_NUMBER
                                                                        AND E.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND D.PART_NUMBER=B.PART_NUMBER
                                                                           AND D.PS_CODE = C.VC_TYPE
                                                                           AND D.PS_SEQ=C.I_IDE
                                                                           AND D.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND F.PART_NUMBER=B.PART_NUMBER
                                                                           AND F.POWERSET_KEY = D.POWERSET_KEY
                                                                           AND F.PS_SUB_CODE=C.I_PKID
                                                                           AND F.IS_USED='Y'
                                                LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                LEFT JOIN dbo.POR_WO_OEM P ON P.ORDER_NUMBER = A.WORKNUMBER 
                                                AND P.IS_USED = 'Y'
                                                LEFT JOIN dbo.POR_WO_OEM G ON G.ORDER_NUMBER = B.WORK_ORDER_NO
							                                                  AND G.IS_USED = 'Y'
                                                WHERE A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP>'0'
                                                AND A.VIRTUAL_PALLET_NO IN ({0})
                                                AND B.STATUS < 2
                                                ORDER BY ad.ITEM_NO;
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT ROW_NUMBER()over(order by B.LOT_NUMBER) AS 'NO',
                                                   A.VIRTUAL_PALLET_NO,
                                                   CASE G.CUSROMER WHEN 'SE' THEN G.CUSROMER + '-' + G.CELL_TYPE + CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + G.STRUCTURE_PARAM + G.PLACE_ORIGIN
												   + G.GLASS_TYPE + '-' + G.BOM_AUTHENTICATION_CODE + G.JUNCTION_BOX
												   ELSE SUBSTRING(D.PS_CODE,1,CHARINDEX('-',D.PS_CODE)-1) END AS 'TYPE',
                                                   '''' + B.LOT_NUMBER AS 'SERIALNO',
                                                   CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                   CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                   CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                   CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                   CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                   CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) AS 'PNOM',
                                                   K.GRADE_NAME AS 'GRADE',
                                                   CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
                                                                     FROM POR_LOT H 
                                                                     WHERE H.PALLET_NO=A.VIRTUAL_PALLET_NO),120) AS 'PRODUCT_DATE',
                                                   E.JUNCTION_BOX AS 'BOX',
                                                   CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                                   CAST(C.INTENSITY as decimal(6,2)) as 'INTENSITY',
                                                  (SELECT rtrim(ltrim(F.POWERLEVEL)) 
                                                   FROM BASE_POWERSET_DETAIL F 
                                                   WHERE F.POWERSET_KEY=D.POWERSET_KEY 
                                                   AND F.PS_DTL_SUBCODE=C.I_PKID
                                                   AND F.ISFLAG='1') AS 'IMPPLEVEL',
                                                   'M'+P.CELL_SUPPLIER+P.CELL_MODEL+ P.SE_MODULE_TYPE +P.PLACE_ORIGIN+P.BOM_DESIGN AS 'Type2'
                                            FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,POR_PRODUCT E,V_PRODUCTGRADE K,dbo.POR_WO_OEM P,dbo.POR_WO_OEM G
                                            WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.LOT_NUMBER=C.LOT_NUM AND C.VC_TYPE=D.PS_CODE AND C.I_IDE=D.PS_SEQ
                                            AND B.PRO_ID=E.PRODUCT_CODE 
                                            AND B.PRO_LEVEL=K.GRADE_CODE
                                            AND A.ISFLAG='1' 
                                            AND A.CS_DATA_GROUP!='0' 
                                            AND B.DELETED_TERM_FLAG!='2' 
                                            AND C.VC_DEFAULT='1'
                                            AND D.ISFLAG='1' 
                                            AND E.ISFLAG='1'
                                            AND A.VIRTUAL_PALLET_NO IN ({0})
                                            AND P.ORDER_NUMBER = A.WORKNUMBER 
                                            AND P.IS_USED = 'Y'
                                            AND G.ORDER_NUMBER = B.WORK_ORDER_NO
                                            AND G.IS_USED = 'Y'
                                            ORDER BY NO;
                                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListCommonData Error: " + ex.Message);
            }
            return dsReturn;
        }

        //包装清单打印查询Japan数据
        public DataSet GetPackingListJapanData(string sPalltNo, string sCINumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = @"SELECT  
                        ROW_NUMBER()over(order by A.PALLET_NO, A.LOT_NUMBER) AS 'NO',
                        A.PALLET_NO AS 'PALLET_NO',
                        CHAR(39) + LOT_NUMBER AS LOT_NUMBER,
                        CAST(C.COEF_VOC as decimal(6,2)) AS COEF_VOC,
                        CAST(C.COEF_ISC as decimal(6,2)) AS COEF_ISC,
                        CAST(C.COEF_VMAX as decimal(6,2)) AS COEF_VMAX,
                        CAST(C.COEF_IMAX as decimal(6,2)) AS COEF_IMAX,
                        CAST(C.COEF_PMAX as decimal(6,2)) AS COEF_PMAX,
                        CONVERT(varchar,CAST(C.COEF_FF*100 as decimal(6,2))) + '%'  AS COEF_FF,
                        'OK' AS 'Facade','合格' AS 'IsOK',
                        CONVERT(char(10),(SELECT MIN(H.PALLET_TIME) 
				                          FROM POR_LOT H 
				                          WHERE H.PALLET_NO=A.PALLET_NO),120) AS 'PRODUCT_DATE'
                        FROM POR_LOT A 
                        LEFT JOIN  WIP_IV_TEST C ON A.LOT_NUMBER = C.LOT_NUM AND C.VC_DEFAULT = 1
                        LEFT JOIN WMS_SHIPMENT D ON A.PALLET_NO = D.PALLET_NO AND D.IS_FLAG =1";
                sql = sql + " WHERE 1 = 1 ";
                if (!string.IsNullOrEmpty(sPalltNo))
                {
                    sql += " AND A.PALLET_NO IN (" + sPalltNo + ")";
                }
                if (!string.IsNullOrEmpty(sCINumber))
                {
                    sql += " AND D.CI_NO IN (" + sCINumber + ")";
                }
                sql += " ORDER BY NO ASC";
                DataTable dtFlashData = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtFlashData.TableName = "FlashData";

                dsReturn.Merge(dtFlashData, true, MissingSchemaAction.Add);

                if (!string.IsNullOrEmpty(sCINumber))
                {
                    sql = string.Format(@"IF EXISTS(SELECT 1
                                                  FROM WMS_SHIPMENT a
                                                  INNER JOIN WIP_CONSIGNMENT b ON b.VIRTUAL_PALLET_NO=a.PALLET_NO AND b.ISFLAG=1
                                                  INNER JOIN POR_WORK_ORDER c ON c.ORDER_NUMBER=b.WORKNUMBER
                                                  INNER JOIN POR_WO_PRD d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY AND d.IS_USED='Y'
                                                  AND a.CI_NO IN ({0}))
                                        BEGIN
                                            SELECT  a.VIRTUAL_PALLET_NO,a.LOT_NUMBER_QTY,a.SAP_NO,a.WORKNUMBER,a.GRADE,
                                                    a.CS_DATA_GROUP,a.PRO_ID,a.POWER_LEVEL,a.TOTLE_POWER,E.ARTICNO,0 AS HFLAG,
                                                    b.*
                                            FROM WIP_CONSIGNMENT a
                                            INNER JOIN POR_WORK_ORDER c ON c.ORDER_NUMBER=a.WORKNUMBER
                                            LEFT JOIN POR_WO_PRD_PS d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY
                                                                      AND d.PART_NUMBER=a.SAP_NO
                                                                      AND d.PS_CODE=a.PS_CODE
                                                                      AND d.PMAXSTAB=a.POWER_LEVEL
                                                                      AND d.IS_USED='Y'
                                            LEFT JOIN POR_WO_PRD_PS_CLR e ON e.WORK_ORDER_KEY=c.WORK_ORDER_KEY
                                                                          AND e.PART_NUMBER=a.SAP_NO
                                                                          AND e.POWERSET_KEY=d.POWERSET_KEY
                                                                          AND e.COLOR_NAME=a.LOT_COLOR
                                                                          AND e.IS_USED='Y'
                                            LEFT JOIN WMS_SHIPMENT b ON b.PALLET_NO=a.VIRTUAL_PALLET_NO
                                            WHERE a.CS_DATA_GROUP>2
                                            AND a.ISFLAG=1 
                                            AND b.CI_NO IN ({0});
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT  a.VIRTUAL_PALLET_NO,a.LOT_NUMBER_QTY,a.SAP_NO,a.WORKNUMBER,a.GRADE,
                                                    a.CS_DATA_GROUP,a.PRO_ID,a.POWER_LEVEL,a.TOTLE_POWER,E.ARTICNO,0 AS HFLAG,
                                                    b.*
                                            FROM WIP_CONSIGNMENT a
                                            LEFT JOIN WMS_SHIPMENT b ON b.PALLET_NO=a.VIRTUAL_PALLET_NO
                                            LEFT JOIN 
                                            (SELECT DISTINCT C.PS_CODE,C.PMAXSTAB,D.COLOR_NAME,D.ARTICNO 
                                            FROM BASE_POWERSET C 
                                            LEFT JOIN BASE_POWERSET_COLORATCNO D ON d.POWERSET_KEY=c.POWERSET_KEY AND C.ISFLAG = 1 AND D.ISFLAG=1 
                                            ) E ON E.PS_CODE=a.PS_CODE AND E.PMAXSTAB=a.POWER_LEVEL AND E.COLOR_NAME=A.LOT_COLOR
                                            WHERE a.CS_DATA_GROUP>2
                                            AND a.ISFLAG=1 
                                            AND b.CI_NO IN ({0});
                                        END", sCINumber);

                    DataTable dtShipment = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    dtShipment.TableName = "Shipment";

                    dsReturn.Merge(dtShipment, true, MissingSchemaAction.Add);
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListCommonData Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        public DataSet GetPPSMasterData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                            @"IF EXISTS(SELECT 1
                                          FROM WIP_CONSIGNMENT a
                                          INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                          INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                          WHERE  a.ISFLAG=1
                                          AND a.VIRTUAL_PALLET_NO='{0}')
                                BEGIN
                                    SELECT B.PALLET_NO,B.PRO_ID,B.WORK_ORDER_NO,
                                           CONVERT(VARCHAR(10),B.PALLET_TIME,120) AS 'PRODUCT_DATE',
                                           B.LOT_NUMBER,C.VC_CUSTCODE,'21' + B.LOT_NUMBER AS 'BARCODEDATA',
                                           CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMP',
                                           CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                           CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                           CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMP',
                                           CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMP',
                                           CAST(C.COEF_FF*100 as decimal(6,2)) AS 'FF',
                                           CAST(C.COEF_PMAX as decimal(6,2)) AS 'POWERMAX',
                                           C.TTIME,
                                           D.PS_SUBCODE AS 'PID',
                                           'Conergy PH ' + D.MODULE_NAME AS 'ARTNUMBER',
                                           D.PS_CODE AS 'VC_TYPE',
                                           D.PS_RULE AS 'VC_TYPENAME',
                                           LEFT(D.MODULE_NAME,3) AS 'POWER',
                                           D.P_MIN,D.P_MAX,
                                           K.GRADE_NAME AS 'C_NAME',
                                           K.GRADE_NAME_DESC AS 'E_NAME',
                                           B.QUANTITY_INITIAL AS 'LOT_QTY'
                                    FROM WIP_CONSIGNMENT A
                                    LEFT JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                    LEFT JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                    LEFT JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                    LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                               AND D.PART_NUMBER=B.PART_NUMBER
                                                               AND D.PS_CODE=C.VC_TYPE
                                                               AND D.PS_SEQ=C.I_IDE
                                                               AND D.IS_USED='Y'
                                    LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                    WHERE A.ISFLAG='1' 
                                    AND A.CS_DATA_GROUP>'0' 
                                    AND A.VIRTUAL_PALLET_NO='{0}'
                                    AND B.STATUS < 2
                                    ORDER BY ad.ITEM_NO
                                END
                                ELSE
                                BEGIN
                                    SELECT B.PALLET_NO,B.PRO_ID,B.WORK_ORDER_NO,
                                           CONVERT(VARCHAR(10),B.PALLET_TIME,120) AS 'PRODUCT_DATE',
                                           B.LOT_NUMBER,C.VC_CUSTCODE,'21' + B.LOT_NUMBER AS 'BARCODEDATA',
                                           CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMP',
                                           CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                           CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                           CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMP',
                                           CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMP',
                                           CAST(C.COEF_FF*100 as decimal(6,2)) AS 'FF',
                                           CAST(C.COEF_PMAX as decimal(6,2)) AS 'POWERMAX',
                                           C.TTIME,
                                           D.PS_SUBCODE AS 'PID',
                                           'Conergy PH ' + D.MODULE_NAME AS 'ARTNUMBER',
                                           D.PS_CODE AS 'VC_TYPE',
                                           D.PS_RULE AS 'VC_TYPENAME',
                                           LEFT(D.MODULE_NAME,3) AS 'POWER',
                                           D.P_MIN,D.P_MAX,
                                           K.GRADE_NAME AS 'C_NAME',
                                           K.GRADE_NAME_DESC AS 'E_NAME',
                                           B.QUANTITY_INITIAL AS 'LOT_QTY'
                                    FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,V_PRODUCTGRADE K
                                    WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO 
                                    AND B.LOT_NUMBER=C.LOT_NUM 
                                    AND C.VC_TYPE=D.PS_CODE 
                                    AND C.I_IDE=D.PS_SEQ
                                    AND B.PRO_LEVEL = K.GRADE_CODE
                                    AND A.ISFLAG='1' 
                                    AND A.CS_DATA_GROUP!='0' 
                                    AND B.DELETED_TERM_FLAG!='2' 
                                    AND C.VC_DEFAULT='1' 
                                    AND D.ISFLAG='1'
                                    AND B.STATUS < 2
                                    AND A.VIRTUAL_PALLET_NO='{0}'
                                    ORDER BY B.LOT_NUMBER;
                                END",
                                sPalltNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPPSMasterData Error: " + ex.Message);
            }
            return dsReturn;
        }
        public DataSet GetPortMark(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT B.PORT_MARK
                                            FROM dbo.POR_WO_PRD B
                                            INNER JOIN dbo.POR_WORK_ORDER C ON C.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                            WHERE C.ORDER_NUMBER = (
	                                                SELECT A.WORKNUMBER
	                                                FROM WIP_CONSIGNMENT A
	                                                WHERE A.VIRTUAL_PALLET_NO = '{0}'
	                                                )
                                                AND B.PORT_MARK IS NOT NULL
                                                AND B.IS_USED='Y'", sPalltNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPPSMasterData Error: " + ex.Message);
            }
            return dsReturn;
        }

        //为体现标签/包装清单功率判断获取数据库数据 yibin.fei 2017.10.10
        public DataSet GetPowerShowData(string sWorkNo, string sSapNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                                      @"SELECT WORK_ORDER,
                                             RULE_CODE,
                                             BEFORE_POWER,
                                             AFTER_POWER,
                                             PART_NUMBER,
                                             IS_USED,
                                             VERSION_NO
                                       FROM POR_WO_PRD_POWERSHOW
                                       WHERE WORK_ORDER='{0}'
                                       AND PART_NUMBER='{1}'
                                       AND IS_USED='Y'
                                      
                                   
                                       ",
                              sWorkNo,sSapNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerShowData: " + ex.Message);
            }
            return dsReturn;
        }

        //add by 2018-7-30 begin
        public DataSet GetFlashDataDelivery(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                                              @"SELECT ad.ITEM_NO AS 'NO',
                                                       B.PRO_ID,
                                                       B.LOT_NUMBER AS 'SERIALNO',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMPP',
                                                       CAST(C.COEF_IMAX as decimal(6,2)) AS 'IMPP',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMPP',
                                                       CAST(C.COEF_FF*100 as decimal(6,2)) as 'FF',
                                                       '325' as wattMarking,
                                                        A.VIRTUAL_PALLET_NO as CartonNo,
                                                       'Pass' as InsResult,
                                                       CAST(C. RS as decimal(6,2)) AS  'RS' ,
		                                               CAST(C. RSH as decimal(8,2)) AS  'RSH' ,
                                                       (CASE
                                                        WHEN D.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 
                                                            THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                                WHEN D.SUB_PS_WAY = '功率' AND ISNULL(F.PS_SUB_CODE,0)>0 
														    THEN  CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
		                                               ELSE CONVERT(varchar,CONVERT(int,D.PMAXSTAB)) END) AS 'PNOM',
                                                       ' ' as InsBatch						   
                                                FROM WIP_CONSIGNMENT A
                                                INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                LEFT JOIN POR_WO_PRD E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                        AND E.PART_NUMBER=B.PART_NUMBER
                                                                        AND E.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND D.PART_NUMBER=B.PART_NUMBER
                                                                           AND D.PS_CODE = C.VC_TYPE
                                                                           AND D.PS_SEQ=C.I_IDE
                                                                           AND D.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND F.PART_NUMBER=B.PART_NUMBER
                                                                           AND F.POWERSET_KEY = D.POWERSET_KEY
                                                                           AND F.PS_SUB_CODE=C.I_PKID
                                                                           AND F.IS_USED='Y'
                                                LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                LEFT JOIN dbo.POR_WO_OEM P ON P.ORDER_NUMBER = A.WORKNUMBER 
                                                AND P.IS_USED = 'Y'
                                                LEFT JOIN dbo.POR_WO_OEM G ON G.ORDER_NUMBER = B.WORK_ORDER_NO
							                                                  AND G.IS_USED = 'Y'
                                                WHERE A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP>'0'
                                                AND A.VIRTUAL_PALLET_NO IN ({0})
                                                AND B.STATUS < 2
                                                ORDER BY ad.ITEM_NO;
                                        ",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPackingListCommonAiji Error: " + ex.Message);
            }
            return dsReturn;
        }

        //add by 2018-7-30 end


        public DataSet GetPPSCollectData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = string.Format(
                        @"IF EXISTS(SELECT 1
                                  FROM WIP_CONSIGNMENT a
                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                  WHERE  a.ISFLAG=1
                                  AND A.VIRTUAL_PALLET_NO IN ({0}))
                        BEGIN
                            SELECT A.VIRTUAL_PALLET_NO,
                                   A.EDIT_TIME,
                                   A.CREATER,
                                   A.SHIFT,
                                   A.SAP_NO,
                                   A.WORKNUMBER,
                                   B.PRO_ID,
                                   D.CERTIFICATION,
                                   D.TOLERANCE,
                                   D.JUNCTION_BOX,
                                   LEFT(E.MODULE_NAME,3) + 'W' AS 'POWER',
                                   MAX(E.PS_SUBCODE) AS 'PS_SUBCODE',
                                   E.SUB_PS_WAY,
                                   (CASE WHEN E.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 THEN 
                                   CONVERT(varchar,CONVERT(varchar,E.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,LEN(F.POWERLEVEL) - CHARINDEX('-',F.POWERLEVEL))) 
                                   ELSE F.POWERLEVEL END) AS POWERLEVEL,
                                   CAST(ISNULL(MAX(C.COEF_PMAX),0) AS DECIMAL(16,2)) AS 'PMAX',
                                   CAST(ISNULL(MIN(C.COEF_PMAX),0) AS DECIMAL(16,2)) AS 'PMIN',
                                   CAST(ISNULL(SUM(C.COEF_PMAX),0) AS DECIMAL(16,2)) AS 'PSUM',
                                   ISNULL(MAX(C.COEF_ISC),0) AS 'ISC_MAX',
                                   ISNULL(MIN(C.COEF_ISC),0) AS 'ISC_MIN',
                                   ISNULL(MAX(C.COEF_VOC),0) AS 'VOC_MAX',
                                   ISNULL(MIN(C.COEF_VOC),0) AS 'VOC_MIN',
                                   ISNULL(MAX(C.COEF_IMAX),0) AS 'IMP_MAX',
                                   ISNULL(MIN(C.COEF_IMAX),0) AS 'IMP_MIN',
                                   ISNULL(MAX(C.COEF_VMAX),0) AS 'VMP_MAX',
                                   ISNULL(MIN(C.COEF_VMAX),0) AS 'VMP_MIN',
                                   COUNT(B.LOT_NUMBER) AS 'QTY'
                            FROM WIP_CONSIGNMENT A
                            LEFT OUTER JOIN POR_LOT B ON A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.DELETED_TERM_FLAG<'2'
                            LEFT OUTER JOIN WIP_IV_TEST C ON B.LOT_NUMBER=C.LOT_NUM AND C.VC_DEFAULT='1'
                            LEFT JOIN POR_WO_PRD D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                       AND D.PART_NUMBER=B.PART_NUMBER
                                                       AND D.IS_USED='Y'
                            LEFT JOIN POR_WO_PRD_PS E ON E.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                       AND E.PART_NUMBER=B.PART_NUMBER
                                                       AND E.PS_CODE=C.VC_TYPE
                                                       AND E.PS_SEQ=C.I_IDE
                                                       AND E.IS_USED='Y'
                            LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=E.WORK_ORDER_KEY
                                                          AND F.PART_NUMBER=E.PART_NUMBER
                                                          AND F.POWERSET_KEY=E.POWERSET_KEY
                                                          AND F.PS_SUB_CODE=C.I_PKID
                                                          AND F.IS_USED='Y'
                            WHERE A.ISFLAG='1'
                            AND B.STATUS < 2
                            AND A.VIRTUAL_PALLET_NO IN ({0})
                            GROUP BY A.VIRTUAL_PALLET_NO,A.EDIT_TIME,A.CREATER,A.SHIFT,A.SAP_NO, A.WORKNUMBER,
                            B.PRO_ID,D.CERTIFICATION,D.TOLERANCE,D.JUNCTION_BOX,E.MODULE_NAME,E.SUB_PS_WAY,F.POWERLEVEL,SUB_PS_WAY,PS_SUB_CODE,PMAXSTAB;
                        END
                        ELSE
                        BEGIN
                            SELECT A.VIRTUAL_PALLET_NO,
                                   A.EDIT_TIME,
                                   A.CREATER,
                                   A.SHIFT,
                                   A.SAP_NO,
                                   A.WORKNUMBER,
                                   B.PRO_ID,
                                   
                                   D.CERTIFICATION,
                                   D.TOLERANCE,
                                   D.JUNCTION_BOX,
                                   LEFT(E.MODULE_NAME,3) + 'W' AS 'POWER',
                                   F.POWERLEVEL,
                                   CAST(ISNULL(MAX(C.COEF_PMAX),0) AS DECIMAL(16,2)) AS 'PMAX',
                                   CAST(ISNULL(MIN(C.COEF_PMAX),0) AS DECIMAL(16,2)) AS 'PMIN',
                                   CAST(ISNULL(SUM(C.COEF_PMAX),0) AS DECIMAL(16,2)) AS 'PSUM',
                                   ISNULL(MAX(C.COEF_ISC),0) AS 'ISC_MAX',
                                   ISNULL(MIN(C.COEF_ISC),0) AS 'ISC_MIN',
                                   ISNULL(MAX(C.COEF_VOC),0) AS 'VOC_MAX',
                                   ISNULL(MIN(C.COEF_VOC),0) AS 'VOC_MIN',
                                   ISNULL(MAX(C.COEF_IMAX),0) AS 'IMP_MAX',
                                   ISNULL(MIN(C.COEF_IMAX),0) AS 'IMP_MIN',
                                   ISNULL(MAX(C.COEF_VMAX),0) AS 'VMP_MAX',
                                   ISNULL(MIN(C.COEF_VMAX),0) AS 'VMP_MIN',
                                   COUNT(B.LOT_NUMBER) AS 'QTY'
                            FROM WIP_CONSIGNMENT A
                            LEFT OUTER JOIN POR_LOT B ON A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.DELETED_TERM_FLAG<'2'
                            LEFT OUTER JOIN WIP_IV_TEST C ON B.LOT_NUMBER=C.LOT_NUM AND C.VC_DEFAULT='1'
                            LEFT OUTER JOIN POR_PRODUCT D ON B.PRO_ID=D.PRODUCT_CODE AND D.ISFLAG='1'
                            LEFT OUTER JOIN BASE_POWERSET E ON C.VC_TYPE=E.PS_CODE AND C.I_IDE=E.PS_SEQ AND E.ISFLAG='1'
                            LEFT OUTER JOIN BASE_POWERSET_DETAIL F ON E.POWERSET_KEY=F.POWERSET_KEY AND C.I_PKID=F.PS_DTL_SUBCODE AND F.ISFLAG='1'
                            WHERE A.ISFLAG='1'
                            AND B.STATUS < 2
                            AND A.VIRTUAL_PALLET_NO IN ({0})
                            GROUP BY A.VIRTUAL_PALLET_NO,A.EDIT_TIME,A.CREATER,A.SHIFT,A.SAP_NO, A.WORKNUMBER,
                            B.PRO_ID,D.CERTIFICATION,D.TOLERANCE,D.JUNCTION_BOX,E.MODULE_NAME,F.POWERLEVEL;
                        END",
                        sPalltNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPPSCollectData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetCustCheckData(string sSN, string sCustCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM WIP_CUSTCHECK WHERE ISFLAG='1' AND CC_DATA_GROUP='1'";
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND CC_FCODE1='" + sSN + "'";
                }
                if (!string.IsNullOrEmpty(sCustCode))
                {
                    sql += " AND CUSTOMCODE='" + sCustCode + "'";
                }
                if (!string.IsNullOrEmpty(sRoomKey))
                {
                    sql += " AND ROOM_KEY='" + sRoomKey + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCustCheckData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetConsigmentDataBySN(string sSN, string sSideCode, string sCustomerCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM POR_LOT WHERE DELETED_TERM_FLAG!='2' AND ISNULL(PALLET_NO,'')!=''";
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND LOT_NUMBER='" + sSN + "'";
                }
                if (!string.IsNullOrEmpty(sSideCode))
                {
                    sql += " AND LOT_SIDECODE='" + sSideCode + "'";
                }
                if (!string.IsNullOrEmpty(sCustomerCode))
                {
                    sql += " AND LOT_CUSTOMERCODE='" + sCustomerCode + "'";
                }
                if (!string.IsNullOrEmpty(sRoomKey))
                {
                    sql += " AND FACTORYROOM_KEY='" + sRoomKey + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetConsigmentDataBySN Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetBasicData(string sColumnType, string sColumnCode, string sColumnName, string sColumnNameDesc)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT '' AS 'COLUMN_CODE','' AS 'COLUMN_NAME','' AS 'COLUMN_NAME_DESC'";
                sql += " UNION ";
                sql += "SELECT T.COLUMN_CODE,T.COLUMN_NAME,T.COLUMN_NAME_DESC";
                sql += " FROM (SELECT T.ITEM_ORDER";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Index' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Index'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_type' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_type'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_code' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_code'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name_Desc' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name_Desc'";
                sql += " FROM CRM_ATTRIBUTE  T,BASE_ATTRIBUTE  T1,BASE_ATTRIBUTE_CATEGORY T2";
                sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
                sql += " AND UPPER(T2.CATEGORY_NAME) = 'Basic_TestRule_PowerSet'";
                sql += " GROUP BY T.ITEM_ORDER) T";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sColumnType))
                {
                    sql += " AND T.Column_type='" + sColumnType + "'";
                }
                if (!string.IsNullOrEmpty(sColumnCode))
                {
                    sql += " AND T.Column_code='" + sColumnCode + "'";
                }
                if (!string.IsNullOrEmpty(sColumnName))
                {
                    sql += " AND T.Column_Name='" + sColumnName + "'";
                }
                if (!string.IsNullOrEmpty(sColumnNameDesc))
                {
                    sql += " AND T.Column_Name_Desc='" + sColumnNameDesc + "'";
                }
                sql += " ORDER BY  COLUMN_CODE ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBasicData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetReasonCategoryData(string sCategoryType, string sCategoryName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT '' AS 'REASON_CODE_CATEGORY_KEY','' AS 'REASON_CODE_CATEGORY_NAME'";
                sql += " UNION ";
                sql += "SELECT REASON_CODE_CATEGORY_KEY,REASON_CODE_CATEGORY_NAME";
                sql += " FROM FMM_REASON_CODE_CATEGORY";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sCategoryType))
                {
                    sql += " AND REASON_CODE_CATEGORY_TYPE='" + sCategoryType + "'";
                }
                if (!string.IsNullOrEmpty(sCategoryName))
                {
                    sql += " AND REASON_CODE_CATEGORY_NAME='" + sCategoryName + "'";
                }
                sql += " ORDER BY REASON_CODE_CATEGORY_NAME ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCategoryData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetReasonData(string sCategoryKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT B.REASON_CODE_KEY,B.REASON_CODE_NAME";
                sql += " FROM FMM_REASON_R_CATEGORY A,FMM_REASON_CODE B";
                sql += " WHERE A.REASON_CODE_KEY=B.REASON_CODE_KEY";
                if (!string.IsNullOrEmpty(sCategoryKey))
                {
                    sql += " AND A.CATEGORY_KEY='" + sCategoryKey + "'";
                }
                sql += " ORDER BY B.REASON_CODE_NAME ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetOQAData(string sSN, string sCustomCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT C.LOCATION_NAME,A.CC_FCODE1,A.CUSTOMCODE,A.CHECK_TIME,A.WORKNUMBER,A.PRO_ID,A.LOT_COLOR,B.COLUMN_NAME,A.SHIFT_NAME";
                sql += ",A.REASON_CODE_CATEGORY_NAME,A.REASON_CODE_NAME,A.CREATER,A.REMARK";
                sql += " FROM WIP_CUSTCHECK A";
                sql += " LEFT OUTER JOIN (SELECT T.COLUMN_CODE,T.COLUMN_NAME,T.COLUMN_NAME_DESC";
                sql += " FROM (SELECT T.ITEM_ORDER";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Index' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Index'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_type' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_type'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_code' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_code'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name_Desc' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name_Desc'";
                sql += " FROM CRM_ATTRIBUTE  T,BASE_ATTRIBUTE  T1,BASE_ATTRIBUTE_CATEGORY T2";
                sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
                sql += " AND UPPER(T2.CATEGORY_NAME) = 'Basic_TestRule_PowerSet'";
                sql += " GROUP BY T.ITEM_ORDER) T";
                sql += " WHERE T.Column_type='ProductGrade') B ON A.PRO_LEVEL=B.COLUMN_CODE";
                sql += " INNER JOIN FMM_LOCATION C ON A.ROOM_KEY=C.LOCATION_KEY AND C.LOCATION_LEVEL='5'";
                sql += " WHERE A.ISFLAG='1' AND A.CC_DATA_GROUP='1'";
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND A.CC_FCODE1='" + sSN + "'";
                }
                if (!string.IsNullOrEmpty(sCustomCode))
                {
                    sql += " AND A.CUSTOMCODE='" + sCustomCode + "'";
                }
                if (!string.IsNullOrEmpty(sRoomKey))
                {
                    sql += " AND A.ROOM_KEY='" + sRoomKey + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOQAData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetCustCheckSEQ(string sSN, string sCustomCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT ISNULL(MAX(l_ID),0)+1 AS 'SEQ'";
                sql += " FROM WIP_CUSTCHECK";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sSN))
                {
                    sql += " AND CC_FCODE1='" + sSN + "'";
                }
                if (!string.IsNullOrEmpty(sCustomCode))
                {
                    sql += " AND CUSTOMCODE='" + sCustomCode + "'";
                }
                if (!string.IsNullOrEmpty(sRoomKey))
                {
                    sql += " AND ROOM_KEY='" + sRoomKey + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetFactoryInfo()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT 'ALL' AS 'LOCATION_KEY','ALL' AS 'LOCATION_NAME'";
                sql += " UNION ";
                sql += "SELECT LOCATION_KEY,LOCATION_NAME FROM FMM_LOCATION WHERE LOCATION_LEVEL='5'";
                sql += " ORDER BY LOCATION_NAME ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFactoryInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetOQADataReport(string sFactoryKey, string sSNType, string sDefault, string sDateFalg, string sStartSN, string sEndSN, string sWO, string sPROID, string sStartDate, string sEndDate)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT C.LOCATION_NAME,A.CC_FCODE1,A.CUSTOMCODE,A.CHECK_TIME,A.WORKNUMBER,A.PRO_ID,A.LOT_COLOR,B.COLUMN_NAME,A.SHIFT_NAME";
                sql += ",A.REASON_CODE_CATEGORY_NAME,A.REASON_CODE_NAME,A.CREATER,A.REMARK";
                sql += " FROM WIP_CUSTCHECK A";
                sql += " LEFT OUTER JOIN (SELECT T.COLUMN_CODE,T.COLUMN_NAME,T.COLUMN_NAME_DESC";
                sql += " FROM (SELECT T.ITEM_ORDER";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Index' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Index'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_type' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_type'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_code' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_code'";
                sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name_Desc' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name_Desc'";
                sql += " FROM CRM_ATTRIBUTE  T,BASE_ATTRIBUTE  T1,BASE_ATTRIBUTE_CATEGORY T2";
                sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
                sql += " AND UPPER(T2.CATEGORY_NAME) = 'Basic_TestRule_PowerSet'";
                sql += " GROUP BY T.ITEM_ORDER) T";
                sql += " WHERE T.Column_type='ProductGrade') B ON A.PRO_LEVEL=B.COLUMN_CODE";
                sql += " INNER JOIN FMM_LOCATION C ON A.ROOM_KEY=C.LOCATION_KEY AND C.LOCATION_LEVEL='5'";
                if (sFactoryKey != "ALL")
                {
                    sql += " AND C.LOCATION_KEY='" + sFactoryKey + "'";
                }
                sql += " WHERE A.CC_DATA_GROUP='O'";
                if (sDefault == "T")
                {
                    sql += " AND A.ISFLAG='1'";
                }
                if (!string.IsNullOrEmpty(sWO))
                {
                    sql += " AND A.WORKNUMBER='" + sWO + "'";
                }
                if (!string.IsNullOrEmpty(sPROID))
                {
                    sql += " AND A.PRO_ID='" + sPROID + "'";
                }
                if (sSNType == "C")
                {
                    if (!string.IsNullOrEmpty(sStartSN))
                    {
                        sql += " AND A.CUSTOMCODE>='" + sStartSN + "'";
                    }
                    if (!string.IsNullOrEmpty(sEndSN))
                    {
                        sql += " AND A.CUSTOMCODE<='" + sEndSN + "'";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(sStartSN))
                    {
                        sql += " AND A.CC_FCODE1>='" + sStartSN + "'";
                    }
                    if (!string.IsNullOrEmpty(sEndSN))
                    {
                        sql += " AND A.CC_FCODE1<='" + sEndSN + "'";
                    }
                }
                if (sDateFalg == "T")
                {
                    if (!string.IsNullOrEmpty(sStartDate))
                    {
                        sql += " AND A.CHECK_TIME>='" + sStartDate + "'";
                    }
                    if (!string.IsNullOrEmpty(sEndDate))
                    {
                        sql += " AND A.CHECK_TIME<='" + sEndDate + "'";
                    }
                }
                sql += " ORDER BY A.l_ID ASC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFactoryInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetWOAttributeValueByLotNum(string sLotNum, string sType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT C.ATTRIBUTE_NAME,C.ATTRIBUTE_VALUE";
                sql += " FROM POR_LOT A,POR_WORK_ORDER B,POR_WORK_ORDER_ATTR C";
                sql += " WHERE A.WORK_ORDER_NO=B.ORDER_NUMBER AND B.WORK_ORDER_KEY=C.WORK_ORDER_KEY";
                sql += " AND C.ISFLAG='1'";
                if (sType != "")
                {
                    sql += " AND C.ATTRIBUTE_NAME='" + sType + "'";
                }
                if (sLotNum != "")
                {
                    sql += " AND A.LOT_NUMBER='" + sLotNum + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWOAttributeValueByLotNum Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet dsGetConergyPackgeData2(string sPalletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                       @"IF EXISTS(SELECT 1
                                  FROM WIP_CONSIGNMENT a
                                  INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                  INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                  WHERE  a.ISFLAG=1
                                  AND a.VIRTUAL_PALLET_NO='{0}')
                        BEGIN
                            SELECT DISTINCT E.POWERLEVEL
                            FROM WIP_CONSIGNMENT A
                            INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<'2'
                            INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                            LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                       AND D.PART_NUMBER=B.PART_NUMBER
                                                       AND D.PS_CODE=C.VC_TYPE
                                                       AND D.PS_SEQ=C.I_IDE
                                                       AND D.IS_USED='Y'
                            LEFT JOIN POR_WO_PRD_PS_SUB E ON E.WORK_ORDER_KEY=D.WORK_ORDER_KEY
                                                          AND E.PART_NUMBER=D.PART_NUMBER
                                                          AND E.POWERSET_KEY=D.POWERSET_KEY
                                                          AND E.PS_SUB_CODE=C.I_PKID
                                                          AND E.IS_USED='Y'
                            WHERE A.CS_DATA_GROUP>'0' 
                            AND A.ISFLAG='1'
                            AND A.VIRTUAL_PALLET_NO='{0}';
                        END
                        ELSE
                        BEGIN
                            SELECT G.POWERLEVEL
                            FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,POR_PRODUCT D,BASE_TESTRULE E,BASE_POWERSET F,BASE_POWERSET_DETAIL G
                            WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO AND B.LOT_NUMBER=C.LOT_NUM AND B.PRO_ID=D.PRODUCT_CODE
                            AND D.PRO_TEST_RULE=E.TESTRULE_CODE AND E.PS_CODE=F.PS_CODE AND F.POWERSET_KEY=G.POWERSET_KEY
                            AND C.I_IDE=F.PS_SEQ AND C.I_PKID=G.PS_DTL_SUBCODE
                            AND A.CS_DATA_GROUP!='0' AND A.ISFLAG='1' AND B.DELETED_TERM_FLAG!='2' AND C.VC_DEFAULT='1'
                            AND D.ISFLAG='1' AND E.ISFLAG='1' AND F.ISFLAG='1' AND G.ISFLAG='1'
                            AND A.VIRTUAL_PALLET_NO='{0}'
                            GROUP BY G.POWERLEVEL;
                        END", sPalletNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetConergyPackgeData2 Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet dsGetPicPath(string sFactoryCode, string sPicType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT A.* ";
                sql += " FROM (SELECT T.ITEM_ORDER,";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'PIC_ORDER_INDEX' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'PIC_ORDER_INDEX',";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'PIC_ADDRESS' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'PIC_ADDRESS',";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'PIC_FACTORY_CODE' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'PIC_FACTORY_CODE',";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'PIC_ADDRESS_NAME' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'PIC_ADDRESS_NAME',";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'PIC_TYPE' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'PIC_TYPE',";
                sql += "MAX( case T.ATTRIBUTE_NAME when 'PIC_ISCHECK' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'PIC_ISCHECK'";
                sql += " FROM CRM_ATTRIBUTE T,BASE_ATTRIBUTE T1,BASE_ATTRIBUTE_CATEGORY T2";
                sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
                sql += " AND UPPER(T2.CATEGORY_NAME) = 'Uda_Pic_Address'";
                sql += " GROUP BY T.ITEM_ORDER";
                sql += " ) A ";
                sql += " WHERE 1=1";
                if (!string.IsNullOrEmpty(sFactoryCode))
                {
                    sql += " AND A.PIC_FACTORY_CODE='" + sFactoryCode + "'";
                }
                if (!string.IsNullOrEmpty(sPicType))
                {
                    sql += " AND A.PIC_TYPE='" + sPicType + "'";
                }
                sql += " ORDER BY A.ITEM_ORDER ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("dsGetPicPath Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetCodeSoftLabelSet(string sLabelID)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM BASE_CODESOFT_LABEL_SET WHERE LABEL_ID='" + sLabelID + "'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCodeSoftLabelSet Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPPSMasterImpData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"IF EXISTS(SELECT 1
                                                      FROM WIP_CONSIGNMENT a
                                                      INNER JOIN POR_WORK_ORDER b ON b.ORDER_NUMBER=a.WORKNUMBER
                                                      INNER JOIN POR_WO_PRD c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.IS_USED='Y'
                                                      WHERE  a.ISFLAG=1
                                                      AND a.VIRTUAL_PALLET_NO='{0}')
                                            BEGIN
                                                SELECT B.PALLET_NO,
                                                       B.PRO_ID,
                                                       B.WORK_ORDER_NO,
                                                       CONVERT(VARCHAR(10),B.PALLET_TIME,120) AS 'PRODUCT_DATE',
                                                       B.LOT_NUMBER,
                                                       C.VC_CUSTCODE,
                                                       '21' + B.LOT_NUMBER AS 'BARCODEDATA',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMP',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CONVERT(varchar,CAST(C.COEF_IMAX as decimal(6,2)))+ISNULL(E.POWERLEVEL,'') AS 'IMP',
                                                       CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMP',
                                                       CAST(C.COEF_FF*100 as decimal(6,2)) AS 'FF',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'POWERMAX',
                                                       C.TTIME,
                                                       D.PS_SUBCODE AS 'PID',
                                                       'Conergy PH ' + D.MODULE_NAME AS 'ARTNUMBER',
                                                       D.PS_CODE AS 'VC_TYPE',
                                                       D.PS_RULE AS 'VC_TYPENAME',
                                                       LEFT(D.MODULE_NAME,3) AS 'POWER',
                                                       D.P_MIN,
                                                       D.P_MAX,
                                                       K.GRADE_NAME AS 'C_NAME',
                                                       K.GRADE_NAME_DESC AS 'E_NAME'
                                                FROM WIP_CONSIGNMENT A
                                                INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                           AND D.PART_NUMBER=B.PART_NUMBER
                                                                           AND D.PS_CODE=C.VC_TYPE
                                                                           AND D.PS_SEQ=C.I_IDE
                                                                           AND D.IS_USED='Y'
                                                LEFT JOIN POR_WO_PRD_PS_SUB E ON E.WORK_ORDER_KEY=D.WORK_ORDER_KEY
                                                                              AND E.PART_NUMBER=D.PART_NUMBER
                                                                              AND E.POWERSET_KEY=D.POWERSET_KEY
                                                                              AND E.PS_SUB_CODE=C.I_PKID
                                                                              AND E.IS_USED='Y'
                                                LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                WHERE A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP>'0'
                                                AND A.VIRTUAL_PALLET_NO='{0}'
                                                ORDER BY ad.ITEM_NO; 
                                            END
                                            ELSE
                                            BEGIN
                                                SELECT B.PALLET_NO,B.PRO_ID,B.WORK_ORDER_NO,
                                                       CONVERT(VARCHAR(10),B.PALLET_TIME,120) AS 'PRODUCT_DATE',
                                                       B.LOT_NUMBER,C.VC_CUSTCODE,
                                                       '21' + B.LOT_NUMBER AS 'BARCODEDATA',
                                                       CAST(C.COEF_PMAX as decimal(6,2)) AS 'PMP',
                                                       CAST(C.COEF_ISC as decimal(6,2)) AS 'ISC',
                                                       CAST(C.COEF_VOC as decimal(6,2)) AS 'VOC',
                                                       CASE WHEN ISNULL(CONVERT(varchar,CAST(C.COEF_IMAX as decimal(6,2))) +
                                                                (SELECT RTRIM(LTRIM(E.POWERLEVEL)) 
                                                                 FROM BASE_POWERSET_DETAIL E 
                                                                 WHERE E.POWERSET_KEY=D.POWERSET_KEY AND E.PS_DTL_SUBCODE=C.I_PKID AND E.ISFLAG='1'),'')='' 
                                                           THEN CONVERT(VARCHAR,CAST(C.COEF_IMAX as decimal(6,2)))
                                                           ELSE CONVERT(varchar,CAST(C.COEF_IMAX as decimal(6,2))) +
                                                                 (SELECT rtrim(ltrim(E.POWERLEVEL)) FROM BASE_POWERSET_DETAIL E 
                                                                  WHERE E.POWERSET_KEY=D.POWERSET_KEY AND E.PS_DTL_SUBCODE=C.I_PKID AND E.ISFLAG='1') 
                                                       END AS 'IMP',
                                                      CAST(C.COEF_VMAX as decimal(6,2)) AS 'VMP',CAST(C.COEF_FF*100 as decimal(6,2)) AS 'FF',
                                                      CAST(C.COEF_PMAX as decimal(6,2)) AS 'POWERMAX',
                                                      C.TTIME,
                                                      D.PS_SUBCODE AS 'PID',
                                                      'Conergy PH ' + D.MODULE_NAME AS 'ARTNUMBER',
                                                      D.PS_CODE AS 'VC_TYPE',
                                                      D.PS_RULE AS 'VC_TYPENAME',
                                                      LEFT(D.MODULE_NAME,3) AS 'POWER',
                                                      D.P_MIN,D.P_MAX,
                                                      K.GRADE_NAME AS 'C_NAME',
                                                      K.GRADE_NAME_DESC AS 'E_NAME'
                                                FROM WIP_CONSIGNMENT A,POR_LOT B,WIP_IV_TEST C,BASE_POWERSET D,V_ProductGrade K
                                                WHERE A.VIRTUAL_PALLET_NO=B.PALLET_NO 
                                                AND B.LOT_NUMBER=C.LOT_NUM 
                                                AND C.VC_TYPE=D.PS_CODE 
                                                AND C.I_IDE=D.PS_SEQ
                                                AND B.PRO_LEVEL = K.GRADE_CODE
                                                AND A.ISFLAG='1' 
                                                AND A.CS_DATA_GROUP!='0' 
                                                AND B.DELETED_TERM_FLAG!='2' 
                                                AND C.VC_DEFAULT='1' 
                                                AND D.ISFLAG='1'
                                                AND A.VIRTUAL_PALLET_NO='{0}'
                                                ORDER BY B.LOT_NUMBER;
                                            END", sPalltNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPPSMasterImpData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 是否重新计算衰减数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：是。false：否</returns>
        public bool IsRecalcDecayData(string lotNumber)
        {
            try
            {
                string sql = string.Format(@"SELECT b.CS_DATA_GROUP
                                           FROM POR_LOT a
                                           INNER JOIN WIP_CONSIGNMENT b ON a.PALLET_NO=b.VIRTUAL_PALLET_NO
                                           WHERE a.STATUS<2
                                           AND b.ISFLAG=1
                                           AND a.LOT_NUMBER='{0}'",
                                           lotNumber.PreventSQLInjection());
                object objState = db.ExecuteScalar(CommandType.Text, sql);
                //已经过入库检验，则不允许进行衰减数据的计算。
                if (objState != null && objState != DBNull.Value)
                {
                    int state = Convert.ToInt32(objState);
                    return !(state >= 2 && state < 10);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogService.LogError("IsRecalcDecayData Error: " + ex.Message);
                return true;
            }
        }
        /// <summary>
        /// 根据工单号获取工单设置的产品基本规则数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单产品基本规则数据的数据集对象。</returns>
        public DataSet GetWoProductData(string orderNumber)
        {
            DataSet dsReturn = null;
            try
            {
                string sql = string.Format(@"SELECT a.PART_NUMBER,a.PRODUCT_KEY,a.ITEM_NO, a.IS_USED, a.IS_MAIN, a.PRODUCT_CODE, a.PRODUCT_NAME, a.QUANTITY, 
                                            a.MAXPOWER, a.MINPOWER, a.PRO_TEST_RULE, a.PROMODEL_NAME, a.CODEMARK, a.CUSTMARK, a.LABELTYPE, 
                                            a.LABELVAR, a.LABELCHECK, a.PRO_LEVEL, a.SHIP_QTY,a.CERTIFICATION, a.TOLERANCE, 
                                            a.JUNCTION_BOX, a.CALIBRATION_TYPE, a.CALIBRATION_CYCLE, a.FIX_CYCLE, a.LAST_TEST_TYPE, 
                                            a.POWER_DEGREE, a.FULL_PALLET_QTY
                                            FROM POR_WO_PRD a
                                            INNER JOIN POR_WORK_ORDER b ON a.WORK_ORDER_KEY=b.WORK_ORDER_KEY
                                            WHERE a.IS_USED='Y'
                                            AND b.ORDER_NUMBER='{0}'",
                                            orderNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetWoProductData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号、产品料号和功率获取对应的产品衰减系数。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="pm">功率。</param>
        /// <returns>包含工单产品衰减系数数据的数据集对象。</returns>
        public DataSet GetDecayCoefficient(string orderNumber, string partNumber, decimal pm)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT e.D_NAME,e.COEFFICIENT,e.DECOEFFI_TYPE
                                        FROM POR_WO_PRD_DECAY a
                                        INNER JOIN POR_WORK_ORDER b ON a.WORK_ORDER_KEY=b.WORK_ORDER_KEY
                                        INNER JOIN BASE_DECAYCOEFFI d ON d.DECOEFFI_KEY=a.DECOEFFI_KEY AND d.ISFLAG=1
                                        INNER JOIN BASE_DECAYCOEFFI e ON e.D_CODE=d.D_CODE AND e.ISFLAG=1
                                        WHERE a.IS_USED='Y'
                                        AND a.DECAY_POWER_MIN < {0}
                                        AND a.DECAY_POWER_MAX >= {0}
                                        AND a.PART_NUMBER='{1}'
                                        AND b.ORDER_NUMBER='{2}'
                                        ORDER BY a.DECAY_POWER_MIN",
                                        pm,
                                        partNumber.PreventSQLInjection(),
                                        orderNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetDecayCoefficient Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号、产品料号和衰减后功率获取对应的分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="lotNo">组件批次号。</param>
        /// <param name="coefPM">衰减后功率。</param>
        /// <returns>包含对应分档数据的数据集对象。</returns>
        public DataSet GetWOPowerSetData(string orderNumber, string partNumber, string lotNo, decimal coefPM)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT a.WORK_ORDER_KEY,a.PART_NUMBER,a.POWERSET_KEY,a.VERSION_NO,a.DEMAND_QTY,
                                                    a.PS_SEQ,a.PS_CODE,a.PS_RULE,a.MODULE_NAME,a.P_MAX,a.P_MIN,a.PMAXSTAB,a.ISCSTAB,a.VOCSTAB,
                                                    a.IMPPSTAB,a.VMPPSTAB,a.FUSE,a.PS_SUBCODE,a.SUB_PS_WAY,a.POWER_DIFFERENCE
                                        FROM POR_WO_PRD_PS a
                                        INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                        WHERE a.IS_USED='Y'
                                        AND b.ORDER_NUMBER='{1}'
                                        AND a.PART_NUMBER='{2}'
                                        AND a.P_MIN<={0}
                                        AND a.P_MAX>{0}
                                        ORDER BY PS_SEQ",
                                        coefPM,
                                        orderNumber.PreventSQLInjection(),
                                        partNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                //根据组件序列号抓取其颜色，从而获取其对应的ARITICLENO
                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsReturn.Tables[0].Rows[0];
                    sql = string.Format(@"SELECT a.ARTICNO
                                        FROM POR_WO_PRD_PS_CLR a
                                        INNER JOIN POR_LOT b ON a.COLOR_CODE=b.COLOR
                                        WHERE a.IS_USED='Y'
                                        AND a.POWERSET_KEY='{0}'
                                        AND a.WORK_ORDER_KEY='{1}'
                                        AND a.PART_NUMBER='{2}'
                                        AND a.VERSION_NO={3}
                                        AND b.LOT_NUMBER='{4}'",
                                        dr["POWERSET_KEY"],
                                        dr["WORK_ORDER_KEY"],
                                        dr["PART_NUMBER"],
                                        dr["VERSION_NO"],
                                        lotNo.PreventSQLInjection());
                    DataTable dtArticno = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    if (dtArticno.Rows.Count > 0)
                    {
                        dsReturn.ExtendedProperties.Add("articno", Convert.ToString(dtArticno.Rows[0][0]));
                    }
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetWOPowerSetData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单主键、产品料号、分档主键和衰减后数据获取对应的子分档数据。
        /// </summary>
        /// <param name="workOrderKy">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <param name="val">根据子分档规则设置不同的值，如果是功率子分档设置为衰减后功率，如果是电流子分档设置为衰减后电流。</param>
        /// <returns>包含对应子分档数据的数据集对象。</returns>
        public DataSet GetWOPowerSetDetailData(string workOrderKy, string partNumber, string powersetKey, decimal val)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT PS_SUB_CODE,POWERLEVEL,P_DTL_MIN,P_DTL_MAX
                                        FROM POR_WO_PRD_PS_SUB
                                        WHERE WORK_ORDER_KEY='{0}'
                                        AND PART_NUMBER='{1}'
                                        AND POWERSET_KEY='{2}'
                                        AND IS_USED='Y'
                                        AND P_DTL_MIN<={3}
                                        AND P_DTL_MAX>{3}
                                        ORDER BY PS_SUB_CODE",
                                        workOrderKy.PreventSQLInjection(),
                                        partNumber.PreventSQLInjection(),
                                        powersetKey.PreventSQLInjection(),
                                        val);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetWOPowerSetDetailData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单主键、产品料号、分档主键获取对应子分档最大及最小功率区间
        /// </summary>
        /// <param name="workOrderKy">工单主键</param>
        /// <param name="partNumber">产品料号</param>
        /// <param name="powersetKey">分档主键</param>
        /// <returns>工单主键、产品料号、分档主键获取对应子分档最大及最小功率区间数据</returns>
        public DataSet GetWOPowerSetDetailDataRang(string workOrderKy, string partNumber, string powersetKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT WORK_ORDER_KEY,PART_NUMBER,POWERSET_KEY,MIN(P_DTL_MIN) AS PTL_MIN,MAX(P_DTL_MAX) AS PTL_MAX
                                             FROM POR_WO_PRD_PS_SUB
                                             WHERE WORK_ORDER_KEY='{0}'
                                             AND PART_NUMBER='{1}'
                                             AND POWERSET_KEY='{2}'
                                             AND IS_USED='Y'
										     GROUP BY WORK_ORDER_KEY,PART_NUMBER,POWERSET_KEY",
                                             workOrderKy.PreventSQLInjection(),
                                             partNumber.PreventSQLInjection(),
                                             powersetKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetWOPowerSetDetailDataRang Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次号获取有效的IV测试数据。
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <returns>包含批次数据及其IV测试数据的数据集对象。</returns>
        public DataSet GetIVTestData(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT a.LOT_NUMBER,a.PART_NUMBER,a.PRO_ID,a.WORK_ORDER_KEY,a.WORK_ORDER_NO,a.CREATE_OPERTION_NAME,a.FACTORYROOM_NAME,a.QUANTITY_INITIAL,
                                               b.IV_TEST_KEY, b.DEVICENUM, b.PM, b.ISC, b.IPM, b.VOC, b.VPM, b.TTIME, b.VC_PSIGN, 
                                               b.VC_DEFAULT, b.AMBIENTTEMP, b.SENSORTEMP,b.FF, b.EFF, b.RS, b.RSH, b.INTENSITY, 
                                               b.COEF_PMAX, b.COEF_ISC,b.COEF_VOC,b.COEF_IMAX,b.COEF_VMAX,b.COEF_FF, b.Imp_Isc,b.ImpIsc_Control,a.CREATE_TIME,
                                               a.EFFICIENCY AS VC_CELLEFF, b.DEC_CTM,b.CALIBRATION_NO
                                            FROM POR_LOT a
                                            LEFT JOIN WIP_IV_TEST b ON b.LOT_NUM=a.LOT_NUMBER AND b.VC_DEFAULT=1
                                            WHERE a.LOT_NUMBER='{0}'
                                            AND a.DELETED_TERM_FLAG<2
                                            ORDER BY b.L_ID DESC",
                                            lotNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetIVTestData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号、产品料号、产品ID号获取对应的打印标签数据。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="productId">产品ID号。</param>
        /// <returns>包含打印标签数据的数据集对象。</returns>
        public DataSet GetWOPrintLabelDataByNo(string workOrderNo, string partNumber, string productId)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"IF EXISTS(SELECT 1 FROM POR_WORK_ORDER a
                                                      INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                      WHERE a.ORDER_NUMBER='{0}')
                                            BEGIN
                                                SELECT a.PRINTLABEL_ID,a.VIEW_ADDRESS,a.ISLABEL,a.ISMAIN,a.PRINT_QTY,a.ISPACKAGEPRINT
                                                FROM POR_WO_PRD_PRINTSET a
                                                INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                WHERE a.IS_USED='Y'
                                                AND b.ORDER_NUMBER='{0}'
                                                AND a.PART_NUMBER='{2}'
                                                ORDER BY DECAY_KEY;
                                            END
                                            ELSE
                                            BEGIN
                                                SELECT t.VIEW_NAME AS PRINTLABEL_ID,t.VIEW_ADDRESS,t.ISLABEL,t.ISMAIN,t.PRINT_QTY,t.ISPACKAGEPRINT
                                                FROM BASE_TESTRULE_PRINTSET t
                                                INNER JOIN BASE_TESTRULE b ON b.TESTRULE_KEY=t.TESTRULE_KEY AND b.ISFLAG=1
                                                INNER JOIN POR_PRODUCT a ON a.PRO_TEST_RULE=b.TESTRULE_CODE AND a.ISFLAG=1
                                                WHERE a.PRODUCT_CODE='{1}'
                                                AND t.ISFLAG=1
                                                AND DECAY_KEY IN (SELECT DECAY_KEY 
                                                                  FROM BASE_TESTRULE_DECAY 
                                                                  WHERE TESTRULE_KEY=b.TESTRULE_KEY  AND ISFLAG='1')
                                                ORDER BY VIEW_NAME ASC;
                                            END",
                                            workOrderNo.PreventSQLInjection(),
                                            productId.PreventSQLInjection(),
                                            partNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetWOPrintLabelData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单主键、产品料号获取对应的打印标签数据。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <returns>包含打印标签数据的数据集对象。</returns>
        public DataSet GetWOPrintLabelData(string workOrderKey, string partNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT a.PRINTLABEL_ID,a.VIEW_ADDRESS,a.ISLABEL,a.ISMAIN,a.PRINT_QTY,a.ISPACKAGEPRINT,
                                                    ISNULL(b.DECAY_SEQ,0) SEQ
                                            FROM POR_WO_PRD_PRINTSET a
                                            LEFT JOIN POR_WO_PRD_DECAY b ON b.DECAY_KEY=a.DECAY_KEY 
                                                                         AND b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                                         AND b.PART_NUMBER=a.PART_NUMBER 
                                                                         AND a.IS_USED='Y'
                                            WHERE a.IS_USED='Y'
                                            AND a.WORK_ORDER_KEY='{0}'
                                            AND a.PART_NUMBER='{1}'",
                                            workOrderKey.PreventSQLInjection(),
                                            partNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.GetWOPrintLabelData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取工单已生产的产品数量。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <returns>产品数量。</returns>
        public decimal GetWOProductPowersetQty(string workOrderKey, string partNumber, string powersetKey)
        {
            try
            {
                string sql = string.Format(@"SELECT COUNT(DISTINCT a.LOT_NUM)
                                            FROM WIP_IV_TEST a
                                            INNER JOIN POR_LOT b ON b.LOT_NUMBER=a.LOT_NUM
                                            INNER JOIN POR_WO_PRD_PS c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY
                                                                       AND c.PART_NUMBER=b.PART_NUMBER
                                                                       AND c.PS_CODE=a.VC_TYPE 
                                                                       AND c.PS_SEQ=a.I_IDE
                                                                       AND c.IS_USED='Y'
                                            WHERE c.WORK_ORDER_KEY='{0}'
                                            AND c.PART_NUMBER='{1}'
                                            AND c.POWERSET_KEY='{2}'
                                            AND a.VC_DEFAULT=1
                                            AND a.VC_PSIGN='Y'",
                                            workOrderKey.PreventSQLInjection(),
                                            partNumber.PreventSQLInjection(),
                                            powersetKey.PreventSQLInjection());
                decimal qty = Convert.ToDecimal(db.ExecuteScalar(CommandType.Text, sql));
                return qty;
            }
            catch (Exception ex)
            {
                LogService.LogError("PrintIvTestLableEngine.GetWOProductPowersetQty Error: " + ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// 保存打印数据。
        /// </summary>
        /// <param name="dsParams">包含打印数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SavePrintData(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Count <= 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "参数错误。");
                    return dsReturn;
                }
                DataTable dtParams = dsParams.Tables[0];
                using (TransactionScope tsCope = new TransactionScope())
                {
                    foreach (DataRow dr in dtParams.Rows)
                    {
                        string sql = Convert.ToString(dr["SQL_COL"]);
                        this.db.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    tsCope.Complete();
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PrintIvTestLableEngine.SavePrintData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 是否允许打印标签。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="msg">提示信息。如果允许则返回空字符串。否则返回对应的提示信息。</param>
        /// <returns>true：允许；false：不允许。</returns>
        public bool IsAllowPrintLabel(string lotNumber, out string msg)
        {
            try
            {
                msg = string.Empty;
                //根据批次号获取批次使用的工艺流程、当前工序
                string sql = string.Format(@"SELECT a.LOT_KEY,a.WORK_ORDER_KEY,a.WORK_ORDER_NO,b.ROUTE_STEP_KEY,b.ROUTE_STEP_NAME
                                            FROM POR_LOT a
                                            INNER JOIN POR_ROUTE_STEP b ON b.ROUTE_STEP_KEY=a.CUR_STEP_VER_KEY
                                            WHERE a.LOT_NUMBER='{0}'
                                            AND a.STATUS<2",
                                            lotNumber.PreventSQLInjection());
                DataSet dsReturn = this.db.ExecuteDataSet(CommandType.Text, sql);
                if (dsReturn == null || dsReturn.Tables.Count <= 0 || dsReturn.Tables[0].Rows.Count <= 0)
                {
                    msg = string.Format("批次（{0}）不存在，请确认。", lotNumber);
                    return false;
                }
                string stepKey = Convert.ToString(dsReturn.Tables[0].Rows[0]["ROUTE_STEP_KEY"]);
                string stepName = Convert.ToString(dsReturn.Tables[0].Rows[0]["ROUTE_STEP_NAME"]);
                //组件当前工艺流程当前所在工序是否允许打印标签。 "IsAllowPrintLabel"
                sql = string.Format(@"SELECT ATTRIBUTE_VALUE
                                    FROM POR_ROUTE_STEP_ATTR
                                    WHERE ROUTE_STEP_KEY='{0}'
                                    AND ATTRIBUTE_NAME='IsAllowPrintLabel'",
                                    stepKey);
                DataTable dtStep = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //默认没设置，则允许打印。
                if (dtStep.Rows.Count > 0)
                {
                    bool bIsAllowPrintLabel = false;
                    string attrVal = Convert.ToString(dtStep.Rows[0]["ATTRIBUTE_VALUE"]);
                    //如果设置值错误，则允许打印。
                    if (!bool.TryParse(attrVal, out bIsAllowPrintLabel))
                    {
                        bIsAllowPrintLabel = true;
                    }
                    //return false，提示组件当前所在工序，不允许打印标签。
                    if (bIsAllowPrintLabel == false)
                    {
                        msg = string.Format("批次（{0}）当前在({1})工序，不允许打印标签，请确认。",
                                            lotNumber, stepName);
                        return false;
                    }
                }
                string lotKey = Convert.ToString(dsReturn.Tables[0].Rows[0]["LOT_KEY"]);
                string workOrderKey = Convert.ToString(dsReturn.Tables[0].Rows[0]["WORK_ORDER_KEY"]);
                string workOrderNo = Convert.ToString(dsReturn.Tables[0].Rows[0]["WORK_ORDER_NO"]);
                string mustTypeInTestParamsStepName = "组件测试";
                //IsMustTypeinTestParams  检查工单属性设置是否必须输入测试参数数据。
                //
                if (!string.IsNullOrEmpty(workOrderKey)
                    && mustTypeInTestParamsStepName.Contains(stepName))
                {
                    sql = string.Format(@"SELECT a.ATTRIBUTE_VALUE
                                        FROM POR_WORK_ORDER_ATTR a
                                        WHERE a.ATTRIBUTE_NAME='IsMustTypeinTestParams'
                                        AND a.WORK_ORDER_KEY='{0}'
                                        AND a.ISFLAG=1",
                                        workOrderKey);
                    DataTable dtWoAttr = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    //默认没设置，则表示不需要输入参数值。
                    if (dtWoAttr.Rows.Count > 0)
                    {
                        bool bIsMustTypeinTestParams = false;
                        string attrVal = Convert.ToString(dtWoAttr.Rows[0]["ATTRIBUTE_VALUE"]);
                        //如果设置值错误，则允许打印。
                        if (!bool.TryParse(attrVal, out bIsMustTypeinTestParams))
                        {
                            bIsMustTypeinTestParams = false;
                        }
                        //需要输入参数值，才能打印。
                        if (bIsMustTypeinTestParams)
                        {
                            sql = string.Format(@"SELECT COUNT(1) 
                                                FROM WIP_PARAM a
                                                INNER JOIN WIP_TRANSACTION b ON b.TRANSACTION_KEY=a.TRANSACTION_KEY
                                                WHERE a.LOT_KEY='{0}'
                                                AND a.STEP_NAME='{1}'
                                                AND b.UNDO_FLAG=0
                                                AND b.PIECE_KEY='{0}'
                                                AND b.ACTIVITY='TRACKOUT'",
                                                lotKey, stepName);
                            int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                            //如果参数值数量为0，则给出提示，不能打印标签。
                            if (count <= 0)
                            {
                                msg = string.Format("批次（{0}）当前在({1})工序，工序采集参数值没有录入，不允许打印标签，请确认。",
                                            lotNumber, stepName);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError("PrintIvTestLableEngine.IsAllowPrintLable Error: " + ex);
                return false;
            }
        }

        /// <summary>
        /// 通过信息获取对应的SunEdison的信息
        /// </summary>
        /// <param name="hsSunEdison">查询条件信息</param>        
        /// <returns>通过信息获取对应的SunEdison的信息</returns>
        public DataSet GetSunEdisonList(Hashtable hsSunEdison)
        {
            DataSet dsReturn = new DataSet();

            string sql = string.Empty;
            string sqlWhere = string.Empty;

            string workOrderNumber = string.Empty;
            string stratTTime = string.Empty;
            string endTTime = string.Empty;
            string proGrades = string.Empty;


            try
            {
                sql = string.Format(@"
                                        SELECT   ROW_NUMBER() OVER(ORDER BY a.LOT_NUMBER) AS 'RowNumber',
                                                A.LOT_NUMBER AS 'serialnumber',
                                                B.CELLSUPPLIER AS 'Supplier',
                                                'M'+E.CELL_SUPPLIER+E.CELL_MODEL+ E.SE_MODULE_TYPE +E.PLACE_ORIGIN+E.BOM_DESIGN AS 'PartNumber',
                                                B.CELLCOLOR AS 'CELLCOLOR',
                                                CONVERT(varchar,CAST(B.CELLEFFICIENCY as decimal(16,2))) + '%' AS 'CELLEFFICIENCY',
                                                CAST(B.CELLPOWER as decimal(16,2))AS 'CELLPOWER',			
                                                CONVERT(decimal(16, 2), B.CELLPOWER)*A.QUANTITY AS 'NOMINALPOWER',
                                                CAST(C.PM as decimal(18,4))AS 'Pmax',
                                                CONVERT(varchar,CAST(((CONVERT(decimal(16, 2), B.CELLPOWER)*A.QUANTITY - CAST(C.COEF_PMAX as decimal(16,2)))/(CONVERT(decimal(16, 2), B.CELLPOWER)*A.QUANTITY))*100 as decimal(16,2))) + '%' AS 'CTM Loss',
                                                CONVERT(varchar,CAST(C.COEF_FF*100 as decimal(16,2))) + '%' AS 'FF',
                                                CAST(C.VPM as decimal(18,4)) AS 'VPM',
                                                CAST(C.IPM as decimal(18,4)) AS 'IPM',
                                                CAST(C.INTENSITY as decimal(16,2)) AS 'IRR',
                                                CAST(C.RSH as decimal(18,4)) AS 'RSH',
                                                CAST(C.AMBIENTTEMP as decimal(16,2))  AS 'TMOD',
                                                CAST(C.VOC as decimal(18,4)) AS 'VOC',
                                                CAST(C.ISC as decimal(18,4)) AS 'ISC',
                                                CAST(C.RS as decimal(18,4)) AS 'RS' ,
                                                CASE 
													WHEN UPPER(B.CELLSUPPLIER) = 'NSP' THEN B.CELLPN1
													WHEN UPPER(B.CELLSUPPLIER) <> 'NSP' THEN 
														CONVERT(VARCHAR ,B.CELLPN1)+ ' | ' + CONVERT(VARCHAR, B.CELLLOT1) + ' | ' + CONVERT(VARCHAR, B.PACKAGEQTY) 
												END AS 'CELLLOTCODE1',
                                                CASE 
                                                    WHEN UPPER(B.CELLSUPPLIER) = 'NSP' THEN 
														CASE 
															WHEN B.CELLPN2 <> '' THEN B.CELLPN2
															WHEN B.CELLPN2 = '' THEN ''
														END
													WHEN UPPER(B.CELLSUPPLIER) <> 'NSP' THEN
														CASE
															WHEN B.CELLPN2 <> '' THEN 
                                                                CONVERT(VARCHAR ,B.CELLPN2)+ ' | ' + CONVERT(VARCHAR, B.CELLLOT2) + ' | ' + CONVERT(VARCHAR, B.PACKAGEQTY)
															WHEN B.CELLPN2 = '' THEN ''
														END 														
                                                END AS 'CELLLOTCODE2' ,
                                                CAST(D.PMAXSTAB as decimal(16,0))  AS 'BIN',
                                                C.TTIME AS 'TESTERTIME',
                                                C.DEVICENUM AS 'LINE'
                                        FROM  POR_LOT A
                                        LEFT JOIN POR_LOT_CELL_PARAM B ON B.LOT_KEY = A.LOT_KEY AND A.STATUS < 2
                                        INNER JOIN WIP_IV_TEST C ON C.LOT_NUM = A.LOT_NUMBER AND C.VC_DEFAULT = 1
                                        INNER JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY = A.WORK_ORDER_KEY 
                                                                AND D.PART_NUMBER = A.PART_NUMBER 
                                                                AND C.COEF_PMAX <= D.P_MAX 
                                                                AND C.COEF_PMAX >=  D.P_MIN 
                                                                AND D.IS_USED = 'Y'
                                                                AND D.PS_CODE=C.VC_TYPE 
                                                                AND D.PS_SEQ=C.I_IDE
                                        INNER JOIN POR_WO_OEM E ON E.ORDER_NUMBER = A.WORK_ORDER_NO AND E.IS_USED = 'Y'
                                        INNER JOIN WIP_CUSTCHECK F ON A.LOT_NUMBER = F.CC_FCODE1 AND F.ISFLAG = 1 AND F.CC_DATA_GROUP = '1' 
                                        ");

                if (!string.IsNullOrEmpty(hsSunEdison["proGrades"].ToString()))
                {
                    sql += string.Format(@"	
                                        INNER JOIN  SplitStringToTable('{0}') G ON A.PRO_LEVEL = G.VAL ", hsSunEdison["proGrades"]);
                }
                if (hsSunEdison.ContainsKey("workOrderNumber"))
                {
                    sql += string.Format(@"	
                                        INNER JOIN  SplitStringToTable('{0}') H ON F.WORKNUMBER = H.VAL ", hsSunEdison["workOrderNumber"]);
                }
                if (!string.IsNullOrEmpty(hsSunEdison["lotNums"].ToString()))
                {
                    sql += string.Format(@"	
                                        INNER JOIN  SplitStringToTable('{0}') J ON A.LOT_NUMBER = J.VAL ", hsSunEdison["lotNums"]);
                }

                sqlWhere = @"
                                        WHERE a.STATUS<2
                                        AND a.LOT_TYPE= 'N'";

                sqlWhere += string.Format(@"
                                        AND F.CHECK_TIME > = '{0}'
                                        AND F.CHECK_TIME < '{1}'",
                                                             hsSunEdison["stratTTime"],
                                                             hsSunEdison["endTTime"]);
                if (!string.IsNullOrEmpty(hsSunEdison["proId"].ToString()))
                {
                    sqlWhere += string.Format(@" AND A.PRO_ID = '{0}'", hsSunEdison["proId"]);
                }
                if (!string.IsNullOrEmpty(hsSunEdison["partName"].ToString()))
                {
                    sqlWhere += string.Format(@" AND A.PART_NUMBER = '{0}'", hsSunEdison["partName"]);
                }
                sql += sqlWhere;

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSunEdisonList Error: " + ex.Message);
            }
            return dsReturn;
        }

        #region IPrintIvTestEngine 成员

        /// <summary>
        /// 通过批次号查询晶硅功率范围
        /// </summary>
        /// <param name="lotNum"></param>
        /// <returns></returns>
        public DataSet GetPowerRangeDate(string lotNum)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sql = string.Format(@"   SELECT T.LOT_NUM,T.COEF_PMAX,
	                                            C.PS_SEQ,C.P_MIN,C.P_MAX,C.SUB_PS_WAY,
	                                            D.PS_SUB_CODE,D.POWERLEVEL,D.P_DTL_MIN,D.P_DTL_MAX,
	                                             (CASE WHEN c.SUB_PS_WAY = '功率' AND ISNULL(d.PS_SUB_CODE,0)>0 THEN 
	                                            '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(d.P_DTL_MIN,'0'),'.'))+','+
	                                            CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(d.P_DTL_MAX,'0'),'.'))+'】'
	                                            ELSE '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(c.P_MIN,'0'),'.'))+','+
	                                            CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(c.P_MAX,'0'),'.'))+'】' END) AS POWERRANGE
	                                            FROM WIP_IV_TEST t
	                                            INNER JOIN POR_LOT a ON t.LOT_NUM=a.LOT_NUMBER
	                                            LEFT JOIN POR_WO_PRD_PS c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
	                                            AND c.PART_NUMBER=a.PART_NUMBER
	                                            AND c.PS_CODE=t.VC_TYPE 
	                                            AND C.PS_SEQ=t.I_IDE 
	                                            AND c.IS_USED='Y'
	                                            LEFT JOIN POR_WO_PRD_PS_SUB d ON d.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
	                                            AND d.PART_NUMBER=a.PART_NUMBER
	                                            AND d.POWERSET_KEY=c.POWERSET_KEY
	                                            AND d.PS_SUB_CODE=t.I_PKID
	                                            AND d.IS_USED='Y'
	                                            WHERE t.LOT_NUM='{0}' 
	                                            AND ISNULL(t.COEF_PMAX,0)<>0 
	                                            AND t.VC_DEFAULT='1'", lotNum);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerRangeDate Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IPrintIvTestEngine 成员

        /// <summary>
        /// 通过工单主键，效率下限，效率上限，Ctm值获取对应工单设定的符合的ctm信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="lowCelleff">低效率</param>
        /// <param name="highCelleff">高效率</param>
        /// <param name="ctm">实际ctm值</param>
        /// <returns>符合要求的ctm数据集</returns>
        public DataSet GetCtmInfByWorderEffCtm(string workOrderKey, decimal lowCelleff, decimal highCelleff, decimal ctm)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sql = string.Format(@"SELECT COUNT(*) AS CNT FROM dbo.POR_WO_PRD_CTM
                                                    WHERE WORK_ORDER_KEY = '{0}' AND IS_USED = 1 AND EFF_UP = '{1}'
                                                    AND EFF_LOW = '{2}' AND CTM_LOW <= '{3}'  AND CTM_UP >= '{3}'",
                                                    workOrderKey,
                                                    highCelleff,
                                                    lowCelleff,
                                                    ctm);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCtmInfByWorderEffCtm Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单获取效率
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        public DataSet GetInefficientPARAM(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT top 1 * FROM POR_LOT_CELL_PARAM WHERE 1=1 ");

                if (!string.IsNullOrEmpty(lotNumber))
                {
                    sbSql.AppendFormat("AND LOT_NUMBER='{0}'", lotNumber.PreventSQLInjection());
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetInefficientPARAM Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 根据产品类型获取效率
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        public DataSet GetProductCp(string procode,string conopj)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT top 1 A.* FROM BASE_PRODUCTMODEL_CP AS A INNER JOIN  BASE_PRODUCTMODEL AS B ON A.PROMODEL_KEY = B. PROMODEL_KEY ");
                sbSql.AppendFormat(@"INNER JOIN POR_WO_PRD AS C ON B.PROMODEL_NAME = C.PROMODEL_NAME WHERE A.ISFLAG=1 ");

                if (!string.IsNullOrEmpty(procode))
                {
                    sbSql.AppendFormat("AND C.PRODUCT_CODE='{0}'", procode.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(conopj))
                {
                    sbSql.AppendFormat("AND A.CONTROL_OBJ='{0}'", conopj.PreventSQLInjection());
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetProductCp Error: " + ex.Message);
            }

            return dsReturn;
        }
        #endregion

        #region IPrintIvTestEngine 成员


        public DataSet GetPPSMasterDataForMalai(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsReturn01 = new DataSet();
            DataSet dsReturn02 = new DataSet();
            DataSet dsReturn03 = new DataSet();
            DataTable dt01 = new DataTable();
            DataTable dt02 = new DataTable();
            DataTable dt03 = new DataTable();
            try
            {
                string sql = string.Format(
                            @"SELECT GroupId,PALLET_NO,[Type],WORK_ORDER_NO,LOT_NUMBER,
                                    PMP,ISC,VOC,IMP,VMP,Pnom,Grade,CONVERT(varchar,PRODUCT_DATA,120) as PRODUCT_DATA,JunctionBox,FPMP,F,RENGZHENG,PNOMNEW
                                    FROM dbo.WIP_PATTLELIST_MALAI_TEMP
                                    WHERE PALLET_NO = '{0}'",
                                sPalltNo.PreventSQLInjection());
                dsReturn01 = db.ExecuteDataSet(CommandType.Text, sql);
                dt01 = dsReturn01.Tables[0];
                dt01.TableName = "dsReturn01";
                sql = string.Format(
                            @"select 托盘号,工单号,料号 FROM dbo.WIP_PATTLELIST_MALAI_TEMP_PART
                                    where 托盘号 = '{0}'
                                    ",
                                sPalltNo.PreventSQLInjection());
                dsReturn02 = db.ExecuteDataSet(CommandType.Text, sql);
                dt02 = dsReturn02.Tables[0];
                dt02.TableName = "dsReturn02";
                sql = string.Format(
                            @"select distinct PnomNew from WIP_PATTLELIST_MALAI_TEMP
                                    where PALLET_NO =  '{0}'
                                    ",
                sPalltNo.PreventSQLInjection());
                dsReturn03 = db.ExecuteDataSet(CommandType.Text, sql);
                dt03 = dsReturn03.Tables[0];
                dt03.TableName = "dsReturn03";

                dsReturn.Merge(dt01);
                dsReturn.Merge(dt02);
                dsReturn.Merge(dt03);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPPSMasterDataForMalai Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IPrintIvTestEngine 成员


        public DataSet GetPPSMalai(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                            @"SELECT max(FPMP) as maxfpmp,min(FPMP) as minfpmp,sum(FPMP) as totalfpmp
                                    FROM dbo.WIP_PATTLELIST_MALAI_TEMP
                                    WHERE PALLET_NO = '{0}'",
                                sPalltNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPPSMalai Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IPrintIvTestEngine 成员


        public DataSet GetLotNumMalai(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                            @"SELECT LOT_NUMBER,FPMP,PnomNew01
                                    FROM dbo.WIP_PATTLELIST_MALAI_TEMP
                                    where LOT_NUMBER = '{0}'",
                                lotNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotNumMalai Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IPrintIvTestEngine 成员


        public DataSet GetLotNumsMalai(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(
                            @"SELECT LOT_NUMBER,FPMP,PnomNew01
                                    FROM dbo.WIP_PATTLELIST_MALAI_TEMP
                                    where PALLET_NO = '{0}'",
                                palletNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotNumsMalai Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        ///根据托号获取distinct的Color 判定花色是否混，主要用于金刚线和非金刚线的深和浅的问题
        ///金刚线组件：浅花、深花或者混花色（一个单托既有浅花又有深花，则为混花色）。
        ///非金刚线组件：浅蓝、深蓝或者混色（一个单托既有浅蓝又有深蓝，则为混色）。
        /// </summary>
        /// <param name="palletNo">托盘号</param>
        /// <returns>distinct Color 的数据集</returns>
        public DataSet GetColorData(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT DISTINCT COLOR from WIP_CONSIGNMENT_DETAIL A LEFT JOIN WIP_CONSIGNMENT B
                                                        ON A.CONSIGNMENT_KEY = B.CONSIGNMENT_KEY 
                                                        WHERE B.ISFLAG = 1
                                                        AND B.PALLET_NO = '{0}'",
                                                palletNo.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetColorData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据托号获取金刚线
        /// </summary>
        /// <param name="PalletNo">托号</param>
        /// <returns></returns>
        public DataSet GetKingLineByPallet(string PalletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT C.PRODUCT_KEY,C.PRODUCT_CODE,C.PRODUCT_NAME,C.ISKINGLING,E.PALLET_NO,E.ORDER_NUMBER FROM POR_PRODUCT C INNER JOIN
                                                        (SELECT A.CONSIGNMENT_KEY,A.PALLET_NO,A.WORKNUMBER,B.ORDER_NUMBER,B.PRODUCT_CODE,B.PRODUCT_KEY FROM WIP_CONSIGNMENT A INNER JOIN dbo.POR_WO_PRD B 
                                                        ON A.WORKNUMBER = B.ORDER_NUMBER WHERE A.ISFLAG = 1 AND B.IS_USED = 'Y' AND IS_MAIN = 'Y'
                                                        AND A.PALLET_NO = '{0}') E
                                                        ON C.PRODUCT_KEY = E.PRODUCT_KEY
                                                        WHERE C.ISFLAG = 1",
                                                PalletNo.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetKingLineByPallet Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion
    }
}

