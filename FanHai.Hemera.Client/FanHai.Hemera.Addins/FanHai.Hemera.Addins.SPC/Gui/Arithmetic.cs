/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Constants;
using System.IO;

namespace FanHai.Hemera.Addins.SPC
{
    public static class Arithmetic
    {
        #region Calculate point average value  计算平均值
        /// <summary>
        /// 计算平均值
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <returns>不分组所有数据的平均值</returns>
        public static double CalculateXAverageValue(DataTable dataTable)
        {
            decimal xAverage = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                xAverage += Convert.ToDecimal(dataRow[SPC_PARAM_DATA_FIELDS.V_VALUE]);
            }
            xAverage = xAverage / dataTable.Rows.Count;
            return Convert.ToDouble(Math.Round(xAverage, 10));
        }
        /// <summary>
        /// 计算数组平均值
        /// </summary>
        /// <param name="xValueList">平均值得数据源</param>
        /// <returns>平均后数值</returns>
        public static decimal CalculateXAvg(List<decimal> xValueList)
        {
            decimal xAverage = 0;
            for (int i = 0; i < xValueList.Count; i++)
            {
                xAverage += xValueList[i];
            }
            xAverage = xAverage / xValueList.Count;
            return Math.Round(xAverage, 10);
        }
        #endregion

