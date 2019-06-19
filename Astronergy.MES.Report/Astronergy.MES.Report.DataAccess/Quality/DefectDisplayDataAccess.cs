using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    public class DefectDisplayAccess : BaseDBAccess
    {
        //属性设置
        #region
        /// <summary>
        /// 产品ID号
        /// </summary>
        public string Pro_Ids
        {
            get { return proids; }
            set { proids = value; }
        }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrders
        {
            get { return workorders; }
            set { workorders = value; }
        }
        /// <summary>
        /// 产品料号
        /// </summary>
        public string PartNumber
        {
            get { return partNumber; }
            set { partNumber = value; }
        }
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProTypes
        {
            get { return protypes; }
            set { protypes = value; }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 工厂车间ID
        /// </summary>
        public string LocationKey
        {
            get { return locationkey; }
            set { locationkey = value; }
        }
        /// <summary>
        /// 按天查询亦或是按时间查询
        /// </summary>
        public int QueryByDayOrHour
        {
            get { return queryByDayOrHour; }
            set { queryByDayOrHour = value; }
        }
        /// <summary>
        /// 不良代码分类
        /// </summary>
        public string Reason_Code_Class
        {
            get { return reason_Code_Class; }
            set { reason_Code_Class = value; }
        }
        /// <summary>
        /// 不良品统计类别
        /// </summary>
        public string sType
        {
            get { return stype; }
            set { stype = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _msgerror; }
            set { _msgerror = value; }
        }

        private string startDate = string.Empty;
        private string endDate = string.Empty;
        private string protypes = string.Empty;
        private string proids = string.Empty;
        private string workorders = string.Empty;
        private string partNumber = string.Empty;
        private string locationkey = string.Empty;
        private string _msgerror = string.Empty;
        private int queryByDayOrHour = -1;
        private string reason_Code_Class = string.Empty;
        private string stype = string.Empty;
        #endregion

        public DefectDisplayAccess()
        { 
        
        }

        public DataSet GetDefetStatisticsData()
        {
            DataSet dsReturn = new DataSet();
            if (queryByDayOrHour == 0)
                dsReturn = GetDefectStatisticsByDay();
            else if (queryByDayOrHour == 1)
                dsReturn = GetDefectStatisticsByDay();

            return dsReturn;
        }
        public DataTable GetStatisByReasonCode()
        {
            string sqlCommand = string.Empty;
            DataTable dtReturn = new DataTable();
            try
            {
                //预判A0等级
                if (stype.Equals("1"))
                {
                    sqlCommand = @"select distinct convert(varchar,t.TIME_STAMP,120) TIME_STAMP,t1.STEP_NAME,t5.USERNAME,
                                t6.SHIFT_NAME,t4.CUSTOMER,t4.CUSTOMER_TYPE,t2.WORK_ORDER_NO,t2.PART_NUMBER AS '产品料号',t3.PROMODEL_NAME,
                                t2.LOT_NUMBER LOT_NUMBER_DEFECT,t1.REASON_CODE_NAME,t1.DESCRIPTION
                                from WIP_TRANSACTION t inner join WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                inner join POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY 
                                left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                left join V_WORK_ORDER_ATTR t4 on t2.WORK_ORDER_NO=t4.ORDER_NUMBER
                                left join RBAC_USER t5 on t1.EDITOR=t5.BADGE
                                left join (select b.SHIFT_NAME,a.STARTTIME,a.ENDTIME
                                from CAL_SCHEDULE_DAY a left join CAL_SHIFT b on a.SHIFT_KEY=b.SHIFT_KEY
                                )t6 on t.TIME_STAMP between t6.STARTTIME and t6.ENDTIME
                                where t1.ROUTE_NAME=N'返修-终检到层压'";
                }
                //直判A0级
                if (stype.Equals("2"))
                {
                    sqlCommand = @"select distinct convert(varchar,t.TIME_STAMP,120) TIME_STAMP,t1.STEP_NAME,t5.USERNAME,
                                t6.SHIFT_NAME,t4.CUSTOMER,t4.CUSTOMER_TYPE,t2.WORK_ORDER_NO,t2.PART_NUMBER AS '产品料号',t3.PROMODEL_NAME,
                                t2.LOT_NUMBER LOT_NUMBER_DEFECT,t1.REASON_CODE_NAME,t1.DESCRIPTION
                                from WIP_TRANSACTION t inner join WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                inner join POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY 
                                left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                left join V_WORK_ORDER_ATTR t4 on t2.WORK_ORDER_NO=t4.ORDER_NUMBER
                                left join RBAC_USER t5 on t1.EDITOR=t5.BADGE
                                left join (select b.SHIFT_NAME,a.STARTTIME,a.ENDTIME
                                from CAL_SCHEDULE_DAY a left join CAL_SHIFT b on a.SHIFT_KEY=b.SHIFT_KEY
                                )t6 on t.TIME_STAMP between t6.STARTTIME and t6.ENDTIME
                                inner join WIP_CUSTCHECK t7 on t2.LOT_NUMBER=T7.CC_FCODE1
                                where T7.CC_DATA_GROUP IN ('0','1') AND T7.ISFLAG=1
                                AND T7.PRO_LEVEL='Grade_A0J'
                                 AND not exists(
                                select a.PIECE_KEY from WIP_TRANSACTION a inner join WIP_DEFECT b on a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                where b.ROUTE_NAME=N'返修-终检到层压' and t.PIECE_KEY=a.PIECE_KEY
                                )";
                }
                //直判二级
                if (stype.Equals("3"))
                {
                    sqlCommand = @"select distinct convert(varchar,t.TIME_STAMP,120) TIME_STAMP,t1.STEP_NAME,t5.USERNAME,
                                t6.SHIFT_NAME,t4.CUSTOMER,t4.CUSTOMER_TYPE,t2.WORK_ORDER_NO,t2.PART_NUMBER AS '产品料号',t3.PROMODEL_NAME,
                                t2.LOT_NUMBER LOT_NUMBER_DEFECT,t1.REASON_CODE_NAME,t1.DESCRIPTION
                                from WIP_TRANSACTION t inner join WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                inner join POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY 
                                left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                left join V_WORK_ORDER_ATTR t4 on t2.WORK_ORDER_NO=t4.ORDER_NUMBER
                                left join RBAC_USER t5 on t1.EDITOR=t5.BADGE
                                left join (select b.SHIFT_NAME,a.STARTTIME,a.ENDTIME
                                from CAL_SCHEDULE_DAY a left join CAL_SHIFT b on a.SHIFT_KEY=b.SHIFT_KEY
                                )t6 on t.TIME_STAMP between t6.STARTTIME and t6.ENDTIME
                                inner join WIP_CUSTCHECK t7 on t2.LOT_NUMBER=T7.CC_FCODE1
                                where T7.CC_DATA_GROUP IN ('0','1') AND T7.ISFLAG=1
                                AND T7.PRO_LEVEL IN ('Grade_ERJ_WG','Grade_ERJ_XN')
                                 AND not exists(
                                select a.PIECE_KEY from WIP_TRANSACTION a inner join WIP_DEFECT b on a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                where b.ROUTE_NAME=N'返修-终检到层压' and t.PIECE_KEY=a.PIECE_KEY
                                )";
                }
                //直判三级
                if (stype.Equals("4"))
                {
                    sqlCommand = @"select distinct convert(varchar,t.TIME_STAMP,120) TIME_STAMP,t1.STEP_NAME,t5.USERNAME,
                                t6.SHIFT_NAME,t4.CUSTOMER,t4.CUSTOMER_TYPE,t2.WORK_ORDER_NO,t2.PART_NUMBER AS '产品料号',t3.PROMODEL_NAME,
                                t2.LOT_NUMBER LOT_NUMBER_DEFECT,t1.REASON_CODE_NAME,t1.DESCRIPTION
                                from WIP_TRANSACTION t inner join WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                inner join POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY 
                                left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                left join V_WORK_ORDER_ATTR t4 on t2.WORK_ORDER_NO=t4.ORDER_NUMBER
                                left join RBAC_USER t5 on t1.EDITOR=t5.BADGE
                                left join (select b.SHIFT_NAME,a.STARTTIME,a.ENDTIME
                                from CAL_SCHEDULE_DAY a left join CAL_SHIFT b on a.SHIFT_KEY=b.SHIFT_KEY
                                )t6 on t.TIME_STAMP between t6.STARTTIME and t6.ENDTIME
                                inner join WIP_CUSTCHECK t7 on t2.LOT_NUMBER=T7.CC_FCODE1
                                where T7.CC_DATA_GROUP IN ('0','1') AND T7.ISFLAG=1
                                AND T7.PRO_LEVEL IN ('Grade_SANJ_WG','Grade_SANJ_XN')
                                 AND not exists(
                                select a.PIECE_KEY from WIP_TRANSACTION a inner join WIP_DEFECT b on a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                where b.ROUTE_NAME=N'返修-终检到层压' and t.PIECE_KEY=a.PIECE_KEY
                                )";
                }
                //层压报废
                if (stype.Equals("5"))
                {
                    sqlCommand = @"select distinct convert(varchar,t.TIME_STAMP,120) TIME_STAMP,t1.STEP_NAME,t5.USERNAME,
                                t6.SHIFT_NAME,t4.CUSTOMER,t4.CUSTOMER_TYPE,t2.WORK_ORDER_NO,t2.PART_NUMBER AS '产品料号',t3.PROMODEL_NAME,
                                t2.LOT_NUMBER LOT_NUMBER_DEFECT,t1.REASON_CODE_NAME,t1.DESCRIPTION
                                from WIP_TRANSACTION t inner join WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                inner join POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY 
                                left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                                left join V_WORK_ORDER_ATTR t4 on t2.WORK_ORDER_NO=t4.ORDER_NUMBER
                                left join RBAC_USER t5 on t1.EDITOR=t5.BADGE
                                left join (select b.SHIFT_NAME,a.STARTTIME,a.ENDTIME
                                from CAL_SCHEDULE_DAY a left join CAL_SHIFT b on a.SHIFT_KEY=b.SHIFT_KEY
                                )t6 on t.TIME_STAMP between t6.STARTTIME and t6.ENDTIME
                                where t.STEP_NAME=N'层压'
                                and t.ACTIVITY='SCRAP'";
                }

                if (!string.IsNullOrEmpty(protypes))
                    sqlCommand += string.Format(" and t3.PROMODEL_NAME in ({0})", protypes);
                if (!string.IsNullOrEmpty(proids))
                    sqlCommand += string.Format(" and t2.PRO_ID in ({0})", proids);
                if (!string.IsNullOrEmpty(workorders))
                    sqlCommand += string.Format(" and t2.WORK_ORDER_NO in ({0})", workorders);
                if (!string.IsNullOrEmpty(reason_Code_Class))
                    sqlCommand += string.Format(" and t1.REASON_CODE_CLASS='{0}'", reason_Code_Class);
                if (!string.IsNullOrEmpty(locationkey))
                    sqlCommand += string.Format("  and t2.FACTORYROOM_KEY='{0}'", locationkey);
                if (!string.IsNullOrEmpty(startDate))
                    sqlCommand += string.Format("  and t.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                if (!string.IsNullOrEmpty(endDate))
                    sqlCommand += string.Format("  and t.TIME_STAMP<=CONVERT(datetime,'{0}')", endDate);
                if (!string.IsNullOrEmpty(partNumber))
                    sqlCommand += string.Format(" and t2.PART_NUMBER in ({0})", partNumber);

                dtReturn = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
            }
            catch (Exception ex)
            {
                _msgerror = ex.Message;
            }

            return dtReturn;            
        }

        public DataSet GetDefectStatisticsByDay()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            string sqlFilter = string.Empty;
            try
            {


                sqlCommand = @"SELECT t1.REASON_CODE_CLASS,SUM(t1.DEFECT_QUANTITY) DEFECT_QUANTITY,'ADV_Grade_A0J' PRO_LEVEL
                            FROM WIP_TRANSACTION t
                            INNER JOIN WIP_DEFECT t1 ON t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                            INNER JOIN POR_LOT t2 ON t.PIECE_KEY=t2.LOT_KEY
                            LEFT JOIN POR_PRODUCT t3 ON t2.PRO_ID=t3.PRODUCT_CODE AND t3.ISFLAG=1
                            WHERE t1.ROUTE_NAME=N'返修-终检到层压' AND t.UNDO_FLAG=0
                            {0}
                            GROUP BY REASON_CODE_CLASS
                            UNION ALL
                            SELECT  t1.REASON_CODE_CLASS,SUM(t1.DEFECT_QUANTITY) DEFECT_QUANTITY,T4.PRO_LEVEL 
                            FROM WIP_TRANSACTION t 
                            INNER JOIN WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                            INNER JOIN POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY
                            LEFT JOIN POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE
                            INNER JOIN WIP_CUSTCHECK t4 on t2.LOT_NUMBER=t4.CC_FCODE1
                            WHERE t4.CC_DATA_GROUP in('0','1') AND t4.ISFLAG=1 AND t3.ISFLAG=1 AND t.UNDO_FLAG=0
                            AND T3.PRO_LEVEL IN ('Grade_A0J','Grade_ERJ_WG','Grade_ERJ_XN','Grade_SANJ_WG','Grade_SANJ_XN')                                              
                            AND NOT EXISTS(SELECT a.PIECE_KEY 
                                           FROM WIP_TRANSACTION a 
                                           INNER JOIN WIP_DEFECT b on a.TRANSACTION_KEY=b.TRANSACTION_KEY
                                           WHERE b.ROUTE_NAME=N'返修-终检到层压' 
                                           AND t.PIECE_KEY=a.PIECE_KEY
                                           AND a.UNDO_FLAG=0)
                            {0}
                            GROUP BY REASON_CODE_CLASS,t4.PRO_LEVEL
                            UNION ALL
                            SELECT t1.REASON_CODE_CLASS,COUNT(t1.DEFECT_QUANTITY) DEFECT_QUANTITY,'SCRAP' PRO_LEVEL
                            FROM WIP_TRANSACTION t 
                            INNER JOIN WIP_DEFECT t1 ON t.TRANSACTION_KEY=t1.TRANSACTION_KEY 
                            INNER JOIN POR_LOT t2 ON t.PIECE_KEY=t2.LOT_KEY
                            LEFT JOIN POR_PRODUCT t3 ON t2.PRO_ID=t3.PRODUCT_CODE
                            WHERE t.ACTIVITY='SCRAP' AND t3.ISFLAG=1 AND t.UNDO_FLAG=0
                            AND t.STEP_NAME=N'层压'
                             {0} GROUP BY t1.REASON_CODE_CLASS ";
                if (!string.IsNullOrEmpty(protypes))
                    sqlFilter += string.Format(" AND t3.PROMODEL_NAME in ({0})", protypes);
                if (!string.IsNullOrEmpty(locationkey))
                    sqlFilter += string.Format(" AND t2.FACTORYROOM_KEY='{0}'", locationkey);
                if (!string.IsNullOrEmpty(proids))
                    sqlFilter += string.Format(" AND t2.PRO_ID in ({0})", proids);
                if (!string.IsNullOrEmpty(workorders))
                    sqlFilter += string.Format(" AND t2.WORK_ORDER_NO in ({0}) ", workorders);
                if (!string.IsNullOrEmpty(startDate))
                    sqlFilter += string.Format(" AND t.TIME_STAMP>=CONVERT(DATETIME,'{0}')", startDate);
                if (!string.IsNullOrEmpty(endDate))
                    sqlFilter += string.Format(" AND t.TIME_STAMP<=CONVERT(DATETIME,'{0}')", endDate);
                if (!string.IsNullOrEmpty(partNumber))
                    sqlFilter += string.Format(" AND t2.PART_NUMBER in ({0}) ", partNumber);
   

                sqlCommand = string.Format(sqlCommand, sqlFilter);
                DataTable dt01 = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                string sqlReason = string.Format(@"SELECT A.REASON_CODE_CLASS,SUM(A.DEFECT_QUANTITY) DEFECT_QUANTITY
                                                   FROM ({0}) A
                                                   GROUP BY REASON_CODE_CLASS
                                                   ORDER BY DEFECT_QUANTITY DESC", sqlCommand);
                DataTable dt02 = _db.ExecuteDataSet(CommandType.Text, sqlReason).Tables[0];
                DataTable dt03 = Compare2Table(dt01, dt02);
                DataView dv = dt03.DefaultView;
                dv.Sort = " SEQ ASC";
                DataTable dtDefectStatisticsReport = dv.ToTable();
                dtDefectStatisticsReport.TableName = LayoutViewType.ReportTable;
                dsReturn.Merge(dtDefectStatisticsReport, true, MissingSchemaAction.Add);

                dt02.TableName = LayoutViewType.ReportTableForChart;
                dsReturn.Merge(dt02, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _msgerror = ex.Message;
            }

            return dsReturn;
        }

        private DataTable Compare2Table(DataTable dtReport, DataTable dtCount)
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("SEQ");
            dtReturn.Columns.Add("DEFECT_NAME");
            DataRow drNew = dtReturn.NewRow();
            drNew["SEQ"] = 6;
            drNew["DEFECT_NAME"] = "总计";
            dtReturn.Rows.Add(drNew);
            DataTable dtColumns = BindReasonCodeClass();
            foreach (DataRow drCount in dtCount.Rows)
            {
                string col = Convert.ToString(drCount["REASON_CODE_CLASS"]);
                if (!dtReturn.Columns.Contains(col))
                    dtReturn.Columns.Add(col);
                DataRow dr6 = dtReturn.Select("SEQ='6'")[0];
                dr6[col] = drCount["DEFECT_QUANTITY"];

                DataRow[] drsColuns = dtColumns.Select(string.Format("CODE='{0}'", col));
                if (drsColuns.Length > 0)
                    dtColumns.Rows.Remove(drsColuns[0]);
            }

            if (dtColumns.Rows.Count > 0)
            {
                foreach (DataRow drCol in dtColumns.Rows)
                {
                    string col = Convert.ToString(drCol["CODE"]);
                    if (!dtReturn.Columns.Contains(col))
                        dtReturn.Columns.Add(col);
                    DataRow[] drscount = dtCount.Select(string.Format("REASON_CODE_CLASS='{0}'", col));
                    if (drscount != null && drscount.Length < 1)
                    {
                        DataRow drAddCount = dtCount.NewRow();
                        drAddCount["REASON_CODE_CLASS"] = col;
                        drAddCount["DEFECT_QUANTITY"] = 0;
                        dtCount.Rows.Add(drAddCount);
                    }
                }
            }

            try
            {
                //统计A0级预判
                #region
                DataRow drAdd01 = dtReturn.NewRow();
                drAdd01["SEQ"] = 1;
                drAdd01["DEFECT_NAME"] = "A0级品（预判）";
                foreach (DataColumn dcol in dtReturn.Columns)
                {
                    if (dcol.ColumnName == "SEQ" || dcol.ColumnName == "DEFECT_NAME")
                        continue;
                    DataRow[] drsADV_Grade_A0J = dtReport.Select(string.Format("PRO_LEVEL='{0}' AND REASON_CODE_CLASS='{1}'", "ADV_Grade_A0J", dcol.ColumnName));
                    if (drsADV_Grade_A0J != null && drsADV_Grade_A0J.Length > 0)
                    {
                        drAdd01[dcol.ColumnName] = drsADV_Grade_A0J[0]["DEFECT_QUANTITY"];

                    }
                    else
                        drAdd01[dcol.ColumnName] = 0;
                }
                #endregion
                dtReturn.Rows.Add(drAdd01);
                //统计A0级直判
                #region
                DataRow drAdd02 = dtReturn.NewRow();
                drAdd02["SEQ"] = 2;
                drAdd02["DEFECT_NAME"] = "A0级品（直判）";
                foreach (DataColumn dcol in dtReturn.Columns)
                {
                    if (dcol.ColumnName == "SEQ" || dcol.ColumnName == "DEFECT_NAME")
                        continue;
                    DataRow[] drsADV_Grade_A0J = dtReport.Select(string.Format("PRO_LEVEL='{0}' AND REASON_CODE_CLASS='{1}'", "Grade_A0J", dcol.ColumnName));
                    if (drsADV_Grade_A0J != null && drsADV_Grade_A0J.Length > 0)
                    {
                        drAdd02[dcol.ColumnName] = drsADV_Grade_A0J[0]["DEFECT_QUANTITY"];

                    }
                    else
                        drAdd02[dcol.ColumnName] = 0;
                }
                #endregion
                dtReturn.Rows.Add(drAdd02);
                //统计二级直判
                #region
                DataRow drAdd03 = dtReturn.NewRow();
                drAdd03["SEQ"] = 3;
                drAdd03["DEFECT_NAME"] = "二级品";
                foreach (DataColumn dcol in dtReturn.Columns)
                {
                    if (dcol.ColumnName == "SEQ" || dcol.ColumnName == "DEFECT_NAME")
                        continue;
                    DataRow[] drsADV_Grade_A0J = dtReport.Select(string.Format("PRO_LEVEL IN ('{0}','{1}') AND REASON_CODE_CLASS='{2}'", "Grade_ERJ_XN", "Grade_ERJ_WG", dcol.ColumnName));
                    if (drsADV_Grade_A0J != null && drsADV_Grade_A0J.Length > 0)
                    {
                        drAdd03[dcol.ColumnName] = drsADV_Grade_A0J[0]["DEFECT_QUANTITY"];

                    }
                    else
                        drAdd03[dcol.ColumnName] = 0;
                }
                #endregion
                dtReturn.Rows.Add(drAdd03);
                //统计三级直判
                #region
                DataRow drAdd04 = dtReturn.NewRow();
                drAdd04["SEQ"] = 4;
                drAdd04["DEFECT_NAME"] = "三级品";
                foreach (DataColumn dcol in dtReturn.Columns)
                {
                    if (dcol.ColumnName == "SEQ" || dcol.ColumnName == "DEFECT_NAME")
                        continue;
                    DataRow[] drsADV_Grade_A0J = dtReport.Select(string.Format("PRO_LEVEL IN ('{0}','{1}') AND REASON_CODE_CLASS='{2}'", "Grade_SANJ_XN", "Grade_SANJ_WG", dcol.ColumnName));
                    if (drsADV_Grade_A0J != null && drsADV_Grade_A0J.Length > 0)
                    {
                        drAdd04[dcol.ColumnName] = drsADV_Grade_A0J[0]["DEFECT_QUANTITY"];

                    }
                    else
                        drAdd04[dcol.ColumnName] = 0;
                }
                #endregion
                dtReturn.Rows.Add(drAdd04);
                //统计层压报废
                #region
                DataRow drAdd05 = dtReturn.NewRow();
                drAdd05["SEQ"] = 5;
                drAdd05["DEFECT_NAME"] = "报废";
                foreach (DataColumn dcol in dtReturn.Columns)
                {
                    if (dcol.ColumnName == "SEQ" || dcol.ColumnName == "DEFECT_NAME")
                        continue;
                    DataRow[] drsADV_Grade_A0J = dtReport.Select(string.Format("PRO_LEVEL='{0}' AND REASON_CODE_CLASS='{1}'", "SCRAP", dcol.ColumnName));
                    if (drsADV_Grade_A0J != null && drsADV_Grade_A0J.Length > 0)
                    {
                        drAdd05[dcol.ColumnName] = drsADV_Grade_A0J[0]["DEFECT_QUANTITY"];

                    }
                    else
                        drAdd05[dcol.ColumnName] = 0;
                }
                #endregion
                dtReturn.Rows.Add(drAdd05);
                //统计累计比例
                #region
                decimal qtySum = Convert.ToDecimal(dtCount.Compute("sum(DEFECT_QUANTITY)", null));
                DataRow drAdd07 = dtReturn.NewRow();
                drAdd07["SEQ"] = 7;
                drAdd07["DEFECT_NAME"] = "累计比例";
                decimal interation = 0;
                foreach (DataColumn dcol in dtReturn.Columns)
                {
                    if (dcol.ColumnName == "SEQ" || dcol.ColumnName == "DEFECT_NAME")
                        continue;
                    string reasoncount = string.Empty;
                    DataRow[] drsADV_Grade_A0J = dtCount.Select(string.Format(" REASON_CODE_CLASS='{0}'", dcol.ColumnName));
                    if (drsADV_Grade_A0J != null && drsADV_Grade_A0J.Length > 0)
                    {
                        reasoncount = Convert.ToString(drsADV_Grade_A0J[0]["DEFECT_QUANTITY"]);
                        if (reasoncount.Equals(string.Empty))
                            reasoncount = "0";
                        interation += Convert.ToDecimal(reasoncount);
                        drsADV_Grade_A0J[0]["DEFECT_QUANTITY"] = interation;
                    }

                    drAdd07[dcol.ColumnName] = Math.Round(interation / (qtySum == 0 ? 1 : qtySum), 4).ToString("P");
                }
                #endregion
                dtReturn.Rows.Add(drAdd07);

            }
            catch (Exception ex)
            {
                _msgerror = ex.Message;
            }

            return dtReturn;
        }

        public DataSet GetDefectAccessData()
        {
            DataSet dsReturn = new DataSet();
            if (queryByDayOrHour == 0)
                dsReturn = GetDefectAccessByDay();
            else if (queryByDayOrHour == 1)
                dsReturn = GetDefectAccessByHour();

            return dsReturn;
        }

        public DataSet GetDefectAccessByHour()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;         
            try
            {
                sqlCommand = string.Format(@"SELECT t3.CODE,t3.NAME DEFECT_NAME,SUM(1) ""{0}"" ", startDate + "至" + endDate);

                sqlCommand += string.Format(@" FROM WIP_TRANSACTION t,POR_LOT t1,WIP_DEFECT t2 ,V_Basic_ClassOfRCode t3,POR_PRODUCT t4
                                               WHERE t1.PRO_ID=t4.PRODUCT_CODE 
                                               AND t2.REASON_CODE_CLASS=t3.CODE  
                                               AND t.PIECE_KEY=t1.LOT_KEY
                                               AND t.TRANSACTION_KEY=t2.TRANSACTION_KEY 
                                               AND t4.ISFLAG=1
                                               AND t.UNDO_FLAG=0 ");
                if (!string.IsNullOrEmpty(protypes))
                    sqlCommand += string.Format(" AND t4.PROMODEL_NAME IN ({0})", protypes);
                if (!string.IsNullOrEmpty(locationkey))
                    sqlCommand += string.Format(" AND t1.FACTORYROOM_KEY='{0}'", locationkey);
                if (!string.IsNullOrEmpty(proids))
                    sqlCommand += string.Format(" AND t1.PRO_ID IN ({0})", proids);
                if (!string.IsNullOrEmpty(workorders))
                    sqlCommand += string.Format(" AND t1.WORK_ORDER_NO IN ({0}) ", workorders);
                if (!string.IsNullOrEmpty(startDate))
                    sqlCommand += string.Format(" AND t.TIME_STAMP>=CONVERT(DATETIME,'{0}')", startDate);
                if (!string.IsNullOrEmpty(endDate))
                    sqlCommand += string.Format(" AND t.TIME_STAMP<=CONVERT(DATETIME,'{0}')", endDate);
                if (!string.IsNullOrEmpty(partNumber))
                    sqlCommand += string.Format(" AND t1.PART_NUMBER in ({0}) ", partNumber);
                sqlCommand += " GROUP BY t3.CODE,t3.NAME ORDER BY t3.CODE ASC";


                DataTable dtDefactCommon = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtDefactCommon.TableName = LayoutViewType.ReportTable;
                dsReturn.Merge(dtDefactCommon, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _msgerror = ex.Message;
            }

            return dsReturn;
        }

        public DataSet GetDefectAccessByDay()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            DateTime dtStart = Convert.ToDateTime(startDate);
            DateTime dtEnd = Convert.ToDateTime(endDate);
            try
            {
                sqlCommand = @"SELECT t3.CODE,t3.NAME DEFECT_NAME";
                for (DateTime dtm = dtStart; dtm < dtEnd.AddDays(1); dtm = dtm.AddDays(1))
                {
                    DateTime dtm2 = dtm.AddDays(1);
                    sqlCommand += string.Format(@",SUM(CASE WHEN t.TIME_STAMP BETWEEN CONVERT(DATETIME,'{0}') AND CONVERT(DATETIME,'{1}') THEN 1 ELSE 0 END ) ""{2}"" ",
                        dtm.ToString(),
                        dtm2.ToString(), 
                        dtm.ToString());
                }
                sqlCommand += string.Format(@" FROM WIP_TRANSACTION t,POR_LOT t1,WIP_DEFECT t2 ,V_Basic_ClassOfRCode t3,POR_PRODUCT t4
                                               WHERE t1.PRO_ID=t4.PRODUCT_CODE 
                                               AND t2.REASON_CODE_CLASS=t3.CODE  
                                               AND t.PIECE_KEY=t1.LOT_KEY
                                               AND t.TRANSACTION_KEY=t2.TRANSACTION_KEY 
                                               AND t4.ISFLAG=1
                                               AND t.UNDO_FLAG=0");
                if (!string.IsNullOrEmpty(protypes))
                    sqlCommand += string.Format(" AND t4.PROMODEL_NAME IN ({0})", protypes);
                if (!string.IsNullOrEmpty(locationkey))
                    sqlCommand += string.Format(" AND t1.FACTORYROOM_KEY='{0}'", locationkey);
                if (!string.IsNullOrEmpty(proids))
                    sqlCommand += string.Format(" AND t1.PRO_ID IN ({0})", proids);
                if (!string.IsNullOrEmpty(workorders))
                    sqlCommand += string.Format(" AND t1.WORK_ORDER_NO IN ({0}) ", workorders);
                if (!string.IsNullOrEmpty(startDate))
                    sqlCommand += string.Format(" AND t.TIME_STAMP>=CONVERT(DATETIME,'{0}')", dtStart.ToString());
                if (!string.IsNullOrEmpty(endDate))
                    sqlCommand += string.Format(" AND t.TIME_STAMP<=CONVERT(DATETIME,'{0}')", Convert.ToDateTime(endDate).AddDays(1).ToString());
                if (!string.IsNullOrEmpty(partNumber))
                    sqlCommand += string.Format(" AND t1.PART_NUMBER in ({0}) ", partNumber);

                sqlCommand += " GROUP BY t3.CODE,t3.NAME ORDER BY t3.CODE ASC";


                DataTable dtDefactCommon = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                dtDefactCommon.TableName = LayoutViewType.ReportTable;
                dsReturn.Merge(dtDefactCommon, true, MissingSchemaAction.Add);
            }
            catch (Exception ex)
            {
                _msgerror = ex.Message;
            }

            return dsReturn;
        }

        public DataTable GetDefectByReasonCode()
        {
            DataTable dtReasonCode = new DataTable();
            string sqlCommand = string.Empty;
            try
            {
                sqlCommand = @"select convert(varchar,t.TIME_STAMP,120) TIME_STAMP,t1.STEP_NAME,t5.USERNAME,t6.SHIFT_NAME,t4.CUSTOMER,t4.CUSTOMER_TYPE,t2.WORK_ORDER_NO,t2.PART_NUMBER AS '产品料号',t3.PROMODEL_NAME,
                            t2.LOT_NUMBER LOT_NUMBER_DEFECT,t1.REASON_CODE_NAME,t1.DESCRIPTION
                            from WIP_TRANSACTION t 
                            inner join WIP_DEFECT t1 on t.TRANSACTION_KEY=t1.TRANSACTION_KEY
                            inner join POR_LOT t2 on t.PIECE_KEY=t2.LOT_KEY 
                            left join POR_PRODUCT t3 on t2.PRO_ID=t3.PRODUCT_CODE AND t3.ISFLAG=1
                            left join V_WORK_ORDER_ATTR t4 on t2.WORK_ORDER_NO=t4.ORDER_NUMBER
                            left join RBAC_USER t5 on t1.EDITOR=t5.BADGE
                            left join (select b.SHIFT_NAME,a.STARTTIME,a.ENDTIME
                            from CAL_SCHEDULE_DAY a left join CAL_SHIFT b on a.SHIFT_KEY=b.SHIFT_KEY
                            )t6 on t.TIME_STAMP between t6.STARTTIME and t6.ENDTIME
                            where 1=1 ";
                if (!string.IsNullOrEmpty(protypes))
                    sqlCommand += string.Format(" and t3.PROMODEL_NAME in ({0})", protypes);
                if (!string.IsNullOrEmpty(proids))
                    sqlCommand += string.Format(" and t2.PRO_ID in ({0})", proids);
                if (!string.IsNullOrEmpty(workorders))
                    sqlCommand += string.Format(" and t2.WORK_ORDER_NO in ({0})", workorders);
                if (!string.IsNullOrEmpty(reason_Code_Class))
                    sqlCommand += string.Format(" and t1.REASON_CODE_CLASS='{0}'", reason_Code_Class);
                if (!string.IsNullOrEmpty(locationkey))
                    sqlCommand += string.Format("  and t2.FACTORYROOM_KEY='{0}'", locationkey);
                if (!string.IsNullOrEmpty(startDate))
                    sqlCommand += string.Format("  and t.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                if (!string.IsNullOrEmpty(endDate))
                    sqlCommand += string.Format("  and t.TIME_STAMP<=CONVERT(datetime,'{0}')", endDate);
                if (!string.IsNullOrEmpty(partNumber))
                    sqlCommand += string.Format(" and t2.PART_NUMBER in ({0})", partNumber);

                dtReasonCode = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

            }
            catch (Exception ex)
            {
                _msgerror = ex.Message;
            }

            return dtReasonCode;
        }

        public DataTable BindReasonCodeClass()
        {
            DataTable dtReasonCode = new DataTable();
            string sqlCommand = @"SELECT A.* from (SELECT T.ITEM_ORDER,
                                MAX( case T.ATTRIBUTE_NAME when 'CODE' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as CODE,
                                MAX( case T.ATTRIBUTE_NAME when 'NAME' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as NAME
                                FROM CRM_ATTRIBUTE           T,
                                BASE_ATTRIBUTE          T1,
                                BASE_ATTRIBUTE_CATEGORY T2
                                WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY
                                AND T1.CATEGORY_KEY = T2.CATEGORY_KEY
                                AND UPPER(T2.CATEGORY_NAME) = 'Basic_ClassOfRCode'
                                GROUP BY T.ITEM_ORDER  
                                ) A ORDER BY A.ITEM_ORDER ASC";
            dtReasonCode = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

            return dtReasonCode;
        }

    }
}