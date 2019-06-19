/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Gui.Core;
using System.Collections;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using System.Drawing.Imaging;

namespace FanHai.Hemera.Addins.SPC
{
    public partial class Xbar_RChart : UserControl
    {
        private DataRow drStandard = null;
        private DataTable dtStandard = null, dtPoints = null;
        private SpcEntity _spcEntity = null;
        private DataTable valueTable_Control_Plan = null; //Q.003
        string _conCode = string.Empty;
        private DataTable leftValueTable = new DataTable();
        private Dictionary<string, string> dicArr = new Dictionary<string, string>();

        double xCL = 0; //Xbar图中心线，
        double AxCL = 0;//自动计算的中心线
        double xUCL = 0;//Xbar图上线，+3δ
        double xLCL = 0; //Xbar图下线，-3δ
        double rCL = 0,rUCL = 0,rLCL = 0; //R图控制线      
        double USL = 0, TA = 0, LSL = 0;   //规格控制线
        double p_sigma = 0, r_sigma = 0;
        double min_v = 0, max_v = 0, min_r = 0, max_r = 0;
        double pOneSigma = 0, nOneSigma = 0;//+1δ，-1δ区间
        double pTwoSigma = 0, nTwoSigma = 0;//+2δ,-2δ区间
        int _interval = 2;
        //blFlag is false 表示手动计算UCL,LCL，blFlag is true表示自动计算UCL,LCL
        bool blFlag = false;
        int editFlag = 1;
        string c_title = string.Empty;
        List<double> rAvgList = new List<double>();
        double xTotalVag = 0, rTotalAvg = 0, d2 = 0, d3 = 0;
        //坐标的间隔
        
        /// <summary>
        /// 数据点显示纵坐标值
        /// </summary>
        List<string> argDates = new List<string>();
        List<string> argSuppler = new List<string>();
        List<string> argLotNumber = new List<string>();
        bool isDate = false, isSuppler = false,isLotNumber = false;

        ChartControl chart01;
        ChartControl chart02;
        XYDiagram diagram01;
        XYDiagram diagram02;

        SecondaryAxisY xY;
        SecondaryAxisY rY;
        SecondaryAxisX xX;
        SecondaryAxisX rX;

        //DataRow[] drDeletes;
        //List<SpcPoints> xAvgList;        
        Dictionary<string, string> _lst = new Dictionary<string, string>();
        public Xbar_RChart()
        {
            InitializeComponent();
        }

