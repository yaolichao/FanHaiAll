using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface.WarehouseManagement;
using FanHai.Hemera.Utils;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 线上仓物料数据查询类。
    /// </summary>
    public class OnlineMaterialQueryEngine : AbstractEngine, IOnlineMaterialQueryEngine
    {
        private Database db = null;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OnlineMaterialQueryEngine()
        {
            db = DatabaseFactory.CreateDatabase();//实例化对象
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
        }
        /// <summary>
        /// 查询在线物料。
        /// </summary>
        /// <param name="model">查询条件。</param>
        /// <param name="pconfig">分页对象。</param>
        /// <returns>包含在线物料信息的数据集对象。</returns>
        public DataSet Query(OnlineMaterialQueryModel model, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append(@"SELECT A.STORE_MATERIAL_KEY ,
                                  SUM(A.CURRENT_QTY)AS QTY,
                                  C.STORE_NAME AS STORE_NAME,
                                  D.MATERIAL_CODE AS  MATERIAL_CODE,
                                  D.MATERIAL_NAME AS  MATERIAL_NAME,
                                  D.UNIT AS  UNIT, 
                                  E.LOCATION_NAME AS ROOM_NAME,
                                  C.OPERATION_NAME AS  OPERATION_NAME  
                            FROM WST_STORE_MATERIAL_DETAIL A 
                            LEFT JOIN WST_STORE_MATERIAL B ON A.STORE_MATERIAL_KEY=B.STORE_MATERIAL_KEY
                            LEFT JOIN WST_STORE C ON B.STORE_KEY=C.STORE_KEY
                            LEFT JOIN POR_MATERIAL D ON B.MATERIAL_KEY=D.MATERIAL_KEY
                            LEFT JOIN FMM_LOCATION E ON C.LOCATION_KEY=E.LOCATION_KEY
                            WHERE CURRENT_QTY>0 ");
                if (!string.IsNullOrEmpty(model.OperationName))
                {
                    sbSql.AppendFormat(UtilHelper.BuilderWhereConditionString("C.OPERATION_NAME", model.OperationName.Split(',')));
                }

                if (!string.IsNullOrEmpty(model.StoreName))
                {
                    sbSql.AppendFormat(UtilHelper.BuilderWhereConditionString("C.STORE_NAME", model.StoreName.Split(',')));
                }

                if (!string.IsNullOrEmpty(model.MaterialCode))
                {
                    sbSql.AppendFormat(" AND D.MATERIAL_CODE  LIKE '{0}%'", model.MaterialCode.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(model.MaterialLot))
                {
                    sbSql.AppendFormat(" AND A.MATERIAL_LOT  LIKE '{0}%'", model.MaterialLot.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(model.RoomName))
                {
                    sbSql.AppendFormat(" AND E.LOCATION_NAME  LIKE '{0}%'", model.RoomName.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(model.SupplierName))
                {
                    sbSql.AppendFormat(" AND A.MATERIAL_SUPPLIER  LIKE '{0}%'", model.SupplierName.PreventSQLInjection());
                }

                sbSql.Append(@" GROUP BY A.STORE_MATERIAL_KEY,C.STORE_NAME,D.MATERIAL_CODE,E.LOCATION_NAME,
                               D.MATERIAL_NAME,D.UNIT, E.LOCATION_NAME,C.OPERATION_NAME ");
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sbSql.ToString(), 
                                                    pconfig.PageNo, 
                                                    pconfig.PageSize, 
                                                    out pages,
                                                    out records, 
                                                    db, 
                                                    dsReturn, 
                                                    "WST_STORE_MATERIAL",
                                                    "MATERIAL_CODE ASC");
                pconfig.Pages = pages;
                pconfig.Records = records;
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchLotList Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询在线物料明细信息。
        /// </summary>
        /// <param name="model">
        /// 包含查询条件对象。
        /// </param>
        /// <param name="storeMaterialKey">
        /// 在线物料主键
        /// </param>
        /// <returns>包含在线物料信息的数据集。</returns>
        public DataSet QueryDetail(OnlineMaterialQueryModel model, string storeMaterialKey)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append(@"SELECT A.STORE_MATERIAL_KEY,
                                     A.MATERIAL_LOT,
                                     A.CURRENT_QTY,
                                     A.MATERIAL_SUPPLIER,
                                     D.UNIT
                            FROM WST_STORE_MATERIAL_DETAIL A 
                            LEFT JOIN WST_STORE_MATERIAL B ON A.STORE_MATERIAL_KEY=B.STORE_MATERIAL_KEY
                            LEFT JOIN WST_STORE C ON B.STORE_KEY=C.STORE_KEY
                            LEFT JOIN POR_MATERIAL D ON B.MATERIAL_KEY=D.MATERIAL_KEY
                            LEFT JOIN FMM_LOCATION E ON C.LOCATION_KEY=E.LOCATION_KEY
                            WHERE CURRENT_QTY>0 ");
                if (!string.IsNullOrEmpty(model.OperationName))
                {
                    sbSql.AppendFormat(UtilHelper.BuilderWhereConditionString("C.OPERATION_NAME", model.OperationName.Split(',')));
                }

                if (!string.IsNullOrEmpty(model.StoreName))
                {
                    sbSql.AppendFormat(UtilHelper.BuilderWhereConditionString("C.STORE_NAME", model.StoreName.Split(',')));
                }

                if (!string.IsNullOrEmpty(model.MaterialCode))
                {
                    sbSql.AppendFormat(" AND D.MATERIAL_CODE  LIKE '{0}%'", model.MaterialCode.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(model.MaterialLot))
                {
                    sbSql.AppendFormat(" AND A.MATERIAL_LOT  LIKE '{0}%'", model.MaterialLot.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(model.RoomName))
                {
                    sbSql.AppendFormat(" AND E.LOCATION_NAME  LIKE '{0}%'", model.RoomName.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(model.SupplierName))
                {
                    sbSql.AppendFormat(" AND A.MATERIAL_SUPPLIER  LIKE '{0}%'", model.SupplierName.PreventSQLInjection());
                }
                sbSql.AppendFormat(" AND A.STORE_MATERIAL_KEY = '{0}'", storeMaterialKey.PreventSQLInjection());
                dsReturn = this.db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchLotList Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
