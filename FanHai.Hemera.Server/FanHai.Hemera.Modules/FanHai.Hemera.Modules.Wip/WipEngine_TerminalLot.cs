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

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 批次终结操作。
        /// </summary>
        /// <param name="dsParams">包含批次终结数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet TerminalLot(DataSet dsParams)
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
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string lotKey = htParams[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                    string strEditTime = htParams[POR_LOT_FIELDS.FIELD_EDIT_TIME].ToString();

                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                    List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                    listCondition.Add(kvp);
                    if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, strEditTime))
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                        return dsReturn;
                    }
                }
                #endregion

                WipManagement.TerminalLot(db, dbTran, dsParams);
                dbTran.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("TerminalLot Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
                dbTran.Dispose();
                dbConn.Dispose();
            }

            DateTime endTime = DateTime.Now;
            LogService.LogInfo("TerminalLot Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
    }
}
