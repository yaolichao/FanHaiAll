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
    public class ByProductEngine :AbstractEngine, IByProductEngine 
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        private Database db2 = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ByProductEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            db2 = DatabaseFactory.CreateDatabase("SQLServerAwms");
        }
        #region IByProductEngine 成员

        public System.Data.DataSet GetByProductInf(System.Data.DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = @"SELECT BYP_KEY,MATNR_M,MATNR_B2,MATNR_B3,PTYP3,WERKS,CDATE,CUSER,MDATE,MUSER,WAB.PART_DESC,WAB.B2DESC,C.PART_DESC AS B3DESC
                                        FROM(              
	                                        SELECT BYP_KEY,MATNR_M,MATNR_B2,MATNR_B3,PTYP3,WERKS,CDATE,CUSER,MDATE,MUSER,WA.PART_DESC,B.PART_DESC AS B2DESC
	                                        FROM(
		                                        SELECT BYP_KEY,MATNR_M,MATNR_B2,MATNR_B3,PTYP3,WERKS,CDATE,CUSER,MDATE,MUSER,A.PART_DESC 
		                                         FROM DBO.AWMS_PP_ZMMDBYP W
		                                         LEFT join DBO.POR_PART A
		                                         ON
		                                         W.MATNR_M = A.PART_NAME
                                             )  WA
		                                         LEFT JOIN DBO.POR_PART B
		                                         ON 
		                                         WA.MATNR_B2 = B.PART_NAME
                                        ) WAB
                                        LEFT JOIN DBO.POR_PART C
                                        ON WAB.MATNR_B3 = C.PART_NAME WHERE 1=1";
                DataTable dt = new DataTable();
                dt = dataSet.Tables[0];
                if (dt.Rows.Count >= 1)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTM"].ToString()))
                    {
                        sqlCommon += string.Format(" AND MATNR_M like '{0}%'", dt.Rows[0]["PARTM"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTB2"].ToString()))
                    {
                        sqlCommon += string.Format(" AND MATNR_B2 like '{0}%'", dt.Rows[0]["PARTB2"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTB3"].ToString()))
                    {
                        sqlCommon += string.Format(" AND MATNR_B3 like '{0}%'", dt.Rows[0]["PARTB3"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTTYPE"].ToString()))
                    {
                        sqlCommon += string.Format(" AND PTYP3 ='{0}'", dt.Rows[0]["PARTTYPE"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTCREATER"].ToString()))
                    {
                        sqlCommon += string.Format(" AND CUSER like '{0}%'", dt.Rows[0]["PARTCREATER"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTEDITER"].ToString()))
                    {
                        sqlCommon += string.Format(" AND MUSER like '{0}%'", dt.Rows[0]["PARTEDITER"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTCS"].ToString()))
                    {
                        sqlCommon += string.Format(" AND CDATE>='{0}'", DateTime.Parse(dt.Rows[0]["PARTCS"].ToString()));
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PARTCE"].ToString()))
                    {
                        sqlCommon += string.Format(" AND CDATE<='{0}'", DateTime.Parse(dt.Rows[0]["PARTCE"].ToString()));
                    }
                    //if (!string.IsNullOrEmpty(dt.Rows[0]["PARTES"].ToString()))
                    //{
                    //    sqlCommon += string.Format(" AND MDATE>='{0}'", DateTime.Parse(dt.Rows[0]["PARTES"].ToString()));
                    //}
                    //if (!string.IsNullOrEmpty(dt.Rows[0]["PARTEE"].ToString()))
                    //{
                    //    sqlCommon += string.Format(" AND MDATE<='{0}'", DateTime.Parse(dt.Rows[0]["PARTEE"].ToString()));
                    //}
                }

                dsReturn = db2.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetByProductInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        public System.Data.DataSet GetLotPartInf(string strCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = @"SELECT ROW_NUMBER() over(order by part_key) AS ROWNUM,PART_KEY,PART_ID,PART_NAME,PART_TYPE,PART_DESC 
                                     FROM DBO.POR_PART";
                if (!string.IsNullOrEmpty(strCode))
                {
                    sqlCommon += string.Format(" where PART_NAME like '{0}%'", strCode);
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotPartInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        public System.Data.DataSet ByProductCodeInsert(System.Data.DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet ByProductCodeUpdate(System.Data.DataSet dataSet, string number)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet DeleteByProductCode(string number)
        {
            //get dynamic dataset constructor
            DataSet dataDs = new DataSet();
            //define sql 
            string sqlCommand = string.Empty;

            try
            {
                if (number != string.Empty)
                {
                    using (DbConnection dbConn = db2.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            sqlCommand = "DELETE FROM AWMS_PP_ZMMDBYP WHERE BYP_KEY = '" + number + "'";

                            db2.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            //Commit Transaction
                            dbTran.Commit();

                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("DeleteByProductCode Error: " + ex.Message);
                        }
                        finally
                        {
                            //Close Connection
                            dbConn.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("DeleteByProductCode Error: " + ex.Message);
            }

            return dataDs;
        
        }

        #endregion

        public override void Initialize(){}

        #region IByProductEngine 成员


        public DataSet ProUpdate(DataSet dsSetIn)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                if (null != dsSetIn)
                {
                    sql = @" UPDATE AWMS_PP_ZMMDBYP
                                          SET  MATNR_M= '{0}', MATNR_B2= '{1}',MATNR_B3 = '{2}',
                                              PTYP3= '{3}',MUSER= '{4}', MDATE= GETDATE()
                                          WHERE  BYP_KEY= '{5}'";
                    DataTable dtHash = dsSetIn.Tables["HASH"];
                    Hashtable hsTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);
                    string sql2 = string.Format(sql,
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["MATNR_M"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["MATNR_B2"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["MATNR_B3"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["PTYP3"].ToString(),
                        hsTable["EDITOR"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["BYP_KEY"].ToString());
                    db2.ExecuteNonQuery(CommandType.Text, sql2);

                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);

                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "${res:Global.ServerMessage.M0004}");
                }
            }

            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("ProUpdate Error: " + ex.Message);
            }
            return retDS;
        }

        public DataSet ProIsert(DataSet dsSetIn)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                if (null != dsSetIn)
                {
                    sql = @" INSERT INTO AWMS_PP_ZMMDBYP
                                            (BYP_KEY,MATNR_M,MATNR_B2 ,MATNR_B3 ,
                                            PTYP3, CDATE,CUSER,MDATE,MUSER)
                                            VALUES('{0}','{1}','{2}','{3}','{4}',GETDATE(),'{5}',GETDATE(),'{6}')";
                    string key = UtilHelper.GenerateNewKey(0);
                    DataTable dtHash = dsSetIn.Tables["HASH"];
                    Hashtable hsTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);
                    string sql2 = string.Format(sql,
                        key,
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["MATNR_M"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["MATNR_B2"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["MATNR_B3"].ToString(),
                        dsSetIn.Tables["PP_ZMMDBYP"].Rows[0]["PTYP3"].ToString(),
                        hsTable["CREATOR"].ToString(),
                        hsTable["EDITOR"].ToString());
                    db2.ExecuteNonQuery(CommandType.Text, sql2);

                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);

                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "${res:Global.ServerMessage.M0004}");
                }
            }

            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("ProIsert Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion
    }
}
