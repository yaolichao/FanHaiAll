using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using System.Threading;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraBars;
using FanHai.Hemera.Addins.SPC.Gui;

using DevExpress.XtraCharts;
using System.Drawing.Imaging;
using DevExpress.XtraCharts.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;

namespace FanHai.Hemera.Addins.SPC
{
    public partial class SPCChartCtrl : BaseUserCtrl
    {
        #region
        private SpcEntity spcEntity = new SpcEntity();
        private RouteQueryEntity route = new RouteQueryEntity();
        private X_MRChart X_mrChart = null;
        private Xbar_RChart XbarRChart = null;
        private Xbar_SChart XbarSChart = null;
        private XBARChart XbarChart = null;

        private DataTable dtControlPlan = new DataTable(), dtEquipment = new DataTable(), dtSpec = new DataTable();
        private DataTable dtMaterial = new DataTable(), dtSupplier = new DataTable(), dtRoute = new DataTable();

        DataSet paramValueDs = new DataSet(), paramValueDs_temp = new DataSet(), paramValueDs_New = new DataSet();
        DataTable dtStandard = new DataTable();
        string chartType = string.Empty;
        //记录多设备ID+设备名称
        string equipmentKeys = string.Empty;
        string equipmentNames = string.Empty;
        Dictionary<string, string> lst = new Dictionary<string, string>();
        string c_title = string.Empty;
        //管控计划是否为空
        string conid = string.Empty, conCode = string.Empty;
        /// <summary>
        /// 删除点的记录
        /// </summary>
        public static List<string> delPointkeys = new List<string>();
        /// <summary>
        /// 更新点的记录
        /// </summary>
        public static List<KeyValuePair<string, int>> updateIndexs = new List<KeyValuePair<string, int>>();

        private bool serchered = false;
        private int interval = 1;
        string p_sigma = string.Empty;
        #endregion

        public SPCChartCtrl()
        {
            InitializeComponent();
        }

        private void SPCChartCtrl_Load(object sender, EventArgs e)
        {
            BindLueData();
            rbtnPoints.Checked = true;
        }
        private void AddBlankRowToDataTable(DataTable dtComm)
        {
            if (dtComm.Rows.Count < 1) return;

            if (!dtComm.Rows[0][0].ToString().Equals(string.Empty))
            {
                DataRow drBlank = dtComm.NewRow();
                drBlank[0] = string.Empty;
                dtComm.Rows.InsertAt(drBlank, 0);
                dtComm.AcceptChanges();
            }
        }

        /// <summary>
        /// 绑定管控代码
        /// </summary>
        /// <param name="locationkey"></param>
        /// <param name="stepkey"></param>
        private void BindContralPlan(string locationkey, string stepkey)
        {
            DataSet ds = spcEntity.GetSPControlGridData();
            DataTable dt = new DataTable();
            dt = ds.Tables[0].Clone();
            string sql = string.Empty;

            if (!string.IsNullOrEmpty(stepkey) && !string.IsNullOrEmpty(locationkey))
            {
                sql += string.Format("WERKS='{0}' AND STEP_KEY='{1}'", locationkey, stepkey);
            }
            else
            {
                if (!string.IsNullOrEmpty(locationkey))
                {
                    sql += string.Format("WERKS='{0}'", locationkey);
                }

                if (!string.IsNullOrEmpty(stepkey))
                {
                    sql += string.Format("STEP_KEY='{0}'", stepkey);
                }
            }

            DataRow[] drs = ds.Tables[0].Select(string.Format("{0}", sql));
            foreach (DataRow dr in drs)
                dt.ImportRow(dr);
            lueControlCode.Properties.DisplayMember = SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE;
            lueControlCode.Properties.ValueMember = SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID;
            lueControlCode.Properties.DataSource = dt;
        }

        public void BindLueData()
        {
            DataSet dsControlPlan = spcEntity.GetSPControlGridData();
            dtControlPlan = dsControlPlan.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME];
            lueControlCode.Properties.DisplayMember = SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE;
            lueControlCode.Properties.ValueMember = SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID;
            lueControlCode.Properties.DataSource = dtControlPlan;


            DataSet dsBind = spcEntity.GetSpcControlPlan();
            //车间
            DataTable dtWeaks = dsBind.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
            AddBlankRowToDataTable(dtWeaks);
            this.lueWeaks.Properties.DataSource = dtWeaks;
            this.lueWeaks.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            this.lueWeaks.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;

