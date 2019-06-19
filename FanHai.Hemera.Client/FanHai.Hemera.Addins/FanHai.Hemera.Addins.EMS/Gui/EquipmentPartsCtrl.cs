using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentPartsCtrl : BaseUserCtrl
    {
        #region EquipmentPartEntity Object

        private EquipmentPartEntity equipmentPartEntity = new EquipmentPartEntity();

        #endregion

        #region Constructor

        public EquipmentPartsCtrl()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentPartsCtrl_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(EquipmentPartsCtrl_afterStateChanged);

            this.State = ControlState.Read;
        }

        private void EquipmentPartsCtrl_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtPartName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtPartType.Enabled = false;
                    this.txtPartMode.Enabled = false;
                    this.txtPartUnit.Enabled = false;

                    grvPartList_FocusedRowChanged(this.grvPartList, new FocusedRowChangedEventArgs(-1, this.grvPartList.FocusedRowHandle));
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtPartName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtPartType.Enabled = false;
                    this.txtPartMode.Enabled = false;
                    this.txtPartUnit.Enabled = false;

                    grvPartList_FocusedRowChanged(this.grvPartList, new FocusedRowChangedEventArgs(-1, this.grvPartList.FocusedRowHandle));
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtPartName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.txtPartType.Enabled = true;
                    this.txtPartMode.Enabled = true;
                    this.txtPartUnit.Enabled = true;

                    this.txtPartName.Text = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    this.txtPartType.Text = string.Empty;
                    this.txtPartMode.Text = string.Empty;
                    this.txtPartUnit.Text = string.Empty;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtPartName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.txtPartType.Enabled = true;
                    this.txtPartMode.Enabled = true;
                    this.txtPartUnit.Enabled = true;

                    grvPartList_FocusedRowChanged(this.grvPartList, new FocusedRowChangedEventArgs(-1, this.grvPartList.FocusedRowHandle));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            #region Initial Parts Grid

            ControlUtils.InitialGridView(this.grvPartList, EMS_EQUIPMENT_PARTS_FIELDS.CreateDataTable(true));

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
            this.lblTitle.Text = "设备备件";

            this.grvPartList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grvPartList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;

            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME].Caption = "备件名称";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION].Caption = "描述";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE].Caption = "备件类型";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE].Caption = "备件型号";
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT].Caption = "备件单位";

            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grvPartList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME].Visible = false;

            this.grvPartList.BestFitColumns();
        }

        protected override void MapControlsToEntity()
        {
            this.equipmentPartEntity.ClearData();

            switch (State)
            {
                case ControlState.New:
                    this.equipmentPartEntity.PartKey =  CommonUtils.GenerateNewKey(0);

                    this.equipmentPartEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    this.equipmentPartEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    this.equipmentPartEntity.CreateTime = string.Empty;
                    break;
                case ControlState.Edit:
                case ControlState.Delete:
                    this.equipmentPartEntity.PartKey = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY).ToString();
                    this.equipmentPartEntity.PartName = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME).ToString();
                    this.equipmentPartEntity.Description = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION).ToString();
                    this.equipmentPartEntity.PartType = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE).ToString();
                    this.equipmentPartEntity.PartMode = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE).ToString();
                    this.equipmentPartEntity.PartUnit = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT).ToString();

                    this.equipmentPartEntity.Editor = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR).ToString();
                    this.equipmentPartEntity.EditTimeZone = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                    this.equipmentPartEntity.EditTime = this.grvPartList.GetFocusedRowCellValue(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME).ToString();
                    break;
            }

            this.equipmentPartEntity.IsInitializeFinished = true;

            this.equipmentPartEntity.PartName = this.txtPartName.Text.Trim();
            this.equipmentPartEntity.Description = this.txtDescription.Text.Trim();
            this.equipmentPartEntity.PartType = this.txtPartType.Text.Trim();
            this.equipmentPartEntity.PartMode = this.txtPartMode.Text.Trim();
            this.equipmentPartEntity.PartUnit = this.txtPartUnit.Text.Trim();
        }

        protected override void MapEntityToControls()
        {
            switch (State)
            {
                case ControlState.New:
                    //TODO: Refresh Parts Data
                    
                    break;
                case ControlState.Edit:
                    foreach (KeyValuePair<string, DirtyItem> keyValue in this.equipmentPartEntity.DirtyList)
                    {
                        this.grvPartList.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);
                    }

                    this.grvPartList.UpdateCurrentRow();
                    break;
                case ControlState.Delete:
                    this.grvPartList.DeleteRow(this.grvPartList.FocusedRowHandle);

                    this.grvPartList.UpdateCurrentRow();
                    break;
            }

            this.grvPartList.BestFitColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Equipment Parts Data
        /// </summary>
        /// Owner:Andy Gao 2011-08-12 12:37:27
        private void LoadPartsData()
        {
            string msg;

            int pageNo, pageSize, pages, records;

            this.paginationParts.GetPaginationProperties(out pageNo, out pageSize);

            DataTable dataTable = this.equipmentPartEntity.LoadPartsData(this.txtQueryValue.Text.Trim(), pageNo, pageSize, out pages, out records, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                this.paginationParts.SetPaginationProperties(pageNo, pageSize, pages, records);

                this.grdPartList.DataSource = dataTable;
            }
            else
            {
                this.grdPartList.DataSource = EMS_EQUIPMENT_PARTS_FIELDS.CreateDataTable(true);

                MessageService.ShowError(msg);
            }

            this.grvPartList.BestFitColumns();

            State = ControlState.Read;
        }

        #endregion

        #region Component Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadPartsData();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            State = ControlState.New;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.txtPartName.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入备件名称!");

                this.txtPartName.Focus();

                return;
            }

            if (this.txtPartUnit.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage("请输入备件单位!");

                this.txtPartUnit.Focus();

                return;
            }

            MapControlsToEntity();

            if (State == ControlState.New)
            {
                if (this.equipmentPartEntity.Insert())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("保存数据成功!");

                    //State = ControlState.Read;

                    LoadPartsData();
                }
            }
            else if (State == ControlState.Edit)
            {
                if (this.equipmentPartEntity.IsDirty)
                {
                    if (this.equipmentPartEntity.Update())
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
            if (this.grvPartList.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("请选择删除备件!");

                return;
            }

            if (MessageService.AskQuestion("确定删除该备件吗?"))
            {
                State = ControlState.Delete;

                MapControlsToEntity();

                if (this.equipmentPartEntity.Delete())
                {
                    //MapEntityToControls();

                    MessageService.ShowMessage("删除数据成功!");

                    //State = ControlState.Read;

                    LoadPartsData();
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadPartsData();
        }

        private void grdPartList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grvPartList.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                State = ControlState.Edit;
            }
        }

        private void grvPartList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this.txtPartName.Text = this.grvPartList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME).ToString();
                this.txtDescription.Text = this.grvPartList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION).ToString();
                this.txtPartType.Text = this.grvPartList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE).ToString();
                this.txtPartMode.Text = this.grvPartList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE).ToString();
                this.txtPartUnit.Text = this.grvPartList.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT).ToString();
            }
            else
            {
                this.txtPartName.Text = string.Empty;
                this.txtDescription.Text = string.Empty;
                this.txtPartType.Text = string.Empty;
                this.txtPartMode.Text = string.Empty;
                this.txtPartUnit.Text = string.Empty;
            }
        }

        private void paginationParts_DataPaging()
        {
            LoadPartsData();
        }

        #endregion
    }
}
