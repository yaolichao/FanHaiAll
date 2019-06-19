using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    public class DailyReportEntity : BaseDBAccess
    {

        #region
        /// <summary>
        /// 车间ID
        /// </summary>
        public string LocationKeys
        {
            get { return _locationkey; }
            set { _locationkey = value; }
        }
        /// <summary>
        /// 车间名称
        /// </summary>
        public string LocationNames
        {
            get { return _locationName; }
            set { _locationName = value; }
        }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public string Daily_Start_Time
        {
            get { return _daily_start_time; }
            set { _daily_start_time = value; }
        }
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public string Daily_End_Time
        {
            get { return _daily_end_time; }
            set { _daily_end_time = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string Start_Time
        {
            get;
            set;
        }
        /// <summary>
        /// 查询结束的时间
        /// </summary>
        public string End_Time
        {
            get { return _end_time; }
            set { _end_time = value; }
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        public string Current_Time
        {
            get { return _current_date; }
            set { _current_date = value; }
        }
        /// <summary>
        /// 当前系统时间
        /// </summary>
        public string Current_sys_date
        {
            get { return _current_sys_date; }
            set { _current_sys_date = value; }
        }
        /// <summary>
        /// 产品ID号
        /// </summary>
        public string Pro_Ids
        {
            get { return _pro_id; }
            set { _pro_id = value; }
        }
        public string Proids_notcomma
        {
            get { return _proid_notcomma; }
            set { _proid_notcomma = value; }
        }
        public string WoNumbers_notcomma
        {
            get { return _wonumber_notcomma; }
            set { _wonumber_notcomma = value; }
        }
        public string Pro_Type_notcomma
        {
            get { return _protype_notcomma; }
            set { _protype_notcomma = value; }
        }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNumbers
        {
            get { return _wonumber; }
            set { _wonumber = value; }
        }
        /// <summary>
        /// 产品型号
        /// </summary>
        public string Pro_Type
        {
            get { return _protype; }
            set { _protype = value; }
        }
        public string PlanId
        {
            get { return _planid; }
            set { _planid = value; }
        }
        /// <summary>
        /// 不良原因代码
        /// </summary>
        public string Reason_Code_Class
        {
            get { return _reason_code_class; }
            set { _reason_code_class = value; }
        }
        /// <summary>
        /// 产品等级
        /// </summary>
        public string Grade
        {
            get { return _grade; }
            set { _grade = value; }
        }
        /// <summary>
        /// 所在工序
        /// </summary>
        public string Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }
        /// <summary>
        /// 记录报错信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        public string Pmax
        {
            get { return _pmax; }
            set { _pmax = value; }
        }
        /// <summary>
        /// 按时间/日期 查询
        /// </summary>
        public bool IsDay
        {
            get { return _hour_day; }
            set { _hour_day = value; }
        }
        /// <summary>
        /// 班别
        /// </summary>
        public string Shift
        {
            get { return _shift; }
            set { _shift = value; }
        }
        /// <summary>
        /// 班别名称
        /// </summary>
        public string ShiftName
        {
            get { return _shift_name; }
            set { _shift_name = value; }
        }

        private string _locationkey = string.Empty;
        private string _locationName = string.Empty;
        private string _daily_start_time = string.Empty;
        private string _daily_end_time = string.Empty;
        private string _end_time = string.Empty;
        private string _current_date = string.Empty;
        private string _current_sys_date=string.Empty;
        private string _pro_id = string.Empty;
        private string _proid_notcomma = string.Empty;
        private string _wonumber_notcomma = string.Empty;
        private string _protype_notcomma = string.Empty;
        private string _wonumber = string.Empty;
        private string _protype = string.Empty;
        private string _reason_code_class = string.Empty;
        private string _planid = string.Empty;
        private string _grade = string.Empty;
        private string _operation = string.Empty;
        private string _pmax = string.Empty;
        private string _shift = string.Empty;
        private string _shift_name = string.Empty;

        private bool _hour_day = false;

        private string _errorMsg = string.Empty;
        #endregion

        /// <summary>
        /// 获取日运营报表数据。
        /// </summary>
        /// <returns> 
        /// Query 1:按天为列数据呈现，2：按时间为列数据呈现
        /// </returns>
        public DataSet GetDailyReport()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DateTime dtStartDayTime = Convert.ToDateTime(Daily_Start_Time + " " + Start_Time.Substring(0, Start_Time.IndexOf(':')) + ":00:00");
                DateTime dtEndDayTime = Convert.ToDateTime(Daily_End_Time + " " + End_Time.Substring(0, End_Time.IndexOf(':')) + ":00:00");
                TimeSpan tsStart = new TimeSpan(dtStartDayTime.Ticks);
                TimeSpan tsEnd = new TimeSpan(dtEndDayTime.Ticks);
                TimeSpan tsInterval = tsEnd.Subtract(tsStart).Duration();

                int iday = tsInterval.Days;
                int ihour = tsInterval.Hours;
                //按时间查询。
                if (!this.IsDay)
                {
                    if (ihour < 2)
                    {
                        ErrorMsg = "【结束时间】必须大于【开始时间】至少一个小时,请确认!";
                        return dsReturn;
                    }
                    GetDailyReportByHour(dsReturn, dtStartDayTime, dtEndDayTime);
                    dsReturn.ExtendedProperties.Add(LayoutViewType.ViewModule, LayoutViewType.ViewType_Hour);
                }
                //按日期查询
                else 
                {
                    GetDailyReportByDay(dsReturn);
                    dsReturn.ExtendedProperties.Add(LayoutViewType.ViewModule, LayoutViewType.ViewType_Day);
                }
                DataTable dtReport = dsReturn.Tables[LayoutViewType.ReportTable];
                DataTable dtCells = dsReturn.Tables[LayoutViewType.ReportTable].Clone();
                DataRow[] drsReport = dtReport.Select("SEQ=''");
                foreach (DataRow dr in drsReport)
                {
                    dtCells.Rows.Add(dr.ItemArray);
                    dtReport.Rows.Remove(dr);
                }
                dtCells.TableName = LayoutViewType.ReportPressCells;
                dsReturn.Merge(dtCells, true, MissingSchemaAction.Add);
            }
            catch(Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dsReturn;
        }
        /// <summary>
        /// 按小时统计同一天数据
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private void GetDailyReportByHour(DataSet dsReturn, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dtDailyReport = new DataTable();
            DataTable dtYieldForCustCheck = new DataTable();
            try
            {
                string sqlView = string.Empty;
                string tmpDate = string.Empty;
                string temp01 = _daily_end_time;
                string temp02 = _daily_start_time;
                string s_row2col01 = " SELECT PRO_NAME, SEQ ,count(0) RN ";
                string s_row2col = " ";
                //把结束时间初始化为当前时间
                if (dtEnd > Convert.ToDateTime(Convert.ToDateTime(_current_date).ToString("yyyy-MM-dd HH:mm") + ":00"))
                {
                    dtEnd = Convert.ToDateTime(Convert.ToDateTime(_current_date).ToString("yyyy-MM-dd HH:mm") + ":00");
                }
                //------------------------------------------------------------------------------------------------------               
                for (DateTime dtime = dtStart; dtime <= dtEnd; dtime = dtime.AddHours(1))
                {
                    string tmp = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                    s_row2col += string.Format(@" ,CONVERT(varchar,SUM(case DAILY_DATE  when CONVERT(DATETIME,'{0}') then SUM_DATA else 0 end)) ""{0}"" ", tmp);

                    
                    DataRow[] drsTemp = this.Get(dtime.ToString(), dtime.AddHours(1).ToString(), _locationkey, string.Empty, _protype_notcomma, _proid_notcomma, _wonumber_notcomma);
                    if (dtYieldForCustCheck.Rows.Count < 1 && drsTemp != null && drsTemp.Length > 0)
                        dtYieldForCustCheck = drsTemp[0].Table.Clone();
                    foreach (DataRow dr in drsTemp)
                    {
                        dr["DATA_DATE"] = tmp;
                        dtYieldForCustCheck.Rows.Add(dr.ItemArray);
                    }
                }

                string endColumn = string.Empty, datecolumn = string.Empty;
                string sql01 = @"  FROM RPT_WIP_DAILY
                                     WHERE 1=1 ";
                sql01 += string.Format(@"  AND DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')", dtStart.ToString(), dtEnd.ToString());
                if (!string.IsNullOrEmpty(_pro_id))
                    sql01 += string.Format(@" AND PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_protype))
                    sql01 += string.Format(@" AND PART_TYPE in ({0})", _protype);
                if (!string.IsNullOrEmpty(_locationkey))
                    sql01 += string.Format(@" AND LOCATION_KEY ='{0}'", _locationkey.PreventSQLInjection());

                sqlView = string.Format(s_row2col01 + s_row2col + sql01);

                if (!string.IsNullOrEmpty(_wonumber))
                    sqlView += string.Format(@" and WORK_ORDER_NO in ({0})", _wonumber);

                sqlView += " GROUP BY PRO_NAME, SEQ";

                string sql02 = @" SELECT CONVERT(varchar,T.ALL_SUM_DATA) ALL_SUM_DATA, A.*
                                          FROM ({0}) A,
                                               (SELECT T.PRO_NAME, SUM(T.SUM_DATA) ALL_SUM_DATA
                                                  FROM RPT_WIP_DAILY T WHERE 1=1 ";
                if (!string.IsNullOrEmpty(_pro_id))
                    sql02 += string.Format(@" AND t.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_protype))
                    sql02 += string.Format(@" AND t.PART_TYPE in ({0}) ", _protype);
                if (!string.IsNullOrEmpty(_locationkey))
                    sql02 += string.Format(@" AND t.LOCATION_KEY ='{0}'", _locationkey.PreventSQLInjection());

                if (!string.IsNullOrEmpty(_wonumber))
                    sql02 += string.Format(" and WORK_ORDER_NO in ({0})", _wonumber);

                sql02 += string.Format(@" AND T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}') ", dtStart.ToString(), dtEnd.ToString());

                sql02 += @"GROUP BY T.PRO_NAME) T WHERE A.PRO_NAME = T.PRO_NAME ORDER BY A.SEQ ASC ";

                string sql = string.Format(sql02, sqlView);

                dtDailyReport = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
           
                if (dtDailyReport.Rows.Count < 1)
                {
                    _errorMsg = "没有查询数据";
                    return;
                }

                string sqlCommand = string.Format(@" select COUNT(DAILY_DATE) RN,CONVERT(varchar, DAILY_DATE,120) DAILY_DATE,PRO_NAME, SEQ,SUM(SUM_DATA) SUM_DATA
                                                      FROM RPT_WIP_DAILY
                                                    WHERE 1=1  ");
                if (!string.IsNullOrEmpty(_pro_id))
                    sqlCommand += string.Format(@" AND PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_protype))
                    sqlCommand += string.Format(@" AND PART_TYPE in ({0})", _protype);
                if (!string.IsNullOrEmpty(_locationkey))
                    sqlCommand += string.Format(@" AND LOCATION_KEY ='{0}'", _locationkey.PreventSQLInjection());

                if (!string.IsNullOrEmpty(_wonumber))
                    sqlCommand += string.Format(" and WORK_ORDER_NO in ({0})", _wonumber);

                sqlCommand += string.Format(@" AND DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}') ", dtStart.ToString(), dtEnd.ToString());

                sqlCommand += " GROUP BY DAILY_DATE,PRO_NAME, SEQ ";
                DataTable dtCount = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                
                SetRowEffiData(dtDailyReport, dtCount, dtYieldForCustCheck);


                #region 统计计划数据
                sql = string.Format(@"SELECT  convert(varchar,T.PLAN_DATE_START,111) PLAN_DATE,CONVERT(numeric(10,2), SUM(T.QUANTITY_INPUT)/24) QUANTITY_INPUT, 
                                    CONVERT(numeric(10,2), SUM(T.QUANTITY_OUTPUT)/24) QUANTITY_OUTPUT
                                    FROM RPT_PLAN_AIM T
                                        WHERE t.ISFLAG=1 ");
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(" and t.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_locationName))
                    sql += string.Format(" and t.LOCATION_NAME='{0}'", _locationName);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(" and t.PART_TYPE in ({0}) ", _protype);
                if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                    sql += string.Format(" and  T.PLAN_DATE_START BETWEEN convert(datetime,'{0}') AND convert(datetime,'{1}')", dtStart.ToString("yyyy-MM-dd 00:00:00"), dtEnd.ToString());

                sql += "   GROUP BY T.PLAN_DATE_START ORDER BY T.PLAN_DATE_START ASC";
                #endregion

                DataTable dtPlan = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //新增投入计划
                #region
                DataRow drDaily = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                     if (dc.ColumnName == "RN") continue;
                    decimal v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drDaily[dc.ColumnName] = "PLAN_INPUT";
                        continue;
                    }
                    else if (dc.ColumnName == "SEQ")
                    {
                        drDaily[dc.ColumnName] = "A10";
                        continue;
                    }
                    else if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToDecimal(Convert.ToString(dr["QUANTITY_INPUT"]) == string.Empty ? "0" : Convert.ToString(dr["QUANTITY_INPUT"]));
                        }
                        drDaily[dc.ColumnName] = v.ToString();

                        continue;
                    }             
                    else
                    {
                        DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd")));
                        if (drPlans != null && drPlans.Length > 0)
                        {
                            drDaily[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_INPUT"]) == string.Empty ? "0" : drPlans[0]["QUANTITY_INPUT"].ToString();
                        }
                    }
                }
                dtDailyReport.Rows.InsertAt(drDaily, 0);
                #endregion
                //新增入库计划
                #region
                DataRow drToStore = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    if (dc.ColumnName == "RN") continue;
                    decimal v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drToStore[dc.ColumnName] = "PLAN_TOSTORE";
                    }
                    else if (dc.ColumnName == "SEQ")
                    {
                        drToStore[dc.ColumnName] = "B10";
                    }
                    else if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToDecimal(dr["QUANTITY_OUTPUT"].ToString() == string.Empty ? "0" : dr["QUANTITY_OUTPUT"].ToString());
                        }
                        drToStore[dc.ColumnName] = v.ToString();
                    }
                    else
                    {
                        DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd")));
                        if (drPlans != null && drPlans.Length > 0)
                        {
                            drToStore[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_OUTPUT"]) == string.Empty ? "0" : drPlans[0]["QUANTITY_OUTPUT"];
                        }
                    }
                }
                dtDailyReport.Rows.InsertAt(drToStore, 6);
                #endregion
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            
            dtDailyReport.TableName = LayoutViewType.ReportTable;
            dsReturn.Merge(dtDailyReport, true, MissingSchemaAction.Add);
            return;
        }
        /// <summary>
        /// 不是同一天数据
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private void GetDailyReportByDayHour(DataSet dsReturn, DateTime dtStart, DateTime dtMiddle, DateTime dtEnd, int invervalDay)
        {
            DataTable dtDailyReport = new DataTable();
            try
            {
                string sqlView = string.Empty;
                string tmpDate = string.Empty;
                string temp01 = _daily_end_time;
                string temp02 = _daily_start_time;
                string s_row2col01 = " SELECT A.PRO_NAME, A.SEQ ,COUNT(A.PRO_NAME) RN ";
                string s_row2col = " ";
                //把结束时间初始化为当前时间
                if (dtEnd > Convert.ToDateTime(Convert.ToDateTime(_current_date).ToString("yyyy-MM-dd HH:mm") + ":00"))
                {
                    dtEnd = Convert.ToDateTime(Convert.ToDateTime(_current_date).ToString("yyyy-MM-dd HH:mm") + ":00");
                }
                //------------------------------------------------------------------------------------------------------  
                if (invervalDay == 0)
                {  
                    for (DateTime dtime = dtStart.AddDays(-1); dtime <= dtEnd; dtime = dtime.AddDays(1))
                    {
                        string tmp = dtime.ToString("yyyy-MM-dd") + " 00:00:00";
                        s_row2col += string.Format(@" ,CONVERT(varchar,SUM(case DAILY_DATE  when CONVERT(DATETIME,'{0}') then SUM_DATA else 0 end)) ""{0}"" ", tmp);
                    }
                }
                if (invervalDay == 1)
                {
                    DateTime dtBegin = new DateTime();
                    if (dtStart > Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd HH") + ":00:00"))
                        dtBegin = dtStart;
                    else
                        dtBegin = dtStart.AddDays(-1);

                    for (DateTime dtime = dtBegin; dtime <= dtEnd; dtime = dtime.AddDays(1))
                    {
                        string tmp = dtime.ToString("yyyy-MM-dd") + " 00:00:00";
                        s_row2col += string.Format(@" ,CONVERT(varchar,SUM(case DAILY_DATE  when CONVERT(DATETIME,'{0}') then SUM_DATA else 0 end)) ""{0}"" ", tmp);
                    }
                }

                string endColumn = string.Empty, datecolumn = string.Empty;
                string sql = " from (  ";
                string sql01 = string.Format(@"select t.PRO_NAME,SUM(ISNULL(t.SUM_DATA,0)) SUM_DATA,T.SEQ ,CONVERT(datetime,'{2}') DAILY_DATE
                                from RPT_WIP_DAILY t 
                                where t.DAILY_DATE BETWEEN CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')
                                and ISNULL(T.SEQ,'')<>''
                               ", dtStart, dtMiddle, dtStart.AddDays(-1).ToString("yyyy-MM-dd"));
                if(!string.IsNullOrEmpty(_locationkey))
                    sql01 += string.Format(@" and T.LOCATION_KEY='{0}'", _locationkey);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql01 += string.Format(@" and T.PRO_ID IN ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sql01 += string.Format(@" and T.WORK_ORDER_NO IN ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_protype))
                    sql01 += string.Format(@" AND T.PART_TYPE='{0}'", _protype);
                sql01 += @" GROUP BY T.PRO_NAME,T.SEQ   union all";

                sql01 += string.Format(@" select t1.PRO_NAME,SUM(ISNULL(t1.SUM_DATA,0)) SUM_DATA,T1.SEQ ,CONVERT(datetime,'{2}') DAILY_DATE
                                from RPT_WIP_DAILY t1 
                                where t1.DAILY_DATE BETWEEN CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')
                                ", dtMiddle, dtEnd, dtEnd.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(_locationkey))
                    sql01 += string.Format(@" and T1.LOCATION_KEY='{0}'", _locationkey);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql01 += string.Format(@" and T1.PRO_ID IN ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sql01 += string.Format(@" and T1.WORK_ORDER_NO IN ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_protype))
                    sql01 += string.Format(@" AND T1.PART_TYPE='{0}'", _protype);
                sql01 += @" GROUP BY T1.PRO_NAME,T1.SEQ ";

                string sql02 = @" ) A GROUP BY A.PRO_NAME, A.SEQ  ";

               string sqlCommand01 = s_row2col01 + s_row2col + sql + sql01 + sql02;

               string sql_sum = string.Format(@"select SUM(ISNULL(T.SUM_DATA,0)) ALL_SUM_DATA,T.PRO_NAME,T.SEQ from RPT_WIP_DAILY t 
                               where   T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')
                                 ", sqlCommand01, dtStart.ToString(), dtEnd.ToString());
               if (!string.IsNullOrEmpty(_locationkey))
                   sql_sum += string.Format(@" and T.LOCATION_KEY='{0}'", _locationkey);
               if (!string.IsNullOrEmpty(_pro_id))
                   sql_sum += string.Format(@" and T.PRO_ID IN ({0})", _pro_id);
               if (!string.IsNullOrEmpty(_wonumber))
                   sql_sum += string.Format(@" and T.WORK_ORDER_NO IN ({0})", _wonumber);
               if (!string.IsNullOrEmpty(_protype))
                   sql_sum += string.Format(@" AND T.PART_TYPE='{0}'", _protype);
               sql_sum += @" GROUP BY T.PRO_NAME,T.SEQ  ";

               string sql_all = string.Format(@"select convert(varchar,tt.ALL_SUM_DATA) ALL_SUM_DATA,tt1.* from ({0}) tt,({1}) tt1 where tt.PRO_NAME=tt1.PRO_NAME order by tt1.seq asc ", sql_sum, sqlCommand01);

               dtDailyReport = _db.ExecuteDataSet(CommandType.Text, sql_all).Tables[0];


                if (dtDailyReport.Rows.Count < 1)
                {
                    _errorMsg = "没有查询数据";
                    return;
                }

                //------------------------------统计总和中的优品率------------------------------------------------
                #region 
                sql = @"SELECT SUM(T.SUM_DATA) ALL_SUM_DATA,T.PRO_NAME,COUNT(PRO_NAME) ALL_COUNT FROM RPT_WIP_DAILY T WHERE ISNULL(T.SEQ,'')=''";
                if (!string.IsNullOrEmpty(_locationkey))
                    sql += string.Format(@" AND T.LOCATION_KEY='{0}'", _locationkey);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(@" AND T.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and T.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(@" AND T.PART_TYPE='{0}'", _protype);

                sql += string.Format(@" AND T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')", dtStart.ToString(), dtEnd.ToString());

                sql += " GROUP BY T.PRO_NAME";

                DataTable dtSum = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //获得总实际入库组件
                DataRow drstore = (from DataRow row in dtDailyReport.Rows
                                   where row.Field<string>("PRO_NAME").ToString() == "REL_TOSTORE_QTY"
                                   select row).FirstOrDefault();
                DataRow drkj = (from DataRow row in dtSum.Rows
                                where row.Field<string>("PRO_NAME").ToString() == "TO_STORE_KJ"
                                select row).FirstOrDefault();
                DataRow draj = (from DataRow row in dtSum.Rows
                                where row.Field<string>("PRO_NAME").ToString() == "TO_STORE_AJ"
                                select row).FirstOrDefault();
                DataRow dra0j = (from DataRow row in dtSum.Rows
                                 where row.Field<string>("PRO_NAME").ToString() == "TO_STORE_A0J"
                                 select row).FirstOrDefault();

                DataRow dr_a0j = (from DataRow row in dtDailyReport.Rows
                                  where row.Field<string>("PRO_NAME").ToString() == "PER_GR_A0J"
                                  select row).FirstOrDefault();
                DataRow dr_aj = (from DataRow row in dtDailyReport.Rows
                                 where row.Field<string>("PRO_NAME").ToString() == "PER_GR_AJ"
                                 select row).FirstOrDefault();
                DataRow dr_kj = (from DataRow row in dtDailyReport.Rows
                                 where row.Field<string>("PRO_NAME").ToString() == "PER_GR_KJ"
                                 select row).FirstOrDefault();

                decimal store_qty = Convert.ToDecimal(drstore["ALL_SUM_DATA"] == null ? 0 : drstore["ALL_SUM_DATA"]);
                decimal kj_qty = Convert.ToDecimal(drkj["ALL_SUM_DATA"] == null ? 0 : drkj["ALL_SUM_DATA"]);
                decimal aj_qty = Convert.ToDecimal(draj["ALL_SUM_DATA"] == null ? 0 : draj["ALL_SUM_DATA"]);
                decimal a0j_qty = Convert.ToDecimal(dra0j["ALL_SUM_DATA"] == null ? 0 : dra0j["ALL_SUM_DATA"]);


                dr_a0j["ALL_SUM_DATA"] = Math.Round((a0j_qty + aj_qty + kj_qty) / (store_qty == 0 ? 1 : store_qty), 2).ToString();
                dr_aj["ALL_SUM_DATA"] = Math.Round((aj_qty + kj_qty) / (store_qty == 0 ? 1 : store_qty), 2).ToString();
                dr_kj["ALL_SUM_DATA"] = Math.Round((kj_qty) / (store_qty == 0 ? 1 : store_qty), 2).ToString();

                #endregion
                //------------------------------------------------------------------------------------------------

                #region 统计计划数据
                sql = string.Format(@"SELECT  convert(varchar,T.PLAN_DATE_START,111) PLAN_DATE, SUM(T.QUANTITY_INPUT) QUANTITY_INPUT, 
                                        SUM(T.QUANTITY_OUTPUT) QUANTITY_OUTPUT
                                        FROM RPT_PLAN_AIM T
                                        WHERE 1=1 ");
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(" and t.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_locationName))
                    sql += string.Format(" and t.LOCATION_NAME='{0}'", _locationName);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(" and t.PART_TYPE='{0}'", _protype);
                if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                    sql += string.Format(" and  T.PLAN_DATE BETWEEN convert(datetime,'{0}') AND convert(datetime,'{1}')", dtStart.ToString(), dtEnd.ToString());

                sql += "   GROUP BY T.PLAN_DATE ORDER BY T.PLAN_DATE ASC";
                #endregion

                DataTable dtPlan = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //新增投入计划
                #region
                DataRow drDaily = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drDaily[dc.ColumnName] = "PLAN_INPUT";
                    }
                    if (dc.ColumnName == "SEQ")
                    {
                        drDaily[dc.ColumnName] = "A10";
                    }

                    if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(Convert.ToString(dr["QUANTITY_INPUT"]) == string.Empty ? "0" : Convert.ToString(dr["QUANTITY_INPUT"]));
                        }
                        drDaily[dc.ColumnName] = v.ToString();
                    }

                    DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd")));
                    if (drPlans != null && drPlans.Length > 0)
                    {
                        drDaily[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_INPUT"]) == "" ? 0 : Math.Round(Convert.ToDecimal(drPlans[0]["QUANTITY_INPUT"]) / 12, 2);
                    }
                }
                dtDailyReport.Rows.InsertAt(drDaily, 0);
                #endregion

                //新增入库计划
                #region
                DataRow drToStore = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drToStore[dc.ColumnName] = "PLAN_TOSTORE";
                    }
                    if (dc.ColumnName == "SEQ")
                    {
                        drToStore[dc.ColumnName] = "B10";
                    }

                    if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(dr["QUANTITY_OUTPUT"].ToString() == string.Empty ? "0" : dr["QUANTITY_OUTPUT"].ToString());
                        }
                        drToStore[dc.ColumnName] = v.ToString();
                    }

                    DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd")));
                    if (drPlans != null && drPlans.Length > 0)
                    {
                        drToStore[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_OUTPUT"]) == "" ? 0 : Math.Round(Convert.ToDecimal(drPlans[0]["QUANTITY_OUTPUT"]) / 12, 2);
                    }
                }
                dtDailyReport.Rows.InsertAt(drToStore, 6);
                #endregion
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            dtDailyReport.TableName = LayoutViewType.ReportTable;
            dsReturn.Merge(dtDailyReport, true, MissingSchemaAction.Add);
            return;
        }
        /// <summary>
        /// 按天统计日运营数据。
        /// </summary>
        /// <param name="dsReturn"></param>
        private void GetDailyReportByDay(DataSet dsReturn)
        {
            DataTable dtDailyReport = new DataTable();
            try
            {
                string sqlView = string.Empty;
                string tmpDate = string.Empty;
                string temp01 = _daily_end_time;
                string temp02 = _daily_start_time;
                DataTable dtCommand = new DataTable(), 
                    dtSql_all = new DataTable(), 
                    dtReport = new DataTable(), 
                    dtYieldForCustCheck = new DataTable(),
                    dtSumfront = new DataTable(), 
                    dtSumfront_tmp = new DataTable();
                string sql = string.Empty;
                if (!string.IsNullOrEmpty(_locationkey))
                    sql += string.Format(@" AND T.LOCATION_KEY='{0}' ", _locationkey);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(@" AND T.PRO_ID IN ({0}) ", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(@" AND T.WORK_ORDER_NO IN ({0}) ", _wonumber);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(@" AND T.PART_TYPE in ({0}) ", _protype);
                sql += @" GROUP BY T.PRO_NAME,T.SEQ ";
                
                string sqlCommand = string.Empty;
                string sqlCommand_Effi = string.Empty;
                //------------------------------------------------------------------------------------------------------   
                #region 按天查询
                string tmp = string.Empty;
                DateTime dtStart_q = Convert.ToDateTime(Daily_Start_Time);
                DateTime dtEnd_h = Convert.ToDateTime(Daily_End_Time);
                bool isLoadReport = false;
                for (DateTime dtime = dtStart_q; dtime <= dtEnd_h; dtime = dtime.AddDays(1))
                {
                    tmp = dtime.ToString("yyyy-MM-dd") + " 00:00:00";
                    DateTime dtStart = DateTime.Now;
                    DateTime dtEnd = DateTime.Now;
                    string errMsg = string.Empty;
                    string sql_all = string.Empty;
                    string[] lst = new string[2];
                    lst = CommonFunction.GetOptShiftDate(_locationkey, "单串焊", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));

                    #region//单串焊工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //WIP+工序MOVE量+实际单串焊产量+电池片总投入量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                 AND t.PRO_NAME IN ('E100','REL_INPUT','CELL_ALL_QTY')", 
                                                 tmp,dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    if (!isLoadReport)
                    {
                        dtReport = dtCommand.Clone();
                        isLoadReport = true;
                    }

                    foreach (DataRow drCommand in dtCommand.Rows)
                        dtReport.Rows.Add(drCommand.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "敷设", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region //敷设数据
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //敷设数据
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME,t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME='E101'", 
                                                tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "层压前EL测试", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region//统计层压前EL测试工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //wip+工序move量+层压前EL测试产量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME,t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME='E102'", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "层压", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region//统计层压工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //wip+工序move量+实际层压产量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                 AND t.PRO_NAME IN ('E103','REL_PRESS')", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "装框", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region//统计装框工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //wip+工序move量+实际层压产量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                 AND t.PRO_NAME='E104'", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "清洁", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region//统计清洁工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //wip+工序move量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME='E105'", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "组件测试", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //组件测试统计+wip+move量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND (t.PRO_NAME ='E106' OR t.SEQ='B233')",
                                                tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);
                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "终检", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //终检等级数量+wip+move量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                 AND t.PRO_NAME IN ('KJ_QTY','AJ_QTY','A0J_QTY','ERJ_QTY','SANJ_QTY','E107')", 
                                                 tmp, dtStart, dtEnd);
                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }
                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);
                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "包装", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region//统计包装工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //wip+工序move量+实际包装产量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME ='E108'", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "入库检验", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region//统计入库检验工序
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //wip+工序move量+实际入库检验产量
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') and CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME ='E108'", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "入库", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //入库wip+入库move量+入库统计+各档位分布情况
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND (t.PRO_NAME IN ('REL_TOSTORE_QTY','TO_STORE_KJ','TO_STORE_AJ','TO_STORE_A0J',
                                                                    'TO_STORE_ERJ','TO_STORE_SANJ',
                                                                    'PER_GR_KJ','PER_GR_AJ','PER_GR_A0J','REL_TOSTORE_POWER','E110') 
                                                     OR t.SEQ='B222')", 
                                                tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "转工单", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region 转工单
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //转入转出数量统计
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                 AND t.PRO_NAME IN ('REL_EXCHG_IN_QTY','REL_EXCHG_OUT_QTY','REL_OUT')", 
                                                 tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "物料工序", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region 碎片数量统计
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //碎片不良数据
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME IN ('IQC_CELL_QTY','PRESS_CELL_QTY','PER_CELL_CRUSH','REL_IQC_CELL_CRUSH','REL_PRESS_CELL_CRUSH')",
                                                tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "IPQC工序", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region IPQC工序数量统计
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //碎片不良数据
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                FROM RPT_WIP_DAILY t 
                                                WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                AND t.PRO_NAME ='E111'", 
                                                tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    lst = CommonFunction.GetOptShiftDate(_locationkey, "返修工序", _shift, out errMsg, dtime.ToString("yyyy-MM-dd"));
                    #region 返修工序数量统计
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        break;
                    }
                    dtStart = Convert.ToDateTime(lst[0]);
                    dtEnd = Convert.ToDateTime(lst[1]);

                    //碎片不良数据
                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}')  
                                                 AND t.PRO_NAME ='E112'", tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drEffi in dtCommand.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);

                    #endregion

                    #region 统计个工序在制品数据

                    sqlCommand = string.Format(@"SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,SUM(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                 FROM RPT_WIP_DAILY t 
                                                 WHERE t.DAILY_DATE = CONVERT(DATETIME,'{1}') 
                                                 AND t.PRO_NAME LIKE 'D%'", 
                                                 tmp, 
                                                 dtime.AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00");
                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand += sql;
                    }

                    dtCommand = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                    foreach (DataRow drCommand in dtCommand.Rows)
                        dtReport.Rows.Add(drCommand.ItemArray);

                    #endregion

                    #region//统计效率+CTM
                    sqlCommand_Effi = string.Format(@"SELECT t.PRO_NAME,t.SEQ,SUM(t1.SUM_DATA) RN,SUM(t.SUM_DATA*t1.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE 
                                                     FROM RPT_WIP_DAILY t,RPT_WIP_DAILY t1 
                                                     WHERE t.LOCATION_KEY=t1.LOCATION_KEY 
                                                     AND t.PART_TYPE=t1.PART_TYPE 
                                                     AND t.DAILY_DATE=t1.DAILY_DATE
                                                     AND t.PRO_ID=t1.PRO_ID 
                                                     AND t.WORK_ORDER_NO=t1.WORK_ORDER_NO
                                                     AND t.PRO_NAME IN ('WEIGHTING_EFFI', 'WEIGHTING_CTM_COUNT','WEIGHTING_CTM','THEORYTOTLEPOWER','RELTOTLEPOWER')
                                                     AND t.DAILY_DATE BETWEEN CONVERT(DATETIME,'{1}') AND CONVERT(DATETIME,'{2}') ", 
                                                     tmp, dtStart, dtEnd);

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand_Effi += sql;
                    }

                    DataTable dtCommand_Effi = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Effi).Tables[0];
                    foreach (DataRow drEffi in dtCommand_Effi.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);
                    #endregion

                    #region 客级品率
                    try
                    {
                        DataRow[] drsTemp = this.Get(dtime.ToString(),
                                                     dtime.AddDays(1).ToString(),
                                                     _locationkey, 
                                                     string.Empty,
                                                     _protype_notcomma, 
                                                     _proid_notcomma, 
                                                     _wonumber_notcomma);
                        if (dtYieldForCustCheck.Rows.Count < 1 && drsTemp != null && drsTemp.Length > 0)
                            dtYieldForCustCheck = drsTemp[0].Table.Clone();
                        foreach (DataRow dr in drsTemp)
                        {
                            dr["DATA_DATE"] = tmp;
                            dtYieldForCustCheck.Rows.Add(dr.ItemArray);
                        }
                    }
                    catch (Exception ex)
                    { 
                    
                    }
                    #endregion
                }
                    
                #endregion
                //----------------------------------------------------------------------------------------------------
                dtReport.AcceptChanges();

                dtSumfront = new DataTable();
                dtSumfront = dtReport.DefaultView.ToTable(true, new string[] { "PRO_NAME", "SEQ" });
                dtSumfront.Columns.Add("ALL_SUM_DATA");

                foreach (DataRow drSum in dtSumfront.Rows)
                {
                    string p_name = Convert.ToString(drSum["PRO_NAME"]);
                    string seq = Convert.ToString(drSum["SEQ"]);
                    if (!string.IsNullOrEmpty(seq))
                    {
                        drSum["ALL_SUM_DATA"] = Convert.ToString(dtReport.Compute("SUM(SUM_DATA)", string.Format("PRO_NAME='{0}'", p_name)));
                    }
                    else
                    {
                        drSum["ALL_SUM_DATA"] = Convert.ToString(dtReport.Compute("SUM(SUM_DATA)", string.Format("PRO_NAME='{0}' AND SEQ='{1}'", p_name, seq)));
                    }
                }
                dtSumfront.AcceptChanges();

                DataTable dtDistinctDatetime = dtReport.DefaultView.ToTable("DailyReport", true, "DAILY_DATE");
                foreach (DataRow drDistinctDatetime in dtDistinctDatetime.Rows)
                {
                    string sday = Convert.ToString(drDistinctDatetime["DAILY_DATE"]);

                    if (!dtSumfront.Columns.Contains(sday))
                        dtSumfront.Columns.Add(sday);

                    DataRow[] drs = dtReport.Select(string.Format("DAILY_DATE='{0}'", sday));
                    foreach (DataRow dr01 in drs)
                    {
                        string columname_report = Convert.ToString(dr01["PRO_NAME"]);
                        string columname_seq = Convert.ToString(dr01["SEQ"]);
                        DataRow[] drs01 = null;
                        if (columname_seq.Trim().Equals(string.Empty))
                        {
                            drs01 = dtSumfront.Select(string.Format("PRO_NAME='{0}' ", columname_report));
                        }
                        else
                        {
                            drs01 = dtSumfront.Select(string.Format("PRO_NAME='{0}' and SEQ='{1}' ", columname_report, columname_seq));
                        }

                        if (drs01 != null)
                        {
                            drs01[0][sday] = dr01["SUM_DATA"];
                        }
                    }
                }


                if (dtSumfront.Rows.Count < 1)
                {
                    _errorMsg = "没有查询到数据";
                    return;
                }
                dtDailyReport = dtSumfront;

                //在制品分布数据汇总
                #region
                DataRow[] drsDailyReport = dtDailyReport.Select("SEQ LIKE 'D%'");
                foreach (DataRow drDailyReport in drsDailyReport)
                {
                    string seq = Convert.ToString(drDailyReport["SEQ"]);
                    int qty_Wip = 0;
                    DataRow[] drs01 = dtReport.Select(string.Format(@"SEQ='{0}'", seq));
                    foreach (DataRow drWip in drs01)
                    {
                        qty_Wip += Convert.ToInt16(drWip["SUM_DATA"]);
                    }

                    drDailyReport["ALL_SUM_DATA"] = qty_Wip.ToString();
                }
                #endregion

                //DataTable dtCount = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                SetRowEffiData(dtDailyReport, dtReport, dtYieldForCustCheck);

                #region 统计计划数据
                sql = string.Format(@"SELECT  CONVERT(VARCHAR,T.PLAN_DATE_START,111) PLAN_DATE, SUM(T.QUANTITY_INPUT) QUANTITY_INPUT, 
                                        SUM(T.QUANTITY_OUTPUT) QUANTITY_OUTPUT
                                      FROM RPT_PLAN_AIM T
                                      WHERE t.ISFLAG=1 ");
                if (!string.IsNullOrEmpty(_wonumber))
                {
                    sql += string.Format(" AND t.WORK_ORDER_NO in ({0})", _wonumber);
                }
                if (!string.IsNullOrEmpty(_pro_id))
                {
                    sql += string.Format(" AND t.PRO_ID in ({0})", _pro_id);
                }
                if (!string.IsNullOrEmpty(_locationName))
                {
                    sql += string.Format(" AND t.LOCATION_NAME='{0}'", _locationName);
                }
                if (!string.IsNullOrEmpty(_protype))
                {
                    sql += string.Format(" AND t.PART_TYPE in ({0}) ", _protype);
                }
                if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                {
                    sql += string.Format(@" AND CONVERT(VARCHAR, T.PLAN_DATE_START,111)  BETWEEN CONVERT(VARCHAR,CONVERT(DATETIME,'{0}'),111)
                                            AND CONVERT(VARCHAR,CONVERT(DATETIME,'{1}'),111)",
                                          Convert.ToDateTime(Daily_Start_Time).ToString(),
                                          Convert.ToDateTime(Daily_End_Time).ToString());
                }
                sql += " GROUP BY T.PLAN_DATE_START ORDER BY T.PLAN_DATE_START ASC";
                #endregion

                DataTable dtPlan = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //新增投入计划
                #region
                DataRow drDaily = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drDaily[dc.ColumnName] = "PLAN_INPUT";
                    }
                    else if (dc.ColumnName == "SEQ")
                    {
                        drDaily[dc.ColumnName] = "A10";
                    }
                    else if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(Convert.ToString(dr["QUANTITY_INPUT"]) == string.Empty ? "0" : Convert.ToString(dr["QUANTITY_INPUT"]));
                        }
                        drDaily[dc.ColumnName] = v.ToString();
                    }
                    else
                    {
                        string coldate = Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd");

                        DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", coldate));
                        if (drPlans != null && drPlans.Length > 0)
                        {
                            drDaily[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_INPUT"]) == "" ? "0" : Convert.ToString(drPlans[0]["QUANTITY_INPUT"]);
                        }
                    }
                }
                dtDailyReport.Rows.InsertAt(drDaily, 0);
                #endregion

                //新增入库计划
                #region
                DataRow drToStore = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drToStore[dc.ColumnName] = "PLAN_TOSTORE";
                    }
                    else if (dc.ColumnName == "SEQ")
                    {
                        drToStore[dc.ColumnName] = "B10";
                    }
                    else if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(dr["QUANTITY_OUTPUT"].ToString() == string.Empty ? "0" : dr["QUANTITY_OUTPUT"].ToString());
                        }
                        drToStore[dc.ColumnName] = v.ToString();
                    }
                    else
                    {
                        string coldate = Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd");
                        DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", coldate));
                        if (drPlans != null && drPlans.Length > 0)
                        {
                            drToStore[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_OUTPUT"]) == "" ? "0" : Convert.ToString(drPlans[0]["QUANTITY_OUTPUT"]);
                        }
                    }
                }
                dtDailyReport.Rows.InsertAt(drToStore, 6);
                #endregion
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            dtDailyReport.TableName = LayoutViewType.ReportTable;
            dsReturn.Merge(dtDailyReport, true, MissingSchemaAction.Add);
            return;
        }
        /// <summary>
        /// 跨多天的数据
        /// </summary>
        /// <param name="dsReturn"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtStart_q"></param>
        /// <param name="dtEnd_h"></param>
        /// <param name="dtEnd"></param>
        private void GetDailyReportByDay(DataSet dsReturn, DateTime dtStart, DateTime dtStart_q,DateTime dtEnd_h, DateTime dtEnd)
        {
            DataTable dtDailyReport = new DataTable();
            try
            {
                string sqlView = string.Empty;
                string tmpDate = string.Empty;
                string temp01 = _daily_end_time;
                string temp02 = _daily_start_time;
                DataTable dtCommon = new DataTable(), dtReport = new DataTable(), dtYieldForCustCheck = new DataTable();
                string sql = string.Empty;
                if (!string.IsNullOrEmpty(_locationkey))
                    sql += string.Format(@" and T.LOCATION_KEY='{0}' ", _locationkey);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(@" and T.PRO_ID IN ({0}) ", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(@" and T.WORK_ORDER_NO IN ({0}) ", _wonumber);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(@" AND T.PART_TYPE in ({0}) ", _protype);
                sql += @" GROUP BY T.PRO_NAME,T.SEQ ";
                string tmp = dtStart.ToString("yyyy-MM-dd") + " 00:00:00";
                string tmp01 = string.Empty;
                string sqlCommand = string.Empty;
                string sqlCommand_Wip = string.Empty;
                string sqlCommand_Effi = string.Empty;
                if (dtStart < dtStart_q)
                {
                    if (dtStart < Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd ") + "08:00:00"))
                        tmp01 = dtStart.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00";
                    else
                        tmp01 = dtStart.ToString("yyyy-MM-dd") + " 00:00:00";

                    sqlCommand = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,sum(t.SUM_DATA) SUM_DATA, '{0}' DAILY_DATE from RPT_WIP_DAILY t 
                                                where t.SEQ NOT LIKE 'D%' AND  t.DAILY_DATE between CONVERT(datetime,'{1}') and CONVERT(datetime,'{2}') 
                                                and t.PRO_NAME not in('WEIGHTING_EFFI','WEIGHTING_CTM_COUNT') ", tmp01, dtStart.ToString(), dtStart_q.ToString());

                    sqlCommand_Wip = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,sum(t.SUM_DATA) SUM_DATA, '{0}' DAILY_DATE from RPT_WIP_DAILY t 
                                                    where t.SEQ  LIKE 'D%' AND  t.DAILY_DATE= CONVERT(datetime,'{1}') ",
                                                 tmp01, dtStart_q.ToString());

                    sqlCommand_Effi = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,sum(t1.SUM_DATA) RN,sum(t.SUM_DATA*t1.SUM_DATA) SUM_DATA, '{0}' DAILY_DATE 
                                                         from RPT_WIP_DAILY t,RPT_WIP_DAILY t1 
                                                         where   t.LOCATION_KEY=t1.LOCATION_KEY 
                                                         and t.PART_TYPE=t1.PART_TYPE and t.DAILY_DATE=t1.DAILY_DATE
                                                         and t.PRO_ID=t1.PRO_ID and t.WORK_ORDER_NO=t1.WORK_ORDER_NO
                                                         and t.PRO_NAME='WEIGHTING_EFFI'
                                                         and t1.PRO_NAME= 'WEIGHTING_CTM_COUNT'
                                                          and t.DAILY_DATE between CONVERT(datetime,'{1}') and CONVERT(datetime,'{2}') ", tmp01, dtStart.ToString(), dtStart_q.ToString());

                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlCommand_Wip += sql;
                        sqlCommand += sql;
                        sqlCommand_Effi += sql;
                    }
                    DataTable dtTemp = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Wip).Tables[0];
                    dtReport = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                    foreach (DataRow drWip in dtTemp.Rows)
                        dtReport.Rows.Add(drWip.ItemArray);

                    DataTable dtTemp2 = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Effi).Tables[0];

                    foreach(DataRow drEffi in dtTemp2.Rows)
                        dtReport.Rows.Add(drEffi.ItemArray);


                    DataRow[] drsTemp = this.Get(dtStart.ToString(), dtStart_q.ToString(), _locationkey, string.Empty, _protype_notcomma, _proid_notcomma, _wonumber_notcomma);
                    if (dtYieldForCustCheck.Rows.Count < 1 && drsTemp != null && drsTemp.Length > 0)
                        dtYieldForCustCheck = drsTemp[0].Table.Clone();
                    foreach (DataRow dr in drsTemp)
                    {
                        dr["DATA_DATE"] = tmp01;
                        dtYieldForCustCheck.Rows.Add(dr.ItemArray);
                    }
                }
                if (dtEnd_h > dtStart_q)
                {
                    //------------------------------------------------------------------------------------------------------   
                    if (dtStart_q < dtEnd_h)
                    {
                        for (DateTime dtime = dtStart_q.AddHours(1); dtime < dtEnd_h; dtime = dtime.AddDays(1))
                        {
                            tmp = dtime.ToString("yyyy-MM-dd") + " 00:00:00";
                            sqlCommand = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,sum(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE from RPT_WIP_DAILY t 
                                                where t.SEQ NOT LIKE 'D%' AND t.DAILY_DATE between CONVERT(datetime,'{1}') and CONVERT(datetime,'{2}')  
                                                and t.PRO_NAME not in('WEIGHTING_EFFI','WEIGHTING_CTM_COUNT')", tmp, dtime, dtime.AddDays(1).AddHours(-1));

                            sqlCommand_Wip = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,sum(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE from RPT_WIP_DAILY t 
                                                where t.SEQ  LIKE 'D%' AND t.DAILY_DATE = CONVERT(datetime,'{1}')  ", tmp, dtime.AddDays(1).AddHours(-1));

                            sqlCommand_Effi = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,sum(t1.SUM_DATA) RN,sum(t.SUM_DATA*t1.SUM_DATA) SUM_DATA, '{0}' DAILY_DATE 
                                                         from RPT_WIP_DAILY t,RPT_WIP_DAILY t1 
                                                         where   t.LOCATION_KEY=t1.LOCATION_KEY 
                                                         and t.PART_TYPE=t1.PART_TYPE and t.DAILY_DATE=t1.DAILY_DATE
                                                         and t.PRO_ID=t1.PRO_ID and t.WORK_ORDER_NO=t1.WORK_ORDER_NO
                                                         and t.PRO_NAME='WEIGHTING_EFFI'
                                                         and t1.PRO_NAME= 'WEIGHTING_CTM_COUNT'
                                                          and t.DAILY_DATE between CONVERT(datetime,'{1}') and CONVERT(datetime,'{2}') ", tmp, dtime, dtime.AddDays(1).AddHours(-1));

                            if (!string.IsNullOrEmpty(sql))
                            {
                                sqlCommand_Wip += sql;
                                sqlCommand += sql;
                                sqlCommand_Effi += sql;
                            }
                            DataTable dtTemp = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Wip).Tables[0];
                            dtCommon = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                            foreach (DataRow dr in dtCommon.Rows)                            
                                dtReport.Rows.Add(dr.ItemArray);

                            foreach (DataRow dr in dtTemp.Rows)
                                dtReport.Rows.Add(dr.ItemArray);

                            DataTable dtTemp2 = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Effi).Tables[0];

                            foreach (DataRow drEffi in dtTemp2.Rows)
                                dtReport.Rows.Add(drEffi.ItemArray);

                            DataRow[] drsTemp = this.Get(dtime.ToString(), dtime.AddDays(1).ToString(), _locationkey, string.Empty, _protype_notcomma, _proid_notcomma, _wonumber_notcomma);
                            if (dtYieldForCustCheck.Rows.Count < 1 && drsTemp != null && drsTemp.Length > 0)
                                dtYieldForCustCheck = drsTemp[0].Table.Clone();
                            foreach (DataRow dr in drsTemp)
                            {
                                dr["DATA_DATE"] = tmp;
                                dtYieldForCustCheck.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    //----------------------------------------------------------------------------------------------------
                    if (dtEnd > dtEnd_h)
                    {
                        if (dtEnd < Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd") + " 08:00:00"))
                            tmp = dtEnd.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00";
                        else
                            tmp = dtEnd.ToString("yyyy-MM-dd") + " 00:00:00";

                        sqlCommand = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,sum(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE from RPT_WIP_DAILY t 
                                                where t.SEQ NOT LIKE 'D%' AND t.DAILY_DATE between CONVERT(datetime,'{1}') and CONVERT(datetime,'{2}')  
                                                and t.PRO_NAME not in('WEIGHTING_EFFI','WEIGHTING_CTM_COUNT')", tmp, dtEnd_h, dtEnd);

                        sqlCommand_Wip = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,COUNT(t.PRO_NAME) RN,sum(t.SUM_DATA) SUM_DATA,'{0}' DAILY_DATE from RPT_WIP_DAILY t 
                                                where T.SEQ LIKE 'D%' AND t.DAILY_DATE = CONVERT(datetime,'{1}')  ", tmp, dtEnd);

                        sqlCommand_Effi = string.Format(@" SELECT t.PRO_NAME, t.SEQ ,sum(t1.SUM_DATA) RN,sum(t.SUM_DATA*t1.SUM_DATA) SUM_DATA, '{0}' DAILY_DATE 
                                                         from RPT_WIP_DAILY t,RPT_WIP_DAILY t1 
                                                         where   t.LOCATION_KEY=t1.LOCATION_KEY 
                                                         and t.PART_TYPE=t1.PART_TYPE and t.DAILY_DATE=t1.DAILY_DATE
                                                         and t.PRO_ID=t1.PRO_ID and t.WORK_ORDER_NO=t1.WORK_ORDER_NO
                                                         and t.PRO_NAME='WEIGHTING_EFFI'
                                                         and t1.PRO_NAME= 'WEIGHTING_CTM_COUNT'
                                                          and t.DAILY_DATE between CONVERT(datetime,'{1}') and CONVERT(datetime,'{2}') ", tmp, dtEnd_h, dtEnd);

                        if (!string.IsNullOrEmpty(sql))
                        {
                            sqlCommand_Wip += sql;
                            sqlCommand += sql;
                            sqlCommand_Effi += sql;
                        }
                        DataTable dtTemp = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Wip).Tables[0];
                        dtCommon = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                        foreach (DataRow dr in dtCommon.Rows)                        
                            dtReport.Rows.Add(dr.ItemArray);
                        
                        foreach (DataRow dr in dtTemp.Rows)                        
                            dtReport.Rows.Add(dr.ItemArray);

                        DataTable dtTemp2 = _db.ExecuteDataSet(CommandType.Text, sqlCommand_Effi).Tables[0];

                        foreach (DataRow drEffi in dtTemp2.Rows)
                            dtReport.Rows.Add(drEffi.ItemArray);

                        DataRow[] drsTemp = this.Get(dtEnd_h.ToString(), dtEnd.ToString(), _locationkey, string.Empty, _protype_notcomma, _proid_notcomma, _wonumber_notcomma);
                        if (dtYieldForCustCheck.Rows.Count < 1 && drsTemp != null && drsTemp.Length > 0)
                            dtYieldForCustCheck = drsTemp[0].Table.Clone();
                        foreach (DataRow dr in drsTemp)
                        {
                            dr["DATA_DATE"] = tmp;
                            dtYieldForCustCheck.Rows.Add(dr.ItemArray);
                        }
                    }
                }

              
                dtReport.AcceptChanges();


                string sql_all = string.Format(@"select convert(varchar,sum(t.SUM_DATA)) ALL_SUM_DATA,T.SEQ, T.PRO_NAME
                                            from RPT_WIP_DAILY t 
                                            where t.DAILY_DATE between CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}')
                                            ", dtStart, dtEnd);
                if (!string.IsNullOrEmpty(sql))
                    sql_all += sql;
                //sql_all+=@" GROUP BY T.SEQ, T.PRO_NAME ";

                DataTable dtSumfront = _db.ExecuteDataSet(CommandType.Text, sql_all).Tables[0];

                DataTable dtDistinctDatetime = dtReport.DefaultView.ToTable("DailyReport", true, "DAILY_DATE");
                foreach (DataRow drDistinctDatetime in dtDistinctDatetime.Rows)
                {
                    string sday = Convert.ToString(drDistinctDatetime["DAILY_DATE"]);

                    if (!dtSumfront.Columns.Contains(sday))
                        dtSumfront.Columns.Add(sday);

                    DataRow[] drs = dtReport.Select(string.Format("DAILY_DATE='{0}'", sday));
                    foreach (DataRow dr01 in drs)
                    {
                        string columname_report = Convert.ToString(dr01["PRO_NAME"]);
                        string columname_seq = Convert.ToString(dr01["SEQ"]);
                        DataRow[] drs01 = null;
                        if (columname_seq.Trim().Equals(string.Empty))
                        {
                            drs01 = dtSumfront.Select(string.Format("PRO_NAME='{0}' ", columname_report));                            
                        }
                        else
                        {
                             drs01 = dtSumfront.Select(string.Format("PRO_NAME='{0}' and SEQ='{1}' ", columname_report,columname_seq));                        
                        }

                        if (drs01 != null)
                        {
                            drs01[0][sday] = dr01["SUM_DATA"];
                        }
                    }
                }


                if (dtSumfront.Rows.Count < 1)
                {
                    _errorMsg = "没有查询到数据";
                    return;
                }
                dtDailyReport = dtSumfront;

                //在制品分布数据汇总
                #region
                DataRow[] drsDailyReport = dtDailyReport.Select("SEQ LIKE 'D%'");
                foreach (DataRow drDailyReport in drsDailyReport)
                {
                    string seq = Convert.ToString(drDailyReport["SEQ"]);
                    int qty_Wip = 0;
                    DataRow[] drs01 = dtReport.Select(string.Format(@"SEQ='{0}'", seq));
                    foreach (DataRow drWip in drs01)
                    {
                        qty_Wip += Convert.ToInt16(drWip["SUM_DATA"]);
                    }

                    drDailyReport["ALL_SUM_DATA"] = qty_Wip.ToString();
                }
                #endregion

                //DataTable dtCount = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                SetRowEffiData(dtDailyReport, dtReport, dtYieldForCustCheck);

                #region 统计计划数据
                sql = string.Format(@"SELECT  convert(varchar,T.PLAN_DATE_START,111) PLAN_DATE, SUM(T.QUANTITY_INPUT) QUANTITY_INPUT, 
                                        SUM(T.QUANTITY_OUTPUT) QUANTITY_OUTPUT
                                        FROM RPT_PLAN_AIM T
                                        WHERE t.ISFLAG=1 ");
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(" and t.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_locationName))
                    sql += string.Format(" and t.LOCATION_NAME='{0}'", _locationName);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(" and t.PART_TYPE in ({0}) ", _protype);
                if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                    sql += string.Format(" and  CONVERT(varchar, T.PLAN_DATE_START,111)  BETWEEN convert(varchar,convert(datetime,'{0}'),111) AND convert(varchar,convert(datetime,'{1}'),111)", dtStart.ToString(), dtEnd.ToString());

                sql += "   GROUP BY T.PLAN_DATE_START ORDER BY T.PLAN_DATE_START ASC";
                #endregion

                DataTable dtPlan = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //新增投入计划
                #region
                DataRow drDaily = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drDaily[dc.ColumnName] = "PLAN_INPUT";
                    }
                    else if (dc.ColumnName == "SEQ")
                    {
                        drDaily[dc.ColumnName] = "A10";
                    }
                    else if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(Convert.ToString(dr["QUANTITY_INPUT"]) == string.Empty ? "0" : Convert.ToString(dr["QUANTITY_INPUT"]));
                        }
                        drDaily[dc.ColumnName] = v.ToString();
                    }
                    else
                    {
                        string coldate = Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd");

                        DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", coldate));
                        if (drPlans != null && drPlans.Length > 0)
                        {
                            drDaily[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_INPUT"]) == "" ? "0" : Convert.ToString(drPlans[0]["QUANTITY_INPUT"]);
                        }
                    }

                }
                dtDailyReport.Rows.InsertAt(drDaily, 0);
                #endregion

                //新增入库计划
                #region
                DataRow drToStore = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drToStore[dc.ColumnName] = "PLAN_TOSTORE";
                    }
                    else if (dc.ColumnName == "SEQ")
                    {
                        drToStore[dc.ColumnName] = "B10";
                    }
                    else if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(dr["QUANTITY_OUTPUT"].ToString() == string.Empty ? "0" : dr["QUANTITY_OUTPUT"].ToString());
                        }
                        drToStore[dc.ColumnName] = v.ToString();
                    }
                    else
                    {
                        string coldate = Convert.ToDateTime(dc.ColumnName).ToString("yyyy/MM/dd");
                        DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", coldate));
                        if (drPlans != null && drPlans.Length > 0)
                        {
                            drToStore[dc.ColumnName] = Convert.ToString(drPlans[0]["QUANTITY_OUTPUT"]) == "" ? "0" : Convert.ToString(drPlans[0]["QUANTITY_OUTPUT"]);
                        }
                    }
                }
                dtDailyReport.Rows.InsertAt(drToStore, 6);
                #endregion
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            dtDailyReport.TableName = LayoutViewType.ReportTable;
            dsReturn.Merge(dtDailyReport, true, MissingSchemaAction.Add);
            return;
        }

        /// <summary>
        /// 获取指定日期区间的优品率数据。
        /// </summary>
        private DataRow[] Get(string start_time, string end_time, string roomKey,
                            string customer, string productModel, string proId, string workOrderNo)
        {
         
            if (roomKey.ToUpper() == "ALL") roomKey = string.Empty;
            if (workOrderNo.ToUpper() == "ALL") workOrderNo = string.Empty;
            if (productModel.ToUpper() == "ALL") productModel = string.Empty;
            if (customer.ToUpper() == "ALL") customer = string.Empty;

            DataTable dtTemp = new QualityProductDataAccess().Get(0, start_time, end_time, roomKey, customer, productModel, proId, workOrderNo,string.Empty).Tables[0];
        

            DataRow[] drsTemp = dtTemp.Select(@"PROMODEL_NAME='ALL'");

            return drsTemp;
        }

        /// <summary>
        /// 统计效率及比例数据
        /// </summary>
        /// <param name="dtReport"></param>
        /// <param name="dtCount"></param>
        private void SetRowEffiData(DataTable dtReport, DataTable dtCount, DataTable dtYieldForCustCheck)
        {
            decimal effi = 0, ctm = 0, efficount = 0;
            DataRow[] dr_b1511 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_A0J_B1511"));
            DataRow[] dr_b1512 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_AJ_B1512"));
            DataRow[] dr_b1513 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_KJ_B1513"));

            if (dr_b1511 == null || dr_b1511.Length < 1)
            {
                DataRow drnew_a0j = dtReport.NewRow();
                drnew_a0j["PRO_NAME"] = "PER_GR_A0J_B1511";
                drnew_a0j["SEQ"] = "B1511";
                dtReport.Rows.Add(drnew_a0j);
            }
            if (dr_b1512 == null || dr_b1512.Length < 1)
            {
                DataRow drnew_aj = dtReport.NewRow();
                drnew_aj["PRO_NAME"] = "PER_GR_AJ_B1512";
                drnew_aj["SEQ"] = "B1512";
                dtReport.Rows.Add(drnew_aj);
            }
            if (dr_b1513 == null || dr_b1513.Length < 1)
            {
                DataRow drnew_kj = dtReport.NewRow();
                drnew_kj["PRO_NAME"] = "PER_GR_KJ_B1513";
                drnew_kj["SEQ"] = "B1513";
                dtReport.Rows.Add(drnew_kj);
            }

            DataTable dtColumns = dtCount.DefaultView.ToTable("DateTime", true, new string[] { "DAILY_DATE" });
            foreach (DataRow drColumn in dtColumns.Rows)
            {
                string column = Convert.ToString(drColumn[0]).Trim();
                if (string.IsNullOrEmpty(column)) continue;

                column = Convert.ToDateTime(column).ToString("yyyy-MM-dd HH:mm:ss");

                try
                {
                    //转工单
                    #region
                    string rel_input = string.Empty;
                    string rel_exchg_in_qty = string.Empty;
                    string rel_exchg_out_qty = string.Empty;
                    string rel_out = string.Empty;
                    DataRow[] drsinput = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_INPUT"));
                    DataRow[] drinqty = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_EXCHG_IN_QTY"));
                    DataRow[] drsoutqty = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_EXCHG_OUT_QTY"));
                    DataRow[] drsrelout = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_OUT"));

                    if (drsinput != null && drsinput.Length > 0)
                        rel_input = Convert.ToString(drsinput[0][column]);
                    if (drinqty != null && drinqty.Length > 0)
                        rel_exchg_in_qty = Convert.ToString(drinqty[0][column]);
                    if (drsoutqty != null && drsoutqty.Length > 0)
                        rel_exchg_out_qty = Convert.ToString(drsoutqty[0][column]);

                    decimal r_out = Convert.ToDecimal(rel_input == string.Empty ? "0" : rel_input) + Convert.ToDecimal(rel_exchg_in_qty == string.Empty ? "0" : rel_exchg_in_qty) - Convert.ToDecimal(rel_exchg_out_qty == string.Empty ? "0" : rel_exchg_out_qty);
                    rel_out = r_out.ToString();
                    if (drsrelout != null && drsrelout.Length > 0)
                        drsrelout[0][column] = rel_out;
                    #endregion

                }
                catch 
                {
                }
                try
                {
                    //--入库统计数量
                    #region
                    string store = string.Empty;
                    string a0 = string.Empty;
                    string aj = string.Empty;
                    string kj = string.Empty;
                    DataRow[] drsStore = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_TOSTORE_QTY"));
                    DataRow[] drsa0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_A0J"));
                    DataRow[] drsaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_AJ"));
                    DataRow[] drskj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_KJ"));

                    if (drsStore != null && drsStore.Length > 0)
                        store = Convert.ToString(drsStore[0][column]);
                    if (drsa0 != null && drsa0.Length > 0)
                        a0 = Convert.ToString(drsa0[0][column]);
                    if (drsaj != null && drsaj.Length > 0)
                        aj = Convert.ToString(drsaj[0][column]);
                    if (drskj != null && drskj.Length > 0)
                        kj = Convert.ToString(drskj[0][column]);

                    DataRow[] drspera0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_A0J"));
                    DataRow[] drsperaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_AJ"));
                    DataRow[] drsperkj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_KJ"));
                    if (string.IsNullOrEmpty(store) || Convert.ToDecimal(store) < 1)
                        store = "1";

                    if (string.IsNullOrEmpty(kj) || Convert.ToDecimal(kj) < 1)
                        kj = "0";
                    if (drsperkj != null)
                    {
                        drsperkj[0][column] = Math.Round(Convert.ToDecimal(kj) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                    }

                    if (string.IsNullOrEmpty(aj) || Convert.ToDecimal(aj) < 1)
                        aj = "0";
                    if (drsperaj != null)
                    {
                        drsperaj[0][column] = Math.Round((Convert.ToDecimal(aj) + Convert.ToDecimal(kj)) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                    }

                    if (string.IsNullOrEmpty(a0) || Convert.ToDecimal(a0) < 1)
                        a0 = "0";
                    if (drspera0 != null)
                    {
                        drspera0[0][column] = Math.Round((Convert.ToDecimal(a0) + Convert.ToDecimal(aj) + Convert.ToDecimal(kj)) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                    }
                    #endregion
                }
                catch
                {

                }
                try
                {
                    //-----------终检产出数量-----------
                    #region
                    string store = string.Empty;
                    string a0 = string.Empty;
                    string aj = string.Empty;
                    string kj = string.Empty;
                    string erj = string.Empty;
                    string sanj = string.Empty;

                    DataRow[] drspera0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_A0J_B1511"));
                    DataRow[] drsperaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_AJ_B1512"));
                    DataRow[] drsperkj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_KJ_B1513"));

                    if (dtYieldForCustCheck.Rows.Count > 0)
                    {
                        DataRow[] drsYieldForCustCheck = dtYieldForCustCheck.Select(string.Format("DATA_DATE='{0}'", column));
                        DataRow dr = drsYieldForCustCheck[0];

                        double trackoutQty = Convert.ToDouble(dr["TRACKOUT_QTY"]);
                        double kjAjQty = Convert.ToDouble(dr["KJ_AJ_QTY"]);
                        double kjA0jQty = Convert.ToDouble(dr["KJ_A0J_QTY"]);
                        double kjErsanJiQty = Convert.ToDouble(dr["KJ_ERSANJI_QTY"]);
                        double kjScrapQty = Convert.ToDouble(dr["KJ_SCRAP_QTY"]);
                        double kjQty = Convert.ToDouble(dr["KJ_QTY"]);
                        double ajQty = Convert.ToDouble(dr["AJ_QTY"]);

                        double a0jQty = Convert.ToDouble(dr["A0J_QTY"]);
                        double directA0JQty = Convert.ToDouble(dr["DIRECT_A0J_QTY"]);
                        double reworkA0JQty = Convert.ToDouble(dr["REWORK_A0J_QTY"]);
                        double preA0JQty = Convert.ToDouble(dr["PRE_A0J_QTY"]);
                        double ersanjQty = Convert.ToDouble(dr["ERSANJ_QTY"]);
                        double noReworkERSANJQty = Convert.ToDouble(dr["NOREWORK_ERSANJ_QTY"]);
                        double reworkERSANJQty = Convert.ToDouble(dr["REWORK_ERSANJ_QTY"]);
                        double noCYReworkERSANJQty = Convert.ToDouble(dr["NO_CY_REWORK_ERSANJ_QTY"]);
                        double scrapQty = Convert.ToDouble(dr["SCRAP_QTY"]);
                        double noReworkScrapQty = Convert.ToDouble(dr["NOREWORK_SCRAP_QTY"]);
                        double noCYReworkScrapQty = Convert.ToDouble(dr["NO_CY_REWORK_SCRAP_QTY"]);
                        if ((kjQty + kjAjQty + kjA0jQty + kjErsanJiQty + kjScrapQty) > 0)
                        {
                            // "客级以上优品率"
                            //［1-(直判A级+直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%                              
                            double kjUponQualityRate = kjQty / (kjQty + kjAjQty + kjA0jQty + kjErsanJiQty + kjScrapQty);
                            drsperkj[0][column] = kjUponQualityRate;
                        }

                        if (scrapQty + trackoutQty > 0)
                        {                           
                            // "A级以上优品率"
                            //［1-(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%  
                                                    //1 - (directA0JQty + preA0JQty + noCYReworkERSANJQty + noCYReworkScrapQty) / (scrapQty + trackoutQty);
                            double aUponQualityRate = 1 - (directA0JQty + preA0JQty + noCYReworkERSANJQty + noCYReworkScrapQty) / (scrapQty + trackoutQty);
                            drsperaj[0][column] = aUponQualityRate;

                            // "A0级以上优品率"
                            //［1-(二三级+报废）/（入库数+报废数）］*100%    
                                                     //1 - (ersanjQty + scrapQty) / (scrapQty + trackoutQty);
                            double a0UponQualityRate = 1 - (ersanjQty + scrapQty) / (scrapQty + trackoutQty);
                            drspera0[0][column] = a0UponQualityRate;

             
                        }
                    }
                    #endregion
                }
                catch 
                { }

                try
                {
                    //碎片统计
                    #region
                    decimal AllCells = 0;
                    decimal TwoCells = 0;

                    string cell_all = string.Empty;
                    string iqc_cell = string.Empty;
                    string press_cell = string.Empty;

                    DataRow[] drsCrush = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "CELL_ALL_QTY"));
                    DataRow[] drsIqc = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "IQC_CELL_QTY"));
                    DataRow[] drsPress = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PRESS_CELL_QTY"));

                    if (drsCrush != null && drsCrush.Length > 0)
                        cell_all = Convert.ToString(drsCrush[0][column]);
                    if (drsIqc != null && drsIqc.Length > 0)
                        iqc_cell = Convert.ToString(drsIqc[0][column]);
                    if (drsPress != null && drsPress.Length > 0)
                        press_cell = Convert.ToString(drsPress[0][column]);

                    DataRow[] drs_rel_cell_crush = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_CELL_CRUSH"));
                    DataRow[] drs_per_iqc = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_IQC_CELL_CRUSH"));
                    DataRow[] drs_per_press = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_PRESS_CELL_CRUSH"));

                    TwoCells = Convert.ToDecimal(iqc_cell == string.Empty ? "0" : iqc_cell) + Convert.ToDecimal(press_cell == string.Empty ? "0" : press_cell);

                    //AllCells = Convert.ToDecimal(cell_all == string.Empty ? "0" : cell_all) + TwoCells;
                    AllCells = Convert.ToDecimal(cell_all == string.Empty ? "0" : cell_all);
                    if (AllCells == 0)
                        AllCells = 1;
                    if (TwoCells < 1)
                        TwoCells = 0;

                    if (drs_rel_cell_crush != null && drs_rel_cell_crush.Length > 0)
                    {
                        drs_rel_cell_crush[0][column] = Math.Round(TwoCells / AllCells, 4);
                    }

                    if (drs_per_iqc != null && drs_per_iqc.Length > 0)
                    {
                        if (string.IsNullOrEmpty(iqc_cell) || Convert.ToDecimal(iqc_cell) < 1)
                            iqc_cell = "0";
                        drs_per_iqc[0][column] = Math.Round(Convert.ToDecimal(iqc_cell) / AllCells, 4);
                    }

                    if (drs_per_press != null && drs_per_press.Length > 0)
                    {
                        if (string.IsNullOrEmpty(press_cell) || Convert.ToDecimal(press_cell) < 1)
                            press_cell = "0";
                        drs_per_press[0][column] = Math.Round(Convert.ToDecimal(press_cell) / AllCells, 4);
                    }
                    #endregion

                }
                catch 
                { }

                try
                {
                    //计算加权统计
                    #region

                    decimal theorypower = 0, reltotlepower = 0, count = 0, effiTmp = 0;
                    //理论功率
                    DataRow[] drs01 = dtCount.Select(string.Format("PRO_NAME='{0}' AND DAILY_DATE='{1}'", "THEORYTOTLEPOWER", column));
                    //测试功率
                    DataRow[] drs02 = dtCount.Select(string.Format("PRO_NAME='{0}' AND DAILY_DATE='{1}'", "RELTOTLEPOWER", column));
                    //效率及效率总和数
                    DataRow[] drsEffi = dtCount.Select(string.Format("PRO_NAME='{0}' AND DAILY_DATE='{1}'", "WEIGHTING_EFFI", column));

                    if (drs01 != null && drs01.Length > 0)
                        theorypower = Convert.ToDecimal(drs01[0]["SUM_DATA"]);
                    if (drs02 != null && drs02.Length > 0)
                        reltotlepower = Convert.ToDecimal(drs02[0]["SUM_DATA"]);
                    if (drsEffi != null && drsEffi.Length > 0)
                    {
                        count = Convert.ToDecimal(drsEffi[0]["RN"]);
                        effiTmp = Convert.ToDecimal(drsEffi[0]["SUM_DATA"]);

                        efficount += count;
                        effi += effiTmp;
                    }
                    
                    DataRow[] drs03 = dtReport.Select(string.Format("PRO_NAME='{0}'", "WEIGHTING_CTM"));
                    if (drs03 != null && drs03.Length > 0)
                    {
                        ctm = Math.Round(reltotlepower / (theorypower == 0 ? 1 : theorypower), 4);

                        drs03[0][column] = ctm;
                    }

                    DataRow[] drs04 = dtReport.Select(string.Format("PRO_NAME='{0}'", "WEIGHTING_EFFI"));
                    if (drs04 != null && drs04.Length > 0)
                    {                      
                        drs04[0][column] = Math.Round(effiTmp / (count == 0 ? 1 : count * 100), 4);
                    }
                  
                    #endregion
                }
                catch
                { }
            }

            string col = "ALL_SUM_DATA";
            try
            {
                //转工单
                #region
                string rel_input = string.Empty;
                string rel_exchg_in_qty = string.Empty;
                string rel_exchg_out_qty = string.Empty;
                string rel_out = string.Empty;
                DataRow[] drsinput = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_INPUT"));
                DataRow[] drinqty = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_EXCHG_IN_QTY"));
                DataRow[] drsoutqty = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_EXCHG_OUT_QTY"));
                DataRow[] drsrelout = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_OUT"));

                if (drsinput != null && drsinput.Length > 0)
                    rel_input = Convert.ToString(drsinput[0][col]);
                if (drinqty != null && drinqty.Length > 0)
                    rel_exchg_in_qty = Convert.ToString(drinqty[0][col]);
                if (drsoutqty != null && drsoutqty.Length > 0)
                    rel_exchg_out_qty = Convert.ToString(drsoutqty[0][col]);

                decimal r_out = Convert.ToDecimal(rel_input == string.Empty ? "0" : rel_input) + Convert.ToDecimal(rel_exchg_in_qty == string.Empty ? "0" : rel_exchg_in_qty) - Convert.ToDecimal(rel_exchg_out_qty == string.Empty ? "0" : rel_exchg_out_qty);
                rel_out = r_out.ToString();
                if (drsrelout != null && drsrelout.Length > 0)
                    drsrelout[0][col] = rel_out;
                #endregion

            }
            catch 
            {
            }
            try
            {
                //加权计算
                #region

                decimal theorypower = 0, reltotlepower = 0, count = 0;
                //理论功率
                DataRow[] drs01 = dtReport.Select(string.Format("PRO_NAME='{0}'", "THEORYTOTLEPOWER"));
                //测试功率
                DataRow[] drs02 = dtReport.Select(string.Format("PRO_NAME='{0}'", "RELTOTLEPOWER"));

               
                if (drs01 != null && drs01.Length > 0)
                    theorypower = Convert.ToDecimal(drs01[0]["ALL_SUM_DATA"]);
                if (drs02 != null && drs02.Length > 0)
                {                    
                    reltotlepower = Convert.ToDecimal(drs02[0]["ALL_SUM_DATA"]);
                }
                DataRow[] drs03 = dtReport.Select(string.Format("PRO_NAME='{0}'", "WEIGHTING_CTM"));
                if (drs03 != null && drs03.Length > 0)
                {
                    //if (dtReport.Columns.Contains("RN"))
                    //    count = Convert.ToDecimal(drs02[0]["RN"]);
                    //else
                    //{
                    //    DataRow[] drsCount01 = dtCount.Select(string.Format("PRO_NAME='{0}'", "RELTOTLEPOWER"));
                    //    if (drs01 != null && drs01.Length > 0)
                    //    {
                    //        foreach (DataRow drCount in drsCount01)
                    //        {
                    //            count += Convert.ToInt16(drCount["RN"]);
                    //        }
                    //    }
                    //}
                    ctm = Math.Round(reltotlepower / (theorypower == 0 ? 1 : theorypower), 4);

                    drs03[0][col] = ctm;
                }

                DataRow[] drs04 = dtReport.Select(string.Format("PRO_NAME='{0}'", "WEIGHTING_EFFI"));
                if (drs04 != null && drs04.Length > 0)
                {
                    drs04[0][col] = Math.Round(effi / (efficount == 0 ? 1 : efficount * 100), 4);
                }
              
                #endregion
            }
            catch { }
            try
            {
                //碎片率统计
                #region
                decimal AllCells = 0;
                decimal TwoCells = 0;

                string cell_all = string.Empty;
                string iqc_cell = string.Empty;
                string press_cell = string.Empty;

                DataRow[] drsCrush = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "CELL_ALL_QTY"));
                DataRow[] drsIqc = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "IQC_CELL_QTY"));
                DataRow[] drsPress = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PRESS_CELL_QTY"));

                if (drsCrush != null && drsCrush.Length > 0)
                    cell_all = Convert.ToString(drsCrush[0][col]);
                if (drsIqc != null && drsIqc.Length > 0)
                    iqc_cell = Convert.ToString(drsIqc[0][col]);
                if (drsPress != null && drsPress.Length > 0)
                    press_cell = Convert.ToString(drsPress[0][col]);

                DataRow[] drs_rel_cell_crush = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_CELL_CRUSH"));
                DataRow[] drs_per_iqc = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_IQC_CELL_CRUSH"));
                DataRow[] drs_per_press = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_PRESS_CELL_CRUSH"));

                TwoCells = Convert.ToDecimal(iqc_cell == string.Empty ? "0" : iqc_cell) + Convert.ToDecimal(press_cell == string.Empty ? "0" : press_cell);

                //AllCells = Convert.ToDecimal(cell_all == string.Empty ? "0" : cell_all) + TwoCells;
                AllCells = Convert.ToDecimal(cell_all == string.Empty ? "0" : cell_all);
                if (AllCells == 0)
                    AllCells = 1;
                if (TwoCells < 1)
                    TwoCells = 0;

                if (drs_rel_cell_crush != null && drs_rel_cell_crush.Length > 0)
                {
                    drs_rel_cell_crush[0][col] = Math.Round(TwoCells / AllCells, 4);
                }

                if (drs_per_iqc != null && drs_per_iqc.Length > 0)
                {
                    if (string.IsNullOrEmpty(iqc_cell) || Convert.ToDecimal(iqc_cell) < 1)
                        iqc_cell = "0";
                    drs_per_iqc[0][col] = Math.Round(Convert.ToDecimal(iqc_cell) / AllCells, 4);
                }

                if (drs_per_press != null && drs_per_press.Length > 0)
                {
                    if (string.IsNullOrEmpty(press_cell) || Convert.ToDecimal(press_cell) < 1)
                        press_cell = "0";
                    drs_per_press[0][col] = Math.Round(Convert.ToDecimal(press_cell) / AllCells, 4);
                }
                #endregion
            }
            catch { }
            try
            {
                //--入库统计数量
                #region
                string store = string.Empty;
                string a0 = string.Empty;
                string aj = string.Empty;
                string kj = string.Empty;
                DataRow[] drsStore = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_TOSTORE_QTY"));
                DataRow[] drsa0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_A0J"));
                DataRow[] drsaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_AJ"));
                DataRow[] drskj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_KJ"));

                if (drsStore != null && drsStore.Length > 0)
                    store = Convert.ToString(drsStore[0][col]);
                if (drsa0 != null && drsa0.Length > 0)
                    a0 = Convert.ToString(drsa0[0][col]);
                if (drsaj != null && drsaj.Length > 0)
                    aj = Convert.ToString(drsaj[0][col]);
                if (drskj != null && drskj.Length > 0)
                    kj = Convert.ToString(drskj[0][col]);

                DataRow[] drspera0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_A0J"));
                DataRow[] drsperaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_AJ"));
                DataRow[] drsperkj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_KJ"));
                if (string.IsNullOrEmpty(store) || Convert.ToDecimal(store) < 1)
                    store = "1";

                if (string.IsNullOrEmpty(kj) || Convert.ToDecimal(kj) < 1)
                    kj = "0";
                if (drsperkj != null)
                {
                    drsperkj[0][col] = Math.Round(Convert.ToDecimal(kj) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                }

                if (string.IsNullOrEmpty(aj) || Convert.ToDecimal(aj) < 1)
                    aj = "0";
                if (drsperaj != null)
                {
                    drsperaj[0][col] = Math.Round((Convert.ToDecimal(aj) + Convert.ToDecimal(kj)) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                }

                if (string.IsNullOrEmpty(a0) || Convert.ToDecimal(a0) < 1)
                    a0 = "0";
                if (drspera0 != null)
                {
                    drspera0[0][col] = Math.Round((Convert.ToDecimal(a0) + Convert.ToDecimal(aj) + Convert.ToDecimal(kj)) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                }
                #endregion
            }
            catch { }

            try
            {
                //-----------终检产出数量-----------
                #region

                DataRow[] drspera0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_A0J_B1511"));
                DataRow[] drsperaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_AJ_B1512"));
                DataRow[] drsperkj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_KJ_B1513"));

                //--------------------------------------------------------------

                DataRow[] drsTemp = this.Get(Daily_Start_Time + " " + Start_Time, Daily_End_Time + " " + End_Time, 
                    _locationkey, string.Empty, _protype_notcomma, _proid_notcomma, _wonumber_notcomma);

                //-------------------------------------------------------------

                DataRow dr = drsTemp[0];

                double trackoutQty = Convert.ToDouble(dr["TRACKOUT_QTY"]);
                double kjAjQty = Convert.ToDouble(dr["KJ_AJ_QTY"]);
                double kjA0jQty = Convert.ToDouble(dr["KJ_A0J_QTY"]);
                double kjErsanJiQty = Convert.ToDouble(dr["KJ_ERSANJI_QTY"]);
                double kjScrapQty = Convert.ToDouble(dr["KJ_SCRAP_QTY"]);
                double kjQty = Convert.ToDouble(dr["KJ_QTY"]);
                double ajQty = Convert.ToDouble(dr["AJ_QTY"]);

                double a0jQty = Convert.ToDouble(dr["A0J_QTY"]);
                double directA0JQty = Convert.ToDouble(dr["DIRECT_A0J_QTY"]);
                double reworkA0JQty = Convert.ToDouble(dr["REWORK_A0J_QTY"]);
                double preA0JQty = Convert.ToDouble(dr["PRE_A0J_QTY"]);
                double ersanjQty = Convert.ToDouble(dr["ERSANJ_QTY"]);
                double noReworkERSANJQty = Convert.ToDouble(dr["NOREWORK_ERSANJ_QTY"]);
                double reworkERSANJQty = Convert.ToDouble(dr["REWORK_ERSANJ_QTY"]);
                double noCYReworkERSANJQty = Convert.ToDouble(dr["NO_CY_REWORK_ERSANJ_QTY"]);
                double scrapQty = Convert.ToDouble(dr["SCRAP_QTY"]);
                double noReworkScrapQty = Convert.ToDouble(dr["NOREWORK_SCRAP_QTY"]);
                double noCYReworkScrapQty = Convert.ToDouble(dr["NO_CY_REWORK_SCRAP_QTY"]);
                if ((kjQty + kjAjQty + kjA0jQty + kjErsanJiQty + kjScrapQty) > 0)
                {
                    // "客级以上优品率"
                    //［1-(直判A级+直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%                              
                    double kjUponQualityRate = kjQty / (kjQty + kjAjQty + kjA0jQty + kjErsanJiQty + kjScrapQty);
                    drsperkj[0][col] = kjUponQualityRate;
                }

                if (scrapQty + trackoutQty > 0)
                {
                    // "A级以上优品率"
                    //［1-(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%  
                    //1 - (directA0JQty + preA0JQty + noCYReworkERSANJQty + noCYReworkScrapQty) / (scrapQty + trackoutQty);
                    double aUponQualityRate = 1 - (directA0JQty + preA0JQty + noCYReworkERSANJQty + noCYReworkScrapQty) / (scrapQty + trackoutQty);
                    drsperaj[0][col] = aUponQualityRate;

                    // "A0级以上优品率"
                    //［1-(二三级+报废）/（入库数+报废数）］*100%    
                    //1 - (ersanjQty + scrapQty) / (scrapQty + trackoutQty);
                    double a0UponQualityRate = 1 - (ersanjQty + scrapQty) / (scrapQty + trackoutQty);
                    drspera0[0][col] = a0UponQualityRate;


                }

                #endregion
            }
            catch { }

            try
            {
                //优品率
                #region

                string store = string.Empty;
                string a0 = string.Empty;
                string aj = string.Empty;
                string kj = string.Empty;
                DataRow[] drsStore = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "REL_TOSTORE_QTY"));
                DataRow[] drsa0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_A0J"));
                DataRow[] drsaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_AJ"));
                DataRow[] drskj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "TO_STORE_KJ"));

                if (drsStore != null && drsStore.Length > 0)
                    store = Convert.ToString(drsStore[0][col]);
                if (drsa0 != null && drsa0.Length > 0)
                    a0 = Convert.ToString(drsa0[0][col]);
                if (drsaj != null && drsaj.Length > 0)
                    aj = Convert.ToString(drsaj[0][col]);
                if (drskj != null && drskj.Length > 0)
                    kj = Convert.ToString(drskj[0][col]);

                DataRow[] drspera0 = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_A0J"));
                DataRow[] drsperaj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_AJ"));
                DataRow[] drsperkj = dtReport.Select(string.Format(@"PRO_NAME='{0}'", "PER_GR_KJ"));
                if (string.IsNullOrEmpty(store) || Convert.ToDecimal(store) < 1)
                    store = "1";

                if (string.IsNullOrEmpty(kj) || Convert.ToDecimal(kj) < 1)
                    kj = "0";
                if (drsperkj != null)
                {
                    drsperkj[0][col] = Math.Round(Convert.ToDecimal(kj) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                }

                if (string.IsNullOrEmpty(aj) || Convert.ToDecimal(aj) < 1)
                    aj = "0";
                if (drsperaj != null)
                {
                    drsperaj[0][col] = Math.Round((Convert.ToDecimal(aj) + Convert.ToDecimal(kj)) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                }

                if (string.IsNullOrEmpty(a0) || Convert.ToDecimal(a0) < 1)
                    a0 = "0";
                if (drspera0 != null)
                {
                    drspera0[0][col] = Math.Round((Convert.ToDecimal(a0) + Convert.ToDecimal(aj) + Convert.ToDecimal(kj)) / Convert.ToDecimal(store == string.Empty ? "0" : store), 4);
                }
                #endregion

            }
            catch { }

            //统计档位分布汇总
            #region
            DataRow[] drProIds = dtReport.Select("SEQ='B222' OR SEQ='B233'");
            if (drProIds != null && drProIds.Length > 0)
            {
                foreach (DataRow drProId in drProIds)
                {
                    decimal qtyAllProid = 0;
                    foreach (DataColumn dc in dtReport.Columns)
                    {
                        if (dc.ColumnName == "PRO_NAME" || dc.ColumnName == "SEQ" || dc.ColumnName == "ALL_SUM_DATA" || dc.ColumnName == "RN")
                            continue;
                        qtyAllProid += Convert.ToDecimal(Convert.ToString( drProId[dc.ColumnName]) == string.Empty ? 0 : drProId[dc.ColumnName]);
                    }
                    drProId["ALL_SUM_DATA"] = qtyAllProid;
                }
            }
            #endregion

            DataRow[] drsDel = dtReport.Select("SEQ=''");
            if (drsDel != null)
            {
                foreach (DataRow drDel in drsDel)
                {
                    string proname = Convert.ToString(drDel["PRO_NAME"]);
                    if (!proname.Equals("CELL_ALL_QTY") && !proname.Equals("IQC_CELL_QTY") && !proname.Equals("PRESS_CELL_QTY"))
                        dtReport.Rows.Remove(drDel);
                }
            }
            //DataView dv = dtReport.DefaultView;
            //dv.Sort = " SEQ ASC";
            //dtReport = dv.ToTable();

        }

      

        public DataSet GetDailyForPatchData()
        {
            string sqlCommand = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                sqlCommand = string.Format(@"select t.REASON_CODE_NAME,SUM(T.PATCH_QUANTITY) PATCH_QUANTITY
                                            from (
                                            select distinct t.PATCHED_TRANSACTION_KEY,t.TRANSACTION_KEY,t.PATCH_LOT_KEY,t.REASON_CODE_NAME,t.PATCH_QUANTITY
                                            from WIP_PATCH t inner join  WIP_TRANSACTION t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                            inner join POR_LOT t2 on t1.PIECE_KEY=t2.LOT_KEY
                                            left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                        where  t1.TIME_STAMP between CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}') ", Start_Time, End_Time);

                if (!string.IsNullOrEmpty(_wonumber))
                    sqlCommand += string.Format(" and t2.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_pro_id))
                    sqlCommand += string.Format(" and t2.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_protype))
                    sqlCommand += string.Format(" and t3.PROMODEL_NAME in ({0})", _protype);
                if (!string.IsNullOrEmpty(_locationkey))
                    sqlCommand += string.Format(" and t2.FACTORYROOM_KEY='{0}'", _locationkey);
                if (!string.IsNullOrEmpty(_reason_code_class))
                    sqlCommand += string.Format(" and t.REASON_CODE_CLASS='{0}'", _reason_code_class);
                else
                    sqlCommand += " and t.REASON_CODE_CLASS in ('19','21')";

                sqlCommand += " ) t group by  t.REASON_CODE_NAME ";

                DataTable dtPatchData = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dsReturn.Merge(dtPatchData, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }

            return dsReturn;
        }


        /// <summary>
        /// 查询优品率数据之后进行数据的行列转换。
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
                    if (key.StartsWith("LJ_") && (queryType == 0 || queryType == 3))
                    {
                        continue;
                    }
                    DataRow drNew = dtNew.NewRow();
                    drNew["KEY_VALUE"] = string.Format("{0}${1}", proModelName, key);
                    drNew["PROMODEL_NAME"] = proModelName;
                    drNew["COL_VALUE"] = key;
                    for (int j = dtBase.Columns.Count + 1; j < dtNew.Columns.Count - 1; j++)
                    {
                        string dataDate = dtNew.Columns[j].ColumnName;
                        string filter = string.Format("PROMODEL_NAME='{0}' AND DATA_DATE='{1}'", proModelName, dataDate);
                        DataRow[] drs = dtSource.Select(filter);
                        if (drs.Length > 0)
                        {
                            double kjQty = Convert.ToDouble(drs[0]["KJ_QTY"]);
                            double trackoutQty = Convert.ToDouble(drs[0]["TRACKOUT_QTY"]);
                            double a0jQty = Convert.ToDouble(drs[0]["A0J_QTY"]);
                            double directA0JQty = Convert.ToDouble(drs[0]["DIRECT_A0J_QTY"]);
                            double reworkA0JQty = Convert.ToDouble(drs[0]["REWORK_A0J_QTY"]);
                            double preA0JQty = Convert.ToDouble(drs[0]["PRE_A0J_QTY"]);

                            double ersanjQty = Convert.ToDouble(drs[0]["ERSANJ_QTY"]);
                            double noReworkERSANJQty = Convert.ToDouble(drs[0]["NOREWORK_ERSANJ_QTY"]);
                            double reworkERSANJQty = Convert.ToDouble(drs[0]["REWORK_ERSANJ_QTY"]);
                            double noCYReworkERSANJQty = Convert.ToDouble(drs[0]["NO_CY_REWORK_ERSANJ_QTY"]);

                            double scrapQty = Convert.ToDouble(drs[0]["SCRAP_QTY"]);
                            double noReworkScrapQty = Convert.ToDouble(drs[0]["NOREWORK_SCRAP_QTY"]);
                            double noCYReworkScrapQty = Convert.ToDouble(drs[0]["NO_CY_REWORK_SCRAP_QTY"]);

                            if (scrapQty + trackoutQty > 0)
                            {
                                if (key == "TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = trackoutQty;
                                }
                                else if (key == "A0J_QTY")
                                {
                                    drNew[dataDate] = a0jQty;
                                }
                                else if (key == "DIRECT_A0J_QTY")
                                {
                                    drNew[dataDate] = directA0JQty;
                                }
                                else if (key == "REWORK_A0J_QTY")
                                {
                                    drNew[dataDate] = reworkA0JQty;
                                }
                                else if (key == "PRE_A0J_QTY")
                                {
                                    drNew[dataDate] = preA0JQty;
                                }
                                else if (key == "ERSANJ_QTY")
                                {
                                    drNew[dataDate] = ersanjQty;
                                }
                                else if (key == "NOREWORK_ERSANJ_QTY")
                                {
                                    drNew[dataDate] = noReworkERSANJQty;
                                }
                                else if (key == "REWORK_ERSANJ_QTY")
                                {
                                    drNew[dataDate] = reworkERSANJQty;
                                }
                                else if (key == "NO_CY_REWORK_ERSANJ_QTY")
                                {
                                    drNew[dataDate] = noCYReworkERSANJQty;
                                }
                                else if (key == "SCRAP_QTY")
                                {
                                    drNew[dataDate] = scrapQty;
                                }
                                else if (key == "NOREWORK_SCRAP_QTY")
                                {
                                    drNew[dataDate] = noReworkScrapQty;
                                }
                                else if (key == "NO_CY_REWORK_SCRAP_QTY")
                                {
                                    drNew[dataDate] = noCYReworkScrapQty;
                                }
                                //{"A_UPON_QUALITY_RATE",                     "A级以上优品率"},
                                //［1-(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%
                                else if (key == "A_UPON_QUALITY_RATE")
                                {
                                    double aUponQualityRate = 1 - (directA0JQty + preA0JQty + noCYReworkERSANJQty + noCYReworkScrapQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = aUponQualityRate;
                                }
                                //{"A0_UPON_QUALITY_RATE",                    "A0级以上优品率"},
                                //［1-(二三级+报废）/（入库数+报废数）］*100%
                                else if (key == "A0_UPON_QUALITY_RATE")
                                {
                                    double a0UponQualityRate = 1 - (ersanjQty + scrapQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = a0UponQualityRate;
                                }
                                //{"A0_ERSANJ_QUALITY_RATE",                  "A0、二三级品率"},
                                //（A0级+二三级）/（入库数+报废数）
                                else if (key == "A0_ERSANJ_QUALITY_RATE")
                                {
                                    double a0ERSANJQualityRate = (a0jQty + ersanjQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = a0ERSANJQualityRate;
                                }
                                //{"REWORK_A0_ERSANJ_QUALITY_RATE",           "返修后A0、二三级品率"},
                                //（返修后A0级+返修后二三级）/（入库数+报废数）
                                else if (key == "REWORK_A0_ERSANJ_QUALITY_RATE")
                                {
                                    double reworkA0ERSANJQualityRate = (reworkA0JQty + reworkERSANJQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = reworkA0ERSANJQualityRate;
                                }
                                //{"PASS_RATE",                               "合格率"},
                                //[1-报废数/(入库数+报废数)]*100%
                                else if (key == "PASS_RATE")
                                {
                                    double passRate = 1 - scrapQty / (scrapQty + trackoutQty);
                                    drNew[dataDate] = passRate;
                                }
                                //报废数/(入库数+报废数)
                                else if (key == "SCRAP_RATE")
                                {
                                    double rate = scrapQty / (scrapQty + trackoutQty);
                                    drNew[dataDate] = rate;
                                }
                            }
                            //LJ_TRACKOUT_QTY,
                            //LJ_A0J_QTY, LJ_DIRECT_A0J_QTY,LJ_REWORK_A0J_QTY,LJ_PRE_A0J_QTY,
                            //LJ_ERSANJ_QTY,LJ_NOREWORK_ERSANJ_QTY,LJ_REWORK_ERSANJ_QTY,LJ_NO_CY_REWORK_ERSANJ_QTY,
                            //SCRAP_QTY, LJ_SCRAP_QTY, LJ_NOREWORK_SCRAP_QTY, LJ_NO_CY_REWORK_SCRAP_QTY
                            if (queryType == 1 || queryType == 2)
                            {
                                double ljTrackoutQty = Convert.ToDouble(drs[0]["LJ_TRACKOUT_QTY"]);
                                double ljA0jQty = Convert.ToDouble(drs[0]["LJ_A0J_QTY"]);
                                double ljDirectA0JQty = Convert.ToDouble(drs[0]["LJ_DIRECT_A0J_QTY"]);
                                double ljReworkA0JQty = Convert.ToDouble(drs[0]["LJ_REWORK_A0J_QTY"]);
                                double ljPreA0JQty = Convert.ToDouble(drs[0]["LJ_PRE_A0J_QTY"]);

                                double ljErsanjQty = Convert.ToDouble(drs[0]["LJ_ERSANJ_QTY"]);
                                double ljNoReworkERSANJQty = Convert.ToDouble(drs[0]["LJ_NOREWORK_ERSANJ_QTY"]);
                                double ljReworkERSANJQty = Convert.ToDouble(drs[0]["LJ_REWORK_ERSANJ_QTY"]);
                                double ljNoCYReworkERSANJQty = Convert.ToDouble(drs[0]["LJ_NO_CY_REWORK_ERSANJ_QTY"]);

                                double ljScrapQty = Convert.ToDouble(drs[0]["LJ_SCRAP_QTY"]);
                                double ljNoReworkScrapQty = Convert.ToDouble(drs[0]["LJ_NOREWORK_SCRAP_QTY"]);
                                double ljNoCYReworkScrapQty = Convert.ToDouble(drs[0]["LJ_NO_CY_REWORK_SCRAP_QTY"]);

                                if (ljTrackoutQty + ljScrapQty > 0)
                                {
                                    if (key == "LJ_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljTrackoutQty;
                                    }
                                    else if (key == "LJ_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljA0jQty;
                                    }
                                    else if (key == "LJ_DIRECT_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljDirectA0JQty;
                                    }
                                    else if (key == "LJ_REWORK_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljReworkA0JQty;
                                    }
                                    else if (key == "LJ_PRE_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljPreA0JQty;
                                    }
                                    else if (key == "LJ_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljErsanjQty;
                                    }
                                    else if (key == "LJ_NOREWORK_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljNoReworkERSANJQty;
                                    }
                                    else if (key == "LJ_REWORK_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljReworkERSANJQty;
                                    }
                                    else if (key == "LJ_NO_CY_REWORK_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljNoCYReworkERSANJQty;
                                    }
                                    else if (key == "LJ_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljScrapQty;
                                    }
                                    else if (key == "LJ_NOREWORK_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljNoReworkScrapQty;
                                    }
                                    else if (key == "LJ_NO_CY_REWORK_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljNoCYReworkScrapQty;
                                    }
                                    //{"LJ_A_UPON_QUALITY_RATE",                  "累计 A级以上优品率"},
                                    //1-[累计(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/累计（入库数+报废数）]
                                    else if (key == "LJ_A_UPON_QUALITY_RATE")
                                    {
                                        double ljAUponQualityRate = 1 - (ljDirectA0JQty + ljPreA0JQty + ljNoCYReworkERSANJQty + ljNoCYReworkScrapQty) / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljAUponQualityRate;
                                    }
                                    //{"LJ_A0_UPON_QUALITY_RATE",                 "累计 A0级以上优品率"},
                                    //1-[累计(二三级+报废）/累计（入库数+报废数）]
                                    else if (key == "LJ_A0_UPON_QUALITY_RATE")
                                    {
                                        double ljA0UponQualityRate = 1 - (ljErsanjQty + ljScrapQty) / (ljScrapQty + ljTrackoutQty);
                                        drNew[dataDate] = ljA0UponQualityRate;
                                    }
                                    //{"LJ_A0_ERSANJ_QUALITY_RATE",               "累计A0、二三级品率"},
                                    //[累计（A0级+二三级）/累计（入库数+报废数）]
                                    else if (key == "LJ_A0_ERSANJ_QUALITY_RATE")
                                    {
                                        double ljA0ERSANJQualityRate = (ljA0jQty + ljErsanjQty) / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljA0ERSANJQualityRate;
                                    }
                                    //{"LJ_REWORK_A0_ERSANJ_QUALITY_RATE",        "累计返修后A0、二三级品率"},
                                    //[累计（返修后A0级+返修后二三级）/累计（入库数+报废数）]
                                    else if (key == "LJ_REWORK_A0_ERSANJ_QUALITY_RATE")
                                    {
                                        double ljReworkA0ERSANJQualityRate = (ljReworkA0JQty + ljReworkERSANJQty) / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljReworkA0ERSANJQualityRate;
                                    }
                                    //{"LJ_PASS_RATE",                            "累计合格率"}
                                    else if (key == "LJ_PASS_RATE")
                                    {
                                        double ljPassRate = 1 - ljScrapQty / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljPassRate;
                                    }
                                    //累计报废数/累计（入库数+报废数）
                                    else if (key == "LJ_SCRAP_RATE")
                                    {
                                        double rate = ljScrapQty / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = rate;
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
        /// <summary>
        /// 获得报废品数据
        /// </summary>
        /// <returns></returns>
        private DataSet GetScrashData()
        {
            string sqlCommand = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                sqlCommand = string.Format(@"select t.WORK_ORDER_NO,t.PRO_ID,t.LOT_NUMBER,t.LOT_CUSTOMERCODE,t.LOT_SIDECODE ,
                                            t.PALLET_NO,t.PALLET_TIME,t1.CHECK_POWER,t1.LOT_COLOR,t1.CREATER,t3.EQUIPMENT_NAME
                                            from POR_LOT t inner join WIP_CUSTCHECK t1 on t.LOT_NUMBER=t1.CC_FCODE1
                                            inner join POR_PRODUCT t2 on t.PRO_ID=t2.PRODUCT_CODE
                                            left join EMS_EQUIPMENTS t3 on t1.DEVICENUM=t3.EQUIPMENT_KEY
                                            where t1.CC_DATA_GROUP='1'
                                            and t1.CREATE_TIME between convert(datetime,'{0}') and  convert(datetime,'{1}') ", Start_Time, End_Time);

                if (string.IsNullOrEmpty(_wonumber))
                    sqlCommand += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                if (string.IsNullOrEmpty(_pro_id))
                    sqlCommand += string.Format(" and t.PRO_ID in ({0}) ", _pro_id);
                if (string.IsNullOrEmpty(_protype))
                    sqlCommand += string.Format(" and t2.PROMODEL_NAME in ({0})", _protype);
                if (string.IsNullOrEmpty(_locationkey))
                    sqlCommand += string.Format(" and t1.ROOM_KEY in ({0})", _locationkey);
                if (!string.IsNullOrEmpty(_grade))
                {
                    if (_grade.Equals("Grade_KJ") || _grade.Equals("Grade_AJ") || _grade.Equals("Grade_A0J"))
                        sqlCommand += string.Format(" and t1.PRO_LEVEL='{0}'", _grade);
                    if (_grade.Equals("Grade_ERJ"))
                        sqlCommand += string.Format(" and t1.PRO_LEVEL IN ('Grade_ERJ_WG','Grade_ERJ_XN')");
                    if (_grade.Equals("Grade_SANJ"))
                        sqlCommand += string.Format(" and t1.PRO_LEVEL IN ('Grade_SANJ_WG','Grade_SANJ_XN')");

                }

                DataTable dtPatchData = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dsReturn.Merge(dtPatchData, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }

            return dsReturn;
        }
        /// <summary>
        /// 获得终检等级的数据
        /// </summary>
        /// <returns></returns>
        private DataSet GetMoveOutData(int i)
        {
            string sqlCommand = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                //终检
                if (i == 0)
                {
                    sqlCommand = string.Format(@"select ROW_NUMBER() OVER( ORDER BY a.CREATE_TIME asc) SEQ,a.*
                                                from (select distinct t1.CREATE_TIME, t.WORK_ORDER_NO,t.PRO_ID,t.LOT_NUMBER,t.LOT_CUSTOMERCODE,t.LOT_SIDECODE ,
                                                t1.CREATE_TIME CUSTCHECK_TIME,t.PALLET_NO,t.PALLET_TIME,t1.CHECK_POWER,t1.LOT_COLOR,
                                                case T1.PRO_LEVEL when 'Grade_AJ' then N'A级'
                                                when 'Grade_A0J' then N'A0级'
                                                when 'Grade_KJ' then N'客级'
                                                when 'Grade_ERJ_WG' then N'二级'
                                                when 'Grade_ERJ_XN' then N'二级'
                                                when 'Grade_SANJ_WG' THEN N'三级'
                                                when 'Grade_SANJ_XN' THEN N'三级'
                                                else '' end PRO_LEVEL,
                                                t1.OPERATERS,t3.EQUIPMENT_NAME
                                                from POR_LOT t inner join WIP_CUSTCHECK t1 on t.LOT_NUMBER=t1.CC_FCODE1
                                                inner join POR_PRODUCT t2 on t.PRO_ID=t2.PRODUCT_CODE
                                                left join EMS_EQUIPMENTS t3 on t1.DEVICENUM=t3.EQUIPMENT_KEY
                                                where t1.CC_DATA_GROUP='1'
                                                and t1.CREATE_TIME between convert(datetime,'{0}') and  convert(datetime,'{1}') ", Start_Time, End_Time);

                    if (!string.IsNullOrEmpty(_wonumber))
                        sqlCommand += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                    if (!string.IsNullOrEmpty(_pro_id))
                        sqlCommand += string.Format(" and t.PRO_ID in ({0}) ", _pro_id);
                    if (!string.IsNullOrEmpty(_protype))
                        sqlCommand += string.Format(" and t2.PROMODEL_NAME in ({0})", _protype);
                    if (!string.IsNullOrEmpty(_locationkey))
                        sqlCommand += string.Format(" and t1.ROOM_KEY in ({0})", _locationkey);
                    if (!string.IsNullOrEmpty(_grade))
                    {
                        if (_grade.Equals("Grade_KJ") || _grade.Equals("Grade_AJ") || _grade.Equals("Grade_A0J"))
                            sqlCommand += string.Format(" and t1.PRO_LEVEL='{0}'", _grade);
                        if (_grade.Equals("Grade_ERJ"))
                            sqlCommand += string.Format(" and t1.PRO_LEVEL IN ('Grade_ERJ_WG','Grade_ERJ_XN')");
                        if (_grade.Equals("Grade_SANJ"))
                            sqlCommand += string.Format(" and t1.PRO_LEVEL IN ('Grade_SANJ_WG','Grade_SANJ_XN')");

                    }

                    sqlCommand += " ) a order by a.CREATE_TIME asc  ";
                }
                //入库
                if (i == 1)
                {
                    sqlCommand = string.Format(@"select ROW_NUMBER() OVER( ORDER BY A.TO_WH_TIME asc) SEQ, A.* from (
                                                SELECT distinct  T.LOT_NUMBER,T.LOT_CUSTOMERCODE,
                                                T.LOT_SIDECODE,T.PRO_ID,T.WORK_ORDER_NO,
                                                t3.CHECK_POWER,T1.TO_WH,T1.TO_WH_TIME,T1.SAP_NO,
                                                T1.AVG_POWER,T1.TOTLE_POWER,T1.LOT_COLOR,
                                                case T.PRO_LEVEL when 'Grade_AJ' then N'A级'
                                                when 'Grade_A0J' then N'A0级'
                                                when 'Grade_KJ' then N'客级'
                                                when 'Grade_ERJ_WG' then N'二级'
                                                when 'Grade_ERJ_XN' then N'二级'
                                                when 'Grade_SANJ_WG' THEN N'三级'
                                                when 'Grade_SANJ_XN' THEN N'三级'
                                                else '' end PRO_LEVEL
                                                FROM POR_LOT T INNER JOIN WIP_CONSIGNMENT T1 ON T.PALLET_NO=T1.PALLET_NO
                                                INNER JOIN POR_PRODUCT T2 ON T.PRO_ID=T2.PRODUCT_CODE
                                                inner join WIP_CUSTCHECK t3 on t.LOT_NUMBER=t3.CC_FCODE1
                                                WHERE T1.CS_DATA_GROUP='3' AND T1.ISFLAG=1 and t3.ISFLAG=1 and t3.CC_DATA_GROUP='1'
                                                AND T1.TO_WH_TIME BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')
                                              ", Start_Time, End_Time);

                    if (!string.IsNullOrEmpty(_wonumber))
                        sqlCommand += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                    if (!string.IsNullOrEmpty(_pro_id))
                        sqlCommand += string.Format(" and t.PRO_ID in ({0}) ", _pro_id);
                    if (!string.IsNullOrEmpty(_protype))
                        sqlCommand += string.Format(" and t2.PROMODEL_NAME in ({0})", _protype);
                    if (!string.IsNullOrEmpty(_locationkey))
                        sqlCommand += string.Format(" and t.FACTORYROOM_KEY in ({0})", _locationkey);
                    if (!string.IsNullOrEmpty(_grade))
                    {
                        if (_grade.Equals("Grade_KJ") || _grade.Equals("Grade_AJ") || _grade.Equals("Grade_A0J"))
                            sqlCommand += string.Format(" and t.PRO_LEVEL='{0}'", _grade);
                        if (_grade.Equals("Grade_ERJ"))
                            sqlCommand += string.Format(" and t.PRO_LEVEL IN ('Grade_ERJ_WG','Grade_ERJ_XN')");
                        if (_grade.Equals("Grade_SANJ"))
                            sqlCommand += string.Format(" and t.PRO_LEVEL IN ('Grade_SANJ_WG','Grade_SANJ_XN')");
                    }
                    sqlCommand += " ) A order by A.TO_WH_TIME asc";
                }

                DataTable dtPatchData = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dsReturn.Merge(dtPatchData, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }

            return dsReturn;
        }
        /// <summary>
        /// 获取终检或者入库的过站数量
        /// </summary>
        /// <returns></returns>
        public DataSet GetMoveOutDataByOperation()
        {
            DataSet dsReturn=new DataSet();
            if (_operation.Equals("终检"))
            {
                if (!string.IsNullOrEmpty(_grade))
                {
                    if (_grade.Equals("Grade_Scrap"))
                        dsReturn = GetScrashData();
                    else
                        dsReturn = GetMoveOutData(0);
                }
            }
            if (_operation.Equals("入库"))
            {
                dsReturn = GetMoveOutData(1);
            }

            return dsReturn;
        }
        /// <summary>
        /// 获得功率等级
        /// </summary>
        /// itype:0:入库；1:测试。表示是终检档位还是入库档位
        /// <returns></returns>
        public DataSet GetPmaxstabLevel(int itype)
        {
            //转换为档位数据
            string pmax = _operation;

            DataSet dsReturn = new DataSet();
            if (itype == 0)
            {
                dsReturn = GetWarehousePLevel(pmax);
            }

            if (itype == 1)
            {
                dsReturn = GetCustCheckPLevel(pmax);
            }

            return dsReturn;
        }
        /// <summary>
        /// 获得转工单数据 itype 0:转进来；1:转出去
        /// </summary>
        /// <returns></returns>
        public DataSet GetRelExchgWo(int itype)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;

            sqlCommand = @"select  ROW_NUMBER() OVER( ORDER BY A.TIME_STAMP asc) SEQ, A.LOT_NUMBER,A.PRO_ID,
	                            A.WORK_ORDER_NO,A.PRO_ID2,A.WORK_ORDER_NO2,
	                            A.USERNAME USERNAME2,convert(varchar,A.TIME_STAMP ,120) TIME_STAMP2,A.PROMODEL_NAME 
                            from (select distinct t.TRANSACTION_KEY,t.PIECE_KEY ,t1.LOT_NUMBER,t1.PRO_ID,t1.WORK_ORDER_NO
                        ,t2.PRO_ID PRO_ID2,T2.WORK_ORDER_NO WORK_ORDER_NO2,
                        T4.USERNAME,T.TIME_STAMP,T3.PROMODEL_NAME
	                        from WIP_TRANSACTION t left join POR_LOT t1 on t.PIECE_KEY=t1.LOT_KEY
	                        left join WIP_LOT t2 on t.TRANSACTION_KEY=t2.TRANSACTION_KEY and t.PIECE_KEY=t2.LOT_KEY
	                        left join POR_PRODUCT t3 on t1.PRO_ID=t3.PRODUCT_CODE
                            left join RBAC_USER t4 on t.EDITOR=t4.BADGE
	                        where t.ACTIVITY like 'CHANGE_%'	                                            	                       	                        	                       
	                        and t.UNDO_FLAG=0 
	                        and t1.IS_MAIN_LOT=1
	                        AND t1.PRO_ID<>t2.PRO_ID ";

            //转进来
            if (itype == 0)
            {
                #region            
                if (!string.IsNullOrEmpty(_pro_id))
                    sqlCommand += string.Format(@" AND t1.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sqlCommand += string.Format(@" and t1.WORK_ORDER_NO in ({0})", _wonumber);            
                #endregion

            }
            if (itype == 1)
            {
                #region
                if (!string.IsNullOrEmpty(_pro_id))
                    sqlCommand += string.Format(@" AND t2.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sqlCommand += string.Format(@" and t2.WORK_ORDER_NO in ({0})", _wonumber);
                #endregion
            }
            if (!string.IsNullOrEmpty(Start_Time))
                sqlCommand += string.Format("  and t.TIME_STAMP >= CONVERT(datetime,'{0}') ", Start_Time);
            if (!string.IsNullOrEmpty(_end_time))
                sqlCommand += string.Format("  and t.TIME_STAMP<=  CONVERT(datetime,'{0}')  ", _end_time);
            if (!string.IsNullOrEmpty(_protype))
                sqlCommand += string.Format(" and t3.PROMODEL_NAME in ({0})", _protype);

            sqlCommand += " ) A ORDER BY A.TIME_STAMP ASC ";

            dsReturn = _db.ExecuteDataSet(CommandType.Text, sqlCommand);

            return dsReturn;

        }
        /// <summary>
        /// 获得CTM数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetCtmData()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = @"SELECT  ROW_NUMBER() OVER( ORDER BY A.TTIME asc) SEQ, A.LOT_NUM LOT_NUMBER,CONVERT(varchar, A.TTIME,120) TTIME,A.LEFFICIENCY,
	                            A.UEFFICIENCY,A.PRO_ID,A.PROMODEL_NAME,A.WORK_ORDER_NO,A.COEF_PMAX,A.PM,A.COEF_FF,A.COEF_IMAX,A.COEF_ISC	FROM (
	                            select DISTINCT T.LOT_NUM,t3.PROMODEL_NAME,t.TTIME,T2.LEFFICIENCY,T2.UEFFICIENCY,t1.PRO_ID,t1.WORK_ORDER_NO,t.COEF_PMAX,
	                            t.PM,t.COEF_FF,t.COEF_IMAX,t.COEF_ISC
	                            from WIP_IV_TEST t inner join POR_LOT t1 on t.LOT_NUM=t1.LOT_NUMBER
	                            left join BASE_EFFICIENCY t2 on t.VC_CELLEFF=t2.EFFICIENCY_NAME
	                            left join POR_PRODUCT t3 on t1.PRO_ID=t3.PRODUCT_CODE
	                            where  t.VC_DEFAULT='1' ";

            if (!string.IsNullOrEmpty(_locationkey))
                sqlCommand += string.Format(" and t2.EFFICIENCY_NAME in ({0})", _locationkey);
            if (!string.IsNullOrEmpty(_pro_id))
                sqlCommand += string.Format(" and t1.PRO_ID ='{0}' ", _pro_id);
            if (!string.IsNullOrEmpty(_wonumber))
                sqlCommand += string.Format(" and t1.WORK_ORDER_NO in ({0})", _wonumber);        
            if (!string.IsNullOrEmpty(_protype))
                sqlCommand += string.Format(" and t3.PROMODEL_NAME in ({0})", _protype);
         
            if (!string.IsNullOrEmpty(Start_Time))
                sqlCommand += string.Format(" and t.TTIME >= CONVERT(datetime,'{0}') ", Start_Time);
            if (!string.IsNullOrEmpty(_end_time))
                sqlCommand += string.Format(" and t.TTIME <= CONVERT(datetime,'{0}') ", _end_time);

            sqlCommand += "  ) A  order by a.TTIME asc ";

            dsReturn = _db.ExecuteDataSet(CommandType.Text, sqlCommand);

            return dsReturn;
        }
        /// <summary>
        /// 日运营报表中碎片明细
        /// </summary>
        /// <returns></returns>
        public DataSet GetDepathData()
        {

            string sqlCommand = string.Empty;
            string reason_name = Operation;

            DataSet dsReturn = new DataSet();
            try
            {
                sqlCommand = string.Format(@" select distinct t2.LOT_NUMBER,t2.PRO_ID,t2.WORK_ORDER_NO, t.REASON_CODE_NAME,t.PATCH_QUANTITY
                                            from WIP_PATCH t inner join  WIP_TRANSACTION t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                            inner join POR_LOT t2 on t.PATCHED_LOT_KEY=t2.LOT_KEY
                                            left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                            where  t1.TIME_STAMP between CONVERT(datetime,'{0}') and CONVERT(datetime,'{1}') ", Start_Time, End_Time);

                if (!string.IsNullOrEmpty(_wonumber))
                    sqlCommand += string.Format(" and t2.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_pro_id))
                    sqlCommand += string.Format(" and t2.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_protype))
                    sqlCommand += string.Format(" and t3.PROMODEL_NAME in ({0})", _protype);
                if (!string.IsNullOrEmpty(_locationkey))
                    sqlCommand += string.Format(" and t2.FACTORYROOM_KEY='{0}'", _locationkey);
                if (!string.IsNullOrEmpty(reason_name))
                    sqlCommand += string.Format(" and t.REASON_CODE_NAME='{0}'", reason_name);

                if (!string.IsNullOrEmpty(_reason_code_class))
                    sqlCommand += string.Format(" and t.REASON_CODE_CLASS='{0}'", _reason_code_class);
                else
                    sqlCommand += " and t.REASON_CODE_CLASS in ('19','21')";

                DataTable dtPatchData = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dsReturn.Merge(dtPatchData, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }

            return dsReturn;

        }

        /// <summary>
        /// 统计入库档位分布
        /// </summary>
        /// <param name="dsReturn"></param>
        private DataSet GetWarehousePLevel(string pmax)
        {
            DataSet dsReturn = new DataSet();

            string sqlCommand = @" select  ROW_NUMBER() OVER( ORDER BY t4.PMAXSTAB asc) SEQ,
                                t4.PMAXSTAB,t1.PRO_ID,t1.WORKNUMBER WORK_ORDER_NO,t.LOT_CUSTOMERCODE,t.LOT_SIDECODE,
                                 t.PALLET_NO,t.PALLET_TIME,t1.CHECK_POWER,t1.LOT_COLOR,
                                 case T1.PRO_LEVEL when 'Grade_AJ' then N'A级'
                                when 'Grade_A0J' then N'A0级'
                                when 'Grade_KJ' then N'客级'
                                when 'Grade_ERJ_WG' then N'二级'
                                when 'Grade_ERJ_XN' then N'二级'
                                when 'Grade_SANJ_WG' THEN N'三级'
                                when 'Grade_SANJ_XN' THEN N'三级'
                                else '' end PRO_LEVEL
	                                from WIP_TRANSACTION t0 inner join POR_LOT t on t0.PIECE_KEY=t.LOT_KEY
	                                inner join WIP_CUSTCHECK t1 on t.LOT_NUMBER=t1.CC_FCODE1
	                                left join POR_PRODUCT t2 on t1.PRO_ID=t2.PRODUCT_CODE
	                                left join BASE_TESTRULE t3 on t2.PRO_TEST_RULE=t3.TESTRULE_CODE
	                                left join BASE_POWERSET t4 on t3.PS_CODE=t4.PS_CODE
	                                where t1.ISFLAG=1 and t2.ISFLAG=1 and t3.ISFLAG=1 and t4.ISFLAG=1
		                        and t1.CHECK_POWER between CONVERT(varchar, t4.P_MIN) and CONVERT(varchar, t4.P_MAX)
		                        and t0.UNDO_FLAG=0	                                	                      
		                        and t0.STEP_NAME='入库'			                  
		                        and t0.ACTIVITY='TRACKIN'";

            if (!string.IsNullOrEmpty(_wonumber))
                sqlCommand += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
            if (!string.IsNullOrEmpty(_pro_id))
                sqlCommand += string.Format(" and t.PRO_ID ='{0}' ", _pro_id);
            if (!string.IsNullOrEmpty(_protype))
                sqlCommand += string.Format(" and t2.PROMODEL_NAME in ({0})", _protype);
            if (!string.IsNullOrEmpty(_locationkey))
                sqlCommand += string.Format(" and t.FACTORYROOM_KEY in ({0})", _locationkey);
          if(!string.IsNullOrEmpty(Start_Time))
               sqlCommand += string.Format(" and t0.TIME_STAMP >= CONVERT(datetime,'{0}') ", Start_Time);
               if(!string.IsNullOrEmpty(_end_time))
               sqlCommand += string.Format(" and t0.TIME_STAMP <= CONVERT(datetime,'{0}') ", _end_time);

               if (!string.IsNullOrEmpty(pmax))
                   sqlCommand += string.Format(" and t4.PMAXSTAB='{0}' ", pmax);

               sqlCommand += " ORDER BY t4.PMAXSTAB asc ";

               dsReturn = _db.ExecuteDataSet(CommandType.Text, sqlCommand);

               return dsReturn;

        }
        /// <summary>
        /// 统计终检档位分布
        /// </summary>
        /// <param name="dsReturn"></param>
        private DataSet GetCustCheckPLevel(string pmax)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = @" select  ROW_NUMBER() OVER( ORDER BY t4.PMAXSTAB asc) SEQ, t4.PMAXSTAB,t.PRO_ID,
                             t.WORK_ORDER_NO, t.LOT_NUMBER,t.LOT_CUSTOMERCODE,t.LOT_SIDECODE,
                             t1.COEF_FF,t1.COEF_IMAX,t1.COEF_PMAX,t1.TTIME,t1.PM                                                                                                
	                            from POR_LOT t 
                                inner join WIP_IV_TEST t1 on t.LOT_NUMBER=t1.LOT_NUM
	                            left join POR_PRODUCT t2 on t.PRO_ID=t2.PRODUCT_CODE
	                            left join BASE_TESTRULE t3 on t2.PRO_TEST_RULE=t3.TESTRULE_CODE
	                            left join BASE_POWERSET t4 on t3.PS_CODE=t4.PS_CODE
	                            where t1.VC_DEFAULT='1' and t2.ISFLAG=1 and t3.ISFLAG=1 and t4.ISFLAG=1
		                            and t1.COEF_PMAX between t4.P_MIN and t4.P_MAX ";
            if (!string.IsNullOrEmpty(_protype))
                sqlCommand += string.Format(" and t2.PROMODEL_NAME in ({0})", _protype);
            if (!string.IsNullOrEmpty(Start_Time))
                sqlCommand += string.Format(" and t1.TTIME >= CONVERT(datetime,'{0}') ", Start_Time);
            if (!string.IsNullOrEmpty(_end_time))
                sqlCommand += string.Format(" and t1.TTIME <= CONVERT(datetime,'{0}') ", _end_time);

            if (!string.IsNullOrEmpty(_wonumber))
                sqlCommand += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
            if (!string.IsNullOrEmpty(_pro_id))
                sqlCommand += string.Format(" and t.PRO_ID ='{0}' ", _pro_id);
    
            if (!string.IsNullOrEmpty(_locationkey))
                sqlCommand += string.Format(" and t.FACTORYROOM_KEY in ({0})", _locationkey);
    
            if (!string.IsNullOrEmpty(pmax))
                sqlCommand += string.Format(" and t4.PMAXSTAB='{0}' ", pmax);

            sqlCommand += " ORDER BY t4.PMAXSTAB asc";

            dsReturn = _db.ExecuteDataSet(CommandType.Text, sqlCommand);

            return dsReturn;
        }
        /// <summary>
        /// 获得运营报表数据,按天统计数据
        /// </summary>
        /// <param name="hsParams">主界面查询参数</param>
        /// <returns>日运营报表的数据</returns>
        private DataTable GetDailyReportData(DateTime dtStart,DateTime dtEnd)
        {
            DataTable dtDailyReport = new DataTable();
            try
            {
                string sqlView = string.Empty;
                string tmpDate = string.Empty;
                bool isQueryEndTime = false;
                string temp01 = _daily_end_time;
                string temp02 = _daily_start_time;
                string s_row2col01 = " SELECT PRO_NAME, SEQ ,COUNT(PRO_NAME) RN ";
                string s_row2col = " ";
                //------------------------------------------------------------------------------------------------------               

                DateTime stDate = Convert.ToDateTime(_daily_start_time);
                DateTime enDate = Convert.ToDateTime(_daily_end_time);
                DateTime middleDate = Convert.ToDateTime(_current_date);
                DateTime dateTmp = new DateTime();
                string query_end_Time = string.Empty;

                if (!string.IsNullOrEmpty(End_Time))
                {                  
                    isQueryEndTime = true;
                    query_end_Time = Convert.ToDateTime(_daily_end_time).ToString("yyyy-MM-dd ") + End_Time + ":00";
                }
                dateTmp = enDate;


                for (DateTime dtime = stDate; dtime < dateTmp; dtime = dtime.AddDays(1))
                {
                    string tmp = dtime.ToString("yyyy-MM-dd hh:mm:ss");
                    s_row2col += string.Format(@" ,CONVERT(varchar,SUM(case DAILY_DATE  when CONVERT(DATETIME,'{0}') then SUM_DATA else 0 end)) ""{0}"" ", tmp);
                }
                string endColumn = string.Empty, datecolumn = string.Empty;
                
             
                //结束时间累加进来-----------------------------------------------------------------------------------------------------------                  
                if (isQueryEndTime == true)
                {
                    endColumn = Convert.ToDateTime(_daily_end_time).ToString("yyyy-MM-dd") + " 08:00:00";
                    string sqlCommand = string.Format(@" select max(t.DAILY_DATE) from RPT_WIP_DAILY t 
                                                where t.LOCATION_KEY='{0}'
                                                and t.PART_TYPE='{1}'
                                                --and ISNULL(t.SEQ,'')<>''
                                                and PRO_ID in ({2})
                                                and t.DAILY_DATE<=CONVERT(datetime,'{4}')
                                                and t.WORK_ORDER_NO in ({3})", _locationkey, _protype, _pro_id, _wonumber, query_end_Time);
                    datecolumn = Convert.ToString(_db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0].Rows[0][0]);

                    s_row2col += string.Format(@" ,CONVERT(varchar,SUM(case DAILY_DATE  when CONVERT(DATETIME,'{0}') then SUM_DATA else 0 end)) ""{1}"" ", datecolumn, endColumn);
                }
                //----------------------------------------------------------------------------------------------------------------------------------
            

                 string sql01 = @"  FROM RPT_WIP_DAILY
                                     WHERE  LOCATION_KEY ='{0}'
                                       AND PART_TYPE = '{1}'
                                       AND ISNULL(SEQ,'')<>''                                       
                                        AND PRO_ID in ({2}) ";
                 if (isQueryEndTime)
                 {
                     sql01 += string.Format(@"  AND ( DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')   or DAILY_DATE=convert(datetime,'{2}'))",
                         _daily_start_time, _daily_end_time, datecolumn);
                 }
                 else
                     sql01 += string.Format(@"  AND DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')", _daily_start_time, _daily_end_time);

                 sqlView = string.Format(s_row2col01 + s_row2col + sql01, _locationkey, _protype, _pro_id);

                if (!string.IsNullOrEmpty(_wonumber))
                    sqlView += string.Format(@" and WORK_ORDER_NO in ({0})", _wonumber);

                sqlView += " GROUP BY PRO_NAME, SEQ";

                string sql02 = @" SELECT CONVERT(varchar,T.ALL_SUM_DATA) ALL_SUM_DATA, A.*
                                          FROM ({2}) A,
                                               (SELECT T.PRO_NAME, SUM(T.SUM_DATA) ALL_SUM_DATA
                                                  FROM RPT_WIP_DAILY T WHERE T.LOCATION_KEY='{0}' AND T.PART_TYPE = '{1}'                                                 
                                                 AND PRO_ID in ({3}) AND ISNULL(SEQ,'')<>'' ";

                string sql = string.Format(sql02, _locationkey, _protype,  sqlView, _pro_id);

                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and WORK_ORDER_NO in ({0})", _wonumber);
                if (isQueryEndTime)
                {
                    sql += string.Format(@"  AND ( T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')  or DAILY_DATE=convert(datetime,'{2}'))", 
                        _daily_start_time, _daily_end_time, datecolumn);
                }
                else
                    sql += string.Format(@" AND T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}') ", _daily_start_time, _daily_end_time);

                sql += @"GROUP BY T.PRO_NAME) T WHERE A.PRO_NAME = T.PRO_NAME ORDER BY A.SEQ ASC ";

                dtDailyReport = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];

                if (dtDailyReport.Rows.Count < 1)
                {
                    _errorMsg = "没有查询数据";
                    return dtDailyReport;
                }
             

                #region 统计总和中的优品率计算
                sql = @"SELECT SUM(T.SUM_DATA) ALL_SUM_DATA,T.PRO_NAME,COUNT(PRO_NAME) ALL_COUNT FROM RPT_WIP_DAILY T WHERE ISNULL(T.SEQ,'')=''";
                if (!string.IsNullOrEmpty(_locationkey))
                    sql += string.Format(@" AND T.LOCATION_KEY='{0}'", _locationkey);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(@" AND T.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and T.WORK_ORDER_NO in ({0})", _wonumber);               
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(@" AND T.PART_TYPE='{0}'", _protype);

                if (isQueryEndTime)
                {
                    sql += string.Format(@"  AND ( T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}') 
                                            or T.DAILY_DATE =convert(datetime,'{2}')", _daily_start_time, _daily_end_time, datecolumn);
                }
                else if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                    sql += string.Format(@" AND T.DAILY_DATE BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}')", _daily_start_time, _daily_end_time);

                sql += " GROUP BY T.PRO_NAME";

                DataTable dtSum = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];



                DataRow drstore = (from DataRow row in dtSum.Rows
                                   where row.Field<string>("PRO_NAME").ToString() == "REL_TOSTORE_QTY"
                                   select row).FirstOrDefault();
                DataRow drkj = (from DataRow row in dtSum.Rows
                                   where row.Field<string>("PRO_NAME").ToString() == "TO_STORE_KJ"
                                   select row).FirstOrDefault();
                DataRow draj = (from DataRow row in dtSum.Rows
                                where row.Field<string>("PRO_NAME").ToString() == "TO_STORE_AJ"
                                   select row).FirstOrDefault();
                DataRow dra0j = (from DataRow row in dtSum.Rows
                                 where row.Field<string>("PRO_NAME").ToString() == "TO_STORE_A0J"
                                   select row).FirstOrDefault();

                DataRow dr_a0j = (from DataRow row in dtSum.Rows
                                  where row.Field<string>("PRO_NAME").ToString() == "PER_GR_A0J"
                                  select row).FirstOrDefault();
                DataRow dr_aj = (from DataRow row in dtSum.Rows
                                 where row.Field<string>("PRO_NAME").ToString() == "PER_GR_AJ"
                                 select row).FirstOrDefault();
                DataRow dr_kj = (from DataRow row in dtSum.Rows
                                 where row.Field<string>("PRO_NAME").ToString() == "PER_GR_KJ"
                                 select row).FirstOrDefault();

                decimal store_qty = Convert.ToDecimal(drstore["ALL_SUM_DATA"] == null ? 0 : drstore["ALL_SUM_DATA"]);
                decimal kj_qty = Convert.ToDecimal(drkj["ALL_SUM_DATA"] == null ? 0 : drkj["ALL_SUM_DATA"]);
                decimal aj_qty = Convert.ToDecimal(draj["ALL_SUM_DATA"] == null ? 0 : draj["ALL_SUM_DATA"]);
                decimal a0j_qty = Convert.ToDecimal(dra0j["ALL_SUM_DATA"] == null ? 0 : dra0j["ALL_SUM_DATA"]);

             
                dr_a0j["ALL_SUM_DATA"] = Math.Round((a0j_qty + aj_qty + kj_qty) / (store_qty == 0 ? 1 : store_qty), 2).ToString("P");
                dr_aj["ALL_SUM_DATA"] = Math.Round((aj_qty + kj_qty) / (store_qty == 0 ? 1 : store_qty), 2).ToString("P");
                dr_kj["ALL_SUM_DATA"] = Math.Round((kj_qty) / (store_qty == 0 ? 1 : store_qty), 2).ToString("P");

                #endregion

               

                sql = string.Format(@"SELECT  T.PLAN_DATE, SUM(T.QUANTITY_INPUT) QUANTITY_INPUT, 
                                        SUM(T.QUANTITY_OUTPUT) QUANTITY_OUTPUT
                                        FROM RPT_PLAN_AIM T
                                        WHERE 1=1 ");
                if (!string.IsNullOrEmpty(_wonumber))
                    sql += string.Format(" and t.WORK_ORDER_NO in ({0})", _wonumber);
                if (!string.IsNullOrEmpty(_pro_id))
                    sql += string.Format(" and t.PRO_ID in ({0})", _pro_id);
                if (!string.IsNullOrEmpty(_locationName))
                    sql += string.Format(" and t.LOCATION_NAME='{0}'", _locationName);
                if (!string.IsNullOrEmpty(_protype))
                    sql += string.Format(" and t.PART_TYPE='{0}'", _protype);
                if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                    sql += string.Format(" and  T.PLAN_DATE BETWEEN convert(datetime,'{0}') AND convert(datetime,'{1}')", _daily_start_time, _daily_end_time);
                
                sql += "   GROUP BY T.PLAN_DATE ORDER BY T.PLAN_DATE ASC";

                DataTable dtPlan = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //新增投入计划
                #region
                DataRow drDaily = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drDaily[dc.ColumnName] = "PLAN_INPUT";
                    }
                    if (dc.ColumnName == "SEQ")
                    {
                        drDaily[dc.ColumnName] = "A10";
                    }

                    if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(dr["QUANTITY_INPUT"].ToString() == string.Empty ? "0" : dr["QUANTITY_INPUT"].ToString());
                        }
                        drDaily[dc.ColumnName] = v.ToString();
                    }

                    DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", dc.ColumnName));
                    if (drPlans != null && drPlans.Length > 0)
                    {
                        drDaily[dc.ColumnName] = drPlans[0]["QUANTITY_INPUT"].ToString() == "" ? "0" : drPlans[0]["QUANTITY_INPUT"].ToString();
                    }
                }
                dtDailyReport.Rows.InsertAt(drDaily, 0);
                #endregion
                //新增入库计划
                #region
                DataRow drToStore = dtDailyReport.NewRow();
                foreach (DataColumn dc in dtDailyReport.Columns)
                {
                    int v = 0;
                    if (dc.ColumnName == "PRO_NAME")
                    {
                        drToStore[dc.ColumnName] = "PLAN_TOSTORE";
                    }
                    if (dc.ColumnName == "SEQ")
                    {
                        drToStore[dc.ColumnName] = "B10";
                    }

                    if (dc.ColumnName == "ALL_SUM_DATA")
                    {
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            v += Convert.ToInt32(dr["QUANTITY_OUTPUT"].ToString() == string.Empty ? "0" : dr["QUANTITY_OUTPUT"].ToString());
                        }
                        drToStore[dc.ColumnName] = v.ToString();
                    }

                    DataRow[] drPlans = dtPlan.Select(string.Format(@"PLAN_DATE='{0}'", dc.ColumnName));
                    if (drPlans != null && drPlans.Length > 0)
                    {
                        drToStore[dc.ColumnName] = drPlans[0]["QUANTITY_OUTPUT"].ToString() == "" ? "0" : drPlans[0]["QUANTITY_OUTPUT"].ToString();
                    }
                }
                dtDailyReport.Rows.InsertAt(drToStore, 6);
                #endregion

               
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dtDailyReport;
        }

        private void SetRowColumnData(DataTable dtDailyReport, string column, string pro_name, int allnum)
        {
            DataRow[] drS = dtDailyReport.Select(string.Format("PRO_NAME='{0}'", pro_name));
            DataRow dr = drS[0];

        }

        private void SetRowData(DataTable dtDailyReport, string pro_name, string valueColumn, int allNum)
        {
            string sBak = string.Empty;
            DataRow dr = null;
            DataRow[] drS = dtDailyReport.Select(string.Format("PRO_NAME='{0}'", pro_name));
            if (drS.Length > 0)
                dr = drS[0];

            if (dr != null)
            {
                DataRow drv = null;
                DataRow[] drvs = dtDailyReport.Select(string.Format("PRO_NAME='{0}'", valueColumn));
                if (drvs.Length > 0)
                    drv = drvs[0];
                if (drv != null)
                {
                    //string v = Math.Round((Convert.ToDouble(drv["ALL_SUM_DATA"].ToString()) / allNum), 5).ToString());
                    string v = Math.Round((Convert.ToDouble(drv["ALL_SUM_DATA"].ToString()) / allNum), 5).ToString("#0.0000");
                    if (v.Contains('.'))
                    {
                        if (v.Substring(v.IndexOf('.'), v.Length - v.IndexOf('.')).Length > 3)
                        {
                            string vv = v;
                            vv = Math.Round(Convert.ToDouble(v), 4).ToString();

                            v = v.Substring(0, vv.IndexOf('.') + 5);
                        }
                    }
                    //  sBak = (Convert.ToDouble(dr["ALL_SUM_DATA"].ToString()) / allNum).ToString();
                    dr["ALL_SUM_DATA"] = v;
                }
            }
        }

        public bool UpdatePlanAimsData(DataTable dtPlan)
        {
            bool bl = false;
            using (DbConnection dbConn = _db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                try
                {
                    foreach (DataRow dr in dtPlan.Rows)
                    {
//                        sqlCommand = string.Format(@"UPDATE RPT_PLAN_AIM T
//                                       SET T.QUANTITY_INPUT = '{0}', T.QUANTITY_OUTPUT = '{1}'
//                                     WHERE T.PLANID = '{2}'
//                                    ", dr["QUANTITY_INPUT"].ToString(),
//                                             dr["QUANTITY_OUTPUT"].ToString(),
//                                             dr["PLANID"].ToString());
                        sqlCommand = string.Format(@"UPDATE RPT_PLAN_AIM SET " + PlanAimField.LOCATION_NAME +"='"+ Convert.ToString(dr[PlanAimField.LOCATION_NAME])+ "',"
                                                                         + PlanAimField.LOT_TYPE + "='" + Convert.ToString(dr[PlanAimField.LOT_TYPE]) + "',"
                                                                         + PlanAimField.PART_TYPE + "='" + Convert.ToString(dr[PlanAimField.PART_TYPE]) + "',"
                                                                         + PlanAimField.PLAN_DATE + "='" + Convert.ToString(dr[PlanAimField.PLAN_DATE]) + "',"
                                                                         + PlanAimField.PRO_ID + "='" + Convert.ToString(dr[PlanAimField.PRO_ID]) + "',"
                                                                         + PlanAimField.QUANTITY_INPUT + "='" + Convert.ToString(dr[PlanAimField.QUANTITY_INPUT]) + "',"
                                                                         + PlanAimField.QUANTITY_OUTPUT + "='" + Convert.ToString(dr[PlanAimField.QUANTITY_OUTPUT]) + "',"
                                                                         + PlanAimField.WORK_ORDER_NO + "='" + Convert.ToString(dr[PlanAimField.WORK_ORDER_NO]) + "',"
                                                                         + PlanAimField.SHIFT_VALUE + "='" + Convert.ToString(dr[PlanAimField.SHIFT_VALUE]) + "'"
                                                                        + " WHERE " + PlanAimField.PLANID + "='{0}'", Convert.ToString(dr[PlanAimField.PLANID]));

                        int iRow = _db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                        if (iRow < 1)
                        {
                            sqlCommand = string.Format(@"INSERT INTO RPT_PLAN_AIM (" + PlanAimField.LOCATION_NAME + ","
                                                                            + PlanAimField.LOT_TYPE + ","
                                                                            + PlanAimField.PART_TYPE + ","
                                                                            + PlanAimField.PLAN_DATE + ","
                                                                            + PlanAimField.PRO_ID + ","
                                                                            + PlanAimField.QUANTITY_INPUT + ","
                                                                            + PlanAimField.QUANTITY_OUTPUT + ","
                                                                            + PlanAimField.PLANID + ","
                                                                            + PlanAimField.SHIFT_VALUE + ","
                                                                            + PlanAimField.WORK_ORDER_NO + ")"
                                                                            + " Values('" +
                                                                           Convert.ToString(dr[PlanAimField.LOCATION_NAME]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.LOT_TYPE]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.PART_TYPE]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.PLAN_DATE]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.PRO_ID]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.QUANTITY_INPUT]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.QUANTITY_OUTPUT]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.PLANID]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.SHIFT_VALUE]) + "','"
                                                                           + Convert.ToString(dr[PlanAimField.WORK_ORDER_NO]) + "')");

                            _db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    dbTran.Commit();
                    bl = true;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    this._errorMsg = ex.Message;
                    bl = false;
                }
            }

            return bl;
        }
        /// <summary>
        /// 获得计划输入数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetPlanAimsData()
        {
            string sqlCommand = string.Format(@" SELECT t.*  FROM RPT_PLAN_AIM T  WHERE 1=1  ");
            if (!string.IsNullOrEmpty(_locationName))
                sqlCommand += string.Format(" and T.LOCATION_NAME = '{0}'", _locationName);
            if (!string.IsNullOrEmpty(_daily_start_time) && !string.IsNullOrEmpty(_daily_end_time))
                sqlCommand += string.Format(" AND T.PLAN_DATE BETWEEN convert(datetime,'{0}') AND convert(datetime,'{1}')", _daily_start_time, _daily_end_time);
            if (!string.IsNullOrEmpty(PlanId))
                sqlCommand += string.Format(" and t.Planid='{0}'", _planid);

            sqlCommand += "  ORDER BY T.PLAN_DATE ASC,T.PART_TYPE ASC ";

            DataTable dtPlanAimData = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

            return dtPlanAimData;
        }

    }

    public class PlanAimField
    {
        public const string PLANID = "PLANID";
        public const string PLAN_DATE = "PLAN_DATE";
        public const string LOCATION_NAME = "LOCATION_NAME";
        public const string PART_TYPE = "PART_TYPE";
        public const string LOT_TYPE = "LOT_TYPE";
        public const string QUANTITY_INPUT = "QUANTITY_INPUT";
        public const string QUANTITY_OUTPUT = "QUANTITY_OUTPUT";
        public const string PRO_ID = "PRO_ID";
        public const string WORK_ORDER_NO = "WORK_ORDER_NO";
        public const string GREATER_EFFI = "GREATER_EFFI";
        public const string CREATE_TIME = "CREATE_TIME";
        public const string CREATOR = "CREATOR";
        public const string EDITE_TIME = "EDITE_TIME";
        public const string EDITOR = "EDITOR";
        public const string SHIFT_VALUE = "SHIFT_VALUE";

    }

    public class LayoutViewType
    {
        /// <summary>
        /// 呈现模式，按小时或者按天
        /// </summary>
        public const string ViewModule = "ViewModule";
        /// <summary>
        /// 今天数据，按小时呈现
        /// </summary>
        public const string ViewType_Hour = "Hour";
        /// <summary>
        /// 超过一天数据，按天呈现
        /// </summary>
        public const string ViewType_Day = "Day";
        /// <summary>
        /// 报表表名
        /// </summary>
        public const string ReportTable = "ReportTable";

        public const string ReportPressCells = "PressCells";
        /// <summary>
        /// 图表数据源名
        /// </summary>
        public const string ReportTableForChart = "ReportTableForChart";
    }
}
