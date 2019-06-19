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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 批次管理类。
    /// </summary>
    public class LotManagement
    {
        /// <summary>
        /// 创建批次。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbTran">数据库事务操作对象。</param>
        /// <param name="dsParams">包含批次创建信息的数据集对象。</param>
        public static void CreateLot(Database db, DbTransaction dbTran, DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            DateTime curTime = UtilHelper.GetSysdate(db);
            string lotEditTime = curTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
            {
                DataTable dtMainData = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                DataTable dtAddtionData = dsParams.Tables[1];   //存放附加数据

                Hashtable htMainData = CommonUtils.ConvertToHashtable(dtMainData);
                Hashtable htAddtionData = CommonUtils.ConvertToHashtable(dtAddtionData);
                string enterpriseName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME]);
                string routeName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME]);
                string stepName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME]);

                string editor = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_CREATOR]);
                string editTimezone = string.Empty;
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE))
                {
                    editTimezone = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]);
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_EDITOR) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_EDITOR, editor);
                }
                htMainData[POR_LOT_FIELDS.FIELD_LOT_KEY] = UtilHelper.GenerateNewKey(0);
                htMainData[POR_LOT_FIELDS.FIELD_EDIT_TIME] = null;
                htMainData[POR_LOT_FIELDS.FIELD_CREATE_TIME] = null;
                htMainData[POR_LOT_FIELDS.FIELD_START_WAIT_TIME] = null;

                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO, htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                }
                else
                {
                    htMainData[POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO] = htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO];
                }

                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_STATE_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_REWORKED) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_REWORKED, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT, "1");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_HOLD_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_SPLITED) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_SPLITED, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG, "0");
                }
                //创建批次。
                POR_LOT_FIELDS porLotFields = new POR_LOT_FIELDS();
                string sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, htMainData, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                #region 插入操作记录
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                Hashtable htTransaction = new Hashtable();
                DataTable dtTransaction = new DataTable();
                string strChildTransKey = UtilHelper.GenerateNewKey(0);
                string strShiftKey = UtilHelper.GetShiftKey(db, curTime.ToString("yyyy-MM-dd HH:mm:ss"));
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, htMainData[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, htMainData[POR_LOT_FIELDS.FIELD_QUANTITY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, htMainData[POR_LOT_FIELDS.FIELD_QUANTITY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, htMainData[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htMainData[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, strShiftKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htMainData[POR_LOT_FIELDS.FIELD_SHIFT_NAME]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, htMainData[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, htMainData[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, htMainData[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, curTime);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, editor);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, htMainData[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                #endregion

                if (dsParams.Tables.Contains(POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                           new POR_LOT_ATTR_FIELDS(),
                                                           dsParams.Tables[POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                           new Dictionary<string, string>() 
                                                           {  
                                                                {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME,null},
                                                                {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}                                                                      
                                                           },
                                                           new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE });
                }
                foreach (string s in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTran, CommandType.Text, s);
                }
            }
        }

        /// <summary>
        /// 创建批次。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dsParams">包含批次创建信息的数据集对象。</param>
        /// <param name="drLotCell">创批组件对应的。</param>
        public static void CreateLot(Database db, DataSet dsParams, DataRow drCellData)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            DateTime curTime = UtilHelper.GetSysdate(db);
            string lotEditTime = curTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
            {
                DataTable dtMainData = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                DataTable dtAddtionData = dsParams.Tables[1];   //存放附加数据

                Hashtable htMainData = CommonUtils.ConvertToHashtable(dtMainData);
                Hashtable htAddtionData = CommonUtils.ConvertToHashtable(dtAddtionData);
                string enterpriseName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME]);
                string routeName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME]);
                string stepName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME]);

                string editor = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_CREATOR]);
                string editTimezone = string.Empty;
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE))
                {
                    editTimezone = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]);
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_EDITOR) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_EDITOR, editor);
                }
                string lotKey = UtilHelper.GenerateNewKey(0);
                htMainData[POR_LOT_FIELDS.FIELD_LOT_KEY] = lotKey;
                htMainData[POR_LOT_FIELDS.FIELD_EDIT_TIME] = null;
                htMainData[POR_LOT_FIELDS.FIELD_CREATE_TIME] = null;
                htMainData[POR_LOT_FIELDS.FIELD_START_WAIT_TIME] = null;

                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO, htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                }
                else
                {
                    htMainData[POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO] = htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO];
                }

                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_STATE_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_REWORKED) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_REWORKED, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT, "1");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_HOLD_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_SPLITED) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_SPLITED, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG, "0");
                }
                //创建批次。
                POR_LOT_FIELDS porLotFields = new POR_LOT_FIELDS();
                string sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, htMainData, null);
                db.ExecuteNonQuery(CommandType.Text, sql);

                #region 插入组件片对应电池片信息

                string sqlCommond = string.Empty;

                sqlCommond = string.Format(@"SELECT LOT_KEY FROM POR_LOT_CELL_PARAM
                                            WHERE LOT_NUMBER = '{0}' 
                                            AND IS_USED = 'Y'", drCellData["LOT_NUMBER"].ToString());
                DataTable dtCellReturn = db.ExecuteDataSet(CommandType.Text, sqlCommond).Tables[0];

                if (dtCellReturn.Rows.Count > 0)
                {
                    sqlCommond = string.Format(@"UPDATE  POR_LOT_CELL_PARAM
                                                SET IS_USED = 'N'
                                                WHERE LOT_KEY = '{0}'", dtCellReturn.Rows[0]["LOT_KEY"].ToString());
                    db.ExecuteNonQuery(CommandType.Text, sqlCommond);
                }

                sqlCommond = string.Format(@"INSERT INTO POR_LOT_CELL_PARAM (  [LOT_KEY]
                                                                              ,[LOT_NUMBER]
                                                                              ,[CELLLOT1]
                                                                              ,[CELLPN1]
                                                                              ,[CELLLOT2]
                                                                              ,[CELLPN2]
                                                                              ,[PACKAGEQTY]
                                                                              ,[CELLSUPPLIER]
                                                                              ,[CELLFACTORY]
                                                                              ,[CELLLINE]
                                                                              ,[CELLCOLOR]
                                                                              ,[CELLEFFICIENCY]
                                                                              ,[CELLPOWER]
                                                                              ,[SMALL_PACK_NUMBER]
                                                                              ,[IS_USED]
                                                                              ,[CREATOR]
                                                                              ,[CREATE_TIME]
                                                                              ,[CREATE_TIMEZONE]
                                                                              ,[EDITOR]
                                                                              ,[EDIT_TIME]
                                                                              ,[EDIT_TIMEZONE]
                                                                              ,[ETL_FLAG]
                                                                              ,[MATERIAL_TIME])
                                                                              VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',
                                                                                      '{12}','{13}','Y','{14}',SYSDATETIME(),'{15}','{16}',SYSDATETIME(),'{17}','Y','{18}')",
                                                                               lotKey,
                                                                               drCellData["LOT_NUMBER"].ToString(),
                                                                               drCellData["CELLLOT1"].ToString(),
                                                                               drCellData["CELLPN1"].ToString(),
                                                                               drCellData["CELLLOT2"].ToString(),
                                                                               drCellData["CELLPN2"].ToString(),
                                                                               drCellData["PACKAGEQTY"].ToString(),
                                                                               drCellData["CELLSUPPLIER"].ToString(),
                                                                               drCellData["CELLFACTORY"].ToString(),
                                                                               drCellData["CELLLINE"].ToString(),
                                                                               drCellData["CELLCOLOR"].ToString(),
                                                                               drCellData["CELLEFFICIENCY"].ToString(),
                                                                               drCellData["CELLPOWER"].ToString(),
                                                                               drCellData["SMALL_PACK_NUMBER"].ToString(),
                                                                               editor,
                                                                               editTimezone,
                                                                               editor,
                                                                               editTimezone,
                                                                               drCellData.Table.Columns.Contains("MATERIAL_TIME") ? drCellData["MATERIAL_TIME"] : new LotEngine().GetSysdate().AddDays(-15)
                );


                db.ExecuteNonQuery(CommandType.Text, sqlCommond);
                #endregion


                #region 插入操作记录
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                Hashtable htTransaction = new Hashtable();
                DataTable dtTransaction = new DataTable();
                string strChildTransKey = UtilHelper.GenerateNewKey(0);
                string strShiftKey = UtilHelper.GetShiftKey(db, curTime.ToString("yyyy-MM-dd HH:mm:ss"));
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, htMainData[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, htMainData[POR_LOT_FIELDS.FIELD_QUANTITY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, htMainData[POR_LOT_FIELDS.FIELD_QUANTITY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, htMainData[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htMainData[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, strShiftKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htMainData[POR_LOT_FIELDS.FIELD_SHIFT_NAME]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, htMainData[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, htMainData[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, htMainData[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, curTime);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, editor);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, htMainData[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
                db.ExecuteNonQuery(CommandType.Text, sql);
                #endregion

                if (dsParams.Tables.Contains(POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                           new POR_LOT_ATTR_FIELDS(),
                                                           dsParams.Tables[POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                           new Dictionary<string, string>() 
                                                           {  
                                                                {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME,null},
                                                                {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}                                                                      
                                                           },
                                                           new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE });
                }
                foreach (string s in sqlCommandList)
                {
                    db.ExecuteNonQuery(CommandType.Text, s);
                }
            }
        }



        /// <summary>
        /// 根据批次主键获取批次及其自定义属性值数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>包含批次及其自定义属性值数据的数据集对象。</returns>
        public static DataSet GetLotDetailsEx(Database db, DbTransaction dbTrans, string lotKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Format(@"SELECT A.* FROM POR_LOT A
                                        WHERE A.LOT_KEY = '{0}'",
                                        lotKey.PreventSQLInjection());
            dsReturn = db.ExecuteDataSet(dbTrans, CommandType.Text, sql);
            dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;

            sql = string.Format(@"SELECT LOT_KEY,ATTRIBUTE_KEY,ATTRIBUTE_NAME,ATTRIBUTE_VALUE,EDIT_TIME , '' AS DATA_TYPE,EDITOR
                                FROM POR_LOT_ATTR
                                WHERE LOT_KEY = '{0}' 
                                ORDER BY ATTRIBUTE_NAME",
                                lotKey.PreventSQLInjection());
            DataSet dsTemp = db.ExecuteDataSet(dbTrans, CommandType.Text, sql);
            DataTable tTable = dsTemp.Tables[0];
            tTable.TableName = POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME;
            dsReturn.Merge(tTable, false, MissingSchemaAction.Add);
            return dsReturn;
        }
        /// <summary>
        /// 根据批次主键获取批次基础信息。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次基础数据的数据集对象。
        /// 数据集对象中包含一个名称为<see cref="POR_LOT_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// </returns>
        public static DataSet GetLotBasicInfo(Database db, string lotKey)
        {
            return GetLotBasicInfo(db, null, lotKey);
        }
        /// <summary>
        /// 根据批次主键获取批次基础信息。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbTrans">数据库事务对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次基础数据的数据集对象。
        /// 数据集对象中包含一个名称为<see cref="POR_LOT_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// </returns>
        public static DataSet GetLotBasicInfo(Database db, DbTransaction dbTrans, string lotKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";

            sql = string.Format(@"SELECT A.*,B.ENTERPRISE_NAME,B.ENTERPRISE_VERSION,C.ROUTE_NAME ,D.ROUTE_STEP_NAME,F.EQUIPMENT_KEY
                                FROM POR_LOT A
                                LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                LEFT JOIN EMS_LOT_EQUIPMENT F ON A.LOT_KEY=F.LOT_KEY AND F.STEP_KEY=A.CUR_STEP_VER_KEY
                                WHERE A.LOT_KEY = '{0}'
                                ORDER BY F.START_TIMESTAMP DESC",
                                lotKey.PreventSQLInjection());
            if (dbTrans == null)
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            else
            {
                dsReturn = db.ExecuteDataSet(dbTrans, CommandType.Text, sql);
            }
            dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            return dsReturn;
        }
        /// <summary>
        /// 根据批次主键获取批次自定义属性值数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>包含批次自定义属性值数据的数据集对象。</returns>
        public static DataSet GetLotUDAsInfo(Database db, string lotKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Format(@"SELECT LOT_KEY ,ATTRIBUTE_KEY,ATTRIBUTE_NAME,ATTRIBUTE_VALUE,EDIT_TIME ,'' AS DATA_TYPE,EDITOR 
                                        FROM POR_LOT_ATTR 
                                        WHERE LOT_KEY = '{0}' 
                                        ORDER BY ATTRIBUTE_NAME", lotKey.PreventSQLInjection()); ;

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            dsReturn.Tables[0].TableName = POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME;
            return dsReturn;
        }
        /// <summary>
        /// 更新批次基础数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dsParams">包含批次基础数据的数据集对象。</param>
        public static void UpdateLotBasicInfo(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            if (dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                //initialize tablefields
                POR_LOT_FIELDS porLotFields = new POR_LOT_FIELDS();
                string lotKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                WhereConditions wc = new WhereConditions(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                //detail deal
                sql = DatabaseTable.BuildUpdateSqlStatement(porLotFields, htParams, wc);
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
            }
        }
        /// <summary>
        /// 更新批次自定义属性值数据。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dsParams">包含批次基础数据的数据集对象。</param>
        public static void UpdateLotUDAs(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string udaSql = "";
            string udaDeleteSql = "";

            if (dsParams.Tables.Contains("MAIN_DATA"))
            {
                DataTable dtParams = dsParams.Tables["MAIN_DATA"];
                Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                //initialize tablefields
                POR_LOT_FIELDS porLotFields = new POR_LOT_FIELDS();
                POR_LOT_ATTR_FIELDS lotUda = new POR_LOT_ATTR_FIELDS();
                string lotKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                WhereConditions wc = new WhereConditions(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                //add uda
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_UDAS))
                {
                    //delete uda
                    udaDeleteSql = string.Format("DELETE FROM {0} WHERE LOT_KEY='{1}'",
                                                  POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME,
                                                  lotKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbtran, CommandType.Text, udaDeleteSql);

                    DataTable lotUdaTable = dsParams.Tables[TRANS_TABLES.TABLE_UDAS];
                    for (int i = 0; i < lotUdaTable.Rows.Count; i++)
                    {
                        Hashtable fields = new Hashtable()
                             {
                                 {POR_LOT_ATTR_FIELDS.FIELD_LOT_KEY, htParams[POR_LOT_ATTR_FIELDS.FIELD_LOT_KEY]},
                                 {POR_LOT_ATTR_FIELDS.FIELD_ATTRIBUTE_KEY, lotUdaTable.Rows[i][BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]},
                                 {POR_LOT_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME, lotUdaTable.Rows[i][BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]},
                                 {POR_LOT_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE, lotUdaTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]},
                                 {POR_LOT_ATTR_FIELDS.FIELD_EDITOR, ""},//lotUdaTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_EDITOR]
                                 {POR_LOT_ATTR_FIELDS.FIELD_EDIT_TIME, null},//lotUdaTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_EDIT_TIME]
                                 {POR_LOT_ATTR_FIELDS.FIELD_EDIT_TIMEZONE,""}//lotUdaTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE_KEY]
                             };
                        udaSql = DatabaseTable.BuildInsertSqlStatement(lotUda, fields, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, udaSql);
                    }
                }
            }
        }
        /// <summary>
        /// 更新工单剩余数量和序号。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据库事务对象。</param>
        /// <param name="dsParams">包含批次基础数据的数据集对象。</param>
        public static void UpdateWorkLeftQuantityAndSequence(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (dsParams.Tables.Contains("MAIN_DATA"))
            {
                DataTable dtParams = dsParams.Tables["MAIN_DATA"];
                Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                string workOrderKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                //get sequence
                string sql = string.Format("SELECT NEXT_SEQ FROM POR_WORK_ORDER WHERE WORK_ORDER_KEY='{0}'",
                                            workOrderKey.PreventSQLInjection());
                string sequence = db.ExecuteDataSet(dbtran, CommandType.Text, sql).Tables[0].Rows[0]["NEXT_SEQ"].ToString();
                string quantity = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_QUANTITY]);
                sql = string.Format(@"UPDATE POR_WORK_ORDER 
                                    SET QUANTITY_LEFT= QUANTITY_LEFT - {0}, NEXT_SEQ=  '{1}'
                                    WHERE WORK_ORDER_KEY='{2}'",
                                    quantity.PreventSQLInjection(),
                                    (Convert.ToInt32(sequence) + 1).ToString("000"),
                                    workOrderKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
            }
        }


        /// <summary>
        /// 创建批次。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dsParams">包含批次创建信息的数据集对象。</param>
        /// <param name="drLotCell">创批组件对应的。</param>
        public static void CreateNewLot(Database db, DataSet dsParams, DataRow drCellData)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();
            DateTime curTime = UtilHelper.GetSysdate(db);
            string lotEditTime = curTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
            {
                DataTable dtMainData = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                DataTable dtAddtionData = dsParams.Tables[1];   //存放附加数据

                Hashtable htMainData = CommonUtils.ConvertToHashtable(dtMainData);
                Hashtable htAddtionData = CommonUtils.ConvertToHashtable(dtAddtionData);
                string enterpriseName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME]);
                string routeName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME]);
                string stepName = Convert.ToString(htAddtionData[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME]);

                string editor = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_CREATOR]);
                string editTimezone = string.Empty;
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE))
                {
                    editTimezone = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE]);
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_EDITOR) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_EDITOR, editor);
                }
                string lotKey = UtilHelper.GenerateNewKey(0);
                htMainData[POR_LOT_FIELDS.FIELD_LOT_KEY] = lotKey;
                htMainData[POR_LOT_FIELDS.FIELD_EDIT_TIME] = null;
                htMainData[POR_LOT_FIELDS.FIELD_CREATE_TIME] = null;
                htMainData[POR_LOT_FIELDS.FIELD_START_WAIT_TIME] = null;

                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO, htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                }
                else
                {
                    htMainData[POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO] = htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO];
                }

                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_STATE_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_REWORKED) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_REWORKED, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_MAIN_LOT, "1");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_HOLD_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_IS_SPLITED) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_IS_SPLITED, "0");
                }
                if (htMainData.ContainsKey(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG) == false)
                {
                    htMainData.Add(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG, "0");
                }
                //创建批次。
                POR_LOT_FIELDS porLotFields = new POR_LOT_FIELDS();
                string sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, htMainData, null);
                db.ExecuteNonQuery(CommandType.Text, sql);

                #region 插入组件片对应电池片信息

                string sqlCommond = string.Empty;

                sqlCommond = string.Format(@"SELECT LOT_KEY FROM POR_LOT_CELL_PARAM
                                            WHERE LOT_NUMBER = '{0}' 
                                            AND IS_USED = 'Y'", drCellData["LOT_NUMBER"].ToString());
                DataTable dtCellReturn = db.ExecuteDataSet(CommandType.Text, sqlCommond).Tables[0];

                if (dtCellReturn.Rows.Count > 0)
                {
                    sqlCommond = string.Format(@"UPDATE  POR_LOT_CELL_PARAM
                                                SET IS_USED = 'N'
                                                WHERE LOT_KEY = '{0}'", dtCellReturn.Rows[0]["LOT_KEY"].ToString());
                    db.ExecuteNonQuery(CommandType.Text, sqlCommond);
                }

                sqlCommond = string.Format(@"INSERT INTO POR_LOT_CELL_PARAM (  [LOT_KEY]
                                                                              ,[LOT_NUMBER]
                                                                              ,[CELLLOT1]
                                                                              ,[CELLPN1]
                                                                              ,[CELLLOT2]
                                                                              ,[CELLPN2]
                                                                              ,[PACKAGEQTY]
                                                                              ,[CELLSUPPLIER]
                                                                              ,[CELLFACTORY]
                                                                              ,[CELLLINE]
                                                                              ,[CELLCOLOR]
                                                                              ,[CELLEFFICIENCY]
                                                                              ,[CELLPOWER]
                                                                              ,[SMALL_PACK_NUMBER]
                                                                              ,[IS_USED]
                                                                              ,[CREATOR]
                                                                              ,[CREATE_TIME]
                                                                              ,[CREATE_TIMEZONE]
                                                                              ,[EDITOR]
                                                                              ,[EDIT_TIME]
                                                                              ,[EDIT_TIMEZONE]
                                                                              ,[ETL_FLAG])
                                                                              VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',
                                                                                      '{12}','{13}','Y','{14}',SYSDATETIME(),'{15}','{16}',SYSDATETIME(),'{17}','Y')",
                                                                               lotKey,
                                                                               drCellData["LOT_NUMBER"].ToString(),
                                                                               drCellData["CELLLOT1"].ToString(),
                                                                               drCellData["CELLPN1"].ToString(),
                                                                               drCellData["CELLLOT2"].ToString(),
                                                                               drCellData["CELLPN2"].ToString(),
                                                                               drCellData["PACKAGEQTY"].ToString(),
                                                                               drCellData["CELLSUPPLIER"].ToString(),
                                                                               drCellData["CELLFACTORY"].ToString(),
                                                                               drCellData["CELLLINE"].ToString(),
                                                                               drCellData["CELLCOLOR"].ToString(),
                                                                               drCellData["CELLEFFICIENCY"].ToString(),
                                                                               drCellData["CELLPOWER"].ToString(),
                                                                               drCellData["SMALL_PACK_NUMBER"].ToString(),
                                                                               editor,
                                                                               editTimezone,
                                                                               editor,
                                                                               editTimezone
                );


                db.ExecuteNonQuery(CommandType.Text, sqlCommond);
                #endregion


                #region 插入操作记录
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                Hashtable htTransaction = new Hashtable();
                DataTable dtTransaction = new DataTable();
                string strChildTransKey = UtilHelper.GenerateNewKey(0);
                string strShiftKey = UtilHelper.GetShiftKey(db, curTime.ToString("yyyy-MM-dd HH:mm:ss"));
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, htMainData[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, htMainData[POR_LOT_FIELDS.FIELD_QUANTITY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, htMainData[POR_LOT_FIELDS.FIELD_QUANTITY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, htMainData[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htMainData[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, strShiftKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htMainData[POR_LOT_FIELDS.FIELD_SHIFT_NAME]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, htMainData[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, htMainData[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, htMainData[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, curTime);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, editor);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, htMainData[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
                db.ExecuteNonQuery(CommandType.Text, sql);
                #endregion

                if (dsParams.Tables.Contains(POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                           new POR_LOT_ATTR_FIELDS(),
                                                           dsParams.Tables[POR_LOT_ATTR_FIELDS.DATABASE_TABLE_NAME],
                                                           new Dictionary<string, string>() 
                                                           {  
                                                                {COMMON_FIELDS.FIELD_COMMON_EDIT_TIME,null},
                                                                {COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, "CN-ZH"}                                                                      
                                                           },
                                                           new List<string>() { COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE });
                }
                foreach (string s in sqlCommandList)
                {
                    db.ExecuteNonQuery(CommandType.Text, s);
                }
            }
        }
    }
}
