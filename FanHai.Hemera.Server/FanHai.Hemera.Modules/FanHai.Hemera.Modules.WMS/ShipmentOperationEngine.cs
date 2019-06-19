using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface;
using System.Data;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections;


namespace FanHai.Hemera.Modules.WMS
{
    /// <summary>
    /// 出货操作类。
    /// </summary>
    public class ShipmentOperationEngine : AbstractEngine,IShipmentOperationEngine
    {
        private Database db;                             //用于数据库访问的变量。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ShipmentOperationEngine()
        {
            db = DatabaseFactory.CreateDatabase();//SQLServerAwms
        }

        //public ShipmentOperationEngine(string CON)
        //{
        //    db = DatabaseFactory.CreateDatabase(CON);
        //}
       
        public override void Initialize(){}
        /// <summary>
        /// 获取出货单中的指定柜号当前已入柜数量。
        /// </summary>
        /// <param name="shipmentNo">出货单号。</param>
        /// <param name="containerNo">货柜号。</param>
        /// <returns>指定柜号当前已入柜数量。</returns>
        public double GetContainerQuantity(string shipmentNo, string containerNo)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT ISNULL(SUM(QUANTITY),0) 
                                    FROM WMS_SHIPMENT
                                    WHERE SHIPMENT_NO='{0}' AND CONTAINER_NO='{1}'
                                    AND IS_FLAG=1",
                                    shipmentNo.PreventSQLInjection(),
                                    containerNo.PreventSQLInjection());
                //执行查询。
                object obj = db.ExecuteScalar(CommandType.Text, sbSql.ToString());
                return Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetContainerQuantity Error: " + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 获取出货单当前已出货数量。
        /// </summary>
        /// <param name="shipmentNo">出货单号。</param>
        /// <returns>已出货数量。</returns>
        public double GetShipmentQuantity(string shipmentNo)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT ISNULL(SUM(QUANTITY),0) 
                                    FROM WMS_SHIPMENT
                                    WHERE SHIPMENT_NO='{0}'
                                    AND IS_FLAG=1",
                                    shipmentNo.PreventSQLInjection());
                //执行查询。
                object obj = db.ExecuteScalar(CommandType.Text, sbSql.ToString());
                return Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetShipmentQuantity Error: " + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取CI当前已出货数量。
        /// </summary>
        /// <param name="ciNumber">CI单号。</param>
        /// <returns>指定CI号当前已出货数量。</returns>
        public double GetCIQuantity(string ciNumber)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT ISNULL(SUM(QUANTITY),0) 
                                    FROM WMS_SHIPMENT
                                    WHERE CI_NO='{0}'
                                    AND IS_FLAG=1",
                                    ciNumber.PreventSQLInjection());
                //执行查询。
                object obj = db.ExecuteScalar(CommandType.Text, sbSql.ToString());
                return Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetShipmentQuantity Error: " + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 根据托盘号获取托盘信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含托盘信息的数据集对象。</returns>
        public DataSet GetPalletInfo(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sbSql = new StringBuilder();
//                sbSql.AppendFormat(@"SELECT A.*
//                                    FROM WIP_CONSIGNMENT A
//                                    WHERE A.VIRTUAL_PALLET_NO='{0}'
//                                    AND A.ISFLAG=1",
//                                    palletNo.PreventSQLInjection());
                //add by chao.pang 20140515
                sbSql.AppendFormat(@"   SELECT 1 AS ROWNUMBER, A.*,
                                            (CASE WHEN C.SUB_PS_WAY = '功率' AND ISNULL(D.PS_SUB_CODE,0)>0 THEN 
                                            '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(D.P_DTL_MIN,'0'),'.'))+','+
                                            CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(D.P_DTL_MAX,'0'),'.'))+')'
                                            ELSE '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(C.P_MIN,'0'),'.'))+','+
                                            CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(C.P_MAX,'0'),'.'))+')' END) AS POWER_RANGE                                                                                       
                                        FROM dbo.WIP_CONSIGNMENT A
                                        LEFT JOIN dbo.POR_WORK_ORDER B 
                                            ON B.ORDER_NUMBER = A.WORKNUMBER 
                                                    AND B.ORDER_STATE IN ('TECO','REL')
                                        LEFT JOIN POR_WO_PRD_PS C ON 
                                                    C.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                    AND C.PART_NUMBER = B.PART_NUMBER 
                                                    AND C.IS_USED = 'Y'
                                                    AND C.PS_CODE=A.PS_CODE 
                                                    AND C.PMAXSTAB = A.POWER_LEVEL 
                                                    AND A.PS_SEQ = C.POWERSET_KEY
                                        LEFT JOIN dbo.POR_WO_PRD_PS_SUB D
                                            ON D.WORK_ORDER_KEY = C.WORK_ORDER_KEY
                                                    AND D.PART_NUMBER  = C.PART_NUMBER
                                                    AND D.POWERSET_KEY = C.POWERSET_KEY
                                                    AND D.POWERLEVEL = A.PS_DTL_SUBCODE
                                                    AND D.IS_USED='Y'
                                        WHERE A.ISFLAG = 1 AND A.VIRTUAL_PALLET_NO = '{0}'", palletNo.PreventSQLInjection());
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 托盘出货作业。
        /// </summary>
        /// <param name="dsParams">包含托盘操作信息的数据集对象。</param>
        /// <returns>包含托盘出货作业结果的数据集对象。</returns>
        public DataSet PalletShipment(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null == dsParams
                || !dsParams.Tables.Contains(WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME))
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入的参数错误。");
                return dsReturn; 
            }
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                
                DataTable dtShipment = dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME];
                //检查是否存在重复的托盘记录。
                var lnq = from item in dtShipment.AsEnumerable()
                          group item by item[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复托盘记录，请检查。");
                    return dsReturn;
                }
                WMS_SHIPMENT_FIELDS shipmentFields = new WMS_SHIPMENT_FIELDS();
                //循环出货信息。
                foreach (DataRow dr in dtShipment.Rows)
                {
                    string palletNo = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]);
                    string editor = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_EDITOR]);
                    string editTimeZone = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIMEZONE]);
                    int editCustCheck = Convert.ToInt32(dr[WMS_SHIPMENT_FIELDS.FIELDS_CUSTCHECK]);
                    //更新托盘信息为出货。
