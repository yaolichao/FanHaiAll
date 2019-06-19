using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using FanHai.Gui.Core;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace FanHai.Hemera.Addins.SPC.Gui
{
    public partial class AbnormalControlPlan : Form
    {
        public string abnormals = string.Empty;
        //private string _strAbnormals = string.Empty;
        private List<string> lst = new List<string>();
        SpcEntity spcEntity = new SpcEntity();
        bool isAddColorField = false;
        public AbnormalControlPlan()
        {
            InitializeComponent();
        }

        public AbnormalControlPlan(string strAbnormals)
        {
            //_strAbnormals = strAbnormals;
            if (!string.IsNullOrEmpty(strAbnormals.Trim()))
            {
                lst = strAbnormals.Split(',').ToList<string>();
            }

            InitializeComponent();
        }

        private void AbnormalControlPlan_Load(object sender, EventArgs e)
        {
            InitialData();
        }
        private void InitialData()
        {
            try
            {
                DataSet dataSet = spcEntity.GetAbnormalRule();        

                if (spcEntity.ErrorMsg.Length > 0)
                {
                    MessageService.ShowError("get spc AbnormalRule error:" + spcEntity.ErrorMsg);
                }
                else
                {
                    DataTable dtMain = dataSet.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];
                    dtMain.Columns.Add("isSelected", typeof(bool));

                    gridView1.Columns[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR].Visible = false;

                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables.Contains(EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME))
                    {
                        this.gcMainAbnormal.DataSource = null;
                        this.gcMainAbnormal.DataSource = dataSet.Relations[0].ParentTable;

                        for (int i = 0; i < gridView1.DataRowCount; i++)
                        {
                            string _code = gridView1.GetRowCellValue(i, EDC_ABNORMAL_FIELDS.FIELD_ARULECODE).ToString();
                            if (lst.Contains(_code))
                                gridView1.SetRowCellValue(i, "isSelected", true);
                        }

                        if (!isAddColorField)
                        {
                            GridColumn unboundColumn = gridView1.Columns.AddField("Color");
                            unboundColumn.VisibleIndex = gridView1.Columns.Count;
                            unboundColumn.UnboundType = DevExpress.Data.UnboundColumnType.Object;
                            RepositoryItemColorEdit ce = new RepositoryItemColorEdit();
                            ce.ShowCustomColors = false;
                            unboundColumn.ColumnEdit = ce;
                            isAddColorField = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
        }
      

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool isOk = false;
            abnormals = string.Empty;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                try
                {
                    bool isSelected = (bool)gridView1.GetRowCellValue(i, "isSelected");
                    if (isSelected)
                    {
                        abnormals += gridView1.GetRowCellValue(i, EDC_ABNORMAL_FIELDS.FIELD_ARULECODE).ToString() + ',';
                        isOk = true;
                    }
                }
                catch { }
            }
            if (!isOk)
            {
                MessageService.ShowMessage("未选择数据,请确认!");
                return;
            }
            else
            {
                abnormals = abnormals.TrimEnd(',');
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
                  
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GridView view = sender as GridView;
            DataView dv = view.DataSource as DataView;
            if (e.IsGetData)
                e.Value = GetColorFromString(dv[e.ListSourceRowIndex][EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR].ToString());
            else
                dv[e.ListSourceRowIndex][EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR] = ((Color)e.Value).Name;
        }

        Color GetColorFromString(string colorString)
        {
            Color color = Color.Empty;
            ColorConverter converter = new ColorConverter();
            try
            {
                color = (Color)converter.ConvertFromString(colorString);
            }
            catch
            { }
            return color;
        }
    }
}
