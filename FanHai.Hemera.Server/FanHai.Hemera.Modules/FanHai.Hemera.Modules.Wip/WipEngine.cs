//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-09-13            重构 迁移到SQL Server数据库
// =================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SolarViewer.Hemera.Modules.EMS;
using SolarViewer.Hemera.Modules.WipJob;
using SolarViewer.Hemera.Modules.FMM;

namespace SolarViewer.Hemera.Modules.Wip
{
    /// <summary>
    /// 在制品数据管理类。
    /// </summary>
    public partial class WipEngine : AbstractEngine,IWipEngine
    {
        private Database db = null;//数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WipEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 获取显示在可视化看板上的等待作业的批次信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含批次信息的数据集对象。</returns>
        public DataSet GetLotsWaitingForDispatch(DataSet dsParams)
        {
            System.DateTime startTime = System.DateTime.Now;

            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string sql = "";
                    string strLines = "", strLineCondition = "";
                    string strOperations = "", strOperationCondition = "";
                    if (htParams.Contains(WIP_FIELDS.FIELDS_LINES))
                    {
                        strLines = htParams[WIP_FIELDS.FIELDS_LINES].ToString();
                    }

                    if (htParams.Contains(WIP_FIELDS.FIELDS_OPERATIONS))
                    {
                        strOperations = htParams[WIP_FIELDS.FIELDS_OPERATIONS].ToString();
                    }
                    sql = @"SELECT T.*,CASE FLOOR((DATEDIFF (mi,GETDATE(),T.START_WAIT_TIME) + 30)/30)
				                            WHEN 0 THEN 0 
				                            WHEN 1 THEN 0
				                            WHEN 2 THEN 1
				                            WHEN 3 THEN 2
				                            ELSE 2
			                            END AS WAIT_FLAG
                            FROM V_WAITINGFORDISPATCH T                          
                            WHERE 1=1";
                    if (strLines.Length > 0)
                    {
                        strLineCondition = UtilHelper.BuilderWhereConditionString("t.LINE_NAME", strLines.Split(','));
                        sql = sql + strLineCondition;
                    }
                    if (strOperations.Length > 0)
                    {
                        strOperationCondition = UtilHelper.BuilderWhereConditionString("t.ROUTE_STEP_NAME", strOperations.Split(','));
                        sql = sql + strOperationCondition;
                    }
                    sql = sql + " ORDER BY T.START_WAIT_TIME DESC";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotsWaitingForDispatch Error: " + ex.Message);
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("GetLotsWaitingForDispatch Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }

        /// <summary>
        /// 获取显示在可视化看板上的等待出站批次信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含批次信息的数据集对象。</returns>
        public DataSet GetLotsInTheDispatch(DataSet dsParams)
        {
            System.DateTime startTime = System.DateTime.Now;
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dataTable = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable hashData = SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string sql = "";
                    string strLines = "", strLineCondition = "";
                    string strOperations = "", strOperationCondition = "";
                    if (hashData.Contains(WIP_FIELDS.FIELDS_LINES))
                    {
                        strLines = hashData[WIP_FIELDS.FIELDS_LINES].ToString();
                    }

                    if (hashData.Contains(WIP_FIELDS.FIELDS_OPERATIONS))
                    {
                        strOperations = hashData[WIP_FIELDS.FIELDS_OPERATIONS].ToString();
                    }


                    sql = @"SELECT A.LOT_KEY,A.LOT_NUMBER,A.PRIORITY,A.LINE_NAME,A.WORK_ORDER_SEQ,A.TYPE,A.MODULE,A.QUANTITY,A.HOLD_FLAG,
		                        A.STATUS,A.STATE_FLAG,A.START_PROCESS_TIME,A.CUR_PRODUCTION_LINE_KEY, A.CUR_ROUTE_VER_KEY,A.WORK_ORDER_KEY,
		                        A.REWORK_FLAG, A.ROUTE_ENTERPRISE_VER_KEY, C.ORDER_NUMBER,C.PART_NUMBER, D.ROUTE_NAME, B.ROUTE_OPERATION_VER_KEY,
		                        B.ROUTE_STEP_NAME, 
		                        CASE A.STATE_FLAG 
			                        WHEN 0 THEN 'WAITINTFORTRACKINEDC' 
			                        WHEN 1 THEN 'TRACKINEDC' 
			                        WHEN 2 THEN 'WAITINGFORTRACKIN' 
			                        WHEN 4 THEN 'WAITINGFOREDC' 
			                        WHEN 5 THEN 'INEDC' 
			                        WHEN 9 THEN 'WAITINGFORTRACKOUT' 
			                        ELSE 'FINISHED'
		                        END AS STATE, 
		                        C.ORDER_NUMBER + '.' + A.WORK_ORDER_SEQ AS SEQUENCE_NUMBER, 
		                        CASE SIGN(DATEDIFF(MI,GETDATE(),A.START_PROCESS_TIME)-B.DURATION)
			                        WHEN -1 THEN 0
			                        WHEN 0  THEN 0
			                        WHEN 1  THEN 1
		                        END AS WAIT_FLAG,  
		                        V.TIMECONTROLBASE,
		                        V.TIMECONTROLMIN, 
		                        V.TIMECONTROLWARNING, 
		                        V.TIMECONTROLMAX, 
		                        ROUND(DATEDIFF(MI,GETDATE(),A.START_PROCESS_TIME),0)AS TIMEDURATION 
                        FROM POR_LOT A
                        LEFT JOIN POR_WORK_ORDER C ON C.WORK_ORDER_KEY=A.WORK_ORDER_KEY
                        LEFT JOIN POR_ROUTE_ROUTE_VER D ON A.CUR_ROUTE_VER_KEY = D.ROUTE_ROUTE_VER_KEY
                        LEFT JOIN POR_ROUTE_STEP B ON A.CUR_STEP_VER_KEY = B.ROUTE_STEP_KEY
                        LEFT JOIN V_ROUTE_STEP_TIMECONTROL V ON V.ROUTE_STEP_KEY = B.ROUTE_STEP_KEY
                        WHERE (A.STATE_FLAG >'2' AND A.STATE_FLAG <10) 
                        AND A.DELETED_TERM_FLAG='0' 
                        AND A.STATUS='1' ";

                    if (strLines.Length > 0)
                    {
                        strLineCondition = UtilHelper.BuilderWhereConditionString("A.OPR_LINE", strLines.Split(','));
                        sql = sql + strLineCondition;
                    }
                    if (strOperations.Length > 0)
                    {
                        strOperationCondition = UtilHelper.BuilderWhereConditionString("B.ROUTE_STEP_NAME", strOperations.Split(','));
                        sql = sql + strOperationCondition;
                    }
                    sql = sql + " ORDER BY A.START_PROCESS_TIME DESC";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotsInTheDispatch Error: " + ex.Message);
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("GetLotsInTheDispatch Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }
        /// <summary>
        /// 根据批次唯一标识符获取批次信息。批次唯一标识符可以是批次号也可以是批次主键。
        /// </summary>
        /// <param name="lotKey">批次号或者批次主键。</param>
        /// <returns>
        /// 获取的包含批次信息的数据集。
        /// [CURTIME,LOT_KEY, LOT_NUMBER,PRIORITY,LINE_NAME,WORK_ORDER_SEQ,TYPE,EDIT_TIME,
        /// MODULE,QUANTITY,HOLD_FLAG,STATUS,STATE_FLAG,CUR_PRODUCTION_LINE_KEY,SHIFT_NAME,
        /// CHILD_LINE,DEFECT_REASON_CODE_CATEGORY_KEY,CUR_ROUTE_VER_KEY,CUR_STEP_VER_KEY,WORK_ORDER_KEY,REWORK_FLAG,
        /// START_PROCESS_TIME,START_WAIT_TIME,EDITOR,ROUTE_ENTERPRISE_VER_KEY,EDIT_TIMEZONE,
        /// DELETED_TERM_FLAG,PART_VER_KEY,EDC_INS_KEY,OPERATOR,OPR_LINE,OPR_LINE_PRE,OPR_COMPUTER,
        /// SHIP_NUMBER,C.ORDER_NUMBER,C.PART_NUMBER,D.ROUTE_NAME,EDC_LIST_KEY,SAMPLING_KEY,ROUTE_OPERATION_VER_KEY,
        /// ROUTE_OPERATION_NAME,ROUTE_STEP_NAME,SCRAP_REASON_CODE_CATEGORY_KEY,FACTORYROOM_KEY,MATERIAL_LOT,
        /// DEFECT_REASON_CODE_CATEGORY_KEY,RE_ROUTE_ENTERPRISE_VER_KEY,RE_START_ROUTE_VER_KEY,RE_START_STEP_KEY,
        /// DURATION,EDC_NAME,CREATE_OPERTION_NAME]
        /// </returns>
        public DataSet GetLotsInfo(string lotKey)
        {
            return GetLotsInfo(null, lotKey);
        }

        private DataSet GetLotsInfo(DbTransaction dbTrans,string lotKey)
        {
            System.DateTime startTime = System.DateTime.Now;
            DataSet dsReturn = new DataSet();
            try
            {
                //查询语句
                string sql = @"SELECT GETDATE() AS CURTIME,
                                    A.LOT_KEY, 
                                    A.LOT_NUMBER,
                                    A.PRIORITY,
                                    A.LINE_NAME,
                                    A.WORK_ORDER_NO,
                                    A.WORK_ORDER_SEQ,
                                    A.EDIT_TIME,
                                    A.QUANTITY,
                                    A.HOLD_FLAG,                   
                                    A.STATUS,
                                    A.PRO_ID,
                                    A.STATE_FLAG,
                                    A.CUR_PRODUCTION_LINE_KEY,
                                    A.SHIFT_NAME,
                                    B.DEFECT_REASON_CODE_CATEGORY_KEY,
                                    A.CUR_ROUTE_VER_KEY,
                                    A.CUR_STEP_VER_KEY,              
                                    A.WORK_ORDER_KEY, 
                                    A.REWORK_FLAG,
                                    A.START_PROCESS_TIME,
                                    A.START_WAIT_TIME,
                                    A.EDITOR,
                                    A.ROUTE_ENTERPRISE_VER_KEY,
                                    A.EDIT_TIMEZONE,
                                    A.DELETED_TERM_FLAG,           
                                    A.PART_VER_KEY,
                                    A.EDC_INS_KEY,
                                    A.OPERATOR,
                                    A.OPR_LINE,
                                    A.OPR_LINE_PRE,
                                    A.OPR_COMPUTER,
                                    A.FACTORYROOM_KEY,
                                    A.MATERIAL_LOT,
                                    A.MATERIAL_CODE,               
                                    C.ORDER_NUMBER,
                                    C.PART_NUMBER,
                                    D.ROUTE_NAME,
                                    B.EDC_LIST_KEY,
                                    B.SAMPLING_KEY,
                                    B.ROUTE_OPERATION_VER_KEY,
                                    B.ROUTE_OPERATION_NAME,          
                                    B.ROUTE_STEP_NAME,
                                    B.SCRAP_REASON_CODE_CATEGORY_KEY,
                                    B.RE_ROUTE_ENTERPRISE_VER_KEY,
                                    B.RE_START_ROUTE_VER_KEY,       
                                    B.RE_START_STEP_KEY,
                                    B.DURATION,
                                    E.EDC_NAME,
                                    A.CREATE_OPERTION_NAME
                                FROM POR_LOT A
                                INNER JOIN POR_WORK_ORDER C ON C.WORK_ORDER_KEY=A.WORK_ORDER_KEY
                                INNER JOIN POR_ROUTE_STEP B ON A.CUR_STEP_VER_KEY = B.ROUTE_STEP_KEY
                                INNER JOIN POR_ROUTE_ROUTE_VER D ON A.CUR_ROUTE_VER_KEY = D.ROUTE_ROUTE_VER_KEY
                                LEFT JOIN EDC_MAIN E ON E.EDC_KEY=B.EDC_LIST_KEY
                                WHERE 1=1";

                if (lotKey != "") //有传入参数值。
                {
                    sql = sql + " AND (A.LOT_KEY='" + lotKey.PreventSQLInjection() + "'";                 //批次主键
                    sql = sql + " OR A.LOT_NUMBER='" + lotKey.PreventSQLInjection() + "')";               //批次号码
                }
                if (dbTrans == null)
                {
                    //执行查询并返回结果集合。
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                }
                else
                {
                    //执行查询并返回结果集合。
                    dsReturn = db.ExecuteDataSet(dbTrans,CommandType.Text, sql);
                }
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotsInfo Error: " + ex.Message);
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("GetLotsInfo Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
        /// <summary>
        /// 根据批次号码、上一流程组主键、上一流程主键、上一工步主键获取批次信息。
        /// </summary>
        /// <param name="dtParams">包含查询条件的数据表对象。
        /// 键名目前可以设置的为：
        /// <see cref="POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY"/>（当前流程主键），
        /// <see cref="POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY"/>（当前工步主键），
        /// <see cref="POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY"/>（当前流程组主键），
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_NUMBER"/>（批次号码）。
        /// </param>
        /// <returns>包含批次信息数据的数据集对象。
        /// </returns>
        public DataSet GetLotInfoWithCondition(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            string sql = string.Empty;
            Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            try
            {
                string routeKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string stepKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                string enterpriseKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                sql = string.Format(@"SELECT  A.LOT_KEY,A.LOT_NUMBER,A.LINE_NAME,A.EDIT_TIME,A.MODULE,A.QUANTITY,A.HOLD_FLAG,A.STATUS,A.STATE_FLAG,
                                         A.CUR_PRODUCTION_LINE_KEY,A.SHIFT_NAME,'{0}' as CUR_ROUTE_VER_KEY, '{1}' as CUR_STEP_VER_KEY,A.WORK_ORDER_KEY,
                                         A.EDITOR, '{2}' as ROUTE_ENTERPRISE_VER_KEY,A.EDIT_TIMEZONE, A.DELETED_TERM_FLAG, A.PART_VER_KEY, A.EDC_INS_KEY ,A.FACTORYROOM_KEY,
                                         A.REWORK_FLAG,A.OPR_LINE,A.OPR_LINE_PRE,A.OPR_LINE_PRE, C.ORDER_NUMBER,C.PART_NUMBER,
                                         (select D.ROUTE_NAME from POR_ROUTE_ROUTE_VER D  where D.ROUTE_ROUTE_VER_KEY='{0}') as ROUTE_NAME,
                                         (select B.SCRAP_REASON_CODE_CATEGORY_KEY from  POR_ROUTE_STEP B where b.route_step_key='{1}') as SCRAP_REASON_CODE_CATEGORY_KEY,
                                         (select ROUTE_STEP_NAME from  POR_ROUTE_STEP  where route_step_key='{1}') as ROUTE_STEP_NAME,
                                         (select ROUTE_OPERATION_VER_KEY from  POR_ROUTE_STEP  where route_step_key='{1}') as ROUTE_OPERATION_VER_KEY,
                                         (select DEFECT_REASON_CODE_CATEGORY_KEY from  POR_ROUTE_STEP  where route_step_key='{1}') as DEFECT_REASON_CODE_CATEGORY_KEY
                                    FROM POR_LOT A
                                    LEFT JOIN POR_WORK_ORDER C ON C.WORK_ORDER_KEY=A.WORK_ORDER_KEY
                                    WHERE A.LOT_NUMBER='{3}'",
                                routeKey.PreventSQLInjection(),                             
                                stepKey.PreventSQLInjection(),                            
                                enterpriseKey.PreventSQLInjection(),
                                lotNumber.PreventSQLInjection());            
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotInfoWithCondition Error: " + ex.Message);
            } 
            return dsReturn;
        }

        /// <summary>
        /// 根据工艺流程组主键、工艺流程主键和工步主键查询对应的名称。
        /// </summary>
        /// <param name="dtParams">包含查询条件的数据表对象。
        /// --------------------------------------------------
        /// {TO_ENTERPRISE_VER_KEY}{TO_ROUTE_VER_KEY}{TO_STEP_VER_KEY}
        /// --------------------------------------------------
        /// </param>
        /// <returns>包含工艺流程组主键、工艺流程主键和工步主键查询对应的名称的数据集对象。</returns>
        public DataSet GetProcessPlanNamesByKey(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            if (dtParams != null && dtParams.Rows.Count > 0)
            {
                Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                try
                {
                    string enterpriseKey = Convert.ToString(htParams[WIP_FIELDS.FIELDS_TO_ENTERPRISE_VER_KEY]);
                    string routeKey=Convert.ToString(htParams[WIP_FIELDS.FIELDS_TO_ROUTE_VER_KEY]);
                    string stepKey=Convert.ToString(htParams[WIP_FIELDS.FIELDS_TO_STEP_VER_KEY]);
                    string sqlCommand =string.Format(@"SELECT ENTERPRISE_NAME, ROUTE_NAME, ROUTE_STEP_NAME
                                                      FROM V_PROCESS_PLAN T
                                                      WHERE T.ROUTE_ENTERPRISE_VER_KEY = '{0}' 
                                                      AND T.ROUTE_ROUTE_VER_KEY = '{1}' 
                                                      AND T.ROUTE_STEP_KEY = '{2}'",
                                                      enterpriseKey.PreventSQLInjection(),
                                                      routeKey.PreventSQLInjection(),
                                                      stepKey.PreventSQLInjection());
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch(Exception ex)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("GetProcessPlanNamesByKey Error: " + ex.Message);
                }
            }
            return dsReturn;
        }
        
        /// <summary>
        /// 获取工步的自定义属性和属性值集合。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含工步自定义属性值的数据集对象。</returns>
        public static DataSet GetStepUda(string stepKey)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql =string.Format(@"SELECT A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE
                                        FROM   POR_ROUTE_STEP_ATTR A
                                        WHERE  A.ROUTE_STEP_KEY ='{0}'", 
                                        stepKey.PreventSQLInjection());
            DataSet attributeData = db.ExecuteDataSet(CommandType.Text, sql);
            return attributeData;
        }

        /// <summary>
        /// 获取工步指定的自定义属性值。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="attributeName">自定义属性名。</param>
        /// <returns>自定义属性名对应的属性值。</returns>
        public static string GetStepUdaValue(Database db, string stepKey,string attributeName)
        {
            string strReturn=string.Empty;
            string sql =string.Format(@"SELECT A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE
                                        FROM   POR_ROUTE_STEP_ATTR A
                                        WHERE  A.ROUTE_STEP_KEY ='{0}' AND A.ATTRIBUTE_NAME='{1}'",
                                        stepKey.PreventSQLInjection(),
                                        attributeName.PreventSQLInjection());
            DataSet attributeData = db.ExecuteDataSet(CommandType.Text, sql);
            if (attributeData.Tables[0]!=null && attributeData.Tables[0].Rows.Count > 0)
            {
                strReturn = attributeData.Tables[0].Rows[0]["ATTRIBUTE_VALUE"].ToString();
            }
            return strReturn;
        }
        /// <summary>
        /// 根据查询条件获取原因代码集合。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// 键名目前可以设置的为原因代码分类主键：<see cref="FMM_REASON_CODE_FIELDS.FIELDS_CATEGORY_KEY"/>
        /// </param>
        /// <returns>包含原因代码数据的数据集对象。
        /// [0 AS CODE_NUMBER,A.REASON_CODE_NAME,A.REASON_CODE_KEY,A.REASON_CODE_TYPE]
        /// </returns>
        public DataSet GetReasonCode(DataSet dsParams)
        {
             DateTime startTime =DateTime.Now;
             DataSet dsReturn = new DataSet(); 
             try
             {
                 //数据集中包含名称为TRANS_TABLES.TABLE_PARA的数据表。
                 if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                 {
                     DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                     Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                     
                     string categoryKey="";
                     if(htParams.Contains(FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY))
                     {
                         categoryKey = htParams[FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY].ToString();
                     }
                     string sql = string.Format(@"SELECT 0 AS CODE_NUMBER,A.REASON_CODE_NAME,A.REASON_CODE_KEY,A.REASON_CODE_TYPE
                                        FROM FMM_REASON_CODE A,FMM_REASON_R_CATEGORY B
                                        WHERE A.REASON_CODE_KEY=B.REASON_CODE_KEY
                                        AND B.CATEGORY_KEY='{0}' 
                                        ORDER BY A.REASON_CODE_TYPE DESC ,B.CODE_INDEX ",
                                        categoryKey.PreventSQLInjection());
                     //执行查询。
                     dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
                     SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                 }
             }
             catch (Exception ex)
             {
                 SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("GetReasonCode Error: " + ex.Message);
             }

             System.DateTime endTime = System.DateTime.Now;
             LogService.LogInfo("GetReasonCode Time: " + (endTime - startTime).TotalMilliseconds.ToString());

             return dsReturn;
        }

        /// <summary>
        /// 获取原因代码组数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。键值对类型的数据表
        /// <see cref="FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE"/>
        /// </param>
        /// <returns>
        /// 包含原因代码组数据的数据集。
        /// 【REASON_CODE_CATEGORY_KEY, REASON_CODE_CATEGORY_NAME,
        /// DESCRIPTIONS,  EDITOR,  EDIT_TIME, EDIT_TIMEZONE,  REASON_CODE_CATEGORY_TYPE】
        /// </returns>
        public DataSet GetReasonCodeGroup(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            //define sqlCommond 
            string sqlCommond = string.Empty;
            string categoryType = "";
            try
            {
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE))
                    {
                        categoryType = htParams[FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE].ToString();
                    }                        
                    sqlCommond = @"SELECT REASON_CODE_CATEGORY_KEY,
                                          REASON_CODE_CATEGORY_NAME,
                                          DESCRIPTIONS,
                                          EDITOR,
                                          EDIT_TIME,
                                          EDIT_TIMEZONE,
                                          REASON_CODE_CATEGORY_TYPE
                                     FROM FMM_REASON_CODE_CATEGORY
                                    WHERE REASON_CODE_CATEGORY_TYPE='"+categoryType.PreventSQLInjection()+"'";
                    if (htParams.ContainsKey(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME))
                    {
                        string categoryName = Convert.ToString(htParams[FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME] );
                        sqlCommond += " AND REASON_CODE_CATEGORY_NAME = '" + categoryName.PreventSQLInjection()+ "'";
                    }
                    sqlCommond += " ORDER BY 2";                   
                }
                DataTable dtTable = new DataTable();
                dtTable = db.ExecuteDataSet(CommandType.Text, sqlCommond).Tables[0];
                dtTable.TableName = FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtTable, false, MissingSchemaAction.Add);
                //add paramter table
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");

            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetReasonCodeGroup Error: " + ex.Message);
            }
            DateTime endTime = DateTime.Now;
            LogService.LogInfo("GetReasonCodeGroup Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }

        /// <summary>
        /// 检查指定工艺流程是否可以用在指定线别。
        /// </summary>
        /// <param name="dsParams">包含工艺流程和线别主键的数据集对象。
        /// ----------------------------------------
        /// CUR_ROUTE_VER_KEY，CUR_PRODUCTION_LINE_KEY
        /// ----------------------------------------
        /// </param>
        /// <returns>
        /// 包含执行结果的数据集对象。true:表示可以用在指定线别，否则不能。
        /// </returns>
        public DataSet CheckLineOfRoute(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            try
            {
                if (dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                    Hashtable htParams =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    string routeKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                    string lineKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                    string sql =string.Format(@"SELECT COUNT(*) FROM POR_ROUTE_VER_LINE 
                                                WHERE ROUTE_ROUTE_VER_KEY='{0}' AND PRODUCTION_LINE_KEY='{1}'",
                                                routeKey.PreventSQLInjection(),
                                                lineKey.PreventSQLInjection());
                    int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                    if (count>0)
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "true");
                    }
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("CheckLineOfRoute Error: " + ex.Message);
            }

            DateTime endTime = DateTime.Now;
            LogService.LogInfo("CheckLineOfRoute Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }

        /// <summary>
        /// 设置批次在当前的工步自定义属性值。
        /// </summary>
        /// <param name="dsParams">
        /// 包含批次自定义属性数据的数据集对象。
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SetLotAttributes(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;

            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;            
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                //Create Transaction  
                dbTran = dbConn.BeginTransaction();
                WipManagement.SetLotAttributes(db, dbTran, dsParams); 
                dbTran.Commit();
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SetLotAttributes Error: " + ex.Message);
                dbTran.Rollback();
            }
            finally
            {
                dbConn.Close();
                dbTran.Dispose();
                dbConn.Dispose();
            }
            DateTime endTime = DateTime.Now;
            LogService.LogInfo("SetLotAttributes Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }

        /// <summary>
        /// 获取批次在当前的工步自定义属性值。
        /// </summary>
        /// <param name="dataset">包含批次主键和自定义属性名称的数据集对象。
        /// ----------------------------------
        /// POR_LOT：
        /// {LOT_KEY}
        /// ----------------------------------
        /// ----------------------------------
        /// POR_ROUTE_STEP_ATTR：
        /// {ATTRIBUTE_NAME}:使用逗号分隔的自定义属性名称，attr1,attr2...
        /// ----------------------------------
        /// </param>
        /// <returns>包含自定义属性值的数据集对象。</returns>
        public DataSet GetAttributeValueOfStep(DataSet dsParams)
        {
            DateTime startTime = DateTime.Now;
            DataSet dsReturn = new DataSet();
            string sql = "",attributeNameList = "",attributeNameCondition="";
            try
            {
                if (dsParams != null && dsParams.Tables.Count > 0)
                {                    
                    if (dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME))
                    {
                        string lotKey = Convert.ToString(dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY]);
                        sql = @"SELECT A.ATTRIBUTE_NAME,A.ATTRIBUTE_VALUE
                                FROM POR_ROUTE_STEP_ATTR A,POR_LOT B
                                WHERE A.ROUTE_STEP_KEY =B.CUR_STEP_VER_KEY
                                AND B.LOT_KEY ='" + lotKey.PreventSQLInjection()+ "'";
                    }
                    if (dsParams.Tables.Contains(POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME))
                    {
                        attributeNameList = dsParams.Tables[POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_NAME].ToString();
                        attributeNameCondition = UtilHelper.BuilderWhereConditionString("A.ATTRIBUTE_NAME", attributeNameList.Split(','));
                        sql = sql + attributeNameCondition;
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");                
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAttributeValueOfStep Error: " + ex.Message);
            }

            DateTime endTime = DateTime.Now;
            LogService.LogInfo("GetAttributeValueOfStep Time: " + (endTime - startTime).TotalMilliseconds.ToString());
            return dsReturn;
        }

        /// <summary>
        /// 更新批次状态。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stateFlag">状态标记。</param>
        public void UpdateLotState(string lotKey, int stateFlag)
        {
            UpdateLotState(null, lotKey, stateFlag);
        }

        /// <summary>
        /// 更新批次状态。
        /// </summary>
        /// <param name="DbTransaction">数据库事务对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stateFlag">状态标记。</param>
        public void UpdateLotState(DbTransaction dbtran, string lotKey, int stateFlag)
        {
            string sql = string.Format(@"UPDATE POR_LOT
                                         SET STATE_FLAG={0}  ,EDIT_TIME=GETDATE() 
                                         WHERE LOT_KEY='{1}'", stateFlag,
                                         lotKey.PreventSQLInjection());
            if (dbtran == null)
            {
                db.ExecuteNonQuery(CommandType.Text, sql);
            }
            else
            {
                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
            }
        }

        /// <summary>
        /// 记录在制品操作错误消息。
        /// </summary>
        /// <param name="title">消息标题。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="toUser">消息到达用户。</param>
        /// <param name="group">消息到达角色。</param>
        /// <param name="user">发送消息用户。</param>
        /// <param name="timeZone">时区。</param>
        /// <param name="objectKey">消息类型的对象主键。</param>
        /// <param name="objectType">消息类型</param>
        public void RecordErrorMessage(string title, string message, string toUser, string group,
                                       string user, string timeZone, string objectKey, string objectType)
        {
            RecordErrorMessage(db, title, message, toUser, group, user, timeZone, objectKey, objectType);
        }
        /// <summary>
        /// 记录批次自动进站或自动出站错误消息。
        /// </summary>
        /// <param name="title">消息标题。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="toUser">消息到达用户。</param>
        /// <param name="group">消息到达角色。</param>
        /// <param name="user">发送消息用户。</param>
        /// <param name="timeZone">时区。</param>
        /// <param name="objectKey">消息类型的对象主键。</param>
        /// <param name="objectType">消息类型</param>
        public static void RecordErrorMessage(Database db,
                                        string title, string message, string toUser, string group,
                                        string user, string timeZone, string objectKey, string objectType)
        {
            WIP_MESSAGE_FIELDS wef = new WIP_MESSAGE_FIELDS();
            string rowKey = string.Empty;
            string sql = string.Empty;

            Hashtable hsTable = new Hashtable();
            rowKey = UtilHelper.GenerateNewKey(0);

            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_ROW_KEY, rowKey);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_TITLE, title);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CONTEXT, message);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CRITICAL_LEVEL, "0");
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_PUBLIC_LEVEL, "0");
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_TO_USER, toUser);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_TO_GROUP, group);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_STATUS, "0");
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CREATOR, user);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CREATE_TIME, null);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_CREATE_TIMEZONE, timeZone);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_OBJECTKEY, objectKey);
            hsTable.Add(WIP_MESSAGE_FIELDS.FIELD_OBJECTTYPE, objectType);

            sql = DatabaseTable.BuildInsertSqlStatement(wef, hsTable, null);
            db.ExecuteNonQuery(CommandType.Text, sql);
        }
        

        /// <summary>
        /// 根据批次号、工序名称、操作线别判断是否可以进行报废数据收集。
        /// 符合如下条件的才能进行报废数据收集：
        /// （1）工序相同,且已进站。
        /// （2）工序不同，当前工序为等待进站状态,并且当前工序的上一工序是指定工序
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <param name="operationName">进行报废数据收集的工序名称。</param>
        /// <param name="operateLineName">进行报废数据收集的操作线别名称。</param>
        /// <returns>包含报废工步数据的数据集对象。</returns>
        public DataSet CheckSetLoss(string lotNo, string operationName, string operateLineName)
        {
            DataSet dsReturn=new DataSet ();
            string sql = string.Empty;
            try
            {                
                string enterpriseVerKey = "";
                string routeVerKey = "";
                string stepKey = "";
                string getOperationName = "", stateFlag = "",strLotKey="",oprLine="";
                string preStepName="";
                DataSet dsLot = GetLotsInfo(lotNo);
                enterpriseVerKey    = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                routeVerKey         = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
                stepKey             = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                getOperationName    = dsLot.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
                stateFlag           = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();                
                strLotKey           = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                oprLine             = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();
                DataTable dtTable = new DataTable();
                //工序相同,但未进站
                if (getOperationName == operationName && Convert.ToInt32(stateFlag) < 4)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "当前批次在指定工序不能执行报废操作。");//进站前不能做本工序的报废操作
                    return dsReturn;
                }
                //工序相同,但已进站
                if (getOperationName == operationName && Convert.ToInt32(stateFlag) >= 4)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, "true");// 0可执行setloss  
                    return dsReturn;
                }
                //工序不同但当前工序已经进站
                if (getOperationName != operationName && Convert.ToInt32(stateFlag) >= 4)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "当前批次在指定工序不能执行报废操作。");//1不能执行SetLoss
                    return dsReturn;
                }
                //工序不同，当前工序为等待进站状态,并且当前工序的上一工序是指定工序
                else
                {
                    #region 工序不同，当前工序为等待进站状态，查找工序是否是连续的
                    sql = string.Format(@"SELECT TOP 1 T.ENTERPRISE_KEY,T.ROUTE_KEY,T.STEP_KEY,S.ROUTE_STEP_NAME
                                        FROM WIP_TRANSACTION T
                                        LEFT JOIN POR_ROUTE_STEP S ON S.ROUTE_STEP_KEY=T.STEP_KEY
                                        WHERE T.ACTIVITY = '{0}'
                                            AND T.PIECE_KEY = '{1}'
                                            AND T.UNDO_FLAG = 0
                                        ORDER BY T.TIME_STAMP DESC", ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT, 
                                        strLotKey.PreventSQLInjection());
                    //执行查询作业。
                    dtTable =new DataView(db.ExecuteDataSet(CommandType.Text, sql).Tables[0]).ToTable();
                    if (dtTable.Rows.Count > 0)//查询成功
                    {                  
                        preStepName = dtTable.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
                        //如果查询到的上一个工步名称是指定的工序名称。
                        if (preStepName == operationName)
                        {
                            //返回包含上一步工步信息的数据表。可以执行报废数据收集作业
                            dsReturn.Tables.Add(dtTable);
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, "true");// 0可执行setloss                       
                        }
                        else
                        {
                            //如果查询到的上一个工步名称不是指定的工序名称。
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "当前批次在指定工序不能执行报废操作。");//1不能执行SetLoss
                            return dsReturn;
                        }
                    }
                    else//没有找到上一个工序，执行出错。
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, "当前批次在指定工序不能执行报废操作");//2执行出错
                        return dsReturn;
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, ex.Message);//2执行出错
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据批次主键获取可退库或返工的线边仓数据集。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 获取的包含可退库或返工的线边仓信息的数据集。
        /// </returns>
        public DataSet GetStoreList(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string onLineStore = string.Empty;
                //获取批次当前的工步主键
                string sql = "SELECT CUR_STEP_VER_KEY,FACTORYROOM_KEY FROM POR_LOT WHERE LOT_KEY='" + lotKey.PreventSQLInjection() + "'";
                DataSet ds = db.ExecuteDataSet(CommandType.Text, sql);
                string stepKey = string.Empty;
                string factoryRoomKey = string.Empty;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    stepKey = Convert.ToString(ds.Tables[0].Rows[0]["CUR_STEP_VER_KEY"]);
                    factoryRoomKey = Convert.ToString(ds.Tables[0].Rows[0]["FACTORYROOM_KEY"]);
                }

                if (!string.IsNullOrEmpty(stepKey))
                {                    
                    //从工步自定义属性表中获取工步设置的可返工或退库的线别仓字符串，用逗号（,）分割。
                    onLineStore = GetStepUdaValue(db, stepKey.ToString(), "OnLineStore");
                    if (onLineStore.Length > 0)
                    {
                        //从WST_STORE表中获取可返工或退库的线边仓数据。
                        sql = string.Format("SELECT 0 AS CODE_NUMBER,A.* FROM WST_STORE A WHERE A.LOCATION_KEY='{0}' ",
                                            factoryRoomKey.PreventSQLInjection());
                        sql = sql + UtilHelper.BuilderWhereConditionString("A.STORE_NAME", onLineStore.Split(','));
                        dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                }
                else
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn,"step key is empty");
                }
            }
            catch(Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        
        /// <summary>
        /// 根据批次号检查是否能进行缺片(批次退料)操作。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>
        /// 获取包含是否能进行缺片(批次退料)操作信息的数据集。
        /// </returns>
        public DataSet CheckLossBattery(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            string enterpriseVerKey = "";
            string routeVerKey = "";
            string stepKey = "";
            string getOperationName = "", stateFlag = "",strLotKey="",oprLine="";           
            try
            {
                //获取批次信息。
                DataSet dsLot = GetLotsInfo(lotNumber);
                enterpriseVerKey = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                routeVerKey = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
                stepKey = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                getOperationName = dsLot.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
                stateFlag = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString();
                strLotKey = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                oprLine = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();
                //查询第一个工步的主键。
                sqlCommand = string.Format(@"SELECT TOP 1 T.ROUTE_STEP_KEY
                                          FROM V_PROCESS_PLAN T
                                          WHERE T.ROUTE_ENTERPRISE_VER_KEY = '{0}'
                                          AND T.ROUTE_ROUTE_VER_KEY = '{1}'
                                          ORDER BY ROUTE_STEP_SEQ ASC", 
                                          enterpriseVerKey.PreventSQLInjection(), 
                                          routeVerKey.PreventSQLInjection());
                object objStepKey = db.ExecuteScalar(CommandType.Text, sqlCommand);
                //获取工步的主键成功。
                if (objStepKey != null && objStepKey != DBNull.Value)
                {
                    //工步的主键不是第一道工步的主键，则给出提示。
                    if (objStepKey.ToString() != stepKey)
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "只有在第一道工序才能做批次退料操作");
                    }
                    else
                    {
                        //批次在工序中已经进站，给出提示。
                        if (Convert.ToInt32(stateFlag) >= 4)
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "工序已经进站，不能再做批次退料操作");
                        }
                        else//可以进行缺片操作。
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, "true");
                        }
                    }
                }
                else//查询工序序号失败。
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, "查询工序序号失败，批次退料失败。");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, ex.Message);
            }           
            return dsReturn;
        }
        /// <summary>
        /// 获取退料原因代码
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含退料原因代码的数据集对象。</returns>
        public DataSet GetRetMatReasonCode(string stepKey)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DataSet dsReturn = new DataSet();
            try
            {
                string sql  = @"SELECT  0 AS CODE_NUMBER, T2.REASON_CODE_NAME, T2.REASON_CODE_KEY,T2.REASON_CODE_TYPE
                                FROM(SELECT *
                                     FROM FMM_REASON_R_CATEGORY
                                     WHERE category_key =(SELECT TOP 1 REASON_CODE_CATEGORY_KEY
						                                  FROM FMM_REASON_CODE_CATEGORY
                                                          WHERE REASON_CODE_CATEGORY_NAME =(SELECT TOP 1 ATTRIBUTE_VALUE
												                                            FROM POR_ROUTE_STEP_ATTR
												                                            WHERE ROUTE_STEP_KEY ='{0}' AND ATTRIBUTE_NAME = 'ReturnCode'))) T1
                                LEFT JOIN FMM_REASON_CODE T2 ON T1.REASON_CODE_KEY = T2.REASON_CODE_KEY";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, stepKey.PreventSQLInjection()));
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRetMatReasonCode Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 执行批次缺片操作。
        /// </summary>
        /// <param name="dataTable">
        /// 数据表对象，数据表的名称为"param"，数据表包含两个列"name"和"value"。
        /// 列name存放的键名，列value存放对应的键值。
        /// 键名:
        /// 批次号（POR_LOT_FIELDS.FIELD_LOT_NUMBER），
        /// 线别名称（POR_LOT_FIELDS.FIELD_LINE_NAME），
        /// 当前线别主键（POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY），
        /// 操作线别（POR_LOT_FIELDS.FIELD_OPR_LINE），
        /// 操作人员（POR_LOT_FIELDS.FIELD_OPERATOR），
        /// 操作的计算机（WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER），
        /// 编辑人（WIP_TRANSACTION_FIELDS.FIELD_EDITOR），
        /// 编辑时区（WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY），
        /// 班别名称（WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME），
        /// 缺片数量（QUANTITY_SETLOSS）。
        /// </param>
        /// <returns>
        /// 包含执行结果数据的数据集对象。
        /// </returns>
        public DataSet SetLossBattery(DataTable dataTable)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            string sql = string.Empty;
            try
            {
                Hashtable hashTable =SolarViewer.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                string lotNumber = hashTable[POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
                string lineName = hashTable[POR_LOT_FIELDS.FIELD_LINE_NAME].ToString();
                string lineKey = hashTable[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString();
                string userName=hashTable[POR_LOT_FIELDS.FIELD_OPERATOR].ToString();
                string oprLine=hashTable[POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();
                string editor=hashTable[WIP_TRANSACTION_FIELDS.FIELD_EDITOR].ToString();
                string editTimeZone=hashTable[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY].ToString();
                string oprComputer=hashTable[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER].ToString();
                string editTime = UtilHelper.GetSysdate(db).ToString("yyyy-MM-dd HH:mm:ss");
                string shiftName = Convert.ToString(hashTable[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
                string shiftKey = Convert.ToString(hashTable[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);

                //根据批次号码获取批次信息。
                DataSet dsLotInfo = GetLotsInfo(lotNumber);
                DataTable lotTable = dsLotInfo.Tables[0];
                int setLossQty = Convert.ToInt32(hashTable["QUANTITY_SETLOSS"]);//缺片数量
                int quantityIn = Convert.ToInt32(lotTable.Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);//批次总数
                int reworkFlag = Convert.ToInt32(lotTable.Rows[0]["REWORK_FLAG"]);
                int stateFlag = Convert.ToInt32(lotTable.Rows[0]["STATE_FLAG"]);
                int deletedTermFlag = Convert.ToInt32(lotTable.Rows[0]["DELETED_TERM_FLAG"]);

                string materialLot = Convert.ToString(lotTable.Rows[0]["MATERIAL_LOT"]);
                string materialCode = Convert.ToString(lotTable.Rows[0]["MATERIAL_CODE"]);
                string factoryRoomKey = Convert.ToString(lotTable.Rows[0]["FACTORYROOM_KEY"]);
                string operationName = Convert.ToString(lotTable.Rows[0]["ROUTE_OPERATION_NAME"]);
                string createOperationName = Convert.ToString(lotTable.Rows[0]["CREATE_OPERTION_NAME"]);
                string workOrderKey = Convert.ToString(lotTable.Rows[0]["WORK_ORDER_KEY"]);

                //缺片数量>批次总数量。
                if (setLossQty > quantityIn)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "缺片数量不能大于批次总数。");
                    return dsReturn;
                }

                if (deletedTermFlag != 0 || stateFlag>=10)
                {
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "批次已结束或已删除，不能进行批次退料。");
                    return dsReturn;
                }
                
                #region Wip Transaction
                WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
                Hashtable parenttransactionTable = new Hashtable();
                string strParentTransKey = UtilHelper.GenerateNewKey(0);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, strParentTransKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, setLossQty.ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, setLossQty.ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR,editor);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY,lineKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, "0");
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR,userName);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, lotTable.Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED].ToString());
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE,oprLine);
                parenttransactionTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER,oprComputer);

                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran=dbConn.BeginTransaction();
                try
                {
                    int ret = 0;
                    //插入一笔缺片操作记录。 COMMON_FIELDS.FIELD_ACTIVITY_LOSSBATTERY
                    sql = DatabaseTable.BuildInsertSqlStatement(wipFields, parenttransactionTable, null);
                    ret=db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //根据批次号码更新批次数量，线别名称，当前线别主键，操作线别，操作计算机，编辑人，编辑时间，编辑时区，操作人。
                    sql = string.Format(@"UPDATE POR_LOT 
                                          SET QUANTITY=QUANTITY-{0},LINE_NAME='{1}',
                                              CUR_PRODUCTION_LINE_KEY='{2}',OPR_LINE='{3}',
                                              OPR_COMPUTER='{4}',EDITOR='{5}',EDIT_TIME= GETDATE(),
                                              EDIT_TIMEZONE='{6}',OPERATOR='{7}' 
                                          WHERE LOT_NUMBER='{8}'",
                                          setLossQty,lineName.PreventSQLInjection(),
                                          lineKey.PreventSQLInjection(),
                                          oprLine.PreventSQLInjection(),
                                          oprComputer.PreventSQLInjection(),
                                          editor.PreventSQLInjection(),
                                          editTimeZone.PreventSQLInjection(),
                                          userName.PreventSQLInjection(),
                                          lotNumber.PreventSQLInjection());
                    ret = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //如果是正常批次。
                    if (reworkFlag == 0)
                    {
                        //更新工单剩余数量。
                        sql = string.Format(@"UPDATE POR_WORK_ORDER 
                                              SET    QUANTITY_LEFT = QUANTITY_LEFT + {0}
                                              WHERE  WORK_ORDER_KEY = '{1}'",
                                             setLossQty, workOrderKey.PreventSQLInjection());
                        ret=db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                        #region 正常批次批次退料后不进行数量回加 modify by peter 20120621
//                        string storeKey = string.Empty;
//                        //根据车间主键+工序+仓库类型获取线上仓主键。
//                        sql = string.Format(@"SELECT STORE_KEY,STORE_NAME
//                                                FROM WST_STORE 
//                                                WHERE STORE_TYPE='9' AND LOCATION_KEY='{0}' AND OPERATION_NAME='{1}'",
//                                                    factoryRoomKey, createOperationName);
//                        DataSet ds = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
//                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//                        {
//                            storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
//                        }
//                        string storeMaterialDetailKey = string.Empty;
//                        //根据线上仓主键 + 物料批号键获取线上仓物料明细主键。
//                        sql = string.Format(@"SELECT b.STORE_MATERIAL_DETAIL_KEY
//                                            FROM WST_STORE_MATERIAL a
//                                              LEFT JOIN POR_MATERIAL c ON a.MATERIAL_KEY=c.MATERIAL_KEY
//                                            LEFT JOIN WST_STORE_MATERIAL_DETAIL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
//                                            WHERE a.STORE_KEY='{0}' AND b.MATERIAL_LOT='{1}' AND c.MATERIAL_CODE='{2}'", storeKey, materialLot, materialCode);
//                        ds = db.ExecuteDataSet(dbtran, CommandType.Text, sql);
//                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//                        {
//                            storeMaterialDetailKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"]);
//                        }
//                        //更新线上仓+物料批次数量。
//                        sql = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL SET CURRENT_QTY=CURRENT_QTY+{0}
//                                                 WHERE STORE_MATERIAL_DETAIL_KEY='{1}'", setLossQty, storeMaterialDetailKey);
                        //                        ret = db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                       #endregion
                    }
                    else if (reworkFlag == 1)
                    {
                        string storeKey = string.Empty;
                        //获取重工线上仓主键。
                        sql = string.Format(@"SELECT K.STORE_KEY 
                                            FROM WST_STORE K 
                                            WHERE K.STORE_TYPE = 0 
                                            AND K.LOCATION_KEY =  '{0}' 
                                            AND K.OPERATION_NAME =  '{1}'",
                                            factoryRoomKey.PreventSQLInjection(), 
                                            createOperationName.PreventSQLInjection());
                        DataSet ds = db.ExecuteDataSet(dbTran, CommandType.Text, sql);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            storeKey = Convert.ToString(ds.Tables[0].Rows[0]["STORE_KEY"]);
                        }
                        //更新工单返工数量。
                        sql = string.Format(@"UPDATE POR_WORK_ORDER 
                                             SET     QUANTITY_REWORK =ISNULL(QUANTITY_REWORK,0) + {0}
                                             WHERE   WORK_ORDER_KEY = '{1}'", 
                                             setLossQty, workOrderKey.PreventSQLInjection());
                        ret = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        //更新重工线上仓数据。
                        sql = string.Format(@"UPDATE t 
                                              SET t.SUM_QUANTITY=t.SUM_QUANTITY+{0}
                                              FROM WST_STORE_SUM t
                                              WHERE t.WORKORDER_NUMBER=(SELECT P.ORDER_NUMBER FROM POR_WORK_ORDER P WHERE P.WORK_ORDER_KEY='{1}')
                                              AND t.STORE_KEY = '{2}'",
                                              setLossQty, 
                                              workOrderKey.PreventSQLInjection(), 
                                              storeKey.PreventSQLInjection());
                        ret = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    dbTran.Commit();
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                }
                finally
                {
                    dbConn.Close();
                    dbTran.Dispose();
                    dbConn.Dispose();
                }
                #endregion
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SetLossBattery Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取时间控制信息的数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含时间控制信息的数据集。
        /// [CurTime,TimeControlBase,TimeControlMin,TimeControlWarning,TimeControlMax]
        /// </returns>
        public DataSet GetTimeControlInfo(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                if (lotKey != null)
                {
                    sql = @"SELECT GETDATE() AS CURTIME,
                                   MAX(CASE  WHEN ATTRIBUTE_NAME = 'TimeControlBase' THEN  A.ATTRIBUTE_VALUE  END) AS TimeControlBase,
                                   MAX(CASE  WHEN ATTRIBUTE_NAME = 'TimeControlMin' THEN  A.ATTRIBUTE_VALUE  END) AS TimeControlMin,
                                   MAX(CASE  WHEN ATTRIBUTE_NAME = 'TimeControlWarning' THEN  A.ATTRIBUTE_VALUE  END) AS TimeControlWarning,
                                   MAX(CASE  WHEN ATTRIBUTE_NAME = 'TimeControlMax' THEN  A.ATTRIBUTE_VALUE  END) AS TimeControlMax
                             FROM POR_ROUTE_STEP_ATTR A, POR_LOT T
                             WHERE T.CUR_STEP_VER_KEY = A.ROUTE_STEP_KEY
                             AND T.LOT_KEY = '{0}'
                             GROUP BY T.CUR_STEP_VER_KEY";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql,lotKey));
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTimeControlInfo Error: " + ex.Message);
            }
            return dsReturn;
        } 

        /// <summary>
        /// 根据批次主键或批次号获取批次进站是否超时。
        /// </summary>
        /// <param name="lotKey">批次主键或批次号</param>
        /// <returns>
        /// true表示超时，false表示没有超时。
        /// </returns>
        public bool GetLotTrackInIsDelay(string lotKey)
        {
            bool bIsDelay = false;
            string sql = "";
            try
            {
                //批次号或批次主键不为null
                if (lotKey != null)
                {
                    long iMaxTrackInTime = 60;
                    //获取批次信息。
                    DataSet waitData = GetLotsInfo(lotKey);
                    //获取批次信息成功。
                    if (waitData.Tables.Count > 0 && waitData.Tables[0].Rows.Count > 0)
                    {
                        
                        DateTime startWaitTime = Convert.ToDateTime(waitData.Tables[0].Rows[0]["START_WAIT_TIME"]);
                        DateTime curTime = Convert.ToDateTime(waitData.Tables[0].Rows[0]["CURTIME"]);

                        TimeSpan ts1 = new TimeSpan(curTime.Ticks);
                        TimeSpan ts2 = new TimeSpan(startWaitTime.Ticks);

                        TimeSpan ts = ts1 - ts2;
                        //分钟数
                        long its = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;  //unit is minute

                        string strStepKey = Convert.ToString(waitData.Tables[0].Rows[0]["CUR_STEP_VER_KEY"]);
                        if (!string.IsNullOrEmpty(strStepKey))
                        {
                            //查询当前工步的时间控制，批次最大进站等待时间MAXTRACKINTIME，默认为60
                            sql = @"SELECT ISNULL(T.MAXTRACKINTIME,60) 
                                    FROM V_ROUTE_STEP_TIMECONTROL T 
                                    WHERE T.ROUTE_STEP_KEY='" + strStepKey.PreventSQLInjection() + "'";
                            iMaxTrackInTime = Convert.ToInt64(db.ExecuteScalar(CommandType.Text, sql));
                        }
                        //如果批次进站等待分钟数，大于批次最大进站等待时间，表示超时。
                        if (its > iMaxTrackInTime)
                        {
                            bIsDelay = true;
                        }
                        else
                        {//否则，表示没有超时。
                            bIsDelay = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogService.LogError("GetTrackInTimeControlInfo Error: " + ex.Message);
                throw ex;
            }
            return bIsDelay;
        }

        /// <summary>
        /// 根据批次主键获取批次的时间控制数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次时间控制数据的数据集对象。数据集中包含两个数据表对象。
        /// 有一个名称为TrackOutStatus的数据表对象，数据表的列名为
        /// TimeStatusFlag
        /// （时间状态，
        /// 0：加工时间没有满足最小加工时间。
        /// 1:加工时间满足最小时间，没有超过报警时间。
        /// 2:加工时间超过报警时间，没有超过最大加工时间。
        /// 3:加工时间超过最大加工时间。
        /// ） 
        /// TimeControlBaseSubMin（基础加工时间-最小加工时间）。
        /// </returns>
        public DataSet GetLotTrackOutTimeControlStatus(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = "";
                string strStartTime = "";
                //string strCurTime = "";
                string strQuantity = "";
                string strStepName = "";
                string strStepKey = "";

                float fTimeControlBase = 22.5f;
                float fTimeControlMin = 3;
                float fTimeControlWarning = 15;
                float fTimeControlMax = 30;
                int iQty = 0;

                string TimeStatusFlag = "1";
                //批次主键不等于null
                if (lotKey != null)
                {
                    DataSet inData = GetLotsInfo(lotKey);
                    //获取批次数据成功。
                    if (inData.Tables.Count > 0 && inData.Tables[0].Rows.Count > 0)
                    {
                        strStartTime = inData.Tables[0].Rows[0]["START_PROCESS_TIME"].ToString();   //批次开始操作时间
                        strQuantity = inData.Tables[0].Rows[0]["QUANTITY"].ToString();              //数量
                        strStepName = inData.Tables[0].Rows[0]["ROUTE_STEP_NAME"].ToString();       //工步名
                        strStepKey = inData.Tables[0].Rows[0]["CUR_STEP_VER_KEY"].ToString();       //当前工步主键

                        DateTime curTime = Convert.ToDateTime(inData.Tables[0].Rows[0]["CURTIME"]); //当前时间

                        if (!string.IsNullOrEmpty(strStepKey))
                        {
                            //根据工步主键获取工步的时间控制数据。
                            sql = "SELECT * FROM V_ROUTE_STEP_TIMECONTROL T WHERE T.ROUTE_STEP_KEY='" + strStepKey.PreventSQLInjection() + "'";

                            DataSet timeControlData =  db.ExecuteDataSet(CommandType.Text, sql);
                            //获取工步的时间控制数据成功。
                            if (timeControlData.Tables.Count > 0 && timeControlData.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(timeControlData.Tables[0].Rows[0]["TimeControlBase"].ToString()))
                                    fTimeControlBase = Convert.ToSingle(timeControlData.Tables[0].Rows[0]["TimeControlBase"].ToString());
                                if (!string.IsNullOrEmpty(timeControlData.Tables[0].Rows[0]["TimeControlMin"].ToString()))
                                    fTimeControlMin = Convert.ToSingle(timeControlData.Tables[0].Rows[0]["TimeControlMin"].ToString());
                                if (!string.IsNullOrEmpty(timeControlData.Tables[0].Rows[0]["TimeControlWarning"].ToString()))
                                    fTimeControlWarning = Convert.ToSingle(timeControlData.Tables[0].Rows[0]["TimeControlWarning"].ToString());
                                if (!string.IsNullOrEmpty(timeControlData.Tables[0].Rows[0]["TimeControlMax"].ToString()))
                                    fTimeControlMax = Convert.ToSingle(timeControlData.Tables[0].Rows[0]["TimeControlMax"].ToString());
                            }
                        }

                        if (strQuantity != "")
                        {
                            iQty = Convert.ToInt32(strQuantity);
                        }
                        TimeSpan ts2 = new TimeSpan(curTime.Ticks);
                        if (!string.IsNullOrEmpty(strStartTime))
                        {
                            DateTime startTime = Convert.ToDateTime(strStartTime);
                            ts2 = new TimeSpan(startTime.Ticks);
                        }
                        TimeSpan ts1 = new TimeSpan(curTime.Ticks);
                        
                        //开始时间和当前时间的时间差。加工时间
                        TimeSpan ts = ts1 - ts2;
                        //加工时间换算成分钟数。
                        float fts = (float)(ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds) / 60;  //unit is minute

                        if (fts < fTimeControlBase - fTimeControlMin)//加工时间没有满足最小加工时间。
                            TimeStatusFlag = "0";
                        else if (fts < fTimeControlBase + fTimeControlWarning//加工时间满足最小时间，没有超过报警时间。
                            && fts >= fTimeControlBase - fTimeControlMin)
                            TimeStatusFlag = "1";
                        else if (fts >= fTimeControlBase + fTimeControlWarning//加工时间超过报警时间，没有超过最大加工时间。
                            && fts < fTimeControlBase + fTimeControlMax)
                            TimeStatusFlag = "2";
                        else//加工时间超过最大加工时间。
                            TimeStatusFlag = "3";
                    }

                    DataTable table = new DataTable();
                    table.Columns.Add("TimeStatusFlag");
                    table.Columns.Add("TimeControlBaseSubMin");
                    table.Rows.Add(TimeStatusFlag, (fTimeControlBase-fTimeControlMin).ToString());                    
                    table.TableName = "TrackOutStatus";
                    dsReturn.Merge(table, false, MissingSchemaAction.Add);
                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetTrackOutTimeControlInfo Error: " + ex.Message);

            }
            return dsReturn;
        }

        /// <summary>
        /// 判别班别是否存在。返回班别对应的主键标识符。
        /// </summary>
        /// <param name="shiftValue">班别值。</param>
        /// <returns>返回班别主键的标识字符串。空字符串代表班别不存在</returns>
        public string IsExistsShift(string shiftValue)
        {
            string shiftKey = string.Empty;
            try
            {
                string sql = string.Format(@"SELECT T.DKEY
                                            FROM CAL_SCHEDULE_DAY T, V_SHIFT_OVERTIME T1
                                            WHERE T.SHIFT_KEY = T1.SHIFT_KEY
                                            AND T.SHIFT_VALUE = '{0}'
                                            AND SYSDATETIME() BETWEEN T.STARTTIME AND DATEADD(mi, T1.OVERTIME,T.ENDTIME)", 
                                            shiftValue.PreventSQLInjection());
                object o = db.ExecuteScalar(CommandType.Text, sql);
                if (o != null && o != DBNull.Value)
                {
                    shiftKey = o.ToString();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("IsExistsShift Error: " + ex.Message);
            }
            return shiftKey;
        }

        /// <summary>
        /// 根据当前日期获取班别名称。
        /// </summary>
        /// <returns>班别名称。</returns>
        public string GetShiftNameBySysdate()
        {
            string shiftName = string.Empty;
            try
            {
                string sql = @"SELECT TT.SHIFT_VALUE
                            FROM 
                            (
                                SELECT T.SHIFT_VALUE, ROW_NUMBER() OVER(ORDER BY T.STARTTIME DESC) SEQNUM
                                FROM CAL_SCHEDULE_DAY T, V_SHIFT_OVERTIME T1
                                WHERE  SYSDATETIME() BETWEEN T.STARTTIME AND DATEADD(mi, T1.OVERTIME,T.ENDTIME) 
                                AND T.SHIFT_KEY = T1.SHIFT_KEY
                            ) TT
                            WHERE TT.SEQNUM = 1";
                object o = db.ExecuteScalar(CommandType.Text, sql);
                if (o != null && o != DBNull.Value)
                {
                    shiftName = o.ToString();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetShiftNameBySysdate Error: " + ex.Message);
            }
            return shiftName;
        }
        /// <summary>
        /// 根据线别主键获取工单信息。
        /// </summary>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>
        /// 包含工单信息的数据集对象。
        /// </returns>
        public DataSet SearchWorkOrderInformationByLine(string lineKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = "";
            try
            {
                sql = string.Format(@"SELECT DISTINCT A.WORK_ORDER_KEY,A.ORDER_NUMBER,B.LINE_NAME,C.PRODUCTION_LINE_KEY AS LINE_KEY
                                      FROM POR_WORK_ORDER A 
                                      LEFT JOIN POR_LOT B ON A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                      LEFT JOIN FMM_PRODUCTION_LINE C ON B.LINE_NAME = C.LINE_NAME 
                                      WHERE A.ORDER_STATE = '1' AND B.LINE_NAME IS NOT NULL 
                                      AND C.PRODUCTION_LINE_KEY = '{0}'
                                      ORDER BY A.ORDER_NUMBER ", lineKey.PreventSQLInjection());

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchWorkOrderInformationByLine Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