//                    string sql = string.Format(@"UPDATE WIP_CONSIGNMENT_HIS 
//                                                     SET CS_DATA_GROUP=4,EDITOR='{1}',EDIT_TIME=GETDATE(),OUT_WH='{1}',OUT_WH_TIME=GETDATE()
//                                                     WHERE VIRTUAL_PALLET_NO='{0}'",
//                                                 palletNo.PreventSQLInjection(),
//                                                 editor.PreventSQLInjection());
//                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //更新托盘中的批次记录为已出货。
//                    sql = string.Format(@"UPDATE POR_LOT_HIS
//                                       SET SHIPPED_FLAG=1,EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
//                                       WHERE PALLET_NO='{0}'",
//                                       palletNo.PreventSQLInjection(),
//                                       editor.PreventSQLInjection(),
//                                       editTimeZone.PreventSQLInjection());
//                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //
                    //更新托盘信息为出货。
                                        string sql = string.Format(@"UPDATE WIP_CONSIGNMENT
                                                                         SET CS_DATA_GROUP=4,EDITOR='{1}',EDIT_TIME=GETDATE(),OUT_WH='{1}',OUT_WH_TIME=GETDATE()
                                                                         WHERE VIRTUAL_PALLET_NO='{0}'",
                                                                     palletNo.PreventSQLInjection(),
                                                                     editor.PreventSQLInjection());
                                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //更新托盘中的批次记录为已出货。
                                        sql = string.Format(@"UPDATE POR_LOT
                                                           SET SHIPPED_FLAG=1,EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_TIMEZONE='{2}'
                                                           WHERE PALLET_NO='{0}'",
                                                           palletNo.PreventSQLInjection(),
                                                           editor.PreventSQLInjection(),
                                                           editTimeZone.PreventSQLInjection());
                                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    //新增出货记录
                    //插入一笔工序参数数据。                    
                    dr[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIME] = DBNull.Value;
                    dr[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_KEY] = UtilHelper.GenerateNewKey(0);
                    sql = DatabaseTable.BuildInsertSqlStatement(shipmentFields, dr, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PalletShipment Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return dsReturn;
        }

        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig config)
        {
            return Query(dsParams, ref config, true);
        }
        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet Query(DataSet dsParams)
        {
            PagingQueryConfig config = new PagingQueryConfig();
            return Query(dsParams, ref config, false);
        }
        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        private DataSet Query(DataSet dsParams, ref PagingQueryConfig pconfig, bool isPaging)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
