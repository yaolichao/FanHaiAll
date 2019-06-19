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
    /// <summary>
    /// 产品型号及产品设置操作类
    /// </summary>
    public class DecayCoeffiEngine : AbstractEngine, IDecayCoeffiEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DecayCoeffiEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 获得衰减数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetDecayCoeffiData()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {                
                BASE_DECAYCOEFFI _decayCoeffi = new BASE_DECAYCOEFFI();
                Conditions _conditions = new Conditions();

                _conditions.Add(DatabaseLogicOperator.And, BASE_DECAYCOEFFI.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_decayCoeffi, null, _conditions);
                DataTable dtDecayCoeffi = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtDecayCoeffi.TableName = BASE_DECAYCOEFFI.DATABASE_TABLE_NAME;
                dtDecayCoeffi.Columns.Add(BASE_DECAYCOEFFI.FIELDS_ISNEW);
                dsReturn.Merge(dtDecayCoeffi, true, MissingSchemaAction.Add);              
                
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDecayCoeffiData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet IsExistDecayCoeffiData(DataTable dtInsertDecayCoeffi)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_DECAYCOEFFI _decayCoeffi = new BASE_DECAYCOEFFI();
                foreach (DataRow dr in dtInsertDecayCoeffi.Rows)
                {
                    Conditions _conditions = new Conditions();
                    _conditions.Add(DatabaseLogicOperator.And, BASE_DECAYCOEFFI.FIELDS_D_CODE, DatabaseCompareOperator.Equal, dr[BASE_DECAYCOEFFI.FIELDS_D_CODE].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_DECAYCOEFFI.FIELDS_D_NAME, DatabaseCompareOperator.Equal, dr[BASE_DECAYCOEFFI.FIELDS_D_NAME].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_DECAYCOEFFI.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                    sqlCommand = DatabaseTable.BuildQuerySqlStatement(_decayCoeffi, null, _conditions);
                    DataTable dtProModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (dtProModel.Rows.Count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("衰减代码{0},不能重复!", dr[BASE_DECAYCOEFFI.FIELDS_D_CODE].ToString()));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistDecayCoeffiData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 修改衰减数据
        /// </summary>
        /// <param name="dsProModel"></param>
        /// <returns></returns>
        public DataSet SaveDecayCoeffiData(DataSet dsDecayCoeffi)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtDecayCoeffi_Update = null, dtDecayCoeffi_Insert = null;   
            List<string> sqlCommandList = new List<string>();
            if (dsDecayCoeffi.Tables.Contains(BASE_DECAYCOEFFI.DATABASE_TABLE_FORINSERT))
            {
                dtDecayCoeffi_Insert = dsDecayCoeffi.Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_FORINSERT];
                dtDecayCoeffi_Insert.Columns.Remove(BASE_DECAYCOEFFI.FIELDS_ISNEW);
            }
            if (dsDecayCoeffi.Tables.Contains(BASE_DECAYCOEFFI.DATABASE_TABLE_FORUPDATE))
            {
                dtDecayCoeffi_Update = dsDecayCoeffi.Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_FORUPDATE];
                dtDecayCoeffi_Update.Columns.Remove(BASE_DECAYCOEFFI.FIELDS_ISNEW);
            }
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                
                BASE_DECAYCOEFFI decayCoeffi = new BASE_DECAYCOEFFI();
                try
                {
                    if (dtDecayCoeffi_Insert != null && dtDecayCoeffi_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDecayCoeffi_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(decayCoeffi, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtDecayCoeffi_Update != null && dtDecayCoeffi_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDecayCoeffi_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY, hashTable[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY].ToString());
                            hashTable.Remove(BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY);
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(decayCoeffi, hashTable, wc);
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
                    LogService.LogError("SaveSpcControlPlan Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

    }
}

