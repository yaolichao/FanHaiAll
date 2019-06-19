using System;
using System.Collections;
using System.Collections.Generic;
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

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 线上仓的数据管理类。
    /// </summary>
    public class StoreEngine:AbstractEngine,IStoreEngine
    {
         private Database db = null;//数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public StoreEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 添加线上仓。
        /// </summary>
        /// <param name="dsParams">包含线上仓数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddStore(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sqlCommand = "";
            if (null != dsParams && dsParams.Tables.Contains(WST_STORE_FIELDS.DATABASE_TABLE_NAME))
            {
                //判断线上仓名称是否存在。
                DataTable dtParams = dsParams.Tables[WST_STORE_FIELDS.DATABASE_TABLE_NAME];
                WST_STORE_FIELDS storeFields = new WST_STORE_FIELDS();
                string storeName=Convert.ToString(dtParams.Rows[0][WST_STORE_FIELDS.FIELD_STORE_NAME]);
                sqlCommand = "SELECT COUNT(*) FROM WST_STORE WHERE STORE_NAME ='" + storeName.PreventSQLInjection() + "'";
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                if (count>0)
                {
                    //返回消息“仓库名称不能重复，请重新输入”
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.FMM.StoreEngine.StoreNameAlreadyExist}");
                    return dsReturn;
                }

                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                try
                {
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        //生成INSERT SQL
                        sqlCommand = DatabaseTable.BuildInsertSqlStatement(storeFields,
                                                    dtParams, i, 
                                                    new Dictionary<string, string>() 
                                                    {                                                             
                                                        {WST_STORE_FIELDS.FIELD_CREATE_TIME,null},                                                            
                                                    },
                                                    new List<string>());

                        db.ExecuteNonQuery(dbtran, CommandType.Text, sqlCommand);
                    }
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("AddStore Error: " + ex.Message);
                }
                finally
                {
                    dbtran = null;
                    dbconn.Close();
                    dbconn = null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.CalShiftEngine.TableIsNotExist}");
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓主键获取线边仓信息。
        /// </summary>
        /// <param name="storeKey">线上仓主键。</param>
        /// <returns>包含线上仓信息的数据集对象。</returns>
        public DataSet GetStore(string storeKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";             
            try
            {
                sql = @"SELECT A.*,A.OBJECT_STATUS,B.LOCATION_NAME
                        FROM WST_STORE A
                        LEFT JOIN FMM_LOCATION B ON A.LOCATION_KEY=B.LOCATION_KEY ";  
                if (!string.IsNullOrEmpty(storeKey))
                {
                    sql +=" WHERE A.STORE_KEY='" +storeKey.PreventSQLInjection() +"'";
                } 
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = WST_STORE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStore Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓类型获取线上仓信息。
        /// </summary>
        /// <param name="storeType">线上仓类型。</param>
        /// <returns>包含线上仓信息的数据集对象。</returns>
        public DataSet GetStores(string storeType)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = string.Format(@"SELECT STORE_NAME,STORE_KEY FROM WST_STORE WHERE STORE_TYPE='{0}'",storeType.PreventSQLInjection());              
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = WST_STORE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStores Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓主键删除线上仓记录。
        /// </summary>
        /// <param name="storeKey">线上仓主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteStore(string storeKey)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sql = "";
            try
            {
                dbconn = db.CreateConnection();
                dbconn.Open();
                //Create Transaction  
                dbtran = dbconn.BeginTransaction();
                if (storeKey != "")
                {
                    sql = @"DELETE FROM WST_STORE WHERE STORE_KEY='" + storeKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                }
                dbtran.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbtran.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteStore Error: " + ex.Message);
            }
            finally
            {
                dbtran = null;
                //Close Connection
                dbconn.Close();
                dbconn = null;
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新线上仓数据。
        /// </summary>
        /// <param name="dsParams">包含线上仓数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateStore(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            if (null != dsParams && dsParams.Tables.Contains(WST_STORE_FIELDS.DATABASE_TABLE_NAME))
            {

                DataTable dtParams = dsParams.Tables[WST_STORE_FIELDS.DATABASE_TABLE_NAME];
                string oldEditTime = "", storeKey = "";
                for (int i = 0; i < dsParams.Tables[0].Rows.Count; i++)
                {
                    if (dsParams.Tables[0].Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == COMMON_FIELDS.FIELD_COMMON_EDIT_TIME)
                    {
                        oldEditTime = dsParams.Tables[0].Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE].ToString();
                        storeKey = dsParams.Tables[0].Rows[i][COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY].ToString();
                        break;
                    }
                }

                #region 检查记录是否过期。
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WST_STORE_FIELDS.FIELD_STORE_KEY, storeKey);
                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                listCondition.Add(kvp);
                if (UtilHelper.CheckRecordExpired(db, WST_STORE_FIELDS.DATABASE_TABLE_NAME, listCondition, oldEditTime))
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                    return dsReturn;
                }
                #endregion
                //判断线上仓名称是否重复。
                if (dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME].ToString() == WST_STORE_FIELDS.FIELD_STORE_NAME)
                {
                    string storeName=Convert.ToString(dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE]);
                    string strSql = @"SELECT COUNT(*) FROM WST_STORE WHERE STORE_NAME='" + storeName.PreventSQLInjection() + "'";
                    int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                    if (count>0)
                    {
                        //提示“仓库名称不能重复，请重新输入。”
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.FMM.StoreEngine.StoreNameAlreadyExist}");
                        return dsReturn;
                    }
                }
                //生成UPDATE SQL
                List<string> sqlCommandList = new List<string>();
                DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList,
                                                        new WST_STORE_FIELDS(),
                                                        dsParams.Tables[WST_STORE_FIELDS.DATABASE_TABLE_NAME],
                                                        new Dictionary<string, string>() 
                                                        {                                                             
                                                            {WST_STORE_FIELDS.FIELD_EDIT_TIME, null},                                                            
                                                        },
                                                        new List<string>() 
                                                        {
                                                            WST_STORE_FIELDS.FIELD_STORE_KEY                                                           
                                                        },
                                                        WST_STORE_FIELDS.FIELD_STORE_KEY);
                //更新线上仓数据。
                if (sqlCommandList.Count > 0)
                {
                    dbconn = db.CreateConnection();
                    dbconn.Open();
                    //Create Transaction  
                    dbtran = dbconn.BeginTransaction();
                    try
                    {
                        foreach (string sql in sqlCommandList)
                        {
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        }
                        dbtran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                    catch (Exception ex)
                    {
                        dbtran.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("UpdateStore Error: " + ex.Message);
                    }
                    finally
                    {
                        dbtran = null;
                        //Close Connection
                        dbconn.Close();
                        dbconn = null;
                    }
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:Global.ServerMessage.M0004}");
            }
            return dsReturn;            
        }
        /// <summary>
        /// 获取线上仓数据集合。
        /// </summary>
        /// <returns>
        /// 包含线上仓数据的数据集对象。
        /// [STORE_KEY, STORE_NAME,STORE_TYPE]
        /// </returns>
        public DataSet GetStoreName()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT DISTINCT STORE_KEY, STORE_NAME,STORE_TYPE FROM WST_STORE WHERE OBJECT_STATUS='1' ORDER BY STORE_NAME";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStoreName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓类型获取可用且需要申请过账的线上仓信息。
        /// </summary>
        /// <param name="stroeType">线上仓类型,使用逗号(,)分隔。(可以为空字符串，表示查询所有线上仓）。</param>
        /// <returns>
        /// 查询得到的包含线上仓信息的数据集对象。
        /// </returns>
        public DataSet GetReworkStroeInfor(string stroeType)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT A.STORE_KEY,A.STORE_NAME,A.STORE_TYPE FROM WST_STORE A  WHERE 1=1 AND A.OBJECT_STATUS='1' and A.REQUEST_FLAG=1";
                if (!string.IsNullOrEmpty(stroeType))
                {
                    sqlCommand = sqlCommand + UtilHelper.BuilderWhereConditionString("A.STORE_TYPE", stroeType.Split(','));
                }
                sqlCommand = sqlCommand + " ORDER BY STORE_NAME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReworkStroeInfor Error: " + ex.Message);
            }
            return dsReturn;

        }
        /// <summary>
        /// 根据线上仓名称查询可用线上仓信息。
        /// </summary>
        /// <param name="storeName">线上仓名称，使用逗号(,)分隔。</param>
        /// <returns>包含重工线上仓信息的数据集对象。</returns>
        public DataSet GetReworkStore(string storeName)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT A.STORE_KEY,A.STORE_NAME,A.STORE_TYPE FROM WST_STORE A  WHERE 1=1 AND A.OBJECT_STATUS='1' ";
                if (!string.IsNullOrEmpty(storeName))
                {
                    sqlCommand = sqlCommand + UtilHelper.BuilderWhereConditionString("A.STORE_NAME", storeName.Split(','));
                }
                sqlCommand = sqlCommand + " ORDER BY STORE_NAME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReworkStroe Error: " + ex.Message);
            }
            return dsReturn;
        }
        //WST_STORE_MAT
        /// <summary>
        /// 根据线上仓主键获取打到线上仓的不良生产批次信息。
        /// </summary>
        /// <param name="storeKey">线上仓主键。</param>
        /// <returns>包含线上仓中不良生产批次信息的数据集对象。</returns>
        public DataSet GetLotsInStore(string storeKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT A.ROW_KEY,A.ITEM_NO AS LOT_NUMBER,A.ITEM_QTY,B.STORE_NAME,C.LOCATION_NAME                            
                        FROM WST_STORE_MAT A
                        INNER JOIN WST_STORE B ON A.STORE_KEY=B.STORE_KEY 
                        LEFT JOIN FMM_LOCATION C ON B.LOCATION_KEY=C.LOCATION_KEY 
                        WHERE A.OBJECT_STATUS='1'
                        AND A.ITEM_TYPE='Lot'";
                if (!string.IsNullOrEmpty(storeKey))
                {
                    sql = sql + " AND A.STORE_KEY='"+storeKey.PreventSQLInjection()+"'";
                }               
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotsInStore Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 从线上仓中转出生产批次。
        /// </summary>
        /// <param name="dsParams">包含转出生产批次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet TransferFromStore(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();          
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sql = "";
            string strEditor = "";
            if (dsParams != null && dsParams.Tables.Contains(WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME))
            { 
                DataTable dtParams=dsParams.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                strEditor=Convert.ToString(htParams[WST_STORE_MAT_FIELDS.FIELD_EDITOR]);

                try
                {
                    dbconn = db.CreateConnection();
                    dbconn.Open();
                    dbtran = dbconn.BeginTransaction();

                    string rowKey=Convert.ToString(htParams[WST_STORE_MAT_FIELDS.FIELD_ROW_KEY]);
                    string itemNo=Convert.ToString(htParams[WST_STORE_MAT_FIELDS.FIELD_ITEM_NO]);
                    //更新WST_STORE_MAT
                    sql = string.Format(@"UPDATE WST_STORE_MAT 
                                        SET OBJECT_STATUS=0,EDITOR='{0}',EDIT_TIME=GETDATE()
                                        WHERE ROW_KEY='{1}'", strEditor.PreventSQLInjection(), rowKey.PreventSQLInjection()); ;
                    db.ExecuteNonQuery(dbtran,CommandType.Text,sql);
                    //更新POR_LOT
                    sql = string.Format(@"UPDATE POR_LOT 
                                        SET STATE_FLAG=0,EDITOR='{0}',START_WAIT_TIME=GETDATE(),EDIT_TIME=GETDATE() 
                                        WHERE LOT_NUMBER='{1}'",strEditor.PreventSQLInjection(),itemNo.PreventSQLInjection());
                    db.ExecuteNonQuery(dbtran,CommandType.Text,sql);

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    dbtran.Commit();
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbtran.Rollback();
                    LogService.LogError("TransferFromStore Error: " + ex.Message);
                }
                finally
                {
                    dbtran = null;
                    dbconn.Close();
                    dbconn.Dispose();
                    dbconn = null;
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "no table in input parameters");
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("TransferFromStore Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
        /// <summary>
        /// 获取不良生产批次信息。
        /// </summary>
        /// <param name="paramTable">包含查询条件的数据集对象。</param>
        /// <returns>包含不良生产批次信息的数据集对象。</returns>
        public DataSet GetStoreMatInfo(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();          
            string sqlCommand = string.Empty;
            string storeType = string.Empty;
            try
            {

                Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                string orderNumber=Convert.ToString(htParams[WST_STORE_MAT_FIELDS.FIELD_WORKORDER_NUMBER]);
                //查询未作废的不良生产批次信息
                sqlCommand = string.Format(@"SELECT T.ITEM_NO,T.WORKORDER_NUMBER,T.ITEM_QTY,T.OBJECT_STATUS,F.LINE_NAME,S.ROUTE_STEP_NAME,W.STORE_NAME
                                          FROM WST_STORE_MAT T     
                                          LEFT JOIN WST_STORE W ON W.STORE_KEY=T.STORE_KEY                         
                                          LEFT JOIN FMM_PRODUCTION_LINE F ON T.LINE_KEY=F.PRODUCTION_LINE_KEY
                                          LEFT JOIN POR_ROUTE_STEP S ON T.STEP_KEY=S.ROUTE_STEP_KEY 
                                          WHERE T.STORE_KEY = W.STORE_KEY 
                                          AND T.OBJECT_STATUS<3
                                          AND T.WORKORDER_NUMBER LIKE '%{0}%'", orderNumber.PreventSQLInjection());

                if (htParams.ContainsKey(WST_STORE_MAT_FIELDS.FIELD_STORE_KEY))
                {
                    string storeKey = Convert.ToString(htParams[WST_STORE_MAT_FIELDS.FIELD_STORE_KEY]);
                    sqlCommand += @" AND T.STORE_KEY='" + storeKey.PreventSQLInjection()+ "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
                //查询线上仓工单不良信息合计。
                string sql = string.Format(@"SELECT T.WORKORDER_NUMBER, T.SUM_QUANTITY, T.SUM_QUANTITY_ALL, T.STORE_TYPE, D.NAME AS STORE_NAME
                                          FROM WST_STORE_SUM T
                                          LEFT JOIN (SELECT ITEM_ORDER,
                                                            MAX(CASE WHEN A.ATTRIBUTE_NAME='CODE' THEN A.ATTRIBUTE_VALUE END) {0},
                                                            MAX(CASE WHEN A.ATTRIBUTE_NAME='NAME' THEN A.ATTRIBUTE_VALUE END) {1}
                                                     FROM CRM_ATTRIBUTE             A,
                                                            BASE_ATTRIBUTE          B,
                                                            BASE_ATTRIBUTE_CATEGORY C
                                                     WHERE A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY
                                                        AND B.CATEGORY_KEY = C.CATEGORY_KEY
                                                        AND C.CATEGORY_NAME = 'STORE_TYPE'
                                                     GROUP BY ITEM_ORDER) D ON T.STORE_TYPE = D.CODE
                                          WHERE T.WORKORDER_NUMBER LIKE '%{2}%'","\"CODE\"", "\"NAME\"", 
                                          orderNumber.PreventSQLInjection());
               if (storeType.Length > 0)
               {
                   sql += " AND T.STORE_TYPE='"+storeType.PreventSQLInjection()+"'";
               } 
               DataTable sumTable = new DataView(db.ExecuteDataSet(CommandType.Text, sql).Tables[0]).ToTable(WST_STORE_SUM_FIELDS.DATABASE_TABLE_NAME);
               dsReturn.Tables.Add(sumTable);
               FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStoreMatInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号、线边仓主键、工序名称、线别主键、班别名称、状态获取批次返工或退库的数据。
        /// </summary>
        /// <param name="workerOrder">工单号码。</param>
        /// <param name="storeKey">线边仓主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <param name="shiftName">班别名称。</param>
        /// <param name="status">状态。（0：初始状态 1：申请过账 2：过账通过3：批次作废）</param>
        /// <returns>
        /// 查询得到的包含批次返工或退库的数据的数据集对象。
        /// </returns>
        public DataSet GetReworkLotInformation(string workerOrder,string storeKey,string operationName,string lineKey,string shiftName,string status) 
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = string.Format(@"SELECT '' AS CHECK_BOX,A.ROW_KEY,A.ITEM_NO AS LOT_NUMBER,A.LINE_KEY,D.LINE_NAME,A.STEP_KEY,B.WORK_ORDER_KEY,
                                                 C.ROUTE_STEP_NAME,A.ITEM_QTY,A.EDIT_TIME,A.STEP_KEY,A.ROUTE_KEY,A.ENTERPRISE_KEY,A.BALANCE_QTY,
                                                 A.BALANCE_EDITOR,P.LOT_KEY 
                                           FROM WST_STORE_MAT A  
                                           LEFT JOIN POR_WORK_ORDER B ON A.WORKORDER_NUMBER = B.ORDER_NUMBER  
                                           LEFT JOIN POR_ROUTE_STEP C ON A.STEP_KEY = C.ROUTE_STEP_KEY
                                           LEFT JOIN FMM_PRODUCTION_LINE D ON A.LINE_KEY = D.PRODUCTION_LINE_KEY                                 
                                           LEFT JOIN POR_LOT P ON CHARINDEX(P.LOT_NUMBER,A.ITEM_NO)=1
                                           WHERE A.WORKORDER_NUMBER IS NOT NULL
                                           AND A.DELETED_TERM_FLAG = 0
                                           AND A.OBJECT_STATUS = '{0}' AND A.STORE_KEY = '{1}'",
                                           status.PreventSQLInjection(),
                                           storeKey.PreventSQLInjection());

                sqlCommand = sqlCommand + " AND A.WORKORDER_NUMBER='" + workerOrder.PreventSQLInjection() + "'";

                if (!string.IsNullOrEmpty(operationName))
                {
                    sqlCommand = sqlCommand + " AND C.ROUTE_STEP_NAME='" + operationName.PreventSQLInjection() + "'";
                }
                if (!string.IsNullOrEmpty(lineKey) && lineKey != "0")
                {
                    sqlCommand = sqlCommand + " AND A.LINE_KEY='" + lineKey.PreventSQLInjection() + "'";
                }
                if (!string.IsNullOrEmpty(shiftName))
                {
                    sqlCommand = sqlCommand + "  AND A.SHIFT_NAME IN (SELECT CSD.DKEY FROM CAL_SCHEDULE_DAY CSD  WHERE CSD.SHIFT_VALUE ='" + shiftName.PreventSQLInjection() + "')";
                }
                sqlCommand = sqlCommand + "  ORDER BY A.EDIT_TIME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReworkLotInformation Error: " + ex.Message);
            }
            return dsReturn; 
        }
        /// <summary>
        /// 根据工序名（用","分开的工序名字符串），线别名（用","分开的工序名字符串），线边仓类型，班别名称查询等待返工或退库操作的批次信息。
        /// </summary>
        /// <param name="operations">工序名（用","分开的工序名字符串）</param>
        /// <param name="lines">线别名（用","分开的工序名字符串</param>
        /// <param name="storeTypes">线别仓类型。</param>
        /// <param name="shiftName">班别名称。</param>
        /// <returns>
        /// 查询得到的包含等待返工或退库操作批次信息的数据集对象。
        /// </returns>
        public DataSet SearchLotWaitingForTransact(string operations,string lines,string storeTypes,string shiftName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT T.WORKORDER_NUMBER,A.STORE_NAME,T.ITEM_NO,T.ITEM_QTY,B.LINE_NAME,C.ROUTE_STEP_NAME,CSD.SHIFT_VALUE
                            FROM WST_STORE_MAT T
                            INNER JOIN WST_STORE A ON T.STORE_KEY = A.STORE_KEY  
                            INNER JOIN FMM_PRODUCTION_LINE B ON T.LINE_KEY=B.PRODUCTION_LINE_KEY
                            INNER JOIN POR_ROUTE_STEP C ON T.STEP_KEY=C.ROUTE_STEP_KEY 
                            LEFT JOIN CAL_SCHEDULE_DAY CSD ON T.SHIFT_NAME=CSD.DKEY
                            WHERE A.OBJECT_STATUS = 1
                            AND A.REQUEST_FLAG=1
                            AND T.OBJECT_STATUS=0
                            AND T.DELETED_TERM_FLAG = 0";
                if (shiftName.Length > 0)
                {
                    sql += " AND CSD.SHIFT_VALUE = '" + shiftName + "'";
                }
                if (operations.Length > 0)
                {
                    string operationCondition=UtilHelper.BuilderWhereConditionString("C.ROUTE_STEP_NAME",operations.Split(','));
                    sql += operationCondition;
                }
                if (lines.Length > 0)
                {
                    string lineCondition = UtilHelper.BuilderWhereConditionString("B.LINE_NAME", lines.Split(','));
                    sql += lineCondition;
                }
                if (storeTypes.Length > 0)
                {
                    string storeCondition = UtilHelper.BuilderWhereConditionString("A.STORE_TYPE", storeTypes.Split(','));
                    sql += storeCondition;
                }

                sql += " ORDER BY T.WORKORDER_NUMBER,A.STORE_NAME,T.ITEM_NO";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchLotWaitingForTransact Error:" + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓主键和状态获取线上仓中工单信息。
        /// </summary>
        /// <param name="storeKey">线边仓主键。</param>
        /// <param name="strStatus">状态（0：初始状态 1：申请过账 2：过账通过3：批次作废）。</param>
        /// <returns>
        /// 查询得到的包含线上仓中工单信息的数据集对象。
        /// </returns>
        public DataSet GetWorkOrderInforByStroe(string storeKey,string strStatus)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = string.Format(@"SELECT DISTINCT A.WORKORDER_NUMBER, B.WORK_ORDER_KEY  
                                           FROM WST_STORE_MAT A
                                           LEFT JOIN POR_WORK_ORDER B ON A.WORKORDER_NUMBER = B.ORDER_NUMBER
                                           WHERE A.WORKORDER_NUMBER IS NOT NULL 
                                           AND A.OBJECT_STATUS = '{0}' AND A.STORE_KEY = '{1}' 
                                           ORDER BY A.WORKORDER_NUMBER ",
                                           strStatus.PreventSQLInjection(),storeKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderInforByStroe Error: " + ex.Message);
            }
            return dsReturn;

        }
        /// <summary>
        /// 根据线上仓类型获取线上仓中工单信息。
        /// </summary>
        /// <param name="storeType">线上仓类型。</param>
        /// <returns>
        /// 查询得到的包含线上仓中工单信息的数据集对象。
        /// </returns>
        public DataSet GetWorkOrderInforByStroeType(string storeType)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand =string.Format(@"SELECT A.ROW_KEY, A.WORKORDER_NUMBER, A.SUM_QUANTITY, B.WORK_ORDER_KEY
                                         FROM WST_STORE_SUM A
                                         LEFT JOIN POR_WORK_ORDER B ON A.WORKORDER_NUMBER = B.ORDER_NUMBER
                                         WHERE A.STORE_TYPE = '{0}' 
                                         ORDER BY A.WORKORDER_NUMBER ",
                                         storeType.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_SUM_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderInforByStroe Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓中重工生产批次主键获取对应批次数据。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="strRowKey">线上仓中生产批次主键。</param>
        /// <returns>包含重工生产批次的数据集对象。</returns>
        public static DataSet GetStoreLotDetailsInfor(Database db, string strRowKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            sql = string.Format(@"SELECT A.*, B.WORK_ORDER_KEY 
                                 FROM WST_STORE_MAT A 
                                 LEFT JOIN POR_WORK_ORDER B ON A.WORKORDER_NUMBER = B.ORDER_NUMBER
                                 WHERE A.ROW_KEY ='{0}'",strRowKey.PreventSQLInjection());
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            dsReturn.Tables[0].TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
            return dsReturn;
        }
        /// <summary>
        /// 根据返工或退库记录主键获取返工或退库批次的信息。
        /// </summary>
        /// <param name="strRowKey">返工或退库记录主键。</param>
        /// <returns>
        /// 查询得到的包含返工或退库批次信息的数据集对象。
        /// </returns>
        /// comment by peter 2012-2-23
        public DataSet GetReworkLotInformationByRowKey(string strRowKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = string.Format(@"SELECT A.*, B.LINE_NAME, C.ROUTE_STEP_NAME,D.WORK_ORDER_KEY,E.STORE_NAME
                                          FROM WST_STORE_MAT A
                                          LEFT JOIN FMM_PRODUCTION_LINE B ON A.LINE_KEY = B.PRODUCTION_LINE_KEY
                                          LEFT JOIN POR_ROUTE_STEP C ON A.STEP_KEY = C.ROUTE_STEP_KEY
                                          LEFT JOIN POR_WORK_ORDER D ON A.WORKORDER_NUMBER = D.ORDER_NUMBER
                                          LEFT JOIN WST_STORE E ON A.STORE_KEY=E.STORE_KEY
                                          WHERE A.ROW_KEY ='{0}'",strRowKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReworkLotInformationByRowKey Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据工序主键获取不良缺陷代码信息。
        /// </summary>
        /// <param name="strStepKey">工序主键。</param>
        /// <returns>
        /// 查询得到的包含不良缺陷代码信息的数据集对象。
        /// </returns>
        /// comment by peter 2012-2-23
        public DataSet GetDefectInforByStepKey(string strStepKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand =string.Format(@"SELECT 0 AS CODE_NUMBER,
                                               C.REASON_CODE_CATEGORY_NAME,         
                                               E.REASON_CODE_TYPE,
                                               E.REASON_CODE_NAME,
                                               D.CATEGORY_KEY,
                                               D.REASON_CODE_KEY
                                          FROM 
                                               POR_ROUTE_STEP           B,
                                               FMM_REASON_CODE_CATEGORY C,
                                               FMM_REASON_R_CATEGORY    D,
                                               FMM_REASON_CODE          E
                                          WHERE B.DEFECT_REASON_CODE_CATEGORY_KEY = C.REASON_CODE_CATEGORY_KEY
                                           AND C.REASON_CODE_CATEGORY_KEY = D.CATEGORY_KEY
                                           AND D.REASON_CODE_KEY = E.REASON_CODE_KEY
                                           AND B.ROUTE_STEP_KEY = '{0}'",strStepKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDefectInforByStepKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工序主键获取退库缺陷代码信息。
        /// </summary>
        /// <param name="strStepKey">工序主键。</param>
        /// <returns>
        /// 查询得到的包含退库缺陷代码信息的数据集对象。
        /// </returns>
        /// comment by peter 2012-2-23
        public DataSet GetReasonCodeForReturn(string strStepKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            string codeName = string.Empty;
            try
            {                
                DataSet attributeData = new DataSet();     
                //获取工步自定义属性集合。
                sqlCommand =string.Format(@"SELECT A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE
                                           FROM POR_ROUTE_STEP_ATTR A
                                           WHERE A.ROUTE_STEP_KEY ='{0}'",strStepKey.PreventSQLInjection());
                attributeData = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                if (attributeData != null && attributeData.Tables.Count > 0)//获取自定义属性成功
                {
                    for (int i = 0; i < attributeData.Tables[0].Rows.Count; i++)//遍历自定义属性。
                    {
                        //退库"ReturnCode"设置的退库代码组名称。
                        if (attributeData.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME].ToString() == "ReturnCode")
                        {
                            codeName = Convert.ToString(attributeData.Tables[0].Rows[i][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]).Trim();
                            break;
                        }
                    }
                } 
                //如果有设置退库的代码组名称。
                if (codeName.Length > 0)
                {
                    //查询退库的缺陷代码数据。
                    sqlCommand =string.Format(@"SELECT 0 AS CODE_NUMBER,A.*
                                               FROM FMM_REASON_CODE          A,
                                                   FMM_REASON_CODE_CATEGORY  B,
                                                   FMM_REASON_R_CATEGORY     C
                                               WHERE A.REASON_CODE_KEY = C.REASON_CODE_KEY
                                                   AND B.REASON_CODE_CATEGORY_KEY = C.CATEGORY_KEY
                                                   AND B.REASON_CODE_CATEGORY_NAME = '{0}'",codeName.PreventSQLInjection());

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                    dsReturn.Tables[0].TableName = FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCodeForReturn Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据用户获取错误消息。
        /// </summary>
        /// <param name="User">用户工号。</param>
        /// <returns>包含错误消息的数据集对象。</returns>
        public DataSet GetErrorMessageInfor(string User)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = string.Format(@"SELECT TOP 10 * FROM WIP_MESSAGE
                                      WHERE STATUS = 0 AND UPPER(TO_USER) = '{0}'
                                      ORDER BY CREATE_TIME DESC", User.ToUpper());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = WIP_MESSAGE_FIELDS.DATABASE_TABLE_NAME ;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetErrorMessageInfor Error: " + ex.Message);
            }
            return dsReturn;
        }
    
    }
}
