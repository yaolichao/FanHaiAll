using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class WorkOrderAttrSetting : BaseDialog
    {
        public DataRow drCommon = null;
        public bool isEdit = false;
        public string sType = string.Empty, rowkey = string.Empty;
        public DataTable dtCommon = null;

        private DataTable dtWorkAttr = new DataTable();
        private CrmAttribute crmAttributeEntity = new CrmAttribute();
        private Param _paramEntity = new Param();

        public WorkOrderAttrSetting()
        {
            InitializeComponent();
        }

        private void WorkOrderSettingForm_Load(object sender, EventArgs e)
        {
            if (sType.Trim().Equals("workattr"))
            {
                BindDataAttr();
                BindGvAttrData();
            }
            else
            {
                BindDataParam();
                BindGvParamData();
            }
        }

        private void BindGvAttrData()
        {
            crmAttributeEntity.MyCategory = "Uda_work_order";
            DataSet dsWorkAttr = crmAttributeEntity.GetAttributsColumnsForSomeCategory();
             dtWorkAttr = dsWorkAttr.Tables[0];
            dtWorkAttr.TableName = BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME;
            if (dtCommon != null && dtCommon.Rows.Count > 0)
            {
                foreach (DataRow drDel in dtCommon.Rows)
                {
                    DataRow[] drs = dtWorkAttr.Select(string.Format(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY + "='{0}'", drDel[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString()));
                    if (drs.Length > 0)
                    {
                        foreach (DataRow dr in drs)
                        {
                            dtWorkAttr.Rows.Remove(dr);
                        }
                    }
                }
            }
            if (dtWorkAttr.Rows.Count > 0)
            {
                DataView dv = dtWorkAttr.DefaultView;
                dv.Sort = BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME + " asc";
                this.grcDataList.DataSource = dv.ToTable();
            }
           
        }

        private void BindGvParamData()
        {
            DataSet dsParams = _paramEntity.GetBaseParamsByCategory();
            if (!string.IsNullOrEmpty(_paramEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_paramEntity.ErrorMsg);
                return;
            }
            DataTable dtParams = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];

            if (dtCommon != null && dtCommon.Rows.Count > 0)
            {
                foreach (DataRow drDel in dtCommon.Rows)
                {
                    DataRow[] drs = dtParams.Select(string.Format(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY + "='{0}'", drDel[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString()));
                    if (drs.Length > 0)
                    {
                        foreach (DataRow dr in drs)
                        {
                            dtParams.Rows.Remove(dr);
                        }
                    }
                }
            }

            this.grcDataList.DataSource = dtParams;
        }

        private void BindDataAttr()
        {
            DataTable dataTable;
            DataColumn dc;
            #region
            dataTable = new DataTable(BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME);

            dc = dataTable.Columns.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, typeof(string));
            dc.Caption = "属性主键";
            dc.ReadOnly = false;


            dc = dataTable.Columns.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME);

            dc.Caption = "属性名";
            dc.ReadOnly = false;


            dc = dataTable.Columns.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS);
            dc.Caption = "属性描述";
            dc.ReadOnly = false;

            ControlUtils.InitialGridView(this.grvDataList, dataTable);
            this.grvDataList.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].Visible = false;

            #endregion
        }
        private void BindDataParam()
        {
            DataTable dataTable;
            DataColumn dc;
            #region
            dataTable = new DataTable(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME);

            dc = dataTable.Columns.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY, typeof(string));
            dc.Caption = "物料主键";
            dc.ReadOnly = false;

            dc = dataTable.Columns.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME);
            dc.Caption = "物料名称";
            dc.ReadOnly = false;

            dc = dataTable.Columns.Add(BASE_PARAMETER_FIELDS.FIELD_DESCRIPTIONS);
            dc.Caption = "物料描述";
            dc.ReadOnly = false;

            ControlUtils.InitialGridView(this.grvDataList, dataTable);
            this.grvDataList.Columns[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].Visible = false;            
            
            #endregion
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void grvDataList_DoubleClick(object sender, EventArgs e)
        {
            GetRowData();
        }

        private void GetRowData()
        {
            if (grvDataList.RowCount < 1) return;
            if (grvDataList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择一笔数据!");
                return;
            }

            DataRow dr = this.grvDataList.GetFocusedDataRow();
            if (sType.Trim().Equals("workattr"))
            {
                rowkey = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString();
                drCommon = dr;
            }
            else
            {
                rowkey = dr[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString();
                drCommon = dr;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}