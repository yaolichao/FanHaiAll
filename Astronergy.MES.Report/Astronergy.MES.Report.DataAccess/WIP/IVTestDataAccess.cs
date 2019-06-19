using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 批次IV测试数据访问类
    /// </summary>
    public class IVTestDataAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的IV测试数据。
        /// </summary>
        /// <param name="start_time">测试时间  2012-05-12。</param>
        /// <param name="end_time">测试时间 2012-05-13。</param>
        /// <param name="valid">有效值标识。</param>
        /// <param name="lotNo">批次号。</param>
        /// <param name="testMachineNo">测试机台号。</param>
        /// <param name="palletNo">托盘号。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>包含IV测试数据的数据集。</returns>
        public DataSet GetIVTestData(
                            string start_time,
                            string end_time,
                            string valid,
                            string lotNo,
                            string testMachineNo,
                            string palletNo,
                            string roomName,
                            string proId,
                            string workOrderNo,
                            string partNumber)
        {
            const string storeProcedureName = "SP_QRY_IV_TEST_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 360;
                this._db.AddInParameter(cmd, "p_start_test_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_test_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_valid", DbType.String, valid);
                this._db.AddInParameter(cmd, "p_lotNo", DbType.String, lotNo);
                this._db.AddInParameter(cmd, "p_testMachineNo", DbType.String, testMachineNo);
                this._db.AddInParameter(cmd, "p_palletNo", DbType.String, palletNo);
                this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }
        public DataSet GetIVTestData(string barcode)
        {
            using (DbConnection con = this._dbNow.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM WIP_IV_TEST WHERE LOT_NUM='{0}'", barcode);
                //this._dbNow.AddInParameter(cmd, "LOT_NUM", DbType.String, barcode);
                return this._dbNow.ExecuteDataSet(cmd);
            }
        }
    }
}

