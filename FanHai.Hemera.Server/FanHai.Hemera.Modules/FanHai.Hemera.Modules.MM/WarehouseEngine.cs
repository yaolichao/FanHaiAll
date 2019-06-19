using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Utils.Comm;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Share.Interface;
using System.Data.Common;
using SolarViewer.Hemera.Utils.StaticFuncsUtils;
using System.Collections;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using System.Data.OracleClient;

namespace SolarViewer.Hemera.Modules.MM
{
    public class WarehouseEngine : AbstractEngine, IWarehouseEngine
    {
        private Database db;

        public WarehouseEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public override void Initialize()
        {

        }

        #region ZMMLKOSave
        /// <summary>
        /// Save or update return order
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ZMMLKOSave(DataSet dataSet)
        {
            //如果存在退库单记录则为Update，否则为Insert
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            List<string> sqlCommandList = new List<string>();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string orderNumber = string.Empty;
            try
            {                
                if (null != dataSet && dataSet.Tables.Count > 0 && dataSet.Tables.Contains("IS_KO") && dataSet.Tables.Contains("IT_PO"))
                {
                    orderNumber = dataSet.ExtendedProperties["I_ZMBLNR"].ToString();
                    string factoryCode = dataSet.ExtendedProperties["I_WERKS"].ToString();
                    string zMMLType = dataSet.ExtendedProperties["I_ZMMLTYP"].ToString();
                    string editor = dataSet.ExtendedProperties["Operator"].ToString();
                    sql = "SELECT ZMBLNR FROM WST_TL_ZMMLKO WHERE ZMBLNR='" + orderNumber + "' AND LVORM='0'";                    
                    if (db.ExecuteScalar(CommandType.Text, sql) != null)
                    {   
                        #region update return order header data and delete detail data                    
                        sql = string.Format(@"update wst_tl_zmmlko
                                set werks = '{0}',
                                    zmmltyp ='{1}',                                    
                                    dept ='{2}',
                                    reason = '{3}', 
                                    LAEDA = {4},
                                    AENAM = '{5}'                                                                   
                              where zmblnr ='{6}'",
                              factoryCode, zMMLType, dataSet.Tables["IS_KO"].Rows[0]["DEPT"].ToString(),
                              dataSet.Tables["IS_KO"].Rows[0]["REASON"].ToString(),
                              "TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-DD hh24:mi:ss')",
                               editor, orderNumber);
                        sqlCommandList.Add(sql);
                        sql = "delete from wst_tl_zmmlpo where ZMBLNR='" + orderNumber + "'";
                        sqlCommandList.Add(sql);
                        #endregion
                    } 
                    else
                    {
                        #region Insert return order Header data
                        sql = string.Format(@"insert into wst_tl_zmmlko
                              (zmblnr, werks, zmmltyp,dept, reason,erdat, ernam)
                              values
                              ('{0}','{1}','{2}','{3}','{4}',{5},'{6}')",
                             orderNumber, factoryCode, zMMLType,
                             dataSet.Tables["IS_KO"].Rows[0]["DEPT"],
                             dataSet.Tables["IS_KO"].Rows[0]["REASON"],
                             "TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-DD hh24:mi:ss')",
                             editor);
                        sqlCommandList.Add(sql);
                        #endregion
                    }
                   
                    #region Insert return order detail data
                    foreach (DataRow row in dataSet.Tables["IT_PO"].Rows)
                    {
                        sql = string.Format(@"insert into wst_tl_zmmlpo
                               (zmblnr, aufnr, matnr, lgort, charg, in_menge, menge, meins, ztlsta, purpose, ref_ebeln, barcode)
                             values
                               ('{0}','{1}','{2}','{3}', '{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')
                             ", orderNumber, row["AUFNR"], row["MATNR"], row["LGORT"],row["CHARG"], row["IN_MENGE"], row["MENGE"], row["MEINS"],
                              row["ZTLSTA"], row["PURPOSE"], row["REF_EBELN"], row["BARCODE"]);
                        sqlCommandList.Add(sql);
                    }
                    #endregion
                    if (sqlCommandList.Count > 0)
                    {
                        dbconn = db.CreateConnection();
                        dbconn.Open();
                        dbtran = dbconn.BeginTransaction();
                        try
                        {
                            foreach (string strSql in sqlCommandList)
                            {
                                db.ExecuteNonQuery(dbtran, CommandType.Text, strSql);
                            }
                            dbtran.Commit();
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            dbtran.Rollback();                            
                            LogService.LogError("ZMMLKOSave Execute Sql Error:" + ex.Message);
                            throw ex;
                        }
                        finally
                        {
                            dbconn.Close();
                        }
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "输入数据不完整");
                }
            }
            catch (Exception ex)
            {                
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ZMMLKOSave Error:" + ex.Message);
                SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(dataSet, "CellReturnBillExceptionData", orderNumber);
            }           
            return dsReturn;
        }
        #endregion

