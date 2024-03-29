﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DevExpress.Web;
using Astronergy.MES.Report.DataAccess;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using DevExpress.Web.Rendering;

/// <summary>
/// SE 日产出报表
/// </summary>
public partial class Customer_SE_Daily : BasePage
{
    private WipOutputDataAccess _dataAccess = new WipOutputDataAccess();
    
    Dictionary<string, string> dic = new Dictionary<string, string>()
    {
        {"Plan","Plan"},
        {"层压前EL测试","EL1 \r\n (层压前）"},
        {"清洁","Curing-in \r\n （固化）"},
        {"组件测试","Flasher/ Hipot \r\n（测试）"},
        {"终检","Final Inspec \r\n（终检）"},
        {"包装","FG(产出）"}
    };
    Dictionary<string, AutoResetEvent> dicAutoEvents = new Dictionary<string, AutoResetEvent>();

    /// <summary>
    /// 页面载入事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRoomData();
            BindCustomerType();
            DateTime dtNow = CommonFunction.GetCurrentDateTime();
            this.dateStart.Date = DateTimeHelper.GetWeekFirstDayMon(dtNow);
            this.dateEnd.Date = DateTimeHelper.GetWeekLastDaySun(dtNow);
        }
    }
    /// <summary>
    /// 工厂车间。
    /// </summary>
    private void BindRoomData()
    {
        DataTable dtWorkPlace = CommonFunction.GetFactoryWorkPlace();
        this.cmbWorkPlace.DataSource = dtWorkPlace;
        this.cmbWorkPlace.DataBind();
        this.cmbWorkPlace.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        this.cmbWorkPlace.SelectedIndex = 0;
    }
    /// <summary>
    /// 绑定客户类别。
    /// </summary>
    private void BindCustomerType()
    {
        DataTable dtCustomerType = CommonFunction.GetCustomerType();
        ASPxListBox lst = this.ddeCustomerType.FindControl("lstCustomerType") as ASPxListBox;
        if (lst != null)
        {
            lst.DataSource = dtCustomerType;
            lst.TextField = "CUSTOMER_TYPE";
            lst.ValueField = "CUSTOMER_TYPE";
            lst.DataBind();
            lst.Items.Insert(0, new ListEditItem("ALL", "ALL"));
        }
        
    }
    /// <summary>
    /// 查询SE 日运营报表数据。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this.dateStart.Text)
            || string.IsNullOrEmpty(this.dateEnd.Text))
        {
            return;
        }
        if ((this.dateEnd.Date - this.dateStart.Date).TotalDays < 0
            || (this.dateEnd.Date - this.dateStart.Date).TotalDays > 31)
        {
            return;
        }

        DataTable dt = this.TransferDatatable();
        Cache[Session.SessionID + "_SEDailyProduction"] = dt;
        this.grid.Columns.Clear();
        this.grid.AutoGenerateColumns = true;
        this.grid.DataSource = dt;
        this.grid.DataBind();
        this.grid.AutoGenerateColumns = false;
        this.UpdatePanelResult.Update();
    }

    /// <summary>
    /// 查询SE工序产量之后进行数据格式转换。
    /// </summary>
    /// <returns>行列转换后的数据表。</returns>
    private DataTable TransferDatatable()
    {
        DataTable dtDestData = new DataTable();
        dtDestData.Columns.Add("WORK_STATION");
        //查询自动增加的列名
        
        for(DateTime start=this.dateStart.Date;start<=this.dateEnd.Date;start=start.AddDays(1))
        {
            string col = start.ToString("yyyy-MM-dd");
            dtDestData.Columns.Add(string.Format("Day_{0}", col));
            dtDestData.Columns.Add(string.Format("Night_{0}", col));
            dtDestData.Columns.Add(string.Format("Sum_{0}", col));
            if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                int weekCount = DateTimeHelper.GetWeekOfYearFirstDay(start, DayOfWeek.Monday);
                string weekName = string.Format("WK{0}", weekCount.ToString("00"));
                dtDestData.Columns.Add(string.Format("Day_{0}", weekName));
                dtDestData.Columns.Add(string.Format("Night_{0}", weekName));
                dtDestData.Columns.Add(string.Format("Sum_{0}", weekName));
                dtDestData.Columns.Add(string.Format("Gap_{0}", weekName));
            }
        }
        //查询工序
        DateTime dtStart = DateTime.Now;

        foreach (string row in dic.Keys)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            dicAutoEvents.Add(row, autoEvent);
            DataRow drNew = dtDestData.NewRow();
            drNew["WORK_STATION"] = row;
            dtDestData.Rows.Add(drNew);

            ParameterizedThreadStart start = new ParameterizedThreadStart(SetDatatableData);
            Thread t = new Thread(start);
            t.Start(drNew);
        }
        WaitHandle.WaitAll(dicAutoEvents.Values.ToArray());
        dicAutoEvents.Clear();
        dicAutoEvents = null;
        //计算WK GAP
        var query = from item in dtDestData.Columns.OfType<DataColumn>()
                  where item.ColumnName.StartsWith("Sum_WK")
                  select item;
        foreach (DataColumn col in query)
        {
            string gapColumnName=col.ColumnName.Replace("Sum_WK","Gap_WK");
            for (int i = 0; i < dtDestData.Rows.Count; i++)
            {
                if (i == 0)
                {
                    dtDestData.Rows[i][gapColumnName] = "/";
                }
                else if (i == 1)
                {
                    dtDestData.Rows[i][gapColumnName] = 0;
                }
                else
                {
                    double curSum = 0;  //当前记录的SUM值。
                    double preSum = 0;  //上一条记录的SUM值。
                    double.TryParse(Convert.ToString(dtDestData.Rows[i][col]), out curSum);
                    double.TryParse(Convert.ToString(dtDestData.Rows[i-1][col]), out preSum);
                    dtDestData.Rows[i][gapColumnName] = curSum-preSum;
                }
            }
        }
        //添加差异
        int startIndex = dtDestData.Rows.Count;
        DataRow dr = dtDestData.NewRow();
        dr["WORK_STATION"] = "Gap \r\n -With Plan";
        dtDestData.Rows.Add(dr);
        //添加累计差异
        dr = dtDestData.NewRow();
        dr["WORK_STATION"] = "Cumulative Gap";
        dtDestData.Rows.Add(dr);
        //计算差异。
        query = from item in dtDestData.Columns.OfType<DataColumn>()
                where item.ColumnName.StartsWith("Sum")
                select item;
        double preGap = -1;
        foreach (DataColumn col in query)
        {
            string colName = col.ColumnName;
            string[] colNameItems = colName.Split('_');
            if (!colNameItems[1].StartsWith("WK"))
            {
                //差异：产出-计划
                double sumFG = 0;
                double sumPlan = 0;
                double.TryParse(Convert.ToString(dtDestData.Rows[startIndex - 1][col]), out sumFG);
                double.TryParse(Convert.ToString(dtDestData.Rows[0][col]), out sumPlan);

                dtDestData.Rows[startIndex][col] = sumFG - sumPlan;
                //累计差异：当前差异+前一次累计差异
                if (preGap == -1)
                {
                    preGap = sumFG - sumPlan;
                }
                else
                {
                    preGap = preGap + (sumFG - sumPlan);
                }
            }
            dtDestData.Rows[startIndex + 1][col] = preGap;
        }

        for (int i = 0; i < 6;i++ )
        {
            dtDestData.Rows[i]["WORK_STATION"] = dic[Convert.ToString(dtDestData.Rows[i]["WORK_STATION"])];
        }
        double mill = (DateTime.Now - dtStart).TotalMilliseconds;

        return dtDestData;
    }
    /// <summary>
    /// 设置工序产出数量
    /// </summary>
    /// <param name="obj"></param>
    private void SetDatatableData(object obj)
    {
        DataRow drNew = obj as DataRow;
        if (drNew == null) return;

        int queryType = 1;
        string roomName = this.cmbWorkPlace.Text;
        string customer = this.ddeCustomerType.Text.Trim();
        string workOrderNo = this.txtWorkOrderNumber.Text;
        string partNumber = this.textPartNumber.Text.ToString().Trim();

        string startTime = this.dateStart.Date.ToString("yyyy-MM-dd");
        string endTime = this.dateEnd.Date.ToString("yyyy-MM-dd");

        if (roomName.ToUpper() == "ALL") roomName = string.Empty;
        if (customer.ToUpper() == "ALL") customer = string.Empty;
        string row = Convert.ToString(drNew["WORK_STATION"]);


        DataTable dtSource = null;
        //获取产出计划数据。
        if (row == "Plan")
        {
            DataSet ds = this._dataAccess.GetPlanData(startTime,
                                                  endTime,
                                                  roomName,
                                                  string.Empty,
                                                  workOrderNo,
                                                  partNumber,
                                                  customer);
            if (ds != null
                && ds.Tables.Count > 0)
            {
                dtSource = ds.Tables[0];
            }
        }
        else
        {
            //获取工序产出数量
            DataSet ds = this._dataAccess.GetWIPOutputData(queryType,
                  startTime,
                  endTime,
                  roomName,
                  string.Empty,
                  workOrderNo,
                  row,  //工序名称
                  partNumber,
                  customer);
            if (ds != null
                && ds.Tables.Count > 0)
            {
                dtSource = ds.Tables[0];
            }
        }
        if (dtSource != null && dtSource.Rows.Count > 0)
        {
            double wkDay = 0;
            double wkNight = 0;
            double wkSum = 0;
            double dateDayNightSum = 0;  //每日白夜班数量和
            DataTable dtDestData = drNew.Table;
            for (int i = 1; i < dtDestData.Columns.Count; i++)
            {
                string col = dtDestData.Columns[i].ColumnName;
                string[] colItems = col.Split('_');
                if (colItems[1].StartsWith("WK"))
                {
                    if (colItems[0] == "Day")
                    {
                        drNew[col] = wkDay;
                        wkDay = 0;
                    }
                    else if (colItems[0] == "Night")
                    {
                        drNew[col] = wkNight;
                        wkNight = 0;
                    }
                    else if (colItems[0] == "Sum")
                    {
                        drNew[col] = wkSum;
                        wkSum = 0;
                    }
                    else //Gap
                    {

                    }
                }
                else
                {
                    if (colItems[0] == "Day")
                    {
                        double day = 0;

                        if (row == "Plan")
                        {
                            day = dtSource.AsEnumerable()
                                           .Where(dr => Convert.ToString(dr["DATA_DATE"]) == colItems[1]
                                                        && Convert.ToString(dr["SHIFT_NAME"]) == "白班")
                                           .Sum(dr => Convert.ToDouble(dr["OUT_QTY"]));
                        }
                        else
                        {
                            day = dtSource.AsEnumerable()
                                           .Where(dr => Convert.ToString(dr["OPERATION_NAME"]) == row
                                                        && Convert.ToString(dr["CALC_DATE"]) == colItems[1]
                                                        && Convert.ToString(dr["SHIFT_NAME"]) == "白班")
                                           .Sum(dr => Convert.ToDouble(dr["OUT_QTY"]));
                        }
                        drNew[col] = day;
                        dateDayNightSum += day;
                        wkDay += day;
                    }
                    else if (colItems[0] == "Night" && dtSource != null)
                    {
                        double night = 0;
                        if (row == "Plan")
                        {

                            night = dtSource.AsEnumerable()
                                           .Where(dr => Convert.ToString(dr["DATA_DATE"]) == colItems[1]
                                                        && Convert.ToString(dr["SHIFT_NAME"]) == "夜班")
                                           .Sum(dr => Convert.ToDouble(dr["OUT_QTY"]));
                        }
                        else
                        {

                            night = dtSource.AsEnumerable()
                                           .Where(dr => Convert.ToString(dr["OPERATION_NAME"]) == row
                                                        && Convert.ToString(dr["CALC_DATE"]) == colItems[1]
                                                        && Convert.ToString(dr["SHIFT_NAME"]) == "夜班")
                                           .Sum(dr => Convert.ToDouble(dr["OUT_QTY"]));
                        }
                        drNew[col] = night;
                        dateDayNightSum += night;
                        wkNight += night;
                    }
                    else if (colItems[0] == "Sum")
                    {
                        drNew[col] = dateDayNightSum;
                        wkSum += dateDayNightSum;
                        dateDayNightSum = 0;
                    }
                }
            }
        }
        dicAutoEvents[row].Set();
    }
    /// <summary>
    /// 导出EXCEL。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXlsExport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this.dateStart.Text)
        || string.IsNullOrEmpty(this.dateEnd.Text))
        {
            return;
        }
        if ((this.dateEnd.Date - this.dateStart.Date).TotalDays < 0
            || (this.dateEnd.Date - this.dateStart.Date).TotalDays > 31)
        {
            return;
        }

        DataTable dt = Cache[Session.SessionID + "_SEDailyProduction"]  as DataTable;
        if(dt==null){
            dt=this.TransferDatatable();
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        Response.AppendHeader("content-disposition", "attachment;filename=\"SEDailyProduction.xls\"");
        Response.ContentType = "Application/ms-excel";
        Export.ExportToSEDailyProductionExcel(Response.OutputStream,dt);
        Response.Flush();
        Response.Close();
        Response.End();
    }
    /// <summary>
    /// 自定义文本。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        
    }

    /// <summary>
    /// 行创建时触发事件，用于合并单元格。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        ASPxGridView gv=sender as ASPxGridView;
        if(gv!=null && e.RowType==GridViewRowType.Data)
        {
            
            DataTable dtSource = gv.DataSource as DataTable;
            if (dtSource == null) return;

            if(e.VisibleIndex==0)
            {
                TableRow targetHeader = e.Row;
                SplitTableHeader(targetHeader, dtSource);
            }
            else if (e.VisibleIndex == 5)
            {
                for (int i = 1; i < e.Row.Cells.Count-1; i++)
                {
                    string colName = dtSource.Columns[i].ColumnName;
                    string [] colNameItems=colName.Split('_');
                    if (colNameItems[0] == "Day")
                    {
                        e.Row.Cells[i].ColumnSpan = 2;
                        e.Row.Cells[i + 1].Visible = false;
                        e.Row.Cells[i].Text = Convert.ToString(dtSource.Rows[e.VisibleIndex][i + 2]);
                        i = i + 2;
                    }
                }
            }
            else if (e.VisibleIndex >=6 && e.VisibleIndex<=7)
            {
                for (int i = 1; i < e.Row.Cells.Count-1; i++)
                {
                    string colName = dtSource.Columns[i].ColumnName;
                    string[] colNameItems = colName.Split('_');
                    if (colNameItems[0] == "Day")
                    {

                        e.Row.Cells[i].ColumnSpan = 3;
                        e.Row.Cells[i].Text = Convert.ToString(dtSource.Rows[e.VisibleIndex][i + 2]);
                        e.Row.Cells[i + 1].Visible = false;
                        e.Row.Cells[i + 2].Visible = false;
                        if (colNameItems[1].StartsWith("WK"))
                        {
                            e.Row.Cells[i].RowSpan = 2;
                            
                            e.Row.Cells[i+3].RowSpan = 2;
                            e.Row.Cells[i + 3].Text = "/";
                            if (e.VisibleIndex == 7)
                            {
                                e.Row.Cells[i].Visible = false;
                                e.Row.Cells[i + 3].Visible = false;
                            }
                            else
                            {
                                e.Row.Cells[i].Text = Convert.ToString(dtSource.Rows[e.VisibleIndex + 1][i + 2]);
                            }
                        }
                        i = i + 2;
                    }
                    
                }
            }
        }
    }


    void SplitTableHeader(TableRow targetHeader, DataTable dtSource)
    {
        if (dtSource == null) return;

        Table table = targetHeader.Parent as Table;
        table.Rows.RemoveAt(0);
        int row=3;
        int col = dtSource.Columns.Count;
        for (int k = 0; k < row; k++)
        {
            TableRow trow = new TableRow();
            trow.Height = 20;
            for (int i = 0; i < col;)
            {
                string colName = dtSource.Columns[i].ColumnName;
                string [] colNameItems=colName.Split('_');
                TableCell cell = new TableCell();
                cell.CssClass = "dxgvHeader";
                cell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                cell.Style.Add(HtmlTextWriterStyle.BorderCollapse, "collapse");
                cell.Style.Add("BORDER-LEFT-WIDTH", "0px");
                cell.Style.Add("BORDER-TOP-WIDTH", "0px");

                if (k < 2 && colNameItems[0]=="Day")
                {
                    cell.ColumnSpan = 3;
                    if (k == 0)
                    {
                        cell.Text = colNameItems[1];
                        if (colNameItems[1].StartsWith("WK"))
                        {
                            cell.ColumnSpan += 1;
                            cell.RowSpan = 2;
                            cell.Text = colNameItems[1] + " Total";
                            i = i + 4;
                        }
                        else
                        {
                            i = i + 3;
                        }
                    }
                    else if (k == 1)
                    {
                        if (!colNameItems[1].StartsWith("WK"))
                        {
                            cell.Text = Enum.GetName(typeof(DayOfWeek), DateTime.Parse(colNameItems[1]).DayOfWeek);
                            i = i + 3;
                        }
                        else
                        {
                            cell.ColumnSpan += 1;
                            i = i + 4;
                            continue;
                        }
                    }
                    trow.Cells.Add(cell);
                    continue;
                }
                else if (k==0 && i == 0)
                {
                    cell.RowSpan = 3;
                    cell.Text = "Working station";
                    trow.Cells.Add(cell);
                }
                else if (k >= 2 && i > 0)
                {
                    cell.Text = colNameItems[0];
                    trow.Cells.Add(cell);
                }
                i++;
            }
            table.Rows.AddAt(k, trow);
        }
    }  
    /// <summary>
    /// 导出数据时重新设置文本值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
    {
        //DataTable dt = this.gridExport.DataSource as DataTable;
        //if (dt != null
        //    && e.RowType == DevExpress.Web.GridViewRowType.Data
        //    && e.Column != null 
        //    && e.Column.Index > 1 
        //    && e.Column.Index < dt.Columns.Count-1)
        //{
        //    string trxType = Convert.ToString(dt.Rows[e.VisibleIndex][1]);
        //    if (trxType.EndsWith("RATE"))
        //    {
        //        e.TextValueFormatString = "#0.00%";
        //    }
        //}
    }
    /// <summary>
    /// 数据绑定时触发事件。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_DataBound(object sender, EventArgs e)
    {
        //ASPxGridView gv = sender as ASPxGridView;
        //if (gv != null && gv.Columns.Count > 1)
        //{
        //    gv.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
        //    gv.Columns[0].CellStyle.BackColor = Color.FromName("#ffffd6");
        //}
    }
}