            if (dsBind.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                lueWeaks.ItemIndex = -1;
            }

            //工序
            DataTable dtSteps = dsBind.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
            AddBlankRowToDataTable(dtSteps);
            this.lueSteps.Properties.DataSource = dtSteps;
            this.lueSteps.Properties.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            this.lueSteps.Properties.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;

            if (dsBind.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
            {
                lueSteps.ItemIndex = -1;
            }
            //设备
            dtEquipment = dsBind.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            cklRules.Enabled = false;
            //工艺路线
            dtRoute = route.GetActivedRouteData().Tables[0];
            dtRoute.TableName = POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME;            
        }

        private void BindMatRoutSupp(DataTable dt)
        {
            //-------------------------------------------------------------------------------------
            DataTable dtRoute01 = dtRoute.Clone();
            DataRow[] drs = dt.Select("ROUTE_KEY<>''");
            foreach (DataRow dr in drs)
            {
                if (dtRoute01.Select(string.Format(@"ROUTE_ROUTE_VER_KEY='{0}'", dr["ROUTE_KEY"].ToString())).Length > 0)
                    continue;
                DataRow[] drs02 = dtRoute.Select(string.Format("ROUTE_ROUTE_VER_KEY='{0}'", dr["ROUTE_KEY"].ToString()));
                foreach (DataRow dr02 in drs02)
                {
                    dtRoute01.ImportRow(dr02);
                    if (drs02.Length == 1)
                    {
                        DataRow dr01 = dtRoute01.NewRow();
                        dr01[0] = "0";
                        dr01[1] = "NOT";
                        dr01[2] = "#非_" + dr02[2].ToString();
                        dtRoute01.Rows.Add(dr01);
                    }
                }               
            }
            AddBlankRowToDataTable(dtRoute01);
            lueRouteFlow.Properties.DataSource = dtRoute01;
            //---------------------------------------------------------------------------------------
            DataTable dtm = new DataTable("Mt");
            dtm.Columns.Add("MATERIAL_LOT");
            DataTable dts = new DataTable("Sp");
            dts.Columns.Add("SUPPLIER");
            DataRow[] drms = dt.Select(string.Format(@"MATERIAL_LOT<>''"));
            DataRow[] drsps = dt.Select(string.Format(@"SUPPLIER<>''"));
            foreach (DataRow dr in drms)
            {
                if (dtm.Select(string.Format(@"MATERIAL_LOT='{0}'", dr["MATERIAL_LOT"])).Length > 0)
                    continue;
                DataRow dr01 = dtm.NewRow();
                dr01[0] = dr["MATERIAL_LOT"];
                dtm.Rows.Add(dr01);
            }
            foreach (DataRow dr in drsps)
            {
                if (dts.Select(string.Format(@"SUPPLIER='{0}'", dr["SUPPLIER"])).Length > 0)
                    continue;
                DataRow dr02 = dts.NewRow();
                dr02[0] = dr["SUPPLIER"];
                dts.Rows.Add(dr02);
            }

            if (dtm != null)
            {
                AddBlankRowToDataTable(dtm);
                lueMaterialLot.Properties.DataSource = dtm;
            }
            else
                lueMaterialLot.Properties.DataSource = null;

            if (dts != null)
            {
                AddBlankRowToDataTable(dts);
                lueSupplier.Properties.DataSource = dts;
            }
            else
                lueSupplier.Properties.DataSource = null;
        }

