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
using FanHai.Hemera.Utils.DatabaseHelper;

namespace FanHai.Hemera.Modules.Wip
{
    public class HighOfSFinishedProductsEngine : AbstractEngine, IHighOfSFinishedProductsEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public HighOfSFinishedProductsEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public override void Initialize()
        {

        }

        #region IWarehouseEngine 通过序列号查看高位检的批次信息

        public DataSet GetHighInfByLotNum(string lotNum)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@" select LOT_NUMBER,GWJ_POSITION,GWJ_DETAIL,EDITOR,CREATOR,GWJ_VERSION+1  AS GWJ_VERSION from dbo.POR_GWJ_BULIANG
                                                    where LOT_NUMBER ='{0}'
                                                    and GWJSTATUS = 1", lotNum);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetHighInfByLotNum Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion


        #region IHighOfSFinishedProductsEngine 成员


        public DataSet InsertIntoGWJ(DataTable dtData,string lotNumber)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlInsert = @" INSERT INTO POR_GWJ_BULIANG(GWJ_DETAIL_KEY,LOT_NUMBER,GWJ_POSITION,GWJ_DETAIL,GWJ_VERSION,EDITOR,CREATOR,GWJSTATUS) 
                                        VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',1) ";

                string sqlUpdateStatus = @"UPDATE dbo.POR_GWJ_BULIANG SET  GWJSTATUS= 0 WHERE LOT_NUMBER = '{0}'";
               
                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                string strUpdate = string.Format(sqlUpdateStatus, lotNumber);
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                foreach (DataRow dr in dtData.Rows)
                {
                    string keyPo = UtilHelper.GenerateNewKey(0);
                    string lotnumber = dr["LOT_NUMBER"].ToString().Trim().PreventSQLInjection();
                    string position = dr["GWJ_POSITION"].ToString().Trim().PreventSQLInjection();
                    string detail = dr["GWJ_DETAIL"].ToString().Trim().PreventSQLInjection();
                    string editor = dr["EDITOR"].ToString().Trim().PreventSQLInjection();
                    string creater = dr["CREATOR"].ToString().Trim().PreventSQLInjection();
                    int version =int.Parse(dr["GWJ_VERSION"].ToString().Trim().PreventSQLInjection());

                    string sqlDetail = string.Format(sqlInsert,
                                                    keyPo,             //主键
                                                    lotnumber,
                                                    position,
                                                    detail,
                                                    version,
                                                    editor,
                                                    creater
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("InsertIntoGWJ Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion
    }
}
