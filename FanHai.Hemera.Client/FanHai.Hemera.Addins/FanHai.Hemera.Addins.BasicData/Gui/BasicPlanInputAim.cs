using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using System.Collections;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
using System.IO;
using System.Reflection;
using DevExpress.XtraEditors.Controls;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicPlanInputAim : BaseUserCtrl
    {
        RptCommonEntity _rptEntity = new RptCommonEntity();
        ControlState _ctrlState = new ControlState();
        public delegate void AfterStateChanged(ControlState controlState);
        public AfterStateChanged afterStateChanged = null;

        public BasicPlanInputAim()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            btnQuery.Text = StringParser.Parse("${res:Global.Query}");//查询
            this.btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0001}");//新增
            this.btnModify.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0002}");//修改
            this.btnDel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0003}");//删除
            this.btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0004}");//取消
            this.btSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0005}");//保存
            this.btExport.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0006}");//导出表格数据到Excel
            lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0007}");//计划目标值数据
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0008}");//工厂车间
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0009}");//产品ID号
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0010}");//产品型号
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0011}");//工单号
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0012}");//开始时间
            layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0013}");//结束时间
            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicPlanInputAim.lbl.0014}");//计划输入列表
            PLAN_DATE_START.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0001}");//计划开始日期
            SHIFT_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0002}");//班别
            LOCATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0003}");//车间名称
            LINE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0004}");//线别
            PART_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0005}");//产品型号
            PRO_ID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0006}");//产品ID号
            WORK_ORDER_NO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0007}");//工单号
            QUANTITY_INPUT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0008}");//计划输入量
            QUANTITY_OUTPUT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0009}");//计划产出数量
           
        }

        private void BasicPlanInputAim_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(onChangeControlState);
            BindFactoryRoom();
            BindLines();
            BindQueryData();
            InitGridView();
            this.CtrlState = ControlState.ReadOnly;
        }

        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        public void onChangeControlState(ControlState cState)
        {
            switch (cState)
            {
                case ControlState.Edit:
                    this.btnAdd.Enabled = false;
                    this.btnDel.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.gvPlan.OptionsBehavior.Editable = true;
                    break;
                case ControlState.New:
                    this.btnDel.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnAdd.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.gvPlan.OptionsBehavior.Editable = true;
                    AddNewRow();
                    break;
                case ControlState.ReadOnly:
                    this.btnAdd.Enabled = true;
                    this.btnDel.Enabled = true;
                    this.btSave.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.gvPlan.OptionsBehavior.Editable = false;
                    InitGridView();
                    break;
                default:
                    break;
            }
        }

        private void AddNewRow()
        {
             DataRow drData=null;
             if (this.gvPlan.FocusedRowHandle > -1)
                 drData = this.gvPlan.GetFocusedDataRow();

            DataTable dtAddRow = gcPlan.DataSource as DataTable;
            DataRow dr = dtAddRow.NewRow();
            dr[RPT_PLAN_AIM.FIELDS_CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            dr[RPT_PLAN_AIM.FIELDS_PLANID] = CommonUtils.GenerateNewKey(0);
            dr[RPT_PLAN_AIM.FIELDS_ISNEW] = BASE_DECAYCOEFFI.FIELDS_ISNEW;

            if (drData != null)
            {
                dr[RPT_PLAN_AIM.FIELDS_LOCATION_NAME] = drData[RPT_PLAN_AIM.FIELDS_LOCATION_NAME];
                dr[RPT_PLAN_AIM.FIELDS_LOT_TYPE] = drData[RPT_PLAN_AIM.FIELDS_LOT_TYPE];
                dr[RPT_PLAN_AIM.FIELDS_PART_TYPE] = drData[RPT_PLAN_AIM.FIELDS_PART_TYPE];
                dr[RPT_PLAN_AIM.FIELDS_PRO_ID] = drData[RPT_PLAN_AIM.FIELDS_PRO_ID];
                dr[RPT_PLAN_AIM.FIELDS_QUANTITY_INPUT] = drData[RPT_PLAN_AIM.FIELDS_QUANTITY_INPUT];
                dr[RPT_PLAN_AIM.FIELDS_QUANTITY_OUTPUT] = drData[RPT_PLAN_AIM.FIELDS_QUANTITY_OUTPUT];
                dr[RPT_PLAN_AIM.FIELDS_SHIFT_NAME] = drData[RPT_PLAN_AIM.FIELDS_SHIFT_NAME];
                dr[RPT_PLAN_AIM.FIELDS_SHIFT_VALUE] = drData[RPT_PLAN_AIM.FIELDS_SHIFT_VALUE];
                dr[RPT_PLAN_AIM.FIELDS_WORK_ORDER_NO] = dr[RPT_PLAN_AIM.FIELDS_WORK_ORDER_NO];
                dr[RPT_PLAN_AIM.FIELDS_LINE_CODE] = drData[RPT_PLAN_AIM.FIELDS_LINE_CODE];
                dr[RPT_PLAN_AIM.FIELDS_LINE_NAME] = drData[RPT_PLAN_AIM.FIELDS_LINE_NAME];
            }

            dtAddRow.Rows.Add(dr);
            this.gcPlan.DataSource = dtAddRow;
        }

        private void BindQueryData()
        {
            DataSet dsReturn = _rptEntity.GetProWoModuleType();
            DataTable dtProduct = dsReturn.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
            
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                this.ccbProId.Properties.Items.Clear();
                foreach (DataRow dr in dtProduct.Rows)
                {
                    string productcode = Convert.ToString(dr[POR_PRODUCT.FIELDS_PRODUCT_CODE]);
                    //string equipmentCode = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);
                    string productkey = Convert.ToString(dr[POR_PRODUCT.FIELDS_PRODUCT_KEY]);

                    this.ccbProId.Properties.Items.Add(productkey.Trim(), productcode);
                }
            }
            else
            {
                this.ccbProId.Properties.Items.Clear();
            }
            DataTable dtWo = dsReturn.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            if (dtWo != null && dtWo.Rows.Count > 0)
            {
                this.ccbWorkOrder.Properties.Items.Clear();
                foreach (DataRow dr in dtWo.Rows)
                {
                    string workorder = Convert.ToString(dr[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);               
                    string workey = Convert.ToString(dr[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]);

                    this.ccbWorkOrder.Properties.Items.Add(workey.Trim(), workorder);
                }
            }
            else
            {
                this.ccbWorkOrder.Properties.Items.Clear();
            }

            DataTable dtModel = dsReturn.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_NAME];
            if (dtModel != null && dtModel.Rows.Count > 0)
            {
                this.ccbPartType.Properties.Items.Clear();
                foreach (DataRow dr in dtModel.Rows)
                {
                    string modelname = Convert.ToString(dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME]);
                    string modelkey = Convert.ToString(dr[BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY]);

                    this.ccbPartType.Properties.Items.Add(modelkey.Trim(), modelname);
                }

                this.repository_modelType.DataSource = dtModel;
                this.repository_modelType.DisplayMember = BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME;
                this.repository_modelType.ValueMember = BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME;
            }
            else
            {
                this.ccbPartType.Properties.Items.Clear();
            }

            this.dateEditStart.Text = new LotAfterIvTestEntity().GetSysdate().AddDays(-7).ToString();
            this.dateEditEnd.Text = new LotAfterIvTestEntity().GetSysdate().ToString();


            //统计班别--暂时添加白班，夜班
            //string[] l_s = new string[] { "CODE" };
            //string category = "Basic_Shift";
            //DataTable dtCommon = BaseData.Get(l_s, category);
            DataTable dtCommon = new DataTable();
            dtCommon.Columns.Add("CODE");
            dtCommon.Rows.Add("白班");
            dtCommon.Rows.Add("夜班");

            this.repository_Shift.DataSource = dtCommon;
            this.repository_Shift.DisplayMember = "CODE";
            this.repository_Shift.ValueMember = "CODE";

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitGridView();
        }

        private void InitGridView()
        {
            Hashtable hsParams = new Hashtable();
            //查询条件
            #region
            if (!dateEditStart.Text.Trim().Equals(string.Empty))
                hsParams.Add(RPT_PLAN_AIM.FIELDS_CREATE_TIME_START, dateEditStart.Text.Trim());
            if (!dateEditEnd.Text.Trim().Equals(string.Empty))
                hsParams.Add(RPT_PLAN_AIM.FIELDS_CREATE_TIME_END, dateEditEnd.Text.Trim());
            if (!string.IsNullOrEmpty(Convert.ToString(lueFactoryRoom.Text)))
            {
                string[] s_array = Convert.ToString(lueFactoryRoom.Text).Split(',');
                string locationkey = string.Empty;
                foreach (string s in s_array)
                {
                    locationkey += "'" + s.Trim() + "',";
                }
                if (!string.IsNullOrEmpty(locationkey))
                {
                    locationkey = locationkey.TrimEnd(',');
                }

                hsParams.Add(RPT_PLAN_AIM.FIELDS_LOCATION_NAME, locationkey);
            }

            if (!string.IsNullOrEmpty(ccbPartType.Text.Trim()))
            {
                string[] s_array = Convert.ToString(ccbPartType.Text).Split(',');
                string modelname = string.Empty;
                foreach (string s in s_array)
                {
                    modelname += "'" + s.Trim() + "',";
                }
                if (!string.IsNullOrEmpty(modelname))
                {
                    modelname = modelname.TrimEnd(',');
                }

                hsParams.Add(RPT_PLAN_AIM.FIELDS_PART_TYPE, modelname);
            }

            if (!string.IsNullOrEmpty(ccbProId.Text.Trim()))
            {
                string[] s_array = Convert.ToString(ccbProId.Text).Split(',');
                string procode = string.Empty;
                foreach (string s in s_array)
                {
                    procode += "'" + s.Trim() + "',";
                }
                if (!string.IsNullOrEmpty(procode))
                {
                    procode = procode.TrimEnd(',');
                }

                hsParams.Add(RPT_PLAN_AIM.FIELDS_PRO_ID, procode);               
            }
            if (!string.IsNullOrEmpty(ccbWorkOrder.Text.Trim()))
            {
                string[] s_array = Convert.ToString(ccbWorkOrder.Text).Split(',');
                string workorders = string.Empty;
                foreach (string s in s_array)
                {
                    workorders += "'" + s.Trim() + "',";
                }
                if (!string.IsNullOrEmpty(workorders))
                {
                    workorders = workorders.TrimEnd(',');
                }

                hsParams.Add(RPT_PLAN_AIM.FIELDS_WORK_ORDER_NO, workorders);
            }
            #endregion

            DataSet dsReturn = _rptEntity.GetRptPlanAimData(hsParams);
            if (!string.IsNullOrEmpty(_rptEntity.ErrorMsg))
            {
                MessageService.ShowError(_rptEntity.ErrorMsg);
                return;
            }
            this.gcPlan.MainView = this.gvPlan;
            this.gcPlan.DataSource = dsReturn.Tables[RPT_PLAN_AIM.DATABASE_TABLE_NAME];
            //this.gvPlan.BestFitColumns();
        }

      
        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);

            if (dt != null && dt.Rows.Count > 0)
            {
                this.lueFactoryRoom.Properties.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    string locationame = Convert.ToString(dr[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME]);
                    string locationkey = Convert.ToString(dr[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);

                    this.lueFactoryRoom.Properties.Items.Add(locationkey.Trim(), locationame);
                }

                this.repository_LocationName.DataSource = dt;
                this.repository_LocationName.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
                this.repository_LocationName.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;      
            }
            else
            {
                this.lueFactoryRoom.Properties.Items.Clear();
            }
        }

       /// <summary>
        /// 绑定线别
        /// </summary>
        private void BindLines()
        {
            DataTable dtLines; //获取线别列表
            DataSet dsLines;
            dsLines = _rptEntity.GetLines();

            if (dsLines != null && dsLines.Tables.Count > 0)
            {
                dtLines = dsLines.Tables[0];

                int intCount = (dtLines != null) ? dtLines.Rows.Count : 0;

                if (intCount > 0)
                {
                    this.repository_Lines.DataSource = dtLines;
                    this.repository_Lines.DisplayMember = RPT_PLAN_AIM.FIELDS_LINE_NAME;
                    this.repository_Lines.ValueMember = RPT_PLAN_AIM.FIELDS_LINE_CODE;
                    //this.repository_Lines.Properties.Columns.Add(new LookUpColumnInfo(RPT_PLAN_AIM.FIELDS_LINE_NAME));
                    //this.repository_Lines.Properties.Columns.Add(new LookUpColumnInfo(RPT_PLAN_AIM.FIELDS_LINE_CODE)); 
                }
            }

        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.Edit;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.New;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.gvPlan.FocusedRowHandle < 0 || this.gvPlan.RowCount < 1)
            {
                //MessageService.ShowMessage("请选择删除的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                return;
            }

            if (this.gvPlan.FocusedRowHandle > -1)
            {
                //if (MessageService.AskQuestion("确定要删除该笔数据么?", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataTable dtplan = gcPlan.DataSource as DataTable;
                    DataSet dsplan = new DataSet();
                    string planid = this.gvPlan.GetRowCellValue(this.gvPlan.FocusedRowHandle, RPT_PLAN_AIM.FIELDS_PLANID).ToString();

                    DataTable dtSave = dtplan.Clone();
                    DataRow[] drDels = dtplan.Select(string.Format(RPT_PLAN_AIM.FIELDS_PLANID + "='{0}'", planid));
                    if (drDels.Length > 0)
                    {
                        DataRow drDel = drDels[0];
                        drDel[RPT_PLAN_AIM.FIELDS_ISFLAG] = 0;
                        dtSave.ImportRow(drDel);
                        dtSave.TableName = RPT_PLAN_AIM.DATABASE_TABLE_FORUPDATE;
                        dsplan.Merge(dtSave, true, MissingSchemaAction.Add);

                        bool bl_Bak = _rptEntity.SaveRptPlanAimData(dsplan);
                        if (!bl_Bak)
                        {
                            MessageService.ShowMessage(_rptEntity.ErrorMsg);
                        }
                        else
                        {
                            //MessageService.ShowMessage("删除成功!");
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0003}"));

                            dtplan.Rows.Remove(drDel);
                            dtplan.AcceptChanges();
                            gcPlan.DataSource = dtplan;
                        }

                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.ReadOnly;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            DataTable dtPlan = gcPlan.DataSource as DataTable;
            DataTable dtPlan_Update = dtPlan.GetChanges(DataRowState.Modified);
            DataTable dtPlan_Insert = dtPlan.GetChanges(DataRowState.Added);

            DataSet dsPlan = new DataSet();

            if (dtPlan_Update != null && dtPlan_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtPlan_Update.Clone();
                foreach (DataRow dr in dtPlan_Update.Rows)
                {
                    DataRow[] drUpdates = dtPlan.Select(string.Format(RPT_PLAN_AIM.FIELDS_PLANID + "='{0}'", dr[RPT_PLAN_AIM.FIELDS_PLANID].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtUpdate.Rows.Add(drNew);
                }


                dtUpdate.TableName = RPT_PLAN_AIM.DATABASE_TABLE_FORUPDATE;
                dsPlan.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (dtPlan_Insert != null && dtPlan_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtPlan_Insert.Clone();
                foreach (DataRow dr in dtPlan_Insert.Rows)
                {
                    DataRow[] drUpdates = dtPlan.Select(string.Format(RPT_PLAN_AIM.FIELDS_PLANID + "='{0}'", dr[RPT_PLAN_AIM.FIELDS_PLANID].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtInsert.Rows.Add(drNew);
                }


                dtInsert.TableName = RPT_PLAN_AIM.DATABASE_TABLE_FORINSERT;
                dsPlan.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (dsPlan.Tables.Count > 0)
            {
                bool bl_Bak = _rptEntity.SaveRptPlanAimData(dsPlan);
                if (!bl_Bak)
                {
                    //MessageService.ShowMessage("保存失败!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0004}"));
                }
                else
                {
                    //MessageService.ShowMessage("保存成功!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0005}"));
                    this.CtrlState = ControlState.ReadOnly;
                }
            } 
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if (this.gvPlan.RowCount > 0)
                print();
        }
        /// <summary>
        /// 导出表格数据
        /// </summary>
        /// DataGridView dataGridView1
        public void print()
        {
            //导出到execl  
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView gvExport = this.gvPlan;

                string fieldpath = string.Empty, fileNameExt = string.Empty;
                saveExcelDialog.Filter = "excel文件(*.xls)|*.xls";
                saveExcelDialog.DefaultExt = "xls";
                saveExcelDialog.InitialDirectory = Directory.GetCurrentDirectory();

                saveExcelDialog.RestoreDirectory = true;

                if (DialogResult.OK == saveExcelDialog.ShowDialog())
                {
                    int rowscount = gvExport.RowCount;
                    int colscount = gvExport.Columns.Count;

                    if (rowscount > 65536)
                    {
                        //MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //列数不可以大于255
                    if (colscount > 255)
                    {
                        //MessageBox.Show("数据记录行数太多，不能保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    fieldpath = saveExcelDialog.FileName;
                    fileNameExt = fieldpath.Substring(fieldpath.LastIndexOf("\\") + 1);

                    //验证以fileNameString命名的文件是否存在，如果存在删除它
                    FileInfo file = new FileInfo(fieldpath);
                    if (file.Exists)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception error)
                        {
                            //删除失败！
                            MessageBox.Show(error.Message, StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0008}"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook objWorkbook = objExcel.Workbooks.Add(Missing.Value);
                    Microsoft.Office.Interop.Excel.Worksheet objsheet = (Microsoft.Office.Interop.Excel.Worksheet)objWorkbook.ActiveSheet;
                    try
                    {
                        //设置EXCEL不可见
                        objExcel.Visible = false;
                        //向Excel中写入表格的表头
                        int displayColumnsCount = 1;
                        for (int i = 0; i < gvExport.Columns.Count; i++)
                        {
                            if (gvExport.Columns[i].Visible == true)
                            {
                                string tmp = gvExport.Columns[i].Caption;
                                //objExcel.Cells[1, displayColumnsCount] = tmp;
                                objsheet.Cells[1, displayColumnsCount] = tmp;
                                displayColumnsCount++;
                            }
                        }


                        //向Excel中逐行逐列写入表格中的数据
                        for (int row = 0; row < gvExport.RowCount; row++)
                        {
                            string sfront = "'";
                            displayColumnsCount = 1;
                            for (int col = 0; col < colscount; col++)
                            {
                                if (gvExport.Columns[col].Visible == true)
                                {
                                    if (gvExport.Columns[col].FieldName.Equals(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM)
                                        || gvExport.Columns[col].FieldName.Equals(WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER))
                                        objsheet.Cells[row + 2, displayColumnsCount] = sfront + gvExport.GetRowCellValue(row, gvExport.Columns[col].FieldName).ToString().Trim();
                                    else
                                        objsheet.Cells[row + 2, displayColumnsCount] = gvExport.GetRowCellValue(row, gvExport.Columns[col].FieldName).ToString().Trim();
                                    displayColumnsCount++;
                                }
                            }
                        }

                        //保存文件
                        objWorkbook.SaveAs(fieldpath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
                            Missing.Value, Missing.Value);


                    }
                    catch (Exception error)
                    {
                        //MessageBox.Show(error.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        MessageBox.Show(error.Message, StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0009}"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    finally
                    {
                        //关闭Excel应用
                        if (objWorkbook != null) objWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
                        if (objExcel.Workbooks != null) objExcel.Workbooks.Close();
                        if (objExcel != null) objExcel.Quit();

                        objsheet = null;
                        objWorkbook = null;
                        objExcel = null;
                    }

                    //MessageBox.Show(fieldpath + "\n\n导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(fieldpath + StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }
    }
}
