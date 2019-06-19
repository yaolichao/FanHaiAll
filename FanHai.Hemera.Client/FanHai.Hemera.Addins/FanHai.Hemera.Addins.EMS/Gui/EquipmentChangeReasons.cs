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
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Share.Interface;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment Change Reasons GUI
    /// </summary>
    /// Owner:Andy Gao 2010-07-19 15:45:57
    public partial class EquipmentChangeReasons : BaseUserCtrl
    {
        #region Constructor

        public EquipmentChangeReasons()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentChangeReasons_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(EquipmentChangeReasons_afterStateChanged);

            LoadEquipmentChangeStatesData();

            State = ControlState.Read;
        }

        private void EquipmentChangeReasons_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbEdit.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.btnAddReason.Enabled = false;
                    this.btnDelReason.Enabled = false;
                    this.grdViewChangeStates.OptionsBehavior.Editable = false;
                    this.grdViewChangeReasons.OptionsBehavior.Editable = false;
                    break;
                case ControlState.ReadOnly:
                    this.tsbEdit.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.btnAddReason.Enabled = false;
                    this.btnDelReason.Enabled = false;
                    this.grdViewChangeStates.OptionsBehavior.Editable = false;
                    this.grdViewChangeReasons.OptionsBehavior.Editable = false;
                    break;
                case ControlState.Edit:
                    this.tsbEdit.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.btnAddReason.Enabled = true;
                    this.btnDelReason.Enabled = true;
                    this.grdViewChangeStates.OptionsBehavior.Editable = true;
                    this.grdViewChangeReasons.OptionsBehavior.Editable = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            int index = 0;

            this.grdViewChangeStates.Columns.Clear();

            EMS_EQUIPMENT_CHANGE_STATES_FIELDS equipmentChangeStatesFields = new EMS_EQUIPMENT_CHANGE_STATES_FIELDS();

            foreach (KeyValuePair<string, FieldProperties> field in equipmentChangeStatesFields.FIELDS)
            {
                GridColumn column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;

                this.grdViewChangeStates.Columns.Add(column);
            }

            this.grdViewChangeReasons.Columns.Clear();

            EMS_EQUIPMENT_CHANGE_REASONS_FIELDS equipmentChangeReasonsFields = new EMS_EQUIPMENT_CHANGE_REASONS_FIELDS();

            index = 0;

            foreach (KeyValuePair<string, FieldProperties> field in equipmentChangeReasonsFields.FIELDS)
            {
                GridColumn column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;

                this.grdViewChangeReasons.Columns.Add(column);
            }
        }

        protected override void InitUIResourcesByCulture()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbEdit.Text = StringParser.Parse("${res:Global.Edit}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");

            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //this.tsbEdit.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Edit");
            //this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.Name}");
            //this.grpChangeStates.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.Name}");
            this.grpChangeReasons.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.Name}");

            this.btnAddReason.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.AddReason}");
            this.btnDelReason.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.DeleteReason}");

            this.grdViewChangeStates.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grdViewChangeStates.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.ChangeStateName}");
            this.grdViewChangeStates.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].OptionsColumn.AllowEdit = false;
            this.grdViewChangeStates.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION].Caption = StringParser.Parse("${res:Global.Description}");
            this.grdViewChangeStates.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].Visible = false;
            this.grdViewChangeStates.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].Visible = false;

            this.grdViewChangeReasons.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
            this.grdViewChangeReasons.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.ChangeReasonName}");
            this.grdViewChangeReasons.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_DESCRIPTION].Caption = StringParser.Parse("${res:Global.Description}");
            this.grdViewChangeReasons.Columns[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
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
        /// Load Equipment Change States Data
        /// </summary>
        /// Owner:Andy Gao 2010-07-16 10:02:36
        private void LoadEquipmentChangeStatesData()
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
                    resDS = serverFactory.CreateIEquipmentChangeStates().GetEquipmentChangeStates(reqDS);
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
                BindDataToChangeStatesGrid(resDS.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        private void LoadEquipmentChangeReasonsData(string equipmentChangeStateKey)
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Input Parameters

            if (!string.IsNullOrEmpty(equipmentChangeStateKey))
            {
                DataTable inputParamDataTable = PARAMETERS_INPUT.CreateDataTable();

                object inputKey = DBNull.Value;
                object inputEditor = DBNull.Value;
                object inputEditTime = DBNull.Value;

                inputKey = equipmentChangeStateKey;

                inputParamDataTable.Rows.Add(new object[] { inputKey, inputEditor, inputEditTime });

                inputParamDataTable.AcceptChanges();

                reqDS.Tables.Add(inputParamDataTable);
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentChangeReasons().GetEquipmentChangeReasons(reqDS);
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
                BindDataToChangeReasonsGrid(resDS.Tables[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Update Equipment Change States Data For Update The Description
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-20 15:12:47
        private bool UpdateEquipmentChangeStatesData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Change States Data

            if (this.grdChangeStates.DataSource != null && this.grdChangeStates.DataSource is DataTable)
            {
                DataTable equipmentChangeStatesDataTalbe = (DataTable)this.grdChangeStates.DataSource;

                DataTable modifiedDataTable = equipmentChangeStatesDataTalbe.GetChanges(DataRowState.Modified);

                if (modifiedDataTable != null && modifiedDataTable.Rows.Count > 0)
                {
                    equipmentChangeStatesDataTalbe.TableName = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME;

                    reqDS.Tables.Add(equipmentChangeStatesDataTalbe.Copy());
                }
                else
                {
                    return true;
                }
            }

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentChangeStates().UpdateEquipmentChangeStates(reqDS);
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

        /// <summary>
        /// Update Equipment Change Reasons Data
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-20 14:52:52
        private bool UpdateEquipmentChangeReasonsData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Equipment Change Reasons Data

            if (this.grdChangeReasons.DataSource != null && this.grdChangeReasons.DataSource is DataTable)
            {
                DataTable equipmentChangeReasonsDataTable = (DataTable)this.grdChangeReasons.DataSource;

                DataTable addedDataTable = equipmentChangeReasonsDataTable.GetChanges(DataRowState.Added);
                DataTable modifiedDataTable = equipmentChangeReasonsDataTable.GetChanges(DataRowState.Modified);
                DataTable deletedDataTable = equipmentChangeReasonsDataTable.GetChanges(DataRowState.Deleted);

                if ((addedDataTable != null && addedDataTable.Rows.Count > 0) ||
                    (modifiedDataTable != null && modifiedDataTable.Rows.Count > 0) ||
                    (deletedDataTable != null && deletedDataTable.Rows.Count > 0))
                {
                    equipmentChangeReasonsDataTable.TableName = EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.DATABASE_TABLE_NAME;

                    reqDS.Tables.Add(equipmentChangeReasonsDataTable.Copy());
                }
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0006}", "${res:Global.InformationText}");

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
                    resDS = serverFactory.CreateIEquipmentChangeReasons().UpdateEquipmentChangeReasons(reqDS);
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

        /// <summary>
        /// Bind Data To Change States Grid
        /// </summary>
        /// <param name="changeStatesDataTable"></param>
        private void BindDataToChangeStatesGrid(DataTable changeStatesDataTable)
        {
             this.grdChangeStates.MainView = this.grdViewChangeStates;
             this.grdChangeStates.DataSource = null;
             this.grdChangeStates.DataSource = changeStatesDataTable;
        }

        /// <summary>
        /// Bind Data To Change Reasons Grid
        /// </summary>
        /// <param name="changeReasonsDataTable"></param>
        /// Owner:Andy Gao 2010-07-20 08:40:28
        private void BindDataToChangeReasonsGrid(DataTable changeReasonsDataTable)
        {
            this.grdChangeReasons.MainView = this.grdViewChangeReasons;
            this.grdChangeReasons.DataSource = null;
            this.grdChangeReasons.DataSource = changeReasonsDataTable;
        }

        #endregion

        #region Controls Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadEquipmentChangeStatesData();

            State = ControlState.Read;

            grdViewChangeStates_FocusedRowChanged(this.grdViewChangeStates, new FocusedRowChangedEventArgs(-1, this.grdViewChangeStates.FocusedRowHandle));
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (this.grdViewChangeStates.RowCount > 0)
            {
                State = ControlState.Edit;
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.grdViewChangeStates.FocusedColumn.FieldName == EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION 
                && this.grdViewChangeStates.State == GridState.Editing 
                && this.grdViewChangeStates.IsEditorFocused 
                && this.grdViewChangeStates.EditingValueModified)
            {
                this.grdViewChangeStates.SetFocusedRowCellValue(this.grdViewChangeStates.FocusedColumn, this.grdViewChangeStates.EditingValue);
            }

            if (this.grdViewChangeStates.UpdateCurrentRow() && UpdateEquipmentChangeStatesData())
            {
                if (this.grdChangeStates.DataSource != null && this.grdChangeStates.DataSource is DataTable)
                {
                    ((DataTable)this.grdChangeStates.DataSource).AcceptChanges();
                }

                if ((this.grdViewChangeReasons.FocusedColumn.FieldName == EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME || this.grdViewChangeReasons.FocusedColumn.FieldName == EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_DESCRIPTION)
                    && this.grdViewChangeReasons.State == GridState.Editing
                    && this.grdViewChangeReasons.IsEditorFocused
                    && this.grdViewChangeReasons.EditingValueModified)
                {
                    this.grdViewChangeReasons.SetFocusedRowCellValue(this.grdViewChangeReasons.FocusedColumn, this.grdViewChangeReasons.EditingValue);
                }

                if (this.grdViewChangeReasons.UpdateCurrentRow() && UpdateEquipmentChangeReasonsData())
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0005}", "${res:Global.InformationText}");

                    if (this.grdChangeReasons.DataSource != null && this.grdChangeReasons.DataSource is DataTable)
                    {
                        ((DataTable)this.grdChangeReasons.DataSource).AcceptChanges();
                    }

                    State = ControlState.Read;
                }
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            if (this.grdChangeStates.DataSource != null && this.grdChangeStates.DataSource is DataTable)
            {
                ((DataTable)this.grdChangeStates.DataSource).RejectChanges();
            }

            if (this.grdChangeReasons.DataSource != null && this.grdChangeReasons.DataSource is DataTable)
            {
                ((DataTable)this.grdChangeReasons.DataSource).RejectChanges();
            }

            State = ControlState.Read;
        }

        private void grdViewChangeStates_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                string equipmentChangeStateKey = this.grdViewChangeStates.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();

                if (!string.IsNullOrEmpty(equipmentChangeStateKey))
                {
                    LoadEquipmentChangeReasonsData(equipmentChangeStateKey);
                }
            }
            else
            {
                BindDataToChangeReasonsGrid(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.CreateDataTable());
            }
        }

        private void btnAddReason_Click(object sender, EventArgs e)
        {
            if (this.grdViewChangeStates.GetFocusedRow() != null)
            {
                this.grdViewChangeReasons.AddNewRow();

                this.grdViewChangeReasons.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY, CommonUtils.GenerateNewKey(0));

                string equipmentChangeStateKey = this.grdViewChangeStates.GetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY).ToString();

                this.grdViewChangeReasons.SetFocusedRowCellValue(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY, equipmentChangeStateKey);
            }
        }

        private void btnDelReason_Click(object sender, EventArgs e)
        {
            if (this.grdViewChangeReasons.GetFocusedRow() != null)
            {
                this.grdViewChangeReasons.DeleteRow(this.grdViewChangeReasons.FocusedRowHandle);
            }
        }

        private void grdViewChangeStates_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            if (e.Row != null && e.Row is DataRowView)
            {
                DataRowView dataRowView = e.Row as DataRowView;

                string desc = dataRowView[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_DESCRIPTION].ToString();

                if (desc.Length > 150)
                {
                    e.ErrorText = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0001}");
                    e.Valid = false;
                }
            }
        }

        private void grdViewChangeReasons_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            if (e.Row != null && e.Row is DataRowView)
            {
                DataRowView dataRowView = e.Row as DataRowView;

                string reasonName = dataRowView[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME].ToString();
                string desc = dataRowView[EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_DESCRIPTION].ToString();

                if (string.IsNullOrEmpty(reasonName))
                {
                    e.ErrorText = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0002}");
                    e.Valid = false;
                }

                if (reasonName.Length > 50)
                {
                    e.ErrorText = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0003}");
                    e.Valid = false;
                }

                bool isReasonNameExistent = false;

                for (int i = 0; i < this.grdViewChangeReasons.RowCount; i++)
                {
                    if (i != e.RowHandle)
                    {
                        object otherReasonName = this.grdViewChangeReasons.GetRowCellValue(i, EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);

                        if (otherReasonName != null)
                        {
                            if (reasonName == otherReasonName.ToString())
                            {
                                isReasonNameExistent = true;

                                break;
                            }
                        }
                    }
                }

                if (isReasonNameExistent)
                {
                    e.ErrorText = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0004}");
                    e.Valid = false;
                }

                if (desc.Length > 150)
                {
                    e.ErrorText = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeReasons.M0001}");
                    e.Valid = false;
                }
            }
        }

        #endregion
    }
}
