using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Share.Common;


namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 数据采集用于保存数据的管理类。主要包含保存采集数据相关和暂停生产批次的方法。
    /// </summary>
    public class EDCManagement
    {
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
        internal static void SaveEdcMainInfo(Database db,DbTransaction dbTrans, string lotKey, string edcPointKey,
            string editor, string equipmentKey, string oprLine, string shiftKey)
        {
            string activityType = string.Empty;
            string transactionState = string.Empty;
            string sql = string.Empty;
            List<string> sqlCommandList = new List<string>();
            DataTable lotTable = GetLotsInfo(db, dbTrans, lotKey).Tables[0];

            string lotNumber = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
            string enterpriseKey = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
            string routeKey = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
            string stepKey = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
            string workOrderKey = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
            string partKey = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_PART_VER_KEY].ToString();
            string stateFlag = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
            string edcName = lotTable.Rows[0][EDC_MAIN_FIELDS.FIELD_EDC_NAME].ToString();
            string stepName = lotTable.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
            string partNo = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_PART_NUMBER].ToString();
            string locationKey = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY].ToString();
            string matrialLot = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_LOT].ToString();
            string supplier = Convert.ToString(lotTable.Rows[0][POR_LOT_FIELDS.FIELD_SUPPLIER_NAME]);
            //string partType = lotTable.Rows[0][POR_LOT_FIELDS.FIELD_TYPE].ToString();
            string shiftName = Convert.ToString(lotTable.Rows[0][POR_LOT_FIELDS.FIELD_SHIFT_NAME]);
            string reworkFlag = Convert.ToString(lotTable.Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            string userName = Convert.ToString(lotTable.Rows[0][POR_LOT_FIELDS.FIELD_OPERATOR]);
            string computeName = Convert.ToString(lotTable.Rows[0][POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);

            //插入数据采集记录
            #region 创建生成EDC_MAIN_INS记录的INSERT SQL
            sql = @"SELECT EDC_KEY,SP_KEY FROM EDC_POINT WHERE ROW_KEY='" + edcPointKey.PreventSQLInjection() + "'";
            DataTable pointTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
            string edcInsKey = UtilHelper.GenerateNewKey(0);
            Hashtable mainTable = new Hashtable();
            EDC_MAIN_INS_FIELDS edcMainField = new EDC_MAIN_INS_FIELDS();
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY, pointTable.Rows[0][EDC_POINT_FIELDS.FIELD_EDC_KEY].ToString());
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_SP_KEY, pointTable.Rows[0][EDC_POINT_FIELDS.FIELD_SP_KEY].ToString());
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_ROUTE_KEY, routeKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_STEP_KEY, stepKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY, lotKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER, lotNumber);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_KEY, partKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_POINT_KEY, edcPointKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME, null);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_SUPPLIER, supplier);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME, stepName);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_NO, partNo);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_MATERIAL_LOT, matrialLot);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOCATION_KEY, locationKey);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_NAME, edcName);
            //mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_TYPE, partType);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDITOR, editor);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_CREATOR, editor);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_CREATE_TIME, null);
            mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDIT_TIME, null);
            sql = DatabaseTable.BuildInsertSqlStatement(edcMainField, mainTable, null);
            sqlCommandList.Add(sql);
            #endregion

            if (sqlCommandList.Count > 0)
            {
                foreach (string sqlCommand in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlCommand);
                }
            }
            //更新批次状态。
            if (Convert.ToInt32(stateFlag) == 0)//等待进站数据采集。
            {
                transactionState = "1";
                activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC;
                //更新为进站数据采集。
                sql = string.Format(@"UPDATE POR_LOT 
                                    SET STATE_FLAG=1,EDITOR='{0}',EDIT_TIME=GETDATE(),EDC_INS_KEY='{1}' 
                                    WHERE LOT_KEY='{2}'",
                                    editor.PreventSQLInjection(),
                                    edcInsKey.PreventSQLInjection(),
                                    lotKey.PreventSQLInjection());
            }
            else if (Convert.ToInt32(stateFlag) == 4)//等待出站数据收集
            {
                activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC;
                transactionState = "5";
                //更新为数据采集。
                sql = string.Format(@"UPDATE POR_LOT 
                                    SET STATE_FLAG=5,EDITOR='{0}',EDIT_TIME=GETDATE(),EDC_INS_KEY='{1}' 
                                    WHERE LOT_KEY='{2}'",
                                    editor.PreventSQLInjection(),
                                    edcInsKey.PreventSQLInjection(),
                                    lotKey.PreventSQLInjection());
            }
            db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

            #region 插入操作历史记录
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            Hashtable hashTable = new Hashtable();
            string strTransactionKey = UtilHelper.GenerateNewKey(0);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strTransactionKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, activityType);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString());
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString());
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "EDC");
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);

            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, "CN-ZH");
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, transactionState);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString());
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, userName);
            hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, computeName);

            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, hashTable, null);
            db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
            #endregion
        }

        /// <summary>
        /// 保存采集到的明细数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dataset">包含到采集到的明细数据的数据集。</param>
        internal static void SaveEDCCollectionData(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            string editor = Convert.ToString(dsParams.ExtendedProperties[EDC_MAIN_INS_FIELDS.FIELD_EDITOR]);
            EDC_COLLECTION_DATA_FIELDS edcCollectionData = new EDC_COLLECTION_DATA_FIELDS();
            string sql = "";
            if (dsParams.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME];
                for (int i = 0; i < dtParams.Rows.Count; i++)
                {
                    OperationAction action = (OperationAction)Convert.ToInt32(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]);
                    switch (action)
                    {
                        case OperationAction.New:
                            //生成INSERT SQL
                            string colkey= UtilHelper.GenerateNewKey(0);
                            dtParams.Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_COL_KEY] = colkey;
                            sql = DatabaseTable.BuildInsertSqlStatement(edcCollectionData, dtParams, i, 
                                                                    new Dictionary<string, string>() 
                                                                    {                                               
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDITOR, editor},  
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, string.Empty},   
                                                                        {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"},
                                                                    },
                                                                    new List<string>(){
                                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION                                                           
                                                                    });

                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            break;
                        case OperationAction.Update:
                            //生成UPDATE SQL
                            string paramValue =Convert.ToString(dtParams.Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE]);
                            string colKey = Convert.ToString(dtParams.Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_COL_KEY]);
                            sql = string.Format(@"UPDATE EDC_COLLECTION_DATA 
                                                  SET PARAM_VALUE='{1}',EDIT_TIME= GETDATE(),EDITOR='{2}'
                                                  WHERE COL_KEY='{0}'",
                                                  colKey.PreventSQLInjection(),
                                                  paramValue.PreventSQLInjection(),
                                                  editor.PreventSQLInjection());
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
         /// <summary>
        /// 保存数据采集实例数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dataset">包含数据采集实例数据的数据集。</param>
        internal static void SaveEDCMainIns(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            EDC_MAIN_INS_FIELDS edcMainIns = new EDC_MAIN_INS_FIELDS();
            DataTable dtParams = dsParams.Tables[EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME];
            Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME,null);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_COL_END_TIME, null);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_CREATE_TIME, null);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_EDIT_TIME,null);
            string sql = DatabaseTable.BuildInsertSqlStatement(edcMainIns, htParams, null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
        }
        /// <summary>
        /// 保存数据采集实例数据。
        /// </summary>
        /// <remarks>
        /// 新增数据采集实例数据并根据生产批次状态以完成生产批次的数据采集。
        /// </remarks>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dataset">包含数据采集实例数据的数据集。</param>
        internal static void SaveEDCMainIn(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            string sql = "";
            string activityType = string.Empty;
            string editor = "", lineKey = "", workOrderKey = "", quantity = "", stepKey = "";
            //新增数据采集实例数据
            EDC_MAIN_INS_FIELDS edcMainIns = new EDC_MAIN_INS_FIELDS();
            DataTable dtParams = dsParams.Tables[EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            string lotKey =Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY]);
            string edcInsKey = Convert.ToString(htParams[EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY]);
            string transactionState = string.Empty;
            editor = Convert.ToString(htParams[COMMON_FIELDS.FIELD_COMMON_EDITOR]);
            htParams.Remove(COMMON_FIELDS.FIELD_COMMON_EDITOR);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME, null);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_COL_END_TIME, null);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_CREATOR, editor);
            htParams.Add(EDC_MAIN_INS_FIELDS.FIELD_EDITOR, editor);
            sql = DatabaseTable.BuildInsertSqlStatement(edcMainIns, htParams, null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
            //获取生产批次状态。
            int state = -1;
            sql = @"SELECT STATE_FLAG FROM POR_LOT WHERE LOT_KEY='" + lotKey + "'";
            state = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
            //更新生产批次状态。
            if (state == 0)
            {
                transactionState = "1";
                activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC;
                sql = @"UPDATE POR_LOT SET STATE_FLAG=1, EDITOR='" + editor.PreventSQLInjection() + "'," +
                      "EDIT_TIME=GETDATE()," +
                      "EDC_INS_KEY='" + edcInsKey.PreventSQLInjection() + "' " +
                      "WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
            }
            else if (state == 4)
            {
                activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC;
                transactionState = "5";
                sql = @"UPDATE POR_LOT SET STATE_FLAG=5, EDITOR='" + editor.PreventSQLInjection() + "'," +
                      "EDIT_TIME=GETDATE()," +
                      "EDC_INS_KEY='" + edcInsKey.PreventSQLInjection() + "' " +
                      "WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
            }
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
            //获取批次信息。
            DataSet dsLot =GetLotsInfo(db,dbtran, lotKey);
            if (null != dsLot && dsLot.Tables.Count > 0 && dsLot.Tables[0].Rows.Count > 0)
            {
                lineKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                quantity = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);
                workOrderKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                stepKey = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);

                #region 新增交易记录
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                Hashtable hashTable = new Hashtable();
                string strTransactionKey = UtilHelper.GenerateNewKey(0);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strTransactionKey);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, activityType);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, quantity);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, quantity);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "EDC");

                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, "CN-ZH");
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME,null);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, transactionState);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                hashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, hashTable, null);
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                #endregion
            }
            else
            {
                throw new Exception("获取批次信息失败。");
            }
        }
        /// <summary>
        /// 新增数据采集实例数据。
        /// </summary>
        /// <remarks>
        /// 新增数据采集实例数据并根据生产批次状态以完成生产批次的数据采集。
        /// </remarks>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dataset">包含数据采集实例数据的数据集。</param>
        internal static void InsertEdcMainInsData(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            int lotState = -1;
            string transactionState = string.Empty;
            string activityType = string.Empty;
            string sqlCommand = string.Empty;
            List<string> sqlCommandList = new List<string>();
            DataTable dtParams = dsParams.Tables[EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME];
            //生成新增数据采集实例数据的SQL
            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                new EDC_MAIN_INS_FIELDS(),
                                                dtParams,
                                                new Dictionary<string, string>() 
                                                {
                                                   {EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME,null},
                                                   {EDC_MAIN_INS_FIELDS.FIELD_COL_END_TIME,null}
                                                },
                                                new List<string>() { 
                                                     POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY,
                                                     POR_LOT_FIELDS.FIELD_QUANTITY,
                                                     COMMON_FIELDS.FIELD_COMMON_EDITOR 
                                                });

            //获取批次状态。
            string lotKey = Convert.ToString(dtParams.Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY]);
            sqlCommand = string.Format(@"SELECT STATE_FLAG FROM POR_LOT WHERE LOT_KEY='{0}'", lotKey.PreventSQLInjection());
            lotState = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
            //更新批次状态。
            sqlCommand = @"UPDATE POR_LOT
                              SET STATE_FLAG  = {0},
                                  EDITOR      = '{1}',
                                  EDIT_TIME   =GETDATE(),
                                  EDC_INS_KEY = '{2}'
                            WHERE LOT_KEY = '{3}'";

            if (lotState == 0)
            {
                transactionState = "1";
                activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_INEDC;
                sqlCommand = string.Format(sqlCommand, 1,
                                dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_EDITOR],
                                dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY],
                                dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY]);
            }
            else if (lotState == 4)
            {
                activityType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_OUTEDC;
                transactionState = "5";
                sqlCommand = string.Format(sqlCommand, 5,
                                dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_EDITOR],
                                dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY],
                                dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY]);
            }

            sqlCommandList.Add(sqlCommand);
            //生产新增操作记录的SQL
            DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                   new WIP_TRANSACTION_FIELDS(),
                                                   new DataTable(WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME),
                                                   new Dictionary<string, string>() 
                                                    {
                                                        {EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME,null},
                                                        {EDC_MAIN_INS_FIELDS.FIELD_COL_END_TIME, null},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, UtilHelper.GenerateNewKey(0)},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0"},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY,  dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY].ToString()},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, activityType},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, dtParams.Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString()},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, dtParams.Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString()},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "EDC"},                                                        
                                                        {WIP_TRANSACTION_FIELDS.FIELD_EDITOR, dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_EDITOR].ToString()},                                
                                                        {WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, "CN-ZH"},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, transactionState},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, dtParams.Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString()},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_STEP_KEY].ToString()},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, dtParams.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString()},
                                                        {WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, dtParams.Rows[0][EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY].ToString()}
                                                    },
                                                   new List<string>());
            //执行数据新增。
            if (sqlCommandList.Count > 0)
            {
                DbConnection dbConn = db.CreateConnection();
                dbConn.Open();
                DbTransaction dbTrans = dbConn.BeginTransaction();
                try
                {
                    foreach (string sql in sqlCommandList)
                    {
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
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
                    dbTrans = null;
                    dbConn.Close();
                    dbConn = null;
                }
            }
        }
        /// <summary>
        /// 检查并且暂停生产批次。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dbTran">数据库操作事务对象。</param>
        /// <param name="lotKey">生产批次主键。</param>
        /// <param name="dsParams">包含生产批次暂停数据的数据集对象。</param>
        internal static void CheckAndHoldLot(Database db,DbTransaction dbTran,string lotKey,DataSet dsParams)
        {
           //检查生产批次对应的工步是否设置了预设暂停批次的自定义属性。
           string sql = string.Format(@"SELECT A.ATTRIBUTE_VALUE
                                        FROM POR_ROUTE_STEP_ATTR A,POR_LOT B
                                        WHERE A.ROUTE_STEP_KEY =B.CUR_STEP_VER_KEY
                                        AND B.LOT_KEY='{0}' AND A.ATTRIBUTE_NAME='{1}'",
                                        lotKey.PreventSQLInjection(),
                                        ROUTE_STEP_ATTRIBUTE.FutureHold);
           string operationName = Convert.ToString(db.ExecuteScalar(dbTran,CommandType.Text, sql));
           //属性值不为空。预设暂停批次。
           if (!string.IsNullOrEmpty(operationName))
           {
              InsertFutureHold(db, dbTran, lotKey, operationName);
              return;
           }
           //直接暂停批次。
           HoldLot(db, dbTran, dsParams);
        }
        /// <summary>
        /// 新增生产批次预设暂停的记录。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dbtran">数据库操作事务对象。</param>
        /// <param name="lotKey">生产批次主键。</param>
        /// <param name="operationName">暂停批次的工序。</param>
        private static void InsertFutureHold(Database db,DbTransaction dbtran,string lotKey,string operationName)
        {
            //根据批次主键和工序名称获取暂停的工艺流程组主键、工艺流程主键、工步主键以及批次信息。
            string sql = string.Format(@"SELECT a.ROUTE_ENTERPRISE_VER_KEY,a.ROUTE_ROUTE_VER_KEY,a.ROUTE_STEP_KEY,
                                            b.LOT_NUMBER,b.CUR_ROUTE_VER_KEY,b.CUR_STEP_VER_KEY,b.WORK_ORDER_NO
                                        FROM V_PROCESS_PLAN a
                                        INNER JOIN POR_LOT b ON a.ROUTE_ENTERPRISE_VER_KEY=b.ROUTE_ENTERPRISE_VER_KEY
                                        WHERE a.ROUTE_STEP_NAME='{0}'
                                        AND b.LOT_KEY='{1}'", 
                                        operationName.PreventSQLInjection(), 
                                        lotKey.PreventSQLInjection());
            DataSet dsInfo = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
            if (null != dsInfo
                && dsInfo.Tables.Count > 0
                && dsInfo.Tables[0].Rows.Count > 0)
            {
                DataRow drInfo = dsInfo.Tables[0].Rows[0];

                //检查生产批次对应的工步是否设置了预设暂停批次的自定义属性。
                sql = string.Format(@"SELECT A.ATTRIBUTE_VALUE
                                    FROM POR_ROUTE_STEP_ATTR A,POR_LOT B
                                    WHERE A.ROUTE_STEP_KEY =B.CUR_STEP_VER_KEY
                                    AND B.LOT_KEY='{0}' AND A.ATTRIBUTE_NAME='{1}'",
                                    lotKey.PreventSQLInjection(),
                                    ROUTE_STEP_ATTRIBUTE.HoldPassword);
                string holdPassword = Convert.ToString(db.ExecuteScalar(dbtran, CommandType.Text, sql));

                Hashtable htFuturHold = new Hashtable();
                WIP_FUTUREHOLD_FIELDS futureHoldField = new WIP_FUTUREHOLD_FIELDS();
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_ACTION_NAME, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_CREATE_TIME, null);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_CREATOR, "system");
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_DELETE_FLAG, 0);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_EDIT_TIME, null);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_EDITOR, "system");
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_ENTERPRISE_KEY, drInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_LEVEL, 1);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_HOLD_PASSWORD, holdPassword);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_KEY, lotKey);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_LOT_NUMBER, drInfo[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_OPERATION_NAME,operationName);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE, string.Empty);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_KEY, string.Empty);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_KEY, string.Empty);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_REASON_CODE_CATEGORY_NAME, string.Empty);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_REMARK, "超出控制规格线。");
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_ROUTE_KEY, drInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_ROW_KEY,  UtilHelper.GenerateNewKey(0));
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ENTERPRISE_KEY, drInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_SET_ROUTE_KEY, drInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_SET_STEP_KEY, drInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_STATUS, 0);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_STEP_KEY,  drInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                htFuturHold.Add(WIP_FUTUREHOLD_FIELDS.FIELDS_WORKORDER_NUMBER,  drInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                sql = DatabaseTable.BuildInsertSqlStatement(futureHoldField, htFuturHold, null);
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
            }
        }
        /// <summary>
        /// 暂停生产批次。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dsParams">包含暂停生产批次数据的数据集。</param>
        private static void HoldLot(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            if (!dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
            {
                return;
            }
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);

            string lotKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            string transactionKey = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
            string reasonCode = Convert.ToString(htParams[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY]);
            string reasonCodeName = Convert.ToString(htParams[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME]);
            string comment = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
            string shiftName = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            string shiftKey = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            string oprComputer = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            string opUser = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            string editTimeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);

            DataSet dsLot = GetLotsInfo(db, dbtran, lotKey);
            if (null != dsLot
                && dsLot.Tables.Count > 0
                && dsLot.Tables[0].Rows.Count > 0)
            {
                DataRow drLot = dsLot.Tables[0].Rows[0];

                //组织暂停操作数据。
                WIP_TRANSACTION_FIELDS transFileds = new WIP_TRANSACTION_FIELDS();
                DataTable dtHoldTransaction = CommonUtils.CreateDataTable(transFileds);
                DataRow drHoldTransaction = dtHoldTransaction.NewRow();
                dtHoldTransaction.Rows.Add(drHoldTransaction);
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = "检验数据超出控制线。";
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY] = drLot[POR_LOT_FIELDS.FIELD_EDC_INS_KEY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = editTimeZone;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = editor;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = drLot[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = drLot[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = drLot[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = drLot[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = drLot[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = drLot[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = drLot[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = drLot[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR] = "system";
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER] = oprComputer;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = drLot[POR_LOT_FIELDS.FIELD_LINE_NAME];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE] = drLot[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = lotKey;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE] = 0;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = drLot[POR_LOT_FIELDS.FIELD_QUANTITY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = drLot[POR_LOT_FIELDS.FIELD_QUANTITY];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = drLot[POR_LOT_FIELDS.FIELD_IS_REWORKED];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY] = shiftKey;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = shiftName;
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = drLot[POR_LOT_FIELDS.FIELD_STATE_FLAG];
                drHoldTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY];
                //组织暂停数据
                //检查生产批次对应的工步是否设置了预设暂停批次的自定义属性。
                string sql = string.Format(@"SELECT A.ATTRIBUTE_VALUE
                                            FROM POR_ROUTE_STEP_ATTR A,POR_LOT B
                                            WHERE A.ROUTE_STEP_KEY =B.CUR_STEP_VER_KEY
                                            AND B.LOT_KEY='{0}' AND A.ATTRIBUTE_NAME='{1}'",
                                            lotKey.PreventSQLInjection(),
                                            ROUTE_STEP_ATTRIBUTE.HoldPassword);
                string holdPassword = Convert.ToString(db.ExecuteScalar(dbtran, CommandType.Text, sql));
                WIP_HOLD_RELEASE_FIELDS holdFields = new WIP_HOLD_RELEASE_FIELDS();
                DataTable dtHold = CommonUtils.CreateDataTable(holdFields);
                DataRow drHold = dtHold.NewRow();
                dtHold.Rows.Add(drHold);
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDIT_TIMEZONE] = editTimeZone;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_EDITOR] = "system";
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_DESCRIPTION] = comment;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_OPERATOR] = "system";
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_PASSWORD] = holdPassword;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIME] = DBNull.Value;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_HOLD_TIMEZONE] = editTimeZone;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY] = string.Empty;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME] = string.Empty;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                drHold[WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                //组织暂停参数。
                DataSet dsHoldParams = new DataSet();
                dsHoldParams.Tables.Add(dtHoldTransaction);
                dsHoldParams.Tables.Add(dtHold);
                //执行批次暂停。
                RemotingServer.ServerObjFactory.Get<ILotOperationEngine>().LotHold(dbtran, dsHoldParams);
            }
        }
        /// <summary>
        /// 根据批次主键或批次号获取批次信息。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="lotKey">批次主键或批次号。为空则获取所有生产批次数据。</param>
        /// <returns>
        /// 包含批次信息的数据集。 
        /// </returns>
        private static DataSet GetLotsInfo(Database db,DbTransaction dbtrans,string lotKey)
        {
            string sql =string.Format(@"SELECT A.*,
                                           B.ENTERPRISE_NAME,
                                           C.ROUTE_NAME,
                                           D.EDC_LIST_KEY,
                                           D.SAMPLING_KEY,
                                           D.ROUTE_OPERATION_VER_KEY,
                                           D.ROUTE_OPERATION_NAME,
                                           D.ROUTE_STEP_NAME,
                                           D.SCRAP_REASON_CODE_CATEGORY_KEY,   
                                           D.DEFECT_REASON_CODE_CATEGORY_KEY,                     
                                           D.ROUTE_STEP_NAME,
                                           D.RE_ROUTE_ENTERPRISE_VER_KEY,
                                           D.RE_START_ROUTE_VER_KEY,
                                           D.RE_START_STEP_KEY,
                                           D.DURATION,
                                           E.EDC_NAME,
                                           F.EQUIPMENT_KEY                     
                                        FROM POR_LOT A
                                        LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                        LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                        LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                        LEFT JOIN EDC_MAIN E ON E.EDC_KEY=D.EDC_LIST_KEY
                                        LEFT JOIN EMS_LOT_EQUIPMENT F ON A.LOT_KEY=F.LOT_KEY AND F.STEP_KEY=A.CUR_STEP_VER_KEY AND F.END_TIMESTAMP IS NULL
                                        WHERE (A.LOT_KEY='{0}' OR A.LOT_NUMBER='{0}')
                                        ORDER BY F.START_TIMESTAMP DESC",
                                        lotKey.PreventSQLInjection());
            DataSet dsReturn = db.ExecuteDataSet(dbtrans,CommandType.Text, sql);
            dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            return dsReturn;
        }
    }
}