        private void FirstQuery(out bool blflag)
        {
            string _s = string.Empty;
            string points = string.Empty;
            blflag = true;
            if (rbtnDates.Checked && (deStartTime.Text.Length == 0))
            {
                MessageService.ShowMessage("请选择开始时间");
                deStartTime.BackColor = Color.Red;
                deStartTime.Focus();
                blflag = false;
               return ;
            }
            else
                deStartTime.BackColor = Color.White;

            if (rbtnPoints.Checked && (string.IsNullOrEmpty(txtPoints.Text.Trim())))
            {
                MessageService.ShowMessage("检测点数不能为空");
                txtPoints.BackColor = Color.Red;
                blflag = false;
                return ;
            }
            else
            {
                points = txtPoints.Text.Trim().Replace(",", "");
                if (points.Contains('.'))
                {
                    points = points.Substring(0, points.IndexOf('.'));
                }
                if (points == "0")
                {
                    MessageService.ShowMessage("检测点数不正确");
                    txtPoints.BackColor = Color.Red;
                    blflag = false;
                    return ;
                }
                else
                {
                    txtPoints.BackColor = Color.White;
                    txtPoints.Focus();
                }
            }

            Hashtable hashTable = new Hashtable();
            #region 按点查询
            if (rbtnPoints.Checked)
            {
                hashTable.Add("MODE_CODE", 0);
                hashTable.Add("MODE_VALUE_POINTS", Convert.ToInt32(points));
            }
            #endregion
            #region 按时间查询
            if (rbtnDates.Checked)
            {
                hashTable.Add("MODE_CODE", 1);
                hashTable.Add("MODE_VALUE_STARTDATE", deStartTime.Text.Trim());
                hashTable.Add("MODE_VALUE_ENDDATE", deEndTime.Text.Trim());
            }
            #endregion

            //班别
            if (cmbShift.Text.Length > 0)
                hashTable.Add(SPC_PARAM_DATA_FIELDS.SHIFT_VALUE, cmbShift.Text);

            hashTable.Add(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID, lueControlCode.EditValue.ToString());
            conid = lueControlCode.EditValue.ToString();
            paramValueDs_temp = new DataSet();
            DataTable dtPoints = new DataTable();
            try
            {
                string err = string.Empty;
                paramValueDs_New = spcEntity.SearchParamValue(hashTable, chartType, out err);
                if (!string.IsNullOrEmpty(err))
                {
                    MessageService.ShowError(err.Trim());
                    blflag = false;
                    return ;
                }
                dtStandard = paramValueDs_New.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_STANDARD];
                dtPoints = paramValueDs_New.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS];

            }
            catch //(Exception ex) 
            { }


            DataTable dtGroup = new DataTable();
            paramValueDs_temp = new DataSet();

            if (dtPoints.Rows.Count > 1)
                serchered = true;
            else
                serchered = false;

