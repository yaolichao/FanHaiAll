using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 直通率数据访问类
    /// </summary>
    public class PassYieldDataAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的直通率数据。
        /// </summary>
        /// <param name="queryType">查询方式 0：按时间范围查询 1：By日期查询。2：By日期查询最后一条记录。</param>
        /// <param name="roomKey">车间主键。多个车间用逗号分开。</param>
        /// <param name="customer">客户。多个客户用逗号分开。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。多个产品型号用逗号分开。</param>
        /// <param name="proId">产品ID号。多个产品ID号用逗号分开。</param>
        /// <param name="workOrderNo">工单号。多个工单号用逗号分开。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含直通率数据的数据集。</returns>
        public DataSet Get(int queryType,
                            string start_time,
                            string end_time,
                            string roomKey,
                            string customer,
                            string productModel,
                            string proId,
                            string workOrderNo,
                            string partNumber
                              )
        {
            const string storeProcedureName = "SP_QRY_PASS_YIELD_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_query_type", DbType.String, queryType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        public DataSet GetThrough(int queryType,
                            string start_time,
                            string end_time,
                            string roomKey,
                            string customer,
                            string productModel,
                            string proId,
                            string workOrderNo,
                            string partNumber,
                            string oprline
                              )
        {
            const string storeProcedureName = "SP_QRY_PASS_YIELD_DATA_SH";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_query_type", DbType.String, queryType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                this._db.AddInParameter(cmd, "p_oprline", DbType.String, oprline);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        //public DataSet GetThroughLine(int queryType,
        //                   string start_time,
        //                   string end_time,
        //                   string roomKey,
        //                   string customer,
        //                   string productModel,
        //                   string proId,
        //                   string workOrderNo,
        //                   string partNumber,
        //                   string oprline
        //                     )
        //{
        //    const string storeProcedureName = "SP_QRY_PASS_YIELD_DATA_SH";
        //    using (DbConnection con = this._db.CreateConnection())
        //    {
        //        DbCommand cmd = con.CreateCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = storeProcedureName;
        //        this._db.AddInParameter(cmd, "p_query_type", DbType.String, queryType);
        //        this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
        //        this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
        //        this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
        //        this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
        //        this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
        //        this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
        //        this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
        //        this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
        //        return this._db.ExecuteDataSet(cmd);
        //    }
        //}

        /// <summary>
        /// 获取指定日期区间的直通率明细数据。
        /// </summary>
        /// <param name="dataType">查询的明细数据类型。</param>
        /// <param name="roomKey">车间主键。多个车间用逗号分开。</param>
        /// <param name="customer">客户。多个客户用逗号分开。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。多个产品型号用逗号分开。</param>
        /// <param name="proId">产品ID号。多个产品ID号用逗号分开。</param>
        /// <param name="workOrderNo">工单号。多个工单号用逗号分开。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含直通率明细数据的数据集。</returns>
        public DataSet GetDetail(string dataType,
                                string start_time,
                                string end_time,
                                string roomKey,
                                string customer,
                                string productModel,
                                string proId,
                                string workOrderNo,
                                string partNumber)
        {
            const string storeProcedureName = "SP_QRY_PASS_YIELD_DATA_DTL";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_data_type", DbType.String, dataType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        public DataSet GetDetailLine(string dataType,
                               string start_time,
                               string end_time,
                               string roomKey,
                               string customer,
                               string productModel,
                               string proId,
                               string workOrderNo,
                               string partNumber,
                               string oprline
                                 )
        {
            const string storeProcedureName = "SP_QRY_PASS_YIELD_DATA_DTL_SH";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_data_type", DbType.String, dataType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                this._db.AddInParameter(cmd, "p_oprline", DbType.String, oprline);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        /// <summary>
        /// 查询直通率数据之后进行数据的行列转换。
        /// </summary>
        /// <param name="dsSource">源数据表。</param>
        /// <param name="queryType">查询方式 0：按时间范围查询 1：By日期查询。2：By日期查询最后一条记录。</param>
        /// <param name="dic"></param>
        /// <returns>行列转换后的数据表。</returns>
        public DataTable TransferDatatable(DataTable dtSource, int queryType, Dictionary<string, string> dic)
        {
            DataTable dtBase = dtSource.DefaultView.ToTable(true, new string[] { "PROMODEL_NAME" });
            //组织新表数据结构。
            DataTable dtNew = new DataTable();
            for (int i = 0; i < dtBase.Columns.Count; i++)
            {
                dtNew.Columns.Add(dtBase.Columns[i].ColumnName);
            }
            dtNew.Columns.Add("COL_VALUE");
            IEnumerable<string> dateColNames = dtSource.AsEnumerable()
                                              .Select(row => Convert.ToString(row["DATA_DATE"]))
                                              .Distinct();
            foreach (string colName in dateColNames)
            {
                DataColumn dc = dtNew.Columns.Add(colName);
                dc.DataType = typeof(double);
            }
            dtNew.Columns.Add("KEY_VALUE");
            //填充数据
            for (int i = 0; i < dtBase.Rows.Count; i++)
            {
                string proModelName = Convert.ToString(dtBase.Rows[i]["PROMODEL_NAME"]);
                //添加数据行
                foreach (string key in dic.Keys)
                {
                    //按时间段查询不显示累计值。
                    if (key.StartsWith("LJ_") && (queryType == 0 || queryType==3))
                    {
                        continue;
                    }
                    DataRow drNew = dtNew.NewRow();
                    drNew["KEY_VALUE"] =string.Format("{0}${1}",proModelName,key);
                    drNew["PROMODEL_NAME"] = proModelName;
                    drNew["COL_VALUE"] = key;
                    for (int j = dtBase.Columns.Count + 1; j < dtNew.Columns.Count-1; j++)
                    {
                        string dataDate = dtNew.Columns[j].ColumnName;
                        string filter = string.Format("PROMODEL_NAME='{0}' AND DATA_DATE='{1}'", proModelName, dataDate);
                        DataRow[] drs = dtSource.Select(filter);
                        if (drs.Length > 0)
                        {
                            //CY_TRACKOUT_QTY,CY_REWORK_QTY,CY_REWORK_1_QTY
                            //ZK_TRACKOUT_QTY,ZK_REWORK_QTY,ZK_REWORK_1_QTY
                            //QJ_TRACKOUT_QTY,QJ_REWORK_QTY,QJ_REWORK_1_QTY
                            //ZJCS_TRACKOUT_QTY,ZJCS_REWORK_QTY,ZJCS_REWORK_1_QTY
                            //ZJ_TRACKOUT_QTY,ZJ_REWORK_QTY,ZJ_REWORK_1_QTY
                            double cyTrackoutQty = Convert.ToDouble(drs[0]["CY_TRACKOUT_QTY"]);
                            double cyReworkQty = Convert.ToDouble(drs[0]["CY_REWORK_QTY"]);
                            double cyRework1Qty = Convert.ToDouble(drs[0]["CY_REWORK_1_QTY"]);
                            
                            double zkTrackoutQty = Convert.ToDouble(drs[0]["ZK_TRACKOUT_QTY"]);
                            double zkReworkQty = Convert.ToDouble(drs[0]["ZK_REWORK_QTY"]);
                            double zkRework1Qty = Convert.ToDouble(drs[0]["ZK_REWORK_1_QTY"]);

                            double qjTrackoutQty = Convert.ToDouble(drs[0]["QJ_TRACKOUT_QTY"]);
                            double qjReworkQty = Convert.ToDouble(drs[0]["QJ_REWORK_QTY"]);
                            double qjRework1Qty = Convert.ToDouble(drs[0]["QJ_REWORK_1_QTY"]);

                            double zjcsTrackoutQty = Convert.ToDouble(drs[0]["ZJCS_TRACKOUT_QTY"]);
                            double zjcsReworkQty = Convert.ToDouble(drs[0]["ZJCS_REWORK_QTY"]);
                            double zjcsRework1Qty = Convert.ToDouble(drs[0]["ZJCS_REWORK_1_QTY"]);

                            double zjTrackoutQty = Convert.ToDouble(drs[0]["ZJ_TRACKOUT_QTY"]);
                            double zjReworkQty = Convert.ToDouble(drs[0]["ZJ_REWORK_QTY"]);
                            double zjRework1Qty = Convert.ToDouble(drs[0]["ZJ_REWORK_1_QTY"]);

                            if (cyTrackoutQty + zkTrackoutQty + qjTrackoutQty + zjcsTrackoutQty + zjTrackoutQty > 0)
                            {
                                if (key == "CY_TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = cyTrackoutQty;
                                }
                                else if (key == "CY_REWORK_QTY")
                                {
                                    drNew[dataDate] = cyReworkQty;
                                }
                                else if (key == "CY_REWORK_1_QTY")
                                {
                                    drNew[dataDate] = cyRework1Qty;
                                }
                                else if (key == "ZK_TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = zkTrackoutQty;
                                }
                                else if (key == "ZK_REWORK_QTY")
                                {
                                    drNew[dataDate] = zkReworkQty;
                                }
                                else if (key == "ZK_REWORK_1_QTY")
                                {
                                    drNew[dataDate] = zkRework1Qty;
                                }
                                else if (key == "QJ_TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = qjTrackoutQty;
                                }
                                else if (key == "QJ_REWORK_QTY")
                                {
                                    drNew[dataDate] = qjReworkQty;
                                }
                                else if (key == "QJ_REWORK_1_QTY")
                                {
                                    drNew[dataDate] = qjRework1Qty;
                                }
                                else if (key == "ZJCS_TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = zjcsTrackoutQty;
                                }
                                else if (key == "ZJCS_REWORK_QTY")
                                {
                                    drNew[dataDate] = zjcsReworkQty;
                                }
                                else if (key == "ZJCS_REWORK_1_QTY")
                                {
                                    drNew[dataDate] = zjcsRework1Qty;
                                } 
                                else if (key == "ZJ_TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = zjTrackoutQty;
                                }
                                else if (key == "ZJ_REWORK_QTY")
                                {
                                    drNew[dataDate] = zjReworkQty;
                                }
                                else if (key == "ZJ_REWORK_1_QTY")
                                {
                                    drNew[dataDate] = zjRework1Qty;
                                }
                                //从层压开始到终检的5个工段的良率(1-（返修+返工+报废）的数量/本站总产出数量）相乘获得直通率
                                //返工数量不属于良品的直通率
                                else if (key == "PASS_YIELD_0_RATE")
                                {
                                    double cy = cyTrackoutQty == 0 ? 1 : (1 - (cyReworkQty + cyRework1Qty) / cyTrackoutQty);
                                    double zk = zkTrackoutQty == 0 ? 1 : (1 - (zkReworkQty + zkRework1Qty) / zkTrackoutQty);
                                    double qj = qjTrackoutQty == 0 ? 1 : (1 - (qjReworkQty + qjRework1Qty) / qjTrackoutQty);
                                    double zjcs = zjcsTrackoutQty == 0 ? 1 : (1 - (zjcsReworkQty + zjcsRework1Qty) / zjcsTrackoutQty);
                                    double zj = zjTrackoutQty == 0 ? 1 : (1 - (zjReworkQty + zjRework1Qty) / zjTrackoutQty);
                                    drNew[dataDate] = cy*zk*qj*zjcs*zj;
                                }
                                //返工数量属于良品的直通率
                                else if (key == "PASS_YIELD_1_RATE")
                                {
                                    double cy = cyTrackoutQty == 0 ? 1 : (1 - (cyReworkQty) / cyTrackoutQty);
                                    double zk = zkTrackoutQty == 0 ? 1 : (1 - (zkReworkQty) / zkTrackoutQty);
                                    double qj = qjTrackoutQty == 0 ? 1 : (1 - (qjReworkQty) / qjTrackoutQty);
                                    double zjcs = zjcsTrackoutQty == 0 ? 1 : (1 - (zjcsReworkQty) / zjcsTrackoutQty);
                                    double zj = zjTrackoutQty == 0 ? 1 : (1 - (zjReworkQty) / zjTrackoutQty);
                                    drNew[dataDate] = cy * zk * qj * zjcs * zj;
                                }
                            }
                            //LJ_CY_TRACKOUT_QTY,LJ_CY_REWORK_QTY,LJ_CY_REWORK_1_QTY
                            //LJ_ZK_TRACKOUT_QTY,LJ_ZK_REWORK_QTY,LJ_ZK_REWORK_1_QTY
                            //LJ_QJ_TRACKOUT_QTY,LJ_QJ_REWORK_QTY,LJ_QJ_REWORK_1_QTY
                            //LJ_ZJCS_TRACKOUT_QTY,LJ_ZJCS_REWORK_QTY,LJ_ZJCS_REWORK_1_QTY
                            //LJ_ZJ_TRACKOUT_QTY,LJ_ZJ_REWORK_QTY,LJ_ZJ_REWORK_1_QTY
                            if (queryType == 1 || queryType==2)
                            {
                                double ljCyTrackoutQty = Convert.ToDouble(drs[0]["LJ_CY_TRACKOUT_QTY"]);
                                double ljCyReworkQty = Convert.ToDouble(drs[0]["LJ_CY_REWORK_QTY"]);
                                double ljCyRework1Qty = Convert.ToDouble(drs[0]["LJ_CY_REWORK_1_QTY"]);

                                double ljZkTrackoutQty = Convert.ToDouble(drs[0]["LJ_ZK_TRACKOUT_QTY"]);
                                double ljZkReworkQty = Convert.ToDouble(drs[0]["LJ_ZK_REWORK_QTY"]);
                                double ljZkRework1Qty = Convert.ToDouble(drs[0]["LJ_ZK_REWORK_1_QTY"]);

                                double ljQjTrackoutQty = Convert.ToDouble(drs[0]["LJ_QJ_TRACKOUT_QTY"]);
                                double ljQjReworkQty = Convert.ToDouble(drs[0]["LJ_QJ_REWORK_QTY"]);
                                double ljQjRework1Qty = Convert.ToDouble(drs[0]["LJ_QJ_REWORK_1_QTY"]);

                                double ljZjcsTrackoutQty = Convert.ToDouble(drs[0]["LJ_ZJCS_TRACKOUT_QTY"]);
                                double ljZjcsReworkQty = Convert.ToDouble(drs[0]["LJ_ZJCS_REWORK_QTY"]);
                                double ljZjcsRework1Qty = Convert.ToDouble(drs[0]["LJ_ZJCS_REWORK_1_QTY"]);

                                double ljZjTrackoutQty = Convert.ToDouble(drs[0]["LJ_ZJ_TRACKOUT_QTY"]);
                                double ljZjReworkQty = Convert.ToDouble(drs[0]["LJ_ZJ_REWORK_QTY"]);
                                double ljZjRework1Qty = Convert.ToDouble(drs[0]["LJ_ZJ_REWORK_1_QTY"]);

                                if (ljCyTrackoutQty + ljZkTrackoutQty + ljQjTrackoutQty + ljZjcsTrackoutQty + ljZjTrackoutQty > 0)
                                {
                                    if (key == "LJ_CY_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljCyTrackoutQty;
                                    }
                                    else if (key == "LJ_CY_REWORK_QTY")
                                    {
                                        drNew[dataDate] = ljCyReworkQty;
                                    }
                                    else if (key == "LJ_CY_REWORK_1_QTY")
                                    {
                                        drNew[dataDate] = ljCyRework1Qty;
                                    }
                                    else if (key == "LJ_ZK_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljZkTrackoutQty;
                                    }
                                    else if (key == "LJ_ZK_REWORK_QTY")
                                    {
                                        drNew[dataDate] = ljZkReworkQty;
                                    }
                                    else if (key == "LJ_ZK_REWORK_1_QTY")
                                    {
                                        drNew[dataDate] = ljZkRework1Qty;
                                    }
                                    else if (key == "LJ_QJ_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljQjTrackoutQty;
                                    }
                                    else if (key == "LJ_QJ_REWORK_QTY")
                                    {
                                        drNew[dataDate] = ljQjReworkQty;
                                    }
                                    else if (key == "LJ_QJ_REWORK_1_QTY")
                                    {
                                        drNew[dataDate] = ljQjRework1Qty;
                                    }
                                    else if (key == "LJ_ZJCS_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljZjcsTrackoutQty;
                                    }
                                    else if (key == "LJ_ZJCS_REWORK_QTY")
                                    {
                                        drNew[dataDate] = ljZjcsReworkQty;
                                    }
                                    else if (key == "LJ_ZJCS_REWORK_1_QTY")
                                    {
                                        drNew[dataDate] = ljZjcsRework1Qty;
                                    }
                                    else if (key == "LJ_ZJ_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljZjTrackoutQty;
                                    }
                                    else if (key == "LJ_ZJ_REWORK_QTY")
                                    {
                                        drNew[dataDate] = ljZjReworkQty;
                                    }
                                    else if (key == "LJ_ZJ_REWORK_1_QTY")
                                    {
                                        drNew[dataDate] = ljZjRework1Qty;
                                    }
                                    //返工数量不属于良品的直通率
                                    else if (key == "LJ_PASS_YIELD_0_RATE")
                                    {
                                        double cy = ljCyTrackoutQty == 0 ? 1 : (1 - (ljCyReworkQty + ljCyRework1Qty) / ljCyTrackoutQty);
                                        double zk = ljZkTrackoutQty == 0 ? 1 : (1 - (ljZkReworkQty + ljZkRework1Qty) / ljZkTrackoutQty);
                                        double qj = ljQjTrackoutQty == 0 ? 1 : (1 - (ljQjReworkQty + ljQjRework1Qty) / ljQjTrackoutQty);
                                        double zjcs = ljZjcsTrackoutQty == 0 ? 1 : (1 - (ljZjcsReworkQty + ljZjcsRework1Qty) / ljZjcsTrackoutQty);
                                        double zj = ljZjTrackoutQty == 0 ? 1 : (1 - (ljZjReworkQty + ljZjRework1Qty) / ljZjTrackoutQty);
                                        drNew[dataDate] = cy * zk * qj * zjcs * zj;
                                    }
                                    //返工数量属于良品的直通率
                                    else if (key == "LJ_PASS_YIELD_1_RATE")
                                    {
                                        double cy = ljCyTrackoutQty == 0 ? 1 : (1 - (ljCyReworkQty) / ljCyTrackoutQty);
                                        double zk = ljZkTrackoutQty == 0 ? 1 : (1 - (ljZkReworkQty) / ljZkTrackoutQty);
                                        double qj = ljQjTrackoutQty == 0 ? 1 : (1 - (ljQjReworkQty) / ljQjTrackoutQty);
                                        double zjcs = ljZjcsTrackoutQty == 0 ? 1 : (1 - (ljZjcsReworkQty) / ljZjcsTrackoutQty);
                                        double zj = ljZjTrackoutQty == 0 ? 1 : (1 - (ljZjReworkQty) / ljZjTrackoutQty);
                                        drNew[dataDate] = cy * zk * qj * zjcs * zj;
                                    }
                                }
                            }
                        }
                    }
                    dtNew.Rows.Add(drNew);
                }
            }
            return dtNew;
        }
    }

}

