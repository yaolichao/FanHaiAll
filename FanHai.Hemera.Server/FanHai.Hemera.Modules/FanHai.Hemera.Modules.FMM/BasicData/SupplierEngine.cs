using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;

namespace FanHai.Hemera.Modules.FMM
{
    public class SupplierEngine : AbstractEngine, ISupplierEngine
    {
        private Database db = null;

        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        /// <summary>
        /// constructor
        /// </summary>
        public SupplierEngine()
        {
            //ceate db
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 获取供应商代码
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        public DataSet GetSupplierCode()
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT * FROM BASE_SUPPLIER ORDER BY CODE desc ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStore Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取供应商代码
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        public DataSet GetSupplierCode(string strSupplierName, string strSupplierCode)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT * FROM BASE_SUPPLIER WHERE 1=1";
                if (!strSupplierCode.Equals(string.Empty))
                {
                    sql += " AND CODE='" + strSupplierCode + "'";
                    sql = string.Format(sql, strSupplierCode);
                }
                if (!strSupplierName.Equals(string.Empty))
                {
                    sql += " AND NAME like '%" + strSupplierName + "%'";
                }
                sql += " ORDER BY CODE DESC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStore Error: " + ex.Message);
            }
            return dsReturn;
        }


        //#region ISupplierEngine 成员

        public DataSet SupplierCodeInsert(DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                if (null != dataSet)
                {
                    sql = @" INSERT INTO BASE_SUPPLIER
                                            (CODE,NAME ,CREATE_TIME ,
                                            CREATOR, EDIT_TIME,EDITOR,NICKNAME,CREATE_TIMEZONE)
                                            VALUES('{0}','{1}',GETDATE(),'{2}',GETDATE(),'{3}','{4}','CN-ZH')";
                    string sql2 = string.Format(sql,
                        dataSet.Tables[0].Rows[0]["CODE"].ToString(),
                        dataSet.Tables[0].Rows[0]["NAME"].ToString(),
                        dataSet.Tables[0].Rows[0]["CREATOR"].ToString(),
                        dataSet.Tables[0].Rows[0]["CREATOR"].ToString(),
                        dataSet.Tables[0].Rows[0]["NICKNAME"].ToString());
                    db.ExecuteNonQuery(CommandType.Text, sql2);

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
                LogService.LogError("ReasonCodeInsert Error: " + ex.Message);
            }
            return retDS;
        }

        public DataSet SupplierCodeUpdate(DataSet dataSet,string lblCode)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                if (null != dataSet)
                {
                    sql = @" UPDATE BASE_SUPPLIER SET CODE = '{0}',NAME = '{1}',NICKNAME = '{2}',EDITOR='{3}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='CN-ZH' WHERE CODE = '{4}' ";
                    string sql2 = string.Format(sql,
                        dataSet.Tables[0].Rows[0]["CODE"].ToString(),
                        dataSet.Tables[0].Rows[0]["NAME"].ToString(),
                        dataSet.Tables[0].Rows[0]["NICKNAME"].ToString(),
                        dataSet.Tables[0].Rows[0]["CREATOR"].ToString(),
                        lblCode
                        );
                    db.ExecuteNonQuery(CommandType.Text, sql2);

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
                LogService.LogError("ReasonCodeInsert Error: " + ex.Message);
            }
            return retDS;
        }

        public DataSet DeleteSupplierCode(string supplierCode)
        {
            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sql 
            String[] sqlCommand = new String[5];

            try
            {
                if (supplierCode != string.Empty)
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            sqlCommand[0] = "DELETE BASE_SUPPLIER WHERE CODE = '" + supplierCode + "'";

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand[0]);

                            //Commit Transaction
                            dbTran.Commit();

                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("DeleteReasonCode Error: " + ex.Message);
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
                LogService.LogError("DeleteReasonCode Error: " + ex.Message);
            }

            return dataDs;
        }

        //#endregion

    }
}
