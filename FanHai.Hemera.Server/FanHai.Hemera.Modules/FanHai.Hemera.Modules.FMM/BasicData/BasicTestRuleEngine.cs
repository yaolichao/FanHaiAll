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
    /// 测试规则设置操作类
    /// </summary>
    public class BasicTestRuleEngine : AbstractEngine, IBasicTestRuleEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BasicTestRuleEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
      
        /// <summary>
        /// 获得测试规则主要信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        /// owner genchille.yang 2012-10-31 10:00:01
        public DataSet GetTestRuleMainData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                BASE_TESTRULE _baseTestRule = new BASE_TESTRULE();
                BASE_TESTRULE_DECAY _baseTestRuleDecay = new BASE_TESTRULE_DECAY();            
                Conditions _conditions = new Conditions();
                if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_TESTRULE_CODE))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_TESTRULE_CODE, DatabaseCompareOperator.Equal, hstable[BASE_TESTRULE.FIELDS_TESTRULE_CODE].ToString());
                if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_TESTRULE_NAME))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_TESTRULE_NAME, DatabaseCompareOperator.Like, hstable[BASE_TESTRULE.FIELDS_TESTRULE_NAME].ToString() + "%");

                if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_CREATE_TIME_START) && hstable.ContainsKey(BASE_TESTRULE.FIELDS_CREATE_TIME_END))
                {
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_CREATE_TIME, DatabaseCompareOperator.GreaterThanEqual, hstable[BASE_TESTRULE.FIELDS_CREATE_TIME].ToString());
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_CREATE_TIME, DatabaseCompareOperator.LessThanEqual, hstable[BASE_TESTRULE.FIELDS_CREATE_TIME].ToString());
                }
                else if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_CREATE_TIME_START))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_CREATE_TIME, DatabaseCompareOperator.GreaterThanEqual, hstable[BASE_TESTRULE.FIELDS_CREATE_TIME].ToString());
                else if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_CREATE_TIME_END))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_CREATE_TIME, DatabaseCompareOperator.LessThanEqual, hstable[BASE_TESTRULE.FIELDS_CREATE_TIME].ToString());

                if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_PS_CODE) && !string.IsNullOrEmpty(hstable[BASE_TESTRULE.FIELDS_PS_CODE].ToString()))
                    _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_PS_CODE, DatabaseCompareOperator.Like, hstable[BASE_TESTRULE.FIELDS_PS_CODE].ToString() + "%");

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule, null, _conditions);
                DataTable dtTestRule = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule.Columns.Add(BASE_TESTRULE.FIELDS_ISNEW);
                dtTestRule.TableName = BASE_TESTRULE.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule, true, MissingSchemaAction.Add);

                _conditions.RemoveAll();
                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_DECAY.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRuleDecay, null, _conditions);
                DataTable dtTestRuleDecay = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRuleDecay.Columns.Add(BASE_TESTRULE_DECAY.FIELDS_ISNEW);
                dtTestRuleDecay.TableName = BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRuleDecay, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRuleMainData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得测试规则明细信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        /// owner genchille.yang 2012-10-31 10:05:27
        public DataSet GetTestRuleDeatilData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                string testRulekey = string.Empty;
                BASE_TESTRULE _baseTestRule = new BASE_TESTRULE();
                BASE_TESTRULE_CTLPARA _baseTestRule_ControlPara = new BASE_TESTRULE_CTLPARA();
                BASE_TESTRULE_AVGPOWER _baseTestRule_AvgPower = new BASE_TESTRULE_AVGPOWER();
                BASE_TESTRULE_POWERCTL _baseTestRule_PowerControl = new BASE_TESTRULE_POWERCTL();
                BASE_TESTRULE_PROLEVEL _baseTestRule_ProductLevel = new BASE_TESTRULE_PROLEVEL();
                BASE_TESTRULE_DECAY _baseTestRuleDecay = new BASE_TESTRULE_DECAY();
                BASE_TESTRULE_PRINTSET _baseTestRule_PrintSet = new BASE_TESTRULE_PRINTSET();
                Conditions _conditions = new Conditions();
                if (hstable.ContainsKey(BASE_TESTRULE.FIELDS_TESTRULE_KEY))
                    testRulekey = hstable[BASE_TESTRULE.FIELDS_TESTRULE_KEY].ToString().Trim();
                //------------------------------------------------------------------------------------------------

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule, null, _conditions);
                DataTable dtTestRule = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule.Columns.Add(BASE_TESTRULE.FIELDS_ISNEW);
                dtTestRule.TableName = BASE_TESTRULE.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule, true, MissingSchemaAction.Add);
                //控制参数
                _conditions.RemoveAll();

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_CTLPARA.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);
                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_CTLPARA.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule_ControlPara, null, _conditions);
                DataTable dtTestRule_ControlPara = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule_ControlPara.Columns.Add(BASE_TESTRULE_CTLPARA.FIELDS_ISNEW);
                dtTestRule_ControlPara.TableName = BASE_TESTRULE_CTLPARA.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule_ControlPara, true, MissingSchemaAction.Add);
                //平均功率
                _conditions.RemoveAll();

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_AVGPOWER.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);
                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_AVGPOWER.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule_AvgPower, null, _conditions);
                DataTable dtTestRule_AvgPower = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule_AvgPower.Columns.Add(BASE_TESTRULE_AVGPOWER.FIELDS_ISNEW);
                dtTestRule_AvgPower.TableName = BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule_AvgPower, true, MissingSchemaAction.Add);
                //功率控制
                _conditions.RemoveAll();

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_POWERCTL.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);
                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_POWERCTL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule_PowerControl, null, _conditions);
                DataTable dtTestRule_PowerControl = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule_PowerControl.Columns.Add(BASE_TESTRULE_POWERCTL.FIELDS_ISNEW);
                dtTestRule_PowerControl.TableName = BASE_TESTRULE_POWERCTL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule_PowerControl, true, MissingSchemaAction.Add);
                //产品等级
                _conditions.RemoveAll();

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_PROLEVEL.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);
                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_PROLEVEL.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule_ProductLevel, null, _conditions);
                DataTable dtTestRule_ProductLevel = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule_ProductLevel.Columns.Add(BASE_TESTRULE_PROLEVEL.FIELDS_ISNEW);
                dtTestRule_ProductLevel.TableName = BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule_ProductLevel, true, MissingSchemaAction.Add);
                //衰减设置
                _conditions.RemoveAll();

                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_DECAY.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);
                _conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_DECAY.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRuleDecay, null, _conditions);
                DataTable dtTestRuleDecay = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRuleDecay.Columns.Add(BASE_TESTRULE_DECAY.FIELDS_ISNEW);
                dtTestRuleDecay.TableName = BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRuleDecay, true, MissingSchemaAction.Add);
                //铭牌标签打印
                _conditions.RemoveAll();

                //_conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_PRINTSET.FIELDS_TESTRULE_KEY, DatabaseCompareOperator.Equal, testRulekey);
                //_conditions.Add(DatabaseLogicOperator.And, BASE_TESTRULE_PRINTSET.FIELDS_ISFLAG, DatabaseCompareOperator.Equal, "1");
                //sqlCommand = DatabaseTable.BuildQuerySqlStatement(_baseTestRule_PrintSet, null, _conditions);

                sqlCommand = string.Format(@"SELECT  A.* FROM BASE_TESTRULE_PRINTSET A
                                             INNER JOIN BASE_TESTRULE_DECAY B ON A.DECAY_KEY = B.DECAY_KEY 
                                             WHERE  1 = 1 
                                             AND A.ISFLAG = 1
                                             AND B.ISFLAG = 1
                                             AND B.TESTRULE_KEY = '{0}';", testRulekey);

                DataTable dtTestRule_PrintSet = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtTestRule_PrintSet.Columns.Add(BASE_TESTRULE_PRINTSET.FIELDS_ISNEW);
                dtTestRule_PrintSet.TableName = BASE_TESTRULE_PRINTSET.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTestRule_PrintSet, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTestRuleMainData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet SaveTestRuleAllData(DataSet dsSave)
        {
            #region
            DataSet dsReturn = new DataSet();
            DataTable dtTestRule_Update = null, dtTestRule_Insert = null;
            DataTable dtControlPara_Update = null, dtControlPara_Insert = null;
            DataTable dtAvgPower_Update = null, dtAvgPower_Insert = null;
            DataTable dtPowerControl_Update = null, dtPowerControl_Insert = null;
            DataTable dtProductLevel_Update = null, dtProductLevel_Insert = null;
            DataTable dtDecay_Update = null, dtDecay_Insert = null;
            DataTable dtPrintSet_Update = null, dtPrintSet_Insert = null;
            #endregion
            #region insert table
            if (dsSave.Tables.Contains(BASE_TESTRULE.DATABASE_TABLE_FORINSERT))
            {
                dtTestRule_Insert = dsSave.Tables[BASE_TESTRULE.DATABASE_TABLE_FORINSERT];
                if (dtTestRule_Insert.Columns.Contains(BASE_TESTRULE.FIELDS_ISNEW))
                    dtTestRule_Insert.Columns.Remove(BASE_TESTRULE.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORINSERT))
            {
                dtControlPara_Insert = dsSave.Tables[BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORINSERT];
                if (dtControlPara_Insert.Columns.Contains(BASE_TESTRULE_CTLPARA.FIELDS_ISNEW))
                    dtControlPara_Insert.Columns.Remove(BASE_TESTRULE_CTLPARA.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORINSERT))
            {
                dtAvgPower_Insert = dsSave.Tables[BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORINSERT];
                if (dtAvgPower_Insert.Columns.Contains(BASE_TESTRULE_AVGPOWER.FIELDS_ISNEW))
                    dtAvgPower_Insert.Columns.Remove(BASE_TESTRULE_AVGPOWER.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORINSERT))
            {
                dtPowerControl_Insert = dsSave.Tables[BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORINSERT];
                if (dtPowerControl_Insert.Columns.Contains(BASE_TESTRULE_POWERCTL.FIELDS_ISNEW))
                    dtPowerControl_Insert.Columns.Remove(BASE_TESTRULE_POWERCTL.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORINSERT))
            {
                dtProductLevel_Insert = dsSave.Tables[BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORINSERT];
                if (dtProductLevel_Insert.Columns.Contains(BASE_TESTRULE_PROLEVEL.FIELDS_ISNEW))
                    dtProductLevel_Insert.Columns.Remove(BASE_TESTRULE_PROLEVEL.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_DECAY.DATABASE_TABLE_FORINSERT))
            {
                dtDecay_Insert = dsSave.Tables[BASE_TESTRULE_DECAY.DATABASE_TABLE_FORINSERT];
                if (dtDecay_Insert.Columns.Contains(BASE_TESTRULE_DECAY.FIELDS_ISNEW))
                    dtDecay_Insert.Columns.Remove(BASE_TESTRULE_DECAY.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORINSERT))
            {
                dtPrintSet_Insert = dsSave.Tables[BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORINSERT];
                if (dtPrintSet_Insert.Columns.Contains(BASE_TESTRULE_PRINTSET.FIELDS_ISNEW))
                    dtPrintSet_Insert.Columns.Remove(BASE_TESTRULE_PRINTSET.FIELDS_ISNEW);
            }
            #endregion

            #region update table
            if (dsSave.Tables.Contains(BASE_TESTRULE.DATABASE_TABLE_FORUPDATE))
            {
                dtTestRule_Update = dsSave.Tables[BASE_TESTRULE.DATABASE_TABLE_FORUPDATE];
                if (dtTestRule_Update.Columns.Contains(BASE_TESTRULE.FIELDS_ISNEW))
                    dtTestRule_Update.Columns.Remove(BASE_TESTRULE.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORUPDATE))
            {
                dtControlPara_Update = dsSave.Tables[BASE_TESTRULE_CTLPARA.DATABASE_TABLE_FORUPDATE];
                if (dtControlPara_Update.Columns.Contains(BASE_TESTRULE_CTLPARA.FIELDS_ISNEW))
                    dtControlPara_Update.Columns.Remove(BASE_TESTRULE_CTLPARA.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORUPDATE))
            {
                dtAvgPower_Update = dsSave.Tables[BASE_TESTRULE_AVGPOWER.DATABASE_TABLE_FORUPDATE];
                if (dtAvgPower_Update.Columns.Contains(BASE_TESTRULE_AVGPOWER.FIELDS_ISNEW))
                    dtAvgPower_Update.Columns.Remove(BASE_TESTRULE_AVGPOWER.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORUPDATE))
            {
                dtPowerControl_Update = dsSave.Tables[BASE_TESTRULE_POWERCTL.DATABASE_TABLE_FORUPDATE];
                if (dtPowerControl_Update.Columns.Contains(BASE_TESTRULE_POWERCTL.FIELDS_ISNEW))
                    dtPowerControl_Update.Columns.Remove(BASE_TESTRULE_POWERCTL.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORUPDATE))
            {
                dtProductLevel_Update = dsSave.Tables[BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_FORUPDATE];
                if (dtProductLevel_Update.Columns.Contains(BASE_TESTRULE_PROLEVEL.FIELDS_ISNEW))
                    dtProductLevel_Update.Columns.Remove(BASE_TESTRULE_PROLEVEL.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_DECAY.DATABASE_TABLE_FORUPDATE))
            {
                dtDecay_Update = dsSave.Tables[BASE_TESTRULE_DECAY.DATABASE_TABLE_FORUPDATE];
                if (dtDecay_Update.Columns.Contains(BASE_TESTRULE_DECAY.FIELDS_ISNEW))
                    dtDecay_Update.Columns.Remove(BASE_TESTRULE_DECAY.FIELDS_ISNEW);
            }
            if (dsSave.Tables.Contains(BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORUPDATE))
            {
                dtPrintSet_Update = dsSave.Tables[BASE_TESTRULE_PRINTSET.DATABASE_TABLE_FORUPDATE];
                if (dtPrintSet_Update.Columns.Contains(BASE_TESTRULE_PRINTSET.FIELDS_ISNEW))
                    dtPrintSet_Update.Columns.Remove(BASE_TESTRULE_PRINTSET.FIELDS_ISNEW);
            }
            #endregion
            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                BASE_TESTRULE baseTestRule = new BASE_TESTRULE();
                BASE_TESTRULE_CTLPARA base_controlPara = new BASE_TESTRULE_CTLPARA();
                BASE_TESTRULE_AVGPOWER base_avgpower = new BASE_TESTRULE_AVGPOWER();
                BASE_TESTRULE_POWERCTL base_powercontrol = new BASE_TESTRULE_POWERCTL();
                BASE_TESTRULE_PROLEVEL base_productlevel = new BASE_TESTRULE_PROLEVEL();
                BASE_TESTRULE_DECAY base_decay = new BASE_TESTRULE_DECAY();
                BASE_TESTRULE_PRINTSET base_printset = new BASE_TESTRULE_PRINTSET();

                try
                {
                    #region insert operation
                    if (dtTestRule_Insert != null && dtTestRule_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtTestRule_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(baseTestRule, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtControlPara_Insert != null && dtControlPara_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtControlPara_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(base_controlPara, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtAvgPower_Insert != null && dtAvgPower_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAvgPower_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(base_avgpower, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtPowerControl_Insert != null && dtPowerControl_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerControl_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(base_powercontrol, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProductLevel_Insert != null && dtProductLevel_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProductLevel_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(base_productlevel, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtDecay_Insert != null && dtDecay_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDecay_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(base_decay, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtPrintSet_Insert != null && dtPrintSet_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPrintSet_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(base_printset, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region update operation
                    if (dtTestRule_Update != null && dtTestRule_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtTestRule_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE.FIELDS_TESTRULE_KEY, hashTable[BASE_TESTRULE.FIELDS_TESTRULE_KEY].ToString());

                            if (!Convert.ToString(hashTable[BASE_TESTRULE.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE]
                                                        ([TESTRULE_KEY],[TESTRULE_CODE],[TESTRULE_NAME]
                                                        ,[MEMO],[PS_CODE],[LAST_TEST_TYPE],[POWER_DEGREE]
                                                        ,[FULL_PALLET_QTY],[CREATER],[CREATE_TIME]
                                                        ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                        SELECT '{0}',[TESTRULE_CODE],[TESTRULE_NAME]
                                                        ,[MEMO],[PS_CODE],[LAST_TEST_TYPE],[POWER_DEGREE]
                                                        ,[FULL_PALLET_QTY],[CREATER],[CREATE_TIME]
                                                        ,'{1}',[EDIT_TIME],0
                                                        FROM [BASE_TESTRULE]
                                                        where TESTRULE_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE.FIELDS_TESTRULE_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }

                            hashTable.Remove(BASE_TESTRULE.FIELDS_TESTRULE_KEY);
                            hashTable[BASE_TESTRULE.FIELDS_EDIT_TIME] = string.Empty;

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(baseTestRule, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtControlPara_Update != null && dtControlPara_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtControlPara_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY, hashTable[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY].ToString());

                            if (!Convert.ToString(hashTable[BASE_TESTRULE_CTLPARA.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE_CTLPARA]
                                                            ([CONTROL_PARAM_KEY],[TESTRULE_KEY],[CONTROL_OBJ]
                                                            ,[CONTROL_TYPE],[CONTROL_VALUE],[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                            SELECT '{0}',[TESTRULE_KEY],[CONTROL_OBJ]
                                                            ,[CONTROL_TYPE],[CONTROL_VALUE],[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0
                                                            FROM [BASE_TESTRULE_CTLPARA]
                                                            where CONTROL_PARAM_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_CTLPARA.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                                
                            hashTable.Remove(BASE_TESTRULE_CTLPARA.FIELDS_CONTROL_PARAM_KEY);
                            hashTable[BASE_TESTRULE_CTLPARA.FIELDS_EDIT_TIME] = string.Empty;

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(base_controlPara, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtAvgPower_Update != null && dtAvgPower_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAvgPower_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY, hashTable[BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY].ToString());
                            if (!Convert.ToString(hashTable[BASE_TESTRULE_AVGPOWER.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE_AVGPOWER]
                                                            ([AVG_POWER_KEY],[TESTRULE_KEY],[POWERSET_KEY]
                                                            ,[PS_CODE],[PS_SEQ],[AVGPOWER_MIN],[AVGPOWER_MAX]
                                                            ,[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                            SELECT '{0}',[TESTRULE_KEY],[POWERSET_KEY]
                                                            ,[PS_CODE],[PS_SEQ],[AVGPOWER_MIN],[AVGPOWER_MAX]
                                                            ,[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0
                                                            FROM [BASE_TESTRULE_AVGPOWER]
                                                            WHERE AVG_POWER_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_AVGPOWER.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                              
                            hashTable.Remove(BASE_TESTRULE_AVGPOWER.FIELDS_AVG_POWER_KEY);
                            hashTable[BASE_TESTRULE_AVGPOWER.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(base_avgpower, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtPowerControl_Update != null && dtPowerControl_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPowerControl_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY, hashTable[BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY].ToString());

                            if (!Convert.ToString(hashTable[BASE_TESTRULE_POWERCTL.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE_POWERCTL]
                                                            ([POWER_CONTROL_KEY],[TESTRULE_KEY]
                                                            ,[SEQ],[POWERSET_KEY],[PS_CODE],[PS_SEQ]
                                                            ,[POWERCTL_OBJ],[POWERCTL_TYPE],[POWERCTL_VALUE]
                                                            ,[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                            SELECT '{0}',[TESTRULE_KEY]
                                                            ,[SEQ],[POWERSET_KEY],[PS_CODE],[PS_SEQ]
                                                            ,[POWERCTL_OBJ],[POWERCTL_TYPE],[POWERCTL_VALUE]
                                                            ,[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0
                                                            FROM [BASE_TESTRULE_POWERCTL]
                                                            WHERE POWER_CONTROL_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_POWERCTL.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }

                            hashTable.Remove(BASE_TESTRULE_POWERCTL.FIELDS_POWER_CONTROL_KEY);
                            hashTable[BASE_TESTRULE_POWERCTL.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(base_powercontrol, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProductLevel_Update != null && dtProductLevel_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProductLevel_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY, hashTable[BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY].ToString());
                            if (!Convert.ToString(hashTable[BASE_TESTRULE_PROLEVEL.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE_PROLEVEL]
                                                            ([PRODUCT_LEVEL_KEY],[TESTRULE_KEY],[PROLEVEL_SEQ],[GRADE]
                                                            ,[MIN_LEVEL],[MIN_COLOR],[PALLET_GROUP],[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                            SELECT '{0}',[TESTRULE_KEY],[PROLEVEL_SEQ],[GRADE]
                                                            ,[MIN_LEVEL],[MIN_COLOR],[PALLET_GROUP],[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0
                                                            FROM [BASE_TESTRULE_PROLEVEL]
                                                            WHERE PRODUCT_LEVEL_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_PROLEVEL.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            hashTable.Remove(BASE_TESTRULE_PROLEVEL.FIELDS_PRODUCT_LEVEL_KEY);
                            hashTable[BASE_TESTRULE_PROLEVEL.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(base_productlevel, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtDecay_Update != null && dtDecay_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDecay_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY, hashTable[BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY].ToString());
                            if (!Convert.ToString(hashTable[BASE_TESTRULE_DECAY.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE_DECAY]
                                                            ([DECAY_KEY],[TESTRULE_KEY],[DECAY_SQL],[DECOEFFI_KEY]
                                                            ,[DECAY_POWER_MIN],[DECAY_POWER_MAX],[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG])
                                                            SELECT '{0}',[TESTRULE_KEY],[DECAY_SQL],[DECOEFFI_KEY]
                                                            ,[DECAY_POWER_MIN],[DECAY_POWER_MAX],[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0
                                                            FROM [BASE_TESTRULE_DECAY]
                                                            WHERE DECAY_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_DECAY.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            hashTable.Remove(BASE_TESTRULE_DECAY.FIELDS_DECAY_KEY);
                            hashTable[BASE_TESTRULE_DECAY.FIELDS_EDIT_TIME] = string.Empty;

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(base_decay, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtPrintSet_Update != null && dtPrintSet_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPrintSet_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            WhereConditions wc = new WhereConditions(BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY, hashTable[BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY].ToString());
                            if (!Convert.ToString(hashTable[BASE_TESTRULE_PRINTSET.FIELDS_ISFLAG]).Equals("0"))
                            {
                                sqlCommand = string.Format(@"INSERT INTO [BASE_TESTRULE_PRINTSET]
                                                            ([PRINTSET_KEY],[DECAY_KEY],[TESTRULE_KEY],[VIEW_NAME]
                                                            ,[VIEW_ADDRESS],[ISLABEL],[ISMAIN],[CREATER],[CREATE_TIME]
                                                            ,[EDITOR],[EDIT_TIME],[ISFLAG],[PRINT_QTY])
                                                            SELECT '{0}',[DECAY_KEY],[TESTRULE_KEY],[VIEW_NAME]
                                                            ,[VIEW_ADDRESS],[ISLABEL],[ISMAIN],[CREATER],[CREATE_TIME]
                                                            ,'{1}',[EDIT_TIME],0,[PRINT_QTY]
                                                            FROM [BASE_TESTRULE_PRINTSET]
                                                            WHERE PRINTSET_KEY='{2}'", FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_PRINTSET.FIELDS_EDITOR]),
                                                                                     Convert.ToString(hashTable[BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY]));
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            hashTable.Remove(BASE_TESTRULE_PRINTSET.FIELDS_PRINTSET_KEY);
                            hashTable[BASE_TESTRULE_PRINTSET.FIELDS_EDIT_TIME] = string.Empty;
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(base_printset, hashTable, wc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveTestRuleAllData Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        public DataSet GetPrintData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT LABEL_ID,LABEL_NAME,DATA_TYPE,PRINTER_TYPE,PRODUCT_MODEL,CERTIFICATE_TYPE,POWERSET_TYPE
                               FROM BASE_PRINTLABEL 
                               WHERE IS_USED='Y'
                               AND IS_VALID='Y'";
                dsReturn = this.db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicTestRuleEngine.GetPrintData Error: " + ex.Message);
            }
            return dsReturn;
        }


    }
}

