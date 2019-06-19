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
    public class SpecialMatTeamEngine : AbstractEngine, ISpecialMatTeamEngine
    {
        private Database db = null;

        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        /// <summary>
        /// constructor
        /// </summary>
        public SpecialMatTeamEngine()
        {
            //ceate db
            db = DatabaseFactory.CreateDatabase();
        }



        #region ISpecialMatTeamEngine 成员

        public DataSet GetWorkNumber()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT DISTINCT ORDER_NUMBER FROM dbo.POR_WORK_ORDER
                                                        WHERE  ORDER_STATE IN ('TECO','REL')");

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ISpecialMatTeamEngine 成员


        public DataSet GetMaterialByWorkOrder(string workOrder)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT MATERIAL_CODE,DESCRIPTION FROM dbo.POR_WORK_ORDER_BOM
                                                        WHERE ORDER_NUMBER = '{0}'", workOrder);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ISpecialMatTeamEngine 成员


        public DataSet GetMatSpecialInf(string _workOrder, string _material, string _paramTeam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = @"SELECT * FROM dbo.POR_WORK_ORDER_BOM_EXTENSION
                                                        WHERE STATUS = 1";
                if (!string.IsNullOrEmpty(_workOrder))
                    sqlCommon += string.Format(" AND ORDER_NUMBER LIKE '{0}'",_workOrder);
                if (!string.IsNullOrEmpty(_material))
                    sqlCommon += string.Format(" AND MATERIAL_CODE LIKE '{0}'", _material);
                if (!string.IsNullOrEmpty(_paramTeam))
                    sqlCommon += string.Format(" AND MATKL LIKE '{0}'", _paramTeam);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMatSpecialInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ISpecialMatTeamEngine 成员


        public DataSet DeleteMatSpecialInf(string _workOrderNum, string _mat, string _paramerTeam)
        {
             //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sql 
            String[] sqlCommand = new String[5];

            try
            {
                if (_workOrderNum != string.Empty && _mat != string.Empty && _paramerTeam != string.Empty)
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            sqlCommand[0] = string.Format(@"UPDATE POR_WORK_ORDER_BOM_EXTENSION SET STATUS = 0 WHERE STATUS = 1 AND ORDER_NUMBER = '{0}' AND MATERIAL_CODE = '{1}' AND MATKL = '{2}'",
                                                                 _workOrderNum, _mat, _paramerTeam);

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
                            LogService.LogError("DeleteMatSpecialInf Error: " + ex.Message);
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
                LogService.LogError("DeleteMatSpecialInf Error: " + ex.Message);
            }

            return dataDs;
        }
        #endregion

        #region ISpecialMatTeamEngine 成员


        public DataSet GetMatSpecialInf()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT * FROM dbo.POR_WORK_ORDER_BOM_EXTENSION
                                                        WHERE STATUS = 1");

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMatSpecialInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ISpecialMatTeamEngine 成员


        public DataSet UpdateSpecialMatTeam(DataSet dataSet, string key)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            string sql01 = string.Empty;
            try
            {
                if (null != dataSet)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        sql = string.Format(@"INSERT INTO dbo.POR_WORK_ORDER_BOM_EXTENSION(EXTENSION_KEY,
                                                                   ORDER_NUMBER,MATERIAL_CODE,DESCRIPTION,STORE_LOC,MATKL,CREATOR,CREATE_TIME,STATUS)
                                                            VALUES('{6}','{0}','{1}','{3}','{4}','{2}','{5}',GETDATE(),1)
                                                            ", dataSet.Tables[0].Rows[0]["ORDER_NUMBER"].ToString(),
                                                                dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString(),
                                                                dataSet.Tables[0].Rows[0]["MATKL"].ToString(),
                                                                dataSet.Tables[0].Rows[0]["DESCRIPTION"].ToString(),
                                                                "",
                                                                "",
                                                                FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0)
                                                                );
                        db.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else
                    {
                        sql = string.Format(@"UPDATE POR_WORK_ORDER_BOM_EXTENSION SET
                                                                    STATUS = 0 WHERE STATUS = 1 AND EXTENSION_KEY = '{0}'",key);
                        db.ExecuteNonQuery(CommandType.Text, sql);
                        sql01 = string.Format(@"INSERT INTO dbo.POR_WORK_ORDER_BOM_EXTENSION(EXTENSION_KEY,
                                                                   ORDER_NUMBER,MATERIAL_CODE,DESCRIPTION,STORE_LOC,MATKL,CREATOR,CREATE_TIME,STATUS)
                                                            VALUES('{6}','{0}','{1}','{3}','{4}','{2}','{5}',GETDATE(),1)
                                                            ",  dataSet.Tables[0].Rows[0]["ORDER_NUMBER"].ToString(),
                                                                   dataSet.Tables[0].Rows[0]["MATERIAL_CODE"].ToString(),
                                                                   dataSet.Tables[0].Rows[0]["MATKL"].ToString(),
                                                                   dataSet.Tables[0].Rows[0]["DESCRIPTION"].ToString(),
                                                                   "",
                                                                   "",
                                                                   FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0)
                                                                   );
                        db.ExecuteNonQuery(CommandType.Text, sql01);
                    }
                    

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
                LogService.LogError("UpdateSpecialMatTeam Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region ISpecialMatTeamEngine 成员

        /// <summary>
        /// 获取参数组
        /// </summary>
        /// <returns></returns>
        public DataSet GetParamerTeam()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT EDC_NAME,DESCRIPTIONS FROM dbo.EDC_MAIN 
                                                            WHERE STATUS = 1
                                                            ");

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetParamerTeam Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}
