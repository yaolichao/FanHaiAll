using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Collections;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 区域数据管理类。
    /// </summary>
    public class LocationEngine : AbstractEngine,ILocation
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db;
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LocationEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 保存区域数据。
        /// </summary>
        /// <param name="dsParams">包含区域数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveNewLocation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            Hashtable mainTable = new Hashtable();
            Hashtable relationTable = new Hashtable();
            string sql = "";

            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    //get maintable
                    if (dsParams.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
                    {
                        mainTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
                        string locationName = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME]);

                        //检查区域名称是否存在。
                        sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'",
                                            FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME, 
                                            FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME, 
                                            locationName.PreventSQLInjection());
                        int count=Convert.ToInt32(db.ExecuteScalar( CommandType.Text, sql));
                        if (count > 0)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgLocationNameHasBeenUsed}");
                            return dsReturn;
                        }
                        string locationKey = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);
                        string level = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL]);
                        string descriptions = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS]);
                        string creator = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_CREATOR]);
                        string timeZone = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_CREATE_TIMEZONE]);
                        string editor = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_EDITOR]);
                        string editTimeZone = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_EDIT_TIMEZONE]);
                        sql = " INSERT INTO " + FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                        sql += " ( ";
                        sql += FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_CREATOR + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_CREATE_TIME + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_CREATE_TIMEZONE + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_EDITOR + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_EDIT_TIME + ",";
                        sql += FMM_LOCATION_FIELDS.FIELD_EDIT_TIMEZONE;
                        sql += " ) ";
                        sql += " VALUES (";
                        sql += "'" + locationKey.PreventSQLInjection() + "',";
                        sql += "'" + locationName.PreventSQLInjection()+ "',";
                        sql += "'" + level.PreventSQLInjection() + "',";
                        sql += "'" + descriptions .PreventSQLInjection()+ "',";
                        sql += "'" + creator.PreventSQLInjection() + "',";
                        sql += "GETDATE(),";
                        sql += "'" + timeZone.PreventSQLInjection() + "',";
                        sql += "'" + editor.PreventSQLInjection() + "',";
                        sql += "GETDATE(),";
                        sql += "'" + editTimeZone.PreventSQLInjection() + "'";
                        sql += " ) ";
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    //get relation table
                    if (dsParams.Tables.Contains(FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME))
                    {
                        relationTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME]);
                        string parentLocKey = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY]);
                        string parentLocLevel = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL]);
                        string locationKey = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY]);
                        string locationLevel = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_LEVEL]);
                        sql = " INSERT INTO " + FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                        sql += " ( ";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY + ",";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL + ",";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY + ",";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_LEVEL;
                        sql += " ) ";
                        sql += " VALUES (";
                        sql += "'" + parentLocKey.PreventSQLInjection() + "',";
                        sql += "'" + parentLocLevel.PreventSQLInjection() + "',";
                        sql += "'" + locationKey.PreventSQLInjection() + "',";
                        sql += "'" + locationLevel.PreventSQLInjection() + "'";
                        sql += " ) ";
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("SaveNewLocation Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                }
                finally
                {
                    dbTran.Dispose();
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新区域数据。
        /// </summary>
        /// <param name="dsParams">包含区域数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateLocation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            Hashtable mainTable = new Hashtable();
            Hashtable relationTable = new Hashtable();
            string sql = "";

            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    //get maintable
                    if (dsParams.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
                    {
                        mainTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
                        string locationName = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME] );
                        string locationKey = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);
                        string locationLevel = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL]);
                        //检查区域名称是否存在。
                        sql = " SELECT COUNT(*) FROM " + FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                        sql += " WHERE " + FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME + " = '" +locationName.PreventSQLInjection()+ "'";
                        sql += " AND " + FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY + " <> '" + locationKey.PreventSQLInjection()+ "'";
                        int count =Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                        if (count > 0)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgLocationNameHasBeenUsed}");
                            return dsReturn;
                        }
                        //检查区域关联数据是否存在
                        if (locationLevel == "9"|| locationLevel == "5" || locationLevel == "2")
                        {
                            sql = " SELECT COUNT(*) FROM " + FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                            sql += " WHERE " + FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY + " = '" + locationKey.PreventSQLInjection() + "'";

                            count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                            if (count > 0)
                            {
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgLocationIsUsedNow}");
                                return dsReturn;
                            }
                        }
                        string descriptions = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS]);
                        string editor = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_EDITOR]);
                        string editTimeZone = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_EDIT_TIMEZONE]);
                        sql = " UPDATE " + FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                        sql += " SET ";
                        sql += FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME + "='" + locationName.PreventSQLInjection()+ "',";
                        sql += FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL + "='" + locationLevel.PreventSQLInjection() + "',";
                        sql += FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS + "='" + descriptions.PreventSQLInjection() + "',";
                        sql += FMM_LOCATION_FIELDS.FIELD_EDITOR + "='" + editor.PreventSQLInjection() + "',";
                        sql += FMM_LOCATION_FIELDS.FIELD_EDIT_TIME + "=GETDATE(),";
                        sql += FMM_LOCATION_FIELDS.FIELD_EDIT_TIMEZONE + "='" + editTimeZone.PreventSQLInjection() + "' ";
                        sql += " WHERE "+FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY + "='" + locationKey.PreventSQLInjection() + "'";
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    }
                    //新增区域关联关系。
                    if (dsParams.Tables.Contains(FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME))
                    {
                        relationTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME]);
                        mainTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);

                        string parentLocKey = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY]);
                        string parentLocLevel = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL]);
                        string locationKey = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY]);
                        string locationLevel = Convert.ToString(relationTable[FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_LEVEL]);
                        //删除旧的区域管理关系。
                        sql = " DELETE FROM " + FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                        sql += " WHERE " + FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY + " = ";
                        sql += "'" + locationKey.PreventSQLInjection() + "'";
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        //新增区域关联关系。
                        sql = " INSERT INTO " + FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                        sql += " ( ";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY + ",";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL + ",";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY + ",";
                        sql += FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_LEVEL;
                        sql += " ) ";
                        sql += " VALUES (";
                        sql += "'" + parentLocKey.PreventSQLInjection() + "',";
                        sql += "'" + parentLocLevel.PreventSQLInjection() + "',";
                        sql += "'" + locationKey.PreventSQLInjection() + "',";
                        sql += "'" + locationLevel.PreventSQLInjection() + "'";
                        sql += " ) ";
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("UpdateLocation Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                }
                finally
                {
                    dbTran.Dispose();
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据区域层级值获取可以设置子区域的区域数据。
        /// </summary>
        /// <param name="level">0：空数据集，1：工厂,2：楼层，其他值：车间。</param>
        /// <returns>包含区域数据的数据集对象。</returns>
        public DataSet GetWorkshopBindData(int level)
        {
            DataSet dsReturn = new DataSet(); 
            try
            {
                string sqlString = string.Empty;
                if (level == 0 || level == 1 || level == 2)
                {

                    sqlString =string.Format(@"SELECT CAST(A.LOCATION_LEVEL AS VARCHAR)+'_'+A.LOCATION_KEY AS LOCATION_KEY,A.LOCATION_NAME AS LOCATION_NAME 
                                            FROM FMM_LOCATION A 
                                            WHERE A.LOCATION_LEVEL = '{0}'", level);

                }
                else
                {
                    sqlString =@"SELECT CAST(A.LOCATION_LEVEL AS VARCHAR)+'_'+A.LOCATION_KEY AS LOCATION_KEY ,
                                       B.PARENT_LOCATION_NAME + ' -> '+ A.LOCATION_NAME AS LOCATION_NAME 
                                FROM FMM_LOCATION A, 
                                    (SELECT A.LOCATION_NAME AS PARENT_LOCATION_NAME, B.* 
                                     FROM FMM_LOCATION A,FMM_LOCATION_RET B 
                                     WHERE A.LOCATION_KEY = B.PARENT_LOC_KEY) B 
                                WHERE A.LOCATION_KEY = B.LOCATION_KEY 
                                AND A.LOCATION_LEVEL = '5'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlString);
                dsReturn.Tables[0].TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message );
                LogService.LogError("GetWorkshopBindData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除区域数据。
        /// </summary>
        /// <param name="dsParams">包含区域数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteLocation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            Hashtable mainTable = new Hashtable();
            Hashtable relationTable = new Hashtable();
            string sql = "";

            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    if (dsParams.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
                    {
                        mainTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
                        string locationKey = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);
                        string locationLevel = Convert.ToString(mainTable[FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL]);
                        //检查区域是否设置关联线别。
                        if (locationLevel == "9")
                        {
                            sql = "SELECT COUNT(*) FROM " + FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME;
                            sql += " WHERE " + FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY + "= '" + locationKey.PreventSQLInjection() + "'";
                            int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                            if (count > 0)//存在关联线别。
                            {
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgLocationDeleteCheck}");
                                return dsReturn;
                            }
                        }
                        else
                        {
                            //检查是否设置关联的子区域。
                            sql = "SELECT COUNT(*) FROM " + FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                            sql += " WHERE " + FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY + "= '" + locationKey.PreventSQLInjection() + "'";
                            int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                            if (count > 0)
                            {
                                //车间
                                if (locationLevel == "5")
                                {
                                    //提示已给该车间分配区域不能删除
                                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgLocationIsUsedByArea}");
                                }
                                //楼层
                                else if (locationLevel == "2")
                                {
                                    //提示已给该楼层分配车间不能删除 modi by chao.pang
                                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgFloorIsUsedByWorkShop}");

                                    
                                }
                                //工厂
                                else if (locationLevel == "1")
                                {
                                    //已给该工厂分配楼层不能删除 modi by chao.pang
                                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Modules.EMS.LocationEngine.MsgFloorIsUsedByWorkShop1}");
                                }
                                return dsReturn;
                            }
                        }
                        //删除区域数据。
                        sql = "DELETE FROM  " + FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME ;
                        sql += " WHERE " + FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY + "= '" +locationKey.PreventSQLInjection() + "'";
                        db.ExecuteNonQuery(dbTran,CommandType.Text, sql);
                        //删除区域关联数据。
                        sql = "DELETE FROM  " + FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                        sql += " WHERE " + FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY + "= '" + locationKey.PreventSQLInjection()+ "'";
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("DeleteLocation Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message );
                    //Rollback Transaction
                    dbTran.Rollback();
                }
                finally
                {
                    dbTran.Dispose();
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取区域中的线别信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含区域中的线别信息的数据集对象。</returns>
        public DataSet GetLinesAndLocationLine(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsLineReturn = new DataSet();
            DbTransaction dbTran = null;
            Hashtable mainTable = new Hashtable();
            Hashtable relationTable = new Hashtable();
            string sql = "";
            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    //get maintable
                    if (dsParams.Tables.Contains(FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME))
                    {
                        mainTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME]);
                        string locationKey = Convert.ToString(mainTable[FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY]);
                        //获取线别名称和线别主键。
                        sql=string.Format(@"SELECT A.{0},A.{1}
                                            FROM {2} A  WHERE A.{3} NOT IN  (SELECT B.{4} FROM {5} B WHERE B.{6} = '{7}')",
                                            FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,
                                            FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,
                                            FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,
                                            FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY ,
                                            FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY, 
                                            locationKey.PreventSQLInjection());
                        dsLineReturn=db.ExecuteDataSet(CommandType.Text, sql);
                        dsLineReturn.Tables[0].TableName =FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;
                        //获取区域和线别的管理关系。
                        sql = string.Format(@"SELECT A.{0},B.{1} FROM {2} A 
                                            LEFT JOIN {3} B  ON A.{4}= B.{5}
                                            WHERE A.{6}='{7}'",
                                            FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY,
                                            FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,
                                            FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY,
                                            FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,
                                            FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY,
                                            locationKey.PreventSQLInjection());
                        dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                        //set table name
                        dsReturn.Tables[0].TableName = FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(dsLineReturn.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME], false, MissingSchemaAction.Add);
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    LogService.LogError("GetLinesAndLocationLine Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message );
                    //Rollback Transaction
                    dbTran.Rollback();
                }
                finally
                {
                    dbTran.Dispose();
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存区域和线别的关联关系。
        /// </summary>
        /// <param name="dsParams">包含区域和线别关联关系的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SaveLocationLineRelation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            string sql = "";
            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    //get maintable
                    if (dsParams.Tables.Contains(FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME + "main") && dsParams.Tables.Contains(FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME))
                    {
                        //get parameter data
                        DataTable  mainTable = dsParams.Tables[FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME + "main"];
                        Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(mainTable);
                        DataTable dataTable = dsParams.Tables[FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME];
                        string locationKey=Convert.ToString(htParams[FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY]);
                        //删除管理关系。
                        sql =string.Format(@"DELETE FROM {0} WHERE {1} = '{2}'",
                                            FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY,
                                            locationKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        //新增关联关系
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            string locKey = Convert.ToString(dataTable.Rows[i][FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY]);
                            string lineKey=Convert.ToString(dataTable.Rows[i][FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY]);
                            sql =string.Format(@"INSERT INTO {0} ({1},{2}) VALUES('{3}','{4}')",
                                                FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME,
                                                FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY,
                                                FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY,
                                                locKey.PreventSQLInjection(),
                                                lineKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        dbTran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                }
                catch (Exception ex)
                {
                    LogService.LogError("SaveLocationLineRelation Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                }
                finally
                {
                    dbTran.Dispose();
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn; 
        }
        /// <summary>
        /// 查询区域信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -------------------------------------------
        /// {LOCATION_NAME}
        /// {LOCATION_LEVEL}
        /// -------------------------------------------
        /// </param>
        /// <returns>包含区域信息的数据集对象。</returns>
        public DataSet SearchLocation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            Hashtable htParams = new Hashtable();
            if (dsParams.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
            {
                htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dsParams.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
            }
            try
            {
                //根据位置名称和区域类型区域信息
                string sql =string.Format(@"SELECT {0},{1},{2},CASE {3} WHEN 1 THEN '工厂' 
                                                                        WHEN 2 THEN '楼层' 
                                                                        WHEN 5 THEN '车间' 
                                                                        WHEN 9 THEN '区域' 
                                                               END  {3}
                                            FROM {4}",
                                            FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY,
                                            FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME,
                                            FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS,
                                            FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL,
                                            FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME);
                if (htParams.Count != 0)
                {
                    string locationName = Convert.ToString(htParams[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME]);
                    string locationLevel = Convert.ToString(htParams[FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL]);
                    //名称模糊查询
                    sql += " WHERE " + FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME + " LIKE '%" + locationName.PreventSQLInjection() + "%'";
                    if (locationLevel != "")
                    {
                        sql += " AND " + FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL + " = '" + locationLevel.PreventSQLInjection()+ "'";
                    }
                }
                sql += " ORDER BY LOCATION_NAME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                LogService.LogError("SearchLocation Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取区域信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -------------------------------------------
        /// {LOCATION_KEY}
        /// -------------------------------------------
        /// </param>
        /// <returns>包含区域信息的数据集对象。</returns>
        public DataSet GetLocation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtParams = dsParams.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
            Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            
            try
            {
                string locationKey=Convert.ToString(htParams[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);
                string sql =string.Format(@"SELECT A.* ,CAST(B.{0} AS VARCHAR)+ '_'+B.{1} AS {1}
                                            FROM {2} A 
                                            LEFT JOIN {3} B ON A.{4} =  B.{5}
                                            WHERE A.{6}= '{7}'",
                                            FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL,
                                            FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY,
                                            FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME,
                                            FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY,
                                            FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY,
                                            FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY,
                                            locationKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                LogService.LogError("GetLocation Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message );
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取所有的区域信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含区域信息的数据集对象。</returns>
        public DataSet GetAllLoactions(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = @"SELECT A.LOCATION_KEY ,B.PARENT_LOCATION_NAME + ' -> '+ A.LOCATION_NAME AS LOCATION_NAME 
                                    FROM FMM_LOCATION A, (SELECT A.LOCATION_NAME AS PARENT_LOCATION_NAME, B.* 
                                                          FROM FMM_LOCATION A,FMM_LOCATION_RET B 
                                                          WHERE A.LOCATION_KEY = B.PARENT_LOC_KEY) B 
                                    WHERE A.LOCATION_KEY = B.LOCATION_KEY AND A.LOCATION_LEVEL = 9 
                                    ORDER BY LOCATION_NAME";

                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME });
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetAllLoactions Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据区域层级值获取可以设置子区域的区域数据。
        /// </summary>
        /// <param name="level">0：空数据集，1：工厂,2：楼层，其他值：车间。</param>
        /// <returns>包含区域数据的数据集对象。</returns>
        public DataSet GetParentLocations(int level)
        {
            DataSet resDS = new DataSet();
            string sqlString = string.Empty;
            try
            {

                if (level == 0 || level == 1 || level == 2)
                {

                    sqlString =string.Format(@"SELECT B.PARENT_LOC_LEVEL+'-'+A.LOCATION_NAME  
                                            FROM FMM_LOCATION A, FMM_LOCATION_RET B 
                                            WHERE A.LOCATION_KEY = B.PARENT_LOC_KEY 
                                            AND A.LOCATION_LEVEL = '{0}'",level);

                }
                else
                {
                    sqlString = @"SELECT A.LOCATION_KEY ,B.PARENT_LOCATION_NAME + ' -> '+ A.LOCATION_NAME AS LOCATION_NAME 
                                FROM FMM_LOCATION A,(SELECT A.LOCATION_NAME AS PARENT_LOCATION_NAME, B.* 
				                                     FROM FMM_LOCATION A,FMM_LOCATION_RET B 
					                                 WHERE A.LOCATION_KEY = B.PARENT_LOC_KEY) B 
                                WHERE A.LOCATION_KEY = B.LOCATION_KEY 
                                AND A.LOCATION_LEVEL = '5'";
                }

                db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME });
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetParentLocations Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
            }
            return resDS;

        }
        /// <summary>
        /// 根据线别数据获取线别所属的车间。
        /// </summary>
        /// <param name="lines">使用逗号分开的所有线别值。</param>
        /// <returns>
        /// 包含工厂车间信息的数据集对象。
        /// 【LOCATION_KEY，LOCATION_NAME】。
        /// </returns>
        public DataSet GetFactoryRoomByLines(string lines)
        {
            DataSet resDS = new DataSet();

            try
            {
                string sqlCondition = UtilHelper.BuilderWhereConditionString("c.LINE_NAME", lines.Split(','));
                string sqlString = @"SELECT DISTINCT PARENT_KEY  as LOCATION_KEY,PARENT_NAME  as LOCATION_NAME
                                    FROM V_LOCATION_RET a,FMM_LOCATION_LINE b,FMM_PRODUCTION_LINE c
                                    WHERE a.LOCATION_KEY=b.LOCATION_KEY 
                                    AND b.LINE_KEY=c.PRODUCTION_LINE_KEY
                                    AND LOCATION_LEVEL=9"
                                        +sqlCondition+
                                    "ORDER BY a.PARENT_NAME ASC";
                db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetAllLoactions Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
            }

            return resDS;
        }
        /// <summary>
        /// 根据线上仓数据获取所属的车间。
        /// </summary>
        /// <param name="lines">使用逗号分开的所有线上仓名称。</param>
        /// <returns>
        /// 包含工厂车间信息的数据集对象。
        /// 【LOCATION_KEY，LOCATION_NAME】
        /// </returns>
        public DataSet GetFactoryRoomByStores(string stores)
        {
            DataSet resDS = new DataSet();

            try
            {
                string sqlCondition = UtilHelper.BuilderWhereConditionString("a.STORE_NAME", stores.Split(','));
                string sqlString = @"SELECT DISTINCT b.PARENT_KEY as LOCATION_KEY,b.LOCATION_NAME
                                    FROM WST_STORE a,
                                        (SELECT LOCATION_KEY,LOCATION_KEY PARENT_KEY,LOCATION_NAME
                                        FROM V_LOCATION_RET
                                        WHERE LOCATION_LEVEL=5
                                        UNION 
                                        SELECT LOCATION_KEY,PARENT_KEY,PARENT_NAME LOCATION_NAME
                                        FROM V_LOCATION_RET
                                        WHERE LOCATION_LEVEL=9) b
                                    WHERE a.LOCATION_KEY=b.LOCATION_KEY"
                                    + sqlCondition +
                                    "ORDER BY b.LOCATION_NAME ASC";
                db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetFactoryRoomByStores Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
            }

            return resDS;
        }
        /// <summary>
        /// 根据名称获取车间信息。
        /// </summary>
        /// <param name="name">车间名称。</param>
        /// <returns>包含车间信息的数据集。</returns>
        public DataSet GetFactoryRoom(string name)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT LOCATION_KEY,LOCATION_NAME,DESCRIPTIONS 
                                                   FROM FMM_LOCATION WHERE LOCATION_LEVEL=5 AND LOCATION_NAME='{0}'", name);
                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetFactoryRoom Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 根据车间名称获取其所在的工厂信息。
        /// </summary>
        /// <param name="name">车间名称。</param>
        /// <returns>包含工厂信息的数据集。</returns>
        public DataSet GetFactory(string roomName)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT DISTINCT FACTORY_NAME,FACTORY_KEY FROM V_LOCATION WHERE ROOM_NAME='{0}'", roomName);
                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME });
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetFactory Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
    }

}
