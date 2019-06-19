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
    public class BomMaterialBandEngine : AbstractEngine, IBomMaterialBandEngine
    {
        private Database db = null;

        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        /// <summary>
        /// constructor
        /// </summary>
        public BomMaterialBandEngine()
        {
            //ceate db
            db = DatabaseFactory.CreateDatabase();
        }


        #region IBomMaterialBandEngine 成员
        /// <summary>
        /// 根据物料料号和代码查询结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="barCode"></param>
        /// <returns>结果数据集</returns>
        public DataSet GetMaterialDateByCodeAndBarcode(string code, string barCode)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT * FROM POR_MATERIAL WHERE 1=1 AND STATUS = 1 ";
                if (!string.IsNullOrEmpty(code))
                {
                    sql += string.Format(" AND MATERIAL_CODE LIKE '{0}%'", code);
                }
                if (!string.IsNullOrEmpty(barCode))
                {
                    sql += string.Format(" AND BARCODE LIKE '{0}%'", barCode);
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaterialDateByCodeAndBarcode Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IBomMaterialBandEngine 成员

        /// <summary>
        /// 获取工单BOM中的所有料号
        /// </summary>
        /// <returns></returns>
        public DataSet GetBomMaterial()
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = @"SELECT DISTINCT MATERIAL_CODE FROM POR_WORK_ORDER_BOM ";
               
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBomMaterial Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IBomMaterialBandEngine 成员


        public DataSet DeleteMaterialCode(string _code)
        {
            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sql 
            String[] sqlCommand = new String[5];

            try
            {
                if (_code != string.Empty)
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            string sql01 = string.Format("SELECT MAX(MATERIAL_VERSION) FROM POR_MATERIAL WHERE MATERIAL_CODE = '{0}'",
                                                         _code);
                            DataSet ds01 = db.ExecuteDataSet(CommandType.Text, sql01);
                            int materialVersion = 1;
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                materialVersion = Convert.ToInt32(ds01.Tables[0].Rows[0][0].ToString()) ;
                            }
                            sqlCommand[0] = "UPDATE POR_MATERIAL SET STATUS = 0 ,MATERIAL_VERSION = '" + materialVersion + "' WHERE MATERIAL_CODE = '" + _code + "' AND STATUS = 1";

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
                            LogService.LogError("DeleteMaterialCode Error: " + ex.Message);
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
                LogService.LogError("DeleteMaterialCode Error: " + ex.Message);
            }

            return dataDs;
        }

        #endregion

        #region IBomMaterialBandEngine 成员


        public DataSet MaterialDateInsert(DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                string key = UtilHelper.GenerateNewKey(0);
                if (null != dataSet)
                {
                    string sql01 = string.Format("SELECT MAX(MATERIAL_VERSION) FROM POR_MATERIAL WHERE MATERIAL_CODE = '{0}'",
                                                    dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString());
                    DataSet ds01 = db.ExecuteDataSet(CommandType.Text, sql01);
                    int materialVersion = 1;
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        materialVersion = Convert.ToInt32(ds01.Tables[0].Rows[0][0].ToString()) + 1;
                    }

                    sql = @" INSERT INTO POR_MATERIAL
                                            (MATERIAL_KEY,MATERIAL_NAME ,MATERIAL_CODE ,
                                            MATERIAL_SPEC, BARCODE,CREATOR,CREATE_TIME,CREATE_TIMEZONE,MATERIAL_VERSION)
                                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}',GETDATE(),'CN-ZH','{6}')";
                    string sql2 = string.Format(sql,
                        key,
                        dataSet.Tables[0].Rows[0]["MATERIAL_NAME"].ToString(),
                        dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString(),
                        dataSet.Tables[0].Rows[0]["MATERIAL_SPEC"].ToString(),
                        dataSet.Tables[0].Rows[0]["BARCODE"].ToString(),
                        dataSet.Tables[0].Rows[0]["CREATOR"].ToString(),
                        materialVersion);
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
                LogService.LogError("MaterialDateInsert Error: " + ex.Message);
            }
            return retDS;
        }

        public DataSet MaterialDateUpdate(DataSet dataSet)
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
                    string sql01 = string.Format("SELECT MAX(MATERIAL_VERSION) FROM POR_MATERIAL WHERE MATERIAL_CODE = '{0}'",
                                                    dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString());
                    DataSet ds01 = db.ExecuteDataSet(CommandType.Text, sql01);
                    int materialVersion = 1;
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        materialVersion =Convert.ToInt32(ds01.Tables[0].Rows[0][0].ToString()) + 1;
                    }

                    sql = string.Format(@" UPDATE POR_MATERIAL SET STATUS = 0,EDITOR = '{0}',
                                            EDIT_TIME = GETDATE(), EDIT_TIMEZONE = 'CN-ZH'
                                            WHERE MATERIAL_CODE = '{1}' AND STATUS = 1 ", 
                                            dataSet.Tables[0].Rows[0]["EDITOR"].ToString(), 
                                            dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString());
                    db.ExecuteNonQuery(CommandType.Text, sql);

                    string sql02 = @" INSERT INTO POR_MATERIAL
                                            (MATERIAL_KEY,MATERIAL_NAME ,MATERIAL_CODE,
                                            MATERIAL_SPEC, BARCODE,CREATOR,CREATE_TIME,CREATE_TIMEZONE,MATERIAL_VERSION,
                                            EDITOR,EDIT_TIME,EDIT_TIMEZONE)
                                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}',GETDATE(),'CN-ZH','{6}','{7}',GETDATE(),'CN-ZH')";
                    string key = UtilHelper.GenerateNewKey(0);
                    string sql2 = string.Format(sql02,
                        key,
                        dataSet.Tables[0].Rows[0]["MATERIAL_NAME"].ToString(),
                        dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString(),
                        dataSet.Tables[0].Rows[0]["MATERIAL_SPEC"].ToString(),
                        dataSet.Tables[0].Rows[0]["BARCODE"].ToString(),
                        dataSet.Tables[0].Rows[0]["EDITOR"].ToString(),
                        materialVersion,
                        dataSet.Tables[0].Rows[0]["EDITOR"].ToString());
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
                LogService.LogError("MaterialDateUpdate Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion
    }
}
