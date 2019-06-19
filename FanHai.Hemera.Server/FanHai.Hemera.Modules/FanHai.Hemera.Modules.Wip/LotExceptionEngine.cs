using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Modules.FMM;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.Wip
{
    public class LotExceptionEngine : AbstractEngine,ILotExceptionEngine
    {
        private Database db;                             //用于数据库访问的变量。
        public LotExceptionEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public override void Initialize() { }

        #region ILotExceptionEngine 成员

        public DataSet GetLotExceptionData(string lotNo, string sTime, string eTime)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"Select * from POR_LOT_EXCEPTION where 1=1");

                if (lotNo != "")
                {
                    sql += string.Format(@" and LOT_NUMBER='{0}'",lotNo);
                }
                if (sTime != ""&&eTime!="")
                {
                    sql += string.Format(@" and CREATE_DATE between '{0}' and '{1}'",sTime,eTime);
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotExceptionData Error: " + ex.Message);
            }
            return dsReturn;
        }

      

        #endregion

        #region ILotExceptionEngine 成员


        public DataSet InsertLotExceptionDetail(DataSet data)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTrans = null;
            DbConnection dbCon = null;
            if (data != null)
            {
                try
                {
                    DataTable dtLots = data.Tables[0];
                    if (dtLots == null || dtLots.Rows.Count == 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "不良跟踪信息的批次数据不能为空。");
                        return dsReturn;
                    }

                    string sql = @"INSERT INTO POR_LOT_EXCEPTION ([LOT_NUMBER],[CREATE_OPERATION_NAME],[CREATE_DATE] ,[DOPERATION] ,[FAB]
                                   ,[CLASS],[CUSTOMER],[PORDUCT_TYPE],[ADVERSE_POSITION],[ADVERSE_DESC],[REASON],[IMPROVE]
                                   ,[ADVERSE_CLASS] ,[RESOULT],[RESPONSE_CLASS],[RESPONSE_POSITION],[ATTRIBUTION_RESPONSE]
                                   ,[REMARK],[SUPPLIER],[CELL_GREAD],[WELDING_TYPE],[FRAME_TYPE],[PARTIALLY],[EQUIPMENT1]
                                   ,[EQUIPMENT2],[EQUIPMENT3],[EQUIPMENT4],[EQUIPMENT5],[EQUIPMENT6],[EQUIPMENT7],[AMES_GREAD],[LOT_CREATE_TIME],JudgeTime,DutyLine)
                             VALUES(@LOT_NUMBER,@CREATE_OPERATION_NAME,@CREATE_DATE,@DOPERATION,@FAB
                                   ,@CLASS,@CUSTOMER,@PORDUCT_TYPE,@ADVERSE_POSITION,@ADVERSE_DESC,@REASON,@IMPROVE
                                   ,@ADVERSE_CLASS,@RESOULT,@RESPONSE_CLASS,@RESPONSE_POSITION,@ATTRIBUTION_RESPONSE,@REMARK
                                   ,@SUPPLIER,@CELL_GREAD,@WELDING_TYPE,@FRAME_TYPE,@PARTIALLY,@EQUIPMENT1,@EQUIPMENT2
                                   ,@EQUIPMENT3,@EQUIPMENT4,@EQUIPMENT5,@EQUIPMENT6,@EQUIPMENT7,@AMES_GREAD,@LOT_CREATE_TIME,@JudgeTime,@DutyLine)";

                    dbCon = db.CreateConnection();
                    dbCon.Open();
                    dbTrans = dbCon.BeginTransaction();

                    foreach (DataRow dr in dtLots.Rows)
                    {
                        DbCommand dbCmd = db.GetSqlStringCommand(sql);
                        db.AddInParameter(dbCmd, "LOT_NUMBER", DbType.String, dr["LOT_NUMBER"].ToString());
                        db.AddInParameter(dbCmd, "CREATE_OPERATION_NAME", DbType.String, dr["CREATE_OPERATION_NAME"].ToString());
                        db.AddInParameter(dbCmd, "CREATE_DATE", DbType.DateTime, dr["CREATE_DATE"].ToString());
                        db.AddInParameter(dbCmd, "DOPERATION", DbType.String, dr["DOPERATION"].ToString());
                        db.AddInParameter(dbCmd, "FAB", DbType.String, dr["FAB"].ToString());
                        db.AddInParameter(dbCmd, "CLASS", DbType.String, dr["CLASS"].ToString());
                        db.AddInParameter(dbCmd, "CUSTOMER", DbType.String, dr["CUSTOMER"].ToString());
                        db.AddInParameter(dbCmd, "PORDUCT_TYPE", DbType.String, dr["PORDUCT_TYPE"].ToString());
                        db.AddInParameter(dbCmd, "ADVERSE_POSITION", DbType.String, dr["ADVERSE_POSITION"].ToString());
                        db.AddInParameter(dbCmd, "ADVERSE_DESC", DbType.String, dr["ADVERSE_DESC"].ToString());
                        db.AddInParameter(dbCmd, "REASON", DbType.String, dr["REASON"].ToString());
                        db.AddInParameter(dbCmd, "IMPROVE", DbType.String, dr["IMPROVE"].ToString());
                        db.AddInParameter(dbCmd, "ADVERSE_CLASS", DbType.String, dr["ADVERSE_CLASS"].ToString());
                        db.AddInParameter(dbCmd, "RESOULT", DbType.String, dr["RESOULT"].ToString());
                        db.AddInParameter(dbCmd, "RESPONSE_CLASS", DbType.String, dr["RESPONSE_CLASS"].ToString());
                        db.AddInParameter(dbCmd, "RESPONSE_POSITION", DbType.String, dr["RESPONSE_POSITION"].ToString());
                        db.AddInParameter(dbCmd, "ATTRIBUTION_RESPONSE", DbType.String, dr["ATTRIBUTION_RESPONSE"].ToString());
                        db.AddInParameter(dbCmd, "REMARK", DbType.String, dr["REMARK"].ToString());
                        db.AddInParameter(dbCmd, "SUPPLIER", DbType.String, dr["SUPPLIER"].ToString());
                        db.AddInParameter(dbCmd, "CELL_GREAD", DbType.String, dr["CELL_GREAD"].ToString());
                        db.AddInParameter(dbCmd, "WELDING_TYPE", DbType.String, dr["WELDING_TYPE"].ToString());
                        db.AddInParameter(dbCmd, "FRAME_TYPE", DbType.String, dr["FRAME_TYPE"].ToString());
                        db.AddInParameter(dbCmd, "PARTIALLY", DbType.String, dr["PARTIALLY"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT1", DbType.String, dr["EQUIPMENT1"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT2", DbType.String, dr["EQUIPMENT2"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT3", DbType.String, dr["EQUIPMENT3"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT4", DbType.String, dr["EQUIPMENT4"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT5", DbType.String, dr["EQUIPMENT5"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT6", DbType.String, dr["EQUIPMENT6"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT7", DbType.String, dr["EQUIPMENT7"].ToString());
                        db.AddInParameter(dbCmd, "AMES_GREAD", DbType.String, dr["AMES_GREAD"].ToString());
                        db.AddInParameter(dbCmd, "LOT_CREATE_TIME", DbType.DateTime, dr["LOT_CREATE_TIME"].ToString());
                        db.AddInParameter(dbCmd, "JudgeTime", DbType.DateTime, dr["JudgeTime"].ToString());
                        db.AddInParameter(dbCmd, "DutyLine", DbType.String, dr["DutyLine"].ToString());

                        int nRet = db.ExecuteNonQuery(dbCmd, dbTrans);
                    }
                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    if (dbTrans != null)
                    {
                        dbTrans.Rollback();
                    }
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError(string.Format("LotException-Insert Error:{0}", ex.Message));
                }
                finally
                {
                    if (dbCon != null)
                    {
                        if (dbCon.State != ConnectionState.Closed)
                        {
                            dbCon.Close();
                        }
                        dbCon = null;
                    }
                }
            }
            return dsReturn;
        }

        public DataSet UpdateLotExceptionDatail(DataSet data)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTrans = null;
            DbConnection dbCon = null;
            if (data != null)
            {
                try
                {
                    DataTable dtLots = data.Tables[0];
                    if (dtLots == null || dtLots.Rows.Count == 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "不良跟踪信息的批次数据不能为空。");
                        return dsReturn;
                    }

                    string sql = @"Update  POR_LOT_EXCEPTION set [LOT_NUMBER]=@LOT_NUMBER,[CREATE_OPERATION_NAME]=@CREATE_OPERATION_NAME,[CREATE_DATE]=@CREATE_DATE ,
                                    [DOPERATION]=@DOPERATION ,[FAB]=@FAB
                                   ,[CLASS]=@CLASS,[CUSTOMER]=@CUSTOMER,[PORDUCT_TYPE]=@PORDUCT_TYPE,[ADVERSE_POSITION]=@ADVERSE_POSITION,
                                    [ADVERSE_DESC]=@ADVERSE_DESC,[REASON]=@REASON,[IMPROVE]=@IMPROVE
                                   ,[ADVERSE_CLASS]=@ADVERSE_CLASS ,[RESOULT]=@RESOULT,[RESPONSE_CLASS]=@RESPONSE_CLASS,[RESPONSE_POSITION]=@RESPONSE_POSITION,
                                    [ATTRIBUTION_RESPONSE]=@ATTRIBUTION_RESPONSE
                                   ,[REMARK]=@REMARK,[SUPPLIER]=@SUPPLIER,[CELL_GREAD]=@CELL_GREAD,[WELDING_TYPE]=@WELDING_TYPE,[FRAME_TYPE]=@FRAME_TYPE,[PARTIALLY]=@PARTIALLY
                                    ,[EQUIPMENT1]=@EQUIPMENT1
                                   ,[EQUIPMENT2]=@EQUIPMENT2,[EQUIPMENT3]=@EQUIPMENT3,[EQUIPMENT4]=@EQUIPMENT4
                                    ,[EQUIPMENT5]=@EQUIPMENT5,[EQUIPMENT6]=@EQUIPMENT6,[EQUIPMENT7]=@EQUIPMENT7,[AMES_GREAD]=@AMES_GREAD,[LOT_CREATE_TIME]=@LOT_CREATE_TIME
                                    ,JudgeTime=@JudgeTime,DutyLine=@DutyLine
                                    where LOT_EXCEPTION_ID=@LOT_EXCEPTION_ID";

                    dbCon = db.CreateConnection();
                    dbCon.Open();
                    dbTrans = dbCon.BeginTransaction();

                    foreach (DataRow dr in dtLots.Rows)
                    {
                        DbCommand dbCmd = db.GetSqlStringCommand(sql);
                        db.AddInParameter(dbCmd, "LOT_NUMBER", DbType.String, dr["LOT_NUMBER"].ToString());
                        db.AddInParameter(dbCmd, "CREATE_OPERATION_NAME", DbType.String, dr["CREATE_OPERATION_NAME"].ToString());
                        db.AddInParameter(dbCmd, "CREATE_DATE", DbType.DateTime, dr["CREATE_DATE"].ToString());
                        db.AddInParameter(dbCmd, "DOPERATION", DbType.String, dr["DOPERATION"].ToString());
                        db.AddInParameter(dbCmd, "FAB", DbType.String, dr["FAB"].ToString());
                        db.AddInParameter(dbCmd, "CLASS", DbType.String, dr["CLASS"].ToString());
                        db.AddInParameter(dbCmd, "CUSTOMER", DbType.String, dr["CUSTOMER"].ToString());
                        db.AddInParameter(dbCmd, "PORDUCT_TYPE", DbType.String, dr["PORDUCT_TYPE"].ToString());
                        db.AddInParameter(dbCmd, "ADVERSE_POSITION", DbType.String, dr["ADVERSE_POSITION"].ToString());
                        db.AddInParameter(dbCmd, "ADVERSE_DESC", DbType.String, dr["ADVERSE_DESC"].ToString());
                        db.AddInParameter(dbCmd, "REASON", DbType.String, dr["REASON"].ToString());
                        db.AddInParameter(dbCmd, "IMPROVE", DbType.String, dr["IMPROVE"].ToString());
                        db.AddInParameter(dbCmd, "ADVERSE_CLASS", DbType.String, dr["ADVERSE_CLASS"].ToString());
                        db.AddInParameter(dbCmd, "RESOULT", DbType.String, dr["RESOULT"].ToString());
                        db.AddInParameter(dbCmd, "RESPONSE_CLASS", DbType.String, dr["RESPONSE_CLASS"].ToString());
                        db.AddInParameter(dbCmd, "RESPONSE_POSITION", DbType.String, dr["RESPONSE_POSITION"].ToString());
                        db.AddInParameter(dbCmd, "ATTRIBUTION_RESPONSE", DbType.String, dr["ATTRIBUTION_RESPONSE"].ToString());
                        db.AddInParameter(dbCmd, "REMARK", DbType.String, dr["REMARK"].ToString());
                        db.AddInParameter(dbCmd, "SUPPLIER", DbType.String, dr["SUPPLIER"].ToString());
                        db.AddInParameter(dbCmd, "CELL_GREAD", DbType.String, dr["CELL_GREAD"].ToString());
                        db.AddInParameter(dbCmd, "WELDING_TYPE", DbType.String, dr["WELDING_TYPE"].ToString());
                        db.AddInParameter(dbCmd, "FRAME_TYPE", DbType.String, dr["FRAME_TYPE"].ToString());
                        db.AddInParameter(dbCmd, "PARTIALLY", DbType.String, dr["PARTIALLY"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT1", DbType.String, dr["EQUIPMENT1"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT2", DbType.String, dr["EQUIPMENT2"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT3", DbType.String, dr["EQUIPMENT3"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT4", DbType.String, dr["EQUIPMENT4"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT5", DbType.String, dr["EQUIPMENT5"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT6", DbType.String, dr["EQUIPMENT6"].ToString());
                        db.AddInParameter(dbCmd, "EQUIPMENT7", DbType.String, dr["EQUIPMENT7"].ToString());
                        db.AddInParameter(dbCmd, "AMES_GREAD", DbType.String, dr["AMES_GREAD"].ToString());
                        db.AddInParameter(dbCmd, "LOT_CREATE_TIME", DbType.DateTime, dr["LOT_CREATE_TIME"].ToString());
                        db.AddInParameter(dbCmd, "LOT_EXCEPTION_ID", DbType.Int32, dr["LOT_EXCEPTION_ID"].ToString());
                        db.AddInParameter(dbCmd, "JudgeTime", DbType.DateTime, dr["JudgeTime"].ToString());
                        db.AddInParameter(dbCmd, "DutyLine", DbType.String, dr["DutyLine"].ToString());
                        int nRet = db.ExecuteNonQuery(dbCmd, dbTrans);
                    }
                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    if (dbTrans != null)
                    {
                        dbTrans.Rollback();
                    }
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError(string.Format("LotException-Update Error:{0}", ex.Message));
                }
                finally
                {
                    if (dbCon != null)
                    {
                        if (dbCon.State != ConnectionState.Closed)
                        {
                            dbCon.Close();
                        }
                        dbCon = null;
                    }
                }
            }
            return dsReturn;
        }

        public DataSet DeleteLotExceptionDetail(string id)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                string sql = string.Format(@"Delete POR_LOT_EXCEPTION 
                                             WHERE LOT_EXCEPTION_ID=@LOT_EXCEPTION_ID");
                DbCommand dbCmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCmd, "LOT_EXCEPTION_ID", DbType.Int32, id);
                int nRet = db.ExecuteNonQuery(dbCmd);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError(string.Format("LotException-Delete Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion

        #region ILotExceptionEngine 成员


        public DataSet GetExtendParam(string key)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"Select * from BASE_EXTEND_PARAM where 1=1");

                if (key != "")
                {
                    sql += string.Format(@" and PARAM_KEY='{0}'", key);
                }
               
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetExtendParam Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ILotExceptionEngine 成员


        public DataSet GetAllOperations()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT ROUTE_OPERATION_NAME  FROM POR_ROUTE_OPERATION_VER where OPERATION_STATUS=1 ");
              
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetExtendParam Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetAllFab()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"select LOCATION_NAME from FMM_LOCATION where LOCATION_LEVEL=5");

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAllFab Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
   

        #region ILotExceptionEngine 成员


        public DataSet GetLotInfo(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"select a.LOT_NUMBER,a.CREATE_TIME,b.PART_TYPE from POR_LOT  a with(nolock) 
                                            inner join POR_PART b with(nolock) on a.PART_NUMBER=b.PART_NAME
                                            where a.LOT_NUMBER='{0}' ", lotNo);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}
