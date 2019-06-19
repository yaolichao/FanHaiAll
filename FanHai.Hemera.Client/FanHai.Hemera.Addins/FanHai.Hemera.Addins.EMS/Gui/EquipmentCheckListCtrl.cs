using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentCheckListCtrl : BaseUserCtrl
    {
        #region EquipmentCheckListEntity Object

        private EquipmentCheckListEntity equipmentCheckListEntity = new EquipmentCheckListEntity();

        #endregion

        #region Constructor

        public EquipmentCheckListCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentCheckListCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentCheckListCtrl_afterStateChanged);

            this.State = ControlState.Read;
        }

        private void EquipmentCheckListCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtCheckListName.Enabled = false;
                    this.cmbCheckListType.Enabled = false;
                    this.txtDescription.Enabled = false;

                    this.btnAdd.Enabled = false;
                    this.btnRemove.Enabled = false;

                    this.grvCheckListItemsList.OptionsBehavior.ReadOnly = true;

                    grvCheckList_FocusedRowChanged(this.grvCheckList, new FocusedRowChangedEventArgs(-1, this.grvCheckList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckListName.Enabled = false;
                    this.cmbCheckListType.Enabled = false;
                    this.txtDescription.Enabled = false;

                    this.btnAdd.Enabled = false;
                    this.btnRemove.Enabled = false;

                    this.grvCheckListItemsList.OptionsBehavior.ReadOnly = true;

                    grvCheckList_FocusedRowChanged(this.grvCheckList, new FocusedRowChangedEventArgs(-1, this.grvCheckList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckListName.Enabled = true;
                    this.cmbCheckListType.Enabled = true;
                    this.txtDescription.Enabled = true;

                    this.btnAdd.Enabled = true;
                    this.btnRemove.Enabled = true;

                    this.grvCheckListItemsList.OptionsBehavior.ReadOnly = false;

                    DataTable dataTable = this.grdCheckListItemsList.DataSource as DataTable;

                    if (dataTable != null)
                    {
                        dataTable.Rows.Clear();

                        dataTable.AcceptChanges();
                    }

                    this.txtCheckListName.Text = string.Empty;
                    this.cmbCheckListType.EditValue = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckListName.Enabled = true;
                    this.cmbCheckListType.Enabled = true;
                    this.txtDescription.Enabled = true;

                    this.btnAdd.Enabled = true;
                    this.btnRemove.Enabled = true;

                    this.grvCheckListItemsList.OptionsBehavior.ReadOnly = false;

                    grvCheckList_FocusedRowChanged(this.grvCheckList, new FocusedRowChangedEventArgs(-1, this.grvCheckList.FocusedRowHandle));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            #region Initial Check List Type ComboBox

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("CHECKLIST_TYPE");

            dataTable.Rows.Add("日检查");
            dataTable.Rows.Add("周检查");
            dataTable.Rows.Add("月检查");
            dataTable.Rows.Add("季检查");
            dataTable.Rows.Add("年检查");

            dataTable.AcceptChanges();

            this.cmbCheckListType.Properties.Columns.Clear();

            this.cmbCheckListType.Properties.Columns.Add(new LookUpColumnInfo("CHECKLIST_TYPE"));

            this.cmbCheckListType.Properties.ShowHeader = false;

            this.cmbCheckListType.Properties.DisplayMember = "CHECKLIST_TYPE";
            this.cmbCheckListType.Properties.ValueMember = "CHECKLIST_TYPE";

            this.cmbCheckListType.Properties.DataSource = dataTable;

            #endregion

            #region Initial Check List Item List Grid

            dataTable = GetEmptyCheckListItemsDataTable();

            ControlUtils.InitialGridView(this.grvCheckListItemsList, dataTable);

            GridColumn gridColumn = this.grvCheckListItemsList.Columns.ColumnByFieldName(EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD);

            if (gridColumn != null)
            {
                RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();

                textEdit.MaxLength = 50;

                gridColumn.ColumnEdit = textEdit;
            }

            gridColumn = this.grvCheckListItemsList.Columns.ColumnByFieldName(EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL);

            if (gridColumn != null)
            {
                RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);

                gridColumn.ColumnEdit = checkEdit;
            }

            #endregion

            #region Initial Check List Grid

            dataTable = EMS_CHECKLIST_FIELDS.CreateDataTable(true);

            ControlUtils.InitialGridView(this.grvCheckList, dataTable);

            #endregion
        }

        protected override void InitUIResourcesByCulture()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");

            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //this.tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            //this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //this.tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");
            this.lblTitle.Text = "设备检查表单";

            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY].Visible = false;
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE].Caption = "序号";
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE].Width = 35;
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE].OptionsColumn.ReadOnly = true;
            this.grvCheckListItemsList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].Caption = "检查项名称";
            this.grvCheckListItemsList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].OptionsColumn.ReadOnly = true;
            this.grvCheckListItemsList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE].Caption = "检查项类型";
            this.grvCheckListItemsList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE].OptionsColumn.ReadOnly = true;
            this.grvCheckListItemsList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvCheckListItemsList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION].OptionsColumn.ReadOnly = true;
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD].Caption = "参考标准";
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD].OptionsColumn.ReadOnly = false;
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL].Caption = "是否可选";
            this.grvCheckListItemsList.Columns[EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL].OptionsColumn.ReadOnly = false;

            this.grvCheckListItemsList.BestFitColumns();

            this.grvCheckList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvCheckList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

            this.grvCheckList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
            this.grvCheckList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME].Caption = "检查表单名称";
            this.grvCheckList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE].Caption = "检查表单类型";
            this.grvCheckList.Columns[EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION].Caption = "描述";

            this.grvCheckList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvCheckList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvCheckList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvCheckList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvCheckList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvCheckList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvCheckList.BestFitColumns();
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentCheckListEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentCheckListEntity.CheckListKey =  CommonUtils.GenerateNewKey(0);

                    this.equipmentCheckListEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentCheckListEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentCheckListEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentCheckListEntity.CheckListKey = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY).ToString();
                    this.equipmentCheckListEntity.CheckListName = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME).ToString();
                    this.equipmentCheckListEntity.CheckListType = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE).ToString();
                    this.equipmentCheckListEntity.Description = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION).ToString();

                    this.equipmentCheckListEntity.Editor = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentCheckListEntity.EditTimeZone = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentCheckListEntity.EditTime = this.grvCheckList.GetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentCheckListEntity.IsInitializeFinished = true;

            this.equipmentCheckListEntity.CheckListName = this.txtCheckListName.Text.Trim();
            this.equipmentCheckListEntity.CheckListType = this.cmbCheckListType.Text;
            this.equipmentCheckListEntity.Description = this.txtDescription.Text.Trim();
            this.equipmentCheckListEntity.CheckListItemsData = this.grdCheckListItemsList.DataSource as DataTable;
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    this.grvCheckList.AddNewRow();

                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY, this.equipmentCheckListEntity.CheckListKey);
                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME, this.equipmentCheckListEntity.CheckListName);
                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE, this.equipmentCheckListEntity.CheckListType);
                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION, this.equipmentCheckListEntity.Description);

                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CREATOR, this.equipmentCheckListEntity.Creator);
                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY, this.equipmentCheckListEntity.CreateTimeZone);
                    this.grvCheckList.SetFocusedRowCellValue(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME, this.equipmentCheckListEntity.CreateTime);

                    this.grvCheckList.UpdateCurrentRow();
                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentCheckListEntity.DirtyList)
                    {
                        this.grvCheckList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);
                    }

                    this.grvCheckList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvCheckList.DeleteRow(this.grvCheckList.FocusedRowHandle);

                    this.grvCheckList.UpdateCurrentRow();
                    break;
            }

            this.grvCheckList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Empty Check List Items Data Table
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-18 10:41:00
        private DataTable GetEmptyCheckListItemsDataTable()
        {
            DataTable dataTable = new DataTable(EMS_CHECKLIST_ITEM_FIELDS.DATABASE_TABLE_NAME);

            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKLIST_KEY);
            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY);
            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE, typeof(int));
            dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME);
            dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE);
            dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION);
            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD);
            dataTable.Columns.Add(EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL, typeof(int));

            return dataTable;
        }

        /// <summary>
        /// Load Check List Data
        /// </summary>
        /// <param name="checkListName"></param>
        /// Owner:Andy Gao 2011-07-14 12:41:00
        private void LoadCheckListData(string checkListName)
        {
            string msg;

            int pageNo, pageSize, pages, records;

            this.paginationCheckList.GetPaginationProperties(out pageNo, out pageSize);

            DataTable dataTable = this.equipmentCheckListEntity.LoadCheckListData(checkListName, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationCheckList.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdCheckList.DataSource = dataTable;
            }
            else
            {
                this.grdCheckList.DataSource = EMS_CHECKLIST_FIELDS.CreateDataTable(true);

                MessageService.ShowError(msg);
            }

            this.grvCheckList.BestFitColumns();

            State = ControlState.Read;
        }

        /// <summary>
        /// Load Check List Items Data
        /// </summary>
        /// <param name="checkListkey"></param>
        /// Owner:Andy Gao 2011-07-15 15:35:10
        private void LoadCheckListItemsData(string checkListkey)
        {
            if (string.IsNullOrEmpty(checkListkey))
            {
                this.grdCheckListItemsList.DataSource = GetEmptyCheckListItemsDataTable();
            }
            else
            {
                string msg;

                DataTable dataTable = this.equipmentCheckListEntity.LoadCheckListItemsData(checkListkey, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    this.grdCheckListItemsList.DataSource = dataTable;
                }
                else
                {
                    MessageService.ShowError(msg);

                    this.grdCheckListItemsList.DataSource = GetEmptyCheckListItemsDataTable();
                }
            }

            this.grvCheckListItemsList.BestFitColumns();
        }

        #endregion

        #region Component Events

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadCheckListData(this.txtQueryValue.Text.Trim());
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadCheckListData(this.txtQueryValue.Text.Trim());
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            State = ControlState.New;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.txtCheckListName.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入检查表单名称!");

                this.txtCheckListName.Focus();

                return;
            }

            if (this.cmbCheckListType.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择检查表单类型!");

                this.cmbCheckListType.Focus();

                return;
            }

            if (this.grvCheckListItemsList.RowCount == 0)
            {
                MessageService.ShowMessage("请增加检查表单项!");

                this.btnAdd.Focus();

                return;
            }

            MapControlsToEntity();

            if ((this.grvCheckListItemsList.FocusedColumn.FieldName == EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD || this.grvCheckListItemsList.FocusedColumn.FieldName == EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL) && 
                this.grvCheckListItemsList.State == GridState.Editing && this.grvCheckListItemsList.IsEditorFocused && this.grvCheckListItemsList.EditingValueModified)
            {
                this.grvCheckListItemsList.SetFocusedRowCellValue(this.grvCheckListItemsList.FocusedColumn, this.grvCheckListItemsList.EditingValue);
                this.grvCheckListItemsList.UpdateCurrentRow();
            }

            this.equipmentCheckListEntity.IsCheckListItemsModified = false;

            DataTable dataTable = this.grdCheckListItemsList.DataSource as DataTable;

            if (dataTable != null)
            {
                DataTable changedDataTable = dataTable.GetChanges(DataRowState.Added | DataRowState.Modified | DataRowState.Deleted);

                if (changedDataTable != null && changedDataTable.Rows.Count > 0)
                {
                    this.equipmentCheckListEntity.IsCheckListItemsModified = true;
                }

                dataTable.AcceptChanges();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    dataRow.SetAdded();
                }
            }

            if (State == ControlState.New)
            {
                if (this.equipmentCheckListEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadCheckListData(this.txtQueryValue.Text.Trim());
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentCheckListEntity.IsDirty || this.equipmentCheckListEntity.IsCheckListItemsModified)
                {
                    if (this.equipmentCheckListEntity.Update())
                    {
                        MapEntityToControls();

                        MessageService.ShowMessage("更新数据成功!");

                        State = ControlState.Read;
                    }
                }
                else
                {
                    MessageService.ShowMessage("数据未修改!");
                }
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            State = ControlState.Read;
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (this.grvCheckList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除检查表单!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该检查表单吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentCheckListEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadCheckListData(this.txtQueryValue.Text.Trim());
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentCheckItems", true))
            {
                if (queryDialog.ShowDialog() == DialogResult.OK)
                {
                    if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                    {
                        DataTable dataTable = this.grdCheckListItemsList.DataSource as DataTable;

                        if (dataTable != null)
                        {
                            foreach (DataRow selectedRow in queryDialog.SelectedData)
                            {
                                string checkItemName = selectedRow[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].ToString();

                                if (dataTable.Select(string.Format("{0} = '{1}'", EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, checkItemName)).Length <= 0)
                                {
                                    DataRow dataRow = dataTable.NewRow();

                                    dataRow[EMS_CHECKLIST_ITEM_FIELDS.FIELD_CHECKITEM_KEY] = selectedRow[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY];
                                    dataRow[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE] = dataTable.Rows.Count + 1;
                                    dataRow[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME] = selectedRow[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME];
                                    dataRow[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE] = selectedRow[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE];
                                    dataRow[EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION] = selectedRow[EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION];
                                    dataRow[EMS_CHECKLIST_ITEM_FIELDS.FIELD_STANDARD] = string.Empty;
                                    dataRow[EMS_CHECKLIST_ITEM_FIELDS.FIELD_OPTIONAL] = 0;

                                    dataTable.Rows.Add(dataRow);
                                }
                            }

                            this.grvCheckListItemsList.BestFitColumns();
                        }
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.grvCheckListItemsList.FocusedRowHandle >= 0)
            {
                if (MessageService.AskQuestion("确认要移除该检查项吗?"))
                {
                    this.grvCheckListItemsList.DeleteRow(this.grvCheckListItemsList.FocusedRowHandle);

                    if (this.grvCheckListItemsList.RowCount > 0)
                    {
                        DataTable dataTable = this.grdCheckListItemsList.DataSource as DataTable;

                        int rowIndex = 1;

                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            if (dataRow.RowState != DataRowState.Deleted)
                            {
                                dataRow[EMS_CHECKLIST_ITEM_FIELDS.FIELD_SEQUENCE] = rowIndex++;
                            }
                        }

                        this.grvCheckListItemsList.BestFitColumns();
                    }
                }
            }
        }

        private void grvCheckList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtCheckListName.Text = this.grvCheckList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME).ToString();
                this.cmbCheckListType.EditValue = this.grvCheckList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE).ToString();
                this.txtDescription.Text = this.grvCheckList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION).ToString();

                string checkListKey = this.grvCheckList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY).ToString();

                LoadCheckListItemsData(checkListKey);
            }
            else
            {
                this.txtCheckListName.Text = string.Empty;
                this.cmbCheckListType.EditValue = string.Empty;
                this.txtDescription.Text = string.Empty;

                LoadCheckListItemsData(string.Empty);
            }
        }

        private void grdCheckList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvCheckList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                State = ControlState.Edit;
            }
        }

        private void paginationCheckList_DataPaging()
        {
            LoadCheckListData(this.txtQueryValue.Text.Trim());
        }

        #endregion
    }
}
