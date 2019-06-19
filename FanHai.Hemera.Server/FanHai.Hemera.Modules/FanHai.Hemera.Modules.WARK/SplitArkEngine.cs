using System.Text;
using System.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;

namespace FanHai.Hemera.Modules.WARK
{
    public class SplitArkEngine : AbstractEngine, ISplitArkEngine
    {
         /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        private Database db2 = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SplitArkEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            db2 = DatabaseFactory.CreateDatabase("SQLServerAwms");
        }
        public override void Initialize()
        {
            
        }

        #region ISplitArkEngine 成员

        public DataSet SplitArk(DataSet ds)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                string sql01 = string.Format(@"SELECT PALLET_NO FROM AWMS_CONTAINER_DETAIL WHERE CONTAINER_KEY = '{0}'"
                                                                    , ds.Tables[0].Rows[0]["CONTAINER_KEY"].ToString());
                DataSet ds01 = db2.ExecuteDataSet(CommandType.Text, sql01);

                string strPlus02 = string.Empty;
                DataTable dt02 = ds01.Tables[0];
                for (int i = 0; i < dt02.Rows.Count; i++)
                {
                    strPlus02 += "'" + dt02.Rows[i]["PALLET_NO"].ToString().Trim() + "',";
                }
                if (string.IsNullOrEmpty(strPlus02))
                {
                    strPlus02 = "'',";
                }
                string str02 = strPlus02.Substring(0, strPlus02.Length - 1);
                string sql02 = string.Format(@"UPDATE WIP_CONSIGNMENT SET ARK_FLAG = '0' WHERE PALLET_NO IN({0})"
                                            , str02);
                db.ExecuteNonQuery(CommandType.Text, sql02);

                string sql03 = string.Format(@"DELETE FROM AWMS_CONTAINER_DETAIL WHERE CONTAINER_KEY = '{0}'"
                                                                    , ds.Tables[0].Rows[0]["CONTAINER_KEY"].ToString());
                db2.ExecuteNonQuery(CommandType.Text, sql03);

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("SplitArk Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion
    }
}
