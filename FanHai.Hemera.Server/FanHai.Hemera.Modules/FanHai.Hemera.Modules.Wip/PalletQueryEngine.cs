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
    /// 托盘数据查询操作类。
    /// </summary>
    public partial class PalletQueryEngine : AbstractEngine, IPalletQueryEngine
    {
        private Database db;//数据库操作对象。
        private Database _dbRead = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PalletQueryEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            //如果配置文件中有只读数据库连接字符串，则设置只读数据库实例
            if (System.Configuration.ConfigurationManager.ConnectionStrings["SQLServerHis"] != null)
            {
                this._dbRead = DatabaseFactory.CreateDatabase("SQLServerHis");
            }
            else //否则和默认数据库使用同样的实例。
            {
                this._dbRead = this.db;
            }
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 查询包含托盘信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <returns>包含托盘信息的数据集。</returns>
        public DataSet SearchPalletList(DataSet dsSearch)
        {
            PagingQueryConfig config = new PagingQueryConfig();
            return SearchPalletList(dsSearch, ref config, false,false);
        }
        /// <summary>
        /// 查询包含托盘信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含托盘信息的数据集。</returns>
        public DataSet SearchPalletList(DataSet dsSearch, ref PagingQueryConfig pconfig)
        {
            return SearchPalletList(dsSearch, ref pconfig, true,false);
        }
        /// <summary>
        /// 查询包含托盘信息及其组件序列号的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含托盘信息及其组件序列号的数据集。。</returns>
        public DataSet SearchPalletDetailList(DataSet dsSearch, ref PagingQueryConfig pconfig)
        {
            return SearchPalletList(dsSearch, ref pconfig, true, true);
        }
        /// <summary>
        /// 查询包含托盘信息及其组件序列号的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含托盘信息及其组件序列号的数据集。。</returns>
        public DataSet SearchPalletDetailList(DataSet dsSearch)
        {
            PagingQueryConfig config = new PagingQueryConfig();
            return SearchPalletList(dsSearch, ref config, false, true);
        }
        /// <summary>
        /// 查询包含托盘信息的数据集。
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
        /// <returns>包含托盘信息的数据集。</returns>
        private DataSet SearchPalletList(DataSet dsSearch, ref PagingQueryConfig pconfig, bool isPaging,bool isDetail)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                if (!isDetail)
                {
                    sBuilder.Append(@"SELECT * FROM
                                     ( 
                                         SELECT A.VIRTUAL_PALLET_NO,A.WORKNUMBER,A.SAP_NO,A.PRO_ID,A.GRADE,A.LOT_NUMBER_QTY,A.ROOM_KEY,
                                                A.POWER_LEVEL,A.TOTLE_POWER,A.AVG_POWER,A.LOT_COLOR,A.CS_DATA_GROUP,
                                                A.CREATE_TIME,A.CREATER,A.CHECK_TIME,A.CHECKER,A.TO_WH_TIME,A.TO_WH,
                                               (SELECT MAX(PALLET_TIME) FROM POR_LOT WHERE PALLET_NO=A.VIRTUAL_PALLET_NO) PALLET_TIME
                                         FROM WIP_CONSIGNMENT A
                                         WHERE A.ISFLAG=1
                                     ) AS A
                                     WHERE 1=1");
                }
                else
                {
                    sBuilder.Append(@"
                                    SELECT * FROM
                                    (
                                        SELECT c.VIRTUAL_PALLET_NO,c.WORKNUMBER,c.SAP_NO,c.PRO_ID,c.GRADE,c.LOT_NUMBER_QTY,
                                               c.POWER_LEVEL,c.TOTLE_POWER,c.AVG_POWER,c.LOT_COLOR,c.CS_DATA_GROUP,
                                               c.CREATE_TIME,c.CREATER,c.CHECK_TIME,c.CHECKER,c.TO_WH_TIME,c.TO_WH,c.ROOM_KEY,
                                               d.ITEM_NO,
                                               t.LOT_NUMBER,
                                               t.WORK_ORDER_NO AS LOT_WORK_ORDER_NO,
                                               t.PRO_ID AS LOT_PRO_ID,
                                               t.PART_NUMBER AS LOT_PART_NUMBER,
                                               t.LOT_SIDECODE,
                                               t.LOT_CUSTOMERCODE,
                                               t.COLOR AS LOT_LOT_COLOR,
                                               t.PRO_LEVEL AS LOT_PRO_LEVEL,
                                               t.QUANTITY AS LOT_QUANTITY,
                                               t.EFFICIENCY AS LOT_EFFICIENCY,
                                               b.GRADE_NAME AS LOT_GRADE_NAME,
                                               a.DEVICENUM,
                                               a.TTIME,
                                               a.VC_MODNAME,
                                               a.PM,a.FF,a.IPM,a.ISC,a.VPM,a.VOC,
                                               a.COEF_PMAX,a.COEF_FF,a.COEF_IMAX,a.COEF_ISC,a.COEF_VMAX,a.COEF_VOC
                                        FROM POR_LOT t 
                                        INNER JOIN WIP_CONSIGNMENT c ON c.VIRTUAL_PALLET_NO=t.PALLET_NO AND c.ISFLAG=1
                                        INNER JOIN WIP_CONSIGNMENT_DETAIL d ON d.CONSIGNMENT_KEY=c.CONSIGNMENT_KEY AND d.LOT_NUMBER=t.LOT_NUMBER
                                        LEFT JOIN WIP_IV_TEST a ON a.LOT_NUM=t.LOT_NUMBER AND a.VC_DEFAULT=1
                                        LEFT JOIN V_ProductGrade b ON b.GRADE_CODE=t.PRO_LEVEL
                                        WHERE t.DELETED_TERM_FLAG<2
                                    ) AS A
                                    WHERE 1=1");
                }
                if (dsSearch != null && dsSearch.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsSearch.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY))
                    {
                        string roomKey = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY]);
                        sBuilder.AppendFormat(" AND A.ROOM_KEY ='{0}'", roomKey.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO))
                    {
                        string palletNo = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                        if (!string.IsNullOrEmpty(palletNo))
                        {
                            string palletNos = UtilHelper.BuilderWhereConditionString("A.VIRTUAL_PALLET_NO", palletNo.Split(new char[] { ',', '\n', '#'}));
                            sBuilder.AppendFormat(palletNos);
                        }
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_START"))
                    {
                        string lotNumber = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_START"]);
                        sBuilder.AppendFormat(" AND A.VIRTUAL_PALLET_NO >='{0}'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_END"))
                    {
                        string lotNumber = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_END"]);
                        sBuilder.AppendFormat(" AND A.VIRTUAL_PALLET_NO <='{0}'", lotNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER))
                    {
                        string orderNumber = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER]);
                        sBuilder.AppendFormat(" AND A.WORKNUMBER LIKE '%{0}%'", orderNumber.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO))
                    {
                        string sapNo = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO]);
                        sBuilder.AppendFormat(" AND A.SAP_NO ='{0}'", sapNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID))
                    {
                        string val = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID]);
                        sBuilder.AppendFormat(" AND A.PRO_ID ='{0}'", val.PreventSQLInjection());
                    }

                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP))
                    {
                        string csDataGroup = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
                        sBuilder.AppendFormat(" AND A.CS_DATA_GROUP ='{0}'", csDataGroup.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT))
                    {
                        string shiftName = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_SHIFT]);
                        sBuilder.AppendFormat(" AND A.SHIFT='{0}'", shiftName.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + "_START"))
                    {
                        string createStartTime = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + "_START"]);
                        sBuilder.AppendFormat(" AND A.PALLET_TIME >='{0}'", createStartTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + "_END"))
                    {
                        string createEndTime = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + "_END"]);
                        sBuilder.AppendFormat(" AND A.PALLET_TIME<='{0}'", createEndTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + "_START"))
                    {
                        string createStartTime = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + "_START"]);
                        sBuilder.AppendFormat(" AND A.CHECK_TIME >='{0}'", createStartTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + "_END"))
                    {
                        string createEndTime = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + "_END"]);
                        sBuilder.AppendFormat(" AND A.CHECK_TIME<='{0}'", createEndTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_TO_WH_TIME + "_START"))
                    {
                        string createStartTime = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_TO_WH_TIME + "_START"]);
                        sBuilder.AppendFormat(" AND A.TO_WH_TIME >='{0}'", createStartTime.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WIP_CONSIGNMENT_FIELDS.FIELDS_TO_WH_TIME + "_END"))
                    {
                        string createEndTime = Convert.ToString(htParams[WIP_CONSIGNMENT_FIELDS.FIELDS_TO_WH_TIME + "_END"]);
                        sBuilder.AppendFormat(" AND A.TO_WH_TIME<='{0}'", createEndTime.PreventSQLInjection());
                    }
                }
                if (!isPaging)
                {
                    sBuilder.Append(" ORDER BY A.VIRTUAL_PALLET_NO ASC,A.CREATE_TIME DESC");
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
                        new string[] { "VIRTUAL_PALLET_NO", "CREATE_TIME"});
                    pconfig.Pages = pages;
                    pconfig.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchPalletList Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
