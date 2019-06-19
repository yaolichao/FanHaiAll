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
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentCheckItemsCtrl : BaseUserCtrl
    {
        #region EquipmentCheckItemEntity Object

        private EquipmentCheckItemEntity equipmentCheckItemEntity = new EquipmentCheckItemEntity();

        #endregion

        #region Constructor

        public EquipmentCheckItemsCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentCheckItemsCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentCheckItemsCtrl_afterStateChanged);

            this.State = ControlState.Read;
        }

        private void EquipmentCheckItemsCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtCheckItemName.Enabled = false;
                    this.cmbCheckItemType.Enabled = false;
                    this.txtDescription.Enabled = false;

                    grvCheckItemList_FocusedRowChanged(this.grvCheckItemList, new FocusedRowChangedEventArgs(-1, this.grvCheckItemList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckItemName.Enabled = false;
                    this.cmbCheckItemType.Enabled = false;
                    this.txtDescription.Enabled = false;

                    grvCheckItemList_FocusedRowChanged(this.grvCheckItemList, new FocusedRowChangedEventArgs(-1, this.grvCheckItemList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckItemName.Enabled = true;
                    this.cmbCheckItemType.Enabled = true;
                    this.txtDescription.Enabled = true;

                    this.txtCheckItemName.Text = string.Empty;
                    this.cmbCheckItemType.EditValue = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtCheckItemName.Enabled = true;
                    this.cmbCheckItemType.Enabled = true;
                    this.txtDescription.Enabled = true;

                    grvCheckItemList_FocusedRowChanged(this.grvCheckItemList, new FocusedRowChangedEventArgs(-1, this.grvCheckItemList.FocusedRowHandle));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            #region Initial Check Item Type ComboBox

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("CHECKITEM_TYPE");

            dataTable.Rows.Add("DATA");
            dataTable.Rows.Add("STRING");
            dataTable.Rows.Add("Y/N");

            dataTable.AcceptChanges();

            this.cmbCheckItemType.Properties.Columns.Clear();

            this.cmbCheckItemType.Properties.Columns.Add(new LookUpColumnInfo("CHECKITEM_TYPE"));

            this.cmbCheckItemType.Properties.ShowHeader = false;

            this.cmbCheckItemType.Properties.DisplayMember = "CHECKITEM_TYPE";
            this.cmbCheckItemType.Properties.ValueMember = "CHECKITEM_TYPE";

            this.cmbCheckItemType.Properties.DataSource = dataTable;

            #endregion

            #region Initial Check Item Grid

            dataTable = EMS_CHECKITEMS_FIELDS.CreateDataTable(true);

            ControlUtils.InitialGridView(this.grvCheckItemList, dataTable);

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
            this.lblTitle.Text = "设备检查表单项";

            this.grvCheckItemList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvCheckItemList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY].Visible = false;
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME].Caption = "检查项名称";
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE].Caption = "检查项类型";
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";

            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvCheckItemList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvCheckItemList.BestFitColumns();
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentCheckItemEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentCheckItemEntity.CheckItemKey =  CommonUtils.GenerateNewKey(0);

                    this.equipmentCheckItemEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentCheckItemEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentCheckItemEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentCheckItemEntity.CheckItemKey = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY).ToString();
                    this.equipmentCheckItemEntity.CheckItemName = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME).ToString();
                    this.equipmentCheckItemEntity.CheckItemType = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE).ToString();
                    this.equipmentCheckItemEntity.Description = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION).ToString();

                    this.equipmentCheckItemEntity.Editor = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentCheckItemEntity.EditTimeZone = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentCheckItemEntity.EditTime = this.grvCheckItemList.GetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentCheckItemEntity.IsInitializeFinished = true;

            this.equipmentCheckItemEntity.CheckItemName = this.txtCheckItemName.Text.Trim();
            this.equipmentCheckItemEntity.CheckItemType = this.cmbCheckItemType.Text;
            this.equipmentCheckItemEntity.Description = this.txtDescription.Text.Trim();
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    this.grvCheckItemList.AddNewRow();

                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY, this.equipmentCheckItemEntity.CheckItemKey);
                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME, this.equipmentCheckItemEntity.CheckItemName);
                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE, this.equipmentCheckItemEntity.CheckItemType);
                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION, this.equipmentCheckItemEntity.Description);

                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CREATOR, this.equipmentCheckItemEntity.Creator);
                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, this.equipmentCheckItemEntity.CreateTimeZone);
                    this.grvCheckItemList.SetFocusedRowCellValue(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME, this.equipmentCheckItemEntity.CreateTime);

                    this.grvCheckItemList.UpdateCurrentRow();
                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentCheckItemEntity.DirtyList)
                    {
                        this.grvCheckItemList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);
                    }

                    this.grvCheckItemList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvCheckItemList.DeleteRow(this.grvCheckItemList.FocusedRowHandle);

                    this.grvCheckItemList.UpdateCurrentRow();
                    break;
            }

            this.grvCheckItemList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Check Items Data
        /// </summary>
        /// <param name="checkItemName"></param>
        /// Owner:Andy Gao 2011-07-12 08:48:53
        private void LoadCheckItemsData(string checkItemName)
        {
            string msg;

            int pageNo, pageSize, pages, records;

            this.paginationCheckItems.GetPaginationProperties(out pageNo, out pageSize);

            DataTable dataTable = this.equipmentCheckItemEntity.LoadCheckItemsData(checkItemName, pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationCheckItems.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdCheckItemList.DataSource = dataTable;
            }
            else
            {
                this.grdCheckItemList.DataSource = EMS_CHECKITEMS_FIELDS.CreateDataTable(true);

                MessageService.ShowError(msg);
            }

            this.grvCheckItemList.BestFitColumns();

            State = ControlState.Read;
        }

        #endregion

        #region Component Events

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadCheckItemsData(this.txtQueryValue.Text.Trim());
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadCheckItemsData(this.txtQueryValue.Text.Trim());
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            State = ControlState.New;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.txtCheckItemName.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入检查项名称!");

                this.txtCheckItemName.Focus();

                return;
            }

            if (this.cmbCheckItemType.Text.Length == 0)
            {
                MessageService.ShowMessage("请选择检查项类型!");

                this.cmbCheckItemType.Focus();

                return;
            }

            MapControlsToEntity();

            if (State == ControlState.New)
            {
                if (this.equipmentCheckItemEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadCheckItemsData(this.txtQueryValue.Text.Trim());
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentCheckItemEntity.IsDirty)
                {
                    if (this.equipmentCheckItemEntity.Update())
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
            if (this.grvCheckItemList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除检查项!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该检查项吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentCheckItemEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadCheckItemsData(this.txtQueryValue.Text.Trim());
                }
            }
        }

        private void grvCheckItemList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtCheckItemName.Text = this.grvCheckItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME).ToString();
                this.cmbCheckItemType.EditValue = this.grvCheckItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE).ToString();
                this.txtDescription.Text = this.grvCheckItemList.GetRowCellValue(e.FocusedRowHandle, EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION).ToString();
            }
            else
            {
                this.txtCheckItemName.Text = string.Empty;
                this.cmbCheckItemType.EditValue = string.Empty;
                this.txtDescription.Text = string.Empty;
            }
        }

        private void grdCheckItemList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvCheckItemList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                State = ControlState.Edit;
            }
        }

        private void paginationCheckItems_DataPaging()
        {
            LoadCheckItemsData(this.txtQueryValue.Text.Trim());
        }

        #endregion
    }
}
