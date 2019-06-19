using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using System.Collections;

namespace FanHai.Hemera.Modules.FMM
{
    public class MaterialBuckleControlEngine : AbstractEngine, IMaterialBuckleControlEngine 
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public MaterialBuckleControlEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        public override void Initialize(){}


        #region IMaterialBuckleControlEngine 成员

        public DataSet GetParameter()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT PARAM_KEY,PARAM_NAME FROM dbo.BASE_PARAMETER
                                      WHERE STATUS = 1";
                
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetParameter Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        public DataSet GetInfByParameter(string parameter)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT row_number() over (order by PARAMETER) as ROWNUMBER,* FROM dbo.WST_STORE_MATERIAL_BUCKLE_CONTROL
                                        WHERE STATUS = 1";
                if (!parameter.Equals("ALL"))
                    sql += string.Format(" AND PARAMETER = '{0}'", parameter);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetInfByParameter Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        public DataSet InsertNewInf(string parameter, string useqty, string useunit, string conrtastQty, string conrtastUnt, string name)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlKo = @" INSERT INTO dbo.WST_STORE_MATERIAL_BUCKLE_CONTROL
                                    (PARAMETER_KEY,PARAMETER,USE_QTY,USE_UNIT,
                                    USE_CONRTAST_QTY,USE_CONRTAST_UNIT,CREATOR,
                                    CREATER_TIME,STATUS) 
                                    VALUES
                                    ('{0}','{1}',{2},'{3}',{4},'{5}','{6}',GETDATE(),1) ";
                string check = string.Format(@"SELECT COUNT(*) AS COUNT FROM WST_STORE_MATERIAL_BUCKLE_CONTROL
                                                      WHERE PARAMETER = '{0}' AND STATUS = 1", parameter);
                if (Convert.ToInt32(db.ExecuteDataSet(CommandType.Text, check).Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "已经存在该参数的配置信息！请确认");
                }
                else
                {
                    ///sql执行start-------------------------------------------------------------------------------------------------------------------
                    //插入抬头表信息
                    string strInsert = string.Format(sqlKo,
                                                           UtilHelper.GenerateNewKey(0),                                            //主键
                                                           parameter,
                                                           useqty,
                                                           useunit,
                                                           conrtastQty,
                                                           conrtastUnt,
                                                           name
                      );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, strInsert);
                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("InsertNewInf Error: " + ex.Message);
            }
            return retDS;
        }

        public DataSet DeleteInf(string parameter)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlDelete = @"UPDATE WST_STORE_MATERIAL_BUCKLE_CONTROL SET
                                        STATUS = 0 WHERE PARAMETER = '{0}' AND STATUS = 1";

                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                //插入抬头表信息
                string strUpdate = string.Format(sqlDelete,
                                                       parameter
                  );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("DeleteInf Error: " + ex.Message);
            }
            return retDS;
        }

        public DataSet UpdateParameterInf(string parameter, string useqty, string useunit, string conrtastQty, string conrtastUnt, string name)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlKo = @"UPDATE WST_STORE_MATERIAL_BUCKLE_CONTROL SET
                                        USE_QTY = {0},USE_UNIT ='{1}',
                                        USE_CONRTAST_QTY = {2},USE_CONRTAST_UNIT = '{3}',EDITOR = '{4}',
                                        EDITER_TIME = GETDATE() WHERE PARAMETER = '{5}' AND STATUS = 1";

                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                //插入抬头表信息
                string strUpdate = string.Format(sqlKo,
                                                       useqty,
                                                       useunit,
                                                       conrtastQty,
                                                       conrtastUnt,
                                                       name,
                                                       parameter
                  );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateParameterInf Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion
    }
}
