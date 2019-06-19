//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-09-13            重构 迁移到SQL Server数据库
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Share.Constants;
using System.Data.Common;
using System.Collections;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Modules.WipJob;
using SolarViewer.Hemera.Modules.FMM;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 自动合批操作。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbtran">数据操作的事务对象。</param>
        /// <param name="lotKeyFrom">需要合批的批次主键。</param>
        /// <param name="workOrderKeyFrom">工单主键。</param>
        /// <param name="stepKeyFrom">工步主键</param>
        /// <param name="lineKeyFrom">线别主键</param>
        /// <param name="quantityFrom">数量</param>
        /// <param name="maxBoxQuantity">最大箱数量</param>
        /// <param name="stateFlag">批次状态标记</param>
        /// <param name="editor">编辑人</param>
        /// <param name="isRework">是否重工</param>
        /// <param name="oprLine">操作线别</param>
        /// <param name="shiftName">操作班别</param>
        public static void AutoMerge(Database db, 
                                DbTransaction dbtran, 
                                string lotKeyFrom, 
                                string workOrderKeyFrom,
                                string stepKeyFrom, 
                                string lineKeyFrom, 
                                string quantityFrom, 
                                int maxBoxQuantity, 
                                int stateFlag,
                                string editor,
                                bool isRework,
                                string oprLine, 
                                string shiftName)
        {
            string lotKey = "", quantity = "", enterpriseKey = "", routeKey = "", stepKey = "";
            int totalQuantity = 0;
            int mergedQuantity = 0;
            int leftQuantity = 0;
            string sql = "", strParentTransKey = "";
            DataSet dsMergeLot = new DataSet();

            //获取批次主键不等于指定批次，但工单为指定工单，工步为指定工步，线别为指定线别，状态为指定状态的批次信息。
            sql = string.Format(@"SELECT A.LOT_KEY,A.QUANTITY,A.ROUTE_ENTERPRISE_VER_KEY,A.CUR_ROUTE_VER_KEY,A.CUR_STEP_VER_KEY
                                FROM POR_LOT A
                                LEFT  JOIN FMM_PRODUCTION_LINE B ON A.LINE_NAME=B.LINE_NAME
                                WHERE A.LOT_KEY !='{0}' 
                                AND A.WORK_ORDER_KEY='{1}'
                                AND A.CUR_STEP_VER_KEY='{2}'
                                AND B.PRODUCTION_LINE_KEY='{3}' 
                                AND A.STATUS = 1 AND A.QUANTITY != 0 
                                AND A.DELETED_TERM_FLAG = 0 
                                AND A.HOLD_FLAG=0 
                                AND A.STATE_FLAG={4}",
                                lotKeyFrom.PreventSQLInjection(),
                                workOrderKeyFrom.PreventSQLInjection(),
                                stepKeyFrom.PreventSQLInjection(),
                                lineKeyFrom.PreventSQLInjection(), stateFlag);
            //如果指定了最大箱数量，则查询条件增加数量小于最大箱数量。
            if (maxBoxQuantity != -1)
            {
                sql = sql + " AND A.QUANTITY<" + maxBoxQuantity + "";
            }
            //如果是重工，则查询条件增加重工>0   
            if (isRework)
            {
                sql = sql + " AND A.REWORK_FLAG > 0";
            }
            dsMergeLot = db.ExecuteDataSet(CommandType.Text, sql);

            //查询得到的数据集中的数据表个数>0。
            if (dsMergeLot.Tables.Count > 0)
            {
                //可以合并的批次的记录大于0。
                if (dsMergeLot.Tables[0].Rows.Count > 0)
                {
                    lotKey = dsMergeLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();                             //批次主键
                    quantity = dsMergeLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString();                          //数量
                    enterpriseKey = dsMergeLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();     //流程组主键
                    routeKey = dsMergeLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();                 //流程主键
                    stepKey = dsMergeLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();                   //工步主键
                    string time = UtilHelper.GetSysdate(db).ToString("yyyy-MM-dd HH:mm:ss");                                    //当前时间
                    //合并的批次数量>0 并且 被合并的批次数量>0
                    if (quantity.Length > 0 && quantityFrom.Length > 0)
                    {
                        totalQuantity = Convert.ToInt32(quantity) + Convert.ToInt32(quantityFrom);
                        #region 更新合批批次的数量
                        //如果最大箱数量不等于 -1 并且总数量>最大箱数量。
                        if (maxBoxQuantity != -1 & totalQuantity > maxBoxQuantity)
                        {
                            #region 更新合并到的批次数量为最大箱数量。
                            sql = @"UPDATE POR_LOT SET QUANTITY=" + maxBoxQuantity + "," +
                                  "EDITOR='" + editor.PreventSQLInjection() + "'," +
                                  "EDIT_TIME=GETDATE() " +
                                  "WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            #endregion

                            #region 更新被合并的批次数量为剩余数量。
                            leftQuantity = totalQuantity - maxBoxQuantity;
                            mergedQuantity = Convert.ToInt32(quantityFrom) - leftQuantity;
                            sql = @"UPDATE POR_LOT SET QUANTITY=" + leftQuantity + "," +
                                    "EDITOR='" + editor.PreventSQLInjection() + "'," +
                                    "EDIT_TIME=GETDATE() " +
                                    "WHERE LOT_KEY='" + lotKeyFrom.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            #endregion

                        }
                        else
                        {
                            #region 更新合并到的批次数量为总数量。
                            sql = @"UPDATE POR_LOT SET QUANTITY=" + totalQuantity + "," +
                                 "EDITOR='" + editor.PreventSQLInjection() + "'," +
                                 "EDIT_TIME=GETDATE()" +
                                 "WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            #endregion

                            #region 更新被合并的批次数量为0。删除标记为1。
                            mergedQuantity = Convert.ToInt32(quantityFrom);
                            sql = @"UPDATE POR_LOT SET QUANTITY='0',DELETED_TERM_FLAG='1',
                                    EDITOR='" + editor.PreventSQLInjection() + "'," +
                                   "EDIT_TIME=GETDATE() " +
                                   "WHERE LOT_KEY='" + lotKeyFrom.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            #endregion
                        }
                        #endregion

                        #region 插入一笔【合并到批次】的操作记录。
                        WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                        Hashtable parenttransactionTable = new Hashtable();
                        DataTable parenttransaction = new DataTable();
                        strParentTransKey = UtilHelper.GenerateNewKey(0);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, quantity);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, totalQuantity);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGE);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "");
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, "");
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, "CN-ZH");
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, time);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKeyFrom);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKeyFrom);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag.ToString());
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                        parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipFields, parenttransactionTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        #endregion

                        #region 插入一笔【被合并批次】的操作记录。
                        Hashtable childtransactionTable = new Hashtable();
                        DataTable childtransaction = new DataTable();
                        string strChildTransKey = UtilHelper.GenerateNewKey(0);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strChildTransKey);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKeyFrom);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, mergedQuantity.ToString());
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftQuantity.ToString());
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_MERGED);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, "");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, "");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, "CN-ZH");
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, time);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKeyFrom);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKeyFrom);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag.ToString());
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                        childtransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, oprLine);
                        sql = DatabaseTable.BuildInsertSqlStatement(wipFields, childtransactionTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        #endregion

                        #region 插入一笔批次合批的操作记录。
                        Hashtable MergeHashTable = new Hashtable();
                        WIP_MERGE_FIELDS wipMerge = new WIP_MERGE_FIELDS();

                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_CHILD_LOT_KEY, lotKeyFrom);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_MAIN_LOT_KEY, lotKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_MERGE_QUANTITY, mergedQuantity.ToString());
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_STEP_KEY, stepKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_ROUTE_KEY, routeKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDITOR, "");
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDIT_TIME, time);
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH");
                        MergeHashTable.Add(WIP_MERGE_FIELDS.FIELD_CHILD_TRANSACTION_KEY, strChildTransKey);

                        sql = DatabaseTable.BuildInsertSqlStatement(wipMerge, MergeHashTable, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        #endregion

                    }
                }
            }
        }
        
    }
}
