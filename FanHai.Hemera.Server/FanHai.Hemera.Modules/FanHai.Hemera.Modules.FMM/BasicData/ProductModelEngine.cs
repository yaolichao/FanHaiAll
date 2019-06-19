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
    public class ProductModelEngine : AbstractEngine, IProductModelEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ProductModelEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public DataSet GetProductModelAndCP()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_PRODUCTMODEL _productModel = new BASE_PRODUCTMODEL();
                BASE_PRODUCTMODEL_CP _productModel_cp = new BASE_PRODUCTMODEL_CP();
                BASE_PRODUCTMODEL_POWER _productModel_power = new BASE_PRODUCTMODEL_POWER();
                BASE_PRODUCTMODEL_CTM _productModel_ctm = new BASE_PRODUCTMODEL_CTM();        //add by chao.pang  20150701

                Conditions _conditions = new Conditions();

                _conditions.Add(DatabaseLogicOperator.And, BASE_PRODUCTMODEL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_productModel, null, _conditions);
                DataTable dtProModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtProModel.Columns.Add(BASE_PRODUCTMODEL.FIELDS_ISNEW);
                dtProModel.TableName = BASE_PRODUCTMODEL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtProModel, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And, BASE_PRODUCTMODEL_CP.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_productModel_cp, null, _conditions);
                DataTable dtProModel_cp = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtProModel_cp.Columns.Add(BASE_PRODUCTMODEL_CP.FIELDS_ISNEW);
                dtProModel_cp.TableName = BASE_PRODUCTMODEL_CP.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtProModel_cp, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And, BASE_PRODUCTMODEL_POWER.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_productModel_power, null, _conditions);
                DataTable dtProModel_Power = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtProModel_Power.Columns.Add(BASE_PRODUCTMODEL_POWER.FIELDS_ISNEW);
                dtProModel_Power.TableName = BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtProModel_Power, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And, BASE_PRODUCTMODEL_CTM.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1"); //add by chao.pang  20150701
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_productModel_ctm, null, _conditions); //add by chao.pang  20150701
                DataTable dtProModelCtm = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0]; //add by chao.pang  20150701
                dtProModelCtm.Columns.Add("ischecked", typeof(bool));
                dtProModelCtm.TableName = BASE_PRODUCTMODEL_CTM.DATABASE_TABLE_NAME; //add by chao.pang  20150701
                dsReturn.Merge(dtProModelCtm, true, MissingSchemaAction.Add); //add by chao.pang  20150701

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetProductModelAndCP Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得产品认证信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetCertification()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = string.Format(@"SELECT A.CERTIFICATION_KEY,A.CERTIFICATION_TYPE,A.CERTIFICATION_DATE
                                            FROM BASE_CERTIFICATION A
                                            WHERE A.IS_USED='Y'
                                            ORDER BY A.CERTIFICATION_TYPE,A.CERTIFICATION_DATE");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCertification Error: " + ex.Message);
            }
            return dsReturn;
        }
        public DataSet IsExistProductModel(DataTable dtInsertProductModel)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_PRODUCTMODEL _productModel = new BASE_PRODUCTMODEL();
                foreach (DataRow dr in dtInsertProductModel.Rows)
                {
                    Conditions _conditions = new Conditions();
                    _conditions.Add(DatabaseLogicOperator.And, BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME, DatabaseCompareOperator.Equal, dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_PRODUCTMODEL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                    sqlCommand = DatabaseTable.BuildQuerySqlStatement(_productModel, null, _conditions);
                    DataTable dtProModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (dtProModel.Rows.Count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("已存在型号{0},不能重复!", dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME].ToString()));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistProductModel Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet SaveProductModel(DataSet dsProModel)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtProModel_Update = null, dtProModel_Insert = null;
            DataTable dtProModel_CP_Update = null, dtProModel_CP_Insert = null;
            DataTable dtProModel_Level_Update = null, dtProModel_Level_Insert = null;
            DataTable dtProModel_CTM_Update = null;
            List<string> sqlCommandList = new List<string>();
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL.DATABASE_TABLE_FORINSERT))
                dtProModel_Insert = dsProModel.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_FORINSERT];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL.DATABASE_TABLE_FORUPDATE))
                dtProModel_Update = dsProModel.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_FORUPDATE];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL_CP.DATABASE_TABLE_FORINSERT))
                dtProModel_CP_Insert = dsProModel.Tables[BASE_PRODUCTMODEL_CP.DATABASE_TABLE_FORINSERT];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL_CP.DATABASE_TABLE_FORUPDATE))
                dtProModel_CP_Update = dsProModel.Tables[BASE_PRODUCTMODEL_CP.DATABASE_TABLE_FORUPDATE];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_FORINSERT))
                dtProModel_Level_Insert = dsProModel.Tables[BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_FORINSERT];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_FORUPDATE))
                dtProModel_Level_Update = dsProModel.Tables[BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_FORUPDATE];
            if (dsProModel.Tables.Contains("BASE_PRODUCTMODEL_CTM_INSERT"))
                dtProModel_CTM_Update = dsProModel.Tables["BASE_PRODUCTMODEL_CTM_INSERT"];
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                BASE_PRODUCTMODEL proModel = new BASE_PRODUCTMODEL();
                BASE_PRODUCTMODEL_CP proModel_cp = new BASE_PRODUCTMODEL_CP();
                BASE_PRODUCTMODEL_POWER proModel_Level = new BASE_PRODUCTMODEL_POWER();
                try
                {
                    if (dtProModel_Insert != null && dtProModel_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(proModel, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_CP_Insert != null && dtProModel_CP_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_CP_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(proModel_cp, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_Level_Insert != null && dtProModel_Level_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_Level_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(proModel_Level, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_Update != null && dtProModel_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY, hashTable[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY].ToString());
                            hashTable.Remove(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(proModel, hashTable, wc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_CP_Update != null && dtProModel_CP_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_CP_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY, hashTable[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY].ToString());
                            hashTable.Remove(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(proModel_cp, hashTable, wc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_Level_Update != null && dtProModel_Level_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_Level_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY, hashTable[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY].ToString());
                            hashTable.Remove(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(proModel_Level, hashTable, wc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_CTM_Update != null && dtProModel_CTM_Update.Rows.Count > 0)
                    {
                        string key = dtProModel_CTM_Update.Rows[0]["PROMODEL_KEY"].ToString();
                        string sql = string.Format("UPDATE BASE_PRODUCTMODEL_CTM SET ISFLAG = 0 WHERE PROMODEL_KEY = '{0}'",key);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        foreach (DataRow dr in dtProModel_CTM_Update.Rows)
                        {
                            string guid = System.Guid.NewGuid().ToString();
                            string sqlInsert = string.Format(@"INSERT INTO dbo.BASE_PRODUCTMODEL_CTM(PROMODEL_KEY,CTM_KEY,EFF_UP,EFF_LOW,CTM_UP,CTM_LOW)
                                                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                                    key,
                                                                    guid,
                                                                    dr["EFF_UP"].ToString(),
                                                                    dr["EFF_LOW"].ToString(),
                                                                    dr["CTM_UP"].ToString(),
                                                                    dr["CTM_LOW"].ToString());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlInsert);
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
                    LogService.LogError("SaveProductModel Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        public DataSet DelProductModel(DataSet dsProModel)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtProModel = null,  dtProModel_CP = null, dtProModel_Level= null;
            List<string> sqlCommandList = new List<string>();
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL.DATABASE_TABLE_NAME))
                dtProModel = dsProModel.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_NAME];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL_CP.DATABASE_TABLE_NAME))
                dtProModel_CP = dsProModel.Tables[BASE_PRODUCTMODEL_CP.DATABASE_TABLE_NAME];
            if (dsProModel.Tables.Contains(BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_NAME))
                dtProModel_Level = dsProModel.Tables[BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_NAME];

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                BASE_PRODUCTMODEL proModel = new BASE_PRODUCTMODEL();
                BASE_PRODUCTMODEL_CP proModel_cp = new BASE_PRODUCTMODEL_CP();
                BASE_PRODUCTMODEL_POWER proModel_Level = new BASE_PRODUCTMODEL_POWER();
                try
                {
                    if (dtProModel != null && dtProModel.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY, hashTable[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY].ToString());
                            hashTable.Remove(BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(proModel, hashTable, wc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_CP != null && dtProModel_CP.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_CP.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY, hashTable[BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY].ToString());
                            hashTable.Remove(BASE_PRODUCTMODEL_CP.FIELDS_PROMODEL_DTL_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(proModel_cp, hashTable, wc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProModel_Level != null && dtProModel_Level.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProModel_Level.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY, hashTable[BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY].ToString());
                            hashTable.Remove(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(proModel_Level, hashTable, wc);

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
                    LogService.LogError("DelProductModel Error: " + ex.Message);
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

