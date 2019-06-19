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

namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 数据采集操作类。
    /// </summary>
    /// <remarks>
    /// 参数分组数据管理，抽检点数据管理，保存采集数据，获取采集相关数据等。
    /// </remarks>
    public class EDCEngine:AbstractEngine,IEDCEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EDCEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据抽样规则主键获取抽样规则信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含抽样规则主键的数据集。
        /// -------------------------
        /// {SP_KEY}
        /// -------------------------
        /// </param>
        /// <returns>包含抽样规则信息的数据集。 </returns>
        public DataSet GetSamplingInfo(DataSet dsParams)
        {            
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(EDC_SP_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string spKey = Convert.ToString(htParams[EDC_SP_FIELDS.FIELD_SP_KEY]);
                    string sql = @"SELECT * FROM EDC_SP WHERE SP_KEY='" + spKey.PreventSQLInjection() + "'";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0006}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSamplingInfo Error: "+ex.Message);
            }           
            return dsReturn;
        }
        /// <summary>
        /// 根据管控数据组主键获取需要管控项的信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含管控数据组主键的数据集。
        /// ----------------------------
        /// {EDC_KEY}
        /// ----------------------------
        /// </param>
        /// <returns>
        /// 包含管控项数据的数据集。
        /// 【A.PARAM_KEY,A.PARAM_NAME,A.MANDATORY,A.ISDERIVED,A.DATA_TYPE,A.DEVICE_TYPE】
        /// </returns>
        public DataSet GetParams(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string edcKey = Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY]);
                    string sql = @"SELECT A.PARAM_KEY,A.PARAM_NAME,A.MANDATORY,A.ISDERIVED,A.DATA_TYPE,A.DEVICE_TYPE
                                 FROM BASE_PARAMETER A,EDC_MAIN_PARAM B
                                 WHERE A.PARAM_KEY=B.PARAM_KEY
                                 AND B.EDC_KEY='" + edcKey.PreventSQLInjection() + "'";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0006}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetParams Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存数据采集实例数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含数据采集实例数据的数据集。
        /// </param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet SaveEDCMainIN(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();           
            try
            {                
                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction  
                    DbTransaction dbTrans = dbConn.BeginTransaction();
                    try
                    {
                        if (dsParams.Tables.Contains(EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME))
                        {
                            EDCManagement.SaveEDCMainIn(db, dbTrans, dsParams);
                        }
                        dbTrans.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("SaveEDCMainIN Error: "+ex.Message);
                    }
                    finally
                    {
                        //Close Connection
                        dbConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SaveEDCMainIN Error: "+ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次主键获取批次采集的数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次采集数据的数据集。
        /// 【EDC_KEY,SP_COUNT,SP_UNITS,EDC_INS_KEY】
        /// </returns>
        public DataSet GetEDCMainIN(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT C.EDC_KEY,C.SP_COUNT,C.SP_UNITS,C.EDC_INS_KEY 
                        FROM POR_LOT A,POR_ROUTE_STEP B,EDC_MAIN_INS C
                        WHERE A.EDC_INS_KEY=C.EDC_INS_KEY
                        AND A.CUR_STEP_VER_KEY=B.ROUTE_STEP_KEY                     
                        AND A.LOT_KEY='" + lotKey.PreventSQLInjection() + "'";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEDCMainIN Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存采集到的明细数据。
        /// </summary>
        /// <param name="dsParams">包含到采集到的明细数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet SaveEDCCollectionData(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;

            DataSet dsReturn = new DataSet(); 
            DbConnection dbConn = null;
            DbTransaction dbTran = null;           
            try
            {
                dbConn = db.CreateConnection();
                //Open Connection
                dbConn.Open();
                //Create Transaction  
                dbTran = dbConn.BeginTransaction();
                if (dsParams.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
                {                   
                    EDCManagement.SaveEDCCollectionData(db, dbTran, dsParams);
                } 
                dbTran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SaveEDCCollectionData Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                dbConn.Close();
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("SaveEDCCollectionData Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
        /// <summary>
        /// 根据批次主键和批次数据采集主键获取批次已采集的数据集合。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcInsKey">批次数据采集主键。</param>
        /// <returns>
        /// 包含批次已采集的数据集合的数据集对象。
        /// [SP_UNIT_SEQ,PARAM_KEY,EDC_INS_KEY,PARAM_VALUE,COL_KEY,VALID_FLAG,
        /// FAILED_RULES,SP_SAMP_SEQ,EDITOR,EDIT_TIME,EDIT_TIMEZONE,DELETED_FLAG]
        /// </returns>
        public DataSet GetEDCCollectionData(string lotKey,string edcInsKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (string.IsNullOrEmpty(lotKey.Trim()))
                {
                    sql = string.Format(@"SELECT b.SP_UNIT_SEQ,b.PARAM_KEY,b.EDC_INS_KEY,b.PARAM_VALUE,b.COL_KEY,b.VALID_FLAG,b.FAILED_RULES,b.SP_SAMP_SEQ,b.EDITOR,b.EDIT_TIME,b.EDIT_TIMEZONE,b.DELETED_FLAG
                                    FROM EDC_MAIN_INS t, EDC_COLLECTION_DATA b
                                    WHERE t.EDC_INS_KEY = b.EDC_INS_KEY
                                    AND b.DELETED_FLAG=0   
                                    AND t.EDC_INS_KEY = '{0}'                                 
                                    AND EXISTS (SELECT a.PARAM_KEY
                                                FROM EDC_POINT_PARAMS a,EDC_MAIN_INS c
                                                WHERE a.EDC_POINT_ROWKEY = c.EDC_POINT_KEY
                                                AND  b.PARAM_KEY=a.PARAM_KEY
                                                AND c.EDC_INS_KEY = '{0}')", edcInsKey.PreventSQLInjection());
                }
                else
                {
                    sql = string.Format(@"SELECT b.SP_UNIT_SEQ,b.PARAM_KEY,b.EDC_INS_KEY,b.PARAM_VALUE,b.COL_KEY,b.VALID_FLAG,b.FAILED_RULES,b.SP_SAMP_SEQ,b.EDITOR,b.EDIT_TIME,b.EDIT_TIMEZONE,b.DELETED_FLAG
                                    FROM EDC_MAIN_INS t, EDC_COLLECTION_DATA b
                                    WHERE t.EDC_INS_KEY = b.EDC_INS_KEY
                                    AND t.LOT_KEY = '{0}'
                                    AND b.DELETED_FLAG=0   
                                    AND t.EDC_INS_KEY = '{1}'                                 
                                    AND EXISTS (SELECT a.PARAM_KEY
                                                FROM EDC_POINT_PARAMS a,EDC_MAIN_INS c
                                                WHERE a.EDC_POINT_ROWKEY = c.EDC_POINT_KEY
                                                AND  b.PARAM_KEY=a.PARAM_KEY
                                                AND c.EDC_INS_KEY = '{1}')",
                                    lotKey.PreventSQLInjection(), 
                                    edcInsKey.PreventSQLInjection());
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEDCCollectionData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存在线采集的管控项数据值。
        /// </summary>
        /// <param name="dsParams">包含管控项数据值的数据集。</param>
        /// <param name="paramCount">参数采集数的个数。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcInsKey">数据采集实例主键。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet SaveOnlineEDCData(DataSet dsParams,int paramCount,string lotKey,string edcInsKey)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sql = "";            
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                //查询是否已经完成了数据采集。
                sql = @"SELECT COL_END_TIME FROM EDC_MAIN_INS WHERE EDC_INS_KEY=@edcInsKey";
                DbCommand dbCmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCmd, "edcInsKey", DbType.String, edcInsKey);
                string colEndTime = Convert.ToString(db.ExecuteScalar(dbCmd, dbTran));
                //已完成了数据采集。
                if (!string.IsNullOrEmpty(colEndTime))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "该批次数据采集已被他人完成，请关闭采集窗口进行其他操作。");
                    dbTran.Rollback();
                    return dsReturn;
                }
                //保存采集到的管控项目数据。
                if (dsParams.Tables.Count>0 && dsParams.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
                {   
                    EDCManagement.SaveEDCCollectionData(db, dbTran, dsParams);
                }
                //需要HOLD批次。
                if (dsParams.Tables.Count > 0 && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    EDCManagement.CheckAndHoldLot(db, dbTran, lotKey, dsParams);
                }
                
                string editor = Convert.ToString(dsParams.ExtendedProperties[EDC_MAIN_INS_FIELDS.FIELD_EDITOR]);

                sql = "SELECT COUNT(*) FROM EDC_COLLECTION_DATA a WHERE a.EDC_INS_KEY='" + edcInsKey.PreventSQLInjection() + "' AND a.DELETED_FLAG=0";
                int paramCounted = Convert.ToInt32(db.ExecuteScalar(dbTran,CommandType.Text, sql));
                //已经采集完成。
                if (paramCounted >= paramCount)
                {
                    //更新批次状态。
                    int state = -1;
                    sql = @"SELECT STATE_FLAG FROM POR_LOT WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
                    state = Convert.ToInt32(db.ExecuteScalar(dbTran,CommandType.Text, sql));
                    if (state == 0 || state == 1)
                    {
                        sql = @"UPDATE POR_LOT SET STATE_FLAG=2 WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
                    }
                    else if (state == 4 || state == 5)
                    {
                        sql = @"UPDATE POR_LOT SET STATE_FLAG=9 WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
                    }
                    db.ExecuteNonQuery(dbTran,CommandType.Text, sql);
                    //更新采集实例。
                    sql = string.Format(@"UPDATE EDC_MAIN_INS 
                                          SET COL_END_TIME= GETDATE(),EDIT_TIME= GETDATE(),EDITOR='{0}'
                                          WHERE EDC_INS_KEY='{1}'",
                                          editor.PreventSQLInjection(),
                                          edcInsKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran,CommandType.Text, sql);
                }
                dbTran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SaveOnlineEDCData Error: " + ex.Message);
            }
            finally
            {
                dbConn.Close();
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("SaveOnlineEDCData Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
        /// <summary>
        /// 保存离线采集到的管控项数据值。
        /// </summary>
        /// <param name="dataset">
        /// 包含采集到的管控项数据值的数据集。
        /// <see cref="EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME"/>、 <see cref="EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet SaveOfflineEDCData(DataSet dsParams)
        {
            System.DateTime startTime = System.DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                //Open Connection
                dbConn.Open();
                //Create Transaction
                dbTran = dbConn.BeginTransaction();
                if (dsParams.Tables.Count > 0 && dsParams.Tables.Contains(EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME))
                {
                    EDCManagement.SaveEDCMainIns(db, dbTran, dsParams);
                }
                //保存采集到的管控项目数据。
                if (dsParams.Tables.Count > 0 && dsParams.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
                {
                    EDCManagement.SaveEDCCollectionData(db, dbTran, dsParams);
                }
                dbTran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SaveOfflineEDCData Error: " + ex.Message);
            }
            finally
            {
                //Close Connection
                dbConn.Close();
            }
            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("SaveOfflineEDCData Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 检查指定EDCPoint的是否有满足条件的抽检参数，从而判断是否需要进行抽检。
        /// （1）基础参数处于激活状态。
        /// （2）抽检点设置的需要抽检数量>0
        /// </summary>
        /// <returns>true:需要抽检，false:不需要抽检。</returns>
        public bool CheckEDCPointParams(string edcPointKey)
        {
            DataSet dsParams = GetPointParamsByPointKey(edcPointKey);
            string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsParams);
            if (string.IsNullOrEmpty(msg))
            {
                if (dsParams.Tables.Count > 0 && dsParams.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new Exception(msg);
            }
        }
        /// <summary>
        /// 检查抽检规则的设置从而判断是否需要进行抽检。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <param name="sampKey">抽样规则主键。</param>
        /// <param name="type">数据采集类型。
        /// <see cref="COMMON_FIELDS.FIELD_ACTIVITY_INEDC"/>
        /// 或者<see cref="COMMON_FIELDS.FIELD_ACTIVITY_OUTEDC"/>
        /// </param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>true:需要抽检，false:不需要抽检。</returns>
        public bool CheckSampling(string lotKey, string stepKey, string lineKey, string sampKey,string type,string equipmentKey)
        {
            double sampSize = 0;
            string sampType = string.Empty;
            string sqlCommand = string.Empty;
            string lastEdcTime = string.Empty;
            int edcCount =0;
            string activityType=string.Empty;
            try
            {
                //获取采样规则。
                sqlCommand = string.Format(@"SELECT T.SAMPLING_MODE, T.STRATEGY_SIZE FROM EDC_SP T WHERE T.SP_KEY = '{0}'",
                                             sampKey.PreventSQLInjection());

                using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, sqlCommand))
                {
                    while (dataReader.Read())
                    {
                        sampType = Convert.ToString(dataReader[0]);
                        sampSize = Convert.ToInt32(dataReader[1] == DBNull.Value ? 0 : dataReader[1]);
                    }
                    dataReader.Close();
                }
                //每批抽检。
                if (sampType == "Lot")
                    return true;
                //按时间抽检
                else if (sampType == "LastEdcTime")
                {
                    if (type == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC)
                    {
                        bool haveInEDC = HaveEDCBeforeTrackOut(lotKey, stepKey, lineKey);
                        bool haveUDA=HaveEDCOnStepUDA(lotKey); 
                        if (haveInEDC)
                        {
                            return true;
                        }
                        else if (!haveInEDC && haveUDA)
                        {
                            return false;
                        }
                    }
                    //获取指定工步 指定设备 最后OUTEDC的时间
                    sqlCommand = string.Format(@"SELECT MAX(W.EDIT_TIME),GETDATE()
                                                 FROM WIP_TRANSACTION W 
                                                 WHERE W.STEP_KEY='{0}' AND W.EQUIPMENT_KEY='{1}' AND W.ACTIVITY='{2}'
                                                 ORDER BY W.EDIT_TIME DESC",
                                                 stepKey.PreventSQLInjection(),
                                                 equipmentKey.PreventSQLInjection(), 
                                                 type.PreventSQLInjection());
                    using (IDataReader reader = db.ExecuteReader(CommandType.Text, sqlCommand))
                    {
                        if (reader.Read())
                        {                           
                            lastEdcTime = Convert.ToString(reader[0]);
                            if (string.IsNullOrEmpty(lastEdcTime))
                                return true;
                            DateTime dtCurTime = Convert.ToDateTime(reader[1]);
                            //当期时间和最后采集时间间隔分钟数是否>=策略时间
                            if ((dtCurTime - Convert.ToDateTime(lastEdcTime)).TotalMinutes >= sampSize)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                        reader.Close();
                    }
                }
                //按批次数量。
                else if (sampType == "LotAccount")
                {
                    if (type == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC)
                        activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT;
                    else
                        activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN;
                    if (type == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC)
                    {
                        bool haveInEDC = HaveEDCBeforeTrackOut(lotKey, stepKey, lineKey);
                        bool haveUDA = HaveEDCOnStepUDA(lotKey);
                        if (haveInEDC)
                        {
                            return true;
                        }
                        else if (!haveInEDC && haveUDA)//Step上配有InEDC而未做进站前抽检则出站不需要抽检
                        {
                            return false;
                        }
                    }
                    //先抓最后采集数据时间 在最后采集时间之后的进站或出站批次数量
                    sqlCommand = string.Format(@" SELECT COUNT(*) AS EDC_COUNT
                                                 FROM WIP_TRANSACTION T
                                                 WHERE T.STEP_KEY = '{0}'
                                                 AND T.EQUIPMENT_KEY = '{1}'
                                                 AND T.ACTIVITY = '{2}'
                                                 AND T.EDIT_TIME > (SELECT MAX(A.EDIT_TIME)
                                                                    FROM WIP_TRANSACTION A
                                                                    WHERE A.STEP_KEY = '{0}'
                                                                    AND A.EQUIPMENT_KEY = '{1}'
                                                                    AND A.ACTIVITY = '{3}')", 
                                                stepKey.PreventSQLInjection(),
                                                equipmentKey.PreventSQLInjection(),
                                                activityType.PreventSQLInjection(),
                                                type.PreventSQLInjection());
                    using (IDataReader read = db.ExecuteReader(CommandType.Text, sqlCommand))
                    {
                        if (read.Read())
                        {
                            edcCount =Convert.ToInt32(read[0]);
                            //如果批次数量==0 或者批次数量>抽检策略值。
                            if (edcCount == 0 || edcCount >= sampSize)
                            {
                                return true;
                            }     
                        }
                        read.Close();
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
        /// <summary>
        /// 指定批次在指定工步指定线别是否进行过进站前数据采集。（采集类型：COMMON_FIELDS.FIELD_ACTIVITY_INEDC）
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>true 有记录 false 没有记录。</returns>
        private bool HaveEDCBeforeTrackOut(string lotKey, string stepKey, string lineKey)
        {
            string sqlCommand = string.Format(@"SELECT COUNT(*)
                                                FROM WIP_TRANSACTION W 
                                                WHERE W.STEP_KEY='{0}' 
                                                      AND W.LINE_KEY='{1}' 
                                                      AND W.PIECE_KEY='{2}'
                                                      AND W.ACTIVITY='{3}'",
                                                stepKey.PreventSQLInjection(),
                                                lineKey.PreventSQLInjection(),
                                                lotKey.PreventSQLInjection(),
                                                ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC);
            int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 指定批次当前所在工步是否设置了EDC属性。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>true：设置了EDC属性，false:没有设置EDC属性。</returns>
        private bool HaveEDCOnStepUDA(string lotKey)
        {
            string sqlCommand = string.Format(@"SELECT B.ATTRIBUTE_NAME, B.ATTRIBUTE_VALUE
                                                FROM POR_LOT A, POR_ROUTE_STEP_ATTR B
                                                WHERE A.CUR_STEP_VER_KEY = B.ROUTE_STEP_KEY
                                                AND A.LOT_KEY = '{0}' AND B.ATTRIBUTE_NAME='EDC'", lotKey);
            using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, sqlCommand))
            {
                while (dataReader.Read())
                {
                    if (Convert.ToString(dataReader[0]) == "EDC")
                        return true;
                }
                dataReader.Close();
                return false;
            }
        }
        /// <summary>
        /// 指定批次在指定工序上是否进行了抽检。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <returns>true：进行抽检了。false：没有进行抽检。</returns>
        public bool CheckDependSampStep(string lotKey ,string operationName)
        {
            try
            {
                string sql = string.Format(@"SELECT COUNT(*) 
                                            FROM V_EDC_MAIN_INS t 
                                            WHERE t.LOT_KEY='{0}' AND t.ROUTE_STEP_NAME='{1}'",
                                            lotKey.PreventSQLInjection(),
                                            operationName.PreventSQLInjection());
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                //如果有数据，返回true，表示已经进行了抽检。
                if (count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //表示没有进行抽检。
            return false;
        }
        /// <summary>
        /// 获取数据抽检点设置数据。
        /// </summary>
        /// <param name="dtParams">
        /// 获取数据抽检点的查询条件。
        /// 数据表对象，数据表中必须包含两个列"name"和"value"。列name存放键名，列value存放键值。
        /// 键名:
        /// 产品号(POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER)、
        /// 工序名(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME)、
        /// 设备主键(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY)（可选）、
        /// ACTION_NAME(EDC_POINT_FIELDS.FIELD_ACTION_NAME)。
        /// </param>
        /// <returns>
        /// 包含数据抽检点的数据集对象。
        /// </returns>
        public DataSet CheckEdc(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();                               //包含数据抽检点和执行结果的数据集对象。
            try
            {
                string sqlCommand = string.Empty;                           //查询SQL字符串
                string equipmentKey = string.Empty;                         //设备主键字符串
                DataTable edcPointTable = new DataTable();                  //包含数据抽检点的数据表对象
                if (dtParams != null)
                {

                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);  //将数据表对象转换为哈希表对象。
                    if (htParams.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY))//哈希表中是否包含唯一标识设备的主键值。
                    {
                        equipmentKey = Convert.ToString(htParams[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                    }
                    string partNumber=Convert.ToString(htParams[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER]);
                    string operationName = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME]);
                    string actionName = Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_ACTION_NAME]);
                    string routeKey = Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY]);
                    string partType = string.Empty;
                    //先按工艺流程找抽检点设置数据
                    if (!string.IsNullOrEmpty(routeKey))
                    {
                        //按工艺流程+产品号+工序+设备主键找数据
                        if (!string.IsNullOrEmpty(equipmentKey))
                        {
                            sqlCommand = string.Format(@"SELECT * FROM EDC_POINT 
                                                         WHERE TOPRODUCT='{0}' AND OPERATION_NAME='{1}' 
                                                         AND EQUIPMENT_KEY='{2}' AND ACTION_NAME='{3}' 
                                                         AND ROUTE_VER_KEY='{4}'
                                                         AND POINT_STATUS = '1'",
                                                       partNumber.PreventSQLInjection(),
                                                       operationName.PreventSQLInjection(),
                                                       equipmentKey.PreventSQLInjection(),
                                                       actionName.PreventSQLInjection(),
                                                       routeKey.PreventSQLInjection());
                            edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        }
                        //上一步没有找到数据，按工艺流程+产品号+工序+设备主键为NULL 找数据
                        if (edcPointTable.Rows.Count == 0)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM EDC_POINT
                                                         WHERE TOPRODUCT='{0}'
                                                         AND OPERATION_NAME='{1}'
                                                         AND ACTION_NAME='{2}'
                                                         AND ROUTE_VER_KEY='{3}'
                                                         AND EQUIPMENT_KEY IS NULL 
                                                         AND POINT_STATUS = '1' ",
                                                       partNumber.PreventSQLInjection(),
                                                       operationName.PreventSQLInjection(),
                                                       actionName.PreventSQLInjection(), 
                                                       routeKey.PreventSQLInjection());
                            edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        }
                        //上一步没有找到数据，按工艺流程+产品类型+工序+设备主键找数据
                        if (edcPointTable.Rows.Count == 0)
                        {
                            sqlCommand=string.Format("SELECT PART_TYPE FROM POR_PART WHERE PART_NAME='{0}'", 
                                                      partNumber.PreventSQLInjection());
                            object objPartType = db.ExecuteScalar(CommandType.Text, sqlCommand);
                            if (objPartType != null)//根据产品号有抓到产品类型。
                            {
                                partType = Convert.ToString(objPartType);
                                sqlCommand = string.Format(@"SELECT * FROM EDC_POINT 
                                                             WHERE PART_TYPE='{0}'
                                                             AND OPERATION_NAME='{1}' 
                                                             AND EQUIPMENT_KEY='{2}' 
                                                             AND ACTION_NAME='{3}' 
                                                             AND ROUTE_VER_KEY='{4}'
                                                             AND POINT_STATUS = '1'",
                                                           partType.PreventSQLInjection(),
                                                           operationName.PreventSQLInjection(),
                                                           equipmentKey.PreventSQLInjection(),
                                                           actionName.PreventSQLInjection(), 
                                                           routeKey.PreventSQLInjection());
                                edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                            }
                        }
                        //上一步没有找到数据，按工艺流程+产品类型+工序找数据
                        if (edcPointTable.Rows.Count == 0 && !string.IsNullOrEmpty(partType))
                        {
                            sqlCommand = string.Format(@"SELECT * 
                                                     FROM EDC_POINT WHERE PART_TYPE='{0}' 
                                                     AND OPERATION_NAME='{1}' 
                                                     AND ACTION_NAME='{2}'
                                                     AND ROUTE_VER_KEY='{3}'
                                                     AND EQUIPMENT_KEY IS NULL AND POINT_STATUS = '1'",
                                                       partType.PreventSQLInjection(),
                                                       operationName.PreventSQLInjection(),
                                                       actionName.PreventSQLInjection(), 
                                                       routeKey.PreventSQLInjection());
                            edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        }
                    }
                    //如果按工艺流程找不到数据。
                    //按产品号+工序+设备主键找数据
                    if (edcPointTable.Rows.Count == 0 && !string.IsNullOrEmpty(equipmentKey))
                    {
                        sqlCommand = string.Format(@"SELECT * FROM EDC_POINT 
                                                     WHERE TOPRODUCT='{0}' AND OPERATION_NAME='{1}' 
                                                     AND EQUIPMENT_KEY='{2}' AND ACTION_NAME='{3}' 
                                                     AND ROUTE_VER_KEY IS NULL
                                                     AND POINT_STATUS = '1'",
                                                   partNumber.PreventSQLInjection(), 
                                                   operationName.PreventSQLInjection(),
                                                   equipmentKey.PreventSQLInjection(), 
                                                   actionName.PreventSQLInjection());
                        edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    }
                    //上一步没有找到数据，按产品号+工序+设备主键为NULL 找数据
                    if (edcPointTable.Rows.Count == 0)
                    {
                        sqlCommand = string.Format(@"SELECT * FROM EDC_POINT 
                                                     WHERE TOPRODUCT='{0}'
                                                     AND OPERATION_NAME='{1}' 
                                                     AND ACTION_NAME='{2}'
                                                     AND EQUIPMENT_KEY IS NULL 
                                                     AND ROUTE_VER_KEY IS NULL
                                                     AND POINT_STATUS = '1' ",
                                                   partNumber.PreventSQLInjection(), 
                                                   operationName.PreventSQLInjection(), 
                                                   actionName.PreventSQLInjection());
                        edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    }
                    
                    //上一步没有找到数据，按产品类型+工序+设备主键找数据
                    if (edcPointTable.Rows.Count == 0)
                    {
                        object objPartType = db.ExecuteScalar(CommandType.Text, string.Format("SELECT PART_TYPE FROM POR_PART WHERE PART_NAME='{0}'", partNumber));
                        if (objPartType != null)//根据产品号有抓到产品类型。
                        {
                            partType = Convert.ToString(objPartType);
                            sqlCommand = string.Format(@"SELECT * FROM EDC_POINT 
                                                         WHERE PART_TYPE='{0}'
                                                         AND OPERATION_NAME='{1}' 
                                                         AND EQUIPMENT_KEY='{2}'
                                                         AND ROUTE_VER_KEY IS NULL 
                                                         AND ACTION_NAME='{3}'
                                                         AND POINT_STATUS = '1'",
                                                       partType.PreventSQLInjection(), 
                                                       operationName.PreventSQLInjection(), 
                                                       equipmentKey.PreventSQLInjection(),
                                                       actionName.PreventSQLInjection());
                            edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                        }
                    }
                    //上一步没有找到数据，按产品类型+工序找数据
                    if (edcPointTable.Rows.Count == 0 && !string.IsNullOrEmpty(partType))
                    {
                        sqlCommand = string.Format(@"SELECT * FROM EDC_POINT 
                                                     WHERE PART_TYPE='{0}' 
                                                     AND OPERATION_NAME='{1}' 
                                                     AND ACTION_NAME='{2}'
                                                     AND EQUIPMENT_KEY IS NULL 
                                                     AND ROUTE_VER_KEY IS NULL
                                                     AND POINT_STATUS = '1' ",
                                                   partType.PreventSQLInjection(),
                                                   operationName.PreventSQLInjection(),
                                                   actionName.PreventSQLInjection());
                        edcPointTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    }
                    //设置数据表名称为“EDC_POINT”，并添加到数据集中。
                    edcPointTable.TableName = "EDC_POINT";
                    dsReturn.Merge(edcPointTable);
                    //添加执行结果到数据集中。
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                //添加执行结果到数据集中。
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("CheckEdc Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次数据采集主键和数据采集参数类型（R:根据设备类型正排序，否则根据设备类型倒排序。）获取可进行采集参数集合。
        /// </summary>
        /// <param name="edcInsKey">批次数据采集主键。</param>
        /// <param name="paramType">参数类型（R:根据设备类型正排序，否则根据设备类型倒排序。）</param>
        /// <returns>
        /// 包含批次采样参数点集合的数据集对象。
        /// [ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,
        /// UPPER_SPEC,UPPER_CONTROL,TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,
        /// PARAM_COUNT,PARAM_INDEX,PARAM_KEY,PARAM_FORMULA,Device_Type,DATA_TYPE,ISDERIVED,CALCULATE_TYPE]
        /// </returns>
        public DataSet GetPointParamsByEDCInsKey(string edcInsKey, string paramType)
        {
            DataSet dsParams = new DataSet();
            try
            {
                string sql = string.Empty;
                sql = string.Format(@"SELECT A.ROW_KEY,A.EDC_POINT_ROWKEY,A.EDC_NAME,A.EDC_VERSION,A.PARAM_NAME,
                                        A.UPPER_BOUNDARY,A.UPPER_SPEC,A.UPPER_CONTROL,A.TARGET,A.LOWER_CONTROL,A.LOWER_SPEC,
                                        A.LOWER_BOUNDARY,A.PARAM_COUNT,A.PARAM_INDEX,A.PARAM_KEY,A.PARAM_FORMULA,A.PARAM_TYPE,
                                        A.ALLOW_MIN_VALUE,A.ALLOW_MAX_VALUE,
                                        B.Device_Type,B.DATA_TYPE,B.ISDERIVED,B.CALCULATE_TYPE
                                    FROM EDC_POINT_PARAMS A,BASE_PARAMETER B
                                    WHERE A.PARAM_KEY =B.PARAM_KEY
                                    AND A.PARAM_COUNT>0 AND B.STATUS=1
                                    AND A.EDC_POINT_ROWKEY =(SELECT T.EDC_POINT_KEY 
                                                             FROM  EDC_MAIN_INS T
                                                             WHERE T.EDC_INS_KEY = '{0}')", edcInsKey.PreventSQLInjection());
                if (paramType == "R")//根据设备类型正排序。
                {
                    sql = sql + " ORDER BY B.DEVICE_TYPE,A.PARAM_INDEX";
                }
                else
                {
                    sql = sql + " ORDER BY B.DEVICE_TYPE DESC,A.PARAM_INDEX";
                }
                dsParams = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsParams, string.Empty);
            }
            catch (Exception ex)//程序执行出错
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsParams, ex.Message);
                LogService.LogError("GetPointParamsByEDCInsKey Error: " + ex.Message);
            }
            return dsParams;
        }
        /// <summary>
        /// 根据抽检点设置主键获取可进行采集的参数集合。
        /// </summary>
        /// <param name="pointKey">抽检点设置主键。</param>
        /// <returns>
        /// 包含批次采样参数点集合的数据集对象。
        /// [ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,
        /// UPPER_SPEC,UPPER_CONTROL,TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,
        /// PARAM_COUNT,PARAM_INDEX,PARAM_KEY,PARAM_FORMULA,Device_Type,DATA_TYPE,ISDERIVED,CALCULATE_TYPE]
        /// </returns>
        public DataSet GetPointParamsByPointKey(string pointKey)
        {
            DataSet dsParams = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT A.ROW_KEY,A.EDC_POINT_ROWKEY,A.EDC_NAME,A.EDC_VERSION,A.PARAM_NAME,
                                                A.UPPER_BOUNDARY,A.UPPER_SPEC,A.UPPER_CONTROL,A.TARGET,A.LOWER_CONTROL,A.LOWER_SPEC,
                                                A.LOWER_BOUNDARY,A.PARAM_COUNT,A.PARAM_INDEX,A.PARAM_KEY,A.PARAM_FORMULA,A.PARAM_TYPE,
                                                A.ALLOW_MIN_VALUE,A.ALLOW_MAX_VALUE,
                                                B.Device_Type,B.DATA_TYPE,B.ISDERIVED,B.CALCULATE_TYPE
                                            FROM EDC_POINT_PARAMS A,BASE_PARAMETER B
                                            WHERE A.PARAM_KEY =B.PARAM_KEY
                                            AND A.PARAM_COUNT>0 AND B.STATUS=1
                                            AND A.EDC_POINT_ROWKEY ='{0}'
                                            ORDER BY B.DEVICE_TYPE DESC,A.PARAM_INDEX", 
                                            pointKey.PreventSQLInjection());
                dsParams = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsParams, string.Empty);
            }
            catch (Exception ex)//程序执行出错
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsParams, ex.Message);
                LogService.LogError("GetPointParamsByPointKey Error: " + ex.Message);
            }
            return dsParams;
        }
        /// <summary>
        /// 获取已激活的成品数据。
        /// </summary>
        /// <returns>
        /// 包含成品数据的数据集。
        /// 【PART_KEY,PART_NAME,PART_TYPE】
        /// </returns>
        public DataSet GetPOR_PART()
        {
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = "SELECT PART_KEY,PART_NAME,PART_TYPE FROM POR_PART WHERE PART_STATUS=1";

                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得已激活的工序数据。
        /// </summary>
        /// <returns>
        /// 包含工序数据的数据集。
        /// 【ROUTE_OPERATION_VER_KEY,ROUTE_OPERATION_NAME】
        /// </returns>
        public DataSet GetPOR_ROUTE_OPERATION_VER()
        {
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = @"SELECT ROUTE_OPERATION_VER_KEY,ROUTE_OPERATION_NAME 
                               FROM POR_ROUTE_OPERATION_VER
                               WHERE OPERATION_STATUS = 1";
                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取抽样规则数据。
        /// </summary>
        /// <returns>
        /// 包含抽样规则的数据集。
        /// [SP_KEY,SP_NAME,SAMPLING_MODE,STRATEGY_SIZE,DESCRIPTIONS]
        /// </returns>
        public DataSet GetEDC_SP()
        {
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = "SELECT SP_KEY,SP_NAME,SAMPLING_MODE,STRATEGY_SIZE,DESCRIPTIONS FROM EDC_SP WHERE STATUS=1";

                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取已激活的管控项目组数据。
        /// </summary>
        /// <returns>
        /// 包含管控项目组数据的数据集。
        /// [EDC_KEY,EDC_NAME,DESCRIPTIONS]
        /// </returns>
        public DataSet GetEDC_MAIN()
        {
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = "SELECT EDC_KEY,EDC_NAME,DESCRIPTIONS FROM EDC_MAIN  WHERE STATUS=1";

                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         /// <summary>
        /// 查询离线数据采集数据。
        /// </summary>
        /// <param name="dtParams">数据采集实例。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含数据采集数据的数据集对象。</returns>
        public DataSet QueryEDCData(DataTable dtParams, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT a.EDC_INS_KEY,a.EDC_KEY,b.EDC_NAME,a.LOT_NUMBER,
                                        a.WORK_ORDER_KEY,a.EDC_SP_KEY,a.COL_START_TIME,a.COL_END_TIME,a.LOT_KEY,a.EDC_POINT_KEY,
                                        a.EQUIPMENT_KEY,e.EQUIPMENT_NAME,e.EQUIPMENT_CODE,
                                        e.EQUIPMENT_NAME+'('+e.EQUIPMENT_CODE+')' EQUIPMENT_NAME_CODE,
                                        a.MATERIAL_LOT,a.STEP_NAME,a.LOCATION_KEY,a.PART_TYPE,
                                        (SELECT USERNAME FROM RBAC_USER WHERE BADGE=a.EDITOR) EDITOR,a.EDIT_TIME,
                                        (SELECT USERNAME FROM RBAC_USER WHERE BADGE=a.CREATOR) CREATOR,a.EDC_COMMENT,
                                        a.PART_NO,a.SUPPLIER,c.ACTION_NAME,d.LOCATION_NAME,f.ORDER_NUMBER
                                FROM EDC_MAIN_INS a
                                LEFT JOIN EDC_MAIN b ON a.EDC_KEY=b.EDC_KEY
                                LEFT JOIN EDC_POINT c ON a.EDC_POINT_KEY=c.ROW_KEY
                                LEFT JOIN FMM_LOCATION d ON a.LOCATION_KEY=d.LOCATION_KEY
                                LEFT JOIN EMS_EQUIPMENTS e ON e.EQUIPMENT_KEY=a.EQUIPMENT_KEY
                                LEFT JOIN POR_WORK_ORDER f ON a.WORK_ORDER_KEY=f.WORK_ORDER_KEY
                                WHERE a.CREATOR<>'system'
                                AND a.DELETED_FLAG=0";

                if (dtParams != null)
                {
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(EDC_MAIN_INS_FIELDS.FIELD_LOCATION_KEY))
                    {
                        string locationKey = Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_LOCATION_KEY]);
                        sql += string.Format(" AND a.LOCATION_KEY='{0}'", locationKey.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME))
                    {
                        string stepName=Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME]);
                        sql += string.Format(" AND a.STEP_NAME='{0}'", stepName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(EDC_MAIN_INS_FIELDS.FIELD_EQUIPMENT_KEY))
                    {
                        string equipmentKey=Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_EQUIPMENT_KEY]);
                        sql += string.Format(" AND a.EQUIPMENT_KEY='{0}'", equipmentKey.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(EDC_MAIN_INS_FIELDS.FIELD_PART_TYPE))
                    {
                        string partType=Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_PART_TYPE]);
                        sql += string.Format(" AND a.PART_TYPE='{0}'", partType.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY))
                    {
                        string edcKey = Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY]);
                        sql += string.Format(" AND a.EDC_KEY='{0}'", edcKey.PreventSQLInjection());
                    }
                    string startTime0 = EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME + "_START";
                    string startTime1 = EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME + "_END";
                    if (htParams.ContainsKey(startTime0))
                    {
                        string startTime=Convert.ToString( htParams[startTime0]);
                        sql += string.Format(" AND a.COL_START_TIME>='{0}'", startTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(startTime1))
                    {
                        string startTime = Convert.ToString(htParams[startTime1]);
                        sql += string.Format(" AND a.COL_START_TIME<='{0}'", startTime.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey(EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER))
                    {
                        string lotNo = Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER]);
                        sql += string.Format(" AND a.LOT_NUMBER LIKE '{0}%'", lotNo.PreventSQLInjection());
                    }
                }
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sql, pconfig.PageNo, pconfig.PageSize, out pages,
                    out records, db, dsReturn, EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME,"COL_START_TIME DESC");
                pconfig.Pages = pages;
                pconfig.Records = records;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
             }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("QueryOfflineEDCData Error: " + ex.Message);
            }
            return dsReturn;
        }

        //------------------------------------------------------------------------------------
        //  参数分组
        //------------------------------------------------------------------------------------
        /// <summary>
        /// 新增参数分组及其参数数据。
        /// </summary>
        /// <param name="dsParams">包含参数分组及其参数数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddEdcAndParam(DataSet dsParams)
        {
            DataSet dsReturn =new DataSet();
            try
            {
                if (null!=dsParams && dsParams.Tables.Contains(EDC_MAIN_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            EDC_MAIN_FIELDS edcFields = new EDC_MAIN_FIELDS();
                            EDC_MAIN_PARAM_FIELDS paramFieds = new EDC_MAIN_PARAM_FIELDS();
                            //新增参数分组
                            htParams.Add(EDC_MAIN_FIELDS.FIELD_EDIT_TIME,null);
                            htParams.Add(EDC_MAIN_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH");
                            string sqlCommand = DatabaseTable.BuildInsertSqlStatement(edcFields, htParams, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //增加参数。
                            if (dsParams.Tables.Contains(EDC_MAIN_PARAM_FIELDS.DATABASE_TABLE_NAME))
                            {
                                DataTable paramTable = dsParams.Tables[EDC_MAIN_PARAM_FIELDS.DATABASE_TABLE_NAME];

                                for (int i = 0; i < paramTable.Rows.Count; i++)
                                {
                                    OperationAction action = (OperationAction)Convert.ToInt32(paramTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                                    switch (action)
                                    {
                                        case OperationAction.New:
                                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(paramFieds, paramTable, i);
                                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            dbTran.Rollback();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            LogService.LogError("AddEdcAndParam Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
                }
            }

            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddEdcAndParam Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 删除参数分组及其参数数据。
        /// </summary>
        /// <param name="paramKey">参数分组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteEdcAndParam(string edcKey)
        {
            DataSet dsReturn =new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(edcKey))
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            string sqlCommand = @"DELETE FROM EDC_MAIN WHERE EDC_KEY = '" + edcKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            sqlCommand = @"DELETE FROM EDC_MAIN_PARAM WHERE EDC_KEY = '" + edcKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            LogService.LogError("DeleteEdcAndParam Error: " + ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                        }
                        finally
                        {
                            dbTran=null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteEdcAndParam Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询参数分组。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -----------------------------------------
        /// {EDC_NAME}
        /// -----------------------------------------
        /// </param>
        /// <returns>包含参数分组的数据集对象。</returns>
        public DataSet SearchEdcMain(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Empty;
                if (dsParams != null && dsParams.Tables.Contains(EDC_MAIN_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string edcName = Convert.ToString(htParams[EDC_MAIN_FIELDS.FIELD_EDC_NAME]);

                    sqlCommand =string.Format(@"SELECT * FROM EDC_MAIN
                                              WHERE EDC_NAME LIKE '%{0}%'  AND STATUS<> 2
                                              ORDER BY EDC_NAME", 
                                             edcName.PreventSQLInjection()) ;
                }
                else
                {
                    sqlCommand = @"SELECT * FROM EDC_MAIN WHERE STATUS<> 2 ORDER BY EDC_NAME";
                }

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchEdcMain Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不重复的参数分组名称。
        /// </summary>
        /// <returns>包含参数分组名称的数据集对象。</returns>
        public DataSet GetDistinctEdcName()
        {
            DataSet dsReturn =new DataSet();
            try
            {
                string sqlCommand  = @"SELECT DISTINCT EDC_NAME
                                      FROM EDC_MAIN
                                      WHERE STATUS <> 2 
                                      ORDER BY EDC_NAME";
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctEdcName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据参数分组主键获取参数分组信息。
        /// </summary>
        /// <param name="edcKey">参数分组主键。</param>
        /// <returns>包含参数分组信息的数据集对象。</returns>
        public DataSet GetEdcByKey(string edcKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (edcKey != null && edcKey.Length > 0)
                {
                    //获取参数分组数据
                    string sqlCommand =string.Format(@"SELECT * FROM EDC_MAIN
                                                       WHERE EDC_KEY = '{0}'",
                                                       edcKey.PreventSQLInjection());

                    DataTable dtTable1 = new DataTable();
                    dtTable1 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable1.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable1, false, MissingSchemaAction.Add);
                    //获取参数分组对应的参数数据
                    sqlCommand =string.Format(@"SELECT A.PARAM_KEY, A.PARAM_NAME
                                                FROM BASE_PARAMETER A, EDC_MAIN_PARAM B
                                                WHERE A.PARAM_KEY = B.PARAM_KEY
                                                AND B.EDC_KEY = '{0}'",
                                                edcKey.PreventSQLInjection());

                    DataTable dtTable2 = new DataTable();
                    dtTable2 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable2.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable2, false, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEdcByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 参数分组名称和参数分组主键相互转换。
        /// </summary>
        /// <param name="inputParam">查询条件（EDC_NAME或EDC_KEY）。</param>
        /// <param name="symBol">I：查询条件为EDC_NAME。 O(字母)：查询条件为EDC_KEY。</param>
        /// <returns>
        /// I：返回EDC_KEY。 O(字母)：返回EDC_NAME。
        /// </returns>
        public string ConvertEdcKeyOrName(string inputParam, string symBol)
        {
            try
            {
                string sqlCommand = string.Empty;
                switch (symBol)
                {
                    case "I":
                        sqlCommand = @"SELECT EDC_KEY FROM EDC_MAIN WHERE EDC_NAME = '" + inputParam + "'";
                        break;
                    case "O":
                        sqlCommand = @"SELECT EDC_NAME FROM EDC_MAIN WHERE EDC_KEY = '" + inputParam + "'";
                        break;
                    default:
                        break;
                }
                string result = Convert.ToString(db.ExecuteScalar(CommandType.Text, sqlCommand));
                return result;
            }
            catch (Exception ex)
            {
                LogService.LogError("ConvertEdcKeyOrName Error: " + ex.Message);
            }
            return string.Empty;
        }
        /// <summary>
        /// 更新参数分组及其对应的参数数据。
        /// </summary>
        /// <param name="dsParams">包含参数分组及其对应的参数的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateEdcAndParam(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(EDC_MAIN_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            EDC_MAIN_FIELDS edcFields = new EDC_MAIN_FIELDS();
                            EDC_MAIN_PARAM_FIELDS paramFieds = new EDC_MAIN_PARAM_FIELDS();
                            //更新参数分组数据
                            WhereConditions wc = new WhereConditions(EDC_MAIN_FIELDS.FIELD_EDC_KEY,
                                                                     Convert.ToString(htParams[EDC_MAIN_FIELDS.FIELD_EDC_KEY]));
                            htParams.Remove(EDC_MAIN_FIELDS.FIELD_EDC_KEY);
                            htParams.Add(EDC_MAIN_FIELDS.FIELD_EDIT_TIME, null);
                            htParams.Add(EDC_MAIN_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH");
                            string sqlCommand = DatabaseTable.BuildUpdateSqlStatement(edcFields, htParams, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //更新参数分组对应的参数数据
                            if (dsParams.Tables.Contains(EDC_MAIN_PARAM_FIELDS.DATABASE_TABLE_NAME))
                            {
                                DataTable paramTable = dsParams.Tables[EDC_MAIN_PARAM_FIELDS.DATABASE_TABLE_NAME];

                                for (int i = 0; i < paramTable.Rows.Count; i++)
                                {
                                    WhereConditions delCondition = new WhereConditions(EDC_MAIN_PARAM_FIELDS.FIELD_PARAM_KEY,
                                        paramTable.Rows[i][EDC_MAIN_PARAM_FIELDS.FIELD_PARAM_KEY].ToString());
                                    OperationAction action=(OperationAction)Convert.ToInt32(paramTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                                    switch (action)
                                    {
                                        case OperationAction.New:
                                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(paramFieds, paramTable, i);
                                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                            break;
                                        case OperationAction.Delete:
                                            sqlCommand = DatabaseTable.BuildDeleteSqlStatement(paramFieds, delCondition);
                                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("UpdateEdcAndParam Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0007}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateEdcAndParam Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取最大版本号的参数分组信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ----------------------------------
        /// {EDC_NAME}
        /// ----------------------------------
        /// </param>
        /// <returns>
        /// 包含参数分组信息的数据集对象。
        /// </returns>
        public DataSet GetEDCScrapCategory(DataSet dsParams)
        {
            //define return dataset
            DataSet dsReturn = new DataSet();
            try
            {
                string edcName=Convert.ToString(dsParams.Tables[0].Rows[0][1]);
                string sql =string.Format(@"SELECT A.EDC_KEY AS EDC_KEY, B.EDC_NAME AS EDC_NAME , B.EDC_VERSION AS EDC_VERSION ,A.DESCRIPTIONS AS DESCRIPTIONS
                                        FROM EDC_MAIN A 
                                        RIGHT JOIN (SELECT EDC_NAME, MAX(EDC_VERSION) AS EDC_VERSION 
                                                    FROM EDC_MAIN 
                                                    GROUP BY EDC_NAME ) B ON A.EDC_VERSION = B.EDC_VERSION
                                        AND A.EDC_NAME =B.EDC_NAME
                                        WHERE B.EDC_NAME LIKE '%{0}%' 
                                        ORDER BY EDC_NAME", 
                                        edcName.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = "EDC_MAIN";
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEDCScrapCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的抽检规则信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ------------------------------------
        /// {SP_NAME}
        /// ------------------------------------
        /// </param>
        /// <returns>
        /// 包含抽检规则信息的数据集对象。
        /// </returns>
        public DataSet GetSPScrapCategory(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string spName = Convert.ToString(dsParams.Tables[0].Rows[0][1]);
                string sql = string.Format(@"SELECT A.SP_KEY AS SP_KEY, B.SP_NAME AS SP_NAME , B.SP_VERSION AS SP_VERSION ,A.DESCRIPTIONS AS DESCRIPTIONS
                                             FROM EDC_SP A RIGHT JOIN (SELECT SP_NAME, MAX(SP_VERSION) AS SP_VERSION 
                                                                       FROM EDC_SP 
                                                                       ROUP BY SP_NAME ) B ON A.SP_VERSION = B.SP_VERSION
                                             AND A.SP_NAME =B.SP_NAME
                                             WHERE B.SP_NAME LIKE '%{0}%'
                                             ORDER BY SP_NAME", 
                                             spName.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = "EDC_SP";
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSPScrapCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据参数分组主键获取参数数据。
        /// </summary>
        /// <param name="key">参数分组主键。</param>
        /// <returns>包含参数数据的数据集对象。</returns>
        public DataSet GetBASE_PARAMETERByEDC_KEY(string key)
        {
            try
            {
                string sqlCommand =string.Format(@"SELECT * FROM BASE_PARAMETER T
                                                   WHERE T.PARAM_KEY IN (SELECT EM.PARAM_KEY 
                                                                         FROM EDC_MAIN_PARAM EM 
                                                                         WHERE EM.EDC_KEY = '{0}')",
                                                key.PreventSQLInjection());
                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据抽检点主键获取抽检点对应的参数数据。
        /// </summary>
        /// <param name="ds">包含查询条件的数据集对象。
        /// --------------------------------------
        /// {FIELD_TOPRODUCT}
        /// {FIELD_EDC_KEY}
        /// {FIELD_SP_KEY}
        /// {FIELD_OPERATION_NAME}
        /// --------------------------------------
        /// </param>
        /// <param name="ifHas">输出参数，抽检点是否存在对应的参数数据。</param>
        /// <param name="EdcPointKey">输出参数，抽检点主键。</param>
        /// <returns>包含抽检点对应的参数数据的数据集对象。</returns>
        public DataSet GetPARAMETERByEDCPOINT(DataSet ds, out bool ifHas, out string EdcPointKey)
        {
            try
            {
                EdcPointKey = string.Empty;
                Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(ds.Tables[0]);

                string toProduct = Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_TOPRODUCT]);
                string edcKey=Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_EDC_KEY]);
                string spKey=Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_SP_KEY]);
                string operationName=Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_OPERATION_NAME]);
                string sqlCommand = string.Format(@"SELECT T.ROW_KEY 
                                                    FROM EDC_POINT T 
                                                    WHERE T.TOPRODUCT = '{0}' AND T.EDC_KEY = '{1}' 
                                                    AND T.SP_KEY = '{2}' AND T.OPERATION_NAME = '{3}'",
                                                    toProduct.PreventSQLInjection(),
                                                    edcKey.PreventSQLInjection(),
                                                    spKey.PreventSQLInjection(),
                                                    operationName.PreventSQLInjection());
                object rowk = db.ExecuteScalar(CommandType.Text, sqlCommand);

                if (rowk != null)
                {
                    ifHas = true;
                    EdcPointKey = Convert.ToString(rowk);
                    sqlCommand =string.Format(@"SELECT * 
                                                FROM EDC_POINT_PARAMS T 
                                                WHERE T.EDC_POINT_ROWKEY='{0}'",
                                                EdcPointKey.PreventSQLInjection());
                }
                else
                {
                    ifHas = false;
                    sqlCommand =string.Format(@"SELECT T.*,'' AS PARAM_INDEX,'' AS PARAM_TYPE 
                                                FROM BASE_PARAMETER T 
                                                WHERE T.PARAM_KEY IN (SELECT EM.PARAM_KEY 
                                                                     FROM EDC_MAIN_PARAM EM 
                                                                     WHERE EM.EDC_KEY = '{0}')",
                                                edcKey.PreventSQLInjection());

                }
                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 新增抽检点数据及其对应的参数数据。
        /// </summary>
        /// <param name="EdcPoint">包含抽检点数据的数据集对象。</param>
        /// <param name="EdcPointP">包含抽检点对应参数数据的数据集对象。</param>
        /// <param name="ishasCreate">
        /// true:之前已创建抽检点数据，需要删除抽检点对应参数数据。
        /// false:之前未创建抽检点数据。
        /// </param>
        /// <returns>true：新增成功。false：新增失败。</returns>
        public bool InsertEDCPointAll(DataSet EdcPoint, DataSet EdcPointP, bool ishasCreate)
        {
            try
            {
                bool result = false;
                DbTransaction dbTran = null;
                using (DbConnection dbConn = db.CreateConnection())
                {
                    try
                    {
                        dbConn.Open();
                        dbTran = dbConn.BeginTransaction();
                        Hashtable htEdcPoint =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(EdcPoint.Tables[0]);
                        string edcPointKey = Convert.ToString(htEdcPoint[EDC_POINT_FIELDS.FIELD_ROW_KEY]);
                        //未创建过抽检点数据。
                        if (!ishasCreate)
                        {
                            //新增抽检点数据
                            edcPointKey = UtilHelper.GenerateNewKey(0);
                            htEdcPoint.Remove(EDC_POINT_FIELDS.FIELD_ROW_KEY);
                            htEdcPoint.Add(EDC_POINT_FIELDS.FIELD_ROW_KEY, edcPointKey);

                            DataSet dsEDCPoint = new DataSet();
                            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htEdcPoint);
                            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                            dsEDCPoint.Tables.Add(mainDataTable);
                            CreateEdcPoint(db, dbTran, dsEDCPoint);
                        }
                        else
                        {
                            //删除抽检点数据对应的参数数据
                            DeleteEdcPointParamByEPKey(db, dbTran, edcPointKey);
                        }
                        //新增抽检点数据对应的参数数据。
                        for (int i = 0; i < EdcPointP.Tables[0].Rows.Count; i++)
                        {
                            Hashtable mainDataHashTable = new Hashtable();
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_EDC_POINT_ROWKEY, edcPointKey);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_TARGET, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_TARGET]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_ROW_KEY, UtilHelper.GenerateNewKey(0));
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL]);

                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX, EdcPointP.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX]);
                            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
                            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                            DataSet dsEdcPointP = new DataSet();
                            dsEdcPointP.Tables.Add(mainDataTable);
                            CreateEdcPointParam(db, dbTran, dsEdcPointP);
                        }
                        dbTran.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        LogService.LogError("InsertEDCPointAll Error: " + ex.Message);
                        dbTran.Rollback();
                        result = false;
                    }
                    finally
                    {
                        dbTran = null;
                        dbConn.Close();
                    }
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 新增抽检点数据。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dbtran">数据库事务操作对象。</param>
        /// <param name="dsParams">包含抽检点数据的数据集对象。</param>
        private void CreateEdcPoint(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            DataTable dtParams = dsParams.Tables[0];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            //initialize tablefields
            EDC_POINT_FIELDS porLotFields = new EDC_POINT_FIELDS();
            //get sql
            string sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, htParams, null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);          
        }
        /// <summary>
        /// 新增抽检点对应的参数数据。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dbtran">数据库事务操作对象。</param>
        /// <param name="dsParams">包含抽检点对应参数数据的数据集对象。</param>
        private void CreateEdcPointParam(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            DataTable dtParams = dsParams.Tables[0];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

            EDC_POINT_PARAMS_FIELDS porLotFields = new EDC_POINT_PARAMS_FIELDS();
            string sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, htParams, null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
        }
        /// <summary>
        /// 根据抽检点主键删除其对应的参数数据。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dbtran">数据库事务操作对象。</param>
        /// <param name="EPKey">抽检点主键。</param>
        private void DeleteEdcPointParamByEPKey(Database db, DbTransaction dbtran, string EPKey)
        {
            string sql = "DELETE FROM EDC_POINT_PARAMS T WHERE T.EDC_POINT_ROWKEY='" + EPKey.PreventSQLInjection() + "'";
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
        }
        /// <summary>
        /// 根据批次号查询批次采集的数据。
        /// </summary>
        /// <param name="lotNumber">生产批次号。</param>
        /// <returns>
        /// 包含批次数据采集信息的数据集对象。
        /// </returns>
        public DataSet GetLotParamsCollection(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT T.SP_UNIT_SEQ, T.PARAM_VALUE, T.VALID_FLAG,B.PARAM_NAME,E.STEP_KEY,E.STEP_NAME
                                            FROM EDC_COLLECTION_DATA T,EDC_MAIN_INS E,POR_LOT P,BASE_PARAMETER B
                                            WHERE T.EDC_INS_KEY=E.EDC_INS_KEY
                                            AND E.LOT_KEY=P.LOT_KEY
                                            AND T.PARAM_KEY=B.PARAM_KEY
                                            AND P.LOT_NUMBER='{0}'
                                            AND T.DELETED_FLAG=0
                                            ORDER BY E.STEP_KEY,T.EDIT_TIME", lotNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotParamsCollection Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据方块电阻设备的数据采集主键删除采集的方块电阻数据。
        /// </summary>
        /// <param name="edcInskey">方块电阻设备的数据采集主键</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteRData(string edcInskey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"UPDATE EDC_COLLECTION_DATA
                                             SET DELETED_FLAG=1
                                             WHERE COL_KEY IN (SELECT T.COL_KEY
                                                               FROM EDC_COLLECTION_DATA T, BASE_PARAMETER B
                                                               WHERE T.PARAM_KEY = B.PARAM_KEY
                                                               AND T.EDC_INS_KEY = '{0}'
                                                               AND B.DEVICE_TYPE = 'R' AND T.DELETED_FLAG=0)
                                             AND DELETED_FLAG=0", 
                                             edcInskey.PreventSQLInjection());
                db.ExecuteNonQuery(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("Delete R data error:" + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增数据采集实例及其采集的明细数据。
        /// </summary>
        /// <param name="dsParams">包含数据采集实例及其采集的明细数据的数据集对象。</param>
        /// <returns>true：新增成功，false:新增失败。</returns>
        public bool InsertEdcMainAndDetails(DataSet dsParams)
        {
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTrans = dbConn.BeginTransaction();
                try
                {
                    if (dsParams.Tables.Contains(EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME))
                    {
                        EDCManagement.InsertEdcMainInsData(db, dbTrans, dsParams);
                    }

                    if (dsParams.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
                    {
                        EDCManagement.SaveEDCCollectionData(db, dbTrans, dsParams);
                    }

                    dbTrans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw ex;
                }
                finally
                {
                    dbTrans = null;
                    dbConn.Close();
                }
            }
        }
        /// <summary>
        /// 根据批次号获取批次采集的数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次采集数据的数据集对象。</returns>
        public DataSet GetEdcExistData(string lotNumber)
        {
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = string.Format(@"SELECT A.LOT_NUMBER, B.*
                                            FROM EDC_MAIN_INS A, EDC_COLLECTION_DATA B
                                            WHERE A.EDC_INS_KEY = B.EDC_INS_KEY
                                            AND A.LOT_NUMBER = '{0}'
                                            ORDER BY B.SP_SAMP_SEQ", 
                                            lotNumber.PreventSQLInjection());
                return db.ExecuteDataSet(CommandType.Text, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 比较两个抽检点相同采集参数的采集数量是否相同。
        /// </summary>
        /// <param name="rowKey">待比对的抽检点主键。</param>
        /// <param name="preRowKey">作为标准比对的抽检点主键。</param>
        /// <returns>false：不同。true：相同。</returns>
        public bool CheckEDCParaMatch(string rowKey, string preRowKey)
        {
            bool bReturn = true;
            try
            {
                //根据抽检点主键获取采集参数。
                string sql = string.Format(@"SELECT * FROM EDC_POINT_PARAMS P WHERE P.EDC_POINT_ROWKEY = '{0}'", 
                                             preRowKey.PreventSQLInjection());
                DataSet dsPre = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsPre != null && dsPre.Tables.Count > 0 && dsPre.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsPre.Tables[0].Rows.Count; i++)
                    {
                        string paramKey = Convert.ToString(dsPre.Tables[0].Rows[i]["PARAM_KEY"]);
                        string preParamCount =  Convert.ToString(dsPre.Tables[0].Rows[i]["PARAM_COUNT"]);
                        //根据抽检点主键和参数主键获取采集参数。
                        sql = string.Format(@"SELECT * FROM EDC_POINT_PARAMS P WHERE P.EDC_POINT_ROWKEY = '{0}' AND P.PARAM_KEY = '{1}'",
                                            rowKey.PreventSQLInjection(),
                                            paramKey.PreventSQLInjection());
                        DataSet dsMatch = db.ExecuteDataSet(CommandType.Text, sql);
                        if (dsMatch != null && dsMatch.Tables.Count > 0 && dsMatch.Tables[0].Rows.Count > 0)
                        {
                            string paramCount = Convert.ToString(dsMatch.Tables[0].Rows[0]["PARAM_COUNT"]);
                            if (paramCount != preParamCount)//参数采集数量不同。
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("CheckEDCParaMatch Error: " + ex.Message);
            }
            return bReturn;
        }


        /// <summary>
        /// 保存数据采集实例数据。
        /// </summary>
        /// <param name="dbTrans">事务操作对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcPointKey">抽检点设置主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="oprLine">操作线别名称。</param>
        /// <param name="shiftKey">班别主键。</param>
        public void SaveEdcMainInfo(DbTransaction dbTrans, string lotKey, string edcPointKey, string editor,
                             string equipmentKey, string oprLine, string shiftKey)
        {
            EDCManagement.SaveEdcMainInfo(db, dbTrans, lotKey, edcPointKey, editor, equipmentKey, oprLine, shiftKey);
        }
    }
}
