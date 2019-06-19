using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using FanHai.Hemera.Share.Interface;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Constants;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;

namespace FanHai.Hemera.Modules.WipJob
{
    /// <summary>
    /// 自动过站的数据管理类。
    /// </summary>
    public class WipJobAutoTrack : IWipJobAutoTrack
    {
        private Database db;//数据库管理对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WipJobAutoTrack()
        { 
            db = DatabaseFactory.CreateDatabase();
        }        
        /// <summary>
        /// 插入一笔自动过站任务。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="dbTrans">数据库操作事务对象。</param>
        /// <param name="hashTable">包含自动过站任务数据的数据集对象。</param>
        /// <returns>新增自动过站任务的记录数。1：代表成功。0：代表失败。</returns>
        public static int InsertWipJob(Database db,DbTransaction dbTrans,Hashtable htParams)
        {
            int result=0;
            try
            {
                WIP_JOB_FIELDS wipFields = new WIP_JOB_FIELDS();
                string sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htParams, null);
                result = db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
            }
            catch (Exception ex)
            {               
                throw ex;
            }          
            return result;
        }
        /// <summary>
        /// 获取需要自动过站的最新一笔数据主键。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含最新一笔数据主键的数据集对象。</returns>
        public DataSet GetTrackNow(string stepKey)
        {
            string sql =string.Format(@"SELECT ROW_KEY  
                                        FROM 
                                        (
	                                        SELECT ROW_KEY,ROW_NUMBER() OVER(ORDER BY JOB_CREATETIME) RN 
	                                        FROM WIP_JOB
	                                        WHERE DATEDIFF(S,JOB_NEXTRUNTIME,CAST('{0}' AS DATETIME))>0 
	                                        AND STEP_KEY ='{0}' 
	                                        AND JOB_STATUS=0
                                        	
                                        ) T 
                                        WHERE RN=1",
                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        stepKey.PreventSQLInjection());
            DataSet result = db.ExecuteDataSet(CommandType.Text, sql);
            return result;
        }
        /// <summary>
        /// 获取等待自动出站的不重复的工步主键。
        /// </summary>
        /// <returns>包含工步主键的数据集对象。</returns>
        public DataSet GetMonStepKey()
        {
            string sql = string.Format(@"SELECT {0} FROM {1} GROUP BY {0}",
                                        WIP_JOB_FIELDS.FIELDS_STEP_KEY,
                                        WIP_JOB_FIELDS.DATABASE_TABLE_NAME,
                                        WIP_JOB_FIELDS.FIELDS_STEP_KEY);
            DataSet result = db.ExecuteDataSet(CommandType.Text, sql);
            return result;
        }
        /// <summary>
        /// 获取等待自动出站的生产批次。
        /// </summary>
        /// <returns>包含等待出站的生产批次的数据集对象。</returns>
        public DataSet GetWaitingForTrackOutJobs()
        {
            DataSet result = null;
            string sql = @"SELECT TOP 10 B.* FROM WIP_JOB B, 
                            (
                                SELECT MIN(JOB_NEXTRUNTIME) AS JOB_NEXTRUNTIME, STEP_KEY, LINE_NAME, JOB_TYPE 
                                FROM WIP_JOB A 
                                WHERE A.JOB_STATUS ='0' 
                                AND A.JOB_NEXTRUNTIME < GETDATE()
                                AND A.JOB_TYPE ='TRACKIN'
                                GROUP BY STEP_KEY,LINE_NAME,JOB_TYPE
                            ) C
                            WHERE B.JOB_NEXTRUNTIME = C.JOB_NEXTRUNTIME
                            AND B.STEP_KEY = C.STEP_KEY
                            AND B.LINE_NAME = C.LINE_NAME
                            AND B.JOB_TYPE = C.JOB_TYPE";
            result = db.ExecuteDataSet(CommandType.Text, sql);
            return result;
        }
        /// <summary>
        /// 新增在制品操作信息。
        /// </summary>
        /// <param name="dtParams">包含在制品操作信息的数据集对象。</param>
        public void InsertWipMessage(DataTable dtParams)
        {

        }
        /// <summary>
        /// 获取发送给指定人员的信息。
        /// </summary>
        /// <param name="user">指定人员登录名。</param>
        /// <returns>包含信息的数据集对象。</returns>
        public DataSet GetErrorMessageInfor(string user)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = string.Format (@"SELECT * FROM WIP_MESSAGE 
                                     WHERE STATUS = 0 AND TO_USER ='{0}'",
                                     user.PreventSQLInjection());                
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = WIP_MESSAGE_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetErrorMessageInfor Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
