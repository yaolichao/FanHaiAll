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
using System.Transactions;


namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 包含批次操作方法的类-包装。
    /// </summary>
    public partial class LotOperationEngine : AbstractEngine, ILotOperationEngine
    {
        /// <summary>
        /// 是否检查组件的IV测试数据。
        /// </summary>
        /// <param name="lotNumber">组件序列号。</param>
        /// <returns>true：检查。false：不检查。</returns>
        public bool IsCheckIVTestData(string lotNumber)
        {
            string sql = string.Format(@"SELECT a.ATTRIBUTE_VALUE
                                        FROM POR_WORK_ORDER_ATTR a
                                        INNER JOIN POR_WORK_ORDER b ON a.WORK_ORDER_KEY=b.WORK_ORDER_KEY
                                        INNER JOIN POR_LOT c ON c.WORK_ORDER_KEY=b.WORK_ORDER_KEY
                                        WHERE a.ATTRIBUTE_NAME='IsCheckIVTestData'
                                        AND a.ISFLAG=1
                                        AND c.LOT_NUMBER='{0}'",
                                        lotNumber.PreventSQLInjection());
            object objIsCheckIVTestData = db.ExecuteScalar(CommandType.Text, sql);
            //默认检查IV测试数据
            if (objIsCheckIVTestData == DBNull.Value || objIsCheckIVTestData == null)
            {
                return true;
            }
            bool isCheckIVTestData = true;
            if (!bool.TryParse(Convert.ToString(objIsCheckIVTestData), out isCheckIVTestData))
            {
                isCheckIVTestData = true;
            }
            return isCheckIVTestData;
        }
        /// <summary>
        /// 根据等级代码获取等级名称。
        /// </summary>
        /// <param name="gradeCode">等级代码。</param>
        /// <returns>等级名称。</returns>
        public string GetGradeName(string gradeCode)
        {
            string sqlCommand = string.Format(@"SELECT TOP 1 GRADE_NAME 
                                                FROM V_ProductGrade
                                                WHERE GRADE_CODE='{0}'",
                                                gradeCode.PreventSQLInjection());
            return Convert.ToString(db.ExecuteScalar(CommandType.Text, sqlCommand));
        }
        /// <summary>
        /// 根据分档主键获取分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="psKey">分档主键。</param>
        /// <returns>包含分档数据的数据集对象。</returns>
        public DataSet GetWOProductPowersetData(string orderNumber, string partNumber, string psKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_WORK_ORDER a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.ORDER_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT PS_CODE,PS_RULE,PS_SEQ,P_MIN,P_MAX,MODULE_NAME,PMAXSTAB
                                                        FROM POR_WO_PRD_PS a 
                                                        INNER JOIN POR_WORK_ORDER b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                                        WHERE a.POWERSET_KEY='{2}'
                                                        AND a.IS_USED='Y'
                                                        AND b.ORDER_NUMBER='{0}'
                                                        AND a.PART_NUMBER='{1}';
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        SELECT PS_CODE,PS_RULE,PS_SEQ,P_MIN,P_MAX,MODULE_NAME,PMAXSTAB
                                                        FROM BASE_POWERSET
                                                        WHERE POWERSET_KEY='{2}'
                                                        AND ISFLAG=1;
                                                    END",
                                                    orderNumber.PreventSQLInjection(),
                                                    partNumber.PreventSQLInjection(),
                                                    psKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetWOProductPowersetData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取包装批次数据。
        /// </summary>
        /// <param name="htParams">
        /// 允许参数使用的键值。
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_SIDECODE"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_NUMBER"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY"/>
        /// </param>
        /// <returns>包含包装批次数据的数据集对象。</returns>
        public DataSet GetPackageLotInfo(Hashtable htParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string where = string.Empty;
                if (htParams.Contains(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                {
                    string val = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                    where += string.Format("AND A.LOT_NUMBER='{0}'", val.PreventSQLInjection());
                }
                if (htParams.Contains(POR_LOT_FIELDS.FIELD_LOT_SIDECODE))
                {
                    string val = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_SIDECODE]);
                    where += string.Format("AND A.LOT_SIDECODE='{0}'", val.PreventSQLInjection());
                }
                if (htParams.Contains(POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE))
                {
                    string val = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]);
                    where += string.Format("AND A.LOT_CUSTOMERCODE='{0}'", val.PreventSQLInjection());
                }
                if (htParams.Contains(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                {
                    string val = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                    where += string.Format("AND A.FACTORYROOM_KEY='{0}'", val.PreventSQLInjection());
                }
                string sqlCommand = string.Format(@"SELECT  A.[LOT_KEY],A.[LOT_NUMBER],A.[WORK_ORDER_KEY],A.[WORK_ORDER_NO],A.[WORK_ORDER_SEQ]
                                                            ,A.[PART_VER_KEY],A.[PART_NUMBER],A.[PRO_ID],A.[PRO_LEVEL],A.[PRIORITY],A.[QUANTITY_INITIAL]
                                                            ,A.[QUANTITY],A.[ROUTE_ENTERPRISE_VER_KEY],A.[CUR_ROUTE_VER_KEY],A.[CUR_STEP_VER_KEY]
                                                            ,A.[CUR_PRODUCTION_LINE_KEY],A.[LINE_NAME],A.[START_WAIT_TIME],A.[START_PROCESS_TIME]
                                                            ,A.[EDC_INS_KEY],A.[STATE_FLAG],A.[IS_MAIN_LOT],A.[SPLIT_FLAG],A.[LOT_SEQ],A.[REWORK_FLAG]
                                                            ,A.[HOLD_FLAG],A.[SHIPPED_FLAG],A.[DELETED_TERM_FLAG],A.[IS_PRINT],A.[LOT_TYPE],A.[CREATE_TYPE]
                                                            ,A.[COLOR],A.[PALLET_NO],A.[PALLET_TIME],A.[STATUS],A.[OPERATOR],A.[OPR_LINE],A.[OPR_COMPUTER]
                                                            ,A.[OPR_LINE_PRE],A.[CHILD_LINE],A.[MATERIAL_CODE],A.[MATERIAL_LOT],A.[SUPPLIER_NAME],A.[SI_LOT]
                                                            ,A.[EFFICIENCY],A.[FACTORYROOM_KEY],A.[FACTORYROOM_NAME],A.[CREATE_OPERTION_NAME],A.[CREATOR]
                                                            ,A.[CREATE_TIME],A.[CREATE_TIMEZONE_KEY],A.[EDITOR],A.[EDIT_TIME],A.[EDIT_TIMEZONE],A.[SHIFT_NAME]
                                                            ,A.[DESCRIPTIONS],A.[LOT_SIDECODE],A.[LOT_CUSTOMERCODE],
                                                            B.ENTERPRISE_NAME,
                                                            B.ENTERPRISE_VERSION,
                                                            C.ROUTE_NAME ,
                                                            D.ROUTE_STEP_NAME,
                                                            D.ROUTE_OPERATION_VER_KEY,
                                                            e.COEF_PMAX,
                                                            f.GRADE_NAME
                                                    FROM POR_LOT A
                                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                                    LEFT JOIN WIP_IV_TEST e ON e.LOT_NUM=A.LOT_NUMBER AND e.VC_DEFAULT=1
                                                    LEFT JOIN V_ProductGrade f ON f.GRADE_CODE=A.PRO_LEVEL
                                                    WHERE A.STATUS < 2 {0}",
                                                    where);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetPackageLotInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取包装批次对应产品的平均功率控制数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含包装批次对应产品的平均功率控制数据的数据集对象。</returns>
        public DataSet GetPackageAvgPowerRangeData(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT e.AVGPOWER_MAX,e.AVGPOWER_MIN
                                                    FROM POR_LOT a
                                                    INNER JOIN POR_PRODUCT b ON b.PRODUCT_CODE=a.PRO_ID AND b.ISFLAG=1
                                                    LEFT JOIN WIP_IV_TEST c ON c.LOT_NUM=a.LOT_NUMBER AND c.VC_DEFAULT=1
                                                    LEFT JOIN BASE_TESTRULE d ON d.TESTRULE_CODE=b.PRO_TEST_RULE AND d.ISFLAG=1
                                                    LEFT JOIN BASE_TESTRULE_AVGPOWER e ON e.TESTRULE_KEY=d.TESTRULE_KEY  
                                                                                        AND e.PS_CODE=c.VC_TYPE 
                                                                                        AND e.PS_SEQ=c.I_IDE
                                                                                        AND e.ISFLAG=1
                                                    WHERE a.LOT_NUMBER='{0}' 
                                                    AND a.DELETED_TERM_FLAG<2",
                                                    lotNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetPackageAvgPowerRangeData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次号获取包装混包规则数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含混包规则数据的数据集对象。</returns>
        public DataSet GetPackageMixRule(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.LOT_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT a.PRO_ID AS PRODUCT_CODE,e.GRADE,
                                                               e.MIX_LEVEL AS MIN_LEVEL,
                                                               e.MIX_LEVEL_DETAIL AS MIN_LEVEL_DETAIL,
                                                               e.MIX_COLOR AS MIN_COLOR,e.PALLET_GROUP,a.PART_NUMBER
                                                        FROM POR_LOT a
                                                        LEFT JOIN POR_WO_PRD_LEVEL e ON e.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
                                                                                        AND e.PART_NUMBER=a.PART_NUMBER 
                                                                                        AND e.IS_USED='Y'
                                                        WHERE a.LOT_NUMBER='{0}'
                                                        AND A.STATUS < 2;
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        SELECT t.PRODUCT_CODE,t2.GRADE,t2.MIN_LEVEL,t2.MIN_LEVEL_DETAIL,t2.MIN_COLOR,t2.PALLET_GROUP,a.PART_NUMBER
                                                        FROM POR_LOT a
                                                        LEFT JOIN POR_PRODUCT t ON t.PRODUCT_CODE=a.PRO_ID
                                                        LEFT JOIN BASE_TESTRULE t1 ON t.PRO_TEST_RULE=t1.TESTRULE_CODE
                                                        LEFT JOIN BASE_TESTRULE_PROLEVEL t2 ON t1.TESTRULE_KEY=t2.TESTRULE_KEY
                                                        WHERE t.ISFLAG=1
                                                        AND A.STATUS < 2
                                                        AND t1.ISFLAG=1
                                                        AND t2.ISFLAG=1
                                                        AND a.LOT_NUMBER='{0}'
                                                        AND a.DELETED_TERM_FLAG<2;
                                                    END",
                                                    lotNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetPackageMixRule Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取批次功率分档和子分档数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次功率分档和子分档数据的数据集对象。</returns>
        public DataSet GetLotPowersetData(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.LOT_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT T.VC_TYPE,T.I_IDE,T.I_ID,T.I_PKID,T.LOT_NUM,T.COEF_PMAX,
                                                               c.POWERSET_KEY,c.PMAXSTAB,
                                                               c.PS_CODE,
                                                               c.PS_CODE+':'+c.PS_RULE AS PS_RULE1,
                                                               CAST(c.PS_SEQ AS VARCHAR)+':'+c.MODULE_NAME AS MODULE_NAME1,
                                                               (CASE WHEN C.SUB_PS_WAY = '电流' AND ISNULL(D.PS_SUB_CODE,0)>0 THEN 
                                                                 CONVERT(varchar,CONVERT(int,C.PMAXSTAB)) + 'W-' + CONVERT(varchar,SUBSTRING(D.POWERLEVEL,CHARINDEX('-',D.POWERLEVEL)+1,2)) 
                                                                 ELSE d.POWERLEVEL END) AS POWERLEVEL
                                                        FROM WIP_IV_TEST t
                                                        INNER JOIN POR_LOT a ON t.LOT_NUM=a.LOT_NUMBER
                                                        LEFT JOIN POR_WO_PRD_PS c ON c.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
                                                                                    AND c.PART_NUMBER=a.PART_NUMBER
                                                                                    AND c.PS_CODE=t.VC_TYPE 
                                                                                    AND C.PS_SEQ=t.I_IDE 
                                                                                    AND c.IS_USED='Y'
                                                        LEFT JOIN POR_WO_PRD_PS_SUB d ON d.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
                                                                                    AND d.PART_NUMBER=a.PART_NUMBER
                                                                                    AND d.POWERSET_KEY=c.POWERSET_KEY
                                                                                    AND d.PS_SUB_CODE=t.I_PKID
                                                                                    AND d.IS_USED='Y'
                                                        WHERE t.LOT_NUM='{0}' 
                                                        AND A.STATUS < 2
                                                        AND ISNULL(t.COEF_PMAX,0)<>0 
                                                        AND t.VC_DEFAULT='1'                                                        
                                                        ORDER BY t.DT_CREATE DESC;
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        SELECT T.VC_TYPE,T.I_IDE,T.I_ID,T.I_PKID,T.LOT_NUM,T.COEF_PMAX,
                                                               a.POWERSET_KEY,a.PMAXSTAB,
                                                               a.PS_CODE,
                                                               a.PS_CODE+':'+a.PS_RULE AS PS_RULE1,
                                                               CAST(a.PS_SEQ AS VARCHAR)+':'+a.MODULE_NAME AS MODULE_NAME1,
                                                               b.POWERLEVEL
                                                        FROM WIP_IV_TEST t
                                                        LEFT JOIN BASE_POWERSET a ON a.PS_CODE=t.VC_TYPE AND a.PS_SEQ=t.I_IDE AND a.ISFLAG=1
                                                        LEFT JOIN BASE_POWERSET_DETAIL b ON b.POWERSET_KEY=a.POWERSET_KEY AND b.PS_DTL_SUBCODE=t.I_PKID
                                                        WHERE t.LOT_NUM='{0}' 
                                                        AND ISNULL(t.COEF_PMAX,0)<>0 
                                                        AND t.VC_DEFAULT='1'
                                                        ORDER BY t.DT_CREATE DESC;
                                                    END",
                                                    lotNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetLotPowersetData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据批次号获取满包数量。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>满包数量。</returns>
        public int GetPackageFullQty(string lotNumber)
        {
            try
            {
                string sqlCommand = string.Format(@"IF EXISTS(SELECT 1 FROM POR_LOT a
                                                              INNER JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY AND IS_USED='Y'
                                                              WHERE a.LOT_NUMBER='{0}')
                                                    BEGIN
                                                        SELECT  TOP 1 e.FULL_PALLET_QTY
                                                        FROM POR_LOT a
                                                        LEFT JOIN POR_WO_PRD e ON e.WORK_ORDER_KEY=a.WORK_ORDER_KEY 
                                                                                        AND e.PART_NUMBER=a.PART_NUMBER 
                                                                                        AND e.IS_USED='Y'
                                                        WHERE a.LOT_NUMBER='{0}';
                                                    END
                                                    ELSE
                                                    BEGIN
                                                        SELECT TOP 1 t2.FULL_PALLET_QTY
                                                        FROM POR_LOT a
                                                        INNER JOIN POR_PRODUCT t1 ON t1.PRODUCT_CODE=a.PRO_ID
                                                        INNER JOIN BASE_TESTRULE t2 ON t1.PRO_TEST_RULE=t2.TESTRULE_CODE
                                                        WHERE t1.ISFLAG=1 
                                                        AND t2.ISFLAG=1 
                                                        AND a.LOT_NUMBER='{0}';
                                                    END",
                                                    lotNumber.PreventSQLInjection());
                object fullQty = db.ExecuteScalar(CommandType.Text, sqlCommand);
                if (fullQty != DBNull.Value || fullQty != null)
                {
                    return Convert.ToInt32(fullQty);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("LotOperationEngine.GetPackageFullQty Error: " + ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// 根据批次号获取包装数据。
        /// </summary>
        /// <returns>包含包装及其明细数据的数据集对象。</returns>
        public DataSet GetPackageDataByLotNo(string lotNo)
        {
            return GetPackageData(string.Empty, lotNo);
        }
        /// <summary>
        /// 根据托盘号获取包装数据。
        /// </summary>
        /// <returns>包含包装及其明细数据的数据集对象。</returns>
        public DataSet GetPackageData(string packageNo)
        {
            return GetPackageData(packageNo, string.Empty);
        }
        /// <summary>
        /// 根据托盘号和批次号获取包装数据。
        /// </summary>
        /// <remarks>
        /// 只输入托盘号获取所有包装明细数据。
        /// 只输入批次号，根据批次号获取包装明细数据，从而获取到包装数据。
        /// 同时输入托盘号和批次号，根据批次号获取包装明细数据，再根据包装号获取对应的包装数据。
        /// </remarks>
        /// <returns>包含包装及其明细数据的数据集对象。</returns>
        public DataSet GetPackageData(string packageNo, string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT d.ITEM_NO SEQ,
                                           t.LOT_KEY,
                                           t.LOT_NUMBER,
                                           t.WORK_ORDER_NO,
                                           t.PRO_ID,
                                           t.PALLET_NO,
                                           t.PALLET_TIME,
                                           t.PART_NUMBER,
                                           t.LOT_SIDECODE,
                                           t.LOT_CUSTOMERCODE,
                                           t.COLOR,
                                           t.EDIT_TIME,
                                           t.PRO_LEVEL,
                                           b.GRADE_NAME,
                                           c.EQUIPMENT_NAME,c.SHIFT,c.CHECK_TIME,c.CHECKER,
                                           a.DEVICENUM,a.VC_DEFAULT,a.T_DATE,a.VC_TYPE,a.I_IDE,a.VC_MODNAME,
                                           a.PM,a.FF,a.IPM,a.ISC,a.VPM,a.VOC,
                                           a.COEF_PMAX,a.COEF_FF,a.COEF_IMAX,a.COEF_ISC,a.COEF_VMAX,a.COEF_VOC,
                                           d.FULL_QTY,d.POWER_LEVEL,d.PS_CODE,d.PS_DTL_CODE,d.PS_SEQ,d.AVG_POWER_RANGE
                                    FROM POR_LOT t 
                                    INNER JOIN WIP_CONSIGNMENT c ON c.VIRTUAL_PALLET_NO=t.PALLET_NO AND c.ISFLAG=1
                                    INNER JOIN WIP_CONSIGNMENT_DETAIL d ON d.CONSIGNMENT_KEY=c.CONSIGNMENT_KEY AND d.LOT_NUMBER=t.LOT_NUMBER
                                    LEFT JOIN WIP_IV_TEST a ON a.LOT_NUM=t.LOT_NUMBER AND a.VC_DEFAULT=1
                                    LEFT JOIN V_ProductGrade b ON b.GRADE_CODE=t.PRO_LEVEL
                                    WHERE t.DELETED_TERM_FLAG<2
                                    AND  t.STATUS < 2");

                if (!string.IsNullOrEmpty(packageNo))
                {
                    sbSql.AppendFormat("AND c.VIRTUAL_PALLET_NO='{0}'", packageNo.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(lotNo))
                {
                    sbSql.AppendFormat("AND t.LOT_NUMBER='{0}'", lotNo.PreventSQLInjection());
                }

                DataTable dtPackageDetail = db.ExecuteDataSet(CommandType.Text, sbSql.ToString()).Tables[0];
                dtPackageDetail.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;

                if (string.IsNullOrEmpty(packageNo)
                    && dtPackageDetail.Rows.Count > 0)
                {
                    packageNo = Convert.ToString(dtPackageDetail.Rows[0]["PALLET_NO"]);
                }

                DataTable dtPackage = GetPackageMasterData(null, packageNo);

                dsReturn.Merge(dtPackage, true, MissingSchemaAction.Add);
                dsReturn.Merge(dtPackageDetail, true, MissingSchemaAction.Add);

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetPackageData Error: " + ex.Message);
            }

            return dsReturn;
        }

        private DataTable GetPackageMasterData(DbTransaction dbTrans, string packageNo)
        {
            string sqlCommand = string.Format(@"SELECT [CONSIGNMENT_KEY],[VIRTUAL_PALLET_NO],[PALLET_NO],[WORKNUMBER],[CS_DATA_GROUP]
                                                        ,[SAP_NO],[POWER_LEVEL],[GRADE],[SHIFT],[PS_CODE],[PS_DTL_SUBCODE],[LAST_PALLET]
                                                        ,[CREATER],[CREATE_TIME],[EDITOR],[EDIT_TIME],[ISFLAG],[ROOM_KEY],[CUSTOMER_NO],[LOT_NUMBER_QTY],FULL_QTY
                                                        ,[TOTLE_POWER],[AVG_POWER],[PRO_ID],[PALLET_NO_NEW],[PALLET_TYPE],[CODE_TYPE],[LINE_NAME]
                                                        ,[LINE_KEY],[EQUIPMENT_KEY],[EQUIPMENT_NAME],[AVG_POWER_RANGE],[LOT_COLOR],[PS_SEQ],[CHECKER]
                                                        ,[CHECK_TIME],[TO_WH],[TO_WH_TIME],[OUT_WH],[OUT_WH_TIME],[MEMO1]  
                                                FROM WIP_CONSIGNMENT t 
                                                WHERE t.ISFLAG=1
                                                AND t.VIRTUAL_PALLET_NO='{0}'",
                                               packageNo.PreventSQLInjection());
            DataTable dtPackage = null;
            if (dbTrans != null)
            {
                dtPackage = db.ExecuteDataSet(dbTrans, CommandType.Text, sqlCommand).Tables[0];

            }
            else
            {
                dtPackage = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            }
            dtPackage.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
            return dtPackage;
        }


        /// <summary>
        /// 批次拆包作业。
        /// </summary>
        /// <param name="dsParams">包含包装数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet LotUnpack(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)            //存放批次数据
                )
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                return dsReturn;
            }

            try
            {
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                DataTable dtWipConsignment = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtLots = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                DataTable dtPackageData = null;
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        WIP_CONSIGNMENT_FIELDS consigmentFields = new WIP_CONSIGNMENT_FIELDS();


                        foreach (DataRow dr in dtWipConsignment.Rows)
                        {
                            //int csDataGroup = Convert.ToInt32(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                            string palletNo = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                            string palletKey = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                            //托盘主键为空。给出提示。
                            if (string.IsNullOrEmpty(palletKey))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘记录{0}不存在，请检查。", palletNo));
                                return dsReturn;
                            }

                            //检查托盘记录是否过期。防止重复修改。
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, palletKey);
                            List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                            listCondition.Add(kvp);
                            string opEditTime = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME]);
                            //如果托盘记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                            if (UtilHelper.CheckRecordExpired(db, WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘号{0}信息已过期，请确认。", palletNo));
                                return dsReturn;
                            }
                            //重新获取托盘状态，检查托盘状态。
                            string sql = string.Format(@"SELECT TOP 1 CS_DATA_GROUP 
                                                        FROM WIP_CONSIGNMENT 
                                                        WHERE CONSIGNMENT_KEY='{0}'",
                                                        palletKey.PreventSQLInjection());
                            string sCsDataGroup = Convert.ToString(db.ExecuteScalar(dbTran, CommandType.Text, sql));
                            int csDataGroup = -1;
                            if (!int.TryParse(sCsDataGroup, out csDataGroup))
                            {
                                csDataGroup = -1;
                            }
                            //检查包装状态。
                            if (csDataGroup == 1)
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经等待【入库检验】，请确认", palletNo));
                                return dsReturn;
                            }
                            else if (csDataGroup == 2)
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经等待【入库】，请确认", palletNo));
                                return dsReturn;
                            }
                            else if (csDataGroup == 3)
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经【入库】，请确认", palletNo));
                                return dsReturn;
                            }
                            else if (csDataGroup == 4)
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经【出货】，请确认", palletNo));
                                return dsReturn;
                            }
                            else if (csDataGroup < 0)
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘记录{0}不存在，请检查。", palletNo));
                                return dsReturn;
                            }
                            //循环批次数据。
                            var lnq = from item in dtLots.AsEnumerable()
                                      where Convert.ToString(item[POR_LOT_FIELDS.FIELD_PALLET_NO]) == palletNo
                                      orderby Convert.ToInt32(item[WIP_CONSIGNMENT_FIELDS.FIELDS_SEQ])
                                      select item;

                            foreach (DataRow drLot in lnq)
                            {
                                string lotKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                                string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                                string lotPalletNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                                string lotPalletTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_TIME]);

                                //检查记录是否过期。防止重复修改。
                                KeyValuePair<string, string> kvpLot = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                                List<KeyValuePair<string, string>> lstLotCondition = new List<KeyValuePair<string, string>>();
                                lstLotCondition.Add(kvpLot);
                                string opLotEditTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, lstLotCondition, opLotEditTime))
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("组件{0}信息已过期，请确认。", lotNumber));
                                    return dsReturn;
                                }
                                //获取批次数据
                                string sqlQueryLot = string.Format(@"SELECT A.*,
                                                                        B.ENTERPRISE_NAME,
                                                                        B.ENTERPRISE_VERSION,
                                                                        C.ROUTE_NAME ,
                                                                        D.ROUTE_STEP_NAME
                                                                    FROM POR_LOT A
                                                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                                                    WHERE A.STATUS < 2
                                                                    AND A.LOT_NUMBER='{0}'",
                                                                    lotNumber.PreventSQLInjection());
                                DataTable dtTable = db.ExecuteDataSet(dbTran, CommandType.Text, sqlQueryLot).Tables[0];
                                DataRow drLotInfo = dtTable.Rows[0];
                                LotUnpack(dbTran, drLotInfo, htParams);
                            }
                            sql = string.Format(@"UPDATE WIP_CONSIGNMENT 
                                                SET ISFLAG=0,EDITOR='{0}',EDIT_TIME=GETDATE(),MEMO1='拆包'
                                                WHERE CONSIGNMENT_KEY='{1}'",
                                                editor,
                                                palletKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //新增包装记录。
                            string newPalletKey = UtilHelper.GenerateNewKey(0);
                            dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] = newPalletKey;
                            dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME] = htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME];
                            sql = DatabaseTable.BuildInsertSqlStatement(consigmentFields, dr, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //新增包装明细数据
                            sql = string.Format(@"INSERT INTO WIP_CONSIGNMENT_DETAIL
                                                        (CONSIGNMENT_KEY,ITEM_NO,LOT_NUMBER,WORK_NUMBER,PART_NUMBER,PRO_ID,PRO_LEVEL,
                                                        COLOR,POWER_LEVEL,PS_CODE,PS_DTL_CODE,FULL_QTY,PS_SEQ,AVG_POWER_RANGE,CREATOR,CREATE_TIME)
                                                   SELECT '{0}',
                                                           ROW_NUMBER() OVER(PARTITION BY a.CONSIGNMENT_KEY ORDER BY b.ITEM_NO) ITEM_NO,
                                                           b.LOT_NUMBER,
                                                           b.WORK_NUMBER,
                                                           b.PART_NUMBER,
                                                           b.PRO_ID,
                                                           b.PRO_LEVEL,
                                                           b.COLOR,
                                                           b.POWER_LEVEL,
                                                           b.PS_CODE,
                                                           b.PS_DTL_CODE,
                                                           b.FULL_QTY,
                                                           b.PS_SEQ,
                                                           b.AVG_POWER_RANGE,
                                                           '{2}',
                                                           GETDATE()
                                                    FROM WIP_CONSIGNMENT a
                                                    INNER JOIN WIP_CONSIGNMENT_DETAIL b ON b.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY
                                                    INNER JOIN POR_LOT c ON c.PALLET_NO=a.VIRTUAL_PALLET_NO AND c.LOT_NUMBER=b.LOT_NUMBER 
                                                    WHERE a.CONSIGNMENT_KEY='{1}'",
                                                    newPalletKey,
                                                    palletKey.PreventSQLInjection(),
                                                    editor.PreventSQLInjection());
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //更新现有的包装数据，为其第一条数据的基本数据
                            sql = string.Format(@"UPDATE a
                                                SET    a.WORKNUMBER=b.WORK_NUMBER,
                                                       a.SAP_NO=b.PART_NUMBER,
                                                       a.POWER_LEVEL=b.POWER_LEVEL,
                                                       a.GRADE=b.PRO_LEVEL,
                                                       a.PS_CODE=b.PS_CODE,
                                                       a.PS_DTL_SUBCODE=b.PS_DTL_CODE,
                                                       a.LOT_NUMBER_QTY=(SELECT COUNT(1) FROM WIP_CONSIGNMENT_DETAIL aa WHERE aa.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY),
                                                       a.FULL_QTY=b.FULL_QTY,
                                                       a.TOTLE_POWER=(SELECT ISNULL(SUM(bb.COEF_PMAX),0)
                                                                      FROM WIP_CONSIGNMENT_DETAIL aa
                                                                      INNER JOIN WIP_IV_TEST bb ON bb.LOT_NUM=aa.LOT_NUMBER AND bb.VC_DEFAULT='1'
                                                                      WHERE aa.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY),
                                                       a.PRO_ID=b.PRO_ID,
                                                       a.LOT_COLOR=b.COLOR,
                                                       a.PS_SEQ=b.PS_SEQ,
                                                       a.AVG_POWER_RANGE=b.AVG_POWER_RANGE
                                                FROM WIP_CONSIGNMENT a
                                                LEFT JOIN WIP_CONSIGNMENT_DETAIL b ON b.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY AND b.ITEM_NO=1
                                                WHERE a.CONSIGNMENT_KEY='{0}'",
                                                newPalletKey);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            sql = string.Format(@"UPDATE a
                                                SET    a.AVG_POWER=CASE WHEN a.LOT_NUMBER_QTY=0 THEN 0 ELSE a.TOTLE_POWER/a.LOT_NUMBER_QTY END,
                                                       a.ISFLAG=CASE WHEN a.LOT_NUMBER_QTY>0 THEN 1 ELSE 0 END,
                                                       a.EDIT_TIME=GETDATE()
                                                FROM WIP_CONSIGNMENT a
                                                WHERE a.CONSIGNMENT_KEY='{0}'",
                                                newPalletKey);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            DataTable dtTempPackageData = GetPackageMasterData(dbTran, palletNo);
                            if (dtPackageData == null)
                            {
                                dtPackageData = dtTempPackageData.Clone();
                                dtPackageData.PrimaryKey = new DataColumn[] { dtPackageData.Columns[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] };
                            }
                            dtPackageData.Merge(dtTempPackageData);
                        }
                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
                dsReturn.Merge(dtPackageData, true, MissingSchemaAction.Add);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.LotUnpack Error: " + ex.Message);
            }
            return dsReturn;
        }

        private void LotUnpack(DbTransaction dbTran, DataRow drLotInfo, Hashtable htParams)
        {
            string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            string lotNumber = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
            string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);       //编辑时区
            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);

            //更新批次包装数据。
            string sql = string.Format(@"UPDATE POR_LOT 
                                         SET PALLET_NO=NULL,PALLET_TIME=NULL,EDITOR='{1}',EDIT_TIME=GETDATE()
                                         WHERE LOT_NUMBER='{0}'",
                                         lotNumber.PreventSQLInjection(),
                                         editor.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //向WIP_TRANSACTION表插入批次拆包的操作记录。
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_UNPACK);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, 0);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
        }
        /// <summary>
        /// 包装作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotPackage(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            //参数数据。
            if (dsParams == null
                || !dsParams.Tables.Contains(TRANS_TABLES.TABLE_PARAM)                              //存放附加参数数据
                || !dsParams.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME)            //存放托盘数据
                || !dsParams.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME)            //存放批次数据
                )
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入参数不正确，请检查。");
                return dsReturn;
            }

            try
            {
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_PARAM];
                DataTable dtWipConsignment = dsParams.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtLots = dsParams.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
                string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);       //编辑时区
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        WIP_CONSIGNMENT_FIELDS consigmentFields = new WIP_CONSIGNMENT_FIELDS();

                        foreach (DataRow dr in dtWipConsignment.Rows)
                        {

                            int csDataGroup = Convert.ToInt32(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                            bool bTrackOut = csDataGroup == 1;
                            string palletNo = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                            string palletKey = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                            //判断拖号是否存在。
                            string sql = string.Format("SELECT TOP 1 CS_DATA_GROUP FROM WIP_CONSIGNMENT WHERE VIRTUAL_PALLET_NO='{0}' AND ISFLAG=1",
                                                        palletNo.PreventSQLInjection());
                            string sCsDataGroup = Convert.ToString(db.ExecuteScalar(dbTran, CommandType.Text, sql));
                            //托盘号已存在，托盘主键为空。给出提示。
                            if (!string.IsNullOrEmpty(sCsDataGroup) && string.IsNullOrEmpty(palletKey))
                            {
                                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘号{0}已存在，请检查。", palletNo));
                                return dsReturn;
                            }

                            //有包装记录，根据主键更新包装数据。
                            if (!string.IsNullOrEmpty(sCsDataGroup) && !string.IsNullOrEmpty(palletKey))
                            {
                                //检查记录是否过期。防止重复修改。
                                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, palletKey);
                                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                                listCondition.Add(kvp);
                                string opEditTime = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME]);
                                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                                if (UtilHelper.CheckRecordExpired(db, WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘号{0}信息已过期，请确认。", palletNo));
                                    return dsReturn;
                                }
                                //重新获取托盘状态，检查托盘状态。
                                if (!int.TryParse(sCsDataGroup, out csDataGroup))
                                {
                                    csDataGroup = -1;
                                }
                                //检查包装状态。
                                if (csDataGroup == 1)
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经等待【入库检验】，请确认", palletNo));
                                    return dsReturn;
                                }
                                else if (csDataGroup == 2)
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经等待【入库】，请确认", palletNo));
                                    return dsReturn;
                                }
                                else if (csDataGroup == 3)
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经【入库】，请确认", palletNo));
                                    return dsReturn;
                                }
                                else if (csDataGroup == 4)
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托号【{0}】已经【出货】，请确认", palletNo));
                                    return dsReturn;
                                }
                                else if (csDataGroup < 0)
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("托盘记录{0}不存在，请检查。", palletNo));
                                    return dsReturn;
                                }

                                sql = string.Format(@"UPDATE WIP_CONSIGNMENT 
                                                    SET ISFLAG=0,EDITOR='{0}',EDIT_TIME=GETDATE(),MEMO1='重新包装'
                                                    WHERE CONSIGNMENT_KEY='{1}'",
                                                    editor,
                                                    palletKey.PreventSQLInjection());
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            }
                            //新增包装记录。
                            string newPalletKey = UtilHelper.GenerateNewKey(0);
                            dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY] = newPalletKey;
                            dr[WIP_CONSIGNMENT_FIELDS.FIELDS_EDIT_TIME] = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME];
                            sql = DatabaseTable.BuildInsertSqlStatement(consigmentFields, dr, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            var lnq = from item in dtLots.AsEnumerable()
                                      where Convert.ToString(item[POR_LOT_FIELDS.FIELD_PALLET_NO]) == palletNo
                                      orderby Convert.ToInt32(item[WIP_CONSIGNMENT_FIELDS.FIELDS_SEQ])
                                      select item;

                            Hashtable htStepTransaction = null;//存储下一工步数据。

                            foreach (DataRow drLot in lnq)
                            {
                                string lotKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                                string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                                string lotPalletNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                                string lotPalletTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_TIME]);

                                //检查记录是否过期。防止重复修改。
                                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                                List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                                listCondition.Add(kvp);
                                string opEditTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                                //如果记录过期，当前编辑时间<数据库中的记录编辑时间。结束方法执行。
                                if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, opEditTime))
                                {
                                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Format("组件{0}信息已过期，请确认。", lotNumber));
                                    return dsReturn;
                                }

                                #region 批次进站
                                //获取批次数据
                                string sqlQueryLot = string.Format(@"SELECT A.*,
                                                                        B.ENTERPRISE_NAME,
                                                                        B.ENTERPRISE_VERSION,
                                                                        C.ROUTE_NAME ,
                                                                        D.ROUTE_STEP_NAME
                                                                    FROM POR_LOT A
                                                                    LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                                                                    LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                                                                    WHERE A.STATUS < 2
                                                                    AND A.LOT_NUMBER='{0}'",
                                                                    lotNumber.PreventSQLInjection());
                                DataTable dtTable = db.ExecuteDataSet(dbTran, CommandType.Text, sqlQueryLot).Tables[0];
                                DataRow drLotInfo = dtTable.Rows[0];
                                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                                //如果批次没有进站，先进站。
                                if (stateFlag == 0)
                                {
                                    PackageLotTrackIn(dbTran, drLotInfo, htParams);
                                    stateFlag = 9;
                                }
                                #endregion
                                //包装 更新批次数据 新增包装明细数据。
                                LotPackage(dbTran, drLot, drLotInfo, htParams, newPalletKey);
                                //需要包装过站，则过站。
                                if (bTrackOut)
                                {
                                    #region 批次过站。
                                    //重新获取批次数据。
                                    if (stateFlag > 0 && stateFlag <= 9)
                                    {
                                        string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                                        string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                                        string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                                        string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                                        string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                                        string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                                        //如果下一个工步主键为空，则获取下一个工步数据。每包抓第一块组件的下一工步为标准工步。从而统一包的工艺流程。
                                        if (htStepTransaction == null)
                                        {
                                            IEnterpriseEngine enterpriseEngine = RemotingServer.ServerObjFactory.Get<IEnterpriseEngine>();
                                            DataSet dsRouteNextStep = enterpriseEngine.GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                                            string toEnterpriseKey = enterpriseKey;
                                            string toRouteKey = routeKey;
                                            string toStepKey = stepKey;
                                            string toEnterpriseName = enterpriseName;
                                            string toRouteName = routeName;
                                            string toStepName = stepName;
                                            if (null != dsRouteNextStep
                                                && dsRouteNextStep.Tables.Count > 0
                                                && dsRouteNextStep.Tables[0].Rows.Count > 0)
                                            {
                                                DataRow drRouteNextStep = dsRouteNextStep.Tables[0].Rows[0];
                                                toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                                                toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                                                toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                                                toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                                                toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                                                toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                                            }
                                            htStepTransaction = new Hashtable();
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, timeZone);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                                            htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                                        }
                                        else
                                        {
                                            htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                                            htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                                            htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = stepKey;
                                        }

                                        DataTable dtStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                                        dtStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                                        string nextStepKey = Convert.ToString(htStepTransaction[WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY]);
                                        bool isFinish = stepKey == nextStepKey; //最后一个工步，则结束批次。
                                        PackageLotTrackOut(dbTran, drLotInfo, dtStepTransaction, htParams, isFinish);
                                    }
                                    #endregion
                                }
                            }

                            //更新现有的包装数据，为其第一条数据的基本数据
                            sql = string.Format(@"UPDATE a
                                                SET    a.WORKNUMBER=b.WORK_NUMBER,
                                                       a.SAP_NO=b.PART_NUMBER,
                                                       a.POWER_LEVEL=b.POWER_LEVEL,
                                                       a.GRADE=b.PRO_LEVEL,
                                                       a.PS_CODE=b.PS_CODE,
                                                       a.PS_DTL_SUBCODE=b.PS_DTL_CODE,
                                                       a.LOT_NUMBER_QTY=(SELECT COUNT(1) FROM WIP_CONSIGNMENT_DETAIL aa WHERE aa.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY),
                                                       a.FULL_QTY=b.FULL_QTY,
                                                       a.TOTLE_POWER=(SELECT ISNULL(SUM(bb.COEF_PMAX),0)
                                                                      FROM WIP_CONSIGNMENT_DETAIL aa
                                                                      INNER JOIN WIP_IV_TEST bb ON bb.LOT_NUM=aa.LOT_NUMBER AND bb.VC_DEFAULT='1'
                                                                      WHERE aa.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY),
                                                       a.PRO_ID=b.PRO_ID,
                                                       a.LOT_COLOR=b.COLOR,
                                                       a.PS_SEQ=b.PS_SEQ,
                                                       a.AVG_POWER_RANGE=b.AVG_POWER_RANGE
                                                FROM WIP_CONSIGNMENT a
                                                LEFT JOIN WIP_CONSIGNMENT_DETAIL b ON b.CONSIGNMENT_KEY=a.CONSIGNMENT_KEY AND b.ITEM_NO=1
                                                WHERE a.CONSIGNMENT_KEY='{0}'",
                                                newPalletKey);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                            sql = string.Format(@"UPDATE a
                                                SET    a.AVG_POWER=CASE WHEN a.LOT_NUMBER_QTY=0 THEN 0 ELSE a.TOTLE_POWER/a.LOT_NUMBER_QTY END,
                                                       a.ISFLAG=CASE WHEN a.LOT_NUMBER_QTY>0 THEN 1 ELSE 0 END
                                                FROM WIP_CONSIGNMENT a
                                                WHERE a.CONSIGNMENT_KEY='{0}'",
                                                                           newPalletKey);
                            this.db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.LotPackage Error: " + ex.Message);
            }
            return dsReturn;
        }

        private void LotPackage(DbTransaction dbTran, DataRow drLot, DataRow drLotInfo, Hashtable htParams, string palletKey)
        {
            string lotKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            string lotNumber = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            string lotPalletNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            string lotPalletTime = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PALLET_TIME]);
            string editor = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);         //编辑人
            string timeZone = Convert.ToString(htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);       //编辑时区
            string transactionKey = UtilHelper.GenerateNewKey(0);
            AddWIPLot(dbTran, transactionKey, lotKey);
            //新增包装明细记录
            string sql = string.Format(@"INSERT INTO WIP_CONSIGNMENT_DETAIL
                                        (CONSIGNMENT_KEY,ITEM_NO,LOT_NUMBER,WORK_NUMBER,PART_NUMBER,PRO_ID,PRO_LEVEL,
                                        COLOR,POWER_LEVEL,PS_CODE,PS_DTL_CODE,FULL_QTY,PS_SEQ,AVG_POWER_RANGE,
                                        CREATOR,CREATE_TIME)
                                        VALUES('{0}',{1},'{2}','{3}','{4}','{5}','{6}',
                                        '{7}','{8}','{9}','{10}','{11}','{12}','{13}',
                                       '{14}', '{15}')",
                                        palletKey,
                                        drLot["SEQ"],
                                        drLot[POR_LOT_FIELDS.FIELD_LOT_NUMBER],
                                        drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO],
                                        drLot[POR_LOT_FIELDS.FIELD_PART_NUMBER],
                                        drLot[POR_LOT_FIELDS.FIELD_PRO_ID],
                                        drLot[POR_LOT_FIELDS.FIELD_PRO_LEVEL],
                                        drLot[POR_LOT_FIELDS.FIELD_COLOR],
                                        drLot["POWER_LEVEL"],
                                        drLot["PS_CODE"],
                                        drLot["PS_DTL_CODE"],
                                        drLot["FULL_QTY"],
                                        drLot["PS_SEQ"],
                                        drLot["AVG_POWER_RANGE"],
                                        editor.PreventSQLInjection(),
                                        lotPalletTime);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //更新批次包装数据。
            sql = string.Format(@"UPDATE POR_LOT 
                                 SET PALLET_NO='{1}',PALLET_TIME='{2}',EDITOR='{3}',EDIT_TIME=GETDATE()
                                 WHERE LOT_NUMBER='{0}'",
                                 lotNumber.PreventSQLInjection(),
                                 lotPalletNo.PreventSQLInjection(),
                                 lotPalletTime.PreventSQLInjection(),
                                 editor.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
            //向WIP_TRANSACTION表插入批次包装的操作记录。
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, transactionKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PACKAGE);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, 0);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            if (htParams.Contains(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
            }
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            WIP_TRANSACTION_FIELDS wipFields = new WIP_TRANSACTION_FIELDS();
            sql = DatabaseTable.BuildInsertSqlStatement(wipFields, htTransaction, null);
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
        }
        /// <summary>
        /// 包装批次进站。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="drLotInfo">批次信息。</param>
        /// <param name="htParams">附近参数数据。</param>
        private void PackageLotTrackIn(DbTransaction dbTran, DataRow drLotInfo, Hashtable htParams)
        {
            string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, 0);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            if (htParams.Contains(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
            }
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            DataTable dtTrackInTransaction = CommonUtils.ParseToDataTable(htTransaction);
            dtTrackInTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

            Hashtable htTrackInParams = new Hashtable();
            htTrackInParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
                                htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            DataTable dtTrackInParams = CommonUtils.ParseToDataTable(htTrackInParams);
            dtTrackInParams.TableName = TRANS_TABLES.TABLE_PARAM;

            DataSet dsTrackInParams = new DataSet();
            dsTrackInParams.Tables.Add(dtTrackInTransaction);
            dsTrackInParams.Tables.Add(dtTrackInParams);

            LotTrackIn(dbTran, dsTrackInParams);
            //更新批次状态,9-等待出站
            string sql = string.Format(@"UPDATE POR_LOT SET STATE_FLAG=9
                                        WHERE LOT_KEY='{0}'",
                                        lotKey.PreventSQLInjection());
            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
        }
        /// <summary>
        /// 包装批次出站。
        /// </summary>
        /// <param name="dbTran">数据库事务对象。</param>
        /// <param name="drLotInfo">批次信息。</param>
        /// <param name="htParams">附近参数数据。</param>
        private void PackageLotTrackOut(DbTransaction dbTran,
                                        DataRow drLotInfo,
                                        DataTable dtTrackOutStepTransaction,
                                        Hashtable htParams, bool isFinish)
        {
            Hashtable htTransaction = new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_EDITOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, htParams[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, 0);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, htParams[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, 9);
            if (htParams.Contains(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT))
            {
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, htParams[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT]);
            }
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            DataTable dtTrackOutTransaction = CommonUtils.ParseToDataTable(htTransaction);
            dtTrackOutTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

            Hashtable htTrackOutParams = new Hashtable();
            htTrackOutParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY,
                                htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            DataTable dtTrackOutParams = CommonUtils.ParseToDataTable(htTrackOutParams);
            dtTrackOutParams.TableName = TRANS_TABLES.TABLE_PARAM;

            dtTrackOutStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
            DataSet dsTrackOutParams = new DataSet();
            dsTrackOutParams.Tables.Add(dtTrackOutTransaction);
            dsTrackOutParams.Tables.Add(dtTrackOutParams);
            dsTrackOutParams.Tables.Add(dtTrackOutStepTransaction);

            LotTrackOut(dbTran, dsTrackOutParams, isFinish);
        }
        /// <summary>
        /// 自动生成托号 add by chao.Pang 
        /// </summary>
        /// <param name="_lotNumber">批次号</param>
        /// <returns>包含执行结果的数据集对象。新生成的托号</returns>
        public DataSet NewPalletNo(string _lotNumber)
        {

            DataSet dsReturn = new DataSet();
            DataSet dsItemNo = new DataSet();
            DataSet dsFullPallet = new DataSet();
            DataSet dsWorkOrder = new DataSet();
            DataSet dsMaxArkNo = new DataSet();
            DataSet dsLiuShui = new DataSet();
            DataSet dsFullArk = new DataSet();
            string _palletNo = string.Empty;
            int item_No = 0;
            //001-210000189-001 产品编码号 + 柜号 + - + 工单号 + - + 流水码
            try
            {
                //查询批次号对应的物料的序号  该序号指明该组件料号是否为主料号还是副料号 即生成托号的首位
                string _itemNo = string.Format(@"   SELECT A.MAIN_PART_NUMBER,A.PART_NUMBER,A.ITEM_NO
                                                    FROM
                                                    dbo.POR_PART_BYPRODUCT A
                                                    LEFT JOIN dbo.POR_WORK_ORDER B ON 
                                                              B.PART_NUMBER = A.MAIN_PART_NUMBER
                                                    LEFT JOIN dbo.POR_LOT C ON 
                                                              C.WORK_ORDER_KEY = B.WORK_ORDER_KEY 
                                                              AND C.PART_NUMBER = A.PART_NUMBER
                                                    WHERE C.LOT_NUMBER = '{0}' AND A.IS_USED = 'Y'", _lotNumber);
                dsItemNo = db.ExecuteDataSet(CommandType.Text, _itemNo);

                if (dsItemNo.Tables[0].Rows.Count > 0)
                {
                    //产品料号编码
                    item_No = Convert.ToInt32(dsItemNo.Tables[0].Rows[0]["ITEM_NO"].ToString()) - 1;
                }

                //获取工单号
                string _workOrder = string.Format(@"SELECT WORK_ORDER_NO,PART_NUMBER FROM dbo.POR_LOT WHERE LOT_NUMBER = '{0}'", _lotNumber);
                dsWorkOrder = db.ExecuteDataSet(CommandType.Text, _workOrder);

                if (dsWorkOrder.Tables[0].Rows.Count > 0)
                {
                    //工单号
                    string work_number = dsWorkOrder.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                    string part_number = dsWorkOrder.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                    //获取最大柜号和托号
                    string _maxArkNo = string.Format(@"SELECT MAX(substring(A.VIRTUAL_PALLET_NO,2,CHARINDEX('-',A.VIRTUAL_PALLET_NO)-2)) AS MAX_ARK_NUMBER,
                                                              MAX(A.VIRTUAL_PALLET_NO) AS MAX_VIRTUAL_PALLET_NO
                                                       FROM   dbo.WIP_CONSIGNMENT A
                                                       WHERE  A.VIRTUAL_PALLET_NO LIKE '{0}'+ '[0-9][0-9]-%' 
                                                              AND A.WORKNUMBER= '{1}' 
                                                              AND A.SAP_NO = '{2}'
                                                              AND ISFLAG = 1
                                                                    ", item_No, work_number, part_number);
                    dsMaxArkNo = db.ExecuteDataSet(CommandType.Text, _maxArkNo);
                    if (!string.IsNullOrEmpty(dsMaxArkNo.Tables[0].Rows[0]["MAX_VIRTUAL_PALLET_NO"].ToString()) ||
                        !string.IsNullOrEmpty(dsMaxArkNo.Tables[0].Rows[0]["MAX_ARK_NUMBER"].ToString()))
                    {
                        string maxPalletNo = dsMaxArkNo.Tables[0].Rows[0]["MAX_VIRTUAL_PALLET_NO"].ToString();
                        string maxArkPalletNo = dsMaxArkNo.Tables[0].Rows[0]["MAX_ARK_NUMBER"].ToString();


                        string _fullArk = string.Format(@"  SELECT C.PALLET_QTY,D.SHIP_QTY,C.ARKNUMBER,
                                                                 CASE WHEN (D.SHIP_QTY = C.PALLET_QTY) THEN ('1' + C.ARKNUMBER) + 1 
                                                                 ELSE '1' + C.ARKNUMBER END AS NEWNUMBER
                                                            FROM
                                                            (
                                                                SELECT MAX(substring(A.VIRTUAL_PALLET_NO,2,CHARINDEX('-',A.VIRTUAL_PALLET_NO)-2))AS ARKNUMBER,
                                                                       COUNT(1) PALLET_QTY,
                                                                       A.WORKNUMBER,
                                                                       A.SAP_NO
                                                                FROM dbo.WIP_CONSIGNMENT A
                                                                LEFT JOIN dbo.POR_LOT B ON 
                                                                     A.WORKNUMBER = B.WORK_ORDER_NO AND A.SAP_NO = B.PART_NUMBER
                                                                WHERE B.LOT_NUMBER = '{0}'
                                                                      AND A.VIRTUAL_PALLET_NO LIKE '{1}'+ '[0-9][0-9]-%'
                                                                      AND ISFLAG = 1
                                                                      GROUP BY A.WORKNUMBER,A.SAP_NO 
                                                            ) C
                                                            LEFT JOIN dbo.POR_WO_PRD D ON D.ORDER_NUMBER = C.WORKNUMBER 
                                                                      AND D.PART_NUMBER = C.SAP_NO
                                                            GROUP BY ARKNUMBER,SHIP_QTY,PALLET_QTY", _lotNumber, item_No);
                        dsFullArk = db.ExecuteDataSet(CommandType.Text, _fullArk);
                        if (dsFullArk.Tables[0].Rows.Count > 0)
                        {
                            string _palletQty = dsFullArk.Tables[0].Rows[0]["PALLET_QTY"].ToString().Trim();
                            string _shipQty = dsFullArk.Tables[0].Rows[0]["SHIP_QTY"].ToString().Trim();
                            string _arkNo = dsFullArk.Tables[0].Rows[0]["ARKNUMBER"].ToString().Trim();
                            string _newNo = dsFullArk.Tables[0].Rows[0]["NEWNUMBER"].ToString().Trim();

                            string maxPalletNo_Last = maxPalletNo.Substring(maxPalletNo.Length - 3);
                            if (_palletQty == _shipQty)
                            {
                                _palletNo = item_No.ToString() + _newNo.ToString().Substring(1, 2) + "-" + work_number + "-" + "001";
                            }
                            else
                            {
                                _palletNo = item_No.ToString() + _newNo.ToString().Substring(1, 2) + "-" + work_number + "-" + (Convert.ToInt32(maxPalletNo_Last) + 1).ToString("000");
                            }

                        }
                    }
                    else
                    {
                        _palletNo = item_No.ToString() + "01" + "-" + work_number + "-" + "001";
                    }

                    DataTable dt = new DataTable();
                    dt.Columns.Add("PALLETNO");
                    DataRow dr = dt.NewRow();
                    dr["PALLETNO"] = _palletNo;
                    dt.Rows.Add(dr);
                    dt.TableName = "RETURN";
                    dsReturn.Merge(dt);


                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "没有获取工单信息【自动托号生成】！");
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("NewPalletNo Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 自动生成托号 add by ruhu.yu 
        ///  //工单号(9位)-等级代码(1位)-流水号(4位)
        /// </summary>
        /// <param name="lotNum"></param>
        /// <returns></returns>
        public string GetNewPalletNum(string lotNum)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsReturninfo = new DataSet();
            string RunningCode = string.Empty;
            string NewRunningCode = string.Empty;
            string palletNo = string.Empty;
            string WorkNo = string.Empty;
            string grade = string.Empty;
            try
            {
                //获取工单号和产品等级代码
                string sql = string.Format(@"SELECT  A.[LOT_NUMBER],A.[WORK_ORDER_NO]
                                        , f.GRADE_NAME,(case
		                                when f.GRADE_NAME='客级' then '1'
		                                when f.GRADE_NAME='CA' then '2'
		                                when f.GRADE_NAME='A' then '3'
		                                when f.GRADE_NAME='A02' then '4'
		                                when f.GRADE_NAME='二级品(性能)'
		                                or f.GRADE_NAME='二级品(外观)'
		                                or f.GRADE_NAME='三级品(性能)'
		                                or f.GRADE_NAME='三级品(外观)'
		                                or f.GRADE_NAME='R'
		                                then '5'
		                                when f.GRADE_NAME='A01' then '7'
                                        when f.GRADE_NAME='P' then '8'
                                        when f.GRADE_NAME='C' then '9'
                                        end) GRADE
                                        FROM POR_LOT A
                                        LEFT JOIN WIP_IV_TEST e ON e.LOT_NUM=A.LOT_NUMBER AND e.VC_DEFAULT=1
                                        LEFT JOIN V_ProductGrade f ON f.GRADE_CODE=A.PRO_LEVEL
                                        WHERE A.STATUS < 2 
                                        and A.LOT_NUMBER='{0}'", lotNum);

                dsReturninfo = db.ExecuteDataSet(CommandType.Text, sql);

                if (dsReturninfo.Tables[0].Rows.Count > 0)
                {
                    WorkNo = dsReturninfo.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                    grade = dsReturninfo.Tables[0].Rows[0]["GRADE"].ToString();
                    //获取流水号
                    string sqlRunningCode = string.Format(@"SELECT MAX(RIGHT(PALLET_NO,4)) RUNNINGCODE,WORKNUMBER 
	                                            FROM WIP_CONSIGNMENT 
		                                        WHERE WORKNUMBER='{0}' 
		                                        AND ISNUMERIC(RIGHT(PALLET_NO,4) )=1  
                                                AND LEFT(PALLET_NO,9)=WORKNUMBER
                                                AND ISFLAG=1
	                                            GROUP BY WORKNUMBER", WorkNo);
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlRunningCode);
                    if (dsReturn.Tables[0].Rows.Count > 0)
                    {
                        RunningCode = dsReturn.Tables[0].Rows[0]["RUNNINGCODE"].ToString();
                    }
                    else
                    {
                        RunningCode = "0000";
                    }
                    NewRunningCode = (int.Parse(RunningCode) + 1).ToString("D4");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturninfo, "没有获取工单信息【自动托号生成】！");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetNewPalletNum Error: " + ex.Message);
            }
            palletNo = string.Format("{0}-{1}-{2}", WorkNo, grade, NewRunningCode);

            return palletNo;
        }


        /// <summary>
        /// 判断托盘号是否已存在。yibin.fei 2018.03.13
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        public bool SurePalletNo(string PalletNo)
        {
            DataSet dsReturn = new DataSet();
            bool BeSure = false;

            try
            {
                string _sql = string.Format(@"SELECT PALLET_NO FROM WIP_CONSIGNMENT WHERE PALLET_NO='{0}' AND ISFLAG = 1", PalletNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, _sql);
                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    BeSure = true;
                }

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SurePalletNo Error: " + ex.Message);

            }
            return BeSure;


        }

        /// <summary>
        /// 判断托盘是否包装中托盘。
        /// </summary>
        /// <param name="palletNo">托号</param>
        /// <returns>包含执行结果的数据集对象。托状态</returns>
        public DataSet GetPalletStatus(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string _status = string.Format(@"SELECT CS_DATA_GROUP FROM dbo.WIP_CONSIGNMENT 
                                                 WHERE CS_DATA_GROUP = 0 AND ISFLAG = 1 AND VIRTUAL_PALLET_NO = '{0}'", palletNo);
                dsReturn = db.ExecuteDataSet(CommandType.Text, _status);
                dsReturn.Tables[0].TableName = "STATUS";
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletStatus Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 判断批次是否是SE客户组件。
        /// </summary>
        /// <param name="lotNum"></param>
        /// <returns></returns>
        public bool GetOutLotForSe(string lotNum)
        {
            string sql = string.Format(@"SELECT * FROM dbo.POR_WO_OEM A
                                         LEFT JOIN dbo.POR_LOT B ON a.WORK_ORDER_KEY = b.WORK_ORDER_KEY
                                         WHERE A.CUSROMER = 'SE' AND A.IS_USED = 'Y'
                                         AND B.LOT_NUMBER = '{0}'", lotNum);
            DataSet dsItemNo = db.ExecuteDataSet(CommandType.Text, sql);
            if (dsItemNo.Tables.Count > 0 && dsItemNo.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// 生成SE托盘号。
        /// </summary>
        /// <param name="lotNum"></param>
        /// <returns></returns>
        public DataSet NewSEPalletNo(string lotNum)
        {
            string _constCh = "CH";
            string _year = string.Empty;
            string _weak = string.Empty;
            string _num = string.Empty;
            string _pallet = string.Empty;
            string _palletNo = string.Empty;
            DataSet dsReturn = new DataSet();
            //CHYYWWXXXX Chint + 年两位 2014取14 + WW 第几周 01-53 + 流水码 四位
            try
            {
                _year = DateTime.Now.Year.ToString().Substring(2);
                _weak = GetWeekOfYearFirstDay(DateTime.Now, DayOfWeek.Monday).ToString();
                _pallet = _constCh + _year + _weak;
                string sql = string.Format(@"SELECT TOP 1 VIRTUAL_PALLET_NO 
                                             FROM dbo.WIP_CONSIGNMENT
                                             WHERE VIRTUAL_PALLET_NO LIKE '{0}%'
                                             AND VIRTUAL_PALLET_NO NOT LIKE '{0}%[A-Z]'
                                             ORDER BY VIRTUAL_PALLET_NO DESC", _pallet);
                DataSet dsItemNo = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsItemNo.Tables.Count > 0 && dsItemNo.Tables[0].Rows.Count > 0)
                {
                    string palletNo = Convert.ToString(dsItemNo.Tables[0].Rows[0]["VIRTUAL_PALLET_NO"]);
                    _num = (Convert.ToInt32(palletNo.Substring(6, 4)) + 1).ToString("0000");
                    _palletNo = _pallet + _num;
                }
                else
                {
                    _palletNo = _pallet + "0001";
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("PALLETNO");
                DataRow dr = dt.NewRow();
                dr["PALLETNO"] = _palletNo;
                dt.Rows.Add(dr);
                dt.TableName = "RETURN";
                dsReturn.Merge(dt);
                return dsReturn;
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("NewSEPalletNo Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 今年第几周,年的第一周从年的第一天开始，到指定周的下一个首日结束。
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private int GetWeekOfYearFirstDay(DateTime datetime, DayOfWeek dayOfWeek)
        {
            int weekYear = new System.Globalization.GregorianCalendar()
                                .GetWeekOfYear(datetime,
                                               System.Globalization.CalendarWeekRule.FirstDay,
                                               dayOfWeek);
            return weekYear;
        }

        /// <summary>
        /// 根据过账输入的物料号，工单主键，物料代码
        /// <param name="strmat">过账输入的物料号</param>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料代码</param>
        /// <returns>数据集</returns>
        public DataSet GetWorkOrderBomByMat(string workOrderKey, string mat)
        {
            DataSet dsPorMaterial = new DataSet();
            DataSet dsPorWorkOrderBom = new DataSet();
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@" SELECT A.MATERIAL_CODE,A.BARCODE
                                              FROM POR_MATERIAL A
                                              INNER JOIN POR_WORK_ORDER_BOM B ON B.MATERIAL_CODE = A.MATERIAL_CODE							   
							                                                 AND B.MATERIAL_CODE LIKE '{1}%'
							                                                 AND B.REQ_QTY>0
							                                                 AND A.BARCODE IS NOT NULL
                                              INNER JOIN POR_WORK_ORDER C ON C.ORDER_NUMBER = B.ORDER_NUMBER
						                                                 AND C.ORDER_STATE IN ('TECO','REL')
						                                                 AND C.WORK_ORDER_KEY = '{0}' ", workOrderKey.PreventSQLInjection(), mat);
                dsPorMaterial = db.ExecuteDataSet(CommandType.Text, sql);
                string sql01 = string.Format(@"SELECT DISTINCT MATERIAL_CODE 
                                               FROM dbo.POR_WORK_ORDER_BOM A
                                               INNER JOIN dbo.POR_WORK_ORDER B ON A.ORDER_NUMBER = B.ORDER_NUMBER
                                                                               AND B.ORDER_STATE IN ('TECO','REL')  
                                               WHERE 1=1 
                                               AND A.REQ_QTY>0
                                               AND B.WORK_ORDER_KEY = '{0}' 
                                               AND A.MATERIAL_CODE LIKE '{1}%'", workOrderKey.PreventSQLInjection(), mat);
                dsPorWorkOrderBom = db.ExecuteDataSet(CommandType.Text, sql01);

                dsPorMaterial.Tables[0].TableName = "PorMaterial";
                dsPorWorkOrderBom.Tables[0].TableName = "WorkOrderBom";

                dsReturn.Merge(dsPorMaterial.Tables[0]);
                dsReturn.Merge(dsPorWorkOrderBom.Tables[0]);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderBomByMat Error: " + ex.Message);
            }
            return dsReturn;
        }

        #region ILotOperationEngine 成员

        /// <summary>
        /// 根据参数获取参数组
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <returns></returns>
        public DataSet GetParamerTeamName(string paramName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT A.EDC_NAME FROM dbo.EDC_MAIN A
                                                    LEFT JOIN dbo.EDC_MAIN_PARAM B ON
                                                    A.EDC_KEY = B.EDC_KEY
                                                    LEFT JOIN dbo.BASE_PARAMETER C ON
                                                    B.PARAM_KEY = C.PARAM_KEY
                                                    WHERE C.STATUS = 1 AND A.STATUS = 1
                                                    AND C.PARAM_NAME = '{0}'", paramName);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetParamerTeamName Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ILotOperationEngine 成员


        public DataSet GetWorkOrderBomByWorkKeyAndMat(string workOrderKey, string mat)
        {
            DataSet dsPorMaterial = new DataSet();
            DataSet dsPorWorkOrderBom = new DataSet();
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT A.MATERIAL_CODE,A.BARCODE,B.MATKL
                                              FROM POR_MATERIAL A
                                              INNER JOIN POR_WORK_ORDER_BOM B ON B.MATERIAL_CODE = A.MATERIAL_CODE							   
							                                                 AND B.MATERIAL_CODE LIKE '{1}%'
							                                                 AND B.REQ_QTY>0
							                                                 AND A.BARCODE IS NOT NULL
                                              INNER JOIN POR_WORK_ORDER C ON C.ORDER_NUMBER = B.ORDER_NUMBER
						                                                 AND C.ORDER_STATE IN ('TECO','REL')
						                                                 AND C.WORK_ORDER_KEY = '{0}'", workOrderKey.PreventSQLInjection(), mat);
                dsPorMaterial = db.ExecuteDataSet(CommandType.Text, sql);
                string sql01 = string.Format(@"SELECT A.MATERIAL_CODE,A.MATKL
                                                   FROM dbo.POR_WORK_ORDER_BOM A
                                                   INNER JOIN dbo.POR_WORK_ORDER B ON A.ORDER_NUMBER = B.ORDER_NUMBER
                                                              AND B.ORDER_STATE IN ('TECO','REL')  
                                                   WHERE 1=1 
                                                   AND A.REQ_QTY>0
                                                   AND B.WORK_ORDER_KEY = '{0}' 
                                                   AND A.MATERIAL_CODE LIKE '{1}%'", workOrderKey.PreventSQLInjection(), mat);
                dsPorWorkOrderBom = db.ExecuteDataSet(CommandType.Text, sql01);

                dsPorMaterial.Tables[0].TableName = "PorMaterial";
                dsPorWorkOrderBom.Tables[0].TableName = "WorkOrderBom";

                dsReturn.Merge(dsPorMaterial.Tables[0]);
                dsReturn.Merge(dsPorWorkOrderBom.Tables[0]);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderBomByWorkKeyAndMat Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ILotOperationEngine 成员


        public DataSet GetSpecailBomByWorkKeyAndMat(string workOrderKey, string mat)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@" SELECT A.ORDER_NUMBER,MATERIAL_CODE,MATKL FROM dbo.POR_WORK_ORDER_BOM_EXTENSION A
                                                     LEFT JOIN POR_WORK_ORDER B ON A.ORDER_NUMBER = B.ORDER_NUMBER
                                                     WHERE B.WORK_ORDER_KEY = '{0}' AND A.MATERIAL_CODE LIKE '{1}%'
                                                     AND B.ORDER_STATE IN ('TECO','REL') AND A.STATUS = 1", workOrderKey, mat);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSpecailBomByWorkKeyAndMat Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        /// <summary>
        /// 更新POR_LOT表新工单号字段
        /// </summary>
        /// <param name="newWorkOrder"></param>
        /// <param name="Pallet_No"></param>
        /// <returns></returns>
        public bool UpdatePor_Lot(string newWorkOrder, string Pallet_No)
        {
            DataSet dsReturn = new DataSet();
            //更新新工单
            string sql = string.Format(@"UPDATE POR_LOT SET New_WORK_ORDER_NO='{0}'
                                        WHERE PALLET_NO='{1}'", newWorkOrder.PreventSQLInjection(),
                                        Pallet_No.PreventSQLInjection());

            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新Wip_consignment表新入库单号字段
        /// </summary>
        /// <param name="INWHORDER"></param>
        /// <param name="Pallet_No"></param>
        /// <returns></returns>
        public bool UpdateWip_consignment(string INWHORDER, string Pallet_No)
        {
            DataSet dsReturn = new DataSet();
            //更新新工单
            string sql = string.Format(@"UPDATE WIP_CONSIGNMENT SET INWHORDER='{0}'
                                        WHERE PALLET_NO='{1}'", INWHORDER.PreventSQLInjection(),
                                        Pallet_No.PreventSQLInjection());

            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    using (DbTransaction dbTran = dbConn.BeginTransaction())
                    {
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                        dbTran.Commit();
                    }
                    dbConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
