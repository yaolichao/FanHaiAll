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

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 进行批次报废操作。
        /// </summary>
        /// <param name="dsParams">
        /// 包含批次报废数据信息的数据集对象。
        /// 必须包含名称为<see cref=" TRANS_TABLES.TABLE_PARAM"/>的数据表，用于存储批次主信息的数据。
        /// 包含名称为<see cref=" WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME"/>的数据表（可选），用于存储批次报废的数据。
        /// </param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet LotLossBonus(DataSet dsParams)
        {
            System.DateTime startTime = System.DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string lotKey = string.Empty, editor = string.Empty, editTimeZone = string.Empty, qty = string.Empty;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                //Create Transaction 
                dbTran = dbConn.BeginTransaction();

                #region CheckExpired 检查记录是否过期。防止重复修改。
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    lotKey = htParams[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                    qty = htParams[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT].ToString();
                    editor = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                    editTimeZone = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();

                    string strEditTime = htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME].ToString();
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                    if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, strEditTime))
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                        return dsReturn;
                    }
                }
                #endregion

                //进行报废数据收集。
                WipManagement.SetLossBonus(db, dbTran, dsParams);

                //更新批次数量。
                string sql = string.Format(@"UPDATE POR_LOT 
                                            SET QUANTITY='{0}',EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
                                            WHERE LOT_KEY='{3}'", 
                                            qty.PreventSQLInjection(),
                                            editor.PreventSQLInjection(),
                                            editTimeZone.PreventSQLInjection(),
                                            lotKey.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotLossBonus Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("LotLossBonus Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
    }
}
