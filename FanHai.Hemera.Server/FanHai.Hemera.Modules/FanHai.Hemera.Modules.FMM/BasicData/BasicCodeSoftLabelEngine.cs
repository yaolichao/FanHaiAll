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
    public class BasicCodeSoftLabelEngine : AbstractEngine, IBasicCodeSoftLabelEngine
    {
        private Database db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BasicCodeSoftLabelEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public DataSet GetCodeSoftLabel(string sLabelID)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM BASE_CODESOFT_LABEL_SET  WHERE 1=1";
                if (!string.IsNullOrEmpty(sLabelID))
                {
                    sql += " AND LABEL_ID='" + sLabelID + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCodeSoftLabel Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet UpdateData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    int irows = 0;
                    irows = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    dbTran.Commit();
                    dsReturn.ExtendedProperties.Add("rows", irows.ToString());
                    //string s = dsReturn.ExtendedProperties["rows"].ToString();
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError(sUpFuntionName + " Error: " + ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
            }
            return dsReturn;
        }

        public DataSet AddData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            using (DbConnection dbConn = db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTran = dbConn.BeginTransaction();
                try
                {
                    int irows = 0;
                    irows = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    dbTran.Commit();
                    dsReturn.ExtendedProperties.Add("rows", irows.ToString());
                    //string s = dsReturn.ExtendedProperties["rows"].ToString();
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError(sUpFuntionName + " Error: " + ex.Message);
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