            delPointkeys.Clear();
            BindMatRoutSupp(dtPoints);
            paramValueDs_temp.Merge(dtStandard, false, MissingSchemaAction.Add);
            p_sigma = paramValueDs_New.ExtendedProperties[SPC_PARAM_DATA_FIELDS.P_SIGMA].ToString();
            paramValueDs_temp.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.P_SIGMA, p_sigma);

            //设备
            if (!string.IsNullOrEmpty(betEquipment.EditValue.ToString()))
            {
                dtGroup = dtPoints.Clone();
                string[] s_array = betEquipment.EditValue.ToString().Split(',');
                equipmentNames = betEquipment.Text.Trim();
                foreach (string s in s_array)
                {
                    string sfilter = string.Format(" EQUIPMENT_KEY = '{0}'", s.Trim());
                    DataRow[] drs = dtPoints.Select(sfilter);
                    foreach (DataRow dr in drs)
                        dtGroup.ImportRow(dr);
                }
            }
            else
            {
                dtGroup = dtPoints.Copy();
                equipmentNames = string.Empty;
            }
            dtGroup.AcceptChanges();
            paramValueDs_temp.Merge(dtGroup, false, MissingSchemaAction.Add);
        }

        private void SecondQuery(out bool blflag)
        {
            blflag = true;
            DataTable dtPoints = paramValueDs_New.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS];
            //delete points
            foreach (string pk in delPointkeys)
            {
                DataRow[] drdel = dtPoints.Select(string.Format(@"POINT_KEY='{0}'", pk));
                foreach (DataRow dr in drdel)
                    dtPoints.Rows.Remove(dr);
            }

            foreach (KeyValuePair<string, int> kvp in updateIndexs)
            {
                DataRow[] drs = dtPoints.Select(string.Format(@"POINT_KEY='{0}'", kvp.Key));
                if (drs != null && drs.Length > 0)
                {
                    int i = dtPoints.Rows.IndexOf(drs[0]);
                    dtPoints.Rows[i][SPC_PARAM_DATA_FIELDS.EDIT_FLAG] = kvp.Value;
                }
            }

            dtPoints.AcceptChanges();
            DataTable dtGroup = dtPoints.Clone();

            string sf = " 1=1";
            //供应商批次
            if (lueMaterialLot.EditValue != null && !string.IsNullOrEmpty(lueMaterialLot.EditValue.ToString()))
                sf += string.Format(" AND MATERIAL_LOT='{0}'", lueMaterialLot.EditValue.ToString());
            //供应商
            if (lueSupplier.EditValue != null && !string.IsNullOrEmpty(lueSupplier.EditValue.ToString()))
                sf += string.Format(" AND SUPPLIER='{0}'", lueSupplier.EditValue.ToString());
            //班别
            if (!string.IsNullOrEmpty(cmbShift.Text.Trim()))
                sf += string.Format(" AND SHIFT_VALUE ='{0}'", cmbShift.Text.Trim());
            //工艺路线
            if (lueRouteFlow.EditValue != null && !string.IsNullOrEmpty(lueRouteFlow.EditValue.ToString()))
                sf += string.Format(" AND ROUTE_KEY='{0}'", lueRouteFlow.EditValue.ToString());
          
            
            //Add sort condition
            string sort = " CREATE_TIME ASC";
            //DataTable valueTable_Control_Plan = paramValueDs_New.Tables["SPC_CONTROL_PLAN"];
            paramValueDs_temp = new DataSet();

            Hashtable ht = new Hashtable();
            //筛选设备
            if (!string.IsNullOrEmpty(betEquipment.EditValue.ToString()))
            {
                string e_keys = string.Empty;
                string[] s_array = betEquipment.EditValue.ToString().Split(',');
                equipmentNames = betEquipment.Text.Trim();
                foreach (string s in s_array)
                {
                    e_keys += "'" + s.Trim() + "',";
                    string sfilter = sf;
                    sfilter += string.Format(" AND EQUIPMENT_KEY = '{0}'", s.Trim());
                    DataRow[] drs = dtPoints.Select(sfilter);
                    foreach (DataRow dr in drs)
                        dtGroup.ImportRow(dr);
                }
                e_keys = e_keys.TrimEnd(',');
                ht[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY] = e_keys;
            }
            else
            {
                equipmentNames = string.Empty;
                DataRow[] drs = dtPoints.Select(sf);
                foreach (DataRow dr in drs)
                    dtGroup.ImportRow(dr);
            }

            dtGroup.AcceptChanges();
            if (dtGroup.Rows.Count < 1)
            {
                MessageService.ShowError("没有查询到数据!");
                blflag = false;
                return ;
            }
            DataView dvSort = dtGroup.DefaultView;
            dvSort.Sort = sort;

            paramValueDs_temp.Merge(dvSort.ToTable(dtGroup.TableName), false, MissingSchemaAction.Add);

            ht[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE] = conCode;
            string maxdate = dvSort.ToTable().Compute("MAX(CREATETIME)", "true").ToString();
            string mindate = dvSort.ToTable().Compute("MIN(CREATETIME)", "true").ToString();
            string vavg = dvSort.ToTable().Compute("avg(V_VALUE)", "true").ToString();
            string MIN_S_VALUE = dvSort.ToTable().Compute("MIN(S_VALUE)", "true").ToString();
            string MAX_S_VALUE = dvSort.ToTable().Compute("MAX(S_VALUE)", "true").ToString();
            string MIN_R_VALUE = dvSort.ToTable().Compute("MIN(R_VALUE)", "true").ToString();
            string MAX_R_VALUE = dvSort.ToTable().Compute("MAX(R_VALUE)", "true").ToString();

            ht["MAX_DATE"] = maxdate;
            ht["MIN_DATE"] = mindate;
            string strOut=string.Empty;
            DataSet dsSecond = spcEntity.GetSpcStandardSigma(ht,vavg, out strOut);
            dtStandard = dsSecond.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_STANDARD];
            dtStandard.Rows[0]["MAX_S_VALUE"] = MAX_S_VALUE;
            dtStandard.Rows[0]["MIN_S_VALUE"] = MIN_S_VALUE;
            dtStandard.Rows[0]["MIN_R_VALUE"] = MIN_R_VALUE;
            dtStandard.Rows[0]["MAX_R_VALUE"] = MAX_R_VALUE;
            dtStandard.AcceptChanges();
            p_sigma = dsSecond.ExtendedProperties[SPC_PARAM_DATA_FIELDS.P_SIGMA].ToString();

            paramValueDs_temp.Merge(dtStandard, false, MissingSchemaAction.Add);
            paramValueDs_temp.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.P_SIGMA, p_sigma);
        }

        private void btQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lueControlCode.Text.Trim()))
            {
                MessageService.ShowMessage("管控代码不能为空!");
                lueControlCode.BackColor = Color.Red;
                lueControlCode.Focus();
                return;
            }
            else
            {
                conCode = lueControlCode.Text.Trim();
                lueControlCode.BackColor = Color.White;
            }
            bool blflag = false;
            if (!lueControlCode.EditValue.ToString().Equals(conid))
                FirstQuery(out blflag);
            else
                SecondQuery(out blflag);
            if (!blflag) return;
            LoadChart();
        }

        private void LoadChart()
        {
            spcEntity = new SpcEntity();

            picPanel.Controls.Clear();
            try
            {
                if (paramValueDs_temp != null && paramValueDs_temp.Tables.Count > 0 && paramValueDs_temp.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS].Rows.Count > 0)
                {
                    dtSpec = paramValueDs_New.Tables[EDC_POINT_PARAMS_FIELDS.DATABASE_TABLE_NAME];
                    DataRow dr = null;
                    //设备
                    if (!string.IsNullOrEmpty(betEquipment.EditValue.ToString()))
                    {
                        string[] s_array = betEquipment.EditValue.ToString().Split(',');

                        string s = string.Format("EQUIPMENT_KEY='{0}'", s_array[0]);
                        DataRow[] drs = dtSpec.Select(s);
                        if (drs.Length > 0)
                            dr = drs[0];
                        else
                            dr = dtSpec.Rows[0];
                    }
                    else
                    {
                        DataRow[] drs = dtSpec.Select(@"EQUIPMENT_KEY=''");
                        if (drs.Length > 0)
                            dr = drs[0];
                        else
                            dr = dtSpec.Rows[0];
                    }

                    spcEntity.USL = dr[EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY].ToString();
                    spcEntity.SL = dr[EDC_POINT_PARAMS_FIELDS.FIELD_TARGET].ToString();
                    spcEntity.LSL = dr[EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY].ToString();
                    spcEntity.UCL = dr[EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC].ToString();
                    spcEntity.LCL = dr[EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC].ToString(); 

                    if (paramValueDs_New.ExtendedProperties.ContainsKey("C_TITLE"))
                        c_title = paramValueDs_New.ExtendedProperties["C_TITLE"].ToString() + dtSpec.Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME].ToString();

                    this.layoutControlGroupCondition.Expanded = false;
                    picLoad.Visible = true;
                    picPanel.Controls.Add(picLoad);

                    Control.CheckForIllegalCrossThreadCalls = false;
                    BackgroundWorker backWorker = new BackgroundWorker();
                    backWorker.DoWork += new DoWorkEventHandler((r, d) =>
                    {
                        InitializeChart(paramValueDs_temp, spcEntity);
                    });
                    backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((r, c) =>
                    {
                        picLoad.Visible = false;
                        #region
                        switch (chartType.ToUpper())
                        {
                            case ChartType.XBAR_R:
                                picPanel.Controls.Add(XbarRChart);
                                XbarRChart.Dock = DockStyle.Fill;
                                break;
                            case ChartType.XBAR_S:
                                picPanel.Controls.Add(XbarSChart);
                                XbarSChart.Dock = DockStyle.Fill;
                                break;
                            case ChartType.XBAR_MR:
                                picPanel.Controls.Add(X_mrChart);
                                X_mrChart.Dock = DockStyle.Fill;
                                break;
                            case ChartType.XBAR:
                                picPanel.Controls.Add(XbarChart);
                                XbarChart.Dock = DockStyle.Fill;
                                break;
                            default:
                                break;
                        }
                        #endregion

                    });
                    backWorker.RunWorkerAsync();
                }
                else
                {
                    MessageService.ShowMessage("没有查询到结果");
                }
            }
            catch (Exception ex)
            {
                conid = string.Empty;
                MessageService.ShowError(ex.Message);
            }
        }

        private void InitializeChart(DataSet valueDataSet, SpcEntity spcEntity)
        {
            if (txtPointsInterval.Text.Trim() != "" && Convert.ToInt32(txtPointsInterval.Text) > 0)
                interval = Convert.ToInt32(txtPointsInterval.Text);
            string c_title_01 = c_title;
            if (!equipmentNames.Trim().Equals(string.Empty))
                c_title_01 = equipmentNames + "," + c_title;
            #region 
            switch (chartType.ToUpper())
            {
                case ChartType.XBAR_R:
                    XbarRChart = new Xbar_RChart(valueDataSet, spcEntity, lst, c_title_01, interval, conCode);
                    break;
                case ChartType.XBAR_S:
                    XbarSChart = new Xbar_SChart(valueDataSet, spcEntity, lst, c_title_01, interval, conCode);
                    break;
                case ChartType.XBAR_MR:
                    X_mrChart = new X_MRChart(valueDataSet, spcEntity, lst, c_title_01, interval, conCode);
                    break;
                case ChartType.XBAR:
                    XbarChart = new XBARChart(valueDataSet, spcEntity, lst, c_title_01, interval,conCode);
                    break;
                default:
                    break;
            }
            #endregion
        }

        private void cklRules_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            DataTable dtCklRules = cklRules.DataSource as DataTable;
            if (dtCklRules == null || dtCklRules.Rows.Count < 1) return;
            DataRow dr = dtCklRules.Rows[e.Index];
            if (e.State == CheckState.Checked)
            {
                if (!lst.ContainsValue(dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID].ToString()))
                    lst.Add(dr[EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString(), dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID].ToString());
            }
            else
            {
                if (!lst.ContainsValue(dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID].ToString()))
                    lst.Remove(dr[EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString());
            }
        }

        private void rbtnPoints_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPoints.Checked)
            {
                this.rbtnDates.Checked = false;
                layoutControlItemPoints.Visibility = LayoutVisibility.Always;
                layoutControlItemStartDate.Visibility = LayoutVisibility.Never;
                layoutControlItemEndDate.Visibility = LayoutVisibility.Never;
                conid = string.Empty;
                conCode = string.Empty;
            }
        }

        private void rbtnDates_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDates.Checked)
            {
                rbtnPoints.Checked = false;
                layoutControlItemStartDate.Visibility = LayoutVisibility.Always;
                layoutControlItemEndDate.Visibility = LayoutVisibility.Always;
                layoutControlItemPoints.Visibility = LayoutVisibility.Never;
                conid = string.Empty;
                conCode = string.Empty;
            }
        }

        private void lueControlCode_EditValueChanged(object sender, EventArgs e)
        {
            DataRow dr = dtControlPlan.Select(string.Format(@"CONTROLPLANID='{0}'", lueControlCode.EditValue.ToString()))[0];
            if (dr == null) return;

            betEquipment.Text = string.Empty;
            equipmentKeys = string.Empty;
            string locationKey = string.Empty, stepKey = string.Empty;

            chartType = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
            string strAnormalIDs = dr[SPC_CONTROL_PLAN_FIELD.FIELD_ABNORMALIDS].ToString();
            locationKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString();
            stepKey = dr[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].ToString();

            this.layoutControlItem12.Visibility = LayoutVisibility.Never;
            this.layoutControlItem7.Visibility = LayoutVisibility.Never;
 
            string s = string.Empty;
            if (strAnormalIDs.Length > 0)
            {
                foreach (string str in strAnormalIDs.Split(','))
                    s += "'" + str + "',";

                if (string.IsNullOrEmpty(s)) return;
                s = s.TrimEnd(',');
            }
            try
            {
                DataTable dt01 = spcEntity.GetAbnormalRule().Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];
                string s01 = string.Format(EDC_ABNORMAL_FIELDS.FIELD_ARULECODE + " IN ({0})", s);
                DataView dv = dt01.DefaultView;
                dv.RowFilter = s01;
                DataTable dt02 = dv.ToTable();
                cklRules.DataSource = dt02;
                cklRules.DisplayMember = EDC_ABNORMAL_FIELDS.FIELD_ARULECODE;
                cklRules.ValueMember = EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID;
                cklRules.Enabled = true;

                for (int i = 0; i < cklRules.Items.Count; i++)
                    cklRules.Items[i].CheckState = CheckState.Checked;

                cklRules.CheckAll();
            }
            catch //(Exception ex)
            { }


        }

        /// <summary>
        /// 工厂车间EditValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueWeaks_EditValueChanged(object sender, EventArgs e)
        {
            if (lueWeaks.ItemIndex > -1)
            {
                lueSteps.ItemIndex = -1;
                betEquipment.Text = string.Empty;
                equipmentKeys = string.Empty;
                BindGridView();
            }
        }

        //Editor By jing.xie 2012-06-19 修改代码
        public void BindGridView()
        {
            DataTable dt01 = dtControlPlan;
            DataTable dt02 = dtControlPlan.Clone();
            string locationkey = string.Empty;
            string stepkey = string.Empty;


            if (lueWeaks.EditValue != null)
                locationkey = lueWeaks.EditValue.ToString();
            if (lueSteps.EditValue != null)
                stepkey = lueSteps.EditValue.ToString();

            if (lueWeaks.EditValue == null && lueSteps.EditValue == null)
            {
                this.betEquipment.Properties.Items.Clear();
                betEquipment.Text = string.Empty;
                equipmentKeys = string.Empty;
            }

            DataRow[] drs = null;
            if (!string.IsNullOrEmpty(locationkey) && !string.IsNullOrEmpty(stepkey))
                drs = dt01.Select(string.Format(@"WERKS='{0}' AND STEP_KEY='{1}'", locationkey, stepkey));
            else if (!string.IsNullOrEmpty(locationkey))
                drs = dt01.Select(string.Format(@"WERKS='{0}' ", locationkey));
            else if (!string.IsNullOrEmpty(stepkey))
                drs = dt01.Select(string.Format(@" STEP_KEY='{0}'", stepkey));
            try
            {
                foreach (DataRow dr in drs)
                    dt02.ImportRow(dr);
                dt02.AcceptChanges();

                lueControlCode.Properties.DataSource = dt02;
                LoadEquiment(locationkey, stepkey);
            }
            catch //(Exception ex)
            { }
        }

        /// <summary>
        /// 工序EditValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueSteps_EditValueChanged(object sender, EventArgs e)
        {
            if (lueSteps.ItemIndex > -1)
            {
                betEquipment.Text = string.Empty;
                equipmentKeys = string.Empty;
                BindGridView();
            }
        }

        //Editor By jing.xie 2012-06-19 添加代码
        /// <summary>
        /// 加载设备
        /// </summary>
        /// <param name="locationkey">工厂车间KEY</param>
        /// <param name="stepkey">工序KEY</param>
        private void LoadEquiment(string locationkey, string stepkey)
        {
            #region 加载设备
            DataTable dt = dtEquipment.Clone();
            DataRow[] drs = dtEquipment.Select(string.Format("LOCATION_KEY='{0}' AND OPERATION_KEY='{1}'", locationkey, stepkey));

            foreach (DataRow dr in drs)
                dt.ImportRow(dr);

            if (dt != null && dt.Rows.Count > 0)
            {
                this.betEquipment.Properties.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    string equipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
                    string equipmentCode = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);
                    equipmentKeys = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                    string description = string.Format("{0}({1})", equipmentName, equipmentCode);
                    this.betEquipment.Properties.Items.Add(equipmentKeys.Trim(), description);
                }
            }
            else
            {
                this.betEquipment.Properties.Items.Clear();
            }
            #endregion
        }

        private void txtPoints_EditValueChanged(object sender, EventArgs e)
        {
            conid = string.Empty;
            conCode = string.Empty;
        }

        private void deStartTime_EditValueChanged(object sender, EventArgs e)
        {          
        }

        private void deEndTime_EditValueChanged(object sender, EventArgs e)
        {     
        }

        private void txtPointsInterval_EditValueChanged(object sender, EventArgs e)
        {
            if (serchered)
            {
                if (txtPointsInterval.Text != "" && Convert.ToInt32(txtPointsInterval.Text.Trim()) > 0)
                    interval = Convert.ToInt32(txtPointsInterval.Text);
                    
                switch (chartType.ToUpper())
                {
                    case ChartType.XBAR_R:
                        XbarRChart.SetAxiexInterval(interval);
                        break;
                    case ChartType.XBAR_S:
                   XbarSChart.SetAxiexInterval(interval);
                        break;
                    case ChartType.XBAR_MR:
                        X_mrChart.SetAxiexInterval(interval);
                        break;
                    case ChartType.XBAR:
                        XbarChart.SetAxiexInterval(interval);
                        break;
                    default:
                        break;
                }
            }
        }
    }
   
}
