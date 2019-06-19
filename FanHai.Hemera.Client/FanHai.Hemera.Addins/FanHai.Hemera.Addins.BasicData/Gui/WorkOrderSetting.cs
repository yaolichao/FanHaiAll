using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    public partial class WorkOrderSetting : BaseUserCtrl
    {
        WorkOrders workOrdersEntity = new WorkOrders();
        PorProductEntity porProductEntity = new PorProductEntity();
        private delegate void AfterStateChanged(ControlState controlState);
        private AfterStateChanged afterStateChanged = null;
        private ControlState _controlState = ControlState.Empty;
        string por_workno_key = string.Empty,attribute_key=string.Empty;
        DataTable dtWorkOrderAttr = new DataTable(), dtlueWo = new DataTable(), dtlueWoNotExistProid = new DataTable();
        public WorkOrderSetting()
        {
            InitializeComponent();
        }
        #region State Change
        private ControlState State
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;

                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Deal with state change event
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {

                case ControlState.ReadOnly:
                    this.btnModify.Enabled = true;
                    this.btnSave.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnAdd_wo_attr.Enabled = false;
                    this.btnDel_wo_attr.Enabled = false;
                    this.btnAdd_wo_material.Enabled = false;
                    //this.btnDel_wo_material.Enabled = false;
                    this.gvWoAttr.OptionsBehavior.Editable = false;
                    //this.gvWoMaterial.OptionsBehavior.Editable = false;
                    this.lueProductId.Properties.ReadOnly = true;
                    this.lueWorkOrder.Properties.ReadOnly = true;
                    this.memoWorkOrder.Properties.ReadOnly = true;
                    break;
                case ControlState.Edit:
                    this.btnModify.Enabled = false;
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.btnAdd_wo_attr.Enabled = true;
                    this.btnDel_wo_attr.Enabled = true;
                    this.btnAdd_wo_material.Enabled = true;
                    //this.btnDel_wo_material.Enabled = true;
                    this.gvWoAttr.OptionsBehavior.Editable = true;
                    //this.gvWoMaterial.OptionsBehavior.Editable = true;
                    this.lueProductId.Properties.ReadOnly = false;
                    this.memoWorkOrder.Properties.ReadOnly = false;
                    this.lueWorkOrder.Properties.ReadOnly = true;               
                    break;
                case ControlState.New:
                    this.btnModify.Enabled = false;
                    this.btnSave.Enabled = true;
                    this.btnSave.Enabled = false;
                    this.btnAdd_wo_attr.Enabled = true;
                    this.btnDel_wo_attr.Enabled = true;
                    this.btnAdd_wo_material.Enabled = true;
                    //this.btnDel_wo_material.Enabled = true;
                    this.gvWoAttr.OptionsBehavior.Editable = true;
                    //this.gvWoMaterial.OptionsBehavior.Editable = true;
                    this.lueProductId.Properties.ReadOnly = false;
                    this.memoWorkOrder.Properties.ReadOnly = false;
                    this.lueWorkOrder.Properties.ReadOnly = false;
                    BindData2();
                    break;
            }
        }
       
        #endregion

        private void WorkOrderSetting_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);
            BindData2();            
            BindAllDataByKey("");

            lblMenu.Text = "基础数据 > 工单管理 > 工单属性设置";
        }

        private void BindData2()
        {
            DataSet dsBindAttr = workOrdersEntity.GetAllWorkOrderData();
            dtlueWo = dsBindAttr.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            dtlueWoNotExistProid = dtlueWo.Clone();
            DataRow[] drs = dtlueWo.Select(string.Format(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID + " is null"));
            foreach (DataRow dr in drs)
                dtlueWoNotExistProid.ImportRow(dr);

            lueWorkOrder.Properties.DisplayMember = POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER;
            lueWorkOrder.Properties.ValueMember = POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY;
            lueWorkOrder.Properties.DataSource = dtlueWoNotExistProid;
            lueWorkOrder.ItemIndex = -1;

            DataSet dsBindProid = porProductEntity.GetPorProductData(new Hashtable());
            DataTable dtProid = dsBindProid.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
            lueProductId.Properties.DisplayMember = POR_PRODUCT.FIELDS_PRODUCT_CODE;
            lueProductId.Properties.ValueMember = POR_PRODUCT.FIELDS_PRODUCT_KEY;
            lueProductId.Properties.DataSource = dtProid;
            lueProductId.ItemIndex = -1;
        }

       

        private bool IsValidData()
        {
            bool bl_bak = true;

            if (lueWorkOrder.EditValue == null || string.IsNullOrEmpty(lueWorkOrder.EditValue.ToString()))
            {
                MessageService.ShowMessage("工单号不能为空!");
                return false;
            }
            if (lueProductId.EditValue == null || string.IsNullOrEmpty(lueProductId.EditValue.ToString()))
            {
                MessageService.ShowMessage("产品ID号不能为空!");
                return false;
            }
            else
            {
                DataTable dtProductId = lueProductId.Properties.DataSource as DataTable;

                DataRow[] drs = dtProductId.Select(string.Format(POR_PRODUCT.FIELDS_PRODUCT_CODE + "='{0}' or " + POR_PRODUCT.FIELDS_PRODUCT_KEY + "='{0}'", lueProductId.EditValue.ToString()));
                if (drs != null)
                    lueProductId.EditValue = Convert.ToString(drs[0][POR_PRODUCT.FIELDS_PRODUCT_KEY]);
            }

            for (int i = 0; i < gvWoAttr.RowCount; i++)
            {
                string c1 = gvWoAttr.GetRowCellValue(i, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME).ToString();
                string c2 = gvWoAttr.GetRowCellValue(i, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE).ToString();
                if (string.IsNullOrEmpty(c1))
                {
                    MessageService.ShowMessage("工单属性名不能为空!");
                    bl_bak = false;
                    break;
                }

                if (string.IsNullOrEmpty(c2))
                {
                    MessageService.ShowMessage("工单属性值/SapNo不能为空!");
                    bl_bak = false;
                    break;
                }

                if (c1.Equals(WORKORDER_SETTING_ATTRIBUTE.IsMustInputModuleColorByCleanOpt))
                {
                    if (!c2.ToLower().Equals("true") && !c2.ToLower().Equals("false"))
                    {
                        MessageService.ShowMessage(string.Format("工单属性名称【{0}】值，必须为【true/false】,请确认!", c1));
                        bl_bak = false;
                        break;
                    }
                }
                if (c1.Equals(WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage))
                {
                    if (!c2.ToLower().Equals("true") && !c2.ToLower().Equals("false"))
                    {
                        MessageService.ShowMessage(string.Format("工单属性名称【{0}】值，必须为【true/false】,请确认!", c1));
                        bl_bak = false;
                        break;
                    }
                }
            }

            return bl_bak;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidData()) return;
            string workOrderKey = lueWorkOrder.EditValue.ToString();
            DataSet dsSave = new DataSet();
            DataTable dtWorkProID = new DataTable(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_FORUPDATE);
            dtWorkProID.Columns.Add(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY);
            dtWorkProID.Columns.Add(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID);
            dtWorkProID.Columns.Add(POR_WORK_ORDER_FIELDS.FIELD_COMMENTS);
            dtWorkProID.Columns.Add(POR_WORK_ORDER_FIELDS.FIELD_CREATOR);
            
            DataRow drNew01 = dtWorkProID.NewRow();
            drNew01[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY] = workOrderKey;
            drNew01[POR_WORK_ORDER_FIELDS.FIELD_PRO_ID] = lueProductId.EditValue.ToString();
            drNew01[POR_WORK_ORDER_FIELDS.FIELD_COMMENTS] = memoWorkOrder.Text.Trim();
            drNew01[POR_WORK_ORDER_FIELDS.FIELD_CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            dtWorkProID.Rows.Add(drNew01);
            dsSave.Merge(dtWorkProID, true, MissingSchemaAction.Add);

            //DataTable dtAttr = ((DataView)gvWoAttr.DataSource).Table;
            DataTable dtAttr = gcWoAttr.DataSource as DataTable;

            DataTable dtAttr_Update = dtAttr.GetChanges(DataRowState.Modified);
            DataTable dtAttr_Insert = dtAttr.GetChanges(DataRowState.Added);

            if (dtAttr_Update != null && dtAttr_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtAttr_Update.Clone();
                foreach (DataRow dr in dtAttr_Update.Rows)
                {
                    DataRow[] drUpdates = dtAttr.Select(string.Format(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY + "='{0}'", dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY] = workOrderKey;
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORUPDATE;
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }
            if (dtAttr_Insert != null && dtAttr_Insert.Rows.Count > 0)
            {
                if (!dtAttr_Insert.Columns.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY))
                    dtAttr_Insert.Columns.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY);
                if (!dtAttr_Insert.Columns.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_PRO_ID))
                    dtAttr_Insert.Columns.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_PRO_ID);
                if (!dtAttr_Insert.Columns.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG))
                    dtAttr_Insert.Columns.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG);

                DataTable dtInsert = dtAttr_Insert.Clone();
                foreach (DataRow dr in dtAttr_Insert.Rows)
                {
                    DataRow[] drInserts = dtAttr_Insert.Select(string.Format(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY + "='{0}'", dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }                           
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY] = workOrderKey;
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_PRO_ID] = lueProductId.EditValue.ToString();
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG] = 1;

                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORINSERT;
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

          

            DataSet dsReturn = workOrdersEntity.SaveWorkOrderAttrParam(dsSave);
            if (!string.IsNullOrEmpty(workOrdersEntity.ErrorMsg))
            {
                MessageService.ShowMessage(workOrdersEntity.ErrorMsg);
                return;
            }
            else
            {
                MessageService.ShowMessage("保存成功!");
                dtAttr.AcceptChanges();
                this.gcWoAttr.MainView = gvWoAttr;
                this.gcWoAttr.DataSource = null;
                this.gvWoAttr.FocusedRowHandle = -1;
                this.gcWoAttr.DataSource = dtAttr;

                this.State = ControlState.ReadOnly;
            }

        }

        private void btnAdd_wo_attr_Click(object sender, EventArgs e)
        {
            WorkOrderAttrSetting woas = new WorkOrderAttrSetting();
            woas.sType = "workattr";
            DataTable dt01 = gcWoAttr.DataSource as DataTable;
            DataTable dtTemp = dt01.Clone();
            DataRow[] drs = dt01.Select(string.Format(@"ATTRIBUTE_TYPE='0'"));
            foreach (DataRow dr in drs)
                dtTemp.ImportRow(dr);

            woas.dtCommon = dtTemp;
            //((DataView)gvWoAttr.DataSource).Table;
            if (DialogResult.OK == woas.ShowDialog())
            {
                string attribute_name = string.Empty;
                DataTable dtAttr = gcWoAttr.DataSource as DataTable;
                DataRow drNew = dtAttr.NewRow();
                attribute_name = Convert.ToString(woas.drCommon[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = woas.drCommon[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME] = attribute_name;
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);              
                if (attribute_name.Equals(WORKORDER_SETTING_ATTRIBUTE.IsMustInputModuleColorByCleanOpt))
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "true";
                if (attribute_name.Equals(WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage))
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "false";
                if (attribute_name.Equals(WORKORDER_SETTING_ATTRIBUTE.IsExperimentWo))
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "true";

                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_TYPE] = "0";
                dtAttr.Rows.Add(drNew);
                SortGvData(dtAttr);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            WorkOrderSettingForm wosf = new WorkOrderSettingForm();
            if (DialogResult.OK == wosf.ShowDialog())
            {                
                por_workno_key = wosf.por_work_order_key;
                BindAllDataByKey(por_workno_key);
                this.State = ControlState.ReadOnly;
            }
        }

        private void BindAllDataByKey(string workNoKey)
        {
            DataSet dsBindAll = workOrdersEntity.GetWorkOrderAndAttrParamByKey(workNoKey);
            DataTable dtWorkNo = dsBindAll.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            //lueWorkOrder.Properties.DataSource = dtlueWo;
            if (dtWorkNo != null && dtWorkNo.Rows.Count > 0)
            {
                string key=Convert.ToString( dtWorkNo.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]);
                DataRow[] drsNotExistProid = dtlueWoNotExistProid.Select(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY + string.Format("='{0}'", key));
                if (drsNotExistProid == null || drsNotExistProid.Length < 1)
                {
                    DataRow[] drswo= dtlueWo.Select(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY + string.Format("='{0}'", key));
                    dtlueWoNotExistProid.ImportRow(drswo[0]);
                    lueWorkOrder.Properties.DataSource = dtlueWoNotExistProid;
                }

                lueWorkOrder.EditValue = key;
                lueProductId.EditValue = dtWorkNo.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_PRO_ID];
                memoWorkOrder.Text = dtWorkNo.Rows[0][POR_WORK_ORDER_FIELDS.FIELD_COMMENTS].ToString();
            }
            //---------------------------------------------------------------------------------------
            dtWorkOrderAttr = dsBindAll.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];

            SortGvData(dtWorkOrderAttr); 
            //---------------------------------------------------------------------------------------
        }
      
        private void btnDel_wo_attr_Click(object sender, EventArgs e)
        {
            if (gvWoAttr.FocusedRowHandle < 0) return;

            //DataTable dtAttr = ((DataView)gvWoAttr.DataSource).Table;
            DataTable dtAttr = gcWoAttr.DataSource as DataTable;
            DataRow dr = gvWoAttr.GetFocusedDataRow();
            Hashtable hstable = new Hashtable();
            //hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY];
            //hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY] = dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY];
            hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY];

            DataSet dsReturn = workOrdersEntity.DelWorkAttrDataBy2Key(hstable);
            if (!string.IsNullOrEmpty(workOrdersEntity.ErrorMsg))
            {
                MessageService.ShowMessage(workOrdersEntity.ErrorMsg);
                return;
            }
            else
            {
                MessageService.ShowMessage("删除成功!");
                dtAttr.Rows.Remove(dr);
                this.gcWoAttr.MainView = gvWoAttr;
                this.gcWoAttr.DataSource = null;
                this.gvWoAttr.FocusedRowHandle = -1;
                this.gcWoAttr.DataSource = dtAttr;                
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {               
                if (viewContent.TitleName == "Default Title")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new WorkOrderSettingViewContent());
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (lueWorkOrder.EditValue == null) return;
            this.State = ControlState.Edit;
        }

        private void gvWoAttr_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
                attribute_key = gvWoAttr.GetFocusedRowCellValue(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY).ToString();
        }

        private void gvWoAttr_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(attribute_key.Trim()))
            {
                try
                {
                    DataTable dtWoAttr = gcWoAttr.DataSource as DataTable;
                    //((DataView)gvWoAttr.DataSource).Table;
                    DataRow[] drs = dtWoAttr.Select(string.Format(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + "='{0}'", attribute_key.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch //(Exception ex) 
                { }
            }   
        }
        private void btnAdd_wo_material_Click_1(object sender, EventArgs e)
        {
            WorkOrderAttrSetting woas = new WorkOrderAttrSetting();
            woas.sType = "workparams";
            DataTable dt01 = gcWoAttr.DataSource as DataTable;
            DataTable dtTemp = dt01.Clone();
            DataRow[] drs = dt01.Select(string.Format(@"ATTRIBUTE_TYPE='1'"));
            foreach (DataRow dr in drs)
                dtTemp.ImportRow(dr);

            woas.dtCommon = dtTemp;
            //((DataView)gvWoAttr.DataSource).Table;
            if (DialogResult.OK == woas.ShowDialog())
            {
                DataTable dtAttr = gcWoAttr.DataSource as DataTable;
                DataRow drNew = dtAttr.NewRow();
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = woas.drCommon[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME] = woas.drCommon[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_TYPE] = "1";
                dtAttr.Rows.Add(drNew);
                SortGvData(dtAttr);
            }   
        }

        private void SortGvData(DataTable dtBind)
        {
            this.gcWoAttr.MainView = gvWoAttr;
            this.gcWoAttr.DataSource = null;
            this.gvWoAttr.FocusedRowHandle = -1;
            this.gcWoAttr.DataSource = dtBind;
        }
       
       
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lueWorkOrder.EditValue != null && !string.IsNullOrEmpty(lueWorkOrder.EditValue.ToString()))
            {
                DataSet dsSave = new DataSet();
                string woKey = lueWorkOrder.EditValue.ToString();
                DataTable dt = new DataTable(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_FORUPDATE);
                dt.Columns.Add(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID);
                dt.Columns.Add(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY);

                DataRow drNew = dt.NewRow();
                drNew[POR_WORK_ORDER_FIELDS.FIELD_PRO_ID] = "";
                drNew[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY] = woKey;
                dt.Rows.Add(drNew);

                dsSave.Merge(dt, true, MissingSchemaAction.Add);

                DataSet dsReturn = workOrdersEntity.SaveWorkOrderAttrParam(dsSave);
                if (!string.IsNullOrEmpty(workOrdersEntity.ErrorMsg))
                {
                    MessageService.ShowMessage(workOrdersEntity.ErrorMsg);
                    return;
                }
                else
                {
                    MessageService.ShowMessage("删除成功!");
                    this.lueProductId.ItemIndex = -1;
                    this.lueWorkOrder.ItemIndex = -1;
                    this.memoWorkOrder.Text = "";
                    this.gcWoAttr.MainView = gvWoAttr;
                    this.gcWoAttr.DataSource = null;                                            
                    this.State = ControlState.New;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(por_workno_key)) return;

            BindAllDataByKey(por_workno_key);
            this.State = ControlState.ReadOnly;
        }

        
    }
}
