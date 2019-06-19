using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// EL不良率数据访问类
    /// </summary>
    public class ELDefectYieldDataAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的EL不良率数据。
        /// </summary>
        /// <param name="queryType">查询方式 0：按时间范围查询 1：By日期查询。2：By日期查询最后一条记录。</param>
        /// <param name="roomKey">车间主键。多个车间用逗号分开。</param>
        /// <param name="customer">客户。多个客户用逗号分开。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。多个产品型号用逗号分开。</param>
        /// <param name="proId">产品ID号。多个产品ID号用逗号分开。</param>
        /// <param name="workOrderNo">工单号。多个工单号用逗号分开。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含EL不良率数据的数据集。</returns>
        public DataSet Get(int queryType,
                            string start_time,
                            string end_time,
                            string roomKey,
                            string customer,
                            string productModel,
                            string proId,
                            string workOrderNo,
                            string partNumber)
        {
            const string storeProcedureName = "SP_QRY_EL_DEFECT_DATA";
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

        /// <summary>
        /// 获取指定日期区间的EL不良率明细数据。
        /// </summary>
        /// <param name="dataType">查询的明细数据类型。</param>
        /// <param name="roomKey">车间主键。多个车间用逗号分开。</param>
        /// <param name="customer">客户。多个客户用逗号分开。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。多个产品型号用逗号分开。</param>
        /// <param name="proId">产品ID号。多个产品ID号用逗号分开。</param>
        /// <param name="workOrderNo">工单号。多个工单号用逗号分开。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含EL不良率明细数据的数据集。</returns>
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
            const string storeProcedureName = "SP_QRY_EL_DEFECT_DATA_DTL";
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

        /// <summary>
        /// 查询EL不良率数据之后进行数据的行列转换。
        /// </summary>
        /// <param name="dsSource">源数据表。</param>
        /// <param name="queryType">查询方式 0：按时间范围查询 1：By日期查询。2：By日期查询最后一条记录。</param>
        /// <param name="dic"></param>
        /// <returns>行列转换后的数据表。</returns>
        public DataTable TransferDatatable(DataTable dtSource, int queryType, Dictionary<string, string> dic)
        {
            DataTable dtBase = dtSource.DefaultView.ToTable(true, new string[] { "ROOM_KEY","ROOM_NAME" });
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
                string roomKey = Convert.ToString(dtBase.Rows[i]["ROOM_KEY"]);
                string roomName = Convert.ToString(dtBase.Rows[i]["ROOM_NAME"]);
                //添加数据行
                foreach (string key in dic.Keys)
                {
                    //按时间段查询不显示累计值。
                    if (key.StartsWith("LJ_") && queryType == 0)
                    {
                        continue;
                    }
                    DataRow drNew = dtNew.NewRow();
                    drNew["KEY_VALUE"] =string.Format("{0}${1}",roomKey,key);
                    drNew["ROOM_KEY"] = roomKey;
                    drNew["ROOM_NAME"] = roomName;
                    drNew["COL_VALUE"] = key;
                    for (int j = dtBase.Columns.Count + 1; j < dtNew.Columns.Count-1; j++)
                    {
                        string dataDate = dtNew.Columns[j].ColumnName;
                        string filter = string.Format("ROOM_NAME='{0}' AND DATA_DATE='{1}'", roomName, dataDate);
                        DataRow[] drs = dtSource.Select(filter);
                        if (drs.Length > 0)
                        {
                            //FS_QTY,ZJ_QTY,FS_DEFECT_QTY,ZJ_DEFECT_QTY
                            double fsQty = Convert.ToDouble(drs[0]["FS_QTY"]);
                            double fsDefectQty = Convert.ToDouble(drs[0]["FS_DEFECT_QTY"]);

                            double zjQty = Convert.ToDouble(drs[0]["ZJ_QTY"]);
                            double zjDefectQty = Convert.ToDouble(drs[0]["ZJ_DEFECT_QTY"]);


                            if (fsQty + zjQty > 0)
                            {
                                if (key == "FS_QTY")
                                {
                                    drNew[dataDate] = fsQty;
                                }
                                else if (key == "FS_DEFECT_QTY")
                                {
                                    drNew[dataDate] = fsDefectQty;
                                }
                                else if (key == "ZJ_QTY")
                                {
                                    drNew[dataDate] = zjQty;
                                }
                                else if (key == "ZJ_DEFECT_QTY")
                                {
                                    drNew[dataDate] = zjDefectQty;
                                }
                                //成品EL不良率 终检判定不良数/终检检验量
                                else if (key == "ZJ_DEFECT_RATE" && zjQty > 0)
                                {
                                    drNew[dataDate] = zjDefectQty/zjQty;
                                }
                                //半成品EL不良率 敷设EL站判定不良/敷设站EL测试的数量
                                else if (key == "FS_DEFECT_RATE" && fsQty>0)
                                {
                                    drNew[dataDate] = fsDefectQty / fsQty;
                                }
                            }
                            //LJ_FS_QTY,LJ_ZJ_QTY,LJ_FS_DEFECT_QTY,LJ_ZJ_DEFECT_QTY
                            if (queryType == 1 || queryType==2)
                            {
                                double ljFsQty = Convert.ToDouble(drs[0]["LJ_FS_QTY"]);
                                double ljFsDefectQty = Convert.ToDouble(drs[0]["LJ_FS_DEFECT_QTY"]);

                                double ljZjQty = Convert.ToDouble(drs[0]["LJ_ZJ_QTY"]);
                                double ljZjDefectQty = Convert.ToDouble(drs[0]["LJ_ZJ_DEFECT_QTY"]);


                                if (ljFsQty + ljZjQty > 0)
                                {
                                    if (key == "LJ_FS_QTY")
                                    {
                                        drNew[dataDate] = ljFsQty;
                                    }
                                    else if (key == "LJ_FS_DEFECT_QTY")
                                    {
                                        drNew[dataDate] = ljFsDefectQty;
                                    }
                                    else if (key == "LJ_ZJ_QTY")
                                    {
                                        drNew[dataDate] = ljZjQty;
                                    }
                                    else if (key == "LJ_ZJ_DEFECT_QTY")
                                    {
                                        drNew[dataDate] = ljZjDefectQty;
                                    }
                                    //成品EL不良率 终检判定不良数/终检检验量
                                    else if (key == "LJ_ZJ_DEFECT_RATE" && ljZjQty > 0)
                                    {
                                        drNew[dataDate] = ljZjDefectQty / ljZjQty;
                                    }
                                    //半成品EL不良率 敷设EL站判定不良/敷设站EL测试的数量
                                    else if (key == "LJ_FS_DEFECT_RATE" && ljFsQty > 0)
                                    {
                                        drNew[dataDate] = ljFsDefectQty / ljFsQty;
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

