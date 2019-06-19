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
    public class BasicPowerSetEngine : AbstractEngine, IBasicPowerSetEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BasicPowerSetEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 获取分档设置数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        /// genchille.yang 2012-10-30 12:15
        public DataSet GetPowerSetData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_POWERSET _powerSet = new BASE_POWERSET();
                BASE_POWERSET_DETAIL _powerSet_Dtl = new BASE_POWERSET_DETAIL();
                BASE_POWERSET_COLORATCNO _powerSet_Color_Dtl = new BASE_POWERSET_COLORATCNO();
                Conditions _conditions = new Conditions();
                if (hstable.ContainsKey(BASE_POWERSET.FIELDS_PS_CODE))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_PS_CODE, DatabaseCompareOperator.Equal, hstable[BASE_POWERSET.FIELDS_PS_CODE].ToString());
                if (hstable.ContainsKey(BASE_POWERSET.FIELDS_PS_RULE))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_PS_RULE, DatabaseCompareOperator.Like, hstable[BASE_POWERSET.FIELDS_PS_RULE].ToString() + "%");
                if (hstable.ContainsKey(BASE_POWERSET.FIELDS_P_MIN) && hstable.ContainsKey(BASE_POWERSET.FIELDS_P_MAX))
                {
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_P_MIN, DatabaseCompareOperator.GreaterThanEqual, hstable[BASE_POWERSET.FIELDS_P_MIN].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_P_MAX, DatabaseCompareOperator.LessThanEqual, hstable[BASE_POWERSET.FIELDS_P_MAX].ToString());
                }
                else if (hstable.ContainsKey(BASE_POWERSET.FIELDS_P_MIN))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_P_MIN, DatabaseCompareOperator.GreaterThan, hstable[BASE_POWERSET.FIELDS_P_MIN].ToString());
                else if (hstable.ContainsKey(BASE_POWERSET.FIELDS_P_MAX))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_P_MAX, DatabaseCompareOperator.LessThan, hstable[BASE_POWERSET.FIELDS_P_MAX].ToString());
                if (hstable.ContainsKey(BASE_POWERSET.FIELDS_SUB_PS_WAY) && !string.IsNullOrEmpty(hstable[BASE_POWERSET.FIELDS_SUB_PS_WAY].ToString()))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_SUB_PS_WAY, DatabaseCompareOperator.NotEqual, hstable[BASE_POWERSET.FIELDS_SUB_PS_WAY].ToString());

                _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_powerSet, null, _conditions);
                sqlCommand += " order by PS_CODE asc,PS_SEQ asc";
                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSet.Columns.Add(BASE_POWERSET.FIELDS_ISNEW);
                dtPowerSet.TableName = BASE_POWERSET.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_DETAIL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_powerSet_Dtl, null, _conditions);
                DataTable dtPowerSetDtl = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSetDtl.Columns.Add(BASE_POWERSET_DETAIL.FIELDS_ISNEW);
                dtPowerSetDtl.TableName = BASE_POWERSET_DETAIL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSetDtl, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_COLORATCNO.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_powerSet_Color_Dtl, null, _conditions);
                DataTable dtPowerSetColorDtl = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSetColorDtl.Columns.Add(BASE_POWERSET_COLORATCNO.FIELDS_ISNEW);
                dtPowerSetColorDtl.TableName = BASE_POWERSET_COLORATCNO.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSetColorDtl, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerSetData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet IsExistPowerDtlData(DataTable dtInsertPowerDtlData)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_POWERSET_DETAIL _powersetdtl = new BASE_POWERSET_DETAIL();                
                foreach (DataRow dr in dtInsertPowerDtlData.Rows)
                {
                    Conditions _conditions = new Conditions();
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY, DatabaseCompareOperator.Equal, dr[BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE, DatabaseCompareOperator.Equal, dr[BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_DETAIL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                    sqlCommand = DatabaseTable.BuildQuerySqlStatement(_powersetdtl, null, _conditions);
                    DataTable dtProModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (dtProModel.Rows.Count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("子分档分档代码{0}已经存在,不能重复!", dr[BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE].ToString()));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistPowerSetData Error: " + ex.Message);
            }
            return dsReturn;
        }
        public DataSet IsExistPowerDtlColorData(DataTable dtInsertPowerDtlColorData)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_POWERSET_COLORATCNO _powersetdtlColor = new BASE_POWERSET_COLORATCNO();
                foreach (DataRow dr in dtInsertPowerDtlColorData.Rows)
                {
                    Conditions _conditions = new Conditions();
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY, DatabaseCompareOperator.Equal, dr[BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_COLORATCNO.FIELDS_COLOR_CODE, DatabaseCompareOperator.Equal, dr[BASE_POWERSET_COLORATCNO.FIELDS_COLOR_CODE].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET_DETAIL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                    sqlCommand = DatabaseTable.BuildQuerySqlStatement(_powersetdtlColor, null, _conditions);
                    DataTable dtProModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (dtProModel.Rows.Count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("花色代码{0}已经存在,不能重复!", dr[BASE_POWERSET_COLORATCNO.FIELDS_COLOR_CODE].ToString()));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistPowerSetData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 校验分档数据是否存在
        /// </summary>
        /// <param name="dtInsertPowerSetData"></param>
        /// <returns></returns>
        public DataSet IsExistPowerSetData(DataTable dtInsertPowerSetData)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_POWERSET _powerSet = new BASE_POWERSET();
                foreach (DataRow dr in dtInsertPowerSetData.Rows)
                {
                    Conditions _conditions = new Conditions();
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_PS_CODE, DatabaseCompareOperator.Equal, dr[BASE_POWERSET.FIELDS_PS_CODE].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_PS_SEQ, DatabaseCompareOperator.Equal, dr[BASE_POWERSET.FIELDS_PS_SEQ].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_POWERSET.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                    sqlCommand = DatabaseTable.BuildQuerySqlStatement(_powerSet, null, _conditions);
                    DataTable dtProModel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (dtProModel.Rows.Count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("分档代码{0}及序号{1}已经存在,不能重复!", dr[BASE_POWERSET.FIELDS_PS_CODE].ToString(), dr[BASE_POWERSET.FIELDS_PS_SEQ].ToString()));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("IsExistPowerSetData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存分档设置数据
        /// </summary>
        /// <param name="dsPowerSet"></param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-10-30 17:51
        public DataSet SavePowerSetData(DataSet dsPowerSet)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtPowerSet_Update = null, dtPowerSet_Insert = null;
            DataTable dtPowerSetDtl_Update = null, dtPowerSetDtl_Insert = null;
            DataTable dtPowerSetColorDtl_Update = null, dtPowerSetColorDtl_Insert = null;
            List<string> sqlCommandList = new List<string>();
            if (dsPowerSet.Tables.Contains(BASE_POWERSET.DATABASE_TABLE_FORINSERT))
            {
                dtPowerSet_Insert = dsPowerSet.Tables[BASE_POWERSET.DATABASE_TABLE_FORINSERT];
                if (dtPowerSet_Insert.Columns.Contains(BASE_POWERSET.FIELDS_ISNEW))
                    dtPowerSet_Insert.Columns.Remove(BASE_POWERSET.FIELDS_ISNEW);
            }
            if (dsPowerSet.Tables.Contains(BASE_POWERSET.DATABASE_TABLE_FORUPDATE))
            {
                dtPowerSet_Update = dsPowerSet.Tables[BASE_POWERSET.DATABASE_TABLE_FORUPDATE];
                if (dtPowerSet_Update.Columns.Contains(BASE_POWERSET.FIELDS_ISNEW))
                    dtPowerSet_Update.Columns.Remove(BASE_POWERSET.FIELDS_ISNEW);
            }
            //-----------------------------------------------------------------------
            if (dsPowerSet.Tables.Contains(BASE_POWERSET_DETAIL.DATABASE_TABLE_FORINSERT))
            {
                dtPowerSetDtl_Insert = dsPowerSet.Tables[BASE_POWERSET_DETAIL.DATABASE_TABLE_FORINSERT];
                if (dtPowerSetDtl_Insert.Columns.Contains(BASE_POWERSET_DETAIL.FIELDS_ISNEW))
                    dtPowerSetDtl_Insert.Columns.Remove(BASE_POWERSET_DETAIL.FIELDS_ISNEW);
            }
            if (dsPowerSet.Tables.Contains(BASE_POWERSET_DETAIL.DATABASE_TABLE_FORUPDATE))
            {
                dtPowerSetDtl_Update = dsPowerSet.Tables[BASE_POWERSET_DETAIL.DATABASE_TABLE_FORUPDATE];
                if (dtPowerSetDtl_Update.Columns.Contains(BASE_POWERSET_DETAIL.FIELDS_ISNEW))
                    dtPowerSetDtl_Update.Columns.Remove(BASE_POWERSET_DETAIL.FIELDS_ISNEW);
            }
            //-------------------------------------------------------------------------------
            if (dsPowerSet.Tables.Contains(BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORINSERT))
            {
                dtPowerSetColorDtl_Insert = dsPowerSet.Tables[BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORINSERT];
                if (dtPowerSetColorDtl_Insert.Columns.Contains(BASE_POWERSET_COLORATCNO.FIELDS_ISNEW))
                    dtPowerSetColorDtl_Insert.Columns.Remove(BASE_POWERSET_COLORATCNO.FIELDS_ISNEW);
            }
            if (dsPowerSet.Tables.Contains(BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORUPDATE))
            {
                dtPowerSetColorDtl_Update = dsPowerSet.Tables[BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORUPDATE];
                if (dtPowerSetColorDtl_Update.Columns.Contains(BASE_POWERSET_COLORATCNO.FIELDS_ISNEW))
                    dtPowerSetColorDtl_Update.Columns.Remove(BASE_POWERSET_COLORATCNO.FIELDS_ISNEW);
            }
                      
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                BASE_POWERSET basepowerSet = new BASE_POWERSET();
                BASE_POWERSET_DETAIL basepowerSet_Dtl = new BASE_POWERSET_DETAIL();
                BASE_POWERSET_COLORATCNO basepowerSetColor_Dtl = new BASE_POWERSET_COLORATCNO();
                try
                {
                    if (dtPowerSet_Insert != null && dtPowerSet_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerSet_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(basepowerSet, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtPowerSet_Update != null && dtPowerSet_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerSet_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_POWERSET.FIELDS_POWERSET_KEY, hashTable[BASE_POWERSET.FIELDS_POWERSET_KEY].ToString());
                            //new history data
                            if (!Convert.ToString(hashTable[BASE_POWERSET.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_POWERSET](
                                                        [POWERSET_KEY] ,[PS_CODE],[PS_RULE]
                                                        ,[PS_SEQ],[P_MIN],[P_MAX],[MODULE_NAME]
                                                        ,[PMAXSTAB],[ISCSTAB],[VOCSTAB],[IMPPSTAB]
                                                        ,[VMPPSTAB],[FUSE],[PS_SUBCODE],[PS_SUBCODE_DESC]
                                                        ,[SUB_PS_WAY],[POWER_DIFFERENCE],[CREATER]
                                                        ,[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG]
                                                        ,[PROMODEL_KEY])
                                                        SELECT '{0}' ,[PS_CODE],[PS_RULE]
                                                        ,[PS_SEQ],[P_MIN],[P_MAX],[MODULE_NAME]
                                                        ,[PMAXSTAB],[ISCSTAB],[VOCSTAB],[IMPPSTAB]
                                                        ,[VMPPSTAB],[FUSE],[PS_SUBCODE],[PS_SUBCODE_DESC]
                                                        ,[SUB_PS_WAY],[POWER_DIFFERENCE],[CREATER]
                                                        ,[CREATE_TIME],'{1}',[EDIT_TIME],0
                                                        ,[PROMODEL_KEY]
                                                          FROM [BASE_POWERSET]
                                                          WHERE POWERSET_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                      Convert.ToString(hashTable[BASE_POWERSET.FIELDS_EDITOR]),                                                                                
                                                                                      Convert.ToString(hashTable[BASE_POWERSET.FIELDS_POWERSET_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }

                            hashTable.Remove(BASE_POWERSET.FIELDS_POWERSET_KEY);
                            hashTable[BASE_POWERSET.FIELDS_EDIT_TIME] = string.Empty;

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(basepowerSet, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    //-----------------------------------------------------------------------------------------
                    if (dtPowerSetDtl_Insert != null && dtPowerSetDtl_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerSetDtl_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(basepowerSet_Dtl, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtPowerSetDtl_Update != null && dtPowerSetDtl_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerSetDtl_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL, hashTable[BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL].ToString());
                            //new history data
                            if (!Convert.ToString(hashTable[BASE_POWERSET_DETAIL.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_POWERSET_DETAIL]
                                                        ([POWERSET_KEY_DTL],[POWERSET_KEY]
                                                        ,[PS_DTL_SUBCODE],[POWERLEVEL]
                                                        ,[P_DTL_MIN],[P_DTL_MAX],[EDIT_TIME]
                                                        ,[ISFLAG],[CREATER],[CREATE_TIME],[EDITOR])
                                                        SELECT '{0}',[POWERSET_KEY]
                                                        ,[PS_DTL_SUBCODE],[POWERLEVEL]
                                                        ,[P_DTL_MIN],[P_DTL_MAX],[EDIT_TIME]
                                                        ,0,[CREATER],[CREATE_TIME],'{1}'
                                                          FROM [BASE_POWERSET_DETAIL]
                                                          WHERE POWERSET_KEY_DTL='{2}' ", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),                                                                                           
                                                                                            Convert.ToString(hashTable[BASE_POWERSET_DETAIL.FIELDS_EDITER]),
                                                                                            Convert.ToString(hashTable[BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }

                            hashTable.Remove(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL);
                            hashTable[BASE_POWERSET_DETAIL.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(basepowerSet_Dtl, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    //-----------------------------------------------------------------------------------------
                    if (dtPowerSetColorDtl_Insert != null && dtPowerSetColorDtl_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerSetColorDtl_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(basepowerSetColor_Dtl, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if (dtPowerSetColorDtl_Update != null && dtPowerSetColorDtl_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerSetColorDtl_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC, hashTable[BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC].ToString());
                            //new history data
                            if (!Convert.ToString(hashTable[BASE_POWERSET_COLORATCNO.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"insert into BASE_POWERSET_COLORATCNO
                                                        ([POWERSET_KEY_ATC],[POWERSET_KEY]
                                                        ,[COLOR_CODE],[COLOR_NAME],[ARTICNO]
                                                        ,[DESCRIPTION],[PRO_LEVEL],[CREATER]
                                                        ,[CREATE_TIME],[EDITER],[EDIT_TIME],[ISFLAG])  
                                                        SELECT '{0}',[POWERSET_KEY]
                                                        ,[COLOR_CODE],[COLOR_NAME],[ARTICNO]
                                                        ,[DESCRIPTION],[PRO_LEVEL],[CREATER]
                                                        ,[CREATE_TIME],'{1}',[EDIT_TIME],0
                                                        FROM [BASE_POWERSET_COLORATCNO]
                                                        where POWERSET_KEY_ATC='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                         Convert.ToString(hashTable[BASE_POWERSET_COLORATCNO.FIELDS_EDITER]),                                                                                  
                                                                                         Convert.ToString(hashTable[BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }

                            hashTable.Remove(BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC);
                            hashTable[BASE_POWERSET_COLORATCNO.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(basepowerSetColor_Dtl, hashTable, wc);
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
                    LogService.LogError("SavePowerSetData Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        public DataSet GetPowerSetDtl(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"select t2.PS_CODE+':'+t2.PS_RULE PS_RULE1,
                                CONVERT(varchar(2),t2.PS_SEQ)+':'+t2.MODULE_NAME MODULE_NAME1,
                                t2.*
                                from BASE_POWERSET t2
                                where t2.ISFLAG=1";
                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSet.TableName = BASE_POWERSET.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);
                sqlCommand = @"select t1.*
                               from  BASE_POWERSET_DETAIL t1 
                               where  t1.ISFLAG=1";
                DataTable dtPowerSetDtl = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSetDtl.TableName = BASE_POWERSET_DETAIL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSetDtl, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerSetDtl Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetPowerLevelByLotNum(Hashtable hsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = string.Format(@"SELECT a.PS_SUBCODE
                                            FROM POR_WO_PRD_PS a
                                            INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                            INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY AND c.PART_NUMBER=a.PART_NUMBER
                                            INNER JOIN WIP_IV_TEST d ON d.LOT_NUM=c.LOT_NUMBER AND d.VC_DEFAULT='1'
                                            WHERE a.IS_USED='Y'
                                            AND c.LOT_NUMBER='{0}'
                                            AND a.PS_CODE=d.VC_TYPE
                                            and A.PS_SEQ=d.I_IDE
                                            --AND a.P_MIN<=d.COEF_PMAX
                                            --AND a.P_MAX>d.COEF_PMAX
                                            ORDER BY PS_SEQ",
                                            hsParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtPowerSet.TableName = BASE_POWERSET.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);             
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerLevelByLotNum Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得自定义配置数据信息，主要是工艺基础设定作业模组在使用。
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        /// 该方法已经不再使用，目前已使用通用的方法替代
        public DataSet GetBasicPowerSetEngine_CommonData(string strFilter)
        {
            DataSet dsReturn = new DataSet();
            try
            {               
                string sqlCommon = string.Empty;
                string sf = "1=1 ";
                sqlCommon = @"SELECT A.* from (SELECT T.ITEM_ORDER,
                MAX( case T.ATTRIBUTE_NAME when 'Column_Name' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as Column_Name,
                MAX( case T.ATTRIBUTE_NAME when 'Column_type' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as Column_type,
                MAX( case T.ATTRIBUTE_NAME when 'Column_code' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as Column_code
                FROM CRM_ATTRIBUTE           T,
                BASE_ATTRIBUTE          T1,
                BASE_ATTRIBUTE_CATEGORY T2
                WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY
                AND T1.CATEGORY_KEY = T2.CATEGORY_KEY
                AND UPPER(T2.CATEGORY_NAME) = 'BASIC_TESTRULE_POWERSET'
                GROUP BY T.ITEM_ORDER  
                ) A WHERE {0}  ORDER BY A.ITEM_ORDER ASC";
                if (!string.IsNullOrEmpty(strFilter))
                    sf = string.Format(" AND UPPER(A.Column_type)='{0}'", strFilter);
                sqlCommon = string.Format(sqlCommon, sf);

                DataTable dtReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtReturn.TableName = BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME;

                dsReturn.Merge(dtReturn, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBasicPowerSetEngine_CommonData Error: " + ex.Message);
            }

            return dsReturn;
        }

     
    }
}

