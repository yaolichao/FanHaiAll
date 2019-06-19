using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 在制品分布数据访问。
    /// </summary>
    public class WipDisplayDataAccess:BaseDBAccess
    {
        /// <summary>
        /// 车间名称。
        /// </summary>
        public string FactoryRoomName
        {
            get;
            set;
        }
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string FactoryRoomKey
        {
            get;
            set;
        }
        /// <summary>
        /// 产品ID号。
        /// </summary>
        public string ProId
        {
            get;
            set;
        }
        /// <summary>
        /// 工单主键。
        /// </summary>
        public string WorkOrderNumberKey
        {
            get;
            set;
        }
        /// <summary>
        /// 产品料号。
        /// </summary>
        public string PartNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 等待时间（小时）
        /// </summary>
        public int Hours
        {
            get;
            set;
        }
        /// <summary>
        /// 在线时间（小时）
        /// </summary>
        public int OnLineHours
        {
            get;
            set;
        }
        /// <summary>
        /// 获取工序在制品数据。
        /// </summary>
        /// <param name="dtChart">绑定chart用的数据源</param>
        /// <param name="dtTable">绑定表格用的数据源</param>
        /// <returns>
        /// true：获取成功。false获取失败。
        /// </returns>
        public bool GetWIP(ref DataTable dtChart, ref DataTable dtTable)
        {
            DataSet ds = new DataSet();
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_QRY_WIP_DATA";
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, this.FactoryRoomKey == "ALL" ? string.Empty : this.FactoryRoomKey);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, this.ProId == "ALL" ? string.Empty : this.ProId);
                this._db.AddInParameter(cmd, "p_workorderKey", DbType.String, 
                                              this.WorkOrderNumberKey == "ALL" ? string.Empty : this.WorkOrderNumberKey);
                this._db.AddInParameter(cmd, "p_waitHour", DbType.Int32, this.Hours);
                this._db.AddInParameter(cmd, "p_onlineHouer", DbType.Int32, this.OnLineHours);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, this.PartNumber == "ALL" ? string.Empty : this.PartNumber);
                ds=this._db.ExecuteDataSet(cmd);
            }

            if (ds == null || ds.Tables.Count == 0)
            {
                return false;
            }

            DataTable dsReturn = ds.Tables[0];
            string roomName = this.FactoryRoomName;
            string partType = "%";
            DataTable dtColumns = CommonFunction.GetOperations(roomName, partType).Tables[0];

            #region  把原始数据绑定到chart上

            dtChart = new DataTable();
            DataColumn colOpeartion = new DataColumn("STEP_NAME");
            dtChart.Columns.Add(colOpeartion);
            DataColumn colWait = new DataColumn("WAIT");
            dtChart.Columns.Add(colWait);
            DataColumn colRun = new DataColumn("RUN");
            dtChart.Columns.Add(colRun);
            DataColumn colHold = new DataColumn("HOLD");
            dtChart.Columns.Add(colHold);

            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                DataRow dr = dtChart.NewRow();
                dtChart.Rows.Add(dr);
                dr[0] = dtColumns.Rows[i]["ROUTE_STEP_NAME"];
                dr[1] = 0;
                dr[2] = 0;
                dr[3] = 0;
                var query = from row in dsReturn.AsEnumerable()
                            where Convert.ToString(row["ROUTE_STEP_NAME"]) == Convert.ToString(dr[0])
                                  && row["STATE_FLAG"].ToString() == "等待"
                            select new { QTY = Convert.ToInt32(row["QTY"]) };
                if (query.Count() > 0)
                {
                    int qty = query.Sum(p => p.QTY);
                    dr[1] = qty;
                }

                query = from row in dsReturn.AsEnumerable()
                        where Convert.ToString(row["ROUTE_STEP_NAME"]) == Convert.ToString(dr[0])
                              && row["STATE_FLAG"].ToString() == "运行"
                        select new { QTY = Convert.ToInt32(row["QTY"]) };
                if (query.Count() > 0)
                {
                    int qty = query.Sum(p => p.QTY);
                    dr[2] = qty;
                }

                query = from row in dsReturn.AsEnumerable()
                        where Convert.ToString(row["ROUTE_STEP_NAME"]) == Convert.ToString(dr[0])
                              && row["STATE_FLAG"].ToString() == "暂停"
                        select new { QTY = Convert.ToInt32(row["QTY"]) };
                if (query.Count() > 0)
                {
                    int qty = query.Sum(p => p.QTY);
                    dr[3] = qty;
                }
            }

            #endregion

            #region 整理成显示格式的数据
            if (dsReturn != null && dsReturn.Rows.Count > 0)
            {
                const string SUMMARY_COLUMN_NAME = "合计";
                DataTable dt = new DataTable();
                //增加状态列
                DataColumn dcStatus = new DataColumn("状态");
                dt.Columns.Add(dcStatus);
                DataRow dr0= dt.NewRow();
                dr0[0] = "暂停";
                dt.Rows.Add(dr0);
                DataRow dr1 = dt.NewRow();
                dr1[0] = "等待";
                dt.Rows.Add(dr1);
                DataRow dr2 = dt.NewRow();
                dr2[0] = "运行";
                dt.Rows.Add(dr2);
                DataRow dr3 = dt.NewRow();
                dr3[0] = "总计";
                dt.Rows.Add(dr3);
                try
                {
                    //增加工序列表
                    for (int i = 0; i < dtColumns.Rows.Count; i++)
                    {
                        string colName = Convert.ToString(dtColumns.Rows[i]["ROUTE_STEP_NAME"]);
                        DataColumn col = new DataColumn(colName);
                        col.DataType = typeof(double);
                        dt.Columns.Add(col);
                        dr0[col] = "0";
                        dr1[col] = "0";
                        dr2[col] = "0";
                        dr3[col] = "0";
                    }
                    //增加合计列
                    DataColumn dcSum = new DataColumn(SUMMARY_COLUMN_NAME);
                    dcSum.DataType = typeof(double);
                    dt.Columns.Add(dcSum);

                    int sumHold = 0;
                    int sumWait = 0;
                    int sumRun = 0;
                    ///凑数据源的表头
                    for (int i = 0; i < dsReturn.Rows.Count; i++)
                    {
                        //工序
                        string stepName = dsReturn.Rows[i]["ROUTE_STEP_NAME"].ToString();
                        //状态
                        string strState_flag = dsReturn.Rows[i]["STATE_FLAG"].ToString();
                        //数量
                        string strNum = dsReturn.Rows[i]["QTY"].ToString();
                        int qty = 0;
                        int.TryParse(strNum, out qty);
                        switch (strState_flag)
                        {
                            case "暂停":
                                dr0[stepName] = qty.ToString();
                                sumHold += qty;
                                break;
                            case "等待":
                                dr1[stepName] = qty.ToString();
                                sumWait += qty;
                                break;
                            case "运行":
                                dr2[stepName] = qty.ToString();
                                sumRun += qty;
                                break;
                            default:
                                break;
                        }
                        dr3[stepName] =Convert.ToInt32(dr0[stepName]) + Convert.ToInt32(dr1[stepName]) + Convert.ToInt32(dr2[stepName]);
                    }
                    dr0[SUMMARY_COLUMN_NAME] = sumHold;
                    dr1[SUMMARY_COLUMN_NAME] = sumWait;
                    dr2[SUMMARY_COLUMN_NAME] = sumRun;
                    dr3[SUMMARY_COLUMN_NAME] = sumWait + sumRun + sumHold;

                    dtTable = dt;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            #endregion

            return false;
        }
        /// <summary>
        /// 获取工序在制品分布明细。
        /// </summary>
        /// <param name="stepName">工步名称。</param>
        /// <param name="status">明细状态。</param>
        /// <returns>包含在制品分布明细的数据表。</returns>
        public DataTable GetWIPDetail(string stepName, string status,string partNumber)
        {
            DataSet ds = new DataSet();
            using (DbConnection con = this._db.CreateConnection())
            {
                int type = -1;
                if (status.Equals("暂停"))
                {
                    type = 0;
                }
                if (status.Equals("等待"))
                {
                    type = 1;
                }
                else if (status.Equals("运行"))
                {
                    type = 2;
                }
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_QRY_WIP_DATA_DTL";

                this._db.AddInParameter(cmd, "p_type", DbType.Int32, type);
                this._db.AddInParameter(cmd, "p_stepName", DbType.String, stepName == "ALL" ? string.Empty : stepName);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, this.FactoryRoomKey == "ALL" ? string.Empty : this.FactoryRoomKey);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, this.ProId == "ALL" ? string.Empty : this.ProId);
                this._db.AddInParameter(cmd, "p_workorderKey", DbType.String,
                                              this.WorkOrderNumberKey == "ALL" ? string.Empty : this.WorkOrderNumberKey);
                this._db.AddInParameter(cmd, "p_waitHour", DbType.Int32, this.Hours);
                this._db.AddInParameter(cmd, "p_onLineHour", DbType.Int32, this.OnLineHours);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber == "ALL" ? string.Empty : partNumber);
                ds = this._db.ExecuteDataSet(cmd);
            }
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}