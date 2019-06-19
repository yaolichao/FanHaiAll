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
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.DatabaseHelper;

namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 用于进行批次预设暂停的类，实现了批次预设暂停接口（<see cref="ILotFutureHoldEngine"/>）。
    /// </summary>
    public class LotFutureHoldEngine : AbstractEngine, ILotFutureHoldEngine
    {
        private Database db =null; //数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotFutureHoldEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化函数。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 根据主键获取预设暂停数据。
        /// </summary>
        /// <param name="key">预设暂停主键。</param>
        /// <returns>包含预设暂停数据的数据集对象。</returns>
        public DataSet Get(string key)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                
                string sql = string.Format(@"SELECT a.*,b.ENTERPRISE_NAME,c.ROUTE_NAME
                                            FROM WIP_FUTUREHOLD a
                                            LEFT JOIN POR_ROUTE_ENTERPRISE_VER b ON a.ENTERPRISE_KEY=b.ROUTE_ENTERPRISE_VER_KEY
                                            LEFT JOIN POR_ROUTE_ROUTE_VER c ON a.ROUTE_KEY=c.ROUTE_ROUTE_VER_KEY
                                            WHERE a.ROW_KEY=@key");
                DbCommand dbCmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCmd, "key", DbType.String, key);
                dsReturn = db.ExecuteDataSet(dbCmd);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError(string.Format("LotFutureHoldEngine-Get Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 批量新增预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含待预设暂停批次的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet Insert(DataSet dsParams)
        {
            DataSet dsReturn=new DataSet();
            DbTransaction dbTrans = null;
            DbConnection dbCon = null;
            if (dsParams != null)
            {
                try
                {
                    DataTable dtLots = dsParams.Tables[WIP_FUTUREHOLD_FIELDS.DATABASE_TABLE_NAME];
                    if (dtLots == null || dtLots.Rows.Count == 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "预设暂停的批次数据不能为空。");
                        return dsReturn;
                    }

                    string sql = @"INSERT INTO WIP_FUTUREHOLD(ROW_KEY,WORKORDER_NUMBER,LOT_NUMBER,HOLD_LEVEL,ACTION_NAME,STATUS,
                                       REMARK,STEP_KEY,ROUTE_KEY,ENTERPRISE_KEY,OPERATION_NAME,LOT_KEY,HOLD_PASSWORD,
                                       EDITOR,EDIT_TIME,SET_STEP_KEY,SET_ROUTE_KEY,SET_ENTERPRISE_KEY,
                                       REASON_CODE_CATEGORY_KEY,REASON_CODE_CATEGORY_NAME,REASON_CODE_KEY,REASON_CODE,CREATOR,CREATE_TIME,DELETE_FLAG)
                                VALUES(@key,@orderNo,@lotNo,1,@action,1,
                                       @remark,@stepKey,@routeKey,@enterpriseKey,@operationName,@lotKey,@password,
                                       @editor,GETDATE(),@setStepKey,@setRouteKey,@setEnterpriseKey,
                                       @rcCategoryKey,@rcCategoryName,@rcCodeKey,@rcCodeName,@creator,GETDATE(),0)";

                    dbCon = db.CreateConnection();
                    dbCon.Open();
                    dbTrans = dbCon.BeginTransaction();

                    foreach (DataRow dr in dtLots.Rows)
                    {
                        string key = UtilHelper.GenerateNewKey(0);
                        string orderNo = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER]);
                        string lotNo = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER]);
                        string action = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME]);
                        string remark = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK]);
                        string stepKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_STEP_KEY]);
                        string routeKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ROUTE_KEY]);
                        string enterpriseKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ENTERPRISE_KEY]);
                        string operationName = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME]);
                        string lotKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_KEY]);
                        string editor = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_EDITOR]);
                        string setStepKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_STEP_KEY]);
                        string setRouteKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ROUTE_KEY]);
                        string setEnterpriseKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ENTERPRISE_KEY]);
                        string rcCategoryKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY]);
                        string rcCategoryName = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_NAME]);
                        string rcCodeKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY]);
                        string rcCodeName = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE]);
                        string password = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD]);
                        DbCommand dbCmd = db.GetSqlStringCommand(sql);
                        db.AddInParameter(dbCmd, "key", DbType.String, key);
                        db.AddInParameter(dbCmd, "orderNo", DbType.String, orderNo);
                        db.AddInParameter(dbCmd, "lotNo", DbType.String, lotNo);
                        db.AddInParameter(dbCmd, "action", DbType.String, action);
                        db.AddInParameter(dbCmd, "remark", DbType.String, remark);
                        db.AddInParameter(dbCmd, "stepKey", DbType.String, stepKey);
                        db.AddInParameter(dbCmd, "routeKey", DbType.String, routeKey);
                        db.AddInParameter(dbCmd, "enterpriseKey", DbType.String, enterpriseKey);
                        db.AddInParameter(dbCmd, "operationName", DbType.String, operationName);
                        db.AddInParameter(dbCmd, "lotKey", DbType.String, lotKey);
                        db.AddInParameter(dbCmd, "password", DbType.String, password);
                        db.AddInParameter(dbCmd, "editor", DbType.String, editor);
                        db.AddInParameter(dbCmd, "setStepKey", DbType.String, setStepKey);
                        db.AddInParameter(dbCmd, "setRouteKey", DbType.String, setRouteKey);
                        db.AddInParameter(dbCmd, "setEnterpriseKey", DbType.String, setEnterpriseKey);
                        db.AddInParameter(dbCmd, "rcCategoryKey", DbType.String, rcCategoryKey);
                        db.AddInParameter(dbCmd, "rcCategoryName", DbType.String, rcCategoryName);
                        db.AddInParameter(dbCmd, "rcCodeKey", DbType.String, rcCodeKey);
                        db.AddInParameter(dbCmd, "rcCodeName", DbType.String, rcCodeName);
                        db.AddInParameter(dbCmd, "creator", DbType.String, editor);
                        int nRet = db.ExecuteNonQuery(dbCmd, dbTrans);
                    }
                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,string.Empty);
                }
                catch (Exception ex)
                {
                    if (dbTrans != null)
                    {
                        dbTrans.Rollback();
                    }
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError(string.Format("LotFutureHoldEngine-Insert Error:{0}", ex.Message));
                }
                finally
                {
                    if (dbCon != null)
                    {
                        if (dbCon.State != ConnectionState.Closed)
                        {
                            dbCon.Close();
                        }
                        dbCon = null;
                    }
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含待更新的预设暂停批次的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet Update(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTrans = null;
            DbConnection dbCon = null;
            if (dsParams != null)
            {
                try
                {
                    DataTable dtLots = dsParams.Tables[WIP_FUTUREHOLD_FIELDS.DATABASE_TABLE_NAME];
                    if (dtLots == null || dtLots.Rows.Count == 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "预设暂停的批次数据不能为空。");
                        return dsReturn;
                    }

                    string sql = @"UPDATE WIP_FUTUREHOLD
                                   SET WORKORDER_NUMBER=@orderNo,
                                       LOT_NUMBER=@lotNo,
                                       ACTION_NAME=@action,
                                       REMARK=@remark,
                                       STEP_KEY=@stepKey,
                                       ROUTE_KEY=@routeKey,
                                       ENTERPRISE_KEY=@enterpriseKey,
                                       OPERATION_NAME=@operationName,
                                       LOT_KEY=@lotKey,
                                       HOLD_PASSWORD=@password,
                                       EDITOR=@editor,EDIT_TIME=GETDATE(),
                                       SET_STEP_KEY=@setStepKey,
                                       SET_ROUTE_KEY=@setRouteKey,
                                       SET_ENTERPRISE_KEY=@setEnterpriseKey,
                                       REASON_CODE_CATEGORY_KEY=@rcCategoryKey,
                                       REASON_CODE_CATEGORY_NAME=@rcCategoryName,
                                       REASON_CODE_KEY=@rcCodeKey,
                                       REASON_CODE=@rcCodeName
                                   WHERE ROW_KEY=@key";

                    dbCon = db.CreateConnection();
                    dbCon.Open();
                    dbTrans = dbCon.BeginTransaction();

                    foreach (DataRow dr in dtLots.Rows)
                    {
                        string key = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ROW_KEY]);
                        string lotNo = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER]);
                        string editTime = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_EDIT_TIME]);

                        #region 检查记录是否过期
                        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WIP_FUTUREHOLD_FIELDS.FIELDS_ROW_KEY, key);
                        List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                        listCondition.Add(kvp);
                        //判断数据是否过期，如果过期，则返回执行结果为“数据已过期。”，结束方法执行。
                        if (UtilHelper.CheckRecordExpired(db, WIP_FUTUREHOLD_FIELDS.DATABASE_TABLE_NAME, listCondition, editTime))
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("『{0}』数据已过期。",lotNo));
                            return dsReturn;
                        }
                        #endregion

                        string orderNo = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER]);
                        
                        string action = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME]);
                        string remark = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK]);
                        string stepKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_STEP_KEY]);
                        string routeKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ROUTE_KEY]);
                        string enterpriseKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_ENTERPRISE_KEY]);
                        string operationName = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME]);
                        string lotKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_KEY]);
                        string editor = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_EDITOR]);
                        string setStepKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_STEP_KEY]);
                        string setRouteKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ROUTE_KEY]);
                        string setEnterpriseKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ENTERPRISE_KEY]);
                        string rcCategoryKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY]);
                        string rcCategoryName = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_NAME]);
                        string rcCodeKey = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY]);
                        string rcCodeName = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE]);
                        string password = Convert.ToString(dr[WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD]);
                        DbCommand dbCmd = db.GetSqlStringCommand(sql);
                        db.AddInParameter(dbCmd, "key", DbType.String, key);
                        db.AddInParameter(dbCmd, "orderNo", DbType.String, orderNo);
                        db.AddInParameter(dbCmd, "lotNo", DbType.String, lotNo);
                        db.AddInParameter(dbCmd, "action", DbType.String, action);
                        db.AddInParameter(dbCmd, "remark", DbType.String, remark);
                        db.AddInParameter(dbCmd, "stepKey", DbType.String, stepKey);
                        db.AddInParameter(dbCmd, "routeKey", DbType.String, routeKey);
                        db.AddInParameter(dbCmd, "enterpriseKey", DbType.String, enterpriseKey);
                        db.AddInParameter(dbCmd, "operationName", DbType.String, operationName);
                        db.AddInParameter(dbCmd, "lotKey", DbType.String, lotKey);
                        db.AddInParameter(dbCmd, "password", DbType.String, password);
                        db.AddInParameter(dbCmd, "editor", DbType.String, editor);
                        db.AddInParameter(dbCmd, "setStepKey", DbType.String, setStepKey);
                        db.AddInParameter(dbCmd, "setRouteKey", DbType.String, setRouteKey);
                        db.AddInParameter(dbCmd, "setEnterpriseKey", DbType.String, setEnterpriseKey);
                        db.AddInParameter(dbCmd, "rcCategoryKey", DbType.String, rcCategoryKey);
                        db.AddInParameter(dbCmd, "rcCategoryName", DbType.String, rcCategoryName);
                        db.AddInParameter(dbCmd, "rcCodeKey", DbType.String, rcCodeKey);
                        db.AddInParameter(dbCmd, "rcCodeName", DbType.String, rcCodeName);
                        int nRet = db.ExecuteNonQuery(dbCmd, dbTrans);
                    }
                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    if (dbTrans != null)
                    {
                        dbTrans.Rollback();
                    }
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError(string.Format("LotFutureHoldEngine-Update Error:{0}", ex.Message));
                }
                finally
                {
                    if (dbCon != null)
                    {
                        if (dbCon.State != ConnectionState.Closed)
                        {
                            dbCon.Close();
                        }
                        dbCon = null;
                    }
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据主键删除预设暂停数据。
        /// </summary>
        /// <param name="key">预设暂停主键。</param>
        /// <param name="deletor">删除人。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet Delete(string key,string deletor)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                string sql = string.Format(@"UPDATE WIP_FUTUREHOLD 
                                             SET STATUS=0,DELETE_FLAG=1,EDITOR=@editor,EDIT_TIME=GETDATE() 
                                             WHERE ROW_KEY=@key");
                DbCommand dbCmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCmd, "key", DbType.String, key);
                db.AddInParameter(dbCmd, "editor", DbType.String, deletor);
                int nRet=db.ExecuteNonQuery(dbCmd);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError(string.Format("LotFutureHoldEngine-Delete Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 分页查询预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="config">数据分页配置对象。</param>
        /// <returns>包含预设暂停数据的数据集对象。</returns>
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                string sql = string.Format(@"SELECT a.*,b.PRO_ID,c.ROUTE_NAME,d.ROUTE_STEP_NAME,b.QUANTITY
                                            FROM WIP_FUTUREHOLD a
                                            LEFT JOIN POR_LOT b ON a.LOT_KEY=b.LOT_KEY
                                            LEFT JOIN POR_ROUTE_ROUTE_VER c ON b.CUR_ROUTE_VER_KEY=c.ROUTE_ROUTE_VER_KEY
                                            LEFT JOIN POR_ROUTE_STEP d ON b.CUR_STEP_VER_KEY=d.ROUTE_STEP_KEY
                                            WHERE a.STATUS=1 ");
                string lotNo = string.Empty;
                string orderNo = string.Empty;
                string operationName = string.Empty;
                string action = string.Empty;
                string roomKey = string.Empty;
                //组织SQL字符串。
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.Contains(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                    {
                        roomKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                        sql += string.Format(" AND b.FACTORYROOM_KEY = '{0}'",roomKey.PreventSQLInjection());
                    }
                    if (htParams.Contains(WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER))
                    {
                        lotNo = Convert.ToString(htParams[WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER]);
                        sql += string.Format(" AND a.LOT_NUMBER LIKE '{0}%'", lotNo.PreventSQLInjection());
                    }
                    if (htParams.Contains(WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER))
                    {
                        orderNo = Convert.ToString(htParams[WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER]);
                        sql += string.Format(" AND a.WORKORDER_NUMBER LIKE '{0}%'", orderNo.PreventSQLInjection());
                    }
                    if (htParams.Contains(WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME))
                    {
                        operationName = Convert.ToString(htParams[WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME]);
                        sql += string.Format(" AND a.OPERATION_NAME='{0}'", operationName.PreventSQLInjection());
                    }
                    if (htParams.Contains(WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME))
                    {
                        action = Convert.ToString(htParams[WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME]);
                        sql += string.Format(" AND a.ACTION_NAME='{0}'", action.PreventSQLInjection());
                    }
                }
                //进行分页查询。
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sql, config.PageNo, config.PageSize, out pages,
                    out records, db, dsReturn, WIP_FUTUREHOLD_FIELDS.DATABASE_TABLE_NAME);
                config.Pages = pages;
                config.Records = records;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError(string.Format("LotFutureHoldEngine-Query Error:{0}", ex.Message));
            }
            return dsReturn;
        }
    }
}
