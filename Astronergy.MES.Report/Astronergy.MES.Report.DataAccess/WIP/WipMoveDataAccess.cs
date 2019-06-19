using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    ///工序MOVE量数据访问类
    /// </summary>
    public class WipMoveDataAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的工序MOVE量
        /// </summary>
        /// <param name="queryType">查询类型 0：按时间查询 1：按日期查询。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="supplierName">材料厂家名称。</param>
        /// <param name="routeKey">工艺流程主键。</param>
        /// <returns>包含工序MOVE量数据的数据集。</returns>
        public DataSet GetDayWIPMoveData(
                            int queryType,
                            string start_time,
                            string end_time,
                            string roomName,
                            string proId,
                            string workOrderNo,
                            string operationName,
                            string partNumber)
        {
            const string storeProcedureName = "SP_QRY_OPERATION_MOVE_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 0;
                this._db.AddInParameter(cmd, "p_query_type", DbType.Decimal, queryType);
                this._db.AddInParameter(cmd, "p_start_date", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_date", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_operationName", DbType.String, operationName);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        /// <summary>
        /// 获取工序MOVE量明细数据。
        /// </summary>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="trxType">操作明细类型</param>
        /// <param name="shiftName">班别名称</param>
        /// <param name="isHistory">是否是查询历史数据。1：历史数据 0：当前数据</param>
        /// <returns>包含工序产量明细数据的数据集。</returns>
        public DataSet GetMoveDataDetail(
                            string start_time,
                            string end_time,
                            string roomName,
                            string operationName,
                            string proId,
                            string workOrderNo,
                            string trxType,
                            string shiftName,
                            string isHistory,
                            string partNumber)
        {

            string storeProcedureName = "SP_QRY_OPERATION_MOVE_DATA_DTL";
            DataSet ds = new DataSet();
            using (DbConnection con = this._db.CreateConnection())
            {
                //@p_trxType         AS  VARCHAR(20),   --操作类型
                //@p_start_time      AS  DATETIME,      --'2012-05-23 08:00:00'
                //@p_end_time        AS  DATETIME,      --'2012-05-24 08:00:00'
                //@p_roomName        AS  VARCHAR(20),   --车间名称
                //@p_proId           AS  VARCHAR(50),   --产品ID号
                //@p_workorder       AS  VARCHAR(50),   --工单号
                //@p_operationName   AS  VARCHAR(50),   --工序名称
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 0;
                this._db.AddInParameter(cmd, "p_trxType", DbType.String, trxType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workorder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_operationName", DbType.String, operationName);
                this._db.AddInParameter(cmd, "p_shiftName", DbType.String, shiftName);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }
    }
}

