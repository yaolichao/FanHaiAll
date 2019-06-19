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
    /// 批次数据的操作类。
    /// </summary>
    public partial class LotEngine : AbstractEngine, ILotEngine
    {
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        public DataSet SearchLotList(DataSet dsSearch)
        {
            PagingQueryConfig config = new PagingQueryConfig();
            return SearchLotList(dsSearch, ref config, false);
        }
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        [Obsolete]
        public DataSet SearchLotList(DataSet dsSearch, ref PagingConfig pconfig)
        {
            return SearchLotList(dsSearch, ref pconfig, true);
        }
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        public DataSet SearchLotList(DataSet dsSearch, ref PagingQueryConfig pconfig)
        {
            return SearchLotList(dsSearch, ref pconfig, true);
        }
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <param name="isPaging">
        /// 是否分页查询。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        private DataSet SearchLotList(DataSet dsSearch, ref PagingQueryConfig pconfig, bool isPaging)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                sBuilder.Append(@"SELECT  A.[LOT_KEY],A.[LOT_NUMBER],A.[WORK_ORDER_KEY],A.[WORK_ORDER_NO],A.[WORK_ORDER_SEQ]
                                ,A.[PART_VER_KEY],A.[PART_NUMBER],A.[PRO_ID],A.[PRO_LEVEL],A.[PRIORITY],A.[QUANTITY_INITIAL]
                                ,A.[LOT_LINE_CODE],A.[LOT_LINE_KEY]
                                ,A.[QUANTITY],A.[ROUTE_ENTERPRISE_VER_KEY],A.[CUR_ROUTE_VER_KEY],A.[CUR_STEP_VER_KEY]
                                ,A.[CUR_PRODUCTION_LINE_KEY],A.[LINE_NAME],A.[START_WAIT_TIME],A.[START_PROCESS_TIME]
                                ,A.[EDC_INS_KEY],A.[STATE_FLAG],A.[IS_MAIN_LOT],A.[SPLIT_FLAG],A.[LOT_SEQ],A.[REWORK_FLAG]
                                ,A.[HOLD_FLAG],A.[SHIPPED_FLAG],A.[DELETED_TERM_FLAG],A.[IS_PRINT],A.[LOT_TYPE],A.[CREATE_TYPE]
                                ,A.[COLOR],A.[PALLET_NO],A.[PALLET_TIME],A.[STATUS],A.[OPERATOR],A.[OPR_LINE],A.[OPR_COMPUTER]
                                ,A.[OPR_LINE_PRE],A.[CHILD_LINE],A.[MATERIAL_CODE],A.[MATERIAL_LOT],A.[SUPPLIER_NAME],A.[SI_LOT]
                                ,A.[EFFICIENCY],A.[FACTORYROOM_KEY],A.[FACTORYROOM_NAME],A.[CREATE_OPERTION_NAME],A.[CREATOR]
                                ,A.[CREATE_TIME],A.[CREATE_TIMEZONE_KEY],A.[EDITOR],A.[EDIT_TIME],A.[EDIT_TIMEZONE],A.[SHIFT_NAME]
                                ,A.[DESCRIPTIONS],A.[LOT_SIDECODE],A.[LOT_CUSTOMERCODE],A.[ORG_WORK_ORDER_NO],w.PART_NUMBER AS ORG_PART_NUMBER,
                                E.ENTERPRISE_NAME,
                                D.ROUTE_NAME,
                                B.ROUTE_STEP_NAME,
                               (SELECT TOP 1 EQUIPMENT_KEY 
                                 FROM EMS_LOT_EQUIPMENT 
                                 WHERE LOT_KEY=A.LOT_KEY
                                 AND STEP_KEY=A.CUR_STEP_VER_KEY
                                 AND END_TIMESTAMP IS NULL) AS EQUIPMENT_KEY,
                                 
                               (SELECT EQUIPMENT_NAME
                                FROM EMS_EQUIPMENTS
                                WHERE EQUIPMENT_KEY=(SELECT TOP 1 EQUIPMENT_KEY 
                                                    FROM EMS_LOT_EQUIPMENT 
                                                    WHERE LOT_KEY=A.LOT_KEY
                                                    AND STEP_KEY=A.CUR_STEP_VER_KEY
                                                    AND END_TIMESTAMP IS NULL)) AS EQUIPMENT_NAME
                                FROM POR_LOT A
                                LEFT JOIN POR_WORK_ORDER w ON w.ORDER_NUMBER=A.ORG_WORK_ORDER_NO
                                LEFT JOIN POR_ROUTE_STEP B ON A.CUR_STEP_VER_KEY= B.ROUTE_STEP_KEY
                                LEFT JOIN POR_ROUTE_ROUTE_VER D ON A.CUR_ROUTE_VER_KEY=D.ROUTE_ROUTE_VER_KEY 
                                LEFT JOIN POR_ROUTE_ENTERPRISE_VER E ON  A.ROUTE_ENTERPRISE_VER_KEY =E.ROUTE_ENTERPRISE_VER_KEY
                                WHERE A.STATUS < 2");
                if (dsSearch != null && dsSearch.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsSearch.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                    {
                        string roomKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                        sBuilder.AppendFormat(" AND A.FACTORYROOM_KEY ='{0}'", roomKey.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                    {
                        string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                        sBuilder.AppendFormat(" AND A.LOT_NUMBER LIKE '%{0}%'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_START"))
                    {
                        string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_START"]);
                        sBuilder.AppendFormat(" AND A.LOT_NUMBER >='{0}'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_END"))
                    {
                        string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_END"]);
                        sBuilder.AppendFormat(" AND A.LOT_NUMBER <='{0}'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_HOLD_FLAG))
                    {
                        string holdFalg = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
                        sBuilder.AppendFormat(" AND A.HOLD_FLAG={0}", holdFalg.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_TYPE))
                    {
                        string lotType = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
                        sBuilder.AppendFormat(" AND A.LOT_TYPE='{0}'", lotType.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_STATE_FLAG))
                    {
                        string stateFlag = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                        sBuilder.AppendFormat(" AND A.STATE_FLAG={0}", stateFlag.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATOR))
                    {
                        string creator = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATOR]);
                        sBuilder.AppendFormat(" AND A.CREATOR='{0}'", creator.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG))
                    {
                        string deletedTermFlag = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
                        sBuilder.AppendFormat(" AND A.DELETED_TERM_FLAG={0}", deletedTermFlag.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG))
                    {
                        string flag = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_SHIPPED_FLAG]);
                        sBuilder.AppendFormat(" AND A.SHIPPED_FLAG={0}", flag.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID))
                    {
                        string proId = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PRO_ID]);
                        sBuilder.AppendFormat(" AND A.PRO_ID LIKE '%{0}%'", proId.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PART_NUMBER))
                    {
                        string partNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                        sBuilder.AppendFormat(" AND A.PART_NUMBER LIKE '%{0}%'", partNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO))
                    {
                        string orderNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                        sBuilder.AppendFormat(" AND A.WORK_ORDER_NO LIKE '%{0}%'", orderNumber.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO))
                    {
                        string orderNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_ORG_WORK_ORDER_NO]);
                        sBuilder.AppendFormat(" AND A.ORG_WORK_ORDER_NO LIKE '%{0}%'", orderNumber.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PALLET_NO))
                    {
                        string palletNo = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                        sBuilder.AppendFormat(" AND A.PALLET_NO LIKE '%{0}%'", palletNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LINE_NAME))
                    {
                        string lineName = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                        sBuilder.AppendFormat(" AND A.LINE_NAME ='{0}'", lineName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_SHIFT_NAME))
                    {
                        string shiftName = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_SHIFT_NAME]);
                        sBuilder.AppendFormat(" AND A.SHIFT_NAME='{0}'", shiftName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME))
                    {
                        string enterpriseName = Convert.ToString(htParams[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                        sBuilder.AppendFormat(" AND E.ENTERPRISE_NAME='{0}'", enterpriseName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME))
                    {
                        string routeName = Convert.ToString(htParams[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                        sBuilder.AppendFormat(" AND D.ROUTE_NAME='{0}'", routeName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME))
                    {
                        string stepName = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME]);
                        sBuilder.AppendFormat(" AND B.ROUTE_STEP_NAME='{0}'", stepName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TIME + "_START"))
                    {
                        string createStartTime = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATE_TIME + "_START"]);
                        sBuilder.AppendFormat(" AND A.CREATE_TIME >='{0}'", createStartTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TIME + "_END"))
                    {
                        string createEndTime = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATE_TIME + "_END"]);
                        sBuilder.AppendFormat(" AND A.CREATE_TIME<='{0}'", createEndTime.PreventSQLInjection());
                    }
                }
                if (!isPaging)
                {
                    sBuilder.Append(" ORDER BY A.LOT_NUMBER ASC,A.CREATE_TIME DESC");
                    dsReturn = this._dbRead.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sBuilder.ToString(),
                        pconfig.PageNo,
                        pconfig.PageSize,
                        out pages,
                        out records,
                        this._dbRead,
                        dsReturn,
                        POR_LOT_FIELDS.DATABASE_TABLE_NAME,
                        "ASC",
                        new string[] { "LOT_NUMBER", "CREATE_TIME"});
                    dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_WAIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                    dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                    dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_EDIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                    pconfig.Pages = pages;
                    pconfig.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchLotList Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <param name="isPaging">
        /// 是否分页查询。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        [Obsolete]
        private DataSet SearchLotList(DataSet dsSearch, ref PagingConfig pconfig, bool isPaging)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                sBuilder.Append(@"SELECT   A.[LOT_KEY],A.[LOT_NUMBER],A.[WORK_ORDER_KEY],A.[WORK_ORDER_NO],A.[WORK_ORDER_SEQ]
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
                                    E.ENTERPRISE_NAME,
                                    D.ROUTE_NAME,
                                    B.ROUTE_STEP_NAME,
                                (SELECT TOP 1 EQUIPMENT_KEY 
                                     FROM EMS_LOT_EQUIPMENT 
                                     WHERE LOT_KEY=A.LOT_KEY
                                     AND STEP_KEY=A.CUR_STEP_VER_KEY
                                     AND END_TIMESTAMP IS NULL) AS EQUIPMENT_KEY,
                                     
                                   (SELECT EQUIPMENT_NAME
                                    FROM EMS_EQUIPMENTS
                                    WHERE EQUIPMENT_KEY=(SELECT TOP 1 EQUIPMENT_KEY 
                                                        FROM EMS_LOT_EQUIPMENT 
                                                        WHERE LOT_KEY=A.LOT_KEY
                                                        AND STEP_KEY=A.CUR_STEP_VER_KEY
                                                        AND END_TIMESTAMP IS NULL)) AS EQUIPMENT_NAME
                                FROM POR_LOT A
                                LEFT JOIN POR_ROUTE_STEP B ON A.CUR_STEP_VER_KEY= B.ROUTE_STEP_KEY
                                LEFT JOIN POR_ROUTE_ROUTE_VER D ON A.CUR_ROUTE_VER_KEY=D.ROUTE_ROUTE_VER_KEY 
                                LEFT JOIN POR_ROUTE_ENTERPRISE_VER E ON  A.ROUTE_ENTERPRISE_VER_KEY =E.ROUTE_ENTERPRISE_VER_KEY   
                                WHERE A.STATUS < 2");
                if (dsSearch != null && dsSearch.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsSearch.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY))
                    {
                        string roomKey = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                        sBuilder.AppendFormat(" AND A.FACTORYROOM_KEY ='{0}'",roomKey.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER))
                    {
                        string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                        sBuilder.AppendFormat(" AND A.LOT_NUMBER LIKE '%{0}%'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER+"_START"))
                    {
                        string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_START"]);
                        sBuilder.AppendFormat(" AND A.LOT_NUMBER >='{0}'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_END"))
                    {
                        string lotNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_NUMBER + "_END"]);
                        sBuilder.AppendFormat(" AND A.LOT_NUMBER <='{0}'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_HOLD_FLAG))
                    {
                        string holdFalg = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
                        sBuilder.AppendFormat(" AND A.HOLD_FLAG={0}", holdFalg.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LOT_TYPE))
                    {
                        string lotType = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
                        sBuilder.AppendFormat(" AND A.LOT_TYPE='{0}'", lotType.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_STATE_FLAG))
                    {
                        string stateFlag = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                        sBuilder.AppendFormat(" AND A.STATE_FLAG={0}", stateFlag.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATOR))
                    {
                        string creator = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATOR]);
                        sBuilder.AppendFormat(" AND A.CREATOR='{0}'", creator.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG))
                    {
                        string deletedTermFlag = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
                        sBuilder.AppendFormat(" AND A.DELETED_TERM_FLAG={0}", deletedTermFlag.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_SHIPPED_FLAG))
                    {
                        string flag = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_SHIPPED_FLAG]);
                        sBuilder.AppendFormat(" AND A.SHIPPED_FLAG={0}", flag.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PRO_ID))
                    {
                        string proId = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PRO_ID]);
                        sBuilder.AppendFormat(" AND A.PRO_ID LIKE '%{0}%'", proId.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PART_NUMBER))
                    {
                        string partNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
                        sBuilder.AppendFormat(" AND A.PART_NUMBER LIKE '%{0}%'", partNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO))
                    {
                        string orderNumber = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                        sBuilder.AppendFormat(" AND A.WORK_ORDER_NO LIKE '%{0}%'", orderNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_PALLET_NO))
                    {
                        string palletNo = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                        sBuilder.AppendFormat(" AND A.PALLET_NO LIKE '%{0}%'", palletNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_LINE_NAME))
                    {
                        string lineName = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                        sBuilder.AppendFormat(" AND A.LINE_NAME ='{0}'", lineName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_SHIFT_NAME))
                    {
                        string shiftName = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_SHIFT_NAME]);
                        sBuilder.AppendFormat(" AND A.SHIFT_NAME='{0}'", shiftName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME))
                    {
                        string enterpriseName = Convert.ToString(htParams[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                        sBuilder.AppendFormat(" AND E.ENTERPRISE_NAME='{0}'", enterpriseName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME))
                    {
                        string routeName = Convert.ToString(htParams[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                        sBuilder.AppendFormat( " AND D.ROUTE_NAME='{0}'", routeName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME))
                    {
                        string stepName = Convert.ToString(htParams[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME]);
                        sBuilder.AppendFormat( " AND B.ROUTE_STEP_NAME='{0}'",  stepName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TIME+"_START"))
                    {
                        string createStartTime = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATE_TIME + "_START"]);
                        sBuilder.AppendFormat(" AND A.CREATE_TIME>= '{0}'", createStartTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(POR_LOT_FIELDS.FIELD_CREATE_TIME + "_END"))
                    {
                        string createEndTime = Convert.ToString(htParams[POR_LOT_FIELDS.FIELD_CREATE_TIME + "_END"]);
                        sBuilder.AppendFormat(" AND A.CREATE_TIME<='{0}'", createEndTime.PreventSQLInjection());
                    }
                }
                if (!isPaging)
                {
                    sBuilder.Append(" ORDER BY A.LOT_NUMBER ASC,A.CREATE_TIME DESC");
                    dsReturn = this._dbRead.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sBuilder.ToString(), 
                        pconfig.PageNo, 
                        pconfig.PageSize, 
                        out pages,
                        out records,
                        this._dbRead, 
                        dsReturn, 
                        POR_LOT_FIELDS.DATABASE_TABLE_NAME,
                        "ASC",
                        new string[] { "LOT_NUMBER", "CREATE_TIME" });
                    dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_WAIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                    dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                    dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_EDIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                    pconfig.Pages = pages;
                    pconfig.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchLotList Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据批次号获取批次信息。
        /// </summary>
        /// <param name="lotNo">批次号或批次主键。</param>
        /// <returns>
        /// 包含批次信息的数据集。
        /// </returns>
        public DataSet GetLotInfo(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            string sql = @"SELECT TOP 1  A.[LOT_KEY],A.[LOT_NUMBER],A.[WORK_ORDER_KEY],A.[WORK_ORDER_NO],A.[WORK_ORDER_SEQ]
                                ,A.[PART_VER_KEY],A.[PART_NUMBER],A.[PRO_ID],A.[PRO_LEVEL],A.[PRIORITY],A.[QUANTITY_INITIAL]
                                ,A.[LOT_LINE_CODE],A.[LOT_LINE_KEY]
                                ,A.[QUANTITY],A.[ROUTE_ENTERPRISE_VER_KEY],A.[CUR_ROUTE_VER_KEY],A.[CUR_STEP_VER_KEY]
                                ,A.[CUR_PRODUCTION_LINE_KEY],A.[LINE_NAME],A.[START_WAIT_TIME],A.[START_PROCESS_TIME]
                                ,A.[EDC_INS_KEY],A.[STATE_FLAG],A.[IS_MAIN_LOT],A.[SPLIT_FLAG],A.[LOT_SEQ],A.[REWORK_FLAG]
                                ,A.[HOLD_FLAG],A.[SHIPPED_FLAG],A.[DELETED_TERM_FLAG],A.[IS_PRINT],A.[LOT_TYPE],A.[CREATE_TYPE]
                                ,A.[COLOR],A.[PALLET_NO],A.[PALLET_TIME],A.[STATUS],A.[OPERATOR],A.[OPR_LINE],A.[OPR_COMPUTER]
                                ,A.[OPR_LINE_PRE],A.[CHILD_LINE],A.[MATERIAL_CODE],A.[MATERIAL_LOT],A.[SUPPLIER_NAME],A.[SI_LOT]
                                ,A.[EFFICIENCY],A.[FACTORYROOM_KEY],A.[FACTORYROOM_NAME],A.[CREATE_OPERTION_NAME],A.[CREATOR]
                                ,A.[CREATE_TIME],A.[CREATE_TIMEZONE_KEY],A.[EDITOR],A.[EDIT_TIME],A.[EDIT_TIMEZONE],A.[SHIFT_NAME]
                                ,A.[DESCRIPTIONS],A.[LOT_SIDECODE],A.[LOT_CUSTOMERCODE],A.ORG_WORK_ORDER_NO,
                            B.ENTERPRISE_NAME,
                            B.ENTERPRISE_VERSION,
                            C.ROUTE_NAME ,
                            D.ROUTE_STEP_NAME,
                            (SELECT TOP 1 EQUIPMENT_KEY 
                                 FROM EMS_LOT_EQUIPMENT 
                                 WHERE LOT_KEY=A.LOT_KEY
                                 AND STEP_KEY=A.CUR_STEP_VER_KEY
                                 AND END_TIMESTAMP IS NULL) AS EQUIPMENT_KEY,
                                 
                               (SELECT EQUIPMENT_NAME
                                FROM EMS_EQUIPMENTS
                                WHERE EQUIPMENT_KEY=(SELECT TOP 1 EQUIPMENT_KEY 
                                                    FROM EMS_LOT_EQUIPMENT 
                                                    WHERE LOT_KEY=A.LOT_KEY
                                                    AND STEP_KEY=A.CUR_STEP_VER_KEY
                                                    AND END_TIMESTAMP IS NULL)) AS EQUIPMENT_NAME
                        FROM POR_LOT A
                        LEFT JOIN POR_ROUTE_ENTERPRISE_VER B ON B.ROUTE_ENTERPRISE_VER_KEY=  A.ROUTE_ENTERPRISE_VER_KEY
                        LEFT JOIN POR_ROUTE_ROUTE_VER C ON C.ROUTE_ROUTE_VER_KEY=A.CUR_ROUTE_VER_KEY
                        LEFT JOIN POR_ROUTE_STEP D ON D.ROUTE_STEP_KEY=A.CUR_STEP_VER_KEY
                        WHERE A.STATUS<> 2 
                        AND (A.LOT_NUMBER='{0}' OR A.LOT_KEY='{0}')";
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, lotNo.PreventSQLInjection()));
                dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_WAIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_EDIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotInfo Error: " + ex.Message);
            }
            //返回结果集。
            return dsReturn;
        }

        /// <summary>
        /// 根据托盘号获取批次信息。
        /// </summary>
        /// <param name="pallet_no">托盘号</param>
        /// <returns>包含批次信息的数据集</returns>
        public DataSet GetLotInfoByPallet_No(string pallet_no)
        {
            DataSet dsReturn = new DataSet();
            string sql = @"SELECT *
                        FROM POR_LOT A
                        WHERE A.Pallet_NO='{0}'";
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, pallet_no.PreventSQLInjection()));
                dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_WAIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_START_PROCESS_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                dsReturn.Tables[0].Columns[POR_LOT_FIELDS.FIELD_EDIT_TIME].DateTimeMode = DataSetDateTime.Unspecified;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLotInfo Error: " + ex.Message);
            }
            //返回结果集。
            return dsReturn;
        }

    }
}
