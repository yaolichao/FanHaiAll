using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface.WarehouseManagement;
using System.Data;
using FanHai.Hemera.Share.Constants;
using System.Collections;

namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 批次退料数据查询类。
    /// </summary>
    public class ReturnMaterialQueryEngine : AbstractEngine, IReturnMaterialQueryEngine
    {
        private Database db = null;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReturnMaterialQueryEngine()
        {
            db = DatabaseFactory.CreateDatabase();//实例化对象
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
        }
        /// <summary>
        /// 获取批次退料信息。
        /// </summary>
        /// <returns>包含批次退料信息的数据集对象。</returns>
        public DataSet GetReturnMaterial()
        {
            DataSet dsReturn = null;
            try
            {
                string strsql = @"SELECT T1.EDIT_TIME, T4.MATERIAL_CODE, T4.MATERIAL_LOT, T1.REASON_CODE_NAME,T1.RETURN_QUANTITY, 
	                            T5.ROUTE_STEP_NAME, T3.SHIFT_NAME, T4.LOT_NUMBER,T2.USERNAME, T1.EDITOR
                            FROM WIP_RETURN_MAT T1 
                            LEFT JOIN RBAC_USER T2 ON T1.EDITOR = T2.BADGE
                            LEFT JOIN WIP_TRANSACTION T3 ON T3.TRANSACTION_KEY = T1.TRANSACTION_KEY
                            LEFT JOIN POR_LOT T4 ON T4.LOT_KEY = T3.PIECE_KEY
                            LEFT JOIN POR_ROUTE_STEP T5 ON T5.ROUTE_STEP_KEY = T1.STEP_KEY
                            ORDER BY T1.EDIT_TIME DESC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetReturnMaterial Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取批次退料信息。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集对象。
        /// ---------------------------
        /// {START_TIME}
        /// {END_TIME}
        /// ---------------------------
        /// </param>
        /// <returns>包含批次退料信息的数据集对象。</returns>
        public DataSet GetReturnMaterial(DataSet dsSearch)
        {
            DataSet dsReturn = null;
            string strsql = string.Empty;
            try
            {
                strsql = @"SELECT T1.EDIT_TIME, T4.MATERIAL_CODE, T4.MATERIAL_LOT, T1.REASON_CODE_NAME,
                                   T1.RETURN_QUANTITY, T5.ROUTE_STEP_NAME, T3.SHIFT_NAME, T4.LOT_NUMBER,
                                   T2.USERNAME, T1.EDITOR
                            FROM WIP_RETURN_MAT T1 
                            LEFT JOIN RBAC_USER T2 ON T1.EDITOR = T2.BADGE
                            LEFT JOIN WIP_TRANSACTION T3 ON T3.TRANSACTION_KEY = T1.TRANSACTION_KEY
                            LEFT JOIN POR_LOT T4 ON T4.LOT_KEY = T3.PIECE_KEY
                            LEFT JOIN POR_ROUTE_STEP T5 ON T5.ROUTE_STEP_KEY = T1.STEP_KEY 
                            WHERE 1=1 ";           
                if (dsSearch != null
                    && dsSearch.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA) 
                    && dsSearch.Tables[TRANS_TABLES.TABLE_MAIN_DATA].Rows.Count>0)
                {
                    DataTable dtSearch = dsSearch.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htSearch =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtSearch);
                    //哈希表中包含批次编码键值。
                    if (htSearch.ContainsKey("START_TIME"))
                    {
                        string startTime = Convert.ToString(htSearch["START_TIME"]);
                        if (!string.IsNullOrEmpty(startTime))
                        {
                            strsql += string.Format(@" AND T1.EDIT_TIME>='{0}'", startTime.PreventSQLInjection());
                        }
                    }
                    //哈希表中包含批次状态的键值。
                    if (htSearch.ContainsKey("END_TIME"))
                    {
                        string endTime = Convert.ToString(htSearch["END_TIME"]);
                        if (!string.IsNullOrEmpty(endTime))
                        {
                            strsql += string.Format(@" AND T1.EDIT_TIME< '{0}'",endTime.PreventSQLInjection());
                        }
                    }
                }
                strsql += " ORDER BY T1.EDIT_TIME DESC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetReturnMaterial Error: " + ex.Message);
            }
            return dsReturn;
        }

    }

}