//                sBuilder.Append(@"SELECT  a.VIRTUAL_PALLET_NO,a.LOT_NUMBER_QTY,a.SAP_NO,a.WORKNUMBER,a.GRADE,
//                                        a.CS_DATA_GROUP,a.PRO_ID,a.POWER_LEVEL,a.TOTLE_POWER,E.ARTICNO,0 AS HFLAG,
//                                        b.*
//                                FROM WIP_CONSIGNMENT a
//                                INNER JOIN POR_WORK_ORDER c ON c.ORDER_NUMBER=a.WORKNUMBER
//                                                          AND C.ORDER_STATE IN ('TECO','REL')
//                                LEFT JOIN POR_WO_PRD_PS d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY
//                                                          AND d.PART_NUMBER=a.SAP_NO
//                                                          AND d.PS_CODE=a.PS_CODE
//                                                          AND d.PMAXSTAB=a.POWER_LEVEL
//                                                          AND d.IS_USED='Y'
//                                LEFT JOIN POR_WO_PRD_PS_CLR e ON e.WORK_ORDER_KEY=c.WORK_ORDER_KEY
//                                                              AND e.PART_NUMBER=a.SAP_NO
//                                                              AND e.POWERSET_KEY=d.POWERSET_KEY
//                                                              AND e.COLOR_NAME=a.LOT_COLOR
//                                                              AND e.IS_USED='Y'
//                                LEFT JOIN WMS_SHIPMENT b ON b.PALLET_NO=a.VIRTUAL_PALLET_NO
//                                WHERE a.CS_DATA_GROUP>2
//                                AND a.ISFLAG=1 ");
                sBuilder.Append(@" SELECT ROW_NUMBER() OVER (ORDER BY VIRTUAL_PALLET_NO) AS ROWNUMBER, a.VIRTUAL_PALLET_NO,a.LOT_NUMBER_QTY,a.SAP_NO,a.WORKNUMBER,a.GRADE,
                                    a.CS_DATA_GROUP,a.PRO_ID,a.POWER_LEVEL,a.TOTLE_POWER,E.ARTICNO,0 AS HFLAG,
                                    (CASE WHEN c.WORK_ORDER_KEY IS NULL THEN ''
                                          WHEN d.SUB_PS_WAY = '功率' AND ISNULL(f.PS_SUB_CODE,0)>0 THEN 
                                               '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(f.P_DTL_MIN,'0'),'.'))+','+CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(f.P_DTL_MAX,'0'),'.'))+')'
                                          ELSE '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(d.P_MIN,'0'),'.'))+','+CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(d.P_MAX,'0'),'.'))+')' 
                                     END) AS POWERRANGE,
                                    b.*
                                    FROM dbo.WIP_CONSIGNMENT a
                                    LEFT JOIN POR_WORK_ORDER c ON c.ORDER_NUMBER=a.WORKNUMBER
                                          AND C.ORDER_STATE IN ('TECO','REL')
                                    LEFT JOIN POR_WO_PRD_PS d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY
                                          AND d.PART_NUMBER=a.SAP_NO
                                          AND d.PS_CODE=a.PS_CODE
                                          AND d.PMAXSTAB=a.POWER_LEVEL
                                          AND d.IS_USED='Y'
                                          AND A.PS_SEQ = D.POWERSET_KEY
                                    LEFT JOIN dbo.POR_WO_PRD_PS_SUB f
		                                    ON f.WORK_ORDER_KEY = d.WORK_ORDER_KEY
		                                    AND f.PART_NUMBER  = d.PART_NUMBER
		                                    AND f.POWERSET_KEY = d.POWERSET_KEY
		                                    AND f.POWERLEVEL = A.PS_DTL_SUBCODE
		                                    AND f.IS_USED='Y'
                                    LEFT JOIN POR_WO_PRD_PS_CLR e ON e.WORK_ORDER_KEY=c.WORK_ORDER_KEY
                                              AND e.PART_NUMBER=a.SAP_NO
                                              AND e.POWERSET_KEY=d.POWERSET_KEY
                                              AND e.COLOR_NAME=a.LOT_COLOR
                                              AND e.IS_USED='Y'
                                    LEFT JOIN WMS_SHIPMENT b ON b.PALLET_NO=a.VIRTUAL_PALLET_NO AND b.IS_FLAG=1
                                    WHERE a.CS_DATA_GROUP>2
                                    AND a.ISFLAG=1 ");
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO))
                    {
                        string containerNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO]);
                        sBuilder.AppendFormat(" AND b.CONTAINER_NO LIKE '{0}%'", containerNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO))
                    {
                        string shipmentNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_NO LIKE '{0}%'", shipmentNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE))
                    {
                        string shipmentType = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_TYPE = '{0}'", shipmentType.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_CI_NO))
                    {
                        string ciNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_CI_NO]);
                        sBuilder.AppendFormat(" AND b.CI_NO LIKE '{0}%'", ciNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO))
                    {
                        string palletNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]);
                        if (!string.IsNullOrEmpty(palletNo))
                        {
                            string palletNos = UtilHelper.BuilderWhereConditionString("a.VIRTUAL_PALLET_NO", palletNo.Split(new char[] { ',', '\n', '#' }));
                            sBuilder.AppendFormat(palletNos);
                        }
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_START"))
                    {
                        string shipmentDate = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_START"]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_DATE >='{0}'", shipmentDate.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_END"))
                    {
                        string shipmentDate = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_END"]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_DATE<='{0}'", shipmentDate.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_STATUS))
                    {
                        string shipmentStatus = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_STATUS]);
                        string status = string.Empty;
                        if (shipmentStatus.Equals("已保存未出货"))
                            status = "0";
                        if (shipmentStatus.Equals("已出货"))
                            status = "1";
                        sBuilder.AppendFormat(" AND b.SHIPMENT_FLAG = '{0}'", status.PreventSQLInjection());
                    }

                }
                if (!isPaging)
                {
                    sBuilder.Append(" ORDER BY b.SHIPMENT_NO ASC,b.CONTAINER_NO ASC,b.SHIPMENT_DATE DESC,a.VIRTUAL_PALLET_NO ASC");
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
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
                        db,
                        dsReturn,
                        POR_LOT_FIELDS.DATABASE_TABLE_NAME,
                        "SHIPMENT_NO ASC,CONTAINER_NO ASC,SHIPMENT_DATE DESC,VIRTUAL_PALLET_NO ASC");
                    pconfig.Pages = pages;
                    pconfig.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ShipmentOperationEngine Query Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet ShipmentQuery(DataSet dsParams, ref PagingQueryConfig pconfig)
        {
            bool isPaging = true;
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                sBuilder.Append(@" SELECT ROW_NUMBER() OVER (ORDER BY VIRTUAL_PALLET_NO) AS ROWNUMBER, a.VIRTUAL_PALLET_NO,count(1) as LOT_NUMBER_QTY,a.SAP_NO,a.WORKNUMBER,a.GRADE,
                                            a.CS_DATA_GROUP,a.PRO_ID,g.POWER_LEVEL,a.TOTLE_POWER,E.ARTICNO,0 AS HFLAG,
                                            (CASE WHEN c.WORK_ORDER_KEY IS NULL THEN ''
                                                  WHEN d.SUB_PS_WAY = '功率' AND ISNULL(f.PS_SUB_CODE,0)>0 THEN 
                                                       '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(f.P_DTL_MIN,'0'),'.'))+','+CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(f.P_DTL_MAX,'0'),'.'))+')'
                                                  ELSE '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(d.P_MIN,'0'),'.'))+','+CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(d.P_MAX,'0'),'.'))+')' 
                                             END) AS POWERRANGE,
                                            b.*
                                            FROM WIP_CONSIGNMENT_DETAIL g 
                                            left join dbo.WIP_CONSIGNMENT a on g.CONSIGNMENT_KEY = a.CONSIGNMENT_KEY
                                            LEFT JOIN POR_WORK_ORDER c ON c.ORDER_NUMBER=a.WORKNUMBER
                                                  AND C.ORDER_STATE IN ('TECO','REL')
                                            LEFT JOIN POR_WO_PRD_PS d ON d.WORK_ORDER_KEY=c.WORK_ORDER_KEY
                                                  AND d.PART_NUMBER=a.SAP_NO
                                                  AND d.PS_CODE=a.PS_CODE
                                                  AND d.PMAXSTAB=a.POWER_LEVEL
                                                  AND d.IS_USED='Y'
                                                  AND A.PS_SEQ = D.POWERSET_KEY
                                            LEFT JOIN dbo.POR_WO_PRD_PS_SUB f
                                                    ON f.WORK_ORDER_KEY = d.WORK_ORDER_KEY
                                                    AND f.PART_NUMBER  = d.PART_NUMBER
                                                    AND f.POWERSET_KEY = d.POWERSET_KEY
                                                    AND f.POWERLEVEL = A.PS_DTL_SUBCODE
                                                    AND f.IS_USED='Y'
                                            LEFT JOIN POR_WO_PRD_PS_CLR e ON e.WORK_ORDER_KEY=c.WORK_ORDER_KEY
                                                      AND e.PART_NUMBER=a.SAP_NO
                                                      AND e.POWERSET_KEY=d.POWERSET_KEY
                                                      AND e.COLOR_NAME=a.LOT_COLOR
                                                      AND e.IS_USED='Y'
                                            LEFT JOIN WMS_SHIPMENT b ON b.PALLET_NO=a.VIRTUAL_PALLET_NO AND b.IS_FLAG=1
                                            WHERE a.CS_DATA_GROUP>2
                                            AND a.ISFLAG=1  ");
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO))
                    {
                        string containerNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO]);
                        sBuilder.AppendFormat(" AND b.CONTAINER_NO LIKE '{0}%'", containerNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO))
                    {
                        string shipmentNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_NO LIKE '{0}%'", shipmentNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE))
                    {
                        string shipmentType = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_TYPE = '{0}'", shipmentType.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_CI_NO))
                    {
                        string ciNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_CI_NO]);
                        sBuilder.AppendFormat(" AND b.CI_NO LIKE '{0}%'", ciNo.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO))
                    {
                        string palletNo = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]);
                        if (!string.IsNullOrEmpty(palletNo))
                        {
                            string palletNos = UtilHelper.BuilderWhereConditionString("a.VIRTUAL_PALLET_NO", palletNo.Split(new char[] { ',', '\n', '#' }));
                            sBuilder.AppendFormat(palletNos);
                        }
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_START"))
                    {
                        string shipmentDate = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_START"]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_DATE >='{0}'", shipmentDate.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_END"))
                    {
                        string shipmentDate = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_END"]);
                        sBuilder.AppendFormat(" AND b.SHIPMENT_DATE<='{0}'", shipmentDate.PreventSQLInjection());
                    }
                    if (htParams.ContainsKey(WMS_SHIPMENT_FIELDS.FIELDS_STATUS))
                    {
                        string shipmentStatus = Convert.ToString(htParams[WMS_SHIPMENT_FIELDS.FIELDS_STATUS]);
                        string status = string.Empty;
                        if (shipmentStatus.Equals("已保存未出货"))
                            status = "0";
                        if (shipmentStatus.Equals("已出货"))
                            status = "1";
                        sBuilder.AppendFormat(" AND b.SHIPMENT_FLAG = '{0}'", status.PreventSQLInjection());
                    }

                }
                sBuilder.AppendFormat(@" group by VIRTUAL_PALLET_NO, a.VIRTUAL_PALLET_NO,a.LOT_NUMBER_QTY,a.SAP_NO,a.WORKNUMBER,a.GRADE,
                                            a.CS_DATA_GROUP,a.PRO_ID,a.POWER_LEVEL,g.POWER_LEVEL,a.TOTLE_POWER,E.ARTICNO,
                                            c.WORK_ORDER_KEY,d.SUB_PS_WAY,f.PS_SUB_CODE,P_DTL_MIN,P_DTL_MAX,P_MIN,P_MAX,
                                            b.SHIPMENT_KEY,b.SHIPMENT_NO,b.CONTAINER_NO,b.PALLET_NO,b.CI_NO,b.SHIPMENT_TYPE,
                                            b.QUANTITY,b.SHIPMENT_DATE,b.SHIPMENT_FLAG,b.SHIPMENT_OPERATOR,b.CREATER,b.CREATE_TIME,
                                            b.CREATE_TIMEZONE,b.EDIT_TIME,b.EDITOR,b.EDIT_TIMEZONE,b.IS_FLAG,b.CUSTCHECK,b.ETL_FLAG,b.POWER_RANGE ");
                if (!isPaging)
                {
                    sBuilder.Append(" ORDER BY b.SHIPMENT_NO ASC,b.CONTAINER_NO ASC,b.SHIPMENT_DATE DESC,a.VIRTUAL_PALLET_NO ASC");
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
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
                        db,
                        dsReturn,
                        POR_LOT_FIELDS.DATABASE_TABLE_NAME,
                        "SHIPMENT_NO ASC,CONTAINER_NO ASC,SHIPMENT_DATE DESC,VIRTUAL_PALLET_NO ASC");
                    pconfig.Pages = pages;
                    pconfig.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("ShipmentQuery Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 从BCP数据库中根据指定托盘号导入托盘数据。
        /// </summary>
        /// <param name="palletNo">托盘号。可以使用逗号分隔的多个托盘号。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet ImportPalletDataFromBCP(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string []palletNos = palletNo.Split(new char[] { ',','#','\n'});
                StringBuilder sbMsg = new StringBuilder();
                if (palletNos.Length > 0)
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        dbConn.Open();
                        DbCommand dbCmd = dbConn.CreateCommand();
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.CommandText = "SP_TRANS_PALLET_DATA_FROM_BCP";

                        foreach (string n in palletNos)
                        {
                            try
                            {
                                dbCmd.Parameters.Clear();
                                this.db.AddInParameter(dbCmd, "p_pallet_no", DbType.String, n);
                                this.db.AddInParameter(dbCmd, "p_room_key", DbType.String, DBNull.Value);
                                dbCmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                sbMsg.AppendFormat("{0}:{1}\n", n, ex.Message);
                                continue;
                            }
                        }
                    }
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                LogService.LogError(string.Format("ImportPalletDataFromBCP Error:{0}", ex.Message));
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        #region IShipmentOperationEngine 成员


        public DataSet UpdateConteinerNo(DataSet dssetin)
        {
            const string CONST_UPDATE = @" UPDATE WMS_SHIPMENT SET
                                                 EDITOR = '{0}',EDIT_TIMEZONE = '{1}',CONTAINER_NO = '{2}'
                                                 WHERE SHIPMENT_NO ='{3}' AND PALLET_NO = '{4}'";

            string sqlInsert = string.Empty;

            DataSet dsReturn = new DataSet();
            IList<string> sqlCommandList = new List<string>();

            DataTable dtUsed = dssetin.Tables["WMS_SHIPMENT"];
            DataTable dtHash = dssetin.Tables["HASH"];
            Hashtable hsTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);

            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                foreach (DataRow dr in dtUsed.Rows)
                {
                    string strContainerNo = dr["CONTAINER_NO"].ToString();            
                    string strShipmentNo = dr["SHIPMENT_NO"].ToString();
                    string strPalletNo = dr["PALLET_NO"].ToString();   
                    string sqlDetail = string.Format(CONST_UPDATE,
                                                    hsTable["EDITOR"].ToString(),               //编辑人
                                                    hsTable["EDIT_TIMEZONE"].ToString(),        //编辑时区
                                                    strContainerNo,
                                                    strShipmentNo,
                                                    strPalletNo
                                                    );
                    sqlCommandList.Add(sqlDetail);
                 }
                

                foreach (string sql1 in sqlCommandList)
                {
                    db.ExecuteNonQuery(CommandType.Text, sql1);
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UPDATE Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }

        #endregion



        #region IShipmentOperationEngine 成员

        /// <summary>
        /// 根据出货单号查询出货信息
        /// </summary>
        /// <param name="ShipmentNum">出货单号</param>
        /// <returns>查询结果集</returns>
        public DataSet SelectShipmentInf(string ShipmentNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT DISTINCT SHIPMENT_NO FROM WMS_SHIPMENT WHERE SHIPMENT_NO = '{0}' AND IS_FLAG = 1", ShipmentNum);
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SelectShipmentInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IShipmentOperationEngine 成员

        /// <summary>
        /// 根据出货单号查询出货信息
        /// </summary>
        /// <param name="sNum">出货单号</param>
        /// <returns></returns>
        public DataSet GetShipmentInf(string sNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT 1 AS ROWNUMBER ,A.SHIPMENT_KEY,
	                                               A.SHIPMENT_NO,A.CONTAINER_NO,A.PALLET_NO,A.CI_NO,A.SHIPMENT_TYPE,
	                                               A.QUANTITY,A.SHIPMENT_DATE,A.SHIPMENT_OPERATOR,A.CREATER,A.CREATE_TIME,
	                                               A.CREATE_TIMEZONE,A.EDIT_TIMEZONE,A.EDITOR,A.EDIT_TIME,A.POWER_RANGE,
	                                               A.IS_FLAG,A.SHIPMENT_FLAG,B.WORKNUMBER,B.SAP_NO,
                                                   B.POWER_LEVEL,CAST(CUSTCHECK as int) AS CUSTCHECK,A.SHIP_GOTO
                                                FROM WMS_SHIPMENT A 
                                                LEFT JOIN WIP_CONSIGNMENT B ON
                                                   A.PALLET_NO = B.VIRTUAL_PALLET_NO AND A.IS_FLAG = 1
                                                LEFT JOIN POR_WORK_ORDER C ON
                                                   C.ORDER_NUMBER = B.WORKNUMBER AND C.ORDER_STATE IN ('TECO','REL')
                                             WHERE B.CS_DATA_GROUP > 2 AND B.ISFLAG = 1 AND A.IS_FLAG = 1
                                             AND SHIPMENT_NO = '{0}'", sNum);
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShipmentInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IShipmentOperationEngine 成员

        /// <summary>
        /// 修改出货信息状态
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        public bool DeleteShipMentInf(string ShipmentNum,string name)
        {
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                DateTime dt = System.DateTime.Now;
                string sql = string.Format(@"UPDATE WMS_SHIPMENT SET IS_FLAG = 0,EDIT_TIME ='{0}',EDITOR = '{1}'
                                                WHERE SHIPMENT_NO = '{2}' AND IS_FLAG = 1",dt,name,ShipmentNum);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();
            }
            catch (Exception ex)
            {
                LogService.LogError("DeleteShipMentInf Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
                return false;
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return true;
        }

        #endregion

        #region IShipmentOperationEngine 成员


        public DataSet PalletShipmentAdd(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null == dsParams
                || !dsParams.Tables.Contains(WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME))
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入的参数错误。");
                return dsReturn;
            }
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();

                DataTable dtShipment = dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME];
                //检查是否存在重复的托盘记录。
                var lnq = from item in dtShipment.AsEnumerable()
                          group item by item[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复托盘记录，请检查。");
                    return dsReturn;
                }
                WMS_SHIPMENT_FIELDS shipmentFields = new WMS_SHIPMENT_FIELDS();
                if (dtShipment.Rows.Count > 0)
                {
                    string _cino = dtShipment.Rows[0][WMS_SHIPMENT_FIELDS.FIELDS_CI_NO].ToString();
                    string sql01 = string.Format(@"SELECT * FROM [WMS_SHIPMENT_BASIC] WHERE CI_NO = '{0}' ", _cino);
                    //执行查询。
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql01);
                    string _shipgoto = dtShipment.Rows[0][WMS_SHIPMENT_FIELDS.FIELDS_SHIP_GOTO].ToString();
                    string _creater = dtShipment.Rows[0][WMS_SHIPMENT_FIELDS.FIELDS_CREATER].ToString();
                    if (dsReturn.Tables[0].Rows.Count < 1)
                    {
                        string sql02 = string.Format(@"INSERT INTO [dbo].[WMS_SHIPMENT_BASIC]
                                                               ([CI_NO]
                                                               ,[SHIP_GOTO]
                                                               ,[CREATER])
                                                         VALUES ('{0}','{1}','{2}')",
                                                             _cino,
                                                             _shipgoto,
                                                             _creater);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql02);
                    }
                    else
                    {
                        string sql02 = string.Format(@"update [dbo].[WMS_SHIPMENT_BASIC] set 
                                                               [SHIP_GOTO] = '{0}',[CREATER] = '{1}'
                                                                where CI_NO = '{2}'",
                                                                                    _shipgoto,
                                                                                    _creater, _cino);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql02);
                    }
                }
                //循环出货信息。
                foreach (DataRow dr in dtShipment.Rows)
                {
                    string palletNo = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]);
                    string editor = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_EDITOR]);
                    string editTimeZone = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIMEZONE]);
                    int editCustCheck = Convert.ToInt32(dr[WMS_SHIPMENT_FIELDS.FIELDS_CUSTCHECK]);

                    //新增出货记录
                    //插入一笔工序参数数据。                    
                    dr[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIME] = DBNull.Value;
                    dr[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_KEY] = UtilHelper.GenerateNewKey(0);
                    string sql = DatabaseTable.BuildInsertSqlStatement(shipmentFields, dr, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PalletShipmentAdd Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return dsReturn;
        }

        #endregion

        #region IShipmentOperationEngine 成员

        /// <summary>
        /// 过账修改状态
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet PassShipMentInf(DataSet dsParams,string name)
        {
            DataSet dsReturn = new DataSet();
            if (null == dsParams
                || !dsParams.Tables.Contains(WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME))
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入的参数错误。");
                return dsReturn;
            }
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();

                DataTable dtShipment = dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME];
                //检查是否存在重复的托盘记录。
                var lnq = from item in dtShipment.AsEnumerable()
                          group item by item[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复托盘记录，请检查。");
                    return dsReturn;
                }
                WMS_SHIPMENT_FIELDS shipmentFields = new WMS_SHIPMENT_FIELDS();
                //循环出货信息。
                foreach (DataRow dr in dtShipment.Rows)
                {
                    string palletNo = Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO]);
                    int editCustCheck = Convert.ToInt32(dr[WMS_SHIPMENT_FIELDS.FIELDS_CUSTCHECK]);
                    string sql = string.Format(@"UPDATE WIP_CONSIGNMENT
                                                    SET CS_DATA_GROUP=4,EDITOR='{1}',EDIT_TIME=GETDATE(),OUT_WH='{1}',OUT_WH_TIME=GETDATE()
                                                    WHERE VIRTUAL_PALLET_NO='{0}'",
                                                 palletNo.PreventSQLInjection(),
                                                 name);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //更新托盘中的批次记录为已出货。
                    sql = string.Format(@"UPDATE POR_LOT
                                               SET SHIPPED_FLAG=1,EDITOR='{1}',EDIT_TIME=GETDATE(),ship_goto = '{2}'
                                               WHERE PALLET_NO='{0}'",
                                       palletNo.PreventSQLInjection(),
                                       name,Convert.ToString(dr[WMS_SHIPMENT_FIELDS.FIELDS_SHIP_GOTO]).PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    //更新托盘中的批次记录为已出货。
                    sql = string.Format(@"UPDATE WMS_SHIPMENT
                                               SET SHIPMENT_FLAG=1,EDITOR='{0}',EDIT_TIME=GETDATE()
                                               WHERE PALLET_NO='{1}' AND IS_FLAG = 1",
                                       name,
                                       palletNo.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PassShipMentInf Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return dsReturn;
        }

        #endregion

        #region IShipmentOperationEngine 成员


        public DataSet PalletShipmentAUpdate(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (null == dsParams
                || !dsParams.Tables.Contains(WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME))
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入的参数错误。");
                return dsReturn;
            }
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();

                DataTable dtShipment = dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME];
                //检查是否存在重复的托盘记录。
                var lnq = from item in dtShipment.AsEnumerable()
                          group item by item[WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO] into g
                          where g.Count() > 1
                          select g.Count();
                if (lnq.Count() > 0)
                {
                    dbTran.Rollback();
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "存在重复托盘记录，请检查。");
                    return dsReturn;
                }
                WMS_SHIPMENT_FIELDS shipmentFields = new WMS_SHIPMENT_FIELDS();
                string sql = string.Format(@"UPDATE WMS_SHIPMENT SET IS_FLAG = 0,EDIT_TIME =GETDATE(),EDITOR = '{0}'
                                                WHERE SHIPMENT_NO = '{1}' AND IS_FLAG = 1", dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[0]["SHIPMENT_OPERATOR"], dsParams.Tables[WMS_SHIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[0]["SHIPMENT_NO"]);
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                if (dtShipment.Rows.Count > 0)
                {
                    string _cino = dtShipment.Rows[0][WMS_SHIPMENT_FIELDS.FIELDS_CI_NO].ToString();
                    string sql01 = string.Format(@"SELECT * FROM [WMS_SHIPMENT_BASIC] WHERE CI_NO = '{0}' ", _cino);
                    //执行查询。
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql01);
                    string _shipgoto = dtShipment.Rows[0][WMS_SHIPMENT_FIELDS.FIELDS_SHIP_GOTO].ToString();
                    string _creater = dtShipment.Rows[0][WMS_SHIPMENT_FIELDS.FIELDS_CREATER].ToString();
                    if (dsReturn.Tables[0].Rows.Count < 1)
                    {
                        string sql02 = string.Format(@"INSERT INTO [dbo].[WMS_SHIPMENT_BASIC]
                                                               ([CI_NO]
                                                               ,[SHIP_GOTO]
                                                               ,[CREATER])
                                                         VALUES ('{0}','{1}','{2}')",
                                                             _cino,
                                                             _shipgoto,
                                                             _creater);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql02);
                    }
                    else
                    {
                        string sql02 = string.Format(@"update [dbo].[WMS_SHIPMENT_BASIC] set 
                                                               [SHIP_GOTO] = '{0}',[CREATER] = '{1}'
                                                                where CI_NO = '{2}'",
                                                                                    _shipgoto,
                                                                                    _creater, _cino);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql02);
                    }
                }
                //循环出货信息。
                foreach (DataRow dr in dtShipment.Rows)
                {
                    //新增出货记录
                    //插入一笔工序参数数据。                    
                    dr[WMS_SHIPMENT_FIELDS.FIELDS_EDIT_TIME] = DBNull.Value;
                    dr[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_KEY] = UtilHelper.GenerateNewKey(0);
                    sql = DatabaseTable.BuildInsertSqlStatement(shipmentFields, dr, null);
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                }
                dbTran.Commit();
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("PalletShipmentAUpdate Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return dsReturn;
        }

        #endregion

        #region IShipmentOperationEngine 成员

        /// <summary>
        /// 通过托盘号查询出货清单中是否存在可用记录
        /// </summary>
        /// <param name="palletNo">托盘号</param>
        /// <returns>数据集</returns>
        public DataSet GetShipMentNumByPallet(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT * FROM WMS_SHIPMENT WHERE PALLET_NO = '{0}'
                                                AND IS_FLAG = 1", palletNo);
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShipMentNumByPallet Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IShipmentOperationEngine 成员

        /// <summary>
        /// 根据出货单号进行冲销
        /// </summary>
        /// <param name="shipmentNo">出货单号</param>
        /// <returns></returns>
        public DataSet SterilisationShipment(string shipmentNo)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlUpdatePorLot = @"UPDATE A
                                                SET A.SHIPPED_FLAG = '0'
                                             FROM POR_LOT A
                                                INNER JOIN WMS_SHIPMENT B ON A.PALLET_NO = B.PALLET_NO AND B.IS_FLAG = 1
                                             WHERE B.SHIPMENT_NO = '{0}'";

                string sqlUpdateConsignment = @"UPDATE A
                                                    SET A.CS_DATA_GROUP = '3'
                                                    FROM WIP_CONSIGNMENT A
                                                    INNER JOIN WMS_SHIPMENT B ON A.VIRTUAL_PALLET_NO = B.PALLET_NO AND A.ISFLAG = 1 AND B.IS_FLAG = 1
                                                    WHERE B.SHIPMENT_NO = '{0}'";
                string sqlUpdateShipment = @"UPDATE A
                                                SET A.IS_FLAG = 0
                                                FROM WMS_SHIPMENT A
                                                WHERE A.SHIPMENT_NO = '{0}'
                                                    AND A.IS_FLAG = 1";

                ///sql执行start------------------------------------------------------------------------------------------------------------------
                string strUpdatePorLot = string.Format(sqlUpdatePorLot, shipmentNo);
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdatePorLot);
                string strUpdateConsignment = string.Format(sqlUpdateConsignment, shipmentNo);
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdateConsignment);
                string strUpdateShipment = string.Format(sqlUpdateShipment, shipmentNo);
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdateShipment);
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("SterilisationShipment Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region IShipmentOperationEngine 成员


        public DataSet GetPalletShipInf(DataSet dsPalletShipInf)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                foreach(DataRow dr in dsPalletShipInf.Tables[0].Rows)
                {
                    string sqlCommon = string.Format(@"SELECT DISTINCT A.PNOM,A.VIRTUAL_PALLET_NO FROM (
                                                            SELECT A.VIRTUAL_PALLET_NO,
                                                                   (CASE WHEN D.SUB_PS_WAY = '电流' AND ISNULL(F.PS_SUB_CODE,0)>0 THEN 
                                                                        SUBSTRING(F.POWERLEVEL,CHARINDEX('-',F.POWERLEVEL)+1,1) ELSE '' END) AS PNOM
                                                            FROM WIP_CONSIGNMENT A
                                                            INNER JOIN POR_LOT B ON B.PALLET_NO=A.VIRTUAL_PALLET_NO AND B.DELETED_TERM_FLAG<2
                                                            INNER JOIN WIP_CONSIGNMENT_DETAIL ad ON ad.CONSIGNMENT_KEY=A.CONSIGNMENT_KEY AND ad.LOT_NUMBER=B.LOT_NUMBER
                                                            INNER JOIN WIP_IV_TEST C ON C.LOT_NUM=B.LOT_NUMBER AND C.VC_DEFAULT=1
                                                            LEFT JOIN POR_WO_PRD_PS D ON D.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                                       AND D.PART_NUMBER=B.PART_NUMBER
                                                                                       AND D.PS_CODE = C.VC_TYPE
                                                                                       AND D.PS_SEQ=C.I_IDE
                                                                                       AND D.IS_USED='Y'
                                                            LEFT JOIN POR_WO_PRD_PS_SUB F ON F.WORK_ORDER_KEY=B.WORK_ORDER_KEY
                                                                                       AND F.PART_NUMBER=B.PART_NUMBER
                                                                                       AND F.POWERSET_KEY = D.POWERSET_KEY
                                                                                       AND F.PS_SUB_CODE=C.I_PKID
                                                                                       AND F.IS_USED='Y'
                                                            LEFT JOIN V_ProductGrade K ON K.GRADE_CODE=B.PRO_LEVEL
                                                            WHERE A.ISFLAG='1' 
                                                            AND A.CS_DATA_GROUP>'0'
                                                            AND A.VIRTUAL_PALLET_NO = '{0}'
                                                            AND B.STATUS < 2
                                                            GROUP BY A.VIRTUAL_PALLET_NO,SUB_PS_WAY,PS_SUB_CODE,POWERLEVEL,PMAXSTAB) A", dr["VIRTUAL_PALLET_NO"].ToString());
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                    if (dsReturn != null && dsReturn.Tables[0].Rows.Count > 0)
                    {
                        string power = string.Empty;
                        foreach(DataRow dr01 in dsReturn.Tables[0].Rows)
                        {
                            if(!string.IsNullOrEmpty(dr01["PNOM"].ToString()))
                                power = power + "/" + dr01["PNOM"].ToString();
                        }
                        if (!string.IsNullOrEmpty(power))
                        {
                            dr["POWER_LEVEL"] = dr["POWER_LEVEL"].ToString() + "W-" + power.Substring(1, power.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletShipInf Error: " + ex.Message);
            }
            return dsPalletShipInf;
        }

        #endregion

        public string GetShipMentBasicGOTO(string ciNumber)
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"select ship_goto from dbo.WMS_SHIPMENT_BASIC
                                    WHERE CI_NO='{0}' ",
                                    ciNumber.PreventSQLInjection());
                //执行查询。
                object obj = db.ExecuteScalar(CommandType.Text, sbSql.ToString());
                return Convert.ToString(obj);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetShipMentBasicGOTO Error: " + ex.Message);
                throw ex;
            }
        }

    }
}