        #region 计算总平均值
        /// <summary>
        /// 计算总平均值
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="n">样本容量大小</param>
        /// <param name="xAvgList">每组样本平均值</param>
        /// <returns>总平均值</returns>
        public static double CalculateTotalXAverage(DataTable dataTable, int n, out List<SpcPoints> xAvgList)
        {
            //总平均值
            decimal xTotalAvg = 0; 
            //存放子组数量
            xAvgList = new List<SpcPoints>();

            for (int i = 0; i < dataTable.Rows.Count; i = i + n)
            {
                //每组样本平均值
                decimal xAvg = 0; 
                //单点
                SpcPoints points = new SpcPoints();
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) < dataTable.Rows.Count)
                    {
                        string colKey = dataTable.Rows[i + j][SPC_PARAM_DATA_FIELDS.COL_KEY].ToString();
                        decimal value = Convert.ToDecimal(dataTable.Rows[i + j][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]);
                        KeyValuePair<string, decimal> kvp = new KeyValuePair<string, decimal>(colKey, value);
                        points.listPoint.Add(kvp);
                        xAvg += value;
                    }
                }
                //xAvg = xAvg / n;
                xAvg = xAvg / points.listPoint.Count;
                points.value = Convert.ToDouble(Math.Round(xAvg, 4));
                xAvgList.Add(points);
                xTotalAvg += xAvg;
            }
            //计算总的平均值
            //xTotalAvg=xTotalAvg/(dataTable.Rows.Count/n);
            xTotalAvg = xTotalAvg / Math.Ceiling(Convert.ToDecimal(dataTable.Rows.Count / n));
            return Convert.ToDouble(Math.Round(xTotalAvg, 10));
        }
        #endregion
        #region 计算总平均值—NEW
        /// <summary>
        /// 计算总平均值
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="xAvgList">每组样本平均值</param>
        /// <returns>总平均值</returns>
        public static double CalculateTotalXAverage(DataTable dataTable, DataTable dataTable_Group, out List<SpcPoints> xAvgList)
        {
            decimal xTotalAvg = 0; //总平均值
            xAvgList = new List<SpcPoints>();

            foreach (DataRow dr in dataTable_Group.Rows)
            {
                DataRow[] dr_dataTable = dataTable.Select(string.Format("EDC_INS_KEY='{0}'", dr["EDC_INS_KEY"].ToString()));
                if (dr_dataTable == null || dr_dataTable.Length == 0) continue;


                decimal xAvg = 0; //每组样本平均值
                SpcPoints points = new SpcPoints();
                for (int i = 0; i < dr_dataTable.Length; i++)
                {
                    string colKey = dr_dataTable[i][SPC_PARAM_DATA_FIELDS.COL_KEY].ToString();
                    decimal value = Convert.ToDecimal(dr_dataTable[i][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]);
                    KeyValuePair<string, decimal> kvp = new KeyValuePair<string, decimal>(colKey, value);
                    points.listPoint.Add(kvp);
                    xAvg += value;
                }

                //是否被修改过
                points.editFlag = Convert.ToInt32(dr_dataTable[0][SPC_PARAM_DATA_FIELDS.EDIT_FLAG].ToString());
                //点的创建日期
                points.createTime = dr[SPC_PARAM_DATA_FIELDS.CREATE_TIME].ToString();
                //点的供应商
                points.supplier = dr[SPC_PARAM_DATA_FIELDS.SUPPLIER].ToString();
                //点的批次号
                points.lotNumber = dr[SPC_PARAM_DATA_FIELDS.LOT_NUMBER].ToString();
                //数据采集键值
                points.edc_ins_key = dr[SPC_PARAM_DATA_FIELDS.EDC_INS_KEY].ToString();

                //xAvg = xAvg / n;
                xAvg = xAvg / points.listPoint.Count;
                points.value = Convert.ToDouble(Math.Round(xAvg, 4));
                xAvgList.Add(points);
                xTotalAvg += xAvg;
            }
            //xTotalAvg=xTotalAvg/(dataTable.Rows.Count/n);
            //xTotalAvg = xTotalAvg / Math.Ceiling(Convert.ToDecimal(dataTable.Rows.Count / dataTable_Group.Rows.Count));

            xTotalAvg = xTotalAvg / dataTable_Group.Rows.Count; //dataTable_Group.Rows.Count 表示点数
            //返回总的平均值
            return Convert.ToDouble(Math.Round(xTotalAvg, 5));
        }
        #endregion
        #region Calculate MR average value 计算移动极差平均数
        /// <summary>
        /// 计算平均极差值
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="mrValueList">移动极差值</param>
        /// <returns>平均移动极差值</returns>
        public static double CalculateMRAverageValue(DataTable dataTable, out List<double> mrValueList)
        {
            decimal mrAverage = 0;
            decimal mr = 0;
            decimal x1 = 0;
            decimal x2 = 0;
            mrValueList = new List<double>();
            for (int i = 0; i < dataTable.Rows.Count - 1; i++)
            {
                x1 = Convert.ToDecimal(dataTable.Rows[i][SPC_PARAM_DATA_FIELDS.V_VALUE]);
                x2 = Convert.ToDecimal(dataTable.Rows[i + 1][SPC_PARAM_DATA_FIELDS.V_VALUE]);
                mr = Math.Abs(x1 - x2);
                mrValueList.Add(Convert.ToDouble(Math.Round(mr, 4)));
                mrAverage += mr;
            }
            mrAverage = mrAverage / (dataTable.Rows.Count - 1);
            return Convert.ToDouble(Math.Round(mrAverage, 4));
        }
        #endregion

        #region 计算平均极差
        /// <summary>
        /// 计算总的平均极差
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="n">样本容量</param>
        /// <param name="mrAvgList">每组样本极差</param>
        /// <returns>平均极差</returns>
        public static double CalculateTotalRAvg(DataTable dataTable, int n, out List<double> rAvgList)
        {
            decimal rTotalAvg = 0; //样本平均极差值
            rAvgList = new List<double>();
            for (int i = 0; i < dataTable.Rows.Count; i = i + n)
            {
                decimal rAvg = 0;//每组样本极差值
                List<decimal> listParam = new List<decimal>();
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) < dataTable.Rows.Count)
                    {
                        listParam.Add(Convert.ToDecimal(dataTable.Rows[i + j][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]));
                    }
                }
                rAvg = listParam.Max() - listParam.Min();
                rAvgList.Add(Convert.ToDouble(Math.Round(rAvg, 4)));
                rTotalAvg += rAvg;
            }
            rTotalAvg = rTotalAvg / Math.Ceiling(Convert.ToDecimal((dataTable.Rows.Count / n)));
            return Convert.ToDouble(Math.Round(rTotalAvg, 10));
        }
        #endregion

        #region 计算平均极差-NEW      
        public static double CalculateTotalRAvg(DataTable dataTable, DataTable valueTable_Group, out List<double> rAvgList)
        {
            //样本平均极差值
            decimal rTotalAvg = 0;
            //计算子组的平均值——形成点数，显示在控制图中
            rAvgList = new List<double>();

            foreach (DataRow dr in valueTable_Group.Rows)
            {
                DataRow[] dr_dataTable = dataTable.Select(string.Format("EDC_INS_KEY='{0}'", dr["EDC_INS_KEY"].ToString()));
                if (dr_dataTable == null || dr_dataTable.Length == 0) continue;

                //每组样本极差值
                decimal rAvg = 0;
                //SpcPoints points = new SpcPoints();
                List<decimal> listParam = new List<decimal>();
                for (int i = 0; i < dr_dataTable.Length; i++)
                {
                    //把子组数据记录起来
                    listParam.Add(Convert.ToDecimal(dr_dataTable[i][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]));
                }
                //求出来子组的极差值
                rAvg = listParam.Max() - listParam.Min();
                rAvgList.Add(Convert.ToDouble(Math.Round(rAvg, 4)));
                //子组极差值累加
                rTotalAvg += rAvg;
            }
            //因为整个查询中，子组的数量不一致，所以只能求的总体子组数的平均值
            //dataTable.Rows.Count / valueTable_Group.Rows.Count 表示平均的点数
            //子组平均值，是根据总的数量/总数量的分组数

            //以平均点数算平均极差
            rTotalAvg = rTotalAvg / Math.Ceiling(Convert.ToDecimal((dataTable.Rows.Count / valueTable_Group.Rows.Count)));
            //返回平均极差
            return Convert.ToDouble(Math.Round(rTotalAvg, 10));
        }
        #endregion

        #region 计算样本标准差
        /// <summary>
        /// 计算每个样本的标准差
        /// </summary>
        /// <param name="xValueList">样本原始数据</param>
        /// <returns>样本标准差</returns>
        public static double CalculateS(List<decimal> xValueList)
        {
            if (xValueList.Count == 0) return 0;

            double s = 0;
            double tempValue = 0;
            decimal xAvg = CalculateXAvg(xValueList);
            for (int i = 0; i < xValueList.Count; i++)
            {
                tempValue += Math.Pow(Convert.ToDouble(xValueList[i] - xAvg), 2);
            }
            int tmp = 0;
            if (xValueList.Count == 1)
                tmp = 1;
            else
                tmp = xValueList.Count - 1;

            s = tempValue / tmp;
            s = Math.Round(Math.Sqrt(s), 10);
           
            return s;
        }
        #endregion

        #region 计算平均标准差
        /// <summary>
        /// 计算样本平均标准差
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="n">样本容量</param>
        /// <param name="sList">每个样本的标准差</param>
        /// <returns>平均标准差</returns>
        public static double CalculateAvgS(DataTable dataTable, int n, out List<double> sList)
        {
            sList = new List<double>();
            double avgS = 0;
            for (int i = 0; i < dataTable.Rows.Count; i = i + n)
            {
                double s = 0;
                List<decimal> xValueList = new List<decimal>();
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) < dataTable.Rows.Count)
                    {
                        xValueList.Add(Convert.ToDecimal(dataTable.Rows[i + j][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]));
                    }
                }
                s = CalculateS(xValueList);
                avgS += s;
                sList.Add(Math.Round(s, 4));
            }
            avgS = avgS / Math.Ceiling(Convert.ToDouble((dataTable.Rows.Count / n)));
            return Math.Round(avgS, 10);
        }
        #endregion
        #region
        /// <summary>
        /// 计算样本平均标准差
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="dataTable_Group">原始点数</param>
        /// <param name="sList">每个样本的标准差</param>
        /// <returns>平均标准差</returns>
        public static double CalculateAvgS(DataTable dataTable, DataTable dataTable_Group, out List<double> sList)
        {
            int demominator = 0;
            sList = new List<double>();
            double avgS = 0, avgS_bak = 0;
            foreach (DataRow dr in dataTable_Group.Rows)
            {
                DataRow[] dr_dataTable = dataTable.Select(string.Format("EDC_INS_KEY='{0}'", dr["EDC_INS_KEY"].ToString()));
                if (dr_dataTable == null) break;

                double s = 0;
                List<decimal> xValueList = new List<decimal>();
                for (int i = 0; i < dr_dataTable.Length; i++)
                {
                    xValueList.Add(Convert.ToDecimal(dr_dataTable[i][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]));
                }
                double tmp = 0;
                s = CalculateS(xValueList);
                tmp = Math.Round(avgS + s, 10);
                avgS = tmp;
                sList.Add(Math.Round(s, 4));
            }

            //avgS = avgS / Math.Ceiling(Convert.ToDouble((dataTable.Rows.Count / dataTable_Group.Rows.Count)));

            demominator = dataTable_Group.Rows.Count == 0 ? 1 : dataTable_Group.Rows.Count;
            avgS_bak = avgS / demominator;
            return Math.Round(avgS_bak, 10);
        }
        #endregion
        #region 计算X-MR 控制图管制线
        /// <summary>
        /// 计算X-MR图的管制线
        /// </summary>
        /// <param name="xAvg">总平均值</param>
        /// <param name="mrAvg">移动极差平均值</param>
        /// <returns>X图管制上线、X图管制中线、X图管制下线；MR图管制上线、MR管制中线、MR管制下线</returns>
        public static IDictionary CalculateXMRControlLine(double xAvg, double mrAvg)
        {
            Dictionary<string, double> xrChartDic = new Dictionary<string, double>();
            double xCl = 0;    //X:管制中线
            double xUcl = 0;   //X:管制上线
            double xLcl = 0;   //X:管制下线         
            double xrCl = 0;   //MR:管制中线
            double xrUcl = 0;  //MR:管制上线
            double xrLcl = 0;  //MR:管制下线
            double k1 = 2.66;
            double k2 = 3.267;
            xCl = Math.Round(xAvg, 4);
            xrCl = Math.Round(mrAvg, 4);
            xUcl = Math.Round(xAvg + k1 * mrAvg, 4);
            xLcl = Math.Round(xAvg - k1 * mrAvg, 4);
            xrUcl = Math.Round(k2 * mrAvg, 4);
            xrLcl = 0;
            xrChartDic.Add("xCl", xCl);
            xrChartDic.Add("xUcl", xUcl);
            xrChartDic.Add("xLcl", xLcl);
            xrChartDic.Add("xrCl", xrCl);
            xrChartDic.Add("xrUcl", xrUcl);
            xrChartDic.Add("xrLcl", xrLcl);
            return xrChartDic;
        }
        #endregion

        #region 计算Xbar-R图管制线
        /// <summary>
        /// 计算Xbar-R图管制线
        /// </summary>
        /// <param name="xTotalAvg">总平均值</param>
        /// <param name="rTotalAvg">平均极差值</param>
        /// <param name="d2">d2</param>
        /// <param name="d3">d3</param>
        /// <param name="n">样本容量大小</param>
        /// <returns>X管制上线、X管制中线、X管制下线、R管制上线、R管制中线、R管制下线</returns>
        public static IDictionary CalculateXbar_RControlLine(double xTotalAvg, double rTotalAvg, double d2, double d3, int n)
        {
            Dictionary<string, double> xbarRCtrlLineDic = new Dictionary<string, double>();
            double xCL = 0, xUCL = 0, xLCL = 0;
            double rCL = 0, rUCL = 0, rLCL = 0;
            //double A2 = 0;
            double D3 = 0;
            double D4 = 0;
            double eValue = 0;


            xCL = Math.Round(xTotalAvg, 3);
            rCL = Math.Round(rTotalAvg, 3);
            eValue = rTotalAvg / (Math.Sqrt(n) * d2);
            xUCL = Math.Round(xTotalAvg + 3 * eValue, 3);
            xLCL = Math.Round(xTotalAvg - 3 * eValue, 3);

            D3 = 1 - 3 * d3 / d2;
            D4 = 1 + 3 * d3 / d2;
            rUCL = Math.Round(D4 * rTotalAvg, 3);
            rLCL = Math.Round(D3 * rTotalAvg < 0 ? 0 : D3 * rTotalAvg, 3);
            xbarRCtrlLineDic.Add("xCL", xCL);
            xbarRCtrlLineDic.Add("xUCL", xUCL);
            xbarRCtrlLineDic.Add("xLCL", xLCL);
            xbarRCtrlLineDic.Add("rCL", rCL);
            xbarRCtrlLineDic.Add("rUCL", rUCL);
            xbarRCtrlLineDic.Add("rLCL", rLCL);
            xbarRCtrlLineDic.Add("eValue", eValue);
            return xbarRCtrlLineDic;
        }
        #endregion
        #region 计算Xbar-R图控制线——New
        /// <summary>
        /// 计算Xbar-R图控制线
        /// </summary>
        /// <param name="xTotalAvg">总平均值</param>
        /// <param name="rTotalAvg">平均极差值</param>
        /// <param name="n">平均子组数量</param>
        /// <returns>返回：X管制上线、X管制中线、X管制下线、R管制上线、R管制中线、R管制下线</returns>
        public static IDictionary CalculateXbar_RControlLine(double xTotalAvg, double rTotalAvg, int n)
        {
            double d2 = 0, d3 = 0;
            d2 = new ConstantSPC("D2").GetConstantItem(n).Value;
            d3 = new ConstantSPC("D3").GetConstantItem(n).Value;

            Dictionary<string, double> xbarRCtrlLineDic = new Dictionary<string, double>();
            double xCL = 0, xUCL = 0, xLCL = 0;
            double rCL = 0, rUCL = 0, rLCL = 0;
            //double A2 = 0;
            double D3 = 0;
            double D4 = 0;
            double eValue = 0;


            xCL = Math.Round(xTotalAvg, 4);
            rCL = Math.Round(rTotalAvg, 4);
            eValue = rTotalAvg / (Math.Sqrt(n) * d2);
            xUCL = Math.Round(xTotalAvg + 3 * eValue, 4);
            xLCL = Math.Round(xTotalAvg - 3 * eValue, 4);

            D3 = 1 - 3 * d3 / d2;
            D4 = 1 + 3 * d3 / d2;
            rUCL = Math.Round(D4 * rTotalAvg, 4);
            rLCL = Math.Round(D3 * rTotalAvg < 0 ? 0 : D3 * rTotalAvg, 4);
            xbarRCtrlLineDic.Add("xCL", xCL);
            xbarRCtrlLineDic.Add("xUCL", xUCL);
            xbarRCtrlLineDic.Add("xLCL", xLCL);
            xbarRCtrlLineDic.Add("rCL", rCL);
            xbarRCtrlLineDic.Add("rUCL", rUCL);
            xbarRCtrlLineDic.Add("rLCL", rLCL);
            xbarRCtrlLineDic.Add("eValue", eValue);
            return xbarRCtrlLineDic;
        }
        #endregion

        #region 计算Xbar-S管制线
        /// <summary>
        /// 计算Xbar-S管制线
        /// </summary>
        /// <param name="xTotalAvg">样本总平均值</param>
        /// <param name="avgS">样本平均标准差</param>
        /// <param name="C4"></param>
        /// <param name="n">样本容量</param>
        /// <returns>X管制上线、X管制中线、X管制下线、S管制上线、S管制中线、S管制下线</returns>
        public static IDictionary CalculateXbar_SControlLine(double xTotalAvg, double avgS, double C4, int n)
        {
            Dictionary<string, double> xbarSCtrlLineDic = new Dictionary<string, double>();
            double xCL = 0, xUCL = 0, xLCL = 0;
            double sCL = 0, sUCL = 0, sLCL = 0;
            double A3 = 0, B3 = 0, B4 = 0, tempValue = 0;
            double eValue = 0; //sigma

            xCL = Math.Round(xTotalAvg, 4);
            sCL = Math.Round(avgS, 4);

            //eValue = avgS / (Math.Sqrt(Convert.ToDouble(n)) * C4);
            //计算Sigma
            eValue = avgS / C4;

            A3 = 3 / (Math.Sqrt(Convert.ToDouble(n)) * C4);
            tempValue = (Math.Sqrt(1 - Math.Pow(C4, 2))) / C4;
            B3 = 1 - 3 * tempValue;
            B4 = 1 + 3 * tempValue;

            xUCL = Math.Round(xTotalAvg + A3 * avgS, 4);
            xLCL = Math.Round(xTotalAvg - A3 * avgS, 4);

            sUCL = Math.Round(B4 * avgS, 4);
            sLCL = Math.Round(B3 * avgS, 4);
            xbarSCtrlLineDic.Add("xCL", xCL);
            xbarSCtrlLineDic.Add("xUCL", xUCL);
            xbarSCtrlLineDic.Add("xLCL", xLCL);

            xbarSCtrlLineDic.Add("sCL", sCL);
            xbarSCtrlLineDic.Add("sUCL", sUCL);
            xbarSCtrlLineDic.Add("sLCL", sLCL);
            xbarSCtrlLineDic.Add("eValue", eValue);
            return xbarSCtrlLineDic;
        }
        #endregion
        #region 计算XBAR-S 的管制线—NEW
        /// <summary>
        /// 计算XBAR-S 的管制线
        /// </summary>
        /// <param name="xTotalAvg">样本总平均值</param>
        /// <param name="avgS">样本标准差</param>
        /// <param name="C4"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IDictionary CalculateXbar_SControlLine(double xTotalAvg, double avgS, int n)
        {
            double C4 = 0;
            C4 = new ConstantSPC("C4").GetConstantItem(n).Value;

            Dictionary<string, double> xbarSCtrlLineDic = new Dictionary<string, double>();
            double xCL = 0, xUCL = 0, xLCL = 0;
            double sCL = 0, sUCL = 0, sLCL = 0;
            double A3 = 0, B3 = 0, B4 = 0, tempValue = 0;
            double eValue = 0; //sigma

            xCL = Math.Round(xTotalAvg, 4);
            sCL = Math.Round(avgS, 4);

            //eValue = avgS / (Math.Sqrt(Convert.ToDouble(n)) * C4);
            //计算Sigma
            eValue = avgS / C4;

            A3 = 3 / (Math.Sqrt(Convert.ToDouble(n)) * C4);
            tempValue = (Math.Sqrt(1 - Math.Pow(C4, 2))) / C4;
            B3 = 1 - 3 * tempValue;
            B4 = 1 + 3 * tempValue;

            xUCL = Math.Round(xTotalAvg + A3 * avgS, 4);
            xLCL = Math.Round(xTotalAvg - A3 * avgS, 4);

            sUCL = Math.Round(B4 * avgS, 4);
            sLCL = Math.Round(B3 * avgS, 4);
            xbarSCtrlLineDic.Add("xCL", xCL);
            xbarSCtrlLineDic.Add("xUCL", xUCL);
            xbarSCtrlLineDic.Add("xLCL", xLCL);

            xbarSCtrlLineDic.Add("sCL", sCL);
            xbarSCtrlLineDic.Add("sUCL", sUCL);
            xbarSCtrlLineDic.Add("sLCL", sLCL);
            xbarSCtrlLineDic.Add("eValue", eValue);
            return xbarSCtrlLineDic;
        }
        #endregion
        #region 计算Sigma（样本方差是总体方差的无偏估计量）
        /// <summary>
        /// 计算Sigma
        /// </summary>
        /// <param name="xAvg">样本总平均值</param>
        /// <param name="table">样本数据</param>
        /// <returns>sigma</returns>
        /// Owner genchille.yang 2012-05-08 16:29:29
        public static double CalculateSigma(decimal xAvg, DataTable table)
        {
            double Sigma = 0;
            decimal tempValue = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                decimal value = Convert.ToDecimal(table.Rows[i][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]);
                tempValue += (value - xAvg) * (value - xAvg);
            }
            Sigma = Math.Sqrt(Convert.ToDouble(tempValue / (table.Rows.Count)));
            return Math.Round(Sigma, 5);
        }
        #endregion

        #region 计算Sigma的估计值
        #endregion

        #region 计算Cp
        /// <summary>
        /// 计算过程能力指数Cp
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="eSigma">Sigma的估计值</param>
        /// <returns>Cp</returns>
        public static double CalculateCp(double USL, double LSL, double eSigma)
        {
            double cp = 0;
            if (eSigma != 0)
            {
                cp = (USL - LSL) / (6 * eSigma);
            }
            return Math.Round(cp, 4);
        }
        #endregion

        #region 计算Cpk
        /// <summary>
        /// 计算过程能力指数Cpk
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="eSigma">Sigma的估计值</param>
        /// <param name="xAvg">样本平均值</param>
        /// <returns>Cpk</returns>
        public static double CalculateCpk(double USL, double LSL, double eSigma, double xAvg)
        {
            double Cpu = 0;
            double Cpl = 0;
            double Cpk = 0;
            if (eSigma != 0)
            {
                Cpu = (USL - xAvg) / (3 * eSigma);
                Cpl = (xAvg - LSL) / (3 * eSigma);
                Cpk = Cpu < Cpl ? Cpu : Cpl; //Cpk 取Cpu和Cpl中的小者
            }
            return Math.Round(Cpk, 4);
        }
        #endregion

        #region 计算Cpm
        /// <summary>
        /// 计算过程能力指数Cpm
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="eSigma">Sigma的估计值</param>
        /// <param name="xAvg">样本平均值</param>
        /// <param name="tagertValue">目标值</param>
        /// <returns>Cpm</returns>
        public static double CalculateCpm(double USL, double LSL, double eSigma, double xAvg, double tagertValue)
        {
            double Cpm = 0;
            double denominator = Math.Sqrt(eSigma * eSigma + Math.Pow((xAvg - tagertValue), 2));
            Cpm = (USL - LSL) / (6 * denominator);
            return Math.Round(Cpm, 4);
        }
        #endregion

        #region 计算K
        /// <summary>
        /// 计算偏移系数K
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规则下限</param>
        /// <param name="xAvg">样本平均值</param>
        /// <returns>K</returns>
        public static double CalculateK(double USL, double LSL, double xAvg)
        {
            double k = 0;
            if ((USL - LSL) != 0)//分母不能为0
            {
                double denominator = (USL - LSL) / 2;
                double numerator = Math.Abs(xAvg - (USL + LSL) / 2);
                k = numerator / denominator;
            }
            return Math.Round(k, 4);
        }
        #endregion

        #region 计算Pp
        /// <summary>
        /// 计算过程性能指数Pp
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="sigma">Sigma</param>
        /// <returns>Pp</returns>
        public static double CalculatePp(double USL, double LSL, double sigma)
        {
            double Pp = 0;
            if (sigma != 0)
            {
                Pp = (USL - LSL) / (6 * sigma);
            }
            return Math.Round(Pp, 4);
        }
        #endregion

        #region 计算Ppk
        /// <summary>
        /// 计算过程性能指数Ppk
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="xAvg">样本平均值</param>
        /// <param name="sigma">sigma</param>
        /// <returns>Ppk</returns>
        public static double CalculatePpk(double USL, double LSL, double xAvg, double sigma)
        {
            double Ppk = 0;
            if (sigma != 0)
            {
                double Ppu = (USL - xAvg) / (3 * sigma);
                double Ppl = (xAvg - LSL) / (3 * sigma);
                Ppk = Ppu < Ppl ? Ppu : Ppl;
            }
            return Math.Round(Ppk, 4);
        }
        #endregion

        #region 计算Ppm
        /// <summary>
        /// 计算Ppm
        /// </summary>
        /// <param name="USL">规格上限</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="xAvg">样本平均值</param>
        /// <param name="sigma">Sigma</param>
        /// <param name="tagertValue">目标值</param>
        /// <returns>Ppm</returns>
        public static double CalculatePpm(double USL, double LSL, double xAvg, double sigma, double tagertValue)
        {
            double Ppm = 0;
            double denominator = Math.Sqrt(sigma * sigma + Math.Pow((xAvg - tagertValue), 2));
            Ppm = (USL - LSL) / (6 * denominator);
            return Math.Round(Ppm, 4);
        }
        #endregion

        #region 不良品率控制图（P图）
        /// <summary>
        /// 计算不良品率控制图（P图）
        /// </summary>
        /// <param name="dataTable">样本数据</param>
        /// <param name="UCLList">每组样本UCL值</param>
        /// <param name="LCLList">每组样本LCL值</param>
        /// <param name="pList">每组样本不良品率</param>
        /// <returns>平均不合格品率</returns>
        public static double CalculatePChart(DataTable dataTable, out List<double> UCLList, out List<double> LCLList, out List<double> pList)
        {
            UCLList = new List<double>();
            LCLList = new List<double>();
            pList = new List<double>();
            List<double> TotalQtyList = new List<double>();
            List<double> scrapQtyList = new List<double>();
            double avgP = 0;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                double QtyAll = Convert.ToDouble(dataTable.Rows[i]["QTY_TOTAL"].ToString());
                double QtyScrap = Convert.ToDouble(dataTable.Rows[i]["QTY_SCRAP"].ToString());
                pList.Add(QtyScrap / QtyAll);//每组样本不良品率
                TotalQtyList.Add(QtyAll);
                scrapQtyList.Add(QtyScrap);
            }
            avgP = scrapQtyList.Sum() / TotalQtyList.Sum(); //平均不合格品率

            //计算每组样本控制线
            for (int i = 0; i < TotalQtyList.Count; i++)
            {
                double tempValue = 3 * (Math.Sqrt(avgP * (1 - avgP) / TotalQtyList[i]));
                double UCL = avgP + tempValue;
                double LCL = (avgP - tempValue) > 0 ? avgP - tempValue : 0;
                UCLList.Add(UCL);
                LCLList.Add(LCL);
            }
            return avgP;
        }
        #endregion

        #region 单位缺陷数控制图（U图）
        /// <summary>
        /// 单位缺陷数控制图（U图）
        /// </summary>
        /// <param name="dataTable">原始数据</param>
        /// <param name="UCLList">每组样本UCL值</param>
        /// <param name="LCLList">每组样本LCL值</param>
        /// <param name="UList">单位缺陷数</param>
        /// <returns>平均单位缺陷数</returns>
        public static double CalculateUChart(DataTable dataTable, out List<double> UCLList, out List<double> LCLList, out List<double> UList)
        {
            double avgU = 0;
            UCLList = new List<double>();
            LCLList = new List<double>();
            UList = new List<double>();
            List<double> TotalQtyList = new List<double>();
            double sumQty = 0, sumDefect = 0;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                double QtyAll = Convert.ToDouble(dataTable.Rows[i]["QTY_TOTAL"].ToString());
                double QtyDefect = Convert.ToDouble(dataTable.Rows[i]["QTY_DEFECT"].ToString());
                sumQty += QtyAll;
                sumDefect += QtyDefect;
                UList.Add(QtyDefect / QtyAll); //单位缺陷数
                TotalQtyList.Add(QtyAll);
            }
            avgU = sumDefect / sumQty;//平均单位缺陷数
            //计算控制限
            for (int i = 0; i < TotalQtyList.Count; i++)
            {
                double tempValue = 3 * (Math.Sqrt(avgU / TotalQtyList[i]));
                double UCL = avgU + tempValue;
                double LCL = (avgU - tempValue) > 0 ? avgU - tempValue : 0;
                UCLList.Add(UCL);
                LCLList.Add(LCL);
            }
            return avgU;
        }
        #endregion

        #region 不良品数控制图NP图
        /// <summary>
        /// 不良品数控制图NP图
        /// </summary>
        /// <param name="n">样本容量</param>
        /// <param name="scrapList">每组样本中不良品数量</param>
        /// <returns>控制线</returns>
        public static IDictionary CalculateNpChart(int n, List<int> scrapList)
        {
            double avgP = 0;
            double CL = 0, UCL = 0, LCL = 0;
            Dictionary<string, double> ControlLineDic = new Dictionary<string, double>();
            if (scrapList.Count > 0)
            {
                avgP = Convert.ToDouble(scrapList.Sum()) / (n * scrapList.Count);
            }
            CL = avgP * n;
            UCL = avgP * n + 3 * Math.Sqrt(avgP * n * (1 - avgP));
            LCL = avgP * n - 3 * Math.Sqrt(avgP * n * (1 - avgP));
            ControlLineDic.Add("CL", Math.Round(CL, 2));
            ControlLineDic.Add("UCL", Math.Round(UCL, 2));
            ControlLineDic.Add("LCL", Math.Round(LCL, 2));
            return ControlLineDic;
        }
        #endregion

        #region 缺陷数控制图（C图）
        /// <summary>
        /// 缺陷数控制图（C图）
        /// </summary>
        /// <param name="defectList">每个样本缺陷数</param>
        /// <returns>控制线</returns>
        public static IDictionary CalculateCChart(List<int> defectList)
        {
            double avgC = 0;
            double CL = 0, UCL = 0, LCL = 0;
            Dictionary<string, double> ControlLineDic = new Dictionary<string, double>();
            if (defectList.Count > 0)
            {
                avgC = Convert.ToDouble(defectList.Sum()) / defectList.Count;
            }
            CL = avgC;
            UCL = avgC + 3 * Math.Sqrt(avgC);
            LCL = avgC - 3 * Math.Sqrt(avgC);
            ControlLineDic.Add("CL", Math.Round(CL, 2));
            ControlLineDic.Add("UCL", Math.Round(UCL, 2));
            ControlLineDic.Add("LCL", Math.Round(LCL, 2));
            return ControlLineDic;
        }
        #endregion

        #region 测试数据
        public static DataTable CreateTable()
        {
            List<string[]> ls = new List<string[]>();
            StreamReader fileReader = new StreamReader("D:\\book2.csv");
            string strLine = string.Empty;
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Replace("\"", "").Trim().Split(','));
                    //  ls.Add(strLine.Split(','));
                }
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PARAM_VALUE");
            dataTable.Columns.Add("COL_KEY");
            for (int i = 0; i < ls.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    dataTable.Rows.Add(ls[i][j].ToString(), System.Guid.NewGuid());
                }
            }
            return dataTable;
        }

        public static DataTable CreatePTable()
        {
            List<string[]> ls = new List<string[]>();
            StreamReader fileReader = new StreamReader("D:\\Pbook.csv");
            string strLine = string.Empty;
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Split(','));
                }
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("QTY_TOTAL");
            dataTable.Columns.Add("QTY_SCRAP");
            for (int i = 0; i < ls.Count; i++)
            {

                dataTable.Rows.Add(ls[i][0].ToString(), ls[i][1].ToString());

            }
            return dataTable;
        }

        public static DataTable CreateUTable()
        {
            List<string[]> ls = new List<string[]>();
            StreamReader fileReader = new StreamReader("D:\\Ubook.csv");
            string strLine = string.Empty;
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Split(','));
                }
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("QTY_TOTAL");
            dataTable.Columns.Add("QTY_DEFECT");
            for (int i = 0; i < ls.Count; i++)
            {

                dataTable.Rows.Add(ls[i][0].ToString(), ls[i][1].ToString());

            }
            return dataTable;
        }

        public static List<int> CreateCTable()
        {
            //样本容量为100
            List<int> listValue = new List<int>();
            listValue.Add(5);
            listValue.Add(8);
            listValue.Add(4);
            listValue.Add(9);
            listValue.Add(12);
            listValue.Add(7);
            listValue.Add(8);
            listValue.Add(12);
            listValue.Add(21);
            listValue.Add(7);
            listValue.Add(12);
            listValue.Add(6);
            listValue.Add(9);
            listValue.Add(7);
            listValue.Add(4);
            listValue.Add(9);
            listValue.Add(11);
            listValue.Add(10);
            listValue.Add(6);
            listValue.Add(9);
            listValue.Add(22);
            listValue.Add(13);
            listValue.Add(8);
            listValue.Add(10);
            listValue.Add(7);
            return listValue;
        }
        #endregion        
    }

}
