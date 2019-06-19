using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Collections;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 日运营报表数据访问类
    /// </summary>
    public class DailyReportDataAccess : BaseDBAccess
    {
        Dictionary<string, DataTable> dicDailyReportDatas;
        Dictionary<string, AutoResetEvent> dicAutoEvents;

        /// <summary>
        /// 获取指定日期区间的日运营报表数据。
        /// </summary>
        /// <param name="queryType">查询类型 0：按时间查询 1：按日期查询。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <param name="shiftName">班别。</param>
        /// <param name="roomName">车间名称。</param>
        /// <param name="proModel">产品型号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>包含日运营报表数据的数据集。</returns>
        public DataSet GetDailyReportData(
                            int queryType,
                            string start_time,
                            string end_time,
                            string shiftName,
                            string roomName,
                            string proModel,
                            string proId,
                            string partNumber,
                            string workOrderNo,
                            string factoryshift)
        {
            string[] storeProcedures = new string[]{
                "SP_QRY_DAILY_REPORT_DATA_TOP",
                "SP_QRY_DAILY_REPORT_DATA_OUT",
                "SP_QRY_DAILY_REPORT_DATA_WAREHOUSE_FACT",
                "SP_QRY_DAILY_REPORT_DATA_WAREHOUSE_REWORK",
                "SP_QRY_DAILY_REPORT_DATA_WIP",
                "SP_QRY_DAILY_REPORT_DATA_BRACKET",
                "SP_QRY_DAILY_REPORT_DATA_CHECK",
                "SP_QRY_DAILY_REPORT_DATA_CTM",
                "SP_QRY_DAILY_REPORT_DATA_FRAG"
            };

            dicDailyReportDatas = new Dictionary<string, DataTable>();
            dicAutoEvents = new Dictionary<string, AutoResetEvent>();

            using (DbConnection con = this._db.CreateConnection())
            {
                foreach (string storeProcedure in storeProcedures)
                {
                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    dicAutoEvents.Add(storeProcedure, autoEvent);
                    
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedure;
                    cmd.CommandTimeout = 120;
                    this._db.AddInParameter(cmd, "p_query_type", DbType.Decimal, queryType);
                    this._db.AddInParameter(cmd, "p_start_date", DbType.String, start_time);
                    this._db.AddInParameter(cmd, "p_end_date", DbType.String, end_time);
                    this._db.AddInParameter(cmd, "p_shiftName", DbType.String, shiftName);
                    this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                    this._db.AddInParameter(cmd, "p_proModel", DbType.String, proModel);
                    this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                    this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                    this._db.AddInParameter(cmd, "p_workOrder", DbType.String, workOrderNo);
                    this._db.AddInParameter(cmd, "p_factoryshift", DbType.String, factoryshift);
                    ParameterizedThreadStart start = new ParameterizedThreadStart(ExecuteStoreProcedure);
                    Thread t = new Thread(start);
                    t.Start(cmd);
                }
            }
            WaitHandle.WaitAll(dicAutoEvents.Values.ToArray());
            dicAutoEvents.Clear();
            dicAutoEvents = null;

            DataSet dsReturn = new DataSet();
            foreach (DataTable dt in dicDailyReportDatas.Values)
            {
                dsReturn.Merge(dt);
            }
            dicDailyReportDatas.Clear();
            dicDailyReportDatas = null;
            return dsReturn;
        }
        /// <summary>
        /// 执行存储过程。
        /// </summary>
        /// <param name="cmd"></param>
        private void ExecuteStoreProcedure(object cmd)
        {
            DbCommand command = cmd as DbCommand;
            if (command == null) return;
            string storeProcedure = command.CommandText;
            DataSet ds = this._db.ExecuteDataSet(command);
            dicDailyReportDatas.Add(storeProcedure, ds.Tables[0]);
            dicAutoEvents[storeProcedure].Set();
        }

        public DataTable TransferDatatable(DataTable dtSource, int queryType, Dictionary<string, string> dic)
        {
            dtSource.DefaultView.Sort = "SEQ_INDEX";
            DataTable dtBase = dtSource.DefaultView.ToTable(true, new string[] { "DATA_TYPE", "DATA_NAME", "DATA_FORMAT", "DATA_DETAIL_PAGE" });
            //生成表结构
            DataTable dtNew = new DataTable();
            for (int i = 0; i < dtBase.Columns.Count; i++)
            {
                dtNew.Columns.Add(dtBase.Columns[i].ColumnName);
            }
            IEnumerable<string> dateColNames = dtSource.AsEnumerable()
                                               .Select(row => Convert.ToString(row["DATA_DATE"]))
                                               .Distinct();
            foreach (string colName in dateColNames)
            {
                DataColumn dc = dtNew.Columns.Add(colName);
                string colCaption = Convert.ToDateTime(colName).ToString("yyyy-MM-dd");
                if(queryType==0)
                {
                    colCaption = Convert.ToDateTime(colName).ToString("yyyy-MM-dd HH");
                }
                if (colCaption.StartsWith("1900-01-01"))
                {
                    colCaption = "ALL";
                }
                dc.Caption = colCaption;
                dc.DataType = typeof(double);
            }
            dtNew.Columns.Add("DATA_DESCRIPTION");
            foreach (string dataType in dic.Keys)
            {
                if (dataType != "TOP")  //其他类型数据不显示标题行。
                {
                    DataRow drTitle = dtNew.NewRow();
                    drTitle["DATA_TYPE"] = dataType;
                    drTitle["DATA_NAME"] = dic[dataType];
                    dtNew.Rows.Add(drTitle);
                }
                DataRow[] drBases = dtBase.Select(string.Format("DATA_TYPE='{0}'",dataType));
                //填充数据
                for (int i = 0; i < drBases.Length; i++)
                {
                    string dataName = Convert.ToString(drBases[i]["DATA_NAME"]);
                    DataRow drNew = dtNew.NewRow();
                    drNew["DATA_TYPE"] = dataType;
                    drNew["DATA_NAME"] = dataName;
                    drNew["DATA_FORMAT"] = drBases[i]["DATA_FORMAT"];
                    drNew["DATA_DETAIL_PAGE"] = drBases[i]["DATA_DETAIL_PAGE"];
                    for (int j = dtBase.Columns.Count ; j < dtNew.Columns.Count - 1; j++)
                    {
                        string dataDate = dtNew.Columns[j].ColumnName;
                        string filter = string.Format("DATA_TYPE='{0}' AND DATA_NAME='{1}' AND DATA_DATE='{2}'", dataType, dataName, dataDate);
                        DataRow[] drs = dtSource.Select(filter);
                       
                        if (drs.Length > 0)
                        {
                            string description = Convert.ToString(drs[0]["DATA_DESCRIPTION"]);
                            if (drs[0]["QTY"] != null && drs[0]["QTY"] != DBNull.Value)
                            {
                                double qty = Convert.ToDouble(drs[0]["QTY"]);
                                drNew[dataDate] = qty;
                            }
                            if (!string.IsNullOrEmpty(description) 
                                && string.IsNullOrEmpty(Convert.ToString(drNew["DATA_DESCRIPTION"])))
                            {
                                drNew["DATA_DESCRIPTION"] = description;
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

