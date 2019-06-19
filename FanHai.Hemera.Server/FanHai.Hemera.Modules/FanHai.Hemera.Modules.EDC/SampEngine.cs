#region using
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;

using Microsoft.Practices.EnterpriseLibrary.Data;
#endregion

namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 抽检规则和异常规则的数据管理类。
    /// </summary>
    public class SampEngine:AbstractEngine,ISampEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SampEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 新增抽检规则。
        /// </summary>
        /// <param name="dsParams">包含抽检规则的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddSamp(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null!=dsParams && dsParams.Tables.Contains(EDC_SP_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            EDC_SP_FIELDS spFields = new EDC_SP_FIELDS();
                            string sqlCommand = DatabaseTable.BuildInsertSqlStatement(spFields, htParams, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("AddSamp Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("AddSamp Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 删除抽检规则。
        /// </summary>
        /// <param name="spKey">抽检见规则主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteSamp(string spKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(spKey))
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {

                            string sqlCommand = @"DELETE EDC_SP WHERE SP_KEY = '" + spKey.PreventSQLInjection() + "'";
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("DeleteSamp Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran=null;
                            //Close Connection
                            dbConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("DeleteSamp Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询抽检规则。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -------------------------------
        /// {SP_NAME}
        /// -------------------------------
        /// </param>
        /// <returns>包含抽检规则的数据集对象。</returns>
        public DataSet SearchSamp(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EDC_SP_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string spName=Convert.ToString(htParams[EDC_SP_FIELDS.FIELD_SP_NAME]);
                    string sqlCommand =string.Format(@"SELECT * FROM EDC_SP
                                                    WHERE SP_NAME LIKE '%{0}%' AND STATUS<> 2 
                                                    ORDER BY SP_NAME",
                                                    spName.PreventSQLInjection());
                    DataTable dtTable = new DataTable();
                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchSamp Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据抽检规则主键获取抽检规则。
        /// </summary>
        /// <param name="spKey">抽检规则主键。</param>
        /// <returns>包含抽检规则的数据集对象。</returns>
        public DataSet GetSampByKey(string spKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(spKey))
                {
                    string sqlCommand=string.Format(@"SELECT * FROM EDC_SP
                                                    WHERE SP_KEY = '{0}'",
                                                    spKey.PreventSQLInjection());
                    DataTable dtTable = new DataTable();
                    dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    dtTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSampByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取不重复的抽检规则名称。
        /// </summary>
        /// <returns>包含抽检规则名称的数据集对象。</returns>
        public DataSet GetDistinctSampName()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand =@"SELECT DISTINCT SP_NAME 
                                   FROM EDC_SP
                                   WHERE STATUS <> 2
                                   ORDER BY SP_NAME";

                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctSampName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新抽检规则数据。
        /// </summary>
        /// <param name="dsParams">包含抽检规则的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateSamp(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (null != dsParams && dsParams.Tables.Contains(EDC_SP_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            EDC_SP_FIELDS spFields = new EDC_SP_FIELDS();
                            WhereConditions wc = new WhereConditions(EDC_SP_FIELDS.FIELD_SP_KEY, htParams[EDC_SP_FIELDS.FIELD_SP_KEY].ToString());
                            htParams.Remove(EDC_SP_FIELDS.FIELD_SP_KEY);
                            string sqlCommand = DatabaseTable.BuildUpdateSqlStatement(spFields, htParams, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            //Commit Transaction
                            dbTran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("UpdateSamp Error: " + ex.Message);
                        }
                        finally
                        {
                            dbTran = null;
                            //Close Connection
                            dbConn.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateSamp Error: " + ex.Message);
            }
            return dsReturn;
        }

        //-------------------------------------------------------------------
        // 异常规则
        //-------------------------------------------------------------------
        /// <summary>
        /// 获取异常规则数据。
        /// </summary>
        /// <returns>包含异常规则数据的数据集对象。</returns>
        public DataSet GetAbnormalRule()
        {
            DataSet dsReturn = new DataSet();
            //define sqlCommand 
            string[] sqlCommand = new string[2];
            try
            {
                sqlCommand[0] = @"SELECT T.*
                                  FROM EDC_ABNORMALRULE T
                                  WHERE T.LVORM = '0'
                                  ORDER BY T.ARULECODE";
                sqlCommand[1] = @"SELECT T.*
                                  FROM EDC_ABNORMALRULE_DTL T, EDC_ABNORMALRULE M
                                  WHERE T.LVORM = '0'
                                  AND M.LVORM = '0'
                                  AND T.ABNORMALID = M.ABNORMALID";
                DataTable dt01 = new DataTable();
                DataTable dt02 = new DataTable();
                dt01 = db.ExecuteDataSet(CommandType.Text, sqlCommand[0]).Tables[0];
                AddOneColumnFlag(dt01, "FLAGMAIN");
                dt01.TableName = EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME;
                dt02 = db.ExecuteDataSet(CommandType.Text, sqlCommand[1]).Tables[0];
                AddOneColumnFlag(dt02, "FLAGDTL");
                dt02.TableName = EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dt01, false, MissingSchemaAction.Add);
                dsReturn.Merge(dt02, false, MissingSchemaAction.Add);
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctSampName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 为数据表中指定列添加READONLY标记。
        /// </summary>
        /// <param name="dt">数据表对象。</param>
        /// <param name="colName">列名。</param>
        private void AddOneColumnFlag(DataTable dt, string colName)
        {
            dt.Columns.Add(colName);
            foreach (DataRow dr in dt.Rows)
            {
                dr[colName] = "READONLY";
            }
            dt.AcceptChanges();
        }
        /// <summary>
        /// 根据异常规则主键获得异常规则明细数据。包括异常规则的颜色值
        /// </summary>
        /// <param name="abnormalIds">异常规则主键,格式如下：'id1','id2'...</param>
        /// <returns>包含异常规则明细数据的数据集对象。</returns>
        public DataSet GetAbnormalDetailRule(string abnormalIds)
        {
            DataSet dsReturn = new DataSet();
            try
            {
               string sql = string.Format(@"SELECT T.*, T1.ABNORMALCOLOR,T1.ARULECODE
                                            FROM EDC_ABNORMALRULE_DTL T, EDC_ABNORMALRULE T1
                                            WHERE T.ABNORMALID = T1.ABNORMALID
                                            AND T1.LVORM = 0
                                            AND T.ABNORMALID IN ({0})",abnormalIds);
               DataTable dtAbnormal = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
               dtAbnormal.TableName = EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME;

               dsReturn.Merge(dtAbnormal, false, MissingSchemaAction.Add);
              
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctSampName Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增/更新异常规则。
        /// </summary>
        /// <param name="dsAbnormalRule">包含异常规则数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveAbnormalRule(DataSet dsAbnormalRule)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = "";
            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    try
                    {
                        EDC_ABNORMAL_FIELDS edcAbnormalRule = new EDC_ABNORMAL_FIELDS();
                        EDC_ABNORMAL_DTL_FIELDS edcAbnormalRule_Dtl = new EDC_ABNORMAL_DTL_FIELDS();
                        //插入异常规则数据。
                        if (dsAbnormalRule.Tables.Contains(EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DataTable dataTable = dsAbnormalRule.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(edcAbnormalRule, hashTable, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        //插入异常规则明细数据
                        if (dsAbnormalRule.Tables.Contains(EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DataTable dataTable = dsAbnormalRule.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(edcAbnormalRule_Dtl, hashTable, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        //更新异常规则数据。
                        if (dsAbnormalRule.Tables.Contains(EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME_FORUPDATE))
                        {
                            DataTable dataTable = dsAbnormalRule.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME_FORUPDATE];
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                string abnormalId = Convert.ToString(hashTable[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID]);
                                WhereConditions wc = new WhereConditions(EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID,abnormalId );
                                hashTable.Remove(EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID);
                                sqlCommand = DatabaseTable.BuildUpdateSqlStatement(edcAbnormalRule, hashTable, wc);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        //更新异常规则明细数据。
                        if (dsAbnormalRule.Tables.Contains(EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME_FORUPDATE))
                        {
                            DataTable dataTable = dsAbnormalRule.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME_FORUPDATE];
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                string abnormailDetailId=Convert.ToString(hashTable[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL]);
                                WhereConditions wc = new WhereConditions(EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL, abnormailDetailId);
                                hashTable.Remove(EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL);
                                sqlCommand = DatabaseTable.BuildUpdateSqlStatement(edcAbnormalRule_Dtl, hashTable, wc);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        //Commit Transaction
                        dbTran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                    catch (Exception ex)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        //Rollback Transaction
                        dbTran.Rollback();
                        LogService.LogError("SaveAbnormalRule Error: " + ex.Message);
                    }
                    finally
                    {
                        dbTran = null;
                        //Close Connection
                        dbConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SaveAbnormalRule Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 判断异常规则代码是否存在。
        /// </summary>
        /// <param name="strCode">
        /// 异常规则代码数组。
        /// {ARULECODE(必须),INSERT/UPDATE(必须),ABNORMALID(UPDATE时必须)}
        /// -------------------------------
        /// ARULECODE       ：异常规则代码。
        /// INSERT/UPDATE   ：判断类型。INSERT：是否存在指定的异常规则代码。,UPDATE：除指定规则主键外是否还存在指定的异常规则代码。
        /// ABNORMALID      ：异常规则主键。
        /// -------------------------------
        /// </param>
        /// <returns>
        /// 包含执行结果的数据集对象。
        /// 扩展属性Code_Counts表示存在的异常规则代码个数。
        /// </returns>
        public DataSet IsExistAbnormalCode(string[] strCode)
        {
            DataSet dsReturn =new DataSet();
            string strSql = "";  
            try
            {
                if (strCode[1].ToUpper() == "INSERT")
                {
                    strSql = string.Format(@"SELECT COUNT(1)
                                             FROM EDC_ABNORMALRULE T
                                             WHERE T.ARULECODE = '{0}'
                                             AND T.LVORM = '0'",
                                             strCode[0].PreventSQLInjection());
                }
                else if (strCode[1].ToUpper() == "UPDATE")
                {
                    strSql = string.Format(@"SELECT COUNT(1)
                                            FROM EDC_ABNORMALRULE T
                                            WHERE T.ARULECODE = '{0}'
                                            AND T.LVORM = '0'
                                            AND T.ABNORMALID <> '{1}'", 
                                            strCode[0],
                                            strCode[2]);
                }
                DataTable dt01 = new DataTable();
                int i = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (dsReturn.ExtendedProperties.ContainsKey("Code_Counts"))
                    dsReturn.ExtendedProperties.Remove("Code_Counts");
                dsReturn.ExtendedProperties.Add("Code_Counts", i.ToString());
                //add paramter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistAbnormalCode Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