        #region ZMMLSaveFromView
        /// <summary>
        /// Save return order
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ZMMLKOSaveFromView(DataSet dataSet)
        {
            //如果存在退库单记录则为重复提交，return
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            List<string> sqlCommandList = new List<string>();
            string orderNumber = string.Empty;
            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables.Contains("ES_ZMMLTKO") && dataSet.Tables.Contains("ET_PO"))
                {
                    string deleteFlag = dataSet.Tables["ES_ZMMLTKO"].Rows[0]["LVORM"].ToString();
                    orderNumber = dataSet.Tables["ES_ZMMLTKO"].Rows[0]["ZMBLNR"].ToString();
                    sql = "SELECT ZMBLNR FROM WST_TL_ZMMLKO WHERE ZMBLNR='" + orderNumber + "' AND LVORM='X'";
                    if (db.ExecuteScalar(CommandType.Text, sql) != null)
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "退料单" + orderNumber + "已被删除");
                        return dsReturn;
                    }
                    sql = "SELECT ZMBLNR FROM WST_TL_ZMMLKO WHERE ZMBLNR='" + orderNumber + "' AND LVORM='0'";
                    if (db.ExecuteScalar(CommandType.Text, sql) == null)
                    {
                        #region Save return order data
                        sql = string.Format(@"insert into wst_tl_zmmlko
                              (zmblnr, werks, zmmltyp,dept, reason,erdat, ernam)
                              values
                              ('{0}','{1}','{2}','{3}','{4}',{5},'{6}')",
                            orderNumber, dataSet.Tables["ES_ZMMLTKO"].Rows[0]["WERKS"].ToString(),
                            dataSet.Tables["ES_ZMMLTKO"].Rows[0]["ZMMLTYP"].ToString(),
                            dataSet.Tables["ES_ZMMLTKO"].Rows[0]["DEPT"].ToString(),
                            dataSet.Tables["ES_ZMMLTKO"].Rows[0]["REASON"].ToString(),
                            "TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-DD hh24:mi:ss')",
                            dataSet.ExtendedProperties["Operator"].ToString());
                        sqlCommandList.Add(sql);
                        //insert detail info
                        foreach (DataRow row in dataSet.Tables["ET_PO"].Rows)
                        {
                            sql = string.Format(@"insert into wst_tl_zmmlpo
                               (zmblnr, aufnr, matnr, lgort, charg, in_menge, menge, meins, ztlsta, purpose, ref_ebeln, barcode)
                             values
                               ('{0}','{1}','{2}','{3}', '{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')
                             ", orderNumber, row["AUFNR"], row["MATNR"], row["LGORT"],row["CHARG"], row["IN_MENGE"], row["MENGE"], row["MEINS"],
                                  row["ZTLSTA"], row["PURPOSE"], row["REF_EBELN"], row["BARCODE"]);
                            sqlCommandList.Add(sql);
                        }
                        if (sqlCommandList.Count > 0)
                        {
                            dbconn = db.CreateConnection();
                            dbconn.Open();
                            dbtran = dbconn.BeginTransaction();
                            try
                            {
                                foreach (string strSql in sqlCommandList)
                                {
                                    db.ExecuteNonQuery(dbtran, CommandType.Text, strSql);
                                }
                                dbtran.Commit();
                                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                            }
                            catch (Exception ex)
                            {
                                dbtran.Rollback();                               
                                LogService.LogError("ZMMLKOSaveFromView Execute Sql Error:" + ex.Message);
                                throw ex;
                            }
                            finally
                            {
                                dbconn.Close();
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (deleteFlag == "X")
                        {
                            //sap删除成功，本地删除失败时，删除记录
                            sql = string.Format(@"update WST_TL_ZMMLKO set LVORM='X' where ZMBLNR='{0}'",orderNumber);
                            db.ExecuteNonQuery(CommandType.Text, sql);
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                        }
                        else
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "不能重复保存");
                        }
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "输入数据不完整");
                }
            }
            catch (Exception ex)
            {                
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ZMMLKOSaveFromView Error:" + ex.Message);
                SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(dataSet, "CellReturnBillExceptionData", orderNumber);
            }           
            return dsReturn;
        }
        #endregion

        #region ZMMLKODelete
        /// <summary>
        /// Delete return order
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        public DataSet ZMMLKODelete(string ZMBLNR)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = "select ZMBLNR from WST_TL_ZMMLKO where ZMBLNR='" + ZMBLNR + "' and LVORM='0'";
                if (db.ExecuteScalar(CommandType.Text, sql) != null)
                {
                    //delete 
                    sql = "update WST_TL_ZMMLKO set LVORM='X' where ZMBLNR='" + ZMBLNR + "'";
                    db.ExecuteNonQuery(CommandType.Text, sql);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "记录不存在或已被删除");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ZMMLKODelete Error:" + ex.Message);
                SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(null, "CellReturnBillDeleteException", ZMBLNR);
            }
            return dsReturn;
        }
        #endregion

        /// <summary>
        /// Save Cell Label Data
        /// </summary>
        /// <param name="reqDS"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-06-23 16:20:47
        public string SaveCellLabelData(DataSet reqDS)
        {
            string msg = string.Empty;

            if (reqDS != null && reqDS.Tables.Contains("ZFCMARDH") && reqDS.Tables["ZFCMARD"].Rows.Count > 0)
            {
                string boxCode = string.Empty;

                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Save Cell Label Header Data

                                DataRow dataRow = reqDS.Tables["ZFCMARDH"].Rows[0];

                                boxCode = dataRow["BOXCODE"].ToString();

                                string sqlCommand = string.Format("SELECT ZH.LVORM FROM WST_BQ_ZFCMARDH ZH WHERE ZH.BOXCODE = '{0}'", boxCode);

                                object scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlCommand);

                                if (scalar != null && scalar != DBNull.Value)
                                {
                                    if (scalar.ToString() == "1")
                                    {
                                        throw new Exception("电池片入库标签已经删除!");
                                    }
                                    else
                                    {
                                        sqlCommand = string.Format(@"UPDATE WST_BQ_ZFCMARDH
                                                                       SET ZBOXTYP  = '{1}',
                                                                           WERKS = '{2}',
                                                                           IN_DATE  = TO_DATE('{3}','YYYY-MM-DD'),
                                                                           ZPKMODE  = '{4}',
                                                                           BWERKS  = '{5}',
                                                                           COND  = '{6}',
                                                                           MEMO  = '{7}',
                                                                           XBLB  = '{8}',
                                                                           CHK531 = '{9}',
                                                                           LVORM  = '{10}',
                                                                           LGORT = '{11}',
                                                                           ZSTLOC = '{12}',
                                                                           SWC001 = '{13}',
                                                                           SWC002 = '{14}',
                                                                           SWC003 = '{15}',
                                                                           SWC004 = '{16}',
                                                                           SWC005 = '{17}',
                                                                           SWC006 = '{18}',
                                                                           SWC007 = '{19}',
                                                                           SWC008 = '{20}',
                                                                           SWC011 = '{21}',
                                                                           MATNR = '{22}',
                                                                           MAKTX = '{23}',
                                                                           CHARG = '{24}',
                                                                           EDITOR = '{25}',
                                                                           EDIT_TIME = SYSDATE,
                                                                           OEM_MAT = '{26}',
                                                                           OEM_MATX = '{27}',
                                                                           OEM_WRK = '{28}'
                                                                     WHERE BOXCODE = '{0}'
                                                                    ", boxCode,
                                                                     dataRow["ZBOXTYP"], dataRow["WERKS"], dataRow["IN_DATE"],
                                                                     dataRow["ZPKMODE"], dataRow["BWERKS"], dataRow["COND"],
                                                                     dataRow["MEMO"], dataRow["XBLB"], dataRow["CHK531"],
                                                                     dataRow["LVORM"], dataRow["LGORT"], dataRow["ZSTLOC"],
                                                                     dataRow["SWC001"], dataRow["SWC002"], dataRow["SWC003"],
                                                                     dataRow["SWC004"], dataRow["SWC005"], dataRow["SWC006"],
                                                                     dataRow["SWC007"], dataRow["SWC008"], dataRow["SWC011"],
                                                                     dataRow["MATNR"], dataRow["MAKTX"], dataRow["CHARG"], dataRow["USER"],
                                                                     dataRow["OEM_MAT"], dataRow["OEM_MATX"], dataRow["OEM_WRK"]);
                                    }
                                }
                                else
                                {
                                    sqlCommand = string.Format(@"INSERT INTO WST_BQ_ZFCMARDH
                                                                      (BOXCODE,
                                                                       ZBOXTYP,
                                                                       WERKS,
                                                                       IN_DATE,
                                                                       ZPKMODE,
                                                                       BWERKS,
                                                                       COND,
                                                                       MEMO,
                                                                       XBLB,
                                                                       CHK531,
                                                                       LVORM,
                                                                       LGORT,
                                                                       ZSTLOC,
                                                                       SWC001,
                                                                       SWC002,
                                                                       SWC003,
                                                                       SWC004,
                                                                       SWC005,
                                                                       SWC006,
                                                                       SWC007,
                                                                       SWC008,
                                                                       SWC011,
                                                                       MATNR,
                                                                       MAKTX,
                                                                       CHARG,
                                                                       CREATOR,
                                                                       OEM_MAT,
                                                                       OEM_MATX,
                                                                       OEM_WRK
                                                                       )
                                                                    VALUES
                                                                      ('{0}',
                                                                       '{1}',
                                                                       '{2}',
                                                                       TO_DATE('{3}','YYYY-MM-DD'),
                                                                       '{4}',
                                                                       '{5}',
                                                                       '{6}',
                                                                       '{7}',
                                                                       '{8}',
                                                                       '{9}',
                                                                       '{10}',
                                                                       '{11}',
                                                                       '{12}',
                                                                       '{13}',
                                                                       '{14}',
                                                                       '{15}',
                                                                       '{16}',
                                                                       '{17}',
                                                                       '{18}',
                                                                       '{19}',
                                                                       '{20}',
                                                                       '{21}',
                                                                       '{22}',
                                                                       '{23}',
                                                                       '{24}',
                                                                       '{25}',
                                                                       '{26}',
                                                                       '{27}',
                                                                       '{28}')
                                                                    ", boxCode,
                                                                     dataRow["ZBOXTYP"], dataRow["WERKS"], dataRow["IN_DATE"],
                                                                     dataRow["ZPKMODE"], dataRow["BWERKS"], dataRow["COND"],
                                                                     dataRow["MEMO"], dataRow["XBLB"], dataRow["CHK531"],
                                                                     dataRow["LVORM"], dataRow["LGORT"], dataRow["ZSTLOC"],
                                                                     dataRow["SWC001"], dataRow["SWC002"], dataRow["SWC003"],
                                                                     dataRow["SWC004"], dataRow["SWC005"], dataRow["SWC006"],
                                                                     dataRow["SWC007"], dataRow["SWC008"], dataRow["SWC011"],
                                                                     dataRow["MATNR"], dataRow["MAKTX"], dataRow["CHARG"], dataRow["USER"],
                                                                     dataRow["OEM_MAT"], dataRow["OEM_MATX"], dataRow["OEM_WRK"]);
                                }

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlCommand) <= 0)
                                {
                                    throw new Exception("保存电池片入库标签头数据失败!");
                                }

                                #endregion

                                #region Delete Cell Label Detail Data

                                sqlCommand = string.Format("DELETE WST_BQ_ZFCMARD WHERE BOXCODE = '{0}'", boxCode);

                                db.ExecuteNonQuery(transaction, CommandType.Text, sqlCommand);

                                #endregion

                                #region Save Cell Lable Detail Data

                                if (reqDS.Tables.Contains("ZFCMARD") && reqDS.Tables["ZFCMARD"].Rows.Count > 0)
                                {
                                    DbCommand insertCommand = connection.CreateCommand();

                                    insertCommand.CommandType = CommandType.Text;
                                    insertCommand.CommandText = @"INSERT INTO WST_BQ_ZFCMARD
                                                                      (BOXCODE,
                                                                       FCCODE,
                                                                       AUFNR,
                                                                       MENGE,
                                                                       MEINS,
                                                                       ZPLINE,
                                                                       LIFNR,
                                                                       LFNAME)
                                                                    VALUES
                                                                      (:P1,
                                                                       :P2,
                                                                       :P3,
                                                                       :P4,
                                                                       :P5,
                                                                       :P6,
                                                                       :P7,
                                                                       :P8)";

                                    db.AddInParameter(insertCommand, "P1", DbType.String, "BOXCODE", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P2", DbType.String, "FCCODE", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P3", DbType.String, "AUFNR", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P4", DbType.Decimal, "MENGE", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P5", DbType.String, "MEINS", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P6", DbType.String, "ZPLINE", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P7", DbType.String, "LIFNR", DataRowVersion.Current);
                                    db.AddInParameter(insertCommand, "P8", DbType.String, "LFNAME", DataRowVersion.Current);

                                    if (db.UpdateDataSet(reqDS, "ZFCMARD", insertCommand, null, null, transaction) <= 0)
                                    {
                                        throw new Exception("保存电池片入库标签明细数据失败!");
                                    }
                                }

                                #endregion

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;

                    SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(reqDS, "CellLabelInboundExceptionData", boxCode);
                }
            }
            else
            {
                msg = "电池片入库标签数据不存在!";
            }

            return msg;
        }

        /// <summary>
        /// Delete Cell Label Data
        /// </summary>
        /// <param name="boxCode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-06-24 09:13:42
        public string DeleteCellLabelData(string boxCode, string user)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(boxCode))
            {
                msg = "电池片入库标签箱条码不允许为空!";
            }
            else
            {
                #region Delete Cell Label Data

                try
                {
                    string sqlCommand = string.Format("UPDATE WST_BQ_ZFCMARDH SET LVORM = '1', EDITOR = '{1}', EDIT_TIME = SYSDATE WHERE BOXCODE = '{0}'", boxCode, user);

                    if (db.ExecuteNonQuery(CommandType.Text, sqlCommand) <= 0)
                    {
                        throw new Exception("删除电池片入库标签数据失败!");
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;

                    SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(null, "CellLabelInboundExceptionData", boxCode);
                }

                #endregion
            }

            return msg;
        }

        /// <summary>
        /// Delete Cell Label Data By PackageCode
        /// </summary>
        /// <param name="packageCode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-06-24 09:20:06
        public string DeleteCellLabelDataByPackageCode(string packageCode, string user)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(packageCode))
            {
                msg = "电池片入库标签小包条码不允许为空!";
            }
            else
            {
                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Get Virtual BoxCode By PackageCode

                                string sqlCommand = string.Format(@"SELECT Z.BOXCODE FROM WST_BQ_ZFCMARD Z, WST_BQ_ZFCMARDH ZH 
                                                                    WHERE ZH.BOXCODE = Z.BOXCODE AND ZH.ZBOXTYP = 'X' AND ZH.LVORM = '0' AND Z.FCCODE = '{0}'", packageCode);

                                object scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlCommand);

                                if (scalar == null || scalar == DBNull.Value)
                                {
                                    throw new Exception("电池片入库标签小包条码不存在!");
                                }

                                #endregion

                                #region Delete Cell Label Data

                                sqlCommand = string.Format("UPDATE WST_BQ_ZFCMARDH SET LVORM = '1', EDITOR = '{1}', EDIT_TIME = SYSDATE WHERE BOXCODE = '{0}'", scalar.ToString(), user);

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlCommand) <= 0)
                                {
                                    throw new Exception("删除电池片入库标签数据失败!");
                                }

                                #endregion

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;

                    SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(null, "CellLabelInboundExceptionData", packageCode);
                }
            }

            return msg;
        }

        /// <summary>
        /// Save Cell Inbound Data
        /// </summary>
        /// <param name="reqDS"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-06-08 18:44:30
        public string SaveCellInboundData(DataSet reqDS)
        {
            string msg = string.Empty;

            if (reqDS != null && reqDS.Tables.Contains("ZXBKO") && reqDS.Tables["ZXBKO"].Rows.Count > 0)
            {
                string inboundNo = string.Empty;

                try
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Save Cell Inbound Header Data

                                DataRow dataRow = reqDS.Tables["ZXBKO"].Rows[0];

                                inboundNo = dataRow["ZXBNO"].ToString();

                                string sqlCommand = string.Format("SELECT KO.LVORM FROM WST_RK_ZXBKO KO WHERE KO.ZXBNO = '{0}'", inboundNo);

                                object scalar = db.ExecuteScalar(transaction, CommandType.Text, sqlCommand);

                                if (scalar != null && scalar != DBNull.Value)
                                {
                                    if (scalar.ToString() == "1")
                                    {
                                        throw new Exception("电池片入库单已经删除!");
                                    }
                                    else
                                    {
                                        sqlCommand = string.Format(@"UPDATE WST_RK_ZXBKO
                                                                       SET WERKS  = '{1}',
                                                                           ORDTYP = '{2}',
                                                                           BANCI  = '{3}',
                                                                           BUDAT  = TO_DATE('{4}','YYYY-MM-DD'),
                                                                           MEMO1  = '{5}',
                                                                           MEMO2  = '{6}',
                                                                           MATNR  = '{7}',
                                                                           MAKTX  = '{8}',
                                                                           LVORM  = '{9}',
                                                                           EDITOR = '{10}',
                                                                           EDIT_TIME = SYSDATE
                                                                     WHERE ZXBNO = '{0}'
                                                                    ", inboundNo, dataRow["WERKS"], dataRow["ORDTYP"], dataRow["BANCI"], dataRow["BUDAT"], dataRow["MEMO1"], dataRow["MEMO2"], dataRow["MATNR"], dataRow["MAKTX"], dataRow["LVORM"], dataRow["USER"]);
                                    }
                                }
                                else
                                {
                                    sqlCommand = string.Format(@"INSERT INTO WST_RK_ZXBKO
                                                                      (ZXBNO,
                                                                       WERKS,
                                                                       ORDTYP,
                                                                       BANCI,
                                                                       BUDAT,
                                                                       MEMO1,
                                                                       MEMO2,
                                                                       MATNR,
                                                                       MAKTX,
                                                                       LVORM,
                                                                       CREATOR)
                                                                    VALUES
                                                                      ('{0}',
                                                                       '{1}',
                                                                       '{2}',
                                                                       '{3}',
                                                                       TO_DATE('{4}','YYYY-MM-DD'),
                                                                       '{5}',
                                                                       '{6}',
                                                                       '{7}',
                                                                       '{8}',
                                                                       '{9}',
                                                                       '{10}')", inboundNo, dataRow["WERKS"], dataRow["ORDTYP"], dataRow["BANCI"], dataRow["BUDAT"], dataRow["MEMO1"], dataRow["MEMO2"], dataRow["MATNR"], dataRow["MAKTX"], dataRow["LVORM"], dataRow["USER"]);
                                }

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlCommand) <= 0)
                                {
                                    throw new Exception("保存电池片入库单数据失败!");
                                }

                                #endregion

                                #region Delete Cell Inbound Product And Byproduct Detail Data

                                sqlCommand = string.Format("DELETE WST_RK_ZXBPO WHERE ZXBNO = '{0}'", inboundNo);

                                db.ExecuteNonQuery(transaction, CommandType.Text, sqlCommand);

                                sqlCommand = string.Format("DELETE WST_RK_ZXBPOF WHERE ZXBNO = '{0}'", inboundNo);

                                db.ExecuteNonQuery(transaction, CommandType.Text, sqlCommand);

                                #endregion

                                #region Save Cell Inbound Product Detail Data

                                if (reqDS.Tables.Contains("ZXBPO") && reqDS.Tables["ZXBPO"].Rows.Count > 0)
                                {
                                    DbCommand productInsertCommand = connection.CreateCommand();

                                    productInsertCommand.CommandType = CommandType.Text;
                                    productInsertCommand.CommandText = @"INSERT INTO WST_RK_ZXBPO
                                                                      (ZXBNO,
                                                                       BARCODE,
                                                                       AUFNR,
                                                                       LGORT,
                                                                       CHARG,
                                                                       MENGE,
                                                                       MEINS,
                                                                       ZSTLOC,
                                                                       ZPLINE,
                                                                       KZTXT)
                                                                    VALUES
                                                                      (:P1,
                                                                       :P2,
                                                                       :P3,
                                                                       :P4,
                                                                       :P5,
                                                                       :P6,
                                                                       :P7,
                                                                       :P8,
                                                                       :P9,
                                                                       :P10)";

                                    db.AddInParameter(productInsertCommand, "P1", DbType.String, "ZXBNO", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P2", DbType.String, "BARCODE", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P3", DbType.String, "AUFNR", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P4", DbType.String, "LGORT", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P5", DbType.String, "CHARG", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P6", DbType.Decimal, "MENGE", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P7", DbType.String, "MEINS", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P8", DbType.String, "ZSTLOC", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P9", DbType.String, "ZPLINE", DataRowVersion.Current);
                                    db.AddInParameter(productInsertCommand, "P10", DbType.String, "KZTXT", DataRowVersion.Current);

                                    if (db.UpdateDataSet(reqDS, "ZXBPO", productInsertCommand, null, null, transaction) <= 0)
                                    {
                                        throw new Exception("保存电池片入库单正产品数据失败!");
                                    }
                                }

                                #endregion

                                #region Save Cell Inbound Byproduct Detail Data

                                if (reqDS.Tables.Contains("ZXBPOF") && reqDS.Tables["ZXBPOF"].Rows.Count > 0)
                                {
                                    DbCommand byproductInsertCommand = connection.CreateCommand();

                                    byproductInsertCommand.CommandType = CommandType.Text;
                                    byproductInsertCommand.CommandText = @"INSERT INTO WST_RK_ZXBPOF
                                                                              (ZXBNO, AUFNR, MATNR, MAKTX, LGORT, MENGE, MEINS, ZPLINE)
                                                                            VALUES
                                                                              (:P1, :P2, :P3, :P4, :P5, :P6, :P7, :P8)";

                                    db.AddInParameter(byproductInsertCommand, "P1", DbType.String, "ZXBNO", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P2", DbType.String, "AUFNR", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P3", DbType.String, "MATNR", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P4", DbType.String, "MAKTX", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P5", DbType.String, "LGORT", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P6", DbType.Decimal, "MENGE", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P7", DbType.String, "MEINS", DataRowVersion.Current);
                                    db.AddInParameter(byproductInsertCommand, "P8", DbType.String, "ZPLINE", DataRowVersion.Current);

                                    if (db.UpdateDataSet(reqDS, "ZXBPOF", byproductInsertCommand, null, null, transaction) <= 0)
                                    {
                                        throw new Exception("保存电池片入库单副产品数据失败!");
                                    }
                                }

                                #endregion

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;

                    SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(reqDS, "CellLabelInboundExceptionData", inboundNo);
                }
            }
            else
            {
                msg = "电池片入库单数据不存在!";
            }

            return msg;
        }

        /// <summary>
        /// Delete Cell Inbound Data
        /// </summary>
        /// <param name="inboundNo"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-06-08 18:44:40
        public string DeleteCellInboundData(string inboundNo, string user)
        {
            string msg = string.Empty;

            if (string.IsNullOrEmpty(inboundNo))
            {
                msg = "电池片入库单号不允许为空!";
            }
            else
            {
                #region Delete Cell Inbound Data

                try
                {
                    string sqlCommand = string.Format("UPDATE WST_RK_ZXBKO SET LVORM = '1', EDITOR = '{1}', EDIT_TIME = SYSDATE WHERE ZXBNO = '{0}'", inboundNo, user);

                    if (db.ExecuteNonQuery(CommandType.Text, sqlCommand) <= 0)
                    {
                        throw new Exception("删除电池片入库单数据失败!");
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;

                    SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.SaveDataSetData(null, "CellLabelInboundExceptionData", inboundNo);
                }

                #endregion
            }

            return msg;
        }

        /// <summary>
        /// query In Store Info data
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        /// Owner:vicky Gao 2011-06-20
        public DataSet InStoreInfoQueryResult(DataSet dataSet)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                #region main Sql
                string sql = @"select zxbno,
                                       werks,
                                       budat,
                                       banci,
                                       lifnr,
                                       aufnr,
                                       barcode,
                                       lgort,
                                       charg,
                                       menge,
                                       meins,
                                       barcode,
                                       zpline,
                                       ordtyp,
                                       lvorm,
                                       maktx,
                                       matnr
                                  from v_wst_rk s
                                 where 1 = 1 ";
                #endregion

                #region 组合查询条件
                if (dataSet.Tables.Count > 0)
                {
                    string lowValue, highValue;
                    bool isSqlOr = false;
                    bool isBracketsEnd = false;
                    string operationSql = string.Empty;
                    for (int i = 0; i < dataSet.Tables.Count; i++)
                    {

                        //入库方式单独处理
                        if (dataSet.Tables[i].TableName.ToUpper() == "ORDTYP")
                        {
                            string orderType = dataSet.Tables["ORDTYP"].Rows[0]["OTYPE"].ToString();
                            if (!string.IsNullOrEmpty(orderType) && orderType.ToUpper() != "ALL")
                            {
                                sql = sql + " and   " + dataSet.Tables[i].TableName + " = '" + orderType + "'";
                            }
                        }

                        else //根据选择的条件拼接Sql
                        {
                            string operation = string.Empty;
                            for (int j = 0; j < dataSet.Tables[i].Rows.Count; j++)
                            {
                                isSqlOr = false;
                                isBracketsEnd = false;

                                operationSql = string.Empty;
                                lowValue = dataSet.Tables[i].Rows[j]["LOW"].ToString();
                                highValue = dataSet.Tables[i].Rows[j]["HIGH"].ToString();
                                if (!string.IsNullOrEmpty(lowValue))
                                {
                                    sql = sql + " and (" + dataSet.Tables[i].Rows[j]["SIGN"] + " " + lowValue;
                                    if (lowValue.IndexOf(">") < 0)
                                    {
                                        isSqlOr = true;
                                    }
                                    else
                                    {
                                        isSqlOr = false;
                                    }
                                    isBracketsEnd = true;

                                }

                                if (!string.IsNullOrEmpty(highValue))
                                {
                                    if (isSqlOr)
                                        operationSql = "or ";
                                    else
                                        operationSql = "and ";

                                    sql = sql + operationSql + dataSet.Tables[i].Rows[j]["SIGN"] + " " + highValue;
                                }
                                if (isBracketsEnd)
                                    sql = sql + ")";

                            }
                        }
                    }

                }

                sql = sql + "  order by  zxbno,budat,aufnr,zpline";
                #endregion
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }

            return dsReturn;
        }

        /// <summary>
        /// 检测A级电池片入库数是否超过产出数
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet CheckInStoreQuantity(DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables.Contains("IS_KO") && dataSet.Tables.Contains("IT_PO"))
                    {
                        #region Get orderType and Boxcode
                        string ordType=string.Empty;
                        if(dataSet.ExtendedProperties.Contains("I_ORDTYP"))
                        {
                            ordType = dataSet.ExtendedProperties["I_ORDTYP"].ToString();//入库类型
                        }
                        string zxbNo = string.Empty;
                        if (dataSet.ExtendedProperties.Contains("I_ZXBNO"))
                        {
                            zxbNo = dataSet.ExtendedProperties["I_ZXBNO"].ToString();//入库单号  
                            ordType = zxbNo.Substring(0, 1);
                        }
                        #endregion

                        #region get sum quantity group by workorder 
                        DataTable orderNoTable = new DataTable();
                        if (ordType == "L")
                        {
                            string boxCodes = string.Empty;
                            foreach (DataRow dataRow in dataSet.Tables["IT_PO"].Rows)
                            {                                
                                boxCodes += dataRow["BARCODE"] + ",";
                            }
                            string sql = "select t.aufnr,t.menge from wst_bq_zfcmard t where 1=1";
                            sql = sql + SolarViewer.Hemera.Utils.StaticFuncsUtils.Utils.BuilderWhereConditionString("t.boxcode", boxCodes.Substring(0,boxCodes.Length-1).Split(','));
                            DataTable tempTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                            orderNoTable = GetGroupTable(tempTable);
                        }
                        else if (ordType == "X")
                        {
                            orderNoTable = GetGroupTable(dataSet.Tables["IT_PO"]);
                        }
                        #endregion

                        #region check quantity
                        string returnOrder;//检测不成功的订单号
                        int outQuantity;//分检产出总数
                        int rkQuantity;//工单已入库数量
                        int inputQuantity;//即将入库数量
                        if (CheckInStoreQuantityByWorkOrder(orderNoTable, zxbNo, out returnOrder, out outQuantity, out rkQuantity, out inputQuantity))
                        {
                            //入库超出产出
                            string errorMsg ="工单"+returnOrder+"入库数超出了分检产出总数,";
                           
                            errorMsg += "分检产出数量："+outQuantity.ToString();
                            if (outQuantity != 0)
                            {
                                errorMsg += ";已入库数量："+rkQuantity.ToString();
                                errorMsg += ";即将入库数量："+inputQuantity.ToString();
                            }
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1,errorMsg);
                        }
                        else
                        {
                            //检测成功
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, "检测成功");
                        }
                        #endregion
                    }
                    else
                    {
                        //副产品不用检测
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, "不用检查");
                    }
                }               
            }
            catch (Exception ex)
            {
                //捕获到异常，执行失败
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, -1, "执行异常："+ex.Message);
                LogService.LogError("CheckInStoreQuantity Error:"+ex.Message);
            }
            return dsReturn;
        }

        private bool CheckInStoreQuantityByWorkOrder(DataTable orderNoTable, string zxbNo, out string returnOrder, out int outQuantity, out int rkQuantity,out int inputQuantity)
        {
            returnOrder = string.Empty;
            outQuantity = -1;
            rkQuantity = -1;
            inputQuantity = -1;
            try
            {
                foreach (DataRow dataRow in orderNoTable.Rows)
                {                    
                    string workOrderNumber=dataRow["AUFNR"].ToString().TrimStart('0');
                    #region 获取订单产出数量
                       string sql = string.Format(@"select sum(t.qty_out) qty_out
                                            from v_wip_online_qty t
                                           where t.route_step_name  like'%分检%'
                                             and t.order_number ='{0}'
                                             and t.QTY_TYPE='LT'
                                             group by t.order_number", workOrderNumber);
                        object outQtyObj = db.ExecuteScalar(CommandType.Text, sql);
                        if (outQtyObj == null || outQtyObj == DBNull.Value || outQtyObj.ToString()=="0")
                        {
                            returnOrder = workOrderNumber;
                            outQuantity = 0;
                            return true;
                        }
                    #endregion

                    #region 获取工单已入库数量
                    //工单并包入库数量
                        sql = @"select nvl(sum(t.menge),0) x_Store
                                         from v_rk t
                                        where t.aufnr='" + workOrderNumber + "'";
                        if (zxbNo != string.Empty)
                        {
                            sql += @" and t.zxbno!='" + zxbNo + "'";
                        }
                        sql += " and substr(t.charg,0,1)='A' group by t.aufnr";
                        int LQuantity =Convert.ToInt32(db.ExecuteScalar(CommandType.Text,sql));
                   //工单散包入库数量
                    sql= @"           select nvl(sum(j.menge),0) x_store
                                         from wst_rk_zxbko a, wst_rk_zxbpo j
                                        where a.zxbno = j.zxbno
                                          and a.lvorm = '0'
                                          and a.ordtyp = 'X' 
                                          and substr(j.charg,0,1)='A'
                                          and j.aufnr ='" + workOrderNumber + "'";
                    if (zxbNo != string.Empty)
                    {
                        sql += @" and a.zxbno!='"+zxbNo+"'";
                    }
                    sql += " group by j.aufnr";
                    int XQuantity = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                    //object storeQtyObj = db.ExecuteScalar(CommandType.Text, sql);
                    #endregion

                    if (LQuantity+XQuantity>0)
                    {
                        rkQuantity = LQuantity+XQuantity;                       
                    }
                    else
                    {
                        rkQuantity = 0; 
                    }
                    inputQuantity=Convert.ToInt32(Convert.ToDecimal(dataRow["MENGE"])) ;
                    if(rkQuantity+inputQuantity>Convert.ToInt32(outQtyObj))
                    {
                        returnOrder = workOrderNumber;
                        outQuantity = Convert.ToInt32(outQtyObj);                       
                        return true;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("CheckInStoreQuantityByWorkOrder Error:"+ex.Message);
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// DataTable Group by
        /// </summary>
        /// <param name="p_Table"></param>
        /// <returns></returns>
        public DataTable GetGroupTable(DataTable p_Table)
        {
            DataTable Table = new DataTable();
            Table.Columns.Add("AUFNR");
            Table.Columns.Add("MENGE");
            IList<string> List = new List<string>();

            DataColumn Column = new DataColumn("SumC", typeof(System.Decimal));
            p_Table.Columns.Add(Column);
            Column.Expression = "convert(MENGE,'System.Decimal')";         

            foreach (DataRow Row in p_Table.Rows)
            {
                object ValueA = Row["AUFNR"];
                object ValueC = p_Table.Compute("sum(SumC)", "AUFNR='" + ValueA + "'");

                string Key = ValueA.ToString();
                if (List.IndexOf(Key) == -1)
                {
                    Table.Rows.Add(new object[] { ValueA, ValueC });
                    List.Add(Key);
                }
            }           
            return Table;
        }

        #region 获取最近一天使用过的批号和库位信息
        /// <summary>
        /// cet charge and lgort
        /// </summary>
        /// <returns></returns>
        /// rayna liu 
        public DataSet GetChargAndLgort()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = @"select distinct t.charg
                          from wst_bq_zfcmardh t
                         where t.zboxtyp = 'L'
                           and t.create_time > (sysdate - interval '1' day) ";
                DataTable chargeTable = new DataView(db.ExecuteDataSet(CommandType.Text, sql).Tables[0]).ToTable("CHARG");              
                dsReturn.Tables.Add(chargeTable);

                sql = @"select distinct t.lgort
                      from wst_bq_zfcmardh t
                     where t.zboxtyp = 'L'
                       and t.create_time > (sysdate - interval '1' day)"; 
                DataTable lgortTable = new DataView(db.ExecuteDataSet(CommandType.Text, sql).Tables[0]).ToTable("LGORT");               
                dsReturn.Tables.Add(lgortTable);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetChargeAndLgort Error:" + ex.Message);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        #endregion     

        #region Get Eixst BoxCode By Fccode
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fccode"></param>
        /// <returns></returns>
        /// add by zxa 20110915
        public DataSet GetExistBoxCodeByFccode(string fccode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"select t.boxcode 
                               from wst_bq_zfcmard t,wst_bq_zfcmardh h
                               where t.boxcode = h.boxcode
                               and h.lvorm = '0'
                               and t.fccode = '{0}'",fccode);

                string boxCode = Convert.ToString(db.ExecuteScalar(CommandType.Text, sql));
                if (string.IsNullOrEmpty(boxCode))
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, boxCode);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetExistBoxCodeByFccode Error:" + ex.Message);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;

        }
        #endregion

        #region Get Eixst ZxbNo By boxcode
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fccode"></param>
        /// <returns></returns>
        /// add by zxa 20110915
        public DataSet GetExistZxbNoByBarcode(string barcode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"select k.zxbno 
                                             from wst_rk_zxbko k,wst_rk_zxbpo p
                                             where k.zxbno = p.zxbno
                                             and k.lvorm = '0'
                                             and p.barcode = '{0}'", barcode);

                string zxbNo = Convert.ToString(db.ExecuteScalar(CommandType.Text, sql));
                if (string.IsNullOrEmpty(zxbNo))
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, zxbNo);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetExistZxbNoByBarcode Error:" + ex.Message);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;

        }
        #endregion
        /// <summary>
        /// 获取外箱标签创建时间
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        /// add by zxa
        public string GetCellLabelInbountTime(string barcode)
        {
            string strCreateTime = string.Empty;
            try
            {
                string sql = string.Format(@"select t.create_time 
                                             from wst_bq_zfcmardh t
                                             where t.boxcode = '{0}'", barcode);

                strCreateTime = Convert.ToString(db.ExecuteScalar(CommandType.Text, sql));
               
            }
            catch (Exception ex)
            {
                LogService.LogError("GetExistZxbNoByBarcode Error:" + ex.Message);
            }

            return strCreateTime;

        }



        //vicky add
        #region Get CalDay By Shift value ?
        public string GetCalDayByShift(string shiftValue)
        {
            string calDay = string.Empty;
            try
            {

                string sql = @"select t.day
                              from cal_schedule_day t, v_shift_overtime t1
                             where to_date('2011-6-23 1:00:00','yyyy-mm-dd hh24:mi:ss') between t.starttime and t.endtime
                               and t.shift_key = t1.shift_key
                               and t.shift_value = '" + shiftValue + "'";
                object o = db.ExecuteScalar(CommandType.Text, sql);
                if (o != null && o != DBNull.Value)
                {
                    calDay = o.ToString();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetCalDayByShift Error: " + ex.Message);

            }

            return calDay;
        }
        #endregion



        #region 判断班别与日期是否匹配?
        public bool IsCalDayMapingShift(string calDay,string shiftValue)
        {
            bool isMapping = false;
            try
            {

                string sql = @"select 'Y'from cal_schedule_day t, v_shift_overtime t1 where to_date(t.day, 'yyyy-mm-dd') = to_date('"+calDay+"', 'yyyy-mm-dd') and t.shift_key = t1.shift_key and t.shift_value = '"+shiftValue+"'";
                object o = db.ExecuteScalar(CommandType.Text, sql);
                if (o != null && o != DBNull.Value && (o.ToString()=="Y"))
                {
                    isMapping = true;
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("IsCalDayMapingShift Error: " + ex.Message);

            }

            return isMapping;
        }
        #endregion

    }
}
