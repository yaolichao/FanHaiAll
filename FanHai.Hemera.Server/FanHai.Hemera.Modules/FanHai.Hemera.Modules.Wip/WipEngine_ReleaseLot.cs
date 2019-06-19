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
using SolarViewer.Hemera.Modules.EMS;

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 释放批次。
        /// </summary>
        /// <param name="dataset">包含释放批次数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet ReleaseLot(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                //Create Transaction  
                dbTran = dbConn.BeginTransaction();
                #region CheckExpired
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, htParams[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME].ToString()))
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                        return dsReturn;
                    }
                }
                #endregion
                WipManagement.ReleaseLot(db, dbTran, dsParams);
                dbTran.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ReleaseLot Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
            }

            DateTime endTime = DateTime.Now;
            LogService.LogInfo("ReleaseLot Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
    }
}
