using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 托盘数据访问类
    /// </summary>
    public class PalletDataAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的托盘清单数据。
        /// </summary>
        /// <param name="start_palletTime">包装时间  2012-05-12。</param>
        /// <param name="end_palletTime">包装时间 2012-05-13。</param>
        /// <param name="start_toWHTime">入库时间  2012-05-12。</param>
        /// <param name="end_toWHTime">入库时间 2012-05-13。</param>
        /// <param name="lotNo">批次号。</param>
        /// <param name="palletNo">托盘号。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>包含托盘清单数据的数据集。</returns>
        public DataSet GetPalletListData(
                            string start_palletTime,
                            string end_palletTime,
                            string start_toWHTime,
                            string end_toWHTime,
                            string palletNo,
                            string roomName,
                            string proId,
                            string workOrderNo,
                            string partNumber)
        {
            const string storeProcedureName = "SP_QRY_PALLET_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_start_create_time", DbType.String, start_palletTime);
                this._db.AddInParameter(cmd, "p_end_create_time", DbType.String, end_palletTime);
                this._db.AddInParameter(cmd, "p_start_check_time", DbType.String, string.Empty);
                this._db.AddInParameter(cmd, "p_end_check_time", DbType.String, string.Empty);
                this._db.AddInParameter(cmd, "p_start_towh_time", DbType.String, start_toWHTime);
                this._db.AddInParameter(cmd, "p_end_towh_time", DbType.String, end_toWHTime);
                this._db.AddInParameter(cmd, "p_palletNo", DbType.String, palletNo);
                this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }
    }
}

