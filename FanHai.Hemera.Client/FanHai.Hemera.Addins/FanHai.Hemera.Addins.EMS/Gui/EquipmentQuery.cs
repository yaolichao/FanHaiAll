using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using DXUtils=DevExpress.Utils;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment Query GUI
    /// </summary>
    /// Owner:Andy Gao 2010-08-11 14:58:52
    public partial class EquipmentQuery : BaseDialog
    {
        #region Public Properties

        public object[] SelectedEquipmentData = null;

        #endregion

        #region Constructor

        public EquipmentQuery()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentQuery.Name}"))
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void EquipmentQuery_Load(object sender, EventArgs e)
        {
            InitUIControls();
            InitUIResourcesByCulture();
        }

        #endregion

        #region Private Methods

        private void InitUIControls()
        {
            #region Equipment Grid

            this.grdViewEquipments.Columns.Clear();

            EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

            int index = 0;

            GridColumn column = new GridColumn();

            column.Name = "RN";
            column.FieldName = "RN";
            column.Visible = true;
            column.VisibleIndex = index++;
           
            this.grdViewEquipments.Columns.Add(column);
            column.AppearanceHeader.TextOptions.HAlignment = DXUtils.HorzAlignment.Center;

            foreach (KeyValuePair<string, FieldProperties> field in equipmentsFields.FIELDS)
            {
                column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;
                
                this.grdViewEquipments.Columns.Add(column);
                column.AppearanceHeader.TextOptions.HAlignment = DXUtils.HorzAlignment.Center;
            }

            RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

            checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].ColumnEdit = checkEdit;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].ColumnEdit = checkEdit;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].ColumnEdit = checkEdit;

            #endregion
        }

        private void InitUIResourcesByCulture()
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");

            this.grpEquipmentQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentQuery}");
            this.lblQueryName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentName}");
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.grpEquipmentList.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentQuery.EquipmentList}");

            this.grdViewEquipments.Columns["RN"].Caption = StringParser.Parse("${res:Global.RowNumber}");
            //this.grdViewEquipments.Columns["RN"].Width = 35;

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
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].Caption = "是否腔体";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Caption=StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.IsMultiChamber}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.IsBatch}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].Caption = "腔体编号";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Caption = "腔体数量";

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME].Visible = false;
        }

        /// <summary>
        /// Load Equipment Data
        /// </summary>
        /// <param name="equipmentName"></param>
        /// Owner:Andy Gao 2010-08-11 14:54:17
        private void LoadEquipmentsData(string equipmentName)
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
                    int pages;
                    int records;
                    int pageNo = this.paginationEquipments.PageNo;
                    int pageSize = this.paginationEquipments.PageSize;

                    if (pageNo <= 0)
                    {
                        pageNo = 1;
                    }

                    if (pageSize <= 0)
                    {
                        pageSize = PaginationControl.DEFAULT_PAGESIZE;
                    }

                    resDS = serverFactory.CreateIEquipments().GetAllChildEquipments(equipmentName, pageNo, pageSize, out pages, out records);

                    if (pages > 0 && records > 0)
                    {
                        this.paginationEquipments.PageNo = pageNo > pages ? pages : pageNo;
                        this.paginationEquipments.PageSize = pageSize;
                        this.paginationEquipments.Pages = pages;
                        this.paginationEquipments.Records = records;
                    }
                    else
                    {
                        this.paginationEquipments.PageNo = 0;
                        this.paginationEquipments.PageSize = PaginationControl.DEFAULT_PAGESIZE;
                        this.paginationEquipments.Pages = 0;
                        this.paginationEquipments.Records = 0;
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
                BindDataToEquipmentsGrid(resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME]);
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
        /// Owner:Andy Gao 2010-08-11 14:54:25
        private void BindDataToEquipmentsGrid(DataTable dataTable)
        {
            this.grdEquipments.MainView = this.grdViewEquipments;
            this.grdEquipments.DataSource = null;
            this.grdEquipments.DataSource = dataTable;
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = grdViewEquipments.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                DataRow selectedEquipmentDataRow = this.grdViewEquipments.GetDataRow(rowHandle);

                this.SelectedEquipmentData = new object[selectedEquipmentDataRow.ItemArray.Length - 1];

                for (int i = 0; i < this.SelectedEquipmentData.Length; i++)
                {
                    this.SelectedEquipmentData.SetValue(selectedEquipmentDataRow.ItemArray[i + 1], i);
                }

                return true;
            }
            return false;
        }

        #endregion

        #region Component Events

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadEquipmentsData(this.txtQueryValue.Text.Trim());
        }

        private void paginationEquipments_DataPaging()
        {
            LoadEquipmentsData(this.txtQueryValue.Text.Trim());
        }

        private void grdEquipments_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