        public Xbar_RChart(DataSet dataDataSet, SpcEntity spcEntity, Dictionary<string, string> lst, string title, int interval,string conCode)
        {
            dtPoints = dataDataSet.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS];
            dtStandard = dataDataSet.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_STANDARD];
            drStandard = dtStandard.Rows[0];
            p_sigma = Convert.ToDouble(dataDataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.P_SIGMA].ToString());
            valueTable_Control_Plan = dataDataSet.Tables["SPC_CONTROL_PLAN"]; //Q.003
            InitializeComponent();
            _conCode = conCode;
            _lst = lst;
            this.c_title = title;
            _spcEntity = spcEntity;
            if (interval > 0)
                _interval = interval;
            InitialChart();
        }


        public void SetAxiexInterval(int n)
        {
            _interval = n;
            if (!isDate && !isSuppler && !isLotNumber)
            {
                isDate = true;
                isSuppler = false;
                isLotNumber = false;
            }

            InitialChart();
        }

        //生成图形
        private void InitialChart()
        {
            this.chart01 = this.chartControl1;
            this.diagram01 = (XYDiagram)this.chart01.Diagram;
            this.xY = this.diagram01.SecondaryAxesY.GetAxisByName("xY");
            this.xX=this.diagram01.SecondaryAxesX.GetAxisByName("xX");
            chart01.Titles[0].TextColor = Color.Blue;
            chart01.Titles[0].Text = c_title + "[XBAR]";

            this.chart02 = this.chartControl2;
            this.diagram02 = (XYDiagram)this.chart02.Diagram;
            this.rY = this.diagram02.SecondaryAxesY.GetAxisByName("rY");
           this.rX = this.diagram02.SecondaryAxesX.GetAxisByName("rX");
            chart02.Titles[0].TextColor = Color.Blue;
            chart02.Titles[0].Text = c_title + "[R]";

            try
            {
                if (!isDate && !isSuppler && !isLotNumber)
                {
                    //取平均值
                    //xTotalVag = Arithmetic.CalculateTotalXAverage(valueTable, valueTable_Group, out xAvgList);
                    xTotalVag = Convert.ToDouble(drStandard[SPC_PARAM_DATA_FIELDS.V_AVG]);
                    //取标准差值
                    //根据valueTable总的数据，与分组valueTable_Group数据平分数据的组数，因为子组数量不固定。
                    //rTotalAvg = Arithmetic.CalculateTotalRAvg(valueTable, valueTable_Group, out rAvgList);
                    rTotalAvg = Convert.ToDouble(drStandard[SPC_PARAM_DATA_FIELDS.R_AVG]);
                    int p_dtl_points = Convert.ToInt32(drStandard[SPC_PARAM_DATA_FIELDS.P_DTL_COUNT]);
                    int p_a_points = Convert.ToInt32(drStandard[SPC_PARAM_DATA_FIELDS.P_A_COUNT]);

                    int n = (int)Math.Ceiling(Convert.ToDouble(p_dtl_points / p_a_points));
                    //if (n > 10 || n < 2) return;
                    if (n < 2) return;
                    if (n > 10)
                        n = 10;

                    d2 = new ConstantSPC("D2").GetConstantItem(n).Value;
                    d3 = new ConstantSPC("D3").GetConstantItem(n).Value;

                    //取控制线值
                    IDictionary controlLineDic = Arithmetic.CalculateXbar_RControlLine(xTotalVag, rTotalAvg, d2, d3, n);
                    //IDictionary controlLineDic = Arithmetic.CalculateXbar_RControlLine(xTotalVag, rTotalAvg, n);
                    if (!blFlag)//手动计算
                    {
                        xCL = Convert.ToDouble(_spcEntity.SL);
                        xUCL = Convert.ToDouble(_spcEntity.UCL);
                        xLCL = Convert.ToDouble(_spcEntity.LCL);
                        AxCL = Convert.ToDouble(controlLineDic["xCL"]);
                    }
                    else//自动计算
                    {
                        AxCL = Convert.ToDouble(controlLineDic["xCL"]);
                        xCL = Convert.ToDouble(controlLineDic["xCL"]);
                        xUCL = Convert.ToDouble(controlLineDic["xUCL"]);
                        xLCL = Convert.ToDouble(controlLineDic["xLCL"]);
                    }
                    //
                    USL = Convert.ToDouble(_spcEntity.USL);
                    LSL = Convert.ToDouble(_spcEntity.LSL);

                    rCL = Convert.ToDouble(controlLineDic["rCL"]);
                    rUCL = Convert.ToDouble(controlLineDic["rUCL"]);
                    rLCL = Convert.ToDouble(controlLineDic["rLCL"]);
                    double eValue = Convert.ToDouble(controlLineDic["eValue"]);
                    pOneSigma = Math.Round(xTotalVag + eValue, 4);
                    nOneSigma = Math.Round(xTotalVag - eValue, 4);
                    pTwoSigma = Math.Round(xTotalVag + 2 * eValue, 4);
                    nTwoSigma = Math.Round(xTotalVag - 2 * eValue, 4);

                }
                //Series赋值
                List<double> xValueList = new List<double>();
                this.chartControl1.Series["xSeries"].Points.Clear();
                this.chartControl2.Series["rSeries"].Points.Clear();
                this.xX.CustomLabels.Clear();
                this.rX.CustomLabels.Clear();
                int i = 0;
                SeriesPoint[] seriesPoint_xbar = new SeriesPoint[dtPoints.Rows.Count];
                SeriesPoint[] seriesPoint_r = new SeriesPoint[dtPoints.Rows.Count];
                CustomAxisLabel[] lbls_xbar = new CustomAxisLabel[dtPoints.Rows.Count];
                CustomAxisLabel[] lbls_r = new CustomAxisLabel[dtPoints.Rows.Count];

                foreach (DataRow dr in dtPoints.Rows)
                {
                    string sd = string.Empty;

                    if (isSuppler)
                        sd = dr[SPC_PARAM_DATA_FIELDS.SUPPLIER].ToString();
                    else if (isLotNumber)
                        sd = dr[SPC_PARAM_DATA_FIELDS.MATERIAL_LOT].ToString();
                    else
                        sd = dr[SPC_PARAM_DATA_FIELDS.CREATE_TIME].ToString();
                    //---------------------------------------------------------------------------------------------------
                    string v = dr[SPC_PARAM_DATA_FIELDS.V_VALUE].ToString();
                    SpcPoints spcPoint = new SpcPoints();
                    spcPoint.editFlag = Convert.ToInt32(dr[SPC_PARAM_DATA_FIELDS.EDIT_FLAG].ToString());
                    spcPoint.deleteFlag = Convert.ToInt32(dr[SPC_PARAM_DATA_FIELDS.DELETED_FLAG].ToString());
                    spcPoint.pointkeys = dr[SPC_PARAM_DATA_FIELDS.POINT_KEY].ToString();
                    SeriesPoint seriesPoint = new SeriesPoint(i.ToString(), (object)(v));
                    spcPoint.value = Convert.ToDouble(v);
                    seriesPoint.Tag = spcPoint;
                    seriesPoint_xbar[i] = seriesPoint;

                    CustomAxisLabel xlbl = new CustomAxisLabel();
                    xlbl.Name = sd;
                    if (i % _interval > 0 && _interval != 1)
                        xlbl.AxisValueSerializable = "";
                    else
                        xlbl.AxisValueSerializable = i.ToString();
                    lbls_xbar[i] = xlbl;
                    //---------------------------------------------------------------------------------------------------
                    string r = dr[SPC_PARAM_DATA_FIELDS.R_VALUE].ToString();
                    SpcPoints spcPointr = new SpcPoints();
                    spcPointr.value = Convert.ToDouble(r);

                    SeriesPoint seriesPointr = new SeriesPoint(i.ToString(), new object[] { ((object)(r)) });
                    seriesPointr.Tag = spcPointr;
                    seriesPoint_r[i] = seriesPointr;

                    CustomAxisLabel rlbl = new CustomAxisLabel();
                    rlbl.Name = sd;
                    if (i % _interval > 0 && _interval != 1)
                        rlbl.AxisValueSerializable = "";
                    else
                        rlbl.AxisValueSerializable = i.ToString();
                    lbls_r[i] = rlbl;
                    i++;
                }
                this.chartControl1.Series["xSeries"].Points.AddRange(seriesPoint_xbar);
                this.chartControl2.Series["rSeries"].Points.AddRange(seriesPoint_r);
                this.xX.CustomLabels.AddRange(lbls_xbar);
                this.rX.CustomLabels.AddRange(lbls_r);

                xY.ConstantLines.Clear();
                rY.ConstantLines.Clear();
                min_v = Convert.ToDouble(drStandard[SPC_PARAM_DATA_FIELDS.MIN_V_VALUE].ToString());
                max_v = Convert.ToDouble(drStandard[SPC_PARAM_DATA_FIELDS.MAX_V_VALUE].ToString());
                min_r = Convert.ToDouble(drStandard[SPC_PARAM_DATA_FIELDS.MIN_R_VALUE].ToString());
                max_r = Convert.ToDouble(drStandard[SPC_PARAM_DATA_FIELDS.MAX_R_VALUE].ToString());

                try
                {
                    //修改XBAR纵坐标
                    //double maxY1Value = xValueList.Max() > xUCL ? xValueList.Max() : xUCL;
                    //double minY1Value = xValueList.Min() < xLCL ? xValueList.Min() : xLCL;
                    double maxY1Value = max_v > USL ? max_v : USL;
                    double minY1Value = min_v < LSL ? min_v : LSL;
                    double interVal1 = Math.Round((maxY1Value - minY1Value) / 10, 3);
                    //xY坐标的最大最小值       
                    xY.Range.MaxValue = maxY1Value + interVal1;
                    xY.Range.MinValue = minY1Value - interVal1;

                    //修改XBAR-R纵坐标
                    double maxY2Value = max_r > rUCL ? max_r : rUCL;
                    double minY2Value = min_r < rLCL ? min_r : rLCL;
                    double interVal2 = Math.Round((maxY2Value - minY2Value) / 10, 3);
                    rY.Range.MaxValue = maxY2Value + interVal2;
                    rY.Range.MinValue = minY2Value - interVal2;
                }
                catch { }

                #region add ConstantLine

                ConstantLine xUCLLine = new ConstantLine();
                ConstantLine xCLLine = new ConstantLine();
                ConstantLine xLCLLine = new ConstantLine();
                ConstantLine rCLLine = new ConstantLine();
                ConstantLine rUCLLine = new ConstantLine();
                ConstantLine rLCLine = new ConstantLine();
                ConstantLine USLLine = new ConstantLine();
                ConstantLine TALine = new ConstantLine();
                ConstantLine LSLLine = new ConstantLine();



                xLCLLine.Color = Color.Blue;
                xLCLLine.AxisValue = xLCL;
                xLCLLine.LegendText = "LCL" + "(" + xLCL.ToString() + ")";
                xLCLLine.Name = "LCL";
                xLCLLine.Title.Alignment = DevExpress.XtraCharts.ConstantLineTitleAlignment.Far;

                xCLLine.Color = Color.Blue;
                xCLLine.AxisValue = xCL;
                xCLLine.LegendText = "CL" + "(" + xCL.ToString() + ")";
                xCLLine.Name = "CL";
                xCLLine.Title.Alignment = DevExpress.XtraCharts.ConstantLineTitleAlignment.Far;

                xUCLLine.Color = Color.Blue;
                xUCLLine.AxisValue = xUCL;
                xUCLLine.LegendText = "UCL" + "(" + xUCL.ToString() + ")";
                xUCLLine.Name = "UCL";
                xUCLLine.Title.Alignment = DevExpress.XtraCharts.ConstantLineTitleAlignment.Far;

                USLLine.Color = Color.Red;
                USLLine.AxisValue = _spcEntity.USL;
                USLLine.LegendText = "USL" + "(" + _spcEntity.USL + ")";
                USLLine.Name = "USL";
                USLLine.Title.Alignment = ConstantLineTitleAlignment.Far;

                TALine.Color = Color.Blue;
                TALine.AxisValue = _spcEntity.SL;
                TALine.LegendText = "TA" + "(" + _spcEntity.SL + ")";
                TALine.Name = "Ta";
                TALine.Title.Alignment = ConstantLineTitleAlignment.Far;

                LSLLine.Color = Color.Red;
                LSLLine.AxisValue = _spcEntity.LSL;
                LSLLine.LegendText = "LSL" + "(" + _spcEntity.LSL + ")";
                LSLLine.Name = "LSL";
                LSLLine.Title.Alignment = ConstantLineTitleAlignment.Far;

                xY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
                                                                                        USLLine,
                                                                                        xUCLLine,
                                                                                        xCLLine,                                                                                
                                                                                        xLCLLine,
                                                                                        LSLLine
                                                                                        });

                rLCLine.Color = Color.Blue;
                rLCLine.LegendText = "LCL" + "(" + rLCL.ToString() + ")";
                rLCLine.Name = "LCL";
                rLCLine.AxisValue = rLCL;
                rLCLine.Title.Alignment = DevExpress.XtraCharts.ConstantLineTitleAlignment.Far;
                //rCLLine.AxisValueSerializable = "3";
                rCLLine.Color = Color.Blue;
                rCLLine.LegendText = "CL" + "(" + rCL.ToString() + ")";
                rCLLine.Name = "CL";
                rCLLine.AxisValue = rCL;
                rCLLine.Title.Alignment = DevExpress.XtraCharts.ConstantLineTitleAlignment.Far;

                rUCLLine.Color = Color.Blue;
                rUCLLine.LegendText = "UCL" + "(" + rUCL.ToString() + ")";
                rUCLLine.Name = "UCL";
                rUCLLine.AxisValue = rUCL;
                rUCLLine.Title.Alignment = DevExpress.XtraCharts.ConstantLineTitleAlignment.Far;

                rY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
                                                                                        rUCLLine,
                                                                                        rCLLine,
                                                                                        rLCLine});
                #endregion

                #region 计算过程能力参数
                //计算sigma ， sigma=rTotalAvg/d2;
                //double eSigma = Convert.ToDouble(rTotalAvg)/ 2.326;  cpk是预测的值
                r_sigma = Convert.ToDouble(rTotalAvg) / d2;

                double Cp = 0, Cpk = 0, Cpm = 0, K = 0;
                double Pp = 0, Ppk = 0, Ppm = 0;
                //double Sigma = Arithmetic.CalculateSigma(Convert.ToDecimal(xTotalVag), valueTable);
                if (_spcEntity.LSL != string.Empty && _spcEntity.SL != string.Empty && _spcEntity.USL != string.Empty)
                {
                    USL = double.Parse(_spcEntity.USL);
                    TA = double.Parse(_spcEntity.SL);
                    LSL = double.Parse(_spcEntity.LSL);

                    Cp = Arithmetic.CalculateCp(USL, LSL, r_sigma);
                    Cpk = Arithmetic.CalculateCpk(USL, LSL, r_sigma, xTotalVag);
                    Cpm = Arithmetic.CalculateCpm(USL, LSL, r_sigma, xTotalVag, TA);
                    K = Arithmetic.CalculateK(USL, LSL, xTotalVag);

                    Pp = Arithmetic.CalculatePp(USL, LSL, p_sigma);
                    Ppk = Arithmetic.CalculatePpk(USL, LSL, xTotalVag, p_sigma);
                    Ppm = Arithmetic.CalculatePpm(USL, LSL, xTotalVag, p_sigma, TA);
                    dicArr.Clear();
                    //add new
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_COUNT, dtPoints.Rows.Count.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_USL, USL.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_UCLX, xUCL.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_TA, TA.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_CLX, AxCL.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_LCLX, xLCL.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_LSL, LSL.ToString());

                    dicArr.Add(SPC_COUNT_FIELD.FIELD_XBARSTD, p_sigma.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_CP, Cp.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_CPK, Cpk.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_CPM, Cpm.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_PP, Pp.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_PPK, Ppk.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_PPM, Ppm.ToString());

                    dicArr.Add(SPC_COUNT_FIELD.FIELD_UCLR, rUCL.ToString());
                    dicArr.Add(SPC_COUNT_FIELD.FIELD_LCLR, rLCL.ToString());

                    leftValueTable = _spcEntity.AddRowToTable(dicArr);
                    this.gcCount.MainView = gvCount;
                    this.gcCount.DataSource = null;
                    this.gcCount.DataSource = leftValueTable;
                    //end
                }
                #endregion
                Rules(_lst);
            }
            catch (Exception ex)
            {
                MessageService.ShowError("绘制Xbar-R 图出错，错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 获得异常规则点数的信息
        /// </summary>
        /// <param name="lst">多个异常规则的主键值集合</param>
        /// Owner by Genchille.yang 2012-03-12 15:20:13
        public void Rules(Dictionary<string,string> lst)
        {
            if (lst.Count < 1) return;
            string abnormalIds = string.Empty;
            foreach (string s in lst.Values)
                abnormalIds += "'" + s + "',";
            abnormalIds = abnormalIds.Trim().TrimEnd(',');
            DataSet dsReq = _spcEntity.GetAbnormalDetailRule(abnormalIds);
            DataTable dtlst = dsReq.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];

            Series xSeries = this.chart01.Series["xSeries"];
            List<int> pointList = null;

            foreach (string s in lst.Values)
            {
                string normcode=string.Empty;
                foreach(KeyValuePair<string,string> kv in lst)
                {
                if(kv.Value.Equals(s))
                    normcode=kv.Key;
                }
                DataRow[] dr01 = dtlst.Select(string.Format("ABNORMALID='{0}'", s));
                //表示比较符
                string p_sign = string.Empty, n_sign = string.Empty, sign01 = string.Empty, sign02 = string.Empty, signtemp = string.Empty;
                //表示监控点数
                int watch = 0, p_watch = 0, n_watch = 0;
                //表示超规则点数
                int overs = 0, p_overs = 0, n_overs = 0;
                //表示规则值
                int value = 0, p_value = 0, n_value = 0;
                //表示比较规格
                string rule = string.Empty, p_rule = string.Empty, n_rule = string.Empty;
                //1,两笔明细规则，p:表示cl线以上，n:表示cl线以下；
                int p = 0, m = 0;
                //double standard = 0;
                double p_standard = 0, n_standard = 0;
                //两笔异常明细
                #region 两笔异常明细
                if (dr01.Length == 2)
                {
                    sign01 = dr01[0][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN].ToString();
                    sign02 = dr01[1][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN].ToString();
                    #region 大于，小于判断；大于等于，小于等于判断
                    if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHAN) || sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_LESSTHAN)
                        || sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHANOREQUAL) || sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_LESSTHANOREQUAL))
                    {
                        bool isGreateThanOrEqual = false;
                        if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHAN) && sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_LESSTHAN))
                        {
                            isGreateThanOrEqual = false;
                            p = 0;
                        }
                        else if (sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHAN) && sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHAN))
                        {
                            isGreateThanOrEqual = false;
                            p = 1;
                        }
                        else if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHANOREQUAL) && sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_LESSTHANOREQUAL))
                        {
                            isGreateThanOrEqual = true;
                            p = 0;
                        }
                        else if (sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_LESSTHANOREQUAL) && sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_GREATETHANOREQUAL))
                        {
                            isGreateThanOrEqual = true;
                            p = 1;
                        }
                        else
                        {
                            MessageService.ShowError(string.Format("规则:{0},子规则不对称，请查看!", dr01[0][EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString()));
                            break;
                        }
                        m = p > 0 ? 0 : 1;
                        p_watch = Convert.ToInt32(dr01[p][EDC_ABNORMAL_DTL_FIELDS.FIELD_WATCHPOINTS].ToString());
                        p_overs = Convert.ToInt32(dr01[p][EDC_ABNORMAL_DTL_FIELDS.FIELD_OVERRULEPOINTS].ToString());
                        p_sign = dr01[p][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN].ToString();
                        p_value = Convert.ToInt32(dr01[p][EDC_ABNORMAL_DTL_FIELDS.FIELD_RULEVALUE].ToString());
                        p_rule = dr01[p][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARERULE].ToString();

                        n_watch = Convert.ToInt32(dr01[m][EDC_ABNORMAL_DTL_FIELDS.FIELD_WATCHPOINTS].ToString());
                        n_overs = Convert.ToInt32(dr01[m][EDC_ABNORMAL_DTL_FIELDS.FIELD_OVERRULEPOINTS].ToString());
                        n_sign = dr01[m][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN].ToString();
                        n_value = Convert.ToInt32(dr01[m][EDC_ABNORMAL_DTL_FIELDS.FIELD_RULEVALUE].ToString());
                        n_rule = dr01[m][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARERULE].ToString();

                        #region 规格线,控制线,均值线,Sigma—判断比较规格
                        if ((p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_USL) && n_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_LSL))
                            || (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_UCL) && n_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_LCL))
                            || (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_CL) && n_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_CL))
                            || (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_SIGMA) && n_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_SIGMA)))
                        {

                            if (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_USL))
                            {
                                p_standard = USL;
                                n_standard = LSL;
                            }
                            else if (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_UCL))
                            {
                                p_standard = xUCL;
                                n_standard = xLCL;
                            }
                            else if (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_CL))
                            {
                                p_standard = xCL;
                                n_standard = xCL;
                            }
                            else if (p_rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_SIGMA))
                            {
                                p_standard = pOneSigma;
                                n_standard = nOneSigma;
                            }

                            pointList = new List<int>();
                            for (int i = 0; i < xSeries.Points.Count; i++)
                            {
                                //监控点数，这里取得是上线的监控点数，p_watch=n_watch
                                if (i + p_watch < xSeries.Points.Count + 1)
                                {
                                    int count01 = 0, count02 = 0;
                                    for (int n = 0; n < p_watch; n++)
                                    {
                                        //点数值与规格值比较
                                        if (isGreateThanOrEqual)
                                        {
                                            if (xSeries.Points[i + n].Values[0] >= p_value * p_standard)
                                                count01++;
                                            if (xSeries.Points[i + n].Values[0] <= n_value * n_standard)
                                                count02++;
                                        }
                                        else
                                        {
                                            if (xSeries.Points[i + n].Values[0] > p_value * p_standard)
                                                count01++;
                                            if (xSeries.Points[i + n].Values[0] < n_value * n_standard)
                                                count02++;
                                        }
                                    }
                                    //实际异常点数与超规格点数进行比较
                                    if (isGreateThanOrEqual)
                                    {
                                        if (count01 >= p_overs)
                                        {
                                            for (int k = 0; k < p_watch; k++)
                                            {
                                                SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                                if (xSeries.Points[i + k].Values[0] >= p_value * p_standard)
                                                {
                                                    if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                                    {
                                                        pointList.Add(i + k);
                                                    }
                                                }
                                            }
                                            i++; //跳过以中间点为起点的检测，提高检测效率
                                        }

                                        if (count02 >= p_overs)
                                        {
                                            for (int k = 0; k < p_watch; k++)
                                            {
                                                SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                                if (xSeries.Points[i + k].Values[0] <= n_value * n_standard)
                                                {
                                                    if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                                    {
                                                        pointList.Add(i + k);
                                                    }
                                                }
                                            }
                                            i++; //跳过以中间点为起点的检测，提高检测效率
                                        }
                                    }
                                    else
                                    {
                                        if (count01 >= p_overs)
                                        {
                                            for (int k = 0; k < p_watch; k++)
                                            {
                                                SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                                if (xSeries.Points[i + k].Values[0] > p_value * p_standard)
                                                {
                                                    if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                                    {
                                                        pointList.Add(i + k);
                                                    }
                                                }
                                            }
                                            i++; //跳过以中间点为起点的检测，提高检测效率
                                        }

                                        if (count02 >= p_overs)
                                        {
                                            for (int k = 0; k < p_watch; k++)
                                            {
                                                SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                                if (xSeries.Points[i + k].Values[0] < n_value * n_standard)
                                                {
                                                    if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                                    {
                                                        pointList.Add(i + k);
                                                    }
                                                }
                                            }
                                            i++; //跳过以中间点为起点的检测，提高检测效率
                                        }
                                    }
                                }
                            }

                            ChangePointColor(pointList, normcode);
                        }
                        #endregion
                    }
                    #endregion
                    #region 直增，直减—判断比较规格
                    else if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_STRICTLYINCREASING) || sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_STRICTLYDECREASING))
                    {
                        //p表示直增，n表示直减
                        if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_STRICTLYINCREASING) && sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_STRICTLYDECREASING))
                        {
                            p = 0;
                            m = 1;
                        }
                        else if (sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_STRICTLYINCREASING) && sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_STRICTLYDECREASING))
                        {
                            p = 1;
                            m = 0;
                        }

                        List<int> p_list = new List<int>(), n_list = new List<int>();
                        for (int i = 0; i < xSeries.Points.Count; i++)
                        {
                            //监控点数，这里取得是上线的监控点数
                            if (i + p_watch < xSeries.Points.Count + 1)
                            {
                                int pcount = 0, ncount = 0;
                                for (int n = 0; n < p_watch; n++)
                                {
                                    //点数值与规格值比较
                                    if (xSeries.Points[i + n].Values[0] > xSeries.Points[i + n + 1].Values[0])
                                    {
                                        pcount = 0;
                                        ncount++;
                                    }
                                    if (xSeries.Points[i + n].Values[0] < xSeries.Points[i + n + 1].Values[0])
                                    {
                                        pcount++;
                                        ncount = 0;
                                    }
                                }

                                //实际异常点数与超规格点数进行比较
                                if (pcount >= p_overs || ncount > p_overs)
                                {
                                    for (int k = 0; k < p_watch; k++)
                                    {
                                        //SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                        //直减
                                        if (xSeries.Points[i + k].Values[0] > xSeries.Points[i + k + 1].Values[0])
                                        {
                                            p_list.Clear();
                                            if (!n_list.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                n_list.Add(i + k);
                                            }
                                        }
                                        //直增
                                        if (xSeries.Points[i + k].Values[0] < xSeries.Points[i + k + 1].Values[0])
                                        {
                                            n_list.Clear();
                                            if (!p_list.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                p_list.Add(i + k);
                                            }
                                        }
                                    }
                                    i++; //跳过以中间点为起点的检测，提高检测效率
                                }
                            }
                        }
                        //直增
                        if (p_list.Count >= p_overs)
                            ChangePointColor(p_list, normcode);
                        //直减
                        if (n_list.Count > p_overs)
                            ChangePointColor(n_list, normcode);
                    }
                    #endregion
                    #region 递增，递减—判断比较规格
                    else if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_INCREASING) || sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_DECREASING))
                    {
                        //p表示递增，n表示递减
                        if (sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_INCREASING) && sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_DECREASING))
                        {
                            p = 0;
                            m = 1;
                        }
                        else if (sign02.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_INCREASING) && sign01.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_DECREASING))
                        {
                            p = 1;
                            m = 0;
                        }

                        List<int> p_list = new List<int>(), n_list = new List<int>();
                        for (int i = 0; i < xSeries.Points.Count; i++)
                        {
                            //监控点数，这里取得是上线的监控点数
                            if (i + p_watch < xSeries.Points.Count + 1)
                            {
                                int pcount = 0, ncount = 0;
                                for (int n = 0; n < p_watch; n++)
                                {
                                    //点数值与规格值比较
                                    if (xSeries.Points[i + n].Values[0] > xSeries.Points[i + n + 1].Values[0])
                                    {
                                        ncount++;
                                    }
                                    if (xSeries.Points[i + n].Values[0] < xSeries.Points[i + n + 1].Values[0])
                                    {
                                        pcount++;
                                    }
                                }

                                //实际异常点数与超规格点数进行比较
                                if (pcount >= p_overs || ncount > p_overs)
                                {
                                    for (int k = 0; k < p_watch; k++)
                                    {
                                        //SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                        //递减
                                        if (xSeries.Points[i + k].Values[0] > xSeries.Points[i + k + 1].Values[0])
                                        {
                                            if (!n_list.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                n_list.Add(i + k);
                                            }
                                        }
                                        //递增
                                        if (xSeries.Points[i + k].Values[0] < xSeries.Points[i + k + 1].Values[0])
                                        {
                                            if (!p_list.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                p_list.Add(i + k);
                                            }
                                        }
                                    }
                                    i++; //跳过以中间点为起点的检测，提高检测效率
                                }
                            }
                        }
                        //递增
                        if (p_list.Count >= p_overs)
                            ChangePointColor(p_list, normcode);
                        //递减
                        if (n_list.Count > p_overs)
                            ChangePointColor(n_list, normcode);
                    }
                    #endregion
                }
                #endregion
                //一笔异常明细
                #region 一笔异常明细
                if (dr01.Length == 1)
                {
                    signtemp = dr01[0][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN].ToString();
                    //检测点数
                    watch = Convert.ToInt32(dr01[0][EDC_ABNORMAL_DTL_FIELDS.FIELD_WATCHPOINTS].ToString());
                    //超规点数
                    overs = Convert.ToInt32(dr01[0][EDC_ABNORMAL_DTL_FIELDS.FIELD_OVERRULEPOINTS].ToString());

                    #region 交互—判断比较规格 11
                    if (signtemp.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_ALTERNATING))
                    {
                        pointList = new List<int>();
                        for (int i = 0; i < xSeries.Points.Count; i++)
                        {
                            //监控点数，这里取得是上线的监控点数
                            if (i + watch < xSeries.Points.Count)
                            {
                                int count = 0;
                                bool isGreate = false;
                                for (int n = 0; n < watch; n++)
                                {
                                    //点数值与规格值比较                                   
                                    if (n == 0)
                                    {
                                        if (xSeries.Points[i + n].Values[0] >= xSeries.Points[i + n + 1].Values[0])
                                        {
                                            isGreate = true;
                                            count++;
                                        }
                                        else
                                        {
                                            isGreate = false;
                                            count++;
                                        }
                                    }
                                    else
                                    {
                                        if (isGreate)
                                        {
                                            if (isGreate && xSeries.Points[i + n].Values[0] < xSeries.Points[i + n + 1].Values[0])
                                            {
                                                count++;
                                                isGreate = false;
                                            }
                                            else
                                            {
                                                count = 0;

                                            }
                                        }
                                        if (!isGreate)
                                        {
                                            if (isGreate && xSeries.Points[i + n].Values[0] >= xSeries.Points[i + n + 1].Values[0])
                                            {
                                                count++;
                                                isGreate = true;
                                            }
                                            else
                                            {
                                                count = 0;

                                            }
                                        }
                                    }
                                }

                                //实际异常点数与超规格点数进行比较
                                if (count >= overs)
                                {
                                    for (int k = 0; k < watch; k++)
                                    {
                                        //点数值与规格值比较                                   

                                        if (k == 0)
                                        {
                                            if (xSeries.Points[i + k].Values[0] >= xSeries.Points[i + k + 1].Values[0])
                                            {
                                                isGreate = true;
                                                if (pointList.Exists(delegate(int key) { return key == i + k; }))
                                                {
                                                    pointList.Add(i + k);
                                                }
                                            }
                                            else
                                            {
                                                isGreate = false;
                                                if (pointList.Exists(delegate(int key) { return key == i + k; }))
                                                {
                                                    pointList.Add(i + k);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (isGreate)
                                            {
                                                if (isGreate && xSeries.Points[i + k].Values[0] < xSeries.Points[i + k + 1].Values[0])
                                                {
                                                    isGreate = false;
                                                    if (pointList.Exists(delegate(int key) { return key == i + k; }))
                                                    {
                                                        pointList.Add(i + k);
                                                    }
                                                }
                                                else
                                                    pointList.Clear();
                                            }
                                            if (!isGreate)
                                            {
                                                if (isGreate && xSeries.Points[i + k].Values[0] >= xSeries.Points[i + k + 1].Values[0])
                                                {
                                                    isGreate = true;
                                                    if (pointList.Exists(delegate(int key) { return key == i + k; }))
                                                    {
                                                        pointList.Add(i + k);
                                                    }
                                                }
                                                else
                                                    pointList.Clear();
                                            }
                                        }
                                    }
                                    i++; //跳过以中间点为起点的检测，提高检测效率
                                }
                            }
                        }
                        ChangePointColor(pointList, normcode);
                    }
                    #endregion
                    #region 区间内—判断比较规格 9
                    else if (signtemp.ToUpper().Equals(SPC_ABNORMAL_COMPARE.SIGN_INSIDE))
                    {
                        rule = dr01[0][EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARERULE].ToString();
                        value = Convert.ToInt32(dr01[0][EDC_ABNORMAL_DTL_FIELDS.FIELD_RULEVALUE].ToString());
                        if (rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_ZONEA))
                        {
                            p_standard = pTwoSigma;
                            n_standard = nTwoSigma;
                        }
                        if (rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_ZONEB))
                        {
                            p_standard = pOneSigma;
                            n_standard = nOneSigma;
                        }
                        if (rule.ToUpper().Equals(SPC_ABNORMAL_COMPARE.STANDARD_ZONEC))
                        {
                            p_standard = xCL;
                            n_standard = xCL;
                        }

                        pointList = new List<int>();
                        for (int i = 0; i < xSeries.Points.Count; i++)
                        {
                            //监控点数，这里取得是区间内的监控点数
                            if (i + watch < xSeries.Points.Count + 1)
                            {
                                int count = 0;
                                for (int n = 0; n < watch; n++)
                                {
                                    //点数值与规格值比较
                                    if (xSeries.Points[i + n].Values[0] > value * p_standard || xSeries.Points[i + n].Values[0] < value * n_standard)
                                    {
                                        count++;
                                    }
                                }

                                //实际异常点数与超规格点数进行比较
                                if (count >= overs)
                                {
                                    for (int k = 0; k < watch; k++)
                                    {
                                        SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                        //区间
                                        if (xSeries.Points[i + k].Values[0] > value * p_standard || xSeries.Points[i + k].Values[0] < value * n_standard)
                                        {
                                            if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                pointList.Add(i + k);
                                            }
                                        }
                                    }
                                    i++; //跳过以中间点为起点的检测，提高检测效率
                                }
                            }
                        }
                        ChangePointColor(pointList, normcode);
                    }
                    #endregion
                }
                #endregion
            }
        }

        ToolTipController toolTipController1 = new ToolTipController();
        private void chartControl1_ObjectHotTracked(object sender, HotTrackEventArgs e)
        { 
            SeriesPoint seriesPoint = e.AdditionalObject as SeriesPoint;
            if (seriesPoint != null)
            {
                string rules=string.Empty;
                SpcPoints spcPoints = (SpcPoints)seriesPoint.Tag;
                if (spcPoints.abnormalRules.Length > 0)
                    rules = spcPoints.abnormalRules;

                double[] values = seriesPoint.Values;  
                
                if (values.Length>0)
                {
                    if (rules.Length > 0)
                        rules = "触发异常规则:" + rules;

                    string s = "Avg Value = " + values[0].ToString();
                    s += rules;

                    toolTipController1.ShowHint(s);               
                }
            }
            else
            {
                toolTipController1.HideHint();
            }
        }

        private void chartControl2_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            chartControl1_CustomDrawSeriesPoint(sender, e);
        }

        private void chartControl1_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            DevExpress.XtraCharts.Series series = e.Series;

            double maxValue = 0;
            double minValue = 0;
            if (series.Name == "xSeries")
            {
                maxValue = xUCL;
                minValue = xLCL;
            }
            else
            {
                maxValue = rUCL;
                minValue = rLCL;
            }

            if (e.SeriesDrawOptions != null)
            {
                double value = e.SeriesPoint.Values[0];
                SpcPoints spcPoint = (SpcPoints)e.SeriesPoint.Tag;
                if (spcPoint != null && spcPoint.deleteFlag == 1)
                {
                    e.SeriesDrawOptions.Color = Color.White;
                    PointDrawOptions pointDrawOption = e.SeriesDrawOptions as PointDrawOptions;

                    pointDrawOption.Marker.Kind = MarkerKind.Cross;
                }

                if (value > maxValue || value < minValue)
                {
                    if (spcPoint != null && spcPoint.editFlag == 1)
                        e.SeriesDrawOptions.Color = Color.Green;//已被修改过的绿色表示
                    else
                        e.SeriesDrawOptions.Color = Color.Red;//将超出管制线的点颜色设置为红色 
                }
                else
                {

                    if (spcPoint != null && spcPoint.validateFlag == 1 && spcPoint.redFlag == 1)
                        e.SeriesDrawOptions.Color = Color.Red;//符合规则的点红色显示    
                    else if (spcPoint != null && spcPoint.editFlag == 0 && spcPoint.redFlag == 0)
                        e.SeriesDrawOptions.Color = Color.Blue;

                    if (spcPoint != null && spcPoint.editFlag == 1)
                        e.SeriesDrawOptions.Color = Color.Green;//已被修改过的绿色表示
                    else if (spcPoint != null && spcPoint.editFlag == 2)
                        e.SeriesDrawOptions.Color = Color.Yellow;//已被修改过的黄色表示                    
                }
            }
        }

        ChartHitInfo hi = null;
        private void chartControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hi = chartControl1.CalcHitInfo(new System.Drawing.Point(e.X, e.Y));
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            if (hi.InSeries && hi.SeriesPoint != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    Series series = (DevExpress.XtraCharts.Series)(hi.Series);
                    if (series.Name == "xSeries")
                    {
                        contextMenu.Items.Add("点批次信息", null, new EventHandler(PointInformation));
                        contextMenu.Items.Add("点详细信息", null, new EventHandler(PointDetailInformation));
                        contextMenu.Items.Add("查看注释信息", null, new EventHandler(PointEditInformation));
                        contextMenu.Items.Add("添加注释信息", null, new EventHandler(AddModifyPoint));
                        contextMenu.Items.Add("剔 除", null, new EventHandler(DeletePoint)); 

                        if (hi.SeriesPoint.Values[0] > xUCL || hi.SeriesPoint.Values[0] < xLCL || ((SpcPoints)hi.SeriesPoint.Tag).validateFlag == 1)
                        {                          
                            contextMenu.Items.Add("修 正", null, new EventHandler(ModifyPoint));
                            //contextMenu.Items.Add("剔 除", null, new EventHandler(DeletePoint));                           
                        }
                        //if(((SpcPoints)hi.SeriesPoint.Tag).editFlag == 1)
                        //{
                        //    contextMenu.Items.Add("注释信息", null, new EventHandler(PointEditInformation));                            
                        //}
                        //set the context menu's show position
                        Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                        p = this.PointToClient(p);
                        //show context menu
                        contextMenu.Show(this, p);
                    }
                }
            }
            else
            {
                hi = null;
                if (e.Button == MouseButtons.Right)
                {

                    contextMenu.Items.Add("导出平均数据", null, new EventHandler(exp_av_data)); //Q.003
                    contextMenu.Items.Add("导出原始数据", null, new EventHandler(exp_sr_data)); //Q.003
                    contextMenu.Items.Add("坐标转换为日期", null, new EventHandler(PieChangeDate));
                    contextMenu.Items.Add("坐标转换为供应商", null, new EventHandler(PieChangeSuppler));
                    contextMenu.Items.Add("坐标转换为批次号", null, new EventHandler(PieChangeLotNumber));//jing.xie
                    contextMenu.Items.Add("导出EXCEL", null, new EventHandler(ExportXsl));
                    contextMenu.Items.Add("导出图片", null, new EventHandler(ExportImg));
                    contextMenu.Items.Add("打印预览", null, new EventHandler(PrintPreview));
                    Point p1 = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                    p1 = this.PointToClient(p1);
                    //show context menu
                    contextMenu.Show(this, p1);
                }
            }
        }


        private void ExportXsl(object sender, EventArgs e)
        {
            ChartControlExportHelper.CommExport(this.chartControl1, ExportType.Excel, ((DataView)this.gvCount.DataSource).Table);
        }


        private void ExportImg(object sender, EventArgs e)
        {
            ChartControlExportHelper.CommExport(this.chartControl1, ExportType.Img, ((DataView)this.gvCount.DataSource).Table);           
        }

        private void PrintPreview(object sender, EventArgs e)
        {
            ChartControlExportHelper.CommExport(this.chartControl1, ExportType.PrintPreview, ((DataView)this.gvCount.DataSource).Table);           
        }


        private void PieChangeLotNumber(object sender, EventArgs e)
        {
            isLotNumber = true;
            isSuppler = false;
            isDate = false;
            InitialChart();
        }
        private void PieChangeDate(object sender, EventArgs e)
        {
            isDate = true;
            isSuppler = false;
            isLotNumber = false;
            InitialChart();
        }
        private void PieChangeSuppler(object sender, EventArgs e)
        {
            isSuppler = true;
            isDate = false;
            isLotNumber = false;
            InitialChart();
        }
        //Q.003
        private void exp_sr_data(object sender, EventArgs e)
        {
            string pointkeys = string.Empty;
            foreach (DataRow dr in dtPoints.Rows)
                pointkeys += string.Format(" A1.POINT_KEY='{0}' OR", dr[SPC_PARAM_DATA_FIELDS.POINT_KEY].ToString());
            if (pointkeys.Length > 0)
                pointkeys = pointkeys.Substring(0, pointkeys.Length - 2);         

            SpcEntity spcEntity2 = new SpcEntity();
            DataSet ds = spcEntity2.GetTableData(_conCode, pointkeys);
            DataTable table = ds.Tables[0].Copy();

            string[] title = { "管控代码", "车间", "站点", "成品类型", "生产批号", "采集机台", "参数", "值", "班别", "班别", "采集时间", "采集人员", "硅片料号", "硅片供应商", "PECVD机台", "退火机台", "扩散机台" };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径
                string strinlocalFilePath = saveFileDialog1.FileName.ToString();

                //获取文件名，不带路径
                string fileNameExt = strinlocalFilePath.Substring(strinlocalFilePath.LastIndexOf("\\") + 1);
                //获取文件路径，不带文件名
                string FilePath = strinlocalFilePath.Substring(0, strinlocalFilePath.LastIndexOf("\\"));
                string newFileName = fileNameExt + DateTime.Now.ToString("yyyyMMdd");
                SaveToExcel.SaveExcel(table, newFileName, FilePath, c_title, title);
            }
            else
            {
                //这里放对取消的处理
            }

        }

        //Q.0031
        private void exp_av_data(object sender, EventArgs e)
        {
            string pointkeys = string.Empty;
            foreach (DataRow dr in dtPoints.Rows)
                pointkeys += string.Format(" A1.POINT_KEY='{0}' OR", dr[SPC_PARAM_DATA_FIELDS.POINT_KEY].ToString());
            if (pointkeys.Length > 0)
                pointkeys = pointkeys.Substring(0, pointkeys.Length - 2);   

            SpcEntity spcEntity2 = new SpcEntity();
            DataSet ds = spcEntity2.GetTableAvData(_conCode, pointkeys);
            DataTable table = ds.Tables[0].Copy();

            if (table.Columns.Contains("EDC_INS_KEY"))
            {
                table.Columns.Remove("EDC_INS_KEY");
            }

            string[] title = { "管控代码", "车间", "站点", "成品类型", "生产批号", "采集机台", "参数", "值", "班别", "班别", "采集时间", "采集人员", "硅片料号", "硅片供应商", "PECVD机台", "退火机台", "扩散机台" };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径
                string strinlocalFilePath = saveFileDialog1.FileName.ToString();

                //获取文件名，不带路径
                string fileNameExt = strinlocalFilePath.Substring(strinlocalFilePath.LastIndexOf("\\") + 1);
                //获取文件路径，不带文件名
                string FilePath = strinlocalFilePath.Substring(0, strinlocalFilePath.LastIndexOf("\\"));
                string newFileName = fileNameExt + DateTime.Now.ToString("yyyyMMdd");
                SaveToExcel.SaveExcel(table, newFileName, FilePath, c_title, title);
            }
            else
            {
                //这里放对取消的处理
            }
        }

        /// <summary>
        /// 单点批次信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointInformation(object sender, EventArgs e)
        {
            if (hi != null)
            {
                if (hi.InSeries && hi.SeriesPoint != null)
                {
                    SeriesPoint seriesPoint = hi.SeriesPoint;
                    SpcPoints spcPoint = (SpcPoints)seriesPoint.Tag;
                    //单点批次信息
                    PointInformation editPointDialog = new PointInformation(spcPoint.pointkeys);
                    editPointDialog.ShowDialog();
                }
            }
        }
        
             /// <summary>
        /// 单点详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointDetailInformation(object sender, EventArgs e)
        {
            if (hi != null)
            {
                if (hi.InSeries && hi.SeriesPoint != null)
                {
                    SeriesPoint seriesPoint = hi.SeriesPoint;
                    SpcPoints spcPoint = (SpcPoints)seriesPoint.Tag;
                    //单点详细信息
                    DetailInformation editPointDialog = new DetailInformation(spcPoint.pointkeys);
                    editPointDialog.ShowDialog();
                }
            }
        }
        /// <summary>
        /// 单点编辑信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointEditInformation(object sender, EventArgs e)
        {
            if (hi != null)
            {
                if (hi.InSeries && hi.SeriesPoint != null)
                {
                    SeriesPoint seriesPoint = hi.SeriesPoint;
                    SpcPoints spcPoint = (SpcPoints)seriesPoint.Tag;
                    //单点详细信息
                    EditInformation editPointDialog = new EditInformation(spcPoint.pointkeys);
                    editPointDialog.ShowDialog();
                }
            }
        }

        private void AddModifyPoint(object sender, EventArgs e)
        {
            editFlag = 2;
            ModifyPoint(null, null);
        }
        /// <summary>
        /// 修改异常点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyPoint(object sender, EventArgs e)
        {
            if (hi != null)
            {
                if (hi.InSeries && hi.SeriesPoint != null)
                {
                    SeriesPoint seriesPoint = hi.SeriesPoint;
                    SpcPoints spcPoint = (SpcPoints)seriesPoint.Tag;
                    //弹出修正对话框
                    EditPointDialog editPointDialog = new EditPointDialog(spcPoint.pointkeys,editFlag);
                    editFlag = 1;
                    if (DialogResult.OK == editPointDialog.ShowDialog())
                    {
                        //将修正的点变为十字架，区别显示 
                        spcPoint.editFlag = editPointDialog.EditFlag;
                        KeyValuePair<string, int> kp = new KeyValuePair<string, int>(spcPoint.pointkeys, spcPoint.editFlag);
                        SPCChartCtrl.updateIndexs.Add(kp);
                        string axisXIndex = seriesPoint.Argument;
                        Series series = (DevExpress.XtraCharts.Series)(hi.Series);
                        //series.Points[Convert.ToInt32(axisXIndex)-1].Tag = spcPoint;
                        int addresspints = -1;
                        for (int i = 0; i < series.Points.Count; i++)
                        {
                            if (axisXIndex == series.Points[i].Argument)
                            {
                                addresspints = i;
                                break;
                            }
                        }
                        if (addresspints != -1)
                            series.Points[addresspints].Tag = spcPoint;
                    }
                }
            }
        }
        /// <summary>
        /// 删除异常点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePoint(object sender, EventArgs e)
        {
            if (hi != null)
            {
                if (hi.InSeries && hi.SeriesPoint != null)
                {
                    SeriesPoint seriesPoint = hi.SeriesPoint;
                    SpcPoints spcPoint = (SpcPoints)seriesPoint.Tag;
                    //弹出删除对话框
                    if (MessageService.AskQuestion("确定要剔除数据吗？"))
                    {
                        DeletePointDialog editPointDialog = new DeletePointDialog(spcPoint.pointkeys);
                        if (DialogResult.OK == editPointDialog.ShowDialog())
                        {
                            //将修正的点变为十字架，区别显示 
                            spcPoint.deleteFlag = 1;
                            string axisXIndex = seriesPoint.Argument;
                            Series series = (DevExpress.XtraCharts.Series)(hi.Series);
                            //series.Points[Convert.ToInt32(axisXIndex) - 1].Tag = spcPoint;
                            int addresspints = -1;
                            for (int i = 0; i < series.Points.Count; i++)
                            {
                                if (axisXIndex == series.Points[i].Argument)
                                {
                                    addresspints = i;
                                    break;
                                }
                            }
                            if (addresspints != -1)
                                series.Points[addresspints].Tag = spcPoint;

                            if (!SPCChartCtrl.delPointkeys.Contains(spcPoint.pointkeys))
                                SPCChartCtrl.delPointkeys.Add(spcPoint.pointkeys);    
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 设定超出控制线的点
        /// </summary>
        /// <param name="ruleName"></param>
        public void Rules(string ruleName)
        {
            Series xSeries = this.chart01.Series["XSeries"];
            List<int> pointList = null;
            switch (ruleName)
            {
                case "WEC01"://超出控制线的点
                    for (int i = 0; i < xSeries.Points.Count; i++)
                    {
                        SpcPoints spcPoint = (SpcPoints)xSeries.Points[i].Tag;
                        if (xSeries.Points[i].Values[0] > xUCL || xSeries.Points[i].Values[0] < xLCL)
                        {
                            spcPoint.validateFlag = 1;
                        }
                        else
                        {
                            spcPoint.validateFlag = 0;
                        }
                       xSeries.Points[i].Tag = spcPoint;                        
                    }
                    break;
                case "WEC02"://连续3点中有2点在A区或是A区以外
                    pointList = new List<int>();
                    for (int i = 0; i < xSeries.Points.Count; i++)
                    {
                        if (i + 3 < xSeries.Points.Count + 1)
                        {
                            int count = 0;
                            for (int n = 0; n < 3; n++)
                            {
                                if (xSeries.Points[i + n].Values[0] > pTwoSigma || xSeries.Points[i + n].Values[0] < nTwoSigma)
                                    count++;
                            }
                            if (count >= 2)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                    if (xSeries.Points[i + k].Values[0] > pTwoSigma || xSeries.Points[i + k].Values[0] < nTwoSigma)
                                    {
                                        if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                        {
                                            pointList.Add(i + k);
                                        }
                                    }                                   
                                }
                                i++; //跳过以中间点为起点的检测，提高检测效率
                            }
                        }
                    }
                   // ChangePointColor(pointList);
                    break;
                case "WEC03"://连续5点中有4点在B区或是B区以外 
                    pointList = new List<int>();               
                    for (int i = 0; i < xSeries.Points.Count; i++)
                    {
                        if (i + 5 < xSeries.Points.Count + 1)
                        {
                            int count = 0;
                            for (int n = 0; n < 5; n++)
                            {
                                if (xSeries.Points[i + n].Values[0] > pOneSigma || xSeries.Points[i + n].Values[0] < nOneSigma)
                                    count++;
                            }
                            if (count >= 4)
                            {
                                for (int k = 0; k < 5; k++)
                                {
                                    SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                    if (xSeries.Points[i + k].Values[0] > pOneSigma || xSeries.Points[i + k].Values[0] < nOneSigma)
                                    {                                       
                                        if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                        {
                                            pointList.Add(i + k);
                                        }
                                    }                                  
                                }
                                i++;
                            }
                        }
                    }
                    //ChangePointColor(pointList);
                    break;
                case "WEC04"://连续8点中有7点落在单侧                  
                    pointList = new List<int>();
                    for (int i = 0; i < xSeries.Points.Count; i++)
                    {
                        if (i + 8 < xSeries.Points.Count + 1)
                        {
                            int pCount = 0, nCount = 0;
                            for (int n = 0; n < 8; n++)
                            {
                                if (xSeries.Points[i + n].Values[0] > xCL)
                                    pCount++;
                                else if (xSeries.Points[i + n].Values[0] < xCL)
                                    nCount++;
                            }
                            if (pCount >= 7 || nCount >= 7)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    SpcPoints spcPoint = (SpcPoints)xSeries.Points[i + k].Tag;
                                    if (pCount >= 7)
                                    {
                                        if (xSeries.Points[i + k].Values[0] > xCL)
                                        {
                                            if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                pointList.Add(i + k);
                                            }
                                        }                                          
                                    }
                                    else
                                    {
                                        if (xSeries.Points[i + k].Values[0] < xCL)
                                        {
                                            if (!pointList.Exists(delegate(int key) { return key == i + k; }))
                                            {
                                                pointList.Add(i + k);
                                            }
                                        }                                           
                                    }
                                }
                            }
                        }
                    }
                    //ChangePointColor(pointList);
                    break;
                default:
                    break;
            }
        }   
        /// <summary>
        /// 修改超出控制线的颜色
        /// </summary>
        /// <param name="pointList"></param>
        private void ChangePointColor(List<int> pointList, string normcode)
        {
            Series xSeries = this.chart01.Series["xSeries"];
            for (int i = 0; i < xSeries.Points.Count; i++)
            {

                SpcPoints spcPoint = (SpcPoints)xSeries.Points[i].Tag;

                if (pointList.Exists(delegate(int key) { return key == i; }))
                {
                    if (spcPoint.validateFlag == 0 && spcPoint.redFlag == 0)
                    {
                        spcPoint.validateFlag = 1;
                        spcPoint.redFlag = 1;
                    }

                    if (!spcPoint.abnormalRules.Contains(normcode))
                    {
                        if (spcPoint.abnormalRules.Length > 0)
                            spcPoint.abnormalRules += "\r" + normcode;
                        else
                            spcPoint.abnormalRules = normcode;
                    }
                }
                else
                {
                    if (spcPoint.validateFlag == 1 && spcPoint.redFlag == 0)
                    {
                        spcPoint.validateFlag = 0;
                    }
                }
                xSeries.Points[i].Tag = spcPoint;
            }
        }
      

        private void chartControl2_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            chartControl1_ObjectHotTracked(sender, e);
        }

        private void chartControl2_MouseDown(object sender, MouseEventArgs e)
        {
            chartControl1_MouseDown(sender, e);
        }
        
    }
}
