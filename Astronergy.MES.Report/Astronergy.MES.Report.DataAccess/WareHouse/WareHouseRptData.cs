using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 组件入库信息访问类
    /// </summary>
    public class WareHouseRptData : BaseDBAccess
    {
        /// <summary>
        /// 获取指定时间区间的组件入库信息。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="start_time">入库时间。</param>
        /// <param name="end_time">入库时间。</param>
        /// <returns>包含组件入库信息的数据集。</returns>
        public DataSet GetWareHouseData(string workOrderNo,string start_time,string end_time)
        {
            const string storeProcedureName = "SP_RPT_WH_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 360;
                this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_startTime", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_endTime", DbType.String, end_time);     
             
                return this._db.ExecuteDataSet(cmd);
            }
        }
    }
}

