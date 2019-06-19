using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Controls.Common;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicDataSetting : BaseUserCtrl
    {
        public BaseAttributeCategory baseAttributeCategoryEntity = new BaseAttributeCategory();
        private bool isMultiSelect = false;
        public DataRow[] SelectedData = new DataRow[] { };

        public string categoryName = string.Empty;
        public BasicDataSetting()
        {
            InitializeComponent();
        }
        private void BasicDataSetting_Load(object sender, EventArgs e)
        {
            lblMenu.Text = "基础数据>配置管理>参数配置";
            //加载所有的基础数据类型数据信息列表
            LoadBasicDataTree();
        }
        /// <summary>
        /// 获取基础数据类型数据列表
        /// </summary>
        private void LoadBasicDataTree()
        {
            this.isMultiSelect = false;
            InitialFormControls();
            LoadData();
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            DataTable dataTable;
            DataColumn dc;
            grvDataList.Columns[1].OptionsColumn.AllowEdit = false;
            grvDataList.Columns[2].OptionsColumn.AllowEdit = false;
            DataSet dataDS = new DataSet();
            try
            {
                dataDS = baseAttributeCategoryEntity.GetBaseCategory();
                if (baseAttributeCategoryEntity.ErrorMsg == "")
                {
                    dataTable = dataDS.Tables[0];
                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = string.Empty;
                    dc.DefaultValue = false;

                    this.grdDataList.DataSource = dataTable;

                    this.grvDataList.BestFitColumns();
                }
                else
                {
                    MessageBox.Show(baseAttributeCategoryEntity.ErrorMsg);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        /// <summary>
        /// 添加对应的数据列
        /// </summary>
        private void InitialFormControls()
        {
            GridViewHelper.SetGridView(grvDataList);
            //grvDataList.IndicatorWidth = 60;
            grvDataList.OptionsView.ShowIndicator = false;
            DataTable dataTable;
            DataColumn dc;
            GridColumn gridColumn;

            dataTable = new DataTable(BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME);

            dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));
            dc.Caption = "选择";
            dc.ReadOnly = false;

            dc = dataTable.Columns.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY);
            dc.Caption = "基础数据表主键";
            dc.ReadOnly = true;

            dc = dataTable.Columns.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME);
            dc.Caption = "基础数据表类型";
            dc.ReadOnly = true;

            ControlUtils.InitialGridView(this.grvDataList, dataTable);

            this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
            this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.grvDataList.Columns[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY].Visible = false;

            gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

            if (gridColumn != null)
            {
                RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                gridColumn.ColumnEdit = checkEdit;

                StyleFormatCondition checkCondition = new StyleFormatCondition();

                checkCondition.Appearance.BackColor = Color.Green;
                checkCondition.Appearance.Options.UseBackColor = true;
                checkCondition.ApplyToRow = true;
                checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                checkCondition.Value1 = true;
                checkCondition.Column = gridColumn;

                this.grvDataList.FormatConditions.Add(checkCondition);
            }
        }
        private void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.isMultiSelect)
            {
                DataTable dataTable = this.grdDataList.DataSource as DataTable;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                    foreach (DataRow selectedRow in selectedRows)
                    {
                        selectedRow[COMMON_FIELDS.FIELD_COMMON_CHECKED] = false;
                    }
                }
            }
            if (this.grvDataList.EditingValueModified)
            {
                this.grvDataList.SetFocusedValue(this.grvDataList.EditingValue);
                this.grvDataList.UpdateCurrentRow();
            }

        }
        /// <summary>
        /// 编辑数据表的列信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            DataTable dataTable = this.grdDataList.DataSource as DataTable;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                if (selectedRows.Length > 0)
                {
                    this.SelectedData = selectedRows;
                }
                else
                {
                    MessageService.ShowMessage("请选择数据!");
                }
                if (SelectedData != null && SelectedData.Length > 0)
                {
                    DataRow selectedRow = SelectedData[0];

                    TreeNode tn = new TreeNode();
                    tn.Tag = selectedRow[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY].ToString().Trim();
                    tn.Text = selectedRow[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME].ToString().Trim();
                    BasicSettingsDetail basicSettingsDetail = new BasicSettingsDetail(tn);
                    basicSettingsDetail.Dock = DockStyle.Fill;
                    panel1.Controls.Clear();
                    panel1.Controls.Add(basicSettingsDetail);
                }
            }
            else
            {
                MessageService.ShowMessage("请查询数据!");
            }
        }
        /// <summary>
        /// 编辑数据表的数据信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditData_Click(object sender, EventArgs e)
        {
            DataTable dataTable = this.grdDataList.DataSource as DataTable;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                if (selectedRows.Length > 0)
                {
                    this.SelectedData = selectedRows;
                }
                else
                {
                    MessageService.ShowMessage("请选择数据!");
                }
                if (SelectedData != null && SelectedData.Length > 0)
                {
                    DataRow selectedRow = SelectedData[0];

                    TreeNode tn = new TreeNode();
                    tn.Tag = selectedRow[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY].ToString().Trim();
                    tn.Text = selectedRow[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME].ToString().Trim();
                    BasicSettingsDatDetail basicSettingsDatDetail = new BasicSettingsDatDetail(tn);
                    basicSettingsDatDetail.Dock = DockStyle.Fill;
                    panel1.Controls.Clear();
                    panel1.Controls.Add(basicSettingsDatDetail);
                }
            }
            else
            {
                MessageService.ShowMessage("请查询数据!");
            }
        }
        /// <summary>
        /// 添加新的数据类型信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            DataSet dataDsCategoryKey = new DataSet();
            //show add data type window
            BasicDataTypeDeal basicDataTypeDeal = new BasicDataTypeDeal("add");
            basicDataTypeDeal.ShowDialog();

            #region insert new group name to table and add node to treeview
            if (basicDataTypeDeal.typeName != "")
            {
                //get category name
                categoryName = basicDataTypeDeal.typeName;
                try
                {
                    //set value to entity
                    MapValueToEntity();

                    //save data  
                    baseAttributeCategoryEntity.SaveBaseCategory();
                    //check result
                    if (baseAttributeCategoryEntity.ErrorMsg == "")
                    {
                        LoadData();
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDataAddSuccessfully}"));
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse(baseAttributeCategoryEntity.ErrorMsg));
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            #endregion
        }
        private void MapValueToEntity()
        {
            //set category key
            baseAttributeCategoryEntity.CategoryKey = CommonUtils.GenerateNewKey(0);
            //set category name
            baseAttributeCategoryEntity.CategoryName = categoryName;
            //set descriptions
            baseAttributeCategoryEntity.Descriptions = "";
            //set creator
            baseAttributeCategoryEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //set create time
            baseAttributeCategoryEntity.CreateTime = "";
            //set create time zone
            baseAttributeCategoryEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
        }
        /// <summary>
        /// 删除数据类型信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            DataTable dataTable = this.grdDataList.DataSource as DataTable;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                if (selectedRows.Length > 0)
                {
                    this.SelectedData = selectedRows;
                }
                else
                {
                    MessageService.ShowMessage("请选择数据!");
                }
                if (SelectedData != null && SelectedData.Length > 0)
                {
                    DataRow selectedRow = SelectedData[0];
                    #region insert new group name to table and add node to treeview
                    if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDeleteTableTypeText}"), StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDeleteTableTypeCaption}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        try
                        {
                            //set value to entity
                            baseAttributeCategoryEntity.CategoryKey = selectedRow[BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY].ToString().Trim();

                            //save data  
                            baseAttributeCategoryEntity.DeleteBaseCategory();
                            //check result
                            if (baseAttributeCategoryEntity.ErrorMsg == "")
                            {
                                //重新加载数据
                                LoadData();
                                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDeleteTableTypeSuccessfully}"));

                            }
                            else
                            {
                                MessageBox.Show(StringParser.Parse(baseAttributeCategoryEntity.ErrorMsg));
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }

                    }
                    #endregion
                }
            }
        }
    }
}
