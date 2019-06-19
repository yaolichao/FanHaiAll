using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Modules.FMM;
using FanHai.Hemera.Share.Common;



namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        private Database db;                             //用于数据库访问的变量。
         /// <summary>
        /// 构造函数。
        /// </summary>
        public LotOperationEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化方法。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 获取可选问题工序。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="operations">拥有权限的工序名称，使用逗号(,)分隔。</param>
        /// <returns>包含问题工序的数据集对象。</returns>
        public DataSet GetTroubleStepInfo(string lotKey,string operations)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT *
                                     FROM   
                                     ( 
                                        SELECT a.*
                                        FROM V_PROCESS_PLAN a
                                        WHERE a.ENTERPRISE_STATUS=1 AND a.ROUTE_STATUS=1
                                        AND 
                                        (
                                            EXISTS(SELECT DISTINCT STEP_KEY
                                                   FROM WIP_TRANSACTION
                                                   WHERE ACTIVITY='{1}'
                                                   AND UNDO_FLAG=0
                                                   AND PIECE_TYPE=0
                                                   AND PIECE_KEY='{0}'
                                                   AND STEP_KEY=a.ROUTE_STEP_KEY
                                                   AND ENTERPRISE_KEY=a.ROUTE_ENTERPRISE_VER_KEY)
                                            OR
                                            
                                            EXISTS(SELECT DISTINCT 1
                                                   FROM POR_LOT
                                                   WHERE LOT_KEY='{0}'
                                                   AND CUR_STEP_VER_KEY=a.ROUTE_STEP_KEY
                                                   AND ROUTE_ENTERPRISE_VER_KEY=a.ROUTE_ENTERPRISE_VER_KEY)
                                        )
                                        UNION
                                        SELECT ' ' [ENTERPRISE_NAME]
                                              ,1   [ENTERPRISE_VERSION]
                                              ,' ' [ROUTE_NAME]
                                              ,0   [ROUTE_SEQ]
                                              ,ROUTE_OPERATION_NAME [ROUTE_STEP_NAME]
                                              ,0   [ROUTE_STEP_SEQ]
                                              ,' ' [DESCRIPTIONS]
                                              ,' ' [ROUTE_ENTERPRISE_VER_KEY]
                                              ,' ' [ROUTE_ROUTE_VER_KEY]
                                              ,ROUTE_OPERATION_VER_KEY [ROUTE_STEP_KEY]
                                              ,' ' [ROUTE_NEXT_STEP_KEY]
                                              ,1 [ENTERPRISE_STATUS]
                                              ,1 [ROUTE_STATUS]
                                              ,[SCRAP_REASON_CODE_CATEGORY_KEY]
                                              ,[DEFECT_REASON_CODE_CATEGORY_KEY]
                                        FROM POR_ROUTE_OPERATION_VER
                                        WHERE ROUTE_OPERATION_VER_KEY='1007742a-c450-45af-b98d-d5f1aa2c19de-000'
                                    ) t",
                                    lotKey.PreventSQLInjection(),
                                    ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                //sbSql.Append(UtilHelper.BuilderWhereConditionString("ROUTE_STEP_NAME", operations.PreventSQLInjection().Split(',')));
                sbSql.Append(" ORDER BY ((ROUTE_SEQ+1)*10000000)+ROUTE_STEP_SEQ");

                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTroubleStepInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据原因分组主键获取原因分类。
        /// </summary>
        /// <param name="categoryKey">原因分组主键。</param>
        /// <returns>包含原因分类的数据集对象。</returns>
        public DataSet GetReasonCodeClass(string categoryKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT DISTINCT A.REASON_CODE_CLASS
                                    FROM FMM_REASON_CODE A,FMM_REASON_R_CATEGORY B
                                    WHERE A.REASON_CODE_KEY=B.REASON_CODE_KEY
                                    AND B.CATEGORY_KEY='{0}'",
                                    categoryKey.PreventSQLInjection());
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCodeClass Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据原因代码类别获取原因代码。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        public DataSet GetReasonCode(string categoryKey)
        {
            return GetReasonCode(categoryKey, string.Empty);
        }
        /// <summary>
        /// 根据原因代码类别主键和原因代码分类获取原因代码。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <param name="codeClass">原因代码分类。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        public DataSet GetReasonCode(string categoryKey,string codeClass)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT A.REASON_CODE_NAME,A.REASON_CODE_KEY,A.REASON_CODE_TYPE
                                    FROM FMM_REASON_CODE A,FMM_REASON_R_CATEGORY B
                                    WHERE A.REASON_CODE_KEY=B.REASON_CODE_KEY
                                    AND B.CATEGORY_KEY='{0}'",
                                    categoryKey.PreventSQLInjection());
                if (!string.IsNullOrEmpty(codeClass))
                {
                    sbSql.AppendFormat(" AND A.REASON_CODE_CLASS='{0}'",codeClass.PreventSQLInjection());
                }
                sbSql.Append("ORDER BY A.REASON_CODE_CLASS ASC,A.REASON_CODE_TYPE DESC ,B.CODE_INDEX ");
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCode Error: " + ex.Message);
            }
            return dsReturn;
        }       
    }
}
