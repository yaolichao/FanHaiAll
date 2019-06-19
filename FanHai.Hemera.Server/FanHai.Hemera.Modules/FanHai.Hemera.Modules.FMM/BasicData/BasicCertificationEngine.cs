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
    /// 产品认证维护操作类
    /// </summary>
    public class BasicCertificationEngine : AbstractEngine, IBasicCertificationEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BasicCertificationEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 获取认证数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetCertification(string certificationType, DateTime certificationDate)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = string.Format(@"SELECT *
                                            FROM BASE_CERTIFICATION A
                                            WHERE A.CERTIFICATION_TYPE = '{0}'
	                                            AND {1}
	                                            --AND A.IS_USED = 'Y'
                                            ", certificationType
                                             , certificationDate == DateTime.MinValue ? "CERTIFICATION_DATE IS NULL"
                                                                                      : string.Format("DATEDIFF(DAY, A.CERTIFICATION_DATE, '{0}') = 0", certificationDate));
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
        /// <summary>
        /// 获取有效的认证数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetValidCertification()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT [CERTIFICATION_KEY]
	                                ,[CERTIFICATION_TYPE]
	                                ,[CERTIFICATION_DATE]
	                                ,[VERSION]
	                                ,[CREATOR]
	                                ,[CREATE_TIME]
	                                ,[EDITOR]
	                                ,[EDIT_TIME]
	                                ,[IS_USED]
                                FROM [BASE_CERTIFICATION] A
                                WHERE A.IS_USED = 'Y'
                                ORDER BY A.CERTIFICATION_TYPE,A.CERTIFICATION_DATE ASC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetValidCertification Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取有效的认证类型
        /// </summary>
        /// <returns></returns>
        public DataSet GetValidCertificationType()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"SELECT DISTINCT A.CERTIFICATION_TYPE
                                    FROM BASE_CERTIFICATION A
                                    WHERE A.IS_USED = 'Y'
                                    ORDER BY A.CERTIFICATION_TYPE";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetValidCertificationType Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dsChange"></param>
        /// <returns></returns>
        public DataSet SaveCertification(DataSet dsChange)
        {
            if (dsChange == null || dsChange.Tables.Count <= 0) return null;

            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            DataTable dtInsert = null, dtDelete = null, dtUpdate = null;
            BaseCertification baseCertification = new BaseCertification();

            if (dsChange.Tables.Contains(BaseCertification.TableForInsert)) dtInsert = dsChange.Tables[BaseCertification.TableForInsert];
            if (dsChange.Tables.Contains(BaseCertification.TableForDelete)) dtDelete = dsChange.Tables[BaseCertification.TableForDelete];
            if (dsChange.Tables.Contains(BaseCertification.TableForUpdate)) dtUpdate = dsChange.Tables[BaseCertification.TableForUpdate];

            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    if (dtInsert != null && dtInsert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtInsert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT CONVERT(INT, VERSION) AS VERSION FROM {0}
                                                                 WHERE CERTIFICATION_KEY = '{1}'", baseCertification.TABLE_NAME, dr["CERTIFICATION_KEY"]);
                            DataSet ds = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断是否存在且被删除过，若存在则在原有的基础上对版本号进行+1
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION"] = Convert.ToInt32(ds.Tables[0].Compute("max(VERSION)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            if (hashTable.ContainsKey("ETL_FLAG")) hashTable.Remove("ETL_FLAG");
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement2(baseCertification, hashTable);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtUpdate.Rows)
                        {
                            sqlCommand = string.Format(@"UPDATE T
                                                                SET T.IS_USED = 'N',T.EDITOR='{2}',T.EDIT_TIME=GETDATE(),T.ETL_FLAG='U'
                                                                FROM {0} T
                                                                WHERE CERTIFICATION_KEY = '{1}'
                                                                AND VERSION='{3}';
                                                                ", baseCertification.TABLE_NAME
                                                         , dr["CERTIFICATION_KEY"]
                                                         , dr["EDITOR"]
                                                         , dr["VERSION"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION"] = Convert.ToInt32(dr["VERSION"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["CREATE_TIME"] = DBNull.Value;
                            dr["EDITOR"] = DBNull.Value;
                            dr["EDIT_TIME"] = DBNull.Value;

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            if (hashTable.ContainsKey("ETL_FLAG")) hashTable.Remove("ETL_FLAG");
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement2(baseCertification, hashTable);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtDelete != null && dtDelete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDelete.Rows)
                        {
                            sqlCommand = string.Format(@"UPDATE T
                                                                SET T.IS_USED = 'N',T.EDITOR='{2}',T.EDIT_TIME=GETDATE(),T.ETL_FLAG='U'
                                                                FROM {0} T
                                                                WHERE CERTIFICATION_KEY = '{1}'
                                                                AND VERSION='{3}';
                                                                ", baseCertification.TABLE_NAME
                                                         , dr["CERTIFICATION_KEY"]
                                                         , dr["EDITOR"]
                                                         , dr["VERSION"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError("SaveCertification Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }

            return dsReturn;
        }
    }
}

