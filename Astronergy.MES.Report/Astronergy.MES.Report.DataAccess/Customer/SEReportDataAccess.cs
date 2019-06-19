using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// SE报表数据访问类
    /// </summary>
    public class SEReportDataAccess
    {
        class SEVIBreakdownDataAccess : BaseDBAccess
        {

            public DataTable GetData(
                            string start_time,
                            string end_time,
                            string roomName,
                            string workOrderNo,
                            string partNumber,
                            string customer,
                            int type/*0:终检 1:终检前层压后*/)
            {
                //获取各工序不良原因数量
                List<string> lstOperations = null;
                if (type == 1)
                {
                    lstOperations = new List<string>()
                    {
                        "层压",
                        "装框",
                        "清洁",
                        "组件测试"
                    };
                }
                else
                {
                    lstOperations = new List<string>()
                    {
                        "终检"
                    };
                }
                //合并数据
                DataSet dsReturn = new DataSet();
                Dictionary<string, AutoResetEvent> dicAutoEvents = new Dictionary<string, AutoResetEvent>();
                foreach (string item in lstOperations)
                {
                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    dicAutoEvents.Add(item, autoEvent);
                    ParameterizedThreadStart start = new ParameterizedThreadStart((obj) =>
                    {
                        string operationName = Convert.ToString(obj);
                        using (DbConnection con = this._db.CreateConnection())
                        {
                            DbCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_QRY_SE_DEFECT_DETAIL_DATA";
                            cmd.CommandTimeout = 120;
                            this._db.AddInParameter(cmd, "p_start_date", DbType.String, start_time);
                            this._db.AddInParameter(cmd, "p_end_date", DbType.String, end_time);
                            this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                            this._db.AddInParameter(cmd, "p_workorder", DbType.String, workOrderNo);
                            this._db.AddInParameter(cmd, "p_operationName", DbType.String, operationName);
                            this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                            this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                            DataTable dtDefectData = this._db.ExecuteDataSet(cmd).Tables[0];
                            con.Close();
                            dtDefectData.TableName = "DEFECT_DATA";

                            lock (this)
                            {
                                dsReturn.Merge(dtDefectData, false, MissingSchemaAction.Add);
                            }
                            dicAutoEvents[operationName].Set();
                        }
                    });
                    Thread t = new Thread(start);
                    t.Start(item);
                }
                WaitHandle.WaitAll(dicAutoEvents.Values.ToArray());
                dicAutoEvents.Clear();
                dicAutoEvents = null;
                return TransformData(start_time,end_time,type,dsReturn);
            }


            private DataTable TransformData(string start_time,
                                            string end_time, 
                                            int type,
                                            DataSet dsReturn)
            {
                //获取不良代码名称
                DataTable dtReasonCode = GetReasonCodeData(type);
                DataTable dtDestTable=new DataTable();

                dtDestTable.Columns.Add(new DataColumn("Type"));
                DataColumn dcDefectItem= new DataColumn("DefectItem", typeof(string));
                dcDefectItem.Caption = "Defect Item";
                dtDestTable.Columns.Add(dcDefectItem);

                DateTime startDate= DateTime.Parse(start_time);
                DateTime endDate = DateTime.Parse(end_time);

                for (DateTime start = startDate; start <= endDate; start = start.AddDays(1))
                {
                    DataColumn dc = new DataColumn(start.ToString("yyyy-MM-dd"), typeof(double));
                    dc.Caption = start.ToString("MM.dd");
                    dtDestTable.Columns.Add(dc);
                    if (start.DayOfWeek == DayOfWeek.Sunday || start==endDate)
                    {
                        string wkName = DateTimeHelper.GetWeekOfYearFirstDay(start, DayOfWeek.Monday).ToString("WK00");
                        dtDestTable.Columns.Add(new DataColumn(wkName, typeof(double)));
                    }
                }

                for (int i = 0; i < dtReasonCode.Rows.Count;i++)
                {
                    DataRow dr = dtDestTable.NewRow();
                    string rcName=Convert.ToString(dtReasonCode.Rows[i]["REASON_CODE_NAME"]);
                    string startTemp = string.Empty;
                    string endTemp = string.Empty;
                    foreach (DataColumn col in dtDestTable.Columns)
                    {
                        

                        if (col.ColumnName == "Type")
                        {
                            dr[col] = type == 0 ? "Defects: \r\n Modules（终检）" : "Defects: \r\n Laminate （层压后终检前）";
                        }
                        else if (col.ColumnName == "DefectItem")
                        {
                            dr[col] =rcName;
                        }
                        else if (col.ColumnName.StartsWith("WK"))
                        {
                            var qry = from item in dsReturn.Tables[0].AsEnumerable()
                                      where Convert.ToDateTime(item["DATA_DATE"]) >= DateTime.Parse(startTemp)
                                            && Convert.ToDateTime(item["DATA_DATE"]) <= DateTime.Parse(endTemp)
                                            && Convert.ToString(item["REASON_CODE_NAME"]) == rcName
                                      select Convert.ToDouble(item["DEFECT_QUANTITY"]);
                            dr[col] = qry.Sum();
                            startTemp = string.Empty;
                            endTemp = string.Empty;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(startTemp))
                            {
                                startTemp = col.ColumnName;
                            }
                            endTemp = col.ColumnName;

                            var qry = from item in dsReturn.Tables[0].AsEnumerable()
                                      where Convert.ToDateTime(item["DATA_DATE"]) == DateTime.Parse(col.ColumnName)
                                            && Convert.ToString(item["REASON_CODE_NAME"]) == rcName
                                      select Convert.ToDouble(item["DEFECT_QUANTITY"]);
                            dr[col] = qry.Sum();
                        }
                    }
                    dtDestTable.Rows.Add(dr);
                }
                return dtDestTable;
            }

            private DataTable GetReasonCodeData(int type)
            {
                //获取终检不良代码名称
                DataTable dtReasonCode = null;
                string storeProcedureName = string.Empty;
                if (type == 0)
                {
                    storeProcedureName = "SP_QRY_SE_REASON_CODE_DATA_CHECK";
                }
                else
                {
                    storeProcedureName = "SP_QRY_SE_REASON_CODE_DATA_PRE_CHECK";
                }
                using (DbConnection con = this._db.CreateConnection())
                {
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedureName;
                    dtReasonCode = this._db.ExecuteDataSet(cmd).Tables[0];
                    con.Close();
                }
                return dtReasonCode;
            }
        }

        public static DataSet GetSEVIBreakdownData(
                            string start_time,
                            string end_time,
                            string roomName,
                            string workOrderNo,
                            string partNumber,
                            string customer)
        {
            DataSet dsReturn = new DataSet();
            SEVIBreakdownDataAccess dataAccess = new SEVIBreakdownDataAccess();
            Dictionary<int, AutoResetEvent> dicAutoEvents = new Dictionary<int, AutoResetEvent>();
            
            for (int i = 0; i <= 1;i++ )
            {
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                dicAutoEvents.Add(i, autoEvent);
                ParameterizedThreadStart start = new ParameterizedThreadStart((obj) =>
                {
                    int type = Convert.ToInt32(obj);
                    DataTable dt = dataAccess.GetData(start_time, end_time, roomName, workOrderNo, partNumber, customer, type);
                    dt.TableName = "DEFECT_DATA";
                    lock (dsReturn)
                    {
                        dsReturn.Merge(dt,false,MissingSchemaAction.Add);
                    }
                    dicAutoEvents[type].Set();
                });
                Thread t = new Thread(start);
                t.Start(i);
            }
            WaitHandle.WaitAll(dicAutoEvents.Values.ToArray());
            dicAutoEvents.Clear();
            dicAutoEvents = null;
            return dsReturn;
        }


        class SEProductionYieldDataAccess : BaseDBAccess
        {
            Dictionary<string, string> dicOperations = new Dictionary<string, string>()
            {
                {"单串焊","Stable & String Soldering (单串焊)"},
                {"敷设","Lay-up（敷设）"},
                {"层压前红外","EL1(层压前红外)"},
                {"层压","EL(层压)"},
                {"装框","Frame(装框)"},
                {"清洁","Clearning （清洁）"},
                {"组件测试","FLASHER（测试）"},
                {"红外","EL2（红外）"},
                {"终检","FINAL INSPECTION（终检）"}
            };

            List<string> lstBefore = new List<string>()
                {
                    "单串焊",
                    "敷设",
                    "层压前红外",
                };

            List<string> lstAfter = new List<string>()
                {
                    "层压",
                    "装框",
                    "清洁",
                    "组件测试",
                    "红外",
                    "终检",
                };

            public DataTable GetData(
                            string start_time,
                            string end_time,
                            string roomName,
                            string workOrderNo,
                            string partNumber,
                            string customer)
            {
                //获取各工序投入数量和不良数量
                //合并数据
                DataSet dsReturn = new DataSet();
                Dictionary<string, AutoResetEvent> dicAutoEvents = new Dictionary<string, AutoResetEvent>();
                foreach (string item in dicOperations.Keys)
                {
                    AutoResetEvent autoEvent = new AutoResetEvent(false);
                    dicAutoEvents.Add(item, autoEvent);
                    ParameterizedThreadStart start = new ParameterizedThreadStart((obj) =>
                    {
                        string operationName = Convert.ToString(obj);
                        using (DbConnection con = this._db.CreateConnection())
                        {
                            DbCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_QRY_SE_YIELD_DATA";
                            cmd.CommandTimeout = 120;
                            this._db.AddInParameter(cmd, "p_start_date", DbType.String, start_time);
                            this._db.AddInParameter(cmd, "p_end_date", DbType.String, end_time);
                            this._db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                            this._db.AddInParameter(cmd, "p_workorder", DbType.String, workOrderNo);
                            this._db.AddInParameter(cmd, "p_operationName", DbType.String, operationName);
                            this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                            this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                            DataTable dtData = this._db.ExecuteDataSet(cmd).Tables[0];
                            con.Close();
                            dtData.TableName = "DEFECT_DATA";
                            lock (this)
                            {
                                dsReturn.Merge(dtData, false, MissingSchemaAction.Add);
                            }
                            dicAutoEvents[operationName].Set();
                        }
                    });
                    Thread t = new Thread(start);
                    t.Start(item);
                }
                WaitHandle.WaitAll(dicAutoEvents.Values.ToArray());
                dicAutoEvents.Clear();
                dicAutoEvents = null;
                return TransformData(start_time, end_time, dsReturn);
            }


            private DataTable TransformData(string start_time,
                                            string end_time,
                                            DataSet dsReturn)
            {
                DataTable dtDestTable = new DataTable();
                dtDestTable.Columns.Add(new DataColumn("Type"));
                dtDestTable.Columns.Add(new DataColumn("Station"));

                DataColumn dcTarget = new DataColumn("Target", typeof(string));
                dcTarget.Caption = "Target%";
                dtDestTable.Columns.Add(dcTarget);

                DataColumn dcItemName = new DataColumn("ItemName", typeof(string));
                dcItemName.Caption = "/";
                dtDestTable.Columns.Add(dcItemName);
                
                DateTime startDate = DateTime.Parse(start_time);
                DateTime endDate = DateTime.Parse(end_time);

                for (DateTime start = startDate; start <= endDate; start = start.AddDays(1))
                {
                    DataColumn dc = new DataColumn(start.ToString("yyyy-MM-dd"), typeof(double));
                    dtDestTable.Columns.Add(dc);
                    if (start.DayOfWeek == DayOfWeek.Sunday || start == endDate)
                    {
                        string wkName = DateTimeHelper.GetWeekOfYearFirstDay(start, DayOfWeek.Monday).ToString("WK00");
                        dtDestTable.Columns.Add(new DataColumn(wkName, typeof(double)));
                    }
                }
                CalcOperationYield(dtDestTable, dsReturn, lstBefore,0); 
                CalcFPYRate(dtDestTable, "FPY rate",0);
                CalcOperationYield(dtDestTable, dsReturn, lstAfter,1);
                CalcFPYRate(dtDestTable, "Manufacturing Yield",1);
                CalcFPYRate(dtDestTable, "FPY rate",-1);  //计算全部
                return dtDestTable;
            }

            private void CalcOperationYield(DataTable dtDestTable,DataSet dsReturn, IList<string> lstOperations,int type)
            {
                foreach (string item in lstOperations)
                {
                    //i=0  IN_QTY
                    //i=1  DEFECT_QTY
                    //i=2  Yield
                    for (int i = 0; i <= 2; i++)
                    {
                        DataRow dr = dtDestTable.NewRow();
                        string startTemp = string.Empty;
                        string endTemp = string.Empty;
                        foreach (DataColumn col in dtDestTable.Columns)
                        {
                            if (col.ColumnName == "Station")
                            {
                                dr[col] = dicOperations[item];
                            }
                            else if (col.ColumnName == "Type")
                            {
                                dr[col] = type;
                            }
                            else if (col.ColumnName == "Target")
                            {
                                continue;
                            }
                            else if (col.ColumnName == "ItemName")
                            {
                                string itemName = string.Empty;
                                if (item == "单串焊")
                                {
                                    itemName = i == 0 ? "Input(cells)" : (i == 1 ? "Defect Cells" : "Yield");
                                }
                                else
                                {
                                    itemName = i == 0 ? "Input" : (i == 1 ? "Defect Module" : "Yield");
                                }
                                dr[col] = itemName;
                            }
                            else if (col.ColumnName.StartsWith("WK"))
                            {
                                var qryInQty = from row in dsReturn.Tables[0].AsEnumerable()
                                          where Convert.ToDateTime(row["DATA_DATE"]) >= DateTime.Parse(startTemp)
                                                && Convert.ToDateTime(row["DATA_DATE"]) <= DateTime.Parse(endTemp)
                                                && Convert.ToString(row["STEP_NAME"]) == item
                                          select Convert.ToDouble(row["IN_QTY"]);

                                var qryDefectQty = from row in dsReturn.Tables[0].AsEnumerable()
                                          where Convert.ToDateTime(row["DATA_DATE"]) >= DateTime.Parse(startTemp)
                                                && Convert.ToDateTime(row["DATA_DATE"]) <= DateTime.Parse(endTemp)
                                                && Convert.ToString(row["STEP_NAME"]) == item
                                          select Convert.ToDouble(row["DEFECT_QTY"]);

                                if (i == 0) //IN_QTY
                                {
                                    dr[col] = qryInQty.Sum();
                                }
                                else if (i == 1)//DEFECT_QTY
                                {
                                    dr[col] = qryDefectQty.Sum();
                                }
                                else if (i == 2) //Yield
                                {
                                    double sumInQty = qryInQty.Sum();
                                    if (sumInQty > 0)
                                    {
                                        dr[col] = 1 - qryDefectQty.Sum() / sumInQty;
                                    }
                                    else
                                    {
                                        dr[col] = 1;
                                    }
                                }
                                startTemp = string.Empty;
                                endTemp = string.Empty;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(startTemp))
                                {
                                    startTemp = col.ColumnName;
                                }
                                endTemp = col.ColumnName;

                                var qryInQty = from row in dsReturn.Tables[0].AsEnumerable()
                                               where Convert.ToDateTime(row["DATA_DATE"]) == DateTime.Parse(col.ColumnName)
                                                     && Convert.ToString(row["STEP_NAME"]) == item
                                               select Convert.ToDouble(row["IN_QTY"]);

                                var qryDefectQty = from row in dsReturn.Tables[0].AsEnumerable()
                                                   where Convert.ToDateTime(row["DATA_DATE"]) == DateTime.Parse(col.ColumnName)
                                                         && Convert.ToString(row["STEP_NAME"]) == item
                                                   select Convert.ToDouble(row["DEFECT_QTY"]);

                                if (i == 0) //IN_QTY
                                {
                                    dr[col] = qryInQty.Sum();
                                }
                                else if (i == 1)//DEFECT_QTY
                                {
                                    dr[col] = qryDefectQty.Sum();
                                }
                                else if (i == 2) //Yield
                                {
                                    double sumInQty = qryInQty.Sum();
                                    if (sumInQty > 0)
                                    {
                                        dr[col] = 1 - qryDefectQty.Sum() / sumInQty;
                                    }
                                    else
                                    {
                                        dr[col] = 1;
                                    }
                                }
                            }
                        }
                        dtDestTable.Rows.Add(dr);
                    }
                }
            }

            private void CalcFPYRate(DataTable dt, string fpyRateName,int type)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "Station")
                    {
                        dr[col] = fpyRateName;
                    }
                    else if (col.ColumnName == "Type")
                    {
                        dr[col] = type;
                    }
                    else if (col.ColumnName == "Target")
                    {
                        continue;
                    }
                    else if (col.ColumnName == "ItemName")
                    {
                        dr[col] = string.Empty;
                    }
                    else
                    {
                        var qry = from row in dt.AsEnumerable()
                                  where Convert.ToString(row["ItemName"]) == "Yield" 
                                        && (type>=0?Convert.ToInt32(row["Type"]) == type:true)
                                  select Convert.ToDouble(row[col.ColumnName]);
                        double mulityYield=1;
                        foreach(double yield in qry){
                            mulityYield *= yield;
                        }
                        dr[col] = mulityYield;
                    }
                }
                dt.Rows.Add(dr);
            }

        }


        public static DataSet GetSEProductionYieldData(string start_time,
                            string end_time,
                            string roomName,
                            string workOrderNo,
                            string partNumber,
                            string customer)
        {
            DataSet dsReturn = new DataSet();
            SEProductionYieldDataAccess dataAccess = new SEProductionYieldDataAccess();
            DataTable dt = dataAccess.GetData(start_time, end_time, roomName, workOrderNo, partNumber, customer);
            dt.TableName = "YIELD_DATA";
            dsReturn.Tables.Add(dt);
            return dsReturn;
        }


        class SEFinalDashboardDataAccess : BaseDBAccess
        {
            string start_time;
            string end_time;
            string roomName;
            string workOrderNo;
            string partNumber;
            string customer;

            public SEFinalDashboardDataAccess(string start_time,
                            string end_time,
                            string roomName,
                            string workOrderNo,
                            string partNumber,
                            string customer)
            {

            }
            /// <summary>
            /// 获取实际投入数。
            /// </summary>
            /// <returns></returns>
            public DataTable GetInputData()
            {
                DataTable dt = new DataTable();
                return dt;
            }

        }
    }
}

