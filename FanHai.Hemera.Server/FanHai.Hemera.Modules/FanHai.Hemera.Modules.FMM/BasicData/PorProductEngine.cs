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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.FMM
{  
    /// <summary>
    /// 产品型号及产品设置操作类
    /// </summary>
    public class PorProductEngine : AbstractEngine, IPorProductEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PorProductEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        public DataSet GetPorProductData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                POR_PRODUCT _por_Product = new POR_PRODUCT();
                POR_PRODUCT_DTL _por_product_dtl = new POR_PRODUCT_DTL();
             
                Conditions _conditions = new Conditions();
                if (hstable.ContainsKey(POR_PRODUCT.FIELDS_PRODUCT_CODE))
                    _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_PRODUCT_CODE, DatabaseCompareOperator.Equal, hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString());
                if (hstable.ContainsKey(POR_PRODUCT.FIELDS_PRO_TEST_RULE))
                    _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_PRO_TEST_RULE, DatabaseCompareOperator.Like, hstable[POR_PRODUCT.FIELDS_PRO_TEST_RULE].ToString() + "%");
                if (hstable.ContainsKey(POR_PRODUCT.FIELDS_CREATE_TIME_START) && hstable.ContainsKey(POR_PRODUCT.FIELDS_CREATE_TIME_END))
                {
                    _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_CREATE_TIME, DatabaseCompareOperator.GreaterThanEqual, hstable[POR_PRODUCT.FIELDS_CREATE_TIME_START].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_CREATE_TIME, DatabaseCompareOperator.LessThanEqual, hstable[POR_PRODUCT.FIELDS_CREATE_TIME_END].ToString());
                }
                else if (hstable.ContainsKey(POR_PRODUCT.FIELDS_CREATE_TIME_START))
                    _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_CREATE_TIME, DatabaseCompareOperator.GreaterThan, hstable[POR_PRODUCT.FIELDS_CREATE_TIME_START].ToString());
                else if (hstable.ContainsKey(POR_PRODUCT.FIELDS_CREATE_TIME_END))
                    _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_CREATE_TIME, DatabaseCompareOperator.LessThan, hstable[POR_PRODUCT.FIELDS_CREATE_TIME_END].ToString());

                _conditions.Add(DatabaseLogicOperator.And, POR_PRODUCT.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_por_Product, null, _conditions);
                DataTable dtPorProduct = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                dtPorProduct.TableName = POR_PRODUCT.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPorProduct, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And,POR_PRODUCT_DTL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_por_product_dtl, null, _conditions);
                DataTable dtPorProduct_dtl = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPorProduct_dtl.TableName = POR_PRODUCT_DTL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPorProduct_dtl, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPorProductData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存产品设置数据
        /// </summary>
        /// <param name="dsPowerSet"></param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-10-30 17:51
        public DataSet SavePorProductData(DataSet dsPorProduct)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtPorProduct_Update = null, dtPorProduct_Insert = null;
            DataTable dtPorProduct_Dtl_Update = null, dtPorProduct_Dtl_Insert = null;
       
            List<string> sqlCommandList = new List<string>();
            if (dsPorProduct.Tables.Contains(POR_PRODUCT.DATABASE_TABLE_FORINSERT))            
                dtPorProduct_Insert = dsPorProduct.Tables[POR_PRODUCT.DATABASE_TABLE_FORINSERT];                           
            if (dsPorProduct.Tables.Contains(POR_PRODUCT.DATABASE_TABLE_FORUPDATE))
                dtPorProduct_Update = dsPorProduct.Tables[POR_PRODUCT.DATABASE_TABLE_FORUPDATE];

            if (dsPorProduct.Tables.Contains(POR_PRODUCT_DTL.DATABASE_TABLE_FORINSERT))
                dtPorProduct_Dtl_Insert = dsPorProduct.Tables[POR_PRODUCT_DTL.DATABASE_TABLE_FORINSERT];
            if (dsPorProduct.Tables.Contains(POR_PRODUCT_DTL.DATABASE_TABLE_FORUPDATE))
                dtPorProduct_Dtl_Update = dsPorProduct.Tables[POR_PRODUCT_DTL.DATABASE_TABLE_FORUPDATE];

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                POR_PRODUCT porProduct_fields = new POR_PRODUCT();
                POR_PRODUCT_DTL porProduct_dtl_fields = new POR_PRODUCT_DTL();
               
                try
                {
                    if (dtPorProduct_Insert != null && dtPorProduct_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPorProduct_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(porProduct_fields, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtPorProduct_Update != null && dtPorProduct_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPorProduct_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(POR_PRODUCT.FIELDS_PRODUCT_KEY, hashTable[POR_PRODUCT.FIELDS_PRODUCT_KEY].ToString());
                            if (!Convert.ToString(hashTable[POR_PRODUCT.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [POR_PRODUCT]
                                                            ([PRODUCT_KEY],[PRODUCT_CODE],[PRODUCT_NAME],[QUANTITY],[MAXPOWER],[MINPOWER]
                                                            ,[PRO_TEST_RULE],[PROMODEL_NAME],[CODEMARK],[CUSTMARK],[LABELTYPE],[LABELVAR],[LABELCHECK]
                                                            ,[PRO_LEVEL],[SHIP_QTY],[SAP_PN],[CERTIFICATION],[TOLERANCE],[JUNCTION_BOX],[CALIBRATION_TYPE]
                                                            ,[CALIBRATION_CYCLE],[FIX_CYCLE],[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG],[MEMO1],CONSTANT_TEMPERTATURE_CYCLE,[ROUTE_ENTERPRISE_VER_KEY],[ENTERPRISE_NAME]
                                                            ,[ROUTE_ROUTE_VER_KEY] ,[ROUTE_NAME],[ROUTE_STEP_KEY],[ROUTE_STEP_NAME],[ISKINGLING],[CELL_SIZE])
                                                            SELECT '{0}',[PRODUCT_CODE],[PRODUCT_NAME],[QUANTITY],[MAXPOWER],[MINPOWER]
                                                            ,[PRO_TEST_RULE],[PROMODEL_NAME],[CODEMARK],[CUSTMARK],[LABELTYPE],[LABELVAR],[LABELCHECK]
                                                            ,[PRO_LEVEL],[SHIP_QTY],[SAP_PN],[CERTIFICATION],[TOLERANCE],[JUNCTION_BOX],[CALIBRATION_TYPE]
                                                            ,[CALIBRATION_CYCLE],[FIX_CYCLE],[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0,[MEMO1],CONSTANT_TEMPERTATURE_CYCLE,[ROUTE_ENTERPRISE_VER_KEY],[ENTERPRISE_NAME]
                                                            ,[ROUTE_ROUTE_VER_KEY] ,[ROUTE_NAME],[ROUTE_STEP_KEY],[ROUTE_STEP_NAME],[ISKINGLING],[CELL_SIZE]
                                                            FROM [POR_PRODUCT] WHERE PRODUCT_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[POR_PRODUCT.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[POR_PRODUCT.FIELDS_PRODUCT_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            hashTable.Remove(POR_PRODUCT.FIELDS_PRODUCT_KEY);
                            hashTable[POR_PRODUCT.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(porProduct_fields, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    } 
                    //--------------------------------------------------------------------------------------------------------------

                    if (dtPorProduct_Dtl_Insert != null && dtPorProduct_Dtl_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPorProduct_Dtl_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(porProduct_dtl_fields, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtPorProduct_Dtl_Update != null && dtPorProduct_Dtl_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPorProduct_Dtl_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY, hashTable[POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY].ToString());
                            if (!Convert.ToString(hashTable[POR_PRODUCT_DTL.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [POR_PRODUCT_DTL]
                                                            ([PRODUCT_DTL_KEY],[PRODUCT_KEY],[PRODUCT_GRADE],[PRODUCT_NAME]
                                                            ,[SAP_PN],[ISMAIN],[ORDER_INDEX],[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                            SELECT '{0}',[PRODUCT_KEY],[PRODUCT_GRADE],[PRODUCT_NAME]
                                                            ,[SAP_PN],[ISMAIN],[ORDER_INDEX],[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0
                                                            FROM [POR_PRODUCT_DTL] WHERE PRODUCT_DTL_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[POR_PRODUCT_DTL.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            hashTable.Remove(POR_PRODUCT_DTL.FIELDS_PRODUCT_DTL_KEY);
                            hashTable[POR_PRODUCT_DTL.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(porProduct_dtl_fields, hashTable, wc);
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
                    LogService.LogError("SavePorProductData Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        public DataSet GetProductDtlGrade(string proid)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT t.PRODUCT_CODE,t1.ISMAIN,t1.PRODUCT_GRADE
                                                FROM POR_PRODUCT t 
                                                INNER JOIN POR_PRODUCT_DTL t1 ON t.PRODUCT_KEY=t1.PRODUCT_KEY
                                                WHERE t.ISFLAG=1 and t1.ISFLAG=1
                                                AND t.PRODUCT_CODE='{0}'
                                                AND t1.ISMAIN=1", 
                                                proid.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = POR_PRODUCT.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetProductDtlGrade Error: " + ex.Message);
            }
            return dsReturn;
        }



        #region IPorProductEngine 成员

        /// <summary>
        /// 根据序列号获取金刚线
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        public DataSet GetKingLineInf(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT C.PRODUCT_KEY,C.PRODUCT_CODE,C.PRODUCT_NAME,C.ISKINGLING,D.* FROM POR_PRODUCT C INNER JOIN (
                                                            SELECT A.LOT_KEY,A.LOT_NUMBER,A.WORK_ORDER_KEY,WORK_ORDER_NO,B.PRODUCT_CODE,B.PRODUCT_KEY FROM POR_LOT A 
                                                            INNER JOIN dbo.POR_WO_PRD B ON A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                            AND DELETED_TERM_FLAG = 0
                                                            AND LOT_NUMBER = '{0}'
                                                            AND IS_USED = 'Y'
                                                            AND IS_MAIN ='Y') D
                                                            ON C.PRODUCT_KEY = D.PRODUCT_KEY
                                                            WHERE C.ISFLAG = 1 
                                                            ",
                                                lotNumber.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetKingLineInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}

