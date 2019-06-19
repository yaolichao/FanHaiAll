using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 批次数据访问类
    /// </summary>
    public class LotData4LYGAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的批次清单数据。
        /// </summary>
        /// <param name="start_time">创批时间  2012-05-12。</param>
        /// <param name="end_time">创批时间 2012-05-13。</param>
        /// <param name="start_toWHTime">入库时间  2012-05-12。</param>
        /// <param name="end_toWHTime">入库时间 2012-05-13。</param>
        /// <param name="lotNo">批次号。</param>
        /// <param name="palletNo">托盘号。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>包含批次数据的数据集。</returns>
        public DataSet GetLotListData(
                            string start_time,
                            string end_time,
                            string start_toWHTime,
                            string end_toWHTime,
                            string lotNo,
                            string palletNo,
                            string roomName,
                            string proId,
                            string workOrderNo,
                            string partNumber)
        {
            const string storeProcedureName = "SP_QRY_LOTDATA4LYG";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_start_create_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_create_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_start_towh_time", DbType.String, start_toWHTime);
                this._db.AddInParameter(cmd, "p_end_towh_time", DbType.String, end_toWHTime);
                this._db.AddInParameter(cmd, "p_lotNo", DbType.String, lotNo);
                this._db.AddInParameter(cmd, "p_palletNo", DbType.String, palletNo);
                this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        /// <summary>
        /// 获取指定日期区间的创建的批次清单数据。
        /// </summary>
        /// <param name="start_time">创批时间  2012-05-12。</param>
        /// <param name="end_time">创批时间 2012-05-13。</param>
        /// <param name="lotNo">批次号。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>包含符合条件的批次数据的数据集。</returns>
        public DataSet GetCreateLotListData(
                            string start_create_time,
                            string end_create_time,
                            string lotNo,
                            string roomName,
                            string proId,
                            string workOrderNo,
                            string partNumber)
        {
            const string storeProcedureName = "SP_QRY_CREATE_LOT_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_start_create_time", DbType.String, start_create_time);
                this._db.AddInParameter(cmd, "p_end_create_time", DbType.String, end_create_time);
                this._db.AddInParameter(cmd, "p_lotNo", DbType.String, lotNo);
                this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        /// <summary>
        /// 获取批次数据明细。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="type">0 基本信息 1 加工历史 2 工序参数 3 报废不良</param>
        /// <returns>包含批次数据明细的数据集对象。</returns>
        public DataSet GetLotDetail(string lotKey,int type)
        {
            const string storeProcedureName = "SP_QRY_LOTDATADETAIL4LYG";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_lotKey", DbType.String, lotKey);
                this._db.AddInParameter(cmd, "p_type", DbType.Int32, type);
                return this._db.ExecuteDataSet(cmd);
            }
        }
        /// <summary>
        /// 获取二联关系。
        /// </summary>
        /// <returns>二联的组合关系</returns>
        public DataSet GetDoubleRelationship()
        {

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM V_SE_DOUBLE_PARAM_MATERIAL";
                return this._db.ExecuteDataSet(cmd);
            }
        }

        /// <summary>
        /// 获取三联关系。
        /// </summary>
        /// <returns>三联的组合关系</returns>
        public DataSet GetTripleRelationship()
        {

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM V_SE_TRIPLE_PARAM_MATERIAL";
                return this._db.ExecuteDataSet(cmd);
            }
        }

    }
}

