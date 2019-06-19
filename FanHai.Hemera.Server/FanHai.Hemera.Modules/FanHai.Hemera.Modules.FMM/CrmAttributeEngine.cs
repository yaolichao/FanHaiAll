using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using System.Collections;

using FanHai.Hemera.Utils.DatabaseHelper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 自定义属性数据的数据管理类。
    /// </summary>
    /// <remarks>
    /// ------------------------
    /// Category    (类似数据表)
    /// ------------------------
    /// Column      (类似数据表中的列)
    /// -----------------------
    /// Data        (类似数据表中的列对应的数据)
    /// -----------------------
    /// </remarks>
    public class CrmAttributeEngine : AbstractEngine, ICrmAttributeEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CrmAttributeEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 添加自定义属性数据分类。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddBasicDataCategoryInfo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;
            //initialize tablefields
            BASE_ATTRIBUTE_CATEGORY_FIELDS baseAttributeCategoryFields = new BASE_ATTRIBUTE_CATEGORY_FIELDS();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string categoryName = Convert.ToString(htParams[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME]);
                    string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}'", 
                                        BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME, 
                                        BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, 
                                        categoryName.PreventSQLInjection());
                    int count = Convert.ToInt32(db.ExecuteScalar(dbtran, CommandType.Text, sql));
                    if (count > 0)//存在指定的分类名称
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.BasicData.MsgExistNameCheck}");
                    }
                    else
                    {
                        //生成INSERT SQL
                        sql = DatabaseTable.BuildInsertSqlStatement(baseAttributeCategoryFields, htParams, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        dbtran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("AddBasicDataCategoryInfo Error: " + ex.Message);
                }
                finally
                {
                    dbtran = null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取自定义属性数据分类。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。
        /// 数据表结构：
        ///  -------------------
        /// | 列名    |  列值   |
        ///  -------------------
        /// </param>
        /// <returns>包含自定义属性数据分类的数据集对象。</returns>
        public DataSet GetBasicDataCategoryInfo(DataSet dsParams)
        {
            string sql = "";  //sql
            DataSet dsReturn = new DataSet(); //to return
            try
            {
                if (dsParams != null && dsParams.Tables.Count > 0 && dsParams.Tables[0].Rows.Count > 0)
                {
                    string colName = Convert.ToString(dsParams.Tables[0].Rows[0][0]);
                    string colValue = Convert.ToString(dsParams.Tables[0].Rows[0][1]);
                    sql = "SELECT " + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY + ","
                        + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME + "  FROM " + BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME
                        + " WHERE  1=1 AND "
                        + colName.PreventSQLInjection()
                        + " = '"
                        + colValue.PreventSQLInjection() + "'"
                        + " order by " + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME;
                }
                else
                {
                    sql = "SELECT " + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY + ","
                          + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME
                          + " FROM " + BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME
                          + " ORDER BY " + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME;
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBasicDataCategoryInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除自定义属性数据分类。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据分类的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteBasicDataCategoryInfo(DataSet dsParams)
        {
            string sql = "";  //sql
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;
            //initialize tablefields
            BASE_ATTRIBUTE_CATEGORY_FIELDS baseAttributeCategoryFields = new BASE_ATTRIBUTE_CATEGORY_FIELDS();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string categoryKey = Convert.ToString(htParams[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY]);
                    //先判断是否有分类下是否有属性。
                    sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}'",
                                        BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                        categoryKey.PreventSQLInjection());
                    int count = Convert.ToInt32(db.ExecuteScalar(dbtran, CommandType.Text, sql));
                    if (count > 0)
                    {
                        //add parameter table
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.BasicData.MsgDeleteColumn}");
                    }
                    else
                    {
                        //get sql
                        sql = string.Format("DELETE FROM {0} WHERE {1}='{2}'",
                                            BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME,
                                            BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY,
                                            categoryKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        dbtran.Commit();
                        //add parameter table
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("DeleteBasicDataCategoryInfo Error: " + ex.Message);
                }
                finally
                {
                    dbtran = null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 添加自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据列的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet AddBasicDataColumnInfo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;
            BASE_ATTRIBUTE_FIELDS baseAttributeFields = new BASE_ATTRIBUTE_FIELDS();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string attrName=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]);
                    string attrKey=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY]);
                    string sql =string.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}' AND {3}='{4}'" ,
                                        BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                        attrName.PreventSQLInjection(),
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                        attrKey.PreventSQLInjection());
                    int count=Convert.ToInt32(db.ExecuteScalar(dbtran, CommandType.Text, sql));
                    if (count > 0)
                    {
                        //add parameter table
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.BasicData.MsgUnderSameGroupNoSameName}");
                    }
                    else
                    {
                        //生成INSERT SQL
                        sql = DatabaseTable.BuildInsertSqlStatement(baseAttributeFields, htParams, null);
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        dbtran.Commit();
                        //add parameter table
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("AddBasicDataColumnInfo Error: " + ex.Message);
                }
                finally
                {
                    dbtran=null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性数据列的数据集对象。</returns>
        public DataSet GetBasicDataColumnIInfo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string categoryKey=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY]);
                    string sql=string.Format(@"SELECT A.{0},A.{1},A.{2},A.{3},B.{4} 
                                                FROM {5} A,{6} B
                                                WHERE A.{7}=B.{8} AND B.{8}='{9}'",
                                                BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE,
                                                BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS,
                                                BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME,
                                                BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                                BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME,
                                                BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                                BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY,
                                                categoryKey.PreventSQLInjection());
                    dsReturn = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
                    dbtran.Commit();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("GetBasicDataColumnIInfo Error: " + ex.Message);
                }
                finally
                {
                    dbtran=null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性数据列的数据集对象。</returns>
        public DataSet GetColumns(DataSet dataSet)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dataSet.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;

            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string categoryKey=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY]);
                    string sql=string.Format("SELECT {0},{1},{2} FROM {3} WHERE {4}='{5}'",
                                            BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                            BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                            BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS,
                                            BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                            BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                            categoryKey.PreventSQLInjection());
                    dsReturn = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("GetColumns Error: " + ex.Message);
                }
                finally
                {
                    dbtran=null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据列的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteBasicDataColumnInfo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;

            //initialize tablefields
            BASE_ATTRIBUTE_FIELDS baseAttributeFields = new BASE_ATTRIBUTE_FIELDS();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string attrKey=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]);
                    //判断自定义属性数据列是否有对应的自定义属性数据。
                    string sql=string.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}'",
                                            CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                            CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY ,
                                            attrKey.PreventSQLInjection());
                    int count=Convert.ToInt32(db.ExecuteScalar(dbtran, CommandType.Text, sql));
                    if (count> 0)//存在自定义属性数据。
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.BasicData.MsgCannotDeleteColumn}");
                    }
                    else
                    {
                        sql=string.Format("DELETE FROM {0} WHERE {1}='{2}'",
                                        BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                        attrKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        dbtran.Commit();
                        //add parameter table
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("DeleteBasicDataColumnInfo Error: " + ex.Message);
                }
                finally
                {
                    dbtran=null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据列的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateBasicDataColumnInfo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            DbTransaction dbtran = null;
            //initialize tablefields
            BASE_ATTRIBUTE_CATEGORY_FIELDS baseAttributeCategoryFields = new BASE_ATTRIBUTE_CATEGORY_FIELDS();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                dbtran = dbconn.BeginTransaction();
                try
                {
                    string attrKey=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]);
                    //判断自定义属性数据列是否存在对应的自定义属性数据。
                    string sql=string.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}'",
                                        CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                        CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                        attrKey.PreventSQLInjection());
                    int count=Convert.ToInt32(db.ExecuteScalar(dbtran, CommandType.Text, sql));
                    //存在对应的自定义属性数据。直接返回，不能更新。
                    if (count > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.BasicData.MsgCannotEditColumn}");
                        return dsReturn;
                    }
                    //更新SQL
                    string attrName=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]);
                    string type=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE]);
                    string description=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS]);
                    string editor=Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_EDITOR]);
                    string timezone=Convert.ToString( htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_EDIT_TIMEZONE]);
                    sql=string.Format(@"UPDATE {0} 
                                        SET {1}='{2}',{3}='{4}',{5}='{6}',{7}=GETDATE(),{8}='{9}',{10}='{11}'
                                        WHERE {12}='{13}'",
                                        BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                        attrName.PreventSQLInjection(),
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE,
                                        type.PreventSQLInjection(),
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS,
                                        description.PreventSQLInjection(),
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_EDIT_TIME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_EDITOR,
                                        editor.PreventSQLInjection(),
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_EDIT_TIMEZONE,
                                        timezone.PreventSQLInjection(),
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                        attrKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    dbtran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("UpdateBasicDataColumnInfo Error: " + ex.Message);
                }
                finally
                {
                    dbtran=null;
                    dbconn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取自定义属性数据列。
        /// </summary>
        /// <param name="dataSet">包含查询条件的数据集对象。
        /// --------------------------------
        /// {FIELDS_ATTRIBUTE_KEY,val}
        /// --------------------------------
        /// </param>
        /// <returns>包含自定义属性数据列的数据集对象。</returns>
        public DataSet GetColumnInfoByAttributeKey(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            try
            {
                string attrKey = Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]);
                string sql = string.Format("SELECT A.{0},A.{1},A.{2},A.{3},B.{4} FROM {5} A,{6} B WHERE A.{7}=B.{8} AND A.{9}='{10}'",
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS,
                                        BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME,
                                        BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                        BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY,
                                        BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                        attrKey.PreventSQLInjection());
                //excute sql
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetColumnInfoByAttributeKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveBasicData(DataSet dsParams)
        {
            string sql = "";  //sql
            DataSet dsReturn = new DataSet();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                DbTransaction dbtran = dbconn.BeginTransaction();
                try
                {
                    for (int i = 0; i < dsParams.Tables["columnDt"].Rows.Count; i++)
                    {
                        string columnKey = Convert.ToString(dsParams.Tables["columnDt"].Rows[i]["columnKey"]);
                        string columnName = Convert.ToString(dsParams.Tables["columnDt"].Rows[i]["columnName"]);
                        string columnValue = Convert.ToString(dsParams.Tables["columnDt"].Rows[i]["columnValue"]);
                        string columnItemOrder = Convert.ToString(dsParams.Tables["columnDt"].Rows[i]["columnItemOrder"]);

                        if (dsParams.Tables["columnDt"].Rows[i]["columnKey"].ToString() != "'-1'")
                        {
                            if (dsParams.Tables["columnDt"].Rows[i]["deal"].ToString() == "ADD")
                            {
                                sql =string.Format(@"INSERT INTO {0}({1},{2},{3},{4})
                                                    VALUES ('{5}','{6}','{7}','{8}')",
                                                    CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_VALUE,
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER,
                                                    columnKey.PreventSQLInjection(),
                                                    columnName.PreventSQLInjection(),
                                                    columnValue.PreventSQLInjection(),
                                                    columnItemOrder.PreventSQLInjection());
                                //excute sql
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            }
                            else if (dsParams.Tables["columnDt"].Rows[i]["deal"].ToString() == "EDIT")
                            {
                                //get sql
                                sql = string.Format(@"UPDATE  {0} SET {1} = '{2}'
                                                    WHERE {3}= '{4}' AND {5}='{6}' AND {7} = '{8}'",
                                                    CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_VALUE,
                                                    columnValue.PreventSQLInjection(),
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                    columnName.PreventSQLInjection(),
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                    columnKey.PreventSQLInjection(),
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER,
                                                    columnItemOrder.PreventSQLInjection());
                                //excute sql
                                int count = db.ExecuteNonQuery(dbtran, CommandType.Text, sql);

                                if (count == 0)
                                {
                                    sql = string.Format(@"INSERT INTO {0} ({1},{2},{3},{4})
                                                        VALUES ('{5}','{6}','{7}','{8}')",
                                                        CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                                        CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                        CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                        CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_VALUE,
                                                        CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER ,
                                                        columnKey.PreventSQLInjection() ,
                                                        columnName.PreventSQLInjection() ,
                                                        columnValue.PreventSQLInjection(),
                                                        columnItemOrder.PreventSQLInjection());
                                    //excute sql
                                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                                }
                            }
                        }
                    }
                    dbtran.Commit();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    dbtran.Rollback();
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("SaveBasicData Error: " + ex.Message);
                }
                finally
                {
                    dbtran = null;
                    dbconn.Close();
                }
            }
            return dsReturn;

        }
        /// <summary>
        /// 获取所有自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet GetAllData(DataSet dsParams)
        {
            DataSet dtReturn = new DataSet(); //to return
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            try
            {
                string categoryKey = Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY]);
                string categoryName = Convert.ToString(htParams[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME]);
                string sql = @"SELECT C." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY
                        + ",C." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME
                        + ",C." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_VALUE
                        + ",C." + CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER
                        + " FROM " + CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME + " C,"      //表CRM_ATTRIBUTE
                        + BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME + " A,"       //表BASE_ATTRIBUTE_CATEGORY
                        + BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME + " B"                 //表BASE_ATTRIBUTE
                        + " WHERE (B." + BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + "=C." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY
                        + " AND A." + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY + "=B." + BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY
                        + ")"
                        + " AND ( A." + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY + "='"
                        + categoryKey.PreventSQLInjection()
                        + "' OR A." + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME + "= '"
                        + categoryName .PreventSQLInjection()+ "') ";
                //excute sql
                dtReturn = db.ExecuteDataSet(CommandType.Text, sql);
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dtReturn, string.Empty);
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dtReturn, ex.Message);
                LogService.LogError("GetAllData Error: " + ex.Message);
            }
            return dtReturn;
        }
        /// <summary>
        /// 根据自定义属性分类名或分类主键获取自定义属性列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性列的数据集对象。</returns>
        public DataSet GetGruopBasicData(DataSet dsParams)
        {            
            DataSet returnDS = new DataSet(); //to return 
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                try
                {
                    returnDS = GetGruopBasicData(db,dsParams);
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnDS, "");
                }
                catch (Exception ex)
                {
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(returnDS, ex.Message);
                    LogService.LogError("GetGruopBasicData Error: " + ex.Message);
                }
                finally
                {
                    dbconn.Close();
                }
            }
            return returnDS;
        }
        /// <summary>
        /// 根据自定义属性分类名或分类主键获取自定义属性列。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        ///  <returns>包含自定义属性列的数据集对象。</returns>
        public static DataSet GetGruopBasicData(Database db,DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            string categoryKey = Convert.ToString(htParams[BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY]);
            string categoryName = Convert.ToString(htParams[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME]);
            string sql = string.Format("SELECT B.{0} FROM {1} B,{2} A WHERE (A.{3}=B.{4}) AND (A.{5}='{6}' OR A.{7}='{8}')",
                                    BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                    BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                    BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME,
                                    BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY,
                                    BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                    BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY,
                                    categoryKey.PreventSQLInjection(),
                                    BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME,
                                    categoryName.PreventSQLInjection());
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }
        /// <summary>
        /// 查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换。
        /// 将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// </param>
        /// <returns>包含基础数据信息的数据集对象。</returns>
        public DataSet GetSpecifyAttributeData(DataSet dsParams)
        {
            DataSet resDS = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {

                    StringBuilder sqlString = new StringBuilder("SELECT A.ITEM_ORDER, ");
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string attrKey = Convert.ToString(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_KEY]);
                        string attrVal = Convert.ToString(dtParams.Rows[i][COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                        //将 CRM_ATTRIBUTE 中的行数据转换为列数据。
                        sqlString.AppendFormat("MAX(CASE WHEN A.ATTRIBUTE_NAME='{0}' THEN A.ATTRIBUTE_VALUE END) AS \"{1}\", ",
                                                attrKey.PreventSQLInjection(),
                                                attrVal.PreventSQLInjection());
                    }
                    sqlString.Remove(sqlString.Length - 2, 2);
                    sqlString.Append(" FROM CRM_ATTRIBUTE A ,BASE_ATTRIBUTE B,BASE_ATTRIBUTE_CATEGORY C");
                    sqlString.Append(" WHERE A.ATTRIBUTE_KEY =B.ATTRIBUTE_KEY");
                    sqlString.Append(" AND B.CATEGORY_KEY= C.CATEGORY_KEY");
                    string categoryName=Convert.ToString(dtParams.Rows[0][COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                    sqlString.AppendFormat(" AND C.CATEGORY_NAME = '{0}'", categoryName.PreventSQLInjection());
                    sqlString.Append(" GROUP BY A.ITEM_ORDER");
                    sqlString.Append(" ORDER BY A.ITEM_ORDER");

                    db.LoadDataSet(CommandType.Text, sqlString.ToString(), resDS, new string[] { TRANS_TABLES.TABLE_UDAS });
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("GetSpecifyAttributeData Error: " + ex.Message);
            }
            return resDS;
        }
        /// <summary>
        /// 删除自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteBasicData(DataSet dsParams)
        {
            DataSet dtReturn = new DataSet(); //to return
            try
            {
                //check whether there rows
                if (dsParams != null && dsParams.Tables.Count > 0 && dsParams.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < dsParams.Tables["columnDt"].Rows.Count; k++)
                    {
                        string columnName = Convert.ToString(dsParams.Tables["columnDt"].Rows[k]["columnName"]);
                        string columnKey = Convert.ToString(dsParams.Tables["columnDt"].Rows[k]["columnKey"]);
                        string columnItemOrder = Convert.ToString(dsParams.Tables["columnDt"].Rows[k]["columnItemOrder"]);
                        string sql = string.Format("DELETE FROM {0} WHERE {1}='{2}' AND {3}='{4}' AND {5}='{6}'",
                                                    CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                                    columnName.PreventSQLInjection(),
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                                    columnKey.PreventSQLInjection(),
                                                    CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER,
                                                    columnItemOrder.PreventSQLInjection());
                        db.ExecuteNonQuery(CommandType.Text,sql);
                    }
                    //add parameter table
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dtReturn, "");
                }
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dtReturn, ex.Message);
                LogService.LogError("DeleteBasicData Error: " + ex.Message);
            }
            return dtReturn;
        }
        /// <summary>
        /// 根据自定义属性分类获取自定义属性列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性列的数据集对象。</returns>
        public DataSet GetAttributsColumnsForSomeCategory(DataSet dsParams)
        {
            string sql = "";    //define sql string
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable dtParams = dsParams.Tables[BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME];
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string myCategory = Convert.ToString(htParams[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME]).ToLower();
                    //get sql
                    sql = string.Format(@"SELECT A.{0},A.{1},A.{2},A.{3},'' AS DATA_TYPESTRING
                                        FROM {4} A,{5} B
                                        WHERE B.{6}=A.{7} AND B.{8}=",
                                         BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY,
                                         BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME,
                                         BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS,
                                         BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE,
                                         BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME,
                                         BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME,
                                         BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY,
                                         BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY,
                                         BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME);

                    switch (myCategory)
                    {
                        //case "saleorder":
                        //    sql += "'Uda_sales_order'";
                        //    break;
                        //case "salesorderitem":
                        //    sql += "'Uda_sales_order_item'";
                        //    break;
                        case "workorder":
                            sql += "'Uda_work_order'";
                            break;
                        case "operator":
                            sql += "'Uda_operation'";
                            break;
                        case "step":
                            sql += "'Uda_step'";
                            break;
                        case "product":
                            sql += "''";
                            break;
                        case "lot":
                            sql += "'Uda_lot'";
                            break;
                        //case "lottemplate":
                        //    sql += "'Uda_lot_template'";
                        //    break;
                        case "equipment":
                            sql += "'Uda_equipment'";
                            break;
                        case "part":                       //成品管理  modi by chao.pang
                            sql += "'Uda_part'";
                            break;
                        case "computer":                   //计算机配置  modi by chao.pang
                            sql += "'Uda_computer'";
                            break;
                        case "line":                       //线别管理  modi by chao.pang
                            sql += "'Uda_Line'";
                            break;
                        default:
                            sql += "'" + myCategory.PreventSQLInjection() + "'";
                            break;
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                //add parameter table
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAttributsColumnsForSomeCategory Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换。
        /// 将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。数据集对象中包含两个数据表。数据表名分别为"Columns"和"Category"。
        /// "Columns"数据表存放方法返回给调用者的数据表的列名，"Category"数据表存放属性的分类名。
        /// "Columns"数据表包含一列，该列放置数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// "Category"数据表包含两列，第一列存放数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"），第二列为其对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// </param>
        /// <returns> 数据集对象。</returns>
        public DataSet GetDistinctColumnsData(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet(); //to return
            try
            {
                //数据集不为null //数据集中包含数据表
                if (dsParams != null && dsParams.Tables.Count > 0)
                {
                    dsReturn = GetDistinctColumnsData(db, dsParams);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetDistinctColumnsData Error: " + ex.Message);
            }
            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("GetDistinctColumnsData Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 根据查询条件获取自定义属性数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// 数据集对象中包含三个数据表。数据表名分别为"Columns","Category"，<see cref="BASIC_CONST.PARAM_TABLENAME_CONDITIONS"/>。
        /// "Columns"数据表存放方法返回给调用者的数据表的列名，"Category"数据表存放属性的分类名。"Columns"数据表包含一列，该列放置数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// "Category"数据表包含两列，第一列存放数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"）， 第二列为其对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// 条件数据表为可选，包含两列，第一个列存放属性名，第二列存放属性值，可以包含多行，使用AND连接起来查询数据。
        /// </param>
        /// <returns>包含自定义属性数据信息的数据集对象。</returns>
        public DataSet GetBasicDataByConditons(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet(); //to return
            try
            {
                dsReturn = GetBasicDataByConditons(db, dsParams);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBasicDataByConditons Error: " + ex.Message);
            }

            DateTime endTime = DateTime.Now;
            LogService.LogInfo("GetBasicDataByConditons Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换。
        /// 将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。数据集对象中包含两个数据表。数据表名分别为"Columns"和"Category"。
        /// "Columns"数据表存放方法返回给调用者的数据表的列名，"Category"数据表存放属性的分类名。
        /// "Columns"数据表包含一列，该列放置数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// "Category"数据表包含两列，第一列存放数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"），
        /// 第二列为其对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// </param>
        /// <returns> 包含自定义属性数据信息的数据集对象。</returns>
        public static DataSet GetDistinctColumnsData(Database db, DataSet dsParams)
        {
            DataSet returnDS = new DataSet();
            string sql = string.Empty;  //sql
            sql = "SELECT A." + CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER + ", ";
            //get dynamic sql which can convert columns data to row data
            for (int i = 0; i < dsParams.Tables["Columns"].Rows.Count; i++)
            {
                string attrName=Convert.ToString(dsParams.Tables["Columns"].Rows[i][0]);
                string attrValue = Convert.ToString( dsParams.Tables["Columns"].Rows[i][0]);
                //将 CRM_ATTRIBUTE 中的行数据转换为列数据。
                sql += " MAX( CASE WHEN A." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME + "= '" + attrName.PreventSQLInjection()
                    + "' THEN A." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_VALUE + " END) \"" + attrValue .PreventSQLInjection()+ "\" ,";
            }
            sql = sql.Substring(0, sql.Length - 1);//DELETE THE LAST "," OF SQL
            sql += " FROM " + CRM_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME + " A ,";
            sql += BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME + " B,";
            sql += BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME + " C";
            sql += " WHERE A." + CRM_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + " =B." + BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY;
            sql += " AND B." + BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY + "= C." + BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY;
            sql += " AND C.";
            string colName = Convert.ToString(dsParams.Tables["Category"].Rows[0][0]);
            string colValue= Convert.ToString(dsParams.Tables["Category"].Rows[0][1]);
            sql += colName + " = '" + colValue.PreventSQLInjection() + "'"; 
            sql += " GROUP BY " + CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER;
            //执行查询，返回查询结果。
            returnDS = db.ExecuteDataSet(CommandType.Text,sql);
            return returnDS;
        }
        /// <summary>
        /// 从数据库中查询自定义属性数据。
        /// 即查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换，
        /// 然后将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为数据行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="columnNames">
        /// 字符串数组。
        /// 数组中的数据来源于数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// </param>
        /// <param name="categoryName">
        /// 自定义属性数据类别名。
        /// </param>
        /// <returns>包含自定义属性数据信息的数据集对象。包含<paramref name="columnNames"/>中的列名。</returns>
        public static DataSet GetDistinctColumnsData(Database db, string[] columnNames, string categoryName)
        {
            //列集合为null，列集合没有数据，存放基础数据分类名的键值对对象的键或值无数据
            if (null == columnNames || columnNames.Length < 1 || string.IsNullOrEmpty(categoryName))
                return null;

            DataSet baseData = new DataSet();

            //存放列名的数据表，为调用函数提供参数。
            DataTable columnTable = new DataTable("Columns");
            columnTable.Columns.Add("ColumnName");
            //遍历列名
            for (int i = 0; i < columnNames.Length; i++)
            {
                columnTable.Rows.Add();
                columnTable.Rows[i][0] = columnNames[i];
            }
            //存放分类名的数据表，为远程调用函数提供参数。
            DataTable categoryTable = new DataTable("Category");
            categoryTable.Columns.Add("ColumnName");
            categoryTable.Columns.Add("ColumnValue");
            categoryTable.Rows.Add();
            categoryTable.Rows[0][0] = "CATEGORY_NAME";
            categoryTable.Rows[0][1] = categoryName;
            //将存放列名的数据表和存放分类名的数据表添加到数据集中。为调用函数提供参数。
            DataSet paramData = new DataSet();
            paramData.Merge(columnTable, false, MissingSchemaAction.Add);
            paramData.Merge(categoryTable, false, MissingSchemaAction.Add);
            return GetDistinctColumnsData(db, paramData);
        }
        /// <summary>
        /// 查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换。
        /// 将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="column">属性名称，如有有多列使用逗号(,)分开。</param>
        /// <param name="categoryName">属性类别名称。</param>
        /// <returns> 包含自定义属性数据信息的数据集对象。包含<paramref name="column"/>中的列名。</returns>
        public static DataSet GetDistinctColumnsData(Database db, string column,string categoryName)
        {
            DataSet returnDS = new DataSet();
            string[] columns = column.Split(',');
            return GetDistinctColumnsData(db,columns,categoryName);
        }
        /// <summary>
        /// 根据查询条件获取自定义属性数据。
        /// </summary>
        /// <param name="db">数据库操作对象.</param>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// 数据集对象中包含三个数据表。数据表名分别为"Columns","Category"，<see cref="BASIC_CONST.PARAM_TABLENAME_CONDITIONS"/>。
        /// "Columns"数据表存放方法返回给调用者的数据表的列名，"Category"数据表存放属性的分类名。"Columns"数据表包含一列，该列放置数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// "Category"数据表包含两列，第一列存放数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"）， 第二列为其对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// 条件数据表为可选，包含两列，第一个列存放属性名，第二列存放属性值，可以包含多行，使用AND连接起来查询数据。
        /// </param>
        /// <returns> 包含自定义属性数据信息的数据集对象。</returns>
        public static DataSet GetBasicDataByConditons(Database db,DataSet dsParams)
        {
            try
            {
                if (!dsParams.Tables.Contains(BASIC_CONST.PARAM_TABLENAME_COLUMNS)
                    || !dsParams.Tables.Contains(BASIC_CONST.PARAM_TABLENAME_CATEGORY))
                {
                    return null;
                }

                StringBuilder sqlCommand = new StringBuilder();
                List<string> columnList = new List<string>();

                sqlCommand.AppendLine("SELECT");
                foreach (DataRow dataRow in dsParams.Tables[BASIC_CONST.PARAM_TABLENAME_COLUMNS].Rows)
                {
                    sqlCommand.AppendLine(string.Format("{0},", dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME]));
                }

                //Delete the last "," of this sql
                sqlCommand.Remove(sqlCommand.Length - 3, 1);

                sqlCommand.AppendLine("FROM (SELECT A.ITEM_ORDER,");
                //Get dynamic sql which can convert columns data to row data
                foreach (DataRow dataRow in dsParams.Tables[BASIC_CONST.PARAM_TABLENAME_COLUMNS].Rows)
                {
                    string attrName = Convert.ToString( dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME]);
                    sqlCommand.AppendLine(string.Format("MAX(CASE WHEN A.ATTRIBUTE_NAME= '{0}' THEN A.ATTRIBUTE_VALUE END) {0},", attrName.PreventSQLInjection()));
                    columnList.Add(dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME].ToString());
                }

                if (dsParams.Tables.Contains(BASIC_CONST.PARAM_TABLENAME_CONDITIONS))
                {
                    foreach (DataRow dataRow in dsParams.Tables[BASIC_CONST.PARAM_TABLENAME_CONDITIONS].Rows)
                    {
                        if (columnList.Contains(dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME].ToString()))
                            continue;
                        string attrName = Convert.ToString( dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME]);
                        sqlCommand.AppendLine(string.Format("MAX(CASE WHEN A.ATTRIBUTE_NAME= '{0}' THEN A.ATTRIBUTE_VALUE END) {0},", attrName.PreventSQLInjection()));
                    }
                }

                //Delete the last "," of this sql
                sqlCommand.Remove(sqlCommand.Length - 3, 1);
                sqlCommand.AppendLine("FROM CRM_ATTRIBUTE A, BASE_ATTRIBUTE B, BASE_ATTRIBUTE_CATEGORY C");
                sqlCommand.AppendLine("WHERE A.ATTRIBUTE_KEY = B.ATTRIBUTE_KEY");
                sqlCommand.AppendLine("AND B.CATEGORY_KEY = C.CATEGORY_KEY");
                foreach (DataRow dataRow in dsParams.Tables[BASIC_CONST.PARAM_TABLENAME_CATEGORY].Rows)
                {
                    string colName=Convert.ToString( dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME]);
                    string colValue=Convert.ToString( dataRow[BASIC_CONST.PARAM_COL_COLUMN_VALUE]);
                    sqlCommand.AppendLine(string.Format("AND C.{0} = '{1}'", colName, colValue.PreventSQLInjection()));
                }
                sqlCommand.AppendLine("GROUP BY ITEM_ORDER) V");
                sqlCommand.AppendLine("WHERE 1=1");

                if (dsParams.Tables.Contains(BASIC_CONST.PARAM_TABLENAME_CONDITIONS))
                {
                    foreach (DataRow dataRow in dsParams.Tables[BASIC_CONST.PARAM_TABLENAME_CONDITIONS].Rows)
                    {
                        string colName = Convert.ToString(dataRow[BASIC_CONST.PARAM_COL_COLUMN_NAME]);
                        string colValue = Convert.ToString(dataRow[BASIC_CONST.PARAM_COL_COLUMN_VALUE]);
                        sqlCommand.AppendLine(string.Format("AND V.{0} = '{1}'", colName, colValue.PreventSQLInjection()));
                    }
                }

                DataSet dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand.ToString());
                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据查询条件获取自定义属性数据。
        /// </summary>
        /// <param name="db">数据库操作对象.</param>
        /// <param name="columnNames">列名。</param>
        /// <param name="categoryName">类别名。</param>
        /// <param name="whereConditons">包含查询条件的键值对集合。</param>
        /// <returns> 包含自定义属性数据信息的数据集对象。列名参考<paramref name="columnNames"/></returns>
        public static DataSet GetBasicDataByConditons(Database db, string[] columnNames, string categoryName,
            List<KeyValuePair<string, string>> whereConditons)
        {
            if (null == columnNames || columnNames.Length < 1 || string.IsNullOrEmpty(categoryName))
                return null;

            DataSet paramData = new DataSet();
            DataSet baseData = new DataSet();
            DataTable columnTable = new DataTable(BASIC_CONST.PARAM_TABLENAME_COLUMNS);
            columnTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_NAME);

            foreach (string columnName in columnNames)
            {
                columnTable.Rows.Add();
                columnTable.Rows[columnTable.Rows.Count - 1][BASIC_CONST.PARAM_COL_COLUMN_NAME] = columnName;
            }


            DataTable categoryTable = new DataTable(BASIC_CONST.PARAM_TABLENAME_CATEGORY);
            categoryTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_NAME);
            categoryTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_VALUE);
            categoryTable.Rows.Add();
            categoryTable.Rows[0][BASIC_CONST.PARAM_COL_COLUMN_NAME] ="CATEGORY_NAME";
            categoryTable.Rows[0][BASIC_CONST.PARAM_COL_COLUMN_VALUE] = categoryName;

            paramData.Merge(columnTable, false, MissingSchemaAction.Add);
            paramData.Merge(categoryTable, false, MissingSchemaAction.Add);

            if (null != whereConditons || whereConditons.Count > 0)
            {
                DataTable conditionTable = new DataTable(BASIC_CONST.PARAM_TABLENAME_CONDITIONS);
                conditionTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_NAME);
                conditionTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_VALUE);

                foreach (KeyValuePair<string, string> condition in whereConditons)
                {
                    conditionTable.Rows.Add();
                    conditionTable.Rows[conditionTable.Rows.Count - 1][BASIC_CONST.PARAM_COL_COLUMN_NAME] = condition.Key;
                    conditionTable.Rows[conditionTable.Rows.Count - 1][BASIC_CONST.PARAM_COL_COLUMN_VALUE] = condition.Value;
                }
                paramData.Merge(conditionTable, false, MissingSchemaAction.Add);
            }
            return GetBasicDataByConditons(db, paramData);
        }
        /// <summary>
        /// 根据ERP线别名称转换为MES工厂名称。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="erpLineName">ERP线别名称。</param>
        /// <returns>MES工厂名称。</returns>
        public static string ConvertERPLineNameToMESFactoryName(Database db,string erpLineName)
        {
            string mesFactoryName = string.Empty;
            string[] columns = new string[] { "MESFACTORY" };
            string categoryName = BASEDATA_CATEGORY_NAME.MEScontrastERP;
            KeyValuePair<string, string> condition = new KeyValuePair<string, string>("ERPLINENAME", erpLineName);
            List<KeyValuePair<string, string>> lstCondition = new List<KeyValuePair<string, string>>();
            lstCondition.Add(condition);
            DataSet ds = GetBasicDataByConditons(db,columns, categoryName, lstCondition);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                mesFactoryName = Convert.ToString(ds.Tables[0].Rows[0]["MESFACTORY"]);
            }
            return mesFactoryName;
        }
    }

}