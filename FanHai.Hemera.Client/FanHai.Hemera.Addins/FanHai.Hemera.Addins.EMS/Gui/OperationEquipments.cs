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
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class OperationEquipments : BaseUserCtrl
    {
        #region Constructor

        public OperationEquipments()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void OperationEquipments_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(OperationEquipments_afterStateChanged);

            State = ControlState.Read;
        }

        private void OperationEquipments_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbEdit.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.btnAddEquipment.Enabled = false;
                    this.btnDelEquipment.Enabled = false;
                    break;
                case ControlState.ReadOnly:
                    this.tsbEdit.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.btnAddEquipment.Enabled = false;
                    this.btnDelEquipment.Enabled = false;
                    break;
                case ControlState.Edit:
                    this.tsbEdit.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.btnAddEquipment.Enabled = true;
                    this.btnDelEquipment.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            #region Repository Controls

            RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

            checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);

            RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();

            textEdit.CustomDisplayText += new CustomDisplayTextEventHandler(textEdit_CustomDisplayText);

            #endregion

            #region Operation Grid

            this.grdViewOperations.Columns.Clear();

            int index = 0;

            GridColumn column = new GridColumn();

            column.Name = "RN";
            column.FieldName = "RN";
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
            column.Visible = false;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS;
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IMAGE_KEY;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IMAGE_KEY;
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION;
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION;
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS;
            column.Visible = true;
            column.VisibleIndex = index++;
            column.ColumnEdit = textEdit;

            this.grdViewOperations.Columns.Add(column);

            column = new GridColumn();

            column.Name = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IS_REWORKABLE;
            column.FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IS_REWORKABLE;
            column.Visible = true;
            column.VisibleIndex = index++;
            column.ColumnEdit = checkEdit;

            this.grdViewOperations.Columns.Add(column);

            #endregion

            #region Equipment Grid

            this.grdViewEquipments.Columns.Clear();

            EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

            index = 0;

            foreach (KeyValuePair<string, FieldProperties> field in equipmentsFields.FIELDS)
            {
                column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;

                this.grdViewEquipments.Columns.Add(column);
            }

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].ColumnEdit = checkEdit;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].ColumnEdit = checkEdit;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].ColumnEdit = checkEdit;

            #endregion
        }

        protected override void InitUIResourcesByCulture()
        {
            GridViewHelper.SetGridView(grdViewOperations);
            GridViewHelper.SetGridView(grdViewEquipments);
            this.tsbEdit.Text = StringParser.Parse("${res:Global.Edit}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            //注释 by peter zhang 工具栏图标从当前项目中获取
            //this.tsbEdit.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Edit");
            //this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");

            //this.lblTitle.Text=StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.Name}");
            //this.grpOperationQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationQuery}");
            this.lblQueryName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationName}");
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");

            this.grpOperationInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationInfo}");
            this.grpEquipmentInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.EquipmentInfo}");

            this.btnAddEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.AddEquipment}");
            this.btnDelEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.DelEquipment}");

            this.grdViewOperations.Columns["RN"].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grdViewOperations.Columns["RN"].Width = 35;

            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY].Visible = false;
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationName}");
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationDescription}");
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IMAGE_KEY].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationImage}");
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationDuration}");
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationVersion}");
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.OperationStatus}");
            this.grdViewOperations.Columns[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_IS_REWORKABLE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.IsReworkable}");

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentName}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION].Caption = StringParser.Parse("${res:Global.Description}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentType}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentCode}");

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentMode}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.MinQuantity}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.MaxQuantity}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0002}");//是否腔体
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.IsMultiChamber}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.IsBatch}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0006}"); //"腔体编号";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0005}"); //"腔体数量";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0011}"); //"设备WPH";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0008}"); //"设备TractTime";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0007}"); //"设备AvTime";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0010}"); //"资产编号";

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME].Visible = false;
        }

        protected override void InitUIAuthoritiesByUser()
        {

        }

        protected override void MapControlsToEntity()
        {

        }

        protected override void MapEntityToControls()
        {

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Operations Data
        /// </summary>
        /// <param name="operationName"></param>
        /// Owner:Andy Gao 2010-08-11 11:19:37
        private void LoadOperationsData(string operationName)
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    int pages;
                    int records;
                    int pageNo = this.paginationOperations.PageNo;
                    int pageSize = this.paginationOperations.PageSize;

                    if (pageNo <= 0)
                    {
                        pageNo = 1;
                    }

                    if (pageSize <= 0)
                    {
                        pageSize = PaginationControl.DEFAULT_PAGESIZE;
                    }

                    resDS = serverFactory.CreateIOperationEngine().GetOperations(reqDS, operationName, pageNo, pageSize, out pages, out records);

                    if (pages > 0 && records > 0)
                    {
                        this.paginationOperations.PageNo = pageNo > pages ? pages : pageNo;
                        this.paginationOperations.PageSize = pageSize;
                        this.paginationOperations.Pages = pages;
                        this.paginationOperations.Records = records;
                    }
                    else
                    {
                        this.paginationOperations.PageNo = 0;
                        this.paginationOperations.PageSize = PaginationControl.DEFAULT_PAGESIZE;
                        this.paginationOperations.Pages = 0;
                        this.paginationOperations.Records = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                BindDataToOperationsGrid(resDS.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to Operations Grid
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2010-08-11 11:20:46
        private void BindDataToOperationsGrid(DataTable dataTable)
        {
            this.grdOperations.MainView = this.grdViewOperations;
            this.grdOperations.DataSource = null;
            this.grdOperations.DataSource = dataTable;
        }

        /// <summary>
        /// Load Operation Equipments Data
        /// </summary>
        /// <param name="operationKey"></param>
        /// Owner:Andy Gao 2010-08-11 13:33:50
        private void LoadOperationEquipmentsData(string operationKey)
        {
            #region Variables

            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIOperationEquipments().GetOperationEquipments(operationKey);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                BindDataToEquipmentsGrid(resDS.Tables[EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to Equipments Grid
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2010-08-11 13:39:32
        private void BindDataToEquipmentsGrid(DataTable dataTable)
        {
            this.grdEquipments.MainView = this.grdViewEquipments;
            this.grdEquipments.DataSource = null;
            this.grdEquipments.DataSource = dataTable;
        }

        /// <summary>
        /// Update Operation Equipments Data
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-08-11 16:05:22
        private bool UpdateOperationEquipmentsData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Change Reasons Data

            if (this.grdEquipments.DataSource != null && this.grdEquipments.DataSource is DataTable)
            {
                DataTable equipmentsDataTable = (DataTable)this.grdEquipments.DataSource;

                DataTable addedDataTable = equipmentsDataTable.GetChanges(DataRowState.Added);
                DataTable modifiedDataTable = equipmentsDataTable.GetChanges(DataRowState.Modified);
                DataTable deletedDataTable = equipmentsDataTable.GetChanges(DataRowState.Deleted);

                if ((addedDataTable != null && addedDataTable.Rows.Count > 0) ||
                    (modifiedDataTable != null && modifiedDataTable.Rows.Count > 0) ||
                    (deletedDataTable != null && deletedDataTable.Rows.Count > 0))
                {
                    equipmentsDataTable.TableName = EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME;

                    reqDS.Tables.Add(equipmentsDataTable.Copy());
                }
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.M0002}", "${res:Global.SystemInfo}");

                    return false;
                }
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIOperationEquipments().UpdateOperationEquipments(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                return true;
            }
            else
            {
                MessageService.ShowError(returnMsg);

                return false;
            }

            #endregion
        }

        #endregion

        #region Controls Events

        private void textEdit_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        {
            string value = string.Empty;

            if (e.Value != null)
            {
                value = e.Value.ToString();
            }
            else
            {
                value = "0";
            }

            switch (value)
            {
                case "0":
                    e.DisplayText = "InActive";
                    break;
                case "1":
                    e.DisplayText = "Active";
                    break;
                case "2":
                    e.DisplayText = "Archive";
                    break;
                default:
                    e.DisplayText = "InActive";
                    break;
            }
        }

        private void paginationOperations_DataPaging()
        {
            LoadOperationsData(this.txtQueryValue.Text.Trim());

            State = ControlState.Read;

            grdViewOperations_FocusedRowChanged(this.grdViewOperations, new FocusedRowChangedEventArgs(-1, this.grdViewOperations.FocusedRowHandle));
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (this.grdViewOperations.RowCount > 0)
            {
                State = ControlState.Edit;
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (UpdateOperationEquipmentsData())
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.M0003}", "${res:Global.SystemInfo}");

                if (this.grdEquipments.DataSource != null && this.grdEquipments.DataSource is DataTable)
                {
                    ((DataTable)this.grdEquipments.DataSource).AcceptChanges();
                }

                State = ControlState.Read;
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            if (this.grdEquipments.DataSource != null && this.grdEquipments.DataSource is DataTable)
            {
                ((DataTable)this.grdEquipments.DataSource).RejectChanges();
            }

            State = ControlState.Read;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadOperationsData(this.txtQueryValue.Text.Trim());

            State = ControlState.Read;

            grdViewOperations_FocusedRowChanged(this.grdViewOperations, new FocusedRowChangedEventArgs(-1, this.grdViewOperations.FocusedRowHandle));
        }

        private void grdViewOperations_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                string operationKey = this.grdViewOperations.GetRowCellValue(e.FocusedRowHandle, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY).ToString();

                if (!string.IsNullOrEmpty(operationKey))
                {
                    LoadOperationEquipmentsData(operationKey);
                }
            }
            else
            {
                BindDataToEquipmentsGrid(null);
            }
        }

        private void btnAddEquipment_Click(object sender, EventArgs e)
        {
            if (this.grdViewOperations.GetFocusedRow() != null)
            {
                string operationKey = this.grdViewOperations.GetFocusedRowCellValue(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY).ToString();

                if (!string.IsNullOrEmpty(operationKey))
                {
                    EquipmentQuery equipmentQuery = new EquipmentQuery();

                    if (equipmentQuery.ShowDialog() == DialogResult.OK && equipmentQuery.SelectedEquipmentData != null)
                    {
                        if (this.grdEquipments.DataSource != null && this.grdEquipments.DataSource is DataTable)
                        {
                            DataTable equipmentDataTable = (DataTable)this.grdEquipments.DataSource;

                            string equipmentKey = equipmentQuery.SelectedEquipmentData[0].ToString();

                            DataRow[] findDataRows = equipmentDataTable.Select(string.Format(EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_EQUIPMENT_KEY + " = '{0}'", equipmentKey));

                            if (findDataRows.Length > 0)
                            {
                                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.OperationEquipments.M0001}", "${res:Global.SystemInfo}");
                                return;
                            }

                            DataRow addDataRow = equipmentDataTable.Rows.Add(equipmentQuery.SelectedEquipmentData);

                            addDataRow[EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_OPERATION_EQUIPMENT_KEY] =  CommonUtils.GenerateNewKey(0);
                            addDataRow[EMS_OPERATION_EQUIPMENT_FIELDS.FIELD_OPERATION_KEY] = operationKey;
                        }
                    }
                }
            }
        }

        private void btnDelEquipment_Click(object sender, EventArgs e)
        {
            if (this.grdViewEquipments.GetFocusedRow() != null)
            {
                this.grdViewEquipments.DeleteRow(this.grdViewEquipments.FocusedRowHandle);
            }
        }
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion
    }
}
